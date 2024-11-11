using AngularJSAuthentication.API.Models;

using clibGeneraContratos;
using clibProveedores;

using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.Http;
using System.Xml;
using DocXToPdfConverter;
using System.Net;
using System.Net.Security;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/GeneraContrato")]
    public class GeneraContratoController : ApiController
    {
        private string _clase;

        public GeneraContratoController()
        {
            _clase = GetType().Name;
        }

        [ActionName("ConsultaLicitaciones")]
        [Route("ConsultaLicitaciones")]
        [HttpGet]
        public FormResponseModelo ConsultaLicitaciones(string rolesAplicacion)
        {
            string _horaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _metodo = MethodBase.GetCurrentMethod().Name;
            _metodo += " " + _horaEjecucion;

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            FormResponseModelo _oRetornoConsultaLicitaciones = new FormResponseModelo();

            try
            {
                XmlDocument xmlParamConsultaLicitaciones = new XmlDocument();
                xmlParamConsultaLicitaciones.LoadXml("<Root />");
                xmlParamConsultaLicitaciones.DocumentElement.SetAttribute("tipo", "1");
                var roles = rolesAplicacion.Split('|');

                foreach (var rol in roles)
                {
                    if (rol != "")
                    {
                        XmlElement elem = xmlParamConsultaLicitaciones.CreateElement("Rol");
                        elem.SetAttribute("nombre", rol);
                        xmlParamConsultaLicitaciones.DocumentElement.AppendChild(elem);
                    }
                }

                ClsGeneral objEjecucionConsultaLicitaciones = new ClsGeneral();
                DataSet dsConsultaLicitaciones = objEjecucionConsultaLicitaciones.EjecucionGralDs(xmlParamConsultaLicitaciones.OuterXml, 1104, 1);

                if (dsConsultaLicitaciones.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Licitacion lici;
                    List<Licitacion> lstLicitaciones = new List<Licitacion>();

                    foreach (DataRow item in dsConsultaLicitaciones.Tables[0].Rows)
                    {
                        lici = new Licitacion
                        {
                            IdAdquisicion = item["IdAdquisicion"].ToString(),
                            Nombre = item["NombreLic"].ToString(),
                            Descripcion = item["DescripcionLic"].ToString(),
                            Ruc = item["Ruc"].ToString(),
                            RazonSocial = item["RazonSocial"].ToString(),
                            NumContrato = item["NumContrato"].ToString(),
                            IdEstado = item["IdEstado"].ToString(),
                            DesEstado = item["DesEstado"].ToString(),
                            NombreArchivo = item["NombreArchivo"].ToString()
                        };

                        lstLicitaciones.Add(lici);
                    }

                    _oRetornoConsultaLicitaciones.lSuccess = true;
                    _oRetornoConsultaLicitaciones.cCodError = "0";
                    _oRetornoConsultaLicitaciones.root.Add(lstLicitaciones);
                }
                else
                {
                    _oRetornoConsultaLicitaciones.lSuccess = false;
                    _oRetornoConsultaLicitaciones.cCodError = dsConsultaLicitaciones.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    _oRetornoConsultaLicitaciones.cMsgError = dsConsultaLicitaciones.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + _oRetornoConsultaLicitaciones.cCodError + " - " + _oRetornoConsultaLicitaciones.cMsgError);
                }
            }
            catch (Exception ex)
            {
                _oRetornoConsultaLicitaciones.lSuccess = false;
                _oRetornoConsultaLicitaciones.cCodError = "9999";
                _oRetornoConsultaLicitaciones.cMsgError = ex.Message.ToString();
                p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            }

            return _oRetornoConsultaLicitaciones;
        }

        [ActionName("ConsultaLicitacionesProv")]
        [Route("ConsultaLicitacionesProv")]
        [HttpGet]
        public FormResponseModelo ConsultaLicitacionesProv(string ruc)
        {
            string _horaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _metodo = MethodBase.GetCurrentMethod().Name;
            _metodo += " " + _horaEjecucion;

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            FormResponseModelo _oRetornoConsultaLicitaciones = new FormResponseModelo();

            try
            {
                XmlDocument xmlParamConsultaLicitaciones = new XmlDocument();
                xmlParamConsultaLicitaciones.LoadXml("<Root />");
                xmlParamConsultaLicitaciones.DocumentElement.SetAttribute("tipo", "2");
                xmlParamConsultaLicitaciones.DocumentElement.SetAttribute("usuario", ruc);

                ClsGeneral objEjecucionConsultaLicitaciones = new ClsGeneral();
                DataSet dsConsultaLicitaciones = objEjecucionConsultaLicitaciones.EjecucionGralDs(xmlParamConsultaLicitaciones.OuterXml, 1104, 1);

                if (dsConsultaLicitaciones.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Licitacion lici;
                    List<Licitacion> lstLicitaciones = new List<Licitacion>();

                    foreach (DataRow item in dsConsultaLicitaciones.Tables[0].Rows)
                    {
                        lici = new Licitacion
                        {
                            IdAdquisicion = item["IdAdquisicion"].ToString(),
                            Nombre = item["NombreLic"].ToString(),
                            Descripcion = item["DescripcionLic"].ToString(),
                            Ruc = item["Ruc"].ToString(),
                            RazonSocial = item["RazonSocial"].ToString(),
                            NumContrato = item["NumContrato"].ToString(),
                            IdEstado = item["IdEstado"].ToString(),
                            DesEstado = item["DesEstado"].ToString(),
                            NombreArchivo = item["NombreArchivo"].ToString()
                        };

                        lstLicitaciones.Add(lici);
                    }

                    _oRetornoConsultaLicitaciones.lSuccess = true;
                    _oRetornoConsultaLicitaciones.cCodError = "0";
                    _oRetornoConsultaLicitaciones.root.Add(lstLicitaciones);
                }
                else
                {
                    _oRetornoConsultaLicitaciones.lSuccess = false;
                    _oRetornoConsultaLicitaciones.cCodError = dsConsultaLicitaciones.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    _oRetornoConsultaLicitaciones.cMsgError = dsConsultaLicitaciones.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + _oRetornoConsultaLicitaciones.cCodError + " - " + _oRetornoConsultaLicitaciones.cMsgError);
                }
            }
            catch (Exception ex)
            {
                _oRetornoConsultaLicitaciones.lSuccess = false;
                _oRetornoConsultaLicitaciones.cCodError = "9999";
                _oRetornoConsultaLicitaciones.cMsgError = ex.Message.ToString();
                p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            }

            return _oRetornoConsultaLicitaciones;
        }

        [ActionName("ConsultaDetLicitacion")]
        [Route("ConsultaDetLicitacion")]
        [HttpGet]
        public FormResponseModelo ConsultaDetLicitacion(string idAdquisicion)
        {
            string _horaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _metodo = MethodBase.GetCurrentMethod().Name;
            _metodo += " " + _horaEjecucion;

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            FormResponseModelo _oRetornoConsultaDetLicitacion = new FormResponseModelo();

            try
            {
                XmlDocument xmlParamConsultaDetLicitacion = new XmlDocument();
                xmlParamConsultaDetLicitacion.LoadXml("<Root />");
                xmlParamConsultaDetLicitacion.DocumentElement.SetAttribute("tipo", "3");
                xmlParamConsultaDetLicitacion.DocumentElement.SetAttribute("idAdquisicion", idAdquisicion);

                ClsGeneral objEjecucion = new ClsGeneral();
                DataSet dsConsultaDetLicitacion = objEjecucion.EjecucionGralDs(xmlParamConsultaDetLicitacion.OuterXml, 1104, 1);

                if (dsConsultaDetLicitacion.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    Sociedad soc = new Sociedad();

                    foreach (DataRow item in dsConsultaDetLicitacion.Tables[0].Rows)
                    {
                        soc = new Sociedad
                        {
                            Nombre = item["NombreSociedad"].ToString(),
                            Ruc = item["RucSociedad"].ToString(),
                            RepresentanteLegal = item["RepresentanteLegal"].ToString(),
                            DesActividadEconomica = item["DesActividadEconomica"].ToString(),
                            Direccion = item["Direccion"].ToString(),
                            Locacion = item["Locacion"].ToString(),
                            Correo = item["Correo"].ToString(),
                            Telefono = item["Telefono"].ToString()
                        };
                    }

                    LicitacionDet liciDet = new LicitacionDet();

                    foreach (DataRow item in dsConsultaDetLicitacion.Tables[1].Rows)
                    {
                        liciDet = new LicitacionDet
                        {
                            Nombre = item["NombreLic"].ToString(),
                            Sociedad = item["Sociedad"].ToString(),
                            Ruc = item["Ruc"].ToString(),
                            IdentificacionRepLegal = item["IdentificacionRepLegal"].ToString(),
                            RepresentanteLegal = item["RepLegal"].ToString(),
                            Cargo = item["Cargo"].ToString(),
                            DesPersonalidad = item["DesPersonalidad"].ToString(),
                            DesActividadEconomica = item["DesActividadEconomica"].ToString(),
                            Direccion = item["Direccion"].ToString(),
                            ValorContrato = item["ValorContrato"].ToString(),
                            Correo = item["Correo"].ToString(),
                            Locacion = item["Locacion"].ToString(),
                            DesPais = item["DesPais"].ToString(),
                            Telefono = item["Telefono"].ToString(),
                            Extension = item["Extension"].ToString(),
                            IdContrato = item["IdContrato"].ToString(),
                            CodTipoContrato = item["CodTipoContrato"].ToString(),
                            CodLineaNegocio = item["CodLineaNegocio"].ToString(),
                            CodTipoServicio = item["CodTipoServicio"].ToString(),
                            CodPlazoSuscripcion = item["CodPlazoSuscripcion"].ToString(),
                            AdminBG = item["AdminBG"].ToString(),
                            CorreoAdminBG = item["CorreoAdminBG"].ToString(),
                            AdministradorContrato = item["AdministradorContrato"].ToString(),
                            CorreoAdminPrv = item["CorreoAdminPrv"].ToString(),
                            MotivoActividadComentario = item["MotivoActividadComentario"].ToString(),
                            FormaPagoComentario = item["FormaPagoComentario"].ToString(),
                            FormaPagoTipo = item["FormaPagoTipo"].ToString(),
                            NumeroCuenta = item["NumeroCuenta"].ToString()
                        };
                    }

                    _oRetornoConsultaDetLicitacion.lSuccess = true;
                    _oRetornoConsultaDetLicitacion.cCodError = "0";
                    _oRetornoConsultaDetLicitacion.root.Add(soc);
                    _oRetornoConsultaDetLicitacion.root.Add(liciDet);
                }
                else
                {
                    _oRetornoConsultaDetLicitacion.lSuccess = false;
                    _oRetornoConsultaDetLicitacion.cCodError = dsConsultaDetLicitacion.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    _oRetornoConsultaDetLicitacion.cMsgError = dsConsultaDetLicitacion.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + _oRetornoConsultaDetLicitacion.cCodError + " - " + _oRetornoConsultaDetLicitacion.cMsgError);
                }
            }
            catch (Exception ex)
            {
                _oRetornoConsultaDetLicitacion.lSuccess = false;
                _oRetornoConsultaDetLicitacion.cCodError = "9999";
                _oRetornoConsultaDetLicitacion.cMsgError = ex.Message.ToString();
                p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            }

            return _oRetornoConsultaDetLicitacion;
        }

        [ActionName("ActualizaInfoContrato")]
        [Route("ActualizaInfoContrato")]
        [HttpPost]
        public FormResponseModelo ActualizaInfoContrato(ContratoLicitacion contrato)
        {
            string _horaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _metodo = MethodBase.GetCurrentMethod().Name;
            _metodo += " " + _horaEjecucion;

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            FormResponseModelo _oRetornoActualizaInfoContrato = new FormResponseModelo();

            try
            {
                XmlDocument xmlParamActualizaInfoContrato = new XmlDocument();
                xmlParamActualizaInfoContrato.LoadXml("<Root />");
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("tipo", "4");
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("idContrato", contrato.IdContrato);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("idAdquisicion", contrato.IdAdquisicion);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("nombreLicitacion", contrato.NombreLicitacion);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("codTipoContrato", contrato.CodTipoContrato);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("codLineaNegocio", contrato.CodLineaNegocio);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("codTipoServicio", contrato.CodTipoServicio);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("codPlazoSuscripcion", contrato.CodPlazoSuscripcion);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("adminBG", contrato.AdminBG);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("correoAdminBG", contrato.CorreoAdminBG);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("administradorContrato", contrato.AdministradorContrato);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("correoAdminPrv", contrato.CorreoAdminPrv);
                xmlParamActualizaInfoContrato.DocumentElement.SetAttribute("usuario", contrato.Usuario);

                ClsGeneral objEjecucion = new ClsGeneral();
                DataSet dsActualizaInfoContrato = new DataSet();
                dsActualizaInfoContrato = objEjecucion.EjecucionGralDs(xmlParamActualizaInfoContrato.OuterXml, 1104, 1);

                if (dsActualizaInfoContrato.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    _oRetornoActualizaInfoContrato.lSuccess = true;
                    _oRetornoActualizaInfoContrato.cCodError = "0";
                    _oRetornoActualizaInfoContrato.root.Add(dsActualizaInfoContrato.Tables[0].Rows[0]["IdContrato"].ToString());
                }
                else
                {
                    _oRetornoActualizaInfoContrato.lSuccess = false;
                    _oRetornoActualizaInfoContrato.cCodError = dsActualizaInfoContrato.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    _oRetornoActualizaInfoContrato.cMsgError = dsActualizaInfoContrato.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + _oRetornoActualizaInfoContrato.cCodError + " - " + _oRetornoActualizaInfoContrato.cMsgError);
                }
            }
            catch (Exception ex)
            {
                _oRetornoActualizaInfoContrato.lSuccess = false;
                _oRetornoActualizaInfoContrato.cCodError = "9999";
                _oRetornoActualizaInfoContrato.cMsgError = ex.Message.ToString();
                p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            }

            return _oRetornoActualizaInfoContrato;
        }

        [ActionName("GenerarContrato")]
        [Route("GenerarContrato")]
        [HttpPost]
        public FormResponseModelo GenerarContrato(ContratoPlantilla objPlantilla)
        {
            string _horaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _metodo = MethodBase.GetCurrentMethod().Name;
            _metodo += " " + _horaEjecucion;

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            FormResponseModelo _oRetornoGenerarContrato = new FormResponseModelo();

            string id = DateTime.Now.ToString("yyyyMMddHHmmss");
            string rutaPlantillas = ConfigurationManager.AppSettings["RutaPlantillaContratos"];
            string rutaBaseTmpGenerados = ConfigurationManager.AppSettings["RutaContratosTmpGenerados"];
            string rutaBaseGenerados = ConfigurationManager.AppSettings["RutaContratosGenerados"];
            string rutaArchivo = ConfigurationManager.AppSettings["RutaArchivo"];

            string nombrePlantilla = "";
            string rutaCompletaTmp = Path.Combine(rutaBaseTmpGenerados, id);

            try
            {
                objPlantilla.EMPRESA = objPlantilla.RAZON_SOCIAL;
                objPlantilla.TIPO_SERVICIO_DETALLE = objPlantilla.TIPO_SERVICIO_DETALLE.ToUpper();
                objPlantilla.TIPO_SERVICIOS_COMENTARIO = objPlantilla.TIPO_SERVICIOS_COMENTARIO.Substring(3);
                objPlantilla.FECHA_HOY_LETRAS = Letras(DateTime.Now);

                if (objPlantilla.CodTipoContrato.Equals("CONTIPBIEN"))
                {
                    nombrePlantilla = ConfigurationManager.AppSettings["PlantillaContratoProveedor"];
                }
                else if (objPlantilla.CodTipoContrato.Equals("CONTIPSERV"))
                {
                    nombrePlantilla = ConfigurationManager.AppSettings["PlantillaContratosCompra"];
                }
                else
                {
                    p_Log.Graba_Log_Warn("No existe plantilla para el tipo contrato seleccionado.");
                    throw new Exception("No existe plantilla para el tipo contrato seleccionado.");
                }

                string rutaFullNameTmp = Path.Combine(rutaCompletaTmp, nombrePlantilla);

                if (Directory.Exists(rutaCompletaTmp))
                {
                    Directory.Delete(rutaCompletaTmp, true);
                }
                Directory.CreateDirectory(rutaCompletaTmp);

                File.Copy(Path.Combine(rutaPlantillas, nombrePlantilla), rutaFullNameTmp);

                var dtCampos = GenerateTransposedTable(objPlantilla);
                
                GeneraContratos GenContratos = new GeneraContratos();
                byte[] ResContrato = GenContratos.writeToWordDocx(dtCampos, rutaFullNameTmp);

                if (File.Exists(rutaFullNameTmp)) { p_Log.Graba_Log_Info(_clase + " " + _metodo + " *** Archivo Docx Creado  " + File.GetCreationTime(rutaFullNameTmp) + " ***"); }

                if (ResContrato.Length == 0)
                {
                    _oRetornoGenerarContrato.lSuccess = false;
                    _oRetornoGenerarContrato.cCodError = "9999";
                    _oRetornoGenerarContrato.cMsgError = "Error al reemplazar parametros de contrato";
                    p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : Error al reemplazar parametros de contrato");

                    return _oRetornoGenerarContrato;
                }

                string FileExtension = Path.GetExtension(nombrePlantilla);
                string ChangeExtension = nombrePlantilla.Replace(FileExtension, ".pdf");
                string locationOfLibreOfficeSoffice = ConfigurationManager.AppSettings["RutaLibOffice"];
                string docxLocation = rutaCompletaTmp + "\\" + nombrePlantilla;
                
                var test = new ReportGenerator(locationOfLibreOfficeSoffice);

                test.Convert(docxLocation, Path.Combine(Path.GetDirectoryName(docxLocation), ChangeExtension), null);

                if (File.Exists(rutaFullNameTmp)) { p_Log.Graba_Log_Info(_clase + " " + _metodo + " *** Archivo Pdf Creado  " + File.GetCreationTime(rutaFullNameTmp.Replace("docx", "pdf")) + " ***"); }

                //5.-Actualiza estado de contrato en tabla y registra log
                XmlDocument xmlParamGeneraContrato = new XmlDocument();
                xmlParamGeneraContrato.LoadXml("<Root />");
                xmlParamGeneraContrato.DocumentElement.SetAttribute("tipo", "5");
                xmlParamGeneraContrato.DocumentElement.SetAttribute("idContrato", objPlantilla.Idcontrato);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("idEstado", "2");  //Generado
                xmlParamGeneraContrato.DocumentElement.SetAttribute("idAdquisicion", objPlantilla.IdAdquisicion);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("nombreLicitacion", objPlantilla.NombreLicitacion);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("codTipoContrato", objPlantilla.CodTipoContrato);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("codLineaNegocio", objPlantilla.CodLineaNegocio);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("codTipoServicio", objPlantilla.CodTipoServicio);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("codPlazoSuscripcion", objPlantilla.CodPlazoSuscripcion);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("adminBG", objPlantilla.AdminBG);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("correoAdminBG", objPlantilla.CorreoAdminBG);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("administradorContrato", objPlantilla.AdministradorContrato);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("correoAdminPrv", objPlantilla.CorreoAdminPrv);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("usuario", objPlantilla.Usuario);

                ClsGeneral objEjecucion = new ClsGeneral();
                DataSet dsGeneraContrato = objEjecucion.EjecucionGralDs(xmlParamGeneraContrato.OuterXml, 1104, 1);

                string nombreArchivo = "";

                if (dsGeneraContrato.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    nombreArchivo = dsGeneraContrato.Tables[0].Rows[0]["NombreArchivo"].ToString();
                    _oRetornoGenerarContrato.lSuccess = true;
                    _oRetornoGenerarContrato.cCodError = "0";

                    File.Move(rutaCompletaTmp + "\\" + ChangeExtension, rutaCompletaTmp + "\\" + nombreArchivo);

                    p_Log.Graba_Log_Info("Archivo Final: " + rutaCompletaTmp + "\\" + nombreArchivo);
                }
                else
                {
                    _oRetornoGenerarContrato.lSuccess = false;
                    _oRetornoGenerarContrato.cCodError = dsGeneraContrato.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    _oRetornoGenerarContrato.cMsgError = dsGeneraContrato.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + _oRetornoGenerarContrato.cCodError + " - " + _oRetornoGenerarContrato.cMsgError);
                }

                //6.-Con el número de contrato creo la carpeta en el servidor y copio el PDF
                //POR SIMULACION SE COPIA EL MISMO WORD GENERADO, PERO SE DEBERA GENERAR UN PDF Y COLOCARLO EN LA RUTA
                File.Copy(Path.Combine(rutaCompletaTmp, nombreArchivo), Path.Combine(rutaBaseGenerados, nombreArchivo), true);

                p_Log.Graba_Log_Info("Ruta Server: " + Path.Combine(rutaArchivo, "PDF\\Contratos\\") + nombreArchivo);

                bool folderExists = Directory.Exists(Path.Combine(rutaArchivo, "PDF\\Contratos\\"));
                if (!folderExists)
                    Directory.CreateDirectory(Path.Combine(rutaArchivo, "PDF\\Contratos\\"));


                File.Copy(Path.Combine(rutaCompletaTmp, nombreArchivo), Path.Combine(rutaArchivo, "PDF\\Contratos\\" + nombreArchivo), true);
                Directory.Delete(rutaCompletaTmp, true);

                _oRetornoGenerarContrato.root.Add(rutaArchivo);
                _oRetornoGenerarContrato.root.Add(nombreArchivo);

                p_Log.Graba_Log_Info("Archivo generado en la ruta: " + Path.Combine(rutaBaseGenerados, nombreArchivo));

                //7.-Envia notificacion
                try
                {
                    string asuntoEmail = "Generacion de Contrato - " + nombreArchivo;
                    string correo = objPlantilla.CORREO;
                    string archivo = "";

                    String PI_NombrePlantilla = string.Empty;

                    archivo = rutaArchivo + "PDF\\Contratos\\" + nombreArchivo;
                    
                    PI_NombrePlantilla = "NotificacionContratos.html";
                    AngularJSAuthentication.API.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new AngularJSAuthentication.API.WCFEnvioCorreo.ServEnvioClientSoapClient();
                    
                    p_Log.Graba_Log_Info("ARCH: " + archivo + " - EMAIL: " + correo + " - ASUNT: " + asuntoEmail);

                    if (archivo == "")
                        p_Log.Graba_Log_Error("No existe archivo para adjuntar en la notificación de contrato");
                    else
                    {
                        objEnvMail.EnviaCorreoApi("", correo, "", "", asuntoEmail, "", true, true, true, File.ReadAllBytes(archivo), nombreArchivo, PI_NombrePlantilla, "");
                    }

                }
                catch (Exception ex)
                {
                    p_Log.Graba_Log_Error("Error en [EnviaNotificaciones] -> " + ex.Message.ToString());
                }

                _oRetornoGenerarContrato.lSuccess = true;
                _oRetornoGenerarContrato.cCodError = "0";

            }
            catch (Exception ex)
            {
                if (Directory.Exists(rutaCompletaTmp))
                {
                    Directory.Delete(rutaCompletaTmp, true);
                }

                _oRetornoGenerarContrato.lSuccess = false;
                _oRetornoGenerarContrato.cCodError = "9999";
                _oRetornoGenerarContrato.cMsgError = ex.Message.ToString();
                p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            }

            return _oRetornoGenerarContrato;
        }


        [ActionName("GenerarVistaPreviaContrato")]
        [Route("GenerarVistaPreviaContrato")]
        [HttpPost]
        public FormResponseModelo GenerarVistaPreviaContrato(ContratoPlantilla objPlantilla)
        {
            string _horaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _metodo = MethodBase.GetCurrentMethod().Name;
            _metodo += " " + _horaEjecucion;

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            FormResponseModelo _oRetornoVistaPreviaContrato = new FormResponseModelo();

            string id = DateTime.Now.ToString("yyyyMMddHHmmss");
            string rutaPlantillas = ConfigurationManager.AppSettings["RutaPlantillaContratos"];
            string rutaBaseTmpGenerados = ConfigurationManager.AppSettings["RutaContratosTmpGenerados"];
            string rutaBaseGenerados = ConfigurationManager.AppSettings["RutaContratosGenerados"];
            string rutaArchivo = ConfigurationManager.AppSettings["RutaArchivo"];

            string nombrePlantilla = "";
            string rutaCompletaTmp = Path.Combine(rutaBaseTmpGenerados, id);

            try
            {
                objPlantilla.EMPRESA = objPlantilla.RAZON_SOCIAL;
                objPlantilla.TIPO_SERVICIO_DETALLE = objPlantilla.TIPO_SERVICIO_DETALLE.ToUpper();
                objPlantilla.TIPO_SERVICIOS_COMENTARIO = objPlantilla.TIPO_SERVICIOS_COMENTARIO.Substring(3);
                objPlantilla.FECHA_HOY_LETRAS = Letras(DateTime.Now);

                if (objPlantilla.CodTipoContrato.Equals("CONTIPBIEN"))
                {
                    nombrePlantilla = ConfigurationManager.AppSettings["PlantillaContratoProveedor"];
                }
                else if (objPlantilla.CodTipoContrato.Equals("CONTIPSERV"))
                {
                    nombrePlantilla = ConfigurationManager.AppSettings["PlantillaContratosCompra"];
                }
                else
                {
                    p_Log.Graba_Log_Warn("No existe plantilla para el tipo contrato seleccionado.");
                    throw new Exception("No existe plantilla para el tipo contrato seleccionado.");
                }

                string rutaFullNameTmp = Path.Combine(rutaCompletaTmp, nombrePlantilla);

                if (Directory.Exists(rutaCompletaTmp))
                {
                    Directory.Delete(rutaCompletaTmp, true);
                }
                Directory.CreateDirectory(rutaCompletaTmp);

                File.Copy(Path.Combine(rutaPlantillas, nombrePlantilla), rutaFullNameTmp);

                //1.- Llena los parametros del contato docx
                var dtCampos = GenerateTransposedTable(objPlantilla);

                GeneraContratos GenContratos = new GeneraContratos();
                byte[] ResContrato = GenContratos.writeToWordDocx(dtCampos, rutaFullNameTmp);

                if (File.Exists(rutaFullNameTmp)) { p_Log.Graba_Log_Info(_clase + " " + _metodo + " *** Archivo Docx Creado  " + File.GetCreationTime(rutaFullNameTmp) + " ***"); }

                if (ResContrato.Length == 0)
                {
                    _oRetornoVistaPreviaContrato.lSuccess = false;
                    _oRetornoVistaPreviaContrato.cCodError = "9999";
                    _oRetornoVistaPreviaContrato.cMsgError = "Error al reemplazar parametros de contrato";
                    p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : Error al reemplazar parametros de contrato");

                    return _oRetornoVistaPreviaContrato;
                }

                string FileExtension = Path.GetExtension(nombrePlantilla);
                string ChangeExtension = nombrePlantilla.Replace(FileExtension, ".pdf");
                string locationOfLibreOfficeSoffice = ConfigurationManager.AppSettings["RutaLibOffice"];
                string docxLocation = rutaCompletaTmp + "\\" + nombrePlantilla;

                var test = new ReportGenerator(locationOfLibreOfficeSoffice);

                //2.- Convierte a PDF
                test.Convert(docxLocation, Path.Combine(Path.GetDirectoryName(docxLocation), ChangeExtension), null);

                if (File.Exists(rutaFullNameTmp)) { p_Log.Graba_Log_Info(_clase + " " + _metodo + " *** Archivo Pdf Creado  " + File.GetCreationTime(rutaFullNameTmp.Replace("docx", "pdf")) + " ***"); }

                //3.- Actualiza estado de contrato en tabla y registra log
                XmlDocument xmlParamGeneraContrato = new XmlDocument();
                xmlParamGeneraContrato.LoadXml("<Root />");
                xmlParamGeneraContrato.DocumentElement.SetAttribute("tipo", "5");
                xmlParamGeneraContrato.DocumentElement.SetAttribute("idContrato", objPlantilla.Idcontrato);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("idEstado", "0");  //No cambia el estado
                xmlParamGeneraContrato.DocumentElement.SetAttribute("idAdquisicion", objPlantilla.IdAdquisicion);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("nombreLicitacion", objPlantilla.NombreLicitacion);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("codTipoContrato", objPlantilla.CodTipoContrato);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("codLineaNegocio", objPlantilla.CodLineaNegocio);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("codTipoServicio", objPlantilla.CodTipoServicio);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("codPlazoSuscripcion", objPlantilla.CodPlazoSuscripcion);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("adminBG", objPlantilla.AdminBG);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("correoAdminBG", objPlantilla.CorreoAdminBG);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("administradorContrato", objPlantilla.AdministradorContrato);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("correoAdminPrv", objPlantilla.CorreoAdminPrv);
                xmlParamGeneraContrato.DocumentElement.SetAttribute("usuario", objPlantilla.Usuario);

                ClsGeneral objEjecucion = new ClsGeneral();
                DataSet dsGeneraContrato = objEjecucion.EjecucionGralDs(xmlParamGeneraContrato.OuterXml, 1104, 1);

                string nombreArchivo = "";

                if (dsGeneraContrato.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    nombreArchivo = dsGeneraContrato.Tables[0].Rows[0]["NombreArchivo"].ToString();
                    _oRetornoVistaPreviaContrato.root.Add(dsGeneraContrato.Tables[0].Rows[0]["IdContrato"].ToString()) ;
                    _oRetornoVistaPreviaContrato.lSuccess = true;
                    _oRetornoVistaPreviaContrato.cCodError = "0";

                    if (File.Exists(rutaCompletaTmp + "\\" + nombreArchivo))
                    {
                        File.Delete(rutaCompletaTmp + "\\" + nombreArchivo);
                    }

                    File.Move(rutaCompletaTmp + "\\" + ChangeExtension, rutaCompletaTmp + "\\" + nombreArchivo);

                    p_Log.Graba_Log_Info("Archivo Final: " + rutaCompletaTmp + "\\" + nombreArchivo);
                }
                else
                {
                    _oRetornoVistaPreviaContrato.lSuccess = false;
                    _oRetornoVistaPreviaContrato.cCodError = dsGeneraContrato.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    _oRetornoVistaPreviaContrato.cMsgError = dsGeneraContrato.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + _oRetornoVistaPreviaContrato.cCodError + " - " + _oRetornoVistaPreviaContrato.cMsgError);
                }

                //4.- Con el número de contrato creo la carpeta en el servidor y copio el PDF
                File.Copy(Path.Combine(rutaCompletaTmp, nombreArchivo), Path.Combine(rutaBaseGenerados, nombreArchivo), true);

                p_Log.Graba_Log_Info("Ruta Server: " + Path.Combine(rutaArchivo, "PDF\\Contratos\\") + nombreArchivo);

                bool folderExists = Directory.Exists(Path.Combine(rutaArchivo, "PDF\\Contratos\\"));
                if (!folderExists)
                    Directory.CreateDirectory(Path.Combine(rutaArchivo, "PDF\\Contratos\\"));


                File.Copy(Path.Combine(rutaCompletaTmp, nombreArchivo), Path.Combine(rutaArchivo, "PDF\\Contratos\\" + nombreArchivo), true);

                _oRetornoVistaPreviaContrato.root.Add(rutaArchivo);
                _oRetornoVistaPreviaContrato.root.Add(nombreArchivo);

                p_Log.Graba_Log_Info("Archivo generado en la ruta: " + Path.Combine(rutaBaseGenerados, nombreArchivo));

                _oRetornoVistaPreviaContrato.lSuccess = true;
                _oRetornoVistaPreviaContrato.cCodError = "0";

            }
            catch (Exception ex)
            {
                if (Directory.Exists(rutaCompletaTmp))
                {
                    Directory.Delete(rutaCompletaTmp, true);
                }

                _oRetornoVistaPreviaContrato.lSuccess = false;
                _oRetornoVistaPreviaContrato.cCodError = "9999";
                _oRetornoVistaPreviaContrato.cMsgError = ex.Message.ToString();
                p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            }

            return _oRetornoVistaPreviaContrato;
        }

        [ActionName("AprobarContrato")]
        [Route("AprobarContrato")]
        [HttpPost]
        public FormResponseModelo AprobarContrato(string idAdContrato, string usuario, string idEstado)
        {
            string _horaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _metodo = MethodBase.GetCurrentMethod().Name;
            _metodo += " " + _horaEjecucion;

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            FormResponseModelo _oRetornoAprobarContrato = new FormResponseModelo();

            try
            {
                XmlDocument xmlParamAprobarContrato = new XmlDocument();
                xmlParamAprobarContrato.LoadXml("<Root />");
                xmlParamAprobarContrato.DocumentElement.SetAttribute("tipo", "6");
                xmlParamAprobarContrato.DocumentElement.SetAttribute("idAdquisicion", idAdContrato);
                xmlParamAprobarContrato.DocumentElement.SetAttribute("usuario", usuario);
                xmlParamAprobarContrato.DocumentElement.SetAttribute("idEstado", idEstado);

                ClsGeneral objEjecucion = new ClsGeneral();
                DataSet dsAprobarContrato = objEjecucion.EjecucionGralDs(xmlParamAprobarContrato.OuterXml, 1104, 1);

                if (dsAprobarContrato.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (dsAprobarContrato.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in dsAprobarContrato.Tables[0].Rows)
                        {
                            _oRetornoAprobarContrato.lSuccess = false;
                            _oRetornoAprobarContrato.cCodError = item["CodError"].ToString();
                            _oRetornoAprobarContrato.cMsgError = item["MsgError"].ToString();
                            p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + _oRetornoAprobarContrato.cCodError + " - " + _oRetornoAprobarContrato.cMsgError);
                        }
                    }
                    else
                    {
                        _oRetornoAprobarContrato.lSuccess = true;
                        _oRetornoAprobarContrato.cCodError = "0";
                        _oRetornoAprobarContrato.root.Add("");
                        _oRetornoAprobarContrato.root.Add("");
                    }
                }
                else
                {
                    _oRetornoAprobarContrato.lSuccess = false;
                    _oRetornoAprobarContrato.cCodError = dsAprobarContrato.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    _oRetornoAprobarContrato.cMsgError = dsAprobarContrato.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + _oRetornoAprobarContrato.cCodError + " - " + _oRetornoAprobarContrato.cMsgError);
                }
            }
            catch (Exception ex)
            {
                _oRetornoAprobarContrato.lSuccess = false;
                _oRetornoAprobarContrato.cCodError = "9999";
                _oRetornoAprobarContrato.cMsgError = ex.Message.ToString();
                p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            }

            return _oRetornoAprobarContrato;
        }

        [ActionName("VerificaFirma")]
        [Route("VerificaFirma")]
        [HttpPost]
        public FormResponseModelo VerificaFirma(int idAdq, string nomArchivo, string rutaArchivo)
        {
            string _horaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _metodo = MethodBase.GetCurrentMethod().Name;
            _metodo += " " + _horaEjecucion;

            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            FormResponseModelo _oRetornoVerificaFirma = new FormResponseModelo();

            try
            {
                if (rutaArchivo == "" || rutaArchivo == null)
                {
                    rutaArchivo = ConfigurationManager.AppSettings["RutaArchivo"] + "PDF\\Contratos\\";
                }

                string[] firmasDigitales = CertificarDocumento(rutaArchivo + nomArchivo);
                _metodo = MethodBase.GetCurrentMethod().Name;
                p_Log.Graba_Log_Info("Length de la firma: " + firmasDigitales.Length.ToString());
                if ((firmasDigitales !=null) && (firmasDigitales.Length > 0))
                {
                    //Muestra los nombres de las firmas
                    _oRetornoVerificaFirma.lSuccess = true;
                    _oRetornoVerificaFirma.cCodError = "0";
                    for (int c = 0; c < firmasDigitales.Length; c++) {
                        _oRetornoVerificaFirma.root.Add((c + 1) + ": " + firmasDigitales[c]);
                    }
                }
                else
                {
                    //Archivo sin firmas
                    _oRetornoVerificaFirma.lSuccess = false;
                    _oRetornoVerificaFirma.cCodError = "9999";
                    _oRetornoVerificaFirma.cMsgError = "El Contrato no posee ninguna Firma Digital";
                    p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : El Contrato no posee ninguna Firma Digital");
                }

            }
            catch (Exception ex)
            {
                _oRetornoVerificaFirma.lSuccess = false;
                _oRetornoVerificaFirma.cCodError = "9999";
                _oRetornoVerificaFirma.cMsgError = ex.Message.ToString();
                p_Log.Graba_Log_Error(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            }

            return _oRetornoVerificaFirma;
        }

        [ActionName("LeePDFContratos")]
        [Route("LeePDFContratos")]
        [HttpGet]
        public FormResponseModelo LeePDFContratos(string nomArchivo)
        {
            string listaArchivos = nomArchivo;
            string _metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            FormResponseModelo _oLeePDFContratos = new FormResponseModelo();
            try
            {
                string RutaEscritura = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivo"]).Trim();
                RutaEscritura = Path.Combine(RutaEscritura, "PDF\\Contratos");
                var directorio = listaArchivos;
                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                Proceso.Url = ((string)System.Configuration.ConfigurationManager.AppSettings["Urlbase"]).Trim();
                byte[] archivo = Proceso.LeePdfContrato(directorio);

                p_Log.Graba_Log_Info("Ruta Contrato PDF: " + Path.Combine(RutaEscritura, directorio));

                System.IO.File.WriteAllBytes(Path.Combine(RutaEscritura, directorio), archivo);

                _oLeePDFContratos.lSuccess = true;
                _oLeePDFContratos.cCodError = "0";
            }
            catch (Exception ex)
            {
                _oLeePDFContratos.lSuccess = false;
                _oLeePDFContratos.cCodError = "9999";
                _oLeePDFContratos.cMsgError = ex.Message.ToString();
                p_Log.Graba_Log_Error(ex.Message.ToString());
            }
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

            return _oLeePDFContratos;
        }

        [ActionName("EscribirPDFContratos")]
        [Route("EscribirPDFContratos")]
        [HttpPost]
        public FormResponseModelo EscribirPDFContratos(string[] listaArchivos)
        {
            string _metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            FormResponseModelo _oEscribePDFContratos = new FormResponseModelo();
            try
            {
                var directorio = listaArchivos[0];
                string RutaLectura = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivo"]).Trim();
                RutaLectura = Path.Combine(RutaLectura, "PDF\\Contratos",directorio);

                p_Log.Graba_Log_Info("Ruta Contrato PDF: " + RutaLectura);

                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                Proceso.Url = ((string)System.Configuration.ConfigurationManager.AppSettings["Urlbase"]).Trim();
                Proceso.EscribePdfContrato(File.ReadAllBytes(RutaLectura), directorio);

                _oEscribePDFContratos.lSuccess = true;
                _oEscribePDFContratos.cCodError = "0";
            }
            catch (Exception ex)
            {
                _oEscribePDFContratos.lSuccess = false;
                _oEscribePDFContratos.cCodError = "9999";
                _oEscribePDFContratos.cMsgError = ex.Message.ToString();

                p_Log.Graba_Log_Error(ex.Message.ToString());
            }

            p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

            return _oEscribePDFContratos;
        }

        [ActionName("DescargaContratoPDF")]
        [Route("DescargaContratoPDF")]
        [HttpGet]
        public HttpResponseMessage DescargaContratoPDF(string NombreArchivo)
        {
            string _metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            byte[] res = null;
            try
            {
                string rutaarchivo = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivo"]).Trim();
                rutaarchivo = Path.Combine(rutaarchivo, "PDF\\Contratos", NombreArchivo);             
                

                res = File.ReadAllBytes(rutaarchivo);
                response.Content = new ByteArrayContent(res);
                response.Content.Headers.Add("x-filename", NombreArchivo);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");                 
            }

            catch (Exception ex)
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                res = null;
                p_Log.Graba_Log_Error(ex.Message.ToString());
            }

            return response;

        }

        private DataTable GenerateTransposedTable(ContratoPlantilla inputObj)
        {
            DataTable outputTable = new DataTable();

            // El encabezado de la primera columna es el mismo. 
            outputTable.Columns.Add("Campo");
            outputTable.Columns.Add("Valor");

            Type myClassType = inputObj.GetType();

            foreach (PropertyInfo pi in myClassType.GetProperties())
            {
                DataRow newRow = outputTable.NewRow();
                newRow[0] = pi.Name;
                newRow[1] = pi.GetValue(inputObj, null);
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }

        protected Boolean ValidarCertificadoServidor(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private string[] CertificarDocumento(string rutaDocumentoFirmado)
        {
            string _metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            string[] salidaFirmas = null;
            
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidarCertificadoServidor);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                p_Log.Graba_Log_Info("Doc. Firmado: " + rutaDocumentoFirmado);

                using (var reader = new PdfReader(rutaDocumentoFirmado))
                {                   
                    var campos = reader.AcroFields;
                    var nombresDefirmas = campos.GetSignatureNames();
                    int c = 0;
                    salidaFirmas = new string[nombresDefirmas.Count];
                    foreach (var nombre in nombresDefirmas)
                    {
                        var firmaTmp = campos.VerifySignature(nombre);
                        string[] NombreFirma = firmaTmp.SigningCertificate.SubjectDN.ToString().Split(',');
                        for (int g = 0; g < NombreFirma.Length; g++)
                        {
                            if (NombreFirma[g].Contains("CN"))
                            {
                                salidaFirmas[c] = NombreFirma[g];
                                salidaFirmas[c] = salidaFirmas[c].Replace("CN=", "");
                            }
                        }

                        c++;
                    }
                }
            }
            catch(Exception ex)
            {
                salidaFirmas = null;
                p_Log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message);

            }
            finally {
                p_Log.Graba_Log_Info("Firmas encontradas: " + salidaFirmas);
                p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            }

            
            
            return salidaFirmas;

        }

        /// <summary>
        ///  Retorna una fecha en letras
        /// </summary>
        /// <param name="FechaHoy"></param>
        /// <returns></returns>
        private string Letras(DateTime FechaHoy)
        {
            int dia = FechaHoy.Day;
            int mes = FechaHoy.Month;
            int anio = FechaHoy.Year;
            int anioTmp = 0;

            anioTmp = anio - 2000;


            return NumLetras(dia) + " de " + MesLetras(mes) + " del dos mil " + NumLetras(anioTmp);
        }

        /// <summary>
        ///  Retorna el equivalente en letras de un mes
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        private string MesLetras(int mes)
        {
            string letras = "";
            switch (mes)
            {
                case 1: letras = "Enero"; break;
                case 2: letras = "Febreo"; break;
                case 3: letras = "Marzo"; break;
                case 4: letras = "Abril"; break;
                case 5: letras = "Mayo"; break;
                case 6: letras = "Junio"; break;
                case 7: letras = "Julio"; break;
                case 8: letras = "Agosto"; break;
                case 9: letras = "Septiembre"; break;
                case 10: letras = "Octubre"; break;
                case 11: letras = "Noviembre"; break;
                case 12: letras = "Diciembre"; break;
                default: letras = ""; break;
            }
            return letras;
        }

        /// <summary>
        ///  Retorna el equivalente en letras de un número
        /// </summary>
        /// <param name="numero"></param>
        /// <returns></returns>
        private string NumLetras(int numero)
        {
            string letras = "";
            switch (numero)
            {
                case 1: letras = "uno"; break;
                case 2: letras = "dos"; break;
                case 3: letras = "tres"; break;
                case 4: letras = "cuatro"; break;
                case 5: letras = "cinco"; break;
                case 6: letras = "seis"; break;
                case 7: letras = "siete"; break;
                case 8: letras = "ocho"; break;
                case 9: letras = "nuevo"; break;
                case 10: letras = "diez"; break;
                case 11: letras = "once"; break;
                case 12: letras = "doce"; break;
                case 13: letras = "trece"; break;
                case 14: letras = "catorce"; break;
                case 15: letras = "quince"; break;
                case 16: letras = "dieciseis"; break;
                case 17: letras = "diecisiete"; break;
                case 18: letras = "dieciocho"; break;
                case 19: letras = "diecinueve"; break;
                case 20: letras = "veinte"; break;
                case 21: letras = "veintiuno"; break;
                case 22: letras = "veintidos"; break;
                case 23: letras = "veintitres"; break;
                case 24: letras = "veinticuatro"; break;
                case 25: letras = "veinticinco"; break;
                case 26: letras = "veintiseis"; break;
                case 27: letras = "veintisiete"; break;
                case 28: letras = "veintiocho"; break;
                case 29: letras = "veintinueve"; break;
                case 30: letras = "treinta"; break;
                case 31: letras = "treinta y uno"; break;
                case 32: letras = "treinta y dos"; break;
                case 33: letras = "treinta y tres"; break;
                case 34: letras = "treinta y cuatro"; break;
                case 35: letras = "treinta y cinco"; break;
                case 36: letras = "treinta y seis"; break;
                case 37: letras = "treinta y siete"; break;
                case 38: letras = "treinta y ocho"; break;
                case 39: letras = "treinta y nueve"; break;
                case 40: letras = "cuarenta"; break;
                case 41: letras = "cuarenta y uno"; break;
                case 42: letras = "cuarenta y dos"; break;
                case 43: letras = "cuarenta y tres"; break;
                case 44: letras = "cuarenta y cuatro"; break;
                case 45: letras = "cuarenta y cinco"; break;
                case 46: letras = "cuarenta y seis"; break;
                case 47: letras = "cuarenta y siete"; break;
                case 48: letras = "cuarenta y ocho"; break;
                case 49: letras = "cuarenta y nueve"; break;
                case 50: letras = "cincuenta"; break;
                default: letras = ""; break;
            }
            return letras;
        }
    }
}
