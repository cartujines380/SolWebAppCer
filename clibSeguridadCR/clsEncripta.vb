Imports System
Imports System.Text
'Imports System.Security.Cryptography
Imports com.CryptoTools

Namespace Seguridad
    Public Class clsEncripta

#Region "Utilitarios privados"
        Private Function ConvertHexString(ByVal DatoHex() As Byte) As String
            Dim DatoConvert As New StringBuilder()
            Try
                Dim i As Integer
                For i = 0 To DatoHex.Length - 1
                    DatoConvert.Append(DatoHex(i).ToString("X2"))
                Next
            Catch ex As Exception
            End Try
            Return DatoConvert.ToString()
        End Function
        Private Function ConvertByte(ByVal HexString As String) As Byte()
            Dim buff() As Byte
            Try
                Dim j As Integer = 0
                Dim i As Integer = 0
                ReDim buff((HexString.Length / 2) - 1)
                For i = 0 To HexString.Length - 1 Step 2
                    buff(j) = Convert.ToByte(HexString.Substring(i, 2), 16)
                    j = j + 1
                Next
            Catch ex As Exception
                buff = Nothing
            End Try
            Return buff
        End Function
#End Region
#Region "Metodos Principales"
        Private Function Encripta(ByVal Dato As String, ByVal Semilla As String) As String
            Dim resul As String = ""
            Dim NewKey() As Byte
            Try

                Dim objEnc As New CryptoTripleDES
                If Semilla.Length > 16 Then
                    NewKey = ASCIIEncoding.ASCII.GetBytes(Semilla.Substring(0, 16))
                ElseIf Semilla.Length < 16 Then
                    NewKey = ASCIIEncoding.ASCII.GetBytes(Semilla.PadRight(16, "*"))
                Else
                    NewKey = ASCIIEncoding.ASCII.GetBytes(Semilla)
                End If
                objEnc.Key = NewKey
                objEnc.EncryptText(Dato)
                resul = ConvertHexString(objEnc.Result)
            Catch ex As Exception

            End Try
            Return resul
        End Function
        Private Function Desencripta(ByVal DatoEncriptado As String, ByVal Semilla As String) As String
            Dim resul As String = ""
            Dim NewKey() As Byte
            Try
                If Semilla.Length > 16 Then
                    NewKey = ASCIIEncoding.ASCII.GetBytes(Semilla.Substring(0, 16))
                ElseIf Semilla.Length < 16 Then
                    NewKey = ASCIIEncoding.ASCII.GetBytes(Semilla.PadRight(16, "*"))
                Else
                    NewKey = ASCIIEncoding.ASCII.GetBytes(Semilla)
                End If
                Dim objEnc As New CryptoTripleDES
                objEnc.Key = NewKey
                resul = objEnc.DecryptText(Me.ConvertByte(DatoEncriptado))
            Catch ex As Exception

            End Try
            Return resul
        End Function
#End Region

#Region "Metodos Publicos"
        '/<summary>Decrypts a string</summary>
        '/<param name="encrypted">the encrypted string</param>
        '/<param name="key">the key used in encryption</param>
        '/<returns>the decrypted string</returns>
        Public Function Encrypt(ByVal original As String, ByVal key As String) As String
            Dim clave As String = ""
            Try
                'If key.Length = 0 Then 'se toma la semilla
                '    key = Decrypt(System.Web.HttpContext.Current.Application("Semilla"))
                'End If
                clave = Encripta(original, key)
            Catch ex As Exception
                clave = "ERROR"
            End Try
            Return clave
        End Function
        Public Function Encrypt(ByVal original As String) As String
            Dim clave As String = ""
            Try
                'Longitud de la key es minima 16 caracteres
                Dim key1 As New Text.StringBuilder
                Dim key2 As New Text.StringBuilder
                key1.Append((2 * 1).ToString())
                key1.Append((2 * 2).ToString())
                key1.Append(key1)
                key2.Append((2 * 2).ToString())
                key2.Append((2 * 1).ToString())
                key2.Append(key2)
                key1.Append("-").Append(key2.ToString())
                key1.Append("-").Append(key1.ToString())
                clave = Encripta(original, key1.ToString())
            Catch ex As Exception
                clave = "ERROR"
            End Try
            Return clave
        End Function
        Public Function Decrypt(ByVal encrypted As String, ByVal key As String) As String
            Dim clave As String = ""
            Try
                'Si la key > 0, se toma esto como semilla
                'If key.Length = 0 Then
                '    'la key se toma la semilla de apllicacion, la cual es encriptada
                '    key = Decrypt(System.Web.HttpContext.Current.Application("Semilla"))
                'End If
                clave = Desencripta(encrypted, key)
            Catch ex As Exception
                clave = "ERROR"
            End Try
            Return clave
        End Function
        Friend Function Decrypt(ByVal encryptedHex As String) As String
            Dim clave As String = ""
            Try
                Dim key1 As New Text.StringBuilder
                Dim key2 As New Text.StringBuilder
                key1.Append((2 * 1).ToString())
                key1.Append((2 * 2).ToString())
                key1.Append(key1)
                key2.Append((2 * 2).ToString())
                key2.Append((2 * 1).ToString())
                key2.Append(key2)
                key1.Append("-").Append(key2.ToString())
                key1.Append("-").Append(key1.ToString())
                clave = Desencripta(encryptedHex, key1.ToString())
            Catch ex As Exception
                clave = "ERROR"
            End Try
            Return clave
        End Function
        Public Function Verifica(ByVal encriptado As String, ByVal original As String) As Boolean
            Dim genOriginal As String = Decrypt(encriptado)
            If original.Equals(genOriginal) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function Verifica(ByVal encriptado As String, ByVal original As String, ByVal key As String) As Boolean
            Dim genOriginal As String = Decrypt(encriptado, key)
            If original.Equals(genOriginal) Then
                Return True
            Else
                Return False
            End If
        End Function
#End Region
#Region "Metodos Migrados"
        'Private Function Desencripta(ByVal encryptedHex As String, ByVal key As String) As String
        '    Dim keyhash(), buff(encryptedHex.Length / 2) As Byte

        '    'encrypted string en formato Hexa
        '    'buff = New Byte(encryptedHex.Length / 2)
        '    Dim j As Integer = 0
        '    Dim i As Integer = 0
        '    For i = 0 To encryptedHex.Length - 1 Step 2
        '        buff(j) = Convert.ToByte(encryptedHex.Substring(i, 2), 16)
        '        j = j + 1
        '    Next
        '    'string encrypted = Convert.ToBase64String(sb)

        '    Dim DES As TripleDESCryptoServiceProvider

        '    Dim hashmd5 As MD5CryptoServiceProvider

        '    Dim decrypted As String

        '    hashmd5 = New MD5CryptoServiceProvider()

        '    keyhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key))

        '    hashmd5 = Nothing

        '    DES = New TripleDESCryptoServiceProvider()

        '    DES.Key = keyhash

        '    DES.Mode = CipherMode.ECB

        '    'buff = Convert.FromBase64String(encrypted)


        '    decrypted = ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length - 1))

        '    Return decrypted

        'End Function

        'Public Function Encripta(ByVal original As String, ByVal key As String) As String
        '    Dim DES As TripleDESCryptoServiceProvider
        '    Dim hashmd5 As MD5CryptoServiceProvider
        '    Dim keyhash(), buff() As Byte
        '    Dim encrypted As StringBuilder = New StringBuilder()
        '    hashmd5 = New MD5CryptoServiceProvider()
        '    keyhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key))
        '    hashmd5 = Nothing
        '    DES = New TripleDESCryptoServiceProvider()
        '    DES.Key = keyhash
        '    DES.Mode = CipherMode.ECB
        '    buff = ASCIIEncoding.ASCII.GetBytes(original)
        '    Dim result() As Byte = DES.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length)
        '    ' Build the final string by converting each byte
        '    ' into hex and appending it to a StringBuilder
        '    Dim i As Integer
        '    For i = 0 To result.Length - 1
        '        encrypted.Append(result(i).ToString("X2"))
        '    Next
        '    Return encrypted.ToString()
        'End Function
        'Private Function GeneraHash(ByVal plainText As String, _
        '                        ByVal hashAlgorithm As String, _
        '                        ByVal plainSalt As String) As String


        '    ' Convert plain text into a byte array.
        '    Dim plainTextBytes() As Byte = Encoding.UTF8.GetBytes(plainText)

        '    ' Convert plain salt into a byte array.
        '    Dim saltBytes() As Byte = Encoding.UTF8.GetBytes(plainSalt)

        '    ' Allocate array, which will hold plain text and salt.
        '    Dim plainTextWithSaltBytes(plainTextBytes.Length + saltBytes.Length) As Byte

        '    ' Copy plain text bytes into resulting array.
        '    Dim i As Integer
        '    For i = 0 To plainTextBytes.Length - 1
        '        plainTextWithSaltBytes(i) = plainTextBytes(i)
        '    Next

        '    ' Append salt bytes to the resulting array.
        '    For i = 0 To saltBytes.Length - 1
        '        plainTextWithSaltBytes(plainTextBytes.Length + i) = saltBytes(i)
        '    Next
        '    ' Because we support multiple hashing algorithms, we must define
        '    ' hash object as a common (abstract) base class. We will specify the
        '    ' actual hashing algorithm class later during object creation.
        '    Dim hash As HashAlgorithm

        '    ' Make sure hashing algorithm name is specified.
        '    If IsNothing(hashAlgorithm) Then
        '        hashAlgorithm = ""
        '    End If

        '    ' Initialize appropriate hashing algorithm class.
        '    Select Case hashAlgorithm.ToUpper()
        '        Case "SHA1"
        '            hash = New SHA1Managed()
        '        Case "SHA256"
        '            hash = New SHA256Managed()
        '        Case "SHA384"
        '            hash = New SHA384Managed()
        '        Case "SHA512"
        '            hash = New SHA512Managed()
        '        Case Else
        '            hash = New MD5CryptoServiceProvider()
        '    End Select

        '    ' Compute hash value of our plain text with appended salt.
        '    Dim hashBytes() As Byte = hash.ComputeHash(plainTextWithSaltBytes)


        '    ' Create array which will hold hash and original salt bytes.
        '    Dim hashWithSaltBytes(hashBytes.Length + saltBytes.Length) As Byte

        '    ' Copy hash bytes into resulting array.
        '    For i = 0 To hashBytes.Length - 1
        '        hashWithSaltBytes(i) = hashBytes(i)
        '    Next

        '    ' Append salt bytes to the result.
        '    For i = 0 To saltBytes.Length - 1
        '        hashWithSaltBytes(hashBytes.Length + i) = saltBytes(i)
        '    Next

        '    ' Convert result into a base64-encoded string.
        '    Dim hashString As String = Convert.ToBase64String(hashWithSaltBytes)

        '    ' Return the result.
        '    Return hashString
        'End Function

        '/ <SUMMARY>
        '/ Compares a hash of the specified plain text value to a given hash
        '/ value. Plain text is hashed with the same salt value as the original
        '/ hash.
        '/ </SUMMARY>
        '/ <PARAM name="plainText">
        '/ Plain text to be verified against the specified hash. The function
        '/ does not check whether this parameter is null.
        '/ </PARAM>
        '/ <PARAM name="hashAlgorithm">
        '/ Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
        '/ "SHA256", "SHA384", and "SHA512" (if any other value is specified,
        '/ MD5 hashing algorithm will be used). This value is case-insensitive.
        '/ </PARAM>
        '/ <PARAM name="hashValue">
        '/ Base64-encoded hash value produced by ComputeHash function. This value
        '/ includes the original salt appended to it.
        '/ </PARAM>
        '/ <RETURNS>
        '/ If computed hash mathes the specified hash the function the return
        '/ value is true otherwise, the function returns false.
        '/ </RETURNS>
        'Private Function VerificaHash(ByVal plainText As String, _
        '                        ByVal hashAlgorithm As String, _
        '                        ByVal hashString As String) As Boolean

        '    ' Convert base64-encoded hash value into a byte array.
        '    Dim hashWithSaltBytes() As Byte = Convert.FromBase64String(hashString)

        '    ' We must know size of hash (without salt).
        '    Dim hashSizeInBits As Integer, hashSizeInBytes As Integer

        '    ' Make sure that hashing algorithm name is specified.
        '    If IsNothing(hashAlgorithm) Then
        '        hashAlgorithm = ""
        '    End If
        '    ' Size of hash is based on the specified algorithm.
        '    Select Case (hashAlgorithm.ToUpper())
        '        Case "SHA1"
        '            hashSizeInBits = 160
        '        Case "SHA256"
        '            hashSizeInBits = 256
        '        Case "SHA384"
        '            hashSizeInBits = 384
        '        Case "SHA512"
        '            hashSizeInBits = 512
        '        Case Else
        '            hashSizeInBits = 128
        '    End Select

        '    ' Convert size of hash from bits to bytes.
        '    hashSizeInBytes = hashSizeInBits / 8

        '    ' Make sure that the specified hash value is long enough.
        '    If hashWithSaltBytes.Length < hashSizeInBytes Then
        '        Return False
        '    End If

        '    ' Allocate array to hold original salt bytes retrieved from hash.
        '    Dim saltBytes(hashWithSaltBytes.Length - hashSizeInBytes) As Byte

        '    ' Copy salt from the end of the hash to the new array.
        '    Dim salt As String = ""
        '    Dim i As Integer
        '    For i = 0 To saltBytes.Length - 1
        '        saltBytes(i) = hashWithSaltBytes(hashSizeInBytes + i)
        '        salt = salt + hashWithSaltBytes(hashSizeInBytes + i).ToString()
        '    Next

        '    ' Compute a new hash string.
        '    Dim expectedHashString As String = GeneraHash(plainText, hashAlgorithm, salt)

        '    ' If the computed hash matches the specified hash,
        '    ' the plain text value must be correct.
        '    Return (hashString = expectedHashString)
        'End Function

        'Public Function Encrypt(ByVal original As String, ByVal key As String) As String
        '    Dim DES As TripleDESCryptoServiceProvider

        '    Dim hashmd5 As MD5CryptoServiceProvider

        '    Dim keyhash(), buff() As Byte

        '    Dim encrypted As String

        '    hashmd5 = New MD5CryptoServiceProvider()

        '    keyhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key))

        '    hashmd5 = Nothing

        '    DES = New TripleDESCryptoServiceProvider()

        '    DES.Key = keyhash

        '    DES.Mode = CipherMode.ECB

        '    buff = ASCIIEncoding.ASCII.GetBytes(original)

        '    encrypted = Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length))

        '    Return encrypted

        'End Function

        '/ <summary>

        '/ Decrypts a string

        '/ </summary>

        '/ <param name="encrypted">the encrypted string</param>

        '/ <param name="key">the key used in encryption</param>

        '/ <returns>the decrypted string</returns>

        'Public Function Decrypt(ByVal encrypted As String, ByVal key As String) As String
        '    Dim DES As TripleDESCryptoServiceProvider

        '    Dim hashmd5 As MD5CryptoServiceProvider

        '    Dim keyhash(), buff() As Byte

        '    Dim decrypted As String

        '    hashmd5 = New MD5CryptoServiceProvider()

        '    keyhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key))

        '    hashmd5 = Nothing

        '    DES = New TripleDESCryptoServiceProvider()

        '    DES.Key = keyhash

        '    DES.Mode = CipherMode.ECB

        '    buff = Convert.FromBase64String(encrypted)

        '    decrypted = ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length))

        '    Return decrypted

        'End Function
#End Region
    End Class

End Namespace
