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

namespace WCFEnvioCorreo.SoloBizWsSendMail {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="clsSendMailSoap", Namespace="http://solobiz.net.ec/")]
    public partial class clsSendMail : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback sbnFun_SendMailProveedoresOperationCompleted;
        
        private System.Threading.SendOrPostCallback sbnFun_SendMailProveedoresAttachOperationCompleted;
        
        private System.Threading.SendOrPostCallback sbnFun_SendMailProveedoresGlobalOperationCompleted;
        
        private System.Threading.SendOrPostCallback sbnFun_SendMailProveedoresAttachGlobalOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public clsSendMail() {
            this.Url = global::WCFEnvioCorreo.Properties.Settings.Default.WCFEnvioCorreo_SoloBizWsSendMail_clsSendMail;
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
        public event sbnFun_SendMailProveedoresCompletedEventHandler sbnFun_SendMailProveedoresCompleted;
        
        /// <remarks/>
        public event sbnFun_SendMailProveedoresAttachCompletedEventHandler sbnFun_SendMailProveedoresAttachCompleted;
        
        /// <remarks/>
        public event sbnFun_SendMailProveedoresGlobalCompletedEventHandler sbnFun_SendMailProveedoresGlobalCompleted;
        
        /// <remarks/>
        public event sbnFun_SendMailProveedoresAttachGlobalCompletedEventHandler sbnFun_SendMailProveedoresAttachGlobalCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://solobiz.net.ec/sbnFun_SendMailProveedores", RequestNamespace="http://solobiz.net.ec/", ResponseNamespace="http://solobiz.net.ec/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string sbnFun_SendMailProveedores(string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta) {
            object[] results = this.Invoke("sbnFun_SendMailProveedores", new object[] {
                        p_FromAddress,
                        p_ToAddress,
                        p_asunto,
                        p_body,
                        p_Copia,
                        p_CopiaOculta});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void sbnFun_SendMailProveedoresAsync(string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta) {
            this.sbnFun_SendMailProveedoresAsync(p_FromAddress, p_ToAddress, p_asunto, p_body, p_Copia, p_CopiaOculta, null);
        }
        
        /// <remarks/>
        public void sbnFun_SendMailProveedoresAsync(string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta, object userState) {
            if ((this.sbnFun_SendMailProveedoresOperationCompleted == null)) {
                this.sbnFun_SendMailProveedoresOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsbnFun_SendMailProveedoresOperationCompleted);
            }
            this.InvokeAsync("sbnFun_SendMailProveedores", new object[] {
                        p_FromAddress,
                        p_ToAddress,
                        p_asunto,
                        p_body,
                        p_Copia,
                        p_CopiaOculta}, this.sbnFun_SendMailProveedoresOperationCompleted, userState);
        }
        
        private void OnsbnFun_SendMailProveedoresOperationCompleted(object arg) {
            if ((this.sbnFun_SendMailProveedoresCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.sbnFun_SendMailProveedoresCompleted(this, new sbnFun_SendMailProveedoresCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://solobiz.net.ec/sbnFun_SendMailProveedoresAttach", RequestNamespace="http://solobiz.net.ec/", ResponseNamespace="http://solobiz.net.ec/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string sbnFun_SendMailProveedoresAttach(string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta, string p_filename1, string p_filename2, string p_filename3, string p_filename4, string p_filename5) {
            object[] results = this.Invoke("sbnFun_SendMailProveedoresAttach", new object[] {
                        p_FromAddress,
                        p_ToAddress,
                        p_asunto,
                        p_body,
                        p_Copia,
                        p_CopiaOculta,
                        p_filename1,
                        p_filename2,
                        p_filename3,
                        p_filename4,
                        p_filename5});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void sbnFun_SendMailProveedoresAttachAsync(string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta, string p_filename1, string p_filename2, string p_filename3, string p_filename4, string p_filename5) {
            this.sbnFun_SendMailProveedoresAttachAsync(p_FromAddress, p_ToAddress, p_asunto, p_body, p_Copia, p_CopiaOculta, p_filename1, p_filename2, p_filename3, p_filename4, p_filename5, null);
        }
        
        /// <remarks/>
        public void sbnFun_SendMailProveedoresAttachAsync(string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta, string p_filename1, string p_filename2, string p_filename3, string p_filename4, string p_filename5, object userState) {
            if ((this.sbnFun_SendMailProveedoresAttachOperationCompleted == null)) {
                this.sbnFun_SendMailProveedoresAttachOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsbnFun_SendMailProveedoresAttachOperationCompleted);
            }
            this.InvokeAsync("sbnFun_SendMailProveedoresAttach", new object[] {
                        p_FromAddress,
                        p_ToAddress,
                        p_asunto,
                        p_body,
                        p_Copia,
                        p_CopiaOculta,
                        p_filename1,
                        p_filename2,
                        p_filename3,
                        p_filename4,
                        p_filename5}, this.sbnFun_SendMailProveedoresAttachOperationCompleted, userState);
        }
        
        private void OnsbnFun_SendMailProveedoresAttachOperationCompleted(object arg) {
            if ((this.sbnFun_SendMailProveedoresAttachCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.sbnFun_SendMailProveedoresAttachCompleted(this, new sbnFun_SendMailProveedoresAttachCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://solobiz.net.ec/sbnFun_SendMailProveedoresGlobal", RequestNamespace="http://solobiz.net.ec/", ResponseNamespace="http://solobiz.net.ec/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string sbnFun_SendMailProveedoresGlobal(string p_Origen, string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta) {
            object[] results = this.Invoke("sbnFun_SendMailProveedoresGlobal", new object[] {
                        p_Origen,
                        p_FromAddress,
                        p_ToAddress,
                        p_asunto,
                        p_body,
                        p_Copia,
                        p_CopiaOculta});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void sbnFun_SendMailProveedoresGlobalAsync(string p_Origen, string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta) {
            this.sbnFun_SendMailProveedoresGlobalAsync(p_Origen, p_FromAddress, p_ToAddress, p_asunto, p_body, p_Copia, p_CopiaOculta, null);
        }
        
        /// <remarks/>
        public void sbnFun_SendMailProveedoresGlobalAsync(string p_Origen, string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta, object userState) {
            if ((this.sbnFun_SendMailProveedoresGlobalOperationCompleted == null)) {
                this.sbnFun_SendMailProveedoresGlobalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsbnFun_SendMailProveedoresGlobalOperationCompleted);
            }
            this.InvokeAsync("sbnFun_SendMailProveedoresGlobal", new object[] {
                        p_Origen,
                        p_FromAddress,
                        p_ToAddress,
                        p_asunto,
                        p_body,
                        p_Copia,
                        p_CopiaOculta}, this.sbnFun_SendMailProveedoresGlobalOperationCompleted, userState);
        }
        
        private void OnsbnFun_SendMailProveedoresGlobalOperationCompleted(object arg) {
            if ((this.sbnFun_SendMailProveedoresGlobalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.sbnFun_SendMailProveedoresGlobalCompleted(this, new sbnFun_SendMailProveedoresGlobalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://solobiz.net.ec/sbnFun_SendMailProveedoresAttachGlobal", RequestNamespace="http://solobiz.net.ec/", ResponseNamespace="http://solobiz.net.ec/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string sbnFun_SendMailProveedoresAttachGlobal(string p_Origen, string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta, string p_filename1, string p_filename2, string p_filename3, string p_filename4, string p_filename5) {
            object[] results = this.Invoke("sbnFun_SendMailProveedoresAttachGlobal", new object[] {
                        p_Origen,
                        p_FromAddress,
                        p_ToAddress,
                        p_asunto,
                        p_body,
                        p_Copia,
                        p_CopiaOculta,
                        p_filename1,
                        p_filename2,
                        p_filename3,
                        p_filename4,
                        p_filename5});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void sbnFun_SendMailProveedoresAttachGlobalAsync(string p_Origen, string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta, string p_filename1, string p_filename2, string p_filename3, string p_filename4, string p_filename5) {
            this.sbnFun_SendMailProveedoresAttachGlobalAsync(p_Origen, p_FromAddress, p_ToAddress, p_asunto, p_body, p_Copia, p_CopiaOculta, p_filename1, p_filename2, p_filename3, p_filename4, p_filename5, null);
        }
        
        /// <remarks/>
        public void sbnFun_SendMailProveedoresAttachGlobalAsync(string p_Origen, string p_FromAddress, string p_ToAddress, string p_asunto, string p_body, string p_Copia, string p_CopiaOculta, string p_filename1, string p_filename2, string p_filename3, string p_filename4, string p_filename5, object userState) {
            if ((this.sbnFun_SendMailProveedoresAttachGlobalOperationCompleted == null)) {
                this.sbnFun_SendMailProveedoresAttachGlobalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsbnFun_SendMailProveedoresAttachGlobalOperationCompleted);
            }
            this.InvokeAsync("sbnFun_SendMailProveedoresAttachGlobal", new object[] {
                        p_Origen,
                        p_FromAddress,
                        p_ToAddress,
                        p_asunto,
                        p_body,
                        p_Copia,
                        p_CopiaOculta,
                        p_filename1,
                        p_filename2,
                        p_filename3,
                        p_filename4,
                        p_filename5}, this.sbnFun_SendMailProveedoresAttachGlobalOperationCompleted, userState);
        }
        
        private void OnsbnFun_SendMailProveedoresAttachGlobalOperationCompleted(object arg) {
            if ((this.sbnFun_SendMailProveedoresAttachGlobalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.sbnFun_SendMailProveedoresAttachGlobalCompleted(this, new sbnFun_SendMailProveedoresAttachGlobalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void sbnFun_SendMailProveedoresCompletedEventHandler(object sender, sbnFun_SendMailProveedoresCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class sbnFun_SendMailProveedoresCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal sbnFun_SendMailProveedoresCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void sbnFun_SendMailProveedoresAttachCompletedEventHandler(object sender, sbnFun_SendMailProveedoresAttachCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class sbnFun_SendMailProveedoresAttachCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal sbnFun_SendMailProveedoresAttachCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void sbnFun_SendMailProveedoresGlobalCompletedEventHandler(object sender, sbnFun_SendMailProveedoresGlobalCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class sbnFun_SendMailProveedoresGlobalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal sbnFun_SendMailProveedoresGlobalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void sbnFun_SendMailProveedoresAttachGlobalCompletedEventHandler(object sender, sbnFun_SendMailProveedoresAttachGlobalCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class sbnFun_SendMailProveedoresAttachGlobalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal sbnFun_SendMailProveedoresAttachGlobalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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