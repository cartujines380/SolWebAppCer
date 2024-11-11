Public Class clsFTP

    Public lv_EsPasivo As Boolean = False
    Public lv_IP As String = ""
    Public lv_Usuario As String = ""
    Public lv_Clave As String = ""
    Public lv_Puerto As Integer
    Dim ftp As New Chilkat.Ftp2()
    Dim sftp As New Chilkat.SFtp()

    Public Function ObtenerArchivo(PI_Directorio As String, PI_Archivo As String, _
                                   Optional ByRef PO_ExisteError As Boolean = False, Optional ByRef PO_MsgError As String = "") As Byte()
        Dim lv_Byte As Byte()
        Try

            Dim success As Boolean
            '  Any string unlocks the component for the 1st 30-days.
            success = ftp.UnlockComponent("SIPECMFTP_CYnV2H8MoCnd")
            If (success <> True) Then
                Throw New Exception(ftp.LastErrorText)
            End If


            ftp.Hostname = lv_IP
            ftp.Username = lv_Usuario
            ftp.Password = lv_Clave
            ftp.Port = lv_Puerto

            success = ftp.Connect()
            If (success <> True) Then
                Throw New Exception(ftp.LastErrorText)
            End If
            ftp.Passive = lv_EsPasivo

            success = ftp.ChangeRemoteDir(PI_Directorio)
            If (success <> True) Then
                Throw New Exception(ftp.LastErrorText)
            End If

            lv_Byte = ftp.GetRemoteFileBinaryData(PI_Archivo)
            If (lv_Byte Is Nothing) Then
                Throw New Exception(ftp.LastErrorText)
            End If

            ftp.Disconnect()

        Catch ex As Exception
            PO_ExisteError = True
            PO_MsgError = ex.Message
        End Try
        Return lv_Byte
    End Function

    Public Function CopiarArchivo(PI_Directorio As String, PI_Archivo As String, PI_File As Byte(), _
                                         Optional ByRef PO_ExisteError As Boolean = False, Optional ByRef PO_MsgError As String = "") As Boolean
        Dim lv_Copio As Boolean = False
        Try
            Dim success As Boolean
            '  Any string unlocks the component for the 1st 30-days.
            success = ftp.UnlockComponent("SIPECMFTP_CYnV2H8MoCnd")
            If (success <> True) Then
                Throw New Exception(ftp.LastErrorText)
            End If


            ftp.Hostname = lv_IP
            ftp.Username = lv_Usuario
            ftp.Password = lv_Clave
            ftp.Port = lv_Puerto

            success = ftp.Connect()
            If (success <> True) Then
                Throw New Exception(ftp.LastErrorText)
            End If
            ftp.Passive = lv_EsPasivo

            success = ftp.ChangeRemoteDir(PI_Directorio)
            If (success <> True) Then
                Throw New Exception(ftp.LastErrorText)
            End If

            success = ftp.PutFileFromBinaryData(PI_Archivo, PI_File)
            If (success <> True) Then
                Throw New Exception(ftp.LastErrorText)
            End If

            ftp.Disconnect()

            lv_Copio = True
        Catch ex As Exception
            lv_Copio = False
            PO_ExisteError = True
            PO_MsgError = ex.Message
        End Try
        Return lv_Copio
    End Function

    Public lv_DirectorioLocal As String = ""
    Public Function ObtenerArchivo_Sftp(PI_Directorio As String, PI_Archivo As String, _
                                  PO_MsgError As String) As Byte()
        Dim lv_Byte As Byte()
        Try

            Dim success As Boolean
            Dim tamano As Integer
            '  Any string unlocks the component for the 1st 30-days.
            success = sftp.UnlockComponent("SIPECMSSH_Ta0nAxk82InZ")
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            success = sftp.Connect(lv_IP, lv_Puerto)
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            success = sftp.AuthenticatePw(lv_Usuario, lv_Clave)
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If
            success = sftp.InitializeSftp()
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            Dim handle As String = sftp.OpenFile(PI_Directorio + PI_Archivo, "readOnly", "openExisting")
            If (sftp.LastMethodSuccess <> True) Then
                Throw New Exception("0" + sftp.LastErrorText)
            End If

            'success = sftp.DownloadFile(handle, lv_DirectorioLocal + PI_Archivo)
            tamano = sftp.GetFileSize32(handle, False, True)
            lv_Byte = sftp.ReadFileBytes32(handle, 0, tamano)
            If (success <> True) Then
                Throw New Exception("1:" + sftp.LastErrorText)
            End If

            success = sftp.CloseHandle(handle)
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            sftp.Disconnect()

            'lv_Byte = System.IO.File.ReadAllBytes(lv_DirectorioLocal + PI_Archivo)
            '   System.IO.File.WriteAllBytes(lv_DirectorioLocal + PI_Archivo, lv_Byte)
            Try
                My.Computer.FileSystem.DeleteFile(lv_DirectorioLocal + PI_Archivo) 'Opción corta
            Catch ex As Exception

            End Try
        Catch ex As Exception
            PO_MsgError = ex.Message
        End Try
        Return lv_Byte
    End Function
    Public Function crearCarpeta(ruta As String, carpeta As String) As Boolean
        Dim retorno As Boolean = False

        Try
            Dim success As Boolean
            '  Any string unlocks the component for the 1st 30-days.
            success = sftp.UnlockComponent("SIPECMSSH_Ta0nAxk82InZ")
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            success = sftp.Connect(lv_IP, lv_Puerto)
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If
            success = sftp.AuthenticatePw(lv_Usuario, lv_Clave)
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If
            success = sftp.InitializeSftp()
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If
            success = sftp.CreateDir(ruta + carpeta)
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If


            sftp.Disconnect()
            retorno = True
        Catch ex As Exception
            retorno = False
        End Try
        Return retorno
    End Function
    Public Function CopiarArchivo_Sftp(PI_Directorio As String, PI_Archivo As String, PI_File As Byte(), _
                                          PO_MsgError As String) As Boolean
        Dim lv_Copio As Boolean = False
        Try
            Dim success As Boolean
            '  Any string unlocks the component for the 1st 30-days.
            success = sftp.UnlockComponent("SIPECMSSH_Ta0nAxk82InZ")
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            success = sftp.Connect(lv_IP, lv_Puerto)
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            success = sftp.AuthenticatePw(lv_Usuario, lv_Clave)
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If
            success = sftp.InitializeSftp()
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            Dim handle As String = sftp.OpenFile(PI_Directorio + PI_Archivo, "writeOnly", "createTruncate")
            If (sftp.LastMethodSuccess <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            success = sftp.WriteFileBytes(handle, PI_File)
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            success = sftp.CloseHandle(handle)
            If (success <> True) Then
                Throw New Exception(sftp.LastErrorText)
            End If

            sftp.Disconnect()

            lv_Copio = True
        Catch ex As Exception
            lv_Copio = False
            PO_MsgError = ex.Message
        End Try
        Return lv_Copio
    End Function


End Class
