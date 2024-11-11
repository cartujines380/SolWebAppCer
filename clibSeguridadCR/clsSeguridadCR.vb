Imports System
Imports System.Data
Imports System.Xml

Namespace Seguridad

    Public Class clsSeguridadCR
        Inherits clsBase

#Region "propiedades privadas general"
        Private _Semilla As String

        Private ReadOnly Property getClase() As String
            Get
                Return System.Reflection.MethodBase.GetCurrentMethod.DeclaringType.FullName
            End Get
        End Property
        Private ReadOnly Property pSemilla() As String
            Get
                Return _Semilla 'System.Web.HttpContext.Current.Application("Semilla")
            End Get
        End Property
#End Region


#Region "Metodos Privados"
        Friend Function getSemilla() As String
            Dim Semilla As String = ""
            Try
                'Registra inicio en trace	
                clsError.setMetodo(True, "clsSeguridad", "getSemilla")

                'Datos del usuario de sitio
                objBase.SetDefaultSitio()

                'Datos del stored procedure
                Organizacion = 1 'Seguridad
                Transaccion = 192 'getSemilla

                ReDim arrParam(0)

                Dim xml As XmlDocument = objBase.Exec_SP_ParamOut(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                If xml.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    Semilla = xml.DocumentElement.GetAttribute("PO_Semilla")
                Else
                    Semilla = ""
                End If
            Catch err As Exception
                clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "getSemilla")
            End Try
            Return Semilla
        End Function
        Private Function getConexion(ByVal PI_docXML As XmlDocument) As XmlDocument

            Dim xml As New XmlDocument
            Dim elem As XmlElement = PI_docXML.DocumentElement
            'Datos del usuario de sitio
            objBase.SetDefaultUsuario(elem)

            Try
                'Registra inicio en trace
                clsError.setMetodo(True, "clsSeguridad", "getConexion")

                'Datos del stored procedure
                Organizacion = 1 'Seguridad
                Transaccion = 185 'getconexion

                ReDim arrParam(1)
                arrParam(0) = elem.GetAttribute("PS_IdAplicacion")
                xml = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)

                'FALTA DESENCRIPTAR LOS DATOS

            Catch err As Exception
                xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)

            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "getConexion", xml.OuterXml)
            End Try

            Return xml

        End Function
        Private Function consServApl(ByVal PI_docXML As XmlDocument) As String
            'Retorna un string con un elemento xml contienido los servidores, si no tiene devuelve vacia ""
            Dim xmlServApl As New XmlDocument
            Dim xmlServ As New XmlDocument
            Dim elem As XmlElement = PI_docXML.DocumentElement
            'Datos del usuario para ver si tiene permisos
            objBase.SetDefaultSitio()
            Try
                'Registra inicio en trace
                clsError.setMetodo(True, "clsSeguridad", "getServApl")
                'Datos del stored procedure
                Organizacion = 1 'Seguridad
                Transaccion = 223 'consulta servidores por aplicacion
                ReDim arrParam(0)
                arrParam(0) = elem.GetAttribute("IdAplicacion")
                xmlServApl = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                'FALTA DESENCRIPTAR LOS DATOS de cada servidor
                Dim objenc As New clsEncripta
                Dim IdServidor As String = ""

                xmlServ.LoadXml("<Servidores />")
                Dim nodoServ As XmlElement
                nodoServ = xmlServ.CreateElement("Servidor")
                If xmlServApl.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    For Each elemServ As XmlElement In xmlServApl.DocumentElement.ChildNodes
                        'procesa cada servidor con sus parametros
                        If IdServidor.Equals("") Then
                            IdServidor = elemServ.GetAttribute("IdServidor")
                            nodoServ.SetAttribute("Id", IdServidor)
                            nodoServ.SetAttribute("Tipo", elemServ.GetAttribute("TipoServidor"))
                        End If
                        If Not IdServidor.Equals(elemServ.GetAttribute("IdServidor")) Then
                            'se crea un nuevo nodo
                            xmlServ.DocumentElement.AppendChild(nodoServ)
                            nodoServ = xmlServ.CreateElement("Servidor")
                            IdServidor = elemServ.GetAttribute("IdServidor")
                            nodoServ.SetAttribute("Id", IdServidor)
                            nodoServ.SetAttribute("Tipo", elemServ.GetAttribute("TipoServidor"))
                        End If
                        If elemServ.GetAttribute("Encriptado").ToLower.Equals("true") Then 'esta encriptado, hay que desencriptarlo
                            nodoServ.SetAttribute(elemServ.GetAttribute("Parametro"), objenc.Decrypt(elemServ.GetAttribute("Valor"), pSemilla))
                        Else
                            nodoServ.SetAttribute(elemServ.GetAttribute("Parametro"), elemServ.GetAttribute("Valor"))
                        End If
                    Next
                    'se ingresa el ultimo
                    xmlServ.DocumentElement.AppendChild(nodoServ)
                End If
            Catch err As Exception
                xmlServApl = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "getServApl", xmlServApl.OuterXml)
            End Try
            If xmlServApl.DocumentElement.GetAttribute("CodError").Equals("0") Then
                Return xmlServ.OuterXml
            Else
                Return ""
            End If
        End Function
        Private Function CambiaCarControlXML(ByVal Trama As String) As String
            Dim lTrama As String = Trama
            Try
                lTrama = lTrama.Replace("&", "&amp;")
                lTrama = lTrama.Replace("'", "&apos;")
                lTrama = lTrama.Replace("""", "&quot;")
                lTrama = lTrama.Replace(">", "&gt;")
                lTrama = lTrama.Replace("<", "&lt;")
            Catch
                lTrama = Trama
            End Try
            Return lTrama
        End Function
#End Region

#Region "Metodos Publicos"
        Public Sub SetSemilla(PISemilla As String)
            _Semilla = PISemilla
        End Sub
        Public Function getPerfil(ByVal PI_docXML As XmlDocument) As XmlDocument

            Dim xml As New XmlDocument
            Dim elem As XmlElement = PI_docXML.DocumentElement

            Dim p_Log As String = (CStr(System.Configuration.ConfigurationManager.AppSettings("RutaLog"))).Trim()
            Dim loge As Logger.Logger = New Logger.Logger()
            loge.FilePath = p_Log
            loge.WriteMensaje("*** Perfil *** ")
            loge.Linea()

            Try
                'Registra si esta activo Trace
                clsError.setMetodo(True, "clSeguridad", "getPerfil")

                'se valida licencia de entrada
                If objBase.objLicencia Then '.plicencia Then

                    'Datos del usuario de sitio
                    objBase.SetDefaultUsuario(elem)

                    'Datos del stored procedure
                    Organizacion = 1 'Seguridad
                    Transaccion = 71

                    ReDim arrParam(7)
                    arrParam(0) = elem.GetAttribute("PS_IdUsuario")
                    arrParam(1) = elem.GetAttribute("PS_Token")
                    arrParam(2) = elem.GetAttribute("PS_Maquina")
                    arrParam(3) = elem.GetAttribute("PS_IdAplicacion")
                    If elem.GetAttribute("IdEmpresa").Equals("") Then
                        arrParam(4) = elem.GetAttribute("PS_IdEmpresa")
                    Else
                        arrParam(4) = elem.GetAttribute("IdEmpresa")
                    End If
                    If elem.GetAttribute("IdSucursal").Equals("") Then
                        arrParam(5) = elem.GetAttribute("PS_IdSucursal")
                    Else
                        arrParam(5) = elem.GetAttribute("IdSucursal")
                    End If
                    If elem.GetAttribute("PS_IdOrganizacion").Equals("") Then
                        arrParam(6) = "0"
                    Else
                        arrParam(6) = elem.GetAttribute("PS_IdOrganizacion")
                    End If
                    'Para saber si recupera todas las transacciones o solo tipo menu
                    If elem.GetAttribute("Perfil").Equals("") Then
                        arrParam(7) = "N"
                    Else
                        arrParam(7) = elem.GetAttribute("Perfil")
                    End If
                    'arrParam(8) = elem.GetAttribute("PS_RolesAD")

                    loge.FilePath = p_Log
                    loge.WriteMensaje("1.-" + arrParam(0) + " 2.-" + arrParam(1) + " 3.-" + arrParam(2) + " 4.-" + arrParam(3) + " 5.-" + arrParam(4) +
                    " 6.-" + arrParam(5) + " 7.-" + arrParam(6))
                    loge.Linea()

                    xml = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)

                    loge.FilePath = p_Log
                    loge.WriteMensaje("XML ROLES -> " + xml.OuterXml)
                    loge.Linea()
                Else
                    xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", "No tiene licencia valida el servidor")
                End If

            Catch err As Exception
                xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "getPerfil", xml.OuterXml)
            End Try


            Return xml

        End Function

        Public Function DesRegistraUsuario(ByVal PI_docXML As XmlDocument) As XmlDocument
            'Dim dr As OleDbDataReader
            Dim elem As XmlElement
            Dim xmlRet As New XmlDocument
            Try
                'Registra si esta activo Trace
                clsError.setMetodo(True, "clSeguridad", "DesRegistraUsuario")
                objBase.CodError = 0
                objBase.MsgError = ""
                elem = PI_docXML.DocumentElement
                'Datos del usuario de sitio
                objBase.SetUsuario(elem)

                'Datos del stored procedure
                Organizacion = 1 'Seguridad
                Transaccion = 92
                ReDim arrParam(3)
                arrParam(0) = elem.GetAttribute("PS_IdUsuario")
                arrParam(1) = elem.GetAttribute("PS_Token")
                arrParam(2) = elem.GetAttribute("PS_Maquina")

                Dim usrdefaul As String = System.Configuration.ConfigurationManager.AppSettings("UsuarioS")
                xmlRet = objBase.Exec_SP_NoQuery(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)

            Catch err As Exception
                xmlRet = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "DesRegistraUsuario")
            End Try
            Return xmlRet
        End Function
        ' Nueno esquema de seguridad
        Public Function RegistraUsuario(ByVal PI_docXML As XmlDocument) As XmlDocument
            Dim elem As XmlElement

            Dim perfil As New XmlDocument
            perfil.LoadXml("<Usuario />")
            Dim xmlApl As XmlDocument

            Try
                clsError.setMetodo(True, "clSeguridad", "RegistraUsuario")

                elem = PI_docXML.DocumentElement
                'Pregunta si el FrameWork tiene licencia
                If objBase.objLicencia Then '.plicencia Then
                    'Pregunta si la empresa seleccionada esta licenciada
                    'Me.getLicEmpresa()

                    'Datos del usuario de sitio
                    objBase.SetDefaultUsuario(elem)

                    'Datos del stored procedure
                    Organizacion = 1 'Seguridad
                    Transaccion = 136

                    ReDim arrParam(6)
                    arrParam(0) = elem.GetAttribute("PS_IdUsuario")
                    arrParam(1) = elem.GetAttribute("PS_Token")
                    arrParam(2) = elem.GetAttribute("PS_Maquina")
                    arrParam(3) = elem.GetAttribute("PS_MacMaquina")
                    arrParam(4) = elem.GetAttribute("PS_IdAplicacion")
                    arrParam(5) = elem.GetAttribute("PS_IdEmpresa")
                    arrParam(6) = elem.GetAttribute("PS_Dominio")

                    Dim xml As XmlDocument = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                    If xml.DocumentElement.GetAttribute("CodError").Equals("0") Then
                        'Recupera las Aplicaciones por usuario
                        Organizacion = 1 'Seguridad
                        Transaccion = 183
                        ReDim arrParam(4)
                        arrParam(0) = elem.GetAttribute("PS_IdUsuario")
                        arrParam(1) = elem.GetAttribute("PS_Token")
                        arrParam(2) = elem.GetAttribute("PS_Maquina")
                        arrParam(3) = elem.GetAttribute("PS_IdEmpresa")
                        arrParam(4) = elem.GetAttribute("PS_IdSucursal")

                        Dim p_Log As String = (CStr(System.Configuration.ConfigurationManager.AppSettings("RutaLog"))).Trim()
                        Dim loge As Logger.Logger = New Logger.Logger()
                        loge.FilePath = p_Log
                        loge.WriteMensaje("*** antes Perfil *** ")
                        loge.Linea()

                        loge.FilePath = p_Log
                        loge.WriteMensaje("1.-" + arrParam(0) + " 2.-" + arrParam(1) + " 3.-" + arrParam(2) + " 4.-" + arrParam(3) + " 5.-" + arrParam(4))

                        loge.Linea()

                        xmlApl = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                        If xmlApl.DocumentElement.GetAttribute("CodError").Equals("0") Then
                            If elem.GetAttribute("Perfil").Equals("M") OrElse
                                elem.GetAttribute("Perfil").Equals("T") Then
                                'Recupera los datos del perfil
                                loge.FilePath = p_Log
                                loge.WriteMensaje("*** Va a ingresar a Perfil *** ")
                                perfil = getPerfil(PI_docXML)
                                Dim elemPart As XmlElement = xml.DocumentElement.FirstChild
                                If Not IsNothing(elemPart) Then
                                    perfil.DocumentElement.SetAttribute("IdParticipante", elemPart.GetAttribute("IdParticipante"))
                                    perfil.DocumentElement.SetAttribute("NombreParticipante", elemPart.GetAttribute("Nombre"))
                                    perfil.DocumentElement.SetAttribute("IdentParticipante", elemPart.GetAttribute("Identificacion"))
                                    perfil.DocumentElement.SetAttribute("Estado", elemPart.GetAttribute("Estado"))
                                    perfil.DocumentElement.SetAttribute("IdEmpresaUsuario", elemPart.GetAttribute("IdEmpresaUsuario"))
                                    perfil.DocumentElement.SetAttribute("Opident", elemPart.GetAttribute("Opident"))
                                    perfil.DocumentElement.SetAttribute("EsClaveNuevo", elemPart.GetAttribute("EsClaveNuevo"))
                                    perfil.DocumentElement.SetAttribute("EsClaveCambio", elemPart.GetAttribute("EsClaveCambio"))
                                    perfil.DocumentElement.SetAttribute("EsClaveBloqueo", elemPart.GetAttribute("EsClaveBloqueo"))
                                    perfil.DocumentElement.SetAttribute("CargoEmpleado", elemPart.GetAttribute("CargoEmpleado"))

                                    perfil.DocumentElement.SetAttribute("Rol", elemPart.GetAttribute("Rol"))
                                    perfil.DocumentElement.SetAttribute("rucEmpresa", elemPart.GetAttribute("rucEmpresa"))
                                    perfil.DocumentElement.SetAttribute("nomEmpresa", elemPart.GetAttribute("nomEmpresa"))
                                    Dim elemApl As XmlElement = perfil.CreateElement("Aplicacion")
                                    elemApl.InnerXml = xmlApl.DocumentElement.InnerXml
                                    perfil.DocumentElement.AppendChild(elemApl)
                                End If
                            Else 'No recupera perfil, enviar vacio
                                perfil.DocumentElement.SetAttribute("CodError", "0")
                                perfil.DocumentElement.SetAttribute("MsgError", "")
                            End If
                        Else
                            perfil.DocumentElement.SetAttribute("CodError", xmlApl.DocumentElement.GetAttribute("CodError"))
                            perfil.DocumentElement.SetAttribute("MsgError", xmlApl.DocumentElement.GetAttribute("MsgError"))
                        End If
                    Else
                        perfil.DocumentElement.SetAttribute("CodError", xml.DocumentElement.GetAttribute("CodError"))
                        perfil.DocumentElement.SetAttribute("MsgError", xml.DocumentElement.GetAttribute("MsgError"))
                    End If
                Else
                    perfil.DocumentElement.SetAttribute("CodError", "-1")
                    perfil.DocumentElement.SetAttribute("MsgError", "El servidor no tiene licencia valida")
                End If

            Catch err As Exception
                perfil = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "RegistraUsuario", perfil.OuterXml)
            End Try

            Return perfil

        End Function
        Public Function LoginAplicacion(ByVal PI_docXML As XmlDocument) As XmlDocument
            Dim elem As XmlElement
            Dim parametros As New XmlDocument
            parametros.LoadXml("<Parametros />")
            Try
                clsError.setMetodo(True, "clSeguridad", "LoginAplicacion")

                elem = PI_docXML.DocumentElement
                'Pregunta si el FrameWork tiene licencia
                If objBase.objLicencia Then '.plicencia Then

                    'Datos del usuario de sitio
                    objBase.SetDefaultSitio()

                    'Datos del stored procedure
                    Organizacion = 1 'Seguridad
                    Transaccion = 191 'LoginAplicativo

                    ReDim arrParam(4)
                    arrParam(0) = elem.GetAttribute("IdAplicacion")
                    arrParam(1) = elem.GetAttribute("IdUsuario")
                    arrParam(2) = elem.GetAttribute("Token")
                    arrParam(3) = elem.GetAttribute("Maquina")
                    arrParam(4) = elem.GetAttribute("MacMaquina")

                    parametros = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                    'procesa los parametros para desencritarlos si campo Encriptado es 1
                    If parametros.DocumentElement.GetAttribute("CodError").Equals("0") Then
                        Dim objenc As New clsEncripta
                        For Each elemParam As XmlElement In parametros.DocumentElement.ChildNodes
                            If elemParam.GetAttribute("Encriptado").ToLower.Equals("true") Then 'esta encriptado, hay que desencriptarlo
                                parametros.DocumentElement.SetAttribute(elemParam.GetAttribute("Parametro"), objenc.Decrypt(elemParam.GetAttribute("Valor"), pSemilla))
                            Else
                                parametros.DocumentElement.SetAttribute(elemParam.GetAttribute("Parametro"), elemParam.GetAttribute("Valor"))
                            End If
                        Next
                        'Procesa los Servidores si tiene check para retornar
                        If parametros.DocumentElement.GetAttribute("RetServApl").ToUpper().Equals("S") Then
                            parametros.DocumentElement.InnerXml = consServApl(PI_docXML)
                        Else
                            parametros.DocumentElement.InnerXml = ""
                        End If
                    End If

                Else
                    parametros.DocumentElement.SetAttribute("CodError", "-1")
                    parametros.DocumentElement.SetAttribute("MsgError", "El servidor no tiene licencia valida")
                End If

            Catch err As Exception
                parametros = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + parametros.OuterXml)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "LoginAplicativo", parametros.OuterXml)
            End Try

            Return parametros

        End Function
        Public Function LoginAplicacionMT(ByVal PI_docXML As XmlDocument) As XmlDocument
            Dim elem As XmlElement
            Dim parametros As New XmlDocument
            parametros.LoadXml("<Parametros />")
            Try
                clsError.setMetodo(True, "clSeguridad", "LoginAplicacion")

                elem = PI_docXML.DocumentElement
                'Pregunta si el FrameWork tiene licencia
                If objBase.objLicencia Then '.plicencia Then

                    'Datos del usuario de sitio
                    objBase.SetDefaultSitio()

                    'Datos del stored procedure
                    Organizacion = 1 'Seguridad
                    Transaccion = 225 'LoginAplicativoMT

                    ReDim arrParam(5)
                    arrParam(0) = elem.GetAttribute("IdAplicacion")
                    arrParam(1) = elem.GetAttribute("IdUsuario")
                    arrParam(2) = elem.GetAttribute("Token")
                    arrParam(3) = elem.GetAttribute("Maquina")
                    arrParam(4) = elem.GetAttribute("MacMaquina")
                    arrParam(5) = "S"

                    parametros.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1""?><Registro CodError=""0"" MsgError=""""></Registro>")
                    Dim ds As DataSet, dr As DataRow
                    ds = objBase.Exec_SP(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                    'procesa los parametros para desencritarlos si campo Encriptado es 1
                    dr = ds.Tables("TblEstado").Rows(0)
                    If dr("CodError").ToString.Equals("0") Then
                        Dim objenc As New clsEncripta
                        For Each dr In ds.Tables(0).Rows
                            If dr("Encriptado").ToString.ToLower.Equals("true") Then 'esta encriptado, hay que desencriptarlo
                                parametros.DocumentElement.SetAttribute(dr("Parametro").ToString, objenc.Decrypt(dr("Valor").ToString, ""))
                            Else
                                parametros.DocumentElement.SetAttribute(dr("Parametro").ToString, dr("Valor").ToString)
                            End If
                        Next
                        parametros.DocumentElement.InnerXml = ""
                        'Procesa los Servidores
                        Dim xmlServ As XmlElement
                        Dim IdServidor As String = ""
                        xmlServ = parametros.CreateElement("Servidores")
                        Dim nodoServ As XmlElement
                        nodoServ = parametros.CreateElement("Servidor")
                        For Each dr In ds.Tables(1).Rows
                            'procesa cada servidor con sus parametros
                            If IdServidor.Equals("") Then
                                IdServidor = dr("IdServidor").ToString
                                nodoServ.SetAttribute("Id", IdServidor)
                                nodoServ.SetAttribute("Tipo", dr("TipoServidor").ToString)
                            End If
                            If Not IdServidor.Equals(dr("IdServidor").ToString) Then
                                'se crea un nuevo nodo
                                xmlServ.AppendChild(nodoServ)
                                nodoServ = parametros.CreateElement("Servidor")
                                IdServidor = dr("IdServidor").ToString
                                nodoServ.SetAttribute("Id", IdServidor)
                                nodoServ.SetAttribute("Tipo", dr("TipoServidor").ToString)
                            End If
                            If dr("Encriptado").ToString.ToLower.Equals("true") Then 'esta encriptado, hay que desencriptarlo
                                nodoServ.SetAttribute(dr("Parametro").ToString, objenc.Decrypt(dr("Valor").ToString, ""))
                            Else
                                nodoServ.SetAttribute(dr("Parametro").ToString, dr("Valor").ToString)
                            End If
                        Next
                        'se ingresa el ultimo
                        xmlServ.AppendChild(nodoServ)
                        parametros.DocumentElement.AppendChild(xmlServ)
                        'procesa los puertos
                        Dim elemP As XmlElement, elemPorts As XmlElement
                        elemPorts = parametros.CreateElement("Puertos")
                        parametros.DocumentElement.AppendChild(elemPorts)
                        For Each dr In ds.Tables(2).Rows
                            elemP = parametros.CreateElement("Puerto")
                            For Each col As DataColumn In ds.Tables(2).Columns
                                elemP.SetAttribute(col.ColumnName, dr(col.ColumnName).ToString)
                            Next
                            elemPorts.AppendChild(elemP)
                        Next
                    Else
                        parametros.DocumentElement.SetAttribute("CodError", dr("CodError").ToString)
                        parametros.DocumentElement.SetAttribute("MsgError", dr("MsgError").ToString)
                    End If

                Else
                    parametros.DocumentElement.SetAttribute("CodError", "-1")
                    parametros.DocumentElement.SetAttribute("MsgError", "El servidor no tiene licencia valida")
                End If

            Catch err As Exception
                parametros = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + parametros.OuterXml)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "LoginAplicativo", parametros.OuterXml)
            End Try

            Return parametros

        End Function
        Public Function consTransaccionesMT(ByVal PI_docXML As XmlDocument) As DataSet
            Dim elem As XmlElement = PI_docXML.DocumentElement
            'Datos del usuario de sitio
            objBase.SetUsuario(elem)
            'Datos del stored procedure
            Dim Salida As String = ""
            Organizacion = 1 'Seguridad
            Transaccion = 226
            Me.arrParam(0) = PI_docXML.OuterXml

            Dim ds As DataSet
            ds = Me.objBase.Exec_SP(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)

            Dim objenc As New clsEncripta
            Dim xmlParam As XmlDocument
            For Each dr As DataRow In ds.Tables(1).Rows
                xmlParam = New XmlDocument
                xmlParam.LoadXml(dr("params").ToString)
                For Each xElem As XmlElement In xmlParam.DocumentElement.ChildNodes
                    If xElem.GetAttribute("Encriptado") = "1" Then
                        xElem.SetAttribute("Valor", objenc.Decrypt(xElem.GetAttribute("Valor"), ""))
                    End If
                    xElem.RemoveAttribute("Encriptado")
                Next
                dr.BeginEdit()
                dr("params") = xmlParam.OuterXml
                dr.EndEdit()
            Next
            ds.AcceptChanges()

            Return ds
        End Function
        Public Function consPermisoUserTransOpcion(ByVal PI_docXML As XmlDocument) As XmlDocument
            Dim xml As New XmlDocument
            Try
                clsError.setMetodo(True, "clsSeguridad", "consPermisoUserTransOpcion")
                Dim elem As XmlElement = PI_docXML.DocumentElement
                'Datos del usuario de sitio
                objBase.SetDefaultUsuario(elem)
                'Datos del stored procedure
                Organizacion = 1 'Seguridad
                Transaccion = 167
                ReDim arrParam(6)
                arrParam(0) = elem.GetAttribute("PS_IdUsuario")
                arrParam(1) = elem.GetAttribute("PS_IdAplicacion")
                arrParam(2) = elem.GetAttribute("PS_IdEmpresa")
                arrParam(3) = elem.GetAttribute("PS_IdSucursal")
                arrParam(4) = elem.GetAttribute("IdOrganizacion")
                arrParam(5) = elem.GetAttribute("IdTransaccion")
                xml = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)

            Catch err As Exception
                xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "consPermisoUserTransOpcion", xml.OuterXml)
            End Try
            Return xml
        End Function
        Public Function IsPermisoUserTransOpcion(ByVal PI_docXML As XmlDocument, ByVal PI_txtTransaccion As String) As Boolean
            Dim xml As New XmlDocument
            Dim retorno As Boolean = False

            Try
                'Registra inicio en trace	
                clsError.setMetodo(True, "clsSeguridad", "IsPermisoUserTransOpcion")
                Dim elem As XmlElement = PI_docXML.DocumentElement
                'Datos del usuario de sitio
                objBase.SetDefaultUsuario(elem)
                'Datos del stored procedure
                Organizacion = 1 'Seguridad
                Transaccion = 169
                ReDim arrParam(11)
                'Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
                'Dim usuario As String
                'If elem.GetAttribute("PS_Login").Equals("N") Then
                '    usuario = elem.GetAttribute("PS_IdUsuario")
                '    usuario = objEncripta.Decrypt(usuario)
                'Else
                '    usuario = elem.GetAttribute("PS_IdUsuario")
                'End If
                arrParam(0) = elem.GetAttribute("PS_IdUsuario") 'usuario 'elem.GetAttribute("PS_IdUsuario")
                arrParam(1) = elem.GetAttribute("PS_IdAplicacion")
                arrParam(2) = elem.GetAttribute("PS_IdEmpresa")
                arrParam(3) = elem.GetAttribute("PS_IdSucursal")
                arrParam(4) = elem.GetAttribute("IdOrganizacion")
                arrParam(5) = elem.GetAttribute("IdTransaccion")
                arrParam(6) = elem.GetAttribute("IdOpcion")
                arrParam(7) = elem.GetAttribute("PS_Maquina")
                arrParam(8) = elem.GetAttribute("PS_Token")
                arrParam(9) = PI_txtTransaccion
                arrParam(10) = elem.GetAttribute("ParamAut")
                arrParam(11) = elem.GetAttribute("ValorAut")
                xml = objBase.Exec_SP_ParamOut(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                If xml.DocumentElement.GetAttribute("CodError").Equals("0") AndAlso _
                    xml.DocumentElement.GetAttribute("PO_Permiso").Equals("1") Then
                    retorno = True
                Else
                    retorno = False
                End If

            Catch err As Exception
                xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "IsPermisoUserTransOpcion", xml.OuterXml)
            End Try
            Return retorno
        End Function
        Public Function IsAutorizadoUserTrans(ByVal PI_docXML As XmlDocument) As Boolean
            Dim xml As New XmlDocument
            Dim retorno As Boolean = False
            Try
                'Registra inicio en trace	
                clsError.setMetodo(True, "clsSeguridad", "IsAutorizadoUserTrans")
                Dim elem As XmlElement = PI_docXML.DocumentElement
                'Datos del usuario de sitio
                objBase.SetUsuario(elem)
                retorno = objBase.VerificaTrans(elem.GetAttribute("PS_IdEmpresa") _
                                            , elem.GetAttribute("PS_IdSucursal") _
                                            , elem.GetAttribute("IdOrganizacion") _
                                            , elem.GetAttribute("IdTransaccion") _
                                            , elem.GetAttribute("IdOpcion") _
                                            , elem.GetAttribute("ParamAut") _
                                            , elem.GetAttribute("ValorAut") _
                                            , elem.GetAttribute("txtTrans"))

            Catch err As Exception
                xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "IsAutorizadoUserTrans", xml.OuterXml)
            End Try
            Return retorno
        End Function
        Public Function Consulta(ByVal PI_docXML As XmlDocument) As DataSet
            Dim ds As New DataSet
            Try
                'Registra inicio en trace	
                clsError.setMetodo(True, "clsSeguridad", "Consulta")
                If objBase.objLicencia Then '.plicencia Then
                    Dim elem As XmlElement = PI_docXML.DocumentElement
                    'Datos del usuario de sitio
                    objBase.SetUsuario(elem)
                    Organizacion = 1
                    Transaccion = 1000
                    'Datos del stored procedure
                    Dim msgError As String = ""
                    'Hay que valida que solo sean Select
                    If elem.GetAttribute("NombreDs").Equals("") Then
                        'Recuperar en que servidor se ejecuta el query
                        If elem.GetAttribute("ServidorBDD").Length = 0 Then
                            ds = Me.objBase.Exec_Query(elem.GetAttribute("Query"))
                        Else
                            ds = Me.objBase.Exec_QueryBA(elem.GetAttribute("Query"), elem.GetAttribute("ServidorBDD"))
                        End If
                    Else
                        'Recuperar en que servidor se ejecuta el query
                        If elem.GetAttribute("ServidorBDD").Length = 0 Then
                            ds = Me.objBase.Exec_Query(elem.GetAttribute("Query"), elem.GetAttribute("NombreDs"))
                        Else
                            ds = Me.objBase.Exec_QueryBA(elem.GetAttribute("Query"), elem.GetAttribute("NombreDs"), elem.GetAttribute("ServidorBDD"))
                        End If
                    End If
                Else
                    ds = clsError.setMensajeDS(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", "No tiene licencia valida el servidor")
                End If

            Catch err As Exception
                ds = clsError.setMensajeDS(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "Consulta", ds.GetXml)
            End Try
            Return ds
        End Function
        Public Function actEstadoRegistro(ByVal PI_DocXml As XmlDocument) As XmlDocument
            'Ingresa Empresa
            Dim xml As New XmlDocument
            Try
                'Registra inicio en trace	
                clsError.setMetodo(True, "clsSeguridad", "actEstadoRegistro")
                If objBase.objLicencia Then '.plicencia Then

                    Dim elem As XmlElement = PI_DocXml.DocumentElement
                    'Datos del usuario de sitio
                    objBase.SetUsuario(elem)
                    Organizacion = 1 'Participante
                    Transaccion = 161
                    ReDim arrParam(1)
                    Me.arrParam(0) = PI_DocXml.OuterXml
                    xml = objBase.Exec_SP_NoQuery(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                Else
                    xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", "No tiene licencia valida en servidor")
                End If
            Catch err As Exception
                xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_DocXml.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "actEstadoRegistro", xml.OuterXml)
            End Try
            Return xml
        End Function
        Public Function VerificaUser(ByVal PI_docXML As XmlDocument) As XmlDocument

            Dim xml As New XmlDocument
            Dim xmlRol As XmlDocument
            Dim xmlPerfil As XmlDocument
            Try
                'Registra inicio en trace	
                clsError.setMetodo(True, "clsSeguridad", "VerificaUser")

                xmlRol = Nothing
                If objBase.objLicencia Then '.plicencia Then

                    Dim elem As XmlElement = PI_docXML.DocumentElement
                    'Datos del usuario de sitio
                    objBase.SetDefaultUsuario(elem)
                    xml.LoadXml("<Usuario />")

                    'Datos del stored procedure
                    Organizacion = 1 'Seguridad
                    Transaccion = 184 'verifcaUser

                    ReDim arrParam(4)
                    arrParam(0) = elem.GetAttribute("PS_IdUsuario")
                    arrParam(1) = elem.GetAttribute("PS_Token")
                    arrParam(2) = elem.GetAttribute("PS_Maquina")
                    arrParam(3) = elem.GetAttribute("PS_IdAplicacion")

                    'xml = objBase.Exec_SP_ParamOut(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                    xml = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                    'procesa los parametros para desencritarlos si campo Encriptado es 1
                    If xml.DocumentElement.GetAttribute("CodError").Equals("0") _
                      AndAlso xml.DocumentElement.GetAttribute("PO_Estado").ToLower().Equals("true") Then
                        Dim objenc As New clsEncripta
                        For Each elemParam As XmlElement In xml.DocumentElement.ChildNodes
                            If elemParam.GetAttribute("Encriptado").ToLower.Equals("true") Then 'esta encriptado, hay que desencriptarlo
                                xml.DocumentElement.SetAttribute(elemParam.GetAttribute("Parametro"), objenc.Decrypt(elemParam.GetAttribute("Valor"), ""))
                            Else
                                xml.DocumentElement.SetAttribute(elemParam.GetAttribute("Parametro"), elemParam.GetAttribute("Valor"))
                            End If
                        Next
                        'Procesa los Servidores  si tiene check para retornar
                        If xml.DocumentElement.GetAttribute("RetServClie").ToUpper().Equals("S") Then
                            PI_docXML.DocumentElement.SetAttribute("IdAplicacion", elem.GetAttribute("PS_IdAplicacion"))
                            xml.DocumentElement.InnerXml = consServApl(PI_docXML)
                        Else
                            xml.DocumentElement.InnerXml = ""
                        End If
                        'Recupera el Perfil del usuario
                        If PI_docXML.DocumentElement.GetAttribute("Perfil").Equals("M") OrElse _
                           PI_docXML.DocumentElement.GetAttribute("Perfil").Equals("T") Then
                            xmlPerfil = getPerfil(PI_docXML)
                            If xmlPerfil.DocumentElement.GetAttribute("CodError").Equals("0") Then
                                'Se agrega al xml que retorna el perfil
                                xml.DocumentElement.InnerXml = xml.DocumentElement.InnerXml + xmlPerfil.DocumentElement.InnerXml
                            End If
                        End If
                    End If

                    'If xml.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    '    If xml.DocumentElement.GetAttribute("PO_Estado").ToLower().Equals("true") Then 'esta activo
                    '        'Recupera datos de Rol de Usuario Activo
                    '        'Recupera los datos del Rol ConsRolUsuarioActivo
                    '        'If PI_docXML.DocumentElement.GetAttribute("Rol").ToUpper().Equals("S") Then
                    '        '    xmlRol = ConsRolUsuarioActivo(PI_docXML)
                    '        '    If Not xml.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    '        '        'retorna con error
                    '        '        Return xml
                    '        '    End If
                    '        'End If
                    '        'Dim elemRol As XmlElement
                    '        'If Not IsNothing(xmlRol) Then
                    '        '    elemRol = xml.CreateElement("Rol")
                    '        '    elemRol.InnerXml = xmlRol.DocumentElement.InnerXml
                    '        '    xml.DocumentElement.AppendChild(elemRol)
                    '        'End If
                    '    End If
                    'End If
                Else
                    xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", "No tiene licencia valida en servidor")
                End If
            Catch err As Exception
                xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "VerificaUser", xml.OuterXml)
            End Try
            Return xml

        End Function
        Public Function ConsRolUsuarioActivo(ByVal PI_docXML As XmlDocument) As XmlDocument

            Dim xml As New XmlDocument

            Try
                'Registra inicio en trace	
                clsError.setMetodo(True, "clsSeguridad", "ConsRolUsuarioActivo")

                Dim elem As XmlElement = PI_docXML.DocumentElement
                'Datos del usuario de sitio
                objBase.SetDefaultUsuario(elem)

                'Datos del stored procedure
                Organizacion = 1 'Seguridad
                Transaccion = 189

                ReDim arrParam(4)
                arrParam(0) = elem.GetAttribute("PS_IdUsuario")
                arrParam(1) = elem.GetAttribute("PS_IdEmpresa")
                arrParam(2) = elem.GetAttribute("PS_IdSucursal")

                xml = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)

            Catch err As Exception
                xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "ConsRolUsuarioActivo", xml.OuterXml)
            End Try
            Return xml

        End Function
        Public Function LoginEntidad(ByVal PI_docXML As XmlDocument) As XmlDocument
            Dim elem, elemP As XmlElement
            Dim xmlResul, xmlPerfil As XmlDocument

            Try
                clsError.setMetodo(True, "clSeguridad", "LoginEntidad")
                If objBase.objLicencia Then '.plicencia Then

                    elem = PI_docXML.DocumentElement
                    'Datos del usuario de sitio
                    objBase.SetDefaultUsuario(elem)

                    'Datos del stored procedure
                    Organizacion = 1 'Seguridad
                    Transaccion = 191

                    ReDim arrParam(4)
                    arrParam(0) = elem.GetAttribute("PS_IdUsuario")
                    arrParam(1) = elem.GetAttribute("PS_Token")
                    arrParam(2) = elem.GetAttribute("PS_Maquina")
                    arrParam(3) = elem.GetAttribute("PS_IdAplicacion")
                    arrParam(4) = elem.GetAttribute("PS_IdEmpresa")
                    xmlResul = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)

                    'Recupera el perfil por cada Empresa y sucursal que tenga
                    If elem.GetAttribute("PS_Perfil").Equals("S") Then 'recupera perfil
                        If xmlResul.DocumentElement.GetAttribute("CodError").Equals("0") Then
                            For Each elemE As XmlElement In xmlResul.DocumentElement.ChildNodes
                                PI_docXML.DocumentElement.SetAttribute("IdEmpresa", elemE.GetAttribute("IdEmpresa"))
                                PI_docXML.DocumentElement.SetAttribute("IdSucursal", elemE.GetAttribute("IdAgencia"))
                                xmlPerfil = getPerfil(PI_docXML)
                                If xmlPerfil.DocumentElement.GetAttribute("CodError").Equals("0") Then
                                    elemP = xmlResul.CreateElement("Perfil")
                                    elemP.InnerXml = xmlPerfil.DocumentElement.InnerXml
                                    elemE.AppendChild(elemP)
                                End If
                            Next
                        End If
                    End If
                Else
                    xmlResul = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", "No tiene licencia valida en servidor")
                End If

            Catch err As Exception
                xmlResul = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "LoginEntidad", xmlResul.OuterXml)
            End Try

            Return xmlResul

        End Function
        'Public Function CambioClave(ByVal PI_docXML As XmlDocument) As XmlDocument
        '    Dim xml As New XmlDocument
        '    Try
        '        'Registra inicio en trace	
        '        clsError.setMetodo(True, "clsSeguridad", "CambioClave")
        '        Dim elem As XmlElement = PI_docXML.DocumentElement
        '        'Datos del usuario de sitio
        '        objBase.SetUsuario(elem)
        '        xml.LoadXml("<Registro/>")
        '        'Datos del stored procedure
        '        Organizacion = 1 'Seguridad
        '        Transaccion = 168 'cambio de clave
        '        If objBase.IsPermiso(Organizacion, Transaccion, Opcion, txtTrans) Then
        '            xml.DocumentElement.SetAttribute("CodError", "0")
        '            xml.DocumentElement.SetAttribute("MsgError", "")
        '            'Cambia la clave en el Active Directory
        '            'Dim objAD As clibActiveDirectory.clsActiveDirectory = New clibActiveDirectory.clsActiveDirectory
        '            'xmlAD = objAD.ModClaveUsuario(PI_docXML)
        '        Else
        '            xml.DocumentElement.SetAttribute("CodError", "50000")
        '            xml.DocumentElement.SetAttribute("MsgError", "No tiene permisos para cambio de clave")
        '        End If

        '    Catch err As Exception
        '        xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + PI_docXML.OuterXml)
        '    Finally
        '        'Registra fin en trace
        '        clsError.setMetodo(False, "clsSeguridad", "CambioClave", xml.OuterXml)
        '    End Try
        '    Return xml

        'End Function
        Public Function IsPermisoAplTransOpcion(ByVal XmlSession As XmlDocument, _
                                                ByVal txtTransaccion As String) As Boolean
            Dim xml As New XmlDocument
            Dim retorno As Boolean = False

            Try
                'Registra inicio en trace	
                clsError.setMetodo(True, "clsSeguridad", "IsPermisoAplTransOpcion")
                Dim elem As XmlElement = XmlSession.DocumentElement
                'Datos del usuario de sitio
                objBase.SetDefaultUsuario(elem)
                'Datos del stored procedure
                Organizacion = 1 'Seguridad
                Transaccion = 218
                ReDim arrParam(8)
                arrParam(0) = elem.GetAttribute("IdUsuario")
                arrParam(1) = elem.GetAttribute("IdIdentificacion")
                arrParam(2) = elem.GetAttribute("PS_IdAplicacion")
                arrParam(3) = elem.GetAttribute("PS_IdEmpresa")
                arrParam(4) = elem.GetAttribute("PS_IdSucursal")
                arrParam(5) = elem.GetAttribute("IdOrganizacion")
                arrParam(6) = elem.GetAttribute("IdTransaccion")
                arrParam(7) = elem.GetAttribute("IdOpcion")
                arrParam(8) = txtTransaccion

                xml = objBase.Exec_SP_ParamOut(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                If xml.DocumentElement.GetAttribute("CodError").Equals("0") AndAlso _
                    xml.DocumentElement.GetAttribute("PO_Permiso").Equals("1") Then
                    retorno = True
                Else
                    retorno = False
                End If

            Catch err As Exception
                xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + XmlSession.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "IsPermisoUserTransOpcion", xml.OuterXml)
            End Try
            Return retorno
        End Function

        Public Function getKey() As String
            'solo si el sitio es autorizado, devuelve la key
            Dim key As String = ""
            'se valida licencia de entrada
            If objBase.objLicencia Then '.plicencia Then
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
                key = key1.ToString()
            End If
            Return key
        End Function
        Public Function ConsParamAplicacion(ByVal PI_docXML As XmlDocument) As XmlDocument
            Dim elem As XmlElement
            Dim parametros As New XmlDocument
            parametros.LoadXml("<Parametros />")
            Try
                clsError.setMetodo(True, "clSeguridad", "ConsParamAplicacion")

                elem = PI_docXML.DocumentElement
                'Pregunta si el FrameWork tiene licencia
                If objBase.objLicencia Then '.plicencia Then

                    'Datos del usuario de sitio
                    objBase.SetUsuario(elem)

                    'Datos del stored procedure
                    Organizacion = 1 'Seguridad
                    Transaccion = 216 'ConsParamAplicacion

                    ReDim arrParam(1)
                    arrParam(0) = elem.GetAttribute("IdAplicacion")

                    parametros = objBase.Exec_SP_Reader(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                    'procesa los parametros para desencritarlos si campo Encriptado es 1
                    If parametros.DocumentElement.GetAttribute("CodError").Equals("0") Then
                        Dim objenc As New clsEncripta
                        For Each elemParam As XmlElement In parametros.DocumentElement.ChildNodes
                            If elemParam.GetAttribute("Encriptado").ToLower.Equals("true") Then 'esta encriptado, hay que desencriptarlo
                                parametros.DocumentElement.SetAttribute(elemParam.GetAttribute("Parametro"), objenc.Decrypt(elemParam.GetAttribute("Valor"), ""))
                            Else
                                parametros.DocumentElement.SetAttribute(elemParam.GetAttribute("Parametro"), elemParam.GetAttribute("Valor"))
                            End If
                        Next
                        parametros.DocumentElement.InnerXml = ""
                    End If
                Else
                    parametros.DocumentElement.SetAttribute("CodError", "-1")
                    parametros.DocumentElement.SetAttribute("MsgError", "El servidor no tiene licencia valida")
                End If

            Catch err As Exception
                parametros = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + parametros.OuterXml)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "ConsParamAplicacion", parametros.OuterXml)
            End Try

            Return parametros

        End Function
        Public Function ConsSemillaParticipante(ByVal PI_docXML As XmlDocument) As XmlDocument
            Dim elem As XmlElement
            Dim parametros As New XmlDocument
            parametros.LoadXml("<Semilla />")
            Try
                clsError.setMetodo(True, "clSeguridad", "ConsSemillaParticipante")

                elem = PI_docXML.DocumentElement
                'Pregunta si el FrameWork tiene licencia
                If objBase.objLicencia Then '.plicencia Then

                    'Datos del usuario de sitio
                    objBase.SetUsuario(elem)

                    'Datos del stored procedure
                    Organizacion = 1 'Seguridad
                    Transaccion = 220 'getSemillaParticipante

                    ReDim arrParam(2)
                    If IsNumeric(elem.GetAttribute("IdParticipante")) Then
                        arrParam(0) = elem.GetAttribute("IdParticipante")
                    Else
                        arrParam(0) = 0 'Para que busque por IdUsuario
                    End If
                    arrParam(1) = elem.GetAttribute("IdUsuario")
                    arrParam(2) = elem.GetAttribute("FechaAct")

                    parametros = objBase.Exec_SP_ParamOut(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                    'procesa la semilla para desencriptarla
                    If parametros.DocumentElement.GetAttribute("CodError").Equals("0") Then
                        Dim objenc As New clsEncripta
                        If parametros.DocumentElement.GetAttribute("PO_Semilla").Length > 0 Then 'esta encriptado, hay que desencriptarlo
                            parametros.DocumentElement.SetAttribute("Semilla", objenc.Decrypt(parametros.DocumentElement.GetAttribute("PO_Semilla"), ""))
                        Else
                            parametros.DocumentElement.SetAttribute("Semilla", "")
                        End If
                    End If
                Else
                    parametros.DocumentElement.SetAttribute("CodError", "-1")
                    parametros.DocumentElement.SetAttribute("MsgError", "El servidor no tiene licencia valida")
                End If

            Catch err As Exception
                parametros = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + parametros.OuterXml)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsSeguridad", "ConsSemillaParticipante", parametros.OuterXml)
            End Try

            Return parametros

        End Function
        Public Function getUltimoToken(ByVal XmlSession As XmlDocument) As String
            Dim xml As New XmlDocument
            Dim retorno As String = ""
            Try
                'Registra inicio en trace	
                clsError.setMetodo(True, "clsSeguridad", "getUltimoToken")
                Dim elem As XmlElement = XmlSession.DocumentElement
                'Datos del usuario de sitio
                objBase.SetDefaultUsuario(elem)
                'Datos del stored procedure
                Organizacion = 1 'Seguridad
                Transaccion = 224
                ReDim arrParam(2)
                arrParam(0) = elem.GetAttribute("IdAplicacion")
                arrParam(1) = elem.GetAttribute("IdUsuario")
                arrParam(2) = elem.GetAttribute("Maquina")

                xml = objBase.Exec_SP_ParamOut(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans)
                If xml.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    retorno = xml.DocumentElement.GetAttribute("Token")
                End If
            Catch err As Exception
                xml = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + XmlSession.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "getUltimoToken", xml.OuterXml)
            End Try
            Return retorno
        End Function
#End Region

#Region "Metodos de Ejecucion de Transacciones"
        Public Function EjecutaTransacciones(ByVal XmlSession As XmlDocument, ByVal XmlEntrada As XmlDocument) As XmlDocument
            Dim xmlSalida As New XmlDocument
            Dim elemTran, elem, elemParam As XmlElement
            Dim xmlTrans As XmlDocument
            Try
                'Registra si esta activo Trace
                clsError.setMetodo(True, "clSeguridad", "EjecutaTransacciones")
                xmlSalida.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Transacciones/>")
                CodError = 0
                elem = XmlSession.DocumentElement
                'Datos del usuario de sitio
                objBase.SetUsuario(elem)
                Dim nodoParam As XmlNode
                Dim xmlCDATA As XmlCDataSection
                Dim ValorParam As Object
                'se valida licencia de entrada
                If objBase.objLicencia Then '.plicencia Then
                    'Procesa el XmlEntrada 

                    For Each elemTran In XmlEntrada.DocumentElement.ChildNodes
                        'Por cada transaccion
                        'Si CodError es <> 0 , es porque un SP anterior dio error,
                        ' por lo tanto los demas no se deben ejecutar
                        Dim temp As String
                        If CodError = 0 Then
                            'Datos de la transaccion a ejecutar
                            Organizacion = elemTran.GetAttribute("IdOrganizacion")
                            Transaccion = elemTran.GetAttribute("IdTransaccion")
                            'Si se recibe IdOpcion, se setea, caso contrario se pone el default = 1
                            If IsNumeric(elemTran.GetAttribute("IdOpcion")) Then
                                Opcion = elemTran.GetAttribute("IdOpcion")
                            End If
                            'Pregunta cuantos parametros envia.
                            ReDim arrParam(elemTran.ChildNodes.Count - 1)
                            For Each elemParam In elemTran.ChildNodes
                                'Genera el ArrParam que trae el XML
                                'Pregunta si no tiene una seccion de CDATA que reemplace al valor
                                If elemParam.ChildNodes.Count = 1 Then
                                    nodoParam = elemParam.ChildNodes(0)
                                    If nodoParam.GetType.ToString() = "System.Xml.XmlCDataSection" Then
                                        xmlCDATA = nodoParam
                                        ValorParam = xmlCDATA.Value
                                    Else
                                        ValorParam = nodoParam.Value
                                    End If
                                Else 'Toma el valor del atributo
                                    If elemParam.GetAttribute("IsNull").Equals("S") Then
                                        ValorParam = System.DBNull.Value
                                    Else
                                        'Se pregunta si tiene un formato de fecha diferente
                                        If elemParam.GetAttribute("Culture").Length > 0 Then
                                            Dim Format As System.IFormatProvider = New System.Globalization.CultureInfo(elemParam.GetAttribute("Culture"), True)
                                            ValorParam = DateTime.Parse(elemParam.GetAttribute("Valor"), Format)
                                        Else
                                            ValorParam = elemParam.GetAttribute("Valor")
                                        End If
                                    End If
                                End If
                                arrParam(elemParam.GetAttribute("Pos")) = ValorParam
                            Next
                            'ejecuta la transacion
                            xmlTrans = objBase.Exec_SP_ReaderBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
                            xmlTrans.DocumentElement.SetAttribute("IdOrganizacion", elemTran.GetAttribute("IdOrganizacion"))
                            xmlTrans.DocumentElement.SetAttribute("IdTransaccion", elemTran.GetAttribute("IdTransaccion"))
                            temp = xmlTrans.OuterXml.Replace("<?xml version=""1.0"" encoding=""iso-8859-1""?>", "")
                            xmlSalida.DocumentElement.InnerXml = xmlSalida.DocumentElement.InnerXml + temp
                            CodError = Integer.Parse(xmlTrans.DocumentElement.GetAttribute("CodError"))
                            MsgError = xmlTrans.DocumentElement.GetAttribute("MsgError")
                        End If
                    Next
                End If
            Catch err As Exception
                xmlSalida = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + XmlEntrada.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "ejecutaTransacciones", XmlEntrada.OuterXml)
            End Try
            xmlSalida.DocumentElement.SetAttribute("CodError", CodError)
            xmlSalida.DocumentElement.SetAttribute("MsgError", MsgError)
            Return xmlSalida
        End Function
        Public Function EjecutaTransaccion(ByVal XmlSession As XmlDocument, ByVal XmlEntrada As XmlDocument) As XmlDocument
            Dim elemTran, elem, elemParam As XmlElement
            Dim xmlTrans As New XmlDocument
            Try
                'Registra si esta activo Trace
                clsError.setMetodo(True, "clSeguridad", "EjecutaTransaccion")
                elem = XmlSession.DocumentElement
                'Datos del usuario de sitio
                objBase.SetUsuario(elem)
                'se valida licencia de entrada
                If objBase.objLicencia Then '.plicencia Then
                    'Datos de la transaccion a ejecutar
                    elemTran = XmlEntrada.DocumentElement
                    Organizacion = elemTran.GetAttribute("IdOrganizacion")
                    Transaccion = elemTran.GetAttribute("IdTransaccion")
                    'Si se recibe IdOpcion, se setea, caso contrario se pone el default = 1
                    If IsNumeric(elemTran.GetAttribute("IdOpcion")) Then
                        Opcion = elemTran.GetAttribute("IdOpcion")
                    End If
                    Dim nodoParam As XmlNode
                    Dim xmlCDATA As XmlCDataSection
                    Dim ValorParam As Object
                    'Pregunta cuantos parametros envia.
                    ReDim arrParam(elemTran.ChildNodes.Count - 1)
                    For Each elemParam In elemTran.ChildNodes
                        'Genera el ArrParam que trae el XML
                        'Pregunta si no tiene una seccion de CDATA que reemplace al valor
                        If elemParam.ChildNodes.Count = 1 Then
                            nodoParam = elemParam.ChildNodes(0)
                            If nodoParam.GetType.ToString() = "System.Xml.XmlCDataSection" Then
                                xmlCDATA = nodoParam
                                ValorParam = xmlCDATA.Value
                            Else
                                ValorParam = nodoParam.Value
                            End If
                        Else 'Toma el valor del atributo
                            'Se aumenta un parametro, para saber si el valor es nulo
                            If elemParam.GetAttribute("IsNull").Equals("S") Then
                                ValorParam = System.DBNull.Value
                            Else
                                'Se pregunta si tiene un formato de fecha diferente
                                If elemParam.GetAttribute("Culture").Length > 0 Then
                                    Dim Format As System.IFormatProvider = New System.Globalization.CultureInfo(elemParam.GetAttribute("Culture"), True)
                                    ValorParam = DateTime.Parse(elemParam.GetAttribute("Valor"), Format)
                                Else
                                    ValorParam = elemParam.GetAttribute("Valor")
                                End If
                            End If
                        End If
                        arrParam(elemParam.GetAttribute("Pos")) = ValorParam
                    Next
                    'ejecuta la transacion remotamente
                    xmlTrans = objBase.Exec_SP_ReaderBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
                End If
            Catch err As Exception
                xmlTrans = clsError.setMensajeXml(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + XmlEntrada.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "ejecutaTransaccion", XmlEntrada.OuterXml)
            End Try
            Return xmlTrans
        End Function
        Public Function EjecutaTransaccionDS(ByVal XmlSession As XmlDocument, ByVal XmlEntrada As XmlDocument) As DataSet
            Dim elemTran, elem, elemParam As XmlElement
            Dim dsResul As New DataSet
            Try
                'Registra si esta activo Trace
                clsError.setMetodo(True, "clSeguridad", "EjecutaTransaccionDS")
                elem = XmlSession.DocumentElement
                'Datos del usuario de sitio
                objBase.SetUsuario(elem)
                'se valida licencia de entrada
                If objBase.objLicencia Then '.plicencia Then
                    'Datos de la transaccion a ejecutar
                    elemTran = XmlEntrada.DocumentElement
                    Organizacion = elemTran.GetAttribute("IdOrganizacion")
                    Transaccion = elemTran.GetAttribute("IdTransaccion")
                    'Si se recibe IdOpcion, se setea, caso contrario se pone el default = 1
                    If IsNumeric(elemTran.GetAttribute("IdOpcion")) Then
                        Opcion = elemTran.GetAttribute("IdOpcion")
                    End If
                    Dim nodoParam As XmlNode
                    Dim xmlCDATA As XmlCDataSection
                    Dim ValorParam As Object
                    'Pregunta cuantos parametros envia.
                    ReDim arrParam(elemTran.ChildNodes.Count - 1)
                    For Each elemParam In elemTran.ChildNodes
                        'Genera el ArrParam que trae el XML
                        'Pregunta si no tiene una seccion de CDATA que reemplace al valor
                        If elemParam.ChildNodes.Count = 1 Then
                            nodoParam = elemParam.ChildNodes(0)
                            If nodoParam.GetType.ToString() = "System.Xml.XmlCDataSection" Then
                                xmlCDATA = nodoParam
                                ValorParam = xmlCDATA.Value
                            Else
                                ValorParam = nodoParam.Value
                            End If
                        Else 'Toma el valor del atributo
                            If elemParam.GetAttribute("IsNull").Equals("S") Then
                                ValorParam = System.DBNull.Value
                            Else
                                'Se pregunta si tiene un formato de fecha diferente
                                If elemParam.GetAttribute("Culture").Length > 0 Then
                                    Dim Format As System.IFormatProvider = New System.Globalization.CultureInfo(elemParam.GetAttribute("Culture"), True)
                                    ValorParam = DateTime.Parse(elemParam.GetAttribute("Valor"), Format)
                                Else
                                    ValorParam = elemParam.GetAttribute("Valor")
                                End If
                            End If
                        End If
                        arrParam(elemParam.GetAttribute("Pos")) = ValorParam

                    Next
                    'ejecuta la transacion
                    dsResul = objBase.Exec_SPBA(Organizacion, Transaccion, Opcion, arrParam, ParamAut, ValorAut, txtTrans, -1)
                End If
            Catch err As Exception
                dsResul = clsError.setMensajeDS(getClase, System.Reflection.MethodBase.GetCurrentMethod.Name, "-101", err.Message + " " + XmlEntrada.OuterXml)
            Finally
                'Registra fin en trace
                clsError.setMetodo(False, "clsSeguridad", "ejecutaTransaccionDS", XmlEntrada.OuterXml)
            End Try
            Return dsResul
        End Function
#End Region

#Region "Metodos de Comandos"
        Public Function ExecCmdWindows(ByVal XmlSession As XmlDocument) As XmlDocument
            Dim xml As New XmlDocument()
            Dim stdOutput As String = ""
            Dim Comando As New Text.StringBuilder
            Dim ClaveBD As String = ""
            Dim UsuarioBD As String = ""
            Dim InstanciaBD As String = ""
            'Dim Root As String = ""
            Dim ArchLog As String
            Dim ArchCmd As String
            Dim TimeOut As Integer = 180000 ' default 3 minutos
            CodError = 0
            MsgError = ""
            Dim Ruta, RutaCompartida, Servidor As String
            Try
                ArchCmd = System.Configuration.ConfigurationManager.AppSettings("RutaBatMT")
                If ArchCmd = "" Then ArchCmd = Environment.CurrentDirectory
                ArchCmd = ArchCmd + "\Comando.bat"
                'ArchLog = System.Web.HttpContext.Current.Application("MapPath") + "\LogCmd" + Now.ToString("hhmmss") + ".txt"
                'Recuperar parametros de la base de datos  
                xml = Me.ConsParamAplicacion(XmlSession)
                If xml.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    InstanciaBD = xml.DocumentElement.GetAttribute("Servidor")
                    UsuarioBD = xml.DocumentElement.GetAttribute("Usuario")
                    ClaveBD = xml.DocumentElement.GetAttribute("Clave")
                Else
                    Throw New Exception("ExecCmdWindows:ConsParamAplicacion:" + xml.DocumentElement.GetAttribute("MsgError"))
                End If
                'Recuperar parametros del servidor donde esta el archivo comando  
                XmlSession.DocumentElement.SetAttribute("IdAplicacion", XmlSession.DocumentElement.GetAttribute("ServidorArch"))
                xml = Me.ConsParamAplicacion(XmlSession)
                If xml.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    Servidor = xml.DocumentElement.GetAttribute("Servidor")
                    Ruta = xml.DocumentElement.GetAttribute("Ruta")
                    RutaCompartida = xml.DocumentElement.GetAttribute("RecursoCompartido")
                    If xml.DocumentElement.GetAttribute("TimeOut").Length > 0 AndAlso IsNumeric(xml.DocumentElement.GetAttribute("TimeOut")) Then
                        Try 'TimeOut en segundos se recupera 
                            TimeOut = Integer.Parse(xml.DocumentElement.GetAttribute("TimeOut")) * 1000
                        Catch ex As Exception
                            TimeOut = 180000 'default 3 minutos
                        End Try
                    End If
                Else
                    Throw New Exception("ExecCmdWindows:ConsParamAplicacion:" + xml.DocumentElement.GetAttribute("MsgError"))
                End If
                ArchLog = RutaCompartida + "\" + XmlSession.DocumentElement.GetAttribute("Comando") + "_" + Date.Now.Ticks.ToString + "_" + TimeOut.ToString() + ".txt"

                'ServidorArch contiene el IdServior donde esta el comando a ejecutar
                Comando.Append(" ").Append(Servidor)
                Comando.Append(" ").Append(Ruta + "\" + XmlSession.DocumentElement.GetAttribute("Comando"))
                Comando.Append(" ").Append(InstanciaBD)
                Comando.Append(" ").Append(UsuarioBD)
                Comando.Append(" ").Append(ClaveBD)
                Comando.Append(" > ").Append(ArchLog)
                Dim info1 As ProcessStartInfo = New ProcessStartInfo()
                Dim cmd As Process = New Process()
                cmd.StartInfo.FileName = ArchCmd
                cmd.StartInfo.Arguments = Comando.ToString()
                cmd.StartInfo.UseShellExecute = False
                cmd.StartInfo.CreateNoWindow = True
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                'cmd.StartInfo.RedirectStandardOutput = True
                'cmd.StartInfo.RedirectStandardError = True
                cmd.Start()
                cmd.WaitForExit(TimeOut) 'Espera maximo 30 minutos
                CodError = cmd.ExitCode
                'Recupera el archivo de log
                Dim ArchTmp As System.IO.TextReader
                ArchTmp = System.IO.File.OpenText(ArchLog)
                stdOutput = ArchTmp.ReadToEnd()
                'Oculta usuario y clave pasado como parametros.
                stdOutput = stdOutput.Replace(UsuarioBD, "****")
                stdOutput = stdOutput.Replace(ClaveBD, "****")

                ArchTmp.Close()
                ' IO.File.Delete(ArchLog) 'elimino el arch de log temporal creado
            Catch Ex As Exception
                CodError = -1000
                MsgError = Ex.Message
                stdOutput = Ex.Message
            Finally
                stdOutput = CambiaCarControlXML(stdOutput)
                MsgError = CambiaCarControlXML(MsgError)
            End Try
            xml.LoadXml("<Comando />")
            xml.DocumentElement.SetAttribute("CodError", CodError)
            xml.DocumentElement.SetAttribute("MsgError", MsgError)
            xml.DocumentElement.SetAttribute("OutPut", stdOutput)
            'Dim _Cdata As XmlCDataSection
            '_Cdata = xml.CreateCDataSection(stdOutput)
            'xml.DocumentElement.AppendChild(_Cdata)
            'xml.DocumentElement.SetAttribute("Comando", Comando.ToString())
            Return xml
        End Function
#End Region
    End Class
End Namespace
