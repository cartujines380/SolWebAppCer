using AngularJSAuthentication.API.Models;
using AngularJSAuthentication.API.Servicios;
using clibProveedores;
using Newtonsoft.Json;
using Renci.SshNet;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace AngularJSAuthentication.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/SolicitudProveedor")]

    public class SolicitudProveedorController : ApiController
    {
        private string _clase;

        public SolicitudProveedorController()
        {
            _clase = GetType().Name;
        }

        private List<DMSolcitudProveedor.SolProveedor> MapSolProveedor(DataSet dsSolicitud)
        {
            List<DMSolcitudProveedor.SolProveedor> Retorno = new List<DMSolcitudProveedor.SolProveedor>();
            Retorno = (from reg in dsSolicitud.Tables[0].AsEnumerable()
                       select new DMSolcitudProveedor.SolProveedor
                       {
                           Aprobacion = reg.Field<String>("Aprobacion"),
                           Autorizacion = reg.Field<String>("Autorizacion"),
                           CodGrupoProveedor = reg.Field<String>("CodGrupoProveedor"),
                           CodSapProveedor = reg.Field<String>("CodSapProveedor"),
                           Comentario = reg.Field<String>("Comentario"),
                           CuentaAsociada = reg.Field<String>("CuentaAsociada"),
                           DepSolicitando = reg.Field<String>("DepSolicitando"),
                           DescCuentaAsociada = reg.Field<String>("DescCuentaAsociada"),
                           DescEstado = reg.Field<String>("DescEstado"),
                           DescGrupoTesoreria = reg.Field<String>("DescGrupoTesoreria"),
                           DescIdioma = reg.Field<String>("DescIdioma"),
                           DescProveedor = reg.Field<String>("DescProveedor"),
                           DescSectorComercial = reg.Field<String>("DescSectorComercial"),
                           DEscTipoIndentificacion = reg.Field<String>("DEscTipoIndentificacion"),
                           DescTipoSolicitud = reg.Field<String>("DescTipoSolicitud"),
                           EMAILCorp = reg.Field<String>("EMAILCorp"),
                           EMAILSRI = reg.Field<String>("EMAILSRI"),
                           Estado = reg.Field<String>("Estado"),
                           FechaSolicitud = reg.Field<String>("FechaSolicitud"),
                           FechaSRI = reg.Field<String>("FechaSRI"),
                           GenDocElec = reg.Field<String>("GenDocElec"),
                           GrupoTesoreria = reg.Field<String>("GrupoTesoreria"),
                           IdEmpresa = reg["IdEmpresa"] != DBNull.Value ? reg["IdEmpresa"].ToString() : "",
                           Identificacion = reg.Field<String>("Identificacion"),
                           Idioma = reg.Field<String>("Idioma"),
                           IdSolicitud = reg["IdSolicitud"] != DBNull.Value ? reg["IdSolicitud"].ToString() : "",
                           NomComercial = reg.Field<String>("NomComercial"),
                           RazonSocial = reg.Field<String>("RazonSocial"),
                           Responsable = reg.Field<String>("Responsable"),
                           SectorComercial = reg.Field<String>("SectorComercial"),
                           TelfFax = reg.Field<String>("TelfFax"),
                           TelfFaxEXT = reg.Field<String>("TelfFaxEXT"),
                           TelfFijo = reg.Field<String>("TelfFijo"),
                           TelfFijoEXT = reg.Field<String>("TelfFijoEXT"),
                           TelfMovil = reg.Field<String>("TelfMovil"),
                           TipoIdentificacion = reg.Field<String>("TipoIdentificacion"),
                           TipoProveedor = reg.Field<String>("TipoProveedor"),
                           TipoSolicitud = reg.Field<String>("TipoSolicitud"),
                           TransfArticuProvAnterior = reg.Field<String>("TransfArticuProvAnterior"),
                           ClaseContribuyente = reg.Field<String>("ClaseContribuyente"),
                           DescClaseContribuyente = reg.Field<String>("DescClaseContribuyente"),
                           AnioConsti = reg.Field<String>("AnioConsti"),
                           DescLineaNegocio = reg.Field<String>("DescLineaNegocio"),
                           LineaNegocio = reg.Field<String>("LineaNegocio"),
                           princliente = reg.Field<String>("princliente"),
                           totalventas = reg.Field<String>("totalventas"),
                           PlazoEntrega = reg["PlazoEntrega"] != DBNull.Value ? reg["PlazoEntrega"].ToString() : "",
                           DespachaProvincia = reg.Field<String>("DespachaProvincia"),
                           DescDespachaProvincia = reg.Field<String>("DescDespachaProvincia"),
                           GrupoCuenta = reg.Field<String>("GrupoCuenta"),
                           DescGrupoCuenta = reg.Field<String>("DescGrupoCuenta"),
                           RetencionIva = reg.Field<String>("RetencionIva"),
                           DescRetencionIva = reg.Field<String>("DescRetencionIva"),
                           RetencionIva2 = reg.Field<String>("RetencionIva2"),
                           DescRetencionIva2 = reg.Field<String>("DescRetencionIva2"),
                           RetencionFuente = reg.Field<String>("RetencionFuente"),
                           DescRetencionFuente = reg.Field<String>("DescRetencionFuente"),
                           RetencionFuente2 = reg.Field<String>("RetencionFuente2"),
                           DescRetencionFuente2 = reg.Field<String>("DescRetencionFuente2"),
                           CondicionPago = reg.Field<String>("CondicionPago"),
                           DescCondicionPago = reg.Field<String>("DescCondicionPago"),
                           GrupoCompra = reg.Field<String>("GrupoCompra"),
                           GrupoEsquema = reg.Field<String>("GrupoEsquema"),
                           Ramo = reg.Field<String>("Ramo"),
                           TipoActividad = reg.Field<String>("ActividadEconomica"),
                           TipoServicio = reg.Field<String>("TipoServicio"),
                           Relacion = reg.Field<Boolean>("RelacionBanco"),
                           IdentificacionR = reg.Field<String>("RelacionIdentificacion"),
                           NomCompletosR = reg.Field<String>("RelacionNombres"),
                           AreaLaboraR = reg.Field<String>("RelacionArea"),
                           FechaCreacion = reg.Field<DateTime>("FechaCreacion"),
                           EsCritico = reg.Field<String>("EsCritico"),
                           ProcesoBrindaSoporte = reg.Field<String>("ProcesoBrindaSoporte"),
                           Sgs = reg.Field<String>("Sgs"),
                           TipoCalificacion = reg.Field<String>("TipoCalificacion"),
                           Calificacion = reg.Field<String>("Calificacion"),
                           FecTermCalificacion = reg.Field<DateTime>("FecTermCalificacion"),
                           PersonaExpuesta = reg.Field<Boolean>("PersonaExpuesta"),
                           EleccionPopular = reg.Field<Boolean>("EleccionPopular"),


                       }).ToList<DMSolcitudProveedor.SolProveedor>();
            return Retorno;
        }

        [ActionName("ConsultaSolicitud")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProveedor> GetConsultaSolicitud([FromUri] string IdConSolicitud)
        {
            List<DMSolcitudProveedor.SolProveedor> Retorno = new List<DMSolcitudProveedor.SolProveedor>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();

            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdConSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 201, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count > 1)
                    {
                        Retorno = MapSolProveedor(ds);
                    }
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("ConsultaUltSolProveedor")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProveedor> GetConsultaUltSolProveedor(string IdSolicitud, String CodSapProveedor, string Identificacion, string STipoIdentificacion, string Usuario)
        {
            IEnumerable<DMSolcitudProveedor.SolProveedor> Retorno = new List<DMSolcitudProveedor.SolProveedor>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                if (!String.IsNullOrEmpty(IdSolicitud)) xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdSolicitud);
                if (!String.IsNullOrEmpty(CodSapProveedor)) xmlParam.DocumentElement.SetAttribute("CodSapProveedor", CodSapProveedor);
                if (!String.IsNullOrEmpty(Identificacion)) xmlParam.DocumentElement.SetAttribute("Identificacion", Identificacion);
                if (!String.IsNullOrEmpty(STipoIdentificacion)) xmlParam.DocumentElement.SetAttribute("TipoIdentificacion", STipoIdentificacion);
                if (!String.IsNullOrEmpty(Usuario)) xmlParam.DocumentElement.SetAttribute("UsuarioSolicitud", Usuario);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 202, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count > 1)
                    {
                        Retorno = MapSolProveedor(ds);
                    }
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("ConsultaSolDocAdjunto")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolDocAdjunto> GetSolDocAdjunto(string IdAdSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolDocAdjunto> Retorno = new List<DMSolcitudProveedor.SolDocAdjunto>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdAdSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 203, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolDocAdjunto
                               {
                                   Archivo = reg.Field<String>("Archivo"),
                                   CodDocumento = reg.Field<String>("CodDocumento"),
                                   DescDocumento = reg.Field<String>("DescDocumento"),
                                   Estado = reg.Field<Boolean>("Estado"),
                                   FechaCarga = reg.Field<String>("FechaCarga"),
                                   IdSolDocAdjunto = reg["IdSolDocAdjunto"] != DBNull.Value ? reg["IdSolDocAdjunto"].ToString() : "",
                                   IdSolicitud = reg["IdSolicitud"] != DBNull.Value ? reg["IdSolicitud"].ToString() : "",
                                   NomArchivo = reg.Field<String>("NomArchivo"),

                               }).ToList<DMSolcitudProveedor.SolDocAdjunto>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("ConsultaLineaNegocio")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolLineaNegocio> GetSolLineaNegocio(string IdLinSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolLineaNegocio> Retorno = new List<DMSolcitudProveedor.SolLineaNegocio>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdLinSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 204, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolLineaNegocio
                               {
                                   CodigoSeccion = reg.Field<String>("CodigoSeccion"),
                                   CodigoSociedad = reg.Field<String>("CodigoSociedad"),
                                   DescSeccion = reg.Field<String>("DescSeccion"),
                                   DescSociedad = reg.Field<String>("DescSociedad"),
                                   IdLIneNegocio = reg.Field<String>("IdLIneNegocio"),
                                   IdSolicitud = reg.Field<String>("IdSolicitud"),


                               }).ToList<DMSolcitudProveedor.SolLineaNegocio>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("ConsultaSolProvBanco")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProvBanco> GetSolProvBanco(string IdBancoSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolProvBanco> Retorno = new List<DMSolcitudProveedor.SolProvBanco>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdBancoSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 205, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolProvBanco
                               {
                                   CodABA = reg.Field<String>("CodABA"),
                                   CodBENINT = reg.Field<String>("CodBENINT"),
                                   CodSapBanco = reg.Field<String>("CodSapBanco"),
                                   DescPAis = reg.Field<String>("DescPAis"),
                                   Estado = reg.Field<Boolean>("Estado"),
                                   Extrangera = reg.Field<Boolean>("Extrangera").ToString(),
                                   IdSolBanco = reg["IdSolBanco"] != DBNull.Value ? reg["IdSolBanco"].ToString() : "",
                                   IdSolicitud = reg["IdSolicitud"] != DBNull.Value ? reg["IdSolicitud"].ToString() : "",
                                   NomBanco = reg.Field<String>("NomBanco"),
                                   Pais = reg.Field<String>("Pais"),
                                   Principal = reg.Field<Boolean>("Principal"),
                                   CodSwift = reg.Field<String>("CodSwift"),
                                   DesCuenta = reg.Field<String>("DesCuenta"),
                                   NumeroCuenta = reg.Field<String>("NumeroCuenta"),
                                   ReprCuenta = reg.Field<String>("ReprCuenta"),
                                   TipoCuenta = reg.Field<String>("TipoCuenta"),
                                   TitularCuenta = reg.Field<String>("TitularCuenta"),
                                   BancoExtranjero = reg.Field<String>("BancoExtranjero"),
                                   DescProvincia = reg.Field<String>("DescProvincia"),
                                   DirBancoExtranjero = reg.Field<String>("DirBancoExtranjero"),
                                   Provincia = reg.Field<String>("Provincia"),
                                   FormaPago = reg.Field<String>("FormaPago"),
                                   DesFormaPago = reg.Field<String>("DesFormaPago"),

                               }).ToList<DMSolcitudProveedor.SolProvBanco>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("ConsultaSolProvContacto")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProvContacto> GetSolProvContacto(string IdContactoSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolProvContacto> Retorno = new List<DMSolcitudProveedor.SolProvContacto>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdContactoSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 206, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolProvContacto
                               {
                                   Apellido1 = reg.Field<String>("Apellido1"),
                                   Apellido2 = reg.Field<String>("Apellido2"),
                                   CodSapContacto = reg.Field<String>("CodSapContacto"),
                                   Departamento = reg.Field<String>("Departamento"),
                                   DepCliente = reg.Field<String>("DepCliente"),
                                   DescTipoIdentificacion = reg.Field<String>("DescTipoIdentificacion"),
                                   EMAIL = reg.Field<String>("EMAIL"),
                                   Estado = reg.Field<Boolean>("Estado"),
                                   Funcion = reg.Field<String>("Funcion"),
                                   Identificacion = reg.Field<String>("Identificacion"),
                                   IdSolContacto = reg.Field<Int64>("IdSolContacto") != null ? reg.Field<Int64>("IdSolContacto").ToString() : "",
                                   IdSolicitud = reg.Field<Int64>("IdSolicitud") != null ? reg.Field<Int64>("IdSolicitud").ToString() : "",
                                   NotElectronica = reg.Field<Boolean>("NotElectronica"),
                                   NotTransBancaria = reg.Field<Boolean>("NotTransBancaria"),
                                   Nombre1 = reg.Field<String>("Nombre1"),
                                   Nombre2 = reg.Field<String>("Nombre2"),
                                   PreFijo = reg.Field<String>("PreFijo"),
                                   RepLegal = reg.Field<Boolean>("RepLegal"),
                                   TelfFijo = reg.Field<String>("TelfFijo"),
                                   TelfFijoEXT = reg.Field<String>("TelfFijoEXT"),
                                   TelfMovil = reg.Field<String>("TelfMovil"),
                                   TipoIdentificacion = reg.Field<String>("TipoIdentificacion"),
                                   DescDepartamento = reg.Field<String>("DescDepartamento"),
                                   DescFuncion = reg.Field<String>("DescDepartamento"),
                                   EstadoCivil = reg.Field<String>("EstadoCivil"),
                                   RegimenMatrimonial = reg.Field<String>("RegimenMatrimonial"),
                                   ConyugeTipoIdentificacion = reg.Field<String>("ConyugeTipoIdentificacion"),
                                   ConyugeIdentificacion = reg.Field<String>("ConyugeIdentificacion"),
                                   ConyugeNombres = reg.Field<String>("ConyugeNombres"),
                                   ConyugeApellidos = reg.Field<String>("ConyugeApellidos"),
                                   ConyugeFechaNac = reg.Field<DateTime>("ConyugeFechaNac"),
                                   ConyugeNacionalidad = reg.Field<String>("ConyugeNacionalidad"),
                                   FechaNacimiento = reg.Field<DateTime>("FechaNacimiento"),
                                   Nacionalidad = reg.Field<String>("Nacionalidad"),
                                   Residencia = reg.Field<String>("PaisResidencia"),
                                   RelacionDependencia = reg.Field<String>("RelacionDependencia"),
                                   AntiguedadLaboral = reg.Field<String>("AntiguedadLaboral"),
                                   TipoIngreso = reg.Field<String>("TipoIngreso"),
                                   IngresoMensual = reg.Field<String>("IngresoMensual"),
                                   TipoParticipante = reg.Field<String>("TipoParticipante"),
                               }).ToList<DMSolcitudProveedor.SolProvContacto>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("ConsultaSolProvHistEstado")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProvHistEstado> GetSolProvHistEstado(string IdHisSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolProvHistEstado> Retorno = new List<DMSolcitudProveedor.SolProvHistEstado>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdHisSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 207, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolProvHistEstado
                               {
                                   DesMotivo = reg.Field<String>("DesMotivo"),
                                   EstadoSolicitud = reg.Field<String>("EstadoSolicitud"),
                                   DesEstadoSolicitud = reg.Field<String>("DesEstadoSolicitud"),
                                   Fecha = reg.Field<DateTime>("Fecha"),

                                   IdObservacion = reg["IdObservacion"] != null ? reg["IdObservacion"].ToString() : "",
                                   IdSolicitud = reg["IdSolicitud"] != null ? reg["IdSolicitud"].ToString() : "",
                                   Motivo = reg.Field<String>("Motivo"),
                                   Observacion = reg.Field<String>("Observacion"),
                                   Usuario = reg.Field<String>("Usuario"),
                               }).ToList<DMSolcitudProveedor.SolProvHistEstado>().OrderBy(X => X.Fecha).ToList<DMSolcitudProveedor.SolProvHistEstado>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception)
            {
            }

            return Retorno;
        }

        [ActionName("ConsultaSolZona")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProvZona> GetSolZona(string IdZonaSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolProvZona> Retorno = new List<DMSolcitudProveedor.SolProvZona>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdZonaSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 208, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolProvZona
                               {
                                   CodZona = reg.Field<String>("CodZona"),
                                   DescZona = reg.Field<String>("DescZona"),
                                   Estado = reg.Field<Boolean>("Estado"),
                                   IdSolicitud = reg["IdSolicitud"] != DBNull.Value ? reg["IdSolicitud"].ToString() : "",
                                   IdZona = reg["IdZona"] != DBNull.Value ? reg["IdZona"].ToString() : ""

                               }).ToList<DMSolcitudProveedor.SolProvZona>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("ConsultaSolRamo")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolRamo> GetSolRamo(string IdRamoSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolRamo> Retorno = new List<DMSolcitudProveedor.SolRamo>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdRamoSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 212, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolRamo
                               {
                                   CodRamo = reg.Field<String>("CodRamo"),
                                   DescRamo = reg.Field<String>("DescRamo"),
                                   Estado = reg.Field<Boolean>("Estado"),
                                   IdSolicitud = reg["IdSolicitud"] != DBNull.Value ? reg["IdSolicitud"].ToString() : "",
                                   IdRamo = reg["IdRamo"] != DBNull.Value ? reg["IdRamo"].ToString() : "",
                                   Principal = reg.Field<Boolean>("Principal")


                               }).ToList<DMSolcitudProveedor.SolRamo>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("ConsultaSolVia")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolViapago> GetSolVia(string IdviaSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolViapago> Retorno = new List<DMSolcitudProveedor.SolViapago>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdviaSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 214, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolViapago
                               {
                                   CodVia = reg.Field<String>("CodVia"),
                                   DescVia = reg.Field<String>("DescVia"),
                                   Estado = reg.Field<Boolean>("Estado"),
                                   IdSolicitud = reg["IdSolicitud"] != DBNull.Value ? reg["IdSolicitud"].ToString() : "",
                                   IdVia = reg["IdVia"] != DBNull.Value ? reg["IdVia"].ToString() : ""

                               }).ToList<DMSolcitudProveedor.SolViapago>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }

            return Retorno;
        }

        [ActionName("ConsultaSolProvDireccion")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProvDireccion> GetSolProvDireccion(string IdDirecSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolProvDireccion> Retorno = new List<DMSolcitudProveedor.SolProvDireccion>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {               
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdDirecSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 209, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolProvDireccion
                               {
                                   CallePrincipal = reg.Field<String>("CallePrincipal"),
                                   CalleSecundaria = reg.Field<String>("CalleSecundaria"),
                                   Ciudad = reg.Field<String>("Ciudad"),
                                   CodPostal = reg.Field<String>("CodPostal"),
                                   DescPais = reg.Field<String>("DescPais"),
                                   DescRegion = reg.Field<String>("DescRegion"),
                                   Estado = reg.Field<Boolean>("Estado"),
                                   IdDireccion = reg["IdDireccion"] != DBNull.Value ? reg["IdDireccion"].ToString() : "",
                                   IdSolicitud = reg["IdSolicitud"] != DBNull.Value ? reg["IdSolicitud"].ToString() : "",
                                   Pais = reg.Field<String>("Pais"),
                                   PisoEdificio = reg.Field<String>("PisoEdificio"),
                                   Provincia = reg.Field<String>("Provincia"),
                                   Solar = reg.Field<String>("Solar"),
                               }).ToList<DMSolcitudProveedor.SolProvDireccion>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        //Consulta linea de negocios
        [ActionName("ConsultaSolLineasdeNegocios")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolicitudLineaNegocio> GetSolLineasdeNegocios(string IdSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolicitudLineaNegocio> Retorno = new List<DMSolcitudProveedor.SolicitudLineaNegocio>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", IdSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 217, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolicitudLineaNegocio
                               {
                                   Codigo = reg.Field<String>("Codigo"),
                                   Chekeado = true,
                                   Principal = reg.Field<Boolean>("Principal"),
                               }).ToList<DMSolcitudProveedor.SolicitudLineaNegocio>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        //Consulta linea de negocios
        [ActionName("ConsultaLineasdeNegocios")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolicitudLineaNegocio> GetConsultaLineasdeNegocios(string CodProveedor)
        {
            IEnumerable<DMSolcitudProveedor.SolicitudLineaNegocio> Retorno = new List<DMSolcitudProveedor.SolicitudLineaNegocio>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 219, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolicitudLineaNegocio
                               {
                                   Codigo = reg.Field<String>("Codigo"),
                                   Chekeado = true,
                                   Principal = reg.Field<Boolean>("Principal"),
                               }).ToList<DMSolcitudProveedor.SolicitudLineaNegocio>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("GrabaSolicitud")]
        [HttpPost]
        public String GetGrabaSolicitud(DMSolcitudProveedor SolProveedor)
        {
            string _metodo = "GetGrabaSolicitud";
            string fechaCreacion = SolProveedor.p_SolProveedor[0].FechaCreacion.ToString("dd-MM-yyyy");
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud INI  ");

           
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            String idsolicitud = "";
            String estado = SolProveedor.p_SolProveedor[0].Estado;
            try
            {
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Entramos en el try 699  ");
                idsolicitud = SolProveedor.p_SolProveedor[0].IdSolicitud;

                DataTable dt = new DataTable();
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Creamos las columnas de dts");
                dt.Columns.Add("IdSolicitud", Type.GetType("System.String"));
                dt.Columns.Add("TipMedioContacto", Type.GetType("System.String"));
                dt.Columns.Add("ValorMedioContacto", Type.GetType("System.String"));
                dt.Columns.Add("Estado", Type.GetType("System.String"));
                dt.Columns.Add("Contacto", Type.GetType("System.String"));
                dt.Columns.Add("Identificacion", Type.GetType("System.String"));
                dt.Columns.Add("IdSolContacto", Type.GetType("System.String"));
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "finalizamos las columnas del ds 711");
                if (SolProveedor.p_SolProvContacto != null)
                {
                    p_Log.Graba_Log_Info(_clase + " " + _metodo + "entramos al if si solprovContacto es diferente de null 714");
                    foreach (DMSolcitudProveedor.SolProvContacto erow in SolProveedor.p_SolProvContacto)
                    {

                        DataRow drow = dt.NewRow();
                        drow["IdSolicitud"] = erow.IdSolicitud;
                        drow["IdSolContacto"] = erow.IdSolContacto;
                        drow["TipMedioContacto"] = "TLFFIJO";
                        drow["ValorMedioContacto"] = erow.TelfFijo;
                        drow["Estado"] = "True";
                        drow["Contacto"] = "S";
                        drow["Identificacion"] = erow.Identificacion;
                        dt.Rows.Add(drow);

                        drow = dt.NewRow();
                        drow["IdSolicitud"] = erow.IdSolicitud;
                        drow["IdSolContacto"] = erow.IdSolContacto;
                        drow["TipMedioContacto"] = "TLFFIJOEXT";
                        drow["ValorMedioContacto"] = erow.TelfFijoEXT;
                        drow["Estado"] = "True";
                        drow["Contacto"] = "S";
                        drow["Identificacion"] = erow.Identificacion;
                        dt.Rows.Add(drow);

                        drow = dt.NewRow();
                        drow["IdSolicitud"] = erow.IdSolicitud;
                        drow["IdSolContacto"] = erow.IdSolContacto;
                        drow["TipMedioContacto"] = "TLFMOVIL";
                        drow["ValorMedioContacto"] = erow.TelfMovil;
                        drow["Estado"] = "True";
                        drow["Contacto"] = "S";
                        drow["Identificacion"] = erow.Identificacion;
                        dt.Rows.Add(drow);

                        drow = dt.NewRow();
                        drow["IdSolicitud"] = erow.IdSolicitud;
                        drow["IdSolContacto"] = erow.IdSolContacto;
                        drow["TipMedioContacto"] = "EMAIL";
                        drow["ValorMedioContacto"] = erow.EMAIL;
                        drow["Estado"] = "True";
                        drow["Contacto"] = "S";
                        drow["Identificacion"] = erow.Identificacion;
                        dt.Rows.Add(drow);

                        dt.AcceptChanges();
                    }
                    p_Log.Graba_Log_Info(_clase + " " + _metodo + "Salimos del foreacha 760");
                }
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Agregamos datawor 762");
                DataRow drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = SolProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "TLFFIJO";
                drowSOL["ValorMedioContacto"] = SolProveedor.p_SolProveedor[0].TelfFijo;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = SolProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Agregamos datawor 770");
                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = SolProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "TLFFIJOEXT";
                drowSOL["ValorMedioContacto"] = SolProveedor.p_SolProveedor[0].TelfFijoEXT;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = SolProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Agregamos datawor 780");
                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = SolProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "TLFMOVIL";
                drowSOL["ValorMedioContacto"] = SolProveedor.p_SolProveedor[0].TelfMovil;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = SolProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Agregamos datawor 789");
                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = SolProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "FAX";
                drowSOL["ValorMedioContacto"] = SolProveedor.p_SolProveedor[0].TelfFax;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = SolProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Agregamos datawor 798");
                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = SolProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "FAXEXT";
                drowSOL["ValorMedioContacto"] = SolProveedor.p_SolProveedor[0].TelfFaxEXT;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = SolProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Agregamos datawor 807");
                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = SolProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "EMAILCORP";
                drowSOL["ValorMedioContacto"] = SolProveedor.p_SolProveedor[0].EMAILCorp;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = SolProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Agregamos datawor 816");
                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = SolProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "EMAILSRI";
                drowSOL["ValorMedioContacto"] = SolProveedor.p_SolProveedor[0].EMAILSRI;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = SolProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);

                dt.AcceptChanges();
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Agregamos datawor 827");
                String idDireccion = String.Empty;
                if (SolProveedor.p_SolProvDireccion != null && SolProveedor.p_SolProvDireccion.Count() > 0)
                {
                    idDireccion = SolProveedor.p_SolProvDireccion[0].IdDireccion;
                }
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "JSON que llego"+ JsonConvert.SerializeObject(SolProveedor));
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Empezamos armar el xml");
                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                            from p in dt.AsEnumerable()
                            select new XElement("SolMedioContacto",
                                 new XAttribute("IdSolicitud", p["IdSolicitud"] != null ? p["IdSolicitud"] : "0"),
                                 new XAttribute("IdSolContacto", p["IdSolContacto"] != null ? p["IdSolContacto"] : "0"),
                                 new XAttribute("TipMedioContacto", p["TipMedioContacto"] != null ? p["TipMedioContacto"] : ""),
                                 new XAttribute("ValorMedioContacto", p["ValorMedioContacto"] != null ? p["ValorMedioContacto"] : ""),
                                 new XAttribute("Identificacion", p["Identificacion"] != null ? p["Identificacion"] : ""),
                                 new XAttribute("Contacto", p["Contacto"] != null ? p["Contacto"] : ""),
                                 new XAttribute("Estado", p["Estado"] != null ? p["Estado"] : "0")),

                                SolProveedor.p_SolProvBanco != null ?
                                    from p in SolProveedor.p_SolProvBanco
                                    select new XElement("SolBanco",
                                         new XAttribute("IdSolicitud", p.IdSolicitud != null ? p.IdSolicitud : "0"),
                                         new XAttribute("Extrangera", p.Extrangera != null ? p.Extrangera : ""),
                                         new XAttribute("CodSapBanco", p.CodSapBanco != null ? p.CodSapBanco : ""),
                                         new XAttribute("Pais", !string.IsNullOrWhiteSpace(p.Pais) ? p.Pais : ""),
                                         new XAttribute("TipoCuenta", !string.IsNullOrWhiteSpace(p.TipoCuenta) != null ? p.TipoCuenta : ""),

                                         new XAttribute("NumeroCuenta", p.NumeroCuenta != null ? p.NumeroCuenta : ""),
                                         new XAttribute("TitularCuenta", p.TitularCuenta != null ? p.TitularCuenta : ""),
                                         new XAttribute("ReprCuenta", p.ReprCuenta != null ? p.ReprCuenta : ""),
                                         new XAttribute("CodSwift", p.CodSwift != null ? p.CodSwift : ""),

                                         new XAttribute("CodBENINT", p.CodBENINT != null ? p.CodBENINT : ""),
                                         new XAttribute("CodABA", p.CodABA != null ? p.CodABA : ""),
                                         new XAttribute("Principal", p.Principal != null ? p.Principal.ToString() : "0"),
                                         new XAttribute("Estado", p.Estado != null ? p.Estado.ToString() : "0"),
                                          new XAttribute("IdSolBanco", !string.IsNullOrWhiteSpace(p.IdSolBanco) ? p.IdSolBanco : "0"),
                                          new XAttribute("BancoExtranjero", p.BancoExtranjero != null ? p.BancoExtranjero : ""),
                                          new XAttribute("DirBancoExtranjero", p.DirBancoExtranjero != null ? p.DirBancoExtranjero : ""),
                                          new XAttribute("Provincia", !string.IsNullOrWhiteSpace(p.Provincia)? p.Provincia : ""),
                                          new XAttribute("FormaPago", !string.IsNullOrWhiteSpace(p.FormaPago) ? p.FormaPago : ""),
                                         new XAttribute("ACCION", p.IdSolBanco != null && !String.IsNullOrEmpty(p.IdSolBanco.ToString()) && int.Parse(p.IdSolBanco.ToString()) > 0 ? "U" : "I")
                                        ) : null,

                                        SolProveedor.p_SolProvContacto != null ? from p in SolProveedor.p_SolProvContacto
                                                                                 select new XElement("SolContacto",
                                                                                      new XAttribute("IdSolicitud", p.IdSolicitud != null ? p.IdSolicitud : "0"),
                                                                                      new XAttribute("TipoIdentificacion", p.TipoIdentificacion != null ? p.TipoIdentificacion : ""),
                                                                                      new XAttribute("IdSolContacto", p.IdSolContacto != null ? p.IdSolContacto : "0"),
                                                                                      new XAttribute("Identificacion", p.Identificacion != null ? p.Identificacion : ""),
                                                                                      new XAttribute("Nombre2", p.Nombre2 != null ? p.Nombre2 : ""),
                                                                                      new XAttribute("Nombre1", p.Nombre1 != null ? p.Nombre1 : ""),
                                                                                      new XAttribute("Apellido2", p.Apellido2 != null ? p.Apellido2 : ""),
                                                                                      new XAttribute("Apellido1", p.Apellido1 != null ? p.Apellido1 : ""),
                                                                                      new XAttribute("CodSapContacto", p.CodSapContacto != null ? p.CodSapContacto : ""),
                                                                                      new XAttribute("PreFijo", p.PreFijo != null ? p.PreFijo : ""),
                                                                                      new XAttribute("DepCliente", p.DepCliente != null ? p.DepCliente : ""),
                                                                                      new XAttribute("Departamento", p.Departamento != null ? p.Departamento : ""),
                                                                                      new XAttribute("Funcion", p.Funcion != null ? p.Funcion : ""),
                                                                                      new XAttribute("RepLegal", p.RepLegal != null ? p.RepLegal.ToString() : ""),
                                                                                      new XAttribute("Estado", p.Estado != null ? p.Estado.ToString() : "0"),
                                                                                      new XAttribute("NotElectronica", p.NotElectronica != null ? p.NotElectronica.ToString() : "0"),
                                                                                      new XAttribute("NotTransBancaria", p.NotTransBancaria != null ? p.NotTransBancaria.ToString() : "0"),

                                                                                      new XAttribute("TelfFijo", p.TelfFijo != null ? p.TelfFijo.ToString() : ""),
                                                                                      new XAttribute("TelfFijoEXT", p.TelfFijoEXT != null ? p.TelfFijoEXT.ToString() : ""),
                                                                                      new XAttribute("Celular", p.TelfMovil != null ? p.TelfMovil.ToString() : ""),
                                                                                      new XAttribute("EMAIL", p.EMAIL != null ? p.EMAIL.ToString() : ""),

                                                                                      new XAttribute("EstadoCivil", p.EstadoCivil != null ? p.EstadoCivil.ToString() : ""),
                                                                                      new XAttribute("ConyugeTipoIdentificacion", p.ConyugeTipoIdentificacion != null ? p.ConyugeTipoIdentificacion.ToString() : ""),
                                                                                      new XAttribute("ConyugeIdentificacion", p.ConyugeIdentificacion != null ? p.ConyugeIdentificacion.ToString() : ""),
                                                                                      new XAttribute("ConyugeNombres", p.ConyugeNombres != null ? p.ConyugeNombres.ToString() : ""),
                                                                                      new XAttribute("FechaNacimiento", p.FechaNacimiento != null ? p.FechaNacimiento.ToString("yyyy/MM/dd") : DateTime.Now.ToString("yyyy/MM/dd")),
                                                                                      new XAttribute("Nacionalidad", p.Nacionalidad != null ? p.Nacionalidad.ToString() : ""),
                                                                                      new XAttribute("Residencia", p.Residencia != null ? p.Residencia.ToString() : ""),

                                                                                      new XAttribute("ConyugeApellidos", p.ConyugeApellidos != null ? p.ConyugeApellidos.ToString() : ""),
                                                                                      new XAttribute("ConyugeFechaNac", p.ConyugeFechaNac != null ? p.ConyugeFechaNac.ToString("yyyy/MM/dd") : ""),
                                                                                      new XAttribute("ConyugeNacionalidad", p.ConyugeNacionalidad != null ? p.ConyugeNacionalidad.ToString() : ""),

                                                                                      new XAttribute("RegimenMatrimonial", p.RegimenMatrimonial!= null ? p.RegimenMatrimonial.ToString() : ""),
                                                                                      new XAttribute("RelacionDependencia", p.RelacionDependencia != null ? p.RelacionDependencia.ToString() : ""),
                                                                                      new XAttribute("AntiguedadLaboral", p.AntiguedadLaboral != null ? p.AntiguedadLaboral.ToString() : ""),
                                                                                      new XAttribute("TipoIngreso", !string.IsNullOrWhiteSpace(p.TipoIngreso) ? p.TipoIngreso.ToString() : ""),
                                                                                      new XAttribute("IngresoMensual", p.IngresoMensual != null ? p.IngresoMensual.ToString() : ""),
                                                                                      new XAttribute("TipoParticipante", !string.IsNullOrWhiteSpace(p.TipoParticipante) ? p.TipoParticipante.ToString() : ""),

                                                                                      new XAttribute("ACCION", p.IdSolContacto != null && !String.IsNullOrEmpty(p.IdSolContacto.ToString()) && int.Parse(p.IdSolContacto.ToString()) > 0 ? "U" : "I")
                                                                                      ) : null,


                                              SolProveedor.p_SolProvHistEstado != null ? from p in SolProveedor.p_SolProvHistEstado
                                                                                         select new XElement("SolProvHistEstado",
                                                                                            new XAttribute("IdSolicitud", p.IdSolicitud != null ? p.IdSolicitud : "0"),
                                                                                            new XAttribute("IdObservacion", !string.IsNullOrWhiteSpace(p.IdObservacion) ? p.IdObservacion : "0"),
                                                                                            new XAttribute("Motivo", p.Motivo != null ? p.Motivo : ""),
                                                                                            new XAttribute("Observacion", p.Observacion != null ? p.Observacion : ""),
                                                                                            new XAttribute("Usuario", p.Usuario != null ? p.Usuario : ""),
                                                                                            new XAttribute("EstadoSolicitud", p.EstadoSolicitud != null ? p.EstadoSolicitud : ""),
                                                                                            new XAttribute("ACCION", p.IdObservacion != null && !String.IsNullOrEmpty(p.IdObservacion.ToString()) && int.Parse(p.IdObservacion.ToString()) > 0 ? "U" : "I")
                                                                                            ) : null,

                    SolProveedor.p_SolProvDireccion != null ? new XElement("SolDireccion",
                                     new XAttribute("IdEmpresa", "1"),
                                     new XAttribute("IdSolicitud", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].IdSolicitud)? SolProveedor.p_SolProveedor[0].IdSolicitud : "0"),
                                     new XAttribute("IdDireccion", idDireccion != null ? idDireccion : "0"),
                                     new XAttribute("Pais", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProvDireccion[0].Pais) ? SolProveedor.p_SolProvDireccion[0].Pais : ""),
                                     new XAttribute("Provincia", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProvDireccion[0].Provincia) ? SolProveedor.p_SolProvDireccion[0].Provincia : ""),
                                     new XAttribute("Ciudad", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProvDireccion[0].Ciudad) ? SolProveedor.p_SolProvDireccion[0].Ciudad : ""),
                                     new XAttribute("CallePrincipal", SolProveedor.p_SolProvDireccion[0].CallePrincipal != null ? SolProveedor.p_SolProvDireccion[0].CallePrincipal : ""),
                                     new XAttribute("CalleSecundaria", SolProveedor.p_SolProvDireccion[0].CalleSecundaria != null ? SolProveedor.p_SolProvDireccion[0].CalleSecundaria : ""),
                                     new XAttribute("PisoEdificio", !string.IsNullOrWhiteSpace( SolProveedor.p_SolProvDireccion[0].PisoEdificio) ? SolProveedor.p_SolProvDireccion[0].PisoEdificio : ""),
                                     new XAttribute("CodPostal", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProvDireccion[0].CodPostal) ? SolProveedor.p_SolProvDireccion[0].CodPostal : ""),
                                     new XAttribute("Solar",  !string.IsNullOrWhiteSpace(SolProveedor.p_SolProvDireccion[0].Solar) ? SolProveedor.p_SolProvDireccion[0].Solar : ""),
                                     new XAttribute("Estado", "True"),// p["Estado"] != null ? p["Estado"] : ""),
                                     new XAttribute("ACCION", (!String.IsNullOrEmpty(idDireccion) && int.Parse(idDireccion.ToString()) > 0 ? "U" : "I"))) : null,

                                    SolProveedor.p_SolProvZona != null ? from p in SolProveedor.p_SolProvZona
                                                                         select new XElement("SolZona",
                                                 new XAttribute("IdSolicitud", p.IdSolicitud != null ? p.IdSolicitud : "0"),
                                                 new XAttribute("Idzona", p.IdZona != null ? p.IdZona : "0"),
                                                 new XAttribute("CodZona", p.CodZona != null ? p.CodZona : ""),

                                                 new XAttribute("Estado", p.Estado != null ? p.Estado.ToString() : "0"),


                                                 new XAttribute("ACCION", p.IdZona != null && !String.IsNullOrEmpty(p.IdZona.ToString()) && int.Parse(p.IdZona.ToString()) > 0 ? "U" : "I")
                                                 ) : null,

                                   SolProveedor.p_SolRamo != null ? from p in SolProveedor.p_SolRamo
                                                                    select new XElement("SolRamo",
                                                                                        new XAttribute("IdSolicitud", p.IdSolicitud != null ? p.IdSolicitud : "0"),
                                                                                        new XAttribute("IdRamo", p.IdRamo != null ? p.IdRamo : "0"),
                                                                                        new XAttribute("CodRAmo", p.CodRamo != null ? p.CodRamo : ""),
                                                                                        new XAttribute("Principal", p.Principal != null ? p.Principal.ToString() : ""),
                                                                                        new XAttribute("Estado", p.Estado != null ? p.Estado.ToString() : ""),
                                                                                        new XAttribute("ACCION", p.IdRamo != null && !String.IsNullOrEmpty(p.IdRamo.ToString()) && int.Parse(p.IdRamo.ToString()) > 0 ? "U" : "I")
                                                                                        ) : null,

                                   SolProveedor.p_SolViapago != null ? from p in SolProveedor.p_SolViapago
                                                                       select new XElement("SolViapago",
                                                                             new XAttribute("IdSolicitud", idsolicitud != null ? idsolicitud : "0"),
                                                                             new XAttribute("IdVia", p.IdVia != null ? p.IdVia : "0"),
                                                                             new XAttribute("CodVia", p.CodVia != null ? p.CodVia : ""),
                                                                             new XAttribute("Estado", p.Estado != null ? p.Estado.ToString() : ""),
                                                                             new XAttribute("ACCION", p.IdVia != null && !String.IsNullOrEmpty(p.IdVia.ToString()) && int.Parse(p.IdVia.ToString()) > 0 ? "U" : "I")
                                                                             ) : null,

                SolProveedor.p_SolLineasNegocios != null ?
                from p in SolProveedor.p_SolLineasNegocios
                select new XElement("SolLineasNegocios",

            new XAttribute("Codigo", p.Codigo != null ? p.Codigo : "0"),
            new XAttribute("Principal", p.Principal != null ? p.Principal : false)) : null,

                SolProveedor.p_SolDocAdjunto != null ?
                from p in SolProveedor.p_SolDocAdjunto
                select new XElement("SolDocAdjunto",

            new XAttribute("IdSolicitud", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].IdSolicitud) ? SolProveedor.p_SolProveedor[0].IdSolicitud : "0"),
            new XAttribute("IdSolDocAdjunto", !string.IsNullOrWhiteSpace(p.IdSolDocAdjunto) ? p.IdSolDocAdjunto : "0"),
            new XAttribute("CodDocumento", !string.IsNullOrWhiteSpace(p.CodDocumento) ? p.CodDocumento : ""),
            new XAttribute("Archivo", !string.IsNullOrWhiteSpace(p.Archivo) ? p.Archivo : ""),
            new XAttribute("NomArchivo", p.NomArchivo != null ? p.NomArchivo : ""),
            new XAttribute("Estado", p.Estado != null ? p.Estado.ToString() : "False"),
            new XAttribute("ACCION", (!String.IsNullOrEmpty(p.IdSolDocAdjunto) && int.Parse(p.IdSolDocAdjunto.ToString()) > 0 ? "U" : "I"))) : null,
                new XElement("SolProveedorDetalle",
                    new XAttribute("IdSolicitud", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].IdSolicitud) ? SolProveedor.p_SolProveedor[0].IdSolicitud : "0"),
                    new XAttribute("EsCritico", !string.IsNullOrWhiteSpace( SolProveedor.p_SolProveedor[0].EsCritico)? SolProveedor.p_SolProveedor[0].EsCritico : "N"),
                    new XAttribute("ProcesoBrindaSoporte", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].ProcesoBrindaSoporte) ? SolProveedor.p_SolProveedor[0].ProcesoBrindaSoporte : ""),
                    new XAttribute("Sgs", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].Sgs) ? SolProveedor.p_SolProveedor[0].Sgs : "NO"),
                    new XAttribute("TipoCalificacion", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].TipoCalificacion) ? SolProveedor.p_SolProveedor[0].TipoCalificacion : ""),
                    new XAttribute("Calificacion", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].Calificacion) ? SolProveedor.p_SolProveedor[0].Calificacion : ""),
                    new XAttribute("FecTermCalificacion", SolProveedor.p_SolProveedor[0].FecTermCalificacion.ToString() != null ? SolProveedor.p_SolProveedor[0].FecTermCalificacion.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd")),
                    new XAttribute("FechaCreacion",  !string.IsNullOrWhiteSpace(fechaCreacion) && fechaCreacion != "01-01-0001" ? SolProveedor.p_SolProveedor[0].FechaCreacion.ToString("yyyy/MM/dd") : DateTime.Now.ToString("yyyy/MM/dd")),
                    new XAttribute("CodActividadEconomica", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].TipoActividad) ? SolProveedor.p_SolProveedor[0].TipoActividad : ""),
                    new XAttribute("TipoServicio", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].TipoServicio) ? SolProveedor.p_SolProveedor[0].TipoServicio : ""),
                    new XAttribute("Relacion", SolProveedor.p_SolProveedor[0].Relacion != null ? SolProveedor.p_SolProveedor[0].Relacion.ToString() : "0"),
                    new XAttribute("IdentificacionR", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].IdentificacionR) ? SolProveedor.p_SolProveedor[0].IdentificacionR : ""),
                    new XAttribute("NomCompletosR",  SolProveedor.p_SolProveedor[0].NomCompletosR != null ? SolProveedor.p_SolProveedor[0].NomCompletosR.ToUpper() : ""),
                    new XAttribute("AreaLaboraR", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].AreaLaboraR) ? SolProveedor.p_SolProveedor[0].AreaLaboraR.ToUpper() : ""),
                    new XAttribute("PersonaExpuesta", SolProveedor.p_SolProveedor[0].PersonaExpuesta != null ? SolProveedor.p_SolProveedor[0].PersonaExpuesta.ToString() : ""),
                    new XAttribute("EleccionPopular",  SolProveedor.p_SolProveedor[0].EleccionPopular != null ? SolProveedor.p_SolProveedor[0].EleccionPopular.ToString() : ""),
                    new XAttribute("ACCION", (!String.IsNullOrEmpty(SolProveedor.p_SolProveedor[0].IdSolicitud) && int.Parse(SolProveedor.p_SolProveedor[0].IdSolicitud) > 0 ? "U" : "I"))
                    ),

                    new XElement("SolProveedor",
                                    new XAttribute("IdEmpresa", "1"),
                                     new XAttribute("TipoSolicitud", SolProveedor.p_SolProveedor[0].TipoSolicitud != null ? SolProveedor.p_SolProveedor[0].TipoSolicitud : "NU"),
                                     new XAttribute("IdSolicitud", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].IdSolicitud)  ? SolProveedor.p_SolProveedor[0].IdSolicitud : "0"),
                                     new XAttribute("TipoProveedor", !string.IsNullOrWhiteSpace( SolProveedor.p_SolProveedor[0].TipoProveedor) ? SolProveedor.p_SolProveedor[0].TipoProveedor : ""),
                                     new XAttribute("CodSapProveedor", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].CodSapProveedor) ? SolProveedor.p_SolProveedor[0].CodSapProveedor : ""),
                                     new XAttribute("TipoIdentificacion", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].TipoIdentificacion) ? SolProveedor.p_SolProveedor[0].TipoIdentificacion : ""),
                                     new XAttribute("Identificacion", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].Identificacion) ? SolProveedor.p_SolProveedor[0].Identificacion : ""),
                                     new XAttribute("NomComercial",!string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].NomComercial) ? SolProveedor.p_SolProveedor[0].NomComercial : ""),
                                     new XAttribute("RazonSocial", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].RazonSocial)? SolProveedor.p_SolProveedor[0].RazonSocial : ""),
                                     new XAttribute("FechaSRI", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].FechaSRI) && SolProveedor.p_SolProveedor[0].FechaSRI != "01/01/0001" ? SolProveedor.p_SolProveedor[0].FechaSRI : DateTime.Now.ToString("dd-MM-yyyy")),
                                     new XAttribute("SectorComercial", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].SectorComercial) ? SolProveedor.p_SolProveedor[0].SectorComercial : ""),
                                     new XAttribute("Idioma", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].Idioma)? SolProveedor.p_SolProveedor[0].Idioma : ""),
                                     new XAttribute("ClaseContribuyente", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].ClaseContribuyente) ? SolProveedor.p_SolProveedor[0].ClaseContribuyente : ""),
                                     new XAttribute("GenDocElec", SolProveedor.p_SolProveedor[0].GenDocElec),
                                     new XAttribute("Estado", SolProveedor.p_SolProveedor[0].Estado),
                                     new XAttribute("GrupoTesoreria", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].GrupoTesoreria) ? SolProveedor.p_SolProveedor[0].GrupoTesoreria : ""),
                                     new XAttribute("CuentaAsociada", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].CuentaAsociada) ? SolProveedor.p_SolProveedor[0].CuentaAsociada : ""),
                                     new XAttribute("Autorizacion", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].Autorizacion) ? SolProveedor.p_SolProveedor[0].Autorizacion : "M"),
                                     new XAttribute("TransfArticuProvAnterior", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].TransfArticuProvAnterior) ? SolProveedor.p_SolProveedor[0].TransfArticuProvAnterior : ""),
                                     new XAttribute("DepSolicitando", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].DepSolicitando) ? SolProveedor.p_SolProveedor[0].DepSolicitando : ""),
                                     new XAttribute("Responsable", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].Responsable) ? SolProveedor.p_SolProveedor[0].Responsable : ""),
                                     new XAttribute("Aprobacion", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].Aprobacion) ? SolProveedor.p_SolProveedor[0].Aprobacion : ""),
                                     new XAttribute("Comentario", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].Comentario) ? SolProveedor.p_SolProveedor[0].Comentario : ""),
                                     new XAttribute("princliente", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].princliente)? SolProveedor.p_SolProveedor[0].princliente : ""),
                                     new XAttribute("totalventas", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].totalventas) ? SolProveedor.p_SolProveedor[0].totalventas : ""),
                                     new XAttribute("AnioConsti", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].AnioConsti) ? SolProveedor.p_SolProveedor[0].AnioConsti : ""),
                                     new XAttribute("LineaNegocio", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].LineaNegocio) ? SolProveedor.p_SolProveedor[0].LineaNegocio : ""),
                                     new XAttribute("DescLineaNegocio", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].DescLineaNegocio) ? SolProveedor.p_SolProveedor[0].DescLineaNegocio : ""),
                                     new XAttribute("PlazoEntrega", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].PlazoEntrega ) ? SolProveedor.p_SolProveedor[0].PlazoEntrega : ""),
                                     new XAttribute("DespachaProvincia",!string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].DespachaProvincia) ? SolProveedor.p_SolProveedor[0].DespachaProvincia : "0"),
                                     new XAttribute("GrupoCuenta",!string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].GrupoCuenta) ? SolProveedor.p_SolProveedor[0].GrupoCuenta : ""),
                                     new XAttribute("RetencionIva", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].RetencionIva) ? SolProveedor.p_SolProveedor[0].RetencionIva : ""),
                                     new XAttribute("RetencionIva2",!string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].RetencionIva2) ? SolProveedor.p_SolProveedor[0].RetencionIva2 : ""),
                                     new XAttribute("RetencionFuente", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].RetencionFuente) ? SolProveedor.p_SolProveedor[0].RetencionFuente : ""),
                                     new XAttribute("RetencionFuente2",!string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].RetencionFuente) ? SolProveedor.p_SolProveedor[0].RetencionFuente2 : ""),
                                     new XAttribute("CondicionPago",!string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].CondicionPago) ? SolProveedor.p_SolProveedor[0].CondicionPago : ""),
                                     new XAttribute("GrupoCompra", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].GrupoCompra) ? SolProveedor.p_SolProveedor[0].GrupoCompra : ""),
                                     new XAttribute("GrupoEsquema", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].GrupoEsquema) ? SolProveedor.p_SolProveedor[0].GrupoEsquema : ""),
                                     new XAttribute("Ramo", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].Ramo) ? SolProveedor.p_SolProveedor[0].Ramo : ""),
                                     new XAttribute("CodGrupoProveedor",!string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].CodGrupoProveedor) ? SolProveedor.p_SolProveedor[0].CodGrupoProveedor : ""),                                     
                                     new XAttribute("TelfFijo", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].TelfFijo) ? SolProveedor.p_SolProveedor[0].TelfFijo : ""),
                                     new XAttribute("TelfFijoEXT",!string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].TelfFijoEXT) ? SolProveedor.p_SolProveedor[0].TelfFijoEXT : ""),
                                     new XAttribute("EMAILCorp", !string.IsNullOrWhiteSpace(SolProveedor.p_SolProveedor[0].EMAILCorp) ? SolProveedor.p_SolProveedor[0].EMAILCorp : ""),
                                    new XAttribute("ACCION", (!String.IsNullOrEmpty(SolProveedor.p_SolProveedor[0].IdSolicitud) && int.Parse(SolProveedor.p_SolProveedor[0].IdSolicitud) > 0 ? "U" : "I")))));
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "Terminamos de armar el xml");
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Error en arma el XML primero");
                //Grabar en SAP
                var resultado = "";
                //if (estado == "AP")
                //    resultado = enviasolitudbapi(SolProveedor);
                //return idsolicitud; //borrar
                if (resultado == "")
                {
                    p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Impresion del xml"+wresulFactList.ToString());
                    p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Se va insertar la informacion EjecucionGraLds");
                    ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 200, 1);
                    p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Se salio con exito de EjeucucionGralds "+JsonConvert.SerializeObject(ds));

                }
                    
                else
                {
                    return resultado;
                }

                String Wobservacion = "";
                String WMotivo = "";

                p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Vamos a validar el TblEstado");
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Entro al estado linea 1084");
                    try
                    {
                        if (SolProveedor.p_SolProveedor[0].IdSolicitud == "")
                        {
                            p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Obtener el idSolicitud linea 1088");
                            idsolicitud = ds.Tables[0].Rows[0]["IdSolicitud"].ToString();
                            
                        }
                        if (SolProveedor.p_SolProvHistEstado != null)
                        {
                            p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Obtener SolProvHistEstado 1095");
                            var x = SolProveedor.p_SolProvHistEstado.LastOrDefault();
                            if (x != null)
                            {
                                if (x.DesMotivo.ToString() == "Calificación SGS")
                                {
                                    Wobservacion = String.IsNullOrEmpty(x.Observacion) ? "" : x.Observacion.ToString().ToUpper();
                                    WMotivo = String.IsNullOrEmpty(x.DesMotivo) ? "" : x.DesMotivo.ToString().ToUpper();
                                }
                                else
                                {
                                    Wobservacion = "";
                                    WMotivo = String.IsNullOrEmpty(x.DesMotivo) ? "" : x.DesMotivo.ToString().ToUpper();
                                }
                                
                            }
                        }

                        

                        if (SolProveedor.p_SolProveedor != null && SolProveedor.p_SolProveedor[0].Estado != null)
                        {
                            p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Entramos al Envio de correo 1117");
                            CorreoNotificacion objCorreo = new CorreoNotificacion();
                            var codEstado = "";
                            //*************************************************************************************************
                            SolProveedor.p_SolProveedor[0].Estado = "PA"; //colocado para forzar la aprobacion (frank miranda)
                            SolProveedor.p_SolProveedor[0].TipoSolicitud = "APR";
                            //****************************************************************************************************
                            codEstado = SolProveedor.p_SolProveedor[0].Estado;
                            
                            switch (SolProveedor.p_SolProveedor[0].Estado)
                            {
                                case "EN":
                                case "RC":
                                case "DM":
                                case "EP":
                                    //APR
                                    objCorreo.NotificacionProveedor("APR", "PRV", SolProveedor.p_SolProveedor[0].LineaNegocio, SolProveedor.p_SolProveedor[0].NomComercial, SolProveedor.p_SolProveedor[0].Identificacion, false, null, null, WMotivo, Wobservacion, codEstado);
                                    break;
                                case "RV":
                                    objCorreo.NotificacionProveedor("APG", "PRV", null, SolProveedor.p_SolProveedor[0].NomComercial, SolProveedor.p_SolProveedor[0].Identificacion, false, null, null, WMotivo, Wobservacion, codEstado);
                                    //APG
                                    break;
                                //case "AC":
                                //    objCorreo.NotificacionProveedor("APM", "PRV", null, SolProveedor.p_SolProveedor[0].NomComercial, SolProveedor.p_SolProveedor[0].Identificacion, false, null, null, WMotivo, Wobservacion, codEstado);
                                //    //APM
                                //    break;

                                case "PR":
                                case "RE":
                                case "DP":
                                case "AC":
                                case "AP":
                                case "PA":

                                    String Estado = "";
                                    switch (SolProveedor.p_SolProveedor[0].Estado)
                                    {
                                        case "PR":
                                            Estado = "SOLICITUD DE PRECALIFICACIÓN RECHAZADA ";
                                            break;
                                        case "RE":
                                        case "RC":
                                            Estado = " RECHAZADA ";
                                            break;
                                        case "DP":
                                            Estado = " DEVUELTA ";
                                            break;                                        
                                        case "AC":
                                        case "AP":
                                            Estado = " APROBADA ";
                                            if (ds.Tables[0].Rows[0]["CodigoSap"].ToString() != null)
                                            {
                                                idsolicitud = idsolicitud + "|" + ds.Tables[0].Rows[0]["CodigoSap"].ToString();
                                            }
                                            break;                                                                                   

                                        case "PA":
                                            Estado = "SOLICITUD DE PRECALIFICIÓN APROBADA";
                                            break;
                                    }

                                    objCorreo.NotificacionProveedor("APM", "PRV", null, SolProveedor.p_SolProveedor[0].NomComercial, SolProveedor.p_SolProveedor[0].Identificacion, true, SolProveedor.p_SolProveedor[0].EMAILCorp, Estado, WMotivo, Wobservacion, codEstado);

                                    //PROveedor
                                    break;
                                    //    default;
                            }
                            p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Salimos del switch 1180");
                        }
                    }
                    catch (Exception e)
                    {
                        p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud camimos en la excepcion 1186" + e.Message);
                        idsolicitud =  "Linea 1190" +  e.Message;
                    }
                }
                else
                {
                    p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud Mensaje de error es diferente a cerro 1186" + ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                }
                    
            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Info(_clase + " " + _metodo + "GrabaSolicitud caimos en la excepcion 1198" + ex.Message);
                idsolicitud = ex.Message;
            }

            return idsolicitud;
        }


        /// <summary>
        /// Retorna el path de la ruta temporal donde esta el archivo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [ActionName("SolicitudBajaArchivo")]
        [HttpGet]
        public HttpResponseMessage BajaTempArchivo(string solpath, string solarchivo)
        {
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " -- BajaTempArchivo " + " INI ");

            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + solpath), solarchivo);
            p_Log.Graba_Log_Info("BajaTempArchivo " + " fileSavePath: " + fileSavePath);

            HttpResponseMessage result = null;
            //System.IO.FileInfo toDownload = new System.IO.FileInfo(fileSavePath);
            byte[] bytes = null;
            if (File.Exists(fileSavePath))
            {
                p_Log.Graba_Log_Info("BajaTempArchivo " + " ExistsfileSavePath: " + fileSavePath);

                bytes = File.ReadAllBytes(fileSavePath);

                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(bytes));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = solarchivo;
            }
            else
            {
                p_Log.Graba_Log_Info("BajaTempArchivo " + " NotExistsfileSavePath: " + fileSavePath);
            }

            return result;
        }

        private String bapicreacion(DMSolcitudProveedor solicitudPar)
        {
            var msj = "";
            try
            {
                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;
                IRfcFunction fndatosmaestro;
                CatalogosController catalag = new CatalogosController();
                fndatosmaestro = repo.CreateFunction("ZPPPROVEEDORCREA");

                var DTPTPROV = fndatosmaestro.GetTable("PT_PROV");
                var DTPROVBANK = fndatosmaestro.GetTable("PT_PROVBANK");
                var DTPROVCONTACT = fndatosmaestro.GetTable("PT_PROVCONTACT");
                var DTRETENCION = fndatosmaestro.GetTable("PT_RETENCION");
                IRfcStructure ITPTPROV;
                IRfcStructure ITPROVBANK;
                IRfcStructure ITPROVCONTACT;
                IRfcStructure ITRETENCION;
                //////////       PT_PROV
                ITPTPROV = repo.GetStructureMetadata("ZWAPPPROVEEDORES").CreateStructure();
                //               PT_PROV	ZWAPPPROVEEDORES		Estructura con Datos de Proveedores	Creación 	
                //LIFNR	CHAR	10	ID de Proveedor	
                ITPTPROV.SetValue("LIFNR", solicitudPar.p_SolProveedor[0].CodSapProveedor);
                //BUKRS	CHAR	4	Sociedad	X
                ITPTPROV.SetValue("BUKRS", "1001");
                //STCD1	CHAR	16	RUC	X              
                ITPTPROV.SetValue("STCD1", solicitudPar.p_SolProveedor[0].Identificacion);
                //KTOKK	CHAR	4	Grupo de cuentas acreedor (Tipo de Proveedor )	X
                ITPTPROV.SetValue("KTOKK", !String.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].GrupoCuenta) ? solicitudPar.p_SolProveedor[0].GrupoCuenta : "NACN");
                //NAME1	CHAR	35	Nombre 1 (Razón Social)	X
                if (solicitudPar.p_SolProveedor[0].RazonSocial.Length > 35)
                {
                    if (solicitudPar.p_SolProveedor[0].RazonSocial.Length > 70)
                    {
                        if (solicitudPar.p_SolProveedor[0].RazonSocial.Length > 105)
                        {
                            ITPTPROV.SetValue("NAME1", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(0, 35));
                            ITPTPROV.SetValue("NAME2", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(35, 35));
                            ITPTPROV.SetValue("NAME3", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(70, 35));
                            ITPTPROV.SetValue("NAME4", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(105));
                        }
                        else
                        {
                            ITPTPROV.SetValue("NAME1", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(0, 35));
                            ITPTPROV.SetValue("NAME2", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(35, 35));
                            ITPTPROV.SetValue("NAME3", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(70));
                        }
                    }
                    else
                    {
                        ITPTPROV.SetValue("NAME1", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(0, 35));
                        ITPTPROV.SetValue("NAME2", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(35));
                    }
                }
                else
                {
                    ITPTPROV.SetValue("NAME1", solicitudPar.p_SolProveedor[0].RazonSocial);
                }

                //NAME2	CHAR	35	Nombre 2 (Razón Social)	
                //NAME3	CHAR	35	Nombre 3 (Razón Social)	
                //NAME4	CHAR	35	Nombre 4 (Razón Social)	
                //SORT1	CHAR	20	Concepto de búsqueda 1 (Nombre comercial)	
                ITPTPROV.SetValue("SORT1", solicitudPar.p_SolProveedor[0].NomComercial);
                //STRAS	CHAR	30	Calle y número	X
                ITPTPROV.SetValue("STRAS", solicitudPar.p_SolProvDireccion[0].CallePrincipal + (!string.IsNullOrEmpty(solicitudPar.p_SolProvDireccion[0].Solar) ? "|" + solicitudPar.p_SolProvDireccion[0].Solar : ""));
                //FLOOR	CHAR	10	Planta del edificio	
                ITPTPROV.SetValue("FLOOR", solicitudPar.p_SolProvDireccion[0].PisoEdificio);
                //STR_SUPPL3	CHAR	40	Calle 4	
                ITPTPROV.SetValue("STR_SUPPL3", solicitudPar.p_SolProvDireccion[0].CalleSecundaria);
                //ORT02	CHAR	35	Distrito	
                //PSTLZ	CHAR	10	Código postal	
                ITPTPROV.SetValue("PSTLZ", solicitudPar.p_SolProvDireccion[0].CodPostal);
                //ORT01	CHAR	35	Población	X
                var codCiudad = !String.IsNullOrEmpty(solicitudPar.p_SolProvDireccion[0].Ciudad) ? solicitudPar.p_SolProvDireccion[0].Ciudad.Split('-')[0] : "NA";
                var listaCiudades = catalag.GetCatalogos("tbl_Ciudad");
                var descCiudad = listaCiudades.Where(x => x.Codigo == codCiudad).FirstOrDefault();
                ITPTPROV.SetValue("ORT01", descCiudad == null ? "" : descCiudad.Detalle);
                //LAND1	CHAR	3	Clave de país	X
                ITPTPROV.SetValue("LAND1", solicitudPar.p_SolProvDireccion[0].Pais);
                //REGIO	CHAR	3	Región (Estado federal, "land", provincia, condado)	X
                ITPTPROV.SetValue("REGIO", !String.IsNullOrEmpty(solicitudPar.p_SolProvDireccion[0].Provincia) ? solicitudPar.p_SolProvDireccion[0].Provincia.Split('-')[1] : "RE");
                //SPRAS	LANG	1	Clave de idioma	X
                var codIdioma = solicitudPar.p_SolProveedor[0].Idioma;
                var listaIdiomas = catalag.GetCatalogos("tbl_Idioma");
                var descAlIdioma = listaIdiomas.Where(x => x.Codigo == codIdioma).FirstOrDefault();

                ITPTPROV.SetValue("SPRAS", descAlIdioma != null ? descAlIdioma.DescAlterno : "S");
                //TELF1	CHAR	16	1º número de teléfono	X
                ITPTPROV.SetValue("TELF1", solicitudPar.p_SolProveedor[0].TelfFijo + (!string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].TelfFijoEXT) ? " " + solicitudPar.p_SolProveedor[0].TelfFijoEXT : ""));
                //TELF2	CHAR	16	Nº de teléfono 2	
                ITPTPROV.SetValue("TELF2", solicitudPar.p_SolProveedor[0].TelfMovil);
                //TELFX	CHAR	31	Nº telefax	
                var fax = solicitudPar.p_SolProveedor[0].TelfFax + (!string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].TelfFaxEXT) ? " " + solicitudPar.p_SolProveedor[0].TelfFaxEXT : "");
                if (fax != "")
                    ITPTPROV.SetValue("TELFX", fax);
                else
                    ITPTPROV.SetValue("TELFX", solicitudPar.p_SolProveedor[0].TelfFijo + (!string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].TelfFijoEXT) ? " " + solicitudPar.p_SolProveedor[0].TelfFijoEXT : ""));
                //SMTP_ADDR	CHAR	241	Dirección de correo electrónico	
                ITPTPROV.SetValue("SMTP_ADDR", solicitudPar.p_SolProveedor[0].EMAILCorp);
                //ZZDOCELEC	CHAR	1	Genera Documentos Electrónicos	
                ITPTPROV.SetValue("ZZDOCELEC", solicitudPar.p_SolProveedor[0].GenDocElec == "True" ? "X" : "");
                //CERDT	DATS	8	Fecha de certificación	X
                ITPTPROV.SetValue("CERDT", DateTime.Now.ToString("yyyyMMdd"));
                //MINDK	CHAR	3	Indicador de minorías	
                ITPTPROV.SetValue("MINDK", solicitudPar.p_SolProveedor[0].SectorComercial != null ? solicitudPar.p_SolProveedor[0].SectorComercial : "");

                var apoderado = solicitudPar.p_SolProvContacto.Where(x => x.RepLegal).FirstOrDefault();
                //ZZIOPLN	CHAR	100	Nombres del apoderado
                //ZZIOPLA	CHAR	100	Apellidos del apoderado	
                //ZZSTCDAP	CHAR	16	Número de identificación fiscal Apoderado	
                if (apoderado != null)
                {
                    ITPTPROV.SetValue("ZZIOPLN", apoderado.Nombre1 + " " + apoderado.Nombre2);
                    ITPTPROV.SetValue("ZZIOPLA", apoderado.Apellido1 + " " + apoderado.Apellido2);
                    ITPTPROV.SetValue("ZZSTCDAP", apoderado.Identificacion);
                }
                else
                {
                    ITPTPROV.SetValue("ZZIOPLN", "");
                    ITPTPROV.SetValue("ZZIOPLA", "");
                    ITPTPROV.SetValue("ZZSTCDAP", "NA");
                }
                //PLIFZ	DEC	3	Plazo de entrega previsto en días	
                ITPTPROV.SetValue("PLIFZ", !String.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].PlazoEntrega) ? solicitudPar.p_SolProveedor[0].PlazoEntrega : "0");
                //FECHAMOD	DATS	8	Fecha de última modificación	
                ITPTPROV.SetValue("CERDT", DateTime.Now.ToString("yyyyMMdd"));
                //STCDT	CHAR	2	Tipo de número de identificación fiscal	X
                ITPTPROV.SetValue("STCDT", solicitudPar.p_SolProveedor[0].TipoIdentificacion);
                //ANRED	CHAR	15	Tratamiento	
                ITPTPROV.SetValue("ANRED", "");
                //AKONT	CHAR	10	Cuenta asociada en la contabilidad principal	X
                ITPTPROV.SetValue("AKONT", String.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].CuentaAsociada.ToString().Trim().Split('-')[0].ToString()) ? "NA" : solicitudPar.p_SolProveedor[0].CuentaAsociada.ToString().Trim().Split('-')[1].ToString());
                //ZZMAILAP	CHAR	100	Mail del Apoderado	
                ITPTPROV.SetValue("ZZMAILAP", "");
                //EKORG	CHAR	4	Organización de compras	X
                ITPTPROV.SetValue("EKORG", "1001");
                //BEGRU	CHAR	4	Grupo autorizaciones	X
                ITPTPROV.SetValue("BEGRU", solicitudPar.p_SolProveedor[0].Autorizacion);
                //FITYP	CHAR	2	Clase de impuesto	X
                ITPTPROV.SetValue("FITYP", solicitudPar.p_SolProveedor[0].ClaseContribuyente);
                //FDGRV	CHAR	10	Grupo de tesorería	X
                ITPTPROV.SetValue("FDGRV", solicitudPar.p_SolProveedor[0].GrupoTesoreria == null ? "" : solicitudPar.p_SolProveedor[0].GrupoTesoreria.Split('-')[1]);
                //ZTERM	CHAR	4	Clave de condiciones de pago	X

                int carac = solicitudPar.p_SolProveedor[0].CondicionPago.IndexOf('-');

                var condPago = "";
                if (carac > 0)
                {
                    var condPagoAux = solicitudPar.p_SolProveedor[0].CondicionPago.ToString();
                    condPago = String.IsNullOrEmpty(condPagoAux) ? "" : condPagoAux.Split('-').Length > 0 ?
                                                                        condPagoAux.Substring(condPagoAux.Split('-')[0].Length + 1) :
                                                                        condPagoAux;
                }

                ITPTPROV.SetValue("ZTERM", condPago);
                //ZWELS	CHAR	10	Lista de las vías de pago a tener en cuenta	X
                var lisViaPago = "";

                if (solicitudPar.p_SolViapago != null && solicitudPar.p_SolViapago.Count() > 0)
                {
                    foreach (var a in solicitudPar.p_SolViapago)
                    {
                        lisViaPago = lisViaPago + a.CodVia;
                    }
                }
                ITPTPROV.SetValue("ZWELS", lisViaPago);

                //BRSCH	CHAR	4	Clave de ramo industrial	
                ITPTPROV.SetValue("BRSCH", solicitudPar.p_SolProveedor[0].Ramo == null ? "" : solicitudPar.p_SolProveedor[0].Ramo);

                //KALSK	CHAR	2	Grupo para esquema de cálculo (Proveedor)	
                ITPTPROV.SetValue("KALSK", solicitudPar.p_SolProveedor[0].GrupoEsquema);
                //WAERS	CUKY	5	Clave de moneda	
                ITPTPROV.SetValue("WAERS", "USD");
                //EKGRP	CHAR	3	Grupo de compras
                ITPTPROV.SetValue("EKGRP", solicitudPar.p_SolProveedor[0].GrupoCompra);

                DTPTPROV.Append(ITPTPROV);

                if (solicitudPar.p_SolProvContacto != null)
                {
                    foreach (var contacto in solicitudPar.p_SolProvContacto)
                    {
                        ITPROVCONTACT = repo.GetStructureMetadata("ZWAPPPROVCONTACT").CreateStructure();

                        // LIFNR	CHAR	10	ID de Proveedor		X
                        ITPROVCONTACT.SetValue("LIFNR", solicitudPar.p_SolProveedor[0].CodSapProveedor);
                        //PARNR	NUMC	10	Número de la persona de contacto		X
                        ITPROVCONTACT.SetValue("PARNR", contacto.CodSapContacto);
                        //ANRED	CHAR	30	Tratamiento de la persona de contacto		X
                        ITPROVCONTACT.SetValue("ANRED", contacto.DepCliente);
                        //NAMEV	CHAR	35	Nombre de pila		X
                        ITPROVCONTACT.SetValue("NAMEV", contacto.Nombre1 + " " + contacto.Nombre2);
                        //NAME1	CHAR	35	Nombre 1	X	X
                        ITPROVCONTACT.SetValue("NAME1", contacto.Apellido1 + " " + contacto.Nombre2);
                        //ABTPA	CHAR	12	Departamento de persona contacto en cliente		X
                        ITPROVCONTACT.SetValue("ABTPA", contacto.DescDepartamento);
                        //ABTNR	CHAR	4	Departamento de persona contacto		X
                        ITPROVCONTACT.SetValue("ABTNR", contacto.Departamento);
                        //PAFKT	CHAR	2	Función de la persona de contacto		X
                        ITPROVCONTACT.SetValue("PAFKT", contacto.Funcion);
                        //TELF1	CHAR	16	1º número de teléfono		X
                        ITPROVCONTACT.SetValue("TELF1", contacto.TelfMovil);
                        //TEL_NUMBER	CHAR	30	Número teléfono: Prefijo+número		X
                        //ITPROVCONTACT.SetValue("TEL_NUMBER", contacto.TelfFijo + " " + contacto.TelfFijoEXT);
                        ITPROVCONTACT.SetValue("TEL_NUMBER", contacto.TelfFijo);
                        //SMTP_ADDR	CHAR	241	Dirección de correo electrónico	X	X
                        ITPROVCONTACT.SetValue("SMTP_ADDR", contacto.EMAIL);
                        //J_1ATODC	CHAR	2	Tipo de número de identificación fiscal		
                        ITPROVCONTACT.SetValue("J_1ATODC", contacto.TipoIdentificacion);
                        //IDNUM	CHAR	30	Identity Number		
                        ITPROVCONTACT.SetValue("IDNUM", contacto.Identificacion);
                        if (contacto.RepLegal == false)
                        {
                            ITPROVCONTACT.SetValue("PARH1", "");
                        }
                        else
                        {
                            ITPROVCONTACT.SetValue("PARH1", "X");
                        }
                        if (contacto.NotElectronica == false)
                        {
                            ITPROVCONTACT.SetValue("PARH2", "");
                        }
                        else
                        {
                            ITPROVCONTACT.SetValue("PARH2", "X");
                        }
                        if (contacto.NotTransBancaria == false)
                        {
                            ITPROVCONTACT.SetValue("PARH3", "");
                        }
                        else
                        {
                            ITPROVCONTACT.SetValue("PARH3", "X");
                        }

                        ITPROVCONTACT.SetValue("PARH4", "A");

                        //ACTION	CHAR	1	Acción	
                        ITPROVCONTACT.SetValue("ACTION", !contacto.Estado ? "X" : "");
                        DTPROVCONTACT.Append(ITPROVCONTACT);
                    }
                }

                if (solicitudPar.p_SolProvBanco != null && solicitudPar.p_SolProvBanco.Count() > 0)
                {
                    foreach (var banco in solicitudPar.p_SolProvBanco)
                    {
                        ITPROVBANK = repo.GetStructureMetadata("ZWAPPPROVBANK").CreateStructure();

                        //LIFNR	CHAR	10	ID de Proveedor		X
                        ITPROVBANK.SetValue("LIFNR", solicitudPar.p_SolProveedor[0].CodSapProveedor);
                        //BANKS	CHAR	3	Clave de país del banco		X
                        ITPROVBANK.SetValue("BANKS", banco.Pais);
                        //BANKL	CHAR	15	Código bancario		X
                        ITPROVBANK.SetValue("BANKL", banco.CodSapBanco);
                        //BANKN	CHAR	18	Nº cuenta bancaria		X
                        ITPROVBANK.SetValue("BANKN", banco.NumeroCuenta);
                        //KOINH	CHAR	60	Titular de la cuenta		X
                        ITPROVBANK.SetValue("KOINH", banco.TitularCuenta);
                        //BKONT	CHAR	2	Clave de control de bancos (Tipo de cuenta)		X
                        ITPROVBANK.SetValue("BKONT", banco.TipoCuenta);
                        //BVTYP	CHAR	4	Tipo de banco interlocutor (Cód. BEN/INT)		X
                        ITPROVBANK.SetValue("BVTYP", banco.CodBENINT);
                        //SWIFT	CHAR	11	Código SWIFT para pagos internacionales		X
                        ITPROVBANK.SetValue("SWIFT", banco.CodSwift);
                        //PROVZ	CHAR	3	Región (Estado federal, "land", provincia, condado)		X
                        ITPROVBANK.SetValue("PROVZ", !String.IsNullOrEmpty(banco.Provincia) ? banco.Provincia.Split('-')[1] : "RE");
                        //BANKA	CHAR	60	Nombre de la institución financiera		X
                        ITPROVBANK.SetValue("BANKA", banco.BancoExtranjero);
                        //STRAS	CHAR	35	Calle y nº		X
                        ITPROVBANK.SetValue("STRAS", banco.DirBancoExtranjero);
                        //ACTION	CHAR	1	Acción		
                        ITPROVBANK.SetValue("ACTION", !banco.Estado ? "X" : "");
                        DTPROVBANK.Append(ITPROVBANK);
                    }
                }

                //ITRETENCION
                if (!string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionFuente) && !string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionFuente2))
                {
                    ITRETENCION = repo.GetStructureMetadata("ZWAPPLFBW").CreateStructure();

                    ITRETENCION.SetValue("LIFNR", solicitudPar.p_SolProveedor[0].CodSapProveedor);
                    ITRETENCION.SetValue("BUKRS", "1001");
                    ITRETENCION.SetValue("WT_SUBJCT", "X");
                    ITRETENCION.SetValue("WITHT", solicitudPar.p_SolProveedor[0].RetencionFuente);
                    ITRETENCION.SetValue("WT_WITHCD", !String.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionFuente2) ? solicitudPar.p_SolProveedor[0].RetencionFuente2.Split('-')[2] : "");
                    DTRETENCION.Append(ITRETENCION);
                }

                if (!string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionIva) && !string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionIva2))
                {
                    ITRETENCION = repo.GetStructureMetadata("ZWAPPLFBW").CreateStructure();

                    ITRETENCION.SetValue("LIFNR", solicitudPar.p_SolProveedor[0].CodSapProveedor);
                    ITRETENCION.SetValue("BUKRS", "1001");
                    ITRETENCION.SetValue("WITHT", solicitudPar.p_SolProveedor[0].RetencionIva);
                    ITRETENCION.SetValue("WT_SUBJCT", "X");
                    ITRETENCION.SetValue("WT_WITHCD", !String.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionIva2) ? solicitudPar.p_SolProveedor[0].RetencionIva2.Split('-')[2] : "");
                    DTRETENCION.Append(ITRETENCION);
                }

                fndatosmaestro.SetValue("PT_PROV", DTPTPROV);
                fndatosmaestro.SetValue("PT_PROVCONTACT", DTPROVCONTACT);
                fndatosmaestro.SetValue("PT_PROVBANK", DTPROVBANK);
                fndatosmaestro.SetValue("PT_RETENCION", DTRETENCION);

                //PT_GRUPAUT catalogo para BEGRU
                fndatosmaestro.Invoke(AppConfig.dest);

                var log = fndatosmaestro.GetTable("PT_LOG");
                var CODERROR = fndatosmaestro.GetString("CODERROR");
                var DESERROR = fndatosmaestro.GetString("DESERROR");
                var tablaLog = fndatosmaestro.GetTable("PT_LOG");
                if (CODERROR == "0")
                {
                    var tablaProv = fndatosmaestro.GetTable("PT_PROV");
                    var result = (from a in tablaProv
                                  select new
                                  {
                                      CODPROV = a.GetString("LIFNR"),
                                  }).ToList();

                    var codProvGenerado = "";
                    foreach (var m in result)
                    {
                        codProvGenerado = codProvGenerado + m.CODPROV;
                    }

                    msj = crearProveedor(solicitudPar, codProvGenerado);
                }

                else
                {
                    var result = (from a in tablaLog
                                  select new
                                  {
                                      MESSAGE = a.GetString("MESSAGE"),
                                      NUMBER = a.GetString("NUMBER"),
                                      ID = a.GetString("ID"),
                                  }).ToList();

                    msj = "";
                    foreach (var m in result)
                    {
                        msj = msj + m.NUMBER + "-" + m.MESSAGE + "\n";
                    }

                    msj = "Error en SAP: " + msj;
                }

                var PT_PROVCONTACT = fndatosmaestro.GetTable("PT_PROVCONTACT");
                var PT_PROVBANK = fndatosmaestro.GetTable("PT_PROVBANK");
                var PT_RETENCION = fndatosmaestro.GetTable("PT_RETENCION");
            }
            catch (Exception ex)
            {
                msj = "Error en SAP: " + ex.Message.ToString();
            }
            return msj;
        }

        //Crear proveedor en tablas maestras de proveedores

        public string ValidaCodigoRecibido(string codigo)
        {
            int iTmp;
            string resp = (String.IsNullOrEmpty(codigo) ?
                "" :
                (!int.TryParse(codigo, out iTmp) ?
                    codigo : int.Parse(codigo).ToString()));
            return resp;
        }

        public static string CodigoSAPConCeros(string codigo)
        {
            string resp = codigo.Trim().PadLeft(10, '0');
            return resp;
        }

        public static bool CodigoLegacyFormatEsDistinto(string codigo)
        {
            bool resp = false;
            string cod2 = CodigoLegacyFormatVal(codigo);
            if (codigo != cod2) resp = true;
            return resp;
        }

        public static string CodigoLegacyFormatVal(string codigo)
        {
            string resp = codigo;
            if (!String.IsNullOrEmpty(codigo))
            {
                if (codigo.Length > 4)
                {
                    int iTmp;
                    string subcod = codigo.Substring(0, codigo.Length - 1);
                    if (int.TryParse(subcod, out iTmp))
                    {
                        resp = subcod.Substring(subcod.Length - 4, 4);
                    }
                }
            }

            return resp;
        }

        private String crearProveedor(DMSolcitudProveedor solicitudPar, string codigoProveedor)
        {
            String wresult = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                String IProveedor = codigoProveedor;
                DateTime? Ifecha = null;

                int Total = 0;
                int skipe = 0;
                var p_stake = 10;

                // CONEXIONA A SAP ////////////////////////////////////////////////////////////
                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;
                // CONEXIONA A RFC PROVEEDORES ////////////////////////////////////////////////
                IRfcFunction testfn = repo.CreateFunction("ZPPPROVEEDORCHECK");

                IRfcStructure zstEdi = repo.GetStructureMetadata("ZWAPPPROVLISTA").CreateStructure();

                if (!String.IsNullOrEmpty(IProveedor))
                {
                    IProveedor = ValidaCodigoRecibido(IProveedor);
                    IRfcTable zisTtEdi = testfn.GetTable("P_PRLIST");
                    zisTtEdi.Append(zstEdi);
                    zisTtEdi.SetValue("LIFNR", IProveedor);
                    testfn.Invoke(AppConfig.dest);
                    if (testfn.GetTable("PT_PROV").RowCount < 1)
                    {
                        zstEdi = repo.GetStructureMetadata("ZWAPPPROVLISTA").CreateStructure();
                        testfn.GetTable("P_PRLIST").Clear();
                        zisTtEdi = testfn.GetTable("P_PRLIST");
                        zisTtEdi.Append(zstEdi);
                        zisTtEdi.SetValue("LIFNR", CodigoSAPConCeros(IProveedor));
                        testfn.Invoke(AppConfig.dest);
                    }
                }
                else
                {
                    if (Ifecha.HasValue)
                    {
                        testfn.SetValue("P_FECUA", Ifecha.Value.ToString("yyyyMMdd"));
                    }
                    testfn.Invoke(AppConfig.dest);
                }

                //TABLA 1
                var provList = testfn.GetTable("PT_PROV");
                //TABLA 2
                var bankList = testfn.GetTable("PT_PROVBANK");
                //TABLA 3
                var contList = testfn.GetTable("PT_PROVCONTACT");
                //TABLA 4
                var LegList = testfn.GetTable("PT_PROVLEGACY");
                int totAct = LegList.RowCount;
                for (int idx = 0; idx < totAct; idx++)
                {
                    var LegVal = LegList[idx];
                    if (CodigoLegacyFormatEsDistinto((string)LegVal.GetValue("KOLIF")))
                    {
                        IRfcStructure zstLeg = repo.GetStructureMetadata("ZWAPPPROVLEG").CreateStructure();
                        LegList.Append(zstLeg);
                        LegList.SetValue("LIFNR", (string)LegVal.GetValue("LIFNR"));
                        LegList.SetValue("KOLIF", CodigoLegacyFormatVal((string)LegVal.GetValue("KOLIF")));
                    }
                }

                Total = (from re in provList.AsEnumerable()
                         select re).Count();
                if (Total > 0)
                {
                    if (Total < p_stake)
                    {
                        p_stake = Total;
                    }
                    var wregistrocont = (from reg in contList.AsParallel()
                                         select new
                                         {
                                             LIFNR = ValidaCodigoRecibido(reg.GetString("LIFNR")),
                                             PARNR = ValidaCodigoRecibido(reg.GetString("PARNR")),
                                             ANRED = (String)(String.IsNullOrEmpty(reg.GetString("ANRED")) ? "" : reg.GetString("ANRED")),
                                             NAMEV = (String)(String.IsNullOrEmpty(reg.GetString("NAMEV")) ? "" : reg.GetString("NAMEV")),
                                             NAME1 = (String)(String.IsNullOrEmpty(reg.GetString("NAME1")) ? "" : reg.GetString("NAME1")),
                                             ABTPA = (String)(String.IsNullOrEmpty(reg.GetString("ABTPA")) ? "" : reg.GetString("ABTPA")),
                                             ABTNR = (String)(String.IsNullOrEmpty(reg.GetString("ABTNR")) ? "" : reg.GetString("ABTNR")),
                                             PAFKT = (String)(String.IsNullOrEmpty(reg.GetString("PAFKT")) ? "" : reg.GetString("PAFKT")),
                                             TELF1 = (String)(String.IsNullOrEmpty(reg.GetString("TELF1")) ? "" : reg.GetString("TELF1")),
                                             TEL_NUMBER = (String)(String.IsNullOrEmpty(reg.GetString("TEL_NUMBER")) ? "" : reg.GetString("TEL_NUMBER")),
                                             SMTP_ADDR = (String)(String.IsNullOrEmpty(reg.GetString("SMTP_ADDR")) ? "" : reg.GetString("SMTP_ADDR"))
                                         }).ToList();

                    var wregistrolegacy = (from reg in LegList.AsParallel()
                                           select new
                                           {
                                               LIFNR = ValidaCodigoRecibido(reg.GetString("LIFNR")),
                                               KOLIF = ValidaCodigoRecibido(reg.GetString("KOLIF"))
                                           }).ToList();

                    var wregistrobank = (from reg in bankList.AsParallel()
                                         select new
                                         {
                                             LIFNR = ValidaCodigoRecibido(reg.GetString("LIFNR")),
                                             BANKL = String.IsNullOrEmpty(reg.GetString("BANKL")) ? "" : reg.GetString("BANKL"),
                                             BANKN = String.IsNullOrEmpty(reg.GetString("BANKN")) ? "" : reg.GetString("BANKN")
                                         }).ToList();

                    var wregistroprov = (from reg in provList.AsParallel()
                                         select new
                                         {
                                             LIFNR = ValidaCodigoRecibido(reg.GetString("LIFNR")),
                                             STCD1 = String.IsNullOrEmpty(reg.GetString("STCD1")) ? "" : reg.GetString("STCD1"),
                                             KTOKK = String.IsNullOrEmpty(reg.GetString("KTOKK")) ? "" : reg.GetString("KTOKK"),
                                             NAME1 = String.IsNullOrEmpty(reg.GetString("NAME1")) ? "" : reg.GetString("NAME1"),
                                             STRAS = String.IsNullOrEmpty(reg.GetString("STRAS")) ? "" : reg.GetString("STRAS"),
                                             FLOOR = String.IsNullOrEmpty(reg.GetString("FLOOR")) ? "" : reg.GetString("FLOOR"),
                                             STR_SUPPL3 = String.IsNullOrEmpty(reg.GetString("STR_SUPPL3")) ? "" : reg.GetString("STR_SUPPL3"),
                                             ORT02 = String.IsNullOrEmpty(reg.GetString("ORT02")) ? "" : reg.GetString("ORT02"),
                                             PSTLZ = String.IsNullOrEmpty(reg.GetString("PSTLZ")) ? "0" : reg.GetString("PSTLZ"),
                                             ORT01 = String.IsNullOrEmpty(reg.GetString("ORT01")) ? "" : reg.GetString("ORT01"),
                                             LAND1 = String.IsNullOrEmpty(reg.GetString("LAND1")) ? "" : reg.GetString("LAND1"),
                                             REGIO = String.IsNullOrEmpty(reg.GetString("REGIO")) ? "" : reg.GetString("REGIO"),
                                             SPRAS = String.IsNullOrEmpty(reg.GetString("SPRAS")) ? "" : reg.GetString("SPRAS"),
                                             TELF1 = String.IsNullOrEmpty(reg.GetString("TELF1")) ? "" : reg.GetString("TELF1"),
                                             TELF2 = String.IsNullOrEmpty(reg.GetString("TELF2")) ? "" : reg.GetString("TELF2"),
                                             TELFX = String.IsNullOrEmpty(reg.GetString("TELFX")) ? "" : reg.GetString("TELFX"),
                                             SMTP_ADDR = String.IsNullOrEmpty(reg.GetString("SMTP_ADDR")) ? "" : reg.GetString("SMTP_ADDR"),
                                             ZZDOCELEC = String.IsNullOrEmpty(reg.GetString("ZZDOCELEC")) ? "" : reg.GetString("ZZDOCELEC"),
                                             CERDT = (provList.GetString("CERDT") == null || provList.GetString("CERDT").Equals("0000-00-00") ? DateTime.Parse("1900/01/01") : DateTime.Parse(provList.GetString("CERDT").Replace("-", "/"))).ToString("yyyy/MM/dd"),
                                             MINDK = String.IsNullOrEmpty(reg.GetString("MINDK")) ? "" : reg.GetString("MINDK"),
                                             ZZIOPLN = String.IsNullOrEmpty(reg.GetString("ZZIOPLN")) ? "" : reg.GetString("ZZIOPLN"),
                                             ZZIOPLA = String.IsNullOrEmpty(reg.GetString("ZZIOPLA")) ? "" : reg.GetString("ZZIOPLA"),
                                             ZZSTCDAP = String.IsNullOrEmpty(reg.GetString("ZZSTCDAP")) ? "" : reg.GetString("ZZSTCDAP"),
                                             FECHAMOD = (provList.GetString("FECHAMOD") == null || provList.GetString("FECHAMOD").Equals("0000-00-00") ? DateTime.Parse("1900/01/01") : DateTime.Parse(provList.GetString("FECHAMOD").Replace("-", "/"))).ToString("yyyy/MM/dd")
                                         }).ToList();

                    for (skipe = 0; skipe < Total; skipe = skipe + p_stake)
                    {
                        try
                        {
                            var wregistro = (from wreg in wregistroprov
                                             select wreg).Skip(skipe).Take(p_stake).ToList();

                            var wElementprovlist = (
                                new System.Xml.Linq.XDocument(
                                    new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                    new XElement("CARGAPROPVEEDOR",
                                        from reg in wregistro.AsEnumerable().AsParallel()
                                        select new XElement("PROV",
                                            new XAttribute("LIFNR", reg.LIFNR),
                                            new XAttribute("STCD1", reg.STCD1),
                                            new XAttribute("KTOKK", reg.KTOKK),
                                            new XAttribute("NAME1", reg.NAME1),
                                            new XAttribute("STRAS", reg.STRAS),
                                            new XAttribute("FLOOR", reg.FLOOR),
                                            new XAttribute("STR_SUPPL3", reg.STR_SUPPL3),
                                            new XAttribute("ORT02", reg.ORT02),
                                            new XAttribute("PSTLZ", reg.PSTLZ),
                                            new XAttribute("ORT01", reg.ORT01),
                                            new XAttribute("LAND1", reg.LAND1),
                                            new XAttribute("REGIO", reg.REGIO),
                                            new XAttribute("SPRAS", reg.SPRAS),
                                            new XAttribute("TELF1", reg.TELF1),
                                            new XAttribute("TELF2", reg.TELF2),
                                            new XAttribute("TELFX", reg.TELFX),
                                            new XAttribute("SMTP_ADDR", reg.SMTP_ADDR),
                                            new XAttribute("ZZDOCELEC", reg.ZZDOCELEC),
                                            new XAttribute("CERDT", reg.CERDT),
                                            new XAttribute("MINDK", reg.MINDK),
                                            new XAttribute("ZZIOPLN", reg.ZZIOPLN),
                                            new XAttribute("ZZIOPLA", reg.ZZIOPLA),
                                            new XAttribute("ZZSTCDAP", reg.ZZSTCDAP),
                                            new XAttribute("FECHAMOD", reg.FECHAMOD),
                                            (from bank in wregistrobank.AsEnumerable()
                                             where bank.LIFNR == reg.LIFNR
                                             select bank).Count() > 0 ?
                                             from bank in wregistrobank.AsEnumerable().AsParallel()
                                             where bank.LIFNR == reg.LIFNR
                                             select new XElement("BANK",
                                                 new XAttribute("LIFNR", bank.LIFNR),
                                                 new XAttribute("BANKL", bank.BANKL),
                                                 new XAttribute("BANKN", bank.BANKN)) : null,
                                                 (from cont in wregistrocont.AsEnumerable()
                                                  where cont.LIFNR == reg.LIFNR
                                                  select cont).Count() > 0 ?
                                                  from cont in wregistrocont.AsEnumerable().AsParallel()
                                                  where cont.LIFNR == reg.LIFNR
                                                  select new XElement("CONTACT",
                                                      new XAttribute("LIFNR", cont.LIFNR),
                                                      new XAttribute("PARNR", cont.PARNR),
                                                      new XAttribute("ANRED", cont.ANRED),
                                                      new XAttribute("NAMEV", cont.NAMEV),
                                                      new XAttribute("NAME1", cont.NAME1),
                                                      new XAttribute("ABTPA", cont.ABTPA),
                                                      new XAttribute("ABTNR", cont.ABTNR),
                                                      new XAttribute("PAFKT", cont.PAFKT),
                                                      new XAttribute("TELF1", cont.TELF1),
                                                      new XAttribute("TEL_NUMBER", cont.TEL_NUMBER),
                                                      new XAttribute("SMTP_ADDR", cont.SMTP_ADDR)) : null,
                                                      (from legacy in wregistrolegacy.AsEnumerable()
                                                       where legacy.LIFNR == reg.LIFNR
                                                       select legacy).Count() > 0 ?
                                                       from legacy in wregistrolegacy.AsEnumerable().AsParallel()
                                                       where legacy.LIFNR == reg.LIFNR
                                                       select new XElement("LEGACY",
                                                           new XAttribute("LIFNR", legacy.LIFNR),
                                                           new XAttribute("KOLIF", legacy.KOLIF)) : null))
                                                           ).ToString()).ToString();

                            String IfechaGraba = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                            if (!String.IsNullOrEmpty(wElementprovlist))
                            {
                                wElementprovlist = wElementprovlist.Replace("'", " ");

                                ds = objEjecucion.EjecucionGralDs(wElementprovlist.ToString(), 218, 1);
                                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                                {
                                    wresult = "";

                                }
                                else
                                {
                                    wresult = "Error en Proveedores: " + ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            wresult = "Error en Proveedores: " + ex.Message.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                wresult = "Error en Proveedores: " + ex.Message.ToString();
            }
            return wresult;
        }

        private String bapiupdate(DMSolcitudProveedor solicitudPar)
        {
            var msj = "";
            try
            {
                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;
                IRfcFunction fndatosmaestro;
                fndatosmaestro = repo.CreateFunction("ZPPPROVEEDORMOD");
                CatalogosController catalag = new CatalogosController();

                var DTPTPROV = fndatosmaestro.GetTable("PT_PROV");
                var DTRETENCION = fndatosmaestro.GetTable("PT_RETENCION");
                IRfcStructure ITPTPROV;
                IRfcStructure ITRETENCION;
                ITPTPROV = repo.GetStructureMetadata("ZWAPPPROVEEDORES").CreateStructure();
                //PT_PROV	ZWAPPPROVEEDORES		Estructura con Datos de Proveedores	Creación 	

                //LIFNR	CHAR	10	ID de Proveedor	
                string AUX = "";
                AUX = (Convert.ToInt64(solicitudPar.p_SolProveedor[0].CodSapProveedor) + 10000000000).ToString().Substring(1);
                ITPTPROV.SetValue("LIFNR", AUX);
                //BUKRS	CHAR	4	Sociedad	X
                ITPTPROV.SetValue("BUKRS", "1001");
                //STCD1	CHAR	16	RUC	X              
                ITPTPROV.SetValue("STCD1", solicitudPar.p_SolProveedor[0].Identificacion);
                //KTOKK	CHAR	4	Grupo de cuentas acreedor (Tipo de Proveedor )	X
                ITPTPROV.SetValue("KTOKK", !String.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].GrupoCuenta) ? solicitudPar.p_SolProveedor[0].GrupoCuenta : "NACN");
                //NAME1	CHAR	35	Nombre 1 (Razón Social)	X
                if (solicitudPar.p_SolProveedor[0].RazonSocial.Length > 35)
                {
                    if (solicitudPar.p_SolProveedor[0].RazonSocial.Length > 70)
                    {
                        if (solicitudPar.p_SolProveedor[0].RazonSocial.Length > 105)
                        {
                            ITPTPROV.SetValue("NAME1", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(0, 35));
                            ITPTPROV.SetValue("NAME2", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(35, 35));
                            ITPTPROV.SetValue("NAME3", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(70, 35));
                            ITPTPROV.SetValue("NAME4", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(105));
                        }
                        else
                        {
                            ITPTPROV.SetValue("NAME1", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(0, 35));
                            ITPTPROV.SetValue("NAME2", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(35, 35));
                            ITPTPROV.SetValue("NAME3", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(70));
                        }
                    }
                    else
                    {
                        ITPTPROV.SetValue("NAME1", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(0, 35));
                        ITPTPROV.SetValue("NAME2", solicitudPar.p_SolProveedor[0].RazonSocial.Substring(35));
                    }
                }
                else
                {
                    ITPTPROV.SetValue("NAME1", solicitudPar.p_SolProveedor[0].RazonSocial);
                }
                //NAME2	CHAR	35	Nombre 2 (Razón Social)	
                //NAME3	CHAR	35	Nombre 3 (Razón Social)	
                //NAME4	CHAR	35	Nombre 4 (Razón Social)	
                //SORT1	CHAR	20	Concepto de búsqueda 1 (Nombre comercial)	
                ITPTPROV.SetValue("SORT1", solicitudPar.p_SolProveedor[0].NomComercial);
                //STRAS	CHAR	30	Calle y número	X
                ITPTPROV.SetValue("STRAS", solicitudPar.p_SolProvDireccion[0].CallePrincipal + (!string.IsNullOrEmpty(solicitudPar.p_SolProvDireccion[0].Solar) ? "|" + solicitudPar.p_SolProvDireccion[0].Solar : ""));
                //FLOOR	CHAR	10	Planta del edificio	
                ITPTPROV.SetValue("FLOOR", solicitudPar.p_SolProvDireccion[0].PisoEdificio);
                //STR_SUPPL3	CHAR	40	Calle 4	
                ITPTPROV.SetValue("STR_SUPPL3", solicitudPar.p_SolProvDireccion[0].CalleSecundaria);
                //ORT02	CHAR	35	Distrito	
                //PSTLZ	CHAR	10	Código postal	
                ITPTPROV.SetValue("PSTLZ", solicitudPar.p_SolProvDireccion[0].CodPostal);
                //ORT01	CHAR	35	Población	X
                var codCiudad = !String.IsNullOrEmpty(solicitudPar.p_SolProvDireccion[0].Ciudad) ? solicitudPar.p_SolProvDireccion[0].Ciudad.Split('-')[0] : "NA";
                var listaCiudades = catalag.GetCatalogos("tbl_Ciudad");
                var descCiudad = listaCiudades.Where(x => x.Codigo == codCiudad).FirstOrDefault();
                ITPTPROV.SetValue("ORT01", descCiudad == null ? "" : descCiudad.Detalle);
                //LAND1	CHAR	3	Clave de país	X
                ITPTPROV.SetValue("LAND1", solicitudPar.p_SolProvDireccion[0].Pais);
                //REGIO	CHAR	3	Región (Estado federal, "land", provincia, condado)	X
                ITPTPROV.SetValue("REGIO", !String.IsNullOrEmpty(solicitudPar.p_SolProvDireccion[0].Provincia) ? solicitudPar.p_SolProvDireccion[0].Provincia.Split('-')[1] : "RE");
                //SPRAS	LANG	1	Clave de idioma	X
                var codIdioma = solicitudPar.p_SolProveedor[0].Idioma;
                var listaIdiomas = catalag.GetCatalogos("tbl_Idioma");
                var descAlIdioma = listaIdiomas.Where(x => x.Codigo == codIdioma).FirstOrDefault();

                ITPTPROV.SetValue("SPRAS", descAlIdioma != null ? descAlIdioma.DescAlterno : "S");
                //TELF1	CHAR	16	1º número de teléfono	X
                ITPTPROV.SetValue("TELF1", solicitudPar.p_SolProveedor[0].TelfFijo + (!string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].TelfFijoEXT) ? " " + solicitudPar.p_SolProveedor[0].TelfFijoEXT : ""));
                //TELF2	CHAR	16	Nº de teléfono 2	
                ITPTPROV.SetValue("TELF2", solicitudPar.p_SolProveedor[0].TelfMovil);
                //TELFX	CHAR	31	Nº telefax	
                var fax = solicitudPar.p_SolProveedor[0].TelfFax + (!string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].TelfFaxEXT) ? " " + solicitudPar.p_SolProveedor[0].TelfFaxEXT : "");
                if (fax != "")
                    ITPTPROV.SetValue("TELFX", fax);
                else
                    ITPTPROV.SetValue("TELFX", solicitudPar.p_SolProveedor[0].TelfFijo + (!string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].TelfFijoEXT) ? " " + solicitudPar.p_SolProveedor[0].TelfFijoEXT : ""));
                //SMTP_ADDR	CHAR	241	Dirección de correo electrónico	
                ITPTPROV.SetValue("SMTP_ADDR", solicitudPar.p_SolProveedor[0].EMAILCorp);
                //ZZDOCELEC	CHAR	1	Genera Documentos Electrónicos	
                ITPTPROV.SetValue("ZZDOCELEC", solicitudPar.p_SolProveedor[0].GenDocElec == "True" ? "X" : "");
                //CERDT	DATS	8	Fecha de certificación	X
                ITPTPROV.SetValue("CERDT", DateTime.Now.ToString("yyyyMMdd"));
                //MINDK	CHAR	3	Indicador de minorías	
                ITPTPROV.SetValue("MINDK", solicitudPar.p_SolProveedor[0].SectorComercial != null ? solicitudPar.p_SolProveedor[0].SectorComercial : "");

                //PLIFZ	DEC	3	Plazo de entrega previsto en días	
                ITPTPROV.SetValue("PLIFZ", !String.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].PlazoEntrega) ? solicitudPar.p_SolProveedor[0].PlazoEntrega : "0");
                //FECHAMOD	DATS	8	Fecha de última modificación	
                ITPTPROV.SetValue("CERDT", DateTime.Now.ToString("yyyyMMdd"));
                //STCDT	CHAR	2	Tipo de número de identificación fiscal	X
                ITPTPROV.SetValue("STCDT", solicitudPar.p_SolProveedor[0].TipoIdentificacion);
                //ANRED	CHAR	15	Tratamiento	
                ITPTPROV.SetValue("ANRED", "");
                //AKONT	CHAR	10	Cuenta asociada en la contabilidad principal	X
                ITPTPROV.SetValue("AKONT", String.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].CuentaAsociada.ToString().Trim().Split('-')[0].ToString()) ? "NA" : solicitudPar.p_SolProveedor[0].CuentaAsociada.ToString().Trim().Split('-')[1].ToString());
                //ZZMAILAP	CHAR	100	Mail del Apoderado	
                ITPTPROV.SetValue("ZZMAILAP", "");
                //EKORG	CHAR	4	Organización de compras	X
                ITPTPROV.SetValue("EKORG", "1001");
                //BEGRU	CHAR	4	Grupo autorizaciones	X
                ITPTPROV.SetValue("BEGRU", solicitudPar.p_SolProveedor[0].Autorizacion);
                //FITYP	CHAR	2	Clase de impuesto	X
                ITPTPROV.SetValue("FITYP", solicitudPar.p_SolProveedor[0].ClaseContribuyente);
                //FDGRV	CHAR	10	Grupo de tesorería	X
                ITPTPROV.SetValue("FDGRV", solicitudPar.p_SolProveedor[0].GrupoTesoreria == null ? "" : solicitudPar.p_SolProveedor[0].GrupoTesoreria.Split('-')[1]);
                //ZTERM	CHAR	4	Clave de condiciones de pago	X

                int carac = solicitudPar.p_SolProveedor[0].CondicionPago.IndexOf('-');

                var condPago = "";
                if (carac > 0)
                {
                    var condPagoAux = solicitudPar.p_SolProveedor[0].CondicionPago.ToString();
                    condPago = String.IsNullOrEmpty(condPagoAux) ? "" : condPagoAux.Split('-').Length > 0 ?
                                                                        condPagoAux.Substring(condPagoAux.Split('-')[0].Length + 1) :
                                                                        condPagoAux;
                }

                ITPTPROV.SetValue("ZTERM", condPago);
                //ZWELS	CHAR	10	Lista de las vías de pago a tener en cuenta	X
                var lisViaPago = "";

                if (solicitudPar.p_SolViapago != null && solicitudPar.p_SolViapago.Count() > 0)
                {
                    foreach (var a in solicitudPar.p_SolViapago)
                    {
                        lisViaPago = lisViaPago + a.CodVia;
                    }
                }
                ITPTPROV.SetValue("ZWELS", lisViaPago);
                //BRSCH	CHAR	4	Clave de ramo industrial	
                ITPTPROV.SetValue("BRSCH", solicitudPar.p_SolProveedor[0].Ramo == null ? "" : solicitudPar.p_SolProveedor[0].Ramo);
                //KALSK	CHAR	2	Grupo para esquema de cálculo (Proveedor)	
                ITPTPROV.SetValue("KALSK", solicitudPar.p_SolProveedor[0].GrupoEsquema);
                //WAERS	CUKY	5	Clave de moneda	
                ITPTPROV.SetValue("WAERS", "USD");
                //EKGRP	CHAR	3	Grupo de compras
                ITPTPROV.SetValue("EKGRP", solicitudPar.p_SolProveedor[0].GrupoCompra);

                DTPTPROV.Append(ITPTPROV);

                //ITRETENCION
                if (!string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionFuente) && !string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionFuente2))
                {
                    ITRETENCION = repo.GetStructureMetadata("ZWAPPLFBW").CreateStructure();

                    ITRETENCION.SetValue("LIFNR", solicitudPar.p_SolProveedor[0].CodSapProveedor);
                    ITRETENCION.SetValue("BUKRS", "1001");
                    ITRETENCION.SetValue("WT_SUBJCT", "X");
                    ITRETENCION.SetValue("WITHT", solicitudPar.p_SolProveedor[0].RetencionFuente);
                    ITRETENCION.SetValue("WT_WITHCD", !String.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionFuente2) ? solicitudPar.p_SolProveedor[0].RetencionFuente2.Split('-')[2] : "");
                    DTRETENCION.Append(ITRETENCION);
                }

                if (!string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionIva) && !string.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionIva2))
                {
                    ITRETENCION = repo.GetStructureMetadata("ZWAPPLFBW").CreateStructure();

                    ITRETENCION.SetValue("LIFNR", solicitudPar.p_SolProveedor[0].CodSapProveedor);
                    ITRETENCION.SetValue("BUKRS", "1001");
                    ITRETENCION.SetValue("WITHT", solicitudPar.p_SolProveedor[0].RetencionIva);
                    ITRETENCION.SetValue("WT_SUBJCT", "X");
                    ITRETENCION.SetValue("WT_WITHCD", !String.IsNullOrEmpty(solicitudPar.p_SolProveedor[0].RetencionIva2) ? solicitudPar.p_SolProveedor[0].RetencionIva2.Split('-')[2] : "");
                    DTRETENCION.Append(ITRETENCION);
                }

                fndatosmaestro.SetValue("PT_PROV", DTPTPROV);
                fndatosmaestro.SetValue("PT_RETENCION", DTRETENCION);

                //PT_GRUPAUT catalogo para BEGRU
                fndatosmaestro.Invoke(AppConfig.dest);

                var tablaLog = fndatosmaestro.GetTable("PT_LOG");
                var CODERROR = fndatosmaestro.GetString("CODERROR");
                var DESERROR = fndatosmaestro.GetString("DESERROR");
                var PT_PROV = fndatosmaestro.GetTable("PT_PROV");
                if (CODERROR == "0")
                {
                    msj = crearProveedor(solicitudPar, AUX);
                    msj = "";
                }

                else
                {
                    var result = (from a in tablaLog
                                  select new
                                  {
                                      MESSAGE = a.GetString("MESSAGE"),
                                      NUMBER = a.GetString("NUMBER"),
                                      ID = a.GetString("ID"),
                                  }).ToList();
                    msj = "";
                    foreach (var m in result)
                    {
                        msj = msj + m.NUMBER + "-" + m.MESSAGE + "\n";
                    }

                    msj = "Error en SAP: " + msj;
                }
            }
            catch (Exception ex)
            {
                msj = "Error en SAP: " + ex.Message.ToString();
            }
            return msj;
        }

        private String enviasolitudbapi(DMSolcitudProveedor solicitudPar)
        {
            var msj = "";
            try
            {
                if (solicitudPar != null)
                {
                    if (solicitudPar.p_SolProveedor[0].TipoSolicitud != "AT")
                    {
                        msj = bapicreacion(solicitudPar);
                    }
                    else
                    {
                        msj = bapiupdate(solicitudPar);
                    }
                }
            }
            catch (Exception)
            {
            }
            return msj;
        }

        [ActionName("getProveedorContactoList")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProvContacto> getProveedorContactoList(String idproveedorconta)
        {
            List<DMSolcitudProveedor.SolProvContacto> Retorno = new List<DMSolcitudProveedor.SolProvContacto>();
            try
            {
                var ValorTokenUser = string.Empty;
                ValorTokenUser = InitialiseService.GetTokenUser();

                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                var datos = Proceso.getProveedorContactoList(InitialiseService.Semilla,
                        ValorTokenUser,
                        InitialiseService.IdOrganizacion,
                        idproveedorconta);
                foreach (var a in datos)
                {
                    DMSolcitudProveedor.SolProvContacto aux = new DMSolcitudProveedor.SolProvContacto();
                    aux.Apellido1 = a.Apellido1;
                    aux.Apellido2 = a.Apellido2;
                    aux.CodSapContacto = a.CodSapContacto;
                    aux.Departamento = a.Departamento;
                    aux.DepCliente = a.DepCliente;
                    aux.DescDepartamento = a.DescDepartamento;
                    aux.DescFuncion = a.DescFuncion;
                    aux.DescTipoIdentificacion = a.DescTipoIdentificacion;
                    aux.EMAIL = a.EMAIL;
                    aux.Estado = a.Estado;
                    aux.Funcion = a.Funcion;
                    aux.Identificacion = a.Identificacion;
                    aux.IdSolContacto = a.IdSolContacto;
                    aux.IdSolicitud = a.IdSolicitud;
                    aux.Nombre1 = a.Nombre1;
                    aux.Nombre2 = a.Nombre2;
                    aux.NotElectronica = a.NotElectronica;
                    aux.NotTransBancaria = a.NotTransBancaria;
                    aux.PreFijo = a.PreFijo;
                    aux.RepLegal = a.RepLegal;
                    aux.TelfFijo = a.TelfFijo;
                    aux.TelfFijoEXT = a.TelfFijoEXT;
                    aux.TelfMovil = a.TelfMovil;
                    aux.TipoIdentificacion = a.TipoIdentificacion;
                    Retorno.Add(aux);
                }
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }
            return Retorno;
        }

        [ActionName("getProveedorDireccionList")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProvDireccion> getProveedorDireccionList(String idproveedordir)
        {
            List<DMSolcitudProveedor.SolProvDireccion> Retorno = new List<DMSolcitudProveedor.SolProvDireccion>();
            try
            {
                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                var datos = Proceso.getProveedorDireccionList(idproveedordir);
                foreach (var a in datos)
                {
                    DMSolcitudProveedor.SolProvDireccion aux = new DMSolcitudProveedor.SolProvDireccion();

                    aux.CalleSecundaria = a.CalleSecundaria;
                    aux.Ciudad = a.Ciudad;
                    aux.CodPostal = a.CodPostal;
                    aux.DescPais = a.DescPais;
                    aux.DescRegion = a.DescRegion;
                    aux.Estado = a.Estado;
                    aux.IdDireccion = a.IdDireccion;
                    aux.IdSolicitud = a.IdSolicitud;
                    aux.Pais = a.Pais;
                    aux.PisoEdificio = a.PisoEdificio;
                    aux.Provincia = a.Provincia;

                    int carac = a.CallePrincipal.IndexOf('|');
                    if (carac > 0)
                    {
                        aux.CallePrincipal =
                                       String.IsNullOrEmpty(a.CallePrincipal) ? "" : a.CallePrincipal.Split('|').Length > 0 ?
                                       a.CallePrincipal.Split('|')[0] :
                                       a.CallePrincipal;

                        aux.Solar = String.IsNullOrEmpty(a.CallePrincipal) ? "" : a.CallePrincipal.Split('|').Length > 0 ? a.CallePrincipal.Split('|')[1] : a.CallePrincipal;
                    }
                    else
                    {
                        aux.CallePrincipal = a.CallePrincipal;
                        aux.Solar = "";
                    }

                    Retorno.Add(aux);
                }
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }

            return Retorno;
        }

        [ActionName("ConsultaProveedorBapi")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProveedor> ConsultaProveedorBapi(String idproveedor)
        {
            List<DMSolcitudProveedor.SolProveedor> Retorno = new List<DMSolcitudProveedor.SolProveedor>();
            try
            {
                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                CatalogosController catalag = new CatalogosController();
                var datos = Proceso.ConsultaProveedorBapi(idproveedor);
                foreach (var a in datos)
                {
                    DMSolcitudProveedor.SolProveedor aux = new DMSolcitudProveedor.SolProveedor();
                    aux.AnioConsti = a.AnioConsti;
                    aux.Aprobacion = a.Aprobacion;
                    aux.Autorizacion = a.Autorizacion;
                    aux.ClaseContribuyente = a.ClaseContribuyente;
                    aux.CodGrupoProveedor = a.CodGrupoProveedor;
                    aux.CodSapProveedor = a.CodSapProveedor;
                    aux.Comentario = a.Comentario;
                    aux.CondicionPago = a.CondicionPago;
                    aux.CuentaAsociada = a.GrupoCuenta + "-" + a.CuentaAsociada;
                    aux.DepSolicitando = a.DepSolicitando;
                    aux.DescClaseContribuyente = a.DescClaseContribuyente;
                    aux.DescCondicionPago = a.DescCondicionPago;
                    aux.DescCuentaAsociada = a.DescCuentaAsociada;
                    aux.DescDespachaProvincia = a.DescDespachaProvincia;
                    aux.DescEstado = a.DescEstado;
                    aux.DescGrupoCuenta = a.DescGrupoCuenta;
                    aux.DescGrupoTesoreria = a.DescGrupoTesoreria;
                    aux.DescIdioma = a.DescIdioma;
                    aux.DescLineaNegocio = a.DescLineaNegocio;
                    aux.DescProveedor = a.DescProveedor;
                    aux.DescRetencionFuente = a.DescRetencionFuente;
                    aux.DescRetencionFuente2 = a.DescRetencionFuente2;
                    aux.DescRetencionIva = a.DescRetencionIva;
                    aux.DescRetencionIva2 = a.DescRetencionIva2;
                    aux.DescSectorComercial = a.DescSectorComercial;
                    aux.DEscTipoIndentificacion = a.DEscTipoIndentificacion;
                    aux.DescTipoSolicitud = a.DescTipoSolicitud;
                    aux.DespachaProvincia = a.DespachaProvincia;
                    aux.EMAILCorp = a.EMAILCorp;
                    aux.EMAILSRI = a.EMAILSRI;
                    aux.Estado = a.Estado;
                    aux.FechaSolicitud = a.FechaSolicitud;
                    aux.FechaSRI = a.FechaSRI;
                    aux.GenDocElec = a.GenDocElec;
                    aux.GrupoCuenta = a.GrupoCuenta;
                    aux.GrupoTesoreria = a.GrupoTesoreria;
                    aux.IdEmpresa = a.IdEmpresa;
                    aux.Identificacion = a.Identificacion;

                    var listaIdiomas = catalag.GetCatalogos("tbl_Idioma");
                    if (a.Idioma != null)
                    {
                        var codIdioma = listaIdiomas.Where(x => x.DescAlterno == a.Idioma).FirstOrDefault();
                        aux.Idioma = codIdioma != null ? codIdioma.Codigo : "ES";
                    }
                    else
                        aux.Idioma = "ES";
                    aux.IdSolicitud = a.IdSolicitud;

                    if (a.LineaNegocio != "")
                    {
                        var relLinGrupoC = catalag.GetCatalogos("tbl_ProvGrupoCompras");
                        var linea = relLinGrupoC.Where(x => x.Codigo == a.LineaNegocio).FirstOrDefault();
                        aux.LineaNegocio = linea != null ? linea.DescAlterno : "";

                    }
                    else
                        aux.LineaNegocio = "";

                    aux.GrupoCompra = a.LineaNegocio != "" ? a.LineaNegocio : "";


                    aux.NomComercial = a.NomComercial;
                    aux.PlazoEntrega = a.PlazoEntrega;
                    aux.princliente = a.princliente;
                    aux.RazonSocial = a.RazonSocial;
                    aux.Responsable = a.Responsable;
                    aux.RetencionFuente = a.RetencionFuente;
                    aux.RetencionFuente2 = a.RetencionFuente2;
                    aux.RetencionIva = a.RetencionIva;
                    aux.RetencionIva2 = a.RetencionIva2;
                    aux.SectorComercial = a.SectorComercial;
                    aux.TelfFax = a.TelfFax;
                    aux.TelfFaxEXT = a.TelfFaxEXT;
                    int carac = a.TelfFijo.IndexOf(' ');
                    if (carac > 0)
                    {
                        aux.TelfFijo =
                                       String.IsNullOrEmpty(a.TelfFijo) ? "" : a.TelfFijo.Split(' ').Length > 0 ?
                                       a.TelfFijo.Split(' ')[0] :
                                       a.TelfFijo;

                        aux.TelfFijoEXT = String.IsNullOrEmpty(a.TelfFijo) ? "" : a.TelfFijo.Split(' ').Length > 0 ? a.TelfFijo.Split(' ')[1] : a.TelfFijo;
                    }
                    else
                    {
                        aux.TelfFijo = a.TelfFijo;
                        aux.TelfFijoEXT = "";
                    }

                    aux.TelfMovil = a.TelfMovil;
                    aux.TipoIdentificacion = a.TipoIdentificacion;
                    aux.TipoProveedor = a.TipoProveedor;
                    aux.TipoSolicitud = a.TipoSolicitud;
                    aux.totalventas = a.totalventas;
                    aux.TransfArticuProvAnterior = a.TransfArticuProvAnterior;
                    Retorno.Add(aux);
                }
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }
            return Retorno;
        }

        private Boolean BajaFptArchivo(string path_comp, string nom_archivo, string aux)
        {
            try
            {
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path_comp));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path_comp));
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path_comp), nom_archivo);

                using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                {
                    sftpClient.Connect();
                    if (!sftpClient.Exists(AppConfig.SftpPath + "SolicitudProveedor/" + path_comp))
                    {
                        sftpClient.CreateDirectory(AppConfig.SftpPath + "SolicitudProveedor/" + path_comp);
                    }

                    Stream fin = File.OpenWrite(fileSavePath);
                    sftpClient.DownloadFile(AppConfig.SftpPath + "SolicitudProveedor/" + path_comp + "/" + nom_archivo, fin, null);

                    fin.Close();
                    sftpClient.Disconnect();
                }
            }
            catch (Exception)
            {
            }

            return true;
        }

        [ActionName("ConsultaBandejaSolicitud")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProveedor> GetConsultaBandejaSolicitud(String bIdentificacion, String FecDesde, String FecHasta, String BEstado, String Pantalla, String Opcion, String BIDLINEA, String BIDUSUARIO)
        {
            List<DMSolcitudProveedor.SolProveedor> Retorno = new List<DMSolcitudProveedor.SolProveedor>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();

            if (FecDesde == "undefined")
            {
                FecDesde = null;
            }
            if (FecHasta == "undefined")
            {
                FecHasta = null;
            }

            xmlParam.LoadXml("<Root />");
            try
            {
                if (!String.IsNullOrEmpty(bIdentificacion)) xmlParam.DocumentElement.SetAttribute("Ruc", bIdentificacion);
                if (!String.IsNullOrEmpty(FecDesde)) xmlParam.DocumentElement.SetAttribute("FechaDesde", FecDesde);
                if (!String.IsNullOrEmpty(FecHasta)) xmlParam.DocumentElement.SetAttribute("FechaHasta", FecHasta);
                if (!String.IsNullOrEmpty(BEstado)) xmlParam.DocumentElement.SetAttribute("Estado", BEstado);
                if (!String.IsNullOrEmpty(Pantalla)) xmlParam.DocumentElement.SetAttribute("Pantalla", Pantalla);
                if (!String.IsNullOrEmpty(Opcion)) xmlParam.DocumentElement.SetAttribute("Opcion", Opcion);
                if (!String.IsNullOrEmpty(BIDLINEA)) xmlParam.DocumentElement.SetAttribute("IDLINEA", BIDLINEA);
                if (!String.IsNullOrEmpty(BIDUSUARIO)) xmlParam.DocumentElement.SetAttribute("IDUSUARIO", BIDUSUARIO);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 211, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count > 1)
                    {
                        Retorno = MapSolProveedor(ds);
                    }
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("ConsultaLineaAdmin")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolLineaAdmin> GetSolLineaAdmin(string IDNIVEL, string IDLINEA, string IDMODULO, string IDUSUARIO)
        {
            IEnumerable<DMSolcitudProveedor.SolLineaAdmin> Retorno = new List<DMSolcitudProveedor.SolLineaAdmin>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IDNIVEL", IDNIVEL);
                xmlParam.DocumentElement.SetAttribute("IDLINEA", IDLINEA);
                xmlParam.DocumentElement.SetAttribute("IDMODULO", IDMODULO);
                xmlParam.DocumentElement.SetAttribute("IDUSUARIO", IDUSUARIO);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 216, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolLineaAdmin
                               {
                                   Linea = reg.Field<String>("Linea"),
                                   DescLinea = reg.Field<String>("DescLinea")

                               }).ToList<DMSolcitudProveedor.SolLineaAdmin>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }

            return Retorno;
        }


        //Consulta linea de negocios
        [ActionName("GetListaDocumentosAdjuntos")]
        [HttpGet]
        public IEnumerable<DMProveedor.DocumentoAdjunto> GetListaDocumentosAdjuntos(string TipoPersona)
        {
            IEnumerable<DMProveedor.DocumentoAdjunto> Retorno = new List<DMProveedor.DocumentoAdjunto>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "2");
                xmlParam.DocumentElement.SetAttribute("TipoPersona", TipoPersona);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 106, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMProveedor.DocumentoAdjunto
                               {
                                   Codigo = reg.Field<String>("Codigo"),
                                   Descripcion = reg.Field<String>("Descripcion"),
                                   Obligatorio = reg.Field<String>("EsObligatorio"),
                               }).ToList<DMProveedor.DocumentoAdjunto>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("GetListaAdjuntosCondicionales")]
        [HttpGet]
        public IEnumerable<DMProveedor.DocumentoAdjunto> GetListaAdjuntosCondicionales(string ProcesoSoporte)
        {
            IEnumerable<DMProveedor.DocumentoAdjunto> Retorno = new List<DMProveedor.DocumentoAdjunto>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "3");
                xmlParam.DocumentElement.SetAttribute("TipoPersona", "CO");
                xmlParam.DocumentElement.SetAttribute("ProcesoSoporte", ProcesoSoporte);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 106, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMProveedor.DocumentoAdjunto
                               {
                                   Codigo = reg.Field<String>("Codigo"),
                                   Descripcion = reg.Field<String>("Descripcion"),
                                   Obligatorio = reg.Field<String>("EsObligatorio"),
                               }).ToList<DMProveedor.DocumentoAdjunto>();
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        [ActionName("EscribePDFAdjuntos")]
        [Route("EscribePDFAdjuntos")]
        [HttpPost]
        public FormResponseModelo EscribePDFAdjuntos(string rutaDirectorio,string nombre)
        {
            string _metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            FormResponseModelo _oEscribePDFAdjuntos = new FormResponseModelo();
            try
            {
                string RutaLectura = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivo"]).Trim();
                RutaLectura = Path.Combine(RutaLectura, rutaDirectorio, nombre);

                p_Log.Graba_Log_Info("Ruta Adjunto: " + RutaLectura);

                if (File.Exists(Path.Combine(RutaLectura)))
                {
                    p_Log.Graba_Log_Info("Adjunto: Existe.");
                }
                else
                {
                    p_Log.Graba_Log_Info("Adjunto: No existe o no se tiene acceso a la ruta.");
                }

                byte[] archivo = File.ReadAllBytes(RutaLectura);
                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                Proceso.Url = ((string)System.Configuration.ConfigurationManager.AppSettings["Urlbase"]).Trim();
                Proceso.EscribePdfAdjunto(archivo, rutaDirectorio, nombre);

                _oEscribePDFAdjuntos.lSuccess = true;
                _oEscribePDFAdjuntos.cCodError = "0";
            }
            catch (Exception ex)
            {
                _oEscribePDFAdjuntos.lSuccess = false;
                _oEscribePDFAdjuntos.cCodError = "9999";
                _oEscribePDFAdjuntos.cMsgError = ex.Message.ToString();

                p_Log.Graba_Log_Error(ex.Message.ToString());
            }

            p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

            return _oEscribePDFAdjuntos;
        }

    }
}

