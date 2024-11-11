
'use strict';
app.factory('lineaNegocioService', ['$http', 'ngAuthSettings', 'authService', function ($http, ngAuthSettings, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var lineaNegocioService = {};
    var Ruta = '';

    var _getBandeja = function (criterio) {
        Ruta = serviceBase + "api/AsignacionLineaNeg/ConsUsuarios/?criterioConsulta=" + criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBandejaproveedor = function (criterio) {
        Ruta = serviceBase + "api/AsignacionLineaNeg/ConsProveedor/?criterioConsultaProveedor=" + criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBandejaproveedorTolerancia = function (criterio) {
        Ruta = serviceBase + "api/AsignacionLineaNeg/ConsProveedorTolerancia/?criterioConsultaProveedorTolerancia=" + criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getReporteTolerancia = function (usuario, tiporeporte) {

        Ruta = serviceBase + "Api/AsignacionLineaNeg/GetGeneraReportePackingList/?TipoReporte=" + tiporeporte                                                                     
                                                                        + '&Usuario=' + usuario;

        return $http.get(Ruta, { responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });
    };

    var _getMantenimientoTolerancia = function (codProveedor, porcentaje, accion) {
        Ruta = serviceBase + "api/AsignacionLineaNeg/MantenimientoTolerancia/?CodProveedor=" + codProveedor + "&Porcentaje=" + porcentaje + "&Accion=" + accion;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getLineasEmpleado = function (criterio) {
        Ruta = serviceBase + "api/AsignacionLineaNeg/ConsLineasEmpleado/?criterioConsultaLinea=" + criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getLineasProveedor = function (criterio) {
        Ruta = serviceBase + "api/AsignacionLineaNeg/ConsLineasProveedor/?criterioConsultaLineaProveedor=" + criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getToleranciaProveedor = function (criterio) {
        Ruta = serviceBase + "api/AsignacionLineaNeg/ConsToleranciaProveedor/?criterioConsultaTolerancia=" + criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getGraba = function (criterio) {
        Ruta = serviceBase + "api/AsignacionLineaNeg/Mantenimiento/?criterioMantenimiento=" + criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabaProveedor = function (criterio) {
        Ruta = serviceBase + "api/AsignacionLineaNeg/MantenimientoProveedor/?criterioMantenimientoProveedor=" + criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getElimina = function (Criterio) {
    Ruta = serviceBase + "Api/AsignacionLineaNeg/elimUsuario/?criterioEliminar=" + Criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getEliminaProveedor = function (Criterio) {
        Ruta = serviceBase + "Api/AsignacionLineaNeg/elimProveedor/?criterioEliminarProveedor=" + Criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    lineaNegocioService.getBandeja = _getBandeja;
    lineaNegocioService.getBandejaproveedor = _getBandejaproveedor;
    lineaNegocioService.getLineasEmpleado = _getLineasEmpleado;
    lineaNegocioService.getLineasProveedor = _getLineasProveedor;
    lineaNegocioService.getGraba = _getGraba;
    lineaNegocioService.getGrabaProveedor = _getGrabaProveedor;

    lineaNegocioService.getElimina = _getElimina;
    lineaNegocioService.getEliminaProveedor = _getEliminaProveedor; 
    lineaNegocioService.getToleranciaProveedor = _getToleranciaProveedor; 
    lineaNegocioService.getBandejaproveedorTolerancia = _getBandejaproveedorTolerancia;
    lineaNegocioService.getMantenimientoTolerancia = _getMantenimientoTolerancia; 
    lineaNegocioService.getReporteTolerancia = _getReporteTolerancia;
    return lineaNegocioService;

}]);
