using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Models;

using System.Data;
using System.Xml;
using System.Security.Claims;
//using clibProveedores.Models;
//using clibProveedores;
using clibSeguridadCR;
using clibCustom.Models;
using clibCustom;


namespace AngularJSAuthentication.API.Controllers
{

    [RoutePrefix("api/SegRecuperaClave")]
    public class SegRecuperaClaveController : ApiController
    {

        [ActionName("RecuperaClaveValidar")]
        [HttpGet]
        public formResponseSeguridad GetRecuperaClaveValidar(string Ruc, string Correo, string Usuario)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            seg_GrabaUsrFirstLogon objSegCons = new seg_GrabaUsrFirstLogon();
            DataSet ds = new DataSet();
            //clsClienteSeg objEjecucion = new clsClienteSeg();
            clsSeguridad objEjecucion = new clsSeguridad();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Correo", Correo);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);
                ds = objEjecucion.GetRecuperaClaveValidar(InitialiseService.Semilla, InitialiseService.IdOrganizacion, InitialiseService.PI_Session, xmlParam);
                //objEjecucion.SetSemilla(InitialiseService.Semilla);
                //objEjecucion.IdOrganizacion = InitialiseService.IdOrganizacion;
                //objEjecucion.IdTransaccion = 12;
                //objEjecucion.IdOpcion = 1;
                //objEjecucion.ArrParams = new Object[1] {
                //    xmlParam.OuterXml
                //};
                //  ds = objEjecucion.EjecutaTransaccionDS(InitialiseService.PI_Session);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        seg_GrabaUsrFirstLogon.seg_GrabaUsrFirstLogonItem objInfUsr = new seg_GrabaUsrFirstLogon.seg_GrabaUsrFirstLogonItem();
                        DataRow dr = ds.Tables[0].Rows[0];
                        objInfUsr.pRuc = Convert.ToString(dr["Ruc"]);
                        objInfUsr.pUsuario = Convert.ToString(dr["Usuario"]);
                        objInfUsr.pIdParticipante = Convert.ToInt32(dr["IdParticipante"]);
                        objInfUsr.pNombre = Convert.ToString(dr["Nombre"]);
                        objInfUsr.pCorreoE = Convert.ToString(dr["CorreoE"]);
                        objInfUsr.pCelular = Convert.ToString(dr["Celular"]);
                        objInfUsr.pNomComercial = Convert.ToString(dr["NomComercial"]);
                        //FS
                        seg_GrabaUsrFirstLogon.seg_RespuestasSeguridad[] objRespSeg = null;
                        //clsClienteSeg objEjecFS = new clsClienteSeg();
                        clsSeguridad objEjecFS = new clsSeguridad();
                        //objEjecFS.SetSemilla(InitialiseService.Semilla);
                        DataSet ds2 = objEjecFS.ConsInformacionSeguraUsuarioFS(InitialiseService.Semilla, objInfUsr.pRuc, InitialiseService.PI_Session, objInfUsr.pUsuario);
                        if (ds2.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                        {
                            foreach (DataRow dr1 in ds2.Tables[0].Rows)
                            {
                                objInfUsr.pCodImgSegura = Convert.ToString(dr1["CodImagenSecreta"]);
                            }
                            if (ds2.Tables[1].Rows.Count > 0)
                            {
                                objRespSeg = new seg_GrabaUsrFirstLogon.seg_RespuestasSeguridad[ds2.Tables[1].Rows.Count];
                                int i = 0;
                                foreach (DataRow dr2 in ds2.Tables[1].Rows)
                                {
                                    seg_GrabaUsrFirstLogon.seg_RespuestasSeguridad objResp = new seg_GrabaUsrFirstLogon.seg_RespuestasSeguridad();
                                    objResp.pCodigo = Convert.ToString(dr2["CodPregunta"]);
                                    objResp.pPregunta = Convert.ToString(dr2["DesPregunta"]);
                                    objResp.pRespuesta = Convert.ToString(dr2["Respuesta"]);
                                    objRespSeg[i] = objResp;
                                    i++;
                                }
                            }
                        }
                        else
                        {
                            FormResponse.success = false;
                            FormResponse.codError = ds2.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                            FormResponse.msgError = ds2.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                            return FormResponse;
                        }

                        objSegCons.pDatosUsr = objInfUsr;
                        objSegCons.pRespSeg = objRespSeg;
                    }
                    FormResponse.success = true;
                    FormResponse.root.Add(objSegCons);
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

        


        [ActionName("RecuperaClaveEnviarTmp")]
        [HttpGet]
        public formResponseSeguridad GetRecuperaClaveEnviarTmp(string Ruc, string Usuario, string ClaveTmp, string Correo, string NombreUsr, string NomComercial)
        {
            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            XmlDocument xmlResp = new XmlDocument();
            //clibSeguridadCR.clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            clsSeguridad objEjecucion = new clsSeguridad();
            try
            {
               // objEjecucion.SetSemilla(InitialiseService.Semilla);
                // el metodo [DesbloquearClave] también sirve como Reseteo de Clave si se envía el parámetros "CambiarClave" = true
                //xmlResp = objEjecucion.DesbloquearClave(InitialiseService.PI_Session, Usuario, true, ClaveTmp, Ruc);
                xmlResp.LoadXml(objEjecucion.DesbloquearClave(InitialiseService.Semilla, InitialiseService.PI_Session, Usuario, true, ClaveTmp, Ruc));
                if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
                {
                    FormResponse.success = true;
                    PI_NombrePlantilla = "RecuperarClaveEnvio.html"; //RFD0 - 2022 - 155

                    string asuntoEmail = "Recuperación de Contraseña - Portal de Proveedores";
                    //string asuntoEmail = "Recuperación de Contraseña - Portal Provedores - Sipecom";
                    clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();

                    #region RFD0-2022-155 CORREO
                    Variables = new Dictionary<string, string>();
                    Variables.Add("@@Ruc", Ruc);
                    Variables.Add("@@NombreUsuario", NombreUsr);
                    Variables.Add("@@Usuario", Usuario);
                    Variables.Add("@@Clave", ClaveTmp);
                    Variables.Add("@@RazonSocial", NomComercial);
                    #endregion
                    PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                    byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");

                    //string retorno = objEnvMail.EnviarCorreo("", Correo, "", "", asuntoEmail, mensajeEmail, true, true, false, null);
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


        [ActionName("RecuperaClaveCambiar")]
        [HttpGet]
        public formResponseSeguridad GetRecuperaClaveCambiar(string Ruc, string Usuario, string Clave, string Correo, string NombreUsr, string NomComercial)
        {
            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            XmlDocument xmlResp = new XmlDocument();
            //clibSeguridadCR.clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            clsSeguridad objEjecucion = new clsSeguridad();
            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();


            try
            {
                string errorValidaClave = "";
                if (!seg_Complexity.CheckPasswordComplexity(Clave, ref errorValidaClave))
                {
                    FormResponse.success = false;
                    FormResponse.codError = "-10";
                    FormResponse.msgError = "Contraseña no cumple con requerimientos -> " + errorValidaClave;
                }
                else
                {
                    //objEjecucion.SetSemilla(InitialiseService.Semilla);
                    //xmlResp = objEjecucion.CambiarClaveRecupera(ValorTokenUser, Usuario, Clave, Ruc);
                    xmlResp.LoadXml(objEjecucion.CambiarClaveRecupera(InitialiseService.Semilla,ValorTokenUser, Usuario, Clave, Ruc));
                    if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
                    {
                        FormResponse.success = true;
                        PI_NombrePlantilla = "CambioClaveUser.html"; //RFD0 - 2022 - 155

                        string asuntoEmail = "Cambio de Contraseña Exitoso - Portal de Proveedores";
                        //string asuntoEmail = "Cambio de Contraseña Exitoso - Portal Provedores - Sipecom";
                        clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();

                        #region RFD0-2022-155 CORREO
                        Variables = new Dictionary<string, string>();
                        Variables.Add("@@Ruc", Ruc);
                        Variables.Add("@@NombreUsuario", NombreUsr);
                        Variables.Add("@@Usuario", Usuario);
                        Variables.Add("@@RazonSocial", NomComercial);

                        //SE AGREGA URL PORTAL
                        string UrlPortal = System.Configuration.ConfigurationManager.AppSettings["UrlPortal"];
                        Variables.Add("@@UrlPortal", UrlPortal);
                        #endregion
                        PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                        byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");

                        string retorno = objEnvMail.EnviaCorreoDF("", Correo, "", "", asuntoEmail, "", true, true, false, data, "", PI_NombrePlantilla, PI_Variables);

                        //string retorno = objEnvMail.EnviarCorreo("", Correo, "", "", asuntoEmail, mensajeEmail, true, true, false, null);
                        if (retorno != "")
                        {
                            FormResponse.success = false;
                            FormResponse.codError = "-100";
                            FormResponse.msgError = "La Contraseña fue cambiada pero se falló al enviar el correo con el ERROR: " + retorno;
                        }

                        //WCFEnvioCorreo.CompositeType objResp;
                        //objResp = objEnvMail.EnviarCorreo("", Correo, "", "", asuntoEmail, mensajeEmail, true, true, false, null);
                        //if (objResp.CodError != 0)
                        //{
                        //    FormResponse.success = false;
                        //    FormResponse.codError = objResp.CodError.ToString();
                        //    FormResponse.msgError = "La Contraseña fue cambiada pero se falló al enviar el correo con el ERROR: " + objResp.MsgError;
                        //}
                    }
                    else
                    {
                        FormResponse.success = false;
                        FormResponse.codError = xmlResp.DocumentElement.GetAttribute("CodError");
                        FormResponse.msgError = xmlResp.DocumentElement.GetAttribute("MsgError");
                    }
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