

app.controller('frmReporteBI', ['$scope', 'authService', '$q', '$timeout', 'frmReporteBIService', function ($scope, authService, $q, $timeout, frmReporteBIService) {




    //Inicializacion de variables
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.idAplicacion = "";
    $scope.idWorkStation = "";
    $scope.idReporte = "";
    $scope.param = "";

    $scope.param = location.search;

    $scope.cargaDashBoard = function (accessToken, embedUrl, embedReportId, codproveedor) {
        var codproveedorAut = $scope.authentication.CodSAP;
        // Get models. models contains enums that can be used.
        var models = window['powerbi-client'].models;



        const filter = {
            $schema: "http://powerbi.com/product/schema#basic",
            target: {
                table: "DatosMaestros",
                column: "Proveedor"
            },
            logicalOperator: "OR",
            operator: "In",
            values: [codproveedor]
        };

        const filter2 = {
            $schema: "http://powerbi.com/product/schema#basic",
            target: {
                table: "Participacion",
                column: "CodProveedor"
            },
            logicalOperator: "OR",
            operator: "In",
            values: [codproveedor]
        };

        

        var config = {
            type: 'report',
            tokenType: models.TokenType.Embed,
            accessToken: accessToken,
            embedUrl: embedUrl,
            id: embedReportId,
            permissions: models.Permissions.All,
            filters: [filter, filter2],
            settings: {
                filterPaneEnabled: false,
                navContentPaneEnabled: true
            }
        };

        // Get a reference to the embedded report HTML element
        var reportContainer = $('#embedContainer')[0];


        $("#noRLSdiv").show();

        // Embed the report and display it within the div container.
        var report = powerbi.embed(reportContainer, config);
        //report.setFilters([filter])


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
        } else if (tipo == 'MF') {
            $('#idMensajeOkf').modal('show');
        }
    }


    $("#btnMsjError").click(function () {
        window.parent.close();
    });


    //
    $scope.$watch('$viewContentLoaded', function () {

        $timeout(function () {
            var value = $scope.param.split('=')[1];

            $scope.cargaParametrosBI(value);
        }, 1500);
    });



    //Carga todos los catalogos asincronicamente
    $scope.cargaParametrosBI = function (report) {

        var promise = asyncAllParametros(report);
        promise.then(function (results) {
            //Mensajes del sistema


            if (results[0].data.lSuccess) {
                $scope.ParametrosBI = results[0].data.root[0];


                var accessToken = $scope.ParametrosBI.embedToken.token;
                // Read embed URL from Model

                var embedUrl = $scope.ParametrosBI.embedUrl;
                // Read report Id from Model

                var embedReportId = $scope.ParametrosBI.id;
                var codproveedor = zfill($scope.authentication.CodSAP, 10);
                $scope.cargaDashBoard(accessToken, embedUrl, embedReportId, codproveedor);
            }
            else {

                if (results[0].data.cMsgError != "") {
                    $scope.showMessage('E', results[0].data.cMsgError);
                }
                else {
                    $scope.showMessage('E', "Se ha producido un error inesperado al cargar repotes.");
                }


            }



        }, function (error) {

        });
    };

    //Agregar todos los catalogos necesarios
    function asyncAllParametros(report) {

        var defered = $q.defer();
        var promesas = [];
        promesas.push(frmReporteBIService.getConsToken(report));

        $scope.myPromise = $q.all(promesas).then(function (promesasRes) {
            defered.resolve(promesasRes);
        }, function (error) {
            defered.reject(error);
        }
        ).finally(function () {
        });

        return defered.promise;
    }

    function zfill(number, width) {
        var numberOutput = Math.abs(number); /* Valor absoluto del número */
        var length = number.toString().length; /* Largo del número */
        var zero = "0"; /* String de cero */

        if (width <= length) {
            if (number < 0) {
                return ("-" + numberOutput.toString());
            } else {
                return numberOutput.toString();
            }
        } else {
            if (number < 0) {
                return ("-" + (zero.repeat(width - length)) + numberOutput.toString());
            } else {
                return ((zero.repeat(width - length)) + numberOutput.toString());
            }
        }
    }





    $scope.authentication = authService.authentication;


    if (!$scope.authentication.isAuth) {

        window.parent.close();
        window.location.href = '/Home/Index';
    }
    else {
        //validar ROL estadistico
        var existe = false;
        var listaRoles = $scope.authentication.roles;
        for (var i = 0; i < listaRoles.length; i++) {
            if (listaRoles[i] == '4001') {
                var existe = true;
                break;
            }

        }
        //if (!existe) {
        //    alert('no es estadistico');
        //    window.parent.close();
        //    window.location.href = '/Notificacion/frmVisualizaNotificaciones';
        //}
    }
}
]);