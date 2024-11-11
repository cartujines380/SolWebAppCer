using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace AngularJSAuthentication.API.Models
{
    //**************************************************************************************************************
    //Modelo de Notificacion
    public class Notificacion
    {
        public int CodNotificacion { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public string Prioridad { get; set; }
        public string Comunicado { get; set; }        
        public string Obligatorio { get; set; }
        public string Corporativo { get; set; }
        public string Tipo { get; set; }
        public string FechaVencimiento { get; set; }
        public List<string> ListaAdjuntos { get; set; }        
        public List<Proveedor> ListaProveedores { get; set; }
        public List<LineaNegocio> ListaLineasNegocios { get; set; }
        public List<Departamentos> ListaDepartamentos { get; set; }
        public List<ZonasNot> ListaZonasNot { get; set; }
        public List<DepFuncNot> ListaDepFuncNot { get; set; }
        public List<Roles> ListaRolesNot { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
        public string Ruta { get; set; }
        public string Usuario { get; set; }
        public string MensajeCorreo { get; set; }
        public string TipoCorreo { get; set; }
        public string FechaPublicacion { get; set; }
    }

    //Modelo de Mensajes
    public class Mensajes
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Estado { get; set; }
    }

    //Modelo de Proveedor
    public class Proveedor
    {
        public string CodProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string Representante { get; set; }
        public string Correo { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public string FecAceptacion { get; set; }
        
    }
    public class LineaNegocio
    {
        public string Codigo { get; set; }        
        public string Descripcion { get; set; }
        
       

    }
    public class Departamentos
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }



    }

    public class DepFuncNot
    {
        public string IdDepartamento { get; set; }
        public string DesDepartamento { get; set; }
        public List<Funciones> ListaFunciones { get; set; }

    }
    public class Funciones
    {
        public string IdDepartamento { get; set; }
        public string IdFuncion { get; set; }
        public string DesFuncion { get; set; }
        public Boolean IsCheck { get; set; }

    }
    public class Roles
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
      

    }

    public class Clasificado
    {
        public Int64 NumClasificado { get; set; }
        public string Cargo { get; set; }
        public string Ciudad { get; set; }
        public string FechaPublicacion { get; set; }

    }

    //Modelo de Proveedor
    public class Producto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Archivo { get; set; }
        public Decimal Precio { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string NumRegistro { get; set; }
    }

    public class ConsultaDestinatarios
    {
        public string Correo { get; set; }
        public string Nombre { get; set; }
        public string Proveedor { get; set; }
        public string Ruc { get; set; }
        public string NomComercial { get; set; }
        public string CodDepartamento { get; set; }
        public string DesDepartamento { get; set; }
        
    }

    public class ZonasNot
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
    
    }


    public class FormResponseNotificacion
    {
        public FormResponseNotificacion()
        {
            root = new List<Object>();
        }

        public string codError { get; set; }
        public string msgError { get; set; }

        public Boolean success { get; set; }

        public List<Object> root { get; set; }
    }
    //**************************************************************************************************************

    //**************************************************************************************************************   
       
    //**************************************************************************************************************
}