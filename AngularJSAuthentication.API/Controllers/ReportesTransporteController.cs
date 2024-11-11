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
    [RoutePrefix("api/ReportesTransporte")]
    public class ReportesTransporteController : ApiController
    {

        [ActionName("getTabularCitas")]
        [HttpGet]
        public FormResponseTransporte getTabularCitas(string tipo, string fechadesdeRPT, string fechahastaRPT, string numcita, string codproveedor)
        {
            List<Tra_ReporteTabularCitas> lst_retornoSolBandeja = new List<Tra_ReporteTabularCitas>();
            Tra_ReporteTabularCitas mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("fechadesde", fechadesdeRPT);
                xmlParam.DocumentElement.SetAttribute("fechahasta", fechahastaRPT);
                xmlParam.DocumentElement.SetAttribute("numcita", numcita);
                xmlParam.DocumentElement.SetAttribute("codproveedor", codproveedor); 
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 318, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudBandeja = new Tra_ReporteTabularCitas();
                        mod_SolicitudBandeja.numcita = Convert.ToString(item["numcita"]);
                        mod_SolicitudBandeja.codsap = Convert.ToString(item["codsap"]);
                        mod_SolicitudBandeja.nombreproveedor = Convert.ToString(item["nombreproveedor"]);
                        mod_SolicitudBandeja.fecha = Convert.ToString(item["fecha"]);
                        mod_SolicitudBandeja.horainicial = Convert.ToString(item["horainicial"]);
                        mod_SolicitudBandeja.horafinal = Convert.ToString(item["horafinal"]);
                        mod_SolicitudBandeja.cedulachofer = Convert.ToString(item["cedulachofer"]);
                        mod_SolicitudBandeja.nombrechofer = Convert.ToString(item["nombrechofer"]);
                        mod_SolicitudBandeja.placa = Convert.ToString(item["placa"]);
                        mod_SolicitudBandeja.marca = Convert.ToString(item["marca"]);
                        mod_SolicitudBandeja.modelo = Convert.ToString(item["modelo"]);
                        mod_SolicitudBandeja.almacendestino = Convert.ToString(item["almacendestino"]);
                        lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSolBandeja);
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }


        [ActionName("ExportarTabular")]
        [HttpGet]
        public HttpResponseMessage ExportarTabular(string tiporeporte, string usuarioCreacion, string tipo, string fechadesdeRPT, string fechahastaRPT, string numcita, string codproveedor)
        {
            HttpResponseMessage result = null;
            string archivo = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("fechadesde", fechadesdeRPT);
                xmlParam.DocumentElement.SetAttribute("fechahasta", fechahastaRPT);
                xmlParam.DocumentElement.SetAttribute("numcita", numcita);
                xmlParam.DocumentElement.SetAttribute("codproveedor", codproveedor); 
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 318, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = usuarioCreacion;
                    ct.Rows.Add(drowc);

                    DataTable dt = new DataTable("Detalle");
                    dt.Columns.Add("numcita", System.Type.GetType("System.String"));
                    dt.Columns.Add("codsap", System.Type.GetType("System.String"));
                    dt.Columns.Add("nombreproveedor", System.Type.GetType("System.String"));
                    dt.Columns.Add("fecha", System.Type.GetType("System.String"));
                    dt.Columns.Add("horainicial", System.Type.GetType("System.String"));
                    dt.Columns.Add("horafinal", System.Type.GetType("System.String"));
                    dt.Columns.Add("cedulachofer", System.Type.GetType("System.String"));
                    dt.Columns.Add("nombrechofer", System.Type.GetType("System.String"));
                    dt.Columns.Add("placa", System.Type.GetType("System.String"));
                    dt.Columns.Add("marca", System.Type.GetType("System.String"));
                    dt.Columns.Add("modelo", System.Type.GetType("System.String"));
                    dt.Columns.Add("almacendestino", System.Type.GetType("System.String"));

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["numcita"] = Convert.ToString(item["numcita"]);
                        drowd["codsap"] = Convert.ToString(item["codsap"]);
                        drowd["nombreproveedor"] = Convert.ToString(item["nombreproveedor"]);
                        drowd["fecha"] = Convert.ToString(item["fecha"]);
                        drowd["horainicial"] = Convert.ToString(item["horainicial"]);
                        drowd["horafinal"] = Convert.ToString(item["horafinal"]);
                        drowd["cedulachofer"] = Convert.ToString(item["cedulachofer"]);
                        drowd["nombrechofer"] = Convert.ToString(item["nombrechofer"]);
                        drowd["placa"] = Convert.ToString(item["placa"]);
                        drowd["marca"] = Convert.ToString(item["marca"]);
                        drowd["modelo"] = Convert.ToString(item["modelo"]);
                        drowd["almacendestino"] = Convert.ToString(item["almacendestino"]);
                        dt.Rows.Add(drowd);
                    }

                
                    rptDataSourcecab = new ReportDataSource("RPTTabularCitaCab", ct);
                    rptDataSourcedet = new ReportDataSource("RPTTabularCitaDet", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/TabularCita.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    if (tiporeporte == "1")
                    {
                        archivo = "ReporteTabularCita" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                        bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }
                    if (tiporeporte == "2")
                    {
                        archivo = "ReporteTabularCita" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                        bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }
                    //result = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/DownloadedDocuments/" + archivo;
                    //archivo = HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + archivo);
                    //FileStream fs = System.IO.File.Create(archivo);
                    //fs.Write(bytes, 0, bytes.Length);
                    //fs.Close();
                    //auxc.Dispose();

                    //
                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new MemoryStream(bytes));
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = archivo;
                }

            }catch(Exception ex)
            {
            }
            return result;


            

        }

      
    }
}
