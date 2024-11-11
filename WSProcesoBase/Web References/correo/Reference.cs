﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace WSProcesoBase.correo {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ServEnvioClientSoap", Namespace="http://tempuri.org/")]
    public partial class ServEnvioClient : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback EnviarCorreoOperationCompleted;
        
        private System.Threading.SendOrPostCallback EnviarCorreoAdjuntoOperationCompleted;
        
        private System.Threading.SendOrPostCallback EnviaCorreoApiOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ServEnvioClient() {
            this.Url = global::WSProcesoBase.Properties.Settings.Default.WSProcesoBase_correo_ServEnvioClient;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event EnviarCorreoCompletedEventHandler EnviarCorreoCompleted;
        
        /// <remarks/>
        public event EnviarCorreoAdjuntoCompletedEventHandler EnviarCorreoAdjuntoCompleted;
        
        /// <remarks/>
        public event EnviaCorreoApiCompletedEventHandler EnviaCorreoApiCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EnviarCorreo", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string EnviarCorreo(string PI_UsrEnvia, string PI_UsrDestino, string PI_UsrDestinoCC, string PI_UsrDestinoCCO, string PI_Asunto, string PI_Mensaje, bool PI_MostrarLogo, bool PI_EsHTML, bool PI_TieneAdjuntos, string PI_Adjunto) {
            object[] results = this.Invoke("EnviarCorreo", new object[] {
                        PI_UsrEnvia,
                        PI_UsrDestino,
                        PI_UsrDestinoCC,
                        PI_UsrDestinoCCO,
                        PI_Asunto,
                        PI_Mensaje,
                        PI_MostrarLogo,
                        PI_EsHTML,
                        PI_TieneAdjuntos,
                        PI_Adjunto});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void EnviarCorreoAsync(string PI_UsrEnvia, string PI_UsrDestino, string PI_UsrDestinoCC, string PI_UsrDestinoCCO, string PI_Asunto, string PI_Mensaje, bool PI_MostrarLogo, bool PI_EsHTML, bool PI_TieneAdjuntos, string PI_Adjunto) {
            this.EnviarCorreoAsync(PI_UsrEnvia, PI_UsrDestino, PI_UsrDestinoCC, PI_UsrDestinoCCO, PI_Asunto, PI_Mensaje, PI_MostrarLogo, PI_EsHTML, PI_TieneAdjuntos, PI_Adjunto, null);
        }
        
        /// <remarks/>
        public void EnviarCorreoAsync(string PI_UsrEnvia, string PI_UsrDestino, string PI_UsrDestinoCC, string PI_UsrDestinoCCO, string PI_Asunto, string PI_Mensaje, bool PI_MostrarLogo, bool PI_EsHTML, bool PI_TieneAdjuntos, string PI_Adjunto, object userState) {
            if ((this.EnviarCorreoOperationCompleted == null)) {
                this.EnviarCorreoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnviarCorreoOperationCompleted);
            }
            this.InvokeAsync("EnviarCorreo", new object[] {
                        PI_UsrEnvia,
                        PI_UsrDestino,
                        PI_UsrDestinoCC,
                        PI_UsrDestinoCCO,
                        PI_Asunto,
                        PI_Mensaje,
                        PI_MostrarLogo,
                        PI_EsHTML,
                        PI_TieneAdjuntos,
                        PI_Adjunto}, this.EnviarCorreoOperationCompleted, userState);
        }
        
        private void OnEnviarCorreoOperationCompleted(object arg) {
            if ((this.EnviarCorreoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EnviarCorreoCompleted(this, new EnviarCorreoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EnviarCorreoAdjunto", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string EnviarCorreoAdjunto(string PI_UsrEnvia, string PI_UsrDestino, string PI_UsrDestinoCC, string PI_UsrDestinoCCO, string PI_Asunto, string PI_Mensaje, bool PI_MostrarLogo, bool PI_EsHTML, bool PI_TieneAdjuntos, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] PI_Adjunto, string fileName) {
            object[] results = this.Invoke("EnviarCorreoAdjunto", new object[] {
                        PI_UsrEnvia,
                        PI_UsrDestino,
                        PI_UsrDestinoCC,
                        PI_UsrDestinoCCO,
                        PI_Asunto,
                        PI_Mensaje,
                        PI_MostrarLogo,
                        PI_EsHTML,
                        PI_TieneAdjuntos,
                        PI_Adjunto,
                        fileName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void EnviarCorreoAdjuntoAsync(string PI_UsrEnvia, string PI_UsrDestino, string PI_UsrDestinoCC, string PI_UsrDestinoCCO, string PI_Asunto, string PI_Mensaje, bool PI_MostrarLogo, bool PI_EsHTML, bool PI_TieneAdjuntos, byte[] PI_Adjunto, string fileName) {
            this.EnviarCorreoAdjuntoAsync(PI_UsrEnvia, PI_UsrDestino, PI_UsrDestinoCC, PI_UsrDestinoCCO, PI_Asunto, PI_Mensaje, PI_MostrarLogo, PI_EsHTML, PI_TieneAdjuntos, PI_Adjunto, fileName, null);
        }
        
        /// <remarks/>
        public void EnviarCorreoAdjuntoAsync(string PI_UsrEnvia, string PI_UsrDestino, string PI_UsrDestinoCC, string PI_UsrDestinoCCO, string PI_Asunto, string PI_Mensaje, bool PI_MostrarLogo, bool PI_EsHTML, bool PI_TieneAdjuntos, byte[] PI_Adjunto, string fileName, object userState) {
            if ((this.EnviarCorreoAdjuntoOperationCompleted == null)) {
                this.EnviarCorreoAdjuntoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnviarCorreoAdjuntoOperationCompleted);
            }
            this.InvokeAsync("EnviarCorreoAdjunto", new object[] {
                        PI_UsrEnvia,
                        PI_UsrDestino,
                        PI_UsrDestinoCC,
                        PI_UsrDestinoCCO,
                        PI_Asunto,
                        PI_Mensaje,
                        PI_MostrarLogo,
                        PI_EsHTML,
                        PI_TieneAdjuntos,
                        PI_Adjunto,
                        fileName}, this.EnviarCorreoAdjuntoOperationCompleted, userState);
        }
        
        private void OnEnviarCorreoAdjuntoOperationCompleted(object arg) {
            if ((this.EnviarCorreoAdjuntoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EnviarCorreoAdjuntoCompleted(this, new EnviarCorreoAdjuntoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EnviaCorreoApi", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string EnviaCorreoApi(string PI_UsrEnvia, string PI_UsrDestino, string PI_UsrDestinoCC, string PI_UsrDestinoCCO, string PI_Asunto, string PI_Mensaje, bool PI_MostrarLogo, bool PI_EsHTML, bool PI_TieneAdjuntos, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] PI_Adjunto, string fileName, string PI_NombrePlantilla, string PI_Variables) {
            object[] results = this.Invoke("EnviaCorreoApi", new object[] {
                        PI_UsrEnvia,
                        PI_UsrDestino,
                        PI_UsrDestinoCC,
                        PI_UsrDestinoCCO,
                        PI_Asunto,
                        PI_Mensaje,
                        PI_MostrarLogo,
                        PI_EsHTML,
                        PI_TieneAdjuntos,
                        PI_Adjunto,
                        fileName,
                        PI_NombrePlantilla,
                        PI_Variables});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void EnviaCorreoApiAsync(string PI_UsrEnvia, string PI_UsrDestino, string PI_UsrDestinoCC, string PI_UsrDestinoCCO, string PI_Asunto, string PI_Mensaje, bool PI_MostrarLogo, bool PI_EsHTML, bool PI_TieneAdjuntos, byte[] PI_Adjunto, string fileName, string PI_NombrePlantilla, string PI_Variables) {
            this.EnviaCorreoApiAsync(PI_UsrEnvia, PI_UsrDestino, PI_UsrDestinoCC, PI_UsrDestinoCCO, PI_Asunto, PI_Mensaje, PI_MostrarLogo, PI_EsHTML, PI_TieneAdjuntos, PI_Adjunto, fileName, PI_NombrePlantilla, PI_Variables, null);
        }
        
        /// <remarks/>
        public void EnviaCorreoApiAsync(string PI_UsrEnvia, string PI_UsrDestino, string PI_UsrDestinoCC, string PI_UsrDestinoCCO, string PI_Asunto, string PI_Mensaje, bool PI_MostrarLogo, bool PI_EsHTML, bool PI_TieneAdjuntos, byte[] PI_Adjunto, string fileName, string PI_NombrePlantilla, string PI_Variables, object userState) {
            if ((this.EnviaCorreoApiOperationCompleted == null)) {
                this.EnviaCorreoApiOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnviaCorreoApiOperationCompleted);
            }
            this.InvokeAsync("EnviaCorreoApi", new object[] {
                        PI_UsrEnvia,
                        PI_UsrDestino,
                        PI_UsrDestinoCC,
                        PI_UsrDestinoCCO,
                        PI_Asunto,
                        PI_Mensaje,
                        PI_MostrarLogo,
                        PI_EsHTML,
                        PI_TieneAdjuntos,
                        PI_Adjunto,
                        fileName,
                        PI_NombrePlantilla,
                        PI_Variables}, this.EnviaCorreoApiOperationCompleted, userState);
        }
        
        private void OnEnviaCorreoApiOperationCompleted(object arg) {
            if ((this.EnviaCorreoApiCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EnviaCorreoApiCompleted(this, new EnviaCorreoApiCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void EnviarCorreoCompletedEventHandler(object sender, EnviarCorreoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EnviarCorreoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal EnviarCorreoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void EnviarCorreoAdjuntoCompletedEventHandler(object sender, EnviarCorreoAdjuntoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EnviarCorreoAdjuntoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal EnviarCorreoAdjuntoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void EnviaCorreoApiCompletedEventHandler(object sender, EnviaCorreoApiCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EnviaCorreoApiCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal EnviaCorreoApiCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591