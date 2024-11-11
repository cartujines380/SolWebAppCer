
'use strict';
app.factory('FacturasService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var FacturasServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {
        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _consProveedorExcepto = function (datos) {
        Ruta = serviceBase + "Api/FacFactNoElec/consProveedorExcepto/?proveedorExcepto=" + datos;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _cargarParametros = function (datos) {
        Ruta = serviceBase + "Api/FacFactNoElec/cargarParametros/?parametros=" + datos;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsSelPedidosFiltro = function (CodSap, Ruc, Usuario, NumPedido, FechaIni, FechaFin, Estados, CodAlmacen) {
        Ruta = serviceBase + "Api/FacFactNoElec/ConsSelPedidosFiltro/?CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario
                    + '&NumPedido=' + NumPedido + '&FechaIni=' + FechaIni + '&FechaFin=' + FechaFin + '&Estados=' + Estados + '&CodAlmacen=' + CodAlmacen;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    var _getConsRecupFacturasFiltro = function (CodSap, Ruc, Usuario, NumPedido, FacEst, FacPto, FacSec, FecEsReg, FechaIni, FechaFin, Estados, CodAlmacen) {
        Ruta = serviceBase + "Api/FacFactNoElec/ConsRecupFacturasFiltro/?CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario 
                    + '&NumPedido=' + NumPedido + '&FacEst=' + FacEst + '&FacPto=' + FacPto + '&FacSec=' + FacSec
                    + '&FecEsReg=' + FecEsReg + '&FechaIni=' + FechaIni + '&FechaFin=' + FechaFin + '&Estados=' + Estados + '&CodAlmacen=' + CodAlmacen;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    var _getConsultaPedidoNumero = function (CodSap, Ruc, Usuario, IdPedido) {
        Ruta = serviceBase + "Api/FacFactNoElec/ConsultaPedidoNumero/?CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario + '&IdPedido=' + IdPedido;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    var _getConsultaDocumentoId = function (CodSap, Ruc, Usuario, IdDocumento) {
        Ruta = serviceBase + "Api/FacFactNoElec/ConsultaDocumentoId/?CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario
                    + '&IdDocumento=' + IdDocumento;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    var _getGrabaDocumento = function (objData) {
        return $http.post(serviceBase + 'Api/FacFactNoElec/GrabaDocumento', objData).then(function (response) {
            return response;
        });
    };
    var _getGrabaAnulaDocumento = function (CodSap, Ruc, Usuario, IdDocumentoAnula) {
        Ruta = serviceBase + "Api/FacFactNoElec/GrabaAnulaDocumento/?CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario
                    + '&IdDocumentoAnula=' + IdDocumentoAnula;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    var _getValidaPermisoModFact = function (CodSap, Ruc, Usuario, NumPedido, FacEstabl, FacPtoEmi, FacNumSec) {
        Ruta = serviceBase + "Api/FacFactNoElec/ValidaPermisoModFact/?CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario
                    + '&NumPedido=' + NumPedido + '&FacEstabl=' + FacEstabl + '&FacPtoEmi=' + FacPtoEmi + '&FacNumSec=' + FacNumSec;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    //var _getGeneraFileXML = function (objTxtXML, tipo) {
    //    //return $http.get(serviceBase + 'Api/FacFactNoElec/GeneraFileTextoXML', objTxtXML).then(function (response) {
    //    return $http.post(serviceBase + 'Api/FacFactNoElec/GeneraFilesXmlPDF', objTxtXML, tipo).then(function (response) {
    //        return response;
    //    });
    //};
    //var _getGeneraFilePDF = function (objPDF, tipo) {
    //    //return $http.get(serviceBase + 'Api/FacFactNoElec/GeneraFilePDF', objPDF).then(function (response) {
    //    return $http.post(serviceBase + 'Api/FacFactNoElec/GeneraFilesXmlPDF', objPDF, tipo).then(function (response) {
    //        return response;
    //    });
    //};
    var _getGeneraFilesXmlPDF = function (objInfo) {
        return $http.post(serviceBase + 'Api/FacFactNoElec2/GeneraFilesXmlPDF', objInfo).then(function (response) {
            return response;
        });
    };

    var _getConsAlmacenes = function () {
        Ruta = serviceBase + "Api/PedConsPedidos/ConsAlmacenes?tipoLista=4";
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    FacturasServiceFactory.getCatalogo = _getCatalogo;

    FacturasServiceFactory.getConsSelPedidosFiltro = _getConsSelPedidosFiltro;
    FacturasServiceFactory.getConsRecupFacturasFiltro = _getConsRecupFacturasFiltro;
    FacturasServiceFactory.getConsultaPedidoNumero = _getConsultaPedidoNumero;
    FacturasServiceFactory.getConsultaDocumentoId = _getConsultaDocumentoId;
    FacturasServiceFactory.getGrabaDocumento = _getGrabaDocumento;
    FacturasServiceFactory.getGrabaAnulaDocumento = _getGrabaAnulaDocumento;
    FacturasServiceFactory.getValidaPermisoModFact = _getValidaPermisoModFact;
    //FacturasServiceFactory.getGeneraFileXML = _getGeneraFileXML;
    //FacturasServiceFactory.getGeneraFilePDF = _getGeneraFilePDF;
    FacturasServiceFactory.getGeneraFilesXmlPDF = _getGeneraFilesXmlPDF;
    FacturasServiceFactory.getConsAlmacenes = _getConsAlmacenes;
    FacturasServiceFactory.cargarParametros = _cargarParametros;
    FacturasServiceFactory.consProveedorExcepto = _consProveedorExcepto;
    


    return FacturasServiceFactory;

}]);
