using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Models
{
    public class FormResponseModelo
    {
        public FormResponseModelo()
        {
            root = new List<Object>(); //antes root wcr
        }

        public string cCodError { get; set; }
        public string cMsgError { get; set; }

        public Boolean lSuccess { get; set; }

        public List<Object> root { get; set; } //antes root wcr
    }

    public class repComprador
    {
        public string idComprador { get; set; }
        public string nombreComprador { get; set; }
        public string correoComprador { get; set; }
        public string correoAsistenteComprador { get; set; }
        public string estado { get; set; }
    }
    public class repCatalogoConsulta
    {
        public string idCabecera { get; set; }
        public string codigoCatalogo { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public string descAlterno { get; set; }
    }
    public class repCompradorProvedor
    {
        public string codigo { get; set; }
        public string nombreproveedor { get; set; }
        public string estado { get; set; }
    }
    public class repCatalogo
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public string descAlterno { get; set; }
    }
    public class repCompradorProvedor2
    {
        public string idCompradorProveedor { get; set; }
        public string idComprador { get; set; }
        public string codSap { get; set; }
        public string nombreComercial { get; set; }
        public Boolean Estado { get; set; }
    }

    public class p_detalleCatalogo2
    {
        public string IdDetalle { get; set; }
        public string codigoDetalle { get; set; }
        public string descripcion { get; set; }
        public Boolean Estado { get; set; }
        public string descAlterno { get; set; }
    }

    public class Grabar_Comprador
    {
        public p_cabeceraComprador cabecera { get; set; }
        public p_detalleComprador[] detalleComprador { get; set; }

        public class p_cabeceraComprador
        {
            public string IdComprador { get; set; }
            public string NombreComprador { get; set; }
            public string CorreoComprador { get; set; }
            public string CorreoAsistenteComprador { get; set; }
            public string Estado { get; set; }
        }
        public class p_detalleComprador
        {
            public string idCompradorProveedor { get; set; }
            public string idComprador { get; set; }
            public string codSap { get; set; }
            public Boolean Estado { get; set; }
        }
    }
    public class Grabar_Catalogo
    {
        public p_cabeceraCatalogo cabecera { get; set; }
        public p_detalleCatalogo[] detalleCatalogo { get; set; }

        public class p_cabeceraCatalogo
        {
            public string idCabecera { get; set; }
            public string idCatalogo { get; set; }
            public string descripcion { get; set; }
            public string Estado { get; set; }
        }
        public class p_detalleCatalogo
        {
            public string idDetalle { get; set; }
            public string codigoDetalle { get; set; }
            public string descripcion { get; set; }
            public Boolean Estado { get; set; }
            public string descAlterno { get; set; }
        }
    }

}