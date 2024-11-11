using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class ReporteController : Controller
    {
        public ActionResult frmReporteUsuarioNoIngreso()
        {
            return View();
        }

        public ActionResult frmReporteProveedorNoCompra()
        {
            return View();
        }

        public ActionResult frmReporteProveedorNoSolicitud()
        {
            return View();
        }
        public ActionResult frmReporteLogComunicado()
        {
            return View();
        }
    }
}
