using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Models;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Xml;
using System.Security.Claims;
using clibProveedores.Models;
using clibProveedores;
using clibSeguridadCR;
using System.Xml.Linq;
using System.IO;
using Renci.SshNet;

using AngularJSAuthentication.API.WCFEnvioCorreo;
using System.Net.Http.Headers;
namespace AngularJSAuthentication.API.Controllers
{
     [RoutePrefix("api/ReporteVehiculo")]
    public class ReporteVehiculoController : ApiController
    {

         [ActionName("ExportarVehiculo")]
         [HttpPost]
         public string ExportarVehiculo(Tra_ReporteTabularVehiculo Vehiculo)
         {
             string result = "";
             string archivo = "";
             try
             {
                 ReportDataSource rptDataSourcecab;
                 ReportDataSource rptDataSourcedet;
                 DataTable ct = new DataTable("Cabecera");
                 ct.Columns.Add("usuario", System.Type.GetType("System.String"));
                 Tra_ReporteTabularVehiculo.Tra_ReporteVehiculoCab cb = Vehiculo.p_cabeceraVehiculo;
                 DataRow drowc = ct.NewRow();
                 drowc["usuario"] = cb.usuario;
                 ct.Rows.Add(drowc);


                 DataTable dt = new DataTable("Detalle");
                 dt.Columns.Add("codproveedor", System.Type.GetType("System.String"));
                 dt.Columns.Add("idvehiculo", System.Type.GetType("System.String"));
                 dt.Columns.Add("numplaca", System.Type.GetType("System.String"));
                 dt.Columns.Add("tipovehiculo", System.Type.GetType("System.String"));
                 dt.Columns.Add("marca", System.Type.GetType("System.String"));
                 dt.Columns.Add("modelo", System.Type.GetType("System.String"));
                 dt.Columns.Add("estado", System.Type.GetType("System.String"));
                 dt.Columns.Add("colorprincipal", System.Type.GetType("System.String"));
                 dt.Columns.Add("colorsecundario", System.Type.GetType("System.String"));


                 foreach (Tra_ReporteTabularVehiculo.Tra_ReporteVehiculorDet dr in Vehiculo.p_detalleVehiculo)
                 {
                     DataRow drowd = dt.NewRow();
                     drowd["codproveedor"] = dr.codsap.ToString();
                     drowd["idvehiculo"] = dr.idvehiculo.ToString();
                     drowd["numplaca"] = dr.numplaca.ToString();
                     drowd["tipovehiculo"] = dr.tipovehiculo.ToString();
                     drowd["marca"] = dr.marca.ToString();
                     drowd["modelo"] = dr.modelo.ToString();
                     drowd["estado"] = dr.estado.ToString();
                     drowd["colorprincipal"] = dr.colorprincipal.ToString();
                     drowd["colorsecundario"] = dr.colorsecundario.ToString();
                     dt.Rows.Add(drowd);
                 }
                 rptDataSourcecab = new ReportDataSource("RPTListadoVehiculoCab", ct);
                 rptDataSourcedet = new ReportDataSource("RPTListadoVehiculoDet", dt);
                 ReportViewer auxc = new ReportViewer();
                 Warning[] warnings = null;
                 string[] streamids = null;
                 string mimeType = "";
                 string encoding = "";
                 string extension = "";
                 auxc.ProcessingMode = ProcessingMode.Local;
                 auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteVehiculo.rdlc");
                 auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                 auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                 byte[] bytes = null;
                 if (cb.tipo == "1")
                 {
                     archivo = "ReporteListadoVehiculo" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                     bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                 }
                 else
                 {
                     archivo = "ReporteListadoVehiculo" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                     bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                 }
                 result = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/DownloadedDocuments/" + archivo;
                 archivo = HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + archivo);
                 FileStream fs = System.IO.File.Create(archivo);
                 fs.Write(bytes, 0, bytes.Length);
                 fs.Close();
                 auxc.Dispose();
             }
             catch (Exception ex)
             {
                 result = "";
             }
             return result;




         }
    }
}
