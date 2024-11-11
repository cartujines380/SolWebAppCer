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
    [RoutePrefix("api/ReporteEvo")]
    public class ReporteEvoController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("consReporteEvolucion")]
        [HttpPost]
        public formResponsePedidos consReporteEvolucion(Gra_reporteEnviar datos)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            repVentaxCentro mod_SolicitudVentaCentro;
            repcentro mod_centro; 
            List<repVentaxCentro> lst_retornoVentaCentro = new List<repVentaxCentro>();
            List<griArticulos> lst_articulos = new List<griArticulos>();
            List<string> lstFechas = new List<string>();
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
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 719, 1);
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
                        lstFechas.Add(Convert.ToString(item["fecha"]));
                    }

                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        mod_centro = new repcentro();
                        mod_centro.Centro = Convert.ToString(item["NomAlmacen"]);
                        lst_mod_centro.Add(mod_centro);
                    }

                    foreach (DataRow item in ds.Tables[3].Rows)
                    {
                        griArticulos mod_articulo = new griArticulos();
                        mod_articulo.desArticulo = Convert.ToString(item["DesMaterial"]);
                        mod_articulo.codArticulo = Convert.ToString(item["CodMaterial"]);
                        lst_articulos.Add(mod_articulo);
                    }

                    FormResponse.root.Add(lst_retornoVentaCentro);
                    FormResponse.root.Add(lstFechas);
                    FormResponse.root.Add(lst_mod_centro);
                    FormResponse.root.Add(lst_articulos);
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
