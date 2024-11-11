'use strict';
app.factory('frmReporteBIService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var ReporteBIServiceFactory = {};
    var Ruta = '';
    var _getConsToken = function (report) {
        Ruta = serviceBase + 'Api/ReporteBI/consToken?report=' + report;
        return $http.post(Ruta).then(function (response) {
            return response;
        });
    };
    ReporteBIServiceFactory.getConsToken = _getConsToken;
    return ReporteBIServiceFactory;
}]);