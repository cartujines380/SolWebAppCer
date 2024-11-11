using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Models
{
    public class MDValidacionProveedor
    {
        public Deudor deudor { get; set; }
        public Participante participante { get; set; }

        public class Deudor
        {
            public string idProducto { get; set; }
            public string idexpediente { get; set; }
            public string origen { get; set; }
            public string tipoidentificacion { get; set; }
            public string identificacion { get; set; }
            public string nombres { get; set; }
            public string apellidos { get; set; }
            public string nombrecompleto { get; set; }
            public string fechanacimiento { get; set; }
            public string edad { get; set; }
            public string nacionalidad { get; set; }
            public string estadocivil { get; set; }
            public string regimenmatrimonial { get; set; }
            public string relaciondepenlaboral { get; set; }
            public string antiguedadlaboral { get; set; }
            public string tipoingreso { get; set; }
            public string ingresosmensuales { get; set; }
            public string montosolicitad { get; set; }
            public string plazosolicitado { get; set; }
            public string periocidad { get; set; }
            public string usuariovalidacion { get; set; }
        }

        public class Participante
        {
            public RegistroParticipante[] registroparticipante { get; set; }

            public class RegistroParticipante
            {
                public string idtipoparticipante { get; set; }
                public string tipoidentificacion { get; set; }
                public string identificacion { get; set; }
                public string nombres { get; set; }
                public string apellidos { get; set; }
                public string nombrescompleto { get; set; }
                public string fechanacimiento { get; set; }
                public string edad { get; set; }
                public string nacionalidad { get; set; }
                public string idregimenmatrimonial { get; set; }
                public string estadocivil { get; set; }
                public string relaciondepenlaboral { get; set; }
                public string antiguedadlaboral { get; set; }
                public string tipoingreso { get; set; }
                public string ingresosmensuales { get; set; }
            }

        }
    }
}