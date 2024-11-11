using AngularJSAuthentication.API.Models;
using clibProveedores;
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
    [RoutePrefix("api/Comprador")]
    public class CompradorController : ApiController
    {
        [ActionName("ConsultarComprador")]
        [HttpGet]
        public FormResponseModelo ConsultarComprador(string codigoComprador, string NombreComprador, string Estado)
        {
            FormResponseModelo _oRetorno = new FormResponseModelo();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repComprador> lst_comprador = new List<repComprador>();
            repComprador consultaComprador;
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "3");
                xmlParam.DocumentElement.SetAttribute("IdComprador", codigoComprador);
                xmlParam.DocumentElement.SetAttribute("NombreComprador", NombreComprador);
                xmlParam.DocumentElement.SetAttribute("Estado", Estado);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 850, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        consultaComprador = new repComprador();
                        consultaComprador.idComprador = Convert.ToString(item["idComprador"]);
                        consultaComprador.nombreComprador = Convert.ToString(item["nombreComprador"]);
                        consultaComprador.correoComprador = Convert.ToString(item["correoComprador"]);
                        consultaComprador.correoAsistenteComprador = Convert.ToString(item["correoAsistenteComprador"]);
                        consultaComprador.estado = Convert.ToString(item["estado"]);
                        lst_comprador.Add(consultaComprador);
                    }
                    _oRetorno.root.Add(lst_comprador);
                    _oRetorno.cCodError = "0";
                    _oRetorno.lSuccess = true;
                }
            }
            catch (Exception ex)
            {
                _oRetorno.lSuccess = false;
                _oRetorno.cCodError = "100";
                _oRetorno.cMsgError = "Error Comunicacion";
            }

            return _oRetorno;
        }

        [ActionName("BuscarUnComprador")]
        [HttpGet]
        public FormResponseModelo BuscarUnComprador(string codigoComprador)
        {
            FormResponseModelo _oRetorno = new FormResponseModelo();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repComprador> lst_comprador = new List<repComprador>();
            repComprador consultaComprador;
            List<repCompradorProvedor2> lst_comprador2 = new List<repCompradorProvedor2>();
            repCompradorProvedor2 consultaComprador2;
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "5");
                xmlParam.DocumentElement.SetAttribute("IdComprador", codigoComprador);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 850, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        consultaComprador = new repComprador();
                        consultaComprador.idComprador = Convert.ToString(item["idComprador"]);
                        consultaComprador.nombreComprador = Convert.ToString(item["nombreComprador"]);
                        consultaComprador.correoComprador = Convert.ToString(item["correoComprador"]);
                        consultaComprador.correoAsistenteComprador = Convert.ToString(item["correoAsistenteComprador"]);
                        consultaComprador.estado = Convert.ToString(item["estado"]);
                        lst_comprador.Add(consultaComprador);
                    }
                    _oRetorno.root.Add(lst_comprador);
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        consultaComprador2 = new repCompradorProvedor2();
                        consultaComprador2.idCompradorProveedor = Convert.ToString(item["idCompradorProveedor"]);
                        consultaComprador2.codSap = Convert.ToString(item["CodigoSap"]);
                        consultaComprador2.nombreComercial = Convert.ToString(item["NomComercial"]);
                        if (Convert.ToString(item["Estado"]) == "A")
                        {
                            consultaComprador2.Estado = true;
                        }
                        else
                        {
                            consultaComprador2.Estado = false;
                        }
                        lst_comprador2.Add(consultaComprador2);
                    }
                    _oRetorno.root.Add(lst_comprador2);
                    _oRetorno.cCodError = "0";
                    _oRetorno.lSuccess = true;
                }
            }
            catch (Exception ex)
            {
                _oRetorno.lSuccess = false;
                _oRetorno.cCodError = "100";
                _oRetorno.cMsgError = "Error Comunicacion";
            }

            return _oRetorno;
        }

        [ActionName("GrabarComprador")]
        [HttpPost]
        public FormResponseModelo GrabarComprador(Grabar_Comprador DatosIng)
        {
            FormResponseModelo _oRetorno = new FormResponseModelo();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "1");
                xmlParam.DocumentElement.SetAttribute("NombreComprador", DatosIng.cabecera.NombreComprador);
                xmlParam.DocumentElement.SetAttribute("CorreoComprador", DatosIng.cabecera.CorreoComprador);
                xmlParam.DocumentElement.SetAttribute("CorreoAsistenteComprador", DatosIng.cabecera.CorreoAsistenteComprador);
                xmlParam.DocumentElement.SetAttribute("Estado", DatosIng.cabecera.Estado);

                foreach (var it in DatosIng.detalleComprador)
                {
                    XmlElement elem = xmlParam.CreateElement("CompradorProveedor");
                    elem.SetAttribute("CodigoSap", it.codSap);
                    if (it.Estado == true)
                    {
                        elem.SetAttribute("Estado", "A");
                    }
                    else
                    {
                        elem.SetAttribute("Estado", "I");
                    }


                    xmlParam.DocumentElement.AppendChild(elem);
                }

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 850, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        if (Convert.ToString(item["ESTADO"]) == "INGRESADO")
                        {
                            _oRetorno.root.Add(Convert.ToString(item["RETORNO"]));
                            _oRetorno.cCodError = "0";
                            _oRetorno.lSuccess = true;
                        }
                        else
                        {
                            if (Convert.ToString(item["ESTADO"]) == "EXISTE")
                            {
                                _oRetorno.cMsgError = "EXISTE";
                                _oRetorno.cCodError = "0";
                                _oRetorno.lSuccess = false;
                            }
                            else
                            {
                                _oRetorno.cCodError = "110";
                                _oRetorno.cMsgError = "Error Comunicacion";
                                _oRetorno.lSuccess = false;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _oRetorno.cCodError = "100";
                _oRetorno.cMsgError = "Error Comunicacion";
            }

            return _oRetorno;
        }

     

        [ActionName("catalogoEmpresa")]
        [HttpGet]
        public FormResponseModelo _catalogoEmpresa(string tipo)
        {
            FormResponseModelo _oRetorno = new FormResponseModelo();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repCompradorProvedor> lst_comprador = new List<repCompradorProvedor>();
            repCompradorProvedor consultaComprador;
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 850, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        consultaComprador = new repCompradorProvedor();
                        consultaComprador.codigo = Convert.ToString(item["codigo"]);
                        consultaComprador.nombreproveedor = Convert.ToString(item["nombreproveedor"]);
                        consultaComprador.estado = Convert.ToString(item["estado"]);
                        lst_comprador.Add(consultaComprador);
                    }
                    _oRetorno.root.Add(lst_comprador);
                    _oRetorno.cCodError = "0";
                    _oRetorno.lSuccess = true;
                }
            }
            catch (Exception ex)
            {
                _oRetorno.cCodError = "100";
                _oRetorno.cMsgError = "Error Comunicacion";
            }

            return _oRetorno;
        }
    }
}
