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
    [RoutePrefix("api/ReporteMercadeoExport")]
    public class ReporteMercadeoExportController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("exportarReporteMercadeo")]
        [HttpPost]
        public HttpResponseMessage exportarReporteMercadeo(Gra_reporteMercado datosMercadoExp)
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
                xmlParam.DocumentElement.SetAttribute("TipoLista", datosMercadoExp.p_reporteDatos.TipoLista);
                xmlParam.DocumentElement.SetAttribute("Linea", datosMercadoExp.p_reporteDatos.Linea);
                xmlParam.DocumentElement.SetAttribute("Seccion", datosMercadoExp.p_reporteDatos.Seccion);
                xmlParam.DocumentElement.SetAttribute("SubSeccion", datosMercadoExp.p_reporteDatos.SubSeccion);
                xmlParam.DocumentElement.SetAttribute("Grupo", datosMercadoExp.p_reporteDatos.Grupo);
                xmlParam.DocumentElement.SetAttribute("FechaDesde", datosMercadoExp.p_reporteDatos.Fecha1);
                xmlParam.DocumentElement.SetAttribute("FechaHasta", datosMercadoExp.p_reporteDatos.Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodSAP", (datosMercadoExp.p_reporteDatos.CodSap == null ? "" : datosMercadoExp.p_reporteDatos.CodSap));
                foreach (var it in datosMercadoExp.p_reporteAlmacen)
                {
                    XmlElement elem = xmlParam.CreateElement("Almacen");
                    elem.SetAttribute("CodAlmacen", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                }

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 722, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    ct.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = datosMercadoExp.p_reporteDatos.p_usuario;
                    drowc["NomProveedor"] = datosMercadoExp.p_reporteDatos.nomreporte;
                    ct.Rows.Add(drowc);


                    DataTable dt = new DataTable("ReporteMercadeo");
                    dt.Columns.Add("Linea", System.Type.GetType("System.String"));
                    dt.Columns.Add("Seccion", System.Type.GetType("System.String"));
                    dt.Columns.Add("PorSeccion", System.Type.GetType("System.String"));
                    dt.Columns.Add("SubSeccion", System.Type.GetType("System.String"));
                    dt.Columns.Add("PorSubSeccion", System.Type.GetType("System.String"));
                    dt.Columns.Add("Grupo", System.Type.GetType("System.String"));
                    dt.Columns.Add("PorGrupo", System.Type.GetType("System.String"));


                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["Linea"] = Convert.ToString(item["Linea"]);
                        drowd["Seccion"] = Convert.ToString(item["Seccion"]);
                        drowd["PorSeccion"] = Convert.ToString(item["PorSeccion"]);
                        drowd["SubSeccion"] = Convert.ToString(item["SubSeccion"]);
                        drowd["PorSubSeccion"] = Convert.ToString(item["PorSubSeccion"]);
                        drowd["Grupo"] = Convert.ToString(item["Grupo"]);
                        drowd["PorGrupo"] = Convert.ToString(item["PorGrupo"]);
                        dt.Rows.Add(drowd);
                    }

                  
                    rptDataSourcecab = new ReportDataSource("MercadeoCabecera", ct);
                    rptDataSourcedet = new ReportDataSource("MercadeoDetalle", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteMercadeo.rdlc");
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
