namespace AngularJSAuthentication.API.Models
{
    public class ContratoLicitacion
    {
        public string IdContrato { get; set; }
        public string IdAdquisicion { get; set; }
        public string NombreLicitacion { get; set; }
        public string CodTipoContrato { get; set; }
        public string CodLineaNegocio { get; set; }
        public string CodTipoServicio { get; set; }
        public string CodPlazoSuscripcion { get; set; }
        public string AdminBG { get; set; }
        public string CorreoAdminBG { get; set; }
        public string AdministradorContrato { get; set; }
        public string CorreoAdminPrv { get; set; }
        public string Usuario { get; set; }

    }
}