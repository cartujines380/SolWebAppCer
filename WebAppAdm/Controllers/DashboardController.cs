using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class DashboardController : Controller
    {
        

        public ActionResult frmProveedores()
        {
            return View();
        }

        public ActionResult frmLicitaciones()
        {
            return View();
        }
    }
}
