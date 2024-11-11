using SAP.Middleware.Connector;
using System.Configuration;

namespace WSProcesoBase.Model
{
    public class AppConfig
    {
        private static string _sftpServerPassword;
        private static string _sftpServerUserName;
        private static string _sftpServerIp;
        private static string _workBAPI;
        private static string _sftpServerPort;
        private static string _sftpPath;
        private static string _archivoPDF;

        private static ECCDestinationConfig _cfg;
        private static RfcDestination _dest;

        private static string _enviaCorreoProveedor;
        private static string _archivoPDFPreliminar;

        public static string archivoPDFPreliminar
        {
            get
            {
                if (string.IsNullOrEmpty(_archivoPDFPreliminar))
                    _archivoPDFPreliminar = ConfigurationManager.AppSettings["archivoPDFPreliminar"];
                return _archivoPDFPreliminar;
            }
        }


        public static string enviaCorreoProveedor
        {
            get
            {
                if (string.IsNullOrEmpty(_enviaCorreoProveedor))
                    _enviaCorreoProveedor = ConfigurationManager.AppSettings["enviaCorreoProveedor"];
                return _enviaCorreoProveedor;
            }
        }

        public static string archivoPDF
        {
            get
            {
                if (string.IsNullOrEmpty(_archivoPDF))
                    _archivoPDF = ConfigurationManager.AppSettings["archivoPDF"];
                return _archivoPDF;
            }
        }


        public static string SftpServerUserName
        {
            get
            {
                if (string.IsNullOrEmpty(_sftpServerUserName))
                    _sftpServerUserName = ConfigurationManager.AppSettings["SFTPServerUserName"];
                return _sftpServerUserName;
            }
        }
        public static string SftpServerPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_sftpServerPassword))
                    _sftpServerPassword = ConfigurationManager.AppSettings["SFTPServerPassword"];
                return _sftpServerPassword;
            }
        }

        public static string SftpServerIp
        {
            get
            {
                if (string.IsNullOrEmpty(_sftpServerIp))
                    _sftpServerIp = ConfigurationManager.AppSettings["SFTPServerIP"];
                return _sftpServerIp;
            }
        }

        public static string WorkBAPI
        {
            get
            {
                if (string.IsNullOrEmpty(_workBAPI))
                    _workBAPI = ConfigurationManager.AppSettings["workBAPI"];
                return _workBAPI;
            }
        }

        public static string SftpServerPort
        {
            get
            {
                if (string.IsNullOrEmpty(_sftpServerPort))
                    _sftpServerPort = ConfigurationManager.AppSettings["SFTPServerPort"];
                return _sftpServerPort;
            }
        }

        public static string SftpPath
        {
            get
            {
                if (string.IsNullOrEmpty(_sftpPath))
                    _sftpPath = ConfigurationManager.AppSettings["SFTPPath"];
                return _sftpPath;
            }
        }

        private static ECCDestinationConfig cfg
        {
            get
            {
                if (_cfg == null)
                {
                    try
                    {
                        string p_sApplicationServer = ConfigurationManager.AppSettings["WApplicationServer"].ToString();
                        string p_sClient = ConfigurationManager.AppSettings["WClient"].ToString();
                        string p_sLanguaje = ConfigurationManager.AppSettings["WLanguaje"].ToString();
                        string p_sPassword = ConfigurationManager.AppSettings["WPassword"].ToString();
                        string p_sPoolSize = ConfigurationManager.AppSettings["WPoolSize"].ToString();
                        string p_sSytem = ConfigurationManager.AppSettings["WSytem"].ToString();
                        string p_sSytemNumber = ConfigurationManager.AppSettings["WSytemNumber"].ToString();
                        string p_sUser = ConfigurationManager.AppSettings["WUser"].ToString();
                        string p_sLogonGroup = System.Configuration.ConfigurationManager.AppSettings["WGroupName"].ToString();


                        _cfg = new ECCDestinationConfig();
                        _cfg.p_sApplicationServer = p_sApplicationServer; ;
                        _cfg.p_sClient = p_sClient;
                        _cfg.p_sSytem = p_sSytem;
                        _cfg.p_sSytemNumber = p_sSytemNumber;
                        _cfg.p_sLanguaje = p_sLanguaje;
                        _cfg.p_sPoolSize = p_sPoolSize;
                        _cfg.p_sUser = p_sUser;
                        _cfg.p_sPassword = p_sPassword;
                        _cfg.p_sLogonGroup = p_sLogonGroup;


                    }
                    catch (System.Exception)
                    {


                    }
                }

                return _cfg;
            }
        }

        public static RfcDestination dest
        {
            get
            {
                if (_dest == null)
                {
                    try
                    {
                        RfcDestinationManager.RegisterDestinationConfiguration(cfg);
                        _dest = RfcDestinationManager.GetDestination("ROSADO");
                    }
                    catch (System.Exception)
                    {

                    }



                }

                return _dest;

            }
        }
    }
}