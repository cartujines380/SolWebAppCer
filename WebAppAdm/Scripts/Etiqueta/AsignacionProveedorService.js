'use strict';
app.factory('AsignacionProveedorService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var AsigProvEtiquetaServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

  

    var _getProveedorEtiqList = function () {

        return $http({
            method: 'GET',
            url: serviceBase + "Api/ProvGeneraEtiqueta/ProvGeneraEtiquetaList"
        })
    };

    
    var _getActualizaProv = function (codProv, genEtiq, usr) {
        debugger;
        return $http({
            method: 'GET',
            url: serviceBase + "Api/ProvGeneraEtiqueta/ProvEtiquetaActualiza/?codProvEtiq=" + codProv + "&generaEtiq=" + genEtiq + "&usrTrxEt=" + usr
        })
    };

   


    AsigProvEtiquetaServiceFactory.getCatalogo = _getCatalogo;
    AsigProvEtiquetaServiceFactory.getProveedorEtiqList = _getProveedorEtiqList;
    AsigProvEtiquetaServiceFactory.getActualizaProv = _getActualizaProv;

    return AsigProvEtiquetaServiceFactory;

}]);


'use strict';
app.factory('ReporteSolicitudCitaService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var AsigProvEtiquetaServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };



    var _getProveedorEtiqList = function () {

        return $http({
            method: 'GET',
            url: serviceBase + "Api/ProvGeneraEtiqueta/ProvGeneraEtiquetaList"
        })
    };


    var _getActualizaProv = function (codProv, genEtiq, usr) {
        debugger;
        return $http({
            method: 'GET',
            url: serviceBase + "Api/ProvGeneraEtiqueta/ProvEtiquetaActualiza/?codProvEtiq=" + codProv + "&generaEtiq=" + genEtiq + "&usrTrxEt=" + usr
        })
    };




    AsigProvEtiquetaServiceFactory.getCatalogo = _getCatalogo;
    AsigProvEtiquetaServiceFactory.getProveedorEtiqList = _getProveedorEtiqList;
    AsigProvEtiquetaServiceFactory.getActualizaProv = _getActualizaProv;

    return AsigProvEtiquetaServiceFactory;

}]);
