using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Data;
using System.IO;

namespace AngularJSAuthentication.API.Controllers
{
    public class clsGeneraComprobanteXML
    {
        private string _clase = "clsGeneraComprobanteXML";
        private string _metodo;

        public string CodError;
        public string Msg;

        public clsGeneraComprobanteXML()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string getComprobanteFacturaXML(XmlDocument PI_SipeDocXML, ref string PO_NombreArchivo, ref string PO_ClaveAcceso)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ruc", Type.GetType("System.String"));
            dt.Columns.Add("TipoDoc", Type.GetType("System.String"));
            dt.Columns.Add("XmlDocumento", Type.GetType("System.String"));
            dt.Columns.Add("identificadorCliente", Type.GetType("System.String"));
            dt.Columns.Add("EscontribuyenteEspecial", Type.GetType("System.String"));
            dt.Columns.Add("dirMatriz", Type.GetType("System.String"));
            dt.Columns.Add("ClaveAcceso", Type.GetType("System.String"));
            dt.Columns.Add("nombreComercial", Type.GetType("System.String"));
            dt.Columns.Add("razonSocial", Type.GetType("System.String"));
            dt.Columns.Add("obligadoContabilidad", Type.GetType("System.Boolean"));
            dt.Columns.Add("Ambiente", Type.GetType("System.Boolean"));
            DataRow dr = dt.NewRow();
            dr["TipoDoc"] = "01";
            dr["XmlDocumento"] = PI_SipeDocXML.OuterXml;
            dr["ClaveAcceso"] = PO_ClaveAcceso;
            dr["Ambiente"] = true;
            dr["ruc"] = PI_SipeDocXML.DocumentElement.GetAttribute("r");
            dr["razonSocial"] = PI_SipeDocXML.DocumentElement.GetAttribute("rs");
            dr["nombreComercial"] = PI_SipeDocXML.DocumentElement.GetAttribute("nc");
            dr["dirMatriz"] = PI_SipeDocXML.DocumentElement.GetAttribute("dm");
            dr["identificadorCliente"] = PI_SipeDocXML.DocumentElement.GetAttribute("ic");
            dr["EscontribuyenteEspecial"] = PI_SipeDocXML.DocumentElement.GetAttribute("cce");
            dr["obligadoContabilidad"] = (PI_SipeDocXML.DocumentElement.GetAttribute("oc") == "NO" ? false : true);

            XmlDocument xmlSRI = new XmlDocument();
            xmlSRI.LoadXml("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><autorizacion></autorizacion>");
            XmlElement Elem;
            Elem = xmlSRI.CreateElement("estado");
            Elem.InnerText = "AUTORIZADO";
            xmlSRI.DocumentElement.AppendChild(Elem);
            Elem = xmlSRI.CreateElement("numeroAutorizacion");
            //Elem.InnerText = (DateTime.Now.ToString("yyyyMMddHHmmssffffff") + PI_SipeDocXML.DocumentElement.GetAttribute("s") + PI_SipeDocXML.DocumentElement.GetAttribute("r")).Substring(0, 37);
            Elem.InnerText = PI_SipeDocXML.DocumentElement.GetAttribute("na");
            xmlSRI.DocumentElement.AppendChild(Elem);
            Elem = xmlSRI.CreateElement("fechaAutorizacion");
            try
            {
                //fecha fin de vigencia de autorización del talonario
                string fechaFinVigAut = PI_SipeDocXML.DocumentElement.GetAttribute("ffva");
                int dd = int.Parse(fechaFinVigAut.Substring(0, 2));
                int MM = int.Parse(fechaFinVigAut.Substring(3, 2));
                int yyyy = int.Parse(fechaFinVigAut.Substring(6, 4));
                DateTime fechaFva = new DateTime(yyyy, MM, dd, 23, 59, 59, 999);
                Elem.InnerText = fechaFva.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            }
            catch (Exception)
            {
                Elem.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            }

            xmlSRI.DocumentElement.AppendChild(Elem);
            Elem = xmlSRI.CreateElement("ambiente");
            Elem.InnerText = "PRODUCCION";
            xmlSRI.DocumentElement.AppendChild(Elem);
            Elem = xmlSRI.CreateElement("comprobante");
            XmlCDataSection cdVal = xmlSRI.CreateCDataSection(GeneraDocumento_Factura(dr, ref PO_NombreArchivo, ref PO_ClaveAcceso));
            Elem.AppendChild(cdVal);
            xmlSRI.DocumentElement.AppendChild(Elem);

            var stringWriter = new StringWriter(new StringBuilder());
            var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
            xmlSRI.Save(xmlTextWriter);
            return stringWriter.ToString().Replace("\"utf-16\"", "\"iso-8859-1\"");
        }
        private string RedondearSRI(decimal valor)
        {
            return decimal.Round(valor, 2).ToString("#0.00;-#0.00").Replace(",", ".");
        }
        private void configuration()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ec");
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator = ",";
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ",";
        }
        private string GeneraDocumento_Factura(DataRow PI_drComprobante, ref string PO_NombreArchivo, ref string PO_ClaveAcceso)
        {
            _metodo = MethodBase.GetCurrentMethod().Name;
            //StringWriterWithEncoding builder = new StringWriterWithEncoding(new StringBuilder(), UTF8Encoding.UTF8);
            StringBuilder builder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            configuration();
            settings.Indent = true;
            settings.Encoding = UTF8Encoding.GetEncoding("iso-8859-1");
            settings.OmitXmlDeclaration = true;
            try
            {
                CodError = "";
                string NumeroComprobante;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml((string)PI_drComprobante["XmlDocumento"]);
                string Ambiente = "9";// ((bool)PI_drComprobante["Ambiente"] ? "2" : "1");
                string TipoEmision = "1";
                NumeroComprobante = (xmlDoc.DocumentElement.GetAttribute("e") + ("-"
                            + (xmlDoc.DocumentElement.GetAttribute("pe") + ("-" + xmlDoc.DocumentElement.GetAttribute("s")))));
                if (PO_NombreArchivo == "")
                {
                    PO_NombreArchivo =
                        //  ("\\"
                        //+ ((string)PI_drComprobante["ruc"] + ("\\"
                        //+ (DateTime.Now.Year.ToString() + ("\\"
                        //+ (DateTime.Now.Month.ToString("00.##") + ("\\"
                        //+ (DateTime.Now.Day.ToString("00.##") + ("\\" +
                                  ("FCT_"
                                + (int.Parse(xmlDoc.DocumentElement.GetAttribute("e")).ToString("000.##")
                                + (int.Parse(xmlDoc.DocumentElement.GetAttribute("pe")).ToString("000.##") + ("_"
                                + (xmlDoc.DocumentElement.GetAttribute("s") + ("_"
                                + (DateTime.Now.ToString("ddMMyyyy") + ("_"
                                + (DateTime.Now.ToString("HHmmss") + ("_"
                                + (Ambiente))))))))))); // + ".xml")))))))));
                }
                XmlWriter W = XmlWriter.Create(builder, settings);
                // W.Settings.Encoding = UTF8Encoding.UTF8
                W.Flush();
                W.WriteStartDocument(true);
                W.WriteStartElement("factura");
                W.WriteStartAttribute("id");
                W.WriteValue("comprobante");
                W.WriteEndAttribute();
                W.WriteStartAttribute("version");
                W.WriteValue("1.1.0");
                W.WriteEndAttribute();
                W.WriteStartElement("infoTributaria");
                // ambiente
                W.WriteStartElement("ambiente");
                // xmlDoc.DocumentElement.GetAttribute("e")
                W.WriteValue(Ambiente);
                W.WriteEndElement();
                // tipoEmision
                W.WriteStartElement("tipoEmision");
                W.WriteValue(TipoEmision);
                W.WriteEndElement();
                // razonSocial
                W.WriteStartElement("razonSocial");
                W.WriteValue((string)PI_drComprobante["razonSocial"]);
                W.WriteEndElement();
                if (!((string)PI_drComprobante["nombreComercial"]).Equals(""))
                {
                    W.WriteStartElement("nombreComercial");
                    W.WriteValue((string)PI_drComprobante["nombreComercial"]);
                    W.WriteEndElement();
                }
                // ruc
                W.WriteStartElement("ruc");
                W.WriteValue((string)PI_drComprobante["ruc"]);
                W.WriteEndElement();
                // claveAcceso
                W.WriteStartElement("claveAcceso");
                string fecha;
                fecha = xmlDoc.DocumentElement.GetAttribute("fe").ToString().Replace("/", "");
                if (((string)PI_drComprobante["ClaveAcceso"]).Equals(""))
                {
                    int lv_LongitudClaveAcceso = 0;
                    int i = 1;
                    while (((lv_LongitudClaveAcceso != 49)
                                || (i > 20)))
                    {
                        lv_LongitudClaveAcceso = DevuelveClaveAcceso(xmlDoc, Ambiente, ref PO_ClaveAcceso, PI_drComprobante, fecha, TipoEmision);
                        i = (i + 1);
                    }
                }
                else
                {
                    PO_ClaveAcceso = (string)PI_drComprobante["ClaveAcceso"];
                }
                W.WriteValue(PO_ClaveAcceso);
                W.WriteEndElement();
                // codDoc
                W.WriteStartElement("codDoc");
                W.WriteValue((string)PI_drComprobante["TipoDoc"]);
                W.WriteEndElement();
                // estab
                W.WriteStartElement("estab");
                W.WriteValue(xmlDoc.DocumentElement.GetAttribute("e"));
                W.WriteEndElement();
                // ptoEmi
                W.WriteStartElement("ptoEmi");
                W.WriteValue(xmlDoc.DocumentElement.GetAttribute("pe"));
                W.WriteEndElement();
                // secuencial
                W.WriteStartElement("secuencial");
                W.WriteValue(xmlDoc.DocumentElement.GetAttribute("s"));
                W.WriteEndElement();
                // dirMatriz
                W.WriteStartElement("dirMatriz");
                W.WriteValue((string)PI_drComprobante["dirMatriz"]);
                W.WriteEndElement();
                W.WriteEndElement();
                // fin infoTributaria
                W.WriteStartElement("infoFactura");
                // fechaEmision
                W.WriteStartElement("fechaEmision");
                fecha = xmlDoc.DocumentElement.GetAttribute("fe");
                // Mid(fecha, 1, 2) + "/" + Mid(fecha, 3, 2) + "/" + Mid(fecha, 5, 4)
                W.WriteValue(fecha);
                W.WriteEndElement();
                // dirEstablecimiento
                if (!xmlDoc.DocumentElement.GetAttribute("ds").ToString().Trim().Equals(""))
                {
                    W.WriteStartElement("dirEstablecimiento");
                    W.WriteValue(xmlDoc.DocumentElement.GetAttribute("ds").ToString().Trim());
                    W.WriteEndElement();
                }
                // contribuyenteEspecial
                if (!((string)PI_drComprobante["EscontribuyenteEspecial"]).Equals(""))
                {
                    W.WriteStartElement("contribuyenteEspecial");
                    W.WriteValue((string)PI_drComprobante["EscontribuyenteEspecial"]);
                    W.WriteEndElement();
                }
                // 'obligadoContabilidad
                if ((bool)PI_drComprobante["obligadoContabilidad"])
                {
                    W.WriteStartElement("obligadoContabilidad");
                    W.WriteValue("SI");
                    W.WriteEndElement();
                }
                // tipoIdentificacionComprador
                W.WriteStartElement("tipoIdentificacionComprador");
                W.WriteValue(xmlDoc.DocumentElement.GetAttribute("tic"));
                W.WriteEndElement();
                // razonSocialComprador
                W.WriteStartElement("razonSocialComprador");
                W.WriteValue(xmlDoc.DocumentElement.GetAttribute("rsc"));
                W.WriteEndElement();
                // identificacionComprador
                W.WriteStartElement("identificacionComprador");
                W.WriteValue((string)PI_drComprobante["identificadorCliente"]);
                W.WriteEndElement();
                W.WriteStartElement("totalSinImpuestos");
                W.WriteValue(xmlDoc.DocumentElement.GetAttribute("tsi"));
                W.WriteEndElement();
                // totalSinImpuestos
                W.WriteStartElement("totalDescuento");
                W.WriteValue(xmlDoc.DocumentElement.GetAttribute("td"));
                W.WriteEndElement();
                // totalDescuento
                if ((xmlDoc.SelectNodes("f/tc").Count > 0))
                {
                    W.WriteStartElement("totalConImpuestos");
                    bool ContBI_CAB = false;
                    foreach (XmlElement elementoImpuesto in xmlDoc.SelectNodes("f/tc"))
                    {
                        if (!(elementoImpuesto.Attributes["bi"].Value.Trim() == "0.00") ||
                                elementoImpuesto.Attributes["c"].Value.Trim() == "5")
                        {
                            // Si la Base Imponible es 0, se debe obviar el Impuesto.
                            ContBI_CAB = true;
                            W.WriteStartElement("totalImpuesto");
                            W.WriteStartElement("codigo");
                            W.WriteValue(elementoImpuesto.Attributes["c"].Value);
                            W.WriteEndElement();
                            // codigo
                            W.WriteStartElement("codigoPorcentaje");
                            W.WriteValue(elementoImpuesto.Attributes["cp"].Value);
                            W.WriteEndElement();
                            // codigoPorcentaje
                            W.WriteStartElement("baseImponible");
                            W.WriteValue(elementoImpuesto.Attributes["bi"].Value);
                            W.WriteEndElement();
                            // codigoPorcentaje
                            try
                            {
                                if (!(elementoImpuesto.Attributes["t"] == null))
                                {
                                    W.WriteStartElement("tarifa");
                                    W.WriteValue(elementoImpuesto.Attributes["t"].Value);
                                    W.WriteEndElement();
                                    // tarifa
                                }
                            }
                            catch (Exception)
                            {
                            }
                            W.WriteStartElement("valor");
                            W.WriteValue(elementoImpuesto.Attributes["v"].Value);
                            W.WriteEndElement();
                            // valor
                            W.WriteEndElement();
                            // finaliza totalImpuesto
                        }
                    }
                    if (!ContBI_CAB)
                    {
                        //  Si no existe ningun Impuesto, por lo menos Agregar 1
                        W.WriteStartElement("totalImpuesto");
                        W.WriteStartElement("codigo");
                        W.WriteValue("2");
                        W.WriteEndElement();
                        // codigo
                        W.WriteStartElement("codigoPorcentaje");
                        W.WriteValue("0");
                        W.WriteEndElement();
                        // codigoPorcentaje
                        W.WriteStartElement("baseImponible");
                        W.WriteValue("0.00");
                        W.WriteEndElement();
                        // codigoPorcentaje
                        W.WriteStartElement("tarifa");
                        W.WriteValue("0");
                        W.WriteEndElement();
                        // tarifa                    
                        W.WriteStartElement("valor");
                        W.WriteValue("0.00");
                        W.WriteEndElement();
                        // valor
                        W.WriteEndElement();
                        // finaliza totalImpuesto
                    }
                    W.WriteEndElement();
                    // finaliza totalConImpuestos
                }

                if (xmlDoc.DocumentElement.GetAttribute("cs") != "0.00" && xmlDoc.DocumentElement.GetAttribute("cs")!="")
                {
                    W.WriteStartElement("compensaciones");

                    W.WriteStartElement("compensacion");

                    W.WriteStartElement("codigo");
                    W.WriteValue("1");
                    W.WriteEndElement();

                    W.WriteStartElement("tarifa");
                    W.WriteValue("2");
                    W.WriteEndElement();

                    W.WriteStartElement("valor");
                    W.WriteValue(xmlDoc.DocumentElement.GetAttribute("cs").ToString());
                    W.WriteEndElement();

                    W.WriteEndElement();
                    W.WriteEndElement();
                }


                W.WriteStartElement("propina");
                W.WriteValue(xmlDoc.DocumentElement.GetAttribute("p"));
                W.WriteEndElement();
                // propina
                W.WriteStartElement("importeTotal");
                if (xmlDoc.DocumentElement.GetAttribute("cs") != "0.00" && xmlDoc.DocumentElement.GetAttribute("cs") != "")
                {
                    W.WriteValue(RedondearSRI(Convert.ToDecimal(xmlDoc.DocumentElement.GetAttribute("it")) - Convert.ToDecimal(xmlDoc.DocumentElement.GetAttribute("cs"))));

                }else
                {
                    W.WriteValue(RedondearSRI(Convert.ToDecimal(xmlDoc.DocumentElement.GetAttribute("it"))));
                }
                W.WriteEndElement();
                // importeTotal
                W.WriteStartElement("moneda");
                W.WriteValue(xmlDoc.DocumentElement.GetAttribute("m").ToString());
                W.WriteEndElement();
                // moneda
                W.WriteEndElement();
                // finaliza infoFactura
                W.WriteStartElement("detalles");
                foreach (XmlElement elementoDetalle in xmlDoc.SelectNodes("//f/d"))
                {
                    W.WriteStartElement("detalle");
                    W.WriteStartElement("codigoPrincipal");
                    W.WriteValue(elementoDetalle.GetAttribute("cp"));
                    W.WriteEndElement();
                    // codigoPrincipal
                    if ((elementoDetalle.GetAttribute("ca").Trim() != String.Empty))
                    {
                        W.WriteStartElement("codigoAuxiliar");
                        W.WriteValue(elementoDetalle.GetAttribute("ca"));
                        W.WriteEndElement();
                        // codigoAuxiliar
                    }
                    // descripcion
                    W.WriteStartElement("descripcion");
                    W.WriteValue(elementoDetalle.GetAttribute("de"));
                    W.WriteEndElement();
                    // cantidad
                    W.WriteStartElement("cantidad");
                    W.WriteValue(elementoDetalle.GetAttribute("c"));
                    W.WriteEndElement();
                    // precioUnitario
                    W.WriteStartElement("precioUnitario");
                    W.WriteValue(elementoDetalle.GetAttribute("pu"));
                    W.WriteEndElement();
                    // descuento
                    W.WriteStartElement("descuento");
                    W.WriteValue(elementoDetalle.GetAttribute("d"));
                    W.WriteEndElement();
                    // precioTotalSinImpuesto
                    W.WriteStartElement("precioTotalSinImpuesto");
                    W.WriteValue(elementoDetalle.GetAttribute("ptsi"));
                    W.WriteEndElement();
                    // detalle de Item
                    string detAdicional = elementoDetalle.GetAttribute("dad");
                    // InfoAdicional de Detalle
                    try
                    {
                        if ((elementoDetalle.SelectNodes("detAdicional").Count > 0 || !string.IsNullOrEmpty(detAdicional)))
                        {
                            W.WriteStartElement("detallesAdicionales");
                            W.WriteStartElement("detAdicional");
                            W.WriteStartAttribute("nombre");
                            W.WriteValue("");
                            W.WriteStartAttribute("valor");
                            W.WriteValue(detAdicional);
                            W.WriteEndElement();
                            foreach (XmlElement elementoDetalleAdicional in elementoDetalle.SelectNodes("detAdicional"))
                            {
                                W.WriteStartElement("detAdicional");
                                W.WriteStartAttribute("nombre");
                                W.WriteValue(elementoDetalleAdicional.Attributes["nombre"].Value);
                                W.WriteStartAttribute("valor");
                                W.WriteValue(elementoDetalleAdicional.Attributes["valor"].Value);
                                W.WriteEndElement();
                                // detAdicional
                            }
                            W.WriteEndElement();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    //  Impuestos de Detalle
                    if ((elementoDetalle.SelectNodes("i").Count > 0))
                    {
                        bool ContBI = false;
                        W.WriteStartElement("impuestos");
                        foreach (XmlElement elementoDetalleImpuesto in elementoDetalle.SelectNodes("i"))
                        {
                            if (!(elementoDetalleImpuesto.Attributes["bi"].Value.Trim() == "0.00") ||
                                elementoDetalleImpuesto.Attributes["c"].Value.Trim() == "5")
                            {
                                ContBI = true;
                                W.WriteStartElement("impuesto");
                                W.WriteStartElement("codigo");
                                W.WriteValue(elementoDetalleImpuesto.Attributes["c"].Value);
                                W.WriteEndElement();
                                // codigo
                                W.WriteStartElement("codigoPorcentaje");
                                W.WriteValue(elementoDetalleImpuesto.Attributes["cp"].Value);
                                W.WriteEndElement();
                                // codigoPorcentaje
                                W.WriteStartElement("tarifa");
                                W.WriteValue(elementoDetalleImpuesto.Attributes["t"].Value);
                                W.WriteEndElement();
                                // tarifa
                                W.WriteStartElement("baseImponible");
                                W.WriteValue(elementoDetalleImpuesto.Attributes["bi"].Value);
                                W.WriteEndElement();
                                // baseImponible
                                W.WriteStartElement("valor");
                                W.WriteValue(elementoDetalleImpuesto.Attributes["v"].Value);
                                W.WriteEndElement();
                                // valor
                                W.WriteEndElement();
                                // fin impuesto
                            }
                        }
                        if (!ContBI)
                        {
                            //  Si no existe ningun Impuesto, por lo menos Agregar 1
                            W.WriteStartElement("impuesto");
                            W.WriteStartElement("codigo");
                            W.WriteValue("2");
                            W.WriteEndElement();
                            // codigo
                            W.WriteStartElement("codigoPorcentaje");
                            W.WriteValue("0");
                            W.WriteEndElement();
                            // codigoPorcentaje
                            W.WriteStartElement("tarifa");
                            W.WriteValue("0");
                            W.WriteEndElement();
                            // tarifa
                            W.WriteStartElement("baseImponible");
                            W.WriteValue("0.00");
                            W.WriteEndElement();
                            // baseImponible
                            W.WriteStartElement("valor");
                            W.WriteValue("0.00");
                            W.WriteEndElement();
                            // valor
                            W.WriteEndElement();
                            // fin impuesto
                        }
                        W.WriteEndElement();
                        // fin impuestos
                    }
                    W.WriteEndElement();
                    // fin detalle
                }
                W.WriteEndElement();
                // fin detalles
                if ((xmlDoc.SelectNodes("f/ia").Count > 0))
                {
                    W.WriteStartElement("infoAdicional");

                    foreach (XmlElement elementoAdicional in xmlDoc.SelectNodes("f/ia"))
                    {
                        W.WriteStartElement("campoAdicional");
                        W.WriteAttributeString("nombre", elementoAdicional.Attributes["n"].Value);
                        W.WriteValue(elementoAdicional.Attributes["v"].Value);
                        W.WriteEndElement();
                        // tarifa
                    }

                    W.WriteEndElement();
                }
                W.WriteEndElement();
                // finnd
                W.WriteEndDocument();
                // finaliza documento   
                W.Flush();
                W.Close();
            }
            catch (Exception ex)
            {
                string MsgAdicional = ex.Message;
                try
                {
                    MsgAdicional = ex.InnerException.StackTrace;
                }
                catch (System.Exception)
                {
                    try
                    {
                        if ((CodError == ""))
                        {
                            CodError = "602";
                            // Error en la Generacion de Documento Xml
                            Msg = (_clase + (" "
                                        + (_metodo + (" ERROR 1: "
                                        + (ex.StackTrace + (" " + MsgAdicional))))));
                            // + "  " + ex.InnerException.StackTrace
                        }
                        else
                        {
                            Msg = (_clase + (" "
                                        + (_metodo + (" ERROR 1: "
                                        + (ex.StackTrace + (" " + MsgAdicional))))));
                            //  + "  " + ex.InnerException.StackTrace
                        }
                    }
                    catch (System.Exception)
                    {
                    }
                }
            }
            return "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + builder.ToString();
        }

        private int DevuelveClaveAcceso(XmlDocument xmlDoc, string Ambiente, ref string PO_ClaveAcceso, DataRow PI_drComprobante, string fecha, string TipoEmision)
        {
            int longitud = 0;
            try
            {
                string clave = null;
                //clave = PI_drComprobante("ruc").ToString.Substring(0, 8)
                clave = xmlDoc.DocumentElement.GetAttribute("s").Substring(1, 8);
                PO_ClaveAcceso = fecha + PI_drComprobante["TipoDoc"].ToString() + PI_drComprobante["ruc"].ToString() + Ambiente + xmlDoc.DocumentElement.GetAttribute("e") + xmlDoc.DocumentElement.GetAttribute("pe") + xmlDoc.DocumentElement.GetAttribute("s") + clave + TipoEmision;
                char[] a = null;
                a = PO_ClaveAcceso.ToCharArray();
                PO_ClaveAcceso = algoritmoDigito9(a);
                longitud = PO_ClaveAcceso.Length;
            }
            catch (Exception)
            {
                longitud = 0;
                //CodError = 601 'Error en la Generacion de Clave de Acceso
                //Msg = Msg + " " + _clase + " " + _metodo + " ERROR: " + ex.Message
                //Throw New Exception(Msg)
            }
            return longitud;
        }

        private string algoritmoDigito9(char[] ElNumero)
        {
            string Resultado = "";
            int Multiplicador = 2;
            int iNum = 0;
            int Suma = 0;

            for (int i = 47; i >= 0; i += -1)
            {
                iNum = int.Parse(ElNumero[i].ToString());
                Suma += iNum * Multiplicador;
                Multiplicador += 1;
                if (Multiplicador == 8)
                    Multiplicador = 2;
            }
            Resultado = Convert.ToString(11 - (Suma % 11));
            if (Resultado == "10")
                Resultado = "1";
            if (Resultado == "11")
                Resultado = "0";
            return String.Concat(ElNumero) + Resultado;
        }

    }
}