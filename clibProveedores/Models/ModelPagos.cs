using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJSAuthentication.API.Models
{
    public class Pagos
    {
        public Int64 Num { get; set; }
        public string Identificacion { get; set; }
        public string NomComercial { get; set; }
        public string CodProveedorAx { get; set; }
        public string Factura { get; set; }
        public string FormaPago { get; set; }
        public string TipoPago { get; set; }
        public string FechaPago { get; set; }
        public string Valor { get; set; }
        public string Detalle { get; set; }
    }
    public class FormResponsePagos
    {
        public FormResponsePagos()
        {
            root = new List<Object>();
        }

        public string codError { get; set; }
        public string msgError { get; set; }

        public Boolean success { get; set; }

        public List<Object> root { get; set; }
    }   
 
}
