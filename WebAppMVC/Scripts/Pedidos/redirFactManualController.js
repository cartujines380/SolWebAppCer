//'use strict';
app.controller('redirFactManualController', ['$scope', 'PedidosService', 'authService', function ($scope, PedidosService, authService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;

    $scope.btnSeleccPedidoClick = function () {
        consultaUrlRedireccSiteFactManual('P');
    }

    $scope.btnRecupFacturaClick = function () {
        consultaUrlRedireccSiteFactManual('F');
    }

    consultaUrlRedireccSiteFactManual = function (opc) {
        $scope.myPromise = PedidosService.getUrlRedireccSiteFactManual(opc, $scope.pCodSAP, $scope.pRuc, $scope.pUsuario).then(function (results) {
            if (results.data.success) {
                //event.preventDefault();
                //event.stopPropagation();
                //window.open(results.data.root[0][0], '_top');
                window.location = results.data.root[0][0];
            }
            else {
                $scope.showMessage('E', 'Error al redireccionar: ' + results.data.msgError);
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
    }


    $scope.showMessage = function (tipo, mensaje) {
        //E=Error, I=Informativo, M=Mensaje
        //alert(mensaje);
        $scope.MenjError = mensaje;
        if (tipo == 'M') {
            $('#idMensajeOk').modal('show');
        }
        else {
            $('#idMensajeError').modal('show');
        }
    }

}]);
