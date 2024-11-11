using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clibProveedores.Models
{
    public class Tra_SolicitudConsolidacionGrabar
    {
        public Tra_ConsolidacionPedidos p_consolidacion { get; set; }
        public Tra_PedidosGrabar[] p_Pedidos { get; set; }
        public Tra_DetPedidosGrabar[] p_DetPedidos { get; set; }

        public class Tra_ConsolidacionPedidos
        {
            public string idconsolidacion { get; set; }
            public string Tipo { get; set; }
            public string codChofer { get; set; }
            public string codAyudante { get; set; }
            public string codVehiculo { get; set; }
            public string codProveedor {  get; set; }
            public string cosRapido { get; set; }
            public string codAlmacenDestino { get; set; }
            public string usuarioCreacion { get; set; }
        }
        public class Tra_PedidosGrabar
        {
            public Boolean chkpedido { get; set; }
            public string NumPedido { get; set; }
            public string FechaPedido { get; set; }
            public string RineRide { get; set; }
            public string AlmacenSolicitante { get; set; }
            public string AlmacenDestino { get; set; }
            public string FechaCaducidad { get; set; }
            public string ValorPedido { get; set; }
            public string ValorFactura { get; set; }
            public string NumeroBulto { get; set; }
            public string NumeroPalet { get; set; }
            public string IDPalet { get; set; }
        }

        public class Tra_DetPedidosGrabar
        {
            public string Item { get; set; }
            public string NumPedido { get; set; }
            public string CodigoProducto { get; set; }
            public string Factura { get; set; }
            public string FechaFactura { get; set; }
            public string Descripcion { get; set; }
            public string CantidadPedido { get; set; }
            public string PrecioUnitario { get; set; }
            public string UnidadCaja { get; set; }
            public string Descuento1 { get; set; }
            public string Descuento2 { get; set; }
            public string Iva { get; set; }
            public string Subtotal { get; set; }
            public string Total { get; set; }
            public string CantidadxDespachar { get; set; }
            public string CantidadDespachada { get; set; }
            public string CantidadPediente { get; set; }
        }
    }
    public class Tra_ReporteTabular
    {
        public Tra_ReporteTabularCitasCab p_cabecera { get; set; }
        public Tra_ReporteTabularCitasDet[] p_detalle { get; set; }
        public class Tra_ReporteTabularCitasCab
        {
            public string tipo { get; set; }
            public string usuario { get; set; }
        }
        public class Tra_ReporteTabularCitasDet
        {
            public string numcita { get; set; }
            public string codsap { get; set; }
            public string nombreproveedor { get; set; }
            public string fecha { get; set; }
            public string horainicial { get; set; }
            public string horafinal { get; set; }
            public string cedulachofer { get; set; }
            public string nombrechofer { get; set; }
            public string placa { get; set; }
            public string marca { get; set; }
            public string modelo { get; set; }
            public string almacendestino { get; set; }
        }
    }
    public class Tra_ReporteTabularAprobarCita
    {
        public Tra_ReporteAprobarCitaCab p_cabeceraAprobarCita { get; set; }
        public Tra_ReporteAprobarCitaDet[] p_detalleAprobarCita { get; set; }
        public class Tra_ReporteAprobarCitaCab
        {
            public string tipo { get; set; }
            public string usuario { get; set; }
        }
        public class Tra_ReporteAprobarCitaDet
        {
            public string idcita { get; set; }
            public string numcita { get; set; }
            public string idconsolidacion { get; set; }
            public string bodega { get; set; }
            public string estado { get; set; }
            public string estadorechazo { get; set; }
            public string fechasolicitada { get; set; }
            public string fechaasignada { get; set; }
            public string caducidadsolicitud { get; set; }
            public string citarapida { get; set; }
        }
    }
    public class Tra_ReporteTabularCita
    {
        public Tra_ReporteCitaCab p_cabeceraCita { get; set; }
        public Tra_ReporteCitaDet[] p_detalleCita { get; set; }
        public class Tra_ReporteCitaCab
        {
            public string tipo { get; set; }
            public string usuario { get; set; }
        }
        public class Tra_ReporteCitaDet
        {
            public string idconsolidacion { get; set; }
            public string emision { get; set; }
            public string almacendestino { get; set; }
            public string estadoconsolidacion { get; set; }
            public string caducidadsolicitud { get; set; }
            public string vehiculo { get; set; }
            public string chofer { get; set; }
            public string asistente { get; set; }
        }
    }
    public class Tra_ReporteTabularConsolidacion
    {
        public Tra_ReporteConsolidacionCab p_cabeceraConsolidacion { get; set; }
        public Tra_ReporteConsolidacionDet[] p_detalleConsolidacion { get; set; }
        public class Tra_ReporteConsolidacionCab
        {
            public string tipo { get; set; }
            public string usuario { get; set; }
        }
        public class Tra_ReporteConsolidacionDet
        {
            public string idconsolidacion { get; set; }
            public string idcita { get; set; }
            public string emision { get; set; }
            public string almacensolicitante { get; set; }
            public string almacendestino { get; set; }
            public string estadoconsolidacion { get; set; }
            public string caducidadconsolidacion { get; set; }
            public string imagenurl { get; set; }
            public string imagenurl2 { get; set; }
        }
    }
    public class Tra_ReporteTabularVehiculo
    {
        public Tra_ReporteVehiculoCab p_cabeceraVehiculo { get; set; }
        public Tra_ReporteVehiculorDet[] p_detalleVehiculo { get; set; }
        public class Tra_ReporteVehiculoCab
        {
            public string tipo { get; set; }
            public string usuario { get; set; }
        }
        public class Tra_ReporteVehiculorDet
        {
            public string codsap { get; set; }
            public string idvehiculo { get; set; }
            public string numplaca { get; set; }
            public string tipovehiculo { get; set; }
            public string marca { get; set; }
            public string modelo { get; set; }
            public string estado { get; set; }
            public string colorprincipal { get; set; }
            public string colorsecundario { get; set; }

        
        }
    }
    public class Tra_ReporteTabularChofer
    {
        public Tra_ReporteChoferCab p_cabeceraChofer { get; set; }
        public Tra_ReporteChoferDet[] p_detalleChofer { get; set; }
        public class Tra_ReporteChoferCab
        {
            public string tipo { get; set; }
            public string usuario { get; set; }
        }
        public class Tra_ReporteChoferDet
        {
            public string idchofer { get; set; }
            public string nombres { get; set; }
            public string licencia { get; set; }
            public string telefono { get; set; }
            public string direccion { get; set; }
            public string estado { get; set; }
        
        }
    }
    public class Tra_CitaRapidaGrabar
    {
        public Tra_CitaRapidaChoferVehiculo p_cabecera { get; set; }
        public Tra_ModPedidosCitaRapidaGrabar[] p_detalle { get; set; }

        public class Tra_CitaRapidaChoferVehiculo
        {
            public string txtrucproveedor { get; set; }
            public string txtnombreempresa { get; set; }
            public string idchofer { get; set; }
            public string cedulachofer { get; set; }
            public string txtNombresPrimero { get; set; }
            public string txtApellidoPrimero { get; set; }
            public string idayudante { get; set; }
            public string cedulaayudante { get; set; }
            public string txtnombreayudante{ get; set; }
            public string txtApellidoayudate { get; set; }
            public string idvehiculo { get; set; }
            public string txtplacavehiculo { get; set; }
            public string codtipo { get; set; }
            public string usuarioCreacion { get; set; }
            public string codAlmacenDestino { get; set; }
            public string txtfechasolicitud { get; set; }
            public string horainicial { get; set; }
            public string horafinal { get; set; }
            
         }
        public class Tra_ModPedidosCitaRapidaGrabar
        {
            public string id { get; set; }
            public Boolean chkpedido { get; set; }
            public string pedido { get; set; }
            public string factura { get; set; }
            public string extfactura { get; set; }

        }
    }
    public class Tra_Parametros
    {
        public string dato { get; set; }
    }

    public class Tra_CitaRapidaCho
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
    }
    public class Tra_CitaRapidaCVe
    {
        public string id { get; set; }
        public string placa { get; set; }
        public string codtipo { get; set; }
    }

    public class Tra_SolicitudCitas
    {
        public string idconsolidacion { get; set; }
        public string emision { get; set; }
        public string almacensolicitante { get; set; }
        public string almacendestino { get; set; }
        public string estadoconsolidacion { get; set; }
        public string caducidadsolicitud { get; set; }
        public string vehiculo { get; set; }
        public string chofer { get; set; }
        public string asistente { get; set; }
        public string variacita { get; set; }
        public string citarapida { get; set; }
        public string estado { get; set; }

    }
    public class Tra_SolicitudCitasHorario
    {
        public string Dias { get; set; }
        public string Hora_Inicial { get; set; }
        public string Hora_Final { get; set; }
        public string Color { get; set; }
        public string ColorTexto { get; set; }
        public string Titulo { get; set; }
        public string Id { get; set; }
    }
    public class Tra_ModsolidacionPedidos
    {
        public string idconsolidacion { get; set; }
        public string estado { get; set; }
        public string fechaemision { get; set; }
        public string fechacaducidad { get; set; }
        public string idalmacendestino {get; set;}
        public string almacendestino{ get; set; }
        public string idchofer{ get; set; }
        public string idayudante { get; set; }
        public string idvehiculo { get; set; }
        public string cosrapido { get; set; }

    }
    public class Tra_ModPedidosGrabar
    {
        public Boolean chkpedido { get; set; }
        public string NumPedido { get; set; }
        public string FechaPedido { get; set; }
        public string RineRide { get; set; }
        public string AlmacenSolicitante { get; set; }
        public string AlmacenDestino { get; set; }
        public string FechaCaducidad { get; set; }
        public string ValorPedido { get; set; }
        public string ValorFactura { get; set; }
        public string NumeroBulto { get; set; }
        public string NumeroPalet { get; set; }
        public string IDPalet { get; set; }
    }

    public class Tra_ModPedidosCitaRapida
    {
        public string id { get; set; }
        public Boolean chkpedido { get; set; }
        public string pedido { get; set; }
        public string factura { get; set; }
        public string extfactura { get; set; }
        
    }

    public class Tra_busquedaFecha
    {
        public string fechadesde { get; set; }
        public string fechahasta { get; set; }
    }

    public class Tra_ModDetPedidosGrabar
    {
        public string Item { get; set; }
        public string NumPedido { get; set; }
        public string CodigoProducto { get; set; }
        public string Factura { get; set; }
        public string FechaFactura { get; set; }
        public string Descripcion { get; set; }
        public string CantidadPedido { get; set; }
        public string PrecioUnitario { get; set; }
        public string UnidadCaja { get; set; }
        public string Descuento1 { get; set; }
        public string Descuento2 { get; set; }
        public string Iva { get; set; }
        public string Subtotal { get; set; }
        public string Total { get; set; }
        public string CantidadxDespachar { get; set; }
        public string CantidadDespachada { get; set; }
        public string CantidadPediente { get; set; }
    }

    public class Tra_BandejaVehiculo
    {
        public int IDVEHICULO { get; set; }
        public string NUMPLACA { get; set; }
        public string TIPOVEHICULO { get; set; }
        public string MARCA { get; set; }
        public string MODELO { get; set; }
        public string ESTADO { get; set; }
        public string COLORPRINCIPAL { get; set; }
        public string COLORSECUNDARIO { get; set; }
        public string CODSAP { get; set; }
    }

    public class Tra_BandejaChoferes
    {
        public int IDCHOFER { get; set; }
        public string NOMBRES { get; set; }
        public string LICENCIA { get; set; }
        public string TELEFONO { get; set; }
        public string DIRECCION { get; set; }
        public string ESTADO { get; set; }
        public string CODSAP { get; set; }
    }

    public class Tra_BandejaConsolidacion
    {
        public string proveedor { get; set; }
        public string idconsolidacion { get; set; }
        public string idcita { get; set; }
        public string emision { get; set; }
        public string almacensolicitante { get; set; }
        public string almacendestino { get; set; }
        public string estadoconsolidacion { get; set; }
        public string caducidadconsolidacion { get; set; }
        public string imagenurl { get; set; }
        public string imagenurl2 { get; set; }
        public string codproveedor { get;  set; }
    }

    public class Tra_BandejaAprobacionCitas
    {
        public string idconsolidacion { get; set; }
        public string idcita { get; set; }
        public string numcita { get; set; }
        public string bodega { get; set; }
        public string fechasolicitada { get; set; }
        public string fechaasignada { get; set; }
        public string caducidadsolicitud { get; set; }
        public string estado { get; set; }
        public string estadorechazo { get; set; }
        public string citarapida { get; set; }
    }

    public class Tra_ReporteTabularCitas
    {
        public string numcita { get; set; }
        public string codsap { get; set; }
        public string nombreproveedor { get; set; }
        public string fecha { get; set; }
        public string horainicial { get; set; }
        public string horafinal { get; set; }
        public string cedulachofer { get; set; }
        public string nombrechofer { get; set; }
        public string placa { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string almacendestino { get; set; }
    }
    public class Tra_DetalleAprobacionCitas
    {
        public string idconsolidacion { get; set; }
        public string numpedido { get; set; }
        public string emisionpedido { get; set; }
        public string factura { get; set; }
        public string fechafactura { get; set; }
        public string almacendestino { get; set; }
        public string caducidadpedido { get; set; }
    }

       public class Tra_DetalleChoferVehiculo
    {
        public string nombreschoMostrar { get; set; }
        public string cedulachoMostrar { get; set; }
        public string telefonochoMostrar { get; set; }
        public string imagenurlchofer { get; set; }
        public string nombresasiMostrar { get; set; }
        public string cedulaasiMostrar { get; set; }
        public string telefonoasiMostrar { get; set; }
        public string imagenurlasistente { get; set; }
        public string placavehiculoMostrar { get; set; }
        public string tipovehiculoMostrar { get; set; }
        public string colorprincipalMostrar { get; set; }
        public string imagenurlvehiculo { get; set; }
    }
    
                    
    public class Tra_ConsultaPedidos
    {
        public Boolean chkpedido { get; set; }
        public string NumPedido { get; set; }
        public string FechaPedido { get; set; }
        public string RineRide { get; set; }
        public string AlmacenSolicitante { get; set; }
        public string AlmacenDestino { get; set; }
        public string FechaCaducidad { get; set; }
        public string ValorPedido { get; set; }
        public string ValorFactura { get; set; }
        public string NumeroBulto { get; set; }
        public string NumeroPalet { get; set; }
        public string IDPalet { get; set; }
        public string TipoPedido { get; set; }
        public bool IsCross { get; set; }
        public string Estado { get; set; }
        public string EstadoDesc { get; set; }
        public string tipoPedidos { get; set; }
    }

    public class Tra_ConsultaDetPedidos
    {
        public string Item { get; set; }
        public string NumPedido { get; set; }
        public string CodigoProducto { get; set; }
        public string Factura { get; set; }
        public string FechaFactura { get; set; }
        public string Descripcion { get; set; }
        public string CantidadPedido { get; set; }
        public string PrecioUnitario { get; set; }
        public string UnidadCaja { get; set; }
        public string Descuento1 { get; set; }
        public string Descuento2 { get; set; }
        public string Iva { get; set; }
        public string Subtotal { get; set; }
        public string Total { get; set; }
        public string CantidadxDespachar { get; set; }
        public string CantidadDespachada { get; set; }
        public string CantidadPediente { get; set; }
    }

    public class Tra_BandejaPedidos
    {
        public int IDCITA { get; set; }
        public string BODEGA { get; set; }
        public string FECHAPEDIDO { get; set; }
        public string FECHAENTREGA { get; set; }
        public string ESTADO { get; set; }
    }


    public class Tra_BandejaAuditoria
    {
        public string fechaingreso { get; set; }
        public string estado { get; set; }
        public string motivo { get; set; }
        public string usuario { get; set; }
        public string fechacambio { get; set; }
        public string tipo { get; set; }
    }

    public class Tra_SolicitudCita
    {
        public string ID_Bodega { get; set; }
        public string ID_consolidacion { get; set; }
        public string Orden_compra { get; set; }
        public string ID_Proveedor { get; set; }
        public string Pallet { get; set; }
        public string Bultos { get; set; }
        public string Fecha_propuesta { get; set; }
        public string hora_inicio { get; set; }
        public string hora_fin { get; set; }
        public string Tipo_Vehiculo { get; set; }
        public string ID_Factura { get; set; }
        public string CodEstado { get; set; }

    }
    public class Tra_BandejaGrabar
    {
        public string Tipo { get; set; }
        public string idChofer { get; set; }
        public string txtNombresPrimero { get; set; }
        public string txtNombresSegundo { get; set; }
        public string txtApellidoPrimero { get; set; }
        public string txtApellidoSegundo { get; set; }
        public string CodIdentificacion { get; set; }
        public string txtIdentificacion { get; set; }
        public string txtructranspo { get; set; }
        public string txtNombreEmptran { get; set; }
        public string txtDireccionDomicilio { get; set; }
        public string txtTelefonoDomicilio { get; set; }
        public string CodEstado { get; set; }
        public string CodBloqueo { get; set; }
        public string txtFechaBloqueo { get; set; }
        public string txtOrigenBloque { get; set; }
        public string txtNumeroLicencia { get; set; }
        public string CodCategoria { get; set; }
        public string CodTipoSangre { get; set; }
        public string txtFechaEmision { get; set; }
        public string txtFechaExpiracion { get; set; }
        public string txtFechaNacimiento { get; set; }
        public string txtarchivo { get; set; }
        public string CodProveedor { get; set; }
        public string transecuencial { get; set; }
        public string usuarioCreacion { get; set; }
        public string usuarioModificacion { get; set; }
        


    }

    public class Tra_VehiculoGrabar
    {
        public string Tipo { get; set; }
        public string idvehiculo { get; set; }
        public string CodProveedor { get; set; }
        public string codIdentificacion { get; set; }
        public string txtIdentificacion { get; set; }
        public string txtNombresPrimero { get; set; }
        public string txtNombresSegundo { get; set; }
        public string txtApellidoPrimero { get; set; }
        public string txtApellidoSegundo { get; set; }
        public string txtdirpropie { get; set; }
        public string txttelfpro { get; set; }
        public string codTipoVehiculo { get; set; }
        public string codColorPrincipal { get; set; }
        public string codColorSecundario { get; set; }
        public string txtmotor { get; set; }
        public string codMarca { get; set; }
        public string codModelo { get; set; }
        public string txtplaca { get; set; }
        public string txtchasis { get; set; }
        public string codEstado { get; set; }
        public string codBloqueo { get; set; }
        public string txtFechaBloqueo { get; set; }
        public string txtOrigenBloque { get; set; }
        public string txtmatricula { get; set; }
        public string txtfechamatricula { get; set; }
        public string txtexpiracionmatricula { get; set; }
        public string txtaniomatricula { get; set; }
        public string codPais { get; set; }
        public string txtcilindraje { get; set; }
        public string txttonelaje { get; set; }
        public string PorSota { get; set; }
        public string txtemisionsoat { get; set; }
        public string txtexpiracionsoat { get; set; }
        public string txtarchivo { get; set; }
        public string transecuencial { get; set; }
        public string usuarioCreacion { get; set; } 

    }
    public class FormResponseTransporte
    {
        public FormResponseTransporte()
        {
            root = new List<Object>();
        }

        public string codError { get; set; }
        public string msgError { get; set; }

        public Boolean success { get; set; }

        public List<Object> root { get; set; }
    }
    public class cls_Stock
    {
        public string BASE_UOM { get; set; }
        public string DATE { get; set; }
        public string LIFNR { get; set; }
        public string MATNR { get; set; }
        public string STOCK { get; set; }
        public string WERKS { get; set; }
        public string ZIDNLF { get; set; }
    }
    public class FormResponse
    {
        public FormResponse()
        {
            root = new List<Object>();
        }

        public string codError { get; set; }
        public string msgError { get; set; }

        public Boolean success { get; set; }

        public List<Object> root { get; set; }
    }
}

