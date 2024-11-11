Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Xml


Namespace Seguridad
    Public Class clsBaseDatos

        'propiedades de OleDb
        Private pcnn As OleDbConnection
        Private ptrans As OleDbTransaction
        Private cmd As OleDbCommand

        'Propuedades de Conexion a Base de Datos
        '  Private usuarioBD As String 'Encriptado
        ' Private claveBD As String   'Encriptado
        Protected servidorBD As String
        Protected proveedorBD As String
        Protected UsuarioBD, ClaveBD As String

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

        Public Property pUsuarioBD() As String
            Get
                Return Me.usuarioBD
            End Get
            Set(ByVal value As String)
                Me.usuarioBD = value
            End Set
        End Property
        Public Property pServidorBD() As String
            Get
                Return Me.servidorBD
            End Get
            Set(ByVal value As String)
                Me.servidorBD = value
            End Set
        End Property
        Public Property pProveedorBD() As String
            Get
                Return Me.proveedorBD
            End Get
            Set(ByVal value As String)
                Me.proveedorBD = value
            End Set
        End Property
        Public Property pClaveBD() As String
            Get
                Return Me.claveBD
            End Get
            Set(ByVal value As String)
                Me.claveBD = value
            End Set
        End Property

        'Propiedad de licencia del Sitio
        Public ReadOnly Property objLicencia() As clsLicencia
            Get
                Return System.Web.HttpContext.Current.Application("objLicencia")
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

        Public Property ptxtTrans() As String
            Get
                Return Me.txtTrans
            End Get
            Set(ByVal value As String)
                Me.txtTrans = value
            End Set
        End Property

        Public Sub New()
            pOpcionTrans = "N"
        End Sub

        Public Property Cnn() As OleDbConnection
            Get
                Return Me.pcnn
            End Get
            Set(ByVal value As OleDbConnection)
                Me.pcnn = value
            End Set
        End Property

        Public Property Trans() As OleDbTransaction
            Get
                Return Me.ptrans
            End Get
            Set(ByVal value As OleDbTransaction)
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


        Private Function LeeConexion() As String
            Dim txtconn As System.Text.StringBuilder = New System.Text.StringBuilder

            Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
            Dim UserBD As String = Me.UsuarioBD
            UserBD = objEncripta.Decrypt(UserBD)
            Dim pwdBD As String = Me.ClaveBD
            pwdBD = objEncripta.Decrypt(pwdBD)

            txtconn.Append("user id=").Append(UserBD)
            txtconn.Append(";password=").Append(pwdBD)
            'txtconn.Append(";Database=").Append("master")
            ' txtconn.Append("integrated security=SSPI")
            txtconn.Append(";Server=").Append(Me.servidorBD)
            txtconn.Append(";provider=").Append(Me.proveedorBD)
            txtconn.Append(";connection reset=false")
            txtconn.Append(";connection lifetime=1")
            txtconn.Append(";min pool size=0")
            txtconn.Append(";max pool size=50")

            Return (txtconn.ToString())

        End Function

        Private Function LeeConexion(ByVal PI_Servidor As String, ByVal PI_Proveedor As String, _
                                     ByVal PI_Usuario As String, ByVal PI_Clave As String) As String
            Dim txtconn As System.Text.StringBuilder = New System.Text.StringBuilder

            'Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
            'Dim UserBD As String = Me.usuarioBD
            'UserBD = objEncripta.Decrypt(UserBD)
            'Dim pwdBD As String = Me.claveBD
            'pwdBD = objEncripta.Decrypt(pwdBD)
            If PI_Usuario.Equals("") Then
                txtconn.Append("integrated security=SSPI")
            Else
                txtconn.Append("user id=").Append(PI_Usuario)
                txtconn.Append(";password=").Append(PI_Clave)
            End If
            'txtconn.Append(";Database=").Append("master")
            txtconn.Append(";Server=").Append(PI_Servidor)
            txtconn.Append(";provider=").Append(PI_Proveedor)
            txtconn.Append(";connection reset=false")
            txtconn.Append(";connection lifetime=1")
            txtconn.Append(";min pool size=0")
            txtconn.Append(";max pool size=50")

            Return (txtconn.ToString())

        End Function

        'Public Sub getLicEmpresa()
        '    Me.pCodError = 0
        '    Me.pMsgError = ""

        '    Try

        '        If Me.objLicencia.plicencia Then

        '            Me.Cnn = New OleDbConnection(LeeConexion())
        '            Me.Cnn.Open()

        '            cmd = New OleDbCommand
        '            cmd.Connection = Me.Cnn
        '            cmd.CommandType = CommandType.StoredProcedure
        '            cmd.CommandText = "Sige_Seguridad..seg_p_getLicEmpresa"

        '            Dim param As OleDbParameter

        '            param = New OleDbParameter("@IdEmpresa", System.Data.OleDb.OleDbType.Integer, 4)
        '            param.Direction = System.Data.ParameterDirection.Input
        '            param.Value = Me.pEmpresa
        '            cmd.Parameters.Add(param)

        '            'Retorna el nombre del stored procedure y sus parametros
        '            Dim dr As OleDbDataReader = cmd.ExecuteReader()
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
        '        Cnn.Close()
        '    End Try

        '    'Por que no esta controlando licencia
        '    licenciaEmpresa = True

        'End Sub

        Private Function getLicModuloEmp(ByVal Organizacion As Integer) As Boolean
            Dim item As Integer
            For Each item In Me.modulo
                If item = Organizacion Then
                    Return True
                End If
            Next

            'Return False
            'No esta controlando licencia
            Return True

        End Function
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

            Try

                If Me.objLicencia.plicencia Then

                    'Transacciones iniciales que no necesitan verificacion
                    'de empresa licenciada
                    'Por ahora nose controla licencia de empresa
                    'If Not (Organizacion = 2 And CodTrans >= 1000 And CodTrans < 2000) Then
                    '    If Not Me.plicenciaEmpresa Then
                    '        Throw New Exception("La Empresa no tiene licencia valida")
                    '    End If
                    '    If Not Me.getLicModuloEmp(Organizacion) Then
                    '        Throw New Exception("Este modulo no esta licenciado para esta empresa")
                    '    End If
                    'End If

                    'Genera el log de transaccion que se va a guardar en auditoria
                    'txtTrans = txtTrans + CodTrans.ToString() + ";" + CodOpcion.ToString() + ";" + Organizacion.ToString() _
                    '         + ";" + Me.empresa.ToString() + ";" + Me.sucursal.ToString()

                    cmd = New OleDbCommand

                    Select Case Me.OpcionTrans
                        Case "N"
                            Me.Cnn = New OleDbConnection(LeeConexion())
                            Me.Cnn.Open()
                            cmd.Connection = Me.Cnn
                        Case "B"
                            Me.Cnn = New OleDbConnection(LeeConexion())
                            Me.Cnn.Open()
                            cmd.Connection = Me.Cnn
                            Me.Trans = Me.Cnn.BeginTransaction
                            cmd.Transaction = Me.Trans
                        Case "T"
                            cmd.Connection = Me.Cnn
                            cmd.Transaction = Me.Trans
                        Case Else
                            Me.Cnn = New OleDbConnection(LeeConexion())
                            Me.Cnn.Open()
                            cmd.Connection = Me.Cnn
                    End Select

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "Sige_Seguridad..seg_p_permiso_transaccion"

                    Dim param As OleDbParameter

                    param = New OleDbParameter("@PV_UsrSitio", System.Data.OleDb.OleDbType.VarChar, 20)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = UsrSitio
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_TokenSitio", System.Data.OleDb.OleDbType.VarChar, 32)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = TokenSitio
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_MaqSitio", System.Data.OleDb.OleDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = MaqSitio
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_idUsuario", System.Data.OleDb.OleDbType.VarChar, 20)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = usuario
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_Token", System.Data.OleDb.OleDbType.VarChar, 32)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = token
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_Maquina", System.Data.OleDb.OleDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = maquina
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_idAplicacion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = aplicacion
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_idOrganizacion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Organizacion
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdTransaccion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = CodTrans
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdOpcion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = CodOpcion
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdEmpresa", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Me.empresa
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdSucursal", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Me.sucursal
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_ParamAut", System.Data.OleDb.OleDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = ParamAut
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_Valor", System.Data.OleDb.OleDbType.VarChar, 50)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = ValorAut
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("", System.Data.OleDb.OleDbType.VarChar, 8000)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = txtTrans 'Contiene el valor de la Cadena que se va  aregistrar en Auditoria
                    cmd.Parameters.Add(param)

                    'Retorna el nombre del stored procedure y sus parametros
                    Dim dr As OleDbDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        xmlParam = dr(0).ToString()
                    End If

                    dr.Close()
                    cmd.Dispose()

                Else
                    Throw New Exception("El servidor no tiene licencia valida")
                End If


            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()

            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

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

                If Me.objLicencia.plicencia Then

                    cmd = New OleDbCommand


                    Me.Cnn = New OleDbConnection(LeeConexion())
                    Me.Cnn.Open()
                    cmd.Connection = Me.Cnn

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "Sige_Seguridad..seg_p_permiso_transaccion"

                    Dim param As OleDbParameter

                    param = New OleDbParameter("@PV_UsrSitio", System.Data.OleDb.OleDbType.VarChar, 20)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = UsrSitio
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_TokenSitio", System.Data.OleDb.OleDbType.VarChar, 32)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = TokenSitio
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_MaqSitio", System.Data.OleDb.OleDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = MaqSitio
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_idUsuario", System.Data.OleDb.OleDbType.VarChar, 20)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = usuario
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_Token", System.Data.OleDb.OleDbType.VarChar, 32)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = token
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_Maquina", System.Data.OleDb.OleDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = maquina
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_idAplicacion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = aplicacion
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_idOrganizacion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Organizacion
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdTransaccion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = CodTrans
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdOpcion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = CodOpcion
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdEmpresa", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Me.empresa
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdSucursal", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = Me.sucursal
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_ParamAut", System.Data.OleDb.OleDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = ParamAut
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_Valor", System.Data.OleDb.OleDbType.VarChar, 50)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = ValorAut
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("", System.Data.OleDb.OleDbType.VarChar, 2000)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = txtTrans 'Contiene el valor de la Cadena que se va  aregistrar en Auditoria
                    cmd.Parameters.Add(param)

                    'Retorna el nombre del stored procedure y sus parametros
                    Dim dr As OleDbDataReader = cmd.ExecuteReader()
                    dr.Close()
                    cmd.Dispose()
                    Cnn.Close()
                    permiso = True
                Else
                    Throw New Exception("El servidor no tiene licencia valida")
                End If
            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
            End Try
            Return permiso

        End Function

        Public Function VerificaTrans(ByVal PI_IdEmpresa As Integer, _
                                ByVal PI_IdSucursal As Integer, _
                                ByVal PI_Organizacion As Integer, _
                                ByVal PI_CodTrans As Integer, _
                                ByVal PI_CodOpcion As Integer, _
                                ByVal PI_ParamAut As String, _
                                ByVal PI_ValorAut As String) As Boolean
            'Verifica que la transaccion tiene permisos y retorna un
            'logico de verdadero si tiene permisos y falso si no tiene permisos
            'SI PI_IdSucursal = 0 , No valida por localidad, solo por Empresa

            Dim xmlParam As String = ""
            Me.pCodError = 0
            Me.pMsgError = ""
            Dim retorno As Boolean = False


            Try

                If Me.objLicencia.plicencia Then

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

                    cmd = New OleDbCommand
                    Me.Cnn = New OleDbConnection(LeeConexion())
                    Me.Cnn.Open()
                    cmd.Connection = Me.Cnn

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "Sige_Seguridad..seg_p_verifica_transaccion"

                    Dim param As OleDbParameter

                    param = New OleDbParameter("@PV_idUsuario", System.Data.OleDb.OleDbType.VarChar, 20)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = usuario
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_Token", System.Data.OleDb.OleDbType.VarChar, 32)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = token
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_Maquina", System.Data.OleDb.OleDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = maquina
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_idAplicacion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = aplicacion
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_idOrganizacion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_Organizacion
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdTransaccion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_CodTrans
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdOpcion", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_CodOpcion
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdEmpresa", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_IdEmpresa
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_IdSucursal", System.Data.OleDb.OleDbType.Integer, 4)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_IdSucursal
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_ParamAut", System.Data.OleDb.OleDbType.VarChar, 100)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_ParamAut
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PV_Valor", System.Data.OleDb.OleDbType.VarChar, 50)
                    param.Direction = System.Data.ParameterDirection.Input
                    param.Value = PI_ValorAut
                    cmd.Parameters.Add(param)

                    param = New OleDbParameter("@PO_CodRetorno", System.Data.OleDb.OleDbType.Boolean, 1)
                    param.Direction = System.Data.ParameterDirection.Output
                    cmd.Parameters.Add(param)

                    'Retorna el bit si tiene permisos
                    cmd.ExecuteNonQuery()
                    retorno = cmd.Parameters("@PO_CodRetorno").Value

                    cmd.Dispose()
                    Cnn.Close()

                Else
                    Throw New Exception("El servidor no tiene licencia valida")
                End If
            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()

            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

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
            Dim elem As XmlElement
            Dim xml As New XmlDocument
            xml.LoadXml("<Registro/>")
            Try
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New OleDbCommand
                    cmd.Connection = Cnn
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)

                    cmd.ExecuteNonQuery()
                    Cnn.Close()
                End If
            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()

            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try
            xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
            xml.DocumentElement.SetAttribute("MsgError", MsgError)
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

            Dim da As OleDbDataAdapter = New OleDbDataAdapter
            Me.pCodError = 0
            Me.pMsgError = ""

            Try
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New OleDbCommand
                    cmd.Connection = Cnn
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)

                    da.SelectCommand = cmd
                    da.Fill(ds, nameSP)
                    Cnn.Close()
                End If

            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()

            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try

            GeneraXMLds(ds)
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

            Dim da As OleDbDataAdapter = New OleDbDataAdapter

            Me.pCodError = 0
            Me.pMsgError = ""

            Try

                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New OleDbCommand
                    cmd.Connection = Cnn
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)

                    da.SelectCommand = cmd
                    da.Fill(ds, NombreDS)
                    Cnn.Close()
                End If

            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()

            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try

            GeneraXMLds(ds)
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
            xml.LoadXml("<Registro/>")
            Try
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New OleDbCommand
                    cmd.Connection = Cnn
                    cmd.Transaction = Me.Trans
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure
                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    cmd.ExecuteNonQuery()
                End If

            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                If Cnn.State = ConnectionState.Open Then
                    Cnn.Close()
                End If

            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                If Cnn.State = ConnectionState.Open Then
                    Cnn.Close()
                End If
            End Try
            xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
            xml.DocumentElement.SetAttribute("MsgError", MsgError)
            Return xml

        End Function
        Private Sub GeneraParametros(ByVal PI_rootXML As XmlElement, ByVal PI_arrParam As Array)
            Dim param As OleDbParameter
            Dim nodoParam As XmlNode
            For Each nodoParam In PI_rootXML.ChildNodes
                Dim elemParam As XmlElement = CType(nodoParam, XmlElement)
                Dim longitud As Integer
                If elemParam.GetAttribute("tipo").Equals("image") Then
                    longitud = UBound(PI_arrParam(CType(elemParam.GetAttribute("posicion"), Integer)))
                Else
                    longitud = CType(elemParam.GetAttribute("longitud"), Integer)
                End If

                param = New OleDbParameter(elemParam.GetAttribute("nombre"), _
                                        getTipo(elemParam.GetAttribute("tipo")), longitud)
                param.Direction = getDireccion(elemParam.GetAttribute("direccion"))
                If elemParam.GetAttribute("direccion").Equals("input") Then
                    param.Value = PI_arrParam(CType(elemParam.GetAttribute("posicion"), Integer))
                End If
                cmd.Parameters.Add(param)
            Next
        End Sub
        'Public Function Exec_SP_Tran_Reader(ByVal Organizacion As Integer, _
        '                 ByVal CodTran As Integer, _
        '                 ByVal CodOpcion As Integer, _
        '                 ByVal arrParam As Array, _
        '                 ByVal ParamAut As String, _
        '                 ByVal ValorAut As String, _
        '                 ByVal txtTrans As String) As OleDbDataReader

        '    Dim dr As OleDbDataReader

        '    Me.pCodError = 0
        '    Me.pMsgError = ""

        '    Try
        '        'Recupera Stored Procedure Autorizado y sus parametros
        '        Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
        '        Dim nameSP As String

        '        If Me.pCodError = 0 Then
        '            Dim xmlDoc As XmlDocument = New XmlDocument
        '            xmlDoc.LoadXml(xmlSP)

        '            Dim root As XmlElement = xmlDoc.DocumentElement
        '            nameSP = root.GetAttribute("nombre")

        '            cmd = New OleDbCommand
        '            cmd.Connection = Cnn
        '            cmd.Transaction = Me.Trans
        '            cmd.CommandText = nameSP
        '            cmd.CommandType = CommandType.StoredProcedure

        '            'Genera Parametros al comando
        '            GeneraParametros(root, arrParam)
        '            dr = cmd.ExecuteReader()
        '        End If

        '    Catch errSql As OleDbException
        '        pCodError = errSql.ErrorCode
        '        pMsgError = errSql.Message
        '        If Cnn.State = ConnectionState.Open Then
        '            Cnn.Close()
        '        End If

        '    Catch err As Exception
        '        pCodError = -100
        '        pMsgError = err.Message
        '        If Cnn.State = ConnectionState.Open Then
        '            Cnn.Close()
        '        End If

        '    End Try

        '    Return dr

        'End Function

        Public Function Exec_Query(ByVal query As String) As DataSet
            Dim da As OleDbDataAdapter = New OleDbDataAdapter
            Dim ds As DataSet = New DataSet
            Me.pCodError = 0
            Me.pMsgError = ""

            Try
                Me.Cnn = New OleDbConnection(LeeConexion())
                cmd = New OleDbCommand
                cmd.Connection = Cnn
                cmd.CommandText = query
                cmd.CommandType = CommandType.Text

                da.SelectCommand = cmd
                da.Fill(ds, "consulta")
                Cnn.Close()
            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try
            GeneraXMLds(ds)
            Return ds
        End Function

        Public Function Exec_Query(ByVal query As String, ByVal dsname As String) As DataSet
            Dim da As OleDbDataAdapter = New OleDbDataAdapter
            Dim ds As DataSet = New DataSet
            Me.pCodError = 0
            Me.pMsgError = ""

            Try
                Me.Cnn = New OleDbConnection(LeeConexion())
                cmd = New OleDbCommand
                cmd.Connection = Cnn
                cmd.CommandText = query
                cmd.CommandType = CommandType.Text

                da.SelectCommand = cmd
                da.Fill(ds, dsname)
                Cnn.Close()
            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try
            GeneraXMLds(ds)
            Return ds
        End Function

        'Public Function Exec_QuerySP(ByVal query As String) As OleDbDataReader
        '    Dim dr As OleDbDataReader
        '    Me.pCodError = 0
        '    Me.pMsgError = ""

        '    Try
        '        Me.Cnn = New OleDbConnection(LeeConexion())
        '        Me.Cnn.Open()

        '        cmd = New OleDbCommand
        '        cmd.Connection = Cnn
        '        cmd.CommandText = query
        '        cmd.CommandType = CommandType.Text


        '        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        '        'Dim xml As XmlDocument = New XmlDocument
        '        'Dim elem As XmlElement
        '        'xml.LoadXml("<Registro/>")
        '        'While dr.Read()
        '        '    elem = xml.CreateElement("Row")
        '        '    dr.Item()
        '        'End While
        '        'dr.Close()
        '    Catch errSql As OleDbException
        '        pCodError = errSql.ErrorCode
        '        pMsgError = errSql.Message
        '        Cnn.Close()
        '    Catch err As Exception
        '        pCodError = -100
        '        pMsgError = err.Message

        '    End Try
        '    Return dr
        'End Function
        'Public Function Exec_SP_Reader(ByVal Organizacion As Integer, _
        '                        ByVal CodTran As Integer, _
        '                        ByVal CodOpcion As Integer, _
        '                        ByVal arrParam As Array, _
        '                        ByVal ParamAut As String, _
        '                        ByVal ValorAut As String, _
        '                        ByVal txtTrans As String) As OleDbDataReader

        '    Dim dr As OleDbDataReader
        '    Me.pCodError = 0
        '    Me.pMsgError = ""

        '    Try

        '        'Recupera Stored Procedure Autorizado y sus parametros
        '        Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
        '        Dim nameSP As String

        '        If Me.pCodError = 0 Then
        '            Dim xmlDoc As XmlDocument = New XmlDocument
        '            xmlDoc.LoadXml(xmlSP)

        '            Dim root As XmlElement = xmlDoc.DocumentElement
        '            nameSP = root.GetAttribute("nombre")

        '            cmd = New OleDbCommand
        '            cmd.Connection = Cnn
        '            cmd.CommandText = nameSP
        '            cmd.CommandType = CommandType.StoredProcedure

        '            'Genera Parametros al comando
        '            GeneraParametros(root, arrParam)
        '            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

        '        End If
        '    Catch errSql As OleDbException
        '        pCodError = errSql.ErrorCode
        '        pMsgError = errSql.Message
        '        Cnn.Close()
        '    Catch err As Exception
        '        pCodError = -100
        '        pMsgError = err.Message

        '    End Try
        '    Return dr

        'End Function

        Public Function Exec_SP_ParamOut(ByVal Organizacion As Integer, _
                                ByVal CodTran As Integer, _
                                ByVal CodOpcion As Integer, _
                                ByVal arrParam As Array, _
                                ByVal ParamAut As String, _
                                ByVal ValorAut As String, _
                                ByVal txtTrans As String) As XmlDocument

            Me.pCodError = 0
            Me.pMsgError = ""
            Dim elem As XmlElement
            Dim xml As New XmlDocument
            Dim i As Integer
            xml.LoadXml("<Registro/>")
            Try

                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New OleDbCommand
                    cmd.Connection = Cnn
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    cmd.ExecuteNonQuery()
                    'Retorna los valores de salida

                    Dim item As OleDbParameter
                    For Each item In cmd.Parameters
                        If item.Direction.ToString().Equals("Output") Then
                            If Not item.ParameterName.Equals("") Then
                                xml.DocumentElement.SetAttribute(item.ParameterName.Substring(1, item.ParameterName.Length - 1), item.Value.ToString())
                            Else
                                xml.DocumentElement.SetAttribute("PO_", item.Value.ToString())
                            End If
                        End If
                    Next
                    Cnn.Close()
                End If
            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try
            xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
            xml.DocumentElement.SetAttribute("MsgError", MsgError)

            Return xml

        End Function

        Private Function getTipo(ByVal tipo As String) As OleDbType
            Select Case LCase(tipo)
                Case "int"
                    Return OleDbType.Integer
                Case "char"
                    Return OleDbType.Char
                Case "varchar"
                    Return OleDbType.VarChar
                Case "datetime"
                    Return OleDbType.VarChar
                Case "float"
                    Return OleDbType.Decimal
                Case "image"
                    Return OleDbType.VarBinary
            End Select

        End Function

        Private Function getDireccion(ByVal direccion As String) As ParameterDirection
            Select Case direccion
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

        ' ---------------------------------------------------
        Public Function Exec_QuerySP(ByVal query As String) As XmlDocument
            Dim dr As OleDbDataReader
            Dim xml As New XmlDocument
            Me.pCodError = 0
            Me.pMsgError = ""
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Registro/>")

            Try
                Me.Cnn = New OleDbConnection(LeeConexion())
                Me.Cnn.Open()

                cmd = New OleDbCommand
                cmd.Connection = Cnn
                cmd.CommandText = query
                cmd.CommandType = CommandType.Text


                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                xml = GeneraXMLdr(dr)
                dr.Close()
                Cnn.Close()

            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try

            xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
            xml.DocumentElement.SetAttribute("MsgError", MsgError)
            Return xml
        End Function
        Private Sub GeneraXMLds(ByVal PI_ds As DataSet)
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
        End Sub
        Private Function GeneraXMLdr(ByVal PI_dr As OleDbDataReader) As XmlDocument
            Dim elem As XmlElement
            Dim xml As New XmlDocument
            Dim i As Integer
            Try
                xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Registro/>")
                While PI_dr.Read()
                    elem = xml.CreateElement("Row")
                    For i = 0 To PI_dr.FieldCount - 1
                        If PI_dr.GetName(i).Equals("") Then
                            elem.SetAttribute("Retorno", PI_dr(i).ToString())
                        Else
                            elem.SetAttribute(PI_dr.GetName(i), PI_dr(i).ToString())
                        End If
                    Next
                    xml.DocumentElement.AppendChild(elem)
                End While
            Catch ex As Exception
                MsgError = ex.Message
                CodError = -100
            End Try
            xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
            xml.DocumentElement.SetAttribute("MsgError", MsgError)
            Return xml
        End Function
        Public Function Exec_SP_Tran_Reader(ByVal Organizacion As Integer, _
                        ByVal CodTran As Integer, _
                        ByVal CodOpcion As Integer, _
                        ByVal arrParam As Array, _
                        ByVal ParamAut As String, _
                        ByVal ValorAut As String, _
                        ByVal txtTrans As String) As XmlDocument

            Dim dr As OleDbDataReader
            Dim xml As New XmlDocument
            Me.pCodError = 0
            Me.pMsgError = ""
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Registro/>")

            Try
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New OleDbCommand
                    cmd.Connection = Cnn
                    cmd.Transaction = Me.Trans
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    dr = cmd.ExecuteReader()
                    xml = GeneraXMLdr(dr)
                    dr.Close()
                End If

            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                If Cnn.State = ConnectionState.Open Then
                    Cnn.Close()
                End If

            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message
                If Cnn.State = ConnectionState.Open Then
                    Cnn.Close()
                End If

            End Try

            xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
            xml.DocumentElement.SetAttribute("MsgError", MsgError)

            Return xml

        End Function

        Public Function Exec_SP_Reader(ByVal Organizacion As Integer, _
                                       ByVal CodTran As Integer, _
                                       ByVal CodOpcion As Integer, _
                                       ByVal arrParam As Array, _
                                       ByVal ParamAut As String, _
                                       ByVal ValorAut As String, _
                                       ByVal txtTrans As String) As XmlDocument

            Dim dr As OleDbDataReader
            Dim xml As New XmlDocument
            Me.pCodError = 0
            Me.pMsgError = ""
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Registro/>")

            Try

                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New OleDbCommand
                    cmd.Connection = Cnn
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    xml = GeneraXMLdr(dr)
                    dr.Close()
                    Cnn.Close()
                End If
            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try

            xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
            xml.DocumentElement.SetAttribute("MsgError", MsgError)

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
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                If pCodError = 0 Then
                    Retorno = True
                    Me.Cnn.Close()

                End If
            Catch Ex As Exception
                Retorno = False
            End Try

            Return Retorno

        End Function

        Public Function Exec_SP_ParamOutBA(ByVal Organizacion As Integer, _
                                ByVal CodTran As Integer, _
                                ByVal CodOpcion As Integer, _
                                ByVal arrParam As Array, _
                                ByVal ParamAut As String, _
                                ByVal ValorAut As String, _
                                ByVal txtTrans As String, _
                                ByVal PI_Servidor As String, _
                                ByVal PI_Proveedor As String, _
                                ByVal PI_Usuario As String, _
                                ByVal PI_Clave As String) As XmlDocument

            Me.pCodError = 0
            Me.pMsgError = ""
            Dim elem As XmlElement
            Dim xml As New XmlDocument
            Dim i As Integer
            xml.LoadXml("<Registro/>")
            Try

                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Cnn.Close()
                    ' Cnn.ConnectionString = LeeConexion(PI_Servidor, PI_Proveedor, PI_Usuario, PI_Clave)
                    Me.Cnn = New OleDbConnection(LeeConexion(PI_Servidor, PI_Proveedor, PI_Usuario, PI_Clave))
                    Me.Cnn.Open()

                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New OleDbCommand
                    cmd.Connection = Cnn
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    cmd.ExecuteNonQuery()
                    'Retorna los valores de salida

                    Dim item As OleDbParameter
                    For Each item In cmd.Parameters
                        If item.Direction.ToString().Equals("Output") Then
                            If Not item.ParameterName.Equals("") Then
                                xml.DocumentElement.SetAttribute(item.ParameterName.Substring(1, item.ParameterName.Length - 1), item.Value.ToString())
                            Else
                                xml.DocumentElement.SetAttribute("PO_", item.Value.ToString())
                            End If
                        End If
                    Next
                    Cnn.Close()

                End If
            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try
            xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
            xml.DocumentElement.SetAttribute("MsgError", MsgError)

            Return xml

        End Function

        Public Function Exec_SP_ReaderBA(ByVal Organizacion As Integer, _
                                       ByVal CodTran As Integer, _
                                       ByVal CodOpcion As Integer, _
                                       ByVal arrParam As Array, _
                                       ByVal ParamAut As String, _
                                       ByVal ValorAut As String, _
                                       ByVal txtTrans As String, _
                                       ByVal PI_Servidor As String, _
                                       ByVal PI_Proveedor As String, _
                                       ByVal PI_Usuario As String, _
                                       ByVal PI_Clave As String) As XmlDocument

            Dim dr As OleDbDataReader
            Dim xml As New XmlDocument
            Me.pCodError = 0
            Me.pMsgError = ""
            xml.LoadXml("<?xml version=""1.0"" encoding=""iso-8859-1"" ?><Registro/>")

            Try

                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Cnn.Close()
                    ' Cnn.ConnectionString = LeeConexion(PI_Servidor, PI_Proveedor, PI_Usuario, PI_Clave)
                    Me.Cnn = New OleDbConnection(LeeConexion(PI_Servidor, PI_Proveedor, PI_Usuario, PI_Clave))
                    Me.Cnn.Open()

                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New OleDbCommand
                    cmd.Connection = Cnn
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    xml = GeneraXMLdr(dr)
                    dr.Close()
                    Cnn.Close()

                End If
            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()
            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try

            xml.DocumentElement.SetAttribute("CodError", CodError.ToString())
            xml.DocumentElement.SetAttribute("MsgError", MsgError)

            Return xml

        End Function
        Public Function Exec_SPBA(ByVal Organizacion As Integer, _
                                       ByVal CodTran As Integer, _
                                       ByVal CodOpcion As Integer, _
                                       ByVal arrParam As Array, _
                                       ByVal ParamAut As String, _
                                       ByVal ValorAut As String, _
                                       ByVal txtTrans As String, _
                                       ByVal PI_Servidor As String, _
                                       ByVal PI_Proveedor As String, _
                                       ByVal PI_Usuario As String, _
                                       ByVal PI_Clave As String) As DataSet
            Dim ds As DataSet = New DataSet

            Dim da As OleDbDataAdapter = New OleDbDataAdapter
            Me.pCodError = 0
            Me.pMsgError = ""

            Try
                'Recupera Stored Procedure Autorizado y sus parametros
                Dim xmlSP As String = getPermiso(Organizacion, CodTran, CodOpcion, ParamAut, ValorAut, txtTrans)
                Dim nameSP As String

                If Me.pCodError = 0 Then
                    Cnn.Close()
                    ' Cnn.ConnectionString = LeeConexion(PI_Servidor, PI_Proveedor, PI_Usuario, PI_Clave)
                    Me.Cnn = New OleDbConnection(LeeConexion(PI_Servidor, PI_Proveedor, PI_Usuario, PI_Clave))
                    Me.Cnn.Open()

                    Dim xmlDoc As XmlDocument = New XmlDocument
                    xmlDoc.LoadXml(xmlSP)

                    Dim root As XmlElement = xmlDoc.DocumentElement
                    nameSP = root.GetAttribute("nombre")

                    cmd = New OleDbCommand
                    cmd.Connection = Cnn
                    cmd.CommandText = nameSP
                    cmd.CommandType = CommandType.StoredProcedure

                    'Genera Parametros al comando
                    GeneraParametros(root, arrParam)

                    da.SelectCommand = cmd
                    da.Fill(ds, nameSP)
                    Cnn.Close()
                End If

            Catch errSql As OleDbException
                pCodError = errSql.ErrorCode
                pMsgError = errSql.Message
                Cnn.Close()

            Catch err As Exception
                pCodError = -100
                pMsgError = err.Message

            End Try

            GeneraXMLds(ds)
            Return ds

        End Function

        Public Sub SetUsuario(ByVal PI_elem As XmlElement)
            Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
            If PI_elem.GetAttribute("PS_Login").Equals("N") Then
                pUsuario = PI_elem.GetAttribute("PS_IdUsuario")
                pUsuario = objEncripta.Decrypt(pUsuario)
                pToken = PI_elem.GetAttribute("PS_Token")
                pToken = objEncripta.Decrypt(pToken)
            Else
                pUsuario = PI_elem.GetAttribute("PS_IdUsuario")
                pToken = PI_elem.GetAttribute("PS_Token")
            End If
            setUsrSitio(PI_elem)
            pMaquina = PI_elem.GetAttribute("PS_Maquina")
            pAplicacion = CType(PI_elem.GetAttribute("PS_IdAplicacion"), Integer)
            Me.pEmpresa = CType(PI_elem.GetAttribute("PS_IdEmpresa"), Integer)
            Me.pSucursal = CType(PI_elem.GetAttribute("PS_IdSucursal"), Integer)
            Me.txtTrans = PI_elem.OuterXml

        End Sub

        Public Sub SetDefaultUsuario(ByVal PI_elem As XmlElement)
            setUsrSitio(PI_elem)
            Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
            pAplicacion = CType(PI_elem.GetAttribute("PS_IdAplicacion"), Integer)
            pEmpresa = CType(PI_elem.GetAttribute("PS_IdEmpresa"), Integer)
            pSucursal = CType(PI_elem.GetAttribute("PS_IdSucursal"), Integer)
            'Usuario de sesion por default
            pUsuario = System.Configuration.ConfigurationManager.AppSettings("UsuarioWS")
            pUsuario = objEncripta.Decrypt(pUsuario)
            pToken = System.Configuration.ConfigurationManager.AppSettings("TokenWS")
            pToken = objEncripta.Decrypt(pToken)
            pMaquina = System.Web.HttpContext.Current.Application("MaquinaWS")
            Me.txtTrans = PI_elem.OuterXml

        End Sub
        Public Sub setUsrSitio(ByVal PI_elem As XmlElement)
            'Desencripta del formato .net y lo encripta en Attala
            Dim objEncripta As Seguridad.clsEncripta = New Seguridad.clsEncripta
            'Usuario de sitio
            pUsrSitio = PI_elem.GetAttribute("PS_UsrSitio")
            pUsrSitio = objEncripta.Decrypt(pUsrSitio)
            pTokenSitio = PI_elem.GetAttribute("PS_TokenSitio")
            pTokenSitio = objEncripta.Decrypt(pTokenSitio)
            pMaqSitio = PI_elem.GetAttribute("PS_MaqSitio")
            pServidorBD = System.Configuration.ConfigurationManager.AppSettings("ServidorBD")
            pProveedorBD = System.Configuration.ConfigurationManager.AppSettings("ProveedorBD")
            pUsuarioBD = System.Configuration.ConfigurationManager.AppSettings("UsuarioBD")
            pClaveBD = System.Configuration.ConfigurationManager.AppSettings("ClaveBD")

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace
