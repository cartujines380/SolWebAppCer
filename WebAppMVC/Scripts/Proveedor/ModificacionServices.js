'use strict';
app.factory('ModificacionProveedor', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    var baseUrl = ngAuthSettings.apiServiceBaseUri + "Api/";
    return {
        getProveedor: function (CodSap) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaProveedorBapi/?idproveedor=" + CodSap
                 ,
            })
        },
        getProveedorDireccionList: function (CodSap) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/getProveedorDireccionList/?idproveedordir=" + CodSap
                 ,
            })
        },
        getUsuarioZonaList: function (CodSap, Usuario) {
            return $http({
                method: 'GET',
                url: baseUrl + "ContactoProveedor/getUsuarioZonaList/?idproveedorconta=" + CodSap + "&Usuario=" + Usuario
                 ,
            })
        },
        getGrabaContacto: function (Contactos, codSAPproveedor, identificacion, Zonas, accion, almacenes) {

            var SolContacto = {
                'p_SolProvContacto': Contactos,
                'p_CodigoSAP': codSAPproveedor,
                'p_Identificacion': identificacion,
                'p_SolZonas': Zonas,
                'p_Accion': accion,
                'p_ListaAlm': almacenes,
            }
          
            return $http.post(serviceBase + 'Api/ContactoProveedor/ContactoGraba', SolContacto).then(function (response) {

                return response;

            });
        },

        //Lineas de negocios
        getConsLienasNeg: function (CodProv) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaLineasdeNegocios/?CodProveedor=" + CodProv,
            })
        },

        getConsAlmacenes : function (tipoLista) {
            Ruta = serviceBase + "Api/PedConsPedidos/ConsAlmacenes?tipoLista=" + tipoLista;
            return $http.get(Ruta).then(function (results) {
                return results;
            })
        },
        getContactoList: function (CodSap) {

            return $http({
                method: 'GET',
                url: baseUrl + "ContactoProveedor/getProveedorContactoList/?idproveedorconta=" + CodSap
                 ,
            })
        },

      
        getProveedorContactoList: function (CodSap) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/getProveedorContactoList/?idproveedorconta=" + CodSap
                 ,
            })
        },
        getProveedorList: function (IdEmpresa, CodProveedor, Ruc, FechaDesde, FechaHasta) {
            return $http({
                method: 'GET',
                url: baseUrl + "Proveedor/ConsultaProveedor/?IdEmpresa=" + IdEmpresa + "&CodProveedor=" + CodProveedor + "&Ruc=" + Ruc + "&FechaDesde=" + FechaDesde + "&FechaHasta=" + FechaHasta
                 ,
            })
        },
        getBancoList: function (CodPais) {

            return $http({
                method: 'GET',
                url: baseUrl + "Proveedor/ConsultaBanco/?CodPais=" + CodPais
                 ,
            })
        },

        getSolProveedorList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaSolicitud/?IdConSolicitud=" + IdSolicitud,

            })
        },
        getUltSolProveedorList: function (IdSolicitud, CodSapProveedor, Identificacion, STipoIdentificacion, Usuario) {
            return $http({

                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaUltSolProveedor/?IdSolicitud=" + IdSolicitud + "&CodSapProveedor=" + CodSapProveedor + "&Identificacion=" + Identificacion + "&STipoIdentificacion=" + STipoIdentificacion + "&Usuario=" + Usuario,
            })
        },
        getSolDocAdjuntoList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaSolDocAdjunto/?IdAdSolicitud=" + IdSolicitud,
            })
        },
        getSolLineaNegocioList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaLineaNegocio/?IdLinSolicitud=" + IdSolicitud,
            })
        },
        getSolProvBancoList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaSolProvBanco/?IdBancoSolicitud=" + IdSolicitud,
            })
        },
        getSolProvContactoList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaSolProvContacto/?IdContactoSolicitud=" + IdSolicitud,
            })
        },

        getSolProvHistEstadoList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaSolProvHistEstado/?IdHisSolicitud=" + IdSolicitud,
            })
        },
        getSolZonaList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaSolZona/?IdZonaSolicitud=" + IdSolicitud,
            })
        },

        getRamoList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaSolRamo/?IdRamoSolicitud=" + IdSolicitud,

            })
        },

        getViaList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaSolVia/?IdviaSolicitud=" + IdSolicitud,

            })
        },


        getSolProvDireccionList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaSolProvDireccion/?IdDirecSolicitud=" + IdSolicitud,
            })
        },

        getSolProvLineaAdminList: function (IDNIVEL, IDLINEA, IDMODULO, IDUSUARIO) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaLineaAdmin/?IDNIVEL=" + IDNIVEL + "&IDLINEA=" + IDLINEA + "&IDMODULO=" + IDMODULO + "&IDUSUARIO=" + IDUSUARIO,
            })
        },

        getPostSolicitudList: function (SolcitudProveedor, SolContacto, SolBanco, SolDireccion, SolDocAdjunto, ViaPago, SolRamo, SolZona, SolProvHistEstado, SolLineaNegocios) {


            var SolProveedorid = {
                'p_SolProvContacto': SolContacto,
                'p_SolProveedor': SolcitudProveedor,
                'p_SolProvBanco': SolBanco,
                'p_SolProvDireccion': SolDireccion,
                'p_SolDocAdjunto': SolDocAdjunto,
                'p_SolViapago': ViaPago != null ? ViaPago : "",
                'p_SolRamo': SolRamo != null ? SolRamo : "",
                'p_SolProvZona': SolZona != null ? SolZona : "",
                'p_SolProvHistEstado': SolProvHistEstado != null ? SolProvHistEstado : "",
                'p_SolLineasNegocios': SolLineaNegocios
            }
            return $http.post(serviceBase + 'Api/SolicitudProveedor/GrabaSolicitud', SolProveedorid).then(function (response) {

                return response;

            });
        },



        getDescargarArchivos: function (listaArchivos) {

            var Ruta = serviceBase + "Api/Download/DownloadFile/";


            return $http.post(Ruta, listaArchivos).then(function (results) {
                return results;
            });
        },



        getUploadFileSFTP: function (listaArchivos) {

            var Ruta = serviceBase + "Api/UploadSFTP/UploadFileSFTP/";


            return $http.post(Ruta, listaArchivos).then(function (results) {
                return results;
            });

        },
        getrutaarchivos: function (solpath, solarchivo) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/SolicitudBajaArchivo/?solpath=" + solpath + "&solarchivo=" + solarchivo,
            })



        },


        getBandejaSolicitudList: function (bIdentificacion, FecDesde, FecHasta, BEstado, Pantalla, Opcion, BIDLINEA, BIDUSUARIO) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaBandejaSolicitud/?bIdentificacion=" + bIdentificacion + "&FecDesde=" + FecDesde + "&FecHasta=" + FecHasta + "&BEstado=" + BEstado + "&Pantalla=" + Pantalla + "&Opcion=" + Opcion + "&BIDLINEA=" + BIDLINEA + "&BIDUSUARIO=" + BIDUSUARIO,
            })
        },

        getConsTodasZonas : function (errDefZ) {
            return $http({
                method: 'GET',
                url: baseUrl + "SegBandjUsrsAdic/ConsTodasZonas/?errDefZ=" + errDefZ,
            })

           
        },


    }
}]);