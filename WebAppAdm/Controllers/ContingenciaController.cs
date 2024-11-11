using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class ContingenciaController : Controller
    {
        // GET: Pantalla de contingencias para envío de pedidos a los proveedores para uso de los empleados de El Rosado
        public ActionResult frmConsEnvPedidos()
        {
            return View();
        }
    }
}