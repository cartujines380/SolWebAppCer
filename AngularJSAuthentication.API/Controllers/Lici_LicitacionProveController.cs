using AngularJSAuthentication.API.Handlers;
using AngularJSAuthentication.API.Models;
using clibProveedores;
using clibProveedores.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Xml;

namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Lici_LicitacionProve")]
    public class Lici_LicitacionProveController : ApiController
    {
        private string _clase;

        public Lici_LicitacionProveController()
        {
            _clase = GetType().Name;
        }

        [AntiForgeryValidate]
        [ActionName("getPubliProv")]
        [HttpGet]
        public FormResponse getPubliProv(string criterio, string feIni, string feFin, string req, string cat, string prov)
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
                xmlParam.DocumentElement.SetAttribute("CAT", cat);
                xmlParam.DocumentElement.SetAttribute("PROV", prov);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1102, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<publicacion> listaRetorno = new List<publicacion>();
                    List<_catalogo> listaProv = new List<_catalogo>();
                    List<documento> listaDocs = new List<documento>();
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        var doc = new documento();
                        //doc.idDoc = Convert.ToInt16(item["idadjunto"]);
                        doc.idDoc = int.Parse(item["idadjunto"].ToString());
                        doc.desc  = Convert.ToString(item["descripcion"]);
                        //doc.idReq = Convert.ToInt16(item["idrequerimiento"]);
                        doc.idReq = int.Parse(item["idrequerimiento"].ToString());
                        doc.archivo = Convert.ToString(item["nombreArchivo"]);
                        listaDocs.Add(doc);
                    }
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var pub = new publicacion();
                        //pub.idPubli = Convert.ToInt16(item["idPubli"]);
                        pub.idPubli = int.Parse(item["idPubli"].ToString());
                        //pub.idReq = Convert.ToInt16(item["idrequerimiento"]);
                        pub.idReq = int.Parse(item["idrequerimiento"].ToString());
                        //pub.version = (item["version"]==null)? 0: Convert.ToInt16(item["version"]);
                        pub.version = (item["version"]==null)? 0: int.Parse(item["version"].ToString());
                        pub.descripcion = Convert.ToString(item["descripcion"]);
                        pub.titulo = Convert.ToString(item["titulo"]);
                        pub.feIni = Convert.ToString(item["feIniLicitacion"]);
                        pub.feFin = Convert.ToString(item["feFinLicitacion"]);
                        pub.hoFin = Convert.ToString(item["hoFinLicitacion"]);
                        pub.estado = Convert.ToString(item["estadoLicitacion"]);
                        pub.descPubli = Convert.ToString(item["descripcionPub"]);
                        pub.monto = Convert.ToString(item["monto"]);
                        pub.nomPubli = Convert.ToString(item["nombrePub"]);
                        pub.documentos = listaDocs.Where(x=>x.idReq== pub.idReq);
                        listaRetorno.Add(pub);
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
        [ActionName("setParticipa")]
        [HttpGet]
        public FormResponse setParticipa(string idPubli, string prov)
        {
            var respuesta = new FormResponse();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("ACCION", "PA");
                xmlParam.DocumentElement.SetAttribute("IDPUBLI", idPubli);
                xmlParam.DocumentElement.SetAttribute("PROV", prov);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1102, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    respuesta.success = true;
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
        [ActionName("getParticipa")]
        [HttpGet]
        public FormResponse getParticipa(string prov)
        {
            var respuesta = new FormResponse();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("ACCION", "CP");
                xmlParam.DocumentElement.SetAttribute("PROV", prov);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1102, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<publicacion> listaRetorno = new List<publicacion>();
                    List<_catalogo> listaProv = new List<_catalogo>();
                    List<documento> listaDocs = new List<documento>();
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        var doc = new documento();
                        //doc.idDoc = Convert.ToInt16(item["idadjunto"]);
                        doc.idDoc = int.Parse(item["idadjunto"].ToString());
                        doc.desc = Convert.ToString(item["descripcion"]);
                        //doc.idReq = Convert.ToInt16(item["idrequerimiento"]);
                        doc.idReq = int.Parse(item["idrequerimiento"].ToString());
                        doc.archivo = Convert.ToString(item["nombreArchivo"]);
                        listaDocs.Add(doc);
                    }
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var pub = new publicacion();
                        //pub.idPubli = Convert.ToInt16(item["idPubli"]);
                        pub.idPubli = int.Parse(item["idPubli"].ToString());
                        //pub.idReq = Convert.ToInt16(item["idrequerimiento"]);
                        pub.idReq = int.Parse(item["idrequerimiento"].ToString());
                        //pub.version = (item["version"] == null) ? 0 : Convert.ToInt16(item["version"]);
                        pub.version = (item["version"] == null) ? 0 : int.Parse(item["version"].ToString());
                        pub.titulo = Convert.ToString(item["titulo"]);
                        pub.descripcion = Convert.ToString(item["descripcion"]);
                        pub.feIni = Convert.ToString(item["feIniLicitacion"]);
                        pub.feFin = Convert.ToString(item["feFinLicitacion"]);
                        pub.hoFin = Convert.ToString(item["hoFinLicitacion"]);
                        pub.estado = Convert.ToString(item["estadoLicitacion"]);
                        pub.estadoLicDesc = Convert.ToString(item["estadoLicDesc"]);
                        pub.descPubli = Convert.ToString(item["descripcionPub"]);
                        pub.estadoPublicado = Convert.ToString(item["estadopublicado"]);
                        pub.estadoParticipando = Convert.ToString(item["estadoparticipandp"]);
                        pub.estadoParticipandoDesc = Convert.ToString(item["estadoParticipandoDesc"]);
                        pub.monto = Convert.ToString(item["monto"]);
                        pub.nomPubli = Convert.ToString(item["nombrePub"]);
                        pub.documentos = listaDocs.Where(x => x.idReq == pub.idReq);
                        listaRetorno.Add(pub);
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
        [ActionName("getOferta")]
        [HttpGet]
        public FormResponse getOferta(string idOferta, string prov)
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
                xmlParam.DocumentElement.SetAttribute("PROV", prov);
                xmlParam.DocumentElement.SetAttribute("IDPUBLI", idOferta);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1102, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<_oferta> listaRetorno = new List<_oferta>();
                    List<documento> listaDocs = new List<documento>();
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        var doc = new documento();
                        //doc.idDoc = Convert.ToInt16(item["idadjunto"]);
                        doc.idDoc = int.Parse(item["idadjunto"].ToString());
                        doc.desc = Convert.ToString(item["descripcion"]);
                        doc.idOferta = Convert.ToInt32(item["idAdquisicion"]);
                        doc.codProveedor = Convert.ToString(item["codProveedor"]);
                        doc.archivo = Convert.ToString(item["nombreArchivo"]);
                        listaDocs.Add(doc);
                    }
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var oferta = new _oferta();
                        oferta.idOferta = Convert.ToInt32(item["idAdquisicion"]);
                        oferta.monto = Convert.ToString(item["monto"]);
                        oferta.fechaEnvio = Convert.ToString(item["fechaEnvio"]);
                        oferta.estado = Convert.ToString(item["estado"]);
                        //oferta.tiempoEjecucion = Convert.ToInt16(item["tiempoEjecucion"]);
                        oferta.tiempoEjecucion = int.Parse(item["tiempoEjecucion"].ToString());
                        oferta.codProveedor = Convert.ToString(item["codProveedor"]);
                        oferta.documentos = listaDocs;
                        listaRetorno.Add(oferta);
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
        [ActionName("guardaOferta")]
        [HttpPost]
        public FormResponse guardaOferta(string idOferta, string codProveedor)
        {
            var respuesta = new FormResponse();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            int retorno = 0;

            var nomdocs = System.Web.HttpContext.Current.Request.Form.GetValues("nomDoc");
            var queda = System.Web.HttpContext.Current.Request.Form.GetValues("queda");

            if (!subirDocs(System.Web.HttpContext.Current.Request.Files, idOferta + "_"+ codProveedor, ref xmlParam, nomdocs))
                return new FormResponse { success = false, codError = "-1", msgError = "Ocurrión un error al cargar los documentos" };

            try
            {
                xmlParam.DocumentElement.SetAttribute("ACCION", "GO");
                xmlParam.DocumentElement.SetAttribute("IDPUBLI", idOferta);
                xmlParam.DocumentElement.SetAttribute("PROV", codProveedor);
                
                if(queda!=null)
                    foreach (string item in queda)
                    {
                        var Docquda = xmlParam.CreateElement("Docqueda");
                        Docquda.SetAttribute("id", item);
                        xmlParam.DocumentElement.AppendChild(Docquda);
                    }

                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 1102, 1);

                retorno = Convert.ToInt32(ds.Tables[0].Rows[0]["retorno"]);
            }
            catch (Exception ex)
            {
                return new FormResponse { success = false, codError = "-1", msgError = ex.Message };
            }
            return new FormResponse { success = true }; ;
        }

        [AntiForgeryValidate]
        [ActionName("enviaOferta")]
        [HttpPost]
        public FormResponse enviaOferta(string idOferta, string codProveedor, string monto, string tiempo)
        {
            var respuesta = new FormResponse();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            int retorno = 0;

            var nomdocs = System.Web.HttpContext.Current.Request.Form.GetValues("nomDoc");
            var queda = System.Web.HttpContext.Current.Request.Form.GetValues("queda");

            if (!subirDocs(System.Web.HttpContext.Current.Request.Files, idOferta + "_" + codProveedor, ref xmlParam, nomdocs))
                return new FormResponse { success = false, codError = "-1", msgError = "Ocurrión un error al cargar los documentos" };

            try
            {
                xmlParam.DocumentElement.SetAttribute("ACCION", "EO");
                xmlParam.DocumentElement.SetAttribute("IDPUBLI", idOferta);
                xmlParam.DocumentElement.SetAttribute("PROV", codProveedor);
                xmlParam.DocumentElement.SetAttribute("MONT", monto);
                xmlParam.DocumentElement.SetAttribute("TIEM", tiempo);

                if (queda != null)
                    foreach (string item in queda)
                    {
                        var Docquda = xmlParam.CreateElement("Docqueda");
                        Docquda.SetAttribute("id", item);
                        xmlParam.DocumentElement.AppendChild(Docquda);
                    }

                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 1102, 1);

                retorno = Convert.ToInt32(ds.Tables[0].Rows[0]["retorno"]);
            }
            catch (Exception ex)
            {
                return new FormResponse { success = false, codError = "-1", msgError = ex.Message };
            }
            return new FormResponse { success = true }; ;
        }

        private bool subirDocs(System.Web.HttpFileCollection docs, string nombreLiq, ref XmlDocument xmlParam, string[] nomDocs)
        {
            var retorno = true;
            try
            {
                if (!Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/UploadedDocuments/Uploads/")))
                    Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/UploadedDocuments/Uploads/"));

                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/UploadedDocuments/Uploads/");

                for (int i = 0; i < docs.Count; i++)
                {
                    System.Web.HttpPostedFile hpf = docs[i];

                    if (hpf.ContentLength > 0)
                    {
                        var nombre = Path.GetFileName("LIC" + DateTime.Now.ToString("ddMMyyyyhhmmssffftt") + Path.GetExtension(System.Web.HttpUtility.HtmlDecode(hpf.FileName)));
                        System.Threading.Thread.Sleep(10);
                        var ruta = sPath + nombre;
                        var doc = xmlParam.CreateElement("doc");
                        doc.SetAttribute("nombre", nombre);
                        doc.SetAttribute("descDoc", nomDocs[i]);
                        hpf.SaveAs(ruta);
                        xmlParam.DocumentElement.AppendChild(doc);

                        EscribePDFAdjuntos("Uploads", nombre);
                    }
                }
            }
            catch(Exception)
            {
                retorno = false;
            }

            return retorno;
        }

        [ActionName("EscribePDFAdjuntos")]
        [Route("EscribePDFAdjuntos")]
        [HttpPost]
        public FormResponseModelo EscribePDFAdjuntos(string rutaDirectorio, string nombre)
        {
            string _metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            FormResponseModelo _oEscribePDFAdjuntos = new FormResponseModelo();
            try
            {
                string RutaLectura = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivo"]).Trim();
                RutaLectura = Path.Combine(RutaLectura, rutaDirectorio, nombre);

                p_Log.Graba_Log_Info("Ruta Adjunto: " + RutaLectura);

                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                Proceso.Url = ((string)System.Configuration.ConfigurationManager.AppSettings["Urlbase"]).Trim();
                Proceso.EscribePdfAdjunto(File.ReadAllBytes(RutaLectura), rutaDirectorio, nombre);

                _oEscribePDFAdjuntos.lSuccess = true;
                _oEscribePDFAdjuntos.cCodError = "0";
            }
            catch (Exception ex)
            {
                _oEscribePDFAdjuntos.lSuccess = false;
                _oEscribePDFAdjuntos.cCodError = "9999";
                _oEscribePDFAdjuntos.cMsgError = ex.Message.ToString();

                p_Log.Graba_Log_Error(ex.Message.ToString());
            }

            p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

            return _oEscribePDFAdjuntos;
        }
    }
}

