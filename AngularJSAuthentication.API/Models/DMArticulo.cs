using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Models
{
    public class DMSolicitudArticulo
    {
        public SolCabecera[] p_SolCabecera { get; set; }
        public SolDetalle[] p_SolDetalle { get; set; }
        public SolDetalleG[] p_SolDetalleG { get; set; }
        public SolMedida[] p_SolMedida { get; set; }
        public SolCodigoBarra[] p_SolCodigoBarra { get; set; }
        public SolImagen[] p_SolImagen { get; set; }
        public SolRutas[] p_SolRutas { get; set; }
        public SolCompras[] p_SolCompras { get; set; }
        public SolCatalogacion[] p_SolCatalogacion { get; set; }
        public SolAlmacen[] p_SolAlmacen { get; set; }
        public SolCaracteristicas[] p_SolCaracteristicas { get; set; }
        public SolIndTipoAlmEnt[] p_SolIndTipoAlmEnt { get; set; }
        public SolIndTipoAlmSal[] p_SolIndTipoAlmSal { get; set; }
        public SolIndAreaAlmacen[] p_SolIndAreaAlmacen { get; set; }
        public SolCentros[] p_SolCentros { get; set; }
        public SolObserv[] p_SolObserv { get; set; }
        public CodigoLegacy[] p_CodLegacy { get; set; }

        public class CodigoLegacy
        {
            public string codigo { get; set; }
            public string detalle { get; set; }
        }

        public class SolCabecera
        {
            public string CodProveedor { get; set; }
            public string IdSolicitud { get; set; }
            public string TipoSolicitud { get; set; }
            public string LineaNegocio { get; set; }
            public string Accion { get; set; }
            public string Estado { get; set; }
            public string Usuario { get; set; }
            public string Observacion { get; set; }
            public string MotivoRechazo { get; set; }            
            public string EnviaNotificacion { get; set; }
            public string EnviaAprobar { get; set; }
            public string Ruc { get; set; }
            public string RazonSocial { get; set; }
            public string Correo { get; set; }
            public string CorreoProveedor { get; set; }            
            public string Telefono { get; set; }
            public string Celular { get; set; }
            public string FecSolicitud { get; set; }
            public string PersonaContacto { get; set; }
            public string Responsable { get; set; }
            public string Aprobador { get; set; }
            public string Departamento { get; set; }
        }

        public class SolDetalle
        {
            public string IdDetalle { get; set; }
            public string CodReferencia { get; set; }
            public string CodSAPart { get; set; }            
            public string Marca { get; set; }
            public string DesMarca { get; set; }
            public string MarcaNueva { get; set; }
            public string PaisOrigen { get; set; }
            public string RegionOrigen { get; set; }
            public string TamArticulo { get; set; }
            public string GradoAlcohol { get; set; }
            public string Talla { get; set; }
            public string Color { get; set; }
            public string Fragancia { get; set; }
            public string Tipos { get; set; }
            public string Sabor { get; set; }
            public string Modelo { get; set; }
            public string Descripcion { get; set; }
            public string OtroId { get; set; }
            public Boolean ContAlcohol { get; set; }
            public string Iva { get; set; }
            public string Deducible { get; set; }
            public string Retencion { get; set; }
            public string Estado { get; set; }
            public string Observacion { get; set; }
            public string Accion { get; set; }
            public string CodTipoArticulo { get; set; }
            public string TipoArticulo { get; set; }
            public string PrecioBruto { get; set; }
            public string PrecioNuevo { get; set; }
            public string Descuento1 { get; set; }
            public string Descuento2 { get; set; }
            public string Coleccion { get; set; }
            public string Temporada { get; set; }
            public string Estacion { get; set; }
            public string CantidadPedir { get; set; }
            public Boolean ImpVerde { get; set; }
            public Boolean SeleccionaGenerico { get; set; }
            public Boolean IsVariante { get; set; }
            public Boolean IsGenerico { get; set; }
            public string CodGenerico { get; set; }
            public string CodProveedor { get; set; }

        }

        public class SolDetalleG //Sin caracteristicas
        {
            public string IdDetalle { get; set; }
            public string CodReferencia { get; set; }
            public string CodSAPart { get; set; }

            public string Marca { get; set; }
            public string DesMarca { get; set; }
            public string MarcaNueva { get; set; }
            public string PaisOrigen { get; set; }
            public string RegionOrigen { get; set; }
            public string GradoAlcohol { get; set; }
            public string Modelo { get; set; }
            public string Descripcion { get; set; }
            public string OtroId { get; set; }
            public Boolean ContAlcohol { get; set; }
            public string Iva { get; set; }
            public string Deducible { get; set; }
            public string Retencion { get; set; }
            public string Estado { get; set; }
            public string Accion { get; set; }

        }

        public class SolMedida
        {
            public string IdDetalle { get; set; }
            public string UnidadMedida { get; set; }
            public string DesUnidadMedida { get; set; }
            public string UniMedidaVolumen { get; set; }
            public string DesUniMedidaVolumen { get; set; }
            public string TipoUnidadMedida { get; set; }
            public string DesTipoUnidadMedida { get; set; }
            public string UniMedConvers { get; set; }
            public string DesUniMedConvers { get; set; }
            public string FactorCon { get; set; }
            public string PesoNeto { get; set; }
            public string PesoBruto { get; set; }
            public string Longitud { get; set; }
            public string Ancho { get; set; }
            public string Altura { get; set; }
            public string Volumen { get; set; }
            public string PrecioBruto { get; set; }
            public string Descuento1 { get; set; }
            public string Descuento2 { get; set; }
            public Boolean ImpVerde { get; set; }
            public Boolean uniMedBase { get; set; }
            public Boolean uniMedES { get; set; }
            public Boolean uniMedPedido { get; set; }
            public Boolean uniMedVenta { get; set; }
            public string Estado { get; set; }
            public string Accion { get; set; }            
            
        }

        public class SolCodigoBarra
        {
            public string IdDetalle { get; set; }
            public string UnidadMedida { get; set; }
            public string NumeroEan { get; set; }
            public string TipoEan { get; set; }
            public string DescripcionEan { get; set; }
            public string paisEan { get; set; }
            public string paisDesEan { get; set; }
            public Boolean Principal { get; set; }
            public string Accion { get; set; }
        }

        public class SolImagen
        {
            public string IdDetalle { get; set; }
            public string IdDocAdjunto { get; set; }
            public string NomArchivo { get; set; }
            public string Path { get; set; }
            public string Accion { get; set; }
        }

        public class SolRutas
        {
            public string IdDetalle { get; set; }
            public string Path { get; set; }
        }

        public class SolCompras
        {
            public string IdDetalle { get; set; }
            public string CodLegacyProv { get; set; }
            public string OrganizacionCompras { get; set; }
            public string OrganizacionComprasDes { get; set; }
            public string FrecuenciaEntrega { get; set; }
            public string TipoMaterial { get; set; }
            public string TipoMaterialDes { get; set; }
            public string CategoriaMaterial { get; set; }
            public string CategoriaMaterialDes { get; set; }
            public string GrupoArticulo { get; set; }
            public string SeccionArticulo { get; set; }
            //public string Catalogacion { get; set; }
            public string SurtidoParcial { get; set; }
            public string Materia { get; set; }
            public string MateriaDes { get; set; }
            public string CostoFOB { get; set; }
            public string IndPedido { get; set; }
            public string PerfilDistribucion { get; set; }
            //public string Almacen { get; set; }
            public string GrupoCompra { get; set; }
            public string CategoriaValoracion { get; set; }
            public string TipoAlamcen { get; set; }
            //public string IndAlmaEntrada { get; set; }
            //public string IndAlmaSalida { get; set; }
            //public string IndAreaAlmacen { get; set; }
            public string CondicionAlmacen { get; set; }
            public string ClListaSurtido { get; set; }
            public string EstatusMaterial { get; set; }
            public string EstatusVenta { get; set; }
            public string ValidoDesde { get; set; }
            public string GrupoBalanzas { get; set; }
            public string Observacion { get; set; }
            public string Coleccion { get; set; }
            public string Temporada { get; set; }
            public string Estacion { get; set; }
            public string CantidadPedir { get; set; }
            public string JerarquiaProd { get; set; } //Nuevos
			public string SusceptBonifEsp { get; set; }
			public string ProcedimCatalog { get; set; }
			public string CaracterPlanNec { get; set; }
            public string FuenteProvision { get; set; }//Nuevos
            public string MotivoRechazo { get; set; }
            public string Estado { get; set; }
            public string Undbase { get; set; }
            public string Undpedido { get; set; }
            public string Undes { get; set; }
            public string Undventa { get; set; }
        }

        public class SolCatalogacion
        {
            public string IdDetalle { get; set; }
            public string Catalogacion { get; set; }
            public string DesCatalogacion { get; set; }
            public string Canaldistribucion { get; set; }
            public string DesCanaldistribucion { get; set; }
            public string Accion { get; set; }
        }

        public class SolCaracteristicas
        {
            public string IdDetalle { get; set; }
            public string IdCaract { get; set; }
            public string IdValor { get; set; }
            public string IdAgrupacion { get; set; }      
            public string Accion { get; set; }
        }

        public class SolCentros
        {
            public string IdDetalle { get; set; }
            public string Centros { get; set; }
            public string DesCentros { get; set; }
            public string PerfilDistribucion { get; set; }
            public string DesperfilDistribucion { get; set; }
            public string Accion { get; set; }
        }

        public class SolAlmacen
        {
            public string IdDetalle { get; set; }
            public string Almacen { get; set; }
            public string DesAlmacen { get; set; }
            public string TipAlmacen { get; set; }
            public string DestipAlmacen { get; set; }
            public string IndAlmacenE { get; set; }
            public string DesindAlmacenE { get; set; }
            public string IndAlmacenS { get; set; }
            public string DesindAlmacenS { get; set; }
            public string IndAreaAlmNew { get; set; }
            public string DesIndAreaAlmNew { get; set; }

            public string Accion { get; set; }  

        }

        public class SolIndTipoAlmEnt
        {
            public string IdDetalle { get; set; }
            public string IndTipoAlmEnt { get; set; }
            public string DesIndTipoAlmEnt { get; set; }
            public string Accion { get; set; }
        }

        public class SolIndTipoAlmSal
        {
            public string IdDetalle { get; set; }
            public string IndTipoAlmSal { get; set; }
            public string DesIndTipoAlmSal { get; set; }
            public string Accion { get; set; }
        }

        public class SolIndAreaAlmacen
        {
            public string IdDetalle { get; set; }
            public string IndAreaAlmacen { get; set; }
            public string DesIndAreaAlmacen { get; set; }
            public string GrupoBalanzas { get; set; }
            public string DesgrupoBalanzas { get; set; }
            public string Accion { get; set; }
        }

        public class SolObserv
        {
            public string Fecha { get; set; }
            public string EstadoSolicitud { get; set; }
            public string Usuario { get; set; }
            public string Motivo { get; set; }
            public string Observacion { get; set; }
        }
    }

    public class ReporteSolicitudArticulo
    {
        public SolCabecera[] p_SolCabecera { get; set; }
        public SolDetalle[] p_SolDetalle { get; set; }
        public SolDetalleG[] p_SolDetalleG { get; set; }
        public SolMedida[] p_SolMedida { get; set; }
        public SolCodigoBarra[] p_SolCodigoBarra { get; set; }
        public SolImagen[] p_SolImagen { get; set; }
        public SolRutas[] p_SolRutas { get; set; }
        public SolCompras[] p_SolCompras { get; set; }
        public SolCatalogacion[] p_SolCatalogacion { get; set; }
        public SolAlmacen[] p_SolAlmacen { get; set; }
        public SolIndTipoAlmEnt[] p_SolIndTipoAlmEnt { get; set; }
        public SolIndTipoAlmSal[] p_SolIndTipoAlmSal { get; set; }
        public SolIndAreaAlmacen[] p_SolIndAreaAlmacen { get; set; }
        public SolCentros[] p_SolCentros { get; set; }
        public SolObserv[] p_SolObserv { get; set; }

        public class SolCabecera
        {
            public string CodProveedor { get; set; }
            public string IdSolicitud { get; set; }
            public string TipoSolicitud { get; set; }
            public string LineaNegocio { get; set; }
            public string Accion { get; set; }
            public string Estado { get; set; }
            public string Usuario { get; set; }
            public string Observacion { get; set; }
            public string MotivoRechazo { get; set; }
            public string EnviaNotificacion { get; set; }
            public string EnviaAprobar { get; set; }
            public string Ruc { get; set; }
            public string RazonSocial { get; set; }
            public string Correo { get; set; }
            public string CorreoProveedor { get; set; }
            public string Telefono { get; set; }
            public string Celular { get; set; }
            public string FecSolicitud { get; set; }
            public string UsrSolicitud { get; set; }
            public string PersonaContacto { get; set; }
            public string Responsable { get; set; }
            public string Aprobador { get; set; }
            public string Departamento { get; set; }
        }

        public class SolDetalle
        {
            
            public string IdDetalle { get; set; }
            public string CodReferencia { get; set; }
            public string CodSAPart { get; set; }
            public string Marca { get; set; }
            public string DesMarca { get; set; }
            public string MarcaNueva { get; set; }
            public string PaisOrigen { get; set; }
            public string RegionOrigen { get; set; }
            public string TamArticulo { get; set; }
            public string GradoAlcohol { get; set; }
            public string Talla { get; set; }
            public string Color { get; set; }
            public string Fragancia { get; set; }
            public string Tipos { get; set; }
            public string Sabor { get; set; }
            public string Modelo { get; set; }
            public string Descripcion { get; set; }
            public string OtroId { get; set; }
            public Boolean ContAlcohol { get; set; }
            public string Iva { get; set; }
            public string Deducible { get; set; }
            public string Retencion { get; set; }
            public string Estado { get; set; }
            public string Accion { get; set; }
            public string CodTipoArticulo { get; set; }
            public string TipoArticulo { get; set; }
            public string PrecioBruto { get; set; }
            public string Descuento1 { get; set; }
            public string Descuento2 { get; set; }
            public Boolean ImpVerde { get; set; }
            public string CantidadPedir { get; set; }
            public string Temporada { get; set; }
            public string Estacion { get; set; }
            public string Coleccion { get; set; }
            
        }

        public class SolDetalleG //Sin caracteristicas
        {
            public string IdDetalle { get; set; }
            public string CodReferencia { get; set; }
            public string CodSAPart { get; set; }

            public string Marca { get; set; }
            public string DesMarca { get; set; }
            public string MarcaNueva { get; set; }
            public string PaisOrigen { get; set; }
            public string RegionOrigen { get; set; }
            public string GradoAlcohol { get; set; }
            public string Modelo { get; set; }
            public string Descripcion { get; set; }
            public string OtroId { get; set; }
            public Boolean ContAlcohol { get; set; }
            public string Iva { get; set; }
            public string Deducible { get; set; }
            public string Retencion { get; set; }
            public string Estado { get; set; }
            public string Accion { get; set; }
            
        }

        public class SolMedida
        {
            public string IdDetalle { get; set; }
            public string UnidadMedida { get; set; }
            public string DesUnidadMedida { get; set; }
            public string TipoUnidadMedida { get; set; }
            public string DesTipoUnidadMedida { get; set; }
            public string UniMedConvers { get; set; }
            public string DesUniMedConvers { get; set; }
            public string FactorCon { get; set; }
            public string PesoNeto { get; set; }
            public string PesoBruto { get; set; }
            public string Longitud { get; set; }
            public string Ancho { get; set; }
            public string Altura { get; set; }
            public string PrecioBruto { get; set; }
            public string Descuento1 { get; set; }
            public string Descuento2 { get; set; }
            public Boolean ImpVerde { get; set; }
            public Boolean uniMedBase { get; set; }
            public Boolean uniMedES { get; set; }
            public Boolean uniMedPedido { get; set; }
            public Boolean uniMedVenta { get; set; }
            public string Volumen { get; set; }
            public string Estado { get; set; }
            public string Accion { get; set; }

        }

        public class SolCodigoBarra
        {
            public string IdDetalle { get; set; }
            public string UnidadMedida { get; set; }
            public string NumeroEan { get; set; }
            public string TipoEan { get; set; }
            public string DescripcionEan { get; set; }
            public string paisEan { get; set; }
            public string paisDesEan { get; set; }
            public Boolean Principal { get; set; }
            public string Accion { get; set; }
        }

        public class SolImagen
        {
            public string IdDetalle { get; set; }
            public string IdDocAdjunto { get; set; }
            public string NomArchivo { get; set; }
            public string Path { get; set; }
            public string Accion { get; set; }
        }

        public class SolRutas
        {
            public string IdDetalle { get; set; }
            public string Path { get; set; }
        }

        public class SolCompras
        {
            public string IdDetalle { get; set; }
            public string CodLegacyProv { get; set; }
            public string OrganizacionCompras { get; set; }
            public string OrganizacionComprasDes { get; set; }
            public string FrecuenciaEntrega { get; set; }
            public string TipoMaterial { get; set; }
            public string TipoMaterialDes { get; set; }
            public string CategoriaMaterial { get; set; }
            public string CategoriaMaterialDes { get; set; }
            public string GrupoArticulo { get; set; }
            public string SeccionArticulo { get; set; }
            //public string Catalogacion { get; set; }
            public string SurtidoParcial { get; set; }
            public string Materia { get; set; }
            public string MateriaDes { get; set; }
            public string CostoFOB { get; set; }
            public string IndPedido { get; set; }
            public string PerfilDistribucion { get; set; }
            //public string Almacen { get; set; }
            public string GrupoCompra { get; set; }
            public string CategoriaValoracion { get; set; }
            public string TipoAlamcen { get; set; }
            //public string IndAlmaEntrada { get; set; }
            //public string IndAlmaSalida { get; set; }
            //public string IndAreaAlmacen { get; set; }
            public string CondicionAlmacen { get; set; }
            public string ClListaSurtido { get; set; }
            public string EstatusMaterial { get; set; }
            public string EstatusVenta { get; set; }
            public string ValidoDesde { get; set; }
            public string GrupoBalanzas { get; set; }
            public string Observacion { get; set; }
            public string Coleccion { get; set; }
            public string Temporada { get; set; }
            public string Estacion { get; set; }
            public string JerarquiaProd { get; set; } //Nuevos
            public string SusceptBonifEsp { get; set; }
            public string ProcedimCatalog { get; set; }
            public string CaracterPlanNec { get; set; }
            public string FuenteProvision { get; set; }//Nuevos
            public string MotivoRechazo { get; set; }
            public string Estado { get; set; }
        }

        public class SolCatalogacion
        {
            public string IdDetalle { get; set; }
            public string Catalogacion { get; set; }
            public string DesCatalogacion { get; set; }

            public string Accion { get; set; }
        }

        public class SolCentros
        {
            public string IdDetalle { get; set; }
            public string Centros { get; set; }
            public string DesCentros { get; set; }
            public string PerfilDistribucion { get; set; }
            public string DesperfilDistribucion { get; set; }
            public string Accion { get; set; }
        }

        public class SolAlmacen
        {
            public string IdDetalle { get; set; }
            public string Almacen { get; set; }
            public string DesAlmacen { get; set; }
            public string TipAlmacen { get; set; }
            public string DestipAlmacen { get; set; }
            public string IndAlmacenE { get; set; }
            public string DesindAlmacenE { get; set; }
            public string IndAlmacenS { get; set; }
            public string DesindAlmacenS { get; set; }
            public string Accion { get; set; }
            public string DesIndAreaAlmNew { get; set; }
        }

        public class SolIndTipoAlmEnt
        {
            public string IdDetalle { get; set; }
            public string IndTipoAlmEnt { get; set; }
            public string DesIndTipoAlmEnt { get; set; }
            public string Accion { get; set; }
        }

        public class SolIndTipoAlmSal
        {
            public string IdDetalle { get; set; }
            public string IndTipoAlmSal { get; set; }
            public string DesIndTipoAlmSal { get; set; }
            public string Accion { get; set; }
        }

        public class SolIndAreaAlmacen
        {
            public string IdDetalle { get; set; }
            public string IndAreaAlmacen { get; set; }
            public string DesIndAreaAlmacen { get; set; }
            public string GrupoBalanzas { get; set; }
            public string DesgrupoBalanzas { get; set; }
            public string Accion { get; set; }
        }

        public class SolObserv
        {
            public string Fecha { get; set; }
            public string EstadoSolicitud { get; set; }
            public string Usuario { get; set; }
            public string Motivo { get; set; }
            public string Observacion { get; set; }
        }
    }
}