using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clibProveedores.Models
{
    public class Tra_ReporteUsuarioNoIngreso
    {
        public Tra_ReporteUsuarioNoIngresoCab p_cabeceraNoIngreso { get; set; }
        public Tra_ReporteUsuarioNoIngresoDet[] p_detalleNoIngreso { get; set; }
        public class Tra_ReporteUsuarioNoIngresoCab
        {
            public string tipo { get; set; }
            public string usuario { get; set; }
        }
        public class Tra_ReporteUsuarioNoIngresoDet
        {
            public string ruc { get; set; }
            public string codproveedor { get; set; }
            public string nomcomercial { get; set; }
            public string Telefono { get; set; }
            public string Usuario { get; set; }
            public string CorreoE { get; set; }
            public string FechaRegistro { get; set; }
        }
    }

    public class ModelReporteAdminNoUsuario
    {
        public string ruc { get; set; }
        public string codproveedor { get; set; }
        public string nomcomercial { get; set; }
        public string Telefono { get; set; }
        public string Usuario { get; set; }
        public string nombre { get; set; }
        public string CorreoE { get; set; }
        public string FechaRegistro { get; set; }
    }

    public class ModelReporteAdminNoOrdenCompra
    {
        public string ruc { get; set; }
        public string codproveedor { get; set; }
        public string nomcomercial { get; set; }
        public string telefono { get; set; }
        public string fechapedido { get; set; }
        public string cantidad { get; set; }
    }

    public class ModelReporteActaRecepcion
    {
        public string ID_Secuencial { get; set; }
        public string NomComercial { get; set; }
        public string NoOrden { get; set; }
        public string NoFactura { get; set; }
        public string almacen { get; set; }
        public string ciudad { get; set; }
        public string anio { get; set; }
        public string mes { get; set; }
        public string dia { get; set; }
        public string archivo { get; set; }
        public string RUC { get; set; }
        public string Estado { get; set; }
        public string DesEstado { get; set; }

    }

    public class ModelReporteLogComunicacioncab
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string comunicado { get; set; }
        public string categoria { get; set; }
        public string prioridad { get; set; }
        public string obligatorio { get; set; }
        public string usringreso { get; set; }
        public string fechapublicacion { get; set; }
    }
    public class ModelReporteSolicitudEtiqueta
    {
        public string fechasolicitud { get; set; }
        public string numerosolicitud { get; set; }
        public string nombreproveedor { get; set; }
        public string codarticulo { get; set; }
        public string descripcion { get; set; }
        public string cantidadpedido { get; set; }
        public string cantidadsolicitud { get; set; }
        public string estado { get; set; }
    }
    public class ModelReporteLogComunicaciondet
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string codproveedor { get; set; }
        public string ruc { get; set; }
        public string nomcomercial { get; set; }
        public string correoe { get; set; }
        public string dircalleprinc { get; set; }
    }

    public class ModelReporteLogComunicaciondetusuario
    {
        public string cod_notificacion { get; set; }
        public string cod_proveedor { get; set; }
        public string usuario { get; set; }
        public string identificacion { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string nombre1 { get; set; }
        public string nombre2 { get; set; }
        public string correoe { get; set; }
        public string funcion { get; set; }
        public string departamento { get; set; }
        public int rolAdmin { get; set; }
        public int rolComercial { get; set; }
        public int rolContable { get; set; }
        public int rolLogistico { get; set; }
    }


    public class ModelReporteAdminProveedorNoSolicitud
    {
        public string ruc { get; set; }
        public string codproveedor { get; set; }
        public string nomcomercial { get; set; }
        public string dirCalleNum { get; set; }
        public string telefono { get; set; }
        public string ciudad { get; set; }
        public string correoE { get; set; }
        public string representante { get; set; }
    }
    public class FormResponseReporte
    {
        public FormResponseReporte()
        {
            root = new List<Object>();
        }

        public string codError { get; set; }
        public string msgError { get; set; }

        public Boolean success { get; set; }

        public List<Object> root { get; set; }
    }
}
