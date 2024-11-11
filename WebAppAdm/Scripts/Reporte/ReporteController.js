//Reporte Usuario No Ingreso
app.controller('ReporteNoUsuarioController', ['$scope', '$location', 'ReporteAdministradorService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, ReporteAdministradorService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {
    
    //Variable de Busquedas
    $scope.txtsap = "";
    $scope.txtusuario = "";
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    //Fin Variable
    $scope.filtroBusquedad = false;
    $scope.etiTotRegistros = "";
    //Combos
    $scope.EstadoSolicitudGrip = [];
    $scope.selectedItemGrip = "";
    //Fin Combos

    $scope.GridConsolidacion = [];
    var _GridConsolidacion = [];
    $scope.pagesCo = [];
    $scope.pageContentCo = [];

    $scope.resDgConsulta = [];
    $scope.sortTypeCon = 'idconsolidacion';

    $scope.pageContentCon = [];

    $scope.codid = "";


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

    $scope.busTodos = false;

    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    //Fin Variable

    $scope.pOpcGrp1 = "P";
    $scope.habilitaru = true;
    $scope.habilitarp = false;

    $scope.CodProveedor = authService.authentication.CodSAP;
    $scope.usuarioCreacion = authService.authentication.userName;
    $scope.variablelleva = "";
    $scope.id = "";
    $scope.idcita = "";


    $scope.limpioCaja = function () {
        $scope.txtsap = "";
        $scope.txtusuario = "";

    }

    $scope.habilitar= function()
    {
        var dato = $scope.pOpcGrp1;
        if (dato=="P") {
            $scope.habilitaru = true;
            $scope.habilitarp = false;
            $scope.txtusuario = "";
            $scope.txtsap = "";
        } else {
            if (dato == "U") {
                $scope.habilitaru = false;
                $scope.habilitarp = true;
                $scope.txtusuario = "";
                $scope.txtsap = "";
            }
        }

    }
    $scope.verFiltro = function () {
        if ($scope.busTodos) {
            $scope.filtroBusquedad = true;
            $scope.txtusuario = "";
            $scope.txtsap = "";
        }
        else {
            $scope.filtroBusquedad = false;
            $scope.txtusuario = "";
            $scope.txtsap = "";
        }
    };
    $scope.Consultar = function () {

        if ($scope.busTodos == false) {
            if ($scope.pOpcGrp1 == "P") {
                if ($scope.txtsap == "") {
                    $scope.MenjError = "Codigo de Proveedor es Obligatorio."
                    $('#idMensajeInformativo').modal('show');
                    return;
                }

            }
            if ($scope.pOpcGrp1 == "U") {
                if ($scope.txtusuario == "") {
                    $scope.MenjError = "Usuario de Proveedor es Obligatorio."
                    $('#idMensajeInformativo').modal('show');
                    return;
                }

            }
        }


        $scope.myPromise = null;
        $scope.etiTotRegistros = "";
        $scope.myPromise = ReporteAdministradorService.getConsulaGridNoIngreso( $scope.txtsap, $scope.txtusuario).then(function (results) {
            if (results.data.success) {
                $scope.resDgConsulta = results.data.root[0];
                $scope.etiTotRegistros = $scope.resDgConsulta.length.toString();
            }
            else
            {
                $scope.resDgConsulta = [];
                $scope.etiTotRegistros = $scope.resDgConsulta.length.toString();
            }
            //if ($scope.pageContentCo.length == 0) {
            if ($scope.resDgConsulta.length == 0) {
                $scope.MenjError = "No existen resultados para su consulta."
                $scope.resDgConsulta = [];
                $('#idMensajeInformativo').modal('show');
                return;
            }
            setTimeout(function () { $('#btnConsulta1').focus(); }, 100);
            setTimeout(function () { $('#busTodos').focus(); }, 150);

        }, function (error) {
        });
       
    }

    $scope.exportar = function (tipo) {
        if ($scope.pageContentCo.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeInformativo').modal('show');
            return;
        }
        $scope.myPromise = null;
     
        $scope.myPromise = ReporteAdministradorService.getExportarData(tipo, $scope.usuarioCreacion, $scope.txtsap, $scope.txtusuario).then(function (results) {
            if (results.data != "") {
                if (tipo == "1") {
                    var file = new Blob([results.data], { type: 'application/xls' });
                    saveAs(file, 'ReporteNoIngreso.xls');
                }
                if (tipo == "2") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'ReporteNoIngreso.pdf');
                }
            }
        }, function (error) {
        });
        setTimeout(function () { $('#consultagrid').focus(); }, 10);

    }


}
]);

//Reporte Usuario Proveedor No Orden Compra
app.controller('ReporteNoOrdenCompraController', ['$scope', '$location', 'ReporteAdministradorService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, ReporteAdministradorService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {

    //Variable de Busquedas
    $scope.txtsap = "";
    $scope.txtruc = "";
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    //Fin Variable

    $scope.etiTotRegistros = "";
    //Combos
    $scope.EstadoSolicitudGrip = [];
    $scope.selectedItemGrip = "";
    //Fin Combos

    $scope.GridConsolidacion = [];
    var _GridConsolidacion = [];
    $scope.pagesCo = [];
    $scope.pageContentCo = [];

    $scope.resDgConsulta = [];
    $scope.sortTypeCon = 'idconsolidacion';

    $scope.pageContentCon = [];

    $scope.codid = "";


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

    $scope.Consultar = function () {



        $scope.myPromise = null;
        $scope.etiTotRegistros = "";
        $scope.myPromise = ReporteAdministradorService.getConsulaGridNoOrdenCompra($scope.txtsap, $scope.txtruc).then(function (results) {
            if (results.data.success) {
                $scope.resDgConsulta = results.data.root[0];
                $scope.etiTotRegistros = $scope.resDgConsulta.length.toString();
            } else {
                $scope.MenjError = "No existen resultados para su consulta."
                $('#idMensajeInformativo').modal('show');
            }
            setTimeout(function () { $('#btnConsulta1').focus(); }, 100);
            setTimeout(function () { $('#txtsap').focus(); }, 150);

        }, function (error) {
        });
    }

    $scope.exportar = function (tipo) {
        if ($scope.pageContentCo.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeInformativo').modal('show');
            return;
        }
        $scope.myPromise = null;

        $scope.myPromise = ReporteAdministradorService.getExportarNoCompra(tipo, $scope.usuarioCreacion, $scope.txtsap, $scope.txtruc).then(function (results) {
            if (results.data != "") {
                if (tipo == "1") {
                    var file = new Blob([results.data], { type: 'application/xls' });
                    saveAs(file, 'ReporteNoProveedorCompra.xls');
                }
                if (tipo == "2") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'ReporteNoProveedorCompra.pdf');
                }
            }
        }, function (error) {
        });
        setTimeout(function () { $('#consultagrid').focus(); }, 10);

    }


}
]);

//Reporte Usuario Proveedor No Orden Compra
app.controller('ReporteProveedorNoSolicitudController', ['$scope', '$location', 'ReporteAdministradorService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, ReporteAdministradorService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {

    //Variable de Busquedas
    $scope.txtsap = "";
    $scope.txtruc = "";
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    //Fin Variable

    $scope.etiTotRegistros = "";
    //Combos
    $scope.EstadoSolicitudGrip = [];
    $scope.selectedItemGrip = "";
    //Fin Combos

    $scope.GridConsolidacion = [];
    var _GridConsolidacion = [];
    $scope.pagesCo = [];
    $scope.pageContentCo = [];

    $scope.resDgConsulta = [];
    $scope.sortTypeCon = 'idconsolidacion';

    $scope.pageContentCon = [];

    $scope.codid = "";
    $scope.busTodos = false;

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

    $scope.pOpcGrp1 = "P";
    $scope.habilitaru = true;
    $scope.habilitarp = false;

    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    //Fin Variable

    $scope.CodProveedor = authService.authentication.CodSAP;
    $scope.usuarioCreacion = authService.authentication.userName;
    $scope.variablelleva = "";
    $scope.id = "";
    $scope.idcita = "";

    $scope.habilitar = function () {
        var dato = $scope.pOpcGrp1;
        if (dato == "P") {
            $scope.habilitaru = true;
            $scope.habilitarp = false;
            $scope.txtsap = "";
            $scope.txtruc = "";
        } else {
            if (dato == "U") {
                $scope.habilitaru = false;
                $scope.habilitarp = true;
                $scope.txtsap = "";
                $scope.txtruc = "";
            }
        }

    }
    $scope.verFiltro = function () {
        if ($scope.busTodos) {
            $scope.filtroBusquedad = true;
            $scope.txtsap = "";
            $scope.txtruc = "";
        }
        else {
            $scope.filtroBusquedad = false;
            $scope.txtsap = "";
            $scope.txtruc = "";
        }
    };
    $scope.limpioCaja = function () {
        $scope.txtruc = "";
        $scope.txtsap = "";

    }

    $scope.Consultar = function () {
        if ($scope.busTodos==false) {
            if ($scope.pOpcGrp1 == "P") {
                if ($scope.txtsap=="") {
                    $scope.MenjError = "Codigo de Proveedor es Obligatorio."
                    $('#idMensajeInformativo').modal('show');
                    return;
                }
                
            }
            if ($scope.pOpcGrp1 == "U") {
                if ($scope.txtruc == "") {
                    $scope.MenjError = "RUC de Proveedor es Obligatorio."
                    $('#idMensajeInformativo').modal('show');
                    return;
                }
                
            }
        }
       
        

        $scope.myPromise = null;
        $scope.etiTotRegistros = "";
        $scope.myPromise = ReporteAdministradorService.getConsulaGridProveedorNoSolicitud($scope.txtsap, $scope.txtruc).then(function (results) {
            if (results.data.success) {
                $scope.resDgConsulta = results.data.root[0];
                $scope.etiTotRegistros = $scope.resDgConsulta.length.toString();
            } 
            
            if ($scope.resDgConsulta.length.toString() == 0) {
                    $scope.MenjError = "No exiten resultados para su consulta."
                    $('#idMensajeInformativo').modal('show');
                }

            setTimeout(function () { $('#btnConsulta1').focus(); }, 100);
            setTimeout(function () { $('#busTodos').focus(); }, 150);

        }, function (error) {
        });
    }

    $scope.exportar = function (tipo) {
        if ($scope.pageContentCo.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeInformativo').modal('show');
            return;
        }
        $scope.myPromise = null;

        $scope.myPromise = ReporteAdministradorService.getExportarProveedorNoSolicitud(tipo, $scope.usuarioCreacion, $scope.txtsap, $scope.txtruc).then(function (results) {
            if (results.data != "") {
                if (tipo == "1") {
                    var file = new Blob([results.data], { type: 'application/xls' });
                    saveAs(file, 'ReporteProveedorNoSolicitud.xls');
                }
                if (tipo == "2") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'ReporteProveedorNoSolicitud.pdf');
                }
            }
        }, function (error) {
        });
        setTimeout(function () { $('#consultagrid').focus(); }, 10);

    }


}
]);

//Reporte Usuario Proveedor No Orden Compra
app.controller('ReporteLogComunicacionController', ['$scope', '$location', 'ReporteAdministradorService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, ReporteAdministradorService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {
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
        $scope.pgcDgPedidosDETus = $filter('filter')($scope.resDgPedidostodoslosregistrousuario, { cod_notificacion: content.codigo,cod_proveedor: content.codproveedor });

        $scope.etiTotRegistrosdetlogdet = "";
        $scope.etiTotRegistrosdetlogdet = $scope.pgcDgPedidosDETus.length.toString();

        $('#MostradetalleLogComunicadodet').modal('show');
    }
    $scope.Consultar = function () {
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
        $scope.etiTotRegistros = "";
        $scope.myPromise = ReporteAdministradorService.getConsulaGridLogComunicacion($scope.txtsap, $scope.txtruc, Fecha1, Fecha2).then(function (results) {
            if (results.data.success) {
                $scope.resDgConsulta = results.data.root[0];
                $scope.etiTotRegistros = $scope.resDgConsulta.length.toString();

                $scope.resDgPedidostodoslosregistro = results.data.root[1];
                $scope.resDgPedidostodoslosregistrousuario = results.data.root[2];
            }
            setTimeout(function () { $('#btnConsulta1').focus(); }, 100);
            setTimeout(function () { $('#txtsap').focus(); }, 150);

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

        $scope.myPromise = ReporteAdministradorService.getExportarLogComunicado(tipo, $scope.usuarioCreacion, $scope.txtsap, $scope.txtruc, Fecha1, Fecha2).then(function (results) {
            if (results.data != "") {
                if (tipo == "1") {
                    var file = new Blob([results.data], { type: 'application/xls' });
                    saveAs(file, 'ReporteLogComunicado.xls');
                }
                if (tipo == "2") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'ReporteLogComunicado.pdf');
                }
            }
        }, function (error) {
        });
        setTimeout(function () { $('#consultagrid').focus(); }, 10);

    }


}
]);