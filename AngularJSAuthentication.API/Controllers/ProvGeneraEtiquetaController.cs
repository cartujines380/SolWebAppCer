using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AngularJSAuthentication.API.Models;
using System.Data;

using System.Xml;
using System.Xml.Linq;
using clibProveedores;
using System.Web;
using Renci.SshNet;
using System.IO;
using SAP.Middleware.Connector;
using System.Threading;

namespace AngularJSAuthentication.API.Controllers
{


    //[Authorize]

    [RoutePrefix("api/ProvGeneraEtiqueta")]

    public class ProvGeneraEtiquetaController : ApiController
    {


        [ActionName("ProvGeneraEtiquetaList")]
        [HttpGet]
        public FormResponseEtiqueta getProvGeneraEtiquetaList()
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            FormResponseEtiqueta resultado = new FormResponseEtiqueta();
            List<DMEtiqueta.EtiProveedor> lst_retornoProvEtiq = new List<DMEtiqueta.EtiProveedor>();
            DMEtiqueta.EtiProveedor eti_Proveedor;
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Accion", "C");
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 700, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        eti_Proveedor = new DMEtiqueta.EtiProveedor();
                        eti_Proveedor.CodProveedor = Convert.ToString(item["CodProveedor"]);
                        eti_Proveedor.Ruc = Convert.ToString(item["Ruc"]);
                        eti_Proveedor.NombreComercial = Convert.ToString(item["NomComercial"]) ;
                        eti_Proveedor.GeneraEtiqueta = Convert.ToString(item["GenEtiqueta"]) == "1" ? true : false;
                        lst_retornoProvEtiq.Add(eti_Proveedor);
                    }
                    resultado.success = true;
                    resultado.root.Add(lst_retornoProvEtiq);
                }
                else
                {
                    resultado.success = false;
                    resultado.mensaje = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }

            }
            catch (Exception e)
            {
                resultado.success = false;
                resultado.mensaje = e.Message.ToString();
            }

            return resultado;
        }

        [ActionName("ProvEtiquetaActualiza")]
        [HttpGet]
        public FormResponseEtiqueta getProvEtiquetaActualiza(string codProvEtiq, string generaEtiq, string usrTrxEt)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            FormResponseEtiqueta resultado = new FormResponseEtiqueta();
            List<DMEtiqueta.EtiProveedor> lst_retornoProvEtiq = new List<DMEtiqueta.EtiProveedor>();
           
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Accion", "T");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", codProvEtiq);
                xmlParam.DocumentElement.SetAttribute("GeneraEtiqueta", generaEtiq);
                xmlParam.DocumentElement.SetAttribute("Usr", usrTrxEt);              
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 700, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    resultado.success = true;
                }
                else
                {
                    resultado.success = false;
                    resultado.mensaje = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }

            }
            catch (Exception e)
            {
                resultado.success = false;
                resultado.mensaje = e.Message.ToString();
            }

            return resultado;
        }


        [ActionName("ValidaProveedor")]
        [HttpGet]
        public FormResponseEtiqueta getValidaProveedor(string codProvValid)
        {
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            FormResponseEtiqueta resultado = new FormResponseEtiqueta();
            List<DMEtiqueta.EtiProveedor> lst_retornoProvEtiq = new List<DMEtiqueta.EtiProveedor>();
            resultado.success = false;
            resultado.mensaje = "";
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Accion", "V");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", codProvValid);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 700, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        resultado.success = true;
                    }
                }
                else
                {
                    resultado.success = false;
                    resultado.mensaje = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }

            }
            catch (Exception e)
            {
                resultado.success = false;
                resultado.mensaje = e.Message.ToString();
            }

            return resultado;
        }

    }


}

