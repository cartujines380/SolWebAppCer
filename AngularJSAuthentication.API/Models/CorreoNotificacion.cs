using clibProveedores;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml;

namespace AngularJSAuthentication.API.Models
{
    public class CorreoNotificacion
    {
        #region "Proveedores"
        public void NotificacionProveedor(String IDNIVEL, String IDMODULO, String IDLINEA, String Proveedor, String Ruc, Boolean NotifProveedor, String correoProveedor, String Estado, String Motivo, String observacion, String codEstado)
        {
            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            XmlDocument xmlParam = new XmlDocument();
            List<DMSolcitudProveedor.SolNotificacion> Retorno = new List<DMSolcitudProveedor.SolNotificacion>();
            XmlDocument xmlResp = new XmlDocument();
            ClsGeneral objEjecucion = new ClsGeneral();
            DataSet ds = new DataSet();

            clibLogger.clsLogger _objLogServicio = new clibLogger.clsLogger();
            _objLogServicio.Graba_Log_Info("INI >>  Mail - NotificacionProveedor: " + PI_NombrePlantilla);

            try
            {
                xmlParam.LoadXml("<Root />");

                if (!String.IsNullOrEmpty(IDNIVEL))
                {
                    xmlParam.DocumentElement.SetAttribute("IDNIVEL", IDNIVEL);
                }

                if (!String.IsNullOrEmpty(IDMODULO))
                {
                    xmlParam.DocumentElement.SetAttribute("IDMODULO", IDMODULO);
                }

                if (!String.IsNullOrEmpty(IDLINEA))
                {
                    xmlParam.DocumentElement.SetAttribute("IDLINEA", IDLINEA);
                }

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 215, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    if (ds.Tables.Count > 1)
                    {

                        Retorno = (from reg in ds.Tables[0].AsEnumerable().AsParallel()
                                   select new DMSolcitudProveedor.SolNotificacion
                                   {
                                       Apellido1 = reg.Field<String>("Apellido1"),
                                       Apellido2 = reg.Field<String>("Apellido2"),
                                       Cargo = reg.Field<String>("Cargo"),
                                       CorreoE = reg.Field<String>("CorreoE"),
                                       DescLinea = reg.Field<String>("DescLinea"),
                                       DesModulo = reg.Field<String>("DesModulo"),
                                       DEsNivel = reg.Field<String>("DEsNivel"),
                                       DesTipoIdentificacion = reg.Field<String>("DesTipoIdentificacion"),
                                       IdEmpresa = reg["IdEmpresa"] != DBNull.Value ? reg["IdEmpresa"].ToString() : "",
                                       Linea = reg.Field<String>("Linea"),
                                       Modulo = reg.Field<String>("Modulo"),
                                       Nivel = reg.Field<String>("Nivel"),
                                       Nombre1 = reg.Field<String>("Nombre1"),
                                       Nombre2 = reg.Field<String>("Nombre2"),
                                       Ruc = reg.Field<String>("Ruc"),
                                       TipoIdent = reg.Field<String>("TipoIdent"),
                                       Usuario = reg.Field<String>("Usuario"),


                                   }).ToList<DMSolcitudProveedor.SolNotificacion>();
                    }

                    // ********************** CODIGO DEL METODO **********************

                    string asuntoEmail = "";

                    // ********************** CODIGO DEL METODO **********************

                    if (!NotifProveedor)
                    {
                        asuntoEmail = " Portal Provedores";

                        WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new WCFEnvioCorreo.ServEnvioClientSoapClient();


                        PI_NombrePlantilla = "NotificacionSolProveedor.html"; //RFD0 - 2022 - 155
                        _objLogServicio.Graba_Log_Info("INI >>  Mail - Plantilla: " + PI_NombrePlantilla);

                        foreach (DMSolcitudProveedor.SolNotificacion drow in Retorno)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(drow.CorreoE))
                                {
                                    //mensajeEmailparameter = mensajeEmailparameter.Replace("@@Ruc", Ruc);
                                    //mensajeEmailparameter = mensajeEmailparameter.Replace("@@NombreUsuario", drow.Nombre1 + " " + drow.Nombre2 + " " + drow.Apellido1 + " " + drow.Apellido2);
                                    //mensajeEmailparameter = mensajeEmailparameter.Replace("@@Usuario", Proveedor);
                                    //mensajeEmailparameter = mensajeEmailparameter.Replace("@@Motivo", Motivo);
                                    //mensajeEmailparameter = mensajeEmailparameter.Replace("@@Observacion", observacion);

                                    #region RFD0-2022-155 CORREO
                                    Variables = new Dictionary<string, string>();
                                    Variables.Add("@@Ruc", Ruc);
                                    Variables.Add("@@NombreUsuario", drow.Nombre1 + " " + drow.Nombre2 + " " + drow.Apellido1 + " " + drow.Apellido2);
                                    Variables.Add("@@Usuario", Proveedor);
                                    Variables.Add("@@Motivo", Motivo);
                                    Variables.Add("@@Observacion", observacion);
                                    PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();

                                    #endregion


                                    #region RFD0-2022-155 Variables CORREO
                                    _objLogServicio.Graba_Log_Info("INI >> Envio Mail.");
                                    _objLogServicio.Graba_Log_Info("Envia Mail -> NombrePlantilla: " + PI_NombrePlantilla + " Variables: " + PI_Variables);

                                    byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");
                                    string retorno = objEnvMail.EnviaCorreoApi("", drow.CorreoE, "", "", asuntoEmail, "", true, true, false, data, null, PI_NombrePlantilla, PI_Variables);
                                    _objLogServicio.Graba_Log_Info("Envia Mail -> Respuesta: " + retorno);

                                    #endregion RFD0-2022-155 Variables CORREO



                                    //string retorno = objEnvMail.EnviarCorreo("", drow.CorreoE, "", "", asuntoEmail, mensajeEmailparameter, true, true, false, null);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            string UrlPortal = string.Empty;
                            UrlPortal = System.Configuration.ConfigurationManager.AppSettings["UrlPortal"];
                            if (codEstado == "PA")
                            {
                                PI_NombrePlantilla = "NotProvPrecalificaAprobada.html"; //RFD0 - 2022 - 155
                                _objLogServicio.Graba_Log_Info("INI >>  Mail - Plantilla: " + PI_NombrePlantilla);
                                UrlPortal = System.Configuration.ConfigurationManager.AppSettings["UrlLoginSolicitud"];

                            }
                            else
                                 if (codEstado == "PR")
                            {
                                PI_NombrePlantilla = "NotProvPrecalificaRechaza.html"; //RFD0 - 2022 - 155
                                _objLogServicio.Graba_Log_Info("INI >>  Mail - Plantilla: " + PI_NombrePlantilla);

                            }
                            else
                            {
                                PI_NombrePlantilla = "NotificacionProveedor.html"; //RFD0 - 2022 - 155
                                _objLogServicio.Graba_Log_Info("INI >>  Mail - Plantilla: " + PI_NombrePlantilla);

                            }

                            asuntoEmail = " Portal Provedores";

                            WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new WCFEnvioCorreo.ServEnvioClientSoapClient();


                            if (!String.IsNullOrEmpty(correoProveedor))
                            {

                                #region RFD0-2022-155 CORREO
                                Variables = new Dictionary<string, string>();
                                Variables.Add("@@NombreUsuario", Proveedor);
                                Variables.Add("@@Estado", Estado);
                                Variables.Add("@@Motivo", Motivo);
                                Variables.Add("@@Observacion", observacion);
                                Variables.Add("@@Ruc", Ruc);

                                //SE AGREGA URL PORTAL                                
                                Variables.Add("@@UrlPortal", UrlPortal);

                                #endregion

                                #region RFD0-2022-155 Variables CORREO
                                PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                                _objLogServicio.Graba_Log_Info("INI >> Envio Mail.");
                                _objLogServicio.Graba_Log_Info("Envia Mail -> NombrePlantilla: " + PI_NombrePlantilla + " Variables: " + PI_Variables);

                                byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");
                                string retorno = objEnvMail.EnviaCorreoApi("", correoProveedor, "", "", asuntoEmail, "", true, true, false, data, null, PI_NombrePlantilla, PI_Variables);
                                _objLogServicio.Graba_Log_Info("Envia Mail -> Respuesta: " + retorno);

                                #endregion RFD0-2022-155 Variables CORREO

                                //string retorno = objEnvMail.EnviarCorreo("", correoProveedor, "", "", asuntoEmail, mensajeEmail, true, true, false, null);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else
                {
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        #endregion

        #region "Licitacion"

        public void NotificacionLicitacion(string titulo, string nombreComercial, string mensaje, string correoProveedor, string codEstado)
        {
            XmlDocument xmlParam = new XmlDocument();
            List<DMSolcitudProveedor.SolNotificacion> Retorno = new List<DMSolcitudProveedor.SolNotificacion>();
            XmlDocument xmlResp = new XmlDocument();
            ClsGeneral objEjecucion = new ClsGeneral();
            DataSet ds = new DataSet();

            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            try
            {
                //string rutaEmail = System.Web.Hosting.HostingEnvironment.MapPath("~/PlantillasEMail");
                string asuntoEmail = "";

                try
                {
                    switch (codEstado)
                    {
                        case "PL":
                            PI_NombrePlantilla = "NotPartLicitacion.html"; //RFD0 - 2022 - 155
                            break;
                        default:
                            break;
                    }

                    asuntoEmail = " Portal Provedores";

                    WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new WCFEnvioCorreo.ServEnvioClientSoapClient();

                    if (!String.IsNullOrEmpty(correoProveedor))
                    {
                        #region RFD0-2022-155 CORREO
                        Variables = new Dictionary<string, string>();
                        Variables.Add("@@titulo", titulo);
                        Variables.Add("@@NombreComercial", nombreComercial);
                        Variables.Add("@@Mensaje", mensaje);
                        #endregion

                        #region RFD0-2022-155 CORREO
                        PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                        byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");
                        string retorno = objEnvMail.EnviaCorreoApi("", correoProveedor, "", "", asuntoEmail, "", true, true, false,
                            data, null,
                            PI_NombrePlantilla, PI_Variables);
                        #endregion

                        //string retorno = objEnvMail.EnviarCorreo("", correoProveedor, "", "", asuntoEmail, mensajeEmail, true, true, false, null);
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        #endregion

    }
}