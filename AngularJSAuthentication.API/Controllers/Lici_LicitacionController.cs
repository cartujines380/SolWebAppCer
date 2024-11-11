using AngularJSAuthentication.API.Handlers;
using AngularJSAuthentication.API.Models;
using clibProveedores;
using clibProveedores.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Xml;

namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Lici_Licitacion")]
    public class Lici_LicitacionController : ApiController
    {
        private string _clase;
        public Lici_LicitacionController()
        {
            _clase = GetType().Name;
        }

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

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1101, 1);
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
        [ActionName("pubLicitacion")]
        [HttpPost]
        public FormResponse pubLicitacion(string req, string idProvs, string nombre, string desc, string feIni, string feFin, string hoFin, string tipoInvitacion)
        {
            var respuesta = new FormResponse();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");

            var nomdocs = System.Web.HttpContext.Current.Request.Form.GetValues("nomDoc");
            var queda = System.Web.HttpContext.Current.Request.Form.GetValues("queda");

            if (!subirDocs(System.Web.HttpContext.Current.Request.Files, nombre.Replace(' ', '_'), ref xmlParam, nomdocs))
                return new FormResponse { success = false, codError = "-1", msgError = "Ocurrión un error al cargar los documentos" };

            try
            {
                xmlParam.DocumentElement.SetAttribute("ACCION", "PL");
                xmlParam.DocumentElement.SetAttribute("REQ", req);
                xmlParam.DocumentElement.SetAttribute("NOMBRE", nombre);
                xmlParam.DocumentElement.SetAttribute("DESC", desc);
                xmlParam.DocumentElement.SetAttribute("FEINI", feIni);
                xmlParam.DocumentElement.SetAttribute("FEFIN", feFin);
                xmlParam.DocumentElement.SetAttribute("HOFIN", hoFin);
                xmlParam.DocumentElement.SetAttribute("TIPO", (tipoInvitacion=="PRO")?"P":"C");

                foreach (string item in idProvs.Split(','))
                {
                    if (item.Trim() != "")
                    {
                        if (tipoInvitacion == "PRO")
                        {
                            var provs = xmlParam.CreateElement("provs");
                            provs.SetAttribute("codProv", item.Trim());
                            xmlParam.DocumentElement.AppendChild(provs);
                        }
                        else
                            if (tipoInvitacion == "CAT")
                            {
                                var provs = xmlParam.CreateElement("cats");
                                provs.SetAttribute("codCat", item.Trim());
                                xmlParam.DocumentElement.AppendChild(provs);
                            }
                    }
                }

                if (queda != null)
                    foreach (string item in queda)
                    {
                        var Docquda = xmlParam.CreateElement("Docqueda");
                        Docquda.SetAttribute("id", item);
                        xmlParam.DocumentElement.AppendChild(Docquda);
                    }

                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 1101, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    CorreoNotificacion objCorreo = new CorreoNotificacion();

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var titulo = Convert.ToString(item["titulo"]);
                        var nombreComercial = Convert.ToString(item["NomComercial"]);
                        var mensaje = Convert.ToString(item["mensaje"]);
                        var correo = Convert.ToString(item["correo"]);

                        objCorreo.NotificacionLicitacion(titulo, nombreComercial, mensaje, correo, "PL");
                    }
                }
            }
            catch (Exception ex)
            {
                return new FormResponse { success = false, codError = "-1", msgError = ex.Message };
            }
            return new FormResponse { success = true}; ;
        }

        [AntiForgeryValidate]
        [ActionName("editaPubli")]
        [HttpPost]
        public FormResponse editaPubli(string req, string nombre, string desc, string feIni, string feFin, string hoFin)
        {
            var respuesta = new FormResponse();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");

            var nomdocs = System.Web.HttpContext.Current.Request.Form.GetValues("nomDoc");
            var queda = System.Web.HttpContext.Current.Request.Form.GetValues("queda");

            if (!subirDocs(System.Web.HttpContext.Current.Request.Files, nombre.Replace(' ', '_'), ref xmlParam, nomdocs))
                return new FormResponse { success = false, codError = "-1", msgError = "Ocurrión un error al cargar los documentos" };
            
            try
            {
                xmlParam.DocumentElement.SetAttribute("ACCION", "EL");
                xmlParam.DocumentElement.SetAttribute("REQ", req);
                xmlParam.DocumentElement.SetAttribute("NOMBRE", nombre);
                xmlParam.DocumentElement.SetAttribute("DESC", desc);
                xmlParam.DocumentElement.SetAttribute("FEINI", feIni);
                xmlParam.DocumentElement.SetAttribute("FEFIN", feFin);
                xmlParam.DocumentElement.SetAttribute("HOFIN", hoFin);

                if (queda != null)
                    foreach (string item in queda)
                    {
                        var Docquda = xmlParam.CreateElement("Docqueda");
                        Docquda.SetAttribute("id", item);
                        xmlParam.DocumentElement.AppendChild(Docquda);
                    }

                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 1101, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    CorreoNotificacion objCorreo = new CorreoNotificacion();

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var titulo = Convert.ToString(item["titulo"]);
                        var nombreComercial = Convert.ToString(item["NomComercial"]);
                        var mensaje = Convert.ToString(item["mensaje"]);
                        var correo = Convert.ToString(item["correo"]);

                        objCorreo.NotificacionLicitacion(titulo, nombreComercial, mensaje, correo, "PL");
                    }
                }
            }
            catch (Exception ex)
            {
                return new FormResponse { success = false, codError = "-1", msgError = ex.Message };
            }
            return new FormResponse { success = true }; ;
        }

        [AntiForgeryValidate]
        [ActionName("getPubli")]
        [HttpGet]
        public FormResponse getPubli()
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

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1101, 1);
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
                        var cat = new _catalogo();
                        cat.id = Convert.ToString (item["CodProveedor"]);
                        cat.descripcion = Convert.ToString(item["NomComercial"]);
                        cat.id1 = Convert.ToString(item["idrequerimiento"]);
                        listaProv.Add(cat);
                    }
                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        var doc = new documento();
                        doc.idDoc = Convert.ToInt32(item["idadjunto"]);
                        doc.desc  = Convert.ToString(item["descripcion"]);
                        doc.idReq = Convert.ToInt32(item["idrequerimiento"]);
                        doc.archivo = Convert.ToString(item["nombreArchivo"]);
                        listaDocs.Add(doc);
                    }
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var pub = new publicacion();
                        pub.idReq = Convert.ToInt32(item["idrequerimiento"]);
                        pub.version = (item["version"]==null)? 0: Convert.ToInt32(item["version"]);
                        pub.titulo = Convert.ToString(item["titulo"]);
                        pub.descripcion = Convert.ToString(item["descReq"]);
                        pub.feIni = Convert.ToString(item["feIniLicitacion"]);
                        pub.feFin = Convert.ToString(item["feFinLicitacion"]);
                        pub.hoFin = Convert.ToString(item["hoFinLicitacion"]);
                        pub.estado = Convert.ToString(item["estadoLicitacion"]);
                        pub.descEmp = Convert.ToString(item["descEmp"]);
                        pub.descPubli = Convert.ToString(item["descripcionPub"]);
                        pub.codEmpresa = Convert.ToString(item["codEmpresa"]);
                        pub.monto = Convert.ToString(item["monto"]);
                        pub.nomPubli = Convert.ToString(item["nombrePub"]);
                        pub.documentos = listaDocs.Where(x=>x.idReq== pub.idReq);
                        pub.proveedores = listaProv.Where(x=>x.id1==pub.idReq.ToString());
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
        [ActionName("getPBandera")]
        [HttpGet]
        public FormResponse getPBandera(string idbandeja)
        {
            var respuesta = new FormResponse();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("ACCION", "CPB");

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1101, 1);
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
                        var cat = new _catalogo();
                        cat.id = Convert.ToString(item["CodProveedor"]);
                        cat.descripcion = Convert.ToString(item["NomComercial"]);
                        cat.id1 = Convert.ToString(item["idrequerimiento"]);
                        listaProv.Add(cat);
                    }
                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        var doc = new documento();
                        doc.idDoc = Convert.ToInt32(item["idadjunto"]);
                        doc.desc = Convert.ToString(item["descripcion"]);
                        doc.idReq = Convert.ToInt32(item["idrequerimiento"]);
                        doc.archivo = Convert.ToString(item["nombreArchivo"]);
                        listaDocs.Add(doc);
                    }
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var pub = new publicacion();
                        pub.idReq = Convert.ToInt32(item["idrequerimiento"]);
                        pub.version = (item["version"] == null) ? 0 : Convert.ToInt32(item["version"]);
                        pub.titulo = Convert.ToString(item["titulo"]);
                        pub.descripcion = Convert.ToString(item["descReq"]);
                        pub.feIni = Convert.ToString(item["feIniLicitacion"]);
                        pub.feFin = Convert.ToString(item["feFinLicitacion"]);
                        pub.hoFin = Convert.ToString(item["hoFinLicitacion"]);
                        pub.estado = Convert.ToString(item["estadoLicitacion"]);
                        pub.descEmp = Convert.ToString(item["descEmp"]);
                        pub.descPubli = Convert.ToString(item["descripcionPub"]);
                        pub.codEmpresa = Convert.ToString(item["codEmpresa"]);
                        pub.monto = Convert.ToString(item["monto"]);
                        pub.nomPubli = Convert.ToString(item["nombrePub"]);
                        pub.documentos = listaDocs.Where(x => x.idReq == pub.idReq);
                        pub.proveedores = listaProv.Where(x => x.id1 == pub.idReq.ToString());
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
        [ActionName("getDocs")]
        [HttpGet]
        public FormResponse getDocs(string req)
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
                xmlParam.DocumentElement.SetAttribute("REQ", req);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1101, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<documento> listaRetorno = new List<documento>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var doc = new documento();
                        doc.idDoc = Convert.ToInt32(item["idadjunto"]);
                        doc.desc = Convert.ToString(item["descripcion"]);
                        doc.idReq = Convert.ToInt32(item["idrequerimiento"]);
                        doc.archivo = Convert.ToString(item["nombreArchivo"]);
                        listaRetorno.Add(doc);
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
        [ActionName("getProvDocs")]
        [HttpGet]
        public FormResponse getProvDocs(string idReq)
        {
            var respuesta = new FormResponse();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("ACCION", "CD");
                xmlParam.DocumentElement.SetAttribute("REQ", idReq);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 1101, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<_aplicante> listaRetorno = new List<_aplicante>();
                    List<documento> listaDocs = new List<documento>();
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        var doc = new documento();
                        doc.idDoc = Convert.ToInt32(item["idadjunto"]);
                        doc.desc = Convert.ToString(item["descripcion"]);
                        doc.idOferta = Convert.ToInt32(item["idAdquisicion"]);
                        doc.codProveedor = Convert.ToString(item["codProveedor"]);
                        doc.archivo = Convert.ToString(item["nombreArchivo"]);
                        listaDocs.Add(doc);
                    }
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var app = new _aplicante();
                        app.idOferta = Convert.ToInt32(item["idAdquisicion"]);
                        app.codProveedor = Convert.ToString(item["codproveedor"]); 
                        app.nomProveedor = Convert.ToString(item["NomComercial"]);
                        app.monto = Convert.ToString(item["monto"]);
                        app.fechaEnvio = Convert.ToString(item["fechaEnvio"]);
                        app.tiempoEjecucion = Convert.ToInt32(item["tiempoEjecucion"]);
                        app.estadoParticipando = Convert.ToString(item["estadoparticipandp"]);
                        app.estadoParticipandoDesc = Convert.ToString(item["estadoparticipandodesc"]);
                        app.estadoSeleccionado = Convert.ToString(item["estadoselecionado"]);
                        app.documentos = listaDocs.Where(x => x.idOferta == app.idOferta && x.codProveedor == app.codProveedor);
                        listaRetorno.Add(app);
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
                    }
                }
            }
            catch (Exception)
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

        [ActionName("LeePDFContratos")]
        [Route("LeePDFContratos")]
        [HttpGet]
        public FormResponseModelo LeePDFContratos(string rutaDirectorio, string nomArchivo)
        {
            string FileName = String.IsNullOrEmpty(nomArchivo) ? "" : nomArchivo.Trim();
            string _metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            FormResponseModelo _oLeePDFContratos = new FormResponseModelo();
            try
            {
                string RutaEscritura = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivo"]).Trim();
                RutaEscritura = Path.Combine(RutaEscritura, rutaDirectorio);
                
                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                Proceso.Url = ((string)System.Configuration.ConfigurationManager.AppSettings["Urlbase"]).Trim();
                byte[] archivo = Proceso.LeePdfAdjunto(rutaDirectorio, FileName);

                p_Log.Graba_Log_Info("Ruta Contrato PDF: " + Path.Combine(RutaEscritura, FileName));

                System.IO.File.WriteAllBytes(Path.Combine(RutaEscritura, FileName), archivo);

                _oLeePDFContratos.lSuccess = true;
                _oLeePDFContratos.cCodError = "0";
            }
            catch (Exception ex)
            {
                _oLeePDFContratos.lSuccess = false;
                _oLeePDFContratos.cCodError = "9999";
                _oLeePDFContratos.cMsgError = ex.Message.ToString();
                p_Log.Graba_Log_Error(ex.Message.ToString());
            }
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

            return _oLeePDFContratos;
        }

        [ActionName("DescargaContratoPDF")]
        [Route("DescargaContratoPDF")]
        [HttpGet]
        public HttpResponseMessage DescargaContratoPDF(string rutaDirectorio, string nomArchivo)
        {
            string FileName = String.IsNullOrEmpty(nomArchivo) ? "" : nomArchivo.Trim();

            string _metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            var response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK);
            byte[] res = null;
            try
            {
                string rutaarchivo = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivo"]).Trim();
                rutaarchivo = Path.Combine(rutaarchivo, rutaDirectorio, FileName);


                res = File.ReadAllBytes(rutaarchivo);
                response.Content = new ByteArrayContent(res);
                response.Content.Headers.Add("x-filename", FileName);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            }

            catch (Exception ex)
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                res = null;
                p_Log.Graba_Log_Error(ex.Message.ToString());
            }

            return response;

        }

    }
}

