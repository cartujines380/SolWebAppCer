﻿using AngularJSAuthentication.API.Models;
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
    [RoutePrefix("api/CatalogoModi")]
    public class CatalogoModiController : ApiController
    {
        [ActionName("ModificarCatalogo")]
        [HttpPost]
        public FormResponseModelo ModificarCatalogo(Grabar_Catalogo DatosModi)
        {
            FormResponseModelo _oRetorno = new FormResponseModelo();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", "4");
                xmlParam.DocumentElement.SetAttribute("idCabecera", DatosModi.cabecera.idCabecera);
                xmlParam.DocumentElement.SetAttribute("IdCatalogo", DatosModi.cabecera.idCatalogo);
                xmlParam.DocumentElement.SetAttribute("descripcion", DatosModi.cabecera.descripcion);
                xmlParam.DocumentElement.SetAttribute("Estado", DatosModi.cabecera.Estado);

                foreach (var it in DatosModi.detalleCatalogo)
                {
                    XmlElement elem = xmlParam.CreateElement("CatalogoDetalle");
                    elem.SetAttribute("idDetalle", it.idDetalle);
                    elem.SetAttribute("codigoDetalle", it.codigoDetalle);
                    elem.SetAttribute("descripcion", it.descripcion);
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

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 851, 1);
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
