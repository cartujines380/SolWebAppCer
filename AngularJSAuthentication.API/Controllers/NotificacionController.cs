using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AngularJSAuthentication.API.Models;
using System.Data;
using System.Xml.Linq;
using System.Threading;
using System.IO;
using System.Web;
using Logger;

namespace clibProveedores
{
    [Authorize]
    [RoutePrefix("api/Notificacion")]
    public class NotificacionController : ApiController
    {
        Logger.Logger log = new Logger.Logger();

        //Consulta lista de notificaciones en BD
        [ActionName("consultaNotificaciones")]
        [HttpGet]
        public FormResponseNotificacion GetconsultaNotificaciones(string estado)
        {
          
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XElement("SecNotificacion",
                                     new XAttribute("EstadoNotificacion", estado != null ? estado : ""))));
            ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 401, 1);
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;

            try
            {
                List<Notificacion> Retorno = new List<Notificacion>();
                Retorno = (from reg in ds.Tables[0].AsEnumerable()
                           select new Notificacion
                           {
                               CodNotificacion = reg.Field<int>("Codigo"),
                               Titulo = reg.Field<String>("Titulo"),
                               Categoria = reg.Field<String>("Categoria"),
                               Prioridad = reg.Field<String>("Prioridad"),
                               Comunicado = reg.Field<String>("Comunicado"),
                               Obligatorio = reg.Field<String>("Obligatorio"),
                               Corporativo = reg.Field<String>("Corporativo"),
                               FechaVencimiento = reg.Field<string>("FechaVencimiento"),
                               FechaPublicacion = reg.Field<string>("FechaPublicacion"),
                               Estado = reg.Field<String>("Estado"),
                               Ruta = reg.Field<String>("Ruta"),
                               Observacion = reg.Field<String>("Observacion"),
                               Tipo = reg.Field<String>("Tipo"),
                               MensajeCorreo = reg.Field<String>("MsjCorreo"),
                               TipoCorreo = reg.Field<String>("TipoCorreo"),
                           }).ToList<Notificacion>();
                FormResponse.root.Add(Retorno);
            }
            catch (Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
            }
            return FormResponse;        
    }

        //Consulta lista de proveedores en BDgetConsultaListaNotificacionesProv
        [ActionName("consultaLisProveedor")]
        [HttpGet]
        public FormResponseNotificacion GetconsultaLisProveedor(string tipo, string dato, string usr)
        {
            //Tipo
            //1=Consulta proveedor por numero de ruc
            //2=Devuelve Lista Proveedor y Archivos por codigo de notificacion
            //3=Devuelve lista de Notificacion pendientes
            
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = false;
            FormResponse.codError = "";
            FormResponse.msgError = "";
            
            if (tipo == "1")
            {
                List<Proveedor> lst_retornoSol = new List<Proveedor>();
                DataSet ds = new DataSet();
                ClsGeneral objEjecucion = new ClsGeneral();
                try
                {

                    var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XElement("SecNotificacion",
                                         new XAttribute("Usuario", usr != null ? usr : ""),
                                     new XAttribute("Ruc", dato != null ? dato : ""))));

                    ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 404, 1);
                    if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                    {

                        lst_retornoSol = (from reg in ds.Tables[0].AsEnumerable()
                                          select new Proveedor
                                   {
                                       CodProveedor = reg.Field<String>("CodProveedor"),
                                       RazonSocial = reg.Field<String>("NomComercial"),
                                       Representante = reg.Field<String>("Ruc"),


                                   }).ToList<Proveedor>();

                        if (lst_retornoSol.Count > 0)
                        {
                            FormResponse.root.Add(lst_retornoSol);
                        }

                    }
                    else
                    {
                        FormResponse.success = false;
                        FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString() + ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                        return FormResponse;
                    }                      

                }
                catch(Exception e)
                {
                    FormResponse.success = false;
                    FormResponse.msgError = e.Message.ToString();
                    return FormResponse; ;
                }
                
            }
            if (tipo == "2")
            {
                List<Proveedor> lst_retornoSol = new List<Proveedor>();
                
                DataSet ds2 = new DataSet();
                ClsGeneral objEjecucion2 = new ClsGeneral();
                try
                {

                    var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XElement("SecNotificacion",
                                     new XAttribute("CodNotificacion", dato != null ? dato : ""))));

                    ds2 = objEjecucion2.EjecucionGralDs(wresulFactList.ToString(), 405, 1);
                    if (ds2.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                    {


                        lst_retornoSol = (from reg in ds2.Tables[0].AsEnumerable()
                                          select new Proveedor
                                          {
                                              CodProveedor = reg.Field<String>("Cod_proveedor"),
                                              RazonSocial = reg.Field<String>("NomComercial"),
                                              Representante = reg.Field<String>("Ruc"),
                                              Estado = reg.Field<String>("Estado"),
                                              FecAceptacion = reg.Field<String>("FecAceptacion"),
                                          }).ToList<Proveedor>();
                    }
                    else
                    {
                        FormResponse.success = false;
                        FormResponse.msgError = ds2.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                        return FormResponse;
                    }
                   

                }
                catch(Exception e)
                {

                    FormResponse.success = false;
                    FormResponse.msgError = e.Message.ToString();
                    return FormResponse;
                }

                
                List<string> listaArchivos = new List<string>();
                List<string> LineasSeleccionadas = new List<string>();
                List<string> listaDepartamentos = new List<string>();
                List<string> listaZonas = new List<string>();
                List<string> listaRoles = new List<string>();
                List<Funciones> listaFunciones = new List<Funciones>();
                DataSet ds = new DataSet();
                ClsGeneral objEjecucion = new ClsGeneral();
                try
                {

                    var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XElement("SecNotificacion",
                                     new XAttribute("CodNotificacion", dato != null ? dato : ""))));

                    ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 403, 1);
                    if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                    {


                        foreach (var i in ds.Tables[0].AsEnumerable())
                        {
                            listaArchivos.Add(i.Field<String>("NomArchivo"));
                        }


                        foreach (var i in ds.Tables[1].AsEnumerable())
                        {
                            LineasSeleccionadas.Add(i.Field<String>("Cod_Linea"));
                        }

                        foreach (var i in ds.Tables[2].AsEnumerable())
                        {
                            listaDepartamentos.Add(i.Field<String>("Cod_Departamento"));
                        }

                        foreach (var i in ds.Tables[3].AsEnumerable())
                        {
                            listaZonas.Add(i.Field<String>("IdZona"));
                        }
                        foreach (var i in ds.Tables[4].AsEnumerable())
                        {
                            listaRoles.Add(i.Field<String>("CodRol"));
                        }
                        foreach (var i in ds.Tables[5].AsEnumerable())
                        {
                            Funciones func = new Funciones();
                            func.IdDepartamento = i.Field<String>("CodDepartamento");
                            func.IdFuncion = i.Field<String>("CodFuncion");
                            listaFunciones.Add(func);
                        }
                    }
                    else
                    {
                        FormResponse.success = false;
                        FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString() + ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                        return FormResponse;
                    }

                }
                catch(Exception e)
                {
                    FormResponse.success = false;
                    FormResponse.msgError = e.Message.ToString();
                    return FormResponse;
                }
                FormResponse.root.Add(lst_retornoSol);
                FormResponse.root.Add(listaArchivos);
                FormResponse.root.Add(LineasSeleccionadas);
                FormResponse.root.Add(listaDepartamentos);
                FormResponse.root.Add(listaZonas);
                FormResponse.root.Add(listaRoles);
                FormResponse.root.Add(listaFunciones);
            }
            if (tipo == "3")
            {
                List<Notificacion> lst_retornoSol = new List<Notificacion>();
                DataSet ds = new DataSet();
                ClsGeneral objEjecucion = new ClsGeneral();
                try
                {

                    var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XElement("SecNotificacion",
                                         new XAttribute("Usuario", usr != null ? usr : ""),
                                     new XAttribute("Ruc", dato != null ? dato : ""))));

                    ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 407, 1);
                    if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                    {


                        lst_retornoSol = (from reg in ds.Tables[0].AsEnumerable()
                                          select new Notificacion
                                          {
                                              CodNotificacion = reg.Field<int>("Codigo"),
                                              Titulo = reg.Field<String>("Titulo"),
                                              Comunicado = reg.Field<String>("Comunicado"),
                                              Ruta = reg.Field<String>("Ruta"),
                                              Obligatorio = reg.Field<String>("Obligatorio"),
                                              Prioridad = reg.Field<int>("CodAgrupacion").ToString(),

                                          }).ToList<Notificacion>();
                        
                    }
                    else
                    { 
                        FormResponse.success = false;
                        FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString() + ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                        return FormResponse;
                    }
                }
                catch(Exception e)
                {
                    FormResponse.success = false;
                    FormResponse.msgError = e.Message.ToString();
                    return FormResponse;
                }
               
                FormResponse.root.Add(lst_retornoSol);
            }
            FormResponse.success = true;
            return FormResponse;
        }
        
        //Graba nueva notificacion
        [ActionName("grabaNotificacion")]
        [HttpPost]
        public FormResponseNotificacion GetgrabaNotificacion(Notificacion notGrabar)
        {
            
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = false;
            List<Funciones> listaFUnciones = new List<Funciones>();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                //Lista de Departamentos y Funciones
                foreach(var listaDepRec in notGrabar.ListaDepFuncNot)
                {
                   foreach(var lisFunRec in listaDepRec.ListaFunciones)
                   {
                       if (lisFunRec.IsCheck)  
                            listaFUnciones.Add(lisFunRec);
                   }
                
                }

               var wresulFactList =
               new System.Xml.Linq.XDocument(
                       new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                       new XElement("Root",
                                new XElement("NewNotificacion",
                                new XAttribute("Codigo", notGrabar.CodNotificacion != null ? notGrabar.CodNotificacion : 0),
                                new XAttribute("Titulo", notGrabar.Titulo != null ? notGrabar.Titulo : ""),
                                new XAttribute("Categoria", notGrabar.Categoria != null ? notGrabar.Categoria : ""),
                                new XAttribute("Obligatorio", notGrabar.Obligatorio != null ? notGrabar.Obligatorio : ""),
                                new XAttribute("Corporativo", notGrabar.Corporativo != null ? notGrabar.Corporativo : ""),
                                new XAttribute("FechaVencimiento", notGrabar.FechaVencimiento != null && notGrabar.FechaVencimiento != "01/01/0001" ? notGrabar.FechaVencimiento : ""),
                                new XAttribute("FechaPublicacion", notGrabar.FechaPublicacion != null && notGrabar.FechaPublicacion != "01/01/0001" ? notGrabar.FechaPublicacion : ""),
                                new XAttribute("Comunicado", notGrabar.Comunicado != null ? notGrabar.Comunicado : ""),

                                new XAttribute("Prioridad", notGrabar.Prioridad != null ? notGrabar.Prioridad : ""),
                                new XAttribute("Tipo", notGrabar.Tipo != null && notGrabar.Tipo !="" ? notGrabar.Tipo : "T"),
                                new XAttribute("Estado", notGrabar.Estado != null ? notGrabar.Estado : ""),
                                new XAttribute("Ruta", notGrabar.Ruta != null ? notGrabar.Ruta : ""),
                                 new XAttribute("TipoCorreo", notGrabar.TipoCorreo != null ? notGrabar.TipoCorreo : ""),
                                new XAttribute("MensajeCorreo", notGrabar.MensajeCorreo != null ? notGrabar.MensajeCorreo : ""),
                                new XAttribute("Usuario", notGrabar.Usuario != null ? notGrabar.Usuario : "")),

                               notGrabar.ListaAdjuntos != null ? from p in notGrabar.ListaAdjuntos
                                                                 select new XElement("NewListaAdjuntos",
                                                                             new XAttribute("Adjunto", p != null ? p : ""),
                                                                             new XAttribute("Comunicado", p == notGrabar.Comunicado ? "S" : "N")
                                                                             ) : null,

                               notGrabar.ListaLineasNegocios != null ?
                                   from p in notGrabar.ListaLineasNegocios
                                   select new XElement("NewListaLinNegocios",                                       
                                        new XAttribute("Codigo", p.Codigo != null ? p.Codigo : "")                          
                                                                              ) : null,

                               notGrabar.ListaDepartamentos != null ?
                                   from p in notGrabar.ListaDepartamentos
                                   select new XElement("ListaDepartamentos",
                                        new XAttribute("CodigoDepartamento", p.Id != null ? p.Id : "")
                                                                              ) : null,

                               notGrabar.ListaZonasNot != null ?
                                   from p in notGrabar.ListaZonasNot
                                   select new XElement("ListaZonas",
                                        new XAttribute("CodigoZona", p.Id != null ? p.Id : "")
                                                                              ) : null,
                               
                               

                               listaFUnciones != null ?
                                   from p in listaFUnciones
                                   select new XElement("ListaDepFunciones",
                                       new XAttribute("CodDepartamento", p.IdDepartamento != null ? p.IdDepartamento : ""),
                                        new XAttribute("CodFuncion", p.IdFuncion != null ? p.IdFuncion: "")
                                                                              ) : null,

                                notGrabar.ListaRolesNot != null ?
                                   from p in notGrabar.ListaRolesNot
                                   select new XElement("ListaRoles",
                                      
                                        new XAttribute("CodRol", p.Id != null ? p.Id : "")
                                                                              ) : null,

                               notGrabar.ListaProveedores != null ?
                                   from p in notGrabar.ListaProveedores
                                   select new XElement("NewListaProveedores",                                       
                                        new XAttribute("CodProveedor", p.CodProveedor != null ? p.CodProveedor.ToString() : "")                          
                                                                              ) : null ));
               var newXML = wresulFactList.ToString().Replace(',', '¥');
               
               ds = objEjecucion.EjecucionGralDs(newXML, 400, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    FormResponse.success = true;
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    return FormResponse;

                }
             
            }               
            catch(Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
                return FormResponse;
            }
            
            return FormResponse;
        }

        [ActionName("secuenciaDirectorio")]
        [HttpGet]
        public FormResponseNotificacion GetsecuenciaDirectorio(string tipo)
        {
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = false;
            string secuencia = "";
            string pais = "";
            string region = "";
            string codSapProv = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                try
                {
                    codSapProv = !string.IsNullOrEmpty(tipo) ? tipo.Split('|')[1] : "";
                    tipo = !string.IsNullOrEmpty(tipo) ? tipo.Split('|')[0] : "";
                }
                catch (Exception e)
                {
                    codSapProv = "";
                }

                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                 new XElement("SecDirectorio",
                                    new XAttribute("desDirectorio", tipo != null ? tipo : ""),
                                    new XAttribute("codSap", codSapProv != null ? codSapProv : ""))));

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 402, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    secuencia = ds.Tables[0].Rows[0]["Secuencia"].ToString();
                    FormResponse.success = true;
                    FormResponse.root.Add(secuencia);
                    if (codSapProv != "")
                    {
                        pais = ds.Tables[1].Rows[0]["Pais"].ToString();
                        region = ds.Tables[1].Rows[0]["Region"].ToString();
                        FormResponse.root.Add(pais);
                        FormResponse.root.Add(region);
                    }

                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    return FormResponse;

                }

            }
            catch (Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
                return FormResponse;
            }
            FormResponse.success = true;
            return FormResponse;
        }

        [ActionName("actualizaEstado")]
        [HttpGet]
        public FormResponseNotificacion GetactualizaEstado(string codigo, string estado, string usuario, string observacion)
        {
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                 new XElement("SecNotificacion",
                                 new XAttribute("CodNotificacion", codigo != null ? codigo : ""),
                                 new XAttribute("Usuario", usuario != null ? usuario : ""),
                                 new XAttribute("Observacion", observacion != null ? observacion : ""),
                                 new XAttribute("EstadoNotificacion", estado != null ? estado : "")
                                 )));

                var newXML = wresulFactList.ToString().Replace(',', '¥');
                ds = objEjecucion.EjecucionGralDs(newXML, 406, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = true;
                    //Envio de correo
                    if (estado == "E")
                    {
                        log.FilePath = p_Log;
                        log.WriteMensaje("***** Empieza la Notificación por Correo *****");
                        ConsultaDestinatarios Destinatarios_Consulta;
                        List<ConsultaDestinatarios> listaDestinatarios = new List<ConsultaDestinatarios>();
                        var titulo = "";
                        var ruta = "";
                        var nombreArchivo = "";
                        var tipoCorreo = "";
                        var mensaje = "";
                        var rutaArchivo = "";
                        List<string> ListaArchivos = new List<string>();

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            titulo = Convert.ToString(item["Titulo"]);
                            ruta = "PDF/" + Convert.ToString(item["Ruta"]);
                            nombreArchivo = Convert.ToString(item["NomArchivo"]);
                        }

                        log.FilePath = p_Log;
                        log.WriteMensaje("Titulo: " + titulo + " - Ruta: " + ruta + " - Nombre: " + nombreArchivo);

                        foreach (DataRow item in ds.Tables[2].Rows)
                        {
                            titulo = Convert.ToString(item["Titulo"]);
                            mensaje = Convert.ToString(item["MsjCorreo"]);
                            tipoCorreo = Convert.ToString(item["TipoCorreo"]);
                            ruta = "PDF/" + Convert.ToString(item["Ruta"]);
                        }
                        foreach (DataRow item in ds.Tables[3].Rows)
                        {
                            ListaArchivos.Add(Convert.ToString(item["NomArchivo"]));
                        }


                        if (tipoCorreo == "S")
                        {
                            if (ListaArchivos.Count == 1)
                                rutaArchivo = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + ruta), ListaArchivos[0]);
                        }
                        else
                        {
                            rutaArchivo = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + ruta), nombreArchivo);
                        }

                        log.FilePath = p_Log;
                        log.WriteMensaje("RutaCompleta: " + rutaArchivo);

                        if (tipoCorreo != "G")
                        {
                            log.FilePath = p_Log;
                            log.WriteMensaje("DatosEnvio: " + ds.Tables[1].Rows.Count.ToString());

                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                Destinatarios_Consulta = new ConsultaDestinatarios();
                                Destinatarios_Consulta.Correo = Convert.ToString(item["CorreoE"]);
                                Destinatarios_Consulta.Nombre = Convert.ToString(item["Nombre"]);
                                Destinatarios_Consulta.Proveedor = Convert.ToString(item["CodProveedor"]);
                                Destinatarios_Consulta.Ruc = Convert.ToString(item["Ruc"]);
                                Destinatarios_Consulta.NomComercial = Convert.ToString(item["NomComercial"]);
                                Destinatarios_Consulta.CodDepartamento = Convert.ToString(item["Departamento"]);
                                Destinatarios_Consulta.DesDepartamento = Convert.ToString(item["desDepartamento"]);
                                listaDestinatarios.Add(Destinatarios_Consulta);

                                log.FilePath = p_Log;
                                log.WriteMensaje("CorreoE: " + Convert.ToString(item["CorreoE"] + " - Ruc: " + Convert.ToString(item["Ruc"])));
                            }

                            if (listaDestinatarios.Count > 0)
                            {
                                Thread t = new Thread(() => this.EnviaNotificaciones(listaDestinatarios, titulo, rutaArchivo, tipoCorreo, mensaje));
                                t.Start();

                                log.FilePath = p_Log;
                                log.WriteMensaje("Envío OK");
                            }
                        }
                        log.FilePath = p_Log;
                        log.WriteMensaje("***** Fin Envio Correo *****");
                        log.Linea();
                    }

                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    return FormResponse;
                }

            }
            catch(Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();

                log.FilePath = p_Log;
                log.WriteMensaje("Error en [GetactualizaEstado] -> " + e.Message.ToString());

                return FormResponse;
            }

            return FormResponse;
        }

        [ActionName("actualizaEstadoNot")]
        [HttpGet]
        public FormResponseNotificacion GetactualizaEstadoNot(string codNotificacion, string codProveedor, string usr)
        {
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                 new XElement("SecNotificacion",
                                 new XAttribute("CodNotificacion", codNotificacion != null ? codNotificacion : ""),
                                 new XAttribute("Usuario", usr != null ? usr : ""),
                                 new XAttribute("CodProveedor", codProveedor != null ? codProveedor : "")
                                 )));

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 408, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = true;
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    return FormResponse;
                }

            }
            catch(Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
                return FormResponse;
            }
            return FormResponse;
        }

        [ActionName("agrupaNotificacion")]
        [HttpGet]
        public FormResponseNotificacion GetagrupaNotificacion(string codNotificacion, string codNotificacion2)
        {
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                 new XElement("SecNotificacion",
                                 new XAttribute("CodNotificacion1", codNotificacion != null ? codNotificacion : ""),
                                 new XAttribute("CodNotificacion2", codNotificacion2 != null ? codNotificacion2 : "")
                                 )));

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 409, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = true;
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    return FormResponse;
                }
                
            }
            catch (Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
                return FormResponse;
            }
            return FormResponse;
        }

        //Consulta lista de proveedores en BD
        [ActionName("consultaListaNotProveedor")]
        [HttpGet]
        public FormResponseNotificacion GetconsultaListaNotProveedor(string ruc, string usr)
        {
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();                         
            List<Notificacion> lst_retornoSol = new List<Notificacion>();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {

                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                    new XElement("SecNotificacion",
                                        new XAttribute("Usuario", usr != null ? usr : ""),
                                    new XAttribute("Ruc", ruc != null ? ruc : ""))));

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 410, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    lst_retornoSol = (from reg in ds.Tables[0].AsEnumerable()
                                        select new Notificacion
                                        {
                                            CodNotificacion = reg.Field<int>("Codigo"),
                                            Titulo = reg.Field<String>("Titulo"),
                                            Comunicado = reg.Field<String>("Comunicado"),
                                            FechaVencimiento = reg.Field<String>("FechaVencimiento"),
                                            Categoria = reg.Field<String>("Categoria"),
                                            Prioridad = reg.Field<String>("Prioridad"),
                                            Ruta = reg.Field<String>("Ruta"),
                                            Obligatorio = reg.Field<String>("Obligatorio"),
                                            Estado = reg.Field<String>("Estado"),
                                            
                                        }).ToList<Notificacion>();
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    return FormResponse;
                }

            }
            catch(Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
                return FormResponse;
            }
            FormResponse.success = true;
            FormResponse.root.Add(lst_retornoSol);
            

            return FormResponse;
        }

        //Consulta lista de proveedores en BD
        [ActionName("actualizaMensajesPagosProv")]
        [HttpGet]
        public FormResponseNotificacion ActualizaMensajesPagosProveedor(string ruc,string id,string f)
        {
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            List<Mensajes> lst_retornoSol = new List<Mensajes>();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {

                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                    new XElement("Pagos",
                                    new XElement("Tipo", "CN"),
                                    new XElement("Id", id),
                                    new XElement("Ruc", ruc != null ? ruc : ""))));

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 414, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    lst_retornoSol = (from reg in ds.Tables[0].AsEnumerable()
                                      select new Mensajes
                                      {
                                          Id = reg.Field<Int32>("Id"),
                                          Titulo = reg.Field<String>("Titulo"),
                                          Mensaje = reg.Field<String>("Mensaje"),
                                          Estado = reg.Field<String>("Estado"),

                                      }).ToList<Mensajes>();
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    return FormResponse;
                }

            }
            catch (Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
                return FormResponse;
            }
            FormResponse.success = true;
            FormResponse.root.Add(lst_retornoSol);


            return FormResponse;
        }

        [ActionName("registraMensaje")]
        [HttpGet]
        public FormResponseNotificacion GetregistraMensaje(string usuario, string correo, string mensaje)
        {
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                 new XElement("SecNotificacion",
                                 new XAttribute("Usuario", usuario != null ? usuario : ""),
                                 new XAttribute("Correo", correo != null ? correo : ""),
                                 new XAttribute("Mensaje", mensaje != null ? mensaje : "")
                                 )));

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 411, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = true;  
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    return FormResponse;
                }

            }
            catch (Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
                return FormResponse;
            }
            return FormResponse;
        }

        [ActionName("consultaListaPrecios")]
        [HttpGet]
        public FormResponseNotificacion GetconsultaListaPrecios(string tipoLista, string ruc)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XElement("SecNotificacion",
                                          new XAttribute("Ruc", ruc != null ? ruc : ""),
                                     new XAttribute("TipoLista", tipoLista != null ? tipoLista : ""))));
            ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 412, 1);
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;
      
            List<Producto> Retorno = new List<Producto>();

            if (tipoLista == "3")
            {
                Retorno = (from reg in ds.Tables[0].AsEnumerable()
                           select new Producto
                           {
                               Codigo = reg.Field<String>("Anio"),
                               Descripcion = reg.Field<String>("Mes"),
                               Archivo = reg.Field<String>("Archivo"),
                           }).ToList<Producto>();
            }
            else
            {
                Retorno = (from reg in ds.Tables[0].AsEnumerable()
                           select new Producto
                           {
                               Codigo = reg.Field<String>("Codigo"),
                               Descripcion = reg.Field<String>("Descripcion"),
                               Precio = reg.Field<Decimal>("Precio"),
                               FechaPublicacion = reg.Field<DateTime>("FechaPublicacion"),

                           }).ToList<Producto>();
            }
            
            FormResponse.root.Add(Retorno);
            return FormResponse;
        }

        [ActionName("consultaClasificados")]
        [HttpGet]
        public FormResponseNotificacion GetconsultaClasificados(string tipoLista2)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XElement("SecNotificacion",
                                     new XAttribute("TipoLista", tipoLista2 != null ? tipoLista2 : ""))));
            ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 413, 1);
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;
            
            List<Clasificado> Retorno = new List<Clasificado>();
            Retorno = (from reg in ds.Tables[0].AsEnumerable()
                       select new Clasificado
                       {
                           NumClasificado = reg.Field<Int64>("NumClasificado"),
                           Cargo = reg.Field<String>("Cargo"),
                           Ciudad = reg.Field<String>("Ciudad"),
                           FechaPublicacion = reg.Field<DateTime>("FechaPublicacion").ToString("dd/MM/yyyy"),

                       }).ToList<Clasificado>();
            FormResponse.root.Add(Retorno);
            return FormResponse;
        }

        [ActionName("descargaNotificacionG")]
        [HttpGet]
        public FormResponseNotificacion GetdescargaNotificacionG()
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                 new XElement("SecNotificacion",
                                
                                 new XAttribute("Accion", "G")
                                 )));

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 405, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    var directorio =  ds.Tables[0].Rows[0]["Ruta"].ToString();
                    var archivo = ds.Tables[0].Rows[0]["Comunicado"].ToString();
                    var archivoAnterior = "";
                    try
                    {
                        archivoAnterior = ds.Tables[0].Rows[1]["Comunicado"].ToString();
                    }
                    catch
                    {
                        archivoAnterior = "";
                    }

                    //Validar si ya fue descargado la notificación general
                    var fileSavePath = Path.Combine(AppConfig.archivoPDF, archivo);

                    if (File.Exists(fileSavePath))
                    {
                        FormResponse.success = true;
                        FormResponse.msgError = archivo;
                        return FormResponse;
                    }

                    //Eliminar Notificacion general anterior
                    try
                    {
                        if (archivoAnterior != "")
                        {
                            var fileSaveAntPath = Path.Combine(AppConfig.archivoPDF, archivoAnterior);
                            if (File.Exists(fileSaveAntPath))
                            {

                                File.Delete(fileSaveAntPath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.FilePath = p_Log;
                        log.WriteMensaje("Error en [GetdescargaNotificacionG] -> " + ex.Message.ToString());
                    }
                    string lv_Msg = "";
                    clsFTP lv_sFtp = new clsFTP();
                    lv_sFtp.lv_EsPasivo = false;
                    lv_sFtp.lv_IP = AppConfig.SftpServerIp;
                    lv_sFtp.lv_Puerto = Convert.ToInt32(AppConfig.SftpServerPort);
                    lv_sFtp.lv_Usuario = AppConfig.SftpServerUserName;
                    lv_sFtp.lv_Clave = AppConfig.SftpServerPassword;
                    string tutafinal = AppConfig.SftpPath + directorio + "/";
                    
                    byte[] retorno = lv_sFtp.ObtenerArchivo_Sftp(tutafinal, archivo, lv_Msg);

                    File.WriteAllBytes(fileSavePath, retorno);

                    FormResponse.success = true;
                    FormResponse.msgError = archivo;
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    return FormResponse;
                }
               


            }
            catch (Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
                return FormResponse;
            }
            return FormResponse;
        }

        public void EnviaNotificaciones(List<ConsultaDestinatarios> listaDestinarios, string titulo, string rutaCompletaArchivo, string tipoCorreo, string mensajeCorreo)
        {
            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables = new Dictionary<string, string>();
            
            #endregion RFD0-2022-155 Variables CORREO

            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            try
            {
                string asuntoEmail = "";

                if (tipoCorreo == "S")
                {
                    PI_NombrePlantilla = "CorreoProveedor.html"; //RFD0 - 2022 - 155

                }
                else
                {
                    PI_NombrePlantilla = "ComunicadoProveedor.html"; //RFD0 - 2022 - 155
                }

                asuntoEmail = titulo + "";

                AngularJSAuthentication.API.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new AngularJSAuthentication.API.WCFEnvioCorreo.ServEnvioClientSoapClient();
                var valcorreo = "";
                foreach (var i in listaDestinarios)
                {
                    if (valcorreo != i.Correo)
                    {
                        if (!String.IsNullOrEmpty(i.Correo))
                        {
                            if (tipoCorreo == "S")
                            {
                                #region RFD0-2022-155 CORREO
                                Variables = new Dictionary<string, string>();
                                Variables.Add("@@MensajeCorreo", mensajeCorreo);
                                #endregion
                            }

                            log.FilePath = p_Log;
                            log.WriteMensaje("T.CORREO: " + tipoCorreo +" - ARCH: " + rutaCompletaArchivo + " - EMAIL: " + i.Correo + " - ASUNT: " + asuntoEmail);

                            if (rutaCompletaArchivo == "")
                            {
                                #region RFD0-2022-155 CORREO
                                PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                                byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");

                                objEnvMail.EnviaCorreoApi("", i.Correo, "", "", asuntoEmail, "", true, true, false,
                                    data, null,
                                    PI_NombrePlantilla, PI_Variables);
                                #endregion
                            }
                            else
                            {
                                #region RFD0-2022-155 CORREO
                                PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                                byte[] data = File.ReadAllBytes(rutaCompletaArchivo);
                                FileInfo fi = new FileInfo(rutaCompletaArchivo);

                                objEnvMail.EnviaCorreoApi("", i.Correo, "", "", asuntoEmail, "", true, true, true,
                                    data, fi.Name,
                                    PI_NombrePlantilla, PI_Variables);
                                #endregion
                            }
                        }
                    }
                    valcorreo = i.Correo;
                }
                
            }
            catch (Exception ex)
            {
                log.FilePath = p_Log;
                log.WriteMensaje("Error en [EnviaNotificaciones] -> " + ex.Message.ToString());
            }

        }
   
    }
}
    
