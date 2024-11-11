using clibLogger;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AngularJSAuthentication.API.Servicios
{
    public class ApiServices
    {
        private NameValueCollection parametros;
        public ApiServices()
        {
            parametros = HttpUtility.ParseQueryString(string.Empty);
        }
        protected bool ParametrosGet(string codigo, string valor)
        {

            parametros.Add(codigo, valor);

            return true;

        }
        protected bool LimpiarParametros()
        {
            parametros.Clear();
            return true;
        }

        protected async Task<JObject> AutorizacionApi(string request, List<Tuple<string, string>> param)
        {
            //Logger.Logger log = new Logger.Logger();
            clsLogger log = new clsLogger();
            //string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            try
            {

                using (HttpClient cliente = new HttpClient())
                {
                    var paramUrl = new List<KeyValuePair<string, string>>();
                    foreach (var item in param)
                    {
                        paramUrl.Add(new KeyValuePair<string, string>(item.Item1, item.Item2));
                    }

                    //log.FilePath = p_Log;
                    log.Graba_Log_Info("[AutorizacionApi] -> " + "Request: " + request + " Param: " + paramUrl.ToString()
                        );
                    //log.Linea();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var req = new HttpRequestMessage(HttpMethod.Post, request) { Content = new FormUrlEncodedContent(paramUrl) };
                    var respuestaApi = await cliente.SendAsync(req);
                    
                    if (respuestaApi.IsSuccessStatusCode)
                    {
                        var contenido = await respuestaApi.Content.ReadAsStringAsync();
                        return JObject.Parse(contenido);
                    }
                    else
                    {
                        var contenido = await respuestaApi.Content.ReadAsStringAsync();
                        //log.FilePath = p_Log;
                        log.Graba_Log_Info("[AutorizacionApi no IsSuccessStatusCode] -> Envia: " + contenido + " -"
                            );
                        //log.Linea();
                    }
                    

                }
                                    
                

                return new JObject { { "access_token", "" }, { "token_type", "" } };
            }
            catch (Exception errorAut)
            {
                //log.FilePath = p_Log;
                log.Graba_Log_Error("Error AutorizacionApi: " + errorAut.ToString());
                //log.FilePath = p_Log;
                //log.Linea();
                return new JObject { { "access_token", "" }, { "token_type", "" } };
            }
        }

        protected async Task<JObject> ConsumoGetApiAsync(string rutaServicio, string rutaApi, string nombreMetodo,
                                                         string autorizacion = null)
        {
            string url = string.Empty;
            clsLogger log = new clsLogger();
            //Logger.Logger log = new Logger.Logger();
            //string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    if (parametros.Count> 0)
                    {
                        url = string.Format("{0}/{1}{2}?{3}", rutaServicio, rutaApi, nombreMetodo, parametros.ToString());
                    }
                    else
                    {
                        url = string.Format("{0}/{1}{2}", rutaServicio, rutaApi, nombreMetodo);
                    }

                    //log.FilePath = p_Log;
                    log.Graba_Log_Info("[ConsumoGetApiAsync] -> Envia: " + url + " -"
                        );
                    //log.Linea();

                    //cliente.DefaultRequestHeaders.Add("content-type", "application/json");
                    if (autorizacion != null && autorizacion.Trim().Length > 0)
                    {
                        //log.FilePath = p_Log;
                        log.Graba_Log_Info("[ConsumoGetApiAsync] -> Authorization: "
                            );
                        //log.Linea();
                        var token = string.Format("Bearer {0}", autorizacion);
                        cliente.DefaultRequestHeaders.Add("Authorization", token);
                        
                    }
                    var respuesta = await cliente.GetAsync(new Uri(url));
                    //var contenido = await respuesta.Content.ReadAsStringAsync();               
                    //jsonRespuesta = JObject.Parse(contenido);
                    switch (respuesta.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            //log.FilePath = p_Log;
                            log.Graba_Log_Info("[ConsumoGetApiAsync] -> ok: "
                                );
                            //log.Linea();
                            return await RespuestaOk(respuesta);
                        case HttpStatusCode.Unauthorized:
                            //log.FilePath = p_Log;
                            log.Graba_Log_Info("[ConsumoGetApiAsync] -> respuestaApi.Unauthorized " + respuesta.StatusCode.ToString()
                                );
                            //log.Linea();
                            return await RespuestaUnauthorized(respuesta);
                        case HttpStatusCode.BadRequest:
                            //log.FilePath = p_Log;
                            log.Graba_Log_Info("[ConsumoGetApiAsync] -> respuestaApi.BadRequest " + respuesta.StatusCode.ToString()
                                );
                            //log.Linea();
                            return await RespuestaBadRequest(respuesta);
                        default:
                            //log.FilePath = p_Log;
                            log.Graba_Log_Info("[ConsumoGetApiAsync] -> respuestaApi.StatusCode " + respuesta.StatusCode.ToString()
                                );
                            //log.Linea();
                            return new JObject { { "actividadEconomica", "False" }, { "dictamenExperto", "False" }, { "mensaje", "NotFound" } };
                    };
                }
                catch (Exception ex)
                {
                    //log.FilePath = p_Log;
                    log.Graba_Log_Error("Error ConsumoGetApiAsync: " + ex.ToString());
                    //log.FilePath = p_Log;
                    //log.Linea();
                    return new JObject { { "actividadEconomica", "False" }, { "dictamenExperto", "False" }, { "mensaje", ex.Message.ToString().Substring(0,50) } };
                }
            }

        }

        protected async Task<JObject> ConsumoPostAsync(string rutaServicio, string rutaApi, string nombreMetodo, object parametros,
                                                       string autorizacion = null)
        {
            string url = string.Empty;
            string respuesta = string.Empty;
            clsLogger log = new clsLogger();
            //Logger.Logger log = new Logger.Logger();
            //string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    url = string.Format("{0}/{1}{2}", rutaServicio, rutaApi, nombreMetodo);
                    //log.FilePath = p_Log;
                    log.Graba_Log_Info("[ConsumoPostAsync] -> Envia: " + url + " -"
                        );
                    //log.Linea();

                    if (parametros == null)
                    {
                        dynamic errorRetorno = new JObject();
                        errorRetorno.error = true;
                        errorRetorno.codRetorno = -1;
                        errorRetorno.Mensaje = "Los paràmetros no pueden ser nulos o vacios";
                        return errorRetorno;
                    }
                    if (autorizacion != null && autorizacion.Trim().Length > 0)
                    {
                        //log.FilePath = p_Log;
                        log.Graba_Log_Info("[ConsumoPostAsync] -> Authorization: " 
                            );
                        //log.Linea();
                        var token = string.Format("Bearer {0}", autorizacion);
                        cliente.DefaultRequestHeaders.Add("Authorization", token);
                    }

                    var respuestaApi = await cliente.PostAsJsonAsync(url, parametros);
                    switch (respuestaApi.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            //log.FilePath = p_Log;
                            log.Graba_Log_Info("[ConsumoPostAsync] -> ok: "
                                );
                            //log.Linea();
                            return await RespuestaOk(respuestaApi);
                        case HttpStatusCode.Unauthorized:
                            //log.FilePath = p_Log;
                            log.Graba_Log_Info("[ConsumoPostAsync] -> Unauthorized: "
                                );
                            //log.Linea();
                            return await RespuestaUnauthorized(respuestaApi);
                        case HttpStatusCode.BadRequest:
                            //log.FilePath = p_Log;
                            log.Graba_Log_Info("[ConsumoPostAsync] -> BadRequest: "
                                );
                            //log.Linea();
                            return await RespuestaBadRequest(respuestaApi);
                        default:
                            //log.FilePath = p_Log;
                            log.Graba_Log_Info("[ConsumoPostAsync] -> respuestaApi.StatusCode " + respuestaApi.StatusCode.ToString()
                                );
                            //log.Linea();
                            return new JObject { { "actividadEconomica", "False" }, { "dictamenExperto", "False" }, { "mensaje", "NotFound" } };
                    };
                }
                catch (Exception ex)
                {
                    //log.FilePath = p_Log;
                    log.Graba_Log_Error("Error ConsumoPostAsync: " + ex.ToString());
                    //log.FilePath = p_Log;
                    //log.Linea();
                    return new JObject { { "actividadEconomica", "False" }, { "dictamenExperto", "False" }, { "mensaje", ex.Message.ToString().Substring(0, 400) } };
                }
            }
        }

        protected async Task<JObject> RespuestaOk(HttpResponseMessage respuesta)
        {
            JObject jsonRespuesta = null;
            try
            {
                Logger.Logger log = new Logger.Logger();
                string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();

                var contenido = await respuesta.Content.ReadAsStringAsync();
                jsonRespuesta = JObject.Parse(contenido);
                log.FilePath = p_Log;
                log.WriteMensaje("[ConsumoGetApiAsync] -> RespuestaOk " + jsonRespuesta.ToString()
                    );
                log.Linea();
            }
            catch (Exception ex)
            {
                jsonRespuesta = new JObject
                {
                    { "actividadEconomica", "False" },
                    { "dictamenExperto", "False" },
                    { "mensaje", ex.Message.ToString().Substring(0,50) }
                };
            }

            return jsonRespuesta;
        }

        protected async Task<JObject> RespuestaBadRequest(HttpResponseMessage respuesta)
        {
            JObject jsonRespuesta = null;
            Logger.Logger log = new Logger.Logger();
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();

            try
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                jsonRespuesta = JObject.Parse(contenido);
                log.FilePath = p_Log;
                log.WriteMensaje("[ConsumoGetApiAsync] -> RespuestaBadRequest " + jsonRespuesta.ToString()
                    );
                log.Linea();

            }
            catch (Exception ex)
            {
                jsonRespuesta = new JObject
                {
                    { "actividadEconomica", "False" },
                    { "dictamenExperto", "False" },
                    { "mensaje", ex.Message.ToString().Substring(0,50) }
                };
            }

            return jsonRespuesta;
        }

        protected async Task<JObject> RespuestaUnauthorized(HttpResponseMessage respuesta)
        {
            JObject jsonRespuesta = null;
            Logger.Logger log = new Logger.Logger();
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            try
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                jsonRespuesta = JObject.Parse(contenido);                
                log.FilePath = p_Log;
                log.WriteMensaje("[ConsumoGetApiAsync] -> RespuestaUnauthorized " + jsonRespuesta.ToString()
                    );
                log.Linea();
            }
            catch (Exception ex)
            {
                jsonRespuesta = new JObject
                {
                    { "actividadEconomica", "False" },
                    { "dictamenExperto", "False" },
                    { "mensaje", ex.Message.ToString().Substring(0,50) }
                };
            }

            return jsonRespuesta;
        }

    }
}