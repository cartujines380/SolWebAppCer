using AngularJSAuthentication.API.Models;
using clibProveedores;
using clibProveedores.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/ReqAdjudica")]
    public class ReqAdjudicaController : ApiController
    {

        [ActionName("getConsultarRequerimientoAju")]
        [HttpGet]
        public formResponsePedidos getConsultarRequerimiento(string fechadesdeA, string fechahastaA, string codrequerimientoA, string fechadesdeFA, string fechahastaFA)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repAdjudicacion> lstEmpresas = new List<repAdjudicacion>();
            repAdjudicacion mod_empresas = new repAdjudicacion();

            List<listArchivo> mod_archivo = new List<listArchivo>();
            listArchivo lstArchivo = new listArchivo();

            List<listEmpresa> mod_empesa = new List<listEmpresa>();
            listEmpresa lstEmpresa = new listEmpresa();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "BRLI");
                if (fechadesdeA == "undefined" || fechadesdeA == null)
                {
                    xmlParam.DocumentElement.SetAttribute("fechadesde", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("fechadesde", fechadesdeA);
                }
                if (fechahastaA == "undefined" || fechahastaA == null)
                {
                    xmlParam.DocumentElement.SetAttribute("fechahasta", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("fechahasta", fechahastaA);
                }
                if (fechadesdeFA == "undefined" || fechadesdeFA == null)
                {
                    xmlParam.DocumentElement.SetAttribute("fechadesdeF", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("fechadesdeF", fechadesdeFA);
                }
                if (fechahastaFA == "undefined" || fechahastaFA == null)
                {
                    xmlParam.DocumentElement.SetAttribute("fechahastaF", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("fechahastaF", fechahastaFA);
                }

                if (codrequerimientoA == "undefined" || codrequerimientoA == null)
                {
                    xmlParam.DocumentElement.SetAttribute("cod", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("cod", codrequerimientoA);
                }

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_empresas = new repAdjudicacion();
                        mod_empresas.codRequerimiento = Convert.ToString(item["codRequerimiento"]);
                        mod_empresas.desRequerimiento = Convert.ToString(item["desRequerimiento"]);
                        mod_empresas.fe_empieza = Convert.ToString(item["fe_empieza"]);
                        mod_empresas.fe_exp = Convert.ToString(item["fe_exp"]);
                        mod_empresas.ho_exp = Convert.ToString(item["ho_exp"]);
                        mod_empresas.feRequerimiento = Convert.ToString(item["feRequerimiento"]);
                        mod_empresas.montoRequerimiento = Convert.ToString(item["montoRequerimiento"]);
                        mod_empresas.empRequerimiento = Convert.ToString(item["empRequerimiento"]);
                        mod_empresas.nombre = Convert.ToString(item["nombre"]);

                        lstEmpresas.Add(mod_empresas);
                    }
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        lstArchivo = new listArchivo();
                        lstArchivo.id = Convert.ToString(item["id"]);
                        lstArchivo.descripcion = Convert.ToString(item["descripcion"]);
                        lstArchivo.archivo = Convert.ToString(item["nombre"]);
                        mod_archivo.Add(lstArchivo);
                    }
                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        lstEmpresa = new listEmpresa();
                        lstEmpresa.id = Convert.ToString(item["id"]);
                        lstEmpresa.empresa = Convert.ToString(item["empresa"]);
                        lstEmpresa.monto = Convert.ToString(item["monto"]);
                        lstEmpresa.codproveedor = Convert.ToString(item["codproveedor"]);
                        mod_empesa.Add(lstEmpresa);
                    }

                    FormResponse.root.Add(lstEmpresas);
                    FormResponse.root.Add(mod_archivo);
                    FormResponse.root.Add(mod_empesa);
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
        [ActionName("getEliminarLici")]
        [HttpGet]
        public formResponsePedidos getEliminarLici(string idEliLici)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            List<listArchivo> archivoGeneral = new List<listArchivo>();
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "ELILI");
                xmlParam.DocumentElement.SetAttribute("id", idEliLici);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    ProcesoWs.ServBaseProceso proc = new ProcesoWs.ServBaseProceso();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        CorreoNotificacion objCorreo = new CorreoNotificacion();

                        var titulo = Convert.ToString(item["titulo"]);
                        var nombreComercial = Convert.ToString(item["NomComercial"]);
                        var mensaje = Convert.ToString(item["mensaje"]);
                        var correo = Convert.ToString(item["correo"]);

                        objCorreo.NotificacionLicitacion(titulo, nombreComercial, mensaje, correo, "PL");
                    }
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

        [ActionName("getUpdateAjudicacion")]
        [HttpGet]
        public formResponsePedidos getUpdateAjudicacion(string codEmpresaAju, string idrAju)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repEmpresas> lstEmpresas = new List<repEmpresas>();
            repEmpresas mod_empresas = new repEmpresas();
            xmlParam.LoadXml("<Root />");
            List<listArchivo> archivoGeneral = new List<listArchivo>();
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "UREF");
                xmlParam.DocumentElement.SetAttribute("codEmpresa", codEmpresaAju);
                xmlParam.DocumentElement.SetAttribute("id", idrAju);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    ProcesoWs.ServBaseProceso proc = new ProcesoWs.ServBaseProceso();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        CorreoNotificacion objCorreo = new CorreoNotificacion();

                        var titulo = Convert.ToString(item["titulo"]);
                        var nombreComercial = Convert.ToString(item["NomComercial"]);
                        var mensaje = Convert.ToString(item["mensaje"]);
                        var correo = Convert.ToString(item["correo"]);

                        objCorreo.NotificacionLicitacion(titulo, nombreComercial, mensaje, correo, "PL");
                    }
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
