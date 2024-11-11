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
    [RoutePrefix("api/ReporteChofer")]
    public class ReporteChoferController : ApiController
    {

        [ActionName("ExportarChofer")]
        [HttpPost]
        public string ExportarChofer(Tra_ReporteTabularChofer Chofer)
        {
            string result = "";
            string archivo = "";
            try
            {
                ReportDataSource rptDataSourcecab;
                ReportDataSource rptDataSourcedet;
                DataTable ct = new DataTable("Cabecera");
                ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                Tra_ReporteTabularChofer.Tra_ReporteChoferCab cb = Chofer.p_cabeceraChofer;
                DataRow drowc = ct.NewRow();
                drowc["usuario"] = cb.usuario;
                ct.Rows.Add(drowc);


                DataTable dt = new DataTable("Detalle");
                dt.Columns.Add("idchofer", System.Type.GetType("System.String"));
                dt.Columns.Add("nombres", System.Type.GetType("System.String"));
                dt.Columns.Add("licencia", System.Type.GetType("System.String"));
                dt.Columns.Add("telefono", System.Type.GetType("System.String"));
                dt.Columns.Add("direccion", System.Type.GetType("System.String"));
                dt.Columns.Add("estado", System.Type.GetType("System.String"));


                foreach (Tra_ReporteTabularChofer.Tra_ReporteChoferDet dr in Chofer.p_detalleChofer)
                {
                    DataRow drowd = dt.NewRow();
                    drowd["idchofer"] = dr.idchofer.ToString();
                    drowd["nombres"] = dr.nombres.ToString();
                    drowd["licencia"] = dr.licencia.ToString();
                    drowd["telefono"] = dr.telefono.ToString();
                    drowd["direccion"] = dr.direccion.ToString();
                    drowd["estado"] = dr.estado.ToString();
                    dt.Rows.Add(drowd);
                }
                rptDataSourcecab = new ReportDataSource("RPTListadoChoferesCab", ct);
                rptDataSourcedet = new ReportDataSource("RPTListadoChoferesDet", dt);
                ReportViewer auxc = new ReportViewer();
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = "";
                string encoding = "";
                string extension = "";
                auxc.ProcessingMode = ProcessingMode.Local;
                auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteChoferes.rdlc");
                auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                byte[] bytes = null;
                if (cb.tipo == "1")
                {
                    archivo = "ReporteListadoChofer" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                    bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                }
                else
                {
                    archivo = "ReporteListadoChofer" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
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
