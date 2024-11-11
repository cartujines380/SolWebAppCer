using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Xml;
using System.Security.Claims;
using clibProveedores;
using clibProveedores.Models;
using AngularJSAuthentication.API.Models;


namespace AngularJSAuthentication.API.Controllers
{


    [RoutePrefix("api/Catalogos")]
    public class CatalogosController : ApiController
    {

        [ActionName("Catalogos")]
        public List<TablaCatalogo> GetCatalogos(string NombreCatalogo)
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            Logger.Logger loge = new Logger.Logger();

            loge.FilePath = p_Log;
            loge.WriteMensaje("GetCatalogos NombreCatalogo: "+ NombreCatalogo);
            loge.Linea();

            ClsGeneral ObjEjecucion = new ClsGeneral();
            return ObjEjecucion.GetCatalogos(NombreCatalogo);

        }

        [ActionName("cargarParametrosCO")]
        [HttpGet]
        public string cargarParametrosCO(string parametrosCO)
        {
            string retorno = "";

            string lv_porcentaje14 = System.Configuration.ConfigurationManager.AppSettings["Porcentaje"];
            string lv_Codigoporcentaje14 = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje"];
            string lv_porcentaje12 = System.Configuration.ConfigurationManager.AppSettings["Porcentaje12"];
            string lv_Codigoporcentaje12 = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje12"];

            retorno = lv_Codigoporcentaje12 + "," + lv_porcentaje12 + "," + lv_Codigoporcentaje14 + "," + lv_porcentaje14;
            return retorno;
        }

        [ActionName("grabarCatalogo")]
        [HttpPost]
        public FormResponseModelo grabarCatalogo(Grabar_Catalogo DatosIng)
        {
            FormResponseModelo _oRetorno = new FormResponseModelo();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "2");
                xmlParam.DocumentElement.SetAttribute("IdCatalogo", DatosIng.cabecera.idCatalogo);
                xmlParam.DocumentElement.SetAttribute("descripcion", DatosIng.cabecera.descripcion);
                xmlParam.DocumentElement.SetAttribute("Estado", DatosIng.cabecera.Estado);

                foreach (var it in DatosIng.detalleCatalogo)
                {
                    XmlElement elem = xmlParam.CreateElement("CatalogoDetalle");
                    elem.SetAttribute("codigoDetalle", it.codigoDetalle);
                    elem.SetAttribute("descripcion", it.descripcion);
                    elem.SetAttribute("descAlterno", it.descAlterno);
                    if (it.Estado == true)
                    {
                        elem.SetAttribute("Estado", "A");
                    }
                    else
                    {
                        elem.SetAttribute("Estado", "I");
                    }
                    xmlParam.DocumentElement.AppendChild(elem);
                }

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 851, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        if (Convert.ToString(item["ESTADO"]) == "INGRESADO")
                        {
                            _oRetorno.root.Add(Convert.ToString(item["RETORNO"]));
                            _oRetorno.cCodError = "0";
                            _oRetorno.lSuccess = true;
                        }
                        else
                        {
                            if (Convert.ToString(item["ESTADO"]) == "EXISTE")
                            {
                                _oRetorno.cMsgError = "EXISTE";
                                _oRetorno.cCodError = "0";
                                _oRetorno.lSuccess = false;
                            }
                            else
                            {
                                _oRetorno.cCodError = "110";
                                _oRetorno.cMsgError = "Error Comunicacion";
                                _oRetorno.lSuccess = false;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _oRetorno.cCodError = "100";
                _oRetorno.cMsgError = "Error Comunicacion";
            }
            return _oRetorno;

        }
        [ActionName("getCargaSociedad")]
        [HttpGet]
        public FormResponseTransporte getCargaSociedad(string tipoid)
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            Logger.Logger loge = new Logger.Logger();

            loge.FilePath = p_Log;
            loge.WriteMensaje("getCargaSociedad tipoid: " + tipoid);
            loge.Linea();
            string retorno = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            TablaCatalogo mod_SolicitudBandeja;
            List<TablaCatalogo> lst_retornoSolBandeja = new List<TablaCatalogo>();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipoid);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 306, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudBandeja = new TablaCatalogo();
                        mod_SolicitudBandeja.Codigo = Convert.ToString(item["codigo"]);
                        mod_SolicitudBandeja.Detalle = Convert.ToString(item["detalle"]);
                        mod_SolicitudBandeja.DescAlterno = Convert.ToString(item["DescAlterno"]);
                        mod_SolicitudBandeja.Licencia = Convert.ToString(item["Licencia"]);
                        clibUtil.Util.clsUtilitario obj = new clibUtil.Util.clsUtilitario();
                        try
                        {
                            if (obj.Decrypt(Convert.ToString(item["Licencia"])).ToString().Split('|')[0] == mod_SolicitudBandeja.Codigo && obj.Decrypt(Convert.ToString(item["Licencia"])).ToString().Split('|')[2] == "1928374650")
                            {
                                lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSolBandeja);
                }
            }
            catch (Exception)
            {

            }
            return FormResponse;
        }

        [ActionName("ConsultarContalogo")]
        [HttpGet]
        public FormResponseModelo ConsultarContalogo(string codigoCatalogo, string NombreCatalogo, string Estado)
        {
            FormResponseModelo _oRetorno = new FormResponseModelo();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repCatalogoConsulta> lst_catalogo = new List<repCatalogoConsulta>();
            repCatalogoConsulta consultacatalogo;
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "3");
                xmlParam.DocumentElement.SetAttribute("IdCatalogo", codigoCatalogo);
                xmlParam.DocumentElement.SetAttribute("NombreCatalogo", NombreCatalogo);
                xmlParam.DocumentElement.SetAttribute("Estado", Estado);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 851, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        consultacatalogo = new repCatalogoConsulta();
                        consultacatalogo.idCabecera = Convert.ToString(item["IdCabecera"]);
                        consultacatalogo.codigoCatalogo = Convert.ToString(item["codigoCatalogo"]);
                        consultacatalogo.descripcion = Convert.ToString(item["descripcion"]);
                        //consultacatalogo.descAlterno = Convert.ToString(item["DescAlterno"]);
                        consultacatalogo.estado = Convert.ToString(item["estado"]);
                        lst_catalogo.Add(consultacatalogo);
                    }
                    _oRetorno.root.Add(lst_catalogo);
                    _oRetorno.cCodError = "0";
                    _oRetorno.lSuccess = true;
                }
            }
            catch (Exception ex)
            {
                _oRetorno.lSuccess = false;
                _oRetorno.cCodError = "100";
                _oRetorno.cMsgError = "Error Comunicacion";
            }

            return _oRetorno;
        }

        [ActionName("BuscarUnCatalogo")]
        [HttpGet]
        public FormResponseModelo BuscarUnCatalogo(string codigoCatalogo)
        {
            FormResponseModelo _oRetorno = new FormResponseModelo();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repCatalogoConsulta> lst_catalogo = new List<repCatalogoConsulta>();
            repCatalogoConsulta consultaCatalogo;
            List<p_detalleCatalogo2> lst_comprador2 = new List<p_detalleCatalogo2>();
            p_detalleCatalogo2 consultaComprador2;
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "5");
                xmlParam.DocumentElement.SetAttribute("IdCatalogo", codigoCatalogo);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 851, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        consultaCatalogo = new repCatalogoConsulta();
                        consultaCatalogo.idCabecera = Convert.ToString(item["IdCabecera"]);
                        consultaCatalogo.codigoCatalogo = Convert.ToString(item["codigoCatalogo"]);
                        consultaCatalogo.descripcion = Convert.ToString(item["descripcion"]);
                        lst_catalogo.Add(consultaCatalogo);
                    }
                    _oRetorno.root.Add(lst_catalogo);
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        consultaComprador2 = new p_detalleCatalogo2();
                        consultaComprador2.IdDetalle = Convert.ToString(item["CodElemento"]);
                        consultaComprador2.codigoDetalle = Convert.ToString(item["codigoDetalle"]);
                        consultaComprador2.descripcion = Convert.ToString(item["Descripcion"]);
                        consultaComprador2.descAlterno = Convert.ToString(item["DescAlterno"]);
                        if (Convert.ToString(item["Estado"]) == "A")
                        {
                            consultaComprador2.Estado = true;
                        }
                        else
                        {
                            consultaComprador2.Estado = false;
                        }
                        lst_comprador2.Add(consultaComprador2);
                    }
                    _oRetorno.root.Add(lst_comprador2);
                    _oRetorno.cCodError = "0";
                    _oRetorno.lSuccess = true;
                }
            }
            catch (Exception ex)
            {
                _oRetorno.lSuccess = false;
                _oRetorno.cCodError = "100";
                _oRetorno.cMsgError = "Error Comunicacion";
            }

            return _oRetorno;
        }
    }
}
