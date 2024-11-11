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
    [RoutePrefix("api/MantenimientoUsrAdic")]
    public class ConsDatosUsrAdicController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get(string IdEmpresa, string Ruc, string Usuario)
        {
            List<SegConsDatosUsrAdic_Zonas> olZonas = new List<SegConsDatosUsrAdic_Zonas>();
            SegConsDatosUsrAdic oUsr = new SegConsDatosUsrAdic();

            DataSet ds = new DataSet();
            //ClsGeneral objWCF = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("IdEmpresa", IdEmpresa);
                xmlParam.DocumentElement.SetAttribute("Ruc", Ruc);
                xmlParam.DocumentElement.SetAttribute("Usuario", Usuario);
                ds = new DataSet(); //objWCF.ConsultaDatosUsuarioAdicional(objWCF.gl_Session, xmlParam.OuterXml);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        DataRow drA = ds.Tables[1].Rows[0];
                        oUsr.Apellido1 = Convert.ToString(drA["Apellido1"]);
                        oUsr.Apellido2 = Convert.ToString(drA["Apellido2"]);
                        oUsr.Celular = Convert.ToString(dr["Celular"]);
                        oUsr.Ciudad = Convert.ToString(drA["Ciudad"]);
                        oUsr.CorreoE = Convert.ToString(dr["CorreoE"]);
                        oUsr.Direccion = Convert.ToString(drA["Direccion"]);
                        oUsr.Estado = Convert.ToString(dr["Estado"]);
                        oUsr.EstadoCivil = Convert.ToString(drA["EstadoCivil"]);
                        oUsr.Genero = Convert.ToString(drA["Genero"]);
                        oUsr.Identificacion = Convert.ToString(drA["Identificacion"]);
                        oUsr.IdParticipante = Convert.ToInt32(dr["IdParticipante"]);
                        oUsr.Nombre1 = Convert.ToString(drA["Nombre1"]);
                        oUsr.Nombre2 = Convert.ToString(drA["Nombre2"]);
                        oUsr.Pais = Convert.ToString(drA["Pais"]);
                        oUsr.Provincia = Convert.ToString(drA["Provincia"]);
                        oUsr.Telefono = Convert.ToString(dr["Telefono"]);
                        oUsr.TipoIdent = Convert.ToString(drA["TipoIdent"]);
                        oUsr.Usuario = Convert.ToString(dr["Usuario"]);
                        foreach (DataRow item in ds.Tables[2].Rows)
                        {
                            SegConsDatosUsrAdic_Zonas TmpItem = new SegConsDatosUsrAdic_Zonas();
                            TmpItem.Zona = Convert.ToString(item["Zona"]);
                            TmpItem.DesZona = Convert.ToString(item["DesZona"]);
                            olZonas.Add(TmpItem);
                        }
                        oUsr.Zonas = olZonas;

                    }
                }
            }
            catch (Exception)
            { }

            var jsonData = new
            {
                total = 1,
                page = 1,
                records = oUsr.Zonas.Count(),
                rows = (
                  from Data in oUsr.Zonas.ToList()
                  select new
                  {
                      Identificacion = Data.Zona,
                      cell = new string[] { 
                            Data.Zona.ToString(),
                            Data.DesZona.ToString()
                      }
                  }).ToArray()
            };

            return Ok(jsonData);

        }

    }
}