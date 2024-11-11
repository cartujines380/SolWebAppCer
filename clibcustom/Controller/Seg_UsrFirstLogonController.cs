using System;
using System.Collections.Generic;
using System.Web.Http;

using System.Data;
using System.Xml;
using clibCustom;
using clibCustom.Models;
using System.IO;
using clibLogger;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/SegUsrFirstLogon")]
    public class SegUsrFirstLogonController : ApiController
    {

        [ActionName("ConsValUsrFirstLogon")]
        [HttpGet]
        public formResponseSeguridad GetConsValUsrFirstLogon(string Ruc, string Usuario)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            List<seg_ConsUsrFirstLogon> lst_retornoSol = new List<seg_ConsUsrFirstLogon>();
            seg_ConsUsrFirstLogon mod_objSeg;

            DataSet ds = new DataSet();

            clsSeguridad objEjecucion = new clsSeguridad();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();


            try
            {
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);

                ds = objEjecucion.GetConsValUsrFirstLogon(InitialiseService.Semilla, InitialiseService.IdOrganizacion, ValorTokenUser, xmlParam);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_objSeg = new seg_ConsUsrFirstLogon();
                        mod_objSeg.ruc = Convert.ToString(item["Ruc"]);
                        mod_objSeg.usuario = Convert.ToString(item["Usuario"]);
                        mod_objSeg.idParticipante = Convert.ToInt32(item["IdParticipante"]);
                        mod_objSeg.codSAP = Convert.ToString(item["CodSAP"]);
                        mod_objSeg.identReprLegal = Convert.ToString(item["IdentReprLegal"]);
                        mod_objSeg.correoE = Convert.ToString(item["CorreoE"]);
                        mod_objSeg.celular = Convert.ToString(item["Celular"]);
                        mod_objSeg.telefono = Convert.ToString(item["Telefono"]);
                        mod_objSeg.pNomComercial = Convert.ToString(item["NomComercial"]);
                        lst_retornoSol.Add(mod_objSeg);
                    }
                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSol);
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

        [ActionName("ValidaEmailUsrFirstLogon")]
        [HttpPost]
        public formResponseSeguridad GetValidaEmailUsrFirstLogon(string CorreoE, string CodigoValidacion, string NombreUsuario, string Ruc, string RazonSocial)
        {
            clsLogger _objLogServicio = new clsLogger();

            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();

            try
            {
                _objLogServicio.Graba_Log_Info("GetValidaEmailUsrFirstLogon: INI >> Envio Mail.");

                PI_NombrePlantilla = "CodValidacion.html"; //RFD0 - 2022 - 155

                string asuntoEmail = "Código de Validación Temporal - Portal Provedores";

                clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();

                #region RFD0-2022-155 CORREO
                Variables = new Dictionary<string, string>();
                Variables.Add("@@NombreUsuario", NombreUsuario);
                Variables.Add("@@CodigoValidacion", CodigoValidacion);
                Variables.Add("@@Ruc", Ruc);
                Variables.Add("@@RazonSocial", RazonSocial);
                #endregion

                PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");
                _objLogServicio.Graba_Log_Info("Envia Mail -> NombrePlantilla: " + PI_NombrePlantilla + " Variables: " + PI_Variables);

                string retorno = objEnvMail.EnviaCorreoDF("", CorreoE, "", "", asuntoEmail, "", true, true, false, data, "", PI_NombrePlantilla, PI_Variables);
                _objLogServicio.Graba_Log_Info("Envia Mail -> Respuesta: " + retorno);

                if (retorno != "")
                {
                    FormResponse.success = false;
                    FormResponse.codError = "-100";
                    FormResponse.msgError = retorno.ToString();
                }
            }
            catch (Exception ex)
            {
                _objLogServicio.Graba_Log_Info("GetValidaEmailUsrFirstLogon -> ERROR: " + ex.Message);

                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = "Intente más tarde.";
            }
            _objLogServicio.Graba_Log_Info("GetValidaEmailUsrFirstLogon: FIN >> Envio Mail.");

            return FormResponse;
        }

        [ActionName("GrabarUsrFirstLogon")]
        [HttpPost]
        public formResponseSeguridad GetGrabarUsrFirstLogon(seg_GrabaUsrFirstLogon userModel)
        {
            clsLogger _objLogServicio = new clsLogger();

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

            clsSeguridad objEjecucion = new clsSeguridad();

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();

            try
            {
                _objLogServicio.Graba_Log_Info("GetGrabarUsrFirstLogon: INI.");

                string errorValidaClave = "";
                if (!seg_Complexity.CheckPasswordComplexity(userModel.pDatosUsr.pClaveNew, ref errorValidaClave))
                {
                    FormResponse.success = false;
                    FormResponse.codError = "-10";
                    FormResponse.msgError = "Contraseña no cumple con requerimientos -> " + errorValidaClave;
                }
                else
                {
                    xmlParam.LoadXml("<Root />");
                    // INI - armado de datos
                    xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                    xmlParam.DocumentElement.SetAttribute("Ruc", userModel.pDatosUsr.pRuc);
                    xmlParam.DocumentElement.SetAttribute("Usuario", userModel.pDatosUsr.pUsuario);
                    xmlParam.DocumentElement.SetAttribute("IdParticipante", userModel.pDatosUsr.pIdParticipante.ToString());
                    xmlParam.DocumentElement.SetAttribute("CorreoE", userModel.pDatosUsr.pCorreoE);
                    xmlParam.DocumentElement.SetAttribute("Celular", userModel.pDatosUsr.pCelular);
                    xmlParam.DocumentElement.SetAttribute("Telefono", userModel.pDatosUsr.pTelefono);
                    xmlParam.DocumentElement.SetAttribute("CodImgSegura", userModel.pDatosUsr.pCodImgSegura);
                    xmlParam.DocumentElement.SetAttribute("ClaveNew", userModel.pDatosUsr.pClaveNew);
                    foreach (seg_GrabaUsrFirstLogon.seg_RespuestasSeguridad objResp in userModel.pRespSeg)
                    {
                        xEl1 = xmlParam.CreateElement("Resp");
                        xEl1.SetAttribute("Codigo", objResp.pCodigo);
                        xEl1.SetAttribute("Respuesta", objResp.pRespuesta);
                        xmlParam.DocumentElement.AppendChild(xEl1);
                    }
                    // FIN - armado de datos

                    xmlResp.LoadXml(objEjecucion.GrabaActivacionNuevoUsuario(InitialiseService.Semilla, ValorTokenUser, xmlParam));
                    if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
                    {
                        FormResponse.success = true;

                        PI_NombrePlantilla = "FinActivarLogin.html"; //RFD0 - 2022 - 155

                        string asuntoEmail = "Proceso Activación Usuario - Portal Provedores";

                        clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();
                        #region RFD0-2022-155 CORREO
                        Variables = new Dictionary<string, string>();
                        Variables.Add("@@Ruc", userModel.pDatosUsr.pRuc);
                        Variables.Add("@@Usuario", userModel.pDatosUsr.pUsuario);
                        Variables.Add("@@NombreUsuario", userModel.pDatosUsr.pNombre);
                        Variables.Add("@@RazonSocial", userModel.pDatosUsr.pNomComercial);
                        #endregion

                        var rutaArchivo = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivoNewUsr"]).Trim();
                        var nomArchivo = rutaArchivo.Split('\\');
                        FileInfo fInfo = new FileInfo(rutaArchivo);
                        long numBytes = fInfo.Length;
                        FileStream fStream = new FileStream(rutaArchivo,
                        FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fStream);
                        // convert the file to a byte array
                        byte[] data = br.ReadBytes((int)numBytes);

                        PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();

                        string retorno = objEnvMail.EnviaCorreoDF("", userModel.pDatosUsr.pCorreoE, "", "", asuntoEmail, "", true, true, true, 
                            data, nomArchivo[nomArchivo.Length - 1],
                            PI_NombrePlantilla, PI_Variables);


                        if (retorno != "")
                        {
                            FormResponse.success = false;
                            FormResponse.codError = "-100";
                            FormResponse.msgError = "El proceso fue completado pero se falló al enviar el correo de confirmación con el ERROR: " + retorno;
                        }

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
                _objLogServicio.Graba_Log_Info("GetGrabarUsrFirstLogon -> ERROR: " + ex.Message);
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            _objLogServicio.Graba_Log_Info("GetGrabarUsrFirstLogon: FIN.");

            return FormResponse;
        }

    }
}
