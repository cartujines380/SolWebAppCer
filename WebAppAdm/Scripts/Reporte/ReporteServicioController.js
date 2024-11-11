//Servicio Reporte Administrador
'use strict';
app.factory('ReporteAdministradorService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {


    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ReporteAdministradorServiceFactory = {};
    var Ruta = '';



    var _getConsulaGridNoIngreso = function (codsap, usuario) {

        Ruta = serviceBase + "Api/ReporteAdministrador/getConsulaGridNoIngreso/?CodSap=" + codsap + '&Usuario=' + usuario;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsulaGridNoOrdenCompra = function (codsap, ruc) {

        Ruta = serviceBase + "Api/ReporteAdministrador/getConsulaGridNoOrdenCompra/?CodSapno=" + codsap + '&rucno=' + ruc;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsulaGridLogComunicacion = function (codsap, ruc,fecha1,fecha2) {

        Ruta = serviceBase + "Api/ReporteAdministrador/getConsulaGridLogComunicacion/?CodSapno=" + codsap + '&rucno=' + ruc + '&fecha1=' + fecha1 + '&fecha2=' + fecha2;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    

    var _getConsulaGriSolicitudEtiqueta = function (codsap, ruc, fecha1, fecha2, estado) {

        Ruta = serviceBase + "Api/ReporteAdministrador/getConsulaGridSolicitudEtiqueta/?CodSapno=" + codsap + '&rucno=' + ruc + '&fecha1=' + fecha1 + '&fecha2=' + fecha2 + '&estado=' + estado;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsulaGridProveedorNoSolicitud = function (codsap, ruc) {

        Ruta = serviceBase + "Api/ReporteAdministrador/getConsulaGridProveedorNoSolicitud/?CodSapns=" + codsap + '&rucns=' + ruc;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarData = function (tipo,usuariologon,CodSap,Usuario) {

        Ruta = serviceBase + 'Api/ReporteAdministrador/ExportarNoIngreso';

        return $http.get(Ruta,{ params:{ tipo :tipo,usuariologon:usuariologon,CodSap:CodSap,Usuario:Usuario } ,responseType: 'arraybuffer'} ).then(function (results) {
            return results;
        });


    };

    var _getExportarNoCompra = function (tipo, usuariologon, CodSap, ruc) {

        Ruta = serviceBase + 'Api/ReporteAdministrador/ExportarNoCompra';

        return $http.get(Ruta, { params: { tipo: tipo, usuariologon: usuariologon, CodSap: CodSap, ruc: ruc }, responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });


    };


    var _getExportarLogComunicado = function (tipo, usuariologon, CodSap, ruc, fecha1, fecha2) {

        Ruta = serviceBase + 'Api/ReporteAdministrador/ExportarLogComunicado';

        return $http.get(Ruta, { params: { tipo: tipo, usuariologon: usuariologon, CodSap: CodSap, ruc: ruc, fecha1: fecha1, fecha2: fecha2 }, responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });


    };

    var _getExportarSolicitudEtiqueta = function (tipo, usuariologon, CodSap, ruc, fecha1, fecha2,estado) {

        Ruta = serviceBase + 'Api/ReporteAdministrador/ExportarSolicitudEtiqueta';

        return $http.get(Ruta, { params: { tiposo: tipo, usuariologonso: usuariologon, CodSapso: CodSap, rucso: ruc, fecha1so: fecha1, fecha2so: fecha2, estadoso: estado }, responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });


    };
    var _getExportarProveedorNoSolicitud = function (tipo, usuariologon, CodSap, ruc) {

        Ruta = serviceBase + 'Api/ReporteAdministrador/ExportarProveedorNoSolicitud';

        return $http.get(Ruta, { params: { tipo: tipo, usuariologonns: usuariologon, CodSapns: CodSap, rucns: ruc }, responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });


    };

    ReporteAdministradorServiceFactory.getConsulaGridNoIngreso = _getConsulaGridNoIngreso;
    ReporteAdministradorServiceFactory.getConsulaGridNoOrdenCompra = _getConsulaGridNoOrdenCompra;
    ReporteAdministradorServiceFactory.getConsulaGridProveedorNoSolicitud = _getConsulaGridProveedorNoSolicitud;
    
    ReporteAdministradorServiceFactory.getExportarData = _getExportarData;
    ReporteAdministradorServiceFactory.getExportarNoCompra = _getExportarNoCompra;
    ReporteAdministradorServiceFactory.getExportarLogComunicado = _getExportarLogComunicado;
    ReporteAdministradorServiceFactory.getExportarProveedorNoSolicitud = _getExportarProveedorNoSolicitud;
    ReporteAdministradorServiceFactory.getConsulaGridLogComunicacion = _getConsulaGridLogComunicacion;
    ReporteAdministradorServiceFactory.getConsulaGriSolicitudEtiqueta = _getConsulaGriSolicitudEtiqueta;
    ReporteAdministradorServiceFactory.getExportarSolicitudEtiqueta = _getExportarSolicitudEtiqueta;
    



    return ReporteAdministradorServiceFactory;

}]);
