using System.Configuration;
using System.Threading.Tasks;
using WCFEnvioCorreo.Model;

namespace WCFEnvioCorreo.Servicios
{
    public class EnviaMailBG: ApiServices
    {
        private readonly string rutaServicio = ConfigurationManager.AppSettings["RutaServiciosBGApi"].ToString();

        //public async Task<FormResponseModelo> Procesar2(object body)
        //{
        //    var respuesta = await ConsumoPostAsync2(rutaServicio, "/Plantillas/v1/", "procesar", body);
        //    return respuesta;
        //}

        //public async Task<FormResponseModelo> Enviar2(object body)
        //{
        //    var respuesta = await ConsumoPostAsync2(rutaServicio, "/notificaciones/v1/", "enviar", body);
        //    return respuesta;
        //}

        public FormResponseModelo Procesar(object body)
        {
            var respuesta = ConsumoPostAsync(rutaServicio, "/Plantillas/v1/", "procesar", body);
            return respuesta;
        }

        public FormResponseModelo Enviar(object body)
        {
            var respuesta = ConsumoPostAsync(rutaServicio, "/notificaciones/v1/", "enviar", body);
            return respuesta;
        }

    }
}