using AngularJSAuthentication.API.Handlers;
using clibProveedores;
using clibProveedores.Models;
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
    [Authorize]
    [RoutePrefix("api/Ped_ConsEtiPedidosActualizar")]
    public class Ped_ConsEtiPedidosActualizarController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("getActualizarSolicitudEtiquetas")]
        [HttpPost]
        public int ActualizarSolicitudEtiquetas(Act_pedConsPedidosEtiF EtiquetasACtualizar)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            int retorno = 0;
            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("OPCION","U");
                xmlParam.DocumentElement.SetAttribute("IDSOLICITUD", EtiquetasACtualizar.p_transaccion);
                foreach (Act_pedConsPedidosEtiF.pedConsPedidosEtiFAct dr in EtiquetasACtualizar.p_Etiquetas)
                {
                    XmlElement elem = xmlParam.CreateElement("EtiquetaDet");
                    elem.SetAttribute("CODARTICULO", dr.pCodArticulo);
                    elem.SetAttribute("CANDESPACHAR", Convert.ToString(dr.catDesp));
                    xmlParam.DocumentElement.AppendChild(elem);
                }

                ds = objEjecucion.EjecucionGralEtiDs(xmlParam.OuterXml, 800, 1);

                retorno = Convert.ToInt16(ds.Tables[0].Rows[0]["retorno"]);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return retorno;
        }
    }
}
