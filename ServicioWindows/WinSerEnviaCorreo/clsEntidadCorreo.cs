using System;

namespace FE.ServicioSendCorreo
{
    static class clsEntidadCorreo
    {
        public static clibLogger.clsLogger p_Log = new clibLogger.clsLogger();

        
        private static int _intervaloServicioCorreo;        
        private static Int32 _maxRegistrosCorreo;
        private static Int32 _maxItemsConcurrenciaCorreo;
               
        private static string _Notificacion_ServidorSmtp;
        private static string _Notificacion_PuertoSmtp;
        private static string _Notificacion_UsuarioSmtp;
        private static string _Notificacion_ClaveSmtp;
        private static string _Notificacion_EnviaSmtp;
        private static string _Notificacion_SenderSmtp;
        private static Boolean _Notificacion_AplicarSSL;

        private static string _Notificacion_NoAutorizados;
        private static string _Notificacion_ExpiraCertificado;

        static public string Notificacion_ExpiraCertificado
        {
            get
            {
                return _Notificacion_ExpiraCertificado;
            }
            set
            {
                _Notificacion_ExpiraCertificado = value;
            }
        }

        static public string Notificacion_NoAutorizados
        {
            get
            {
                return _Notificacion_NoAutorizados;
            }
            set
            {
                _Notificacion_NoAutorizados = value;
            }
        }

        static public string Notificacion_SenderSmtp
        {
            get
            {
                return _Notificacion_SenderSmtp;
            }
            set
            {
                _Notificacion_SenderSmtp = value;
            }
        }
 
 
        static public string Notificacion_EnviaSmtp
        {
            get
            {
                return _Notificacion_EnviaSmtp;
            }
            set
            {
                _Notificacion_EnviaSmtp = value;
            }
        }

        static public string Notificacion_ClaveSmtp
        {
            get
            {
                return _Notificacion_ClaveSmtp;
            }
            set
            {
                _Notificacion_ClaveSmtp = value;
            }
        }

        static public string Notificacion_UsuarioSmtp
        {
            get
            {
                return _Notificacion_UsuarioSmtp;
            }
            set
            {
                _Notificacion_UsuarioSmtp = value;
            }
        }

        static public string Notificacion_PuertoSmtp
        {
            get
            {
                return _Notificacion_PuertoSmtp;
            }
            set
            {
                _Notificacion_PuertoSmtp = value;
            }
        }

        static public string Notificacion_ServidorSmtp
        {
            get
            {
                return _Notificacion_ServidorSmtp;
            }
            set
            {
                _Notificacion_ServidorSmtp = value;
            }
        }


        static public Boolean Notificacion_AplicarSSL
        {
            get
            {
                return _Notificacion_AplicarSSL;
            }
            set
            {
                _Notificacion_AplicarSSL = value;
            }
        }
       
             
        static public Int32 maxRegistrosCorreo
        {
            get
            {
                return _maxRegistrosCorreo;
            }
            set
            {
                _maxRegistrosCorreo = value;
            }
        }

        static public Int32 intervaloServicioCorreo
        {
            get
            {
                return _intervaloServicioCorreo;
            }
            set
            {
                _intervaloServicioCorreo = value;
            }
        }

        static public Int32 maxItemsConcurrenciaCorreo
        {
            get
            {
                return _maxItemsConcurrenciaCorreo;
            }
            set
            {
                _maxItemsConcurrenciaCorreo = value;
            }
        }
    }
}
