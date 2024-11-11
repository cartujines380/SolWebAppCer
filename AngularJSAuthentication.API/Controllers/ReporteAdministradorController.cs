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
using System.Globalization;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/ReporteAdministrador")]
    public class ReporteAdministradorController : ApiController
    {
        [ActionName("getConsulaGridNoIngreso")]
        [HttpGet]
        public FormResponseTransporte ConsulaGridNoIngreso(string CodSap, string Usuario)
        {
            List<ModelReporteAdminNoUsuario> lst_retornoBandeja = new List<ModelReporteAdminNoUsuario>();
            ModelReporteAdminNoUsuario mod_Bandeja;
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("CodSap", CodSap);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 321, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_Bandeja = new ModelReporteAdminNoUsuario();
                        mod_Bandeja.ruc = Convert.ToString(item["RUC"]);
                        mod_Bandeja.codproveedor = Convert.ToString(item["CodProveedor"]);
                        mod_Bandeja.nomcomercial = Convert.ToString(item["NomComercial"]);
                        mod_Bandeja.Telefono = Convert.ToString(item["Telefono"]);
                        mod_Bandeja.Usuario = Convert.ToString(item["Usuario"]);
                        mod_Bandeja.nombre = Convert.ToString(item["nombre"]);
                        mod_Bandeja.CorreoE = Convert.ToString(item["CorreoE"]);
                        mod_Bandeja.FechaRegistro = Convert.ToString(item["FechaRegistro"]);
                        lst_retornoBandeja.Add(mod_Bandeja);
                    }
                    FormResponse.root.Add(lst_retornoBandeja);
                    if (ds.Tables[0].Rows.Count>0)
                    {
                        FormResponse.success = true;    
                    }else
                    {
                        FormResponse.success = false;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
            }
            return FormResponse;
        }

        [ActionName("getConsulaGridNoOrdenCompra")]
        [HttpGet]
        public FormResponseTransporte ConsulaGridNoOrdenCompra(string CodSapno, string rucno)
        {
            List<ModelReporteAdminNoOrdenCompra> lst_retornoBandeja = new List<ModelReporteAdminNoOrdenCompra>();
            ModelReporteAdminNoOrdenCompra mod_Bandeja;
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("CodSap", CodSapno);
                xmlParam.DocumentElement.SetAttribute("ruc", rucno);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 322, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_Bandeja = new ModelReporteAdminNoOrdenCompra();
                        mod_Bandeja.ruc = Convert.ToString(item["RUC"]);
                        mod_Bandeja.codproveedor = Convert.ToString(item["CodProveedor"]);
                        mod_Bandeja.nomcomercial = Convert.ToString(item["NomComercial"]);
                        mod_Bandeja.telefono = Convert.ToString(item["Telefono"]);
                        mod_Bandeja.fechapedido = Convert.ToString(item["FechaPedido"]);
                        mod_Bandeja.cantidad = Convert.ToString(item["CANTIDAD"]);
                        lst_retornoBandeja.Add(mod_Bandeja);
                    }
                    FormResponse.root.Add(lst_retornoBandeja);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FormResponse.success = true;
                    }
                    else
                    {
                        FormResponse.success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
            }
            return FormResponse;
        }

        [ActionName("getConsulaGridProveedorNoSolicitud")]
        [HttpGet]
        public FormResponseTransporte ConsulaGridProveedorNoSolicitud(string CodSapns, string rucns)
        {
            List<ModelReporteAdminProveedorNoSolicitud> lst_retornoBandeja = new List<ModelReporteAdminProveedorNoSolicitud>();
            ModelReporteAdminProveedorNoSolicitud mod_Bandeja;
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("CodSap", CodSapns);
                xmlParam.DocumentElement.SetAttribute("ruc", rucns);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 323, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_Bandeja = new ModelReporteAdminProveedorNoSolicitud();
                        mod_Bandeja.ruc = Convert.ToString(item["RUC"]);
                        mod_Bandeja.codproveedor = Convert.ToString(item["CodProveedor"]);
                        mod_Bandeja.nomcomercial = Convert.ToString(item["NomComercial"]);
                        mod_Bandeja.dirCalleNum = Convert.ToString(item["DirCalleNum"]);
                        mod_Bandeja.ciudad = Convert.ToString(item["ciudad"]);
                        mod_Bandeja.telefono = Convert.ToString(item["Telefono"]);
                        mod_Bandeja.correoE = Convert.ToString(item["CorreoE"]);
                        mod_Bandeja.representante = Convert.ToString(item["representante"]);
                        lst_retornoBandeja.Add(mod_Bandeja);
                    }
                    FormResponse.root.Add(lst_retornoBandeja);
                    FormResponse.success = true;
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
            }
            return FormResponse;
        }


        [ActionName("ExportarNoIngreso")]
        [HttpGet]
        public HttpResponseMessage ExportarNoIngreso(string tipo,string usuariologon,string CodSap, string Usuario)
        {
            HttpResponseMessage result = null;
            string archivo = "";
            try
            {
                DataSet ds = new DataSet();
                ClsGeneral objEjecucion = new ClsGeneral();
                XmlDocument xmlParam = new XmlDocument();
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("CodSap", CodSap);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 321, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    
                ReportDataSource rptDataSourcecab;
                ReportDataSource rptDataSourcedet;
                DataTable ct = new DataTable("Cabecera");
                ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                DataRow drowc = ct.NewRow();
                drowc["usuario"] = usuariologon;
                ct.Rows.Add(drowc);

                DataTable dt = new DataTable("Detalle");
                dt.Columns.Add("ruc", System.Type.GetType("System.String"));
                dt.Columns.Add("codproveedor", System.Type.GetType("System.String"));
                dt.Columns.Add("nomcomercial", System.Type.GetType("System.String"));
                dt.Columns.Add("Telefono", System.Type.GetType("System.String"));
                dt.Columns.Add("Usuario", System.Type.GetType("System.String"));
                dt.Columns.Add("nombre", System.Type.GetType("System.String"));
                dt.Columns.Add("CorreoE", System.Type.GetType("System.String"));
                dt.Columns.Add("FechaRegistro", System.Type.GetType("System.String"));

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    DataRow drowd = dt.NewRow();
                    drowd["ruc"] = Convert.ToString(item["RUC"]);
                    drowd["codproveedor"] = Convert.ToString(item["CodProveedor"]);
                    drowd["nomcomercial"] = Convert.ToString(item["NomComercial"]);
                    drowd["Telefono"] = Convert.ToString(item["Telefono"]);
                    drowd["Usuario"] = Convert.ToString(item["Usuario"]);
                    drowd["nombre"] = Convert.ToString(item["nombre"]);
                    drowd["CorreoE"] = Convert.ToString(item["CorreoE"]);
                    drowd["FechaRegistro"] = Convert.ToString(item["FechaRegistro"]);
                    dt.Rows.Add(drowd);
                }
                rptDataSourcecab = new ReportDataSource("RPTListadoNoIngresoCab", ct);
                rptDataSourcedet = new ReportDataSource("RPTListadoNoIngresoDet", dt);
                ReportViewer auxc = new ReportViewer();
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = "";
                string encoding = "";
                string extension = "";
                auxc.ProcessingMode = ProcessingMode.Local;
                auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteUsuarioNoIngreso.rdlc");
                auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                byte[] bytes = null;
                if (tipo == "1")
                {
                    archivo = "ReporteListadoCita" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                    bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                }
                if (tipo == "2")
                {
                    archivo = "ReporteListadoCita" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                    bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                }

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

        [ActionName("ExportarNoCompra")]
        [HttpGet]
        public HttpResponseMessage ExportarNoCompra(string tipo, string usuariologon, string CodSap, string ruc)
        {
            HttpResponseMessage result = null;
            string archivo = "";
            try
            {
                DataSet ds = new DataSet();
                ClsGeneral objEjecucion = new ClsGeneral();
                XmlDocument xmlParam = new XmlDocument();
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("CodSap", CodSap);
                xmlParam.DocumentElement.SetAttribute("ruc", ruc);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 322, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = usuariologon;
                    ct.Rows.Add(drowc);

                    DataTable dt = new DataTable("Detalle");
                    dt.Columns.Add("ruc", System.Type.GetType("System.String"));
                    dt.Columns.Add("codproveedor", System.Type.GetType("System.String"));
                    dt.Columns.Add("nomcomercial", System.Type.GetType("System.String"));
                    dt.Columns.Add("Telefono", System.Type.GetType("System.String"));
                    dt.Columns.Add("FechaPedido", System.Type.GetType("System.String"));
                    dt.Columns.Add("cantidad", System.Type.GetType("System.String"));


                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["ruc"] = Convert.ToString(item["RUC"]);
                        drowd["codproveedor"] = Convert.ToString(item["CodProveedor"]);
                        drowd["nomcomercial"] = Convert.ToString(item["NomComercial"]);
                        drowd["Telefono"] = Convert.ToString(item["Telefono"]);
                        drowd["FechaPedido"] = Convert.ToString(item["FechaPedido"]);
                        drowd["cantidad"] = Convert.ToString(item["CANTIDAD"]);
                        dt.Rows.Add(drowd);
                    }
                    rptDataSourcecab = new ReportDataSource("RPTListadoNoCompraCab", ct);
                    rptDataSourcedet = new ReportDataSource("RPTListadoNoCompraDet", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteProveedorNoCompra.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    if (tipo == "1")
                    {
                        archivo = "ReporteListadoNoCompra" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                        bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }
                    if (tipo == "2")
                    {
                        archivo = "ReporteListadoNoCompra" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                        bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }

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

        [ActionName("ExportarProveedorNoSolicitud")]
        [HttpGet]
        public HttpResponseMessage ExportarProveedorNoSolicitud(string tipo, string usuariologonns, string CodSapns, string rucns)
        {
            HttpResponseMessage result = null;
            string archivo = "";
            try
            {
                DataSet ds = new DataSet();
                ClsGeneral objEjecucion = new ClsGeneral();
                XmlDocument xmlParam = new XmlDocument();
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("CodSap", CodSapns);
                xmlParam.DocumentElement.SetAttribute("ruc", rucns);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 323, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = usuariologonns;
                    ct.Rows.Add(drowc);

                    DataTable dt = new DataTable("Detalle");
                    dt.Columns.Add("ruc", System.Type.GetType("System.String"));
                    dt.Columns.Add("codproveedor", System.Type.GetType("System.String"));
                    dt.Columns.Add("nomcomercial", System.Type.GetType("System.String"));
                    dt.Columns.Add("dirCalleNum", System.Type.GetType("System.String"));
                    dt.Columns.Add("ciudad", System.Type.GetType("System.String"));
                    dt.Columns.Add("telefono", System.Type.GetType("System.String"));
                    dt.Columns.Add("correoE", System.Type.GetType("System.String"));
                    dt.Columns.Add("representante", System.Type.GetType("System.String"));

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["ruc"] = Convert.ToString(item["RUC"]);
                        drowd["codproveedor"] = Convert.ToString(item["CodProveedor"]);
                        drowd["nomcomercial"] = Convert.ToString(item["NomComercial"]);
                        drowd["dirCalleNum"] = Convert.ToString(item["DirCalleNum"]);
                        drowd["ciudad"] = Convert.ToString(item["ciudad"]);
                        drowd["telefono"] = Convert.ToString(item["Telefono"]);
                        drowd["correoE"] = Convert.ToString(item["CorreoE"]);
                        drowd["representante"] = Convert.ToString(item["representante"]);
                        dt.Rows.Add(drowd);
                    }
                    rptDataSourcecab = new ReportDataSource("RPTListadoNoSolicitudCab", ct);
                    rptDataSourcedet = new ReportDataSource("RPTListadoNoSolicitudDet", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteNoSolicitud.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    if (tipo == "1")
                    {
                        archivo = "ReporteNoSolicitud" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                        bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }
                    if (tipo == "2")
                    {
                        archivo = "ReporteNoSolicitud" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                        bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }

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

       

        [ActionName("ExportarSolicitudEtiqueta")]
        [HttpGet]
        public HttpResponseMessage ExportarSolicitudEtiqueta(string tiposo, string usuariologonso, string CodSapso, string rucso, string fecha1so, string fecha2so, string estadoso)
        {
            HttpResponseMessage result = null;
            string archivo = "";
            String reporteTipo = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("CodSap", CodSapso);
                xmlParam.DocumentElement.SetAttribute("ruc", rucso);
                xmlParam.DocumentElement.SetAttribute("Fecha1", fecha1so);
                xmlParam.DocumentElement.SetAttribute("Fecha2", fecha2so);
                xmlParam.DocumentElement.SetAttribute("Estado", estadoso);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 327, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    ct.Columns.Add("TipoReporte", System.Type.GetType("System.String"));
                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = usuariologonso;
                    if (estadoso =="2")
                    {
                        reporteTipo = "IMPRESAS";
                    }
                    else
                    {
                        reporteTipo = "ANULADAS";
                    }
                    drowc["TipoReporte"] = reporteTipo;
                    ct.Rows.Add(drowc);


                    DataTable dt = new DataTable("Detallecab");
                    dt.Columns.Add("fechasolicitud", System.Type.GetType("System.String"));
                    dt.Columns.Add("numerosolicitud", System.Type.GetType("System.String"));
                    dt.Columns.Add("nombreproveedor", System.Type.GetType("System.String"));
                    dt.Columns.Add("codarticulo", System.Type.GetType("System.String"));
                    dt.Columns.Add("descripcion", System.Type.GetType("System.String"));
                    dt.Columns.Add("cantidadpedido", System.Type.GetType("System.String"));
                    dt.Columns.Add("cantidadsolicitud", System.Type.GetType("System.String"));

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["fechasolicitud"] = Convert.ToString(item["fechasolicitud"]);
                        drowd["numerosolicitud"] = Convert.ToString(item["numerosolicitud"]);
                        drowd["nombreproveedor"] = Convert.ToString(item["nombreproveedor"]);
                        drowd["codarticulo"] = Convert.ToString(item["codarticulo"]);
                        drowd["descripcion"] = Convert.ToString(item["descripcion"]);
                        drowd["cantidadpedido"] = Convert.ToString(item["cantidadpedido"]);
                        drowd["cantidadsolicitud"] = Convert.ToString(item["cantidadsolicitud"]);
                        dt.Rows.Add(drowd);
                    }




                    rptDataSourcecab = new ReportDataSource("SolicitudEtiquetaCab", ct);
                    rptDataSourcedet = new ReportDataSource("SolicitudEtiquetaDet", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteSolicitudEtiqueta.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    if (tiposo == "1")
                    {
                        archivo = "ReporteSolicitudEtiqueta" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                        bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }
                    if (tiposo == "2")
                    {
                        archivo = "ReporteSolicitudEtiqueta" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                        bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }

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


        [ActionName("ExportarLogComunicado")]
        [HttpGet]
        public HttpResponseMessage ExportarLogComunicado(string tipo, string usuariologon, string CodSap, string ruc, string fecha1, string fecha2)
        {
            HttpResponseMessage result = null;
            string archivo = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("CodSap", CodSap);
                xmlParam.DocumentElement.SetAttribute("ruc", ruc);
                xmlParam.DocumentElement.SetAttribute("Fecha1", fecha1);
                xmlParam.DocumentElement.SetAttribute("Fecha2", fecha2);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 325, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = usuariologon;
                    ct.Rows.Add(drowc);


                    DataTable dt = new DataTable("Detallecab");
                    dt.Columns.Add("codigo", System.Type.GetType("System.String"));
                    dt.Columns.Add("titulo", System.Type.GetType("System.String"));
                    dt.Columns.Add("comunicado", System.Type.GetType("System.String"));
                    dt.Columns.Add("categoria", System.Type.GetType("System.String"));
                    dt.Columns.Add("prioridad", System.Type.GetType("System.String"));
                    dt.Columns.Add("obligatorio", System.Type.GetType("System.String"));
                    dt.Columns.Add("usringreso", System.Type.GetType("System.String"));
                    dt.Columns.Add("fechapublicacion", System.Type.GetType("System.String"));
                    dt.Columns.Add("codproveedor", System.Type.GetType("System.String"));
                    dt.Columns.Add("ruc", System.Type.GetType("System.String"));
                    dt.Columns.Add("nomcomercial", System.Type.GetType("System.String"));
                    dt.Columns.Add("correoe", System.Type.GetType("System.String"));
                    dt.Columns.Add("dircalleprinc", System.Type.GetType("System.String"));
                    dt.Columns.Add("usuario", System.Type.GetType("System.String"));
                    dt.Columns.Add("identificacion", System.Type.GetType("System.String"));
                    dt.Columns.Add("apellido1", System.Type.GetType("System.String"));
                    dt.Columns.Add("apellido2", System.Type.GetType("System.String"));
                    dt.Columns.Add("nombre1", System.Type.GetType("System.String"));
                    dt.Columns.Add("nombre2", System.Type.GetType("System.String"));
                    dt.Columns.Add("correousuario", System.Type.GetType("System.String"));
                    dt.Columns.Add("funcion", System.Type.GetType("System.String"));
                    dt.Columns.Add("departamento", System.Type.GetType("System.String"));
                    dt.Columns.Add("rolAdmin", System.Type.GetType("System.String"));
                    dt.Columns.Add("rolComercial", System.Type.GetType("System.String"));
                    dt.Columns.Add("rolContable", System.Type.GetType("System.String"));
                    dt.Columns.Add("rolLogistico", System.Type.GetType("System.String"));
                    dt.Columns.Add("imagen", System.Type.GetType("System.Byte[]"));
                    var fileSavePath = HttpContext.Current.Server.MapPath("~/Imagen/visto.png");
                    byte[] datos = File.ReadAllBytes(fileSavePath);

                    foreach (DataRow item in ds.Tables[3].Rows)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["codigo"] = Convert.ToString(item["codigo"]);
                        drowd["titulo"] = Convert.ToString(item["titulo"]);
                        drowd["comunicado"] = Convert.ToString(item["comunicado"]);
                        drowd["categoria"] = Convert.ToString(item["categoria"]);
                        drowd["prioridad"] = Convert.ToString(item["prioridad"]);
                        drowd["obligatorio"] = Convert.ToString(item["obligatorio"]);
                        drowd["usringreso"] = Convert.ToString(item["usringreso"]);
                        drowd["fechapublicacion"] = Convert.ToString(item["fechapublicacion"]);
                        drowd["codproveedor"] = Convert.ToString(item["codproveedor"]);
                        drowd["ruc"] = Convert.ToString(item["ruc"]);
                        drowd["nomcomercial"] = Convert.ToString(item["nomcomercial"]);
                        drowd["correoe"] = Convert.ToString(item["correoe"]);
                        drowd["dircalleprinc"] = Convert.ToString(item["dircalleprinc"]);
                        drowd["usuario"] = Convert.ToString(item["usuario"]);
                        drowd["identificacion"] = Convert.ToString(item["identificacion"]);
                        drowd["apellido1"] = Convert.ToString(item["apellido1"]);
                        drowd["apellido2"] = Convert.ToString(item["apellido2"]);
                        drowd["nombre1"] = Convert.ToString(item["nombre1"]);
                        drowd["nombre2"] = Convert.ToString(item["nombre2"]);
                        drowd["funcion"] = Convert.ToString(item["funcion"]);
                        drowd["departamento"] = Convert.ToString(item["departamento"]);
                        drowd["correousuario"] = Convert.ToString(item["CORREOUSUARIO"]);
                        drowd["rolAdmin"] = Convert.ToString(item["rolAdmin"]);
                        drowd["rolComercial"] = Convert.ToString(item["rolComercial"]);
                        drowd["rolContable"] = Convert.ToString(item["rolContable"]);
                        drowd["rolLogistico"] = Convert.ToString(item["rolLogistico"]);
                        drowd["imagen"] = datos;

                        dt.Rows.Add(drowd);
                    }




                    rptDataSourcecab = new ReportDataSource("LogComunicacionRPT", ct);
                    rptDataSourcedet = new ReportDataSource("LogComunicacionCab", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteLogComunicacion.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    if (tipo == "1")
                    {
                        archivo = "ReporteListadoNoCompra" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                        bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }
                    if (tipo == "2")
                    {
                        archivo = "ReporteListadoNoCompra" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                        bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }

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

        [ActionName("getConsulaGridSolicitudEtiqueta")]
        [HttpGet]
        public FormResponseTransporte ConsulaGridSolicitudEtiqueta(string CodSapno, string rucno, string fecha1, string fecha2, string estado)
        {
            List<ModelReporteSolicitudEtiqueta> lst_retornoBandejacab = new List<ModelReporteSolicitudEtiqueta>();
            ModelReporteSolicitudEtiqueta mod_Bandejacab;
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("CodSap", CodSapno);
                xmlParam.DocumentElement.SetAttribute("ruc", rucno);
                xmlParam.DocumentElement.SetAttribute("Fecha1", fecha1);
                xmlParam.DocumentElement.SetAttribute("Fecha2", fecha2);
                xmlParam.DocumentElement.SetAttribute("Estado", estado);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 327, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_Bandejacab = new ModelReporteSolicitudEtiqueta();
                        mod_Bandejacab.fechasolicitud = Convert.ToString(item["fechasolicitud"]);
                        mod_Bandejacab.numerosolicitud = Convert.ToString(item["numerosolicitud"]);
                        mod_Bandejacab.nombreproveedor = Convert.ToString(item["nombreproveedor"]);
                        mod_Bandejacab.codarticulo = Convert.ToString(item["codarticulo"]);
                        mod_Bandejacab.descripcion = Convert.ToString(item["descripcion"]);
                        mod_Bandejacab.cantidadpedido = Convert.ToString(item["cantidadpedido"]);
                        mod_Bandejacab.cantidadsolicitud = Convert.ToString(item["cantidadsolicitud"]);
                        mod_Bandejacab.estado = Convert.ToString(item["estado"]);
                        lst_retornoBandejacab.Add(mod_Bandejacab);
                    }
                    FormResponse.root.Add(lst_retornoBandejacab);
                   FormResponse.success = true;
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
            }
            return FormResponse;
        }

        [ActionName("getConsulaGridLogComunicacion")]
        [HttpGet]
        public FormResponseTransporte ConsulaGridLogComunicacion(string CodSapno, string rucno, string fecha1, string fecha2)
        {
            List<ModelReporteLogComunicacioncab> lst_retornoBandejacab = new List<ModelReporteLogComunicacioncab>();
            ModelReporteLogComunicacioncab mod_Bandejacab;
            List<ModelReporteLogComunicaciondet> lst_retornoBandejadet = new List<ModelReporteLogComunicaciondet>();
            ModelReporteLogComunicaciondet mod_Bandejadet;
            List<ModelReporteLogComunicaciondetusuario> lst_retornoBandejadetusuario = new List<ModelReporteLogComunicaciondetusuario>();
            ModelReporteLogComunicaciondetusuario mod_Bandejadetusuario;
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("CodSap", CodSapno);
                xmlParam.DocumentElement.SetAttribute("ruc", rucno);
                xmlParam.DocumentElement.SetAttribute("Fecha1", fecha1);
                xmlParam.DocumentElement.SetAttribute("Fecha2", fecha2);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 325, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_Bandejacab = new ModelReporteLogComunicacioncab();
                        mod_Bandejacab.codigo = Convert.ToString(item["codigo"]);
                        mod_Bandejacab.categoria = Convert.ToString(item["categoria"]);
                        mod_Bandejacab.comunicado = Convert.ToString(item["comunicado"]);
                        mod_Bandejacab.obligatorio = Convert.ToString(item["obligatorio"]);
                        mod_Bandejacab.prioridad = Convert.ToString(item["prioridad"]);
                        mod_Bandejacab.titulo = Convert.ToString(item["titulo"]);
                        mod_Bandejacab.usringreso = Convert.ToString(item["usringreso"]);
                        mod_Bandejacab.fechapublicacion = Convert.ToString(item["fechapublicacion"]);
                        lst_retornoBandejacab.Add(mod_Bandejacab);
                    }
                    FormResponse.root.Add(lst_retornoBandejacab);
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        mod_Bandejadet = new ModelReporteLogComunicaciondet();
                        mod_Bandejadet.codigo = Convert.ToString(item["codigo"]);
                        mod_Bandejadet.codproveedor = Convert.ToString(item["codproveedor"]);
                        mod_Bandejadet.correoe = Convert.ToString(item["correoe"]);
                        mod_Bandejadet.dircalleprinc = Convert.ToString(item["dircalleprinc"]);
                        mod_Bandejadet.nomcomercial = Convert.ToString(item["nomcomercial"]);
                        mod_Bandejadet.ruc = Convert.ToString(item["ruc"]);
                        mod_Bandejadet.titulo = Convert.ToString(item["titulo"]);
                        lst_retornoBandejadet.Add(mod_Bandejadet);
                    }
                    FormResponse.root.Add(lst_retornoBandejadet);
                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        mod_Bandejadetusuario = new ModelReporteLogComunicaciondetusuario();
                        mod_Bandejadetusuario.apellido1 = Convert.ToString(item["apellido1"]);
                        mod_Bandejadetusuario.apellido2 = Convert.ToString(item["apellido2"]);
                        mod_Bandejadetusuario.nombre1 = Convert.ToString(item["nombre1"]);
                        mod_Bandejadetusuario.nombre2 = Convert.ToString(item["nombre2"]);
                        mod_Bandejadetusuario.usuario = Convert.ToString(item["usuario"]);
                        mod_Bandejadetusuario.cod_notificacion = Convert.ToString(item["cod_notificacion"]);
                        mod_Bandejadetusuario.cod_proveedor = Convert.ToString(item["cod_proveedor"]);
                        mod_Bandejadetusuario.correoe = Convert.ToString(item["correoe"]);
                        mod_Bandejadetusuario.identificacion = Convert.ToString(item["identificacion"]);
                        mod_Bandejadetusuario.funcion = Convert.ToString(item["funcion"]);
                        mod_Bandejadetusuario.departamento = Convert.ToString(item["departamento"]);
                        mod_Bandejadetusuario.rolAdmin = Convert.ToInt32(item["rolAdmin"]);
                        mod_Bandejadetusuario.rolComercial = Convert.ToInt32(item["rolComercial"]);
                        mod_Bandejadetusuario.rolContable = Convert.ToInt32(item["rolContable"]);
                        mod_Bandejadetusuario.rolLogistico = Convert.ToInt32(item["rolLogistico"]);
                        lst_retornoBandejadetusuario.Add(mod_Bandejadetusuario);
                    }
                    FormResponse.root.Add(lst_retornoBandejadetusuario);
                    FormResponse.success = true;
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
            }
            return FormResponse;
        }


        [ActionName("getConsulaGridActaRecepcion")]
        [HttpGet]
        public FormResponseTransporte ConsulaGridActaRecepcion(string tipo, string Norden, string Nfactura, string Fecha1, string Fecha2, string codsapat, string codigoAlmacen)
        {
            List<ModelReporteActaRecepcion> lst_retornoBandeja = new List<ModelReporteActaRecepcion>();
            ModelReporteActaRecepcion mod_Bandeja;
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {

                xmlParam.DocumentElement.SetAttribute("Tipo", "2");
                xmlParam.DocumentElement.SetAttribute("Norden", Norden);
                xmlParam.DocumentElement.SetAttribute("Nfactura", Nfactura);
                xmlParam.DocumentElement.SetAttribute("Fecha1", Fecha1);
                xmlParam.DocumentElement.SetAttribute("Fecha2", Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodSap", codsapat);
                xmlParam.DocumentElement.SetAttribute("CodigoAlmacen", codigoAlmacen);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 326, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = false;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {

                        mod_Bandeja = new ModelReporteActaRecepcion();
                        mod_Bandeja.ID_Secuencial = Convert.ToString(item["ID_Secuencial"]);
                        mod_Bandeja.NomComercial = Convert.ToString(item["NomComercial"]);
                        mod_Bandeja.NoOrden = Convert.ToString(item["NoOrden"]);
                        mod_Bandeja.NoFactura = Convert.ToString(item["NoFactura"]);
                        mod_Bandeja.almacen = Convert.ToString(item["almacen"]);
                        mod_Bandeja.ciudad = Convert.ToString(item["ciudad"]);
                        mod_Bandeja.anio = Convert.ToString(item["anio"]);
                        mod_Bandeja.mes = Convert.ToString(item["mes"]);
                        mod_Bandeja.dia = Convert.ToString(item["dia"]);
                        mod_Bandeja.archivo = Convert.ToString(item["archivo"]);
                        mod_Bandeja.RUC = Convert.ToString(item["Ruc"]);
                        mod_Bandeja.Estado = Convert.ToString(item["Estado"]);
                        mod_Bandeja.DesEstado = Convert.ToString(item["DesEstado"]);

                        lst_retornoBandeja.Add(mod_Bandeja);
                    }
                    FormResponse.root.Add(lst_retornoBandeja);
                    if (ds.Tables[0].Rows.Count>0)
                    {
                        FormResponse.success = true;
                    }
                    else
                    {
                        FormResponse.success = false;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
            }
            return FormResponse;
        }
        [ActionName("getActualizaEstadoActaRecepcion")]
        [HttpGet]
        public FormResponseTransporte ActualizaEstadoActaRecepcion(string idSencuencial, string estado)
        {


            FormResponseTransporte FormResponse = new FormResponseTransporte();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {

                xmlParam.DocumentElement.SetAttribute("Tipo", "3");
                xmlParam.DocumentElement.SetAttribute("idSencuencial", idSencuencial);
                xmlParam.DocumentElement.SetAttribute("estado", estado);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 326, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = true;

                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.msgError = ex.Message.ToString();
            }
            return FormResponse;
        }

        [ActionName("ExportarArchivoActaRecepcionADM")]
        [HttpGet]
        public HttpResponseMessage ExportarArchivoActaRecepcionADM(string ADMidarchivo, string ADManio, string ADMmes, string ADMdia)
        {
            HttpResponseMessage result = null;
            try
            {


                byte[] bytes = null;

                string ruta = ((string)System.Configuration.ConfigurationManager.AppSettings["rutaArchivosActas"]).Trim();
                FileInfo fInfo = new FileInfo(ruta + ADManio + "/" + ADMmes + "/" + ADMdia + "/" + ADMidarchivo);
                long numBytes = fInfo.Length;
                FileStream fStream = new FileStream(ruta + ADManio + "/" + ADMmes + "/" + ADMdia + "/" + ADMidarchivo, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fStream);
                // convert the file to a byte array
                bytes = br.ReadBytes((int)numBytes);
                br.Close();

                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(bytes));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = ADMidarchivo;

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        [ActionName("ExportarArchivoActaRecepcion")]
        [HttpGet]
        public HttpResponseMessage ExportarArchivoActaRecepcion(string idarchivo, string anio, string codSap, string codAlmacen)
        {
            HttpResponseMessage result = null;
            try
            {

                ProcesoWs.ServBaseProceso datos = new ProcesoWs.ServBaseProceso();
                byte[] bytes = datos.ArchivoActa(idarchivo, anio, codSap, codAlmacen);
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(bytes));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = idarchivo;

            }
            catch (Exception ex)
            {

            }
            return result;
        }


        #region RFD0-2022-155

        [ActionName("getReporteRol")]
        [HttpGet]
        public FormResponseReporteLog ObtieneReporteRol(string FechaIni, string FechaFin, string Accion)
        {
            clibLogger.clsLogger _objLogServicio = new clibLogger.clsLogger();
            _objLogServicio.Graba_Log_Info("INI >>  API - ObtieneReporteRol " + DateTime.Now);

            List<ModelReporteLog> lst_retornoReporte = new List<ModelReporteLog>();
            ModelReporteLog itemReporteLog;
            FormResponseReporteLog FormResponse = new FormResponseReporteLog();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {

                if (FechaIni != null) {
                    FechaIni = Convert.ToDateTime(FechaIni).ToShortDateString();
                }

                if (FechaFin != null)
                {
                    FechaFin = Convert.ToDateTime(FechaFin).ToShortDateString();
                }

                xmlParam.DocumentElement.SetAttribute("Trx", Accion);
                xmlParam.DocumentElement.SetAttribute("FechaIni", FechaIni);
                xmlParam.DocumentElement.SetAttribute("FechaFin", FechaFin);
                _objLogServicio.Graba_Log_Info("INFO >>  INI-EJECUTA - ObtieneReporteRol " );
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 4301, 1);
                _objLogServicio.Graba_Log_Info("INFO >>  FIN-EJECUTA - ObtieneReporteRol ");

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = true;
                    _objLogServicio.Graba_Log_Info("INFO >>  ITERACION - ObtieneReporteRol ");

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        itemReporteLog = new ModelReporteLog();

                        if (Accion == "ReporteRol")
                        {
                            itemReporteLog.Rol = Convert.ToString(item["CodAD"]);
                            itemReporteLog.Descripcion = Convert.ToString(item["Descripcion"]);
                            itemReporteLog.Fecha = Convert.ToDateTime(item["Fecha"]).ToString("g", CultureInfo.CreateSpecificCulture("es-ES"));
                        }
                        else if (Accion == "AccesoRol")
                        {
                            itemReporteLog.Fecha = Convert.ToDateTime(item["Fecha"]).ToString("g", CultureInfo.CreateSpecificCulture("es-ES"));
                            itemReporteLog.Usuario = Convert.ToString(item["Usuario"]);
                            itemReporteLog.Accion = Convert.ToString(item["Accion"]);
                        }
                        else
                        {
                            itemReporteLog.IdUsuario = Convert.ToString(item["Usuario"]);
                            itemReporteLog.IdTransaccion = Convert.ToString(item["IdTransaccion"]);
                            itemReporteLog.DescTransaccion = Convert.ToString(item["DescTransaccion"]);
                            itemReporteLog.IdOrganizacion = Convert.ToString(item["IdOrganizacion"]);
                            itemReporteLog.DescOrganizacion = Convert.ToString(item["DescOrganizacion"]);
                            itemReporteLog.IdAplicacion = Convert.ToString(item["IdAplicacion"]);
                            itemReporteLog.DescAplicativo = Convert.ToString(item["DescAplicativo"]);
                            itemReporteLog.IdIdentificacion = Convert.ToString(item["IdIdentificacion"]);
                            itemReporteLog.Fecha = Convert.ToDateTime(item["Fecha"]).ToString("g", CultureInfo.CreateSpecificCulture("es-ES"));

                            itemReporteLog.Usuario = Convert.ToString(item["Usuario"]);
                        }

                        lst_retornoReporte.Add(itemReporteLog);
                    }
                    FormResponse.root.Add(lst_retornoReporte);


                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _objLogServicio.Graba_Log_Info("INFO >>  EXISTEN DATOS - ObtieneReporteRol ");

                        FormResponse.success = true;
                        return FormResponse;
                    }

                }
                _objLogServicio.Graba_Log_Info("INFO >> NO  EXISTEN DATOS - ObtieneReporteRol ");

            }
            catch (Exception ex)
            {
                _objLogServicio.Graba_Log_Info("ERROR >>  API - ObtieneReporteRol:  "+ ex.Message + "" + DateTime.Now);

                FormResponse.success = false;
                FormResponse.msgError = ex.Message.ToString();
            }
            return FormResponse;
        }

        [ActionName("getGeneraReporteRol")]
        [HttpPost]
        public HttpResponseMessage  GeneraReporteRol(List<ModelReporteLog> obj, string opcion)
        {


            clibLogger.clsLogger _objLogServicio = new clibLogger.clsLogger();
            _objLogServicio.Graba_Log_Info("INI >>  API - GeneraReporteRol " + DateTime.Now);

            HttpResponseMessage result = null;
            string archivo = "";
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                ReportDataSource rptDataSourcedet;

                DataTable dt = new DataTable("Detalle");
                dt.Columns.Add("Rol", System.Type.GetType("System.String"));
                dt.Columns.Add("Descripcion", System.Type.GetType("System.String"));
                dt.Columns.Add("FechaRegistro", System.Type.GetType("System.String"));
                dt.Columns.Add("Usuario", System.Type.GetType("System.String"));
                dt.Columns.Add("Accion", System.Type.GetType("System.String"));
                dt.Columns.Add("Transaccion", System.Type.GetType("System.String"));
                dt.Columns.Add("Organizacion", System.Type.GetType("System.String"));

                List<ModelReporteLog> lst_retornoReporte = obj;
                _objLogServicio.Graba_Log_Info("INFO >>  API - GeneraReporteRol " + DateTime.Now + " ITERACION");

                foreach (var item in lst_retornoReporte)
                {
                    DataRow drowd = dt.NewRow();
                    drowd["Rol"] = Convert.ToString(item.Rol);
                    drowd["Descripcion"] = Convert.ToString(item.Descripcion);
                    drowd["FechaRegistro"] = Convert.ToString(item.Fecha);
                    drowd["Usuario"] = Convert.ToString(item.Usuario);
                    drowd["Accion"] = Convert.ToString(item.Accion);
                    drowd["Transaccion"] = Convert.ToString(item.DescTransaccion);
                    drowd["Organizacion"] = Convert.ToString(item.DescOrganizacion);
                       
                    dt.Rows.Add(drowd);
                }
                _objLogServicio.Graba_Log_Info("INFO >>  API - GeneraReporteRol : Arma Reporte");

                rptDataSourcedet = new ReportDataSource("DataSetSegAcceso", dt);
                ReportViewer auxc = new ReportViewer();
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = "";
                string encoding = "";
                string extension = "";
                auxc.ProcessingMode = ProcessingMode.Local;
                String RutaTemp = HttpContext.Current.Server.MapPath("~/Reporte/ReporteAuditoria.rdlc");
                _objLogServicio.Graba_Log_Info("INFO >>  API - GeneraReporteRol : Ruta " + RutaTemp);

                auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteAuditoria.rdlc");
                auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                _objLogServicio.Graba_Log_Info("INFO >>  API - GeneraReporteRol : Parametros");

                ReportParameter[] parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("TipoReporte", opcion);

                _objLogServicio.Graba_Log_Info("INFO >>  API - GeneraReporteRol : Obtiene Reporte Local");

                auxc.LocalReport.SetParameters(parameters);
                auxc.LocalReport.Refresh();

                _objLogServicio.Graba_Log_Info("INFO >>  API - GeneraReporteRol : Fin Reporte Local");

                byte[] bytes = null;

                archivo = "Reporte" + ".xls";
                bytes = auxc.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(bytes));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = archivo;

            }
            catch (Exception ex)
            {
                _objLogServicio.Graba_Log_Info("ERROR >>  API - GeneraReporteRol " + DateTime.Now + " ERROR: " + ex.Message);

            }
            return result;
        }


        #endregion

    }
}

