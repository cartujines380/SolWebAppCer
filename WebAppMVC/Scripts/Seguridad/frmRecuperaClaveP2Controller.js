
//'use strict';
app.controller('frmRecuperaClaveP2Controller', ['$scope', 'SeguridadService', 'authService', function ($scope, SeguridadService,authService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.usrClv = {};
    $scope.usrClv.pRuc = "";
    $scope.usrClv.pUsuario = "";
    $scope.usrClv.pNombre = "";
    $scope.usrClv.pClaveNew = "";
    $scope.usrClv.pClaveCnf = "";
    $scope.usrClv.pCorreoE = "";


    $scope.btnCancelar = function () {
        authService.logOut();
        window.location = "../Home/Index"; 
    };


    $scope.btnGrabarClick = function () {
        var minPwd = 8;
        if ($scope.usrClv.pClaveNew == null || $scope.usrClv.pClaveNew.length < minPwd) {
            $scope.showMessage('I', 'Ingrese correctamente la nueva Contraseña (' + minPwd.toString() + ' caracteres mínimo).');
            return;
        }
        if ($scope.usrClv.pClaveNew != $scope.usrClv.pClaveCnf) {
            $scope.showMessage('E', 'La confirmación de la Contraseña no es igual a la nueva Contraseña ingresada.');
            return;
        }
        var NomComercial = authService.authentication.nomEmpresa;
        $scope.myPromise = SeguridadService.getRecuperaClaveCambiar($scope.usrClv.pRuc, $scope.usrClv.pUsuario,
                                        $scope.usrClv.pClaveNew, $scope.usrClv.pCorreoE, $scope.usrClv.pNombre, NomComercial).then(function (results) {
            if (results.data.success) {
                $scope.showMessage('M', 'Proceso completado exitosamente, ahora puede ingresar con su nueva contraseña.');
                $('#frmRecuperaClaveDialog').modal('hide');
                authService.logOut();
                
                $('#btnMensajeOK').click(function () {
                    window.location = "../Home/Index"; //"/Account/FrmloginProv";
                });
            }
            else {
                $scope.showMessage('E', 'Error al grabar: ' + results.data.msgError);
            }
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

    $('#frmRecuperaClaveDialog').modal('show');
    //Adicionado por djbm
    $scope.usrClv.pRuc = authService.authentication.ruc;
    $scope.usrClv.pUsuario = authService.authentication.Usuario;
    $scope.usrClv.pNombre = authService.authentication.NombreParticipante;
    $scope.usrClv.pCorreoE = authService.authentication.CorreoE;
    //fin


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
