'use strict';
app.factory('NotificacionService', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var NotificacionServiceFactory = {};
    var Ruta = '';

    //Invoca a API para cagar datos de notificaciones registradas 
    var _getConsultaNotificaciones = function (estado) {
        
        Ruta = serviceBase + "Api/Notificacion/consultaNotificaciones/?estado=" + estado;
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getConsultaLisProveedores = function (txtTipo, txtDato, txtUsr) {

        Ruta = serviceBase + "Api/Notificacion/consultaLisProveedor/?tipo=" + txtTipo + "&dato=" + txtDato + "&usr=" + txtUsr;;
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getGrabaNotificacion = function (notGrabar) {
        //debugger;
        Ruta = serviceBase + 'Api/Notificacion/grabaNotificacion/';
        //return $http.get(Ruta, userData).then(function (results) {
        return $http.post(Ruta, notGrabar, { cache: false }).then(function (response) {
            //debugger;
            return response;
            //debugger;
        });
        $httpBackend.flush();
    };

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getSecuenciaDirectorio = function (pTabla) {

        Ruta = serviceBase + "Api/Notificacion/secuenciaDirectorio/?tipo=" + pTabla;

        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
        
    };

    var _getDescargarArchivos = function (listaArchivos) {

        Ruta = serviceBase + "Api/Download/DownloadFile/";

        var respuesta = serviceBase;
        return $http.post(Ruta, listaArchivos, { cache: false }).then(function (results) {
           // debugger;
            respuesta = respuesta + results.data;
            return respuesta;
        });
        $httpBackend.flush();
    };

    var _getUploadFileSFTP = function (listaArchivos) {

        Ruta = serviceBase + "Api/UploadSFTP/UploadFileSFTP/";


        return $http.post(Ruta, listaArchivos, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getActualizaEstado = function (pCod, pEstado, pUsuario, pObservacion) {

        Ruta = serviceBase + "Api/Notificacion/actualizaEstado/?codigo=" + pCod + "&estado=" + pEstado + "&usuario=" + pUsuario + "&observacion=" + pObservacion;

        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getActualizaEstadoNot = function (pCodNot, pCodProv) {

        Ruta = serviceBase + "Api/Notificacion/actualizaEstadoNot/?codNotificacion=" + pCodNot + "&codProveedor=" + pCodProv;

        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getagrupaNotificacion = function (pCodNot, pCodNot2) {

        Ruta = serviceBase + "Api/Notificacion/agrupaNotificacion/?codNotificacion=" + pCodNot + "&codNotificacion2=" + pCodNot2;

        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getConsultaListaNotificacionesProv = function (ruc) {

        Ruta = serviceBase + "Api/Notificacion/consultaListaNotProveedor/?ruc=" + ruc ;

        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getRegistraMensaje = function (pUsuario, pCorreo, pMensaje) {

        Ruta = serviceBase + "Api/Notificacion/registraMensaje/?usuario=" + pUsuario + "&correo=" + pCorreo + "&mensaje=" + pMensaje;

        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getConsultaListaPrecios = function (tipoLista) {

        Ruta = serviceBase + "Api/Notificacion/consultaListaPrecios/?tipoLista=" + tipoLista;
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };

    var _getConsultaClasificados = function (tipoLista) {
        debugger
        Ruta = serviceBase + "Api/Notificacion/consultaClasificados/?tipoLista2=" + tipoLista;
        return $http.get(Ruta, { cache: false }).then(function (results) {
            return results;
        });
        $httpBackend.flush();
    };
    
    var _getConsTodosRoles = function (errDefR) {
        Ruta = serviceBase + "Api/SegBandjUsrsAdic/ConsTodosRoles/?errDefR=" + errDefR;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    NotificacionServiceFactory.getConsTodosRoles = _getConsTodosRoles;
    NotificacionServiceFactory.getConsultaNotificaciones = _getConsultaNotificaciones;
    NotificacionServiceFactory.getConsultaListaNotificacionesProv = _getConsultaListaNotificacionesProv;
    NotificacionServiceFactory.getConsultaLisProveedores = _getConsultaLisProveedores;
    NotificacionServiceFactory.getGrabaNotificacion = _getGrabaNotificacion;
    NotificacionServiceFactory.getCatalogo = _getCatalogo;
    NotificacionServiceFactory.getSecuenciaDirectorio = _getSecuenciaDirectorio;
    NotificacionServiceFactory.getDescargarArchivos = _getDescargarArchivos;
    NotificacionServiceFactory.getActualizaEstado = _getActualizaEstado;
    NotificacionServiceFactory.getActualizaEstadoNot = _getActualizaEstadoNot;
    NotificacionServiceFactory.getagrupaNotificacion = _getagrupaNotificacion;
    NotificacionServiceFactory.getUploadFileSFTP = _getUploadFileSFTP;
    NotificacionServiceFactory.getRegistraMensaje = _getRegistraMensaje;
    NotificacionServiceFactory.getConsultaListaPrecios = _getConsultaListaPrecios;
    NotificacionServiceFactory.getConsultaClasificados = _getConsultaClasificados;


    return NotificacionServiceFactory;

}]);

