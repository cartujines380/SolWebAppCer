'use strict';
app.factory('IndicadoresService', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var IndicadoresServiceFactory = {};
    var Ruta = '';


    var _getMarcas = function (trx) {
        Ruta = '';

        Ruta = serviceBase + "Api/Indicadores/obtenerMarcas?k=" + trx +"&p='1'";
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getProveedores = function () {
        Ruta = '';

        Ruta = serviceBase + "Api/Indicadores/obtenerProveedores?id='-1'";
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getResumenVentas = function (
        fecIni, fecFin, idcanal, marca, proveedor, proveedorUsr, codSegmento, subCanal) {
        Ruta = '';

        if (fecIni == undefined || fecIni == "-1")
            fecIni = '';

        if (fecFin == undefined || fecFin == "-1")
            fecFin = '';

        if (idcanal == undefined || idcanal == -1)
            idcanal = -1;

        if (marca == undefined || marca == -1)
            marca = '';

        if (codSegmento == undefined || codSegmento == -1)
            codSegmento = '';

        if (subCanal == undefined || subCanal == -1)
            subCanal = '';

        if (proveedor == undefined || proveedor == -1)
            proveedor = '';

        if (proveedorUsr == undefined || proveedorUsr == -1)
            proveedorUsr = '';

        Ruta = serviceBase + "Api/Indicadores/resumenVentas?fecIni=" + fecIni + "&fecFin="
            + fecFin + "&idcanal=" + idcanal + "&marca=" + marca + "&proveedor="
            + proveedor + "&proveedorUsr=" + proveedorUsr
            + "&idSegmento=" + codSegmento + "&codSubCanal=" + subCanal;

        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getResumenInventario = function (
        fecIni, fecFin, idcanal, marca, proveedor, proveedorUsr, codSegmento, subCanal) {

        Ruta = '';
        if (fecIni == undefined || fecIni == "-1")
            fecIni = '';

        if (fecFin == undefined || fecFin == "-1")
            fecFin = '';

        if (idcanal == undefined || idcanal == -1)
            idcanal = -1;

        if (marca == undefined || marca == -1)
            marca = '';

        if (codSegmento == undefined || codSegmento == -1)
            codSegmento = '';

        if (subCanal == undefined || subCanal == -1)
            subCanal = '';

        if (proveedor == undefined || proveedor == -1)
            proveedor = '';

        if (proveedorUsr == undefined || proveedorUsr == -1)
            proveedorUsr = '';

        Ruta = serviceBase + "Api/Indicadores/resumenInventario?fecIni=" + fecIni + "&fecFin="
            + fecFin + "&idcanal=" + idcanal + "&marca=" + marca + "&proveedor="
            + proveedor + "&proveedorUsr=" + proveedorUsr
            + "&idSegmento=" + codSegmento + "&codSubCanal=" + subCanal;

        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };
    

    var _getResumenCobertura = function (
        fecIni, fecFin, idcanal, marca, proveedor, proveedorUsr, codSegmento, subCanal) {
        Ruta = '';
        if (fecIni == undefined || fecIni == "-1")
            fecIni = '';

        if (fecFin == undefined || fecFin == "-1")
            fecFin = '';

        if (idcanal == undefined || idcanal == -1)
            idcanal = -1;

        if (marca == undefined || marca == -1)
            marca = '';

        if (codSegmento == undefined || codSegmento == -1)
            codSegmento = '';

        if (subCanal == undefined || subCanal == -1)
            subCanal = '';

        if (proveedor == undefined || proveedor == -1)
            proveedor = '';

        if (proveedorUsr == undefined || proveedorUsr == -1)
            proveedorUsr = '';

        Ruta = serviceBase + "Api/Indicadores/resumenCoberturas?fecIni=" + fecIni + "&fecFin="
            + fecFin + "&idcanal=" + idcanal + "&marca=" + marca + "&proveedor="
            + proveedor + "&proveedorUsr=" + proveedorUsr
            + "&idSegmento=" + codSegmento + "&codSubCanal=" + subCanal;

        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    }

    IndicadoresServiceFactory.getResumenVentas = _getResumenVentas;
    IndicadoresServiceFactory.getResumenInventario = _getResumenInventario;
    IndicadoresServiceFactory.getResumenCobertura = _getResumenCobertura;
    IndicadoresServiceFactory.getProveedores = _getProveedores;
    IndicadoresServiceFactory.getMarcas = _getMarcas;

    return IndicadoresServiceFactory;
}]);