'use strict';
app.controller('ReqEmpresaController', ['$scope', '$location', '$http', 'RequerimientoService', 'GeneralService', 'ngAuthSettings', '$cookies', '$filter', 'FileUploader', 'authService', 'localStorageService', '$sce', function ($scope, $location, $http, RequerimientoService, GeneralService, ngAuthSettings, $cookies, $filter, FileUploader, authService, localStorageService, $sce) {

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

    var dateString1 = new Date();
    var dateString = new Date();
    var d1 = dateString1.format("dd/mm/yyyy");
    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");

    $scope.pFechaIni = d2;
    $scope.pFechaFin = d1;

    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.pagesCon = [];
    $scope.rdbOpcion = "T";

    $scope.proveedor = {};
    $scope.Proveedores = [];
    $scope.SettingGrupoArt = { enableSearch: true, selectionLimit: 1, displayProp: 'descripcion', idProp: 'id', scrollableHeight: '200px', scrollable: true, };//{ displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    //Filtro de consultas
    $scope.filtroConsulta3 = function () {

        if ($scope.rdbCodigo == "1" && $scope.txtCodProveedor == "") {
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

    $scope.openReq = function (item) {
        $scope.req = angular.copy(item);
        $('#editor1').html($scope.req.descripcion);
        $('#reqModal').modal('show');
    }

    //Consulta Bandeja
    $scope.CargaConsulta = function () {

        var Fecha1 = "";
        var Fecha2 = "";
        var empresa = "";
        var categoria = "";

        if ($scope.rdbOpcion == "F") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.MenjError = "Seleccione la fecha inicial del rango a consultar.";
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.MenjError = "Seleccione la fecha final del rango a consultar.";
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.MenjError = "La fecha final debe ser mayor a la fecha inicial a consultar.";
                $('#idMensajeInformativo').modal('show');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd/MM/yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd/MM/yyyy');
        }

        if ($scope.rdbOpcion == "R") {
            if ($scope.txtCodRequerimiento == null || $scope.txtCodRequerimiento == "" || angular.isUndefined($scope.txtCodRequerimiento)) {
                $scope.MenjError = "Ingrese el código de la requisición";
                $('#idMensajeInformativo').modal('show');
                return;
            }
        }

        if ($scope.rdbOpcion == "E") {
            if ($scope.cboEmpresa == null || $scope.cboEmpresa == "" || angular.isUndefined($scope.cboEmpresa)) {
                $scope.MenjError = "Seleccione una empresa";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            empresa = $scope.cboEmpresa.id;
        }

        if ($scope.rdbOpcion == "C") {
            if ($scope.cboCategoria == null || $scope.cboCategoria == "" || angular.isUndefined($scope.cboCategoria)) {
                $scope.MenjError = "Seleccione una línea de negocio";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            categoria = $scope.cboCategoria.id;
        }

        $scope.myPromise = null;
        $scope.etiTotRegistros = "";
        $scope.myPromise = RequerimientoService.getReqs($scope.rdbOpcion, Fecha1, Fecha2, $scope.txtCodRequerimiento, empresa, categoria).then(function (results) {
            if (results.data.success) {
                $scope.ReqsEmpresa = results.data.root[0];
                $scope.etiTotRegistros = $scope.ReqsEmpresa.length.toString();
                if ($scope.etiTotRegistros == '0') {
                    $scope.MenjError = "No hay resultado de la consulta.";
                    $('#idMensajeInformativo').modal('show');
                }
            }
            else {
                $scope.MenjError = "No hay resultado de la consulta.";
                $('#idMensajeInformativo').modal('show');
            }

        }, function (error) {
        });
    }

    function validate_fechaMayorQue(fechaInicial, fechaFinal) {

        var valuesStart = fechaInicial.split("/");
        var valuesEnd = fechaFinal.split("/");

        // Verificamos que la fecha no sea posterior a la actual
        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);
        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);

        if (dateStart >= dateEnd) {
            return 1;
        }

        return 0;
    }

    //Carga Empresas
    function cargaEmpresas() {

        $scope.Empresas = [];
        $scope.myPromise = RequerimientoService.getCatalogo('E').then(function (results) {

            if (results.data.success) {
                $scope.Empresas = results.data.root[0];
            }
        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
            $('#idMensajeError').modal('show');
        });
    }

    function cargaProveedores() {

        $scope.Proveedores = [];
        $scope.myPromise = RequerimientoService.getCatalogo('P').then(function (results) {

            if (results.data.success) {
                debugger;
                $scope.Proveedores = results.data.root[0];
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
            $('#idMensajeError').modal('show');
        });
    }

    cargaEmpresas();
    cargaProveedores();
    $scope.cboProveedor = $scope.Proveedores[0];

    function cargaCategorias() {

        $scope.Categorias = [];
        $scope.myPromise = RequerimientoService.getCatalogo('C').then(function (results) {

            if (results.data.success) {
                $scope.Categorias = results.data.root[0];
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
            $('#idMensajeError').modal('show');
        });
    }

    cargaCategorias();

    $scope.confirmaPrioritario = function (item) {
        if (item.estadoRequerimiento == 'C') {
            $scope.selePrio = angular.copy(item);
            $scope.seleNPrio = {};
            $scope.proveedor = { id: 1 };
            $('#proveedoresModal').modal('show');
            $scope.mensajeProvee = "Existe un contrato preestablecido, pero puede elegir otra empresa.";
        }
        else {
            $scope.selePrio = angular.copy(item);
            $scope.seleNPrio = {};
            $scope.proveedor = {};
            $('#proveedoresModal').modal('show');
            $scope.mensajeProvee = "Seleccione un proveedor para la adjudicación directa:";
            $scope.accion = 'AD';
        }
    }

    $scope.confirmaNPrioritario = function (item) {

        $scope.selePrio = {};
        $scope.seleNPrio = angular.copy(item);
        $scope.accion = 'LI';
        $scope.MenjConfirmacion = "¿Está seguro de enviar la requisición a licitar?";
        $('#idMensajeConfirmacion').modal('show');

        return;
    }
    $scope.aceptar = function () {
        $scope.CargaConsulta();
    }
    $scope.grabar = function () {
        if ($scope.accion == 'AD') {
            if (angular.isUndefined($scope.proveedor.id)) {
                $scope.MenjError = "Seleccione al menos un proveedor";
                $('#idMensajeInformativo').modal('show');
                return;
            }

            $scope.myPromise = RequerimientoService.setDirecto($scope.selePrio.idReq, $scope.proveedor.id, 1).then(function (results) {
                if (results.data == 1) {
                    $('#proveedoresModal').modal('hide');
                    $scope.MenjError = "Requisición adjudicada";
                    $('#idMensajeOk').modal('show');
                    return;
                }

            }, function (error) {
                $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
                $('#idMensajeError').modal('show');
            });
        }
        else
            if ($scope.accion == 'LI') {
                $scope.myPromise = RequerimientoService.setLicitacion($scope.seleNPrio.idReq).then(function (results) {
                    if (results.data == 1) {
                        $('#proveedoresModal').modal('hide');
                        $scope.MenjError = "La requisición fue enviada a licitar correctamente";
                        $('#idMensajeOk').modal('show');
                        return;
                    }

                }, function (error) {
                    $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
                    $('#idMensajeError').modal('show');
                });
            }
    }
}]);

'use strict';
app.controller('ReqConcursoController', ['$scope', '$location', '$http', 'RequerimientoService', 'GeneralService', 'ngAuthSettings', '$cookies', '$filter', 'FileUploader', 'authService', 'localStorageService', '$sce', '$timeout', function ($scope, $location, $http, RequerimientoService, GeneralService, ngAuthSettings, $cookies, $filter, FileUploader, authService, localStorageService, $sce, $timeout) {

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

    $scope.Seles = [{ id: "PRO", desc: "Proveedor:" }, { id: "CAT", desc: "Línea de negocio:" }];

    Date.prototype.format = function (mask, utc) {
        return dateFormat(this, mask, utc);
    };

    var dateString1 = new Date();
    var dateString = new Date();

    var d1 = dateString1.format("dd/mm/yyyy");

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");

    $scope.pFechaIni = d2;
    $scope.pFechaFin = d1;

    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.txtCodProveedor = "";
    $scope.txtRuc = "";
    $scope.txtNomComer = "";
    $scope.pagesCon = [];
    $scope.ReqsEmpresa = [];
    $scope.ReqsEmpresa_ = [];
    $scope.rdbOpcion = "T";
    $scope.seleTodos = false;
    $scope.idInputFile = "";
    $scope.idFile = "";
    $scope.idre = "";

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    $scope.Ruta = serviceBase + 'UploadedDocuments/Uploads/';
    var Ruta = serviceBase + 'api/Upload/UploadFile/?path=prueba';
    ///CARGA DE ARCHIVO 
    var uploader2 = $scope.uploader2 = new FileUploader({
        url: Ruta
    });

    uploader2.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {

            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "pdf") {
                $("#" + $scope.idInputFile.toString()).val(item.name);
                return true;
            }
            else {
                $scope.MenjError = "El formato del archivo es invalido, favor selecione un documento  pdf";
                $('#idMensajeError').modal('show');
                $("#" + $scope.idFile).val('');
                return false;
            }
        }
    });

    $scope.file = function (index, id) {
        $scope.idInputFile = index;
        $scope.idFile = id;

        $timeout(function () {
            angular.element('#' + id).trigger('click');
        });
    };

    $scope.CargaConsulta = function () {

        var Fecha1 = "";
        var Fecha2 = "";
        var empresa = "";
        var categoria = "";

        if ($scope.rdbOpcion == "F") {
            if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                $scope.MenjError = "Seleccione la fecha inicial del rango a consultar.";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                $scope.MenjError = "Seleccione la fecha final del rango a consultar.";
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                $scope.MenjError = "La fecha final debe ser mayor a la fecha inicial a consultar.";
                $('#idMensajeInformativo').modal('show');
                return;
            }

            Fecha1 = $filter('date')($scope.pFechaIni, 'dd/MM/yyyy');
            Fecha2 = $filter('date')($scope.pFechaFin, 'dd/MM/yyyy');
        }

        if ($scope.rdbOpcion == "R") {
            if ($scope.txtCodRequerimiento == null || $scope.txtCodRequerimiento == "" || angular.isUndefined($scope.txtCodRequerimiento)) {
                $scope.MenjError = "Ingrese el código de la requisición";
                $('#idMensajeInformativo').modal('show');
                return;
            }
        }

        if ($scope.rdbOpcion == "E") {
            if ($scope.cboEmpresa == null || $scope.cboEmpresa == "" || angular.isUndefined($scope.cboEmpresa)) {
                $scope.MenjError = "Seleccione una empresa";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            empresa = $scope.cboEmpresa.id;
        }

        if ($scope.rdbOpcion == "C") {
            if ($scope.cboCategoria == null || $scope.cboCategoria == "" || angular.isUndefined($scope.cboCategoria)) {
                $scope.MenjError = "Seleccione una línea de negocio";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            categoria = $scope.cboCategoria.id;
        }

        $scope.myPromise = null;
        $scope.etiTotRegistros = "";
        $scope.myPromise = RequerimientoService.getReqsLici($scope.rdbOpcion, Fecha1, Fecha2, $scope.txtCodRequerimiento, empresa, categoria).then(function (results) {
            if (results.data.success) {
                $scope.ReqsEmpresa = results.data.root[0];
                $scope.etiTotRegistros = $scope.ReqsEmpresa.length.toString();
                if ($scope.etiTotRegistros == '0') {
                    $scope.MenjError = "No hay resultado de la consulta.";
                    $('#idMensajeInformativo').modal('show');
                }
            }
            else {
                $scope.MenjError = "No hay resultado de la consulta.";
                $('#idMensajeInformativo').modal('show');
            }

        }, function (error) {
        });
    }

    $scope.openReq = function (item) {

        $scope.req = angular.copy(item);
        if (!angular.isUndefined($scope.req.descEmp))
            $scope.req.empresa = $scope.req.descEmp;

        $('#editor1').html($scope.req.descripcion);
        $('#reqModal').modal('show');
    }

    function cargaEmpresas() {

        $scope.Empresas = [];

        $scope.myPromise = RequerimientoService.getCatalogo('E').then(function (results) {

            if (results.data.success) {
                $scope.Empresas = results.data.root[0];
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
            $('#idMensajeError').modal('show');
        });

    }

    cargaEmpresas();

    function cargaProveedores() {

        $scope.proveedoresDS = [];

        $scope.myPromise = RequerimientoService.getCatalogo('P').then(function (results) {

            if (results.data.success) {
                $scope.proveedoresDS = results.data.root[0];
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
            $('#idMensajeError').modal('show');
        });

    }

    cargaProveedores();

    //Carga Categorias
    function cargaCategorias() {

        $scope.Categorias = [];

        $scope.myPromise = RequerimientoService.getCatalogo('C').then(function (results) {

            if (results.data.success) {
                $scope.Categorias = results.data.root[0];
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
            $('#idMensajeError').modal('show');
        });
    }

    cargaCategorias();

    $scope.proveedores = [];
    $scope.proveedoresDS = [];
    $scope.SettingGrupoArt = { enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk', displayProp: 'descripcion', idProp: 'id', };

    $scope.categs = [];
    $scope.SettingCat = { enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk', displayProp: 'descripcion', idProp: 'id', };

    $scope.tipoBusqueda = "xL";
    $scope.esAplia = false;
    $scope.concurso = { documentos: [] };

    $scope.elimDoc = function (index) {
        $scope.concurso.documentos.splice(index, 1);
    }

    //Filtro de consultas
    $scope.filtroConsulta3 = function () {
        if ($scope.rdbCodigo == "1" && $scope.txtCodProveedor == "") {
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

    //Consulta Bandeja
    $scope.CargaConcursos = function () {

        $scope.Concursos = [];
        $scope.myPromise = RequerimientoService.getPublicaciones().then(function (results) {

            if (results.data.success) {
                $scope.Concursos = results.data.root[0];
                $scope.TotConcursos = $scope.Concursos.length;
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getPublicaciones()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.terminaLici = function (item) {
        $scope.accion = 'TL';
        $scope.seleLici = item;
        $('#idMensajeConfirmacion').modal('show');
        $scope.MenjConfirmacion = "La Licitación Pasará a un Estado de Terminada.";
    }

    function validate_fechaMayorQue(fechaInicial, fechaFinal) {

        var valuesStart = fechaInicial.split("/");
        var valuesEnd = fechaFinal.split("/");

        // Verificamos que la fecha no sea posterior a la actual
        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);
        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);

        if (dateStart > dateEnd) {
            return 1;
        }

        return 0;
    }

    function validate_fechaMayorQueHora(fechaInicial, fechaFinal, horainicio, horafin) {

        var valuesStart = fechaInicial.split("/");
        var valuesEnd = fechaFinal.split("/");
        var horaini = horainicio.split(":");
        var horafin = horafin.split(":");

        // Verificamos que la fecha no sea posterior a la actual
        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0], horaini[0], horaini[1], horaini[2]);
        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0], horafin[0], horafin[1], horafin[2]);

        if (dateStart > dateEnd) {
            return 1;
        }

        return 0;
    }

    $scope.CargaAplicantes = function (item) {
        $scope.myPromise = RequerimientoService.getProvDocs(item.idReq).then(function (results) {
            if (results.data.success) {
                $('#proveedoresModal').modal('show');
                $scope.provSeles = results.data.root[0];
            }
        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getProvDocs()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.CargaProDocs = function (item) {
        $scope.proDocSele = angular.copy(item);
        $('#proDocsModal').modal('show');
    }

    //open modal
    $scope.openPublicar = function (content) {
        var dateString1 = new Date();
        var dateString = new Date();

        var d1 = dateString1.format("dd/mm/yyyy");

        dateString.setDate(dateString.getDate() + 15);
        var d2 = dateString.format("dd/mm/yyyy");

        $scope.esAplia = false;
        $scope.ReqsSele = [];
        $scope.ReqsSele.push(content);
        var fecha = moment();
        $scope.concurso = { nomPubli: "LIC_" + fecha.format('DDMMYYYY'), documentos: [] };
        $scope.concurso.feIni = d2;
        $scope.concurso.feFin = d1;

        $scope.concurso.hoFin = "00:00:00";

        for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
            if ($scope.ReqsEmpresa[i].isSele)
                $scope.ReqsSele.push($scope.ReqsEmpresa[i]);

        $scope.myPromise = RequerimientoService.getDocumentos($scope.ReqsSele[0].idReq).then(function (results) {
            if (results.data.success) {
                $scope.concurso.documentos = results.data.root[0];
                return;
            }
            else {
                $scope.MenjError = "Ocurrión un error al cargar los documentos.";
                $('#idMensajeError').modal('show');
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getDocumentos()";
            $('#idMensajeError').modal('show');
        });

        if ($scope.ReqsSele.length > 0)
            $('#concursoModal').modal('show');
        else {
            $scope.MenjError = "Debe seleccionar al menos un una requisición";
            $('#idMensajeError').modal('show');
        }
    }

    $scope.seleccionaTodos = function () {
        $scope.seleTodos = !$scope.seleTodos;

        if ($scope.seleTodos == false) {
            for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
                $scope.ReqsEmpresa[i].isSele = false;

            for (var i = 0; i < $scope.ReqsEmpresa_.length; i++)
                for (var j = 0; j < $scope.ReqsEmpresa_[i].ReqsEmpresa.length; j++)
                    $scope.ReqsEmpresa_[i].ReqsEmpresa[j].isSele = false;
        }
        else
            if ($scope.seleTodos == true) {
                for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
                    $scope.ReqsEmpresa[i].isSele = true;


                for (var i = 0; i < $scope.ReqsEmpresa_.length; i++)
                    for (var j = 0; j < $scope.ReqsEmpresa_[i].ReqsEmpresa.length; j++)
                        $scope.ReqsEmpresa_[i].ReqsEmpresa[j].isSele = true;
            }

    }

    $scope.CargaConcursos();

    function validaConcurso() {

        if ($scope.concurso.nomPubli == null || angular.isUndefined($scope.concurso.nomPubli) || $scope.concurso.nomPubli.trim() == "") {
            $scope.MenjError = "Ingrese un nombre para la publicación";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if ($scope.concurso.descPubli == null || angular.isUndefined($scope.concurso.descPubli) || $scope.concurso.descPubli.trim() == "") {
            $scope.MenjError = "Ingrese una descripción";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if ($scope.concurso.feIni == null || angular.isUndefined($scope.concurso.feIni) || $scope.concurso.feIni.trim() == "") {
            $scope.MenjError = "Ingrese la fecha de inicio";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if ($scope.concurso.feFin == null || angular.isUndefined($scope.concurso.feFin) || $scope.concurso.feFin.trim() == "") {
            $scope.MenjError = "Ingrese la fecha de finalización";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        if ($scope.concurso.hoFin == null || angular.isUndefined($scope.concurso.hoFin) || $scope.concurso.hoFin.trim() == "") {
            $scope.MenjError = "Ingrese la hora de finalización";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if ($scope.concurso.documentos.length == 0) {
            $scope.MenjError = "Debe subir al menos un arhivo";
            $('#idMensajeInformativo').modal('show');
            return false;
        }

        return true;
    }

    $scope.aceptar = function () {
        $scope.CargaConcursos();
        $scope.CargaConsulta();
    }

    $scope.guardaConcurso = function () {
        debugger;
        if (!validaConcurso)
            return

        if ($scope.concurso.descPubli == '' || angular.isUndefined($scope.concurso.descPubli)) {
            $scope.MenjError = "Debe ingresar una descripción.";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if (angular.isUndefined($scope.Sele) || angular.isUndefined($scope.Sele.id)) {
            $scope.MenjError = "Debe seleccionar los invitados a licitar";
            $('#idMensajeInformativo').modal('show');
            return false;
        }

        if ($scope.Sele.id == 'PRO' && $scope.proveedores.length == 0) {
            $scope.MenjError = "Debe seleccionar los proveedores";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        else
            if ($scope.Sele.id == 'CAT' && $scope.categs.length == 0) {
                $scope.MenjError = "Debe seleccionar las línea de negocio";
                $('#idMensajeInformativo').modal('show');
                return false;
            }

        $scope.concurso.feFin = $filter('date')($scope.concurso.feFin, 'dd/MM/yyyy');
        $scope.concurso.feIni = $filter('date')($scope.concurso.feIni, 'dd/MM/yyyy');
        $scope.concurso.hoFin = $filter('date')($scope.concurso.hoFin, 'hh:mm:ss');
        $scope.concurso.idReq = $scope.ReqsSele[0].idReq;

        var dateString1 = new Date();
        var d1hoy = dateString1.format("dd/mm/yyyy");
        var hora = dateString1.format("HH:MM:ss");

        if (validate_fechaMayorQueHora(d1hoy, $scope.concurso.feIni, hora, $scope.concurso.hoFin)) {
            $scope.MenjError = "La hora de finalización de la publicación no puede ser menor a la hora de fin de la publicación.";
            $('#idMensajeError').modal('show');
            return;
        }

        if (validate_fechaMayorQue($scope.concurso.feFin, $scope.concurso.feIni)) {
            $scope.MenjError = "La fecha de finalización de la publicación no puede ser menor a la fecha de inicio de la publicación.";
            $('#idMensajeError').modal('show');
            return;
        }

        var provs = "";
        if ($scope.Sele.id == 'PRO')
            $scope.proveedores.forEach(function (val) {
                provs += val.id + ','
            });
        else
            $scope.categs.forEach(function (val) {
                provs += val.id + ','
            });

        $scope.concurso.tipoInvitacion = $scope.Sele.id;

        $scope.concurso.provs = provs;

        var files = [];
        var index = 0;
        angular.forEach($scope.concurso.documentos, function (value) {
            if (value.idDoc == 0)
                files.push($("#adjunto" + index)[0].files[0]);
            index++;
        });

        var a1 = $scope.concurso.feFin;
        var b1 = $scope.concurso.feIni;

        $scope.concurso.feFin = b1
        $scope.concurso.feIni = a1;

        $scope.myPromise = RequerimientoService.publica($scope.concurso, files).then(function (results) {
            if (results.data.success) {
                $('#concursoModal').modal('hide');
                $scope.MenjError = "Se publicó la licitación correctamente";
                $('#idMensajeOk').modal('show');
                return;
            }
            else {
                $scope.MenjError = "Ocurrión un error.";
                $('#idMensajeError').modal('show');
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.publica()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.showAdjuntos = function (item) {
        $scope.liciSele = angular.copy(item);
        $('#documentosModal').modal('show');
        $scope.Documentos = angular.copy($scope.liciSele.documentos);
    };

    $scope.grabar = function () {

        if ($scope.accion == 'TL') {
            $scope.seleLici.estado = 'T';
            $scope.seleLici.version = $scope.seleLici.version + 1;
        }
    }

    $scope.agregaDoc = function () {
        $scope.concurso.documentos.push({ desc: "", idDoc: 0, archivo: "", idDoc: 0 });
    }

    $scope.habiltar = false;

    $scope.eliminarLicitacion = function () {
        $scope.myPromise = RequerimientoService.getEliLici($scope.idre).then(function (results) {
            if (results.data.success) {
                $('#concursoModal').modal('hide');
                $scope.MenjError = "Licitación Eliminada con éxito";
                $('#idMensajeOk').modal('show');
                return;
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getProvDocs()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.openAmplia = function (item) {
        $scope.idre = item.idReq
        $scope.concurso = angular.copy(item);
        $scope.concurso.feFin = item.feIni;
        $scope.concurso.feIni = item.feFin;
        $scope.ReqsSele = [];
        $scope.ReqsSele.push({ titulo: $scope.concurso.titulo, descripcion: $scope.concurso.descripcion, idReq: $scope.concurso.idReq, monto: $scope.concurso.monto });
        $scope.esAplia = true;
        $scope.habiltar = false;
        $scope.myPromise = RequerimientoService.getProvDocs(item.idReq).then(function (results) {
            if (results.data.success) {
                var tmp = results.data.root[0];
                for (var i = 0; i < tmp.length; i++) {
                    if (tmp[i].estadoParticipando == "A") {
                        $scope.habiltar = true;
                    }
                }
                if (item.estado.trim() == "E") {
                    $scope.habiltar = true;
                }
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getProvDocs()";
            $('#idMensajeError').modal('show');
        });

        $('#concursoModal').modal('show');

    }

    $scope.ampliaFecha = function () {

        $scope.myPromise = RequerimientoService.getProvDocs($scope.idre).then(function (results) {

            if (results.data.success) {
                var datos = $filter('filter')(results.data.root[0], { estadoParticipando: "P" }, true);

                if (datos.length > 0) {
                    $scope.MenjError = "No se puede modificar la licitacion ya hay un proveedor participando.";
                    $('#idMensajeInformativo').modal('show');
                } else {

                    if (!validaConcurso())
                        return;

                    var files = [];
                    var index = 0;

                    angular.forEach($scope.concurso.documentos, function (value) {
                        if (value.idDoc == 0)
                            files.push($("#adjunto" + index)[0].files[0]);
                        index++;
                    });

                    $scope.myPromise = RequerimientoService.editaPubli($scope.concurso, files).then(function (results) {
                        if (results.data.success) {
                            $('#concursoModal').modal('hide');
                            $scope.MenjError = "Licitación modificada con éxito";
                            $('#idMensajeOk').modal('show');
                            return;
                        }
                        else {
                            $scope.MenjError = "Ocurrión un error.";
                            $('#idMensajeError').modal('show');
                        }

                    }, function (error) {
                        $scope.MenjError = "Error de comunicación: RequerimientoService.editaPubli()";
                        $('#idMensajeError').modal('show');
                    });
                }
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getProvDocs()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.ampliaLici = function () {
    }
}]);

'use strict';
app.controller('ReqConcursoBandejaController', ['$scope', '$location', '$http', 'RequerimientoService', 'GeneralService', 'ngAuthSettings', '$cookies', '$filter', 'FileUploader', 'authService', 'localStorageService', '$sce', function ($scope, $location, $http, RequerimientoService, GeneralService, ngAuthSettings, $cookies, $filter, FileUploader, authService, localStorageService, $sce) {

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

    $scope.Seles = [{ id: "PRO", desc: "Proveedor:" }, { id: "CAT", desc: "Línea de negocio:" }];

    Date.prototype.format = function (mask, utc) {
        return dateFormat(this, mask, utc);
    };

    var dateString1 = new Date();
    var dateString = new Date();

    var d1 = dateString1.format("dd/mm/yyyy");

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");

    $scope.pFechaIni = d2;
    $scope.pFechaFin = d1;

    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.txtCodProveedor = "";
    $scope.txtRuc = "";
    $scope.txtNomComer = "";
    $scope.pagesCon = [];
    $scope.ReqsEmpresa = [];
    $scope.ReqsEmpresa_ = [];
    $scope.rdbOpcion = "T";
    $scope.seleTodos = false;

    $scope.idre = "";

    $scope.Ruta = serviceBase + 'UploadedDocuments/Uploads/';

    $scope.openReq = function (item) {

        $scope.req = angular.copy(item);
        if (!angular.isUndefined($scope.req.descEmp))
            $scope.req.empresa = $scope.req.descEmp;

        $('#editor1').html($scope.req.descripcion);
        $('#reqModal').modal('show');
    }

    function cargaEmpresas() {

        $scope.Empresas = [];
        $scope.myPromise = RequerimientoService.getCatalogo('E').then(function (results) {

            if (results.data.success) {
                $scope.Empresas = results.data.root[0];
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
            $('#idMensajeError').modal('show');
        });
    }

    cargaEmpresas();

    function cargaProveedores() {

        $scope.proveedoresDS = [];
        $scope.myPromise = RequerimientoService.getCatalogo('P').then(function (results) {

            if (results.data.success) {
                $scope.proveedoresDS = results.data.root[0];
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
            $('#idMensajeError').modal('show');
        });

    }

    cargaProveedores();

    function cargaCategorias() {

        $scope.Categorias = [];
        $scope.myPromise = RequerimientoService.getCatalogo('C').then(function (results) {

            if (results.data.success) {
                $scope.Categorias = results.data.root[0];
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getCatalogo()";
            $('#idMensajeError').modal('show');
        });
    }

    cargaCategorias();

    $scope.proveedores = [];
    $scope.proveedoresDS = [];
    $scope.SettingGrupoArt = { enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk', displayProp: 'descripcion', idProp: 'id', };

    $scope.categs = [];
    $scope.SettingCat = { enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk', displayProp: 'descripcion', idProp: 'id', };

    $scope.tipoBusqueda = "xL";
    $scope.esAplia = false;
    $scope.concurso = { documentos: [] };

    $scope.elimDoc = function (index) {
        $scope.concurso.documentos.splice(index, 1);
    }

    //Filtro de consultas
    $scope.filtroConsulta3 = function () {
        if ($scope.rdbCodigo == "1" && $scope.txtCodProveedor == "") {
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

    //Consulta Bandeja
    $scope.CargaConcursos = function () {

        $scope.Concursos = [];
        $scope.myPromise = RequerimientoService.getPublicacioneBande().then(function (results) {

            if (results.data.success) {
                $scope.Concursos = results.data.root[0];
                $scope.TotConcursos = $scope.Concursos.length;
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getPublicaciones()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.terminaLici = function (item) {
        $scope.accion = 'TL';
        $scope.seleLici = item;
        $('#idMensajeConfirmacion').modal('show');
        $scope.MenjConfirmacion = "La Licitación Pasará a un Estado de Terminada.";
    }

    function validate_fechaMayorQue(fechaInicial, fechaFinal) {

        var valuesStart = fechaInicial.split("/");
        var valuesEnd = fechaFinal.split("/");

        // Verificamos que la fecha no sea posterior a la actual
        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);
        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);

        if (dateStart > dateEnd) {
            return 1;
        }

        return 0;
    }

    $scope.CargaAplicantes = function (item) {
        $scope.myPromise = RequerimientoService.getProvDocs(item.idReq).then(function (results) {
            if (results.data.success) {
                $('#proveedoresModal').modal('show');
                $scope.provSeles = results.data.root[0];
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getProvDocs()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.CargaProDocs = function (item) {
        $scope.proDocSele = angular.copy(item);
        $('#proDocsModal').modal('show');
    }

    //open modal
    $scope.openPublicar = function (content) {
        var dateString1 = new Date();
        var dateString = new Date();

        var d1 = dateString1.format("dd/mm/yyyy");

        dateString.setDate(dateString.getDate() + 15);
        var d2 = dateString.format("dd/mm/yyyy");

        $scope.esAplia = false;
        $scope.ReqsSele = [];
        $scope.ReqsSele.push(content);
        var fecha = moment();
        $scope.concurso = { nomPubli: "LIC_" + fecha.format('DDMMYYYY'), documentos: [] };
        $scope.concurso.feIni = d2;
        $scope.concurso.feFin = d1;

        $scope.concurso.hoFin = "00:00:00";

        for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
            if ($scope.ReqsEmpresa[i].isSele)
                $scope.ReqsSele.push($scope.ReqsEmpresa[i]);


        $scope.myPromise = RequerimientoService.getDocumentos($scope.ReqsSele[0].idReq).then(function (results) {
            if (results.data.success) {
                $scope.concurso.documentos = results.data.root[0];
                return;
            }
            else {
                $scope.MenjError = "Ocurrión un error al cargar los documentos.";
                $('#idMensajeError').modal('show');
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getDocumentos()";
            $('#idMensajeError').modal('show');
        });

        if ($scope.ReqsSele.length > 0)
            $('#concursoModal').modal('show');
        else {
            $scope.MenjError = "Debe seleccionar al menos un una requisición";
            $('#idMensajeError').modal('show');
        }
    }

    $scope.seleccionaTodos = function () {
        $scope.seleTodos = !$scope.seleTodos;

        if ($scope.seleTodos == false) {
            for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
                $scope.ReqsEmpresa[i].isSele = false;

            for (var i = 0; i < $scope.ReqsEmpresa_.length; i++)
                for (var j = 0; j < $scope.ReqsEmpresa_[i].ReqsEmpresa.length; j++)
                    $scope.ReqsEmpresa_[i].ReqsEmpresa[j].isSele = false;
        }
        else
            if ($scope.seleTodos == true) {
                for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
                    $scope.ReqsEmpresa[i].isSele = true;


                for (var i = 0; i < $scope.ReqsEmpresa_.length; i++)
                    for (var j = 0; j < $scope.ReqsEmpresa_[i].ReqsEmpresa.length; j++)
                        $scope.ReqsEmpresa_[i].ReqsEmpresa[j].isSele = true;
            }
    }

    $scope.CargaConcursos();

    function validaConcurso() {

        if ($scope.concurso.nomPubli == null || angular.isUndefined($scope.concurso.nomPubli) || $scope.concurso.nomPubli.trim() == "") {
            $scope.MenjError = "Ingrese un nombre para la publicación";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if ($scope.concurso.descPubli == null || angular.isUndefined($scope.concurso.descPubli) || $scope.concurso.descPubli.trim() == "") {
            $scope.MenjError = "Ingrese una descripción";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if ($scope.concurso.feIni == null || angular.isUndefined($scope.concurso.feIni) || $scope.concurso.feIni.trim() == "") {
            $scope.MenjError = "Ingrese la fecha de inicio";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if ($scope.concurso.feFin == null || angular.isUndefined($scope.concurso.feFin) || $scope.concurso.feFin.trim() == "") {
            $scope.MenjError = "Ingrese la fecha de finalización";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        if ($scope.concurso.hoFin == null || angular.isUndefined($scope.concurso.hoFin) || $scope.concurso.hoFin.trim() == "") {
            $scope.MenjError = "Ingrese la hora de finalización";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if ($scope.concurso.documentos.length == 0) {
            $scope.MenjError = "Debe subir al menos un arhivo";
            $('#idMensajeInformativo').modal('show');
            return false;
        }

        return true;
    }


    $scope.aceptar = function () {
        $scope.CargaConcursos();
    }

    $scope.guardaConcurso = function () {
        if (!validaConcurso)
            return

        if (angular.isUndefined($scope.Sele) || angular.isUndefined($scope.Sele.id)) {
            $scope.MenjError = "Debe seleccionar los invitados a licitar";
            $('#idMensajeInformativo').modal('show');
            return false;
        }

        if ($scope.Sele.id == 'PRO' && $scope.proveedores.length == 0) {
            $scope.MenjError = "Debe seleccionar los proveedores";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        else
            if ($scope.Sele.id == 'CAT' && $scope.categs.length == 0) {
                $scope.MenjError = "Debe seleccionar las línea de negocio";
                $('#idMensajeInformativo').modal('show');
                return false;
            }

        $scope.concurso.feFin = $filter('date')($scope.concurso.feFin, 'dd/MM/yyyy');
        $scope.concurso.feIni = $filter('date')($scope.concurso.feIni, 'dd/MM/yyyy');
        $scope.concurso.hoFin = $filter('date')($scope.concurso.hoFin, 'hh:mm:ss');
        $scope.concurso.idReq = $scope.ReqsSele[0].idReq;

        if (validate_fechaMayorQue($scope.concurso.feFin, $scope.concurso.feIni)) {
            $scope.MenjError = "La fecha de finalización de la publicación no puede ser menor a la fecha de inicio de la publicación.";
            $('#idMensajeError').modal('show');
            return;
        }

        var provs = "";
        if ($scope.Sele.id == 'PRO')
            $scope.proveedores.forEach(function (val) {
                provs += val.id + ','
            });
        else
            $scope.categs.forEach(function (val) {
                provs += val.id + ','
            });

        $scope.concurso.tipoInvitacion = $scope.Sele.id;

        $scope.concurso.provs = provs;

        var files = [];
        var index = 0;
        angular.forEach($scope.concurso.documentos, function (value) {
            if (value.idDoc == 0)
                files.push($("#adjunto" + index)[0].files[0]);
            index++;
        });

        var a1 = $scope.concurso.feFin;
        var b1 = $scope.concurso.feIni;

        $scope.concurso.feFin = b1
        $scope.concurso.feIni = a1;

        $scope.myPromise = RequerimientoService.publica($scope.concurso, files).then(function (results) {
            if (results.data.success) {
                $('#concursoModal').modal('hide');
                $scope.MenjError = "Se publicó la licitación correctamente";
                $('#idMensajeOk').modal('show');
                return;
            }
            else {
                $scope.MenjError = "Ocurrión un error.";
                $('#idMensajeError').modal('show');
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.publica()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.showAdjuntos = function (item) {
        $scope.liciSele = angular.copy(item);
        $('#documentosModal').modal('show');
        $scope.Documentos = angular.copy($scope.liciSele.documentos);
    };

    $scope.grabar = function () {

        if ($scope.accion == 'TL') {
            $scope.seleLici.estado = 'T';
            $scope.seleLici.version = $scope.seleLici.version + 1;
        }
    }

    $scope.agregaDoc = function () {
        $scope.concurso.documentos.push({ desc: "", idDoc: 0, archivo: "", idDoc: 0 });
    }

    $scope.habiltar = false;

    $scope.eliminarLicitacion = function () {
        $scope.myPromise = RequerimientoService.getEliLici($scope.idre).then(function (results) {
            if (results.data.success) {
                $('#concursoModal').modal('hide');
                $scope.MenjError = "Licitación Eliminada con éxito";
                $('#idMensajeOk').modal('show');
                return;
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getProvDocs()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.openAmplia = function (item) {
        $scope.idre = item.idReq
        $scope.concurso = angular.copy(item);
        $scope.concurso.feFin = item.feIni;
        $scope.concurso.feIni = item.feFin;
        $scope.ReqsSele = [];
        $scope.ReqsSele.push({ titulo: $scope.concurso.titulo, descripcion: $scope.concurso.descripcion, idReq: $scope.concurso.idReq, monto: $scope.concurso.monto });
        $scope.esAplia = true;
        $scope.habiltar = false;
        $scope.myPromise = RequerimientoService.getProvDocs(item.idReq).then(function (results) {
            if (results.data.success) {
                var tmp = results.data.root[0];
                for (var i = 0; i < tmp.length; i++) {
                    if (tmp[i].estadoParticipando == "A") {
                        $scope.habiltar = true;
                    }
                }
                if (item.estado.trim() == "E") {
                    $scope.habiltar = true;
                }
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getProvDocs()";
            $('#idMensajeError').modal('show');
        });

        $('#concursoModal').modal('show');
    }

    $scope.ampliaFecha = function () {

        $scope.myPromise = RequerimientoService.getProvDocs($scope.idre).then(function (results) {

            if (results.data.success) {
                var datos = $filter('filter')(results.data.root[0], { estadoParticipando: "P" }, true);

                if (datos.length > 0) {
                    $scope.MenjError = "No se puede modificar la licitacion ya hay un proveedor participando.";
                    $('#idMensajeInformativo').modal('show');
                } else {

                    if (!validaConcurso())
                        return;

                    var files = [];
                    var index = 0;

                    angular.forEach($scope.concurso.documentos, function (value) {
                        if (value.idDoc == 0)
                            files.push($("#adjunto" + index)[0].files[0]);
                        index++;
                    });

                    $scope.myPromise = RequerimientoService.editaPubli($scope.concurso, files).then(function (results) {
                        if (results.data.success) {
                            $('#concursoModal').modal('hide');
                            $scope.MenjError = "Licitación modificada con éxito";
                            $('#idMensajeOk').modal('show');
                            return;
                        }
                        else {
                            $scope.MenjError = "Ocurrión un error.";
                            $('#idMensajeError').modal('show');
                        }

                    }, function (error) {
                        $scope.MenjError = "Error de comunicación: RequerimientoService.editaPubli()";
                        $('#idMensajeError').modal('show');
                    });
                }
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getProvDocs()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.ampliaLici = function () {
    }
}]);

'use strict';
app.controller('ReqAdjudicaController', ['$scope', '$location', '$http', 'RequerimientoService', 'GeneralService', 'ngAuthSettings', '$cookies', '$filter', 'FileUploader', 'authService', 'localStorageService', '$sce', function ($scope, $location, $http, RequerimientoService, GeneralService, ngAuthSettings, $cookies, $filter, FileUploader, authService, localStorageService, $sce) {

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

    var dateString1 = new Date();
    var dateString = new Date();

    var d1 = dateString1.format("dd/mm/yyyy");

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");

    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.txtCodProveedor = "";
    $scope.txtRuc = "";
    $scope.txtNomComer = "";
    $scope.ProveedorEti = [];
    $scope.ProveedorEti2 = [];
    $scope.ProveedorEtiT = [];
    $scope.pagesCon = [];
    $scope.rdbOpcion = 0;
    $scope.seleTodos = false;


    $scope.idrequerimiento = "";
    $scope.idempresa = [];
    $scope.idempresa.id = "";
    $scope.idempresa.empresa = "";
    $scope.idempresa.monto = "";
    $scope.idempresa.codproveedor = "";

    $scope.rdbOpcion_ = 0;

    $scope.Ruta = serviceBase + 'UploadedDocuments/Uploads/';

    $scope.proveedores = [];
    $scope.proveedoresDS = [{ id: 1, label: "HP del Ecuador S.A." }, { id: 2, label: "Papelesa S.A." }, { id: 3, label: "Xerox del Ecuador S.A." }, { id: 4, label: "Autopartes Centro S.A." }, { id: 5, label: "Andres Borbor S.A." }, { id: 6, label: "Briko S.A." }];
    $scope.SettingGrupoArt = { enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.familias = [];
    $scope.familiasDS = [{ detalle: "Papelería", codigo: 1 }, { detalle: "Repuestos", codigo: 2 }, { detalle: "Automotores", codigo: 3 }, { detalle: "Tecnología", codigo: 4 }, { detalle: "Electrónica", codigo: 5 }, { detalle: "Varios", codigo: 6 }];

    $scope.tipoBusqueda = "xL";
    $scope.concurso = { reqs: [] };

    $scope.ConcursosGeneral = [];

    $scope.Concursos = [];
    $scope.pFechaIniIni = "";
    $scope.pFechaIniFin = "";
    $scope.txtNoLicitacion = "";
    $scope.pFechaFinIni = "";
    $scope.pFechaFinFin = "";

    $scope.limpioCaja = function () {
        $scope.pFechaIniIni = "";
        $scope.pFechaIniFin = "";
        $scope.txtNoLicitacion = "";
        $scope.pFechaFinIni = "";
        $scope.pFechaFinFin = "";

    }

    $scope.selecionaCheck = "";
    $scope.selecciona = function (item) {
        for (var i = 0; i < $scope.ConcursoSele.length; i++) {

            if ($scope.ConcursoSele[i].codproveedor == item.codproveedor) {

                $scope.selecionaCheck = item;
                $scope.idempresa = item;
            }
            else
                if ($scope.ConcursoSele[i].codproveedor != item.codproveedor)
                    $scope.ConcursoSele[i].isSele = false;

        }
    }

    function validate_fechaMayorQue(fechaInicial, fechaFinal) {

        var valuesStart = fechaInicial.split("/");
        var valuesEnd = fechaFinal.split("/");

        // Verificamos que la fecha no sea posterior a la actual
        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);
        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);

        if (dateStart >= dateEnd) {
            return 1;
        }

        return 0;
    }


    $scope.elimDoc = function (index) {
        $scope.concurso.documentos.splice(index, 1);
    }

    //Filtro de consultas
    $scope.filtroConsulta3 = function () {
        if ($scope.rdbCodigo == "1" && $scope.txtCodProveedor == "") {
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

    $scope.CargaConcursos = function () {

        var Fecha1 = ""; var Fecha2 = "";
        var FechaF1 = ""; var FechaF2 = "";

        if ($scope.rdbOpcion_ == "1") {

            if ($scope.pFechaIniIni == null || $scope.pFechaIniIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaIniFin == null || $scope.pFechaIniFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }
            if (validate_fechaMayorQue($scope.pFechaIniIni, $scope.pFechaIniFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }
            Fecha1 = $filter('date')($scope.pFechaIniIni, 'dd-MM-yyyy');
            Fecha2 = $filter('date')($scope.pFechaIniFin, 'dd-MM-yyyy');
        }
        if ($scope.rdbOpcion_ == "2") {

            if ($scope.pFechaFinIni == null || $scope.pFechaFinIni == "") {
                $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                return;
            }
            if ($scope.pFechaFinFin == null || $scope.pFechaFinFin == "") {
                $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                return;
            }
            if (validate_fechaMayorQue($scope.pFechaFinIni, $scope.pFechaFinFin)) {
                $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                return;
            }
            FechaF1 = $filter('date')($scope.pFechaFinIni, 'dd-MM-yyyy');
            FechaF2 = $filter('date')($scope.pFechaFinFin, 'dd-MM-yyyy');
        }

        if ($scope.rdbOpcion_ == "3") {
            if ($scope.txtNoLicitacion == "") {
                $scope.showMessage('I', 'Ingreso el numero Requisicion a consultar.');
                return;
            }
        }
        $scope.ConcursosGeneral = [];
        $scope.Concursos = [];
        $scope.myPromise = RequerimientoService.getConsultarRequerimientoAju(Fecha1, Fecha2, $scope.txtNoLicitacion, FechaF1, FechaF2).then(function (results) {

            if (results.data.success) {

                var aux = results.data.root[0];
                $scope.etiTotRegistros = aux.length;
                var aux1 = results.data.root[1];
                var aux2 = results.data.root[2];
                for (var i = 0; i < aux.length; i++) {
                    var tmp = {};
                    tmp.nombre = aux[i].nombre;
                    tmp.fe_empieza = aux[i].fe_empieza;
                    tmp.fe_exp = aux[i].fe_exp;
                    tmp.ho_exp = aux[i].ho_exp;
                    tmp.feRequerimiento = aux[i].feRequerimiento;
                    tmp.montoRequerimiento = aux[i].montoRequerimiento;
                    tmp.codRequerimiento = aux[i].codRequerimiento;
                    tmp.desRequerimiento = aux[i].desRequerimiento;
                    tmp.empRequerimiento = aux[i].empRequerimiento;

                    tmp.reqs = [];
                    for (var j = 0; j < aux1.length; j++) {
                        var tmpd = {};
                        if (aux[i].codRequerimiento == aux1[j].id) {
                            tmpd.id = aux1[j].id;
                            tmpd.descripcion = aux1[j].descripcion;
                            tmpd.archivo = aux1[j].archivo;
                            tmp.reqs.push(tmpd);
                        }
                    }
                    tmp.empresa = [];
                    for (var h = 0; h < aux2.length; h++) {
                        var tmpe = {};
                        if (aux[i].codRequerimiento == aux2[h].id) {
                            tmpe.id = aux2[h].id;
                            tmpe.empresa = aux2[h].empresa;
                            tmpe.monto = aux2[h].monto;
                            tmpe.codproveedor = aux2[h].codproveedor;
                            tmp.empresa.push(tmpe);
                        }
                    }
                    $scope.ConcursosGeneral.push(tmp);
                }

            }
            setTimeout(function () { $('#btnConsulta1').focus(); }, 100);
            setTimeout(function () { $('#rbtArtRef').focus(); }, 150);

        }, function (error) {
            $scope.MenjError = "Error de comunicación: AsignacionProveedorService.getProveedorEtiqList()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.reset = function () {

        $scope.idrequerimiento = "";
        $scope.idempresa = [];
        $scope.idempresa.id = "";
        $scope.idempresa.empresa = "";
        $scope.idempresa.monto = "";
        $scope.idempresa.codproveedor = "";
    }
    $scope.terminaLici = function (item) {
        $scope.accion = 'TL';
        $scope.seleLici = item;
        $('#idMensajeConfirmacion').modal('show');
        $scope.MenjConfirmacion = "La Licitación Pasará a un Estado de Terminada.";
    }

    $scope.CargaAplicantes = function (item, id) {

        $scope.idrequerimiento = id;
        $scope.ConcursoSele = item;

        $('#proveedoresModal').modal('show');
    }

    //open modal
    $scope.openPublicar = function () {

        $scope.esAplia = false;
        $scope.ReqsSele = [];
        var fecha = moment();
        $scope.concurso = { nombre: "lic_" + fecha.format('DDMMYYYY') + "_034", documentos: [] };

        for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
            if ($scope.ReqsEmpresa[i].isSele)
                $scope.ReqsSele.push($scope.ReqsEmpresa[i]);

        if ($scope.ReqsSele.length > 0)
            $('#concursoModal').modal('show');
        else {
            $scope.MenjError = "Debe seleccionar al menos un un requerimiento";
            $('#idMensajeError').modal('show');
        }
    }

    $scope.seleccionaTodos = function () {
        $scope.seleTodos = !$scope.seleTodos;

        if ($scope.seleTodos == false) {
            for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
                $scope.ReqsEmpresa[i].isSele = false;

            for (var i = 0; i < $scope.ReqsEmpresa_.length; i++)
                for (var j = 0; j < $scope.ReqsEmpresa_[i].ReqsEmpresa.length; j++)
                    $scope.ReqsEmpresa_[i].ReqsEmpresa[j].isSele = false;
        }
        else
            if ($scope.seleTodos == true) {
                for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
                    $scope.ReqsEmpresa[i].isSele = true;


                for (var i = 0; i < $scope.ReqsEmpresa_.length; i++)
                    for (var j = 0; j < $scope.ReqsEmpresa_[i].ReqsEmpresa.length; j++)
                        $scope.ReqsEmpresa_[i].ReqsEmpresa[j].isSele = true;
            }

    }

    $scope.CargaConcursos();

    //Carga Empresas
    function cargaEmpresas() {

        $scope.Empresas = [
            { idEmpresa: "1", descripcion: "Ingenio Valdez" },
            { idEmpresa: "1", descripcion: "La Universal" },
        ];
    }

    cargaEmpresas();

    $scope.guardaConcurso = function () {

        $scope.Concursos.push(
            { nombre: $scope.concurso.nombre, fe_empieza: $scope.concurso.fe_empieza, fe_exp: $scope.concurso.fe_exp, reqs: $scope.ReqsSele, estado: "A", version: 1 }
        );

        for (var i = 0; i < $scope.ReqsSele.length; i++)
            for (var j = 0; j < $scope.ReqsEmpresa.length; j++)
                if ($scope.ReqsSele[i].codRequerimiento == $scope.ReqsEmpresa[j].codRequerimiento)
                    $scope.ReqsEmpresa.splice(j, 1);

        $('#concursoModal').modal('hide');

        $scope.MenjError = "Operación Realizada con Éxito";
        $('#idMensajeOk').modal('show');
    }

    $scope.showAdjuntos = function (item) {
        $scope.liciSele = angular.copy(item);
        $('#documentosModal').modal('show');
        $scope.Documentos = item;
    };


    $scope.grabar = function () {
        debugger;
        if ($scope.accion == 'TL') {
            $scope.seleLici.estado = 'T';
            $scope.seleLici.version = $scope.seleLici.version + 1;
        }
        else
            if ($scope.accion == 'sele') {
                $scope.aceptaSele();
            }
    }

    $scope.openConf = function () {

        $scope.MenjConfirmacion = "¿Está seguro de adjudicar la licitación al proveedor seleccionado?";
        $('#idMensajeConfirmacion').modal('show');
        $scope.accion = "sele";
    }

    $scope.aceptaSele = function () {

        if ($scope.idempresa.codproveedor != "") {
            $scope.myPromise = RequerimientoService.getUpdateAjudicacion($scope.idempresa.codproveedor, $scope.idrequerimiento).then(function (results) {
                if (results.data.success) {
                    $('#proveedoresModal').modal('hide');
                    $scope.MenjError = "Licitación adjudicada";
                    $('#idMensajeOk').modal('show');
                    $scope.CargaConcursos();
                }
                else {
                    $scope.MenjError = "No se Pudo Actualizar la Licitacion.";
                    $('#idMensajeInformativo').modal('show');
                }

            }, function (error) {
            });
        } else {
            $scope.idrequerimiento = "";
            $scope.idempresa = [];
            $scope.idempresa.id = "";
            $scope.idempresa.empresa = "";
            $scope.idempresa.monto = "";
            $scope.idempresa.codproveedor = "";
        }
    }

    $scope.agregaDoc = function () {
        $scope.concurso.documentos.push({ nombre: "" });
    }

    $scope.openAmplia = function (item) {
        debugger;
        $scope.concurso = item;
        $scope.ReqsSele = angular.copy($scope.concurso.reqs);
        $scope.esAplia = true;

        $('#concursoModal').modal('show');
    }

    $scope.ampliaFecha = function () {
        $scope.concurso.version = $scope.concurso.version + 1;

        $('#concursoModal').modal('hide');

        $scope.MenjError = "Operación Realizada con Éxito";
        $('#idMensajeOk').modal('show');
    }

    $scope.ampliaLici = function () {
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
}]);

'use strict';
app.controller('ReqRequisitoController', ['$scope', '$location', 'RequerimientoService', '$sce', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', '$http', 'authService', function ($scope, $location, RequerimientoService, $sce, $cookies, ngAuthSettings, FileUploader, $filter, $http, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var Ruta = serviceBase + 'api/FileTransporte/UploadFile/?direccion=Uploads';
    var uploader = $scope.uploader = new FileUploader({
        url: Ruta
    });

    $scope.trix = '';

    $scope.report = {}
    $scope.report.id = "";
    $scope.report.viewDetailPath = "";
    $scope.pagesPedidos = [];
    $scope.etiTotRegistros = "";
    $scope.etiTotRegistrosLP = "";
    $scope.etiTotRegistrosHE = "";
    $scope.delay = 0;
    $scope.minDuration = 0;
    $scope.message = 'Por Favor Espere...';
    $scope.backdrop = true;
    $scope.promise = null;
    $scope.templateUrl = '';

    $scope.myPromise = null;
    $scope.isSaving = undefined;
    $scope.div1 = false;
    $scope.div2 = true;

    $scope.myFile = "";
    $scope.formData = {};
    //Variable de Grid
    $scope.GridTransporte = [];
    $scope.pagesCho = [];
    $scope.pageContentCho = [];

    $scope.pageArchivo = [];
    $scope.txtdescripcionajunto = "";

    $scope.Ruta = serviceBase + 'UploadedDocuments/Uploads/';

    $scope.file = null;
    $scope.GridTransporteP = [];
    $scope.pagesChoP = [];
    $scope.pageContentChoP = [];

    $scope.GridTransporteH = [];
    $scope.pagesChoH = [];
    $scope.pageContentChoH = [];

    $scope.gridapiTRP = {};
    $scope.gridapiTRPP = {};
    $scope.gridapiTRPH = {};

    $scope.baseEmpresa = [];
    $scope.baseCategoria = [];
    $scope.baseCategoriaBuscar = [];
    $scope.baseEmpresaBuscar = [];

    //fin Grid

    //Variable de Busquedas
    $scope.PorNumeroCed = 1;
    $scope.txtNumero = "";
    $scope.PorDatosNombreApellido = 1;
    $scope.txtPorNombre = "";
    $scope.txtPorApellido = "";
    $scope.PorEstadosTipo = 1;
    $scope.EstadoSolicitudTipo = [];

    //Fin Variable

    $scope.descripcion = "";
    $scope.habilitares = true;
    $scope.habilitar = true;
    $scope.traCho = {};
    $scope.traCho.Tipo = "1";
    $scope.traCho.id = "";
    $scope.traCho.txtmonto = "";
    $scope.traCho.txtrequerimiento = "";
    $scope.traCho.CodCategoria = "";
    $scope.traCho.txttiulo = "";
    $scope.traCho.CodEmpresa = "";
    $scope.traCho.descripcion = "";
    $scope.traCho.usuarioCreacion = authService.authentication.userName;

    $scope.txtrequerimientobuscar = "";

    $scope.selectedCategoria = "";
    $scope.selectedCategoriaBuscar = "";
    $scope.selectedEmpresa = "";
    $scope.selectedEmpresaBuscar = "";

    $scope.habilitar = true;


    $scope.idselgrid = "";
    $scope.btneliact = true;

    var ideliminar = "";
    var idestado = "";

    $scope.sortType = 'nombres'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';
    $scope.rutaDirectorio = "";
    //Fin Variable

    $scope.imagenurl = "";
    //Variable de Mensaje
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    //Fin Variable

    $scope.keyPress = function (eve) {
        if (eve.keyCode < 45 || eve.keyCode > 57) {
        }
    }

    $scope.printDivPage = function () {

        $('#idConfirmacionImprimir').modal('show');
    }
    $scope.printDiv = function (divName) {

        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;

        if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
            var popupWin = window.open('', '_blank', 'width=800,height=600,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no;orientation=horizontal;');
            popupWin.window.focus();
            popupWin.document.write('<!DOCTYPE html><html><head>' +
                '<link rel="stylesheet" type="text/css" href="style.css" />' +
                '</head><body onload="window.print()"><div class="reward-body">' + printContents + '</div></html>');
            popupWin.onbeforeunload = function (event) {
                popupWin.close();
                return '.\n';
            };
            popupWin.onabort = function (event) {
                popupWin.document.close();
                popupWin.close();
            }
        } else {
            var popupWin = window.open('', '_blank', 'width=800,height=600');
            popupWin.document.open();
            popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="style.css" /></head><body onload="window.print()">' + printContents + '</html>');
            popupWin.document.close();
        }
        popupWin.document.close();

        return true;
    }

    //Carga Tipo Identificacion
    $scope.okGrabar = function () {
        $scope.nuevo();
        $scope.habilitar = true;
    }

    //Carga Catalogo Tipo
    RequerimientoService.getEmpresa('EMP').then(function (results) {
        $scope.baseEmpresa = results.data.root[0];
        $scope.baseEmpresaBuscar = results.data.root[0];
    }, function (error) {
    });

    RequerimientoService.getCategoria('CAT').then(function (results) {
        $scope.baseCategoria = results.data.root[0];
        $scope.baseCategoriaBuscar = results.data.root[0];
    }, function (error) {
    });

    // $scope.getthefile();
    $scope.limpioCaja = function () {
        if ($scope.PorNumeroCed === "1") {
            $scope.txtNumero = "";
        }
        if ($scope.PorDatosNombreApellido === "1") {
            $scope.txtPorApellido = "";
        }
        if ($scope.PorDatosNombreApellido === "2") {
            $scope.txtPorNombre = "";
        }
        if ($scope.PorEstadosTipo === "1") {
            $scope.selectedItemTipo = "";
        }
    }

    function Limpiar() {

        $scope.habilitar = false;
        $scope.btneliact = true;
        $scope.traCho.Tipo = "1";
        $scope.traCho.id = "";
        $scope.traCho.txtmonto = "";
        $scope.traCho.txtrequerimiento = "";
        $scope.traCho.CodCategoria = "";
        $scope.traCho.CodEmpresa = "";
        $scope.traCho.txttiulo = "";

        idestado = '';

        $scope.selectedCategoria = "";
        $scope.selectedEmpresa = "";

        $scope.pageContentChoP = [];
        $scope.pageContentChoH = [];

        $scope.pageArchivo = [];

        $('#editor1').html("");
        $scope.trix = "";

        var rutanew = serviceBase + 'api/FileTransporte/UploadFile/?direccion=Uploads';

        uploader.clearQueue()
        $scope.uploader.url = rutanew;
    }

    $scope.nuevo = function () {
        Limpiar();
    }

    $scope.cancelar = function () {
        Limpiar();
        $scope.habilitar = true;
    }

    $scope.BuscarFiltro = function () {
        $scope.Consultar();

    }

    $scope.Confirmargrabar = function () {
        if ($scope.traCho.Tipo === "1") {
            $scope.MenjConfirmacion = "¿Está seguro de guardar la información?"
        }
        if ($scope.traCho.Tipo === "2") {
            $scope.MenjConfirmacion = "¿Está seguro de modificar la información?"
        }
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.selecionaComboEstado = function () {
        if ($scope.selectEstado.codigo == "B") {
            $scope.habilitar = false;
            $scope.selectBloqueo = ""
            $scope.traCho.txtFechaBloqueo = "";
            $scope.traCho.txtOrigenBloque = "";
        } else {
            $scope.habilitar = true;
            $scope.selectBloqueo = ""
            $scope.traCho.txtFechaBloqueo = "";
            $scope.traCho.txtOrigenBloque = "";
        }
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

    $scope.quitar = function (nomArchivo) {
        if (idestado == "Ingresado" || idestado == "") {
            var listaArchivos = $scope.uploader.queue;
            for (var i = 0; i < listaArchivos.length; i++) {
                var nomArchivo2 = $scope.uploader.queue[i]._file.name;
                if (nomArchivo == nomArchivo2) {
                    $scope.uploader.queue[i].remove();
                }
            }
            debugger;
            for (var i = 0; i < $scope.pageArchivo.length; i++) {
                if ($scope.pageArchivo[i].archivo == nomArchivo) break;

            }
            $scope.pageArchivo.splice(i, 1);
        }
    };

    $scope.aceptar = function () {
        $scope.Consultar();
        Limpiar();
        $scope.habilitar = true;
    }

    $scope.grabar = function () {
        $scope.isSaving = true;

        if ($scope.pageArchivo.length == 0) {
            $scope.MenjError = "Debe seleccionar al menos un archivo."
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.traCho.Tipo === "1") {

            if ($scope.traCho.txtrequerimiento == "") {
                $scope.MenjError = "Debe Ingresar fecha de Requisición."
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.selectedCategoria === null)
                $scope.traCho.CodCategoria = "";
            else
                $scope.traCho.CodCategoria = $scope.selectedCategoria.codEmpresa;

            if ($scope.selectedEmpresa === null)
                $scope.traCho.CodEmpresa = "";
            else
                $scope.traCho.CodEmpresa = $scope.selectedEmpresa.codEmpresa;

            var a = $('#editor1').html();

            if (a == "") {
                $scope.MenjError = "Debe ingresar descripción de la requisición";
                $('#idMensajeError').modal('show');
                return;
            }

            $scope.traCho.descripcion = a;

            var listaArchivos = $scope.uploader.queue;
            if (listaArchivos.length > 0) {
                uploader.uploadAll();          
            }

            var Fecha1 = $filter('date')($scope.traCho.txtrequerimiento, 'yyyy-dd-MM');

            $scope.myPromise = null;
            var tmp = "";
            for (var i = 0; i < $scope.pageArchivo.length; i++) {
                if (i == 0) {
                    tmp = $scope.pageArchivo[i].descripcion + ';' + $scope.pageArchivo[i].archivo;
                } else {
                    tmp = tmp + '|' + $scope.pageArchivo[i].descripcion + ';' + $scope.pageArchivo[i].archivo;
                }
            }

            $scope.myPromise = RequerimientoService.getGrabarRequerimiento(Fecha1, $scope.traCho.CodEmpresa, $scope.traCho.CodCategoria, $scope.traCho.txtmonto, $scope.traCho.descripcion, $scope.traCho.usuarioCreacion, tmp, $scope.traCho.txttiulo).then(function (results) {
                if (results.data.success) {
                    if ($scope.traCho.Tipo === "1") {
                        $scope.MenjError = "La requisición ha sido ingresada correctamente"
                        $('#idMensajeOk').modal('show');
                        $scope.traCho.Tipo = "1";
                        $scope.isSaving = false;
                    }
                }
                else {
                    if (results.data.msgError === "EXISTE")
                        if ($scope.traCho.Tipo === "1") {
                            $scope.MenjError = "El Requisicion ya exite verifique"
                            $('#idMensajeError').modal('show');
                            $scope.isSaving = false;
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
                    alert("Error en comunicación: " + errors.join(' '));
                });
        }
        if ($scope.traCho.Tipo === "2") {
            if (idestado == "Ingresado") {
                if ($scope.traCho.txtrequerimiento == "") {
                    $scope.MenjError = "Debe Ingresar fecha de Requisición."
                    $('#idMensajeError').modal('show');
                    return;
                }

                if ($scope.selectedCategoria === null)
                    $scope.traCho.CodCategoria = "";
                else
                    $scope.traCho.CodCategoria = $scope.selectedCategoria.codEmpresa;

                if ($scope.selectedEmpresa === null)
                    $scope.traCho.CodEmpresa = "";
                else
                    $scope.traCho.CodEmpresa = $scope.selectedEmpresa.codEmpresa;

                var a = $('#editor1').html();
                if (a == "") {
                    $scope.MenjError = "Ingresar descripción de Requisicion.";
                    $('#idMensajeInformativo').modal('show');
                    return;
                }

                $scope.traCho.descripcion = a;
                uploader.uploadAll();

                var Fecha1 = $filter('date')($scope.traCho.txtrequerimiento, 'dd/MM/yyyy');

                $scope.traCho.txtrequerimiento = Fecha1;

                var tmp = "";
                for (var i = 0; i < $scope.pageArchivo.length; i++) {
                    if (i == 0) {
                        tmp = $scope.pageArchivo[i].descripcion + ';' + $scope.pageArchivo[i].archivo;
                    } else {
                        tmp = tmp + '|' + $scope.pageArchivo[i].descripcion + ';' + $scope.pageArchivo[i].archivo;
                    }
                }

                $scope.myPromise = null;
                $scope.myPromise = RequerimientoService.getUpdateRequerimiento($scope.traCho.id, $scope.traCho.txtrequerimiento, $scope.traCho.CodEmpresa, $scope.traCho.CodCategoria, $scope.traCho.txtmonto, $scope.traCho.descripcion, $scope.traCho.usuarioCreacion, tmp, $scope.traCho.txttiulo).then(function (results) {
                    if (results.data.success) {
                        if ($scope.traCho.Tipo === "2") {
                            $scope.MenjError = "La Requisicion se modificado correctamente"
                            $('#idMensajeOk').modal('show');
                            $scope.nuevo();
                            $('.nav-tabs a[href="#RequerimientosRegistrados"]').tab('show');
                            $scope.traCho.Tipo = "1";
                        }
                    }
                    else {

                        $scope.MenjError = "No se pudo actualizar la Requisicion"
                        $('#idMensajeError').modal('show');
                        $scope.isSaving = false;
                    }
                },
                    function (error) {
                        var errors = [];
                        for (var key in error.data.modelState) {
                            for (var i = 0; i < error.data.modelState[key].length; i++) {
                                errors.push(error.data.modelState[key][i]);
                            }
                        }
                        alert("Error en comunicación: " + errors.join(' '));
                    });
            } else {
                $scope.MenjError = "Solo se puede modificar Requisición en estado Ingresado.."
                $('#idMensajeInformativo').modal('show');
            }
        }
    }

    $scope.Consultar = function () {

        $scope.btneliact = true;
        $scope.etiTotRegistros = "";
        var Fecha1 = $filter('date')($scope.txtrequerimientobuscar, 'dd/MM/yyyy');

        var categor = "";
        if ($scope.selectedCategoriaBuscar === null)
            categor = "";
        else
            categor = $scope.selectedCategoriaBuscar.codEmpresa;

        var empre = "";
        if ($scope.selectedEmpresaBuscar === null)
            empre = "";
        else
            empre = $scope.selectedEmpresaBuscar.codEmpresa;

        $scope.myPromise = null;
        $scope.GridTransporte = [];
        $scope.pageContentCho = [];
        $scope.myPromise = RequerimientoService.getConsultarRequerimiento(Fecha1, empre, categor).then(function (results) {
            if (results.data.success) {
                $scope.GridTransporte = results.data.root[0];
                $scope.etiTotRegistros = $scope.GridTransporte.length.toString();
                $scope.showPaginate = true;

                if ($scope.GridTransporte.length == 0) {
                    $scope.MenjError = "No hay datos para su consulta."
                    $('#idMensajeInformativo').modal('show');
                    return;
                }

            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
        }, function (error) {
        });

    }

    $scope.modi = function () {
        $scope.div1 = false;
        $scope.div2 = true;
    }

    $scope.Eliminar = function () {
        $scope.myPromise = null;
        $scope.myPromise = RequerimientoService.getEliminar(ideliminar).then(function (results) {
            if (results.data.success) {
                $scope.MenjError = "Requisicion Eliminado Correctamente."
                $('#idMensajeInformativo').modal('show');
                $scope.cancelar();
                $('.nav-tabs a[href="#RequerimientosRegistrados"]').tab('show');
                $scope.Consultar();
            }

        }, function (error) {
        });
    }

    $scope.descargarPdf = function (nomArchivo) {
        $scope.myPromise = RequerimientoService.LeePDFContratos(nomArchivo).then(function (results) {
            if (results.data.lSuccess) {
                console.log("Lectura PDF " + nomArchivo + " remota exitosa");

                RequerimientoService.getDescargarArchivos(nomArchivo).then(function (results) {
                    if (results.data != "") {
                        var file = new Blob([results.data], { type: 'application/pdf' });
                        saveAs(file, nomArchivo);
                    }

                }, function (error) {
                });
            }

        }, function (error) {
        });

    };

    $scope.EliminarGrid = function () {
        var id = ideliminar;
        if (id == undefined) {
            return;
        }
        if (idestado == "Ingresado") {
            $scope.MenjConfirmacion = "¿Está seguro de Eliminar el Requisición?"
            $('#idMensajeConfirmacionEliminar').modal('show');
        } else {
            $scope.MenjError = "Solo se puede Eliminar Requisición en estado Ingresado."
            $('#idMensajeInformativo').modal('show');
        }
    }

    $scope.SelecionarGrid = function (id) {

        Limpiar();

        if (id == undefined) {
            return;
        } else {

            $scope.btneliact = false;

            ideliminar = id.id;

            idestado = id.estado;
        }
        $('.nav-tabs a[href="#RequerimientosNuevos"]').tab('show');
        $scope.myPromise = null;
        $scope.myPromise = RequerimientoService.getSeleccionar(id.id).then(function (results) {
            if (results.data.success) {

                var retorno = {};
                retorno = results.data.root[0];

                $scope.pageArchivo = results.data.root[1];

                $scope.traCho.Tipo = "2";
                $scope.traCho.txtmonto = retorno[0].monto;
                $scope.traCho.txtrequerimiento = retorno[0].fecha;

                $scope.traCho.txttiulo = retorno[0].titulo;
                var index;
                var a = $scope.baseCategoria;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codEmpresa.trim() == retorno[0].categoria.trim())
                        $scope.selectedCategoria = a[index];
                }
                var a = $scope.baseEmpresa;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codEmpresa.trim() == retorno[0].empresa.trim())
                        $scope.selectedEmpresa = a[index];
                }

                $scope.traCho.descripcion = retorno[0].descripcion;
                $('#editor1').html(retorno[0].descripcion);

                $scope.traCho.id = retorno[0].id;

                if (idestado != "Ingresado") {
                    $scope.habilitar = true;
                } else {
                    $scope.habilitar = false;
                }
            }
        }, function (error) {
        });
    }

    $scope.loadrecordg = function () {
        $('#MantUsrAdminDialog').modal('show');
    }

    function selRowUsrsAdmin(rowId) {

        var ret = $scope.GridTransporte[rowId - 1];
        $scope.txtNombresPrimero1 = ret.tipoSolArticulo;
        $scope.txtNombresSegundo1 = ret.idSolicitud;
        $('#MantUsrAdminDialog').modal('show');

        return;
    }

    //Archivo
    uploader.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "pdf" || extension == "doc" || extension == "docx" || extension == "rtf" || extension == "jpg" || extension == "png" || extension == "xls")
                return true;
            else {
                return false;
            }
        }
    });

    uploader.filters.push({
        name: 'sizeFilter',
        fn: function (item, options) {
            var fileSize = item.size;
            fileSize = parseInt(fileSize) / (1024 * 1024);
            if (fileSize <= 5)
                return true;
            else {
                return false;
            }
        }
    });

    uploader.filters.push({
        name: 'itemResetFilter',
        fn: function (item, options) {
            if (this.queue.length < 5)
                return true;
            else {
                return false;
            }
        }
    });

    // CALLBACKS

    uploader.onWhenAddingFileFailed = function (item, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };

    uploader.onAfterAddingFile = function (fileItem) {
        $scope.txtarchivo = fileItem.file.name;
        var tmp = {};
        tmp.descripcion = $scope.txtdescripcionajunto;
        tmp.archivo = $scope.txtarchivo;
        $scope.pageArchivo.push(tmp);
        angular.element("input[type='file']").val(null);

        if ($scope.txtdescripcionajunto == "") {
            $scope.MenjError = "Ingrese una Descripcion del Archivo Adjunto ."
            $('#idMensajeInformativo').modal('show');
            $scope.quitar($scope.txtarchivo);
            $scope.txtdescripcionajunto = "";
        } else {
            $scope.txtdescripcionajunto = "";
        }
    };

    uploader.onSuccessItem = function (fileItem, response, status, headers) {
       
    };

    uploader.onErrorItem = function (fileItem, response, status, headers) {
    };

    uploader.onCancelItem = function (fileItem, response, status, headers) {
    };

    uploader.onAfterAddingAll = function (addedFileItems) {
        console.info('onAfterAddingAll', addedFileItems);
    };
    uploader.onBeforeUploadItem = function (item) {
        console.info('onBeforeUploadItem', item);
    };
    uploader.onProgressItem = function (fileItem, progress) {
        console.info('onProgressItem', fileItem, progress);
    };
    uploader.onProgressAll = function (progress) {
        console.info('onProgressAll', progress);
    };

    uploader.onCompleteItem = function (fileItem, response, status, headers) {
        console.info('onCompleteItem', fileItem, response, status, headers);
    };
    uploader.onCompleteAll = function () {
        console.info('onCompleteAll');
    };

    //Fin Archivo

    $scope.Consultar();
}
]);


