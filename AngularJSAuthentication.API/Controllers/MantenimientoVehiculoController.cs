using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Models;

using System.Data;
using System.Xml;
using System.Security.Claims;
using clibProveedores.Models;
using clibProveedores;
using clibSeguridadCR;
using Renci.SshNet;
using System.IO;
using Logger;

namespace AngularJSAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/MantenimientoVehiculo")]
    public class MantenimientoVehiculoController : ApiController
    {
        [ActionName("consBuscarVehiculoUno")]
        [HttpGet]
        public FormResponseTransporte GetconsBuscarVehiculoUno(string tipo, string idVehiculo, string CodSap)
        {
            List<Tra_VehiculoGrabar> lst_retornoSolBandeja = new List<Tra_VehiculoGrabar>();
            List<Tra_BandejaPedidos> lst_retornoSolPedido = new List<Tra_BandejaPedidos>();
            List<Tra_BandejaAuditoria> lst_retornoAuditoria = new List<Tra_BandejaAuditoria>(); 
            Tra_VehiculoGrabar mod_SolicitudBandeja;
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
                xmlParam.DocumentElement.SetAttribute("IdVehiculo", idVehiculo);
                xmlParam.DocumentElement.SetAttribute("CodSAP", (CodSap == null ? "" : CodSap));

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 305, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    if (tipo == "1")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolicitudBandeja = new Tra_VehiculoGrabar();
                            mod_SolicitudBandeja.codIdentificacion = Convert.ToString(item["TIPOIDENTIFICACION"]);
                            mod_SolicitudBandeja.txtIdentificacion = Convert.ToString(item["IDENPROPIETARIO"]);
                            mod_SolicitudBandeja.txtNombresPrimero = Convert.ToString(item["PRIMERNOMBRE"]);
                            mod_SolicitudBandeja.txtNombresSegundo = Convert.ToString(item["SEGUNDONOMBRE"]);
                            mod_SolicitudBandeja.txtApellidoPrimero = Convert.ToString(item["PRIMERAPELLIDO"]);
                            mod_SolicitudBandeja.txtApellidoSegundo = Convert.ToString(item["SEGUNDOAPELLIDO"]);
                            mod_SolicitudBandeja.txtdirpropie = Convert.ToString(item["DIRPROPIETARIO"]);
                            mod_SolicitudBandeja.txttelfpro = Convert.ToString(item["TELEFONOPROPIETARIO"]);
                            mod_SolicitudBandeja.codTipoVehiculo = Convert.ToString(item["TIPOVEHICULO"]);
                            mod_SolicitudBandeja.codColorPrincipal = Convert.ToString(item["COLORPRIMARIO"]);
                            mod_SolicitudBandeja.codColorSecundario = Convert.ToString(item["COLORSECUNDARIO"]);
                            mod_SolicitudBandeja.codMarca = Convert.ToString(item["MARCAVEHICULO"]);
                            mod_SolicitudBandeja.codModelo = Convert.ToString(item["MODELOVEHICULO"]);
                            mod_SolicitudBandeja.txtmotor = Convert.ToString(item["MOTORVEHICULO"]);
                            mod_SolicitudBandeja.txtplaca = Convert.ToString(item["PLACAVEHICULO"]);
                            mod_SolicitudBandeja.txtchasis = Convert.ToString(item["CHASISVEHICULO"]);
                            mod_SolicitudBandeja.txtmatricula = Convert.ToString(item["MATRICULAVEHICULO"]);
                            mod_SolicitudBandeja.txtfechamatricula = Convert.ToString(item["FECHAMATRICULAVEHICULO"]);
                            mod_SolicitudBandeja.txtexpiracionmatricula = Convert.ToString(item["FECHAEXPMATRICULAVEHICULO"]);
                            mod_SolicitudBandeja.txtaniomatricula = Convert.ToString(item["ANIOMATRICULA"]);
                            mod_SolicitudBandeja.codPais = Convert.ToString(item["PAISMATRICULA"]);
                            mod_SolicitudBandeja.txtcilindraje = Convert.ToString(item["CILINDRAJEVEHICULO"]);
                            mod_SolicitudBandeja.txttonelaje = Convert.ToString(item["TONELAJEVEHICULO"]);
                            mod_SolicitudBandeja.PorSota = Convert.ToString(item["SOATVEHICULO"]);
                            mod_SolicitudBandeja.txtemisionsoat = Convert.ToString(item["FECHASOAT"]);
                            mod_SolicitudBandeja.txtexpiracionsoat = Convert.ToString(item["FECHAEXPSOAT"]);
                            mod_SolicitudBandeja.codEstado = Convert.ToString(item["ESTADO"]);
                            mod_SolicitudBandeja.codBloqueo = Convert.ToString(item["ESTADOBLOQUEO"]);
                            mod_SolicitudBandeja.txtFechaBloqueo = Convert.ToString(item["FECHABLOQUEO"]);
                            mod_SolicitudBandeja.txtOrigenBloque = Convert.ToString(item["ORIGENBLOQUE"]);
                            mod_SolicitudBandeja.CodProveedor = Convert.ToString(item["CODPROVEEDOR"]);
                            mod_SolicitudBandeja.txtarchivo = Convert.ToString(item["IMAGEN"]);
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


        [ActionName("consBuscarVehiculoUnoRUCCED")]
        [HttpGet]
        public FormResponseTransporte GetconsBuscarVehiculoUnoRUCCED(string tipov, string placav)
        {
            List<Tra_VehiculoGrabar> lst_retornoSolBandeja = new List<Tra_VehiculoGrabar>();
            Tra_VehiculoGrabar mod_SolicitudBandeja;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipov);
                xmlParam.DocumentElement.SetAttribute("placa", placav);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 305, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    if (tipov == "3")
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            FormResponse.success = false;
                        }
                        else
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                mod_SolicitudBandeja = new Tra_VehiculoGrabar();
                                mod_SolicitudBandeja.codIdentificacion = Convert.ToString(item["TIPOIDENTIFICACION"]);
                                mod_SolicitudBandeja.txtIdentificacion = Convert.ToString(item["IDENPROPIETARIO"]);
                                mod_SolicitudBandeja.txtNombresPrimero = Convert.ToString(item["PRIMERNOMBRE"]);
                                mod_SolicitudBandeja.txtNombresSegundo = Convert.ToString(item["SEGUNDONOMBRE"]);
                                mod_SolicitudBandeja.txtApellidoPrimero = Convert.ToString(item["PRIMERAPELLIDO"]);
                                mod_SolicitudBandeja.txtApellidoSegundo = Convert.ToString(item["SEGUNDOAPELLIDO"]);
                                mod_SolicitudBandeja.txtdirpropie = Convert.ToString(item["DIRPROPIETARIO"]);
                                mod_SolicitudBandeja.txttelfpro = Convert.ToString(item["TELEFONOPROPIETARIO"]);
                                mod_SolicitudBandeja.codTipoVehiculo = Convert.ToString(item["TIPOVEHICULO"]);
                                mod_SolicitudBandeja.codColorPrincipal = Convert.ToString(item["COLORPRIMARIO"]);
                                mod_SolicitudBandeja.codColorSecundario = Convert.ToString(item["COLORSECUNDARIO"]);
                                mod_SolicitudBandeja.codMarca = Convert.ToString(item["MARCAVEHICULO"]);
                                mod_SolicitudBandeja.codModelo = Convert.ToString(item["MODELOVEHICULO"]);
                                mod_SolicitudBandeja.txtmotor = Convert.ToString(item["MOTORVEHICULO"]);
                                mod_SolicitudBandeja.txtplaca = Convert.ToString(item["PLACAVEHICULO"]);
                                mod_SolicitudBandeja.txtchasis = Convert.ToString(item["CHASISVEHICULO"]);
                                mod_SolicitudBandeja.txtmatricula = Convert.ToString(item["MATRICULAVEHICULO"]);
                                mod_SolicitudBandeja.txtfechamatricula = Convert.ToString(item["FECHAMATRICULAVEHICULO"]);
                                mod_SolicitudBandeja.txtexpiracionmatricula = Convert.ToString(item["FECHAEXPMATRICULAVEHICULO"]);
                                mod_SolicitudBandeja.txtaniomatricula = Convert.ToString(item["ANIOMATRICULA"]);
                                mod_SolicitudBandeja.codPais = Convert.ToString(item["PAISMATRICULA"]);
                                mod_SolicitudBandeja.txtcilindraje = Convert.ToString(item["CILINDRAJEVEHICULO"]);
                                mod_SolicitudBandeja.txttonelaje = Convert.ToString(item["TONELAJEVEHICULO"]);
                                mod_SolicitudBandeja.PorSota = Convert.ToString(item["SOATVEHICULO"]);
                                mod_SolicitudBandeja.txtemisionsoat = Convert.ToString(item["FECHASOAT"]);
                                mod_SolicitudBandeja.txtexpiracionsoat = Convert.ToString(item["FECHAEXPSOAT"]);
                                mod_SolicitudBandeja.codEstado = Convert.ToString(item["ESTADO"]);
                                mod_SolicitudBandeja.codBloqueo = Convert.ToString(item["ESTADOBLOQUEO"]);
                                mod_SolicitudBandeja.txtFechaBloqueo = Convert.ToString(item["FECHABLOQUEO"]);
                                mod_SolicitudBandeja.txtOrigenBloque = Convert.ToString(item["ORIGENBLOQUE"]);
                                mod_SolicitudBandeja.CodProveedor = Convert.ToString(item["CODPROVEEDOR"]);
                                mod_SolicitudBandeja.txtarchivo = Convert.ToString(item["IMAGEN"]);
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

        [ActionName("consBuscarVehiculoAdmi")]
        [HttpGet]
        public FormResponseTransporte GetconsBuscarVehiculoAdmi(string tipo, string tipovehiculo,
                                             string propietario, string numplaca,
                                             string tipoestado,string rucproveedor, string CodSap)
        {
            List<Tra_BandejaVehiculo> lst_retornoSolBandeja = new List<Tra_BandejaVehiculo>();
            List<Tra_BandejaPedidos> lst_retornoSolPedido = new List<Tra_BandejaPedidos>();
            Tra_BandejaVehiculo mod_SolicitudBandeja;
            Tra_BandejaPedidos mod_SolicitudPedido;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("TipoVehiculo", (tipovehiculo == null ? "" : tipovehiculo));
                xmlParam.DocumentElement.SetAttribute("Propietario", (propietario == null ? "" : propietario));
                xmlParam.DocumentElement.SetAttribute("NumPlaca", (numplaca == null ? "" : numplaca));
                xmlParam.DocumentElement.SetAttribute("TipoEstado", (tipoestado == "undefined" ? "" : tipoestado));
                xmlParam.DocumentElement.SetAttribute("RucProveedor", (rucproveedor == "undefined" ? "" : rucproveedor));
                xmlParam.DocumentElement.SetAttribute("CodSAP", (CodSap == "undefined" ? "" : CodSap));

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 303, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    if (tipo == "2")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolicitudBandeja = new Tra_BandejaVehiculo();

                            mod_SolicitudBandeja.IDVEHICULO = Convert.ToInt16(item["IDVEHICULO"]);
                            mod_SolicitudBandeja.NUMPLACA = Convert.ToString(item["NUMPLACA"]);
                            mod_SolicitudBandeja.TIPOVEHICULO = Convert.ToString(item["TIPOVEHICULO"]);
                            mod_SolicitudBandeja.MARCA = Convert.ToString(item["MARCA"]);
                            mod_SolicitudBandeja.MODELO = Convert.ToString(item["MODELO"]);
                            mod_SolicitudBandeja.ESTADO = Convert.ToString(item["ESTADO"]);
                            mod_SolicitudBandeja.COLORPRINCIPAL = Convert.ToString(item["COLORPRINCIPAL"]);
                            mod_SolicitudBandeja.COLORSECUNDARIO = Convert.ToString(item["COLORSECUNDARIO"]);
                            mod_SolicitudBandeja.CODSAP = Convert.ToString(item["CODSAP"]);
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
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retornoSolPedido);
                    }
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;
        }

        [ActionName("consBuscarVehiculo")]
        [HttpGet]
        public FormResponseTransporte GetconsBuscarVehiculo(string tipo, string tipovehiculo,
                                             string propietario, string numplaca,
                                             string tipoestado, string CodSap)
        {
            List<Tra_BandejaVehiculo> lst_retornoSolBandeja = new List<Tra_BandejaVehiculo>();
            Tra_BandejaVehiculo mod_SolicitudBandeja;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseTransporte FormResponse = new FormResponseTransporte();
            xmlParam.LoadXml("<Root />");
            try
            {

                xmlParam.DocumentElement.SetAttribute("Tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("TipoVehiculo", (tipovehiculo == "undefined" ? "" : tipovehiculo));
                xmlParam.DocumentElement.SetAttribute("Propietario", (propietario == "undefined" ? "" : propietario));
                xmlParam.DocumentElement.SetAttribute("NumPlaca", (numplaca == "undefined" ? "" : numplaca));
                xmlParam.DocumentElement.SetAttribute("TipoEstado", (tipoestado == "undefined" ? "" : tipoestado));
                xmlParam.DocumentElement.SetAttribute("CodSAP", (CodSap == null ? "" : CodSap));

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 303, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    if (tipo == "1")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolicitudBandeja = new Tra_BandejaVehiculo();
                            mod_SolicitudBandeja.IDVEHICULO = Convert.ToInt16(item["IDVEHICULO"]);
                            mod_SolicitudBandeja.NUMPLACA = Convert.ToString(item["NUMPLACA"]);
                            mod_SolicitudBandeja.TIPOVEHICULO = Convert.ToString(item["TIPOVEHICULO"]);
                            mod_SolicitudBandeja.MARCA = Convert.ToString(item["MARCA"]);
                            mod_SolicitudBandeja.MODELO = Convert.ToString(item["MODELO"]);
                            mod_SolicitudBandeja.ESTADO = Convert.ToString(item["ESTADO"]);
                            mod_SolicitudBandeja.COLORPRINCIPAL = Convert.ToString(item["COLORPRINCIPAL"]);
                            mod_SolicitudBandeja.COLORSECUNDARIO = Convert.ToString(item["COLORSECUNDARIO"]);
                            mod_SolicitudBandeja.CODSAP = Convert.ToString(item["CODIGOSAP"]);
                            lst_retornoSolBandeja.Add(mod_SolicitudBandeja);
                        }
                        FormResponse.root.Add(lst_retornoSolBandeja);
                        FormResponse.success = true;
                      
                    }
                }
            }
            catch (Exception ex)
            { }
            return FormResponse;
        }

        [ActionName("bajararchivoVehiculo")]
        [HttpGet]
        public string bajararchivoVehiculo(string identificacion, string archivo)
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
                string tutafinal = AppConfig.SftpPath + "Vehiculo/" + identificacion + "/";
                //directorio = "/srv/ftp/sftp/Sipecom" + AppConfig.SftpPath + directorio + "/";
                //directorio = "/home/sftpuser" + AppConfig.SftpPath + "" + anio + "/Actas/" + codSap + "/" + codAlmacen + "/";
                byte[] retorno = lv_sFtp.ObtenerArchivo_Sftp(tutafinal, archivo, lv_Msg);

                File.WriteAllBytes(fileSavePath, retorno);
                //using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                //{
                //    sftpClient.Connect();
                //    if (!sftpClient.Exists(AppConfig.SftpPath + "Vehiculo/" + identificacion))
                //    {
                //        sftpClient.CreateDirectory(AppConfig.SftpPath + "Vehiculo/" + identificacion);
                //    }

                //    Stream fin = File.OpenWrite(fileSavePath);
                //    try
                //    {
                //        sftpClient.DownloadFile(AppConfig.SftpPath + "Vehiculo/" + identificacion + "/" + archivo, fin, null);
                //    }
                //    catch (Exception e)
                //    {

                //    }


                //    fin.Close();
                //    sftpClient.Disconnect();
                //}


                var localFilePath = fileSavePath;

                if (File.Exists(localFilePath))
                {
                    //result = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/DownloadedDocuments/" + identificacion + "/" + archivo;
                    result = "https://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/webapi/DownloadedDocuments/" + identificacion + "/" + archivo;
                }
            }
            catch
            {

            }
            

            return result;
        }

        [ActionName("grabarVehiculo")]
        [HttpPost]
        public FormResponseTransporte GetgrabarVehiculo(Tra_VehiculoGrabar traCho)
        {
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
                xmlParam.DocumentElement.SetAttribute("IdVehiculo", traCho.idvehiculo);
                xmlParam.DocumentElement.SetAttribute("Nombre1", traCho.txtNombresPrimero);
                xmlParam.DocumentElement.SetAttribute("Nombre2", traCho.txtNombresSegundo);
                xmlParam.DocumentElement.SetAttribute("Apellido1", traCho.txtApellidoPrimero);
                xmlParam.DocumentElement.SetAttribute("Apellido2", traCho.txtApellidoSegundo);
                xmlParam.DocumentElement.SetAttribute("DireccionPropietario", traCho.txtdirpropie);
                xmlParam.DocumentElement.SetAttribute("TipoIdentificacion", traCho.codIdentificacion);
                xmlParam.DocumentElement.SetAttribute("Identificacion", traCho.txtIdentificacion);
                xmlParam.DocumentElement.SetAttribute("TelefonoPropietario", traCho.txttelfpro);
                xmlParam.DocumentElement.SetAttribute("TipoVehiculo", traCho.codTipoVehiculo);
                xmlParam.DocumentElement.SetAttribute("ColorPrincipal", traCho.codColorPrincipal);
                xmlParam.DocumentElement.SetAttribute("ColorSecundario", traCho.codColorSecundario);
                xmlParam.DocumentElement.SetAttribute("Motor", traCho.txtmotor);
                xmlParam.DocumentElement.SetAttribute("Marca", traCho.codMarca);
                xmlParam.DocumentElement.SetAttribute("Modelo", traCho.codModelo);
                xmlParam.DocumentElement.SetAttribute("Placa", traCho.txtplaca);
                xmlParam.DocumentElement.SetAttribute("Chasis", traCho.txtchasis);
                xmlParam.DocumentElement.SetAttribute("Estado", traCho.codEstado);
                xmlParam.DocumentElement.SetAttribute("Bloque", traCho.codBloqueo);
                xmlParam.DocumentElement.SetAttribute("FechaBloqueo", traCho.txtFechaBloqueo);
                xmlParam.DocumentElement.SetAttribute("OrigenBloqueo", traCho.txtOrigenBloque);
                xmlParam.DocumentElement.SetAttribute("Matricula", traCho.txtmatricula);
                xmlParam.DocumentElement.SetAttribute("FechaMatricula", traCho.txtfechamatricula);
                xmlParam.DocumentElement.SetAttribute("ExpiracionMatricula", traCho.txtexpiracionmatricula);
                xmlParam.DocumentElement.SetAttribute("AnioMatricula", traCho.txtaniomatricula);
                xmlParam.DocumentElement.SetAttribute("Pais", traCho.codPais);
                xmlParam.DocumentElement.SetAttribute("Cilindraje", traCho.txtcilindraje);
                xmlParam.DocumentElement.SetAttribute("Tonelaje", traCho.txttonelaje);
                xmlParam.DocumentElement.SetAttribute("Soat", traCho.PorSota);
                xmlParam.DocumentElement.SetAttribute("EmisionSoat", traCho.txtemisionsoat);
                xmlParam.DocumentElement.SetAttribute("ExpiracionSoat", traCho.txtexpiracionsoat);
                xmlParam.DocumentElement.SetAttribute("txtarchivo", traCho.txtarchivo);
                xmlParam.DocumentElement.SetAttribute("CodProveedor", traCho.CodProveedor);
                xmlParam.DocumentElement.SetAttribute("UsuarioCreacion", traCho.usuarioCreacion);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 304, 1);
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
                            try
                            {
                                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + traCho.transecuencial), traCho.txtarchivo);
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
                                    lv_sFtp.crearCarpeta(AppConfig.SftpPath + "Vehiculo/", traCho.idvehiculo);

                                    lv_sFtp.CopiarArchivo_Sftp( AppConfig.SftpPath + "Vehiculo/" + traCho.idvehiculo + "/", traCho.txtarchivo, contenido, lv_Msg);


                                    //using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                                    //{
                                    //    sftpClient.Connect();
                                    //    if (!sftpClient.Exists(AppConfig.SftpPath + "Vehiculo/" + traCho.idvehiculo))
                                    //    {
                                    //        sftpClient.CreateDirectory(AppConfig.SftpPath + "Vehiculo/" + traCho.idvehiculo);
                                    //    }
                                    //    Stream fin = File.OpenRead(fileSavePath);
                                    //    sftpClient.UploadFile(fin, AppConfig.SftpPath + "Vehiculo/" + traCho.idvehiculo + "/" + traCho.txtarchivo, true);
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
                        else
                        {
                            try
                            {
                                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + traCho.transecuencial), traCho.txtarchivo);
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
                                    lv_sFtp.crearCarpeta( AppConfig.SftpPath + "Vehiculo/", Mensaje);

                                    lv_sFtp.CopiarArchivo_Sftp( AppConfig.SftpPath + "Vehiculo/" + Mensaje + "/", traCho.txtarchivo, contenido, lv_Msg);


                                    //using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                                    //{
                                    //    sftpClient.Connect();
                                    //    if (!sftpClient.Exists(AppConfig.SftpPath + "Vehiculo/" + Mensaje))
                                    //    {
                                    //        sftpClient.CreateDirectory(AppConfig.SftpPath + "Vehiculo/" + Mensaje);
                                    //    }
                                    //    Stream fin = File.OpenRead(fileSavePath);
                                    //    sftpClient.UploadFile(fin, AppConfig.SftpPath + "Vehiculo/" + Mensaje + "/" + traCho.txtarchivo, true);
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
