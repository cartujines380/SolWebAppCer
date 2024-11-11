using AngularJSAuthentication.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Xml.Linq;

namespace clibProveedores
{
    [Authorize]
    [RoutePrefix("api/MantenimientoDocumentos")]
    public class MantenimientoDocumentosController: ApiController
    {
        Logger.Logger log = new Logger.Logger();
        private string _clase;

        public MantenimientoDocumentosController()
        {
            _clase = GetType().Name;
        }

        //Consulta lista de los documentos en BD
        [ActionName("consultaDocumentos")]
        [HttpGet]
        public FormResponseDocumentos getConsultaDocumentos()
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            string _metodo = MethodBase.GetCurrentMethod().Name;

            log.FilePath = p_Log;
            log.WriteMensaje(_clase + " " + _metodo + " INI ");
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XAttribute("Tipo", 1)));
            log.FilePath = p_Log;
            log.WriteMensaje(_clase + " " +_metodo + ": \n" + wresulFactList.ToString());

            ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 415, 1);
            FormResponseDocumentos FormResponse = new FormResponseDocumentos();
            FormResponse.success = true;

            try
            {
                List<Documentos> Retorno = new List<Documentos>();
                Retorno = (from reg in ds.Tables[0].AsEnumerable()
                           select new Documentos
                           {
                               IdDocumentos = reg.Field<int>("IdDocumentos"),
                               CodTipoPersona = reg.Field<string>("CodTipoPersona"),
                               NomTipoPersona = reg.Field<string>("NomTipoPersona"),
                               Codigo = reg.Field<string>("Codigo"),
                               Descripcion = reg.Field<string>("Descripcion"),
                               EsObligatorio = reg.Field<string>("EsObligatorio"),
                               FechaRegistro = reg.Field<DateTime>("FechaRegistro").ToString("dd-MM-yyyy"),
                               Estado = reg.Field<string>("Estado"),

                           }).ToList<Documentos>();
                FormResponse.root.Add(Retorno);
            }
            catch (Exception e)
            {
                log.FilePath = p_Log;
                log.WriteMensaje(_clase  + " " + _metodo + "Error : " + e.Message.ToString());

                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
            }

            log.FilePath = p_Log;
            log.WriteMensaje(_clase + " " + _metodo + " FIN ");
            

            return FormResponse;
        }

        //Ingreso de documentos
        [ActionName("ingresaDocumentos")]
        [Route("ingresaDocumentos")]
        [HttpPost]

        public FormResponseDocumentos ingresaDocumentos(Documentos objDocumentos, string nAccion)
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            string _metodo = MethodBase.GetCurrentMethod().Name;

            FormResponseDocumentos _oRetornoDocumentos = new FormResponseDocumentos();
            try
            {
                log.FilePath = p_Log;
                log.WriteMensaje(_clase + " " + _metodo + " INI ");
                DataSet ds = new DataSet();
                ClsGeneral objEjecucion = new ClsGeneral();

                var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XAttribute("Tipo", 2),
                            new XElement("Pagos",
                                    new XAttribute("Accion", nAccion),
                                    new XAttribute("IdDocumentos", objDocumentos.IdDocumentos),
                                    new XAttribute("CodTipoPersona", objDocumentos.CodTipoPersona),
                                    new XAttribute("Descripcion", objDocumentos.Descripcion.Length > 200 ? objDocumentos.Descripcion.Substring(200): objDocumentos.Descripcion),
                                    new XAttribute("EsObligatorio", objDocumentos.EsObligatorio == "true" ? "S":"N"),
                                    new XAttribute("Estado", objDocumentos.Estado),
                                    new XAttribute("UsuarioCreacion", objDocumentos.UsuarioCreacion.Length > 20 ? objDocumentos.UsuarioCreacion.Substring(20): objDocumentos.UsuarioCreacion))));

                log.FilePath = p_Log;
                log.WriteMensaje(_clase + " - " + _metodo + ": \n" + wresulFactList.ToString());

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 415, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    _oRetornoDocumentos.success = true;
                    _oRetornoDocumentos.codError = "0";
                }
                else
                {
                    _oRetornoDocumentos.success = false;
                    _oRetornoDocumentos.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    _oRetornoDocumentos.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();

                    log.FilePath = p_Log;
                    log.WriteMensaje(_clase + " " + _metodo + "  Error : " + _oRetornoDocumentos.codError + " - " + _oRetornoDocumentos.msgError);
                }


            }
            catch (Exception ex)
            {
                _oRetornoDocumentos.success = false;
                _oRetornoDocumentos.codError = "9999";
                _oRetornoDocumentos.msgError = ex.Message.ToString();

                log.FilePath = p_Log;
                log.WriteMensaje(_clase + " " + _metodo + "  Error : " + ex.Message.ToString());
            }
            finally
            {
                log.FilePath = p_Log;
                log.WriteMensaje(_clase + " " + _metodo + " FIN ");
            }

            return _oRetornoDocumentos;
        }
    }
}