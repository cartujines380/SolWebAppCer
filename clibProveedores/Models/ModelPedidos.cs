using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace clibProveedores.Models
{

    public class formResponsePedidos
    {
        public formResponsePedidos()
        {
            root = new List<Object>();
        }

        public Boolean success { get; set; }

        public string codError { get; set; }
        public string msgError { get; set; }

        public List<Object> root { get; set; }
    }

    public class Gra_reporteEnviar
    {
        public conReporteDatos p_reporteDatos { get; set; }
        public conReporteAlmacen[] p_reporteAlmacen { get; set; }
        public conReporteMaterial[] p_reporteMaterial { get; set; }

        public class conReporteDatos
        {
            public string CodSap { get; set; }
            public string Fecha1 { get; set; }
            public string Fecha2 { get; set; }
            public string p_usuario { get; set; }
            public string nomreporte { get; set; }
        }
        public class conReporteAlmacen
        {
            public string id { get; set; }
            public string descripcion { get; set; }
        }
        public class conReporteMaterial
        {
            public string id { get; set; }
            public string descripcion { get; set; }
        }
    }


    public class Gra_reporteMercado
    {
        public conReporteDatos p_reporteDatos { get; set; }
        public conReporteAlmacen[] p_reporteAlmacen { get; set; }

        public class conReporteDatos
        {
            public string CodSap { get; set; }
            public string TipoLista { get; set; }
            public string Fecha1 { get; set; }
            public string Fecha2 { get; set; }
            public string Linea { get; set; }
            public string Seccion { get; set; }
            public string SubSeccion { get; set; }
            public string Grupo { get; set; }
            public string p_usuario { get; set; }
            public string nomreporte { get; set; }
        }
        public class conReporteAlmacen
        {
            public string id { get; set; }
            public string descripcion { get; set; }
        }
    }


    public class merCatalogo
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string dePendencia { get; set; }

    }

    public class repEmpresas
    {
        public string codEmpresa { get; set; }
        public string descripcion { get; set; }
        
    }
    public class listArchivo
    {
        public string id { get; set; }
        public string descripcion { get; set; }
        public string archivo { get; set; }

    }
    public class listEmpresa
    {
        public string id { get; set; }
        public string empresa { get; set; }
        public string monto { get; set; }
        public string codproveedor { get; set; }

    }
    public class repRequerimiento
    {
        public string id { get; set; }
        public string fecha { get; set; }
        public string categoria { get; set; }
        public string empresa { get; set; }
        public string descripcion { get; set; }
        public string monto { get; set; }
        public string estado { get; set; }
        public string titulo { get; set; }

    }
    public class repAdjudicacion
    {
        public string nombre { get; set; }
        public string fe_empieza { get; set; }
        public string fe_exp { get; set; }
        public string ho_exp { get; set; }
        public string feRequerimiento { get; set; }
        public string montoRequerimiento { get; set; }
        public string codRequerimiento { get; set; }
        public string desRequerimiento { get; set; }
        public string empRequerimiento { get; set; }
        

    }
    public class repVentaMecado
    {
        public string codLinea { get; set; }
        public string linea { get; set; }
        public string porLinea { get; set; }
        public string codSeccion { get; set; }
        public string seccion { get; set; }
        public string porSeccion { get; set; }
        public string codSubseccion { get; set; }
        public string subSeccion { get; set; }
        public string porSubSeccion { get; set; }
        public string codGrupo { get; set; }
        public string grupo { get; set; }
        public string porGrupo { get; set; }
    }
       public class repVentaxCentro
    {
        public string fecha { get; set; }
        public string CodCentro { get; set; }
        public string NomAlmacen { get; set; }
        public string CodMaterial { get; set; }
        public string DesMaterial { get; set; }
        public string CantVendida { get; set; }
        public string UnidadVenta { get; set; }
        public string mes { get; set; }
        public string anio { get; set; }
    }
       public class repventaStock
       {
           public string CodCentro { get; set; }
           public string NomAlmacen { get; set; }
           public string CantVendida { get; set; }
       }

       public class repCentroStock
       {
           public string nomAlmacen { get; set; }
           public string stock { get; set; }
           public string pstock { get; set; }
           public string pventa { get; set; }
           public string noventa { get; set; }
       }
       public class repArticuloMese
       {
           public string anio { get; set; }
           public string mes { get; set; }
           public string codMaterial { get; set; }
           public string desMaterial { get; set; }
           public string cantVendida { get; set; }
       }

    public class repcodigoArticulo
    {
        public string CodMaterial { get; set; }
        public string DesMaterial { get; set; }
    }

    public class griArticulos
    {
        public string codArticulo { get; set; }
        public string desArticulo { get; set; }
    }
    public class griArticulosBase
    {
        public string codAlmacen { get; set; }
        public string nomAlmacen { get; set; }
        public string cantidad { get; set; }
        public string articulo { get; set; }
    }
    public class repcentro
    {
        public string Centro { get; set; }
        public string Almacen { get; set; }
    }
    public class pedCiudadesAlmac
    {
        public string pCodCiudad { get; set; }
        public string pNomCiudad { get; set; }
    }
    public class pedAlmacenes
    {
        public string pCodAlmacen { get; set; }
        public string pNomAlmacen { get; set; }
        public string pCodCiudad { get; set; }
    }
    public class pedAnios
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }
     public class Tra_ReportePedido
    {
         public p_cabeceraPedido p_cabecera { get; set; }
         public p_detallePedido[] p_detalle { get; set; }

         public class p_cabeceraPedido
         {
             public string pCodSAP { get; set; }
             public string pRuc { get; set; }
             public string pUsuario { get; set; }
             public string pOpcGrp1 { get; set; }
             public string pOpcGrp2 { get; set; }
             public string Fecha1 { get; set; }
             public string Fecha2 { get; set; }
             public string Ciudad { get; set; }
             public string pNumOrden { get; set; }
             public string Almacen { get; set; }
             public string tipo { get; set; }
         }

         public class p_detallePedido
         {
             public int idPedido { get; set; }
             public string numPedido { get; set; }
             public DateTime fechaPedido { get; set; }
             public string codAlmacen { get; set; }
             public string nomAlmacen { get; set; }
             public string codAlmDestino { get; set; }
             public string zonaOrigen { get; set; }
             public bool esDescargado { get; set; }
             public bool esImpreso { get; set; }
             public string esDescargadoDes { get; set; }
             public string esImpresoDes { get; set; }
             public string estadoDes { get; set; }
             public decimal valorTotalPedido { get; set; }
             public decimal totalSumaFacturas { get; set; }
             public string estado { get; set; }
             public decimal valorPendPedido { get; set; }
             public char siValorPend { get; set; }

         }
    }
     public class pedReporteCross
     {
         public string codtienda { get; set; }
         public string nomTienda { get; set; }
         public string ordenCompra { get; set; }
         public string codArticulo { get; set; }
         public string nomArticulo { get; set; }
         public string ordenSalida { get; set; }
         public string cantidadaplanificar { get; set; }
         public string unidadplan { get; set; }
         public string cantidadPlanificada { get; set; }
         public string unidadplanreal { get; set; }
         public string unidadxcaja { get; set; }
     }
     public class pedConsPedidosF
     {
         public string pOrigen { get; set; }
         public string pIdPedido { get; set; }
         public string pNumPedido { get; set; }
         public string porcentaje { get; set; }
         public string pCodAlmacen { get; set; }
         public string pNomAlmacen { get; set; }
         public DateTime pFechaPedido { get; set; }
         public string pCodAlmDestino { get; set; }
         public string pCodProveedor { get; set; }
         public string pNomProveedor { get; set; }
         public string pZonaOrigen { get; set; }
         public string pItem { get; set; }
         public string pCodArticulo { get; set; }
         public string pDesArticulo { get; set; }
         public string pTamano { get; set; }
         public decimal pCantPedido { get; set; }
         public decimal pPrecioCosto { get; set; }
         public string pUndPorCaja { get; set; }
         public decimal pDescuento1 { get; set; }
         public decimal pDescuento2 { get; set; }
         public string pIndIva1 { get; set; }
         public string pTamanoCaja { get; set; }
         public string pCodEAN { get; set; }
         public string esDescargado { get; set; }
         public string esImpreso { get; set; }
         public string pRuc { get; set; }
         public decimal pCantidadDistribucion { get; set; }
         public string pUnidadDistribucion { get; set; }
         public decimal pCantidadProveedor { get; set; }
         public decimal pCantidadRealDistribucion { get; set; }
         public string pUnidadRealDistribucion { get; set; }
         public decimal pTolerancia { get; set; }
         public string pEstado { get; set; }
         public string pEstadoDes { get; set; }
         public string pFechaRegistro { get; set; }
         public string pUsuario { get; set; }
         public string pNumPedidoSalida { get; set; }
         public decimal catDesp { get; set; }
         public string estadodistri { get; set; }
         public string tipoPedido { get; set; }
        public string real { get; set; }
        public string estadoDistri { get; set; }
        public DateTime fecEntregaActual { get; set; }
    }
     public class pedConsSolicitud
     {
         public string pNumPedido { get; set; }
         public string pItem { get; set; }
         public decimal pCantPedido { get; set; }
         public string pCodArticulo { get; set; }
         public string pTamano { get; set; }
         public string pTamanoCaja { get; set; }
         public string pCodEAN { get; set; }
         public string pDesArticulo { get; set; }
         public string pPrecioCosto { get; set; }
         public decimal catDesp { get; set; }
         public decimal saldo { get; set; }
     }
  
     public class pedConsPedidosEtiF
     {
         public string idSolEtiqueta { get; set; }
         public string idPedido { get; set; }
         public string NumPedido { get; set; }
         public string codArticulo { get; set; }
         public int CanDespachar { get; set; }
         public String Estado { get; set; }
         public string fechaEntrega { get; set; }
         public string detalle { get; set; }
         public string codestado { get; set; }
        public string tipoPedido { get; set; }
    }

     public class pedConsPedidosEtiImpresar
     {
         public string fechaEntrega { get; set; }
         public string numPedido { get; set; }
         public string etiquetaCarton { get; set; }
         public string etiquetaAdhesivas { get; set; }
         public string etiquetaRFID { get; set; }
        public string tipoPedido { get; set; }
    }

     public class Gra_pedConsPedidosEtiF
     {
         public pedConsPedidosEtiFGrar[] p_Etiquetas { get; set; }

         public class pedConsPedidosEtiFGrar
         {
             public string idSolEtiqueta { get; set; }
             public string idPedido { get; set; }
             public string NumPedido { get; set; }
             public string codsap { get; set; }
             public string codArticulo { get; set; }
             public int CanDespachar { get; set; }
             public String Estado { get; set; }
             public string fechaEntrega { get; set; }
             public string detalle { get; set; }
             public string opcion { get; set; }

         }
     }

     public class Act_pedConsPedidosEtiF
     {
         public string p_transaccion { get; set; }
         public pedConsPedidosEtiFAct[] p_Etiquetas { get; set; }

         public class pedConsPedidosEtiFAct
         {
             public string pNumPedido { get; set; }
             public string pItem { get; set; }
             public decimal pCantPedido { get; set; }
             public string pCodArticulo { get; set; }
             public string pTamano { get; set; }
             public string pTamanoCaja { get; set; }
             public string pCodEAN { get; set; }
             public string pDesArticulo { get; set; }
             public string pPrecioCosto { get; set; }
             public decimal catDesp { get; set; }
             public decimal saldo { get; set; }

         }
     }

     public class pedidosCross
     {
         public string p_estado { get; set; }
         public string p_usuario { get; set; }
         public pedConsPedidosF[] p_detallePedidos { get; set; }
     }
    public class pedidosFlow
    {
        public string p_estado { get; set; }
        public string p_usuario { get; set; }
        public pedConsPedidosFlow[] p_detallePedidos { get; set; }
    }
    public class pedConsPedidosFlow
    {
        public string cantPlanificada { get; set; }
        public string cantReal { get; set; }
        public string codArticulo { get; set; }
        public string desArticulo { get; set; }
        public string pIdPedido { get; set; }
        public string pTolerancia { get; set; }
        public string tipoPedidos { get; set; }
        public string uniPlanificada { get; set; }
        public string uniReal { get; set; }
    }

     public class _requerimiento
     {
         public int idReq { get; set; }
         public string fechaReq { get; set; }
         public string codCategoria { get; set; }
         public string categoria { get; set; }
         public string codEmpresa { get; set; }
         public string empresa { get; set; }
         public string monto { get; set; }
         public string titulo { get; set; }
         public string descripcion { get; set; }
         public string estado { get; set; }
         public string usuarioCreacion { get; set; }
         public string fechaCreacion { get; set; }

     }
     public class _catalogo
     {
         public string id { get; set; }
         public string descripcion { get; set; }

         public string id1 { get; set; }
     }

     public class publicacion
     {
         public int idPubli { get; set; }
         public string estado { get; set; }
         public int version { get; set; }
         public int idReq { get; set; }
         public string monto { get; set; }
         public string nomPubli { get; set; }
         public string titulo { get; set; }
         public string descripcion { get; set; }
         public string descPubli { get; set; }
         public string descEmp { get; set; }
         public string codEmpresa { get; set; }
         public string feIni { get; set; }
         public string feFin { get; set; }
         public string hoFin { get; set; }
         public string estadoPublicado { get; set; }
         public string estadoParticipando { get; set; }
         public string estadoParticipandoDesc { get; set; }
         public string estadoLicDesc { get; set; }
         public Object documentos { get; set; }
         public Object proveedores { get; set; }
     }

     public class documento
     {
         public int idDoc { get; set; }
         public int idReq { get; set; }
         public int idOferta { get; set; }
         public string codProveedor { get; set; }
         public string desc { get; set; }
         public string archivo { get; set; }
     }

     public class _oferta
     {
         public int idOferta { get; set; }
         public string codProveedor { get; set; }
         public string fechaEnvio { get; set; }
         public string monto { get; set; }
         public string estado { get; set; }
         public int tiempoEjecucion { get; set; }
         public List<documento> documentos { get; set; }
     }

     public class _aplicante
     {
         public int idOferta { get; set; }
         public string codProveedor { get; set; }
         public string nomProveedor { get; set; }
         public string monto { get; set; }
         public string fechaEnvio { get; set; }
         public int tiempoEjecucion { get; set; }
         public string estadoParticipando { get; set; }
         public string estadoSeleccionado { get; set; }
         public string estadoParticipandoDesc { get; set; }
         public Object documentos { get; set; }

     }


}

