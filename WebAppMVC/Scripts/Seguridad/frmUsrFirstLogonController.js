app.controller('frmUsrFirstLogonController', ['$scope', 'SeguridadService', 'authService', function ($scope, SeguridadService,authService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.varTitulo = "Ingreso de Usuario por primera vez";

    $scope.usrValida = {};
    $scope.usrValida.pRuc = "";
    $scope.usrValida.pUsuario = "";
    $scope.usrValida.pNombre = "";

    $scope.usrValida.pCodSAP = "";
    $scope.usrValida.pIdentificacion = "";
    $scope.usrValida.pCorreoOrig = "";

    $scope.usrValida.pCorreoOK = "";
    $scope.usrValida.pCodValidacion = "";
    $scope.usrValida.pCodValidacionIng = "";
    $scope.usrValida.pCodigoOK = false;
    $scope.usrValida.pFechaEnvio = null;

    $scope.usrIng = {};
    $scope.usrIng.pCorreoE = "";
    $scope.usrIng.pTelefono = "";
    $scope.usrIng.pCelular = "";
    $scope.usrIng.pClaveNew = "";
    $scope.usrIng.pClaveCnf = "";
    $scope.usrIng.pIdParticipante = 0;
    $scope.usrIng.pNomComercial = "";
    $scope.pathImgSeg = "../Images/imgSeguridad/";

    $scope.usrIng.pCodImgSegura = "";
    $scope.imgSelected = $scope.pathImgSeg + "000.jpg";

    $scope.usrValida.listImgs = [];
    $scope.myPromise = SeguridadService.getCatalogosFS("ConsImgSeguras").then(function (results) {
        for (i = 0; i < results.data.length; i++) {
            $scope.usrValida.listImgs.push({
                codImgSeg: results.data[i].codigo,
                imageUrl: $scope.pathImgSeg + results.data[i].detalle,
                sec: Math.floor(Math.random() * (10))
            });
        };
        results.data = null;
    }, function (error) {
        
    });


    $scope.toggleImgSelected = function (item) {
        $scope.usrIng.pCodImgSegura = item.codImgSeg;
        $scope.imgSelected = item.imageUrl;
    };

    $scope.usrIng.listPregs = [];
    $scope.myPromise = SeguridadService.getCatalogosFS("ConsPregSeguras").then(function (results) {
        
        for (i = 0; i < results.data.length; i++) {
            $scope.usrIng.listPregs.push({
                pCodigo: results.data[i].codigo,
                pPregunta: results.data[i].detalle,
                pRespuesta: ''
            });
        }
    }, function (error) {
        
    });


    //Adicionado por djbm
    $scope.usrValida.pRuc = authService.authentication.ruc;
    $scope.usrValida.pUsuario = authService.authentication.Usuario;
    $scope.usrValida.pNombre = authService.authentication.NombreParticipante;

    //fin


    var tmpCodSAP = "";
    var tmpIdentReprLegal = "";
    var tmpCorreoE = "";

    $('#UsrFirstLogonP1Dialog').modal('show');

    $scope.btnCancelar = function () {
        authService.logOut();
        window.location = "../Home/Index";
    };


    $scope.btnSiguienteP1 = function () {
        if ($scope.usrValida.pRuc.length < 10) {
            $scope.showMessage('E', 'No se recibió el RUC del proveedor.');
            return;
        }
        if ($scope.usrValida.pUsuario.length < 5) {

            $scope.usrValida.pUsuario = authService.authentication.Usuario;
            if ($scope.usrValida.pUsuario.length < 5) {
                $scope.showMessage('E', 'No se recibió el Usuario.');
                return;
            }

        }

        $scope.myPromise = SeguridadService.getValUsrFirstLogon($scope.usrValida.pRuc, $scope.usrValida.pUsuario).then(function (results) {
            
            if (results.data.success) {
                tmpCodSAP = results.data.root[0][0].codSAP;
                tmpIdentReprLegal = results.data.root[0][0].identReprLegal;
                tmpCorreoE = results.data.root[0][0].correoE;
                $scope.usrIng.pCelular = results.data.root[0][0].celular;
                $scope.usrIng.pTelefono = results.data.root[0][0].telefono;
                $scope.usrIng.pIdParticipante = results.data.root[0][0].idParticipante;
                $scope.usrIng.pNomComercial = results.data.root[0][0].pNomComercial;
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' +results.data.msgError);
            };
        }, function (error) {
            var errors = [];
            for (var key in error.data.modelState) {
                for (var i = 0; i < error.data.modelState[key].length; i++) {
                    errors.push(error.data.modelState[key][i]);
                }
            }
            $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
        });

        $('#UsrFirstLogonP1Dialog').modal('hide');
        $('#UsrFirstLogonP2Dialog').modal('show');
    };

    $scope.btnSiguienteP2 = function () {
        if ($scope.usrValida.pIdentificacion.length < 2) {
            $scope.showMessage('I', 'Ingrese correctamente la identificación del Representante Legal del proveedor (2 caracteres mínimo).');
            return;
        }
        if ($scope.usrValida.pCorreoOrig == '') {
            $scope.showMessage('I', 'Ingrese correctamente el Correo Electrónico del proveedor.');
            return;
        }
        var hashInput = $scope.rpHash($('#txtCAPTCHA')[0].value);
        
        if (hashInput != $('#txtCAPTCHA').realperson('getHash')) {
            $scope.showMessage('E', "El código captcha ingresado es incorrecto.");
            return;
        }

        if ($scope.usrValida.pCodSAP != tmpCodSAP ) {
            $scope.showMessage('E', 'Los datos ingresados no han superado la validación, favor revisar y corregir: Codigo SAP.');
            return;
        }



        $scope.usrIng.pCorreoE = $scope.usrValida.pCorreoOrig;
        $scope.usrValida.pCorreoOK = $scope.usrIng.pCorreoE;
        $scope.usrValida.pCodigoOK = true;

        $('#UsrFirstLogonP2Dialog').modal('hide');
        $('#UsrFirstLogonP3Dialog').modal('show');
    };

    $scope.btnSiguienteP3 = function () {
        if ($scope.usrIng.pCorreoE == '') {
            $scope.showMessage('I', 'Ingrese correctamente el Correo Electrónico para su usuario.');
            return;
        }
        if ($scope.usrValida.pCorreoOK != $scope.usrIng.pCorreoE || $scope.usrValida.pCodigoOK == false) {
            $scope.showMessage('I', 'Debe validar el actual Correo Electrónico ingresado para su usuario.');
            return;
        }
        if ($scope.usrIng.pCelular.length < 10) {
            $scope.showMessage('I', 'Ingrese correctamente el Celular para su usuario (10 caracteres mínimo).');
            return;
        }
        
        $('#UsrFirstLogonP3Dialog').modal('hide');
        $('#UsrFirstLogonP4Dialog').modal('show');
    };
    
    $scope.btnSiguienteP4 = function () {
        if ($scope.usrIng.pCodImgSegura == '') {
            $scope.showMessage('I', 'Debe seleccionar una de las imágenes para continuar.');
            return;
        }
        var tot = 0;
        for (i = 0; i < $scope.usrIng.listPregs.length; i++) {
            if ($scope.usrIng.listPregs[i].pRespuesta.length > 0)
                tot++;
        }
        if (tot < 3) {
            $scope.showMessage('I', 'Debe ingresar al menos 3 respuestas para las preguntas de validación.');
            return;
        }
        $('#UsrFirstLogonP4Dialog').modal('hide');
        $('#UsrFirstLogonP5Dialog').modal('show');
    };
    
    $scope.btnSiguienteP5 = function () {
        var minPwd = 8;
        if ($scope.usrIng.pClaveNew == null || $scope.usrIng.pClaveNew.length < minPwd) {
            $scope.showMessage('I', 'Ingrese correctamente la nueva Contraseña (' + minPwd.toString() + ' caracteres mínimo).');
            return;
        }
        if ($scope.usrIng.pClaveNew != $scope.usrIng.pClaveCnf) {
            $scope.showMessage('E', 'La confirmación de la Contraseña no es igual a la nueva Contraseña ingresada.');
            return;
        }
        $scope.usrData = {};
        $scope.usrData.pRuc = $scope.usrValida.pRuc;
        $scope.usrData.pUsuario = $scope.usrValida.pUsuario;
        $scope.usrData.pIdParticipante = $scope.usrIng.pIdParticipante;
        $scope.usrData.pCorreoE = $scope.usrIng.pCorreoE;
        $scope.usrData.pCelular = $scope.usrIng.pCelular;
        $scope.usrData.pTelefono = $scope.usrIng.pTelefono;
        $scope.usrData.pCodImgSegura = $scope.usrIng.pCodImgSegura;
        $scope.usrData.pClaveNew = $scope.usrIng.pClaveNew;
        $scope.usrData.pNombre = $scope.usrValida.pNombre;
        $scope.usrData.pNomComercial = $scope.usrIng.pNomComercial;
        $scope.pregSeg = [];
        for (i = 0; i < $scope.usrIng.listPregs.length; i++) {
            if ($scope.usrIng.listPregs[i].pRespuesta != "") {
                $scope.pregSeg.push({
                    pCodigo: $scope.usrIng.listPregs[i].pCodigo,
                    pPregunta: "",
                    pRespuesta: $scope.usrIng.listPregs[i].pRespuesta
                });
            }
        }
        $scope.myPromise = SeguridadService.getGrabaUsrFirstLogon($scope.usrData, $scope.pregSeg).then(function (results) {
            if (results.data.success) {
                
                $scope.showMessage('M', 'Proceso completado exitosamente, ahora puede ingresar con su nueva contraseña.');
                $('#UsrFirstLogonP5Dialog').modal('hide');
                authService.logOut();
                $('#btnMensajeOK').click(function () {
                    window.location = "../Home/Index";
                });
            }
            else {
                $scope.showMessage('E', 'Error al grabar: ' + results.data.msgError);
            }
            $scope.usrData = null;
            $scope.pregSeg = null;
        },
         function (error) {
             debugger;
             var errors = [];
             for (var key in error.data.modelState) {
                 for (var i = 0; i < error.data.modelState[key].length; i++) {
                     errors.push(error.data.modelState[key][i]);
                 }
             }
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
             $scope.usrData = null;
             $scope.pregSeg = null;
         });
    };

    $scope.btnEnvCodValClick = function () {
        $scope.usrValida.pCodValidacion = getRandomIntInclusive(1000, 9999).toString();
        $scope.usrValida.pCodValidacionIng = "";
        $scope.myPromise = SeguridadService.getValidaEmailUsrFirstLogon(
            $scope.usrIng.pCorreoE,
            $scope.usrValida.pCodValidacion,
            $scope.usrValida.pNombre,
            authService.authentication.ruc, authService.authentication.nomEmpresa).then(function (results) {
            if (results.data.success) {
                $scope.usrValida.pCorreoOK = $scope.usrIng.pCorreoE;
                $scope.usrValida.pCodigoOK = false;
                $scope.usrValida.pFechaEnvio = new Date();
                $scope.showMessage('M', 'Se ha enviado un Código de Validación al Correo Electrónico ingresado, su tiempo de vigencia es de 10 minutos. ');
            }
            else {
                debugger;
                $scope.showMessage('E', 'Error al Enviar Correo: ' + results.data.msgError);
            }
        },
         function (error) {
             debugger;
             var errors = [];
             for (var key in error.data.modelState) {
                 for (var i = 0; i < error.data.modelState[key].length; i++) {
                     errors.push(error.data.modelState[key][i]);
                 }
             }
             $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
         });
    };

    $scope.btnCompCodValClick = function () {
        if ($scope.usrValida.pCodValidacion == null || $scope.usrValida.pCodValidacion.length < 4) {
            $scope.showMessage('E', 'No hay un Código de Validación pendiente para validar.');
            return;
        }
        if ($scope.usrValida.pCorreoOK != $scope.usrIng.pCorreoE) {
            $scope.showMessage('E', 'El Correo Electrónico ingresado para su usuario ha cambiado, envíe un nuevo Código de Validación.');
            return;
        }
        var givenDate = new Date();
        var tEnvio = ($scope.usrValida.pFechaEnvio.getHours() * 100) + $scope.usrValida.pFechaEnvio.getMinutes();
        var tActual = (givenDate.getHours() * 100) + givenDate.getMinutes();
        var diferencia = tActual - tEnvio;
        if (diferencia < 0 && diferencia > 10) {
            $scope.showMessage('E', 'El Código de Validación enviado ya ha caducado (máximo 10 minutos).');
            return;
        }
        if ($scope.usrValida.pCodValidacionIng == null || $scope.usrValida.pCodValidacionIng.length < 4) {
            $scope.showMessage('I', 'Ingrese correctamente el Código de Validación enviado al Correo Electrónico ingresado.');
            return;
        }
        if ($scope.usrValida.pCodValidacion != $scope.usrValida.pCodValidacionIng) {
            $scope.showMessage('E', 'Código de Validación ingresado no es correcto.');
            return;
        }

        $scope.usrValida.pCodigoOK = true;
        $scope.usrValida.pCodValidacion = "";
        $scope.usrValida.pCodValidacionIng = "";
        $scope.usrValida.pFechaEnvio = null;

        $scope.showMessage('M', 'El Correo Electrónico ingresado fue validado correctamente.');
    };



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
    }

    function getRandomIntInclusive(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    $scope.rpHash = function (value) {
        var hash = 5381;
        for (var i = 0; i < value.length; i++) {
            hash = ((hash << 5) + hash) + value.charCodeAt(i);
        }
        return hash;
    };
    
}
]);

app.directive('onlyDigits', function () {

    return {
        restrict: 'A',
        require: '?ngModel',
        link: function (scope, element, attrs, ngModel) {
            if (!ngModel) return;
            ngModel.$parsers.unshift(function (inputValue) {
                var digits = inputValue.split('').filter(function (s) { return (!isNaN(s) && s != ' '); }).join('');
                ngModel.$viewValue = digits;
                ngModel.$render();
                return digits;
            });
        }
    };
});
