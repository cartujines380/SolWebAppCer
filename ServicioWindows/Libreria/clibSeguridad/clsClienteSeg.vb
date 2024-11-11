Imports System.Xml
Imports clibSeguridadCR

Public Class clsClienteSeg


    Public IdOrganizacion As Integer
    Public IdTransaccion As Integer
    Public IdOpcion As Integer
    Public cadenaBD
    Public semilla
    Public session

    'Propiedades de Retornos
    Protected pCodError As Integer
    Protected pMsgError As String

    Public Property pCadenaBD() As String
        Get
            Return Me.cadenaBD
        End Get
        Set(ByVal value As String)
            Me.cadenaBD = value
        End Set
    End Property

    Public Property pSemilla() As String
        Get
            Return Me.semilla
        End Get
        Set(ByVal value As String)
            Me.semilla = value
        End Set
    End Property

    Public Property pSession() As String
        Get
            Return Me.session
        End Get
        Set(ByVal value As String)
            Me.session = value
        End Set
    End Property

    Private ReadOnly Property getClase() As String
        Get
            Return System.Reflection.MethodBase.GetCurrentMethod.DeclaringType.FullName
        End Get
    End Property

    Public Function GrabaUsuarioAdministrador(PI_XmlDoc As String) As XmlDocument
        Dim elem As XmlElement
        Dim xmlResul As New XmlDocument
        Dim xmlDoc As New XmlDocument
        Dim respXML As New XmlDocument
        Try

            xmlDoc.LoadXml(PI_XmlDoc)

            Dim xEleUsr As XmlElement = xmlDoc.DocumentElement.FirstChild
            If (xEleUsr.Name = "New") Then

                ' -->> INGRESO EN ESTRUCTURA PROVEEDORES
                'Organizacion = 39 - Transaccion = 6 - Opcion = 1
                respXML = Proc_GrabaUsrAdmin(xmlDoc)

                If respXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
                    xEleUsr.SetAttribute("Usuario", respXML.DocumentElement.GetAttribute("PO_Usuario"))
                    xEleUsr.SetAttribute("IdUsuario", xEleUsr.GetAttribute("Ruc").Substring(0, 10) & respXML.DocumentElement.GetAttribute("PO_Usuario"))

                    ' armado de estructuras
                    Dim xmlPar As XmlDocument = getXmlIngParticipanteProvAdmin(xEleUsr)

                    ' -->> INGRESO EN FRAMEWORK SEGURIDAD
                    'Organizacion = 2 - Transaccion = 41

                    respXML = Proc_IngresaParticipante(xmlPar.OuterXml, "<Participante />")

                    If respXML.DocumentElement.GetAttribute("CodError").Equals("0") Then
                        xEleUsr.SetAttribute("IdParticipante", respXML.DocumentElement.GetAttribute("PO_IdParticipante"))

                        respXML.DocumentElement.SetAttribute("IdParticipante", xEleUsr.GetAttribute("IdParticipante"))
                        respXML.DocumentElement.SetAttribute("Usuario", xEleUsr.GetAttribute("Usuario"))
                        xmlResul = respXML

                        ' -->> ACTUALIZO ROLES EN FRAMEWORK SEGURIDAD
                        Dim xmlRoles As XmlDocument = getXmlActualizarRoles(xEleUsr)
                        ActualizaRolesUsuarioFS(xmlRoles)
                    Else
                        xmlResul = respXML
                        EliminaUsuarioAdministradorNuevo(xEleUsr)
                    End If
                Else
                    xmlResul = respXML
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
        xElePar.SetAttribute("Usuario", xEleUsr.GetAttribute("Usuario"))

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


#Region "propiedades publicas"
    Public objBase As New Seguridad.clsBaseDatosSQL
    Public Property txtTrans() As String
        Get
            Return Me.objBase.ptxtTrans
        End Get
        Set(ByVal Value As String)
            Me.objBase.ptxtTrans = Value
        End Set
    End Property
    Public Property CodError() As Integer
        Get
            Return Me.objBase.CodError
        End Get
        Set(ByVal Value As Integer)
            Me.objBase.CodError = Value
        End Set
    End Property
    Public Property MsgError() As String
        Get
            Return Me.objBase.MsgError
        End Get
        Set(ByVal Value As String)
            objBase.MsgError = Value
        End Set
    End Property
#End Region


#Region "Propiedades heredadas"
    Protected arrParam(1) As Object
    Protected ParamAut As String = ""
    Protected ValorAut As String = ""
    Protected Property Organizacion() As Integer
        Get
            Return objBase.pOrganizacion
        End Get
        Set(ByVal value As Integer)
            objBase.pOrganizacion = value
        End Set
    End Property
    Protected Property Transaccion() As Integer
        Get
            Return objBase.pTransaccion
        End Get
        Set(ByVal value As Integer)
            objBase.pTransaccion = value
        End Set
    End Property
    Protected Property Opcion() As Integer
        Get
            Return objBase.pOpcion
        End Get
        Set(ByVal value As Integer)
            objBase.pOpcion = value
        End Set
    End Property
#End Region


    Private Sub ActualizaRolesUsuarioFS(PI_docXML As XmlDocument)
        Try
            Organizacion = 1 'Seguridad
            Transaccion = 147
            Opcion = 1
            ReDim arrParam(0)
            Me.arrParam(0) = PI_docXML.OuterXml
            Dim Proceso As New ProcesoWs.ServBaseProceso()
            Dim ds = New DataSet()

            Proceso.Url = Configuration.ConfigurationManager.AppSettings("UrlBase")
            ds = Proceso.DatosBase(pSemilla, Organizacion, Transaccion, Opcion, arrParam, pSession)

        Catch ex As Exception
        End Try
    End Sub


    Private Sub EliminaUsuarioAdministradorNuevo(PI_XmlEleUsr As XmlElement)
        Try
            Dim xmlDocEli As New XmlDocument
            xmlDocEli.LoadXml("<Root />")
            Dim xEleEli As XmlElement = xmlDocEli.CreateElement("Eli")
            xEleEli.SetAttribute("IdEmpresa", PI_XmlEleUsr.GetAttribute("IdEmpresa"))
            xEleEli.SetAttribute("Ruc", PI_XmlEleUsr.GetAttribute("Ruc"))
            xEleEli.SetAttribute("Usuario", PI_XmlEleUsr.GetAttribute("Usuario"))
            xmlDocEli.DocumentElement.AppendChild(xEleEli)

            Proc_GrabaUsrAdmin(xmlDocEli)

            ' -->> ELIMINO EN ESTRUCTURA PROVEEDORES
            'Organizacion = 39
            'Transaccion = 6
            'Opcion = 1


        Catch ex As Exception
        End Try
    End Sub

    Private Sub EliminaParticipanteFS(PI_IdParticipante As String)
        Try


            Dim op As New PP.AccesoDatos.OperadorBaseDatos(cadenaBD)
            op.ProcedimientoAlmacenado = "[Participante].[Par_P_eliParticipante]"
            op.AgregarParametro("@PI_IdParticipante", SqlDbType.Int, PI_IdParticipante)
            Dim dt = op.ConsultarDataTable()

            'Organizacion = 2 'Participante
            'Transaccion = 43

        Catch ex As Exception

        End Try
    End Sub







    ''' <summary>
    ''' [Seguridad].[Seg_P_GrabaUsrAdmin]
    ''' Organizacion = 39
    ''' Transaccion = 6
    ''' </summary>
    Public Function Proc_GrabaUsrAdmin(PI_XmlEleUsr As XmlDocument)

        Me.pCodError = 0
        Me.pMsgError = ""
        Dim xml As New XmlDocument
        Dim salida As String = ""
        xml = New XmlDocument()

        Try

            Dim op As New PP.AccesoDatos.OperadorBaseDatos(cadenaBD)
            op.ProcedimientoAlmacenado = "[Seguridad].[Seg_P_GrabaUsrAdmin]"
            op.AgregarParametro("@PI_DocXML", SqlDbType.Xml, PI_XmlEleUsr.OuterXml)
            op.AgregarParametroDeSalida("@PO_Usuario", SqlDbType.NVarChar)

            op.Ejecutar()

            salida = op.LeerParametroDeSalida("@PO_Usuario")

            'salida = op.AgregarParametroDeSalida("@PO_Usuario", SqlDbType.VarChar)
        Catch ex As Exception
            pCodError = -100
            pMsgError = ex.Message
        End Try
        xml.LoadXml("<Root />")


        xml.DocumentElement.SetAttribute("CodError", Me.pCodError.ToString())
        xml.DocumentElement.SetAttribute("MsgError", Me.pMsgError)
        xml.DocumentElement.SetAttribute("PO_Usuario", salida)

        Return xml


    End Function


    ''' <summary>
    ''' Participante.Par_P_ingParticipante
    ''' Organizacion = 2
    ''' Transaccion = 41
    ''' </summary>
    ''' 
    Public Function Proc_IngresaParticipante(PI_docXML As String, PI_docXMLcep As String)

        Me.pCodError = 0
        Me.pMsgError = ""
        Dim xml As New XmlDocument
        xml = New XmlDocument()

        Dim Proceso As New ProcesoWs.ServBaseProceso()
        Dim ds = New DataSet()

        Try
            Organizacion = 39 'Participante
            Transaccion = 4404
            ReDim arrParam(1)
            Me.arrParam(0) = PI_docXML
            Me.arrParam(1) = PI_docXMLcep
            Proceso.Url = Configuration.ConfigurationManager.AppSettings("UrlBase")
            ds = Proceso.DatosBase(pSemilla, Organizacion, Transaccion, Opcion, arrParam, pSession)

        Catch ex As Exception
            pCodError = -100
            pMsgError = ex.Message
        End Try

        If (ds IsNot Nothing) Then
            If ds.Tables.Count > 1 Then
                Dim item0 = ds.Tables(0)
                Dim item1 = ds.Tables(1)
                xml.LoadXml("<Root />")
                xml.DocumentElement.SetAttribute("CodError", item1.Rows(0)("CodError").ToString())
                xml.DocumentElement.SetAttribute("MsgError", item1.Rows(0)("MsgError").ToString())

                xml.DocumentElement.SetAttribute("PO_IdParticipante", item0.Rows(0)("IdParticipante").ToString())

            Else

                Dim item0 = ds.Tables(0)

                xml.LoadXml("<Root />")
                xml.DocumentElement.SetAttribute("CodError", item0.Rows(0)("CodError").ToString())
                xml.DocumentElement.SetAttribute("MsgError", item0.Rows(0)("MsgError").ToString())


            End If



        End If


        Return xml


    End Function



End Class
