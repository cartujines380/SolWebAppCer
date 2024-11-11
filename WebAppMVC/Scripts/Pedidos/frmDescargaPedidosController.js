
//'use strict';
app.controller('frmDescargaPedidosController', ['$scope', 'PedidosService', '$filter', 'authService', function ($scope, PedidosService, $filter, authService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.pRutaDownloadTxt = "";
    $scope.pRutaDownloadXml = "";

    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;

    var bandVerRes = false;
    var bandGenTxt = true;
    var bandGenXml = true;
    var bandGenHtml = false;
    var bandVerPdf = false;

    var Fecha1 = ""; var Fecha2 = ""; var Ciudad = "";
    var Opc1 = "F"; var Opc2 = "N"; var NumOrden = "";
    var Almacen = "";
    var filetxt = "";
    var filexml = "";
    function CargaInicial1()
    {

        $scope.myPromise = PedidosService.getExportarDataupd($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, Opc1, Opc2,
       Fecha1, Fecha2, Ciudad, NumOrden, Almacen, 1).then(function (results) {
           if (results.data != "") {
                   filetxt = new Blob([results.data], { type: 'application/txt' });
           }
         
       }, function (error) {
           $('#pnlMsgNoPedidosNuevos').show();
       });

    }
    function CargaInicial2() {
        $scope.myPromise = PedidosService.getExportarDataupd($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, Opc1, Opc2,
            Fecha1, Fecha2, Ciudad, NumOrden, Almacen, 2).then(function (results) {
            if (results.data != "") {
                    filexml = new Blob([results.data], { type: 'application/xml' });
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
        var Opc1 = "XD"
        $scope.myPromise = PedidosService.getExportarDataupd($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, Opc1, Opc2,
        Fecha1, Fecha2, Ciudad, NumOrden, Almacen, 5).then(function (results) {
            
        }, function (error) {
       
        });
    }
    CargaInicial1();
    CargaInicial2();

    $scope.exportar  = function (tipo)
    {
        if (tipo == "1")
            saveAs(filetxt, 'pedidos_' + $scope.pCodSAP + '.txt');
        if (tipo == "2")
            saveAs(filexml, 'pedidos_' + $scope.pCodSAP + '.xml');

        CargaInicial3();
    }
    //$scope.myPromise = PedidosService.getConsPedidosFiltro($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, Opc1, Opc2,
    //Fecha1, Fecha2, Ciudad, NumOrden, bandVerRes, bandGenTxt, bandGenXml, bandGenHtml, bandVerPdf).then(function (results) {
    //    if (results.data.success) {
    //        $scope.pRutaDownloadTxt = results.data.root[0][1];
    //        $scope.pRutaDownloadXml = results.data.root[0][2];
    //        if ($scope.pRutaDownloadTxt == "") {
    //            $('#pnlMsgNoPedidosNuevos').show();
    //        }
    //        else {
    //            $('#pnlCtrPedidosNuevos').show();
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
