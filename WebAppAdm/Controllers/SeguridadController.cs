using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSeguridad.Controllers
{
    public class SeguridadController : Controller
    {

        public ActionResult frmBandejaUsrAdmin()
        {
            return View();
        }
        public ActionResult frmCompradores()
        {
            return View();
        }
        public ActionResult frmArchivoP12()
        {
            return View();
        }
        public ActionResult frmArchivoPDF()
        {
            return View();
        }
        //J. Navarrete 12-01-2016
        public ActionResult frmGestionUsuarios()
        {
            return View();
        }
        //C. López 04-05-2016
        public ActionResult frmGestionLineaNeg()
        {
            return View();
        }

        public ActionResult frmGestionLineaNegProveedor()
        {
            return View();
        }
        public ActionResult frmToleranciaPedidos()
        {
            return View();
        }

        #region RFD0-2022-155: Módulo de seguridad

        public ActionResult frmReporteRol()
        {
            ViewBag.Opcion = "ReporteRol";
            return View("frmReporteRoles");
        }

        public ActionResult frmReporteAccesoRol()
        {
            ViewBag.Opcion = "AccesoRol";
            return View("frmReporteRoles");
        }

        public ActionResult frmReporteAccesoUsuario()
        {
            ViewBag.Opcion = "AccesoUsuario";
            return View("frmReporteRoles");
        }

        #endregion

    }
}