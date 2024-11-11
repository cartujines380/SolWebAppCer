'Imports System.Net.Mail
Imports System.Web.Mail
Public Class clsCorreo
#Region "Propiedades Privadas"
    Private pCodError As Integer
    Private pMsgError As String
#End Region
#Region "propiedades Publicas"
    Public Property CodError() As Integer
        Get
            Return pCodError
        End Get
        Set(ByVal value As Integer)
            pCodError = value
        End Set
    End Property
    Public Property MsgError() As String
        Get
            Return pMsgError
        End Get
        Set(ByVal value As String)
            pMsgError = value
        End Set
    End Property

#End Region

    Public Sub EnviaCorreo(ByVal PI_UsrEnvia As String, ByVal PI_UsrDestino As String, _
                        ByVal PI_Motivo As String, ByVal PI_Mensaje As String, _
                              Optional ByVal PI_Formato As String = "text")

        'reset variables de errores
        CodError = 0
        MsgError = ""

        'Create a new blank MailMessage
        Dim correo As MailMessage = New MailMessage
        Try
            'Set the properties of the MailMessage to the values on the form
            If PI_Formato.Equals("text") Then
                correo.BodyFormat = MailFormat.Text
            Else
                correo.BodyFormat = MailFormat.Html
            End If
            correo.From = PI_UsrEnvia
            correo.To = PI_UsrDestino
            correo.Subject = PI_Motivo
            correo.Body = PI_Mensaje

            'Set the SMTP server and send the email
            Dim servidor As String = System.Configuration.ConfigurationManager.AppSettings("ServidorCorreo")
            If Not servidor.Equals("") Then
                SmtpMail.SmtpServer = servidor
            End If
            SmtpMail.Send(correo)

        Catch ex As Exception
            CodError = -100
            MsgError = ex.Message
        End Try

    End Sub

    'Public Sub EnviaCorreo(ByVal PI_UsrEnvia As String, ByVal PI_UsrDestino As String, _
    '                        ByVal PI_Motivo As String, ByVal PI_Mensaje As String, _
    '                              Optional ByVal PI_Formato As String = "text")

    '    'reset variables de errores
    '    CodError = 0
    '    MsgError = ""

    '    'Create a new blank MailMessage
    '    Dim correo As MailMessage = New MailMessage

    '    Try
    '        correo.From = New System.Net.Mail.MailAddress(PI_UsrEnvia)
    '        correo.To.Add(PI_UsrDestino)
    '        correo.Subject = PI_Motivo
    '        correo.Body = PI_Mensaje
    '        If PI_Formato.Equals("text") Then
    '            correo.IsBodyHtml = False
    '        Else
    '            correo.IsBodyHtml = True
    '        End If
    '        correo.Priority = System.Net.Mail.MailPriority.Normal

    '        'Set the SMTP server and send the email
    '        Dim servidor As String = System.Configuration.ConfigurationManager.AppSettings("ServidorCorreo")
    '        Dim SmtpMail As New System.Net.Mail.SmtpClient
    '        If Not servidor.Equals("") Then
    '            SmtpMail.Host = servidor
    '        End If
    '        'SmtpMail.Credentials = New System.Net.NetworkCredential("usuario", "password")
    '        SmtpMail.Send(correo)

    '    Catch ex As Exception
    '        CodError = -100
    '        MsgError = ex.Message
    '    End Try

    'End Sub
    'Public Sub EnviaCorreo(ByVal PI_UsrEnvia As String, ByVal PI_UsrDestino As String, _
    '                    ByVal PI_UsrCopia As String, _
    '                    ByVal PI_Motivo As String, ByVal PI_Mensaje As String, _
    '                   ByVal PI_Formato As String)

    '    'reset variables de errores
    '    CodError = 0
    '    MsgError = ""

    '    'Create a new blank MailMessage
    '    Dim correo As MailMessage = New MailMessage
    '    Try
    '        correo.From = New System.Net.Mail.MailAddress(PI_UsrEnvia)
    '        correo.To.Add(PI_UsrDestino)
    '        correo.CC.Add(PI_UsrCopia)
    '        correo.Subject = PI_Motivo
    '        correo.Body = PI_Mensaje
    '        If PI_Formato.Equals("text") Then
    '            correo.IsBodyHtml = False
    '        Else
    '            correo.IsBodyHtml = True
    '        End If
    '        correo.Priority = System.Net.Mail.MailPriority.Normal

    '        'Set the SMTP server and send the email
    '        Dim servidor As String = System.Configuration.ConfigurationManager.AppSettings("ServidorCorreo")
    '        Dim SmtpMail As New System.Net.Mail.SmtpClient
    '        If Not servidor.Equals("") Then
    '            SmtpMail.Host = servidor
    '        End If
    '        'SmtpMail.Credentials = New System.Net.NetworkCredential("usuario", "password")
    '        SmtpMail.Send(correo)

    '    Catch ex As Exception
    '        CodError = -100
    '        MsgError = ex.Message
    '    End Try

    'End Sub
    'Public Sub EnviaCorreo(ByVal PI_UsrEnvia As String, ByVal PI_UsrDestino As String, _
    '                                ByVal PI_UsrCopia As String, _
    '                                ByVal PI_Motivo As String, ByVal PI_Mensaje As String, _
    '                                ByVal PI_Formato As String, _
    '                                ByVal PI_NombreArchivo As String)

    '    'reset variables de errores
    '    CodError = 0
    '    MsgError = ""

    '    'Create a new blank MailMessage
    '    Dim correo As MailMessage = New MailMessage
    '    Try
    '        correo.From = New System.Net.Mail.MailAddress(PI_UsrEnvia)
    '        correo.To.Add(PI_UsrDestino)
    '        correo.Subject = PI_Motivo
    '        correo.Body = PI_Mensaje
    '        If PI_Formato.Equals("text") Then
    '            correo.IsBodyHtml = False
    '        Else
    '            correo.IsBodyHtml = True
    '        End If
    '        correo.Priority = System.Net.Mail.MailPriority.Normal

    '        'Atach el archivo recibido
    '        Dim ArchAttach As Net.Mail.Attachment = New Attachment(PI_NombreArchivo)
    '        'attach the newly created email attachment
    '        correo.Attachments.Add(ArchAttach)

    '        'Set the SMTP server and send the email
    '        Dim servidor As String = System.Configuration.ConfigurationManager.AppSettings("ServidorCorreo")
    '        Dim SmtpMail As New System.Net.Mail.SmtpClient
    '        If Not servidor.Equals("") Then
    '            SmtpMail.Host = servidor
    '        End If
    '        'SmtpMail.Credentials = New System.Net.NetworkCredential("usuario", "password")
    '        SmtpMail.Send(correo)

    '    Catch ex As Exception
    '        CodError = -100
    '        MsgError = ex.Message
    '    End Try

    'End Sub


End Class

