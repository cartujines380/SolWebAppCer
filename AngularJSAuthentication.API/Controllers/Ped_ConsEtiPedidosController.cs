using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Data;
using System.Xml;
using clibProveedores.Models;
using clibProveedores;
using AngularJSAuthentication.API.Handlers;
using Microsoft.Reporting.WebForms;
using System.Web;
using System.IO;

namespace AngularJSAuthentication.API.Controllers
{
    
    [RoutePrefix("api/PedConsEtiPedidos")]
    public class PedConsEtiPedidosController : ApiController
    {


        [AntiForgeryValidate]
        [ActionName("getBuscarAsignacion")]
        [HttpGet]
        public string BuscarAsignacion(string opcion, string codsap)
        {
            string FormResponse = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("OPCION", opcion);
                xmlParam.DocumentElement.SetAttribute("CODSAP", codsap);

                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 800, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<pedConsSolicitud> TmpLstCons = new List<pedConsSolicitud>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        FormResponse = Convert.ToString(item["RETORNO"]);
                    }
                }
                else
                {
                    FormResponse = "100";
                }
            }
            catch (Exception ex)
            {
                FormResponse = "500";
            }
            return FormResponse;
        }


        [AntiForgeryValidate]
        [ActionName("getPedidoSolicitud")]
        [HttpGet]
        public formResponsePedidos PedidoSolicitud(string idSolicitud, string opcion)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
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
                xmlParam.DocumentElement.SetAttribute("OPCION", opcion);
                xmlParam.DocumentElement.SetAttribute("IDSOLICITUD", idSolicitud);

                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 800, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<pedConsSolicitud> TmpLstCons = new List<pedConsSolicitud>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedConsSolicitud TmpItem = new pedConsSolicitud(); //pedConsPedidosEtiF
                        TmpItem.pNumPedido = Convert.ToString(item["pNumPedido"]);
                        TmpItem.pItem = Convert.ToString(item["pItem"]);
                        TmpItem.pCantPedido = Convert.ToDecimal(item["pCantPedido"]);
                        TmpItem.pCodArticulo = Convert.ToString(item["pCodArticulo"]);
                        TmpItem.pTamano = Convert.ToString(item["pTamano"]);
                        TmpItem.pTamanoCaja = Convert.ToString(item["pTamanoCaja"]);
                        TmpItem.pCodEAN = Convert.ToString(item["pCodEAN"]);
                        TmpItem.pDesArticulo = Convert.ToString(item["pDesArticulo"]);
                        TmpItem.pDesArticulo = Convert.ToString(item["pDesArticulo"]);
                        TmpItem.pPrecioCosto = Convert.ToString(item["pPrecioCosto"]);
                        TmpItem.catDesp = Convert.ToDecimal(item["catDesp"]);
                        TmpItem.saldo = Convert.ToDecimal(item["saldo"]);
                        TmpLstCons.Add(TmpItem);
                    }

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
        [ActionName("getPedidoSolicitudprin")]
        [HttpGet]
        public formResponsePedidos PedidoSolicitudprin(string idSolicitudpri, string opcionpri)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
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
                xmlParam.DocumentElement.SetAttribute("OPCION", opcionpri);
                xmlParam.DocumentElement.SetAttribute("IDSOLICITUD", idSolicitudpri);

                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 800, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<pedConsPedidosF> TmpLstCons = new List<pedConsPedidosF>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedConsPedidosF TmpItem = new pedConsPedidosF(); //pedConsPedidosEtiF
                        TmpItem.pOrigen = Convert.ToString(item["Origen"]);
                        TmpItem.pIdPedido = Convert.ToString(item["IdPedido"]);
                        TmpItem.pNumPedido = Convert.ToString(item["NumPedido"]);
                        TmpItem.pCodAlmacen = Convert.ToString(item["CodAlmacen"]);
                        TmpItem.pNomAlmacen = Convert.ToString(item["NomAlmacen"]);
                        TmpItem.pFechaPedido = Convert.ToDateTime(item["FechaPedido"]);
                        TmpItem.pCodAlmDestino = Convert.ToString(item["CodAlmDestino"]);
                        TmpItem.pCodProveedor = Convert.ToString(item["CodProveedor"]);
                        TmpItem.pNomProveedor = Convert.ToString(item["NomProveedor"]);
                        TmpItem.pZonaOrigen = Convert.ToString(item["ZonaOrigen"]);
                        TmpItem.pItem = Convert.ToString(item["Item"]);
                        TmpItem.pCodArticulo = Convert.ToString(item["CodArticulo"]);
                        TmpItem.pDesArticulo = Convert.ToString(item["DesArticulo"]);
                        TmpItem.pTamano = Convert.ToString(item["Tamano"]);
                        TmpItem.pCantPedido = Convert.ToDecimal(item["CantPedido"]);
                        TmpItem.pPrecioCosto = Convert.ToDecimal(item["PrecioCosto"]);
                        TmpItem.pUndPorCaja = Convert.ToString(item["UndPorCaja"]);
                        TmpItem.pDescuento1 = Convert.ToDecimal(item["Descuento1"]);
                        TmpItem.pDescuento2 = Convert.ToDecimal(item["Descuento2"]);
                        TmpItem.pIndIva1 = Convert.ToString(item["IndIva1"]);
                        TmpItem.pTamanoCaja = Convert.ToString(item["TamanoCaja"]);
                        TmpItem.pCodEAN = Convert.ToString(item["CodEAN"]);
                        TmpItem.esDescargado = Convert.ToString(item["EsDescargado"]);
                        TmpItem.esImpreso = Convert.ToString(item["EsImpreso"]);
                        TmpItem.catDesp = Convert.ToDecimal(item["catDesp"]);
                        TmpItem.estadodistri = Convert.ToString(item["estadodistri"]);
                        TmpItem.tipoPedido = Convert.ToString(item["tipoPedido"]); 
                        TmpLstCons.Add(TmpItem);
                    }

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
        [ActionName("ConsPedidosEtiFiltro")]
        [HttpGet]
        public formResponsePedidos GetConsPedidosEtiFiltro(string CodSap, string Ruc, string Usuario, string Opc1, string Opc2,
                                                       string Fecha1, string Fecha2, string Ciudad, string NumOrden,
                                                       bool SiGrd, bool SiTxt, bool SiXml, bool SiHtml, bool SiPdf, string almacen,string tipoPedido)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
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
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);
                xmlParam.DocumentElement.SetAttribute("Opcion1", Opc1);
                xmlParam.DocumentElement.SetAttribute("Opcion2", Opc2);
                xmlParam.DocumentElement.SetAttribute("Fecha1", Fecha1);
                xmlParam.DocumentElement.SetAttribute("Fecha2", Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodCiudad", Ciudad);
                xmlParam.DocumentElement.SetAttribute("NumPedido", NumOrden);
                xmlParam.DocumentElement.SetAttribute("CodAlmacen", almacen != null ? almacen : "");
                xmlParam.DocumentElement.SetAttribute("tipoPedido", tipoPedido); 
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 701, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<pedConsPedidosF> TmpLstCons = new List<pedConsPedidosF>();
                    List<pedConsPedidosEtiF> TmpLstEtiCons = new List<pedConsPedidosEtiF>();
                    List<facConsSelPedidos> TmpLstConsp = new List<facConsSelPedidos>();
                    if (SiGrd)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            pedConsPedidosF TmpItem = new pedConsPedidosF(); //pedConsPedidosEtiF
                            TmpItem.pOrigen = Convert.ToString(item["Origen"]);
                            TmpItem.pIdPedido = Convert.ToString(item["IdPedido"]);
                            TmpItem.pNumPedido = Convert.ToString(item["NumPedido"]);
                            TmpItem.pCodAlmacen = Convert.ToString(item["CodAlmacen"]);
                            TmpItem.pNomAlmacen = Convert.ToString(item["NomAlmacen"]);
                            TmpItem.pFechaPedido = Convert.ToDateTime(item["FechaPedido"]);
                            TmpItem.pCodAlmDestino = Convert.ToString(item["CodAlmDestino"]);
                            TmpItem.pCodProveedor = Convert.ToString(item["CodProveedor"]);
                            TmpItem.pNomProveedor = Convert.ToString(item["NomProveedor"]);
                            TmpItem.pZonaOrigen = Convert.ToString(item["ZonaOrigen"]);
                            TmpItem.pItem = Convert.ToString(item["Item"]);
                            TmpItem.pCodArticulo = Convert.ToString(item["CodArticulo"]);
                            TmpItem.pDesArticulo = Convert.ToString(item["DesArticulo"]);
                            TmpItem.pTamano = Convert.ToString(item["Tamano"]);
                            TmpItem.pCantPedido = Convert.ToDecimal(item["CantPedido"]);
                            TmpItem.pPrecioCosto = Convert.ToDecimal(item["PrecioCosto"]);
                            TmpItem.pUndPorCaja = Convert.ToString(item["UndPorCaja"]);
                            TmpItem.pDescuento1 = Convert.ToDecimal(item["Descuento1"]);
                            TmpItem.pDescuento2 = Convert.ToDecimal(item["Descuento2"]);
                            TmpItem.pIndIva1 = Convert.ToString(item["IndIva1"]);
                            TmpItem.pTamanoCaja = Convert.ToString(item["TamanoCaja"]);
                            TmpItem.pCodEAN = Convert.ToString(item["CodEAN"]);
                            TmpItem.esDescargado = Convert.ToString(item["EsDescargado"]);
                            TmpItem.esImpreso = Convert.ToString(item["EsImpreso"]);
                            TmpItem.tipoPedido = Convert.ToString(item["tipoPedido"]);
                            TmpLstCons.Add(TmpItem);
                        }

                        foreach (DataRow item in ds.Tables[2].Rows)
                        {
                            pedConsPedidosEtiF TmpItem = new pedConsPedidosEtiF();
                            TmpItem.idSolEtiqueta = Convert.ToString(item["idSolEtiqueta"]);
                            TmpItem.idPedido = Convert.ToString(item["idPedido"]);
                            TmpItem.NumPedido = Convert.ToString(item["NumPedido"]);
                            TmpItem.codArticulo = Convert.ToString(item["codArticulo"]);
                            TmpItem.CanDespachar = Convert.ToInt32(item["CanDespachar"]);
                            TmpItem.Estado = Convert.ToString(item["Estado"]);
                            TmpItem.fechaEntrega = Convert.ToString(item["FechaEntregaFormato"]);
                            TmpItem.detalle = Convert.ToString(item["DesArticulo"]);
                            TmpLstEtiCons.Add(TmpItem);
                        }
                    }
                    TmpList.Add(TmpLstCons);
                    if (SiGrd)
                    {
                        decimal valPendPed;
                        foreach (DataRow item in ds.Tables[1].Rows)
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
                            TmpItem.tipoPedido = Convert.ToString(item["tipoPedido"]);
                            TmpLstConsp.Add(TmpItem);
                        }
                    }
                    TmpList.Add(TmpLstConsp);
                    TmpList.Add(TmpLstEtiCons);
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
        [ActionName("geteliminarSolicitudEti")]
        [HttpGet]
        public int eliminarSolicitudEti(string opcionEli, string idPedidoEli, string estadoEli)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            int retorno = 0;
            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("OPCION", opcionEli);
                xmlParam.DocumentElement.SetAttribute("IDSOLICITUD", idPedidoEli);
                xmlParam.DocumentElement.SetAttribute("ESTADO", estadoEli);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 800, 1);

                retorno = Convert.ToInt16(ds.Tables[0].Rows[0]["retorno"]);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return retorno;
        }

        [AntiForgeryValidate]
        [ActionName("mantSolicitudEti")]
        [HttpPost]
        public string mantSolicitudEti(Gra_pedConsPedidosEtiF Etiquetas)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            string retorno = "";
            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("OPCION", Etiquetas.p_Etiquetas[0].opcion);
                xmlParam.DocumentElement.SetAttribute("IDPEDIDO", Etiquetas.p_Etiquetas[0].idPedido);
                xmlParam.DocumentElement.SetAttribute("NUMPEDIDO", Etiquetas.p_Etiquetas[0].NumPedido);
                xmlParam.DocumentElement.SetAttribute("FECHA", Etiquetas.p_Etiquetas[0].fechaEntrega);
                xmlParam.DocumentElement.SetAttribute("CODSAP", Etiquetas.p_Etiquetas[0].codsap);
                if (Etiquetas.p_Etiquetas != null)
                {
                    foreach (Gra_pedConsPedidosEtiF.pedConsPedidosEtiFGrar dr in Etiquetas.p_Etiquetas)
                    {
                        XmlElement elem = xmlParam.CreateElement("EtiquetaDet");
                        elem.SetAttribute("IDSOLICITUD", dr.idSolEtiqueta);
                        elem.SetAttribute("CODARTICULO", dr.codArticulo);
                        elem.SetAttribute("CANDESPACHAR", Convert.ToString(dr.CanDespachar));
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }


                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 800, 1);

                retorno = Convert.ToString(ds.Tables[0].Rows[0]["retorno"]);
            }
            catch (Exception ex)
            {
                return "0";
            }
            return retorno;
        }

        [AntiForgeryValidate]
        [ActionName("consSolicitudEti")]
        [HttpGet]
        public List<object> consSolicitudEti(string Opc, string idPed, string codArt)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<Object> lista = new List<Object>();
            List<pedConsPedidosEtiF> listaFilt = new List<pedConsPedidosEtiF>();
            List<pedConsPedidosEtiF> listasnFilt = new List<pedConsPedidosEtiF>();

            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("OPCION", Opc);
                xmlParam.DocumentElement.SetAttribute("IDPEDIDO", idPed);
                xmlParam.DocumentElement.SetAttribute("CODARTICULO", codArt);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 800, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedConsPedidosEtiF TmpItem = new pedConsPedidosEtiF();
                        TmpItem.idSolEtiqueta = Convert.ToString(item["idSolEtiqueta"]);
                        TmpItem.idPedido = Convert.ToString(item["idPedido"]);
                        TmpItem.NumPedido = Convert.ToString(item["NumPedido"]);
                        TmpItem.codArticulo = Convert.ToString(item["codArticulo"]);
                        TmpItem.CanDespachar = Convert.ToInt32(item["CanDespachar"]);
                        TmpItem.Estado = Convert.ToString(item["Estado"]);
                        TmpItem.fechaEntrega = Convert.ToString(item["FechaEntregaFormato"]);
                        TmpItem.detalle = Convert.ToString(item["DesArticulo"]);

                        listaFilt.Add(TmpItem);
                    }

                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        pedConsPedidosEtiF TmpItem = new pedConsPedidosEtiF();
                        TmpItem.idSolEtiqueta = Convert.ToString(item["idSolEtiqueta"]);
                        TmpItem.idPedido = Convert.ToString(item["idPedido"]);
                        TmpItem.NumPedido = Convert.ToString(item["NumPedido"]);
                        TmpItem.codArticulo = Convert.ToString(item["codArticulo"]);
                        TmpItem.CanDespachar = Convert.ToInt32(item["CanDespachar"]);
                        TmpItem.Estado = Convert.ToString(item["Estado"]);
                        TmpItem.fechaEntrega = Convert.ToString(item["FechaEntregaFormato"]);
                        TmpItem.detalle = Convert.ToString(item["DesArticulo"]);

                        listasnFilt.Add(TmpItem);
                    }
                    lista.Add(listaFilt);
                    lista.Add(listasnFilt);
                }
            }
            catch (Exception ex)
            {
                return lista;
            }
            return lista;
        }


        [ActionName("ExportarDataImpresas")]
        [HttpGet]
        public HttpResponseMessage ExportarDataImpresas(string tipoEt,string usuarioEt,string nombreEmp,string Fecha1Et, string Fecha2Et, string CodSapEt)
        {
            HttpResponseMessage result = null;
            DataSet ds = new DataSet();
            string archivo = "";
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repVentaxCentro> lst_retornoVentaCentro = new List<repVentaxCentro>();
            List<repcodigoArticulo> lst_codigoImagen = new List<repcodigoArticulo>();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("OPCION", "R1");
                xmlParam.DocumentElement.SetAttribute("FECHA", Fecha1Et);
                xmlParam.DocumentElement.SetAttribute("FECHA2", Fecha2Et);
                xmlParam.DocumentElement.SetAttribute("CODSAP", CodSapEt);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 800, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    ct.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = usuarioEt;
                    drowc["NomProveedor"] = nombreEmp;
                    ct.Rows.Add(drowc);


                    DataTable cd = new DataTable("CabeceraImpresas");
                    cd.Columns.Add("fechaEntrega", System.Type.GetType("System.String"));
                    cd.Columns.Add("numPedido", System.Type.GetType("System.String"));
                    cd.Columns.Add("etiquetaCarton", System.Type.GetType("System.Int16"));
                    cd.Columns.Add("etiquetaAdhesivas", System.Type.GetType("System.Int16"));
                    cd.Columns.Add("etiquetaRFID", System.Type.GetType("System.Int16"));
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        DataRow drowcd = cd.NewRow();
                        drowcd["fechaEntrega"] = Convert.ToString(item["fechaEntrega"]);
                        drowcd["numPedido"] = Convert.ToString(item["numPedido"]);
                        drowcd["etiquetaCarton"] = Convert.ToInt16(item["etiquetaCarton"]);
                        drowcd["etiquetaAdhesivas"] = Convert.ToInt16(item["etiquetaAdhesivas"]);
                        drowcd["etiquetaRFID"] = Convert.ToInt16(item["etiquetaRFID"]);
                        cd.Rows.Add(drowcd);
                    }

                    rptDataSourcecab = new ReportDataSource("Cabecera", ct);
                    rptDataSourcedet = new ReportDataSource("DetalleImpresas", cd);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/RptImpresas.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    if (tipoEt=="1")
                    {
                        archivo = "Reporte" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                        bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }
                    if (tipoEt == "2")
                    {
                        archivo = "Reporte" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                        bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }
                   
                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new MemoryStream(bytes));
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = archivo;
                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        [AntiForgeryValidate]
        [ActionName("consPedEtiImpresas")]
        [HttpGet]
        public formResponsePedidos consPedEtiImpresas(string Fecha1, string Fecha2,  string codigosap)
        {
            DataSet ds = new DataSet();

            formResponsePedidos fr = new formResponsePedidos();

            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<Object> lista = new List<Object>();
            List<pedConsPedidosEtiImpresar> listaFilt = new List<pedConsPedidosEtiImpresar>();

            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("OPCION", "R1");
                xmlParam.DocumentElement.SetAttribute("FECHA", Fecha1);
                xmlParam.DocumentElement.SetAttribute("FECHA2", Fecha2);
                xmlParam.DocumentElement.SetAttribute("CODSAP", codigosap);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 800, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedConsPedidosEtiImpresar TmpItem = new pedConsPedidosEtiImpresar();
                        TmpItem.fechaEntrega = Convert.ToString(item["fechaEntrega"]);
                        TmpItem.numPedido = Convert.ToString(item["numPedido"]);
                        TmpItem.etiquetaCarton = Convert.ToString(item["etiquetaCarton"]);
                        TmpItem.etiquetaAdhesivas = Convert.ToString(item["etiquetaAdhesivas"]);
                        TmpItem.etiquetaRFID = Convert.ToString(item["etiquetaRFID"]);
                        listaFilt.Add(TmpItem);
                    }
                    fr.success = true;
                    fr.root.Add(listaFilt);
                }
            }
            catch (Exception ex)
            {
                fr.success = false;
                fr.codError = "-1";
                fr.msgError = ex.Message;
            }
            return fr;
        }

        [AntiForgeryValidate]
        [ActionName("consPedEti")]
        [HttpGet]
        public formResponsePedidos consPedEti(string Opc1, string Fecha1, string Fecha2, string Estado, string NumOrden, string codigosap)
        {
            DataSet ds = new DataSet();

            formResponsePedidos fr = new formResponsePedidos();

            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<Object> lista = new List<Object>();
            List<pedConsPedidosEtiF> listaFilt = new List<pedConsPedidosEtiF>();

            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("OPCION", Opc1);
                xmlParam.DocumentElement.SetAttribute("FECHA", Fecha1);
                xmlParam.DocumentElement.SetAttribute("FECHA2", Fecha2);
                xmlParam.DocumentElement.SetAttribute("ESTADO", Estado);
                xmlParam.DocumentElement.SetAttribute("NUMPEDIDO", NumOrden);
                xmlParam.DocumentElement.SetAttribute("CODSAP", codigosap);
                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 800, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedConsPedidosEtiF TmpItem = new pedConsPedidosEtiF();
                        TmpItem.idSolEtiqueta = Convert.ToString(item["idSolEtiqueta"]);
                        TmpItem.idPedido = Convert.ToString(item["idPedido"]);
                        TmpItem.NumPedido = Convert.ToString(item["NumPedido"]);
                        TmpItem.codArticulo = Convert.ToString(item["codArticulo"]);
                        TmpItem.CanDespachar = Convert.ToInt32(item["CanDespachar"]);
                        TmpItem.Estado = Convert.ToString(item["EstadoFormato"]);
                        TmpItem.fechaEntrega = Convert.ToString(item["FechaEntregaFormato"]);
                        TmpItem.detalle = Convert.ToString(item["DesArticulo"]);
                        TmpItem.codestado = Convert.ToString(item["estado"]);
                        listaFilt.Add(TmpItem);
                    }
                    fr.success = true;
                    fr.root.Add(listaFilt);
                }
            }
            catch (Exception ex)
            {
                fr.success = false;
                fr.codError = "-1";
                fr.msgError = ex.Message;
            }
            return fr;
        }
    }
}

