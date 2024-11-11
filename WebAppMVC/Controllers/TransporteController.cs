using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class TransporteController : Controller
    {
        // GET: Transporte
        public ActionResult frmBandejaChoferes()
        {
            return View();
        }

        public ActionResult frmActaRecepcion()
        {
            return View();
        }

        public ActionResult frmConsolidacionPri()
        {
            return View();
        }
        public ActionResult frmBandejaChoferesAdmi()
        {
            return View();
        }
        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult frmConsolidacion()
        {
            return View();
        }
        public ActionResult frmBandejaVehiculo()
        {
            return View();
        }
        public ActionResult frmBandejaVehiculoAdmin()
        {
            return View();
        }

        public ActionResult frmConsolidacionPedidos()
        {
            return View();
        }

        public ActionResult frmSolicitudCita()
        {
            return View();
        }

        public ActionResult frmAprobacionCitas()
        {
            return View();
        }


        public ActionResult frmAprobacionCitasProveedor()
        {
            return View();

        }
        public ActionResult frmCitaRapida()
        {
            return View();
        }

        public ActionResult RPTTabularCitas()
        {
            return View();
        }

        public ActionResult RPTMercaderiaRecibirProveedor()
        {
            return View();
        }
        public ActionResult frmoffline()
        {
            return View();
        }
    }
}