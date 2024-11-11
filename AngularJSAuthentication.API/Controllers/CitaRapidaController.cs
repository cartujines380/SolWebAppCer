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
    [RoutePrefix("api/CitaRapida")]
    public class CitaRapidaController : ApiController
    {

        [ActionName("BuscarDatosVarios")]
        [HttpGet]
        public FormResponseTransporte BuscarDatosVarios(string tipod, string dato,string ruc)
        {
            List<Tra_CitaRapidaCho> lst_retornoSolCho = new List<Tra_CitaRapidaCho>();
            List<Tra_CitaRapidaCVe> lst_retornoSolVe = new List<Tra_CitaRapidaCVe>();
            Tra_CitaRapidaCho mod_SolicitudChofer;
            Tra_CitaRapidaCVe mod_SolicitudVehiculo;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipod);
                xmlParam.DocumentElement.SetAttribute("ruc", ruc);
                xmlParam.DocumentElement.SetAttribute("dato", dato);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 315, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (tipod == "3")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolicitudChofer = new Tra_CitaRapidaCho();
                            mod_SolicitudChofer.id = Convert.ToString(item["id"]);
                            mod_SolicitudChofer.nombre = Convert.ToString(item["nombre"]);
                            mod_SolicitudChofer.apellido = Convert.ToString(item["apellido"]);
                            lst_retornoSolCho.Add(mod_SolicitudChofer);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retornoSolCho);
                    }
                    if (tipod == "4")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolicitudVehiculo = new Tra_CitaRapidaCVe();
                            mod_SolicitudVehiculo.id = Convert.ToString(item["id"]);
                            mod_SolicitudVehiculo.placa = Convert.ToString(item["placa"]);
                            mod_SolicitudVehiculo.codtipo = Convert.ToString(item["codigo"]);
                            lst_retornoSolVe.Add(mod_SolicitudVehiculo);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retornoSolVe);
                    }
                }
                else
                {
                    FormResponse.success = false;
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }


        [ActionName("BuscarProveedorDatos")]
        [HttpGet]
        public FormResponseTransporte BuscarProveedorDatos(string tipo, string ruc)
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
                    xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                    xmlParam.DocumentElement.SetAttribute("ruc", ruc);
                    ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 315, 1);
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
                    else
                    {
                        FormResponse.success = false;
                    }
                }
                catch (Exception ex)
                { }
                return FormResponse;

            }

        [ActionName("BuscarPedidosCitas")]
        [HttpGet]
        public FormResponseTransporte BuscarPedidosCitas(string tipoCita, string rucCita)
        {
            List<Tra_ModPedidosCitaRapida> lst_retornoSolBandeja = new List<Tra_ModPedidosCitaRapida>();
            Tra_ModPedidosCitaRapida mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipoCita);
                xmlParam.DocumentElement.SetAttribute("ruc", rucCita);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 315, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        mod_SolicitudBandeja = new Tra_ModPedidosCitaRapida();
                        mod_SolicitudBandeja.id = Convert.ToString(item["id"]);
                        mod_SolicitudBandeja.chkpedido = Convert.ToBoolean(item["chkpedido"]);
                        mod_SolicitudBandeja.pedido = Convert.ToString(item["pedido"]);
                        mod_SolicitudBandeja.factura = Convert.ToString(item["factura"]);
                        mod_SolicitudBandeja.extfactura = "0";
                        lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                    }

                    FormResponse.success = true;
                    FormResponse.root.Add(lst_retornoSolBandeja);
                }
                else
                {
                    FormResponse.success = false;
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;

        }


        [ActionName("grabarCitaRapida")]
        [HttpPost]
        public Boolean grabarCitaRapida(Tra_CitaRapidaGrabar CitaRapida)
        {
            Boolean Retorno = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                //Datos de la cabecera
                if (CitaRapida.p_cabecera != null)
                {
                    xmlParam.DocumentElement.SetAttribute("txtrucproveedor", CitaRapida.p_cabecera.txtrucproveedor);
                    xmlParam.DocumentElement.SetAttribute("txtnombreempresa", CitaRapida.p_cabecera.txtnombreempresa);
                    xmlParam.DocumentElement.SetAttribute("idchofer", CitaRapida.p_cabecera.idchofer);
                    xmlParam.DocumentElement.SetAttribute("cedulachofer", CitaRapida.p_cabecera.cedulachofer);
                    xmlParam.DocumentElement.SetAttribute("txtNombresPrimero", CitaRapida.p_cabecera.txtNombresPrimero);
                    xmlParam.DocumentElement.SetAttribute("txtApellidoPrimero", CitaRapida.p_cabecera.txtApellidoPrimero);
                    xmlParam.DocumentElement.SetAttribute("idayudante", CitaRapida.p_cabecera.idayudante);
                    xmlParam.DocumentElement.SetAttribute("cedulaayudante", CitaRapida.p_cabecera.cedulaayudante);
                    xmlParam.DocumentElement.SetAttribute("txtnombreayudante", CitaRapida.p_cabecera.txtnombreayudante);
                    xmlParam.DocumentElement.SetAttribute("txtApellidoayudate", CitaRapida.p_cabecera.txtApellidoayudate);
                    xmlParam.DocumentElement.SetAttribute("idvehiculo", CitaRapida.p_cabecera.idvehiculo);
                    xmlParam.DocumentElement.SetAttribute("txtplacavehiculo", CitaRapida.p_cabecera.txtplacavehiculo);
                    xmlParam.DocumentElement.SetAttribute("codtipo", CitaRapida.p_cabecera.codtipo);
                    xmlParam.DocumentElement.SetAttribute("usuarioCreacion", CitaRapida.p_cabecera.usuarioCreacion);
                    xmlParam.DocumentElement.SetAttribute("codAlmacenDestino", CitaRapida.p_cabecera.codAlmacenDestino);
                    xmlParam.DocumentElement.SetAttribute("fechacita", CitaRapida.p_cabecera.txtfechasolicitud);
                    xmlParam.DocumentElement.SetAttribute("horainicial", CitaRapida.p_cabecera.horainicial);
                    xmlParam.DocumentElement.SetAttribute("horafinal", CitaRapida.p_cabecera.horafinal);
          

                }
                //Datos Detalle Cita Rapida
                if (CitaRapida.p_detalle != null)
                {
                    foreach (Tra_CitaRapidaGrabar.Tra_ModPedidosCitaRapidaGrabar dr in CitaRapida.p_detalle)
                    {
                        XmlElement elem = xmlParam.CreateElement("DetalleCita");

                        elem.SetAttribute("id", dr.id);
                        elem.SetAttribute("chkpedido", dr.chkpedido.ToString());
                        elem.SetAttribute("pedido", dr.pedido);
                        elem.SetAttribute("factura", dr.factura);
                        elem.SetAttribute("extfactura", dr.extfactura);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
               
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 317, 1); //Articulo.Sol_P_Grabar	102	
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
