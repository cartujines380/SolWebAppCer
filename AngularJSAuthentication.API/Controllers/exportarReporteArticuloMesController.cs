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
    [RoutePrefix("api/exportarReporteArticuloMes")]
    public class exportarReporteArticuloMesController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("exportarReporteArticulo")]
        [HttpPost]
        public HttpResponseMessage exportarReporteArticulo(Gra_reporteEnviar datosExportarArticulo)
        {
            HttpResponseMessage result = null;
            DataSet ds = new DataSet();
            string archivo = "";
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repVentaxCentro> lst_retornoVentaCentro = new List<repVentaxCentro>();
            List<repcodigoArticulo> lst_codigoImagen = new List<repcodigoArticulo>();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("FechaDesde", datosExportarArticulo.p_reporteDatos.Fecha1);
                xmlParam.DocumentElement.SetAttribute("FechaHasta", datosExportarArticulo.p_reporteDatos.Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodSAP", (datosExportarArticulo.p_reporteDatos.CodSap == null ? "" : datosExportarArticulo.p_reporteDatos.CodSap));
                foreach (var it in datosExportarArticulo.p_reporteAlmacen)
                {
                    XmlElement elem = xmlParam.CreateElement("Almacen");
                    elem.SetAttribute("CodAlmacen", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                }

                foreach (var it in datosExportarArticulo.p_reporteMaterial)
                {
                    XmlElement elem = xmlParam.CreateElement("Articulo");
                    elem.SetAttribute("CodArticulo", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 718, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    ct.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = datosExportarArticulo.p_reporteDatos.p_usuario;
                    drowc["NomProveedor"] = datosExportarArticulo.p_reporteDatos.nomreporte;
                    ct.Rows.Add(drowc);


                    DataTable dt = new DataTable("Detallecab");
                    dt.Columns.Add("anio", System.Type.GetType("System.String"));
                    dt.Columns.Add("mes", System.Type.GetType("System.String"));
                    dt.Columns.Add("codMaterial", System.Type.GetType("System.String"));
                    dt.Columns.Add("desMaterial", System.Type.GetType("System.String"));
                    dt.Columns.Add("cantVendida", System.Type.GetType("System.String"));
 

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["anio"] = Convert.ToString(item["anio"]);
                        drowd["mes"] = Convert.ToString(item["mes"]);
                        drowd["codMaterial"] = Convert.ToString(item["codMaterial"]);
                        drowd["desMaterial"] = Convert.ToString(item["desMaterial"]);
                        drowd["cantVendida"] = Convert.ToString(item["cantVendida"]);
                        dt.Rows.Add(drowd);
                    }

                    rptDataSourcecab = new ReportDataSource("CabeceraVentaArticulo", ct);
                    rptDataSourcedet = new ReportDataSource("DetalleVentaArticulo", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteVentaArticulo.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    archivo = "Reporte" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                    bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);

                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new MemoryStream(bytes));
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = archivo;
                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
