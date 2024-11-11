using System;
using System.Text;
using System.Collections.Generic;
using SAP.Middleware.Connector;

namespace WSProcesoBase.Model
{
    class ECCDestinationConfig : IDestinationConfiguration
    {
        private String _sApplicationServer;
        private String _sSytem;
        private String _sSytemNumber;
        private String _sClient;
        private String _sLanguaje;
        private String _sUser;
        private String _sPassword;
        private String _sPoolSize;
        private String _sLogonGroup;

        public String p_sApplicationServer
        {
            get
            {
                return _sApplicationServer;
            }
            set
            {
                _sApplicationServer = value;
            }
        }
        public String p_sSytem
        {
            get
            {
                return _sSytem;
            }
            set
            {
                _sSytem = value;
            }
        }
        public String p_sSytemNumber
        {
            get
            {
                return _sSytemNumber;
            }
            set
            {
                _sSytemNumber = value;
            }
        }
        public String p_sClient
        {
            get
            {
                return _sClient;
            }
            set
            {
                _sClient = value;
            }
        }
        public String p_sLanguaje
        {
            get
            {
                return _sLanguaje;
            }
            set
            {
                _sLanguaje = value;
            }
        }
        public String p_sUser
        {
            get
            {
                return _sUser;
            }
            set
            {
                _sUser = value;
            }
        }
        public String p_sPassword
        {
            get
            {
                return _sPassword;
            }
            set
            {
                _sPassword = value;
            }
        }
        public String p_sPoolSize
        {
            get
            {
                return _sPoolSize;
            }
            set
            {
                _sPoolSize = value;
            }
        }
        public String p_sLogonGroup
        {
            get
            {
                return _sLogonGroup;
            }
            set
            {
                _sLogonGroup = value;
            }
        }

        public bool ChangeEventsSupported()
        {
            return false;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters parms = new RfcConfigParameters();

            if (string.IsNullOrEmpty(_sLogonGroup))
            {
                parms.Add(RfcConfigParameters.AppServerHost, _sApplicationServer);
            }
            else
            {
                parms.Add(RfcConfigParameters.MessageServerHost, _sApplicationServer);
                parms.Add(RfcConfigParameters.LogonGroup, _sLogonGroup);
            }
            parms.Add(RfcConfigParameters.SystemNumber, _sSytemNumber);
            parms.Add(RfcConfigParameters.SystemID, _sSytem);
            parms.Add(RfcConfigParameters.User, _sUser);
            parms.Add(RfcConfigParameters.Password, _sPassword);
            parms.Add(RfcConfigParameters.Client, _sClient);
            parms.Add(RfcConfigParameters.Language, _sLanguaje);
            parms.Add(RfcConfigParameters.PoolSize, _sPoolSize);

            return parms;
        }
    }
}
