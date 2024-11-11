using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSProcesoBase.Model
{
    public class DMSolcitudProveedor
    {
        public SolProveedor[] p_SolProveedor { get; set; }
        public SolProvContacto[] p_SolProvContacto { get; set; }
        public SolProvDireccion[] p_SolProvDireccion { get; set; }
        public SolDocAdjunto[] p_SolDocAdjunto { get; set; }
        public SolLineaNegocio[] p_SolLineaNegocio { get; set; }
        public SolProvBanco[] p_SolProvBanco { get; set; }
        public SolProvHistEstado[] p_SolProvHistEstado { get; set; }
        public SolProvZona[] p_SolProvZona { get; set; }
        public SolRamo[] p_SolRamo { get; set; }
        public SolViapago[] p_SolViapago { get; set; }
        public SolNotificacion[] p_SolNotificacion { get; set; }
        public SolLineaAdmin[] p_SolLineaAdmin { get; set; }


        public class SolProveedor
        {
            [Required]
            public string IdEmpresa { get; set; }
            public string IdSolicitud { get; set; }
            [Required]
            public string TipoSolicitud { get; set; }
            public string DescTipoSolicitud { get; set; }
            [Required]
            public string TipoProveedor { get; set; }
            public string DescProveedor { get; set; }
            public string CodSapProveedor { get; set; }
            [Required]
            public string TipoIdentificacion { get; set; }
            public string DEscTipoIndentificacion { get; set; }
            [Required]
            public string Identificacion { get; set; }
            [Required]
            public string NomComercial { get; set; }
            public string RazonSocial { get; set; }
            [Required]
            //[DataType(DataType.Date)]
            //public DateTime FechaSRI { get; set; }
            public string FechaSRI { get; set; }
            public string SectorComercial { get; set; }
            public string DescSectorComercial { get; set; }
            public string Idioma { get; set; }
            public string DescIdioma { get; set; }
            public string CodGrupoProveedor { get; set; }
            public string GenDocElec { get; set; }
            //[DataType(DataType.Date)]
            //public DateTime FechaSolicitud { get; set; }
            public string FechaSolicitud { get; set; }
            public string Estado { get; set; }
            public string DescEstado { get; set; }
            public string GrupoTesoreria { get; set; }
            public string DescGrupoTesoreria { get; set; }
            public string CuentaAsociada { get; set; }
            public string DescCuentaAsociada { get; set; }
            public string Autorizacion { get; set; }
            public string TransfArticuProvAnterior { get; set; }
            public string DepSolicitando { get; set; }
            public string Responsable { get; set; }
            public string Aprobacion { get; set; }
            public string Comentario { get; set; }
            [Required]
            public string TelfFijo { get; set; }
            public string TelfFijoEXT { get; set; }
            public string TelfMovil { get; set; }
            public string TelfFax { get; set; }
            public string TelfFaxEXT { get; set; }
            [Required]
            [DataType(DataType.EmailAddress)]
            public string EMAILCorp { get; set; }
            [Required]
            [DataType(DataType.EmailAddress)]
            public string EMAILSRI { get; set; }
            public string ClaseContribuyente { get; set; }
            public string DescClaseContribuyente { get; set; }
            public string princliente { get; set; }
            public string totalventas { get; set; }
            public string AnioConsti { get; set; }
            public string LineaNegocio { get; set; }
            public string DescLineaNegocio { get; set; }
            public string PlazoEntrega { get; set; }
            public string DespachaProvincia { get; set; }
            public string DescDespachaProvincia { get; set; }
            public string GrupoCuenta { get; set; }
            public string DescGrupoCuenta { get; set; }
            public string RetencionIva { get; set; }
            public string DescRetencionIva { get; set; }
            public string RetencionIva2 { get; set; }
            public string DescRetencionIva2 { get; set; }
            public string RetencionFuente { get; set; }
            public string DescRetencionFuente { get; set; }
            public string RetencionFuente2 { get; set; }
            public string DescRetencionFuente2 { get; set; }
            //public string ViaPago	             { get; set; }
            //public string DescViaPago            { get; set; }
            public string CondicionPago { get; set; }
            public string DescCondicionPago { get; set; }
            public string Ramo { get; set; }
            public string GrupoEsquema { get; set; }
            public string ListaViasPago { get; set; }

        }
        public class SolProvContacto
        {
            public string IdSolContacto { get; set; }
            public string IdSolicitud { get; set; }
            public string TipoIdentificacion { get; set; }
            public string DescTipoIdentificacion { get; set; }
            public string Identificacion { get; set; }
            public string Nombre2 { get; set; }
            public string Nombre1 { get; set; }
            public string Apellido2 { get; set; }
            public string Apellido1 { get; set; }
            public string CodSapContacto { get; set; }
            public string PreFijo { get; set; }
            public string DepCliente { get; set; }
            public string Departamento { get; set; }
            public string Funcion { get; set; }
            public Boolean RepLegal { get; set; }
            public Boolean Estado { get; set; }
            public string TelfFijo { get; set; }
            public string TelfFijoEXT { get; set; }
            public string TelfMovil { get; set; }
            public string EMAIL { get; set; }
            public string DescFuncion { get; set; }
            public string DescDepartamento { get; set; }
            public Boolean NotElectronica { get; set; }
            public Boolean NotTransBancaria { get; set; }
            public string actas { get; set; }
        }

        public class SolProvContactoCiudad
        {
            public string codigoSap { get; set; }
            public string Identificacion { get; set; }
            public string codigoAlmacen { get; set; }
            public string codigoPais { get; set; }
            public string codigoCiudad { get; set; }
            public string codigoRegion { get; set; }
        }
        public class SolProvDireccion
        {

            public string IdDireccion { get; set; }
            public string IdSolicitud { get; set; }
            public string Pais { get; set; }
            public string DescPais { get; set; }
            public string Provincia { get; set; }
            public string DescRegion { get; set; }
            public string Ciudad { get; set; }
            public string CallePrincipal { get; set; }
            public string CalleSecundaria { get; set; }
            public string PisoEdificio { get; set; }
            public string CodPostal { get; set; }
            public string Solar { get; set; }
            public Boolean Estado { get; set; }

        }
        public class SolDocAdjunto
        {

            public string IdSolicitud { get; set; }
            public string IdSolDocAdjunto { get; set; }
            public string CodDocumento { get; set; }
            public string DescDocumento { get; set; }
            public string NomArchivo { get; set; }
            public string Archivo { get; set; }
            public string FechaCarga { get; set; }
            //public DateTime FechaCarga { get; set; }
            public Boolean Estado { get; set; }

        }
        public class SolLineaNegocio
        {
            public string IdSolicitud { get; set; }
            public string CodigoSociedad { get; set; }
            public string DescSociedad { get; set; }
            public string CodigoSeccion { get; set; }
            public string DescSeccion { get; set; }
            public string IdLIneNegocio { get; set; }
        }
        public class SolProvBanco
        {
            public string IdSolBanco { get; set; }
            public string IdSolicitud { get; set; }
            public string Extrangera { get; set; }
            public string CodSapBanco { get; set; }
            public string NomBanco { get; set; }
            public string Pais { get; set; }
            public string DescPAis { get; set; }
            public string TipoCuenta { get; set; }
            public string DesCuenta { get; set; }
            public string NumeroCuenta { get; set; }
            public string TitularCuenta { get; set; }
            public string ReprCuenta { get; set; }
            public string CodSwift { get; set; }
            public string CodBENINT { get; set; }
            public string CodABA { get; set; }
            public Boolean Principal { get; set; }
            public Boolean Estado { get; set; }
            public string Provincia { get; set; }
            public string DescProvincia { get; set; }
            public string DirBancoExtranjero { get; set; }
            public string BancoExtranjero { get; set; }
        }
        public class SolProvHistEstado
        {
            public string IdObservacion { get; set; }
            public string IdSolicitud { get; set; }
            public string Motivo { get; set; }
            public string DesMotivo { get; set; }
            public string Observacion { get; set; }
            public string Usuario { get; set; }
            public DateTime Fecha { get; set; }
            public string EstadoSolicitud { get; set; }
            public string DesEstadoSolicitud { get; set; }
        }
        public class SolProvZona
        {
            public string IdSolicitud { get; set; }
            public string IdZona { get; set; }
            public string CodZona { get; set; }
            public string DescZona { get; set; }
            public Boolean Estado { get; set; }
        }
        public class SolRamo
        {
            public string IdSolicitud { get; set; }
            public string IdRamo { get; set; }
            public string CodRamo { get; set; }
            public string DescRamo { get; set; }
            public Boolean Estado { get; set; }
            public Boolean Principal { get; set; }
        }
        public class SolViapago
        {
            public string IdSolicitud { get; set; }
            public string CodVia { get; set; }
            public string DescVia { get; set; }
            public Boolean Estado { get; set; }
            public string IdVia { get; set; }
        }
        public class SolNotificacion
        {

            public string IdEmpresa { get; set; }
            public string Modulo { get; set; }
            public string DesModulo { get; set; }
            public string Nivel { get; set; }
            public string DEsNivel { get; set; }
            public string Ruc { get; set; }
            public string Usuario { get; set; }
            public string Linea { get; set; }
            public string DescLinea { get; set; }
            public string Apellido1 { get; set; }
            public string Apellido2 { get; set; }
            public string Nombre1 { get; set; }
            public string Nombre2 { get; set; }
            public string CorreoE { get; set; }
            public string Cargo { get; set; }
            public string TipoIdent { get; set; }
            public string DesTipoIdentificacion { get; set; }
        }

        public class SolLineaAdmin
        {

            public string Linea { get; set; }
            public string DescLinea { get; set; }

        }

        
    }
    public class FormResponseProveedor
    {
        public FormResponseProveedor()
        {
            root = new List<Object>();
        }

        public string codError { get; set; }
        public string msgError { get; set; }

        public Boolean success { get; set; }

        public List<Object> root { get; set; }
    }
}

