using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class RequerimientoController : Controller
    {
        // GET: Requerimiento
        public ActionResult frmReqConcurso()
        {
            return View();
        }

        public ActionResult frmBandejaRequerimiento()
        {
            return View();
        }
    }
}