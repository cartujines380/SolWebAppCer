
//'use strict';
app.controller('frmConsPedidosCrossController', ['$scope', 'PedidosService','EtiquetasService', '$filter', 'authService', function ($scope, PedidosService,EtiquetasService, $filter, authService) {
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
    $scope.searchText = "";

    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;


    $scope.resDgPedidos = [];
    var _resDgPedidos = [];
    $scope.pagesPedidos = [];
    $scope.pgcDgPedidos = [];
    $scope.totalGrid = {};

    $scope.resDgPedidostodoslosregistro = [];

    $scope.pgcDgPedidosDET = [];

    $scope.pOpcGrp1 = "R";
    $scope.pOpcGrp2 = "T";
    
    var idTipoPedido = "";

    var dateString1 = new Date();
    var dateString = new Date();

    var d1 = dateString1.format("dd/mm/yyyy");

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");


    //$scope.pFechaIni = "";
    //$scope.pFechaFin = "";


    $scope.pFechaIni = d2;
    $scope.pFechaFin = d1;



    //$scope.pFecha = d1;

    $scope.pFecha = "";

    $scope.pNumOrden = "";
    $scope.bandVerRes = true;
    $scope.bandVerPdf = true;
    $scope.bandGenTxt = true;
    $scope.bandGenXml = true;

    $scope.hideGrid = true;
    $scope.hideBtnT = true;
    $scope.hideBtnX = true;
    $scope.hideBtnX = true;
    $scope.hideBtnPdf = true;

    $scope.pRutaDownloadTxt = "";
    $scope.pRutaDownloadXml = "";
    $scope.pRutaDownloadHtml = "";
    $scope.pRutaDownloadPdf = "";
    $scope.npage = 1;
    //$scope.pRuc = "";
    //$scope.pUsuario = "";
    //$scope.pCodSAP = "";
    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;

    $scope.pNomProveedorLog = authService.authentication.nomEmpresa;

    $scope.numPedidoCross = 0;


    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];
    $scope.cboPaginSelItem = $scope.cboPaginList[0];

    $scope.cboCiudadList1 = [];
    $scope.cboCiudadSelItem1 = null;

    $scope.cboCiudadList2 = [];
    $scope.cboCiudadSelItem2 = null;

    $scope.cboAlmacenList1 = [];
    $scope.cboAlmacenList1T = [];
    $scope.cboAlmacenSelItem1 = null;

    $scope.cboAlmacenList2 = [];
    $scope.cboAlmacenList2T = [];
    $scope.cboAlmacenSelItem2 = null;

    $scope.selectpedidoB = "";
    $scope.listaPedidoB = [];

    $scope.myPromise = EtiquetasService.catalogoCatalogo("1", "1010").then(function (results) {
        if (results.data.cCodError == "0") {
            var datos = results.data.root[0];
            $scope.listaPedidoB = datos;
            $scope.selectpedidoB = datos[0];
        }

    }, function (error) {


    });


    $scope.$watch('cboCiudadSelItem1', function () {
        if ($scope.cboCiudadSelItem1 == null) return;


        if ($scope.cboCiudadSelItem1.pCodCiudad == "-999") {
            $scope.cboAlmacenList1 = $scope.cboAlmacenList1T;

        }
        else {
            $scope.cboAlmacenList1 = [];

            $scope.cboAlmacenList1 = $filter('filter')($scope.cboAlmacenList1T, { pCodCiudad: $scope.cboCiudadSelItem1.pCodCiudad }, true);
            $scope.cboAlmacenList1.splice(0, 0, { pCodAlmacen: "-999", pNomAlmacen: "Todos los Almacenes" });
        }
        $scope.cboAlmacenSelItem1 = $scope.cboAlmacenList1[0];

    });

    $scope.$watch('cboCiudadSelItem2', function () {
        if ($scope.cboCiudadSelItem2 == null) return;


        if ($scope.cboCiudadSelItem2.pCodCiudad == "-999") {
            $scope.cboAlmacenList2 = $scope.cboAlmacenList1T;

        }
        else {
            $scope.cboAlmacenList2 = [];

            $scope.cboAlmacenList2 = $filter('filter')($scope.cboAlmacenList2T, { pCodCiudad: $scope.cboCiudadSelItem2.pCodCiudad }, true);
            $scope.cboAlmacenList2.splice(0, 0, { pCodAlmacen: "-999", pNomAlmacen: "Todos los Almacenes" });
        }
        $scope.cboAlmacenSelItem2 = $scope.cboAlmacenList2[0];

    });


    

    $scope.croosconsolidado = function () {
        $scope.npage = 1;
        $scope.isCross = true;

        

        $scope.showMessage('I', 'El reporte presentará los pedidos mostrados en pantalla con el estado de la distribución finalizado y Aprobado.');
        

        var resumen = $filter('filter')($scope.resDgPedidos, { estadoDes: "Finalizado" }, true);
        var resumen1 = $filter('filter')($scope.resDgPedidos, { estadoDes: "Aprobado" }, true);
        if (resumen.length == 0 && resumen1.length == 0) {
            return;
        }
        $scope.resDgPedidos

        var Fecha1 = ""; var Fecha2 = ""; var Ciudad = ""; var bandGenHtml = true; var Almacen = "";
        if ($scope.pOpcGrp1 == "F") {
            if ($scope.pFecha == null || $scope.pFecha == "") {
                $scope.showMessage('I', 'Seleccione la fecha para consultar.');
                return;
            }
            Fecha1 = $filter('date')($scope.pFecha, 'dd-MM-yyyy');
            Fecha2 = Fecha1;
            Ciudad = $scope.cboCiudadSelItem1.pCodCiudad;
            Almacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
        }
        if ($scope.pOpcGrp1 == "R") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }


            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
            if ($scope.cboCiudadSelItem1 != null) {
                Ciudad = $scope.cboCiudadSelItem2.pCodCiudad;
            }
            if ($scope.cboAlmacenSelItem2 != null) {
                Almacen = $scope.cboAlmacenSelItem2.pCodAlmacen;
            }

        }
        if ($scope.pOpcGrp1 == "O") {
            if ($scope.pNumOrden == "") {
                $scope.showMessage('I', 'Ingrese el número de la orden a consultar.');
                return;
            }
        }
        if ($scope.bandVerRes == false && $scope.bandGenTxt == false && $scope.bandGenXml == false && $scope.bandVerPdf == false) {
            $scope.showMessage('I', 'Se tiene que seleccionar al menos una opción para la devolución del resultado.');
            return;
        }
        $scope.pRutaDownloadTxt = "";
        $scope.pRutaDownloadXml = "";
        $scope.pRutaDownloadHtml = "";
        $scope.pRutaDownloadPdf = "";
        $scope.etiTotRegistros = "";

        //if (registro.estado == "0") {
        //    $scope.MenjError = "Debe finalizar la distribución del pedido para generar reporte.";
        //    $('#idMensajeInformativo').modal('show');
        //    return;
        //}
        var Fecha1;
        var Fecha2;
        var Ciudad;
        var Almacen;
        var tipo = 2;
        var exportconsi='F'
        $scope.myPromise = PedidosService.getExportarCrossConsolidado(tipo, $scope.pCodSAP, $scope.pNomProveedorLog, $scope.pUsuario, $scope.pNumOrden, $scope.pRuc, $scope.pOpcGrp1, exportconsi,
        Fecha1, Fecha2, Ciudad, $scope.pNumOrden, true, $scope.bandGenTxt, $scope.bandGenXml, bandGenHtml, $scope.bandVerPdf, Almacen, $scope.isCross, $scope.selectpedidoB.codigo).then(function (results) {
            if (results.data != "") {
                if (tipo == "1") {
                    var file = new Blob([results.data], { type: 'application/xls' });
                    saveAs(file, 'ReporteCrossConsolidado.xls');
                }
                if (tipo == "2") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'ReporteCrossConsolidado.pdf');
                }
            }
        }, function (error) {
        });
    }
    $scope.selRowGrid = function (content) {
        $scope.pgcDgPedidosDET = [];
        idTipoPedido = content.tipoPedido;
        $scope.pgcDgPedidosDET = $filter('filter')($scope.resDgPedidostodoslosregistro, { pIdPedido: content.idPedido });

        $scope.etiTotRegistrosdet = "";
        $scope.etiTotRegistrosdet = $scope.pgcDgPedidosDET.length.toString();
        
        if (content.tipoPedido=="3") {
            $('#MostradetallePedidoFlow').modal('show');
        }
        if (content.tipoPedido == "2") {
            $('#MostradetallePedido').modal('show');
        }
    }

    $scope.myPromise = PedidosService.getConsCiudadesEnAlmacen().then(function (results) {
        if (results.data.success) {
            var listCiud = results.data.root[0];
            listCiud.splice(0, 0, { pCodCiudad: "-999", pNomCiudad: "Todas las Ciudades" });
            $scope.cboCiudadList1 = listCiud;
            $scope.cboCiudadList2 = listCiud;
            $scope.cboCiudadSelItem1 = listCiud[0];
            $scope.cboCiudadSelItem2 = listCiud[0];
        }
        else {
            $scope.showMessage('E', 'Error al consultar ciudades: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: getConsCiudadesEnAlmacen().");
    });

    $scope.myPromise = PedidosService.getConsAlmacenes("4").then(function (results) {
        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            listAlmacen.splice(0, 0, { pCodAlmacen: "-999", pNomAlmacen: "Todos los Almacenes" });
            $scope.cboAlmacenList1T = listAlmacen;
            $scope.cboAlmacenList1 = listAlmacen;
            $scope.cboAlmacenSelItem1 = listAlmacen[0];
            $scope.cboAlmacenList2T = listAlmacen;
            $scope.cboAlmacenList2 = listAlmacen;
            $scope.cboAlmacenSelItem2 = listAlmacen[0];

        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: getConsAlmacenes().");
    });
    function validate_fechaMayorQue(fechaInicial, fechaFinal) {

        valuesStart = fechaInicial.split("/");

        valuesEnd = fechaFinal.split("/");



        // Verificamos que la fecha no sea posterior a la actual

        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);

        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);

        if (dateStart >= dateEnd) {

            return 1;

        }

        return 0;

    }


    function CargaInicial3() {
        if ($scope.pgcDgPedidos.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeGrabar').modal('show');
            return;
        }

        var Fecha1 = ""; var Fecha2 = ""; var Ciudad = ""; var bandGenHtml = true; var Almacen = "";
        if ($scope.pOpcGrp1 == "F") {
            if ($scope.pFecha == null || $scope.pFecha == "") {
                $scope.showMessage('I', 'Seleccione la fecha para consultar.');
                return;
            }
            Fecha1 = $filter('date')($scope.pFecha, 'dd-MM-yyyy');
            Fecha2 = Fecha1;
            Ciudad = $scope.cboCiudadSelItem1.pCodCiudad;
            Almacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
        }
        if ($scope.pOpcGrp1 == "R") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }


            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
            if ($scope.cboCiudadSelItem1 != null) {
                Ciudad = $scope.cboCiudadSelItem2.pCodCiudad;
            }
            if ($scope.cboAlmacenSelItem2 != null) {
                Almacen = $scope.cboAlmacenSelItem2.pCodAlmacen;
            }

        }
        if ($scope.pOpcGrp1 == "O") {
            if ($scope.pNumOrden == "") {
                $scope.showMessage('I', 'Ingrese el número de la orden a consultar.');
                return;
            }
        }
        if ($scope.bandVerRes == false && $scope.bandGenTxt == false && $scope.bandGenXml == false && $scope.bandVerPdf == false) {
            $scope.showMessage('I', 'Se tiene que seleccionar al menos una opción para la devolución del resultado.');
            return;
        }
        var Opc1 = "XD"
        var Opc2 = "N";

        $scope.myPromise = PedidosService.getExportarDataupd($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, Opc1, Opc2,
        Fecha1, Fecha2, Ciudad, $scope.pNumOrden, Almacen, 5).then(function (results) {

        }, function (error) {

        });
    }
    function CargaInicial4() {
        if ($scope.pgcDgPedidos.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeGrabar').modal('show');
            return;
        }

        var Fecha1 = ""; var Fecha2 = ""; var Ciudad = ""; var bandGenHtml = true; var Almacen = "";
        if ($scope.pOpcGrp1 == "F") {
            if ($scope.pFecha == null || $scope.pFecha == "") {
                $scope.showMessage('I', 'Seleccione la fecha para consultar.');
                return;
            }
            Fecha1 = $filter('date')($scope.pFecha, 'dd-MM-yyyy');
            Fecha2 = Fecha1;
            Ciudad = $scope.cboCiudadSelItem1.pCodCiudad;
            Almacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
        }
        if ($scope.pOpcGrp1 == "R") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }


            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
            if ($scope.cboCiudadSelItem1 != null) {
                Ciudad = $scope.cboCiudadSelItem2.pCodCiudad;
            }
            if ($scope.cboAlmacenSelItem2 != null) {
                Almacen = $scope.cboAlmacenSelItem2.pCodAlmacen;
            }

        }
        if ($scope.pOpcGrp1 == "O") {
            if ($scope.pNumOrden == "") {
                $scope.showMessage('I', 'Ingrese el número de la orden a consultar.');
                return;
            }
        }
        if ($scope.bandVerRes == false && $scope.bandGenTxt == false && $scope.bandGenXml == false && $scope.bandVerPdf == false) {
            $scope.showMessage('I', 'Se tiene que seleccionar al menos una opción para la devolución del resultado.');
            return;
        }
        var Opc1 = "XI"
        var Opc2 = "N";
        $scope.myPromise = PedidosService.getExportarDataupd($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, Opc1, Opc2,
        Fecha1, Fecha2, Ciudad, $scope.pNumOrden, Almacen, 5).then(function (results) {

        }, function (error) {

        });
    }


    $scope.reportePackingList = function () {
        $scope.npage = 1;
        $scope.isCross = true;

        $scope.showMessage('I', 'El reporte presentará los pedidos mostrados en pantalla con el estado de la distribución finalizado y Aprobado.');


        var resumen = $filter('filter')($scope.resDgPedidos, { estadoDes: "Finalizado" }, true);
        var resumen1 = $filter('filter')($scope.resDgPedidos, { estadoDes: "Aprobado" }, true);
        if (resumen.length == 0 && resumen1.length == 0) {
            return;
        }


        var Fecha1 = ""; var Fecha2 = ""; var Ciudad = ""; var bandGenHtml = true; var Almacen = "";
        if ($scope.pOpcGrp1 == "F") {
            if ($scope.pFecha == null || $scope.pFecha == "") {
                $scope.showMessage('I', 'Seleccione la fecha para consultar.');
                return;
            }
            Fecha1 = $filter('date')($scope.pFecha, 'dd-MM-yyyy');
            Fecha2 = Fecha1;
            Ciudad = $scope.cboCiudadSelItem1.pCodCiudad;
            Almacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
        }
        if ($scope.pOpcGrp1 == "R") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }


            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
            if ($scope.cboCiudadSelItem1 != null) {
                Ciudad = $scope.cboCiudadSelItem2.pCodCiudad;
            }
            if ($scope.cboAlmacenSelItem2 != null) {
                Almacen = $scope.cboAlmacenSelItem2.pCodAlmacen;
            }

        }
        if ($scope.pOpcGrp1 == "O") {
            if ($scope.pNumOrden == "") {
                $scope.showMessage('I', 'Ingrese el número de la orden a consultar.');
                return;
            }
        }
        if ($scope.bandVerRes == false && $scope.bandGenTxt == false && $scope.bandGenXml == false && $scope.bandVerPdf == false) {
            $scope.showMessage('I', 'Se tiene que seleccionar al menos una opción para la devolución del resultado.');
            return;
        }
        $scope.pRutaDownloadTxt = "";
        $scope.pRutaDownloadXml = "";
        $scope.pRutaDownloadHtml = "";
        $scope.pRutaDownloadPdf = "";
        $scope.etiTotRegistros = "";

        //if (registro.estado == "0") {
        //    $scope.MenjError = "Debe finalizar la distribución del pedido para generar reporte.";
        //    $('#idMensajeInformativo').modal('show');
        //    return;
        //}
        var Fecha1;
        var Fecha2;
        var Ciudad;
        var Almacen;
        var tipo = 2;

        var exportconsi = 'F'
        $scope.myPromise = PedidosService.getReportePackingList($scope.pNumOrden, $scope.pCodSAP, $scope.pNomProveedorLog, "", "1", $scope.pUsuario, 
            $scope.pRuc, $scope.pOpcGrp1, exportconsi, Fecha1, Fecha2, Ciudad, true, $scope.bandGenTxt, $scope.bandGenXml, bandGenHtml, $scope.bandVerPdf, Almacen, $scope.isCross, $scope.selectpedidoB.codigo).then(function (results) {

            if (results.data != "") {
                var file;
                file = new Blob([results.data], { type: 'application/pdf' });
                saveAs(file, 'ReportePackingList_' + $scope.pCodSAP +  '.pdf');
            }

        }, function (error) {

            $scope.MenjError = "Error en comunicación: getReportePackingList().";
            $('#idMensajeError').modal('show');

        });

    }


    $scope.verDetalleHistorico = function () {
        $scope.npage = 1;
        $scope.isCross = true;
        var Fecha1 = ""; var Fecha2 = ""; var Ciudad = ""; var bandGenHtml = true; var Almacen = "";
        if ($scope.pOpcGrp1 == "F") {
            if ($scope.pFecha == null || $scope.pFecha == "") {
                $scope.showMessage('I', 'Seleccione la fecha para consultar.');
                return;
            }
            Fecha1 = $filter('date')($scope.pFecha, 'dd-MM-yyyy');
            Fecha2 = Fecha1;
            Ciudad = $scope.cboCiudadSelItem1.pCodCiudad;
            Almacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
        }
        if ($scope.pOpcGrp1 == "R") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }


            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
            if ($scope.cboCiudadSelItem1 != null) {
                Ciudad = $scope.cboCiudadSelItem2.pCodCiudad;
            }
            if ($scope.cboAlmacenSelItem2 != null) {
                Almacen = $scope.cboAlmacenSelItem2.pCodAlmacen;
            }

        }
        if ($scope.pOpcGrp1 == "O") {
            if ($scope.pNumOrden == "") {
                $scope.showMessage('I', 'Ingrese el número de la orden a consultar.');
                return;
            }
        }
        if ($scope.bandVerRes == false && $scope.bandGenTxt == false && $scope.bandGenXml == false && $scope.bandVerPdf == false) {
            $scope.showMessage('I', 'Se tiene que seleccionar al menos una opción para la devolución del resultado.');
            return;
        }
        $scope.pRutaDownloadTxt = "";
        $scope.pRutaDownloadXml = "";
        $scope.pRutaDownloadHtml = "";
        $scope.pRutaDownloadPdf = "";
        $scope.etiTotRegistros = "";
        //$scope.numPedidoCross = registro.numPedido;
        $scope.myPromise = PedidosService.getBuscarDetPedidosCrossHis($scope.pNumOrden, authService.authentication.CodSAP, "0",
            $scope.pRuc, $scope.pUsuario, $scope.pOpcGrp1, $scope.pOpcGrp2,
        Fecha1, Fecha2, Ciudad, true, $scope.bandGenTxt, $scope.bandGenXml, bandGenHtml, $scope.bandVerPdf, Almacen, $scope.isCross, $scope.selectpedidoB.codigo).then(function (results) {

            if (results.data.success) {
               
                if ($scope.selectpedidoB.codigo == "C") {
                    $scope.datosDetalleCrossHis = results.data.root[0];
                    $('#DetallePedidoCrossHis').modal('show');
                }
                
                if ($scope.selectpedidoB.codigo == "F") {
                    $scope.datosDetalleCrossHisFlow = results.data.root[0];
                    $('#DetallePedidoCrossHisFlow').modal('show');
                }
                
                if ($scope.selectpedidoB.codigo == "T") {

                    var tmp = $filter('filter')(results.data.root[0], { tipoPedido: 'Cross' });
                    $scope.datosDetalleCrossHis = tmp;
                    var tmp = $filter('filter')(results.data.root[0], { tipoPedido: 'Flow Reparto' });
                    $scope.datosDetalleCrossHisFlow = tmp;
                    $('#DetallePedidoCrossHisFlowTotal').modal('show');
                }
                
                


            }
            else {
                $scope.MenjError = results.data.msgError;
                $('#idMensajeError').modal('show');
            }

        }, function (error) {

            $scope.MenjError = "Error en comunicación: getBuscarDetPedidosCrossHis().";
            $('#idMensajeError').modal('show');

        });
    }


    $scope.exportar = function (tipo) {


        if ($scope.pgcDgPedidos.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeGrabar').modal('show');
            return;
        }

        var Fecha1 = ""; var Fecha2 = ""; var Ciudad = ""; var bandGenHtml = true; var Almacen = "";
        if ($scope.pOpcGrp1 == "F") {
            if ($scope.pFecha == null || $scope.pFecha == "") {
                $scope.showMessage('I', 'Seleccione la fecha para consultar.');
                return;
            }
            Fecha1 = $filter('date')($scope.pFecha, 'dd-MM-yyyy');
            Fecha2 = Fecha1;
            Ciudad = $scope.cboCiudadSelItem1.pCodCiudad;
            Almacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
        }
        if ($scope.pOpcGrp1 == "R") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }


            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
            if ($scope.cboCiudadSelItem1 != null) {
                Ciudad = $scope.cboCiudadSelItem2.pCodCiudad;
            }
            if ($scope.cboAlmacenSelItem2 != null) {
                Almacen = $scope.cboAlmacenSelItem2.pCodAlmacen;
            }

        }
        if ($scope.pOpcGrp1 == "O") {
            if ($scope.pNumOrden == "") {
                $scope.showMessage('I', 'Ingrese el número de la orden a consultar.');
                return;
            }
        }
        if ($scope.bandVerRes == false && $scope.bandGenTxt == false && $scope.bandGenXml == false && $scope.bandVerPdf == false) {
            $scope.showMessage('I', 'Se tiene que seleccionar al menos una opción para la devolución del resultado.');
            return;
        }
        $scope.myPromise = null;
        //var datos = "";
        //if (tipo == "1" || tipo == "2")
        //{
        //    datos = "XD";
        //}
        //else if (tipo == "3" || tipo == "4")
        //{
        //    datos = "XI";
        //}
        //var datos = [];
        //for (var i = 0; i < $scope.resDgPedidos.length; i++)
        //{
        //    datos[i] = $scope.resDgPedidos[i].idPedido;
        //}

        if (Almacen == undefined)
            Almacen = "";

        $scope.Tablacabecera = {};
        $scope.Tablacabecera.pCodSAP = $scope.pCodSAP;
        $scope.Tablacabecera.pRuc = $scope.pRuc;
        $scope.Tablacabecera.pUsuario = $scope.pUsuario;
        $scope.Tablacabecera.pOpcGrp1 = $scope.pOpcGrp1;
        $scope.Tablacabecera.pOpcGrp2 = "N2";
        $scope.Tablacabecera.Fecha1 = Fecha1;
        $scope.Tablacabecera.Fecha2 = Fecha2;
        $scope.Tablacabecera.Ciudad = Ciudad;
        $scope.Tablacabecera.pNumOrden = $scope.pNumOrden;
        $scope.Tablacabecera.Almacen = Almacen;
        $scope.Tablacabecera.tipo = tipo;

        $scope.myPromise = PedidosService.getExportarData($scope.Tablacabecera, $scope.resDgPedidos).then(function (results) {
            if (results.data != "") {
                if (tipo == "1") {
                    var file = new Blob([results.data], { type: 'application/txt' });
                    saveAs(file, 'pedidos_' + $scope.pCodSAP + '.txt');
                    CargaInicial3();

                    //var fileURL = URL.createObjectURL(file);
                    //window.open(fileURL, '_blank', '');


                }
                if (tipo == "2") {
                    var file = new Blob([results.data], { type: 'application/xml' });
                    saveAs(file, 'pedidos_' + $scope.pCodSAP + '.xml');
                    CargaInicial3();
                    //var fileURL = URL.createObjectURL(file);
                    //window.open(fileURL, '_blank', '');
                }
                if (tipo == "3") {
                    var file = new Blob([results.data], { type: 'application/html' });
                    saveAs(file, 'pedidos_' + $scope.pCodSAP + '.html');
                    CargaInicial4();
                    //var fileURL = URL.createObjectURL(file);
                    //window.open(fileURL, '_blank', '');
                }
                if (tipo == "4") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'pedidos_' + $scope.pCodSAP + '.pdf');
                    CargaInicial4();
                    //var fileURL = URL.createObjectURL(file);
                    //window.open(fileURL, '_blank', '');
                }
                //  window.open(results.data, '_blank', "");
            }
        }, function (error) {
        });
        setTimeout(function () { $('#consultagrid').focus(); }, 10);

    }

    function download(strData, strFileName, strMimeType) {
        var D = document,
            a = D.createElement("a");
        strMimeType = strMimeType || "application/octet-stream";


        if (navigator.msSaveBlob) { // IE10
            return navigator.msSaveBlob(new Blob([strData], { type: strMimeType }), strFileName);
        } /* end if(navigator.msSaveBlob) */


        if ('download' in a) { //html5 A[download]
            a.href = "data:" + strMimeType + "," + encodeURIComponent(strData);
            a.setAttribute("download", strFileName);
            a.innerHTML = "downloading...";
            D.body.appendChild(a);
            setTimeout(function () {
                a.click();
                D.body.removeChild(a);
            }, 66);
            return true;
        } /* end if('download' in a) */


        //do iframe dataURL download (old ch+FF):
        var f = D.createElement("iframe");
        D.body.appendChild(f);
        f.src = "data:" + strMimeType + "," + encodeURIComponent(strData);

        setTimeout(function () {
            D.body.removeChild(f);
        }, 333);
        return true;
    } /* end download() */
    function Carga() {
        $scope.totalGrid = [];
        for (var i = 0; i < $scope.resDgPedidos.length; i++) {
            var grid = {};

            grid.pOrigen = $scope.resDgPedidos[i].pOrigen;
            grid.pIdPedido = $scope.resDgPedidos[i].pIdPedido;
            grid.pNumPedido = $scope.resDgPedidos[i].pNumPedido;
            grid.pCodAlmacen = $scope.resDgPedidos[i].pCodAlmacen;
            grid.pNomAlmacen = $scope.resDgPedidos[i].pNomAlmacen;
            grid.pFechaPedido = $scope.resDgPedidos[i].pFechaPedido;
            grid.pCodAlmDestino = $scope.resDgPedidos[i].pCodAlmDestino;
            grid.pCodProveedor = $scope.resDgPedidos[i].pCodProveedor;
            grid.pNomProveedor = $scope.resDgPedidos[i].pNomProveedor;
            grid.pZonaOrigen = $scope.resDgPedidos[i].pZonaOrigen;
            grid.pItem = $scope.resDgPedidos[i].pItem;
            grid.pCodArticulo = $scope.resDgPedidos[i].pCodArticulo;
            grid.pDesArticulo = $scope.resDgPedidos[i].pDesArticulo;
            grid.pTamano = $scope.resDgPedidos[i].pTamano;
            grid.pCantPedido = $scope.resDgPedidos[i].pCantPedido;
            grid.pPrecioCosto = $scope.resDgPedidos[i].pPrecioCosto;
            grid.pUndPorCaja = $scope.resDgPedidos[i].pUndPorCaja;
            grid.pDescuento1 = $scope.resDgPedidos[i].pDescuento1;
            grid.pDescuento2 = $scope.resDgPedidos[i].pDescuento2;
            grid.pIndIva1 = $scope.resDgPedidos[i].pIndIva1;
            grid.pTamanoCaja = $scope.resDgPedidos[i].pTamanoCaja;
            grid.pCodEAN = $scope.resDgPedidos[i].pCodEAN;
            grid.esDescargado = $scope.resDgPedidos[i].esDescargado;
            grid.esImpreso = $scope.resDgPedidos[i].esImpreso;

            $scope.totalGrid.push(grid);
        }
    }
    $scope.btnConsultaClick = function () {
        $scope.npage = 1;
        $scope.isCross = true;
        var Fecha1 = ""; var Fecha2 = ""; var Ciudad = ""; var bandGenHtml = true; var Almacen = "";
        if ($scope.pOpcGrp1 == "F") {
            if ($scope.pFecha == null || $scope.pFecha == "") {
                $scope.showMessage('I', 'Seleccione la fecha para consultar.');
                return;
            }
            Fecha1 = $filter('date')($scope.pFecha, 'dd-MM-yyyy');
            Fecha2 = Fecha1;
            Ciudad = $scope.cboCiudadSelItem1.pCodCiudad;
            Almacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
        }
        if ($scope.pOpcGrp1 == "R") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }


            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
            if ($scope.cboCiudadSelItem1 != null) {
                Ciudad = $scope.cboCiudadSelItem2.pCodCiudad;
            }
            if ($scope.cboAlmacenSelItem2 != null) {
                Almacen = $scope.cboAlmacenSelItem2.pCodAlmacen;
            }

        }
        if ($scope.pOpcGrp1 == "O") {
            if ($scope.pNumOrden == "") {
                $scope.showMessage('I', 'Ingrese el número de la orden a consultar.');
                return;
            }
        }
        if ($scope.bandVerRes == false && $scope.bandGenTxt == false && $scope.bandGenXml == false && $scope.bandVerPdf == false) {
            $scope.showMessage('I', 'Se tiene que seleccionar al menos una opción para la devolución del resultado.');
            return;
        }
        $scope.pRutaDownloadTxt = "";
        $scope.pRutaDownloadXml = "";
        $scope.pRutaDownloadHtml = "";
        $scope.pRutaDownloadPdf = "";
        $scope.etiTotRegistros = "";
        $scope.myPromise = PedidosService.getConsPedidosCrossFiltro($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, $scope.pOpcGrp1, $scope.pOpcGrp2,
        Fecha1, Fecha2, Ciudad, $scope.pNumOrden, true, $scope.bandGenTxt, $scope.bandGenXml, bandGenHtml, $scope.bandVerPdf, Almacen, $scope.isCross, $scope.selectpedidoB.codigo).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0][0].length == 0) {
                    $scope.showMessage('I', 'No exiten resultado para su consulta.');
                    $scope.resDgPedidos = [];
                }
                else {
                    $scope.hideGrid = !$scope.bandVerRes;
                    $scope.hideBtnT = !$scope.bandGenTxt;
                    $scope.hideBtnX = !$scope.bandGenXml;
                    $scope.hideBtnPdf = !$scope.bandVerPdf;
                    //if ($scope.bandVerRes) {
                    $scope.resDgPedidos = results.data.root[0][1];

                    $scope.etiTotRegistros = $scope.resDgPedidos.length.toString();

                    $scope.resDgPedidostodoslosregistro = results.data.root[0][0];
                    //}
                    //if ($scope.bandGenTxt) {
                    //    $scope.pRutaDownloadTxt = results.data.root[0][1];
                    //}
                    //if ($scope.bandGenXml) {
                    //    $scope.pRutaDownloadXml = results.data.root[0][2];
                    //}
                    //if ($scope.bandVerPdf) {
                    //    $scope.pRutaDownloadPdf = results.data.root[0][4];
                    //}
                    //$scope.pRutaDownloadHtml = results.data.root[0][3];

                    $scope.showPaginate = true;

                }
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#rbtPorFecha').focus(); }, 150);
        },
         function (error) {
             var errors = [];
             for (var key in error.data.modelState) {
                 for (var i = 0; i < error.data.modelState[key].length; i++) {
                     errors.push(error.data.modelState[key][i]);
                 }
             }
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
        setTimeout(function () { $('#btnConsulta').focus(); }, 100);
        setTimeout(function () { $('#rbtPorFecha').focus(); }, 150);
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

    //setTimeout(function () { $('#btnConsulta').focus(); }, 150);
    //setTimeout(function () { angular.element('#btnConsulta').trigger('click'); }, 250);
    $scope.btnConsultaClick();
}]);


'use strict';
app.controller('ReporteCrossConsolidadoController', ['$scope', '$location', 'ReportCrosseService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, ReportCrosseService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {
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
    //Variable de Busquedas
    $scope.txtsap = "";
    $scope.txtruc = "";
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    //Fin Variable



    $scope.pOpcGrp1 = "F";
    $scope.etiTotRegistros = "";
    //Combos
    $scope.EstadoSolicitudGrip = [];
    $scope.selectedItemGrip = "";
    //Fin Combos

    $scope.porestado = 1;

    $scope.GridConsolidacion = [];
    var _GridConsolidacion = [];
    $scope.pagesCo = [];
    $scope.pageContentCo = [];

    $scope.resDgConsulta = [];
    $scope.sortTypeCon = 'idconsolidacion';

    $scope.pageContentCon = [];

    $scope.codid = "";

    var dateString1 = new Date();
    var dateString = new Date();

    var d1 = dateString1.format("dd/mm/yyyy");

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");


    //$scope.pFechaIni = "";
    //$scope.pFechaFin = "";


    $scope.pFechaIni = d2;
    $scope.pFechaFin = d1;

    $scope.traCho = {};
    $scope.traCho.Tipo = "1";
    $scope.traCho.txtfechasolicitud = "";
    $scope.traCho.horainicial = "";
    $scope.traCho.horafinal = "";


    $scope.sortType = 'nombres'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';
    $scope.rutaDirectorio = "";
    //Fin Variable

    $scope.resDgPedidostodoslosregistro = [];
    $scope.resDgPedidostodoslosregistrousuario = [];


    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    //Fin Variable

    $scope.CodProveedor = authService.authentication.CodSAP;
    $scope.usuarioCreacion = authService.authentication.userName;
    $scope.nomProveedor = authService.authentication.nomEmpresa;
    $scope.variablelleva = "";
    $scope.id = "";
    $scope.idcita = "";


    $scope.cboCiudadList1 = [];
    $scope.cboCiudadSelItem1 = null;

    $scope.cboCiudadList2 = [];
    $scope.cboCiudadSelItem2 = null;

    $scope.cboAlmacenList1 = [];
    $scope.cboAlmacenList1T = [];
    $scope.cboAlmacenSelItem1 = null;

    $scope.cboAlmacenList2 = [];
    $scope.cboAlmacenList2T = [];
    $scope.cboAlmacenSelItem2 = null;

    $scope.$watch('cboCiudadSelItem1', function () {
        if ($scope.cboCiudadSelItem1 == null) return;


        if ($scope.cboCiudadSelItem1.pCodCiudad == "-999") {
            $scope.cboAlmacenList1 = $scope.cboAlmacenList1T;

        }
        else {
            $scope.cboAlmacenList1 = [];

            $scope.cboAlmacenList1 = $filter('filter')($scope.cboAlmacenList1T, { pCodCiudad: $scope.cboCiudadSelItem1.pCodCiudad }, true);
            $scope.cboAlmacenList1.splice(0, 0, { pCodAlmacen: "-999", pNomAlmacen: "Todos los Almacenes" });
        }
        $scope.cboAlmacenSelItem1 = $scope.cboAlmacenList1[0];

    });

    $scope.$watch('cboCiudadSelItem2', function () {
        if ($scope.cboCiudadSelItem2 == null) return;


        if ($scope.cboCiudadSelItem2.pCodCiudad == "-999") {
            $scope.cboAlmacenList2 = $scope.cboAlmacenList1T;

        }
        else {
            $scope.cboAlmacenList2 = [];

            $scope.cboAlmacenList2 = $filter('filter')($scope.cboAlmacenList2T, { pCodCiudad: $scope.cboCiudadSelItem2.pCodCiudad }, true);
            $scope.cboAlmacenList2.splice(0, 0, { pCodAlmacen: "-999", pNomAlmacen: "Todos los Almacenes" });
        }
        $scope.cboAlmacenSelItem2 = $scope.cboAlmacenList2[0];

    });

    $scope.myPromise = ReportCrosseService.getConsCiudadesEnAlmacen().then(function (results) {
        if (results.data.success) {
            var listCiud = results.data.root[0];
            listCiud.splice(0, 0, { pCodCiudad: "-999", pNomCiudad: "Todas las Ciudades" });
            $scope.cboCiudadList1 = listCiud;
            $scope.cboCiudadList2 = listCiud;
            $scope.cboCiudadSelItem1 = listCiud[0];
            $scope.cboCiudadSelItem2 = listCiud[0];
        }
        else {
            $scope.showMessage('E', 'Error al consultar ciudades: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: getConsCiudadesEnAlmacen().");
    });

    $scope.myPromise = ReportCrosseService.getConsAlmacenes("4").then(function (results) {
        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            listAlmacen.splice(0, 0, { pCodAlmacen: "-999", pNomAlmacen: "Todos los Almacenes" });
            $scope.cboAlmacenList1T = listAlmacen;
            $scope.cboAlmacenList1 = listAlmacen;
            $scope.cboAlmacenSelItem1 = listAlmacen[0];
            $scope.cboAlmacenList2T = listAlmacen;
            $scope.cboAlmacenList2 = listAlmacen;
            $scope.cboAlmacenSelItem2 = listAlmacen[0];

        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: getConsAlmacenes().");
    });
    $scope.limpioCaja = function () {
        $scope.txtsap = "";
        $scope.txtusuario = "";

    }

    $scope.selRowGrid = function (content) {
        $scope.pgcDgPedidosDET = [];
        $scope.pgcDgPedidosDET = $filter('filter')($scope.resDgPedidostodoslosregistro, { codigo: content.codigo });

        $scope.etiTotRegistrosdetlog = "";
        $scope.etiTotRegistrosdetlog = $scope.pgcDgPedidosDET.length.toString();

        $('#MostradetalleLogComunicado').modal('show');
    }

    $scope.selRselRowGridDETowGrid = function (content) {
        $scope.pgcDgPedidosDETus = [];
        $scope.pgcDgPedidosDETus = $filter('filter')($scope.resDgPedidostodoslosregistrousuario, { cod_notificacion: content.codigo, cod_proveedor: content.codproveedor });

        $scope.etiTotRegistrosdetlogdet = "";
        $scope.etiTotRegistrosdetlogdet = $scope.pgcDgPedidosDETus.length.toString();

        $('#MostradetalleLogComunicadodet').modal('show');
    }
    $scope.Consultar = function () {
        var Fecha1 = "";
        var Fecha2 = "";



        if ($scope.pOpcGrp1 == "F") {
            if ($scope.pFecha == null || $scope.pFecha == "") {
                $scope.showMessage('I', 'Seleccione la fecha para consultar.');
                return;
            }
            Fecha1 = $filter('date')($scope.pFecha, 'dd-MM-yyyy');
            Fecha2 = Fecha1;
            Ciudad = $scope.cboCiudadSelItem1.pCodCiudad;
            Almacen = $scope.cboAlmacenSelItem1.pCodAlmacen;
        }

        if ($scope.pOpcGrp1 == "R") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }


            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
            if ($scope.cboCiudadSelItem1 != null) {
                Ciudad = $scope.cboCiudadSelItem2.pCodCiudad;
            }
            if ($scope.cboAlmacenSelItem2 != null) {
                Almacen = $scope.cboAlmacenSelItem2.pCodAlmacen;
            }

        }

        $scope.myPromise = null;
        $scope.etiTotRegistros = "";
        $scope.myPromise = ReportCrosseService.getConsulaGriCrossConsolidado($scope.CodProveedor, Fecha1, Fecha2, Ciudad, Almacen).then(function (results) {
            if (results.data.success) {
                $scope.resDgConsulta = results.data.root[0];
                $scope.etiTotRegistros = $scope.resDgConsulta.length.toString();

                //$scope.resDgPedidostodoslosregistro = results.data.root[1];
                //$scope.resDgPedidostodoslosregistrousuario = results.data.root[2];
            }
            setTimeout(function () { $('#btnConsulta1').focus(); }, 100);
            setTimeout(function () { $('#rbtPorFecha').focus(); }, 150);

        }, function (error) {
        });
    }

    function validate_fechaMayorQue(fechaInicial, fechaFinal) {

        valuesStart = fechaInicial.split("/");

        valuesEnd = fechaFinal.split("/");



        // Verificamos que la fecha no sea posterior a la actual

        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);

        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);

        if (dateStart >= dateEnd) {

            return 1;

        }

        return 0;

    }
    $scope.exportar = function (tipo) {
        if ($scope.pageContentCo.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeGrabar').modal('show');
            return;
        }

        var Fecha1 = "";
        var Fecha2 = "";

        if ($scope.pOpcGrp1 == "R") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }


            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
            if ($scope.cboCiudadSelItem1 != null) {
                Ciudad = $scope.cboCiudadSelItem2.pCodCiudad;
            }
            if ($scope.cboAlmacenSelItem2 != null) {
                Almacen = $scope.cboAlmacenSelItem2.pCodAlmacen;
            }

        }
        $scope.myPromise = null;

        $scope.myPromise = ReportCrosseService.getExportarCrossConsolidado(tipo, $scope.CodProveedor, $scope.nomProveedor, $scope.usuarioCreacion, Fecha1, Fecha2, Ciudad, Almacen).then(function (results) {
            if (results.data != "") {
                if (tipo == "1") {
                    var file = new Blob([results.data], { type: 'application/xls' });
                    saveAs(file, 'ReporteCrossConsolidado.xls');
                }
                if (tipo == "2") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'ReporteCrossConsolidado.pdf');
                }
            }
        }, function (error) {
        });
        setTimeout(function () { $('#consultagrid').focus(); }, 10);

    }


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
}
]);
