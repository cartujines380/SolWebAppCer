
'use strict';
app.factory('SeguridadService', ['$http', 'ngAuthSettings', 'authService', function ($http, ngAuthSettings, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var SeguridadServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {
        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsBandjUsrsAdmin = function (CodSap, Ruc, Nombre, ConUsuario, Estado, Usuario) {
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
        Ruta = serviceBase + "Api/SegRecuperaClave/RecuperaClaveValidar/?Ruc=" + Ruc + '&Correo=' + Correo + '&Usuario=' + Usuario;
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
        Ruta = serviceBase + "Api/Seguridad/Menus/?clientId=" + ngAuthSettings.clientId + '&userName=' + authService.authentication.IdentParticipante;

        //Ruta = serviceBase + "Api/Seguridad/Menus/";
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

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

    var _getContactoList = function (CodSap) {
        Ruta = serviceBase + "Api/ContactoProveedor/getProveedorContactoList/?idproveedorconta=" + CodSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getListUsrActas = function (ListaUsr, ListaActas, razonSoc, ruc, fecDesde, fecHasta) {
        debugger;
        var Actas = {
            'p_ListaUsuarios': ListaUsr,
            'p_ListaActas': ListaActas,
            'p_RazonSocial': razonSoc,
            'p_Ruc': ruc,
            'p_FecDesde': fecDesde,
            'p_FecHasta': fecHasta
        }
        Ruta = serviceBase + 'api/ActasEnviaAdmin/ActasEnvia/';
        debugger;
        return $http.post(Ruta, Actas).then(function (response) {
            return response;
        });
    };

    var _getConsAlmacenes = function (tipoLista) {
        Ruta = serviceBase + "Api/PedConsPedidos/ConsAlmacenes?tipoLista=" + tipoLista;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsulaGridActaRecepcion = function (tipo, Norden, Nfactura, Fecha1, Fecha2, codsapat, codigoAlmacen) {

        Ruta = serviceBase + "Api/ReporteAdministrador/getConsulaGridActaRecepcion/?tipo=" + tipo + '&Norden=' + Norden + '&Nfactura=' + Nfactura + '&Fecha1=' + Fecha1 + '&Fecha2=' + Fecha2 + '&codsapat=' + codsapat + '&codigoAlmacen=' + codigoAlmacen;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getActualizaEstadoActaRecepcion = function (id, estado) {
        Ruta = serviceBase + "Api/ReporteAdministrador/getActualizaEstadoActaRecepcion/?idSencuencial=" + id + '&estado=' + estado;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getArchivoActaRecepcion = function (idarchivo, anio, mes, dia) {
        Ruta = serviceBase + 'Api/ReporteAdministrador/ExportarArchivoActaRecepcion';

        return $http.get(Ruta, { params: { idarchivo: idarchivo, anio: anio, mes: mes, dia: dia }, responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });
    };

    //#region RFD0 - 2022 - 155: Módulo de seguridad
    var _getConsultaReporteRol = function (FechaIni, FechaFin, Accion) {

        Ruta = serviceBase + "Api/ReporteAdministrador/getReporteRol/?FechaIni=" + FechaIni
            + '&FechaFin=' + FechaFin
            + '&Accion=' + Accion
            ;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGeneraReporteRol = function (SolArticulo, Opcion) {
        Ruta = serviceBase + 'Api/ReporteAdministrador/getGeneraReporteRol/?opcion='+ Opcion;
        return $http.post(Ruta, SolArticulo, { responseType: 'arraybuffer' }).then(function (response) {
            return response;
        });
    };

    var _verificarTransaccion = function (transaccion) {
        Ruta = serviceBase + "Api/VerificaTransaccion/Verificar?transaccion=" + transaccion;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    }

    //#endregion

    


    SeguridadServiceFactory.getActualizaEstadoActaRecepcion = _getActualizaEstadoActaRecepcion;
    SeguridadServiceFactory.getArchivoActaRecepcion = _getArchivoActaRecepcion;
    SeguridadServiceFactory.getConsulaGridActaRecepcion = _getConsulaGridActaRecepcion;
    SeguridadServiceFactory.getListUsrActas = _getListUsrActas;
    SeguridadServiceFactory.getContactoList = _getContactoList;
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
    SeguridadServiceFactory.getConsAlmacenes = _getConsAlmacenes;

    SeguridadServiceFactory.getCambiarClave = _getCambiarClave;

    SeguridadServiceFactory.getRecuperaClaveValidar = _getRecuperaClaveValidar;
    SeguridadServiceFactory.getRecuperaClaveEnviarTmp = _getRecuperaClaveEnviarTmp;
    SeguridadServiceFactory.getRecuperaClaveCambiar = _getRecuperaClaveCambiar;
    SeguridadServiceFactory.getMenusS = _getMenus;

    //#region RFD0 - 2022 - 155: Módulo de seguridad
    SeguridadServiceFactory.getConsultaReporteRol = _getConsultaReporteRol;
    SeguridadServiceFactory.getGeneraReporteRol = _getGeneraReporteRol;
    SeguridadServiceFactory.verificarTransaccion = _verificarTransaccion;
    //#endregion

    return SeguridadServiceFactory;

}]);
