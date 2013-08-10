using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC4ManyToMany.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = @"Your course registration system. Use the ""User Profile Page"" below to add users to the system.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Course registration system.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact me using the details here.";

            return View();
        }
    }
}
