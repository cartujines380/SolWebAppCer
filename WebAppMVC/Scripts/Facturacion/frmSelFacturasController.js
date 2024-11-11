
//'use strict';
app.controller('frmSelFacturasController', ['$scope', 'FacturasService', '$filter', 'authService', 'localStorageService', function ($scope, FacturasService, $filter, authService, localStorageService) {
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

    $scope.resDgConsFact = [];
    var _resDgConsFact = [];
    $scope.pagesConsFact = [];
    $scope.pgcDgConsFact = [];

    $scope.rbtFiltroCons = 'EF';
    $scope.rbtFechas = 'R';
    $scope.txtNumPedido = "";
    $scope.txtFacEst = "";
    $scope.txtFacPto = "";
    $scope.txtFacSec = "";

    var dateString1 = new Date();
    var d1 = dateString1.format("dd/mm/yyyy");
    
    $scope.txtFechaIni = d1
    $scope.txtFechaFin = d1;



    $scope.chkEstadosList = [];
    $scope.chkEstadosSelModel = [];
    $scope.chkEstadosSettings = { displayProp: 'detalle', idProp: 'codigo', enableSearch: false, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.etiTotRegistros = "0";

    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;

    $scope.cboAlmacenList1 = [];
    $scope.cboAlmacenSelItem1 = null;

    $scope.myPromise = FacturasService.getConsAlmacenes().then(function (results) {
        debugger;
        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            listAlmacen.splice(0, 0, { pCodAlmacen: "-999", pNomAlmacen: "Todos los Almacenes" });
            $scope.cboAlmacenList1 = listAlmacen;
            $scope.cboAlmacenSelItem1 = listAlmacen[0];

        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: getConsAlmacenes().");
    });

    $scope.myPromise = FacturasService.getCatalogo('tbl_EstadosDocumentos').then(function (results) {
        $scope.chkEstadosList = results.data;
        //LANZO CONSULTA AUTOMATICA AL CARGAR
        var list = $scope.chkEstadosList;
        for (var i = 0; i < list.length; i++) {
            if (list[i].codigo != "RE") {
                $scope.chkEstadosSelModel.push({ id: list[i].codigo });
            }
        }
        $scope.rbtFiltroCons = 'EF';
        $scope.rbtFechas = 'E';

        var dateString1 = new Date();
        var dateString = new Date();
        var d1 = dateString1.format("dd/mm/yyyy");

        dateString.setDate(dateString.getDate() - 15);
        var d2 = dateString.format("dd/mm/yyyy");

        $scope.txtFechaIni = d2;
        $scope.txtFechaFin = d1;
        
        $scope.btnConsultaClick1();
    }, function (error) {
    });

    $scope.btnConsultaClick1 = function () {
        var numPedido = "";
        if ($scope.rbtFiltroCons == 'P') {
            numPedido = $scope.txtNumPedido;
            if (numPedido == '') {
                $scope.showMessage('I', 'Ingrese el Número del Pedido.');
                return;
            }
        }
        var facEst = ""; var facPto = ""; var facSec = "";
        if ($scope.rbtFiltroCons == 'F') {
            facEst = $scope.txtFacEst;
            facPto = $scope.txtFacPto;
            facSec = $scope.txtFacSec;
            if (facEst == '') {
                $scope.showMessage('I', 'Ingrese el Establecimiento de la Factura.');
                return;
            }
            if (facPto == '') {
                $scope.showMessage('I', 'Ingrese el Punto de Emisión de la Factura.');
                return;
            }
            if (facSec == '') {
                $scope.showMessage('I', 'Ingrese el Secuencial de la Factura.');
                return;
            }
        }
        var estados = "";
        var fechaEsReg = "";
        var fechaIni = "";
        var fechaFin = "";
        var codAlmacen = "";
        if ($scope.rbtFiltroCons == 'EF') {
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
            var li = $scope.chkEstadosSelModel;
            //if (li.length < 1) {
            //    $scope.showMessage('I', 'Seleccione al menos un estado.');
            //    return;
            //}
            for (idx = 0; idx < li.length; idx++) {
                estados = estados + li[idx].id + ';';
            }
            fechaEsReg = (($scope.rbtFechas == 'R') ? 'S' : 'N');

            if ($scope.cboAlmacenSelItem1 != null) {
                codAlmacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
            }
        }
        
        
        $scope.myPromise = FacturasService.getConsRecupFacturasFiltro($scope.pCodSAP, $scope.pRuc, $scope.pUsuario,
            numPedido, facEst, facPto, facSec, fechaEsReg, fechaIni, fechaFin, estados, codAlmacen).then(function (results) {
                if (results.data.success) {
                    $scope.resDgConsFact = results.data.root[0][0];
                    if ($scope.resDgConsFact.length < 1) {
                        //$scope.showMessage('I', 'No existen datos para mostrar.');
                        $scope.etiTotRegistros = "";
                    }
                    else {
                        $scope.etiTotRegistros = $scope.resDgConsFact.length.toString();
                    }
                }
                else {
                    $scope.resDgConsFact = [];
                    $scope.etiTotRegistros = "";
                    $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
                }

                setTimeout(function () { $('#btnConsulta').focus(); }, 100);
                setTimeout(function () { $('#rbtEstadoFechas').focus(); }, 150);

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
        debugger;
        var numPedido = "";
        if ($scope.rbtFiltroCons == 'P') {
            numPedido = $scope.txtNumPedido;
            if (numPedido == '') {
                $scope.showMessage('I', 'Ingrese el Número del Pedido.');
                return;
            }
        }
        var facEst = ""; var facPto = ""; var facSec = "";
        if ($scope.rbtFiltroCons == 'F') {
            facEst = $scope.txtFacEst;
            facPto = $scope.txtFacPto;
            facSec = $scope.txtFacSec;
            if (facEst == '') {
                $scope.showMessage('I', 'Ingrese el Establecimiento de la Factura.');
                return;
            }
            if (facPto == '') {
                $scope.showMessage('I', 'Ingrese el Punto de Emisión de la Factura.');
                return;
            }
            if (facSec == '') {
                $scope.showMessage('I', 'Ingrese el Secuencial de la Factura.');
                return;
            }
        }
        var estados = "";
        var fechaEsReg = "";
        var fechaIni = "";
        var fechaFin = "";
        var codAlmacen = "";
        if ($scope.rbtFiltroCons == 'EF') {
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
            var li = $scope.chkEstadosSelModel;
            if (li.length < 1) {
                $scope.showMessage('I', 'Seleccione al menos un estado.');
                return;
            }
            for (idx = 0; idx < li.length; idx++) {
                estados = estados + li[idx].id + ';';
            }
            fechaEsReg = (($scope.rbtFechas == 'R') ? 'S' : 'N');
            if ($scope.cboAlmacenSelItem1 != null) {
                codAlmacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
            }
        }
        
        
        $scope.myPromise = FacturasService.getConsRecupFacturasFiltro($scope.pCodSAP, $scope.pRuc, $scope.pUsuario,
            numPedido, facEst, facPto, facSec, fechaEsReg, fechaIni, fechaFin, estados, codAlmacen).then(function (results) {
                if (results.data.success) {
                    $scope.resDgConsFact = results.data.root[0][0];
                    if ($scope.resDgConsFact.length < 1) {
                        $scope.showMessage('I', 'No existen datos para mostrar.');
                        $scope.etiTotRegistros = "";
                    }
                    else {
                        $scope.etiTotRegistros = $scope.resDgConsFact.length.toString();
                    }
                }
                else {
                    $scope.resDgConsFact = [];
                    $scope.etiTotRegistros = "";
                    $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
                }

                setTimeout(function () { $('#btnConsulta').focus(); }, 100);
                setTimeout(function () { $('#rbtEstadoFechas').focus(); }, 150);

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
        // ini - preparado de envio de parametros
        if (localStorageService.get('facControlFact')) {
            localStorageService.remove('facControlFact');
        }
        var someSessionObj = {
            'tipoID': 'F',
            'numID': rowSel.idDocumento,
            'codigoIva12':rowSel.data.split(",")[0],
            'porcentajeIva12':rowSel.data.split(",")[1],
            'codigoIva14':rowSel.data.split(",")[2],
            'porcentajeIva14':rowSel.data.split(",")[3]

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
    setTimeout(function () { $('#btnConsulta1').focus(); }, 150);
    setTimeout(function () { angular.element('#btnConsulta1').trigger('click'); }, 250);
}]);

app.directive('onlyDigits', function () {

    return {
        restrict: 'A',
        require: '?ngModel',
        link: function (scope, element, attrs, ngModel) {
            if (!ngModel) return;
            ngModel.$parsers.unshift(function (inputValue) {
                var digits = inputValue.split('').filter(function (s) { return (!isNaN(s) && s != ' '); }).join('');
                ngModel.$viewValue = digits;
                ngModel.$render();
                return digits;
            });
        }
    };
});
