Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Web

Namespace Seguridad
    Public Class clsBaseDatosSQL

#Region "Propiedades Protegidas"
        'propiedades de Sql
        Private pcnn As SqlConnection
        Private ptrans As SqlTransaction
        Private cmd As SqlCommand
        Private strcnn As String
        Private pcnnSeg As SqlConnection

        Private _Semilla As String

        'Clases de Objetos SYBASE,SQLSERVER,ORACLE EXTERNOS
        'Private objSybase As clsBaseDatosSybase
        'Private objOtro As clsBaseDatosOLEDB
        
        'Propuedades de Conexion a Base de Datos
        '  Private usuarioBD As String 'Encriptado
        ' Private claveBD As String   'Encriptado
        Protected servidorBD, BaseSeguridad As String
        Protected proveedorBD As String
        Protected UsuarioBD, ClaveBD As String
        Protected MinPool, MaxPool, ConnectionTimeOut As String
        Protected AppBD, BaseDatos As String

        'Propuedades de Conexion a Base de Datos
        ' para aplicaciones externas al framework
        Protected servidorBD_Ext, TipoServidorBD_Ext As String
        Protected proveedorBD_Ext As String
        Protected UsuarioBD_Ext, ClaveBD_Ext As String
        Protected MinPool_Ext, MaxPool_Ext, ConnectionTimeOut_Ext As String
        Protected AppBD_Ext, PuertoBD_Ext, BaseDatos_Ext, Semilla As String
        Protected IdServidor_Ext As Integer 'IdAplicacion = IdServidor donde se ejecuta la transaccion

        'Propiedades de Session del Usuario
        Protected usuario As String
        Protected token As String
        Protected maquina As String
        Protected empresa As Integer
        Protected nombreEmpresa As String
        Protected sucursal As Integer
        Protected nombreSucursal As String

        'Propiedades de Session del Usuario de Sitio
        Protected UsrSitio As String
        Protected TokenSitio As String
        Protected MaqSitio As String

        'Propiedades de Retornos
        Protected pCodError As Integer
        Protected pMsgError As String

        'Propiedad de Transaccion
        Protected OpcionTrans As String
        Protected aplicacion As Integer
        Protected Organizacion As Integer
        Protected Transaccion As Integer
        Protected Opcion As Integer = 1
        Protected arrParam(1) As Object
        Protected ParamAut As String = ""
        Protected ValorAut As String = ""
        Protected txtTrans As String = ""
#End Region
#Region "Propiedades de Monitoreo"
        Private XmlEntrada As XmlDocument
        Private XmlSalida As XmlDocument
        Private TraceTrans As String = "N" 'Si tiene trace para transaccion
#End Region
#Region "Propiedades Publicas"

        Public Property pEmpresa() As Integer
            Get
                Return Me.empresa
            End Get
            Set(ByVal value As Integer)
                Me.empresa = value
            End Set
        End Property
        Public Property pNombreEmpresa() As String
            Get
                Return Me.nombreEmpresa
            End Get
            Set(ByVal value As String)
                Me.nombreEmpresa = value
            End Set
        End Property
        Public Property pSucursal() As Integer
            Get
                Return Me.sucursal
            End Get
            Set(ByVal value As Integer)
                Me.sucursal = value
            End Set
        End Property
        Public Property pnombreSucursal() As String
            Get
                Return Me.nombreSucursal
            End Get
            Set(ByVal value As String)
                Me.nombreSucursal = value
            End Set
        End Property
        Public Property pAplicacion() As Integer
            Get
                Return Me.aplicacion
            End Get
            Set(ByVal value As Integer)
                Me.aplicacion = value
            End Set
        End Property
        Public Property pUsuario() As String
            Get
                Return Me.usuario
            End Get
            Set(ByVal value As String)
                Me.usuario = value
            End Set
        End Property
        Public Property pToken() As String
            Get
                Return Me.token
            End Get
            Set(ByVal value As String)
                Me.token = value
            End Set
        End Property
        Public Property pMaquina() As String
            Get
                Return Me.maquina
            End Get
            Set(ByVal value As String)
                Me.maquina = value
            End Set
        End Property
        Public Property pUsrSitio() As String
            Get
                Return Me.UsrSitio
            End Get
            Set(ByVal value As String)
                Me.UsrSitio = value
            End Set
        End Property
        Public Property pTokenSitio() As String
            Get
                Return Me.TokenSitio
            End Get
            Set(ByVal value As String)
                Me.TokenSitio = value
            End Set
        End Property
        Public Property pMaqSitio() As String
            Get
                Return Me.MaqSitio
            End Get
            Set(ByVal value As String)
                Me.MaqSitio = value
            End Set
        End Property
        Friend Property pUsuarioBD() As String
            Get
                Return Me.UsuarioBD
            End Get
            Set(ByVal value As String)
                Me.UsuarioBD = value
            End Set
        End Property
        Friend Property pServidorBD() As String
            Get
                Return Me.servidorBD
            End Get
            Set(ByVal value As String)
                Me.servidorBD = value
            End Set
        End Property
        Friend Property pProveedorBD() As String
            Get
                Return Me.proveedorBD
            End Get
            Set(ByVal value As String)
                Me.proveedorBD = value
            End Set
        End Property
        Friend Property pClaveBD() As String
            Get
                Return Me.ClaveBD
            End Get
            Set(ByVal value As String)
                Me.ClaveBD = value
            End Set
        End Property
        Friend Property pMinPool() As String
            Get
                Return Me.MinPool
            End Get
            Set(ByVal value As String)
                Me.MinPool = value
            End Set
        End Property
        Friend Property pMaxPool() As String
            Get
                Return Me.MaxPool
            End Get
            Set(ByVal value As String)
                Me.MaxPool = value
            End Set
        End Property
        Friend Property pAppBD() As String
            Get
                Return Me.AppBD
            End Get
            Set(ByVal value As String)
                Me.AppBD = value
            End Set
        End Property

        Public Property ptxtTrans() As String
            Get
                Return Me.txtTrans
            End Get
            Set(ByVal value As String)
                Me.txtTrans = value
            End Set
        End Property
        Public Property Cnn() As SqlConnection
            Get
                Return Me.pcnn
            End Get
            Set(ByVal value As SqlConnection)
                Me.pcnn = value
            End Set
        End Property
        Public Property Trans() As SqlTransaction
            Get
                Return Me.ptrans
            End Get
            Set(ByVal value As SqlTransaction)
                Me.ptrans = value
            End Set
        End Property
        Public Property CodError() As Integer
            Get
                Return Me.pCodError
            End Get
            Set(ByVal Value As Integer)
                pCodError = Value
            End Set
        End Property
        Public Property MsgError() As String
            Get
                Return Me.pMsgError
            End Get
            Set(ByVal Value As String)
                pMsgError = Value
            End Set
        End Property
        Public Property pOpcionTrans() As String
            Get
                Return Me.OpcionTrans
            End Get
            Set(ByVal value As String)
                Me.OpcionTrans = value
            End Set
        End Property
        Friend Property pBaseDatos() As String
            Get
                Return Me.BaseDatos
            End Get
            Set(ByVal value As String)
                BaseDatos = value
            End Set
        End Property
        Public Property pTransaccion() As Integer
            Get
                Return Me.Transaccion
            End Get
            Set(ByVal value As Integer)
                Transaccion = value
            End Set
        End Property
        Public Property pOrganizacion() As Integer
            Get
                Return Me.Organizacion
            End Get
            Set(ByVal value As Integer)
                Organizacion = value
            End Set
        End Property
        Public Property pOpcion() As Integer
            Get
                Return Me.Opcion
            End Get
            Set(ByVal value As Integer)
                Opcion = value
            End Set
        End Property
        Friend Property pConnectionTimeOut() As String
            Get
                Return Me.ConnectionTimeOut
            End Get
            Set(ByVal value As String)
                ConnectionTimeOut = value
            End Set
        End Property
#End Region
#Region "Propiedades para aplicaciones Externas"
        Friend Property pIdServidor_Ext() As Integer
            Get
                Return Me.IdServidor_Ext
            End Get
            Set(ByVal value As Integer)
                Me.IdServidor_Ext = value
            End Set
        End Property
        Friend Property pUsuarioBD_Ext() As String
            Get
                Return Me.UsuarioBD_Ext
            End Get
            Set(ByVal value As String)
                Me.UsuarioBD_Ext = value
            End Set
        End Property
        Friend Property pServidorBD_Ext() As String
            Get
                Return Me.servidorBD_Ext
            End Get
            Set(ByVal value As String)
                Me.servidorBD_Ext = value
            End Set
        End Property
        Friend Property pTipoServidorBD_Ext() As String
            Get
                Return Me.TipoServidorBD_Ext
            End Get
            Set(ByVal value As String)
                Me.TipoServidorBD_Ext = value
            End Set
        End Property
        Friend Property pProveedorBD_Ext() As String
            Get
                Return Me.proveedorBD_Ext
            End Get
            Set(ByVal value As String)
                Me.proveedorBD_Ext = value
            End Set
        End Property
        Friend Property pClaveBD_Ext() As String
            Get
                Return Me.ClaveBD_Ext
            End Get
            Set(ByVal value As String)
                Me.ClaveBD_Ext = value
            End Set
        End Property
        Friend Property pMinPool_Ext() As String
            Get
                Return Me.MinPool_Ext
            End Get
            Set(ByVal value As String)
                Me.MinPool_Ext = value
            End Set
        End Property
        Friend Property pMaxPool_Ext() As String
            Get
                Return MaxPool_Ext
            End Get
            Set(ByVal value As String)
                MaxPool_Ext = value
            End Set
        End Property
        Friend Property pAppBD_Ext() As String
            Get
                Return Me.AppBD_Ext
            End Get
            Set(ByVal value As String)
                AppBD_Ext = value
            End Set
        End Property
        Friend Property pPuertoBD_Ext() As String
            Get
                Return Me.PuertoBD_Ext
            End Get
            Set(ByVal value As String)
                PuertoBD_Ext = value
            End Set
        End Property
        Friend Property pBaseDatos_Ext() As String
            Get
                Return Me.BaseDatos_Ext
            End Get
            Set(ByVal value As String)
                BaseDatos_Ext = value
            End Set
        End Property
        Friend Property pConnectionTimeOut_Ext() As String
            Get
                Return Me.ConnectionTimeOut_Ext
            End Get
            Set(ByVal value As String)
                ConnectionTimeOut_Ext = value
            End Set
        End Property
#End Region
#Region "Propiedades de LICENCIA "
        'Propiedad de licencia del Sitio
        Public ReadOnly Property objLicencia() As Boolean 'clsLicencia
            Get
                Return True 'System.Web.HttpContext.Current.Application("objLicencia")
            End Get
        End Property

        'Propiedad de Licencia de la Empresa
        Protected licenciaEmpresa As Boolean
        'Propiedades de los modulos autorizados por empresa
        Protected modulo(30) As Integer
        Public ReadOnly Property plicenciaEmpresa() As Boolean
            Get
                Return licenciaEmpresa
            End Get
        End Property
        Public ReadOnly Property pmodulo() As Integer()
            Get
                Return modulo
            End Get
        End Property
#End Region
#Region "Constructor"
        Public Sub New()
            Try
                pOpcionTrans = "N"
                BaseSeguridad = System.Configuration.ConfigurationManager.AppSettings("BaseSeguridad")

                'setea parametros de base de datos
                setParamBase()

            Catch ex As Exception

            End Try
            
        End Sub
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

#End Region
#Region "Metodos para monitoreo"
        'Private Sub GeneraXmlEntrada(ByVal PI_Cmd As SqlCommand)
        '    Dim param As SqlParameter
        '    Dim elem As XmlElement
        '    XmlEntrada = New XmlDocument
        '    XmlEntrada.LoadXml("<SP />")
        '    XmlEntrada.DocumentElement.SetAttribute("Nombre", PI_Cmd.CommandText)

        '    For Each param In PI_Cmd.Parameters
        '        elem = XmlEntrada.CreateElement("Param")
        '        elem.SetAttribute("N", param.ParameterName)
        '        elem.SetAttribute("IO", param.Direction.ToString())
        '        elem.SetAttribute("T", param.DbType.ToString())
        '        If param.Direction = ParameterDirection.Input Then
        '            elem.SetAttribute("V", param.Value.ToString())
        '        Else
        '            elem.SetAttribute("V", "")
        '        End If
        '        XmlEntrada.DocumentElement.AppendChild(elem)
        '    Next
        'End Sub
        'Private Sub RegTraceTrans(ByVal Organizacion As Integer, _
        '               ByVal CodTran As Integer, _
        '               ByVal CodOpcion As Integer, _
        '               ByVal ParamAut As String, _
        '               ByVal ValorAut As String)

        '    Me.pCodError = 0
        '    Me.pMsgError = ""

        '    Try

        '        cmd = New SqlCommand

        '        Me.Cnn = New SqlConnection(LeeConexion())
        '        Me.Cnn.Open()

        '        cmd.CommandType = CommandType.StoredProcedure
        '        cmd.CommandText = "Sige_Trace..tra_p_RegTraceTrans"
        '        cmd.Connection = Cnn

        '        Dim param As SqlParameter

        '        param = New SqlParameter("@PI_UsrSitio", System.Data.SqlDbType.VarChar, 20)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = UsrSitio
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_TokenSitio", System.Data.SqlDbType.VarChar, 32)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = TokenSitio
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_MaqSitio", System.Data.SqlDbType.VarChar, 100)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = MaqSitio
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_IdUsuario", System.Data.SqlDbType.VarChar, 20)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = usuario
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_Token", System.Data.SqlDbType.VarChar, 32)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = token
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_Maquina", System.Data.SqlDbType.VarChar, 100)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = maquina
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_IdAplicacion", System.Data.SqlDbType.Int, 4)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = aplicacion
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_IdOrganizacion", System.Data.SqlDbType.Int, 4)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = Organizacion
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_IdTransaccion", System.Data.SqlDbType.Int, 4)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = CodTran
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_IdOpcion", System.Data.SqlDbType.Int, 4)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = CodOpcion
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_IdEmpresa", System.Data.SqlDbType.Int, 4)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = Me.empresa
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_IdSucursal", System.Data.SqlDbType.Int, 4)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = Me.sucursal
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_ParamAut", System.Data.SqlDbType.VarChar, 100)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = ParamAut
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_ValorAut", System.Data.SqlDbType.VarChar, 50)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = ValorAut
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_XmlEntrada", System.Data.SqlDbType.VarChar, XmlEntrada.OuterXml.Length)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = XmlEntrada.OuterXml
        '        cmd.Parameters.Add(param)

        '        param = New SqlParameter("@PI_XmlSalida", System.Data.SqlDbType.VarChar, XmlSalida.OuterXml.Length)
        '        param.Direction = System.Data.ParameterDirection.Input
        '        param.Value = XmlSalida.OuterXml
        '        cmd.Parameters.Add(param)

        '        cmd.ExecuteNonQuery()
        '        cmd.Dispose()
        '        Cnn.Close()

        '    Catch errSql As SqlException
        '        pCodError = errSql.Number
        '        pMsgError = "getRegTraceTrans: " + errSql.Message
        '        CierraCnn(Cnn)
        '    Catch err As Exception
        '        pCodError = -100
        '        pMsgError = "getRegTraceTrans: " + err.Message
        '        CierraCnn(Cnn)
        '    End Try


        'End Sub
#End Region
#Region "Metodos Privads"
        Private Function settxtTrans(ByVal arrParam As Array, ByVal txtTrans As String) As String
            Dim xmlDatos As New XmlDocument
            Dim xmlTemp As New XmlDocument
            Try
                xmlDatos.LoadXml("<T />")
                Dim elem As XmlElement
                Dim i As Integer = 0
                If txtTrans.Length = 0 Then
                    For Each item As Object In arrParam
                        Try
                            xmlTemp.LoadXml(item.ToString())
                            elem = xmlDatos.CreateElement("P" + i.ToString())
                            elem.InnerXml = xmlTemp.DocumentElement.OuterXml
                            xmlDatos.DocumentElement.AppendChild(elem)
                        Catch ex As Exception
                            xmlDatos.DocumentElement.SetAttribute("P" + i.ToString(), item.ToString())
                        End Try
                        i = i + 1
                    Next
                Else 'se deja lo que trae
                    Try
                        xmlTemp.LoadXml(txtTrans)
                        elem = xmlDatos.CreateElement("P1")
                        elem.InnerXml = xmlTemp.DocumentElement.OuterXml
                        xmlDatos.DocumentElement.AppendChild(elem)
                    Catch ex As Exception
                        xmlDatos.DocumentElement.SetAttribute("P1", txtTrans)
                    End Try
                End If

            Catch ex As Exception

            End Try
            Return xmlDatos.OuterXml
        End Function
        Private Function ArrayToString(ByVal arrInput() As Object) As String
            Dim i As Integer
            Dim sOutput As New Text.StringBuilder(arrInput.Length)
            For i = 0 To arrInput.Length - 1
                Try
                    sOutput.Append(arrInput(i).ToString()).Append(";")
                Catch ex As Exception
                    Exit For
                End Try
            Next
            Return sOutput.ToString()
        End Function
        Private Function LeeConexion() As String
            Dim txtconn As System.Text.StringBuilder = New System.Text.StringBuilder
            Dim _Dato As String = ""
            Dim USerBD As String
            Dim pwdBD As String = ""
            Try
                clsError.setMetodo(True, "clsBaseDatosSQL", "LeeConexion")

                If Me.UsuarioBD.Equals("") Then 'Relacion de confianza
                    txtconn.Append("integrated security=SSPI")
                Else 'con usuario aplicativo
                    Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
                    USerBD = Me.UsuarioBD
                    UserBD = objEncripta.Decrypt(UserBD)
                    pwdBD = Me.ClaveBD
                    pwdBD = objEncripta.Decrypt(pwdBD)
                    txtconn.Append("user id=").Append(UserBD)
                    txtconn.Append(";password=").Append(pwdBD)
                End If
                'If BaseSeguridad.Length > 0 Then
                'txtconn.Append(";Database=").Append(BaseSeguridad)
                'End If
                txtconn.Append(";Server=").Append(Me.servidorBD)
                txtconn.Append(";Initial Catalog=SIPE_FRAMEWORK")   'jcastro
                ' txtconn.Append(";provider=").Append(Me.proveedorBD)
                txtconn.Append(";connection reset=false")
                txtconn.Append(";connection lifetime=1")
                txtconn.Append(";min pool size=").Append(MinPool)
                txtconn.Append(";max pool size=").Append(MaxPool)
                txtconn.Append(";Application Name=").Append(AppBD)
                If ConnectionTimeOut.length > 0 Then
                    txtconn.Append(";Connect Timeout=").Append(ConnectionTimeOut)
                End If
                'Para auditar si viene con clave
                If pwdBD.Length > 0 Then
                    _Dato = txtconn.ToString().Replace(pwdBD, "********")
                Else
                    _Dato = txtconn.ToString()
                End If
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "LeeConexion", pCodError, pMsgError + " " + _Dato)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "LeeConexion", _Dato)
            End Try

            Return (txtconn.ToString())

        End Function
        Private Function LeeConexion(ByVal PI_Servidor As String, ByVal PI_Proveedor As String, _
                                     ByVal PI_Usuario As String, ByVal PI_Clave As String) As String
            Dim txtconn As System.Text.StringBuilder = New System.Text.StringBuilder
            Dim _Dato As String = ""
            Try

                Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
                PI_Usuario = objEncripta.Decrypt(PI_Usuario)
                PI_Clave = objEncripta.Decrypt(PI_Clave)

                clsError.setMetodo(True, "clsBaseDatosSQL", "LeeConexion")
                txtconn.Append("user id=").Append(PI_Usuario)
                txtconn.Append(";password=").Append(PI_Clave)

                'txtconn.Append(";Database=").Append("master")
                txtconn.Append(";Server=").Append(PI_Servidor)
                txtconn.Append(";Initial Catalog=SIPE_FRAMEWORK")   'jcastro
                ' txtconn.Append(";provider=").Append(PI_Proveedor)
                txtconn.Append(";connection reset=false")
                txtconn.Append(";connection lifetime=1")
                txtconn.Append(";min pool size=").Append(MinPool)
                txtconn.Append(";max pool size=").Append(MaxPool)
                txtconn.Append(";Application Name=").Append(AppBD)
                'Para auditar
                If PI_Clave.Length > 0 Then
                    _Dato = txtconn.ToString().Replace(PI_Clave, "********")
                Else
                    _Dato = txtconn.ToString()
                End If
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "LeeConexion", pCodError, pMsgError + " " + _Dato)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "LeeConexion", _Dato)
            End Try

            Return (txtconn.ToString())

        End Function
        Private Function LeeConexion_Ext(ByVal IdAplicacion As Integer) As String
            Dim txtconn As System.Text.StringBuilder = New System.Text.StringBuilder
            Dim _Dato As String = ""
            Try
                clsError.setMetodo(True, "clsBaseDatosSQL", "LeeConexion_Ext")

                Me.getConexionApl(IdAplicacion)
                If CodError = 0 Then
                    'If UsuarioBD_Ext.Length > 0 Then 'estan encriptados
                    'End If

                    Select Case TipoServidorBD_Ext
                        Case "20" 'ORACLE
                            txtconn.Append("user id=").Append(UsuarioBD_Ext)
                            txtconn.Append(";password=").Append(ClaveBD_Ext)
                            txtconn.Append(";Data Source=").Append(Me.servidorBD_Ext)
                            txtconn.Append(";provider=").Append("OraOLEDB.Oracle")
                            txtconn.Append(";connection reset=false")
                            txtconn.Append(";connection lifetime=5;PLSQLRSet=1")
                            'txtconn.Append(";connection timeout=30")
                            If Not ConnectionTimeOut_Ext.Equals("") Then
                                txtconn.Append(";connection timeout=").Append(Me.ConnectionTimeOut_Ext)
                            End If
                        Case "3" 'SYBASE
                            txtconn.Append("Data Source=").Append(Me.servidorBD_Ext)
                            txtconn.Append(";UID=").Append(Me.UsuarioBD_Ext)
                            txtconn.Append(";PWD=").Append(Me.ClaveBD_Ext)

                            If Not BaseDatos_Ext.Equals("") Then
                                txtconn.Append(";Database=").Append(Me.BaseDatos_Ext)
                            End If
                            If Not PuertoBD_Ext.Equals("") Then
                                txtconn.Append(";Port=").Append(PuertoBD_Ext)
                            End If
                            If Not AppBD_Ext.Equals("") Then
                                txtconn.Append(";Application Name=").Append(AppBD_Ext)
                            End If
                            If Not ConnectionTimeOut_Ext.Equals("") Then
                                txtconn.Append(";Connection Timeout=").Append(Me.ConnectionTimeOut_Ext)
                            End If
                        Case Else 'SQLSERVER
                            If UsuarioBD_Ext.Equals("") Then 'Relacion de confianza
                                txtconn.Append("integrated security=SSPI")
                            Else 'usuario aplicativo
                                txtconn.Append("user id=").Append(UsuarioBD_Ext)
                                txtconn.Append(";password=").Append(ClaveBD_Ext)
                            End If
                            txtconn.Append(";Server=").Append(Me.servidorBD_Ext)
                            txtconn.Append(";Initial Catalog=SIPE_FRAMEWORK")   'jcastro
                            txtconn.Append(";connection reset=false")
                            If Not AppBD_Ext.Equals("") Then
                                txtconn.Append(";Application Name=").Append(AppBD_Ext)
                            End If
                            If Not PuertoBD_Ext.Equals("") Then
                                txtconn.Append(";Port=").Append(PuertoBD_Ext)
                            End If
                            If Not BaseDatos_Ext.Equals("") Then
                                txtconn.Append(";Database=").Append(Me.BaseDatos_Ext)
                            End If
                            If Not ConnectionTimeOut_Ext.Equals("") Then
                                txtconn.Append(";Connect Timeout=").Append(Me.ConnectionTimeOut_Ext)
                            End If
                    End Select

                    If Not MinPool_Ext.Equals("") Then
                        txtconn.Append(";min pool size=").Append(MinPool_Ext)
                    End If
                    If Not MaxPool_Ext.Equals("") Then
                        txtconn.Append(";max pool size=").Append(MaxPool_Ext)
                    End If
                    'Para auditar si la clave tiene datos
                    If ClaveBD_Ext.Length > 0 Then
                        _Dato = txtconn.ToString().Replace(ClaveBD_Ext, "********")
                    Else
                        _Dato = txtconn.ToString()
                    End If
                End If
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "LeeConexion_Ext", pCodError, pMsgError + " " + _Dato)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "LeeConexion_Ext", _Dato)
            End Try

            Return (txtconn.ToString())

        End Function
        Private Sub CierraCnn(ByRef cn As SqlConnection)
            Try
                If Not IsNothing(cn) Then
                    If cn.State <> ConnectionState.Fetching And cn.State <> ConnectionState.Closed Then
                        Try
                            If Not IsNothing(Trans) Then
                                Trans.Rollback()
                            End If
                        Catch ex As Exception
                            Trans = Nothing
                        End Try
                    End If
                    If cn.State = ConnectionState.Open OrElse cn.State = ConnectionState.Fetching Then
                        cn.Close()
                    End If
                End If
            Catch ex As Exception
                cn = Nothing
            End Try
        End Sub
        Private Function getLicModuloEmp(ByVal Organizacion As Integer) As Boolean
            Dim item As Integer
            Try
                For Each item In Me.modulo
                    If item = Organizacion Then
                        Return True
                    End If
                Next
            Catch ex As Exception

            End Try

            'Return False
            'No esta controlando licencia
            Return True

        End Function
        Private Sub GeneraParametros(ByVal PI_rootXML As XmlElement, ByVal PI_arrParam As Array)
            Dim param As SqlParameter
            Dim nodoParam As XmlNode
            Dim txtAuditoria As String = ""
            Try
                Try
                    Dim xmlAud As New XmlDocument
                    xmlAud.LoadXml(PI_rootXML.OuterXml)
                    xmlAud.DocumentElement.SetAttribute("Valores", ArrayToString(PI_arrParam))
                    txtAuditoria = xmlAud.OuterXml
                Catch ex As Exception
                    txtAuditoria = PI_rootXML.OuterXml
                End Try

                clsError.setMetodo(True, "clsBaseDatosSQL", "GeneraParametros", txtAuditoria)
                For Each nodoParam In PI_rootXML.ChildNodes
                    Dim elemParam As XmlElement = CType(nodoParam, XmlElement)
                    Dim longitud As Integer
                    If elemParam.GetAttribute("tipo").ToLower().Equals("image") _
                        Or elemParam.GetAttribute("tipo").ToLower().Equals("varbinary") Then
                        If Not IsNothing(PI_arrParam(CType(elemParam.GetAttribute("posicion"), Integer))) Then
                            longitud = UBound(PI_arrParam(CType(elemParam.GetAttribute("posicion"), Integer))) + 1
                        Else
                            longitud = 0
                            PI_arrParam(CType(elemParam.GetAttribute("posicion"), Integer)) = System.DBNull.Value
                        End If
                    ElseIf Not elemParam.GetAttribute("longitud").ToLower().Equals("max") Then
                        'Para el caso de campos varchar(max) no se pone longitud
                        longitud = CType(elemParam.GetAttribute("longitud"), Integer)
                    Else
                        If elemParam.GetAttribute("direccion").ToLower().Equals("input") Then
                            longitud = PI_arrParam(CType(elemParam.GetAttribute("posicion"), Integer)).ToString().Length
                        Else ' Es parametro de salida, no tiene valor que calcular
                            longitud = -1
                        End If
                    End If

                    param = New SqlParameter(elemParam.GetAttribute("nombre"), _
                                            getTipo(elemParam.GetAttribute("tipo")), longitud)
                    param.Direction = getDireccion(elemParam.GetAttribute("direccion"))
                    If elemParam.GetAttribute("direccion").ToLower().Equals("input") Then
                        param.Value = PI_arrParam(CType(elemParam.GetAttribute("posicion"), Integer))
                    End If
                    cmd.Parameters.Add(param)
                Next

            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "GeneraParametros", pCodError, pMsgError + " " + txtAuditoria)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "GeneraParametros")
            End Try

            'Si esta habilitado el monitoreo, se registra
            'If TraceTrans.Equals("S") Then
            '    GeneraXmlEntrada(cmd)
            'End If


        End Sub
        Private Function getTipo(ByVal tipo As String) As SqlDbType
            Select Case LCase(tipo)
                Case "int"
                    Return SqlDbType.Int
                Case "tinyint"
                    Return SqlDbType.TinyInt
                Case "smallint"
                    Return SqlDbType.SmallInt
                Case "char"
                    Return SqlDbType.Char
                Case "varchar"
                    Return SqlDbType.VarChar
                Case "datetime"
                    Return SqlDbType.DateTime
                Case "float"
                    Return SqlDbType.Float
                Case "money"
                    Return SqlDbType.Money
                Case "decimal"
                    Return SqlDbType.Decimal
                Case "image"
                    Return SqlDbType.VarBinary
                Case "bit"
                    Return SqlDbType.Bit
                Case "nchar"
                    Return SqlDbType.NChar
                Case "nvarchar"
                    Return SqlDbType.NVarChar
                Case "real"
                    Return SqlDbType.Real
                Case "varbinary"
                    Return SqlDbType.VarBinary
                Case "binary"
                    Return SqlDbType.Binary
                Case Else
                    Return SqlDbType.VarChar
            End Select

        End Function
        Private Function getDireccion(ByVal direccion As String) As ParameterDirection
            Select Case direccion.ToLower()
                Case "input"
                    Return ParameterDirection.Input
                Case "output"
                    Return ParameterDirection.Output
                Case "return"
                    Return ParameterDirection.ReturnValue
                Case "inputoutput"
                    Return ParameterDirection.InputOutput
            End Select
        End Function
        Private Sub GeneraXMLds(ByVal PI_ds As DataSet)
            Try
                Dim dt As DataTable = New DataTable("TblEstado")
                dt.Columns.Add(New DataColumn("CodError", GetType(Integer)))
                dt.Columns.Add(New DataColumn("MsgError", GetType(String)))
                Dim dr As DataRow
                dr = dt.NewRow
                dr.BeginEdit()
                dr("CodError") = CodError
                dr("MSgError") = MsgError
                dr.EndEdit()
                dt.Rows.Add(dr)
                PI_ds.Tables.Add(dt)
            Catch ex As Exception

            End Try

        End Sub
        Private Function GeneraXMLdr(ByVal PI_dr As SqlDataReader) As XmlDocument

            Dim elem As XmlElement
            Dim xml As New XmlDocument
            Dim i As Integer
            Try
                clsError.setMetodo(True, "clsBaseDatosSQL", "GeneraXMLdr")
                xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Registro/>")
                While PI_dr.Read()
                    elem = xml.CreateElement("Row")
                    For i = 0 To PI_dr.FieldCount - 1
                        If PI_dr.GetName(i).Equals("") Then
                            elem.SetAttribute("Colum" + i.ToString(), PI_dr(i).ToString())
                        Else
                            elem.SetAttribute(PI_dr.GetName(i), PI_dr(i).ToString())
                        End If
                    Next
                    xml.DocumentElement.AppendChild(elem)
                End While
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "GeneraXMLdr", pCodError, pMsgError)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "GeneraXMLdr")
            End Try

            xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
            xml.DocumentElement.SetAttribute("MsgError", MsgError)
            Return xml
        End Function
        Private Sub GeneraXmlOut(ByRef xml As XmlDocument)
            Dim item As SqlParameter
            Try
                For Each item In cmd.Parameters
                    Try
                        If item.Direction.ToString().Equals("Output") Then
                            If Not item.ParameterName.Equals("") Then
                                xml.DocumentElement.SetAttribute(item.ParameterName.Substring(1, item.ParameterName.Length - 1), item.Value.ToString())
                            Else
                                xml.DocumentElement.SetAttribute("PO_", item.Value.ToString())
                            End If
                        ElseIf item.ParameterName.Equals("codRetorno") Then
                            If Not item.Value.ToString.Equals("0") Then
                                'setear codigo de error
                                Me.pCodError = item.Value.ToString()
                                Me.pMsgError = "Transaccion retorna codigo de error diferente de cero"
                                Exit For
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                Next
            Catch ex As Exception
                pMsgError = ex.Message
                pCodError = -100
            End Try

        End Sub
#End Region
#Region "Metodos Publicos"
        Public Sub SetSemilla(PISemilla As String)
            _Semilla = PISemilla
        End Sub
        'Public Sub getLicEmpresa()
        '    Me.pCodError = 0
        '    Me.pMsgError = ""

        '    Try

        '        If Me.objLicencia.plicencia Then

        '            Me.Cnn = New SqlConnection(LeeConexion())
        '            Me.Cnn.Open()

        '            cmd = New SqlCommand
        '            cmd.Connection = Me.Cnn
        '            cmd.CommandType = CommandType.StoredProcedure
        '            cmd.CommandText = BaseSeguridad + "seg_p_getLicEmpresa"

        '            Dim param As SqlParameter

        '            param = New SqlParameter("@IdEmpresa", System.Data.SqlDbType.Int, 4)
        '            param.Direction = System.Data.ParameterDirection.Input
        '            param.Value = Me.pEmpresa
        '            cmd.Parameters.Add(param)

        '            'Retorna el nombre del stored procedure y sus parametros
        '            Dim dr As SqlDataReader = cmd.ExecuteReader()
        '            Dim strLicencia As String = ""
        '            Dim strNombre As String = ""
        '            Dim strEmpresa As String = ""

        '            If dr.Read() Then
        '                strNombre = dr(0)
        '                strLicencia = dr(1)
        '            End If

        '            dr.Close()
        '            cmd.Dispose()
        '            Cnn.Close()
        '            'Recupero el desifrado
        '            Dim objEncripta As clsEncripta = New clsEncripta
        '            strLicencia = objEncripta.Decrypt(strLicencia, "vh700-vh700")
        '            Dim pos As Integer

        '            pos = strLicencia.IndexOf("-")
        '            If pos > 0 Then
        '                strEmpresa = strLicencia.Substring(0, pos)
        '                pos = pos + 1
        '                strLicencia = strLicencia.Substring(pos, strLicencia.Length - pos)
        '            End If
        '            If CType(strEmpresa, Integer) = Me.pEmpresa Then
        '                pos = strLicencia.IndexOf("-")
        '                If pos > 0 Then
        '                    strNombre = strLicencia.Substring(0, pos)
        '                End If
        '                If Me.nombreEmpresa.IndexOf(strNombre) >= 0 Then
        '                    pos = pos + 1
        '                    strLicencia = strLicencia.Substring(pos, strLicencia.Length - pos)
        '                    Dim i As Integer = 0
        '                    Dim j As Integer
        '                    For j = 0 To strLicencia.Length - 1 Step 2
        '                        Me.pmodulo(i) = CType(strLicencia.Substring(j, 2), Integer)
        '                        i = i + 1
        '                    Next
        '                    Me.licenciaEmpresa = True
        '                Else
        '                    Me.licenciaEmpresa = False
        '                End If
        '            Else
        '                Me.licenciaEmpresa = False
        '            End If
        '        End If
        '    Catch err As Exception
        '        pCodError = -100
        '        pMsgError = err.Message
        '        CierraCnn(Cnn)
        '    End Try

        '    'Por que no esta controlando licencia
        '    licenciaEmpresa = True

        'End Sub
        Public Sub getConexionApl(ByVal IdAplicacion As Integer)
            'Recupera los datos de conexion de la aplicacion dada 
            'Esto es para acceso a bases externas
            Dim xmlParam As String = ""
            Me.pCodError = 0
            Me.pMsgError = ""

            Try
                'Registra inicio en trace

                clsError.setMetodo(True, "clsBaseDatosSQL", "getConexionApl", IdAplicacion.ToString())

                cmd = New SqlCommand
                Me.pcnnSeg = New SqlConnection(LeeConexion())
                Me.pcnnSeg.Open()

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = BaseSeguridad + "Seg_P_getConexion"
                cmd.Connection = pcnnSeg

                Dim param As SqlParameter

                param = New SqlParameter("@PI_IdAplicacion", System.Data.SqlDbType.Int, 4)
                param.Direction = System.Data.ParameterDirection.Input
                param.Value = IdAplicacion
                cmd.Parameters.Add(param)

                'Retorna datos de conexion para esa aplicacion
                Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
                Dim dr As SqlDataReader = cmd.ExecuteReader()
                Dim valor As String = ""
                While dr.Read()
                    'Pregunta si encripta o no el parametro
                    If dr("Encriptado") = 1 Then 'Si esta encriptado lo desencripta
                        If dr("Valor").ToString().Length > 0 Then
                            valor = objEncripta.Decrypt(dr("Valor").ToString(), _Semilla)
                        Else
                            valor = dr("Valor").ToString()
                        End If
                    Else
                        valor = dr("Valor").ToString()
                    End If
                    Select Case dr("Parametro").ToString()
                        Case "Servidor"
                            Me.pServidorBD_Ext = valor
                        Case "TipoServidor"
                            Me.pTipoServidorBD_Ext = valor
                        Case "Usuario"
                            Me.pUsuarioBD_Ext = valor
                        Case "Clave"
                            Me.pClaveBD_Ext = valor
                        Case "Puerto"
                            Me.pPuertoBD_Ext = valor
                        Case "MinPool"
                            Me.pMinPool_Ext = valor
                        Case "MaxPool"
                            Me.pMaxPool_Ext = valor
                        Case "NombreApp"
                            pAppBD_Ext = valor
                        Case "BaseDatos"
                            pBaseDatos_Ext = valor
                        Case "ConnectionTimeOut"
                            pConnectionTimeOut_Ext = valor
                            'Case "Semilla"
                            '   Semilla = dr("Valor").ToString()
                    End Select
                End While
                dr.Close()
                cmd.Dispose()
                pcnnSeg.Close()

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(pcnnSeg)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "getConexionApl", pCodError, pMsgError)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(pcnnSeg)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "getConexionApl", pCodError, pMsgError)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "getConexionApl", xmlParam)
            End Try

        End Sub
        Public Function getPermiso(ByVal Organizacion As Integer, _
                                ByVal CodTrans As Integer, _
                                ByVal CodOpcion As Integer, _
                                ByVal ParamAut As String, _
                                ByVal ValorAut As String, _
                                ByVal txtTrans As String) As String
            'Verifica que la trnasaccion tiene permisos y retorna un
            'documento xml con los datos del Stored Procedure a Ejecutar
            Dim xmlParam As String = ""
            Me.pCodError = 0
            Me.pMsgError = ""
            Dim xmlVerifica As New XmlDocument
            Try
                'Registra inicio en trace

                clsError.setMetodo(True, "clsBaseDatosSQL", "getPermiso", _
                                "org=" + Organizacion.ToString() + _
                                ",trans=" + CodTrans.ToString() + _
                                ",opc=" + CodOpcion.ToString() + _
                                ",parAut=" + ParamAut + _
                                ",valAut=" + ValorAut + _
                                ",txtTrans=" + txtTrans)

                If (CodTrans = 192 And Organizacion = 1) OrElse _
                    Me.objLicencia Then

                    Me.pcnnSeg = New SqlConnection(LeeConexion())
                    Me.pcnnSeg.Open()

                    cmd = New SqlCommand
                    cmd.Connection = pcnnSeg

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = BaseSeguridad + "seg_p_permiso_transaccion"

                    Dim param As SqlParameter

                    param = New SqlParameter("@PV_UsrSitio", System.Data.SqlDbType.VarChar, 20)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = UsrSitio
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_TokenSitio", System.Data.SqlDbType.VarChar, 32)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = TokenSitio
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_MaqSitio", System.Data.SqlDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = MaqSitio
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_idUsuario", System.Data.SqlDbType.VarChar, 20)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = usuario
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_Token", System.Data.SqlDbType.VarChar, 32)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = token
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_Maquina", System.Data.SqlDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = maquina
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_idAplicacion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = aplicacion
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_idOrganizacion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Organizacion
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdTransaccion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = CodTrans
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdOpcion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = CodOpcion
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdEmpresa", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Me.empresa
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdSucursal", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Me.sucursal
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_ParamAut", System.Data.SqlDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = ParamAut
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_Valor", System.Data.SqlDbType.VarChar, 50)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = ValorAut
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_txtTransaccion", System.Data.SqlDbType.VarChar, txtTrans.Length)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = txtTrans 'Contiene el valor de la Cadena que se va  aregistrar en Auditoria
                    cmd.Parameters.Add(param)

                    'Retorna el nombre del stored procedure y sus parametros
                    Dim dr As SqlDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        ' TraceTrans = dr("TraceTrans").ToString()
                        xmlParam = dr("Parametros").ToString()

                        Try
                            xmlVerifica.LoadXml(xmlParam)
                        Catch ex As Exception
                            Throw New Exception("Xml incorrecto en campo (Parametros). Tabla Seguridad.Seg_Transaccion. " & xmlParam)
                        End Try

                        'tiene el Id del Servidor Remoto donde se debe ejecutar la 
                        'transaccion, si es el local trae 0
                        Me.pIdServidor_Ext = dr("IdServidor")
                    End If
                    dr.Close()
                    cmd.Dispose()
                    pcnnSeg.Close()
                    pcnnSeg.Dispose()
                Else
                    Throw New Exception("El servidor no tiene licencia valida")
                End If

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(pcnnSeg)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "getPermiso", pCodError, pMsgError + _
                                "org=" + Organizacion.ToString() + _
                                ",trans=" + CodTrans.ToString() + _
                                ",opc=" + CodOpcion.ToString() + _
                                ",parAut=" + ParamAut + _
                                ",valAut=" + ValorAut + _
                                ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.StackTrace & "Mensaje: " & err.Message
                CierraCnn(pcnnSeg)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "getPermiso", pCodError, pMsgError + _
                                "org=" + Organizacion.ToString() + _
                                ",trans=" + CodTrans.ToString() + _
                                ",opc=" + CodOpcion.ToString() + _
                                ",parAut=" + ParamAut + _
                                ",valAut=" + ValorAut + _
                                ",txtTrans=" + txtTrans)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "getPermiso", xmlParam)
            End Try

            Return xmlParam

        End Function
        Public Function ValidaPermiso(ByVal Organizacion As Integer, _
                                ByVal CodTrans As Integer, _
                                ByVal CodOpcion As Integer, _
                                ByVal ParamAut As String, _
                                ByVal ValorAut As String, _
                                ByVal txtTrans As String) As Boolean
            'Verifica que la trnasaccion tiene permisos y retorna un
            'documento xml con los datos del Stored Procedure a Ejecutar
            Dim permiso As Boolean = False
            Me.pCodError = 0
            Me.pMsgError = ""

            Try

                clsError.setMetodo(True, "clsBaseDatosSQL", "ValidaPermiso", _
                           "org=" + Organizacion.ToString() + _
                           ",trans=" + CodTrans.ToString() + _
                           ",opc=" + CodOpcion.ToString() + _
                           ",parAut=" + ParamAut + _
                           ",valAut=" + ValorAut + _
                           ",txtTrans=" + txtTrans)


                If Me.objLicencia Then

                    cmd = New SqlCommand


                    Me.pcnnSeg = New SqlConnection(LeeConexion())
                    Me.pcnnSeg.Open()
                    cmd.Connection = Me.pcnnSeg

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = BaseSeguridad + "seg_p_permiso_transaccion"

                    Dim param As SqlParameter

                    param = New SqlParameter("@PV_UsrSitio", System.Data.SqlDbType.VarChar, 20)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = UsrSitio
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_TokenSitio", System.Data.SqlDbType.VarChar, 32)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = TokenSitio
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_MaqSitio", System.Data.SqlDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = MaqSitio
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_idUsuario", System.Data.SqlDbType.VarChar, 20)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = usuario
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_Token", System.Data.SqlDbType.VarChar, 32)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = token
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_Maquina", System.Data.SqlDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = maquina
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_idAplicacion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = aplicacion
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_idOrganizacion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Organizacion
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdTransaccion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = CodTrans
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdOpcion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = CodOpcion
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdEmpresa", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Me.empresa
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdSucursal", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Me.sucursal
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_ParamAut", System.Data.SqlDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = ParamAut
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_Valor", System.Data.SqlDbType.VarChar, 50)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = ValorAut
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_txtTransaccion", System.Data.SqlDbType.VarChar, 2000)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = txtTrans 'Contiene el valor de la Cadena que se va  aregistrar en Auditoria
                    cmd.Parameters.Add(param)

                    'Retorna el nombre del stored procedure y sus parametros
                    Dim dr As SqlDataReader = cmd.ExecuteReader()
                    dr.Close()
                    cmd.Dispose()
                    pcnnSeg.Close()
                    pcnnSeg.Dispose()
                    permiso = True
                Else
                    Throw New Exception("El servidor no tiene licencia valida")
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = "getPermiso: " + errSql.Message
                CierraCnn(pcnnSeg)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "ValidaPermiso", pCodError, pMsgError + _
                           "org=" + Organizacion.ToString() + _
                           ",trans=" + CodTrans.ToString() + _
                           ",opc=" + CodOpcion.ToString() + _
                           ",parAut=" + ParamAut + _
                           ",valAut=" + ValorAut + _
                           ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = "getPermiso: " + err.Message
                CierraCnn(pcnnSeg)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "ValidaPermiso", pCodError, pMsgError + _
                           "org=" + Organizacion.ToString() + _
                           ",trans=" + CodTrans.ToString() + _
                           ",opc=" + CodOpcion.ToString() + _
                           ",parAut=" + ParamAut + _
                           ",valAut=" + ValorAut + _
                           ",txtTrans=" + txtTrans)
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "ValidaPermiso", IIf(permiso, "true", "false"))
            End Try
            Return permiso

        End Function
        Public Function VerificaTrans(ByVal PI_IdEmpresa As Integer, _
                                ByVal PI_IdSucursal As Integer, _
                                ByVal PI_Organizacion As Integer, _
                                ByVal PI_CodTrans As Integer, _
                                ByVal PI_CodOpcion As Integer, _
                                ByVal PI_ParamAut As String, _
                                ByVal PI_ValorAut As String, _
                                Optional ByVal PI_txtTrans As String = "") As Boolean
            'Verifica que la transaccion tiene permisos y retorna un
            'logico de verdadero si tiene permisos y falso si no tiene permisos
            'SI PI_IdSucursal = 0 , No valida por localidad, solo por Empresa

            Dim xmlParam As String = ""
            Me.pCodError = 0
            Me.pMsgError = ""
            Dim retorno As Boolean = False


            Try
                clsError.setMetodo(True, "clsBaseDatosSQL", "VerificaPermiso", _
                                    "org=" + PI_Organizacion.ToString() + _
                                    ",trans=" + PI_CodTrans.ToString() + _
                                    ",opc=" + PI_CodOpcion.ToString() + _
                                    ",parAut=" + PI_ParamAut + _
                                    ",valAut=" + PI_ValorAut + _
                                    ",empr=" + PI_IdEmpresa.ToString() + _
                                    ",suc=" + PI_IdSucursal.ToString())

                If Me.objLicencia Then

                    'Lo lleva a formato de Xml si es un string sin formato
                    txtTrans = settxtTrans(arrParam, PI_txtTrans)

                    'Transacciones iniciales que no necesitan verificacion
                    'de empresa licenciada
                    'Por ahora nose controla licencia de empresa
                    'If Not (PI_Organizacion = 2 And PI_CodTrans >= 1000 And PI_CodTrans < 2000) Then
                    '    If Not Me.plicenciaEmpresa Then
                    '        Throw New Exception("La Empresa no tiene licencia valida")
                    '    End If
                    '    If Not Me.getLicModuloEmp(PI_Organizacion) Then
                    '        Throw New Exception("Este modulo no esta licenciado para esta empresa")
                    '    End If
                    'End If

                    cmd = New SqlCommand
                    Me.pcnnSeg = New SqlConnection(LeeConexion())
                    Me.pcnnSeg.Open()
                    cmd.Connection = Me.pcnnSeg

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = BaseSeguridad + "seg_p_verifica_transaccion"

                    Dim param As SqlParameter

                    param = New SqlParameter("@PV_idUsuario", System.Data.SqlDbType.VarChar, 20)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = usuario
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_Token", System.Data.SqlDbType.VarChar, 32)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = token
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_Maquina", System.Data.SqlDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = maquina
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_idAplicacion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = aplicacion
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_idOrganizacion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_Organizacion
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdTransaccion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_CodTrans
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdOpcion", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_CodOpcion
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdEmpresa", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_IdEmpresa
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_IdSucursal", System.Data.SqlDbType.Int, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_IdSucursal
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_ParamAut", System.Data.SqlDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_ParamAut
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_Valor", System.Data.SqlDbType.VarChar, 50)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_ValorAut
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PV_txtTransaccion", System.Data.SqlDbType.VarChar, txtTrans.Length)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = txtTrans 'Contiene el valor de la Cadena que se va  aregistrar en Auditoria
                    cmd.Parameters.Add(param)

                    param = New SqlParameter("@PO_CodRetorno", System.Data.SqlDbType.Bit, 1)
                    param.Direction = System.Data.ParameterDirection.Output
                    cmd.Parameters.Add(param)

                    'Retorna el bit si tiene permisos
                    cmd.ExecuteNonQuery()
                    retorno = cmd.Parameters("@PO_CodRetorno").Value

                    cmd.Dispose()
                    pcnnSeg.Close()
                    pcnnSeg.Dispose()
                Else
                    Throw New Exception("El servidor no tiene licencia valida")
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = "getPermiso: " + errSql.Message
                CierraCnn(pcnnSeg)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "VerificaTrans", pCodError, pMsgError + _
                                    ",trans=" + PI_CodTrans.ToString() + _
                                    ",opc=" + PI_CodOpcion.ToString() + _
                                    ",parAut=" + PI_ParamAut + _
                                    ",valAut=" + PI_ValorAut + _
                                    ",empr=" + PI_IdEmpresa.ToString() + _
                                    ",suc=" + PI_IdSucursal.ToString())
            Catch err As Exception
                pCodError = -100
                pMsgError = "getPermiso: " + err.Message
                CierraCnn(pcnnSeg)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "VerificaTrans", pCodError, pMsgError + _
                                    ",trans=" + PI_CodTrans.ToString() + _
                                    ",opc=" + PI_CodOpcion.ToString() + _
                                    ",parAut=" + PI_ParamAut + _
                                    ",valAut=" + PI_ValorAut + _
                                    ",empr=" + PI_IdEmpresa.ToString() + _
                                    ",suc=" + PI_IdSucursal.ToString())
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "VerificaTrans", IIf(retorno, "true", "false"))
            End Try

            Return retorno

        End Function
        Public Function Exec_SP_NoQuery(ByVal Organizacion As Integer, _
                       ByVal CodTran As Integer, _
                       ByVal CodOpcion As Integer, _
                       ByVal arrParam As Array, _
                       ByVal ParamAut As String, _
                       ByVal ValorAut As String, _
                       ByVal txtTrans As String) As XmlDocument

            Me.pMsgError = ""
            'Dim elem As XmlElement
            Dim xml As New XmlDocument
            Dim root As XmlElement
            xml.LoadXml("<Regitro/>")
            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SP_NoQuery", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)

                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Me.Cnn = New SqlConnection(LeeConexion())
                    Me.Cnn.Open()

                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    root = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New SqlCommand
                    cmd.Connection = Cnn
                    If pConnectionTimeOut.Length > 0 Then
                        cmd.CommandTimeout = pConnectionTimeOut
                    End If
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    If pCodError = 0 Then
                        cmd.ExecuteNonQuery()
                        Me.GeneraXmlOut(xml)
                    End If
                    Cnn.Close()
                    Cnn.Dispose()
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_NoQuery", pCodError, pMsgError + _
                                   ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_NoQuery", pCodError, pMsgError + _
                                   ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP_NoQuery", xml.OuterXml)
            End Try

            'Si esta habilitado el monitoreo, se registra
            'If TraceTrans.Equals("S") Then
            '    XmlSalida = New XmlDocument
            '    XmlSalida.LoadXml(xml.OuterXml)
            '    'Registra Monitoreo en Base
            '    RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            'End If
            Return xml

        End Function
        Public Function Exec_SP(ByVal Organizacion As Integer, _
                                ByVal CodTran As Integer, _
                                ByVal CodOpcion As Integer, _
                                ByVal arrParam As Array, _
                                ByVal ParamAut As String, _
                                ByVal ValorAut As String, _
                                ByVal txtTrans As String) As DataSet
            Dim ds As DataSet = New DataSet

            Dim da As SqlDataAdapter = New SqlDataAdapter
            Me.pCodError = 0
            Me.pMsgError = ""

            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Me.Cnn = New SqlConnection(LeeConexion())
                    Me.Cnn.Open()
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New SqlCommand
                    cmd.Connection = Cnn
                    If pConnectionTimeOut.Length > 0 Then
                        cmd.CommandTimeout = pConnectionTimeOut
                    End If
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    If pCodError = 0 Then
                        da.SelectCommand = cmd
                        da.Fill(ds, nameSP)
                    End If
                    Cnn.Close()
                    Cnn.Dispose()
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP", pCodError, pMsgError + _
                                   ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP", pCodError, pMsgError + _
                                   ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                GeneraXMLds(ds)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP", ds.GetXml())
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(ds.GetXml())
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If

            'Catch ex As Exception

            'End Try

            Return ds

        End Function
        Public Function Exec_SP(ByVal Organizacion As Integer, _
                          ByVal CodTran As Integer, _
                          ByVal CodOpcion As Integer, _
                          ByVal arrParam As Array, _
                          ByVal ParamAut As String, _
                          ByVal ValorAut As String, _
                          ByVal txtTrans As String, _
                          ByVal NombreDS As String) As DataSet
            Dim ds As DataSet = New DataSet

            Dim da As SqlDataAdapter = New SqlDataAdapter

            Me.pCodError = 0
            Me.pMsgError = ""

            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SP", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)

                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Me.Cnn = New SqlConnection(LeeConexion())
                    Me.Cnn.Open()
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New SqlCommand
                    cmd.Connection = Cnn
                    If pConnectionTimeOut.Length > 0 Then
                        cmd.CommandTimeout = pConnectionTimeOut
                    End If
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    If pCodError = 0 Then
                        da.SelectCommand = cmd
                        da.Fill(ds, NombreDS)
                    End If
                    Cnn.Close()
                    Cnn.Dispose()
                End If

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                GeneraXMLds(ds)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP", ds.GetXml)
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(ds.GetXml())
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If
            'Catch ex As Exception

            'End Try

            Return ds

        End Function
        Public Function Exec_SP_Tran(ByVal Organizacion As Integer, _
                        ByVal CodTran As Integer, _
                        ByVal CodOpcion As Integer, _
                        ByVal arrParam As Array, _
                        ByVal ParamAut As String, _
                        ByVal ValorAut As String, _
                        ByVal txtTrans As String) As XmlDocument

            Me.pCodError = 0
            Me.pMsgError = ""
            Dim xml As New XmlDocument
            xml.LoadXml("<Regitro/>")
            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SP_Tran", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)

                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    'Logica de Transaccionalida usando cnn
                    cmd = New SqlCommand
                    Select Case Me.OpcionTrans
                        Case "N"
                            Me.Cnn = New SqlConnection(LeeConexion())
                            Me.Cnn.Open()
                            cmd.Connection = Me.Cnn
                        Case "B"
                            Me.Cnn = New SqlConnection(LeeConexion())
                            Me.Cnn.Open()
                            cmd.Connection = Me.Cnn
                            Me.Trans = Me.Cnn.BeginTransaction
                            cmd.Transaction = Me.Trans
                        Case "T"
                            cmd.Connection = Me.Cnn
                            cmd.Transaction = Me.Trans
                        Case Else
                            Me.Cnn = New SqlConnection(LeeConexion())
                            Me.Cnn.Open()
                            cmd.Connection = Me.Cnn
                    End Select

                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")
                    If pConnectionTimeOut.Length > 0 Then
                        cmd.CommandTimeout = pConnectionTimeOut
                    End If
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure
                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    If pCodError = 0 Then
                        cmd.ExecuteNonQuery()
                        Me.GeneraXmlOut(xml)
                    End If

                End If

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_Tran", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_Tran", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP_Tran", xml.OuterXml)
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(xml.OuterXml)
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If

            'Catch ex As Exception

            'End Try

            Return xml

        End Function
        Public Function Exec_Query(ByVal query As String) As DataSet
            Dim da As SqlDataAdapter = New SqlDataAdapter
            Dim ds As DataSet = New DataSet
            Me.pCodError = 0
            Me.pMsgError = ""

            Try
                'Setea lo que se audita de los datos
                txtTrans = query 'asume esto como parametro
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_Query", txtTrans)

                Dim xmlSP As String = getPermiso(Me.Organizacion, Me.Transaccion, Me.Opcion, "", "", txtTrans)
                If Me.pCodError = 0 Then
                    Me.Cnn = New SqlConnection(LeeConexion())
                    Me.Cnn.Open()
                    cmd = New SqlCommand
                    cmd.Connection = Cnn
                    If pConnectionTimeOut.Length > 0 Then
                        cmd.CommandTimeout = pConnectionTimeOut
                    End If
                    cmd.CommandText = query
                    cmd.CommandType = CommandType.Text

                    da.SelectCommand = cmd
                    da.Fill(ds, "consulta")
                    Cnn.Close()
                    Cnn.Dispose()
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_Query", pCodError, pMsgError + " " + query)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_Query", pCodError, pMsgError + " " + query)
            Finally
                GeneraXMLds(ds)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_Query", ds.GetXml)
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(ds.GetXml())
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(0, 0, 0, "Consulta Generica", "")
            '    End If

            'Catch ex As Exception

            'End Try
            Return ds
        End Function
        Public Function Exec_Query(ByVal query As String, ByVal dsname As String) As DataSet
            Dim da As SqlDataAdapter = New SqlDataAdapter
            Dim ds As DataSet = New DataSet
            Me.pCodError = 0
            Me.pMsgError = ""

            Try
                'Setea lo que se audita de los datos
                txtTrans = query
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_Query", txtTrans)
                Dim xmlSP As String = getPermiso(Me.Organizacion, Me.Transaccion, Me.Opcion, "", "", txtTrans)
                If Me.pCodError = 0 Then
                    Me.Cnn = New SqlConnection(LeeConexion())
                    Me.Cnn.Open()
                    cmd = New SqlCommand
                    cmd.Connection = Cnn
                    If pConnectionTimeOut.Length > 0 Then
                        cmd.CommandTimeout = pConnectionTimeOut
                    End If
                    cmd.CommandText = query
                    cmd.CommandType = CommandType.Text

                    da.SelectCommand = cmd
                    da.Fill(ds, dsname)
                    Cnn.Close()
                    Cnn.Dispose()
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_Query", pCodError, pMsgError + " " + query)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_Query", pCodError, pMsgError + " " + query)
            Finally
                GeneraXMLds(ds)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_Query", ds.GetXml)
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(ds.GetXml())
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(0, 0, 0, "Consulta Generica:Ds", "")
            '    End If

            'Catch ex As Exception

            'End Try

            Return ds
        End Function
        Public Function Exec_SP_ParamOut(ByVal Organizacion As Integer, _
                                ByVal CodTran As Integer, _
                                ByVal CodOpcion As Integer, _
                                ByVal arrParam As Array, _
                                ByVal ParamAut As String, _
                                ByVal ValorAut As String, _
                                ByVal txtTrans As String) As XmlDocument

            Me.pCodError = 0
            Me.pMsgError = ""
            'Dim elem As XmlElement
            Dim xml As New XmlDocument
            'Dim i As Integer
            xml.LoadXml("<Regitro/>")
            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_ParamOut", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)

                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Me.Cnn = New SqlConnection(LeeConexion())
                    Me.Cnn.Open()
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New SqlCommand
                    cmd.Connection = Cnn
                    If pConnectionTimeOut.Length > 0 Then
                        cmd.CommandTimeout = pConnectionTimeOut
                    End If
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    If pCodError = 0 Then
                        cmd.ExecuteNonQuery()
                        'Retorna los valores de salida
                        Me.GeneraXmlOut(xml)
                    End If
                    Cnn.Close()
                    Cnn.Dispose()
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_ParamOut", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_ParamOut", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_ParamOut", xml.OuterXml)
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(xml.OuterXml)
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If

            'Catch ex As Exception

            'End Try

            Return xml

        End Function
        Public Function Exec_QuerySP(ByVal query As String) As XmlDocument
            Dim dr As SqlDataReader
            Dim xml As New XmlDocument
            Me.pCodError = 0
            Me.pMsgError = ""
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")

            Try
                'Setea lo que se audita de los datos
                txtTrans = query
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_QuerySP", txtTrans)
                Dim xmlSP As String = getPermiso(Me.Organizacion, Me.Transaccion, Me.Opcion, "", "", txtTrans)
                If Me.pCodError = 0 Then
                    Me.Cnn = New SqlConnection(LeeConexion())
                    Me.Cnn.Open()

                    cmd = New SqlCommand
                    cmd.Connection = Cnn
                    If pConnectionTimeOut.Length > 0 Then
                        cmd.CommandTimeout = pConnectionTimeOut
                    End If
                    cmd.CommandText = query
                    cmd.CommandType = CommandType.Text

                    dr = cmd.ExecuteReader()
                    xml = GeneraXMLdr(dr)
                    dr.Close()
                    Me.GeneraXmlOut(xml)
                    Cnn.Close()
                    Cnn.Dispose()
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_QuerySP", pCodError, pMsgError + " " + query)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_QuerySP", pCodError, pMsgError + " " + query)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_QuerySP", xml.OuterXml)
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(xml.OuterXml)
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(0, 0, 0, "Consulta Generica: Xml", "")
            '    End If

            'Catch ex As Exception

            'End Try

            Return xml
        End Function
        Public Function Exec_SP_Tran_Reader(ByVal Organizacion As Integer, _
                        ByVal CodTran As Integer, _
                        ByVal CodOpcion As Integer, _
                        ByVal arrParam As Array, _
                        ByVal ParamAut As String, _
                        ByVal ValorAut As String, _
                        ByVal txtTrans As String) As XmlDocument

            Dim dr As SqlDataReader
            Dim xml As New XmlDocument
            Me.pCodError = 0
            Me.pMsgError = ""
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")

            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SP_Tran_Reader", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)

                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    'Poner Logica de Transaccionalidad
                    cmd = New SqlCommand
                    Select Case Me.OpcionTrans
                        Case "N"
                            Me.Cnn = New SqlConnection(LeeConexion())
                            Me.Cnn.Open()
                            cmd.Connection = Me.Cnn
                        Case "B"
                            Me.Cnn = New SqlConnection(LeeConexion())
                            Me.Cnn.Open()
                            cmd.Connection = Me.Cnn
                            Me.Trans = Me.Cnn.BeginTransaction
                            cmd.Transaction = Me.Trans
                        Case "T"
                            cmd.Connection = Me.Cnn
                            cmd.Transaction = Me.Trans
                        Case Else
                            Me.Cnn = New SqlConnection(LeeConexion())
                            Me.Cnn.Open()
                            cmd.Connection = Me.Cnn
                    End Select

                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")
                    If pConnectionTimeOut.Length > 0 Then
                        cmd.CommandTimeout = pConnectionTimeOut
                    End If
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    If pCodError = 0 Then
                        dr = cmd.ExecuteReader()
                        xml = GeneraXMLdr(dr)
                        dr.Close()
                        Me.GeneraXmlOut(xml)
                    End If
                End If

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_Tran_Reader", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_Tran_Reader", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP_Tran_Reader", xml.OuterXml)
            End Try


            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(xml.OuterXml)
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If

            'Catch ex As Exception

            'End Try

            Return xml

        End Function
        Public Function Exec_SP_Reader(ByVal Organizacion As Integer, _
                                       ByVal CodTran As Integer, _
                                       ByVal CodOpcion As Integer, _
                                       ByVal arrParam As Array, _
                                       ByVal ParamAut As String, _
                                       ByVal ValorAut As String, _
                                       ByVal txtTrans As String) As XmlDocument

            Dim dr As SqlDataReader
            Dim xml As New XmlDocument
            Me.pCodError = 0
            Me.pMsgError = ""
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")

            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SP_Reader", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Me.Cnn = New SqlConnection(LeeConexion())
                    Me.Cnn.Open()
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New SqlCommand
                    cmd.Connection = Cnn
                    If pConnectionTimeOut.Length > 0 Then
                        cmd.CommandTimeout = pConnectionTimeOut
                    End If
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    If pCodError = 0 Then
                        dr = cmd.ExecuteReader()
                        xml = GeneraXMLdr(dr)
                        dr.Close()
                        Me.GeneraXmlOut(xml)
                    End If
                    Cnn.Close()
                    Cnn.Dispose()
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_Reader", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_Reader", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP_Reader", xml.OuterXml)
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(xml.OuterXml)
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If

            'Catch ex As Exception

            'End Try

            Return xml

        End Function
        Public Function IsPermiso(ByVal Organizacion As Integer, _
                                   ByVal CodTran As Integer, _
                                   ByVal CodOpcion As Integer, _
                                   ByVal txtTrans As String) As Boolean

            Me.pCodError = 0
            Me.pMsgError = ""
            Dim Retorno As Boolean = False
            Try
                clsError.setMetodo(True, "clsBaseDatosSQL", "IsPermiso", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",txtTrans=" + txtTrans)

                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                If pCodError = 0 Then
                    Retorno = True
                End If
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "IsPermiso", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",txtTrans=" + txtTrans)
                Retorno = False
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "IsPermiso", IIf(Retorno, "true", "false"))
            End Try

            Return Retorno

        End Function

#End Region
#Region "Metodos de Session"
        Public Sub setUsrSitio(ByVal PI_elem As XmlElement)
            Try
                clsError.setMetodo(True, "clsBaseDatosSQL", "setUsrSitio")
                'Se setea el Usuario de sitio de donde se ejecuta la transaccion
                If PI_elem.GetAttribute("PS_UsrSitio").Equals("") Then
                    Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
                    Me.pUsrSitio = objEncripta.Decrypt(System.Configuration.ConfigurationManager.AppSettings("UsuarioWS"))
                    Me.TokenSitio = objEncripta.Decrypt(System.Configuration.ConfigurationManager.AppSettings("TokenWS"))
                    Me.MaqSitio = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList(0).ToString()
                    'Me.MaqSitio = System.Web.HttpContext.Current.Application("MaquinaWS")
                Else
                    pUsrSitio = PI_elem.GetAttribute("PS_UsrSitio")
                    pTokenSitio = PI_elem.GetAttribute("PS_TokenSitio")
                    pMaqSitio = PI_elem.GetAttribute("PS_MaqSitio")
                End If
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "setUsrSitio", pCodError, pMsgError)
                Throw New Exception("Retorno Error")
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "setUsrSitio")
            End Try
        End Sub
        Public Sub SetUsuario(ByVal PI_elem As XmlElement)
            Try
                clsError.setMetodo(True, "clsBaseDatosSQL", "SetUsuario")
                'Se setea el usuario de sitio
                setUsrSitio(PI_elem)

                'Pregunto si Ps_IdUsuario es nulo, se asigna al usuario de Sitio como usuario login
                If PI_elem.GetAttribute("PS_IdUsuario").Equals("") Then
                    'Asigna como usuario al mismo usuario de sitio que trae
                    pUsuario = PI_elem.GetAttribute("PS_UsrSitio")
                    pToken = PI_elem.GetAttribute("PS_TokenSitio")
                    pMaquina = PI_elem.GetAttribute("PS_MaqSitio")
                Else 'Si viene usuario login
                    pUsuario = PI_elem.GetAttribute("PS_IdUsuario")
                    pToken = PI_elem.GetAttribute("PS_Token")
                    pMaquina = PI_elem.GetAttribute("PS_Maquina")
                End If
                pAplicacion = CType(PI_elem.GetAttribute("PS_IdAplicacion"), Integer)
                Me.pEmpresa = CType(PI_elem.GetAttribute("PS_IdEmpresa"), Integer)
                Me.pSucursal = CType(PI_elem.GetAttribute("PS_IdSucursal"), Integer)
                'Me.txtTrans = PI_elem.OuterXml
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "SetUsuario", pCodError, pMsgError)
                Throw New Exception("Retorno Error")
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "SetUsuario")
            End Try
        End Sub
        Public Sub SetDefaultUsuario(ByVal PI_elem As XmlElement)
            Try
                clsError.setMetodo(True, "clsBaseDatosSQL", "SetDefaultUsuario")
                'Se asigna al usuario de sitio que ejecuta la transaccion
                setUsrSitio(PI_elem)

                'Setea usuarios de ejecucion como si fuera el WebServices
                Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
                Me.pUsuario = objEncripta.Decrypt(System.Configuration.ConfigurationManager.AppSettings("UsuarioWS"))
                Me.pToken = objEncripta.Decrypt(System.Configuration.ConfigurationManager.AppSettings("TokenWS"))
                Me.pMaquina = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList(0).ToString()
                'Me.pMaquina = System.Web.HttpContext.Current.Application("MaquinaWS")

                If PI_elem.GetAttribute("PS_IdAplicacion").Length = 0 Then
                    pAplicacion = CType(System.Configuration.ConfigurationManager.AppSettings("IdAplicacion"), Integer)
                Else
                    pAplicacion = CType(PI_elem.GetAttribute("PS_IdAplicacion"), Integer)
                End If
                If PI_elem.GetAttribute("PS_IdEmpresa").Length = 0 Then
                    Me.pEmpresa = CType(System.Configuration.ConfigurationManager.AppSettings("IdEmpresa"), Integer)
                Else
                    Me.pEmpresa = CType(PI_elem.GetAttribute("PS_IdEmpresa"), Integer)
                End If
                If PI_elem.GetAttribute("PS_IdSucursal").Length = 0 Then
                    Me.pSucursal = CType(System.Configuration.ConfigurationManager.AppSettings("IdSucursal"), Integer)
                Else
                    Me.pSucursal = CType(PI_elem.GetAttribute("PS_IdSucursal"), Integer)
                End If
                'Me.txtTrans = PI_elem.OuterXml
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "SetDefaultUsuario", pCodError, pMsgError)
                Throw New Exception("Retorno Error")
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "SetDefaultUsuario")
            End Try

        End Sub
        
        Public Sub SetDefaultSitio()
            Try
                'El usuario de sitio y el usuario que ejecuta la trabsaccion es el Usuario del Sitio Web Services
                clsError.setMetodo(True, "clsBaseDatosSQL", "SetDefaultSitio")
                'Usuario de sesion por default
                Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
                Me.pUsrSitio = objEncripta.Decrypt(System.Configuration.ConfigurationManager.AppSettings("UsuarioWS"))
                Me.TokenSitio = objEncripta.Decrypt(System.Configuration.ConfigurationManager.AppSettings("TokenWS"))
                Me.MaqSitio = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList(0).ToString()
                'Me.MaqSitio = System.Web.HttpContext.Current.Application("MaquinaWS")

                pUsuario = Me.pUsrSitio 'objEncripta.Decrypt(System.Configuration.ConfigurationManager.AppSettings("UsuarioWS"))
                pToken = Me.TokenSitio 'objEncripta.Decrypt(System.Configuration.ConfigurationManager.AppSettings("TokenWS"))
                Me.pMaquina = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList(0).ToString()
                'pMaquina = Me.MaqSitio 'System.Web.HttpContext.Current.Application("MaquinaWS") 'System.Web.HttpContext.Current.Server.MachineName

                'parametros tomados del webservices porque es el que ejecuta la transaccion
                Me.pAplicacion = CType(System.Configuration.ConfigurationManager.AppSettings("IdAplicacion"), Integer)
                Me.pEmpresa = CType(System.Configuration.ConfigurationManager.AppSettings("IdEmpresa"), Integer)
                Me.pSucursal = CType(System.Configuration.ConfigurationManager.AppSettings("IdSucursal"), Integer)

            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "SetDefaultSitio", pCodError, pMsgError)
                Throw New Exception("Retorno Error")
            Finally
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "SetDefaultSitio")
            End Try

        End Sub
        Private Sub setParamBase()
            Try
                pServidorBD = System.Configuration.ConfigurationManager.AppSettings("ServidorBD")
                pProveedorBD = System.Configuration.ConfigurationManager.AppSettings("ProveedorBD")
                Try
                    pUsuarioBD = System.Configuration.ConfigurationManager.AppSettings("UsuarioBD")
                Catch ex As Exception
                    pUsuarioBD = ""
                End Try
                Try
                    pClaveBD = System.Configuration.ConfigurationManager.AppSettings("ClaveBD")
                Catch ex As Exception
                    pClaveBD = ""
                End Try
                BaseSeguridad = System.Configuration.ConfigurationManager.AppSettings("BaseSeguridad")
                'Aumento Min y Max Pool
                pMinPool = System.Configuration.ConfigurationManager.AppSettings("MinPool")
                pMaxPool = System.Configuration.ConfigurationManager.AppSettings("MaxPool")
                pAppBD = System.Configuration.ConfigurationManager.AppSettings("AppBD")
                Try
                    pConnectionTimeOut = System.Configuration.ConfigurationManager.AppSettings("ConnectionTimeOut")
                    If pConnectionTimeOut Is Nothing Then
                        pConnectionTimeOut = ""
                    End If
                Catch ex As Exception
                    pConnectionTimeOut = ""
                End Try
                Me.BaseDatos = ""
                Me.pServidorBD_Ext = ""
                Me.pUsuarioBD_Ext = ""
                Me.pClaveBD_Ext = ""
                Me.pAppBD_Ext = ""
                Me.pMaxPool_Ext = ""
                Me.pMinPool_Ext = ""
                Me.pTipoServidorBD_Ext = ""
                Me.pProveedorBD_Ext = ""
                Me.pPuertoBD_Ext = ""
                Me.pBaseDatos_Ext = ""
                pConnectionTimeOut_Ext = ""
            Catch ex As Exception

            End Try
        End Sub
#End Region
#Region "Metodos de Bases Externas"
        Public Function Exec_Query_ReaderBA(ByVal query As String, ByVal PI_IdAplicacion As Integer) As XmlDocument
            Dim dr As SqlDataReader
            Me.pCodError = 0
            Me.pMsgError = ""
            Dim xml As New XmlDocument
            xml.LoadXml("<Trans />")
            xml.DocumentElement.SetAttribute("Query", query)
            Try
                'Setea lo que se audita de los datos
                txtTrans = query 'asume esto como parametro
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_Query_ReaderBA", txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Me.Organizacion, Me.Transaccion, Me.Opcion, "", "", txtTrans)

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If
                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    objSybase = New clsBaseDatosSybase(strcnn)
                        '    xml = objSybase.Exec_Query_Reader(query)
                        'Case "20" 'oracle
                        '    objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    xml = objOtro.Exec_Query_Reader(query)
                        Case Else 'sqlserver 2,6
                            Me.Cnn = New SqlConnection(strcnn)
                            Me.Cnn.Open()
                            cmd = New SqlCommand
                            cmd.Connection = Cnn
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = query
                            cmd.CommandType = CommandType.Text
                            dr = cmd.ExecuteReader()
                            xml = GeneraXMLdr(dr)
                            dr.Close()
                            Me.GeneraXmlOut(xml)
                            Cnn.Close()
                            Cnn.Dispose()
                    End Select
                End If

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_Query_ReaderBA", pCodError, pMsgError + " " + query)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_Query_ReaderBA", pCodError, pMsgError + " " + query)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_Query_ReaderBA", xml.OuterXml)
            End Try

            Return xml
        End Function
        Public Function Exec_QueryBA(ByVal query As String, ByVal PI_IdAplicacion As Integer) As DataSet
            Dim da As SqlDataAdapter = New SqlDataAdapter
            Dim ds As DataSet = New DataSet
            Me.pCodError = 0
            Me.pMsgError = ""
            Dim xml As New XmlDocument
            xml.LoadXml("<Trans />")
            xml.DocumentElement.SetAttribute("Query", query)
            Try
                'Setea lo que se audita de los datos
                txtTrans = query 'asume esto como parametro
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_QueryBA", txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Me.Organizacion, Me.Transaccion, Me.Opcion, "", "", txtTrans)

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If
                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    objSybase = New clsBaseDatosSybase(strcnn)
                        '    ds = objSybase.Exec_Query(query)
                        'Case "20" 'oracle
                        '    objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    ds = objOtro.Exec_Query(query)
                        Case Else 'sqlserver 2,6
                            Me.Cnn = New SqlConnection(strcnn)
                            Me.Cnn.Open()
                            cmd = New SqlCommand
                            cmd.Connection = Cnn
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = query
                            cmd.CommandType = CommandType.Text
                            da.SelectCommand = cmd
                            da.Fill(ds, "consulta")
                            Cnn.Close()
                            Cnn.Dispose()
                    End Select
                End If

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_QueryBA", pCodError, pMsgError + " " + query)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_QueryBA", pCodError, pMsgError + " " + query)
            Finally
                GeneraXMLds(ds)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_QueryBA", ds.GetXml)
            End Try

            Return ds
        End Function
        Public Function Exec_QueryBA(ByVal query As String, ByVal dsname As String, ByVal PI_IdAplicacion As Integer) As DataSet
            Dim da As SqlDataAdapter = New SqlDataAdapter
            Dim ds As DataSet = New DataSet
            Me.pCodError = 0
            Me.pMsgError = ""
            Dim xml As New XmlDocument
            xml.LoadXml("<Trans />")
            xml.DocumentElement.SetAttribute("Query", query)
            Try
                'Setea lo que se audita de los datos
                txtTrans = query 'asume esto como parametro
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_QueryBA", txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Me.Organizacion, Me.Transaccion, Me.Opcion, "", "", txtTrans)

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If
                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    objSybase = New clsBaseDatosSybase(strcnn)
                        '    ds = objSybase.Exec_Query(query, dsname)
                        'Case "20" 'oracle
                        '    objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    ds = objOtro.Exec_Query(query, dsname)
                        Case Else 'sqlserver 2,6
                            Me.Cnn = New SqlConnection(strcnn)
                            Me.Cnn.Open()
                            cmd = New SqlCommand
                            cmd.Connection = Cnn
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = query
                            cmd.CommandType = CommandType.Text
                            da.SelectCommand = cmd
                            da.Fill(ds, dsname)
                            Cnn.Close()
                            Cnn.Dispose()
                    End Select
                End If

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_QueryBA", pCodError, pMsgError + " " + query)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_QueryBA", pCodError, pMsgError + " " + query)
            Finally
                GeneraXMLds(ds)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_QueryBA", ds.GetXml)
            End Try

            Return ds
        End Function
        Public Function Exec_SP_NoQueryBA(ByVal Organizacion As Integer, _
                           ByVal CodTran As Integer, _
                           ByVal CodOpcion As Integer, _
                           ByVal arrParam As Array, _
                           ByVal ParamAut As String, _
                           ByVal ValorAut As String, _
                           ByVal txtTrans As String, _
                           ByVal PI_IdAplicacion As Integer) As XmlDocument

            Me.pMsgError = ""
            'Dim elem As XmlElement
            Dim xml As New XmlDocument
            Dim root As XmlElement
            xml.LoadXml("<Regitro/>")
            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SP_NoQueryBA", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If
                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    objSybase = New clsBaseDatosSybase(strcnn)
                        '    xml = objSybase.Exec_SP_NoQuery(xmlSP, arrParam)
                        'Case "20" 'oracle
                        '    objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    xml = objOtro.Exec_SP_NoQuery(xmlSP, arrParam)
                        Case Else 'sqlserver 2,6
                            Me.Cnn = New SqlConnection(strcnn)
                            Me.Cnn.Open()
                            Dim xmlDoc As XmlDocument = New XmlDocument
                            xmlDoc.LoadXml(xmlSP)
                            root = xmlDoc.DocumentElement
                            nameSP = root.GetAttribute("nombre")
                            cmd = New SqlCommand
                            cmd.Connection = Cnn
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = nameSP
                            cmd.CommandType = CommandType.StoredProcedure
                            'Genera Parametros al comando
                            GeneraParametros(root, arrParam)
                            If pCodError = 0 Then
                                cmd.ExecuteNonQuery()
                                Me.GeneraXmlOut(xml)
                            End If
                            Cnn.Close()
                            Cnn.Dispose()
                    End Select
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_NoQueryBA", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_NoQueryBA", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP_NoQueryBA")
            End Try

            'Si esta habilitado el monitoreo, se registra
            'If TraceTrans.Equals("S") Then
            '    XmlSalida = New XmlDocument
            '    XmlSalida.LoadXml(xml.OuterXml)
            '    'Registra Monitoreo en Base
            '    RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            'End If
            Return xml

        End Function
        Public Function Exec_SP_ParamOutBA(ByVal Organizacion As Integer, _
                                ByVal CodTran As Integer, _
                                ByVal CodOpcion As Integer, _
                                ByVal arrParam As Array, _
                                ByVal ParamAut As String, _
                                ByVal ValorAut As String, _
                                ByVal txtTrans As String, _
                                ByVal PI_IdAplicacion As Integer) As XmlDocument

            Me.pCodError = 0
            Me.pMsgError = ""
            Dim xml As New XmlDocument
            xml.LoadXml("<Regitro/>")
            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SP_ParamOutBA", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If
                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    objSybase = New clsBaseDatosSybase(strcnn)
                        '    xml = objSybase.Exec_SP_ParamOut(xmlSP, arrParam)
                        'Case "20" 'oracle
                        '    objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    xml = objOtro.Exec_SP_ParamOut(xmlSP, arrParam)
                        Case Else 'sqlserver 2,6
                            Me.Cnn = New SqlConnection(strcnn)
                            Me.Cnn.Open()
                            Dim xmlDoc As XmlDocument = New XmlDocument
                            xmlDoc.LoadXml(xmlSP)
                            Dim root As XmlElement = xmlDoc.DocumentElement
                            nameSP = root.GetAttribute("nombre")
                            cmd = New SqlCommand
                            cmd.Connection = Cnn
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = nameSP
                            cmd.CommandType = CommandType.StoredProcedure
                            'Genera Parametros al comando
                            GeneraParametros(root, arrParam)
                            If pCodError = 0 Then
                                cmd.ExecuteNonQuery()
                                'Retorna los valores de salida
                                Me.GeneraXmlOut(xml)
                            End If
                            Cnn.Close()
                            Cnn.Dispose()
                    End Select

                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_ParamOutBA", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_ParamOutBA", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP_ParamOutBA", xml.OuterXml)
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(xml.OuterXml)
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If

            'Catch ex As Exception

            'End Try

            Return xml

        End Function
        Public Function Exec_SP_ReaderBA(ByVal Organizacion As Integer, _
                                       ByVal CodTran As Integer, _
                                       ByVal CodOpcion As Integer, _
                                       ByVal arrParam As Array, _
                                       ByVal ParamAut As String, _
                                       ByVal ValorAut As String, _
                                       ByVal txtTrans As String, _
                                       ByVal PI_IdAplicacion As Integer) As XmlDocument

            Dim dr As SqlDataReader
            Dim xml As New XmlDocument
            Me.pCodError = 0
            Me.pMsgError = ""
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")

            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SP_ReaderBA", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If

                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    objSybase = New clsBaseDatosSybase(strcnn)
                        '    xml = objSybase.Exec_SP_Reader(xmlSP, arrParam)
                        'Case "20" 'oracle
                        '    objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    xml = objOtro.Exec_SP_Reader(xmlSP, arrParam)
                        Case Else 'sqlserver 2,6
                            Me.Cnn = New SqlConnection(strcnn)
                            Me.Cnn.Open()
                            Dim xmlDoc As XmlDocument = New XmlDocument
                            xmlDoc.LoadXml(xmlSP)
                            Dim root As XmlElement = xmlDoc.DocumentElement
                            nameSP = root.GetAttribute("nombre")
                            cmd = New SqlCommand
                            cmd.Connection = Cnn
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = nameSP
                            cmd.CommandType = CommandType.StoredProcedure
                            'Genera Parametros al comando
                            GeneraParametros(root, arrParam)
                            If pCodError = 0 Then
                                dr = cmd.ExecuteReader()
                                xml = GeneraXMLdr(dr)
                                dr.Close()
                                Me.GeneraXmlOut(xml)
                            End If
                            Cnn.Close()
                            Cnn.Dispose()
                    End Select
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_ReaderBA", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_ReaderBA", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP_ReaderBA", xml.OuterXml)
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(xml.OuterXml)
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If

            'Catch ex As Exception

            'End Try

            Return xml

        End Function
        Public Function Exec_SP_ReaderBA_Trans(ByVal Organizacion As Integer, _
                                       ByVal CodTran As Integer, _
                                       ByVal CodOpcion As Integer, _
                                       ByVal arrParam As Array, _
                                       ByVal ParamAut As String, _
                                       ByVal ValorAut As String, _
                                       ByVal txtTrans As String, _
                                       ByVal PI_IdAplicacion As Integer) As XmlDocument

            Dim dr As SqlDataReader
            Dim xml As New XmlDocument
            Me.pCodError = 0
            Me.pMsgError = ""
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")

            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SP_ReaderBA", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If

                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    If IsNothing(objSybase) Then
                        '        objSybase = New clsBaseDatosSybase(strcnn)
                        '    End If
                        '    objSybase.pOpcionTrans = Me.pOpcionTrans
                        '    xml = objSybase.Exec_SP_Reader_Trans(xmlSP, arrParam)
                        'Case "20" 'oracle
                        '    If IsNothing(objOtro) Then
                        '        objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    End If
                        '    objOtro.pOpcionTrans = Me.pOpcionTrans
                        '    xml = objOtro.Exec_SP_Reader_Trans(xmlSP, arrParam)
                        Case Else 'sqlserver 2,6
                            cmd = New SqlCommand
                            Select Case Me.OpcionTrans
                                Case "N"
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                                Case "B"
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                                    Me.Trans = Me.Cnn.BeginTransaction
                                    cmd.Transaction = Me.Trans
                                Case "T"
                                    cmd.Connection = Me.Cnn
                                    cmd.Transaction = Me.Trans
                                Case Else
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                            End Select
                            Dim xmlDoc As XmlDocument = New XmlDocument
                            xmlDoc.LoadXml(xmlSP)
                            Dim root As XmlElement = xmlDoc.DocumentElement
                            nameSP = root.GetAttribute("nombre")
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = nameSP
                            cmd.CommandType = CommandType.StoredProcedure
                            'Genera Parametros al comando
                            GeneraParametros(root, arrParam)
                            If pCodError = 0 Then
                                dr = cmd.ExecuteReader()
                                xml = GeneraXMLdr(dr)
                                dr.Close()
                                Me.GeneraXmlOut(xml)
                            End If
                    End Select
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_ReaderBA", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_ReaderBA", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP_ReaderBA", xml.OuterXml)
            End Try

            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(xml.OuterXml)
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If

            'Catch ex As Exception

            'End Try

            Return xml

        End Function
        Public Function Exec_SP_ReaderBA_Trans(ByVal Query As String, _
                                       ByVal PI_IdAplicacion As Integer) As XmlDocument

            Dim dr As SqlDataReader
            Dim xml As New XmlDocument
            Me.pCodError = 0
            Me.pMsgError = ""
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")

            Try
                'Setea lo que se audita de los datos
                txtTrans = Query
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SP_ReaderBA_Trans", txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Me.Organizacion, Me.Transaccion, Me.Opcion, "", "", txtTrans)
                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If

                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    If IsNothing(objSybase) Then
                        '        objSybase = New clsBaseDatosSybase(strcnn)
                        '    End If
                        '    objSybase.pOpcionTrans = Me.pOpcionTrans
                        '    xml = objSybase.Exec_SP_Reader_Trans(Query)
                        'Case "20" 'oracle
                        '    If IsNothing(objOtro) Then
                        '        objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    End If
                        '    objOtro.pOpcionTrans = Me.pOpcionTrans
                        '    xml = objOtro.Exec_SP_Reader_Trans(Query)
                        Case Else 'sqlserver 2,6
                            cmd = New SqlCommand
                            Select Case Me.OpcionTrans
                                Case "N"
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                                Case "B"
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                                    Me.Trans = Me.Cnn.BeginTransaction
                                    cmd.Transaction = Me.Trans
                                Case "T"
                                    cmd.Connection = Me.Cnn
                                    cmd.Transaction = Me.Trans
                                Case Else
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                            End Select
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = Query
                            cmd.CommandType = CommandType.Text
                            If pCodError = 0 Then
                                dr = cmd.ExecuteReader()
                                xml = GeneraXMLdr(dr)
                                dr.Close()
                                Me.GeneraXmlOut(xml)
                            End If
                    End Select
                End If
            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_ReaderBA_Trans", pCodError, pMsgError + " " + Query)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SP_ReaderBA_Trans", pCodError, pMsgError + " " + Query)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SP_ReaderBA_Trans", xml.OuterXml)
            End Try

            Return xml

        End Function
        Public Function Exec_SPBA(ByVal Organizacion As Integer, _
                                       ByVal CodTran As Integer, _
                                       ByVal CodOpcion As Integer, _
                                       ByVal arrParam As Array, _
                                       ByVal ParamAut As String, _
                                       ByVal ValorAut As String, _
                                       ByVal txtTrans As String, _
                                       ByVal PI_IdAplicacion As Integer) As DataSet
            Dim ds As DataSet = New DataSet

            Dim da As SqlDataAdapter = New SqlDataAdapter
            Me.pCodError = 0
            Me.pMsgError = ""
            Dim p_Log As String = (CStr(System.Configuration.ConfigurationManager.AppSettings("RutaLog"))).Trim()
            Dim loge As Logger.Logger = New Logger.Logger()
            Try

                loge.FilePath = p_Log
                loge.WriteMensaje("Inicia Base")
                loge.Linea()

                loge.FilePath = p_Log
                loge.WriteMensaje("txtTrans: " + txtTrans)
                loge.Linea()

                For Each item As Object In arrParam
                    loge.FilePath = p_Log
                    loge.WriteMensaje("item: " + item.ToString())
                    loge.Linea()
                Next

                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                loge.FilePath = p_Log
                loge.WriteMensaje("Org: " + Organizacion.ToString() + " trans: " + CodTran.ToString() + " opc: " + CodOpcion.ToString() + " parAut: " + ParamAut + " valAut: " + ValorAut + " txtTrans: " + txtTrans)
                loge.Linea()


                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SPBA",
                                    ",Org=" + Organizacion.ToString() +
                                    ",trans=" + CodTran.ToString() +
                                    ",opc=" + CodOpcion.ToString() +
                                    ",parAut=" + ParamAut +
                                    ",valAut=" + ValorAut +
                                    ",txtTrans=" + txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                loge.FilePath = p_Log
                loge.WriteMensaje("xmlSP: " + xmlSP + " pCodError: " + pCodError.ToString())
                loge.Linea()

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If


                    loge.FilePath = p_Log
                    loge.WriteMensaje("strcnn: " + strcnn)
                    loge.Linea()

                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    objSybase = New clsBaseDatosSybase(strcnn)
                        '    ds = objSybase.Exec_SP(xmlSP, arrParam)
                        'Case "20" 'oracle
                        '    objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    ds = objOtro.Exec_SP(xmlSP, arrParam)
                        Case Else 'sqlserver 2,6
                            Me.Cnn = New SqlConnection(strcnn)
                            Me.Cnn.Open()
                            Dim xmlDoc As XmlDocument = New XmlDocument
                            xmlDoc.LoadXml(xmlSP)
                            Dim root As XmlElement = xmlDoc.DocumentElement
                            nameSP = root.GetAttribute("nombre")
                            cmd = New SqlCommand
                            cmd.Connection = Cnn
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = nameSP
                            cmd.CommandType = CommandType.StoredProcedure
                            'Genera Parametros al comando
                            GeneraParametros(root, arrParam)
                            If pCodError = 0 Then
                                da.SelectCommand = cmd
                                da.Fill(ds, nameSP)
                            End If
                            Cnn.Close()
                            Cnn.Dispose()
                    End Select
                End If

            Catch errSql As SqlException

                loge.FilePath = p_Log
                loge.WriteMensaje("errSql: " + errSql.Message)
                loge.Linea()

                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SPBA", pCodError, pMsgError +
                                    ",Org=" + Organizacion.ToString() +
                                    ",trans=" + CodTran.ToString() +
                                    ",opc=" + CodOpcion.ToString() +
                                    ",parAut=" + ParamAut +
                                    ",valAut=" + ValorAut +
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception

                loge.FilePath = p_Log
                loge.WriteMensaje("errSql: " + err.Message)
                loge.Linea()

                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SPBA", pCodError, pMsgError +
                                    ",Org=" + Organizacion.ToString() +
                                    ",trans=" + CodTran.ToString() +
                                    ",opc=" + CodOpcion.ToString() +
                                    ",parAut=" + ParamAut +
                                    ",valAut=" + ValorAut +
                                    ",txtTrans=" + txtTrans)
            Finally
                GeneraXMLds(ds)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SPBA", ds.GetXml)
            End Try
            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(ds.GetXml())
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If

            'Catch ex As Exception

            'End Try

            Return ds

        End Function
        Public Function Exec_SPBA_Trans(ByVal Organizacion As Integer, _
                               ByVal CodTran As Integer, _
                               ByVal CodOpcion As Integer, _
                               ByVal arrParam As Array, _
                               ByVal ParamAut As String, _
                               ByVal ValorAut As String, _
                               ByVal txtTrans As String, _
                               ByVal PI_IdAplicacion As Integer) As DataSet
            Dim ds As DataSet = New DataSet

            Dim da As SqlDataAdapter = New SqlDataAdapter
            Me.pCodError = 0
            Me.pMsgError = ""

            Try
                'Setea lo que se audita de los datos
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SPBA", _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If
                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    If IsNothing(objSybase) Then
                        '        objSybase = New clsBaseDatosSybase(strcnn)
                        '    End If
                        '    objSybase.pOpcionTrans = Me.pOpcionTrans
                        '    ds = objSybase.Exec_SP_Trans(xmlSP, arrParam)
                        'Case "20" 'oracle
                        '    If IsNothing(objOtro) Then
                        '        objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    End If
                        '    objOtro.pOpcionTrans = Me.pOpcionTrans
                        '    ds = objOtro.Exec_SP_Trans(xmlSP, arrParam)
                        Case Else 'sqlserver 2,6
                            'Logica para ver si conexion Ext como se abre
                            'para controlar transaccion
                            cmd = New SqlCommand
                            Select Case Me.OpcionTrans
                                Case "N"
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                                Case "B"
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                                    Me.Trans = Me.Cnn.BeginTransaction
                                    cmd.Transaction = Me.Trans
                                Case "T"
                                    cmd.Connection = Me.Cnn
                                    cmd.Transaction = Me.Trans
                                Case Else
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                            End Select
                            Dim xmlDoc As XmlDocument = New XmlDocument
                            xmlDoc.LoadXml(xmlSP)
                            Dim root As XmlElement = xmlDoc.DocumentElement

                            nameSP = root.GetAttribute("nombre")
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = nameSP
                            cmd.CommandType = CommandType.StoredProcedure
                            'Genera Parametros al comando
                            GeneraParametros(root, arrParam)
                            If pCodError = 0 Then
                                da.SelectCommand = cmd
                                da.Fill(ds, nameSP)
                            End If
                    End Select
                End If

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SPBA", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SPBA", pCodError, pMsgError + _
                                    ",Org=" + Organizacion.ToString() + _
                                    ",trans=" + CodTran.ToString() + _
                                    ",opc=" + CodOpcion.ToString() + _
                                    ",parAut=" + ParamAut + _
                                    ",valAut=" + ValorAut + _
                                    ",txtTrans=" + txtTrans)
            Finally
                GeneraXMLds(ds)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SPBA", ds.GetXml)
            End Try
            'Try
            '    'Si esta habilitado el monitoreo, se registra
            '    If TraceTrans.Equals("S") Then
            '        XmlSalida = New XmlDocument
            '        XmlSalida.LoadXml(ds.GetXml())
            '        'Registra Monitoreo en Base
            '        RegTraceTrans(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut)
            '    End If

            'Catch ex As Exception

            'End Try

            Return ds

        End Function
        Public Function Exec_SPBA_Trans(ByVal Query As String, _
                               ByVal PI_IdAplicacion As Integer) As DataSet
            Dim ds As DataSet = New DataSet
            Dim xml As New XmlDocument
            Dim da As SqlDataAdapter = New SqlDataAdapter
            Me.pCodError = 0
            Me.pMsgError = ""
            Xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Regitro/>")

            Try
                'Setea lo que se audita de los datos
                txtTrans = Query
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "Exec_SPBA_Trans", _
                                    txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Me.Organizacion, Me.Transaccion, Me.Opcion, "", "", txtTrans)

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If
                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    If IsNothing(objSybase) Then
                        '        objSybase = New clsBaseDatosSybase(strcnn)
                        '    End If
                        '    objSybase.pOpcionTrans = Me.pOpcionTrans
                        '    ds = objSybase.Exec_SP_Trans(Query)
                        'Case "20" 'oracle
                        '    If IsNothing(objOtro) Then
                        '        objOtro = New clsBaseDatosOLEDB(strcnn)
                        '    End If
                        '    objOtro.pOpcionTrans = Me.pOpcionTrans
                        '    ds = objOtro.Exec_SP_Trans(Query)
                        Case Else 'sqlserver 2,6
                            'Logica para ver si conexion Ext como se abre
                            'para controlar transaccion
                            cmd = New SqlCommand
                            Select Case Me.OpcionTrans
                                Case "N"
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                                Case "B"
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                                    Me.Trans = Me.Cnn.BeginTransaction
                                    cmd.Transaction = Me.Trans
                                Case "T"
                                    cmd.Connection = Me.Cnn
                                    cmd.Transaction = Me.Trans
                                Case Else
                                    Me.Cnn = New SqlConnection(strcnn)
                                    Me.Cnn.Open()
                                    cmd.Connection = Me.Cnn
                            End Select
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = Query
                            cmd.CommandType = CommandType.Text
                            If pCodError = 0 Then
                                da.SelectCommand = cmd
                                da.Fill(ds)
                            End If
                    End Select
                End If

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SPBA_Trans", pCodError, pMsgError + " " + Query)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "Exec_SPBA_Trans", pCodError, pMsgError + " " + Query)
            Finally
                GeneraXMLds(ds)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "Exec_SPBA_Trans", ds.GetXml)
            End Try
            
            Return ds

        End Function
        Public Function VerificaPermisoSP(ByVal query As String, ByVal PI_IdAplicacion As Integer) As XmlDocument
            Dim dr As SqlDataReader
            Me.pCodError = 0
            Me.pMsgError = ""
            Dim xml As New XmlDocument
            xml.LoadXml("<Trans />")
            xml.DocumentElement.SetAttribute("Query", query)
            Try
                'Setea lo que se audita de los datos
                txtTrans = query
                txtTrans = settxtTrans(arrParam, txtTrans)

                clsError.setMetodo(True, "clsBaseDatosSQL", "VerificaPermisoSP", txtTrans)
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Me.Organizacion, Me.Transaccion, Me.Opcion, "", "", txtTrans)

                If Me.pCodError = 0 Then
                    If PI_IdAplicacion < 0 Then 'toma el IdServidor definido en la transaccion
                        strcnn = LeeConexion_Ext(pIdServidor_Ext)
                    Else 'toma el IdServidor del parametro de entrada
                        strcnn = LeeConexion_Ext(PI_IdAplicacion)
                    End If
                    Select Case Me.TipoServidorBD_Ext
                        'Case "3" 'sybase
                        '    objSybase = New clsBaseDatosSybase(strcnn)
                        '    xml = objSybase.VerificaPermisoSP(query)
                        Case Else 'sqlserver 2,6
                            Me.Cnn = New SqlConnection(strcnn)
                            Me.Cnn.Open()
                            'Primero se ponel en modo de solo Plan
                            Me.Trans = Me.Cnn.BeginTransaction
                            cmd = New SqlCommand
                            cmd.Connection = Me.Cnn
                            cmd.Transaction = Me.Trans
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandText = "SET SHOWPLAN_TEXT ON"
                            cmd.CommandType = CommandType.Text
                            cmd.ExecuteNonQuery()
                            'Servidor Tipo SQL 2000 no permite ejecutar SP sin parametros
                            'dmunoz 11 Febrero 2010
                            If Me.TipoServidorBD_Ext = 2 Then
                                xml.LoadXml("<Resultado />")
                            Else
                                'Ejecuta el SP
                                cmd = New SqlCommand
                                cmd.Connection = Me.Cnn
                                cmd.Transaction = Me.Trans
                                If pConnectionTimeOut_Ext.Length > 0 Then
                                    cmd.CommandTimeout = pConnectionTimeOut_Ext
                                End If
                                cmd.CommandText = query
                                cmd.CommandType = CommandType.Text
                                dr = cmd.ExecuteReader()
                                If dr.Read Then
                                    xml.LoadXml("<Resultado />")
                                    dr.Close()
                                End If
                            End If
                            'Reversamos el modo solo plan
                            cmd = New SqlCommand
                            cmd.Connection = Me.Cnn
                            cmd.Transaction = Me.Trans
                            cmd.CommandText = "SET SHOWPLAN_TEXT OFF"
                            If pConnectionTimeOut_Ext.Length > 0 Then
                                cmd.CommandTimeout = pConnectionTimeOut_Ext
                            End If
                            cmd.CommandType = CommandType.Text
                            cmd.ExecuteNonQuery()
                            'Finaliza transaccion y libera conexion
                            Trans.Commit()
                            Trans.Dispose()
                            Cnn.Close()
                            Cnn.Dispose()
                    End Select
                End If

            Catch errSql As SqlException
                pCodError = errSql.Number
                pMsgError = errSql.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "VerificaPermisoSP", pCodError, pMsgError + " " + query)
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                CierraCnn(Cnn)
                pMsgError = clsError.setMensaje("clsBaseDatosSQL", "VerificaPermisoSP", pCodError, pMsgError + " " + query)
            Finally
                xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
                xml.DocumentElement.SetAttribute("MsgError", MsgError)
                'Registra inicio en trace
                clsError.setMetodo(False, "clsBaseDatosSQL", "VerificaPermisoSP", xml.OuterXml)
            End Try

            Return xml
        End Function

#End Region
    End Class

End Namespace

