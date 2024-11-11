using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clibProveedores.Models
{
    public class formResponseLinea
    {
        public formResponseLinea()
        {
            root = new List<Object>();
        }

        public Boolean success { get; set; }

        public string codError { get; set; }
        public string msgError { get; set; }

        public List<Object> root { get; set; }
    }

    public class liConsUsuarios
    {
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public int IdEmpresa { get; set; }
        public string Ruc { get; set; }
        public int IdTipoLogin{ get; set; }
        public string DescripcionRol { get; set; }
        public string NumIdent { get; set; }
        public string IdUsuario { get; set; }
        public int Rol28 { get; set; }
        public int Rol29 { get; set; }
        public int Rol30 { get; set; }

        public string esIngresado { get; set; }
        public string Correo { get; set; }

    }

    public class liConsProveedor
    {
            public string CodProveedor { get; set; }
            public string Ruc { get; set; }
            public string RazonSocial { get; set; }
            public string Porcentaje { get; set; }
            public string TipoPedido { get; set; }
            public string CodElemento { get; set; }
    }

    public class liConsEmpleLinea
    {
        public int IdEmpresa { get; set; }
        public string Ruc { get; set; }
        public string Usuario { get; set; }
        public string Linea { get; set; }
    }

    public class liConsProveeLinea
    {
        public string CodProveedor { get; set; }
        public string Linea { get; set; }
    }
}
