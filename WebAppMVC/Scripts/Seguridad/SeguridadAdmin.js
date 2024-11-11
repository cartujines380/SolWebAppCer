
'use strict';
app.factory('SeguridadAdmin', ['$http', 'ngAuthSettings', 'authService', function ($http, ngAuthSettings, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var SeguridadServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {
        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsBandjUsrsAdmin = function (CodSap, Ruc, Nombre, ConUsuario, Estado ,Usuario) {
        Ruta = serviceBase + "Api/SegBandjUsrsAdmin/ConsBandjUsrsAdmin/?CodSap=" + CodSap + '&Ruc=' + Ruc + '&Nombre=' + Nombre + '&ConUsuario=' + ConUsuario + '&Estado=' + Estado + '&Usuario=' + Usuario;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabaUsrAdmin = function (userData) {
        return $http.post(serviceBase + 'Api/SegBandjUsrsAdmin/GrabarUsrAdmin', userData).then(function (response) {
            return response;
        });
    };

    var _getCatalogosFS = function (NombreTablaFS) {
        Ruta = serviceBase + "Api/Seguridad/CatalogosFS/?NombreTablaFS=" + NombreTablaFS;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getValUsrFirstLogon = function (Ruc, Usuario) {
        Ruta = serviceBase + "Api/SegUsrFirstLogon/ConsValUsrFirstLogon/?Ruc=" + Ruc + '&Usuario=' + Usuario;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getValidaEmailUsrFirstLogon = function (CorreoE, CodigoValidacion, NombreUsuario) {
        return $http.post(serviceBase + 'Api/SegUsrFirstLogon/ValidaEmailUsrFirstLogon/?CorreoE=' + CorreoE + '&CodigoValidacion=' + CodigoValidacion + '&NombreUsuario=' + NombreUsuario).then(function (response) {
            return response;
        });
    };

    var _getGrabaUsrFirstLogon = function (userData, respSeg) {
        var userModel = {
            'pDatosUsr': userData,
            'pRespSeg': respSeg
        };
        return $http.post(serviceBase + 'Api/SegUsrFirstLogon/GrabarUsrFirstLogon', userModel).then(function (response) {
            return response;
        });
    };

    var _getConsBandjUsrsAdic = function (Ruc) {
        Ruta = serviceBase + "Api/SegBandjUsrsAdic/ConsBandjUsrsAdic/?Ruc=" + Ruc;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsDatosUsrsAdic = function (Ruc, Usuario) {
        Ruta = serviceBase + "Api/SegBandjUsrsAdic/ConsDatosUsrsAdic/?Ruc=" + Ruc + '&Usuario=' + Usuario;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsTodasZonas = function (errDefZ) {
        Ruta = serviceBase + "Api/SegBandjUsrsAdic/ConsTodasZonas/?errDefZ=" + errDefZ;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsTodosRoles = function (errDefR) {
        Ruta = serviceBase + "Api/SegBandjUsrsAdic/ConsTodosRoles/?errDefR=" + errDefR;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabaUsrAdic = function (userData) {
        return $http.post(serviceBase + 'Api/SegBandjUsrsAdic/GrabarUsrAdic', userData).then(function (response) {
            return response;
        });
    };

    var _getResetClaveUsrAdic = function (Ruc, Usuario, Clave, Correo, NombreUsuario) {
        Ruta = serviceBase + "Api/SegBandjUsrsAdic/ResetClaveUsrAdic/?Ruc=" + Ruc + '&Usuario=' + Usuario
         + '&Clave=' + Clave + '&Correo=' + Correo + '&NombreUsuario=' + NombreUsuario;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getDesbloquearClaveUsrAdic = function (Ruc, Usuario, Correo, NombreUsuario) {
        Ruta = serviceBase + "Api/SegBandjUsrsAdic/DesbloquearClaveUsrAdic/?Ruc=" + Ruc + '&Usuario=' + Usuario
         + '&Correo=' + Correo + '&NombreUsuario=' + NombreUsuario;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getCambiarClave = function (Ruc, Usuario, ClaveAct, ClaveNew, Correo, NombreUsr) {
        Ruta = serviceBase + "Api/SegCambioClave/CambiarClave/?Ruc=" + Ruc + '&Usuario=' + Usuario
         + '&ClaveAct=' + ClaveAct + '&ClaveNew=' + ClaveNew + '&Correo=' + Correo + '&NombreUsr=' + NombreUsr;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getRecuperaClaveValidar = function (Ruc, Correo, Usuario) {
        Ruta = serviceBase + "Api/SegRecuperaClave/RecuperaClaveValidar/?Ruc=" + Ruc + '&Correo=' + Correo + '&Usuario=';
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getRecuperaClaveEnviarTmp = function (Ruc, Usuario, ClaveTmp, Correo, NombreUsr) {
        Ruta = serviceBase + "Api/SegRecuperaClave/RecuperaClaveEnviarTmp/?Ruc=" + Ruc + '&Usuario=' + Usuario
         + '&ClaveTmp=' + ClaveTmp + '&Correo=' + Correo + '&NombreUsr=' + NombreUsr;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getRecuperaClaveCambiar = function (Ruc, Usuario, Clave, Correo, NombreUsr) {
        Ruta = serviceBase + "Api/SegRecuperaClave/RecuperaClaveCambiar/?Ruc=" + Ruc + '&Usuario=' + Usuario
         + '&Clave=' + Clave + '&Correo=' + Correo + '&NombreUsr=' + NombreUsr;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getMenus = function () {
        //Ruta = serviceBase + "Api/Seguridad/Menus/?clientId=" + ngAuthSettings.clientId + '&userName=' + authService.authentication.userName;
        Ruta = serviceBase + "Api/Seguridad/Menus/?clientId=" + ngAuthSettings.clientId + '&userName=' + authService.authentication.IdentParticipante;


        //Ruta = serviceBase + "Api/Seguridad/Menus/";
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    //J. Navarrete 16-01-2016
    var _getConsDatosLegAsociados = function (Ruc, Nombres) {
        Ruta = serviceBase + "Api/SegBandjUsrsAdic/ConsDatosLegAsociados/?Tipo=1&Ruc=" + Ruc + '&Nombres=' + Nombres;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    var _getActDatosLegAsociados = function (Ruc, Usuario, Cedula, CodLegacy, UserLegacy) {
        Ruta = serviceBase + "Api/SegBandjUsrsAdic/ActDatosLegAsociados/?Tipo=1&Ruc=" + Ruc + '&Usuario=' + Usuario + '&Cedula=' + Cedula + '&CodLegacy=' + CodLegacy + '&UserLegacy=' + UserLegacy;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    SeguridadServiceFactory.getConsDatosLegAsociados = _getConsDatosLegAsociados;
    SeguridadServiceFactory.getActDatosLegAsociados = _getActDatosLegAsociados;

    SeguridadServiceFactory.getCatalogo = _getCatalogo;

    SeguridadServiceFactory.getConsBandjUsrsAdmin = _getConsBandjUsrsAdmin;
    SeguridadServiceFactory.getGrabaUsrAdmin = _getGrabaUsrAdmin;

    SeguridadServiceFactory.getCatalogosFS = _getCatalogosFS;
    SeguridadServiceFactory.getValUsrFirstLogon = _getValUsrFirstLogon;
    SeguridadServiceFactory.getValidaEmailUsrFirstLogon = _getValidaEmailUsrFirstLogon;
    SeguridadServiceFactory.getGrabaUsrFirstLogon = _getGrabaUsrFirstLogon;

    SeguridadServiceFactory.getConsBandjUsrsAdic = _getConsBandjUsrsAdic;
    SeguridadServiceFactory.getConsDatosUsrsAdic = _getConsDatosUsrsAdic;
    SeguridadServiceFactory.getConsTodasZonas = _getConsTodasZonas;
    SeguridadServiceFactory.getConsTodosRoles = _getConsTodosRoles;
    SeguridadServiceFactory.getGrabaUsrAdic = _getGrabaUsrAdic;
    SeguridadServiceFactory.getResetClaveUsrAdic = _getResetClaveUsrAdic;
    SeguridadServiceFactory.getDesbloquearClaveUsrAdic = _getDesbloquearClaveUsrAdic;

    SeguridadServiceFactory.getCambiarClave = _getCambiarClave;

    SeguridadServiceFactory.getRecuperaClaveValidar = _getRecuperaClaveValidar;
    SeguridadServiceFactory.getRecuperaClaveEnviarTmp = _getRecuperaClaveEnviarTmp;
    SeguridadServiceFactory.getRecuperaClaveCambiar = _getRecuperaClaveCambiar;
    SeguridadServiceFactory.getMenusS = _getMenus;


    return SeguridadServiceFactory;

}]);
