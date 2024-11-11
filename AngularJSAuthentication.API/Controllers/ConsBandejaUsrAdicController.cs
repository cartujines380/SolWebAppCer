using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJSAuthentication.API.Models;
//using AngularJSAuthentication.API.DAL;
using System.Data;
using System.Xml;
using System.Security.Claims;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/BandejaUsrAdic")]
    public class ConsBandejaUsrAdicController : ApiController
    {

        [Route("")]
        public IHttpActionResult Get(string IdEmpresa, string Ruc)
        {
            List<SegConsBandejaUsrAdic> Retorno = new List<SegConsBandejaUsrAdic>();
            SegConsBandejaUsrAdic TmpItem;

            DataSet ds = new DataSet();
            //ClsGeneral objWCF = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", IdEmpresa);
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                ds = new DataSet(); //objWCF.ConsultaBandejaUsuariosAdicionales(objWCF.gl_Session, xmlParam.OuterXml);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TmpItem = new SegConsBandejaUsrAdic();
                        TmpItem.Identificacion = Convert.ToString(item["Identificacion"]);
                        TmpItem.Usuario = Convert.ToString(item["Usuario"]);
                        TmpItem.Nombre = Convert.ToString(item["Nombre"]);
                        TmpItem.CorreoE = Convert.ToString(item["CorreoE"]);
                        TmpItem.UsrAutorizador = Convert.ToString(item["UsrAutorizador"]);
                        TmpItem.Estado = Convert.ToString(item["Estado"]);
                        Retorno.Add(TmpItem);
                    }
                }
            }
            catch (Exception)
            { }

            var jsonData = new
            {
                total = 1,
                page = 1,
                records = Retorno.Count(),
                rows = (
                  from Data in Retorno.ToList()
                  select new
                  {
                      Identificacion = Data.Identificacion,
                      cell = new string[] { 
                            Data.Identificacion.ToString(),
                            Data.Usuario.ToString(),
                            Data.Nombre.ToString(),
                            Data.CorreoE.ToString(),
                            Data.UsrAutorizador.ToString(),
                            Data.Estado.ToString()
                      }
                  }).ToArray()
            };

            return Ok(jsonData);

        }

    }
}