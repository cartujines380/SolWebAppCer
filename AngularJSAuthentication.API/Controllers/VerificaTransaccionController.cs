using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Models;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Xml;
using System.Security.Claims;
using clibProveedores.Models;
using clibProveedores;
using clibSeguridadCR;
using System.Xml.Linq;
using System.IO;
using Renci.SshNet;

using AngularJSAuthentication.API.WCFEnvioCorreo;
using System.Net.Http.Headers;
using System.Drawing;
using AngularJSAuthentication.API.Handlers;
using System.Text;
namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/VerificaTransaccion")]
    public class VerificaTransaccionController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("Verificar")]
        [HttpGet]
        public Boolean Verificar(string transaccion)
        {

            clibLogger.clsLogger _objLogServicio = new clibLogger.clsLogger();
            _objLogServicio.Graba_Log_Info("INI >>  API - VerificarTransaccion " + DateTime.Now);

            Boolean ds = false;
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                _objLogServicio.Graba_Log_Info("INFO >>  API - VerificarTransaccion " + transaccion);

                ds = objEjecucion.EjecucionVerificar(transaccion,Convert.ToInt32(transaccion), 1);
            }
            catch (Exception ex)
            {
                ds = false;
            }
            _objLogServicio.Graba_Log_Info("FIN >>  API - VerificarTransaccion " + DateTime.Now);

            return ds;
        }
    }
}
