app.controller('frmReporteRolesController', ['$scope', 'SeguridadService', '$filter', function ($scope, SeguridadService, $filter) {

    $scope.PorFechas = 1;
    $scope.pagesUsrsAdmin = [];

    $scope.txtfecdesde = "";
    $scope.txtfechasta = "";

    $scope.pgcDgUsrsAdmin = [];
    $scope.pageContentCho = [];
    $scope.pagesCho = [];
    $scope.SolProveedor = [];

    $scope.init = function (Valor) {
        var opcion = document.getElementById("hidOpcion").value;
        $scope.opcion = opcion;
    }

    $scope.Consultar = function () {
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
        }

        var opcion = document.getElementById("hidOpcion").value;

        $scope.myPromise = null;
        $scope.myPromise = SeguridadService.getConsultaReporteRol(fecha1, fecha2, opcion).then(function (results) {
            $scope.etiTotRegistros = "";

            if (results.data != undefined) {

                if (results.data.success) {
                    $scope.SolProveedor = results.data.root[0];
                    if ($scope.SolProveedor.length < 1) {
                        $scope.showMessage('I', 'No existen datos para mostrar.');
                    }
                    else {
                        $scope.etiTotRegistros = $scope.SolProveedor.length.toString();
                    }

                }
                else {
                    $scope.SolProveedor = [];
                    $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
                }

            }
            if ($scope.etiTotRegistros == "0") {
                $scope.MenjError = 'No existen datos para mostrar.';
                $('#idMensajeInformativo').modal('show');
            }
            setTimeout(function () { $('#btnConsultaini').focus(); }, 100);
            setTimeout(function () { $('#rbtTodasFechas').focus(); }, 150);

        }, function (error) {
            $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
            $('#idMensajeError').modal('show');
        });

    }

    //Generar Reportes
    $scope.exportar = function (Id, tipoSol) {
        //debugger;
        var opcion = document.getElementById("hidOpcion").value;

        if ($scope.SolProveedor == null || $scope.SolProveedor.length == 0)
        {
            $scope.showMessage('I', 'No existen datos para generar el reporte.');
            return;
        }

        var FileName = "";
        switch (opcion) {
            case "ReporteRol":
                FileName = "ReporteRol_"
                break;
            case "AccesoRol":
                FileName = "ReporteAccesoRol_"
                break;
            default:
                FileName = "ReporteLogAcceso_"
        }

        $scope.myPromise =
            SeguridadService.getGeneraReporteRol($scope.SolProveedor, opcion).then(function (response) {
                //debugger;
                if (response.data != "") {
                    var file;
                    var file = new Blob([response.data], {
                        type: 'application/xls'
                    });
                    var Fecha = formatDate();
                    saveAs(file, FileName + Fecha + ".xls");
                }
                else {

                    $scope.MenjError = "Error al generar reporte.";
                    $('#idMensajeError').modal('show');
                    return;
                }
            },
                function (err) {
                    $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                    $('#idMensajeError').modal('show');
                });
        ;
    }

    $scope.AuditarLogAcceso = function (trx) {
        $scope.myPromise = SeguridadService.verificarTransaccion(trx).then(function (results) { }, function (error) { });
    };


    function formatDate() {
        var date = new Date();
        var dia = padTo2Digits(date.getDay());
        var mes = padTo2Digits(date.getMonth());
        var anio = padTo2Digits(date.getFullYear());

        var hora = date.getHours();
        var minuto = date.getMinutes();

        var DateString = anio + mes + dia + "_" + hora + minuto;
        return DateString;
    }

    function padTo2Digits(num) {
        return num.toString().padStart(2, '0');
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
    

    setTimeout(function () { $('#btnConsultaini').focus(); }, 150);
    setTimeout(function () { angular.element('#btnConsultaini').trigger('click'); }, 250);

}]);
