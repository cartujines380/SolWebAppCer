using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


using System.Data;
using System.Xml;
using clibProveedores.Models;
using clibProveedores;
using AngularJSAuthentication.API.Handlers;
using Microsoft.Reporting.WebForms;
using System.Web;
using System.IO;

namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/AsignacionLineaNeg")]
    public class AsignacionLineaNegController : ApiController
    {

        [AntiForgeryValidate]
        [ActionName("ConsProveedor")]
        [HttpGet]
        public formResponseLinea ConsProveedor(String criterioConsultaProveedor)
        {
            formResponseLinea FormResponse = new formResponseLinea();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                xmlParam.LoadXml(criterioConsultaProveedor);
                ds = objEjecucion.EjecucionGralLinea(xmlParam.OuterXml, 710, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<liConsProveedor> TmpLstCons = new List<liConsProveedor>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        liConsProveedor TmpItem = new liConsProveedor(); //pedConsPedidosEtiF
                        TmpItem.CodProveedor = Convert.ToString(item["CodProveedor"]);
                        TmpItem.Ruc = Convert.ToString(item["RUC"]);
                        TmpItem.RazonSocial = Convert.ToString(item["NomComercial"]);
                        

                        TmpLstCons.Add(TmpItem);
                    }
                    FormResponse.root.Add(TmpLstCons);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        [AntiForgeryValidate]
        [ActionName("ConsUsuarios")]
        [HttpGet]
        public formResponseLinea ConsUsuarios(String criterioConsulta)
        {
            formResponseLinea FormResponse = new formResponseLinea();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                xmlParam.LoadXml(criterioConsulta);
                ds = objEjecucion.EjecucionGralLinea(xmlParam.OuterXml, 702, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<liConsUsuarios> TmpLstCons = new List<liConsUsuarios>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        liConsUsuarios TmpItem = new liConsUsuarios(); //pedConsPedidosEtiF
                        TmpItem.Apellido1 = Convert.ToString(item["Apellido1"]);
                        TmpItem.Apellido2 = Convert.ToString(item["Apellido2"]);
                        TmpItem.Nombre1 = Convert.ToString(item["Nombre1"]);
                        TmpItem.Nombre2 = Convert.ToString(item["Nombre2"]);
                        TmpItem.Ruc = Convert.ToString(item["Ruc"]);
                        TmpItem.IdTipoLogin = Convert.ToInt32(item["IdTipoLogin"]);
                        TmpItem.DescripcionRol = Convert.ToString(item["DescripcionRol"]);
                        TmpItem.NumIdent = Convert.ToString(item["NumIdent"]);
                        TmpItem.IdUsuario = Convert.ToString(item["IdUsuario"]);
                        TmpItem.Rol28 = Convert.ToInt32(item["28"]);
                        TmpItem.Rol29 = Convert.ToInt32(item["29"]);
                        TmpItem.Rol30 = Convert.ToInt32(item["30"]);
                        TmpItem.esIngresado = Convert.ToString(item["esIngresado"]);
                        TmpItem.Correo = Convert.ToString(item["correo"]);

                        TmpLstCons.Add(TmpItem);
                    }
                    FormResponse.root.Add(TmpLstCons);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        [AntiForgeryValidate]
        [ActionName("ConsLineasEmpleado")]
        [HttpGet]
        public formResponseLinea ConsLineasEmpleado(String criterioConsultaLinea)
        {
            formResponseLinea FormResponse = new formResponseLinea();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                xmlParam.LoadXml(criterioConsultaLinea);
                ds = objEjecucion.EjecucionGralLinea(xmlParam.OuterXml, 703, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<liConsEmpleLinea> TmpLstCons = new List<liConsEmpleLinea>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        liConsEmpleLinea TmpItem = new liConsEmpleLinea(); //pedConsPedidosEtiF
                        TmpItem.IdEmpresa = Convert.ToInt32(item["IdEmpresa"]);
                        TmpItem.Ruc = Convert.ToString(item["Ruc"]);
                        TmpItem.Usuario = Convert.ToString(item["Usuario"]);
                        TmpItem.Linea = Convert.ToString(item["Linea"]);


                        TmpLstCons.Add(TmpItem);
                    }
                    FormResponse.root.Add(TmpLstCons);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        [AntiForgeryValidate]
        [ActionName("ConsLineasProveedor")]
        [HttpGet]
        public formResponseLinea ConsLineasProveedor(String criterioConsultaLineaProveedor)
        {
            formResponseLinea FormResponse = new formResponseLinea();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                xmlParam.LoadXml(criterioConsultaLineaProveedor);
                ds = objEjecucion.EjecucionGralLinea(xmlParam.OuterXml, 711, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<liConsProveeLinea> TmpLstCons = new List<liConsProveeLinea>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        liConsProveeLinea TmpItem = new liConsProveeLinea(); //pedConsPedidosEtiF
                        TmpItem.CodProveedor = Convert.ToString(item["CodProveedor"]);
                        TmpItem.Linea = Convert.ToString(item["CodLineaNegocio"]);


                        TmpLstCons.Add(TmpItem);
                    }
                    FormResponse.root.Add(TmpLstCons);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }


        [AntiForgeryValidate]
        [ActionName("ConsToleranciaProveedor")]
        [HttpGet]
        public formResponseLinea ConsToleranciaProveedor(String criterioConsultaTolerancia)
        {
            formResponseLinea FormResponse = new formResponseLinea();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                xmlParam.LoadXml(criterioConsultaTolerancia);
                ds = objEjecucion.EjecucionGralLinea(xmlParam.OuterXml, 712, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<liConsProveeLinea> TmpLstCons = new List<liConsProveeLinea>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        liConsProveeLinea TmpItem = new liConsProveeLinea(); //pedConsPedidosEtiF
                        TmpItem.CodProveedor = Convert.ToString(item["CodProveedor"]);
                        TmpItem.Linea = Convert.ToString(item["CodLineaNegocio"]);


                        TmpLstCons.Add(TmpItem);
                    }
                    FormResponse.root.Add(TmpLstCons);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        [AntiForgeryValidate]
        [ActionName("MantenimientoTolerancia")]
        [HttpGet]
        public formResponseLinea getMantenimientoTolerancia(String CodProveedor, String Porcentaje, String Accion, String TipoPedido)
        {
            formResponseLinea FormResponse = new formResponseLinea();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);
                xmlParam.DocumentElement.SetAttribute("Porcentaje", Porcentaje);
                xmlParam.DocumentElement.SetAttribute("Accion", Accion);
                xmlParam.DocumentElement.SetAttribute("TipoPedido", TipoPedido);
                ds = objEjecucion.EjecucionGralLinea(xmlParam.OuterXml, 714, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
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
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }


        [AntiForgeryValidate]
        [ActionName("ConsProveedorTolerancia")]
        [HttpGet]
        public formResponseLinea ConsProveedorTolerancia(String criterioConsultaProveedorTolerancia)
        {
            formResponseLinea FormResponse = new formResponseLinea();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                xmlParam.LoadXml(criterioConsultaProveedorTolerancia);
                ds = objEjecucion.EjecucionGralLinea(xmlParam.OuterXml, 713, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<liConsProveedor> TmpLstCons = new List<liConsProveedor>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        liConsProveedor TmpItem = new liConsProveedor(); //pedConsPedidosEtiF
                        TmpItem.CodProveedor = Convert.ToString(item["CodProveedor"]);
                        TmpItem.Ruc = Convert.ToString(item["RUC"]);
                        TmpItem.RazonSocial = Convert.ToString(item["NomComercial"]);
                        TmpItem.Porcentaje = Convert.ToInt16(item["Porcentaje"]).ToString();
                        TmpItem.TipoPedido = Convert.ToString(item["TipoPedido"]).ToString();
                        TmpItem.CodElemento = Convert.ToString(item["CodElemento"]).ToString();
                        TmpLstCons.Add(TmpItem);
                    }
                    FormResponse.root.Add(TmpLstCons);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }


        [AntiForgeryValidate]
        [ActionName("ConsProveedorToleranciaNuevo")]
        [HttpGet]
        public HttpResponseMessage ConsProveedorToleranciaNuevo(String criterioConsultaProveedorReporte, string TipoReporte, string Usuario ,string tipoped)
        {
            formResponseLinea FormResponse = new formResponseLinea();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            HttpResponseMessage result = null;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                xmlParam.LoadXml(criterioConsultaProveedorReporte);
                ds = objEjecucion.EjecucionGralLinea(xmlParam.OuterXml, 713, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    List<liConsProveedor> TmpLstCons = new List<liConsProveedor>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        liConsProveedor TmpItem = new liConsProveedor(); //pedConsPedidosEtiF
                        TmpItem.CodProveedor = Convert.ToString(item["CodProveedor"]);
                        TmpItem.Ruc = Convert.ToString(item["RUC"]);
                        TmpItem.RazonSocial = Convert.ToString(item["NomComercial"]);
                        TmpItem.Porcentaje = Convert.ToInt16(item["Porcentaje"]).ToString();
                        TmpItem.TipoPedido = Convert.ToString(item["TipoPedido"]);
                        TmpLstCons.Add(TmpItem);
                    }


                    //Datos para reporte de packing list
                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("Usuario", System.Type.GetType("System.String"));
                    ct.Columns.Add("TipoReporte", System.Type.GetType("System.String"));

                    DataRow drowc = ct.NewRow();
                    drowc["Usuario"] = Usuario;
                    if (tipoped=="T")
                    {
                        drowc["TipoReporte"] = "";
                    }
                    if (tipoped == "F")
                    {
                        drowc["TipoReporte"] = "Pedidos Flow Reparto";
                    }
                    if (tipoped == "C")
                    {
                        drowc["TipoReporte"] = "Pedidos Cross";
                    }
                    ct.Rows.Add(drowc);

                    DataTable dt = new DataTable("Detalle");
                    dt.Columns.Add("CodProveedor", System.Type.GetType("System.String"));
                    dt.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                    dt.Columns.Add("RUC", System.Type.GetType("System.String"));
                    dt.Columns.Add("Porcentaje", System.Type.GetType("System.String"));
                    dt.Columns.Add("TipoPedido", System.Type.GetType("System.String"));


                    foreach (var item in TmpLstCons)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["CodProveedor"] = item.CodProveedor;
                        drowd["NomProveedor"] = item.RazonSocial;
                        drowd["RUC"] = item.Ruc;
                        drowd["Porcentaje"] = item.Porcentaje;
                        drowd["TipoPedido"] = item.TipoPedido;
                        dt.Rows.Add(drowd);
                    }

                    rptDataSourcecab = new ReportDataSource("RPTToleranciaCab", ct);
                    rptDataSourcedet = new ReportDataSource("RPTToleranciaDet", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string archivo = "";
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteTolerancia.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    if (TipoReporte == "1")
                    {
                        archivo = "ReporteToleranciaProveedor" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".pdf";
                        bytes = auxc.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                    }
                    if (TipoReporte == "2")
                    {
                        archivo = "ReporteToleranciaProveedor" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xls";
                        bytes = auxc.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                    }

                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new MemoryStream(bytes));
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = archivo;

                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return result;
        }

        [AntiForgeryValidate]
        [ActionName("elimUsuario")]
        [HttpGet]
        public int ConsEmpleLinea(String criterioEliminar)
        {

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            int retorno = 0;
            try
            {
                xmlParam.LoadXml(criterioEliminar);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 703, 1);

                retorno = Convert.ToInt16(ds.Tables[0].Rows[0]["CodError"]);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return retorno;
        }

        [AntiForgeryValidate]
        [ActionName("elimProveedor")]
        [HttpGet]
        public int ConsProveedorLinea(String criterioEliminarProveedor)
        {

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            int retorno = 0;
            try
            {
                xmlParam.LoadXml(criterioEliminarProveedor);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 711, 1);

                retorno = Convert.ToInt16(ds.Tables[0].Rows[0]["CodError"]);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return retorno;
        }

        [AntiForgeryValidate]
        [ActionName("Mantenimiento")]
        [HttpGet]
        public int mantLinea(string criterioMantenimiento)
        {

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            int retorno = 0;
            try
            {
                xmlParam.LoadXml(criterioMantenimiento);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 703, 1);

                retorno = Convert.ToInt16(ds.Tables[0].Rows[0]["CodError"]);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return retorno;
        }

        [AntiForgeryValidate]
        [ActionName("MantenimientoProveedor")]
        [HttpGet]
        public int MantenimientoProveedor(string criterioMantenimientoProveedor)
        {

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            int retorno = 0;
            try
            {
                xmlParam.LoadXml(criterioMantenimientoProveedor);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 711, 1);

                retorno = Convert.ToInt16(ds.Tables[0].Rows[0]["CodError"]);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return retorno;
        }

        [AntiForgeryValidate]
        [ActionName("GeneraReporteTolerancia")]
        [HttpGet]
        public HttpResponseMessage GetGeneraReporteTolerancia(string Usuario, string TipoReporte)
        {
            HttpResponseMessage result = null;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();

            try
            {

                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root CRITERIO='T' NOMBRE='' RUC='' />");
                
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 713, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    List<liConsProveedor> TmpLstCons = new List<liConsProveedor>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        liConsProveedor TmpItem = new liConsProveedor(); //pedConsPedidosEtiF
                        TmpItem.CodProveedor = Convert.ToString(item["CodProveedor"]);
                        TmpItem.Ruc = Convert.ToString(item["RUC"]);
                        TmpItem.RazonSocial = Convert.ToString(item["NomComercial"]);
                        TmpItem.Porcentaje = Convert.ToInt16(item["Porcentaje"]).ToString();
                        TmpItem.TipoPedido = Convert.ToString(item["TipoPedido"]);
                        TmpLstCons.Add(TmpItem);
                    }                   
                    

                    //Datos para reporte de packing list
                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("Usuario", System.Type.GetType("System.String"));
                    

                    DataRow drowc = ct.NewRow();
                    drowc["Usuario"] = Usuario;
                  
                    ct.Rows.Add(drowc);

                    DataTable dt = new DataTable("Detalle");
                    dt.Columns.Add("CodProveedor", System.Type.GetType("System.String"));
                    dt.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                    dt.Columns.Add("RUC", System.Type.GetType("System.String"));
                    dt.Columns.Add("Porcentaje", System.Type.GetType("System.String"));
                    dt.Columns.Add("TipoPedido", System.Type.GetType("System.String"));
                   

                    foreach (var item in TmpLstCons)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["CodProveedor"] = item.CodProveedor;
                        drowd["NomProveedor"] = item.RazonSocial;
                        drowd["RUC"] = item.Ruc;
                        drowd["Porcentaje"] = item.Porcentaje;
                        drowd["TipoPedido"] = item.TipoPedido;
                        dt.Rows.Add(drowd);
                    }

                    rptDataSourcecab = new ReportDataSource("RPTToleranciaCab", ct);
                    rptDataSourcedet = new ReportDataSource("RPTToleranciaDet", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string archivo = "";
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteTolerancia.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    if (TipoReporte == "1")
                    {
                        archivo = "ReporteToleranciaProveedor" + DateTime.Now.ToString("yyyyMMddhhmmssfff")  + ".pdf";
                        bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }
                    if (TipoReporte == "2")
                    {
                        archivo = "ReporteToleranciaProveedor" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xls";
                        bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
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


    }
}
