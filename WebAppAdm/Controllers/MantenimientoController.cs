using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        public ActionResult frmCatalogos()
        {
            return View();
        }

        public ActionResult frmMantDocumentos()
        {
            return View();
        }
    }
}