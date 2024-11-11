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
using clibProveedores.Models;
using clibProveedores;
using clibSeguridadCR;
using System.Xml.Linq;
using System.IO;
using Renci.SshNet;
using AngularJSAuthentication.API.WCFEnvioCorreo;
using System.Threading;



namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/AprobacionCitas")]
    public class AprobacionCitasController : ApiController
    {
        [ActionName("conBuscarAprobacionCitas")]
        [HttpGet]
        public FormResponseTransporte conBuscarAprobacionCitas(string tipo, string fechadesde, string fechahasta, string numero, string estado, string codProveedor, string ruc)
        {
            List<Tra_BandejaAprobacionCitas> lst_retornoSolBandeja = new List<Tra_BandejaAprobacionCitas>();
            Tra_BandejaAprobacionCitas mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("numero", numero);
                xmlParam.DocumentElement.SetAttribute("codProveedor", codProveedor);
                xmlParam.DocumentElement.SetAttribute("ruc", ruc);
                xmlParam.DocumentElement.SetAttribute("fechadesde", fechadesde);
                xmlParam.DocumentElement.SetAttribute("fechahasta", fechahasta);
                if (estado != null)
                {
                    List<string> TagIds = estado.Split(',').Select(Convert.ToString).ToList();
                    for (int i = 0; i < TagIds.Count; i++)
                    {
                        XmlElement elem = xmlParam.CreateElement("Est");
                        elem.SetAttribute("id", TagIds[i].ToString());
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 311, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudBandeja = new Tra_BandejaAprobacionCitas();
                        mod_SolicitudBandeja.idconsolidacion = Convert.ToString(item["idconsolidacion"]);
                        mod_SolicitudBandeja.idcita = Convert.ToString(item["idcita"]);
                        mod_SolicitudBandeja.numcita = Convert.ToString(item["numcita"]);
                        mod_SolicitudBandeja.bodega = Convert.ToString(item["bodega"]);
                        mod_SolicitudBandeja.fechasolicitada = Convert.ToString(item["fechasolicitada"]);
                        mod_SolicitudBandeja.fechaasignada = Convert.ToString(item["fechaasignada"]);
                        mod_SolicitudBandeja.caducidadsolicitud = Convert.ToString(item["caducidadsolicitud"]);
                        mod_SolicitudBandeja.estado = Convert.ToString(item["estado"]);
                        mod_SolicitudBandeja.estadorechazo = Convert.ToString(item["estadorechazo"]);
                        mod_SolicitudBandeja.citarapida = Convert.ToString(item["citarapida"]);
                        lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSolBandeja);
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }


        public string bajararchivo(string identificacion, string archivo)
        {
            String result = null;
                       try
            {
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion));
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion), archivo);
                //httpPostedFile.SaveAs(fileSavePath);

                using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                {
                    sftpClient.Connect();
                    if (!sftpClient.Exists(AppConfig.SftpPath + "Transporte/" + identificacion))
                    {
                        sftpClient.CreateDirectory(AppConfig.SftpPath + "Transporte/" + identificacion);
                    }

                    Stream fin = File.OpenWrite(fileSavePath);
                    sftpClient.DownloadFile(AppConfig.SftpPath + "Transporte/" + identificacion + "/" + archivo, fin, null);

                    fin.Close();
                    sftpClient.Disconnect();
                }


                var localFilePath = fileSavePath;

                if (File.Exists(localFilePath))
                {

                    result = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/DownloadedDocuments/" + identificacion + "/" + archivo;
                }
            }
            catch
            {

            }
            return result;
        }

        public string bajararchivoVehiculo(string identificacion, string archivo)
        {
            String result = null;
            try
            {
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion));
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion), archivo);
                //httpPostedFile.SaveAs(fileSavePath);

                using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                {
                    sftpClient.Connect();
                    if (!sftpClient.Exists(AppConfig.SftpPath + "Vehiculo/" + identificacion))
                    {
                        sftpClient.CreateDirectory(AppConfig.SftpPath + "Vehiculo/" + identificacion);
                    }

                    Stream fin = File.OpenWrite(fileSavePath);
                    try
                    {
                        sftpClient.DownloadFile(AppConfig.SftpPath + "Vehiculo/" + identificacion + "/" + archivo, fin, null);
                    }
                    catch (Exception e)
                    {

                    }


                    fin.Close();
                    sftpClient.Disconnect();
                }


                var localFilePath = fileSavePath;

                if (File.Exists(localFilePath))
                {
                    result = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/DownloadedDocuments/" + identificacion + "/" + archivo;
                }
            }
            catch
            {

            }


            return result;
        }

        [ActionName("conBuscarDetalleGrid")]
        [HttpGet]
        public FormResponseTransporte conBuscarDetalleGrid(string tipo, string id)
        {
            List<Tra_DetalleAprobacionCitas> lst_retornoSolBandeja = new List<Tra_DetalleAprobacionCitas>();
            List<Tra_DetalleChoferVehiculo> lst_ChoferVehiculo = new List<Tra_DetalleChoferVehiculo>();
            Tra_DetalleAprobacionCitas mod_SolicitudBandeja;
            Tra_DetalleChoferVehiculo mod_ChoferVehiculo;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("idconsolidacion", id);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 313, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudBandeja = new Tra_DetalleAprobacionCitas();
                        mod_SolicitudBandeja.idconsolidacion = Convert.ToString(item["idconsolidacion"]);
                        mod_SolicitudBandeja.numpedido = Convert.ToString(item["numpedido"]);
                        mod_SolicitudBandeja.emisionpedido = Convert.ToString(item["emisionpedido"]);
                        mod_SolicitudBandeja.factura = Convert.ToString(item["factura"]);
                        mod_SolicitudBandeja.fechafactura = Convert.ToString(item["fechafactura"]);
                        mod_SolicitudBandeja.almacendestino = Convert.ToString(item["almacendestino"]);
                        mod_SolicitudBandeja.caducidadpedido = Convert.ToString(item["caducidadpedido"]);
                        lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                    }
                    FormResponse.root.Add(lst_retornoSolBandeja);
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        mod_ChoferVehiculo = new Tra_DetalleChoferVehiculo();
                        mod_ChoferVehiculo.nombreschoMostrar = Convert.ToString(item["nombreschoMostrar"]);
                        mod_ChoferVehiculo.cedulachoMostrar = Convert.ToString(item["cedulachoMostrar"]);
                        mod_ChoferVehiculo.telefonochoMostrar = Convert.ToString(item["telefonochoMostrar"]);
                        mod_ChoferVehiculo.imagenurlchofer = bajararchivo(Convert.ToString(item["cedulachoMostrar"]), Convert.ToString(item["imagenurlchofer"]));
                        mod_ChoferVehiculo.nombresasiMostrar = Convert.ToString(item["nombresasiMostrar"]);
                        mod_ChoferVehiculo.cedulaasiMostrar = Convert.ToString(item["cedulaasiMostrar"]);
                        mod_ChoferVehiculo.telefonoasiMostrar = Convert.ToString(item["telefonoasiMostrar"]);
                        mod_ChoferVehiculo.imagenurlasistente = bajararchivo(Convert.ToString(item["cedulaasiMostrar"]), Convert.ToString(item["imagenurlasistente"]));
                        mod_ChoferVehiculo.placavehiculoMostrar = Convert.ToString(item["placavehiculoMostrar"]);
                        mod_ChoferVehiculo.tipovehiculoMostrar = Convert.ToString(item["tipovehiculoMostrar"]);
                        mod_ChoferVehiculo.colorprincipalMostrar = Convert.ToString(item["colorprincipalMostrar"]);
                        mod_ChoferVehiculo.imagenurlvehiculo = bajararchivoVehiculo(Convert.ToString(item["placavehiculoMostrar"]), Convert.ToString(item["imagenurlvehiculo"]));

                        lst_ChoferVehiculo.Add(mod_ChoferVehiculo);
                    }
                    FormResponse.success = true;
                    FormResponse.root.Add(lst_ChoferVehiculo);
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }

        private string  EnviarEmail(string pCorreoE, string asuntoEmail, string mensajeEmail, String PI_NombrePlantilla, Dictionary<string, string> Variables)
        {
            string retornon = "";
            #region RFD0-2022-155 Variables CORREO
            String PI_Variables = string.Empty;
            #endregion RFD0-2022-155 Variables CORREO

            try
            {
                clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();

                #region RFD0-2022-155 CORREO
                PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");

                retornon = objEnvMail.EnviaCorreoDF("", pCorreoE, "", "", asuntoEmail, mensajeEmail, true, true, false, data, null, PI_NombrePlantilla,
                    PI_Variables);
                #endregion

                //retornon = objEnvMail.EnviarCorreo("", pCorreoE, "", "", asuntoEmail, mensajeEmail, true, true, false, null);
            }
            catch (Exception ex)
            {

            }
            return retornon;
        }
        [ActionName("getGrabarCancelar")]
        [HttpGet]
        public FormResponseTransporte getGrabarCancelar(string id, string codigo, string usuarioCreacion, string idcita)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();

            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "1");
                xmlParam.DocumentElement.SetAttribute("id", id);
                xmlParam.DocumentElement.SetAttribute("codigo", codigo);
                xmlParam.DocumentElement.SetAttribute("usuarioCreacion", usuarioCreacion);
                xmlParam.DocumentElement.SetAttribute("idcita", idcita);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 314, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = true;

                    string asuntoEmail = "";

                    PI_NombrePlantilla = "CorreoRechazo.html"; //RFD0 - 2022 - 155
                    asuntoEmail = "Rechazo de Cita - Portal de Proveedores";
                    if (FormResponse.success)
                    {
                        try
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                                                

                                #region RFD0-2022-155 CORREO
                                Variables = new Dictionary<string, string>();

                                Variables.Add("@@Proveedor", Convert.ToString(item["Proveedor"]));
                                Variables.Add("@@Cita", Convert.ToString(item["Cita"]));
                                Variables.Add("@@Motivo", Convert.ToString(item["Motivo"]));
                                Variables.Add("@@Usuario", Convert.ToString(item["Usuario"]));
                                #endregion

                                Thread t = new Thread(() => EnviarEmail(Convert.ToString(item["Correo"]), asuntoEmail,"", PI_NombrePlantilla, Variables));
                                t.Start();
                            }
                        }
                        catch (Exception es)
                        { }

                    }
                    else
                    {
                        FormResponse.success = false;
                    }
                }
            }
            catch (Exception ex)
            { FormResponse.success = false; }
            return FormResponse;

        }
    }
}
