
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;
using PowerBIEmbedded_AppOwnsData.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

using System.Xml;

namespace PowerBIEmbedded_AppOwnsData.Services
{
    public class EmbedService : IEmbedService
    {
        private static readonly string AuthenticationType = ConfigurationManager.AppSettings["AuthenticationType"];
        private static readonly string AuthorityUrl = ConfigurationManager.AppSettings["authorityUrl"];
        private static readonly string ResourceUrl = ConfigurationManager.AppSettings["resourceUrl"];
        private static readonly string ApiUrl = ConfigurationManager.AppSettings["apiUrl"];
        private static string ApplicationId = "";
        private static string WorkspaceId = "";
        private static string ReportId = "";
        private static readonly NameValueCollection sectionConfig = ConfigurationManager.GetSection(AuthenticationType) as NameValueCollection;
        private static string Username = "";
        private static string Password = "";

        public EmbedConfig EmbedConfig
        {
            get { return m_embedConfig; }
        }

        public TileEmbedConfig TileEmbedConfig
        {
            get { return m_tileEmbedConfig; }
        }

        private EmbedConfig m_embedConfig;
        private TileEmbedConfig m_tileEmbedConfig;
        private TokenCredentials m_tokenCredentials;

        public EmbedService(string user, string password, string idAplicacion, string idWorkStation, string idReporte)
        {
            m_tokenCredentials = null;
            m_embedConfig = new EmbedConfig();
            m_tileEmbedConfig = new TileEmbedConfig();
            try
            {

                Username = user;
                Password = password;

            }
            catch (Exception)
            {

            }

            ApplicationId = idAplicacion;
            WorkspaceId = idWorkStation;
            ReportId = idReporte;

        }

        public bool EmbedReport(string username, string roles)
        {

            // Get token credentials for user
            var getCredentialsResult = GetTokenCredentials();
            if (!getCredentialsResult)
            {
                // The error message set in GetTokenCredentials
                return false;
            }

            try
            {
                // Create a Power BI Client object. It will be used to call Power BI APIs.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (var client = new PowerBIClient(new Uri(ApiUrl), m_tokenCredentials))
                {
                    // Get a list of reports.
                    var reports = client.Reports.GetReports(WorkspaceId);

                    // No reports retrieved for the given workspace.
                    if (reports.Value.Count() == 0)
                    {
                        m_embedConfig.ErrorMessage = "WRKS-FOUND";
                        return false;
                    }

                    Report report;
                    if (string.IsNullOrWhiteSpace(ReportId))
                    {
                        // Get the first report in the workspace.
                        m_embedConfig.ErrorMessage = "RPT-EMPTY";
                        return false;
                        //report = reports.Value.FirstOrDefault();
                    }
                    else
                    {
                        report = reports.Value.FirstOrDefault(r => r.Id.Equals(ReportId, StringComparison.InvariantCultureIgnoreCase));
                    }

                    if (report == null)
                    {
                        m_embedConfig.ErrorMessage = "REPT-FOUND";
                        return false;
                    }

                    var datasets = client.Datasets.GetDatasetByIdInGroup(WorkspaceId, report.DatasetId);
                    m_embedConfig.IsEffectiveIdentityRequired = datasets.IsEffectiveIdentityRequired;
                    m_embedConfig.IsEffectiveIdentityRolesRequired = datasets.IsEffectiveIdentityRolesRequired;
                    GenerateTokenRequest generateTokenRequestParameters;
                    // This is how you create embed token with effective identities
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        var rls = new EffectiveIdentity(username, new List<string> { report.DatasetId });
                        if (!string.IsNullOrWhiteSpace(roles))
                        {
                            var rolesList = new List<string>();
                            rolesList.AddRange(roles.Split(','));
                            rls.Roles = rolesList;
                        }
                        // Generate Embed Token with effective identities.
                        generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view", identities: new List<EffectiveIdentity> { rls });
                    }
                    else
                    {
                        // Generate Embed Token for reports without effective identities.
                        generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                    }

                    var tokenResponse = client.Reports.GenerateTokenInGroup(WorkspaceId, report.Id, generateTokenRequestParameters);

                    if (tokenResponse == null)
                    {
                        m_embedConfig.ErrorMessage = "ERR-TOKEN";
                        return false;
                    }

                    // Generate Embed Configuration.
                    m_embedConfig.EmbedToken = tokenResponse;
                    m_embedConfig.EmbedUrl = report.EmbedUrl;
                    m_embedConfig.Id = report.Id;
                }
            }
            catch (HttpOperationException exc)
            {
                //m_embedConfig.ErrorMessage = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
                m_embedConfig.ErrorMessage = string.Format("Estado: {0} ({1})\r\nRespuesta: {2}\r\nIdSolicitud: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
                return false;
            }

            return true;
        }



        /// <summary>
        /// Check if web.config embed parameters have valid values.
        /// </summary>
        /// <returns>Null if web.config parameters are valid, otherwise returns specific error string.</returns>
        private string GetWebConfigErrors()
        {
            // Application Id must have a value.
            if (string.IsNullOrWhiteSpace(ApplicationId))
            {
                //return "ApplicationId is empty. please register your application as Native app in https://dev.powerbi.com/apps and fill client Id in web.config.";
                return "IDAP-EMPTY";
            }

            // Application Id must be a Guid object.
            Guid result;
            if (!Guid.TryParse(ApplicationId, out result))
            {
                //return "ApplicationId must be a Guid object. please register your application as Native app in https://dev.powerbi.com/apps and fill application Id in web.config.";
                return "IDAP-NOREG";
            }

            // Workspace Id must have a value.
            if (string.IsNullOrWhiteSpace(WorkspaceId))
            {
                //return "WorkspaceId is empty. Please select a group you own and fill its Id in web.config";
                return "WKRS-EMPTY";
            }

            // Workspace Id must be a Guid object.
            if (!Guid.TryParse(WorkspaceId, out result))
            {
                //return "WorkspaceId must be a Guid object. Please select a workspace you own and fill its Id in web.config";
                return "WKRS-NOREG";
            }

            if (AuthenticationType.Equals("MasterUser"))
            {
                // Username must have a value.
                if (string.IsNullOrWhiteSpace(Username))
                {
                    //return "Username is empty. Please fill Power BI username in web.config";
                    return "USER-EMPTY";
                }

                // Password must have a value.
                if (string.IsNullOrWhiteSpace(Password))
                {
                    //return "Password is empty. Please fill password of Power BI username in web.config";
                    return "PASS-EMPTY";

                }
            }


            return null;
        }

        private AuthenticationResult DoAuthentication()
        {
            AuthenticationResult authenticationResult = null;
            if (AuthenticationType.Equals("MasterUser"))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var authenticationContext = new AuthenticationContext(AuthorityUrl);

                // Authentication using master user credentials
                var credential = new UserPasswordCredential(Username, Password);

                authenticationResult = authenticationContext.AcquireTokenAsync(ResourceUrl, ApplicationId, credential).Result;
            }


            return authenticationResult;
        }

        private bool GetTokenCredentials()
        {
            // var result = new EmbedConfig { Username = username, Roles = roles };
            var error = GetWebConfigErrors();
            if (error != null)
            {
                m_embedConfig.ErrorMessage = error;
                return false;
            }

            // Authenticate using created credentials
            AuthenticationResult authenticationResult = null;
            try
            {
                authenticationResult = DoAuthentication();
            }
            catch (AggregateException exc)
                {
                m_embedConfig.ErrorMessage = "ERR-AUTH";
                return false;
            }

            if (authenticationResult == null)
            {
                m_embedConfig.ErrorMessage = "ERR-AUTH";
                return false;
            }

            m_tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer");
            return true;
        }


    }
}