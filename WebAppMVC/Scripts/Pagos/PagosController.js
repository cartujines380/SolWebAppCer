app.controller('PagosController', ['$scope', 'ngAuthSettings', 'PagosService', 'FileUploader', '$sce', 'authService', '$http', '$filter', function ($scope, ngAuthSettings, PagosService, FileUploader, $sce, authService, $http, $filter) {
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    $scope.estadoObser = "";
    $scope.accion = "T";
    $scope.GridPagos = [];

    $scope.ListadoPagos = [];
    $scope.allNotificaciones = [];
    $scope.PorFechas = 1;
    $scope.myPromise = null;
    $scope.Identificacion = authService.authentication.ruc;

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

        // Regexes and supporting functions are cached through closure
        return function (date, mask, utc) {
            var dF = dateFormat;

            // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
            if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }

            // Passing date through Date applies Date.parse, if necessary
            date = date ? new Date(date) : new Date;
            if (isNaN(date)) throw SyntaxError("invalid date");

            mask = String(dF.masks[mask] || mask || dF.masks["default"]);

            // Allow setting the utc argument via the mask
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

    // Internationalization strings
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

    // For convenience...
    Date.prototype.format = function (mask, utc) {
        return dateFormat(this, mask, utc);
    };

    $scope.Consultar = function () {

        var today = new Date();
        var dateString = today.format("ddmmyyyyHHMMss");

        var a1 = "";
        var a2 = "";

        var fecha1 = "";
        var fecha2 = "";


        if ($scope.PorFechas == 2) {
            if ($scope.txtfecdesde == "") {
                $scope.MenjError = "Ingrese fecha desde a consultar."
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if ($scope.txtfechasta == "") {
                $scope.MenjError = "Ingrese fecha hasta a consultar."
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if ($scope.txtfecdesde != "") {
                var parts1 = $scope.txtfecdesde.split('/');

                a1 = new Date(parts1[2], parts1[1] - 1, parts1[0]);

            }

            if ($scope.txtfechasta != "") {
                var parts2 = $scope.txtfechasta.split('/');

                a2 = new Date(parts2[2], parts2[1] - 1, parts2[0]);

            }

            fecha1 = $filter('date')(a1, 'yyyy-MM-dd');
            fecha2 = $filter('date')(a2, 'yyyy-MM-dd');

            if (fecha1 > fecha2) {
                $scope.MenjError = "Fecha Desde no puede ser mayor a Fecha Hasta"
                $('#idMensajeInformativo').modal('show');
                return;
            }

            $scope.accion = "F";
        }
        else {
            $scope.accion = "T";
            $('#txtfecdesde').val('');
            $('#txtfechasta').val('');
        }

        PagosService.getConsultaPagosFechas($scope.Identificacion, $scope.accion, fecha1, fecha2).then(function (results) {
            
            $scope.etiTotRegistros = "";
            
            if (results.data != undefined) {
                $scope.allNotificaciones = results.data.root[0];
                $scope.GridPagos = $scope.allNotificaciones;
                $scope.etiTotRegistros = $scope.GridPagos.length;
            }
            if ($scope.etiTotRegistros == "0") {
                $scope.MenjError = 'No existen datos para mostrar.';
                $('#idMensajeInformativo').modal('show');
            }
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            setTimeout(function () { $('#cargaPagos').focus(); }, 150);
        }, function (error) {
            $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
            $('#idMensajeError').modal('show');
        });
    };

    //Carga lista de notificaciones registradas
    $scope.ConsultaPagos = function (estado) {

        PagosService.getConsultaPagos($scope.Identificacion,estado).then(function (results) {
            if (results.data.success) {
                $scope.allNotificaciones = results.data.root[0];
                $scope.GridPagos = $scope.allNotificaciones;
                $scope.etiTotRegistros = $scope.GridPagos.length;
            }
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            setTimeout(function () { $('#cargaPagos').focus(); }, 150);
        }, function (error) {
            $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
            $('#idMensajeError').modal('show');
        });

    };

    const { jsPDF } = window.jspdf;

    $scope.ImprimeConPagos = function () {
        var doc = new jsPDF({ orientation: "landscape" }, '', 'A4');

        doc.addImage("../../Images/logo1.png", "JPEG", 5, 10, 48, 20);

        doc.setFontSize(30);
        doc.setTextColor(210, 0, 110);
        doc.text("Detalle de Pagos", 10, 40);

        doc.setFontSize(12);
        doc.setTextColor(210, 0, 110);
        doc.text(FechaActual(), 10, 45);

        doc.setDrawColor(163, 26, 97);
        doc.setLineWidth(0.5);
        doc.line(10, 48, 285, 48);

        doc.setFontSize(12);
        doc.setTextColor(100);
        doc.setDrawColor(22, 15, 65);
        doc.setLineWidth(0.1);
        doc.table(10, 50, generateData(), headers,
            {
                autoSize: false,
                headerBackgroundColor: '#006865',
                headerTextColor: '#FFF',
                pageSize: 15,
                margins: {
                    top: 15,
                    bottom: 15,
                    left: 20
                }
            });

        doc.save("Consulta_de_Pagos_" + $scope.Identificacion + ".pdf");

    };

    var generateData = function () {
        var result = [];

        for (var i = 0; i < $scope.GridPagos.length; i += 1) {
            var data = {
                Identificación: $scope.GridPagos[i]['identificacion'],
                Proveedor: $scope.GridPagos[i]['nomComercial'],
                Factura: $scope.GridPagos[i]['factura'] == "" ? " " : $scope.GridPagos[i]['factura'],
                FormaPago: $scope.GridPagos[i]['formaPago'],
                FechaPago: $scope.GridPagos[i]['fechaPago'],
                Valor: $scope.GridPagos[i]['valor'],
                Detalle: $scope.GridPagos[i]['detalle'] == "" ? " " : $scope.GridPagos[i]['detalle']
            }
            data.N = (i + 1).toString();
            result.push(Object.assign({}, data));
        }

        return result;
    };

    function FechaActual() {
        var meses = new Array("Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
        var dias = new Array("Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado");
        var fecha = new Date();
        return dias[fecha.getDay()] + ", " + fecha.getDate() + " de " + meses[fecha.getMonth()] + " de " + fecha.getFullYear();
    }

    function createHeaders(keys) {
        var result = [];
        var ancho = 0;
        for (var i = 0; i < keys.length; i += 1) {
            switch (keys[i]) {
                case "N":
                    ancho = 15;
                    break;
                case "Proveedor":
                    ancho = 60;
                    break;
                case "FormaPago":
                    ancho = 40;
                    break;
                case "FechaPago":
                    ancho = 38;
                    break;
                case "Detalle":
                    ancho = 65;
                    break;
                default:
                    ancho = 50;
            }

            result.push({
                id: keys[i],
                name: keys[i],
                prompt: keys[i],
                width: ancho,
                align: "center",
                padding: 0,
                fontSize: 8,
                top: 10
            });
        }
        return result;
    }

    var headers = createHeaders([
        "N",
        "Identificación",
        "Proveedor",
        "Factura",
        "FormaPago",
        "FechaPago",
        "Valor",
        "Detalle"
    ]);

}]);
