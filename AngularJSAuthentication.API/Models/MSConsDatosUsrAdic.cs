using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Models
{
    public class SegConsDatosUsrAdic_Zonas
    {
        public string Zona { get; set; }
        public string DesZona { get; set; }
    }
    public class SegConsDatosUsrAdic
    {
        public string Usuario { get; set; }
        public Int32 IdParticipante { get; set; }
        public string TipoIdent { get; set; }
        public string Identificacion { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public string CorreoE { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string EstadoCivil { get; set; }
        public string Genero { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Estado { get; set; }

        public List<SegConsDatosUsrAdic_Zonas> Zonas { get; set; }
    }
}