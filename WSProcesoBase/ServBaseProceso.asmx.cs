using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WSProcesoBase.Model;
using System.Web.Services;
using SAP.Middleware.Connector;
using System.Reflection;
using System.Xml;
using clibSeguridadCR;
using System.Xml.Linq;
using System.IO;
using Renci.SshNet;
using System.Net;
using Renci.SshNet.Common;
using Logger;
using System.Globalization;
using System.Threading;
using clibSeguridadCR.Seguridad;
using WSProcesoBase.Services;
using PowerBIEmbedded_AppOwnsData.Services;

namespace WSProcesoBase
{
    /// <summary>
    /// Descripción breve de ServBaseProceso
    /// 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ServBaseProceso : System.Web.Services.WebService
    {
        [WebMethod]
        public string envioCorreoLici(string titulo, string proveedor, string mensaje, string correo)
        {
            string rutaEmail = System.Web.Hosting.HostingEnvironment.MapPath("~/Plantilla");
            string asuntoEmail = "";
            rutaEmail = rutaEmail + "\\Correo.htm";
            asuntoEmail = " Portal de Proveedores";

            correo.ServEnvioClient objEnvMail = new correo.ServEnvioClient();

            string mensajeEmail = System.IO.File.ReadAllText(rutaEmail);
            string mensajeEmailparameter = mensajeEmail;
            try
            {
                mensajeEmailparameter = mensajeEmailparameter.Replace("_@Titulo", titulo);
                mensajeEmailparameter = mensajeEmailparameter.Replace("_@Proveedor", proveedor);
                mensajeEmailparameter = mensajeEmailparameter.Replace("_@Mensaje", mensaje);


                //String retorno = objEnvMail.EnviarCorreo("", drow.CorreoE, "", "", asuntoEmail, mensajeEmailparameter, true, true, false, null);

                Thread t = new Thread(() => objEnvMail.EnviarCorreo("", correo, "", "", asuntoEmail, mensajeEmailparameter, true, true, false, null));
                t.Start();

            }
            catch (Exception)
            {


            }

            return "";
        }

        [WebMethod]
        public string getHorarioCitas(string bodega, string CodProveedor, string fecha)
        {
            WSNuoBuscarHorarios.WS_CITAS_SIPECOM wsNuo = new WSNuoBuscarHorarios.WS_CITAS_SIPECOM();
            WSNuoBuscarHorarios.CitaResponse datos = new WSNuoBuscarHorarios.CitaResponse();

            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                datos = wsNuo.NUO_citas(bodega, CodProveedor, fecha);
                xmlParam.DocumentElement.SetAttribute("NumError", datos.num_error.ToString());
                int i = 0;
                try
                {
                    xmlParam.DocumentElement.SetAttribute("DescError", datos.error.ToString());
                }
                catch (Exception)
                {
                    xmlParam.DocumentElement.SetAttribute("DescError", "");
                }

                foreach (var item in datos.libre)
                {
                    XmlElement elem = xmlParam.CreateElement("Horario");
                    elem.SetAttribute("DIA", item.FECHA);
                    elem.SetAttribute("HORA_INICIO", item.HORA_INICIO);
                    //elem.SetAttribute("HORA_INICIO", "14:00");
                    elem.SetAttribute("HORA_FIN", item.HORA_FIN);
                    // elem.SetAttribute("HORA_FIN", "15:00");
                    elem.SetAttribute("COLOR", "blue");
                    elem.SetAttribute("COLORTEXTO", "white");
                    elem.SetAttribute("TITULO", "Horario Disponible");
                    elem.SetAttribute("ID", i.ToString());
                    xmlParam.DocumentElement.AppendChild(elem);
                    i++;
                }
                foreach (var item in datos.aprobada)
                {
                    XmlElement elem = xmlParam.CreateElement("Horario");
                    elem.SetAttribute("DIA", item.FECHA);
                    elem.SetAttribute("HORA_INICIO", item.HORA_INICIO);
                    //elem.SetAttribute("HORA_INICIO", "14:00");
                    elem.SetAttribute("HORA_FIN", item.HORA_FIN);
                    // elem.SetAttribute("HORA_FIN", "15:00");
                    elem.SetAttribute("COLOR", "green");
                    elem.SetAttribute("COLORTEXTO", "white");
                    elem.SetAttribute("TITULO", "Horario Asignado");
                    elem.SetAttribute("ID", i.ToString());
                    xmlParam.DocumentElement.AppendChild(elem);
                    i++;
                }
                foreach (var item in datos.ocupada)
                {
                    XmlElement elem = xmlParam.CreateElement("Horario");
                    elem.SetAttribute("DIA", item.FECHA);
                    elem.SetAttribute("HORA_INICIO", item.HORA_INICIO);
                    //elem.SetAttribute("HORA_INICIO", "14:00");
                    elem.SetAttribute("HORA_FIN", item.HORA_FIN);
                    // elem.SetAttribute("HORA_FIN", "15:00");
                    elem.SetAttribute("COLOR", "grey");
                    elem.SetAttribute("COLORTEXTO", "white");
                    elem.SetAttribute("TITULO", "Horario no Disponible");
                    elem.SetAttribute("ID", i.ToString());
                    xmlParam.DocumentElement.AppendChild(elem);
                    i++;
                }
                foreach (var item in datos.pendiente)
                {
                    XmlElement elem = xmlParam.CreateElement("Horario");
                    elem.SetAttribute("DIA", item.FECHA);
                    elem.SetAttribute("HORA_INICIO", item.HORA_INICIO);
                    //elem.SetAttribute("HORA_INICIO", "14:00");
                    elem.SetAttribute("HORA_FIN", item.HORA_FIN);
                    // elem.SetAttribute("HORA_FIN", "15:00");
                    elem.SetAttribute("COLOR", "yellow");
                    elem.SetAttribute("COLORTEXTO", "black");
                    elem.SetAttribute("TITULO", "Horario Solicitado");
                    elem.SetAttribute("ID", i.ToString());
                    xmlParam.DocumentElement.AppendChild(elem);
                    i++;
                }
            }
            catch (Exception ex)
            {

                xmlParam.DocumentElement.SetAttribute("NumError", "100");
            }



            return xmlParam.OuterXml;
        }

        [WebMethod]
        public Boolean setSolpedidoCross(XmlDocument lst_mod_Tra_pedidoCross, int cantidad)
        {
            Boolean Retorno = true;

            Logger.Logger loge = new Logger.Logger();
            string ordenCompra = "";
            string UsuarioPRD = ((string)System.Configuration.ConfigurationManager.AppSettings["UsuarioPRDPI"]).Trim();
            string ClavePRD = ((string)System.Configuration.ConfigurationManager.AppSettings["ClavePRDPI"]).Trim();
            string RutaCross = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLogCross"]).Trim();
            try
            {
                Sap_EnvioPedidoCross.SI_CrossDockDistr_OUT_SYNCService objWs = new Sap_EnvioPedidoCross.SI_CrossDockDistr_OUT_SYNCService();
                objWs.Credentials = new System.Net.NetworkCredential(UsuarioPRD, ClavePRD);
                objWs.Timeout = 120000;
                Sap_EnvioPedidoCross.DT_CrossDockDistr_Request lv_WsReq = new Sap_EnvioPedidoCross.DT_CrossDockDistr_Request();

                //XmlNodeList Detalles;
                //Detalles = lst_mod_Tra_SolicitudCita.SelectNodes("//SOLICITUDCITA");
                Sap_EnvioPedidoCross.DT_CrossDockDistr_RequestItem[] lv_WsReqItem = new Sap_EnvioPedidoCross.DT_CrossDockDistr_RequestItem[cantidad - 1];
                int i = 0;

                foreach (XmlNode Elementot in lst_mod_Tra_pedidoCross.DocumentElement)
                {
                    Sap_EnvioPedidoCross.DT_CrossDockDistr_RequestItem tmp = new Sap_EnvioPedidoCross.DT_CrossDockDistr_RequestItem();
                    XmlDocument tmpd = new XmlDocument();
                    tmpd.LoadXml(Elementot.OuterXml);
                    XmlNode Elemento = tmpd.DocumentElement;
                    tmp.BTYPB = "1";
                    tmp.BLNRB = Elemento.SelectSingleNode("//DATOS/@Orden_compra").Value;
                    ordenCompra = Elemento.SelectSingleNode("//DATOS/@Orden_compra").Value;
                    tmp.BPOSB = Elemento.SelectSingleNode("//DATOS/@Posicion_item").Value;
                    tmp.BTYPA = "1";
                    tmp.BLNRA = Elemento.SelectSingleNode("//DATOS/@Documento_salida").Value;
                    tmp.BPOSA = Elemento.SelectSingleNode("//DATOS/@Posicion_salida").Value;
                    tmp.MNGFT = Elemento.SelectSingleNode("//DATOS/@Cantidad").Value;
                    lv_WsReqItem[i] = tmp;
                    i++;
                }
                lv_WsReq.PT_DISTR = lv_WsReqItem;
                Sap_EnvioPedidoCross.DT_CrossDockDistr_Response lv_retorno = new Sap_EnvioPedidoCross.DT_CrossDockDistr_Response();
                lv_retorno = objWs.SI_CrossDockDistr_OUT_SYNC(lv_WsReq);


                loge.FilePath = RutaCross;
                loge.WriteMensaje("Pedido Cross " + " N°: " + ordenCompra + ' ' + lv_retorno.CODERROR + ' ' + lv_retorno.DESERROR);
                loge.FilePath = RutaCross;
                loge.Linea();

            }
            catch (Exception ex)
            {
                Retorno = false;

                loge.FilePath = RutaCross;
                loge.WriteMensaje("Pedido Cross " + " N°: " + ordenCompra + ' ' + ex.Message.ToString());
                loge.FilePath = RutaCross;
                loge.Linea();

            }
            return Retorno;
        }



        [WebMethod]
        public FormResponse setSolcitudCitaCER(XmlDocument lst_mod_Tra_SolicitudCita, int cantidad, int cantidad1)
        {
            FormResponse Retorno = new FormResponse();
            Logger.Logger loge = new Logger.Logger();
            string consolidacion = "";
            string UsuarioPRD = ((string)System.Configuration.ConfigurationManager.AppSettings["UsuarioPRDPI"]).Trim();
            string ClavePRD = ((string)System.Configuration.ConfigurationManager.AppSettings["ClavePRDPI"]).Trim();
            string RutaCitas = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLogCitas"]).Trim();
            try
            {
                WS_SolicitudCitasCER.SI_SolicitudCita_OUT_SYNCService objWs = new WS_SolicitudCitasCER.SI_SolicitudCita_OUT_SYNCService();
                objWs.Credentials = new System.Net.NetworkCredential(UsuarioPRD, ClavePRD);
                objWs.Timeout = 120000;
                WS_SolicitudCitasCER.DT_SolicitudCita_Request lv_WsReq = new WS_SolicitudCitasCER.DT_SolicitudCita_Request();

                //XmlNodeList Detalles;
                //Detalles = lst_mod_Tra_SolicitudCita.SelectNodes("//SOLICITUDCITA");
                WS_SolicitudCitasCER.DT_SolicitudCita_RequestItem[] lv_WsReqItem = new WS_SolicitudCitasCER.DT_SolicitudCita_RequestItem[cantidad - 1];
                //       WS_SolicitudCitasCER.DT_SolicitudCita_RequestItem1[] lv_WsReqItemChefer = new WS_SolicitudCitasCER.DT_SolicitudCita_RequestItem1[0];
                int i = 0;
                foreach (XmlNode Elementot in lst_mod_Tra_SolicitudCita.DocumentElement)
                {
                    WS_SolicitudCitasCER.DT_SolicitudCita_RequestItem tmp = new WS_SolicitudCitasCER.DT_SolicitudCita_RequestItem();
                    XmlDocument tmpd = new XmlDocument();
                    tmpd.LoadXml(Elementot.OuterXml);
                    XmlNode Elemento = tmpd.DocumentElement;
                    tmp.BULTOS = Elemento.SelectSingleNode("//DATOS/@Bultos").Value;
                    tmp.IDCONSOLID = Elemento.SelectSingleNode("//DATOS/@ID_consolidacion").Value;
                    consolidacion = Elemento.SelectSingleNode("//DATOS/@ID_consolidacion").Value;
                    tmp.LIFNR = Elemento.SelectSingleNode("//DATOS/@ID_Proveedor").Value;
                    tmp.TIPO_VEHICULO = Elemento.SelectSingleNode("//DATOS/@Tipo_Vehiculo").Value;
                    tmp.HORA_INI = Elemento.SelectSingleNode("//DATOS/@hora_inicio").Value;
                    tmp.HORA_FIN = Elemento.SelectSingleNode("//DATOS/@hora_fin").Value;
                    tmp.ESTADO_CITA = Elemento.SelectSingleNode("//DATOS/@CodEstado").Value;
                    tmp.FEC_PROPUESTA = Elemento.SelectSingleNode("//DATOS/@Fecha_propuesta").Value;
                    tmp.PALLET = Elemento.SelectSingleNode("//DATOS/@Pallet").Value;
                    tmp.XBLNR = Elemento.SelectSingleNode("//DATOS/@ID_Factura").Value;
                    tmp.EBELN = Elemento.SelectSingleNode("//DATOS/@Orden_compra").Value;
                    tmp.WERKS = Elemento.SelectSingleNode("//DATOS/@ID_Bodega").Value;
                    lv_WsReqItem[i] = tmp;
                    i++;
                }
                i = 0;


                lv_WsReq.PT_CITAS = lv_WsReqItem;

                //   lv_WsReq.PT_CITASCAM = lv_WsReqItemChefer;
                WS_SolicitudCitasCER.DT_SolicitudCita_Response lv_retorno = new WS_SolicitudCitasCER.DT_SolicitudCita_Response();
                lv_retorno = objWs.SI_SolicitudCita_OUT_SYNC(lv_WsReq);
                loge.FilePath = RutaCitas;
                loge.WriteMensaje("Solicitud de Cita  N°: " + consolidacion + ' ' + lv_retorno.CODERROR + ' ' + lv_retorno.DESERROR);
                loge.FilePath = RutaCitas;
                loge.Linea();
                Retorno.codError = lv_retorno.CODERROR;
                Retorno.msgError = lv_retorno.DESERROR;
                Retorno.success = true;

            }
            catch (Exception ex)
            {
                Retorno.codError = "10000";
                Retorno.msgError = "Error en Conexion con Corporacion el Rosado";
                Retorno.success = false;
                loge.FilePath = RutaCitas;
                loge.WriteMensaje(ex.Message.ToString());
                loge.FilePath = RutaCitas;
                loge.Linea();
            }
            return Retorno;
        }

        [WebMethod]
        public Boolean getConsultaEAN(String codEANart, String tipoEANart)
        {
            Boolean Retorno = false;

            try
            {
                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;
                IRfcFunction fndatosmaestro = repo.CreateFunction("ZPPVALIDAEAN");
                fndatosmaestro.SetValue("P_CODEAN", codEANart);
                //fndatosmaestro.SetValue("P_MEINH", "ST");
                fndatosmaestro.SetValue("P_NUMTP", tipoEANart);
                fndatosmaestro.Invoke(AppConfig.dest);
                var coderror = fndatosmaestro.GetString("CODERROR");
                var deserror = fndatosmaestro.GetString("DESERROR");
                if (coderror == "")
                {
                    Retorno = true;

                }
            }
            catch (Exception ex)
            {
                Retorno = false;

            }
            return Retorno;
        }



        [WebMethod]
        public List<string> ConsultaArtBapi(String tipo, String codigo,
                                               String chkCodRef, String CodRef,
                                               String chkCodSap, String CodSap,
                                               String chkGrupoArt, String GrupoArt,
                                               String chkLinea, String LineaNegocio, String CodProveedorCons)
        {

            List<string> listaRetorno = new List<string>();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;
                IRfcFunction fndatosmaestro = repo.CreateFunction("ZPPARTICULOCHECK");
                var tipoConsulta = "";    //P_TIPO  Tipo de consulta  S: por codigo SAP ; R : cod. referencia proveedor; G: grupo de artículos;
                var codSAPArt = ""; //P_MATNR Codigo SAP de articulo
                var codSAPProv = (Convert.ToInt64(CodProveedorCons) + 10000000000).ToString().Substring(1); //P_LIFNR Codigo SAP de proveedor                                        
                var codRefProv = "";  //P_IDNLF Codigo de referencia proveedor
                var grupoArt = "";//P_MATKL

                if (tipo == "1")
                {
                    if (Convert.ToBoolean(chkCodSap))
                    {
                        tipoConsulta = "S";
                        //codSAPArt = (Convert.ToInt64(CodSap) + 1000000000000000000).ToString().Substring(1);
                        try
                        {
                            codSAPArt = (Convert.ToInt64(CodSap) + 1000000000000000000).ToString().Substring(1);
                        }
                        catch (Exception)
                        {

                            codSAPArt = CodSap;
                        }
                    }

                    if (Convert.ToBoolean(chkCodRef))
                    {
                        tipoConsulta = "R";
                        codRefProv = CodRef;
                    }

                    if (Convert.ToBoolean(chkGrupoArt))
                    {
                        tipoConsulta = "G";
                        grupoArt = GrupoArt;
                    }

                    fndatosmaestro.SetValue("P_TIPO", tipoConsulta);
                    fndatosmaestro.SetValue("P_MATNR", codSAPArt);
                    fndatosmaestro.SetValue("P_LIFNR", codSAPProv);
                    fndatosmaestro.SetValue("P_IDNLF", codRefProv);
                    fndatosmaestro.SetValue("P_MATKL", grupoArt);
                    fndatosmaestro.Invoke(AppConfig.dest);
                    var coderror = fndatosmaestro.GetString("CODERROR");
                    var deserror = fndatosmaestro.GetString("DESERROR");
                    if (coderror == "0")
                    {

                        //Obtener datos de la consulta BAPI     
                        var PT_ARTICULOS = fndatosmaestro.GetTable("PT_ARTICULOS");
                        XElement xroot = new XElement("Root");
                        List<XElement> wlistaRetorno = (from wpa in PT_ARTICULOS
                                                        select new XElement("PT_ARTICULOS",
                                                             new XAttribute("CODSAP", Convert.ToInt64(wpa.GetString("MATNR"))),
                                                             new XAttribute("CODARTPROV", wpa.GetString("IDNLF")),
                                                             new XAttribute("LINEANEGOCIO", wpa.GetString("LINNEG")),
                                                             new XAttribute("DESLINEGOCIO", wpa.GetString("KSCHL")),
                                                             new XAttribute("MARCA", wpa.GetString("BRAND_ID")),
                                                             new XAttribute("MARCADES", wpa.GetString("BRAND_DESCR")),
                                                             new XAttribute("DESARTICULO", wpa.GetString("MAKTX")),
                                                             new XAttribute("PAIS", wpa.GetString("WHERL")),
                                                             new XAttribute("REGION", wpa.GetString("WHERL") + "-" + wpa.GetString("WHERR")),
                                                             new XAttribute("TAMAÑO", wpa.GetString("GROES")),
                                                             new XAttribute("GRADOAL", wpa.GetString("EXTWG")),
                                                             new XAttribute("COLOR", wpa.GetString("COLOR_ATINN")),
                                                             new XAttribute("SIZE", wpa.GetString("SIZE1_ATINN")),
                                                             new XAttribute("FRAGANCIA", wpa.GetString("FRAGANCIA")),
                                                             new XAttribute("TIPOS", wpa.GetString("TIPOS")),
                                                             new XAttribute("SABOR", wpa.GetString("SABOR")),
                                                             new XAttribute("NUMOBJINTERNO", wpa.GetString("CUOBF")),
                                                             new XAttribute("CLASFISCAL", wpa.GetString("TAKLV")),
                                                             new XAttribute("TIPODEDUCCION", wpa.GetString("PLGTP")),
                                                             new XAttribute("LABOR", wpa.GetString("LABOR")),
                                                             new XAttribute("MODELO", wpa.GetString("NORMT")),
                                                             new XAttribute("TIPOCERT", wpa.GetString("URZTP")),
                                                             new XAttribute("UNIMEDIDABASE", wpa.GetString("MEINS")),
                                                             new XAttribute("UNIMEDIDAPEDIDO", wpa.GetString("BSTME")),
                                                             new XAttribute("UNIMEDIDAPEDIDODES", wpa.GetString("MSEHL")),
                                                             new XAttribute("FACTORCONVERSION", wpa.GetString("UMREZ")),
                                                             new XAttribute("UNIMEDIDABASE2", wpa.GetString("LMEIN")),
                                                             new XAttribute("PESON", wpa.GetString("NTGEW")),
                                                             new XAttribute("PESOB", wpa.GetString("BRGEW")),
                                                             new XAttribute("LONGITUD", wpa.GetString("LAENG")),
                                                             new XAttribute("ANCHO", wpa.GetString("BREIT")),
                                                             new XAttribute("ALTURA", wpa.GetString("HOEHE")),
                                                             new XAttribute("PRECIOB", wpa.GetString("PRECIO_BRUTO")),
                                                             new XAttribute("DESCT1", wpa.GetString("DSCTO1")),
                                                             new XAttribute("DESCT2", wpa.GetString("DSCTO2")),
                                                             new XAttribute("IMPVERDE", wpa.GetString("IMPTO_VERDE")),
                                                             new XAttribute("CODSAPPROV", wpa.GetString("LIFNR")),
                                                             new XAttribute("ORGCOMPRAS", wpa.GetString("EKORG")),
                                                             new XAttribute("NUMANTIGUO", wpa.GetString("BISMT")),
                                                             new XAttribute("CALENPLAN", wpa.GetString("MRPPP")),
                                                             new XAttribute("TIPOMAT", wpa.GetString("MTART")),
                                                             new XAttribute("CATMAT", wpa.GetString("ATTYP")),
                                                             new XAttribute("GRPART", wpa.GetString("MATKL")),
                                                             new XAttribute("SECART", wpa.GetString("SECCION")),
                                                             new XAttribute("SURTPAR", wpa.GetString("LTSNR")),
                                                             new XAttribute("MATERIA", wpa.GetString("WRKST")),
                                                             new XAttribute("COSTOFOB", wpa.GetString("COSTOFOB").Replace(',', '.')),
                                                             new XAttribute("INDPEDIDO", wpa.GetString("SERVV")),
                                                             new XAttribute("GRPCOMPRAS", wpa.GetString("WEKGR")),
                                                             new XAttribute("CATVALORACION", wpa.GetString("WBKLA")),
                                                             new XAttribute("CONDALMACENAJE", wpa.GetString("RAUBE")),
                                                             new XAttribute("CLASELISTASURT", wpa.GetString("BBTYP")),
                                                             new XAttribute("STATUSMAT", wpa.GetString("MSTAE")),
                                                             new XAttribute("STATUSMATCAD", wpa.GetString("MSTAV")),
                                                             new XAttribute("FECHAINICIO", wpa.GetString("MSTDV")),
                                                             new XAttribute("COLECTEMPORADA", wpa.GetString("SAITY")),
                                                             new XAttribute("TIPTEMPORADA", wpa.GetString("SAISO")),
                                                             new XAttribute("ANIOESTACION", wpa.GetString("SAISJ")),
                                                             new XAttribute("JERARQUIA", wpa.GetString("PRODH")))).ToList<XElement>();
                        if (wlistaRetorno != null)
                        {
                            xroot.Add(wlistaRetorno);
                            listaRetorno.Add(xroot.ToString());
                            return listaRetorno;
                        }

                    }




                }

                if (tipo == "2")
                {
                    String[] lista = codigo.Split('|');

                    var indDetalle = 0;
                    var indice = 0;
                    if (lista[0] == "")
                    {
                        indice = 1;
                    }
                    else
                    {
                        indice = 0;
                    }

                    for (var i = indice; i < lista.Length; i++)
                    {
                        XElement xroot = new XElement("Root");
                        List<XElement> wlistaRetorno = new List<XElement>();
                        tipoConsulta = "S";
                        codigo = lista[i];
                        indDetalle = i;
                        codSAPArt = (Convert.ToInt64(codigo) + 1000000000000000000).ToString().Substring(1);
                        fndatosmaestro.SetValue("P_TIPO", tipoConsulta);
                        fndatosmaestro.SetValue("P_MATNR", codSAPArt);
                        fndatosmaestro.SetValue("P_LIFNR", codSAPProv);
                        fndatosmaestro.SetValue("P_IDNLF", codRefProv);
                        fndatosmaestro.SetValue("P_MATKL", grupoArt);
                        fndatosmaestro.Invoke(AppConfig.dest);


                        var coderror = fndatosmaestro.GetString("CODERROR");
                        if (coderror == "0")
                        {

                            //Obtener datos de EAN desde la BAPI
                            var PT_EAN = fndatosmaestro.GetTable("PT_EAN");                                 //      **************ESTRUCTURA # 1 PT_EAN****************

                            wlistaRetorno = (from wpa in PT_EAN
                                             select new XElement("PT_EAN",
                                                      new XAttribute("IDDETALLE", indDetalle.ToString()),
                                                      new XAttribute("UNIDVISU", wpa.GetString("MEINH")),
                                                      new XAttribute("NUMEAN", wpa.GetString("EAN11")),
                                                      new XAttribute("TIPEAN", wpa.GetString("EANTP")),
                                                      new XAttribute("EANPRI", wpa.GetString("HPEAN")))).ToList<XElement>();
                            if (wlistaRetorno != null)
                                xroot.Add(wlistaRetorno);



                            //Obtener datos de ALMACEN desde la BAPI
                            var PT_ALMACEN = fndatosmaestro.GetTable("PT_ALMACEN");                      //      **************ESTRUCTURA # 2 PT_ALMACEN****************
                            wlistaRetorno = (from wpa in PT_ALMACEN
                                             select new XElement("PT_ALMACEN",
                                                      new XAttribute("IDDETALLE", indDetalle.ToString()),
                                                      new XAttribute("NUMALM", wpa.GetString("LGNUM")),
                                                      new XAttribute("TIPALM", wpa.GetString("LGTYP")),
                                                      new XAttribute("INDALME", wpa.GetString("LTKZE")),
                                                      new XAttribute("INDALMC", wpa.GetString("LGBKZ")),
                                                      new XAttribute("INDALMS", wpa.GetString("LTKZA")))).ToList<XElement>();
                            if (wlistaRetorno != null)
                                xroot.Add(wlistaRetorno);

                            //Obtener datos de catalogacion desde la BAPI
                            var PT_CATALOGACION = fndatosmaestro.GetTable("PT_CATALOGACION");           //      **************ESTRUCTURA # 3 PT_CATALOGACION****************
                            wlistaRetorno = (from wpa in PT_CATALOGACION
                                             select new XElement("PT_CATALOGACION",
                                                      new XAttribute("IDDETALLE", indDetalle.ToString()),
                                                      new XAttribute("CODCAT", wpa.GetString("FILIA")),
                                                      new XAttribute("CANDIS", wpa.GetString("VTWEG")),
                                                      new XAttribute("DESCAT", wpa.GetString("NAME1")))).ToList<XElement>();
                            if (wlistaRetorno != null)
                                xroot.Add(wlistaRetorno);


                            //Obtener datos de BALANZAS desde la BAPI
                            var PT_BALANZAS = fndatosmaestro.GetTable("PT_BALANZAS");                 //      **************ESTRUCTURA # 4 PT_BALANZAS****************
                            wlistaRetorno = (from wpa in PT_BALANZAS
                                             select new XElement("PT_BALANZAS",
                                                      new XAttribute("IDDETALLE", indDetalle.ToString()),
                                                      new XAttribute("GRPBAL", wpa.GetString("SCAGR")),
                                                      new XAttribute("DESBAL", wpa.GetString("BEZEI20")),
                                                      new XAttribute("CNLDIS", wpa.GetString("VTWEG")))).ToList<XElement>();
                            if (wlistaRetorno != null)
                                xroot.Add(wlistaRetorno);

                            //Obtener datos de UNIDADES DE MEDIDA desde la BAPI
                            var PT_UNIMED = fndatosmaestro.GetTable("PT_UNIMED");                 //      **************ESTRUCTURA # 5 PT_UNIMED****************
                            wlistaRetorno = (from wpa in PT_UNIMED
                                             select new XElement("PT_UNIMED",
                                                      new XAttribute("IDDETALLE", indDetalle.ToString()),
                                                      new XAttribute("UNBASE", wpa.GetString("MEINS")),
                                                      new XAttribute("LONGIT", wpa.GetString("LAENG")),
                                                      new XAttribute("ANCHO", wpa.GetString("BREIT")),
                                                      new XAttribute("ALTURA", wpa.GetString("HOEHE")),
                                                      new XAttribute("PESBRT", wpa.GetString("BRGEW")),
                                                      new XAttribute("CONVER", wpa.GetString("UMREZ")),
                                                      new XAttribute("VOLUM", wpa.GetString("VOLUM")),
                                                      new XAttribute("UNIVOL", wpa.GetString("VOLEH")))).ToList<XElement>();
                            if (wlistaRetorno != null)
                                xroot.Add(wlistaRetorno);

                            //Obtener datos de UNIDADES DE MEDIDA desde la BAPI
                            var PT_ALMACEN_MARD = fndatosmaestro.GetTable("PT_ALMACEN_MARD");                 //   **************ESTRUCTURA # 6 PT_ALMACEN_MARD****************
                            wlistaRetorno = (from wpa in PT_ALMACEN_MARD
                                             select new XElement("PT_ALMACEN_MARD",
                                                      new XAttribute("IDDETALLE", indDetalle.ToString()),
                                                      new XAttribute("CENTRO", wpa.GetString("WERKS")),
                                                      new XAttribute("ALMAC", wpa.GetString("LGORT")))).ToList<XElement>();
                            if (wlistaRetorno != null)
                                xroot.Add(wlistaRetorno);


                            //Obtener datos de la consulta BAPI     
                            var PT_ARTICULOS = fndatosmaestro.GetTable("PT_ARTICULOS");              //      **************ESTRUCTURA # 7 PT_BALANZAS****************

                            wlistaRetorno = (from wpa in PT_ARTICULOS
                                             select new XElement("PT_ARTICULOS",
                                                  new XAttribute("IDDETALLE", indDetalle.ToString()),
                                                  new XAttribute("CODSAP", Convert.ToInt64(wpa.GetString("MATNR"))),
                                                  new XAttribute("CODARTPROV", wpa.GetString("IDNLF")),
                                                  new XAttribute("LINEANEGOCIO", wpa.GetString("LINNEG")),
                                                  new XAttribute("DESLINEGOCIO", wpa.GetString("KSCHL")),
                                                  new XAttribute("MARCA", wpa.GetString("BRAND_ID")),
                                                  new XAttribute("MARCADES", wpa.GetString("BRAND_DESCR")),
                                                  new XAttribute("DESARTICULO", wpa.GetString("MAKTX")),
                                                  new XAttribute("PAIS", wpa.GetString("WHERL")),
                                                  new XAttribute("REGION", wpa.GetString("WHERL") + "-" + wpa.GetString("WHERR")),
                                                  new XAttribute("TAMAÑO", wpa.GetString("GROES")),
                                                  new XAttribute("GRADOAL", wpa.GetString("EXTWG")),
                                                  new XAttribute("COLOR", wpa.GetString("COLOR_ATINN")),
                                                  new XAttribute("SIZE", wpa.GetString("SIZE1_ATINN")),
                                                  new XAttribute("FRAGANCIA", wpa.GetString("FRAGANCIA")),
                                                  new XAttribute("TIPOS", wpa.GetString("TIPOS")),
                                                  new XAttribute("SABOR", wpa.GetString("SABOR")),
                                                  new XAttribute("NUMOBJINTERNO", wpa.GetString("CUOBF")),
                                                  new XAttribute("CLASFISCAL", wpa.GetString("TAKLV")),
                                                  new XAttribute("TIPODEDUCCION", wpa.GetString("PLGTP")),
                                                  new XAttribute("LABOR", wpa.GetString("LABOR")),
                                                  new XAttribute("MODELO", wpa.GetString("NORMT")),
                                                  new XAttribute("TIPOCERT", wpa.GetString("URZTP")),
                                                  new XAttribute("UNIMEDIDABASE", wpa.GetString("MEINS")),
                                                  new XAttribute("UNIMEDIDAPEDIDO", wpa.GetString("BSTME")),
                                                  new XAttribute("UNIMEDIDAPEDIDODES", wpa.GetString("MSEHL")),
                                                  new XAttribute("FACTORCONVERSION", wpa.GetString("UMREZ")),
                                                  new XAttribute("UNIMEDIDABASE2", wpa.GetString("LMEIN")),
                                                  new XAttribute("PESON", wpa.GetString("NTGEW")),
                                                  new XAttribute("PESOB", wpa.GetString("BRGEW")),
                                                  new XAttribute("LONGITUD", wpa.GetString("LAENG")),
                                                  new XAttribute("ANCHO", wpa.GetString("BREIT")),
                                                  new XAttribute("ALTURA", wpa.GetString("HOEHE")),
                                                  new XAttribute("PRECIOB", wpa.GetString("PRECIO_BRUTO")),
                                                  new XAttribute("DESCT1", wpa.GetString("DSCTO1")),
                                                  new XAttribute("DESCT2", wpa.GetString("DSCTO2")),
                                                  new XAttribute("IMPVERDE", wpa.GetString("IMPTO_VERDE")),
                                                  new XAttribute("CODSAPPROV", wpa.GetString("LIFNR")),
                                                  new XAttribute("ORGCOMPRAS", wpa.GetString("EKORG")),
                                                  new XAttribute("NUMANTIGUO", wpa.GetString("BISMT")),
                                                  new XAttribute("CALENPLAN", wpa.GetString("MRPPP")),
                                                  new XAttribute("TIPOMAT", wpa.GetString("MTART")),
                                                  new XAttribute("CATMAT", wpa.GetString("ATTYP")),
                                                  new XAttribute("GRPART", wpa.GetString("MATKL")),
                                                  new XAttribute("SECART", wpa.GetString("SECCION")),
                                                  new XAttribute("SURTPAR", wpa.GetString("LTSNR")),
                                                  new XAttribute("MATERIA", wpa.GetString("WRKST")),
                                                  new XAttribute("COSTOFOB", wpa.GetString("COSTOFOB").Replace(',', '.')),
                                                  new XAttribute("INDPEDIDO", wpa.GetString("SERVV")),
                                                  new XAttribute("GRPCOMPRAS", wpa.GetString("WEKGR")),
                                                  new XAttribute("CATVALORACION", wpa.GetString("WBKLA")),
                                                  new XAttribute("CONDALMACENAJE", wpa.GetString("RAUBE")),
                                                  new XAttribute("CLASELISTASURT", wpa.GetString("BBTYP")),
                                                  new XAttribute("STATUSMAT", wpa.GetString("MSTAE")),
                                                  new XAttribute("STATUSMATCAD", wpa.GetString("MSTAV")),
                                                  new XAttribute("FECHAINICIO", wpa.GetString("MSTDV")),
                                                  new XAttribute("COLECTEMPORADA", wpa.GetString("SAITY")),
                                                  new XAttribute("TIPTEMPORADA", wpa.GetString("SAISO")),
                                                  new XAttribute("ANIOESTACION", wpa.GetString("SAISJ")),
                                                  new XAttribute("VOLUMEN", wpa.GetString("VOLEH")),
                                                  new XAttribute("UNIVOL", wpa.GetString("VOLEH")),
                                                  new XAttribute("UNIVENTA", wpa.GetString("WVRKM")),
                                                  new XAttribute("UNIES", wpa.GetString("WAUSM")),
                                                  new XAttribute("CANPED", wpa.GetString("ZEINR")),
                                                  new XAttribute("JERARQUIA", wpa.GetString("PRODH")))).ToList<XElement>();
                            if (wlistaRetorno != null)
                                xroot.Add(wlistaRetorno);

                            if (xroot != null)
                                listaRetorno.Add(xroot.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaRetorno = new List<string>();
            }


            return listaRetorno;


        }


        [WebMethod]
        public string copiaXMLRINE(string nombreArchivo, string contenido)
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            try
            {
                Logger.Logger loge = new Logger.Logger();

                string rutaEnvio = (string)System.Configuration.ConfigurationManager.AppSettings["RutaEnvioXML"];
                File.WriteAllText(Path.Combine(rutaEnvio, nombreArchivo), contenido);
                loge.FilePath = p_Log;
                loge.WriteMensaje("copiaXMLRINE " + " OK " + nombreArchivo);
                loge.FilePath = p_Log;
                loge.Linea();
            }
            catch (Exception e)
            {
                Logger.Logger loge = new Logger.Logger();
                loge.FilePath = p_Log;
                loge.WriteMensaje("copiaXMLRINE " + " ERROR " + e.Message + " " + nombreArchivo);
                loge.FilePath = p_Log;
                loge.Linea();
            }

            return "";
        }
        [WebMethod]
        public string copiaTXTRINE(string nombreArchivo, string contenido)
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            try
            {
                Logger.Logger loge = new Logger.Logger();

                string rutaEnvio = (string)System.Configuration.ConfigurationManager.AppSettings["RutaEnvioTXT"];
                File.WriteAllText(Path.Combine(rutaEnvio, nombreArchivo), contenido);
                loge.FilePath = p_Log;
                loge.WriteMensaje("copiaXMLRINE " + " OK " + nombreArchivo);
                loge.FilePath = p_Log;
                loge.Linea();
            }
            catch (Exception e)
            {
                Logger.Logger loge = new Logger.Logger();
                loge.FilePath = p_Log;
                loge.WriteMensaje("copiaXMLRINE " + " ERROR " + e.Message + " " + nombreArchivo);
                loge.FilePath = p_Log;
                loge.Linea();
            }

            return "";
        }


        [WebMethod]
        public DataSet ConsInformacionSeguraUsuarioFS(string Semilla, string Ruc, string PI_Session, string pUsuario)
        {
            DataSet ds = new DataSet();
            clsClienteSeg objEjecFS = new clsClienteSeg();
            objEjecFS.SetSemilla(Semilla);
            ds = objEjecFS.ConsInformacionSeguraUsuarioFS(PI_Session, pUsuario, Ruc);
            return ds;
        }

        [WebMethod]
        public XmlDocument getActualizaContactoListBD(string Semilla, string ValorTokenUser, int IdOrganizacion, XmlDocument listaContacto, XmlDocument listaUsrAlmacen, int tipo,String codSap)
        {
            XmlDocument ds;
            clsClienteSeg objEjecucion = new clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            objEjecucion.IdOrganizacion = IdOrganizacion;
            objEjecucion.IdTransaccion = 105;
            objEjecucion.IdOpcion = 1;
            objEjecucion.ArrParams = new Object[4] {
                    listaContacto.OuterXml,
                    listaUsrAlmacen.OuterXml,
                    tipo,
                    codSap
                };

            ds = objEjecucion.EjecutaTransaccionXML(ValorTokenUser);

            return ds;
            //return "1000 | Texto de respuesta";
        }

        [WebMethod]
        public string getActualizaContactoList(XmlDocument listaContacto, XmlDocument listaUsrAlmacen, String codSap)
        {
            string aUX = "";
            List<DMSolcitudProveedor.SolProvContacto> listaContactos = new List<DMSolcitudProveedor.SolProvContacto>();
            List<DMSolcitudProveedor.SolProvContactoCiudad> listaUsrAlmacenes = new List<DMSolcitudProveedor.SolProvContactoCiudad>();
            DMSolcitudProveedor.SolProvContacto contacto;
            DMSolcitudProveedor.SolProvContactoCiudad usrAlmacen;
            try
            {
                XmlNodeList root = listaContacto.GetElementsByTagName("Root");
                XmlNodeList lista =
                    ((XmlElement)root[0]).GetElementsByTagName("Contacto");
                foreach (XmlElement nodo in lista)
                {

                    contacto = new DMSolcitudProveedor.SolProvContacto();
                    contacto.Apellido1 = nodo.GetAttribute("Apellido1");
                    contacto.Apellido2 = nodo.GetAttribute("Apellido2");
                    contacto.CodSapContacto = nodo.GetAttribute("CodSapContacto");
                    contacto.Departamento = nodo.GetAttribute("Departamento");
                    contacto.DepCliente = nodo.GetAttribute("DepCliente");
                    contacto.DescDepartamento = nodo.GetAttribute("DescDepartamento");
                    contacto.DescFuncion = nodo.GetAttribute("DescFuncion");
                    contacto.DescTipoIdentificacion = nodo.GetAttribute("DescTipoIdentificacion");
                    contacto.EMAIL = nodo.GetAttribute("EMAIL");
                    contacto.Estado = nodo.GetAttribute("Estado") == "1" ? true : false;
                    contacto.Funcion = nodo.GetAttribute("Funcion");
                    contacto.Identificacion = nodo.GetAttribute("Identificacion");
                    contacto.IdSolContacto = nodo.GetAttribute("IdSolContacto");
                    contacto.IdSolicitud = nodo.GetAttribute("IdSolicitud");
                    contacto.Nombre1 = nodo.GetAttribute("Nombre1");
                    contacto.Nombre2 = nodo.GetAttribute("Nombre2");
                    contacto.NotElectronica = nodo.GetAttribute("NotElectronica") == "1" ? true : false;
                    contacto.NotTransBancaria = nodo.GetAttribute("NotTransBancaria") == "1" ? true : false;
                    contacto.PreFijo = nodo.GetAttribute("PreFijo");
                    contacto.RepLegal = nodo.GetAttribute("RepLegal") == "1" ? true : false;
                    contacto.TelfFijo = nodo.GetAttribute("TelfFijo");
                    contacto.TelfFijoEXT = nodo.GetAttribute("TelfFijoEXT");
                    contacto.TelfMovil = nodo.GetAttribute("TelfMovil");
                    contacto.TipoIdentificacion = nodo.GetAttribute("TipoIdentificacion");
                    contacto.actas = nodo.GetAttribute("RecActas") == "1" ? "X" : "";

                    listaContactos.Add(contacto);

                }
                //Usuario - Almacenes
                XmlNodeList root2 = listaUsrAlmacen.GetElementsByTagName("Root");
                XmlNodeList lista2 =
                    ((XmlElement)root2[0]).GetElementsByTagName("UsrAlmacen");
                foreach (XmlElement nodo in lista2)
                {

                    usrAlmacen = new DMSolcitudProveedor.SolProvContactoCiudad();
                    usrAlmacen.codigoSap = codSap.ToString(); //nodo.GetAttribute("Apellido1");
                    usrAlmacen.Identificacion = nodo.GetAttribute("Identificacion");
                    usrAlmacen.codigoAlmacen = nodo.GetAttribute("CodAlmacen");
                    usrAlmacen.codigoPais = nodo.GetAttribute("CodPais");
                    usrAlmacen.codigoRegion = nodo.GetAttribute("CodRegion");
                    usrAlmacen.codigoCiudad = nodo.GetAttribute("CodCiudad");
                    listaUsrAlmacenes.Add(usrAlmacen);

                }


                var res = bapiupdate(listaContactos, listaUsrAlmacenes, codSap);
                //if (!res) 
                //if (res.Split("|"))
                aUX = res.ToString();
                //else
                //    aUX = "Error";

            }
            catch (Exception ex)
            {
                aUX = "10000|" + ex.Message.ToString();

            }
            return aUX;
        }


        private string bapiupdate(List<DMSolcitudProveedor.SolProvContacto> solicitudPar, List<DMSolcitudProveedor.SolProvContactoCiudad> solicitudCiudad, string codsap)
        {
            string retorno = "";
            Logger.Logger loge = new Logger.Logger();

            try
            {
                //   DMSolcitudProveedor solicitudPar = solicitudParlista[0];

                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;
                IRfcFunction fndatosmaestro;
                fndatosmaestro = repo.CreateFunction("ZPPPROVEEDORMOD");
                var DTPTPROV = fndatosmaestro.GetTable("PT_PROV");
                var DTPROVCONTACT = fndatosmaestro.GetTable("PT_PROVCONTACT");
                var DTPROVCONTACTCIUDAD = fndatosmaestro.GetTable("PT_PROVCONTACTCIUDAD");

                IRfcStructure ITPTPROV;
                IRfcStructure ITPROVCONTACT;
                IRfcStructure ITPROVCONTACTCIUDAD;
                //////////       PT_PROV

                //LIFNR    CHAR   10     ID de Proveedor     
                string AUX = "";
                //AUX = (Convert.ToInt64("119969") + 10000000000).ToString().Substring(1);
                //    AUX = (Convert.ToInt64(codsap) + 10000000000).ToString().Substring(1);

                try
                {
                    AUX = (Convert.ToInt64(codsap) + 10000000000).ToString().Substring(1);
                }
                catch (Exception)
                {

                    AUX = codsap;
                }


                ITPTPROV = repo.GetStructureMetadata("ZWAPPPROVEEDORES").CreateStructure();
                ITPTPROV.SetValue("LIFNR", AUX);
                ITPTPROV.SetValue("BUKRS", "");
                ITPTPROV.SetValue("STCD1", "");
                ITPTPROV.SetValue("KTOKK", "");
                ITPTPROV.SetValue("NAME1", "");
                ITPTPROV.SetValue("SORT1", "");
                ITPTPROV.SetValue("STRAS", "");
                ITPTPROV.SetValue("FLOOR", "");
                ITPTPROV.SetValue("STR_SUPPL3", "");
                ITPTPROV.SetValue("PSTLZ", "");
                ITPTPROV.SetValue("ORT01", "");
                ITPTPROV.SetValue("LAND1", "");
                ITPTPROV.SetValue("REGIO", "");
                ITPTPROV.SetValue("SPRAS", "");
                ITPTPROV.SetValue("TELF1", "");
                ITPTPROV.SetValue("TELF2", "");
                ITPTPROV.SetValue("TELFX", "");
                ITPTPROV.SetValue("SMTP_ADDR", "");
                ITPTPROV.SetValue("ZZDOCELEC", "");
                ITPTPROV.SetValue("CERDT", "");// DateTime.Now.ToString("yyyyMMdd"));
                ITPTPROV.SetValue("MINDK", "");
                ITPTPROV.SetValue("ZZIOPLN", "");
                ITPTPROV.SetValue("ZZIOPLA", "");
                ITPTPROV.SetValue("ZZSTCDAP", "");
                ITPTPROV.SetValue("CERDT", "");
                ITPTPROV.SetValue("STCDT", "");
                ITPTPROV.SetValue("ANRED", "");
                ITPTPROV.SetValue("AKONT", "");
                ITPTPROV.SetValue("ZZMAILAP", "");
                ITPTPROV.SetValue("EKORG", "");
                ITPTPROV.SetValue("BEGRU", "");
                ITPTPROV.SetValue("FITYP", "");
                ITPTPROV.SetValue("FDGRV", "");
                ITPTPROV.SetValue("ZTERM", "");
                ITPTPROV.SetValue("ZWELS", "");
                ITPTPROV.SetValue("BRSCH", "");
                ITPTPROV.SetValue("NO_MOD", "X");
                DTPTPROV.Append(ITPTPROV);

                if (solicitudPar != null)
                {

                    foreach (var contacto in solicitudPar)
                    {
                        ITPROVCONTACT = repo.GetStructureMetadata("ZWAPPPROVCONTACT").CreateStructure();

                        // LIFNR  CHAR   10     ID de Proveedor            X
                        ITPROVCONTACT.SetValue("LIFNR", AUX);
                        //PARNR   NUMC   10     Número de la persona de contacto        X
                        ITPROVCONTACT.SetValue("PARNR", contacto.CodSapContacto);
                        //ANRED   CHAR   30     Tratamiento de la persona de contacto          X
                        ITPROVCONTACT.SetValue("ANRED", contacto.PreFijo);
                        //NAMEV   CHAR   35     Nombre de pila             X
                        ITPROVCONTACT.SetValue("NAMEV", ((!string.IsNullOrEmpty(contacto.Nombre1) ? " " + contacto.Nombre1 : "") + " " + (!string.IsNullOrEmpty(contacto.Nombre2) ? " " + contacto.Nombre2 : "")).Trim());
                        //NAME1   CHAR   35     Nombre 1     X      X
                        ITPROVCONTACT.SetValue("NAME1", ((!string.IsNullOrEmpty(contacto.Apellido1) ? " " + contacto.Apellido1 : "") + " " + (!string.IsNullOrEmpty(contacto.Apellido2) ? " " + contacto.Apellido2 : "")).Trim());
                        //ABTPA   CHAR   12     Departamento de persona contacto en cliente           X
                        ITPROVCONTACT.SetValue("ABTPA", contacto.DepCliente);
                        //ABTNR   CHAR   4      Departamento de persona contacto        X
                        ITPROVCONTACT.SetValue("ABTNR", contacto.Departamento);
                        //PAFKT   CHAR   2      Función de la persona de contacto       X
                        ITPROVCONTACT.SetValue("PAFKT", contacto.Funcion);
                        //TELF1   CHAR   16     1º número de teléfono             X
                        ITPROVCONTACT.SetValue("TELF1", contacto.TelfMovil);
                        //TEL_NUMBER    CHAR   30     Número teléfono: Prefijo+número         X
                        //ITPROVCONTACT.SetValue("TEL_NUMBER", contacto.TelfFijo + " " + contacto.TelfFijoEXT);
                        ITPROVCONTACT.SetValue("TEL_NUMBER", contacto.TelfFijo);
                        //SMTP_ADDR     CHAR   241    Dirección de correo electrónico   X      X
                        ITPROVCONTACT.SetValue("SMTP_ADDR", contacto.EMAIL);
                        //J_1ATODC      CHAR   2      Tipo de número de identificación fiscal        
                        ITPROVCONTACT.SetValue("J_1ATODC", contacto.TipoIdentificacion);
                        //IDNUM   CHAR   30     Identity Number            
                        ITPROVCONTACT.SetValue("IDNUM", contacto.Identificacion);
                        if (contacto.RepLegal == false)
                        {
                            ITPROVCONTACT.SetValue("PARH1", "");
                        }
                        else
                        {
                            ITPROVCONTACT.SetValue("PARH1", "1");
                        }
                        if (contacto.NotElectronica == false)
                        {
                            ITPROVCONTACT.SetValue("PARH2", "");
                        }
                        if (contacto.NotTransBancaria == false)
                        {
                            ITPROVCONTACT.SetValue("PARH3", "");
                        }
                        if (contacto.Estado == false)
                        {
                            ITPROVCONTACT.SetValue("PARH4", "I");
                        }
                        else
                        {
                            ITPROVCONTACT.SetValue("PARH4", "A");
                        }
                        if (contacto.actas == "" || contacto.actas == null)
                        {
                            ITPROVCONTACT.SetValue("PARH5", "");
                        }
                        else
                        {
                            ITPROVCONTACT.SetValue("PARH5", "X");
                        }
                        //ACTION  CHAR   1      Acción 
                        // ITPROVCONTACT.SetValue("ACTION", !contacto.Estado ? "X" : "");
                        DTPROVCONTACT.Append(ITPROVCONTACT);
                    }

                }

                if (solicitudCiudad != null)
                {
                    foreach (var contacto in solicitudCiudad)
                    {
                        ITPROVCONTACTCIUDAD = repo.GetStructureMetadata("ZWAPPCONTACTOCIUDAD").CreateStructure();

                        // LIFNR  CHAR   10     ID de Proveedor            X
                        ITPROVCONTACTCIUDAD.SetValue("LIFNR", AUX);
                        //PARNR   NUMC   10     Número de la persona de contacto        X
                        ITPROVCONTACTCIUDAD.SetValue("IDNUM", contacto.Identificacion);
                        //ANRED   CHAR   30     Tratamiento de la persona de contacto          X
                        ITPROVCONTACTCIUDAD.SetValue("WERKS", contacto.codigoAlmacen);
                        //NAMEV   CHAR   35     Nombre de pila             X
                        ITPROVCONTACTCIUDAD.SetValue("NAME1", "");
                        //NAME1   CHAR   35     Nombre 1     X      X
                        ITPROVCONTACTCIUDAD.SetValue("LAND1", contacto.codigoPais);
                        //ABTPA   CHAR   12     Departamento de persona contacto en cliente           X
                        ITPROVCONTACTCIUDAD.SetValue("BLAND", contacto.codigoRegion);
                        //ABTNR   CHAR   4      Departamento de persona contacto        X
                        ITPROVCONTACTCIUDAD.SetValue("CITY_CODE", contacto.codigoCiudad);
                        //PAFKT   CHAR   2      Función de la persona de contacto       X
                        ITPROVCONTACTCIUDAD.SetValue("CITY_NAME", "");
                        //TELF1   CHAR   16     1º número de teléfono             X
                        ITPROVCONTACTCIUDAD.SetValue("MC_CITY", "");
                        //TEL_NUMBER    CHAR   30     Número teléfono: Prefijo+número         X
                        //ITPROVCONTACT.SetValue("TEL_NUMBER", contacto.TelfFijo + " " + contacto.TelfFijoEXT);
                        ITPROVCONTACTCIUDAD.SetValue("ZONE1", "");
                        //SMTP_ADDR     CHAR   241    Dirección de correo electrónico   X      X
                        ITPROVCONTACTCIUDAD.SetValue("VTEXT", "");

                        //ACTION  CHAR   1      Acción 
                        // ITPROVCONTACT.SetValue("ACTION", !contacto.Estado ? "X" : "");
                        DTPROVCONTACTCIUDAD.Append(ITPROVCONTACTCIUDAD);
                    }
                }


                fndatosmaestro.SetValue("PT_PROV", DTPTPROV);
                fndatosmaestro.SetValue("PT_PROVCONTACT", DTPROVCONTACT);
                fndatosmaestro.SetValue("PT_PROVCONTACTCIUDAD", DTPROVCONTACTCIUDAD);

                //PT_GRUPAUT catalogo para BEGRU
                fndatosmaestro.Invoke(AppConfig.dest);

                var log = fndatosmaestro.GetTable("PT_LOG");
                var CODERROR = fndatosmaestro.GetString("CODERROR");
                var DESERROR = fndatosmaestro.GetString("DESERROR");
                var PT_PROV = fndatosmaestro.GetTable("PT_PROV");
                var PT_PROVCONTACT = fndatosmaestro.GetTable("PT_PROVCONTACT");
                var PT_PROVCONTACTCIUDAD = fndatosmaestro.GetTable("PT_PROVCONTACTCIUDAD");
                if (CODERROR != "0")
                {
                    var result = (from a in log
                                  select new
                                  {
                                      MESSAGE = a.GetString("MESSAGE"),
                                      NUMBER = a.GetString("NUMBER"),
                                      ID = a.GetString("ID"),
                                  }).ToList();
                    var msj = "";
                    foreach (var m in result)
                    {
                        msj = msj + m.NUMBER + "-" + m.MESSAGE + "<br/>";
                    }
                    if (msj != "")
                    {
                        retorno = msj;
                        //loge.FilePath = p_Log;
                        //loge.WriteMensaje(codsap);
                        //loge.FilePath = p_Log;
                        //loge.WriteMensaje(msj);
                        //loge.FilePath = p_Log;
                        //loge.Linea();
                    }


                }
                //var PT_PROVBANK = fndatosmaestro.GetTable("PT_PROVBANK");
                //var PT_RETENCION = fndatosmaestro.GetTable("PT_RETENCION");
            }
            catch (Exception ex)
            {
                retorno = "1000|Error de comunicacion con el Sistema: " + ex.Message;
            }
            return retorno;
        }



        [WebMethod]
        public List<DMSolcitudProveedor.SolProvContacto> getProveedorContactoList(string Semilla, string ValorTokenUser, int IdOrganizacion, string idproveedorconta)
        {
            Logger.Logger log = new Logger.Logger();
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();

            DataSet ds = new DataSet();
            clsClienteSeg objEjecucion = new clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            objEjecucion.IdOrganizacion = IdOrganizacion;
            objEjecucion.IdTransaccion = 105;
            objEjecucion.IdOpcion = 1;
            objEjecucion.ArrParams = new Object[4] {
                "<Root />",
                "<Root />",
                2,
                idproveedorconta
            };

            ds = objEjecucion.EjecutaTransaccionDS(ValorTokenUser);

            List<DMSolcitudProveedor.SolProvContacto> Retorno = new List<DMSolcitudProveedor.SolProvContacto>();
            try
            {
                List<DMSolcitudProveedor.SolProvContacto> re = new List<DMSolcitudProveedor.SolProvContacto>();
                DMSolcitudProveedor.SolProvContacto aux;
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    aux = new DMSolcitudProveedor.SolProvContacto();
                    aux.CodSapContacto = Convert.ToString(row["CodProveedor"]);
                    aux.PreFijo = Convert.ToString(row["Prefijo"]);
                    aux.Nombre1 = Convert.ToString(row["Nombre1"]);
                    aux.Nombre2 = Convert.ToString(row["Nombre2"]);
                    aux.Apellido1 = Convert.ToString(row["Apellido1"]);
                    aux.Apellido2 = Convert.ToString(row["Apellido2"]);
                    aux.Departamento = Convert.ToString(row["CodDepartamento"]);
                    aux.Funcion = Convert.ToString(row["CodFuncion"]);
                    aux.TelfMovil = Convert.ToString(row["TelfMovil"]);
                    aux.TelfFijo = Convert.ToString(row["TelfFijo"]);
                    aux.EMAIL = Convert.ToString(row["email"]);
                    aux.TipoIdentificacion = Convert.ToString(row["TipoIdentificacion"]);
                    aux.Identificacion = Convert.ToString(row["Identificacion"]);
                    aux.DepCliente = "";
                    if (Convert.ToString(row["RepLegal"]) == "1")
                        aux.RepLegal = true;
                    else
                        aux.RepLegal = false;

                    if (Convert.ToString(row["NotElectronica"]) == "1")
                        aux.NotElectronica = true;
                    else
                        aux.NotElectronica = false;

                    if (Convert.ToString(row["NotTransBancaria"]) == "1")
                        aux.NotTransBancaria = true;
                    else
                        aux.NotTransBancaria = false;

                    if (Convert.ToString(row["Estado"]) == "1")
                        aux.Estado = true;
                    else
                        aux.Estado = false;

                    if (Convert.ToString(row["RecActas"]) == "1")
                        aux.actas = "X";
                    else
                        aux.actas = "";

                    re.Add(aux);

                }

                Retorno = re.ToList();

            }
            catch (Exception ex)
            {
                log.FilePath = p_Log;
                log.WriteMensaje("Error [getProveedorContactoList]: " + ex.Message);
                throw new DataException(ex.Message);
            }
            return Retorno;
        }
        [WebMethod]
        public List<DMSolcitudProveedor.SolProvContactoCiudad> getProveedorContactoListCiudad(string Semilla, string ValorTokenUser, int IdOrganizacion, string idproveedorconta) 
        {
            Logger.Logger log = new Logger.Logger();
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();

            DataSet ds = new DataSet();
            clsClienteSeg objEjecucion = new clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            objEjecucion.IdOrganizacion = IdOrganizacion;
            objEjecucion.IdTransaccion = 105;
            objEjecucion.IdOpcion = 1;
            objEjecucion.ArrParams = new Object[4] {
                "<Root />",
                "<Root />",
                3,
                idproveedorconta
            };

            ds = objEjecucion.EjecutaTransaccionDS(ValorTokenUser);

            List<DMSolcitudProveedor.SolProvContactoCiudad> Retorno = new List<DMSolcitudProveedor.SolProvContactoCiudad>();
            try
            {
                List<DMSolcitudProveedor.SolProvContactoCiudad> reCiudad = new List<DMSolcitudProveedor.SolProvContactoCiudad>();
                DMSolcitudProveedor.SolProvContactoCiudad aux;

                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    aux = new DMSolcitudProveedor.SolProvContactoCiudad();
                    aux.codigoSap = Convert.ToString(row["CodProveedor"]); ;
                    aux.Identificacion = Convert.ToString(row["Identificacion"]);
                    aux.codigoAlmacen = Convert.ToString(row["CodAlmacen"]);
                    aux.codigoCiudad = Convert.ToString(row["CodCiudad"]);
                    aux.codigoPais = Convert.ToString(row["CodPais"]);
                    aux.codigoRegion = Convert.ToString(row["CodRegion"]);
                    reCiudad.Add(aux);
                }

                Retorno = reCiudad;
            }
            catch (Exception ex)
            {
                log.FilePath = p_Log;
                log.WriteMensaje("Error [getProveedorContactoList]: " + ex.Message);
                throw new DataException(ex.Message);
            }
            return Retorno;
        }
        [WebMethod]
        public List<DMSolcitudProveedor.SolProvDireccion> getProveedorDireccionList(String idproveedordir)
        {
            string aUX = "";
            List<DMSolcitudProveedor.SolProvDireccion> Retorno = new List<DMSolcitudProveedor.SolProvDireccion>();
            try
            {
                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;
                IRfcFunction fndatosmaestro = repo.CreateFunction("ZPPPROVEEDORCHECK");
                var DTPRLIST = fndatosmaestro.GetTable("P_PRLIST");
                IRfcStructure ITPTPROV;
                ITPTPROV = repo.GetStructureMetadata("ZWAPPPROVLISTA").CreateStructure();
                //ITPTPROV.SetValue("LIFNR", idproveedor);
                //aUX = (Convert.ToInt64(idproveedordir) + 10000000000).ToString().Substring(1);

                try
                {
                    aUX = (Convert.ToInt64(idproveedordir) + 10000000000).ToString().Substring(1);
                }
                catch (Exception)
                {

                    aUX = idproveedordir;
                }
                ITPTPROV.SetValue("LIFNR", aUX);



                DTPRLIST.Append(ITPTPROV);
                fndatosmaestro.SetValue("P_PRLIST", DTPRLIST);

                fndatosmaestro.Invoke(AppConfig.dest);


                var CODERROR = fndatosmaestro.GetString("CODERROR");
                var DESERROR = fndatosmaestro.GetString("DESERROR");

                if (CODERROR != "")
                {
                    throw new DataException(DESERROR.ToString());

                }
                else
                {
                    var PT_PROV = fndatosmaestro.GetTable("PT_PROV");


                    List<DMSolcitudProveedor.SolProvDireccion> re = (from e in PT_PROV
                                                                     select new DMSolcitudProveedor.SolProvDireccion
                                                                     {

                                                                         CallePrincipal = e.GetString("STRAS"),
                                                                         Solar = e.GetString("STRAS"),
                                                                         PisoEdificio = e.GetString("FLOOR"),
                                                                         CalleSecundaria = e.GetString("STR_SUPPL3"),
                                                                         CodPostal = e.GetString("PSTLZ"),
                                                                         Ciudad = String.IsNullOrEmpty(e.GetString("REGIO")) ? "" : e.GetString("REGIO") + "-" + e.GetString("ORT01"),
                                                                         Provincia = String.IsNullOrEmpty(e.GetString("LAND1")) ? "" : e.GetString("LAND1") + "-" + e.GetString("REGIO"),
                                                                         Pais = e.GetString("LAND1"),
                                                                         DescRegion = e.GetString("REGIO"),
                                                                     }).ToList();

                    Retorno = re.ToList();

                }








            }
            catch (Exception ex)
            {

                throw new DataException(ex.Message);
            }
            return Retorno;
        }
        [WebMethod]
        public List<DMSolcitudProveedor.SolProveedor> ConsultaProveedorBapi(String idproveedor)
        {
            string aUX = "";
            List<DMSolcitudProveedor.SolProveedor> Retorno = new List<DMSolcitudProveedor.SolProveedor>();
            try
            {
                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;
                IRfcFunction fndatosmaestro = repo.CreateFunction("ZPPPROVEEDORCHECK");
                var DTPRLIST = fndatosmaestro.GetTable("P_PRLIST");
                IRfcStructure ITPTPROV;
                ITPTPROV = repo.GetStructureMetadata("ZWAPPPROVLISTA").CreateStructure();
                //ITPTPROV.SetValue("LIFNR", idproveedor);
                try
                {
                    aUX = (Convert.ToInt64(idproveedor) + 10000000000).ToString().Substring(1);
                }
                catch (Exception e)
                {
                    aUX = idproveedor;
                }

                ITPTPROV.SetValue("LIFNR", aUX);
                ITPTPROV.SetValue("BUKRS", "1001");


                DTPRLIST.Append(ITPTPROV);
                fndatosmaestro.SetValue("P_PRLIST", DTPRLIST);

                fndatosmaestro.Invoke(AppConfig.dest);


                var CODERROR = fndatosmaestro.GetString("CODERROR");
                var DESERROR = fndatosmaestro.GetString("DESERROR");

                if (CODERROR != "")
                {
                    throw new DataException(DESERROR.ToString());

                }
                else
                {
                    var PT_PROV = fndatosmaestro.GetTable("PT_PROV");
                    var PT_RET = fndatosmaestro.GetTable("PT_RETENCION");
                    var a1 = "";
                    var a2 = "";
                    var a3 = "";
                    var a4 = "";
                    var reT = (from e in PT_RET
                               select new
                               {
                                   RetencionFuente = e.GetString("WITHT"),
                                   RetencionFuente2 = e.GetString("WT_WITHCD"),
                                   RetencionIva = e.GetString("WITHT"),
                                   RetencionIva2 = e.GetString("WT_WITHCD"),
                               }).ToList();

                    if (reT.Count > 0)
                    {
                        a1 = reT[0].RetencionFuente;
                        a2 = reT[0].RetencionFuente2;
                        a3 = reT[1].RetencionIva;
                        a4 = reT[1].RetencionIva2;
                    }


                    List<DMSolcitudProveedor.SolProveedor> re = (from e in PT_PROV
                                                                 select new DMSolcitudProveedor.SolProveedor
                                                                 {
                                                                     SectorComercial = e.GetString("MINDK"),
                                                                     //Aprobacion = e.GetString(""),
                                                                     Autorizacion = e.GetString("BEGRU"),
                                                                     ClaseContribuyente = e.GetString("FITYP"),
                                                                     //CodGrupoProveedor = e.GetString(""),
                                                                     CodSapProveedor = e.GetString("LIFNR"),
                                                                     CondicionPago = e.GetString("ZTERM"),
                                                                     CuentaAsociada = e.GetString("AKONT"),
                                                                     //DepSolicitando = e.GetString(""),
                                                                     EMAILCorp = e.GetString("SMTP_ADDR"),
                                                                     //EMAILSRI = e.GetString(""),
                                                                     GenDocElec = e.GetString("ZZDOCELEC"),
                                                                     GrupoCuenta = e.GetString("KTOKK"),
                                                                     GrupoTesoreria = e.GetString("FDGRV"),
                                                                     IdEmpresa = "1",
                                                                     Identificacion = e.GetString("STCD1"),
                                                                     Idioma = e.GetString("SPRAS"),

                                                                     //LineaNegocio = e.GetString(""),
                                                                     NomComercial = e.GetString("SORT1"),
                                                                     PlazoEntrega = e.GetString("PLIFZ"),

                                                                     RazonSocial = e.GetString("NAME1") + e.GetString("NAME2") + e.GetString("NAME3") + e.GetString("NAME4"),

                                                                     RetencionFuente = a1,
                                                                     RetencionFuente2 = a2,
                                                                     RetencionIva = a3,
                                                                     RetencionIva2 = a4,
                                                                     TelfFax = e.GetString("TELFX"),
                                                                     //TelfFaxEXT = e.GetString(""),
                                                                     TelfFijo = e.GetString("TELF1"),

                                                                     TelfFijoEXT = e.GetString("TELF1"),

                                                                     //TelfFijoEXT = e.GetString(""),
                                                                     TelfMovil = e.GetString("TELF2"),
                                                                     TipoIdentificacion = e.GetString("STCDT"),
                                                                     TipoProveedor = e.GetString("KTOKK"),
                                                                     LineaNegocio = e.GetString("EKGRP"),
                                                                     Ramo = e.GetString("BRSCH"),
                                                                     GrupoEsquema = e.GetString("KALSK"),
                                                                     ListaViasPago = e.GetString("ZWELS"),


                                                                 }).ToList();



                    Retorno = re.ToList();

                }
            }
            catch (Exception ex)
            {

                throw new DataException(ex.Message);
            }
            return Retorno;
        }


        [WebMethod]
        public DataSet GetConsValUsrFirstLogon(string Semilla, int IdOrganizacion, string ValorTokenUser, XmlDocument xmlParam)
        {
            DataSet ds = new DataSet();
            clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            objEjecucion.IdOrganizacion = IdOrganizacion;
            objEjecucion.IdTransaccion = 9;
            objEjecucion.IdOpcion = 1;
            objEjecucion.ArrParams = new Object[1] {
                    xmlParam.OuterXml
                };
            ds = objEjecucion.EjecutaTransaccionDS(ValorTokenUser);
            return ds;
        }
        [WebMethod]
        public string GrabaUsuarioAdministrador(string Semilla, string ValorTokenUser, XmlDocument xmlParam)
        {
            XmlDocument xmlResp = new System.Xml.XmlDocument();
            clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            xmlResp = objEjecucion.GrabaUsuarioAdministrador(ValorTokenUser, xmlParam.OuterXml);
            return xmlResp.OuterXml;
        }
        [WebMethod]
        public string GrabaActivacionNuevoUsuario(string Semilla, string ValorTokenUser, XmlDocument xmlParam)
        {
            XmlDocument xmlResp = new System.Xml.XmlDocument();
            clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            xmlResp = objEjecucion.GrabaActivacionNuevoUsuario(ValorTokenUser, xmlParam.OuterXml);
            return xmlResp.OuterXml;
        }
        [WebMethod]
        public DataSet GetRecuperaClaveValidar(string Semilla, int IdOrganizacion, string PI_Session, XmlDocument xmlParam)
        {
            DataSet ds = new DataSet();
            clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            objEjecucion.IdOrganizacion = IdOrganizacion;
            objEjecucion.IdTransaccion = 12;
            objEjecucion.IdOpcion = 1;
            objEjecucion.ArrParams = new Object[1] {
                    xmlParam.OuterXml
                };
            ds = objEjecucion.EjecutaTransaccionDS(PI_Session);
            return ds;
        }
        [WebMethod]
        public string CambiarClaveRecupera(string Semilla, string ValorTokenUser, string Usuario, string Clave, string Ruc)
        {
            XmlDocument xmlResp = new System.Xml.XmlDocument();
            clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            xmlResp = objEjecucion.CambiarClaveRecupera(ValorTokenUser, Usuario, Clave, Ruc);
            return xmlResp.OuterXml;
        }
        [WebMethod]
        public string CambiarClave(string Semilla, string ValorTokenUser, string Usuario, string ClaveAct, string ClaveNew, string Ruc)
        {
            XmlDocument xmlResp = new System.Xml.XmlDocument();
            clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            xmlResp = objEjecucion.CambiarClave(ValorTokenUser, Usuario, ClaveAct, ClaveNew, Ruc);
            return xmlResp.OuterXml;
        }
        [WebMethod]
        public string DesbloquearClave(string Semilla, string ValorTokenUser, string Usuario, Boolean bandera, string Clave, string Ruc)
        {
            XmlDocument xmlResp = new System.Xml.XmlDocument();
            clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            xmlResp = objEjecucion.DesbloquearClave(ValorTokenUser, Usuario, bandera, Clave, Ruc);
            return xmlResp.OuterXml;
        }
        [WebMethod]
        public string GrabaUsuarioAdicional(string Semilla, string ValorTokenUser, string xmlParam)
        {
            XmlDocument xmlResp = new System.Xml.XmlDocument();
            clsClienteSeg objEjecucion = new clibSeguridadCR.clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            xmlResp = objEjecucion.GrabaUsuarioAdicional(ValorTokenUser, xmlParam);
            return xmlResp.OuterXml;
        }
        [WebMethod]
        public DataSet ConsRolesPorOrgFS(string Semilla, string ValorTokenUser, int IdOrganizacion)
        {
            DataSet ds;
            clsClienteSeg objEjecucion = new clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            ds = objEjecucion.ConsRolesPorOrgFS(ValorTokenUser, IdOrganizacion.ToString());
            return ds;
        }

        [WebMethod]
        public DataSet GetConsTodasZonas(string Semilla, int IdOrganizacion, string ValorTokenUser)
        {
            DataSet ds;
            Object[] arrParam = new Object[1];
            arrParam[0] = "";
            clsClienteSeg objEjecucion = new clsClienteSeg();
            objEjecucion.SetSemilla(Semilla);
            objEjecucion.IdOrganizacion = IdOrganizacion;
            objEjecucion.IdTransaccion = 11;
            objEjecucion.ArrParams = new object[0];
            objEjecucion.IdOpcion = 1;
            objEjecucion.ArrParams = arrParam;

            ds = objEjecucion.EjecutaTransaccionDS(ValorTokenUser);
            return ds;
        }
        [WebMethod]
        public Boolean IsPermisoUserTransOpcion(string Semilla, int IdOrganizacion, int IdTransaccion, int IdOpcion, string PI_ParamObjList, string ValorTokenUser)
        {
            Boolean retorno = false;
            string xml = "";
            XmlDocument tmp = new XmlDocument();
            clibSeguridadCR.clsClienteSeg objseg = new clsClienteSeg();
            tmp = objseg.IsPermisoUserTransOpcion(ValorTokenUser, IdOrganizacion, IdTransaccion, IdOpcion, PI_ParamObjList);
            xml = tmp.OuterXml;
            XmlDocument xmlResp = new XmlDocument();
            xmlResp.LoadXml(xml);

            if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
            {
                if (xmlResp.DocumentElement.GetAttribute("Permiso") == "S")
                {
                    retorno = true;
                }
                else
                {
                    retorno = false;
                }

            }
            else
            {
                retorno = false;
            }

            return retorno;
        }

        [WebMethod]
        public DataSet DatosBase(string Semilla, int IdOrganizacion, int IdTransaccion, int IdOpcion, Object[] PI_ParamObjList, string ValorTokenUser)
        {
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            DataSet ds = new DataSet();
            try
            {
                Logger.Logger loge = new Logger.Logger();
                //Object[] PI_ParamObjListL = new Object[1];
                //String[] idRepo = PI_ParamObjList.Split(',');
                //PI_ParamObjListL[0] = idRepo[0];
                loge.FilePath = p_Log;
                loge.WriteMensaje("Semilla: "+ Semilla+ " IdOrganizacion: "+ IdOrganizacion+ " IdTransaccion: "+ IdOpcion+ " PI_ParamObjList: "+ PI_ParamObjList.Length+ " ValorTokenUser: "+ ValorTokenUser);
                loge.Linea();

                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(Semilla);
                objSeg.IdOrganizacion = IdOrganizacion;
                objSeg.IdTransaccion = IdTransaccion;
                objSeg.IdOpcion = IdOpcion;
                //objSeg.ArrParams = new Object[1] {
                //    PI_ParamXML
                //};

                objSeg.ArrParams = PI_ParamObjList;
                loge.FilePath = p_Log;
                loge.WriteMensaje("DatosBase antes de ejecutar transaccion");
                loge.Linea();
                //ds = objSeg.EjecutaTransaccionDS(InitialiseService.PI_Session);
                ds = objSeg.EjecutaTransaccionDS(ValorTokenUser);

                StringWriter sw = new StringWriter();
                ds.WriteXml(sw);
                string result = sw.ToString();

                loge.FilePath = p_Log;
                loge.WriteMensaje("DatosBase respuesta EjecutaTransaccionDS: "+ result);
                loge.Linea();
            }
            catch (Exception e)
            {
                Logger.Logger loge = new Logger.Logger();
                loge.FilePath = p_Log;
                loge.WriteMensaje("DatosBase " + " ERROR " + e.Message );
                loge.FilePath = p_Log;
                loge.Linea();
            }
            return ds;
        }

        [WebMethod]
        public DataSet ConsRolesPorUsuarioFS(string Semilla, string ValorTokenUser, string Usuario, string Ruc)
        {
            DataSet ds;
            clsClienteSeg objEjecRol = new clsClienteSeg();
            objEjecRol.SetSemilla(Semilla);
            ds = objEjecRol.ConsRolesPorUsuarioFS(ValorTokenUser, Usuario, Ruc);
            return ds;
        }

        [WebMethod]
        public DataSet ConsRolesPorListaUsuariosFS(string ValorTokenUser, string[] usuarios, string Ruc)
        {
            DataSet ds;
            clibSeguridadCR.clsClienteSeg objEjecucionFS = new clibSeguridadCR.clsClienteSeg();
            ds = objEjecucionFS.ConsRolesPorListaUsuariosFS(ValorTokenUser, usuarios, Ruc);

            return ds;
        }

        [WebMethod]
        public string LoginSesionUsuario(string Semilla, string PI_Session, string sourceLogin, string PI_ParamXML)
        {
            string gl_SemillaEncryp = string.Empty;
            clibSeguridadCR.clsClienteSeg objCliSeg = new clibSeguridadCR.clsClienteSeg();

            System.Xml.XmlDocument xmlResp = new System.Xml.XmlDocument();
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
            objSeg.SetSemilla(Semilla);

            try
            {
                if (sourceLogin == "4")
                    xmlResp = objSeg.RegistraUserEmpleado(PI_Session, PI_ParamXML);
                else
                    xmlResp = objSeg.RegistraUser(PI_Session, PI_ParamXML);

            }
            catch (Exception ex)
            {
                xmlResp = clibSeguridadCR.clsClienteSeg.getXmlEstado(-1, ex.Message);
            }
            return xmlResp.OuterXml;
        }

        [WebMethod]
        public DataSet GetCatalogosFS(string PI_Session, int IdTblCatalogo)
        {
            DataSet ds;
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
            ds = objSeg.ConsCatalogoFS(PI_Session, IdTblCatalogo.ToString());
            return ds;
        }
        [WebMethod]
        public List<cls_Stock> BuscarDatosStock(XmlDocument datos, int metriales, int almacen)
        {

            FormResponse Retorno = new FormResponse();
            List<cls_Stock> stockRetorno = new List<cls_Stock>();
            Logger.Logger loge = new Logger.Logger();
            string UsuarioPRD = ((string)System.Configuration.ConfigurationManager.AppSettings["UsuarioPRDPI"]).Trim();
            string ClavePRD = ((string)System.Configuration.ConfigurationManager.AppSettings["ClavePRDPI"]).Trim();
            string RutaCitas = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLogCitas"]).Trim();
            string fecha = "";
            string codSap = "";
            try
            {
                wsReporteStock.SI_ConsultarStock_OUT_SYNCService _envio = new wsReporteStock.SI_ConsultarStock_OUT_SYNCService();
                wsReporteStock.DT_ConsultarStock_Request _parametros = new wsReporteStock.DT_ConsultarStock_Request();
                wsReporteStock.DT_ConsultarStock_Response _salida = new wsReporteStock.DT_ConsultarStock_Response();
                wsReporteStock.DT_ConsultarStock_RequestITEM[] _materiales = new wsReporteStock.DT_ConsultarStock_RequestITEM[metriales];
                wsReporteStock.DT_ConsultarStock_RequestITEM1[] _almaces = new wsReporteStock.DT_ConsultarStock_RequestITEM1[almacen];
                int i = 0;
                foreach (XmlNode Elementot in datos.DocumentElement.SelectNodes("//Articulo"))
                {
                    XmlDocument tmpd = new XmlDocument();
                    tmpd.LoadXml(Elementot.OuterXml);
                    XmlNode Elemento = tmpd.DocumentElement;
                    wsReporteStock.DT_ConsultarStock_RequestITEM _materialesItem = new wsReporteStock.DT_ConsultarStock_RequestITEM();
                    _materialesItem.MATNR = Elemento.SelectSingleNode("//Articulo/@CodArticulo").Value;
                    _materiales[i] = _materialesItem;
                    i++;
                }
                i = 0;
                foreach (XmlNode Elementot in datos.DocumentElement.SelectNodes("//Almacen"))
                {
                    XmlDocument tmpd = new XmlDocument();
                    tmpd.LoadXml(Elementot.OuterXml);
                    XmlNode Elemento = tmpd.DocumentElement;
                    wsReporteStock.DT_ConsultarStock_RequestITEM1 _almacesItem = new wsReporteStock.DT_ConsultarStock_RequestITEM1();
                    _almacesItem.WERKS = Elemento.SelectSingleNode("//Almacen/@CodAlmacen").Value;
                    _almaces[i] = _almacesItem;
                    i++;
                }
                fecha = datos.DocumentElement.SelectSingleNode("//@FechaDesde").Value;
                codSap = datos.DocumentElement.SelectSingleNode("//@CodSAP").Value;
                _parametros.P_MATNR = _materiales;
                _parametros.P_WERKS = _almaces;
                _parametros.PS_LIFNR = codSap;
                _parametros.PS_DATE = fecha;
                _envio.Credentials = new System.Net.NetworkCredential(UsuarioPRD, ClavePRD);
                _envio.Timeout = 1200000;
                _salida = _envio.SI_ConsultarStock_OUT_SYNC(_parametros);
                wsReporteStock.DT_ConsultarStock_ResponseITEM[] _salidaStock = _salida.PT_STOCK;



                foreach (var item in _salidaStock)
                {
                    cls_Stock tmp = new cls_Stock();
                    tmp.BASE_UOM = item.BASE_UOM;
                    tmp.DATE = item.DATE;
                    tmp.LIFNR = item.LIFNR;
                    tmp.MATNR = item.MATNR;
                    tmp.STOCK = item.STOCK;
                    tmp.WERKS = item.WERKS;
                    tmp.ZIDNLF = item.ZIDNLF;
                    stockRetorno.Add(tmp);
                }
                Retorno.root.Add(stockRetorno);
            }
            catch (Exception ex)
            {

            }

            return stockRetorno;
        }

        [WebMethod]
        public DataSet ConsultaBandejaUsuariosAdministradores(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds;
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
            objSeg.SetSemilla(PI_Semilla);
            objSeg.IdOrganizacion = 39;
            objSeg.IdTransaccion = 2;
            objSeg.IdOpcion = 1;
            objSeg.ArrParams = new Object[1] {
                    PI_XmlDoc.OuterXml
                };
            ds = objSeg.EjecutaTransaccionDS(PI_Session);
            return ds;
        }
        //Agregado el 16-01-2016 por J. Navarrete
        [WebMethod]
        public DataSet ConsultaDatosLegacyAsociados(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds;
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
            objSeg.SetSemilla(PI_Semilla);
            objSeg.IdOrganizacion = 39;
            objSeg.IdTransaccion = 13; //Consulta de Datos Legacy Asociados
            objSeg.IdOpcion = 1;
            objSeg.ArrParams = new Object[1] {
                    PI_XmlDoc.OuterXml
                };
            ds = objSeg.EjecutaTransaccionDS(PI_Session);
            return ds;
        }
        //Agregado el 16-01-2016 por J. Navarrete
        [WebMethod]
        public DataSet ActualizaDatosLegacyAsociados(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds;
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
            objSeg.SetSemilla(PI_Semilla);
            objSeg.IdOrganizacion = 39;
            objSeg.IdTransaccion = 14; //Actualiza Datos Legacy Asociados
            objSeg.IdOpcion = 1;
            objSeg.ArrParams = new Object[1] {
                    PI_XmlDoc.OuterXml
                };
            ds = objSeg.EjecutaTransaccionDS(PI_Session);
            return ds;
        }
        [WebMethod]
        public DataSet ConsultaBandejaUsuariosAdicionales(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds;
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
            objSeg.SetSemilla(PI_Semilla);
            objSeg.IdOrganizacion = 39;
            objSeg.IdTransaccion = 3;
            objSeg.IdOpcion = 1;
            objSeg.ArrParams = new Object[1] {
                    PI_XmlDoc.OuterXml
                };
            ds = objSeg.EjecutaTransaccionDS(PI_Session);
            return ds;
        }

        [WebMethod]
        public DataSet ConsultaDatosUsuarioAdministrador(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds;
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
            objSeg.SetSemilla(PI_Semilla);
            objSeg.IdOrganizacion = 39;
            objSeg.IdTransaccion = 4;
            objSeg.IdOpcion = 1;
            objSeg.ArrParams = new Object[1] {
                    PI_XmlDoc.OuterXml
                };
            ds = objSeg.EjecutaTransaccionDS(PI_Session);
            return ds;
        }
        [WebMethod]
        public DataSet ConsultaActivarUsuario(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds;
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
            objSeg.SetSemilla(PI_Semilla);
            objSeg.IdOrganizacion = 39;
            objSeg.IdTransaccion = 705;
            objSeg.IdOpcion = 1;
            objSeg.ArrParams = new Object[1] {
                    PI_XmlDoc.OuterXml
                };
            ds = objSeg.EjecutaTransaccionDS(PI_Session);
            return ds;
        }
        [WebMethod]
        public DataSet ConsultaDatosUsuarioAdicional(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds;
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
            objSeg.SetSemilla(PI_Semilla);
            objSeg.IdOrganizacion = 39;
            objSeg.IdTransaccion = 5;
            objSeg.IdOpcion = 1;
            objSeg.ArrParams = new Object[1] {
                    PI_XmlDoc.OuterXml
                };
            ds = objSeg.EjecutaTransaccionDS(PI_Session);
            return ds;
        }

        [WebMethod]
        public string[] ConsSemilla(string IdApl, string IdUsrLocal, string IdUsr, string Maquina)
        {
            string _Semilla = "";
            XmlDocument xmlResul = new XmlDocument();
            string[] retorno = new string[3];

            clibSeguridadCR.clsClienteSeg objCliSeg = new clibSeguridadCR.clsClienteSeg();
            _Semilla = objCliSeg.ConsSemilla();
            objCliSeg.SetSemilla(_Semilla);
            retorno[2] = _Semilla;
            xmlResul = objCliSeg.LoginAplicacion(int.Parse(IdApl), IdUsrLocal, Maquina);
            if (xmlResul.DocumentElement.GetAttribute("CodError").Equals("0"))
            {
                retorno[0] = xmlResul.OuterXml;
            }


            xmlResul = objCliSeg.LoginAplicacion(int.Parse(IdApl), IdUsr, Maquina);
            if (xmlResul.DocumentElement.GetAttribute("CodError").Equals("0"))
            {
                retorno[1] = xmlResul.OuterXml;
            }


            return retorno;
        }


        [WebMethod]
        public Boolean SubirArchivoActa(string archivo, string anio, string mes, string dia, byte[] contenido)
        {
            Boolean retorno = false;
            try
            {
                //var keyFile = new PrivateKeyFile(@"C:\\LogPagina\\OpenSsh-RSA-key.ppk");
                //var keyFiles = new[] { keyFile };
                //var username = AppConfig.SftpServerUserName;

                //var methods = new List<AuthenticationMethod>();
                //methods.Add(new PasswordAuthenticationMethod(username, AppConfig.SftpServerPassword));
                //methods.Add(new PrivateKeyAuthenticationMethod(username, keyFiles));

                //var con = new ConnectionInfo(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, methods.ToArray());


                //     Dim lv_sFtp As New clsFTP
                //lv_sFtp.lv_EsPasivo = chkPasivo.Checked
                //lv_sFtp.lv_IP = txtIP.Text
                //lv_sFtp.lv_Puerto = txtPuerto.Text
                //lv_sFtp.lv_Usuario = txtUsuario.Text
                //lv_sFtp.lv_Clave = txtClave.Text
                //lv_sFtp.lv_DirectorioLocal = txtDirectorioLocal.Text
                bool success = false;
                Boolean lv_SeCopio = false;
                string lv_Msg = "";
                clsFTP lv_sFtp = new clsFTP();
                string directorio = "";
                lv_sFtp.lv_EsPasivo = false;
                lv_sFtp.lv_IP = AppConfig.SftpServerIp;
                lv_sFtp.lv_Puerto = Convert.ToInt32(AppConfig.SftpServerPort);
                lv_sFtp.lv_Usuario = AppConfig.SftpServerUserName;
                lv_sFtp.lv_Clave = AppConfig.SftpServerPassword;
                directorio = "/home/sftpuser" + AppConfig.SftpPath + "/PDF/";
                success = lv_sFtp.crearCarpeta(directorio, anio);
                directorio = "/home/sftpuser" + AppConfig.SftpPath + "/PDF/" + anio + "/";
                success = lv_sFtp.crearCarpeta(directorio, mes);
                directorio = "/home/sftpuser" + AppConfig.SftpPath + "/PDF/" + anio + "/" + mes + "/";
                success = lv_sFtp.crearCarpeta(directorio, dia);
                directorio = "/home/sftpuser" + AppConfig.SftpPath + "/PDF/" + anio + "/" + mes + "/" + dia + "/";
                lv_SeCopio = lv_sFtp.CopiarArchivo_Sftp(directorio, archivo, contenido, lv_Msg);
                if (lv_SeCopio)
                {
                    retorno = true;
                }
                else
                    retorno = false;
                // using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                //// using (SftpClient sftpClient = new SftpClient(con))
                // {
                //     sftpClient.Connect();


                //     if (!sftpClient.Exists("/home/sftpuser" + AppConfig.SftpPath + "/PDF/" + anio))
                //     {
                //         sftpClient.CreateDirectory("/home/sftpuser" + AppConfig.SftpPath + "/PDF/" + anio);
                //     }

                //     if (!sftpClient.Exists("/home/sftpuser" + AppConfig.SftpPath + "/PDF/" + anio + "/" + mes))
                //     {
                //         sftpClient.CreateDirectory("/home/sftpuser" + AppConfig.SftpPath + "/PDF/" + anio + "/" + mes);
                //     }

                //     if (!sftpClient.Exists("/home/sftpuser" + AppConfig.SftpPath + "/PDF/" + anio + "/" + mes + "/" + dia))
                //     {
                //         sftpClient.CreateDirectory("/home/sftpuser" + AppConfig.SftpPath + "/PDF/" + anio + "/" + mes + "/" + dia);
                //     }
                //     sftpClient.WriteAllBytes("/home/sftpuser" + AppConfig.SftpPath + "/PDF/" + anio + "/" + mes + "/" + dia + "/" + archivo, contenido);
                //     sftpClient.Disconnect();
                //     retorno = true;
                // }

            }
            catch (Exception)
            {

                throw;
            }
            return retorno;
        }



        private void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = AppConfig.SftpServerPassword;
                }
            }
        }
        [WebMethod]
        public byte[] ArchivoActa(string archivo, string anio, string codSap, string codAlmacen)
        {
            byte[] retorno = null;
            try
            {
                string lv_Msg = "";
                clsFTP lv_sFtp = new clsFTP();
                string directorio = "";
                lv_sFtp.lv_EsPasivo = false;
                lv_sFtp.lv_IP = AppConfig.SftpServerIp;
                lv_sFtp.lv_Puerto = Convert.ToInt32(AppConfig.SftpServerPort);
                lv_sFtp.lv_Usuario = AppConfig.SftpServerUserName;
                lv_sFtp.lv_Clave = AppConfig.SftpServerPassword;
                directorio = AppConfig.SftpPath + "" + anio + "/Actas/" + codSap + "/" + codAlmacen + "/";
                //directorio = "/home/sftpuser" + AppConfig.SftpPath + "" + anio + "/Actas/" + codSap + "/" + codAlmacen + "/";
                retorno = lv_sFtp.ObtenerArchivo_Sftp(directorio, archivo, lv_Msg);
                //using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                //{
                //    sftpClient.Connect();
                //    retorno = sftpClient.ReadAllBytes(AppConfig.SftpPath + "/PDF/" + anio + "/" + mes + "/" + dia + "/" + archivo);
                //    sftpClient.Disconnect();
                //}

            }
            catch (Exception)
            {

                throw;
            }
            return retorno;
        }

        [WebMethod]
        public DataSet MantenimientoUser(string PI_Semilla, string PI_Session, string PI_XmlDoc)
        {
            DataSet ds;
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
            objSeg.SetSemilla(PI_Semilla);
            objSeg.IdOrganizacion = 39;
            objSeg.IdTransaccion = 1103;
            objSeg.IdOpcion = 1;
            objSeg.ArrParams = new Object[1] {
                    PI_XmlDoc
                };
            ds = objSeg.EjecutaTransaccionDS(PI_Session);
            return ds;
        }        

        [WebMethod]
        public byte[] LeePdfContrato(string nombre)
        {
            string RutaLectura = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivoPDF"]).Trim();
            RutaLectura = Path.Combine(RutaLectura, "PDF\\Contratos", nombre);
            return File.ReadAllBytes(RutaLectura);
        }

        [WebMethod]
        public void EscribePdfContrato(byte[] archivo, string nombre)
        {
            string RutaEscritura = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivoPDF"]).Trim();
            RutaEscritura = Path.Combine(RutaEscritura,"PDF\\Contratos");
            System.IO.File.WriteAllBytes(Path.Combine(RutaEscritura, nombre), archivo);
        }

        [WebMethod]
        public void EscribePdfAdjunto(byte[] archivo, string dir,string nombre)
        {
            string RutaEscritura = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivoPDF"]).Trim();

            if (!(Directory.Exists(Path.Combine(RutaEscritura,dir))))
            {
                Directory.CreateDirectory(Path.Combine(RutaEscritura, dir));
            }          

            System.IO.File.WriteAllBytes(Path.Combine(RutaEscritura, dir, nombre), archivo);
        }

        [WebMethod]
        public byte[] LeePdfAdjunto(string dir, string nombre)
        {
            string RutaLectura = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivoPDF"]).Trim();
            RutaLectura = Path.Combine(RutaLectura, dir, nombre);
            return File.ReadAllBytes(RutaLectura);
        }

        [WebMethod]
        public EmbedResponse consultaTokenBI(string user, string password, string IdAplicacion, string IdWorkStation, string IdReporte)
        {
            EmbedResponse respuesta = new EmbedResponse();
            IEmbedService m_embedService;
            m_embedService = new EmbedService(user, password, IdAplicacion, IdWorkStation, IdReporte);
            var embedResult = m_embedService.EmbedReport(null, null);
            respuesta.Resultado = embedResult;
            respuesta.EmbedConfig = m_embedService.EmbedConfig;
            return respuesta;
        }

    }

}

