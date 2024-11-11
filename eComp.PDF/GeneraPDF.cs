using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml;
using System.Data;
using System.Globalization;

namespace eComp.PDF
{
    /// <summary>
    /// Clase utilizada para la generación de los archivos en formato PDF que serán visualizados en la página web y enviados por correo electrónico
    /// </summary>
    public class GeneraPDF
    {


        /// <summary>
        /// Método para generación de factura RIDE
        /// </summary>
        /// <param name="pDocumento_autorizado">Documento Autorizado</param>
        /// <param name="pRutaLogo">Ruta física o URL del logo a imprimir</param>
        /// <param name="pCultura">Cultura, por defecto en-US</param>
        /// <returns>Arreglo de bytes</returns>
        public byte[] GeneraFactura(string pDocumento_autorizado, string pRutaLogo,string porcentaje,string pCultura = "en-US")
        {
            MemoryStream ms = null;
            byte[] Bytes = null;
            try
            {
                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "", 
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sRuc ="", sContribuyenteEspecial ="", sAmbienteVal ="",
                           sNumAutorizacion = "", sObligadoContabilidad = "",
                           sFechaEmision = "", FechaIniVigAutSRI = "";
                    Cultura = pCultura;
                    
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";

                    oDocument.LoadXml(pDocumento_autorizado);

                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    sSucursal = (oDocument.SelectSingleNode("//infoFactura/dirEstablecimiento") == null) ? "" : oDocument.SelectSingleNode("//infoFactura/dirEstablecimiento").InnerText;
                    sContribuyenteEspecial = oDocument.SelectSingleNode("//infoFactura/contribuyenteEspecial") ==null ? "":  oDocument.SelectSingleNode("//infoFactura/contribuyenteEspecial").InnerText;
                    sObligadoContabilidad = oDocument.SelectSingleNode("//infoFactura/obligadoContabilidad") == null ? "" : oDocument.SelectSingleNode("//infoFactura/obligadoContabilidad").InnerText;

                    try
                    {
                        string fechaTmp = oDocument.SelectSingleNode("//infoFactura/fechaEmision").InnerText;
                        int dd = int.Parse(fechaTmp.Substring(0, 2));
                        int MM = int.Parse(fechaTmp.Substring(3, 2));
                        int yyyy = int.Parse(fechaTmp.Substring(6, 4));
                        DateTime fechaObj = new DateTime(yyyy, MM, dd, 23, 59, 59, 999);
                        sFechaEmision = fechaObj.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    }
                    catch (Exception)
                    {
                        sFechaEmision = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    }

                    try
                    {
                        XmlNode nodoFechaIniVigSRI = oDocument.SelectNodes("//infoAdicional/campoAdicional[@nombre='FechaIniVigAutSRI']").Item(0);
                        string fechaTmp = nodoFechaIniVigSRI.InnerText;
                        int dd = int.Parse(fechaTmp.Substring(0, 2));
                        int MM = int.Parse(fechaTmp.Substring(3, 2));
                        int yyyy = int.Parse(fechaTmp.Substring(6, 4));
                        DateTime fechaObj = new DateTime(yyyy, MM, dd, 23, 59, 59, 999);
                        FechaIniVigAutSRI = fechaObj.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    }
                    catch (Exception)
                    {
                        FechaIniVigAutSRI = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    }

                    int registros = 0;
                    int PagLimite1 = 30;
                    int MaxPagina1 = 40;
                    int MaxSoloPagina = 65;

                    float posDetalleCliente = 0;
                    float posDetalleFactura = 0;
                    float posInfoAdicional = 0;

                    PdfWriter writer;
                    RoundRectangle rr = new RoundRectangle();
                    Document documento = new Document();

                    writer = PdfWriter.GetInstance(documento, ms);

                    iTextSharp.text.Font cabeceraPrin = GetArial(11);
                    iTextSharp.text.Font cabecera = GetArial(8);
                    iTextSharp.text.Font detalle = GetArial(7);
                    iTextSharp.text.Font detAdicional = GetArial(6);

                    //BaseColor colorTX = BaseColor.BLACK;
                    //BaseColor colorBG = BaseColor.LIGHT_GRAY;

                    //cabecera.Color = colorTX;
                    detalle.Color = cabecera.Color;
                    detAdicional.Color = cabecera.Color;

                    documento.Open();
                    var oEvents = new ITextEvents();
                    writer.PageEvent = oEvents;
                    PdfContentByte canvas = writer.DirectContent;
                    iTextSharp.text.Image jpg = null;

                    jpg = iTextSharp.text.Image.GetInstance(pRutaLogo);
                    
                    jpg.ScaleToFit(250f, 70f);


                    #region TablaDerecha
                    PdfPTable tableR = new PdfPTable(1);
                    PdfPTable innerTableD = new PdfPTable(1);

                    iTextSharp.text.Font cabeceraFilaIniFin = GetArial(4);
                    PdfPCell filaINI = new PdfPCell(new Paragraph(" ", cabeceraFilaIniFin));
                    filaINI.Border = Rectangle.NO_BORDER;
                    innerTableD.AddCell(filaINI);

                    iTextSharp.text.Font cabOtroColor = GetArial(12);
                    cabOtroColor.SetColor(80, 37, 90);
                    Paragraph Parte;
                    PdfPCell Separador;

                    //Separador = new PdfPCell(new Paragraph(" ", cabeceraPrin));
                    //Separador.Border = Rectangle.NO_BORDER;
                    //innerTableD.AddCell(Separador);

                    //Separador = new PdfPCell(new Paragraph(" ", cabeceraPrin));
                    //Separador.Border = Rectangle.NO_BORDER;
                    //innerTableD.AddCell(Separador);

                    Parte = new Paragraph("R.U.C.: ", cabeceraPrin);
                    Parte.Add(new Chunk(sRuc, cabOtroColor));
                    PdfPCell RUC = new PdfPCell(Parte);
                    RUC.Border = Rectangle.NO_BORDER;
                    RUC.Padding = 5f;
                    RUC.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(RUC);

                    Separador = new PdfPCell(new Paragraph(" ", cabeceraPrin));
                    Separador.Border = Rectangle.NO_BORDER;
                    innerTableD.AddCell(Separador);

                    PdfPCell Factura = new PdfPCell(new Paragraph("NÚMERO DE FACTURA:", cabeceraPrin));
                    Factura.Border = Rectangle.NO_BORDER;
                    Factura.Padding = 5f;
                    Factura.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(Factura);

                    PdfPCell NumFactura = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabOtroColor));
                    NumFactura.Border = Rectangle.NO_BORDER;
                    NumFactura.Padding = 5f;
                    NumFactura.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(NumFactura);

                    Separador = new PdfPCell(new Paragraph(" ", cabecera));
                    Separador.Border = Rectangle.NO_BORDER;
                    innerTableD.AddCell(Separador);

                    PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabeceraPrin));
                    lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                    lblNumAutorizacion.Padding = 5f;
                    lblNumAutorizacion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(lblNumAutorizacion);

                    PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabOtroColor));
                    NumAutorizacion.Border = Rectangle.NO_BORDER;
                    NumAutorizacion.Padding = 5f;
                    NumAutorizacion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(NumAutorizacion);

                    Separador = new PdfPCell(new Paragraph(" ", cabeceraPrin));
                    Separador.Border = Rectangle.NO_BORDER;
                    innerTableD.AddCell(Separador);

                    PdfPCell FechaInicioVigAutSRI = new PdfPCell(new Paragraph("FECHA INICIO VIGENCIA DE AUTORIZACIÓN:", cabeceraPrin));
                    FechaInicioVigAutSRI.Border = Rectangle.NO_BORDER;
                    FechaInicioVigAutSRI.Padding = 5f;
                    FechaInicioVigAutSRI.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(FechaInicioVigAutSRI);

                    PdfPCell FechaInicioVigAutSRIVal = new PdfPCell(new Paragraph(FechaIniVigAutSRI, cabOtroColor));
                    FechaInicioVigAutSRIVal.Border = Rectangle.NO_BORDER;
                    FechaInicioVigAutSRIVal.Padding = 5f;
                    FechaInicioVigAutSRIVal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(FechaInicioVigAutSRIVal);

                    Separador = new PdfPCell(new Paragraph(" ", cabeceraPrin));
                    Separador.Border = Rectangle.NO_BORDER;
                    innerTableD.AddCell(Separador);

                    PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA FIN VIGENCIA DE AUTORIZACIÓN:", cabeceraPrin));
                    FechaAutorizacion.Border = Rectangle.NO_BORDER;
                    FechaAutorizacion.Padding = 5f;
                    FechaAutorizacion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(FechaAutorizacion);

                    PdfPCell FechaAutorizacionVal = new PdfPCell(new Paragraph(sFechaAutorizacion, cabOtroColor));
                    FechaAutorizacionVal.Border = Rectangle.NO_BORDER;
                    FechaAutorizacionVal.Padding = 5f;
                    FechaAutorizacionVal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(FechaAutorizacionVal);

                    Separador = new PdfPCell(new Paragraph(" ", cabeceraPrin));
                    Separador.Border = Rectangle.NO_BORDER;
                    innerTableD.AddCell(Separador);

                    PdfPCell FechaEmsion = new PdfPCell(new Paragraph("FECHA DE EMISIÓN DE LA FACTURA:", cabeceraPrin));
                    FechaEmsion.Border = Rectangle.NO_BORDER;
                    FechaEmsion.Padding = 5f;
                    FechaEmsion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(FechaEmsion);

                    PdfPCell FechaEmsionVal = new PdfPCell(new Paragraph(sFechaEmision, cabOtroColor));
                    FechaEmsionVal.Border = Rectangle.NO_BORDER;
                    FechaEmsionVal.Padding = 5f;
                    FechaEmsionVal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    innerTableD.AddCell(FechaEmsionVal);

                    //Parte = new Paragraph("AMBIENTE: ", cabeceraPrin);
                    //Parte.Add(new Chunk(sAmbiente, cabOtroColor));
                    //PdfPCell Ambiente = new PdfPCell(Parte);
                    //Ambiente.Border = Rectangle.NO_BORDER;
                    //Ambiente.Padding = 5f;
                    //Ambiente.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    //innerTableD.AddCell(Ambiente);

                    //Parte = new Paragraph("EMISIÓN: ", cabeceraPrin);
                    //Parte.Add(new Chunk(sTipoEmision, cabOtroColor));
                    //PdfPCell Emision = new PdfPCell(Parte);
                    //Emision.Border = Rectangle.NO_BORDER;
                    //Emision.Padding = 5f;
                    //Emision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    //innerTableD.AddCell(Emision);

                    //PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CÓDIGO DE SEGURIDAD: ", cabeceraPrin));
                    //ClaveAcceso.Border = Rectangle.NO_BORDER;
                    //ClaveAcceso.Padding = 5f;
                    //ClaveAcceso.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    //innerTableD.AddCell(ClaveAcceso);

                    //PdfPCell ClaveAccesoVal = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, cabOtroColor));
                    //ClaveAccesoVal.Border = Rectangle.NO_BORDER;
                    //ClaveAccesoVal.Padding = 5f;
                    //ClaveAccesoVal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    //innerTableD.AddCell(ClaveAccesoVal);

                    ////Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);

                    ////PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                    ////ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                    ////ImgClaveAcceso.Padding = 5f;
                    ////ImgClaveAcceso.Colspan = 2;
                    ////ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;

                    ////innerTableD.AddCell(ImgClaveAcceso);

                    PdfPCell filaFIN = new PdfPCell(new Paragraph(" ", cabeceraFilaIniFin));
                    filaFIN.Border = Rectangle.NO_BORDER;
                    filaFIN.Padding = 5f;
                    innerTableD.AddCell(filaFIN);

                    var ContenedorD = new PdfPCell(innerTableD);
                    ContenedorD.CellEvent = rr;
                    ContenedorD.Border = Rectangle.NO_BORDER;
                    //ContenedorD.BackgroundColor = colorBG;
                    tableR.AddCell(ContenedorD);
                    tableR.TotalWidth = 278f;
                    #endregion

                    #region TablaIzquierda
                    PdfPTable tableL = new PdfPTable(1);
                    PdfPTable innerTableL = new PdfPTable(1);

                    PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                    RazonSocial.Border = Rectangle.NO_BORDER;
                    RazonSocial.Padding = 5f;
                    RazonSocial.FixedHeight = 20f;
                    innerTableL.AddCell(RazonSocial);

                    PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dir Matriz: " + sMatriz, cabecera));
                    DirMatriz.Border = Rectangle.NO_BORDER;
                    DirMatriz.Padding = 5f;
                    innerTableL.AddCell(DirMatriz);

                    PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dir Sucursal: " + sSucursal, cabecera));
                    DirSucursal.Border = Rectangle.NO_BORDER;
                    DirSucursal.Padding = 5f;
                    if (sSucursal.Length > 40) { 
                        DirSucursal.FixedHeight = 28f;
                    }
                    innerTableL.AddCell(DirSucursal);

                    PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro: " + sContribuyenteEspecial , cabecera));
                    ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                    ContribuyenteEspecial.Padding = 5f;
                    ContribuyenteEspecial.FixedHeight = 20f;
                    innerTableL.AddCell(ContribuyenteEspecial);

                    PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("Obligado a llevar contabilidad: " + sObligadoContabilidad, cabecera));
                    ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                    ObligadoContabilidad.Padding = 5f;
                    ObligadoContabilidad.FixedHeight = 20f;
                    innerTableL.AddCell(ObligadoContabilidad);

                    var ContenedorL = new PdfPCell(innerTableL);
                    ContenedorL.CellEvent = rr;
                    ContenedorL.Border = Rectangle.NO_BORDER;
                    //ContenedorL.BackgroundColor = colorBG;
                    tableL.AddCell(ContenedorL);
                    tableL.TotalWidth = 250f;
                    #endregion

                    #region Logo
                    PdfPTable tableLOGO = new PdfPTable(1);
                    PdfPCell logo = null;
                    logo = new PdfPCell(jpg);
                    
                    logo.Border = Rectangle.NO_BORDER;
                    tableLOGO.AddCell(logo);
                    tableLOGO.TotalWidth = 250f;
                    #endregion

                    #region DetalleCliente
                    PdfPTable tableDetalleCliente = new PdfPTable(4);
                    tableDetalleCliente.TotalWidth = 540f;
                    tableDetalleCliente.WidthPercentage = 100;
                    float[] DetalleClientewidths = new float[] { 30f, 120f, 30f, 40f };
                    tableDetalleCliente.SetWidths(DetalleClientewidths);

                    //var lblNombreCliente = new PdfPCell(new Paragraph("Razón Social / Nombres y Apellidos:", detalle));
                    var lblNombreCliente = new PdfPCell(new Paragraph("Razón Social:", detalle));
                    lblNombreCliente.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(lblNombreCliente);
                    var NombreCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/razonSocialComprador").InnerText, detalle));
                    NombreCliente.Border = Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(NombreCliente);
                    var lblRUC = new PdfPCell(new Paragraph("RUC / CI:", detalle));
                    lblRUC.Border = Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(lblRUC);
                    var RUCcliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/identificacionComprador").InnerText, detalle));
                    RUCcliente.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableDetalleCliente.AddCell(RUCcliente);

                    //var lblFechaEmisionCliente = new PdfPCell(new Paragraph("Fecha Emisión:", detalle));
                    var lblFechaEmisionCliente = new PdfPCell(new Paragraph("", detalle));
                    lblFechaEmisionCliente.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(lblFechaEmisionCliente);

                    //var FechaEmisionCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/fechaEmision").InnerText, detalle));
                    var FechaEmisionCliente = new PdfPCell(new Paragraph("", detalle));
                    FechaEmisionCliente.Border = Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(FechaEmisionCliente);

                    //var lblGuiaRemision = new PdfPCell(new Paragraph("Guia de remisión", detalle));
                    var lblGuiaRemision = new PdfPCell(new Paragraph("", detalle));
                    lblGuiaRemision.Border = Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(lblGuiaRemision);

                    //var GuiaRemision = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/guiaRemision") == null ? "" : oDocument.SelectSingleNode("//infoFactura/guiaRemision").InnerText, detalle));
                    var GuiaRemision = new PdfPCell(new Paragraph("", detalle));
                    GuiaRemision.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                    tableDetalleCliente.AddCell(GuiaRemision);

                    #endregion

                    #region DetalleFactura
                    PdfPTable tableDetalleFactura = new PdfPTable(9); //10 
                    tableDetalleFactura.TotalWidth = 540f;
                    tableDetalleFactura.WidthPercentage = 100;
                    tableDetalleFactura.LockedWidth = true;
                    float[] DetalleFacturawidths = new float[] { 30f, 15f, 80f, 35f, 35f, 35f, 20f, 22f, 22f }; 
                    //float[] DetalleFacturawidths = new float[] { 40f, 15f, 120f, 45f, 25f, 25f, 30f };
                    tableDetalleFactura.SetWidths(DetalleFacturawidths);

                    var fontEncabezado = GetArial(7);
                    fontEncabezado.Color = cabecera.Color;
                    var encCodPrincipal = new PdfPCell(new Paragraph("Cod. Principal", fontEncabezado));
                    encCodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    //var encCodAuxiliar = new PdfPCell(new Paragraph("Cod. Auxiliar", fontEncabezado));
                    //encCodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encCant = new PdfPCell(new Paragraph("Cant.", fontEncabezado));
                    encCant.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDescripcion = new PdfPCell(new Paragraph("Descripción", fontEncabezado));
                    encDescripcion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDetalleAdicional1 = new PdfPCell(new Paragraph("Detalle Adicional", fontEncabezado));
                    encDetalleAdicional1.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDetalleAdicional2 = new PdfPCell(new Paragraph("Detalle Adicional", fontEncabezado));
                    encDetalleAdicional2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDetalleAdicional3 = new PdfPCell(new Paragraph("Detalle Adicional", fontEncabezado));
                    encDetalleAdicional3.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPrecioUnitario = new PdfPCell(new Paragraph("Precio Unit.", fontEncabezado));
                    encPrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDescuento = new PdfPCell(new Paragraph("Descuento", fontEncabezado));
                    encDescuento.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPrecioTotal = new PdfPCell(new Paragraph("Precio Total", fontEncabezado));
                    encPrecioTotal.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    tableDetalleFactura.AddCell(encCodPrincipal);
                    //tableDetalleFactura.AddCell(encCodAuxiliar);
                    tableDetalleFactura.AddCell(encCant);
                    tableDetalleFactura.AddCell(encDescripcion);
                    tableDetalleFactura.AddCell(encDetalleAdicional1);
                    tableDetalleFactura.AddCell(encDetalleAdicional2);
                    tableDetalleFactura.AddCell(encDetalleAdicional3);
                    tableDetalleFactura.AddCell(encPrecioUnitario);
                    tableDetalleFactura.AddCell(encDescuento);
                    tableDetalleFactura.AddCell(encPrecioTotal);

                    PdfPCell CodPrincipal = null;
                    //PdfPCell CodAuxiliar = null;
                    PdfPCell Cant;
                    PdfPCell Descripcion;
                    PdfPCell DetalleAdicional1;
                    PdfPCell DetalleAdicional2;
                    PdfPCell DetalleAdicional3;
                    PdfPCell PrecioUnitario;
                    PdfPCell Descuento;
                    PdfPCell PrecioTotal;

                    XmlNodeList Detalles;
                    Detalles = oDocument.SelectNodes("//detalles/detalle");
                    registros = Detalles.Count;
                    foreach (XmlNode Elemento in Detalles)
                    {
                            CodPrincipal = new PdfPCell(new Phrase(Elemento["codigoPrincipal"].InnerText, detAdicional));
                            CodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            //CodAuxiliar = new PdfPCell(new Phrase(Elemento["codigoAuxiliar"] == null ? "" : Elemento["codigoAuxiliar"].InnerText, detAdicional));
                            //CodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            Cant = new PdfPCell(new Phrase(Elemento["cantidad"].InnerText, detAdicional));
                            Cant.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            Descripcion = new PdfPCell(new Phrase(Elemento["descripcion"].InnerText.Length > 50 ? Elemento["descripcion"].InnerText.Substring(0, 49) : Elemento["descripcion"].InnerText, detalle));
                            XmlNodeList DetallesAdicionales;

                            DetallesAdicionales = Elemento.SelectNodes("detallesAdicionales/detAdicional");
                            if (!(DetallesAdicionales[0] == null))
                            {
                                DetalleAdicional1 = new PdfPCell(new Phrase((string.IsNullOrEmpty(DetallesAdicionales[0].Attributes["nombre"].Value) ? "" : DetallesAdicionales[0].Attributes["nombre"].Value + ": ") + DetallesAdicionales[0].Attributes["valor"].Value, detAdicional));
                            }
                            else
                            {
                                DetalleAdicional1 = new PdfPCell(new Phrase("", detAdicional));
                            }
                            if (!(DetallesAdicionales[1] == null))
                            {
                                DetalleAdicional2 = new PdfPCell(new Phrase((string.IsNullOrEmpty(DetallesAdicionales[1].Attributes["nombre"].Value) ? "" : DetallesAdicionales[1].Attributes["nombre"].Value + ": ") + DetallesAdicionales[1].Attributes["valor"].Value, detAdicional));
                            }
                            else
                            {
                                DetalleAdicional2 = new PdfPCell(new Phrase("", detAdicional));
                            }
                            if (!(DetallesAdicionales[2] == null))
                            {
                                DetalleAdicional3 = new PdfPCell(new Phrase((string.IsNullOrEmpty(DetallesAdicionales[2].Attributes["nombre"].Value) ? "" : DetallesAdicionales[2].Attributes["nombre"].Value + ": ") + DetallesAdicionales[2].Attributes["valor"].Value, detAdicional));
                            }
                            else
                            {
                                DetalleAdicional3 = new PdfPCell(new Phrase("", detAdicional));
                            }

                            PrecioUnitario = new PdfPCell(new Phrase(Elemento["precioUnitario"].InnerText, detalle));
                            PrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                            Descuento = new PdfPCell(new Phrase(Elemento["descuento"].InnerText, detalle));
                            Descuento.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                            PrecioTotal = new PdfPCell(new Phrase(Elemento["precioTotalSinImpuesto"].InnerText, detalle));
                            PrecioTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                            tableDetalleFactura.AddCell(CodPrincipal);
                            //tableDetalleFactura.AddCell(CodAuxiliar);
                            tableDetalleFactura.AddCell(Cant);
                            tableDetalleFactura.AddCell(Descripcion);
                            tableDetalleFactura.AddCell(DetalleAdicional1);
                            tableDetalleFactura.AddCell(DetalleAdicional2);
                            tableDetalleFactura.AddCell(DetalleAdicional3);
                            tableDetalleFactura.AddCell(PrecioUnitario);
                            tableDetalleFactura.AddCell(Descuento);
                            tableDetalleFactura.AddCell(PrecioTotal);
                    }

                    #endregion
                    
                    #region InformacionAdicional
                    var tableInfoAdicional = new PdfPTable(2);
                    tableInfoAdicional.TotalWidth = 250f;
                    float[] InfoAdicionalWidths = new float[] { 65f, 170f };
                    tableInfoAdicional.SetWidths(InfoAdicionalWidths);

                    var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                    lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblInfoAdicional.Colspan = 2;
                    lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblInfoAdicional.Padding = 5f;
                    bool AgregalblInfoAdicional = false;

                    var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottom.Padding = 2f;
                    var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                    Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottom.Padding = 2f;

                    XmlNodeList InfoAdicional;
                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    PdfPCell lblCodigo;
                    PdfPCell Codigo;

                    string NumPedidoCodBarra = "";
                    string NumFacturaCodBarra = "";
                    string FechaGeneraPortal = "";

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        if (campoAdicional.Attributes["nombre"].Value == "FechaGeneraPortal")
                        {
                            FechaGeneraPortal = campoAdicional.InnerText;
                        }
                        else if (campoAdicional.Attributes["nombre"].Value == "FechaIniVigAutSRI")
                        {
                            //no hacer nada - no se muestra
                            FechaIniVigAutSRI = FechaIniVigAutSRI.Trim();
                        }
                        else if (campoAdicional.Attributes["nombre"].Value == "NumPedido")
                        {
                            NumPedidoCodBarra = campoAdicional.InnerText;
                            NumFacturaCodBarra = oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText;
                        }
                        else
                        {
                            if (AgregalblInfoAdicional == false)
                            {
                                tableInfoAdicional.AddCell(lblInfoAdicional);
                                AgregalblInfoAdicional = true;
                            }
                            string nomCampo = campoAdicional.Attributes["nombre"].Value;
                            if (nomCampo == "AlmacOrigId") nomCampo = "ID Almacén Origen";
                            else if (nomCampo == "AlmacOrigDes") nomCampo = "Descripción Almacén Origen";
                            else if (nomCampo == "FechaPlanEntrega") nomCampo = "Fecha Planificada de Entrega";

                            if (nomCampo == "Compensación Solidaria")
                            {
                                if(campoAdicional.InnerText=="0.00" || campoAdicional.InnerText=="")
                                {
                                    nomCampo = "";
                                }
                            }
                            lblCodigo = new PdfPCell(new Paragraph(nomCampo + ":", detAdicional));
                            lblCodigo.Border = Rectangle.LEFT_BORDER;
                            lblCodigo.Padding = 2f;
                            Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText.Length > 150 ? campoAdicional.InnerText.Substring(0, 150) + "..." : campoAdicional.InnerText, detAdicional));
                            Codigo.Border = Rectangle.RIGHT_BORDER;
                            Codigo.Padding = 2f;
                            Codigo.HorizontalAlignment = Element.ALIGN_RIGHT;

                            tableInfoAdicional.AddCell(lblCodigo);
                            tableInfoAdicional.AddCell(Codigo);
                        }
                    }

                    if (FechaGeneraPortal != "" || NumPedidoCodBarra != "" || NumFacturaCodBarra != "")
                    {
                        if (AgregalblInfoAdicional)
                        {
                            lblCodigo = new PdfPCell(new Paragraph(" ", detAdicional));
                            lblCodigo.Border = Rectangle.BOTTOM_BORDER + Rectangle.TOP_BORDER;
                            lblCodigo.Padding = 2f;
                            lblCodigo.Colspan = 2;
                            tableInfoAdicional.AddCell(lblCodigo);
                        }
                        //fecha generacion
                        if (FechaGeneraPortal != "")
                        {
                            lblCodigo = new PdfPCell(new Paragraph("Fecha Generación en Portal:", detAdicional));
                            lblCodigo.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER;
                            lblCodigo.Padding = 2f;
                            lblCodigo.Colspan = 2;
                            lblCodigo.HorizontalAlignment = Element.ALIGN_CENTER;
                            tableInfoAdicional.AddCell(lblCodigo);

                            //iTextSharp.text.Font detAdicionalN = GetArialBlack(6);
                            lblCodigo = new PdfPCell(new Paragraph(FechaGeneraPortal, detAdicional));
                            lblCodigo.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                            lblCodigo.Padding = 2f;
                            lblCodigo.Colspan = 2;
                            lblCodigo.HorizontalAlignment = Element.ALIGN_CENTER;
                            tableInfoAdicional.AddCell(lblCodigo);
                        }
                        //numero pedido
                        if (NumPedidoCodBarra != "")
                        {
                            lblCodigo = new PdfPCell(new Paragraph("No.Pedido", detAdicional));
                            lblCodigo.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER;
                            lblCodigo.Padding = 2f;
                            lblCodigo.Colspan = 2;
                            lblCodigo.HorizontalAlignment = Element.ALIGN_CENTER;

                            Image img128_2 = BarcodeHelper.GetBarcode128(canvas, NumPedidoCodBarra, false, Barcode.CODE128);

                            PdfPCell ImgNumPedido = new PdfPCell(img128_2);
                            ImgNumPedido.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                            ImgNumPedido.Padding = 2f;
                            ImgNumPedido.Colspan = 2;
                            ImgNumPedido.HorizontalAlignment = Element.ALIGN_CENTER;

                            tableInfoAdicional.AddCell(lblCodigo);
                            tableInfoAdicional.AddCell(ImgNumPedido);
                        }
                        //secuencial de factura
                        if (NumFacturaCodBarra != "")
                        {
                            lblCodigo = new PdfPCell(new Paragraph("No.Factura", detAdicional));
                            lblCodigo.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER;
                            lblCodigo.Padding = 2f;
                            lblCodigo.Colspan = 2;
                            lblCodigo.HorizontalAlignment = Element.ALIGN_CENTER;

                            Image img128_3 = BarcodeHelper.GetBarcode128(canvas, NumFacturaCodBarra, false, Barcode.CODE128);

                            PdfPCell ImgNumFact = new PdfPCell(img128_3);
                            ImgNumFact.Border = Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                            ImgNumFact.Padding = 2f;
                            ImgNumFact.Colspan = 2;
                            ImgNumFact.HorizontalAlignment = Element.ALIGN_CENTER;

                            tableInfoAdicional.AddCell(lblCodigo);
                            tableInfoAdicional.AddCell(ImgNumFact);
                        }
                    }
                    else
                    {
                        if (AgregalblInfoAdicional)
                        {
                            tableInfoAdicional.AddCell(lblBottom);
                            tableInfoAdicional.AddCell(Bottom);
                        }
                    }
                    
                    #endregion


                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    decimal dpropina = 0;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        if (campoAdicional.Attributes["nombre"].Value == "propina")
                        {
                            dpropina = decimal.Parse(campoAdicional.InnerText,nfi);
                        }
                    }

                    #region Totales
                    var tableTotales = new PdfPTable(2);
                    tableTotales.TotalWidth = 180f;
                    float[] InfoTotales = new float[] { 105f, 75f };
                    tableTotales.SetWidths(InfoTotales);

                    XmlNodeList Impuestos;
                    Impuestos = oDocument.SelectNodes("//infoFactura/totalConImpuestos/totalImpuesto");
                    decimal dSubtotal12 = 0, dSubtotal0 = 0, dSubtotalNSI = 0, dICE = 0, dIVA12 = 0, dSubtotalExcento =0, dIRBPNR = 0;
                    foreach (XmlNode Impuesto in Impuestos)
                    {
                        switch (Impuesto["codigo"].InnerText)
                        {
                            case "2":        // IVA
                                if (Impuesto["codigoPorcentaje"].InnerText == "0") // 0%
                                {
                                    dSubtotal0 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "2")    // 12%
                                {
                                    dSubtotal12 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                    dIVA12 = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "3")    // 12%
                                {
                                    dSubtotal12 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                    dIVA12 = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "6")    // No objeto de Impuesto
                                {
                                    dSubtotalNSI = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "7")
                                {
                                    dSubtotalExcento = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                break;
                            case "3":       // ICE
                                dICE = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                                break;
                            case "5":
                                dIRBPNR = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                                 break;
                        }
                    }

                    XmlNodeList xmlRetenciones;
                    xmlRetenciones = oDocument.SelectNodes("//retenciones/retencion");
                    decimal dRenta = 0, dIVAPresuntivo = 0;
                    string tarRenta = "0.00", tarIVAPresuntivo = "0.00";

                    if (xmlRetenciones.Count > 0)
                    {
                        foreach (XmlNode retencion in xmlRetenciones)
                        {
                            if (int.Parse(retencion["codigoPorcentaje"].InnerText) > 7)
                            {
                                dRenta = decimal.Parse(retencion["valor"].InnerText, new CultureInfo(Cultura));
                                tarRenta = (decimal.Parse(retencion["tarifa"].InnerText, new CultureInfo(Cultura)) * 100).ToString(nfi);
                            }
                            else {
                                dIVAPresuntivo = decimal.Parse(retencion["valor"].InnerText, new CultureInfo(Cultura));
                                tarIVAPresuntivo = (decimal.Parse(retencion["tarifa"].InnerText, new CultureInfo(Cultura)) * 100).ToString(nfi);
                            }
                        }
                    }

                    iTextSharp.text.Font detalleVal = GetArial(11);

                    var lblSubTotal12 = new PdfPCell(new Paragraph("SUBTOTAL " + porcentaje + ".00%:", detalle));
                    lblSubTotal12.Padding = 2f;
                    var SubTotal12 = new PdfPCell(new Paragraph(dSubtotal12 == 0 ? "0.00" : dSubtotal12.ToString(nfi), detalleVal));
                    SubTotal12.Padding = 2f;
                    SubTotal12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotal0 = new PdfPCell(new Paragraph("SUBTOTAL 0%:", detalle));
                    lblSubTotal0.Padding = 2f;
                    var SubTotal0 = new PdfPCell(new Paragraph(dSubtotal0 == 0 ? "0.00" : dSubtotal0.ToString(nfi), detalleVal));
                    SubTotal0.Padding = 2f;
                    SubTotal0.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalNoSujetoIVA = new PdfPCell(new Paragraph("SUBTOTAL NO OBJETO IVA:", detalle));
                    lblSubTotalNoSujetoIVA.Padding = 2f;
                    var SubTotalNoSujetoIVA = new PdfPCell(new Paragraph(dSubtotalNSI == 0 ? "0.00" : dSubtotalNSI.ToString(nfi), detalleVal));
                    SubTotalNoSujetoIVA.Padding = 2f;
                    SubTotalNoSujetoIVA.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalExcentoIVA = new PdfPCell(new Paragraph("SUBTOTAL EXENTO IVA:", detalle));
                    lblSubTotalExcentoIVA.Padding = 2f;
                    var SubTotalExcentoIVA = new PdfPCell(new Paragraph(dSubtotalExcento == 0 ? "0.00" : dSubtotalExcento.ToString(nfi), detalleVal));
                    SubTotalExcentoIVA.Padding = 2f;
                    SubTotalExcentoIVA.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalSinImpuestos = new PdfPCell(new Paragraph("SUBTOTAL SIN IMPUESTOS:", detalle));
                    lblSubTotalSinImpuestos.Padding = 2f;
                    var SubTotalSinImpuestos = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/totalSinImpuestos").InnerText, detalleVal));
                    SubTotalSinImpuestos.Padding = 2f;
                    SubTotalSinImpuestos.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblDescuento = new PdfPCell(new Paragraph("TOTAL DESCUENTO:", detalle));
                    lblDescuento.Padding = 2f;
                    var TotalDescuento = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/totalDescuento").InnerText, detalleVal));
                    TotalDescuento.Padding = 2f;
                    TotalDescuento.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblICE = new PdfPCell(new Paragraph("ICE:", detalle));
                    lblICE.Padding = 2f;
                    var ICE = new PdfPCell(new Paragraph(dICE == 0 ? "0.00" : dICE.ToString(nfi), detalleVal));
                    ICE.Padding = 2f;
                    ICE.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblIVA12 = new PdfPCell(new Paragraph("IVA " + porcentaje + ".00%:", detalle));
                    lblIVA12.Padding = 2f;
                    var IVA12 = new PdfPCell(new Paragraph(dIVA12 == 0 ? "0.00" : dIVA12.ToString(nfi), detalleVal));
                    IVA12.Padding = 2f;
                    IVA12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblIRBPNR = new PdfPCell(new Paragraph("IRBPNR:", detalle));
                    lblIRBPNR.Padding = 2f;
                    var IRBPNR = new PdfPCell(new Paragraph(dIRBPNR == 0 ? "0.00" : dIRBPNR.ToString(nfi), detalleVal));
                    IRBPNR.Padding = 2f;
                    IRBPNR.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblPropina = new PdfPCell(new Paragraph("PROPINA:", detalle));
                    lblPropina.Padding = 2f;
                    var Propina = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoFactura/propina").InnerText, detalleVal));
                    Propina.Padding = 2f;
                    Propina.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblIVAPresuntivo = new PdfPCell(new Paragraph(String.Format("IVA PRESUNTIVO {0}%: ", tarIVAPresuntivo), detalle));
                    lblIVAPresuntivo.Padding = 2f;
                    var IVAPresuntivo = new PdfPCell(new Paragraph(decimal.Round(dIVAPresuntivo, 2).ToString(nfi), detalleVal));
                    IVAPresuntivo.Padding = 2f;
                    IVAPresuntivo.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblRenta = new PdfPCell(new Paragraph("RENTA:", detalle));
                    lblRenta.Padding = 2f;
                    var Renta = new PdfPCell(new Paragraph(decimal.Round(dRenta, 2).ToString(nfi), detalleVal));
                    Renta.Padding = 2f;
                    Renta.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblValorTotal = new PdfPCell(new Paragraph("VALOR TOTAL:", detalle));
                    lblValorTotal.Padding = 2f;
                   

                    var lblvalorsolidaria = new PdfPCell(new Paragraph("(-)DESC. SOLIDARIO 2% IVA:", detalle));
                    lblvalorsolidaria.Padding = 2f;

                    var datos ="0.00";
                    decimal aux=0;
                    XmlNodeList info;
                    info = oDocument.SelectNodes("//infoFactura/compensaciones");
                   
                    if (info.Count > 0)
                    {
                        foreach (XmlNode item in info)
                        {
                                if (item.SelectSingleNode("//compensacion/valor").InnerText != "")
                                {
                                    datos = item.SelectSingleNode("//compensacion/valor").InnerText;
                                    aux = Convert.ToDecimal(item.SelectSingleNode("//compensacion/valor").InnerText.ToString().Trim());    
                                }else
                                    datos = "0.00";
                        }
                    }else
                        datos="0.00";

                    var ValorPagarCompesacion = new PdfPCell(new Paragraph(datos, detalleVal));
                    ValorPagarCompesacion.Padding = 2f;
                    ValorPagarCompesacion.HorizontalAlignment = Rectangle.ALIGN_RIGHT;



                    decimal decImporteTotal =0, decTotal= 0;
                    decImporteTotal = decimal.Parse(oDocument.SelectSingleNode("//infoFactura/importeTotal").InnerText,nfi);
                    decTotal = decImporteTotal + dIVAPresuntivo + dRenta;

                    var lbltotalpagarcompesacion = new PdfPCell(new Paragraph("VALOR A PAGAR:", detalle));
                    lbltotalpagarcompesacion.Padding = 2f;

                    decimal resultado = 0;
                    decimal datosfinal=0;
                    if (datos!="0.00")
	                {
                        resultado = decImporteTotal ;
                        datosfinal = decImporteTotal + decimal.Parse(datos, nfi);
	                }else
                    {
                        datosfinal = decImporteTotal + decimal.Parse(datos, nfi);
                    }

                    var ValorTotal = new PdfPCell(new Paragraph(decimal.Round(datosfinal, 2).ToString(nfi), detalleVal));
                    ValorTotal.Padding = 2f;
                    ValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    var ValorPagarCompesaciontotal = new PdfPCell(new Paragraph(decimal.Round(resultado, 2).ToString(nfi), detalleVal));
                    ValorPagarCompesaciontotal.Padding = 2f;
                    ValorPagarCompesaciontotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;


                    var lblValorPagar = new PdfPCell(new Paragraph("VALOR A PAGAR:", detalle));
                    lblValorPagar.Padding = 2f;
                    var ValorPagar = new PdfPCell(new Paragraph(decimal.Round(decTotal, 2).ToString(nfi), detalleVal));
                    ValorPagar.Padding = 2f;
                    ValorPagar.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    var lblServicio= new PdfPCell(new Paragraph("SERVICIO 10%:", detalle));
                    lblValorTotal.Padding = 2f;
                    var Servicio = new PdfPCell(new Paragraph(dpropina.ToString(nfi), detalleVal));
                    Servicio.Padding = 2f;
                    Servicio.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    tableTotales.AddCell(lblSubTotal12);
                    tableTotales.AddCell(SubTotal12);
                    tableTotales.AddCell(lblSubTotal0);
                    tableTotales.AddCell(SubTotal0);
                    tableTotales.AddCell(lblSubTotalNoSujetoIVA);
                    tableTotales.AddCell(SubTotalNoSujetoIVA);
                    tableTotales.AddCell(lblSubTotalExcentoIVA);
                    tableTotales.AddCell(SubTotalExcentoIVA);
                    tableTotales.AddCell(lblSubTotalSinImpuestos);
                    tableTotales.AddCell(SubTotalSinImpuestos);
                    tableTotales.AddCell(lblDescuento);
                    tableTotales.AddCell(TotalDescuento);
                    tableTotales.AddCell(lblICE);
                    tableTotales.AddCell(ICE);
                    tableTotales.AddCell(lblIVA12);
                    tableTotales.AddCell(IVA12);
                    tableTotales.AddCell(lblIRBPNR);
                    tableTotales.AddCell(IRBPNR);
                    tableTotales.AddCell(lblPropina);
                    tableTotales.AddCell(Propina);
                    tableTotales.AddCell(lblValorTotal);
                    tableTotales.AddCell(ValorTotal);
                    if (datos != "0.00")
                    {
                        tableTotales.AddCell(lblvalorsolidaria);
                        tableTotales.AddCell(ValorPagarCompesacion);
                        tableTotales.AddCell(lbltotalpagarcompesacion);
                        tableTotales.AddCell(ValorPagarCompesaciontotal);
    
                    }
                    if (xmlRetenciones.Count > 0)
                    {
                        tableTotales.AddCell(lblIVAPresuntivo);
                        tableTotales.AddCell(IVAPresuntivo);
                        tableTotales.AddCell(lblRenta);
                        tableTotales.AddCell(Renta);
                        tableTotales.AddCell(lblValorPagar);
                        tableTotales.AddCell(ValorPagar);
                    }
                    if (dpropina > 0)
                    {
                        tableTotales.AddCell(lblServicio);
                        tableTotales.AddCell(Servicio);
                    }

                    #endregion


                    // titulo inicial del documento
                    iTextSharp.text.Font tituloPrinc = GetArialBlack(16);
                    tituloPrinc.Color = BaseColor.RED;
                    PdfPCell TITULO_PRINC = new PdfPCell(new Paragraph("REPRESENTACIÓN IMPRESA NO ELECTRÓNICA (RINE)", tituloPrinc));
                    TITULO_PRINC.Border = Rectangle.NO_BORDER;
                    TITULO_PRINC.Padding = 4f;
                    TITULO_PRINC.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    iTextSharp.text.Font subTituloPrinc = GetArialBlack(12);
                    subTituloPrinc.SetColor(255, 128, 0);
                    PdfPCell SUBTIT_PRINC = new PdfPCell(new Paragraph("DOCUMENTO NO TRIBUTABLE", subTituloPrinc));
                    SUBTIT_PRINC.Border = Rectangle.NO_BORDER;
                    SUBTIT_PRINC.Padding = 3f;
                    SUBTIT_PRINC.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    PdfPTable tableTitPrinc = new PdfPTable(1);
                    tableTitPrinc.AddCell(TITULO_PRINC);
                    tableTitPrinc.AddCell(SUBTIT_PRINC);
                    tableTitPrinc.TotalWidth = 540f;
                    tableTitPrinc.SpacingAfter = PdfPTable.LINECANVAS;


                    tableTitPrinc.WriteSelectedRows(0, 2, 28, 816, canvas);
                    float newLine = -50;


                    tableR.WriteSelectedRows(0, 15, 292, 806 + newLine, canvas);
                    tableLOGO.WriteSelectedRows(0, 1, 30, 780 + newLine, canvas);
                    posDetalleCliente = 796 - tableR.TotalHeight;
                    posDetalleFactura = (posDetalleCliente - 8) - tableDetalleCliente.TotalHeight;

                    tableDetalleCliente.WriteSelectedRows(0, 5, 28, posDetalleCliente + newLine, canvas);
                    
                    if (sSucursal.Length > 40 || sMatriz.Length > 40)
                    {
                        tableL.WriteSelectedRows(0, 5, 28, 705 + newLine, canvas);
                    }
                    else {
                        tableL.WriteSelectedRows(0, 5, 28, 697 + newLine, canvas);
                    }
                    
                    if (registros <= PagLimite1)    // Una sola página
                    {
                        oEvents.Contador = 1;
                        //tableDetalleFactura.WriteSelectedRows(0, PagLimite1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight + newLine, canvas);
                        posInfoAdicional = (posDetalleFactura - 10) - tableDetalleFactura.TotalHeight;
                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInfoAdicional + newLine, canvas);
                        tableTotales.WriteSelectedRows(0, 15, 388, posInfoAdicional + newLine, canvas);

                    }
                    else if (registros > PagLimite1 && registros <= MaxPagina1)  // Una sola página con detalle en la siguiente.
                    {
                        oEvents.Contador = 2;
                        //tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight + newLine, canvas);
                        documento.NewPage();

                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806 + newLine, canvas);
                        tableTotales.WriteSelectedRows(0, 15, 388, 806 + newLine, canvas);
                    }
                    else
                    {
                        //tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight + newLine, canvas);

                        decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));
                        oEvents.Contador = Convert.ToInt32(Paginas + 1) ;
                        documento.NewPage();
                        float posInicial = 0;
                        int faltantes = 0, ultimo = 0, hasta = 0;
                        ultimo = MaxPagina1 + 1;
                        hasta = MaxPagina1 + MaxSoloPagina + 1;
                        faltantes = registros - MaxPagina1 + 1;
                        for (int i = 0; i < Paginas; i++)
                        {
                            posInicial = 0;
                            documento.NewPage();
                            //tableDetalleFactura.WriteSelectedRows(ultimo, hasta, 28, 806 + newLine, canvas);
                            ultimo = hasta;
                            hasta = ultimo + MaxSoloPagina;
                            if (faltantes > MaxSoloPagina){
                                faltantes = faltantes - (hasta - ultimo);
                            }
                        }

                        posInicial = (806 - (faltantes * 11)) - 20;

                        if (posInicial > 120){
                            tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInicial + 10 + newLine, canvas);
                            tableTotales.WriteSelectedRows(0, 15, 388, posInicial + 10 + newLine, canvas);
                        }
                        else{
                            tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806 + newLine, canvas);
                            tableTotales.WriteSelectedRows(0, 15, 388, 806 + newLine, canvas);
                        }
                    }
                    writer.CloseStream = false;
                    documento.Close();
                    ms.Position = 0;
                    Bytes = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                //var oLog = new LogWriter();
                //oLog.RegistraLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName + "." + MethodBase.GetCurrentMethod().Name,
                //    "Error al generar PDF.", ex);
                throw ex;
            }
            return Bytes;
        }


        /// <summary>
        /// Método para generación de guía de remisión RIDE
        /// </summary>
        /// <param name="pDocumento_autorizado">Documento Autorizado</param>
        /// <param name="pRutaLogo">Ruta física o URL del logo a imprimir</param>
        /// <param name="pCultura">Cultura, por defecto en-US</param>
        /// <returns>Arreglo de bytes</returns>
        public byte[] GeneraGuiaRemision(string pDocumento_autorizado, string pRutaLogo, string pCultura = "en-US")
        {
            iTextSharp.text.Font detAdicional = GetArial(6);
            MemoryStream ms = null;
            byte[] Bytes = null;
            try
            {
                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    //clsParametros oParametros = new clsParametros();
                    XmlNode xmlAutorizaciones;
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sRuc = "", sContribuyenteEspecial = "",
                           sAmbienteVal = "", sNumAutorizacion = "";
                    Cultura = pCultura;
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";


                    oDocument.LoadXml(pDocumento_autorizado);
                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    sSucursal = (oDocument.SelectSingleNode("//infoGuiaRemision/dirEstablecimiento") == null) ? "" : oDocument.SelectSingleNode("//infoGuiaRemision/dirEstablecimiento").InnerText;
                    sContribuyenteEspecial = oDocument.SelectSingleNode("//infoGuiaRemision/contribuyenteEspecial") == null ? "6925" : oDocument.SelectSingleNode("//infoGuiaRemision/contribuyenteEspecial").InnerText;
 

                    int registros = 0;
                    int PagLimite1 = 35;
                    int MaxPagina1 = 45;
                    int MaxSoloPagina = 70;

                    float posDetalleTransportista = 0;
                    float posDetalleGuia = 0;
                    float posInfoAdicional = 0;

                    PdfWriter writer;
                    RoundRectangle rr = new RoundRectangle();
                    //Creamos un tipo de archivo que solo se cargará en la memoria principal
                    Document documento = new Document();

                    writer = PdfWriter.GetInstance(documento, ms);

                    iTextSharp.text.Font cabecera = GetArial(8);
                    iTextSharp.text.Font detalle = GetArial(7);

                    documento.Open();
                    var oEvents = new ITextEvents();
                    writer.PageEvent = oEvents;
                    PdfContentByte canvas = writer.DirectContent;
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(pRutaLogo);
                    jpg.ScaleToFit(250f, 70f);

                    #region TablaDerecha
                    PdfPTable tableR = new PdfPTable(1);
                    PdfPTable innerTableD = new PdfPTable(1);

                    PdfPCell RUC = new PdfPCell(new Paragraph("R.U.C.: " + sRuc, cabecera));
                    RUC.Border = Rectangle.NO_BORDER;
                    RUC.Padding = 5f;
                    innerTableD.AddCell(RUC);

                    PdfPCell Factura = new PdfPCell(new Paragraph("G U I A  D E  R E M I S I Ó N ", cabecera));
                    Factura.Border = Rectangle.NO_BORDER;
                    Factura.Padding = 5f;
                    innerTableD.AddCell(Factura);

                    PdfPCell NumFactura = new PdfPCell(new Paragraph("No. " + oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabecera));
                    NumFactura.Border = Rectangle.NO_BORDER;
                    NumFactura.Padding = 5f;
                    innerTableD.AddCell(NumFactura);

                    PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabecera));
                    lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                    lblNumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(lblNumAutorizacion);

                    PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabecera));
                    NumAutorizacion.Border = Rectangle.NO_BORDER;
                    NumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(NumAutorizacion);

                    PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA Y HORA AUTORIZACIÓN: " + sFechaAutorizacion, cabecera));
                    FechaAutorizacion.Border = Rectangle.NO_BORDER;
                    FechaAutorizacion.Padding = 5f;
                    innerTableD.AddCell(FechaAutorizacion);

                    PdfPCell Ambiente = new PdfPCell(new Paragraph("AMBIENTE: " + sAmbiente, cabecera));
                    Ambiente.Border = Rectangle.NO_BORDER;
                    Ambiente.Padding = 5f;
                    innerTableD.AddCell(Ambiente);

                    PdfPCell Emision = new PdfPCell(new Paragraph("EMISIÓN: " + sTipoEmision, cabecera));
                    Emision.Border = Rectangle.NO_BORDER;
                    Emision.Padding = 5f;
                    innerTableD.AddCell(Emision);

                    PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CLAVE DE ACCESO: ", cabecera));
                    ClaveAcceso.Border = Rectangle.NO_BORDER;
                    ClaveAcceso.Padding = 5f;
                    innerTableD.AddCell(ClaveAcceso);

                    Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);

                    PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                    ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                    ImgClaveAcceso.Padding = 5f;
                    ImgClaveAcceso.Colspan = 2;
                    ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;

                    innerTableD.AddCell(ImgClaveAcceso);

                    var ContenedorD = new PdfPCell(innerTableD);
                    ContenedorD.CellEvent = rr;
                    ContenedorD.Border = Rectangle.NO_BORDER;
                    tableR.AddCell(ContenedorD);
                    tableR.TotalWidth = 278f;
                    #endregion

                    #region TablaIzquierda
                    PdfPTable tableL = new PdfPTable(1);
                    PdfPTable innerTableL = new PdfPTable(1);

                    PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                    RazonSocial.Border = Rectangle.NO_BORDER;
                    RazonSocial.Padding = 5f;
                    innerTableL.AddCell(RazonSocial);

                    PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dir Matriz: " + sMatriz, cabecera));
                    DirMatriz.Border = Rectangle.NO_BORDER;
                    DirMatriz.Padding = 5f;
                    innerTableL.AddCell(DirMatriz);

                    PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dir Sucursal: " + sSucursal, cabecera));
                    DirSucursal.Border = Rectangle.NO_BORDER;
                    DirSucursal.Padding = 5f;
                    innerTableL.AddCell(DirSucursal);

                    PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro: " + sContribuyenteEspecial, cabecera));
                    ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                    ContribuyenteEspecial.Padding = 5f;
                    innerTableL.AddCell(ContribuyenteEspecial);

                    //PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD: SI", cabecera));
                    //ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                    //ObligadoContabilidad.Padding = 5f;
                    //innerTableL.AddCell(ObligadoContabilidad);

                    PdfPCell FechaInicioTransporte = new PdfPCell(new Paragraph("Fecha Inicio de Transporte: " + oDocument.SelectSingleNode("//infoGuiaRemision/fechaIniTransporte").InnerText, cabecera));
                    FechaInicioTransporte.Border = Rectangle.NO_BORDER;
                    FechaInicioTransporte.Padding = 5f;
                    innerTableL.AddCell(FechaInicioTransporte);

                    PdfPCell FechaFinTransporte = new PdfPCell(new Paragraph("Fecha Fin de Transporte: " + oDocument.SelectSingleNode("//infoGuiaRemision/fechaFinTransporte").InnerText, cabecera));
                    FechaFinTransporte.Border = Rectangle.NO_BORDER;
                    FechaFinTransporte.Padding = 5f;
                    innerTableL.AddCell(FechaFinTransporte);

                    var ContenedorL = new PdfPCell(innerTableL);
                    ContenedorL.CellEvent = rr;
                    ContenedorL.Border = Rectangle.NO_BORDER;
                    tableL.AddCell(ContenedorL);
                    tableL.TotalWidth = 250f;

                    #endregion

                    #region Logo
                    PdfPTable tableLOGO = new PdfPTable(1);
                    PdfPCell logo = new PdfPCell(jpg);
                    logo.Border = Rectangle.NO_BORDER;
                    tableLOGO.AddCell(logo);
                    tableLOGO.TotalWidth = 250f;
                    #endregion

                    #region DetalleTransportista
                    PdfPTable tableDetalleTransportista = new PdfPTable(4);
                    tableDetalleTransportista.TotalWidth = 540f;
                    tableDetalleTransportista.WidthPercentage = 100;
                    float[] DetalleTransportistawidths = new float[] { 30f, 90f, 20f, 90f };
                    tableDetalleTransportista.SetWidths(DetalleTransportistawidths);

                    var lblRUCTransportista = new PdfPCell(new Paragraph("RUC/CI (Transportista):", detalle));
                    lblRUCTransportista.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableDetalleTransportista.AddCell(lblRUCTransportista);
                    var RUCTransportista = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoGuiaRemision/rucTransportista").InnerText, detalle));
                    RUCTransportista.Border = Rectangle.TOP_BORDER;
                    tableDetalleTransportista.AddCell(RUCTransportista);
                    var lblPlaca = new PdfPCell(new Paragraph("Placa:", detalle));
                    lblPlaca.Border = Rectangle.TOP_BORDER;
                    tableDetalleTransportista.AddCell(lblPlaca);
                    var Placa = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoGuiaRemision/placa").InnerText, detalle));
                    Placa.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableDetalleTransportista.AddCell(Placa);

                    var lblRazonSocial = new PdfPCell(new Paragraph("Razon Social / Nombres y Apellidos:", detalle));
                    lblRazonSocial.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableDetalleTransportista.AddCell(lblRazonSocial);

                    var RazonSocialTransportista = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoGuiaRemision/razonSocialTransportista").InnerText, detalle));
                    RazonSocialTransportista.Border = Rectangle.BOTTOM_BORDER;
                    tableDetalleTransportista.AddCell(RazonSocialTransportista);
                    var lblPuntoPartida = new PdfPCell(new Paragraph("Punto de partida:", detalle));
                    lblPuntoPartida.Border = Rectangle.BOTTOM_BORDER;
                    lblPuntoPartida.PaddingBottom = 5f;
                    tableDetalleTransportista.AddCell(lblPuntoPartida);
                    var PuntoPartida = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoGuiaRemision/dirPartida").InnerText, detalle));
                    PuntoPartida.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                    PuntoPartida.PaddingBottom = 5f;
                    tableDetalleTransportista.AddCell(PuntoPartida);
                    #endregion

                    #region DetalleFactura
                    PdfPTable tableDetalleFactura = new PdfPTable(4);

                    tableDetalleFactura.TotalWidth = 540f;
                    tableDetalleFactura.WidthPercentage = 100;
                    float[] DetalleFacturawidths = new float[] { 30f, 180f, 50f, 50f }; //, 30f};
                    tableDetalleFactura.SetWidths(DetalleFacturawidths);

                    var fontEncabezado = GetArial(7);

                    var encCant = new PdfPCell(new Paragraph("Cant.", fontEncabezado));
                    encCant.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    var encDescripcion = new PdfPCell(new Paragraph("Descripción", fontEncabezado));
                    encDescripcion.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    var encCodPrincipal = new PdfPCell(new Paragraph("Cod. Principal", fontEncabezado));
                    encCodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    var encCodAuxiliar = new PdfPCell(new Paragraph("Cod. Auxiliar", fontEncabezado));
                    encCodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    tableDetalleFactura.AddCell(encCant);
                    tableDetalleFactura.AddCell(encDescripcion);
                    tableDetalleFactura.AddCell(encCodPrincipal);
                    tableDetalleFactura.AddCell(encCodAuxiliar);
                
                    PdfPCell CodPrincipal;
                    PdfPCell CodAuxiliar;
                    PdfPCell Cant;
                    PdfPCell Descripcion;

                    XmlNodeList Detalles;
                    Detalles = oDocument.SelectNodes("//destinatarios/destinatario/detalles/detalle");
                    registros = Detalles.Count;

                    foreach (XmlNode Elemento in Detalles)
                    {
                        CodPrincipal = new PdfPCell(new Phrase(Elemento["codigoInterno"].InnerText, detalle));
                        CodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        Cant = new PdfPCell(new Phrase(Elemento["cantidad"].InnerText, detalle));
                        Cant.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        Descripcion = new PdfPCell(new Phrase(Elemento["descripcion"].InnerText, detalle));
                        CodAuxiliar = new PdfPCell(new Phrase(Elemento["codigoAdicional"] == null ? "" : Elemento["codigoAdicional"].InnerText, detalle));
                        CodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                        tableDetalleFactura.AddCell(Cant);
                        tableDetalleFactura.AddCell(Descripcion);
                        tableDetalleFactura.AddCell(CodPrincipal);
                        tableDetalleFactura.AddCell(CodAuxiliar);
                    }
                    #endregion

                    #region CabeceraGuia
                    PdfPTable tableCabeceraGuia = new PdfPTable(2);
                    tableCabeceraGuia.TotalWidth = 540f;
                    tableCabeceraGuia.WidthPercentage = 100;
                    tableCabeceraGuia.LockedWidth = true;
                    float[] DetalleGuiaWidths = new float[] { 80f, 300f};
                    tableCabeceraGuia.SetWidths(DetalleGuiaWidths);

                    var lblTOP = new PdfPCell(new Paragraph("", detalle));
                    lblTOP.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableCabeceraGuia.AddCell(lblTOP);

                    var TOP = new PdfPCell(new Paragraph("", detalle));
                    TOP.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableCabeceraGuia.AddCell(TOP);

                    //var lblComprobanteVenta = new PdfPCell(new Paragraph("Comprobante de venta:", detalle));
                    //lblComprobanteVenta.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    //tableCabeceraGuia.AddCell(lblComprobanteVenta);
                    //var ComprobanteVenta = new PdfPCell(new Paragraph("", detalle));
                    //ComprobanteVenta.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    //tableCabeceraGuia.AddCell(ComprobanteVenta);
                    //var lblFechaEmision = new PdfPCell(new Paragraph("Fecha de emisión:", detalle));
                    //lblFechaEmision.Border = Rectangle.LEFT_BORDER;
                    //tableCabeceraGuia.AddCell(lblFechaEmision);
                    //var FechaEmision = new PdfPCell(new Paragraph("", detalle));
                    //FechaEmision.Border = Rectangle.RIGHT_BORDER;
                    //tableCabeceraGuia.AddCell(FechaEmision);

                    //var lblNumeroAutorizacion = new PdfPCell(new Paragraph("Número de autorización:", detalle));
                    //lblNumeroAutorizacion.Border = Rectangle.LEFT_BORDER ;
                    //tableCabeceraGuia.AddCell(lblNumeroAutorizacion);
                    //var NumeroAutorizacion = new PdfPCell(new Paragraph("", detalle));
                    //NumeroAutorizacion.Border = Rectangle.RIGHT_BORDER;
                    //tableCabeceraGuia.AddCell(NumeroAutorizacion);

                    var lblMotivoTraslado = new PdfPCell(new Paragraph("Motivo Traslado:", detalle));
                    lblMotivoTraslado.Border = Rectangle.LEFT_BORDER;
                    tableCabeceraGuia.AddCell(lblMotivoTraslado);
                    var MotivoTraslado = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//destinatarios/destinatario/motivoTraslado").InnerText, detalle));
                    MotivoTraslado.Border = Rectangle.RIGHT_BORDER ;
                    tableCabeceraGuia.AddCell(MotivoTraslado);

                    var lblRUCDestinatario = new PdfPCell(new Paragraph("RUC / CI (Destinatario):", detalle));
                    lblRUCDestinatario.Border = Rectangle.LEFT_BORDER;
                    tableCabeceraGuia.AddCell(lblRUCDestinatario);
                    var RUCDestinatario = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//destinatarios/destinatario/identificacionDestinatario").InnerText, detalle));
                    RUCDestinatario.Border = Rectangle.RIGHT_BORDER;
                    tableCabeceraGuia.AddCell(RUCDestinatario);

                    var lblRazonSocialDestinatario = new PdfPCell(new Paragraph("Razón social / Nombres Apellidos:", detalle));
                    lblRazonSocialDestinatario.Border = Rectangle.LEFT_BORDER;
                    tableCabeceraGuia.AddCell(lblRazonSocialDestinatario);
                    var RazonSocialDestinatario = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//destinatarios/destinatario/razonSocialDestinatario").InnerText, detalle));
                    RazonSocialDestinatario.Border = Rectangle.RIGHT_BORDER;
                    tableCabeceraGuia.AddCell(RazonSocialDestinatario);

                    //var lblDocumentoAduanero = new PdfPCell(new Paragraph("Documento Aduanero:", detalle));
                    //lblDocumentoAduanero.Border = Rectangle.LEFT_BORDER;
                    //tableCabeceraGuia.AddCell(lblDocumentoAduanero);
                    //var DocumentoAduanero = new PdfPCell(new Paragraph("", detalle));
                    //DocumentoAduanero.Border = Rectangle.RIGHT_BORDER;
                    //tableCabeceraGuia.AddCell(DocumentoAduanero);

                    var lblCodEstablecimiento = new PdfPCell(new Paragraph("Código Establecimiento Destino:", detalle));
                    lblCodEstablecimiento.Border = Rectangle.LEFT_BORDER;
                    tableCabeceraGuia.AddCell(lblCodEstablecimiento);
                    var CodEstablecimiento = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//destinatarios/destinatario/codEstabDestino") == null ? "" : oDocument.SelectSingleNode("//destinatarios/destinatario/codEstabDestino").InnerText, detalle));
                    CodEstablecimiento.Border = Rectangle.RIGHT_BORDER;
                    tableCabeceraGuia.AddCell(CodEstablecimiento);

                    var lblDestino = new PdfPCell(new Paragraph("Destino (Punto de llegada):", detalle));
                    lblDestino.Border = Rectangle.LEFT_BORDER;
                    tableCabeceraGuia.AddCell(lblDestino);
                    var Destino = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//destinatarios/destinatario/dirDestinatario").InnerText, detalle));
                    Destino.Border = Rectangle.RIGHT_BORDER;
                    tableCabeceraGuia.AddCell(Destino);

                    //var lblRuta = new PdfPCell(new Paragraph("Ruta:", detalle));
                    //lblRuta.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    //lblRuta.PaddingBottom = 5f;
                    //tableCabeceraGuia.AddCell(lblRuta);
                    //var Ruta = new PdfPCell(new Paragraph("", detalle));
                    //Ruta.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                    //Ruta.PaddingBottom = 5f;
                    //tableCabeceraGuia.AddCell(Ruta);

                    var lblBOTTOM = new PdfPCell(new Paragraph("", detalle));
                    lblBOTTOM.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableCabeceraGuia.AddCell(lblBOTTOM);

                    var BOTTOM = new PdfPCell(new Paragraph("", detalle));
                    BOTTOM.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                    tableCabeceraGuia.AddCell(BOTTOM);
                    #endregion

                    #region InformacionAdicional
                    var tableInfoAdicional = new PdfPTable(2);
                    tableInfoAdicional.TotalWidth = 250f;
                    float[] InfoAdicionalWidths = new float[] { 65f, 170f };
                    tableInfoAdicional.SetWidths(InfoAdicionalWidths);

                    var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                    lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    lblInfoAdicional.Colspan = 2;
                    lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblInfoAdicional.Padding = 5f;
                    tableInfoAdicional.AddCell(lblInfoAdicional);

                    var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottom.Padding = 2f;
                    var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                    Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottom.Padding = 2f;

                    XmlNodeList InfoAdicional;
                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    PdfPCell lblCodigo;
                    PdfPCell Codigo;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        lblCodigo = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value, detAdicional));
                        lblCodigo.Border = Rectangle.LEFT_BORDER;
                        lblCodigo.Padding = 2f;

                        Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText, detAdicional));
                        Codigo.Border = Rectangle.RIGHT_BORDER;
                        Codigo.Padding = 2f;

                        tableInfoAdicional.AddCell(lblCodigo);
                        tableInfoAdicional.AddCell(Codigo);
                    }

                    tableInfoAdicional.AddCell(lblBottom);
                    tableInfoAdicional.AddCell(Bottom);

                    #endregion

                    tableR.WriteSelectedRows(0, 15, 292, 806, canvas);
                    tableLOGO.WriteSelectedRows(0, 1, 30, 780, canvas);
                    posDetalleTransportista = 796 - tableR.TotalHeight;
                    posDetalleGuia = (posDetalleTransportista - 8) - tableDetalleTransportista.TotalHeight;

                    tableDetalleTransportista.WriteSelectedRows(0, 5, 28, posDetalleTransportista, canvas);
                    tableL.WriteSelectedRows(0, 5, 28, 715, canvas);

                    if (registros <= PagLimite1)    // Una sola página 
                    {
                        oEvents.Contador = 1;
                        tableDetalleFactura.WriteSelectedRows(0, PagLimite1 + 1, 28, posDetalleTransportista - 10 - tableDetalleTransportista.TotalHeight, canvas);
                        posInfoAdicional = (posDetalleGuia - 10) - tableDetalleFactura.TotalHeight;
                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInfoAdicional, canvas);
                        //tableTotales.WriteSelectedRows(0, 10, 408, posInfoAdicional, canvas);

                    }
                    else if (registros > PagLimite1 && registros <= MaxPagina1)  // Una sola página con detalle en la siguiente.
                    {
                        oEvents.Contador = 2;
                        tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleTransportista - 10 - tableDetalleTransportista.TotalHeight, canvas);
                        documento.NewPage();

                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806, canvas);
                    }
                    else
                    {
                        tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleTransportista - 10 - tableDetalleTransportista.TotalHeight, canvas);

                        decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));
                        oEvents.Contador = Convert.ToInt32(Paginas + 1);
                        documento.NewPage();
                        float posInicial = 0;
                        int faltantes = 0, ultimo = 0, hasta = 0;
                        ultimo = MaxPagina1 + 1;
                        hasta = MaxPagina1 + MaxSoloPagina + 1;
                        faltantes = registros - MaxPagina1 + 1;
                        for (int i = 0; i < Paginas; i++)
                        {
                            posInicial = 0;
                            documento.NewPage();
                            tableDetalleFactura.WriteSelectedRows(ultimo, hasta, 28, 806, canvas);
                            ultimo = hasta;
                            hasta = ultimo + MaxSoloPagina;
                            if (faltantes > MaxSoloPagina)
                            {
                                faltantes = faltantes - (hasta - ultimo);
                            }
                        }

                        posInicial = (806 - (faltantes * 11)) - 20;

                        if (posInicial > 120)
                        {
                            tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInicial + 10, canvas);
                            //tableTotales.WriteSelectedRows(0, 10, 408, posInicial + 10, canvas);
                        }
                        else
                        {
                            tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806, canvas);
                            //tableTotales.WriteSelectedRows(0, 10, 408, 806, canvas);
                        }

                        //tableL.WriteSelectedRows(0, 5, 28, 712, canvas);
                        //tableCabeceraGuia.WriteSelectedRows(0, 11, 28, posDetalleTransportista - 10 - tableDetalleTransportista.TotalHeight, canvas);
                        //posDetalleGuia = (posDetalleGuia - 10) - tableCabeceraGuia.TotalHeight;
                        //tableDetalleFactura.WriteSelectedRows(0, (PagLimite1 + 1), 28, posDetalleGuia, canvas);
                        //documento.NewPage();
                        ////tableDetalleFactura.WriteSelectedRows((PagLimite1 + 1), registros, 28, 806, canvas);


                        //if (registros > PagLimite1)
                        //{
                        //    //tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleTransportista - 10 - tableDetalleTransportista.TotalHeight, canvas);
                        //    //documento.NewPage();

                        //    decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));
                        //    int faltantes = 0, ultimo = 0, hasta = 0;
                        //    ultimo = MaxPagina1 + 1;
                        //    hasta = MaxPagina1 + MaxSoloPagina + 1;
                        //    faltantes = registros - MaxPagina1 + 1;
                        //    for (int i = 0; i < Paginas; i++)
                        //    {
                        //        documento.NewPage();
                        //        tableDetalleFactura.WriteSelectedRows(ultimo, hasta, 28, 806, canvas);
                        //        ultimo = hasta;
                        //        hasta = ultimo + MaxSoloPagina;
                        //        if (faltantes > MaxSoloPagina)
                        //        {
                        //            faltantes = faltantes - (hasta - ultimo);
                        //        }
                        //    }
                        //}
                    }
                    writer.CloseStream = false;
                    documento.Close();
                    ms.Position = 0;
                    Bytes = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                //var oLog = new LogWriter();
                //oLog.RegistraLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName + "." + MethodBase.GetCurrentMethod().Name,
                //    "Error al generar PDF.", ex);
                throw ex;
            }
            return Bytes;
        }

        /// <summary>
        /// Método para generación de nota de crédito RIDE
        /// </summary>
        /// <param name="pDocumento_autorizado">Documento Autorizado</param>
        /// <param name="pRutaLogo">Ruta física o URL del logo a imprimir</param>
        /// <param name="pCultura">Cultura, por defecto en-US</param>
        /// <returns>Arreglo de bytes</returns>
        public byte[] GeneraNotaCredito(string pDocumento_autorizado, string pRutaLogo, string pCultura = "en-US")
        {
            MemoryStream ms = null;
            byte[] Bytes = null;
            iTextSharp.text.Font detAdicional = GetArial(6);
            try
            {

                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    XmlNode xmlAutorizaciones;
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sContribuyenteEspecial= "", sRuc="",
                           sAmbienteVal = "", sNumAutorizacion = "", sObligadoContabilidad = "",
                           sComprobanteModificacion ="";
                    Cultura = pCultura;
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";
                    

                    oDocument.LoadXml(pDocumento_autorizado);

                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    sAmbiente = oDocument.SelectSingleNode("//ambiente") == null ? sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN" : oDocument.SelectSingleNode("//ambiente").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    sSucursal = (oDocument.SelectSingleNode("//infoNotaCredito/dirEstablecimiento") == null) ? "" : oDocument.SelectSingleNode("//infoNotaCredito/dirEstablecimiento").InnerText;
                    sContribuyenteEspecial = (oDocument.SelectSingleNode("//infoNotaCredito/contribuyenteEspecial") == null) ? "" : oDocument.SelectSingleNode("//infoNotaCredito/contribuyenteEspecial").InnerText;
                    sObligadoContabilidad = oDocument.SelectSingleNode("//infoNotaCredito/obligadoContabilidad") == null ? "" : oDocument.SelectSingleNode("//infoNotaCredito/obligadoContabilidad").InnerText;


                    int registros = 200;
                    int PagLimite1 = 30;
                    int MaxPagina1 = 39;
                    int MaxSoloPagina = 70;

                    float posDetalleCliente = 0;
                    float posDetalleFactura = 0;
                    float posInfoAdicional = 0;

                    PdfWriter writer;
                    RoundRectangle rr = new RoundRectangle();
                    //Creamos un tipo de archivo que solo se cargará en la memoria principal
                    Document documento = new Document();
                    //Creamos la instancia para generar el archivo PDF
                    writer = PdfWriter.GetInstance(documento, ms);

                    iTextSharp.text.Font cabecera = GetArial(8);
                    iTextSharp.text.Font detalle = GetArial(7);
                   

                    documento.Open();
                    var oEvents = new ITextEvents();
                    writer.PageEvent = oEvents;
                    PdfContentByte canvas = writer.DirectContent;
                    iTextSharp.text.Image jpg = null;

                    jpg = iTextSharp.text.Image.GetInstance(pRutaLogo);
                    jpg.ScaleToFit(250f, 70f);
                    #region TablaDerecha
                    PdfPTable tableR = new PdfPTable(1);
                    PdfPTable innerTableD = new PdfPTable(1);

                    PdfPCell RUC = new PdfPCell(new Paragraph("R.U.C.: " + sRuc, cabecera));
                    RUC.Border = Rectangle.NO_BORDER;
                    RUC.Padding = 5f;
                    innerTableD.AddCell(RUC);

                    PdfPCell Factura = new PdfPCell(new Paragraph("N O T A  D E  C R É D I T O ", cabecera));
                    Factura.Border = Rectangle.NO_BORDER;
                    Factura.Padding = 5f;
                    innerTableD.AddCell(Factura);

                    PdfPCell NumFactura = new PdfPCell(new Paragraph("No. " + oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabecera));
                    NumFactura.Border = Rectangle.NO_BORDER;
                    NumFactura.Padding = 5f;
                    innerTableD.AddCell(NumFactura);

                    PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabecera));
                    lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                    lblNumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(lblNumAutorizacion);

                    PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabecera));
                    NumAutorizacion.Border = Rectangle.NO_BORDER;
                    NumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(NumAutorizacion);

                    PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA Y HORA AUTORIZACIÓN: " + sFechaAutorizacion, cabecera));
                    FechaAutorizacion.Border = Rectangle.NO_BORDER;
                    FechaAutorizacion.Padding = 5f;
                    innerTableD.AddCell(FechaAutorizacion);

                    PdfPCell Ambiente = new PdfPCell(new Paragraph("AMBIENTE: " + sAmbiente, cabecera));
                    Ambiente.Border = Rectangle.NO_BORDER;
                    Ambiente.Padding = 5f;
                    innerTableD.AddCell(Ambiente);

                    PdfPCell Emision = new PdfPCell(new Paragraph("EMISIÓN: " + sTipoEmision, cabecera));
                    Emision.Border = Rectangle.NO_BORDER;
                    Emision.Padding = 5f;
                    innerTableD.AddCell(Emision);

                    PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CLAVE DE ACCESO: ", cabecera));
                    ClaveAcceso.Border = Rectangle.NO_BORDER;
                    ClaveAcceso.Padding = 5f;
                    innerTableD.AddCell(ClaveAcceso);

                    Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);

                    PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                    ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                    ImgClaveAcceso.Padding = 5f;
                    ImgClaveAcceso.Colspan = 2;
                    ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;

                    innerTableD.AddCell(ImgClaveAcceso);

                    var ContenedorD = new PdfPCell(innerTableD);
                    ContenedorD.CellEvent = rr;
                    ContenedorD.Border = Rectangle.NO_BORDER;
                    tableR.AddCell(ContenedorD);
                    tableR.TotalWidth = 278f;
                    #endregion

                    #region TablaIzquierda
                    PdfPTable tableL = new PdfPTable(1);
                    PdfPTable innerTableL = new PdfPTable(1);

                    PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                    RazonSocial.Border = Rectangle.NO_BORDER;
                    RazonSocial.Padding = 5f;
                    innerTableL.AddCell(RazonSocial);

                    PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dir Matriz: " + sMatriz, cabecera));
                    DirMatriz.Border = Rectangle.NO_BORDER;
                    DirMatriz.Padding = 5f;
                    innerTableL.AddCell(DirMatriz);

                    PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dir Sucursal: " + sSucursal, cabecera));
                    DirSucursal.Border = Rectangle.NO_BORDER;
                    DirSucursal.Padding = 5f;
                    innerTableL.AddCell(DirSucursal);

                    PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro: " + sContribuyenteEspecial, cabecera));
                    ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                    ContribuyenteEspecial.Padding = 5f;
                    innerTableL.AddCell(ContribuyenteEspecial);

                    PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD: " + sObligadoContabilidad, cabecera));
                    ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                    ObligadoContabilidad.Padding = 5f;
                    innerTableL.AddCell(ObligadoContabilidad);

                    var ContenedorL = new PdfPCell(innerTableL);
                    ContenedorL.CellEvent = rr;
                    ContenedorL.Border = Rectangle.NO_BORDER;
                    tableL.AddCell(ContenedorL);
                    tableL.TotalWidth = 250f;

                    #endregion

                    #region Logo
                    PdfPTable tableLOGO = new PdfPTable(1);
                    PdfPCell logo = new PdfPCell(jpg);
                    logo.Border = Rectangle.NO_BORDER;
                    tableLOGO.AddCell(logo);
                    tableLOGO.TotalWidth = 250f;
                    #endregion

                    #region CabeceraNotaCredito
                    PdfPTable tableNotaCredito = new PdfPTable(4);
                    tableNotaCredito.TotalWidth = 540f;
                    tableNotaCredito.WidthPercentage = 100;
                    float[] DetalleNotaCreditoWidths = new float[] { 40f, 120f, 30f, 40f };
                    tableNotaCredito.SetWidths(DetalleNotaCreditoWidths);

                    var lblNombreCliente = new PdfPCell(new Paragraph("Razón Social / Nombres y Apellidos:", detalle));
                    lblNombreCliente.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableNotaCredito.AddCell(lblNombreCliente);
                    var NombreCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/razonSocialComprador").InnerText, detalle));
                    NombreCliente.Border = Rectangle.TOP_BORDER;
                    tableNotaCredito.AddCell(NombreCliente);
                    var lblRUC = new PdfPCell(new Paragraph("RUC / CI:", detalle));
                    lblRUC.Border = Rectangle.TOP_BORDER;
                    tableNotaCredito.AddCell(lblRUC);
                    var RUCcliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/identificacionComprador").InnerText, detalle));
                    RUCcliente.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableNotaCredito.AddCell(RUCcliente);

                    var lblFechaEmisionCliente = new PdfPCell(new Paragraph("Fecha Emisión:", detalle));
                    lblFechaEmisionCliente.Border = Rectangle.LEFT_BORDER;
                    lblFechaEmisionCliente.PaddingBottom = 8f;
                    tableNotaCredito.AddCell(lblFechaEmisionCliente);

                    var FechaEmisionCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/fechaEmision").InnerText, detalle));
                    FechaEmisionCliente.Border = Rectangle.BOTTOM_BORDER;
                    FechaEmisionCliente.PaddingBottom = 8f;
                    tableNotaCredito.AddCell(FechaEmisionCliente);
                    var lblGuiaRemision = new PdfPCell(new Paragraph("", detalle));
                    lblGuiaRemision.Border = Rectangle.BOTTOM_BORDER;
                    lblGuiaRemision.PaddingBottom = 8f;
                    tableNotaCredito.AddCell(lblGuiaRemision);
                    var GuiaRemision = new PdfPCell(new Paragraph("", detalle));
                    GuiaRemision.PaddingBottom = 8f;
                    GuiaRemision.Border = Rectangle.RIGHT_BORDER;
                    tableNotaCredito.AddCell(GuiaRemision);

                    var lblDocModifica = new PdfPCell(new Paragraph("Comprobante que se modifica:  ", detalle));
                    lblDocModifica.Border = Rectangle.LEFT_BORDER;
                    lblDocModifica.PaddingTop = 5f;
                    tableNotaCredito.AddCell(lblDocModifica);

                    sComprobanteModificacion = oDocument.SelectSingleNode("//infoNotaCredito/codDocModificado") == null ? "" : oDocument.SelectSingleNode("//infoNotaCredito/codDocModificado").InnerText;

                    switch (sComprobanteModificacion)
                    {
                        case "01":
                            sComprobanteModificacion = "FACTURA: ";
                            break;
                        case "04":
                            sComprobanteModificacion = "NOTA DE CRÉDITO: ";
                            break;
                        case "05":
                            sComprobanteModificacion = "NOTA DE DÉBITO: ";
                            break;
                        case "06":
                            sComprobanteModificacion = "GUÍA DE REMISIÓN: ";
                            break;
                        case "07":
                            sComprobanteModificacion = "COMPROBANTE DE RETENCIÓN: ";
                            break;
                        default:
                            break;
                    }
 

                    var DocModifica = new PdfPCell(new Paragraph(sComprobanteModificacion + oDocument.SelectSingleNode("//infoNotaCredito/numDocModificado").InnerText, detalle));
                    DocModifica.PaddingTop = 5f;
                    DocModifica.Colspan = 3;
                    DocModifica.Border = Rectangle.RIGHT_BORDER;
                    tableNotaCredito.AddCell(DocModifica);

                    var lblFechaEmision = new PdfPCell(new Paragraph("Fecha emisión (comprobante a modificar): ", detalle));
                    lblFechaEmision.Border = Rectangle.LEFT_BORDER;
                    lblFechaEmision.PaddingBottom = 5f;
                    tableNotaCredito.AddCell(lblFechaEmision);

                    var FechaEmision = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/fechaEmision").InnerText, detalle));
                    FechaEmision.Colspan = 3;
                    FechaEmision.Border = Rectangle.RIGHT_BORDER;
                    tableNotaCredito.AddCell(FechaEmision);

                    var lblRazonModificacion = new PdfPCell(new Paragraph("Razón de Modificación: ", detalle));
                    lblRazonModificacion.PaddingBottom = 5f;
                    lblRazonModificacion.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableNotaCredito.AddCell(lblRazonModificacion);

                    var RazonModificacion = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/motivo").InnerText, detalle));
                    RazonModificacion.PaddingBottom = 5f;
                    RazonModificacion.Colspan = 3;
                    RazonModificacion.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableNotaCredito.AddCell(RazonModificacion);

                    #endregion

                    #region DetalleNotaCredito
                    PdfPTable tableDetalleNotaCredito = new PdfPTable(10); // 10

                    tableDetalleNotaCredito.TotalWidth = 540f;
                    tableDetalleNotaCredito.WidthPercentage = 100;
                    tableDetalleNotaCredito.LockedWidth = true;
                    float[] DetalleFacturawidths = new float[] { 30f, 30f, 15f, 80f, 27f, 27f, 27f, 20f, 22f, 22f };
                    //float[] DetalleFacturawidths = new float[] { 40f, 20f, 120f, 35f, 25f, 25f, 30f };
                    tableDetalleNotaCredito.SetWidths(DetalleFacturawidths);

                    var fontEncabezado = GetArial(7);
                    var encCodPrincipal = new PdfPCell(new Paragraph("Cod. Principal", fontEncabezado));
                    encCodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encCodAuxiliar = new PdfPCell(new Paragraph("Cod. Auxiliar", fontEncabezado));
                    encCodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encCant = new PdfPCell(new Paragraph("Cant.", fontEncabezado));
                    encCant.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDescripcion = new PdfPCell(new Paragraph("Descripción", fontEncabezado));
                    encDescripcion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDetalleAdicional1 = new PdfPCell(new Paragraph("Detalle Adicional", fontEncabezado));
                    encDetalleAdicional1.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDetalleAdicional2 = new PdfPCell(new Paragraph("Detalle Adicional", fontEncabezado));
                    encDetalleAdicional2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDetalleAdicional3 = new PdfPCell(new Paragraph("Detalle Adicional", fontEncabezado));
                    encDetalleAdicional3.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPrecioUnitario = new PdfPCell(new Paragraph("Precio Unit.", fontEncabezado));
                    encPrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encDescuento = new PdfPCell(new Paragraph("Descuento", fontEncabezado));
                    encDescuento.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPrecioTotal = new PdfPCell(new Paragraph("Precio Total", fontEncabezado));
                    encPrecioTotal.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    tableDetalleNotaCredito.AddCell(encCodPrincipal);
                    tableDetalleNotaCredito.AddCell(encCodAuxiliar);
                    tableDetalleNotaCredito.AddCell(encCant);
                    tableDetalleNotaCredito.AddCell(encDescripcion);
                    tableDetalleNotaCredito.AddCell(encDetalleAdicional1);
                    tableDetalleNotaCredito.AddCell(encDetalleAdicional2);
                    tableDetalleNotaCredito.AddCell(encDetalleAdicional3);
                    tableDetalleNotaCredito.AddCell(encPrecioUnitario);
                    tableDetalleNotaCredito.AddCell(encDescuento);
                    tableDetalleNotaCredito.AddCell(encPrecioTotal);

                    PdfPCell CodPrincipal;
                    PdfPCell CodAuxiliar;
                    PdfPCell Cant;
                    PdfPCell Descripcion;
                    PdfPCell DetalleAdicional1;
                    PdfPCell DetalleAdicional2;
                    PdfPCell DetalleAdicional3;
                    PdfPCell PrecioUnitario;
                    PdfPCell Descuento;
                    PdfPCell PrecioTotal;

                    XmlNodeList Detalles;
                    Detalles = oDocument.SelectNodes("//detalles/detalle");
                    registros = Detalles.Count;
                    XmlNodeList DetallesAdicionales;
                    foreach(XmlNode Elemento in Detalles)
                    {
                        CodPrincipal = new PdfPCell(new Phrase(Elemento["codigoInterno"].InnerText, detAdicional));
                        CodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        CodAuxiliar = new PdfPCell(new Phrase(Elemento["codigoAdicional"] == null ? "" : Elemento["codigoAdicional"].InnerText, detalle));
                        Cant = new PdfPCell(new Phrase(Elemento["cantidad"].InnerText, detalle));
                        Cant.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        Descripcion = new PdfPCell(new Phrase(Elemento["descripcion"].InnerText.Length > 50 ? Elemento["descripcion"].InnerText.Substring(1, 49) : Elemento["descripcion"].InnerText, detalle));
                        DetallesAdicionales = Elemento.SelectNodes("detallesAdicionales/detAdicional");
                        if (!(DetallesAdicionales[0] == null))
                        {
                            DetalleAdicional1 = new PdfPCell(new Phrase(DetallesAdicionales[0].Attributes["nombre"].Value + ": " + DetallesAdicionales[0].Attributes["valor"].Value, detAdicional));
                        }else{
                            DetalleAdicional1 = new PdfPCell(new Phrase("", detAdicional));
                        }
                        if (!(DetallesAdicionales[1] == null))
                        {
                            DetalleAdicional2 = new PdfPCell(new Phrase(DetallesAdicionales[1].Attributes["nombre"].Value + ": " + DetallesAdicionales[1].Attributes["valor"].Value, detAdicional));
                        }else{
                            DetalleAdicional2 = new PdfPCell(new Phrase("", detAdicional));
                        }
                        if (!(DetallesAdicionales[2] == null))
                        {
                            DetalleAdicional3 = new PdfPCell(new Phrase(DetallesAdicionales[2].Attributes["nombre"].Value + ": " + DetallesAdicionales[2].Attributes["valor"].Value, detAdicional));
                        }else{
                            DetalleAdicional3 = new PdfPCell(new Phrase("", detAdicional));
                        }

                        PrecioUnitario = new PdfPCell(new Phrase(Elemento["precioUnitario"].InnerText, detalle));
                        PrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        Descuento = new PdfPCell(new Phrase(Elemento["descuento"].InnerText, detalle));
                        Descuento.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                        PrecioTotal = new PdfPCell(new Phrase(Elemento["precioTotalSinImpuesto"].InnerText, detalle));
                        PrecioTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                        tableDetalleNotaCredito.AddCell(CodPrincipal);
                        tableDetalleNotaCredito.AddCell(CodAuxiliar);
                        tableDetalleNotaCredito.AddCell(Cant);
                        tableDetalleNotaCredito.AddCell(Descripcion);
                        tableDetalleNotaCredito.AddCell(DetalleAdicional1);
                        tableDetalleNotaCredito.AddCell(DetalleAdicional2);
                        tableDetalleNotaCredito.AddCell(DetalleAdicional3);
                        tableDetalleNotaCredito.AddCell(PrecioUnitario);
                        tableDetalleNotaCredito.AddCell(Descuento);
                        tableDetalleNotaCredito.AddCell(PrecioTotal);
                    }
                    #endregion

                    #region InformacionAdicional
                    var tableInfoAdicional = new PdfPTable(2);
                    tableInfoAdicional.TotalWidth = 250f;
                    float[] InfoAdicionalWidths = new float[] { 65f, 170f };
                    tableInfoAdicional.SetWidths(InfoAdicionalWidths);

                    var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                    lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    lblInfoAdicional.Colspan = 2;
                    lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblInfoAdicional.Padding = 5f;
                    tableInfoAdicional.AddCell(lblInfoAdicional);

                    var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottom.Padding = 2f;
                    var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                    Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottom.Padding = 2f;

                    XmlNodeList InfoAdicional;
                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    PdfPCell lblCodigo;
                    PdfPCell Codigo;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        lblCodigo = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value, detAdicional));
                        lblCodigo.Border = Rectangle.LEFT_BORDER;
                        lblCodigo.Padding = 2f;

                        Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText.Length > 150 ? campoAdicional.InnerText.Substring(0, 150) + "..." : campoAdicional.InnerText, detAdicional));
                        Codigo.Border = Rectangle.RIGHT_BORDER;
                        Codigo.Padding = 2f;

                        tableInfoAdicional.AddCell(lblCodigo);
                        tableInfoAdicional.AddCell(Codigo);
                    }

                    tableInfoAdicional.AddCell(lblBottom);
                    tableInfoAdicional.AddCell(Bottom);

                    #endregion

                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    decimal dpropina = 0;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        if (campoAdicional.Attributes["nombre"].Value == "propina")
                        {
                            dpropina = decimal.Parse(campoAdicional.InnerText, nfi);
                        }
                    }

                    #region Totales

                    var tableTotales = new PdfPTable(2);
                    tableTotales.TotalWidth = 160f;
                    float[] InfoTotales = new float[] { 105f, 55f };
                    tableTotales.SetWidths(InfoTotales);

                    XmlNodeList Impuestos;
                    Impuestos = oDocument.SelectNodes("//infoNotaCredito/totalConImpuestos/totalImpuesto");
                    decimal dSubtotal12 = 0, dSubtotal0 = 0, dSubtotalNSI = 0, dICE = 0, dIVA12 = 0, dSubtotalExcento = 0, dIRBPNR = 0;
                    foreach (XmlNode Impuesto in Impuestos)
                    {
                        switch (Impuesto["codigo"].InnerText)
                        {
                            case "2":        // IVA
                                if (Impuesto["codigoPorcentaje"].InnerText == "0") // 0%
                                {
                                    dSubtotal0 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "2")    // 12%
                                {
                                    dSubtotal12 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                    dIVA12 = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "6")    // No objeto de Impuesto
                                {
                                    dSubtotalNSI = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                else if (Impuesto["codigoPorcentaje"].InnerText == "7")
                                {
                                    dSubtotalExcento = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                }
                                break;
                            case "3":       // ICE
                                dICE = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                break;
                            case "5":
                                dIRBPNR = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                break;
                        }
                    }
                    var lblSubTotal12 = new PdfPCell(new Paragraph("SUBTOTAL 12.00%:", detalle));
                    lblSubTotal12.Padding = 2f;
                    var SubTotal12 = new PdfPCell(new Paragraph(dSubtotal12 == 0 ? "0.00" : dSubtotal12.ToString(nfi), detalle));
                    SubTotal12.Padding = 2f;
                    SubTotal12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotal0 = new PdfPCell(new Paragraph("SUBTOTAL 0%:", detalle));
                    lblSubTotal0.Padding = 2f;
                    var SubTotal0 = new PdfPCell(new Paragraph(dSubtotal0 == 0 ? "0.00" : dSubtotal0.ToString(nfi), detalle));
                    SubTotal0.Padding = 2f;
                    SubTotal0.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalNoSujetoIVA = new PdfPCell(new Paragraph("SUBTOTAL NO OBJETO IVA:", detalle));
                    lblSubTotalNoSujetoIVA.Padding = 2f;
                    var SubTotalNoSujetoIVA = new PdfPCell(new Paragraph(dSubtotalNSI == 0 ? "0.00" : dSubtotalNSI.ToString(nfi), detalle));
                    SubTotalNoSujetoIVA.Padding = 2f;
                    SubTotalNoSujetoIVA.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalExcentoIVA = new PdfPCell(new Paragraph("SUBTOTAL EXENTO IVA:", detalle));
                    lblSubTotalExcentoIVA.Padding = 2f;
                    var SubTotalExcentoIVA = new PdfPCell(new Paragraph(dSubtotalExcento == 0 ? "0.00" : dSubtotalExcento.ToString(nfi), detalle));
                    SubTotalExcentoIVA.Padding = 2f;
                    SubTotalExcentoIVA.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblSubTotalSinImpuestos = new PdfPCell(new Paragraph("SUBTOTAL SIN IMPUESTOS:", detalle));
                    lblSubTotalSinImpuestos.Padding = 2f;
                    var SubTotalSinImpuestos = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/totalSinImpuestos").InnerText, detalle));
                    SubTotalSinImpuestos.Padding = 2f;
                    SubTotalSinImpuestos.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblDescuento = new PdfPCell(new Paragraph("TOTAL DESCUENTO:", detalle));
                    lblDescuento.Padding = 2f;
                    var TotalDescuento = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/totalDescuento") == null ? "0.00" : oDocument.SelectSingleNode("//infoNotaCredito/totalDescuento").InnerText, detalle));
                    TotalDescuento.Padding = 2f;
                    TotalDescuento.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblICE = new PdfPCell(new Paragraph("ICE:", detalle));
                    lblICE.Padding = 2f;
                    var ICE = new PdfPCell(new Paragraph(dICE == 0 ? "0.00" : dICE.ToString(nfi), detalle));
                    ICE.Padding = 2f;
                    ICE.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblIVA12 = new PdfPCell(new Paragraph("IVA 12.00%:", detalle));
                    lblIVA12.Padding = 2f;
                    var IVA12 = new PdfPCell(new Paragraph(dIVA12.ToString(nfi), detalle));
                    IVA12.Padding = 2f;
                    IVA12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblIRBPNR = new PdfPCell(new Paragraph("IRBPNR:", detalle));
                    lblIRBPNR.Padding = 2f;
                    var IRBPNR = new PdfPCell(new Paragraph(dIRBPNR == 0 ? "0.00" : dIRBPNR.ToString(nfi), detalle));
                    IRBPNR.Padding = 2f;
                    IRBPNR.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    //var lblPropina = new PdfPCell(new Paragraph("PROPINA:", detalle));
                    //lblPropina.Padding = 2f;
                    //var Propina = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/propina").InnerText, detalle));
                    //Propina.Padding = 2f;
                    //Propina.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                    var lblValorTotal = new PdfPCell(new Paragraph("VALOR TOTAL:", detalle));
                    lblValorTotal.Padding = 2f;
                    var ValorTotal = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/valorModificacion").InnerText, detalle));
                    ValorTotal.Padding = 2f;
                    ValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    var lblServicio = new PdfPCell(new Paragraph("SERVICIO 10%:", detalle));
                    lblValorTotal.Padding = 2f;
                    var Servicio = new PdfPCell(new Paragraph(dpropina.ToString(nfi), detalle));
                    Servicio.Padding = 2f;
                    Servicio.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    tableTotales.AddCell(lblSubTotal12);
                    tableTotales.AddCell(SubTotal12);
                    tableTotales.AddCell(lblSubTotal0);
                    tableTotales.AddCell(SubTotal0);
                    tableTotales.AddCell(lblSubTotalNoSujetoIVA);
                    tableTotales.AddCell(SubTotalNoSujetoIVA);
                    tableTotales.AddCell(lblSubTotalExcentoIVA);
                    tableTotales.AddCell(SubTotalExcentoIVA);
                    tableTotales.AddCell(lblSubTotalSinImpuestos);
                    tableTotales.AddCell(SubTotalSinImpuestos);
                    tableTotales.AddCell(lblDescuento);
                    tableTotales.AddCell(TotalDescuento);
                    tableTotales.AddCell(lblICE);
                    tableTotales.AddCell(ICE);
                    tableTotales.AddCell(lblIVA12);
                    tableTotales.AddCell(IVA12);
                    tableTotales.AddCell(lblIRBPNR);
                    tableTotales.AddCell(IRBPNR);
                    //tableTotales.AddCell(lblPropina);
                    //tableTotales.AddCell(Propina);
                    tableTotales.AddCell(lblValorTotal);
                    tableTotales.AddCell(ValorTotal);
                    if (dpropina > 0)
                    {
                        tableTotales.AddCell(lblServicio);
                        tableTotales.AddCell(Servicio);
                    }
                    #endregion

                   
                    tableR.WriteSelectedRows(0, 15, 292, 806, canvas);
                    tableLOGO.WriteSelectedRows(0, 1, 30, 780, canvas);
                    posDetalleCliente = 796 - tableR.TotalHeight;
                    posDetalleFactura = (posDetalleCliente - 8) - tableNotaCredito.TotalHeight;

                    tableNotaCredito.WriteSelectedRows(0, 10, 28, posDetalleCliente, canvas);
                    if (sSucursal.Length > 40 && sMatriz.Length > 40)
                    {
                        tableL.WriteSelectedRows(0, 5, 28, 705, canvas);
                    }
                    else
                    {
                        tableL.WriteSelectedRows(0, 5, 28, 695, canvas);
                    }

                    if (registros <= PagLimite1)    // Una sola página 
                    {
                        tableDetalleNotaCredito.WriteSelectedRows(0, PagLimite1 + 1, 28, posDetalleCliente - 10 - tableNotaCredito.TotalHeight, canvas);
                        posInfoAdicional = (posDetalleFactura - 10) - tableDetalleNotaCredito.TotalHeight;
                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInfoAdicional, canvas);
                        tableTotales.WriteSelectedRows(0, 12, 408, posInfoAdicional, canvas);

                    }
                    else if (registros > PagLimite1 && registros <= MaxPagina1)  // Una sola página con detalle en la siguiente.
                    {
                        tableDetalleNotaCredito.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableNotaCredito.TotalHeight, canvas);
                        documento.NewPage();

                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806, canvas);
                        tableTotales.WriteSelectedRows(0, 12, 408, 806, canvas);
                        Console.WriteLine(tableDetalleNotaCredito.TotalHeight);  // 513
                    }
                    else
                    {
                        tableDetalleNotaCredito.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableNotaCredito.TotalHeight, canvas);
                        documento.NewPage();

                        decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));
                        float posInicial = 0;
                        int faltantes = 0, ultimo = 0, hasta = 0;
                        ultimo = MaxPagina1 + 1;
                        hasta = MaxPagina1 + MaxSoloPagina + 1;
                        faltantes = registros - MaxPagina1 + 1;
                        for (int i = 0; i < Paginas; i++)
                        {
                            posInicial = 0;
                            documento.NewPage();
                            tableDetalleNotaCredito.WriteSelectedRows(ultimo, hasta, 28, 806, canvas);
                            ultimo = hasta;
                            hasta = ultimo + MaxSoloPagina;
                            if (faltantes > MaxSoloPagina)
                            {
                                faltantes = faltantes - (hasta - ultimo);
                            }
                        }

                        posInicial = (806 - (faltantes * 11)) - 20;

                        if (posInicial > 120)
                        {
                            tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInicial + 10, canvas);
                            tableTotales.WriteSelectedRows(0, 12, 408, posInicial + 10, canvas);
                        }
                        else
                        {
                            tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806, canvas);
                            tableTotales.WriteSelectedRows(0, 12, 408, 806, canvas);
                        }
                        Console.WriteLine(writer.PageNumber);
                    }

                    writer.CloseStream = false;
                    documento.Close();
                    ms.Position = 0;
                    Bytes = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                //var oLog = new LogWriter();
                //oLog.RegistraLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName + "." + MethodBase.GetCurrentMethod().Name,
                //    "Error al generar PDF.", ex);
            }
            return Bytes;
        }

        /// <summary>
        /// Método para generación de nota de débito RIDE
        /// </summary>
        /// <param name="pDocumento_autorizado">Documento Autorizado</param>
        /// <param name="pRutaLogo">Ruta física o URL del logo a imprimir</param>
        /// <param name="pCultura">Cultura, por defecto en-US</param>
        /// <returns>Arreglo de bytes</returns>
        public byte[] GeneraNotaDebito(string pDocumento_autorizado, string pRutaLogo, string pCultura = "en-US")
        {
            MemoryStream ms = null;
            byte[] Bytes = null;
            iTextSharp.text.Font detAdicional = GetArial(6);
            try
            {
                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sRuc = "", sContribuyenteEspecial = "",
                           sAmbienteVal = "", sNumAutorizacion;
                    Cultura = pCultura;
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";


                    oDocument.LoadXml(pDocumento_autorizado);
                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    sSucursal = (oDocument.SelectSingleNode("//infoNotaDebito/dirEstablecimiento") == null) ? "" : oDocument.SelectSingleNode("//infoNotaDebito/dirEstablecimiento").InnerText;
                    sContribuyenteEspecial = (oDocument.SelectSingleNode("//infoNotaDebito/contribuyenteEspecial") == null) ? "" : oDocument.SelectSingleNode("//infoNotaDebito/contribuyenteEspecial").InnerText;

                int registros = 0;
                int PagLimite1 = 30;
                int MaxPagina1 = 39;
                int MaxSoloPagina = 70;

                float posDetalleCliente = 0;
                float posDetalleFactura = 0;
                float posInfoAdicional = 0;

                PdfWriter writer;
                RoundRectangle rr = new RoundRectangle();
                //Creamos un tipo de archivo que solo se cargará en la memoria principal
                Document documento = new Document();
                //Creamos la instancia para generar el archivo PDF
                //Le pasamos el documento creado arriba y con capacidad para abrir o Crear y de nombre Mi_Primer_PDF
                writer = PdfWriter.GetInstance(documento, ms);

                iTextSharp.text.Font cabecera = GetArial(8);
                iTextSharp.text.Font detalle = GetArial(7);

                documento.Open();
                var oEvents = new ITextEvents();
                writer.PageEvent = oEvents;
                PdfContentByte canvas = writer.DirectContent;
                iTextSharp.text.Image jpg = null;

                jpg = iTextSharp.text.Image.GetInstance(pRutaLogo);
                jpg.ScaleToFit(250f, 70f);

                #region TablaDerecha
                PdfPTable tableR = new PdfPTable(1);
                PdfPTable innerTableD = new PdfPTable(1);

                PdfPCell RUC = new PdfPCell(new Paragraph("R.U.C.: " + sRuc, cabecera));
                RUC.Border = Rectangle.NO_BORDER;
                RUC.Padding = 5f;
                innerTableD.AddCell(RUC);

                PdfPCell Factura = new PdfPCell(new Paragraph("N O T A  D E  D É B I T O ", cabecera));
                Factura.Border = Rectangle.NO_BORDER;
                Factura.Padding = 5f;
                innerTableD.AddCell(Factura);

                PdfPCell NumFactura = new PdfPCell(new Paragraph("No. " + oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabecera));
                NumFactura.Border = Rectangle.NO_BORDER;
                NumFactura.Padding = 5f;
                innerTableD.AddCell(NumFactura);

                PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabecera));
                lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                lblNumAutorizacion.Padding = 5f;
                innerTableD.AddCell(lblNumAutorizacion);

                PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabecera));
                NumAutorizacion.Border = Rectangle.NO_BORDER;
                NumAutorizacion.Padding = 5f;
                innerTableD.AddCell(NumAutorizacion);

                PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA Y HORA AUTORIZACIÓN: " + sFechaAutorizacion, cabecera));
                FechaAutorizacion.Border = Rectangle.NO_BORDER;
                FechaAutorizacion.Padding = 5f;
                innerTableD.AddCell(FechaAutorizacion);

                PdfPCell Ambiente = new PdfPCell(new Paragraph("AMBIENTE: " + sAmbiente, cabecera));
                Ambiente.Border = Rectangle.NO_BORDER;
                Ambiente.Padding = 5f;
                innerTableD.AddCell(Ambiente);

                PdfPCell Emision = new PdfPCell(new Paragraph("EMISIÓN: " + sTipoEmision, cabecera));
                Emision.Border = Rectangle.NO_BORDER;
                Emision.Padding = 5f;
                innerTableD.AddCell(Emision);

                PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CLAVE DE ACCESO: ", cabecera));
                ClaveAcceso.Border = Rectangle.NO_BORDER;
                ClaveAcceso.Padding = 5f;
                innerTableD.AddCell(ClaveAcceso);

                Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);

                PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                ImgClaveAcceso.Padding = 5f;
                ImgClaveAcceso.Colspan = 2;
                ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;

                innerTableD.AddCell(ImgClaveAcceso);

                var ContenedorD = new PdfPCell(innerTableD);
                ContenedorD.CellEvent = rr;
                ContenedorD.Border = Rectangle.NO_BORDER;
                tableR.AddCell(ContenedorD);
                tableR.TotalWidth = 278f;
                #endregion

                #region TablaIzquierda
                PdfPTable tableL = new PdfPTable(1);
                PdfPTable innerTableL = new PdfPTable(1);

                PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                RazonSocial.Border = Rectangle.NO_BORDER;
                RazonSocial.Padding = 5f;
                innerTableL.AddCell(RazonSocial);

                PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dir Matriz: " + sMatriz, cabecera));
                DirMatriz.Border = Rectangle.NO_BORDER;
                DirMatriz.Padding = 5f;
                innerTableL.AddCell(DirMatriz);

                PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dir Sucursal: " + sSucursal, cabecera));
                DirSucursal.Border = Rectangle.NO_BORDER;
                DirSucursal.Padding = 5f;
                innerTableL.AddCell(DirSucursal);

                PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro: " + sContribuyenteEspecial, cabecera));
                ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                ContribuyenteEspecial.Padding = 5f;
                innerTableL.AddCell(ContribuyenteEspecial);

                //PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD: SI", cabecera));
                //ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                //ObligadoContabilidad.Padding = 5f;
                //innerTableL.AddCell(ObligadoContabilidad);

                var ContenedorL = new PdfPCell(innerTableL);
                ContenedorL.CellEvent = rr;
                ContenedorL.Border = Rectangle.NO_BORDER;
                tableL.AddCell(ContenedorL);
                tableL.TotalWidth = 250f;

                #endregion

                #region Logo
                PdfPTable tableLOGO = new PdfPTable(1);
                PdfPCell logo = new PdfPCell(jpg);
                logo.Border = Rectangle.NO_BORDER;
                tableLOGO.AddCell(logo);
                tableLOGO.TotalWidth = 250f;
                #endregion

                #region CabeceraNotaDebito
                PdfPTable tableNotaDebito = new PdfPTable(4);
                tableNotaDebito.TotalWidth = 540f;
                tableNotaDebito.WidthPercentage = 100;
                float[] DetalleNotaCreditoWidths = new float[] { 30f, 120f, 30f, 40f };
                tableNotaDebito.SetWidths(DetalleNotaCreditoWidths);

                var lblNombreCliente = new PdfPCell(new Paragraph("Razón Social / Nombres y Apellidos:", detalle));
                lblNombreCliente.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                tableNotaDebito.AddCell(lblNombreCliente);
                var NombreCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/razonSocialComprador").InnerText, detalle));
                NombreCliente.Border = Rectangle.TOP_BORDER;
                tableNotaDebito.AddCell(NombreCliente);
                var lblRUC = new PdfPCell(new Paragraph("RUC / CI:", detalle));
                lblRUC.Border = Rectangle.TOP_BORDER;
                tableNotaDebito.AddCell(lblRUC);
                var RUCcliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/identificacionComprador").InnerText, detalle));
                RUCcliente.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                tableNotaDebito.AddCell(RUCcliente);

                var lblFechaEmisionCliente = new PdfPCell(new Paragraph("Fecha Emisión:", detalle));
                lblFechaEmisionCliente.Border = Rectangle.LEFT_BORDER;
                lblFechaEmisionCliente.PaddingBottom = 8f;
                tableNotaDebito.AddCell(lblFechaEmisionCliente);

                var FechaEmisionCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/fechaEmision").InnerText, detalle));
                FechaEmisionCliente.Border = Rectangle.BOTTOM_BORDER;
                FechaEmisionCliente.PaddingBottom = 8f;
                tableNotaDebito.AddCell(FechaEmisionCliente);
                var lblGuiaRemision = new PdfPCell(new Paragraph("", detalle));
                lblGuiaRemision.Border = Rectangle.BOTTOM_BORDER;
                lblGuiaRemision.PaddingBottom = 8f;
                tableNotaDebito.AddCell(lblGuiaRemision);
                var GuiaRemision = new PdfPCell(new Paragraph("", detalle));
                GuiaRemision.PaddingBottom = 8f;
                GuiaRemision.Border = Rectangle.RIGHT_BORDER;
                tableNotaDebito.AddCell(GuiaRemision);

                var lblDocModifica = new PdfPCell(new Paragraph("Comprobante que se modifica: ", detalle));
                lblDocModifica.Border = Rectangle.LEFT_BORDER;
                lblDocModifica.PaddingTop = 5f;
                tableNotaDebito.AddCell(lblDocModifica);

                var DocModifica = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/numDocModificado").InnerText, detalle));
                DocModifica.PaddingTop = 5f;
                DocModifica.Colspan = 3;
                DocModifica.Border = Rectangle.RIGHT_BORDER;
                tableNotaDebito.AddCell(DocModifica);

                var lblFechaEmision = new PdfPCell(new Paragraph("Fecha de emisión: ", detalle));
                lblFechaEmision.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER; ;
                lblFechaEmision.PaddingBottom = 5f;
                tableNotaDebito.AddCell(lblFechaEmision);

                var FechaEmision = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/fechaEmision").InnerText, detalle));
                FechaEmision.Colspan = 3;
                FechaEmisionCliente.PaddingBottom = 5f;
                FechaEmision.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                tableNotaDebito.AddCell(FechaEmision);

                #endregion

                #region DetalleNotaDebito
                PdfPTable tableDetalleNotaDebito = new PdfPTable(2);

                tableDetalleNotaDebito.TotalWidth = 540f;
                tableDetalleNotaDebito.WidthPercentage = 100;
                tableDetalleNotaDebito.LockedWidth = true;
                float[] DetalleFacturawidths = new float[] { 90f, 40f };
                tableDetalleNotaDebito.SetWidths(DetalleFacturawidths);

                var fontEncabezado = GetArial(7);
                var encCodPrincipal = new PdfPCell(new Paragraph("Razón Modificación", fontEncabezado));
                encCodPrincipal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                var encCodAuxiliar = new PdfPCell(new Paragraph("Valor Modificación", fontEncabezado));
                encCodAuxiliar.HorizontalAlignment = Rectangle.ALIGN_CENTER;


                tableDetalleNotaDebito.AddCell(encCodPrincipal);
                tableDetalleNotaDebito.AddCell(encCodAuxiliar);

                PdfPCell DetRazonModificacion;
                PdfPCell DetValorModificacion;

                XmlNodeList Detalles;
                Detalles = oDocument.SelectNodes("//motivos/motivo");
                registros = Detalles.Count;

                foreach (XmlNode Elemento in Detalles)
                {
                    DetRazonModificacion = new PdfPCell(new Phrase(Elemento["razon"].InnerText, detalle));
                    DetRazonModificacion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    DetValorModificacion = new PdfPCell(new Phrase(Elemento["valor"].InnerText, detalle));
                    DetValorModificacion.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    tableDetalleNotaDebito.AddCell(DetRazonModificacion);
                    tableDetalleNotaDebito.AddCell(DetValorModificacion);

                }
                #endregion

                #region InformacionAdicional
                var tableInfoAdicional = new PdfPTable(2);
                tableInfoAdicional.TotalWidth = 250f;
                float[] InfoAdicionalWidths = new float[] { 65f, 170f };
                tableInfoAdicional.SetWidths(InfoAdicionalWidths);


                var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                lblInfoAdicional.Colspan = 2;
                lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                lblInfoAdicional.Padding = 5f;
                tableInfoAdicional.AddCell(lblInfoAdicional);

                XmlNodeList InfoAdicional;
                InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                PdfPCell lblCodigo;
                PdfPCell Codigo;

                var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                lblBottom.Padding = 2f;
                var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                Bottom.Padding = 2f;

                foreach (XmlNode campoAdicional in InfoAdicional)
                {
                    lblCodigo = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value, detAdicional));
                    lblCodigo.Border = Rectangle.LEFT_BORDER;
                    lblCodigo.Padding = 2f;

                    Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText.Length > 150 ? campoAdicional.InnerText.Substring(0, 150) + "..." : campoAdicional.InnerText, detAdicional));
                    Codigo.Border = Rectangle.RIGHT_BORDER;
                    Codigo.Padding = 2f;

                    tableInfoAdicional.AddCell(lblCodigo);
                    tableInfoAdicional.AddCell(Codigo);
                }

                tableInfoAdicional.AddCell(lblBottom);
                tableInfoAdicional.AddCell(Bottom);

                #endregion

                #region Totales

                var tableTotales = new PdfPTable(2);
                tableTotales.TotalWidth = 160f;
                float[] InfoTotales = new float[] { 105f, 55f };
                tableTotales.SetWidths(InfoTotales);

                XmlNodeList Impuestos;
                Impuestos = oDocument.SelectNodes("//infoNotaDebito/impuestos/impuesto");
                decimal dSubtotal12 = 0, dSubtotal0 = 0, dSubtotalNSI = 0, dICE = 0, dIVA12 = 0;
                foreach (XmlNode Impuesto in Impuestos)
                {
                    switch (Impuesto["codigo"].InnerText)
                    {
                        case "2":        // IVA
                            if (Impuesto["codigoPorcentaje"].InnerText == "0") // 0%
                            {
                                dSubtotal0 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                            }
                            else if (Impuesto["codigoPorcentaje"].InnerText == "2")    // 12%
                            {
                                dSubtotal12 = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                                dIVA12 = decimal.Parse(Impuesto["valor"].InnerText, new CultureInfo(Cultura));
                            }
                            else if (Impuesto["codigoPorcentaje"].InnerText == "2")    // No objeto de Impuesto
                            {
                                dSubtotalNSI = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                            }
                            break;
                        case "3":       // ICE
                            dICE = decimal.Parse(Impuesto["baseImponible"].InnerText, new CultureInfo(Cultura));
                            break;
                    }
                }
                var lblSubTotal12 = new PdfPCell(new Paragraph("SUBTOTAL 12.00%:", detalle));
                lblSubTotal12.Padding = 2f;
                var SubTotal12 = new PdfPCell(new Paragraph(dSubtotal12.ToString(nfi), detalle));
                SubTotal12.Padding = 2f;
                SubTotal12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                var lblSubTotal0 = new PdfPCell(new Paragraph("SUBTOTAL 00.00%:", detalle));
                lblSubTotal0.Padding = 2f;
                var SubTotal0 = new PdfPCell(new Paragraph(dSubtotal0.ToString(nfi), detalle));
                SubTotal0.Padding = 2f;
                SubTotal0.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                var lblSubTotalNoSujetoIVA = new PdfPCell(new Paragraph("SUBTOTAL No sujeto de IVA:", detalle));
                lblSubTotalNoSujetoIVA.Padding = 2f;
                var SubTotalNoSujetoIVA = new PdfPCell(new Paragraph(dSubtotalNSI.ToString(nfi), detalle));
                lblSubTotalNoSujetoIVA.Padding = 2f;
                SubTotalNoSujetoIVA.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                var lblSubTotalSinImpuestos = new PdfPCell(new Paragraph("SUBTOTAL SIN IMPUESTOS:", detalle));
                lblSubTotalSinImpuestos.Padding = 2f;
                var SubTotalSinImpuestos = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/totalSinImpuestos").InnerText, detalle));
                SubTotalSinImpuestos.Padding = 2f;
                SubTotalSinImpuestos.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                var lblDescuento = new PdfPCell(new Paragraph("DESCUENTO:", detalle));
                lblDescuento.Padding = 2f;
                var TotalDescuento = new PdfPCell(new Paragraph((oDocument.SelectSingleNode("//infoNotaDebito/totalDescuento") == null ? "0.00" : oDocument.SelectSingleNode("//infoNotaDebito/totalDescuento").InnerText), detalle));
                TotalDescuento.Padding = 2f;
                TotalDescuento.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                var lblICE = new PdfPCell(new Paragraph("ICE:", detalle));
                lblICE.Padding = 2f;
                var ICE = new PdfPCell(new Paragraph(dICE.ToString(nfi), detalle));
                ICE.Padding = 2f;
                ICE.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                var lblIVA12 = new PdfPCell(new Paragraph("IVA 12.00%:", detalle));
                lblIVA12.Padding = 2f;
                var IVA12 = new PdfPCell(new Paragraph(dIVA12.ToString(nfi), detalle));
                IVA12.Padding = 2f;
                IVA12.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                //var lblPropina = new PdfPCell(new Paragraph("PROPINA:", detalle));
                //lblPropina.Padding = 2f;
                //var Propina = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaCredito/propina").InnerText, detalle));
                //Propina.Padding = 2f;
                //Propina.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                var lblValorTotal = new PdfPCell(new Paragraph("VALOR TOTAL:", detalle));
                lblValorTotal.Padding = 2f;
                var ValorTotal = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoNotaDebito/valorTotal").InnerText, detalle));
                ValorTotal.Padding = 2f;
                ValorTotal.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                tableTotales.AddCell(lblSubTotal12);
                tableTotales.AddCell(SubTotal12);
                tableTotales.AddCell(lblSubTotal0);
                tableTotales.AddCell(SubTotal0);
                tableTotales.AddCell(lblSubTotalNoSujetoIVA);
                tableTotales.AddCell(SubTotalNoSujetoIVA);
                tableTotales.AddCell(lblSubTotalSinImpuestos);
                tableTotales.AddCell(SubTotalSinImpuestos);
                tableTotales.AddCell(lblDescuento);
                tableTotales.AddCell(TotalDescuento);
                tableTotales.AddCell(lblICE);
                tableTotales.AddCell(ICE);
                tableTotales.AddCell(lblIVA12);
                tableTotales.AddCell(IVA12);
                //tableTotales.AddCell(lblPropina);
                //tableTotales.AddCell(Propina);
                tableTotales.AddCell(lblValorTotal);
                tableTotales.AddCell(ValorTotal);

                #endregion

                tableR.WriteSelectedRows(0, 15, 292, 806, canvas);
                tableLOGO.WriteSelectedRows(0, 1, 30, 780, canvas);
                posDetalleCliente = 796 - tableR.TotalHeight;
                posDetalleFactura = (posDetalleCliente - 8) - tableNotaDebito.TotalHeight;

                tableNotaDebito.WriteSelectedRows(0, 10, 28, posDetalleCliente, canvas);
                if (sSucursal.Length > 42 && sMatriz.Length > 42)
                {
                    tableL.WriteSelectedRows(0, 5, 28, 695, canvas);
                }
                else
                {
                    tableL.WriteSelectedRows(0, 5, 28, 675, canvas);
                }

                if (registros <= PagLimite1)    // Una sola página 
                {
                    tableDetalleNotaDebito.WriteSelectedRows(0, PagLimite1 + 1, 28, posDetalleCliente - 10 - tableNotaDebito.TotalHeight, canvas);
                    posInfoAdicional = (posDetalleFactura - 10) - tableDetalleNotaDebito.TotalHeight;
                    tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInfoAdicional, canvas);
                    tableTotales.WriteSelectedRows(0, 10, 408, posInfoAdicional, canvas);
                    Console.WriteLine(tableDetalleNotaDebito.TotalHeight);  // 403

                }
                else if (registros > PagLimite1 && registros <= MaxPagina1)  // Una sola página con detalle en la siguiente.
                {
                    tableDetalleNotaDebito.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableNotaDebito.TotalHeight, canvas);
                    documento.NewPage();

                    tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806, canvas);
                    tableTotales.WriteSelectedRows(0, 10, 408, 806, canvas);
                    Console.WriteLine(tableDetalleNotaDebito.TotalHeight);  // 513
                }
                else
                {
                    tableDetalleNotaDebito.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableNotaDebito.TotalHeight, canvas);
                    documento.NewPage();

                    decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));
                    float posInicial = 0;
                    int faltantes = 0, ultimo = 0, hasta = 0;
                    ultimo = MaxPagina1 + 1;
                    hasta = MaxPagina1 + MaxSoloPagina + 1;
                    faltantes = registros - MaxPagina1 + 1;
                    for (int i = 0; i < Paginas; i++)
                    {
                        posInicial = 0;
                        documento.NewPage();
                        tableDetalleNotaDebito.WriteSelectedRows(ultimo, hasta, 28, 806, canvas);
                        ultimo = hasta;
                        hasta = ultimo + MaxSoloPagina;
                        if (faltantes > MaxSoloPagina)
                        {
                            faltantes = faltantes - (hasta - ultimo);
                        }
                    }

                    posInicial = (806 - (faltantes * 11)) - 20;

                    if (posInicial > 120)
                    {
                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, posInicial + 10, canvas);
                        tableTotales.WriteSelectedRows(0, 10, 408, posInicial + 10, canvas);
                    }
                    else
                    {
                        tableInfoAdicional.WriteSelectedRows(0, 17, 28, 806, canvas);
                        tableTotales.WriteSelectedRows(0, 10, 408, 806, canvas);
                    }
                   
                }

                writer.CloseStream = false;
                documento.Close();
                ms.Position = 0;
                Bytes = ms.ToArray();
                }

            }
            catch (Exception ex)
            {
                //var oLog = new LogWriter();
                //oLog.RegistraLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName + "." + MethodBase.GetCurrentMethod().Name,
                //    "Error al generar PDF.", ex);
            }
            return Bytes;
        }
        /// <summary>
        /// Método para generación de comprobante de retención RIDE
        /// </summary>
        /// <param name="pDocumento_autorizado">Documento Autorizado</param>
        /// <param name="pRutaLogo">Ruta física o URL del logo a imprimir</param>
        /// <param name="pCultura">Cultura, por defecto en-US</param>
        /// <returns>Arreglo de bytes</returns>
        public byte[] GeneraComprobanteRetencion(string pDocumento_autorizado, string pRutaLogo, string pCultura = "en-US")
        {
            MemoryStream ms = null;
            byte[] Bytes = null;
            try
            {
                using (ms = new MemoryStream())
                {
                    XmlDocument oDocument = new XmlDocument();
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sRuc = "", sContribuyenteEspecial = "",
                           sAmbienteVal = "", sEjercicioFiscal = "", sNumAutorizacion = "", sObligadoContabilidad="";
                    Cultura = pCultura;
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";

                    oDocument.LoadXml(pDocumento_autorizado);
                    sFechaAutorizacion = oDocument.SelectSingleNode("//fechaAutorizacion").InnerText;
                    sNumAutorizacion = oDocument.SelectSingleNode("//numeroAutorizacion").InnerText;
                    sAmbiente = oDocument.SelectSingleNode("//ambiente") == null ? sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN" : oDocument.SelectSingleNode("//ambiente").InnerText;
                    oDocument.LoadXml(oDocument.SelectSingleNode("//comprobante").InnerText);
                    sAmbienteVal = oDocument.SelectSingleNode("//infoTributaria/ambiente").InnerText;
                    sAmbiente = sAmbienteVal == "1" ? "PRUEBAS" : "PRODUCCIÓN";
                    sRuc = oDocument.SelectSingleNode("//infoTributaria/ruc").InnerText;
                    sTipoEmision = (oDocument.SelectSingleNode("//infoTributaria/tipoEmision").InnerText == "1") ? "NORMAL" : "INDISPONIBILIDAD DEL SISTEMA";
                    sMatriz = oDocument.SelectSingleNode("//infoTributaria/dirMatriz").InnerText;
                    sRazonSocial = oDocument.SelectSingleNode("//infoTributaria/razonSocial").InnerText;
                    sSucursal = (oDocument.SelectSingleNode("//infoCompRetencion/dirEstablecimiento") == null) ? "" : oDocument.SelectSingleNode("//infoCompRetencion/dirEstablecimiento").InnerText;
                    sContribuyenteEspecial = oDocument.SelectSingleNode("//infoCompRetencion/contribuyenteEspecial") == null ? "" : oDocument.SelectSingleNode("//infoCompRetencion/contribuyenteEspecial").InnerText;
                    sEjercicioFiscal = oDocument.SelectSingleNode("//infoCompRetencion/periodoFiscal").InnerText;
                    sObligadoContabilidad = oDocument.SelectSingleNode("//infoCompRetencion/obligadoContabilidad") == null ? "" : oDocument.SelectSingleNode("//infoCompRetencion/obligadoContabilidad").InnerText;

                    
                    int registros = 0;
                    int PagLimite1 = 35;
                    int MaxPagina1 = 45;
                    int MaxSoloPagina = 70;

                    float posDetalleCliente = 0;
                    float posDetalleFactura = 0;
                    float posInfoAdicional = 0;

                    PdfWriter writer;
                    RoundRectangle rr = new RoundRectangle();
                    //Creamos un tipo de archivo que solo se cargará en la memoria principal
                    Document documento = new Document();
                    //Creamos la instancia para generar el archivo PDF
                    //Le pasamos el documento creado arriba y con capacidad para abrir o Crear y de nombre Mi_Primer_PDF
                    writer = PdfWriter.GetInstance(documento, ms);

                    iTextSharp.text.Font cabecera = GetArial(8);
                    iTextSharp.text.Font detalle = GetArial(7);
                    iTextSharp.text.Font detAdicional = GetArial(6);

                    documento.Open();
                    var oEvents = new ITextEvents();
                    writer.PageEvent = oEvents;
                    PdfContentByte canvas = writer.DirectContent;
                    iTextSharp.text.Image jpg = null;

                    jpg = iTextSharp.text.Image.GetInstance(pRutaLogo);
                    jpg.ScaleToFit(250f, 70f);

                    #region TablaDerecha
                    PdfPTable tableR = new PdfPTable(1);
                    PdfPTable innerTableD = new PdfPTable(1);

                    PdfPCell RUC = new PdfPCell(new Paragraph("R.U.C.: " + sRuc, cabecera));
                    RUC.Border = Rectangle.NO_BORDER;
                    RUC.Padding = 5f;
                    innerTableD.AddCell(RUC);

                    PdfPCell Factura = new PdfPCell(new Paragraph("C O M P R O B A N T E  D E  R E T E N C I Ó N", cabecera));
                    Factura.Border = Rectangle.NO_BORDER;
                    Factura.Padding = 5f;
                    innerTableD.AddCell(Factura);

                    PdfPCell NumFactura = new PdfPCell(new Paragraph("No. " + oDocument.SelectSingleNode("//infoTributaria/estab").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/ptoEmi").InnerText + "-" + oDocument.SelectSingleNode("//infoTributaria/secuencial").InnerText, cabecera));
                    NumFactura.Border = Rectangle.NO_BORDER;
                    NumFactura.Padding = 5f;
                    innerTableD.AddCell(NumFactura);

                    PdfPCell lblNumAutorizacion = new PdfPCell(new Paragraph("NÚMERO DE AUTORIZACIÓN:", cabecera));
                    lblNumAutorizacion.Border = Rectangle.NO_BORDER;
                    lblNumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(lblNumAutorizacion);

                    PdfPCell NumAutorizacion = new PdfPCell(new Paragraph(sNumAutorizacion.ToString(), cabecera));
                    NumAutorizacion.Border = Rectangle.NO_BORDER;
                    NumAutorizacion.Padding = 5f;
                    innerTableD.AddCell(NumAutorizacion);

                    PdfPCell FechaAutorizacion = new PdfPCell(new Paragraph("FECHA Y HORA AUTORIZACIÓN: " + sFechaAutorizacion, cabecera));
                    FechaAutorizacion.Border = Rectangle.NO_BORDER;
                    FechaAutorizacion.Padding = 5f;
                    innerTableD.AddCell(FechaAutorizacion);

                    PdfPCell Ambiente = new PdfPCell(new Paragraph("AMBIENTE: " + sAmbiente, cabecera));
                    Ambiente.Border = Rectangle.NO_BORDER;
                    Ambiente.Padding = 5f;
                    innerTableD.AddCell(Ambiente);

                    PdfPCell Emision = new PdfPCell(new Paragraph("EMISIÓN: " + sTipoEmision, cabecera));
                    Emision.Border = Rectangle.NO_BORDER;
                    Emision.Padding = 5f;
                    innerTableD.AddCell(Emision);

                    PdfPCell ClaveAcceso = new PdfPCell(new Paragraph("CLAVE DE ACCESO: ", cabecera));
                    ClaveAcceso.Border = Rectangle.NO_BORDER;
                    ClaveAcceso.Padding = 5f;
                    innerTableD.AddCell(ClaveAcceso);

                    Image image128 = BarcodeHelper.GetBarcode128(canvas, oDocument.SelectSingleNode("//infoTributaria/claveAcceso").InnerText, false, Barcode.CODE128);

                    PdfPCell ImgClaveAcceso = new PdfPCell(image128);
                    ImgClaveAcceso.Border = Rectangle.NO_BORDER;
                    ImgClaveAcceso.Padding = 5f;
                    ImgClaveAcceso.Colspan = 2;
                    ImgClaveAcceso.HorizontalAlignment = Element.ALIGN_CENTER;

                    innerTableD.AddCell(ImgClaveAcceso);

                    var ContenedorD = new PdfPCell(innerTableD);
                    ContenedorD.CellEvent = rr;
                    ContenedorD.Border = Rectangle.NO_BORDER;
                    tableR.AddCell(ContenedorD);
                    tableR.TotalWidth = 278f;
                    #endregion

                    #region TablaIzquierda
                    PdfPTable tableL = new PdfPTable(1);
                    PdfPTable innerTableL = new PdfPTable(1);

                    PdfPCell RazonSocial = new PdfPCell(new Paragraph(sRazonSocial, cabecera));
                    RazonSocial.Border = Rectangle.NO_BORDER;
                    RazonSocial.Padding = 5f;
                    innerTableL.AddCell(RazonSocial);

                    PdfPCell DirMatriz = new PdfPCell(new Paragraph("Dir Matriz: " + sMatriz, cabecera));
                    DirMatriz.Border = Rectangle.NO_BORDER;
                    DirMatriz.Padding = 5f;
                    innerTableL.AddCell(DirMatriz);

                    PdfPCell DirSucursal = new PdfPCell(new Paragraph("Dir Sucursal: " + sSucursal, cabecera));
                    DirSucursal.Border = Rectangle.NO_BORDER;
                    DirSucursal.Padding = 5f;
                    if (sSucursal.Length > 40)
                    {
                        DirSucursal.FixedHeight = 28f;
                    }
                    innerTableL.AddCell(DirSucursal);

                    PdfPCell ContribuyenteEspecial = new PdfPCell(new Paragraph("Contribuyente Especial Nro: " + sContribuyenteEspecial, cabecera));
                    ContribuyenteEspecial.Border = Rectangle.NO_BORDER;
                    ContribuyenteEspecial.Padding = 5f;
                    innerTableL.AddCell(ContribuyenteEspecial);

                    PdfPCell ObligadoContabilidad = new PdfPCell(new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD: " + sObligadoContabilidad, cabecera));
                    ObligadoContabilidad.Border = Rectangle.NO_BORDER;
                    ObligadoContabilidad.Padding = 5f;
                    innerTableL.AddCell(ObligadoContabilidad);

                    var ContenedorL = new PdfPCell(innerTableL);
                    ContenedorL.CellEvent = rr;
                    ContenedorL.Border = Rectangle.NO_BORDER;
                    tableL.AddCell(ContenedorL);
                    tableL.TotalWidth = 250f;

                    #endregion

                    #region Logo
                    PdfPTable tableLOGO = new PdfPTable(1);
                    PdfPCell logo = new PdfPCell(jpg);

                    logo.Border = Rectangle.NO_BORDER;
                    tableLOGO.AddCell(logo);
                    tableLOGO.TotalWidth = 250f;
                    #endregion

                    #region DetalleCliente
                    PdfPTable tableDetalleCliente = new PdfPTable(4);
                    tableDetalleCliente.TotalWidth = 540f;
                    tableDetalleCliente.WidthPercentage = 100;
                    float[] DetalleClientewidths = new float[] { 30f, 120f, 30f, 40f };
                    tableDetalleCliente.SetWidths(DetalleClientewidths);

                    var lblNombreCliente = new PdfPCell(new Paragraph("Razón Social / Nombres y Apellidos:", detalle));
                    lblNombreCliente.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(lblNombreCliente);
                    var NombreCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoCompRetencion/razonSocialSujetoRetenido").InnerText, detalle));
                    NombreCliente.Border = Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(NombreCliente);
                    var lblRUC = new PdfPCell(new Paragraph("RUC / CI:", detalle));
                    lblRUC.Border = Rectangle.TOP_BORDER;
                    tableDetalleCliente.AddCell(lblRUC);
                    var RUCcliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoCompRetencion/identificacionSujetoRetenido").InnerText, detalle));
                    RUCcliente.Border = Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tableDetalleCliente.AddCell(RUCcliente);

                    var lblFechaEmisionCliente = new PdfPCell(new Paragraph("Fecha Emisión:", detalle));
                    lblFechaEmisionCliente.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(lblFechaEmisionCliente);

                    var FechaEmisionCliente = new PdfPCell(new Paragraph(oDocument.SelectSingleNode("//infoCompRetencion/fechaEmision").InnerText, detalle));
                    FechaEmisionCliente.Border = Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(FechaEmisionCliente);
                    var lblGuiaRemision = new PdfPCell(new Paragraph("", detalle));
                    lblGuiaRemision.Border = Rectangle.BOTTOM_BORDER;
                    tableDetalleCliente.AddCell(lblGuiaRemision);
                    var GuiaRemision = new PdfPCell(new Paragraph("", detalle));
                    GuiaRemision.Border = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
                    tableDetalleCliente.AddCell(GuiaRemision);
                    #endregion

                    #region DetalleFactura
                    PdfPTable tableDetalleFactura = new PdfPTable(8);

                    tableDetalleFactura.TotalWidth = 540f;
                    tableDetalleFactura.WidthPercentage = 100;
                    tableDetalleFactura.LockedWidth = true;
                    float[] DetalleFacturawidths = new float[] { 45f, 45f, 25f, 35f, 35f, 35f, 35f, 35f };
                    tableDetalleFactura.SetWidths(DetalleFacturawidths);

                    var fontEncabezado = GetArial(7);
                    var encComprobante = new PdfPCell(new Paragraph("Comprobante", fontEncabezado));
                    encComprobante.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encNumero = new PdfPCell(new Paragraph("Numero", fontEncabezado));
                    encNumero.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encFechaEmision = new PdfPCell(new Paragraph("Fecha Emisión", fontEncabezado));
                    encFechaEmision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encEjercicioFiscal = new PdfPCell(new Paragraph("Ejercicio Fiscal", fontEncabezado));
                    encEjercicioFiscal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encBaseImponible = new PdfPCell(new Paragraph("Base Imponible para la retención", fontEncabezado));
                    encBaseImponible.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encImpuesto = new PdfPCell(new Paragraph("Impuesto", fontEncabezado));
                    encImpuesto.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPorcentajeRetencion = new PdfPCell(new Paragraph("Porcentaje Retención", fontEncabezado));
                    encPorcentajeRetencion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    var encPrecioUnitario = new PdfPCell(new Paragraph("Valor Retenido.", fontEncabezado));
                    encPrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                    tableDetalleFactura.AddCell(encComprobante);
                    tableDetalleFactura.AddCell(encNumero);
                    tableDetalleFactura.AddCell(encFechaEmision);
                    tableDetalleFactura.AddCell(encEjercicioFiscal);
                    tableDetalleFactura.AddCell(encBaseImponible);
                    tableDetalleFactura.AddCell(encImpuesto);
                    tableDetalleFactura.AddCell(encPorcentajeRetencion);
                    tableDetalleFactura.AddCell(encPrecioUnitario);

                    PdfPCell Comprobante = null;
                    PdfPCell Numero;
                    PdfPCell FechaEmision;
                    PdfPCell EjercicioFiscal;
                    PdfPCell BaseImponible;
                    PdfPCell Impuesto;
                    PdfPCell PorcentajeRetencion;
                    PdfPCell PrecioUnitario;

                    XmlNodeList Detalles;
                    Detalles = oDocument.SelectNodes("//impuestos/impuesto");
                    registros = Detalles.Count;

                    Dictionary<int,string> dictionary = new Dictionary<int, string>();
                    dictionary.Add(1, "Factura");
                    dictionary.Add(3, "LIQUIDACION DE COMPRAS DE BIENES Y PRESTACION DE SERVICIOS");
                    dictionary.Add(4, "Nota de crédito");
                    dictionary.Add(5, "Nota de débito");
                    dictionary.Add(6, "Guía de remisión");
                    dictionary.Add(7, "Comprobante de retención");
                    dictionary.Add(12, "DOCUMENTOS EMITIDOS POR IFIS");
                    


                    foreach (XmlNode Elemento in Detalles)
                    {

                        if (Elemento["codDocSustento"] != null)
                        {
                            if (dictionary.ContainsKey(int.Parse(Elemento["codDocSustento"].InnerText)))
                            {
                                Comprobante = new PdfPCell(new Phrase(dictionary[int.Parse(Elemento["codDocSustento"].InnerText)], detalle));
                            }
                            else
                            {
                                Comprobante = new PdfPCell(new Phrase("Comprobante", detalle));
                            }
                        }
                        else {
                            Comprobante = new PdfPCell(new Phrase("Comprobante", detalle));
                        }

                        Comprobante.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        Numero = new PdfPCell(new Phrase(Elemento["numDocSustento"] == null ? "" : Elemento["numDocSustento"].InnerText, detalle));
                        Numero.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        FechaEmision = new PdfPCell(new Phrase(Elemento["fechaEmisionDocSustento"] == null ? "" : Elemento["fechaEmisionDocSustento"].InnerText, detalle));
                        FechaEmision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        EjercicioFiscal = new PdfPCell(new Phrase(sEjercicioFiscal, detalle));
                        EjercicioFiscal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        BaseImponible = new PdfPCell(new Phrase(Elemento["baseImponible"].InnerText, detalle));
                        BaseImponible.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        switch (Elemento["codigo"].InnerText)
                        {
                            case "1":
                                Impuesto = new PdfPCell(new Phrase("RENTA", detalle));
                                break;
                            case "2":
                                Impuesto = new PdfPCell(new Phrase("IVA", detalle));
                                break;
                            case "6":
                                Impuesto = new PdfPCell(new Phrase("ISD", detalle));
                                break;
                            default: 
                                Impuesto = new PdfPCell(new Phrase("", detalle));
                                break;
                        }
                        Impuesto.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        PorcentajeRetencion = new PdfPCell(new Phrase(Elemento["porcentajeRetener"].InnerText + "%", detalle));
                        PorcentajeRetencion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                        PrecioUnitario = new PdfPCell(new Phrase(Elemento["valorRetenido"].InnerText, detalle));
                        PrecioUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                        tableDetalleFactura.AddCell(Comprobante);
                        tableDetalleFactura.AddCell(Numero);
                        tableDetalleFactura.AddCell(FechaEmision);
                        tableDetalleFactura.AddCell(EjercicioFiscal);
                        tableDetalleFactura.AddCell(BaseImponible);
                        tableDetalleFactura.AddCell(Impuesto);
                        tableDetalleFactura.AddCell(PorcentajeRetencion);
                        tableDetalleFactura.AddCell(PrecioUnitario);
                    }
                    #endregion

                    #region InformacionAdicional
                    var tableInfoAdicional = new PdfPTable(2);
                    tableInfoAdicional.TotalWidth = 200f;
                    float[] InfoAdicionalWidths = new float[] { 40f, 130f };
                    tableInfoAdicional.SetWidths(InfoAdicionalWidths);


                    var lblInfoAdicional = new PdfPCell(new Paragraph("Información Adicional", detalle));
                    lblInfoAdicional.Border = Rectangle.LEFT_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    lblInfoAdicional.Colspan = 2;
                    lblInfoAdicional.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    lblInfoAdicional.Padding = 5f;
                    tableInfoAdicional.AddCell(lblInfoAdicional);

                    var lblBottom = new PdfPCell(new Paragraph(" ", detalle));
                    lblBottom.Border = Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
                    lblBottom.Padding = 5f;
                    var Bottom = new PdfPCell(new Paragraph("  ", detalle));
                    Bottom.Border = Rectangle.RIGHT_BORDER + Rectangle.BOTTOM_BORDER;
                    Bottom.Padding = 5f;

                    XmlNodeList InfoAdicional;
                    InfoAdicional = oDocument.SelectNodes("//infoAdicional/campoAdicional");

                    PdfPCell lblCodigo;
                    PdfPCell Codigo;

                    foreach (XmlNode campoAdicional in InfoAdicional)
                    {
                        lblCodigo = new PdfPCell(new Paragraph(campoAdicional.Attributes["nombre"].Value, detAdicional));
                        lblCodigo.Border = Rectangle.LEFT_BORDER;
                        lblCodigo.Padding = 2f;

                        Codigo = new PdfPCell(new Paragraph(campoAdicional.InnerText.Length > 150 ? campoAdicional.InnerText.Substring(0, 150) + "..." : campoAdicional.InnerText, detAdicional));
                        Codigo.Border = Rectangle.RIGHT_BORDER;
                        Codigo.Padding = 2f;

                        tableInfoAdicional.AddCell(lblCodigo);
                        tableInfoAdicional.AddCell(Codigo);
                    }

                    tableInfoAdicional.AddCell(lblBottom);
                    tableInfoAdicional.AddCell(Bottom);

                    #endregion

                    tableR.WriteSelectedRows(0, 15, 292, 806, canvas);
                    tableLOGO.WriteSelectedRows(0, 1, 30, 780, canvas);
                    posDetalleCliente = 796 - tableR.TotalHeight;
                    posDetalleFactura = (posDetalleCliente - 8) - tableDetalleCliente.TotalHeight;

                    tableDetalleCliente.WriteSelectedRows(0, 5, 28, posDetalleCliente, canvas);
                    if (sSucursal.Length > 40 || sMatriz.Length > 40)
                    {
                        tableL.WriteSelectedRows(0, 5, 28, 705, canvas);
                    }
                    else
                    {
                        tableL.WriteSelectedRows(0, 5, 28, 695, canvas);
                    }
                    

                    if (registros <= PagLimite1)    // Una sola página 
                    {
                        tableDetalleFactura.WriteSelectedRows(0, PagLimite1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight, canvas);
                        posInfoAdicional = (posDetalleFactura - 10) - tableDetalleFactura.TotalHeight;
                        tableInfoAdicional.WriteSelectedRows(0, 16, 28, posInfoAdicional, canvas);

                    }
                    else if (registros > PagLimite1 && registros <= MaxPagina1)  // Una sola página con detalle en la siguiente.
                    {
                        tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight, canvas);
                        documento.NewPage();

                        tableInfoAdicional.WriteSelectedRows(0, 16, 28, 806, canvas);
                    }
                    else
                    {
                        tableDetalleFactura.WriteSelectedRows(0, MaxPagina1 + 1, 28, posDetalleCliente - 10 - tableDetalleCliente.TotalHeight, canvas);
                        documento.NewPage();

                        decimal Paginas = Math.Ceiling((Convert.ToDecimal(registros) - Convert.ToDecimal(PagLimite1)) / Convert.ToDecimal(MaxSoloPagina));
                        float posInicial = 0;
                        int faltantes = 0, ultimo = 0, hasta = 0;
                        ultimo = MaxPagina1 + 1;
                        hasta = MaxPagina1 + MaxSoloPagina + 1;
                        faltantes = registros - MaxPagina1 + 1;
                        for (int i = 0; i < Paginas; i++)
                        {
                            posInicial = 0;
                            documento.NewPage();
                            tableDetalleFactura.WriteSelectedRows(ultimo, hasta, 28, 806, canvas);
                            ultimo = hasta;
                            hasta = ultimo + MaxSoloPagina;
                            if (faltantes > MaxSoloPagina)
                            {
                                faltantes = faltantes - (hasta - ultimo);
                            }
                        }

                        posInicial = (806 - (faltantes * 11)) - 20;

                        if (posInicial > 120)
                        {
                            tableInfoAdicional.WriteSelectedRows(0, 16, 28, posInicial + 10, canvas);
                        }
                        else
                        {
                            tableInfoAdicional.WriteSelectedRows(0, 16, 28, 806, canvas);
                        }
                    }
                    writer.CloseStream = false;
                    documento.Close();
                    ms.Position = 0;
                    Bytes = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                //var oLog = new LogWriter();
                //oLog.RegistraLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName + "." + MethodBase.GetCurrentMethod().Name,
                //    "Error al generar PDF.", ex);
            }
            return Bytes;
        }
        /// <summary>
        /// Método privado para obtener el tipo de letra 
        /// </summary>
        /// <param name="pFontSize">Tamaño de la fuente</param>
        /// <returns></returns>
        private static iTextSharp.text.Font GetArial(int pFontSize)
        {
            var fontName = "Arial";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\Arial.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, pFontSize, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        }
        /// <summary>
        /// Método privado para obtener el tipo de letra
        /// </summary>
        /// <param name="pFontSize">Tamaño de la fuente</param>
        /// <returns></returns>
        private static iTextSharp.text.Font GetArialBlack(int pFontSize)
        {
            var fontName = "Arial";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\Arial.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, pFontSize, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        }
    }
}
