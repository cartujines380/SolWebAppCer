using clibProveedores;
using clibProveedores.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Indicadores")]
    public class IndicadoresController : ApiController
    {
        Logger.Logger log = new Logger.Logger();

        [ActionName("resumenVentas")]
        [HttpGet]
        public ResumenVenta getResumenVentas(
            string fecIni, string fecFin, string idcanal, string marca, string proveedor,
            string proveedorUsr, string idSegmento, string codSubCanal)
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                            new XElement("Indicadores",
                                     new XAttribute("proveedor", (proveedor == null || proveedor == "-1" ? "" : proveedor)),
                                     new XAttribute("proveedorUsr", (proveedorUsr == null || proveedorUsr == "-1" ? "" : proveedorUsr)),
                                     new XAttribute("fec_ini", fecIni),
                                     new XAttribute("fec_fin", fecFin),
                                     new XAttribute("codCanal", (idcanal == null || idcanal == "-1" ? "" : idcanal)),
                                     new XAttribute("codSubCanal", (codSubCanal == null || codSubCanal == "-1" ? "" : codSubCanal)),
                                     new XAttribute("idSegmento", (idSegmento == null || idSegmento == "-1" ? "" : idSegmento)),
                                     new XAttribute("marca", (marca == null || marca == "-1" ? "" : marca)))));
            log.FilePath = p_Log;
            log.WriteMensaje("getResumenVentas: \n" + wresulFactList.ToString());

            ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 4405, 1);
            ResumenVenta FormResponse = new ResumenVenta();
            FormResponse.lSuccess = true;

            try
            {

                List<Ventas> Retorno = new List<Ventas>();
                Retorno = (from reg in ds.Tables[0].AsEnumerable()
                           select new Ventas
                           {
                               Periodo = reg.Field<string>("Periodo"),
                               SumaVenta = reg["SumaVenta"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["SumaVenta"]), 2) : 0,//Math.Round((reg.Field<decimal?>("SumaVenta") ?? 0), 2),
                               Crecimiento = reg["Crecimiento"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["Crecimiento"]), 2) : 0,// Math.Round(reg.Field<decimal>("Crecimiento"), 2),
                               Porcentaje = reg["Porcentaje"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["Porcentaje"]), 2) : 0,
                               Factura = reg["Factura"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["Factura"]), 2) : 0,// Math.Round((reg.Field<decimal?>("Factura") ?? 0), 2),
                               Presupuesto = reg["Presupuesto"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["Presupuesto"]), 2) : 0// Math.Round((reg.Field<decimal?>("Presupuesto") ?? 0), 2),
                           }).ToList<Ventas>();


                foreach (var item in Retorno)
                {
                    item.SumaVenta = TransformUnid(item.SumaVenta, out string unida);
                    item.Unida = unida;
                    item.Crecimiento = TransformUnid(item.Crecimiento, out string unidb);
                    item.Factura = TransformUnid(item.Factura, out string unidd);
                    item.Unidb = unidb;
                    item.Presupuesto = TransformUnid(item.Presupuesto, out string unidc);
                    item.Unidc = unidc;
                }

                //var m = TransformUnid(Math.Abs( (decimal)(Retorno[1].Presupuesto - Retorno[0].Presupuesto) ), out string unidadp);
                //Retorno[0].Unidc = unidadp;

                FormResponse.root.Add(Retorno);
            }
            catch (Exception e)
            {
                log.FilePath = p_Log;
                log.WriteMensaje("Error getResumenVentas: " + e.Message.ToString());

                FormResponse.lSuccess = false;
                FormResponse.cMsgError = e.Message.ToString();
            }
            return FormResponse;
        }

        [Route("resumenInventario")]
        [HttpGet]
        public ResumenVenta getResumenInventario(
            string fecIni, string fecFin, string idcanal, string marca, string proveedor,
            string proveedorUsr, string idSegmento, string codSubCanal
            )
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                            new XElement("Indicadores",
                                     new XAttribute("proveedor", (proveedor == null || proveedor == "-1" ? "" : proveedor)),
                                     new XAttribute("proveedorUsr", (proveedorUsr == null || proveedorUsr == "-1" ? "" : proveedorUsr)),
                                     new XAttribute("fec_ini", fecIni),
                                     new XAttribute("fec_fin", fecFin),
                                     new XAttribute("codCanal", (idcanal == null || idcanal == "-1" ? "" : idcanal)),
                                     new XAttribute("codSubCanal", (codSubCanal == null || codSubCanal == "-1" ? "" : codSubCanal)),
                                     new XAttribute("idSegmento", (idSegmento == null || idSegmento == "-1" ? "" : idSegmento)),
                                     new XAttribute("marca", (marca == null || marca == "-1" ? "" : marca)))));
            log.FilePath = p_Log;
            log.WriteMensaje("getResumenInventario: \n" + wresulFactList.ToString());

            ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 4406, 1);
            ResumenVenta FormResponse = new ResumenVenta();
            FormResponse.lSuccess = true;

            try
            {

                List<Inventario> Retorno = new List<Inventario>();
                Retorno = (from reg in ds.Tables[0].AsEnumerable()
                           select new Inventario
                           {
                               StockCosto = reg["StockCosto"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["StockCosto"]), 2) : 0,
                               CostoNeto = reg["CostoNeto"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["CostoNeto"]), 2) : 0,
                               StockUnidades = reg["StockUnidades"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["StockUnidades"]), 2) : 0,
                               DiasInv = reg["DiasInv"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["DiasInv"]), 2) : 0

                           }).ToList<Inventario>();


                foreach (var item in Retorno)
                {

                    item.StockCosto = TransformUnid(item.StockCosto, out string unida);
                    item.Unida = unida;
                    item.CostoNeto = TransformUnid(item.CostoNeto, out string unidb);
                    item.Unidb = unidb;
                    item.StockUnidades = TransformUnid(item.StockUnidades, out string unidc);
                    item.Unidc = unidc;
                    item.DiasInv = TransformUnid(item.DiasInv, out string unidd);
                    item.Unidd = unidd;
                }


                FormResponse.root.Add(Retorno);
            }
            catch (Exception e)
            {
                log.FilePath = p_Log;
                log.WriteMensaje("Error getResumenInventario: " + e.Message.ToString());

                FormResponse.lSuccess = false;
                FormResponse.cMsgError = e.Message.ToString();
            }
            return FormResponse;
        }

        [Route("resumenCoberturas")]
        [HttpGet]
        public ResumenVenta getResumenCoberturas(
            string fecIni, string fecFin, string idcanal, string marca, string proveedor,
            string proveedorUsr, string idSegmento, string codSubCanal
            )
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                            new XElement("Indicadores",
                                     new XAttribute("proveedor", (proveedor == null || proveedor == "-1" ? "" : proveedor)),
                                     new XAttribute("proveedorUsr", (proveedorUsr == null || proveedorUsr == "-1" ? "" : proveedorUsr)),
                                     new XAttribute("fec_ini", fecIni),
                                     new XAttribute("fec_fin", fecFin),
                                     new XAttribute("codCanal", (idcanal == null || idcanal == "-1" ? "" : idcanal)),
                                     new XAttribute("codSubCanal", (codSubCanal == null || codSubCanal == "-1" ? "" : codSubCanal)),
                                     new XAttribute("idSegmento", (idSegmento == null || idSegmento == "-1" ? "" : idSegmento)),
                                     new XAttribute("marca", (marca == null || marca == "-1" ? "" : marca)))));
            log.FilePath = p_Log;
            log.WriteMensaje("getResumenCoberturas: \n" + wresulFactList.ToString());

            ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 4421, 1);
            ResumenVenta FormResponse = new ResumenVenta();
            FormResponse.lSuccess = true;


            try
            {
                List<Cobertura> Retorno = new List<Cobertura>();
                Retorno = (from reg in ds.Tables[0].AsEnumerable()
                           select new Cobertura
                           {
                               Periodo = $"{fecIni}-{fecFin}",
                               TotalGlobal = reg["TotalGlobal"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["TotalGlobal"]), 2) : 0,
                               AvgCobInventario = reg["AvgCobInventario"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["AvgCobInventario"]), 2) : 0,
                               AvgCobRotacion = reg["AvgCobRotacion"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["AvgCobRotacion"]), 2) : 0,
                               CoberturaInvSKU = reg["CoberturaInvSKU"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["CoberturaInvSKU"]), 2) : 0,
                               CoberturaRotacion = reg["CoberturaRotacion"] != DBNull.Value ? Math.Round(Convert.ToDecimal(reg["CoberturaRotacion"]), 2) : 0

                           }).ToList<Cobertura>();
                FormResponse.root.Add(Retorno);
            }
            catch (Exception e)
            {
                log.FilePath = p_Log;
                log.WriteMensaje("Error getResumenCoberturas: " + e.Message.ToString());

                FormResponse.lSuccess = false;
                FormResponse.cMsgError = e.Message.ToString();
            }
            return FormResponse;

        }



        private decimal TransformUnid(decimal? monto, out string unid)
        {
            unid = "";
            if (monto >= 1000 && monto < 10000)
            {
                monto /= 1000; //k
                unid = "K";
            }
            else if (monto >= 10000 && monto < 100000)
            {
                monto /= 10000;//k
                unid = "K";
            }
            else if (monto >= 100000 && monto < 1000000)
            {
                monto /= 100000;//k
                unid = "K";
            }
            else if (monto >= 1000000 && monto < 10000000)
            {
                monto /= 1000000;//M
                unid = "M";
            }
            else if (monto >= 10000000 && monto < 100000000)
            {
                monto /= 10000000;//M
                unid = "M";
            }
            else if (monto >= 100000000 && monto < 1000000000)
            {
                monto /= 100000000;//M
                unid = "M";
            }
            else if (monto >= 1000000000 && monto < 10000000000)
            {
                monto /= 1000000000;//B
                unid = "B";
            }

            return monto.Value;
        }

        [ActionName("obtenerMarcas")]
        [HttpGet]
        public ModelosSelect getMarcas(string k, string p)
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();

            ModelosSelect FormResponse = new ModelosSelect();
            FormResponse.lSuccess = true;

            XmlDocument xmlParam = new XmlDocument();
            ClsGeneral objEjecucion = new ClsGeneral();
            List<SelectModel> Retorno = new List<SelectModel>();
            DataSet ds = new DataSet();

            try
            {
                FormResponse.lSuccess = true;
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("Trx", k);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 4410, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    DataTable entidad = ds.Tables[0];

                    if (entidad.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in entidad.Rows)
                        {
                            SelectModel Item = new SelectModel();
                            Item.Id = Convert.ToInt32(dr1["Codigo"]);
                            Item.Desc = Convert.ToString(dr1["Descripcion"]);
                            FormResponse.root.Add(Item);
                        }

                    }
                }
                else
                {
                    FormResponse.lSuccess = false;
                    FormResponse.cCodError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.cMsgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }


            }
            catch (Exception e)
            {

                log.FilePath = p_Log;
                log.WriteMensaje("Error obtenerMarcas: " + e.Message.ToString());

                FormResponse.lSuccess = false;
                FormResponse.cMsgError = e.Message.ToString();
            }

            return FormResponse;
        }

        [ActionName("obtenerProveedores")]
        [HttpGet]
        public ModelosSelect getProveedores(string id)
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();

            ModelosSelect FormResponse = new ModelosSelect();
            FormResponse.lSuccess = true;

            XmlDocument xmlParam = new XmlDocument();
            ClsGeneral objEjecucion = new ClsGeneral();
            List<SelectModel> Retorno = new List<SelectModel>();
            DataSet ds = new DataSet();


            try
            {
                FormResponse.lSuccess = true;
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("Trx", "PRO");
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 4410, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    DataTable entidad = ds.Tables[0];

                    if (entidad.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in entidad.Rows)
                        {
                            SelectModel Item = new SelectModel();
                            Item.Codigo = Convert.ToString(dr1["Codigo"]);
                            Item.Desc = Convert.ToString(dr1["Descripcion"]);
                            FormResponse.root.Add(Item);
                        }

                    }
                }
                else
                {
                    FormResponse.lSuccess = false;
                    FormResponse.cCodError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.cMsgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }

            }
            catch (Exception e)
            {

                log.FilePath = p_Log;
                log.WriteMensaje("Error obtenerProveedores: " + e.Message.ToString());

                FormResponse.lSuccess = false;
                FormResponse.cMsgError = e.Message.ToString();
            }

            return FormResponse;
        }
    }
}
