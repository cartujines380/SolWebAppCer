using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSeguridad.Controllers
{
    public class FacturacionController : Controller
    {

        public ActionResult frmSelPedidos()
        {
            return View();
        }

        public ActionResult frmSelFacturas()
        {
            return View();
        }

        public ActionResult frmControlFacturas()
        {
            return View();
        }
    }
}