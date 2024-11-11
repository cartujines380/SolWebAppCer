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
    [RoutePrefix("api/Tolerancia")]
    public class ToleranciaController : ApiController
    {
        [ActionName("catalogoTipoPedido")]
        [HttpGet]
        public FormResponseModelo catalogoTipoPedido(string tipo, string IdCatalogo)
        {
            FormResponseModelo _oRetorno = new FormResponseModelo();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            List<repCatalogo> lst_catalogo = new List<repCatalogo>();
            repCatalogo consultaCatalogo;
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("IdCatalogo", IdCatalogo);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 852, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        consultaCatalogo = new repCatalogo();
                        consultaCatalogo.codigo = Convert.ToString(item["Codigo"]);
                        consultaCatalogo.descripcion = Convert.ToString(item["Descripcion"]);
                        lst_catalogo.Add(consultaCatalogo);
                    }
                    _oRetorno.root.Add(lst_catalogo);
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
