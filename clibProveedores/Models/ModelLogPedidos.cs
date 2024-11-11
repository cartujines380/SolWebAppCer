using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clibProveedores.Models
{
    public class formResponseLogPedidos
    {
        public formResponseLogPedidos()
        {
            root = new List<Object>();
        }

        public Boolean success { get; set; }

        public string codError { get; set; }
        public string msgError { get; set; }

        public List<Object> root { get; set; }
    }

    public class liLogPedidos
    {
        public int Idlog { get; set; }
        public string FechaProceso { get; set; }
        public string FechaPedido { get; set; }
        public string NumPedido { get; set; }
        public string Archivo { get; set; }
        public string Estado { get; set; }
        public string Linea { get; set; }
    }
}
