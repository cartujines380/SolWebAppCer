﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.42000
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace WCFEnvioCorreo
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="WCFEnvioCorreo.ServEnvioClientSoap")>  _
    Public Interface ServEnvioClientSoap
        
        'CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento PI_UsrEnvia del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/EnviaCorreoDF", ReplyAction:="*")>  _
        Function EnviaCorreoDF(ByVal request As WCFEnvioCorreo.EnviaCorreoDFRequest) As WCFEnvioCorreo.EnviaCorreoDFResponse
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/EnviaCorreoDF", ReplyAction:="*")>  _
        Function EnviaCorreoDFAsync(ByVal request As WCFEnvioCorreo.EnviaCorreoDFRequest) As System.Threading.Tasks.Task(Of WCFEnvioCorreo.EnviaCorreoDFResponse)
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(IsWrapped:=false)>  _
    Partial Public Class EnviaCorreoDFRequest
        
        <System.ServiceModel.MessageBodyMemberAttribute(Name:="EnviaCorreoDF", [Namespace]:="http://tempuri.org/", Order:=0)>  _
        Public Body As WCFEnvioCorreo.EnviaCorreoDFRequestBody
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal Body As WCFEnvioCorreo.EnviaCorreoDFRequestBody)
            MyBase.New
            Me.Body = Body
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.Runtime.Serialization.DataContractAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public Class EnviaCorreoDFRequestBody
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=0)>  _
        Public PI_UsrEnvia As String
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=1)>  _
        Public PI_UsrDestino As String
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=2)>  _
        Public PI_UsrDestinoCC As String
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=3)>  _
        Public PI_UsrDestinoCCO As String
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=4)>  _
        Public PI_Asunto As String
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=5)>  _
        Public PI_Mensaje As String
        
        <System.Runtime.Serialization.DataMemberAttribute(Order:=6)>  _
        Public PI_MostrarLogo As Boolean
        
        <System.Runtime.Serialization.DataMemberAttribute(Order:=7)>  _
        Public PI_EsHTML As Boolean
        
        <System.Runtime.Serialization.DataMemberAttribute(Order:=8)>  _
        Public PI_TieneAdjuntos As Boolean
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=9)>  _
        Public PI_Adjunto() As Byte
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=10)>  _
        Public fileName As String
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=11)>  _
        Public PI_NombrePlantilla As String
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=12)>  _
        Public PI_Variables As String
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal PI_UsrEnvia As String, ByVal PI_UsrDestino As String, ByVal PI_UsrDestinoCC As String, ByVal PI_UsrDestinoCCO As String, ByVal PI_Asunto As String, ByVal PI_Mensaje As String, ByVal PI_MostrarLogo As Boolean, ByVal PI_EsHTML As Boolean, ByVal PI_TieneAdjuntos As Boolean, ByVal PI_Adjunto() As Byte, ByVal fileName As String, ByVal PI_NombrePlantilla As String, ByVal PI_Variables As String)
            MyBase.New
            Me.PI_UsrEnvia = PI_UsrEnvia
            Me.PI_UsrDestino = PI_UsrDestino
            Me.PI_UsrDestinoCC = PI_UsrDestinoCC
            Me.PI_UsrDestinoCCO = PI_UsrDestinoCCO
            Me.PI_Asunto = PI_Asunto
            Me.PI_Mensaje = PI_Mensaje
            Me.PI_MostrarLogo = PI_MostrarLogo
            Me.PI_EsHTML = PI_EsHTML
            Me.PI_TieneAdjuntos = PI_TieneAdjuntos
            Me.PI_Adjunto = PI_Adjunto
            Me.fileName = fileName
            Me.PI_NombrePlantilla = PI_NombrePlantilla
            Me.PI_Variables = PI_Variables
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(IsWrapped:=false)>  _
    Partial Public Class EnviaCorreoDFResponse
        
        <System.ServiceModel.MessageBodyMemberAttribute(Name:="EnviaCorreoDFResponse", [Namespace]:="http://tempuri.org/", Order:=0)>  _
        Public Body As WCFEnvioCorreo.EnviaCorreoDFResponseBody
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal Body As WCFEnvioCorreo.EnviaCorreoDFResponseBody)
            MyBase.New
            Me.Body = Body
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.Runtime.Serialization.DataContractAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public Class EnviaCorreoDFResponseBody
        
        <System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue:=false, Order:=0)>  _
        Public EnviaCorreoDFResult As String
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal EnviaCorreoDFResult As String)
            MyBase.New
            Me.EnviaCorreoDFResult = EnviaCorreoDFResult
        End Sub
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface ServEnvioClientSoapChannel
        Inherits WCFEnvioCorreo.ServEnvioClientSoap, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class ServEnvioClientSoapClient
        Inherits System.ServiceModel.ClientBase(Of WCFEnvioCorreo.ServEnvioClientSoap)
        Implements WCFEnvioCorreo.ServEnvioClientSoap
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Function WCFEnvioCorreo_ServEnvioClientSoap_EnviaCorreoDF(ByVal request As WCFEnvioCorreo.EnviaCorreoDFRequest) As WCFEnvioCorreo.EnviaCorreoDFResponse Implements WCFEnvioCorreo.ServEnvioClientSoap.EnviaCorreoDF
            Return MyBase.Channel.EnviaCorreoDF(request)
        End Function
        
        Public Function EnviaCorreoDF(ByVal PI_UsrEnvia As String, ByVal PI_UsrDestino As String, ByVal PI_UsrDestinoCC As String, ByVal PI_UsrDestinoCCO As String, ByVal PI_Asunto As String, ByVal PI_Mensaje As String, ByVal PI_MostrarLogo As Boolean, ByVal PI_EsHTML As Boolean, ByVal PI_TieneAdjuntos As Boolean, ByVal PI_Adjunto() As Byte, ByVal fileName As String, ByVal PI_NombrePlantilla As String, ByVal PI_Variables As String) As String
            Dim inValue As WCFEnvioCorreo.EnviaCorreoDFRequest = New WCFEnvioCorreo.EnviaCorreoDFRequest()
            inValue.Body = New WCFEnvioCorreo.EnviaCorreoDFRequestBody()
            inValue.Body.PI_UsrEnvia = PI_UsrEnvia
            inValue.Body.PI_UsrDestino = PI_UsrDestino
            inValue.Body.PI_UsrDestinoCC = PI_UsrDestinoCC
            inValue.Body.PI_UsrDestinoCCO = PI_UsrDestinoCCO
            inValue.Body.PI_Asunto = PI_Asunto
            inValue.Body.PI_Mensaje = PI_Mensaje
            inValue.Body.PI_MostrarLogo = PI_MostrarLogo
            inValue.Body.PI_EsHTML = PI_EsHTML
            inValue.Body.PI_TieneAdjuntos = PI_TieneAdjuntos
            inValue.Body.PI_Adjunto = PI_Adjunto
            inValue.Body.fileName = fileName
            inValue.Body.PI_NombrePlantilla = PI_NombrePlantilla
            inValue.Body.PI_Variables = PI_Variables
            Dim retVal As WCFEnvioCorreo.EnviaCorreoDFResponse = CType(Me,WCFEnvioCorreo.ServEnvioClientSoap).EnviaCorreoDF(inValue)
            Return retVal.Body.EnviaCorreoDFResult
        End Function
        
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Function WCFEnvioCorreo_ServEnvioClientSoap_EnviaCorreoDFAsync(ByVal request As WCFEnvioCorreo.EnviaCorreoDFRequest) As System.Threading.Tasks.Task(Of WCFEnvioCorreo.EnviaCorreoDFResponse) Implements WCFEnvioCorreo.ServEnvioClientSoap.EnviaCorreoDFAsync
            Return MyBase.Channel.EnviaCorreoDFAsync(request)
        End Function
        
        Public Function EnviaCorreoDFAsync(ByVal PI_UsrEnvia As String, ByVal PI_UsrDestino As String, ByVal PI_UsrDestinoCC As String, ByVal PI_UsrDestinoCCO As String, ByVal PI_Asunto As String, ByVal PI_Mensaje As String, ByVal PI_MostrarLogo As Boolean, ByVal PI_EsHTML As Boolean, ByVal PI_TieneAdjuntos As Boolean, ByVal PI_Adjunto() As Byte, ByVal fileName As String, ByVal PI_NombrePlantilla As String, ByVal PI_Variables As String) As System.Threading.Tasks.Task(Of WCFEnvioCorreo.EnviaCorreoDFResponse)
            Dim inValue As WCFEnvioCorreo.EnviaCorreoDFRequest = New WCFEnvioCorreo.EnviaCorreoDFRequest()
            inValue.Body = New WCFEnvioCorreo.EnviaCorreoDFRequestBody()
            inValue.Body.PI_UsrEnvia = PI_UsrEnvia
            inValue.Body.PI_UsrDestino = PI_UsrDestino
            inValue.Body.PI_UsrDestinoCC = PI_UsrDestinoCC
            inValue.Body.PI_UsrDestinoCCO = PI_UsrDestinoCCO
            inValue.Body.PI_Asunto = PI_Asunto
            inValue.Body.PI_Mensaje = PI_Mensaje
            inValue.Body.PI_MostrarLogo = PI_MostrarLogo
            inValue.Body.PI_EsHTML = PI_EsHTML
            inValue.Body.PI_TieneAdjuntos = PI_TieneAdjuntos
            inValue.Body.PI_Adjunto = PI_Adjunto
            inValue.Body.fileName = fileName
            inValue.Body.PI_NombrePlantilla = PI_NombrePlantilla
            inValue.Body.PI_Variables = PI_Variables
            Return CType(Me,WCFEnvioCorreo.ServEnvioClientSoap).EnviaCorreoDFAsync(inValue)
        End Function
    End Class
End Namespace