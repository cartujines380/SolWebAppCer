using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WCFEnvioCorreo.Model;

namespace WCFEnvioCorreo.Servicios
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

        protected FormResponseModelo ConsumoGetApiAsync(string rutaServicio, string rutaApi, string nombreMetodo,
                                                         string autorizacion = null)
        {
            string url = string.Empty;

            FormResponseModelo Response = new FormResponseModelo()
            {
                success = true
            };

            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    if (parametros.Count > 0)
                    {
                        url = string.Format("{0}/{1}{2}?{3}", rutaServicio, rutaApi, nombreMetodo, parametros.ToString());
                    }
                    else
                    {
                        url = string.Format("{0}/{1}{2}", rutaServicio, rutaApi, nombreMetodo);
                    }

                    //cliente.DefaultRequestHeaders.Add("content-type", "application/json");
                    if (autorizacion != null)
                    {
                        var token = string.Format("Bearer {0}", autorizacion);
                        cliente.DefaultRequestHeaders.Add("Authorization", token);

                    }
                    var respuesta = cliente.GetAsync(new Uri(url)).Result;
                    //var contenido = await respuesta.Content.ReadAsStringAsync();               
                    //jsonRespuesta = JObject.Parse(contenido);
                    switch (respuesta.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            return RespuestaOk(respuesta);
                        case HttpStatusCode.Unauthorized:
                            return RespuestaUnauthorized(respuesta);
                        case HttpStatusCode.BadRequest:
                            return RespuestaBadRequest(respuesta);
                        default:
                            return Response = new FormResponseModelo()
                            {
                                success = false,
                                msgError = "ERROR ",
                                codError = "-2"
                            };
                    }
                }
                catch (Exception ex)
                {
                    return Response = new FormResponseModelo()
                    {
                        success = false,
                        msgError = "ERROR: " + ex.Message,
                        codError = "-1"
                    };
                }
            }

        }

        protected FormResponseModelo ConsumoPostAsync(string rutaServicio, string rutaApi, string nombreMetodo, object parametros,
                                               string autorizacion = null)
        {
            string Patch = string.Empty;

            Logger.Logger log = new Logger.Logger();
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();

            FormResponseModelo Response = new FormResponseModelo()
            {
                success = true
            };

            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    #region Log
                    log.FilePath = p_Log;
                    log.WriteMensaje("INI - ConsumoGetApiAsync");
                    #endregion Log

                    Patch = string.Format("{0}{1}", rutaApi, nombreMetodo);
                    if (parametros == null)
                    {
                        dynamic errorRetorno = new JObject();
                        errorRetorno.error = true;
                        errorRetorno.codRetorno = -1;
                        errorRetorno.Mensaje = "Los paràmetros no pueden ser nulos o vacios";
                        return errorRetorno;
                    }

                    if (autorizacion != null)
                    {
                        cliente.DefaultRequestHeaders.Add("Authorization", autorizacion);
                    }
                    System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    cliente.BaseAddress = new Uri(rutaServicio);

                    #region Log
                    log.FilePath = p_Log;
                    log.WriteMensaje("INFO - ConsumoGetApiAsync - URL: " + cliente.BaseAddress);
                    #endregion Log

                    cliente.DefaultRequestHeaders.Accept.Clear();
                    cliente.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application / json");

                    var json = JObject.FromObject(parametros);
                    #region Log
                    log.FilePath = p_Log;
                    log.WriteMensaje("INFO - ConsumoGetApiAsync - REQUEST SERVICIO: " + json);
                    #endregion Log

                    HttpContent httpContent = new StringContent(json.ToString());
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    #region Log
                    log.FilePath = p_Log;
                    log.WriteMensaje("INFO - ConsumoGetApiAsync - EJECUTA SERVICIO.");
                    #endregion Log
                    HttpResponseMessage message = cliente.PostAsync(Patch, httpContent).Result;

                    //var respuesta = cliente.PostAsync("Plantillas/v1/procesar", httpContent).Result;


                    var respuesta = message;
                    #region Log
                    log.FilePath = p_Log;
                    log.WriteMensaje("INFO - ConsumoGetApiAsync - RESPONSE SERVICIO: " + respuesta);

                    log.FilePath = p_Log;
                    log.WriteMensaje("Datos: " + rutaServicio + "---" + Patch);
                    #endregion Log

                    switch (respuesta.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            return RespuestaOk(respuesta);
                        case HttpStatusCode.Unauthorized:
                            return RespuestaUnauthorized(respuesta);
                        case HttpStatusCode.BadRequest:
                            return RespuestaBadRequest(respuesta);
                        default:
                            return Response = new FormResponseModelo()
                            {
                                success = false,
                                msgError = "ERROR ",
                                codError = "-2"
                            };
                    }
                }
                catch (Exception ex)
                {
                    #region Log
                    log.FilePath = p_Log;
                    log.WriteMensaje("ERROR: " + ex.Message);
                    log.WriteMensaje("FIN - ConsumoGetApiAsync");
                    #endregion Log

                    return Response = new FormResponseModelo()
                    {
                        success = false,
                        msgError = "ERROR: " + ex.Message,
                        codError = "-1"
                    };
                }
            }
        }


        #region Respuestas Servicio

        protected FormResponseModelo RespuestaOk(HttpResponseMessage respuesta)
        {
            JObject jsonRespuesta = null;
            Logger.Logger log = new Logger.Logger();
            string p_Log = System.Configuration.ConfigurationManager.AppSettings["RutaLog"].Trim();

            FormResponseModelo Response = new FormResponseModelo()
            {
                success = true
            };

            try
            {
                #region Log
                log.FilePath = p_Log;
                log.WriteMensaje("INFO: SERVICE RESPUESTA OK.");
                #endregion Log

                var contenido = respuesta.Content.ReadAsStringAsync().Result;

                if (contenido.StartsWith("\""))
                {
                    string responseString = contenido.Replace("\"", "");

                    var ResponseService = new System.Collections.Generic.Dictionary<string, string>();
                    ResponseService.Add("data", responseString);

                    jsonRespuesta = JObject.FromObject(ResponseService);
                    Response.root.Add(jsonRespuesta);
                    return Response;
                }

                else if (contenido.StartsWith("<html"))
                {
                    Response.root.Add(contenido);
                    return Response;

                }
                else
                {
                    jsonRespuesta = JObject.Parse(contenido);
                }

                

                Response.root.Add(jsonRespuesta);
            }
            catch (Exception ex)
            {
                #region Log
                log.FilePath = p_Log;
                log.WriteMensaje("ERROR: SERVICE RESPUESTA OK " + ex.Message);
                #endregion Log

                Response = new FormResponseModelo()
                {
                    success = false,
                    msgError = "ERROR: " + ex.Message,
                    codError = "-1"
                };
            }

            return Response;
        }

        protected FormResponseModelo RespuestaBadRequest(HttpResponseMessage respuesta)
        {
            JObject jsonRespuesta = null;
            FormResponseModelo Response = new FormResponseModelo()
            {
                success = false,
                codError = "400"
            };

            try
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                jsonRespuesta = JObject.Parse(contenido);
                ErrorResponseApi error = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponseApi>(jsonRespuesta.ToString());
                Response.root.Add(error);

                Response.msgError = error.Message;

            }
            catch (Exception ex)
            {
                Response = new FormResponseModelo()
                {
                    success = false,
                    msgError = "ERROR: " + ex.Message,
                    codError = "-1"
                };
            }

            return Response;
        }

        protected FormResponseModelo RespuestaUnauthorized(HttpResponseMessage respuesta)
        {
            JObject jsonRespuesta = null;
            FormResponseModelo Response = new FormResponseModelo()
            {
                success = false,
                codError = "500"
            };
            try
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                jsonRespuesta = JObject.Parse(contenido);
                //jsonRespuesta = JObject.Parse(jsonRespuesta.SelectToken("Message").ToString());
                jsonRespuesta = JObject.Parse(contenido);
                Response.root.Add(jsonRespuesta);

            }
            catch (Exception ex)
            {
                Response = new FormResponseModelo()
                {
                    success = false,
                    msgError = "ERROR: " + ex.Message,
                    codError = "-1"
                };
            }

            return Response;
        }

        #endregion

    }
}