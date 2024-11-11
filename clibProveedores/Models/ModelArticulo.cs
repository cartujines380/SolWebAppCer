using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace clibProveedores.Models
{
    //**************************************************************************************************************
    //Para la pantalla de Consulta de solicitudes
    public class Art_ConsultaSolicitud
    {
        public string IdSolicitud { get; set; }
        public string IdTipoSolicitud { get; set; }
        public string TipoSolArticulo { get; set; }
        public string CodSapProveedor { get; set; }
        public string CodSapContacto { get; set; }
        public string CantidadArticulos { get; set; }
        public string Fecha { get; set; }
        public string EstadoSolicitud { get; set; }
        public string LineaNegocio { get; set; }
        public string Usuario { get; set; }
        public string Observacion { get; set; }
        public string RucProveedor { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        
    }

    //**************************************************************************************************************

    //**************************************************************************************************************
    //Consulta de caracteristicas de artículos
    public class Art_ConsultaCaracteristicas
    {
        public string Id { get; set; }
        public string Agrupacion { get; set; }
        public string Descripcion { get; set; }
        public Boolean Modificable { get; set; }
        public Boolean Heredable { get; set; }
        public Boolean Lista { get; set; }
        public string OrigenLista { get; set; }
        public List<TablaCatalogo> listaValor { get; set; }
       
       

    }

    //**************************************************************************************************************


    //**************************************************************************************************************
    //Para la pantalla de Consulta de Articulos
    public class Art_ConsultaArticulo
    {
        public string CodSap { get; set; }
        public string CodReferencia { get; set; }
        public string Marca { get; set; }
        public string Descripcion { get; set; }
        public string PersonaContacto { get; set; }
        public string Email { get; set; }
        public string PaisOrigen { get; set; }
        public string TipoArticulo { get; set; }
        public Boolean EsGenerico { get; set; }
    }

   
    //**************************************************************************************************************

  

    //**************************************************************************************************************

    //**************************************************************************************************************
    //Para consulta de varios modelos
    public class FormResponseArticulo
    {
        public FormResponseArticulo()
        {
            root = new List<Object>();
            root2 = new List<Object>();
        }
        

       public Boolean success { get; set; }
       public String mensaje { get; set; }
       public List<Object> root { get; set; }
       public List<Object> root2 { get; set; }
    }
    //**************************************************************************************************************

    public class Art_ReporteSolArticulo
    {
        public Art_ReporteSolArticuloCab p_cabeceraSolArticulo { get; set; }
        public Art_ReporteSolArticuloDet[] p_detalleSolArticulo { get; set; }
        public class Art_ReporteSolArticuloCab
        {
            public string tipo { get; set; }
            public string tipoSolicitud { get; set; }
            public string usuario { get; set; }
            public string proveedor { get; set; }
            public string numSolicitud { get; set; }
            public string pais { get; set; }
            public string responsable { get; set; }
            public string email { get; set; }
            public string departamento { get; set; }
            public string aprobacion { get; set; }
            public string contacto { get; set; }
            public string ruc { get; set; }
            public string codSociedad { get; set; }
            public string tipoMaterial { get; set; }
            public string desCodSociedad { get; set; }
            public string desTipoMaterial { get; set; }
            public string codSapProveedor { get; set; }
            public string codLegacy { get; set; }
            public string catMaterial { get; set; }
            public string desCatMaterial { get; set; }
        }
        public class Art_ReporteSolArticuloDet
        {
            public string codigo { get; set; }
            public string marca { get; set; }
            public string descripcion { get; set; }
            public string texto { get; set; }
            public string presentacion { get; set; }
            public string alcohol { get; set; }
            public string modelo { get; set; }
            public string iva { get; set; }
            public string deducible { get; set; }
            public string retencion { get; set; }
            public string CodSapGenerado { get; set; }
            public string GrupoArticulo { get; set; }
            public string UMbase { get; set; }
            public string UMpedido { get; set; }
            public string UxC { get; set; }
            public string CodBarra { get; set; }
            public string PesoNeto { get; set; }
            public string PesoBruto { get; set; }
            public string Longitud { get; set; }
            public string Ancho { get; set; }
            public string Altura { get; set; }
            public string Volumen { get; set; }
            public string CatValoracion { get; set; }
            public string PaisOrigen { get; set; }
            public string GrupoCompras { get; set; }
            public string IndPedido { get; set; }
            public string Temporada { get; set; }
            public string Coleccion { get; set; }
            public string Materia { get; set; }
            public string CantPedir { get; set; }
            public string Catalogacion { get; set; }
            public string SurtidoParcial { get; set; }
            public string CostoFob { get; set; }
            public string Desc1 { get; set; }
            public string Desc2 { get; set; }
            public string PerfilDistribucion { get; set; }
            public string Almacen { get; set; }
            public string TipoAlmacen { get; set; }
            public string Almacenamiento { get; set; }
            public string Entrada { get; set; }
            public string Salida { get; set; }
            public string CtdMac { get; set; }
            public string UM { get; set; }
            public string TUA { get; set; }
            public string MatReferencia { get; set; }
            public string CenReferencia { get; set; }
            public string Afecha { get; set; }
            public string multiplicador { get; set; }
            public string estado { get; set; }

        }
    }
}