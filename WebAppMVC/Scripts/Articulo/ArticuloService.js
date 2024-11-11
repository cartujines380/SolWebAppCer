//Pantalla de Bandeja de Solicitudes
'use strict';
app.factory('ConsSolArticuloService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ConsSolArticuloServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    //tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkFecha, FechaDesde, FechaHasta, chkEstado, Estado, chkTipoSol, TipoSolicitud
    var _getArticulos = function (tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkFecha, FechaDesde, FechaHasta, chkEstado, Estado, chkTipoSol, TipoSolicitud, chkLinea, LineaNegocio,Usuario,Nivel,CodSapProveedor) {
        debugger;
        Ruta = serviceBase + "Api/Art_Solicitud/ConsSolArticulo/?tipo=" + tipo + '&codigo=' + codigo + '&chkCodRef=' + chkCodRef + '&CodRef=' + CodRef + '&chkCodSap=' + chkCodSap + '&CodSap=' + CodSap + '&chkFecha=' + chkFecha + '&FechaDesde=' + FechaDesde
            + '&FechaHasta=' + FechaHasta + '&chkEstado=' + chkEstado + '&Estado=' + Estado + '&chkTipoSol=' + chkTipoSol +
            '&TipoSolicitud=' + TipoSolicitud + '&chkLinea=' + chkLinea + '&LineaNegocio=' + LineaNegocio + '&ArtUsuario=' + Usuario + '&ArtNivel=' + Nivel + '&CodProveedorCons=' + CodSapProveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    ConsSolArticuloServiceFactory.getArticulos = _getArticulos;
    ConsSolArticuloServiceFactory.getCatalogo = _getCatalogo;


    return ConsSolArticuloServiceFactory;

}]);

//Pantalla de Bandeja de Articulos
'use strict';
app.factory('ConsArticuloService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ConsArticuloServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    //tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkGrupoArt, GrupoArt
    var _getArticulos = function (tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkGrupoArt, GrupoArt, chkLinea, LineaNegocio, Usuario, Nivel, CodSapProveedor) {
        debugger;
        Ruta = serviceBase + "Api/Art_Consulta/ConsArticulo/?tipo=" + tipo + '&codigo=' + codigo + '&chkCodRef=' + chkCodRef + '&CodRef=' + CodRef + '&chkCodSap=' + chkCodSap + '&CodSap=' + CodSap + '&chkGrupoArt=' + chkGrupoArt + '&GrupoArt=' + GrupoArt + '&chkLinea=' +
            chkLinea + '&LineaNegocio=' + LineaNegocio + '&ArtUsuario=' + Usuario + '&ArtNivel=' + Nivel + '&CodProveedorCons=' + CodSapProveedor ;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    //Para consulta de genéricos
    var _getArticulosG = function (tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkGrupoArt, GrupoArt, chkLinea, LineaNegocio, Usuario, Nivel, CodSapProveedor) {
        debugger;
        Ruta = serviceBase + "Api/Art_Consulta/ConsArticuloG/?tipo=" + tipo + '&codigo=' + codigo + '&chkCodRef=' + chkCodRef + '&CodRef=' + CodRef + '&chkCodSap=' + chkCodSap + '&CodSap=' + CodSap + '&chkGrupoArt=' + chkGrupoArt + '&GrupoArt=' + GrupoArt + '&chkLinea=' +
            chkLinea + '&LineaNegocio=' + LineaNegocio + '&ArtUsuario=' + Usuario + '&ArtNivel=' + Nivel + '&CodProveedorCons=' + CodSapProveedor + '&flag=1';
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    ConsArticuloServiceFactory.getCatalogo = _getCatalogo;
    ConsArticuloServiceFactory.getArticulos = _getArticulos;
    ConsArticuloServiceFactory.getArticulosG = _getArticulosG;

    return ConsArticuloServiceFactory;

}]);

//Pantalla de Ingreso de Articulos
'use strict';
app.factory('IngArticuloService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var IngArticuloServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getArticulos = function (tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkFecha, FechaDesde, FechaHasta, chkEstado, Estado, chkTipoSol, TipoSolicitud, chkLinea, LineaNegocio, Usuario, Nivel, CodSapProveedor) {
        debugger;
        Ruta = serviceBase + "Api/Art_Solicitud/ConsSolArticulo/?tipo=" + tipo + '&codigo=' + codigo + '&chkCodRef=' + chkCodRef + '&CodRef=' + CodRef + '&chkCodSap=' + chkCodSap + '&CodSap=' + CodSap + '&chkFecha=' + chkFecha + '&FechaDesde=' + FechaDesde + '&FechaHasta=' + FechaHasta + '&chkEstado=' + chkEstado + '&Estado=' + Estado + '&chkTipoSol=' + chkTipoSol + '&TipoSolicitud=' + TipoSolicitud + '&chkLinea=' + chkLinea +
            '&LineaNegocio=' + LineaNegocio + '&ArtUsuario=' + Usuario + '&ArtNivel=' + Nivel + '&CodProveedorCons=' + CodSapProveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabaSolicitud = function (Cab, Det, Med, CBr, Ima, Com, Cat, Alm, Iae, Ias, Iaa, Cen, Car) {
        debugger;
        var SolArticulo = {
            'p_SolCabecera': Cab,
            'p_SolDetalle': Det,
            'p_SolMedida': Med,
            'p_SolCodigoBarra': CBr,
            'p_SolImagen': Ima,
            'p_SolCompras': Com,
            'p_SolCatalogacion': Cat,
            'p_SolAlmacen': Alm,
            'p_SolIndTipoAlmEnt': Iae,
            'p_SolIndTipoAlmSal': Ias,
            'p_SolIndAreaAlmacen': Iaa,
            'p_SolCentros': Cen,
            'p_SolCaracteristicas': Car
        }
        Ruta = serviceBase + 'Api/Art_Solicitud/GrabaSolArticulo/';
        debugger;
        return $http.post(Ruta, SolArticulo).then(function (response) {
            return response;
        });
    };

    var _getSecuenciaDirectorio = function (pTabla) {

        Ruta = serviceBase + "Api/Notificacion/secuenciaDirectorio/?tipo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
           
            return results;
        });
    };

    //string identificacion, string archivo
    //Retorna el path de la ruta temporal donde esta el archivo
    var _getBajaTempArchivo = function (path, archivo) {

        Ruta = serviceBase + "Api/Art_Solicitud/BajaTempArchivo/?path=" + path + '&archivo=' + archivo;
       
        return $http.get(Ruta).then(function (results) {
           
            results.data = serviceBase + results.data;
            return results;
        });
    };

    //string identificacion, string archivo
    //Descarga el archivo desde el fpt, lo coloca en el temporal y retorna la ruta para visualizar
    var _getBajaFptArchivo = function (path, archivo) {

        Ruta = serviceBase + "Api/Art_Solicitud/BajaFptArchivo/?path_comp=" + path + '&nom_archivo=' + archivo + '&aux=';
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getCargaMasiva = function (path, archivo) {
        debugger;
        Ruta = serviceBase + "Api/Art_Solicitud/CargaMasiva/?pathmasivo=" + path + '&nomarchivo=' + archivo + '&aux1=&aux2=';
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsultaEAN = function (codEAN, tipoEAN) {
        debugger;
        Ruta = serviceBase + "Api/Art_Solicitud/ConsultaEAN/?codEANart=" + codEAN + "&tipoEANart=" + tipoEAN + "&isAdmin=N";;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBajaPlantilla = function () {
        Ruta = serviceBase + "Api/Art_Solicitud/BajaPlantilla";
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getReporteSolicitud = function (Cab, Det) {
        var ReporteArticulo = {
            'p_cabeceraSolArticulo': Cab,
            'p_detalleSolArticulo': Det           
        }
        Ruta = serviceBase + 'Api/ReporteSolArticulo/ExportarSolArticulo/';
            return $http.post(Ruta, ReporteArticulo, {responseType:'arraybuffer'}).then(function (response) {
            return response;
        });
    };

    var _getConsCaracteristicas = function (tipo) {
        Ruta = serviceBase + "Api/Art_Solicitud/ConsCaracteristicas?tipo=" + tipo;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    IngArticuloServiceFactory.getConsCaracteristicas = _getConsCaracteristicas;
    IngArticuloServiceFactory.getCatalogo = _getCatalogo;
    IngArticuloServiceFactory.getArticulos = _getArticulos;
    IngArticuloServiceFactory.getGrabaSolicitud = _getGrabaSolicitud;
    IngArticuloServiceFactory.getSecuenciaDirectorio = _getSecuenciaDirectorio;
    IngArticuloServiceFactory.getBajaTempArchivo = _getBajaTempArchivo;
    IngArticuloServiceFactory.getBajaFptArchivo = _getBajaFptArchivo;
    IngArticuloServiceFactory.getCargaMasiva = _getCargaMasiva;
    IngArticuloServiceFactory.getConsultaEAN = _getConsultaEAN;
    IngArticuloServiceFactory.getBajaPlantilla = _getBajaPlantilla;
    IngArticuloServiceFactory.getReporteSolicitud = _getReporteSolicitud;

    return IngArticuloServiceFactory;

}]);

//Pantalla de Modificacion de Articulos
'use strict';
app.factory('ModArticuloService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ModArticuloServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getArticulos = function (tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkFecha, FechaDesde, FechaHasta, chkEstado, Estado, chkTipoSol, TipoSolicitud, chkLinea, LineaNegocio, Usuario, Nivel, CodSapProveedor) {
        debugger;
        Ruta = serviceBase + "Api/Art_Solicitud/ConsSolArticulo/?tipo=" + tipo + '&codigo=' + codigo + '&chkCodRef=' + chkCodRef + '&CodRef=' + CodRef + '&chkCodSap=' + chkCodSap + '&CodSap=' + CodSap + '&chkFecha=' + chkFecha + '&FechaDesde=' + FechaDesde + '&FechaHasta=' + FechaHasta + '&chkEstado=' + chkEstado + '&Estado=' + Estado + '&chkTipoSol=' + chkTipoSol + '&TipoSolicitud=' + TipoSolicitud + '&chkLinea=' + chkLinea
            + '&LineaNegocio=' + LineaNegocio + '&ArtUsuario=' + Usuario + '&ArtNivel=' + Nivel + '&CodProveedorCons=' + CodSapProveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsCaracteristicas = function (tipo) {
        Ruta = serviceBase + "Api/Art_Solicitud/ConsCaracteristicas?tipo=" + tipo;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    

    var _getGrabaSolicitud = function (Cab, Det, Med, CBr, Ima, Com, Cat, Alm, Iae, Ias, Iaa, Car) {
        var SolArticulo = {
            'p_SolCabecera': Cab,
            'p_SolDetalle': Det,
            'p_SolMedida': Med,
            'p_SolCodigoBarra': CBr,
            'p_SolImagen': Ima,
            'p_SolCompras': Com,
            'p_SolCatalogacion': Cat,
            'p_SolAlmacen': Alm,
            'p_SolIndTipoAlmEnt': Iae,
            'p_SolIndTipoAlmSal': Ias,
            'p_SolIndAreaAlmacen': Iaa,
            'p_SolCaracteristicas': Car
        }
        Ruta = serviceBase + 'Api/Art_Solicitud/GrabaSolArticulo/';
        return $http.post(Ruta, SolArticulo).then(function (response) {
            return response;
        });
    };
    ModArticuloServiceFactory.getConsCaracteristicas = _getConsCaracteristicas;
    ModArticuloServiceFactory.getCatalogo = _getCatalogo;
    ModArticuloServiceFactory.getArticulos = _getArticulos;
    ModArticuloServiceFactory.getGrabaSolicitud = _getGrabaSolicitud;

    return ModArticuloServiceFactory;

}]);
