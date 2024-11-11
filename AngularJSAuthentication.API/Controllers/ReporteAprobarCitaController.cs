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
namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/ReporteAprobarCita")]
    public class ReporteAprobarCitaController : ApiController
    {
        [ActionName("ExportarAprobarCita")]
        [HttpPost]
        public string ExportarAprobarCita(Tra_ReporteTabularAprobarCita AprobarCita)
        {
            string result = "";
            string archivo = "";
            try
            {
                ReportDataSource rptDataSourcecab;
                ReportDataSource rptDataSourcedet;
                DataTable ct = new DataTable("Cabecera");
                ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                Tra_ReporteTabularAprobarCita.Tra_ReporteAprobarCitaCab cb = AprobarCita.p_cabeceraAprobarCita;
                DataRow drowc = ct.NewRow();
                drowc["usuario"] = cb.usuario;
                ct.Rows.Add(drowc);

                DataTable dt = new DataTable("Detalle");
                dt.Columns.Add("idcita", System.Type.GetType("System.String"));
                dt.Columns.Add("numcita", System.Type.GetType("System.String"));
                dt.Columns.Add("idconsolidacion", System.Type.GetType("System.String"));
                dt.Columns.Add("bodega", System.Type.GetType("System.String"));
                dt.Columns.Add("estado", System.Type.GetType("System.String"));
                dt.Columns.Add("estadorechazo", System.Type.GetType("System.String"));
                dt.Columns.Add("fechasolicitada", System.Type.GetType("System.String"));
                dt.Columns.Add("fechaasignada", System.Type.GetType("System.String"));
                dt.Columns.Add("caducidadsolicitud", System.Type.GetType("System.String"));
                dt.Columns.Add("citarapida", System.Type.GetType("System.String"));

                foreach (Tra_ReporteTabularAprobarCita.Tra_ReporteAprobarCitaDet dr in AprobarCita.p_detalleAprobarCita)
                {
                    DataRow drowd = dt.NewRow();
                    drowd["idcita"] = dr.idcita.ToString();
                    drowd["numcita"] = dr.numcita.ToString();
                    drowd["idconsolidacion"] = dr.idconsolidacion.ToString();
                    drowd["bodega"] = dr.bodega.ToString();
                    drowd["estado"] = dr.estado.ToString();
                    drowd["estadorechazo"] = dr.estadorechazo.ToString();
                    drowd["fechasolicitada"] = dr.fechasolicitada.ToString();
                    drowd["fechaasignada"] = dr.fechaasignada.ToString();
                    drowd["caducidadsolicitud"] = dr.caducidadsolicitud.ToString();
                    drowd["citarapida"] = dr.citarapida.ToString();
                    dt.Rows.Add(drowd);
                }
                rptDataSourcecab = new ReportDataSource("RPTListadoAprobarCitaCab", ct);
                rptDataSourcedet = new ReportDataSource("RPTListadoAprobarCitaDet", dt);
                ReportViewer auxc = new ReportViewer();
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = "";
                string encoding = "";
                string extension = "";
                auxc.ProcessingMode = ProcessingMode.Local;
                auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteAprobarCita.rdlc");
                auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                byte[] bytes = null;
                if (cb.tipo == "1")
                {
                    archivo = "ReporteListadoAprobarCita" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                    bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                }
                else
                {
                    archivo = "ReporteListadoAprobarCita" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                    bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                }
                result = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/DownloadedDocuments/" + archivo;
                archivo = HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + archivo);
                FileStream fs = System.IO.File.Create(archivo);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                auxc.Dispose();
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }
    }
}
