using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Models;

using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Security.Claims;
using clibProveedores.Models;
using clibProveedores;
using clibSeguridadCR;
using Ionic.Zip;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp;
using PdfSharp.Drawing;
using System.Threading;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using AngularJSAuthentication.API.Handlers;
using Microsoft.Reporting.WebForms;
using GenCode128;
using BarcodeLib;
using System.Drawing;


namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/PedPedidosCross")]
    public class PedPedidosCrossController : ApiController
    {

        [ActionName("ActualizaPedidoCross")]
        [HttpPost]
        public formResponsePedidos getActualizaPedidoCross(pedidosCross PedidoCross)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            string estado = "";
            try
            {

                xmlParam.LoadXml("<Root />");
                foreach (var item in PedidoCross.p_detallePedidos)
                {
                    XmlElement elem = xmlParam.CreateElement("Detalle");
                    elem.SetAttribute("IdEmpresa", "1");
                    elem.SetAttribute("IdPedido", item.pIdPedido);
                    elem.SetAttribute("Almacen", item.pCodAlmacen);
                    elem.SetAttribute("Item", item.pItem);
                    elem.SetAttribute("CodArticulo", item.pCodArticulo);
                    elem.SetAttribute("PedidoSal", item.pNumPedidoSalida);
                    elem.SetAttribute("CantidadPlanificada", item.pCantidadDistribucion.ToString());
                    elem.SetAttribute("UnidadPlanificada", item.pUnidadDistribucion);
                    elem.SetAttribute("Cantidad", item.pCantidadRealDistribucion.ToString());
                    elem.SetAttribute("Unidad", item.pUnidadRealDistribucion);
                    elem.SetAttribute("Usuario", PedidoCross.p_usuario);
                    elem.SetAttribute("Estado", PedidoCross.p_estado);
                    estado = PedidoCross.p_estado;
                    xmlParam.DocumentElement.AppendChild(elem);

                }

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 508, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = true;
                    if (estado=="1")
                    {
                        XmlDocument xmlParamCross = new XmlDocument();
                        xmlParamCross.LoadXml("<DISTRIBUCIONCROSS />");
                        int i = 1;
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            XmlElement elem = xmlParamCross.CreateElement("DATOS");
                            elem.SetAttribute("Orden_compra", Convert.ToString(item["Orden_compra"]));
                            elem.SetAttribute("Posicion_item", Convert.ToString(item["Posicion_item"]));
                            elem.SetAttribute("Documento_salida", Convert.ToString(item["Documento_salida"]));
                            elem.SetAttribute("Posicion_salida", Convert.ToString(item["Posicion_salida"]));
                            elem.SetAttribute("Cantidad", Convert.ToString(item["Cantidad"]));
                            xmlParamCross.DocumentElement.AppendChild(elem);
                            i++;
                        }
                        ProcesoWs.ServBaseProceso ProcesoBase = new ProcesoWs.ServBaseProceso();
                        ProcesoBase.setSolpedidoCross(xmlParamCross, i);
                        FormResponse.success = true;  
                    }
                   
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }




            }
            catch (Exception e)
            {
                FormResponse.success = false;
                FormResponse.codError = "999";
                FormResponse.msgError = e.Message.ToString();
            }

            return FormResponse;
        }

        


        [AntiForgeryValidate]
        [ActionName("ConsPedidosCrossConsolidado")]
        [HttpGet]
        public formResponsePedidos GetConsPedidosCrossConsolidado(string CodSapco, string Fecha1co, string Fecha2co, string Ciudadco, string almacenco)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            List<pedReporteCross> TmpLstCons = new List<pedReporteCross>();
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
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodSapco);
                xmlParam.DocumentElement.SetAttribute("Fecha1", Fecha1co);
                xmlParam.DocumentElement.SetAttribute("Fecha2", Fecha2co);
                xmlParam.DocumentElement.SetAttribute("CodCiudad", Ciudadco);
                xmlParam.DocumentElement.SetAttribute("CodAlmacen", almacenco != null ? almacenco : "");
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 801, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedReporteCross TmpItem = new pedReporteCross();
                        TmpItem.codtienda = Convert.ToString(item["codtienda"]);
                        TmpItem.nomTienda = Convert.ToString(item["nomTienda"]);
                        TmpItem.ordenCompra = Convert.ToString(item["ordenCompra"]);
                        TmpItem.codArticulo = Convert.ToString(item["codArticulo"]);
                        TmpItem.nomArticulo = Convert.ToString(item["nomArticulo"]);
                        TmpItem.ordenSalida = Convert.ToString(item["ordenSalida"]);
                        TmpItem.cantidadaplanificar = Convert.ToString(item["cantidadaplanificar"]);
                        TmpItem.unidadplan = Convert.ToString(item["unidadplan"]);
                        TmpItem.cantidadPlanificada = Convert.ToString(item["cantidadPlanificada"]);
                        TmpItem.unidadplanreal = Convert.ToString(item["unidadplanreal"]);
                        TmpItem.unidadxcaja = Convert.ToString(item["unidadxcaja"]);
                        TmpLstCons.Add(TmpItem);
                    }
                    FormResponse.root.Add(TmpLstCons);
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
        [ActionName("ConsPedidosCrossConsolidadoExportar")]
        [HttpGet]
        public HttpResponseMessage ConsPedidosCrossConsolidadoExportar(string tiposo, string codsapex, string nomproveedorex, string usuariologonso, string idpedido,
                                                        string Ruc, string Opc1, string Opc2,
                                                        string Fecha1, string Fecha2, string Ciudad, string NumOrden,
                                                        bool SiGrd, bool SiTxt, bool SiXml, bool SiHtml, bool SiPdf, string almacen, bool isCross,string tipoPedido)
        {
            HttpResponseMessage result = null;
            string archivo = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", codsapex);
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", usuariologonso);
                xmlParam.DocumentElement.SetAttribute("Opcion1", Opc1);
                xmlParam.DocumentElement.SetAttribute("Opcion2", Opc2);
                xmlParam.DocumentElement.SetAttribute("Fecha1", Fecha1);
                xmlParam.DocumentElement.SetAttribute("Fecha2", Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodCiudad", Ciudad);
                xmlParam.DocumentElement.SetAttribute("NumPedido", NumOrden);
                xmlParam.DocumentElement.SetAttribute("CodAlmacen", almacen != null ? almacen : "");
                xmlParam.DocumentElement.SetAttribute("IsCross", isCross ? "1" : "0");
                xmlParam.DocumentElement.SetAttribute("tipoPedido", tipoPedido);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 801, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    ct.Columns.Add("CodProveedor", System.Type.GetType("System.String"));
                    ct.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = usuariologonso;
                    drowc["CodProveedor"] = codsapex;
                    drowc["NomProveedor"] = nomproveedorex;
                    ct.Rows.Add(drowc);


                    DataTable dt = new DataTable("Detallecab");
                    dt.Columns.Add("CodTienda", System.Type.GetType("System.String"));
                    dt.Columns.Add("Tienda", System.Type.GetType("System.String"));
                    dt.Columns.Add("OrdenCompra", System.Type.GetType("System.String"));
                    dt.Columns.Add("CodArticulo", System.Type.GetType("System.String"));
                    dt.Columns.Add("DescripcionArticulo", System.Type.GetType("System.String"));
                    dt.Columns.Add("Referencia", System.Type.GetType("System.String"));
                    dt.Columns.Add("NumeroDocumento", System.Type.GetType("System.String"));
                    dt.Columns.Add("Cantidadaplanificar", System.Type.GetType("System.String"));
                    dt.Columns.Add("Unidadplan", System.Type.GetType("System.String"));
                    dt.Columns.Add("CantidadPlanificada", System.Type.GetType("System.String"));
                    dt.Columns.Add("Unidadplanreal", System.Type.GetType("System.String"));
                    dt.Columns.Add("Unidadxcaja", System.Type.GetType("System.String"));
                    dt.Columns.Add("tipoPedido", System.Type.GetType("System.String"));

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["CodTienda"] = Convert.ToString(item["codtienda"]);
                        drowd["Tienda"] = Convert.ToString(item["nomTienda"]);
                        drowd["OrdenCompra"] = Convert.ToString(item["ordenCompra"]);
                        drowd["CodArticulo"] = Convert.ToString(item["codArticulo"]);
                        drowd["DescripcionArticulo"] = Convert.ToString(item["nomArticulo"]);
                        drowd["Referencia"] = Convert.ToString(item["Referencia"]);
                        drowd["NumeroDocumento"] = Convert.ToString(item["ordenSalida"]);
                        drowd["Cantidadaplanificar"] = Convert.ToString(item["cantidadaplanificar"]);
                        drowd["Unidadplan"] = Convert.ToString(item["unidadplan"]);
                        drowd["CantidadPlanificada"] = Convert.ToString(item["cantidadPlanificada"]);
                        drowd["Unidadplanreal"] = Convert.ToString(item["unidadplanreal"]);
                        drowd["Unidadxcaja"] = Convert.ToString(item["unidadxcaja"]);
                        drowd["tipoPedido"] = Convert.ToString(item["tipoPedido"]);
                        dt.Rows.Add(drowd);
                    }




                    rptDataSourcecab = new ReportDataSource("CabeceraCross", ct);
                    rptDataSourcedet = new ReportDataSource("DetalleCross", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteCrossConsolidado.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    if (tiposo == "1")
                    {
                        archivo = "ReporteCrossConsolidado" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                        bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                    }
                    if (tiposo == "2")
                    {
                        archivo = "ReporteCrossConsolidado" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
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
        [ActionName("ConsPedidosCrossFiltro")]
        [HttpGet]
        public formResponsePedidos GetConsPedidosCrossFiltro(string CodSap, string Ruc, string Usuario, string Opc1, string Opc2,
                                                        string Fecha1, string Fecha2, string Ciudad, string NumOrden,
                                                        bool SiGrd, bool SiTxt, bool SiXml, bool SiHtml, bool SiPdf, string almacen, bool isCross,string tipoPedido)
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
                xmlParam.DocumentElement.SetAttribute("IsCross", isCross ? "1" : "0");
                if (tipoPedido== "undefined")
                {
                    tipoPedido = "T";
                }
                xmlParam.DocumentElement.SetAttribute("tipoPedido", tipoPedido);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 506, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<pedConsPedidosF> TmpLstCons = new List<pedConsPedidosF>();
                    List<facConsSelPedidos> TmpLstConsp = new List<facConsSelPedidos>();
                    if (SiGrd)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            pedConsPedidosF TmpItem = new pedConsPedidosF();
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
                            TmpItem.pCantidadDistribucion = Convert.ToDecimal(item["CanPlanDistribucion"]);
                            TmpItem.pUnidadDistribucion = Convert.ToString(item["UniPlanDistribucion"]);
                            TmpItem.pCantidadRealDistribucion = Convert.ToDecimal(item["CanRealDistribucion"]);
                            TmpItem.pUnidadRealDistribucion = Convert.ToString(item["UniRealDistribucion"]);
                            TmpItem.pNumPedidoSalida = Convert.ToString(item["NumPedidoSal"]);
                            TmpItem.pEstado = Convert.ToString(item["Estado"]);
                            TmpItem.estadoDistri= Convert.ToString(item["estadoDistri"]);
                            TmpItem.tipoPedido = Convert.ToString(item["tipoPedido"]);
                            TmpLstCons.Add(TmpItem);
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
                            TmpItem.codProveedor = Convert.ToString(item["CodProveedor"]);
                            TmpItem.nomProveedor = Convert.ToString(item["NomProveedor"]);
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
                            TmpItem.cantidadTotalPedido = Convert.ToInt16(item["CantidadTotalPedido"]);
                            TmpItem.valorTotalPedido = Convert.ToDecimal(item["ValorTotalPedido"]);
                            TmpItem.totalSumaFacturas = Convert.ToDecimal(item["TotalSumaFacturas"]);
                            TmpItem.estado = Convert.ToString(item["Estado"]);
                            valPendPed = Convert.ToDecimal(item["ValorTotalPedido"]) - Convert.ToDecimal(item["TotalSumaFacturas"]);
                            TmpItem.valorPendPedido = valPendPed;
                            TmpItem.siValorPend = (valPendPed > (decimal)0 ? 'S' : 'N');
                            TmpItem.estadoDistri = Convert.ToString(item["estadoDistri"]);
                            TmpItem.tipoPedido = Convert.ToString(item["tipoPedido"]);
                            TmpLstConsp.Add(TmpItem);
                        }
                    }
                    TmpList.Add(TmpLstConsp);


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
        [ActionName("ConsPedidosCrossDetalleHis")]
        [HttpGet]
        public formResponsePedidos GetConsPedidosCrossDetalleHis(string NumOrden, string CodProveedor, string idPedido,
                                                        string Ruc, string Usuario, string Opc1, string Opc2,
                                                        string Fecha1, string Fecha2, string Ciudad,
                                                        bool SiGrd, bool SiTxt, bool SiXml, bool SiHtml, bool SiPdf, string almacen, bool isCross, string tipoPedido)
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
                //xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                //xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);
                //xmlParam.DocumentElement.SetAttribute("NumPedido", NumOrden);

                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);
                xmlParam.DocumentElement.SetAttribute("Opcion1", Opc1);
                xmlParam.DocumentElement.SetAttribute("Opcion2", Opc2);
                xmlParam.DocumentElement.SetAttribute("Fecha1", Fecha1);
                xmlParam.DocumentElement.SetAttribute("Fecha2", Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodCiudad", Ciudad);
                xmlParam.DocumentElement.SetAttribute("NumPedido", NumOrden);
                xmlParam.DocumentElement.SetAttribute("CodAlmacen", almacen != null ? almacen : "");
                xmlParam.DocumentElement.SetAttribute("IsCross", isCross ? "1" : "0");
                xmlParam.DocumentElement.SetAttribute("tipoPedidos", tipoPedido);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 509, 1);
                
                //ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 506, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<pedConsPedidosF> TmpLstCons = new List<pedConsPedidosF>();
                    if (tipoPedido=="C")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            pedConsPedidosF TmpItem = new pedConsPedidosF();
                            TmpItem.pNumPedido = Convert.ToString(item["NumPedido"]);
                            TmpItem.pIdPedido = Convert.ToString(item["IdPedido"]);
                            TmpItem.pCodProveedor = Convert.ToString(item["CodProveedor"]);
                            TmpItem.pNomProveedor = Convert.ToString(item["NomProveedor"]);
                            TmpItem.pCodAlmacen = Convert.ToString(item["CodAlmacen"]);
                            TmpItem.pNomAlmacen = Convert.ToString(item["NomAlmacen"]);
                            TmpItem.pCodArticulo = Convert.ToString(item["CodArticulo"]);
                            TmpItem.pDesArticulo = Convert.ToString(item["DesArticulo"]);
                            TmpItem.pCantidadDistribucion = Convert.ToDecimal(item["CanPlanDistribucion"]);
                            TmpItem.pUnidadDistribucion = Convert.ToString(item["UniPlanDistribucion"]);
                            TmpItem.pCantidadRealDistribucion = Convert.ToDecimal(item["CanRealDistribucion"]);
                            TmpItem.pUnidadRealDistribucion = Convert.ToString(item["UniRealDistribucion"]);
                            TmpItem.pEstado = Convert.ToString(item["Estado"]);
                            TmpItem.pEstadoDes = Convert.ToString(item["DesEstado"]);
                            TmpItem.pUsuario = Convert.ToString(item["Usuario"]);
                            TmpItem.pFechaRegistro = Convert.ToString(item["FechaRegistro"]);
                            TmpItem.pNumPedidoSalida = Convert.ToString(item["NumPedidoSal"]);
                            TmpItem.tipoPedido = Convert.ToString(item["TipoPedido"]);

                            TmpLstCons.Add(TmpItem);
                        }
                        FormResponse.root.Add(TmpLstCons);
                    }
                    if (tipoPedido == "F")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            pedConsPedidosF TmpItem = new pedConsPedidosF();
                            TmpItem.pFechaRegistro = Convert.ToString(item["FechaRegistro"]);
                            TmpItem.pNumPedido = Convert.ToString(item["NumPedido"]);
                            TmpItem.porcentaje = Convert.ToString(item["Porcentaje"]);
                            TmpItem.pCodArticulo = Convert.ToString(item["CodArticulo"]);
                            TmpItem.pDesArticulo = Convert.ToString(item["DesArticulo"]);
                            TmpItem.pCantidadDistribucion = Convert.ToDecimal(item["CanPlanDistribucion"]);
                            TmpItem.pCantidadRealDistribucion = Convert.ToDecimal(item["CanRealDistribucion"]);
                            TmpItem.pEstadoDes = Convert.ToString(item["DesEstado"]);
                            TmpItem.pUsuario = Convert.ToString(item["Usuario"]);
                            TmpItem.tipoPedido = Convert.ToString(item["TipoPedido"]);
                            TmpLstCons.Add(TmpItem);

                        }
                        FormResponse.root.Add(TmpLstCons);


                    }
                    if (tipoPedido == "T")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            pedConsPedidosF TmpItem = new pedConsPedidosF();
                            TmpItem.pNumPedido = Convert.ToString(item["NumPedido"]);
                            TmpItem.pIdPedido = Convert.ToString(item["IdPedido"]);
                            TmpItem.pCodProveedor = Convert.ToString(item["CodProveedor"]);
                            TmpItem.pNomProveedor = Convert.ToString(item["NomProveedor"]);
                            TmpItem.pCodAlmacen = Convert.ToString(item["CodAlmacen"]);
                            TmpItem.pNomAlmacen = Convert.ToString(item["NomAlmacen"]);
                            TmpItem.pCodArticulo = Convert.ToString(item["CodArticulo"]);
                            TmpItem.pDesArticulo = Convert.ToString(item["DesArticulo"]);
                            TmpItem.pCantidadDistribucion = Convert.ToDecimal(item["CanPlanDistribucion"]);
                            TmpItem.pUnidadDistribucion = Convert.ToString(item["UniPlanDistribucion"]);
                            TmpItem.pCantidadRealDistribucion = Convert.ToDecimal(item["CanRealDistribucion"]);
                            TmpItem.pUnidadRealDistribucion = Convert.ToString(item["UniRealDistribucion"]);
                            TmpItem.pEstado = Convert.ToString(item["Estado"]);
                            TmpItem.pEstadoDes = Convert.ToString(item["DesEstado"]);
                            TmpItem.pUsuario = Convert.ToString(item["Usuario"]);
                            TmpItem.pFechaRegistro = Convert.ToString(item["FechaRegistro"]);
                            TmpItem.pNumPedidoSalida = Convert.ToString(item["NumPedidoSal"]);
                            TmpItem.tipoPedido = Convert.ToString(item["TipoPedido"]);
                            TmpItem.porcentaje = Convert.ToString(item["Porcentaje"]);
                            TmpLstCons.Add(TmpItem);
                        }
                        FormResponse.root.Add(TmpLstCons);
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
        [ActionName("GeneraReportePackingList")]
        [HttpGet]
        public HttpResponseMessage GetGeneraReportePackingList(string NumOrden, string CodProveedor, string NomProveedor, string FechaPedido,
                                                                string TipoReporte, string Usuario,string Ruc, string Opc1, string Opc2,
                                                                string Fecha1, string Fecha2, string Ciudad, bool SiGrd, bool SiTxt,
                                                                bool SiXml, bool SiHtml, bool SiPdf, string almacen, bool isCross,string tipoPedido)
        {
            HttpResponseMessage result = null;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();

            try
            {

                // INI - CONSULTO a STORE PROCEDURE DE BD
                xmlParam.LoadXml("<Root />");
                //xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                //xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);
                //xmlParam.DocumentElement.SetAttribute("NumPedido", NumOrden);
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);
                xmlParam.DocumentElement.SetAttribute("Opcion1", Opc1);
                xmlParam.DocumentElement.SetAttribute("Opcion2", Opc2);
                xmlParam.DocumentElement.SetAttribute("Fecha1", Fecha1);
                xmlParam.DocumentElement.SetAttribute("Fecha2", Fecha2);
                xmlParam.DocumentElement.SetAttribute("CodCiudad", Ciudad);
                xmlParam.DocumentElement.SetAttribute("NumPedido", NumOrden);
                xmlParam.DocumentElement.SetAttribute("CodAlmacen", almacen != null ? almacen : "");
                xmlParam.DocumentElement.SetAttribute("IsCross", isCross ? "1" : "0");
                xmlParam.DocumentElement.SetAttribute("tipoPedidos", tipoPedido);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 507, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<pedConsPedidosF> TmpLstCons = new List<pedConsPedidosF>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedConsPedidosF TmpItem = new pedConsPedidosF();
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
                        TmpItem.pCantidadDistribucion = Convert.ToDecimal(item["CanPlanDistribucion"]);
                        TmpItem.pUnidadDistribucion = Convert.ToString(item["UniPlanDistribucion"]);
                        TmpItem.pCantidadRealDistribucion = Convert.ToDecimal(item["CanRealDistribucion"]);
                        TmpItem.pCantidadProveedor = Convert.ToDecimal(item["CanRealDistribucion"]);
                        TmpItem.pUnidadRealDistribucion = Convert.ToString(item["UniRealDistribucion"]);
                        TmpItem.pTolerancia = Convert.ToDecimal(item["Tolerancia"]);
                        TmpItem.pEstado = Convert.ToString(item["Estado"]);
                        TmpItem.pNumPedidoSalida = Convert.ToString(item["NumPedidoSal"]);
                        TmpItem.tipoPedido = Convert.ToString(item["TipoPedido"]);
                        TmpLstCons.Add(TmpItem);
                    }

                    //Datos para reporte de packing list
                    ReportDataSource rptDataSourcecab;
                    ReportDataSource rptDataSourcedet;
                    DataTable ct = new DataTable("Cabecera");
                    ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                    ct.Columns.Add("nomproveedor", System.Type.GetType("System.String"));
                    ct.Columns.Add("proveedor", System.Type.GetType("System.String"));
                    ct.Columns.Add("numOrden", System.Type.GetType("System.String"));
                    ct.Columns.Add("fecOrden", System.Type.GetType("System.String"));

                    DataRow drowc = ct.NewRow();
                    drowc["usuario"] = Usuario;
                    drowc["nomproveedor"] = NomProveedor;
                    drowc["proveedor"] = CodProveedor;
                    drowc["numOrden"] = NumOrden;
                    drowc["fecOrden"] = FechaPedido;
                    ct.Rows.Add(drowc);

                    DataTable dt = new DataTable("Detalle");
                    dt.Columns.Add("numOrden", System.Type.GetType("System.String"));
                    dt.Columns.Add("fecOrden", System.Type.GetType("System.String"));
                    dt.Columns.Add("CodigoArt", System.Type.GetType("System.String"));
                    dt.Columns.Add("DescripcionArt", System.Type.GetType("System.String"));
                    dt.Columns.Add("Referencia", System.Type.GetType("System.String"));
                    dt.Columns.Add("Almacen", System.Type.GetType("System.String"));
                    dt.Columns.Add("NomAlmacen", System.Type.GetType("System.String"));
                    dt.Columns.Add("Cantidad", System.Type.GetType("System.String"));
                    dt.Columns.Add("Unidad", System.Type.GetType("System.String"));
                    dt.Columns.Add("PedidoSal", System.Type.GetType("System.String"));
                    dt.Columns.Add("TipoPedido", System.Type.GetType("System.String"));
                    foreach (var item in TmpLstCons)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["numOrden"] = item.pNumPedido;
                        drowd["fecOrden"] = item.pFechaPedido.ToString().Substring(1,10);
                        drowd["CodigoArt"] = item.pCodArticulo;
                        drowd["DescripcionArt"] = item.pDesArticulo;
                        drowd["Referencia"] = item.pTamano;
                        drowd["Almacen"] = item.pCodAlmacen;
                        drowd["NomAlmacen"] = item.pNomAlmacen;
                        drowd["Cantidad"] = Convert.ToInt32(item.pCantidadRealDistribucion);
                        drowd["Unidad"] = item.pUnidadRealDistribucion;
                        drowd["PedidoSal"] = item.pNumPedidoSalida;
                        drowd["TipoPedido"] = item.tipoPedido;
                        dt.Rows.Add(drowd);
                    }

                    rptDataSourcecab = new ReportDataSource("RPTPackingListCab", ct);
                    rptDataSourcedet = new ReportDataSource("RPTPackingListDet", dt);
                    ReportViewer auxc = new ReportViewer();
                    Warning[] warnings = null;
                    string[] streamids = null;
                    string archivo = "";
                    string mimeType = "";
                    string encoding = "";
                    string extension = "";
                    auxc.ProcessingMode = ProcessingMode.Local;
                    auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReportePackingList.rdlc");
                    auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                    auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                    byte[] bytes = null;
                    if (TipoReporte == "1")
                    {
                        archivo = "ReportePackingList_" + CodProveedor + "_" + NumOrden + ".pdf";
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
        [ActionName("ConsPedidosCrossDetalle")]
        [HttpGet]
        public formResponsePedidos GetConsPedidosCrossDetalle(string NumOrden, string CodProveedor)
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
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);
                xmlParam.DocumentElement.SetAttribute("NumPedido", NumOrden);
                xmlParam.DocumentElement.SetAttribute("Opcion1", "O");
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 507, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<pedConsPedidosF> TmpLstCons = new List<pedConsPedidosF>();


                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedConsPedidosF TmpItem = new pedConsPedidosF();
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
                        TmpItem.pCantidadDistribucion = Convert.ToDecimal(item["CanPlanDistribucion"]);
                        TmpItem.pUnidadDistribucion = Convert.ToString(item["UniPlanDistribucion"]);
                        TmpItem.pCantidadRealDistribucion = Convert.ToDecimal(item["CanRealDistribucion"]);
                        TmpItem.pCantidadProveedor = Convert.ToDecimal(item["CanRealDistribucion"]);
                        TmpItem.pUnidadRealDistribucion = Convert.ToString(item["UniRealDistribucion"]);
                        TmpItem.pNumPedidoSalida = Convert.ToString(item["NumPedidoSal"]);
                        TmpItem.pTolerancia = Convert.ToDecimal(item["Tolerancia"]);
                        TmpItem.pEstado = Convert.ToString(item["Estado"]);
                        TmpLstCons.Add(TmpItem);
                    }

                    FormResponse.root.Add(TmpLstCons);
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

        [ActionName("ConsPedidosCrossDetalleCrossFlow")]
        [HttpGet]
        public formResponsePedidos GetConsPedidosCrossDetalleCrossFlow(string NumOrden, string CodProveedor, string tipoPedidos)
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
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedor);
                xmlParam.DocumentElement.SetAttribute("NumPedido", NumOrden);
                xmlParam.DocumentElement.SetAttribute("tipoPedidos", tipoPedidos);
                xmlParam.DocumentElement.SetAttribute("Opcion1", "O");
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 609, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<pedConsPedidosF> TmpLstCons = new List<pedConsPedidosF>();


                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedConsPedidosF TmpItem = new pedConsPedidosF();
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
                        TmpItem.pCantidadDistribucion = Convert.ToDecimal(item["CanPlanDistribucion"]);
                        TmpItem.pUnidadDistribucion = Convert.ToString(item["UniPlanDistribucion"]);
                        TmpItem.pCantidadRealDistribucion = Convert.ToDecimal(item["CanRealDistribucion"]);
                        TmpItem.pCantidadProveedor = Convert.ToDecimal(item["CanRealDistribucion"]);
                        TmpItem.pUnidadRealDistribucion = Convert.ToString(item["UniRealDistribucion"]);
                        TmpItem.pNumPedidoSalida = Convert.ToString(item["NumPedidoSal"]);
                        TmpItem.pTolerancia = Convert.ToDecimal(item["Tolerancia"]);
                        TmpItem.pEstado = Convert.ToString(item["Estado"]);
                        TmpItem.real = Convert.ToString(item["real"]);
                        TmpLstCons.Add(TmpItem);
                    }

                    FormResponse.root.Add(TmpLstCons);
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
    }
}