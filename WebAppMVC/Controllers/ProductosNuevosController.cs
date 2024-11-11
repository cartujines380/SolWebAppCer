using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class ProductosNuevosController : Controller
    {
        // GET: ProductosNuevos
        public ActionResult frmProductosNuevos()
        {
            return View();
        }

        public ActionResult frmIngresaSolicitud()
        {
            return View();
        }
    }
}