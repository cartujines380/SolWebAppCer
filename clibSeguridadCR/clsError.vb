Imports System.IO
Imports System.Xml
'Imports log4net

Public Class clsError

    Delegate Sub GrabaLogCallBack(ByVal Datos As String)
    Private ThreadGrabaLog As Threading.Thread = Nothing

    'Shared ReadOnly logger As ILog = LogManager.GetLogger("clibSeguridad")

#Region "Metodos de Clases"
    Public Shared Sub setMetodo(ByVal PI_Opcion As Boolean, _
                                ByVal PI_Clase As String, _
                                ByVal PI_Metodo As String, _
                                Optional ByVal PI_Info As String = "")
        Dim datos As New Text.StringBuilder
        Try
            'PI_Opcion = true Inicia, false = termina
            'Si esta activo el trace se escribe en la salida
            'datos.Append(Now.ToString("dd-MM-yyyy hh:mm:ss "))
            datos.Append("Clase: ").Append(PI_Clase).Append(" Metodo: ").Append(PI_Metodo)
            If (PI_Opcion) Then
                datos.Append(" INICIA ").Append(PI_Info)
            Else
                datos.Append(" FIN ").Append(PI_Info)
                Graba_Log(datos.ToString()) 'Solo sI esta activo la auditoria
            End If
            'If (System.Web.HttpContext.Current.Trace.IsEnabled) Then
            '    System.Web.HttpContext.Current.Trace.Write(datos.ToString())
            'End If

            Graba_Log(datos.ToString)

            'Dim d As New GrabaLogCallBack(AddressOf Graba_Log)
            'Threading.Invoke(d, New Object() {datos.ToString})



        Catch ex As Exception
            GrabaEventLog("clsError", "setMetodo", "-1000", ex.Message)
        End Try
    End Sub
    Public Shared Function setMensajeDS(ByVal PI_Clase As String, _
                                ByVal PI_Metodo As String, _
                                ByVal PI_CodError As Long, _
                                ByVal PI_MsgError As String) As DataSet

        Dim msg As String = setMensaje(PI_Clase, PI_Metodo, PI_CodError, PI_MsgError)
        Return GeneraDs(PI_CodError, msg)
    End Function
    Public Shared Function setMensajeXml(ByVal PI_Clase As String, _
                                    ByVal PI_Metodo As String, _
                                    ByVal PI_CodError As Long, _
                                    ByVal PI_MsgError As String) As xmlDocument
        Dim xmlRet As New XmlDocument
        xmlRet.LoadXml("<Registro />")
        xmlRet.DocumentElement.SetAttribute("CodError", PI_CodError)
        xmlRet.DocumentElement.SetAttribute("MsgError", setMensaje(PI_Clase, PI_Metodo, PI_CodError, PI_MsgError))
        Return xmlRet
    End Function

    Public Shared Function setMensaje(ByVal PI_Clase As String, _
                            ByVal PI_Metodo As String, _
                            ByVal PI_CodError As Long, _
                            ByVal PI_MsgError As String) As String
        Dim datos As New Text.StringBuilder
        Dim Retorno As String = ""
        Try
            'datos.Append(Now.ToString("dd-MM-yyyy hh:mm:ss"))
            datos.Append(" Clase: ").Append(PI_Clase).Append(" Metodo: ").Append(PI_Metodo)
            datos.Append(" Codigo Error: ").Append(PI_CodError.ToString())
            datos.Append(" Msg Error: ").Append(PI_MsgError)
            'Si esta activo el trace se escribe en la salida
            'If (System.Web.HttpContext.Current.Trace.IsEnabled) Then
            '    System.Web.HttpContext.Current.Trace.Warn(datos.ToString())
            'End If
            Graba_Log_Error(datos.ToString())

            'Mensajes personalizados por el dueño del producto,son creados en DB Master
            ' e invocados desde los SP
            If (PI_CodError < 0 AndAlso PI_CodError <> 0) Then 'Error severo o de Base de Datos
                Retorno = "Por favor reportar al error a Sistemas enviando un print de pantalla del mismo."
                Try
                    Dim UsrEnvia As String = System.Configuration.ConfigurationManager.AppSettings("UsuarioBACorreo")
                    Dim UsrRecibe As String = System.Configuration.ConfigurationManager.AppSettings("UsuarioCorreoError")
                    If Not UsrEnvia.Equals("") Then
                        Dim objCorreo As New clsCorreo
                        objCorreo.EnviaCorreo(UsrEnvia, UsrRecibe, "Error FrameWork Seguridad", Now.ToString("dd-MM-yyyy hh:mm:ss") & datos.ToString())
                        If objCorreo.CodError <> 0 Then
                            GrabaEventLog("clsError", "setMensaje", -1000, objCorreo.MsgError)
                        End If
                    End If

                Catch ex As Exception
                    GrabaEventLog("clsError", "setMensaje", -1000, ex.Message)
                End Try

                GrabaEventLog(PI_Clase, PI_Metodo, PI_CodError, PI_MsgError)
            Else
                If PI_MsgError.ToUpper().IndexOf("ORG=") > 0 Then
                    Retorno = PI_MsgError.Substring(0, PI_MsgError.ToUpper().IndexOf("ORG="))
                Else
                    Retorno = PI_MsgError
                End If

            End If

        Catch ex As Exception
            'Si esta activo el trace se escribe en la salida
            If (System.Web.HttpContext.Current.Trace.IsEnabled) Then
                System.Web.HttpContext.Current.Trace.Warn("Clase clsError Metodo setMensaje")
                System.Web.HttpContext.Current.Trace.Warn("Msg Error: " + ex.Message)
            End If
            GrabaEventLog("clsError", "setMensaje", "-100", ex.Message)
            Retorno = "Por favor reportar al error a Sistemas enviando un print de pantalla del mismo."
        End Try
        Return Retorno
    End Function

    Public Shared Sub GrabaEventLog(ByVal PI_Clase As String, _
                                    ByVal PI_Metodo As String, _
                                    ByVal PI_CodError As Long, _
                                    ByVal PI_MsgError As String)
        Try
            Dim txtMsg As New System.Text.StringBuilder()
            txtMsg.Append("Error en la aplicacion del FrameWrok Seguridad")
            txtMsg.Append("Clase: ").Append(PI_Clase)
            txtMsg.Append("\nMetodo: ").Append(PI_Metodo)
            txtMsg.Append("\nCodigo de Error:" + PI_CodError.ToString())
            txtMsg.Append("\nMensaje de Error:" + PI_MsgError)
            EventLog.WriteEntry("FRAMEWORK SEGURIDAD", txtMsg.ToString(), _
                        IIf(PI_CodError = 0, EventLogEntryType.Information, EventLogEntryType.Error))
        Catch ex As Exception

        End Try
    End Sub
 
    Public Shared Sub Graba_Log(ByVal Datos As String)
        Try
            'log4net.Config.DOMConfigurator.Configure()
            'logger.Info(Datos)
        Catch ex As Exception
            GrabaEventLog("clsError", "Graba_log", -1000, ex.Message)
        End Try
    End Sub

    Public Shared Sub Graba_Log_Error(ByVal Datos As String)
        Try
            'log4net.Config.DOMConfigurator.Configure()
            'logger.Error(Datos)
        Catch ex As Exception
            GrabaEventLog("clsError", "Graba_log_Error", -1000, ex.Message)
        End Try
    End Sub

    Public Shared Function GeneraDs(ByVal CodError As Integer, ByVal MsgError As String) As DataSet
        Dim dt As DataTable = New DataTable("TblEstado")
        Dim ds As New DataSet
        dt.Columns.Add(New DataColumn("CodError", GetType(Integer)))
        dt.Columns.Add(New DataColumn("MsgError", GetType(String)))
        Dim dr As DataRow
        dr = dt.NewRow
        dr.BeginEdit()
        dr("CodError") = CodError
        dr("MSgError") = MsgError
        dr.EndEdit()
        dt.Rows.Add(dr)
        ds.Tables.Add(dt)
        Return ds
    End Function
#End Region
End Class

