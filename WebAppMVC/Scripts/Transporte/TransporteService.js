//Servicio Choferes Proveedor
'use strict';
app.factory('TransporteProveedorService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var TransporteProveedorServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    //catalogoServiceFactory.getCatalogo = _getCatalogo;

    var _getConsulaGrid = function (tipo, licencia, nombre, apellido, tipoestado, CodSap) {

        Ruta = serviceBase + "Api/MantenimientoTransporte/consBuscarChoferes/?tipo=" + tipo + '&licencia=' + licencia + '&nombre=' + nombre + '&apellido=' + apellido + '&tipoestado=' + tipoestado + '&CodSap=' + CodSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getConsulaGridUno = function (tipo, idChofer, CodSap) {

        Ruta = serviceBase + "Api/MantenimientoTransporte/consBuscarChoferesUno/?tipo=" + tipo + '&idChofer=' + idChofer + '&CodSap=' + CodSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsulaGridUnorucced = function (tipo, rucced) {

        Ruta = serviceBase + "Api/MantenimientoTransporte/consBuscarChoferesUnoRUCCED/?tipo=" + tipo + '&rucced=' + rucced
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabaTraCho = function (traCho) {
        Ruta = serviceBase + 'Api/MantenimientoTransporte/grabarTransporte/';
        //return $http.get(Ruta, userData).then(function (results) {
        return $http.post(Ruta, traCho).then(function (response) {
            return response;
        });
    };

    var _getActuaTraCho = function (traCho) {
        Ruta = serviceBase + 'Api/MantenimientoTransporte/actualizarTransporte/';
        //return $http.get(Ruta, userData).then(function (results) {
        return $http.post(Ruta, traCho).then(function (response) {
            return response;
        });
    };



    var _getthefile = function (identificacion, archivo) {
        Ruta = serviceBase + 'api/MantenimientoTransporte/bajararchivo/?identificacion=' + identificacion + '&archivo=' + archivo
        return $http.get(Ruta).then(function (results) {
            return results;
        });

    };
    var _getSecuenciaDirectorio = function (pTabla) {

        Ruta = serviceBase + "Api/MantenimientoTransporte/secuenciaDirectorio/?tipo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarDataChofer = function (Tablacabecera, Tabladetalle) {
        var Chofer = {
            'p_cabeceraChofer': Tablacabecera,
            'p_detalleChofer': Tabladetalle
        }
        Ruta = serviceBase + 'Api/ReporteChofer/ExportarChofer';
        return $http.post(Ruta, Chofer).then(function (response) {
            return response;
        });

    };

    TransporteProveedorServiceFactory.getConsulaGridUno = _getConsulaGridUno;
    TransporteProveedorServiceFactory.getConsulaGrid = _getConsulaGrid;
    TransporteProveedorServiceFactory.getCatalogo = _getCatalogo;
    TransporteProveedorServiceFactory.getGrabaTraCho = _getGrabaTraCho;
    TransporteProveedorServiceFactory.getActuaTraCho = _getActuaTraCho;
    TransporteProveedorServiceFactory.getthefile = _getthefile;
    TransporteProveedorServiceFactory.getSecuenciaDirectorio = _getSecuenciaDirectorio;
    TransporteProveedorServiceFactory.getConsulaGridUnorucced = _getConsulaGridUnorucced;
    TransporteProveedorServiceFactory.getExportarDataChofer = _getExportarDataChofer;



    return TransporteProveedorServiceFactory;

}]);

//Servicio Choferes Administrativo
'use strict';
app.factory('TransporteAdministrativoService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var TransporteAdministrativoServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    //catalogoServiceFactory.getCatalogo = _getCatalogo;

    var _getConsulaGrid = function (tipo, licencia, nombre, apellido, txtRucProveedor, CodSap) {

        Ruta = serviceBase + "Api/MantenimientoTransporte/consBuscarChoferesAdmi/?tipo=" + tipo + '&licencia=' + licencia + '&nombre=' + nombre + '&apellido=' + apellido + '&rucproveedor=' + txtRucProveedor + '&CodSap=' + CodSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getConsulaGridUno = function (tipo, idChofer, CodSap) {

        Ruta = serviceBase + "Api/MantenimientoTransporte/consBuscarChoferesUno/?tipo=" + tipo + '&idChofer=' + idChofer + '&CodSap=' + CodSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabaTraCho = function (traCho) {
        Ruta = serviceBase + 'Api/MantenimientoTransporte/grabarTransporte/';
        return $http.post(Ruta, traCho).then(function (response) {
            return response;
        });
    };

    var _getActuaTraCho = function (traCho) {
        Ruta = serviceBase + 'Api/MantenimientoTransporte/actualizarTransporte/';
        return $http.post(Ruta, traCho).then(function (response) {
            return response;
        });
    };

    var _getthefile = function (identificacion, archivo) {
        Ruta = serviceBase + 'api/MantenimientoTransporte/bajararchivo/?identificacion=' + identificacion + '&archivo=' + archivo
        return $http.get(Ruta).then(function (results) {
            return results;
        });

    };
    var _getSecuenciaDirectorio = function (pTabla) {

        Ruta = serviceBase + "Api/MantenimientoTransporte/secuenciaDirectorio/?tipo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarDataChofer = function (Tablacabecera, Tabladetalle) {
        var Chofer = {
            'p_cabeceraChofer': Tablacabecera,
            'p_detalleChofer': Tabladetalle
        }
        Ruta = serviceBase + 'Api/ReporteChofer/ExportarChofer';
        return $http.post(Ruta, Chofer).then(function (response) {
            return response;
        });

    };


    TransporteAdministrativoServiceFactory.getConsulaGridUno = _getConsulaGridUno;
    TransporteAdministrativoServiceFactory.getConsulaGrid = _getConsulaGrid;
    TransporteAdministrativoServiceFactory.getCatalogo = _getCatalogo;
    TransporteAdministrativoServiceFactory.getGrabaTraCho = _getGrabaTraCho;
    TransporteAdministrativoServiceFactory.getActuaTraCho = _getActuaTraCho;
    TransporteAdministrativoServiceFactory.getthefile = _getthefile;
    TransporteAdministrativoServiceFactory.getSecuenciaDirectorio = _getSecuenciaDirectorio;
    TransporteAdministrativoServiceFactory.getExportarDataChofer = _getExportarDataChofer;
    return TransporteAdministrativoServiceFactory;

}]);

//Servicio Vehiculos Proveedor
'use strict';
app.factory('VehiculosProveedorService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var VehiculoProveedorServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    //catalogoServiceFactory.getCatalogo = _getCatalogo;

    var _getConsulaGrid = function (tipo, tipovehiculo, propietario, numplaca, tipoestado, CodSap) {
        Ruta = serviceBase + "Api/MantenimientoVehiculo/consBuscarVehiculo/?tipo=" + tipo + '&tipovehiculo=' + tipovehiculo + '&propietario=' + propietario + '&numplaca=' + numplaca + '&tipoestado=' + tipoestado + '&CodSap=' + CodSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });

    };


    var _getConsulaGridUno = function (tipo, idVehiculo, CodSap) {

        Ruta = serviceBase + "Api/MantenimientoVehiculo/consBuscarVehiculoUno/?tipo=" + tipo + '&idVehiculo=' + idVehiculo + '&CodSap=' + CodSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsulaGridUnorucced = function (tipo, placav) {

        Ruta = serviceBase + "Api/MantenimientoVehiculo/consBuscarVehiculoUnoRUCCED/?tipov=" + tipo + '&placav=' + placav
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabaTraCho = function (traCho) {
        Ruta = serviceBase + 'Api/MantenimientoVehiculo/grabarVehiculo/';
        return $http.post(Ruta, traCho).then(function (response) {
            return response;
        });
    };

    var _getActuaTraCho = function (traCho) {
        Ruta = serviceBase + 'Api/MantenimientoVehiculo/actualizarTransporte/';
        return $http.post(Ruta, traCho).then(function (response) {
            return response;
        });
    };

    var _getthefile = function (identificacion, archivo) {
        Ruta = serviceBase + 'api/MantenimientoVehiculo/bajararchivo/?identificacion=' + identificacion + '&archivo=' + archivo
        return $http.get(Ruta).then(function (results) {
            return results;
        });

    };

    var _getSecuenciaDirectorio = function (pTabla) {

        Ruta = serviceBase + "Api/MantenimientoTransporte/secuenciaDirectorio/?tipo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarDataVehiculo = function (Tablacabecera, Tabladetalle) {
        var Vehiculo = {
            'p_cabeceraVehiculo': Tablacabecera,
            'p_detalleVehiculo': Tabladetalle
        }
        Ruta = serviceBase + 'Api/ReporteVehiculo/ExportarVehiculo';
        return $http.post(Ruta, Vehiculo).then(function (response) {
            return response;
        });

    };

    VehiculoProveedorServiceFactory.getConsulaGridUno = _getConsulaGridUno;
    VehiculoProveedorServiceFactory.getConsulaGrid = _getConsulaGrid;
    VehiculoProveedorServiceFactory.getCatalogo = _getCatalogo;
    VehiculoProveedorServiceFactory.getGrabaTraCho = _getGrabaTraCho;
    VehiculoProveedorServiceFactory.getActuaTraCho = _getActuaTraCho;
    VehiculoProveedorServiceFactory.getthefile = _getthefile;
    VehiculoProveedorServiceFactory.getSecuenciaDirectorio = _getSecuenciaDirectorio;
    VehiculoProveedorServiceFactory.getExportarDataVehiculo = _getExportarDataVehiculo;
    VehiculoProveedorServiceFactory.getConsulaGridUnorucced = _getConsulaGridUnorucced;



    return VehiculoProveedorServiceFactory;

}]);

//Servicio Vehiculos Administrativo
'use strict';
app.factory('VehiculosAdministrativoService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var VehiculoAdministrativoServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    //catalogoServiceFactory.getCatalogo = _getCatalogo;

    var _getConsulaGrid = function (tipo, tipovehiculo, propietario, numplaca, tipoestado, CodSap) {

        Ruta = serviceBase + "Api/MantenimientoVehiculo/consBuscarVehiculo/?tipo=" + tipo + '&tipovehiculo=' + tipovehiculo + '&propietario=' + propietario + '&numplaca=' + numplaca + '&tipoestado=' + tipoestado + '&CodSap=' + CodSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getConsulaGridUno = function (tipo, idVehiculo, CodSap) {
        Ruta = serviceBase + "Api/MantenimientoVehiculo/consBuscarVehiculoUno/?tipo=" + tipo + '&idVehiculo=' + idVehiculo + '&CodSap=' + CodSap;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabaTraCho = function (traCho) {
        Ruta = serviceBase + 'Api/MantenimientoVehiculo/grabarVehiculo/';
        return $http.post(Ruta, traCho).then(function (response) {
            return response;
        });
    };

    var _getActuaTraCho = function (traCho) {
        Ruta = serviceBase + 'Api/MantenimientoTransporte/actualizarTransporte/';
        return $http.post(Ruta, traCho).then(function (response) {
            return response;
        });
    };
    var _getthefile = function (identificacion, archivo) {
        Ruta = serviceBase + 'api/MantenimientoVehiculo/bajararchivo/?identificacion=' + identificacion + '&archivo=' + archivo
        return $http.get(Ruta).then(function (results) {
            return results;
        });

    };


    var _getSecuenciaDirectorio = function (pTabla) {
        Ruta = serviceBase + "Api/MantenimientoTransporte/secuenciaDirectorio/?tipo=" + pTabla;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarDataVehiculo = function (Tablacabecera, Tabladetalle) {
        var Vehiculo = {
            'p_cabeceraVehiculo': Tablacabecera,
            'p_detalleVehiculo': Tabladetalle
        }
        Ruta = serviceBase + 'Api/ReporteVehiculo/ExportarVehiculo';
        return $http.post(Ruta, Vehiculo).then(function (response) {
            return response;
        });

    };

    VehiculoAdministrativoServiceFactory.getConsulaGridUno = _getConsulaGridUno;
    VehiculoAdministrativoServiceFactory.getConsulaGrid = _getConsulaGrid;
    VehiculoAdministrativoServiceFactory.getCatalogo = _getCatalogo;
    VehiculoAdministrativoServiceFactory.getGrabaTraCho = _getGrabaTraCho;
    VehiculoAdministrativoServiceFactory.getActuaTraCho = _getActuaTraCho;
    VehiculoAdministrativoServiceFactory.getthefile = _getthefile;
    VehiculoAdministrativoServiceFactory.getSecuenciaDirectorio = _getSecuenciaDirectorio;
    VehiculoAdministrativoServiceFactory.getExportarDataVehiculo = _getExportarDataVehiculo;


    return VehiculoAdministrativoServiceFactory;

}]);

//Servicio Consolidacion Pedidos
'use strict';
app.factory('ConsolidacionPedidosService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ConsolidacionPedidosServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getChoferVehiculos = function (tipo, codproveedor) {
        Ruta = serviceBase + "Api/ConsolidacionPedidos/ChoferVehiculo/?tipoid=" + tipo + '&codproveedor=' + codproveedor;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBuscarPedidosBodega = function (tipo, codproveedor,bodega) {
        
        Ruta = serviceBase + "Api/ConsolidacionPedidos/BuscarPedidosBodega/?tipoPe=" + tipo + '&codproveedorPe=' + codproveedor + '&Bodega=' + bodega;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBuscarPedidos = function (tipo, codproveedor) {

        Ruta = serviceBase + "Api/ConsolidacionPedidos/BuscarPedidos/?tipoPe=" + tipo + '&codproveedorPe=' + codproveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBuscarDetPedidos = function (tipo, numPedido) {

        Ruta = serviceBase + "Api/ConsolidacionPedidos/BuscarDetPedidos/?tipoPeD=" + tipo + '&numPedido=' + numPedido;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBuscarConsolidacion = function (numero, codproveedor, estado, fechadesde, fechahasta) {
        debugger;
        Ruta = serviceBase + "Api/ConsolidacionPedidos/BuscarConsolidacion/?numero=" + numero + '&codProveedorConso=' + codproveedor + '&estado=' + estado + '&fechadesde=' + fechadesde + '&fechahasta=' + fechahasta;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBuscarConsolidacionAdmin = function (numero, codproveedor, estado, fechadesde, fechahasta) {

        Ruta = serviceBase + "Api/ConsolidacionPedidos/BuscarConsolidacionAdmin/?numeroAdmin=" + numero + '&codProveedorConsoAdmin=' + codproveedor + '&estadoAdmin=' + estado + '&fechadesdeAdmin=' + fechadesde + '&fechahastaAdmin=' + fechahasta;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getGrabaConsolidacion = function (Cabconsolidacion, Pedidos, DetPedidos) {
        var Consolidacion = {
            'p_consolidacion': Cabconsolidacion,
            'p_Pedidos': Pedidos,
            'p_DetPedidos': DetPedidos
        }
        Ruta = serviceBase + 'Api/ConsolidacionPedidos/grabarConsolidacion';
        return $http.post(Ruta, Consolidacion).then(function (response) {
            return response;
        });
    };

    var _getEliminarConsolidacion = function (idconsolidacion, codproveedor) {

        Ruta = serviceBase + "Api/ConsolidacionPedidos/EliminarConsolidacion/?idconsolidacion=" + idconsolidacion + '&codProveedorEliConso=' + codproveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getModificarConsolidacion = function (idconsolidacion, codproveedor) {

        Ruta = serviceBase + "Api/ConsolidacionPedidos/BuscarConModi/?idconsomodi=" + idconsolidacion + '&codproveedormodi=' + codproveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarDataConsolidacion = function (Tablacabecera, Tabladetalle) {
        var Consolidacion = {
            'p_cabeceraConsolidacion': Tablacabecera,
            'p_detalleConsolidacion': Tabladetalle
        }
        Ruta = serviceBase + 'Api/ReporteConsolidacion/ExportarConsolidacion';
        return $http.post(Ruta, Consolidacion).then(function (response) {
            return response;
        });

    };

    var _getBuscarDetPedidosCross = function (numPedido, codProveedor) {

        Ruta = serviceBase + "Api/PedPedidosCross/ConsPedidosCrossDetalle/?NumOrden=" + numPedido + '&CodProveedor=' + codProveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBuscarDetPedidosCrossFlow = function (numPedido, codProveedor, tipoPedidos) {

        Ruta = serviceBase + "Api/PedPedidosCross/ConsPedidosCrossDetalleCrossFlow/?NumOrden=" + numPedido + '&CodProveedor=' + codProveedor + '&tipoPedidos=' + tipoPedidos;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getActualizaPedidoCross = function (detallePedido, estado, usuario) {
        var PedidoCross = {
            'p_estado': estado,
            'p_usuario': usuario,
            'p_detallePedidos': detallePedido
        }
        Ruta = serviceBase + "Api/PedPedidosCross/ActualizaPedidoCross";
        return $http.post(Ruta, PedidoCross).then(function (response) {
            return response;
        });
    };

    var _getActualizaPedidoCrossFlow = function (detallePedido, estado, usuario) {
        var PedidoCrossFlow = {
            'p_estado': estado,
            'p_usuario': usuario,
            'p_detallePedidos': detallePedido
        }
        Ruta = serviceBase + "Api/PedPedidosFlow/ActualizaPedidoCrossFlow";
        return $http.post(Ruta, PedidoCrossFlow).then(function (response) {
            return response;
        });
    };

    ConsolidacionPedidosServiceFactory.getGrabaConsolidacion = _getGrabaConsolidacion;
    ConsolidacionPedidosServiceFactory.getCatalogo = _getCatalogo;
    ConsolidacionPedidosServiceFactory.getChoferVehiculos = _getChoferVehiculos;
    ConsolidacionPedidosServiceFactory.getBuscarPedidos = _getBuscarPedidos;
    ConsolidacionPedidosServiceFactory.getBuscarPedidosBodega = _getBuscarPedidosBodega;
    ConsolidacionPedidosServiceFactory.getBuscarConsolidacion = _getBuscarConsolidacion;
    ConsolidacionPedidosServiceFactory.getBuscarConsolidacionAdmin = _getBuscarConsolidacionAdmin;
    ConsolidacionPedidosServiceFactory.getBuscarDetPedidos = _getBuscarDetPedidos;
    ConsolidacionPedidosServiceFactory.getEliminarConsolidacion = _getEliminarConsolidacion;
    ConsolidacionPedidosServiceFactory.getModificarConsolidacion = _getModificarConsolidacion;
    ConsolidacionPedidosServiceFactory.getExportarDataConsolidacion = _getExportarDataConsolidacion;
    ConsolidacionPedidosServiceFactory.getBuscarDetPedidosCross = _getBuscarDetPedidosCross;
    ConsolidacionPedidosServiceFactory.getActualizaPedidoCross = _getActualizaPedidoCross;
    ConsolidacionPedidosServiceFactory.getActualizaPedidoCrossFlow = _getActualizaPedidoCrossFlow;
    ConsolidacionPedidosServiceFactory.getBuscarDetPedidosCrossFlow = _getBuscarDetPedidosCrossFlow;



    return ConsolidacionPedidosServiceFactory;

}]);

//Servicio Solicitud Cita
'use strict';
app.factory('SolicitudCitaService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var SolicitudCitaServiceFactory = {};
    var Ruta = '';



    var _getConsulaGrid = function (tipo, fechadesde, fechahasta, numero, CodSap,bodega) {

        Ruta = serviceBase + "Api/SolicitudCita/conBuscarSolicitud/?tipo=" + tipo + '&fechadesde=' + fechadesde + '&fechahasta=' + fechahasta + '&numero=' + numero + '&codProveedor=' + CodSap + '&bodega=' + bodega;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getCargarHorario = function (bodega, CodSap,fecha) {

        Ruta = serviceBase + "Api/SolicitudCita/getCargarHorario/?bodega=" + bodega + "&codProveedor=" + CodSap + "&fecha=" + fecha;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGenerarCita = function (idconsolidacion, fechacita, horainicial, horafinal, CodSap, usuarioproveedor) {
        Ruta = serviceBase + "Api/SolicitudCita/generarCita/?idconsolidacion=" + idconsolidacion + '&fechacita=' + fechacita + '&horainicial=' + horainicial + '&horafinal=' + horafinal + '&CodSap=' + CodSap + '&usuarioproveedor=' + usuarioproveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getdatos = function (id, citas) {
        Ruta = serviceBase + "Api/SolicitudCita/getdatos/?id=" + id + '&citas=' + citas;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarDataCita = function (Tablacabecera, Tabladetalle) {
        var Chofer = {
            'p_cabeceraCita': Tablacabecera,
            'p_detalleCita': Tabladetalle
        }
        Ruta = serviceBase + 'Api/ReporteCita/ExportarCita';
        return $http.post(Ruta, Chofer).then(function (response) {
            return response;
        });

    };

    SolicitudCitaServiceFactory.getConsulaGrid = _getConsulaGrid;
    SolicitudCitaServiceFactory.getGenerarCita = _getGenerarCita;
    SolicitudCitaServiceFactory.getdatos = _getdatos;
    SolicitudCitaServiceFactory.getExportarDataCita = _getExportarDataCita;
    SolicitudCitaServiceFactory.getCargarHorario = _getCargarHorario;


    return SolicitudCitaServiceFactory;

}]);

//Servicio Aprobacion Citas
'use strict';
app.factory('AprobacionCitasService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {


    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var AprobacionCitaServiceFactory = {};
    var Ruta = '';


    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    var _getConsulaGrid = function (tipo, fechadesde, fechahasta, numero, estado, CodSap, ruc) {

        Ruta = serviceBase + "Api/AprobacionCitas/conBuscarAprobacionCitas/?tipo=" + tipo + '&fechadesde=' + fechadesde + '&fechahasta=' + fechahasta + '&numero=' + numero + '&estado=' + estado + '&codProveedor=' + CodSap + '&ruc=' + ruc;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsulaGridDetallePedido = function (tipo, id) {

        Ruta = serviceBase + "Api/AprobacionCitas/conBuscarDetalleGrid/?tipo=" + tipo + '&id=' + id;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGenerarCita = function (idconsolidacion, fechacita, horainicial, horafinal, CodSap, usuarioproveedor) {
        Ruta = serviceBase + "Api/SolicitudCita/generarCita/?idconsolidacion=" + idconsolidacion + '&fechacita=' + fechacita + '&horainicial=' + horainicial + '&horafinal=' + horafinal + '&CodSap=' + CodSap + '&usuarioproveedor=' + usuarioproveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabarCancelar = function (id, codigo, usuarioCreacion, idcita) {
        Ruta = serviceBase + "Api/AprobacionCitas/getGrabarCancelar/?id=" + id + '&codigo=' + codigo + '&usuarioCreacion=' + usuarioCreacion + '&idcita=' + idcita;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarDataAprobarCita = function (Tablacabecera, Tabladetalle) {
        var AprobarCita = {
            'p_cabeceraAprobarCita': Tablacabecera,
            'p_detalleAprobarCita': Tabladetalle
        }
        Ruta = serviceBase + 'Api/ReporteAprobarCita/ExportarAprobarCita';
        return $http.post(Ruta, AprobarCita).then(function (response) {
            return response;
        });

    };

    AprobacionCitaServiceFactory.getCatalogo = _getCatalogo;
    AprobacionCitaServiceFactory.getConsulaGrid = _getConsulaGrid;
    AprobacionCitaServiceFactory.getGenerarCita = _getGenerarCita;
    AprobacionCitaServiceFactory.getConsulaGridDetallePedido = _getConsulaGridDetallePedido;
    AprobacionCitaServiceFactory.getGrabarCancelar = _getGrabarCancelar;
    AprobacionCitaServiceFactory.getExportarDataAprobarCita = _getExportarDataAprobarCita;



    return AprobacionCitaServiceFactory;

}]);

//Servicio Aprobacion Citas Proveedor
'use strict';
app.factory('AprobacionCitasProveedorService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {


    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var AprobacionCitaProveedorServiceFactory = {};
    var Ruta = '';


    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    var _getConsulaGrid = function (tipo, fechadesde, fechahasta, numero, estado, CodSap, ruc) {

        Ruta = serviceBase + "Api/AprobacionCitasProveedor/conBuscarAprobacionCitasProveedor/?tipo=" + tipo + '&fechadesde=' + fechadesde + '&fechahasta=' + fechahasta + '&numero=' + numero + '&estado=' + estado + '&codProveedor=' + CodSap + '&ruc=' + ruc;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getConsulaGridDetallePedido = function (tipo, id) {

        Ruta = serviceBase + "Api/AprobacionCitasProveedor/conBuscarDetalleGridProveedor/?tipo=" + tipo + '&id=' + id;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGenerarCita = function (idconsolidacion, fechacita, horainicial, horafinal, CodSap, usuarioproveedor) {
        Ruta = serviceBase + "Api/SolicitudCita/generarCita/?idconsolidacion=" + idconsolidacion + '&fechacita=' + fechacita + '&horainicial=' + horainicial + '&horafinal=' + horafinal + '&CodSap=' + CodSap + '&usuarioproveedor=' + usuarioproveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabarCancelar = function (id, codigo, usuarioCreacion, idcita) {
        Ruta = serviceBase + "Api/AprobacionCitasProveedor/getGrabarCancelarProveedor/?id=" + id + '&codigo=' + codigo + '&usuarioCreacion=' + usuarioCreacion + '&idcita=' + idcita;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarDataAprobarCita = function (Tablacabecera, Tabladetalle) {
        var AprobarCita = {
            'p_cabeceraAprobarCita': Tablacabecera,
            'p_detalleAprobarCita': Tabladetalle
        }
        Ruta = serviceBase + 'Api/ReporteAprobarCita/ExportarAprobarCita';
        return $http.post(Ruta, AprobarCita).then(function (response) {
            return response;
        });

    };

    AprobacionCitaProveedorServiceFactory.getCatalogo = _getCatalogo;
    AprobacionCitaProveedorServiceFactory.getConsulaGrid = _getConsulaGrid;
    AprobacionCitaProveedorServiceFactory.getGenerarCita = _getGenerarCita;
    AprobacionCitaProveedorServiceFactory.getConsulaGridDetallePedido = _getConsulaGridDetallePedido;
    AprobacionCitaProveedorServiceFactory.getGrabarCancelar = _getGrabarCancelar;
    AprobacionCitaProveedorServiceFactory.getExportarDataAprobarCita = _getExportarDataAprobarCita;



    return AprobacionCitaProveedorServiceFactory;

}]);

//Servicio Solcitud de Cita Rapida
'use strict';
app.factory('SolicitudCitaRapidaService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var SolicitudCitaRapidaServiceFactory = {};
    var Ruta = '';

    var _getCatalogo = function (pTabla) {

        Ruta = serviceBase + "Api/Catalogos/?NombreCatalogo=" + pTabla;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getChoferVehiculos = function (tipo, codproveedor) {
        Ruta = serviceBase + "Api/ConsolidacionPedidos/ChoferVehiculo/?tipoid=" + tipo + '&codproveedor=' + codproveedor;

        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getBuscarProveedorDatos = function (tipo, ruc) {

        Ruta = serviceBase + "Api/CitaRapida/BuscarProveedorDatos/?tipo=" + tipo + '&ruc=' + ruc;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getBuscarPedidosCitas = function (tipo, ruc) {

        Ruta = serviceBase + "Api/CitaRapida/BuscarPedidosCitas/?tipoCita=" + tipo + '&rucCita=' + ruc;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };


    var _getGrabaConsolidacion = function (Cabconsolidacion, Pedidos, DetPedidos) {
        var Consolidacion = {
            'p_consolidacion': Cabconsolidacion,
            'p_Pedidos': Pedidos,
            'p_DetPedidos': DetPedidos
        }
        Ruta = serviceBase + 'Api/CitaRapida/grabarConsolidacion';
        return $http.post(Ruta, Consolidacion).then(function (response) {
            return response;
        });
    };


    var _getBuscarDatosVarios = function (tipod, dato, ruc) {

        Ruta = serviceBase + "Api/CitaRapida/BuscarDatosVarios/?tipod=" + tipod + '&dato=' + dato + '&ruc=' + ruc;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getGrabaCitaRapida = function (CabCitaRapida, DetCitaRapida) {
        var CitaRapida = {
            'p_cabecera': CabCitaRapida,
            'p_detalle': DetCitaRapida
        }

        Ruta = serviceBase + 'Api/CitaRapida/grabarCitaRapida';
        return $http.post(Ruta, CitaRapida).then(function (response) {
            return response;
        });
    };

    var _getdatos = function (id, citas) {
        Ruta = serviceBase + "Api/SolicitudCita/getdatos/?id=" + id + '&citas=' + citas;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };




    SolicitudCitaRapidaServiceFactory.getChoferVehiculos = _getChoferVehiculos;
    SolicitudCitaRapidaServiceFactory.getGrabaConsolidacion = _getGrabaConsolidacion;
    SolicitudCitaRapidaServiceFactory.getCatalogo = _getCatalogo;
    SolicitudCitaRapidaServiceFactory.getBuscarProveedorDatos = _getBuscarProveedorDatos;
    SolicitudCitaRapidaServiceFactory.getBuscarPedidosCitas = _getBuscarPedidosCitas;
    SolicitudCitaRapidaServiceFactory.getBuscarDatosVarios = _getBuscarDatosVarios;
    SolicitudCitaRapidaServiceFactory.getGrabaCitaRapida = _getGrabaCitaRapida;
    SolicitudCitaRapidaServiceFactory.getdatos = _getdatos;




    return SolicitudCitaRapidaServiceFactory;

}]);

//Servicio Reporte Tabular Citas
'use strict';
app.factory('ReporteTabularCitasService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {


    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ReporteTabularCitasServiceFactory = {};
    var Ruta = '';



    var _getConsulaGrid = function (tipo, fechadesde, fechahasta) {

        Ruta = serviceBase + "Api/ReportesTransporte/getTabularCitas/?tipo=" + tipo + '&fechadesdeRPT=' + fechadesde + '&fechahastaRPT=' + fechahasta;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarData = function (Tablacabecera, Tabladetalle) {
        var Tabla = {
            'p_cabecera': Tablacabecera,
            'p_detalle': Tabladetalle
        }
        Ruta = serviceBase + 'Api/ReportesTransporte/ExportarTabular';
        return $http.post(Ruta, Tabla).then(function (response) {
            return response;
        });

    };

    ReporteTabularCitasServiceFactory.getConsulaGrid = _getConsulaGrid;
    ReporteTabularCitasServiceFactory.getExportarData = _getExportarData;



    return ReporteTabularCitasServiceFactory;

}]);

//Servicio Reporte Mercaderia a Recibir
'use strict';
app.factory('ReporteMercaderiaRecibirService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {


    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ReporteMercaderiaRecibirServiceFactory = {};
    var Ruta = '';



    var _getConsulaGrid = function (tipo, fechadesde, fechahasta, numcita,codproveedor) {

        Ruta = serviceBase + "Api/ReportesTransporte/getTabularCitas/?tipo=" + tipo + '&fechadesdeRPT=' + fechadesde + '&fechahastaRPT=' + fechahasta + '&numcita=' + numcita + '&codproveedor=' + codproveedor;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };

    var _getExportarData = function (tiporeporte, usuarioCreacion, tipo, fechadesde, fechahasta, numcita, codproveedor) {

        Ruta = serviceBase + 'Api/ReportesTransporte/ExportarTabular';
        //return $http.post(Ruta, Tabla).then(function (response) {
        //    return response;
        //});

        return $http.get(Ruta, { params: { tiporeporte: tiporeporte, usuarioCreacion: usuarioCreacion, tipo: tipo, fechadesdeRPT: fechadesde, fechahastaRPT: fechahasta, numcita: numcita, codproveedor: codproveedor }, responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });

    };

    ReporteMercaderiaRecibirServiceFactory.getConsulaGrid = _getConsulaGrid;
    ReporteMercaderiaRecibirServiceFactory.getExportarData = _getExportarData;



    return ReporteMercaderiaRecibirServiceFactory;

}]);


'use strict';
app.factory('ReporteActaRecepcionService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {


    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ReporteActaRecepcionServiceFactory = {};
    var Ruta = '';




    var _getConsulaGridActaRecepcion = function (tipo, Norden, Nfactura, Fecha1, Fecha2, codsapat, codigoAlmacen) {

        Ruta = serviceBase + "Api/ReporteAdministrador/getConsulaGridActaRecepcion/?tipo=" + tipo + '&Norden=' + Norden + '&Nfactura=' + Nfactura + '&Fecha1=' + Fecha1 + '&Fecha2=' + Fecha2 + '&codsapat=' + codsapat + '&codigoAlmacen=' + codigoAlmacen;
        return $http.get(Ruta).then(function (results) {
            return results;
        });
    };
    
    var _getArchivoActaRecepcion = function (idarchivo, anio, codSap, codAlmacen) {

        Ruta = serviceBase + 'Api/ReporteAdministrador/ExportarArchivoActaRecepcion';

        return $http.get(Ruta, { params: { idarchivo: idarchivo, anio: anio, codSap: codSap, codAlmacen: codAlmacen }, responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });


    };

    var _getExportarActaRecepcion = function (tipo, usuariologon, CodSap, ruc) {

        Ruta = serviceBase + 'Api/ReporteAdministrador/ExportarNoCompra';

        return $http.get(Ruta, { params: { tipo: tipo, usuariologon: usuariologon, CodSap: CodSap, ruc: ruc }, responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });


    };

    ReporteActaRecepcionServiceFactory.getConsulaGridActaRecepcion = _getConsulaGridActaRecepcion;

    ReporteActaRecepcionServiceFactory.getExportarActaRecepcion = _getExportarActaRecepcion;
    ReporteActaRecepcionServiceFactory.getArchivoActaRecepcion = _getArchivoActaRecepcion;
   
    return ReporteActaRecepcionServiceFactory;

}]);
