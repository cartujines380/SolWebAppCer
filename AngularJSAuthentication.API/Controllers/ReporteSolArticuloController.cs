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
    [RoutePrefix("api/ReporteSolArticulo")]
    public class ReporteSolArticuloController : ApiController
    {

        [ActionName("ExportarSolArticulo")]
        [HttpPost]
        public HttpResponseMessage ExportarSolArticulo(Art_ReporteSolArticulo SolArticulo)
        {
            HttpResponseMessage result = null;
            string archivo = "";
            try
            {
                ReportDataSource rptDataSourcecab;
                ReportDataSource rptDataSourcedet;
                DataTable ct = new DataTable("Cabecera");
                ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                ct.Columns.Add("numsolicitud", System.Type.GetType("System.String"));
                ct.Columns.Add("proveedor", System.Type.GetType("System.String"));
                ct.Columns.Add("pais", System.Type.GetType("System.String"));
                ct.Columns.Add("tiposolicitud", System.Type.GetType("System.String"));
                Art_ReporteSolArticulo.Art_ReporteSolArticuloCab cb = SolArticulo.p_cabeceraSolArticulo;
                //Tra_ReporteTabularChofer.Tra_ReporteChoferCab cb = Chofer.p_cabeceraChofer;
                DataRow drowc = ct.NewRow();
                drowc["usuario"] = cb.usuario;
                drowc["numsolicitud"] = cb.numSolicitud;
                drowc["proveedor"] = cb.proveedor;
                drowc["pais"] = cb.pais;
                if (SolArticulo.p_cabeceraSolArticulo.tipoSolicitud == "1")
                    drowc["tiposolicitud"] = "SOLICITUD DE NUEVO ARTÍCULO";
                if (SolArticulo.p_cabeceraSolArticulo.tipoSolicitud == "2")
                    drowc["tiposolicitud"] = "SOLICITUD DE MODIFICACIÓN ARTÍCULO";
                ct.Rows.Add(drowc);


                DataTable dt = new DataTable("Detalle");
                dt.Columns.Add("Codigo", System.Type.GetType("System.String"));
                dt.Columns.Add("Marca", System.Type.GetType("System.String"));
                dt.Columns.Add("Descripcion", System.Type.GetType("System.String"));
                dt.Columns.Add("Texto", System.Type.GetType("System.String"));
                dt.Columns.Add("Presentacion", System.Type.GetType("System.String"));
                dt.Columns.Add("Alcohol", System.Type.GetType("System.String"));
                dt.Columns.Add("Modelo", System.Type.GetType("System.String"));
                dt.Columns.Add("Iva", System.Type.GetType("System.String"));
                dt.Columns.Add("Deducible", System.Type.GetType("System.String"));
                dt.Columns.Add("Retencion", System.Type.GetType("System.String"));


                foreach (Art_ReporteSolArticulo.Art_ReporteSolArticuloDet dr in SolArticulo.p_detalleSolArticulo)
                {
                    if (dr.estado != "Rechazado")
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["Codigo"] = !string.IsNullOrEmpty(dr.codigo) ? dr.codigo.ToString() : "";
                        drowd["Marca"] = !string.IsNullOrEmpty(dr.marca) ? dr.marca.ToString() : "";
                        drowd["Descripcion"] = !string.IsNullOrEmpty(dr.descripcion) ? dr.descripcion.ToString() : "";
                        drowd["Texto"] = !string.IsNullOrEmpty(dr.texto) ? dr.texto.ToString() : "";
                        drowd["Presentacion"] = !string.IsNullOrEmpty(dr.presentacion) ? dr.presentacion.ToString() : "";
                        drowd["Alcohol"] = !string.IsNullOrEmpty(dr.alcohol) ? dr.alcohol.ToString() : "";
                        drowd["Modelo"] = !string.IsNullOrEmpty(dr.modelo) ? dr.modelo.ToString() : "";
                        drowd["Iva"] = !string.IsNullOrEmpty(dr.iva) ? dr.iva.ToString() : "";
                        drowd["Deducible"] = !string.IsNullOrEmpty(dr.deducible) ? dr.deducible.ToString() : "";
                        drowd["Retencion"] = !string.IsNullOrEmpty(dr.retencion) ? dr.retencion.ToString() : "";
                        dt.Rows.Add(drowd);
                    }
                }
                rptDataSourcecab = new ReportDataSource("RPTSolArticuloCab", ct);
                rptDataSourcedet = new ReportDataSource("RPTSolArticuloDet", dt);
                ReportViewer auxc = new ReportViewer();
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = "";
                string encoding = "";
                string extension = "";
                auxc.ProcessingMode = ProcessingMode.Local;
                auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteSolArticulo.rdlc");
                auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                byte[] bytes = null;
                if (cb.tipo == "1")
                {
                    archivo = "ReporteSolArticulo" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                    bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                }
                if (cb.tipo == "2")
                {
                    archivo = "ReporteSolArticulo" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                    bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                }
                

                
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(bytes));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = archivo;




                //archivo = HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + archivo);
                //FileStream fs = System.IO.File.Create(archivo);
                //fs.Write(bytes, 0, bytes.Length);
                //fs.Close();
                auxc.Dispose();
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;




        }

        

    }
}
