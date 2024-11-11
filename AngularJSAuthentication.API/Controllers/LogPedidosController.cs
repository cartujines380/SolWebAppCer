using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Xml;
using clibProveedores.Models;
using clibProveedores;
using AngularJSAuthentication.API.Handlers;

namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/LogPedidos")]
    public class LogPedidosController : ApiController
    {
        [AntiForgeryValidate]
        [ActionName("ConsLogPedidos")]
        [HttpGet]
        public formResponseLogPedidos ConsUsuarios(String criterioConsulta)
        {
            formResponseLogPedidos FormResponse = new formResponseLogPedidos();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            try
            {
                xmlParam.LoadXml(criterioConsulta);
                ds = objEjecucion.EjecucionGralLinea(xmlParam.OuterXml, 704, 1);
                // FIN - CONSULTO a STORE PROCEDURE DE BD
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    List<object> TmpList = new List<object>();
                    // INI - ARMADO DE ESTRUCTURA PARA GRID
                    List<liLogPedidos> TmpLstCons = new List<liLogPedidos>();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        liLogPedidos TmpItem = new liLogPedidos(); //pedConsPedidosEtiF
                        TmpItem.Idlog = Convert.ToInt32(item["Idlog"]);
                        TmpItem.FechaPedido = Convert.ToString(item["FechaPedido"]);
                        TmpItem.FechaProceso = Convert.ToString(item["FechaProceso"]);
                        TmpItem.NumPedido = Convert.ToString(item["NumPedido"]);
                        TmpItem.Archivo = Convert.ToString(item["Archivo"]);
                        TmpItem.Estado = Convert.ToString(item["Estado"]);
                        TmpItem.Linea = Convert.ToString(item["Linea"]);

                        TmpLstCons.Add(TmpItem);
                    }
                    FormResponse.root.Add(TmpLstCons);
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.codError = ds.Tables["TblEstado"].Rows[0]["CodError"].ToString();
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
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
