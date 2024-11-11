Imports System
Imports System.Data
Imports System.IO
Imports System.Text
Imports System.Xml

Public Class clsClienteSeg
    Inherits clibSeguridadCR.Seguridad.clsBase

    Private elem As XmlElement

    Private p_Semilla As String

    Private ReadOnly Property getClase() As String
        Get
            Return System.Reflection.MethodBase.GetCurrentMethod.DeclaringType.FullName
        End Get
    End Property

    Public IdOrganizacion As Integer
    Public IdTransaccion As Integer
    Public IdOpcion As Integer
    Public ArrParams() As Object


    Public Sub New()
        IdOrganizacion = 0
        IdTransaccion = 0
        IdOpcion = 1
    End Sub


    Public Function ConsSemilla() As String
        Try
            Dim objseg As New Seguridad.clsSeguridadCR
            Dim objCry As New Seguridad.clsEncripta
            Return objCry.Decrypt(objseg.getSemilla())
        Catch ex As Exception
        End Try
        Return ""
    End Function

    Public Sub SetSemilla(PI_Semilla As String)
        p_Semilla = PI_Semilla
    End Sub



    Public Function getSesionIni(ByVal PI_XmlSession As XmlDocument) As String
        Dim xml As XmlDocument = getXmlSessionDefault(PI_XmlSession)
        Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
        Return objEncSess.Encripta(xml.OuterXml, getSemillaKey())
    End Function

    Public Function LoginAplicacion(ByVal IdAplicacion As Integer, ByVal UsrApl As String, ByVal Maquina As String) As XmlDocument
        'Valida usuario, clave y permisos de seguridad
        Dim Token As String = ""
        Dim xmlDatos As XmlDocument
        Dim xmlResul As XmlDocument = New System.Xml.XmlDocument
        Dim objenc As New Seguridad.clsEncripta
        Try
            'se recupera la MACADDRESS DE LA MAQUINA
            'Dim objMac As clibSeguridad.Seguridad.clsMACAddress = New clibSeguridad.Seguridad.clsMACAddress
            'Dim Maquina As String = Context.Request.UserHostAddress
            Dim MacMaquina As String '= objMac.GetMACAddress(Maquina)

            xmlDatos = New XmlDocument()
            xmlDatos.LoadXml("<Datos />")
            xmlDatos.DocumentElement.SetAttribute("IdAplicacion", IdAplicacion)
            xmlDatos.DocumentElement.SetAttribute("IdUsuario", UsrApl)
            xmlDatos.DocumentElement.SetAttribute("Maquina", Maquina)

            'Dim objAdm As New clibAdministracion.Administracion.clsUsuario
            'xmlResul = objAdm.ConsLoginUser(xmlDatos)

            elem = xmlDatos.DocumentElement
            objBase.SetDefaultUsuario(elem)
            Organizacion = 1 'Seguridad
            Transaccion = 217
            ReDim arrParam(1)
            Me.arrParam(0) = elem.GetAttribute("IdUsuario")
            If elem.GetAttribute("IdAplicacion").Length = 0 Then
                Me.arrParam(1) = elem.GetAttribute("PS_IdAplicacion")
            Else
                Me.arrParam(1) = elem.GetAttribute("IdAplicacion")
            End If

            xmlResul = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)

            If xmlResul.DocumentElement.GetAttribute("CodError").Equals("0") Then
                'Recupera la MAC a partir de la IP si es local, si el usuario tiene un 
                'perfil que necesite MAC, caso contrario envia la IP no mas.
                Dim elemU As XmlElement = xmlResul.DocumentElement.FirstChild
                If elemU.GetAttribute("MAC").Equals("S") Then
                    Dim objMAC As clibSeguridadCR.Seguridad.clsMACAddress = New clibSeguridadCR.Seguridad.clsMACAddress
                    MacMaquina = objMAC.GetMACAddress(Maquina)
                Else
                    MacMaquina = ""
                End If
            Else 'Usuario no existe
                'Retorna el mensaje
                Return xmlResul
            End If

            Dim objSeg As clibSeguridadCR.Seguridad.clsSeguridadCR = New clibSeguridadCR.Seguridad.clsSeguridadCR
            ''Preguntar si es el usuario de sitio Web del Framework para recuperar el ultimo
            'token asignado
            'If UsrApl.ToLower().Equals("usrwebfs") Then
            '    Token = objSeg.getUltimoToken(xmlDatos)
            '    If Token.Length > 0 Then
            '        'se encontro un token registrado, se retorna
            '        xmlResul.DocumentElement.SetAttribute("PS_UsrSitio", UsrApl)
            '        xmlResul.DocumentElement.SetAttribute("PS_TokenSitio", Token)
            '        xmlResul.DocumentElement.SetAttribute("PS_MaqSitio", Maquina)
            '        Return objenc.Encripta(xmlResul.OuterXml, p_key)
            '    End If
            'End If
            objSeg.SetSemilla(p_Semilla)

            Token = DateTime.Now.ToString("ffffffddhhyyyymmMMssffffffddssmm")
            xmlDatos.DocumentElement.SetAttribute("Token", Token)
            xmlDatos.DocumentElement.SetAttribute("MacMaquina", MacMaquina)

            'Hay que validad el usuario de red y la clave
            'Dim objActDir As clibActiveDirectory.clsActiveDirectory = New clibActiveDirectory.clsActiveDirectory
            Dim xmlAD As XmlDocument
            'xmlAD = objActDir.ValidaLogin("", UsrApl, Maquina)
            xmlAD = New XmlDocument
            xmlAD.LoadXml("<Usuario CodError='0' />")

            If (xmlAD.DocumentElement.GetAttribute("CodError").Equals("0")) Then
                'Registra el Login Aplicativo y retorna los parametros si tuviera
                'Hay que enviar Encripta=N para que tome el usuario no encriptado
                xmlResul = objSeg.LoginAplicacion(xmlDatos)
                xmlResul.DocumentElement.SetAttribute("PS_UsrSitio", UsrApl)
                xmlResul.DocumentElement.SetAttribute("PS_TokenSitio", Token)
                xmlResul.DocumentElement.SetAttribute("PS_MaqSitio", Maquina)
                'dmunoz 24-marzo-2010
                ' validacion hecha para permitir que se ejecute IsPermisoUserTransOpcion
                ' sin necesidad de ejecutar primero un RegistraUser
                xmlResul.DocumentElement.SetAttribute("PS_IdUsuario", UsrApl)
                xmlResul.DocumentElement.SetAttribute("PS_Token", Token)
                xmlResul.DocumentElement.SetAttribute("PS_Maquina", Maquina)

            Else
                xmlResul.LoadXml("<Registro />")
                xmlResul.DocumentElement.SetAttribute("CodError", xmlAD.DocumentElement.GetAttribute("CodError"))
                xmlResul.DocumentElement.SetAttribute("MsgError", xmlAD.DocumentElement.GetAttribute("MsgError"))
            End If
        Catch ex As Exception
            xmlResul.LoadXml("<Registro CodError='-211' MsgError='Mensaje XML recibido tiene formato incorrecto' />")
        End Try
        Return xmlResul
    End Function

    Public Function RegistraUser(ByVal PI_Session As String, ByVal PI_xmlParam As String) As XmlDocument
        Return RegistraUserFull(PI_Session, PI_xmlParam, True)
    End Function

    Public Function RegistraUserEmpleado(ByVal PI_Session As String, ByVal PI_xmlParam As String) As XmlDocument
        Return RegistraUserFull(PI_Session, PI_xmlParam, True)
    End Function

    'Private Function RegistraUserFull(ByVal PI_Session As String, ByVal PI_xmlParam As String, PI_ValidaClave As Boolean) As XmlDocument
    '    Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
    '    Dim XmlSession As XmlDocument = New XmlDocument()
    '    XmlSession.LoadXml(objEncSess.Desencripta(PI_Session, getSemillaKey()))
    '    'Valida usuario, clave y permisos de seguridad
    '    Dim Usuario, Token, Clave As String
    '    Dim msgXML As String = ""
    '    Dim docParam As XmlDocument = New System.Xml.XmlDocument
    '    Dim docXML As XmlDocument = New System.Xml.XmlDocument
    '    'Dim RolesAD As String = ""
    '    Try
    '        docParam.LoadXml(PI_xmlParam)
    '        objBase.SetSemilla(p_Semilla)

    '        Dim elem As XmlElement = docParam.DocumentElement
    '        'Asigna los parametros de entrada
    '        Usuario = elem.GetAttribute("PS_IdUsuario") '.ToLower()
    '        Clave = elem.GetAttribute("PS_Clave")
    '        Token = DateTime.Now.ToString("ffffffddhhyyyymmMMssffffffddssmm")
    '        'RolesAD = elem.GetAttribute("PS_sociedad")

    '        'Si es invocado desde una aplicacion Windows, se recupera la IdMaquina Cliente
    '        'Si es invocado desde una aplicacion Web, se solicita la Id del Browser Cliente
    '        'Si es aplicacion Windows, el PS_UsrSitio es "" porque no se da LoginAplicacion
    '        Dim Maquina As String
    '        Dim MacMaquina As String = ""
    '        Maquina = elem.GetAttribute("PS_Maquina")
    '        If Maquina.Length > 20 Then
    '            Maquina = Maquina.Substring(0, 20)
    '        End If

    '        'Hay que preguntar si el usuario que da login en que active directory
    '        'se valida, debe taer UrlAD y si se debe o no recuperar MACAddress
    '        Dim objAdm As New Administracion.clsLogon
    '        Dim elemU As XmlElement = Nothing

    '        XmlSession.DocumentElement.SetAttribute("PS_IdUsuario", Usuario)
    '        XmlSession.DocumentElement.SetAttribute("PS_Maquina", elem.GetAttribute("PS_Maquina"))
    '        XmlSession.DocumentElement.SetAttribute("PS_Login", elem.GetAttribute("PS_Login"))
    '        XmlSession.DocumentElement.SetAttribute("Perfil", elem.GetAttribute("Perfil"))

    '        XmlSession.DocumentElement.SetAttribute("IdUsuario", Usuario)

    '        Dim BanderaApl As String = elem.GetAttribute("PS_BandAplicativo")
    '        If BanderaApl = "1" Then
    '            PI_ValidaClave = True
    '        End If

    '        'docXML = objAdm.ConsLoginUser(XmlSession)
    '        'If docXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
    '        'docXML.DocumentElement.SetAttribute("Autenticacion", "S")
    '        Dim docLogin As New XmlDocument()
    '        Dim xmlAD As New XmlDocument
    '        'Dim oUtil As New Util.clsUtilitario()
    '        docLogin.LoadXml(objEncSess.Desencripta(PI_Session, getSemillaKey()))
    '        docLogin.DocumentElement.SetAttribute("PV_Identificacion", Usuario)
    '        docLogin.DocumentElement.SetAttribute("PV_Clave", objAdm.Encrypt(Clave))
    '        If (PI_ValidaClave) Then
    '            Dim objLogin As Administracion.clsLogon = New Administracion.clsLogon
    '            xmlAD = objLogin.ValidaLogin(docLogin)
    '            objLogin = Nothing
    '        Else
    '            xmlAD.LoadXml("<Registro CodError=""0"" MsgError="""" />")
    '        End If
    '        'Else 'Usuario no existe
    '        '    'Retorna el mensaje
    '        '    Return docXML
    '        'End If

    '        If (xmlAD.DocumentElement.GetAttribute("CodError").Equals("0")) Then
    '            'recupero info del usuario en proveedores, sino recupera se asume empleado
    '            Dim docValReg As New XmlDocument()
    '            Dim xmlReg As New XmlDocument
    '            docValReg.LoadXml("<Root />")
    '            docValReg.DocumentElement.SetAttribute("IdEmpresa", elem.GetAttribute("PS_IdEmpresa"))
    '            docValReg.DocumentElement.SetAttribute("Ruc", elem.GetAttribute("PS_Ruc"))
    '            docValReg.DocumentElement.SetAttribute("Usuario", elem.GetAttribute("PS_Usuario"))

    '            xmlReg = ValidaRegistroProveedor(docLogin, docValReg)

    '            Dim rowNode As XmlNode = xmlReg.SelectSingleNode("/Registro/Row")

    '            If (xmlReg.DocumentElement.GetAttribute("CodError").Equals("0") And (
    '                    (PI_ValidaClave = True And rowNode IsNot Nothing) Or
    '                    (PI_ValidaClave = False))
    '                ) Then
    '                'Recupera la MAC a partir de la IP si es local, si el usuario tiene un 
    '                'perfil que necesite MAC, caso contrario envia la IP no mas.
    '                If elem.GetAttribute("MAC") <> Nothing Then
    '                    If elem.GetAttribute("MAC").Equals("S") Then
    '                        Dim objMAC As clibSeguridadCR.Seguridad.clsMACAddress = New clibSeguridadCR.Seguridad.clsMACAddress
    '                        MacMaquina = objMAC.GetMACAddress(Maquina)
    '                    Else
    '                        MacMaquina = ""
    '                    End If
    '                End If

    '                'Dim fecha As String = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")
    '                'Dim objEncripta As clibSeguridad.Seguridad.clsEncripta = New clibSeguridad.Seguridad.clsEncripta
    '                'Token = objEncripta.Encrypt(fecha, Token).ToUpper()
    '                'Actualiza el Token y Maquina del usuario
    '                XmlSession.DocumentElement.SetAttribute("PS_Token", Token)
    '                XmlSession.DocumentElement.SetAttribute("PS_Maquina", Maquina)
    '                XmlSession.DocumentElement.SetAttribute("PS_MacMaquina", MacMaquina)
    '                'XmlSession.DocumentElement.SetAttribute("PS_RolesAD", RolesAD)

    '                Dim objSeg As clibSeguridadCR.Seguridad.clsSeguridadCR = New clibSeguridadCR.Seguridad.clsSeguridadCR
    '                docXML = objSeg.RegistraUsuario(XmlSession)
    '                docXML.DocumentElement.SetAttribute("PS_Token", Token)
    '                docXML.DocumentElement.SetAttribute("PS_Maquina", Maquina)

    '                'adiciono información del usuario del modulo proveedor si existe
    '                For Each eleR As XmlElement In xmlReg.DocumentElement.ChildNodes
    '                    docXML.DocumentElement.SetAttribute("Usuario", eleR.GetAttribute("Usuario"))
    '                    docXML.DocumentElement.SetAttribute("IdentParticipante", eleR.GetAttribute("Identificacion"))
    '                    docXML.DocumentElement.SetAttribute("CodSAP", eleR.GetAttribute("CodSAP"))
    '                    docXML.DocumentElement.SetAttribute("NombreParticipante", eleR.GetAttribute("Nombre"))
    '                    docXML.DocumentElement.SetAttribute("CorreoE", eleR.GetAttribute("CorreoE"))
    '                    docXML.DocumentElement.SetAttribute("Celular", eleR.GetAttribute("Celular"))
    '                    docXML.DocumentElement.SetAttribute("EsAdmin", eleR.GetAttribute("EsAdmin"))
    '                    docXML.DocumentElement.SetAttribute("EsEtiqueta", eleR.GetAttribute("EsEtiqueta"))
    '                Next

    '                'NUEVO XML SESION - lo adiciono  a la respuesta
    '                Dim docSession As New XmlDocument()
    '                docSession.LoadXml(objEncSess.Desencripta(PI_Session, getSemillaKey()))
    '                docSession.DocumentElement.SetAttribute("PS_IdUsuario", Usuario)
    '                docSession.DocumentElement.SetAttribute("PS_Maquina", Maquina)
    '                docSession.DocumentElement.SetAttribute("PS_Token", Token)
    '                docXML.DocumentElement.SetAttribute("PXML_SESSION", objEncSess.Encripta(docSession.OuterXml, getSemillaKey()))
    '            Else

    '                Dim msgTemp As String
    '                Dim codTemp As String

    '                msgTemp = xmlAD.DocumentElement.GetAttribute("MsgError")
    '                codTemp = xmlAD.DocumentElement.GetAttribute("CodError")

    '                If msgTemp.Equals("Ingreso exitoso") Then
    '                    msgTemp = "Datos ingresados incorrectamente."
    '                    codTemp = "1"
    '                End If

    '                docXML.LoadXml("<Registro />")
    '                docXML.DocumentElement.SetAttribute("CodError", codTemp)
    '                docXML.DocumentElement.SetAttribute("MsgError", msgTemp)
    '            End If
    '        Else
    '            docXML.LoadXml("<Registro />")
    '            docXML.DocumentElement.SetAttribute("CodError", xmlAD.DocumentElement.GetAttribute("CodError"))
    '            docXML.DocumentElement.SetAttribute("MsgError", xmlAD.DocumentElement.GetAttribute("MsgError"))
    '        End If
    '    Catch ex As Exception
    '        docXML.LoadXml("<Registro CodError=""-211"" MsgError="""" />")
    '        docXML.DocumentElement.SetAttribute("MsgError", ex.Message)
    '    End Try
    '    'Se envia los nuevos datos encriptados
    '    Return docXML
    'End Function

    Private Function RegistraUserFull(ByVal PI_Session As String, ByVal PI_xmlParam As String, PI_ValidaClave As Boolean) As XmlDocument
        Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
        Dim XmlSession As XmlDocument = New XmlDocument()
        XmlSession.LoadXml(objEncSess.Desencripta(PI_Session, getSemillaKey()))
        'Valida usuario, clave y permisos de seguridad
        Dim Usuario, Token, Clave As String
        Dim msgXML As String = ""
        Dim docParam As XmlDocument = New System.Xml.XmlDocument
        Dim docXML As XmlDocument = New System.Xml.XmlDocument
        Try
            docParam.LoadXml(PI_xmlParam)
            objBase.SetSemilla(p_Semilla)

            Dim elem As XmlElement = docParam.DocumentElement
            'Asigna los parametros de entrada
            Usuario = elem.GetAttribute("PS_IdUsuario") '.ToLower()
            Clave = elem.GetAttribute("PS_Clave")
            Token = DateTime.Now.ToString("ffffffddhhyyyymmMMssffffffddssmm")

            'Si es invocado desde una aplicacion Windows, se recupera la IdMaquina Cliente
            'Si es invocado desde una aplicacion Web, se solicita la Id del Browser Cliente
            'Si es aplicacion Windows, el PS_UsrSitio es "" porque no se da LoginAplicacion
            Dim Maquina As String
            Dim MacMaquina As String = ""
            Maquina = elem.GetAttribute("PS_Maquina")
            If Maquina.Length > 20 Then
                Maquina = Maquina.Substring(0, 20)
            End If

            'Hay que preguntar si el usuario que da login en que active directory
            'se valida, debe taer UrlAD y si se debe o no recuperar MACAddress
            Dim objAdm As New Administracion.clsLogon
            Dim elemU As XmlElement = Nothing

            XmlSession.DocumentElement.SetAttribute("PS_IdUsuario", Usuario)
            XmlSession.DocumentElement.SetAttribute("PS_Maquina", elem.GetAttribute("PS_Maquina"))
            XmlSession.DocumentElement.SetAttribute("PS_Login", elem.GetAttribute("PS_Login"))
            XmlSession.DocumentElement.SetAttribute("Perfil", elem.GetAttribute("Perfil"))

            XmlSession.DocumentElement.SetAttribute("IdUsuario", Usuario)

            'docXML = objAdm.ConsLoginUser(XmlSession)
            'If docXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
            'docXML.DocumentElement.SetAttribute("Autenticacion", "S")
            Dim docLogin As New XmlDocument()
            Dim xmlAD As New XmlDocument
            'Dim oUtil As New Util.clsUtilitario()
            docLogin.LoadXml(objEncSess.Desencripta(PI_Session, getSemillaKey()))
            docLogin.DocumentElement.SetAttribute("PV_Identificacion", Usuario)
            docLogin.DocumentElement.SetAttribute("PV_Clave", objAdm.Encrypt(Clave))
            If (PI_ValidaClave) Then
                Dim objLogin As Administracion.clsLogon = New Administracion.clsLogon
                xmlAD = objLogin.ValidaLogin(docLogin)
                objLogin = Nothing
            Else
                xmlAD.LoadXml("<Registro CodError=""0"" MsgError="""" />")
            End If
            'Else 'Usuario no existe
            '    'Retorna el mensaje
            '    Return docXML
            'End If

            If (xmlAD.DocumentElement.GetAttribute("CodError").Equals("0")) Then
                'recupero info del usuario en proveedores, sino recupera se asume empleado
                Dim docValReg As New XmlDocument()
                Dim xmlReg As New XmlDocument
                docValReg.LoadXml("<Root />")
                docValReg.DocumentElement.SetAttribute("IdEmpresa", elem.GetAttribute("PS_IdEmpresa"))
                docValReg.DocumentElement.SetAttribute("Ruc", elem.GetAttribute("PS_Ruc"))
                docValReg.DocumentElement.SetAttribute("Usuario", elem.GetAttribute("PS_Usuario"))

                xmlReg = ValidaRegistroProveedor(docLogin, docValReg)

                If (xmlReg.DocumentElement.GetAttribute("CodError").Equals("0")) Then
                    'Recupera la MAC a partir de la IP si es local, si el usuario tiene un 
                    'perfil que necesite MAC, caso contrario envia la IP no mas.
                    If elem.GetAttribute("MAC") <> Nothing Then
                        If elem.GetAttribute("MAC").Equals("S") Then
                            Dim objMAC As clibSeguridadCR.Seguridad.clsMACAddress = New clibSeguridadCR.Seguridad.clsMACAddress
                            MacMaquina = objMAC.GetMACAddress(Maquina)
                        Else
                            MacMaquina = ""
                        End If
                    End If

                    'Dim fecha As String = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")
                    'Dim objEncripta As clibSeguridad.Seguridad.clsEncripta = New clibSeguridad.Seguridad.clsEncripta
                    'Token = objEncripta.Encrypt(fecha, Token).ToUpper()
                    'Actualiza el Token y Maquina del usuario
                    XmlSession.DocumentElement.SetAttribute("PS_Token", Token)
                    XmlSession.DocumentElement.SetAttribute("PS_Maquina", Maquina)
                    XmlSession.DocumentElement.SetAttribute("PS_MacMaquina", MacMaquina)
                    Dim objSeg As clibSeguridadCR.Seguridad.clsSeguridadCR = New clibSeguridadCR.Seguridad.clsSeguridadCR
                    docXML = objSeg.RegistraUsuario(XmlSession)
                    docXML.DocumentElement.SetAttribute("PS_Token", Token)
                    docXML.DocumentElement.SetAttribute("PS_Maquina", Maquina)

                    'adiciono información del usuario del modulo proveedor si existe
                    For Each eleR As XmlElement In xmlReg.DocumentElement.ChildNodes
                        docXML.DocumentElement.SetAttribute("Usuario", eleR.GetAttribute("Usuario"))
                        docXML.DocumentElement.SetAttribute("IdentParticipante", eleR.GetAttribute("Identificacion"))
                        docXML.DocumentElement.SetAttribute("CodSAP", eleR.GetAttribute("CodSAP"))
                        docXML.DocumentElement.SetAttribute("NombreParticipante", eleR.GetAttribute("Nombre"))
                        docXML.DocumentElement.SetAttribute("CorreoE", eleR.GetAttribute("CorreoE"))
                        docXML.DocumentElement.SetAttribute("Celular", eleR.GetAttribute("Celular"))
                        docXML.DocumentElement.SetAttribute("EsAdmin", eleR.GetAttribute("EsAdmin"))
                        docXML.DocumentElement.SetAttribute("EsEtiqueta", eleR.GetAttribute("EsEtiqueta"))
                    Next

                    'NUEVO XML SESION - lo adiciono  a la respuesta
                    Dim docSession As New XmlDocument()
                    docSession.LoadXml(objEncSess.Desencripta(PI_Session, getSemillaKey()))
                    docSession.DocumentElement.SetAttribute("PS_IdUsuario", Usuario)
                    docSession.DocumentElement.SetAttribute("PS_Maquina", Maquina)
                    docSession.DocumentElement.SetAttribute("PS_Token", Token)
                    docXML.DocumentElement.SetAttribute("PXML_SESSION", objEncSess.Encripta(docSession.OuterXml, getSemillaKey()))
                Else
                    docXML.LoadXml("<Registro />")
                    docXML.DocumentElement.SetAttribute("CodError", xmlReg.DocumentElement.GetAttribute("CodError"))
                    docXML.DocumentElement.SetAttribute("MsgError", xmlReg.DocumentElement.GetAttribute("MsgError"))
                End If
            Else
                docXML.LoadXml("<Registro />")
                docXML.DocumentElement.SetAttribute("CodError", xmlAD.DocumentElement.GetAttribute("CodError"))
                docXML.DocumentElement.SetAttribute("MsgError", xmlAD.DocumentElement.GetAttribute("MsgError"))
            End If
        Catch ex As Exception
            docXML.LoadXml("<Registro CodError=""-211"" MsgError="""" />")
            docXML.DocumentElement.SetAttribute("MsgError", ex.Message)
        End Try
        'Se envia los nuevos datos encriptados
        Return docXML
    End Function

    Public Function CambiarClave(ByVal PI_Session As String, ByVal PI_User As String,
                                ByVal PI_ClaveOld As String, ByVal PI_ClaveNew As String, Optional ByVal PI_RUC As String = "") As XmlDocument
        Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
        Dim XmlSession As XmlDocument = New XmlDocument()
        Dim docXML As XmlDocument = New System.Xml.XmlDocument
        Dim LV_ID_User As String
        Try
            LV_ID_User = PI_User
            If (PI_RUC <> "") Then
                LV_ID_User = PI_RUC.Substring(0, 10) & PI_User
            End If
            Dim objAdm As New Administracion.clsLogon
            XmlSession.LoadXml(objEncSess.Desencripta(PI_Session, getSemillaKey()))
            XmlSession.DocumentElement.SetAttribute("IdUsuario", LV_ID_User)
            XmlSession.DocumentElement.SetAttribute("ClaveOld", objAdm.Encrypt(PI_ClaveOld))
            XmlSession.DocumentElement.SetAttribute("ClaveNew", objAdm.Encrypt(PI_ClaveNew))
            Dim objLogin As Administracion.clsLogon = New Administracion.clsLogon
            docXML = objLogin.CambiarClave(XmlSession)
        Catch ex As Exception
            docXML.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")
            docXML.DocumentElement.SetAttribute("CodError", "-1")
            docXML.DocumentElement.SetAttribute("MsgError", ex.Message)
        End Try
        Return docXML
    End Function

    Public Function DesbloquearClave(ByVal PI_Session As String, ByVal PI_User As String,
                                ByVal PI_CambiarClave As Boolean, ByVal PI_ClaveNew As String, Optional ByVal PI_RUC As String = "") As XmlDocument
        Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
        Dim XmlSession As XmlDocument = New XmlDocument()
        Dim docXML As XmlDocument = New System.Xml.XmlDocument
        Dim LV_ID_User As String
        Try
            LV_ID_User = PI_User
            If (PI_RUC <> "") Then
                LV_ID_User = PI_RUC.Substring(0, 10) & PI_User
            End If
            Dim objAdm As New Administracion.clsLogon
            XmlSession.LoadXml(objEncSess.Desencripta(PI_Session, getSemillaKey()))
            XmlSession.DocumentElement.SetAttribute("IdUsuario", LV_ID_User)
            If (PI_CambiarClave) Then
                XmlSession.DocumentElement.SetAttribute("CambiarClave", "S")
                XmlSession.DocumentElement.SetAttribute("ClaveNew", objAdm.Encrypt(PI_ClaveNew))
            Else
                XmlSession.DocumentElement.SetAttribute("CambiarClave", "N")
                XmlSession.DocumentElement.SetAttribute("ClaveNew", "")
            End If
            Dim objLogin As Administracion.clsLogon = New Administracion.clsLogon
            docXML = objLogin.DesbloquearClave(XmlSession)
        Catch ex As Exception
            docXML.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")
            docXML.DocumentElement.SetAttribute("CodError", "-1")
            docXML.DocumentElement.SetAttribute("MsgError", ex.Message)
        End Try
        Return docXML
    End Function

    Public Function CambiarClaveRecupera(ByVal PI_Session As String, ByVal PI_User As String,
                                ByVal PI_ClaveNew As String, Optional ByVal PI_RUC As String = "") As XmlDocument
        Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
        Dim XmlSession As XmlDocument = New XmlDocument()
        Dim docXML As XmlDocument = New System.Xml.XmlDocument
        Dim LV_ID_User As String
        Try
            LV_ID_User = PI_User
            If (PI_RUC <> "") Then
                LV_ID_User = PI_RUC.Substring(0, 10) & PI_User
            End If
            Dim objAdm As New Administracion.clsLogon
            XmlSession.LoadXml(objEncSess.Desencripta(PI_Session, getSemillaKey()))
            XmlSession.DocumentElement.SetAttribute("IdUsuario", LV_ID_User)
            XmlSession.DocumentElement.SetAttribute("ClaveNew", objAdm.Encrypt(PI_ClaveNew))
            Dim objLogin As Administracion.clsLogon = New Administracion.clsLogon
            docXML = objLogin.CambiarClaveRecupera(XmlSession)
        Catch ex As Exception
            docXML.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")
            docXML.DocumentElement.SetAttribute("CodError", "-1")
            docXML.DocumentElement.SetAttribute("MsgError", ex.Message)
        End Try
        Return docXML
    End Function

    Public Function EjecutaTransaccionDS(ByVal Session As String) As DataSet
        Dim elem As XmlElement
        Dim dsResul As New DataSet
        Try
            Dim p_Log As String = (CStr(System.Configuration.ConfigurationManager.AppSettings("RutaLog"))).Trim()
            'Registra si esta activo Trace
            clsError.setMetodo(True, "clSeguridad", "EjecutaTransaccionDS")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))

            Dim loge As Logger.Logger = New Logger.Logger()
            loge.FilePath = p_Log
            loge.WriteMensaje("XmlSession: " + XmlSession.OuterXml)
            loge.Linea()

            elem = XmlSession.DocumentElement
            'Datos del usuario de sitio
            objBase.SetUsuario(elem)
            'se valida licencia de entrada
            If objBase.objLicencia Then
                'Datos de la transaccion a ejecutar
                Organizacion = IdOrganizacion
                Transaccion = IdTransaccion
                Opcion = IdOpcion
                'Pregunta cuantos parametros envia.
                arrParam = ArrParams
                'ejecuta la transacion
                objBase.SetSemilla(p_Semilla)
                loge.FilePath = p_Log
                loge.WriteMensaje("antes de llamar objeto de BDD")
                loge.Linea()
                dsResul = objBase.Exec_SPBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
            End If
        Catch err As Exception
            dsResul = clsError.setMensajeDS(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "ejecutaTransaccionDS", "")
        End Try
        Return dsResul
    End Function

    Public Function EjecutaTransaccionXML(ByVal Session As String) As XmlDocument
        Dim elem As XmlElement
        Dim xmlResul As New XmlDocument
        Try
            'Registra si esta activo Trace
            clsError.setMetodo(True, "clSeguridad", "EjecutaTransaccionXML")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))
            elem = XmlSession.DocumentElement
            'Datos del usuario de sitio
            objBase.SetUsuario(elem)
            'se valida licencia de entrada
            If objBase.objLicencia Then
                'Datos de la transaccion a ejecutar
                Organizacion = IdOrganizacion
                Transaccion = IdTransaccion
                Opcion = IdOpcion
                'Pregunta cuantos parametros envia.
                arrParam = ArrParams
                'ejecuta la transacion
                objBase.SetSemilla(p_Semilla)
                xmlResul = objBase.Exec_SP_ReaderBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
            End If
        Catch err As Exception
            xmlResul = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "EjecutaTransaccionXML", "")
        End Try
        Return xmlResul
    End Function

    Public Function IsPermisoUserTransOpcion(ByVal Session As String,
                                             ByVal IdOrganizacion As Integer,
                                             ByVal IdTransaccion As Integer,
                                             ByVal IdOpcion As Integer,
                                             ByVal txtXmlTransaccion As String) As XmlDocument
        Dim xml As New XmlDocument
        Dim p_Log As String = (CStr(System.Configuration.ConfigurationManager.AppSettings("RutaLog"))).Trim()
        Dim loge As Logger.Logger = New Logger.Logger()
        Try
            'Registra inicio en trace	
            clsError.setMetodo(True, "clsSeguridad", "IsPermisoUserTransOpcion")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))
            Dim elem As XmlElement = XmlSession.DocumentElement
            'Datos del usuario de sitio
            objBase.SetUsuario(elem)
            'Datos del stored procedure
            Organizacion = 1 'Seguridad
            Transaccion = 169
            ReDim arrParam(11)
            arrParam(0) = elem.GetAttribute("PS_IdUsuario")
            arrParam(1) = elem.GetAttribute("PS_IdAplicacion")
            arrParam(2) = elem.GetAttribute("PS_IdEmpresa")
            arrParam(3) = elem.GetAttribute("PS_IdSucursal")
            arrParam(4) = IdOrganizacion.ToString
            arrParam(5) = IdTransaccion.ToString
            arrParam(6) = IdOpcion.ToString
            arrParam(7) = elem.GetAttribute("PS_Maquina")
            arrParam(8) = elem.GetAttribute("PS_Token")
            arrParam(9) = txtXmlTransaccion
            arrParam(10) = "" 'elem.GetAttribute("ParamAut")
            arrParam(11) = "" 'elem.GetAttribute("ValorAut")



            loge.FilePath = p_Log
            loge.WriteMensaje("=======> IsPermisoUserTransOpcion - Log Permiso: ")
            loge.Linea()

            loge.FilePath = p_Log
            loge.WriteMensaje("TxtTrx " & arrParam(0) & " - " & arrParam(1) & " - " & arrParam(2) & " - " & arrParam(3) & " - " &
                arrParam(4) & " - " & arrParam(5) & " - " & arrParam(6) & " - " & arrParam(7) & " - " &
                arrParam(8) & " - " & arrParam(9) & " - " & arrParam(10) & " - " & arrParam(11) & " - ")

            loge.Linea()


            xml = objBase.Exec_SP_ParamOut(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
            If xml.DocumentElement.GetAttribute("CodError").Equals("0") Then
                If xml.DocumentElement.GetAttribute("PO_Permiso").Equals("1") Then
                    xml.LoadXml("<Root CodError=""0"" MsgError="""" Permiso=""S"" />")
                Else
                    xml.LoadXml("<Root CodError=""0"" MsgError="""" Permiso=""N"" />")
                End If
            End If

        Catch err As Exception
            loge.FilePath = p_Log
            loge.WriteMensaje("=======>ERROR IsPermisoUserTransOpcion - Log Permiso: ")
            loge.Linea()

            xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "IsPermisoUserTransOpcion", xml.OuterXml)
        End Try
        Return xml
    End Function


    Public Function GrabaUsuarioAdministrador(ByVal Session As String, PI_XmlDoc As String) As XmlDocument
        Dim elem As XmlElement
        Dim xmlResul As New XmlDocument
        Dim xmlDoc As New XmlDocument
        Dim respXML As New XmlDocument
        Try
            'Registra si esta activo Trace
            clsError.setMetodo(True, "clSeguridad", "GrabaUsuarioAdministrador")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))
            elem = XmlSession.DocumentElement
            xmlDoc.LoadXml(PI_XmlDoc)
            'Datos del usuario de sitio
            objBase.SetUsuario(elem)
            objBase.SetSemilla(p_Semilla)
            Dim xEleUsr As XmlElement = xmlDoc.DocumentElement.FirstChild
            If (xEleUsr.Name = "New") Then
                xEleUsr.SetAttribute("UsrAutorizador", elem.GetAttribute("PS_IdUsuario"))
                ' -->> INGRESO EN ESTRUCTURA PROVEEDORES
                Organizacion = 39
                Transaccion = 6
                Opcion = 1
                ReDim arrParam(2)
                arrParam(0) = xmlDoc.OuterXml
                arrParam(1) = ""
                respXML = objBase.Exec_SP_ParamOutBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
                If respXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    xEleUsr.SetAttribute("Usuario", respXML.DocumentElement.GetAttribute("PO_Usuario"))
                    xEleUsr.SetAttribute("IdUsuario", xEleUsr.GetAttribute("Ruc").Substring(0, 10) & respXML.DocumentElement.GetAttribute("PO_Usuario"))
                    ' armado de estructuras
                    Dim xmlPar As XmlDocument = getXmlIngParticipanteProvAdmin(xEleUsr)
                    ' -->> INGRESO EN FRAMEWORK SEGURIDAD
                    Organizacion = 2 'Participante
                    Transaccion = 41
                    ReDim arrParam(2)
                    Me.arrParam(0) = xmlPar.OuterXml
                    Me.arrParam(1) = "<Participante />" 'PI_datosXMLcep.OuterXm
                    respXML = objBase.Exec_SP_ParamOut(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                    If respXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
                        xEleUsr.SetAttribute("IdParticipante", respXML.DocumentElement.GetAttribute("PO_IdParticipante"))
                        Dim xmlDocID As New XmlDocument
                        xmlDocID.LoadXml("<Root />")
                        Dim xEleID As XmlElement = xmlDocID.CreateElement("NewID")
                        xEleID.SetAttribute("IdEmpresa", xEleUsr.GetAttribute("IdEmpresa"))
                        xEleID.SetAttribute("Ruc", xEleUsr.GetAttribute("Ruc"))
                        xEleID.SetAttribute("Usuario", xEleUsr.GetAttribute("Usuario"))
                        xEleID.SetAttribute("IdParticipante", xEleUsr.GetAttribute("IdParticipante"))
                        xmlDocID.DocumentElement.AppendChild(xEleID)
                        ' -->> ACTUALIZO REGISTRO NUEVO EN ESTRUCTURA PROVEEDORES
                        Organizacion = 39
                        Transaccion = 6
                        Opcion = 1
                        ReDim arrParam(2)
                        arrParam(0) = xmlDocID.OuterXml
                        arrParam(1) = xEleUsr.GetAttribute("Usuario")
                        respXML = objBase.Exec_SP_ReaderBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
                        respXML.DocumentElement.SetAttribute("IdParticipante", xEleUsr.GetAttribute("IdParticipante"))
                        respXML.DocumentElement.SetAttribute("Usuario", xEleUsr.GetAttribute("Usuario"))
                        xmlResul = respXML
                        If respXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
                            ' -->> ACTUALIZO ROLES EN FRAMEWORK SEGURIDAD
                            Dim xmlRoles As XmlDocument = getXmlActualizarRoles(xEleUsr)
                            ActualizaRolesUsuarioFS(xmlRoles)
                        Else
                            EliminaUsuarioAdministradorNuevo(xEleUsr)
                            EliminaParticipanteFS(xEleUsr.GetAttribute("IdParticipante"))
                        End If
                    Else
                        xmlResul = respXML
                        EliminaUsuarioAdministradorNuevo(xEleUsr)
                    End If
                Else
                    xmlResul = respXML
                End If
            ElseIf (xEleUsr.Name = "Mod") Then
                xEleUsr.SetAttribute("IdUsuario", xEleUsr.GetAttribute("Ruc").Substring(0, 10) & xEleUsr.GetAttribute("Usuario"))
                ' -->> ACTUALIZO EN ESTRUCTURA PROVEEDORES
                Organizacion = 39
                Transaccion = 6
                Opcion = 1
                ReDim arrParam(2)
                arrParam(0) = xmlDoc.OuterXml
                arrParam(1) = xEleUsr.GetAttribute("Usuario")
                respXML = objBase.Exec_SP_NoQueryBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
                xmlResul = respXML
                If respXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    ' -->> ACTUALIZO ROLES EN FRAMEWORK SEGURIDAD
                    Dim xmlRoles As XmlDocument = getXmlActualizarRoles(xEleUsr)
                    ActualizaRolesUsuarioFS(xmlRoles)
                End If
            Else
                xmlResul.LoadXml("<Root CodError=""-1"" MsgError=""Xml recibido con estructura no reconocida"" />")
            End If
        Catch err As Exception
            xmlResul = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "GrabaUsuarioAdministrador", "")
        End Try
        Return xmlResul
    End Function

    Public Function GrabaUsuarioAdicional(ByVal Session As String, PI_XmlDoc As String) As XmlDocument
        Dim elem As XmlElement
        Dim xmlResul As New XmlDocument
        Dim xmlDoc As New XmlDocument
        Dim respXML As New XmlDocument
        Try
            'Registra si esta activo Trace
            clsError.setMetodo(True, "clSeguridad", "GrabaUsuarioAdicional")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))
            elem = XmlSession.DocumentElement
            xmlDoc.LoadXml(PI_XmlDoc)
            'Datos del usuario de sitio
            objBase.SetUsuario(elem)
            objBase.SetSemilla(p_Semilla)
            Dim xEleUsr As XmlElement = xmlDoc.DocumentElement.FirstChild
            If (xEleUsr.Name = "New") Then
                xEleUsr.SetAttribute("IdUsuario", xEleUsr.GetAttribute("Ruc").Substring(0, 10) & xEleUsr.GetAttribute("Usuario"))
                ' armado de estructuras
                Dim xmlPar As XmlDocument = getXmlIngParticipanteProvAdicional(xEleUsr)
                ' -->> INGRESO EN FRAMEWORK SEGURIDAD
                Organizacion = 2 'Participante
                Transaccion = 41
                ReDim arrParam(2)
                Me.arrParam(0) = xmlPar.OuterXml
                Me.arrParam(1) = "<Participante />" 'PI_datosXMLcep.OuterXm
                respXML = objBase.Exec_SP_ParamOut(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                If respXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    xEleUsr.SetAttribute("IdParticipante", respXML.DocumentElement.GetAttribute("PO_IdParticipante"))
                    xEleUsr.SetAttribute("UsrAutorizador", elem.GetAttribute("PS_IdUsuario"))
                    ' -->> INGRESO EN ESTRUCTURA PROVEEDORES
                    Organizacion = 39
                    Transaccion = 7
                    Opcion = 1
                    ReDim arrParam(1)
                    arrParam(0) = xmlDoc.OuterXml
                    respXML = objBase.Exec_SP_NoQueryBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
                    respXML.DocumentElement.SetAttribute("IdParticipante", xEleUsr.GetAttribute("IdParticipante"))
                    xmlResul = respXML
                    If respXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
                        ' -->> ACTUALIZO ROLES EN FRAMEWORK SEGURIDAD
                        Dim xmlRoles As XmlDocument = getXmlActualizarRoles(xEleUsr)
                        ActualizaRolesUsuarioFS(xmlRoles)
                    Else
                        EliminaParticipanteFS(xEleUsr.GetAttribute("IdParticipante"))
                    End If
                Else
                    xmlResul = respXML
                End If
            ElseIf (xEleUsr.Name = "Mod") Then
                xEleUsr.SetAttribute("IdUsuario", xEleUsr.GetAttribute("Ruc").Substring(0, 10) & xEleUsr.GetAttribute("Usuario"))
                ' -->> ACTUALIZO EN ESTRUCTURA PROVEEDORES
                Organizacion = 39
                Transaccion = 7
                Opcion = 1
                ReDim arrParam(1)
                arrParam(0) = xmlDoc.OuterXml
                respXML = objBase.Exec_SP_NoQueryBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
                xmlResul = respXML
                If respXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    ' -->> ACTUALIZO ROLES EN FRAMEWORK SEGURIDAD
                    Dim xmlRoles As XmlDocument = getXmlActualizarRoles(xEleUsr)
                    ActualizaRolesUsuarioFS(xmlRoles)
                End If
            Else
                xmlResul.LoadXml("<Root CodError=""-1"" MsgError=""Xml recibido con estructura no reconocida"" />")
            End If
        Catch err As Exception
            xmlResul = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "GrabaUsuarioAdicional", "")
        End Try
        Return xmlResul
    End Function


    Public Function GrabaActivacionNuevoUsuario(ByVal Session As String, PI_XmlDoc As String) As XmlDocument
        Dim elem As XmlElement
        Dim xmlResul As New XmlDocument
        Dim xmlDoc As New XmlDocument
        Dim respXML As New XmlDocument
        Try
            'Registra si esta activo Trace
            clsError.setMetodo(True, "clSeguridad", "GrabaActivacionNuevoUsuario")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))
            elem = XmlSession.DocumentElement
            xmlDoc.LoadXml(PI_XmlDoc)
            'Datos del usuario de sitio
            objBase.SetUsuario(elem)
            objBase.SetSemilla(p_Semilla)
            Dim objAdm As New Administracion.clsLogon
            xmlDoc.DocumentElement.SetAttribute("ClaveNew", objAdm.Encrypt(xmlDoc.DocumentElement.GetAttribute("ClaveNew")))
            xmlDoc.DocumentElement.SetAttribute("IdUsuario", xmlDoc.DocumentElement.GetAttribute("Ruc").Substring(0, 10) & xmlDoc.DocumentElement.GetAttribute("Usuario"))
            ' -->> ACTUALIZACION EN FRAMEWORK SEGURIDAD
            Organizacion = 1 'Seguridad
            Transaccion = 903
            ReDim arrParam(1)
            Me.arrParam(0) = xmlDoc.OuterXml
            respXML = objBase.Exec_SP_NoQuery(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
            If respXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
                xmlDoc.DocumentElement.RemoveAttribute("ClaveNew")
                ' -->> ACTUALIZACION EN ESTRUCTURA PROVEEDORES
                Organizacion = 39
                Transaccion = 10
                Opcion = 1
                ReDim arrParam(1)
                arrParam(0) = xmlDoc.OuterXml
                respXML = objBase.Exec_SP_NoQueryBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
            End If
            xmlResul = respXML
        Catch err As Exception
            xmlResul = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "GrabaActivacionNuevoUsuario", "")
        End Try
        Return xmlResul
    End Function



    Public Function ConsCatalogoFS(ByVal Session As String, ByVal PI_CodCatalogo As String) As DataSet
        Dim elem As XmlElement
        Dim dsResul As New DataSet
        Try
            'Registra si esta activo Trace
            clsError.setMetodo(True, "clSeguridad", "ConsCatalogoFS")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))
            elem = XmlSession.DocumentElement
            'objBase.SetDefaultUsuario(elem)
            objBase.SetUsuario(elem)
            'se valida licencia de entrada
            If objBase.objLicencia Then
                Organizacion = 5
                Transaccion = 5
                Opcion = 1
                ReDim arrParam(1)
                Me.arrParam(0) = PI_CodCatalogo
                dsResul = objBase.Exec_SP(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
            End If
        Catch err As Exception
            dsResul = clsError.setMensajeDS(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "ConsCatalogoFS", "")
        End Try
        Return dsResul
    End Function

    Public Function ConsRolesPorOrgFS(ByVal Session As String, ByVal PI_CodOrganizacion As String) As DataSet
        Dim elem As XmlElement
        Dim dsResul As New DataSet
        Try
            'Registra si esta activo Trace
            clsError.setMetodo(True, "clSeguridad", "ConsRolesPorOrgFS")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))
            elem = XmlSession.DocumentElement
            objBase.SetUsuario(elem)
            'se valida licencia de entrada
            If objBase.objLicencia Then
                Organizacion = 1
                Transaccion = 300
                Opcion = 1
                ReDim arrParam(1)
                Me.arrParam(0) = PI_CodOrganizacion
                dsResul = objBase.Exec_SP(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
            End If
        Catch err As Exception
            dsResul = clsError.setMensajeDS(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "ConsRolesPorOrgFS", "")
        End Try
        Return dsResul
    End Function

    Public Function ConsRolesPorUsuarioFS(ByVal Session As String, ByVal PI_Usuario As String, Optional ByVal PI_RUC As String = "") As DataSet
        Dim elem As XmlElement
        Dim dsResul As New DataSet
        Dim LV_ID_User As String
        Try
            LV_ID_User = PI_Usuario
            If (PI_RUC <> "") Then
                LV_ID_User = PI_RUC.Substring(0, 10) & PI_Usuario
            End If
            'Registra si esta activo Trace
            clsError.setMetodo(True, "clSeguridad", "ConsRolesPorUsuarioFS")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))
            elem = XmlSession.DocumentElement
            objBase.SetUsuario(elem)
            'se valida licencia de entrada
            If objBase.objLicencia Then
                Organizacion = 1
                Transaccion = 78
                Opcion = 1
                ReDim arrParam(1)
                Me.arrParam(0) = LV_ID_User
                dsResul = objBase.Exec_SP(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
            End If
        Catch err As Exception
            dsResul = clsError.setMensajeDS(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "ConsRolesPorUsuarioFS", "")
        End Try
        Return dsResul
    End Function

    Public Function ConsRolesPorListaUsuariosFS(ByVal Session As String, ByVal PI_Usuario() As String, Optional ByVal PI_RUC As String = "") As DataSet
        Dim elem As XmlElement
        Dim dsResul As New DataSet
        Dim xmlUsrs As New XmlDocument
        Try
            xmlUsrs.LoadXml("<Root />")
            If (PI_RUC <> "") Then
                PI_RUC = PI_RUC.Substring(0, 10)
            End If
            For Each usrItem As String In PI_Usuario
                Dim xlUsr As XmlElement = xmlUsrs.CreateElement("Usr")
                xlUsr.SetAttribute("Usuario", PI_RUC & usrItem)
                xmlUsrs.DocumentElement.AppendChild(xlUsr)
            Next
            'Registra si esta activo Trace
            clsError.setMetodo(True, "clSeguridad", "ConsRolesPorListaUsuariosFS")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))
            elem = XmlSession.DocumentElement
            objBase.SetUsuario(elem)
            'se valida licencia de entrada
            If objBase.objLicencia Then
                Organizacion = 1
                Transaccion = 301
                Opcion = 1
                ReDim arrParam(1)
                Me.arrParam(0) = xmlUsrs.OuterXml
                dsResul = objBase.Exec_SP(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                If (PI_RUC <> "") Then
                    For Each dr As DataRow In dsResul.Tables(0).Rows
                        dr.BeginEdit()
                        dr.Item("idusuario") = dr.Item("idusuario").ToString.Substring(PI_RUC.Length)
                        dr.EndEdit()
                    Next
                    dsResul.Tables(0).AcceptChanges()
                End If
            End If
        Catch err As Exception
            dsResul = clsError.setMensajeDS(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "ConsRolesPorListaUsuariosFS", "")
        End Try
        Return dsResul
    End Function



    Public Function ConsInformacionSeguraUsuarioFS(ByVal Session As String, ByVal PI_Usuario As String, Optional ByVal PI_RUC As String = "") As DataSet
        Dim elem As XmlElement
        Dim dsResul As New DataSet
        Dim LV_ID_User As String
        Try
            LV_ID_User = PI_Usuario
            If (PI_RUC <> "") Then
                LV_ID_User = PI_RUC.Substring(0, 10) & PI_Usuario
            End If
            'Registra si esta activo Trace
            clsError.setMetodo(True, "clSeguridad", "ConsInformacionSeguraUsuarioFS")
            Dim objEncSess As clibEncripta.clsEncripta = New clibEncripta.clsEncripta()
            Dim XmlSession As XmlDocument = New XmlDocument()
            XmlSession.LoadXml(objEncSess.Desencripta(Session, getSemillaKey()))
            elem = XmlSession.DocumentElement
            objBase.SetUsuario(elem)
            'se valida licencia de entrada
            If objBase.objLicencia Then
                Organizacion = 1
                Transaccion = 905
                Opcion = 1
                ReDim arrParam(1)
                Me.arrParam(0) = LV_ID_User
                dsResul = objBase.Exec_SP(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
            End If
        Catch err As Exception
            dsResul = clsError.setMensajeDS(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
        Finally
            'Registra fin en trace
            clsError.setMetodo(False, "clsSeguridad", "ConsInformacionSeguraUsuarioFS", "")
        End Try
        Return dsResul
    End Function



    Public Shared Function getTblEstado(Codigo As Integer, Mensaje As String) As DataTable
        Dim dt As DataTable = New DataTable("TblEstado")
        dt.Columns.Add("CodError", Type.GetType("System.Int32"))
        dt.Columns.Add("MsgError", Type.GetType("System.String"))
        Dim dr As DataRow = dt.NewRow()
        dr("CodError") = Codigo
        dr("MsgError") = Mensaje
        dt.Rows.Add(dr)
        Return dt
    End Function

    Public Shared Function getXmlEstado(Codigo As Integer, Mensaje As String) As XmlDocument
        Dim xmlResp As XmlDocument
        xmlResp = New XmlDocument()
        xmlResp.LoadXml("<Root />")
        xmlResp.DocumentElement.SetAttribute("CodError", Codigo.ToString())
        xmlResp.DocumentElement.SetAttribute("MsgError", Mensaje)
        Return xmlResp
    End Function




    Private Function getSemillaKey() As String
        Dim Clave As String = "S1pec0m"
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
            Clave = key1.ToString()
        Catch ex As Exception
        End Try
        Return Clave
    End Function

    Private Function getXmlSessionDefault(ByVal PI_XmlSession As XmlDocument) As XmlDocument
        Dim xml As New XmlDocument
        xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Usuario/>")
        Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta

        xml.DocumentElement.SetAttribute("PS_UsrSitio", objEncripta.Decrypt(System.Configuration.ConfigurationManager.AppSettings("UsuarioWS")))
        xml.DocumentElement.SetAttribute("PS_TokenSitio", objEncripta.Decrypt(System.Configuration.ConfigurationManager.AppSettings("TokenWS")))
        xml.DocumentElement.SetAttribute("PS_MaqSitio", System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList(0).ToString())
        xml.DocumentElement.SetAttribute("PS_IdEmpresa", System.Configuration.ConfigurationManager.AppSettings("IdEmpresa"))
        xml.DocumentElement.SetAttribute("PS_IdSucursal", System.Configuration.ConfigurationManager.AppSettings("IdSucursal"))
        xml.DocumentElement.SetAttribute("PS_IdAplicacion", System.Configuration.ConfigurationManager.AppSettings("IdAplicacionApl"))
        'xml.DocumentElement.SetAttribute("PS_IdOrganizacion", System.Configuration.ConfigurationManager.AppSettings("IdOrganizacionApl"))
        xml.DocumentElement.SetAttribute("PS_IdUsuario", PI_XmlSession.DocumentElement.GetAttribute("PS_IdUsuario"))
        xml.DocumentElement.SetAttribute("PS_Token", PI_XmlSession.DocumentElement.GetAttribute("PS_Token"))
        xml.DocumentElement.SetAttribute("PS_Maquina", PI_XmlSession.DocumentElement.GetAttribute("PS_Maquina"))

        xml.DocumentElement.SetAttribute("PS_Login", "S")

        'Retorna el XML conteniendo la Session
        Return xml
    End Function

    Public Function ValidaRegistroProveedor(PI_docXML As XmlDocument, PI_paramXml As XmlDocument) As XmlDocument
        Dim elem As XmlElement
        Dim xmlSP As New XmlDocument()
        Try
            elem = PI_docXML.DocumentElement
            objBase.SetDefaultUsuario(elem)
            Organizacion = 39
            Transaccion = 8
            ReDim arrParam(0)
            arrParam(0) = PI_paramXml.OuterXml
            xmlSP = objBase.Exec_SP_ReaderBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
        Catch ex As Exception
            xmlSP.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")
            xmlSP.DocumentElement.SetAttribute("CodError", "-1")
            xmlSP.DocumentElement.SetAttribute("MsgError", ex.Message)
        End Try
        Return xmlSP
    End Function

    Private Sub EliminaUsuarioAdministradorNuevo(PI_XmlEleUsr As XmlElement)
        Try
            Dim xmlDocEli As New XmlDocument
            xmlDocEli.LoadXml("<Root />")
            Dim xEleEli As XmlElement = xmlDocEli.CreateElement("Eli")
            xEleEli.SetAttribute("IdEmpresa", PI_XmlEleUsr.GetAttribute("IdEmpresa"))
            xEleEli.SetAttribute("Ruc", PI_XmlEleUsr.GetAttribute("Ruc"))
            xEleEli.SetAttribute("Usuario", PI_XmlEleUsr.GetAttribute("Usuario"))
            xmlDocEli.DocumentElement.AppendChild(xEleEli)
            ' -->> ELIMINO EN ESTRUCTURA PROVEEDORES
            Organizacion = 39
            Transaccion = 6
            Opcion = 1
            ReDim arrParam(2)
            arrParam(0) = xmlDocEli.OuterXml
            arrParam(1) = ""
            objBase.Exec_SP_ReaderBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub EliminaParticipanteFS(PI_IdParticipante As String)
        Try
            Organizacion = 2 'Participante
            Transaccion = 43
            Opcion = 1
            ReDim arrParam(1)
            Me.arrParam(0) = PI_IdParticipante
            objBase.Exec_SP_NoQuery(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ActualizaRolesUsuarioFS(PI_docXML As XmlDocument)
        Try
            Organizacion = 1 'Seguridad
            Transaccion = 147
            Opcion = 1
            ReDim arrParam(1)
            Me.arrParam(0) = PI_docXML.OuterXml
            objBase.Exec_SP_NoQuery(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
        Catch ex As Exception
        End Try
    End Sub

    Private Function getXmlIngParticipanteProvAdmin(xEleUsr As XmlElement) As XmlDocument
        Dim objAdm As New Administracion.clsLogon
        Dim xmlPar As New XmlDocument
        xmlPar.LoadXml("<ResultSet />")

        Dim xElePar As XmlElement = xmlPar.CreateElement("Participante")
        xElePar.SetAttribute("IdTipoIdentificacion", "2") 'es RUC
        xElePar.SetAttribute("Identificacion", xEleUsr.GetAttribute("Ruc"))
        xElePar.SetAttribute("TipoParticipante", "E") ' es Empresa
        xElePar.SetAttribute("IdUsuario", xEleUsr.GetAttribute("IdUsuario"))
        xElePar.SetAttribute("TipoPart", "0") ' es default
        xElePar.SetAttribute("Opident", xEleUsr.GetAttribute("Ruc"))
        xElePar.SetAttribute("IdEmpresa", xEleUsr.GetAttribute("IdEmpresa"))
        xElePar.SetAttribute("TipoPartRegistro", "P") ' es Proveedor (U = es Usuario)
        xElePar.SetAttribute("Clave", objAdm.Encrypt(xEleUsr.GetAttribute("Clave")))
        xElePar.SetAttribute("Estado", xEleUsr.GetAttribute("Estado"))
        xElePar.SetAttribute("IdPais", "593") ' es Ecuador
        xElePar.SetAttribute("IdProvincia", "10-04") ' es Guayas
        xElePar.SetAttribute("IdCiudad", "81") ' es Guayaquil
        xElePar.SetAttribute("FechaExpira", "01-01-2999") ' valor fijo
        xElePar.SetAttribute("TiempoExpira", "0") ' valor fijo
        xmlPar.DocumentElement.AppendChild(xElePar)

        Dim xEleEmp As XmlElement = xmlPar.CreateElement("Empresa")
        xEleEmp.SetAttribute("Nombre", xEleUsr.GetAttribute("Nombre"))
        xEleEmp.SetAttribute("IdCategoriaEmpresa", "5") ' valor fijo
        xEleEmp.SetAttribute("Nivel", "2") ' valor fijo
        xEleEmp.SetAttribute("IdEmpresaPadre", "0") ' valor fijo
        xmlPar.DocumentElement.AppendChild(xEleEmp)

        Dim xEleRC As XmlElement = xmlPar.CreateElement("RegistroCliente")
        xEleRC.SetAttribute("IdUsuario", xEleUsr.GetAttribute("IdUsuario"))
        xEleRC.SetAttribute("RolAsignado", "1") ' es Propietario
        xEleRC.SetAttribute("IdUsuarioRegistro", "") ' es Vacio cuando es el mismo
        xEleRC.SetAttribute("OrigenRegistro", "I") ' es Pregunta por Permiso Registro
        xEleRC.SetAttribute("TipoCliente", "1") ' valor fijo
        xEleRC.SetAttribute("IdTipoLogin", "3") ' valor fijo
        xEleRC.SetAttribute("Estado", "1") ' valor fijo
        xmlPar.DocumentElement.AppendChild(xEleRC)


        Dim xEleDirS As XmlElement = xmlPar.CreateElement("Direcciones")
        Dim xEleDir As XmlElement = xmlPar.CreateElement("Direccion_N")
        xEleDir.SetAttribute("IdDireccion", "1")
        xEleDir.SetAttribute("IdTipoDireccion", "1") ' es Casa
        xEleDir.SetAttribute("Direccion", "PROVEEDORES") ' valor fijo
        xEleDir.SetAttribute("IdPais", "593") ' es Ecuador
        xEleDir.SetAttribute("IdProvincia", "10-04") ' es Guayas
        xEleDir.SetAttribute("IdCiudad", "81") ' es Guayaquil
        xEleDirS.AppendChild(xEleDir)
        xmlPar.DocumentElement.AppendChild(xEleDirS)

        Return xmlPar
    End Function

    Private Function getXmlIngParticipanteProvAdicional(xEleUsr As XmlElement) As XmlDocument
        Dim objAdm As New Administracion.clsLogon
        Dim xmlPar As New XmlDocument
        xmlPar.LoadXml("<ResultSet />")

        Dim xElePar As XmlElement = xmlPar.CreateElement("Participante")
        Select Case xEleUsr.GetAttribute("TipoIdent")
            Case "C"
                xElePar.SetAttribute("IdTipoIdentificacion", "1") 'es Cedula
            Case "R"
                xElePar.SetAttribute("IdTipoIdentificacion", "2") 'es RUC
            Case "P"
                xElePar.SetAttribute("IdTipoIdentificacion", "3") 'es Pasaporte
            Case Else
                xElePar.SetAttribute("IdTipoIdentificacion", "4") 'es Otros
        End Select
        xElePar.SetAttribute("Identificacion", xEleUsr.GetAttribute("Identificacion"))
        xElePar.SetAttribute("TipoParticipante", "P") ' es Persona
        xElePar.SetAttribute("IdUsuario", xEleUsr.GetAttribute("IdUsuario"))
        xElePar.SetAttribute("TipoPart", "0") ' es default
        xElePar.SetAttribute("Opident", xEleUsr.GetAttribute("Ruc"))
        xElePar.SetAttribute("IdEmpresa", xEleUsr.GetAttribute("IdEmpresa"))
        xElePar.SetAttribute("TipoPartRegistro", "P") ' es Proveedor (U = es Usuario)
        xElePar.SetAttribute("Clave", objAdm.Encrypt(xEleUsr.GetAttribute("Clave")))
        xElePar.SetAttribute("Estado", xEleUsr.GetAttribute("Estado"))
        xElePar.SetAttribute("IdPais", "593") ' es Ecuador
        xElePar.SetAttribute("IdProvincia", "10-04") ' es Guayas
        xElePar.SetAttribute("IdCiudad", "81") ' es Guayaquil
        xElePar.SetAttribute("FechaExpira", "01-01-2999") ' valor fijo
        xElePar.SetAttribute("TiempoExpira", "0") ' valor fijo
        xmlPar.DocumentElement.AppendChild(xElePar)

        Dim xEleEmp As XmlElement = xmlPar.CreateElement("Persona")
        xEleEmp.SetAttribute("Apellido1", xEleUsr.GetAttribute("Apellido1"))
        xEleEmp.SetAttribute("Apellido2", xEleUsr.GetAttribute("Apellido2"))
        xEleEmp.SetAttribute("Nombre1", xEleUsr.GetAttribute("Nombre1"))
        xEleEmp.SetAttribute("Nombre2", xEleUsr.GetAttribute("Nombre2"))
        xEleEmp.SetAttribute("IdTitulo", "1") ' valor fijo
        xEleEmp.SetAttribute("Sexo", "1") ' valor fijo
        xEleEmp.SetAttribute("FechaNacimiento", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
        xEleEmp.SetAttribute("EstadoCivil", "1") ' valor fijo
        xEleEmp.SetAttribute("Ruc", xEleUsr.GetAttribute("Ruc"))
        xmlPar.DocumentElement.AppendChild(xEleEmp)

        Dim xEleRC As XmlElement = xmlPar.CreateElement("RegistroCliente")
        xEleRC.SetAttribute("IdUsuario", xEleUsr.GetAttribute("IdUsuario"))
        xEleRC.SetAttribute("RolAsignado", "1") ' es Propietario
        xEleRC.SetAttribute("IdUsuarioRegistro", "") ' es Vacio cuando es el mismo
        xEleRC.SetAttribute("OrigenRegistro", "I") ' es Pregunta por Permiso Registro
        xEleRC.SetAttribute("TipoCliente", "1") ' valor fijo
        xEleRC.SetAttribute("IdTipoLogin", "3") ' valor fijo
        xEleRC.SetAttribute("Estado", "1") ' valor fijo
        xmlPar.DocumentElement.AppendChild(xEleRC)


        Dim xEleDirS As XmlElement = xmlPar.CreateElement("Direcciones")
        Dim xEleDir As XmlElement = xmlPar.CreateElement("Direccion_N")
        xEleDir.SetAttribute("IdDireccion", "1")
        xEleDir.SetAttribute("IdTipoDireccion", "1") ' es Casa
        xEleDir.SetAttribute("Direccion", "PROVEEDORES") ' valor fijo
        xEleDir.SetAttribute("IdPais", "593") ' es Ecuador
        xEleDir.SetAttribute("IdProvincia", "10-04") ' es Guayas
        xEleDir.SetAttribute("IdCiudad", "81") ' es Guayaquil
        xEleDirS.AppendChild(xEleDir)
        xmlPar.DocumentElement.AppendChild(xEleDirS)

        Return xmlPar
    End Function

    Private Function getXmlActualizarRoles(xEleUsr As XmlElement) As XmlDocument
        Dim xmlSeg As New XmlDocument
        xmlSeg.LoadXml("<ResultSet />")

        xmlSeg.DocumentElement.SetAttribute("IdUsuario", xEleUsr.GetAttribute("IdUsuario"))

        Dim xEleRoles As XmlElement = xmlSeg.CreateElement("Roles")

        For Each xUsrRol As XmlElement In xEleUsr.ChildNodes
            If (xUsrRol.Name = "Rol") Then
                Dim xElRol As XmlElement = xmlSeg.CreateElement("Rol")
                xElRol.SetAttribute("IdRol", xUsrRol.GetAttribute("IdRol"))
                xElRol.SetAttribute("IdUsuario", xEleUsr.GetAttribute("IdUsuario"))
                xElRol.SetAttribute("IdHorario", "1") ' valor fijo
                xElRol.SetAttribute("Estado", "ACTIVE") ' valor fijo
                xElRol.SetAttribute("FechaInicial", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                xElRol.SetAttribute("FechaFinal", "2999-01-01 00:00:00") ' valor fijo
                xElRol.SetAttribute("TipoIdentificacion", "0") ' valor fijo
                xEleRoles.AppendChild(xElRol)
            End If
        Next

        xmlSeg.DocumentElement.AppendChild(xEleRoles)

        Return xmlSeg
    End Function

End Class
