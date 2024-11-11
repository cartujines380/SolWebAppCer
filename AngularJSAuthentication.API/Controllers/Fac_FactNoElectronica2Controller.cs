using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Models;

using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Security.Claims;

using clibProveedores.Models;
using AngularJSAuthentication.API.Handlers;

namespace AngularJSAuthentication.API.Controllers
{
        [Authorize]
    [RoutePrefix("api/FacFactNoElec2")]
    public class FacFactNoElec2Controller : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("GeneraFilesXmlPDF")]
        [HttpPost]
        public formResponseFacturas GetGeneraFilesXmlPDF(facGeneraFile objInfo)
        {
            formResponseFacturas FormResponse = new formResponseFacturas();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            try
            {
                string RutaUrlWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaVirtualDownload"]).Trim();
                string RutaDirWebAPP = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaFisicaDownload"]).Trim();

                if (objInfo.tipo == "XML")
                {
                    System.IO.File.WriteAllText(RutaDirWebAPP + objInfo.nombreFile + ".xml", objInfo.dataXML);
                    string[] result = new string[1] { RutaUrlWebAPP + objInfo.nombreFile + ".xml" };
                    FormResponse.root.Add(result);
                }
                else if (objInfo.tipo == "PDF")
                {
                    eComp.PDF.GeneraPDF PDFDATA = new eComp.PDF.GeneraPDF();

                    byte[] dato = null;
                    String RutaLogo;
                    string lv_DirectorioImagenLogoNotificacion = System.Configuration.ConfigurationManager.AppSettings["RutaImagenesArchivoPDF"];
                    string lv_porcentaje = System.Configuration.ConfigurationManager.AppSettings["Porcentaje"];
                    XmlDocument oDocument = new XmlDocument();
                    String sRazonSocial = "", sMatriz = "", sTipoEmision = "",
                           sAmbiente = "", sFechaAutorizacion = "", Cultura = "",
                           sSucursal = "", sRuc = "", sContribuyenteEspecial = "", sAmbienteVal = "",
                           sNumAutorizacion = "", sObligadoContabilidad = "",
                           sFechaEmision = "", FechaIniVigAutSRI = "";
      
                    oDocument.LoadXml(objInfo.dataXML.ToString());

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
                    sContribuyenteEspecial = oDocument.SelectSingleNode("//infoFactura/contribuyenteEspecial") == null ? "" : oDocument.SelectSingleNode("//infoFactura/contribuyenteEspecial").InnerText;
                    sObligadoContabilidad = oDocument.SelectSingleNode("//infoFactura/obligadoContabilidad") == null ? "" : oDocument.SelectSingleNode("//infoFactura/obligadoContabilidad").InnerText;
                    try
                    {
                      
                        string fechaTmp = oDocument.SelectSingleNode("//infoFactura/fechaEmision").InnerText;
                        sFechaEmision = fechaTmp;
                    }
                    catch (Exception)
                    {
                        sFechaEmision = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    }

                    if (Convert.ToDateTime(sFechaEmision) <= Convert.ToDateTime("31/05/2016"))
                    {
                        lv_porcentaje = System.Configuration.ConfigurationManager.AppSettings["Porcentaje12"];
                    }
                    else
                    {
                        if (Convert.ToDateTime(sFechaEmision) <= Convert.ToDateTime("31/05/2017"))
                        {
                            lv_porcentaje = System.Configuration.ConfigurationManager.AppSettings["Porcentaje"];
                        }
                        else
                        {
                            lv_porcentaje = System.Configuration.ConfigurationManager.AppSettings["Porcentaje12"];

                        }
                    }

              
                    if (System.IO.File.Exists(System.IO.Path.Combine(lv_DirectorioImagenLogoNotificacion, objInfo.codSap + ".png")))
                    {
                        RutaLogo = System.IO.Path.Combine(lv_DirectorioImagenLogoNotificacion, objInfo.codSap + ".png");
                    }
                    else
                    {
                        RutaLogo = System.IO.Path.Combine(lv_DirectorioImagenLogoNotificacion, "Logo_Defecto.png");
                    }
                    dato = PDFDATA.GeneraFactura(objInfo.dataXML, RutaLogo, lv_porcentaje);

                    System.IO.File.WriteAllBytes(RutaDirWebAPP + objInfo.nombreFile + ".pdf", dato);
                    string[] result = new string[1] { RutaUrlWebAPP + objInfo.nombreFile + ".pdf" };
                    FormResponse.root.Add(result);
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

    }

}