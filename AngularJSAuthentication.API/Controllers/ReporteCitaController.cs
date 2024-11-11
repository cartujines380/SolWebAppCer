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
    [RoutePrefix("api/ReporteCita")]
    public class ReporteCitaController : ApiController
    {

        [ActionName("ExportarCita")]
        [HttpPost]
             public string ExportarCita(Tra_ReporteTabularCita Cita)
        {
            string result = "";
            string archivo = "";
            try
            {
                ReportDataSource rptDataSourcecab;
                ReportDataSource rptDataSourcedet;
                DataTable ct = new DataTable("Cabecera");
                ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                Tra_ReporteTabularCita.Tra_ReporteCitaCab cb = Cita.p_cabeceraCita;
                DataRow drowc = ct.NewRow();
                drowc["usuario"] = cb.usuario;
                ct.Rows.Add(drowc);

                DataTable dt = new DataTable("Detalle");
                dt.Columns.Add("idconsolidacion", System.Type.GetType("System.String"));
                dt.Columns.Add("emision", System.Type.GetType("System.String"));
                dt.Columns.Add("almacendestino", System.Type.GetType("System.String"));
                dt.Columns.Add("estadoconsolidacion", System.Type.GetType("System.String"));
                dt.Columns.Add("caducidadsolicitud", System.Type.GetType("System.String"));
                dt.Columns.Add("vehiculo", System.Type.GetType("System.String"));
                dt.Columns.Add("chofer", System.Type.GetType("System.String"));
                dt.Columns.Add("asistente", System.Type.GetType("System.String"));


                foreach (Tra_ReporteTabularCita.Tra_ReporteCitaDet dr in Cita.p_detalleCita)
                {
                    DataRow drowd = dt.NewRow();
                    drowd["idconsolidacion"] = dr.idconsolidacion.ToString();
                    drowd["emision"] = dr.emision.ToString();
                    drowd["almacendestino"] = dr.almacendestino.ToString();
                    drowd["estadoconsolidacion"] = dr.estadoconsolidacion.ToString();
                    drowd["caducidadsolicitud"] = dr.caducidadsolicitud.ToString();
                    drowd["vehiculo"] = dr.vehiculo.ToString();
                    drowd["chofer"] = dr.chofer.ToString();
                    drowd["asistente"] = dr.asistente.ToString();
                    dt.Rows.Add(drowd);
                }
                rptDataSourcecab = new ReportDataSource("RPTListadoCitaCab", ct);
                rptDataSourcedet = new ReportDataSource("RPTListadoCitaDet", dt);
                ReportViewer auxc = new ReportViewer();
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = "";
                string encoding = "";
                string extension = "";
                auxc.ProcessingMode = ProcessingMode.Local;
                auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteCita.rdlc");
                auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                byte[] bytes = null;
                if (cb.tipo == "1")
                {
                    archivo = "ReporteListadoCita" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                    bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                }
                else
                {
                    archivo = "ReporteListadoCita" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
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
