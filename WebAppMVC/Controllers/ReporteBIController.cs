using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PowerBIEmbedded_AppOwnsData.Controllers
{
    public class ReporteBIController : Controller
    {
        public ActionResult EmbedReport()
        {
            return View();
        }
        public ActionResult EmbedPremium()
        {
            return Redirect(System.Configuration.ConfigurationManager.AppSettings.Get("urlPremium"));
        }

    }
}