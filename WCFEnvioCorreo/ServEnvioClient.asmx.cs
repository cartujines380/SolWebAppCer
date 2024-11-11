using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web.Services;
using WCFEnvioCorreo.Model;
using WCFEnvioCorreo.Servicios;
using WCFEnvioCorreo.Util;

namespace WCFEnvioCorreo
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ServEnvioClient : System.Web.Services.WebService
    {
        #region       VARIABLES
        private string _clase;
        #endregion

        #region       CONSTRUCTOR
        public ServEnvioClient()
        {
            _clase = this.GetType().Name;
        }
        #endregion

        [WebMethod]
        public string EnviaCorreoDF(
                       String PI_UsrEnvia, String PI_UsrDestino,
                       String PI_UsrDestinoCC, String PI_UsrDestinoCCO,
                       String PI_Asunto, 
                       String PI_Mensaje, Boolean PI_MostrarLogo, Boolean PI_EsHTML,
                       Boolean PI_TieneAdjuntos, byte[] PI_Adjunto, string fileName,
                       String PI_NombrePlantilla, string PI_Variables
          )
        {
            #region Log
            string metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();

            p_Log.Graba_Log_Info(_clase + " " + metodo + " INI ");

            p_Log.Graba_Log_Info("[EnviaCorreoApi] -> Envia: " + PI_UsrEnvia + " - Destino: " + PI_UsrDestino
                + " - Asunto: " + PI_Asunto + " - HTML: " + PI_EsHTML.ToString() + " - Adjunto: " + PI_Adjunto.ToString()
                + " - NombrePlantilla: " + PI_NombrePlantilla.ToString());
            #endregion Log

            string RespuestaString = String.Empty;
            string _conexion = String.Empty;
            DataSet dtRetorno;

            try
            {
                p_Log.Graba_Log_Info(string.Format("{0} {1} {2}", _clase, metodo, "LOGIN DE SERVICIO - INI"));
                if (!clsGlobal.SetLoginAplicacion())
                {
                    p_Log.Graba_Log_Info(string.Format("{0} {1} {2}", _clase, metodo, "LOGIN DE SERVICIO - ERROR.: "+ clsGlobal.Msg));
                    return null;
                }
                _conexion = clsGlobal.CadenaConexion;

                p_Log.Graba_Log_Info(string.Format("{0} {1} {2}", _clase, metodo, "LOGIN DE SERVICIO - FIN"));

                #region    ================> SECCION: Variables

                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(PI_Variables);
                StringBuilder result = new StringBuilder();
                foreach (var kvp in dictionary)
                {
                    result.Append($"{kvp.Key};{kvp.Value}|");
                }
                if (result.Length > 0)
                {
                    result.Length -= 1;
                }

                #endregion ================> SECCION: Variables

                #region    ================> SECCION USUARIOS DESTINOS


                PI_UsrDestino = PI_UsrDestino.Replace(";", ",");

                if (PI_UsrDestinoCC != "")
                {
                    PI_UsrDestinoCC = PI_UsrDestinoCC.Replace(";", ",");                  
                }

                if (PI_UsrDestinoCCO != "")
                {
                    PI_UsrDestinoCCO = PI_UsrDestinoCCO.Replace(";", ",");                 
                }

                #endregion ================> SECCION USUARIO DESTINO           

                //#region    ================> SECCION ADJUNTO
                //if (PI_TieneAdjuntos && PI_Adjunto.Length > 0)
                //{
                //    string rutaFisica = System.Configuration.ConfigurationManager.AppSettings["RutaFisicaDownload"].ToString().Trim();

                //    if (!File.Exists(rutaFisica + fileName))
                //    {
                //        MemoryStream ms = new MemoryStream(PI_Adjunto);
                //        FileStream fs = new FileStream(rutaFisica + fileName, FileMode.Create);
                //        ms.WriteTo(fs);

                //        ms.Close();
                //        fs.Close();
                //        fs.Dispose();
                //    }

                //}
                //#endregion ================> SECCION ADJUNTO


                p_Log.Graba_Log_Info(_clase + " " + metodo + " Conn: " + _conexion);

                OperadorBaseDatos op = new OperadorBaseDatos(_conexion);
                op.ProcedimientoAlmacenado = "[Notificacion].[Not_SendNotification]";

                op.AgregarParametro("@destinatarios", SqlDbType.VarChar, PI_UsrDestino);
                op.AgregarParametro("@asunto", SqlDbType.VarChar, PI_Asunto);
                op.AgregarParametro("@plantilla", SqlDbType.VarChar, PI_NombrePlantilla); 
                op.AgregarParametro("@tramBody", SqlDbType.VarChar, result.ToString());
                op.AgregarParametro("@nombreAdjunto", SqlDbType.VarChar, fileName);

                if (PI_TieneAdjuntos) 
                {
                    op.AgregarParametro("@adjunto", SqlDbType.VarBinary, PI_Adjunto);
                }
                

                op.AgregarParametro("@trx", SqlDbType.VarChar, "1");

                p_Log.Graba_Log_Info(_clase + " " + metodo + " ****Inicia ConsultarDataSet");
                dtRetorno = op.ConsultarDataSet();
                p_Log.Graba_Log_Info(_clase + " " + metodo + " ****Fin ConsultarDataSet");
                p_Log.Graba_Log_Info(_clase + " " + metodo + " FIN ");

                return RespuestaString;


            }
            catch (Exception ex)
            {
                RespuestaString = ex.Message;
                #region ================> SECCION LOG
                p_Log.Graba_Log_Info(metodo + " ERROR.: " + "CORREO DESTINO:" + PI_UsrDestino);
                p_Log.Graba_Log_Info(metodo + " ERROR INFO: " + RespuestaString);
                #endregion ================> SECCION LOG
                return RespuestaString;
            }

        }


    }
}
