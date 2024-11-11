
'use strict';
app.factory('PedidosService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var PedidosServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {
        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsCiudadesEnAlmacen = function () {
        Ruta = serviceBase + "Api/PedConsPedidos/ConsCiudadesEnAlmacen";
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsAlmacenes = function (tipoLista) {
        Ruta = serviceBase + "Api/PedConsPedidos/ConsAlmacenes?tipoLista=" + tipoLista;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsPedidosFiltro = function (CodSap, Ruc, Usuario, Opc1, Opc2, Fecha1, Fecha2, Ciudad, NumOrden, SiGrd, SiTxt, SiXml, SiHtml, SiPdf, Almacen) {
        if (Almacen == undefined)
            Almacen = "";
        Ruta = serviceBase + "Api/PedConsPedidos/ConsPedidosFiltro/?CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario
                    + '&Opc1=' + Opc1 + '&Opc2=' + Opc2 + '&Fecha1=' + Fecha1 + '&Fecha2=' + Fecha2 + '&Ciudad=' + Ciudad
                    + '&NumOrden=' + NumOrden + '&SiGrd=' + SiGrd + '&SiTxt=' + SiTxt + '&SiXml=' + SiXml + '&SiHtml=' + SiHtml  + '&SiPdf=' + SiPdf + '&Almacen=' + Almacen;
        return $http.get(Ruta) .then(function (results) {
            return results;
        });
    };

    var _getExportarDataupd = function (CodSap, Ruc, Usuario, Opc1, Opc2, Fecha1, Fecha2, Ciudad, NumOrden, Almacen, tipo) {
 
        Ruta = serviceBase + 'Api/PedConsPedidos/exptPedidosFiltro';
      return $http.get(Ruta, { params: { CodSapn: CodSap, Rucn: Ruc, Usuarion: Usuario, Opc1n: Opc1, Opc2n: Opc2, Fecha1n: Fecha1, Fecha2n: Fecha2, Ciudadn: Ciudad, NumOrdenn: NumOrden, Almacenn: Almacen, Tipo: tipo }, responseType: 'arraybuffer' }).then(function (results) {
          return results;
      });

    };

    var _getExportarData = function (Tablacabecera, Tabladetalle) {

        //Ruta = serviceBase + "Api/PedConsPedidos/exptPedidosFiltro/?CodSapn=" + CodSap + '&Rucn=' + Ruc + '&Usuarion=' + Usuario
        //              + '&Opc1n=' + Opc1 + '&Opc2n=' + Opc2 + '&Fecha1n=' + Fecha1 + '&Fecha2n=' + Fecha2 + '&Ciudadn=' + Ciudad
        //              + '&NumOrdenn=' + NumOrden + '&Almacenn=' + Almacen + '&Tipo=' + tipo;
        //  return $http.get(Ruta).then(function (results) {
        //      return results;
        //  });
        var Lista = {
            'p_cabecera': Tablacabecera,
            'p_detalle': Tabladetalle
        }
        //Ruta = serviceBase + 'Api/PedConsPedidos/exptPedidosFiltro';
        //return $http.get(Ruta, { params: { Lista: Listag }, responseType: 'arraybuffer' }).then(function (response) {
        //    return response;
        //});

        Ruta = serviceBase + 'Api/PedConsPedidos/exptPedidosFiltro';
        return $http.post(Ruta, Lista, { responseType: 'arraybuffer' }).then(function (response) {
            return response;
        });
        //return $http.get(Ruta, { params: { CodSapn: CodSap, Rucn: Ruc, Usuarion: Usuario, Opc1n: Opc1, Opc2n: Opc2, Fecha1n: Fecha1, Fecha2n: Fecha2, Ciudadn: Ciudad, NumOrdenn: NumOrden, Almacenn: Almacen, Tipo: tipo }, responseType: 'arraybuffer' }).then(function (results) {
        //    return results;
        //});

    };
    var _getUrlRedireccSiteFactManual = function (Opc, CodSap, Ruc, Usuario) {
        Ruta = serviceBase + "Api/PedConsPedidos/UrlRedireccSiteFactManual/?Opc=" + Opc + "&CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsPedidosCrossFiltro = function (CodSap, Ruc, Usuario, Opc1, Opc2, Fecha1, Fecha2, Ciudad, NumOrden, SiGrd, SiTxt, SiXml, SiHtml, SiPdf, Almacen, isCross) {
        if (Almacen == undefined)
            Almacen = "";
        if (isCross == undefined) isCross = false;
        Ruta = serviceBase + "Api/PedPedidosCross/ConsPedidosCrossFiltro/?CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario
                    + '&Opc1=' + Opc1 + '&Opc2=' + Opc2 + '&Fecha1=' + Fecha1 + '&Fecha2=' + Fecha2 + '&Ciudad=' + Ciudad
                    + '&NumOrden=' + NumOrden + '&SiGrd=' + SiGrd + '&SiTxt=' + SiTxt + '&SiXml=' + SiXml + '&SiHtml=' + SiHtml
                    + '&SiPdf=' + SiPdf + '&Almacen=' + Almacen + '&isCross=' + isCross;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    PedidosServiceFactory.getCatalogo = _getCatalogo;

    PedidosServiceFactory.getConsCiudadesEnAlmacen = _getConsCiudadesEnAlmacen;
    PedidosServiceFactory.getConsAlmacenes = _getConsAlmacenes;
    PedidosServiceFactory.getConsPedidosFiltro = _getConsPedidosFiltro;

    PedidosServiceFactory.getUrlRedireccSiteFactManual = _getUrlRedireccSiteFactManual;
    PedidosServiceFactory.getExportarData = _getExportarData;
    PedidosServiceFactory.getExportarDataupd = _getExportarDataupd;
    PedidosServiceFactory.getConsPedidosCrossFiltro = _getConsPedidosCrossFiltro;
    
    return PedidosServiceFactory;

}]);
