
//'use strict';
app.controller('frmCambioClaveController', ['$scope', 'SeguridadService', 'authService', function ($scope, SeguridadService,authService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.usrClv = {};
    $scope.usrClv.pRuc = "";
    $scope.usrClv.pUsuario = "";
    $scope.usrClv.pNombre = "";
    $scope.usrClv.pClaveAct = "";
    $scope.usrClv.pClaveNew = "";
    $scope.usrClv.pClaveCnf = "";
    $scope.usrClv.pCorreoE = "";
    $scope.usrClv.pNomComercial = "";
    $scope.btnCancelar = function () {
        $('#frmCambioClaveDialog').modal('hide');
        window.location = "../Home/IndexPrivado";
    };

    $scope.btnGrabarClick = function () {
        var minPwd = 8;
        if ($scope.usrClv.pClaveAct == null || $scope.usrClv.pClaveAct.length < minPwd) {
            $scope.showMessage('I', 'Ingrese correctamente la Contraseña actual (' + minPwd.toString() + ' caracteres mínimo).');
            return;
        }
        if ($scope.usrClv.pClaveNew == null || $scope.usrClv.pClaveNew.length < minPwd) {
            $scope.showMessage('I', 'Ingrese correctamente la nueva Contraseña (' + minPwd.toString() + ' caracteres mínimo).');
            return;
        }
        if ($scope.usrClv.pClaveAct == $scope.usrClv.pClaveNew) {
            $scope.showMessage('E', 'La nueva Contraseña no puede ser igual a la Contraseña actual.');
            return;
        }
        if ($scope.usrClv.pClaveNew != $scope.usrClv.pClaveCnf) {
            $scope.showMessage('E', 'La confirmación de la Contraseña no es igual a la nueva Contraseña ingresada.');
            return;
        }

        $scope.myPromise = SeguridadService.getCambiarClave($scope.usrClv.pRuc, $scope.usrClv.pUsuario, $scope.usrClv.pClaveAct,
                                        $scope.usrClv.pClaveNew, $scope.usrClv.pCorreoE, $scope.usrClv.pNombre, $scope.usrClv.pNomComercial).then(function (results) {
            if (results.data.success) {
                $scope.showMessage('M', 'Proceso completado exitosamente, ahora puede ingresar con su nueva contraseña.');
                $('#frmCambioClaveDialog').modal('hide');
                $('#btnMensajeOK').click(function () {
                    window.location = "../Home/IndexPrivado";
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

    $('#frmCambioClaveDialog').modal('show');


    //recuperar del login
    //$scope.usrClv.pRuc = "1001402211001";
    //$scope.usrClv.pUsuario = "CER00008";
    //$scope.usrClv.pNombre = "CAZAR BEDOYA LUIS PLUTARCO";
    //$scope.usrClv.pCorreoE = "cvera@sipecom.com";

    //Adicionado por djbm

    $scope.usrClv.pRuc = authService.authentication.ruc;
    $scope.usrClv.pUsuario = authService.authentication.Usuario;
    $scope.usrClv.pNombre = authService.authentication.NombreParticipante;
    $scope.usrClv.pCorreoE = authService.authentication.CorreoE;
    $scope.usrClv.pNomComercial = authService.authentication.nomEmpresa;
    //fin

    //valida información de seguridad
    $scope.myPromise = SeguridadService.getRecuperaClaveValidar($scope.usrClv.pRuc, $scope.usrClv.pCorreoE, $scope.usrClv.pUsuario).then(function (results) {
        if (results.data.success) {
            if (results.data.root[0].pDatosUsr == null) {
                $scope.errCancelarDatosSeguros('Error al validar información de seguridad: Información no encontrada en nuestros registros. Por favor verifique y vuelva a intentar');
            } else {
                console.log("Ok RecClave");
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
        $scope.errCancelarDatosSeguros("Error al validar información de seguridad: [Error en comunicación] " + errors.join(' '));
    });

    $scope.errCancelarDatosSeguros = function (errorMsg) {
        $scope.showMessage('I', errorMsg);
        $('#btnMsjError').click(function () {
            window.location = "../Home/IndexPrivado";
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
