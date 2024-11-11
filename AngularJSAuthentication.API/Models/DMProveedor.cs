using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Models
{
    public class DMProveedor
    {
        public Proveedor[] p_Proveedor { get; set; }
        public Proveedor[] p_Banco { get; set; }
        public DocumentoAdjunto[] p_DocumentoAdjuntos { get; set; }
        public class Proveedor
        {

                        public string  CodProveedor { get; set; }	
                        public string  Ruc { get; set; }
                        public string  TipoProveedor { get; set; }
                        public string  NomComercial { get; set; }
                        public string  DirCalleNum { get; set; }
                        public string   DirPisoEdificio { get; set; }
                        public string  DirCallePrinc { get; set; }
                        public string  DirDistrito { get; set; }
                        public string  DirCodPostal { get; set; }
                        public string  Poblacion { get; set; }
                        public string  Pais { get; set; }
                        public string  Region { get; set; }
                        public string  Idioma { get; set; }
                        public string Telefono { get; set; }
                        public string Movil { get; set; }
                        public string Fax { get; set; }
                        [DataType(DataType.EmailAddress)]            
                        public string  CorreoE { get; set; }
                        public string  GenDocElec { get; set; }
                         [DataType(DataType.Date)]
                        public DateTime  FechaCertifica { get; set; }
                        public string IndMinoria { get; set; }
                        public string  ApoderadoNom { get; set; }
                        public string ApoderadoApe { get; set; }
                        public string ApoderadoIdFiscal { get; set; }
                        public string PlazoEntregaPrev { get; set; }
                        [DataType(DataType.Date)]
                        public DateTime  FechaMod { get; set; }
                }


        public class Banco
        {
            public string CodPais { get; set; }
            public string CodBanco { get; set; }
            public string NomBanco { get; set; }
            public string Region { get; set; }
            public string Direcion { get; set; }
            public string Poblacion { get; set; }
            public string CodSWIFT { get; set; }
            public string GrupoBancario { get; set; }
            public string IndGiroCajapost { get; set; }
            public string IndBorrado { get; set; }
            public string CodBancario { get; set; }
        }

        public class DocumentoAdjunto
        {
            public string Id { get; set; }
            public string TipoPersona { get; set; }
            public string Codigo { get; set; }
            public string Descripcion { get; set; }
            public string Obligatorio { get; set; }
            public string Estado { get; set; }
        }

    }
}