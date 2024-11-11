using System;
using System.Collections.Generic;

namespace clibProveedores.Models
{
    public class ModelReporteLog
    {
        public string IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string IdTransaccion { get; set; }
        public string DescTransaccion { get; set; }
        public string IdOrganizacion { get; set; }
        public string DescOrganizacion { get; set; }
        public string IdAplicacion { get; set; }
        public string DescAplicativo { get; set; }
        public string IdIdentificacion { get; set; }

        public string Fecha { get; set; }

        public string Rol { get; set; }
        public string Descripcion { get; set; }
        public string Accion { get; set; }
        public string Opcion { get; set; }

        public string FechaModificacion { get; set; }
        public string FechaRegistro { get; set; }


    }

    public class FormResponseReporteLog
    {
        public FormResponseReporteLog()
        {
            root = new List<Object>();
        }

        public string codError { get; set; }
        public string msgError { get; set; }

        public Boolean success { get; set; }

        public List<Object> root { get; set; }
    }

}
