using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJSAuthentication.API.Models;
using System.Data;
using clibProveedores;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/Proveedor")]
    public class ProveedorController : ApiController
    {
        readonly string MetodoController = "ProveedorController";

        [ActionName("ConsultaProveedor")]
        [HttpGet]
        public IEnumerable<DMProveedor.Proveedor> GetConsultaProveedor(String IdEmpresa,String CodProveedor,String Ruc, String FechaDesde,String FechaHasta  )
        {
            List<DMProveedor.Proveedor> Retorno = new List<DMProveedor.Proveedor>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();

            xmlParam.LoadXml("<Root />");
            try
            {


                if (!String.IsNullOrEmpty(IdEmpresa)) xmlParam.DocumentElement.SetAttribute("IdEmpresa", IdEmpresa);
                if (!String.IsNullOrEmpty(CodProveedor)) xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);
                if (!String.IsNullOrEmpty(Ruc)) xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                if (!String.IsNullOrEmpty(FechaDesde)) xmlParam.DocumentElement.SetAttribute("FechaDesde", FechaDesde);
                if (!String.IsNullOrEmpty(FechaHasta)) xmlParam.DocumentElement.SetAttribute("FechaHasta", FechaHasta);

                
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 210, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMProveedor.Proveedor
                               {        
                                   ApoderadoApe = reg.Field<String>("ApoderadoApe"),
                                   ApoderadoIdFiscal = reg.Field<String>("ApoderadoIdFiscal"),
                                   ApoderadoNom = reg.Field<String>("ApoderadoNom"),
                                   CodProveedor = reg.Field<String>("CodProveedor"),
                                   CorreoE = reg.Field<String>("CorreoE"),
                                   DirCalleNum = reg.Field<String>("DirCalleNum"),
                                   DirCallePrinc = reg.Field<String>("DirCallePrinc"),
                                   DirCodPostal = reg.Field<String>("DirCodPostal"),
                                   DirDistrito = reg.Field<String>("DirDistrito"),
                                   DirPisoEdificio = reg.Field<String>("DirPisoEdificio"),
                                   Fax = reg.Field<String>("Fax"),
                                   //FechaCertifica = DateTime.Parse(reg.Field<String>("FechaCertifica")),
                                   FechaMod = reg.Field<DateTime>("FechaMod"),
                                   GenDocElec = reg.Field<String>("GenDocElec"),
                                   Idioma = reg.Field<String>("Idioma"),
                                   IndMinoria = reg.Field<String>("IndMinoria"),
                                   Movil = reg.Field<String>("Movil"),
                                   NomComercial = reg.Field<String>("NomComercial"),
                                   Pais = reg.Field<String>("Pais"),
                                   PlazoEntregaPrev = reg.Field<String>("PlazoEntregaPrev"),
                                   Poblacion = reg.Field<String>("Poblacion"),
                                   Region = reg.Field<String>("Region"),
                                   Ruc = reg.Field<String>("Ruc"),
                                   Telefono = reg.Field<String>("Telefono"),
                                   TipoProveedor = reg.Field<String>("TipoProveedor"),


                                   //Aprobacion = reg.Field<String>("Aprobacion"),

                               }).ToList<DMProveedor.Proveedor>();
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


        [ActionName("ConsultaBanco")]
        [HttpGet]
        public IEnumerable<DMProveedor.Banco> GetConsultaBanco(String CodPais)
        {
            List<DMProveedor.Banco> Retorno = new List<DMProveedor.Banco>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();

            xmlParam.LoadXml("<Root />");
            try
            {


                if (!String.IsNullOrEmpty(CodPais)) xmlParam.DocumentElement.SetAttribute("CodPais", CodPais);
              


                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 213, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables.Count>1)
                    { 
                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMProveedor.Banco
                               {
                                   CodBancario = reg.Field<String>("CodBancario"),
                                   CodBanco = reg.Field<String>("CodBanco"),
                                   CodPais = reg.Field<String>("CodPais"),
                                   CodSWIFT = reg.Field<String>("CodSWIFT"),
                                   Direcion = reg.Field<String>("Direcion"),
                                   GrupoBancario = reg.Field<String>("GrupoBancario"),
                                   IndBorrado = reg.Field<String>("IndBorrado"),
                                   IndGiroCajapost = reg.Field<String>("IndGiroCajapost"),
                                   NomBanco = reg.Field<String>("NomBanco"),
                                   Poblacion = reg.Field<String>("Poblacion"),
                                   Region = reg.Field<String>("Region"),
                                           }).ToList<DMProveedor.Banco>();
                    }
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw;// new Exception(ex.Message.ToString());
            }

            return Retorno;
        }

        [ActionName("ConsultaProveedorSol")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProveedor> GetConsultaProveedorSol(String CodProveedor, String IdEmpresa)
        {
            List<DMSolcitudProveedor.SolProveedor> Retorno = new List<DMSolcitudProveedor.SolProveedor>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();

            xmlParam.LoadXml("<Root />");
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info("ProveedorController" + " -- GetConsultaProveedorSol " + " INI ");
            try
            {

                if (!String.IsNullOrEmpty(CodProveedor)) xmlParam.DocumentElement.SetAttribute("Tipo", "C");
                if (!String.IsNullOrEmpty(IdEmpresa)) xmlParam.DocumentElement.SetAttribute("IdEmpresa", IdEmpresa);
                if (!String.IsNullOrEmpty(CodProveedor)) xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);

                //if (!String.IsNullOrEmpty(Ruc)) xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                //if (!String.IsNullOrEmpty(FechaDesde)) xmlParam.DocumentElement.SetAttribute("FechaDesde", FechaDesde);
                //if (!String.IsNullOrEmpty(FechaHasta)) xmlParam.DocumentElement.SetAttribute("FechaHasta", FechaHasta);

                p_Log.Graba_Log_Info("ProveedorController" + " -- GetConsultaProveedorSol " + " Data " + xmlParam.OuterXml);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 210, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Retorno = MapSolProveedor(ds);
                }
                else
                {
                    p_Log.Graba_Log_Error("ProveedorController" + " -- GetConsultaProveedorSol " + " Error: " + ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                }
            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Error("ProveedorController" + " -- GetConsultaProveedorSol " + " Error: " + ex.Message);
                throw ex;
            }
            p_Log.Graba_Log_Info("ProveedorController" + " -- GetConsultaProveedorSol " + " FIN ");
            return Retorno;
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

        [ActionName("ConsultaProvDireccion")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProvDireccion> GetProvDireccion(string IdDirecSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolProvDireccion> Retorno = new List<DMSolcitudProveedor.SolProvDireccion>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info( MetodoController + " -- GetProvDireccion " + " INI ");
            try
            {
                
                xmlParam.DocumentElement.SetAttribute("CodProveedor", IdDirecSolicitud);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 209, 1);
                p_Log.Graba_Log_Info("GetProvDireccion:" + " -- Estado: " + ds.Tables["TblEstado"].Rows[0]["CodError"].ToString());
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    string jsonString = JsonConvert.SerializeObject(ds.Tables[0].AsEnumerable());
                    p_Log.Graba_Log_Info("GetProvDireccion:" + " -- DATA : " + jsonString);
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
                                   //IdSolicitud = reg["IdSolicitud"] != DBNull.Value ? reg["IdSolicitud"].ToString() : "",
                                   Pais = reg.Field<String>("Pais"),
                                   PisoEdificio = reg.Field<String>("PisoEdificio"),
                                   Provincia = reg.Field<String>("Provincia"),
                                   Solar = reg.Field<String>("Solar"),
                               }).ToList<DMSolcitudProveedor.SolProvDireccion>();
                }
                else 
                {
                    p_Log.Graba_Log_Error("GetProvDireccion:" + " -- Error_ESTADO : " + ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                }
            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Error("GetProvDireccion:" + " -- Error : " + ex.Message.ToString());
                throw ex;
            }

            return Retorno;
        }

        [ActionName("ConsultaDocAdjunto")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolDocAdjunto> GetDocAdjunto(string IdAdSolicitud)
        {
            IEnumerable<DMSolcitudProveedor.SolDocAdjunto> Retorno = new List<DMSolcitudProveedor.SolDocAdjunto>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("CodProveedor", IdAdSolicitud);
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
                                   //IdSolDocAdjunto = reg["IdSolDocAdjunto"] != DBNull.Value ? reg["IdSolDocAdjunto"].ToString() : "",
                                   //IdSolicitud = reg["IdSolicitud"] != DBNull.Value ? reg["IdSolicitud"].ToString() : "",
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

        [ActionName("ActualizaProveedor")]
        [HttpPost]
        public String GetActualizaProveedor(DMSolcitudProveedor SolProveedor)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            String idsolicitud = "";
            String CodSapProveedor = "";
            var CodProveedor = new DMSolcitudProveedor();
            CodProveedor = SolProveedor;
            String estado = CodProveedor.p_SolProveedor[0].Estado;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(MetodoController + " -- GetActualizaProveedor " + " INI ");
            try
            {
                idsolicitud = CodProveedor.p_SolProveedor[0].IdSolicitud;
                CodSapProveedor = CodProveedor.p_SolProveedor[0].CodSapProveedor;
                DataTable dt = new DataTable();

                dt.Columns.Add("IdSolicitud", Type.GetType("System.String"));
                dt.Columns.Add("TipMedioContacto", Type.GetType("System.String"));
                dt.Columns.Add("ValorMedioContacto", Type.GetType("System.String"));
                dt.Columns.Add("Estado", Type.GetType("System.String"));
                dt.Columns.Add("Contacto", Type.GetType("System.String"));
                dt.Columns.Add("Identificacion", Type.GetType("System.String"));
                dt.Columns.Add("IdSolContacto", Type.GetType("System.String"));

                if (CodProveedor.p_SolProvContacto != null)
                {
                    foreach (DMSolcitudProveedor.SolProvContacto erow in CodProveedor.p_SolProvContacto)
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

                }

                DataRow drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = CodProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "TLFFIJO";
                drowSOL["ValorMedioContacto"] = CodProveedor.p_SolProveedor[0].TelfFijo;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = CodProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);

                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = CodProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "TLFFIJOEXT";
                drowSOL["ValorMedioContacto"] = CodProveedor.p_SolProveedor[0].TelfFijoEXT;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = CodProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);

                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = CodProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "TLFMOVIL";
                drowSOL["ValorMedioContacto"] = CodProveedor.p_SolProveedor[0].TelfMovil;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = CodProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);

                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = CodProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "FAX";
                drowSOL["ValorMedioContacto"] = CodProveedor.p_SolProveedor[0].TelfFax;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = CodProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);

                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = CodProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "FAXEXT";
                drowSOL["ValorMedioContacto"] = CodProveedor.p_SolProveedor[0].TelfFaxEXT;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = CodProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);

                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = CodProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "EMAILCORP";
                drowSOL["ValorMedioContacto"] = CodProveedor.p_SolProveedor[0].EMAILCorp;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = CodProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);

                drowSOL = dt.NewRow();
                drowSOL["IdSolicitud"] = CodProveedor.p_SolProveedor[0].IdSolicitud;
                drowSOL["TipMedioContacto"] = "EMAILSRI";
                drowSOL["ValorMedioContacto"] = CodProveedor.p_SolProveedor[0].EMAILSRI;
                drowSOL["Estado"] = "True";
                drowSOL["Contacto"] = "N";
                drowSOL["Identificacion"] = CodProveedor.p_SolProveedor[0].Identificacion;
                dt.Rows.Add(drowSOL);

                dt.AcceptChanges();
                String idDireccion = String.Empty;
                if (CodProveedor.p_SolProvDireccion != null && CodProveedor.p_SolProvDireccion.Count() > 0)
                {
                    idDireccion = CodProveedor.p_SolProvDireccion[0].IdDireccion;
                }

                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                            from p in dt.AsEnumerable()
                            select new XElement("SolMedioContacto",
                                 new XAttribute("IdSolicitud", p["IdSolicitud"] != null ? p["IdSolicitud"] : ""),
                                 new XAttribute("CodProveedor", CodProveedor.p_SolProveedor[0].CodSapProveedor != null ? CodProveedor.p_SolProveedor[0].CodSapProveedor : ""),
                                 new XAttribute("IdSolContacto", p["IdSolContacto"] != null ? p["IdSolContacto"] : ""),
                                 new XAttribute("TipMedioContacto", p["TipMedioContacto"] != null ? p["TipMedioContacto"] : ""),
                                 new XAttribute("ValorMedioContacto", p["ValorMedioContacto"] != null ? p["ValorMedioContacto"] : ""),
                                 new XAttribute("Identificacion", p["Identificacion"] != null ? p["Identificacion"] : ""),
                                 new XAttribute("Contacto", p["Contacto"] != null ? p["Contacto"] : ""),
                                 new XAttribute("Estado", p["Estado"] != null ? p["Estado"] : "")),

                            SolProveedor.p_SolProvBanco != null ?
                                    from p in SolProveedor.p_SolProvBanco
                                    select new XElement("SolBanco",
                                         new XAttribute("IdSolicitud", p.IdSolicitud != null ? p.IdSolicitud : ""),
                                         new XAttribute("CodProveedor", CodProveedor.p_SolProveedor[0].CodSapProveedor != null ? CodProveedor.p_SolProveedor[0].CodSapProveedor : ""),
                                         new XAttribute("Extrangera", p.Extrangera != null ? p.Extrangera : ""),
                                         new XAttribute("CodSapBanco", p.CodSapBanco != null ? p.CodSapBanco : ""),
                                         new XAttribute("Pais", p.Pais != null ? p.Pais : ""),
                                         new XAttribute("TipoCuenta", p.TipoCuenta != null ? p.TipoCuenta : ""),

                                         new XAttribute("NumeroCuenta", p.NumeroCuenta != null ? p.NumeroCuenta : ""),
                                         new XAttribute("TitularCuenta", p.TitularCuenta != null ? p.TitularCuenta : ""),
                                         new XAttribute("ReprCuenta", p.ReprCuenta != null ? p.ReprCuenta : ""),
                                         new XAttribute("CodSwift", p.CodSwift != null ? p.CodSwift : ""),

                                         new XAttribute("CodBENINT", p.CodBENINT != null ? p.CodBENINT : ""),
                                         new XAttribute("CodABA", p.CodABA != null ? p.CodABA : ""),
                                         new XAttribute("Principal", p.Principal != null ? p.Principal.ToString() : ""),
                                         new XAttribute("Estado", p.Estado != null ? p.Estado.ToString() : ""),
                                          new XAttribute("IdSolBanco", p.IdSolBanco != null ? p.IdSolBanco : ""),
                                          new XAttribute("BancoExtranjero", p.BancoExtranjero != null ? p.BancoExtranjero : ""),
                                          new XAttribute("DirBancoExtranjero", p.DirBancoExtranjero != null ? p.DirBancoExtranjero : ""),
                                          new XAttribute("Provincia", p.Provincia != null ? p.Provincia : ""),
                                          new XAttribute("FormaPago", p.FormaPago != null ? p.FormaPago : ""),
                                         new XAttribute("ACCION", p.IdSolBanco != null && !String.IsNullOrEmpty(p.IdSolBanco.ToString()) && int.Parse(p.IdSolBanco.ToString()) > 0 ? "U" : "I")
                                        ) : null,

                                        CodProveedor.p_SolProvContacto != null ? from p in CodProveedor.p_SolProvContacto
                                                                                 select new XElement("SolContacto",
                                                                                      new XAttribute("IdSolicitud", p.IdSolicitud != null ? p.IdSolicitud : ""),
                                                                                      new XAttribute("CodProveedor", CodProveedor.p_SolProveedor[0].CodSapProveedor != null ? CodProveedor.p_SolProveedor[0].CodSapProveedor : ""),
                                                                                      new XAttribute("TipoIdentificacion", p.TipoIdentificacion != null ? p.TipoIdentificacion : ""),
                                                                                      new XAttribute("IdSolContacto", p.IdSolContacto != null ? p.IdSolContacto : ""),
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
                                                                                      new XAttribute("Estado", p.Estado != null ? p.Estado.ToString() : ""),
                                                                                      new XAttribute("NotElectronica", p.NotElectronica != null ? p.NotElectronica.ToString() : ""),
                                                                                      new XAttribute("NotTransBancaria", p.NotTransBancaria != null ? p.NotTransBancaria.ToString() : ""),

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
                                                                                      new XAttribute("ConyugeFechaNac", DateTime.Now.ToString("yyyy/MM/dd")),
                                                                                      new XAttribute("ConyugeNacionalidad", p.ConyugeNacionalidad != null ? p.ConyugeNacionalidad.ToString() : ""),

                                                                                      new XAttribute("RegimenMatrimonial", p.RegimenMatrimonial != null ? p.RegimenMatrimonial.ToString() : ""),
                                                                                      new XAttribute("RelacionDependencia", p.RelacionDependencia != null ? p.RelacionDependencia.ToString() : ""),
                                                                                      new XAttribute("AntiguedadLaboral", p.AntiguedadLaboral != null ? p.AntiguedadLaboral.ToString() : ""),
                                                                                      new XAttribute("TipoIngreso", p.TipoIngreso != null ? p.TipoIngreso.ToString() : ""),
                                                                                      new XAttribute("IngresoMensual", p.IngresoMensual != null ? p.IngresoMensual.ToString() : ""),
                                                                                      new XAttribute("TipoParticipante", p.TipoParticipante != null ? p.TipoParticipante.ToString() : ""),

                                                                                      new XAttribute("ACCION", p.IdSolContacto != null && !String.IsNullOrEmpty(p.IdSolContacto.ToString()) && int.Parse(p.IdSolContacto.ToString()) > 0 ? "U" : "I")
                                                                                      ) : null,


                    CodProveedor.p_SolProvDireccion != null ? new XElement("SolDireccion",
                                     new XAttribute("IdEmpresa", "1"),
                                     new XAttribute("IdSolicitud", CodProveedor.p_SolProveedor[0].IdSolicitud != null ? CodProveedor.p_SolProveedor[0].IdSolicitud : ""),
                                     new XAttribute("CodProveedor", CodProveedor.p_SolProveedor[0].CodSapProveedor != null ? CodProveedor.p_SolProveedor[0].CodSapProveedor : ""),
                                     new XAttribute("IdDireccion", idDireccion != null ? idDireccion : ""),
                                     new XAttribute("Pais", CodProveedor.p_SolProvDireccion[0].Pais != null ? CodProveedor.p_SolProvDireccion[0].Pais : ""),
                                     new XAttribute("Provincia", CodProveedor.p_SolProvDireccion[0].Provincia != null ? CodProveedor.p_SolProvDireccion[0].Provincia : ""),
                                     new XAttribute("Ciudad", CodProveedor.p_SolProvDireccion[0].Ciudad != null ? CodProveedor.p_SolProvDireccion[0].Ciudad : ""),
                                     new XAttribute("CallePrincipal", CodProveedor.p_SolProvDireccion[0].CallePrincipal != null ? CodProveedor.p_SolProvDireccion[0].CallePrincipal : ""),
                                     new XAttribute("CalleSecundaria", CodProveedor.p_SolProvDireccion[0].CalleSecundaria != null ? CodProveedor.p_SolProvDireccion[0].CalleSecundaria : ""),
                                     new XAttribute("PisoEdificio", CodProveedor.p_SolProvDireccion[0].PisoEdificio != null ? CodProveedor.p_SolProvDireccion[0].PisoEdificio : ""),
                                     new XAttribute("CodPostal", CodProveedor.p_SolProvDireccion[0].CodPostal != null ? CodProveedor.p_SolProvDireccion[0].CodPostal : ""),
                                     new XAttribute("Solar", CodProveedor.p_SolProvDireccion[0].Solar != null ? CodProveedor.p_SolProvDireccion[0].Solar : ""),
                                     new XAttribute("Estado", "True"),// p["Estado"] != null ? p["Estado"] : ""),
                                     new XAttribute("ACCION", (!String.IsNullOrEmpty(idDireccion) && int.Parse(idDireccion.ToString()) > 0 ? "U" : "I"))) : null,

                CodProveedor.p_SolLineasNegocios != null ?
                from p in CodProveedor.p_SolLineasNegocios
                select new XElement("SolLineasNegocios",

            new XAttribute("Codigo", p.Codigo != null ? p.Codigo : ""),
            new XAttribute("Principal", p.Principal != null ? p.Principal : false)) : null,

                CodProveedor.p_SolDocAdjunto != null ?
                from p in CodProveedor.p_SolDocAdjunto
                select new XElement("SolDocAdjunto",

            new XAttribute("IdSolicitud", CodProveedor.p_SolProveedor[0].IdSolicitud != null ? CodProveedor.p_SolProveedor[0].IdSolicitud : ""),
            new XAttribute("CodProveedor", CodProveedor.p_SolProveedor[0].CodSapProveedor != null ? CodProveedor.p_SolProveedor[0].CodSapProveedor : ""),
            new XAttribute("IdSolDocAdjunto", p.IdSolDocAdjunto != null ? p.IdSolDocAdjunto : ""),
            new XAttribute("CodDocumento", p.CodDocumento != null ? p.CodDocumento : ""),
            new XAttribute("NomArchivo", p.NomArchivo != null ? p.NomArchivo : ""),
            new XAttribute("Archivo", p.Archivo != null ? p.Archivo : ""),
            new XAttribute("Estado", p.Estado != null ? p.Estado.ToString() : "False"),
            new XAttribute("ACCION", (!String.IsNullOrEmpty(p.IdSolDocAdjunto) && int.Parse(p.IdSolDocAdjunto.ToString()) > 0 ? "U" : "I"))) : null,

                new XElement("SolProveedorDetalle",
                    new XAttribute("IdSolicitud", CodProveedor.p_SolProveedor[0].IdSolicitud != null ? CodProveedor.p_SolProveedor[0].IdSolicitud : ""),
                    new XAttribute("CodProveedor", CodProveedor.p_SolProveedor[0].CodSapProveedor != null ? CodProveedor.p_SolProveedor[0].CodSapProveedor : ""),
                    new XAttribute("EsCritico", CodProveedor.p_SolProveedor[0].EsCritico != null ? CodProveedor.p_SolProveedor[0].EsCritico : "N"),
                    new XAttribute("ProcesoBrindaSoporte", CodProveedor.p_SolProveedor[0].ProcesoBrindaSoporte != null ? CodProveedor.p_SolProveedor[0].ProcesoBrindaSoporte : ""),
                    new XAttribute("Sgs", CodProveedor.p_SolProveedor[0].Sgs != null ? CodProveedor.p_SolProveedor[0].Sgs : "NO"),
                    new XAttribute("TipoCalificacion", CodProveedor.p_SolProveedor[0].TipoCalificacion != null ? CodProveedor.p_SolProveedor[0].TipoCalificacion : ""),
                    new XAttribute("Calificacion", CodProveedor.p_SolProveedor[0].Calificacion != null ? CodProveedor.p_SolProveedor[0].Calificacion : ""),
                    new XAttribute("FecTermCalificacion", CodProveedor.p_SolProveedor[0].FecTermCalificacion.ToString() != null ? CodProveedor.p_SolProveedor[0].FecTermCalificacion.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd")),
                    new XAttribute("FechaCreacion", CodProveedor.p_SolProveedor[0].FechaCreacion.ToString() != null ? CodProveedor.p_SolProveedor[0].FechaCreacion.ToString("yyyy/MM/dd") : ""),
                    new XAttribute("CodActividadEconomica", CodProveedor.p_SolProveedor[0].TipoActividad != null ? CodProveedor.p_SolProveedor[0].TipoActividad : ""),
                    new XAttribute("TipoServicio", CodProveedor.p_SolProveedor[0].TipoServicio != null ? CodProveedor.p_SolProveedor[0].TipoServicio : ""),
                    new XAttribute("Relacion", CodProveedor.p_SolProveedor[0].Relacion != null ? CodProveedor.p_SolProveedor[0].Relacion.ToString() : "0"),
                    new XAttribute("IdentificacionR", CodProveedor.p_SolProveedor[0].IdentificacionR != null ? CodProveedor.p_SolProveedor[0].IdentificacionR : ""),
                    new XAttribute("NomCompletosR", CodProveedor.p_SolProveedor[0].NomCompletosR != null ? CodProveedor.p_SolProveedor[0].NomCompletosR.ToUpper() : ""),
                    new XAttribute("AreaLaboraR", CodProveedor.p_SolProveedor[0].AreaLaboraR != null ? CodProveedor.p_SolProveedor[0].AreaLaboraR.ToUpper() : ""),
                    new XAttribute("PersonaExpuesta", CodProveedor.p_SolProveedor[0].PersonaExpuesta != null ? CodProveedor.p_SolProveedor[0].PersonaExpuesta.ToString() : "0"),
                    new XAttribute("EleccionPopular", CodProveedor.p_SolProveedor[0].EleccionPopular != null ? CodProveedor.p_SolProveedor[0].EleccionPopular.ToString() : "0"),
                    new XAttribute("ACCION", (!String.IsNullOrEmpty(CodProveedor.p_SolProveedor[0].CodSapProveedor) && int.Parse(CodProveedor.p_SolProveedor[0].CodSapProveedor) > 0 ? "U" : "I"))
                    ),

                    new XElement("SolProveedor",
                                    new XAttribute("IdEmpresa", "1"),
                                     new XAttribute("TipoSolicitud", CodProveedor.p_SolProveedor[0].TipoSolicitud != null ? CodProveedor.p_SolProveedor[0].TipoSolicitud : "NU"),
                                     new XAttribute("IdSolicitud", CodProveedor.p_SolProveedor[0].IdSolicitud != null ? CodProveedor.p_SolProveedor[0].IdSolicitud : ""),
                                     new XAttribute("TipoProveedor", CodProveedor.p_SolProveedor[0].TipoProveedor != null ? CodProveedor.p_SolProveedor[0].TipoProveedor : ""),
                                     new XAttribute("CodSapProveedor", CodProveedor.p_SolProveedor[0].CodSapProveedor != null ? CodProveedor.p_SolProveedor[0].CodSapProveedor : ""),
                                     new XAttribute("TipoIdentificacion", CodProveedor.p_SolProveedor[0].TipoIdentificacion != null ? CodProveedor.p_SolProveedor[0].TipoIdentificacion : ""),
                                     new XAttribute("Identificacion", CodProveedor.p_SolProveedor[0].Identificacion != null ? CodProveedor.p_SolProveedor[0].Identificacion : ""),
                                     new XAttribute("NomComercial", CodProveedor.p_SolProveedor[0].NomComercial != null ? CodProveedor.p_SolProveedor[0].NomComercial : ""),
                                     new XAttribute("RazonSocial", CodProveedor.p_SolProveedor[0].RazonSocial != null ? CodProveedor.p_SolProveedor[0].RazonSocial : ""),
                                     new XAttribute("FechaSRI", CodProveedor.p_SolProveedor[0].FechaSRI != null && CodProveedor.p_SolProveedor[0].FechaSRI != "01/01/0001" ? CodProveedor.p_SolProveedor[0].FechaSRI : ""),
                                     new XAttribute("SectorComercial", CodProveedor.p_SolProveedor[0].SectorComercial != null ? CodProveedor.p_SolProveedor[0].SectorComercial : ""),
                                     new XAttribute("Idioma", CodProveedor.p_SolProveedor[0].Idioma != null ? CodProveedor.p_SolProveedor[0].Idioma : ""),
                                     new XAttribute("ClaseContribuyente", CodProveedor.p_SolProveedor[0].ClaseContribuyente != null ? CodProveedor.p_SolProveedor[0].ClaseContribuyente : ""),
                                     new XAttribute("GenDocElec", CodProveedor.p_SolProveedor[0].GenDocElec),
                                     new XAttribute("Estado", CodProveedor.p_SolProveedor[0].Estado != null ? CodProveedor.p_SolProveedor[0].Estado : ""),
                                     new XAttribute("GrupoTesoreria", CodProveedor.p_SolProveedor[0].GrupoTesoreria != null ? CodProveedor.p_SolProveedor[0].GrupoTesoreria : ""),
                                     new XAttribute("CuentaAsociada", CodProveedor.p_SolProveedor[0].CuentaAsociada != null ? CodProveedor.p_SolProveedor[0].CuentaAsociada : ""),
                                     new XAttribute("Autorizacion", CodProveedor.p_SolProveedor[0].Autorizacion != null ? CodProveedor.p_SolProveedor[0].Autorizacion : "M"),
                                     new XAttribute("TransfArticuProvAnterior", CodProveedor.p_SolProveedor[0].TransfArticuProvAnterior != null ? CodProveedor.p_SolProveedor[0].TransfArticuProvAnterior : ""),
                                     new XAttribute("DepSolicitando", CodProveedor.p_SolProveedor[0].DepSolicitando != null ? CodProveedor.p_SolProveedor[0].DepSolicitando : ""),
                                     new XAttribute("Responsable", CodProveedor.p_SolProveedor[0].Responsable != null ? CodProveedor.p_SolProveedor[0].Responsable : ""),
                                     new XAttribute("Aprobacion", CodProveedor.p_SolProveedor[0].Aprobacion != null ? CodProveedor.p_SolProveedor[0].Aprobacion : ""),
                                     new XAttribute("Comentario", CodProveedor.p_SolProveedor[0].Comentario != null ? CodProveedor.p_SolProveedor[0].Comentario : ""),
                                     new XAttribute("princliente", CodProveedor.p_SolProveedor[0].princliente != null ? CodProveedor.p_SolProveedor[0].princliente : ""),
                                     new XAttribute("totalventas", CodProveedor.p_SolProveedor[0].totalventas != null ? CodProveedor.p_SolProveedor[0].totalventas : ""),
                                     new XAttribute("AnioConsti", CodProveedor.p_SolProveedor[0].AnioConsti != null ? CodProveedor.p_SolProveedor[0].AnioConsti : ""),
                                     new XAttribute("LineaNegocio", CodProveedor.p_SolProveedor[0].LineaNegocio != null ? CodProveedor.p_SolProveedor[0].LineaNegocio : ""),
                                     new XAttribute("DescLineaNegocio", CodProveedor.p_SolProveedor[0].DescLineaNegocio != null ? CodProveedor.p_SolProveedor[0].DescLineaNegocio : ""),
                                     new XAttribute("PlazoEntrega", CodProveedor.p_SolProveedor[0].PlazoEntrega != null ? CodProveedor.p_SolProveedor[0].PlazoEntrega : ""),
                                     new XAttribute("DespachaProvincia", CodProveedor.p_SolProveedor[0].DespachaProvincia != null ? CodProveedor.p_SolProveedor[0].DespachaProvincia : ""),
                                     new XAttribute("GrupoCuenta", CodProveedor.p_SolProveedor[0].GrupoCuenta != null ? CodProveedor.p_SolProveedor[0].GrupoCuenta : ""),
                                     new XAttribute("RetencionIva", CodProveedor.p_SolProveedor[0].RetencionIva != null ? CodProveedor.p_SolProveedor[0].RetencionIva : ""),
                                     new XAttribute("RetencionIva2", CodProveedor.p_SolProveedor[0].RetencionIva2 != null ? CodProveedor.p_SolProveedor[0].RetencionIva2 : ""),
                                     new XAttribute("RetencionFuente", CodProveedor.p_SolProveedor[0].RetencionFuente != null ? CodProveedor.p_SolProveedor[0].RetencionFuente : ""),
                                     new XAttribute("RetencionFuente2", CodProveedor.p_SolProveedor[0].RetencionFuente2 != null ? CodProveedor.p_SolProveedor[0].RetencionFuente2 : ""),
                                     new XAttribute("CondicionPago", CodProveedor.p_SolProveedor[0].CondicionPago != null ? CodProveedor.p_SolProveedor[0].CondicionPago : ""),
                                     new XAttribute("GrupoCompra", CodProveedor.p_SolProveedor[0].GrupoCompra != null ? CodProveedor.p_SolProveedor[0].GrupoCompra : ""),
                                     new XAttribute("GrupoEsquema", CodProveedor.p_SolProveedor[0].GrupoEsquema != null ? CodProveedor.p_SolProveedor[0].GrupoEsquema : ""),
                                     new XAttribute("Ramo", CodProveedor.p_SolProveedor[0].Ramo != null ? CodProveedor.p_SolProveedor[0].Ramo : ""),
                                     new XAttribute("CodGrupoProveedor", CodProveedor.p_SolProveedor[0].CodGrupoProveedor != null ? CodProveedor.p_SolProveedor[0].CodGrupoProveedor : ""),
                                     new XAttribute("TelfFijo", CodProveedor.p_SolProveedor[0].TelfFijo != null ? CodProveedor.p_SolProveedor[0].TelfFijo : ""),
                                     new XAttribute("TelfFijoEXT", CodProveedor.p_SolProveedor[0].TelfFijoEXT != null ? CodProveedor.p_SolProveedor[0].TelfFijoEXT : ""),
                                      new XAttribute("EMAILCorp", CodProveedor.p_SolProveedor[0].EMAILCorp != null ? CodProveedor.p_SolProveedor[0].EMAILCorp : ""),                                                                    
                                    new XAttribute("ACCION", (!String.IsNullOrEmpty(CodProveedor.p_SolProveedor[0].CodSapProveedor) && int.Parse(CodProveedor.p_SolProveedor[0].CodSapProveedor) > 0 ? "U" : "I")))));

                //Grabar en SAP
                var resultado = "";
                //if (estado == "AP")
                //    resultado = enviasolitudbapi(CodProveedor);
                //return CodSapProveedor; //borrar
                if (resultado == "")
                {
                    p_Log.Graba_Log_Info(MetodoController + " -- GetActualizaProveedor: REQUEST " + wresulFactList.ToString());
                    ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 107, 1);
                }
                else
                {
                    return resultado;
                }

                String Wobservacion = "";
                String WMotivo = "";
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    try
                    {

                        if (CodProveedor.p_SolProvHistEstado != null)
                        {
                            var x = CodProveedor.p_SolProvHistEstado.LastOrDefault();
                            if (x != null)
                            {
                                Wobservacion = String.IsNullOrEmpty(x.Observacion) ? "" : x.Observacion.ToString().ToUpper();
                                WMotivo = String.IsNullOrEmpty(x.DesMotivo) ? "" : x.DesMotivo.ToString().ToUpper();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CodSapProveedor = "Error-9999: " + ex.Message;
                    }
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                CodSapProveedor = "Error-9999: " + ex.Message;
            }

            return CodSapProveedor;
        }

        [ActionName("ConsultaSolProvBanco")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProvBanco> GetSolProvBanco(string CodProveedor)
        {
            IEnumerable<DMSolcitudProveedor.SolProvBanco> Retorno = new List<DMSolcitudProveedor.SolProvBanco>();

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {                
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);
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
                                   IdSolBanco = reg["IdProveedorPago"] != DBNull.Value ? reg["IdProveedorPago"].ToString() : "",
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

    }
}
