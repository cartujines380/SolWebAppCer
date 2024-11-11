using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class PagosController:Controller
    {
        public ActionResult frmBandejaPagos()
        {
            return View();
        }
    }
}