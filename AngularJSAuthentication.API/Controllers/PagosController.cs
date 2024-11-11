using AngularJSAuthentication.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace clibProveedores
{
    [Authorize]
    [RoutePrefix("api/Pagos")]
    public class PagosController : ApiController
    {
        Logger.Logger log = new Logger.Logger();

        //Consulta lista de pagos en BD
        [ActionName("consultaPagos")]
        [HttpGet]
        public FormResponsePagos getConsultaPagos(String Identificacion, String Tipo)
        {
            if (Identificacion == "undefined")
            {
                Identificacion = null;
            }

            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XElement("Pagos",
                                     new XAttribute("Identificacion", Identificacion != null ? Identificacion : ""),
                                     new XAttribute("Tipo", Tipo != null ? Tipo : ""))));
            log.FilePath = p_Log;
            log.WriteMensaje("getConsultaPagos: \n" + wresulFactList.ToString());

            ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 414, 1);
            FormResponsePagos FormResponse = new FormResponsePagos();
            FormResponse.success = true;
            
            try
            {
                List<Pagos> Retorno = new List<Pagos>();
                Retorno = (from reg in ds.Tables[0].AsEnumerable()
                           select new Pagos
                           {
                               Num = reg.Field<Int64>("Num"),
                               Identificacion = reg.Field<string>("Identificacion"),
                               NomComercial = reg.Field<string>("NomComercial"),
                               CodProveedorAx = reg.Field<string>("CodProveedorAx"),
                               Factura = reg.Field<string>("Factura"),
                               FormaPago = reg.Field<string>("FormaPago"),
                               FechaPago = reg.Field<DateTime>("FechaPago").ToString("yyyy-MM-dd"),
                               Valor = String.Format("{0:C}", reg.Field<Decimal>("Valor")),
                               Detalle = reg.Field<string>("Detalle"),
                               
                           }).ToList<Pagos>();
                FormResponse.root.Add(Retorno);
            }
            catch (Exception e)
            {
                log.FilePath = p_Log;
                log.WriteMensaje("Error getConsultaPagos: " + e.Message.ToString());

                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
            }
            return FormResponse;
        }

        //Consulta lista de pagos en BD
        [ActionName("consultaPagosFechas")]
        [HttpGet]
        public FormResponsePagos getConsultaPagosFechas(String Identificacion, String Tipo, String FecDesde, String FecHasta)
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();

            if (FecDesde == "undefined")
            {
                FecDesde = null;
            }
            if (FecHasta == "undefined")
            {
                FecHasta = null;
            }
            if (Identificacion == "undefined")
            {
                Identificacion = null;
            }

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XElement("Pagos",
                                     new XAttribute("Identificacion", Identificacion != null ? Identificacion : ""),
                                     new XAttribute("Tipo", Tipo != null ? Tipo : ""),
                                     new XAttribute("FechaDesde", FecDesde != null ? FecDesde : ""),
                                     new XAttribute("FechaHasta", FecHasta != null ? FecHasta : ""))));

            log.FilePath = p_Log;
            log.WriteMensaje("getConsultaPagosFechas: \n" + wresulFactList.ToString());

            ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 414, 1);
            FormResponsePagos FormResponse = new FormResponsePagos();
            FormResponse.success = true;

            try
            {
                List<Pagos> Retorno = new List<Pagos>();
                Retorno = (from reg in ds.Tables[0].AsEnumerable()
                           select new Pagos
                           {
                               Num = reg.Field<Int64>("Num"),
                               Identificacion = reg.Field<string>("Identificacion"),
                               NomComercial = reg.Field<string>("NomComercial"),
                               CodProveedorAx = reg.Field<string>("CodProveedorAx"),
                               Factura = reg.Field<string>("Factura"),
                               FormaPago = reg.Field<string>("FormaPago"),
                               FechaPago = reg.Field<DateTime>("FechaPago").ToString("yyyy-MM-dd"),
                               Valor = String.Format("{0:C}", reg.Field<Decimal>("Valor")),
                               Detalle = reg.Field<string>("Detalle"),

                           }).ToList<Pagos>();
                FormResponse.root.Add(Retorno);
            }
            catch (Exception e)
            {
                log.FilePath = p_Log;
                log.WriteMensaje("getConsultaPagosFechas: " + e.Message.ToString());

                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
            }
            return FormResponse;
        }
    }
}
