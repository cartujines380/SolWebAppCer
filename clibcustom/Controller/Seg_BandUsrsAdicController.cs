using clibCustom;
using clibCustom.Handlers;
using clibCustom.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Http;
using System.Xml;

namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/SegBandjUsrsAdic")]
    public class SegBandjUsrsAdicController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("ConsBandjUsrsAdic")]
        [HttpGet]
        public formResponseSeguridad GetConsBandjUsrsAdic(string Ruc, string Usuario, string Nombre,string Apellido,string Estado, string RecActas)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            List<seg_ConsBandejaUsrAdic> objListSegCons = new List<seg_ConsBandejaUsrAdic>();
            seg_ConsBandejaUsrAdic objSegCons;

            DataSet ds = new DataSet();
            DataSet dsR = new DataSet();
            clsSeguridad objEjecucion = new clsSeguridad();
            XmlDocument xmlParam = new XmlDocument();
            String ApoderadoFiscal = "";
            String RazonSocial = "";
            

            var ValorTokenUser = string.Empty; 
            ValorTokenUser = InitialiseService.GetTokenUser();
            //InitialiseService.PI_Session

            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);
                xmlParam.DocumentElement.SetAttribute("Nombre", Nombre);
                xmlParam.DocumentElement.SetAttribute("Apellido", Apellido);
                xmlParam.DocumentElement.SetAttribute("Estado", Estado);
                xmlParam.DocumentElement.SetAttribute("RecActas", RecActas);


                ds = objEjecucion.ConsultaBandejaUsuariosAdicionales(InitialiseService.Semilla, ValorTokenUser, xmlParam);
                ApoderadoFiscal = ds.Tables[1].Rows[0]["Apoderado"].ToString();
                RazonSocial = ds.Tables[1].Rows[0]["NomComercial"].ToString();
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string[] usuarios = new string[ds.Tables[0].Rows.Count];
                            int i = 0;
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                usuarios[i] = Convert.ToString(item["Usuario"]);
                                i++;
                            }
                           
                            dsR = objEjecucion.ConsRolesPorListaUsuariosFS(ValorTokenUser.ToString(), usuarios, Ruc);
                          
                            if (dsR.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                            {
                                foreach (DataRow itRol in dsR.Tables[0].Select("idrol>23", "descripcion, idusuario"))
                                {
                                    foreach (DataRow item in ds.Tables[0].Select("Usuario='" + Convert.ToString(itRol["idusuario"]) + "'", "Usuario"))
                                    {
                                        objSegCons = new seg_ConsBandejaUsrAdic();
                                        objSegCons.identificacion = Convert.ToString(item["Identificacion"]);
                                        objSegCons.usuario = Convert.ToString(item["Usuario"]);
                                        objSegCons.nombre = Convert.ToString(item["Nombre"]);
                                        objSegCons.apellido = Convert.ToString(item["Apellido"]);
                                        objSegCons.correoE = Convert.ToString(item["CorreoE"]);
                                        objSegCons.celular = Convert.ToString(item["Celular"]);
                                        objSegCons.usrAutorizador = Convert.ToString(item["UsrAutorizador"]);
                                        objSegCons.estado = Convert.ToString(item["Estado"]);
                                        objSegCons.idRol = Convert.ToString(itRol["idrol"]);
                                        objSegCons.rol = Convert.ToString(itRol["descripcion"]);
                                        objSegCons.recActas = Convert.ToBoolean(item["RecibeActas"]);
                                        clibUtil.Util.clsUtilitario obj = new clibUtil.Util.clsUtilitario();
                                        objSegCons.clave = obj.Decrypt(Convert.ToString(item["Clave"]));
                                        objListSegCons.Add(objSegCons);
                                    }
                                }
                            }
                            else
                            {
                                FormResponse.success = false;
                                FormResponse.codError = dsR.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                                FormResponse.msgError = dsR.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                            }
                        }
                    }
                    FormResponse.success = true;
                    FormResponse.root.Add(objListSegCons);
                    FormResponse.root.Add(ApoderadoFiscal);
                    FormResponse.root.Add(RazonSocial);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }

            return FormResponse;
        }

        [ActionName("ConsActivarusuario")]
        [HttpGet]
        public formResponseSeguridad GetConsActivarusuario(string RucActivar, string UsuarioActivar, string tipo)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            DataSet ds = new DataSet();
            clsSeguridad objEjecucion = new clsSeguridad();
            XmlDocument xmlParam = new XmlDocument();
            var ValorTokenUser = string.Empty;

            ValorTokenUser = InitialiseService.GetTokenUser();


            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("Ruc", RucActivar);
                xmlParam.DocumentElement.SetAttribute("Usuario", UsuarioActivar);
                xmlParam.DocumentElement.SetAttribute("tipo", tipo);

                ds = objEjecucion.ConsultaActivarUsuario(InitialiseService.Semilla, ValorTokenUser, xmlParam);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count > 1)
                    {
                        DataRow dr1 = ds.Tables[0].Rows[0];
                    }
                    FormResponse.success = true;
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }

            return FormResponse;
        }

        [ActionName("ConsDatosUsrsAdic")]
        [HttpGet]
        public formResponseSeguridad GetConsDatosUsrsAdic(string Ruc, string Usuario)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            List<seg_GrabaUsrAdic> objListSegCons = new List<seg_GrabaUsrAdic>();
            DataSet ds = new DataSet();
            clsSeguridad objEjecucion = new clsSeguridad();
            XmlDocument xmlParam = new XmlDocument();

            var ValorTokenUser = string.Empty;

            ValorTokenUser = InitialiseService.GetTokenUser();


            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);

                ds = objEjecucion.ConsultaDatosUsuarioAdicional(InitialiseService.Semilla, ValorTokenUser, xmlParam);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count > 1)
                    {
                        seg_GrabaUsrAdic objSegCons = new seg_GrabaUsrAdic();
                        DataRow dr1 = ds.Tables[0].Rows[0];
                        DataRow dr2 = ds.Tables[1].Rows[0];
                        objSegCons.pRuc = Ruc;
                        objSegCons.pUsuario = Usuario;
                        objSegCons.pIdParticipante = Convert.ToInt32(dr1["IdParticipante"]);
                        objSegCons.pTipoIdent = Convert.ToString(dr2["TipoIdent"]);
                        objSegCons.pIdentificacion = Convert.ToString(dr2["Identificacion"]);
                        objSegCons.pCodSap = Convert.ToString(dr1["CodSAP"]);
                        objSegCons.pEstado = Convert.ToString(dr1["Estado"]);
                        objSegCons.pApellido1 = Convert.ToString(dr2["Apellido1"]);
                        objSegCons.pApellido2 = Convert.ToString(dr2["Apellido2"]);
                        objSegCons.pNombre1 = Convert.ToString(dr2["Nombre1"]);
                        objSegCons.pNombre2 = Convert.ToString(dr2["Nombre2"]);
                        objSegCons.pEstadoCivil = Convert.ToString(dr2["EstadoCivil"]);
                        objSegCons.pGenero = Convert.ToString(dr2["Genero"]);
                        objSegCons.pCorreoE = Convert.ToString(dr1["CorreoE"]);
                        objSegCons.pCelular = Convert.ToString(dr1["Celular"]);
                        objSegCons.pTelefono = Convert.ToString(dr1["Telefono"]);
                        objSegCons.pPais = Convert.ToString(dr2["Pais"]);
                        objSegCons.pProvincia = Convert.ToString(dr2["Provincia"]);
                        objSegCons.pCiudad = Convert.ToString(dr2["Ciudad"]);
                        objSegCons.pDireccion = Convert.ToString(dr2["Direccion"]);
                        objSegCons.pDepartamento = Convert.ToString(dr1["Cargo"]);
                        objSegCons.pFuncion = Convert.ToString(dr1["Funcion"]);
                        objSegCons.pRecibeActa = Convert.ToBoolean(dr2["recActas"]);
                        
                        objSegCons.pClave = "";
                        
                        //estructura Zonas
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            int tot = ds.Tables[2].Rows.Count;
                            int idx = 0;
                            seg_GrabaUsrAdic.seg_UsrAdicZona[] listZonas = new seg_GrabaUsrAdic.seg_UsrAdicZona[tot];
                            foreach (DataRow dr3 in ds.Tables[2].Rows)
                            {
                                seg_GrabaUsrAdic.seg_UsrAdicZona objZona = new seg_GrabaUsrAdic.seg_UsrAdicZona();
                                objZona.pCodZona = Convert.ToString(dr3["Zona"]);
                                objZona.pDescripcion = Convert.ToString(dr3["DesZona"]);
                                listZonas[idx] = objZona;
                                idx++;
                            }
                            objSegCons.pListZona = listZonas;
                        }

                        

                        DataSet ds2 = objEjecucion.ConsRolesPorUsuarioFS(InitialiseService.Semilla, ValorTokenUser, Usuario, Ruc);
                        if (ds2.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                        {
                            DataRow[] listRolUsr = ds2.Tables[0].Select("IdRol>23");
                            if (listRolUsr.Length > 0)
                            {
                                seg_GrabaUsrAdic.seg_UsrAdicRol[] listRoles = new seg_GrabaUsrAdic.seg_UsrAdicRol[listRolUsr.Length];
                                for (int i = 0; i < listRolUsr.Length; i++)
                                {
                                    seg_GrabaUsrAdic.seg_UsrAdicRol objRol = new seg_GrabaUsrAdic.seg_UsrAdicRol();
                                    objRol.pIdRol = Convert.ToInt32(listRolUsr[i]["IdRol"]).ToString();
                                    objRol.pDescripcion = Convert.ToString(listRolUsr[i]["Descripcion"]);
                                    listRoles[i] = objRol;
                                }
                                objSegCons.pListRol = listRoles;
                            }
                        }

                        //estructura Almacenes
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            int tot = ds.Tables[3].Rows.Count;
                            int idx = 0;
                            seg_GrabaUsrAdic.seg_UsrAdicAlmacen[] listAlmacen = new seg_GrabaUsrAdic.seg_UsrAdicAlmacen[tot];
                            foreach (DataRow dr3 in ds.Tables[3].Rows)
                            {
                                seg_GrabaUsrAdic.seg_UsrAdicAlmacen objAlmacen = new seg_GrabaUsrAdic.seg_UsrAdicAlmacen();
                                objAlmacen.pCodCiudad = Convert.ToString(dr3["Zona"]);
                                objAlmacen.pCodAlmacen = Convert.ToString(dr3["Almacen"]);
                                listAlmacen[idx] = objAlmacen;
                                idx++;
                            }
                            objSegCons.pListAlm = listAlmacen;
                        }

                        objListSegCons.Add(objSegCons);
                    }
                    FormResponse.success = true;
                    FormResponse.root.Add(objListSegCons);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }

            return FormResponse;
        }

        //Agregado el 16-01-2016 por J. Navarrete
        [ActionName("ConsDatosLegAsociados")]
        [HttpGet]
        public formResponseSeguridad GetConsDatosLegAsociados(string Tipo, string Ruc, string Nombres)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            List<seg_ConsDatosLegAsociados> objListSegCons = new List<seg_ConsDatosLegAsociados>();
            DataSet ds = new DataSet();
            clsSeguridad objEjecucion = new clsSeguridad();
            XmlDocument xmlParam = new XmlDocument();

            var ValorTokenUser = string.Empty;

            ValorTokenUser = InitialiseService.GetTokenUser();


            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Nombres", Nombres);

                ds = objEjecucion.ConsultaDatosLegacyAsociados(InitialiseService.Semilla, ValorTokenUser, xmlParam);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count > 1)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            seg_ConsDatosLegAsociados objSegCons = new seg_ConsDatosLegAsociados();
                            objSegCons.pRuc = Convert.ToString(dr["Ruc"]);
                            objSegCons.pUsuario = Convert.ToString(dr["Usuario"]);
                            objSegCons.pCedula = Convert.ToString(dr["Cedula"]);
                            objSegCons.pApellidos = Convert.ToString(dr["Apellidos"]);
                            objSegCons.pNombres = Convert.ToString(dr["Nombres"]);
                            objSegCons.pCodLegacy = Convert.ToString(dr["CodLegacy"]);
                            objSegCons.pUserLegacy = Convert.ToString(dr["UserLegacy"]);
                            objSegCons.pCorreo = Convert.ToString(dr["CorreoE"]);
                            objSegCons.pCelular = Convert.ToString(dr["Celular"]);
                            objSegCons.prolAdmin = Convert.ToBoolean(dr["rolAdmin"]);
                            objSegCons.prolComercial = Convert.ToBoolean(dr["rolComercial"]);
                            objSegCons.prolContable = Convert.ToBoolean(dr["rolContable"]);
                            objSegCons.prolLogistico = Convert.ToBoolean(dr["rolLogistico"]);
                            objListSegCons.Add(objSegCons);
                        }
                    }
                    FormResponse.success = true;
                    FormResponse.root.Add(objListSegCons);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }

            return FormResponse;
        }

        //Agregado el 16-01-2016 por J. Navarrete
        [ActionName("ActDatosLegAsociados")]
        [HttpGet]
        public formResponseSeguridad GetActDatosLegAsociados(string Tipo, string Ruc, string Usuario, string Cedula, string CodLegacy, string UserLegacy)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            XmlDocument xmlParam = new XmlDocument();
            DataSet ds = new DataSet();

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();

            clsSeguridad objEjecucion = new clsSeguridad();
            xmlParam.LoadXml("<Root />");
            try
            {

                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);
                xmlParam.DocumentElement.SetAttribute("Cedula", Cedula);
                xmlParam.DocumentElement.SetAttribute("CodLegacy", CodLegacy);
                xmlParam.DocumentElement.SetAttribute("UserLegacy", UserLegacy);

                ds = objEjecucion.ActualizaDatosLegacyAsociados(InitialiseService.Semilla, ValorTokenUser, xmlParam);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = true;
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }


        [ActionName("ConsTodasZonas")]
        [HttpGet]
        public formResponseSeguridad GetConsTodasZonas(string errDefZ)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            List<seg_GrabaUsrAdic.seg_UsrAdicZona> objListSegCons = new List<seg_GrabaUsrAdic.seg_UsrAdicZona>();
            seg_GrabaUsrAdic.seg_UsrAdicZona objSegCons;
            DataSet ds = new DataSet();

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();

            clsSeguridad objEjecucion=new clsSeguridad();
            try
            {
                ds = objEjecucion.GetConsTodasZonas(InitialiseService.Semilla, InitialiseService.IdOrganizacion, ValorTokenUser);

               
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count > 1)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            objSegCons = new seg_GrabaUsrAdic.seg_UsrAdicZona();
                            objSegCons.pCodZona = Convert.ToString(item["CodZona"]);
                            objSegCons.pDescripcion = Convert.ToString(item["Descripcion"]);
                            objListSegCons.Add(objSegCons);
                        }
                    }
                    FormResponse.success = true;
                    FormResponse.root.Add(objListSegCons);
                }
                else
                {
                    Logger.Logger log = new Logger.Logger();
                    string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();

                    log.FilePath = p_Log;
                    log.WriteMensaje("Error en zonas: "+ ds.Tables["TblEstado"].Rows[0]["CodError"].ToString() + " - "+ ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString()); 
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }

            return FormResponse;
        }


        [ActionName("ConsTodosRoles")]
        [HttpGet]
        public formResponseSeguridad GetConsTodosRoles(string errDefR)
        {
            Logger.Logger log = new Logger.Logger();
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            List<seg_GrabaUsrAdic.seg_UsrAdicRol> objListSegCons = new List<seg_GrabaUsrAdic.seg_UsrAdicRol>();
            seg_GrabaUsrAdic.seg_UsrAdicRol objSegCons;
            DataSet ds = new DataSet();
            clsSeguridad objEjecucion = new clsSeguridad();
           
            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();

            log.FilePath = p_Log;
            log.WriteMensaje("ConsTodosRoles");

            log.FilePath = p_Log;
            log.WriteMensaje("DATOS_ROLES SM: "+ InitialiseService.Semilla + " TK: " + ValorTokenUser.ToString() + " ORG: " + InitialiseService.IdOrganizacion);

            try
            {

                ds = objEjecucion.ConsRolesPorOrgFS(InitialiseService.Semilla,ValorTokenUser, InitialiseService.IdOrganizacion);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count > 1)
                    {
                        foreach (DataRow item in ds.Tables[0].Select("IdRol>23"))
                        {
                            objSegCons = new seg_GrabaUsrAdic.seg_UsrAdicRol();
                            objSegCons.pIdRol = Convert.ToInt32(item["IdRol"]).ToString();
                            objSegCons.pDescripcion = Convert.ToString(item["Nombre"]);
                            objSegCons.pDescripcionRol = Convert.ToString(item["Descripcion"]);
                            objListSegCons.Add(objSegCons);

                            log.FilePath = p_Log;
                            log.WriteMensaje("ROLES ID: " + Convert.ToInt32(item["IdRol"]).ToString() + " - "+ Convert.ToString(item["Descripcion"]));
                        }
                    }
                    FormResponse.success = true;
                    FormResponse.root.Add(objListSegCons);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();

                    log.FilePath = p_Log;
                    log.WriteMensaje("ConsTodosRoles - Error: " + ds.Tables["TblEstado"].Rows[0]["CodError"].ToString() + " - "+ ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;

                log.FilePath = p_Log;
                log.WriteMensaje("Error [GetConsTodosRoles]: " + ex.Message);
            }

            return FormResponse;
        }


        [ActionName("GrabarUsrAdic")]
        [HttpPost]
        public formResponseSeguridad GetGrabarUsrAdic(seg_GrabaUsrAdic userModel)
        {
            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            XmlDocument xmlParam = new XmlDocument();
            XmlElement xEl1;
            XmlDocument xmlResp = new XmlDocument();
            var listaRoles = "";
            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();

            clsSeguridad objEjecucion = new clsSeguridad();
            try
            {
                
                    xmlParam.LoadXml("<Root />");
                    // INI - armado de datos
                    string opcion = "Mod";
                    if (userModel.pIdParticipante == 0)
                    {
                        opcion = "New";
                    }
                    xEl1 = xmlParam.CreateElement(opcion);
                    xEl1.SetAttribute("IdEmpresa", "1");
                    xEl1.SetAttribute("Ruc", userModel.pRuc);
                    xEl1.SetAttribute("Usuario", userModel.pUsuario);
                    xEl1.SetAttribute("IdParticipante", userModel.pIdParticipante.ToString());
                    xEl1.SetAttribute("CodProveedor", userModel.pCodSap);
                    xEl1.SetAttribute("CorreoE", userModel.pCorreoE);
                    xEl1.SetAttribute("Telefono", userModel.pTelefono);
                    xEl1.SetAttribute("Celular", userModel.pCelular);
                    xEl1.SetAttribute("Estado", userModel.pEstado);
                    xEl1.SetAttribute("Departamento", userModel.pDepartamento);
                    xEl1.SetAttribute("Funcion", userModel.pFuncion);
                    if (userModel.pRecibeActa)
                        xEl1.SetAttribute("RecibeActas", "1");
                    else
                        xEl1.SetAttribute("RecibeActas", "0");
                    if (opcion == "New")
                    {
                        xEl1.SetAttribute("Clave", userModel.pClave);
                    }
                    xEl1.SetAttribute("TipoIdent", userModel.pTipoIdent);
                    xEl1.SetAttribute("Identificacion", userModel.pIdentificacion);
                    xEl1.SetAttribute("Apellido1", userModel.pApellido1);
                    xEl1.SetAttribute("Apellido2", userModel.pApellido2);
                    xEl1.SetAttribute("Nombre1", userModel.pNombre1);
                    xEl1.SetAttribute("Nombre2", userModel.pNombre2);
                    xEl1.SetAttribute("EstadoCivil", userModel.pEstadoCivil);
                    xEl1.SetAttribute("Genero", userModel.pGenero);
                    xEl1.SetAttribute("Pais", userModel.pPais);
                    xEl1.SetAttribute("Provincia", userModel.pProvincia);
                    xEl1.SetAttribute("Ciudad", userModel.pCiudad);
                    xEl1.SetAttribute("Direccion", userModel.pDireccion);
                    XmlElement xRol;
                    xRol = xmlParam.CreateElement("Rol");
                    xRol.SetAttribute("IdRol", "3"); //rol default para usuarios que pueden hacer logon
                    xEl1.AppendChild(xRol);
                    xRol = xmlParam.CreateElement("Rol");
                    xRol.SetAttribute("IdRol", "21"); //rol con Permisos de Usuarios Proveedores Básicos
                    xEl1.AppendChild(xRol);
                    foreach (seg_GrabaUsrAdic.seg_UsrAdicRol rol in userModel.pListRol)
                    {
                        xRol = xmlParam.CreateElement("Rol");
                        xRol.SetAttribute("IdRol", rol.pIdRol);
                        listaRoles += rol.pDescripcionRol + " | ";
                        xEl1.AppendChild(xRol);
                    }
                    foreach (seg_GrabaUsrAdic.seg_UsrAdicZona zona in userModel.pListZona)
                    {
                        xRol = xmlParam.CreateElement("Zona");
                        xRol.SetAttribute("Zona", zona.pCodZona);
                        xEl1.AppendChild(xRol);
                    }
                    foreach (seg_GrabaUsrAdic.seg_UsrAdicAlmacen alm in userModel.pListAlm)
                    {
                        xRol = xmlParam.CreateElement("Alm");
                        xRol.SetAttribute("codAlmacen", alm.pCodAlmacen);
                        xRol.SetAttribute("codCiudad", alm.pCodCiudad);
                        xEl1.AppendChild(xRol);
                    }
                    xmlParam.DocumentElement.AppendChild(xEl1);
                    // FIN - armado de datos

                    xmlResp.LoadXml(objEjecucion.GrabaUsuarioAdicional(InitialiseService.Semilla,ValorTokenUser, xmlParam.OuterXml));
                    if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
                    {
                        FormResponse.success = true;
                        FormResponse.root.Add(userModel);
                        if (opcion == "New")
                        {
                            userModel.pIdParticipante = Int32.Parse(xmlResp.DocumentElement.GetAttribute("IdParticipante"));
                        PI_NombrePlantilla = "NewUserAdic.html"; //RFD0 - 2022 - 155
                        string asuntoEmail = "Nuevo Usuario - Portal de Proveedores";

                            clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();
                            


                        #region RFD0-2022-155 CORREO
                        Variables = new Dictionary<string, string>();
                        Variables.Add("@@Ruc", userModel.pRuc);
                        Variables.Add("@@RazonSocial", userModel.pRazonSocial);
                        Variables.Add("@@NombreUsuario",
                                userModel.pApellido1 + ' ' + userModel.pApellido2 + ' ' +
                                userModel.pNombre1 + ' ' + userModel.pNombre2);
                        Variables.Add("@@Usuario", userModel.pUsuario);
                        Variables.Add("@@Clave", userModel.pClave);
                        Variables.Add("@@RolesAsig", listaRoles);
                        Variables.Add("@@Apoderado", userModel.pApoderado);
                        Variables.Add("@@CodSap", userModel.pCodSap);

                        //SE AGREGA URL PORTAL
                        string UrlPortal = System.Configuration.ConfigurationManager.AppSettings["UrlPortal"];
                        Variables.Add("@@UrlPortal", UrlPortal);

                        PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();

                        #endregion


                        //Envio de pdf adjunto en correo
                        var rutaArchivo = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivoNewUsr"]).Trim();
                            var nomArchivo  = rutaArchivo.Split('\\');
                            FileInfo fInfo = new FileInfo(rutaArchivo);
                            long numBytes = fInfo.Length;
                            FileStream fStream = new FileStream(rutaArchivo,
                            FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fStream);
                            // convert the file to a byte array
                            byte[] data = br.ReadBytes((int)numBytes);

                            fStream.Close();
                        PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                        string retorno = objEnvMail.EnviaCorreoDF("", userModel.pCorreoE, "", "", asuntoEmail, "", true, true, true, data, nomArchivo[nomArchivo.Length - 1], PI_NombrePlantilla, PI_Variables);

                        if (retorno != "")
                            {
                                FormResponse.success = false;
                                FormResponse.codError = "-100";
                                FormResponse.msgError = "Los datos fueron actualizados pero se falló al enviar el correo con el ERROR: " + retorno;
                            }
                            
                        }
                    }
                    else
                    {
                        FormResponse.success = false;
                        FormResponse.codError = xmlResp.DocumentElement.GetAttribute("CodError");
                        FormResponse.msgError = xmlResp.DocumentElement.GetAttribute("MsgError");
                    }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }


        [ActionName("ResetClaveUsrAdic")]
        [HttpGet]
        public formResponseSeguridad GetResetClaveUsrAdic(string Ruc, string Usuario, string Clave, string Correo, string NombreUsuario, string RazonSocial)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            XmlDocument xmlResp = new XmlDocument();

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();

            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            clsSeguridad objEjecucion = new clsSeguridad();
            try
            {
                // el metodo [DesbloquearClave] también sirve como Reseteo de Clave si se envía el parámetros "CambiarClave" = true
                xmlResp.LoadXml(objEjecucion.DesbloquearClave(InitialiseService.Semilla, ValorTokenUser, Usuario, true, Clave, Ruc));
                if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
                {
                    FormResponse.success = true;
                    PI_NombrePlantilla = "ModClaveUserAdic.html"; //RFD0 - 2022 - 155

                    string asuntoEmail = "Recuperación de Contraseña - Portal Proveedores - El Rosado";
                    clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();
                    #region RFD0-2022-155 CORREO
                    Variables = new Dictionary<string, string>();
                    Variables.Add("@@Ruc", Ruc);
                    Variables.Add("@@NombreUsuario", NombreUsuario);
                    Variables.Add("@@Usuario", Usuario);
                    Variables.Add("@@Clave", Clave);
                    Variables.Add("@@RazonSocial", RazonSocial);

                    //SE AGREGA URL PORTAL
                    string UrlPortal = System.Configuration.ConfigurationManager.AppSettings["UrlPortal"];
                    Variables.Add("@@UrlPortal", UrlPortal);
                    #endregion
                    PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                    byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");

                    string retorno = objEnvMail.EnviaCorreoDF("", Correo, "", "", asuntoEmail, "", true, true, false, data, "", PI_NombrePlantilla, PI_Variables);

                    if (retorno != "")
                    {
                        FormResponse.success = false;
                        FormResponse.codError = "-100";
                        FormResponse.msgError = "La Contraseña fue modificada pero se falló al enviar el correo con el ERROR: " + retorno;
                    }

                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = xmlResp.DocumentElement.GetAttribute("CodError");
                    FormResponse.msgError = xmlResp.DocumentElement.GetAttribute("MsgError");
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }


        [ActionName("DesbloquearClaveUsrAdic")]
        [HttpGet]
        public formResponseSeguridad GetDesbloquearClaveUsrAdic(string Ruc, string Usuario, string Correo, string NombreUsuario, string RazonSocial)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            XmlDocument xmlResp = new XmlDocument();

            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();


            clsSeguridad objEjecucion = new clsSeguridad();
            try
            {
                xmlResp.LoadXml(objEjecucion.DesbloquearClave(InitialiseService.Semilla, ValorTokenUser, Usuario, false, "", Ruc));
                if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
                {
                    FormResponse.success = true;
                    PI_NombrePlantilla = "DesbloqClaveUserAdic.html"; //RFD0 - 2022 - 155


                    string asuntoEmail = "Desbloqueo de Contraseña - Portal de Proveedores";
                    clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();

                    #region RFD0-2022-155 CORREO
                    Variables = new Dictionary<string, string>();
                    Variables.Add("@@Ruc", Ruc);
                    Variables.Add("@@NombreUsuario", NombreUsuario);
                    Variables.Add("@@Usuario", Usuario);
                    Variables.Add("@@RazonSocial", RazonSocial);
                    #endregion

                    PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                    byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");

                    string retorno = objEnvMail.EnviaCorreoDF("", Correo, "", "", asuntoEmail, "", true, true, false, data, "", PI_NombrePlantilla, PI_Variables);


                    if (retorno != "")
                    {
                        FormResponse.success = false;
                        FormResponse.codError = "-100";
                        FormResponse.msgError = "La Contraseña fue desbloqueada pero se falló al enviar el correo con el ERROR: " + retorno;
                    }

                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = xmlResp.DocumentElement.GetAttribute("CodError");
                    FormResponse.msgError = xmlResp.DocumentElement.GetAttribute("MsgError");
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

    }
}