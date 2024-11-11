using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class EtiquetasController : Controller
    {
        // GET: Etiquetas
        public ActionResult frmConsEtiquetas()
        {
            return View();
        }

        public ActionResult frmConsPedEti()
        {
            return View();
        }

        public ActionResult frmConEtiquetaTipo()
        {
            return View();
        }
    }
}