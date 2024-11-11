using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class ComunicadoController : Controller
    {
        // GET: Transporte
        public ActionResult frmLineaNegocios()
        {
            return View();
        }
        public ActionResult frmQuienesSomos()
        {
            return View();
        }
        public ActionResult frmInformacion()
        {
            return View();
        }
        
    }
}