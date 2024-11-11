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
using Logger;

namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/MantenimientoTransporte")]
    public class MantenimientoTransporteController : ApiController
    {
        //Consulta Carga Incial de Busqueda

        [ActionName("bajararchivo")]
        [HttpGet]
        public string bajararchivo(string identificacion, string archivo)
        {
            String result = null;
            try
            {
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion));
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/DownloadedDocuments/" + identificacion), archivo);
                //httpPostedFile.SaveAs(fileSavePath);
                string lv_Msg = "";
                clsFTP lv_sFtp = new clsFTP();
                lv_sFtp.lv_EsPasivo = false;
                lv_sFtp.lv_IP = AppConfig.SftpServerIp;
                lv_sFtp.lv_Puerto = Convert.ToInt32(AppConfig.SftpServerPort);
                lv_sFtp.lv_Usuario = AppConfig.SftpServerUserName;
                lv_sFtp.lv_Clave = AppConfig.SftpServerPassword;
                string tutafinal =  AppConfig.SftpPath + "Transporte/" + identificacion + "/";
                //directorio = "/srv/ftp/sftp/Sipecom" + AppConfig.SftpPath + directorio + "/";
                //directorio = "/home/sftpuser" + AppConfig.SftpPath + "" + anio + "/Actas/" + codSap + "/" + codAlmacen + "/";
                byte[] retorno = lv_sFtp.ObtenerArchivo_Sftp(tutafinal, archivo, lv_Msg);

                File.WriteAllBytes(fileSavePath, retorno);
                //using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                //{
                //    sftpClient.Connect();
                //    if (!sftpClient.Exists(AppConfig.SftpPath + "Transporte/" + identificacion))
                //    {
                //        sftpClient.CreateDirectory(AppConfig.SftpPath + "Transporte/" + identificacion);
                //    }

                //    Stream fin = File.OpenWrite(fileSavePath);
                //    sftpClient.DownloadFile(AppConfig.SftpPath + "Transporte/" + identificacion + "/" + archivo, fin, null);

                //    fin.Close();
                //    sftpClient.Disconnect();
                //}


                var localFilePath = fileSavePath;

                if (File.Exists(localFilePath))
                {

                    
                    result = "https://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/webapi/DownloadedDocuments/" + identificacion + "/" + archivo;
                    //result = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/DownloadedDocuments/" + identificacion + "/" + archivo;
                }
            }
            catch
            {

            }
            return result;
        }

        [ActionName("consBuscarChoferesUno")]
        [HttpGet]
        public FormResponseTransporte GetconsBuscarChoferesUno(string tipo, string idChofer, string CodSap)
        {
            List<Tra_BandejaGrabar> lst_retornoSolBandeja = new List<Tra_BandejaGrabar>();
            List<Tra_BandejaPedidos> lst_retornoSolPedido = new List<Tra_BandejaPedidos>();
            List<Tra_BandejaAuditoria> lst_retornoAuditoria = new List<Tra_BandejaAuditoria>();
            Tra_BandejaGrabar mod_SolicitudBandeja;
            Tra_BandejaPedidos mod_SolicitudPedido;
            Tra_BandejaAuditoria mod_SolicitudAuditoria;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("IdChofer", idChofer);
                xmlParam.DocumentElement.SetAttribute("CodSAP", (CodSap == null ? "" : CodSap));
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 302, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    if (tipo == "1" || tipo == "2")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolicitudBandeja = new Tra_BandejaGrabar();
                            mod_SolicitudBandeja.txtNombresPrimero = Convert.ToString(item["PRIMERNOMBRE"]);
                            mod_SolicitudBandeja.txtNombresSegundo = Convert.ToString(item["SEGUNDONOMBRE"]);
                            mod_SolicitudBandeja.txtApellidoPrimero = Convert.ToString(item["PRIMERAPELLIDO"]);
                            mod_SolicitudBandeja.txtApellidoSegundo = Convert.ToString(item["SEGUNDOAPELLIDO"]);
                            mod_SolicitudBandeja.txtFechaNacimiento = Convert.ToString(item["FECHANACIMIENTO"]);
                            mod_SolicitudBandeja.CodIdentificacion = Convert.ToString(item["TIPOIDENTIFICACION"]);
                            mod_SolicitudBandeja.txtIdentificacion = Convert.ToString(item["IDENTIFICACION"]);
                            mod_SolicitudBandeja.txtructranspo = Convert.ToString(item["RUCEMPTRANASPORTISTA"]);
                            mod_SolicitudBandeja.txtNombreEmptran = Convert.ToString(item["NOMBREEMPRESATRAN"]);
                            mod_SolicitudBandeja.txtDireccionDomicilio = Convert.ToString(item["DIRECCION"]);
                            mod_SolicitudBandeja.txtTelefonoDomicilio = Convert.ToString(item["TELEFONO"]);
                            mod_SolicitudBandeja.CodEstado = Convert.ToString(item["ESTADO"]);
                            mod_SolicitudBandeja.CodBloqueo = Convert.ToString(item["ESTADOBLOQUEO"]);
                            mod_SolicitudBandeja.txtFechaBloqueo = Convert.ToString(item["FECHABLOQUEO"]);
                            mod_SolicitudBandeja.txtOrigenBloque = Convert.ToString(item["ORIGENBLOQUE"]);
                            mod_SolicitudBandeja.txtNumeroLicencia = Convert.ToString(item["NUMEROLICENCIA"]);
                            mod_SolicitudBandeja.CodCategoria = Convert.ToString(item["CATEGORIA"]);
                            mod_SolicitudBandeja.CodTipoSangre = Convert.ToString(item["TIPOSANGRE"]);
                            mod_SolicitudBandeja.txtFechaEmision = Convert.ToString(item["FECHAEMISION"]);
                            mod_SolicitudBandeja.txtFechaExpiracion = Convert.ToString(item["FECHAEXPEDICION"]);
                            mod_SolicitudBandeja.CodProveedor = Convert.ToString(item["CODPROVEEDOR"]);
                            mod_SolicitudBandeja.txtarchivo = Convert.ToString(item["IMGCHOFER"]);
                            lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                        }


                        FormResponse.root.Add(lst_retornoSolBandeja);

                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            mod_SolicitudPedido = new Tra_BandejaPedidos();
                            mod_SolicitudPedido.IDCITA = Convert.ToInt16(item["IDCITA"]);
                            mod_SolicitudPedido.BODEGA = Convert.ToString(item["BODEGA"]);
                            mod_SolicitudPedido.FECHAENTREGA = Convert.ToString(item["FECHAENTREGA"]);
                            mod_SolicitudPedido.FECHAPEDIDO = Convert.ToString(item["FECHAPEDIDO"]);
                            mod_SolicitudPedido.ESTADO = Convert.ToString(item["ESTADO"]);
                            lst_retornoSolPedido.Add(mod_SolicitudPedido);
                        }

                        FormResponse.root.Add(lst_retornoSolPedido);
                        foreach (DataRow item in ds.Tables[2].Rows)
                        {
                            mod_SolicitudAuditoria = new Tra_BandejaAuditoria();
                            mod_SolicitudAuditoria.fechaingreso = Convert.ToString(item["fechaingreso"]);
                            mod_SolicitudAuditoria.estado = Convert.ToString(item["estado"]);
                            mod_SolicitudAuditoria.motivo = Convert.ToString(item["motivo"]);
                            mod_SolicitudAuditoria.usuario = Convert.ToString(item["usuario"]);
                            mod_SolicitudAuditoria.fechacambio = Convert.ToString(item["fechacambio"]);
                            mod_SolicitudAuditoria.tipo = Convert.ToString(item["tipo"]);
                            lst_retornoAuditoria.Add(mod_SolicitudAuditoria);
                        }
                        FormResponse.root.Add(lst_retornoAuditoria);
                        FormResponse.success = true;
                    }
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;
        }

       

        [ActionName("consBuscarChoferesUnoRUCCED")]
        [HttpGet]
        public FormResponseTransporte GetconsBuscarChoferesUnoRUCCED(string tipo, string rucced)
        {
            List<Tra_BandejaGrabar> lst_retornoSolBandeja = new List<Tra_BandejaGrabar>();
            Tra_BandejaGrabar mod_SolicitudBandeja;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("RUC", rucced);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 302, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    if (tipo == "3" )
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            FormResponse.success = false;
                        }
                        else
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                mod_SolicitudBandeja = new Tra_BandejaGrabar();
                                mod_SolicitudBandeja.txtNombresPrimero = Convert.ToString(item["PRIMERNOMBRE"]);
                                mod_SolicitudBandeja.txtNombresSegundo = Convert.ToString(item["SEGUNDONOMBRE"]);
                                mod_SolicitudBandeja.txtApellidoPrimero = Convert.ToString(item["PRIMERAPELLIDO"]);
                                mod_SolicitudBandeja.txtApellidoSegundo = Convert.ToString(item["SEGUNDOAPELLIDO"]);
                                mod_SolicitudBandeja.txtFechaNacimiento = Convert.ToString(item["FECHANACIMIENTO"]);
                                mod_SolicitudBandeja.CodIdentificacion = Convert.ToString(item["TIPOIDENTIFICACION"]);
                                mod_SolicitudBandeja.txtIdentificacion = Convert.ToString(item["IDENTIFICACION"]);
                                mod_SolicitudBandeja.txtructranspo = Convert.ToString(item["RUCEMPTRANASPORTISTA"]);
                                mod_SolicitudBandeja.txtNombreEmptran = Convert.ToString(item["NOMBREEMPRESATRAN"]);
                                mod_SolicitudBandeja.txtDireccionDomicilio = Convert.ToString(item["DIRECCION"]);
                                mod_SolicitudBandeja.txtTelefonoDomicilio = Convert.ToString(item["TELEFONO"]);
                                mod_SolicitudBandeja.CodEstado = Convert.ToString(item["ESTADO"]);
                                mod_SolicitudBandeja.CodBloqueo = Convert.ToString(item["ESTADOBLOQUEO"]);
                                mod_SolicitudBandeja.txtFechaBloqueo = Convert.ToString(item["FECHABLOQUEO"]);
                                mod_SolicitudBandeja.txtOrigenBloque = Convert.ToString(item["ORIGENBLOQUE"]);
                                mod_SolicitudBandeja.txtNumeroLicencia = Convert.ToString(item["NUMEROLICENCIA"]);
                                mod_SolicitudBandeja.CodCategoria = Convert.ToString(item["CATEGORIA"]);
                                mod_SolicitudBandeja.CodTipoSangre = Convert.ToString(item["TIPOSANGRE"]);
                                mod_SolicitudBandeja.txtFechaEmision = Convert.ToString(item["FECHAEMISION"]);
                                mod_SolicitudBandeja.txtFechaExpiracion = Convert.ToString(item["FECHAEXPEDICION"]);
                                mod_SolicitudBandeja.CodProveedor = Convert.ToString(item["CODPROVEEDOR"]);
                                mod_SolicitudBandeja.txtarchivo = Convert.ToString(item["IMGCHOFER"]);
                                lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                            }


                            FormResponse.root.Add(lst_retornoSolBandeja);
                            FormResponse.success = true;
                        }
                    }
                }
                else
                {
                    FormResponse.success = false;
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;
        }

        [ActionName("consBuscarChoferesAdmi")]
        [HttpGet]
        public FormResponseTransporte GetconsBuscarChoferesAdmi(string tipo, string licencia,
                                            string nombre, string apellido,
                                            string rucproveedor, string CodSap)
        {
            List<Tra_BandejaChoferes> lst_retornoSolBandeja = new List<Tra_BandejaChoferes>();
            Tra_BandejaChoferes mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("LicenciaCedula", (licencia == null ? "" : licencia));
                xmlParam.DocumentElement.SetAttribute("Nombre", (nombre == null ? "" : nombre));
                xmlParam.DocumentElement.SetAttribute("Apellido", (apellido == null ? "" : apellido));
                xmlParam.DocumentElement.SetAttribute("TipoEstado", "");
                xmlParam.DocumentElement.SetAttribute("RucProveedor", (rucproveedor == "undefined" ? "" : rucproveedor));
                xmlParam.DocumentElement.SetAttribute("CodSAP", (CodSap == "undefined" ? "" : CodSap));

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 300, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    if (tipo == "2")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolicitudBandeja = new Tra_BandejaChoferes();
                            mod_SolicitudBandeja.IDCHOFER = Convert.ToInt16(item["IDCHOFER"]);
                            mod_SolicitudBandeja.NOMBRES = Convert.ToString(item["NOMBRES"]);
                            mod_SolicitudBandeja.LICENCIA = Convert.ToString(item["LICENCIA"]);
                            mod_SolicitudBandeja.TELEFONO = Convert.ToString(item["TELEFONO"]);
                            mod_SolicitudBandeja.DIRECCION = Convert.ToString(item["DIRECCION"]);
                            mod_SolicitudBandeja.ESTADO = Convert.ToString(item["ESTADO"]);
                            mod_SolicitudBandeja.CODSAP = Convert.ToString(item["CODSAP"]);
                            lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retornoSolBandeja);
                    }
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;
        }

        [ActionName("consBuscarChoferes")]
        [HttpGet]
        public FormResponseTransporte GetconsBuscarChoferes(string tipo, string licencia,
                                             string nombre, string apellido,
                                             string tipoestado, string CodSap)
        {
            List<Tra_BandejaChoferes> lst_retornoSolBandeja = new List<Tra_BandejaChoferes>();
            Tra_BandejaChoferes mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("LicenciaCedula", (licencia == null ? "" : licencia));
                xmlParam.DocumentElement.SetAttribute("Nombre", (nombre == null ? "" : nombre));
                xmlParam.DocumentElement.SetAttribute("Apellido", (apellido == null ? "" : apellido));
                xmlParam.DocumentElement.SetAttribute("TipoEstado", (tipoestado == "undefined" ? "" : tipoestado));
                xmlParam.DocumentElement.SetAttribute("CodSAP", (CodSap == null ? "" : CodSap));

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 300, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    if (tipo == "1")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolicitudBandeja = new Tra_BandejaChoferes();
                            mod_SolicitudBandeja.IDCHOFER = Convert.ToInt16(item["IDCHOFER"]);
                            mod_SolicitudBandeja.NOMBRES = Convert.ToString(item["NOMBRES"]);
                            mod_SolicitudBandeja.LICENCIA = Convert.ToString(item["LICENCIA"]);
                            mod_SolicitudBandeja.TELEFONO = Convert.ToString(item["TELEFONO"]);
                            mod_SolicitudBandeja.DIRECCION = Convert.ToString(item["DIRECCION"]);
                            mod_SolicitudBandeja.ESTADO = Convert.ToString(item["ESTADO"]);
                            lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retornoSolBandeja);
                    }
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;
        }

        [ActionName("secuenciaDirectorio")]
        [HttpGet]
        public FormResponseNotificacion GetsecuenciaDirectorio(string tipo)
        {
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;
            string secuencia = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {

                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                 new XElement("SecDirectorio",
                                 new XAttribute("desDirectorio", tipo != null ? tipo : ""))));

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 402, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    secuencia = ds.Tables[0].Rows[0]["Secuencia"].ToString();
                    FormResponse.root.Add(secuencia);
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());


            }
            catch
            {

            }
            return FormResponse;
        }


        [ActionName("grabarTransporte")]
        [HttpPost]
        public FormResponseTransporte GetgrabarTransporte(Tra_BandejaGrabar traCho)
        {
            //seg_GrabaUsrAdmin userModel
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            string Mensaje = "";
            XmlDocument xmlParam = new XmlDocument();
            XmlDocument xmlResp = new XmlDocument();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("Tipo", traCho.Tipo);
                xmlParam.DocumentElement.SetAttribute("IdChofer", traCho.idChofer);
                xmlParam.DocumentElement.SetAttribute("Nombre1", traCho.txtNombresPrimero);
                xmlParam.DocumentElement.SetAttribute("Nombre2", traCho.txtNombresSegundo);
                xmlParam.DocumentElement.SetAttribute("Apellido1", traCho.txtApellidoPrimero);
                xmlParam.DocumentElement.SetAttribute("Apellido2", traCho.txtApellidoSegundo);
                xmlParam.DocumentElement.SetAttribute("FechaNacimiento", traCho.txtFechaNacimiento);
                xmlParam.DocumentElement.SetAttribute("TipoIdentificacion", traCho.CodIdentificacion);
                xmlParam.DocumentElement.SetAttribute("Identificacion", traCho.txtIdentificacion);
                xmlParam.DocumentElement.SetAttribute("RucEmpTransportista", traCho.txtructranspo);
                xmlParam.DocumentElement.SetAttribute("NombreTransportista", traCho.txtNombreEmptran);
                xmlParam.DocumentElement.SetAttribute("Direccion", traCho.txtDireccionDomicilio);
                xmlParam.DocumentElement.SetAttribute("Telefono", traCho.txtTelefonoDomicilio);
                xmlParam.DocumentElement.SetAttribute("CodEstado", traCho.CodEstado);
                xmlParam.DocumentElement.SetAttribute("CodBloqueo", traCho.CodBloqueo);
                xmlParam.DocumentElement.SetAttribute("FechaBloqueo", traCho.txtFechaBloqueo);
                xmlParam.DocumentElement.SetAttribute("OrigenBloque", traCho.txtOrigenBloque);
                xmlParam.DocumentElement.SetAttribute("NumeroLicencia", traCho.txtNumeroLicencia);
                xmlParam.DocumentElement.SetAttribute("CodCategoria", traCho.CodCategoria);
                xmlParam.DocumentElement.SetAttribute("FechaEmision", traCho.txtFechaEmision);
                xmlParam.DocumentElement.SetAttribute("FechaExpiracion", traCho.txtFechaExpiracion);
                xmlParam.DocumentElement.SetAttribute("CodTipoSangre", traCho.CodTipoSangre);
                xmlParam.DocumentElement.SetAttribute("txtarchivo", traCho.txtarchivo);
                xmlParam.DocumentElement.SetAttribute("CodProveedor", traCho.CodProveedor);
                xmlParam.DocumentElement.SetAttribute("UsuarioCreacion", traCho.usuarioCreacion);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 301, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        Mensaje = Convert.ToString(item["MENSAJE"]);
                    }
                    if (Mensaje == "EXISTE")
                    {

                        FormResponse.success = false;
                        FormResponse.msgError = Mensaje;
                    }
                    else
                    {
                        if (Mensaje == "ACTUALIZADA")
                        {
                            FormResponse.success = true;
                            FormResponse.msgError = Mensaje;
                            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + traCho.transecuencial), traCho.txtarchivo);
                             try
                            {
                            if (File.Exists(fileSavePath))
                            {
                                    clsFTP lv_sFtp = new clsFTP();
                                    lv_sFtp.lv_EsPasivo = false;
                                    lv_sFtp.lv_IP = AppConfig.SftpServerIp;
                                    lv_sFtp.lv_Puerto = Convert.ToInt32(AppConfig.SftpServerPort);
                                    lv_sFtp.lv_Usuario = AppConfig.SftpServerUserName;
                                    lv_sFtp.lv_Clave = AppConfig.SftpServerPassword;
                                    string lv_Msg = "";
                                    //Creación de Archivo a partir de un arreglo de bytes
                                    byte[] contenido = File.ReadAllBytes(fileSavePath);
                                    lv_sFtp.crearCarpeta(AppConfig.SftpPath + "Transporte/", traCho.txtIdentificacion);

                                    lv_sFtp.CopiarArchivo_Sftp( AppConfig.SftpPath + "Transporte/" + traCho.txtIdentificacion + "/", traCho.txtarchivo, contenido, lv_Msg);


                                    //using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                                    //{
                                    //    sftpClient.Connect();
                                    //    if (!sftpClient.Exists(AppConfig.SftpPath + "Transporte/" + traCho.txtIdentificacion))
                                    //    {
                                    //        sftpClient.CreateDirectory(AppConfig.SftpPath + "Transporte/" + traCho.txtIdentificacion);
                                    //    }
                                    //    Stream fin = File.OpenRead(fileSavePath);
                                    //    sftpClient.UploadFile(fin, AppConfig.SftpPath + "Transporte/" + traCho.txtIdentificacion + "/" + traCho.txtarchivo, true);
                                    //    fin.Close();
                                    //    sftpClient.Disconnect();
                                    //}
                                }
                            }
                             catch (Exception ex)
                             {

                             }
                        }
                        else
                        {
                            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + traCho.transecuencial), traCho.txtarchivo);
                            try
                            {
                                if (File.Exists(fileSavePath))
                                {
                                    clsFTP lv_sFtp = new clsFTP();
                                    lv_sFtp.lv_EsPasivo = false;
                                    lv_sFtp.lv_IP = AppConfig.SftpServerIp;
                                    lv_sFtp.lv_Puerto = Convert.ToInt32(AppConfig.SftpServerPort);
                                    lv_sFtp.lv_Usuario = AppConfig.SftpServerUserName;
                                    lv_sFtp.lv_Clave = AppConfig.SftpServerPassword;
                                    string lv_Msg = "";
                                    //Creación de Archivo a partir de un arreglo de bytes
                                    byte[] contenido = File.ReadAllBytes(fileSavePath);
                                    lv_sFtp.crearCarpeta( AppConfig.SftpPath + "Transporte/", traCho.txtIdentificacion);

                                    lv_sFtp.CopiarArchivo_Sftp(AppConfig.SftpPath + "Transporte/" + traCho.txtIdentificacion + "/", traCho.txtarchivo, contenido, lv_Msg);


                                    //using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                                    //{
                                    //    sftpClient.Connect();
                                    //    if (!sftpClient.Exists(AppConfig.SftpPath + "Transporte/" + traCho.txtIdentificacion))
                                    //    {
                                    //        sftpClient.CreateDirectory(AppConfig.SftpPath + "Transporte/" + traCho.txtIdentificacion);
                                    //    }
                                    //    Stream fin = File.OpenRead(fileSavePath);
                                    //    sftpClient.UploadFile(fin, AppConfig.SftpPath + "Transporte/" + traCho.txtIdentificacion + "/" + traCho.txtarchivo, true);
                                    //    fin.Close();
                                    //    sftpClient.Disconnect();
                                    //}
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            FormResponse.success = true;
                            FormResponse.msgError = Mensaje;


                        }
                    }

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