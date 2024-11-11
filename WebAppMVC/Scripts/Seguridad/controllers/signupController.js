'use strict';
app.controller('signupController', ['$scope', '$location', '$timeout', 'authService', 'SolicitudProveedor', 'GeneralService', 'localStorageService', function ($scope, $location, $timeout, authService, SolicitudProveedor, GeneralService, localStorageService) {
    $scope.MenjError = "";
    $scope.savedSuccessfully = false;
    $scope.message = "";
    $scope.errorverifprov = false;
    $scope.messageP = 'Por Favor Espere...';
    $scope.myPromise = null;


    $scope.registration = {
        ruc: "",
        userName: "",
        password: "",
        confirmPassword: "",
        email: ""
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


    $scope.SignupSolicitante = function () {
        $scope.errorverifprov = false;
        $scope.registration.email = $scope.registration.userName;

        if ($scope.registration.ruc == '' || $scope.registration.userName == '') {
            $scope.showMessage('I', "Ingrese la Identificación");
            return;
        }

        if ($scope.registration.password == '') {
            $scope.showMessage('I', "Ingrese el password");
            return;
        }

        if ($scope.registration.confirmPassword == '') {
            $scope.showMessage('I', "Ingrese la confirmación de contraseña");
            return;
        }

        if ($scope.registration.password != $scope.registration.confirmPassword) {
            $scope.showMessage('I', "Las contraseñas no coinciden por favor verificar.");
            return;
        }

        localStorageService.remove('SolPreca');
        $scope.myPromise = null;
        $scope.myPromise = SolicitudProveedor.getProveedorList("", "", $scope.registration.ruc, "", "").then(function (response) {
            $scope.ProveedorList = response.data;
            debugger;
            var DesEstado = '';
            var idx = 0;

            if ($scope.ProveedorList != null && $scope.ProveedorList.length > 0) {
                $scope.MenjError = "Ya existe como proveedor";
                $scope.errorverifprov = true;
                $scope.showMessage('I', $scope.MenjError);
            }
            else {
                $scope.myPromise = SolicitudProveedor.getUltSolProveedorList("", "", $scope.registration.ruc, '', '').then(function (response) {
                    if (response.data != null && response.data.length > 0) {

                        for (idx = 0; idx < $scope.EstadosSolicitud.length; idx++) {
                            if ($scope.EstadosSolicitud[idx].codigo == response.data[0].estado) {
                                DesEstado = $scope.EstadosSolicitud[idx].detalle;
                                break;
                            }
                        }

                        if (response.data[0].estado == "PA") {
                            $scope.MenjError = "Su Precalificacion ya fue Aprobada. ";
                            $scope.errorverifprov = true;
                        }

                        if (response.data[0].estado != "IP" && response.data[0].estado != "PR") {
                            $scope.MenjError = "Su solicitud esta " + DesEstado;
                            $scope.errorverifprov = true;
                            return;
                        }

                        var _estado = response.data[0].estado;
                        var _idsolicitud = response.data[0].idSolicitud;

                        if (response.data[0].estado == "IP" || response.data[0].estado == "PR") {
                            var _estado = "IP";
                            if (response.data[0].estado == "PR") {
                                _idsolicitud = "";
                            }
                        }
                        debugger;
                        var someSessionObj = {
                            'tipoide': '',
                            'identificacion': $scope.registration.ruc,
                            'tipoidentificacion': 'RC',
                            'IdSolicitud': _idsolicitud,
                            'Estado': _estado,
                            'tiposolicitud': 'PR',
                            'indpage': 'ING',
                            'MenjError': $scope.MenjError
                        };

                        localStorageService.set('SolPreca', someSessionObj);
                        if (($scope.MenjError != "" && response.data[0].estado != "PA") && (response.data[0].estado != "IN")) {
                            if (response.data[0].estado != "PR") {
                                if (response.data[0].estado != "RE") {
                                    $scope.MenjError = ("Su solicitud esta en aprobación");
                                }
                                else {
                                    $scope.MenjError = ("Su solicitud ha sido rechazada");
                                }

                                $scope.errorverifprov = true;
                            }
                            else {
                                $scope.MenjError = ("Su solicitud ha sido rechazada");
                                $scope.errorverifprov = true;
                            }

                            $scope.showMessage('I', $scope.MenjError);
                        }
                        else {
                            $scope.errorverifprov = false;
                        }

                        if ($scope.errorverifprov == false)
                            $scope.signUp();
                    }
                    else {

                        //validar correo repetido
                        $scope.myPromise = SolicitudProveedor.getUltSolProveedorList("", "", $scope.registration.ruc, '', $scope.registration.userName).then(function (response) {
                            if (response.data != null && response.data.length > 0) {



                                $scope.MenjError = "Correo ya registrado con otro proveedor";
                                $scope.errorverifprov = true;
                                $scope.showMessage('I', $scope.MenjError);

                            } else {

                                var someSessionObj = {
                                    'tipoide': '',
                                    'identificacion': $scope.registration.ruc,
                                    'tipoidentificacion': 'R',
                                    'IdSolicitud': '',
                                    'Estado': 'IP',
                                    'tiposolicitud': 'PR',
                                    'indpage': 'ING',
                                    'MenjError': $scope.MenjError
                                };

                                localStorageService.set('SolPreca', someSessionObj);
                                $scope.errorverifprov = false;


                                if ($scope.errorverifprov == false)
                                    $scope.signUp();
                            }
                        },function (err) {
                                $scope.errorverifprov = true;
                                $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                                $scope.showMessage('E', $scope.MenjError);
                        });


                        
                    }
                    
                },

                    function (err) {
                        $scope.errorverifprov = true;
                        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                        $scope.showMessage('E', $scope.MenjError);
                    });
            }
            //if ($scope.errorverifprov == false)
            //    $scope.signUp();
        },
            function (err) {
                $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                $scope.showMessage('E', $scope.MenjError);
            });
    };

    $scope.validadorCedula = function () {

        var txtIdentificacion = $scope.registration.ruc;
        var numero = 0;
        var d1 = 0;
        var d2 = 0;
        var d3 = 0;
        var d4 = 0;
        var d5 = 0;
        var d6 = 0;
        var d7 = 0;
        var d8 = 0;
        var d9 = 0;
        var d10 = 0;

        var p1 = 0;
        var p2 = 0;
        var p3 = 0;
        var p4 = 0;
        var p5 = 0;
        var p6 = 0;
        var p7 = 0;
        var p8 = 0;
        var p9 = 0;



        var campos = txtIdentificacion;
        if (campos != undefined) {
            if (campos.length == 13) {
                numero = campos;
                var suma = 0;
                var residuo = 0;
                var pri = false;
                var pub = false;
                var nat = false;
                var numeroProvincias = 24;
                var modulo = 11;

                /* Verifico que el campo no contenga letras */
                var ok = 1;

                /* Aqui almacenamos los digitos de la cedula en variables. */
                d1 = numero.substr(0, 1);
                d2 = numero.substr(1, 1);
                d3 = numero.substr(2, 1);
                d4 = numero.substr(3, 1);
                d5 = numero.substr(4, 1);
                d6 = numero.substr(5, 1);
                d7 = numero.substr(6, 1);
                d8 = numero.substr(7, 1);
                d9 = numero.substr(8, 1);
                d10 = numero.substr(9, 1);

                /* El tercer digito es: */
                /* 9 para sociedades privadas y extranjeros */
                /* 6 para sociedades publicas */
                /* menor que 6 (0,1,2,3,4,5) para personas naturales */

                if (d3 == 7 || d3 == 8) {
                    alert('El tercer dígito ingresado es inválido');
                    return false;
                }

                /* Solo para personas naturales (modulo 10) */
                if (d3 < 6) {
                    nat = true;
                    p1 = d1 * 2; if (p1 >= 10) p1 -= 9;
                    p2 = d2 * 1; if (p2 >= 10) p2 -= 9;
                    p3 = d3 * 2; if (p3 >= 10) p3 -= 9;
                    p4 = d4 * 1; if (p4 >= 10) p4 -= 9;
                    p5 = d5 * 2; if (p5 >= 10) p5 -= 9;
                    p6 = d6 * 1; if (p6 >= 10) p6 -= 9;
                    p7 = d7 * 2; if (p7 >= 10) p7 -= 9;
                    p8 = d8 * 1; if (p8 >= 10) p8 -= 9;
                    p9 = d9 * 2; if (p9 >= 10) p9 -= 9;
                    modulo = 10;
                }

                /* Solo para sociedades publicas (modulo 11) */
                /* Aqui el digito verficador esta en la posicion 9, en las otras 2 en la pos. 10 */
                else if (d3 == 6) {
                    pub = true;
                    p1 = d1 * 3;
                    p2 = d2 * 2;
                    p3 = d3 * 7;
                    p4 = d4 * 6;
                    p5 = d5 * 5;
                    p6 = d6 * 4;
                    p7 = d7 * 3;
                    p8 = d8 * 2;
                    p9 = 0;
                }

                /* Solo para entidades privadas (modulo 11) */
                else if (d3 == 9) {
                    pri = true;
                    p1 = d1 * 4;
                    p2 = d2 * 3;
                    p3 = d3 * 2;
                    p4 = d4 * 7;
                    p5 = d5 * 6;
                    p6 = d6 * 5;
                    p7 = d7 * 4;
                    p8 = d8 * 3;
                    p9 = d9 * 2;
                }

                suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
                residuo = suma % modulo;


                /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
                if (pub == true) {
                    
                    /* El ruc de las empresas del sector publico terminan con 0001*/
                    if (numero.substr(9, 4) != '0001') {
                        alert('El ruc de la empresa del sector público debe terminar con 0001');
                        return false;
                    }
                }
                else if (pri == true) {
                    
                    if (numero.substr(10, 3) != '001') {
                        alert('El ruc de la empresa del sector privado debe terminar con 001');
                        return false;
                    }
                }

                else if (nat == true) {
                    
                    if (numero.length > 10 && numero.substr(10, 3) != '001') {
                        alert('El ruc de la persona natural debe terminar con 001');
                        return false;
                    }
                }
                return true;
            }
        }

    }
    $scope.rpHash = function (value) {
        var hash = 5381;
        for (var i = 0; i < value.length; i++) {
            hash = ((hash << 5) + hash) + value.charCodeAt(i);
        }
        return hash;
    };

    $scope.signUp = function () {

        var hashInput = $scope.rpHash($('#txtCAPTCHA')[0].value);
        if (hashInput != $('#txtCAPTCHA').realperson('getHash')) {
            $scope.showMessage('E', "El código captcha ingresado es incorrecto.");
            return;
        }
        $scope.myPromise = null;
        $scope.myPromise = authService.saveRegistration($scope.registration).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.UrlRedirect = "/Home/Index";
            $scope.showMessage('IA', "Usuario creado exitosamente, verifique su bandeja de correo y siga las instrucciones.");
        },
            function (response) {
                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                }

                if (response.status == "400") {
                    $scope.showMessage('I', "Su usuario ya se encuentra registrado.");
                } else {
                    $scope.showMessage('E', "Falló al registrar usuario.");
                }
            });
    };
    $scope.Redireccion = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            //$location.path('/login');

            window.location = $scope.UrlRedirect;
        }, 2000);

    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            //$location.path('/login');
            window.location = "/Home/Index";
        }, 2000);
    }

}]);