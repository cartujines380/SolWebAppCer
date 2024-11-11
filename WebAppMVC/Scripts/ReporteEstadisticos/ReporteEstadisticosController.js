
//var chartDirective = function () {
app.directive([function () {
    return {
        restrict: 'E',
        replace: true,
        template: '<div></div>',
        scope: {
            config: '='
        },
        link: function (scope, element, attrs) {
            var chart;
            var process = function () {
                var defaultOptions = {
                    chart: { renderTo: element[0] },
                };
                var config = angular.extend(defaultOptions, scope.config);
                chart = new Highcharts.Chart(config);
            };
            process();
            scope.$watch("config.series", function (loading) {
                process();
            });
            scope.$watch("config.loading", function (loading) {
                if (!chart) {
                    return;
                }
                if (loading) {
                    chart.showLoading();
                } else {
                    chart.hideLoading();
                }
            });
        }
    };
}]);
//'use strict';
app.controller('ReporteEstadisticosController', ['$scope', 'ReporteEstadisticosService','SeguridadService', '$filter', 'authService', function ($scope, ReporteEstadisticosService,SeguridadService, $filter, authService) {
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
    $scope.resDgPedidosDatos = [];
    var _resDgPedidos = [];
    $scope.pagesPedidos = [];
    $scope.pgcDgPedidos = [];



    $scope.grPedido = [];
    var _resDgPedidos = [];
    $scope.pageContentPed = [];
    $scope.pagesPed = [];
    $scope.totalGrid = {};

    $scope.resDgPedidostodoslosregistro = [];

    $scope.pgcDgPedidosDET = [];

    $scope.pOpcGrp1 = "P";
    $scope.pOpcGrp2 = "N";


    var dateString1 = new Date();
    var dateString = new Date();

    var d1 = dateString1.format("dd/mm/yyyy");

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");

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

    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;

    $scope.listaAlmacenes = [];
    $scope.selecAlmacenes = [];
    $scope.SettingAlmacenes = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '300px', scrollable: true,  buttonClasses: 'btn btn-default btn-multiselect-chk' };
    //$scope.SettingAlmacenes = { selectionLimit: 1 };


    $scope.chkArticuloList = [];
    $scope.chkArticuloSelModel = [];
    $scope.chkArticuloSettings = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];
    $scope.cboPaginSelItem = $scope.cboPaginList[0];

    //------ combo estados

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

    $scope.codigoImagen = [];
    $scope.genaralData = [];
    $scope.genaralCentro = [];




    $scope.Eliminar = function () {
        $('#container').highcharts({
            title: { text: ' ' },
            series: []
        });

    }
    $scope.myPromise = SeguridadService.verificarTransaccion("4001").then(function (results) {
        if (results.data==false) {
            window.location = "../Notificacion/frmVisualizaNotificaciones";
        }
    }, function (error) {
    });

    $scope.CargarGrafico = function () {

        $scope.Eliminar();
        var rndtotal = [];
        //rndtotal.name = "";
        //rndtotal.data = [];
        var cantidad = $scope.genaralCentro.length - 1;
        if (cantidad > 9) {
            cantidad = 9;
        }
        var categorias = [];
        var data = [];

        for (var i = 0; i < $scope.genaralCentro.length; i++) {
            categorias.push($scope.genaralCentro[i].almacen);
        }
        
        for (var i = 0; i < $scope.codigoImagen.length; i++) {
            var resumen = $filter('filter')($scope.resDgPedidosDatos, { codMaterial: $scope.codigoImagen[i].codMaterial }, true);

            var auxData = {};

            auxData.name = resumen[0].desMaterial;
            auxData.data = [];
            for (var j = 0; j < $scope.genaralCentro.length; j++) {
                var aux = $filter('filter')(resumen, { codCentro: $scope.genaralCentro[j].centro }, true);

                if (aux.length > 0)
                    auxData.data.push(parseInt(aux[0].cantVendida));
                else
                    if (aux.length == 0)
                        auxData.data.push(0);
            }
            data.push(auxData);
        }
        //for (var i = 0; i < $scope.codigoImagen.length; i++) {
        //    var resumen = $filter('filter')($scope.genaralData, { codMaterial: $scope.codigoImagen[i].codMaterial }, true);
        //    var rnd = [];
        //    for (var j = 0; j < resumen.length; j++) {
        //        rnd.push({ name: resumen[j].nomAlmacen, y: parseInt(resumen[j].cantVendida, 10) });// resumen[j].cantVendida });
        //    }
        //    rndtotal.push({ name: $scope.codigoImagen[i].desMaterial, data: rnd })
        //}


        $('#container').highcharts({
            chart: {
                type: 'column',
                borderWidth: 1,
                zoomType: 'xy'
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        style: { fontSize: '8px', fontFamily: 'Verdana, sans-serif' },
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                }
            },
            navigation: {
                buttonOptions: {
                    align: 'center'
                }
            },
            yAxis: [{ title: { text: "Ventas Totales" } }], stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            },

           

            xAxis: {
                min: 0,
                max: cantidad,
                categories: categorias
            },
            scrollbar: {
                enabled: true
            },
            title: { text: 'Ventas por Artículo Periodo:'+$scope.pFechaIni + ' - '+ $scope.pFechaFin },
            scrollbar: {
                enabled: true,
                barBackgroundColor: 'gray',
                barBorderRadius: 7,
                barBorderWidth: 0,
                buttonBackgroundColor: 'gray',
                buttonBorderWidth: 0,
                buttonArrowColor: 'yellow',
                buttonBorderRadius: 7,
                rifleColor: 'yellow',
                trackBackgroundColor: 'white',
                trackBorderWidth: 1,
                trackBorderColor: 'silver',
                trackBorderRadius: 7
            },
            series: data
            });

    }

  

    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("1", $scope.pCodSAP).then(function (results) {
        
        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            $scope.listaAlmacenes = listAlmacen;
         }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });

    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("2", $scope.pCodSAP).then(function (results) {

        if (results.data.success) {
            var listArticulo = results.data.root[0];
            $scope.chkArticuloList = listArticulo;
        }
        else {
            $scope.showMessage('E', 'Error al consultar Articulos: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesArticulosReportes().");
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
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

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
        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }
        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        cabeceraDatos.p_usuario = $scope.pUsuario;
        cabeceraDatos.nomreporte = "VENTAS POR ARTÍCULO";
        $scope.resDgPedidos = [];



        if ($scope.etiTotRegistros == '' || $scope.etiTotRegistros == '0') {
            $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
            return;
        }

        $scope.myPromise = ReporteEstadisticosService.getExportarData(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (tipo == "1") {
                var file = new Blob([results.data], { type: 'application/xls' });
                saveAs(file, 'VENTAS DE ARTÍCULO_' + $scope.pCodSAP + '.xls');
            }
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
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
   
    $scope.btnConsultaClick = function () {
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

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
        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }


        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        $scope.resDgPedidos = [];
        $scope.myPromise = ReporteEstadisticosService.getConsReporteEsta(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
                    $scope.resDgPedidos = [];
                    $scope.Eliminar();
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {
                    $scope.resDgPedidos = results.data.root[0];
                    //$scope.pgcDgPedidos = $scope.resDgPedidos;

                    $scope.codigoImagen = results.data.root[1];
                    $scope.genaralData = results.data.root[0];
                    $scope.genaralCentro = results.data.root[2];
                    $scope.resDgPedidosDatos = results.data.root[3];


                    $scope.etiTotRegistros = $scope.resDgPedidos.length.toString();

                    if ($scope.selecAlmacenes.length <= 150 && $scope.chkArticuloSelModel.length <= 120) {
                        $scope.Eliminar();
                        $scope.CargarGrafico();
                        $scope.hideGrid = false;
                    } else {
                        $scope.Eliminar();
                        $scope.hideGrid = true;
                    }


                    $scope.showPaginate = true;

                }
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
        setTimeout(function () { $('#btnConsulta').focus(); }, 100);
        setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
    };

    $scope.Mantenimiento = function () {
        if ($scope.solicitud.opcion == 'I')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
                    cargaSolicitudes();
                    $scope.solicitud.opcion = 'I';
                }, function (error) {

                });


        if ($scope.solicitud.opcion == 'A')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo) + parseInt($scope.auxSaldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
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
        ReporteEstadisticosService.consSolicitud('C', $scope.pedido, $scope.articulo).then(function (results) {
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

}]);

app.controller('ReporteEstadisticosCentroAlmacenController', ['$scope', 'ReporteEstadisticosService','SeguridadService', '$filter', 'authService', function ($scope, ReporteEstadisticosService,SeguridadService, $filter, authService) {
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

    $scope.myPromise = SeguridadService.verificarTransaccion("4002").then(function (results) {
        if (results.data == false) {
            window.location = "../Notificacion/frmVisualizaNotificaciones";
        }
    }, function (error) {
    });

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



    $scope.grPedido = [];
    var _resDgPedidos = [];
    $scope.pageContentPed = [];
    $scope.pagesPed = [];
    $scope.totalGrid = {};

    $scope.resDgPedidostodoslosregistro = [];

    $scope.pgcDgPedidosDET = [];

    $scope.pOpcGrp1 = "P";
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

    $scope.listaAlmacenes = [];
    $scope.selecAlmacenes = [];
    $scope.SettingAlmacenes = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '300px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };


    $scope.chkArticuloList = [];
    $scope.chkArticuloSelModel = [];
    $scope.chkArticuloSettings = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];

    $scope.chbMeses = [
       { codigo: '01', descripcion: "Enero" },
       { codigo: '02', descripcion: "Febrero" },
       { codigo: '03', descripcion: "Marzo" },
       { codigo: '04', descripcion: "Abril" },
       { codigo: '05', descripcion: "Mayo" },
       { codigo: '06', descripcion: "Junio" },
       { codigo: '07', descripcion: "Julio" },
       { codigo: '08', descripcion: "Agosto" },
       { codigo: '09', descripcion: "Septiembre" },
       { codigo: '10', descripcion: "Octubre" },
       { codigo: '11', descripcion: "Noviembre" },
       { codigo: '12', descripcion: "Diciembre" }
    ];
    $scope.cboPaginSelItemMeses = $scope.chbMeses[0];
    var dateStringAnio = new Date();
    var dateStringAnioD = new Date();
    var dAnio = dateStringAnio.format("yyyy");

    dateStringAnioD.setDate(dateStringAnioD.getDate() + 365);
    var dAnioD = dateStringAnioD.format("yyyy");

    $scope.chbAnio = [];
    // { codigo: dAnio, descripcion: dAnio },
    // { codigo: dAnioD, descripcion: dAnioD }
    //];
    $scope.cboPaginSelItemAnio = "";
    $scope.cboPaginSelItem = $scope.cboPaginList[0];

    //------ combo estados

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

    $scope.codigoImagen = [];
    $scope.genaralData = [];
    $scope.genaralCentro = [];




    $scope.Eliminar = function () {
        $('#container').highcharts({
            title: { text: ' ' },
            series: []
        });

    }

    $scope.CargarGrafico = function () {

        $scope.Eliminar();
        var rndtotal = [];
        //rndtotal.name = "";
        //rndtotal.data = [];
        var cantidad = $scope.genaralCentro.length - 1;
        if (cantidad > 9) {
            cantidad = 9;
        }
        for (var i = 0; i < $scope.codigoImagen.length; i++) {
            var resumen = $filter('filter')($scope.genaralData, { articulo: $scope.codigoImagen[i].codMaterial }, true);
            var rnd = [];
            for (var j = 0; j < resumen.length; j++) {
                rnd.push({ name: resumen[j].nomAlmacen, y: parseInt(resumen[j].cantidad, 10) });// resumen[j].cantVendida });
            }
            rndtotal.push({ type: 'pie', name: $scope.codigoImagen[i].desMaterial, data: rnd })
        }


        $('#container').highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        },
                        connectorColor: 'silver',
                    },
                    showInLegend: true
                }
            },
            navigation: {
                buttonOptions: {
                    align: 'center'
                }
            },
            xAxis: {
                type: 'category', min: 0,
                max: cantidad
            },
            scrollbar: {
                enabled: true
            },
           legend: { layout: 'vertical', align: 'left', verticalAlign: 'middle', borderWidth: 0 },
           title: { text: 'Ventas por Almacén Periodo:' + $scope.cboPaginSelItemMeses.descripcion + ' - ' + $scope.cboPaginSelItemAnio.descripcion },
            scrollbar: {
                enabled: true,
                barBackgroundColor: 'gray',
                barBorderRadius: 7,
                barBorderWidth: 0,
                buttonBackgroundColor: 'gray',
                buttonBorderWidth: 0,
                buttonArrowColor: 'yellow',
                buttonBorderRadius: 7,
                rifleColor: 'yellow',
                trackBackgroundColor: 'white',
                trackBorderWidth: 1,
                trackBorderColor: 'silver',
                trackBorderRadius: 7
            },
            series: rndtotal
        });

    }


    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("1", $scope.pCodSAP).then(function (results) {
        if (results.data.success) {

            var listAlmacen = results.data.root[0];

            $scope.listaAlmacenes = listAlmacen;
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });


    $scope.myPromise = ReporteEstadisticosService.getAnios("3", $scope.pCodSAP).then(function (results) {
        if (results.data.success) {

            var lisanio = results.data.root[0];

            $scope.chbAnio = lisanio;
            $scope.cboPaginSelItemAnio = $scope.chbAnio[0];
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });

    


    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("2", $scope.pCodSAP).then(function (results) {

        if (results.data.success) {

            var listArticulo = results.data.root[0];

            $scope.chkArticuloList = listArticulo;
            //for (var i = 0; i < listArticulo.length; i++) {

            //    $scope.chkArticuloSelModel.push({ id: listArticulo[i].pCodAlmacen });

            //}


        }
        else {
            $scope.showMessage('E', 'Error al consultar Articulos: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesArticulosReportes().");
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
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

        //if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
        //    $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
        //    return;
        //}
        //if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
        //    $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
        //    return;
        //}


        //if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
        //    $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
        //    return;
        //}
        var date = new Date();
        var ultimoDia = new Date($scope.cboPaginSelItemAnio.codigo, $scope.cboPaginSelItemMeses.codigo, 0);

        var dateStringDia = new Date();
        $scope.pFechaIni = '01/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;
        $scope.pFechaFin = ultimoDia.getDate() + '/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;

        Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }
        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        cabeceraDatos.p_usuario = $scope.pUsuario;
        cabeceraDatos.nomreporte = "VENTAS POR ALMACÉN";
        $scope.resDgPedidos = [];



        if ($scope.etiTotRegistros == '' || $scope.etiTotRegistros == '0') {
            $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
            return;
        }

        $scope.myPromise = ReporteEstadisticosService.getExportarData(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (tipo == "1") {
                var file = new Blob([results.data], { type: 'application/xls' });
                saveAs(file, 'VENTAS POR ALMACÉN_' + $scope.pCodSAP + '.xls');
            }
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
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
      $scope.btnConsultaClick = function () {
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

       
        var date = new Date();
        var ultimoDia = new Date($scope.cboPaginSelItemAnio.codigo, $scope.cboPaginSelItemMeses.codigo, 0);

        var dateStringDia = new Date();
        $scope.pFechaIni = '01/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;
        $scope.pFechaFin = ultimoDia.getDate() + '/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;


        Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }


        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        $scope.resDgPedidos = [];
        $scope.myPromise = ReporteEstadisticosService.getConsReporteEstaCentroAlmacen(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
                    $scope.resDgPedidos = [];
                    $scope.Eliminar();
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {
                    $scope.resDgPedidos = results.data.root[0];

                    $scope.codigoImagen = results.data.root[1];
                    $scope.genaralData = results.data.root[3];
                    $scope.pgbaseArticulos = results.data.root[2];


                    $scope.etiTotRegistros = $scope.resDgPedidos.length.toString();

                    if ($scope.selecAlmacenes.length <= 150 && $scope.chkArticuloSelModel.length <= 120) {
                        $scope.Eliminar();
                        $scope.CargarGrafico();
                        $scope.hideGrid = false;
                    } else {
                        $scope.Eliminar();
                        $scope.hideGrid = true;
                    }


                    $scope.showPaginate = true;

                }
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
        setTimeout(function () { $('#btnConsulta').focus(); }, 100);
        setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
    };

    $scope.Mantenimiento = function () {
        if ($scope.solicitud.opcion == 'I')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
                    cargaSolicitudes();
                    $scope.solicitud.opcion = 'I';
                }, function (error) {

                });


        if ($scope.solicitud.opcion == 'A')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo) + parseInt($scope.auxSaldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
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
        ReporteEstadisticosService.consSolicitud('C', $scope.pedido, $scope.articulo).then(function (results) {
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
  
}]);

app.controller('ReporteEstadisticosArticuloController', ['$scope', 'ReporteEstadisticosService', 'SeguridadService', '$filter', 'authService', function ($scope, ReporteEstadisticosService, SeguridadService, $filter, authService) {
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

    $scope.myPromise = SeguridadService.verificarTransaccion("4003").then(function (results) {
        if (results.data == false) {
            window.location = "../Notificacion/frmVisualizaNotificaciones";
        }
    }, function (error) {
    });

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



    $scope.grPedido = [];
    var _resDgPedidos = [];
    $scope.pageContentPed = [];
    $scope.pagesPed = [];
    $scope.totalGrid = {};

    $scope.resDgPedidostodoslosregistro = [];

    $scope.pgcDgPedidosDET = [];

    $scope.pOpcGrp1 = "P";
    $scope.pOpcGrp2 = "N";


    var dateString1 = new Date();
    var dateString = new Date();

    var d1 = dateString1.format("dd/mm/yyyy");

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");

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

    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;

    $scope.listaAlmacenes = [];
    $scope.selecAlmacenes = [];
    $scope.SettingAlmacenes = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '300px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };


    $scope.chkArticuloList = [];
    $scope.chkArticuloSelModel = [];
    $scope.chkArticuloSettings = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];
    $scope.cboPaginSelItem = $scope.cboPaginList[0];

    //------ combo estados

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

    $scope.codigoImagen = [];
    $scope.genaralData = [];
    $scope.genaralCentro = [];




    $scope.Eliminar = function () {
        $('#container').highcharts({
            title: { text: ' ' },
            series: []
        });

    }

    $scope.CargarGrafico = function () {

        $scope.Eliminar();
        var rndtotal = [];
        //rndtotal.name = "";
        //rndtotal.data = [];
        var cantidad = $scope.genaralCentro.length - 1;
        if (cantidad > 9) {
            cantidad = 9;
        }
        //for (var i = 0; i < $scope.codigoImagen.length; i++) {
        //    var resumen = $filter('filter')($scope.genaralData, { codMaterial: $scope.codigoImagen[i].codMaterial }, true);
        //    var rnd = [];
        //    for (var j = 0; j < resumen.length; j++) {
        //        rnd.push({ name: resumen[j].mes, y: parseInt(resumen[j].cantVendida, 10) });// resumen[j].cantVendida });
        //    }
        //    rndtotal.push({ type: 'column', name: $scope.codigoImagen[i].desMaterial, data: rnd })
        //}

        var categorias = [];
        var data = [];

        for (var i = 0; i < $scope.genaralCentro.length; i++) {
            categorias.push($scope.genaralCentro[i].centro);
        }
        for (var i = 0; i < $scope.codigoImagen.length; i++) {
            var resumen = $filter('filter')($scope.resDgPedidos, { codMaterial: $scope.codigoImagen[i].codMaterial }, true);

            var auxData = {};

            auxData.name = resumen[0].desMaterial;
            auxData.data = [];
            for (var j = 0; j < $scope.genaralCentro.length; j++) {
                var aux = $filter('filter')(resumen, { mes: $scope.genaralCentro[j].centro }, true);

                if (aux.length > 0)
                    auxData.data.push(parseInt(aux[0].cantVendida));
                else
                    if (aux.length == 0)
                        auxData.data.push(0);
            }
            data.push(auxData);
        }

       
        $('#container').highcharts({
            chart: {
                type: 'column',
                borderWidth: 1,
                zoomType: 'xy'
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    dataLabels: {
                        enabled: true,
                        style: { fontSize: '8px', fontFamily: 'Verdana, sans-serif' },
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'black'
                    }
                }
            },
           navigation: {
                buttonOptions: {
                    align: 'center'
                }
            },
            yAxis: [{ title: { text: "Unidades Vendidas por Mes" } }], stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            },
            xAxis: {
                min: 0,
                max: cantidad,
                categories: categorias
            },
            scrollbar: {
                enabled: true
            },
            title: { text: 'Ventas por Artículo Periodo:' + $scope.pFechaIni + ' - ' + $scope.pFechaFin },
            scrollbar: {
                enabled: true,
                barBackgroundColor: 'gray',
                barBorderRadius: 7,
                barBorderWidth: 0,
                buttonBackgroundColor: 'gray',
                buttonBorderWidth: 0,
                buttonArrowColor: 'yellow',
                buttonBorderRadius: 7,
                rifleColor: 'yellow',
                trackBackgroundColor: 'white',
                trackBorderWidth: 1,
                trackBorderColor: 'silver',
                trackBorderRadius: 7
            },
            series: data
        });

    }


    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("1", $scope.pCodSAP).then(function (results) {
        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            $scope.listaAlmacenes = listAlmacen;
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });

    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("2", $scope.pCodSAP).then(function (results) {

        if (results.data.success) {
            var listArticulo = results.data.root[0];
            $scope.chkArticuloList = listArticulo;
        }
        else {
            $scope.showMessage('E', 'Error al consultar Articulos: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesArticulosReportes().");
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
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

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
        //if ($scope.selecAlmacenes.length == 0) {
        //    $scope.showMessage('I', 'Seleccione al menos un almacén.');
        //    return;
        //}

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }
        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        cabeceraDatos.p_usuario = $scope.pUsuario;
        cabeceraDatos.nomreporte = "VENTAS EN UNIDADES TOTALIZADO";
        $scope.resDgPedidos = [];



        if ($scope.etiTotRegistros == '' || $scope.etiTotRegistros == '0') {
            $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
            return;
        }
        $scope.myPromise = ReporteEstadisticosService.getExportarDataArticulo(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (tipo == "1") {
                var file = new Blob([results.data], { type: 'application/xls' });
                saveAs(file, 'VENTAS EN UNIDADES TOTALIZADO_' + $scope.pCodSAP + '.xls');
            }
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
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

    $scope.btnConsultaClick = function () {
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

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
        //if ($scope.selecAlmacenes.length == 0) {
        //    $scope.showMessage('I', 'Seleccione al menos un almacén.');
        //    return;
        //}

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }


        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        $scope.resDgPedidos = [];
        $scope.myPromise = ReporteEstadisticosService.getconsReporteConsultaArticulo(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
                    $scope.resDgPedidos = [];
                    $scope.Eliminar();
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {
                    $scope.resDgPedidos = results.data.root[0];
                    //$scope.pgcDgPedidos = $scope.resDgPedidos;

                    $scope.codigoImagen = results.data.root[1];
                    $scope.genaralData = results.data.root[0];
                    $scope.genaralCentro = results.data.root[2];


                    $scope.etiTotRegistros = $scope.resDgPedidos.length.toString();

                    if ($scope.selecAlmacenes.length <= 150 && $scope.chkArticuloSelModel.length <= 120) {
                        $scope.Eliminar();
                        $scope.CargarGrafico();
                        $scope.hideGrid = false;
                    } else {
                        $scope.Eliminar();
                        $scope.hideGrid = true;
                    }


                    $scope.showPaginate = true;

                }
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
        setTimeout(function () { $('#btnConsulta').focus(); }, 100);
        setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
    };

    $scope.Mantenimiento = function () {
        if ($scope.solicitud.opcion == 'I')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
                    cargaSolicitudes();
                    $scope.solicitud.opcion = 'I';
                }, function (error) {

                });


        if ($scope.solicitud.opcion == 'A')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo) + parseInt($scope.auxSaldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
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
        ReporteEstadisticosService.consSolicitud('C', $scope.pedido, $scope.articulo).then(function (results) {
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

}]);

app.controller('ReporteEvolucionController', ['$scope', 'ReporteEstadisticosService',  'SeguridadService','$filter', 'authService', function ($scope, ReporteEstadisticosService,SeguridadService, $filter, authService) {
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

    $scope.myPromise = SeguridadService.verificarTransaccion("4004").then(function (results) {
        if (results.data == false) {
            window.location = "../Notificacion/frmVisualizaNotificaciones";
        }
    }, function (error) {
    });

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



    $scope.grPedido = [];
    var _resDgPedidos = [];
    $scope.pageContentPed = [];
    $scope.pagesPed = [];
    $scope.totalGrid = {};

    $scope.resDgPedidostodoslosregistro = [];

    $scope.pgcDgPedidosDET = [];

    $scope.pOpcGrp1 = "P";
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

    $scope.listaAlmacenes = [];
    $scope.selecAlmacen = {};
    $scope.SettingAlmacenes = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '300px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk', selectionLimit: 1 };


    $scope.chkArticuloList = [];
    $scope.chkArticuloSelModel = [];
    $scope.chkArticuloSettings = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '300px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];
    $scope.cboPaginSelItem = $scope.cboPaginList[0];

    //------ combo estados

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

    $scope.Fechas = [];
    $scope.genaralData = [];
    $scope.genaralCentro = [];
    $scope.genaralArticulo = [];




    $scope.Eliminar = function () {
        $('#container').highcharts({
            title: { text: ' ' },
            series: []
        });

    }

    $scope.CargarGrafico = function () {

        $scope.Eliminar();
        var rndtotal = [];

        var datos = [];

        for (var i = 0; i < $scope.genaralArticulo.length; i++) {
            var resumen = $filter('filter')($scope.genaralData, { codMaterial: $scope.genaralArticulo[i].codArticulo }, true);
            debugger;
            var rnd = [];
            var aux = {};
            aux.name = resumen[0].desMaterial;
            for (var j = 0; j < $scope.Fechas.length; j++) {

                var aux1 = $filter('filter')(resumen, { fecha: $scope.Fechas[j] }, true);

                if (aux1.length > 0)
                    rnd.push(parseInt(aux1[0].cantVendida, 10));
                else
                    if (aux1.length == 0)
                        rnd.push(0);
            }
            aux.data = rnd;
            datos.push(aux);
        }




        $('#container').highcharts({
            title: {
                text: 'Periodo: ' + $scope.pFechaIni + ' - ' + $scope.pFechaFin,
                x: -20 //center
            },
            subtitle: {
                text: '',
                x: -20
            },
            xAxis: {
                categories: $scope.Fechas,
                title: {
                    text: $scope.genaralCentro[0].centro
                }
            },
            yAxis: {
                title: {
                    text: 'Cantidad Vendida'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                valueSuffix: ' unidades'
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            series: datos
        });

    }


    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("1", $scope.pCodSAP).then(function (results) {
        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            $scope.listaAlmacenes = listAlmacen;
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });

    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("2", $scope.pCodSAP).then(function (results) {

        if (results.data.success) {
            var listArticulo = results.data.root[0];
            $scope.chkArticuloList = listArticulo;
        }
        else {
            $scope.showMessage('E', 'Error al consultar Articulos: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesArticulosReportes().");
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
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

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
        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }
        if (!angular.isUndefined($scope.selecAlmacen.pCodAlmacen))
            $scope.selecAlmacenes.push($scope.selecAlmacen);
        else {
            $scope.showMessage('I', 'Seleccione un almacén.');
            return;
        }


        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].pCodAlmacen;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }
        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        cabeceraDatos.p_usuario = $scope.pUsuario;
        cabeceraDatos.nomreporte = "EVOLUCIÓN DE VENTAS ARTÍCULO / ALMACÉN";
        $scope.resDgPedidos = [];



        if ($scope.etiTotRegistros == '' || $scope.etiTotRegistros == '0') {
            $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
            return;
        }

        $scope.myPromise = ReporteEstadisticosService.getExportarData(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (tipo == "1") {
                var file = new Blob([results.data], { type: 'application/xls' });
                saveAs(file, 'EVOLUCION DE VENTAS DE ARTÍCULO_ALMACÉN_' + $scope.pCodSAP + '.xls');
            }
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
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
        $scope.selecAlmacenes = [];

        if (!angular.isUndefined($scope.selecAlmacen))
            $scope.selecAlmacenes.push($scope.selecAlmacen);

        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

        if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
            $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
            return;
        }
        if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
            $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
            return;
        }
        if (!angular.isUndefined($scope.selecAlmacen.pCodAlmacen))
            $scope.selecAlmacenes.push($scope.selecAlmacen);
        else {
            $scope.showMessage('I', 'Seleccione un almacén.');
            return;
        }


        if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
            $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
            return;
        }

        Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].pCodAlmacen;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }


        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        $scope.resDgPedidos = [];
        $scope.myPromise = ReporteEstadisticosService.getConsReporteEvolucion(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
                    $scope.resDgPedidos = [];
                    $scope.Eliminar();
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {
                    debugger;
                    $scope.resDgPedidos = results.data.root[0];
                    //$scope.pgcDgPedidos = $scope.resDgPedidos;
                    $scope.Fechas = results.data.root[1];
                    $scope.genaralData = results.data.root[0];
                    $scope.genaralCentro = results.data.root[2];
                    $scope.genaralArticulo = results.data.root[3];


                    $scope.etiTotRegistros = $scope.resDgPedidos.length.toString();

                    if ($scope.selecAlmacenes.length <= 150 && $scope.chkArticuloSelModel.length <= 120) {
                        $scope.Eliminar();
                        $scope.CargarGrafico();
                        $scope.hideGrid = false;
                    } else {
                        $scope.Eliminar();
                        $scope.hideGrid = true;
                    }


                    $scope.showPaginate = true;

                }
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
        setTimeout(function () { $('#btnConsulta').focus(); }, 100);
        setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
    };

    $scope.Mantenimiento = function () {
        if ($scope.solicitud.opcion == 'I')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
                    cargaSolicitudes();
                    $scope.solicitud.opcion = 'I';
                }, function (error) {

                });


        if ($scope.solicitud.opcion == 'A')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo) + parseInt($scope.auxSaldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
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
        ReporteEstadisticosService.consSolicitud('C', $scope.pedido, $scope.articulo).then(function (results) {
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
        $scope.myPromise = ReporteEstadisticosService.getBuscarAsignacion("AP", authService.authentication.CodSAP).then(function (results) {
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
    // $scope.CargaInicial();
}]);

app.controller('ReporteVentaProvMensualController', ['$scope', 'ReporteEstadisticosService','SeguridadService', '$filter', 'authService', function ($scope, ReporteEstadisticosService,SeguridadService, $filter, authService) {
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



    $scope.grPedido = [];
    var _resDgPedidos = [];
    $scope.pageContentPed = [];
    $scope.pagesPed = [];
    $scope.totalGrid = {};

    $scope.resDgPedidostodoslosregistro = [];

    $scope.pgcDgPedidosDET = [];

    $scope.pOpcGrp1 = "P";
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


    $scope.myPromise = SeguridadService.verificarTransaccion("4005").then(function (results) {
        if (results.data == false) {
            window.location = "../Notificacion/frmVisualizaNotificaciones";
        }
    }, function (error) {
    });

    $scope.listaAlmacenes = [];
    $scope.selecAlmacenes = [];
    $scope.SettingAlmacenes = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '300px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };


    $scope.chkArticuloList = [];
    $scope.chkArticuloSelModel = [];
    $scope.chkArticuloSettings = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];

    $scope.chbMeses = [
       { codigo: '01', descripcion: "Enero" },
       { codigo: '02', descripcion: "Febrero" },
       { codigo: '03', descripcion: "Marzo" },
       { codigo: '04', descripcion: "Abril" },
       { codigo: '05', descripcion: "Mayo" },
       { codigo: '06', descripcion: "Junio" },
       { codigo: '07', descripcion: "Julio" },
       { codigo: '08', descripcion: "Agosto" },
       { codigo: '09', descripcion: "Septiembre" },
       { codigo: '10', descripcion: "Octubre" },
       { codigo: '11', descripcion: "Noviembre" },
       { codigo: '12', descripcion: "Diciembre" }
    ];
    $scope.cboPaginSelItemMeses = $scope.chbMeses[0];
    var dateStringAnio = new Date();
    var dateStringAnioD = new Date();
    var dAnio = dateStringAnio.format("yyyy");

    dateStringAnioD.setDate(dateStringAnioD.getDate() + 365);
    var dAnioD = dateStringAnioD.format("yyyy");

    $scope.chbAnio = [];
    //// { codigo: dAnio, descripcion: dAnio },
    //// { codigo: dAnioD, descripcion: dAnioD }
    ////];
    $scope.cboPaginSelItemAnio = "";
    $scope.cboPaginSelItem = $scope.cboPaginList[0];

    //------ combo estados

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

    $scope.codigoImagen = [];
    $scope.genaralData = [];
    $scope.genaralCentro = [];




    $scope.Eliminar = function () {
        $('#container').highcharts({
            title: { text: ' ' },
            series: []
        });

    }

    $scope.CargarGrafico = function () {

        $scope.Eliminar();
        var rndtotal = [];

        var cantidad = $scope.genaralCentro.length - 1;
        if (cantidad > 9) {
            cantidad = 9;
        }
        var categorias = [];
        var data = [];

        for (var i = 0; i < $scope.genaralData.length; i++) {
            categorias.push($scope.genaralData[i].nomAlmacen);
        }

        for (var i = 0; i < $scope.pgbaseArticulos.length; i++) {
            var resumen = $filter('filter')($scope.resDgPedidos, { codMaterial: $scope.pgbaseArticulos[i].codArticulo }, true);

            var auxData = {};

            auxData.name = resumen[0].desMaterial;
            auxData.data = [];
            for (var j = 0; j < $scope.genaralData.length; j++) {
                var aux = $filter('filter')(resumen, { codCentro: $scope.genaralData[j].codAlmacen }, true);

                if (aux.length > 0)
                    auxData.data.push(parseInt(aux[0].cantVendida));
                else
                    if (aux.length == 0)
                        auxData.data.push(0);
            }
            data.push(auxData);
        }
        $('#container').highcharts({
            chart: {
                type: 'bar'
            },
            title: {
                text: 'Periodo: ' + $scope.cboPaginSelItemMeses.descripcion + ' ' + $scope.cboPaginSelItemAnio.descripcion
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: categorias,
                title: {
                    text: null
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Ventas (unidades)',
                    align: 'high'
                },
                labels: {
                    overflow: 'justify'
                }
            },
            tooltip: {
                valueSuffix: ' unidades'
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            legend: {
                //layout: 'vertical',
                //align: 'right',
                //verticalAlign: 'top',
                //x: -40,
                //y: 80,
                //floating: true,
                //borderWidth: 1,
                backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                shadow: true
            },
            credits: {
                enabled: false
            },
            series: data
        });

    }


    $scope.myPromise = ReporteEstadisticosService.getAnios("3", $scope.pCodSAP).then(function (results) {
        if (results.data.success) {

            var lisanio = results.data.root[0];

            $scope.chbAnio = lisanio;
            $scope.cboPaginSelItemAnio = $scope.chbAnio[0];
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });


    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("1", $scope.pCodSAP).then(function (results) {
        if (results.data.success) {

            var listAlmacen = results.data.root[0];

            $scope.listaAlmacenes = listAlmacen;
            //for (var i = 0; i < listAlmacen.length; i++) {

            //    $scope.selecAlmacenes.push({ id: listAlmacen[i].pCodAlmacen });

            //}


        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });

    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("2", $scope.pCodSAP).then(function (results) {

        if (results.data.success) {

            var listArticulo = results.data.root[0];

            $scope.chkArticuloList = listArticulo;
            //for (var i = 0; i < listArticulo.length; i++) {

            //    $scope.chkArticuloSelModel.push({ id: listArticulo[i].pCodAlmacen });

            //}


        }
        else {
            $scope.showMessage('E', 'Error al consultar Articulos: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesArticulosReportes().");
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
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

        //if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
        //    $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
        //    return;
        //}
        //if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
        //    $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
        //    return;
        //}


        //if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
        //    $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
        //    return;
        //}
        var date = new Date();
        var ultimoDia = new Date(date.getFullYear(), date.getMonth() + 1, 0);

        var dateStringDia = new Date();
        $scope.pFechaIni = '01/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;
        $scope.pFechaFin = ultimoDia.getDate() + '/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;

        Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }
        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        cabeceraDatos.p_usuario = $scope.pUsuario;
        cabeceraDatos.nomreporte = "VENTAS POR MES EN UNIDADES";
        $scope.resDgPedidos = [];




        if ($scope.etiTotRegistros == '' || $scope.etiTotRegistros == '0') {
            $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
            return;
        }

        $scope.myPromise = ReporteEstadisticosService.getExportarDataProvMensual(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (tipo == "1") {
                var file = new Blob([results.data], { type: 'application/xls' });
                saveAs(file, 'VENTAS POR MES EN UNIDADES_' + $scope.pCodSAP + '.xls');
            }
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
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

        //if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
        //    $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
        //    return;
        //}
        //if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
        //    $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
        //    return;
        //}


        //if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
        //    $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
        //    return;
        //}
        var date = new Date();
        var ultimoDia = new Date(date.getFullYear(), date.getMonth() + 1, 0);

        var dateStringDia = new Date();
        $scope.pFechaIni = '01/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;
        $scope.pFechaFin = ultimoDia.getDate() + '/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;


        Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');
        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }


        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        $scope.resDgPedidos = [];
        $scope.myPromise = ReporteEstadisticosService.getConsReporteVentaProvMensual(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
                    $scope.resDgPedidos = [];
                    $scope.Eliminar();
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {
                    $scope.resDgPedidos = results.data.root[0];
                    //$scope.pgcDgPedidos = $scope.resDgPedidos;

                    $scope.codigoImagen = results.data.root[1];
                    $scope.genaralData = results.data.root[3];
                    $scope.pgbaseArticulos = results.data.root[2];




                    $scope.etiTotRegistros = $scope.resDgPedidos.length.toString();

                    if ($scope.selecAlmacenes.length <= 150 && $scope.chkArticuloSelModel.length <= 120) {
                        $scope.Eliminar();
                        $scope.CargarGrafico();
                        $scope.hideGrid = false;
                    } else {
                        $scope.Eliminar();
                        $scope.hideGrid = true;
                    }


                    $scope.showPaginate = true;

                }
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
        setTimeout(function () { $('#btnConsulta').focus(); }, 100);
        setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
    };

    $scope.Mantenimiento = function () {
        if ($scope.solicitud.opcion == 'I')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
                    cargaSolicitudes();
                    $scope.solicitud.opcion = 'I';
                }, function (error) {

                });


        if ($scope.solicitud.opcion == 'A')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo) + parseInt($scope.auxSaldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
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
        ReporteEstadisticosService.consSolicitud('C', $scope.pedido, $scope.articulo).then(function (results) {
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
        $scope.myPromise = ReporteEstadisticosService.getBuscarAsignacion("AP", authService.authentication.CodSAP).then(function (results) {
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
    // $scope.CargaInicial();
}]);

app.controller('ReporteEstadisticosStockAlmacenController', ['$scope', 'ReporteEstadisticosService', 'SeguridadService', '$filter', 'authService', function ($scope, ReporteEstadisticosService, SeguridadService, $filter, authService) {
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

    $scope.myPromise = SeguridadService.verificarTransaccion("4006").then(function (results) {
        if (results.data == false) {
            window.location = "../Notificacion/frmVisualizaNotificaciones";
        }
    }, function (error) {
        debugger;
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });

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



    $scope.grPedido = [];
    var _resDgPedidos = [];
    $scope.pageContentPed = [];
    $scope.pagesPed = [];
    $scope.totalGrid = {};

    $scope.resDgPedidostodoslosregistro = [];

    $scope.pgcDgPedidosDET = [];

    $scope.pOpcGrp1 = "P";
    $scope.pOpcGrp2 = "N";


    var dateString1 = new Date();
    var dateString = new Date();

    var d1 = dateString1.format("dd/mm/yyyy");

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");



    //$scope.pFechaIni = "";
    //$scope.pFechaFin = "";


    $scope.pFechaIni = d1;
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

    $scope.listaAlmacenes = [];
    $scope.selecAlmacenes = [];
    $scope.SettingAlmacenes = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '300px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };


    $scope.chkArticuloList = [];
    $scope.chkArticuloSelModel = [];
    $scope.chkArticuloSettings = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];

    $scope.chbMeses = [
       { codigo: '01', descripcion: "Enero" },
       { codigo: '02', descripcion: "Febrero" },
       { codigo: '03', descripcion: "Marzo" },
       { codigo: '04', descripcion: "Abril" },
       { codigo: '05', descripcion: "Mayo" },
       { codigo: '06', descripcion: "Junio" },
       { codigo: '07', descripcion: "Julio" },
       { codigo: '08', descripcion: "Agosto" },
       { codigo: '09', descripcion: "Septiembre" },
       { codigo: '10', descripcion: "Octubre" },
       { codigo: '11', descripcion: "Noviembre" },
       { codigo: '12', descripcion: "Diciembre" }
    ];
    $scope.cboPaginSelItemMeses = $scope.chbMeses[0];
    var dateStringAnio = new Date();
    var dateStringAnioD = new Date();
    var dAnio = dateStringAnio.format("yyyy");

    dateStringAnioD.setDate(dateStringAnioD.getDate() + 365);
    var dAnioD = dateStringAnioD.format("yyyy");

    $scope.chbAnio = [];
    // { codigo: dAnio, descripcion: dAnio },
    // { codigo: dAnioD, descripcion: dAnioD }
    //];
    $scope.cboPaginSelItemAnio = "";
    $scope.cboPaginSelItem = $scope.cboPaginList[0];

    //------ combo estados

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

    $scope.codigoImagen = [];
    $scope.genaralData = [];
    $scope.genaralCentro = [];




    $scope.Eliminar = function () {
        $('#container').highcharts({
            title: { text: ' ' },
            series: []
        });

    }

    $scope.CargarGrafico = function () {
        debugger;
        $scope.Eliminar();
        var rndtotal = [];
        //rndtotal.name = "";
        //rndtotal.data = [];
        var cantidad = $scope.genaralCentro.length - 1;
        cantidad = (cantidad<0)?cantidad=0:cantidad=cantidad;
        if (cantidad > 9) {
            cantidad = 9;
        }
        for (var i = 0; i < $scope.codigoImagen.length; i++) {
            var resumen = $filter('filter')($scope.genaralData, { articulo: $scope.codigoImagen[i].codMaterial }, true);
            var rnd = [];
            for (var j = 0; j < resumen.length; j++) {
                rnd.push({ name: resumen[j].nomAlmacen, y: parseInt(resumen[j].cantidad, 10) });// resumen[j].cantVendida });
            }
            rndtotal.push({ type: 'pie', name: $scope.codigoImagen[i].desMaterial, data: rnd })
        }


        $('#container').highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        },
                        connectorColor: 'silver',
                    },
                    showInLegend: true
                }
            },
            navigation: {
                buttonOptions: {
                    align: 'center'
                }
            },
            xAxis: {
                type: 'category', min: 0,
                max: cantidad
            },
            scrollbar: {
                enabled: true
            },
            legend: { layout: 'vertical', align: 'left', verticalAlign: 'middle', borderWidth: 0 },
            title: { text: 'Stock por Almacén Fecha:' + $scope.pFechaIni },
            scrollbar: {
                enabled: true,
                barBackgroundColor: 'gray',
                barBorderRadius: 7,
                barBorderWidth: 0,
                buttonBackgroundColor: 'gray',
                buttonBorderWidth: 0,
                buttonArrowColor: 'yellow',
                buttonBorderRadius: 7,
                rifleColor: 'yellow',
                trackBackgroundColor: 'white',
                trackBorderWidth: 1,
                trackBorderColor: 'silver',
                trackBorderRadius: 7
            },
            series: rndtotal
        });

        setTimeout(function () { $('#btnConsulta').focus(); }, 200);
        setTimeout(function () { $('#btnConsultaexecel').focus(); }, 250);
    }


    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("1", $scope.pCodSAP).then(function (results) {
        if (results.data.success) {

            var listAlmacen = results.data.root[0];

            $scope.listaAlmacenes = listAlmacen;
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });


    //$scope.myPromise = ReporteEstadisticosService.getAnios("3", $scope.pCodSAP).then(function (results) {
    //    if (results.data.success) {

    //        var lisanio = results.data.root[0];

    //        $scope.chbAnio = lisanio;
    //        $scope.cboPaginSelItemAnio = $scope.chbAnio[0];
    //    }
    //    else {
    //        $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
    //    }
    //}, function (error) {
    //    $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    //});
   

    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("2", $scope.pCodSAP).then(function (results) {

        if (results.data.success) {

            var listArticulo = results.data.root[0];

            $scope.chkArticuloList = listArticulo;
            //for (var i = 0; i < listArticulo.length; i++) {

            //    $scope.chkArticuloSelModel.push({ id: listArticulo[i].pCodAlmacen });

            //}


        }
        else {
            $scope.showMessage('E', 'Error al consultar Articulos: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesArticulosReportes().");
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
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

        //if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
        //    $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
        //    return;
        //}
        //if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
        //    $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
        //    return;
        //}


        //if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
        //    $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
        //    return;
        //}

        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
      
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            var tmp = $filter('filter')($scope.listaAlmacenes, { pCodAlmacen: $scope.selecAlmacenes[ix].id }, true);
            list.descripcion = tmp[0].pNomAlmacen
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            var tmp = $filter('filter')($scope.chkArticuloList, { pCodAlmacen: $scope.chkArticuloSelModel[ix].id }, true);
            list.descripcion = tmp[0].pNomAlmacen
            listaEnviarArt.push(list);
        }
        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = $scope.pFechaIni;
        cabeceraDatos.Fecha2 = $scope.pFechaIni;
        cabeceraDatos.p_usuario = $scope.pUsuario;
        cabeceraDatos.nomreporte = "STOCK POR ALMACÉN";
        $scope.resDgPedidos = [];



        if ($scope.etiTotRegistros == '' || $scope.etiTotRegistros == '0') {
            $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
            return;
        }

        $scope.myPromise = ReporteEstadisticosService.getexportarReporteStockAlmacena(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (tipo == "1") {
                var file = new Blob([results.data], { type: 'application/xls' });
                saveAs(file, 'STOCK POR ALMACÉN_' + $scope.pCodSAP + '.xls');
            }
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
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
    $scope.btnConsultaClick = function () {
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";


      


        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }
        
        

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            var tmp = $filter('filter')($scope.listaAlmacenes, { pCodAlmacen: $scope.selecAlmacenes[ix].id }, true);
            list.descripcion = tmp[0].pNomAlmacen
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            var tmp = $filter('filter')($scope.chkArticuloList, { pCodAlmacen: $scope.chkArticuloSelModel[ix].id }, true);
            list.descripcion = tmp[0].pNomAlmacen
            listaEnviarArt.push(list);
        }


        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = $scope.pFechaIni;
        cabeceraDatos.Fecha2 = $scope.pFechaIni;
        $scope.resDgPedidos = [];
        $scope.myPromise = ReporteEstadisticosService.getconsultaStockTienda(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
                    $scope.resDgPedidos = [];
                    $scope.Eliminar();
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {
                    $scope.resDgPedidos = results.data.root[0];

                    $scope.codigoImagen = results.data.root[1];
                    $scope.genaralData = results.data.root[3];
                    $scope.pgbaseArticulos = results.data.root[2];


                    $scope.etiTotRegistros = $scope.resDgPedidos.length.toString();

                    if ($scope.selecAlmacenes.length <= 150 && $scope.chkArticuloSelModel.length <= 120) {
                        $scope.Eliminar();
                        $scope.CargarGrafico();
                        $scope.hideGrid = false;
                    } else {
                        $scope.Eliminar();
                        $scope.hideGrid = true;
                    }


                    $scope.showPaginate = true;

                }
            }
            else {
                $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
            }
            setTimeout(function () { $('#btnConsulta').focus(); }, 200);
            setTimeout(function () { $('#btnConsultaexecel').focus(); }, 250);
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
        setTimeout(function () { $('#btnConsulta').focus(); }, 200);
        setTimeout(function () { $('#btnConsultaexecel').focus(); }, 250);
    };

    $scope.Mantenimiento = function () {
        if ($scope.solicitud.opcion == 'I')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
                    cargaSolicitudes();
                    $scope.solicitud.opcion = 'I';
                }, function (error) {

                });


        if ($scope.solicitud.opcion == 'A')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo) + parseInt($scope.auxSaldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
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
        ReporteEstadisticosService.consSolicitud('C', $scope.pedido, $scope.articulo).then(function (results) {
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

}]);

app.controller('ReporteEstadisticosStockAlmacenArticulosController', ['$scope', 'ReporteEstadisticosService', 'SeguridadService', '$filter', 'authService', function ($scope, ReporteEstadisticosService, SeguridadService, $filter, authService) {
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
    $scope.resDgPedidosDatos = [];
    var _resDgPedidos = [];
    $scope.pagesPedidos = [];
    $scope.pgcDgPedidos = [];



    $scope.grPedido = [];
    var _resDgPedidos = [];
    $scope.pageContentPed = [];
    $scope.pagesPed = [];
    $scope.totalGrid = {};

    $scope.resDgPedidostodoslosregistro = [];

    $scope.pgcDgPedidosDET = [];

    $scope.pOpcGrp1 = "P";
    $scope.pOpcGrp2 = "N";


    var dateString1 = new Date();
    var dateString = new Date();

    var d1 = dateString1.format("dd/mm/yyyy");

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");

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

    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;

    $scope.listaAlmacenes = [];
    $scope.selecAlmacenes = [];
    $scope.SettingAlmacenes = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '300px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };
    //$scope.SettingAlmacenes = { selectionLimit: 1 };


    $scope.chkArticuloList = [];
    $scope.chkArticuloSelModel = [];
    $scope.chkArticuloSettings = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];
    $scope.cboPaginSelItem = $scope.cboPaginList[0];

    //------ combo estados

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

    $scope.codigoImagen = [];
    $scope.genaralData = [];
    $scope.genaralCentro = [];




    $scope.Eliminar = function () {
        $('#container').highcharts({
            title: { text: ' ' },
            series: []
        });

    }
    $scope.myPromise = SeguridadService.verificarTransaccion("4007").then(function (results) {
        if (results.data == false) {
            window.location = "../Notificacion/frmVisualizaNotificaciones";
        }
    }, function (error) {
    });

    $scope.CargarGrafico = function () {

        $scope.Eliminar();
        var rndtotal = [];
        //rndtotal.name = "";
        //rndtotal.data = [];
        var cantidad = $scope.genaralCentro.length - 1;
        if (cantidad > 9) {
            cantidad = 9;
        }
        var categorias = [];
        var data = [];

        for (var i = 0; i < $scope.genaralCentro.length; i++) {
            categorias.push($scope.genaralCentro[i].almacen);
        }

        for (var i = 0; i < $scope.codigoImagen.length; i++) {
            var resumen = $filter('filter')($scope.resDgPedidosDatos, { codMaterial: $scope.codigoImagen[i].codMaterial }, true);

            var auxData = {};
            if (i==0) {
                auxData.type = 'column';
            }
            if (i == 1) {
                auxData.type = 'spline';
            }
            
            if (resumen.length>0) {
                auxData.name = resumen[0].desMaterial;
                auxData.data = [];
                for (var j = 0; j < $scope.genaralCentro.length; j++) {
                    var aux = $filter('filter')(resumen, { codCentro: $scope.genaralCentro[j].centro }, true);

                    if (aux.length > 0)
                        auxData.data.push(parseInt(aux[0].cantVendida));
                    else
                        if (aux.length == 0)
                            auxData.data.push(0);
                }
                data.push(auxData);
            }
          


            //for (var i = 0; i < $scope.codigoImagen.length; i++) {
            //    var resumen = $filter('filter')($scope.genaralData, { articulo: $scope.codigoImagen[i].codMaterial }, true);
            //    var rnd = [];
            //    for (var j = 0; j < resumen.length; j++) {
            //        rnd.push({ name: resumen[j].nomAlmacen, y: parseInt(resumen[j].cantidad, 10) });// resumen[j].cantVendida });
            //    }
            //    rndtotal.push({ type: 'pie', name: $scope.codigoImagen[i].desMaterial, data: rnd })
            //}


        }
        //for (var i = 0; i < $scope.codigoImagen.length; i++) {
        //    var resumen = $filter('filter')($scope.genaralData, { codMaterial: $scope.codigoImagen[i].codMaterial }, true);
        //    var rnd = [];
        //    for (var j = 0; j < resumen.length; j++) {
        //        rnd.push({ name: resumen[j].nomAlmacen, y: parseInt(resumen[j].cantVendida, 10) });// resumen[j].cantVendida });
        //    }
        //    rndtotal.push({ name: $scope.codigoImagen[i].desMaterial, data: rnd })
        //}

        $('#container').highcharts({
            //chart: {
            //    type: 'column',
            //    borderWidth: 1,
            //    zoomType: 'xy'
            //},
            plotOptions: {
                column: {
                    //stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        style: { fontSize: '8px', fontFamily: 'Verdana, sans-serif' },
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                }
            },
            navigation: {
                buttonOptions: {
                    align: 'center'
                }
            },
            yAxis: [{ title: { text: "Ventas Totales" } }], stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            },



            xAxis: {
                min: 0,
                max: cantidad,
                categories: categorias
            },
            scrollbar: {
                enabled: true
            },
            title: { text: 'Ventas vs Stock por Artículo Fecha desde: ' + $scope.pFechaIni + ' hasta: ' + $scope.pFechaFin },
            scrollbar: {
                enabled: true,
                barBackgroundColor: 'gray',
                barBorderRadius: 7,
                barBorderWidth: 0,
                buttonBackgroundColor: 'gray',
                buttonBorderWidth: 0,
                buttonArrowColor: 'yellow',
                buttonBorderRadius: 7,
                rifleColor: 'yellow',
                trackBackgroundColor: 'white',
                trackBorderWidth: 1,
                trackBorderColor: 'silver',
                trackBorderRadius: 7
            },
            series: data
        });

    }



    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("1", $scope.pCodSAP).then(function (results) {

        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            $scope.listaAlmacenes = listAlmacen;
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });

    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("2", $scope.pCodSAP).then(function (results) {

        if (results.data.success) {
            var listArticulo = results.data.root[0];
            $scope.chkArticuloList = listArticulo;
        }
        else {
            $scope.showMessage('E', 'Error al consultar Articulos: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesArticulosReportes().");
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
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

        if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
            $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
            return;
        }

   

        Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        Fecha2 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }
        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        cabeceraDatos.p_usuario = $scope.pUsuario;
        cabeceraDatos.nomreporte = "VENTAS VS STOCK POR ARTÍCULO";
        $scope.resDgPedidos = [];



        if ($scope.etiTotRegistros == '' || $scope.etiTotRegistros == '0') {
            $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
            return;
        }

        $scope.myPromise = ReporteEstadisticosService.getexportarReporteStockAlmacenaArt(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (tipo == "1") {
                var file = new Blob([results.data], { type: 'application/xls' });
                saveAs(file, 'STOCK ALMACÉN POR ARTÍCULO_' + $scope.pCodSAP + '.xls');
            }
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
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

    $scope.btnConsultaClick = function () {
        $scope.npage = 1;
        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

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
        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkArticuloSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un Artículo.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            var list = {};
            list.id = $scope.selecAlmacenes[ix].id;
            listaEnviarAlm.push(list);
        }
        var listaEnviarArt = [];
        for (var ix = 0 ; ix < $scope.chkArticuloSelModel.length; ix++) {
            var list = {};
            list.id = $scope.chkArticuloSelModel[ix].id;
            listaEnviarArt.push(list);
        }


        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        $scope.resDgPedidos = [];
        $scope.myPromise = ReporteEstadisticosService.getconsultaStockTiendaAlamcen(cabeceraDatos, listaEnviarAlm, listaEnviarArt).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
                    $scope.resDgPedidos = [];
                    $scope.Eliminar();
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {

                    debugger;
                    $scope.resDgPedidos = results.data.root[0];
                    //$scope.pgcDgPedidos = $scope.resDgPedidos;

                    $scope.codigoImagen = results.data.root[1];
                    $scope.genaralData = results.data.root[0];
                    $scope.genaralCentro = results.data.root[2];
                    $scope.resDgPedidosDatos = results.data.root[3];


                    $scope.etiTotRegistros = $scope.resDgPedidos.length.toString();

                    if ($scope.selecAlmacenes.length <= 150 && $scope.chkArticuloSelModel.length <= 120) {
                        $scope.Eliminar();
                        $scope.CargarGrafico();
                        $scope.hideGrid = false;
                    } else {
                        $scope.Eliminar();
                        $scope.hideGrid = true;
                    }


                    $scope.showPaginate = true;

                }
            }
            else {
                $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
                $scope.resDgPedidos = [];
                $scope.Eliminar();
                $scope.hideGrid = true;
                $scope.showPaginate = false;
                $scope.etiTotRegistros = '';
            }
            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
        setTimeout(function () { $('#btnConsulta').focus(); }, 100);
        setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
    };

    $scope.Mantenimiento = function () {
        if ($scope.solicitud.opcion == 'I')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
                    cargaSolicitudes();
                    $scope.solicitud.opcion = 'I';
                }, function (error) {

                });


        if ($scope.solicitud.opcion == 'A')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo) + parseInt($scope.auxSaldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
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
        ReporteEstadisticosService.consSolicitud('C', $scope.pedido, $scope.articulo).then(function (results) {
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

}]);

app.controller('ReporteMercadoController', ['$scope', 'ReporteEstadisticosService', 'SeguridadService', '$filter', 'authService', function ($scope, ReporteEstadisticosService, SeguridadService, $filter, authService) {
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

    $scope.myPromise = SeguridadService.verificarTransaccion("4004").then(function (results) {
        if (results.data == false) {
            window.location = "../Notificacion/frmVisualizaNotificaciones";
        }
    }, function (error) {
    });

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



    $scope.grPedido = [];
    var _resDgPedidos = [];
    $scope.pageContentPed = [];
    $scope.pagesPed = [];
    $scope.totalGrid = {};

    $scope.resDgPedidostodoslosregistro = [];

    $scope.pgcDgPedidosDET = [];

    $scope.pOpcGrp1 = "P";
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
    $scope.titulo = "";
    $scope.npage = 1;
    //$scope.pRuc = "";
    //$scope.pUsuario = "";
    //$scope.pCodSAP = "";
    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;



    $scope.selecLinea = {};
    $scope.selecSeccion = {};
    $scope.selecSubSeccion = {};
    $scope.selecGrupo = {};

    $scope.listaLinea = [];
    $scope.listaSeccion = [];
    $scope.listaSeccionTMP = [];
    $scope.listaSubSeccion = [];
    $scope.listaSubSeccionTMP = [];
    $scope.listaGrupo = [];
    $scope.listaGrupoTMP = [];

    $scope.listaAlmacenes = [];
    $scope.selecAlmacen = {};
    $scope.SettingAlmacenes = { displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '300px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk', selectionLimit: 1 };



    $scope.cboPaginList = [
        { codigo: 10, descripcion: "10" },
        { codigo: 25, descripcion: "25" },
        { codigo: 35, descripcion: "35" },
        { codigo: 50, descripcion: "50" }
    ];
    $scope.cboPaginSelItem = $scope.cboPaginList[0];


    $scope.chbMeses = [
    { codigo: '01', descripcion: "Enero" },
    { codigo: '02', descripcion: "Febrero" },
    { codigo: '03', descripcion: "Marzo" },
    { codigo: '04', descripcion: "Abril" },
    { codigo: '05', descripcion: "Mayo" },
    { codigo: '06', descripcion: "Junio" },
    { codigo: '07', descripcion: "Julio" },
    { codigo: '08', descripcion: "Agosto" },
    { codigo: '09', descripcion: "Septiembre" },
    { codigo: '10', descripcion: "Octubre" },
    { codigo: '11', descripcion: "Noviembre" },
    { codigo: '12', descripcion: "Diciembre" }
    ];
    $scope.cboPaginSelItemMeses = $scope.chbMeses[0];


    $scope.chbAnio = [];
    // { codigo: dAnio, descripcion: dAnio },
    // { codigo: dAnioD, descripcion: dAnioD }
    //];
    $scope.cboPaginSelItemAnio = "";


    //------ combo estados

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
    $scope.etiTotRegistros = "";
    $scope.Fechas = [];
    $scope.genaralData = [];
    $scope.genaralCentro = [];
    $scope.genaralArticulo = [];

    $scope.myPromise = ReporteEstadisticosService.getAnios("4", $scope.pCodSAP).then(function (results) {
        if (results.data.success) {

            var lisanio = results.data.root[0];

            $scope.chbAnio = lisanio;
            $scope.cboPaginSelItemAnio = $scope.chbAnio[0];
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });



    $scope.Eliminar = function () {
        $('#container').highcharts({
            title: { text: ' ' },
            series: []
        });

    }

    $scope.CargarGrafico = function () {

        $scope.Eliminar();
        var rndtotal = [];

        var datos = [];

        for (var i = 0; i < $scope.genaralArticulo.length; i++) {
            var resumen = $filter('filter')($scope.genaralData, { codMaterial: $scope.genaralArticulo[i].codArticulo }, true);
            debugger;
            var rnd = [];
            var aux = {};
            aux.name = resumen[0].desMaterial;
            for (var j = 0; j < $scope.Fechas.length; j++) {

                var aux1 = $filter('filter')(resumen, { fecha: $scope.Fechas[j] }, true);

                if (aux1.length > 0)
                    rnd.push(parseInt(aux1[0].cantVendida, 10));
                else
                    if (aux1.length == 0)
                        rnd.push(0);
            }
            aux.data = rnd;
            datos.push(aux);
        }




        $('#container').highcharts({
            title: {
                text: 'Periodo: ' + $scope.pFechaIni + ' - ' + $scope.pFechaFin,
                x: -20 //center
            },
            subtitle: {
                text: '',
                x: -20
            },
            xAxis: {
                categories: $scope.Fechas,
                title: {
                    text: $scope.genaralCentro[0].centro
                }
            },
            yAxis: {
                title: {
                    text: 'Cantidad Vendida'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                valueSuffix: ' unidades'
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            series: datos
        });

    }


    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("1", $scope.pCodSAP).then(function (results) {
        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            listAlmacen.splice(0, 0, { pCodAlmacen: "", pNomAlmacen: "Todas los Almacenes" });
            $scope.listaAlmacenes = listAlmacen;
            $scope.selecAlmacen = listAlmacen[0];
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesReportes().");
    });

    $scope.myPromise = ReporteEstadisticosService.getConsAlmacenes("2", $scope.pCodSAP).then(function (results) {

        if (results.data.success) {
            var listArticulo = results.data.root[0];
            $scope.chkArticuloList = listArticulo;
        }
        else {
            $scope.showMessage('E', 'Error al consultar Articulos: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: ConsAlmacenesArticulosReportes().");
    });









    function validate_fechaMayorQue(fechaInicial, fechaFinal) {

        valuesStart = fechaInicial.split("/");

        valuesEnd = fechaFinal.split("/");



        // Verificamos que la fecha no sea posterior a la actual

        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);

        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);

        if (dateStart > dateEnd) {

            return 1;

        }

        return 0;

    }

    $scope.exportar = function (tipo) {
        $scope.npage = 1;
        
        $scope.selecAlmacenes = [];

        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";


        if (!angular.isUndefined($scope.selecAlmacen.pCodAlmacen))
            $scope.selecAlmacenes.push($scope.selecAlmacen);
        else {
            $scope.showMessage('I', 'Seleccione un almacén.');
            return;
        }

        var date = new Date();
        var ultimoDia = new Date($scope.cboPaginSelItemAnio.codigo, $scope.cboPaginSelItemMeses.codigo, 0);

        var dateStringDia = new Date();
        $scope.pFechaIni = '01/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;
        $scope.pFechaFin = ultimoDia.getDate() + '/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;


        Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');

        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }


        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            if ($scope.selecAlmacenes[ix].pCodAlmacen != "") {
                var list = {};
                list.id = $scope.selecAlmacenes[ix].pCodAlmacen;
                listaEnviarAlm.push(list);
            }
        }


        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.TipoLista = "2";
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        cabeceraDatos.Linea = $scope.selecLinea.codigo;
        cabeceraDatos.Seccion = $scope.selecSeccion.codigo;
        cabeceraDatos.SubSeccion = $scope.selecSubSeccion.codigo;
        cabeceraDatos.Grupo = $scope.selecGrupo.codigo;
        cabeceraDatos.nomreporte = $scope.titulo;
        cabeceraDatos.p_usuario = $scope.pUsuario;


        $scope.resDgPedidos = [];



        if ($scope.etiTotRegistros == '' || $scope.etiTotRegistros == '0') {
            $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
            return;
        }

        $scope.myPromise = ReporteEstadisticosService.getConsReporteMercadoExp(cabeceraDatos, listaEnviarAlm).then(function (results) {
            if (tipo == "1") {
                var file = new Blob([results.data], { type: 'application/xls' });
                saveAs(file, 'REPORTE PARTICIPACIÓN_MERCADO_' + $scope.pCodSAP + '.xls');
            }
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
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
        $scope.selecAlmacenes = [];

        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";


        if (!angular.isUndefined($scope.selecAlmacen.pCodAlmacen))
            $scope.selecAlmacenes.push($scope.selecAlmacen);
        else {
            $scope.showMessage('I', 'Seleccione un almacén.');
            return;
        }




        var date = new Date();
        var ultimoDia = new Date($scope.cboPaginSelItemAnio.codigo, $scope.cboPaginSelItemMeses.codigo, 0);

        var dateStringDia = new Date();
        $scope.pFechaIni = '01/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;
        $scope.pFechaFin = ultimoDia.getDate() + '/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;


        Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');

        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }


        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            if ($scope.selecAlmacenes[ix].pCodAlmacen != "") {
                var list = {};
                list.id = $scope.selecAlmacenes[ix].pCodAlmacen;
                listaEnviarAlm.push(list);
            }
        }


        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.TipoLista = "1";
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        cabeceraDatos.Linea = "";
        cabeceraDatos.Seccion = "";
        cabeceraDatos.SubSeccion = "";
        cabeceraDatos.Grupo = "";
        $scope.pgcDgPedidos = [];
        $scope.myPromise = ReporteEstadisticosService.getConsReporteMercado(cabeceraDatos, listaEnviarAlm).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
                    $scope.pgcDgPedidos = [];
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {
                    $scope.selecLinea = {};
                    $scope.listaLinea = [];
                    $scope.selecSeccion = {};
                    $scope.listaSeccion = [];
                    $scope.listaSeccionTMP = [];
                    $scope.selecSubSeccion = {};
                    $scope.listaSubSeccion = [];
                    $scope.listaSubSeccionTMP = [];
                    $scope.selecGrupo = {};
                    $scope.listaGrupo = [];
                    $scope.listaGrupoTMP = [];
                    $scope.titulo = "";
                    $scope.titulo = "Almacén:" + $scope.selecAlmacen.pNomAlmacen + " Periodo:" + $scope.cboPaginSelItemMeses.descripcion + "/" + $scope.cboPaginSelItemAnio.codigo;
                    var resumen = results.data.root[0];
                    resumen.splice(0, 0, { codigo: "", descripcion: "Todas las Líneas" });
                    $scope.listaLinea = results.data.root[0];
                    $scope.selecLinea = resumen[0];
                    $scope.listaSeccionTMP = results.data.root[1];
                    $scope.listaSubSeccionTMP = results.data.root[2];
                    $scope.listaGrupoTMP = results.data.root[3];

                    var resumen = [];
                    resumen = $scope.listaSeccionTMP;
                    $scope.listaSeccion = resumen;
                    $scope.selecSeccion = resumen[0];

                    var resumen = [];
                    resumen.splice(0, 0, { codigo: "", descripcion: "Todas las SubSecciones" });
                    $scope.listaSubSeccion = resumen;
                    $scope.selecSubSeccion = resumen[0];

                    var resumen = [];
                    resumen.splice(0, 0, { codigo: "", descripcion: "Todos los Grupos" });
                    $scope.listaGrupo = resumen;
                    $scope.selecGrupo = resumen[0];


                    $scope.pgcDgPedidos = results.data.root[4];


                    $scope.etiTotRegistros = $scope.pgcDgPedidos.length;

                    $scope.buscarsubseccion();
                    $scope.buscargrupo();
                    //$scope.pgcDgPedidos = $scope.resDgPedidos;
                }
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            //setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            //setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
        //setTimeout(function () { $('#btnConsulta').focus(); }, 100);
        //setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
    };

    $scope.Limpiar = function () {

        //$scope.selecLinea = {};
        //$scope.listaLinea = [];
        //$scope.selecSeccion = {};
        //$scope.listaSeccion = [];
        //$scope.listaSeccionTMP = [];
        //$scope.selecSubSeccion = {};
        //$scope.listaSubSeccion = [];
        //$scope.listaSubSeccionTMP = [];
        //$scope.selecGrupo = {};
        //$scope.listaGrupo = [];
        //$scope.listaGrupoTMP = [];
        //$scope.titulo = "";

        $scope.pgcDgPedidos = [];

    }
    $scope.btnConsultaClickD = function () {
        $scope.npage = 1;
        $scope.selecAlmacenes = [];

        var Fecha1 = ""; var Fecha2 = ""; var Estado = ""; var bandGenHtml = true; var Almacen = "";

      
        if (!angular.isUndefined($scope.selecAlmacen.pCodAlmacen))
            $scope.selecAlmacenes.push($scope.selecAlmacen);
        else {
            $scope.showMessage('I', 'Seleccione un almacén.');
            return;
        }

        var date = new Date();
        var ultimoDia = new Date($scope.cboPaginSelItemAnio.codigo, $scope.cboPaginSelItemMeses.codigo, 0);

        var dateStringDia = new Date();
        $scope.pFechaIni = '01/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;
        $scope.pFechaFin = ultimoDia.getDate() + '/' + $scope.cboPaginSelItemMeses.codigo + '/' + $scope.cboPaginSelItemAnio.codigo;


        Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');

        if ($scope.selecAlmacenes.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }


        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = [];
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++) {
            if ($scope.selecAlmacenes[ix].pCodAlmacen != "") {
                var list = {};
                list.id = $scope.selecAlmacenes[ix].pCodAlmacen;
                listaEnviarAlm.push(list);
            }
        }


        var cabeceraDatos = {};
        cabeceraDatos.CodSap = $scope.pCodSAP;
        cabeceraDatos.TipoLista = "2";
        cabeceraDatos.Fecha1 = Fecha1;
        cabeceraDatos.Fecha2 = Fecha2;
        cabeceraDatos.Linea = $scope.selecLinea.codigo;
        cabeceraDatos.Seccion = $scope.selecSeccion.codigo;
        cabeceraDatos.SubSeccion = $scope.selecSubSeccion.codigo;
        cabeceraDatos.Grupo = $scope.selecGrupo.codigo;
        cabeceraDatos.nomreporte = "Reporte Mercadeo";
        cabeceraDatos.p_usuario = $scope.pUsuario;
        $scope.pgcDgPedidos = [];
        $scope.myPromise = ReporteEstadisticosService.getConsReporteMercado(cabeceraDatos, listaEnviarAlm).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].length == 0) {
                    $scope.showMessage('I', 'No existen datos para generar el reporte con los filtros seleccionados.');
                    $scope.pgcDgPedidos = [];
                    $scope.hideGrid = true;
                    $scope.showPaginate = false;
                    $scope.etiTotRegistros = '';
                }
                else {

                    //$scope.resDgPedidos = results.data.root[0];
                    //var listAlmacen = results.data.root[0];
                    //listAlmacen.splice(0, 0, { cCodigoEmpresaOfi: "", cDescEmpresaOfi: "Todas los Almacenes" });
                    $scope.pgcDgPedidos = results.data.root[0];
                    $scope.etiTotRegistros = $scope.pgcDgPedidos.length;
                    //$scope.pgcDgPedidos = $scope.resDgPedidos;


                }
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            //setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            //setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
        },
         function (error) {
             var errors = [];
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
        //setTimeout(function () { $('#btnConsulta').focus(); }, 100);
        //setTimeout(function () { $('#btnConsultaexecel').focus(); }, 150);
    };
    $scope.buscarseccion = function () {
        var resumen = {};
        if ($scope.selecLinea.codigo!="") {
            resumen = $filter('filter')($scope.listaSeccionTMP, { dePendencia: $scope.selecLinea.codigo }, true);
        } else
        {
            resumen = $scope.listaSeccionTMP
        }
         
        ////resumen.splice(0, 0, { codigo: "", descripcion: "Todas las Secciones" });
        $scope.listaSeccion = resumen;
        $scope.selecSeccion = resumen[0];

        var resumen = [];
        resumen.splice(0, 0, { codigo: "", descripcion: "Todas las SubSecciones" });
        $scope.listaSubSeccion = resumen;
        $scope.selecSubSeccion = resumen[0];

        var resumen = [];
        resumen.splice(0, 0, { codigo: "", descripcion: "Todos los Grupos" });
        $scope.listaGrupo = resumen;
        $scope.selecGrupo = resumen[0];

        $scope.btnConsultaClickD();
    }

    $scope.buscarsubseccion = function () {
        var resumen = $filter('filter')($scope.listaSubSeccionTMP, { dePendencia: $scope.selecSeccion.codigo }, true);
        resumen.splice(0, 0, { codigo: "", descripcion: "Todas las SubSecciones" });
        $scope.listaSubSeccion = resumen;
        $scope.selecSubSeccion = resumen[0];

        var resumen = [];
        resumen.splice(0, 0, { codigo: "", descripcion: "Todos los Grupos" });
        $scope.listaGrupo = resumen;
        $scope.selecGrupo = resumen[0];

        $scope.btnConsultaClickD();
    }

    $scope.buscargrupo = function () {

        var resumen = $filter('filter')($scope.listaGrupoTMP, { dePendencia: $scope.selecSubSeccion.codigo }, true);
        resumen.splice(0, 0, { codigo: "", descripcion: "Todos los Grupos" });
        $scope.listaGrupo = resumen;
        $scope.selecGrupo = resumen[0];

        $scope.btnConsultaClickD();
    }

    $scope.buscargrupof = function () {
        $scope.btnConsultaClickD();
    }
    $scope.Mantenimiento = function () {
        if ($scope.solicitud.opcion == 'I')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
                    cargaSolicitudes();
                    $scope.solicitud.opcion = 'I';
                }, function (error) {

                });


        if ($scope.solicitud.opcion == 'A')
            if (parseInt($scope.solicitud.canDespachar) <= parseInt($scope.rowDetalle.saldo) + parseInt($scope.auxSaldo))
                ReporteEstadisticosService.mntSolicitud($scope.solicitud).then(function (results) {
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
        ReporteEstadisticosService.consSolicitud('C', $scope.pedido, $scope.articulo).then(function (results) {
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
        $scope.myPromise = ReporteEstadisticosService.getBuscarAsignacion("AP", authService.authentication.CodSAP).then(function (results) {
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
    // $scope.CargaInicial();
}]);

