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
    [RoutePrefix("api/ReporteEstadisticos")]
    public class ReporteEstadisticosController : ApiController
    {

        [AntiForgeryValidate]
        [ActionName("ConsAlmacenesArticulosReportes")]
        [HttpGet]
        public formResponsePedidos GetConsAlmacenes(String tipoLista, string codSap)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                var xml = "<Root> <SecNotificacion  TipoLista=\"" + tipoLista + "\"  CodigoSap=\"" + codSap + "\" /> </Root>";
                ds = objEjecucion.EjecucionGralDs(xml, 715, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<pedAlmacenes> TmpList = new List<pedAlmacenes>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedAlmacenes TmpItem = new pedAlmacenes();
                        TmpItem.pCodAlmacen = Convert.ToString(item["CodAlmacen"]);
                        TmpItem.pNomAlmacen = Convert.ToString(item["NomAlmacen"]);
                        TmpList.Add(TmpItem);
                    }
                    FormResponse.root.Add(TmpList);
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
        [ActionName("GetAnios")]
        [HttpGet]
        public formResponsePedidos GetAnios(String tipoListacod, string codSapproveedor)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                var xml = "<Root> <SecNotificacion  TipoLista=\"" + tipoListacod + "\"  CodigoSap=\"" + codSapproveedor + "\" /> </Root>";
                ds = objEjecucion.EjecucionGralDs(xml, 715, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<pedAnios> TmpList = new List<pedAnios>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedAnios TmpItem = new pedAnios();
                        TmpItem.codigo = Convert.ToString(item["codigo"]);
                        TmpItem.descripcion = Convert.ToString(item["descripcion"]);
                        TmpList.Add(TmpItem);
                    }
                    FormResponse.root.Add(TmpList);
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
        [ActionName("consReporteConsulta")]
        [HttpPost]
        public formResponsePedidos consReporteConsulta(Gra_reporteEnviar datos)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            repVentaxCentro mod_SolicitudVentaCentro;
            repcodigoArticulo mod_codigoImagen;
            repcentro mod_centro;
            List<repVentaxCentro> lst_retornoVentaCentro = new List<repVentaxCentro>();
            List<repVentaxCentro> lst_retornoVentaGeneral = new List<repVentaxCentro>();
            List<repcodigoArticulo> lst_codigoImagen = new List<repcodigoArticulo>();
            List<repcentro> lst_mod_centro = new List<repcentro>();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("FechaDesde", datos.p_reporteDatos.Fecha1);
                xmlParam.DocumentElement.SetAttribute("FechaHasta", datos.p_reporteDatos.Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodSAP", (datos.p_reporteDatos.CodSap == null ? "" : datos.p_reporteDatos.CodSap));
                foreach (var it in datos.p_reporteAlmacen)
                    {
                            XmlElement elem = xmlParam.CreateElement("Almacen");
                            elem.SetAttribute("CodAlmacen", it.id);
                            xmlParam.DocumentElement.AppendChild(elem);
                    }

                    foreach (var it in datos.p_reporteMaterial)
                    {
                            XmlElement elem = xmlParam.CreateElement("Articulo");
                            elem.SetAttribute("CodArticulo", it.id);
                            xmlParam.DocumentElement.AppendChild(elem);
                    }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 716, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudVentaCentro = new repVentaxCentro();
                        mod_SolicitudVentaCentro.fecha = Convert.ToString(item["fecha"]);
                        mod_SolicitudVentaCentro.CodCentro = Convert.ToString(item["CodCentro"]);
                        mod_SolicitudVentaCentro.NomAlmacen = Convert.ToString(item["NomAlmacen"]);
                        mod_SolicitudVentaCentro.CodMaterial = Convert.ToString(item["CodMaterial"]);
                        mod_SolicitudVentaCentro.DesMaterial = Convert.ToString(item["DesMaterial"]);
                        mod_SolicitudVentaCentro.CantVendida = Convert.ToString(item["CantVendida"]);
                        mod_SolicitudVentaCentro.UnidadVenta = Convert.ToString(item["UnidadVenta"]);

                        lst_retornoVentaCentro.Add(mod_SolicitudVentaCentro);
                    }
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        mod_codigoImagen = new repcodigoArticulo();
                        mod_codigoImagen.CodMaterial = Convert.ToString(item["CodMaterial"]);
                        mod_codigoImagen.DesMaterial = Convert.ToString(item["DesMaterial"]);

                        lst_codigoImagen.Add(mod_codigoImagen);
                    }

                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        mod_centro = new repcentro();
                        mod_centro.Centro = Convert.ToString(item["CodCentro"]);
                        mod_centro.Almacen = Convert.ToString(item["NomAlmacen"]);
                        lst_mod_centro.Add(mod_centro);
                    }
                    foreach (DataRow item in ds.Tables[3].Rows)
                    {
                        mod_SolicitudVentaCentro = new repVentaxCentro();
                        mod_SolicitudVentaCentro.CodCentro = Convert.ToString(item["CodCentro"]);
                        mod_SolicitudVentaCentro.NomAlmacen = Convert.ToString(item["NomAlmacen"]);
                        mod_SolicitudVentaCentro.CodMaterial = Convert.ToString(item["CodMaterial"]);
                        mod_SolicitudVentaCentro.DesMaterial = Convert.ToString(item["DesMaterial"]);
                        mod_SolicitudVentaCentro.CantVendida = Convert.ToString(item["CantVendida"]);
                        mod_SolicitudVentaCentro.UnidadVenta = Convert.ToString(item["UnidadVenta"]);

                        lst_retornoVentaGeneral.Add(mod_SolicitudVentaCentro);
                    }
                    FormResponse.root.Add(lst_retornoVentaCentro);
                    FormResponse.root.Add(lst_codigoImagen);
                    FormResponse.root.Add(lst_mod_centro);
                    FormResponse.root.Add(lst_retornoVentaGeneral);
                    FormResponse.success = true;
                    FormResponse.msgError = "";
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.msgError = ex.Message.ToString();
            }
            return FormResponse;
        }


    

    }
}
