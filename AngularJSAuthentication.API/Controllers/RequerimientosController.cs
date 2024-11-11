using clibProveedores;
using clibProveedores.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/Requerimientos")]
    [Authorize]
    public class RequerimientosController : ApiController
    {
        [ActionName("getEmpresas")]
        [HttpGet]
        public formResponsePedidos getEmpresas(string tipoEmpresas)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repEmpresas> lstEmpresas = new List<repEmpresas>();
            repEmpresas mod_empresas = new repEmpresas();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipoEmpresas);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_empresas = new repEmpresas();
                        mod_empresas.codEmpresa = Convert.ToString(item["codEmpresa"]);
                        mod_empresas.descripcion = Convert.ToString(item["descripcion"]);

                        lstEmpresas.Add(mod_empresas);
                    }
                    FormResponse.root.Add(lstEmpresas);
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

        [ActionName("getCategoria")]
        [HttpGet]
        public formResponsePedidos getCategoria(string tipoCategoria)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repEmpresas> lstEmpresas = new List<repEmpresas>();
            repEmpresas mod_empresas = new repEmpresas();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipoCategoria);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_empresas = new repEmpresas();
                        mod_empresas.codEmpresa = Convert.ToString(item["codEmpresa"]);
                        mod_empresas.descripcion = Convert.ToString(item["descripcion"]);

                        lstEmpresas.Add(mod_empresas);
                    }
                    FormResponse.root.Add(lstEmpresas);
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

        [ActionName("getGrabarRequerimiento")]
        [HttpGet]
        public formResponsePedidos getGrabarRequerimiento(string fecha, string codEmpresa, string codCategoria, string monto, string descripcion, string usuario, string pageArchivo, string txttiulo)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repEmpresas> lstEmpresas = new List<repEmpresas>();
            repEmpresas mod_empresas = new repEmpresas();
            xmlParam.LoadXml("<Root />");
            List<listArchivo> archivoGeneral = new List<listArchivo>();
            listArchivo archivoI;
            string Mensaje = "";
            if (pageArchivo!="")
            {
                foreach (var itemt in pageArchivo.Split('|'))
                {
                    archivoI = new listArchivo();
                    archivoI.descripcion = itemt.Split(';')[0];
                    archivoI.archivo = itemt.Split(';')[1];
                    archivoGeneral.Add(archivoI);
                }
            }
      
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "GRE");
                xmlParam.DocumentElement.SetAttribute("fecha", fecha);
                xmlParam.DocumentElement.SetAttribute("codEmpresa", codEmpresa);
                xmlParam.DocumentElement.SetAttribute("codCategoria", codCategoria);
                xmlParam.DocumentElement.SetAttribute("monto", monto);
                xmlParam.DocumentElement.SetAttribute("descripcion", descripcion);
                xmlParam.DocumentElement.SetAttribute("usuario", usuario);
                xmlParam.DocumentElement.SetAttribute("titulo", txttiulo);

                foreach (var it in archivoGeneral)
                {
                    XmlElement elem = xmlParam.CreateElement("Archivo");
                    elem.SetAttribute("descripcion", it.descripcion);
                    elem.SetAttribute("archivo", it.archivo);
                    xmlParam.DocumentElement.AppendChild(elem);
                }


                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        Mensaje = Convert.ToString(item["MENSAJE"]);
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

        [ActionName("getUpdateRequerimiento")]
        [HttpGet]
        public formResponsePedidos getUpdateRequerimiento(string idu, string fechau, string codEmpresau, string codCategoriau, string montou, string descripcionu, string usuariou, string upageArchivo, string tituloup)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repEmpresas> lstEmpresas = new List<repEmpresas>();
            repEmpresas mod_empresas = new repEmpresas();
            xmlParam.LoadXml("<Root />");
            List<listArchivo> archivoGeneral = new List<listArchivo>();
            listArchivo archivoI;
            string Mensaje = idu;
            foreach (var itemt in upageArchivo.Split('|'))
            {
                archivoI = new listArchivo();
                archivoI.descripcion = itemt.Split(';')[0];
                archivoI.archivo = itemt.Split(';')[1];
                archivoGeneral.Add(archivoI);
            }
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "URE");
                xmlParam.DocumentElement.SetAttribute("id", idu);
                xmlParam.DocumentElement.SetAttribute("fecha", fechau);
                xmlParam.DocumentElement.SetAttribute("codEmpresa", codEmpresau);
                xmlParam.DocumentElement.SetAttribute("codCategoria", codCategoriau);
                xmlParam.DocumentElement.SetAttribute("monto", montou);
                xmlParam.DocumentElement.SetAttribute("descripcion", descripcionu);
                xmlParam.DocumentElement.SetAttribute("usuario", usuariou);
                xmlParam.DocumentElement.SetAttribute("titulo", tituloup);
                foreach (var it in archivoGeneral)
                {
                    XmlElement elem = xmlParam.CreateElement("Archivo");
                    elem.SetAttribute("descripcion", it.descripcion);
                    elem.SetAttribute("archivo", it.archivo);
                    xmlParam.DocumentElement.AppendChild(elem);
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
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

        [ActionName("getEliminar")]
        [HttpGet]
        public formResponsePedidos getEliminar(string idEli)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "ELI");
                xmlParam.DocumentElement.SetAttribute("id", idEli);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
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

        [ActionName("getSeleccionar")]
        [HttpGet]
        public formResponsePedidos getSeleccionar(string idSel)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repRequerimiento> lstEmpresas = new List<repRequerimiento>();
            repRequerimiento mod_empresas = new repRequerimiento();
            List<listArchivo> archivoGeneral = new List<listArchivo>();
            listArchivo archivoI;
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "BSEL");
                xmlParam.DocumentElement.SetAttribute("id", idSel);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_empresas = new repRequerimiento();
                        mod_empresas.id = Convert.ToString(item["id"]);
                        mod_empresas.fecha = Convert.ToString(item["fecha"]);
                        mod_empresas.categoria = Convert.ToString(item["categoria"]);
                        mod_empresas.empresa = Convert.ToString(item["empresa"]);
                        mod_empresas.descripcion = Convert.ToString(item["descripcion"]);
                        mod_empresas.monto = Convert.ToString(item["monto"]);
                        mod_empresas.titulo = Convert.ToString(item["titulo"]);

                        lstEmpresas.Add(mod_empresas);
                    }
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        archivoI = new listArchivo();
                        archivoI.descripcion = Convert.ToString(item["descripcion"]);
                        archivoI.archivo = Convert.ToString(item["nombrearchivo"]);
                        archivoGeneral.Add(archivoI);
                    }
                    FormResponse.root.Add(lstEmpresas);
                    FormResponse.root.Add(archivoGeneral);
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
        [ActionName("getConsultarRequerimiento")]
        [HttpGet]
        public formResponsePedidos getConsultarRequerimiento(string fechabuscar, string codEmpresabuscar, string codCategoriabuscar)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repRequerimiento> lstEmpresas = new List<repRequerimiento>();
            repRequerimiento mod_empresas = new repRequerimiento();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "BGRE");
                if (fechabuscar == "undefined" || fechabuscar == null)
                {
                    xmlParam.DocumentElement.SetAttribute("fecha", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("fecha", fechabuscar);
                }
                if (codEmpresabuscar == "undefined" || codEmpresabuscar == null)
                {
                    xmlParam.DocumentElement.SetAttribute("codEmpresa", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("codEmpresa", codEmpresabuscar);
                }
                if (codCategoriabuscar == "undefined" || codCategoriabuscar == null)
                {
                    xmlParam.DocumentElement.SetAttribute("codCategoria", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("codCategoria", codCategoriabuscar);
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_empresas = new repRequerimiento();
                        mod_empresas.id = Convert.ToString(item["id"]);
                        mod_empresas.fecha = Convert.ToString(item["fecha"]);
                        mod_empresas.categoria = Convert.ToString(item["categoria"]);
                        mod_empresas.empresa = Convert.ToString(item["empresa"]);
                        mod_empresas.descripcion = Convert.ToString(item["descripcion"]);
                        mod_empresas.estado = Convert.ToString(item["estado"]);

                        lstEmpresas.Add(mod_empresas);
                    }
                    FormResponse.root.Add(lstEmpresas);
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

        [ActionName("getConsultarRequerimientoPrvo")]
        [HttpGet]
        public formResponsePedidos getConsultarRequerimientoPrvo(string fechabuscarp, string codEmpresabuscarp, string codCategoriabuscarp,string codproveedor)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repRequerimiento> lstEmpresas = new List<repRequerimiento>();
            repRequerimiento mod_empresas = new repRequerimiento();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "BASI");
                xmlParam.DocumentElement.SetAttribute("Proveedor", codproveedor);
                if (fechabuscarp == "undefined" || fechabuscarp == null)
                {
                    xmlParam.DocumentElement.SetAttribute("fecha", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("fecha", fechabuscarp);
                }
                if (codEmpresabuscarp == "undefined" || codEmpresabuscarp == null)
                {
                    xmlParam.DocumentElement.SetAttribute("codEmpresa", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("codEmpresa", codEmpresabuscarp);
                }
                if (codCategoriabuscarp == "undefined" || codCategoriabuscarp == null)
                {
                    xmlParam.DocumentElement.SetAttribute("codCategoria", "");
                }
                else
                {
                    xmlParam.DocumentElement.SetAttribute("codCategoria", codCategoriabuscarp);
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 900, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_empresas = new repRequerimiento();
                        mod_empresas.id = Convert.ToString(item["id"]);
                        mod_empresas.fecha = Convert.ToString(item["fecha"]);
                        mod_empresas.categoria = Convert.ToString(item["categoria"]);
                        mod_empresas.empresa = Convert.ToString(item["empresa"]);
                        mod_empresas.descripcion = Convert.ToString(item["descripcion"]);
                        mod_empresas.estado = Convert.ToString(item["estado"]);

                        lstEmpresas.Add(mod_empresas);
                    }
                    FormResponse.root.Add(lstEmpresas);
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
