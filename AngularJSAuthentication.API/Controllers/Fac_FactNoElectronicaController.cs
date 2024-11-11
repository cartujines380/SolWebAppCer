using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Models;

using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Security.Claims;
using clibProveedores.Models;
using clibProveedores;
using clibSeguridadCR;

using SAP.Middleware.Connector;
using AngularJSAuthentication.API.Handlers;


namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/FacFactNoElec")]
    public class FacFactNoElecController : ApiController
    {

        [AntiForgeryValidate]
        [ActionName("consProveedorExcepto")]
        [HttpGet]
        public string consProveedorExcepto(string proveedorExcepto)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            string retorno = "";
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", proveedorExcepto);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 706, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {

                        if (retorno == "")
                        {
                            retorno = Convert.ToString(item["codigoSap"]);
                        }
                        else
                        {
                            retorno = retorno + ";" + Convert.ToString(item["codigoSap"]);
                        }

                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {

            }
            return retorno;
        }

        [AntiForgeryValidate]
        [ActionName("ConsSelPedidosFiltro")]
        [HttpGet]
        public formResponseFacturas GetConsSelPedidosFiltro(string CodSap, string Ruc, string Usuario,
                                   string NumPedido, string FechaIni, string FechaFin, string Estados, string CodAlmacen)
        {
            formResponseFacturas FormResponse = new formResponseFacturas();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            string retorno = "";

            string lv_porcentaje14 = System.Configuration.ConfigurationManager.AppSettings["Porcentaje"];
            string lv_Codigoporcentaje14 = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje"];
            string lv_porcentaje12 = System.Configuration.ConfigurationManager.AppSettings["Porcentaje12"];
            string lv_Codigoporcentaje12 = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje12"];

            retorno = lv_Codigoporcentaje12 + "," + lv_porcentaje12 + "," + lv_Codigoporcentaje14 + "," + lv_porcentaje14;
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodSap);
                xmlParam.DocumentElement.SetAttribute("CodAlmacen", CodAlmacen != null ? CodAlmacen : "");
                if (!String.IsNullOrEmpty(NumPedido)) xmlParam.DocumentElement.SetAttribute("NumPedido", NumPedido);
                if (!String.IsNullOrEmpty(FechaIni)) xmlParam.DocumentElement.SetAttribute("FechaDesde", FechaIni);
                if (!String.IsNullOrEmpty(FechaFin)) xmlParam.DocumentElement.SetAttribute("FechaHasta", FechaFin);
                if (!String.IsNullOrEmpty(Estados))
                {
                    foreach (string it in Estados.Split(new char[] { ';' }))
                    {
                        if (it != "")
                        {
                            XmlElement elem = xmlParam.CreateElement("Est");
                            elem.SetAttribute("id", it);
                            xmlParam.DocumentElement.AppendChild(elem);
                        }
                    }
                }
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 601, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<facConsSelPedidos> TmpLstCons = new List<facConsSelPedidos>();
                    decimal valPendPed;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        facConsSelPedidos TmpItem = new facConsSelPedidos();
                        TmpItem.idPedido = Convert.ToInt32(item["IdPedido"]);
                        TmpItem.numPedido = Convert.ToString(item["NumPedido"]);
                        TmpItem.fechaPedido = Convert.ToDateTime(item["FechaPedido"]);
                        TmpItem.codAlmacen = Convert.ToString(item["CodAlmacen"]); ;
                        TmpItem.nomAlmacen = Convert.ToString(item["NomAlmacen"]);
                        TmpItem.codAlmDestino = Convert.ToString(item["CodAlmDestino"]);
                        TmpItem.zonaOrigen = Convert.ToString(item["ZonaOrigen"]);
                        TmpItem.esDescargado = Convert.ToBoolean(item["EsDescargado"]);
                        TmpItem.esImpreso = Convert.ToBoolean(item["EsImpreso"]);
                        TmpItem.esDescargadoDes = Convert.ToString(item["EsDescargadoDes"]);
                        TmpItem.esImpresoDes = Convert.ToString(item["EsImpresoDes"]);
                        TmpItem.estadoDes = Convert.ToString(item["EstadoDes"]);
                        TmpItem.valorTotalPedido = Convert.ToDecimal(item["ValorTotalPedido"]);
                        TmpItem.totalSumaFacturas = Convert.ToDecimal(item["TotalSumaFacturas"]);
                        TmpItem.estado = Convert.ToString(item["Estado"]);
                        valPendPed = Convert.ToDecimal(item["ValorTotalPedido"]) - Convert.ToDecimal(item["TotalSumaFacturas"]);
                        TmpItem.valorPendPedido = valPendPed;
                        TmpItem.siValorPend = (valPendPed > (decimal)0 ? 'S' : 'N');
                        TmpItem.data = retorno;
                        TmpLstCons.Add(TmpItem);
                    }
                    TmpList.Add(TmpLstCons);
                    // FIN - ARMADO DE ESTRUCTURA PARA GRID
                    // INI - DEVOLVER ARREGLO
                    string[] dataAdic = new string[2];
                    dataAdic[0] = ds.Tables[1].Rows[0]["NomComercial"].ToString();
                    dataAdic[1] = ds.Tables[1].Rows[0]["Ruc"].ToString();
                    TmpList.Add(dataAdic);
                    // FIN - DEVOLVER ARREGLO
                    FormResponse.root.Add(TmpList);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false; FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }



        [AntiForgeryValidate]
        [ActionName("cargarParametros")]
        [HttpGet]
        public string cargarParametros(string parametros)
        {
            string retorno = "";

            string lv_porcentaje14 = System.Configuration.ConfigurationManager.AppSettings["Porcentaje"];
            string lv_Codigoporcentaje14 = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje"];
            string lv_porcentaje12 = System.Configuration.ConfigurationManager.AppSettings["Porcentaje12"];
            string lv_Codigoporcentaje12 = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje12"];

            retorno = lv_Codigoporcentaje12 + "," + lv_porcentaje12 + "," + lv_Codigoporcentaje14 + "," + lv_porcentaje14;
            return retorno;
        }


        [AntiForgeryValidate]
        [ActionName("ConsRecupFacturasFiltro")]
        [HttpGet]
        public formResponseFacturas GetConsRecupFacturasFiltro(string CodSap, string Ruc, string Usuario,
                                   string NumPedido, string FacEst, string FacPto, string FacSec,
                                   string FecEsReg, string FechaIni, string FechaFin, string Estados, string CodAlmacen)
        {
            formResponseFacturas FormResponse = new formResponseFacturas();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodSap);
                xmlParam.DocumentElement.SetAttribute("CodAlmacen", CodAlmacen != null ? CodAlmacen : "");
                xmlParam.DocumentElement.SetAttribute("FecReg", FecEsReg);
                if (!String.IsNullOrEmpty(FechaIni)) xmlParam.DocumentElement.SetAttribute("FechaInicio", FechaIni);
                if (!String.IsNullOrEmpty(FechaFin)) xmlParam.DocumentElement.SetAttribute("FechaFin", FechaFin);
                xmlParam.DocumentElement.SetAttribute("Guardado", "N");
                xmlParam.DocumentElement.SetAttribute("Emitido", "N");
                xmlParam.DocumentElement.SetAttribute("Receptado", "N");
                if (!String.IsNullOrEmpty(Estados))
                {
                    foreach (string it in Estados.Split(new char[] { ';' }))
                    {
                        if (it == "IN") xmlParam.DocumentElement.SetAttribute("Guardado", "S");
                        if (it == "EN") xmlParam.DocumentElement.SetAttribute("Emitido", "S");
                        if (it == "RE") xmlParam.DocumentElement.SetAttribute("Receptado", "S");
                    }
                }
                string retorno = "";

                string lv_porcentaje14 = System.Configuration.ConfigurationManager.AppSettings["Porcentaje"];
                string lv_Codigoporcentaje14 = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje"];
                string lv_porcentaje12 = System.Configuration.ConfigurationManager.AppSettings["Porcentaje12"];
                string lv_Codigoporcentaje12 = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje12"];

                retorno = lv_Codigoporcentaje12 + "," + lv_porcentaje12 + "," + lv_Codigoporcentaje14 + "," + lv_porcentaje14;

                if (!String.IsNullOrEmpty(NumPedido)) xmlParam.DocumentElement.SetAttribute("NumPedido", NumPedido);
                if (!String.IsNullOrEmpty(FacEst)) xmlParam.DocumentElement.SetAttribute("NumFactEst", FacEst);
                if (!String.IsNullOrEmpty(FacPto)) xmlParam.DocumentElement.SetAttribute("NumFactPto", FacPto);
                if (!String.IsNullOrEmpty(FacSec)) xmlParam.DocumentElement.SetAttribute("NumFactSec", FacSec);
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 602, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<facConsRecFacturas> TmpLstCons = new List<facConsRecFacturas>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        facConsRecFacturas TmpItem = new facConsRecFacturas();
                        TmpItem.idPedido = Convert.ToInt32(item["IdPedido"]);
                        TmpItem.idDocumento = Convert.ToInt32(item["IdDocumento"]);
                        TmpItem.numPedido = Convert.ToString(item["NumPedido"]);
                        TmpItem.numFactura = Convert.ToString(item["NumFactura"]);
                        TmpItem.fechaRegistro = Convert.ToDateTime(item["FechaRegistro"]);
                        TmpItem.fechaEmision = Convert.ToDateTime(item["FechaEmision"]);
                        TmpItem.nombreArchivo = Convert.ToString(item["NombreArchivo"]);
                        TmpItem.estado = Convert.ToString(item["Estado"]);
                        TmpItem.estadoDes = Convert.ToString(item["EstadoDes"]);
                        TmpItem.codAlmacen = Convert.ToString(item["CodAlmacen"]);
                        TmpItem.almacen = Convert.ToString(item["Almacen"]);
                        TmpItem.data = retorno;
                        TmpLstCons.Add(TmpItem);
                    }
                    TmpList.Add(TmpLstCons);
                    // FIN - ARMADO DE ESTRUCTURA PARA GRID
                    // INI - DEVOLVER ARREGLO
                    string[] dataAdic = new string[2];
                    dataAdic[0] = ds.Tables[1].Rows[0]["NomComercial"].ToString();
                    dataAdic[1] = ds.Tables[1].Rows[0]["Ruc"].ToString();
                    TmpList.Add(dataAdic);
                    // FIN - DEVOLVER ARREGLO
                    FormResponse.root.Add(TmpList);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        [AntiForgeryValidate]
        [ActionName("ConsultaPedidoNumero")]
        [HttpGet]
        public formResponseFacturas GetConsultaPedidoNumero(string CodSap, string Ruc, string Usuario, string IdPedido)
        {
            formResponseFacturas FormResponse = new formResponseFacturas();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("IdPedido", IdPedido);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 603, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    // INI - ARMADO DE CABECERA
                    facConsPedidoNumeroCab dataCab = new facConsPedidoNumeroCab();
                    dataCab.idPedido = Convert.ToInt32(ds.Tables[0].Rows[0]["IdPedido"]);
                    dataCab.numPedido = Convert.ToString(ds.Tables[0].Rows[0]["NumPedido"]);
                    dataCab.nomProveedor = Convert.ToString(ds.Tables[0].Rows[0]["NomProveedor"]);
                    dataCab.nomComercial = Convert.ToString(ds.Tables[0].Rows[0]["NomComercial"]);
                    dataCab.rucProveedor = Convert.ToString(ds.Tables[0].Rows[0]["RucProveedor"]);
                    dataCab.dirCallePrinc = Convert.ToString(ds.Tables[0].Rows[0]["DirCallePrinc"]);
                    dataCab.dirCalleNum = Convert.ToString(ds.Tables[0].Rows[0]["DirCalleNum"]);
                    dataCab.dirPisoEdificio = Convert.ToString(ds.Tables[0].Rows[0]["DirPisoEdificio"]);
                    dataCab.nomEmpresa = Convert.ToString(ds.Tables[0].Rows[0]["NomEmpresa"]);
                    dataCab.rucEmpresa = Convert.ToString(ds.Tables[0].Rows[0]["RucEmpresa"]);
                    dataCab.codAlmacen = Convert.ToString(ds.Tables[0].Rows[0]["CodAlmacen"]);
                    dataCab.nomAlmacen = Convert.ToString(ds.Tables[0].Rows[0]["NomAlmacen"]);
                    try
                    {
                        dataCab.fechaPedido = Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaPedido"]).ToString(getDateFormatSite());
                    }
                    catch (Exception)
                    {
                        dataCab.fechaPedido = DateTime.Now.ToString(getDateFormatSite());
                    }
                    dataCab.subTotalSumaFacturas = Convert.ToDecimal(ds.Tables[0].Rows[0]["SubTotalSumaFacturas"]);
                    // FIN - ARMADO DE CABECERA
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<facConsPedidoNumeroDetItem> TmpLstCons = new List<facConsPedidoNumeroDetItem>();
                    decimal valTotal = (decimal)0;
                    decimal valTotalTbl = (decimal)0;
                    decimal subTotal;
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        facConsPedidoNumeroDetItem TmpItem = new facConsPedidoNumeroDetItem();
                        TmpItem.item = Convert.ToString(item["Item"]);
                        TmpItem.codArticulo = Convert.ToString(item["CodArticulo"]); ;
                        TmpItem.desArticulo = Convert.ToString(item["DesArticulo"]);
                        TmpItem.cantPedido = Convert.ToDecimal(item["CantPedido"]);
                        TmpItem.cantPendiente = Convert.ToDecimal(item["CantPendiente"]);
                        TmpItem.precioCosto = Convert.ToDecimal(item["PrecioCosto"]);
                        TmpItem.tamanoCaja = Convert.ToString(item["TamanoCaja"]);
                        TmpItem.descuento1 = Convert.ToDecimal(item["Descuento1"]);
                        TmpItem.descuento2 = Convert.ToDecimal(item["Descuento2"]);
                        TmpItem.indIva1 = Convert.ToString(item["IndIva1"]);
                        TmpItem.indIva1Des = Convert.ToString(item["IndIva1Des"]);
                        TmpItem.subTotalItem = Convert.ToDecimal(item["SubTotalItem"]);
                        TmpItem.totalItem = Convert.ToDecimal(item["TotalItem"]);
                        subTotal = TmpItem.cantPedido * TmpItem.precioCosto;
                        valTotal += subTotal;
                        valTotalTbl += TmpItem.subTotalItem;
                        TmpLstCons.Add(TmpItem);
                    }
                    // FIN - ARMADO DE ESTRUCTURA PARA GRID
                    if (valTotalTbl > valTotal)
                    {
                        valTotal = valTotalTbl;
                    }
                    dataCab.valTotalPedido = valTotal;
                    TmpList.Add(dataCab);
                    TmpList.Add(TmpLstCons);
                    FormResponse.root.Add(TmpList);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        [AntiForgeryValidate]
        [ActionName("ConsultaDocumentoId")]
        [HttpGet]
        public formResponseFacturas GetConsultaDocumentoId(string CodSap, string Ruc, string Usuario, string IdDocumento)
        {
            formResponseFacturas FormResponse = new formResponseFacturas();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            XmlDocument xmlSipeDoc = new XmlDocument();
            XmlDocument xmlInfoPed = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("IdDocumento", IdDocumento);
                xmlParam.DocumentElement.SetAttribute("SiDetPedido", "S");
                xmlParam.DocumentElement.SetAttribute("SiDetDoc", "N");
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 604, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    // INI - ARMADO DE CABECERA
                    facConsDocumentoIdCab dataCab = new facConsDocumentoIdCab();
                    dataCab.idDocumento = Convert.ToInt32(ds.Tables[0].Rows[0]["IdDocumento"]);
                    dataCab.idPedido = Convert.ToInt32(ds.Tables[0].Rows[0]["IdPedido"]);
                    if (ds.Tables[0].Rows[0]["NombreArchivo"] != DBNull.Value && (string)ds.Tables[0].Rows[0]["NombreArchivo"] != "")
                    {
                        dataCab.nombreArchivo = Convert.ToString(ds.Tables[0].Rows[0]["NombreArchivo"]);
                        dataCab.xmlSRI = Convert.ToString(ds.Tables[0].Rows[0]["XmlSRI"]);
                        dataCab.claveAcceso = Convert.ToString(ds.Tables[0].Rows[0]["ClaveAcceso"]);
                    }
                    else
                    {
                        dataCab.nombreArchivo = "";
                    }
                    dataCab.estado = Convert.ToString(ds.Tables[0].Rows[0]["Estado"]);
                    dataCab.chkcompesacion = Convert.ToBoolean(ds.Tables[0].Rows[0]["chkcompesacion"]);
                    xmlSipeDoc.LoadXml(Convert.ToString(ds.Tables[0].Rows[0]["XmlDocumento"]));
                    dataCab.nomProveedor = xmlSipeDoc.DocumentElement.GetAttribute("rs");
                    if (xmlSipeDoc.DocumentElement.GetAttribute("nc") != "")
                    {
                        dataCab.nomComercial = xmlSipeDoc.DocumentElement.GetAttribute("nc");
                    }
                    else
                    {
                        dataCab.nomComercial = xmlSipeDoc.DocumentElement.GetAttribute("rs");
                    }
                    dataCab.rucProveedor = xmlSipeDoc.DocumentElement.GetAttribute("r");
                    dataCab.facEstabl = xmlSipeDoc.DocumentElement.GetAttribute("e");
                    dataCab.facPtoEmi = xmlSipeDoc.DocumentElement.GetAttribute("pe");
                    dataCab.facNumSec = xmlSipeDoc.DocumentElement.GetAttribute("s");
                    dataCab.dirMatriz = xmlSipeDoc.DocumentElement.GetAttribute("dm");
                    dataCab.dirSucursal = xmlSipeDoc.DocumentElement.GetAttribute("ds");
                    dataCab.fechaEmision = xmlSipeDoc.DocumentElement.GetAttribute("fe");
                    dataCab.fechaIniVigAut = xmlSipeDoc.DocumentElement.GetAttribute("fiva");
                    dataCab.fechaFinVigAut = xmlSipeDoc.DocumentElement.GetAttribute("ffva");
                    dataCab.numAutorizaTal = xmlSipeDoc.DocumentElement.GetAttribute("na");
                    dataCab.nomEmpresa = xmlSipeDoc.DocumentElement.GetAttribute("rsc");
                    dataCab.rucEmpresa = xmlSipeDoc.DocumentElement.GetAttribute("ic");
                    xmlInfoPed.LoadXml(Convert.ToString(ds.Tables[0].Rows[0]["XmlInfoPedido"]));
                    dataCab.numPedido = xmlInfoPed.DocumentElement.GetAttribute("NumPedido");
                    dataCab.fechaPedido = xmlInfoPed.DocumentElement.GetAttribute("FechaPedido");
                    dataCab.codAlmacen = xmlInfoPed.DocumentElement.GetAttribute("CodAlmacen");
                    dataCab.nomAlmacen = xmlInfoPed.DocumentElement.GetAttribute("NomAlmacen");
                    dataCab.subTotalSumaFacturas = Convert.ToDecimal(ds.Tables[0].Rows[0]["SubTotalSumaFacturas"]);
                    foreach (XmlElement elem in xmlSipeDoc.SelectNodes("f/d"))
                    {
                        if (elem.Attributes["cp"].Value == "99990")
                        {
                            dataCab.detSubTot_0 = elem.Attributes["pu"].Value;
                            dataCab.detDescuento_0 = elem.Attributes["d"].Value;
                            foreach (XmlElement elemI in elem.SelectNodes("i"))
                            {
                                if (elemI.Attributes["c"].Value == "5")
                                {
                                    dataCab.detIrbpnr_0 = elemI.Attributes["v"].Value;
                                }
                            }
                        }
                        if (elem.Attributes["cp"].Value == "99992")
                        {
                            dataCab.detSubTot_12 = elem.Attributes["pu"].Value;
                            dataCab.detDescuento_12 = elem.Attributes["d"].Value;
                            foreach (XmlElement elemI in elem.SelectNodes("i"))
                            {
                                if (elemI.Attributes["c"].Value == "3")
                                {
                                    dataCab.detIce = elemI.Attributes["v"].Value;
                                }
                                if (elemI.Attributes["c"].Value == "5")
                                {
                                    dataCab.detIrbpnr_12 = elemI.Attributes["v"].Value;
                                }
                            }
                        }
                    }
                    // FIN - ARMADO DE CABECERA
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<facConsPedidoNumeroDetItem> TmpLstCons = new List<facConsPedidoNumeroDetItem>();
                    decimal valTotal = (decimal)0;
                    decimal valTotalTbl = (decimal)0;
                    decimal subTotal;
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        facConsPedidoNumeroDetItem TmpItem = new facConsPedidoNumeroDetItem();
                        TmpItem.item = Convert.ToString(item["Item"]);
                        TmpItem.codArticulo = Convert.ToString(item["CodArticulo"]); ;
                        TmpItem.desArticulo = Convert.ToString(item["DesArticulo"]);
                        TmpItem.cantPedido = Convert.ToDecimal(item["CantPedido"]);
                        TmpItem.cantPendiente = Convert.ToDecimal(item["CantPendiente"]);
                        TmpItem.precioCosto = Convert.ToDecimal(item["PrecioCosto"]);
                        TmpItem.tamanoCaja = Convert.ToString(item["TamanoCaja"]);
                        TmpItem.descuento1 = Convert.ToDecimal(item["Descuento1"]);
                        TmpItem.descuento2 = Convert.ToDecimal(item["Descuento2"]);
                        TmpItem.indIva1 = Convert.ToString(item["IndIva1"]);
                        TmpItem.indIva1Des = Convert.ToString(item["IndIva1Des"]);
                        TmpItem.subTotalItem = Convert.ToDecimal(item["SubTotalItem"]);
                        TmpItem.totalItem = Convert.ToDecimal(item["TotalItem"]);
                        subTotal = TmpItem.cantPedido * TmpItem.precioCosto;
                        valTotal += subTotal;
                        valTotalTbl += TmpItem.subTotalItem;
                        TmpLstCons.Add(TmpItem);
                    }
                    // FIN - ARMADO DE ESTRUCTURA PARA GRID
                    if (valTotalTbl > valTotal)
                    {
                        valTotal = valTotalTbl;
                    }
                    dataCab.valTotalPedido = valTotal;
                    TmpList.Add(dataCab);
                    TmpList.Add(TmpLstCons);
                    FormResponse.root.Add(TmpList);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        [AntiForgeryValidate]
        [ActionName("GrabaDocumento")]
        [HttpPost]
        public formResponseFacturas GetGrabaDocumento(facGrabaDocumento objData)
        {
            formResponseFacturas FormResponse = new formResponseFacturas();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            XmlDocument xmlDocSipe = new XmlDocument();

            string lv_porcentaje = System.Configuration.ConfigurationManager.AppSettings["Porcentaje"];
            string lv_Codigoporcentaje = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje"];
            string lv_porcentaje12 = System.Configuration.ConfigurationManager.AppSettings["Porcentaje12"];
            string lv_Codigoporcentaje12 = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje12"];
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("TipoDoc", "01");
                xmlParam.DocumentElement.SetAttribute("ACCION", objData.tipoACCION);
                if (objData.tipoACCION == "A" || objData.tipoACCION == "E")
                {
                    xmlParam.DocumentElement.SetAttribute("IdDocumento", objData.idDocumento.ToString());
                }
                xmlParam.DocumentElement.SetAttribute("CodProveedor", objData.codSAP);
                xmlParam.DocumentElement.SetAttribute("IdPedido", objData.idPedido.ToString());
                xmlParam.DocumentElement.SetAttribute("NumPedido", objData.numPedido);
                xmlParam.DocumentElement.SetAttribute("Establecimiento", objData.facEstabl);
                xmlParam.DocumentElement.SetAttribute("PtoEmision", objData.facPtoEmi);
                xmlParam.DocumentElement.SetAttribute("Secuencial", objData.facNumSec);
                xmlParam.DocumentElement.SetAttribute("FechaEmision", objData.fechaEmision);
                xmlParam.DocumentElement.SetAttribute("chkcompoesacion", objData.chkcompesacion.ToString());
                if (Convert.ToDateTime(objData.fechaEmision) <= Convert.ToDateTime("31/05/2016"))
                {
                    lv_porcentaje = System.Configuration.ConfigurationManager.AppSettings["Porcentaje12"];
                    lv_Codigoporcentaje = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje12"];
                }
                else
                {
                    if (Convert.ToDateTime(objData.fechaEmision) <= Convert.ToDateTime("31/05/2017"))
                    {
                        lv_porcentaje = System.Configuration.ConfigurationManager.AppSettings["Porcentaje"];
                        lv_Codigoporcentaje = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje"];
                    }
                    else
                    {
                        lv_porcentaje = System.Configuration.ConfigurationManager.AppSettings["Porcentaje12"];
                        lv_Codigoporcentaje = System.Configuration.ConfigurationManager.AppSettings["CodigoPorcentaje12"];
                    }
                }

                if (objData.tipoACCION == "E")
                {


                    xmlDocSipe = GeneraDocSipeXML(objData, lv_porcentaje, lv_Codigoporcentaje, true);
                    clsGeneraComprobanteXML objXmlSri = new clsGeneraComprobanteXML();
                    string rec_nombreArchivo = "";
                    string rec_claveAcceso = "";
                    objData.xmlSRI = objXmlSri.getComprobanteFacturaXML(xmlDocSipe, ref rec_nombreArchivo, ref rec_claveAcceso);
                    objData.nombreArchivo = rec_nombreArchivo;
                    objData.claveAcceso = rec_claveAcceso;
                    xmlParam.DocumentElement.SetAttribute("SiModificaDetalleFactura", "N");
                    xmlParam.DocumentElement.SetAttribute("ClaveAcceso", objData.claveAcceso);
                    xmlParam.DocumentElement.SetAttribute("NombreArchivo", objData.nombreArchivo);
                }
                else
                {
                    xmlDocSipe = GeneraDocSipeXML(objData, lv_porcentaje, lv_Codigoporcentaje, false);
                }
                ds = objEjecucion.EjecucionGralDs(
                    new Object[6] {
                        xmlParam.OuterXml,
                        xmlDocSipe.OuterXml,
                        objData.xmlSRI,
                        objData.idDocumento,
                        "",
                        ""
                    },
                    605, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    string[] arrParamOut = new string[4];
                    arrParamOut[0] = Convert.ToInt32(ds.Tables[0].Rows[0]["PO_IdDocumento"]).ToString();
                    arrParamOut[1] = objData.nombreArchivo;
                    arrParamOut[2] = objData.claveAcceso;
                    arrParamOut[3] = objData.xmlSRI;
                    FormResponse.root.Add(arrParamOut);
                    if (objData.tipoACCION == "E")
                    {
                        string sError = "";
                        try
                        {
                            string rutaEnvio = (string)System.Configuration.ConfigurationManager.AppSettings["RutaEnvioXML"];
                            ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                            Proceso.copiaXMLRINE(objData.nombreArchivo + ".xml", objData.xmlSRI);
                            //File.WriteAllText(Path.Combine(rutaEnvio, objData.nombreArchivo + ".xml"), objData.xmlSRI);
                        }
                        catch (Exception ex)
                        {
                            sError = sError + " [No fue posible enviar el XML]: " + ex.Message;
                        }
                        try
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[0]["PO_NomArchivoTxt"]) != "")
                            {
                                string rutaEnvio = (string)System.Configuration.ConfigurationManager.AppSettings["RutaEnvioTXT"];
                                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                                Proceso.copiaTXTRINE(Convert.ToString(ds.Tables[0].Rows[0]["PO_NomArchivoTxt"]), Convert.ToString(ds.Tables[0].Rows[0]["PO_SalidaTxt"]));
                                //File.WriteAllText(Path.Combine(rutaEnvio, Convert.ToString(ds.Tables[0].Rows[0]["PO_NomArchivoTxt"])), Convert.ToString(ds.Tables[0].Rows[0]["PO_SalidaTxt"]));
                            }
                        }
                        catch (Exception ex)
                        {
                            sError = sError + " [No fue posible enviar el TXT]: " + ex.Message;
                        }
                        FormResponse.msgError = sError;
                    }
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        [AntiForgeryValidate]
        [ActionName("GrabaAnulaDocumento")]
        [HttpGet]
        public formResponseFacturas GetGrabaAnulaDocumento(string CodSap, string Ruc, string Usuario, string IdDocumentoAnula)
        {
            formResponseFacturas FormResponse = new formResponseFacturas();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("IdDocumento", IdDocumentoAnula);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 606, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (!ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        [AntiForgeryValidate]
        [ActionName("ValidaPermisoModFact")]
        [HttpGet]
        public formResponseFacturas GetValidaPermisoModFact(string CodSap, string Ruc, string Usuario, string NumPedido,
                                                            string FacEstabl, string FacPtoEmi, string FacNumSec)
        {
            formResponseFacturas FormResponse = new formResponseFacturas();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            string[] result = new string[1] { "N" };
            string NumFactura = "";
            try
            {
                NumFactura = FacEstabl + FacPtoEmi + "-" + FacNumSec;
                if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ValorDefault_ValidaModFact_SAP"]))
                {
                    // CONEXIONA A SAP ////////////////////////////////////////////////////////////
                    //InitialiseService.dest.Ping();
                    //RfcRepository repo = InitialiseService.dest.Repository;
                    AppConfig.dest.Ping();
                    RfcRepository repo = AppConfig.dest.Repository;
                    // CONEXIONA A RFC FACTURAS ///////////////////////////////////////////////////
                    IRfcFunction fnFact = repo.CreateFunction("ZPPFACTURACHECK");
                    IRfcStructure zsFacEdi = repo.GetStructureMetadata("ZWAPPFACTLISTA").CreateStructure();
                    zsFacEdi.SetValue("LIFNR", CodSap); //cod prov
                    zsFacEdi.SetValue("FACTURA", NumFactura); //num fact
                    zsFacEdi.SetValue("EBELN", NumPedido); //num pedido
                    zsFacEdi.SetValue("RECEP", " "); //indicador X = si se receptó
                    var zFactTtEdi = fnFact.GetTable("P_FALIST");
                    zFactTtEdi.Clear();
                    zFactTtEdi.Append(zsFacEdi);
                    fnFact.SetValue("P_FALIST", zFactTtEdi);
                    //fnFact.Invoke(InitialiseService.dest);
                    fnFact.Invoke(AppConfig.dest);
                    var FactList = fnFact.GetTable("P_FALIST");
                    for (int companyptr = 0; companyptr < FactList.RowCount; companyptr++)
                    {
                        FactList.CurrentIndex = companyptr;
                        if (ValidaCodigoRecibido(FactList.GetString("LIFNR")) == ValidaCodigoRecibido(CodSap) &&
                            FactList.GetString("FACTURA").Trim() == NumFactura &&
                            FactList.GetString("EBELN").Trim() == NumPedido &&
                            FactList.GetString("RECEP").Trim().ToUpper() != "X")
                        {
                            result[0] = "S";
                        }
                        else
                        {
                            result[0] = "N";
                        }
                    }
                }
                else
                {
                    result[0] = System.Configuration.ConfigurationManager.AppSettings["ValorDefault_ValidaModFact_SAP"];
                }
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
                result[0] = "N";
            }
            FormResponse.root.Add(result);
            return FormResponse;
        }


        //metodos para facturacion no electronica migrados del portal anterior de sipecom


        private XmlDocument GeneraDocSipeXML(facGrabaDocumento objDoc, string lv_porcentaje, string lv_Codigoporcentaje, bool AddFechaGeneracion)
        {
            XmlDocument oXML = new XmlDocument();
            try
            {
                XmlElement raiz = oXML.CreateElement("f");
                oXML.AppendChild(raiz);
                raiz.SetAttribute("rs", objDoc.nomProveedor.Trim());
                if (!objDoc.nomProveedor.Trim().Equals(objDoc.nomComercial.Trim()))
                    raiz.SetAttribute("nc", objDoc.nomComercial.Trim());
                else
                    raiz.SetAttribute("nc", objDoc.nomProveedor.Trim());
                raiz.SetAttribute("r", objDoc.rucProveedor.Trim());
                raiz.SetAttribute("tdoc", "01");
                raiz.SetAttribute("e", objDoc.facEstabl.Trim());
                raiz.SetAttribute("pe", objDoc.facPtoEmi.Trim());
                raiz.SetAttribute("s", objDoc.facNumSec.PadLeft(9, '0'));

                raiz.SetAttribute("dm", objDoc.dirMatriz.Trim());
                raiz.SetAttribute("fe", objDoc.fechaEmision.Trim());
                raiz.SetAttribute("fiva", objDoc.fechaIniVigAut.Trim());
                raiz.SetAttribute("ffva", objDoc.fechaFinVigAut.Trim());
                raiz.SetAttribute("oc", "SI");
                raiz.SetAttribute("na", objDoc.numAutorizaTal.Trim());

                raiz.SetAttribute("rsc", objDoc.nomEmpresa.Trim());
                raiz.SetAttribute("ic", objDoc.rucEmpresa.Trim());
                raiz.SetAttribute("tic", "04"); //Tipo Identificacion = RUC
                raiz.SetAttribute("tsi", RedondearSRI(ConvertStrToDec(objDoc.detSubTotSinImp) - ConvertStrToDec(objDoc.detTotDescuento)));
                raiz.SetAttribute("td", RedondearSRI(ConvertStrToDec(objDoc.detTotDescuento)));
                raiz.SetAttribute("hctd", "N");
                raiz.SetAttribute("p", RedondearSRI(ConvertStrToDec(objDoc.detPropina)));
                raiz.SetAttribute("it", RedondearSRI(ConvertStrToDec(objDoc.detValTotal)));
                if (objDoc.chkcompesacion)
                {
                    raiz.SetAttribute("cs", RedondearSRI(ConvertStrToDec(objDoc.txtcompesacion)));
                }
                raiz.SetAttribute("m", "DOLAR");

                raiz.SetAttribute("ds", objDoc.dirSucursal.Trim());
                raiz.SetAttribute("gr", "");

                XmlElement totalConImpuestos = default(XmlElement);
                if (ConvertStrToDec(objDoc.detSubTot_0) != ConvertStrToDec("0"))
                {
                    totalConImpuestos = oXML.CreateElement("tc");
                    totalConImpuestos.SetAttribute("c", "2");
                    totalConImpuestos.SetAttribute("cp", "0");
                    //totalConImpuestos.SetAttribute("t", "0");
                    totalConImpuestos.SetAttribute("bi", RedondearSRI(ConvertStrToDec(objDoc.detSubTot_0) - ConvertStrToDec(objDoc.detDescuento_0)));
                    totalConImpuestos.SetAttribute("v", RedondearSRI(ConvertStrToDec("0")));
                    raiz.AppendChild(totalConImpuestos);
                }
                if (ConvertStrToDec(objDoc.detSubTot_12) != ConvertStrToDec("0"))
                {
                    totalConImpuestos = oXML.CreateElement("tc");
                    totalConImpuestos.SetAttribute("c", "2");
                    totalConImpuestos.SetAttribute("cp", lv_Codigoporcentaje);
                    //totalConImpuestos.SetAttribute("t", "12");
                    totalConImpuestos.SetAttribute("bi", RedondearSRI(ConvertStrToDec(objDoc.detSubTot_12) - ConvertStrToDec(objDoc.detDescuento_12) + ConvertStrToDec(objDoc.detIce)));
                    totalConImpuestos.SetAttribute("v", RedondearSRI(ConvertStrToDec(objDoc.detIva_12)));
                    raiz.AppendChild(totalConImpuestos);
                }
                if (ConvertStrToDec(objDoc.detSubTotNoObjIva) != ConvertStrToDec("0"))
                {
                    totalConImpuestos = oXML.CreateElement("tc");
                    totalConImpuestos.SetAttribute("c", "2");
                    totalConImpuestos.SetAttribute("cp", "6");
                    //totalConImpuestos.SetAttribute("t", "0");
                    totalConImpuestos.SetAttribute("bi", RedondearSRI(ConvertStrToDec(objDoc.detSubTotNoObjIva)));
                    totalConImpuestos.SetAttribute("v", RedondearSRI(ConvertStrToDec("0")));
                    raiz.AppendChild(totalConImpuestos);
                }
                if (ConvertStrToDec(objDoc.detSubTotExenIva) != ConvertStrToDec("0"))
                {
                    totalConImpuestos = oXML.CreateElement("tc");
                    totalConImpuestos.SetAttribute("c", "2");
                    totalConImpuestos.SetAttribute("cp", "7");
                    //totalConImpuestos.SetAttribute("t", "0");
                    totalConImpuestos.SetAttribute("bi", RedondearSRI(ConvertStrToDec(objDoc.detSubTotExenIva)));
                    totalConImpuestos.SetAttribute("v", RedondearSRI(ConvertStrToDec("0")));
                    raiz.AppendChild(totalConImpuestos);
                }
                if (ConvertStrToDec(objDoc.detIce) != ConvertStrToDec("0"))
                {
                    totalConImpuestos = oXML.CreateElement("tc");
                    totalConImpuestos.SetAttribute("c", "3");
                    totalConImpuestos.SetAttribute("cp", "3660");
                    //totalConImpuestos.SetAttribute("t", "0");
                    totalConImpuestos.SetAttribute("bi", RedondearSRI(ConvertStrToDec(objDoc.detSubTot_12) - ConvertStrToDec(objDoc.detDescuento_12)));
                    totalConImpuestos.SetAttribute("v", RedondearSRI(ConvertStrToDec(objDoc.detIce)));
                    raiz.AppendChild(totalConImpuestos);
                }
                if (ConvertStrToDec(objDoc.detTotIrbpnr) != ConvertStrToDec("0"))
                {
                    totalConImpuestos = oXML.CreateElement("tc");
                    totalConImpuestos.SetAttribute("c", "5");
                    totalConImpuestos.SetAttribute("cp", "5001");
                    //totalConImpuestos.SetAttribute("t", "0");
                    totalConImpuestos.SetAttribute("bi", RedondearSRI(ConvertStrToDec("0")));
                    totalConImpuestos.SetAttribute("v", RedondearSRI(ConvertStrToDec(objDoc.detTotIrbpnr)));
                    raiz.AppendChild(totalConImpuestos);
                }

                XmlElement detalle = default(XmlElement);

                // INI - ITEMS O DETALLES DE LA FACTURA

                XmlElement impuesto;

                // DETALLE DE IVA 0%
                if (ConvertStrToDec(objDoc.detSubTot_0) > (decimal)0)
                {
                    detalle = oXML.CreateElement("d");
                    detalle.SetAttribute("cp", "99990");
                    detalle.SetAttribute("de", "PRODUCTO 0%");
                    detalle.SetAttribute("c", "1");
                    detalle.SetAttribute("pu", RedondearSRI(ConvertStrToDec(objDoc.detSubTot_0)));
                    detalle.SetAttribute("d", RedondearSRI(ConvertStrToDec(objDoc.detDescuento_0)));
                    detalle.SetAttribute("ptsi", RedondearSRI((ConvertStrToDec(objDoc.detSubTot_0) - ConvertStrToDec(objDoc.detDescuento_0))));
                    detalle.SetAttribute("dad", "");

                    impuesto = oXML.CreateElement("i");
                    detalle.AppendChild(impuesto);
                    impuesto.SetAttribute("c", "2");
                    impuesto.SetAttribute("cp", "0");
                    impuesto.SetAttribute("t", "0");
                    impuesto.SetAttribute("bi", RedondearSRI((ConvertStrToDec(objDoc.detSubTot_0) - ConvertStrToDec(objDoc.detDescuento_0))));
                    impuesto.SetAttribute("v", RedondearSRI((decimal)0));

                    if (ConvertStrToDec(objDoc.detIrbpnr_0) != ConvertStrToDec("0"))
                    {
                        impuesto = oXML.CreateElement("i");
                        detalle.AppendChild(impuesto);
                        impuesto.SetAttribute("c", "5");
                        impuesto.SetAttribute("cp", "5001");
                        impuesto.SetAttribute("t", "0");
                        impuesto.SetAttribute("bi", RedondearSRI(ConvertStrToDec("0")));
                        impuesto.SetAttribute("v", RedondearSRI(ConvertStrToDec(objDoc.detIrbpnr_0)));
                    }

                    raiz.AppendChild(detalle);
                }

                // DETALLE DE IVA 12%
                if (ConvertStrToDec(objDoc.detSubTot_12) > (decimal)0)
                {
                    detalle = oXML.CreateElement("d");
                    detalle.SetAttribute("cp", "99992");
                    detalle.SetAttribute("de", "PRODUCTO " + lv_porcentaje + "%");
                    detalle.SetAttribute("c", "1");
                    detalle.SetAttribute("pu", RedondearSRI(ConvertStrToDec(objDoc.detSubTot_12)));
                    detalle.SetAttribute("d", RedondearSRI(ConvertStrToDec(objDoc.detDescuento_12)));
                    detalle.SetAttribute("ptsi", RedondearSRI((ConvertStrToDec(objDoc.detSubTot_12) - ConvertStrToDec(objDoc.detDescuento_12))));
                    detalle.SetAttribute("dad", "");

                    impuesto = oXML.CreateElement("i");
                    detalle.AppendChild(impuesto);
                    impuesto.SetAttribute("c", "2");
                    impuesto.SetAttribute("cp", lv_Codigoporcentaje);
                    impuesto.SetAttribute("t", lv_porcentaje);
                    impuesto.SetAttribute("bi", RedondearSRI((ConvertStrToDec(objDoc.detSubTot_12) - ConvertStrToDec(objDoc.detDescuento_12) + ConvertStrToDec(objDoc.detIce))));
                    impuesto.SetAttribute("v", RedondearSRI(ConvertStrToDec(objDoc.detIva_12)));

                    if (ConvertStrToDec(objDoc.detIce) != ConvertStrToDec("0"))
                    {
                        impuesto = oXML.CreateElement("i");
                        detalle.AppendChild(impuesto);
                        impuesto.SetAttribute("c", "3");
                        impuesto.SetAttribute("cp", "3660");
                        impuesto.SetAttribute("t", "0");
                        impuesto.SetAttribute("bi", RedondearSRI((ConvertStrToDec(objDoc.detSubTot_12) - ConvertStrToDec(objDoc.detDescuento_12))));
                        impuesto.SetAttribute("v", RedondearSRI(ConvertStrToDec(objDoc.detIce)));
                    }

                    if (ConvertStrToDec(objDoc.detIrbpnr_12) != ConvertStrToDec("0"))
                    {
                        impuesto = oXML.CreateElement("i");
                        detalle.AppendChild(impuesto);
                        impuesto.SetAttribute("c", "5");
                        impuesto.SetAttribute("cp", "5001");
                        impuesto.SetAttribute("t", "0");
                        impuesto.SetAttribute("bi", RedondearSRI(ConvertStrToDec("0")));
                        impuesto.SetAttribute("v", RedondearSRI(ConvertStrToDec(objDoc.detIrbpnr_12)));
                    }

                    raiz.AppendChild(detalle);
                }
                // FIN - ITEMS O DETALLES DE LA FACTURA

                XmlElement infoAdicional = default(XmlElement);

                //agrego info adicional: Numero de Pedido
                infoAdicional = oXML.CreateElement("ia");
                infoAdicional.SetAttribute("n", "NumPedido");
                infoAdicional.SetAttribute("v", objDoc.numPedido);
                raiz.AppendChild(infoAdicional);
                //agrego info adicional: Almacen Origen - Codigo
                infoAdicional = oXML.CreateElement("ia");
                infoAdicional.SetAttribute("n", "AlmacOrigId");
                infoAdicional.SetAttribute("v", objDoc.codAlmacen);
                raiz.AppendChild(infoAdicional);
                //agrego info adicional: Almacen Origen - Descripcion
                infoAdicional = oXML.CreateElement("ia");
                infoAdicional.SetAttribute("n", "AlmacOrigDes");
                infoAdicional.SetAttribute("v", objDoc.nomAlmacen);
                raiz.AppendChild(infoAdicional);
                //agrego info adicional: Fecha Inicio Vigencia Autorización SRI
                infoAdicional = oXML.CreateElement("ia");
                infoAdicional.SetAttribute("n", "FechaIniVigAutSRI");
                infoAdicional.SetAttribute("v", objDoc.fechaIniVigAut.Trim());
                raiz.AppendChild(infoAdicional);
                //agrego info adicional: Fecha de Generacion en Portal
                if (AddFechaGeneracion)
                {
                    infoAdicional = oXML.CreateElement("ia");
                    infoAdicional.SetAttribute("n", "FechaGeneraPortal");
                    infoAdicional.SetAttribute("v", DateTime.Now.ToString(getDateFormatSite() + " " + getTimeFormatSite()));
                    raiz.AppendChild(infoAdicional);
                }

                oXML.AppendChild(raiz);

            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error en la generación del XML: " + ex.Message);
            }

            return oXML;
        }

        private Decimal ConvertStrToDec(string num)
        {
            System.Globalization.CultureInfo usCulture = new System.Globalization.CultureInfo("en-US");
            System.Globalization.NumberFormatInfo dbNumberFormat = usCulture.NumberFormat;
            return Decimal.Parse(num, dbNumberFormat);
        }
        private string RedondearSRI(decimal valor)
        {
            return decimal.Round(valor, 2).ToString("#0.00;-#0.00").Replace(",", ".");
        }
        private string getDateFormatSite()
        {
            return "dd/MM/yyyy";
        }
        private string getTimeFormatSite()
        {
            return "HH:mm:ss";
        }
        private string ValidaCodigoRecibido(string codigo)
        {
            int iTmp;
            string resp = (String.IsNullOrEmpty(codigo) ?
                "" :
                (!int.TryParse(codigo, out iTmp) ?
                    codigo : int.Parse(codigo).ToString()));
            return resp;
        }

        //private DateTime convertStrDDMMYYYYtoDateTime(string fecha)
        //{
        //    return new DateTime(Convert.ToInt32(fecha.Substring(6, 4)), Convert.ToInt32(fecha.Substring(3, 2)), Convert.ToInt32(fecha.Substring(0, 2)));
        //}

    }

}