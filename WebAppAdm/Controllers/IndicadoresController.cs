using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class IndicadoresController : Controller
    {
        // GET: Indicadores
        public ActionResult frmResumen()
        {
            return View();
        }
    }
}