using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class ResultViewModel
    {
        public bool success { get; set; }

        public string msg { get; set; }

        public object data { get; set; }
    }
}