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
using System.IO;


namespace AngularJSAuthentication.API.Controllers
{
          [Authorize]       
    [RoutePrefix("api/SegBandjUsrsAdmin")]
    public class SegBandjUsrsAdminController : ApiController
    {
        readonly string MetodoController = "SegBandjUsrsAdminController";
        [ActionName("ConsBandjUsrsAdmin")]
        [HttpGet]
        public formResponseSeguridad GetConsBandjUsrsAdmin(string CodSap, string Ruc, string Nombre, string ConUsuario, string Estado, string Usuario)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            List<seg_ConsBandejaUsrAdmin> lst_retornoSol = new List<seg_ConsBandejaUsrAdmin>();
            seg_ConsBandejaUsrAdmin mod_objSeg;

            DataSet ds = new DataSet();
            clsSeguridad objEjecucion = new clsSeguridad();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();
         


            try
            {
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                if (CodSap != null && CodSap != "")
                    xmlParam.DocumentElement.SetAttribute("CodSAP", CodSap);
                if (Ruc != null && Ruc != "")
                    xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                if (Nombre != null && Nombre != "")
                    xmlParam.DocumentElement.SetAttribute("Nombre", Nombre);
                if (Estado != null && Estado != "")
                    xmlParam.DocumentElement.SetAttribute("Estado", Estado);
                if (ConUsuario != null && ConUsuario != "")
                    xmlParam.DocumentElement.SetAttribute("ConUsuario", ConUsuario);
                if (Usuario != null && Usuario != "")
                    xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);

                ds = objEjecucion.ConsultaBandejaUsuariosAdministradores(InitialiseService.Semilla, ValorTokenUser, xmlParam);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count > 1)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_objSeg = new seg_ConsBandejaUsrAdmin();
                            mod_objSeg.ruc = Convert.ToString(item["Ruc"]);
                            mod_objSeg.codSAP = Convert.ToString(item["CodSAP"]);
                            mod_objSeg.razonSocial = Convert.ToString(item["RazonSocial"]);
                            mod_objSeg.correoE = Convert.ToString(item["CorreoE"]);
                            mod_objSeg.telefono = Convert.ToString(item["Telefono"]);
                            mod_objSeg.celular = Convert.ToString(item["Celular"]);
                            mod_objSeg.usuario = Convert.ToString(item["Usuario"]);
                            mod_objSeg.estado = Convert.ToString(item["Estado"]);
                            mod_objSeg.idParticipante = Convert.ToInt32(item["IdParticipante"]);
                            mod_objSeg.IdRepresentante = Convert.ToString(item["IdRepresentante"]);
                            lst_retornoSol.Add(mod_objSeg);
                        }
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


        [ActionName("GrabarUsrAdmin")]
        [HttpPost]
        public formResponseSeguridad GetGrabarUsrAdmin(seg_GrabaUsrAdmin userModel)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            XmlDocument xmlParam = new XmlDocument();
            XmlElement xEl1;
            XmlDocument xmlResp = new XmlDocument();
            //clibSeguridadCR.clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            clsSeguridad objEjecucion = new clsSeguridad();

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();

            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(MetodoController + " -- GetGrabarUsrAdmin " + " INI ");
            try
            {
                xmlParam.LoadXml("<Root />");
                // INI - armado de datos
                string opcion = "Mod";
                if (userModel.pUsuario == "")
                {
                    opcion = "New";
                }
                xEl1 = xmlParam.CreateElement(opcion);
                xEl1.SetAttribute("IdEmpresa", "1");
                xEl1.SetAttribute("Ruc", userModel.pRuc);
                if (opcion == "Mod")
                {
                    xEl1.SetAttribute("Usuario", userModel.pUsuario);
                    xEl1.SetAttribute("IdParticipante", userModel.pIdParticipante.ToString());
                }
                xEl1.SetAttribute("CodProveedor", userModel.pCodSap);
                xEl1.SetAttribute("CorreoE", userModel.pCorreoE);
                xEl1.SetAttribute("Telefono", userModel.pTelefono);
                xEl1.SetAttribute("Celular", userModel.pCelular);
                xEl1.SetAttribute("Estado", userModel.pEstado);
                if (opcion == "New")
                {
                    xEl1.SetAttribute("Clave", userModel.pClave);
                    xEl1.SetAttribute("Nombre", userModel.pNombre);
                }
                XmlElement xRol;
                xRol = xmlParam.CreateElement("Rol");
                xRol.SetAttribute("IdRol", "3"); //rol default para usuarios que pueden hacer logon
                xEl1.AppendChild(xRol);
                xRol = xmlParam.CreateElement("Rol");
                xRol.SetAttribute("IdRol", "21"); //rol con Permisos de Usuarios Proveedores Básicos
                xEl1.AppendChild(xRol);
                xRol = xmlParam.CreateElement("Rol");
                //xRol.SetAttribute("IdRol", "23"); //rol con Permisos de Usuarios Proveedores Administradores
                xRol.SetAttribute("IdRol", "24"); //rol con Permisos de Usuarios Proveedores Administradores
                xEl1.AppendChild(xRol);
                xmlParam.DocumentElement.AppendChild(xEl1);
                // FIN - armado de datos
                //objEjecucion.SetSemilla(InitialiseService.Semilla);
                //xmlResp = objEjecucion.GrabaUsuarioAdministrador(ValorTokenUser, xmlParam.OuterXml);
                xmlResp.LoadXml( objEjecucion.GrabaUsuarioAdministrador(InitialiseService.Semilla,ValorTokenUser, xmlParam));
                if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
                {
                    //string rutaEmail = System.Web.Hosting.HostingEnvironment.MapPath("~/PlantillasEMail");
                    string asuntoEmail = "";
                    if (opcion == "New")
                    {
                        userModel.pUsuario = xmlResp.DocumentElement.GetAttribute("Usuario");
                        userModel.pIdParticipante = Int32.Parse(xmlResp.DocumentElement.GetAttribute("IdParticipante"));
                        PI_NombrePlantilla = "NewUserAdmin.html"; //RFD0 - 2022 - 155
                        asuntoEmail = "Nuevo Usuario - Portal de Proveedores";
                    }
                    FormResponse.success = true;
                    FormResponse.root.Add(userModel);
                    if (opcion == "Mod" && userModel.pClave != "")
                    {
                        // el metodo [DesbloquearClave] también sirve como Reseteo de Clave si se envía el parámetros "CambiarClave" = true
                        //xmlResp = objEjecucion.DesbloquearClave(ValorTokenUser, userModel.pUsuario, true, userModel.pClave, userModel.pRuc);
                        xmlResp.LoadXml(objEjecucion.DesbloquearClave(InitialiseService.Semilla, ValorTokenUser, userModel.pUsuario, true, userModel.pClave, userModel.pRuc));
                        if (xmlResp.DocumentElement.GetAttribute("CodError") != "0")
                        {
                            FormResponse.success = false;
                            FormResponse.codError = xmlResp.DocumentElement.GetAttribute("CodError");
                            FormResponse.msgError = "Los datos fueron actualizados pero se falló al cambiar la contraseña con el ERROR: " + xmlResp.DocumentElement.GetAttribute("MsgError");
                        }
                        PI_NombrePlantilla = "ModClaveUserAdmin.html"; //RFD0 - 2022 - 155
                        asuntoEmail = "Cambio de Contraseña - Portal de Proveedores";
                    }
                    if (FormResponse.success && userModel.pClave != "")
                    {
                        clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();


                        #region RFD0-2022-155 CORREO
                        Variables = new Dictionary<string, string>();

                        Variables.Add("@@Ruc", userModel.pRuc);
                        Variables.Add("@@CodSap", userModel.pCodSap);
                        Variables.Add("@@NombreRazonSocial", userModel.pNombre);
                        Variables.Add("@@Usuario", userModel.pUsuario);
                        Variables.Add("@@Clave", userModel.pClave);

                        //SE AGREGA URL PORTAL
                        string UrlPortal = System.Configuration.ConfigurationManager.AppSettings["UrlPortal"];
                        Variables.Add("@@UrlPortal", UrlPortal);
                        #endregion

                        //mensajeEmail = mensajeEmail.Replace("_@Ruc", userModel.pRuc);
                        //mensajeEmail = mensajeEmail.Replace("_@CodSap", userModel.pCodSap);
                        //mensajeEmail = mensajeEmail.Replace("_@NombreRazonSocial", userModel.pNombre);
                        //mensajeEmail = mensajeEmail.Replace("_@Usuario", userModel.pUsuario);
                        //mensajeEmail = mensajeEmail.Replace("_@Clave", userModel.pClave);
                        if (userModel.pIdRepresentante != "")
                        {
                            Variables.Add("@@IdRepresentante", userModel.pIdRepresentante);

                        }
                        else
                        {
                            Variables.Add("@@IdRepresentante", userModel.pRuc);

                        }
                        var rutaArchivo = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivoNewUsr"]).Trim();
                        p_Log.Graba_Log_Info("GetGrabarUsrAdmin:" + " -- rutaArchivo: " + rutaArchivo);

                        string FileName = String.Empty;
                        byte[] data = null;
                        if (System.IO.File.Exists(rutaArchivo))
                        {
                            p_Log.Graba_Log_Info("GetGrabarUsrAdmin:" + " -- ExistsArchivo: " + rutaArchivo);

                            var nomArchivo = rutaArchivo.Split('\\');
                            FileInfo fInfo = new FileInfo(rutaArchivo);
                            long numBytes = fInfo.Length;
                            FileStream fStream = new FileStream(rutaArchivo,
                            FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fStream);
                            // convert the file to a byte array
                            data = br.ReadBytes((int)numBytes);

                            FileName = nomArchivo[nomArchivo.Length - 1];
                        }


                        #region RFD0-2022-155 CORREO
                        PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                        p_Log.Graba_Log_Info("GetGrabarUsrAdmin:" + " -- PI_Variables: " + PI_Variables);
                        p_Log.Graba_Log_Info("GetGrabarUsrAdmin:" + " -- objEnvMail: " + objEnvMail.Endpoint.Address.Uri.ToString());

                        string retorno = objEnvMail.EnviaCorreoDF("", userModel.pCorreoE, "", "", asuntoEmail, "", true, true, true, 
                            data, FileName,
                            PI_NombrePlantilla, PI_Variables);
                        #endregion

                        //string retorno = objEnvMail.EnviarCorreoAdjunto("", userModel.pCorreoE, "", "", asuntoEmail, mensajeEmail, true, true, true, data, nomArchivo[nomArchivo.Length - 1]);
                        if (retorno != "")
                        {
                            FormResponse.success = false;
                            FormResponse.codError = "-100";
                            FormResponse.msgError = "Los datos fueron actualizados pero se falló al enviar el correo con el ERROR: " + retorno;
                        }
                        //objResp = objEnvMail.EnviarCorreo("", userModel.pCorreoE, "", "", asuntoEmail, mensajeEmail, true,true, false, null);
                        //if (objResp.CodError != 0)
                        //{
                        //    FormResponse.success = false;
                        //    FormResponse.codError = objResp.CodError.ToString();
                        //    FormResponse.msgError = "Los datos fueron actualizados pero se falló al enviar el correo con el ERROR: " + objResp.MsgError;
                        //}
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
