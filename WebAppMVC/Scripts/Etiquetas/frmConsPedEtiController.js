
//'use strict';
app.controller('frmConsPedEtiController', ['$scope', 'EtiquetasService', '$filter', 'authService', function ($scope, EtiquetasService, $filter, authService) {
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
    $scope.solicitud = {}
    $scope.solicitud.opcion = "I";
    $scope.idgridp = "";

    $scope.limpiaControles = function (op, solicitud) {
        if (op === 'A') {
            $scope.solicitud.opcion = op;
            $scope.solicitud.canDespachar = solicitud.canDespachar;
            $scope.solicitud.fechaEntrega = solicitud.fechaEntrega
            $scope.solicitud.idSolicitud = solicitud.idSolEtiqueta;
            $scope.bloqueaSol = false;
            $scope.auxSaldo = parseInt(solicitud.canDespachar);
        }
        else if (op === 'I') {
            $scope.solicitud.opcion = op;
            $scope.solicitud.canDespachar = "";
            $scope.solicitud.fechaEntrega = "";
            $scope.solicitud.idSolicitud = "";
            verificaSol();
        }
    }

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
    $scope.pOpcGrp2 = "N";


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
    $scope.bandVerPdf = false;
    $scope.bandGenTxt = false;
    $scope.bandGenXml = false;

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



    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];
    $scope.cboPaginSelItem = $scope.cboPaginList[0];

    //------ combo estados
    $scope.cboEstados = [{ Descripcion: 'Todos', Id: 'T' }, { Descripcion: 'En Solicitud', Id: 'S' }, { Descripcion: 'Cancelado', Id: 'C' }, { Descripcion: 'Generando Impresión', Id: 'G' }, { Descripcion: 'Recibida por el proveedor', Id: 'R' }];
    $scope.cboEstadosSel1 = $scope.cboEstados[0];
    $scope.cboEstadosSel2 = $scope.cboEstados[0];

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
    $scope.tipoTra = "";

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
    $scope.grabar = function () {

        $scope.myPromise = "";
        $scope.myPromise = EtiquetasService.getActualizarSolicitudEtiquetas($scope.tipoTra, $scope.pgcDgPedidosDET).then(function (results) {
            $scope.MenjError = "Solicitud Modificada correctamente"
            $('#idMensajeOk').modal('show');
        }, function (error) {
            $scope.showMessage('E', "No se puedo modificar la solicitud");
        });
    }

    $scope.okGrabarCargar = function () {
        $('#MostradetallePedido').modal('hide');
        $scope.btnConsultaClick();
    }
    $scope.confirmarGrabar = function () {
        $scope.MenjConfirmacion = "¿Está seguro de Guardar la solicitud " + $scope.tipoTra + "?";
        $('#idMensajeConfirmacion').modal('show');
    }
    $scope.EliminarRegistro = function (idSolEtiqueta, estado) {
        $scope.tipoTra = "E";
        if (estado == "En Solicitud") {
            $scope.idgridp = idSolEtiqueta;
            $scope.MenjConfirmacion = "¿Está seguro de eliminar la solicitud " + idSolEtiqueta + "?";
            $('#idMensajeConfirmacionEliminar').modal('show');
        }
        else {
            $scope.MenjError = "Solo se puede eliminar solicitud en estado:" + estado;
            $('#idMensajeError').modal('show');
        }
    }

    $scope.Eliminar = function () {
        $scope.myPromise = "";
        $scope.myPromise = EtiquetasService.geteliminarSolicitudEti("E", $scope.idgridp, "C").then(function (results) {
            if (results.data) {
                $scope.MenjError = "Se eliminó la solicitud sin novedad";
                $('#idMensajeOkeliminar').modal('show');
            }
            else {
                $scope.MenjError = "No se puedo eliminar la solicitud"
                $('#idMensajeError').modal('show');
            }
        },
                function (error) {
                    var errors = [];
                    for (var key in error.data.modelState) {
                        for (var i = 0; i < error.data.modelState[key].length; i++) {
                            errors.push(error.data.modelState[key][i]);
                        }
                    }
                    $scope.MenjError = "No se puedo eliminar la solicitud"
                    $('#idMensajeError').modal('show');
                });
    }

    $scope.okGrabar = function () {
        $scope.btnConsultaClick();
    }

    $scope.selRowGrid = function (content) {
        $scope.tipoTra = content.idSolEtiqueta;
        $scope.myPromise = "";
        $scope.myPromise = EtiquetasService.getPedidoSolicitud(content.idSolEtiqueta, "L").then(function (results) {
            if (results.data.success) {
                $scope.pgcDgPedidosDET = [];
                $scope.pgcDgPedidosDET = results.data.root[0][0];
                $scope.etiTotRegistrosdet = "";
                $scope.etiTotRegistrosdet = $scope.pgcDgPedidosDET.length.toString();
                $('#MostradetallePedido').modal('show');
            }
            else {
                $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
            }
        }, function (error) {
            $scope.showMessage('E', "Error en comunicación: getConsAlmacenes().");
        });




    }

    $scope.selRowGridEti = function (content) {
        $scope.rowDetalle = content
        $scope.dsEtiquetas = [];
        $scope.pedido = angular.copy(content.pIdPedido);
        $scope.articulo = angular.copy(content.pCodArticulo);
        $scope.cantPedido = angular.copy(content.pCantPedido);
        cargaSolicitudes();
        //$scope.dsEtiquetas = $filter('filter')($scope.resDsSolicituEtiqueta, { idPedido: content.pIdPedido, codArticulo: content.pCodArticulo });


        $scope.etiTotRegistrosEti = "";
        $scope.etiTotRegistrosEti = $scope.dsEtiquetas.length.toString();

        $scope.solicitud.idPedido = content.pIdPedido;
        $scope.solicitud.numPedido = content.pNumPedido;
        $scope.solicitud.codArticulo = content.pCodArticulo;
        $scope.solicitud.canDespachar = "";
        $scope.solicitud.fechaEntrega = "";
        $scope.solicitud.idSolicitud = "";

        $('#MostrarDsEtiqueta').modal('show');
    }

    $scope.myPromise = EtiquetasService.getConsCiudadesEnAlmacen().then(function (results) {
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

    $scope.myPromise = EtiquetasService.getConsAlmacenes("4").then(function (results) {
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


        $scope.myPromise = EtiquetasService.getExportarData($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, $scope.pOpcGrp1, $scope.pOpcGrp2,
        Fecha1, Fecha2, Ciudad, $scope.pNumOrden, Almacen, tipo).then(function (results) {
            if (results.data != "") {
                if (tipo == "1") {
                    var file = new Blob([results.data], { type: 'application/txt' });
                    saveAs(file, 'pedidos_' + $scope.pCodSAP + '.txt');
                    //if (window.saveAs) {
                    //    window.saveAs(file, 'pedidos_' + $scope.pCodSAP + '.txt');
                    //}
                    //else {
                    //    navigator.saveBlob(file, 'pedidos_' + $scope.pCodSAP + '.txt');
                    //}

                }
                if (tipo == "2") {
                    var file = new Blob([results.data], { type: 'application/xml' });
                    saveAs(file, 'pedidos_' + $scope.pCodSAP + '.xml');
                    //if (window.saveAs) {
                    //    window.saveAs(file, 'pedidos_' + $scope.pCodSAP + '.xml');
                    //}
                    //else {
                    //    navigator.saveBlob(file, 'pedidos_' + $scope.pCodSAP + '.xml');
                    //}
                }
                if (tipo == "3") {
                    var file = new Blob([results.data], { type: 'application/html' });
                    saveAs(file, 'pedidos_' + $scope.pCodSAP + '.html');

                    //            var data = { x: 42, s: "hello, world", d: new Date() },
                    //fileName = "my-download.json";
                    //            var json = JSON.stringify(data),
                    //                blob = new Blob([json], { type: "octet/stream" });

                    //            saveAs(blob, fileName);

                    //if (window.saveAs) {
                    //    window.saveAs(file, 'pedidos_' + $scope.pCodSAP + '.html');
                    //}
                    //else {
                    // navigator.msSaveOrOpenBlob(file, 'pedidos_' + $scope.pCodSAP + '.html');
                    //}
                }
                if (tipo == "4") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'pedidos_' + $scope.pCodSAP + '.pdf');
                    //if (window.saveAs) {
                    //    window.saveAs(file, 'pedidos_' + $scope.pCodSAP + '.pdf');
                    //}
                    //else {
                    //    navigator.saveBlob(file, 'pedidos_' + $scope.pCodSAP + '.pdf');
                    //}
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
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";
        if ($scope.pOpcGrp1 == "F") {
            if ($scope.pFecha == null || $scope.pFecha == "") {
                $scope.showMessage('I', 'Seleccione la fecha para consultar.');
                return;
            }
            Fecha1 = $filter('date')($scope.pFecha, 'dd-MM-yyyy');
            Fecha2 = Fecha1;
            if ($scope.cboEstadosSel1 != null) {
                Estado = $scope.cboEstadosSel1.Id;
            }
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
            if ($scope.cboEstadosSel2 != null) {
                Estado = $scope.cboEstadosSel2.Id;
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
        $scope.myPromise = EtiquetasService.getConsPedEti($scope.pOpcGrp1, Fecha1, Fecha2, Estado, $scope.pNumOrden, $scope.pCodSAP).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No se encontraron registros con los valores especificados.');
                    $scope.resDgPedidos = [];
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {
                    $scope.resDgPedidos = results.data.root[0];
                    //$scope.pgcDgPedidos = $scope.resDgPedidos;
                    $scope.etiTotRegistros = $scope.resDgPedidos.length.toString();

                    $scope.showPaginate = true;
                    $scope.hideGrid = false;

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

    $scope.Mantenimiento = function () {
        if ($scope.solicitud.opcion == 'I')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo))
                EtiquetasService.mntSolicitud($scope.solicitud).then(function (results) {
                    cargaSolicitudes();
                    $scope.solicitud.opcion = 'I';
                }, function (error) {

                });


        if ($scope.solicitud.opcion == 'A')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo) + parseInt($scope.auxSaldo))
                EtiquetasService.mntSolicitud($scope.solicitud).then(function (results) {
                    cargaSolicitudes();
                    $scope.solicitud.opcion = 'I';
                }, function (error) {

                });
    }

    $scope.calculaCantDesp = function (pedido, articulo) {
        var filtro = $filter('filter')($scope.resDsSolicituEtiqueta, { idPedido: pedido, codArticulo: articulo });
        var cont = 0;

        for (var i = 0; i < filtro.length; i++)
            cont += filtro[i].canDespachar;

        return cont;
    }

    function cargaSolicitudes() {
        EtiquetasService.consSolicitud('C', $scope.pedido, $scope.articulo).then(function (results) {
            $scope.dsEtiquetas = results.data[0];
            $scope.resDsSolicituEtiqueta = results.data[1];

            $scope.etiTotRegistrosEti = "";
            $scope.etiTotRegistrosEti = $scope.dsEtiquetas.length.toString();

            $scope.solicitud.canDespachar = "";
            $scope.solicitud.fechaEntrega = "";
            $scope.solicitud.idSolicitud = "";

            verificaSol();

        }, function (error) {

        });

    }

    function verificaSol() {
        var cont = 0;

        for (var i = 0; i < $scope.dsEtiquetas.length; i++)
            cont += $scope.dsEtiquetas[i].canDespachar;

        if (cont < $scope.cantPedido)
            $scope.bloqueaSol = false;
        else
            $scope.bloqueaSol = true;
    }

    $scope.showMessage = function (tipo, mensaje) {

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
        else if (tipo == 'IR') {
            $('#idMensajeInformativoredi').modal('show');
        }
    }

    $scope.Redireccion = function () {
        window.location = "../Notificacion/frmVisualizaNotificaciones";
    }
    $scope.CargaInicial = function () {
        $scope.myPromise = EtiquetasService.getBuscarAsignacion("AP", authService.authentication.CodSAP).then(function (results) {
            if (results.data == "0") {
                $scope.showMessage('IR', 'Estimado usuario usted no esta autorizado para visualizar esta opción');
                return;
            } else {
                if (results.data == "1") {
                    $scope.btnConsultaClick();
                } else {

                }
            }
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
    }
    $scope.CargaInicial();
}]);



//'use strict';
app.controller('frmConsEtiquetasTipoController', ['$scope', 'EtiquetasService', '$filter', 'authService', function ($scope, EtiquetasService, $filter, authService) {
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
    $scope.solicitud = {}
    $scope.solicitud.opcion = "I";
    $scope.idgridp = "";

    $scope.limpiaControles = function (op, solicitud) {
        if (op === 'A') {
            $scope.solicitud.opcion = op;
            $scope.solicitud.canDespachar = solicitud.canDespachar;
            $scope.solicitud.fechaEntrega = solicitud.fechaEntrega
            $scope.solicitud.idSolicitud = solicitud.idSolEtiqueta;
            $scope.bloqueaSol = false;
            $scope.auxSaldo = parseInt(solicitud.canDespachar);
        }
        else if (op === 'I') {
            $scope.solicitud.opcion = op;
            $scope.solicitud.canDespachar = "";
            $scope.solicitud.fechaEntrega = "";
            $scope.solicitud.idSolicitud = "";
            verificaSol();
        }
    }

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
    $scope.pOpcGrp2 = "N";


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
    $scope.bandVerPdf = false;
    $scope.bandGenTxt = false;
    $scope.bandGenXml = false;

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
    $scope.pEmpresa = authService.authentication.nomEmpresa;
    $scope.pCodSAP = authService.authentication.CodSAP;



    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];
    $scope.cboPaginSelItem = $scope.cboPaginList[0];

    //------ combo estados
    $scope.cboEstados = [{ Descripcion: 'Todos', Id: 'T' }, { Descripcion: 'En Solicitud', Id: 'S' }, { Descripcion: 'Cancelado', Id: 'C' }, { Descripcion: 'Generando Impresión', Id: 'G' }, { Descripcion: 'Recibida por el proveedor', Id: 'R' }];
    $scope.cboEstadosSel1 = $scope.cboEstados[0];
    $scope.cboEstadosSel2 = $scope.cboEstados[0];

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
    $scope.tipoTra = "";

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
    $scope.grabar = function () {

        $scope.myPromise = "";
        $scope.myPromise = EtiquetasService.getActualizarSolicitudEtiquetas($scope.tipoTra, $scope.pgcDgPedidosDET).then(function (results) {
            $scope.MenjError = "Solicitud Modificada correctamente"
            $('#idMensajeOk').modal('show');
        }, function (error) {
            $scope.showMessage('E', "No se puedo modificar la solicitud");
        });
    }

    $scope.okGrabarCargar = function () {
        $('#MostradetallePedido').modal('hide');
        $scope.btnConsultaClick();
    }
    $scope.confirmarGrabar = function () {
        $scope.MenjConfirmacion = "¿Está seguro de Guardar la solicitud " + $scope.tipoTra + "?";
        $('#idMensajeConfirmacion').modal('show');
    }
    $scope.EliminarRegistro = function (idSolEtiqueta, estado) {
        $scope.tipoTra = "E";
        if (estado == "En Solicitud") {
            $scope.idgridp = idSolEtiqueta;
            $scope.MenjConfirmacion = "¿Está seguro de eliminar la solicitud " + idSolEtiqueta + "?";
            $('#idMensajeConfirmacionEliminar').modal('show');
        }
        else {
            $scope.MenjError = "Solo se puede eliminar solicitud en estado:" + estado;
            $('#idMensajeError').modal('show');
        }
    }

 

    $scope.okGrabar = function () {
        $scope.btnConsultaClick();
    }

    $scope.selRowGrid = function (content) {
        $scope.tipoTra = content.idSolEtiqueta;
        $scope.myPromise = "";
        $scope.myPromise = EtiquetasService.getPedidoSolicitud(content.idSolEtiqueta, "L").then(function (results) {
            if (results.data.success) {
                $scope.pgcDgPedidosDET = [];
                $scope.pgcDgPedidosDET = results.data.root[0][0];
                $scope.etiTotRegistrosdet = "";
                $scope.etiTotRegistrosdet = $scope.pgcDgPedidosDET.length.toString();
                $('#MostradetallePedido').modal('show');
            }
            else {
                $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
            }
        }, function (error) {
            $scope.showMessage('E', "Error en comunicación: getConsAlmacenes().");
        });




    }

    $scope.selRowGridEti = function (content) {
        $scope.rowDetalle = content
        $scope.dsEtiquetas = [];
        $scope.pedido = angular.copy(content.pIdPedido);
        $scope.articulo = angular.copy(content.pCodArticulo);
        $scope.cantPedido = angular.copy(content.pCantPedido);
        cargaSolicitudes();
        //$scope.dsEtiquetas = $filter('filter')($scope.resDsSolicituEtiqueta, { idPedido: content.pIdPedido, codArticulo: content.pCodArticulo });


        $scope.etiTotRegistrosEti = "";
        $scope.etiTotRegistrosEti = $scope.dsEtiquetas.length.toString();

        $scope.solicitud.idPedido = content.pIdPedido;
        $scope.solicitud.numPedido = content.pNumPedido;
        $scope.solicitud.codArticulo = content.pCodArticulo;
        $scope.solicitud.canDespachar = "";
        $scope.solicitud.fechaEntrega = "";
        $scope.solicitud.idSolicitud = "";

        $('#MostrarDsEtiqueta').modal('show');
    }

    $scope.myPromise = EtiquetasService.getConsCiudadesEnAlmacen().then(function (results) {
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
        if ($scope.pgcDgPedidos.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeGrabar').modal('show');
            return;
        }

        var Fecha1 = ""; var Fecha2 = ""; var Ciudad = ""; var bandGenHtml = true; var Almacen = "";

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


        $scope.myPromise = EtiquetasService.getExportarDataImpresas(tipo,$scope.pUsuario, $scope.pEmpresa, Fecha1, Fecha2, $scope.pCodSAP).then(function (results) {
            if (results.data != "") {
                if (tipo == "1") {
                    var file = new Blob([results.data], { type: 'application/xls' });
                    saveAs(file, 'EtiquetasImpresas_' + $scope.pCodSAP + '.xls');
                }
                if (tipo == "2") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'EtiquetasImpresas_' + $scope.pCodSAP + '.pdf');
                }
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
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

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
            if ($scope.cboEstadosSel2 != null) {
                Estado = $scope.cboEstadosSel2.Id;
            }

        }

        $scope.myPromise = EtiquetasService.getConsPedEtiImpresas(Fecha1, Fecha2, $scope.pCodSAP).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No se encontraron registros con los valores especificados.');
                    $scope.resDgPedidos = [];
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {
                    $scope.resDgPedidos = results.data.root[0];
                    //$scope.pgcDgPedidos = $scope.resDgPedidos;
                    $scope.etiTotRegistros = $scope.resDgPedidos.length.toString();

                    $scope.showPaginate = true;
                    $scope.hideGrid = false;

                }
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#rbtPorRango').focus(); }, 150);
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
        setTimeout(function () { $('#rbtPorRango').focus(); }, 150);
    };

    $scope.calculaCantDesp = function (pedido, articulo) {
        var filtro = $filter('filter')($scope.resDsSolicituEtiqueta, { idPedido: pedido, codArticulo: articulo });
        var cont = 0;

        for (var i = 0; i < filtro.length; i++)
            cont += filtro[i].canDespachar;

        return cont;
    }

    function cargaSolicitudes() {
        EtiquetasService.consSolicitud('C', $scope.pedido, $scope.articulo).then(function (results) {
            $scope.dsEtiquetas = results.data[0];
            $scope.resDsSolicituEtiqueta = results.data[1];

            $scope.etiTotRegistrosEti = "";
            $scope.etiTotRegistrosEti = $scope.dsEtiquetas.length.toString();

            $scope.solicitud.canDespachar = "";
            $scope.solicitud.fechaEntrega = "";
            $scope.solicitud.idSolicitud = "";

            verificaSol();

        }, function (error) {

        });

    }

    function verificaSol() {
        var cont = 0;

        for (var i = 0; i < $scope.dsEtiquetas.length; i++)
            cont += $scope.dsEtiquetas[i].canDespachar;

        if (cont < $scope.cantPedido)
            $scope.bloqueaSol = false;
        else
            $scope.bloqueaSol = true;
    }

    $scope.showMessage = function (tipo, mensaje) {

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
        else if (tipo == 'IR') {
            $('#idMensajeInformativoredi').modal('show');
        }
    }

    $scope.Redireccion = function () {
        window.location = "../Notificacion/frmVisualizaNotificaciones";
    }
    $scope.CargaInicial = function () {
        $scope.myPromise = EtiquetasService.getBuscarAsignacion("AP", authService.authentication.CodSAP).then(function (results) {
            if (results.data == "0") {
                $scope.showMessage('IR', 'Estimado usuario usted no esta autorizado para visualizar esta opción');
                return;
            } else {
                if (results.data == "1") {
                    $scope.btnConsultaClick();
                } else {

                }
            }
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
    }
    $scope.CargaInicial();
}]);
