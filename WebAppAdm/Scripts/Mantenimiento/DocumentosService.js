'use strict';
app.factory('DocumentosService', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var DocumentosServiceFactory = {};
    var Ruta = '';

    var _getConsultaDocumentos = function () {
        Ruta = '';
        Ruta = serviceBase + "Api/MantenimientoDocumentos/consultaDocumentos";
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _ingresaDocumentos = function (objDocumentos, nAccion) {
        Ruta = '';
        Ruta = serviceBase + "Api/MantenimientoDocumentos/ingresaDocumentos?nAccion=" + nAccion;
        return $http.post(Ruta, objDocumentos).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    DocumentosServiceFactory.getConsultaDocumentos = _getConsultaDocumentos;
    DocumentosServiceFactory.ingresaDocumentos = _ingresaDocumentos;
    

    return DocumentosServiceFactory;

}]);
