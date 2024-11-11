using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AngularJSAuthentication.API.Models;
using System.Data;

using System.Xml;
using System.Xml.Linq;
using clibProveedores;
using System.Web;
using Renci.SshNet;
using System.IO;
using SAP.Middleware.Connector;
using System.Threading;
using Ionic.Zip;

namespace AngularJSAuthentication.API.Controllers
{


    //[Authorize]

    [RoutePrefix("api/ActasEnviaAdmin")]

    public class ActasEnviaAdminController : ApiController
    {
      
        [ActionName("ActasEnvia")]
        [HttpPost]
        public FormResponseActa GetActasEnvia(DMActas ModActas)
        {
            FormResponseActa resultado = new FormResponseActa();
            //ReporteAdministradorController descargaActas = new ReporteAdministradorController();
            var rutaActas = ((string)System.Configuration.ConfigurationManager.AppSettings["rutaArchivosActas"]).Trim();
            string destinarios = "";
            //Armar archivo .ZIP de todas las actas a enviar
            try
            {
                ZipFile FileZip = new ZipFile();
                var ms = new System.IO.MemoryStream();
                foreach (var listaArchivos in ModActas.p_ListaActas)
                {
                    //var rutaArchivo = rutaActas + listaArchivos.anio + "\\" + listaArchivos.mes + "\\" + listaArchivos.dia + "\\"  +listaArchivos.nombreArchivo;
                    //if (File.Exists(rutaArchivo))
                    //{
                        //FileInfo fInfo = new FileInfo(rutaArchivo);
                        //long numBytes = fInfo.Length; 
                        //FileStream fStream = new FileStream(rutaArchivo,
                        //FileMode.Open, FileAccess.Read);
                        //BinaryReader br = new BinaryReader(fStream);
                        //// convert the file to a byte array
                        //descargaActas.ExportarArchivoActaRecepcion(listaArchivos.nombreArchivo, listaArchivos.anio,listaArchivos.mes,listaArchivos.dia); 

                        ProcesoWs.ServBaseProceso datos = new ProcesoWs.ServBaseProceso();
                        byte[] bytes = datos.ArchivoActa(listaArchivos.nombreArchivo, listaArchivos.anio, listaArchivos.mes, listaArchivos.dia);
                        //BinaryReader br = new BinaryReader(bytes);
                        FileZip.AddEntry(listaArchivos.nombreArchivo, bytes);

                        //result = Request.CreateResponse(HttpStatusCode.OK);
                        //result.Content = new StreamContent(new MemoryStream(bytes));
                        //result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                        //result.Content.Headers.ContentDisposition.FileName = idarchivo;

                        //BinaryReader br = new BinaryReader(fStream);
                        //byte[] data = br.ReadBytes((int)numBytes);
                         
                       
                        //FileZip.AddEntry(listaArchivos.nombreArchivo, data);

                        bytes = ms.ToArray();
                        
                    //}

                    
                   
                }

                FileZip.Save(ms);
                var buffer = ms.ToArray();
               
                    //Armar lista de destinatarios a ENVIAR
                    foreach (var dest in ModActas.p_ListaUsuarios)
                    {
                        if(dest.isCheck)
                        {
                            destinarios = destinarios + dest.correoEnviar + ";";
                        }
                    }
                    if (destinarios != "")
                    {
                        destinarios = destinarios.Substring(0, destinarios.Length - 1);
                    }

                #region RFD0-2022-155 CORREO
                String PI_NombrePlantilla = "ActasEntRecepcion.html";
                Dictionary<string, string> Variables = new Dictionary<string, string>();

                Variables.Add("@@Proveedor", ModActas.p_RazonSocial);
                Variables.Add("@@Ruc", ModActas.p_Ruc);
                Variables.Add("@@Desde", ModActas.p_FecDesde);
                Variables.Add("@@Hasta", ModActas.p_FecHasta);

                #endregion

                    Thread t = new Thread(() =>  EnviarEmail(destinarios, "Actas de Entrega-Recepción", "", buffer, PI_NombrePlantilla, Variables));
                    t.Start();
                    resultado.mensaje = "";
                    resultado.success = true;

            }catch(Exception e){
                resultado.success = false;
                resultado.mensaje = e.Message.ToString();
            }
            return resultado;
        }

        private string EnviarEmail(string pCorreoE, string asuntoEmail, string mensajeEmail, byte [] archivo, 
            String PI_NombrePlantilla, Dictionary<string, string> Variables)
        {
            string retornon = "";
            #region RFD0-2022-155 Variables CORREO
            String PI_Variables = string.Empty;
            #endregion RFD0-2022-155 Variables CORREO

            try
            {
                var rutaActas = ((string)System.Configuration.ConfigurationManager.AppSettings["senderActas"]).Trim();
                clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();

                #region RFD0-2022-155 CORREO
                PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();

                retornon = objEnvMail.EnviaCorreoDF(rutaActas, pCorreoE, "", "", asuntoEmail, mensajeEmail, true, true, true, archivo, "Actas_EntregaRecepcion.zip", PI_NombrePlantilla,
                    PI_Variables);
                #endregion

            }
            catch (Exception ex) { 
            }
            return retornon;
        }


      
    }


}

