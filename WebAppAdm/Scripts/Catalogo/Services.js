/// <reference path="../angular.js" />  
/// <reference path="../angular.min.js" />   


/// <reference path="Modules.js" />   
   
app.service("GeneralService22", function ($http) {
    //Get Order Master Records  
    this.getMenuDetails = function (pUserName) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        return $http.get(serviceBase + "Api/Menus/GetMenuDetails/?UserName=" + pUserName);
        //return $http.get("http://localhost:3514/Service1.svc/GetMenuDetails");
    };

    this.getTablasCatalogos = function (pTabla) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var Ruta='';
 
        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;
  
        return $http.get(Ruta);
       
    };
  

});


'use strict';
app.factory('GeneralService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    //
    //
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    //var serviceBase = 'http://localhost:26264/';
    var GeneralServiceFactory = {};

    var _getCatalogo = function (pTabla) {

        Ruta =serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };    
    var _getCargaSociedad = function (tipo) {
        
        Ruta = serviceBase + "Api/Catalogos/getCargaSociedad/?tipoid=" + tipo;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    //catalogoServiceFactory.getCatalogo = _getCatalogo;

    var _getArticulos = function (tipo, codigo, chkCodRef, CodRef, chkCodSap,CodSap, chkFecha, FechaDesde, FechaHasta, chkTipoSol, TipoSolicitud) {
       
        Ruta = serviceBase + "Api/ConsSolicitudArticulo?tipo=" + tipo + '&codigo=' + codigo + '&chkCodRef=' + chkCodRef + '&CodRef=' + CodRef +'&chkCodSap=' + chkCodSap + '&CodSap=' + CodSap + '&chkFecha=' + chkFecha + '&FechaDesde=' + FechaDesde + '&FechaHasta=' + FechaHasta + '&chkEstado=false&Estado=0&' + 'chkTipoSol=' + chkTipoSol + '&TipoSolicitud=' + TipoSolicitud;
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