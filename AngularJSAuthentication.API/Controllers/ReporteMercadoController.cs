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
    [RoutePrefix("api/ReporteMercado")]
    public class ReporteMercadoController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("consReporteMercado")]
        [HttpPost]
        public formResponsePedidos consReporteEvolucion(Gra_reporteMercado datosMercado)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            repVentaMecado mod_SolicitudVentaCentro;
            merCatalogo mod_catalogo;
            List<repVentaMecado> lst_retornoVentaCentro = new List<repVentaMecado>();
            List<merCatalogo> lst_mod_catalogo1 = new List<merCatalogo>();
            List<merCatalogo> lst_mod_catalogo2 = new List<merCatalogo>();
            List<merCatalogo> lst_mod_catalogo3 = new List<merCatalogo>();
            List<merCatalogo> lst_mod_catalogo4 = new List<merCatalogo>();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("TipoLista", datosMercado.p_reporteDatos.TipoLista);
                xmlParam.DocumentElement.SetAttribute("Linea", datosMercado.p_reporteDatos.Linea);
                xmlParam.DocumentElement.SetAttribute("Seccion", datosMercado.p_reporteDatos.Seccion);
                xmlParam.DocumentElement.SetAttribute("SubSeccion", datosMercado.p_reporteDatos.SubSeccion);
                xmlParam.DocumentElement.SetAttribute("Grupo", datosMercado.p_reporteDatos.Grupo);
                xmlParam.DocumentElement.SetAttribute("FechaDesde", datosMercado.p_reporteDatos.Fecha1);
                xmlParam.DocumentElement.SetAttribute("FechaHasta", datosMercado.p_reporteDatos.Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodSAP", (datosMercado.p_reporteDatos.CodSap == null ? "" : datosMercado.p_reporteDatos.CodSap));
                foreach (var it in datosMercado.p_reporteAlmacen)
                {
                    XmlElement elem = xmlParam.CreateElement("Almacen");
                    elem.SetAttribute("CodAlmacen", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                }

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 722, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (datosMercado.p_reporteDatos.TipoLista == "1")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_catalogo = new merCatalogo();
                            mod_catalogo.codigo = Convert.ToString(item["Codigo"]);
                            mod_catalogo.descripcion = Convert.ToString(item["Descripcion"]);
                            lst_mod_catalogo1.Add(mod_catalogo);
                        }
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            mod_catalogo = new merCatalogo();
                            mod_catalogo.codigo = Convert.ToString(item["Codigo"]);
                            mod_catalogo.descripcion = Convert.ToString(item["Descripcion"]);
                            mod_catalogo.dePendencia = Convert.ToString(item["DePendencia"]);
                            lst_mod_catalogo2.Add(mod_catalogo);
                        }
                        foreach (DataRow item in ds.Tables[2].Rows)
                        {
                            mod_catalogo = new merCatalogo();
                            mod_catalogo.codigo = Convert.ToString(item["Codigo"]);
                            mod_catalogo.descripcion = Convert.ToString(item["Descripcion"]);
                            mod_catalogo.dePendencia = Convert.ToString(item["DePendencia"]);
                            lst_mod_catalogo3.Add(mod_catalogo);
                        }
                        foreach (DataRow item in ds.Tables[3].Rows)
                        {
                            mod_catalogo = new merCatalogo();
                            mod_catalogo.codigo = Convert.ToString(item["Codigo"]);
                            mod_catalogo.descripcion = Convert.ToString(item["Descripcion"]);
                            mod_catalogo.dePendencia = Convert.ToString(item["DePendencia"]);
                            lst_mod_catalogo4.Add(mod_catalogo);
                        }

                        foreach (DataRow item in ds.Tables[4].Rows)
                        {
                            mod_SolicitudVentaCentro = new repVentaMecado();
                            mod_SolicitudVentaCentro.codLinea = Convert.ToString(item["CodLinea"]);
                            mod_SolicitudVentaCentro.linea = Convert.ToString(item["Linea"]);
                            mod_SolicitudVentaCentro.porLinea = Convert.ToString(item["PorLinea"]);
                            mod_SolicitudVentaCentro.codSeccion = Convert.ToString(item["CodSeccion"]);
                            mod_SolicitudVentaCentro.seccion = Convert.ToString(item["Seccion"]);
                            mod_SolicitudVentaCentro.porSeccion = Convert.ToString(item["porSeccion"]);
                            mod_SolicitudVentaCentro.codSubseccion = Convert.ToString(item["CodSubseccion"]);
                            mod_SolicitudVentaCentro.subSeccion = Convert.ToString(item["SubSeccion"]);
                            mod_SolicitudVentaCentro.porSubSeccion = Convert.ToString(item["PorSubSeccion"]);
                            mod_SolicitudVentaCentro.codGrupo = Convert.ToString(item["codGrupo"]);
                            mod_SolicitudVentaCentro.grupo = Convert.ToString(item["Grupo"]);
                            mod_SolicitudVentaCentro.porGrupo = Convert.ToString(item["PorGrupo"]);

                            lst_retornoVentaCentro.Add(mod_SolicitudVentaCentro);
                        }
                        

                        FormResponse.root.Add(lst_mod_catalogo1);
                        FormResponse.root.Add(lst_mod_catalogo2);
                        FormResponse.root.Add(lst_mod_catalogo3);
                        FormResponse.root.Add(lst_mod_catalogo4);
                        FormResponse.root.Add(lst_retornoVentaCentro);
                    }else
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolicitudVentaCentro = new repVentaMecado();
                            mod_SolicitudVentaCentro.codLinea = Convert.ToString(item["CodLinea"]);
                            mod_SolicitudVentaCentro.linea = Convert.ToString(item["Linea"]);
                            mod_SolicitudVentaCentro.porLinea = Convert.ToString(item["PorLinea"]);
                            mod_SolicitudVentaCentro.codSeccion = Convert.ToString(item["CodSeccion"]);
                            mod_SolicitudVentaCentro.seccion = Convert.ToString(item["Seccion"]);
                            mod_SolicitudVentaCentro.porSeccion = Convert.ToString(item["porSeccion"]);
                            mod_SolicitudVentaCentro.codSubseccion = Convert.ToString(item["CodSubseccion"]);
                            mod_SolicitudVentaCentro.subSeccion = Convert.ToString(item["SubSeccion"]);
                            mod_SolicitudVentaCentro.porSubSeccion = Convert.ToString(item["PorSubSeccion"]);
                            mod_SolicitudVentaCentro.codGrupo = Convert.ToString(item["codGrupo"]);
                            mod_SolicitudVentaCentro.grupo = Convert.ToString(item["Grupo"]);
                            mod_SolicitudVentaCentro.porGrupo = Convert.ToString(item["PorGrupo"]);
                            lst_retornoVentaCentro.Add(mod_SolicitudVentaCentro);
                        }
                        FormResponse.root.Add(lst_retornoVentaCentro);

                    }

                    //Bandeja de Solicitudes
              
                   
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
