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
    [RoutePrefix("api/exportarReporteVentasProvMensual")]
    public class exportarReporteVentasProvMensualController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("exportarReporteVentasProvMensual")]
        [HttpPost]
        public HttpResponseMessage exportarReporteVentasProvMensual(Gra_reporteEnviar datosExportar)
        {
            String[] meses = new String[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
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
                xmlParam.DocumentElement.SetAttribute("FechaDesde", datosExportar.p_reporteDatos.Fecha1);
                xmlParam.DocumentElement.SetAttribute("FechaHasta", datosExportar.p_reporteDatos.Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodSAP", (datosExportar.p_reporteDatos.CodSap == null ? "" : datosExportar.p_reporteDatos.CodSap));
                foreach (var it in datosExportar.p_reporteAlmacen)
                {
                    XmlElement elem = xmlParam.CreateElement("Almacen");
                    elem.SetAttribute("CodAlmacen", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                }

                foreach (var it in datosExportar.p_reporteMaterial)
                {
                    XmlElement elem = xmlParam.CreateElement("Articulo");
                    elem.SetAttribute("CodArticulo", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 720, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    ct.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = datosExportar.p_reporteDatos.p_usuario;
                    drowc["NomProveedor"] = datosExportar.p_reporteDatos.nomreporte;
                    ct.Rows.Add(drowc);


                    DataTable dt = new DataTable("Detallecab");
                    dt.Columns.Add("anio", System.Type.GetType("System.String"));
                    dt.Columns.Add("mes", System.Type.GetType("System.String"));
                    dt.Columns.Add("CodCentro", System.Type.GetType("System.String"));
                    dt.Columns.Add("NomAlmacen", System.Type.GetType("System.String"));
                    dt.Columns.Add("CodMaterial", System.Type.GetType("System.String"));
                    dt.Columns.Add("DesMaterial", System.Type.GetType("System.String"));
                    dt.Columns.Add("CantVendida", System.Type.GetType("System.String"));

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["anio"] = datosExportar.p_reporteDatos.Fecha1.Split('/')[2];
                        drowd["mes"] = meses[Convert.ToInt16(datosExportar.p_reporteDatos.Fecha1.Split('/')[1]) - 1];
                        drowd["CodCentro"] = Convert.ToString(item["CodCentro"]);
                        drowd["NomAlmacen"] = Convert.ToString(item["NomAlmacen"]);
                        drowd["CodMaterial"] = Convert.ToString(item["CodMaterial"]);
                        drowd["DesMaterial"] = Convert.ToString(item["DesMaterial"]);
                        drowd["CantVendida"] = Convert.ToString(item["CantVendida"]);
                        dt.Rows.Add(drowd);
                    }

                    rptDataSourcecab = new ReportDataSource("CabeceraVentaCentro", ct);
                    rptDataSourcedet = new ReportDataSource("detalleVentaProvMEnsual", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReportVentaProvMensual.rdlc");
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
