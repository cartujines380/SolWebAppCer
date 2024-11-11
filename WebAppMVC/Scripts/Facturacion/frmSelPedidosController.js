
//'use strict';
app.controller('frmSelPedidosController', ['$scope', 'FacturasService', '$filter', 'authService', 'localStorageService', function ($scope, FacturasService, $filter, authService, localStorageService) {
    var dateFormat = function () {
        var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
            timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
            timezoneClip = /[^-+\dA-Z]/g,
            pad = function (val, len) {
                val = String(val);
                len = len || 2;
                while (val.length < len) val = "0" + val;
                return val;
            };

        return function (date, mask, utc) {
            var dF = dateFormat;


            if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }


            date = date ? new Date(date) : new Date;
            if (isNaN(date)) throw SyntaxError("invalid date");

            mask = String(dF.masks[mask] || mask || dF.masks["default"]);


            if (mask.slice(0, 4) == "UTC:") {
                mask = mask.slice(4);
                utc = true;
            }

            var _ = utc ? "getUTC" : "get",
                d = date[_ + "Date"](),
                D = date[_ + "Day"](),
                m = date[_ + "Month"](),
                y = date[_ + "FullYear"](),
                H = date[_ + "Hours"](),
                M = date[_ + "Minutes"](),
                s = date[_ + "Seconds"](),
                L = date[_ + "Milliseconds"](),
                o = utc ? 0 : date.getTimezoneOffset(),
                flags = {
                    d: d,
                    dd: pad(d),
                    ddd: dF.i18n.dayNames[D],
                    dddd: dF.i18n.dayNames[D + 7],
                    m: m + 1,
                    mm: pad(m + 1),
                    mmm: dF.i18n.monthNames[m],
                    mmmm: dF.i18n.monthNames[m + 12],
                    yy: String(y).slice(2),
                    yyyy: y,
                    h: H % 12 || 12,
                    hh: pad(H % 12 || 12),
                    H: H,
                    HH: pad(H),
                    M: M,
                    MM: pad(M),
                    s: s,
                    ss: pad(s),
                    l: pad(L, 3),
                    L: pad(L > 99 ? Math.round(L / 10) : L),
                    t: H < 12 ? "a" : "p",
                    tt: H < 12 ? "am" : "pm",
                    T: H < 12 ? "A" : "P",
                    TT: H < 12 ? "AM" : "PM",
                    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
                };

            return mask.replace(token, function ($0) {
                return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
            });
        };
    }();


    dateFormat.masks = {
        "default": "ddd mmm dd yyyy HH:MM:ss",
        shortDate: "m/d/yy",
        mediumDate: "mmm d, yyyy",
        longDate: "mmmm d, yyyy",
        fullDate: "dddd, mmmm d, yyyy",
        shortTime: "h:MM TT",
        mediumTime: "h:MM:ss TT",
        longTime: "h:MM:ss TT Z",
        isoDate: "yyyy-mm-dd",
        isoTime: "HH:MM:ss",
        isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
        isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
    };


    dateFormat.i18n = {
        dayNames: [
            "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        ],
        monthNames: [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
        ]
    };


    Date.prototype.format = function (mask, utc) {
        return dateFormat(this, mask, utc);
    };
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.resDgConsPed = [];
    var _resDgConsPed = [];
    $scope.pagesConsPed = [];
    $scope.pgcDgConsPed = [];

    $scope.rbtPedidos = 'T';
    $scope.rbtFechas = 'T';
    $scope.rbtEstados = 'T';
    $scope.rbtAlmacen = 'T';
    $scope.txtNumPedido = "";
    var dateString1 = new Date();
    var dateString = new Date();
    var d1 = dateString1.format("dd/mm/yyyy");
    

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");

    $scope.txtFechaIni = d2
    $scope.txtFechaFin = d1;
   // $scope.txtFechaIni.setDate($scope.txtFechaIni.getDate() - 15);
    //$scope.cboEstados = [];
    //$scope.cboEstadosSelItem = "";
    $scope.chkEstadosList = [];
    $scope.chkEstadosSelModel = [];
    $scope.chkEstadosSettings = { displayProp: 'detalle', idProp: 'codigo', enableSearch: false, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.etiTotRegistros = "0";

    var  txtFechaInil = new Date();
    var  txtFechaFinl = new Date();
    txtFechaInil.setDate(txtFechaInil.getDate() - 15);

  
    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;

    $scope.cboAlmacenList1 = [];
    $scope.cboAlmacenSelItem1 = null;

    $scope.myPromise = FacturasService.getConsAlmacenes().then(function (results) {
        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            //listAlmacen.splice(0, 0, { pCodAlmacen: "-999", pNomAlmacen: "Todos los Almacenes" });
            $scope.cboAlmacenList1 = listAlmacen;
            //$scope.cboAlmacenSelItem1 = listAlmacen[0];
         
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: getConsAlmacenes().");
    });

    $scope.myPromise = FacturasService.getCatalogo('tbl_EstadosPedidos').then(function (results) {
        //$scope.cboEstados = results.data;
        //$scope.cboEstadosSelItem = results.data[0];
        $scope.chkEstadosList = results.data;
        //LANZO CONSULTA AUTOMATICA AL CARGAR
        var list = $scope.chkEstadosList;
        for (var i = 0; i < list.length; i++) {
            if (list[i].codigo != "VE" && list[i].codigo != "FI" && list[i].codigo != "FT") {
                $scope.chkEstadosSelModel.push({ id: list[i].codigo });
            }
        }
        $scope.rbtEstados = 'F';
       

    

        $scope.btnConsultaClick();
        $scope.rbtFechas = 'F';
       
    }, function (error) {
    });

    $scope.btnConsultaClick1 = function () {
       
        var numPedido = "";
        if ($scope.rbtPedidos == 'F') {
            numPedido = $scope.txtNumPedido;
            if (numPedido == '') {
                $scope.showMessage('I', 'Ingrese el Número del Pedido.');
                return;
            }
        }
        var fechaIni = "";
        var fechaFin = "";
        
        if ($scope.rbtFechas == 'F') {
            if ($scope.txtFechaIni != '' && $scope.txtFechaIni != null) {
                fechaIni = $filter('date')($scope.txtFechaIni, 'dd-MM-yyyy');
            }
            if ($scope.txtFechaFin != '' && $scope.txtFechaFin != null) {
                fechaFin = $filter('date')($scope.txtFechaFin, 'dd-MM-yyyy');
            }
            if (fechaIni == '' && fechaFin == '') {
                $scope.showMessage('I', 'Ingrese la fecha desde o la fecha hasta o ambas.');
                return;
            }
        }
        var estados = "";
        if ($scope.rbtEstados == 'F') {
            var li = $scope.chkEstadosSelModel;
            if (li.length < 1) {
                $scope.showMessage('I', 'Seleccione al menos un estado.');
                return;
            }
            for (idx = 0; idx < li.length; idx++) {
                estados = estados + li[idx].id + ';';
            }
        }
        
        

        $scope.myPromise = FacturasService.getConsSelPedidosFiltro($scope.pCodSAP, $scope.pRuc, $scope.pUsuario,
            numPedido, fechaIni, fechaFin, estados).then(function (results) {
                if (results.data.success) {
                    $scope.resDgConsPed = results.data.root[0][0];
                  
                    if ($scope.resDgConsPed.length < 1) {
                       // $scope.showMessage('I', 'No existen datos para mostrar.');
                        $scope.etiTotRegistros = "";
                    }
                    else {
                        $scope.etiTotRegistros = $scope.resDgConsPed.length.toString();
                    }
                    
                }
                else {
                    $scope.resDgConsPed = [];
                    $scope.etiTotRegistros = "";
                    $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
                }

                setTimeout(function () { $('#btnConsulta').focus(); }, 100);
                setTimeout(function () { $('#rbtPedidosAll').focus(); }, 150);

            }, function (error) {
                $scope.etiTotRegistros = "";
                var errors = [];
                for (var key in error.data.modelState) {
                    for (var i = 0; i < error.data.modelState[key].length; i++) {
                        errors.push(error.data.modelState[key][i]);
                    }
                }
                $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
            });
    };

    $scope.btnConsultaClick = function () {
        
        var numPedido = "";
        if ($scope.rbtPedidos == 'F') {
            numPedido = $scope.txtNumPedido;
            if (numPedido == '') {
                $scope.showMessage('I', 'Ingrese el Número del Pedido.');
                return;
            }
        }
        var fechaIni = "";
        var fechaFin = "";
        if ($scope.rbtFechas == 'F') {
            if ($scope.txtFechaIni != '' && $scope.txtFechaIni != null) {
                fechaIni = $filter('date')($scope.txtFechaIni, 'dd-MM-yyyy');
            }
            if ($scope.txtFechaFin != '' && $scope.txtFechaFin != null) {
                fechaFin = $filter('date')($scope.txtFechaFin, 'dd-MM-yyyy');
            }
            if (fechaIni == '' && fechaFin == '') {
                $scope.showMessage('I', 'Ingrese la fecha desde o la fecha hasta o ambas.');
                return;
            }
        }
        var estados = "";
        if ($scope.rbtEstados == 'F') {
            var li = $scope.chkEstadosSelModel;
            if (li.length < 1) {
                $scope.showMessage('I', 'Seleccione al menos un estado.');
                return;
            }
            for (idx = 0; idx < li.length; idx++) {
                estados = estados + li[idx].id + ';';
            }
        }
        var codAlmacen = "";
        if ($scope.rbtAlmacen == 'F') {

            if ($scope.cboAlmacenSelItem1 == null)
            {
                $scope.showMessage('I', 'Seleccione un almacén.');
                 return;
            }
            codAlmacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
        }
       
        $scope.myPromise = FacturasService.getConsSelPedidosFiltro($scope.pCodSAP, $scope.pRuc, $scope.pUsuario,
            numPedido, fechaIni, fechaFin, estados, codAlmacen).then(function (results) {
            if (results.data.success) {
                $scope.resDgConsPed = results.data.root[0][0];
                if ($scope.resDgConsPed.length < 1) {
                    $scope.showMessage('I', 'No existen datos para mostrar.');
                    $scope.etiTotRegistros = "";
                }
                else {
                    $scope.etiTotRegistros = $scope.resDgConsPed.length.toString();
                }
            }
            else {
                $scope.resDgConsPed = [];
                $scope.etiTotRegistros = "";
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            $scope.showPaginate = true;
            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#rbtPedidosAll').focus(); }, 150);

        }, function (error) {
            $scope.etiTotRegistros = "";
            var errors = [];
            for (var key in error.data.modelState) {
                for (var i = 0; i < error.data.modelState[key].length; i++) {
                    errors.push(error.data.modelState[key][i]);
                }
            }
            $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
        });
    };

    $scope.selRowGrid = function (rowSel) {
        if (rowSel.estado == 'VE') {
            $scope.showMessage('I', 'El pedido seleccionado tiene estado vencido (fecha de emisión mayor al límite para ingreso de facturas).');
            return;
        }
        if (rowSel.estado == 'FI') {
            $scope.showMessage('I', 'El pedido seleccionado tiene estado finalizado (fecha de emisión mayor al límite para continuar ingresando facturas).');
            return;
        }
        if (rowSel.siValorPend == 'N') {
            $scope.showMessage('I', 'El pedido seleccionado ya no tiene valor pendiente para facturar.');
            return;
        }
        // ini - preparado de envio de parametros
        if (localStorageService.get('facControlFact')) {
            localStorageService.remove('facControlFact');
        }
        var someSessionObj = {
            'tipoID': 'P',
            'numID': rowSel.idPedido,
            'codigoIva12': rowSel.data.split(",")[0],
            'porcentajeIva12': rowSel.data.split(",")[1],
            'codigoIva14': rowSel.data.split(",")[2],
            'porcentajeIva14': rowSel.data.split(",")[3]
        };
        localStorageService.set('facControlFact', someSessionObj);
        // fin - preparado de envio de parametros
        //event.preventDefault();
        //event.stopPropagation();
        window.location = "/Facturacion/frmControlFacturas";
    };


    $scope.showMessage = function (tipo, mensaje) {
        //E=Error, I=Informativo, M/S/G=MensajeOK(grabar,procesar,satisfactorio,etc.)
        $scope.MenjError = mensaje;
        if (tipo == 'I') {
            $('#idMensajeInformativo').modal('show');
        }
        else if (tipo == 'E') {
            $('#idMensajeError').modal('show');
        }
        else if (tipo == 'M' || tipo == 'S' || tipo == 'G') {
            $('#idMensajeOk').modal('show');
        }
    }
//    $scope.btnConsultaClick();
    //setTimeout(function () { $('#btnConsulta').focus(); }, 150);
    //setTimeout(function () { angular.element('#btnConsulta').trigger('click'); }, 250);
}]);
