'use strict';
app.controller('signupController', ['$scope', '$location', '$timeout', 'authService', 'GeneralService', function ($scope, $location, $timeout, authService, GeneralService) {

    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.registration = {
        ruc:"",
        userName: "",
        password: "",
        confirmPassword: ""
    };


    $scope.TipoIdentificacion = [];
    $scope.EstadosSolicitud = [];
    $scope.ProveedorList = [];

    GeneralService.getCatalogo('tbl_EstadosSolicitudProveedor').then(function (results) {
        $scope.EstadosSolicitud = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_tipoIdentificacionSys').then(function (results) {
        $scope.TipoIdentificacion = results.data;
    }, function (error) {
    });

    $scope.message = "";

    $scope.PreSolicitud = function () {
        debugger;

        if ($scope.registration.ruc == '') {

            return;
        }

        SolicitudProveedor.getProveedorList("", "", $scope.registration.ruc, "", "").then(function (response) {
            debugger;
            $scope.ProveedorList = response.data;

            var DesEstado = '';
            var idx = 0;

            if ($scope.ProveedorList != null && $scope.ProveedorList.length > 0) {
                $scope.message = "Ya existe como proveedor";
                //$('#idMensajeError').modal('show');
            }
            else {
                SolicitudProveedor.getUltSolProveedorList("", "", $scope.registration.ruc, '', '').then(function (response) {
                    debugger;
                    if (response.data != null && response.data.length > 0) {

                        for (idx = 0; idx < $scope.EstadosSolicitud.length; idx++) {

                            if ($scope.EstadosSolicitud[idx].codigo == response.data[0].estado) {
                                DesEstado = $scope.EstadosSolicitud[idx].detalle;
                                break;
                            }
                        }


                        if (response.data[0].estado == "PA") {
                            $scope.message = "Su Precalificacion ya fue Aprobada. ";
                            //$('#idMensajeError').modal('show');
                            return;
                        }

                        if (response.data[0].estado != "IP" && response.data[0].estado != "PR") {
                            $scope.message = "Su solicitud esta. " + DesEstado;
                            //$('#idMensajeError').modal('show');
                            return;
                        }



                        if (response.data[0].estado == "IP" || response.data[0].estado == "PR") {



                            $scope.signUp();
                            //window.location = "/Proveedor/frmIngPreCalifica";
                        }
                    }
                    else {

                        $scope.signUp();
                        //window.location = "/Proveedor/frmIngPreCalifica";
                    }
                },

    function (err) {
        $scope.message = "Se ha producido el siguiente error: " + err.error_description;
    });

            }
        },
         function (err) {
             $scope.message = "Se ha producido el siguiente error: " + err.error_description;
         });
    };



    $scope.signUp = function () {

        authService.saveRegistration($scope.registration).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
            startTimer();

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.message = "Failed to register user due to:" + errors.join(' ');
         });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            //$location.path('/login');
            window.location = "/Account/FrmloginSol";
        }, 2000);
    }

}]);