using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using WebApplication3.Models;

namespace WebApplication3.Filter
{
    public class IgnoreResultAttribute : Attribute
    {
    }
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var result = new ResultViewModel
            {
                success = false,
                msg = actionExecutedContext.Exception.Message
            };

            //var result = Newtonsoft.Json.JsonConvert.SerializeObject(new ResultViewModel
            //{
            //    success = false,
            //    msg = actionExecutedContext.Exception.Message
            //});

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(
                System.Net.HttpStatusCode.ExpectationFailed,
                result, System.Net.Http.Formatting.JsonMediaTypeFormatter.DefaultMediaType);
        }
    }
    public class ResultAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                return;
            }

            var ignoreResult1 = actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<IgnoreResultAttribute>().FirstOrDefault();
            var ignoreResult2 = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<IgnoreResultAttribute>().FirstOrDefault();
            if (ignoreResult1 != null || ignoreResult2 != null)
            {
                return;
            }

            var objectContent = actionExecutedContext.Response.Content as ObjectContent;

            var data = objectContent?.Value;

            var result = new ResultViewModel
            {
                success = true,
                data = data
            };

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(
                System.Net.HttpStatusCode.OK,
                result, System.Net.Http.Formatting.JsonMediaTypeFormatter.DefaultMediaType);
        }
    }
}