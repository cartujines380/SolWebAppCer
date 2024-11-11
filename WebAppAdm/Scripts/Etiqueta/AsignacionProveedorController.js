

'use strict';
app.controller('AsignacionProveedorController', ['$scope', '$location', '$http', 'AsignacionProveedorService', 'GeneralService', 'ngAuthSettings', '$cookies', '$filter', 'FileUploader', 'authService', 'localStorageService', '$sce', function ($scope, $location, $http, AsignacionProveedorService, GeneralService, ngAuthSettings, $cookies, $filter, FileUploader, authService, localStorageService, $sce) {

    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.txtCodProveedor = "";
    $scope.txtRuc = "";
    $scope.txtNomComer = "";
    $scope.ProveedorEti = [];
    $scope.ProveedorEti2 = [];
    $scope.ProveedorEtiT = [];
    $scope.pagesCon = [];
    $scope.rdbCodigo = 0;
    

    //Filtro de consultas
    $scope.filtroConsulta3 = function () {
        debugger;
        if ($scope.rdbCodigo == "1" && $scope.txtCodProveedor == "")
        {
            $scope.MenjError = "Ingrese código del proveedor.";
            $('#idMensajeInformativo').modal('show');
        }
        if ($scope.rdbCodigo == "2" && $scope.txtRuc == "") {
            $scope.MenjError = "Ingrese RUC del proveedor.";
            $('#idMensajeInformativo').modal('show');
        }

        if ($scope.rdbCodigo == "3" && $scope.txtNomComer == "") {
            $scope.MenjError = "Ingrese nombre comercial del proveedor.";
            $('#idMensajeInformativo').modal('show');
        }

        if ($scope.rdbCodigo == "0") {
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            $scope.ProveedorEti2 = $scope.ProveedorEtiT.slice();
            setTimeout(function () { $('#rbtArtRef').focus(); }, 150);
        }

        if ($scope.rdbCodigo == "1") {
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            $scope.ProveedorEti2 = $filter('filter')($scope.ProveedorEtiT, { codProveedor: $scope.txtCodProveedor }, true);
            setTimeout(function () { $('#rbtArtRef').focus(); }, 150);
        }

        if ($scope.rdbCodigo == "2") {
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            $scope.ProveedorEti2 = $filter('filter')($scope.ProveedorEtiT, { ruc: $scope.txtRuc }, true);
            setTimeout(function () { $('#rbtArtRef').focus(); }, 150);
        }

        if ($scope.rdbCodigo == "3") {
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            $scope.ProveedorEti2 = $filter('filter')($scope.ProveedorEtiT, { nombreComercial: $scope.txtNomComer });
            setTimeout(function () { $('#rbtArtRef').focus(); }, 150);
        }

        $scope.etiTotRegistros = $scope.ProveedorEti2.length;
        if ($scope.etiTotRegistros == 0) {
            $scope.MenjError = "No existe resultado para su consulta.";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        
    }


    //Consulta Proveedores
    $scope.CargaConsulta = function () {
        
        $scope.myPromise = AsignacionProveedorService.getProveedorEtiqList().then(function (results) {
           
            if (results.data.success) {

                $scope.ProveedorEti2 = results.data.root[0];
                $scope.ProveedorEtiT = $scope.ProveedorEti2.slice();
                $scope.etiTotRegistros = $scope.ProveedorEti2.length;
                
            }
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            setTimeout(function () { $('#rbtArtRef').focus(); }, 150);

        }, function (error) {
            $scope.MenjError = "Error de comunicación: AsignacionProveedorService.getProveedorEtiqList()";
            $('#idMensajeError').modal('show');
        });

    }

    $scope.GeneraEtiqueta = function (content) {
        $scope.contenido = content;
        if (content.generaEtiqueta) {
            $scope.MenjConfirmacion = "¿Está seguro que proveedor genera etiquetas?";
            $('#idMensajeConfirmacion').modal('show');
        }
        else {
            $scope.grabar("S");
        }
        content.generaEtiqueta = false;

    }

    

    //Generar etiquetas S/N
    $scope.grabar = function (valido) {
        debugger;
        if (valido != "S")
            $scope.contenido.generaEtiqueta = true;
        var content = $scope.contenido;
       
        var genEtiq = "0";
        var usr = authService.authentication.userName;
        if (content.generaEtiqueta)
        {
            genEtiq = "1";
        }

        $scope.myPromise = AsignacionProveedorService.getActualizaProv(content.codProveedor, genEtiq, usr).then(function (results) {
            
            if (results.data.success) {


            }
            else {
                $scope.MenjError = results.data.mensaje;
                $('#idMensajeError').modal('show');
                return;
            }
           
        }, function (error) {
            $scope.MenjError = "Error de comunicación: AsignacionProveedorService.getActualizaProv()" ;
            $('#idMensajeError').modal('show');
        });


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

}]);

'use strict';
app.controller('ReporteSolicitudCitaController', ['$scope', '$location', 'ReporteAdministradorService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, ReporteAdministradorService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {
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

    $scope.pOpcGrp1 = "P";
    $scope.etiTotRegistros = "";
    //Combos
    $scope.EstadoSolicitudGrip = [];
    $scope.selectedItemGrip = "";
    //Fin Combos

    $scope.porestado = 2;

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
    $scope.variablelleva = "";
    $scope.id = "";
    $scope.idcita = "";


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

        if ($scope.pOpcGrp1 == "R") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                //$scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                $scope.MenjError = "Seleccione la fecha inicial del rango a consultar.";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                //$scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                $scope.MenjError = "Seleccione la fecha final del rango a consultar.";
                $('#idMensajeInformativo').modal('show');
                return;
            }


            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                //$scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                $scope.MenjError = "La fecha final debe ser mayor a la fecha inicial a consultar.";
                $('#idMensajeInformativo').modal('show');
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
        $scope.myPromise = ReporteAdministradorService.getConsulaGriSolicitudEtiqueta($scope.txtsap, $scope.txtruc, Fecha1, Fecha2, $scope.porestado).then(function (results) {
            if (results.data.success) {
                $scope.resDgConsulta = results.data.root[0];
                $scope.etiTotRegistros = $scope.resDgConsulta.length.toString();
                if ($scope.etiTotRegistros=='0') {
                    $scope.MenjError = "No hay resultado de la consulta.";
                    $('#idMensajeInformativo').modal('show');
                }
                //$scope.resDgPedidostodoslosregistro = results.data.root[1];
                //$scope.resDgPedidostodoslosregistrousuario = results.data.root[2];
            }
            else {
                $scope.MenjError = "No hay resultado de la consulta.";
                $('#idMensajeInformativo').modal('show');
            }
            setTimeout(function () { $('#btnConsulta1').focus(); }, 100);
            setTimeout(function () { $('#txtsap').focus(); }, 150);

        }, function (error) {
        });
    }

    function validate_fechaMayorQue(fechaInicial, fechaFinal) {

      var  valuesStart = fechaInicial.split("/");

      var valuesEnd = fechaFinal.split("/");



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

        $scope.myPromise = ReporteAdministradorService.getExportarSolicitudEtiqueta(tipo, $scope.usuarioCreacion, $scope.txtsap, $scope.txtruc, Fecha1, Fecha2, $scope.porestado).then(function (results) {
            if (results.data != "") {
                if (tipo == "1") {
                    var file = new Blob([results.data], { type: 'application/xls' });
                    saveAs(file, 'ReporteSolicitudEtiqueta.xls');
                }
                if (tipo == "2") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'ReporteSolicitudEtiqueta.pdf');
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



