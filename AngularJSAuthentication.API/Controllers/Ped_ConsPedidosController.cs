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
    [RoutePrefix("api/PedConsPedidos")]
    public class PedConsPedidosController : ApiController
    {
         [AntiForgeryValidate]
        [ActionName("ConsCiudadesEnAlmacen")]
        [HttpGet]
        public formResponsePedidos GetConsCiudadesEnAlmacen()
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                ds = objEjecucion.EjecucionGralDs("", 501, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<pedCiudadesAlmac> TmpList = new List<pedCiudadesAlmac>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedCiudadesAlmac TmpItem = new pedCiudadesAlmac();
                        TmpItem.pCodCiudad = Convert.ToString(item["CodCiudad"]);
                        TmpItem.pNomCiudad = Convert.ToString(item["NomCiudad"]);
                        TmpList.Add(TmpItem);
                    }
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
        [ActionName("ConsPedidosFiltro")]
        [HttpGet]
        public formResponsePedidos GetConsPedidosFiltro(string CodSap, string Ruc, string Usuario, string Opc1, string Opc2,
                                                        string Fecha1, string Fecha2, string Ciudad, string NumOrden,
                                                        bool SiGrd, bool SiTxt, bool SiXml, bool SiHtml, bool SiPdf, string almacen)
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
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 502, 1);
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
                            TmpItem.fecEntregaActual = Convert.ToDateTime(item["FecEntregaActual"]);
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
                            TmpItem.fecEntregaActual = Convert.ToDateTime(item["FecEntregaActual"]);
                            TmpLstConsp.Add(TmpItem);
                        }
                    }
                    TmpList.Add(TmpLstConsp);
                    // FIN - ARMADO DE ESTRUCTURA PARA GRID
                    // INI - ARMADO DE ARCHIVO DE TEXTO
                    //string strRespTxt = "";
                    //if (SiTxt)
                    //{
                    //    if (ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        System.Text.StringBuilder sbTxtPed = new System.Text.StringBuilder("");
                    //        foreach (DataRow item in ds.Tables[0].Rows)
                    //        {
                    //            sbTxtPed.Append((Convert.ToString(item["CodProveedor"]).Trim() + new string(' ', 10)).Substring(0, 10));
                    //            sbTxtPed.Append((Convert.ToString(item["NomProveedor"]).Trim() + new string(' ', 40)).Substring(0, 40));
                    //            sbTxtPed.Append((Convert.ToString(item["NumPedido"]).Trim() + new string(' ', 10)).Substring(0, 10));
                    //            sbTxtPed.Append((Convert.ToString(item["Item"]).Trim() + new string(' ', 5)).Substring(0, 5));
                    //            sbTxtPed.Append((Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") + new string(' ', 10)).Substring(0, 10));
                    //            sbTxtPed.Append((Convert.ToDecimal(item["CantPedido"]).ToString("##0.00").Replace(",", ".") + new string(' ', 12)).Substring(0, 12));
                    //            sbTxtPed.Append((Convert.ToString(item["CodArticulo"]).Trim() + new string(' ', 18)).Substring(0, 18));
                    //            sbTxtPed.Append((Convert.ToString(item["Tamano"]).Trim() + new string(' ', 18)).Substring(0, 18));
                    //            sbTxtPed.Append((Convert.ToString(item["TamanoCaja"]).Trim() + new string(' ', 10)).Substring(0, 10));
                    //            sbTxtPed.Append((Convert.ToString(item["CodEAN"]).Trim() + new string(' ', 13)).Substring(0, 13));
                    //            sbTxtPed.Append((Convert.ToString(item["DesArticulo"]).Trim() + new string(' ', 40)).Substring(0, 40));
                    //            sbTxtPed.Append((Convert.ToDecimal(item["PrecioCosto"]).ToString("##0.00").Replace(",", ".") + new string(' ', 14)).Substring(0, 14));
                    //            sbTxtPed.Append((Convert.ToString(item["UndPorCaja"]).Trim() + new string(' ', 6)).Substring(0, 6));
                    //            sbTxtPed.Append((Convert.ToDecimal(item["Descuento1"]).ToString("##0.00").Replace(",", ".") + new string(' ', 5)).Substring(0, 5));
                    //            sbTxtPed.Append((Convert.ToDecimal(item["Descuento2"]).ToString("##0.00").Replace(",", ".") + new string(' ', 5)).Substring(0, 5));
                    //            sbTxtPed.Append((Convert.ToString(item["IndIva1"]).Trim() + new string(' ', 1)).Substring(0, 1));
                    //            sbTxtPed.Append((Convert.ToString(item["CodAlmacen"]).Trim() + new string(' ', 4)).Substring(0, 4));
                    //            sbTxtPed.Append((Convert.ToString(item["NomAlmacen"]).Trim() + new string(' ', 40)).Substring(0, 40));
                    //            sbTxtPed.Append((Convert.ToString(item["ZonaOrigen"]).Trim() + new string(' ', 2)).Substring(0, 2));
                    //            sbTxtPed.Append((Convert.ToString(item["CodAlmDestino"]).Trim() + new string(' ', 4)).Substring(0, 4));
                    //            sbTxtPed.AppendLine();
                    //        }
                    //        string RutaUrlWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaVirtualDownload"]).Trim();
                    //        string RutaDirWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaFisicaDownload"]).Trim();
                    //        string NombreFile = "consulta" + DateTime.Now.ToString("ddMMyy") + " " + Convert.ToString(ds.Tables[0].Rows[0]["CodProveedor"]).Trim() + ".txt";
                    //        System.IO.File.WriteAllText(RutaDirWebAPP + NombreFile, sbTxtPed.ToString());
                    //        strRespTxt = RutaUrlWebAPP + NombreFile;
                    //        sbTxtPed = null;
                    //    }
                    //}
                    //TmpList.Add(strRespTxt);
                    // //FIN - ARMADO DE ARCHIVO DE TEXTO
                    // //INI - ARMADO DE ARCHIVO XML
                    //string strRespXml = "";
                    //if (SiXml)
                    //{
                    //    if (ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        XmlDocument xmlLstPed = new XmlDocument();
                    //        XmlElement xmlPed = null;
                    //        XmlElement xmlCab = null;
                    //        XmlElement xmlLstDet = null;
                    //        XmlElement xmlDet = null;
                    //        XmlElement xmlItem = null;
                    //        xmlLstPed.LoadXml("<?xml version=\"1.0\" encoding=\"windows-1252\" ?><pedidos />");
                    //        string numPed = "";
                    //        string fechaPedido = "";
                    //        foreach (DataRow item in ds.Tables[0].Rows)
                    //        {
                    //            if (Convert.ToString(item["NumPedido"]) != numPed || Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") != fechaPedido)
                    //            {
                    //                numPed = Convert.ToString(item["NumPedido"]);
                    //                fechaPedido = Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd");
                    //                xmlPed = xmlLstPed.CreateElement("pedido");
                    //                xmlCab = xmlLstPed.CreateElement("cabeceraPedido");
                    //                xmlLstDet = xmlLstPed.CreateElement("detallePedido");
                    //                xmlPed.AppendChild(xmlCab);
                    //                xmlPed.AppendChild(xmlLstDet);
                    //                xmlItem = xmlLstPed.CreateElement("empresa"); xmlItem.InnerText = "CORPORACION EL ROSADO S.A."; xmlCab.AppendChild(xmlItem);
                    //                xmlItem = xmlLstPed.CreateElement("codigoProveedor"); xmlItem.InnerText = Convert.ToString(item["CodProveedor"]); xmlCab.AppendChild(xmlItem);
                    //                xmlItem = xmlLstPed.CreateElement("nombreProveedor"); xmlItem.InnerText = Convert.ToString(item["NomProveedor"]); xmlCab.AppendChild(xmlItem);
                    //                xmlItem = xmlLstPed.CreateElement("numeroPedido"); xmlItem.InnerText = Convert.ToString(item["NumPedido"]); xmlCab.AppendChild(xmlItem);
                    //                xmlItem = xmlLstPed.CreateElement("fechaPedido"); xmlItem.InnerText = Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd"); xmlCab.AppendChild(xmlItem);
                    //                xmlItem = xmlLstPed.CreateElement("codigoAlmacen"); xmlItem.InnerText = Convert.ToString(item["CodAlmacen"]); xmlCab.AppendChild(xmlItem);
                    //                xmlItem = xmlLstPed.CreateElement("nombreAlmacen"); xmlItem.InnerText = Convert.ToString(item["NomAlmacen"]); xmlCab.AppendChild(xmlItem);
                    //                xmlItem = xmlLstPed.CreateElement("almacenDestino"); xmlItem.InnerText = Convert.ToString(item["CodAlmDestino"]); xmlCab.AppendChild(xmlItem);
                    //                xmlItem = xmlLstPed.CreateElement("zonaOrigen"); xmlItem.InnerText = Convert.ToString(item["ZonaOrigen"]); xmlCab.AppendChild(xmlItem);
                    //                xmlLstPed.DocumentElement.AppendChild(xmlPed);
                    //            }
                    //            xmlDet = xmlLstPed.CreateElement("detalle");
                    //            xmlItem = xmlLstPed.CreateElement("item"); xmlItem.InnerText = Convert.ToString(item["Item"]); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("cantidad"); xmlItem.InnerText = Convert.ToDecimal(item["CantPedido"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("articulo"); xmlItem.InnerText = Convert.ToString(item["CodArticulo"]); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("codigoTamano"); xmlItem.InnerText = Convert.ToString(item["Tamano"]); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("tamano"); xmlItem.InnerText = Convert.ToString(item["TamanoCaja"]); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("codigoEan"); xmlItem.InnerText = Convert.ToString(item["CodEAN"]); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("descripcion"); xmlItem.InnerText = Convert.ToString(item["DesArticulo"]); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("precioCosto"); xmlItem.InnerText = Convert.ToDecimal(item["PrecioCosto"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("UnidadesCaja"); xmlItem.InnerText = Convert.ToString(item["UndPorCaja"]); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("descuento1"); xmlItem.InnerText = Convert.ToDecimal(item["Descuento1"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("descuento2"); xmlItem.InnerText = Convert.ToDecimal(item["Descuento2"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                    //            xmlItem = xmlLstPed.CreateElement("indiceIva"); xmlItem.InnerText = Convert.ToString(item["IndIva1"]); xmlDet.AppendChild(xmlItem);
                    //            xmlLstDet.AppendChild(xmlDet);
                    //        }
                    //        string RutaUrlWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaVirtualDownload"]).Trim();
                    //        string RutaDirWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaFisicaDownload"]).Trim();
                    //        string NombreFile = "consulta" + DateTime.Now.ToString("ddMMyy") + " " + Convert.ToString(ds.Tables[0].Rows[0]["CodProveedor"]).Trim() + ".xml";
                    //        System.IO.File.WriteAllText(RutaDirWebAPP + NombreFile, xmlLstPed.OuterXml);
                    //        strRespXml = RutaUrlWebAPP + NombreFile;
                    //        xmlDet = null;
                    //        xmlItem = null;
                    //        xmlLstDet = null;
                    //        xmlCab = null;
                    //        xmlPed = null;
                    //        xmlLstPed = new XmlDocument();
                    //        xmlLstPed = null;
                    //    }
                    //}
                    //TmpList.Add(strRespXml);
                    //// FIN - ARMADO DE ARCHIVO XML
                    //// INI - ARMADO DE HTML PARA IMPRIMIR
                    //string strRespHtml = "";
                    
                    //if (SiHtml)
                    //{
                       
                    //    if (ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        System.Text.StringBuilder sbJQUERY = new System.Text.StringBuilder("");
                    //        sbJQUERY.Append("<script type=\"text/javascript\">").AppendLine();
                    //        sbJQUERY.Append("  $(document).ready(function () {").AppendLine();

                    //        System.Text.StringBuilder sbHtmlPed = new System.Text.StringBuilder("");
                    //        sbHtmlPed.Append("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">").AppendLine();
                    //        sbHtmlPed.Append("<title>CORPORACION EL ROSADO S.A.</title>").AppendLine();
                    //        sbHtmlPed.Append("<script type=\"text/javascript\" src=\"jquery-1.3.2.min.js\"></script>").AppendLine();
                    //        sbHtmlPed.Append("<script type=\"text/javascript\" src=\"jquery-barcode.js\"></script>").AppendLine();
                    //        sbHtmlPed.Append("<script type=\"text/javascript\" src=\"angular.js\"></script>").AppendLine();
                    //        sbHtmlPed.Append("<link rel=\"stylesheet\" href=\"ace.css\"/>").AppendLine();
                    //        sbHtmlPed.Append("<link rel=\"stylesheet\" href=\"bootstrap.css\"/>").AppendLine();
                    //        sbHtmlPed.Append("<script type=\"text/javascript\" src=\"bootstrap.js\"></script>").AppendLine();

                    //        sbHtmlPed.Append("</head>").AppendLine();
                    //        sbHtmlPed.Append("").AppendLine();
                    //        sbHtmlPed.Append("<body>").AppendLine();
                    //        sbHtmlPed.Append("").AppendLine();
                    //        string numPed = "";
                    //        string fechaPedido = "";
                    //        int numItems = 0; long idCodBarr = 0;
                    //        foreach (DataRow item in ds.Tables[0].Rows)
                    //        {
                    //            if (Convert.ToString(item["NumPedido"]) != numPed || Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") != fechaPedido)
                    //            {
                    //                if (numPed != "")
                    //                {
                    //                    sbHtmlPed.Append("  <tr>").AppendLine();
                    //                    sbHtmlPed.Append("    <td width=\"709\" colspan=\"12\"><b><font size=\"1\">TOTAL DE ITEMS: " + numItems.ToString() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                    //                    sbHtmlPed.Append("  </tr>").AppendLine();
                    //                    sbHtmlPed.Append("</tbody></table>").AppendLine();
                    //                    sbHtmlPed.Append("<br>").AppendLine();
                    //                    sbHtmlPed.Append("").AppendLine();
                    //                }
                    //                idCodBarr = idCodBarr + 1;

                    //                sbJQUERY.Append("    $(\"#bc" + idCodBarr.ToString() + "\").barcode(\"" + Convert.ToString(item["NumPedido"]).Trim() + "\", \"code128\");").AppendLine();

                    //                numItems = 0;
                    //                numPed = Convert.ToString(item["NumPedido"]);
                    //                fechaPedido = Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd");
                    //                sbHtmlPed.Append("<div class=\"row\">").AppendLine();
                    //                sbHtmlPed.Append("<div class=\"col-md-4\" id=\"bc" + idCodBarr.ToString() + "\"></div>").AppendLine();
                    //                idCodBarr = idCodBarr + 1;

                    //                sbJQUERY.Append("    $(\"#bc" + idCodBarr.ToString() + "\").barcode(\"" + Convert.ToInt32(CodSap.ToString()).ToString().Trim() + "\", \"code128\");").AppendLine();
                    //                sbHtmlPed.Append("<div class=\"col-md-4\"></div>");
                    //                sbHtmlPed.Append("<div class=\"col-md-4\" id=\"bc" + idCodBarr.ToString() + "\"></div></div>").AppendLine();
                    //                sbHtmlPed.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolor=\"#111111\" width=\"733\" id=\"AutoNumber2\">").AppendLine();
                    //                sbHtmlPed.Append("  <tbody><tr>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"733\" colspan=\"12\"><b><font size=\"1\">CORPORACION EL ROSADO S. A.</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("  </tr>").AppendLine();
                    //                sbHtmlPed.Append("  <tr>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"102\" colspan=\"2\"><b><font size=\"1\">PROVEEDOR</font></b></td>").AppendLine();
                    //                //sbHtmlPed.Append("    <td width=\"303\" colspan=\"4\"><font size=\"1\">" + Convert.ToString(item["CodProveedor"]).Trim() + "-" + Convert.ToString(item["NomProveedor"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                    //                if (Convert.ToString(item["CodProveedor"]).ToString().Contains(CodSap.ToString()) == true)
                    //                {
                    //                    sbHtmlPed.Append("    <td width=\"303\" colspan=\"4\"><font size=\"1\">" + Convert.ToString(item["NomProveedor"]).Trim() + " - " + (new string('0', (10 - CodSap.Trim().Length)) + CodSap.Trim()) + "</font>&nbsp;</td>").AppendLine();
                    //                }
                    //                else
                    //                {
                    //                    sbHtmlPed.Append("    <td width=\"303\" colspan=\"4\"><font size=\"1\">" + Convert.ToString(item["NomProveedor"]).Trim() + " - " + (new string('0', (10 - CodSap.Trim().Length)) + CodSap.Trim()) + " - Cod. Legacy:" + Convert.ToString(item["CodProveedor"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                    //                }

                    //                sbHtmlPed.Append("    <td width=\"155\" colspan=\"3\"><b><font size=\"1\">ZONA DE ORIGEN</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"153\" colspan=\"3\"><font size=\"1\">" + Convert.ToString(item["ZonaOrigen"]).Trim() + "</font></td>").AppendLine();
                    //                sbHtmlPed.Append("  </tr>").AppendLine();
                    //                sbHtmlPed.Append("  <tr>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"102\" colspan=\"2\"><b><font size=\"1\">PEDIDOS POR</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"103\"><font size=\"1\">" + Convert.ToString(item["CodAlmacen"]).Trim() + "-" + Convert.ToString(item["NomAlmacen"]).Trim() + "</font></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"71\"><b><font size=\"1\">ALMACEN</font><font size=\"1\"> DESTINO</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"73\"><font size=\"1\">" + Convert.ToString(item["CodAlmDestino"]).Trim() + "</font></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"52\">&nbsp;</td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"63\"><b><font size=\"1\">FECHA DEL PEDIDO</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"34\"><font size=\"1\">" + Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") + "</font></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"209\" colspan=\"4\">&nbsp;</td>").AppendLine();
                    //                sbHtmlPed.Append("  </tr>").AppendLine();
                    //                sbHtmlPed.Append("  <tr>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"102\" colspan=\"2\"><b><font size=\"1\">NUMERO DE ORDEN</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"625\" colspan=\"10\"><b><font size=\"1\">" + Convert.ToString(item["NumPedido"]).Trim() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                    //                sbHtmlPed.Append("  </tr>").AppendLine();
                    //                sbHtmlPed.Append("  <tr>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"2 7\" align=\"right\">").AppendLine();
                    //                sbHtmlPed.Append("    <p align=\"center\"><b><font size=\"1\">ITEN</font></b></p></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"73\"><b><font size=\"1\">ARTICULO</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"249\" colspan=\"3\"><b><font size=\"1\">DESCRIPCION&nbsp;&nbsp; </font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"52\"><b><font size=\"1\">REFENCIA</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"53\" align=\"left\"><p align=\"left\"><b><font size=\"1\">TAMA&Ntilde;O</font></b></p></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"34\"><b><font size=\"1\">UXC</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"54\"><b><font size=\"1\">CANTIDAD</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"36\"><b><font size=\"1\">COSTO</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"55\"><b><font size=\"1\">DESCTO</font><font size=\"1\"> 1</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("    <td width=\"57\"><b><font size=\"1\">DESCTO</font><font size=\"1\"> 2</font></b></td>").AppendLine();
                    //                sbHtmlPed.Append("  </tr>").AppendLine();
                    //                sbHtmlPed.Append("").AppendLine();
                    //            }
                    //            numItems = numItems + 1;
                    //            sbHtmlPed.Append("  <tr>").AppendLine();
                    //            sbHtmlPed.Append("    <td width=\"27\" align=\"center\"><font size=\"1\">" + Convert.ToString(item["Item"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                    //            sbHtmlPed.Append("    <td width=\"73\" align=\"center\"><font size=\"1\">" + Convert.ToString(item["CodArticulo"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                    //            sbHtmlPed.Append("    <td width=\"249\" colspan=\"3\"><font size=\"1\">" + Convert.ToString(item["DesArticulo"]).Trim() + "</font></td>").AppendLine();
                    //            sbHtmlPed.Append("    <td width=\"52\"><font size=\"1\">" + Convert.ToString(item["Tamano"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                    //            sbHtmlPed.Append("    <td width=\"53\" align=\"left\"><font size=\"1\">" + Convert.ToString(item["TamanoCaja"]).Trim() + "</font></td>").AppendLine();
                    //            sbHtmlPed.Append("    <td width=\"34\" align=\"right\"><font size=\"1\">" + Convert.ToString(item["UndPorCaja"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                    //            sbHtmlPed.Append("    <td width=\"54\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["CantPedido"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                    //            sbHtmlPed.Append("    <td width=\"36\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["PrecioCosto"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                    //            sbHtmlPed.Append("    <td width=\"45\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["Descuento1"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                    //            sbHtmlPed.Append("    <td width=\"47\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["Descuento2"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                    //            sbHtmlPed.Append("  </tr>").AppendLine();
                    //            sbHtmlPed.Append("").AppendLine();
                    //        }
                    //        sbHtmlPed.Append("  <tr>").AppendLine();
                    //        sbHtmlPed.Append("    <td width=\"709\" colspan=\"12\"><b><font size=\"1\">TOTAL DE ITEMS: " + numItems.ToString() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                    //        sbHtmlPed.Append("  </tr>").AppendLine();
                    //        sbHtmlPed.Append("</tbody></table>").AppendLine();
                    //        sbHtmlPed.Append("<br>").AppendLine();
                    //        sbHtmlPed.Append("").AppendLine();

                    //        sbJQUERY.Append("  });").AppendLine();
                    //        sbJQUERY.Append("</script>").AppendLine();
                    //        sbJQUERY.Append("").AppendLine();

                    //        sbHtmlPed.Append(sbJQUERY.ToString());
                    //        sbHtmlPed.Append("</body></html>");

                    //        string RutaUrlWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaVirtualDownload"]).Trim();
                    //        string RutaDirWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaFisicaDownload"]).Trim();
                    //        string NombreFile = "consulta" + DateTime.Now.ToString("ddMMyy") + " " + Convert.ToString(ds.Tables[0].Rows[0]["CodProveedor"]).Trim() + ".html";
                    //        System.IO.File.WriteAllText(RutaDirWebAPP + NombreFile, sbHtmlPed.ToString());
                    //        strRespHtml = RutaUrlWebAPP + NombreFile;
                    //        sbJQUERY = null;
                    //        sbHtmlPed = null;
                          
                    //    }
                    //}
                    //TmpList.Add(strRespHtml);
                    // //FIN - ARMADO DE HTML PARA IMPRIMIR

                    // //INI - ARMADO DE PDF PARA IMPRIMIR
                    //string strRespPDF = "";
                    //if (SiPdf)
                    //{
                    //    strRespPDF = reporte(ds.Tables[0], "2", CodSap.ToString());
                    //}
                    //TmpList.Add(strRespPDF);
                    // FIN - ARMADO DE PDF PARA IMPRIMIR

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


         [ActionName("exptPedidosFiltro")]
         [HttpPost]
         //public HttpResponseMessage exptPedidosFiltro(string CodSapn, string Rucn, string Usuarion, string Opc1n, string Opc2n,
         //                                               string Fecha1n, string Fecha2n, string Ciudadn, string NumOrdenn, string almacenn, string tipo)
         public HttpResponseMessage exptPedidosFiltro(Tra_ReportePedido Lista)
         {
             HttpResponseMessage result = null;
             DataSet ds = new DataSet();
             ClsGeneral objEjecucion = new ClsGeneral();
             XmlDocument xmlParam = new XmlDocument();

             //string CodSap = Tabla.p_cabecera.CodSap;
             //string codproveedor = Tabla.p_cabecera.codproveedor;
             //string tipo = Tabla.p_cabecera.tipo;

              
             string codproveedor = Lista.p_cabecera.pCodSAP;
             string CodSap = Lista.p_cabecera.pCodSAP;
             string tipo = Lista.p_cabecera.tipo;

             try
             {


                 xmlParam.LoadXml("<Root />");
                 xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                 xmlParam.DocumentElement.SetAttribute("CodProveedor", CodSap);
                 xmlParam.DocumentElement.SetAttribute("Ruc", Lista.p_cabecera.pRuc);
                 xmlParam.DocumentElement.SetAttribute("Usuario", Lista.p_cabecera.pUsuario);
                 xmlParam.DocumentElement.SetAttribute("Opcion1", Lista.p_cabecera.pOpcGrp1);
                 xmlParam.DocumentElement.SetAttribute("Opcion2", Lista.p_cabecera.pOpcGrp2);
                 xmlParam.DocumentElement.SetAttribute("Fecha1", Lista.p_cabecera.Fecha1);
                 xmlParam.DocumentElement.SetAttribute("Fecha2", Lista.p_cabecera.Fecha2);
                 xmlParam.DocumentElement.SetAttribute("CodCiudad", Lista.p_cabecera.Ciudad);
                 xmlParam.DocumentElement.SetAttribute("NumPedido", Lista.p_cabecera.pNumOrden);
                 xmlParam.DocumentElement.SetAttribute("CodAlmacen", Lista.p_cabecera.Almacen != null ? Lista.p_cabecera.Almacen : "");
                 
                  
                 foreach(var aux in Lista.p_detalle)
                 {
                     XmlElement elem = xmlParam.CreateElement("ListaPedido");
                     elem.SetAttribute("idPedido", aux.idPedido.ToString());
                     xmlParam.DocumentElement.AppendChild(elem);
                 }

                 ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 502, 1);
                 // FIN - CONSULTO a STORE PROCEDURE DE BD
                 if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                 {

                 }

                 string RutaUrlWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaVirtualDownload"]).Trim();
                 string RutaDirWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaFisicaDownload"]).Trim();
                 DataTable dt = new DataTable("rptPedidos");
                 dt.Columns.Add("Origen", System.Type.GetType("System.String"));
                 dt.Columns.Add("IdPedido", System.Type.GetType("System.String"));
                 dt.Columns.Add("NumPedido", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodAlmacen", System.Type.GetType("System.String"));
                 dt.Columns.Add("NomAlmacen", System.Type.GetType("System.String"));
                 dt.Columns.Add("FechaPedido", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodAlmDestino", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodProveedor", System.Type.GetType("System.String"));
                 dt.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                 dt.Columns.Add("ZonaOrigen", System.Type.GetType("System.String"));
                 dt.Columns.Add("Item", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodArticulo", System.Type.GetType("System.String"));
                 dt.Columns.Add("DesArticulo", System.Type.GetType("System.String"));
                 dt.Columns.Add("Tamano", System.Type.GetType("System.String"));
                 dt.Columns.Add("CantPedido", System.Type.GetType("System.String"));
                 dt.Columns.Add("PrecioCosto", System.Type.GetType("System.String"));
                 dt.Columns.Add("UndPorCaja", System.Type.GetType("System.String"));
                 dt.Columns.Add("Descuento1", System.Type.GetType("System.String"));
                 dt.Columns.Add("Descuento2", System.Type.GetType("System.String"));
                 dt.Columns.Add("IndIva1", System.Type.GetType("System.String"));
                 dt.Columns.Add("TamanoCaja", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodEAN", System.Type.GetType("System.String"));
                 dt.Columns.Add("EsDescargado", System.Type.GetType("System.String"));
                 dt.Columns.Add("EsImpreso", System.Type.GetType("System.String"));
                 dt.Columns.Add("numPedidoImagen", System.Type.GetType("System.Byte[]"));
                 dt.Columns.Add("codProveedorImagen", System.Type.GetType("System.Byte[]"));
                 dt.Columns.Add("codSap", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodProveedorint", System.Type.GetType("System.String"));
                dt.Columns.Add("FecEntregaActual", System.Type.GetType("System.String"));
                foreach (DataRow dr in ds.Tables[0].Rows)
                 {
                     DataRow drowd = dt.NewRow();
                     drowd["Origen"] = dr[0].ToString();
                     drowd["IdPedido"] = dr[1].ToString();
                     drowd["NumPedido"] = dr[2].ToString();
                     drowd["CodAlmacen"] = dr[3].ToString();
                     drowd["NomAlmacen"] = dr[4].ToString();
                     drowd["FechaPedido"] = dr[5].ToString().Substring(6, 4) + "." + dr[5].ToString().Substring(3, 2) + "." + dr[5].ToString().Substring(0, 2);
                     drowd["CodAlmDestino"] = dr[6].ToString();
                     drowd["CodProveedor"] = dr[7].ToString();
                     drowd["NomProveedor"] = dr[8].ToString();
                     drowd["ZonaOrigen"] = dr[9].ToString();
                     drowd["Item"] = dr[10].ToString();
                     drowd["CodArticulo"] = dr[11].ToString();
                     drowd["DesArticulo"] = dr[12].ToString();
                     drowd["Tamano"] = dr[13].ToString();
                     drowd["CantPedido"] = dr[14].ToString();
                     drowd["PrecioCosto"] = dr[15].ToString();
                     drowd["UndPorCaja"] = dr[16].ToString();
                     drowd["Descuento1"] = dr[17].ToString();
                     drowd["Descuento2"] = dr[18].ToString();
                     drowd["IndIva1"] = dr[19].ToString();
                     drowd["TamanoCaja"] = dr[20].ToString();
                     drowd["CodEAN"] = dr[21].ToString();
                     drowd["EsDescargado"] = dr[22].ToString();
                     drowd["EsImpreso"] = dr[23].ToString();


                     if (tipo == "4")
                     {
                         BarcodeLib.Barcode barcode = new BarcodeLib.Barcode()
                         {
                             IncludeLabel = true,
                             Alignment = AlignmentPositions.CENTER,
                             Width = 130,
                             Height = 40,
                             RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                             BackColor = Color.White,
                             ForeColor = Color.Black,
                         };

                         System.Drawing.Image img = barcode.Encode(TYPE.CODE128, Convert.ToString(drowd["NumPedido"]));

                         MemoryStream ms = new MemoryStream();

                         img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                         //var ruta = RutaDirWebAPP + "0" + Convert.ToString(drowd["NumPedido"]) + ".png";
                         //GenCode128.Code128Rendering.MakeBarcodeImage("0" + Convert.ToString(drowd["NumPedido"]), 1, true).Save(ruta, System.Drawing.Imaging.ImageFormat.Png);
                         drowd["numPedidoImagen"] = ms.ToArray();// File.ReadAllBytes(ruta);
                         string aux = "";
                         try
                         {
                             aux = Convert.ToDouble(drowd["CodProveedor"]).ToString();
                         }
                         catch (Exception)
                         {

                             aux = drowd["CodProveedor"].ToString();
                         }

                         System.Drawing.Image img1 = barcode.Encode(TYPE.CODE128, Convert.ToString(aux));
                         MemoryStream ms1 = new MemoryStream();
                         img1.Save(ms1, System.Drawing.Imaging.ImageFormat.Png);
                         //GenCode128.Code128Rendering.MakeBarcodeImage("0" + Convert.ToString(Convert.ToDouble(drowd["CodProveedor"])), 1, true).Save(ruta1, System.Drawing.Imaging.ImageFormat.Png);
                         drowd["codProveedorImagen"] = ms1.ToArray();
                         barcode.Dispose();
                         ms.Dispose();
                         ms1.Dispose();
                         img.Dispose();
                         img1.Dispose();
                     }
                     drowd["codSap"] = CodSap;
                     try
                     {
                         drowd["CodProveedorint"] = Convert.ToDouble(dr[7].ToString());
                     }
                     catch (Exception)
                     {

                         drowd["CodProveedorint"] = dr[7].ToString();
                     }

                    drowd["FecEntregaActual"] = dr[24].ToString().Substring(6, 4) + "." + dr[24].ToString().Substring(3, 2) + "." + dr[24].ToString().Substring(0, 2);
                    dt.Rows.Add(drowd);
                 }

                 string strRespTxt = "";
                 if (tipo == "1")
                 {

                     System.Text.StringBuilder sbTxtPed = new System.Text.StringBuilder("");
                     foreach (DataRow item in dt.Rows)
                     {
                         sbTxtPed.Append((Convert.ToString(item["CodProveedor"]).Trim() + new string(' ', 10)).Substring(0, 10));
                         sbTxtPed.Append((Convert.ToString(item["NomProveedor"]).Trim() + new string(' ', 40)).Substring(0, 40));
                         sbTxtPed.Append((Convert.ToString(item["NumPedido"]).Trim() + new string(' ', 10)).Substring(0, 10));
                         sbTxtPed.Append((Convert.ToString(item["Item"]).Trim() + new string(' ', 5)).Substring(0, 5));
                         sbTxtPed.Append((Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") + new string(' ', 10)).Substring(0, 10));
                         sbTxtPed.Append((Convert.ToDecimal(item["CantPedido"]).ToString("##0.00").Replace(",", ".") + new string(' ', 12)).Substring(0, 12));
                         sbTxtPed.Append((Convert.ToString(item["CodArticulo"]).Trim() + new string(' ', 18)).Substring(0, 18));
                         sbTxtPed.Append((Convert.ToString(item["Tamano"]).Trim() + new string(' ', 18)).Substring(0, 18));
                         sbTxtPed.Append((Convert.ToString(item["TamanoCaja"]).Trim() + new string(' ', 10)).Substring(0, 10));
                         sbTxtPed.Append((Convert.ToString(item["CodEAN"]).Trim() + new string(' ', 13)).Substring(0, 13));
                         sbTxtPed.Append((Convert.ToString(item["DesArticulo"]).Trim() + new string(' ', 40)).Substring(0, 40));
                         sbTxtPed.Append((Convert.ToDecimal(item["PrecioCosto"]).ToString("##0.00").Replace(",", ".") + new string(' ', 14)).Substring(0, 14));
                         sbTxtPed.Append((Convert.ToString(item["UndPorCaja"]).Trim() + new string(' ', 6)).Substring(0, 6));
                         sbTxtPed.Append((Convert.ToDecimal(item["Descuento1"]).ToString("##0.00").Replace(",", ".") + new string(' ', 5)).Substring(0, 5));
                         sbTxtPed.Append((Convert.ToDecimal(item["Descuento2"]).ToString("##0.00").Replace(",", ".") + new string(' ', 5)).Substring(0, 5));
                         sbTxtPed.Append((Convert.ToString(item["IndIva1"]).Trim() + new string(' ', 1)).Substring(0, 1));
                         sbTxtPed.Append((Convert.ToString(item["CodAlmacen"]).Trim() + new string(' ', 4)).Substring(0, 4));
                         sbTxtPed.Append((Convert.ToString(item["NomAlmacen"]).Trim() + new string(' ', 40)).Substring(0, 40));
                         sbTxtPed.Append((Convert.ToString(item["ZonaOrigen"]).Trim() + new string(' ', 2)).Substring(0, 2));
                         sbTxtPed.Append((Convert.ToString(item["CodAlmDestino"]).Trim() + new string(' ', 4)).Substring(0, 4));
                         sbTxtPed.AppendLine();
                     }

                     string NombreFile = "consulta" + DateTime.Now.ToString("ddMMyy") + " " + Convert.ToString(codproveedor).Trim() + ".txt";
                     if (dt.Rows.Count > 0)
                     {
                         byte[] bytes = Encoding.ASCII.GetBytes(sbTxtPed.ToString());
                         result = Request.CreateResponse(HttpStatusCode.OK);
                         result.Content = new StreamContent(new MemoryStream(bytes));
                         result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                         result.Content.Headers.ContentDisposition.FileName = NombreFile;
                     }


                 }
                 if (tipo == "2")
                 {

                     XmlDocument xmlLstPed = new XmlDocument();
                     XmlElement xmlPed = null;
                     XmlElement xmlCab = null;
                     XmlElement xmlLstDet = null;
                     XmlElement xmlDet = null;
                     XmlElement xmlItem = null;
                     xmlLstPed.LoadXml("<?xml version=\"1.0\" encoding=\"windows-1252\" ?><pedidos />");
                     string numPed = "";
                     string fechaPedido = "";
                     foreach (DataRow item in dt.Rows)
                     {
                         if (Convert.ToString(item["NumPedido"]) != numPed || Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") != fechaPedido)
                         {
                             numPed = Convert.ToString(item["NumPedido"]);
                             fechaPedido = Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd");
                             xmlPed = xmlLstPed.CreateElement("pedido");
                             xmlCab = xmlLstPed.CreateElement("cabeceraPedido");
                             xmlLstDet = xmlLstPed.CreateElement("detallePedido");
                             xmlPed.AppendChild(xmlCab);
                             xmlPed.AppendChild(xmlLstDet);
                             xmlItem = xmlLstPed.CreateElement("empresa"); xmlItem.InnerText = "CORPORACION EL ROSADO S.A."; xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("codigoProveedor"); xmlItem.InnerText = Convert.ToString(item["CodProveedor"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("nombreProveedor"); xmlItem.InnerText = Convert.ToString(item["NomProveedor"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("numeroPedido"); xmlItem.InnerText = Convert.ToString(item["NumPedido"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("fechaPedido"); xmlItem.InnerText = Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd"); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("codigoAlmacen"); xmlItem.InnerText = Convert.ToString(item["CodAlmacen"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("nombreAlmacen"); xmlItem.InnerText = Convert.ToString(item["NomAlmacen"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("almacenDestino"); xmlItem.InnerText = Convert.ToString(item["CodAlmDestino"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("zonaOrigen"); xmlItem.InnerText = Convert.ToString(item["ZonaOrigen"]); xmlCab.AppendChild(xmlItem);
                             xmlLstPed.DocumentElement.AppendChild(xmlPed);
                         }
                         xmlDet = xmlLstPed.CreateElement("detalle");
                         xmlItem = xmlLstPed.CreateElement("item"); xmlItem.InnerText = Convert.ToString(item["Item"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("cantidad"); xmlItem.InnerText = Convert.ToDecimal(item["CantPedido"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("articulo"); xmlItem.InnerText = Convert.ToString(item["CodArticulo"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("codigoTamano"); xmlItem.InnerText = Convert.ToString(item["Tamano"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("tamano"); xmlItem.InnerText = Convert.ToString(item["TamanoCaja"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("codigoEan"); xmlItem.InnerText = Convert.ToString(item["CodEAN"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("descripcion"); xmlItem.InnerText = Convert.ToString(item["DesArticulo"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("precioCosto"); xmlItem.InnerText = Convert.ToDecimal(item["PrecioCosto"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("UnidadesCaja"); xmlItem.InnerText = Convert.ToString(item["UndPorCaja"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("descuento1"); xmlItem.InnerText = Convert.ToDecimal(item["Descuento1"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("descuento2"); xmlItem.InnerText = Convert.ToDecimal(item["Descuento2"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("indiceIva"); xmlItem.InnerText = Convert.ToString(item["IndIva1"]); xmlDet.AppendChild(xmlItem);
                         xmlLstDet.AppendChild(xmlDet);
                     }
                     string NombreFile = "consulta" + DateTime.Now.ToString("ddMMyy") + " " + Convert.ToString(codproveedor).Trim() + ".xml";

                     if (dt.Rows.Count > 0)
                     {
                         byte[] bytes = Encoding.ASCII.GetBytes(xmlLstPed.OuterXml.ToString());
                         result = Request.CreateResponse(HttpStatusCode.OK);
                         result.Content = new StreamContent(new MemoryStream(bytes));
                         result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                         result.Content.Headers.ContentDisposition.FileName = NombreFile;
                     }
                     xmlDet = null;
                     xmlItem = null;
                     xmlLstDet = null;
                     xmlCab = null;
                     xmlPed = null;
                     xmlLstPed = new XmlDocument();
                     xmlLstPed = null;
                 }

                 if (tipo == "3")
                 {


                     System.Text.StringBuilder sbJQUERY = new System.Text.StringBuilder("");
                     sbJQUERY.Append("<script type=\"text/javascript\">").AppendLine();
                     sbJQUERY.Append("  $(document).ready(function () {").AppendLine();

                     System.Text.StringBuilder sbHtmlPed = new System.Text.StringBuilder("");
                     sbHtmlPed.Append("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">").AppendLine();
                     sbHtmlPed.Append("<title>CORPORACION EL ROSADO S.A.</title>").AppendLine();
                     sbHtmlPed.Append("<script type=\"text/javascript\" src=\"https://proveedores.elrosado.com/DownloadTmp/jquery-1.3.2.min.js\"></script>").AppendLine();
                     sbHtmlPed.Append("<script type=\"text/javascript\" src=\"https://proveedores.elrosado.com/DownloadTmp/jquery-barcode.js\"></script>").AppendLine();
                     sbHtmlPed.Append("<script type=\"text/javascript\" src=\"https://proveedores.elrosado.com/DownloadTmp/angular.js\"></script>").AppendLine();
                     sbHtmlPed.Append("<link rel=\"stylesheet\" href=\"https://proveedores.elrosado.com/DownloadTmp/ace.css\"/>").AppendLine();
                     sbHtmlPed.Append("<link rel=\"stylesheet\" href=\"https://proveedores.elrosado.com/DownloadTmp/bootstrap.css\"/>").AppendLine();
                     sbHtmlPed.Append("<script type=\"text/javascript\" src=\"https://proveedores.elrosado.com/DownloadTmp/bootstrap.js\"></script>").AppendLine();

                     sbHtmlPed.Append("</head>").AppendLine();
                     sbHtmlPed.Append("").AppendLine();
                     sbHtmlPed.Append("<body>").AppendLine();
                     sbHtmlPed.Append("").AppendLine();
                     string numPed = "";
                     string fechaPedido = "";
                     int numItems = 0; long idCodBarr = 0;
                     foreach (DataRow item in dt.Rows)
                     {
                         if (Convert.ToString(item["NumPedido"]) != numPed || Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") != fechaPedido)
                         {
                             if (numPed != "")
                             {
                                 sbHtmlPed.Append("  <tr>").AppendLine();
                                 sbHtmlPed.Append("    <td width=\"709\" colspan=\"12\"><b><font size=\"1\">TOTAL DE ITEMS: " + numItems.ToString() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                                 sbHtmlPed.Append("  </tr>").AppendLine();
                                 sbHtmlPed.Append("</tbody></table>").AppendLine();
                                 sbHtmlPed.Append("<br>").AppendLine();
                                 sbHtmlPed.Append("").AppendLine();
                             }
                             idCodBarr = idCodBarr + 1;

                             sbJQUERY.Append("    $(\"#bc" + idCodBarr.ToString() + "\").barcode(\"" + Convert.ToString(item["NumPedido"]).Trim() + "\", \"code128\");").AppendLine();

                             numItems = 0;
                             numPed = Convert.ToString(item["NumPedido"]);
                             fechaPedido = Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd");
                             sbHtmlPed.Append("<div class=\"row\">").AppendLine();
                             
                             sbHtmlPed.Append("<div class=\"col-md-4\" id=\"bc" + idCodBarr.ToString() + "\"></div>").AppendLine();
                             idCodBarr = idCodBarr + 1;
                             string aux="";
                             try
                             {
                                 aux = Convert.ToInt32(CodSap.ToString()).ToString().Trim();
                             }
                             catch (Exception)
                             {

                                 aux = CodSap.ToString().ToString();
                             }

                             sbJQUERY.Append("    $(\"#bc" + idCodBarr.ToString() + "\").barcode(\"" + aux + "\", \"code128\");").AppendLine();
                             sbHtmlPed.Append("<div class=\"col-md-4\"></div>");
                             sbHtmlPed.Append("<div class=\"col-md-4\" id=\"bc" + idCodBarr.ToString() + "\"></div></div>").AppendLine();


                             sbHtmlPed.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolor=\"#111111\" width=\"733\" id=\"AutoNumber2\">").AppendLine();
                             sbHtmlPed.Append("  <tbody><tr>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"733\" colspan=\"12\"><b><font size=\"1\">CORPORACION EL ROSADO S. A.</font></b></td>").AppendLine();
                             sbHtmlPed.Append("  </tr>").AppendLine();
                             sbHtmlPed.Append("  <tr>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"102\" colspan=\"2\"><b><font size=\"1\">PROVEEDOR</font></b></td>").AppendLine();
                             //sbHtmlPed.Append("    <td width=\"303\" colspan=\"4\"><font size=\"1\">" + Convert.ToString(item["CodProveedor"]).Trim() + "-" + Convert.ToString(item["NomProveedor"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                             if (Convert.ToString(item["CodProveedor"]).ToString().Contains(CodSap.ToString()) == true)
                             {
                                 sbHtmlPed.Append("    <td width=\"303\" colspan=\"4\"><font size=\"1\">" + Convert.ToString(item["NomProveedor"]).Trim() + " - " + (new string('0', (10 - CodSap.Trim().Length)) + CodSap.Trim()) + "</font>&nbsp;</td>").AppendLine();
                             }
                             else
                             {
                                 sbHtmlPed.Append("    <td width=\"303\" colspan=\"4\"><font size=\"1\">" + Convert.ToString(item["NomProveedor"]).Trim() + " - " + (new string('0', (10 - CodSap.Trim().Length)) + CodSap.Trim()) + " - Cod. Legacy:" + Convert.ToString(item["CodProveedor"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                             }

                             sbHtmlPed.Append("    <td width=\"155\" colspan=\"3\"><b><font size=\"1\">ZONA DE ORIGEN</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"153\" colspan=\"3\"><font size=\"1\">" + Convert.ToString(item["ZonaOrigen"]).Trim() + "</font></td>").AppendLine();
                             sbHtmlPed.Append("  </tr>").AppendLine();
                             sbHtmlPed.Append("  <tr>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"102\" colspan=\"2\"><b><font size=\"1\">PEDIDOS POR</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"103\"><font size=\"1\">" + Convert.ToString(item["CodAlmacen"]).Trim() + "-" + Convert.ToString(item["NomAlmacen"]).Trim() + "</font></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"71\"><b><font size=\"1\">ALMACEN</font><font size=\"1\"> DESTINO</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"73\"><font size=\"1\">" + Convert.ToString(item["CodAlmDestino"]).Trim() + "</font></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"52\">&nbsp;</td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"63\"><b><font size=\"1\">FECHA DEL PEDIDO</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"34\"><font size=\"1\">" + Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") + "</font></td>").AppendLine();
                            sbHtmlPed.Append("    <td width=\"52\"><b><font size=\"1\"></font></b></td>").AppendLine();
                            sbHtmlPed.Append("    <td width=\"157\" colspan=\"4\"><font size=\"1\"></font></td>").AppendLine();
                            //sbHtmlPed.Append("    <td width=\"52\"><b><font size=\"1\">FECHA DE ENTREGA</font></b></td>").AppendLine();
                            //sbHtmlPed.Append("    <td width=\"157\" colspan=\"4\"><font size=\"1\">" + Convert.ToDateTime(item["FecEntregaActual"]).ToString("yyyy.MM.dd") + "</font></td>").AppendLine();
                            sbHtmlPed.Append("  </tr>").AppendLine();
                             sbHtmlPed.Append("  <tr>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"102\" colspan=\"2\"><b><font size=\"1\">NUMERO DE ORDEN</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"625\" colspan=\"10\"><b><font size=\"1\">" + Convert.ToString(item["NumPedido"]).Trim() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                             sbHtmlPed.Append("  </tr>").AppendLine();
                             sbHtmlPed.Append("  <tr>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"2 7\" align=\"right\">").AppendLine();
                             sbHtmlPed.Append("    <p align=\"center\"><b><font size=\"1\">ITEN</font></b></p></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"73\"><b><font size=\"1\">ARTICULO</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"249\" colspan=\"3\"><b><font size=\"1\">DESCRIPCION&nbsp;&nbsp; </font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"52\"><b><font size=\"1\">REFENCIA</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"53\" align=\"left\"><p align=\"left\"><b><font size=\"1\">TAMA&Ntilde;O</font></b></p></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"34\"><b><font size=\"1\">UXC</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"54\"><b><font size=\"1\">CANTIDAD</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"36\"><b><font size=\"1\">COSTO</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"55\"><b><font size=\"1\">DESCTO</font><font size=\"1\"> 1</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"57\"><b><font size=\"1\">DESCTO</font><font size=\"1\"> 2</font></b></td>").AppendLine();
                             sbHtmlPed.Append("  </tr>").AppendLine();
                             sbHtmlPed.Append("").AppendLine();
                         }
                         numItems = numItems + 1;
                         sbHtmlPed.Append("  <tr>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"27\" align=\"center\"><font size=\"1\">" + Convert.ToString(item["Item"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"73\" align=\"center\"><font size=\"1\">" + Convert.ToString(item["CodArticulo"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"249\" colspan=\"3\"><font size=\"1\">" + Convert.ToString(item["DesArticulo"]).Trim() + "</font></td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"52\"><font size=\"1\">" + Convert.ToString(item["Tamano"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"53\" align=\"left\"><font size=\"1\">" + Convert.ToString(item["TamanoCaja"]).Trim() + "</font></td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"34\" align=\"right\"><font size=\"1\">" + Convert.ToString(item["UndPorCaja"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"54\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["CantPedido"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"36\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["PrecioCosto"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"45\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["Descuento1"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"47\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["Descuento2"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("  </tr>").AppendLine();
                         sbHtmlPed.Append("").AppendLine();
                     }
                     sbHtmlPed.Append("  <tr>").AppendLine();
                     sbHtmlPed.Append("    <td width=\"709\" colspan=\"12\"><b><font size=\"1\">TOTAL DE ITEMS: " + numItems.ToString() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                     sbHtmlPed.Append("  </tr>").AppendLine();
                     sbHtmlPed.Append("</tbody></table>").AppendLine();
                     sbHtmlPed.Append("<br>").AppendLine();
                     sbHtmlPed.Append("").AppendLine();

                     sbJQUERY.Append("  });").AppendLine();
                     sbJQUERY.Append("</script>").AppendLine();
                     sbJQUERY.Append("").AppendLine();

                     sbHtmlPed.Append(sbJQUERY.ToString());
                     sbHtmlPed.Append("</body></html>");


                     string NombreFile = "consulta" + DateTime.Now.ToString("ddMMyy") + " " + Convert.ToString(codproveedor).Trim() + ".html";

                     if (dt.Rows.Count > 0)
                     {
                         byte[] bytes = Encoding.ASCII.GetBytes(sbHtmlPed.ToString());
                         result = Request.CreateResponse(HttpStatusCode.OK);
                         result.Content = new StreamContent(new MemoryStream(bytes));
                         result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                         result.Content.Headers.ContentDisposition.FileName = NombreFile;
                     }
                     sbJQUERY = null;
                     sbHtmlPed = null;
                 }

                 if (tipo == "4")
                 {
                     string archivo = "";
                     ReportDataSource rptDataSourcecab;
                     rptDataSourcecab = new ReportDataSource("rptPedidos", dt);
                     ReportViewer auxc = new ReportViewer();
                     Warning[] warnings = null;
                     string[] streamids = null;
                     string mimeType = "";
                     string encoding = "";
                     string extension = "";

                     auxc.ProcessingMode = ProcessingMode.Local;
                     auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/reportePedido.rdlc");

                     auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                     byte[] bytes = null;
                     //if (tipo == "1")
                     //{
                     //    archivo = "Reportepedidos" + CodSap + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xls";
                     //    bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                     //}
                     //if (tipo == "2")
                     //{
                     archivo = "Reportepedidos" + CodSap + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".pdf";
                     bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                     //

                     if (dt.Rows.Count > 0)
                     {
                         result = Request.CreateResponse(HttpStatusCode.OK);
                         result.Content = new StreamContent(new MemoryStream(bytes));
                         result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                         result.Content.Headers.ContentDisposition.FileName = archivo;
                     }

                     //strRespTxt = RutaUrlWebAPP + archivo;
                     //archivo = RutaDirWebAPP + archivo;
                     //FileStream fs = System.IO.File.Create(archivo);
                     //fs.Write(bytes, 0, bytes.Length);
                     //fs.Close();
                     //auxc.Dispose();

                 }
             }catch(Exception e)
             {

             }
             
             return result;
         }

         [ActionName("exptPedidosFiltroupd")]
         [HttpGet]
         public HttpResponseMessage exptPedidosFiltroupd(string CodSapn, string Rucn, string Usuarion, string Opc1n, string Opc2n,
                                                        string Fecha1n, string Fecha2n, string Ciudadn, string NumOrdenn, string almacenn, string tipo)
         {
             HttpResponseMessage result = null;
             DataSet ds = new DataSet();
             ClsGeneral objEjecucion = new ClsGeneral();
             XmlDocument xmlParam = new XmlDocument();

             //string CodSap = Tabla.p_cabecera.CodSap;
             //string codproveedor = Tabla.p_cabecera.codproveedor;
             //string tipo = Tabla.p_cabecera.tipo;


             string codproveedor = CodSapn;
             string CodSap = CodSapn;
             

             try
             {


                 xmlParam.LoadXml("<Root />");
                 xmlParam.DocumentElement.SetAttribute("IdEmpresa", "1");
                 xmlParam.DocumentElement.SetAttribute("CodProveedor", CodSap);
                 xmlParam.DocumentElement.SetAttribute("Ruc", Rucn);
                 xmlParam.DocumentElement.SetAttribute("Usuario", Usuarion);
                 xmlParam.DocumentElement.SetAttribute("Opcion1", Opc1n);
                 xmlParam.DocumentElement.SetAttribute("Opcion2", Opc2n);
                 xmlParam.DocumentElement.SetAttribute("Fecha1", Fecha1n);
                 xmlParam.DocumentElement.SetAttribute("Fecha2", Fecha2n);
                 xmlParam.DocumentElement.SetAttribute("CodCiudad", Ciudadn);
                 xmlParam.DocumentElement.SetAttribute("NumPedido", NumOrdenn);
                 xmlParam.DocumentElement.SetAttribute("CodAlmacen", almacenn != null ? almacenn : "");


                 ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 502, 1);
                 // FIN - CONSULTO a STORE PROCEDURE DE BD
                 if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                 {

                 }

                 string RutaUrlWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaVirtualDownload"]).Trim();
                 string RutaDirWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaFisicaDownload"]).Trim();
                 DataTable dt = new DataTable("rptPedidos");
                 dt.Columns.Add("Origen", System.Type.GetType("System.String"));
                 dt.Columns.Add("IdPedido", System.Type.GetType("System.String"));
                 dt.Columns.Add("NumPedido", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodAlmacen", System.Type.GetType("System.String"));
                 dt.Columns.Add("NomAlmacen", System.Type.GetType("System.String"));
                 dt.Columns.Add("FechaPedido", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodAlmDestino", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodProveedor", System.Type.GetType("System.String"));
                 dt.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                 dt.Columns.Add("ZonaOrigen", System.Type.GetType("System.String"));
                 dt.Columns.Add("Item", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodArticulo", System.Type.GetType("System.String"));
                 dt.Columns.Add("DesArticulo", System.Type.GetType("System.String"));
                 dt.Columns.Add("Tamano", System.Type.GetType("System.String"));
                 dt.Columns.Add("CantPedido", System.Type.GetType("System.String"));
                 dt.Columns.Add("PrecioCosto", System.Type.GetType("System.String"));
                 dt.Columns.Add("UndPorCaja", System.Type.GetType("System.String"));
                 dt.Columns.Add("Descuento1", System.Type.GetType("System.String"));
                 dt.Columns.Add("Descuento2", System.Type.GetType("System.String"));
                 dt.Columns.Add("IndIva1", System.Type.GetType("System.String"));
                 dt.Columns.Add("TamanoCaja", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodEAN", System.Type.GetType("System.String"));
                 dt.Columns.Add("EsDescargado", System.Type.GetType("System.String"));
                 dt.Columns.Add("EsImpreso", System.Type.GetType("System.String"));
                 dt.Columns.Add("numPedidoImagen", System.Type.GetType("System.Byte[]"));
                 dt.Columns.Add("codProveedorImagen", System.Type.GetType("System.Byte[]"));
                 dt.Columns.Add("codSap", System.Type.GetType("System.String"));
                 dt.Columns.Add("CodProveedorint", System.Type.GetType("System.String"));
                 dt.Columns.Add("FecEntregaActual", System.Type.GetType("System.String"));
                foreach (DataRow dr in ds.Tables[0].Rows)
                 {
                     DataRow drowd = dt.NewRow();
                     drowd["Origen"] = dr[0].ToString();
                     drowd["IdPedido"] = dr[1].ToString();
                     drowd["NumPedido"] = dr[2].ToString();
                     drowd["CodAlmacen"] = dr[3].ToString();
                     drowd["NomAlmacen"] = dr[4].ToString();
                     drowd["FechaPedido"] = dr[5].ToString().Substring(6, 4) + "." + dr[5].ToString().Substring(3, 2) + "." + dr[5].ToString().Substring(0, 2);
                     drowd["CodAlmDestino"] = dr[6].ToString();
                     drowd["CodProveedor"] = dr[7].ToString();
                     drowd["NomProveedor"] = dr[8].ToString();
                     drowd["ZonaOrigen"] = dr[9].ToString();
                     drowd["Item"] = dr[10].ToString();
                     drowd["CodArticulo"] = dr[11].ToString();
                     drowd["DesArticulo"] = dr[12].ToString();
                     drowd["Tamano"] = dr[13].ToString();
                     drowd["CantPedido"] = dr[14].ToString();
                     drowd["PrecioCosto"] = dr[15].ToString();
                     drowd["UndPorCaja"] = dr[16].ToString();
                     drowd["Descuento1"] = dr[17].ToString();
                     drowd["Descuento2"] = dr[18].ToString();
                     drowd["IndIva1"] = dr[19].ToString();
                     drowd["TamanoCaja"] = dr[20].ToString();
                     drowd["CodEAN"] = dr[21].ToString();
                     drowd["EsDescargado"] = dr[22].ToString();
                     drowd["EsImpreso"] = dr[23].ToString();


                     if (tipo == "4")
                     {
                         BarcodeLib.Barcode barcode = new BarcodeLib.Barcode()
                         {
                             IncludeLabel = true,
                             Alignment = AlignmentPositions.CENTER,
                             Width = 130,
                             Height = 40,
                             RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                             BackColor = Color.White,
                             ForeColor = Color.Black,
                         };

                         System.Drawing.Image img = barcode.Encode(TYPE.CODE128, Convert.ToString(drowd["NumPedido"]));

                         MemoryStream ms = new MemoryStream();
                         img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                         //var ruta = RutaDirWebAPP + "0" + Convert.ToString(drowd["NumPedido"]) + ".png";
                         //GenCode128.Code128Rendering.MakeBarcodeImage("0" + Convert.ToString(drowd["NumPedido"]), 1, true).Save(ruta, System.Drawing.Imaging.ImageFormat.Png);
                         drowd["numPedidoImagen"] = ms.ToArray();// File.ReadAllBytes(ruta);
                         //var ruta1 = RutaDirWebAPP + "0" + Convert.ToString(drowd["CodProveedor"]) + ".png";
                         System.Drawing.Image img1 = barcode.Encode(TYPE.CODE128, Convert.ToString(Convert.ToDouble(drowd["CodProveedor"])));
                         MemoryStream ms1 = new MemoryStream();
                         img1.Save(ms1, System.Drawing.Imaging.ImageFormat.Png);
                         //GenCode128.Code128Rendering.MakeBarcodeImage("0" + Convert.ToString(Convert.ToDouble(drowd["CodProveedor"])), 1, true).Save(ruta1, System.Drawing.Imaging.ImageFormat.Png);
                         drowd["codProveedorImagen"] = ms1.ToArray();
                         barcode.Dispose();
                         ms.Dispose();
                         ms1.Dispose();
                         img.Dispose();
                         img1.Dispose();
                     }
                     drowd["codSap"] = CodSap;
                     drowd["CodProveedorint"] = dr[7].ToString();
                    drowd["FecEntregaActual"] = dr[24].ToString().Substring(6, 4) + "." + dr[24].ToString().Substring(3, 2) + "." + dr[24].ToString().Substring(0, 2);
                    dt.Rows.Add(drowd);
                 }

                 string strRespTxt = "";
                 if (tipo == "1")
                 {

                     System.Text.StringBuilder sbTxtPed = new System.Text.StringBuilder("");
                     foreach (DataRow item in dt.Rows)
                     {
                         sbTxtPed.Append((Convert.ToString(item["CodProveedor"]).Trim() + new string(' ', 10)).Substring(0, 10));
                         sbTxtPed.Append((Convert.ToString(item["NomProveedor"]).Trim() + new string(' ', 40)).Substring(0, 40));
                         sbTxtPed.Append((Convert.ToString(item["NumPedido"]).Trim() + new string(' ', 10)).Substring(0, 10));
                         sbTxtPed.Append((Convert.ToString(item["Item"]).Trim() + new string(' ', 5)).Substring(0, 5));
                         sbTxtPed.Append((Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") + new string(' ', 10)).Substring(0, 10));
                         sbTxtPed.Append((Convert.ToDecimal(item["CantPedido"]).ToString("##0.00").Replace(",", ".") + new string(' ', 12)).Substring(0, 12));
                         sbTxtPed.Append((Convert.ToString(item["CodArticulo"]).Trim() + new string(' ', 18)).Substring(0, 18));
                         sbTxtPed.Append((Convert.ToString(item["Tamano"]).Trim() + new string(' ', 18)).Substring(0, 18));
                         sbTxtPed.Append((Convert.ToString(item["TamanoCaja"]).Trim() + new string(' ', 10)).Substring(0, 10));
                         sbTxtPed.Append((Convert.ToString(item["CodEAN"]).Trim() + new string(' ', 13)).Substring(0, 13));
                         sbTxtPed.Append((Convert.ToString(item["DesArticulo"]).Trim() + new string(' ', 40)).Substring(0, 40));
                         sbTxtPed.Append((Convert.ToDecimal(item["PrecioCosto"]).ToString("##0.00").Replace(",", ".") + new string(' ', 14)).Substring(0, 14));
                         sbTxtPed.Append((Convert.ToString(item["UndPorCaja"]).Trim() + new string(' ', 6)).Substring(0, 6));
                         sbTxtPed.Append((Convert.ToDecimal(item["Descuento1"]).ToString("##0.00").Replace(",", ".") + new string(' ', 5)).Substring(0, 5));
                         sbTxtPed.Append((Convert.ToDecimal(item["Descuento2"]).ToString("##0.00").Replace(",", ".") + new string(' ', 5)).Substring(0, 5));
                         sbTxtPed.Append((Convert.ToString(item["IndIva1"]).Trim() + new string(' ', 1)).Substring(0, 1));
                         sbTxtPed.Append((Convert.ToString(item["CodAlmacen"]).Trim() + new string(' ', 4)).Substring(0, 4));
                         sbTxtPed.Append((Convert.ToString(item["NomAlmacen"]).Trim() + new string(' ', 40)).Substring(0, 40));
                         sbTxtPed.Append((Convert.ToString(item["ZonaOrigen"]).Trim() + new string(' ', 2)).Substring(0, 2));
                         sbTxtPed.Append((Convert.ToString(item["CodAlmDestino"]).Trim() + new string(' ', 4)).Substring(0, 4));
                         sbTxtPed.AppendLine();
                     }

                     string NombreFile = "consulta" + DateTime.Now.ToString("ddMMyy") + " " + Convert.ToString(codproveedor).Trim() + ".txt";
                     if (dt.Rows.Count > 0)
                     {
                         byte[] bytes = Encoding.ASCII.GetBytes(sbTxtPed.ToString());
                         result = Request.CreateResponse(HttpStatusCode.OK);
                         result.Content = new StreamContent(new MemoryStream(bytes));
                         result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                         result.Content.Headers.ContentDisposition.FileName = NombreFile;
                     }


                 }
                 if (tipo == "2")
                 {

                     XmlDocument xmlLstPed = new XmlDocument();
                     XmlElement xmlPed = null;
                     XmlElement xmlCab = null;
                     XmlElement xmlLstDet = null;
                     XmlElement xmlDet = null;
                     XmlElement xmlItem = null;
                     xmlLstPed.LoadXml("<?xml version=\"1.0\" encoding=\"windows-1252\" ?><pedidos />");
                     string numPed = "";
                     string fechaPedido = "";
                     foreach (DataRow item in dt.Rows)
                     {
                         if (Convert.ToString(item["NumPedido"]) != numPed || Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") != fechaPedido)
                         {
                             numPed = Convert.ToString(item["NumPedido"]);
                             fechaPedido = Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd");
                             xmlPed = xmlLstPed.CreateElement("pedido");
                             xmlCab = xmlLstPed.CreateElement("cabeceraPedido");
                             xmlLstDet = xmlLstPed.CreateElement("detallePedido");
                             xmlPed.AppendChild(xmlCab);
                             xmlPed.AppendChild(xmlLstDet);
                             xmlItem = xmlLstPed.CreateElement("empresa"); xmlItem.InnerText = "CORPORACION EL ROSADO S.A."; xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("codigoProveedor"); xmlItem.InnerText = Convert.ToString(item["CodProveedor"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("nombreProveedor"); xmlItem.InnerText = Convert.ToString(item["NomProveedor"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("numeroPedido"); xmlItem.InnerText = Convert.ToString(item["NumPedido"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("fechaPedido"); xmlItem.InnerText = Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd"); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("codigoAlmacen"); xmlItem.InnerText = Convert.ToString(item["CodAlmacen"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("nombreAlmacen"); xmlItem.InnerText = Convert.ToString(item["NomAlmacen"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("almacenDestino"); xmlItem.InnerText = Convert.ToString(item["CodAlmDestino"]); xmlCab.AppendChild(xmlItem);
                             xmlItem = xmlLstPed.CreateElement("zonaOrigen"); xmlItem.InnerText = Convert.ToString(item["ZonaOrigen"]); xmlCab.AppendChild(xmlItem);
                             xmlLstPed.DocumentElement.AppendChild(xmlPed);
                         }
                         xmlDet = xmlLstPed.CreateElement("detalle");
                         xmlItem = xmlLstPed.CreateElement("item"); xmlItem.InnerText = Convert.ToString(item["Item"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("cantidad"); xmlItem.InnerText = Convert.ToDecimal(item["CantPedido"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("articulo"); xmlItem.InnerText = Convert.ToString(item["CodArticulo"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("codigoTamano"); xmlItem.InnerText = Convert.ToString(item["Tamano"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("tamano"); xmlItem.InnerText = Convert.ToString(item["TamanoCaja"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("codigoEan"); xmlItem.InnerText = Convert.ToString(item["CodEAN"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("descripcion"); xmlItem.InnerText = Convert.ToString(item["DesArticulo"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("precioCosto"); xmlItem.InnerText = Convert.ToDecimal(item["PrecioCosto"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("UnidadesCaja"); xmlItem.InnerText = Convert.ToString(item["UndPorCaja"]); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("descuento1"); xmlItem.InnerText = Convert.ToDecimal(item["Descuento1"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("descuento2"); xmlItem.InnerText = Convert.ToDecimal(item["Descuento2"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                         xmlItem = xmlLstPed.CreateElement("indiceIva"); xmlItem.InnerText = Convert.ToString(item["IndIva1"]); xmlDet.AppendChild(xmlItem);
                         xmlLstDet.AppendChild(xmlDet);
                     }
                     string NombreFile = "consulta" + DateTime.Now.ToString("ddMMyy") + " " + Convert.ToString(codproveedor).Trim() + ".xml";

                     if (dt.Rows.Count > 0)
                     {
                         byte[] bytes = Encoding.ASCII.GetBytes(xmlLstPed.OuterXml.ToString());
                         result = Request.CreateResponse(HttpStatusCode.OK);
                         result.Content = new StreamContent(new MemoryStream(bytes));
                         result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                         result.Content.Headers.ContentDisposition.FileName = NombreFile;
                     }
                     xmlDet = null;
                     xmlItem = null;
                     xmlLstDet = null;
                     xmlCab = null;
                     xmlPed = null;
                     xmlLstPed = new XmlDocument();
                     xmlLstPed = null;
                 }

                 if (tipo == "3")
                 {


                     System.Text.StringBuilder sbJQUERY = new System.Text.StringBuilder("");
                     sbJQUERY.Append("<script type=\"text/javascript\">").AppendLine();
                     sbJQUERY.Append("  $(document).ready(function () {").AppendLine();

                     System.Text.StringBuilder sbHtmlPed = new System.Text.StringBuilder("");
                     sbHtmlPed.Append("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">").AppendLine();
                     sbHtmlPed.Append("<title>CORPORACION EL ROSADO S.A.</title>").AppendLine();
                     sbHtmlPed.Append("<script type=\"text/javascript\" src=\"http://proveedores.elrosado.com/DownloadTmp/jquery-1.3.2.min.js\"></script>").AppendLine();
                     sbHtmlPed.Append("<script type=\"text/javascript\" src=\"http://proveedores.elrosado.com/DownloadTmp/jquery-barcode.js\"></script>").AppendLine();
                     sbHtmlPed.Append("<script type=\"text/javascript\" src=\"http://proveedores.elrosado.com/DownloadTmp/angular.js\"></script>").AppendLine();
                     sbHtmlPed.Append("<link rel=\"stylesheet\" href=\"http://proveedores.elrosado.com/DownloadTmp/ace.css\"/>").AppendLine();
                     sbHtmlPed.Append("<link rel=\"stylesheet\" href=\"http://proveedores.elrosado.com/DownloadTmp/bootstrap.css\"/>").AppendLine();
                     sbHtmlPed.Append("<script type=\"text/javascript\" src=\"http://proveedores.elrosado.com/DownloadTmp/bootstrap.js\"></script>").AppendLine();

                     sbHtmlPed.Append("</head>").AppendLine();
                     sbHtmlPed.Append("").AppendLine();
                     sbHtmlPed.Append("<body>").AppendLine();
                     sbHtmlPed.Append("").AppendLine();
                     string numPed = "";
                     string fechaPedido = "";
                     int numItems = 0; long idCodBarr = 0;
                     foreach (DataRow item in dt.Rows)
                     {
                         if (Convert.ToString(item["NumPedido"]) != numPed || Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") != fechaPedido)
                         {
                             if (numPed != "")
                             {
                                 sbHtmlPed.Append("  <tr>").AppendLine();
                                 sbHtmlPed.Append("    <td width=\"709\" colspan=\"12\"><b><font size=\"1\">TOTAL DE ITEMS: " + numItems.ToString() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                                 sbHtmlPed.Append("  </tr>").AppendLine();
                                 sbHtmlPed.Append("</tbody></table>").AppendLine();
                                 sbHtmlPed.Append("<br>").AppendLine();
                                 sbHtmlPed.Append("").AppendLine();
                             }
                             idCodBarr = idCodBarr + 1;

                             sbJQUERY.Append("    $(\"#bc" + idCodBarr.ToString() + "\").barcode(\"" + Convert.ToString(item["NumPedido"]).Trim() + "\", \"code128\");").AppendLine();

                             numItems = 0;
                             numPed = Convert.ToString(item["NumPedido"]);
                             fechaPedido = Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd");
                             sbHtmlPed.Append("<div class=\"row\">").AppendLine();
                             sbHtmlPed.Append("<div class=\"col-md-4\" id=\"bc" + idCodBarr.ToString() + "\"></div>").AppendLine();
                             idCodBarr = idCodBarr + 1;

                             sbJQUERY.Append("    $(\"#bc" + idCodBarr.ToString() + "\").barcode(\"" + Convert.ToInt32(CodSap.ToString()).ToString().Trim() + "\", \"code128\");").AppendLine();
                             sbHtmlPed.Append("<div class=\"col-md-4\"></div>");
                             sbHtmlPed.Append("<div class=\"col-md-4\" id=\"bc" + idCodBarr.ToString() + "\"></div></div>").AppendLine();
                             sbHtmlPed.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolor=\"#111111\" width=\"733\" id=\"AutoNumber2\">").AppendLine();
                             sbHtmlPed.Append("  <tbody><tr>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"733\" colspan=\"12\"><b><font size=\"1\">CORPORACION EL ROSADO S. A.</font></b></td>").AppendLine();
                             sbHtmlPed.Append("  </tr>").AppendLine();
                             sbHtmlPed.Append("  <tr>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"102\" colspan=\"2\"><b><font size=\"1\">PROVEEDOR</font></b></td>").AppendLine();
                             //sbHtmlPed.Append("    <td width=\"303\" colspan=\"4\"><font size=\"1\">" + Convert.ToString(item["CodProveedor"]).Trim() + "-" + Convert.ToString(item["NomProveedor"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                             if (Convert.ToString(item["CodProveedor"]).ToString().Contains(CodSap.ToString()) == true)
                             {
                                 sbHtmlPed.Append("    <td width=\"303\" colspan=\"4\"><font size=\"1\">" + Convert.ToString(item["NomProveedor"]).Trim() + " - " + (new string('0', (10 - CodSap.Trim().Length)) + CodSap.Trim()) + "</font>&nbsp;</td>").AppendLine();
                             }
                             else
                             {
                                 sbHtmlPed.Append("    <td width=\"303\" colspan=\"4\"><font size=\"1\">" + Convert.ToString(item["NomProveedor"]).Trim() + " - " + (new string('0', (10 - CodSap.Trim().Length)) + CodSap.Trim()) + " - Cod. Legacy:" + Convert.ToString(item["CodProveedor"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                             }

                             sbHtmlPed.Append("    <td width=\"155\" colspan=\"3\"><b><font size=\"1\">ZONA DE ORIGEN</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"153\" colspan=\"3\"><font size=\"1\">" + Convert.ToString(item["ZonaOrigen"]).Trim() + "</font></td>").AppendLine();
                             sbHtmlPed.Append("  </tr>").AppendLine();
                             sbHtmlPed.Append("  <tr>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"102\" colspan=\"2\"><b><font size=\"1\">PEDIDOS POR</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"103\"><font size=\"1\">" + Convert.ToString(item["CodAlmacen"]).Trim() + "-" + Convert.ToString(item["NomAlmacen"]).Trim() + "</font></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"71\"><b><font size=\"1\">ALMACEN</font><font size=\"1\"> DESTINO</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"73\"><font size=\"1\">" + Convert.ToString(item["CodAlmDestino"]).Trim() + "</font></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"52\">&nbsp;</td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"63\"><b><font size=\"1\">FECHA DEL PEDIDO</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"34\"><font size=\"1\">" + Convert.ToDateTime(item["FechaPedido"]).ToString("yyyy.MM.dd") + "</font></td>").AppendLine();
                            sbHtmlPed.Append("    <td width=\"52\"><b><font size=\"1\"></font></b></td>").AppendLine();
                            sbHtmlPed.Append("    <td width=\"157\" colspan=\"4\"><font size=\"1\"></font></td>").AppendLine();
                            //sbHtmlPed.Append("    <td width=\"52\"><b><font size=\"1\">FECHA DE ENTREGA</font></b></td>").AppendLine();
                            //sbHtmlPed.Append("    <td width=\"157\" colspan=\"4\"><font size=\"1\">" + Convert.ToDateTime(item["FecEntregaActual"]).ToString("yyyy.MM.dd") + "</font></td>").AppendLine();
                            sbHtmlPed.Append("  </tr>").AppendLine();
                             sbHtmlPed.Append("  <tr>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"102\" colspan=\"2\"><b><font size=\"1\">NUMERO DE ORDEN</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"625\" colspan=\"10\"><b><font size=\"1\">" + Convert.ToString(item["NumPedido"]).Trim() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                             sbHtmlPed.Append("  </tr>").AppendLine();
                             sbHtmlPed.Append("  <tr>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"2 7\" align=\"right\">").AppendLine();
                             sbHtmlPed.Append("    <p align=\"center\"><b><font size=\"1\">ITEN</font></b></p></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"73\"><b><font size=\"1\">ARTICULO</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"249\" colspan=\"3\"><b><font size=\"1\">DESCRIPCION&nbsp;&nbsp; </font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"52\"><b><font size=\"1\">REFENCIA</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"53\" align=\"left\"><p align=\"left\"><b><font size=\"1\">TAMA&Ntilde;O</font></b></p></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"34\"><b><font size=\"1\">UXC</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"54\"><b><font size=\"1\">CANTIDAD</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"36\"><b><font size=\"1\">COSTO</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"55\"><b><font size=\"1\">DESCTO</font><font size=\"1\"> 1</font></b></td>").AppendLine();
                             sbHtmlPed.Append("    <td width=\"57\"><b><font size=\"1\">DESCTO</font><font size=\"1\"> 2</font></b></td>").AppendLine();
                             sbHtmlPed.Append("  </tr>").AppendLine();
                             sbHtmlPed.Append("").AppendLine();
                         }
                         numItems = numItems + 1;
                         sbHtmlPed.Append("  <tr>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"27\" align=\"center\"><font size=\"1\">" + Convert.ToString(item["Item"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"73\" align=\"center\"><font size=\"1\">" + Convert.ToString(item["CodArticulo"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"249\" colspan=\"3\"><font size=\"1\">" + Convert.ToString(item["DesArticulo"]).Trim() + "</font></td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"52\"><font size=\"1\">" + Convert.ToString(item["Tamano"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"53\" align=\"left\"><font size=\"1\">" + Convert.ToString(item["TamanoCaja"]).Trim() + "</font></td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"34\" align=\"right\"><font size=\"1\">" + Convert.ToString(item["UndPorCaja"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"54\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["CantPedido"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"36\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["PrecioCosto"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"45\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["Descuento1"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("    <td width=\"47\" align=\"right\"><font size=\"1\">" + Convert.ToDecimal(item["Descuento2"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                         sbHtmlPed.Append("  </tr>").AppendLine();
                         sbHtmlPed.Append("").AppendLine();
                     }
                     sbHtmlPed.Append("  <tr>").AppendLine();
                     sbHtmlPed.Append("    <td width=\"709\" colspan=\"12\"><b><font size=\"1\">TOTAL DE ITEMS: " + numItems.ToString() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                     sbHtmlPed.Append("  </tr>").AppendLine();
                     sbHtmlPed.Append("</tbody></table>").AppendLine();
                     sbHtmlPed.Append("<br>").AppendLine();
                     sbHtmlPed.Append("").AppendLine();

                     sbJQUERY.Append("  });").AppendLine();
                     sbJQUERY.Append("</script>").AppendLine();
                     sbJQUERY.Append("").AppendLine();

                     sbHtmlPed.Append(sbJQUERY.ToString());
                     sbHtmlPed.Append("</body></html>");


                     string NombreFile = "consulta" + DateTime.Now.ToString("ddMMyy") + " " + Convert.ToString(codproveedor).Trim() + ".html";

                     if (dt.Rows.Count > 0)
                     {
                         byte[] bytes = Encoding.ASCII.GetBytes(sbHtmlPed.ToString());
                         result = Request.CreateResponse(HttpStatusCode.OK);
                         result.Content = new StreamContent(new MemoryStream(bytes));
                         result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                         result.Content.Headers.ContentDisposition.FileName = NombreFile;
                     }
                     sbJQUERY = null;
                     sbHtmlPed = null;
                 }

                 if (tipo == "4")
                 {
                     string archivo = "";
                     ReportDataSource rptDataSourcecab;
                     rptDataSourcecab = new ReportDataSource("rptPedidos", dt);
                     ReportViewer auxc = new ReportViewer();
                     Warning[] warnings = null;
                     string[] streamids = null;
                     string mimeType = "";
                     string encoding = "";
                     string extension = "";

                     auxc.ProcessingMode = ProcessingMode.Local;
                     auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/reportePedido.rdlc");

                     auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                     byte[] bytes = null;
                     //if (tipo == "1")
                     //{
                     //    archivo = "Reportepedidos" + CodSap + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xls";
                     //    bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                     //}
                     //if (tipo == "2")
                     //{
                     archivo = "Reportepedidos" + CodSap + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".pdf";
                     bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                     //

                     if (dt.Rows.Count > 0)
                     {
                         result = Request.CreateResponse(HttpStatusCode.OK);
                         result.Content = new StreamContent(new MemoryStream(bytes));
                         result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                         result.Content.Headers.ContentDisposition.FileName = archivo;
                     }

                     //strRespTxt = RutaUrlWebAPP + archivo;
                     //archivo = RutaDirWebAPP + archivo;
                     //FileStream fs = System.IO.File.Create(archivo);
                     //fs.Write(bytes, 0, bytes.Length);
                     //fs.Close();
                     //auxc.Dispose();

                 }
             }
             catch (Exception e)
             {

             }

             return result;
         }
        private string reporte(DataTable data,string tipo,string codSap)
        {
            string result = "";
            string archivo = "";
            try
            {
                ReportDataSource rptDataSourcecab;
                string RutaUrlWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaVirtualDownload"]).Trim();
                string RutaDirWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaFisicaDownload"]).Trim();
              
                DataTable dt = new DataTable("rptPedidos");
                dt.Columns.Add("Origen", System.Type.GetType("System.String"));
                dt.Columns.Add("IdPedido", System.Type.GetType("System.String"));
                dt.Columns.Add("NumPedido", System.Type.GetType("System.String"));
                dt.Columns.Add("CodAlmacen", System.Type.GetType("System.String"));
                dt.Columns.Add("NomAlmacen", System.Type.GetType("System.String"));
                dt.Columns.Add("FechaPedido", System.Type.GetType("System.String"));
                dt.Columns.Add("CodAlmDestino", System.Type.GetType("System.String"));
                dt.Columns.Add("CodProveedor", System.Type.GetType("System.String"));
                dt.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                dt.Columns.Add("ZonaOrigen", System.Type.GetType("System.String"));
                dt.Columns.Add("Item", System.Type.GetType("System.String"));
                dt.Columns.Add("CodArticulo", System.Type.GetType("System.String"));
                dt.Columns.Add("DesArticulo", System.Type.GetType("System.String"));
                dt.Columns.Add("Tamano", System.Type.GetType("System.String"));
                dt.Columns.Add("CantPedido", System.Type.GetType("System.String"));
                dt.Columns.Add("PrecioCosto", System.Type.GetType("System.String"));
                dt.Columns.Add("UndPorCaja", System.Type.GetType("System.String"));
                dt.Columns.Add("Descuento1", System.Type.GetType("System.String"));
                dt.Columns.Add("Descuento2", System.Type.GetType("System.String"));
                dt.Columns.Add("IndIva1", System.Type.GetType("System.String"));
                dt.Columns.Add("TamanoCaja", System.Type.GetType("System.String"));
                dt.Columns.Add("CodEAN", System.Type.GetType("System.String"));
                dt.Columns.Add("EsDescargado", System.Type.GetType("System.String"));
                dt.Columns.Add("EsImpreso", System.Type.GetType("System.String"));
                dt.Columns.Add("numPedidoImagen", System.Type.GetType("System.Byte[]"));
                dt.Columns.Add("codProveedorImagen", System.Type.GetType("System.Byte[]"));
                dt.Columns.Add("codSap", System.Type.GetType("System.String"));
                dt.Columns.Add("CodProveedorint", System.Type.GetType("System.String"));
                foreach (DataRow dr in data.Rows)
                {
                    DataRow drowd = dt.NewRow();
                    drowd["Origen"] = dr.ItemArray[0].ToString();
                    drowd["IdPedido"] = dr.ItemArray[1].ToString();
                    drowd["NumPedido"] = dr.ItemArray[2].ToString();
                    drowd["CodAlmacen"] = dr.ItemArray[3].ToString();
                    drowd["NomAlmacen"] = dr.ItemArray[4].ToString();
                    drowd["FechaPedido"] = dr.ItemArray[5].ToString().Substring(6, 4) + "." + dr.ItemArray[5].ToString().Substring(3, 2) + "." + dr.ItemArray[5].ToString().Substring(0, 2);
                    drowd["CodAlmDestino"] = dr.ItemArray[6].ToString();
                    drowd["CodProveedor"] = dr.ItemArray[7].ToString();
                    drowd["NomProveedor"] = dr.ItemArray[8].ToString();
                    drowd["ZonaOrigen"] = dr.ItemArray[9].ToString();
                    drowd["Item"] = dr.ItemArray[10].ToString();
                    drowd["CodArticulo"] = dr.ItemArray[11].ToString();
                    drowd["DesArticulo"] = dr.ItemArray[12].ToString();
                    drowd["Tamano"] = dr.ItemArray[13].ToString();
                    drowd["CantPedido"] = dr.ItemArray[14].ToString();
                    drowd["PrecioCosto"] = dr.ItemArray[15].ToString();
                    drowd["UndPorCaja"] = dr.ItemArray[16].ToString();
                    drowd["Descuento1"] = dr.ItemArray[17].ToString();
                    drowd["Descuento2"] = dr.ItemArray[18].ToString();
                    drowd["IndIva1"] = dr.ItemArray[19].ToString();
                    drowd["TamanoCaja"] = dr.ItemArray[20].ToString();
                    drowd["CodEAN"] = dr.ItemArray[21].ToString();
                    drowd["EsDescargado"] = dr.ItemArray[22].ToString();
                    drowd["EsImpreso"] = dr.ItemArray[23].ToString();

                  

                    var ruta = RutaDirWebAPP + "0" + Convert.ToString(drowd["NumPedido"]) + ".png";
                    GenCode128.Code128Rendering.MakeBarcodeImage("0" + Convert.ToString(drowd["NumPedido"]), 1, true).Save(ruta, System.Drawing.Imaging.ImageFormat.Png);
                    drowd["numPedidoImagen"] = File.ReadAllBytes(ruta);
                    var ruta1 = RutaDirWebAPP + "0" + Convert.ToString(drowd["CodProveedor"]) + ".png";
                    GenCode128.Code128Rendering.MakeBarcodeImage("0" + Convert.ToString(Convert.ToDouble(drowd["CodProveedor"])), 1, true).Save(ruta1, System.Drawing.Imaging.ImageFormat.Png);
                    drowd["codProveedorImagen"] = File.ReadAllBytes(ruta1);
                    drowd["codSap"] = codSap;
                    drowd["CodProveedorint"] = Convert.ToDouble(dr.ItemArray[7].ToString());
                    
                    dt.Rows.Add(drowd);
                }
                rptDataSourcecab = new ReportDataSource("rptPedidos", dt);
                ReportViewer auxc = new ReportViewer();
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = "";
                string encoding = "";
                string extension = "";
       
                auxc.ProcessingMode = ProcessingMode.Local;
                auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/reportePedido.rdlc");
                
                auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                byte[] bytes = null;
                if (tipo == "1")
                {
                    archivo = "Reportepedidos" + codSap + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xls";
                    bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                }
                else
                {
                    archivo = "Reportepedidos" + codSap + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".pdf";
                    bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                }


                result = RutaUrlWebAPP + archivo;
                archivo =RutaDirWebAPP + archivo;
                FileStream fs = System.IO.File.Create(archivo);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                auxc.Dispose();
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }

        [AntiForgeryValidate]
        [ActionName("UrlRedireccSiteFactManual")]
        [HttpGet]
        public formResponsePedidos GetUrlRedireccSiteFactManual(string Opc, string CodSap, string Ruc, string Usuario)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            try
            {
                List<object> TmpList = new List<object>();
                string urlRedirect = ((string)System.Configuration.ConfigurationManager.AppSettings["Url_SitioFactManual"]).Trim();
                if (Opc == "P")
                {
                    urlRedirect = urlRedirect + "pedidos/frmConsPedidos.aspx";
                }
                if (Opc == "F")
                {
                    urlRedirect = urlRedirect + "facturacion/frmConsFacturas.aspx";
                }
                clibEncriptaQS.clsEncriptaQS objEnc = new clibEncriptaQS.clsEncriptaQS();
                urlRedirect = urlRedirect + "?params=" + objEnc.Encryp("CodProveedorSAP=" + CodSap + "&usuario=" + Usuario);
                TmpList.Add(urlRedirect);
                FormResponse.root.Add(TmpList);
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        //J. Navarrete - 13-10-2015 [Contingencia]
         [AntiForgeryValidate]
        [ActionName("ConsEnvPedidos")]
        [HttpGet]
        [AntiForgeryValidate]
        public formResponsePedidos GetConsEnvPedidos(string tipo, string FechaIni, string FechaFin, string Ruc, bool SiTxt, bool SiXml, bool SiPdf,
                                               string accion, string codProveedor, string numPedido, string ConsPestados, string ConsP2, string Destinatarios
                                              )
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
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("FechaIni", FechaIni);
                xmlParam.DocumentElement.SetAttribute("FechaFin", FechaFin);
                xmlParam.DocumentElement.SetAttribute("Accion", accion == null ? "" : accion);
                xmlParam.DocumentElement.SetAttribute("CodProveedor", codProveedor == null ? "" : codProveedor);
                xmlParam.DocumentElement.SetAttribute("NumPedido", numPedido == null ? "" : numPedido);
                if (!String.IsNullOrEmpty(ConsPestados))
                {
                    foreach (string it in ConsPestados.Split(new char[] { '|' }))
                    {
                        if (it != "")
                        {
                            XmlElement elem = xmlParam.CreateElement("Est");
                            elem.SetAttribute("id", it);
                            xmlParam.DocumentElement.AppendChild(elem);
                        }
                    }
                }

                if (!String.IsNullOrEmpty(ConsP2))
                {
                    foreach (string it in ConsP2.Split(new char[] { '|' }))
                    {
                        if (it != "")
                        {
                            XmlElement elem = xmlParam.CreateElement("Alm");
                            elem.SetAttribute("id", it);
                            xmlParam.DocumentElement.AppendChild(elem);
                        }
                    }
                }

                //Transacción 504 - SIPE_PROVEEDOR_CERT.PEDIDOS.PED_P_CONSENVPEDIDOS
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 504, 1);

                // FIN - CONSULTO a STORE PROCEDURE DE BD

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();

                    if (tipo == "1")
                    {
                        // INI - ARMADO DE ESTRUCTURA PARA GRID
                        List<pedConsPedidosF> TmpLstCons = new List<pedConsPedidosF>();
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            pedConsPedidosF TmpItem = new pedConsPedidosF();
                            TmpItem.pNumPedido = Convert.ToString(item["NumPedido"]);
                            TmpItem.pFechaPedido = Convert.ToDateTime(item["FechaPedido"]);
                            TmpItem.pCodAlmacen = Convert.ToString(item["CodAlmacen"]);
                            TmpItem.pNomAlmacen = Convert.ToString(item["NomAlmacen"]);
                            TmpItem.pCodAlmDestino = Convert.ToString(item["CodAlmDestino"]);
                            TmpItem.pZonaOrigen = Convert.ToString(item["ZonaOrigen"]);

                            TmpItem.pCantPedido = Convert.ToDecimal(item["ValorTotalPedido"]);
                            TmpItem.pPrecioCosto = Convert.ToDecimal(item["TotalSumaFacturas"]);
                            TmpItem.pDescuento1 = Convert.ToDecimal(item["ValorTotalPedido"]) - Convert.ToDecimal(item["TotalSumaFacturas"]);

                            TmpItem.pRuc = Convert.ToString(item["Ruc"]);
                            TmpItem.pIdPedido = Convert.ToString(item["IdPedido"]);
                            TmpItem.pCodProveedor = Convert.ToString(item["CodProveedor"]);
                            TmpItem.pNomProveedor = Convert.ToString(item["NomProveedor"]);
                            TmpItem.fecEntregaActual = Convert.ToDateTime(item["FecEntregaActual"]);
                            //TmpItem.pOrigen = Convert.ToString(item["Origen"]);


                            //TmpItem.pItem = Convert.ToString(item["Item"]);
                            //TmpItem.pCodArticulo = Convert.ToString(item["CodArticulo"]);
                            //TmpItem.pDesArticulo = Convert.ToString(item["DesArticulo"]);
                            //TmpItem.pTamano = Convert.ToString(item["Tamano"]);                            
                            //TmpItem.pPrecioCosto = Convert.ToDecimal(item["PrecioCosto"]);
                            //TmpItem.pUndPorCaja = Convert.ToString(item["UndPorCaja"]);
                            //TmpItem.pDescuento1 = Convert.ToDecimal(item["Descuento1"]);
                            //TmpItem.pDescuento2 = Convert.ToDecimal(item["Descuento2"]);
                            //TmpItem.pIndIva1 = Convert.ToString(item["IndIva1"]);
                            //TmpItem.pTamanoCaja = Convert.ToString(item["TamanoCaja"]);
                            //TmpItem.pCodEAN = Convert.ToString(item["CodEAN"]);


                            TmpLstCons.Add(TmpItem);
                        }
                        TmpList.Add(TmpLstCons);
                        // FIN - ARMADO DE ESTRUCTURA PARA GRID
                    }

                    //Envio de Mails masivos
                    if (tipo == "2")
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //Falta consumo de BAPI para correos SAP
                            string rutaEmail = System.Web.Hosting.HostingEnvironment.MapPath("~/PlantillasEMail");
                            string asuntoEmail = "Pedidos - Portal de Proveedores";
                            rutaEmail = rutaEmail + "\\ContingenciaPedido.htm";

                            //Tabla que almacena los datos para envio de mail
                            DataTable dtmails = new DataTable();
                            dtmails.Columns.Add("zipFilename", typeof(System.String));
                            dtmails.Columns.Add("mensajeEmail", typeof(System.String));
                            dtmails.Columns.Add("correo", typeof(System.String));
                            dtmails.Columns.Add("proveedor", typeof(System.String));



                            var codCorreoProv = "";
                            var rucCorreoProv = "";
                            var razCorreoProv = "";
                            System.Text.StringBuilder sbTxtPed = new System.Text.StringBuilder("");
                            foreach (DataRow itxt in ds.Tables[0].Rows)
                            {
                                codCorreoProv = Convert.ToString(itxt["CodProveedor"]);
                                rucCorreoProv = Convert.ToString(itxt["Ruc"]);
                                razCorreoProv = Convert.ToString(itxt["NomProveedor"]);
                                sbTxtPed.Append((Convert.ToString(itxt["CodProveedor"]).Trim() + new string(' ', 10)).Substring(0, 10));
                                sbTxtPed.Append((Convert.ToString(itxt["NomProveedor"]).Trim() + new string(' ', 40)).Substring(0, 40));
                                sbTxtPed.Append((Convert.ToString(itxt["NumPedido"]).Trim() + new string(' ', 10)).Substring(0, 10));
                                sbTxtPed.Append((Convert.ToString(itxt["Item"]).Trim() + new string(' ', 5)).Substring(0, 5));
                                sbTxtPed.Append((Convert.ToDateTime(itxt["FechaPedido"]).ToString("yyyy.MM.dd") + new string(' ', 10)).Substring(0, 10));
                                sbTxtPed.Append((Convert.ToDecimal(itxt["CantPedido"]).ToString("##0.00").Replace(",", ".") + new string(' ', 12)).Substring(0, 12));
                                sbTxtPed.Append((Convert.ToString(itxt["CodArticulo"]).Trim() + new string(' ', 18)).Substring(0, 18));
                                sbTxtPed.Append((Convert.ToString(itxt["Tamano"]).Trim() + new string(' ', 18)).Substring(0, 18));
                                sbTxtPed.Append((Convert.ToString(itxt["TamanoCaja"]).Trim() + new string(' ', 10)).Substring(0, 10));
                                sbTxtPed.Append((Convert.ToString(itxt["CodEAN"]).Trim() + new string(' ', 13)).Substring(0, 13));
                                sbTxtPed.Append((Convert.ToString(itxt["DesArticulo"]).Trim() + new string(' ', 40)).Substring(0, 40));
                                sbTxtPed.Append((Convert.ToDecimal(itxt["PrecioCosto"]).ToString("##0.00").Replace(",", ".") + new string(' ', 14)).Substring(0, 14));
                                sbTxtPed.Append((Convert.ToString(itxt["UndPorCaja"]).Trim() + new string(' ', 6)).Substring(0, 6));
                                sbTxtPed.Append((Convert.ToDecimal(itxt["Descuento1"]).ToString("##0.00").Replace(",", ".") + new string(' ', 5)).Substring(0, 5));
                                sbTxtPed.Append((Convert.ToDecimal(itxt["Descuento2"]).ToString("##0.00").Replace(",", ".") + new string(' ', 5)).Substring(0, 5));
                                sbTxtPed.Append((Convert.ToString(itxt["IndIva1"]).Trim() + new string(' ', 1)).Substring(0, 1));
                                sbTxtPed.Append((Convert.ToString(itxt["CodAlmacen"]).Trim() + new string(' ', 4)).Substring(0, 4));
                                sbTxtPed.Append((Convert.ToString(itxt["NomAlmacen"]).Trim() + new string(' ', 40)).Substring(0, 40));
                                sbTxtPed.Append((Convert.ToString(itxt["ZonaOrigen"]).Trim() + new string(' ', 2)).Substring(0, 2));
                                sbTxtPed.Append((Convert.ToString(itxt["CodAlmDestino"]).Trim() + new string(' ', 4)).Substring(0, 4));
                                sbTxtPed.AppendLine();
                            }

                            ZipFile zip = new ZipFile();
                            string zipFilename = Path.Combine(((string)System.Configuration.ConfigurationManager.AppSettings["RutaMailPedidos"]).Trim(), "Pedidos" + "-" +
                                                 codCorreoProv + "-" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".zip");

                            string txtFilename = "";
                            string RutaDirWebAPP = "";
                            string NombreFile = "";

                            // INI - ARMADO DE ARCHIVO DE TEXTO
                            //Siempre genera el txt para poder poblar el pdf


                            RutaDirWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaMailPedidos"]).Trim();
                            NombreFile = "Pedidos" + "-" + codCorreoProv + "-" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                            //NombreFile = "Pedidos" + "-" + Convert.ToString(item["CodProveedor"]) + "-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
                            if (File.Exists(RutaDirWebAPP + NombreFile) == false)
                            {
                                System.IO.File.WriteAllText(RutaDirWebAPP + NombreFile, sbTxtPed.ToString());
                                txtFilename = Path.Combine(RutaDirWebAPP, NombreFile);
                                if (SiTxt)
                                {
                                    zip.AddFile(txtFilename);
                                    sbTxtPed = null;
                                }

                            }


                            //Solo si tiene el check lo zipea

                            // FIN - ARMADO DE ARCHIVO DE TEXTO

                            // INI - ARMADO DE ARCHIVO XML
                            if (SiXml)
                            {
                                RutaDirWebAPP = "";
                                NombreFile = "";
                                XmlDocument xmlLstPed = new XmlDocument();
                                XmlElement xmlPed = null;
                                XmlElement xmlCab = null;
                                XmlElement xmlLstDet = null;
                                XmlElement xmlDet = null;
                                XmlElement xmlItem = null;
                                xmlLstPed.LoadXml("<?xml version=\"1.0\" encoding=\"windows-1252\" ?><pedidos />");
                                string numPed = "";
                                string fechaPedido = "";
                                foreach (DataRow itxm in ds.Tables[0].Rows)
                                {
                                    if (Convert.ToString(itxm["NumPedido"]) != numPed || Convert.ToDateTime(itxm["FechaPedido"]).ToString("yyyy.MM.dd") != fechaPedido)
                                    {
                                        numPed = Convert.ToString(itxm["NumPedido"]);
                                        fechaPedido = Convert.ToDateTime(itxm["FechaPedido"]).ToString("yyyy.MM.dd");
                                        xmlPed = xmlLstPed.CreateElement("pedido");
                                        xmlCab = xmlLstPed.CreateElement("cabeceraPedido");
                                        xmlLstDet = xmlLstPed.CreateElement("detallePedido");
                                        xmlPed.AppendChild(xmlCab);
                                        xmlPed.AppendChild(xmlLstDet);
                                        xmlItem = xmlLstPed.CreateElement("empresa"); xmlItem.InnerText = "CORPORACION EL ROSADO S.A."; xmlCab.AppendChild(xmlItem);
                                        xmlItem = xmlLstPed.CreateElement("codigoProveedor"); xmlItem.InnerText = Convert.ToString(itxm["CodProveedor"]); xmlCab.AppendChild(xmlItem);
                                        xmlItem = xmlLstPed.CreateElement("nombreProveedor"); xmlItem.InnerText = Convert.ToString(itxm["NomProveedor"]); xmlCab.AppendChild(xmlItem);
                                        xmlItem = xmlLstPed.CreateElement("numeroPedido"); xmlItem.InnerText = Convert.ToString(itxm["NumPedido"]); xmlCab.AppendChild(xmlItem);
                                        xmlItem = xmlLstPed.CreateElement("fechaPedido"); xmlItem.InnerText = Convert.ToDateTime(itxm["FechaPedido"]).ToString("yyyy.MM.dd"); xmlCab.AppendChild(xmlItem);
                                        xmlItem = xmlLstPed.CreateElement("codigoAlmacen"); xmlItem.InnerText = Convert.ToString(itxm["CodAlmacen"]); xmlCab.AppendChild(xmlItem);
                                        xmlItem = xmlLstPed.CreateElement("nombreAlmacen"); xmlItem.InnerText = Convert.ToString(itxm["NomAlmacen"]); xmlCab.AppendChild(xmlItem);
                                        xmlItem = xmlLstPed.CreateElement("almacenDestino"); xmlItem.InnerText = Convert.ToString(itxm["CodAlmDestino"]); xmlCab.AppendChild(xmlItem);
                                        xmlItem = xmlLstPed.CreateElement("zonaOrigen"); xmlItem.InnerText = Convert.ToString(itxm["ZonaOrigen"]); xmlCab.AppendChild(xmlItem);
                                        xmlLstPed.DocumentElement.AppendChild(xmlPed);
                                    }
                                    xmlDet = xmlLstPed.CreateElement("detalle");
                                    xmlItem = xmlLstPed.CreateElement("itxm"); xmlItem.InnerText = Convert.ToString(itxm["Item"]); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("cantidad"); xmlItem.InnerText = Convert.ToDecimal(itxm["CantPedido"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("articulo"); xmlItem.InnerText = Convert.ToString(itxm["CodArticulo"]); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("codigoTamano"); xmlItem.InnerText = Convert.ToString(itxm["Tamano"]); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("tamano"); xmlItem.InnerText = Convert.ToString(itxm["TamanoCaja"]); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("codigoEan"); xmlItem.InnerText = Convert.ToString(itxm["CodEAN"]); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("descripcion"); xmlItem.InnerText = Convert.ToString(itxm["DesArticulo"]); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("precioCosto"); xmlItem.InnerText = Convert.ToDecimal(itxm["PrecioCosto"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("UnidadesCaja"); xmlItem.InnerText = Convert.ToString(itxm["UndPorCaja"]); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("descuento1"); xmlItem.InnerText = Convert.ToDecimal(itxm["Descuento1"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("descuento2"); xmlItem.InnerText = Convert.ToDecimal(itxm["Descuento2"]).ToString("##0.00").Replace(",", "."); xmlDet.AppendChild(xmlItem);
                                    xmlItem = xmlLstPed.CreateElement("indiceIva"); xmlItem.InnerText = Convert.ToString(itxm["IndIva1"]); xmlDet.AppendChild(xmlItem);
                                    xmlLstDet.AppendChild(xmlDet);
                                }
                                //string RutaDirWebAPP = HttpContext.Current.Server.MapPath("~/DownloadedDocuments/");
                                RutaDirWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaMailPedidos"]).Trim();
                                NombreFile = "Pedidos" + "-" + codCorreoProv + "-" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xml";
                                //NombreFile = "Pedidos" + "-" + Convert.ToString(item["CodProveedor"]) + "-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";
                                if (File.Exists(RutaDirWebAPP + NombreFile) == false)
                                {
                                    System.IO.File.WriteAllText(RutaDirWebAPP + NombreFile, xmlLstPed.OuterXml);
                                    string xmlFilename = Path.Combine(RutaDirWebAPP, NombreFile);
                                    zip.AddFile(xmlFilename);
                                }
                                //    System.IO.File.WriteAllText(RutaDirWebAPP + NombreFile, xmlLstPed.OuterXml);


                                xmlDet = null;
                                xmlItem = null;
                                xmlLstDet = null;
                                xmlCab = null;
                                xmlPed = null;
                                xmlLstPed = new XmlDocument();
                                xmlLstPed = null;
                            }
                            // FIN - ARMADO DE ARCHIVO XML

                            // INI - ARMADO DE ARCHIVO PDF
                            if (SiPdf)
                            {

                                System.Text.StringBuilder sbHtmlPed = new System.Text.StringBuilder("");
                                sbHtmlPed.Append("<html><head>").AppendLine();
                                sbHtmlPed.Append("<title>CORPORACIÓN EL ROSADO S.A.</title><br/><br/>").AppendLine();
                                sbHtmlPed.Append("<script type=\"text/javascript\" src=\"http://proveedores.elrosado.com/DownloadTmp/jquery-1.3.2.min.js\"></script>").AppendLine();
                                sbHtmlPed.Append("<script type=\"text/javascript\" src=\"http://proveedores.elrosado.com/DownloadTmp/jquery-barcode.js\"></script>").AppendLine();
                                sbHtmlPed.Append("<script type=\"text/javascript\" src=\"http://proveedores.elrosado.com/DownloadTmp/angular.js\"></script>").AppendLine();
                                sbHtmlPed.Append("<link rel=\"stylesheet\" href=\"http://proveedores.elrosado.com/DownloadTmp/ace.css\"/>").AppendLine();
                                sbHtmlPed.Append("<link rel=\"stylesheet\" href=\"http://proveedores.elrosado.com/DownloadTmp/bootstrap.css\"/>").AppendLine();
                                sbHtmlPed.Append("<script type=\"text/javascript\" src=\"http://proveedores.elrosado.com/DownloadTmp/bootstrap.js\"></script>").AppendLine();
                                sbHtmlPed.Append("</head>").AppendLine();
                                sbHtmlPed.Append("").AppendLine();
                                sbHtmlPed.Append("<body>").AppendLine();
                                sbHtmlPed.Append("").AppendLine();
                                string numPed = "";
                                int numItems = 0; long idCodBarr = 0;
                                string fechaPedido = "";
                                foreach (DataRow rw in ds.Tables[0].Rows)
                                {
                                    if (Convert.ToString(rw["NumPedido"]) != numPed || Convert.ToDateTime(rw["FechaPedido"]).ToString("yyyy.MM.dd") != fechaPedido)
                                    {
                                        if (numPed != "")
                                        {
                                            sbHtmlPed.Append("  <tr>").AppendLine();
                                            sbHtmlPed.Append("    <td style=\"width:709px;\" colspan=\"12\"><b><font size=\"1\">TOTAL DE ÍTEMS: " + numItems.ToString() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                                            sbHtmlPed.Append("  </tr>").AppendLine();
                                            sbHtmlPed.Append("</tbody></table>").AppendLine();
                                            sbHtmlPed.Append("<br>").AppendLine();
                                            sbHtmlPed.Append("").AppendLine();
                                        }
                                        idCodBarr = idCodBarr + 1;

                                        numItems = 0;
                                        numPed = Convert.ToString(rw["NumPedido"]);
                                        fechaPedido = Convert.ToDateTime(rw["FechaPedido"]).ToString("yyyy.MM.dd");
                                        //Genera el codigo de barra
                                        iTextSharp.text.pdf.Barcode128 code128 = new iTextSharp.text.pdf.Barcode128();
                                        code128.CodeType = iTextSharp.text.pdf.Barcode.CODABAR;
                                        code128.ChecksumText = true;
                                        code128.GenerateChecksum = true;
                                        code128.StartStopText = true;
                                        code128.Code = Convert.ToString(rw["NumPedido"]).Trim();
                                        string codename = Path.Combine(((string)System.Configuration.ConfigurationManager.AppSettings["RutaMailPedidos"]).Trim(), "Code128" + "-" +
                                                             Convert.ToString(rw["NumPedido"]).Trim() + ".png");
                                        code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White).
                                            Save(codename, System.Drawing.Imaging.ImageFormat.Png);
                                        //Genera el codigo de barra del proveedor
                                        iTextSharp.text.pdf.Barcode128 code1280 = new iTextSharp.text.pdf.Barcode128();
                                        code1280.CodeType = iTextSharp.text.pdf.Barcode.CODABAR;
                                        code1280.ChecksumText = true;
                                        code1280.GenerateChecksum = true;
                                        code1280.StartStopText = true;
                                        code1280.Code = Convert.ToString(rw["codProveedor"]).Trim();
                                        string codename0 = Path.Combine(((string)System.Configuration.ConfigurationManager.AppSettings["RutaMailPedidos"]).Trim(), "Code1280" + "-" +
                                                             Convert.ToString(rw["codProveedor"]).Trim() + ".png");
                                        code1280.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White).
                                            Save(codename0, System.Drawing.Imaging.ImageFormat.Png);


                                        sbHtmlPed.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolor=\"#111111\" width=\"733\" id=\"AutoNumber2\">").AppendLine();
                                        sbHtmlPed.Append("  <tbody><tr>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:50%;\" colspan=\"12\"><img src=\"" + codename + "\" /></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:50%;\" colspan=\"12\"><img src=\"" + codename0 + "\" /></td>").AppendLine();
                                        sbHtmlPed.Append("  </tr>").AppendLine();
                                        sbHtmlPed.Append("  <tr>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:50%;\" colspan=\"12\">" + Convert.ToString(rw["NumPedido"]).Trim() + "</td>").AppendLine();

                                        sbHtmlPed.Append("    <td style=\"width:50%;\" colspan=\"12\">" + Convert.ToString(rw["codProveedor"]).Trim() + "</td>").AppendLine();
                                        sbHtmlPed.Append("  </tr>").AppendLine();
                                        sbHtmlPed.Append("  </table>").AppendLine();

                                        sbHtmlPed.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolor=\"#111111\" width=\"733\" id=\"AutoNumber2\">").AppendLine();
                                        sbHtmlPed.Append("  <tbody><tr>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:733px;\" colspan=\"12\"><b><font size=\"1\">CORPORACIÓN EL ROSADO S. A.</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("  </tr>").AppendLine();
                                        sbHtmlPed.Append("  <tr>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:102px;\" colspan=\"2\"><b><font size=\"1\">PROVEEDOR</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:303px;\" colspan=\"4\"><font size=\"1\">" + Convert.ToString(rw["NomProveedor"]).Trim() + " - " + (new string('0', (10 - Convert.ToString(codCorreoProv).Trim().Length)) + Convert.ToString(codCorreoProv).Trim()) + "</font>&nbsp;</td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:155px;\" colspan=\"3\"><b><font size=\"1\">ZONA DE ORIGEN</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:153px;\" colspan=\"3\"><font size=\"1\">" + Convert.ToString(rw["ZonaOrigen"]).Trim() + "</font></td>").AppendLine();
                                        sbHtmlPed.Append("  </tr>").AppendLine();
                                        sbHtmlPed.Append("  <tr>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:102px;\" colspan=\"2\"><b><font size=\"1\">PEDIDOS POR</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:103px;\"><font size=\"1\">" + Convert.ToString(rw["CodAlmacen"]).Trim() + "-" + Convert.ToString(rw["NomAlmacen"]).Trim() + "</font></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:71px;\"><b><font size=\"1\">ALMACÉN</font><font size=\"1\"> DESTINO</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:73px;\"><font size=\"1\">" + Convert.ToString(rw["CodAlmDestino"]).Trim() + "</font></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:52px;\">&nbsp;</td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:63px;\"><b><font size=\"1\">FECHA DEL PEDIDO</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:34px;\"><font size=\"1\">" + Convert.ToDateTime(rw["FechaPedido"]).ToString("yyyy.MM.dd") + "</font></td>").AppendLine();
                                        sbHtmlPed.Append("    <td width=\"52\"><b><font size=\"1\"></font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td width=\"157\" colspan=\"4\"><font size=\"1\"></font></td>").AppendLine();
                                        //sbHtmlPed.Append("    <td width=\"52\"><b><font size=\"1\">FECHA DE ENTREGA</font></b></td>").AppendLine();
                                        //sbHtmlPed.Append("    <td width=\"157\" colspan=\"4\"><font size=\"1\">" + Convert.ToDateTime(rw["FecEntregaActual"]).ToString("yyyy.MM.dd") + "</font></td>").AppendLine();
                                        sbHtmlPed.Append("  </tr>").AppendLine();
                                        sbHtmlPed.Append("  <tr>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:102px;\" colspan=\"2\"><b><font size=\"1\">NÚMERO DE ORDEN</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:625px;\" colspan=\"10\"><b><font size=\"1\">" + Convert.ToString(rw["NumPedido"]).Trim() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                                        sbHtmlPed.Append("  </tr>").AppendLine();
                                        sbHtmlPed.Append("  <tr>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:27px; text-align:right;\" >").AppendLine();
                                        sbHtmlPed.Append("    <p style=\"text-align:center;\"><b><font size=\"1\">ÍTEM</font></b></p></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:73px;\"><b><font size=\"1\">ARTÍCULO</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:249px;\" colspan=\"3\"><b><font size=\"1\">DESCRIPCIÓN&nbsp;&nbsp; </font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:52px;\"><b><font size=\"1\">REFERENCIA</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:53px;\" align=\"left\"><p align=\"left\"><b><font size=\"1\">TAMA&Ntilde;O</font></b></p></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:34px;\"><b><font size=\"1\">UXC</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:54px;\"><b><font size=\"1\">CANTIDAD</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:36px;\"><b><font size=\"1\">COSTO</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:55px;\"><b><font size=\"1\">DESCTO</font><font size=\"1\"> 1</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("    <td style=\"width:57px;\"><b><font size=\"1\">DESCTO</font><font size=\"1\"> 2</font></b></td>").AppendLine();
                                        sbHtmlPed.Append("  </tr>").AppendLine();
                                        sbHtmlPed.Append("").AppendLine();
                                    }
                                    numItems = numItems + 1;
                                    sbHtmlPed.Append("  <tr>").AppendLine();
                                    sbHtmlPed.Append("    <td style=\"width:27px; text-align:center;\"><font size=\"1\">" + Convert.ToString(rw["Item"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                                    sbHtmlPed.Append("    <td style=\"width:73px; text-align:center;\"><font size=\"1\">" + Convert.ToString(rw["CodArticulo"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                                    sbHtmlPed.Append("    <td width=\"249\" colspan=\"3\"><font size=\"1\">" + Convert.ToString(rw["DesArticulo"]).Trim() + "</font></td>").AppendLine();
                                    sbHtmlPed.Append("    <td style=\"width:52px;\"><font size=\"1\">" + Convert.ToString(rw["Tamano"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                                    sbHtmlPed.Append("    <td style=\"width:53px;\" align=\"left\"><font size=\"1\">" + Convert.ToString(rw["TamanoCaja"]).Trim() + "</font></td>").AppendLine();
                                    sbHtmlPed.Append("    <td style=\"width:34px; text-align:right;\"><font size=\"1\">" + Convert.ToString(rw["UndPorCaja"]).Trim() + "</font>&nbsp;</td>").AppendLine();
                                    sbHtmlPed.Append("    <td style=\"width:54px; text-align:right;\"><font size=\"1\">" + Convert.ToDecimal(rw["CantPedido"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                                    sbHtmlPed.Append("    <td style=\"width:36px; text-align:right;\"><font size=\"1\">" + Convert.ToDecimal(rw["PrecioCosto"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                                    sbHtmlPed.Append("    <td style=\"width:45px; text-align:right;\"><font size=\"1\">" + Convert.ToDecimal(rw["Descuento1"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                                    sbHtmlPed.Append("    <td style=\"width:47px; text-align:right;\"><font size=\"1\">" + Convert.ToDecimal(rw["Descuento2"]).ToString("##0.00").Replace(",", ".") + "</font>&nbsp;</td>").AppendLine();
                                    sbHtmlPed.Append("  </tr>").AppendLine();
                                    sbHtmlPed.Append("").AppendLine();
                                }
                                sbHtmlPed.Append("  <tr>").AppendLine();
                                sbHtmlPed.Append("    <td width=\"709\" colspan=\"12\"><b><font size=\"1\">TOTAL DE íTEMS: " + numItems.ToString() + "</font><b>&nbsp;</b></b></td>").AppendLine();
                                sbHtmlPed.Append("  </tr>").AppendLine();
                                sbHtmlPed.Append("</tbody></table>").AppendLine();
                                sbHtmlPed.Append("<br>").AppendLine();
                                sbHtmlPed.Append("").AppendLine();
                                sbHtmlPed.Append("</body></html>");

                                //Almacena el pdf
                                string pdfFilename = Path.Combine(((string)System.Configuration.ConfigurationManager.AppSettings["RutaMailPedidos"]).Trim(), "Pedidos" + "-" +
                                                     codCorreoProv + "-" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf");
                                //Convert.ToString(item["CodProveedor"]) + "-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

                                //Convierte el html string a pdf
                                Document document = new Document();
                                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                                if (File.Exists(pdfFilename) == false)
                                {
                                    PdfWriter.GetInstance(document, new FileStream(pdfFilename, FileMode.Create));
                                    document.Open();
                                    iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
                                    hw.Parse(new StringReader(sbHtmlPed.ToString()));
                                    document.Close();
                                    zip.AddFile(pdfFilename);
                                }



                            }
                            // FIN - ARMADO DE ARCHIVO PDF

                            //Valida que este seleccionado por lo menos un formato para enviar el mail
                            if (SiPdf || SiXml || SiTxt)
                            {
                                //Para enviar un solo archivo zip
                                //Ruta/nombre.zip --> para guardar el zip y poder enviarlo - Concatenar hora minutos y segundos
                                if (File.Exists(zipFilename) == false)
                                {
                                    zip.Save(zipFilename);
                                }
                                string mensajeEmail = System.IO.File.ReadAllText(rutaEmail);
                                mensajeEmail = mensajeEmail.Replace("_@Proveedor", razCorreoProv);
                                mensajeEmail = mensajeEmail.Replace("_@Ruc", rucCorreoProv);
                                mensajeEmail = mensajeEmail.Replace("_@Desde", FechaIni);
                                mensajeEmail = mensajeEmail.Replace("_@Hasta", FechaFin);

                                //Llena la tabla con los datos de los mails a enviar
                                DataRow dr = dtmails.NewRow();
                                dr["zipFilename"] = zipFilename;
                                dr["mensajeEmail"] = mensajeEmail;
                                dr["correo"] = Destinatarios;
                                dr["proveedor"] = codCorreoProv;
                                dtmails.Rows.Add(dr);
                            }






                            //Registra Auditoria 
                            XmlDocument ParamXML = new XmlDocument();
                            ParamXML.LoadXml("<Root />");
                            ParamXML.DocumentElement.SetAttribute("AsuntoEmail", asuntoEmail);

                            //Recorre la tabla con los datos de los mails a enviar
                            foreach (DataRow row in dtmails.Rows)
                            {
                                //Thread t = new Thread(() => EnviarEmail(Destinatarios, asuntoEmail,
                                //                                        row["mensajeEmail"].ToString(), row["zipFilename"].ToString()));
                                //t.Start();

                                //  EnviarEmail(row["correo"].ToString(), asuntoEmail,row["mensajeEmail"].ToString(), row["zipFilename"].ToString());//);
                                //Xml por cada mail - destinatorio
                                XmlElement elem = ParamXML.CreateElement("CorreosEnviados");
                                elem.SetAttribute("CorreoDestino", row["correo"].ToString());
                                elem.SetAttribute("CodigoProveedor", row["proveedor"].ToString());
                                elem.SetAttribute("Mensaje", row["mensajeEmail"].ToString());
                                elem.SetAttribute("DirZipAdjunto", row["zipFilename"].ToString());
                                ParamXML.DocumentElement.AppendChild(elem);
                            }

                            //Transacción 505 - PEDIDOS.PED_P_ENVIOPEDIDOS_AUDIT
                            ds = objEjecucion.EjecucionGralDs(ParamXML.OuterXml, 505, 1);

                        }
                    }
                    FormResponse.root.Add(TmpList);

                }
                else
                {
                    //Error base de datos
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                //Error controlado
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            return FormResponse;
        }

        private string EnviarEmail(string pCorreoE, string asuntoEmail, string mensajeEmail, string rutaEmail)
        {
            string retornon = "";
            #region RFD0-2022-155 Variables CORREO
            String PI_Variables = string.Empty;
            #endregion RFD0-2022-155 Variables CORREO
            try
            {
                var nomArchivo = rutaEmail.Split('\\');
                //var indice = rutaEmail.Length - 1;
                var indice = nomArchivo.Length - 1;
                FileInfo fInfo = new FileInfo(rutaEmail);
                long numBytes = fInfo.Length;
                FileStream fStream = new FileStream(rutaEmail,FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fStream);
                // convert the file to a byte array
                byte[] data = br.ReadBytes((int)numBytes);
                br.Close();
                clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();

                #region RFD0-2022-155 CORREO

                //retornon = objEnvMail.EnviaCorreoApi("", pCorreoE, "", "", asuntoEmail, mensajeEmail, true, true, true, data, nomArchivo[indice], PI_NombrePlantilla,
                //    PI_Variables);
                #endregion

                //retornon = objEnvMail.EnviarCorreoAdjunto("", pCorreoE, "", "", asuntoEmail, mensajeEmail, true, true, true, data, nomArchivo[indice]);
            }
            catch (Exception ex) { }
            return retornon;
        }

    }
}