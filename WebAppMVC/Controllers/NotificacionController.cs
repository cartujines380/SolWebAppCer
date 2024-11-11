using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class NotificacionController : Controller
    {
      
       
      
        public ActionResult frmVisualizaNotificaciones()
        {
            return View();
        }
     
        public ActionResult frmBandejaNotificacionesProv()
        {
            return View();
        }

       
        
    }
}