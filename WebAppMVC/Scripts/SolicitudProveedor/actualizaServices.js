
'use strict';
app.factory('SolicitudProveedor', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {
    var baseUrl = ngAuthSettings.apiServiceBaseUri + "Api/";
    return {

        getProveedorList: function (IdEmpresa, CodProveedor, Ruc, FechaDesde, FechaHasta) {
            return $http({
                method: 'GET',
                url: baseUrl + "Proveedor/ConsultaProveedor/?IdEmpresa=" + IdEmpresa + "&CodProveedor=" + CodProveedor + "&Ruc=" + Ruc + "&FechaDesde=" + FechaDesde + "&FechaHasta=" + FechaHasta
                ,
            })
        },

        getProveedorSList: function (IdEmpresa, CodProveedor, Ruc, FechaDesde, FechaHasta) {
            return $http({
                method: 'GET',
                url: baseUrl + "Proveedor/ConsultaProveedorSol/?CodProveedor=" + CodProveedor + "&IdEmpresa=" + IdEmpresa
                ,
            })
        },

        getContactoSList: function (CodSap) {
            return $http({
                method: 'GET',
                url: baseUrl + "ContactoProveedor/ProveedorContactoList/?idproveedorconta=" + CodSap
                ,
            })
        },

        getSolProvBancoSList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "Proveedor/ConsultaSolProvBanco/?CodProveedor=" + IdSolicitud,
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

        getDocAdjuntoList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "Proveedor/ConsultaDocAdjunto/?IdAdSolicitud=" + IdSolicitud,
            })
        },
        //Lineas de negocios
        getSolLienasNeg: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaSolLineasdeNegocios/?IdSolicitud=" + IdSolicitud,
            })
        },

        //Lineas de negocios
        getConsLienasNeg: function (CodProv) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaLineasdeNegocios/?CodProveedor=" + CodProv,
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

        getProvDireccionList: function (IdSolicitud) {
            return $http({
                method: 'GET',
                url: baseUrl + "Proveedor/ConsultaProvDireccion/?IdDirecSolicitud=" + IdSolicitud,
            })
        },

        getSolProvLineaAdminList: function (IDNIVEL, IDLINEA, IDMODULO, IDUSUARIO) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaLineaAdmin/?IDNIVEL=" + IDNIVEL + "&IDLINEA=" + IDLINEA + "&IDMODULO=" + IDMODULO + "&IDUSUARIO=" + IDUSUARIO,
            })
        },

        getPostSolicitudList: function (SolcitudProveedor, SolContacto, SolBanco, SolDireccion, SolDocAdjunto, ViaPago, SolRamo, SolZona, SolProvHistEstado, SolLineaNegocios) {
            debugger;


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
            console.log(JSON.stringify(SolProveedorid));
            return $http.post(serviceBase + 'Api/SolicitudProveedor/GrabaSolicitud', SolProveedorid).then(function (response) {
                //  debugger;
                return response;

            });
        },

        getPostProveedorList: function (SolcitudProveedor, SolContacto, SolBanco, SolDireccion, SolDocAdjunto, ViaPago, SolRamo, SolZona, SolProvHistEstado, SolLineaNegocios) {
            debugger;

            var vf = "d"
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
            return $http.post(serviceBase + 'Api/Proveedor/ActualizaProveedor', SolProveedorid).then(function (response) {
                debugger;
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
            Ruta = baseUrl + "SolicitudProveedor/SolicitudBajaArchivo/";
            return $http.get(Ruta, { params: { solpath: solpath, solarchivo: solarchivo }, responseType: 'arraybuffer' }).then(function (results) {
                return results;
            });
        },


        getBandejaSolicitudList: function (bIdentificacion, FecDesde, FecHasta, BEstado, Pantalla, Opcion, BIDLINEA, BIDUSUARIO) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/ConsultaBandejaSolicitud/?bIdentificacion=" + bIdentificacion + "&FecDesde=" + FecDesde + "&FecHasta=" + FecHasta + "&BEstado=" + BEstado + "&Pantalla=" + Pantalla + "&Opcion=" + Opcion + "&BIDLINEA=" + BIDLINEA + "&BIDUSUARIO=" + BIDUSUARIO,
            })
        },

        getListDocumentoAdjunto: function (TipoPersona) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/GetListaDocumentosAdjuntos/?TipoPersona=" + TipoPersona,
            })
        },

        getListAdjuntoCondicional: function (TipoPersona) {
            return $http({
                method: 'GET',
                url: baseUrl + "SolicitudProveedor/GetListaAdjuntosCondicionales/?ProcesoSoporte=" + TipoPersona,
            })
        },

        getValidaActividadEconomica: function (Identificacion, ActividadEconomica) {
            //var deferred = $q.defer();
             return $http({
                method: 'GET',
                url: baseUrl + "ValidacionSolProveedor/GetValidaActividadEconomica/?identificacion=" + Identificacion + "&actividadEconomica=" + ActividadEconomica,
            }).then(function (response) {
                return response;
                //deferred.resolve(response);
                
            });

            //return deferred.promise;
        },

        getPostValidacionPoliticas: function (SolcitudProveedor, SolContacto) {
            var deferred = $q.defer();
            var SolProveedorid = {
                'p_SolProvContacto': SolContacto,
                'p_SolProveedor': SolcitudProveedor,
                'p_SolProvBanco': "",
                'p_SolProvDireccion': "",
                'p_SolDocAdjunto': "",
                'p_SolViapago': "",
                'p_SolRamo': "",
                'p_SolProvZona': "",
                'p_SolProvHistEstado': "",
                'p_SolLineasNegocios': ""
            }
            //return $http.get(serviceBase + 'Api/SolicitudProveedor/ValidacionPoliticas', SolProveedorid).then(function (response) {
            //    //  debugger;
            //    return response;

            //});

             $http({
                method: 'post',
                url: baseUrl + "ValidacionSolProveedor/ValidacionPoliticas/",
                headers: { 'Content-Type': 'application/json' },
                data: SolProveedorid
            }).then(function (response) {
               // return response;                
                deferred.resolve(response);
            });
            return deferred.promise;
        },

        EscribePDFAdjuntos: function (rutaDirectorio, nombre) {
            return $http({
                method: 'POST',
                url: baseUrl + "SolicitudProveedor/EscribePDFAdjuntos/?rutaDirectorio=" + rutaDirectorio + "&nombre=" + nombre ,
            })
        },

    }
}]);