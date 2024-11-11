/// <reference path="../angular.js" />  
/// <reference path="../angular.min.js" />   


/// <reference path="Modules.js" />   
   
app.service("GeneralService22", function ($http) {
    //Get Order Master Records  
    this.getMenuDetails = function (pUserName) {
         
        return $http.get("http://localhost:26264/Api/Menus/GetMenuDetails/?UserName=" + pUserName);
    };

    this.getTablasCatalogos = function (pTabla) {

        var Ruta='';
        Ruta = "http://localhost:26264/Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta);
    };
});


'use strict';
app.factory('GeneralService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var GeneralServiceFactory = {};
    var _getCatalogo = function (pTabla) {

        Ruta =serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getCargaSociedad = function (tipo) {
        Ruta = serviceBase + "api/Catalogos/getCargaSociedad/?tipoid=" + tipo;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getArticulos = function (tipo, codigo, chkCodRef, CodRef, chkCodSap,CodSap, chkFecha, FechaDesde, FechaHasta, chkTipoSol, TipoSolicitud) {
       
        Ruta = "http://localhost:26264/Api/ConsSolicitudArticulo?tipo=" + tipo + '&codigo=' + codigo + '&chkCodRef=' + chkCodRef + '&CodRef=' + CodRef +'&chkCodSap=' + chkCodSap + '&CodSap=' + CodSap + '&chkFecha=' + chkFecha + '&FechaDesde=' + FechaDesde + '&FechaHasta=' + FechaHasta + '&chkEstado=false&Estado=0&' + 'chkTipoSol=' + chkTipoSol + '&TipoSolicitud=' + TipoSolicitud;
        alert(Ruta);
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    GeneralServiceFactory.getArticulos = _getArticulos;
    GeneralServiceFactory.getCatalogo = _getCatalogo;
    GeneralServiceFactory.getCargaSociedad = _getCargaSociedad;

    return GeneralServiceFactory;
}]);