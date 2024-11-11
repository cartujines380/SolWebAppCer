using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Models;
using System.IO;
using System.Data;
using System.Xml;
using System.Security.Claims;
using clibProveedores.Models;
using Renci.SshNet;
using SAP.Middleware.Connector;
using System.Xml.Linq;
using AngularJSAuthentication.API.Controllers;

namespace clibProveedores
{

    [RoutePrefix("api/Art_Consulta")]
    public class Art_ConsultaController : ApiController
    {


      
        [ActionName("ConsArticulo")]
        [HttpGet]
        public FormResponseArticulo GetConsArticulo(string tipo, string codigo,
                                               string chkCodRef, string CodRef,
                                               string chkCodSap, string CodSap,
                                               string chkGrupoArt, string GrupoArt,
                                               string chkLinea, string LineaNegocio, string ArtNivel, string CodProveedorCons)
        {
            List<Art_ConsultaArticulo> lst_retornoSol = new List<Art_ConsultaArticulo>();
            Art_ConsultaArticulo mod_Consulta;

            //Para la consulta de un articulo en especifico
            List<DMSolicitudArticulo.SolCabecera> lst_retSol_Cab = new List<DMSolicitudArticulo.SolCabecera>();
            DMSolicitudArticulo.SolCabecera mod_SolCabecera;
            //Detalle
            List<DMSolicitudArticulo.SolDetalle> lst_retSol_Det = new List<DMSolicitudArticulo.SolDetalle>();
            DMSolicitudArticulo.SolDetalle mod_SolDetalle;
            //Medidas
            List<DMSolicitudArticulo.SolMedida> lst_retSol_Med = new List<DMSolicitudArticulo.SolMedida>();
            DMSolicitudArticulo.SolMedida mod_SolMedidas;
            //Codigo Barra
            List<DMSolicitudArticulo.SolCodigoBarra> lst_retSol_CBa = new List<DMSolicitudArticulo.SolCodigoBarra>();
            DMSolicitudArticulo.SolCodigoBarra mod_SolCodBarra;
            //Imagenes
            List<DMSolicitudArticulo.SolImagen> lst_retSol_Ima = new List<DMSolicitudArticulo.SolImagen>();
            DMSolicitudArticulo.SolImagen mod_SolImagenes;
            //Rutas
            List<DMSolicitudArticulo.SolRutas> lst_retSol_Rut = new List<DMSolicitudArticulo.SolRutas>();
            DMSolicitudArticulo.SolRutas mod_SolRutas;
            //Compras
            List<DMSolicitudArticulo.SolCompras> lst_retSol_Com = new List<DMSolicitudArticulo.SolCompras>();
            DMSolicitudArticulo.SolCompras mod_SolCompras;
            //Catalogacion
            List<DMSolicitudArticulo.SolCatalogacion> lst_retSol_Cat = new List<DMSolicitudArticulo.SolCatalogacion>();
            DMSolicitudArticulo.SolCatalogacion mod_SolCatalog;
            //Almacen
            List<DMSolicitudArticulo.SolAlmacen> lst_retSol_Alm = new List<DMSolicitudArticulo.SolAlmacen>();
            DMSolicitudArticulo.SolAlmacen mod_SolAlmacen;
            //IndTipoAlmEnt
            List<DMSolicitudArticulo.SolIndTipoAlmEnt> lst_retSol_Iae = new List<DMSolicitudArticulo.SolIndTipoAlmEnt>();
            DMSolicitudArticulo.SolIndTipoAlmEnt mod_SolIndAlmEnt;
            //IndTipoAlmSal
            List<DMSolicitudArticulo.SolIndTipoAlmSal> lst_retSol_Ias = new List<DMSolicitudArticulo.SolIndTipoAlmSal>();
            DMSolicitudArticulo.SolIndTipoAlmSal mod_SolIndAlmSal;
            //IndAreaAlmacen
            List<DMSolicitudArticulo.SolIndAreaAlmacen> lst_retSol_Ara = new List<DMSolicitudArticulo.SolIndAreaAlmacen>();
            DMSolicitudArticulo.SolIndAreaAlmacen mod_SolAreaAlm;
            //Observaciones
            List<DMSolicitudArticulo.SolObserv> lst_retSol_Obs = new List<DMSolicitudArticulo.SolObserv>();
            DMSolicitudArticulo.SolObserv mod_SolObserv;
            //Centros
            List<DMSolicitudArticulo.SolCentros> lst_retSol_Cen = new List<DMSolicitudArticulo.SolCentros>();
            DMSolicitudArticulo.SolCentros mod_SolCentros;
            //Caracteristicas
            List<DMSolicitudArticulo.SolCaracteristicas> lst_retSol_Carac = new List<DMSolicitudArticulo.SolCaracteristicas>();
            DMSolicitudArticulo.SolCaracteristicas mod_SolCaracteristicas;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseArticulo FormResponse = new FormResponseArticulo();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("IdArticulo", (tipo == "1" ? "0" : codigo));

                xmlParam.DocumentElement.SetAttribute("IdOrganizacion", "");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", CodProveedorCons == null ? "0" : CodProveedorCons);
                if (Convert.ToBoolean(chkCodRef)) xmlParam.DocumentElement.SetAttribute("CodReferencia", CodRef);
                if (Convert.ToBoolean(chkCodSap)) xmlParam.DocumentElement.SetAttribute("CodSAP", CodSap);
                if (Convert.ToBoolean(chkGrupoArt))
                {
                    List<string> TagIds = GrupoArt.Split(',').Select(Convert.ToString).ToList();
                    for (int i = 0; i < TagIds.Count; i++)
                    {
                        XmlElement elem = xmlParam.CreateElement("Grp");
                        elem.SetAttribute("id", TagIds[i].ToString());
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                if (Convert.ToBoolean(chkLinea)) xmlParam.DocumentElement.SetAttribute("LineaNegocio", LineaNegocio);

                var workBapi = AppConfig.WorkBAPI;

                if (workBapi == "S")
                {
                   FormResponse =  ConsultaArtBapi(tipo,  codigo, chkCodRef,  CodRef, chkCodSap,  CodSap, chkGrupoArt,  GrupoArt, chkLinea,  LineaNegocio,
                                   CodProveedorCons);
                   return FormResponse;
                }
                else
                {
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 100, 1); //Articulo.Art_P_Consulta	100
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    if (tipo == "1")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_Consulta = new Art_ConsultaArticulo();
                            mod_Consulta.CodSap = Convert.ToString(item["CodSap"]);

                            mod_Consulta.CodReferencia = Convert.ToString(item["CodReferencia"]);
                            mod_Consulta.Marca = Convert.ToString(item["Marca"]);
                            mod_Consulta.Descripcion = Convert.ToString(item["Descripcion"]);
                            mod_Consulta.PersonaContacto = Convert.ToString(item["PersonaContacto"]);
                            mod_Consulta.Email = Convert.ToString(item["Email"]);
                            mod_Consulta.PaisOrigen = Convert.ToString(item["PaisOrigen"]);
                            mod_Consulta.TipoArticulo = Convert.ToString(item["TipoArticulo"]);
                            mod_Consulta.EsGenerico = Convert.ToBoolean(item["EsGenerico"]);
                            
                            lst_retornoSol.Add(mod_Consulta);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retornoSol);
                    }

                    //Un articulo en especifco
                    if (tipo == "2")
                    {
                        //Cabecera
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolCabecera = new DMSolicitudArticulo.SolCabecera();
                            mod_SolCabecera.CodProveedor = "";
                            mod_SolCabecera.IdSolicitud = "";
                            mod_SolCabecera.TipoSolicitud = "";
                            mod_SolCabecera.LineaNegocio = Convert.ToString(item["LineaNegocio"]);
                            mod_SolCabecera.Accion = "";
                            mod_SolCabecera.Estado = "";
                            mod_SolCabecera.Usuario = "";
                            lst_retSol_Cab.Add(mod_SolCabecera);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Cab);



                        //Detalle
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            mod_SolDetalle = new DMSolicitudArticulo.SolDetalle();
                            mod_SolDetalle.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolDetalle.CodReferencia = Convert.ToString(item["CodReferencia"]);                          
                            mod_SolDetalle.CodProveedor = Convert.ToString(item["CodProveedor"]);
                            mod_SolDetalle.CodSAPart = Convert.ToString(item["CodSapArticulo"]);
                            mod_SolDetalle.Marca = Convert.ToString(item["Marca"]);
                            mod_SolDetalle.DesMarca =  Convert.ToString(item["DesMarca"]);
                            mod_SolDetalle.MarcaNueva = "";
                            mod_SolDetalle.PaisOrigen = Convert.ToString(item["PaisOrigen"]);
                            mod_SolDetalle.RegionOrigen = Convert.ToString(item["RegionOrigen"]);
                            mod_SolDetalle.TamArticulo = Convert.ToString(item["TamArticulo"]);
                            mod_SolDetalle.GradoAlcohol = Convert.ToString(item["GradoAlcohol"]);
                            mod_SolDetalle.Talla = Convert.ToString(item["Talla"]);
                            mod_SolDetalle.Color = Convert.ToString(item["Color"]);
                            mod_SolDetalle.Fragancia = Convert.ToString(item["Fragancia"]);
                            mod_SolDetalle.Tipos = Convert.ToString(item["Tipos"]);
                            mod_SolDetalle.Sabor = Convert.ToString(item["Sabor"]);
                            mod_SolDetalle.Modelo = Convert.ToString(item["Modelo"]);
                            mod_SolDetalle.Descripcion = Convert.ToString(item["Descripcion"]);
                            mod_SolDetalle.OtroId = Convert.ToString(item["OtroId"]);
                            mod_SolDetalle.ContAlcohol = Convert.ToBoolean(item["ContAlcohol"]);
                            mod_SolDetalle.Estado = Convert.ToString(item["Estado"]);
                            mod_SolDetalle.Iva = Convert.ToString(item["Iva"]);
                            mod_SolDetalle.Deducible = Convert.ToString(item["Deducible"]);
                            mod_SolDetalle.Retencion = Convert.ToString(item["Retencion"]);
                            mod_SolDetalle.Accion = Convert.ToString(item["Accion"]);
                            mod_SolDetalle.CodTipoArticulo = Convert.ToString(item["CodTipoArticulo"]);
                            mod_SolDetalle.TipoArticulo = Convert.ToString(item["TipoArticulo"]);
                            mod_SolDetalle.PrecioBruto = Convert.ToString(item["PrecioBruto"]).Replace(',', '.');
                            mod_SolDetalle.PrecioNuevo = Convert.ToString(item["PrecioNuevo"]).Replace(',', '.');
                            mod_SolDetalle.Descuento1 = Convert.ToString(item["Descuento1"]).Replace(',', '.');
                            mod_SolDetalle.Descuento2 = Convert.ToString(item["Descuento2"]).Replace(',', '.');
                            mod_SolDetalle.ImpVerde = Convert.ToBoolean(item["ImpVerde"]);
                            mod_SolDetalle.CantidadPedir = Convert.ToString(item["Cantidad"]);
                            mod_SolDetalle.Temporada = Convert.ToString(item["Temporada"]);
                            mod_SolDetalle.Coleccion = Convert.ToString(item["Coleccion"]);
                            mod_SolDetalle.Estacion = Convert.ToString(item["Estacion"]);
                            mod_SolDetalle.IsGenerico = Convert.ToBoolean(item["Generico"]);
                            mod_SolDetalle.IsVariante = Convert.ToBoolean(item["Variante"]);
                            mod_SolDetalle.CodGenerico = Convert.ToString(item["CodGenerico"]);
                            lst_retSol_Det.Add(mod_SolDetalle);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Det);
                        //Medidas
                        foreach (DataRow item in ds.Tables[2].Rows)
                        {
                            mod_SolMedidas = new DMSolicitudArticulo.SolMedida();
                            mod_SolMedidas.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolMedidas.UnidadMedida = Convert.ToString(item["UnidadMedida"]);
                            mod_SolMedidas.DesUnidadMedida = Convert.ToString(item["DesUnidadMedida"]);
                            mod_SolMedidas.UniMedidaVolumen = Convert.ToString(item["UnidadVolumen"]);
                            mod_SolMedidas.DesUniMedidaVolumen = Convert.ToString(item["DesUniMedidaVolumen"]);

                            mod_SolMedidas.TipoUnidadMedida = Convert.ToString(item["TipoUnidadMedida"]);
                            mod_SolMedidas.DesTipoUnidadMedida = Convert.ToString(item["DesTipoUnidadMedida"]);
                            mod_SolMedidas.UniMedConvers = Convert.ToString(item["UniMedConvers"]);
                            mod_SolMedidas.DesUniMedConvers = Convert.ToString(item["DesUniMedConvers"]);
                            mod_SolMedidas.FactorCon = Convert.ToString(item["FactorCon"]);
                            mod_SolMedidas.PesoNeto = Convert.ToString(item["PesoNeto"]).Replace(',', '.');
                            mod_SolMedidas.PesoBruto = Convert.ToString(item["PesoBruto"]).Replace(',', '.');
                            mod_SolMedidas.Longitud = Convert.ToString(item["Longitud"]).Replace(',', '.');
                            mod_SolMedidas.Ancho = Convert.ToString(item["Ancho"]).Replace(',', '.');
                            mod_SolMedidas.Altura = Convert.ToString(item["Altura"]).Replace(',', '.');
                            mod_SolMedidas.Volumen = Convert.ToString(item["Volumen"]).Replace(',', '.');
                            mod_SolMedidas.PrecioBruto = Convert.ToString(item["PrecioBruto"]).Replace(',', '.');
                            mod_SolMedidas.Descuento1 = Convert.ToString(item["Descuento1"]).Replace(',', '.');
                            mod_SolMedidas.Descuento2 = Convert.ToString(item["Descuento2"]).Replace(',', '.');
                            mod_SolMedidas.ImpVerde = Convert.ToBoolean(item["ImpVerde"]);
                            mod_SolMedidas.Estado = Convert.ToString(item["Estado"]);

                            mod_SolMedidas.uniMedBase = Convert.ToBoolean(item["UnidadMedidaBase"]);
                            mod_SolMedidas.uniMedPedido = Convert.ToBoolean(item["UnidadMedidaPedido"]);
                            mod_SolMedidas.uniMedES = Convert.ToBoolean(item["UnidadMedidaES"]);
                            mod_SolMedidas.uniMedVenta = Convert.ToBoolean(item["UnidadMedidaVenta"]);
                            

                            mod_SolMedidas.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_Med.Add(mod_SolMedidas);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Med);
                        //Codigos de Barra
                        foreach (DataRow item in ds.Tables[3].Rows)
                        {
                            mod_SolCodBarra = new DMSolicitudArticulo.SolCodigoBarra();
                            mod_SolCodBarra.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolCodBarra.UnidadMedida = Convert.ToString(item["UnidadMedida"]);
                            mod_SolCodBarra.NumeroEan = Convert.ToString(item["NumeroEan"]);
                            mod_SolCodBarra.TipoEan = Convert.ToString(item["TipoEan"]);
                            mod_SolCodBarra.DescripcionEan = Convert.ToString(item["DescripcionEan"]);
                            mod_SolCodBarra.Principal = Convert.ToBoolean(item["Principal"]);
                            mod_SolCodBarra.paisEan = Convert.ToString(item["PaisEan"]);
                            mod_SolCodBarra.paisDesEan = Convert.ToString(item["PaisDesEan"]);
                            mod_SolCodBarra.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_CBa.Add(mod_SolCodBarra);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_CBa);
                        String codSol = "";
                        //Imagenes
                        foreach (DataRow item in ds.Tables[4].Rows)
                        {
                            mod_SolImagenes = new DMSolicitudArticulo.SolImagen();
                            mod_SolImagenes.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolImagenes.IdDocAdjunto = Convert.ToString(item["IdDocAdjunto"]);
                            mod_SolImagenes.NomArchivo = Convert.ToString(item["NomArchivo"]);
                            mod_SolImagenes.Path = Convert.ToString(item["Path"]);
                            codSol = mod_SolImagenes.Path.Split('-')[0];
                            mod_SolImagenes.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_Ima.Add(mod_SolImagenes);

                            //Descarga las imagenes del ftp y las crea en una carpeta temporal [idsol-idart]
                            BajaFptArchivo(Convert.ToString(item["Path"]), Convert.ToString(item["NomArchivo"]), "");

                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Ima);
                        //Rutas
                        foreach (DataRow item in ds.Tables[4].Rows) //Recorre el detalle y concatena el id de la solicitud
                        {
                            mod_SolRutas = new DMSolicitudArticulo.SolRutas();
                            mod_SolRutas.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolRutas.Path = Convert.ToString(item["Path"]);
                            lst_retSol_Rut.Add(mod_SolRutas);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Rut);
                        //Compras
                        foreach (DataRow item in ds.Tables[5].Rows)
                        {
                            mod_SolCompras = new DMSolicitudArticulo.SolCompras();
                            mod_SolCompras.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolCompras.OrganizacionCompras = Convert.ToString(item["OrganizacionCompras"]);
                            mod_SolCompras.FrecuenciaEntrega = Convert.ToString(item["FrecuenciaEntrega"]);
                            mod_SolCompras.TipoMaterial = Convert.ToString(item["TipoMaterial"]);
                            mod_SolCompras.CategoriaMaterial = Convert.ToString(item["CategoriaMaterial"]);
                            mod_SolCompras.GrupoArticulo = Convert.ToString(item["GrupoArticulo"]);
                            mod_SolCompras.SeccionArticulo = Convert.ToString(item["SeccionArticulo"]);
                            //mod_SolCompras.Catalogacion = Convert.ToString(item["Catalogacion"]);
                            mod_SolCompras.SurtidoParcial = Convert.ToString(item["SurtidoParcial"]);
                            mod_SolCompras.Materia = Convert.ToString(item["Materia"]);
                            mod_SolCompras.CostoFOB = Convert.ToString(item["CostoFOB"]).Replace(',', '.');
                            
                            mod_SolCompras.IndPedido = Convert.ToString(item["IndPedido"]);
                            mod_SolCompras.PerfilDistribucion = Convert.ToString(item["PerfilDistribucion"]);
                            //mod_SolCompras.Almacen = Convert.ToString(item["Almacen"]);
                            mod_SolCompras.GrupoCompra = Convert.ToString(item["GrupoCompra"]);
                            mod_SolCompras.CategoriaValoracion = Convert.ToString(item["CategoriaValoracion"]);
                            mod_SolCompras.TipoAlamcen = Convert.ToString(item["TipoAlamcen"]);
                            //mod_SolCompras.IndAlmaEntrada = Convert.ToString(item["IndAlmaEntrada"]);
                            //mod_SolCompras.IndAlmaSalida = Convert.ToString(item["IndAlmaSalida"]);
                            //mod_SolCompras.IndAreaAlmacen = Convert.ToString(item["IndAreaAlmacen"]);
                            mod_SolCompras.CondicionAlmacen = Convert.ToString(item["CondicionAlmacen"]);
                            mod_SolCompras.ClListaSurtido = Convert.ToString(item["ClListaSurtido"]);
                            mod_SolCompras.EstatusMaterial = Convert.ToString(item["EstatusMaterial"]);
                            mod_SolCompras.EstatusVenta = Convert.ToString(item["EstatusVenta"]);
                            mod_SolCompras.ValidoDesde = Convert.ToString(item["ValidoDesde"]);
                            mod_SolCompras.GrupoBalanzas = Convert.ToString(item["GrupoBalanzas"]);
                            mod_SolCompras.Observacion = Convert.ToString(item["Observacion"]);
                            //mod_SolCompras.Coleccion = Convert.ToString(item["Coleccion"]);
                            //mod_SolCompras.Temporada = Convert.ToString(item["Temporada"]);
                            //mod_SolCompras.Estacion = Convert.ToString(item["Estacion"]);
                            mod_SolCompras.JerarquiaProd = Convert.ToString(item["JerarquiaProd"]); //Nuevos
                            mod_SolCompras.SusceptBonifEsp = Convert.ToString(item["SusceptBonifEsp"]);
                            mod_SolCompras.ProcedimCatalog = Convert.ToString(item["ProcedimCatalog"]);
                            mod_SolCompras.CaracterPlanNec = Convert.ToString(item["CaracterPlanNec"]);
                            mod_SolCompras.FuenteProvision = Convert.ToString(item["FuenteProvision"]);//Nuevos
                            mod_SolCompras.MotivoRechazo = Convert.ToString(item["MotivoRechazo"]);
                            mod_SolCompras.Estado = Convert.ToString(item["Estado"]);
                            lst_retSol_Com.Add(mod_SolCompras);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Com);

                        //Catalogaciones
                        foreach (DataRow item in ds.Tables[6].Rows)
                        {
                            mod_SolCatalog = new DMSolicitudArticulo.SolCatalogacion();
                            mod_SolCatalog.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolCatalog.Catalogacion = Convert.ToString(item["Catalogacion"]);
                            mod_SolCatalog.DesCatalogacion = Convert.ToString(item["DesCatalogacion"]);
                 
                            mod_SolCatalog.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_Cat.Add(mod_SolCatalog);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Cat);
                        //Almacen
                        foreach (DataRow item in ds.Tables[7].Rows)
                        {
                            mod_SolAlmacen = new DMSolicitudArticulo.SolAlmacen();
                            mod_SolAlmacen.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolAlmacen.Almacen = Convert.ToString(item["Almacen"]);
                            //mod_SolAlmacen.DesAlmacen = Convert.ToString(item["DesAlmacen"]);

                            mod_SolAlmacen.TipAlmacen = Convert.ToString(item["TipoAlmacen"]);
                            //mod_SolAlmacen.DestipAlmacen = Convert.ToString(item["DestipAlmacen"]);
                            mod_SolAlmacen.IndAlmacenE = Convert.ToString(item["IndTipoAlmacenE"]);
                            //mod_SolAlmacen.DesindAlmacenE = Convert.ToString(item["DesindAlmacenE"]);
                            mod_SolAlmacen.IndAlmacenS = Convert.ToString(item["IndTipoAlmacenS"]);
                            //mod_SolAlmacen.DesindAlmacenS = Convert.ToString(item["DesindAlmacenS"]);

                            mod_SolAlmacen.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_Alm.Add(mod_SolAlmacen);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Alm);
                        //IndTipoAlmEnt
                        foreach (DataRow item in ds.Tables[8].Rows)
                        {
                            mod_SolIndAlmEnt = new DMSolicitudArticulo.SolIndTipoAlmEnt();
                            mod_SolIndAlmEnt.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolIndAlmEnt.IndTipoAlmEnt = Convert.ToString(item["IndTipoAlmEnt"]);
                            mod_SolIndAlmEnt.DesIndTipoAlmEnt = Convert.ToString(item["DesIndTipoAlmEnt"]);
                            mod_SolIndAlmEnt.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_Iae.Add(mod_SolIndAlmEnt);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Iae);
                        //IndTipoAlmSal
                        foreach (DataRow item in ds.Tables[9].Rows)
                        {
                            mod_SolIndAlmSal = new DMSolicitudArticulo.SolIndTipoAlmSal();
                            mod_SolIndAlmSal.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolIndAlmSal.IndTipoAlmSal = Convert.ToString(item["IndTipoAlmSal"]);
                            mod_SolIndAlmSal.DesIndTipoAlmSal = Convert.ToString(item["DesIndTipoAlmSal"]);
                            mod_SolIndAlmSal.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_Ias.Add(mod_SolIndAlmSal);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Ias);
                        //IndAreaAlmacen
                        foreach (DataRow item in ds.Tables[10].Rows)
                        {
                            mod_SolAreaAlm = new DMSolicitudArticulo.SolIndAreaAlmacen();
                            mod_SolAreaAlm.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolAreaAlm.IndAreaAlmacen = Convert.ToString(item["IndAreaAlmacen"]);
                            mod_SolAreaAlm.DesIndAreaAlmacen = Convert.ToString(item["DesIndAreaAlmacen"]);
                            mod_SolAreaAlm.GrupoBalanzas = Convert.ToString(item["GrupoBalanzas"]);
                            mod_SolAreaAlm.DesgrupoBalanzas = Convert.ToString(item["DesgrupoBalanzas"]);

                            mod_SolAreaAlm.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_Ara.Add(mod_SolAreaAlm);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Ara);

                        //Centros
                        foreach (DataRow item in ds.Tables[11].Rows)
                        {
                            mod_SolCentros = new DMSolicitudArticulo.SolCentros();
                            mod_SolCentros.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolCentros.Centros = Convert.ToString(item["Centro"]);
                            //mod_SolCentros.DesCentros = Convert.ToString(item["DesCentro"]);
                            mod_SolCentros.PerfilDistribucion = Convert.ToString(item["PerfilDistribucion"]);
                            //mod_SolCentros.DesperfilDistribucion = Convert.ToString(item["DesperfilDistribucion"]);
                            mod_SolCentros.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_Cen.Add(mod_SolCentros);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Cen);

                        //Caracteristicas
                        foreach (DataRow item in ds.Tables[12].Rows)
                        {
                            mod_SolCaracteristicas = new DMSolicitudArticulo.SolCaracteristicas();
                            mod_SolCaracteristicas.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolCaracteristicas.IdValor = Convert.ToString(item["Valor"]);
                            mod_SolCaracteristicas.IdCaract = Convert.ToString(item["IdCaracteristica"]);
                            mod_SolCaracteristicas.IdAgrupacion = Convert.ToString(item["Agrupacion"]);
                            mod_SolCaracteristicas.Accion = "I";
                            lst_retSol_Carac.Add(mod_SolCaracteristicas);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Carac);


                    }
                }

                }




            }
            catch (Exception ex)
            { }
            return FormResponse;
        }


        [ActionName("ConsArticuloG")]
        [HttpGet]
        public FormResponseArticulo GetConsArticuloG(string tipo, string codigo,
                                               string chkCodRef, string CodRef,
                                               string chkCodSap, string CodSap,
                                               string chkGrupoArt, string GrupoArt,
                                               string chkLinea, string LineaNegocio,
                                               string ArtNivel, string CodProveedorCons, string flag)
        {
            List<Art_ConsultaArticulo> lst_retornoSol = new List<Art_ConsultaArticulo>();
            Art_ConsultaArticulo mod_Consulta;

            //Para la consulta de un articulo en especifico
            List<DMSolicitudArticulo.SolCabecera> lst_retSol_Cab = new List<DMSolicitudArticulo.SolCabecera>();
            DMSolicitudArticulo.SolCabecera mod_SolCabecera;
            //Detalle
            List<DMSolicitudArticulo.SolDetalle> lst_retSol_Det = new List<DMSolicitudArticulo.SolDetalle>();
            DMSolicitudArticulo.SolDetalle mod_SolDetalle;
            //Detalle Generico
            List<DMSolicitudArticulo.SolDetalleG> lst_retSol_DetG = new List<DMSolicitudArticulo.SolDetalleG>();
            DMSolicitudArticulo.SolDetalleG mod_SolDetalleG;
            //Medidas
            List<DMSolicitudArticulo.SolMedida> lst_retSol_Med = new List<DMSolicitudArticulo.SolMedida>();
            DMSolicitudArticulo.SolMedida mod_SolMedidas;
            //Codigo Barra
            List<DMSolicitudArticulo.SolCodigoBarra> lst_retSol_CBa = new List<DMSolicitudArticulo.SolCodigoBarra>();
            DMSolicitudArticulo.SolCodigoBarra mod_SolCodBarra;
            //Imagenes
            List<DMSolicitudArticulo.SolImagen> lst_retSol_Ima = new List<DMSolicitudArticulo.SolImagen>();
            DMSolicitudArticulo.SolImagen mod_SolImagenes;
            //Rutas
            List<DMSolicitudArticulo.SolRutas> lst_retSol_Rut = new List<DMSolicitudArticulo.SolRutas>();
            DMSolicitudArticulo.SolRutas mod_SolRutas;
            //Compras
            List<DMSolicitudArticulo.SolCompras> lst_retSol_Com = new List<DMSolicitudArticulo.SolCompras>();
            DMSolicitudArticulo.SolCompras mod_SolCompras;
            //Catalogacion
            List<DMSolicitudArticulo.SolCatalogacion> lst_retSol_Cat = new List<DMSolicitudArticulo.SolCatalogacion>();
            DMSolicitudArticulo.SolCatalogacion mod_SolCatalog;
            //Almacen
            List<DMSolicitudArticulo.SolAlmacen> lst_retSol_Alm = new List<DMSolicitudArticulo.SolAlmacen>();
            DMSolicitudArticulo.SolAlmacen mod_SolAlmacen;
            //IndTipoAlmEnt
            List<DMSolicitudArticulo.SolIndTipoAlmEnt> lst_retSol_Iae = new List<DMSolicitudArticulo.SolIndTipoAlmEnt>();
            DMSolicitudArticulo.SolIndTipoAlmEnt mod_SolIndAlmEnt;
            //IndTipoAlmSal
            List<DMSolicitudArticulo.SolIndTipoAlmSal> lst_retSol_Ias = new List<DMSolicitudArticulo.SolIndTipoAlmSal>();
            DMSolicitudArticulo.SolIndTipoAlmSal mod_SolIndAlmSal;
            //IndAreaAlmacen
            List<DMSolicitudArticulo.SolIndAreaAlmacen> lst_retSol_Ara = new List<DMSolicitudArticulo.SolIndAreaAlmacen>();
            DMSolicitudArticulo.SolIndAreaAlmacen mod_SolAreaAlm;
            //Observaciones
            List<DMSolicitudArticulo.SolObserv> lst_retSol_Obs = new List<DMSolicitudArticulo.SolObserv>();
            DMSolicitudArticulo.SolObserv mod_SolObserv;
            //Centros
            List<DMSolicitudArticulo.SolCentros> lst_retSol_Cen = new List<DMSolicitudArticulo.SolCentros>();
            DMSolicitudArticulo.SolCentros mod_SolCentros;

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseArticulo FormResponse = new FormResponseArticulo();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("IdArticulo", (tipo == "1" ? "0" : codigo));

                xmlParam.DocumentElement.SetAttribute("IdOrganizacion", "");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", "");
                if (Convert.ToBoolean(chkCodRef)) xmlParam.DocumentElement.SetAttribute("CodReferencia", CodRef);
                if (Convert.ToBoolean(chkCodSap)) xmlParam.DocumentElement.SetAttribute("CodSAP", CodSap);
                if (Convert.ToBoolean(chkGrupoArt))
                {
                    List<string> TagIds = GrupoArt.Split('|').Select(Convert.ToString).ToList();
                    for (int i = 0; i < TagIds.Count; i++)
                    {
                        XmlElement elem = xmlParam.CreateElement("Grp");
                        elem.SetAttribute("id", TagIds[i].ToString());
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                if (Convert.ToBoolean(chkLinea)) xmlParam.DocumentElement.SetAttribute("LineaNegocio", LineaNegocio);

                var workBapi = AppConfig.WorkBAPI;

                if (workBapi == "S")
                {
                    FormResponse = ConsultaArtBapi(tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkGrupoArt, GrupoArt, chkLinea, LineaNegocio,
                                    CodProveedorCons);
                    return FormResponse;
                }
                else
                {
                    ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 100, 1); //Articulo.Art_P_Consulta	100
                    if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                    {
                        //Bandeja de Solicitudes
                        if (tipo == "1")
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                mod_Consulta = new Art_ConsultaArticulo();
                                mod_Consulta.CodSap = Convert.ToString(item["CodSap"]);
                                mod_Consulta.CodReferencia = Convert.ToString(item["CodReferencia"]);
                                mod_Consulta.Marca = Convert.ToString(item["Marca"]);
                                mod_Consulta.Descripcion = Convert.ToString(item["Descripcion"]);
                                mod_Consulta.PersonaContacto = Convert.ToString(item["PersonaContacto"]);
                                mod_Consulta.Email = Convert.ToString(item["Email"]);
                                mod_Consulta.PaisOrigen = Convert.ToString(item["PaisOrigen"]);
                                lst_retornoSol.Add(mod_Consulta);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retornoSol);
                        }

                        //Un articulo en especifco
                        if (tipo == "2")
                        {
                            //Cabecera
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                mod_SolCabecera = new DMSolicitudArticulo.SolCabecera();
                                mod_SolCabecera.CodProveedor = "";
                                mod_SolCabecera.IdSolicitud = "";
                                mod_SolCabecera.TipoSolicitud = "";
                                mod_SolCabecera.LineaNegocio = Convert.ToString(item["LineaNegocio"]);
                                mod_SolCabecera.Accion = "";
                                mod_SolCabecera.Estado = "";
                                mod_SolCabecera.Usuario = "";
                                lst_retSol_Cab.Add(mod_SolCabecera);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Cab);

                            //Detalle Genérico
                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                mod_SolDetalleG = new DMSolicitudArticulo.SolDetalleG();
                                mod_SolDetalleG.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolDetalleG.CodReferencia = Convert.ToString(item["CodReferencia"]);
                                mod_SolDetalleG.CodSAPart = Convert.ToString(item["CodSapArticulo"]);
                                mod_SolDetalleG.Marca = Convert.ToString(item["Marca"]);
                                mod_SolDetalleG.DesMarca = Convert.ToString(item["DesMarca"]);
                                mod_SolDetalleG.MarcaNueva = "";
                                mod_SolDetalleG.PaisOrigen = Convert.ToString(item["PaisOrigen"]);
                                mod_SolDetalleG.RegionOrigen = Convert.ToString(item["RegionOrigen"]);
                                mod_SolDetalleG.GradoAlcohol = Convert.ToString(item["GradoAlcohol"]);
                                mod_SolDetalleG.Modelo = Convert.ToString(item["Modelo"]);
                                mod_SolDetalleG.Descripcion = Convert.ToString(item["Descripcion"]);
                                mod_SolDetalleG.OtroId = Convert.ToString(item["OtroId"]);
                                mod_SolDetalleG.ContAlcohol = Convert.ToBoolean(item["ContAlcohol"]);
                                mod_SolDetalleG.Estado = Convert.ToString(item["Estado"]);
                                mod_SolDetalleG.Iva = Convert.ToString(item["Iva"]);
                                mod_SolDetalleG.Deducible = Convert.ToString(item["Deducible"]);
                                mod_SolDetalleG.Retencion = Convert.ToString(item["Retencion"]);
                                mod_SolDetalleG.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_DetG.Add(mod_SolDetalleG);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_DetG);

                            //Detalle
                            foreach (DataRow item in ds.Tables[2].Rows)
                            {
                                mod_SolDetalle = new DMSolicitudArticulo.SolDetalle();
                                mod_SolDetalle.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolDetalle.CodReferencia = Convert.ToString(item["CodReferencia"]);
                                mod_SolDetalle.CodSAPart = Convert.ToString(item["CodSapArticulo"]);
                                mod_SolDetalle.Marca = Convert.ToString(item["Marca"]);
                                mod_SolDetalle.DesMarca = Convert.ToString(item["DesMarca"]);
                                mod_SolDetalle.MarcaNueva = "";
                                mod_SolDetalle.PaisOrigen = Convert.ToString(item["PaisOrigen"]);
                                mod_SolDetalle.RegionOrigen = Convert.ToString(item["RegionOrigen"]);
                                mod_SolDetalle.TamArticulo = Convert.ToString(item["TamArticulo"]);
                                mod_SolDetalle.GradoAlcohol = Convert.ToString(item["GradoAlcohol"]);
                                mod_SolDetalle.Talla = Convert.ToString(item["Talla"]);
                                mod_SolDetalle.Color = Convert.ToString(item["Color"]);
                                mod_SolDetalle.Fragancia = Convert.ToString(item["Fragancia"]);
                                mod_SolDetalle.Tipos = Convert.ToString(item["Tipos"]);
                                mod_SolDetalle.Sabor = Convert.ToString(item["Sabor"]);
                                mod_SolDetalle.Modelo = Convert.ToString(item["Modelo"]);
                                mod_SolDetalle.Descripcion = Convert.ToString(item["Descripcion"]);
                                mod_SolDetalle.OtroId = Convert.ToString(item["OtroId"]);
                                mod_SolDetalle.ContAlcohol = Convert.ToBoolean(item["ContAlcohol"]);
                                mod_SolDetalle.Estado = Convert.ToString(item["Estado"]);
                                mod_SolDetalle.Iva = Convert.ToString(item["Iva"]);
                                mod_SolDetalle.Deducible = Convert.ToString(item["Deducible"]);
                                mod_SolDetalle.Retencion = Convert.ToString(item["Retencion"]);
                                mod_SolDetalle.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_Det.Add(mod_SolDetalle);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Det);
                            //Medidas
                            foreach (DataRow item in ds.Tables[3].Rows)
                            {
                                mod_SolMedidas = new DMSolicitudArticulo.SolMedida();
                                mod_SolMedidas.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolMedidas.UnidadMedida = Convert.ToString(item["UnidadMedida"]);
                                mod_SolMedidas.DesUnidadMedida = Convert.ToString(item["DesUnidadMedida"]);
                                mod_SolMedidas.TipoUnidadMedida = Convert.ToString(item["TipoUnidadMedida"]);
                                mod_SolMedidas.DesTipoUnidadMedida = Convert.ToString(item["DesTipoUnidadMedida"]);
                                mod_SolMedidas.UniMedConvers = Convert.ToString(item["UniMedConvers"]);
                                mod_SolMedidas.DesUniMedConvers = Convert.ToString(item["DesUniMedConvers"]);
                                mod_SolMedidas.FactorCon = Convert.ToString(item["FactorCon"]);
                                mod_SolMedidas.PesoNeto = Convert.ToString(item["PesoNeto"]);
                                mod_SolMedidas.PesoBruto = Convert.ToString(item["PesoBruto"]);
                                mod_SolMedidas.Longitud = Convert.ToString(item["Longitud"]);
                                mod_SolMedidas.Ancho = Convert.ToString(item["Ancho"]);
                                mod_SolMedidas.Altura = Convert.ToString(item["Altura"]);
                                mod_SolMedidas.PrecioBruto = Convert.ToString(item["PrecioBruto"]);
                                mod_SolMedidas.Descuento1 = Convert.ToString(item["Descuento1"]);
                                mod_SolMedidas.Descuento2 = Convert.ToString(item["Descuento2"]);
                                mod_SolMedidas.ImpVerde = Convert.ToBoolean(item["ImpVerde"]);
                                mod_SolMedidas.Estado = Convert.ToString(item["Estado"]);
                                mod_SolMedidas.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_Med.Add(mod_SolMedidas);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Med);
                            //Codigos de Barra
                            foreach (DataRow item in ds.Tables[4].Rows)
                            {
                                mod_SolCodBarra = new DMSolicitudArticulo.SolCodigoBarra();
                                mod_SolCodBarra.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolCodBarra.UnidadMedida = Convert.ToString(item["UnidadMedida"]);
                                mod_SolCodBarra.NumeroEan = Convert.ToString(item["NumeroEan"]);
                                mod_SolCodBarra.TipoEan = Convert.ToString(item["TipoEan"]);
                                mod_SolCodBarra.DescripcionEan = Convert.ToString(item["DescripcionEan"]);
                                mod_SolCodBarra.Principal = Convert.ToBoolean(item["Principal"]);
                                mod_SolCodBarra.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_CBa.Add(mod_SolCodBarra);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_CBa);
                            String codSol = "";
                            //Imagenes
                            foreach (DataRow item in ds.Tables[5].Rows)
                            {
                                mod_SolImagenes = new DMSolicitudArticulo.SolImagen();
                                mod_SolImagenes.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolImagenes.IdDocAdjunto = Convert.ToString(item["IdDocAdjunto"]);
                                mod_SolImagenes.NomArchivo = Convert.ToString(item["NomArchivo"]);
                                mod_SolImagenes.Path = Convert.ToString(item["Path"]);
                                codSol = mod_SolImagenes.Path.Split('-')[0];
                                mod_SolImagenes.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_Ima.Add(mod_SolImagenes);

                                //Descarga las imagenes del ftp y las crea en una carpeta temporal [idsol-idart]
                                BajaFptArchivo(Convert.ToString(item["Path"]), Convert.ToString(item["NomArchivo"]), "");

                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Ima);
                            //Rutas
                            foreach (DataRow item in ds.Tables[5].Rows) //Recorre el detalle y concatena el id de la solicitud
                            {
                                mod_SolRutas = new DMSolicitudArticulo.SolRutas();
                                mod_SolRutas.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolRutas.Path = Convert.ToString(item["Path"]);
                                lst_retSol_Rut.Add(mod_SolRutas);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Rut);
                            //Compras
                            foreach (DataRow item in ds.Tables[6].Rows)
                            {
                                mod_SolCompras = new DMSolicitudArticulo.SolCompras();
                                mod_SolCompras.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolCompras.OrganizacionCompras = Convert.ToString(item["OrganizacionCompras"]);
                                mod_SolCompras.FrecuenciaEntrega = Convert.ToString(item["FrecuenciaEntrega"]);
                                mod_SolCompras.TipoMaterial = Convert.ToString(item["TipoMaterial"]);
                                mod_SolCompras.CategoriaMaterial = Convert.ToString(item["CategoriaMaterial"]);
                                mod_SolCompras.GrupoArticulo = Convert.ToString(item["GrupoArticulo"]);
                                mod_SolCompras.SeccionArticulo = Convert.ToString(item["SeccionArticulo"]);
                                //mod_SolCompras.Catalogacion = Convert.ToString(item["Catalogacion"]);
                                mod_SolCompras.SurtidoParcial = Convert.ToString(item["SurtidoParcial"]);
                                mod_SolCompras.Materia = Convert.ToString(item["Materia"]);
                                mod_SolCompras.CostoFOB = Convert.ToString(item["CostoFOB"]);
                                mod_SolCompras.IndPedido = Convert.ToString(item["IndPedido"]);
                                mod_SolCompras.PerfilDistribucion = Convert.ToString(item["PerfilDistribucion"]);
                                //mod_SolCompras.Almacen = Convert.ToString(item["Almacen"]);
                                mod_SolCompras.GrupoCompra = Convert.ToString(item["GrupoCompra"]);
                                mod_SolCompras.CategoriaValoracion = Convert.ToString(item["CategoriaValoracion"]);
                                mod_SolCompras.TipoAlamcen = Convert.ToString(item["TipoAlamcen"]);
                                //mod_SolCompras.IndAlmaEntrada = Convert.ToString(item["IndAlmaEntrada"]);
                                //mod_SolCompras.IndAlmaSalida = Convert.ToString(item["IndAlmaSalida"]);
                                //mod_SolCompras.IndAreaAlmacen = Convert.ToString(item["IndAreaAlmacen"]);
                                mod_SolCompras.CondicionAlmacen = Convert.ToString(item["CondicionAlmacen"]);
                                mod_SolCompras.ClListaSurtido = Convert.ToString(item["ClListaSurtido"]);
                                mod_SolCompras.EstatusMaterial = Convert.ToString(item["EstatusMaterial"]);
                                mod_SolCompras.EstatusVenta = Convert.ToString(item["EstatusVenta"]);
                                mod_SolCompras.ValidoDesde = Convert.ToString(item["ValidoDesde"]);
                                mod_SolCompras.GrupoBalanzas = Convert.ToString(item["GrupoBalanzas"]);
                                mod_SolCompras.Observacion = Convert.ToString(item["Observacion"]);
                                mod_SolCompras.Coleccion = Convert.ToString(item["Coleccion"]);
                                mod_SolCompras.Temporada = Convert.ToString(item["Temporada"]);
                                mod_SolCompras.Estacion = Convert.ToString(item["Estacion"]);
                                mod_SolCompras.JerarquiaProd = Convert.ToString(item["JerarquiaProd"]); //Nuevos
                                mod_SolCompras.SusceptBonifEsp = Convert.ToString(item["SusceptBonifEsp"]);
                                mod_SolCompras.ProcedimCatalog = Convert.ToString(item["ProcedimCatalog"]);
                                mod_SolCompras.CaracterPlanNec = Convert.ToString(item["CaracterPlanNec"]);
                                mod_SolCompras.FuenteProvision = Convert.ToString(item["FuenteProvision"]);//Nuevos
                                mod_SolCompras.MotivoRechazo = Convert.ToString(item["MotivoRechazo"]);
                                mod_SolCompras.Estado = Convert.ToString(item["Estado"]);
                                lst_retSol_Com.Add(mod_SolCompras);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Com);

                            //Catalogaciones
                            foreach (DataRow item in ds.Tables[7].Rows)
                            {
                                mod_SolCatalog = new DMSolicitudArticulo.SolCatalogacion();
                                mod_SolCatalog.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolCatalog.Catalogacion = Convert.ToString(item["Catalogacion"]);
                                mod_SolCatalog.DesCatalogacion = Convert.ToString(item["DesCatalogacion"]);

                                mod_SolCatalog.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_Cat.Add(mod_SolCatalog);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Cat);
                            //Almacen
                            foreach (DataRow item in ds.Tables[8].Rows)
                            {
                                mod_SolAlmacen = new DMSolicitudArticulo.SolAlmacen();
                                mod_SolAlmacen.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolAlmacen.Almacen = Convert.ToString(item["Almacen"]);
                                mod_SolAlmacen.DesAlmacen = Convert.ToString(item["DesAlmacen"]);

                                mod_SolAlmacen.TipAlmacen = Convert.ToString(item["TipoAlmacen"]);
                                mod_SolAlmacen.DestipAlmacen = Convert.ToString(item["DestipAlmacen"]);
                                mod_SolAlmacen.IndAlmacenE = Convert.ToString(item["IndTipoAlmacenE"]);
                                mod_SolAlmacen.DesindAlmacenE = Convert.ToString(item["DesindAlmacenE"]);
                                mod_SolAlmacen.IndAlmacenS = Convert.ToString(item["IndTipoAlmacenS"]);
                                mod_SolAlmacen.DesindAlmacenS = Convert.ToString(item["DesindAlmacenS"]);

                                mod_SolAlmacen.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_Alm.Add(mod_SolAlmacen);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Alm);
                            //IndTipoAlmEnt
                            foreach (DataRow item in ds.Tables[9].Rows)
                            {
                                mod_SolIndAlmEnt = new DMSolicitudArticulo.SolIndTipoAlmEnt();
                                mod_SolIndAlmEnt.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolIndAlmEnt.IndTipoAlmEnt = Convert.ToString(item["IndTipoAlmEnt"]);
                                mod_SolIndAlmEnt.DesIndTipoAlmEnt = Convert.ToString(item["DesIndTipoAlmEnt"]);
                                mod_SolIndAlmEnt.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_Iae.Add(mod_SolIndAlmEnt);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Iae);
                            //IndTipoAlmSal
                            foreach (DataRow item in ds.Tables[10].Rows)
                            {
                                mod_SolIndAlmSal = new DMSolicitudArticulo.SolIndTipoAlmSal();
                                mod_SolIndAlmSal.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolIndAlmSal.IndTipoAlmSal = Convert.ToString(item["IndTipoAlmSal"]);
                                mod_SolIndAlmSal.DesIndTipoAlmSal = Convert.ToString(item["DesIndTipoAlmSal"]);
                                mod_SolIndAlmSal.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_Ias.Add(mod_SolIndAlmSal);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Ias);
                            //IndAreaAlmacen
                            foreach (DataRow item in ds.Tables[11].Rows)
                            {
                                mod_SolAreaAlm = new DMSolicitudArticulo.SolIndAreaAlmacen();
                                mod_SolAreaAlm.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolAreaAlm.IndAreaAlmacen = Convert.ToString(item["IndAreaAlmacen"]);
                                mod_SolAreaAlm.DesIndAreaAlmacen = Convert.ToString(item["DesIndAreaAlmacen"]);
                                mod_SolAreaAlm.GrupoBalanzas = Convert.ToString(item["GrupoBalanzas"]);
                                mod_SolAreaAlm.DesgrupoBalanzas = Convert.ToString(item["DesgrupoBalanzas"]);

                                mod_SolAreaAlm.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_Ara.Add(mod_SolAreaAlm);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Ara);

                            //Centros
                            foreach (DataRow item in ds.Tables[12].Rows)
                            {
                                mod_SolCentros = new DMSolicitudArticulo.SolCentros();
                                mod_SolCentros.IdDetalle = Convert.ToString(item["IdDetalle"]);
                                mod_SolCentros.Centros = Convert.ToString(item["Centro"]);
                                mod_SolCentros.DesCentros = Convert.ToString(item["DesCentro"]);
                                mod_SolCentros.PerfilDistribucion = Convert.ToString(item["PerfilDistribucion"]);
                                mod_SolCentros.DesperfilDistribucion = Convert.ToString(item["DesperfilDistribucion"]);
                                mod_SolCentros.Accion = Convert.ToString(item["Accion"]);
                                lst_retSol_Cen.Add(mod_SolCentros);
                            }
                            FormResponse.success = true;
                            FormResponse.root.Add(lst_retSol_Cen);

                        }
                    }

                }




            }
            catch (Exception ex)
            { }
            return FormResponse;
        }
        

        private void BajaFptArchivo(string path_comp, string nom_archivo, string aux)
        {
            try
            {
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path_comp));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path_comp));
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path_comp), nom_archivo);

                using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                {
                    sftpClient.Connect();
                    if (!sftpClient.Exists(AppConfig.SftpPath + "Articulo/" + path_comp))
                    {
                        sftpClient.CreateDirectory(AppConfig.SftpPath + "Articulo/" + path_comp);
                    }

                    Stream fin = File.OpenWrite(fileSavePath);
                    sftpClient.DownloadFile(AppConfig.SftpPath + "Articulo/" + path_comp + "/" + nom_archivo, fin, null);

                    fin.Close();
                    sftpClient.Disconnect();
                }
            }
            catch (Exception ex)
            { }
        }

        //Consulta de Articulos
        private FormResponseArticulo ConsultaArtBapi(string tipo, string codigo,
                                               string chkCodRef, string CodRef,
                                               string chkCodSap, string CodSap,
                                               string chkGrupoArt, string GrupoArt,
                                               string chkLinea, string LineaNegocio, string CodProveedorCons)
        {

            
            FormResponseArticulo consultaArtBapi = new FormResponseArticulo();
            Art_ConsultaArticulo mod_Consulta;
            List<Art_ConsultaArticulo> lst_retornoSol = new List<Art_ConsultaArticulo>();
            List<DMSolicitudArticulo.SolCabecera> lst_retSol_Cab = new List<DMSolicitudArticulo.SolCabecera>();
            DMSolicitudArticulo.SolCabecera mod_SolCabecera;
            //Detalle
            List<DMSolicitudArticulo.SolDetalle> lst_retSol_Det = new List<DMSolicitudArticulo.SolDetalle>();
            DMSolicitudArticulo.SolDetalle mod_SolDetalle;
            //Medidas
            List<DMSolicitudArticulo.SolMedida> lst_retSol_Med = new List<DMSolicitudArticulo.SolMedida>();
            DMSolicitudArticulo.SolMedida mod_SolMedidas;
            //Codigo Barra
            List<DMSolicitudArticulo.SolCodigoBarra> lst_retSol_CBa = new List<DMSolicitudArticulo.SolCodigoBarra>();
            DMSolicitudArticulo.SolCodigoBarra mod_SolCodBarra;
            //Imagenes
            List<DMSolicitudArticulo.SolImagen> lst_retSol_Ima = new List<DMSolicitudArticulo.SolImagen>();
            DMSolicitudArticulo.SolImagen mod_SolImagenes;
            //Rutas
            List<DMSolicitudArticulo.SolRutas> lst_retSol_Rut = new List<DMSolicitudArticulo.SolRutas>();
            DMSolicitudArticulo.SolRutas mod_SolRutas;
            //Compras
            List<DMSolicitudArticulo.SolCompras> lst_retSol_Com = new List<DMSolicitudArticulo.SolCompras>();
            DMSolicitudArticulo.SolCompras mod_SolCompras;
            //Catalogacion
            List<DMSolicitudArticulo.SolCatalogacion> lst_retSol_Cat = new List<DMSolicitudArticulo.SolCatalogacion>();
            DMSolicitudArticulo.SolCatalogacion mod_SolCatalog;
            //Almacen
            List<DMSolicitudArticulo.SolAlmacen> lst_retSol_Alm = new List<DMSolicitudArticulo.SolAlmacen>();
            DMSolicitudArticulo.SolAlmacen mod_SolAlmacen;
            //IndTipoAlmEnt
            List<DMSolicitudArticulo.SolIndTipoAlmEnt> lst_retSol_Iae = new List<DMSolicitudArticulo.SolIndTipoAlmEnt>();
            DMSolicitudArticulo.SolIndTipoAlmEnt mod_SolIndAlmEnt;
            //IndTipoAlmSal
            List<DMSolicitudArticulo.SolIndTipoAlmSal> lst_retSol_Ias = new List<DMSolicitudArticulo.SolIndTipoAlmSal>();
            DMSolicitudArticulo.SolIndTipoAlmSal mod_SolIndAlmSal;
            //IndAreaAlmacen
            List<DMSolicitudArticulo.SolIndAreaAlmacen> lst_retSol_Ara = new List<DMSolicitudArticulo.SolIndAreaAlmacen>();
            DMSolicitudArticulo.SolIndAreaAlmacen mod_SolAreaAlm;

            List<DMSolicitudArticulo.SolCentros> lst_retSol_Cen = new List<DMSolicitudArticulo.SolCentros>();
            consultaArtBapi.success = true;
            consultaArtBapi.mensaje = "";
            ClsGeneral objEjecucion = new ClsGeneral();
            DataSet ds = new DataSet();
            try{
                if (tipo == "1")
                {
                    ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                    var listaRetorno = Proceso.ConsultaArtBapi(tipo, codigo,
                                                        chkCodRef, CodRef,
                                                        chkCodSap, CodSap,
                                                        chkGrupoArt, GrupoArt,
                                                        chkLinea, LineaNegocio, CodProveedorCons);
                    //List<string> listaRetorno = new List<string>();
                    //listaRetorno.Add("<Root> <PT_ARTICULOS CODSAP=\"40010216\" CODARTPROV=\"NO TIENE\" LINEANEGOCIO=\"S\" DESLINEGOCIO=\"\" MARCA=\"4366\" MARCADES=\"SUPER XTRA\" DESARTICULO=\"ARROZ PRECOCIDO 2KG SUPER XTRA\" PAIS=\"EC\" REGION=\"EC-\" TAMAÑO=\"2 KG.\" GRADOAL=\"\" COLOR=\"0000000000\" SIZE=\"0000000000\" FRAGANCIA=\"\" TIPOS=\"\" SABOR=\"\" NUMOBJINTERNO=\"000000000000000000\" CLASFISCAL=\"0\" TIPODEDUCCION=\"01\" LABOR=\"001\" MODELO=\"\" TIPOCERT=\"\" UNIMEDIDABASE=\"ST\" UNIMEDIDAPEDIDO=\"CS\" UNIMEDIDAPEDIDODES=\"Unidad\" FACTORCONVERSION=\"8\" UNIMEDIDABASE2=\"ST\" PESON=\"0.000\" PESOB=\"0.000\" LONGITUD=\"0.000\" ANCHO=\"0.000\" ALTURA=\"0.000\" PRECIOB=\"19.39\" DESCT1=\"-10.00\" DESCT2=\"0.00\" IMPVERDE=\"0.00\" CODSAPPROV=\"0000106089\" ORGCOMPRAS=\"1001\" NUMANTIGUO=\"335190\" CALENPLAN=\"\" TIPOMAT=\"HAWA\" CATMAT=\"00\" GRPART=\"S29003003\" SECART=\"S29\" SURTPAR=\"\" MATERIA=\"\" COSTOFOB=\"19.39\" INDPEDIDO=\"0\" GRPCOMPRAS=\"C04\" CATVALORACION=\"3100\" CONDALMACENAJE=\"00\" CLASELISTASURT=\"\" STATUSMAT=\"\" STATUSMATCAD=\"\" FECHAINICIO=\"0000-00-00\" COLECTEMPORADA=\"\" TIPTEMPORADA=\"\" ANIOESTACION=\"\" JERARQUIA=\"\" /> <PT_ARTICULOS CODSAP=\"40181988\" CODARTPROV=\"VIEJO\" LINEANEGOCIO=\"S\" DESLINEGOCIO=\"\" MARCA=\"4366\" MARCADES=\"SUPER XTRA\" DESARTICULO=\"ARROZ SUPER EXTRA VIEJO 11.3 KILOS\" PAIS=\"\" REGION=\"-\" TAMAÑO=\"11.3 KILOS\" GRADOAL=\"\" COLOR=\"0000000000\" SIZE=\"0000000000\" FRAGANCIA=\"\" TIPOS=\"\" SABOR=\"\" NUMOBJINTERNO=\"000000000000000000\" CLASFISCAL=\"0\" TIPODEDUCCION=\"01\" LABOR=\"001\" MODELO=\"\" TIPOCERT=\"\" UNIMEDIDABASE=\"ST\" UNIMEDIDAPEDIDO=\"CS\" UNIMEDIDAPEDIDODES=\"Unidad\" FACTORCONVERSION=\"2\" UNIMEDIDABASE2=\"ST\" PESON=\"0.000\" PESOB=\"0.000\" LONGITUD=\"0.000\" ANCHO=\"0.000\" ALTURA=\"0.000\" PRECIOB=\"26.93\" DESCT1=\"-10.00\" DESCT2=\"0.00\" IMPVERDE=\"0.00\" CODSAPPROV=\"0000106089\" ORGCOMPRAS=\"1001\" NUMANTIGUO=\"283255\" CALENPLAN=\"\" TIPOMAT=\"HAWA\" CATMAT=\"00\" GRPART=\"S29003003\" SECART=\"S29\" SURTPAR=\"\" MATERIA=\"\" COSTOFOB=\"26.93\" INDPEDIDO=\"0\" GRPCOMPRAS=\"C04\" CATVALORACION=\"3100\" CONDALMACENAJE=\"00\" CLASELISTASURT=\"\" STATUSMAT=\"\" STATUSMATCAD=\"\" FECHAINICIO=\"0000-00-00\" COLECTEMPORADA=\"\" TIPTEMPORADA=\"\" ANIOESTACION=\"\" JERARQUIA=\"\" /> </Root>");
                    if(listaRetorno.Length>0)
                    { 
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(listaRetorno[0]);
                        XmlNodeList root = xmlDoc.GetElementsByTagName("Root");
                        XmlNodeList lista =
                            ((XmlElement)root[0]).GetElementsByTagName("PT_ARTICULOS");
                        foreach (XmlElement nodo in lista)
                        {
                            if (nodo.GetAttribute("LINEANEGOCIO") == LineaNegocio)
                            {
                                mod_Consulta = new Art_ConsultaArticulo();
                                mod_Consulta.CodSap = nodo.GetAttribute("CODSAP");
                                mod_Consulta.CodReferencia = nodo.GetAttribute("CODARTPROV");
                                mod_Consulta.Marca = nodo.GetAttribute("MARCADES");
                                mod_Consulta.Descripcion = nodo.GetAttribute("DESARTICULO");
                                mod_Consulta.PersonaContacto = "";
                                mod_Consulta.Email = "";
                                mod_Consulta.PaisOrigen = "";
                                lst_retornoSol.Add(mod_Consulta);
                            }                       
                        }
                        consultaArtBapi.success = true;
                        consultaArtBapi.root.Add(lst_retornoSol);
                        return consultaArtBapi;
                    }
                }
                if (tipo == "2")
                {
                    ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                    CatalogosController catalag = new CatalogosController();
                   

                    var listaRetorno = Proceso.ConsultaArtBapi(tipo, codigo,
                                                        chkCodRef, CodRef,
                                                        chkCodSap, CodSap,
                                                        chkGrupoArt, GrupoArt,
                                                        chkLinea, LineaNegocio, CodProveedorCons);
                    //List<string> listaRetorno = new List<string>();
                    //listaRetorno.Add("<Root><PT_EAN IDDETALLE=\"1\" UNIDVISU=\"ST\" NUMEAN=\"7861035002000\" TIPEAN=\"HE\" EANPRI=\"\" />  <PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R190\" DESCAT=\"HIPERMARKET BAHIA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R252\" DESCAT=\"HIPERMARKET BALLENITA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R167\" DESCAT=\"HIPERMARKET BABAHOYO\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R184\" DESCAT=\"HIPERMARKET PLAYAS\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R205\" DESCAT=\"HIPERMARKET PASEO DURAN\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R094\" DESCAT=\"COMISARIATO GARZOTA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R069\" DESCAT=\"COMISARIATO PARQUE CALIFORNIA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R189\" DESCAT=\"HIPERMARKET VIA A LA COSTA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R192\" DESCAT=\"HIPERMARKET 12.5 VIA DAULE\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R174\" DESCAT=\"HIPERMARKET DAULE\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R003\" DESCAT=\"COMISARIATO URDESA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R152\" DESCAT=\"HIPERMARKET QUEVEDO\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R008\" DESCAT=\"COMISARIATO FENIX\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R169\" DESCAT=\"HIPERMARKET VERGELES\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R100\" DESCAT=\"HIPERMARKET ELOY ALFARO\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R109\" DESCAT=\"HIPERMARKET SANTO DOMINGO\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R112\" DESCAT=\"METROPOLIS RIOCEIBOS\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R116\" DESCAT=\"HIPERMARKET MACHALA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R120\" DESCAT=\"HIPERMARKET VIA DAULE\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R001\" DESCAT=\"COMISARIATO CENTRO\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R006\" DESCAT=\"HIPERMARKET SUR\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R010\" DESCAT=\"COMISARIATO ALBORADA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R011\" DESCAT=\"COMISARIATO NN UU\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R020\" DESCAT=\"HIPERMARKET ALBAN BORJA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R030\" DESCAT=\"HIPERMARKET LA LIBERTAD\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R012\" DESCAT=\"HIPERMARKET PRENSA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R013\" DESCAT=\"COMISARIATO GARCIA MORENO\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R014\" DESCAT=\"COMISARIATO CEIBOS 4 ½\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R017\" DESCAT=\"COMISARIATO MACHALA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R018\" DESCAT=\"COMISARIATO  PLAZA QUIL\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R019\" DESCAT=\"HIPERMARKET DURAN\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R032\" DESCAT=\"COMISARIATO VILLAFLORA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R041\" DESCAT=\"COMISARIATO RIOCENTRO PUNTILLA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R049\" DESCAT=\"BODEGA CENTRAL\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R050\" DESCAT=\"HIPERMARKET PORTOVIEJO\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R064\" DESCAT=\"HIPERMARKET MILAGRO\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R065\" DESCAT=\"HIPERMARKET NORTE\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R072\" DESCAT=\"COMISARIATO MANTA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R093\" DESCAT=\"HIPERMARKET RIOCEIBOS\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R113\" DESCAT=\"BRINKER RIOCEIBOS\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R009\" DESCAT=\"COMISARIATO AMERICAS\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R128\" DESCAT=\"HIPERMARKET VALLE DE LOS CHILL\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R157\" DESCAT=\"HIPERMARKET RIOBAMBA\" /><PT_CATALOGACION IDDETALLE=\"1\" CODCAT=\"R191\" DESCAT=\"HIPERMARKET RIOCENTRO EL DORAD\" /><PT_BALANZAS IDDETALLE=\"1\" GRPBAL=\"0003\" DESBAL=\"No pesar\" CNLDIS=\"01\" /><PT_ARTICULOS IDDETALLE=\"1\" CODSAP=\"40010216\" CODARTPROV=\"NO TIENE\" LINEANEGOCIO=\"S\" DESLINEGOCIO=\"\" MARCA=\"4366\" MARCADES=\"SUPER XTRA\" DESARTICULO=\"ARROZ PRECOCIDO 2KG SUPER XTRA\" PAIS=\"EC\" REGION=\"EC-\" TAMAÑO=\"2 KG.\" GRADOAL=\"\" COLOR=\"0000000000\" SIZE=\"0000000000\" FRAGANCIA=\"\" TIPOS=\"\" SABOR=\"\" NUMOBJINTERNO=\"000000000000000000\" CLASFISCAL=\"0\" TIPODEDUCCION=\"01\" LABOR=\"001\" MODELO=\"\" TIPOCERT=\"\" UNIMEDIDABASE=\"ST\" UNIMEDIDAPEDIDO=\"CS\" UNIMEDIDAPEDIDODES=\"Unidad\" FACTORCONVERSION=\"8\" UNIMEDIDABASE2=\"ST\" PESON=\"0.000\" PESOB=\"0.000\" LONGITUD=\"0.000\" ANCHO=\"0.000\" ALTURA=\"0.000\" PRECIOB=\"19.39\" DESCT1=\"-10.00\" DESCT2=\"0.00\" IMPVERDE=\"0.00\" CODSAPPROV=\"0000106089\" ORGCOMPRAS=\"1001\" NUMANTIGUO=\"335190\" CALENPLAN=\"\" TIPOMAT=\"HAWA\" CATMAT=\"00\" GRPART=\"S29003003\" SECART=\"S29\" SURTPAR=\"\" MATERIA=\"\" COSTOFOB=\"19.39\" INDPEDIDO=\"0\" GRPCOMPRAS=\"C04\" CATVALORACION=\"3100\" CONDALMACENAJE=\"00\" CLASELISTASURT=\"\" STATUSMAT=\"\" STATUSMATCAD=\"\" FECHAINICIO=\"0000-00-00\" COLECTEMPORADA=\"\" TIPTEMPORADA=\"\" ANIOESTACION=\"\" JERARQUIA=\"\" /></Root>");
                    //listaRetorno.Add("<Root> <PT_EAN IDDETALLE=\"2\" UNIDVISU=\"ST\" NUMEAN=\"7861035002000\" TIPEAN=\"HE\" EANPRI=\"\" /><PT_EAN IDDETALLE=\"2\" UNIDVISU=\"ST\" NUMEAN=\"7861035000341\" TIPEAN=\"HE\" EANPRI=\"X\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R190\" DESCAT=\"HIPERMARKET BAHIA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R252\" DESCAT=\"HIPERMARKET BALLENITA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R167\" DESCAT=\"HIPERMARKET BABAHOYO\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R184\" DESCAT=\"HIPERMARKET PLAYAS\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R205\" DESCAT=\"HIPERMARKET PASEO DURAN\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R094\" DESCAT=\"COMISARIATO GARZOTA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R069\" DESCAT=\"COMISARIATO PARQUE CALIFORNIA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R189\" DESCAT=\"HIPERMARKET VIA A LA COSTA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R192\" DESCAT=\"HIPERMARKET 12.5 VIA DAULE\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R174\" DESCAT=\"HIPERMARKET DAULE\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R003\" DESCAT=\"COMISARIATO URDESA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R152\" DESCAT=\"HIPERMARKET QUEVEDO\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R008\" DESCAT=\"COMISARIATO FENIX\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R169\" DESCAT=\"HIPERMARKET VERGELES\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R100\" DESCAT=\"HIPERMARKET ELOY ALFARO\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R109\" DESCAT=\"HIPERMARKET SANTO DOMINGO\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R112\" DESCAT=\"METROPOLIS RIOCEIBOS\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R116\" DESCAT=\"HIPERMARKET MACHALA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R120\" DESCAT=\"HIPERMARKET VIA DAULE\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R001\" DESCAT=\"COMISARIATO CENTRO\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R006\" DESCAT=\"HIPERMARKET SUR\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R010\" DESCAT=\"COMISARIATO ALBORADA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R011\" DESCAT=\"COMISARIATO NN UU\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R020\" DESCAT=\"HIPERMARKET ALBAN BORJA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R030\" DESCAT=\"HIPERMARKET LA LIBERTAD\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R012\" DESCAT=\"HIPERMARKET PRENSA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R013\" DESCAT=\"COMISARIATO GARCIA MORENO\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R014\" DESCAT=\"COMISARIATO CEIBOS 4 ½\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R017\" DESCAT=\"COMISARIATO MACHALA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R018\" DESCAT=\"COMISARIATO  PLAZA QUIL\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R019\" DESCAT=\"HIPERMARKET DURAN\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R032\" DESCAT=\"COMISARIATO VILLAFLORA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R041\" DESCAT=\"COMISARIATO RIOCENTRO PUNTILLA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R049\" DESCAT=\"BODEGA CENTRAL\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R050\" DESCAT=\"HIPERMARKET PORTOVIEJO\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R064\" DESCAT=\"HIPERMARKET MILAGRO\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R065\" DESCAT=\"HIPERMARKET NORTE\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R072\" DESCAT=\"COMISARIATO MANTA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R093\" DESCAT=\"HIPERMARKET RIOCEIBOS\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R113\" DESCAT=\"BRINKER RIOCEIBOS\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R009\" DESCAT=\"COMISARIATO AMERICAS\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R128\" DESCAT=\"HIPERMARKET VALLE DE LOS CHILL\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R157\" DESCAT=\"HIPERMARKET RIOBAMBA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R191\" DESCAT=\"HIPERMARKET RIOCENTRO EL DORAD\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R049\" DESCAT=\"BODEGA CENTRAL\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R011\" DESCAT=\"COMISARIATO NN UU\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R012\" DESCAT=\"HIPERMARKET PRENSA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R013\" DESCAT=\"COMISARIATO GARCIA MORENO\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R032\" DESCAT=\"COMISARIATO VILLAFLORA\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R128\" DESCAT=\"HIPERMARKET VALLE DE LOS CHILL\" /><PT_CATALOGACION IDDETALLE=\"2\" CODCAT=\"R157\" DESCAT=\"HIPERMARKET RIOBAMBA\" /><PT_BALANZAS IDDETALLE=\"2\" GRPBAL=\"0003\" DESBAL=\"No pesar\" CNLDIS=\"01\" /><PT_BALANZAS IDDETALLE=\"2\" GRPBAL=\"0003\" DESBAL=\"No pesar\" CNLDIS=\"01\" /><PT_ARTICULOS IDDETALLE=\"2\" CODSAP=\"40181988\" CODARTPROV=\"VIEJO\" LINEANEGOCIO=\"S\" DESLINEGOCIO=\"\" MARCA=\"4366\" MARCADES=\"SUPER XTRA\" DESARTICULO=\"ARROZ SUPER EXTRA VIEJO 11.3 KILOS\" PAIS=\"\" REGION=\"-\" TAMAÑO=\"11.3 KILOS\" GRADOAL=\"\" COLOR=\"0000000000\" SIZE=\"0000000000\" FRAGANCIA=\"\" TIPOS=\"\" SABOR=\"\" NUMOBJINTERNO=\"000000000000000000\" CLASFISCAL=\"0\" TIPODEDUCCION=\"01\" LABOR=\"001\" MODELO=\"\" TIPOCERT=\"\" UNIMEDIDABASE=\"ST\" UNIMEDIDAPEDIDO=\"CS\" UNIMEDIDAPEDIDODES=\"Unidad\" FACTORCONVERSION=\"2\" UNIMEDIDABASE2=\"ST\" PESON=\"0.000\" PESOB=\"0.000\" LONGITUD=\"0.000\" ANCHO=\"0.000\" ALTURA=\"0.000\" PRECIOB=\"26.93\" DESCT1=\"-10.00\" DESCT2=\"0.00\" IMPVERDE=\"0.00\" CODSAPPROV=\"0000106089\" ORGCOMPRAS=\"1001\" NUMANTIGUO=\"283255\" CALENPLAN=\"\" TIPOMAT=\"HAWA\" CATMAT=\"00\" GRPART=\"S29003003\" SECART=\"S29\" SURTPAR=\"\" MATERIA=\"\" COSTOFOB=\"26.93\" INDPEDIDO=\"0\" GRPCOMPRAS=\"C04\" CATVALORACION=\"3100\" CONDALMACENAJE=\"00\" CLASELISTASURT=\"\" STATUSMAT=\"\" STATUSMATCAD=\"\" FECHAINICIO=\"0000-00-00\" COLECTEMPORADA=\"\" TIPTEMPORADA=\"\" ANIOESTACION=\"\" JERARQUIA=\"\" /></Root>");
                    if (listaRetorno.Length>0)
                    {
                        consultaArtBapi.success = true; consultaArtBapi.mensaje = "OK";
                        foreach (var itemRetorno in listaRetorno )
                        {
                            
                            //Datos del articulo
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(itemRetorno);
                            XmlNodeList root = xmlDoc.GetElementsByTagName("Root");


                            XmlNodeList lista =
                                ((XmlElement)root[0]).GetElementsByTagName("PT_ARTICULOS");
                            foreach (XmlElement nodo in lista)
                            {
                                //Obtner datos adiconales que no trae la BAPI
                                codigo = Convert.ToInt32(nodo.GetAttribute("CODSAP") ).ToString();
                                var indDetalle = nodo.GetAttribute("IDDETALLE");
                                var textoBreve = "";
                                var PI_ParamXML =
                                new System.Xml.Linq.XDocument(
                                new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                new XElement("Root",
                                         new XAttribute("DatosAdic", "S"),
                                         new XAttribute("CodSAPArticulo", codigo != null ? codigo : "")
                                         ));

                                ds = objEjecucion.EjecucionGralDs(PI_ParamXML.ToString(), 100, 1); //Articulo.Art_P_Consulta	100
                                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                                {
                                    String codSol = "";
                                    //Imagenes
                                    foreach (DataRow item in ds.Tables[0].Rows)
                                    {
                                        mod_SolImagenes = new DMSolicitudArticulo.SolImagen();
                                        mod_SolImagenes.IdDetalle = indDetalle.ToString();
                                        mod_SolImagenes.IdDocAdjunto = Convert.ToString(item["IdDocAdjunto"]);
                                        mod_SolImagenes.NomArchivo = Convert.ToString(item["NomArchivo"]);
                                        mod_SolImagenes.Path = Convert.ToString(item["Path"]);
                                        //codSol = mod_SolImagenes.Path.Split('-')[0];
                                        mod_SolImagenes.Accion = "C";
                                        lst_retSol_Ima.Add(mod_SolImagenes);
                                        //Descarga las imagenes del ftp y las crea en una carpeta temporal [idsol-idart]
                                        BajaFptArchivo(Convert.ToString(item["Path"]), Convert.ToString(item["NomArchivo"]), "");
                                    }

                                    //Rutas
                                    foreach (DataRow item in ds.Tables[0].Rows) //Recorre el detalle y concatena el id de la solicitud
                                    {
                                        mod_SolRutas = new DMSolicitudArticulo.SolRutas();
                                        mod_SolRutas.IdDetalle = indDetalle.ToString();
                                        mod_SolRutas.Path = Convert.ToString(item["Path"]);
                                        lst_retSol_Rut.Add(mod_SolRutas);
                                    }
                                    foreach (DataRow item in ds.Tables[1].Rows) //Datos Adicionales
                                    {
                                        textoBreve = Convert.ToString(item["TextoBreve"]);
                                    }
                                }
                                    //Datos de cabecera
                                mod_SolCabecera = new DMSolicitudArticulo.SolCabecera();
                                mod_SolCabecera.CodProveedor = CodProveedorCons;
                                mod_SolCabecera.IdSolicitud = "";
                                mod_SolCabecera.TipoSolicitud = "2";
                                mod_SolCabecera.LineaNegocio = nodo.GetAttribute("LINEANEGOCIO");
                                mod_SolCabecera.Accion = "";
                                mod_SolCabecera.Estado = "";
                                mod_SolCabecera.Usuario = "";
                                lst_retSol_Cab.Add(mod_SolCabecera);
                                //Detalle                                    
                                mod_SolDetalle = new DMSolicitudArticulo.SolDetalle();
                                mod_SolDetalle.IdDetalle = indDetalle.ToString();
                                mod_SolDetalle.CodReferencia = nodo.GetAttribute("CODARTPROV"); 
                                mod_SolDetalle.CodSAPart = nodo.GetAttribute("CODSAP");
                                mod_SolDetalle.Marca = nodo.GetAttribute("MARCA"); 
                                mod_SolDetalle.DesMarca = nodo.GetAttribute("MARCADES");
                                mod_SolDetalle.CodProveedor = CodProveedorCons;
                                mod_SolDetalle.MarcaNueva = "";
                                mod_SolDetalle.PaisOrigen = nodo.GetAttribute("PAIS");
                                mod_SolDetalle.RegionOrigen = nodo.GetAttribute("REGION");
                                mod_SolDetalle.TamArticulo = nodo.GetAttribute("TAMAÑO"); 
                                mod_SolDetalle.GradoAlcohol = nodo.GetAttribute("GRADOAL");
                                mod_SolDetalle.Talla = nodo.GetAttribute("SIZE");
                                mod_SolDetalle.Color = nodo.GetAttribute("COLOR"); 
                                mod_SolDetalle.Fragancia = nodo.GetAttribute("FRAGANCIA"); 
                                mod_SolDetalle.Tipos = nodo.GetAttribute("TIPOS"); 
                                mod_SolDetalle.Sabor = nodo.GetAttribute("SABOR");
                                mod_SolDetalle.Modelo = nodo.GetAttribute("MODELO"); 
                                mod_SolDetalle.Descripcion = nodo.GetAttribute("DESARTICULO"); 
                                mod_SolDetalle.PrecioBruto = nodo.GetAttribute("PRECIOB").Replace(',', '.'); 
                                mod_SolDetalle.Descuento1 = nodo.GetAttribute("DESCT1").Replace(',', '.'); 
                                mod_SolDetalle.Descuento2 = nodo.GetAttribute("DESCT2").Replace(',', '.'); 
                                if (Convert.ToDecimal(nodo.GetAttribute("IMPVERDE")) > 0)
                                    mod_SolDetalle.ImpVerde = true;
                                else
                                    mod_SolDetalle.ImpVerde = false;
                                mod_SolDetalle.OtroId = textoBreve;
                                //mod_SolDetalle.ContAlcohol = Convert.ToBoolean("N");
                                mod_SolDetalle.ContAlcohol = false;
                                mod_SolDetalle.Estado =  Convert.ToString("PE");
                                mod_SolDetalle.Iva = nodo.GetAttribute("CLASFISCAL"); 
                                mod_SolDetalle.Deducible = nodo.GetAttribute("TIPODEDUCCION"); 
                                mod_SolDetalle.Retencion = nodo.GetAttribute("LABOR");
                                mod_SolDetalle.Accion = Convert.ToString("I");
                                mod_SolDetalle.CantidadPedir = nodo.GetAttribute("CANPED");
                                mod_SolDetalle.Coleccion = nodo.GetAttribute("COLECTEMPORADA");
                                mod_SolDetalle.Temporada = nodo.GetAttribute("TIPTEMPORADA");
                                mod_SolDetalle.Estacion = nodo.GetAttribute("ANIOESTACION");
                                var tipoMaterial = nodo.GetAttribute("CATMAT");
                                if (tipoMaterial == "00")
                                {
                                    mod_SolDetalle.CodGenerico = "0";
                                    mod_SolDetalle.IsGenerico = false;
                                    mod_SolDetalle.IsVariante = false;
                                }
                                if (tipoMaterial == "01")
                                {
                                    mod_SolDetalle.CodGenerico = "0";
                                    mod_SolDetalle.IsGenerico = true;
                                    mod_SolDetalle.IsVariante = false;
                                }
                                if (tipoMaterial == "02")
                                {
                                    mod_SolDetalle.CodGenerico = "0";
                                    mod_SolDetalle.IsGenerico = false;
                                    mod_SolDetalle.IsVariante = true;
                                }
                               

                                lst_retSol_Det.Add(mod_SolDetalle);   
                               
                                mod_SolCompras = new DMSolicitudArticulo.SolCompras();
                                mod_SolCompras.IdDetalle = indDetalle.ToString();
                                mod_SolCompras.OrganizacionCompras = nodo.GetAttribute("ORGCOMPRAS"); 
                                mod_SolCompras.FrecuenciaEntrega = nodo.GetAttribute("CALENPLAN"); 
                                mod_SolCompras.TipoMaterial = nodo.GetAttribute("TIPOMAT"); 
                                mod_SolCompras.CategoriaMaterial = nodo.GetAttribute("CATMAT"); 
                                mod_SolCompras.GrupoArticulo = nodo.GetAttribute("GRPART");
                                mod_SolCompras.SeccionArticulo = nodo.GetAttribute("SECART");
                              
                                mod_SolCompras.SurtidoParcial = nodo.GetAttribute("SURTPAR"); 
                                mod_SolCompras.Materia = nodo.GetAttribute("MATERIA"); 
                                mod_SolCompras.CostoFOB = nodo.GetAttribute("PRECIOB").Replace(',', '.');
                                mod_SolCompras.IndPedido = nodo.GetAttribute("INDPEDIDO"); 
                                //mod_SolCompras.PerfilDistribucion = Convert.ToString(item.);
                                //mod_SolCompras.Almacen = Convert.ToString(item["Almacen"]);
                                mod_SolCompras.GrupoCompra = nodo.GetAttribute("GRPCOMPRAS");
                                mod_SolCompras.CategoriaValoracion = nodo.GetAttribute("CATVALORACION"); 
                                //mod_SolCompras.TipoAlamcen = Convert.ToString(item.);
                                //mod_SolCompras.IndAlmaEntrada = Convert.ToString(item["IndAlmaEntrada"]);
                                //mod_SolCompras.IndAlmaSalida = Convert.ToString(item["IndAlmaSalida"]);
                                //mod_SolCompras.IndAreaAlmacen = Convert.ToString(item["IndAreaAlmacen"]);
                                mod_SolCompras.CondicionAlmacen = nodo.GetAttribute("CONDALMACENAJE"); 
                                mod_SolCompras.ClListaSurtido = nodo.GetAttribute("CLASELISTASURT"); 
                                mod_SolCompras.EstatusMaterial = nodo.GetAttribute("STATUSMAT");
                                mod_SolCompras.EstatusVenta = nodo.GetAttribute("STATUSMATCAD");
                                if (nodo.GetAttribute("FECHAINICIO")  == "0000-00-00")
                                    mod_SolCompras.ValidoDesde = "";
                                else
                                    mod_SolCompras.ValidoDesde = nodo.GetAttribute("FECHAINICIO"); 
                               
                                mod_SolCompras.Coleccion = nodo.GetAttribute("COLECTEMPORADA");
                                mod_SolCompras.Temporada = nodo.GetAttribute("TIPTEMPORADA"); 
                                mod_SolCompras.Estacion = nodo.GetAttribute("ANIOESTACION"); 
                                mod_SolCompras.JerarquiaProd = nodo.GetAttribute("JERARQUIA");
                                mod_SolCompras.Undbase = nodo.GetAttribute("UNIMEDIDABASE");
                                mod_SolCompras.Undpedido = nodo.GetAttribute("UNIMEDIDAPEDIDO");
                                if (mod_SolCompras.Undpedido == "")
                                    mod_SolCompras.Undpedido = nodo.GetAttribute("UNIMEDIDABASE");
                                mod_SolCompras.Undes = nodo.GetAttribute("UNIES");
                                if (mod_SolCompras.Undes == "")
                                    mod_SolCompras.Undes = nodo.GetAttribute("UNIMEDIDABASE");
                                mod_SolCompras.Undventa = nodo.GetAttribute("UNIVENTA");
                                if (mod_SolCompras.Undventa == "")
                                    mod_SolCompras.Undventa = nodo.GetAttribute("UNIMEDIDABASE");

                                
                                mod_SolCompras.Estado = Convert.ToString("AA");
                                lst_retSol_Com.Add(mod_SolCompras);
                                
                            }


                            lista =
                                ((XmlElement)root[0]).GetElementsByTagName("PT_UNIMED");
                            foreach (XmlElement nodo in lista)
                            {
                                mod_SolMedidas = new DMSolicitudArticulo.SolMedida();
                                mod_SolMedidas.IdDetalle = nodo.GetAttribute("IDDETALLE");
                                var unidad = lst_retSol_Com.Where(x => x.IdDetalle == mod_SolMedidas.IdDetalle).FirstOrDefault();

                                mod_SolMedidas.UnidadMedida = nodo.GetAttribute("UNBASE");

                                mod_SolMedidas.uniMedBase = false;
                                if (mod_SolMedidas.UnidadMedida == unidad.Undbase)
                                    mod_SolMedidas.uniMedBase = true;
                                mod_SolMedidas.uniMedPedido = false;
                                if (mod_SolMedidas.UnidadMedida == unidad.Undpedido)
                                    mod_SolMedidas.uniMedPedido = true;
                                mod_SolMedidas.uniMedES = false;
                                if (mod_SolMedidas.UnidadMedida == unidad.Undes)
                                    mod_SolMedidas.uniMedES = true;
                                mod_SolMedidas.uniMedVenta = false;
                                if (mod_SolMedidas.UnidadMedida == unidad.Undventa)
                                    mod_SolMedidas.uniMedVenta = true;

                                mod_SolMedidas.UniMedConvers = unidad.Undbase;
                                //if (mod_SolMedidas.uniMedBase)
                                //    mod_SolMedidas.UniMedConvers = unidad.Undpedido;
                                //else
                                //    mod_SolMedidas.UniMedConvers = mod_SolMedidas.UnidadMedida;
                                mod_SolMedidas.FactorCon = nodo.GetAttribute("CONVER").Replace(',', '.');
                                mod_SolMedidas.PesoNeto = nodo.GetAttribute("PESON").Replace(',', '.');
                                mod_SolMedidas.PesoBruto = nodo.GetAttribute("PESBRT").Replace(',', '.');
                                mod_SolMedidas.Longitud = nodo.GetAttribute("LONGIT").Replace(',', '.');
                                mod_SolMedidas.Ancho = nodo.GetAttribute("ANCHO").Replace(',', '.');
                                mod_SolMedidas.Altura = nodo.GetAttribute("ALTURA").Replace(',', '.');
                                mod_SolMedidas.Volumen = nodo.GetAttribute("VOLUM").Replace(',', '.');
                                mod_SolMedidas.UniMedidaVolumen = nodo.GetAttribute("UNIVOL");
                                

                                mod_SolMedidas.Estado = Convert.ToString("NN");
                                mod_SolMedidas.Accion = Convert.ToString("I");
                                lst_retSol_Med.Add(mod_SolMedidas);
                            }
   
                            lista =
                                ((XmlElement)root[0]).GetElementsByTagName("PT_EAN");
                            foreach (XmlElement nodo in lista)
                            {
                                mod_SolCodBarra = new DMSolicitudArticulo.SolCodigoBarra();
                                mod_SolCodBarra.IdDetalle = nodo.GetAttribute("IDDETALLE"); 
                                mod_SolCodBarra.UnidadMedida = nodo.GetAttribute("UNIDVISU"); 
                                mod_SolCodBarra.NumeroEan = nodo.GetAttribute("NUMEAN"); 
                                mod_SolCodBarra.TipoEan = nodo.GetAttribute("TIPEAN");
                                //mod_SolCodBarra.DescripcionEan = Convert.ToString(item.);
                                if (nodo.GetAttribute("EANPRI") == "X")
                                {
                                    mod_SolCodBarra.Principal = true;
                                }
                                else
                                {
                                    mod_SolCodBarra.Principal = false;
                                }

                                mod_SolCodBarra.Accion = Convert.ToString("I");
                                if (nodo.GetAttribute("NUMEAN") != "")
                                    lst_retSol_CBa.Add(mod_SolCodBarra);
                            }

                            lista =
                                ((XmlElement)root[0]).GetElementsByTagName("PT_ALMACEN");
                            foreach (XmlElement nodo in lista)
                            {
                                mod_SolAlmacen = new DMSolicitudArticulo.SolAlmacen();
                                mod_SolAlmacen.IdDetalle = nodo.GetAttribute("IDDETALLE");
                                mod_SolAlmacen.Almacen = nodo.GetAttribute("NUMALM"); 
                                mod_SolAlmacen.DesAlmacen = Convert.ToString("");
                                mod_SolAlmacen.TipAlmacen = nodo.GetAttribute("TIPALM"); 
                                mod_SolAlmacen.DestipAlmacen =  Convert.ToString("");
                                mod_SolAlmacen.IndAlmacenE = nodo.GetAttribute("INDALME");
                                mod_SolAlmacen.DesindAlmacenE =  Convert.ToString("");
                                mod_SolAlmacen.IndAlmacenS = nodo.GetAttribute("INDALMS"); 
                                mod_SolAlmacen.DesindAlmacenS = Convert.ToString("");

                                mod_SolAlmacen.Accion = Convert.ToString("I");
                                lst_retSol_Alm.Add(mod_SolAlmacen);
                            }
                            lista =
                                ((XmlElement)root[0]).GetElementsByTagName("PT_CATALOGACION");
                            foreach (XmlElement nodo in lista)
                            {
                                mod_SolCatalog = new DMSolicitudArticulo.SolCatalogacion();
                                mod_SolCatalog.IdDetalle = nodo.GetAttribute("IDDETALLE");
                                mod_SolCatalog.Catalogacion = nodo.GetAttribute("CODCAT"); 
                                mod_SolCatalog.DesCatalogacion = nodo.GetAttribute("DESCAT");
                                mod_SolCatalog.Canaldistribucion = nodo.GetAttribute("CANDIS");
                                var catalogo = catalag.GetCatalogos("tbl_Canaldistribucion");
                                var desCanalDis = catalogo.Where(x => x.Codigo == mod_SolCatalog.Canaldistribucion).FirstOrDefault();
                                mod_SolCatalog.DesCanaldistribucion =  desCanalDis != null ? desCanalDis.Detalle : "";
                                mod_SolCatalog.Accion = Convert.ToString("I");
                                lst_retSol_Cat.Add(mod_SolCatalog);
                            }
                            lista =
                                ((XmlElement)root[0]).GetElementsByTagName("PT_BALANZAS");
                            foreach (XmlElement nodo in lista)
                            {
                                mod_SolAreaAlm = new DMSolicitudArticulo.SolIndAreaAlmacen();
                                mod_SolAreaAlm.IdDetalle = nodo.GetAttribute("IDDETALLE"); 
                                mod_SolAreaAlm.IndAreaAlmacen = nodo.GetAttribute("CNLDIS"); 
                                mod_SolAreaAlm.DesIndAreaAlmacen =  Convert.ToString("");
                                mod_SolAreaAlm.GrupoBalanzas = nodo.GetAttribute("GRPBAL"); 
                                mod_SolAreaAlm.DesgrupoBalanzas = nodo.GetAttribute("DESBAL"); 
                                mod_SolAreaAlm.Accion = Convert.ToString("I");
                                lst_retSol_Ara.Add(mod_SolAreaAlm);
                            }
                        }

                        if (lst_retSol_Cab.Count > 0)
                        {
                            //Cabecera               
                            consultaArtBapi.success = true;
                            consultaArtBapi.root.Add(lst_retSol_Cab);
                            //Detalle              
                            consultaArtBapi.root.Add(lst_retSol_Det);
                            //Medidas                
                            consultaArtBapi.root.Add(lst_retSol_Med);
                            //Codigos de Barra                
                            consultaArtBapi.root.Add(lst_retSol_CBa);
                            //Consultar datos que no traen las BAPIS(Imagenes y otros campos)
                            consultaArtBapi.root.Add(lst_retSol_Ima);
                            consultaArtBapi.root.Add(lst_retSol_Rut);
                            //Compras              
                            consultaArtBapi.root.Add(lst_retSol_Com);
                            //Catalogaciones
                            consultaArtBapi.root.Add(lst_retSol_Cat);
                            //Almacen                
                            consultaArtBapi.root.Add(lst_retSol_Alm);
                            //IndTipoAlmEnt
                            consultaArtBapi.root.Add(lst_retSol_Iae);
                            //IndTipoAlmSal
                            consultaArtBapi.root.Add(lst_retSol_Ias);
                            //Balanzas                 
                            consultaArtBapi.root.Add(lst_retSol_Ara);
                            //Centros
                            consultaArtBapi.root.Add(lst_retSol_Cen);
                            var lst_retSol_Caract = new DMSolicitudArticulo.SolCaracteristicas();
                            consultaArtBapi.root.Add(lst_retSol_Caract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                consultaArtBapi.success = false;
                consultaArtBapi.mensaje = ex.Message.ToString();

            }


            return consultaArtBapi;
        }
        

    }
}