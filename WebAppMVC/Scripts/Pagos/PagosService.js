'use strict';
app.factory('PagosService', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var PagoServiceFactory = {};
    var Ruta = '';

    var _getConsultaPagos = function (Identificacion, estado) {
        Ruta = '';
        Ruta = serviceBase + "Api/Pagos/consultaPagos?Identificacion=" + Identificacion+"&Tipo=" + estado;
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getConsultaPagosFechas = function (Identificacion,estado,FechaDesde,FechaHasta) {
        Ruta = '';
        Ruta = serviceBase + "Api/Pagos/consultaPagosFechas?Identificacion=" + Identificacion +"&Tipo=" + estado + "&FecDesde=" + FechaDesde + "&FecHasta=" + FechaHasta;
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };


    PagoServiceFactory.getConsultaPagos = _getConsultaPagos;
    PagoServiceFactory.getConsultaPagosFechas = _getConsultaPagosFechas;

    return PagoServiceFactory;

}]);
