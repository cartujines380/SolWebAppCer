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
    [RoutePrefix("api/ReporteEstadisticosArticulo")]
    public class ReporteEstadisticosArticuloController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("consReporteConsultaArticulo")]
        [HttpPost]
        public formResponsePedidos consReporteConsultaArticulo(Gra_reporteEnviar datosArt)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            repArticuloMese mod_SolicitudVentaCentro;
            repcodigoArticulo mod_codigoImagen;
            repcentro mod_centro;
            List<repArticuloMese> lst_retornoVentaCentro = new List<repArticuloMese>();
            List<repcodigoArticulo> lst_codigoImagen = new List<repcodigoArticulo>();
            List<repcentro> lst_mod_centro = new List<repcentro>();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("FechaDesde", datosArt.p_reporteDatos.Fecha1);
                xmlParam.DocumentElement.SetAttribute("FechaHasta", datosArt.p_reporteDatos.Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodSAP", (datosArt.p_reporteDatos.CodSap == null ? "" : datosArt.p_reporteDatos.CodSap));
                foreach (var it in datosArt.p_reporteAlmacen)
                {
                    XmlElement elem = xmlParam.CreateElement("Almacen");
                    elem.SetAttribute("CodAlmacen", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                }

                foreach (var it in datosArt.p_reporteMaterial)
                {
                    XmlElement elem = xmlParam.CreateElement("Articulo");
                    elem.SetAttribute("CodArticulo", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 718, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudVentaCentro = new repArticuloMese();
                        mod_SolicitudVentaCentro.anio = Convert.ToString(item["anio"]);
                        mod_SolicitudVentaCentro.mes = Convert.ToString(item["mes"]);
                        mod_SolicitudVentaCentro.codMaterial = Convert.ToString(item["codMaterial"]);
                        mod_SolicitudVentaCentro.desMaterial = Convert.ToString(item["desMaterial"]);
                        mod_SolicitudVentaCentro.cantVendida = Convert.ToString(item["cantVendida"]);

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
                        lst_mod_centro.Add(mod_centro);
                    }
                    FormResponse.root.Add(lst_retornoVentaCentro);
                    FormResponse.root.Add(lst_codigoImagen);
                    FormResponse.root.Add(lst_mod_centro);
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