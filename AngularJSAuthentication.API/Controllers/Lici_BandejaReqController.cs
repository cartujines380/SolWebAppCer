using AngularJSAuthentication.API.Handlers;
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
    [Authorize]
    [RoutePrefix("api/Lici_BandejaReq")]
    public class Lici_BandejaReqController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("getReqs")]
        [HttpGet]
        public FormResponse getReqs(string criterio, string feIni, string feFin, string req, string empresa, string cat)
        {
            var respuesta = new FormResponse();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("ACCION", "CO");
                xmlParam.DocumentElement.SetAttribute("CRITERIO", criterio);
                xmlParam.DocumentElement.SetAttribute("FEINI", feIni);
                xmlParam.DocumentElement.SetAttribute("FEFIN", feFin);
                xmlParam.DocumentElement.SetAttribute("REQ", req);
                xmlParam.DocumentElement.SetAttribute("EMPRESA", empresa);
                xmlParam.DocumentElement.SetAttribute("CAT", cat);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1100, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<_requerimiento> listaRetorno = new List<_requerimiento>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var _req = new _requerimiento();
                        _req.idReq = Convert.ToInt32(item["idrequerimiento"]);
                        _req.fechaReq = Convert.ToString(item["fecharequerimiento"]);
                        _req.codCategoria = Convert.ToString(item["codcategoria"]);
                        _req.categoria = Convert.ToString(item["categoria"]);
                        _req.codEmpresa = Convert.ToString(item["codempresa"]);
                        _req.empresa = Convert.ToString(item["empresa"]);
                        _req.monto = Convert.ToString(item["monto"]);
                        _req.titulo = Convert.ToString(item["titulo"]);
                        _req.descripcion  = Convert.ToString(item["descripcion"]);
                        _req.estado = Convert.ToString(item["estado"]);
                        _req.usuarioCreacion = Convert.ToString(item["usuariocreacion"]);
                        _req.fechaCreacion = Convert.ToString(item["fechacreacion"]);
                        listaRetorno.Add(_req);
                    }
                    respuesta.success = true;
                    respuesta.root.Add(listaRetorno);
                }
                else
                {
                    respuesta.success = false;
                    respuesta.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.msgError = ex.Message;
            }
            return respuesta;
        }


        [AntiForgeryValidate]
        [ActionName("getCatalogo")]
        [HttpGet]
        public FormResponse getCatalogo(string criterio)
        {
            var respuesta = new FormResponse();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("ACCION", "CA");
                xmlParam.DocumentElement.SetAttribute("CRITERIO", criterio);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1100, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<_catalogo> listaRetorno = new List<_catalogo>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var catalog = new _catalogo();
                        catalog.id = Convert.ToString(item["id"]);
                        catalog.descripcion = Convert.ToString(item["descripcion"]);
                        listaRetorno.Add(catalog);
                    }
                    respuesta.success = true;
                    respuesta.root.Add(listaRetorno);
                }
                else
                {
                    respuesta.success = false;
                    respuesta.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta.success = false;
                respuesta.msgError = ex.Message;
            }
            return respuesta;
        }
        
        [AntiForgeryValidate]
        [ActionName("setDirecto")]
        [HttpGet]
        public int setDirecto(string req, string idProv, string idUsu)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            int retorno = 0;
            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("ACCION", "AD");
                xmlParam.DocumentElement.SetAttribute("REQ", req);
                xmlParam.DocumentElement.SetAttribute("IDPROV", idProv);
                xmlParam.DocumentElement.SetAttribute("USUARIO", idUsu);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 1100, 1);


                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //ProcesoWs.ServBaseProceso proc = new ProcesoWs.ServBaseProceso();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        CorreoNotificacion objCorreo = new CorreoNotificacion();
                        //var ret = proc.envioCorreoLici(Convert.ToString(item["titulo"]),
                        //                                    Convert.ToString(item["NomComercial"]),
                        //                                        Convert.ToString(item["mensaje"]),
                        //                                            Convert.ToString(item["correo"]));

                        objCorreo.NotificacionLicitacion(item["titulo"].ToString()
                                                        ,item["NomComercial"].ToString()
                                                        ,item["mensaje"].ToString()
                                                        ,item["correo"].ToString()
                                                        ,"PL");
                    }
                    retorno = 1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return retorno;
        }

        [AntiForgeryValidate]
        [ActionName("setLicitacion")]
        [HttpGet]
        public int setLicitacion(string req)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            int retorno = 0;
            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("ACCION", "LI");
                xmlParam.DocumentElement.SetAttribute("REQ", req);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 1100, 1);

                retorno = Convert.ToInt16(ds.Tables[0].Rows[0]["retorno"]);
            }
            catch (Exception)
            {
                return 0;
            }
            return retorno;
        }
    }
}

