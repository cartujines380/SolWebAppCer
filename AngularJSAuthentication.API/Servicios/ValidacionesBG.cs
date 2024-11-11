using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AngularJSAuthentication.API.Servicios
{
    public class ValidacionesBG: ApiServices
    {
        //private string rutaApi = "actividad-economica/";
        private string rutaServicio = ConfigurationManager.AppSettings["RutaServiciosBGApi"].ToString();


        public async Task<string> ObtenerToken()
        {
            string respuesta = string.Empty;
            var clientId = InitialiseService.BgClientID;
            var tenant = InitialiseService.BgIdDirectorio != null ? InitialiseService.BgIdDirectorio : "";
            try
            {
                string p_LoginBG = String.Format(System.Globalization.CultureInfo.InvariantCulture, System.Configuration.ConfigurationManager.AppSettings["Authority"], tenant);

                
                var parametrosApi = new List<Tuple<string, string>>();

                parametrosApi.Add(Tuple.Create("grant_type", "client_credentials"));
                parametrosApi.Add(Tuple.Create("client_id", InitialiseService.BgClientID != null ? InitialiseService.BgClientID : ""));
                parametrosApi.Add(Tuple.Create("client_secret", InitialiseService.BgSecret != null ? InitialiseService.BgSecret : ""));
                parametrosApi.Add(Tuple.Create("scope", InitialiseService.BgScope != null ? InitialiseService.BgScope : ""));

                var res = await AutorizacionApi(p_LoginBG, parametrosApi);
                respuesta = res["access_token"].ToString() != null ? res["access_token"].ToString() : "";
            }
            catch (Exception)
            {
                return null;
            }
            

            return respuesta;
        }

        public async Task<string> ActividadEconomicaPermitida(string identificacion, string actividadEconomica)
        {
            string respuesta = string.Empty;
            //var token = await ObtenerToken();
            //var res = await ConsumoGetApiAsync(rutaServicio, "policy/v1/actividad_economica/" + actividadEconomica + "/" + identificacion, "",token);
            //respuesta = res["actividadEconomica"].ToString() + "|" + res["mensaje"].ToString();
            respuesta = "True|OK";
            return respuesta;

        }

        public async Task<string> ValidacionPoliticas(object proveedor)
        {
            string respuesta = string.Empty;
            var token = await ObtenerToken();
            var res = await ConsumoPostAsync(rutaServicio, "policy/v1/modelo_experto", "", proveedor, token);
            respuesta = res["dictamenExperto"].ToString() + "|" + res["mensaje"].ToString(); ;

            return respuesta;
        }


    }
}