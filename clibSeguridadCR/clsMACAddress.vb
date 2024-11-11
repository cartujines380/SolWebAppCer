Imports System.Net
Imports System.Diagnostics
Imports System.Threading

Namespace Seguridad

    Public Class clsMACAddress
        'Public Function GetMACAddress(ByVal strName As String) As String
        '    Dim Net, Sh, Fso, Ts, MacAddress, Data

        '    Dim strIP As String = GetIPAddress(strName)
        '    Dim archivo As String = System.Web.HttpContext.Current.Server.MapPath(strIP)

        '    Net = CreateObject("wscript.network")
        '    Sh = CreateObject("wscript.shell")
        '    Sh.run("%comspec% /c nbtstat -A " & strIP & " > " & archivo & ".txt", 0, True)
        '    Sh = Nothing
        '    Fso = CreateObject("scripting.filesystemobject")
        '    Ts = Fso.opentextfile(archivo & ".txt")
        '    MacAddress = ""
        '    Do While Not Ts.AtEndOfStream
        '        Data = UCase(Trim(Ts.readline))
        '        If InStr(Data, "MAC ") Then 'MAC ADDRESS
        '            MacAddress = Trim(Split(Data, "=")(1))
        '            Exit Do
        '        End If
        '    Loop
        '    Ts.close()
        '    Ts = Nothing
        '    Fso.deletefile(archivo & ".txt")
        '    Fso = Nothing
        '    GetMACAddress = MacAddress
        'End Function
        'Public Function GetIPAddress(ByVal strName As String) As String
        '    Dim Net, Sh, Fso, Ts, IPAddress, Data
        '    Dim archivo As String = System.Web.HttpContext.Current.Server.MapPath(strName)
        '    Net = CreateObject("wscript.network")
        '    Sh = CreateObject("wscript.shell")
        '    Sh.run("%comspec% /c ipconfig " & " > " & archivo & ".txt", 0, True)
        '    Sh = Nothing
        '    Fso = CreateObject("scripting.filesystemobject")
        '    Ts = Fso.opentextfile(archivo & ".txt")
        '    IPAddress = ""
        '    Do While Not Ts.AtEndOfStream
        '        Data = UCase(Trim(Ts.readline))
        '        If InStr(Data, "IP") And InStr(Data, ": ") Then 'MAC ADDRESS
        '            IPAddress = Trim(Split(Data, ":")(1))
        '            Exit Do
        '        End If
        '    Loop
        '    Ts.close()
        '    Ts = Nothing
        '    Fso.deletefile(archivo & ".txt")
        '    Fso = Nothing
        '    GetIPAddress = IPAddress
        'End Function

        Public Function GetIPAddress(ByVal Equipo As String) As String
            Dim ip As String = ""
            Try
                Dim IpTmp As IPAddress
                IpTmp = Dns.GetHostEntry(Equipo).AddressList(0)
                ip = IpTmp.ToString()
            Catch
                ip = ""
            End Try
            Return ip
        End Function

        Public Function GetMACAddress(ByVal Equipo As String) As String
            Dim str As String = ""
            Dim Existe As Boolean = False
            Try
                Dim info1 As ProcessStartInfo = New ProcessStartInfo()
                Dim cmd As Process = New Process()
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                cmd.StartInfo.CreateNoWindow = True
                info1.FileName = "nbtstat"
                info1.RedirectStandardInput = False
                info1.RedirectStandardOutput = True
                info1.Arguments = "-a " + Equipo
                info1.UseShellExecute = False
                cmd = Process.Start(info1)
                While Not Existe
                    str = cmd.StandardOutput.ReadLine()
                    ' Direccion Mac Idioma Español Mac Address Idioma Ingles
                    If ((str.ToLower().IndexOf("mac =") > 0) OrElse (str.ToLower().IndexOf("address =") > 0)) Then
                        str = str.Split("=")(1).Trim() 'Obtener el Substring despues del '=' y eliminar Espacios en blanco
                        Existe = True
                    End If
                End While
                cmd.WaitForExit()
            Catch Ex As Exception
                str = ""
                'clsError.setMensaje("clsMACAddress", "GetMACAddress", "-1000", Ex.Message)
            End Try
            Return str
        End Function

    End Class
End Namespace