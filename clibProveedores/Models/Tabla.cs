using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace clibProveedores.Models
{
    public class TablaCatalogo
    {
        public int Tabla { get; set; }

        public string Codigo { get; set; }

        public string Detalle { get; set; }

        public string Estado { get; set; }

        public string DescAlterno { get; set; }

        public string Licencia { get; set; }

        public string Error { get; set; }


    }
}