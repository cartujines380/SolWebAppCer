app.filter('startFrom', function () {
    return function (input, start) {
        start = +start; //parse to int
        if (input != undefined) {
            return input.slice(start);
        } else
            return;
    };
});

app.controller('GeneraContratoController', ['$scope', 'authService', 'ngAuthSettings', 'GeneraContratoService', 'GeneralService', 'FileUploader', '$http', '$filter', '$timeout', '$q', function ($scope, authService, ngAuthSettings, GeneraContratoService, GeneralService, FileUploader, $http, $filter, $timeout, $q) {

    //VARIABLES
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    //Variables de pagineo
    $scope.gridBandeja = [];
    $scope.pageContentCho = [];
    $scope.etiTotRegistros;
    $scope.currentPage = 0;
    $scope.pageSize = 10;

    $scope.botonesDeshabilitados = true;
    $scope.contratante = null;
    $scope.licitacionDet = null;
    $scope.listaTipoContrato = [];
    $scope.listaLineaNegocio = [];
    $scope.listaTipoServicio = [];
    $scope.listaPlazoSuscripcion = [];
    $scope.selectedTipoContratoCon = null;
    $scope.selectedLineaNegocioCon = null;
    $scope.selectedTipoServicioCon = null;
    $scope.selectedPlazoSuscripcionCon = null;
    $scope.rutaArchivo = "";

    $scope.Notificacion = [];
    $scope.Notificacion.ListaAdjuntos = [];
    $scope.Notificacion.Lista = [];
    $scope.nomUltFileUpload1 = "";
    $scope.rutaDirectorio = "";
    $scope.ListaAdjuntos = [];
    $scope.idAdquisicion = 0;
    $scope.nomValidaArchivo = "";
    $scope.estadoItem = 0;
    $scope.mostrarDescarga = true;
    $scope.mostrarCarga = true;
    $scope.mostrarAprobacion = true;
    $scope.mostrarEditar = false;
    $scope.roles = [];
    $scope.nombreArchivoCon = "";

    var adquisicionProcesar = null;
    var botonOrigen = "";
    
    $(document).ready(function () {
       
    });


    //FUNCIONES

    $scope.CargaConsulta = function (tipo) {

        var ruc = authService.authentication.ruc;

        if (tipo != "") {
            ruc = "T";
        }

        $scope.myPromise = GeneraContratoService.getLicitacionesProv(ruc).then(function (results) {
            if (results.data.cCodError == "0") {
                $scope.gridBandeja = results.data.root[0];
                $scope.etiTotRegistros = $scope.gridBandeja.length.toString();
            }
            else {
                console.log(results.data.cMsgError);
            }
        }, function (error) {
            console.log(error);
        });

    };


    $scope.grabarContrato = function () {
        botonOrigen = "GRABAR";

        $scope.MenjConfirmacion = "¿Está seguro de grabar la información?";
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.generarContrato = function () {
        botonOrigen = "GENERAR";

        $scope.MenjConfirmacion = "¿Está seguro de generar el contrato?";
        $('#idMensajeConfirmacion').modal('show');
    }

    //Funcion invocada desde el modal de confirmación
    $scope.grabar = function () {
        switch (botonOrigen) {
            case "GRABAR":
                $scope.grabarDatosContrato();
                break;
            case "GENERAR":
                $scope.generarContrato();
                break;
            case "APROBAR":
                $scope.aprobarContrato();
                break;
            default:
                break;
        }
    };

    $scope.grabarDatosContrato = function () {

        let codTipoContrato;
        let codLineaNegocio;
        let codTipoServicio;
        let codPlazoSuscripcion;

        if ($scope.selectedTipoContratoCon != null) {
            codTipoContrato = $scope.selectedTipoContratoCon.codigo;
        }

        if ($scope.selectedLineaNegocioCon != null) {
            codLineaNegocio = $scope.selectedLineaNegocioCon.codigo;
        }

        if ($scope.selectedTipoServicioCon != null) {
            codTipoServicio = $scope.selectedTipoServicioCon.codigo;
        }

        if ($scope.selectedPlazoSuscripcionCon != null) {
            codPlazoSuscripcion = $scope.selectedPlazoSuscripcionCon.codigo;
        }

        let contrato = {
            'IdContrato': $scope.licitacionDet.idContrato,
            'IdAdquisicion': adquisicionProcesar.idAdquisicion,
            'NombreLicitacion': adquisicionProcesar.nombre,
            'CodTipoContrato': codTipoContrato,
            'CodLineaNegocio': codLineaNegocio,
            'CodTipoServicio': codTipoServicio,
            'CodPlazoSuscripcion': codPlazoSuscripcion,
            'AdministradorContrato': $scope.licitacionDet.administradorContrato,
            'Usuario': ''
        };

        $scope.myPromise = GeneraContratoService.actualizaInfoContrato(contrato).then(function (results) {
            if (results.data.lSuccess) {
                $scope.limpiar();
                $scope.showMessage('S', 'Registro actualizado exitosamente.');
            } else {
                console.log(results.data.cMsgError);
                $scope.showMessage('E', 'No se pudo grabar la información del contrato.');
            }
        }, function (error) {
            console.log(error);
        });

    }

    $scope.descargarPdf = function (nomArchivo) {
        $scope.myPromise = GeneraContratoService.LeePDFContratos(nomArchivo).then(function (results) {
            if (results.data.lSuccess) {
                console.log("Lectura PDF " + nomArchivo + " remota exitosa");

                GeneraContratoService.getDescargarArchivos(nomArchivo).then(function (results) {
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

    $scope.cargarPdf = function (archivo) {
        $scope.nombreArchivoCon = "";
        $scope.nombreArchivoCon = archivo
    };

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var Ruta = serviceBase + 'api/Upload/UploadFile/?path=Contratos';
    ///CARGA DE ARCHIVO 
    var uploader2 = $scope.uploader2 = new FileUploader({
        url: Ruta
    });

    // FILTERS
    uploader2.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            item.name = $scope.nombreArchivoCon;
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();

            if (extension == "pdf")
                return true;
            else {

                $scope.MenjError = "El tipo de archivo para la notificación debe de ser de tipo PDF.";
                $('#idMensajeInformativo').modal('show');
                $('#upload2').val("");

                return false;
            }
        }
    });

    uploader2.filters.push({
        name: 'sizeFilter',
        fn: function (item, options) {
            item.name = $scope.nombreArchivoCon;
            var fileSize = item.size;
            fileSize = parseInt(fileSize) / (1024 * 1024);
            if (fileSize <= 2)
                return true;
            else {

                $scope.MenjError = "Archivo demasiado grande. Maximo 2 MG.";
                $('#idMensajeError').modal('show');
                if ($scope.Notificacion.Lista.length > 1) $scope.listaAdjplus = true;
                else $scope.listaAdjplus = false;

                $('#upload2').val("");

                return false;
            }
        }
    });

    uploader2.filters.push({
        name: 'itemResetFilter',
        fn: function (item, options) {
            item.name = $scope.nombreArchivoCon;

            return true;
            
        }
    });

    // CALLBACKS
    uploader2.onWhenAddingFileFailed = function (item, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };

    uploader2.onAfterAddingFile = function (fileItem) {
        $('#upload2').val("");
        fileItem.file.name = $scope.nombreArchivoCon;
        $scope.Notificacion.ListaAdjuntos.push(fileItem.file.name);

        $scope.Notificacion.Lista.push(
            { name: fileItem.file.name, iscomunicado: true });

        $scope.notGrabar = new Object();
        $scope.notGrabar.ListaAdjuntos = $scope.Notificacion.ListaAdjuntos;
        $scope.nomUltFileUpload1 = "";
        $scope.myPromise = uploader2.uploadAll();
        $scope.nomUltFileUpload1 = fileItem.file.name;

    };

    uploader2.onSuccessItem = function (fileItem, response, status, headers) {
        
        if ($scope.uploader2.progress == 100) {
            
            var ArrFilePDF =[$scope.Notificacion.ListaAdjuntos[0], "PDF/Contratos"];
            $scope.myPromise =
                GeneraContratoService.getUploadFileSFTP(ArrFilePDF).then(function (results) {
                    if (results.data.success) {
                        $scope.myPromise = uploader2.uploadAll();
                        $scope.MenjError = "Archivo cargado con éxito";
                        $('#idMensajeOk').modal('show');
                    }
                    else {
                        $scope.MenjError = "Error al realizar carga de archivo(s).";
                        $('#idMensajeError').modal('show');
                    }

                }, function (error) {

                    $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                    $('#idMensajeError').modal('show');
                });
            ;
        }
    };

    uploader2.onProgressAll = function (progress) {
        console.info('onProgressAll', progress);
    };
    uploader2.onCompleteAll = function () {
        console.info('onCompleteAll');
    };
    console.info('uploader', uploader2);

    $scope.firmasOk = function () {
        if ($scope.estadoItem  >= 3) {
            $scope.MenjError = "Contrato ya fue enviado";
            $('#idMensajeOk').modal('show');
            return;
        }

        $scope.MenjConfirmacion = "¿Está seguro de enviar el contrato?";
        $('#idMensajeConfirmacion').modal('show');
    };

    $scope.aprobar = function (id, estado, archivo) {
        botonOrigen = "APROBAR";
        $scope.idAdquisicion = id;
        $scope.estadoItem = estado;
        $scope.nomValidaArchivo = archivo;

        $scope.myPromise = GeneraContratoService.verificaFirma($scope.idAdquisicion, $scope.nomValidaArchivo, $scope.rutaArchivo).then(function (results) {
            if (results.data.cCodError == "0") {
                var firmas = "";

                if (results.data.root.length < 1) {
                    $scope.MenjError = "No posee las firmas necesarias para continuar";
                    $('#idMensajeError').modal('show');
                    return;
                }

                for (i = 0; i < results.data.root.length; i++) {
                    firmas = firmas + "<i class='fa fa-check-square-o' aria-hidden='true'></i> " +results.data.root[i] + "</br>";
                }

                $scope.MsnLinea1 = "Se ha(n) encontrado la(s) siguiente(s) firma(s) digital(es):";
                $scope.MsnLinea2 = archivo;
                document.getElementById("lblFirmas").innerHTML = firmas;
                $('#idMensajeFirmaOk').modal('show');
   
            }
            else {
                $scope.MenjError = results.data.cMsgError;
                $('#idMensajeError').modal('show');
                console.log(results.data.cMsgError);
                return;
            }
        }, function (error) {
            console.log(error);
        });

        
    };

    $scope.aprobarContrato = function () {
        $scope.myPromise = GeneraContratoService.aprobarContrato($scope.idAdquisicion, authService.authentication.Usuario, 3).then(function (results) {
            if (results.data.cCodError == "0") {

                $scope.MenjError = "El contrato fue enviado con éxito";
                $('#idMensajeOk').modal('show');

                $scope.CargaConsulta("");
                
            } else {
                $scope.MenjError = results.data.cMsgError;
                $('#idMensajeError').modal('show');
                console.log(results.data.cMsgError);
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.cargaCatalagos = function () {

        var promise = asyncAllCatalogos();
        promise.then(function (results) {
            $scope.listaTipoContrato = results[0].data;
            $scope.listaLineaNegocio = results[1].data;
            $scope.listaTipoServicio = results[2].data;
            $scope.listaPlazoSuscripcion = results[3].data;
        }, function (error) {

        });
    };

    function asyncAllCatalogos() {

        var defered = $q.defer();
        var promesas = [];
        promesas.push(GeneralService.getCatalogo('tbl_TipoContrato'));
        promesas.push(GeneralService.getCatalogo('tbl_LineaNegocio'));
        promesas.push(GeneralService.getCatalogo('tbl_TipoServicio'));
        promesas.push(GeneralService.getCatalogo('tbl_PlazoSuscripcion'));

        $scope.myPromise = $q.all(promesas).then(function (promesasRes) {
            defered.resolve(promesasRes);
        }, function (error) {
            defered.reject(error);
        }
        ).finally(function () {
        });

        return defered.promise;
    };

    $scope.$watch('$viewContentLoaded', function () {
        $timeout(function () {
            $scope.cargaCatalagos();
        }, 500);
    });

    $scope.limpiar = function () {
        $scope.botonesDeshabilitados = true;
        $scope.contratante = null;
        $scope.licitacionDet = null;
        $scope.selectedTipoContratoCon = null;
        $scope.selectedLineaNegocioCon = null;
        $scope.selectedTipoServicioCon = null;
        $scope.selectedPlazoSuscripcionCon = null;
        adquisicionProcesar = null;
    }

    //PAGINACION INI

    $scope.getData = function () {
        if ($scope.gridBandeja != undefined) {
            if ($scope.gridBandeja.length != "0") {
                return $filter('filter')($scope.gridBandeja, $scope.filtro)
            } else
                return [];
        }
    }

    $scope.numberOfPages = function () {
        var datos = $scope.getData();
        if (datos != undefined) {
            return Math.ceil($scope.getData().length / $scope.pageSize);
        } else
            return Math.ceil(0 / $scope.pageSize);
    }

    $scope.previuspg = function (id) {
        if (id == "0") {
            if ($scope.currentPage > 0) {
                $scope.currentPage = 0;
            }
        }
        if (id == "1") {
            if ($scope.currentPage > 0) {
                $scope.currentPage = $scope.currentPage - 1;
            }
        }
        if (id == "2") {
            if ($scope.currentPage < $scope.numberOfPages() - 1) {
                $scope.currentPage = $scope.currentPage + 1;
            }
        }
        if (id == "3") {
            if ($scope.currentPage < $scope.numberOfPages() - 1) {
                $scope.currentPage = $scope.numberOfPages() - 1;
            }
        }
    }

    //PAGINACION FIN

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


    //Impresion
    const { jsPDF } = window.jspdf;

    $scope.imprimir = function () {
        var doc = new jsPDF({ orientation: "landscape" }, '', 'A4');

        doc.addImage("../../Images/logo1.png", "JPEG", 5, 10, 48, 20);

        doc.setFontSize(30);
        doc.setTextColor(210, 0, 110);
        doc.text("Generación de Contratos", 10, 40);

        doc.setFontSize(12);
        doc.setTextColor(210, 0, 110);
        doc.text(FechaActual(), 10, 48);

        doc.setDrawColor(163, 26, 97);
        doc.setLineWidth(0.5);
        doc.line(10, 52, 285, 52);

        doc.setFontSize(12);
        doc.setTextColor(100);
        doc.setDrawColor(22, 15, 65);
        doc.setLineWidth(0.1);
        doc.table(10, 55, generateData(), headers,
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

        doc.save("Generación_Contratos.pdf");

    };

    var generateData = function () {
        var result = [];

        for (var i = 0; i < $scope.gridBandeja.length; i += 1) {
            var data = {
                Licitación: $scope.gridBandeja[i]['nombre'],
                Detalle: $scope.gridBandeja[i]['descripcion'] == "" ? " " : $scope.gridBandeja[i]['descripcion'],
                Identificación: $scope.gridBandeja[i]['ruc'],
                RazonSocial: $scope.gridBandeja[i]['razonSocial'],
                Contrato: $scope.gridBandeja[i]['numContrato'] == "" ? " " : $scope.gridBandeja[i]['numContrato'],
                Estado: $scope.gridBandeja[i]['desEstado']
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
                case "Detalle":
                    ancho = 70;
                    break;
                case "RazonSocial":
                    ancho = 80;
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
        "Licitación",
        "Detalle",
        "Identificación",
        "RazonSocial",
        "Contrato",
        "Estado"
    ]);

}
]);