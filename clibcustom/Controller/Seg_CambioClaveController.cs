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
using clibCustom;
using clibCustom.Models;
using AngularJSAuthentication.API;
using clibCustom.WCFEnvioCorreo;

namespace AngularJSAuthentication.API.Controllers
{

    [RoutePrefix("api/SegCambioClave")]
    public class SegCambioClaveController : ApiController
    {

        [ActionName("CambiarClave")]
        [HttpGet]
        public formResponseSeguridad GetCambiarClave(string Ruc, string Usuario, string ClaveAct, string ClaveNew, string Correo, string NombreUsr, string NomComercial)
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

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();


            //clibSeguridadCR.clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            clsSeguridad objEjecucion = new clsSeguridad();
            try
            {
                string errorValidaClave = "";
                if (!seg_Complexity.CheckPasswordComplexity(ClaveNew, ref errorValidaClave))
                {
                    FormResponse.success = false;
                    FormResponse.codError = "-10";
                    FormResponse.msgError = "Contraseña no cumple con requerimientos -> " + errorValidaClave;
                }
                else
                {
                    //objEjecucion.SetSemilla(InitialiseService.Semilla);
                    xmlResp.LoadXml(objEjecucion.CambiarClave(InitialiseService.Semilla,ValorTokenUser, Usuario, ClaveAct, ClaveNew, Ruc));
                    if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
                    {
                        FormResponse.success = true;
                        PI_NombrePlantilla = "CambioClaveUser.html"; //RFD0 - 2022 - 155

                        string asuntoEmail = "Cambio de Contraseña Exitoso - Portal de Proveedores";
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

                        if (retorno != "")
                        {
                            FormResponse.success = false;
                            FormResponse.codError = "-100";
                            FormResponse.msgError = "La Contraseña fue cambiada pero se falló al enviar el correo con el ERROR: " + retorno;
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
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

    }
}