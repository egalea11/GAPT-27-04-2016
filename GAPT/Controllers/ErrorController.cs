using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAPT.Controllers
{
        public class ErrorController : Controller
        {
            public ViewResult PageNotFound()
            {
                Response.StatusCode = 404; 
                return View();
            }
        }
}