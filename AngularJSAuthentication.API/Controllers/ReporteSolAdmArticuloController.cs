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
    [RoutePrefix("api/ReporteSolAdmArticulo")]
    public class ReporteSolAdmArticuloController : ApiController
    {

       

        [ActionName("ReporteSolicitudArticulo")]
        [HttpPost]
        public HttpResponseMessage GetReporteSolicitudArticulo(ReporteSolicitudArticulo ReporteSolArticulo)
        {
            HttpResponseMessage result = null;
            string archivo = "";
            try
            {
                ReportDataSource rptDataSourcecab;
                ReportDataSource rptDataSourcedet;
                DataTable ct = new DataTable("Cabecera");
                ct.Columns.Add("numsolicitud", System.Type.GetType("System.String"));
                ct.Columns.Add("fechaSol", System.Type.GetType("System.String"));
                ct.Columns.Add("horaSol", System.Type.GetType("System.String"));
                //ct.Columns.Add("numsolicitud", System.Type.GetType("System.String"));
                ct.Columns.Add("responsable", System.Type.GetType("System.String"));
                ct.Columns.Add("email", System.Type.GetType("System.String"));
                ct.Columns.Add("departamento", System.Type.GetType("System.String"));
                ct.Columns.Add("aprobacion", System.Type.GetType("System.String"));
                ct.Columns.Add("proveedor", System.Type.GetType("System.String"));
                ct.Columns.Add("contacto", System.Type.GetType("System.String"));
                ct.Columns.Add("ruc", System.Type.GetType("System.String"));
                ct.Columns.Add("emailProv", System.Type.GetType("System.String"));
                ct.Columns.Add("codSociedad", System.Type.GetType("System.String"));
                ct.Columns.Add("desCodSociedad", System.Type.GetType("System.String"));
                ct.Columns.Add("tipoMaterial", System.Type.GetType("System.String"));
                ct.Columns.Add("desTipoMaterial", System.Type.GetType("System.String"));
                ct.Columns.Add("codSapProveedor", System.Type.GetType("System.String"));
                ct.Columns.Add("codLegacy", System.Type.GetType("System.String"));
                ct.Columns.Add("catMaterial", System.Type.GetType("System.String"));
                ct.Columns.Add("desCatMaterial", System.Type.GetType("System.String"));
                ct.Columns.Add("tiposolicitud", System.Type.GetType("System.String"));
                
                //Art_ReporteSolArticulo.Art_ReporteSolArticuloCab cb = SolArticulo.p_cabeceraSolArticulo;
                //Tra_ReporteTabularChofer.Tra_ReporteChoferCab cb = Chofer.p_cabeceraChofer;
                DataRow drowc = ct.NewRow();
                drowc["numsolicitud"] = ReporteSolArticulo.p_SolCabecera[0].IdSolicitud;
                drowc["fechaSol"] = !string.IsNullOrEmpty(ReporteSolArticulo.p_SolCabecera[0].FecSolicitud) ? ReporteSolArticulo.p_SolCabecera[0].FecSolicitud.Split(' ')[0] : "";
                drowc["horaSol"] = !string.IsNullOrEmpty(ReporteSolArticulo.p_SolCabecera[0].FecSolicitud) ? ReporteSolArticulo.p_SolCabecera[0].FecSolicitud.Split(' ')[1] : "";
                drowc["responsable"] = ReporteSolArticulo.p_SolCabecera[0].Responsable;
                drowc["email"] = ReporteSolArticulo.p_SolCabecera[0].Correo;
                drowc["departamento"] = ReporteSolArticulo.p_SolCabecera[0].Departamento;
                drowc["aprobacion"] = ReporteSolArticulo.p_SolCabecera[0].Aprobador;
                drowc["proveedor"] = ReporteSolArticulo.p_SolCabecera[0].RazonSocial;
                drowc["contacto"] = ReporteSolArticulo.p_SolCabecera[0].PersonaContacto;
                drowc["ruc"] = ReporteSolArticulo.p_SolCabecera[0].Ruc;
                drowc["emailProv"] = ReporteSolArticulo.p_SolCabecera[0].CorreoProveedor;
                drowc["codSociedad"] = ReporteSolArticulo.p_SolCompras[0].OrganizacionCompras;
                drowc["desCodSociedad"] = !string.IsNullOrEmpty(ReporteSolArticulo.p_SolCompras[0].OrganizacionComprasDes) ? ReporteSolArticulo.p_SolCompras[0].OrganizacionComprasDes.Split('-')[1] : "";
                drowc["tipoMaterial"] = ReporteSolArticulo.p_SolCompras[0].TipoMaterial;
                drowc["desTipoMaterial"] = !string.IsNullOrEmpty(ReporteSolArticulo.p_SolCompras[0].TipoMaterialDes) ? ReporteSolArticulo.p_SolCompras[0].TipoMaterialDes.Split('-')[1] : "";
                drowc["codSapProveedor"] = ReporteSolArticulo.p_SolCabecera[0].CodProveedor;
                drowc["codLegacy"] = ReporteSolArticulo.p_SolCompras[0].CodLegacyProv;
                drowc["catMaterial"] = ReporteSolArticulo.p_SolCompras[0].CategoriaMaterial;
                drowc["desCatMaterial"] = !string.IsNullOrEmpty(ReporteSolArticulo.p_SolCompras[0].CategoriaMaterialDes) ? ReporteSolArticulo.p_SolCompras[0].CategoriaMaterialDes.Split('-')[1] : "";
                if (ReporteSolArticulo.p_SolCabecera[0].TipoSolicitud == "1")
                    drowc["tiposolicitud"] = "SOLICITUD CREACIÓN MATERIALES";
                if (ReporteSolArticulo.p_SolCabecera[0].TipoSolicitud == "2")
                    drowc["tiposolicitud"] = "SOLICITUD MODIFICACIÓN MATERIALES";
                ct.Rows.Add(drowc);

                DataTable dt = new DataTable("Detalle");
                dt.Columns.Add("Id", System.Type.GetType("System.String"));
                dt.Columns.Add("CodSapGenerado", System.Type.GetType("System.String"));
                dt.Columns.Add("Descripcion", System.Type.GetType("System.String"));                
                dt.Columns.Add("Texto", System.Type.GetType("System.String"));
                dt.Columns.Add("GrupoArticulo", System.Type.GetType("System.String"));
                dt.Columns.Add("UMbase", System.Type.GetType("System.String"));
                dt.Columns.Add("UMpedido", System.Type.GetType("System.String"));
                dt.Columns.Add("UxC", System.Type.GetType("System.String"));
                dt.Columns.Add("CodBarra", System.Type.GetType("System.String"));
                dt.Columns.Add("PesoNeto", System.Type.GetType("System.String"));
                dt.Columns.Add("PesoBruto", System.Type.GetType("System.String"));
                dt.Columns.Add("Longitud", System.Type.GetType("System.String"));
                dt.Columns.Add("Ancho", System.Type.GetType("System.String"));
                dt.Columns.Add("Altura", System.Type.GetType("System.String"));
                dt.Columns.Add("Volumen", System.Type.GetType("System.String"));
                dt.Columns.Add("Marca", System.Type.GetType("System.String"));
                dt.Columns.Add("Deducible", System.Type.GetType("System.String"));
                dt.Columns.Add("Iva", System.Type.GetType("System.String"));
                dt.Columns.Add("CatValoracion", System.Type.GetType("System.String"));
                dt.Columns.Add("PaisOrigen", System.Type.GetType("System.String"));
                dt.Columns.Add("GrupoCompras", System.Type.GetType("System.String"));
                dt.Columns.Add("IndPedido", System.Type.GetType("System.String"));
                dt.Columns.Add("Modelo", System.Type.GetType("System.String"));
                dt.Columns.Add("Temporada", System.Type.GetType("System.String"));
                dt.Columns.Add("Coleccion", System.Type.GetType("System.String"));
                dt.Columns.Add("Alcohol", System.Type.GetType("System.String"));
                dt.Columns.Add("Materia", System.Type.GetType("System.String"));
                dt.Columns.Add("Presentacion", System.Type.GetType("System.String"));
                dt.Columns.Add("CantPedir", System.Type.GetType("System.String"));
                dt.Columns.Add("Catalogacion", System.Type.GetType("System.String"));
                dt.Columns.Add("Codigo", System.Type.GetType("System.String"));
                dt.Columns.Add("SurtidoParcial", System.Type.GetType("System.String"));
                dt.Columns.Add("CostoFob", System.Type.GetType("System.String"));
                dt.Columns.Add("Desc1", System.Type.GetType("System.String"));
                dt.Columns.Add("Desc2", System.Type.GetType("System.String"));
                dt.Columns.Add("PerfilDistribucion", System.Type.GetType("System.String"));
                dt.Columns.Add("Almacen", System.Type.GetType("System.String"));
                dt.Columns.Add("TipoAlmacen", System.Type.GetType("System.String"));
                dt.Columns.Add("Almacenamiento", System.Type.GetType("System.String"));
                dt.Columns.Add("Entrada", System.Type.GetType("System.String"));
                dt.Columns.Add("Salida", System.Type.GetType("System.String"));
                dt.Columns.Add("CtdMac", System.Type.GetType("System.String"));
                dt.Columns.Add("UM", System.Type.GetType("System.String"));
                dt.Columns.Add("TUA", System.Type.GetType("System.String"));
                dt.Columns.Add("MatReferencia", System.Type.GetType("System.String"));
                dt.Columns.Add("CenReferencia", System.Type.GetType("System.String"));
                dt.Columns.Add("Afecha", System.Type.GetType("System.String"));
                dt.Columns.Add("multiplicador", System.Type.GetType("System.String"));
                var id = 0;
                foreach (var dr in ReporteSolArticulo.p_SolDetalle)
                {
                    if(dr.Estado == "Aprobado")
                    {
                    id++;
                    DataRow drowd = dt.NewRow();
                    var datosCompras = ReporteSolArticulo.p_SolCompras.Where(x => x.IdDetalle == dr.IdDetalle).FirstOrDefault();
                    var datosMedidad = ReporteSolArticulo.p_SolMedida.Where(x => x.IdDetalle == dr.IdDetalle).ToList();
                    var datosEAN     = ReporteSolArticulo.p_SolCodigoBarra.Where(x => x.IdDetalle == dr.IdDetalle).ToList();
                    var datosCatalogaciones = ReporteSolArticulo.p_SolCatalogacion.Where(x => x.IdDetalle == dr.IdDetalle).ToList();
                    var datosAlmacen = ReporteSolArticulo.p_SolAlmacen.Where(x => x.IdDetalle == dr.IdDetalle).ToList();
                    drowd["Id"] = id.ToString();
                    drowd["CodSapGenerado"] = dr.CodSAPart.ToString();
                    drowd["Descripcion"] = dr.Descripcion.ToString();
                    drowd["Texto"] = dr.OtroId.ToString();
                    drowd["GrupoArticulo"] = !string.IsNullOrEmpty(datosCompras.GrupoArticulo) ? datosCompras.GrupoArticulo.ToString() : "";
                    //Obtener unidad de medida base
                    var MedBase = "";
                    foreach(var reg in datosMedidad)
                    {
                        if(reg.uniMedBase)
                        {
                            MedBase = reg.UnidadMedida + "-" + reg.DesUnidadMedida;
                            break;
                        }
                    }
                    drowd["UMbase"] = MedBase.ToString();
                    var MedPedido = "";
                    foreach (var reg in datosMedidad)
                    {
                        if (reg.uniMedPedido)
                        {
                            MedPedido = reg.UnidadMedida + "-" + reg.DesUnidadMedida;
                            break;
                        }
                    }
                    drowd["UMpedido"] = MedPedido;
                    //Factor conversion
                    var uxc = "";
                    foreach (var reg in datosMedidad)
                    {
                        if (reg.UnidadMedida == "CS")
                        {
                            uxc = reg.FactorCon;
                        }
                    }

                    drowd["UxC"] = uxc.ToString();
                    //Concatenar Codigos de barra
                    var codBarras = "";
                    foreach (var reg in datosEAN)
                    {
                        codBarras = codBarras + "/" + reg.UnidadMedida + "-" + reg.NumeroEan;
                    }
                    if (codBarras != "")
                    {
                        codBarras = codBarras.Substring(1);
                    }
                    drowd["CodBarra"] = codBarras.ToString();

                    //Concatenar PesoNeto
                    var pesoNeto = "";
                    foreach (var reg in datosMedidad)
                    {
                        pesoNeto = pesoNeto + "/" + reg.UnidadMedida + "-" + reg.PesoNeto;
                    }
                    if (pesoNeto != "")
                    {
                        pesoNeto = pesoNeto.Substring(1);
                        
                    }
                    drowd["PesoNeto"] = pesoNeto.ToString();
                    //Concatenar PesoBruto
                    var pesoBruto = "";
                    foreach (var reg in datosMedidad)
                    {
                        pesoBruto = pesoBruto + "/" + reg.UnidadMedida + "-" + reg.PesoBruto;
                    }
                    if (pesoBruto != "")
                    {
                        pesoBruto = pesoBruto.Substring(1);

                    }
                    drowd["PesoBruto"] = pesoBruto.ToString();
                    //Concatenar Longitud
                    var longitud = "";
                    foreach (var reg in datosMedidad)
                    {
                        longitud = longitud + "/" + reg.UnidadMedida + "-" + reg.Longitud;
                    }
                    if (longitud != "")
                    {
                        longitud = longitud.Substring(1);

                    }
                    drowd["Longitud"] = longitud.ToString();
                    //Concatenar Ancho
                    var ancho = "";
                    foreach (var reg in datosMedidad)
                    {
                        ancho = ancho + "/" + reg.UnidadMedida + "-" + reg.Ancho;
                    }
                    if (ancho != "")
                    {
                        ancho = ancho.Substring(1);

                    }
                    drowd["Ancho"] = ancho.ToString();
                    //Concatenar Altura
                    var altura = "";
                    foreach (var reg in datosMedidad)
                    {
                        altura = altura + "/" + reg.UnidadMedida + "-" + reg.Altura;
                    }
                    if (altura != "")
                    {
                        altura = altura.Substring(1);

                    }
                    drowd["Altura"] = altura.ToString();
                    //Concatenar Volumen
                    var volumen = "";
                    foreach (var reg in datosMedidad)
                    {
                        volumen = volumen + "/" + reg.UnidadMedida + "-" + reg.Volumen;
                    }
                    if (volumen != "")
                    {
                        volumen = volumen.Substring(1);

                    }
                    drowd["Volumen"] = volumen.ToString();


                    drowd["Marca"] = dr.DesMarca.ToString();
                    drowd["Deducible"] = dr.Deducible.ToString();
                    drowd["Iva"] = dr.Iva.ToString();
                    drowd["CatValoracion"] = !string.IsNullOrEmpty(datosCompras.CategoriaValoracion) ? datosCompras.CategoriaValoracion.ToString() : "";
                    drowd["PaisOrigen"] = dr.PaisOrigen.ToString();
                    drowd["GrupoCompras"] = !string.IsNullOrEmpty(datosCompras.GrupoCompra) ? datosCompras.GrupoCompra.ToString() : "";
                    drowd["IndPedido"] =  !string.IsNullOrEmpty(datosCompras.IndPedido) ? datosCompras.IndPedido.ToString(): "" ;
                    drowd["Modelo"] = dr.Modelo.ToString();
                    drowd["Temporada"] = !string.IsNullOrEmpty(dr.Temporada)?dr.Temporada.ToString() : "";
                    drowd["Coleccion"] = !string.IsNullOrEmpty(dr.Coleccion) ?dr.Coleccion.ToString() : "" ;
                    drowd["Alcohol"] = !string.IsNullOrEmpty(dr.GradoAlcohol) ? dr.GradoAlcohol.ToString() : "" ;
                    drowd["Materia"] = !string.IsNullOrEmpty(datosCompras.MateriaDes) ? datosCompras.MateriaDes.ToString() : "" ;
                    drowd["Presentacion"] = dr.TamArticulo.ToString();
                    drowd["CantPedir"] = !string.IsNullOrEmpty(dr.CantidadPedir) ? dr.CantidadPedir.ToString() : "";
                    //Concatenar catalogaciones                   
                    var catalogaciones = "";
                    foreach (var reg in datosCatalogaciones)
                    {
                        catalogaciones = catalogaciones + "/"  + reg.DesCatalogacion;
                    }
                    drowd["Catalogacion"] = catalogaciones.ToString();
                    drowd["Codigo"] = dr.CodReferencia.ToString();
                    drowd["SurtidoParcial"] = !string.IsNullOrEmpty(datosCompras.SurtidoParcial) ? datosCompras.SurtidoParcial.ToString() : "";
                    drowd["CostoFob"] = dr.PrecioBruto.ToString();
                    drowd["Desc1"] = dr.Descuento1.ToString();
                    drowd["Desc2"] = dr.Descuento2.ToString();
                    drowd["PerfilDistribucion"] = !string.IsNullOrEmpty(datosCompras.PerfilDistribucion) ? datosCompras.PerfilDistribucion.ToString() : "" ;
                    //Muestra el primer registro
                    var datAlmacen = "";
                    var datTipoAlmacen = "";
                    var datEntrada = "";
                    var datSalida = "";
                    var datAlmacenamiento = "";
                    foreach (var reg in datosAlmacen)
                    {
                        datAlmacen =  "/" + reg.DesAlmacen.ToString();
                        datTipoAlmacen = "/" + reg.DestipAlmacen;
                        datEntrada =  "/" + reg.DesindAlmacenE.ToString();
                        datSalida =  "/" + reg.DesindAlmacenS.ToString();
                        datAlmacenamiento = "/" + reg.DesIndAreaAlmNew.ToString();
                    }

                    drowd["Almacen"] = datAlmacen.ToString();
                    drowd["TipoAlmacen"] = datTipoAlmacen.ToString();
                    drowd["Almacenamiento"] = datAlmacenamiento.ToString();
                    drowd["Entrada"] = datEntrada.ToString();
                    drowd["Salida"] = datSalida.ToString();
                    drowd["CtdMac"] = "";
                    drowd["UM"] = "";
                    drowd["TUA"] = "";
                    drowd["MatReferencia"] = "";
                    drowd["CenReferencia"] = "";
                    drowd["Afecha"] = "";
                    drowd["multiplicador"] = "";

                    dt.Rows.Add(drowd);
                   }
                }

                rptDataSourcecab = new ReportDataSource("RPTSolArticuloCab", ct);
                rptDataSourcedet = new ReportDataSource("RPTSolArticuloDet", dt);
                ReportViewer auxc = new ReportViewer();
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = "";
                string encoding = "";
                string extension = "";
                auxc.ProcessingMode = ProcessingMode.Local;
                auxc.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~/Reporte/ReporteSolMaterial.rdlc");
                auxc.LocalReport.DataSources.Add(rptDataSourcecab);
                auxc.LocalReport.DataSources.Add(rptDataSourcedet);
                byte[] bytes = null;
                //if (cb.tipo == "1")
                //{
                    archivo = "ReporteSolMaterial" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                    bytes = auxc.LocalReport.Render("Excel", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                //}
                //if (cb.tipo == "2")
                //{
                //    archivo = "ReporteSolArticulo" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                //    bytes = auxc.LocalReport.Render("PDF", null, out  mimeType, out  encoding, out  extension, out  streamids, out  warnings);
                //}
                //result = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/webapi/DownloadedDocuments/" + archivo;
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(bytes));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = archivo;

                //archivo = HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + archivo);
                //FileStream fs = System.IO.File.Create(archivo);
                //fs.Write(bytes, 0, bytes.Length);
                //fs.Close();
                auxc.Dispose();


            }
            catch (Exception e)
            {  return result; }
            return result;

        }

    }
}
