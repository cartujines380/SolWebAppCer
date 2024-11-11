using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult frmAsignarCita()
        {
            return View();
        }
        

        public ActionResult frmBandejaChoferesAdmi()
        {
            return View();
        }
        public ActionResult frmConsolidacionPedidosAdmin()
        {
            return View();
        }
        
        public ActionResult Profile()
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
        
        public ActionResult frmCitaRapida()
        {
            return View();
        }

        public ActionResult RPTTabularCitas()
        {
            return View();
        }

        public ActionResult RPTMercaderiaRecibir()
        {
            return View();
        }
        public ActionResult frmoffline()
        {
            return View();
        }
    }
}