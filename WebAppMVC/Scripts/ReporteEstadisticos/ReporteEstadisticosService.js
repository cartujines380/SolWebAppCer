
'use strict';
app.factory('ReporteEstadisticosService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ReporteEstadisticosServiceFactory = {};
    var Ruta = '';

  
    var _getConsAlmacenes = function (tipoLista,codigoSap) {
        Ruta = serviceBase + "Api/ReporteEstadisticos/ConsAlmacenesArticulosReportes?tipoLista=" + tipoLista + "&codSap=" + codigoSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getAnios = function (tipoLista, codigoSap) {
        Ruta = serviceBase + "Api/ReporteEstadisticos/GetAnios?tipoListacod=" + tipoLista + "&codSapproveedor=" + codigoSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsReporteEsta = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datos = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/ReporteEstadisticos/consReporteConsulta';
        return $http.post(Ruta, datos).then(function (response) {
            return response;
        });
    };


    var _getConsReporteMercado = function (Tablacabecera, TablaAlmacen) {

        var datosMercado = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen
        }
        Ruta = serviceBase + 'Api/ReporteMercado/consReporteMercado';
        return $http.post(Ruta, datosMercado).then(function (response) {
            return response;
        });
    };


    var _getConsReporteMercadoExp = function (Tablacabecera, TablaAlmacen) {

        var datosMercadoExp = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen
        }
        Ruta = serviceBase + 'Api/ReporteMercadeoExport/exportarReporteMercadeo';
        return $http.post(Ruta, datosMercadoExp, { responseType: 'arraybuffer' }).then(function (response) {
            return response;
        });

   
    };

    var _getConsReporteEvolucion = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datos = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/ReporteEvo/consReporteEvolucion';
        debugger;
        return $http.post(Ruta, datos).then(function (response) {
            return response;
        });
    };


    var _getConsReporteEstaCentroAlmacen = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datoscentro = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/ReporteEstadisticosCentroAlmacen/consReporteConsultaCentroAlmacen';
        return $http.post(Ruta, datoscentro).then(function (response) {
            return response;
        });
    };
    var _getconsultaStockTienda = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datosStock = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/StockTienda/consultaStockTienda';
        return $http.post(Ruta, datosStock).then(function (response) {
            return response;
        });
    };

    var _getconsultaStockTiendaAlamcen = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datosStockAlmacen = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/StockTiendaAlmacen/consultaStockTiendaAlamcen';
        return $http.post(Ruta, datosStockAlmacen).then(function (response) {
            return response;
        });
    };

    var _getconsReporteConsultaArticulo = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datosArt = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/ReporteEstadisticosArticulo/consReporteConsultaArticulo';
        return $http.post(Ruta, datosArt).then(function (response) {
            return response;
        });
    };


    var _getExportarData = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datosExportar = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/exportarReporteVentasCentro/exportarReporteVentas';
        return $http.post(Ruta, datosExportar, { responseType: 'arraybuffer' }).then(function (response) {
            return response;
        });
    };

    var _getExportarDataArticulo = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datosExportarArticulo = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/exportarReporteArticuloMes/exportarReporteArticulo';
        return $http.post(Ruta, datosExportarArticulo, { responseType: 'arraybuffer' }).then(function (response) {
            return response;
        });
    };

    var _getConsReporteVentaProvMensual = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datoscentro = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/ReporteVentaProvMensual/consReporteConsultaVentaProvMesual';
        return $http.post(Ruta, datoscentro).then(function (response) {
            return response;
        });
    };

    var _getExportarDataProvMensual = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datosExportar = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/exportarReporteVentasProvMensual/exportarReporteVentasProvMensual';
        return $http.post(Ruta, datosExportar, { responseType: 'arraybuffer' }).then(function (response) {
            return response;
        });
    };
  
    var _getexportarReporteStockAlmacena = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datosExportarStock = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/exportarReporteStockAlmacen/getexportarReporteStockAlmacena';
        return $http.post(Ruta, datosExportarStock, { responseType: 'arraybuffer' }).then(function (response) {
            return response;
        });
    };

    var _getexportarReporteStockAlmacenaArt = function (Tablacabecera, TablaAlmacen, TablaMaterial) {

        var datosExportarStockAr = {
            'p_reporteDatos': Tablacabecera,
            'p_reporteAlmacen': TablaAlmacen,
            'p_reporteMaterial': TablaMaterial,
        }
        Ruta = serviceBase + 'Api/exportarReporteStockAlmacenArticulo/getexportarReporteStockAlmacenaArt';
        return $http.post(Ruta, datosExportarStockAr, { responseType: 'arraybuffer' }).then(function (response) {
            return response;
        });
    };

    ReporteEstadisticosServiceFactory.getConsAlmacenes = _getConsAlmacenes;
    ReporteEstadisticosServiceFactory.getConsReporteEsta = _getConsReporteEsta;
    ReporteEstadisticosServiceFactory.getConsReporteEstaCentroAlmacen = _getConsReporteEstaCentroAlmacen;
    ReporteEstadisticosServiceFactory.getconsReporteConsultaArticulo = _getconsReporteConsultaArticulo;
    ReporteEstadisticosServiceFactory.getconsultaStockTienda = _getconsultaStockTienda;
    ReporteEstadisticosServiceFactory.getExportarData = _getExportarData;
    ReporteEstadisticosServiceFactory.getExportarDataArticulo = _getExportarDataArticulo;
    ReporteEstadisticosServiceFactory.getConsReporteEvolucion = _getConsReporteEvolucion;
    ReporteEstadisticosServiceFactory.getConsReporteVentaProvMensual = _getConsReporteVentaProvMensual
    ReporteEstadisticosServiceFactory.getExportarDataProvMensual = _getExportarDataProvMensual;
    ReporteEstadisticosServiceFactory.getAnios = _getAnios;
    ReporteEstadisticosServiceFactory.getexportarReporteStockAlmacena = _getexportarReporteStockAlmacena;
    ReporteEstadisticosServiceFactory.getconsultaStockTiendaAlamcen = _getconsultaStockTiendaAlamcen;
    ReporteEstadisticosServiceFactory.getexportarReporteStockAlmacenaArt = _getexportarReporteStockAlmacenaArt;


    ReporteEstadisticosServiceFactory.getConsReporteMercado = _getConsReporteMercado;
    ReporteEstadisticosServiceFactory.getConsReporteMercadoExp = _getConsReporteMercadoExp;
    

    
    return ReporteEstadisticosServiceFactory;

}]);
