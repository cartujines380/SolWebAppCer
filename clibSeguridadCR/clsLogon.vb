Imports System
Imports System.Data
Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Environment
Imports System.Security.Cryptography

Namespace Administracion

    Public Class clsLogon
        Inherits clibSeguridadCR.Seguridad.clsBase

        Dim elem As XmlElement

        'Public Function ConsLoginUser(ByVal PI_ParamXml As XmlDocument) As XmlDocument
        '    elem = PI_ParamXml.DocumentElement
        '    objBase.SetDefaultUsuario(elem)
        '    Organizacion = 1 'Seguridad
        '    Transaccion = 217
        '    ReDim arrParam(1)
        '    Me.arrParam(0) = elem.GetAttribute("IdUsuario")
        '    If elem.GetAttribute("IdAplicacion").Length = 0 Then
        '        Me.arrParam(1) = elem.GetAttribute("PS_IdAplicacion")
        '    Else
        '        Me.arrParam(1) = elem.GetAttribute("IdAplicacion")
        '    End If

        '    Return objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
        'End Function

        Public Function ValidaLogin(PI_docXML As XmlDocument) As XmlDocument
            Dim elem As XmlElement
            Dim xmlSP As New XmlDocument()
            Dim xml As New XmlDocument()
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")
            Try
                elem = PI_docXML.DocumentElement
                objBase.SetDefaultUsuario(elem)
                Organizacion = 1
                Transaccion = 900
                ReDim arrParam(3)
                arrParam(0) = PI_docXML.DocumentElement.GetAttribute("PV_Identificacion") 'es en realidad el IdUsuario
                arrParam(1) = PI_docXML.DocumentElement.GetAttribute("PV_Clave")
                Try
                    If (PI_docXML.DocumentElement.GetAttribute("PS_sociedad") = "") Then
                        arrParam(2) = PI_docXML.DocumentElement.GetAttribute("PS_sociedad")
                        arrParam(3) = "2"
                    Else
                        arrParam(2) = PI_docXML.DocumentElement.GetAttribute("PS_sociedad")
                        arrParam(3) = "1"
                    End If
                Catch ex As Exception
                    arrParam(2) = ""
                    arrParam(3) = "2"
                End Try
                xmlSP = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                If (xmlSP.DocumentElement.GetAttribute("CodError").Equals("0")) Then
                    xml.DocumentElement.SetAttribute("CodError", xmlSP.SelectSingleNode("//Row").Attributes("CodError").InnerText)
                    xml.DocumentElement.SetAttribute("MsgError", xmlSP.SelectSingleNode("//Row").Attributes("MsgError").InnerText)
                Else
                    xml.DocumentElement.SetAttribute("CodError", xmlSP.DocumentElement.GetAttribute("CodError"))
                    xml.DocumentElement.SetAttribute("MsgError", xmlSP.DocumentElement.GetAttribute("MsgError"))
                End If
            Catch ex As Exception
                xml.DocumentElement.SetAttribute("CodError", "-1")
                xml.DocumentElement.SetAttribute("MsgError", ex.Message)
            End Try
            Return xml
        End Function

        Public Function CambiarClave(PI_docXML As XmlDocument) As XmlDocument
            Dim elem As XmlElement
            Dim xmlSP As New XmlDocument()
            Dim xml As New XmlDocument()
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")
            Try
                elem = PI_docXML.DocumentElement
                'objBase.SetDefaultUsuario(elem)
                objBase.SetUsuario(elem)
                Organizacion = 1
                Transaccion = 901
                ReDim arrParam(2)
                arrParam(0) = PI_docXML.DocumentElement.GetAttribute("IdUsuario")
                arrParam(1) = PI_docXML.DocumentElement.GetAttribute("ClaveOld")
                arrParam(2) = PI_docXML.DocumentElement.GetAttribute("ClaveNew")
                xmlSP = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                If (xmlSP.DocumentElement.GetAttribute("CodError").Equals("0")) Then
                    xml.DocumentElement.SetAttribute("CodError", xmlSP.SelectSingleNode("//Row").Attributes("CodError").InnerText)
                    xml.DocumentElement.SetAttribute("MsgError", xmlSP.SelectSingleNode("//Row").Attributes("MsgError").InnerText)
                Else
                    xml.DocumentElement.SetAttribute("CodError", xmlSP.DocumentElement.GetAttribute("CodError"))
                    xml.DocumentElement.SetAttribute("MsgError", xmlSP.DocumentElement.GetAttribute("MsgError"))
                End If
            Catch ex As Exception
                xml.DocumentElement.SetAttribute("CodError", "-1")
                xml.DocumentElement.SetAttribute("MsgError", ex.Message)
            End Try
            Return xml
        End Function

        Public Function DesbloquearClave(PI_docXML As XmlDocument) As XmlDocument
            Dim elem As XmlElement
            Dim xmlSP As New XmlDocument()
            Try
                elem = PI_docXML.DocumentElement
                'objBase.SetDefaultUsuario(elem)
                objBase.SetUsuario(elem)
                Organizacion = 1
                Transaccion = 902
                ReDim arrParam(2)
                arrParam(0) = PI_docXML.DocumentElement.GetAttribute("IdUsuario")
                arrParam(1) = PI_docXML.DocumentElement.GetAttribute("CambiarClave")
                arrParam(2) = PI_docXML.DocumentElement.GetAttribute("ClaveNew")
                xmlSP = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
            Catch ex As Exception
                xmlSP.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")
                xmlSP.DocumentElement.SetAttribute("CodError", "-1")
                xmlSP.DocumentElement.SetAttribute("MsgError", ex.Message)
            End Try
            Return xmlSP
        End Function

        Public Function CambiarClaveRecupera(PI_docXML As XmlDocument) As XmlDocument
            Dim elem As XmlElement
            Dim xmlSP As New XmlDocument()
            Try
                elem = PI_docXML.DocumentElement
                'objBase.SetDefaultUsuario(elem)
                objBase.SetUsuario(elem)
                Organizacion = 1
                Transaccion = 904
                ReDim arrParam(1)
                arrParam(0) = PI_docXML.DocumentElement.GetAttribute("IdUsuario")
                arrParam(1) = PI_docXML.DocumentElement.GetAttribute("ClaveNew")
                xmlSP = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
            Catch ex As Exception
                xmlSP.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")
                xmlSP.DocumentElement.SetAttribute("CodError", "-1")
                xmlSP.DocumentElement.SetAttribute("MsgError", ex.Message)
            End Try
            Return xmlSP
        End Function


        Public Function Encrypt(clearText As String) As String
            Dim EncryptionKey As String = "S1pec0m"
            Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
            Using encryptor As Aes = Aes.Create()
                Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
                 &H65, &H64, &H76, &H65, &H64, &H65, _
                 &H76})
                encryptor.Key = pdb.GetBytes(32)
                encryptor.IV = pdb.GetBytes(16)
                Using ms As New MemoryStream()
                    Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                        cs.Write(clearBytes, 0, clearBytes.Length)
                        cs.Close()
                    End Using
                    clearText = Convert.ToBase64String(ms.ToArray())
                End Using
            End Using
            Return clearText
        End Function

    End Class

End Namespace
