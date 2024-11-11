using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Renci.SshNet;
using AngularJSAuthentication.API.Models;
using Logger;
using System.Reflection;

namespace AngularJSAuthentication.API.Controllers
{
    public class UploadController : ApiController
    {
        private string _clase;

        public UploadController()
        {
            _clase = GetType().Name;
        }

        [System.Web.Http.HttpPost]
        public void UploadFile(string path)
        {
            string _horaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _metodo = MethodBase.GetCurrentMethod().Name;
            _metodo += " " + _horaEjecucion;

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["file"];
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path));
                if (!folderExists)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path));
                }
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path), httpPostedFile.FileName);
                httpPostedFile.SaveAs(fileSavePath);

                //Copia del archivo
                folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments/PDF/" + path));
                if (!folderExists)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments/PDF/" + path));
                }
                p_Log.Graba_Log_Info("Archivo Up: " + folderExists);
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments/PDF/" + path));
                fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/PDF/" + path), httpPostedFile.FileName);
                httpPostedFile.SaveAs(fileSavePath);
                //Fin Copia
            }

            p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
        }
    }

    public class DownloadController : ApiController
    {
        private string _clase;

        public DownloadController()
        {
            _clase = GetType().Name;
        }

        [System.Web.Http.HttpPost]
        public string DownloadFile(string[] listaArchivos)
        {
            string _horaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _metodo = MethodBase.GetCurrentMethod().Name;
            _metodo += " " + _horaEjecucion;

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            int lgtmax = listaArchivos.Length - 1;
            var result = "";
            var directorio = listaArchivos[lgtmax];
            bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + directorio));
            if (!folderExists)
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + directorio));
            try
            {
                for (int i = 0; i < lgtmax; i++)
                {
                    var filename = listaArchivos[i];
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + directorio), filename);
                    p_Log.Graba_Log_Info("Archivo Down: " + directorio + " FileName: "+ filename);
                    //Validar si ya existe no descargar el archivo desde el FTP
                    var bajarFTP = false;  //sin FTP
                    if (File.Exists(fileSavePath))
                    {
                        //validar que la fecha creacion del archivo sea el mismo del dia
                        var fecha = File.GetCreationTime(fileSavePath);
                        if (DateTime.Now.Date == fecha.Date)
                        {
                            bajarFTP = false;
                        }

                    }
                    else{
                        try
                        {
                            p_Log.Graba_Log_Error("Archivo no existe en la ruta " + fileSavePath);
                            p_Log.Graba_Log_Info("Extracción desde BaseProceso");
                            ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                            Proceso.Url = ((string)System.Configuration.ConfigurationManager.AppSettings["Urlbase"]).Trim();
                            byte[] archivo = Proceso.LeePdfAdjunto(directorio, filename);
                            File.WriteAllBytes(fileSavePath, archivo);
                        }
                        catch (Exception ex) {
                            p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
                        }
                    }

                    if (bajarFTP)
                    {
                        string lv_Msg = "";
                        clsFTP lv_sFtp = new clsFTP();
                        lv_sFtp.lv_EsPasivo = false;
                        lv_sFtp.lv_IP = AppConfig.SftpServerIp;
                        lv_sFtp.lv_Puerto = Convert.ToInt32(AppConfig.SftpServerPort);
                        lv_sFtp.lv_Usuario = AppConfig.SftpServerUserName;
                        lv_sFtp.lv_Clave = AppConfig.SftpServerPassword;
                        string tutafinal = AppConfig.SftpPath + directorio + "/";
                        
                        byte[] retorno = lv_sFtp.ObtenerArchivo_Sftp(tutafinal, filename, lv_Msg);

                        File.WriteAllBytes(fileSavePath, retorno);
                        result = null;

                    }
                    var localFilePath = fileSavePath;

                    if (File.Exists(localFilePath))
                    {
                        result = "UploadedDocuments/" + directorio + "/";
                    }

                    p_Log.Graba_Log_Info(result);
                }
                
            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
            }

            p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

            return result;
        }
    }

    public class UploadSFTPController : ApiController
    {
        [System.Web.Http.HttpPost]
        public FormResponseNotificacion UploadFileSFTP(string [] listaArchivos)
        {
            FormResponseNotificacion respuesta = new FormResponseNotificacion();
             try{
                int lgtmax = listaArchivos.Length - 1;
                        
                var path = listaArchivos[lgtmax];
                var nomArchivo = "";
                string[] files = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path));

                foreach (var FileName in files)
                {              

                    var inde = FileName.LastIndexOf("\\");
                    nomArchivo = FileName.Substring(inde+1);

                    var existe = listaArchivos.Where(x => x == nomArchivo).FirstOrDefault();
                    if (existe == nomArchivo)
                    {
                        var fileSavePath = Path.Combine(FileName);
                        if (File.Exists(fileSavePath))
                        {
                            string lv_Msg = "";

                            clsFTP lv_sFtp = new clsFTP();
                            lv_sFtp.lv_EsPasivo = false;
                            lv_sFtp.lv_IP = AppConfig.SftpServerIp;
                            lv_sFtp.lv_Puerto = Convert.ToInt32(AppConfig.SftpServerPort);
                            lv_sFtp.lv_Usuario = AppConfig.SftpServerUserName;
                            lv_sFtp.lv_Clave = AppConfig.SftpServerPassword;

                            //Creación de Archivo a partir de un arreglo de bytes
                            byte[] contenido = File.ReadAllBytes(fileSavePath);
                            lv_sFtp.crearCarpeta( AppConfig.SftpPath, path);
                                    
                        }

                    }
                    else
                    { 
                        var fileSavePath = Path.Combine(FileName);
                        if (File.Exists(fileSavePath))
                        {
                            //#if !DEBUG
                            //File.Delete(fileSavePath);
                            //#endif
                        }
                    }


              
                
                }


                respuesta.success = true;

             }catch (Exception e)             
             {
                 respuesta.success = false;
                 respuesta.msgError = "Error al realizar carga de archivo";
             }

             return respuesta;

        }
    }


}
