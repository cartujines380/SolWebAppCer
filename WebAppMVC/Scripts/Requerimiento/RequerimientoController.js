

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
    $scope.misApp = [];
    $scope.rdbOpcion = "T";
    $scope.oferta = {};
    $scope.oferta.documentos = [];
    $scope.Ruta = serviceBase + 'UploadedDocuments/Uploads/';

    $scope.idInputFile = "";
    $scope.idFile = "";

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

    function cargaCategorias() {

        $scope.Categorias = [];

        $scope.myPromise = RequerimientoService.getCata('C').then(function (results) {

            if (results.data.success) {
                $scope.Categorias = results.data.root[0];
            }

        }, function (error) {
            $scope.MenjError = "Error de comunicación: RequerimientoService.getCata()";
            $('#idMensajeError').modal('show');
        });
    }

    $scope.showAdjuntos = function (item) {
        $scope.liciSele = angular.copy(item);
        $('#documentosModal').modal('show');
        $scope.Documentos = angular.copy($scope.liciSele.documentos);
    };

    cargaCategorias();

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
                $scope.MenjError = "Ingrese el código del requerimiento";
                $('#idMensajeInformativo').modal('show');
                return;
            }

        }

        if ($scope.rdbOpcion == "C") {
            if ($scope.cboCategoria == null || $scope.cboCategoria == "" || angular.isUndefined($scope.cboCategoria)) {
                $scope.MenjError = "Seleccione una categoría";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            categoria = $scope.cboCategoria.id;
        }

        $scope.myPromise = null;
        $scope.etiTotRegistros = "";

        $scope.myPromise = RequerimientoService.getPubli($scope.rdbOpcion, Fecha1, Fecha2, $scope.txtCodRequerimiento, categoria, authService.authentication.CodSAP).then(function (results) {
            if (results.data.success) {
                $scope.Concursos = results.data.root[0];
                $scope.etiTotRegistros = $scope.Concursos.length.toString();
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

    $scope.elimDoc = function (index) {
        $scope.oferta.documentos.splice(index, 1);
    }

    $scope.okGrabarCargar = function () {
        if ($scope.accionAceptar == "EP")
            $scope.cargaMisApp();
        else
            if ($scope.accionAceptar == "PP") {
                $scope.CargaConsulta();
                $scope.cargaMisApp();
            }
    }

    $scope.openConfirma = function (item) {
        $scope.liciConfirma = angular.copy(item);
        $scope.accion = "PP";
        $scope.MenjConfirmacion = "¿Desea confirmar su participación en esta licitación?";
        $('#idMensajeConfirmacion').modal('show');
    }

    //open modal
    $scope.openPropuesta = function (item) {
        $scope.nomPubli = item.nomPubli;

        $scope.myPromise = RequerimientoService.getOferta(item.idPubli, authService.authentication.CodSAP).then(function (results) {
            if (results.data.success) {
                if (!angular.isUndefined(results.data.root[0][0]))
                    $scope.oferta = results.data.root[0][0];
                else {
                    $scope.oferta = {};
                    $scope.oferta.documentos = [];
                }
                $('#propuestaModal').modal('show');

            }

        }, function (error) {
        });
    }

    //Aplicar
    $scope.aplicar = function (item) {

        $scope.myPromise = RequerimientoService.setParticipa(item.idPubli, authService.authentication.CodSAP).then(function (results) {
            if (results.data.success) {

                $scope.MenjError = "Licitación aplicada correctamente";
                $('#idMensajeOk').modal('show');
                $scope.accionAceptar = "PP";
                return;
            }
            else {
                $scope.MenjError = "No hay resultado de la consulta.";
                $('#idMensajeInformativo').modal('show');
            }

        }, function (error) {
        });
    }

    //agrega documento
    $scope.agregaDoc = function () {
        $scope.oferta.documentos.push({ desc: "", idDoc: 0, archivo: "", idDoc: 0 });
    }

    //open modal
    $scope.guardaAplica = function () {
        debugger;
        var files = [];
        var index = 0;
        angular.forEach($scope.oferta.documentos, function (value) {
            if (value.idDoc == 0)
                files.push($("#adjunto" + index)[0].files[0]);
            index++;
        });

        $scope.myPromise = RequerimientoService.guardaOferta($scope.oferta, files).then(function (results) {
            debugger;
            if (results.data.success) {
                $('#propuestaModal').modal('hide');
                $scope.MenjError = "Oferta guardada correctamente";
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

    $scope.cargaMisApp = function () {

        $scope.myPromise = RequerimientoService.getParticipa(authService.authentication.CodSAP).then(function (results) {
            if (results.data.success) {
                $scope.misApp = results.data.root[0];
                $scope.etiTotMisApp = $scope.misApp.length;
                return;
            }
            else {
                $scope.MenjError = "No hay resultado de la consulta.";
                $('#idMensajeInformativo').modal('show');
            }

        }, function (error) {
        });
    }

    $scope.cargaMisApp();

    $scope.descargarPdf = function (nomArchivo) {
        var nomArchivo = nomArchivo.trim();
        var rutaTmp = "Uploads";
        $scope.myPromise = RequerimientoService.LeePDFContratos(rutaTmp, nomArchivo).then(function (results) {
            if (results.data.lSuccess) {
                console.log("Lectura PDF " + nomArchivo + " remota exitosa");

                RequerimientoService.getDescargarArchivos(rutaTmp, nomArchivo).then(function (results) {
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

    $scope.enviaAplica = function () {
        debugger;
        if ($scope.oferta.monto == 0) {
            $scope.MenjError = "Ingrese el monto de la propuesta";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if ($scope.oferta.tiempoEjecucion == 0) {
            $scope.MenjError = "Ingrese el tiempo de entrega";
            $('#idMensajeInformativo').modal('show');
            return false;
        }

        if ($scope.oferta.monto == null || angular.isUndefined($scope.oferta.monto) || $scope.oferta.monto.trim() == "") {
            $scope.MenjError = "Ingrese el monto de la propuesta";
            $('#idMensajeInformativo').modal('show');
            return false;
        }
        if ($scope.oferta.tiempoEjecucion == null || angular.isUndefined($scope.oferta.tiempoEjecucion) || $scope.oferta.tiempoEjecucion.trim() == "") {
            $scope.MenjError = "Ingrese el tiempo de entrega";
            $('#idMensajeInformativo').modal('show');
            return false;
        }

        var files = [];
        var index = 0;
        angular.forEach($scope.oferta.documentos, function (value) {
            if (value.idDoc == 0)
                files.push($("#adjunto" + index)[0].files[0]);
            index++;
        });

        $scope.myPromise = RequerimientoService.enviaOferta($scope.oferta, files).then(function (results) {
            if (results.data.success) {
                $('#enviaModal').modal('hide');
                $scope.MenjError = "Oferta enviada correctamente";
                $scope.accionAceptar = "EP";
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

    $scope.openEnvia = function () {

        $('#propuestaModal').modal('hide');
        $('#enviaModal').modal('show');
    }

    $scope.cargaProp = function (item) {

        $('#propuestaModal').modal('show');
        $scope.propuesta = angular.copy(item);
    }

    $scope.seleccionaTodos = function () {
        $scope.seleTodos = !$scope.seleTodos;

        if ($scope.seleTodos == false) {
            for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
                $scope.ReqsEmpresa[i].isSele = false;
        }
        else
            if ($scope.seleTodos == true) {
                for (var i = 0; i < $scope.ReqsEmpresa.length; i++)
                    $scope.ReqsEmpresa[i].isSele = true;
            }
    }

    $scope.guardaConcurso = function () {
        $scope.Concursos.push(
            { nombre: $scope.concurso.nombre, fe_empieza: $scope.concurso.fe_empieza, fe_exp: $scope.concurso.fe_exp, reqs: $scope.ReqsSele }
        );

        $('#concursoModal').modal('hide');
    }

    $scope.grabar = function () {
        if ($scope.accion == "PP")
            $scope.aplicar($scope.liciConfirma);
    }

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

}]);

app.controller('ReqRequisitoController', ['$scope', '$location', 'RequerimientoService', '$sce', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', '$http', 'authService', function ($scope, $location, RequerimientoService, $sce, $cookies, ngAuthSettings, FileUploader, $filter, $http, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var Ruta = serviceBase + 'api/FileTransporte/UploadFile/?direccion=prueba';
    var uploader = $scope.uploader = new FileUploader({
        url: Ruta
    });

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

    $scope.Ruta = serviceBase + 'UploadedDocuments/Uploads/';

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
    $scope.traCho.CodEmpresa = "";
    $scope.traCho.descripcion = "";
    $scope.traCho.usuarioCreacion = authService.authentication.userName;

    $scope.txtrequerimientobuscar = "";

    $scope.selectedCategoria = "";
    $scope.selectedCategoriaBuscar = "";
    $scope.selectedEmpresa = "";
    $scope.selectedEmpresaBuscar = "";

    $scope.habilitar = true;


    var ideliminar = "";

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

        $scope.traCho.Tipo = "1";
        $scope.traCho.id = "";
        $scope.traCho.txtmonto = "";
        $scope.traCho.txtrequerimiento = "";
        $scope.traCho.CodCategoria = "";
        $scope.traCho.CodEmpresa = "";


        $scope.selectedCategoria = "";
        $scope.selectedEmpresa = "";

        $scope.pageContentChoP = [];
        $scope.pageContentChoH = [];

        $scope.pageArchivo = [];

        $('#editor1').html("");

        var rutanew = serviceBase + 'api/FileTransporte/UploadFile/?direccion=prueba';

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
            $scope.MenjConfirmacion = "¿Está seguro de guardar la información?";
        }
        if ($scope.traCho.Tipo === "2") {
            $scope.MenjConfirmacion = "¿Está seguro de modificar la información?";
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

    $scope.grabar = function () {
        $scope.isSaving = true;
        if ($scope.traCho.Tipo === "1") {

            if ($scope.traCho.txtrequerimiento == "") {
                $scope.MenjError = "Debe Ingresar fecha de requerimiento.";
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
                $scope.MenjError = "Ingresar descripcion de Requerimiento.";
                $('#idMensajeInformativo').modal('show');
                return;
            }

            $scope.traCho.descripcion = a;

            uploader.uploadAll();

            var Fecha1 = $filter('date')($scope.traCho.txtrequerimiento, 'dd/MM/yyyy');

            $scope.traCho.txtrequerimiento = Fecha1;
            $scope.myPromise = null;
            var tmp = "";
            for (var i = 0; i < $scope.pageArchivo.length; i++) {
                if (i == 0) {
                    tmp = $scope.pageArchivo[i].archivo;
                } else {
                    tmp = tmp + '|' + $scope.pageArchivo[i].archivo;
                }

            }

            $scope.myPromise = RequerimientoService.getGrabarRequerimiento($scope.traCho.txtrequerimiento, $scope.traCho.CodEmpresa, $scope.traCho.CodCategoria, $scope.traCho.txtmonto, $scope.traCho.descripcion, $scope.traCho.usuarioCreacion, tmp).then(function (results) {
                if (results.data.success) {
                    if ($scope.traCho.Tipo === "1") {
                        $scope.MenjError = "Requerimiento ingresado correctamente";
                        $('#idMensajeGrabar').modal('show');
                        $scope.traCho.Tipo = "1";
                        $scope.isSaving = false;
                        $scope.Consultar();
                    }
                }
                else {
                    if (results.data.msgError === "EXISTE")
                        if ($scope.traCho.Tipo === "1") {
                            $scope.MenjError = "El Requerimiento ya exite verifique";
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
            if ($scope.traCho.txtrequerimiento == "") {
                $scope.MenjError = "Debe Ingresar fecha de requerimiento.";
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
                $scope.MenjError = "Ingresar descripcion de Requerimiento.";
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
                    tmp = $scope.pageArchivo[i].archivo;
                } else {
                    tmp = tmp + '|' + $scope.pageArchivo[i].archivo;
                }

            }
            $scope.myPromise = null;
            $scope.myPromise = RequerimientoService.getUpdateRequerimiento($scope.traCho.id, $scope.traCho.txtrequerimiento, $scope.traCho.CodEmpresa, $scope.traCho.CodCategoria, $scope.traCho.txtmonto, $scope.traCho.descripcion, $scope.traCho.usuarioCreacion, tmp).then(function (results) {
                if (results.data.success) {
                    if ($scope.traCho.Tipo === "2") {
                        $scope.MenjError = "Requerimiento modificado correctamente"
                        $('#idMensajeGrabar').modal('show');
                        $scope.nuevo();
                        $('.nav-tabs a[href="#RequerimientosRegistrados"]').tab('show');
                        $scope.traCho.Tipo = "1";
                        $scope.Consultar();
                    }
                }
                else {

                    $scope.MenjError = "No se pudo actualizar el requerimiento";
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
        }
    }

    $scope.quitarAdjunto = function (nomArchivo) {

        var listaArchivos = $scope.uploader.queue;
        for (var i = 0; i < listaArchivos.length; i++) {
            var nomArchivo2 = $scope.uploader.queue[i]._file.name;
            if (nomArchivo == nomArchivo2) {
                $scope.uploader.queue[i].remove();
            }
        }

        for (var i = 0; i < $scope.pageArchivo.length; i++) {
            if ($scope.pageArchivo[i].archivo == nomArchivo) break;

        }
        $scope.pageArchivo.splice(i, 1);



    };

    $scope.Consultar = function () {

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
        $scope.myPromise = RequerimientoService.getConsultarRequerimiento(Fecha1, empre, categor, authService.authentication.CodSAP).then(function (results) {
            if (results.data.success) {
                $scope.GridTransporte = results.data.root[0];
                $scope.etiTotRegistros = $scope.GridTransporte.length.toString();
                $scope.showPaginate = true;

                if ($scope.GridTransporte.length == 0) {
                    $scope.MenjError = "No hay datos para su consulta.";
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
                $scope.MenjError = "Requerimiento Eliminado Correctamente.";
                $('#idMensajeInformativo').modal('show');
                $scope.Consultar();
            }

        }, function (error) {
        });
    }

    $scope.EliminarGrid = function (id) {
        ideliminar = id;
        $scope.MenjConfirmacion = "¿Está seguro de Eliminar el Requerimiento?";
        $('#idMensajeConfirmacionEliminar').modal('show');
    }

    $scope.SelecionarGrid = function (id) {

        Limpiar();
        $('.nav-tabs a[href="#RequerimientosNuevos"]').tab('show');
        $scope.myPromise = null;
        $scope.myPromise = RequerimientoService.getSeleccionar(id).then(function (results) {
            if (results.data.success) {

                var retorno = {};
                retorno = results.data.root[0];

                $scope.pageArchivo = results.data.root[1];

                $scope.traCho.Tipo = "2";
                $scope.traCho.txtmonto = retorno[0].monto;
                $scope.traCho.txtrequerimiento = retorno[0].fecha;

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
                $scope.habilitar = true;
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
                alert('Invalid file format. Please select a file with pdf/doc/docs or rtf format  and try again.');
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
                alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
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
                alert('You have exceeded the limit of uploading files.');
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
        tmp.archivo = $scope.txtarchivo;
        $scope.pageArchivo.push(tmp);
        angular.element("input[type='file']").val(null);
    };

    uploader.onSuccessItem = function (fileItem, response, status, headers) {
    };

    uploader.onErrorItem = function (fileItem, response, status, headers) {
        alert('We were unable to upload your file. Please try again.');
    };

    uploader.onCancelItem = function (fileItem, response, status, headers) {
        alert('File uploading has been cancelled.');
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



