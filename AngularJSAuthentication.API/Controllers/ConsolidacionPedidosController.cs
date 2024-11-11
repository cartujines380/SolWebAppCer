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
using System.Security.Claims;
using clibProveedores.Models;
using clibProveedores;
using clibSeguridadCR;
using System.Xml.Linq;
using System.IO;
using Renci.SshNet;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/ConsolidacionPedidos")]
    public class ConsolidacionPedidosController : ApiController
    {
        //Consulta Carga Incial de Busqueda

        [ActionName("bajararchivo")]
        [HttpGet]
        public string bajararchivo(string identificacion, string archivo)
        {
            bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion));
            if (!folderExists)
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion));
            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion), archivo);
            //httpPostedFile.SaveAs(fileSavePath);

            using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
            {
                sftpClient.Connect();
                if (!sftpClient.Exists(AppConfig.SftpPath + "Transporte/" + identificacion))
                {
                    sftpClient.CreateDirectory(AppConfig.SftpPath + "Transporte/" + identificacion);
                }

                Stream fin = File.OpenWrite(fileSavePath);
                sftpClient.DownloadFile(AppConfig.SftpPath + "Transporte/" + identificacion + "/" + archivo, fin, null);

                fin.Close();
                sftpClient.Disconnect();
            }

            String result = null;
            var localFilePath = fileSavePath;

            if (File.Exists(localFilePath))
            {
                //result = "http://localhost:26264/DownloadedDocuments/" + identificacion + "/" + archivo;
                result = "https://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/webapi/DownloadedDocuments/" + identificacion + "/" + archivo;
            }
            //else
            //{// serve the file to the client
            //    result = Request.CreateResponse(HttpStatusCode.OK);
            //    result.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
            //    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            //    result.Content.Headers.ContentDisposition.FileName = "SampleImg";
            //}

            return result;
        }


        [ActionName("ChoferVehiculo")]
        [HttpGet]
        public FormResponseTransporte ChoferVehiculo(string tipoid, string codproveedor)
        {
            List<TablaCatalogo> lst_retornoSolBandeja = new List<TablaCatalogo>();
            TablaCatalogo mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipoid);
                xmlParam.DocumentElement.SetAttribute("CodProveedor", codproveedor);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 306, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudBandeja = new TablaCatalogo();
                        mod_SolicitudBandeja.Codigo = Convert.ToString(item["codigo"]);
                        mod_SolicitudBandeja.Detalle = Convert.ToString(item["detalle"]);
                        mod_SolicitudBandeja.DescAlterno = Convert.ToString(item["DescAlterno"]);
                        lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSolBandeja);
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }

        [ActionName("BuscarConsolidacionAdmin")]
        [HttpGet]
        public FormResponseTransporte BuscarConsolidacionAdmin(string numeroAdmin, string codProveedorConsoAdmin, string estadoAdmin, string fechadesdeAdmin, string fechahastaAdmin)
        {
            List<Tra_BandejaConsolidacion> lst_retornoSolBandeja = new List<Tra_BandejaConsolidacion>();
            Tra_BandejaConsolidacion mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("numero", numeroAdmin);
                xmlParam.DocumentElement.SetAttribute("codProveedor", codProveedorConsoAdmin);
                xmlParam.DocumentElement.SetAttribute("fechadesde", fechadesdeAdmin);
                xmlParam.DocumentElement.SetAttribute("fechahasta", fechahastaAdmin);
                if (estadoAdmin != null)
                {
                    List<string> TagIds = estadoAdmin.Split(',').Select(Convert.ToString).ToList();
                    for (int i = 0; i < TagIds.Count; i++)
                    {
                        XmlElement elem = xmlParam.CreateElement("Est");
                        elem.SetAttribute("id", TagIds[i].ToString());
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 320, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.msgError = "NO";
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {

                        mod_SolicitudBandeja = new Tra_BandejaConsolidacion();
                        mod_SolicitudBandeja.proveedor = Convert.ToString(item["NomComercial"]);
                        mod_SolicitudBandeja.idconsolidacion = Convert.ToString(item["idconsolidacion"]);
                        mod_SolicitudBandeja.idcita = Convert.ToString(item["idcita"]);
                        mod_SolicitudBandeja.emision = Convert.ToString(item["emision"]);
                        mod_SolicitudBandeja.almacensolicitante = Convert.ToString(item["almacensolicitante"]);
                        mod_SolicitudBandeja.almacendestino = Convert.ToString(item["almacendestino"]);
                        mod_SolicitudBandeja.estadoconsolidacion = Convert.ToString(item["estadoconsolidacion"]);
                        mod_SolicitudBandeja.caducidadconsolidacion = Convert.ToString(item["caducidadconsolidacion"]);
                        var ruta = HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + "0" + Convert.ToString(item["idconsolidacion"]) + ".png");
                        GenCode128.Code128Rendering.MakeBarcodeImage("0" + Convert.ToString(item["idconsolidacion"]), 1, true).Save(ruta, System.Drawing.Imaging.ImageFormat.Png);
                        mod_SolicitudBandeja.imagenurl = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/DownloadedDocuments/" + "0" + Convert.ToString(item["idconsolidacion"]) + ".png";
                        mod_SolicitudBandeja.imagenurl2 = ruta;
                        mod_SolicitudBandeja.codproveedor = Convert.ToString(item["CodProveedor"]);
                        lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                        FormResponse.msgError = "SI";
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSolBandeja);
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }

        [ActionName("BuscarConsolidacion")]
        [HttpGet]
        public FormResponseTransporte BuscarConsolidacion(string numero, string codProveedorConso, string estado, string fechadesde, string fechahasta)
        {
            List<Tra_BandejaConsolidacion> lst_retornoSolBandeja = new List<Tra_BandejaConsolidacion>();
            Tra_BandejaConsolidacion mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("numero", numero);
                xmlParam.DocumentElement.SetAttribute("codProveedor", codProveedorConso);
                xmlParam.DocumentElement.SetAttribute("fechadesde", fechadesde);
                xmlParam.DocumentElement.SetAttribute("fechahasta", fechahasta);
                if (estado != null)
                {
                    List<string> TagIds = estado.Split(',').Select(Convert.ToString).ToList();
                    for (int i = 0; i < TagIds.Count; i++)
                    {
                        XmlElement elem = xmlParam.CreateElement("Est");
                        elem.SetAttribute("id", TagIds[i].ToString());
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 308, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.msgError = "NO";
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {

                        mod_SolicitudBandeja = new Tra_BandejaConsolidacion();
                        mod_SolicitudBandeja.idconsolidacion = Convert.ToString(item["idconsolidacion"]);
                        mod_SolicitudBandeja.idcita = Convert.ToString(item["idcita"]);
                        mod_SolicitudBandeja.emision = Convert.ToString(item["emision"]);
                        mod_SolicitudBandeja.almacensolicitante = Convert.ToString(item["almacensolicitante"]);
                        mod_SolicitudBandeja.almacendestino = Convert.ToString(item["almacendestino"]);
                        mod_SolicitudBandeja.estadoconsolidacion = Convert.ToString(item["estadoconsolidacion"]);
                        mod_SolicitudBandeja.caducidadconsolidacion = Convert.ToString(item["caducidadconsolidacion"]);
                        var ruta= HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + "0" + Convert.ToString(item["idconsolidacion"]) + ".png");
                        GenCode128.Code128Rendering.MakeBarcodeImage("0" + Convert.ToString(item["idconsolidacion"]) , 1, true).Save(ruta, System.Drawing.Imaging.ImageFormat.Png);
                        mod_SolicitudBandeja.imagenurl = "https://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/webapi/DownloadedDocuments/" + "0" + Convert.ToString(item["idconsolidacion"]) + ".png";
                        mod_SolicitudBandeja.imagenurl2 = ruta;
                        lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                        FormResponse.msgError = "SI";
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSolBandeja);
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }

        [ActionName("BuscarPedidosBodega")]
        [HttpGet]
        public FormResponseTransporte BuscarPedidosBodega(string tipoPe, string codproveedorPe,string Bodega)
        {
            List<Tra_ConsultaPedidos> lst_retornoSolBandeja = new List<Tra_ConsultaPedidos>();
            Tra_ConsultaPedidos mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipoPe);
                xmlParam.DocumentElement.SetAttribute("CodProveedor", codproveedorPe);
                xmlParam.DocumentElement.SetAttribute("CodBodega", Bodega);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 306, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudBandeja = new Tra_ConsultaPedidos();
                        mod_SolicitudBandeja.chkpedido = false;
                        mod_SolicitudBandeja.NumPedido = Convert.ToString(item["NumPedido"]);
                        mod_SolicitudBandeja.FechaPedido = Convert.ToString(item["FechaPedido"]);
                        mod_SolicitudBandeja.RineRide = Convert.ToString(item["RineRide"]);
                        mod_SolicitudBandeja.AlmacenSolicitante = Convert.ToString(item["AlmacenSolicitante"]);
                        mod_SolicitudBandeja.AlmacenDestino = Convert.ToString(item["AlmacenDestino"]);
                        mod_SolicitudBandeja.FechaCaducidad = Convert.ToString(item["FechaCaducidad"]);
                        mod_SolicitudBandeja.ValorPedido = Convert.ToString(item["ValorPedido"]);
                        mod_SolicitudBandeja.ValorFactura = Convert.ToString(item["ValorFactura"]);
                        mod_SolicitudBandeja.NumeroBulto = Convert.ToString(item["NumeroBulto"]);
                        mod_SolicitudBandeja.NumeroPalet = Convert.ToString(item["NumeroPalet"]);
                        mod_SolicitudBandeja.IDPalet = Convert.ToString(item["IDPalet"]);
                        mod_SolicitudBandeja.TipoPedido = Convert.ToString(item["TipoPedido"]);
                        mod_SolicitudBandeja.IsCross = Convert.ToBoolean(item["IsCross"]);
                        mod_SolicitudBandeja.Estado = Convert.ToString(item["Estado"]);
                        mod_SolicitudBandeja.EstadoDesc = Convert.ToString(item["EstadoDesc"]);
                        lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSolBandeja);
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }

        [ActionName("BuscarPedidos")]
        [HttpGet]
        public FormResponseTransporte BuscarPedidos(string tipoPe, string codproveedorPe)
        {
            List<Tra_ConsultaPedidos> lst_retornoSolBandeja = new List<Tra_ConsultaPedidos>();
            Tra_ConsultaPedidos mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipoPe);
                xmlParam.DocumentElement.SetAttribute("CodProveedor", codproveedorPe);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 306, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudBandeja = new Tra_ConsultaPedidos();
                        mod_SolicitudBandeja.chkpedido = false;
                        mod_SolicitudBandeja.NumPedido = Convert.ToString(item["NumPedido"]);
                        mod_SolicitudBandeja.FechaPedido = Convert.ToString(item["FechaPedido"]);
                        mod_SolicitudBandeja.RineRide = Convert.ToString(item["RineRide"]);
                        mod_SolicitudBandeja.AlmacenSolicitante = Convert.ToString(item["AlmacenSolicitante"]);
                        mod_SolicitudBandeja.AlmacenDestino = Convert.ToString(item["AlmacenDestino"]);
                        mod_SolicitudBandeja.FechaCaducidad = Convert.ToString(item["FechaCaducidad"]);
                        mod_SolicitudBandeja.ValorPedido = Convert.ToString(item["ValorPedido"]);
                        mod_SolicitudBandeja.ValorFactura = Convert.ToString(item["ValorFactura"]);
                        mod_SolicitudBandeja.NumeroBulto = Convert.ToString(item["NumeroBulto"]);
                        mod_SolicitudBandeja.NumeroPalet = Convert.ToString(item["NumeroPalet"]);
                        mod_SolicitudBandeja.IDPalet = Convert.ToString(item["IDPalet"]);
                        mod_SolicitudBandeja.TipoPedido = Convert.ToString(item["TipoPedido"]);
                        mod_SolicitudBandeja.IsCross = Convert.ToBoolean(item["IsCross"]);
                        mod_SolicitudBandeja.Estado = Convert.ToString(item["Estado"]);
                        mod_SolicitudBandeja.EstadoDesc = Convert.ToString(item["EstadoDesc"]);
                        mod_SolicitudBandeja.tipoPedidos = Convert.ToString(item["tipoPedidos"]);
                        lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSolBandeja);
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }

        [ActionName("BuscarDetPedidos")]
        [HttpGet]
        public FormResponseTransporte BuscarDetPedidos(string tipoPeD, string numPedido)
        {
            List<Tra_ConsultaDetPedidos> lst_retornoSolBandeja = new List<Tra_ConsultaDetPedidos>();
            Tra_ConsultaDetPedidos mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipoPeD);
                xmlParam.DocumentElement.SetAttribute("NumPedido", numPedido);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 306, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudBandeja = new Tra_ConsultaDetPedidos();
                        mod_SolicitudBandeja.Item = Convert.ToString(item["Item"]);
                        mod_SolicitudBandeja.NumPedido = Convert.ToString(item["NumPedido"]);
                        mod_SolicitudBandeja.CodigoProducto = Convert.ToString(item["CodigoProducto"]);
                        mod_SolicitudBandeja.Factura = Convert.ToString(item["Factura"]);
                        mod_SolicitudBandeja.FechaFactura = Convert.ToString(item["FechaFactura"]);
                        mod_SolicitudBandeja.Descripcion = Convert.ToString(item["Descripcion"]);
                        mod_SolicitudBandeja.CantidadPedido = Convert.ToString(item["CantidadPedido"]);
                        mod_SolicitudBandeja.PrecioUnitario = Convert.ToString(item["PrecioUnitario"]);
                        mod_SolicitudBandeja.UnidadCaja = Convert.ToString(item["UnidadCaja"]);
                        mod_SolicitudBandeja.Descuento1 = Convert.ToString(item["Descuento1"]);
                        mod_SolicitudBandeja.Descuento2 = Convert.ToString(item["Descuento2"]);
                        mod_SolicitudBandeja.Iva = Convert.ToString(item["Iva"]);
                        mod_SolicitudBandeja.Subtotal = Convert.ToString(item["Subtotal"]);
                        mod_SolicitudBandeja.Total = Convert.ToString(item["Total"]);
                        mod_SolicitudBandeja.CantidadxDespachar = Convert.ToString(item["CantidadxDespachar"]);
                        mod_SolicitudBandeja.CantidadDespachada = Convert.ToString(item["CantidadDespachada"]);
                        mod_SolicitudBandeja.CantidadPediente = Convert.ToString(item["CantidadPediente"]);
                        lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSolBandeja);
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }


        [ActionName("BuscarConModi")]
        [HttpGet]
        public FormResponseTransporte BuscarConModi(string idconsomodi, string codproveedormodi)
        {
            List<Tra_ModsolidacionPedidos> lst_Tra_ModsolidacionPedidos = new List<Tra_ModsolidacionPedidos>();
            List<Tra_ModPedidosGrabar> lst_Tra_ModPedidosGrabar = new List<Tra_ModPedidosGrabar>();
            List<Tra_ModDetPedidosGrabar> lst_Tra_ModDetPedidosGrabar = new List<Tra_ModDetPedidosGrabar>();
            Tra_ModsolidacionPedidos _Tra_ModsolidacionPedidos;
            Tra_ModPedidosGrabar _Tra_ModPedidosGrabar;
            Tra_ModDetPedidosGrabar _Tra_ModDetPedidosGrabar;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "5");
                xmlParam.DocumentElement.SetAttribute("IdConsolidacion", idconsomodi);
                xmlParam.DocumentElement.SetAttribute("CodProveedor", codproveedormodi);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 306, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        _Tra_ModsolidacionPedidos = new Tra_ModsolidacionPedidos();
                        _Tra_ModsolidacionPedidos.idconsolidacion = Convert.ToString(item["IDCONSOLIDACION"]);
                        _Tra_ModsolidacionPedidos.estado = Convert.ToString(item["ESTADO"]);
                        _Tra_ModsolidacionPedidos.fechaemision = Convert.ToString(item["FECHAEMISION"]);
                        _Tra_ModsolidacionPedidos.fechacaducidad = Convert.ToString(item["FECHACADUCIDAD"]);
                        _Tra_ModsolidacionPedidos.almacendestino = Convert.ToString(item["ALMACENDESTINO"]);
                        _Tra_ModsolidacionPedidos.idalmacendestino = Convert.ToString(item["IDALMACENDESTINO"]);
                        _Tra_ModsolidacionPedidos.idchofer = Convert.ToString(item["IDCHOFER"]);
                        _Tra_ModsolidacionPedidos.idayudante = Convert.ToString(item["IDAYUDANTE"]);
                        _Tra_ModsolidacionPedidos.idvehiculo = Convert.ToString(item["IDVEHICULO"]);
                        _Tra_ModsolidacionPedidos.cosrapido = Convert.ToString(item["COSRAPIDO"]);
                        lst_Tra_ModsolidacionPedidos.Add(_Tra_ModsolidacionPedidos);
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_Tra_ModsolidacionPedidos);

                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        _Tra_ModPedidosGrabar = new Tra_ModPedidosGrabar();
                        _Tra_ModPedidosGrabar.chkpedido = Convert.ToBoolean(item["chk"]);
                        _Tra_ModPedidosGrabar.NumPedido = Convert.ToString(item["NumPedido"]);
                        _Tra_ModPedidosGrabar.FechaPedido = Convert.ToString(item["FechaPedido"]);
                        _Tra_ModPedidosGrabar.RineRide = Convert.ToString(item["RineRide"]);
                        _Tra_ModPedidosGrabar.AlmacenSolicitante = Convert.ToString(item["AlmacenSolicitante"]);
                        _Tra_ModPedidosGrabar.AlmacenDestino = Convert.ToString(item["AlmacenDestino"]);
                        _Tra_ModPedidosGrabar.FechaCaducidad = Convert.ToString(item["FechaCaducidad"]);
                        _Tra_ModPedidosGrabar.ValorPedido = Convert.ToString(item["ValorPedido"]);
                        _Tra_ModPedidosGrabar.ValorFactura = Convert.ToString(item["ValorFactura"]);
                        _Tra_ModPedidosGrabar.NumeroBulto = Convert.ToString(item["NumeroBulto"]);
                        _Tra_ModPedidosGrabar.NumeroPalet = Convert.ToString(item["NumeroPalet"]);
                        _Tra_ModPedidosGrabar.IDPalet = Convert.ToString(item["IDPalet"]);
                        lst_Tra_ModPedidosGrabar.Add(_Tra_ModPedidosGrabar);
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_Tra_ModPedidosGrabar);

                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        _Tra_ModDetPedidosGrabar = new Tra_ModDetPedidosGrabar();
                        _Tra_ModDetPedidosGrabar.Item = Convert.ToString(item["Item"]);
                        _Tra_ModDetPedidosGrabar.NumPedido = Convert.ToString(item["NumPedido"]);
                        _Tra_ModDetPedidosGrabar.CodigoProducto = Convert.ToString(item["CodigoProducto"]);
                        _Tra_ModDetPedidosGrabar.Factura = Convert.ToString(item["Factura"]);
                        _Tra_ModDetPedidosGrabar.FechaFactura = Convert.ToString(item["FechaFactura"]);
                        _Tra_ModDetPedidosGrabar.Descripcion = Convert.ToString(item["Descripcion"]);
                        _Tra_ModDetPedidosGrabar.CantidadPedido = Convert.ToString(item["CantidadPedido"]);
                        _Tra_ModDetPedidosGrabar.PrecioUnitario = Convert.ToString(item["PrecioUnitario"]);
                        _Tra_ModDetPedidosGrabar.UnidadCaja = Convert.ToString(item["UnidadCaja"]);
                        _Tra_ModDetPedidosGrabar.Descuento1 = Convert.ToString(item["Descuento1"]);
                        _Tra_ModDetPedidosGrabar.Descuento2 = Convert.ToString(item["Descuento2"]);
                        _Tra_ModDetPedidosGrabar.Iva = Convert.ToString(item["Iva"]);
                        _Tra_ModDetPedidosGrabar.Subtotal = Convert.ToString(item["Subtotal"]);
                        _Tra_ModDetPedidosGrabar.Total = Convert.ToString(item["Total"]);
                        _Tra_ModDetPedidosGrabar.CantidadxDespachar = Convert.ToString(item["CantidadxDespachar"]);
                        _Tra_ModDetPedidosGrabar.CantidadDespachada = Convert.ToString(item["CantidadDespachada"]);
                        _Tra_ModDetPedidosGrabar.CantidadPediente = Convert.ToString(item["CantidadPediente"]);
                        lst_Tra_ModDetPedidosGrabar.Add(_Tra_ModDetPedidosGrabar);
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_Tra_ModDetPedidosGrabar);
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }

        [ActionName("secuenciaDirectorio")]
        [HttpGet]
        public FormResponseNotificacion GetsecuenciaDirectorio(string tipo)
        {
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;
            string secuencia = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {

                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                 new XElement("SecDirectorio",
                                 new XAttribute("desDirectorio", tipo != null ? tipo : ""))));

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 402, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    secuencia = ds.Tables[0].Rows[0]["Secuencia"].ToString();
                    FormResponse.root.Add(secuencia);
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());


            }
            catch
            {

            }
            return FormResponse;
        }


        [ActionName("grabarConsolidacion")]
        [HttpPost]
        public string grabarConsolidacion(Tra_SolicitudConsolidacionGrabar Consolidacion)
        {
            //seg_GrabaUsrAdmin userModel
            string Retorno = "true";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                //Datos de la cabecera
                if (Consolidacion.p_consolidacion != null)
                {
                    xmlParam.DocumentElement.SetAttribute("Tipo", Consolidacion.p_consolidacion.Tipo);
                    xmlParam.DocumentElement.SetAttribute("idconsolidacion", Consolidacion.p_consolidacion.idconsolidacion);
                    xmlParam.DocumentElement.SetAttribute("codChofer", Consolidacion.p_consolidacion.codChofer);
                    xmlParam.DocumentElement.SetAttribute("codAyudante", Consolidacion.p_consolidacion.codAyudante);
                    xmlParam.DocumentElement.SetAttribute("codVehiculo", Consolidacion.p_consolidacion.codVehiculo);
                    xmlParam.DocumentElement.SetAttribute("codProveedor", Consolidacion.p_consolidacion.codProveedor);
                    xmlParam.DocumentElement.SetAttribute("cosRapido", Consolidacion.p_consolidacion.cosRapido);
                    xmlParam.DocumentElement.SetAttribute("codAlmacenDestino", Consolidacion.p_consolidacion.codAlmacenDestino);
                    xmlParam.DocumentElement.SetAttribute("UsuarioCreacion", Consolidacion.p_consolidacion.usuarioCreacion);
                }
                //Datos Pedido Cabecera
                if (Consolidacion.p_Pedidos != null)
                {
                    foreach (Tra_SolicitudConsolidacionGrabar.Tra_PedidosGrabar dr in Consolidacion.p_Pedidos)
                    {
                        XmlElement elem = xmlParam.CreateElement("PedidoCab");
                        elem.SetAttribute("NumPedido", dr.NumPedido);
                        elem.SetAttribute("FechaPedido", dr.FechaPedido);
                        elem.SetAttribute("RineRide", dr.RineRide);
                        elem.SetAttribute("AlmacenSolicitante", dr.AlmacenSolicitante);
                        elem.SetAttribute("AlmacenDestino", dr.AlmacenDestino);
                        elem.SetAttribute("FechaCaducidad", dr.FechaCaducidad);
                        elem.SetAttribute("ValorPedido", dr.ValorPedido);
                        elem.SetAttribute("ValorFactura", dr.ValorFactura);
                        elem.SetAttribute("NumeroBulto", dr.NumeroBulto);
                        elem.SetAttribute("NumeroPalet", dr.NumeroPalet);
                        elem.SetAttribute("IDPalet", dr.IDPalet);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                //Detalle de Pedido
                if (Consolidacion.p_DetPedidos != null)
                {
                    foreach (Tra_SolicitudConsolidacionGrabar.Tra_DetPedidosGrabar dr in Consolidacion.p_DetPedidos)
                    {
                        XmlElement elem = xmlParam.CreateElement("PedidoDet");
                        elem.SetAttribute("Item", dr.Item);
                        elem.SetAttribute("NumPedido", dr.NumPedido);
                        elem.SetAttribute("CodigoProducto", dr.CodigoProducto);
                        elem.SetAttribute("Factura", dr.Factura);
                        elem.SetAttribute("FechaFactura", dr.FechaFactura);
                        elem.SetAttribute("Descripcion", dr.Descripcion);
                        elem.SetAttribute("CantidadPedido", dr.CantidadPedido);
                        elem.SetAttribute("PrecioUnitario", dr.PrecioUnitario);
                        elem.SetAttribute("UnidadCaja", dr.UnidadCaja);
                        elem.SetAttribute("Descuento1", dr.Descuento1);
                        elem.SetAttribute("Descuento2", dr.Descuento2);
                        elem.SetAttribute("Iva", dr.Iva);
                        elem.SetAttribute("Subtotal", dr.Subtotal);
                        elem.SetAttribute("Total", dr.Total);
                        elem.SetAttribute("CantidadxDespachar", dr.CantidadxDespachar);
                        elem.SetAttribute("CantidadDespachada", dr.CantidadDespachada);
                        elem.SetAttribute("CantidadPediente", dr.CantidadPediente);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 307, 1); //Articulo.Sol_P_Grabar	102	
                if (!ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                else
                    Retorno = "true" + "|" + ds.Tables[1].Rows[0]["Secuencia"].ToString();
            }
            catch (Exception ex)
            {
                Retorno = "false"+"|0";
            }
            return Retorno;
        }


       
        [ActionName("EliminarConsolidacion")]
        [HttpGet]
        public Boolean EliminarConsolidacion(string idconsolidacion, string codProveedorEliConso)
        {
            Boolean Retorno = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("idconsolidacion", idconsolidacion);
                xmlParam.DocumentElement.SetAttribute("codProveedor", codProveedorEliConso);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 309, 1); //Eliminar Consolidacion
                if (!ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                Retorno = false;
            }
            return Retorno;

        }


    }
}
