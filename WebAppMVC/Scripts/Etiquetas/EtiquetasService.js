
'use strict';
app.factory('EtiquetasService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var EtiquetasServiceFactory = {};
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
    var _geteliminarSolicitudEti = function (opcionEli, idPedidoEli, estadoEli) {
        Ruta = serviceBase + "Api/PedConsEtiPedidos/geteliminarSolicitudEti?opcionEli=" + opcionEli + "&idPedidoEli=" + idPedidoEli + "&estadoEli=" + estadoEli;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getActualizarSolicitudEtiquetas = function (tipoTra, pgcDgPedidosDET) {
        var EtiquetasACtualizar = {
            'p_transaccion': tipoTra,
            'p_Etiquetas': pgcDgPedidosDET
        }
        Ruta = serviceBase + "Api/Ped_ConsEtiPedidosActualizar/getActualizarSolicitudEtiquetas/"
        return $http.post(Ruta, EtiquetasACtualizar).then(function (results) {
            return results;
        });
    };
    var _getPedidoSolicitudprin = function (idSolicitud, opcion) {
        Ruta = serviceBase + "Api/PedConsEtiPedidos/getPedidoSolicitudprin?idSolicitudpri=" + idSolicitud + "&opcionpri=" + opcion;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    
    var _getPedidoSolicitud = function (idSolicitud, opcion) {
        Ruta = serviceBase + "Api/PedConsEtiPedidos/getPedidoSolicitud?idSolicitud=" + idSolicitud + "&opcion=" + opcion;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBuscarAsignacion = function (opcion, codsap) {
        Ruta = serviceBase + "Api/PedConsEtiPedidos/getBuscarAsignacion/?opcion=" + opcion + '&codsap=' + codsap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getConsPedidosFiltro = function (CodSap, Ruc, Usuario, Opc1, Opc2, Fecha1, Fecha2, Ciudad, NumOrden, SiGrd, SiTxt, SiXml, SiHtml, SiPdf, Almacen,tipoPedido) {
        if (Almacen == undefined)
            Almacen = "";
        Ruta = serviceBase + "Api/PedConsEtiPedidos/ConsPedidosEtiFiltro/?CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario
                    + '&Opc1=' + Opc1 + '&Opc2=' + Opc2 + '&Fecha1=' + Fecha1 + '&Fecha2=' + Fecha2 + '&Ciudad=' + Ciudad
                    + '&NumOrden=' + NumOrden + '&SiGrd=' + SiGrd + '&SiTxt=' + SiTxt + '&SiXml=' + SiXml + '&SiHtml=' + SiHtml + '&SiPdf=' + SiPdf + '&Almacen=' + Almacen + '&tipoPedido=' + tipoPedido;
        return $http.get(Ruta) .then(function (results) {
            return results;
        });
    };

    var _getConsPedEti = function (Opc1, Fecha1, Fecha2, Estado, NumOrden, codigosap) {
        Ruta = serviceBase + "Api/PedConsEtiPedidos/consPedEti/?Opc1=" + Opc1 + '&Fecha1=' + Fecha1 + '&Fecha2=' + Fecha2 
                    + '&Estado=' + Estado + '&NumOrden=' + NumOrden + '&codigosap=' + codigosap;
        return $http.get(Ruta) .then(function (results) {
            return results;
        });
    };

    var _getConsPedEtiImpresas = function (Fecha1, Fecha2, codigosap) {
        Ruta = serviceBase + "Api/PedConsEtiPedidos/consPedEtiImpresas/?Fecha1=" + Fecha1 + '&Fecha2=' + Fecha2 + '&codigosap=' + codigosap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _catalogoCatalogo = function (tipo, IdCatalogo) {

        Ruta = serviceBase + "Api/Tolerancia/catalogoTipoPedido/?tipo=" + tipo + "&IdCatalogo=" + IdCatalogo;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    }

    var _getExportarDataImpresas = function (tipoEt,usuarioEt, nombreEmp, Fecha1, Fecha2, CodSap) {


        Ruta = serviceBase + "Api/PedConsEtiPedidos/ExportarDataImpresas/";


        return $http.get(Ruta, { params: {tipoEt:tipoEt, usuarioEt: usuarioEt, nombreEmp:nombreEmp, Fecha1Et: Fecha1, Fecha2Et: Fecha2, CodSapEt: CodSap }, responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });

    };
    var _getExportarData = function (CodSap, Ruc, Usuario, Opc1, Opc2, Fecha1, Fecha2, Ciudad, NumOrden,Almacen, tipo) {
      if (Almacen == undefined)
            Almacen = "";
      //Ruta = serviceBase + "Api/PedConsPedidos/exptPedidosFiltro/?CodSapn=" + CodSap + '&Rucn=' + Ruc + '&Usuarion=' + Usuario
      //              + '&Opc1n=' + Opc1 + '&Opc2n=' + Opc2 + '&Fecha1n=' + Fecha1 + '&Fecha2n=' + Fecha2 + '&Ciudadn=' + Ciudad
      //              + '&NumOrdenn=' + NumOrden + '&Almacenn=' + Almacen + '&Tipo=' + tipo;
      //  return $http.get(Ruta).then(function (results) {
      //      return results;
      //  });
      Ruta = serviceBase + "Api/PedConsPedidos/exptPedidosFiltro/";
          
 
          return $http.get(Ruta,{ params: { CodSapn:CodSap,Rucn:Ruc,Usuarion:Usuario,Opc1n:Opc1,Opc2n:Opc2,Fecha1n:Fecha1,Fecha2n:Fecha2,Ciudadn:Ciudad,NumOrdenn:NumOrden,Almacenn:Almacen,Tipo:tipo},responseType: 'arraybuffer'}).then(function (results) {
          return results;
      });

    };

    var _getUrlRedireccSiteFactManual = function (Opc, CodSap, Ruc, Usuario) {
        Ruta = serviceBase + "Api/PedConsPedidos/UrlRedireccSiteFactManual/?Opc=" + Opc + "&CodSap=" + CodSap + '&Ruc=' + Ruc + '&Usuario=' + Usuario;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _mntSolicitud = function (solicitud) {
        var Etiquetas = {
            'p_Etiquetas': solicitud
        }
        Ruta = serviceBase + "Api/PedConsEtiPedidos/mantSolicitudEti/";
        //.opcion + "&idSol=" + solicitud.idSolicitud + '&idPed=' + solicitud.idPedido + '&numPed=' + solicitud.numPedido + '&codArt=' + solicitud.codArticulo + '&canDes=' + solicitud.canDespachar + '&fe=' + solicitud.fechaEntrega;

        return $http.post(Ruta, Etiquetas).then(function (results) {
            return results;
        });


        //return $http.get(Ruta).then(function (results) {
        //    return results;
        //});
    };

    var _consSolicitud = function (opc, idPed, codArt) {
        debugger;
        Ruta = serviceBase + "Api/PedConsEtiPedidos/consSolicitudEti/?Opc=" + opc + '&idPed=' + idPed + '&codArt=' + codArt;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    EtiquetasServiceFactory.getCatalogo = _getCatalogo;

    EtiquetasServiceFactory.getConsCiudadesEnAlmacen = _getConsCiudadesEnAlmacen;
    EtiquetasServiceFactory.getConsAlmacenes = _getConsAlmacenes;
    EtiquetasServiceFactory.getConsPedidosFiltro = _getConsPedidosFiltro;

    EtiquetasServiceFactory.getUrlRedireccSiteFactManual = _getUrlRedireccSiteFactManual;
    EtiquetasServiceFactory.getExportarData = _getExportarData;
    EtiquetasServiceFactory.getExportarDataImpresas = _getExportarDataImpresas;
    EtiquetasServiceFactory.getBuscarAsignacion = _getBuscarAsignacion;
    
    EtiquetasServiceFactory.mntSolicitud = _mntSolicitud;
    EtiquetasServiceFactory.consSolicitud = _consSolicitud;
    EtiquetasServiceFactory.catalogoCatalogo = _catalogoCatalogo;
    
    EtiquetasServiceFactory.getConsPedEti = _getConsPedEti;
    EtiquetasServiceFactory.getConsPedEtiImpresas = _getConsPedEtiImpresas;
    EtiquetasServiceFactory.getPedidoSolicitud = _getPedidoSolicitud;
    EtiquetasServiceFactory.getPedidoSolicitudprin = _getPedidoSolicitudprin;
    EtiquetasServiceFactory.geteliminarSolicitudEti = _geteliminarSolicitudEti;
    EtiquetasServiceFactory.getActualizarSolicitudEtiquetas = _getActualizarSolicitudEtiquetas;

    return EtiquetasServiceFactory;

}]);
