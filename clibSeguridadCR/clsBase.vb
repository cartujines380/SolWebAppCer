Imports System
Imports System.Xml

Namespace Seguridad

    Public Class clsBase

#Region "Constructor"
        Public Sub New()
            Try
                Dim idioma As String
                'leen una key del archivo de configuracion app.config
                idioma = System.Configuration.ConfigurationManager.AppSettings.Item("culture")
                'Setear la configuracion regional de la aplicacion
                System.Threading.Thread.CurrentThread.CurrentCulture = _
                                New System.Globalization.CultureInfo(idioma)
            Catch ex As Exception

            End Try
        End Sub
#End Region

#Region "propiedades publicas"
        Public objBase As New clsBaseDatosSQL
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

        Public Overridable Sub actAtributo(ByVal att As String, ByVal valor As String)
            'Debe ser sobreescrito en las clases derivadas

        End Sub
    End Class

End Namespace