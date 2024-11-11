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
    [RoutePrefix("api/exportarReporteStockAlmacen")]
    public class exportarReporteStockAlmacenController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("getexportarReporteStockAlmacena")]
        [HttpPost]
        public HttpResponseMessage getexportarReporteStockAlmacena(Gra_reporteEnviar datosExportarStock)
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
            try
            {
                xmlParam.LoadXml("<Root />");

                xmlParam.DocumentElement.SetAttribute("FechaDesde", datosExportarStock.p_reporteDatos.Fecha1.Split('/')[2] + "-" + datosExportarStock.p_reporteDatos.Fecha1.Split('/')[1] + "-" + datosExportarStock.p_reporteDatos.Fecha1.Split('/')[0]);
                try
                {
                    var tmp = Convert.ToInt32(datosExportarStock.p_reporteDatos.CodSap);
                    xmlParam.DocumentElement.SetAttribute("CodSAP", "0000" + datosExportarStock.p_reporteDatos.CodSap);
                }
                catch (Exception)
                {
                    xmlParam.DocumentElement.SetAttribute("CodSAP", datosExportarStock.p_reporteDatos.CodSap);
                }


                foreach (var it in datosExportarStock.p_reporteMaterial)
                {
                    XmlElement elem = xmlParam.CreateElement("Articulo");
                    elem.SetAttribute("CodArticulo", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                    i++;
                }


                foreach (var it in datosExportarStock.p_reporteAlmacen)
                {
                    XmlElement elem = xmlParam.CreateElement("Almacen");
                    elem.SetAttribute("CodAlmacen", it.id);
                    xmlParam.DocumentElement.AppendChild(elem);
                    j++;
                }

                ProcesoWs.ServBaseProceso ProcesoBase = new ProcesoWs.ServBaseProceso();
                var retunr = ProcesoBase.BuscarDatosStock(xmlParam, i, j);

                foreach (var item in datosExportarStock.p_reporteAlmacen)
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
                    xmlParam.DocumentElement.SetAttribute("FechaDesde", datosExportarStock.p_reporteDatos.Fecha1);
                    xmlParam.DocumentElement.SetAttribute("FechaHasta", "");
                    xmlParam.DocumentElement.SetAttribute("CodSAP", (datosExportarStock.p_reporteDatos.CodSap == null ? "" : datosExportarStock.p_reporteDatos.CodSap));
                    foreach (var it in datosExportarStock.p_reporteAlmacen)
                    {
                        XmlElement elem = xmlParam.CreateElement("Almacen");
                        elem.SetAttribute("CodAlmacen", it.id);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }

                    foreach (var it in datosExportarStock.p_reporteMaterial)
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
                dt.Columns.Add("noventa", System.Type.GetType("System.String"));

                foreach (var item in datosExportarStock.p_reporteAlmacen)
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
                        drowd["noventa"] = "";
                        foreach (var st in lst_ventaStock)
                        {
                            if (st.NomAlmacen == item.descripcion)
                            {
                                drowd["noventa"] = st.CantVendida;
                                drowd["pventa"] = (Math.Round((Convert.ToDouble(st.CantVendida) / ventastocktotal * 100), 1)).ToString() + " %";
                            }
                        }
                        if (drowd["pventa"] == "")
                        {
                            drowd["pventa"] = "0";
                        }
                        if (drowd["noventa"] == "")
                        {
                            drowd["noventa"] = "0";
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
                drowc["usuario"] = datosExportarStock.p_reporteDatos.p_usuario;
                drowc["NomProveedor"] = datosExportarStock.p_reporteDatos.nomreporte;
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
                auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteStockAlmacen.rdlc");
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
    }
}
