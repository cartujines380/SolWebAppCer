
//'use strict';
app.controller('frmRecuperaClaveP1Controller', ['$scope', 'SeguridadService', 'authService', function ($scope, SeguridadService,authService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.datosRC = {};
    $scope.datosRC.pRuc = "";
    $scope.datosRC.pCorreoE = "";
    $scope.datosRC.pUsuario = "";
    $scope.datosRC.pNombre = "";
    $scope.datosRC.pCodImgSegura = "";
    $scope.datosRC.pRespuestaSegura = "";
    $scope.datosRC.pNomComercial = "";

    $scope.pathImgSeg = "../Images/imgSeguridad/";
    $scope.imgSelected = $scope.pathImgSeg + "000.jpg";

    $scope.usrValida = {};

    $scope.usrValida.cboPregSegList = [];
    $scope.usrValida.cboPregSegSelItem = null;

    $scope.usrValida.pCodImgSegura = "";
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
        $scope.usrValida.pCodImgSegura = item.codImgSeg;
        $scope.imgSelected = item.imageUrl;
    };


    $scope.btnCancelar = function () {
        window.location = "../Home/Index"; 
    };


    $scope.btnSiguienteClick = function () {
        $scope.myPromise = SeguridadService.getRecuperaClaveValidar($scope.datosRC.pRuc, $scope.datosRC.pCorreoE, $scope.datosRC.pUsuario).then(function (results) {
            if (results.data.success) {
                if (results.data.root[0].pDatosUsr == null) {
                    $scope.showMessage('E', 'Información no encontrada en nuestros registros. Por favor verifique y vuelva a intentar');
                }
                else {
                    $scope.datosRC.pUsuario = results.data.root[0].pDatosUsr.pUsuario;
                    $scope.datosRC.pNombre = results.data.root[0].pDatosUsr.pNombre;
                    $scope.datosRC.pCodImgSegura = results.data.root[0].pDatosUsr.pCodImgSegura;
                    $scope.datosRC.pCorreoE = results.data.root[0].pDatosUsr.pCorreoE;
                    $scope.datosRC.pNomComercial = results.data.root[0].pDatosUsr.pNomComercial;
                    $scope.usrValida.cboPregSegList = results.data.root[0].pRespSeg;
                    $scope.usrValida.cboPregSegSelItem = results.data.root[0].pRespSeg[0];

                    $('#frmRecuperaClaveP1Dialog').modal('hide');
                    $('#frmRecuperaClaveP2Dialog').modal('show');
                }
            }
            else {
                $scope.showMessage('E', 'Error al enviar: ' + results.data.msgError);
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
    
    $scope.btnGrabarClick = function () {
        if ($scope.datosRC.pCodImgSegura != $scope.usrValida.pCodImgSegura) {
            $scope.showMessage('I', "Uno de los datos de seguridad seleccionado/ingresado no es correcto.");
            return;
        }
        if ($scope.datosRC.pRespuestaSegura.toUpperCase() != $scope.usrValida.cboPregSegSelItem.pRespuesta.toUpperCase()) {
            $scope.showMessage('I', "Uno de los datos de seguridad seleccionado/ingresado no es correcto.");
            return;
        }
        var hashInput = $scope.rpHash($('#txtCAPTCHA')[0].value);
        
        if (hashInput != $('#txtCAPTCHA').realperson('getHash')) {
            $scope.showMessage('E', "El código captcha ingresado es incorrecto.");
            return;
        }
        var ClaveTmp = generarPassword(8, 0, '');
        $scope.myPromise = SeguridadService.getRecuperaClaveEnviarTmp($scope.datosRC.pRuc, $scope.datosRC.pUsuario,
                                        ClaveTmp, $scope.datosRC.pCorreoE, $scope.datosRC.pNombre, $scope.datosRC.pNomComercial).then(function (results) {
            if (results.data.success) {
                $scope.showMessage('M', 'Se ha enviado una nueva contraseña temporal a su correo electrónico, favor revisarlo.');
                $('#frmRecuperaClaveP2Dialog').modal('hide');
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

    $('#frmRecuperaClaveP1Dialog').modal('show');


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

    $scope.rpHash = function (value) {
        var hash = 5381;
        for (var i = 0; i < value.length; i++) {
            hash = ((hash << 5) + hash) + value.charCodeAt(i);
        }
        return hash;
    };

    //int largo, int numEsp, string listaCarEsp
    function generarPassword(largo, numEsp, listaCarEsp) {
        var Resultado = "";
        var CanNum = false;
        var CanLet = false;
        var idx = 0;
        //Cargamos la matriz con números y letras
        var Caracter = [
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z"
        ];
        //Math.random();
        while (Resultado.length < (largo - numEsp)) {
            idx = Math.floor(36 * Math.random());
            Resultado = Resultado + Caracter[idx];
            if ((idx < 10))
                CanNum = true;
            if ((idx > 9))
                CanLet = true;
        }
        if (CanNum == false) {
            Resultado = Resultado.substring(0, Resultado.length - 1);
            idx = Math.floor(10 * Math.random());
            Resultado = Resultado + Caracter[idx];
        }
        if (CanLet == false) {
            Resultado = Resultado.substring(0, Resultado.length - 1);
            idx = Math.floor(26 * Math.random()) + 10;
            Resultado = Resultado + Caracter[idx];
        }
        //Math.random();
        while (Resultado.length < largo) {
            idx = Math.floor(listaCarEsp.length * Math.random());
            Resultado = Resultado + listaCarEsp.substring(idx, idx + 1);
        }
        if (Resultado.length > 2) {
            Resultado = Resultado.substring(1, Resultado.length) + Resultado.substring(0, 1);
        }
        return Resultado;
    };

}]);

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
