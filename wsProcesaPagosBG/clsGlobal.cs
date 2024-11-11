using System;
using System.Reflection;
using System.Xml;

namespace wsProcesaPagosBG
{
    public static class clsGlobal
    {
        static clsGlobal() { } // default value

        // public get, and private set for strict access control
        public static string CadenaConexion { get; private set; }
        public static string Msg = "";

        public static string ServidorSFTP = "";
        public static string UsuarioSFTP = "";
        public static string ClaveSFTP = "";
        public static string RutaSFTP = "";

        public static int intervaloServicioCorreo = 0;

        private static XmlDocument _xmlPerfilSitio = null;

        public static XmlDocument xmlPerfilSitio
        {
            get
            {
                return _xmlPerfilSitio;
            }
        }

        public static bool SetLoginAplicacion()
        {
            bool lv_Retorno = false;
            Msg = "";
            string _metodo = MethodBase.GetCurrentMethod().Name;
            string _clase = "ClsGlobal SetLoginAplicacion";
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();

            try
            {
                p_Log.Graba_Log((Convert.ToString(_clase + Convert.ToString(" ")) + _metodo) + " INICIO ", "");
                string IdApl = ((string)System.Configuration.ConfigurationManager.AppSettings["IdAplicacionApl"]).Trim();
                string IdUsr = ((string)System.Configuration.ConfigurationManager.AppSettings["IdUsuarioApl"]).Trim();
                string IdUsrLocal = ((string)System.Configuration.ConfigurationManager.AppSettings["IdUsrlocal"]).Trim();
                string Maquina = "::1";
                //para el caso que pase por proxi
                //if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                //{
                //    Maquina = System.Web.HttpContext.Current.Request.UserHostAddress;
                //}
                //else
                //{
                //    Maquina = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                //}

                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();

                string[] datos = Proceso.ConsSemilla(IdApl, IdUsrLocal, IdUsr, Maquina);

                XmlDocument XmlConexion = new XmlDocument();
                XmlConexion.LoadXml(datos[1]);

                _xmlPerfilSitio = XmlConexion;
                
                XmlElement XmlDatos = (XmlElement)_xmlPerfilSitio.SelectSingleNode("/Registro/Servidores/Servidor[@Id='" + System.Configuration.ConfigurationManager.AppSettings["IdServidorEjecutaSQL"].ToString() + "']");
                if (XmlDatos.GetAttribute("Usuario").Trim().Equals(""))
                {
                    CadenaConexion = "Data Source=" + XmlDatos.GetAttribute("Servidor") + ";Initial Catalog=" + XmlDatos.GetAttribute("BaseDatos") + ";Integrated Security=true;Max Pool Size=" + XmlDatos.GetAttribute("MaxPool");
                }
                else
                {
                    CadenaConexion = "Data Source=" + XmlDatos.GetAttribute("Servidor") + ";Initial Catalog=" + XmlDatos.GetAttribute("BaseDatos") + ";User ID=" + XmlDatos.GetAttribute("Usuario") + ";Password=" + XmlDatos.GetAttribute("Clave") + ";Max Pool Size=" + XmlDatos.GetAttribute("MaxPool");
                }

                //SFTP
                ServidorSFTP = _xmlPerfilSitio.DocumentElement.GetAttribute("sftpServerBG");
                UsuarioSFTP = _xmlPerfilSitio.DocumentElement.GetAttribute("sftpUserBG");
                ClaveSFTP = _xmlPerfilSitio.DocumentElement.GetAttribute("sftpPassBG");
                RutaSFTP = _xmlPerfilSitio.DocumentElement.GetAttribute("sftpRutaBG");

                p_Log.Graba_Log_Info("Server: "+ ServidorSFTP + " User: " + UsuarioSFTP + " Ruta: " + RutaSFTP);

                lv_Retorno = true;
             
            }
            catch (Exception ex)
            {
                lv_Retorno = false;
                CadenaConexion = "";
                Msg = ex.Message;
                p_Log.Graba_Log_Error((Convert.ToString(_clase + Convert.ToString(" ")) + _metodo) + " FIN - ERROR: " + ex.Message);
            }
            p_Log.Graba_Log((Convert.ToString(_clase + Convert.ToString(" ")) + _metodo) + " FIN ", "");
            return lv_Retorno;
        }
       
    }
}
