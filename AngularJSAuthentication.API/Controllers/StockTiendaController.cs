using AngularJSAuthentication.API.Handlers;
using clibProveedores;
using clibProveedores.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/StockTienda")]
    public class StockTiendaController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("consultaStockTienda")]
        [HttpPost]
        public formResponsePedidos consultaStockTienda(Gra_reporteEnviar datosStock)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            XmlDocument xmlParam = new XmlDocument();
            repCentroStock mod_SolicitudVentaCentro;
            repcodigoArticulo mod_codigoImagen;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            griArticulos mod_Articulo;
            griArticulosBase modBaseArtciculo;
            List<repCentroStock> lst_retornoVentaCentro = new List<repCentroStock>();
            List<repcodigoArticulo> lst_codigoImagen = new List<repcodigoArticulo>();
            List<griArticulos> lst_mod_Articulo = new List<griArticulos>();
            List<griArticulosBase> lst_mod_BaseArticulo = new List<griArticulosBase>();
            double cantstock = 0;
            double cantstocktotal = 0;
            double ventastocktotal = 0;
            string articulo = "";
            int i = 0;
            int j = 0;
            xmlParam.LoadXml("<Root />");

            xmlParam.DocumentElement.SetAttribute("FechaDesde", datosStock.p_reporteDatos.Fecha1.Split('/')[2] + "-" + datosStock.p_reporteDatos.Fecha1.Split('/')[1] + "-" + datosStock.p_reporteDatos.Fecha1.Split('/')[0]);
            try
            {
                var tmp = Convert.ToInt32(datosStock.p_reporteDatos.CodSap);
                xmlParam.DocumentElement.SetAttribute("CodSAP", "0000" + datosStock.p_reporteDatos.CodSap);
            }
            catch (Exception)
            {
                xmlParam.DocumentElement.SetAttribute("CodSAP", datosStock.p_reporteDatos.CodSap);
            }


            foreach (var it in datosStock.p_reporteMaterial)
            {
                XmlElement elem = xmlParam.CreateElement("Articulo");
                elem.SetAttribute("CodArticulo", it.id);
                xmlParam.DocumentElement.AppendChild(elem);
                i++;
            }


            foreach (var it in datosStock.p_reporteAlmacen)
            {
                XmlElement elem = xmlParam.CreateElement("Almacen");
                elem.SetAttribute("CodAlmacen", it.id);
                xmlParam.DocumentElement.AppendChild(elem);
                j++;
            }
            try
            {
                ProcesoWs.ServBaseProceso ProcesoBase = new ProcesoWs.ServBaseProceso();
                var retunr = ProcesoBase.BuscarDatosStock(xmlParam, i, j);
                if (retunr.Count() > 1)
                {
                    FormResponse.success = true;
                    if (datosStock.p_reporteMaterial.Count() > 1)
                    {
                        mod_codigoImagen = new repcodigoArticulo();
                        mod_codigoImagen.CodMaterial = "1";
                        mod_codigoImagen.DesMaterial = "PRODUCTOS SELECIONADOS";
                        lst_codigoImagen.Add(mod_codigoImagen);
                        articulo = "1";
                    }
                    else
                    {
                        mod_codigoImagen = new repcodigoArticulo();
                        mod_codigoImagen.CodMaterial = datosStock.p_reporteMaterial[0].id;
                        mod_codigoImagen.DesMaterial = datosStock.p_reporteMaterial[0].descripcion;
                        lst_codigoImagen.Add(mod_codigoImagen);
                        articulo = datosStock.p_reporteMaterial[0].id;
                    }

                    foreach (var item in datosStock.p_reporteMaterial)
                    {
                        mod_Articulo = new griArticulos();
                        mod_Articulo.codArticulo = Convert.ToString(item.id);
                        mod_Articulo.desArticulo = Convert.ToString(item.descripcion);
                        lst_mod_Articulo.Add(mod_Articulo);
                    }

                    foreach (var item in datosStock.p_reporteAlmacen)
                    {
                        cantstock = 0;
                        foreach (var item1 in retunr)
                        {
                            if (item.id == item1.WERKS)
                            {
                                cantstock = cantstock + Convert.ToDouble(item1.STOCK);
                                cantstocktotal = cantstocktotal + +Convert.ToDouble(item1.STOCK);
                            }
                        }
                        if (cantstock > 0)
                        {
                            modBaseArtciculo = new griArticulosBase();
                            modBaseArtciculo.codAlmacen = item.id;
                            modBaseArtciculo.nomAlmacen = item.descripcion;
                            modBaseArtciculo.cantidad = Convert.ToString(cantstock);
                            modBaseArtciculo.articulo = articulo;
                            lst_mod_BaseArticulo.Add(modBaseArtciculo);
                        }
                    }
                    foreach (var item in datosStock.p_reporteAlmacen)
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
                            mod_SolicitudVentaCentro = new repCentroStock();
                            mod_SolicitudVentaCentro.nomAlmacen = item.descripcion;
                            mod_SolicitudVentaCentro.stock = Convert.ToString(cantstock);
                            mod_SolicitudVentaCentro.pstock = (Math.Round((cantstock / cantstocktotal * 100), 1)).ToString() + " %";
                            mod_SolicitudVentaCentro.pventa = "0";
                            mod_SolicitudVentaCentro.noventa = "0";
                            lst_retornoVentaCentro.Add(mod_SolicitudVentaCentro);
                        }
                    }

                    xmlParam.LoadXml("<Root />");
                    try
                    {
                        xmlParam.DocumentElement.SetAttribute("FechaDesde", datosStock.p_reporteDatos.Fecha1);
                        xmlParam.DocumentElement.SetAttribute("FechaHasta", "");
                        xmlParam.DocumentElement.SetAttribute("CodSAP", (datosStock.p_reporteDatos.CodSap == null ? "" : datosStock.p_reporteDatos.CodSap));
                        foreach (var it in datosStock.p_reporteAlmacen)
                        {
                            XmlElement elem = xmlParam.CreateElement("Almacen");
                            elem.SetAttribute("CodAlmacen", it.id);
                            xmlParam.DocumentElement.AppendChild(elem);
                        }

                        foreach (var it in datosStock.p_reporteMaterial)
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
                                ventastocktotal = ventastocktotal + Convert.ToDouble(item["CantVendida"]);

                            }

                            for (j = 0; j < lst_retornoVentaCentro.Count; j++)
                            {
                                foreach (DataRow item in ds.Tables[0].Rows)
                                {
                                    if (lst_retornoVentaCentro[j].nomAlmacen == item["NomAlmacen"].ToString())
                                    {
                                        lst_retornoVentaCentro[j].noventa = Convert.ToString(item["CantVendida"]);
                                        lst_retornoVentaCentro[j].pventa = (Math.Round((Convert.ToDouble(item["CantVendida"]) / ventastocktotal * 100), 1)).ToString() + " %";
                                    }
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        FormResponse.success = false;
                        FormResponse.msgError = ex.Message.ToString();
                    }
                    FormResponse.root.Add(lst_retornoVentaCentro);
                    FormResponse.root.Add(lst_codigoImagen);
                    FormResponse.root.Add(lst_mod_Articulo);
                    FormResponse.root.Add(lst_mod_BaseArticulo);
                }
                else
                {
                    FormResponse.success = false;
                }

            }
            catch (Exception ex)
            {


            }



            return FormResponse;
        }
    }
}
