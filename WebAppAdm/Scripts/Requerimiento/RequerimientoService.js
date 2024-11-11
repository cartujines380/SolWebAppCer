'use strict';
app.factory('RequerimientoService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

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

    var _getConsultarRequerimientoAju = function (fechadesde, fechahasta, cod, fechadesdeFA, fechahastaFA) {
        return $http({
            method: 'GET',
            url: serviceBase + "Api/ReqAdjudica/getConsultarRequerimientoAju/?fechadesdeA=" + fechadesde + "&fechahastaA=" + fechahasta + "&codrequerimientoA=" + cod + "&fechadesdeFA=" + fechadesdeFA + "&fechahastaFA=" + fechahastaFA
        })
    };

    var _getGrabarRequerimiento = function (fecha, codEmpresa, codCategoria, monto, descripcion, usuario, pageArchivo, txttiulo) {
        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getGrabarRequerimiento/?fecha=" + fecha + "&codEmpresa=" + codEmpresa + "&codCategoria=" + codCategoria + "&monto=" + monto + "&descripcion=" + descripcion + "&usuario=" + usuario + "&pageArchivo=" + pageArchivo + "&txttiulo=" + txttiulo
        })
    };

    var _getUpdateRequerimiento = function (idu, fecha, codEmpresa, codCategoria, monto, descripcion, usuario, pageArchivo, txttiulo) {
        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getUpdateRequerimiento/?idu=" + idu + "&fechau=" + fecha + "&codEmpresau=" + codEmpresa + "&codCategoriau=" + codCategoria + "&montou=" + monto + "&descripcionu=" + descripcion + "&usuariou=" + usuario + "&upageArchivo=" + pageArchivo + "&tituloup=" + txttiulo
        })
    };

    var _getConsultarRequerimiento = function (fecha, codEmpresa, codCategoria) {
        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getConsultarRequerimiento/?fechabuscar=" + fecha + "&codEmpresabuscar=" + codEmpresa + "&codCategoriabuscar=" + codCategoria
        })
    };

    var _getUpdateAjudicacion = function (codEmpresaAju, idrAju) {
        return $http({
            method: 'GET',
            url: serviceBase + "Api/ReqAdjudica/getUpdateAjudicacion/?codEmpresaAju=" + codEmpresaAju + "&idrAju=" + idrAju
        })
    };

    var _getEliminar = function (idEli) {
        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getEliminar/?idEli=" + idEli
        })
    };

    var _getEliLici = function (idEli) {
        return $http({
            method: 'GET',
            url: serviceBase + "Api/ReqAdjudica/getEliminarLici/?idEliLici=" + idEli
        })
    };

    var _getSeleccionar = function (idSel) {
        return $http({
            method: 'GET',
            url: serviceBase + "Api/Requerimientos/getSeleccionar/?idSel=" + idSel 
        })
    };

    var _getReqs = function (criterio, feIni, feFin, req, empresa, cat) {
        Ruta = serviceBase + "Api/Lici_BandejaReq/getReqs/?criterio=" + criterio + "&feIni=" + feIni + "&feFin=" + feFin + "&req=" + req + "&empresa=" + empresa + "&cat=" + cat;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getReqsLici = function (criterio, feIni, feFin, req, empresa, cat) {
        Ruta = serviceBase + "Api/Lici_Licitacion/getReqs/?criterio=" + criterio + "&feIni=" + feIni + "&feFin=" + feFin + "&req=" + req + "&empresa=" + empresa + "&cat=" + cat;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getCatalogo = function (criterio) {
        Ruta = serviceBase + "Api/Lici_BandejaReq/getCatalogo/?criterio=" + criterio;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _setDirecto = function (req, idProv, idUsu) {
        Ruta = serviceBase + "Api/Lici_BandejaReq/setDirecto/?req=" + req + "&idProv=" + idProv + "&idUsu=" + idUsu;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _setLicitacion = function (req) {
        Ruta = serviceBase + "Api/Lici_BandejaReq/setLicitacion/?req=" + req;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _publica = function (concurso, $files) {
        var formdata = new FormData();
        angular.forEach($files, function (value, key) {
            formdata.append(key, value);
        });

        angular.forEach(concurso.documentos, function (value, key) {
            if (value.idDoc == 0)
                formdata.append('nomDoc', value.desc);
            else
                if (value.idDoc != 0)
                    formdata.append('queda', value.idDoc);
        });

        var req = {
            method: 'POST',
            url: serviceBase + 'Api/Lici_Licitacion/pubLicitacion/?req=' + concurso.idReq + '&idProvs=' + concurso.provs + '&nombre=' + concurso.nomPubli + '&desc=' + concurso.descPubli + '&feIni=' + concurso.feIni + '&feFin=' + concurso.feFin + '&hoFin=' + concurso.hoFin + '&tipoInvitacion=' + concurso.tipoInvitacion,
            headers: {
                'Content-Type': undefined
            },
            data: formdata
        }
        return $http(req).then(function (results) {
            return results;
        });
    };

    var _getPublicaciones = function (req) {
        Ruta = serviceBase + "Api/Lici_Licitacion/getPubli";
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getPublicacioneBande = function (req) {
        Ruta = serviceBase + "Api/Lici_Licitacion/getPBandera/?idbandeja=0";
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _editaPubli = function (concurso, $files) {
        var formdata = new FormData();
        angular.forEach($files, function (value, key) {
            formdata.append(key, value);
        });

        angular.forEach(concurso.documentos, function (value, key) {
            if (value.idDoc == 0)
                formdata.append('nomDoc', value.desc);
            else
                if (value.idDoc != 0)
                    formdata.append('queda', value.idDoc);
        });

        var req = {
            method: 'POST',
            url: serviceBase + 'Api/Lici_Licitacion/editaPubli/?req=' + concurso.idReq + '&nombre=' + concurso.nomPubli + '&desc=' + concurso.descPubli + '&feIni=' + concurso.feFin + '&feFin=' + concurso.feIni + '&hoFin=' + concurso.hoFin,
            headers: {
                'Content-Type': undefined
            },
            data: formdata
        }
        return $http(req).then(function (results) {
            return results;
        });
    };

    var _getDocumentos = function (idReq) {
        Ruta = serviceBase + "Api/Lici_Licitacion/getDocs/?req=" + idReq;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getProvDocs = function (idReq) {
        Ruta = serviceBase + "Api/Lici_Licitacion/getProvDocs/?idReq=" + idReq;
        return $http.get(Ruta).then(function (results) {
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
    AsigProvEtiquetaServiceFactory.getConsultarRequerimientoAju = _getConsultarRequerimientoAju;
    AsigProvEtiquetaServiceFactory.getUpdateAjudicacion = _getUpdateAjudicacion;
    AsigProvEtiquetaServiceFactory.getEliLici = _getEliLici;
    AsigProvEtiquetaServiceFactory.getPublicacioneBande = _getPublicacioneBande;
    AsigProvEtiquetaServiceFactory.getReqs = _getReqs;
    AsigProvEtiquetaServiceFactory.getCatalogo = _getCatalogo;
    AsigProvEtiquetaServiceFactory.setLicitacion = _setLicitacion;
    AsigProvEtiquetaServiceFactory.setDirecto = _setDirecto;
    AsigProvEtiquetaServiceFactory.getReqsLici = _getReqsLici;
    AsigProvEtiquetaServiceFactory.publica = _publica;
    AsigProvEtiquetaServiceFactory.getPublicaciones = _getPublicaciones;
    AsigProvEtiquetaServiceFactory.editaPubli = _editaPubli;
    AsigProvEtiquetaServiceFactory.getDocumentos = _getDocumentos;
    AsigProvEtiquetaServiceFactory.getProvDocs = _getProvDocs;

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
