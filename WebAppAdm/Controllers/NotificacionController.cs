using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class NotificacionController : Controller
    {
        // GET: Transporte
        public ActionResult frmBandejaNotificaciones()
        {
            return View();
        }
        public ActionResult frmAprobacionNotificaciones()
        {
            return View();
        }
        
        public ActionResult frmEnvioNotificacion()
        {
            return View();
        }
  
        
    }
}