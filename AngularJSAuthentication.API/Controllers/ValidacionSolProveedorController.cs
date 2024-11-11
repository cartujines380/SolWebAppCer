using AngularJSAuthentication.API.Models;
using AngularJSAuthentication.API.Servicios;
using clibLogger;
using clibProveedores;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace AngularJSAuthentication.API.Controllers
{

    [RoutePrefix("api/ValidacionSolProveedor")]
    public class ValidacionSolProveedorController: ApiController
    {
        [ActionName("GetValidaActividadEconomica")]
        [HttpGet]
        public async Task<String> ValidadActividadEconomica(string identificacion, string actividadEconomica)
        {
            string respuesta = string.Empty;
            //Logger.Logger log = new Logger.Logger();
            clsLogger log = new clsLogger();
            //string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();

            try
            {
                //log.FilePath = p_Log;
                log.Graba_Log_Info("[GetValidaActividadEconomica] -> Envia: " + identificacion + " -" + actividadEconomica
                    );
                //log.Linea();
                ValidacionesBG api = new ValidacionesBG();
                var res = await api.ActividadEconomicaPermitida(identificacion, actividadEconomica);
                respuesta = res;
            }
            catch (Exception ex)
            {
                respuesta = "False|" + ex.Message.ToString().Substring(0,50);
                //log.FilePath = p_Log;
                log.Graba_Log_Error("Error GetValidaActividadEconomica: " + ex.ToString());
                //log.FilePath = p_Log;
                //log.Linea();
            }


            return respuesta;
        }

       private string ObtieneIdentificacion(string valor)
        {
            string respuesta = string.Empty;
            switch (valor)
            {
                case "RC":
                    respuesta = "r";
                    break;
                case "CD":
                    respuesta = "c";
                    break;
                case "PS":
                    respuesta = "p";
                    break;
                default:
                    respuesta = "r";
                    break;
            }
            return respuesta;

        }

        private string CalcularEdad(DateTime valor)
        {
            try
            {                            
                int edad = DateTime.Today.AddTicks(-valor.Ticks).Year - 1;
                return edad.ToString();
            }
            catch (Exception)
            {
                return "0";
            }
        }

        [ActionName("ValidacionPoliticas")]
        [HttpPost]
        public async Task<String> GetValidacionPoliticas([FromBody] DMSolcitudProveedor SolProveedorV)
        {
            string respuesta = string.Empty;
            //Logger.Logger log = new Logger.Logger();
            //string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            clsLogger log = new clsLogger();
            string p_tipoProducto = ((string)System.Configuration.ConfigurationManager.AppSettings["TipoProducto"]).Trim();
            try
            {

                if (SolProveedorV.p_SolProvContacto == null || SolProveedorV.p_SolProvContacto.Count() == 0)
                {
                    respuesta = "False|Sin Contacto registrado";
                    return respuesta;
                }
                //log.FilePath = p_Log;
                log.Graba_Log_Info("[GetValidacionPoliticas] -> Envia: " + SolProveedorV.ToString() + " -" 
                    );
                //log.Linea();
                ValidacionesBG api = new ValidacionesBG();
                MDValidacionProveedor informacionProveedor = new MDValidacionProveedor();
                MDValidacionProveedor.Deudor deudor = new MDValidacionProveedor.Deudor();
                MDValidacionProveedor.Participante participante = new MDValidacionProveedor.Participante();

                var representanteLegal = (from p in SolProveedorV.p_SolProvContacto.Where(s => s.RepLegal == true)
                                          select new MDValidacionProveedor.Participante.RegistroParticipante
                                          {
                                              fechanacimiento = p.FechaNacimiento.ToString() != null ? p.FechaNacimiento.ToString("dd/MM/yyyy") : "",
                                              edad = CalcularEdad(p.FechaNacimiento),
                                              //p.FechaNacimiento.ToString() != null ? ((DateTime.Today.AddTicks(-p.FechaNacimiento.Ticks).Year) - 1).ToString() : "",
                                              nacionalidad = p.Nacionalidad != null ? p.Nacionalidad : "",
                                              estadocivil = p.EstadoCivil != null ? p.EstadoCivil : "",
                                              idregimenmatrimonial = p.RegimenMatrimonial != null ? p.RegimenMatrimonial : "2159",
                                              relaciondepenlaboral = p.RelacionDependencia != null ? p.RelacionDependencia : "",
                                              antiguedadlaboral = p.AntiguedadLaboral != null ? p.AntiguedadLaboral : "",
                                              tipoingreso = p.TipoIngreso != null ? p.TipoIngreso : "",
                                              ingresosmensuales = p.IngresoMensual != null ? p.IngresoMensual : "",
                                          }).ToArray();

                deudor.idProducto = p_tipoProducto;
                deudor.idexpediente = "2";
                deudor.origen = "neo";
                deudor.tipoidentificacion = SolProveedorV.p_SolProveedor[0].TipoIdentificacion != null ? ObtieneIdentificacion(SolProveedorV.p_SolProveedor[0].TipoIdentificacion) : "";
                deudor.identificacion = SolProveedorV.p_SolProveedor[0].Identificacion != null ? SolProveedorV.p_SolProveedor[0].Identificacion : "";
                deudor.nombres = SolProveedorV.p_SolProveedor[0].RazonSocial != null ? SolProveedorV.p_SolProveedor[0].RazonSocial : "";
                deudor.apellidos = SolProveedorV.p_SolProveedor[0].RazonSocial != null ? SolProveedorV.p_SolProveedor[0].RazonSocial : "";
                deudor.nombrecompleto = SolProveedorV.p_SolProveedor[0].RazonSocial != null ? SolProveedorV.p_SolProveedor[0].RazonSocial : "";
                deudor.fechanacimiento = SolProveedorV.p_SolProveedor[0].FechaCreacion != null ? SolProveedorV.p_SolProveedor[0].FechaCreacion.ToString("dd/MM/yyyy") : "";
                deudor.edad = SolProveedorV.p_SolProveedor[0].FechaCreacion != null ? ((DateTime.Today.AddTicks(-SolProveedorV.p_SolProveedor[0].FechaCreacion.Ticks).Year) - 1).ToString() : "";
                deudor.nacionalidad = representanteLegal[0].nacionalidad;
                deudor.estadocivil = representanteLegal[0].estadocivil;
                deudor.regimenmatrimonial = representanteLegal[0].idregimenmatrimonial != null ? representanteLegal[0].idregimenmatrimonial : "2159";
                deudor.relaciondepenlaboral = representanteLegal[0].relaciondepenlaboral;
                deudor.antiguedadlaboral = CalcularEdad(SolProveedorV.p_SolProveedor[0].FechaCreacion);
                    //SolProveedorV.p_SolProveedor[0].FechaCreacion != null ? ((DateTime.Today.AddTicks(-SolProveedorV.p_SolProveedor[0].FechaCreacion.Ticks).Year) - 1).ToString() : ""; //cambiar a la edad de l empresa
                deudor.tipoingreso = representanteLegal[0].tipoingreso;
                deudor.ingresosmensuales = representanteLegal[0].ingresosmensuales;
                deudor.montosolicitad = "0";
                deudor.plazosolicitado = "0";
                deudor.periocidad = "1";
                deudor.usuariovalidacion = "USR_SYSTEM";

                MDValidacionProveedor.Participante.RegistroParticipante[] registroparticipantes =
                    (from p in SolProveedorV.p_SolProvContacto
                     select new MDValidacionProveedor.Participante.RegistroParticipante
                     {
                         idtipoparticipante = p.TipoParticipante != null ? p.TipoParticipante : "",
                         tipoidentificacion = p.TipoIdentificacion != null ? ObtieneIdentificacion(p.TipoIdentificacion) : "",
                         identificacion = p.Identificacion != null ? p.Identificacion : "",
                         nombres = string.Join(string.Empty, new string[] { p.Nombre1 != null ? p.Nombre1 : "", " ", p.Nombre2 != null ? p.Nombre2 : "" }),
                         apellidos = string.Join(string.Empty, new string[] { p.Apellido1 != null ? p.Apellido1 : "", " ", p.Apellido2 != null ? p.Apellido2 : "" }),
                         nombrescompleto = string.Join(string.Empty, new string[] { p.Nombre1 != null ? p.Nombre1 : "", " ", p.Nombre2 != null ? p.Nombre2 : "", " ", p.Apellido1 != null ? p.Apellido1 : "", " ", p.Apellido2 != null ? p.Apellido2 : "" }),
                         fechanacimiento = p.FechaNacimiento.ToString() != null ? p.FechaNacimiento.ToString("dd/MM/yyyy") : "",
                         edad = p.FechaNacimiento.ToString() != null ? CalcularEdad(p.FechaNacimiento) : "",                         
                         nacionalidad = p.Nacionalidad != null ? p.Nacionalidad : "",
                         idregimenmatrimonial = p.RegimenMatrimonial != null ? p.RegimenMatrimonial : (p.EstadoCivil == "2453" ? "2161" : "")
                     })
                     .Union
                     (from p in SolProveedorV.p_SolProvContacto
                      where (p.EstadoCivil == "2452" || p.EstadoCivil == "2456") && p.RepLegal == true
                      select new MDValidacionProveedor.Participante.RegistroParticipante
                      {
                          idtipoparticipante = "3110",
                          tipoidentificacion = p.ConyugeTipoIdentificacion != null ? ObtieneIdentificacion(p.ConyugeTipoIdentificacion) : "",
                          identificacion = p.ConyugeIdentificacion != null ? p.ConyugeIdentificacion : "",
                          nombres = string.Join(string.Empty, new string[] { p.ConyugeNombres != null ? p.ConyugeNombres : "" }),
                          apellidos = string.Join(string.Empty, new string[] { p.ConyugeApellidos != null ? p.ConyugeApellidos : "" }),
                          nombrescompleto = string.Join(string.Empty, new string[] { p.ConyugeNombres != null ? p.ConyugeNombres : "", " ", p.ConyugeApellidos != null ? p.ConyugeApellidos : "" }),
                          fechanacimiento = p.ConyugeFechaNac.ToString() != null ? p.ConyugeFechaNac.ToString("dd/MM/yyyy") : "",
                          edad = p.ConyugeFechaNac.ToString() != null ? CalcularEdad(p.ConyugeFechaNac) : "",                          
                          nacionalidad = p.ConyugeNacionalidad != null ? p.ConyugeNacionalidad : "",
                          idregimenmatrimonial = p.RegimenMatrimonial != null ? p.RegimenMatrimonial : "2159"
                      })
                      .Union
                      (from p in SolProveedorV.p_SolProvContacto
                       where (p.EstadoCivil == "2452" || p.EstadoCivil == "2456") && p.RepLegal == false
                       select new MDValidacionProveedor.Participante.RegistroParticipante
                       {
                           idtipoparticipante = "350",
                           tipoidentificacion = p.ConyugeTipoIdentificacion != null ? ObtieneIdentificacion(p.ConyugeTipoIdentificacion) : "",
                           identificacion = p.ConyugeIdentificacion != null ? p.ConyugeIdentificacion : "",
                           nombres = string.Join(string.Empty, new string[] { p.ConyugeNombres != null ? p.ConyugeNombres : "" }),
                           apellidos = string.Join(string.Empty, new string[] { p.ConyugeApellidos != null ? p.ConyugeApellidos : "" }),
                           nombrescompleto = string.Join(string.Empty, new string[] { p.ConyugeNombres != null ? p.ConyugeNombres : "", " ", p.ConyugeApellidos != null ? p.ConyugeApellidos : "" }),
                           fechanacimiento = p.ConyugeFechaNac.ToString() != null ? p.ConyugeFechaNac.ToString("dd/MM/yyyy") : "",
                           edad = p.ConyugeFechaNac.ToString() != null ? CalcularEdad(p.ConyugeFechaNac) : "",
                           nacionalidad = p.ConyugeNacionalidad != null ? p.ConyugeNacionalidad : "",
                           idregimenmatrimonial = p.RegimenMatrimonial != null ? p.RegimenMatrimonial : "2159"
                       })
                      .ToArray();                               

                participante.registroparticipante = registroparticipantes;
                informacionProveedor.participante = participante;
                informacionProveedor.deudor = deudor;

                var informacionP = new
                {
                    informacion = informacionProveedor
                };

                var obj = JObject.FromObject(informacionP);
                //log.FilePath = p_Log;
                log.Graba_Log_Info("[GetValidacionPoliticas] -> Fin: " + obj.ToString() + " -"
                    );
                //log.Linea();
                var res = await api.ValidacionPoliticas(obj);
                respuesta = res;

            }
            catch (Exception ex)
            {
                respuesta = "False|" + ex.Message.ToString().Substring(0, 50); ;
                
                //log.FilePath = p_Log;
                log.Graba_Log_Error("Error GetValidacionPoliticas: " + ex.ToString());
                //log.FilePath = p_Log;
                //log.Linea();
            }

            return respuesta;
        }
    }
}