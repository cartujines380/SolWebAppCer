using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class ProveedorController : Controller
    {
        //
        // GET: /Proveedor/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult frmIngresoProveedor()
        {
            return View();
        }

        public ActionResult frmIngPreCalifica()
        {
            return View();
        }

        public ActionResult frmBandejaPrecalifica()
        {
            return View();
        }
        public ActionResult frmBandejaSolicitud()
        {
            return View();
        }

        public ActionResult frmSolictud()
        {
            return View();
        }
        
        public ActionResult frmSolicitudModificacioProveedor()
        {
            return View();
        }

        public ActionResult frmConsultaProveedorSol()
        {
            return View();
        }

    }
}