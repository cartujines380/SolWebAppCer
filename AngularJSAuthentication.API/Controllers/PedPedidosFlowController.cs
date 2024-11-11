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
using System.Xml.Linq;
using System.Security.Claims;
using clibProveedores.Models;
using clibProveedores;
using clibSeguridadCR;
using Ionic.Zip;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp;
using PdfSharp.Drawing;
using System.Threading;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using AngularJSAuthentication.API.Handlers;
using Microsoft.Reporting.WebForms;
using GenCode128;
using BarcodeLib;
using System.Drawing;
namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/PedPedidosFlow")]
    public class PedPedidosFlowController : ApiController
    {
        [ActionName("ActualizaPedidoCrossFlow")]
        [HttpPost]
        public formResponsePedidos getActualizaPedidoCrossFlow(pedidosFlow PedidoCrossFlow)
        {
            formResponsePedidos FormResponse = new formResponsePedidos();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            string estado = "";
            try
            {

                xmlParam.LoadXml("<Root />");
                foreach (var item in PedidoCrossFlow.p_detallePedidos)
                {
                    XmlElement elem = xmlParam.CreateElement("Detalle");
                    elem.SetAttribute("IdEmpresa", "1");
                    elem.SetAttribute("IdPedido", item.pIdPedido);
                    elem.SetAttribute("CodArticulo", item.codArticulo);
                    elem.SetAttribute("CantidadPlanificada", item.cantPlanificada.ToString());
                    elem.SetAttribute("Cantidad", item.cantReal.ToString());
                    elem.SetAttribute("Usuario", PedidoCrossFlow.p_usuario);
                    elem.SetAttribute("Estado", PedidoCrossFlow.p_estado);
                    estado = PedidoCrossFlow.p_estado;
                    xmlParam.DocumentElement.AppendChild(elem);

                }

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 508, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse.success = true;
                    if (estado == "1")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            if (Convert.ToString(item["CORREO"])!="")
                            {
                                string rutaEmail = System.Web.Hosting.HostingEnvironment.MapPath("~/PlantillasEMail") + "\\PedidosFlow.htm";
                                //string asuntoEmail = "Desbloqueo de Contraseña - Portal Provedores - Sipecom";
                                string asuntoEmail = "Pedidos Flow - Portal de Proveedores";
                                clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();
                                string mensajeEmail = System.IO.File.ReadAllText(rutaEmail);
                                mensajeEmail = mensajeEmail.Replace("_@MensajeCorreo", Convert.ToString(item["MENSAJE"]));
                                                              
                            }
                            else
                            {
                                FormResponse.success = false;
                                FormResponse.codError = "-100";
                                FormResponse.msgError = Convert.ToString(item["MENSAJE"]);
                            }
                           
                        }
                    }
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }




            }
            catch (Exception e)
            {
                FormResponse.success = false;
                FormResponse.codError = "999";
                FormResponse.msgError = e.Message.ToString();
            }

            return FormResponse;
        }
    }
}
