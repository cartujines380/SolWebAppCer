'use strict';
app.factory('RequerimientoService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var AsigProvEtiquetaServiceFactory = {};
    var Ruta = '';

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

    var _getEmpresa = function (tipo) {

        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getEmpresas/?tipoEmpresas=" + tipo
        })
    };

    var _getCategoria = function (tipo) {

        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getCategoria/?tipoCategoria=" + tipo
        })
    };

    var _getGrabarRequerimiento = function (fecha, codEmpresa, codCategoria, monto, descripcion, usuario, pageArchivo) {

        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getGrabarRequerimiento/?fecha=" + fecha + "&codEmpresa=" + codEmpresa + "&codCategoria=" + codCategoria + "&monto=" + monto + "&descripcion=" + descripcion + "&usuario=" + usuario + "&pageArchivo=" + pageArchivo
        })
    };

    var _getUpdateRequerimiento = function (idu, fecha, codEmpresa, codCategoria, monto, descripcion, usuario, pageArchivo) {

        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getUpdateRequerimiento/?idu=" + idu + "&fechau=" + fecha + "&codEmpresau=" + codEmpresa + "&codCategoriau=" + codCategoria + "&montou=" + monto + "&descripcionu=" + descripcion + "&usuariou=" + usuario + "&upageArchivo=" + pageArchivo
        })
    };

    var _getConsultarRequerimiento = function (fecha, codEmpresa, codCategoria, codproveedor) {

        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getConsultarRequerimientoPrvo/?fechabuscarp=" + fecha + "&codEmpresabuscarp=" + codEmpresa + "&codCategoriabuscarp=" + codCategoria + "&codproveedor=" + codproveedor
        })
    };

    var _getEliminar = function (idEli) {

        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getEliminar/?idEli=" + idEli
        })
    };

    var _getSeleccionar = function (idSel) {

        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getSeleccionar/?idSel=" + idSel
        })
    };

    var _getPubli = function (criterio, feIni, feFin, req, cat, prov) {
        Ruta = serviceBase + "Api/Lici_LicitacionProve/getPubliProv/?criterio=" + criterio + "&feIni=" + feIni + "&feFin=" + feFin + "&req=" + req + "&cat=" + cat + "&prov=" + prov;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getParticipa = function (prov) {
        Ruta = serviceBase + "Api/Lici_LicitacionProve/getParticipa/?prov=" + prov;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getCata = function (criterio) {
        Ruta = serviceBase + "Api/Lici_BandejaReq/getCatalogo/?criterio=" + criterio;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _setParticipa = function (idPubli, prov) {
        Ruta = serviceBase + "Api/Lici_LicitacionProve/setParticipa/?idPubli=" + idPubli + "&prov=" + prov;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getOferta = function (idOferta, prov) {
        Ruta = serviceBase + "Api/Lici_LicitacionProve/getOferta/?idOferta=" + idOferta + "&prov=" + prov;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _guardaOferta = function (oferta, $files) {

        var formdata = new FormData();

        angular.forEach($files, function (value, key) {
            formdata.append(key, value);
        });

        angular.forEach(oferta.documentos, function (value, key) {
            if (value.idDoc == 0)
                formdata.append('nomDoc', value.desc);
            else
                if (value.idDoc != 0)
                    formdata.append('queda', value.idDoc);
        });

        var req = {
            method: 'POST',
            url: serviceBase + 'Api/Lici_LicitacionProve/guardaOferta/?idOferta=' + oferta.idOferta + '&codProveedor=' + oferta.codProveedor,
            headers: {
                'Content-Type': undefined
            },
            data: formdata
        }
        return $http(req).then(function (results) {
            return results;
        });

    };

    var _enviaOferta = function (oferta, $files) {

        var formdata = new FormData();

        angular.forEach($files, function (value, key) {
            formdata.append(key, value);
        });

        angular.forEach(oferta.documentos, function (value, key) {
            if (value.idDoc == 0)
                formdata.append('nomDoc', value.desc);
            else
                if (value.idDoc != 0)
                    formdata.append('queda', value.idDoc);
        });

        var req = {
            method: 'POST',
            url: serviceBase + 'Api/Lici_LicitacionProve/enviaOferta/?idOferta=' + oferta.idOferta + '&codProveedor=' + oferta.codProveedor + '&monto=' + oferta.monto + '&tiempo=' + oferta.tiempoEjecucion,
            headers: {
                'Content-Type': undefined
            },
            data: formdata
        }
        return $http(req).then(function (results) {
            return results;
        });

    };

    var _getDescargarArchivos = function (rutaDirectorio, nomArchivo) {
        //_LeePDFContratos(nomArchivo);

        Ruta = serviceBase + 'Api/Lici_Licitacion/DescargaContratoPDF';

        return $http.get(Ruta, { params: { rutaDirectorio: rutaDirectorio, nomArchivo: nomArchivo }, responseType: 'arraybuffer' }).then(function (response) {
            return response;
        });
    };

    var _LeePDFContratos = function (rutaDirectorio, nomArchivo) {
        Ruta = serviceBase + "Api/Lici_Licitacion/LeePDFContratos";
        return $http.get(Ruta, { params: { rutaDirectorio: rutaDirectorio , nomArchivo: nomArchivo  } }).then(function (results) {
            return results;
        });
    };

    AsigProvEtiquetaServiceFactory.getCatalogo = _getCatalogo;
    AsigProvEtiquetaServiceFactory.getProveedorEtiqList = _getProveedorEtiqList;
    AsigProvEtiquetaServiceFactory.getEmpresa = _getEmpresa;
    AsigProvEtiquetaServiceFactory.getCategoria = _getCategoria;
    AsigProvEtiquetaServiceFactory.getGrabarRequerimiento = _getGrabarRequerimiento;
    AsigProvEtiquetaServiceFactory.getConsultarRequerimiento = _getConsultarRequerimiento;
    AsigProvEtiquetaServiceFactory.getEliminar = _getEliminar;
    AsigProvEtiquetaServiceFactory.getSeleccionar = _getSeleccionar;
    AsigProvEtiquetaServiceFactory.getUpdateRequerimiento = _getUpdateRequerimiento;

    AsigProvEtiquetaServiceFactory.getPubli = _getPubli;
    AsigProvEtiquetaServiceFactory.getCata = _getCata;
    AsigProvEtiquetaServiceFactory.setParticipa = _setParticipa;
    AsigProvEtiquetaServiceFactory.getParticipa = _getParticipa;
    AsigProvEtiquetaServiceFactory.getOferta = _getOferta;
    AsigProvEtiquetaServiceFactory.guardaOferta = _guardaOferta;
    AsigProvEtiquetaServiceFactory.enviaOferta = _enviaOferta;

    AsigProvEtiquetaServiceFactory.LeePDFContratos = _LeePDFContratos;
    AsigProvEtiquetaServiceFactory.getDescargarArchivos = _getDescargarArchivos;

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
