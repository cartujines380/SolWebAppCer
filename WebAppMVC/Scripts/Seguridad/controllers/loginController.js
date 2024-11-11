'use strict';
app.controller('loginController', ['$scope', '$location', '$http', 'authService', 'ngAuthSettings', 'SolicitudProveedor', 'GeneralService', '$cookies', 'localStorageService', '$filter', function ($scope, $location, $http, authService, ngAuthSettings, SolicitudProveedor, GeneralService, $cookies, localStorageService, $filter) {

    $scope.message = '';
    $scope.messageP = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.TipoIdentificacion = [];
    $scope.EstadosSolicitud = [];
    $scope.ProveedorList = [];
    $scope.Sociedad = "";
    $scope.BodegaDatos = [];
    GeneralService.getCargaSociedad('7').then(function (results) {
        $scope.BodegaDatos = results.data.root[0];
        setTimeout(function () { $scope.inicializacombo(); }, 100);
    }, function (error) {
    });

       

    GeneralService.getCatalogo('tbl_EstadosSolicitudProveedor').then(function (results) {
        $scope.EstadosSolicitud = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_tipoIdentificacionSys').then(function (results) {
        $scope.TipoIdentificacion = results.data;
    }, function (error) {
    });

    $scope.MenjError = "";

    $scope.inicializacombo = function () {
        $("#idselectsociedad").val($("#idselectsociedad option:eq(1)").val());
    };

    $scope.PreCalifica = function () {


        if (authService.authentication.ruc == '') {
            return;
        }


        localStorageService.remove('SolPreca');

        $scope.myPromise = SolicitudProveedor.getProveedorList("", "", authService.authentication.ruc, "", "").then(function (response) {
            $scope.ProveedorList = response.data;

            var DesEstado = '';
            var idx = 0;

            if ($scope.ProveedorList != null && $scope.ProveedorList.length > 0) {
                $scope.MenjError = "Ya existe como proveedor";
                $scope.showMessage('E', $scope.MenjError);
            }
            else {
                $scope.myPromise = SolicitudProveedor.getUltSolProveedorList("", "", authService.authentication.ruc, '', '').then(function (response) {

                    if (response.data != null && response.data.length > 0) {

                        for (idx = 0; idx < $scope.EstadosSolicitud.length; idx++) {

                            if ($scope.EstadosSolicitud[idx].codigo == response.data[0].estado) {
                                DesEstado = $scope.EstadosSolicitud[idx].detalle;
                                break;
                            }
                        }


                        if (response.data[0].estado == "PA") {
                            $scope.MenjError = "Su Precalificacion ya fue Aprobada. ";

                        }else
                        {
                            if (response.data[0].estado != "IP" && response.data[0].estado != "PR" && response.data[0].estado != "DP" && response.data[0].estado != "IN") {
                                $scope.MenjError = "Su solicitud se encuentra en estado " + DesEstado;
                                $scope.showMessage('I', $scope.MenjError);
                                return;
                            }
                        }
                        var _estado = response.data[0].estado;
                        var _idsolicitud = response.data[0].idSolicitud;

                        if (response.data[0].estado == "IP" || response.data[0].estado == "PR") {

                            var _estado = "IP";
                            if (response.data[0].estado == "PR") {

                            }


                        }

                        var someSessionObj = {

                            'tipoide': '',
                            'identificacion': authService.authentication.ruc,
                            'tipoidentificacion': 'RC',
                            'IdSolicitud': _idsolicitud,
                            'Estado': _estado,
                            'tiposolicitud': 'PR',
                            'indpage': 'ING',
                            'MenjError': $scope.MenjError



                        };

                        localStorageService.set('SolPreca', someSessionObj);
                        if (($scope.MenjError != "" && response.data[0].estado != "PA") && (response.data[0].estado != "IN") && (response.data[0].estado != "IP") && (response.data[0].estado != "DP") && (response.data[0].estado != "PR")) {

                            if (response.data[0].tipoSolicitud == 'NU') {
                                if (response.data[0].estado != "PR") {

                                    if (response.data[0].estado != "RE") {
                                        $scope.MenjError = ("Su solicitud esta en Aprobación");
                                     
                                    }
                                    else {
                                        $scope.MenjError = ("Su solicitud ha sido rechazada");  
                                    }

                                }
                                else {
                                    $scope.MenjError = ("Su solicitud ha sido rechazada");

                                }

                                $scope.UrlRedirect = "../Home/Index";
                                $scope.showMessage('R', $scope.MenjError);
                            }
                            else {
                                if (response.data[0].tipoSolicitud == 'EP') {
                                    console.log("EP");
                                }

                            }


                        }
                        else {

                            if (response.data[0].tipoSolicitud == 'PR') {
                                window.location = "../Proveedor/frmIngPreCalifica";
                            }
                            if (response.data[0].tipoSolicitud == 'NU') {
								localStorageService.remove('SolProv');
                                localStorageService.remove('SolPreca');
                                var someSessionObj = {

                                    'tipoide': '',
                                    'identificacion': authService.authentication.ruc,
                                    'tipoidentificacion': 'R',
                                    'IdSolicitud': _idsolicitud,
                                    'Estado': _estado,
                                    'tiposolicitud': 'NU',
                                    'indpage': 'ING',
                                    'MenjError': $scope.MenjError
                                    
                                };

                                localStorageService.set('SolProv', someSessionObj);
                                window.location = "../Proveedor/frmSolictud";
                            }

                        }


                    }
                    else {
                        var someSessionObj = {

                            'tipoide': '',
                            'identificacion': authService.authentication.ruc,
                            'tipoidentificacion': 'RC',

                            'IdSolicitud': '',
                            'Estado': 'IP',
                            'tiposolicitud': 'PR',
                            'indpage': 'ING',
                            'MenjError': $scope.MenjError

                        };


                        localStorageService.set('SolPreca', someSessionObj);

                        window.location = "../Proveedor/frmIngPreCalifica";
                        
                    }
                },

                function (err) {
                    $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                    $scope.showMessage('R', $scope.MenjError);
                });

            }
        },
         function (err) {
             $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
             $scope.showMessage('E', $scope.MenjError);

         });
    };


    $scope.loginData = {
        isAuth: false,
        ruc: "",
        usernamefrm: "",
        useRefreshTokens: false,
        userRetreived: false,
        firstName: '',
        lastName: '',
        email: '',
        roles: [],
        tokenuser: "",
        IdParticipante: "",
        NombreParticipante: "",
        NombreParticipante22: "",
        IdentParticipante: "",
        Estado: "",
        PS_Token: "",
        EsClaveNuevo: "",
        EsClaveCambio: "",
        EsClaveBloqueo: "",
        esEtiqueta: true,
        Sociedad:""
    };
    $scope.rpHash = function (value) {
        var hash = 5381;
        for (var i = 0; i < value.length; i++) {
            hash = ((hash << 5) + hash) + value.charCodeAt(i);
        }
        return hash;
    };
    $scope.login = function (Source) {
        $scope.message = '';

      

        if ($scope.loginData.ruc == '' || angular.isUndefined($scope.loginData.ruc) == true) {
            $scope.message = "Ingrese el RUC.";
            $scope.showMessage('I', $scope.message);
            return;
        }

        if ($scope.loginData.usernamefrm == '') {
            $scope.message = "Ingrese el Usuario.";
            $scope.showMessage('I', $scope.message);
            return;
        }

        if ($scope.loginData.password == null || $scope.loginData.password == '') {
            $scope.message = "Ingrese la Contraseña.";
            $scope.showMessage('I', $scope.message);
            return;
        }
        if (validacedula() == false) {
            return;
        }
        $scope.loginData.sl = Source;
        $scope.loginData.Sociedad = "Null";
        $scope.myPromise = authService.login($scope.loginData).then(function (response) {
       
            $scope.loginData.ruc = response.ruc;
            $scope.loginData.tokenuser = response.tokenUser;


            if (Source == "1") {
                $scope.PreCalifica();


            }
            if (Source == "2") {
                var hashInput = $scope.rpHash($('#txtCAPTCHA')[0].value);
                if (hashInput != $('#txtCAPTCHA').realperson('getHash')) {
                    $scope.showMessage('E', "El código captcha ingresado es incorrecto.");
                    return;
                }
                $scope.loginData.IdParticipante = response.IdParticipante;
                $scope.loginData.NombreParticipante = response.NombreParticipante;
                $scope.loginData.IdentParticipante = response.IdentParticipante;
                $scope.loginData.Estado = response.Estado;
                $scope.loginData.PS_Token = response.PS_Token;
                $scope.loginData.EsClaveNuevo = response.EsClaveNuevo;
                $scope.loginData.EsClaveCambio = response.EsClaveCambio;
                $scope.loginData.EsClaveBloqueo = response.EsClaveBloqueo;
                $scope.loginData.CodSAP = response.CodSAP;
                $scope.loginData.CorreoE = response.CorreoE;
                $scope.loginData.Celular = response.Celular;
                $scope.loginData.EsAdmin = response.EsAdmin;
                $scope.loginData.Usuario = response.Usuario;

                $scope.loginData.rolAdm = response.rolAdm;
                $scope.loginData.nomEmpresa = response.nomEmpresa;
                $scope.loginData.rucEmpresa = response.rucEmpresa;
                $scope.loginData.esEtiqueta = response.esEtiqueta;
                $scope.loginData.Sociedad = response.sociedad;

                if ($scope.loginData.EsClaveNuevo == "S") {
                    window.location = "../Seguridad/frmUsrFirstLogon";
                }
                else if ($scope.loginData.EsClaveCambio == "S") {
                    window.location = "../Seguridad/frmRecuperaClaveP2";
                }
                else if ($scope.loginData.EsClaveBloqueo == "S") {
                    $scope.message = "Cuenta o contraseña bloqueada."; //parcial
                    $scope.showMessage('E', $scope.message);
                }

                if ($scope.loginData.EsClaveNuevo == "N" && $scope.loginData.EsClaveCambio == "N" && $scope.loginData.EsClaveBloqueo == "N") {
                    window.location = "../Notificacion/frmVisualizaNotificaciones";
                }

            }

            if (Source == "3") {

                $scope.loginData.IdParticipante = response.IdParticipante;
                $scope.loginData.NombreParticipante = response.NombreParticipante;
                $scope.loginData.IdentParticipante = response.IdentParticipante;
                $scope.loginData.Estado = response.Estado;
                $scope.loginData.PS_Token = response.PS_Token;
                $scope.loginData.EsClaveNuevo = response.EsClaveNuevo;
                $scope.loginData.EsClaveCambio = response.EsClaveCambio;
                $scope.loginData.EsClaveBloqueo = response.EsClaveBloqueo;
                $scope.loginData.CodSAP = response.CodSAP;
                $scope.loginData.CorreoE = response.CorreoE;
                $scope.loginData.Celular = response.Celular;
                $scope.loginData.EsAdmin = response.EsAdmin;
                $scope.loginData.Usuario = response.Usuario;
                $scope.loginData.esEtiqueta = response.esEtiqueta;
                $scope.loginData.Sociedad = response.sociedad;

                if ($scope.loginData.EsClaveNuevo == "S") {
                    window.location = "../Seguridad/frmUsrFirstLogon";
                }
                else if ($scope.loginData.EsClaveCambio == "S") {
                    window.location = "../Seguridad/frmRecuperaClaveP2";
                }
                else if ($scope.loginData.EsClaveBloqueo == "S") {
                    $scope.message = "Cuenta o contraseña bloqueada.";
                    $scope.showMessage('E', $scope.message);
                }

                if ($scope.loginData.EsClaveNuevo == "N" && $scope.loginData.EsClaveCambio == "N" && $scope.loginData.EsClaveBloqueo == "N") {
                    window.location = "../Home/Index";
                }

            }

        },
         function (error) {
             if (error != null) {

                 $scope.message = error.error_description;
             }
             else {
                 $scope.message = "Error en inicio de sesión, por favor vuelva a intentarlo.";
             }
             $scope.showMessage('E', $scope.message);
         });
    };

    var Userf = function () {
        return {
            ruc: '',
            email: ''

        }
    }

    var Userc = function () {
        return {
            ruc: '',
            email: '',
            code: '',
            username: '',
            userId: '',
            currentpassword: '',
            password: '',
            confirmPassword: ''
        }
    }

    var Userr = function () {
        return {
            ruc: '',
            email: '',
            code: '',
            username: '',
            userId: '',
            currentpassword: '',
            password: '',
            confirmPassword: ''
        }
    }


    $scope.puserf = new Userf();

    $scope.puserc = new Userc();

    $scope.puserr = new Userr();


    $scope.forgotPassword = function () {

        if (!$scope.forgotPasswordForm.$valid) {
            $scope.showMessage('E', 'Verique los datos de entrada.');
            return;
        }

        $scope.myPromise = authService.forgotPassword($scope.puserf).then(function (response) {


            $scope.UrlRedirect = "../Home/Index";
            $scope.showMessage('R', 'El correo electrónico con las instrucciones a sido enviado, revise su bandeja de entrada');
        },
         function (error) {
             var errors = [];
             for (var key in error.data.modelState) {
                 for (var i = 0; i < error.data.modelState[key].length; i++) {
                     errors.push(error.data.modelState[key][i]);
                 }
             }
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
    };



    function validacedula() {
        if (angular.isUndefined($scope.loginData.ruc) == true) {
            return false;
        }
        var campos = $scope.loginData.ruc;
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

        if (campos.length >= 10) {
            var numero = campos;
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
                $scope.MenjError = "El tercer dígito ingresado es inválido"
                $('#idMensajeError').modal('show');
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
                    $scope.MenjError = "El ruc de la empresa del sector público debe terminar con 0001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }
            else if (pri == true) {
                
                if (numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la empresa del sector privado debe terminar con 001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }

            else if (nat == true) {
                
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la persona natural debe terminar con 001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }
            return true;
        }
    }

    function getQueryVar(varName) {
        var queryStr = unescape(window.location.search) + '&';
        var val = "";
        var regex = new RegExp('.*?[&\\?]' + varName + '=(.*?)&.*');
        val = queryStr.replace(regex, "$1");
        return val == queryStr ? false : val;
    }

    var searchObjectcode = getQueryVar('code');
    $scope.puserc.code = searchObjectcode;

    var searchObjectruc = getQueryVar('ruc');
    $scope.puserc.ruc = searchObjectruc;


    var searchObjectemail = getQueryVar('email');
    $scope.puserc.email = searchObjectemail;


    var searchObjectuserId = getQueryVar('userId');
    $scope.puserc.userId = searchObjectuserId;

    var searchObjectuserPass = getQueryVar('userPass');
    $scope.puserc.password = searchObjectuserPass;



    $scope.resetPassword = function () {

        var rsearchObjectcode = getQueryVar('code');
        $scope.puserr.code = rsearchObjectcode;



        $scope.myPromise = authService.resetPassword($scope.puserr).then(function (response) {

            $scope.UrlRedirect = "../Account/FrmloginSol";
            $scope.showMessage('R', 'Su contraseña ha sido actualizada exitosamente ');


        },
             function (err) {
                 $scope.message = err.error_description;
                 $scope.showMessage('E', "Error en Actualizar la clave verifique los datos");
             });
    };

    $scope.Redireccion = function () {
        $scope.UrlRedirect = "../Home/Index";
        window.location = $scope.UrlRedirect;

    };


    $scope.EmailConfirm = function () {

        $scope.myPromise = authService.confirmEmailRuc($scope.puserc).then(function (response) {
            $scope.UrlRedirect = "../Account/FrmloginSol#/est";
            $scope.showMessage('R', 'RUC y correo ha sido verificado exitosamente.');


        },
         function (err) {
             $scope.showMessage('E', 'Confirmación de Correo & Ruc no válida');
         });
    };

    $scope.validaPestana = function () {
        var URLactual = window.location;
        if (URLactual.href.indexOf("FrmloginSol#/est")!==-1) {
            $('.nav-tabs a[href="#est"]').tab('show');
        }
    };

    $scope.authExternalProvider = function (provider) {

        var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';

        var externalProviderUrl = ngAuthSettings.apiServiceBaseUri + "api/Account/ExternalLogin?provider=" + provider
                                                                    + "&response_type=token&client_id=" + ngAuthSettings.clientId
                                                                    + "&redirect_uri=" + redirectUri;
        window.$windowScope = $scope;

        var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
    };

    $scope.authCompletedCB = function (fragment) {

        $scope.$apply(function () {

            if (fragment.haslocalaccount == 'False') {

                authService.logOut();

                authService.externalAuthData = {
                    provider: fragment.provider,
                    userName: fragment.external_user_name,
                    externalAccessToken: fragment.external_access_token
                };

                $location.path('/associate');

            }
            else {
                var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                authService.obtainAccessToken(externalData).then(function (response) {

                    $location.path('/orders');

                },
             function (err) {
                 $scope.message = err.error_description;
                 $scope.showMessage('E', $scope.message);
             });
            }

        });
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
        else if (tipo == 'R') {
            $('#idMensajeInformativon').modal('show');
        }
        else if (tipo == 'IA') {
            $('#idMensajeInformativona').modal('show');
        }
    }

}]);

'use strict';
app.controller('muertosessionController', ['$scope', '$location', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, $cookies, ngAuthSettings, FileUploader, $filter, authService) {

    $scope.aceptarmuerte = function () {
        authService.logOut();
        window.location = "../Home/Index";
    }
}
]);

'use strict';
app.controller('loginControllerConfirmar', ['$scope', '$location', '$http', 'authService', 'ngAuthSettings', 'SolicitudProveedor', 'GeneralService', '$cookies', 'localStorageService', function ($scope, $location, $http, authService, ngAuthSettings, SolicitudProveedor, GeneralService, $cookies, localStorageService) {

    $scope.MenjError = "";

    $scope.message = '';
    $scope.messageP = 'Por Favor Espere...';
    $scope.myPromise = null;

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

    $scope.MenjError = "";

    $scope.PreCalifica = function () {


        if (authService.authentication.ruc == '') {
            
            return;
        }


        localStorageService.remove('SolPreca');

        $scope.myPromise = SolicitudProveedor.getProveedorList("", "", authService.authentication.ruc, "", "").then(function (response) {
            $scope.ProveedorList = response.data;

            var DesEstado = '';
            var idx = 0;

            if ($scope.ProveedorList != null && $scope.ProveedorList.length > 0) {
                $scope.MenjError = "Ya existe como proveedor";
                $scope.showMessage('E', $scope.MenjError);
            }
            else {
                $scope.myPromise = SolicitudProveedor.getUltSolProveedorList("", "", authService.authentication.ruc, '', '').then(function (response) {

                    if (response.data != null && response.data.length > 0) {

                        for (idx = 0; idx < $scope.EstadosSolicitud.length; idx++) {

                            if ($scope.EstadosSolicitud[idx].codigo == response.data[0].estado) {
                                DesEstado = $scope.EstadosSolicitud[idx].detalle;
                                break;
                            }
                        }


                        if (response.data[0].estado == "PA") {
                            $scope.MenjError = "Su Precalificacion ya fue Aprobada. ";

                        }

                        if (response.data[0].estado != "IP" && response.data[0].estado != "PR") {
                            $scope.MenjError = "Su solicitud esta " + DesEstado;
                           
                        }

                        var _estado = response.data[0].estado;
                        var _idsolicitud = response.data[0].idSolicitud;


                        if (response.data[0].estado == "IP" || response.data[0].estado == "PR") {

                            var _estado = "IP";
                            if (response.data[0].estado == "PR") {
                                _idsolicitud = "";
                            }


                        }


                        var someSessionObj = {

                            'tipoide': '',
                            'identificacion': authService.authentication.ruc,
                            'tipoidentificacion': 'RC',
                            'IdSolicitud': _idsolicitud,
                            'Estado': _estado,
                            'tiposolicitud': 'PR',
                            'indpage': 'ING',
                            'MenjError': $scope.MenjError
                        };

                        localStorageService.set('SolPreca', someSessionObj);
                        if (($scope.MenjError != "" && response.data[0].estado != "PA") && (response.data[0].estado != "IN") && (response.data[0].estado != "IP") && (response.data[0].estado != "DP") && (response.data[0].estado != "PR")) {

                            if (response.data[0].tipoSolicitud == 'NU') {
                                if (response.data[0].estado != "PR") {

                                    if (response.data[0].estado != "RE") {
                                        $scope.MenjError = ("Su solicitud esta en Aprobación");
                                    }
                                    else {
                                        $scope.MenjError = ("Su solicitud ha sido rechazada");
                                    }

                                }
                                else {

                                    $scope.MenjError = ("Su solicitud ha sido rechazada");

                                }

                                $scope.UrlRedirect = "../Home/Index";
                                $scope.showMessage('R', $scope.MenjError);
                            }
                            else {
                                if (response.data[0].tipoSolicitud == 'EP') {

                                }

                            }

                        }
                        else {

                            if (response.data[0].tipoSolicitud == 'PR') {
                                window.location = "../Proveedor/frmIngPreCalifica";
                            }
                            if (response.data[0].tipoSolicitud == 'NU') {
                                localStorageService.remove('SolProv');
                                localStorageService.remove('SolPreca');
                                var someSessionObj = {

                                    'tipoide': '',
                                    'identificacion': authService.authentication.ruc,
                                    'tipoidentificacion': 'R',
                                    'IdSolicitud': _idsolicitud,
                                    'Estado': _estado,
                                    'tiposolicitud': 'NU',
                                    'indpage': 'ING',
                                    'MenjError': $scope.MenjError
                                    
                                };

                                localStorageService.set('SolProv', someSessionObj);
                                window.location = "../Proveedor/frmSolictud";
                            }

                        }

                    }
                    else {
                        var someSessionObj = {

                            'tipoide': '',
                            'identificacion': authService.authentication.ruc,
                            'tipoidentificacion': 'RC',

                            'IdSolicitud': '',
                            'Estado': 'IP',
                            'tiposolicitud': 'PR',
                            'indpage': 'ING',
                            'MenjError': $scope.MenjError

                        };


                        localStorageService.set('SolPreca', someSessionObj);
    
                        window.location = "../Proveedor/frmIngPreCalifica";
                        
                    }
                },

                function (err) {
                    $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                    $scope.showMessage('R', $scope.MenjError);
                });

            }
        },
         function (err) {
             $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
             $scope.showMessage('E', $scope.MenjError);

         });
    };


    $scope.loginData = {
        isAuth: false,
        ruc: "",
        usernamefrm: "",
        useRefreshTokens: false,
        userRetreived: false,
        firstName: '',
        lastName: '',
        email: '',
        roles: [],
        tokenuser: "",
        IdParticipante: "",
        NombreParticipante: "",
        NombreParticipante22: "",
        IdentParticipante: "",
        Estado: "",
        PS_Token: "",
        EsClaveNuevo: "",
        EsClaveCambio: "",
        EsClaveBloqueo: ""
    };

    $scope.login = function (Source) {
        $scope.message = '';

        if ($scope.loginData.ruc == '' || angular.isUndefined($scope.loginData.ruc) == true) {
            $scope.message = "Ingrese el RUC.";
            $scope.showMessage('I', $scope.message);
            return;
        }

        if ($scope.loginData.usernamefrm == '') {
            $scope.message = "Ingrese el Usuario.";
            $scope.showMessage('I', $scope.message);
            return;
        }

        if ($scope.loginData.password == null || $scope.loginData.password == '') {
            $scope.message = "Ingrese la Contraseña.";
            $scope.showMessage('I', $scope.message);
            return;
        }
        if (validacedula() == false) {
            return;
        }

        $scope.loginData.sl = Source;
        $scope.myPromise = authService.login($scope.loginData).then(function (response) {

            $scope.loginData.ruc = response.ruc;
            $scope.loginData.tokenuser = response.tokenUser;


            if (Source == "1") {
                $scope.PreCalifica();


            }
            if (Source == "2") {

                $scope.loginData.IdParticipante = response.IdParticipante;
                $scope.loginData.NombreParticipante = response.NombreParticipante;
                $scope.loginData.IdentParticipante = response.IdentParticipante;
                $scope.loginData.Estado = response.Estado;
                $scope.loginData.PS_Token = response.PS_Token;
                $scope.loginData.EsClaveNuevo = response.EsClaveNuevo;
                $scope.loginData.EsClaveCambio = response.EsClaveCambio;
                $scope.loginData.EsClaveBloqueo = response.EsClaveBloqueo;
                $scope.loginData.CodSAP = response.CodSAP;
                $scope.loginData.CorreoE = response.CorreoE;
                $scope.loginData.Celular = response.Celular;
                $scope.loginData.EsAdmin = response.EsAdmin;
                $scope.loginData.Usuario = response.Usuario;


                if ($scope.loginData.EsClaveNuevo == "S") {
                    window.location = "../Seguridad/frmUsrFirstLogon";
                }
                else if ($scope.loginData.EsClaveCambio == "S") {
                    window.location = "../Seguridad/frmRecuperaClaveP2";
                }
                else if ($scope.loginData.EsClaveBloqueo == "S") {
                    $scope.message = "Cuenta o contraseña bloqueada."; //parcial
                    $scope.showMessage('E', $scope.message);
                }

                if ($scope.loginData.EsClaveNuevo == "N" && $scope.loginData.EsClaveCambio == "N" && $scope.loginData.EsClaveBloqueo == "N") {
                    window.location = "../Notificacion/frmVisualizaNotificaciones";
                }

            }

            if (Source == "3") {

                $scope.loginData.IdParticipante = response.IdParticipante;
                $scope.loginData.NombreParticipante = response.NombreParticipante;
                $scope.loginData.IdentParticipante = response.IdentParticipante;
                $scope.loginData.Estado = response.Estado;
                $scope.loginData.PS_Token = response.PS_Token;
                $scope.loginData.EsClaveNuevo = response.EsClaveNuevo;
                $scope.loginData.EsClaveCambio = response.EsClaveCambio;
                $scope.loginData.EsClaveBloqueo = response.EsClaveBloqueo;
                $scope.loginData.CodSAP = response.CodSAP;
                $scope.loginData.CorreoE = response.CorreoE;
                $scope.loginData.Celular = response.Celular;
                $scope.loginData.EsAdmin = response.EsAdmin;
                $scope.loginData.Usuario = response.Usuario;


                if ($scope.loginData.EsClaveNuevo == "S") {
                    window.location = "../Seguridad/frmUsrFirstLogon";
                }
                else if ($scope.loginData.EsClaveCambio == "S") {
                    window.location = "../Seguridad/frmRecuperaClaveP2";
                }
                else if ($scope.loginData.EsClaveBloqueo == "S") {
                    $scope.message = "Cuenta o contraseña bloqueada.";
                    $scope.showMessage('E', $scope.message);
                }

                if ($scope.loginData.EsClaveNuevo == "N" && $scope.loginData.EsClaveCambio == "N" && $scope.loginData.EsClaveBloqueo == "N") {
                    window.location = "../Home/Index";
                }

            }

        },
         function (error) {
             if (error != null) {
       
                 $scope.message = error.error_description;
             }
             else {
                 $scope.message = "Se ha producido un error en el Sitio. Por favor reportar al departamento de Sistemas de Corporación El Rosado.";
             }
             $scope.showMessage('E', $scope.message);
         });
    };

    var Userf = function () {
        return {
            ruc: '',
            email: ''

        }
    }

    var Userc = function () {
        return {
            ruc: '',
            email: '',
            code: '',
            username: '',
            userId: '',
            currentpassword: '',
            password: '',
            confirmPassword: ''
        }
    }

    var Userr = function () {
        return {
            ruc: '',
            email: '',
            code: '',
            username: '',
            userId: '',
            currentpassword: '',
            password: '',
            confirmPassword: ''
        }
    }


    $scope.puserf = new Userf();

    $scope.puserc = new Userc();

    $scope.puserr = new Userr();


    $scope.forgotPassword = function () {

        if (!$scope.forgotPasswordForm.$valid) {
            $scope.showMessage('E', 'Verique los datos de entrada.');
            return;
        }

        $scope.myPromise = authService.forgotPassword($scope.puserf).then(function (response) {


            $scope.UrlRedirect = "../Home/Index";
            $scope.showMessage('R', 'El correo electrónico con las instrucciones a sido enviado, revise su bandeja de entrada');

        },
         function (error) {
             var errors = [];
             for (var key in error.data.modelState) {
                 for (var i = 0; i < error.data.modelState[key].length; i++) {
                     errors.push(error.data.modelState[key][i]);
                 }
             }
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));

         });
    };



    function validacedula() {
        if (angular.isUndefined($scope.loginData.ruc) == true) {
            return false;
        }
        var campos = $scope.loginData.ruc;
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

        if (campos.length >= 10) {
            var numero = campos;
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
                $scope.MenjError = "El tercer dígito ingresado es inválido"
                $('#idMensajeError').modal('show');
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
                    $scope.MenjError = "El ruc de la empresa del sector público debe terminar con 0001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }
            else if (pri == true) {
                if (numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la empresa del sector privado debe terminar con 001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }

            else if (nat == true) {

                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la persona natural debe terminar con 001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }
            return true;
        }

    }

    function getQueryVar(varName) {
        var queryStr = unescape(window.location.search) + '&';
        var val = "";

        var regex = new RegExp('.*?[&\\?]' + varName + '=(.*?)&.*');

        val = queryStr.replace(regex, "$1");

        return val == queryStr ? false : val;
    }

    var searchObjectcode = getQueryVar('code'); 
    $scope.puserc.code = searchObjectcode;

    var searchObjectruc = getQueryVar('ruc');
    $scope.puserc.ruc = searchObjectruc;


    var searchObjectemail = getQueryVar('email');
    $scope.puserc.email = searchObjectemail;


    var searchObjectuserId = getQueryVar('userId');
    $scope.puserc.userId = searchObjectuserId;

    var searchObjectuserPass = getQueryVar('userPass');
    $scope.puserc.password = searchObjectuserPass;



    $scope.resetPassword = function () {

        var rsearchObjectcode = getQueryVar('code'); 
        $scope.puserr.code = rsearchObjectcode;



        $scope.myPromise = authService.resetPassword($scope.puserr).then(function (response) {

            $scope.UrlRedirect = "../Account/FrmloginSol";
            $scope.showMessage('R', 'Su contraseña ha sido actualizada exitosamente ');


        },
             function (err) {
                 $scope.message = err.error_description;
                 $scope.showMessage('E', $scope.message);
             });
    };

    $scope.Redireccion = function () {

        window.location = $scope.UrlRedirect;

    };


    $scope.EmailConfirm = function () {

        $scope.myPromise = authService.confirmEmailRuc($scope.puserc).then(function (response) {

            $scope.UrlRedirect = "../Account/FrmloginSol#/est";

            $scope.showMessage('R', 'RUC y correo ha sido verificado exitosamente.');


        },
         function (err) {
             
             $scope.showMessage('E', 'Confirmación de Correo & Ruc no válida');
         });
    };

    $scope.authExternalProvider = function (provider) {

        var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';

        var externalProviderUrl = ngAuthSettings.apiServiceBaseUri + "api/Account/ExternalLogin?provider=" + provider
                                                                    + "&response_type=token&client_id=" + ngAuthSettings.clientId
                                                                    + "&redirect_uri=" + redirectUri;
        window.$windowScope = $scope;

        var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
    };

    $scope.authCompletedCB = function (fragment) {

        $scope.$apply(function () {

            if (fragment.haslocalaccount == 'False') {

                authService.logOut();

                authService.externalAuthData = {
                    provider: fragment.provider,
                    userName: fragment.external_user_name,
                    externalAccessToken: fragment.external_access_token
                };

                $location.path('/associate');

            }
            else {
                var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                authService.obtainAccessToken(externalData).then(function (response) {

                    $location.path('/orders');

                },
             function (err) {
                 $scope.message = err.error_description;
                 $scope.showMessage('E', $scope.message);
             });
            }

        });
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
        else if (tipo == 'R') {
            $('#idMensajeInformativon').modal('show');
        }
    }
    setTimeout(function () { $('#btnConsulta').focus(); }, 150);
    setTimeout(function () { angular.element('#btnConsulta').trigger('click'); }, 250);    
}]);