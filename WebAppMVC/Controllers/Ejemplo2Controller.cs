using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class Ejemplo2Controller : Controller
    {
        // GET: Ejemplo2
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Ordenes()
        {
            return View();
        }

        public ActionResult Prueba()
        {
            return View();
        }

    }
}