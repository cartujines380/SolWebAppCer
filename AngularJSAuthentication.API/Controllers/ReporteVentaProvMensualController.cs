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
    [RoutePrefix("api/ReporteVentaProvMensual")]
    public class ReporteVentaProvMensualController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("consReporteConsultaVentaProvMesual")]
        [HttpPost]
        public formResponsePedidos consReporteConsultaVentaProvMesual(Gra_reporteEnviar datoscentro)
        {
            String[] meses = new String[]{ "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            repVentaxCentro mod_SolicitudVentaCentro;
            repcodigoArticulo mod_codigoImagen;
            griArticulos mod_Articulo;
            griArticulosBase modBaseArtciculo;
            List<repVentaxCentro> lst_retornoVentaCentro = new List<repVentaxCentro>();
            List<repcodigoArticulo> lst_codigoImagen = new List<repcodigoArticulo>();
            List<griArticulos> lst_mod_Articulo = new List<griArticulos>();
            List<griArticulosBase> lst_mod_BaseArticulo = new List<griArticulosBase>();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("FechaDesde", datoscentro.p_reporteDatos.Fecha1);
                xmlParam.DocumentElement.SetAttribute("FechaHasta", datoscentro.p_reporteDatos.Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodSAP", (datoscentro.p_reporteDatos.CodSap == null ? "" : datoscentro.p_reporteDatos.CodSap));
                foreach (var it in datoscentro.p_reporteAlmacen)
                {
                    XmlElement elem = xmlParam.CreateElement("Almacen");
                    elem.SetAttribute("CodAlmacen", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                }

                foreach (var it in datoscentro.p_reporteMaterial)
                {
                    XmlElement elem = xmlParam.CreateElement("Articulo");
                    elem.SetAttribute("CodArticulo", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 720, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudVentaCentro = new repVentaxCentro();
                        mod_SolicitudVentaCentro.CodCentro = Convert.ToString(item["CodCentro"]);
                        mod_SolicitudVentaCentro.NomAlmacen = Convert.ToString(item["NomAlmacen"]);
                        mod_SolicitudVentaCentro.CodMaterial = Convert.ToString(item["CodMaterial"]);
                        mod_SolicitudVentaCentro.DesMaterial = Convert.ToString(item["DesMaterial"]);
                        mod_SolicitudVentaCentro.CantVendida = Convert.ToString(item["CantVendida"]);
                        mod_SolicitudVentaCentro.mes = meses[Convert.ToInt16(datoscentro.p_reporteDatos.Fecha1.Split('/')[1]) - 1];
                        mod_SolicitudVentaCentro.anio = datoscentro.p_reporteDatos.Fecha1.Split('/')[2];
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
                        mod_Articulo = new griArticulos();
                        mod_Articulo.codArticulo = Convert.ToString(item["CodArticulo"]);
                        mod_Articulo.desArticulo = Convert.ToString(item["DesMaterial"]);
                        lst_mod_Articulo.Add(mod_Articulo);
                    }
                    foreach (DataRow item in ds.Tables[3].Rows)
                    {
                        modBaseArtciculo = new griArticulosBase();
                        modBaseArtciculo.codAlmacen = Convert.ToString(item["CodAlmacen"]);
                        modBaseArtciculo.nomAlmacen = Convert.ToString(item["NomAlmacen"]);
                        modBaseArtciculo.cantidad = Convert.ToString(item["CANTIDAD"]);
                        modBaseArtciculo.articulo = Convert.ToString(item["articulo"]);
                        lst_mod_BaseArticulo.Add(modBaseArtciculo);
                    }
                    FormResponse.root.Add(lst_retornoVentaCentro);
                    FormResponse.root.Add(lst_codigoImagen);
                    FormResponse.root.Add(lst_mod_Articulo);
                    FormResponse.root.Add(lst_mod_BaseArticulo);
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
