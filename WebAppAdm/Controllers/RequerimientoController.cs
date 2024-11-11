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

        public ActionResult frmReqConcursoBandeja()
        {
            return View();
        }
        public ActionResult frmReqEmpresa()
        {
            return View();
        }
        public ActionResult frmReqConcurso()
        {
            return View();
        }
        public ActionResult frmReqAdjudicacion()
        {
            return View();
        }

        public ActionResult frmReqRequisito()
        {
            return View();
        }
    }
}