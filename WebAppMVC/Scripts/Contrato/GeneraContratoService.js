'use strict';
app.factory('GeneraContratoService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var GeneraContratoServiceFactory = {};
    var Ruta = '';

    var _getLicitaciones = function (rolesAplicacion) {
        Ruta = serviceBase + "api/GeneraContrato/ConsultaLicitaciones?rolesAplicacion=" + rolesAplicacion;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getLicitacionesProv = function (ruc) {
        Ruta = serviceBase + "api/GeneraContrato/ConsultaLicitacionesProv?ruc=" + ruc;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getLicitacionDet = function (idAdquisicion) {
        Ruta = serviceBase + "api/GeneraContrato/ConsultaDetLicitacion?idAdquisicion=" + idAdquisicion;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _actualizaInfoContrato = function (contrato) {
        Ruta = serviceBase + "api/GeneraContrato/ActualizaInfoContrato/";
        return $http.post(Ruta, contrato).then(function (results) {
            return results;
        });
    };

    var _generarContrato = function (objPlantilla) {
        Ruta = serviceBase + "api/GeneraContrato/GenerarContrato/";
        return $http.post(Ruta, objPlantilla).then(function (results) {
            return results;
        });
    };

    var _getDescargarArchivos = function (listaArchivos) {

        Ruta = serviceBase + 'Api/GeneraContrato/DescargaContratoPDF';

        return $http.get(Ruta, { params: { NombreArchivo: listaArchivos }, responseType: 'arraybuffer' }).then(function (response) {
            return response;
        });
    };

    var _LeePDFContratos = function (nomArchivo) {
        Ruta = serviceBase + "Api/GeneraContrato/LeePDFContratos";
        return $http.get(Ruta, { params: { nomArchivo: nomArchivo } } ).then(function (results) {
            return results;
        });
    };

    var _EscribePDFContratos = function (listaArchivos) {
        Ruta = serviceBase + "api/GeneraContrato/EscribirPDFContratos/";
        return $http.post(Ruta, listaArchivos).then(function (results) {
            return results;
        });
    };

    var _getUploadFileSFTP = function (listaArchivos) {

        Ruta = serviceBase + "Api/UploadSFTP/UploadFileSFTP/";

        return $http.post(Ruta, listaArchivos, { cache: false }).then(function (results) {
            _EscribePDFContratos(listaArchivos);
            return results;
        });

        $httpBackend.flush();

    };

    var _aprobarContrato = function (idAdContrato, usuario, idEstado) {
        Ruta = serviceBase + "api/GeneraContrato/AprobarContrato?idAdContrato=" + idAdContrato + "&usuario=" + usuario + "&idEstado=" + idEstado;
        return $http.post(Ruta).then(function (results) {
            return results;
        });
    };

    var _verificaFirma = function (idAdq, nomArchivo, rutaArchivo) {
        Ruta = serviceBase + "api/GeneraContrato/VerificaFirma?idAdq=" + idAdq + "&nomArchivo=" + nomArchivo + "&rutaArchivo=" + rutaArchivo;
        return $http.post(Ruta).then(function (results) {
            return results;
        });
    };

    GeneraContratoServiceFactory.getLicitaciones = _getLicitaciones;
    GeneraContratoServiceFactory.getLicitacionesProv = _getLicitacionesProv;
    GeneraContratoServiceFactory.getLicitacionDet = _getLicitacionDet;
    GeneraContratoServiceFactory.actualizaInfoContrato = _actualizaInfoContrato;
    GeneraContratoServiceFactory.generarContrato = _generarContrato;
    GeneraContratoServiceFactory.getDescargarArchivos = _getDescargarArchivos;
    GeneraContratoServiceFactory.getUploadFileSFTP = _getUploadFileSFTP;
    GeneraContratoServiceFactory.aprobarContrato = _aprobarContrato;
    GeneraContratoServiceFactory.verificaFirma = _verificaFirma;
    GeneraContratoServiceFactory.LeePDFContratos = _LeePDFContratos;

    return GeneraContratoServiceFactory;
}]);
