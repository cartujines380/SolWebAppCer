using clibSeguridad.ProcesoWs;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Xml;

namespace WinSerCreaUsuario
{
    public static class clsGlobal
    {
        static clsGlobal() { } // default value

        // public get, and private set for strict access control
        public static string CadenaConexion { get; private set; }
        public static string _Semilla { get; private set; }
        public static string _PI_Session { get; private set; }


        public static string Msg = "";

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
            string _clase = "ClsGlobal";
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();

            try
            {
                _Semilla = "";

                p_Log.Graba_Log_Info(_clase + " " + _metodo + " INICIO ");
                string IdApl = ((string)System.Configuration.ConfigurationManager.AppSettings["IdAplicacionApl"]).Trim();
                string IdUsr = ((string)System.Configuration.ConfigurationManager.AppSettings["IdUsuarioApl"]).Trim();
                string IdUsrLocal = ((string)System.Configuration.ConfigurationManager.AppSettings["IdUsrlocal"]).Trim();
                string Maquina = "::1";
                string IdEmpresa = ((string)System.Configuration.ConfigurationManager.AppSettings["IdEmpresa"]).Trim();

                string IdSucursal = ((string)System.Configuration.ConfigurationManager.AppSettings["IdSucursal"]).Trim();

                //para el caso que pase por proxi
                //if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                //{
                //    Maquina = System.Web.HttpContext.Current.Request.UserHostAddress;
                //}
                //else
                //{
                //    Maquina = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                //}

                ServBaseProceso Proceso = new ServBaseProceso();
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " UrlProcesaBase: " + System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString());
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " ConsSemilla: " + IdApl + "|" + IdUsrLocal + "|" + IdUsr + "|" + Maquina);

                string[] datos = Proceso.ConsSemilla(IdApl, IdUsrLocal, IdUsr, Maquina);

                XmlDocument XmlConexion = new XmlDocument();
                XmlDocument XmlOtros = new XmlDocument();

                XmlConexion.LoadXml(datos[1]);
                string jsonString = JsonConvert.SerializeObject(datos[1]);

                p_Log.Graba_Log_Info(_clase + " " + _metodo + " Datos 1: " + jsonString);

                _Semilla = datos[2];
                jsonString = JsonConvert.SerializeObject(datos[2]);
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " Datos 2: " + jsonString);

                XmlConexion.DocumentElement.SetAttribute("PS_IdEmpresa", IdEmpresa);
                XmlConexion.DocumentElement.SetAttribute("PS_IdSucursal", IdSucursal);
                
                jsonString = JsonConvert.SerializeObject(datos[0]);
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " Datos 0: " + jsonString);

                XmlOtros.LoadXml(datos[0]);

                _xmlPerfilSitio = XmlConexion;

                _PI_Session = "";
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();

                try
                {
                    _PI_Session = objSeg.getSesionIni(_xmlPerfilSitio);
                    p_Log.Graba_Log_Info(_clase + " " + _metodo + " _PI_Session: " + _PI_Session);
                }
                catch (Exception)
                {
                    _PI_Session = "";
                    p_Log.Graba_Log_Info(_clase + " " + _metodo + " E_PI_Session: " + _PI_Session);
                }

                XmlElement XmlDatos = (XmlElement)_xmlPerfilSitio.SelectSingleNode("/Registro/Servidores/Servidor[@Id='" + System.Configuration.ConfigurationManager.AppSettings["IdServidorEjecutaSQL"].ToString() + "']");
                if (XmlDatos.GetAttribute("Usuario").Trim().Equals(""))
                {
                    CadenaConexion = "Data Source=" + XmlDatos.GetAttribute("Servidor") + ";Initial Catalog=" + XmlDatos.GetAttribute("BaseDatos") + ";Integrated Security=true;Max Pool Size=" + XmlDatos.GetAttribute("MaxPool");
                }
                else
                {
                    CadenaConexion = "Data Source=" + XmlDatos.GetAttribute("Servidor") + ";Initial Catalog=" + XmlDatos.GetAttribute("BaseDatos") + ";User ID=" + XmlDatos.GetAttribute("Usuario") + ";Password=" + XmlDatos.GetAttribute("Clave") + ";Max Pool Size=" + XmlDatos.GetAttribute("MaxPool");
                }

                p_Log.Graba_Log_Info(_clase + " Conn: " + CadenaConexion + " .");
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
