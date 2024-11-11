'use strict';
app.factory('NotificacionService', ['$http', 'ngAuthSettings', '$q', function ($http, ngAuthSettings, $q) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var NotificacionServiceFactory = {};
    var Ruta = '';

    //Invoca a API para cagar datos de notificaciones registradas 
    var _getConsultaNotificaciones = function (estado) {

        Ruta = serviceBase + "Api/Notificacion/consultaNotificaciones/?estado=" + estado;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getRegistraNuevoProv = function (nombre, email, telefono, celular, productos) {

        Ruta = serviceBase + "Api/ListaPrecio/registraProveedorgeneral/?nombreg=" + nombre + "&emailg=" + email + "&telefonog=" + telefono + "&celularg=" + celular + "&productosg=" + productos;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsultaLisProveedores = function (txtTipo, txtDato,txtUsr) {

        Ruta = serviceBase + "Api/Notificacion/consultaLisProveedor/?tipo=" + txtTipo + "&dato=" + txtDato + "&usr=" + txtUsr;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabaNotificacion = function (notGrabar) {
        Ruta = serviceBase + 'Api/Notificacion/grabaNotificacion/';
        //return $http.get(Ruta, userData).then(function (results) {
        return $http.post(Ruta, notGrabar).then(function (response) {
            //debugger;
            return response;
            //debugger;
        });
    };

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getSecuenciaDirectorio = function (pTabla) {

        Ruta = serviceBase + "Api/Notificacion/secuenciaDirectorio/?tipo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getDescargarArchivos = function (listaArchivos) {

        Ruta = serviceBase + "Api/Download/DownloadFile/";

        var respuesta = serviceBase;
        return $http.post(Ruta, listaArchivos).then(function (results) {
            if (results.data == "") {
                respuesta = respuesta + "UploadedDocuments/" + listaArchivos[1] + "/";
            } else {
                respuesta = respuesta + results.data;
            }
            return respuesta;
        });
    };

    var _getUploadFileSFTP = function (listaArchivos) {

        Ruta = serviceBase + "Api/UploadSFTP/UploadFileSFTP/";


        return $http.post(Ruta, listaArchivos).then(function (results) {
            return results;
        });
    };

    var _getActualizaEstado = function (pCod, pEstado, pUsuario, pObservacion) {

        Ruta = serviceBase + "Api/Notificacion/actualizaEstado/?codigo=" + pCod + "&estado=" + pEstado + "&usuario=" + pUsuario + "&observacion=" + pObservacion;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getActualizaEstadoNot = function (pCodNot, pCodProv,usr) {

        Ruta = serviceBase + "Api/Notificacion/actualizaEstadoNot/?codNotificacion=" + pCodNot + "&codProveedor=" + pCodProv + "&usr=" + usr;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getagrupaNotificacion = function (pCodNot, pCodNot2) {

        Ruta = serviceBase + "Api/Notificacion/agrupaNotificacion/?codNotificacion=" + pCodNot + "&codNotificacion2=" + pCodNot2;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsultaListaNotificacionesProv = function (ruc, usr) {

        Ruta = serviceBase + "Api/Notificacion/consultaListaNotProveedor/?ruc=" + ruc + "&usr=" + usr;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _actualizaMensajesPagosProv = function (ruc, id, f) {

        Ruta = serviceBase + "Api/Notificacion/actualizaMensajesPagosProv/?id=" + id + "&ruc=" + ruc + "&f=" + f;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _actualizaMensajesPagosProv = function (ruc, id, f) {

        Ruta = serviceBase + "Api/Notificacion/actualizaMensajesPagosProv/?id=" + id + "&ruc=" + ruc + "&f=" + f;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getRegistraMensaje = function (pUsuario, pCorreo, pMensaje) {

        Ruta = serviceBase + "Api/Notificacion/registraMensaje/?usuario=" + pUsuario + "&correo=" + pCorreo + "&mensaje=" + pMensaje;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsultaListaPrecios = function (tipoLista, ruc) {

        Ruta = serviceBase + "Api/Notificacion/consultaListaPrecios/?tipoLista=" + tipoLista + "&ruc=" + ruc;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsultaListaPreciosg = function (tipoLista, ruc, regInicial, regFinal) {

        Ruta = serviceBase + "Api/ListaPrecio/consultaListaPreciosgeneral/?tipoListag=" + tipoLista + "&rucg=" + ruc + "&regInicial=" + regInicial + "&RegFinal=" + regFinal;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsultaClasificados = function (tipoLista) {
        Ruta = serviceBase + "Api/Notificacion/consultaClasificados/?tipoLista2=" + tipoLista;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getDescargaNotificacionG = function () {
        Ruta = serviceBase + "Api/Notificacion/descargaNotificacionG" ;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
   

    NotificacionServiceFactory.getConsultaNotificaciones = _getConsultaNotificaciones;
    NotificacionServiceFactory.getConsultaListaNotificacionesProv = _getConsultaListaNotificacionesProv;
    NotificacionServiceFactory.actualizaMensajesPagosProv = _actualizaMensajesPagosProv;
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
    NotificacionServiceFactory.getConsultaListaPreciosg = _getConsultaListaPreciosg;
    NotificacionServiceFactory.getConsultaClasificados = _getConsultaClasificados;
    NotificacionServiceFactory.getRegistraNuevoProv = _getRegistraNuevoProv;
    NotificacionServiceFactory.getDescargaNotificacionG = _getDescargaNotificacionG;
    
    return NotificacionServiceFactory;

}]);

