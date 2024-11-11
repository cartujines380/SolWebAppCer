using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.IO;

using System.Net;
using AngularJSAuthentication.API.Custom;
using AngularJSAuthentication.API.Models;

using System.Linq;
using System.Security.Claims;

  
public static class InitialiseService
{

    static InitialiseService() { }

    public static string Msg = "";
   
    private static XmlDocument _xmlPerfilSitio = null;
    private static XmlDocument _xmlPerfilLocal = null;
    private static string _Semilla = "";
    private static string _PI_Session = "";
    private static int _IdOrganizacion = 0;
    private static string _BgClientID = "";
    private static string _BgSecret = "";
    private static string _BgScope = "";
    private static string _BgIdDirectorio = "";



    public static XmlDocument xmlPerfilSitio
    {
        get
        {
            return _xmlPerfilSitio;
        }
    }

    public static string Semilla
    {
        get
        {
            return _Semilla;
        }
    }

    public static string PI_Session
    {
        get
        {
            return _PI_Session;
        }
        
        set 
        {
            _PI_Session = value;
        }
    }

    public static int IdOrganizacion
    {
        get
        {
            return _IdOrganizacion;
        }
        
        set 
        {
            _IdOrganizacion = value;
        }
    }

    public static string BgClientID
    {
        get
        {
            return _BgClientID;
        }

        set
        {
            _BgClientID = value;
        }
    }

    public static string BgSecret
    {
        get
        {
            return _BgSecret;
        }

        set
        {
            _BgSecret = value;
        }
    }

    public static string BgScope
    {
        get
        {
            return _BgScope;
        }

        set
        {
            _BgScope = value;
        }
    }

    public static string BgIdDirectorio
    {
        get
        {
            return _BgIdDirectorio;
        }

        set
        {
            _BgIdDirectorio = value;
        }
    }

    public static List<TransaccionUser> TransaccionesUsuario { get; set; }

    public static List<Menu> MenusSubMenuItemsGlobal { get; set; }

         

    //private bool CargaMenusSubMenuItems()
    //{
    //    bool resultado = false;
    //    List<Menu> TmpMenues;
    //    Menu TmpMenu;

    //    ApplicationDbContext db = new ApplicationDbContext();

    //    MenusSubMenuItemsGlobal =db.Menues.ToList();


    //    return resultado;

    //}
   
    public static string GetTokenUser()
    {

        var ValorTokenUser = string.Empty;

        var identity = System.Web.HttpContext.Current.User.Identity as ClaimsIdentity;

        
        try
        {
            ValorTokenUser = (from c in identity.Claims where c.Type == "tokenuser" select c.Value).Single();
        }
        catch (InvalidOperationException)
        {
            ValorTokenUser = "";
        }

        return ValorTokenUser;

    }
  
    public static void AppInitialize()
    {
        string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
        Logger.Logger loge = new Logger.Logger();
        try
        {
            string IdEmp = ((string)System.Configuration.ConfigurationManager.AppSettings["IdOrganizacion"]).Trim();
            string IdSuc = ((string)System.Configuration.ConfigurationManager.AppSettings["IdSucursal"]).Trim();
            string IdApl = ((string)System.Configuration.ConfigurationManager.AppSettings["IdAplicacionApl"]).Trim();
            string IdOrg = ((string)System.Configuration.ConfigurationManager.AppSettings["IdOrganizacionApl"]).Trim();
            string IdUsr = ((string)System.Configuration.ConfigurationManager.AppSettings["IdUsuarioApl"]).Trim();
            string IdUsrLocal = ((string)System.Configuration.ConfigurationManager.AppSettings["IdUsrlocal"]).Trim();
            string IdWSF = ((string)System.Configuration.ConfigurationManager.AppSettings["Url_WSFramework"]).Trim();
            string[] retorno;

            TransaccionesUsuario = new List<TransaccionUser>();
            MenusSubMenuItemsGlobal = new List<Menu>();

            _IdOrganizacion = Convert.ToInt32(IdOrg);

            if (!string.IsNullOrEmpty(IdEmp))
            {
                XmlDocument xmlResul = new XmlDocument();

                string Maquina = "";
                //para el caso que pase por proxi
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                {
                    Maquina = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                else
                {
                    Maquina= System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                 
                //clibSeguridadCR.clsClienteSeg objCliSeg = new clibSeguridadCR.clsClienteSeg();
                //_Semilla = objCliSeg.ConsSemilla();
                //objCliSeg.SetSemilla(_Semilla);
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();

                loge.FilePath = p_Log;
                loge.WriteMensaje("EjecucionGralDs Url antes: " + Proceso.Url);
                loge.Linea();

                Proceso.Url= System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();

                loge.FilePath = p_Log;
                loge.WriteMensaje("EjecucionGralDs Url despues: " + Proceso.Url);
                loge.Linea();

                retorno = Proceso.ConsSemilla(IdApl, IdUsrLocal, IdUsr, Maquina);
                xmlResul.LoadXml(retorno[0]);
                _xmlPerfilLocal = xmlResul;
                xmlResul.LoadXml(retorno[1]);
                _xmlPerfilSitio = xmlResul;
                _Semilla = retorno[2];
                //_xmlPerfilLocal.LoadXml(retorno[0]);
                //_xmlPerfilSitio.LoadXml(retorno[1]);

                // xmlResul = objCliSeg.LoginAplicacion(int.Parse(IdApl), IdUsrLocal, Maquina);
                //if (xmlResul.DocumentElement.GetAttribute("CodError").Equals("0"))
                // {
                //     _xmlPerfilLocal = xmlResul;
                // }


                // xmlResul = objCliSeg.LoginAplicacion(int.Parse(IdApl), IdUsr, Maquina);
                // if (xmlResul.DocumentElement.GetAttribute("CodError").Equals("0"))
                // {
                //     _xmlPerfilSitio = xmlResul;
                // }


                BgSecret = xmlPerfilSitio.DocumentElement.GetAttribute("client_secret").ToString();
                BgScope = xmlPerfilSitio.DocumentElement.GetAttribute("scope").ToString();
                BgClientID = xmlPerfilSitio.DocumentElement.GetAttribute("client_id").ToString();
                BgIdDirectorio = xmlPerfilSitio.DocumentElement.GetAttribute("idDirectorio").ToString();


                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();

                _PI_Session = "";
                  
                try
                {
                    _PI_Session = objSeg.getSesionIni(InitialiseService.xmlPerfilSitio);
                }
                catch (Exception)
                {
                      _PI_Session = "";
                }

                   
                //clibClienteSegNet.clsClienteSeg objSeg = new clibClienteSegNet.clsClienteSeg();
                //objSeg.URL_WS = IdWSF;
                //xmlResul.LoadXml(objSeg.LoginAplicacion(int.Parse(IdEmp), int.Parse(IdSuc), int.Parse(IdApl), int.Parse(IdOrg), IdUsr));
                //if (objSeg.codError == 0)
                //{
                //    _objSeguridad = objSeg;
                //}
                //_xmlPerfilSitio = xmlResul;
            }
            clibSeguridadCR.Seguridad.clsEncripta objEnc = new clibSeguridadCR.Seguridad.clsEncripta();
            /*
            clibLogger.clsLogger _objLogServicio = new clibLogger.clsLogger();
            CadenaConexion = "";
            _objLogServicio.Graba_Log_Info("INI >> SetLoginAplicacion");
            if (!SetLoginAplicacion())
            {
                _objLogServicio.Graba_Log_Error(" ERROR: " + Msg);
                return;
            }
            if (CadenaConexion != "")
                _objLogServicio.Graba_Log_Info("  >> [CadenaConexion] OK");
            else
                _objLogServicio.Graba_Log_Info("  >> No se devolvió valor para [CadenaConexion]");
            _objLogServicio.Graba_Log_Info("FIN >> SetLoginAplicacion");
            //GuardaLog("inicializado.");
             */
        }
        catch (Exception)
        {
        }
        try
        {
            //Douglas Bustos
            //cfg = new ECCDestinationConfig();
            //cfg.p_sApplicationServer = System.Configuration.ConfigurationManager.AppSettings["WApplicationServer"].ToString();
            //cfg.p_sClient = System.Configuration.ConfigurationManager.AppSettings["WClient"].ToString();
            //cfg.p_sSytem = System.Configuration.ConfigurationManager.AppSettings["WSytem"].ToString();
            //cfg.p_sSytemNumber = System.Configuration.ConfigurationManager.AppSettings["WSytemNumber"].ToString();
            //cfg.p_sLanguaje = System.Configuration.ConfigurationManager.AppSettings["WLanguaje"].ToString();
            //cfg.p_sPoolSize = System.Configuration.ConfigurationManager.AppSettings["WPoolSize"].ToString();
            //cfg.p_sUser = System.Configuration.ConfigurationManager.AppSettings["WUser"].ToString();
            //cfg.p_sPassword = System.Configuration.ConfigurationManager.AppSettings["WPassword"].ToString();
            //cfg.p_sLogonGroup = System.Configuration.ConfigurationManager.AppSettings["WGroupName"].ToString();
            //RfcDestinationManager.RegisterDestinationConfiguration(cfg);
            //dest = RfcDestinationManager.GetDestination("ROSADO");
        }
        catch (Exception)
        {
        }
    }

    public static void CargaPermisos(string UserName)
    {
        //Douglas Bustos
        //_CadenaConexion = ((string)System.Configuration.ConfigurationManager.AppSettings["CadenaConexion"]).Trim();
        try
        {
            string IdEmp = ((string)System.Configuration.ConfigurationManager.AppSettings["IdOrganizacion"]).Trim();
            string IdSuc = ((string)System.Configuration.ConfigurationManager.AppSettings["IdSucursal"]).Trim();
            string IdApl = ((string)System.Configuration.ConfigurationManager.AppSettings["IdAplicacionApl"]).Trim();
            string IdOrg = ((string)System.Configuration.ConfigurationManager.AppSettings["IdOrganizacionApl"]).Trim();
            string IdUsr = ((string)System.Configuration.ConfigurationManager.AppSettings["IdUsuarioApl"]).Trim();
            string IdWSF = ((string)System.Configuration.ConfigurationManager.AppSettings["Url_WSFramework"]).Trim();

            _IdOrganizacion = Convert.ToInt32(IdOrg);

            if (!string.IsNullOrEmpty(IdEmp))
            {
                XmlDocument xmlResul = new XmlDocument();

                string Maquina = "";
                try
                {
                    Maquina = (Dns.GetHostAddresses(Dns.GetHostName())[0]).ToString();
                }
                catch (Exception)
                {
                    Maquina = Dns.GetHostName();
                }

                clibSeguridadCR.clsClienteSeg objCliSeg = new clibSeguridadCR.clsClienteSeg();
                _Semilla = objCliSeg.ConsSemilla();
                objCliSeg.SetSemilla(_Semilla);
                xmlResul = objCliSeg.LoginAplicacion(int.Parse(IdApl), IdUsr, Maquina);
                if (xmlResul.DocumentElement.GetAttribute("CodError").Equals("0"))
                {
                    _xmlPerfilSitio = xmlResul;
                }


                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();

                _PI_Session = "";

                try
                {
                    _PI_Session = objSeg.getSesionIni(InitialiseService.xmlPerfilSitio);
                }
                catch (Exception)
                {
                    _PI_Session = "";
                }


                //clibClienteSegNet.clsClienteSeg objSeg = new clibClienteSegNet.clsClienteSeg();
                //objSeg.URL_WS = IdWSF;
                //xmlResul.LoadXml(objSeg.LoginAplicacion(int.Parse(IdEmp), int.Parse(IdSuc), int.Parse(IdApl), int.Parse(IdOrg), IdUsr));
                //if (objSeg.codError == 0)
                //{
                //    _objSeguridad = objSeg;
                //}
                //_xmlPerfilSitio = xmlResul;
            }
            clibSeguridadCR.Seguridad.clsEncripta objEnc = new clibSeguridadCR.Seguridad.clsEncripta();
            /*
            clibLogger.clsLogger _objLogServicio = new clibLogger.clsLogger();
            CadenaConexion = "";
            _objLogServicio.Graba_Log_Info("INI >> SetLoginAplicacion");
            if (!SetLoginAplicacion())
            {
                _objLogServicio.Graba_Log_Error(" ERROR: " + Msg);
                return;
            }
            if (CadenaConexion != "")
                _objLogServicio.Graba_Log_Info("  >> [CadenaConexion] OK");
            else
                _objLogServicio.Graba_Log_Info("  >> No se devolvió valor para [CadenaConexion]");
            _objLogServicio.Graba_Log_Info("FIN >> SetLoginAplicacion");
            //GuardaLog("inicializado.");
             */
        }
        catch (Exception)
        {
        }
        try
        {
            //Douglas Bustos
            //cfg = new ECCDestinationConfig();
            //cfg.p_sApplicationServer = System.Configuration.ConfigurationManager.AppSettings["WApplicationServer"].ToString();
            //cfg.p_sClient = System.Configuration.ConfigurationManager.AppSettings["WClient"].ToString();
            //cfg.p_sSytem = System.Configuration.ConfigurationManager.AppSettings["WSytem"].ToString();
            //cfg.p_sSytemNumber = System.Configuration.ConfigurationManager.AppSettings["WSytemNumber"].ToString();
            //cfg.p_sLanguaje = System.Configuration.ConfigurationManager.AppSettings["WLanguaje"].ToString();
            //cfg.p_sPoolSize = System.Configuration.ConfigurationManager.AppSettings["WPoolSize"].ToString();
            //cfg.p_sUser = System.Configuration.ConfigurationManager.AppSettings["WUser"].ToString();
            //cfg.p_sPassword = System.Configuration.ConfigurationManager.AppSettings["WPassword"].ToString();
            //cfg.p_sLogonGroup = System.Configuration.ConfigurationManager.AppSettings["WGroupName"].ToString();
            //RfcDestinationManager.RegisterDestinationConfiguration(cfg);
            //dest = RfcDestinationManager.GetDestination("ROSADO");
        }
        catch (Exception)
        {
        }
    }

    /*
    private static bool SetLoginAplicacion()
    {
        bool lv_Retorno = false;
        Msg = "";
        try
        {
            clibClienteSegNet.clsClienteSeg objSeg = new clibClienteSegNet.clsClienteSeg();
            System.Xml.XmlDocument xmlResul = new System.Xml.XmlDocument();
            objSeg.URL_WS = System.Configuration.ConfigurationManager.AppSettings["Url_WSFramework"];
            xmlResul.LoadXml(objSeg.LoginAplicacion(Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IdOrganizacion"]), Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IdSucursal"]), Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IdAplicacion"]), Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IdOrganizacion"]), System.Configuration.ConfigurationManager.AppSettings["IdUsuarioApl"]));
            if (objSeg.codError == 0)
            {
                //Max Pool Size=200

                XmlElement XmlConexion = (XmlElement)xmlResul.SelectSingleNode("/Registro/Servidores/Servidor[@Id='" + System.Configuration.ConfigurationManager.AppSettings["IdServidorEjecutaSQL"].ToString() + "']");
                if (XmlConexion.GetAttribute("Usuario").Trim().Equals(""))
                {
                    CadenaConexion = "Data Source=" + XmlConexion.GetAttribute("Servidor") + ";Initial Catalog=" + XmlConexion.GetAttribute("BaseDatos") + ";Integrated Security=true;Max Pool Size=" + XmlConexion.GetAttribute("MaxPool");
                }
                else
                {
                    CadenaConexion = "Data Source=" + XmlConexion.GetAttribute("Servidor") + ";Initial Catalog=" + XmlConexion.GetAttribute("BaseDatos") + ";User ID=" + XmlConexion.GetAttribute("Usuario") + ";Password=" + XmlConexion.GetAttribute("Clave") + ";Max Pool Size=" + XmlConexion.GetAttribute("MaxPool");
                }

                lv_Retorno = true;
            }
            else
                throw new Exception(objSeg.MsgError);
        }
        catch (Exception ex)
        {
            lv_Retorno = false;
            CadenaConexion = "";
            Msg = ex.Message;
        }

        return lv_Retorno;
    }*/

}

