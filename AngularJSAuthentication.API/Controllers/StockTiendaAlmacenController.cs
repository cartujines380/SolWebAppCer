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
    [RoutePrefix("api/StockTiendaAlmacen")]
    public class StockTiendaAlmacenController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("consultaStockTiendaAlamcen")]
        [HttpPost]
        public formResponsePedidos consultaStockTiendaAlamcen(Gra_reporteEnviar datosStockAlmacen)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            XmlDocument xmlParam = new XmlDocument();
            repCentroStock mod_SolicitudVentaCentro;
            repcodigoArticulo mod_codigoImagen;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            repVentaxCentro mod_SolicitudVenta;
            repcentro mod_centro;
            List<repCentroStock> lst_retornoVentaCentro = new List<repCentroStock>();
            List<repcodigoArticulo> lst_codigoImagen = new List<repcodigoArticulo>();
            List<repVentaxCentro> lst_retornoVentaGeneral = new List<repVentaxCentro>();
            List<griArticulos> lst_mod_Articulo = new List<griArticulos>();
            List<griArticulosBase> lst_mod_BaseArticulo = new List<griArticulosBase>();
            List<repcentro> lst_mod_centro = new List<repcentro>();
            double cantstock = 0;
            double cantstocktotal = 0;
            double ventastocktotal = 0;
            int i = 0;
            int j = 0;

            List<pedAlmacenes> retornoAlmacen = GetConsAlmacenes("1", datosStockAlmacen.p_reporteDatos.CodSap);

            foreach (var item in datosStockAlmacen.p_reporteAlmacen)
            {
                foreach (var itemAl in retornoAlmacen)
                {
                    if (item.id==itemAl.pCodAlmacen)
                    {
                        item.descripcion = itemAl.pNomAlmacen;
                    }
                }
            }
           
            xmlParam.LoadXml("<Root />");
            xmlParam.DocumentElement.SetAttribute("FechaDesde", datosStockAlmacen.p_reporteDatos.Fecha1.Split('/')[2] + "-" + datosStockAlmacen.p_reporteDatos.Fecha1.Split('/')[1] + "-" + datosStockAlmacen.p_reporteDatos.Fecha1.Split('/')[0]);
            //xmlParam.DocumentElement.SetAttribute("FechaDesde", DateTime.Now.ToString("yyyy-MM-dd"));
            try
            {
                var tmp = Convert.ToInt32(datosStockAlmacen.p_reporteDatos.CodSap);
                xmlParam.DocumentElement.SetAttribute("CodSAP", "0000" + datosStockAlmacen.p_reporteDatos.CodSap);
            }
            catch (Exception)
            {
                xmlParam.DocumentElement.SetAttribute("CodSAP", datosStockAlmacen.p_reporteDatos.CodSap);
            }


            foreach (var it in datosStockAlmacen.p_reporteMaterial)
            {
                XmlElement elem = xmlParam.CreateElement("Articulo");
                elem.SetAttribute("CodArticulo", it.id);
                xmlParam.DocumentElement.AppendChild(elem);
                i++;
            }


            foreach (var it in datosStockAlmacen.p_reporteAlmacen)
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

                    mod_codigoImagen = new repcodigoArticulo();
                    mod_codigoImagen.CodMaterial = "1";
                    mod_codigoImagen.DesMaterial = "VENTA MATERIAL";
                    lst_codigoImagen.Add(mod_codigoImagen);

                    mod_codigoImagen = new repcodigoArticulo();
                    mod_codigoImagen.CodMaterial = "2";
                    mod_codigoImagen.DesMaterial = "STOCK ARTICULO";
                    lst_codigoImagen.Add(mod_codigoImagen);

                    FormResponse.success = true;
               

                    foreach (var item in datosStockAlmacen.p_reporteAlmacen)
                    {
                        mod_centro = new repcentro();
                        mod_centro.Centro = Convert.ToString(item.id);
                        mod_centro.Almacen = Convert.ToString(item.descripcion);
                        lst_mod_centro.Add(mod_centro);
                    }



                    foreach (var item in datosStockAlmacen.p_reporteAlmacen)
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
                            mod_SolicitudVenta = new repVentaxCentro();
                            mod_SolicitudVenta.CodCentro = item.id;
                            mod_SolicitudVenta.NomAlmacen = item.descripcion;
                            mod_SolicitudVenta.CodMaterial = "2";
                            mod_SolicitudVenta.DesMaterial = "STOCK ARTICULO";
                            mod_SolicitudVenta.CantVendida = Convert.ToString(cantstock);
                            mod_SolicitudVenta.UnidadVenta = "";

                            lst_retornoVentaGeneral.Add(mod_SolicitudVenta);
                        }
                    }
                    foreach (var item in datosStockAlmacen.p_reporteAlmacen)
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
                            lst_retornoVentaCentro.Add(mod_SolicitudVentaCentro);
                        }
                    }

                    xmlParam.LoadXml("<Root />");
                    try
                    {
                        xmlParam.DocumentElement.SetAttribute("FechaDesde", datosStockAlmacen.p_reporteDatos.Fecha1);
                        xmlParam.DocumentElement.SetAttribute("FechaHasta", datosStockAlmacen.p_reporteDatos.Fecha2);
                        xmlParam.DocumentElement.SetAttribute("CodSAP", (datosStockAlmacen.p_reporteDatos.CodSap == null ? "" : datosStockAlmacen.p_reporteDatos.CodSap));
                        foreach (var it in datosStockAlmacen.p_reporteAlmacen)
                        {
                            XmlElement elem = xmlParam.CreateElement("Almacen");
                            elem.SetAttribute("CodAlmacen", it.id);
                            xmlParam.DocumentElement.AppendChild(elem);
                        }

                        foreach (var it in datosStockAlmacen.p_reporteMaterial)
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

                                mod_SolicitudVenta = new repVentaxCentro();
                                mod_SolicitudVenta.CodCentro = Convert.ToString(item["CodCentro"]);
                                mod_SolicitudVenta.NomAlmacen = Convert.ToString(item["NomAlmacen"]);
                                mod_SolicitudVenta.CodMaterial = "1";
                                mod_SolicitudVenta.DesMaterial = "VENTA MATERIAL";
                                mod_SolicitudVenta.CantVendida = Convert.ToString(item["CantVendida"]);
                                mod_SolicitudVenta.UnidadVenta = "";

                                lst_retornoVentaGeneral.Add(mod_SolicitudVenta);


                            }

                            for (j = 0; j < lst_retornoVentaCentro.Count; j++)
                            {
                                foreach (DataRow item in ds.Tables[0].Rows)
                                {
                                    if (lst_retornoVentaCentro[j].nomAlmacen == item["NomAlmacen"].ToString())
                                    {
                                        lst_retornoVentaCentro[j].pventa = item["CantVendida"].ToString();
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
                    FormResponse.root.Add(lst_mod_centro);
                    FormResponse.root.Add(lst_retornoVentaGeneral);
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
