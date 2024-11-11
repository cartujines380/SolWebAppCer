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
    [RoutePrefix("api/SolicitudCita")]
    public class SolicitudCitaController : ApiController
    {
        [ActionName("getdatos")]
        [HttpGet]
        public FormResponseTransporte getdatos(string id, string citas)
        {
            List<Tra_Parametros> lst_retornoSolBandeja = new List<Tra_Parametros>();
            Tra_Parametros mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", id);
                xmlParam.DocumentElement.SetAttribute("citas", citas);
                 ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 310, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {

                        mod_SolicitudBandeja = new Tra_Parametros();
                        mod_SolicitudBandeja.dato = Convert.ToString(item["dato"]);
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


        [ActionName("getCargarHorario")]
        [HttpGet]
        public FormResponseTransporte getCargarHorario(string bodega,string codProveedor,string fecha )
        {
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            List<Tra_SolicitudCitasHorario> lst_retornoHorario = new List<Tra_SolicitudCitasHorario>();
            Tra_SolicitudCitasHorario tmpHorario;
            ProcesoWs.ServBaseProceso ProcesoWS=new ProcesoWs.ServBaseProceso();
            string aux = "";
            aux = ProcesoWS.getHorarioCitas(bodega, codProveedor, fecha);
            XmlDocument lst_mod_Tra_pedidoCitas= new XmlDocument();
            lst_mod_Tra_pedidoCitas.LoadXml(aux);
            if (lst_mod_Tra_pedidoCitas.SelectSingleNode("//Root/@NumError").Value =="0")
            {
                foreach (XmlNode Elementot in lst_mod_Tra_pedidoCitas.DocumentElement)
                {
                    XmlDocument tmpd = new XmlDocument();
                    tmpd.LoadXml(Elementot.OuterXml);
                    XmlNode Elemento = tmpd.DocumentElement;
                    tmpHorario = new Tra_SolicitudCitasHorario();
                    tmpHorario.Dias = Elemento.SelectSingleNode("//Horario/@DIA").Value;
                    tmpHorario.Hora_Inicial = Elemento.SelectSingleNode("//Horario/@HORA_INICIO").Value;
                    tmpHorario.Hora_Final = Elemento.SelectSingleNode("//Horario/@HORA_FIN").Value;
                    tmpHorario.Color = Elemento.SelectSingleNode("//Horario/@COLOR").Value;
                    tmpHorario.ColorTexto = Elemento.SelectSingleNode("//Horario/@COLORTEXTO").Value;
                    tmpHorario.Titulo = Elemento.SelectSingleNode("//Horario/@TITULO").Value;
                    tmpHorario.Id = Elemento.SelectSingleNode("//Horario/@ID").Value;
                    lst_retornoHorario.Add(tmpHorario);
                }
                FormResponse.success = true;
                FormResponse.root.Add(lst_retornoHorario);
            }else
            {
                FormResponse.success = false;
            }
            
            

            return FormResponse;  
        }

        [ActionName("conBuscarSolicitud")]
        [HttpGet]
        public FormResponseTransporte conBuscarSolicitud(string tipo,string fechadesde, string fechahasta, string numero, string codProveedor,string bodega)
        {
            List<Tra_SolicitudCitas> lst_retornoSolBandeja = new List<Tra_SolicitudCitas>();
            Tra_SolicitudCitas mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("numero", numero);
                xmlParam.DocumentElement.SetAttribute("codProveedor", codProveedor);
                xmlParam.DocumentElement.SetAttribute("fechadesde", fechadesde);
                xmlParam.DocumentElement.SetAttribute("fechahasta", fechahasta);
                xmlParam.DocumentElement.SetAttribute("bodega", bodega);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 310, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {

                        mod_SolicitudBandeja = new Tra_SolicitudCitas();
                        mod_SolicitudBandeja.idconsolidacion = Convert.ToString(item["IDCONSOLIDACION"]);
                        mod_SolicitudBandeja.emision = Convert.ToString(item["EMISION"]);
                        mod_SolicitudBandeja.almacensolicitante = Convert.ToString(item["ALMACENSOLICTANTE"]);
                        mod_SolicitudBandeja.almacendestino = Convert.ToString(item["ALMACENDESTINO"]);
                        mod_SolicitudBandeja.estadoconsolidacion = Convert.ToString(item["ESTADOCONSOLIDACION"]);
                        mod_SolicitudBandeja.caducidadsolicitud = Convert.ToString(item["CADUCIDADSOLICITUD"]);
                        mod_SolicitudBandeja.vehiculo = Convert.ToString(item["VEHICULO"]);
                        mod_SolicitudBandeja.chofer = Convert.ToString(item["CHOFER"]);
                        mod_SolicitudBandeja.asistente = Convert.ToString(item["ASISTENTE"]);
                        mod_SolicitudBandeja.variacita = Convert.ToString(item["VARIACITA"]);
                        mod_SolicitudBandeja.citarapida = Convert.ToString(item["CITARAPIDA"]);
                        mod_SolicitudBandeja.estado = "0";
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


        [ActionName("generarCita")]
        [HttpGet]
        public FormResponse generarCita(string idconsolidacion, string fechacita, string horainicial, string horafinal, string CodSap, string usuarioproveedor)
        {
            FormResponse Response = new FormResponse();
            Response.codError = "0";
            Response.msgError = "";
            string Mensaje = "";
            string idretorno = "";
            XmlDocument xmlParam = new XmlDocument();
            XmlDocument xmlResp = new XmlDocument();
            List<Tra_SolicitudCita> lst_mod_Tra_SolicitudCita = new List<Tra_SolicitudCita>();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("Tipo", "1");
                xmlParam.DocumentElement.SetAttribute("idconsolidacion", idconsolidacion);
                xmlParam.DocumentElement.SetAttribute("fechacita", fechacita);
                xmlParam.DocumentElement.SetAttribute("horainicial", horainicial);
                xmlParam.DocumentElement.SetAttribute("horafinal", horafinal);
                xmlParam.DocumentElement.SetAttribute("estado", "EV");
                xmlParam.DocumentElement.SetAttribute("usuario", usuarioproveedor);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 312, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        Mensaje = Convert.ToString(item["MENSAJE"]);
                    }
                    if (Mensaje == "EXISTE")
                    {

                        Response.success = false;
                        Response.msgError = Mensaje;
                    }
                    else
                    {
                        Response.success = true;
                        Response.msgError = Mensaje;
                        XmlDocument xmlParamCita = new XmlDocument();
                        xmlParamCita.LoadXml("<SOLICITUDCITA />");
                        int i=1,j=0;
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            XmlElement elem = xmlParamCita.CreateElement("DATOS");
                            elem.SetAttribute("Bultos", Convert.ToString(item["Bultos"]));
                            elem.SetAttribute("CodEstado", Convert.ToString(item["CodEstado"]));
                            elem.SetAttribute("Fecha_propuesta", Convert.ToString(item["Fecha_propuesta"]));
                            elem.SetAttribute("hora_fin", Convert.ToString(item["hora_fin"]));
                            elem.SetAttribute("hora_inicio", Convert.ToString(item["hora_inicio"]));
                            elem.SetAttribute("ID_Bodega", Convert.ToString(item["ID_Bodega"]));
                            idretorno = Convert.ToString(item["ID_consolidacion"]);
                            elem.SetAttribute("ID_consolidacion", Convert.ToString(item["ID_consolidacion"]));
                            elem.SetAttribute("ID_Factura", Convert.ToString(item["ID_Factura"]));
                            elem.SetAttribute("ID_Proveedor", Convert.ToString(item["ID_Proveedor"]));
                            elem.SetAttribute("Orden_compra", Convert.ToString(item["Orden_compra"]));
                            elem.SetAttribute("Pallet", Convert.ToString(item["Pallet"]));
                            elem.SetAttribute("Tipo_Vehiculo", Convert.ToString(item["Tipo_Vehiculo"]));
                            xmlParamCita.DocumentElement.AppendChild(elem);
                            i++;
                        }
                        ////foreach (DataRow item in ds.Tables[2].Rows)
                        ////{
                        ////    XmlElement elem = xmlParamCita.CreateElement("DATOSCHOFERE");
                        ////    elem.SetAttribute("Bultos", Convert.ToString(item["Bultos"]));
                        ////    elem.SetAttribute("ID_consolidacion", Convert.ToString(item["ID_consolidacion"]));
                        ////    elem.SetAttribute("Chofer", Convert.ToString(item["Chofer"]));
                        ////    elem.SetAttribute("Placa", Convert.ToString(item["Placa"]));
                        ////    elem.SetAttribute("Pallet", Convert.ToString(item["Pallet"]));
                        ////    elem.SetAttribute("ID_Bodega", Convert.ToString(item["ID_Bodega"]));
                        ////    xmlParamCita.DocumentElement.AppendChild(elem);
                        ////    j++;
                        ////}
                        Response.codError = "";
                        Response.msgError = "";
                        Response.success = true;
                        Response.root.Add(Mensaje);
                        ProcesoWs.ServBaseProceso ProcesoBase = new ProcesoWs.ServBaseProceso();
                        var datos = ProcesoBase.setSolcitudCitaCER(xmlParamCita, i, j);
                        Response.codError = datos.codError.Trim();
                        Response.msgError = datos.msgError;
                        Response.success = datos.success;
                        Response.root.Add(Mensaje);
                        foreach (var item in datos.root)
                        {
                            Response.root.Add(item);
                        }
                        if (Response.codError.Trim() != "" && Response.codError.Trim() != "0")
                        {
                            XmlDocument xmlParamtmp = new XmlDocument();
                            xmlParamtmp.LoadXml("<Root />");
                            xmlParamtmp.DocumentElement.SetAttribute("Tipo", "2");
                            xmlParamtmp.DocumentElement.SetAttribute("idconsolidacion", idretorno);
                            ds = objEjecucion.EjecucionGralDs(xmlParamtmp.OuterXml, 312, 1);
                            if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                            {
                                Response.success = false;
                                Response.msgError = "";
                            }
                        }
                        
                    }
                 }
                else
                {
                    Response.success = false;
                    Response.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    Response.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }
            }
            catch (Exception ex)
            {
                Response.success = false;
                Response.codError = "10000";
                Response.msgError = ex.Message;
            }
            return Response;

          
           
        }
    }
}
