'use strict';
app.factory('CompradorService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var CompradorServiceFactory = {};
    var Ruta = '';

    //catalogoServiceFactory.getCatalogo = _getCatalogo;

    var _getConsulaGrid = function (codigoComprador, NombreComprador, Estado) {

        Ruta = serviceBase + "Api/Comprador/ConsultarComprador/?codigoComprador=" + codigoComprador + '&NombreComprador=' + NombreComprador + '&Estado=' + Estado;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _catalogoEmpresa = function (tipo) {

        Ruta = serviceBase + "Api/Comprador/catalogoEmpresa/?tipo=" + tipo;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    }

    var _getGrabaComprador = function (Tablacabecera, Tabladetalle) {
        var DatosIng = {
            'cabecera': Tablacabecera,
            'detalleComprador': Tabladetalle
            }

            Ruta = serviceBase + 'Api/Comprador/GrabarComprador/';
            return $http.post(Ruta, DatosIng).then(function (response) {
            return response;
        });
    };

    var _getModiComprador = function (Tablacabecera, Tabladetalle) {
        var DatosModi = {
            'cabecera': Tablacabecera,
            'detalleComprador': Tabladetalle
        }

        Ruta = serviceBase + 'Api/CompradorModi/ModificarComprador/';
        return $http.post(Ruta, DatosModi).then(function (response) {
            return response;
        });
    };
    var _getConsulaGridUno = function (codigoComprador) {

        Ruta = serviceBase + "Api/Comprador/BuscarUnComprador/?codigoComprador=" + codigoComprador;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    //var _getConsulaGridUnorucced = function (tipo, rucced) {

    //    Ruta = serviceBase + "Api/MantenimientoTransporte/consBuscarChoferesUnoRUCCED/?tipo=" + tipo + '&rucced=' + rucced
    //    return $http.get(Ruta).then(function (results) {
    //        return results;
    //    });
    //};



    //var _getActuaTraCho = function (traCho) {
    //    Ruta = serviceBase + 'Api/MantenimientoTransporte/actualizarTransporte/';
    //    //return $http.get(Ruta, userData).then(function (results) {
    //    return $http.post(Ruta, traCho).then(function (response) {
    //        return response;
    //    });
    //};



    //var _getthefile = function (identificacion, archivo) {
    //    Ruta = serviceBase + 'api/MantenimientoTransporte/bajararchivo/?identificacion=' + identificacion + '&archivo=' + archivo
    //    return $http.get(Ruta).then(function (results) {
    //        return results;
    //    });

    //};
    //var _getSecuenciaDirectorio = function (pTabla) {

    //    Ruta = serviceBase + "Api/MantenimientoTransporte/secuenciaDirectorio/?tipo=" + pTabla;

    //    return $http.get(Ruta).then(function (results) {
    //        return results;
    //    });
    //};

    //var _getExportarDataChofer = function (Tablacabecera, Tabladetalle) {
    //    var Chofer = {
    //        'p_cabeceraChofer': Tablacabecera,
    //        'p_detalleChofer': Tabladetalle
    //    }
    //    Ruta = serviceBase + 'Api/ReporteChofer/ExportarChofer';
    //    return $http.post(Ruta, Chofer).then(function (response) {
    //        return response;
    //    });

    //};

    CompradorServiceFactory.getConsulaGrid = _getConsulaGrid;
    CompradorServiceFactory.catalogoEmpresa = _catalogoEmpresa;
    CompradorServiceFactory.getGrabaComprador = _getGrabaComprador;
    CompradorServiceFactory.getModiComprador = _getModiComprador;
    CompradorServiceFactory.getConsulaGridUno = _getConsulaGridUno;
    //CompradorServiceFactory.getActuaTraCho = _getActuaTraCho;
    //CompradorServiceFactory.getthefile = _getthefile;
    //CompradorServiceFactory.getSecuenciaDirectorio = _getSecuenciaDirectorio;
    //CompradorServiceFactory.getConsulaGridUnorucced = _getConsulaGridUnorucced;
    //CompradorServiceFactory.getExportarDataChofer = _getExportarDataChofer;



    return CompradorServiceFactory;

}]);
