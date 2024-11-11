'use strict';
app.factory('CatalogoService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var CatalogoServiceFactory = {};
    var Ruta = '';

    var _getConsulaGrid = function (codigoCatalogo, NombreCatalogo, Estado) {
        Ruta = serviceBase + "Api/Catalogos/ConsultarContalogo/?codigoCatalogo=" + codigoCatalogo + '&NombreCatalogo=' + NombreCatalogo + '&Estado=' + Estado;
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

    var _getGrabaCatalogo = function (Tablacabecera, Tabladetalle) {
        var DatosIng = {
            'cabecera': Tablacabecera,
            'detalleCatalogo': Tabladetalle
        }

        Ruta = serviceBase + 'Api/Catalogos/grabarCatalogo/';
        return $http.post(Ruta, DatosIng).then(function (response) {
            return response;
        });
    };

    var _getModiCatalogo = function (Tablacabecera, Tabladetalle) {
        var DatosModi = {
            'cabecera': Tablacabecera,
            'detalleCatalogo': Tabladetalle
        }

        Ruta = serviceBase + 'Api/CatalogoModi/ModificarCatalogo/';
        return $http.post(Ruta, DatosModi).then(function (response) {
            return response;
        });
    };
    var _getConsulaGridUno = function (codigoCatalogo) {

        Ruta = serviceBase + "Api/Catalogos/BuscarUnCatalogo/?codigoCatalogo=" + codigoCatalogo;
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

    CatalogoServiceFactory.getConsulaGrid = _getConsulaGrid;
    CatalogoServiceFactory.catalogoEmpresa = _catalogoEmpresa;
    CatalogoServiceFactory.getGrabaCatalogo = _getGrabaCatalogo;
    CatalogoServiceFactory.getModiCatalogo = _getModiCatalogo;
    CatalogoServiceFactory.getConsulaGridUno = _getConsulaGridUno;
    //CompradorServiceFactory.getActuaTraCho = _getActuaTraCho;
    //CompradorServiceFactory.getthefile = _getthefile;
    //CompradorServiceFactory.getSecuenciaDirectorio = _getSecuenciaDirectorio;
    //CompradorServiceFactory.getConsulaGridUnorucced = _getConsulaGridUnorucced;
    //CompradorServiceFactory.getExportarDataChofer = _getExportarDataChofer;



    return CatalogoServiceFactory;

}]);
