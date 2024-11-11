using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJSAuthentication.API.Models;
using System.Data;
using System.Xml;
using System.Security.Claims;
using clibProveedores.Models;
using clibProveedores;
using System.IO;
using System.Web;
using Renci.SshNet;
using System.Xml.Linq;
using SAP.Middleware.Connector;
using System.Threading;
using AngularJSAuthentication.API.Controllers;








namespace clibProveedores
{

    [RoutePrefix("api/ActasEnviaAdmin")]
    public class Art_SolicitudController : ApiController
    {

        [ActionName("ConsSolArticulo")]
        [HttpGet]
        public FormResponseArticulo GetconsSolArticulo(string tipo, string codigo,
                                               string chkCodRef, string CodRef,
                                               string chkCodSap, string CodSap,
                                               string chkFecha, string FechaDesde, string FechaHasta,
                                               string chkEstado, string Estado,
                                               string chkTipoSol, string TipoSolicitud,
                                               string chkLinea, string LineaNegocio, string ArtUsuario, string ArtNivel, string CodProveedorCons)
        {
            List<Art_ConsultaSolicitud> lst_retornoSol = new List<Art_ConsultaSolicitud>();
            Art_ConsultaSolicitud mod_Solicitud;

            //********************************************
            //Cabecera
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
            //Caracterisiticas
            List<DMSolicitudArticulo.SolCaracteristicas> lst_retSol_Carac = new List<DMSolicitudArticulo.SolCaracteristicas>();
            DMSolicitudArticulo.SolCaracteristicas mod_SolCaracteristicas;
            //Observaciones
            List<DMSolicitudArticulo.SolObserv> lst_retSol_Obs = new List<DMSolicitudArticulo.SolObserv>();
            DMSolicitudArticulo.SolObserv mod_SolObserv;
            //Centros
            List<DMSolicitudArticulo.SolCentros> lst_retSol_Cen = new List<DMSolicitudArticulo.SolCentros>();
            DMSolicitudArticulo.SolCentros mod_SolCentros;
            //Codigos Legacy
            List<DMSolicitudArticulo.CodigoLegacy> lst_codLegacy = new List<DMSolicitudArticulo.CodigoLegacy>();
            string CodSAPprov = "";
            //********************************************

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            FormResponseArticulo FormResponse = new FormResponseArticulo();
            xmlParam.LoadXml("<Root />");
            try
            {
                xmlParam.DocumentElement.SetAttribute("tipo", tipo);
                xmlParam.DocumentElement.SetAttribute("IdSolicitud", (tipo == "1" ? "0" : codigo));

                xmlParam.DocumentElement.SetAttribute("IdOrganizacion", "");
                xmlParam.DocumentElement.SetAttribute("CodProveedor", "");
                if (Convert.ToBoolean(chkCodRef)) xmlParam.DocumentElement.SetAttribute("CodReferencia", CodRef);
                if (Convert.ToBoolean(chkCodSap)) xmlParam.DocumentElement.SetAttribute("CodSAP", CodSap);
                if (Convert.ToBoolean(chkFecha))
                {
                    xmlParam.DocumentElement.SetAttribute("FechaDesde", FechaDesde);
                    xmlParam.DocumentElement.SetAttribute("FechaHasta", FechaHasta);
                }
                if (Convert.ToBoolean(chkEstado))
                {
                    List<string> TagIds = Estado.Split(',').Select(Convert.ToString).ToList();
                    for (int i = 0; i < TagIds.Count; i++)
                    {
                        XmlElement elem = xmlParam.CreateElement("Est");
                        elem.SetAttribute("id", TagIds[i].ToString());
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                if (Convert.ToBoolean(chkTipoSol)) xmlParam.DocumentElement.SetAttribute("TipoSolicitud", TipoSolicitud);
                if (Convert.ToBoolean(chkLinea)) xmlParam.DocumentElement.SetAttribute("LineaNegocio", LineaNegocio);
                xmlParam.DocumentElement.SetAttribute("Usuario", ArtUsuario);
                xmlParam.DocumentElement.SetAttribute("Nivel", ArtNivel);
                xmlParam.DocumentElement.SetAttribute("CodProveedorSol", CodProveedorCons);

                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 101, 1); //Articulo.Sol_P_Consulta	101
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //Bandeja de Solicitudes
                    if (tipo == "1")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_Solicitud = new Art_ConsultaSolicitud();
                            mod_Solicitud.IdSolicitud = Convert.ToString(item["IdSolicitud"]);
                            mod_Solicitud.IdTipoSolicitud = Convert.ToString(item["IdTipoSolicitud"]);
                            mod_Solicitud.TipoSolArticulo = Convert.ToString(item["TipoSolicitud"]);
                            mod_Solicitud.CodSapProveedor = Convert.ToString(item["CodProveedor"]);
                            mod_Solicitud.CantidadArticulos = Convert.ToString(item["CantidadArticulos"]);
                            mod_Solicitud.Fecha = Convert.ToString(item["Fecha"]);
                            mod_Solicitud.EstadoSolicitud = Convert.ToString(item["EstadoSolicitud"]);
                            mod_Solicitud.LineaNegocio = Convert.ToString(item["LineaNegocio"]);
                            mod_Solicitud.Usuario = Convert.ToString(item["Usuario"]);
                            mod_Solicitud.Observacion = Convert.ToString(item["Observacion"]);
                            
                            lst_retornoSol.Add(mod_Solicitud);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retornoSol);
                    }

                    //Solicitud en especifico
                    if (tipo == "2")
                    {
                        //Cabecera
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            mod_SolCabecera = new DMSolicitudArticulo.SolCabecera();
                            CodSAPprov =  Convert.ToString(item["CodProveedor"]);
                            mod_SolCabecera.CodProveedor = CodSAPprov;                            
                            mod_SolCabecera.RazonSocial = Convert.ToString(item["RazonSocial"]);
                            mod_SolCabecera.IdSolicitud = Convert.ToString(item["IdSolicitud"]);
                            mod_SolCabecera.TipoSolicitud = Convert.ToString(item["TipoSolicitud"]);
                            mod_SolCabecera.LineaNegocio = Convert.ToString(item["LineaNegocio"]);
                            mod_SolCabecera.Accion = Convert.ToString(item["Accion"]);
                            mod_SolCabecera.Estado = Convert.ToString(item["Estado"]);
                            mod_SolCabecera.Usuario = Convert.ToString(item["Usuario"]);
                            mod_SolCabecera.Ruc = Convert.ToString(item["Ruc"]);
                            mod_SolCabecera.Correo = Convert.ToString(item["Correo"]);
                            mod_SolCabecera.Telefono = Convert.ToString(item["Telefono"]);
                            mod_SolCabecera.Celular = Convert.ToString(item["Celular"]);
                            mod_SolCabecera.FecSolicitud = Convert.ToString(item["FecSolicitud"]);
                            mod_SolCabecera.CorreoProveedor = Convert.ToString(item["CorreoE"]);
                            mod_SolCabecera.PersonaContacto = Convert.ToString(item["PersonaContacto"]);
                            mod_SolCabecera.Responsable = Convert.ToString(item["Responsable"]);
                            mod_SolCabecera.Aprobador = Convert.ToString(item["Aprobador"]);
                            mod_SolCabecera.Departamento = Convert.ToString(item["Departamento"]);
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
                            mod_SolDetalle.Marca = Convert.ToString(item["Marca"]);
                            mod_SolDetalle.DesMarca = Convert.ToString(item["DesMarca"]);
                            mod_SolDetalle.MarcaNueva = Convert.ToString(item["MarcaNueva"]);
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
                            mod_SolDetalle.Observacion = Convert.ToString(item["Observacion"]);
                            mod_SolDetalle.Iva = Convert.ToString(item["Iva"]);
                            mod_SolDetalle.Deducible = Convert.ToString(item["Deducible"]);
                            mod_SolDetalle.Retencion = Convert.ToString(item["Retencion"]);
                            mod_SolDetalle.Accion = Convert.ToString(item["Accion"]);
                            mod_SolDetalle.CodSAPart = Convert.ToString(item["CodSapArticulo"]);
                            mod_SolDetalle.PrecioBruto = Convert.ToString(item["PrecioBruto"]).Replace(',', '.');
                            mod_SolDetalle.PrecioNuevo = Convert.ToString(item["PrecioNuevo"]).Replace(',', '.');
                            mod_SolDetalle.Descuento1 = Convert.ToString(item["Descuento1"]).Replace(',', '.');
                            mod_SolDetalle.Descuento2 = Convert.ToString(item["Descuento2"]).Replace(',', '.');
                            mod_SolDetalle.ImpVerde = Convert.ToBoolean(item["ImpVerde"]);
                            mod_SolDetalle.Coleccion = Convert.ToString(item["Coleccion"]);
                            mod_SolDetalle.Temporada = Convert.ToString(item["Temporada"]);
                            mod_SolDetalle.Estacion = Convert.ToString(item["Estacion"]);
                            mod_SolDetalle.CantidadPedir = Convert.ToString(item["Cantidad"]);
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
                            mod_SolMedidas.FactorCon = Convert.ToString(item["FactorCon"]).Replace(',', '.');
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
                            mod_SolMedidas.uniMedBase = Convert.ToBoolean(item["UnidadMedidaBase"]);
                            mod_SolMedidas.uniMedPedido = Convert.ToBoolean(item["UnidadMedidaPedido"]);
                            mod_SolMedidas.uniMedES = Convert.ToBoolean(item["UnidadMedidaES"]);
                            mod_SolMedidas.uniMedVenta = Convert.ToBoolean(item["UnidadMedidaVenta"]);                          
                            mod_SolMedidas.Estado = Convert.ToString(item["Estado"]);
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
                            mod_SolCodBarra.paisEan = Convert.ToString(item["PaisEan"]);
                            mod_SolCodBarra.paisDesEan = Convert.ToString(item["PaisDesEan"]);
                            mod_SolCodBarra.Principal = Convert.ToBoolean(item["Principal"]);
                            mod_SolCodBarra.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_CBa.Add(mod_SolCodBarra);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_CBa);
                        //Imagenes
                        foreach (DataRow item in ds.Tables[4].Rows)
                        {
                            mod_SolImagenes = new DMSolicitudArticulo.SolImagen();
                            mod_SolImagenes.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolImagenes.IdDocAdjunto = Convert.ToString(item["IdDocAdjunto"]);
                            mod_SolImagenes.NomArchivo = Convert.ToString(item["NomArchivo"]);
                            mod_SolImagenes.Path = Convert.ToString(item["Path"]);
                            mod_SolImagenes.Accion = Convert.ToString(item["Accion"]);
                            lst_retSol_Ima.Add(mod_SolImagenes);

                            //Descarga las imagenes del ftp y las crea en una carpeta temporal [idsol-idart]
                            BajaFptArchivo(codigo + "-" + Convert.ToInt32(item["IdDetalle"]), Convert.ToString(item["NomArchivo"]), "");

                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Ima);
                        //Rutas
                        foreach (DataRow item in ds.Tables[1].Rows) //Recorre el detalle y concatena el id de la solicitud
                        {
                            mod_SolRutas = new DMSolicitudArticulo.SolRutas();
                            mod_SolRutas.IdDetalle = Convert.ToString(item["IdDetalle"]);
                            mod_SolRutas.Path = codigo + "-" + Convert.ToInt32(item["IdDetalle"]);
                            lst_retSol_Rut.Add(mod_SolRutas);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Rut);
                        //Compras
                        foreach (DataRow item in ds.Tables[5].Rows)
                        {
                            mod_SolCompras = new DMSolicitudArticulo.SolCompras();
                            mod_SolCompras.IdDetalle = Convert.ToString(item["IdDetalle"]);                            
                            mod_SolCompras.CodLegacyProv = Convert.ToString(item["CodLegacyProv"]);
                            mod_SolCompras.OrganizacionCompras = Convert.ToString(item["OrganizacionCompras"]);
                            mod_SolCompras.OrganizacionComprasDes = Convert.ToString(item["OrganizacionComprasDes"]);
                            mod_SolCompras.FrecuenciaEntrega = Convert.ToString(item["FrecuenciaEntrega"]);
                            mod_SolCompras.TipoMaterial = Convert.ToString(item["TipoMaterial"]);
                            mod_SolCompras.TipoMaterialDes = Convert.ToString(item["TipoMaterialDes"]);
                            mod_SolCompras.CategoriaMaterial = Convert.ToString(item["CategoriaMaterial"]);
                            mod_SolCompras.CategoriaMaterialDes = Convert.ToString(item["CategoriaMaterialDes"]);
                            mod_SolCompras.GrupoArticulo = Convert.ToString(item["GrupoArticulo"]);
                            mod_SolCompras.SeccionArticulo = Convert.ToString(item["SeccionArticulo"]);
                            //mod_SolCompras.Catalogacion = Convert.ToString(item["Catalogacion"]);
                            mod_SolCompras.SurtidoParcial = Convert.ToString(item["SurtidoParcial"]);
                            mod_SolCompras.Materia = Convert.ToString(item["Materia"]);
                            mod_SolCompras.MateriaDes = Convert.ToString(item["MateriaDes"]);
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
                            //mod_SolCompras.CantidadPedir = Convert.ToString(item["Cantidad"]);
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
                            mod_SolCatalog.Canaldistribucion = Convert.ToString(item["CanalDistribucion"]);
                            mod_SolCatalog.DesCanaldistribucion = Convert.ToString(item["DesCanalDistribucion"]);
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
                            mod_SolAlmacen.DesAlmacen = Convert.ToString(item["DesAlmacen"]);

                            mod_SolAlmacen.TipAlmacen = Convert.ToString(item["TipoAlmacen"]);
                            mod_SolAlmacen.DestipAlmacen = Convert.ToString(item["DestipAlmacen"]);
                            mod_SolAlmacen.IndAlmacenE = Convert.ToString(item["IndTipoAlmacenE"]);
                            mod_SolAlmacen.DesindAlmacenE = Convert.ToString(item["DesindAlmacenE"]);
                            mod_SolAlmacen.IndAlmacenS = Convert.ToString(item["IndTipoAlmacenS"]);
                            mod_SolAlmacen.DesindAlmacenS = Convert.ToString(item["DesindAlmacenS"]);
                            mod_SolAlmacen.IndAreaAlmNew = Convert.ToString(item["IndAreaAlmacenamiento"]);
                            mod_SolAlmacen.DesIndAreaAlmNew = Convert.ToString(item["DesindAreaAlmNew"]);

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

                        //Observaciones
                        foreach (DataRow item in ds.Tables[11].Rows)
                        {
                            mod_SolObserv = new DMSolicitudArticulo.SolObserv();
                            mod_SolObserv.Fecha = Convert.ToString(item["Fecha"]);
                            mod_SolObserv.EstadoSolicitud = Convert.ToString(item["EstadoSolicitud"]);
                            mod_SolObserv.Usuario = Convert.ToString(item["Usuario"]);
                            mod_SolObserv.Motivo = Convert.ToString(item["Motivo"]);
                            mod_SolObserv.Observacion = Convert.ToString(item["Observacion"]);
                            lst_retSol_Obs.Add(mod_SolObserv);
                        }
                        FormResponse.success = true;
                        FormResponse.root.Add(lst_retSol_Obs);

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

                       

                        //Consulta Cod Legacy Prov
                        if (CodProveedorCons == null)
                        {
                            lst_codLegacy = ConsultaCodLegacyProv(CodSAPprov);
                            FormResponse.root.Add(lst_codLegacy);
                        }

                        //Caracteristicas
                        foreach (DataRow item in ds.Tables[13].Rows)
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
            catch (Exception ex)
            { }

            //Si es modificacion y existe codigo SAP se consulta datos del articulo 
            //para presentar datos de la nueva solicitud y datos del actual articulo actual
            /*
            var codSapArt = ",";
            foreach (var i in lst_retSol_Det)
            {
                codSapArt = codSapArt + i.CodSAPart + ",";
            }
            if (codSapArt != ",")
            {
                codSapArt = codSapArt.Substring(0, codSapArt.Length - 1);
                Art_ConsultaController consultaArticulo = new Art_ConsultaController();
                FormResponseArticulo FormResponseConsArticulo = new FormResponseArticulo();
                FormResponseConsArticulo = consultaArticulo.GetConsArticulo("2", codSapArt, null, null, null, null, null, null, null, null, "0", CodProveedorCons);
                FormResponse.root2 = FormResponseConsArticulo.root;
            }
            */

            return FormResponse;
        }

        [ActionName("GrabaSolArticulo")]
        [HttpPost]
        public FormResponseArticulo GetGrabaSolicitud(DMSolicitudArticulo SolArticulo)
        {
            FormResponseArticulo retorno = new FormResponseArticulo();
            retorno.success = false;
            retorno.mensaje = "";
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            
            String IdTipoSolicitud = "";
            xmlParam.LoadXml("<Root />");
            string usrSesion = SolArticulo.p_SolCabecera[0].Usuario;
            try
            {
                //Datos de la cabecera
                if (SolArticulo.p_SolCabecera != null)
                {
                    foreach (DMSolicitudArticulo.SolCabecera dr in SolArticulo.p_SolCabecera)
                    {
                        xmlParam.DocumentElement.SetAttribute("accion", dr.Accion);
                       
                        xmlParam.DocumentElement.SetAttribute("IdSolicitud", dr.IdSolicitud);
                        xmlParam.DocumentElement.SetAttribute("TipoSolicitud", dr.TipoSolicitud);
                        IdTipoSolicitud =  dr.TipoSolicitud;                       
                        xmlParam.DocumentElement.SetAttribute("LineaNegocio", dr.LineaNegocio);
                        xmlParam.DocumentElement.SetAttribute("CodProveedor", dr.CodProveedor);
                        xmlParam.DocumentElement.SetAttribute("EstadoSolicitud", dr.Estado);
                        xmlParam.DocumentElement.SetAttribute("Usuario", dr.Usuario);
                        xmlParam.DocumentElement.SetAttribute("Observacion", dr.Observacion);
                        xmlParam.DocumentElement.SetAttribute("MotivoRechazo", dr.MotivoRechazo);
                    }
                }
                //Datos del detalle
                if (SolArticulo.p_SolDetalle != null)
                {
                    foreach (DMSolicitudArticulo.SolDetalle dr in SolArticulo.p_SolDetalle)
                    {
                        XmlElement elem = xmlParam.CreateElement("Detalle");
                        elem.SetAttribute("IdDetalle", dr.IdDetalle);
                        elem.SetAttribute("CodReferencia", dr.CodReferencia);
                        elem.SetAttribute("CodSapArticulo", dr.CodSAPart);
                        
                       
                        elem.SetAttribute("Marca", dr.Marca);
                        elem.SetAttribute("MarcaNueva", dr.MarcaNueva);
                        elem.SetAttribute("Descripcion", dr.Descripcion);
                        elem.SetAttribute("TextoBreve", dr.OtroId);
                        elem.SetAttribute("PaisOrigen", dr.PaisOrigen);
                        elem.SetAttribute("RegionOrigen", dr.RegionOrigen);
                        elem.SetAttribute("TamArticulo", dr.TamArticulo);
                        elem.SetAttribute("GradoAlcohol", dr.GradoAlcohol);
                        elem.SetAttribute("Talla", dr.Talla);
                        elem.SetAttribute("Color", dr.Color);
                        elem.SetAttribute("Fragancia", dr.Fragancia);
                        elem.SetAttribute("Tipos", dr.Tipos);
                        elem.SetAttribute("Sabor", dr.Sabor);
                        elem.SetAttribute("Modelo", dr.Modelo);
                        elem.SetAttribute("Coleccion", dr.Coleccion);
                        elem.SetAttribute("Estacion", dr.Estacion);
                        elem.SetAttribute("Temporada", dr.Temporada);
                        elem.SetAttribute("CantidadPedir", dr.CantidadPedir);
                        elem.SetAttribute("Iva", dr.Iva);
                        elem.SetAttribute("Deducible", dr.Deducible);
                        elem.SetAttribute("Retencion", dr.Retencion);
                        elem.SetAttribute("EstadoSolicitud", SolArticulo.p_SolCabecera[0].Estado);
                        elem.SetAttribute("EstadoArticulo", dr.Estado);
                        elem.SetAttribute("PrecioBruto", dr.PrecioBruto);
                        elem.SetAttribute("PrecioNuevo", dr.PrecioNuevo != null?dr.PrecioNuevo:"0");
                        elem.SetAttribute("Descuento1", dr.Descuento1);
                        elem.SetAttribute("Descuento2", dr.Descuento2);                        
                        elem.SetAttribute("ImpuestoVerde", (dr.ImpVerde ? "1" : "0"));
                        elem.SetAttribute("Variante", (dr.IsVariante ? "1" : "0"));
                        elem.SetAttribute("Generico", (dr.IsGenerico ? "1" : "0"));
                        elem.SetAttribute("CodGenerico", (dr.CodGenerico != null ? dr.CodGenerico : ""));
                        elem.SetAttribute("accion", dr.Accion);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                //Datos de las Medidas
                if (SolArticulo.p_SolMedida != null)
                {
                    foreach (DMSolicitudArticulo.SolMedida dr in SolArticulo.p_SolMedida)
                    {

                        var f = SolArticulo.p_SolDetalle.Where(x => x.IdDetalle == dr.IdDetalle).ToList();
                        if (f.Count > 0)
                        {                       
                        
                        XmlElement elem = xmlParam.CreateElement("Medidas");
                        elem.SetAttribute("IdDetalle", dr.IdDetalle);
                        elem.SetAttribute("UnidadMedida", dr.UnidadMedida);
                        elem.SetAttribute("TipoUnidMedida", dr.TipoUnidadMedida);
                        elem.SetAttribute("FactorConversion", dr.FactorCon);
                        elem.SetAttribute("UniMedConversion", dr.UniMedConvers);
                        elem.SetAttribute("PesoNeto", dr.PesoNeto);
                        elem.SetAttribute("PesoBruto", dr.PesoBruto);
                        elem.SetAttribute("Longitud", dr.Longitud);
                        elem.SetAttribute("Ancho", dr.Ancho);
                        elem.SetAttribute("Altura", dr.Altura);
                        elem.SetAttribute("Volumen", dr.Volumen);
                        elem.SetAttribute("UniMedidaVolumen", dr.UniMedidaVolumen);
                        elem.SetAttribute("PrecioBruto", dr.PrecioBruto);
                        elem.SetAttribute("Descuento1", dr.Descuento1);
                        elem.SetAttribute("Descuento2", dr.Descuento2);
                        elem.SetAttribute("uniMedBase", (dr.uniMedBase ? "1" : "0"));
                        elem.SetAttribute("uniMedES", (dr.uniMedES ? "1" : "0"));
                        elem.SetAttribute("uniMedPedido", (dr.uniMedPedido ? "1" : "0"));
                        elem.SetAttribute("uniMedVenta", (dr.uniMedVenta ? "1" : "0")); 
                        elem.SetAttribute("ImpuestoVerde", (dr.ImpVerde ? "1" : "0"));
                        elem.SetAttribute("Estado", dr.Estado);                       
                        elem.SetAttribute("accion", dr.Accion);
                        xmlParam.DocumentElement.AppendChild(elem);
                        }
                    }
                }
                //Datos de los Cod. de Barra
                if (SolArticulo.p_SolCodigoBarra != null)
                {
                    foreach (DMSolicitudArticulo.SolCodigoBarra dr in SolArticulo.p_SolCodigoBarra)
                    {
                         var f = SolArticulo.p_SolDetalle.Where(x => x.IdDetalle == dr.IdDetalle).ToList();
                         if (f.Count > 0)
                         {
                             XmlElement elem = xmlParam.CreateElement("CodBarras");
                             elem.SetAttribute("IdDetalle", dr.IdDetalle);
                             elem.SetAttribute("UnidadMedida", dr.UnidadMedida);
                             elem.SetAttribute("TipoEAN", dr.TipoEan);
                             elem.SetAttribute("DescripcionEAN", dr.DescripcionEan);
                             elem.SetAttribute("PaisEAN", dr.paisEan);
                             elem.SetAttribute("PaisDesEAN", dr.paisDesEan);
                             elem.SetAttribute("EAN", dr.NumeroEan);

                             elem.SetAttribute("Principal", (dr.Principal ? "1" : "0"));
                             elem.SetAttribute("accion", dr.Accion);
                             xmlParam.DocumentElement.AppendChild(elem);
                         }
                    }
                }
                //Datos de las Imagenes
                if (SolArticulo.p_SolImagen != null)
                {
                    foreach (DMSolicitudArticulo.SolImagen dr in SolArticulo.p_SolImagen)
                    {
                         var f = SolArticulo.p_SolDetalle.Where(x => x.IdDetalle == dr.IdDetalle).ToList();
                         if (f.Count > 0)
                         {
                             XmlElement elem = xmlParam.CreateElement("Imagenes");
                             elem.SetAttribute("IdDetalle", dr.IdDetalle);
                             elem.SetAttribute("NomArchivo", dr.NomArchivo);
                             elem.SetAttribute("accion", dr.Accion);
                             xmlParam.DocumentElement.AppendChild(elem);
                         }
                    }
                }
                //Datos de Compras
                if (SolArticulo.p_SolCompras != null)
                {
                    foreach (DMSolicitudArticulo.SolCompras dr in SolArticulo.p_SolCompras)
                    {
                        XmlElement elem = xmlParam.CreateElement("Compras");
                        elem.SetAttribute("IdDetalle", dr.IdDetalle);
                        elem.SetAttribute("CodLegacyProv", dr.CodLegacyProv);
                        
                        elem.SetAttribute("OrganizacionCompras", dr.OrganizacionCompras);
                        elem.SetAttribute("FrecuenciaEntrega", dr.FrecuenciaEntrega);
                        elem.SetAttribute("TipoMaterial", dr.TipoMaterial);
                        elem.SetAttribute("CategoriaMaterial", dr.CategoriaMaterial);
                        elem.SetAttribute("GrupoArticulo", dr.GrupoArticulo);
                        elem.SetAttribute("SeccionArticulo", dr.SeccionArticulo);
                        //elem.SetAttribute("Catalogacion", dr.Catalogacion);
                        elem.SetAttribute("SurtidoParcial", dr.SurtidoParcial);
                        elem.SetAttribute("Materia", dr.Materia);
                        elem.SetAttribute("CostoFOB", dr.CostoFOB);
                        elem.SetAttribute("IndPedido", dr.IndPedido);
                        elem.SetAttribute("PerfilDistribucion", dr.PerfilDistribucion);
                        //elem.SetAttribute("Almacen", dr.Almacen);
                        elem.SetAttribute("GrupoCompra", dr.GrupoCompra);
                        elem.SetAttribute("CategoriaValoracion", dr.CategoriaValoracion);
                        elem.SetAttribute("TipoAlamcen", dr.TipoAlamcen);
                        //elem.SetAttribute("IndAlmaEntrada", dr.IndAlmaEntrada);
                        //elem.SetAttribute("IndAlmaSalida", dr.IndAlmaSalida);
                        //elem.SetAttribute("IndAreaAlmacen", dr.IndAreaAlmacen);
                        elem.SetAttribute("CondicionAlmacen", dr.CondicionAlmacen);
                        elem.SetAttribute("ClListaSurtido", dr.ClListaSurtido);
                        elem.SetAttribute("EstatusMaterial", dr.EstatusMaterial);
                        elem.SetAttribute("EstatusVenta", dr.EstatusVenta);
                        elem.SetAttribute("ValidoDesde", dr.ValidoDesde);
                        elem.SetAttribute("GrupoBalanzas", dr.GrupoBalanzas);
                        elem.SetAttribute("Observacion", dr.Observacion);
                        //elem.SetAttribute("Coleccion", dr.Coleccion);
                        //elem.SetAttribute("Temporada", dr.Temporada);
                        //elem.SetAttribute("Estacion", dr.Estacion);
                        //elem.SetAttribute("CantidadPedir", dr.CantidadPedir);
                        elem.SetAttribute("JerarquiaProd", dr.JerarquiaProd);//Nuevos
                        elem.SetAttribute("SusceptBonifEsp", dr.SusceptBonifEsp);
                        elem.SetAttribute("ProcedimCatalog", dr.ProcedimCatalog);
                        elem.SetAttribute("CaracterPlanNec", dr.CaracterPlanNec);
                        elem.SetAttribute("FuenteProvision", dr.FuenteProvision);//Nuevos
                        elem.SetAttribute("MotivoRechazo", dr.MotivoRechazo);
                        elem.SetAttribute("EstadoArticulo", dr.Estado);
                        elem.SetAttribute("accion", "U");
                        var Cab = SolArticulo.p_SolDetalle.Where(x => x.IdDetalle == dr.IdDetalle).FirstOrDefault();
                        if(Cab.Accion != "D")
                            xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                //Datos de Catalogacion
                if (SolArticulo.p_SolCatalogacion != null)
                {
                    foreach (DMSolicitudArticulo.SolCatalogacion dr in SolArticulo.p_SolCatalogacion)
                    {
                        XmlElement elem = xmlParam.CreateElement("Catalogacion");
                        elem.SetAttribute("IdDetalle", dr.IdDetalle);
                        elem.SetAttribute("Catalogacion", dr.Catalogacion);
                        elem.SetAttribute("Canaldistribucion", dr.Canaldistribucion);
                        //elem.SetAttribute("DesCanaldistribucion", dr.DesCanaldistribucion); 
                        elem.SetAttribute("accion", dr.Accion);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }

                //Datos de Centros
                if (SolArticulo.p_SolCentros != null)
                {
                    foreach (DMSolicitudArticulo.SolCentros dr in SolArticulo.p_SolCentros)
                    {
                        XmlElement elem = xmlParam.CreateElement("Centros");
                        elem.SetAttribute("IdDetalle", dr.IdDetalle);
                        elem.SetAttribute("Centro", dr.Centros);
                        elem.SetAttribute("PerfilDistribucion", dr.PerfilDistribucion);                        
                        elem.SetAttribute("accion", dr.Accion);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                //Datos de Almacen
                if (SolArticulo.p_SolAlmacen != null)
                {
                    foreach (DMSolicitudArticulo.SolAlmacen dr in SolArticulo.p_SolAlmacen)
                    {
                        XmlElement elem = xmlParam.CreateElement("Almacen");
                        elem.SetAttribute("IdDetalle", dr.IdDetalle);
                        elem.SetAttribute("Almacen", dr.Almacen);
                        elem.SetAttribute("TipoAlmacen", dr.TipAlmacen);
                        elem.SetAttribute("IndTipoAlmacenE", dr.IndAlmacenE);
                        elem.SetAttribute("IndTipoAlmacenS", dr.IndAlmacenS);                        
                        elem.SetAttribute("IndTipoAreaAlmNew", dr.IndAreaAlmNew);
                        elem.SetAttribute("accion", dr.Accion);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                //Datos de IndTipoAlmEnt
                if (SolArticulo.p_SolIndTipoAlmEnt != null)
                {
                    foreach (DMSolicitudArticulo.SolIndTipoAlmEnt dr in SolArticulo.p_SolIndTipoAlmEnt)
                    {
                        XmlElement elem = xmlParam.CreateElement("IndTipoAlmEnt");
                        elem.SetAttribute("IdDetalle", dr.IdDetalle);
                        elem.SetAttribute("IndTipoAlmEnt", dr.IndTipoAlmEnt);                        
                        elem.SetAttribute("accion", dr.Accion);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                //Datos de IndTipoAlmSal
                if (SolArticulo.p_SolIndTipoAlmSal != null)
                {
                    foreach (DMSolicitudArticulo.SolIndTipoAlmSal dr in SolArticulo.p_SolIndTipoAlmSal)
                    {
                        XmlElement elem = xmlParam.CreateElement("IndTipoAlmSal");
                        elem.SetAttribute("IdDetalle", dr.IdDetalle);
                        elem.SetAttribute("IndTipoAlmSal", dr.IndTipoAlmSal);                        
                        elem.SetAttribute("accion", dr.Accion);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }
                //Datos de IndAreaAlmacen
                if (SolArticulo.p_SolIndAreaAlmacen != null)
                {
                    foreach (DMSolicitudArticulo.SolIndAreaAlmacen dr in SolArticulo.p_SolIndAreaAlmacen)
                    {
                        XmlElement elem = xmlParam.CreateElement("IndAreaAlmacen");
                        elem.SetAttribute("IdDetalle", dr.IdDetalle);
                        elem.SetAttribute("IndAreaAlmacen", dr.IndAreaAlmacen);
                        elem.SetAttribute("GrupoBalanzas", dr.GrupoBalanzas);                        
                        elem.SetAttribute("accion", dr.Accion);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                
                }

                //Datos de Caracteristicas
                if (SolArticulo.p_SolCaracteristicas != null)
                {
                    foreach (DMSolicitudArticulo.SolCaracteristicas dr in SolArticulo.p_SolCaracteristicas)
                    {
                        XmlElement elem = xmlParam.CreateElement("Caracteristicas");
                        elem.SetAttribute("IdDetalle", dr.IdDetalle);
                        elem.SetAttribute("IdCaracteristica", dr.IdCaract);
                        elem.SetAttribute("Valor", dr.IdValor);
                        elem.SetAttribute("Agrupacion", dr.IdAgrupacion); 
                        elem.SetAttribute("accion", dr.Accion);
                        xmlParam.DocumentElement.AppendChild(elem);
                    }
                }

               

                //Enviar a registrar  artículos si solicitud es aprobado por Datos Maestros
                if (SolArticulo.p_SolCabecera[0].Estado == "AD")
                {


                    var workBapi = AppConfig.WorkBAPI;
                    //var workBapi = "N";
                    if (workBapi == "S")
                    //Consumo de BAPI
                    {

                        //Grabar datos de la solicitud ante de enviar a SAP.

                       
                       
                        XmlNodeList nodeList = xmlParam.SelectNodes("//Root");
                         foreach (XmlNode node in nodeList)
                         { node.Attributes["EstadoSolicitud"].Value = "ED";
                         }
                         var newXML2 = xmlParam.OuterXml.ToString().Replace(',', '¥');
                         xmlParam.LoadXml(newXML2);
                         ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 102, 1); //Articulo.Sol_P_Grabar	102	
                        if (!ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                        {
                            //ERROR AL LLAMAR SP
                           
                            registraLogError(ds.Tables["TblEstado"].Rows[0]["CodError"].ToString(), ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString(), "Artículo", "102", xmlParam.OuterXml.ToString(), usrSesion);
                            retorno.success = false;
                            retorno.mensaje = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                          
                        }
                        else
                        {

                            if (SolArticulo.p_SolCabecera[0].TipoSolicitud == "1")
                            {
                                retorno = CreaArticuloBapi(SolArticulo, SolArticulo.p_SolCabecera[0].CodProveedor);
                                if (!retorno.success)
                                    return retorno;
                                else
                                {
                                    XmlNodeList nodeList2 = xmlParam.SelectNodes("//Root");
                                    foreach (XmlNode node in nodeList2)
                                    {
                                        node.Attributes["EstadoSolicitud"].Value = "AD";
                                    }
                                }
                                
                            }
                            else
                            {
                                retorno = ActualizaArticuloBapi(SolArticulo, SolArticulo.p_SolCabecera[0].CodProveedor);
                                if (!retorno.success)
                                    return retorno;
                                else
                                {
                                    XmlNodeList nodeList2 = xmlParam.SelectNodes("//Root");
                                    foreach (XmlNode node in nodeList2)
                                    {
                                        node.Attributes["EstadoSolicitud"].Value = "AD";
                                    }
                                }
                            }
                        }
                    }
                    else
                    //crea articulo simular BAPI
                    {
                        CreaArticulo(SolArticulo);
                    }
                    //SolArticulo

                }



                
               
                var newXML = xmlParam.OuterXml.ToString().Replace(',', '¥');
                xmlParam.LoadXml(newXML);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 102, 1); //Articulo.Sol_P_Grabar	102	

                if (!ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    //ERROR AL LLAMAR SP
                    //Registrar log
                    registraLogError(ds.Tables["TblEstado"].Rows[0]["CodError"].ToString(), ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString(), "Artículo", "102", xmlParam.OuterXml.ToString(), usrSesion);
                    retorno.success = false;
                    retorno.mensaje = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    //throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                }
                else
                {
                    
                    
                    //TRANSACCION OK - errores de EMAIL Y SFTP se pasa por alto
                    retorno.success = true;
                    retorno.mensaje = "OK";
                    //Obtener datos de proveedor
                    List<Proveedor> lst_retornoSol2 = new List<Proveedor>();
                    DataSet ds2 = new DataSet();
                    ClsGeneral objEjecucion2 = new ClsGeneral();
                    String CodProveedorSAP = "";
                    String RucSolArticulo = "";
                    String RazonSocialSolArticulo = "";
                    String CorreoSolArt = "";
                    String MotivoSolArt = "";
                    try
                    {

                        var codSolicitudRetorna = ds.Tables[0].Rows[0]["IdSolicitud"].ToString();
                        if (codSolicitudRetorna != "")
                        {
                            SolArticulo.p_SolCabecera[0].IdSolicitud = codSolicitudRetorna;
                        }
                        var wresulFactList =
                                  new System.Xml.Linq.XDocument(
                                   new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                   new XElement("Root",
                                            new XElement("SecNotificacion",
                                           new XAttribute("CodigoSol", SolArticulo.p_SolCabecera[0].IdSolicitud != null ? SolArticulo.p_SolCabecera[0].IdSolicitud : ""))));
                        ds2 = objEjecucion2.EjecucionGralDs(wresulFactList.ToString(), 404, 1);



                        if (ds2.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                        {
                            lst_retornoSol2 = (from reg in ds2.Tables[0].AsEnumerable()
                                               select new Proveedor
                                               {
                                                   CodProveedor = reg.Field<String>("CodProveedor"),
                                                   RazonSocial = reg.Field<String>("NomComercial"),
                                                   Representante = reg.Field<String>("Ruc"),
                                                   Correo = reg.Field<String>("CorreoE"),
                                                   Motivo = reg.Field<String>("Motivo"),

                                               }).ToList<Proveedor>();

                        }

                        CodProveedorSAP = lst_retornoSol2[0].CodProveedor;
                        RucSolArticulo = lst_retornoSol2[0].Representante;
                        RazonSocialSolArticulo = lst_retornoSol2[0].RazonSocial;
                        CorreoSolArt = lst_retornoSol2[0].Correo;
                        MotivoSolArt = lst_retornoSol2[0].Motivo;

                    }
                    catch (Exception e)
                    {
                        
                    }


                    //ENVIO DE EMAIL 
                    try
                    {

                        if (SolArticulo.p_SolCabecera != null && SolArticulo.p_SolCabecera[0].EnviaNotificacion == "S")
                        {

                            switch (SolArticulo.p_SolCabecera[0].Estado)
                            {

                                case "EN":
                                case "DG":
                                case "DD":
                                    //DATOS MAESTROS
                                    String obsr = SolArticulo.p_SolCabecera[0].Observacion;
                                    String mtvRechazo = MotivoSolArt;
                                    if (SolArticulo.p_SolCabecera[0].Estado == "EN")
                                    {
                                        if (IdTipoSolicitud == "2")
                                        {
                                            mtvRechazo = "Solicitud de Modificación Artículo.";
                                            obsr = "Revisar Solicitud de Modificación Artículo.";
                                        }
                                        if (IdTipoSolicitud == "1")
                                        {
                                            mtvRechazo = "Solicitud de Nuevo Artículo.";
                                            obsr = "Revisar Solicitud de Nuevo Artículo.";
                                        }
                                        if (IdTipoSolicitud == "5")
                                        {
                                            mtvRechazo = "Solicitud de Cambio de Precio Artículo.";
                                            obsr = "Revisar Solicitud de Cambio de Precio Artículo.";
                                        }
                                    }
                                    this.Notificacion22("APR", "ART", SolArticulo.p_SolCabecera[0].LineaNegocio, RazonSocialSolArticulo, RucSolArticulo, false, null, null,
                                        obsr, mtvRechazo, SolArticulo.p_SolCabecera[0].IdSolicitud, IdTipoSolicitud);
                                    break;
                                case "RC":
                                    //GERENTE DE COMPRAS
                                    this.Notificacion22("APG", "ART", null, RazonSocialSolArticulo, RucSolArticulo, false, null, null,
                                        SolArticulo.p_SolCabecera[0].Observacion, MotivoSolArt, SolArticulo.p_SolCabecera[0].IdSolicitud, IdTipoSolicitud);
                                    break;
                                case "AG":
                                    //DATOS MAESTROS
                                    this.Notificacion22("APM", "ART", null, RazonSocialSolArticulo, RucSolArticulo, false, null, null,
                                        SolArticulo.p_SolCabecera[0].Observacion, MotivoSolArt, SolArticulo.p_SolCabecera[0].IdSolicitud, IdTipoSolicitud);

                                    break;
                                case "DC":
                                case "AD":
                                case "RE":
                                    String EstadoSol = "";
                                    if (SolArticulo.p_SolCabecera[0].Estado == "RE")
                                        EstadoSol = "Rechazado";
                                    if (SolArticulo.p_SolCabecera[0].Estado == "DC")
                                        EstadoSol = "Devuelto";
                                    if (SolArticulo.p_SolCabecera[0].Estado == "AD")
                                        EstadoSol = "Aprobado";


                                    this.Notificacion22("APM", "ART", null, RazonSocialSolArticulo, RucSolArticulo, true, CorreoSolArt, EstadoSol, null, null, SolArticulo.p_SolCabecera[0].IdSolicitud, IdTipoSolicitud);
                                    break;

                            }


                        }

                    }
                    catch (Exception)
                    {


                    }

                    //CARGA SFTP
                    try
                    {

                        string idsolicitud = ds.Tables[0].Rows[0]["IdSolicitud"].ToString();
                        foreach (DMSolicitudArticulo.SolImagen dr in SolArticulo.p_SolImagen)
                        {
                            
                          if(dr.Accion != "D")
                          {
                            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + dr.Path), dr.NomArchivo);
                            if (File.Exists(fileSavePath))
                            {
                                using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp, Convert.ToInt32(AppConfig.SftpServerPort), AppConfig.SftpServerUserName, AppConfig.SftpServerPassword))
                                {
                                    sftpClient.Connect();
                                    if (!sftpClient.Exists(AppConfig.SftpPath + "Articulo/" + idsolicitud + "-" + dr.IdDetalle))
                                    {
                                        sftpClient.CreateDirectory(AppConfig.SftpPath + "Articulo/" + idsolicitud + "-" + dr.IdDetalle);
                                    }
                                    Stream fin = File.OpenRead(fileSavePath);
                                    sftpClient.UploadFile(fin, AppConfig.SftpPath + "Articulo/" + idsolicitud + "-" + dr.IdDetalle + "/" + dr.NomArchivo, true);
                                    fin.Close();
                                    sftpClient.Disconnect();
                                }
                            }
                          }
                        }
                    }
                    catch (Exception)
                    { }
                }
            }
            catch (Exception ex)
            {

                registraLogError("999", ex.Message.ToString(), "Artículo", "0", "", usrSesion);
                retorno.success = false;
                retorno.mensaje = ex.Message.ToString();

            }
            return retorno;
        }

       
       

        /// <summary>
        /// Retorna el path de la ruta temporal donde esta el archivo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [ActionName("BajaTempArchivo")]
        [HttpGet]
        public string BajaTempArchivo(string path, string archivo)
        {
            String result = null;
            try
            {
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path), archivo);

                var localFilePath = fileSavePath;

                if (File.Exists(localFilePath))
                {
                    result =  "UploadedDocuments/" + path + "/" + archivo;
                }
            }
            catch (Exception ex)
            { }
            return result;
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

        [ActionName("CargaMasiva")]
        [HttpGet]
        public FormResponseArticulo getCargaMasiva(string pathmasivo, string nomarchivo, string aux1, string aux2)
        {
            //********************************************
            //Detalle
            List<DMSolicitudArticulo.SolDetalle> lst_retSol_Det = new List<DMSolicitudArticulo.SolDetalle>();
            DMSolicitudArticulo.SolDetalle mod_SolDetalle;
            //Medidas
            List<DMSolicitudArticulo.SolMedida> lst_retSol_Med = new List<DMSolicitudArticulo.SolMedida>();
            DMSolicitudArticulo.SolMedida mod_SolMedidas;
            //Codigo Barra
            List<DMSolicitudArticulo.SolCodigoBarra> lst_retSol_CBa = new List<DMSolicitudArticulo.SolCodigoBarra>();
            DMSolicitudArticulo.SolCodigoBarra mod_SolCodBarra;
            //********************************************

            System.Data.OleDb.OleDbConnection l_cnn = new System.Data.OleDb.OleDbConnection();
            System.Data.OleDb.OleDbCommand l_cmd = new System.Data.OleDb.OleDbCommand();
            System.Data.OleDb.OleDbDataAdapter l_da = new System.Data.OleDb.OleDbDataAdapter();
            DataSet dsdet = new DataSet();
            DataSet dsmed = new DataSet();
            DataSet dscbr = new DataSet();

            List<object> logcarga = new List<object>();

            System.Text.StringBuilder sbTxtPed = new System.Text.StringBuilder("");
            string l_comando;
            string l_nomhoja;
            string l_nomcol;
            Decimal result;
            FormResponseArticulo FormResponse = new FormResponseArticulo();
            try
            {
                l_cnn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + pathmasivo), nomarchivo) + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
                l_cnn.Open();

                //Detalle
                l_nomhoja = l_cnn.GetSchema("Tables").Rows[1]["TABLE_NAME"].ToString(); //Detalle - [1]: Ordenado Alfabeticamente
                if (l_nomhoja != "Detalle$")
                {
                    l_cnn.Close();
                    FormResponse.success = false;
                    FormResponse.mensaje = "La hoja <Detalle> no existe. Por favor use la Plantilla.xls";
                    return FormResponse;
                }
                l_comando = "SELECT * FROM [Detalle$]";
                l_cmd.CommandText = l_comando;
                l_cmd.Connection = l_cnn;
                l_da.SelectCommand = l_cmd;
                l_da.Fill(dsdet);

                //Medidas
                l_nomhoja = l_cnn.GetSchema("Tables").Rows[2]["TABLE_NAME"].ToString(); //Medida - [2]: Ordenado Alfabeticamente
                if (l_nomhoja != "Medida$")
                {
                    l_cnn.Close();
                    FormResponse.success = false;
                    FormResponse.mensaje = "La hoja <Medida> no existe. Por favor use la Plantilla.xls";
                    return FormResponse;
                }
                l_comando = "SELECT * FROM [Medida$]";
                l_cmd.CommandText = l_comando;
                l_cmd.Connection = l_cnn;
                l_da.SelectCommand = l_cmd;
                l_da.Fill(dsmed);

                //Codigo de Barra
                l_nomhoja = l_cnn.GetSchema("Tables").Rows[0]["TABLE_NAME"].ToString(); //CodBarra - [0]: Ordenado Alfabeticamente
                if (l_nomhoja != "CodBarra$")
                {
                    l_cnn.Close();
                    FormResponse.success = false;
                    FormResponse.mensaje = "La hoja <CodBarra> no existe. Por favor use la Plantilla.xls";
                    return FormResponse;
                }
                l_comando = "SELECT * FROM [CodBarra$]";
                l_cmd.CommandText = l_comando;
                l_cmd.Connection = l_cnn;
                l_da.SelectCommand = l_cmd;
                l_da.Fill(dscbr);

                l_cnn.Close();

                //Detalle
                int contdet = 1;
                int filadet = 1;
                foreach (DataRow item in dsdet.Tables[0].Rows)
                {
                    //Valida tamaño registro y vacios
                    bool agregadet = true;

                    if (Convert.ToString(item["CodReferencia"]).Length > 30)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna CodReferencia excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (Convert.ToString(item["CodReferencia"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene CodReferencia o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["Marca"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Marca excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (Convert.ToString(item["MarcaNueva"]).Trim() == "" && Convert.ToString(item["Marca"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Marca o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["MarcaNueva"]).Length > 50)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna MarcaNueva excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (Convert.ToString(item["MarcaNueva"]).Trim() == "" && Convert.ToString(item["Marca"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene MarcaNueva o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["PaisOrigen"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna PaisOrigen excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                    }
                    if (Convert.ToString(item["PaisOrigen"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene PaisOrigen o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["RegionOrigen"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna RegionOrigen excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (Convert.ToString(item["RegionOrigen"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene RegionOrigen o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["TamanioArticulo"]).Length > 50)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna TamanioArticulo excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (Convert.ToString(item["TamanioArticulo"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene TamanioArticulo o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["GradoAlcohol"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna GradoAlcohol excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    //if (Convert.ToString(item["GradoAlcohol"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene GradoAlcohol o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}

                    //if (Convert.ToString(item["Talla"]).Length > 10)
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Talla excede el tamaño permitido de digitos.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}
                    //if (Convert.ToString(item["Talla"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Talla o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}

                    //if (Convert.ToString(item["Color"]).Length > 10)
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Color excede el tamaño permitido de digitos.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}
                    //if (Convert.ToString(item["Color"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Color o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}

                    //if (Convert.ToString(item["Fragancia"]).Length > 10)
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Fragancia excede el tamaño permitido de digitos.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}
                    //if (Convert.ToString(item["Fragancia"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Fragancia o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}

                    //if (Convert.ToString(item["Tipos"]).Length > 10)
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Tipos excede el tamaño permitido de digitos.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}
                    //if (Convert.ToString(item["Tipos"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Tipos o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}

                    //if (Convert.ToString(item["Sabor"]).Length > 10)
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Sabor excede el tamaño permitido de digitos.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}
                    //if (Convert.ToString(item["Sabor"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Sabor o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}


                    if (Convert.ToString(item["PrecioBruto"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene PrecioBruto o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (!Decimal.TryParse(Convert.ToString(item["PrecioBruto"]), out result))
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna PrecioBruto no es numérica o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["Descuento1"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Descuento1 o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (!Decimal.TryParse(Convert.ToString(item["Descuento1"]), out result))
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna FactorCon no es numérica o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["Descuento2"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Descuento2 o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (!Decimal.TryParse(Convert.ToString(item["Descuento2"]), out result))
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Descuento2 no es numérica o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["ImpVerde"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene ImpVerde o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }


                    if (Convert.ToString(item["Modelo"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Modelo excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (Convert.ToString(item["Modelo"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Modelo o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["Descripcion"]).Length > 100)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Descripcion excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (Convert.ToString(item["Descripcion"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Descripcion o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["TextoBreve"]).Length > 50)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna TextoBreve excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    //if (Convert.ToString(item["TextoBreve"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene TextoBreve o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregadet = false;
                    //}

                    if (Convert.ToString(item["Iva"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Iva excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (Convert.ToString(item["Iva"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Iva o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["Deducible"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Deducible excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (Convert.ToString(item["Deducible"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Deducible o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    if (Convert.ToString(item["Retencion"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". La columna Retencion excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }
                    if (Convert.ToString(item["Retencion"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Detalle. Fila: " + filadet.ToString() + ". No contiene Retencion o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregadet = false;
                    }

                    //Si esta todo correcto agrega
                    if (agregadet)
                    {
                        mod_SolDetalle = new DMSolicitudArticulo.SolDetalle();
                        mod_SolDetalle.IdDetalle = contdet.ToString(); 
                        mod_SolDetalle.CodReferencia = Convert.ToString(item["CodReferencia"]);
                        mod_SolDetalle.Marca = Convert.ToString(item["Marca"]);
                        mod_SolDetalle.DesMarca = "";
                        mod_SolDetalle.MarcaNueva = Convert.ToString(item["MarcaNueva"]);
                        if (mod_SolDetalle.MarcaNueva != "")
                        {
                            mod_SolDetalle.Marca = "-1";
                            mod_SolDetalle.DesMarca = mod_SolDetalle.MarcaNueva;
                        }
                        mod_SolDetalle.PaisOrigen = Convert.ToString(item["PaisOrigen"]);
                        mod_SolDetalle.RegionOrigen = Convert.ToString(item["RegionOrigen"]);
                        mod_SolDetalle.TamArticulo = Convert.ToString(item["TamanioArticulo"]);
                        mod_SolDetalle.GradoAlcohol = Convert.ToString(item["GradoAlcohol"]);
                        //mod_SolDetalle.Talla = Convert.ToString(item["Talla"]);
                        //mod_SolDetalle.Color = Convert.ToString(item["Color"]);
                        //mod_SolDetalle.Fragancia = Convert.ToString(item["Fragancia"]);
                        //mod_SolDetalle.Tipos = Convert.ToString(item["Tipos"]);
                        //mod_SolDetalle.Sabor = Convert.ToString(item["Sabor"]);
                        mod_SolDetalle.Modelo = Convert.ToString(item["Modelo"]);
                        mod_SolDetalle.Descripcion = Convert.ToString(item["Descripcion"]);
                        mod_SolDetalle.OtroId = Convert.ToString(item["TextoBreve"]);
                        mod_SolDetalle.ContAlcohol = (item["ContAlcohol"].ToString() == "S" ? true : false);
                        mod_SolDetalle.Estado = "NAR";
                        mod_SolDetalle.Iva = Convert.ToString(item["Iva"]);
                        mod_SolDetalle.Deducible = Convert.ToString(item["Deducible"]);
                        mod_SolDetalle.Retencion = Convert.ToString(item["Retencion"]);
                        mod_SolDetalle.PrecioBruto = Convert.ToString(item["PrecioBruto"]).Replace(',', '.');
                        mod_SolDetalle.Descuento1 = Convert.ToString(item["Descuento1"]).Replace(',', '.');
                        mod_SolDetalle.Descuento2 = Convert.ToString(item["Descuento2"]).Replace(',', '.');
                        mod_SolDetalle.ImpVerde = (item["ImpVerde"].ToString() == "S" ? true : false);
                        mod_SolDetalle.Accion = "I";
                        lst_retSol_Det.Add(mod_SolDetalle);
                        contdet += 1;
                    }
                    filadet += 1;
                }
                FormResponse.root.Add(lst_retSol_Det);

                //Medidas
                int filamed = 1;
                foreach (DataRow item in dsmed.Tables[0].Rows)
                {
                    //Valida tamaño registro y vacios
                    bool agregamed = true;

                    if (Convert.ToString(item["CodReferencia"]).Length > 30)
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna CodReferencia excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }
                    if (Convert.ToString(item["CodReferencia"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene CodReferencia o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }

                    if (Convert.ToString(item["UnidadMedida"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna UnidadMedida excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }
                    if (Convert.ToString(item["UnidadMedida"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene UnidadMedida o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }

                    //if (Convert.ToString(item["TipoUnidadMedida"]).Length > 10)
                    //{
                    //    sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna TipoUnidadMedida excede el tamaño permitido de digitos.");
                    //    sbTxtPed.AppendLine();
                    //    agregamed = false;
                    //}
                    //if (Convert.ToString(item["TipoUnidadMedida"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene TipoUnidadMedida o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregamed = false;
                    //}

                    if (Convert.ToString(item["UniMedConvers"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna UniMedConvers excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }
                    if (Convert.ToString(item["UniMedConvers"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene UniMedConvers o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }

                    if (Convert.ToString(item["FactorCon"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene FactorCon o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }
                    //Decimal result;
                 
                    if (!Decimal.TryParse(Convert.ToString(item["FactorCon"]), out result))
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna FactorCon no es numérica o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }

                    if (Convert.ToString(item["PesoNeto"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene PesoNeto o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }
                    if (!Decimal.TryParse(Convert.ToString(item["PesoNeto"]), out result))
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna PesoNeto no es numérica o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }

                    if (Convert.ToString(item["PesoBruto"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene PesoBruto o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }
                    if (!Decimal.TryParse(Convert.ToString(item["PesoBruto"]), out result))
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna PesoBruto no es numérica o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }

                    if (Convert.ToString(item["Longitud"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene Longitud o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }
                    if (!Decimal.TryParse(Convert.ToString(item["Longitud"]), out result))
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna Longitud no es numérica o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }

                    if (Convert.ToString(item["Ancho"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene Ancho o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }
                    if (!Decimal.TryParse(Convert.ToString(item["Ancho"]), out result))
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna Ancho no es numérica o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }

                    if (Convert.ToString(item["Altura"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene Altura o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }
                    if (!Decimal.TryParse(Convert.ToString(item["Altura"]), out result))
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna Altura no es numérica o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }

                    if (Convert.ToString(item["Volumen"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene Volumen o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }
                    if (!Decimal.TryParse(Convert.ToString(item["Volumen"]), out result))
                    {
                        sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". La columna Volumen no es numérica o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregamed = false;
                    }

                   
                    //if (Convert.ToString(item["UniMedBase"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene Unidad de medida base o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregamed = false;
                    //}

                    //if (Convert.ToString(item["UniMedPedido"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene Unidad de medida pedido o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregamed = false;
                    //}

                    //if (Convert.ToString(item["UniMedES"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene Unidad de medida E/S o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregamed = false;
                    //}

                    //if (Convert.ToString(item["UniMedVenta"]).Trim() == "")
                    //{
                    //    sbTxtPed.Append("Hoja: Medida. Fila: " + filamed.ToString() + ". No contiene Unidad de medida venta o el formato es incorrecto.");
                    //    sbTxtPed.AppendLine();
                    //    agregamed = false;
                    //}
                    if (agregamed)
                    {
                        mod_SolMedidas = new DMSolicitudArticulo.SolMedida();
                        string idDetalle = "0";
                        foreach (DMSolicitudArticulo.SolDetalle dr in lst_retSol_Det)
                        {
                            if (dr.CodReferencia == Convert.ToString(item["CodReferencia"]))
                            {
                                idDetalle = dr.IdDetalle;
                                break;
                            }
                        }
                        mod_SolMedidas.IdDetalle = idDetalle;
                        mod_SolMedidas.UnidadMedida = Convert.ToString(item["UnidadMedida"]);
                        mod_SolMedidas.DesUnidadMedida = "";
                        mod_SolMedidas.TipoUnidadMedida = "";
                        mod_SolMedidas.DesTipoUnidadMedida = "";
                        mod_SolMedidas.UniMedConvers = Convert.ToString(item["UniMedConvers"]);
                        mod_SolMedidas.DesUniMedConvers = "";
                        mod_SolMedidas.FactorCon = Convert.ToString(item["FactorCon"]).Replace(',', '.');
                        mod_SolMedidas.PesoNeto = Convert.ToString(item["PesoNeto"]).Replace(',', '.');
                        mod_SolMedidas.PesoBruto = Convert.ToString(item["PesoBruto"]).Replace(',', '.');
                        mod_SolMedidas.Longitud = Convert.ToString(item["Longitud"]).Replace(',', '.');
                        mod_SolMedidas.Ancho = Convert.ToString(item["Ancho"]).Replace(',', '.');
                        mod_SolMedidas.Altura = Convert.ToString(item["Altura"]).Replace(',', '.');
                        mod_SolMedidas.Volumen = Convert.ToString(item["Volumen"]).Replace(',', '.');
                        
                        //mod_SolMedidas.uniMedBase = (item["UniMedBase"].ToString() == "S" ? true : false);
                        //mod_SolMedidas.uniMedPedido = (item["UniMedPedido"].ToString() == "S" ? true : false);
                        //mod_SolMedidas.uniMedES = (item["UniMedES"].ToString() == "S" ? true : false);
                        //mod_SolMedidas.uniMedVenta = (item["UniMedVenta"].ToString() == "S" ? true : false);
                        mod_SolMedidas.Estado = "A";
                        mod_SolMedidas.Accion = "I";
                        if (idDetalle != "0")
                           lst_retSol_Med.Add(mod_SolMedidas);
                    }
                    filamed += 1;
                }
                FormResponse.root.Add(lst_retSol_Med);

                //Codigos de Barra
                int filacod = 1;
                foreach (DataRow item in dscbr.Tables[0].Rows)
                {
                    //Valida tamaño registro y vacios
                    bool agregacod = true;

                    if (Convert.ToString(item["CodReferencia"]).Length > 30)
                    {
                        sbTxtPed.Append("Hoja: CodBarra. Fila: " + filacod.ToString() + ". La columna CodReferencia excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregacod = false;
                    }
                    if (Convert.ToString(item["CodReferencia"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: CodBarra. Fila: " + filacod.ToString() + ". No contiene CodReferencia o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregacod = false;
                    }

                    if (Convert.ToString(item["UnidadMedida"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: CodBarra. Fila: " + filacod.ToString() + ". La columna UnidadMedida excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregacod = false;
                    }
                    if (Convert.ToString(item["UnidadMedida"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: CodBarra. Fila: " + filacod.ToString() + ". No contiene UnidadMedida o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregacod = false;
                    }

                    if (Convert.ToString(item["TipoEan"]).Length > 10)
                    {
                        sbTxtPed.Append("Hoja: CodBarra. Fila: " + filacod.ToString() + ". La columna TipoEan excede el tamaño permitido de digitos.");
                        sbTxtPed.AppendLine();
                        agregacod = false;
                    }
                    if (Convert.ToString(item["TipoEan"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: CodBarra. Fila: " + filacod.ToString() + ". No contiene TipoEan o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregacod = false;
                    }

                    if (Convert.ToString(item["PaisEan"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: CodBarra. Fila: " + filacod.ToString() + ". No contiene PaisEan o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregacod = false;
                    }

                    if (Convert.ToString(item["NumeroEan"]).Trim() == "")
                    {
                        sbTxtPed.Append("Hoja: CodBarra. Fila: " + filacod.ToString() + ". No contiene NumeroEan o el formato es incorrecto.");
                        sbTxtPed.AppendLine();
                        agregacod = false;
                    }



                    //Validar antes de agregar el codigo EAN con el metodo de aqui mismo
                    if (!getConsultaEAN(Convert.ToString(item["NumeroEan"]), Convert.ToString(item["TipoEan"]),"N").success)
                    {
                        sbTxtPed.Append("Hoja: CodBarra. Fila: " + filacod.ToString() + ". El codigo EAN no es correcto.");
                        sbTxtPed.AppendLine();
                        agregacod = false;
                    }

                    if (agregacod)
                    {
                        mod_SolCodBarra = new DMSolicitudArticulo.SolCodigoBarra();
                        string idDetalle = "0";
                        foreach (DMSolicitudArticulo.SolDetalle dr in lst_retSol_Det)
                        {
                            if (dr.CodReferencia == Convert.ToString(item["CodReferencia"]))
                            {
                                idDetalle = dr.IdDetalle;
                                break;
                            }
                        }
                        mod_SolCodBarra.IdDetalle = idDetalle;
                        mod_SolCodBarra.UnidadMedida = Convert.ToString(item["UnidadMedida"]);
                        mod_SolCodBarra.NumeroEan = Convert.ToString(item["NumeroEan"]);
                        mod_SolCodBarra.TipoEan = Convert.ToString(item["TipoEan"]);
                        mod_SolCodBarra.paisEan = Convert.ToString(item["PaisEan"]);
                        mod_SolCodBarra.DescripcionEan = "Ninguna";//Debe tomarla del metodo de validacion
                        mod_SolCodBarra.Principal = (item["Principal"].ToString() == "S" ? true : false);
                        mod_SolCodBarra.Accion = "I";
                        if (idDetalle != "0")
                          lst_retSol_CBa.Add(mod_SolCodBarra);
                    }
                    filacod += 1;
                }
                FormResponse.root.Add(lst_retSol_CBa);

                //Valida si tiene observaciones 
                if (sbTxtPed.Length == 0)
                {
                    FormResponse.mensaje = "";
                }
                else
                {
                    //Graba el Log en la ruta donde esta la plantilla
                    string RutaDir = HttpContext.Current.Server.MapPath("~/PlantillaArticulos/");
                    string NombreFile = "Log_CargaMasiva" + "-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
                    System.IO.File.WriteAllText(RutaDir + NombreFile, sbTxtPed.ToString());
                    FormResponse.mensaje = "Con Observaciones";
                    //Para descargar el Log
                    string txtFilename = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/webapi/PlantillaArticulos/" + NombreFile;
                    logcarga.Add(txtFilename);
                    FormResponse.root.Add(logcarga); //pos [3]
                }

                //Valida que haya cargado algo
                if (lst_retSol_Det.Count > 0)
                {
                    FormResponse.success = true;
                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.mensaje = "No se ha cargado ningún registro.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FormResponse;
        }


        [ActionName("BajaPlantilla")]
        [HttpGet]
        public string BajaPlantilla()
        {
            String result = null;
            try
            {
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/PlantillaArticulos/"), "Plantilla.xls");

                var localFilePath = fileSavePath;

                if (File.Exists(localFilePath))
                {
                    result = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/webapi/PlantillaArticulos/Plantilla.xls";
                    //result = Path.Combine(HttpContext.Current.Server.MapPath("~/PDF/"), "Plantilla.xls");
                }
            }
            catch (Exception ex)
            { }
            return result;
        }



        [ActionName("ConsultaEAN")]
        [HttpGet]
        public FormResponseArticulo getConsultaEAN(string codEANart, string tipoEANart, string  isAdmin)
        {
            FormResponseArticulo retorno = new FormResponseArticulo();
            retorno.success = false;
            try
            {

                var workBapi = AppConfig.WorkBAPI;
                //var workBapi = "N";
                if (workBapi == "S")
                {
                    if (isAdmin == "S")
                    {
                        AppConfig.dest.Ping();
                        RfcRepository repo = AppConfig.dest.Repository;
                        IRfcFunction fndatosmaestro = repo.CreateFunction("ZPPVALIDAEAN");
                        fndatosmaestro.SetValue("P_CODEAN", codEANart);
                        //fndatosmaestro.SetValue("P_MEINH", "ST");
                        fndatosmaestro.SetValue("P_NUMTP", tipoEANart);
                        fndatosmaestro.Invoke(AppConfig.dest);
                        var coderror = fndatosmaestro.GetString("CODERROR");
                        var deserror = fndatosmaestro.GetString("DESERROR");
                        if (coderror == "")
                        {
                          retorno.success = true;
                        }
                   }
                    if (isAdmin == "N")
                    {
                        //Invocar Web services
                        ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                        var resultado = Proceso.getConsultaEAN(codEANart, tipoEANart);
                        if (resultado)
                            retorno.success = true;                     

                    }
                   

                }
                else
                {
                    if (codEANart == "2050086644092")
                    {

                        retorno.success = true;

                    }

                    if (codEANart == "2050086644153")
                    {

                        retorno.success = true;

                    }
                    if (codEANart == "2050000018992")
                    {

                        retorno.success = true;

                    }
                    if (codEANart == "2050086665837")
                    {

                        retorno.success = true;

                    }
                    if (codEANart == "2050000024986")
                    {

                        retorno.success = true;

                    }
                    if (codEANart == "2050000028861")
                    {

                        retorno.success = true;

                    }
                    if (codEANart == "2050000034848")
                    {

                        retorno.success = true;

                    }
                    if (codEANart == "2050000051845")
                    {

                        retorno.success = true;

                    }
                     if (codEANart == "2050000079832")
                    {

                        retorno.success = true;

                    }
                     if (codEANart == "2050000080579")
                    {

                        retorno.success = true;

                    }
                     if (codEANart == "2050000087295")
                    {

                        retorno.success = true;

                    }
                    if (codEANart == "2050000012235")
                    {

                        retorno.success = true;

                    }
                    if (codEANart == "2050000043079")
                    {

                        retorno.success = true;

                    }
                    if (codEANart == "2050000005794")
                    {

                        retorno.success = true;

                    }
                    if (codEANart == "2050000006173")
                    {

                        retorno.success = true;

                    }
                     if (codEANart == "2050000006241")
                    {

                        retorno.success = true;

                    }


                   




                }





            }
            catch (Exception e)
            {
                retorno.success = false;
                retorno.mensaje = e.Message.ToString();
            }


            return retorno;
        }


        [ActionName("ConsCaracteristicas")]
        [HttpGet]
        public FormResponseArticulo GetconsCaracteristicas(string tipo)
        {
            FormResponseArticulo retorno = new FormResponseArticulo();
            List<Art_ConsultaCaracteristicas> lst_caracteristicas = new List<Art_ConsultaCaracteristicas>();
           
            Art_ConsultaCaracteristicas con_caracteristicas;
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            retorno.success = false;
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            List<string> listaAgrupacion = new List<string>();
            try
            {
                
                xmlParam.DocumentElement.SetAttribute("tipo", "4");
                xmlParam.DocumentElement.SetAttribute("Lista", tipo);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 101, 1); //Articulo.Sol_P_Consulta	101
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    if (tipo == "999")
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {

                            listaAgrupacion.Add(Convert.ToString(item["Agrupacion"]));

                        }
                        retorno.success = true;
                        retorno.root.Add(listaAgrupacion);
                    }
                    else
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            con_caracteristicas = new Art_ConsultaCaracteristicas();
                            con_caracteristicas.Id = Convert.ToString(item["Id"]);
                            con_caracteristicas.Agrupacion = Convert.ToString(item["Agrupacion"]);
                            con_caracteristicas.Descripcion = Convert.ToString(item["Descripcion"]);
                            con_caracteristicas.Modificable = Convert.ToBoolean(item["Modificable"]);
                            con_caracteristicas.Heredable = Convert.ToBoolean(item["Heredable"]);
                            con_caracteristicas.Lista = Convert.ToBoolean(item["Lista"]);
                            con_caracteristicas.OrigenLista = Convert.ToString(item["OrigenLista"]);

                            if (con_caracteristicas.Lista )
                            {
                             CatalogosController catalag = new CatalogosController();
                             var listaValores = catalag.GetCatalogos(con_caracteristicas.OrigenLista);                            
                             con_caracteristicas.listaValor = listaValores;
                            }

                            
                            lst_caracteristicas.Add(con_caracteristicas);
                        }

                        retorno.success = true;
                        retorno.root.Add(lst_caracteristicas);
                    }
                    
                }
                else
                {
                    retorno.success = false;
                    retorno.mensaje = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                }

            }
            catch (Exception e)
            {
                retorno.success = false;
                retorno.mensaje = e.Message.ToString();
            }



            return retorno;
        }



        public void Notificacion22(String IDNIVEL, String IDMODULO, String IDLINEA, String Proveedor, String Ruc, Boolean NotifProveedor, String correoProveedor, String Estado, String Observacion, String Motivo, String Idsolicitud, String IdTipoSolicitud)
        {

            XmlDocument xmlParam = new XmlDocument();
            List<DMSolcitudProveedor.SolNotificacion> Retorno = new List<DMSolcitudProveedor.SolNotificacion>();
            XmlDocument xmlResp = new XmlDocument();
            ClsGeneral objEjecucion = new ClsGeneral();
            DataSet ds3 = new DataSet();
            try
            {
                xmlParam.LoadXml("<Root />");

                if (!String.IsNullOrEmpty(IDNIVEL))
                {
                    xmlParam.DocumentElement.SetAttribute("IDNIVEL", IDNIVEL);
                }
                if (!String.IsNullOrEmpty(IDMODULO))
                {
                    xmlParam.DocumentElement.SetAttribute("IDMODULO", IDMODULO);
                }

                if (!String.IsNullOrEmpty(IDLINEA))
                {
                    xmlParam.DocumentElement.SetAttribute("IDLINEA", IDLINEA);
                }

                ds3 = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 215, 1);

                if (ds3.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    if (ds3.Tables.Count > 1)
                    {

                        Retorno = (from reg in ds3.Tables[0].AsEnumerable().AsParallel()
                                   select new DMSolcitudProveedor.SolNotificacion
                                   {
                                       Apellido1 = reg.Field<String>("Apellido1"),
                                       Apellido2 = reg.Field<String>("Apellido2"),
                                       Cargo = reg.Field<String>("Cargo"),
                                       CorreoE = reg.Field<String>("CorreoE"),
                                       DescLinea = reg.Field<String>("DescLinea"),
                                       DesModulo = reg.Field<String>("DesModulo"),
                                       DEsNivel = reg.Field<String>("DEsNivel"),
                                       DesTipoIdentificacion = reg.Field<String>("DesTipoIdentificacion"),
                                       IdEmpresa = reg["IdEmpresa"] != DBNull.Value ? reg["IdEmpresa"].ToString() : "",
                                       Linea = reg.Field<String>("Linea"),
                                       Modulo = reg.Field<String>("Modulo"),
                                       Nivel = reg.Field<String>("Nivel"),
                                       Nombre1 = reg.Field<String>("Nombre1"),
                                       Nombre2 = reg.Field<String>("Nombre2"),
                                       Ruc = reg.Field<String>("Ruc"),
                                       TipoIdent = reg.Field<String>("TipoIdent"),
                                       Usuario = reg.Field<String>("Usuario"),


                                   }).ToList<DMSolcitudProveedor.SolNotificacion>();



                    }

                    // ********************** CODIGO DEL METODO **********************


                    string rutaEmail = System.Web.Hosting.HostingEnvironment.MapPath("~/PlantillasEMail");
                    string asuntoEmail = "";
                    string retorno = "";

                    // ********************** CODIGO DEL METODO **********************

                    if (!NotifProveedor)
                    {

                        if (IdTipoSolicitud == "5")
                        {
                            rutaEmail = rutaEmail + "\\NotificacionSolArticuloCam.htm";
                        }
                        if (IdTipoSolicitud == "2")
                        {
                            rutaEmail = rutaEmail + "\\NotificacionSolArticuloMod.htm";
                        }
                        if (IdTipoSolicitud == "1")
                        {
                            rutaEmail = rutaEmail + "\\NotificacionSolArticulo.htm";
                        }
                        
                        asuntoEmail = "Portal de Proveedores - Solicitud de Proveedor";



                        AngularJSAuthentication.API.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new AngularJSAuthentication.API.WCFEnvioCorreo.ServEnvioClientSoapClient();
                        string mensajeEmail = System.IO.File.ReadAllText(rutaEmail);


                        foreach (DMSolcitudProveedor.SolNotificacion drow in Retorno)
                        {
                            string mensajeEmailparameter = mensajeEmail;
                            try
                            {
                                if (!String.IsNullOrEmpty(drow.CorreoE))
                                {

                                    mensajeEmailparameter = mensajeEmailparameter.Replace("_@Motivo", Motivo);
                                    mensajeEmailparameter = mensajeEmailparameter.Replace("_@Observacion", Observacion);

                                    mensajeEmailparameter = mensajeEmailparameter.Replace("_@Ruc", Ruc);
                                    mensajeEmailparameter = mensajeEmailparameter.Replace("_@Solicitud", Idsolicitud);
                                    mensajeEmailparameter = mensajeEmailparameter.Replace("_@NombreUsuario", drow.Nombre1 + " " + drow.Nombre2 + " " + drow.Apellido1 + " " + drow.Apellido2);
                                    mensajeEmailparameter = mensajeEmailparameter.Replace("_@Usuario", Proveedor);
                                    //                                    AngularJSAuthentication.API.WCFEnvioCorreo.CompositeType objResp;
                                    
                                    Thread t = new Thread(() => objEnvMail.EnviarCorreo("", drow.CorreoE, "", "", asuntoEmail, mensajeEmailparameter, true, true, false, null));
                                    t.Start();
                                   
                                }
                            }
                            catch (Exception)
                            {


                            }



                        }
                    }
                    else
                    {

                        try
                        {

                            if (IdTipoSolicitud == "3")
                            {
                                rutaEmail = rutaEmail + "\\NotificacionSolArticuloCam.htm";
                            }
                            if (IdTipoSolicitud == "2") 
                            {
                                rutaEmail = rutaEmail + "\\NotificacionSolArtProveedorMod.htm";
                            }
                            if (IdTipoSolicitud == "1") 
                            {
                                rutaEmail = rutaEmail + "\\NotificacionSolArtProveedor.htm";
                            }
                            
                            asuntoEmail = "Portal de Proveedores - Solicitud de Proveedor";

                            AngularJSAuthentication.API.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new AngularJSAuthentication.API.WCFEnvioCorreo.ServEnvioClientSoapClient();
                            string mensajeEmail = System.IO.File.ReadAllText(rutaEmail);


                            if (!String.IsNullOrEmpty(correoProveedor))
                            {

                                mensajeEmail = mensajeEmail.Replace("_@NombreUsuario", Proveedor);
                                mensajeEmail = mensajeEmail.Replace("_@Estado", Estado);
                                mensajeEmail = mensajeEmail.Replace("_@Solicitud", Idsolicitud);
                                
                                Thread t = new Thread(() => objEnvMail.EnviarCorreo("", correoProveedor, "", "", asuntoEmail, mensajeEmail, true, true, false, null));
                                t.Start();
                                
                            }


                        }
                        catch (Exception)
                        {


                        }

                    }

                }
                else
                {
                    //throw new Exception(ds3.Tables["TblEstado"].Rows[0]["MsgError"].ToString());

                }
            }
            catch (Exception ex)
            {


            }

        }



        //Simular creacion de articulo 
        private Boolean CreaArticulo(DMSolicitudArticulo modeloArticulo)
        {

            DataSet ds3 = new DataSet();
            ClsGeneral objEjecucion3 = new ClsGeneral();

            for (var i = 0; i < modeloArticulo.p_SolDetalle.Length; i++)
            {

                try
                {
                    //Obtener datos de correo
                    var codSap = "";
                    var codSolEnvia = modeloArticulo.p_SolCabecera[0].IdSolicitud.ToString();
                    var codDetEnvia = modeloArticulo.p_SolDetalle[i].IdDetalle.ToString();
                    codSap = modeloArticulo.p_SolDetalle[i].CodSAPart;
                    var wresulFactList =
                              new System.Xml.Linq.XDocument(
                               new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                               new XElement("Root",
                                       new XAttribute("CodigoSAP", codSap),
                                       new XAttribute("CodigoSol", codSolEnvia),
                                       new XAttribute("DetalleId", codDetEnvia))
                                        );
                    if (modeloArticulo.p_SolDetalle[i].Estado == "Aprobado")
                        ds3 = objEjecucion3.EjecucionGralDs(wresulFactList.ToString(), 104, 1);



                    if (ds3.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                    {



                    }

                }
                catch
                {

                }

            }

            return true;
        }

        //Creacion de articulo llamanado a BAPI
        private FormResponseArticulo CreaArticuloBapi(DMSolicitudArticulo modeloArticulo, String CodProveedorSAP)
        {
           
            FormResponseArticulo creaArtBapi = new FormResponseArticulo();
            DataSet ds4 = new DataSet();
            ClsGeneral objEjecucion4 = new ClsGeneral();
            creaArtBapi.success = false;
            creaArtBapi.mensaje = "";
            try
            {
                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;

               


                // *****INICIO DE LISTADO DE ARTICULOS *********
                foreach (var artGrabar in modeloArticulo.p_SolDetalle)
                {

                    if (artGrabar.Estado == "Aprobado")
                    {
                        IRfcFunction fndatosmaestro = repo.CreateFunction("ZPPARTICULOCREA");

                        fndatosmaestro.SetValue("P_LOGCOMPLETO", "X");

                        //Datos de almacenes                                                                 **********ESTRUCTURA # 1  PT_ALMACEN************
                        var DTALMAC = fndatosmaestro.GetTable("PT_ALMACEN");

                        foreach (var i in modeloArticulo.p_SolAlmacen)
                        {

                            IRfcStructure ITPALMAC;
                            ITPALMAC = repo.GetStructureMetadata("ZWAPPINDALMACEN").CreateStructure();
                            ITPALMAC.SetValue("SEQ", i.IdDetalle);
                            ITPALMAC.SetValue("LGNUM", i.Almacen);
                            ITPALMAC.SetValue("LGTYP", i.TipAlmacen.Split('-')[1]);
                            ITPALMAC.SetValue("LTKZE", i.IndAlmacenE.Split('-')[1]);
                            ITPALMAC.SetValue("LTKZA", i.IndAlmacenS.Split('-')[1]);
                            ITPALMAC.SetValue("LGBKZ", i.IndAreaAlmNew.Split('-')[1]);
                            if (i.Accion != "D" && artGrabar.IdDetalle == i.IdDetalle)
                                DTALMAC.Append(ITPALMAC);
                        }

                        fndatosmaestro.SetValue("PT_ALMACEN", DTALMAC);

                        //Unidades de medidas                                                                 **********ESTRUCTURA # 2  PT_UNIMED************
                        var DTUNIMED = fndatosmaestro.GetTable("PT_UNIMED");

                        //foreach (var i in modeloArticulo.p_SolMedida)
                        //{
                        //    IRfcStructure ITPUNIMED;
                        //    ITPUNIMED = repo.GetStructureMetadata("ZWAPPUNIMED").CreateStructure();
                        //    ITPUNIMED.SetValue("SEQ", i.IdDetalle);
                        //    ITPUNIMED.SetValue("MEINS", i.UnidadMedida);
                        //    ITPUNIMED.SetValue("LAENG", i.Longitud);
                        //    ITPUNIMED.SetValue("BREIT", i.Ancho);
                        //    ITPUNIMED.SetValue("HOEHE", i.Altura);
                        //    ITPUNIMED.SetValue("BRGEW", i.PesoBruto);
                        //    ITPUNIMED.SetValue("UMREZ", i.FactorCon);
                        //    ITPUNIMED.SetValue("VOLUM", i.Volumen);
                        //    ITPUNIMED.SetValue("VOLEH", i.UniMedidaVolumen);
                        //    if (i.Accion != "D" && artGrabar.IdDetalle == i.IdDetalle)
                        //        DTUNIMED.Append(ITPUNIMED);
                        //}

                        fndatosmaestro.SetValue("PT_UNIMED", DTUNIMED);

                        //Codigos de barra                                                                     **********ESTRUCTURA # 3  PT_EAN************
                        var DTEAN = fndatosmaestro.GetTable("PT_EAN");

                        //foreach (var i in modeloArticulo.p_SolCodigoBarra)
                        //{

                        //    IRfcStructure ITPEAN;
                        //    ITPEAN = repo.GetStructureMetadata("ZWAPPMEAN").CreateStructure();
                        //    ITPEAN.SetValue("MEINH", i.UnidadMedida);
                        //    ITPEAN.SetValue("EAN11", i.NumeroEan);
                        //    ITPEAN.SetValue("EANTP", i.TipoEan);
                        //    if (i.Principal)
                        //        ITPEAN.SetValue("HPEAN", "X");
                        //    else
                        //        ITPEAN.SetValue("HPEAN", "");
                        //    if (i.Accion != "D" && artGrabar.IdDetalle == i.IdDetalle)
                        //        DTEAN.Append(ITPEAN);

                        //}
                        fndatosmaestro.SetValue("PT_EAN", DTEAN);

                        //Datos de canales de distrbucion y balanzas                                           **********ESTRUCTURA # 4  PT_BALANZAS************
                        var DTBALANZAS = fndatosmaestro.GetTable("PT_BALANZAS");

                        foreach (var i in modeloArticulo.p_SolIndAreaAlmacen)
                        {
                            IRfcStructure ITPBALANZAS;

                            ITPBALANZAS = repo.GetStructureMetadata("ZWAPPBALANZAS").CreateStructure();
                            ITPBALANZAS.SetValue("SEQ", i.IdDetalle);
                            ITPBALANZAS.SetValue("SCAGR", i.GrupoBalanzas);
                            ITPBALANZAS.SetValue("VTWEG", i.IndAreaAlmacen);
                            if (i.Accion != "D" && artGrabar.IdDetalle == i.IdDetalle)
                                DTBALANZAS.Append(ITPBALANZAS);
                        }


                        fndatosmaestro.SetValue("PT_BALANZAS", DTBALANZAS);

                        //Datos de aticulos                                                                    **********ESTRUCTURA # 5  PT_ARTICULOS************
                        var idDetalle = "";
                        var DTARTIC = fndatosmaestro.GetTable("PT_ARTICULOS");
                        foreach (var i in modeloArticulo.p_SolDetalle)
                        {
                            IRfcStructure ITPTART;

                            ITPTART = repo.GetStructureMetadata("ZWAPPMATNR").CreateStructure();
                            ITPTART.SetValue("SEQ", i.IdDetalle);
                            ITPTART.SetValue("IDNLF", i.CodReferencia);
                            ITPTART.SetValue("LINNEG", modeloArticulo.p_SolCabecera[0].LineaNegocio);
                            ITPTART.SetValue("BRAND_ID", i.Marca);
                            ITPTART.SetValue("MAKTX", i.Descripcion);   //OJO  donde guardar descripcion o texto breve
                            ITPTART.SetValue("WHERL", i.PaisOrigen);
                            ITPTART.SetValue("WHERR", !string.IsNullOrEmpty(i.RegionOrigen) ? i.RegionOrigen.Split('-')[1] : "");
                            ITPTART.SetValue("GROES", i.TamArticulo);
                            ITPTART.SetValue("EXTWG", i.GradoAlcohol);
                            ITPTART.SetValue("TAKLV", i.Iva);
                            ITPTART.SetValue("PLGTP", i.Deducible);
                            ITPTART.SetValue("LABOR", i.Retencion);//ojo revisar
                            ITPTART.SetValue("NORMT", i.Modelo);
                            var f = modeloArticulo.p_SolMedida.Where(x => x.IdDetalle == i.IdDetalle && x.uniMedBase).FirstOrDefault();
                            var p = modeloArticulo.p_SolMedida.Where(x => x.IdDetalle == i.IdDetalle && x.uniMedPedido).FirstOrDefault();
                            var v = modeloArticulo.p_SolMedida.Where(x => x.IdDetalle == i.IdDetalle && x.uniMedVenta).FirstOrDefault();
                            var es = modeloArticulo.p_SolMedida.Where(x => x.IdDetalle == i.IdDetalle && x.uniMedES).FirstOrDefault();

                            if (p != null)
                            {

                                ITPTART.SetValue("BSTME", p.UnidadMedida);

                            }
                            else
                                ITPTART.SetValue("BSTME", "");

                            ITPTART.SetValue("MEINS", f.UnidadMedida);
                            ITPTART.SetValue("UMREZ", p.FactorCon);
                            ITPTART.SetValue("NTGEW", f.PesoNeto);
                            ITPTART.SetValue("BRGEW", f.PesoBruto);
                            ITPTART.SetValue("LAENG", f.Longitud);
                            ITPTART.SetValue("BREIT", f.Ancho);
                            ITPTART.SetValue("HOEHE", f.Altura);
                            ITPTART.SetValue("PRECIO_BRUTO", i.PrecioBruto);
                            ITPTART.SetValue("DSCTO1", i.Descuento1);
                            ITPTART.SetValue("DSCTO2", i.Descuento2);
                            ITPTART.SetValue("VOLUM", f.Volumen);
                            ITPTART.SetValue("VOLEH", f.UniMedidaVolumen);


                            if (v != null)
                            {
                                if (v.UnidadMedida != f.UnidadMedida)
                                    ITPTART.SetValue("WVRKM", v.UnidadMedida);
                                else
                                    ITPTART.SetValue("WVRKM", "");

                            }
                            else
                                ITPTART.SetValue("WVRKM", "");


                            if (es != null)
                            {
                                if (es.UnidadMedida != f.UnidadMedida)
                                    ITPTART.SetValue("WAUSM", es.UnidadMedida);
                                else
                                    ITPTART.SetValue("WAUSM", "");

                            }
                            else
                                ITPTART.SetValue("WAUSM", "");

                            if (f.ImpVerde)
                                ITPTART.SetValue("IMPTO_VERDE", 1);
                            else
                                ITPTART.SetValue("IMPTO_VERDE", 0);
                            ITPTART.SetValue("LIFNR", CodProveedorSAP);
                            var g = modeloArticulo.p_SolCompras.Where(x => x.IdDetalle == i.IdDetalle).FirstOrDefault();
                            ITPTART.SetValue("EKORG", g.OrganizacionCompras);
                            ITPTART.SetValue("MRPPP", g.FrecuenciaEntrega);
                            ITPTART.SetValue("MTART", g.TipoMaterial);
                            ITPTART.SetValue("ATTYP", g.CategoriaMaterial);
                            ITPTART.SetValue("MATKL", g.GrupoArticulo);
                            ITPTART.SetValue("SECCION", g.GrupoArticulo.Substring(0, 3));
                            ITPTART.SetValue("LTSNR", g.SurtidoParcial);
                            ITPTART.SetValue("WRKST", g.MateriaDes);
                            ITPTART.SetValue("COSTOFOB", i.PrecioBruto);
                            ITPTART.SetValue("SERVV", g.IndPedido);
                            ITPTART.SetValue("FPRFM", g.PerfilDistribucion);
                            ITPTART.SetValue("WEKGR", g.GrupoCompra);
                            ITPTART.SetValue("WBKLA", g.CategoriaValoracion);  //ojo revisar
                            ITPTART.SetValue("RAUBE", g.CondicionAlmacen);
                            ITPTART.SetValue("BBTYP", g.ClListaSurtido);
                            ITPTART.SetValue("MSTAE", g.EstatusMaterial);
                            ITPTART.SetValue("MSTAV", g.EstatusVenta);
                            var validoDesdeSAP = "";
                            if (g.ValidoDesde != null)
                            {
                                validoDesdeSAP = g.ValidoDesde.Split('/')[2] + g.ValidoDesde.Split('/')[0] + g.ValidoDesde.Split('/')[1];
                            }
                            ITPTART.SetValue("MSTDV", validoDesdeSAP);
                            ITPTART.SetValue("SAITY", i.Coleccion);
                            ITPTART.SetValue("SAISO", i.Temporada);
                            ITPTART.SetValue("SAISJ", i.Estacion);
                            ITPTART.SetValue("PRODH", g.JerarquiaProd);
                            ITPTART.SetValue("VKORG", "1001");
                            ITPTART.SetValue("ZEINR", i.CantidadPedir);
                            if (i.Accion != "D" && artGrabar.IdDetalle == i.IdDetalle)
                            {
                                DTARTIC.Append(ITPTART);
                                idDetalle = i.IdDetalle;
                            }
                        }
                        fndatosmaestro.SetValue("PT_ARTICULOS", DTARTIC);
                        fndatosmaestro.Invoke(AppConfig.dest);
                        var coderror = fndatosmaestro.GetString("CODERROR");
                        var deserror2 = fndatosmaestro.GetString("DESERROR");
                        if (coderror == "0")
                        {
                            creaArtBapi.success = true; creaArtBapi.mensaje = "OK";
                            var tablaLog2 = fndatosmaestro.GetTable("PT_ARTICULOS");
                            var result = (from a in tablaLog2
                                          select new
                                          {
                                              CODSAP = a.GetString("MATNR"),
                                          }).ToList();

                            if (result != null)
                            {


                                try
                                {                                                                                 //****************TRANSACCION OK EN SAP**************
                                    //Actualizar estado del artículo en la solicitud y su código SAP generado 
                                    var codSolEnvia = modeloArticulo.p_SolCabecera[0].IdSolicitud.ToString();
                                    var codDetEnvia = idDetalle.ToString();
                                    var codSap = result[0].CODSAP;
                                    var wresulFactList =
                                              new System.Xml.Linq.XDocument(
                                               new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                               new XElement("Root",
                                                       new XAttribute("CodigoSAP", codSap),
                                                       new XAttribute("DescripcionError", deserror2 + ". Código SAP generado: " + codSap),
                                                       new XAttribute("CodigoSol", codSolEnvia),
                                                       new XAttribute("DetalleId", codDetEnvia))
                                                        );
                                    ds4 = objEjecucion4.EjecucionGralDs(wresulFactList.ToString(), 104, 1);


                                    if (ds4.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                                    {

                                    }

                                }
                                catch
                                {

                                }

                            }

                        }
                        else
                        {
                            creaArtBapi.success = false;
                            var tablaLog = fndatosmaestro.GetTable("PT_LOG");
                            var result = (from a in tablaLog
                                          select new
                                          {
                                              MESSAGE = a.GetString("MESSAGE"),
                                              NUMBER = a.GetString("NUMBER"),
                                              ID = a.GetString("ID"),
                                          }).ToList();
                            var msj = "";
                            foreach (var m in result)
                            {
                                msj = msj + m.NUMBER + "-" + m.MESSAGE;
                            }
                            if (msj != "")
                            {
                                creaArtBapi.mensaje = msj;
                                //****************TRANSACCION ERROR EN SAP**************
                                //Actualizar estado del artículo en la solicitud  y registrar el error que se presento
                                var codSolEnvia = modeloArticulo.p_SolCabecera[0].IdSolicitud.ToString();
                                var codDetEnvia = idDetalle.ToString();

                                var wresulFactList =
                                          new System.Xml.Linq.XDocument(
                                           new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                           new XElement("Root",
                                                   new XAttribute("CodigoError", coderror),
                                                   new XAttribute("DescripcionError", msj),
                                                   new XAttribute("CodigoSol", codSolEnvia),
                                                   new XAttribute("DetalleId", codDetEnvia))
                                                    );
                                ds4 = objEjecucion4.EjecucionGralDs(wresulFactList.ToString(), 104, 1);


                                if (ds4.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                                {

                                }


                            }

                            //Devuelvo solicitud si existen errores en la creacion
                            return creaArtBapi;


                        }
                        var deserror = fndatosmaestro.GetString("DESERROR");
                        var tablaLog7 = fndatosmaestro.GetTable("PT_LOG");
                        var tablaLog6 = fndatosmaestro.GetTable("PT_ARTICULOS");
                    }
                }
                // *****FIN DE LISTADO DE ARTICULOS *********

            }
            catch (Exception e)
            {

                creaArtBapi.success = false;
                creaArtBapi.mensaje = e.Message.ToString();

            }
            return creaArtBapi;
        }

        //Actualizacion de articulo llamanado a BAPI
        private FormResponseArticulo ActualizaArticuloBapi(DMSolicitudArticulo modeloArticulo, String CodProveedorSAP)
        {

            FormResponseArticulo creaArtBapi = new FormResponseArticulo();
            DataSet ds4 = new DataSet();
            ClsGeneral objEjecucion4 = new ClsGeneral();
            creaArtBapi.success = false;
            creaArtBapi.mensaje = "";
            try
            {
                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;




                // *****INICIO DE LISTADO DE ARTICULOS *********
                foreach (var artGrabar in modeloArticulo.p_SolDetalle)
                {

                    if (artGrabar.Estado == "Aprobado")
                    {
                        IRfcFunction fndatosmaestro = repo.CreateFunction("ZPPARTICULOMOD");

                        fndatosmaestro.SetValue("P_LOGCOMPLETO", "X");

                        //Datos de almacenes                                                                 **********ESTRUCTURA # 1  PT_ALMACEN************
                        //var DTALMAC = fndatosmaestro.GetTable("PT_ALMACEN");

                        //foreach (var i in modeloArticulo.p_SolAlmacen)
                        //{

                        //    IRfcStructure ITPALMAC;
                        //    ITPALMAC = repo.GetStructureMetadata("ZWAPPINDALMACEN").CreateStructure();
                        //    ITPALMAC.SetValue("SEQ", i.IdDetalle);
                        //    ITPALMAC.SetValue("LGNUM", i.Almacen);
                        //    ITPALMAC.SetValue("LGTYP", i.TipAlmacen.Split('-')[1]);
                        //    ITPALMAC.SetValue("LTKZE", i.IndAlmacenE.Split('-')[1]);
                        //    ITPALMAC.SetValue("LTKZA", i.IndAlmacenS.Split('-')[1]);
                        //    ITPALMAC.SetValue("LGBKZ", i.IndAreaAlmNew.Split('-')[1]);
                        //    if (i.Accion != "D" && artGrabar.IdDetalle == i.IdDetalle)
                        //        DTALMAC.Append(ITPALMAC);
                        //}

                        //fndatosmaestro.SetValue("PT_ALMACEN", DTALMAC);

                        //Unidades de medidas                                                                 **********ESTRUCTURA # 2  PT_UNIMED************
                       // var DTUNIMED = fndatosmaestro.GetTable("PT_UNIMED");

                        //foreach (var i in modeloArticulo.p_SolMedida)
                        //{
                        //    IRfcStructure ITPUNIMED;
                        //    ITPUNIMED = repo.GetStructureMetadata("ZWAPPUNIMED").CreateStructure();
                        //    ITPUNIMED.SetValue("SEQ", i.IdDetalle);
                        //    ITPUNIMED.SetValue("MEINS", i.UnidadMedida);
                        //    ITPUNIMED.SetValue("LAENG", i.Longitud);
                        //    ITPUNIMED.SetValue("BREIT", i.Ancho);
                        //    ITPUNIMED.SetValue("HOEHE", i.Altura);
                        //    ITPUNIMED.SetValue("BRGEW", i.PesoBruto);
                        //    ITPUNIMED.SetValue("UMREZ", i.FactorCon);
                        //    ITPUNIMED.SetValue("VOLUM", i.Volumen);
                        //    ITPUNIMED.SetValue("VOLEH", i.UniMedidaVolumen);
                        //    if (i.Accion != "D" && artGrabar.IdDetalle == i.IdDetalle)
                        //        DTUNIMED.Append(ITPUNIMED);
                        //}

                        //fndatosmaestro.SetValue("PT_UNIMED", DTUNIMED);

                        //Codigos de barra                                                                     **********ESTRUCTURA # 3  PT_EAN************
                        //var DTEAN = fndatosmaestro.GetTable("PT_EAN");

                        //foreach (var i in modeloArticulo.p_SolCodigoBarra)
                        //{

                        //    IRfcStructure ITPEAN;
                        //    ITPEAN = repo.GetStructureMetadata("ZWAPPMEAN").CreateStructure();
                        //    ITPEAN.SetValue("MEINH", i.UnidadMedida);
                        //    ITPEAN.SetValue("EAN11", i.NumeroEan);
                        //    ITPEAN.SetValue("EANTP", i.TipoEan);
                        //    if (i.Principal)
                        //        ITPEAN.SetValue("HPEAN", "X");
                        //    else
                        //        ITPEAN.SetValue("HPEAN", "");
                        //    if (i.Accion != "D" && artGrabar.IdDetalle == i.IdDetalle)
                        //        DTEAN.Append(ITPEAN);

                        //}
                        //fndatosmaestro.SetValue("PT_EAN", DTEAN);

                        //Datos de canales de distrbucion y balanzas                                           **********ESTRUCTURA # 4  PT_BALANZAS************
                        var DTBALANZAS = fndatosmaestro.GetTable("PT_BALANZAS");

                        foreach (var i in modeloArticulo.p_SolIndAreaAlmacen)
                        {
                            IRfcStructure ITPBALANZAS;

                            ITPBALANZAS = repo.GetStructureMetadata("ZWAPPBALANZAS").CreateStructure();
                            ITPBALANZAS.SetValue("SEQ", i.IdDetalle);
                            ITPBALANZAS.SetValue("SCAGR", i.GrupoBalanzas);
                            ITPBALANZAS.SetValue("VTWEG", i.IndAreaAlmacen);
                            if (i.Accion != "D" && artGrabar.IdDetalle == i.IdDetalle)
                                DTBALANZAS.Append(ITPBALANZAS);
                        }


                        fndatosmaestro.SetValue("PT_BALANZAS", DTBALANZAS);

                        //Datos de aticulos                                                                    **********ESTRUCTURA # 5  PT_ARTICULOS************
                        var idDetalle = "";
                        var DTARTIC = fndatosmaestro.GetTable("PT_ARTICULOS");
                        foreach (var i in modeloArticulo.p_SolDetalle)
                        {
                            IRfcStructure ITPTART;

                            ITPTART = repo.GetStructureMetadata("ZWAPPMATNR").CreateStructure();
                            ITPTART.SetValue("SEQ", i.IdDetalle);
                            ITPTART.SetValue("IDNLF", i.CodReferencia);
                            ITPTART.SetValue("LINNEG", modeloArticulo.p_SolCabecera[0].LineaNegocio);
                            ITPTART.SetValue("BRAND_ID", i.Marca);
                            ITPTART.SetValue("MAKTX", i.Descripcion);   //OJO  donde guardar descripcion o texto breve
                            ITPTART.SetValue("WHERL", i.PaisOrigen);
                            ITPTART.SetValue("WHERR", !string.IsNullOrEmpty(i.RegionOrigen) ? i.RegionOrigen.Split('-')[1] : "");
                            ITPTART.SetValue("GROES", i.TamArticulo);
                            ITPTART.SetValue("EXTWG", i.GradoAlcohol);
                            ITPTART.SetValue("TAKLV", i.Iva);
                            ITPTART.SetValue("PLGTP", i.Deducible);
                            ITPTART.SetValue("LABOR", i.Retencion);//ojo revisar
                            ITPTART.SetValue("NORMT", i.Modelo);
                            var f = modeloArticulo.p_SolMedida.Where(x => x.IdDetalle == i.IdDetalle && x.uniMedBase).FirstOrDefault();
                            var p = modeloArticulo.p_SolMedida.Where(x => x.IdDetalle == i.IdDetalle && x.uniMedPedido).FirstOrDefault();
                            var v = modeloArticulo.p_SolMedida.Where(x => x.IdDetalle == i.IdDetalle && x.uniMedVenta).FirstOrDefault();
                            var es = modeloArticulo.p_SolMedida.Where(x => x.IdDetalle == i.IdDetalle && x.uniMedES).FirstOrDefault();

                            if (p != null)
                            {

                                ITPTART.SetValue("BSTME", p.UnidadMedida);

                            }
                            else
                                ITPTART.SetValue("BSTME", "");

                            ITPTART.SetValue("MEINS", f.UnidadMedida);
                            ITPTART.SetValue("UMREZ", p.FactorCon);
                            ITPTART.SetValue("NTGEW", f.PesoNeto);
                            ITPTART.SetValue("BRGEW", f.PesoBruto);
                            ITPTART.SetValue("LAENG", f.Longitud);
                            ITPTART.SetValue("BREIT", f.Ancho);
                            ITPTART.SetValue("HOEHE", f.Altura);
                            ITPTART.SetValue("PRECIO_BRUTO", i.PrecioBruto);
                            ITPTART.SetValue("DSCTO1", i.Descuento1);
                            ITPTART.SetValue("DSCTO2", i.Descuento2);
                            ITPTART.SetValue("VOLUM", f.Volumen);
                            ITPTART.SetValue("VOLEH", f.UniMedidaVolumen);


                            if (v != null)
                            {
                                if (v.UnidadMedida != f.UnidadMedida)
                                    ITPTART.SetValue("WVRKM", v.UnidadMedida);
                                else
                                    ITPTART.SetValue("WVRKM", "");

                            }
                            else
                                ITPTART.SetValue("WVRKM", "");


                            if (es != null)
                            {
                                if (es.UnidadMedida != f.UnidadMedida)
                                    ITPTART.SetValue("WAUSM", es.UnidadMedida);
                                else
                                    ITPTART.SetValue("WAUSM", "");

                            }
                            else
                                ITPTART.SetValue("WAUSM", "");

                            if (f.ImpVerde)
                                ITPTART.SetValue("IMPTO_VERDE", 1);
                            else
                                ITPTART.SetValue("IMPTO_VERDE", 0);
                            ITPTART.SetValue("LIFNR", CodProveedorSAP);
                            var g = modeloArticulo.p_SolCompras.Where(x => x.IdDetalle == i.IdDetalle).FirstOrDefault();
                            ITPTART.SetValue("EKORG", g.OrganizacionCompras);
                            ITPTART.SetValue("MRPPP", g.FrecuenciaEntrega);
                            ITPTART.SetValue("MTART", g.TipoMaterial);
                            ITPTART.SetValue("ATTYP", g.CategoriaMaterial);
                            ITPTART.SetValue("MATKL", g.GrupoArticulo);
                            ITPTART.SetValue("SECCION", g.GrupoArticulo.Substring(0, 3));
                            ITPTART.SetValue("LTSNR", g.SurtidoParcial);
                            ITPTART.SetValue("WRKST", g.MateriaDes);
                            ITPTART.SetValue("COSTOFOB", i.PrecioBruto);
                            ITPTART.SetValue("SERVV", g.IndPedido);
                            ITPTART.SetValue("FPRFM", g.PerfilDistribucion);
                            ITPTART.SetValue("WEKGR", g.GrupoCompra);
                            ITPTART.SetValue("WBKLA", g.CategoriaValoracion);  //ojo revisar
                            ITPTART.SetValue("RAUBE", g.CondicionAlmacen);
                            ITPTART.SetValue("BBTYP", g.ClListaSurtido);
                            ITPTART.SetValue("MSTAE", g.EstatusMaterial);
                            ITPTART.SetValue("MSTAV", g.EstatusVenta);
                            var validoDesdeSAP = "";
                            if (g.ValidoDesde != null)
                            {
                                validoDesdeSAP = g.ValidoDesde.Split('/')[2] + g.ValidoDesde.Split('/')[0] + g.ValidoDesde.Split('/')[1];
                            }
                            ITPTART.SetValue("MSTDV", validoDesdeSAP);
                            ITPTART.SetValue("SAITY", i.Coleccion);
                            ITPTART.SetValue("SAISO", i.Temporada);
                            ITPTART.SetValue("SAISJ", i.Estacion);
                            ITPTART.SetValue("PRODH", g.JerarquiaProd);
                            ITPTART.SetValue("VKORG", "1001");
                            ITPTART.SetValue("ZEINR", i.CantidadPedir);
                            if (i.Accion != "D" && artGrabar.IdDetalle == i.IdDetalle)
                            {
                                DTARTIC.Append(ITPTART);
                                idDetalle = i.IdDetalle;
                            }
                        }
                        fndatosmaestro.SetValue("PT_ARTICULOS", DTARTIC);
                        fndatosmaestro.Invoke(AppConfig.dest);
                        var coderror = fndatosmaestro.GetString("CODERROR");
                        var deserror2 = fndatosmaestro.GetString("DESERROR");
                        if (coderror == "0")
                        {
                            creaArtBapi.success = true; creaArtBapi.mensaje = "OK";
                            var tablaLog2 = fndatosmaestro.GetTable("PT_ARTICULOS");
                            var result = (from a in tablaLog2
                                          select new
                                          {
                                              CODSAP = a.GetString("MATNR"),
                                          }).ToList();

                            if (result != null)
                            {


                                try
                                {                                                                                 //****************TRANSACCION OK EN SAP**************
                                    //Actualizar estado del artículo en la solicitud y su código SAP generado 
                                    var codSolEnvia = modeloArticulo.p_SolCabecera[0].IdSolicitud.ToString();
                                    var codDetEnvia = idDetalle.ToString();
                                    var codSap = result[0].CODSAP;
                                    var wresulFactList =
                                              new System.Xml.Linq.XDocument(
                                               new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                               new XElement("Root",
                                                       new XAttribute("CodigoSAP", codSap),
                                                       new XAttribute("DescripcionError", deserror2 + ". Código SAP artículo: " + codSap),
                                                       new XAttribute("CodigoSol", codSolEnvia),
                                                       new XAttribute("DetalleId", codDetEnvia))
                                                        );
                                    ds4 = objEjecucion4.EjecucionGralDs(wresulFactList.ToString(), 104, 1);


                                    if (ds4.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                                    {

                                    }

                                }
                                catch
                                {

                                }

                            }

                        }
                        else
                        {
                            creaArtBapi.success = false;
                            var tablaLog = fndatosmaestro.GetTable("PT_LOG");
                            var result = (from a in tablaLog
                                          select new
                                          {
                                              MESSAGE = a.GetString("MESSAGE"),
                                              NUMBER = a.GetString("NUMBER"),
                                              ID = a.GetString("ID"),
                                          }).ToList();
                            var msj = "";
                            foreach (var m in result)
                            {
                                msj = msj + m.NUMBER + "-" + m.MESSAGE;
                            }
                            if (msj != "")
                            {
                                creaArtBapi.mensaje = msj;
                                //****************TRANSACCION ERROR EN SAP**************
                                //Actualizar estado del artículo en la solicitud  y registrar el error que se presento
                                var codSolEnvia = modeloArticulo.p_SolCabecera[0].IdSolicitud.ToString();
                                var codDetEnvia = idDetalle.ToString();

                                var wresulFactList =
                                          new System.Xml.Linq.XDocument(
                                           new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                           new XElement("Root",
                                                   new XAttribute("CodigoError", coderror),
                                                   new XAttribute("DescripcionError", msj),
                                                   new XAttribute("CodigoSol", codSolEnvia),
                                                   new XAttribute("DetalleId", codDetEnvia))
                                                    );
                                ds4 = objEjecucion4.EjecucionGralDs(wresulFactList.ToString(), 104, 1);


                                if (ds4.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                                {

                                }


                            }
                            //Devuelvo solicitud si existen errores en la modificacion
                            return creaArtBapi;

                        }
                        var deserror = fndatosmaestro.GetString("DESERROR");
                        var tablaLog7 = fndatosmaestro.GetTable("PT_LOG");
                        var tablaLog6 = fndatosmaestro.GetTable("PT_ARTICULOS");
                    }
                }
                // *****FIN DE LISTADO DE ARTICULOS *********

            }
            catch (Exception e)
            {

                creaArtBapi.success = false;
                creaArtBapi.mensaje = e.Message.ToString();

            }
            return creaArtBapi;
        }

        public List<DMSolicitudArticulo.CodigoLegacy> ConsultaCodLegacyProv(string codSAPProv)
        { 
            List<DMSolicitudArticulo.CodigoLegacy> lst_codLegacy = new List<DMSolicitudArticulo.CodigoLegacy>();
            DMSolicitudArticulo.CodigoLegacy mod_codLegacy;
            var workBapi = AppConfig.WorkBAPI;
            if (workBapi == "S")
            {
                try { 
                AppConfig.dest.Ping();
                RfcRepository repo = AppConfig.dest.Repository;
                IRfcFunction fndatosmaestro = repo.CreateFunction("ZPPPROVEEDORCHECK");
                var DTPRLIST = fndatosmaestro.GetTable("P_PRLIST");
                IRfcStructure ITPPRLIST;
                ITPPRLIST = repo.GetStructureMetadata("ZWAPPPROVLISTA").CreateStructure();
                codSAPProv = (Convert.ToInt64(codSAPProv) + 10000000000).ToString().Substring(1);
                ITPPRLIST.SetValue("LIFNR", codSAPProv);
                ITPPRLIST.SetValue("BUKRS", "");
                DTPRLIST.Append(ITPPRLIST);


                fndatosmaestro.SetValue("P_PRLIST", DTPRLIST);
                //fndatosmaestro.SetValue("P_MEINH", "ST");
               
                fndatosmaestro.Invoke(AppConfig.dest);
                var coderror = fndatosmaestro.GetString("CODERROR");
                var deserror = fndatosmaestro.GetString("DESERROR");
                
                
                if (coderror != "1")
                {
                    var listaCodLeg = fndatosmaestro.GetTable("PT_PROVLEGACY");
                    var result = (from a in listaCodLeg
                                  select new
                                  {
                                      ID = a.GetString("KOLIF"),
                                      DETALLE = a.GetString("KOLIF"),
                                      
                                  }).ToList();

                    foreach (var i in result)
                    {
                        mod_codLegacy = new DMSolicitudArticulo.CodigoLegacy();
                        mod_codLegacy.codigo = i.ID;
                        mod_codLegacy.detalle = i.DETALLE;
                        lst_codLegacy.Add(mod_codLegacy);
                    }

                }
                }
                catch (Exception e) { }

            }
            else
            {
                mod_codLegacy = new DMSolicitudArticulo.CodigoLegacy();
                mod_codLegacy.codigo = "aaaaaX";
                mod_codLegacy.detalle = "aaaaaX";
                lst_codLegacy.Add(mod_codLegacy);
                mod_codLegacy = new DMSolicitudArticulo.CodigoLegacy();
                mod_codLegacy.codigo = "bbbbX";
                mod_codLegacy.detalle = "bbbbbX";
                lst_codLegacy.Add(mod_codLegacy);

            
            }

            return lst_codLegacy;
        }


        public void registraLogError(String codError, String msjError, String modulo, String codTransaccion, String parametro, String usrSesion)
        {

            try { 
            System.Text.StringBuilder sbTxtLog = new System.Text.StringBuilder("");
            sbTxtLog.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            sbTxtLog.AppendLine();
            sbTxtLog.Append("Fecha Log :  " + DateTime.Now.ToString("yyyyMMdd_HHmmss").ToString());
            sbTxtLog.AppendLine();
            sbTxtLog.Append("Usuario  :  " + usrSesion);
            sbTxtLog.AppendLine();
            sbTxtLog.Append("Modulo :  " + modulo.ToString());
            sbTxtLog.AppendLine();
            sbTxtLog.Append("Código Trx:  " + codTransaccion.ToString());
            sbTxtLog.AppendLine();
            sbTxtLog.Append("Parametro Trx:  " + parametro.ToString());
            sbTxtLog.AppendLine();
            sbTxtLog.Append("Código de error:  " + codError.ToString());
            sbTxtLog.AppendLine();
            sbTxtLog.Append("Mensaje de error:  " + msjError.ToString());
            sbTxtLog.AppendLine();
            sbTxtLog.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            sbTxtLog.AppendLine();
            string RutaDir = HttpContext.Current.Server.MapPath("~/Log/");
            string NombreFile = "Log_PortalProveedores" + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            //string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter outfile = new StreamWriter(RutaDir + NombreFile, true))
            {
                outfile.Write(sbTxtLog.ToString());
            }
            //System.IO.File.WriteAllText(RutaDir + NombreFile, sbTxtLog.ToString());
            }catch(Exception){}
            

        }

    }
}