using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AngularJSAuthentication.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            string SitiosAutorizados = "*";// "http://localhost:20143";

            var cors = new EnableCorsAttribute(SitiosAutorizados, "*", "*");
            config.EnableCors(cors);



            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            // [ActionName("EstadosPedidos")]
            config.Routes.MapHttpRoute(
            name: "Catalogos",
            routeTemplate: "api/{controller}/{NombreTabla}",
            defaults: new { NombreTabla = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
     name: "cargarParametrosCO",
     routeTemplate: "api/Catalogos/cargarParametrosCO/{parametrosCO}",
     defaults: new { parametros = RouteParameter.Optional }
     );


            // Reporte Administrador
            config.Routes.MapHttpRoute(
            name: "getConsulaGridNoIngreso",
            routeTemplate: "api/ReporteAdministrador/getConsulaGridNoIngreso/{CodSap}/{Usuario}"
            );


            // Reporte Administrador
            config.Routes.MapHttpRoute(
            name: "ConsPedidosCrossConsolidadoExportar",
            routeTemplate: "api/PedPedidosCross/ConsPedidosCrossConsolidadoExportar/{tiposo}/{codsapex}/{nomproveedorex}/{usuariologonso}/{idpedido}"
            );

            config.Routes.MapHttpRoute(
            name: "getActualizarSolicitudEtiquetas",
            routeTemplate: "api/Ped_ConsEtiPedidosActualizar/getActualizarSolicitudEtiquetas/{EtiquetasACtualizar}"
            );
            // Reporte Administrador
            config.Routes.MapHttpRoute(
            name: "getConsulaGridNoOrdenCompra",
            routeTemplate: "api/ReporteAdministrador/getConsulaGridNoOrdenCompra/{CodSapno}/{rucno}"
            );
            // Reporte Acta Recepcion
            config.Routes.MapHttpRoute(
            name: "getConsulaGridActaRecepcion",
            routeTemplate: "api/ReporteAdministrador/getConsulaGridActaRecepcion/{tipo}/{Norden}/{Nfactura}/{Fecha1}/{Fecha2}/{codsapat}/{codigoAlmacen}"
            );

            // Reporte Administrador
            config.Routes.MapHttpRoute(
            name: "getConsulaGridLogComunicacion",
            routeTemplate: "api/ReporteAdministrador/getConsulaGridLogComunicacion/{CodSapno}/{rucno}/{fecha1}/{fecha2}"
            );

            // Reporte Administrador
            config.Routes.MapHttpRoute(
            name: "getConsulaGridProveedorNoSolicitud",
            routeTemplate: "api/ReporteAdministrador/getConsulaGridProveedorNoSolicitud/{CodSapns}/{rucns}"
            );
            config.Routes.MapHttpRoute(
           name: "getPedidoSolicitud",
           routeTemplate: "api/PedConsEtiPedidos/getPedidoSolicitud/{idSolicitud}/{opcion}"
           );


            config.Routes.MapHttpRoute(
         name: "geteliminarSolicitudEti",
         routeTemplate: "api/PedConsEtiPedidos/geteliminarSolicitudEti/{opcionEli}/{idPedidoEli}/{estadoEli}"
         );

            config.Routes.MapHttpRoute(
            name: "ExportarNoIngreso",
            routeTemplate: "api/ReporteAdministrador/ExportarNoIngreso/{tipo}/{usuariologon}/{CodSap}/{Usuario}"
            );

            config.Routes.MapHttpRoute(
           name: "ExportarNoCompra",
           routeTemplate: "api/ReporteAdministrador/ExportarNoCompra/{tipo}/{usuariologon}/{CodSap}/{ruc}"
           );

            config.Routes.MapHttpRoute(
     name: "ExportarArchivoActaRecepcion",
     routeTemplate: "api/ReporteAdministrador/ExportarArchivoActaRecepcion/{idarchivo}/{anio}/{mes}/{dia}"
     );

            config.Routes.MapHttpRoute(
       name: "ExportarLogComunicado",
       routeTemplate: "api/ReporteAdministrador/ExportarLogComunicado/{tipo}/{usuariologon}/{CodSap}/{ruc}/{fecha1}/{fecha2}"
       );

            config.Routes.MapHttpRoute(
            name: "ExportarProveedorNoSolicitud",
            routeTemplate: "api/ReporteAdministrador/ExportarProveedorNoSolicitud/{tipo}/{usuariologonns}/{CodSapns}/{rucns}"
            );


            // Notificaciones
            config.Routes.MapHttpRoute(
            name: "cargaArchivoPDF",
            routeTemplate: "api/FileArchivoPDF/cargaArchivoPDF/{archivoFinal}"
            );


            config.Routes.MapHttpRoute(
             name: "cargainicial",
                 routeTemplate: "api/Notificacion/cargainicial/"
             );


            config.Routes.MapHttpRoute(
             name: "bajararchivPDF",
                 routeTemplate: "api/Notificacion/bajararchivPDF/"
             );

            config.Routes.MapHttpRoute(
             name: "consultaNotificaciones",
                 routeTemplate: "api/Notificacion/consultaNotificaciones/{estado}"
             );

            config.Routes.MapHttpRoute(
               name: "consultaLisProveedor",
                   routeTemplate: "api/Notificacion/consultaLisProveedor/{tipo}/{dato}/{usr}"
                //defaults: new { tipo = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
               name: "grabaNotificacion",
                   routeTemplate: "api/Notificacion/grabaNotificacion/{userModel}"
                //defaults: new { tipo = RouteParameter.Optional }
               );


            config.Routes.MapHttpRoute(
               name: "secuenciaDirectorio",
                   routeTemplate: "api/Notificacion/secuenciaDirectorio/{tipo}"
                //defaults: new { tipo = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
               name: "consultaListaNotProveedor",
                   routeTemplate: "api/Notificacion/consultaListaNotProveedor/{ruc}/{usr}"
                //defaults: new { tipo = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
               name: "actualizaEstado",
                   routeTemplate: "api/Notificacion/actualizaEstado/{codigo}/{estado}/{usuario}/{observacion}"
                //defaults: new { tipo = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
               name: "actualizaEstadoNot",
                   routeTemplate: "api/Notificacion/actualizaEstadoNot/{codNotificacion}/{codProveedor}/{usr}"
                //defaults: new { tipo = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
               name: "agrupaNotificacion",
                   routeTemplate: "api/Notificacion/agrupaNotificacion/{codNotificacion}/{codNotificacion2}"
                //defaults: new { tipo = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
               name: "registraMensaje",
                   routeTemplate: "api/Notificacion/registraMensaje/{usuario}/{correo}/{mensaje}"
                //defaults: new { tipo = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
           name: "consultaListaPrecios",
               routeTemplate: "api/Notificacion/consultaListaPrecios/{tipoLista}/{ruc}"
           );

            config.Routes.MapHttpRoute(
                name: "consultaListaPreciosgeneral",
                    routeTemplate: "api/ListaPrecio/consultaListaPreciosgeneral/{tipoListag}/{rucg}/{regInicial}/{RegFinal}"
                );

            config.Routes.MapHttpRoute(
               name: "consultaClasificados",
                   routeTemplate: "api/Notificacion/consultaClasificados/{tipoLista2}"
               );

            config.Routes.MapHttpRoute(
               name: "registraProveedorgeneral",
                   routeTemplate: "api/ListaPrecio/registraProveedorgeneral/{nombreg}/{emailg}/{telefonog}/{celularg}/{productosg}"
                //defaults: new { tipo = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
              name: "descargaNotificacionG",
                  routeTemplate: "api/ListaPrecio/descargaNotificacionG"
                //defaults: new { tipo = RouteParameter.Optional }
              );

            // Notificaciones


            //Transporte

            config.Routes.MapHttpRoute(
            name: "consBuscarVehiculoUnoRUCCED",
            routeTemplate: "api/MantenimientoVehiculo/consBuscarVehiculoUnoRUCCED/{tipov}/{placav}"
            );


            config.Routes.MapHttpRoute(
            name: "consBuscarChoferesUnoRUCCED",
            routeTemplate: "api/MantenimientoTransporte/consBuscarChoferesUnoRUCCED/{tipo}/{rucced}"
            );

            config.Routes.MapHttpRoute(
            name: "ExportarAprobarCita",
            routeTemplate: "api/ReporteAprobarCita/ExportarAprobarCita/{AprobarCita}"
            );

            config.Routes.MapHttpRoute(
            name: "ExportarCita",
            routeTemplate: "api/ReporteCita/ExportarCita/{Cita}"
            );

            config.Routes.MapHttpRoute(
            name: "ExportarConsolidacion",
            routeTemplate: "api/ReporteConsolidacion/ExportarConsolidacion/{Consolidacion}"
            );

            config.Routes.MapHttpRoute(
            name: "ExportarChofer",
            routeTemplate: "api/ReporteChofer/ExportarChofer/{Chofer}"
            );

            config.Routes.MapHttpRoute(
            name: "ExportarVehiculo",
            routeTemplate: "api/ReporteVehiculo/ExportarVehiculo/{Vehiculo}"
            );

            //config.Routes.MapHttpRoute(
            //name: "ExportarTabular",
            //routeTemplate: "api/ReportesTransporte/ExportarTabular/{Tabla}"
            //);

            config.Routes.MapHttpRoute(
            name: "getTabularCitas",
            routeTemplate: "api/ReportesTransporte/getTabularCitas/{tipo}/{fechadesdeRPT}/{fechahastaRPT}"
            );

            config.Routes.MapHttpRoute(
            name: "grabarCitaRapida",
            routeTemplate: "api/CitaRapida/grabarCitaRapida/{CitaRapida}"
            );

            config.Routes.MapHttpRoute(
            name: "BuscarDatosVarios",
            routeTemplate: "api/CitaRapida/BuscarDatosVarios/{tipod}/{dato}/{ruc}"
            );

            config.Routes.MapHttpRoute(
            name: "BuscarPedidosCitas",
            routeTemplate: "api/CitaRapida/BuscarPedidosCitas/{tipoCita}/{rucCita}"
            );

            config.Routes.MapHttpRoute(
            name: "BuscarProveedorDatos",
            routeTemplate: "api/CitaRapida/BuscarProveedorDatos/{tipo}/{ruc}"
            );

            config.Routes.MapHttpRoute(
            name: "getGrabarCancelarProveedor",
            routeTemplate: "api/AprobacionCitasProveedor/getGrabarCancelarProveedor/{id}/{codigo}/{usuarioCreacion}/{idcita}"
            );

            config.Routes.MapHttpRoute(
            name: "getGrabarCancelar",
            routeTemplate: "api/AprobacionCitas/getGrabarCancelar/{id}/{codigo}/{usuarioCreacion}/{idcita}"
            );

            config.Routes.MapHttpRoute(
            name: "getdatos",
            routeTemplate: "api/AprobacionCitas/getdatos/{id}/{citas}"
            );

            config.Routes.MapHttpRoute(
            name: "conBuscarDetalleGridProveedor",
            routeTemplate: "api/AprobacionCitasProveedor/conBuscarDetalleGridProveedor/{tipo}/{id}"
            );

            config.Routes.MapHttpRoute(
            name: "conBuscarDetalleGrid",
            routeTemplate: "api/AprobacionCitas/conBuscarDetalleGrid/{tipo}/{id}"
            );

            config.Routes.MapHttpRoute(
            name: "conBuscarAprobacionCitas",
            routeTemplate: "api/AprobacionCitas/conBuscarAprobacionCitas/{tipo}/{fechadesde}/{fechahasta}/{numero}/{estado}/{codProveedor}/{ruc}"
            );

            config.Routes.MapHttpRoute(
            name: "conBuscarAprobacionCitasProveedor",
            routeTemplate: "api/AprobacionCitasProveedor/conBuscarAprobacionCitasProveedor/{tipo}/{fechadesde}/{fechahasta}/{numero}/{estado}/{codProveedor}/{ruc}"
            );

            config.Routes.MapHttpRoute(
            name: "conBuscarSolicitud",
            routeTemplate: "api/SolicitudCita/conBuscarSolicitud/{tipo}/{fechadesde}/{fechahasta}/{numero}/{codProveedor}"
            );

            config.Routes.MapHttpRoute(
            name: "generarCita",
            routeTemplate: "api/SolicitudCita/generarCita/{idconsolidacion}/{fechacita}/{horainicial}/{horafinal}/{CodSap}/{usuarioproveedor}"
            );


            config.Routes.MapHttpRoute(
            name: "BuscarConsolidacionAdmin",
            routeTemplate: "api/ConsolidacionPedidos/BuscarConsolidacionAdmin/{numeroAdmin}/{codProveedorConsoAdmin}/{estadoAdmin}/{fechadesdeAdmin}/{fechahastaAdmin}"
            );

            config.Routes.MapHttpRoute(
            name: "BuscarConsolidacion",
            routeTemplate: "api/ConsolidacionPedidos/BuscarConsolidacion/{numero}/{codProveedorConso}/{estado}/{fechadesde}/{fechahasta}"
            );

            config.Routes.MapHttpRoute(
            name: "BuscarConModi",
            routeTemplate: "api/ConsolidacionPedidos/BuscarConModi/{idconsomodi}/{codproveedormodi}"
            );

            config.Routes.MapHttpRoute(
            name: "EliminarConsolidacion",
            routeTemplate: "api/ConsolidacionPedidos/EliminarConsolidacion/{idconsolidacion}/{codProveedorEliConso}"
            );


            config.Routes.MapHttpRoute(
            name: "ChoferVehiculo",
            routeTemplate: "api/ConsolidacionPedidos/ChoferVehiculo/{tipoid}/{codproveedor}"
            );

            config.Routes.MapHttpRoute(
            name: "BuscarPedidos",
            routeTemplate: "api/ConsolidacionPedidos/BuscarPedidos/{tipoPe}/{codproveedorPe}"
            );

            config.Routes.MapHttpRoute(
            name: "BuscarDetPedidos",
            routeTemplate: "api/ConsolidacionPedidos/BuscarDetPedidos/{tipoPeD}/{numPedido}"
            );

            config.Routes.MapHttpRoute(
            name: "grabarConsolidacion",
            routeTemplate: "api/ConsolidacionPedidos/grabarConsolidacion/{Consolidacion}"
            );

            config.Routes.MapHttpRoute(
           name: "bajararchivo",
           routeTemplate: "api/MantenimientoTransporte/bajararchivo/{identificacion}/{archivo}"
           );


            config.Routes.MapHttpRoute(
               name: "consBuscarChoferes",
                   routeTemplate: "api/MantenimientoTransporte/consBuscarChoferes/{tipo}/{licencia}/{nombre}/{apellido}/{tipoestado}/{CodSap}"
                //defaults: new { tipo = RouteParameter.Optional, codigo = RouteParameter.Optional, chkCodRef = RouteParameter.Optional, CodRef = RouteParameter.Optional, chkCodSap = RouteParameter.Optional, CodSap = RouteParameter.Optional , chkFecha = RouteParameter.Optional , FechaDesde = RouteParameter.Optional,FechaHasta = RouteParameter.Optional, chkEstado = RouteParameter.Optional, Estado = RouteParameter.Optional, chkTipoSol = RouteParameter.Optional, TipoSolicitud = RouteParameter.Optional}
               );
            config.Routes.MapHttpRoute(
             name: "consBuscarChoferesUno",
                 routeTemplate: "api/MantenimientoTransporte/consBuscarChoferesUno/{tipo}/{idChofer}/{CodSap}"
             );

            config.Routes.MapHttpRoute(
            name: "grabarTransporte",
                routeTemplate: "api/MantenimientoTransporte/grabarTransporte/{userModel}"
                //defaults: new { userModel = RouteParameter.Optional }
            );


            config.Routes.MapHttpRoute(
              name: "consBuscarChoferesAdmi",
                  routeTemplate: "api/MantenimientoTransporte/consBuscarChoferesAdmi/{tipo}/{licencia}/{nombre}/{apellido}/{rucproveedor}/{CodSap}"
                //defaults: new { tipo = RouteParameter.Optional, codigo = RouteParameter.Optional, chkCodRef = RouteParameter.Optional, CodRef = RouteParameter.Optional, chkCodSap = RouteParameter.Optional, CodSap = RouteParameter.Optional , chkFecha = RouteParameter.Optional , FechaDesde = RouteParameter.Optional,FechaHasta = RouteParameter.Optional, chkEstado = RouteParameter.Optional, Estado = RouteParameter.Optional, chkTipoSol = RouteParameter.Optional, TipoSolicitud = RouteParameter.Optional}
              );

            config.Routes.MapHttpRoute(
          name: "consBuscarVehiculo",
              routeTemplate: "api/MantenimientoVehiculo/consBuscarVehiculo/{tipo}/{tipovehiculo}/{propietario}/{numplaca}/{tipoestado}/{CodSap}"
          );

            config.Routes.MapHttpRoute(
            name: "consBuscarVehiculoUno",
                routeTemplate: "api/MantenimientoVehiculo/consBuscarVehiculoUno/{tipo}/{idVehiculo}/{CodSap}"
            );

            config.Routes.MapHttpRoute(
            name: "grabarVehiculo",
            routeTemplate: "api/MantenimientoVehiculo/grabarVehiculo/{tipo}/{tipovehiculo}/{propietario}/{numplaca}/{tipoestado}/{CodSap}"
            );


            config.Routes.MapHttpRoute(
            name: "bajararchivoVehiculo",
            routeTemplate: "api/MantenimientoVehiculo/bajararchivoVehiculo/{identificacion}/{archivo}"
            );


            //Fin Trasnporte


            //*********************** Artículo ************************
            config.Routes.MapHttpRoute(
            name: "ReporteSolicitudArt",
                //Solicitud -> es un arreglo que contiene toda la solicitud generar reporte
            routeTemplate: "api/ReporteSolAdmArticulo/ReporteSolicitudArticulo/{SolicitudRep}",
                   defaults: new { }
            );

            config.Routes.MapHttpRoute(
            name: "ExportarSolArticulo",
            routeTemplate: "api/ReporteSolArticulo/ExportarSolArticulo/{ReporteArticulo}"
            );

            config.Routes.MapHttpRoute(
            name: "SolicitudArt",
            routeTemplate: "api/Art_Solicitud/ConsSolArticulo/{tipo}/{codigo}/{chkCodRef}/{CodRef}/{chkCodSap}/{CodSap}/{chkFecha}/{FechaDesde}/{FechaHasta}/{chkEstado}/{Estado}/{chkTipoSol}/{TipoSolicitud}/{chkLinea}/{LineaNegocio}/{ArtUsuario}/{ArtNivel}/{CodProveedorCons}",
                   defaults: new { }
            );

            config.Routes.MapHttpRoute(
            name: "ConsultaArt",
            routeTemplate: "api/Art_Consulta/ConsArticulo/{tipo}/{codigo}/{chkCodRef}/{CodRef}/{chkCodSap}/{CodSap}/{chkGrupoArt}/{GrupoArt}/{chkLinea}/{LineaNegocio}",
                   defaults: new { }
            );

            config.Routes.MapHttpRoute(
            name: "GrabaSolArt",
                //Solicitud -> es un arreglo que contiene toda la solicitud
            routeTemplate: "api/Art_Solicitud/GrabaSolArticulo/{Solicitud}",
                   defaults: new { }
            );


            config.Routes.MapHttpRoute(
            name: "BajaTempArch",
            routeTemplate: "api/Art_Solicitud/BajaTempArchivo/{path}/{archivo}",
                   defaults: new { }
            );

            config.Routes.MapHttpRoute(
            name: "BajaFptArch",
            routeTemplate: "api/Art_Solicitud/BajaFptArchivo/{path_comp}/{nom_archivo}/{aux}",
                   defaults: new { }
            );

            config.Routes.MapHttpRoute(
            name: "CargaMasiva",
            routeTemplate: "api/Art_Solicitud/CargaMasiva/{pathmasivo}/{nomarchivo}/{aux1}/{aux2}",
                   defaults: new { }
            );

            config.Routes.MapHttpRoute(
            name: "ConsultaEAN",
            routeTemplate: "api/Art_Solicitud/ConsultaEAN/{codEANart}/{tipoEANart}",
                   defaults: new { }
            );

            config.Routes.MapHttpRoute(
            name: "BajaPlantilla",
            routeTemplate: "api/Art_Solicitud/BajaPlantilla",
            defaults: new { }
            );
            //*********************** Artículo ************************



            //******************************************************************************************************************
            //****  cvera  *****************************************************************************************************
            //******************************************************************************************************************
            config.Routes.MapHttpRoute(
                name: "ConsBandjUsrsAdmin",
                routeTemplate: "api/SegBandjUsrsAdmin/ConsBandjUsrsAdmin/{CodSap}/{Ruc}/{Nombre}/{ConUsuario}/{Estado}",
                defaults: new { CodSap = RouteParameter.Optional, Ruc = RouteParameter.Optional, Nombre = RouteParameter.Optional, ConUsuario = RouteParameter.Optional, Estado = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "GrabarUsrAdmin",
                routeTemplate: "api/SegBandjUsrsAdmin/GrabarUsrAdmin/{userModel}",
                defaults: new { userModel = RouteParameter.Optional }
            );
            //******************************************************************************************************************
            config.Routes.MapHttpRoute(
                name: "CatalogosFS",
                routeTemplate: "api/Seguridad/CatalogosFS/{NombreTablaFS}",
                defaults: new { NombreTablaFS = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ConsValUsrFirstLogon",
                routeTemplate: "api/SegUsrFirstLogon/ConsValUsrFirstLogon/{Ruc}/{Usuario}",
                defaults: new { Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ValidaEmailUsrFirstLogon",
                routeTemplate: "api/SegUsrFirstLogon/ValidaEmailUsrFirstLogon/{CorreoE}/{CodigoValidacion}/{NombreUsuario}",
                defaults: new { CorreoE = RouteParameter.Optional, CodigoValidacion = RouteParameter.Optional, NombreUsuario = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "GrabarUsrFirstLogon",
                routeTemplate: "api/SegUsrFirstLogon/GrabarUsrFirstLogon/{userModel}",
                defaults: new { userModel = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Menus",
                routeTemplate: "api/Seguridad/Menus/{clientId}/{userName}",
                 defaults: new { clientId = RouteParameter.Optional, userName = RouteParameter.Optional }
            );

            //******************************************************************************************************************
            config.Routes.MapHttpRoute(
                   name: "ConsBandjUsrsAdic",
                   routeTemplate: "api/SegBandjUsrsAdic/ConsBandjUsrsAdic/{Ruc}/{Usuario}/{Nombre}/{Apellido}/{Estado}/{RecActas}",
                   defaults: new { Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional, Nombre = RouteParameter.Optional, Apellido = RouteParameter.Optional, Estado = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
              name: "ConsActivarusuario",
              routeTemplate: "api/SegBandjUsrsAdic/ConsActivarusuario/{RucActivar}/{UsuarioActivar}/{tipo}",
              defaults: new { Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional }
          );
            config.Routes.MapHttpRoute(
                name: "ConsDatosUsrsAdic",
                routeTemplate: "api/SegBandjUsrsAdic/ConsDatosUsrsAdic/{Ruc}/{Usuario}",
                defaults: new { Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ConsTodasZonas",
                routeTemplate: "api/SegBandjUsrsAdic/ConsTodasZonas/{errDefZ}",
                defaults: new { errDefZ = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ConsTodosRoles",
                routeTemplate: "api/SegBandjUsrsAdic/ConsTodosRoles/{errDefR}",
                defaults: new { errDefR = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "GrabarUsrAdic",
                routeTemplate: "api/SegBandjUsrsAdic/GrabarUsrAdic/{userModel}",
                defaults: new { userModel = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ResetClaveUsrAdic",
                routeTemplate: "api/SegBandjUsrsAdic/ResetClaveUsrAdic/{Ruc}/{Usuario}/{Clave}/{Correo}/{NombreUsuario}",
                defaults: new { Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional, Clave = RouteParameter.Optional, Correo = RouteParameter.Optional, NombreUsuario = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DesbloquearClaveUsrAdic",
                routeTemplate: "api/SegBandjUsrsAdic/DesbloquearClaveUsrAdic/{Ruc}/{Usuario}/{Correo}/{NombreUsuario}",
                defaults: new { Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional, Correo = RouteParameter.Optional, NombreUsuario = RouteParameter.Optional }
            );
            //J. Navarrete 16-01-2016
            config.Routes.MapHttpRoute(
                name: "ConsDatosLegAsociados",
                routeTemplate: "api/SegBandjUsrsAdic/ConsDatosLegAsociados/{Tipo}/{Ruc}/{Nombres}",
                defaults: new { Tipo = RouteParameter.Optional, Ruc = RouteParameter.Optional, Nombres = RouteParameter.Optional }
            );

            //J. Navarrete 16-01-2016
            config.Routes.MapHttpRoute(
                name: "ActDatosLegAsociados",
                routeTemplate: "api/SegBandjUsrsAdic/ActDatosLegAsociados/{Tipo}/{Ruc}/{Usuario}/{Cedula}/{CodLegacy}/{UserLegacy}",
                defaults: new { Tipo = RouteParameter.Optional, Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional, Cedula = RouteParameter.Optional, CodLegacy = RouteParameter.Optional, UserLegacy = RouteParameter.Optional }
            );


            //******************************************************************************************************************
            config.Routes.MapHttpRoute(
                name: "CambiarClave",
                routeTemplate: "api/SegCambioClave/CambiarClave/{Ruc}/{Usuario}/{ClaveAct}/{ClaveNew}/{Correo}/{NombreUsr}/{NomComercial}",
                defaults: new { Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional, ClaveAct = RouteParameter.Optional, ClaveNew = RouteParameter.Optional, Correo = RouteParameter.Optional, NombreUsr = RouteParameter.Optional, NomComercial = RouteParameter.Optional }
            );
            //******************************************************************************************************************
            config.Routes.MapHttpRoute(
                name: "RecuperaClaveValidar",
                routeTemplate: "api/SegRecuperaClave/RecuperaClaveValidar/{Ruc}/{Correo}/{Usuario}",
                defaults: new { Ruc = RouteParameter.Optional, Correo = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "RecuperaClaveEnviarTmp",
                routeTemplate: "api/SegRecuperaClave/RecuperaClaveEnviarTmp/{Ruc}/{Usuario}/{ClaveTmp}/{Correo}/{NombreUsr}",
                defaults: new { Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional, ClaveTmp = RouteParameter.Optional, Correo = RouteParameter.Optional, NombreUsr = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "RecuperaClaveCambiar",
                routeTemplate: "api/SegRecuperaClave/RecuperaClaveCambiar/{Ruc}/{Usuario}/{Clave}/{Correo}/{NombreUsr}",
                defaults: new { Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional, Clave = RouteParameter.Optional, Correo = RouteParameter.Optional, NombreUsr = RouteParameter.Optional }
            );
            //******************************************************************************************************************
            config.Routes.MapHttpRoute(
                name: "ConsCiudadesEnAlmacen",
                routeTemplate: "api/PedConsPedidos/ConsCiudadesEnAlmacen"
            );

            config.Routes.MapHttpRoute(
                name: "ConsAlmacenes",
                routeTemplate: "api/PedConsPedidos/ConsAlmacenes/{TipoLista}",
                defaults: new
                {
                    TipoLista = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
            name: "exptPedidosFiltroupd",
            routeTemplate: "api/PedConsPedidos/exptPedidosFiltroupd/{CodSapn}/{Rucn}/{Usuarion}/{Opc1n}/{Opc2n}/{Fecha1n}/{Fecha2n}/{Ciudadn}/{NumOrdenn}/{Almacenn}/{Tipo}"
        );

            config.Routes.MapHttpRoute(
            name: "exptPedidosFiltro",
            routeTemplate: "api/PedConsPedidos/exptPedidosFiltro/{Lista}"
            );

            config.Routes.MapHttpRoute(
                name: "ConsPedidosFiltro",
                routeTemplate: "api/PedConsPedidos/ConsPedidosFiltro/{CodSap}/{Ruc}/{Usuario}/{Opc1}/{Opc2}/{Fecha1}/{Fecha2}/{Ciudad}/{NumOrden}/{SiGrd}/{SiTxt}/{SiXml}/{SiHtml}/{SiPdf}/{Almacen}",
                defaults: new
                {
                    CodSap = RouteParameter.Optional,
                    Ruc = RouteParameter.Optional,
                    Usuario = RouteParameter.Optional,
                    Opc1 = RouteParameter.Optional,
                    Opc2 = RouteParameter.Optional,
                    Fecha1 = RouteParameter.Optional,
                    Fecha2 = RouteParameter.Optional,
                    Ciudad = RouteParameter.Optional,
                    NumOrden = RouteParameter.Optional,
                    SiGrd = RouteParameter.Optional,
                    SiTxt = RouteParameter.Optional,
                    SiXml = RouteParameter.Optional,
                    SiHtml = RouteParameter.Optional,
                    SiPdf = RouteParameter.Optional,
                    Almacen = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "UrlRedireccSiteFactManual",
                routeTemplate: "api/PedConsPedidos/UrlRedireccSiteFactManual/{Opc}/{CodSap}/{Ruc}/{Usuario}",
                defaults: new { Opc = RouteParameter.Optional, CodSap = RouteParameter.Optional, Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                  name: "ConsSelPedidosFiltro",
                  routeTemplate: "api/FacFactNoElec/ConsSelPedidosFiltro/{CodSap}/{Ruc}/{Usuario}/{NumPedido}/{FechaIni}/{FechaFin}/{Estados}/{Almacen}",
                  defaults: new
                  {
                      CodSap = RouteParameter.Optional,
                      Ruc = RouteParameter.Optional,
                      Usuario = RouteParameter.Optional,
                      NumPedido = RouteParameter.Optional,
                      FechaIni = RouteParameter.Optional,
                      FechaFin = RouteParameter.Optional,
                      Estados = RouteParameter.Optional,
                      Almacen = RouteParameter.Optional

                  }
              );
            config.Routes.MapHttpRoute(
            name: "consProveedorExcepto",
            routeTemplate: "api/FacFactNoElec/consProveedorExcepto/{proveedorExcepto}",
            defaults: new { proveedorExcepto = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
            name: "cargarParametros",
            routeTemplate: "api/FacFactNoElec/cargarParametros/{parametros}",
            defaults: new { parametros = RouteParameter.Optional }
            );


            config.Routes.MapHttpRoute(
                    name: "ConsRecupFacturasFiltro",
                    routeTemplate: "api/FacFactNoElec/ConsRecupFacturasFiltro/{CodSap}/{Ruc}/{Usuario}/{NumPedido}/{FacEst}/{FacPto}/{FacSec}/{FecEsReg}/{FechaIni}/{FechaFin}/{Estados}/{CodAlmacen}",
                    defaults: new
                    {
                        CodSap = RouteParameter.Optional,
                        Ruc = RouteParameter.Optional,
                        Usuario = RouteParameter.Optional,
                        NumPedido = RouteParameter.Optional,
                        FacEst = RouteParameter.Optional,
                        FacPto = RouteParameter.Optional,
                        FacSec = RouteParameter.Optional,
                        FecEsReg = RouteParameter.Optional,
                        FechaIni = RouteParameter.Optional,
                        FechaFin = RouteParameter.Optional,
                        Estados = RouteParameter.Optional,
                        CodAlmacen = RouteParameter.Optional
                    }
                );

            config.Routes.MapHttpRoute(
                name: "ConsultaPedidoNumero",
                routeTemplate: "api/FacFactNoElec/ConsultaPedidoNumero/{CodSap}/{Ruc}/{Usuario}/{IdPedido}",
                defaults: new { CodSap = RouteParameter.Optional, Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional, IdPedido = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ConsultaDocumentoId",
                routeTemplate: "api/FacFactNoElec/ConsultaDocumentoId/{CodSap}/{Ruc}/{Usuario}/{IdDocumento}",
                defaults: new { CodSap = RouteParameter.Optional, Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional, IdDocumento = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "GrabaDocumento",
                routeTemplate: "api/FacFactNoElec/GrabaDocumento/{objData}",
                defaults: new { objData = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "GrabaAnulaDocumento",
                routeTemplate: "api/FacFactNoElec/GrabaAnulaDocumento/{CodSap}/{Ruc}/{Usuario}/{IdDocumentoAnula}",
                defaults: new { CodSap = RouteParameter.Optional, Ruc = RouteParameter.Optional, Usuario = RouteParameter.Optional, IdDocumentoAnula = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ValidaPermisoModFact",
                routeTemplate: "api/FacFactNoElec/ValidaPermisoModFact/{CodSap}/{Ruc}/{Usuario}/{NumPedido}/{FacEstabl}/{FacPtoEmi}/{FacNumSec}",
                defaults: new
                {
                    CodSap = RouteParameter.Optional,
                    Ruc = RouteParameter.Optional,
                    Usuario = RouteParameter.Optional,
                    NumPedido = RouteParameter.Optional,
                    FacEstabl = RouteParameter.Optional,
                    FacPtoEmi = RouteParameter.Optional,
                    FacNumSec = RouteParameter.Optional
                }
            );
            //config.Routes.MapHttpRoute(
            //    name: "GeneraFileTextoXML",
            //    routeTemplate: "api/FacFactNoElec/GeneraFileTextoXML/{objTxtXML}",
            //    defaults: new { objTxtXML = RouteParameter.Optional }
            //);
            //config.Routes.MapHttpRoute(
            //    name: "GeneraFilePDF",
            //    routeTemplate: "api/FacFactNoElec/GeneraFilePDF/{objPDF}",
            //    defaults: new { objPDF = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
                name: "GeneraFilesXmlPDF",
                routeTemplate: "api/FacFactNoElec2/GeneraFilesXmlPDF/{objInfo}",
                defaults: new { objInfo = RouteParameter.Optional }
            );
            //******************************************************************************************************************
            //****  cvera  *****************************************************************************************************
            //******************************************************************************************************************


            //******************************************************************************************************************
            //****  ECETRE  *****************************************************************************************************
            //******************************************************************************************************************



            //Proveedor
            //http://localhost:26264/api/Proveedor/ConsultaProveedor/?IdEmpresa=1&CodProveedor=&Ruc=0100036219001&FechaDesde=&FechaHasta=
            config.Routes.MapHttpRoute(
            name: "ConsultaProveedor",
            routeTemplate: "api/Proveedor/ConsultaProveedor/{IdEmpresa}/{CodProveedor}/{Ruc}/{FechaDesde}/{FechaHasta}",
            defaults: new { IdEmpresa = RouteParameter.Optional, CodProveedor = RouteParameter.Optional, Ruc = RouteParameter.Optional, FechaDesde = RouteParameter.Optional, FechaHasta = RouteParameter.Optional }
            );
            //String IdEmpresa,String CodProveedor,String Ruc, String FechaDesde,String FechaHasta  

            //http://localhost:26264/api/Proveedor/ConsultaBanco/?CodPais=
            config.Routes.MapHttpRoute(
            name: "ConsultaBanco",
            routeTemplate: "api/Proveedor/ConsultaBanco/{CodPais}",
            defaults: new { CodPais = RouteParameter.Optional }
            );



            ////http://localhost:26264/api/Proveedor/ConsultaBanco/?CodPais=
            //config.Routes.MapHttpRoute(
            //name: "PruebaBapi",
            //routeTemplate: "api/Proveedor/PruebaBapi"//,
            ////defaults: new { CodPais = RouteParameter.Optional }
            //);



            //http://localhost:26264/api/SolicitudProveedor/ConsultaSolicitud/?IdConSolicitud=1
            config.Routes.MapHttpRoute(
            name: "ConsultaSolicitud",
            routeTemplate: "api/SolicitudProveedor/ConsultaSolicitud/{IdConSolicitud}",
            defaults: new { IdConSolicitud = RouteParameter.Optional }
            );

            //http://localhost:26264/api/SolicitudProveedor/ConsultaUltSolProveedor/?IdSolicitud=1&CodSapProveedor=0&Identificacion=0&STipoIdentificacion=0
            config.Routes.MapHttpRoute(
            name: "ConsultaUltSolProveedor",
            routeTemplate: "api/SolicitudProveedor/ConsultaUltSolProveedor/{IdSolicitud}/{CodSapProveedor}/{Identificacion}/{STipoIdentificacion}",
            defaults: new { IdSolicitud = RouteParameter.Optional, CodSapProveedor = RouteParameter.Optional, Identificacion = RouteParameter.Optional, STipoIdentificacion = RouteParameter.Optional }
            );

            //http://localhost:26264/api/SolicitudProveedor/ConsultaSolDocAdjunto/?IdAdSolicitud=1
            config.Routes.MapHttpRoute(
            name: "ConsultaSolDocAdjunto",
            routeTemplate: "api/SolicitudProveedor/ConsultaSolDocAdjunto/{IdAdSolicitud}",
            defaults: new { IdAdSolicitud = RouteParameter.Optional }
            );


            //http://localhost:26264/api/SolicitudProveedor/ConsultaLineaNegocio/?IdLinSolicitud=1
            config.Routes.MapHttpRoute(
            name: "ConsultaLineaNegocio",
            routeTemplate: "api/SolicitudProveedor/ConsultaLineaNegocio/{IdLinSolicitud}",
            defaults: new { IdLinSolicitud = RouteParameter.Optional }
            );


            //http://localhost:26264/api/SolicitudProveedor/ConsultaSolProvBanco/?IdBancoSolicitud=1
            config.Routes.MapHttpRoute(
            name: "ConsultaSolProvBanco",
            routeTemplate: "api/SolicitudProveedor/ConsultaSolProvBanco/{IdBancoSolicitud}",
            defaults: new { IdBancoSolicitud = RouteParameter.Optional }
            );

            //http://localhost:26264/api/SolicitudProveedor/ConsultaSolProvContacto/?IdContactoSolicitud=1
            config.Routes.MapHttpRoute(
            name: "ConsultaSolProvContacto",
            routeTemplate: "api/SolicitudProveedor/ConsultaSolProvContacto/{IdContactoSolicitud}",
            defaults: new { IdContactoSolicitud = RouteParameter.Optional }
            );


            //http://localhost:26264/api/SolicitudProveedor/ConsultaSolProvHistEstado/?IdHisSolicitud=1
            config.Routes.MapHttpRoute(
            name: "ConsultaSolProvHistEstado",
            routeTemplate: "api/SolicitudProveedor/ConsultaSolProvHistEstado/{IdHisSolicitud}",
            defaults: new { IdHisSolicitud = RouteParameter.Optional }
            );


            //http://localhost:26264/api/SolicitudProveedor/ConsultaSolZona/?IdZonaSolicitud=1
            config.Routes.MapHttpRoute(
            name: "ConsultaSolZona",
            routeTemplate: "api/SolicitudProveedor/ConsultaSolZona/{IdZonaSolicitud}",
            defaults: new { IdZonaSolicitud = RouteParameter.Optional }
            );

            //http://localhost:26264/api/SolicitudProveedor/ConsultaSolProvDireccion/?IdDirecSolicitud=1
            config.Routes.MapHttpRoute(
            name: "ConsultaSolProvDireccion",
            routeTemplate: "api/SolicitudProveedor/ConsultaSolProvDireccion/{IdDirecSolicitud}",
            defaults: new { IdDirecSolicitud = RouteParameter.Optional }
            );

            //        //http://localhost:26264/api/SolicitudProveedor/GrabaSolicitud/?SolProveedor=&SolContacto=&SolBanco=&SolDireccion=
            //        config.Routes.MapHttpRoute(
            //name: "GrabaSolicitud",
            //routeTemplate: "api/SolicitudProveedor/GrabaSolicitud/{SolcitudProveedor}/{SolContacto}/{SolBanco}/{SolDireccion}",
            //defaults: new { SolcitudProveedor = RouteParameter.Optional, SolContacto = RouteParameter.Optional, SolBanco = RouteParameter.Optional, SolDireccion = RouteParameter.Optional, }
            // );


            //http://localhost:26264/api/SolicitudProveedor/GrabaSolicitud/?SolProveedor=&SolContacto=&SolBanco=&SolDireccion=
            config.Routes.MapHttpRoute(
            name: "GrabaSolicitud",
            routeTemplate: "api/SolicitudProveedor/GrabaSolicitud/{SolProveedor}",
            defaults: new { SolProveedor = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
            name: "ContactoGraba",
            routeTemplate: "api/ContactoProveedor/ContactoGraba/{SolContacto}",
            defaults: new { SolContacto = RouteParameter.Optional }

            );

            config.Routes.MapHttpRoute(
                name: "ProveedorContactoList",
            routeTemplate: "api/ContactoProveedor/ProveedorContactoList/{idproveedorconta}"


                        );
            config.Routes.MapHttpRoute(
                name: "UsuarioZonaList",
            routeTemplate: "api/ContactoProveedor/UsuarioZonaList/{idproveedorconta}/{Usuario}"


                        );

            config.Routes.MapHttpRoute(
            name: "ConsultaBandejaSolicitud",
            routeTemplate: "api/SolicitudProveedor/ConsultaBandejaSolicitud/{bIdentificacion}/{FecDesde}/{FecHasta}/{BEstado}/{Pantalla}/{Opcion}/{BIDLINEA}/{BIDUSUARIO}",
            defaults: new
            {
                bIdentificacion = RouteParameter.Optional,
                FecDesde = RouteParameter.Optional,
                FecHasta = RouteParameter.Optional,
                BEstado = RouteParameter.Optional,
                Pantalla = RouteParameter.Optional,
                Opcion = RouteParameter.Optional,
                BIDLINEA = RouteParameter.Optional,
                BIDUSUARIO = RouteParameter.Optional

            }
            );


            config.Routes.MapHttpRoute(
            name: "SolicitudBajaArchivo",
            routeTemplate: "api/SolicitudProveedor/SolicitudBajaArchivo/{solpath}/{solarchivo}",
            defaults: new
            {
                solpath = RouteParameter.Optional,
                solarchivo = RouteParameter.Optional


            }
            );

            config.Routes.MapHttpRoute(
            name: "ConsultaSolRamo",
            routeTemplate: "api/SolicitudProveedor/ConsultaSolRamo/{IdRamoSolicitud}",
            defaults: new
            {
                IdRamoSolicitud = RouteParameter.Optional
            }
            );

            config.Routes.MapHttpRoute(
            name: "ConsultaSolVia",
            routeTemplate: "api/SolicitudProveedor/ConsultaSolVia/{IdviaSolicitud}",
            defaults: new
            {
                IdviaSolicitud = RouteParameter.Optional
            }
            );


            //http://localhost:26264/api/Proveedor/ConsultaBanco/?CodPais=
            config.Routes.MapHttpRoute(
            name: "ConsultaLineaAdmin",
            routeTemplate: "api/SolicitudProveedor/ConsultaLineaAdmin/{IDNIVEL}/{IDLINEA}/{IDMODULO}/{IDUSUARIO}",
            defaults: new
            {
                IDNIVEL = RouteParameter.Optional,
                IDLINEA = RouteParameter.Optional,
                IDMODULO = RouteParameter.Optional,
                IDUSUARIO = RouteParameter.Optional
            }
            );

            ///************


            //******************************************************************************************************************
            //****  ECETRE  *****************************************************************************************************
            //******************************************************************************************************************
            /********* J Navarrete - Mod. Contingencia - Consulta y Envío de Pedidos *********/
            config.Routes.MapHttpRoute(
                           name: "ConsEnvPedidos",
                           routeTemplate: "api/PedConsPedidos/ConsEnvPedidos/{tipo}/{FechaIni}/{FechaFin}/{Ruc}/{SiTxt}/{SiXml}/{SiPdf}/{accion}/{codProveedor}/{numPedido}/{ConsPestados}/{ConsP2}/{Destinatarios}",
                           defaults: new
                           {
                               Ruc = RouteParameter.Optional,
                               SiTxt = RouteParameter.Optional,
                               SiXml = RouteParameter.Optional,
                               SiPdf = RouteParameter.Optional
                           }
                       );


            /********* J Navarrete - Mod. Contingencia - Consulta y Envío de Pedidos *********/


            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
