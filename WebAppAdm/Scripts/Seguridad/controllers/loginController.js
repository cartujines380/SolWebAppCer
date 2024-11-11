'use strict';
app.controller('loginController', ['$scope', '$location', '$http', 'authService', 'ngAuthSettings', 'GeneralService', function ($scope, $location, $http, authService, ngAuthSettings, GeneralService) {


    $scope.MenjError = "";
    $scope.message = "";

    $scope.messageP = 'Por Favor Espere...';
    $scope.myPromise = null;

    //$scope.loginData.sl = Source;

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
        sl: "",
        CargoEmpleado: ""
    };

    $scope.Sociedad = "";
    $scope.BodegaDatos = [];
 
    GeneralService.getCargaSociedad('7').then(function (results) {
        $scope.BodegaDatos = results.data.root[0];
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

    //$(document).ready(function () {
    //    //location.reload();
    //    $scope.login();
    //});

    $scope.login = function (Source) {
        $scope.message = '';

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

        $scope.loginData.sl = Source;
        $scope.loginData.Sociedad = $scope.Sociedad.codigo;
        $scope.myPromise = null;
        $scope.myPromise = authService.login($scope.loginData).then(function (response) {

            $scope.loginData.ruc = response.ruc;
            $scope.loginData.tokenuser = response.tokenuser;

            if (Source == "1") {

                window.location = "../Proveedor/frmIngPreCalifica";

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
                $scope.loginData.CargoEmpleado = response.CargoEmpleado;


                $scope.loginData.Sociedad = response.sociedad;
                $scope.loginData.url = response.url;

                if ($scope.loginData.EsClaveNuevo == "S") {
                    // window.location = "/Seguridad/frmUsrFirstLogon";
                }
                else if ($scope.loginData.EsClaveCambio == "S") {
                    //window.location = "/Seguridad/frmCambioClave";
                    // window.location = "/Seguridad/frmRecuperaClaveP2";
                }
                else if ($scope.loginData.EsClaveBloqueo == "S") {
                    //window.location = "/Seguridad/frmRecuperaClaveP2";
                    $scope.message = "Cuenta o contraseña bloqueada.";
                    $scope.showMessage('E', $scope.message);
                }

                //if ($scope.loginData.EsClaveNuevo == "N" && $scope.loginData.EsClaveCambio == "N" && $scope.loginData.EsClaveBloqueo == "N") {
                window.location = "../Notificacion/frmVisualizaNotificaciones";
                //}


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
                $scope.loginData.Sociedad = response.sociedad;
                $scope.loginData.CargoEmpleado = response.CargoEmpleado;
                $scope.loginData.url = response.url;

                if ($scope.loginData.EsClaveNuevo == "S") {// window.location = "/Seguridad/frmUsrFirstLogon";
                }
                else if ($scope.loginData.EsClaveCambio == "S") { }
                //window.location = "/Seguridad/frmCambioClave";
                //   window.location = "/Seguridad/frmRecuperaClaveP2";

                else if ($scope.loginData.EsClaveBloqueo == "S") {
                    //window.location = "/Seguridad/frmRecuperaClaveP2";
                    $scope.message = "Cuenta o contraseña bloqueada.";
                    $scope.showMessage('E', $scope.message);
                }
                //if ($scope.loginData.EsClaveNuevo == "N" && $scope.loginData.EsClaveCambio == "N" && $scope.loginData.EsClaveBloqueo == "N")
                window.location = "../Home/Index";
            }

        },
         function (err) {
             $scope.message = "Usuario o contraseña incorrecta";// err.error_description;
             $scope.showMessage('E', $scope.message);
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
                //Obtain access token and redirect to orders
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

}]);

'use strict';
app.controller('muertosessionController', ['$scope', '$location', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, $cookies, ngAuthSettings, FileUploader, $filter, authService) {

    $scope.aceptarmuerte = function () {
        authService.logOut();
        //$location.path('/home');
        window.location = "../Account/Frmloginadm";
    }
}
]);