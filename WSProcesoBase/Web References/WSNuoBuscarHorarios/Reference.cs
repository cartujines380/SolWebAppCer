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

namespace WSProcesoBase.WSNuoBuscarHorarios {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="WS_CITAS_SIPECOMSoap", Namespace="http://localhost/WS_CITAS_SIPECOM")]
    public partial class WS_CITAS_SIPECOM : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback NUO_citasOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WS_CITAS_SIPECOM() {
            this.Url = global::WSProcesoBase.Properties.Settings.Default.WSProcesoBase_WSNuoBuscarHorarios_WS_CITAS_SIPECOM;
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
        public event NUO_citasCompletedEventHandler NUO_citasCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/WS_CITAS_SIPECOM/NUO_citas", RequestNamespace="http://localhost/WS_CITAS_SIPECOM", ResponseNamespace="http://localhost/WS_CITAS_SIPECOM", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public CitaResponse NUO_citas(string BODEGA, string COD_PROVEEDOR, string FECHA) {
            object[] results = this.Invoke("NUO_citas", new object[] {
                        BODEGA,
                        COD_PROVEEDOR,
                        FECHA});
            return ((CitaResponse)(results[0]));
        }
        
        /// <remarks/>
        public void NUO_citasAsync(string BODEGA, string COD_PROVEEDOR, string FECHA) {
            this.NUO_citasAsync(BODEGA, COD_PROVEEDOR, FECHA, null);
        }
        
        /// <remarks/>
        public void NUO_citasAsync(string BODEGA, string COD_PROVEEDOR, string FECHA, object userState) {
            if ((this.NUO_citasOperationCompleted == null)) {
                this.NUO_citasOperationCompleted = new System.Threading.SendOrPostCallback(this.OnNUO_citasOperationCompleted);
            }
            this.InvokeAsync("NUO_citas", new object[] {
                        BODEGA,
                        COD_PROVEEDOR,
                        FECHA}, this.NUO_citasOperationCompleted, userState);
        }
        
        private void OnNUO_citasOperationCompleted(object arg) {
            if ((this.NUO_citasCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.NUO_citasCompleted(this, new NUO_citasCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://localhost/WS_CITAS_SIPECOM")]
    public partial class CitaResponse {
        
        private CitaLibreItem[] libreField;
        
        private CitaPendienteItem[] pendienteField;
        
        private CitaAprobadaItem[] aprobadaField;
        
        private CitaOcupadoItem[] ocupadaField;
        
        private string errorField;
        
        private int num_errorField;
        
        /// <remarks/>
        public CitaLibreItem[] libre {
            get {
                return this.libreField;
            }
            set {
                this.libreField = value;
            }
        }
        
        /// <remarks/>
        public CitaPendienteItem[] pendiente {
            get {
                return this.pendienteField;
            }
            set {
                this.pendienteField = value;
            }
        }
        
        /// <remarks/>
        public CitaAprobadaItem[] aprobada {
            get {
                return this.aprobadaField;
            }
            set {
                this.aprobadaField = value;
            }
        }
        
        /// <remarks/>
        public CitaOcupadoItem[] ocupada {
            get {
                return this.ocupadaField;
            }
            set {
                this.ocupadaField = value;
            }
        }
        
        /// <remarks/>
        public string error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
            }
        }
        
        /// <remarks/>
        public int num_error {
            get {
                return this.num_errorField;
            }
            set {
                this.num_errorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://localhost/WS_CITAS_SIPECOM")]
    public partial class CitaLibreItem {
        
        private string dIAField;
        
        private string hORA_INICIOField;
        
        private string hORA_FINField;
        
        private string fECHAField;
        
        /// <remarks/>
        public string DIA {
            get {
                return this.dIAField;
            }
            set {
                this.dIAField = value;
            }
        }
        
        /// <remarks/>
        public string HORA_INICIO {
            get {
                return this.hORA_INICIOField;
            }
            set {
                this.hORA_INICIOField = value;
            }
        }
        
        /// <remarks/>
        public string HORA_FIN {
            get {
                return this.hORA_FINField;
            }
            set {
                this.hORA_FINField = value;
            }
        }
        
        /// <remarks/>
        public string FECHA {
            get {
                return this.fECHAField;
            }
            set {
                this.fECHAField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://localhost/WS_CITAS_SIPECOM")]
    public partial class CitaOcupadoItem {
        
        private string dIAField;
        
        private string hORA_INICIOField;
        
        private string hORA_FINField;
        
        private string fECHAField;
        
        /// <remarks/>
        public string DIA {
            get {
                return this.dIAField;
            }
            set {
                this.dIAField = value;
            }
        }
        
        /// <remarks/>
        public string HORA_INICIO {
            get {
                return this.hORA_INICIOField;
            }
            set {
                this.hORA_INICIOField = value;
            }
        }
        
        /// <remarks/>
        public string HORA_FIN {
            get {
                return this.hORA_FINField;
            }
            set {
                this.hORA_FINField = value;
            }
        }
        
        /// <remarks/>
        public string FECHA {
            get {
                return this.fECHAField;
            }
            set {
                this.fECHAField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://localhost/WS_CITAS_SIPECOM")]
    public partial class CitaAprobadaItem {
        
        private string dIAField;
        
        private string hORA_INICIOField;
        
        private string hORA_FINField;
        
        private string fECHAField;
        
        /// <remarks/>
        public string DIA {
            get {
                return this.dIAField;
            }
            set {
                this.dIAField = value;
            }
        }
        
        /// <remarks/>
        public string HORA_INICIO {
            get {
                return this.hORA_INICIOField;
            }
            set {
                this.hORA_INICIOField = value;
            }
        }
        
        /// <remarks/>
        public string HORA_FIN {
            get {
                return this.hORA_FINField;
            }
            set {
                this.hORA_FINField = value;
            }
        }
        
        /// <remarks/>
        public string FECHA {
            get {
                return this.fECHAField;
            }
            set {
                this.fECHAField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://localhost/WS_CITAS_SIPECOM")]
    public partial class CitaPendienteItem {
        
        private string dIAField;
        
        private string hORA_INICIOField;
        
        private string hORA_FINField;
        
        private string fECHAField;
        
        /// <remarks/>
        public string DIA {
            get {
                return this.dIAField;
            }
            set {
                this.dIAField = value;
            }
        }
        
        /// <remarks/>
        public string HORA_INICIO {
            get {
                return this.hORA_INICIOField;
            }
            set {
                this.hORA_INICIOField = value;
            }
        }
        
        /// <remarks/>
        public string HORA_FIN {
            get {
                return this.hORA_FINField;
            }
            set {
                this.hORA_FINField = value;
            }
        }
        
        /// <remarks/>
        public string FECHA {
            get {
                return this.fECHAField;
            }
            set {
                this.fECHAField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void NUO_citasCompletedEventHandler(object sender, NUO_citasCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class NUO_citasCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal NUO_citasCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public CitaResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((CitaResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591