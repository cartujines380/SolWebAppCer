using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Models;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Xml;
using System.Security.Claims;
using clibProveedores.Models;
using clibProveedores;
using clibSeguridadCR;
using System.Xml.Linq;
using System.IO;
using Renci.SshNet;

using AngularJSAuthentication.API.WCFEnvioCorreo;
using System.Net.Http.Headers;
using System.Drawing;
using AngularJSAuthentication.API.Handlers;
using System.Text;
namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/exportarReporteStockAlmacenArticulo")]
    public class exportarReporteStockAlmacenArticuloController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("getexportarReporteStockAlmacenaArt")]
        [HttpPost]
        public HttpResponseMessage getexportarReporteStockAlmacenaArt(Gra_reporteEnviar datosExportarStockAr)
        {
            HttpResponseMessage result = null;
            DataSet ds = new DataSet();
            string archivo = "";
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repventaStock> lst_ventaStock = new List<repventaStock>();
            List<repVentaxCentro> lst_retornoVentaCentro = new List<repVentaxCentro>();
            List<repcodigoArticulo> lst_codigoImagen = new List<repcodigoArticulo>();
            repventaStock ventastock;
            double cantstock = 0;
            double cantstocktotal = 0;
            double ventastocktotal = 0;
            int i = 0;
            int j = 0;

            List<pedAlmacenes> retornoAlmacen = GetConsAlmacenes("1", datosExportarStockAr.p_reporteDatos.CodSap);

            foreach (var item in datosExportarStockAr.p_reporteAlmacen)
            {
                foreach (var itemAl in retornoAlmacen)
                {
                    if (item.id == itemAl.pCodAlmacen)
                    {
                        item.descripcion = itemAl.pNomAlmacen;
                    }
                }
            }
           

            try
            {
                xmlParam.LoadXml("<Root />");

                xmlParam.DocumentElement.SetAttribute("FechaDesde", datosExportarStockAr.p_reporteDatos.Fecha1.Split('/')[2] + "-" + datosExportarStockAr.p_reporteDatos.Fecha1.Split('/')[1] + "-" + datosExportarStockAr.p_reporteDatos.Fecha1.Split('/')[0]);
                //xmlParam.DocumentElement.SetAttribute("FechaDesde", DateTime.Now.ToString("yyyy-MM-dd"));
                try
                {
                    var tmp = Convert.ToInt32(datosExportarStockAr.p_reporteDatos.CodSap);
                    xmlParam.DocumentElement.SetAttribute("CodSAP", "0000" + datosExportarStockAr.p_reporteDatos.CodSap);
                }
                catch (Exception)
                {
                    xmlParam.DocumentElement.SetAttribute("CodSAP", datosExportarStockAr.p_reporteDatos.CodSap);
                }


                foreach (var it in datosExportarStockAr.p_reporteMaterial)
                {
                    XmlElement elem = xmlParam.CreateElement("Articulo");
                    elem.SetAttribute("CodArticulo", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                    i++;
                }


                foreach (var it in datosExportarStockAr.p_reporteAlmacen)
                {
                    XmlElement elem = xmlParam.CreateElement("Almacen");
                    elem.SetAttribute("CodAlmacen", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                    j++;
                }

                ProcesoWs.ServBaseProceso ProcesoBase = new ProcesoWs.ServBaseProceso();
                var retunr = ProcesoBase.BuscarDatosStock(xmlParam, i, j);

                foreach (var item in datosExportarStockAr.p_reporteAlmacen)
                {
                    cantstock = 0;
                    foreach (var item1 in retunr)
                    {
                        if (item.id == item1.WERKS)
                        {
                            cantstocktotal = cantstocktotal + +Convert.ToDouble(item1.STOCK);
                        }
                    }

                }


                xmlParam.LoadXml("<Root />");
                try
                {
                    xmlParam.DocumentElement.SetAttribute("FechaDesde", datosExportarStockAr.p_reporteDatos.Fecha1);
                    xmlParam.DocumentElement.SetAttribute("FechaHasta", datosExportarStockAr.p_reporteDatos.Fecha2);
                    xmlParam.DocumentElement.SetAttribute("CodSAP", (datosExportarStockAr.p_reporteDatos.CodSap == null ? "" : datosExportarStockAr.p_reporteDatos.CodSap));
                    foreach (var it in datosExportarStockAr.p_reporteAlmacen)
                    {
                        XmlElement elem = xmlParam.CreateElement("Almacen");
                        elem.SetAttribute("CodAlmacen", it.id);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }

                    foreach (var it in datosExportarStockAr.p_reporteMaterial)
                    {
                        XmlElement elem = xmlParam.CreateElement("Articulo");
                        elem.SetAttribute("CodArticulo", it.id);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                    ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 721, 1);
                    if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            ventastock = new repventaStock();
                            ventastock.CodCentro = Convert.ToString(item["CodCentro"]);
                            ventastock.NomAlmacen = Convert.ToString(item["NomAlmacen"]);
                            ventastock.CantVendida = Convert.ToString(item["CantVendida"]);
                            ventastocktotal = ventastocktotal + Convert.ToDouble(item["CantVendida"]);
                            lst_ventaStock.Add(ventastock);
                        }

                    }
                }
                catch (Exception ex)
                {

                }


                DataTable dt = new DataTable("Detallecab");
                dt.Columns.Add("nomAlmacen", System.Type.GetType("System.String"));
                dt.Columns.Add("stock", System.Type.GetType("System.String"));
                dt.Columns.Add("pstock", System.Type.GetType("System.String"));
                dt.Columns.Add("pventa", System.Type.GetType("System.String"));

                foreach (var item in datosExportarStockAr.p_reporteAlmacen)
                {
                    cantstock = 0;
                    foreach (var item1 in retunr)
                    {
                        if (item.id == item1.WERKS)
                        {
                            cantstock = cantstock + Convert.ToDouble(item1.STOCK);
                        }
                    }
                    if (cantstock > 0)
                    {
                        DataRow drowd = dt.NewRow();
                        drowd["nomAlmacen"] = item.descripcion;
                        drowd["stock"] = Convert.ToString(cantstock);
                        drowd["pstock"] = (Math.Round((cantstock / cantstocktotal * 100), 1)).ToString() + " %";
                        drowd["pventa"] = "";
                        foreach (var st in lst_ventaStock)
                        {
                            if (st.NomAlmacen == item.descripcion)
                            {
                                drowd["pventa"] = st.CantVendida;
                            }
                        }
                        if (drowd["pventa"] == "")
                        {
                            drowd["pventa"] = "0";
                        }
                        dt.Rows.Add(drowd);
                    }
                }




                ReportDataSource rptDataSourcecab;
                ReportDataSource rptDataSourcedet;
                DataTable ct = new DataTable("Cabecera");
                ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                ct.Columns.Add("NomProveedor", System.Type.GetType("System.String"));
                DataRow drowc = ct.NewRow();
                drowc["usuario"] = datosExportarStockAr.p_reporteDatos.p_usuario;
                drowc["NomProveedor"] = datosExportarStockAr.p_reporteDatos.nomreporte;
                ct.Rows.Add(drowc);




                rptDataSourcecab = new ReportDataSource("CabeceraStockAlmacen", ct);
                rptDataSourcedet = new ReportDataSource("DetalleStockAlmacen", dt);
                ReportViewer auxc = new ReportViewer();
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = "";
                string encoding = "";
                string extension = "";
                auxc.ProcessingMode = ProcessingMode.Local;
                auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteStockAlmacenArticulo.rdlc");
                auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                byte[] bytes = null;
                archivo = "Reporte" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);

                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(bytes));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = archivo;


            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public List<pedAlmacenes> GetConsAlmacenes(String tipoLista, string codSap)
        {
            List<pedAlmacenes> FormResponse = new List<pedAlmacenes>();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                var xml = "<Root> <SecNotificacion  TipoLista=\"" + tipoLista + "\"  CodigoSap=\"" + codSap + "\" /> </Root>";
                ds = objEjecucion.EjecucionGralDs(xml, 715, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<pedAlmacenes> TmpList = new List<pedAlmacenes>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedAlmacenes TmpItem = new pedAlmacenes();
                        TmpItem.pCodAlmacen = Convert.ToString(item["CodAlmacen"]);
                        TmpItem.pNomAlmacen = Convert.ToString(item["NomAlmacen"]);
                        TmpList.Add(TmpItem);
                    }
                    FormResponse = TmpList;
                }

            }
            catch (Exception ex)
            {
            }
            return FormResponse;
        }
    }
}
