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
    [RoutePrefix("api/CompradorModi")]
    public class CompradorModiController : ApiController
    {
        [ActionName("ModificarComprador")]
        [HttpPost]
        public FormResponseModelo ModificarComprador(Grabar_Comprador DatosModi)
        {
            FormResponseModelo _oRetorno = new FormResponseModelo();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "2");
                xmlParam.DocumentElement.SetAttribute("NombreComprador", DatosModi.cabecera.NombreComprador);
                xmlParam.DocumentElement.SetAttribute("CorreoComprador", DatosModi.cabecera.CorreoComprador);
                xmlParam.DocumentElement.SetAttribute("CorreoAsistenteComprador", DatosModi.cabecera.CorreoAsistenteComprador);
                xmlParam.DocumentElement.SetAttribute("IdComprador", DatosModi.cabecera.IdComprador);
                xmlParam.DocumentElement.SetAttribute("Estado", DatosModi.cabecera.Estado);

                foreach (var it in DatosModi.detalleComprador)
                {
                    XmlElement elem = xmlParam.CreateElement("CompradorProveedor");
                    elem.SetAttribute("CodigoSap", it.codSap);
                    elem.SetAttribute("IdCompradorProveedor", it.idCompradorProveedor);
                    elem.SetAttribute("IdComprador", DatosModi.cabecera.IdComprador);
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
                        if (Convert.ToString(item["ESTADO"]) == "MODIFICADO")
                        {
                            _oRetorno.root.Add(Convert.ToString(item["RETORNO"]));
                            _oRetorno.cCodError = "0";
                            _oRetorno.cMsgError = "ACTUALIZADA";
                            _oRetorno.lSuccess = true;
                        }
                        else
                         if (Convert.ToString(item["ESTADO"]) == "ACTIVO")
                        {
                            _oRetorno.cCodError = "110";
                            _oRetorno.cMsgError = Convert.ToString(item["RETORNO"]);
                            _oRetorno.lSuccess = true;
                        }else
                        {
                            _oRetorno.cCodError = "110";
                            _oRetorno.cMsgError = "Error Comunicacion";
                            _oRetorno.lSuccess = false;
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
    }
}
