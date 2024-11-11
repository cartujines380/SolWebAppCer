
//'use strict';
app.controller('frmImprimePedidosController', ['$scope', 'PedidosService', '$filter', 'authService', function ($scope, PedidosService, $filter, authService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.pRutaDownloadHtml = "";
    $scope.pRutaDownloadPdf = "";
    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;

    var bandVerRes = false;
    var bandGenTxt = false;
    var bandGenXml = false;
    var bandGenHtml = true;
    var bandVerPdf = true;

    var Fecha1 = ""; var Fecha2 = ""; var Ciudad = "";
    var Opc1 = "F"; var Opc2 = "I"; var NumOrden = "";
    var Almacen = "";
    var filehtml = "";
    var filepdf = "";
    function CargaInicial1() {

        $scope.myPromise = PedidosService.getExportarDataupd($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, Opc1, Opc2,
       Fecha1, Fecha2, Ciudad, NumOrden, Almacen, 3).then(function (results) {
           if (results.data != "") {
               filehtml = new Blob([results.data], { type: 'application/html' });
           }

       }, function (error) {
           $('#pnlMsgNoPedidosNuevos').show();
       });

    }
    function CargaInicial2() {
        $scope.myPromise = PedidosService.getExportarDataupd($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, Opc1, Opc2,
        Fecha1, Fecha2, Ciudad, NumOrden, Almacen, 4).then(function (results) {
            if (results.data != "") {
                filepdf = new Blob([results.data], { type: 'application/pdf' });
                $('#pnlCtrPedidosNuevos').show();
            }
            else {
                $('#pnlMsgNoPedidosNuevos').show();
            }
        }, function (error) {
            $('#pnlMsgNoPedidosNuevos').show();
        });
    }
    function CargaInicial3() {
        var Opc1 = "XI"
        $scope.myPromise = PedidosService.getExportarDataupd($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, Opc1, Opc2,
        Fecha1, Fecha2, Ciudad, NumOrden, Almacen, 5).then(function (results) {

        }, function (error) {

        });
    }
    CargaInicial1();
    CargaInicial2();

    $scope.exportar = function (tipo) {
        if (tipo == "3")
            saveAs(filehtml, 'pedidos_' + $scope.pCodSAP + '.html');
        if (tipo == "4")
            saveAs(filepdf, 'pedidos_' + $scope.pCodSAP + '.pdf');

        CargaInicial3();
    }

    //$scope.myPromise = PedidosService.getConsPedidosFiltro($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, Opc1, Opc2,
    //Fecha1, Fecha2, Ciudad, NumOrden, bandVerRes, bandGenTxt, bandGenXml, bandGenHtml, bandVerPdf).then(function (results) {
    //    if (results.data.success) {
    //        $scope.pRutaDownloadHtml = results.data.root[0][3];
    //        $scope.pRutaDownloadPdf = results.data.root[0][4];
    //        if ($scope.pRutaDownloadHtml == "") {
    //            $('#pnlMsgNoPedidosNuevos').show();
    //        }
    //        else {
    //            $('#pnlCtrPedidosNuevos').show();
    //            //event.preventDefault();
    //            //event.stopPropagation();
    //            window.open($scope.pRutaDownloadHtml, '_blank');
    //            window.open($scope.pRutaDownloadPdf, '_blank');
    //        }
    //    }
    //    else {
    //        $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
    //    }
    //},
    //function (error) {
    //    var errors = [];
    //    for (var key in error.data.modelState) {
    //        for (var i = 0; i < error.data.modelState[key].length; i++) {
    //            errors.push(error.data.modelState[key][i]);
    //        }
    //    }
    //    $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
    //});


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
