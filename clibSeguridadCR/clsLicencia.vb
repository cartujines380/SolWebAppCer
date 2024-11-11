Imports System.Xml

Namespace Seguridad
    Public Class clsLicencia

#Region "Propiedades Privadas"
        Private licencia As Boolean
        Private MsgError As String = ""
        Private MacAddress As String = ""
#End Region
#Region "Propiedades Publicas"
        Public ReadOnly Property plicencia() As Boolean
            Get
                Return licencia
            End Get
        End Property
        Public ReadOnly Property p_MsgError() As String
            Get
                Return MsgError
            End Get
        End Property
        Public ReadOnly Property p_MacAddress() As String
            Get
                Return MacAddress
            End Get
        End Property
        Public Property p_Semilla() As String
            Get
                Return "" 'System.Web.HttpContext.Current.Application("Semilla")
            End Get
            Set(ByVal value As String)
                'System.Web.HttpContext.Current.Application("Semilla") = value
            End Set
        End Property
        Public Property p_MaquinaWS() As String
            Get
                Return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList(0).ToString() 'System.Web.HttpContext.Current.Application("MaquinaWS")
            End Get
            Set(ByVal value As String)
                'System.Web.HttpContext.Current.Application("MaquinaWS") = value
            End Set
        End Property
#End Region
        
        Public Sub New()
            'Recupera la MAC de la maquina y verifica que este licenciado
            'Obtiene la licencia del Web.Config del Sitio
            licencia = False
            Try
                Dim strLicencia As String = System.Configuration.ConfigurationManager.AppSettings("Licencia")
                Dim Maquina As String = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList(0).ToString() 'System.Web.HttpContext.Current.Server.MachineName
                'Valida por fecha y MAC Address
                licencia = ValidaLicencia(strLicencia, Maquina)
            Catch ex As Exception
                MsgError = ex.Message
                licencia = False
            End Try
        End Sub
        '/ <summary>
        '/ ValidaLicencia es Boolean
        '/ </summary>
        '/ <param name="Licencia">licencia del Sitio</param>
        '/ <param name="Maquina">Nombre de la Maquina de Sitio</param>
        '/ <returns>the decrypted string</returns>
        Public Function ValidaLicencia(ByVal Licencia As String, _
                                        ByVal Maquina As String) As Boolean
            Dim retorno As Boolean = False
            'Licencia encriptada en formato mmddyyyyMACMaquina
            Dim objEncripta As clsEncripta = New clsEncripta
            Dim objMac As New clsMACAddress
            Dim xmlAuditoria As New XmlDocument

            Try
                xmlAuditoria.LoadXml("<Lic />")
                xmlAuditoria.DocumentElement.SetAttribute("Licencia", Licencia)
                xmlAuditoria.DocumentElement.SetAttribute("Maquina", Maquina)
                clsError.setMetodo(True, "clsSeguridad", "ValidaLicencia", xmlAuditoria.OuterXml)

                Dim xmlAud As New Xml.XmlDocument
                xmlAud.LoadXml("<Usuario />")
                xmlAud.DocumentElement.SetAttribute("Licencia", Licencia)
                xmlAud.DocumentElement.SetAttribute("Maquina", Maquina)
                'Registra inicio en trace	
                clsError.setMetodo(True, "clsSeguridad", "ValidaLicencia", xmlAud.OuterXml)
                'Se recupera la MAC de la Maquina
                'Dim MacSitio As String 
                MacAddress = objMac.GetMACAddress(Maquina)
                'Si no recupera la macadress se pone el nombre de la maquina dos
                If MacAddress.Equals("") Then
                    MacAddress = Maquina
                End If
                'Se desencrita la licencia para tener el formato mmddyyyyMACMaquina
                'La semilla viene encriptada, hay que desencritarla con el valor default
                Dim genOriginal As String = objEncripta.Decrypt(Licencia)

                'Se separa fecha y MAC
                Dim mm As Integer = CType(genOriginal.Substring(0, 2), Integer)
                Dim dd As Integer = CType(genOriginal.Substring(2, 2), Integer)
                Dim yyyy As Integer = CType(genOriginal.Substring(4, 4), Integer)
                Dim MAC As String = genOriginal.Substring(8)
                Dim fechaValida As New System.DateTime(yyyy, mm, dd)
                Dim fechaHoy As New System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                If MAC.Equals("00-00-00-00-00-00") Then
                    retorno = True
                    MsgError = "Servidor " + Maquina + " Licenciado Temporal por fuera de Red"
                    clsError.setMensaje("clsSeguridad", "ValidaLicencia", "0", MsgError)
                Else
                    If fechaValida.CompareTo(fechaHoy) > 0 AndAlso MacAddress.Equals(MAC) Then
                        retorno = True
                        MsgError = "Servidor " + Maquina + " Licenciado Ok"
                        clsError.setMensaje("clsSeguridad", "ValidaLicencia", "0", MsgError)
                        'Pregunta si falta menos de 2 meses para avisar
                        If fechaHoy.AddMonths(2).CompareTo(fechaValida) > 0 Then
                            MsgError = "Servidor " + Maquina + " La Licencia del Servidor del FrameWork de Seguridad esta proxima a expirar. " _
                                        + "Licencia solo Valido hasta el (dd-mm-yyyy) " + fechaValida.ToString("dd-MM-yyyy")
                            clsError.setMensaje("clsSeguridad", "ValidaLicencia", "-100", MsgError)
                        End If
                    Else
                        retorno = False
                        MsgError = "Licencia solo Valido hasta el " + fechaValida.ToString("MM-dd-yyyy") _
                                    + ": No muestra MAC de Licencia " _
                                    + ": Fecha Actual " + fechaHoy.ToString("MM-dd-yyyy") _
                                    + ": Maquina de Sitio" + Maquina _
                                    + ": MAC de Sitio " + MacAddress
                        clsError.setMensaje("clsSeguridad", "ValidaLicencia", "1000", MsgError)
                    End If
                End If
                'Asigna la MAC en variable de application
                p_MaquinaWS = Me.MacAddress
                'Recupera la semilla para el WS 
                If retorno Then
                    Dim objseg As New clsSeguridadCR
                    p_Semilla = objseg.getSemilla()
                End If
            Catch err As Exception
                clsError.setMensaje("clsSeguridad", "ValidaLicencia", "-102", err.Message)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "ValidaLicencia", IIf(retorno, "true", "false"))
            End Try
            Return retorno
        End Function

        'Public Sub New(ByVal PI_Ip As String, ByVal PI_Licencia As String)
        '    'Recupera la MAC de la maquina y verifica que este licenciado
        '    'Obtiene la licencia del Web.Config
        '    licencia = False
        '    Try
        '            Dim objEncripta As clsEncripta = New clsEncripta
        '            'Dim objMAC As clsMACAddress = New clsMACAddress
        '            'Dim strMAC As String = objMAC.GetMACAddress(PI_Licencia)

        '            ''licencia = objEncripta.Verifica(PI_Licencia, strMAC.Substring(0, 17))
        '        'If strName.Equals("SIGE") Then
        '        '    licencia = objEncripta.Verifica(strLicencia, "00-00-5F-AD-04-E3")
        '        'Else
        '        '    licencia = False
        '        'End If

        '        'Valida solo por fecha 
        '            licencia = objEncripta.VerificaFecha(PI_Licencia)
        '    Catch ex As Exception
        '        MsgError = ex.Message
        '        licencia = False
        '    End Try
        '    'licencia = True
        'End Sub
    End Class
End Namespace
