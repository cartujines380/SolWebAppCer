'use strict';
app.controller('IntranetController', ['$scope', '$location', '$http', 'authService', 'ngAuthSettings',  function ($scope, $location, $http, authService, ngAuthSettings) {

 
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
        IdParticipante:"",
        NombreParticipante: "",
        NombreParticipante22: "",
        IdentParticipante:"",
        Estado:"",
        PS_Token :"",
        EsClaveNuevo :"",
        EsClaveCambio:"",
        EsClaveBloqueo: "",
        sl: "",
        CargoEmpleado: ""
    };

    
    function getQueryVar(varName) {
        // Grab and unescape the query string - appending an '&' keeps the RegExp simple
        // for the sake of this example.
        var queryStr = unescape(window.location.search) + '&';
        var val = "";

        // Dynamic replacement RegExp
        var regex = new RegExp('.*?[&\\?]' + varName + '=(.*?)&.*');

        // Apply RegExp to the query string
        val = queryStr.replace(regex, "$1");

        // If the string is the same, we didn't find a match - return false
        return val == queryStr ? false : val;
    }
   
    $scope.CargaSession = function (Source) {

        var searchObjectuserId = getQueryVar('acint');
        //$scope.puserc.userId = searchObjectuserId;

        $scope.message = '';
        $scope.loginData.usernamefrm = searchObjectuserId; 

        if ($scope.loginData.usernamefrm == '') {
            //$scope.message = "Ingrese el Usuario.";
            //$scope.showMessage('I', $scope.message);
            return;
        }

        if ($scope.loginData.password == null || $scope.loginData.password == '') {
            //$scope.message = "Ingrese la Contraseña.";
            //$scope.showMessage('I', $scope.message);
            return;
        }

        //authService.logout();

        $scope.loginData.sl = Source;
        $scope.myPromise = null;
        $scope.myPromise = authService.login($scope.loginData).then(function (response) {
            //return '@Url.Action("home", "index")';
            $scope.loginData.ruc = response.ruc;
            $scope.loginData.tokenuser = response.tokenuser;
       
         
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
                $scope.loginData.usernamefrm = response.userName;
                authService.authentication.userName = response.userName;
                $scope.loginData.CargoEmpleado = response.CargoEmpleado;
                 window.location = "../Index";
 

        },
         function (err) {
             $scope.message = "Usuario o contraseña incorrecta";// err.error_description;
             $scope.showMessage('E', $scope.message);
         });

         

    };

    $scope.CargaSession("4");

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


     

    //var searchObjectcode = getQueryVar('code');//  parseLocation(window.location.search)['code'];
    //$scope.puserc.code = searchObjectcode;

    //var searchObjectruc = getQueryVar('ruc');
    //$scope.puserc.ruc = searchObjectruc;


    //var searchObjectemail = getQueryVar('email');
    //$scope.puserc.email = searchObjectemail;


    //var searchObjectuserId = getQueryVar('userId');
    //$scope.puserc.userId = searchObjectuserId;

    //var searchObjectuserPass = getQueryVar('userPass');
    //$scope.puserc.password = searchObjectuserPass;




}]);
