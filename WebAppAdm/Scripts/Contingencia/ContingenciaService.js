//Pantalla de Consulta y Envío de Pedidos
'use strict';
app.factory('ConsEnvPedService', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ConsEnvPedServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {
        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsEnvPedidos = function (tipo, FechaIni, FechaFin, Ruc, SiTxt, SiXml, SiPdf, accion, codProveedor, numPedido, estados, almacenes, correoDestinarios) {
        debugger;
        Ruta = serviceBase + "Api/PedConsPedidos/ConsEnvPedidos/?tipo=" + tipo + "&FechaIni=" + FechaIni + '&FechaFin=' + FechaFin + '&Ruc=' + Ruc
                                                                            + '&SiTxt=' + SiTxt + '&SiXml=' + SiXml + '&SiPdf=' + SiPdf 
                                                                            + '&Accion=' + accion + '&CodProveedor=' + codProveedor + '&NumPedido=' + numPedido
                                                                            + '&ConsPestados=' + estados + '&ConsP2=' + almacenes + '&Destinatarios=' + correoDestinarios;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    var _getConsAlmacenes = function (tipoLista) {
        Ruta = serviceBase + "Api/PedConsPedidos/ConsAlmacenes?tipoLista=" + tipoLista;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    ConsEnvPedServiceFactory.getConsAlmacenes = _getConsAlmacenes;
    ConsEnvPedServiceFactory.getCatalogo = _getCatalogo;
    ConsEnvPedServiceFactory.getConsEnvPedidos = _getConsEnvPedidos;
    
    return ConsEnvPedServiceFactory;

}]);
