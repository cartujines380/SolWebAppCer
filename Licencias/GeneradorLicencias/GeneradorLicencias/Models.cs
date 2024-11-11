using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorLicencias
{
    public class Licencias
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int PlanId { get; set; }
        public string Licencia { get; set; }
        public DateTime Vencimiento { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int cantidadMaxProveedores { get; set; }
        public bool Activo { get; set; }

    }

    public class Clientes
    {
        public int Id { get; set; }
        public string ruc { get; set; }
        public string codProveedor { get; set; }
        public string proveedor { get; set; }
        public bool Activo { get; set; }
    }

    public class Plan
    {
        public int Id { get; set; }
        public string codPlan { get; set; }
        public string plan { get; set; }
        public int cantidad { get; set; }
        public bool Activo { get; set; }
    }

    public class ClientesViewModel
    {
        public int? Id { get; set; }
        public string Codigo { get; set; }
        public int ClienteId { get; set; }
        public string Cliente { get; set; }
        public int PlanId { get; set; }
        public string Plan { get; set;}
        public string Licencia { get; set;}
        public int Cantidad { get; set; }
    }
}
