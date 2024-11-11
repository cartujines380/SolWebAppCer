'use strict';
app.factory('archivoPDFService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', function ($http, $q, localStorageService, ngAuthSettings) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var archivoPDFFactory = {};
    var Ruta = '';

    var _getCargaArchivoPDF = function (estado) {

        Ruta = serviceBase + "Api/FileArchivoPDF/cargaArchivoPDF/?archivoFinal=ArchivoPDF.PDF";
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };


    var _getCargaArchivoP12 = function (nombrearchivo,clave,ruc,usuario) {

        Ruta = serviceBase + "Api/FileArchivoPDF/cargaArchivoP12/?nombrearchivo=" + nombrearchivo + "&clave=" + clave + "&ruc=" + ruc + "&usuario=" + usuario;
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };
    archivoPDFFactory.getCargaArchivoPDF = _getCargaArchivoPDF;
    archivoPDFFactory.getCargaArchivoP12 = _getCargaArchivoP12;
    return archivoPDFFactory;
}]);