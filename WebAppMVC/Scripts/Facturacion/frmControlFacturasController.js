
//'use strict';
app.controller('frmControlFacturasController', ['$scope', 'FacturasService', '$filter', 'authService', 'localStorageService', function ($scope, FacturasService, $filter, authService, localStorageService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.frmtFECHA = 'dd/MM/yyyy';
    $scope.convertStrToDATE = function (fechaStr) {
        return new Date(parseInt(fechaStr.substring(6)), parseInt(fechaStr.substring(3, 5)) - 1, parseInt(fechaStr.substring(0, 2)), 0, 0);
    }

    $scope.tipoConfirma = "";

    $scope.resDgItemsDisp = [];
    var _resDgItemsDisp = [];
    $scope.pagesItemsDisp = [];
    $scope.pgcDgItemsDisp = [];

    $scope.codigoIva12 = "";
    $scope.codigoIva14 = "";
    $scope.porcentajeIva12 = "";
    $scope.porcentajeIva14 = "";
    $scope.porcentaje = "";
    $scope.pIdPedido = 0;
    $scope.pIdDocumento = 0;
    $scope.pNombreComercial = "";
    $scope.pNombreArchivo = "";
    $scope.pClaveAcceso = "";
    $scope.pFechaPedido = "";
    $scope.pSiIngDetFact = false; //constante para SP
    $scope.pOpcSubmit = ""; //para saber cual boton de submit invoca la acccion
    $scope.pOmitirDifTotFact = false; //constante para confirmacion de que se deje pasar la diferencia con el total de la factura
    $scope.pUrlRegresar = "";
    $scope.pXmlSRI = "";
    $scope.pValTotalPedido = 0.0;
    $scope.pValTotSumaFact = 0.0;

    $scope.imgProveedor = '/Images/proveedores/Logo_Defecto.png';
    $scope.disableFactCtr = true;
    $scope.disableAllCtr = true;
    $scope.hide_btnGrabar = true;
    $scope.hide_btnEnviar = true;
    $scope.hide_btnAnular = true;
    $scope.hide_btnModificar = true;
    $scope.hide_btnGetXML = true;
    $scope.hide_btnGetPDF = true;
    $scope.hide_btnRegresar = false;

    $scope.txtNombre = "";
    $scope.txtDirMatriz = "";
    $scope.txtDirSucursal = "";
    $scope.txtRuc = "";
    $scope.txtFacEstabl = "";
    $scope.txtFacPtoEmi = "";
    $scope.txtFacNumSec = "";
    $scope.txtRazonSoc = "";
    $scope.txtAlmacenOrigenCod = "";
    $scope.txtAlmacenOrigenDes = "";
    $scope.txtFechaEmision = "";
    $scope.txtNumPedido = "";
    $scope.pValSaldoPendFact = "";
    $scope.txtIdentificacion = "";
    $scope.txtNumAutorizaTal = "";
    $scope.txtFechaIniVigAut = "";
    $scope.txtFechaFinVigAut = "";

    $scope.txtSubTot_12 = '0.00';
    $scope.txtSubTot_0 = '0.00';
    $scope.txtSubTotNoObjIva = '0.00';
    $scope.txtSubTotExenIva = '0.00';
    $scope.txtSubTotSinImp = '0.00';
    $scope.txtDescuento_12 = '0.00';
    $scope.txtDescuento_0 = '0.00';
    $scope.txtTotDescuento = '0.00';
    $scope.txtIce = '0.00';
    $scope.txtIva_12 = '0.00';
    $scope.txtIrbpnr_12 = '0.00';
    $scope.txtIrbpnr_0 = '0.00';
    $scope.txtTotIrbpnr = '0.00';
    $scope.txtPropina = '0.00';
    $scope.txtcompesacion = '0.00';
    $scope.txtValTotal = '0.00';
    $scope.txtValPagado = '0.00';

    $scope.chkcompesacion = false;

    $scope.hidecompesacion = true;

    //recuperar del login
    $scope.pRuc = authService.authentication.ruc;
    $scope.pUsuario = authService.authentication.Usuario;
    $scope.pCodSAP = authService.authentication.CodSAP;
    $scope.sepEnter = ". ";

    $scope.codigosproveedor = "";

    FacturasService.consProveedorExcepto('tbl_EstadoGeneral').then(function (results) {
        $scope.codigosproveedor = results.data.split(";");
        
    }, function (error) {
    });


    FacturasService.cargarParametros("Iva").then(function (results) {
        $scope.codigoIva12 = results.data.split(",")[0];
        $scope.porcentajeIva12 = results.data.split(",")[1];
        $scope.codigoIva14 = results.data.split(",")[2];
        $scope.porcentajeIva14 = results.data.split(",")[3];

        if (validate_fechaMayorQue($scope.txtFechaEmision, "01/06/2016") && validate_fechaMayorQue("31/05/2017",$scope.txtFechaEmision )) {
            $scope.porcentaje = $scope.porcentajeIva14;
        } else {
            $scope.porcentaje = $scope.porcentajeIva12;
        }

    }, function (error) {

    });

    $scope.funcCalculaTotn = function () {
        var subTotal_0 = parseFloat($("#txtSubTot_0").val());
        var subTotal_12 = parseFloat($("#txtSubTot_12").val());
        var descuento_0 = parseFloat($("#txtDescuento_0").val());
        var descuento_12 = parseFloat($("#txtDescuento_12").val());
        var ice = parseFloat($("#txtIce").val());
        var irbpnr_0 = parseFloat($("#txtIrbpnr_0").val());
        var irbpnr_12 = parseFloat($("#txtIrbpnr_12").val());
        var por = parseFloat($scope.porcentaje) / 100;
        if (isNaN(subTotal_0)) subTotal_0 = parseFloat("0");
        if (isNaN(subTotal_12)) subTotal_12 = parseFloat("0");
        if (isNaN(descuento_0)) descuento_0 = parseFloat("0");
        if (isNaN(descuento_12)) descuento_12 = parseFloat("0");
        if (isNaN(ice)) ice = parseFloat("0");
        if (isNaN(irbpnr_0)) irbpnr_0 = parseFloat("0");
        if (isNaN(irbpnr_12)) irbpnr_12 = parseFloat("0");

        var compesacion = 0;
        if ($("#chkcompesacion").is(":checked") == true) {
            compesacion = (subTotal_12 - descuento_12 + ice) * parseFloat("0.02");
            if (isNaN(compesacion)) compesacion = parseFloat("0");

            $("#txtcompesacion").val(compesacion.toFixed(2));
            $scope.txtcompesacion = compesacion.toFixed(2);
            $scope.hidecompesacion = false;
        } else {
            compesacion = 0;
            $("#txtcompesacion").val(compesacion.toFixed(2));
            $scope.txtcompesacion = compesacion.toFixed(2);
            $scope.hidecompesacion = true;
        }



        


        var iva_12 = (subTotal_12 - descuento_12 + ice) * parseFloat(por);
        var valTotal = (subTotal_0 + subTotal_12) - (descuento_0 + descuento_12) + iva_12;
        valTotal = valTotal + ice + irbpnr_0 + irbpnr_12;

        var valTotal1 = (subTotal_0 + subTotal_12) - (descuento_0 + descuento_12) + iva_12;
        valTotal1 = valTotal1 + ice + irbpnr_0 + irbpnr_12 - compesacion;

     
        $("#txtValPagado").val(valTotal1.toFixed(2));
        $scope.txtValPagado = (valTotal1).toFixed(2);


        $("#txtSubTotSinImp").val((subTotal_12 + subTotal_0).toFixed(2));
        $scope.txtSubTotSinImp = (subTotal_12 + subTotal_0).toFixed(2);
        $("#txtTotDescuento").val((descuento_0 + descuento_12).toFixed(2));
        $scope.txtTotDescuento = (descuento_0 + descuento_12).toFixed(2);
        $("#txtTotIrbpnr").val((irbpnr_0 + irbpnr_12).toFixed(2));
        $scope.txtTotIrbpnr = (irbpnr_0 + irbpnr_12).toFixed(2);
        $("#txtIva_12").val(iva_12.toFixed(2));
        $scope.txtIva_12 = (iva_12).toFixed(2);
        $("#txtValTotal").val(valTotal.toFixed(2));
        $scope.txtValTotal = (valTotal).toFixed(2);

        $("#txtSubTot_0").val(subTotal_0.toFixed(2));
        $scope.txtSubTot_0 = (subTotal_0).toFixed(2);
        $("#txtSubTot_12").val(subTotal_12.toFixed(2));
        $scope.txtSubTot_12 = (subTotal_12).toFixed(2);
        $("#txtDescuento_0").val(descuento_0.toFixed(2));
        $scope.txtDescuento_0 = (descuento_0).toFixed(2);
        $("#txtDescuento_12").val(descuento_12.toFixed(2));
        $scope.txtDescuento_12 = (descuento_12).toFixed(2);
        $("#txtIce").val(ice.toFixed(2));
        $scope.txtIce = (ice).toFixed(2);
        $("#txtIrbpnr_0").val(irbpnr_0.toFixed(2));
        $scope.txtIrbpnr_0 = (irbpnr_0).toFixed(2);
        $("#txtIrbpnr_12").val(irbpnr_12.toFixed(2));
        $scope.txtIrbpnr_12 = (irbpnr_12).toFixed(2);


    };


    $scope.funcCalculaTot = function () {
        var subTotal_0 = parseFloat($("#txtSubTot_0").val());
        var subTotal_12 = parseFloat($("#txtSubTot_12").val());
        var descuento_0 = parseFloat($("#txtDescuento_0").val());
        var descuento_12 = parseFloat($("#txtDescuento_12").val());
        var ice = parseFloat($("#txtIce").val());
        var irbpnr_0 = parseFloat($("#txtIrbpnr_0").val());
        var irbpnr_12 = parseFloat($("#txtIrbpnr_12").val());
        var por = parseFloat($("#txtporcentaje").val()) / 100;
        if (isNaN(subTotal_0)) subTotal_0 = parseFloat("0");
        if (isNaN(subTotal_12)) subTotal_12 = parseFloat("0");
        if (isNaN(descuento_0)) descuento_0 = parseFloat("0");
        if (isNaN(descuento_12)) descuento_12 = parseFloat("0");
        if (isNaN(ice)) ice = parseFloat("0");
        if (isNaN(irbpnr_0)) irbpnr_0 = parseFloat("0");
        if (isNaN(irbpnr_12)) irbpnr_12 = parseFloat("0");

        var compesacion = 0.0;
        if ($("#chkcompesacion").is(":checked") == true){
            compesacion = (subTotal_12 - descuento_12 + ice) * parseFloat("0.02");
            if (isNaN(compesacion)) compesacion = parseFloat("0");

            $("#txtcompesacion").val(compesacion.toFixed(2));
            $scope.txtcompesacion = compesacion.toFixed(2);
        } else
        {
            $("#txtcompesacion").val(compesacion.toFixed(2));
            $scope.txtcompesacion = compesacion.toFixed(2);
        }
            


       


        var iva_12 = (subTotal_12 - descuento_12 + ice) * parseFloat(por);
        var valTotal = (subTotal_0 + subTotal_12) - (descuento_0 + descuento_12) + iva_12;
        valTotal = valTotal + ice + irbpnr_0 + irbpnr_12 ;

        var valTotal1 = (subTotal_0 + subTotal_12) - (descuento_0 + descuento_12) + iva_12;
        valTotal1 = valTotal1 + ice + irbpnr_0 + irbpnr_12 - compesacion;


        $("#txtValPagado").val(valTotal1.toFixed(2));
        $scope.txtValPagado = (valTotal1).toFixed(2);

        $("#txtSubTotSinImp").val((subTotal_12 + subTotal_0).toFixed(2));
        $scope.txtSubTotSinImp = (subTotal_12 + subTotal_0).toFixed(2);
        $("#txtTotDescuento").val((descuento_0 + descuento_12).toFixed(2));
        $scope.txtTotDescuento = (descuento_0 + descuento_12).toFixed(2);
        $("#txtTotIrbpnr").val((irbpnr_0 + irbpnr_12).toFixed(2));
        $scope.txtTotIrbpnr = (irbpnr_0 + irbpnr_12).toFixed(2);
        $("#txtIva_12").val(iva_12.toFixed(2));
        $scope.txtIva_12 = (iva_12).toFixed(2);
        $("#txtValTotal").val(valTotal.toFixed(2));
        $scope.txtValTotal = (valTotal).toFixed(2);

        $("#txtSubTot_0").val(subTotal_0.toFixed(2));
        $scope.txtSubTot_0 = (subTotal_0).toFixed(2);
        $("#txtSubTot_12").val(subTotal_12.toFixed(2));
        $scope.txtSubTot_12 = (subTotal_12).toFixed(2);
        $("#txtDescuento_0").val(descuento_0.toFixed(2));
        $scope.txtDescuento_0 = (descuento_0).toFixed(2);
        $("#txtDescuento_12").val(descuento_12.toFixed(2));
        $scope.txtDescuento_12 = (descuento_12).toFixed(2);
        $("#txtIce").val(ice.toFixed(2));
        $scope.txtIce = (ice).toFixed(2);
        $("#txtIrbpnr_0").val(irbpnr_0.toFixed(2));
        $scope.txtIrbpnr_0 = (irbpnr_0).toFixed(2);
        $("#txtIrbpnr_12").val(irbpnr_12.toFixed(2));
        $scope.txtIrbpnr_12 = (irbpnr_12).toFixed(2);


    };

    $scope.btnSubmitClick = function () {
        if ($scope.ErrorAlValidarAntesGrabar()) {
            return;
        }
        $scope.pOmitirDifTotFact = false;//debo limpiar para que se vuelva a validar en siguiente click
        var objData = {};
        objData.tipoACCION = "";
        objData.idDocumento = $scope.pIdDocumento;
        objData.codSAP = $scope.pCodSAP;
        objData.idPedido = $scope.pIdPedido;
        objData.numPedido = $scope.txtNumPedido;
        objData.facEstabl = $scope.txtFacEstabl;
        objData.facPtoEmi = $scope.txtFacPtoEmi;
        objData.facNumSec = $scope.txtFacNumSec;
        objData.fechaEmision = $filter('date')($scope.txtFechaEmision, $scope.frmtFECHA); //paso como string para el XML y el SP
        objData.claveAcceso = $scope.pClaveAcceso;
        objData.nombreArchivo = $scope.pNombreArchivo;
        objData.nomProveedor = $scope.txtNombre;
        objData.nomComercial = $scope.pNombreComercial;
        objData.rucProveedor = $scope.txtRuc;
        objData.dirMatriz = $scope.txtDirMatriz;
        objData.fechaIniVigAut = $filter('date')($scope.txtFechaIniVigAut, $scope.frmtFECHA); //paso como string para el XML
        objData.fechaFinVigAut = $filter('date')($scope.txtFechaFinVigAut, $scope.frmtFECHA); //paso como string para el XML
        objData.numAutorizaTal = $scope.txtNumAutorizaTal;
        objData.nomEmpresa = $scope.txtRazonSoc;
        objData.rucEmpresa = $scope.txtIdentificacion;
        objData.dirSucursal = $scope.txtDirSucursal;
        objData.detSubTotSinImp = $scope.txtSubTotSinImp;
        objData.detTotDescuento = $scope.txtTotDescuento;
        objData.detPropina = $scope.txtPropina;
        objData.detValTotal = $scope.txtValTotal;
        objData.detSubTot_0 = $scope.txtSubTot_0;
        objData.detDescuento_0 = $scope.txtDescuento_0;
        objData.detSubTot_12 = $scope.txtSubTot_12;
        objData.detDescuento_12 = $scope.txtDescuento_12;
        objData.detIce = $scope.txtIce;
        objData.detIva_12 = $scope.txtIva_12;
        objData.detSubTotNoObjIva = $scope.txtSubTotNoObjIva;
        objData.detSubTotExenIva = $scope.txtSubTotExenIva;
        objData.detTotIrbpnr = $scope.txtTotIrbpnr;
        objData.detIrbpnr_0 = $scope.txtIrbpnr_0;
        objData.detIrbpnr_12 = $scope.txtIrbpnr_12;
        objData.codAlmacen = $scope.txtAlmacenOrigenCod;
        objData.nomAlmacen = $scope.txtAlmacenOrigenDes;
        objData.txtcompesacion = $scope.txtcompesacion;
        objData.chkcompesacion = $scope.chkcompesacion;
        objData.xmlSRI = ""; //siempre se genera en el WEB.API   //$scope.pXmlSRI;
        // INI - CASE X GRABAR
        if ($scope.pOpcSubmit == "G") {
            if ($scope.pIdDocumento == 0) {
                objData.tipoACCION = "I";
            }
            else {
                objData.tipoACCION = "A";
            }
        }
        // FIN - CASE X GRABAR
        // INI - OPCION PARA EL EVENTO ENVIAR
        if ($scope.pOpcSubmit == "E") {
            objData.tipoACCION = "E";
        }
        // FIN - OPCION PARA EL EVENTO ENVIAR
        $scope.myPromise = FacturasService.getGrabaDocumento(objData).then(function (results) {
            if (results.data.success) {
                if ($scope.pOpcSubmit == "G") {
                    $scope.CambiaAccionFrm("A");
                    if ($scope.pIdDocumento == 0) {
                        $scope.pIdDocumento = parseInt(results.data.root[0][0]);
                        $scope.showMessage('M', 'Factura ingresada correctamente.');
                    }
                    else {
                        $scope.showMessage('M', 'Factura grabada correctamente.');
                    }
                }
                if ($scope.pOpcSubmit == "E") {
                    $scope.CambiaAccionFrm("E");
                    $scope.pNombreArchivo = results.data.root[0][1];
                    $scope.pClaveAcceso = results.data.root[0][2];
                    $scope.pXmlSRI = results.data.root[0][3];
                    if (results.data.msgError == '') {
                        $scope.showMessage('M', 'Factura enviada correctamente.');
                    }
                    else {
                        var sError = "Factura enviada correctamente pero con observación: " + $scope.sepEnter + $scope.sepEnter + results.data.msgError
                                   + $scope.sepEnter + $scope.sepEnter + " Favor Reenvíe nuevamente esta Factura para evitar retrasos en el proceso."
                                   + $scope.sepEnter + $scope.sepEnter + " (opción -Modificar- luego nuevamente opción -Enviar-)";
                        $scope.showMessage('I', 'Factura enviada correctamente.' + sError);
                    }
                }
            }
            else {
                $scope.showMessage('E', 'Error al grabar: ' + results.data.msgError);
            }
            $scope.pOpcSubmit = "";
        },
         function (error) {
             $scope.pOpcSubmit = "";
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
        //orden de ejecucion: 1) este metodo, 2) validacion de controles, 3) metodo del submit
        $scope.pOpcSubmit = "G";
    };

    $scope.btnEnviarClick = function () {
        //orden de ejecucion: 1) este metodo, 2) validacion de controles, 3) metodo del submit
        $scope.pOpcSubmit = "E";
    };

    $scope.btnAnularClick = function () {
        $scope.tipoConfirma = "AF";
        $scope.MenjConfirmacion = "¿ESTÁ SEGURO QUE DESEA ANULAR ESTA FACTURA NO ELECTRÓNICA?";
        $('#idMensajeConfirmacion').modal('show');
    };

    $scope.voidAnular = function () {
        $scope.myPromise = FacturasService.getGrabaAnulaDocumento($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, $scope.pIdDocumento.toString()).then(function (results) {
            if (results.data.success) {
                $scope.pIdDocumento = 0;
                $scope.CambiaAccionFrm("");
                $scope.showMessage('M', 'Factura Anulada  correctamente.');
                $('#btnMensajeOK').click(function () {
                    window.location = $scope.pUrlRegresar;
                });
            }
            else {
                $scope.showMessage('E', 'Error al anular: ' + results.data.msgError);
            }
        }, function (error) {
            var errors = [];
            for (var key in error.data.modelState) {
                for (var i = 0; i < error.data.modelState[key].length; i++) {
                    errors.push(error.data.modelState[key][i]);
                }
            }
            $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
        });
    };

    $scope.btnModificarClick = function () {
        $scope.myPromise = FacturasService.getValidaPermisoModFact($scope.pCodSAP, $scope.pRuc, $scope.pUsuario,
            $scope.txtNumPedido, $scope.txtFacEstabl, $scope.txtFacPtoEmi, $scope.txtFacNumSec).then(function (results) {
                if (results.data.success) {
                    if (results.data.root[0] == "S") {
                        $scope.CambiaAccionFrm("M");
                    }
                    else {
                        $scope.showMessage('E', 'La Factura ya no puede ser modificada. ');
                    }
                }
                else {
                    $scope.showMessage('E', 'Error al validar permisos de modificación: ' + results.data.msgError);
                }
            }, function (error) {
                var errors = [];
                for (var key in error.data.modelState) {
                    for (var i = 0; i < error.data.modelState[key].length; i++) {
                        errors.push(error.data.modelState[key][i]);
                    }
                }
                $scope.showMessage('E', "Error al establecer comunicación con SAP: " + errors.join(' '));
            });
    };

    $scope.btnGetXMLClick = function () {
        var objFile = {};
        objFile.tipo = 'XML';
        objFile.codSap = $scope.pCodSAP;
        objFile.ruc = $scope.pRuc;
        objFile.usuario = $scope.pUsuario;
        objFile.nombreFile = $scope.pNombreArchivo;
        objFile.dataXML = $scope.pXmlSRI;
        $scope.myPromise = FacturasService.getGeneraFilesXmlPDF(objFile).then(function (results) {
            if (results.data.success) {
                window.open(results.data.root[0], '_blank');
            }
            else {
                $scope.showMessage('E', 'Error al obtener archivo XML: ' + results.data.msgError);
            }
        }, function (error) {
            var errors = [];
            for (var key in error.data.modelState) {
                for (var i = 0; i < error.data.modelState[key].length; i++) {
                    errors.push(error.data.modelState[key][i]);
                }
            }
            $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
        });
    };

    $scope.btnGetPDFClick = function () {
        var objFile = {};
        objFile.tipo = 'PDF';
        objFile.codSap = $scope.pCodSAP;
        objFile.ruc = $scope.pRuc;
        objFile.usuario = $scope.pUsuario;
        objFile.nombreFile = $scope.pNombreArchivo;
        objFile.dataXML = $scope.pXmlSRI;
        $scope.myPromise = FacturasService.getGeneraFilesXmlPDF(objFile).then(function (results) {
            if (results.data.success) {
                window.open(results.data.root[0], '_blank');
            }
            else {
                $scope.showMessage('E', 'Error al obtener archivo PDF: ' + results.data.msgError);
            }
        }, function (error) {
            var errors = [];
            for (var key in error.data.modelState) {
                for (var i = 0; i < error.data.modelState[key].length; i++) {
                    errors.push(error.data.modelState[key][i]);
                }
            }
            $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
        });
    };

    $scope.btnRegresarClick = function () {
        //event.preventDefault();
        //event.stopPropagation();
        window.location = $scope.pUrlRegresar;
    };

    $scope.CambiaAccionFrm = function (cAccion) {
        $scope.hide_btnEnviar = true;
        $scope.hide_btnAnular = true;
        $scope.hide_btnGrabar = true;
        $scope.hide_btnModificar = true;
        $scope.hide_btnGetXML = true;
        $scope.hide_btnGetPDF = true;
        $scope.HabilitaControles(false);
        switch (cAccion) {
            case "N":
                $scope.hide_btnGrabar = false;
                $scope.HabilitaControles(true);
                break;
            case "A":
                $scope.hide_btnEnviar = false;
                $scope.hide_btnGrabar = false;
                $scope.HabilitaControles(true);
                break;
            case "E":
                $scope.hide_btnModificar = false;
                //$scope.hide_btnGetXML = false;
                $scope.hide_btnGetPDF = false;
                break;
            case "M":
                $scope.hide_btnEnviar = false;
                $scope.hide_btnAnular = false;
                $scope.HabilitaControles(true);
                $scope.disableFactCtr = true;
                break;
        };
    };

    $scope.HabilitaControles = function (bValor) {
        bValor = !bValor;
        $scope.disableFactCtr = bValor;
        $scope.disableAllCtr = bValor;
    };

    $scope.ConsultaPedido = function (idPedido) {
        $scope.myPromise = FacturasService.getConsultaPedidoNumero($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, idPedido.toString()).then(function (results) {
            if (results.data.success) {
                var cab = results.data.root[0][0];
                $scope.pIdPedido = cab.idPedido;
                $scope.txtNumPedido = cab.numPedido;
                $scope.txtNombre = cab.nomProveedor;
                $scope.pNombreComercial = cab.nomComercial;
                $scope.txtRuc = cab.rucProveedor;
                $scope.txtDirMatriz = cab.dirCallePrinc + " " + cab.dirCalleNum + " " + cab.dirPisoEdificio;
                $scope.txtDirSucursal = $scope.txtDirMatriz;
                $scope.txtRazonSoc = cab.nomEmpresa;
                $scope.txtIdentificacion = cab.rucEmpresa;
                $scope.txtFechaEmision = new Date();
                var Fecha1 = $filter('date')($scope.txtFechaEmision, 'dd/MM/yyyy');
                if (validate_fechaMayorQue(Fecha1, "01/06/2016") && validate_fechaMayorQue("31/05/2017", Fecha1)) {
                    $scope.porcentaje = $scope.porcentajeIva14;
                } else {
                    $scope.porcentaje = $scope.porcentajeIva12;
                }

                $scope.txtAlmacenOrigenCod = cab.codAlmacen;
                $scope.txtAlmacenOrigenDes = cab.nomAlmacen;
                $scope.txtFechaIniVigAut = "";
                $scope.txtFechaFinVigAut = "";
                $scope.pFechaPedido = $scope.convertStrToDATE(cab.fechaPedido);
                $scope.pValTotalPedido = cab.valTotalPedido;
                $scope.pValTotSumaFact = cab.subTotalSumaFacturas;
                $scope.pValSaldoPendFact = $scope.pValTotalPedido - $scope.pValTotSumaFact;

                $scope.resDgItemsDisp = results.data.root[0][1];
                $scope.etiTotRegistros = $scope.resDgItemsDisp.length.toString();

                $scope.CambiaAccionFrm("N");

                setTimeout(function () { $('#lnkAccordion').focus(); }, 100);
                setTimeout(function () { $('#btnRegresar').focus(); }, 150);
            }
            else {
                $scope.resDgItemsDisp = [];
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
        }, function (error) {
            var errors = [];
            for (var key in error.data.modelState) {
                for (var i = 0; i < error.data.modelState[key].length; i++) {
                    errors.push(error.data.modelState[key][i]);
                }
            }
            $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
        });
    };

    $scope.ConsultaFactura = function (idFactura) {
        $scope.myPromise = FacturasService.getConsultaDocumentoId($scope.pCodSAP, $scope.pRuc, $scope.pUsuario, idFactura.toString()).then(function (results) {
            if (results.data.success) {
                var cab = results.data.root[0][0];
                $scope.pIdDocumento = idFactura;
                $scope.pIdPedido = cab.idPedido;
                if (cab.nombreArchivo != '') {
                    $scope.pNombreArchivo = cab.nombreArchivo;
                    $scope.pXmlSRI = cab.xmlSRI;
                    $scope.pClaveAcceso = cab.claveAcceso;
                }
                $scope.txtNombre = cab.nomProveedor;
                $scope.pNombreComercial = cab.nomComercial;
                $scope.txtRuc = cab.rucProveedor;
                $scope.txtFacEstabl = cab.facEstabl;
                $scope.txtFacPtoEmi = cab.facPtoEmi;
                $scope.txtFacNumSec = cab.facNumSec;
                $scope.txtDirMatriz = cab.dirMatriz;
                $scope.txtDirSucursal = cab.dirSucursal;
                $scope.txtFechaEmision = $scope.convertStrToDATE(cab.fechaEmision);
                var Fecha1 = $filter('date')($scope.txtFechaEmision, 'dd/MM/yyyy');
                if (validate_fechaMayorQue(Fecha1, "01/06/2016") && validate_fechaMayorQue("31/05/2017", Fecha1)) {
                    $scope.porcentaje = $scope.porcentajeIva14;
                } else {
                    $scope.porcentaje = $scope.porcentajeIva12;
                }
              
                $scope.chkcompesacion = cab.chkcompesacion;
                $scope.txtFechaIniVigAut = $scope.convertStrToDATE(cab.fechaIniVigAut);
                $scope.txtFechaFinVigAut = $scope.convertStrToDATE(cab.fechaFinVigAut);
                $scope.txtNumAutorizaTal = cab.numAutorizaTal;
                $scope.txtRazonSoc = cab.nomEmpresa;
                $scope.txtIdentificacion = cab.rucEmpresa;
                $scope.txtNumPedido = cab.numPedido;
                $scope.pFechaPedido = $scope.convertStrToDATE(cab.fechaPedido);
                $scope.txtAlmacenOrigenCod = cab.codAlmacen;
                $scope.txtAlmacenOrigenDes = cab.nomAlmacen;
                $scope.pValTotSumaFact = cab.subTotalSumaFacturas;
                $scope.pValTotalPedido = cab.valTotalPedido;
                $scope.pValSaldoPendFact = $scope.pValTotalPedido - $scope.pValTotSumaFact;
         


                $scope.resDgItemsDisp = results.data.root[0][1];
                $scope.etiTotRegistros = $scope.resDgItemsDisp.length.toString();

                if (cab.estado == "IN") {
                    $scope.CambiaAccionFrm("A");
                }
                else {
                    $scope.CambiaAccionFrm("E");
                    if (cab.estado == "RE") {
                        $scope.hide_btnModificar = true;
                    }
                }

                $scope.txtSubTot_0 = cab.detSubTot_0;
                $scope.txtDescuento_0 = cab.detDescuento_0;
                $scope.txtIrbpnr_0 = cab.detIrbpnr_0;
                $scope.txtSubTot_12 = cab.detSubTot_12;
                $scope.txtDescuento_12 = cab.detDescuento_12;
                $scope.txtIce = cab.detIce;
                $scope.txtIrbpnr_12 = cab.detIrbpnr_12;


                if (cab.chkcompesacion == true) {
                    $scope.hidecompesacion = false;

                    var compesacion = (cab.detSubTot_12 - cab.detDescuento_12 + cab.detIce) * parseFloat("0.02");
                    if (isNaN(compesacion)) compesacion = parseFloat("0");
                    $scope.txtcompesacion = compesacion.toFixed(2);
                } else {
                    $scope.hidecompesacion = true;
                }

                //invocar a FUNCION: funcCalculaTot()
                setTimeout(function () { $("#txtSubTot_0").trigger("change"); }, 100);

                setTimeout(function () { $('#lnkAccordion').focus(); }, 150);
                setTimeout(function () { $('#btnRegresar').focus(); }, 200);
                //$scope.funcCalculaTotn();
            }
            else {
                $scope.resDgItemsDisp = [];
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
        }, function (error) {
            var errors = [];
            for (var key in error.data.modelState) {
                for (var i = 0; i < error.data.modelState[key].length; i++) {
                    errors.push(error.data.modelState[key][i]);
                }
            }
            $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
        });
    };

    $scope.ErrorAlValidarAntesGrabar = function () {
        var Result = true;
        //inicio validaciones
        if ($scope.txtRuc == '') {
            $scope.showMessage('I', 'ERROR: No existe el número de identificación de la empresa.');
            return Result;
        }
        if ($scope.txtNombre == '') {
            $scope.showMessage('I', 'ERROR: No existe la razón social de la empresa.');
            return Result;
        }
        if ($scope.txtIdentificacion == '') {
            $scope.showMessage('I', 'ERROR: No existe el número de identificación del cliente.');
            return Result;
        }
        if ($scope.txtRazonSoc == '') {
            $scope.showMessage('I', 'ERROR: No existe la razón social del cliente.');
            return Result;
        }
        if ($scope.txtDirMatriz == '') {
            $scope.showMessage('I', 'Ingrese la Dirección Matriz.');
            return Result;
        }
        if ($scope.txtFacEstabl.length < 3) {
            $scope.showMessage('I', 'Ingrese los 3 dígitos del establecimiento del Nro. de la Factura.');
            return Result;
        }
        if ($scope.txtFacPtoEmi.length < 3) {
            $scope.showMessage('I', 'Ingrese los 3 dígitos del punto de emisión del Nro. de la Factura.');
            return Result;
        }
        if ($scope.txtFacNumSec.length < 9) {
            $scope.showMessage('I', 'Ingrese los 9 dígitos del secuencial del Nro. de la Factura.');
            return Result;
        }
        if ($scope.txtFechaEmision == null || $scope.txtFechaEmision == '') {
            $scope.showMessage('I', 'Ingrese la fecha de emisión.');
            return Result;
        }
        if ($scope.txtFechaEmision < $scope.pFechaPedido) {
            $scope.showMessage('I', 'La fecha de emisión no puede ser menor a la fecha del pedido: ' + $filter('date')($scope.pFechaPedido, 'dd-MM-yyyy') + '.');
            return Result;
        }
        
        var datos = "NO";
        for (var i = 0; i < $scope.codigosproveedor.length; i++) {
            if ($scope.codigosproveedor[i] == $scope.pCodSAP) {
                datos = "SI";
            }
        }
        if (datos == "NO") {
            var fechaVenPed = new Date($scope.pFechaPedido.getFullYear(), $scope.pFechaPedido.getMonth(), $scope.pFechaPedido.getDate() + 15, 23, 59);
            if ($scope.txtFechaEmision > fechaVenPed) {
                $scope.showMessage('I', 'La fecha de emisión no puede ser mayor al plazo del pedido que vence el ' + $filter('date')(fechaVenPed, 'dd-MM-yyyy') + '.');
                return Result;
            }
        }

       
        if ($scope.txtFechaIniVigAut == null || $scope.txtFechaIniVigAut == '') {
            $scope.showMessage('I', 'Ingrese la fecha del inicio de la vigencia de la autorización del talonario.');
            return Result;
        }
        if ($scope.txtFechaIniVigAut > $scope.txtFechaEmision) {
            $scope.showMessage('I', 'La fecha del inicio de la vigencia de la autorización del talonario no puede ser mayor a la fecha de emisión.');
            return Result;
        }
        var fechaValTmp = new Date($scope.txtFechaIniVigAut.getFullYear() - 15, $scope.txtFechaIniVigAut.getMonth(), $scope.txtFechaIniVigAut.getDate());
        if ($scope.txtFechaIniVigAut < fechaValTmp) {
            $scope.showMessage('I', 'La fecha del inicio de la vigencia de la autorización del talonario no puede tener tal antiguedad, favor revisar.');
            return Result;
        }
        if ($scope.txtFechaFinVigAut == null || $scope.txtFechaFinVigAut == '') {
            $scope.showMessage('I', 'Ingrese la fecha del fin de la vigencia de la autorización del talonario.');
            return Result;
        }
        if ($scope.txtFechaFinVigAut < $scope.txtFechaEmision) {
            $scope.showMessage('I', 'La fecha del fin de la vigencia de la autorización del talonario no puede ser menor a la fecha de emisión.');
            return Result;
        }
        if ($scope.txtNumAutorizaTal.length < 10) {
            $scope.showMessage('I', 'Ingrese los 10 dígitos del Número de Autorización del SRI.');
            return Result;
        }
        //validaciones de valores
        if (parseFloat($scope.txtDescuento_0) > parseFloat($scope.txtSubTot_0)) {
            $scope.showMessage('I', 'El valor del descuento 0% no puede ser mayor al valor del subtotal 0%.');
            return Result;
        }
        if (parseFloat($scope.txtDescuento_12) > parseFloat($scope.txtSubTot_12)) {
            $scope.showMessage('I', 'El valor del descuento 14% no puede ser mayor al valor del subtotal 14%.');
            return Result;
        }
        if (parseFloat($scope.txtIce) > parseFloat('0') && parseFloat($scope.txtSubTot_12) < parseFloat('0.01')) {
            $scope.showMessage('I', 'No es permitido ingresar el valor del ICE sin primero ingresar el valor del subtotal 14%.');
            return Result;
        }
        if (parseFloat($scope.txtIrbpnr_0) > parseFloat('0') && parseFloat($scope.txtSubTot_0) < parseFloat('0.01')) {
            $scope.showMessage('I', 'No es permitido ingresar el valor del IRBPNR base 0% sin primero ingresar el valor del subtotal 0%.');
            return Result;
        }
        if (parseFloat($scope.txtIrbpnr_12) > parseFloat('0') && parseFloat($scope.txtSubTot_12) < parseFloat('0.01')) {
            $scope.showMessage('I', 'No es permitido ingresar el valor del IRBPNR base 14% sin primero ingresar el valor del subtotal 14%.');
            return Result;
        }
        if (parseFloat($scope.txtValTotal) < parseFloat('0.01')) {
            $scope.showMessage('I', 'El valor del Total de la Factura no puede ser igual o menor a cero.');
            return Result;
        }
        if ((parseFloat($scope.txtSubTotSinImp) - parseFloat($scope.txtTotDescuento)) > parseFloat(($scope.pValTotalPedido - $scope.pValTotSumaFact).toFixed(2))) {
            if ($scope.pOmitirDifTotFact == false) {
                var valorFact = (parseFloat($scope.txtSubTotSinImp) - parseFloat($scope.txtTotDescuento)).toFixed(2);
                var mensjConf = " El Total Sin Impuestos (menos descuentos) de la Factura es: [$ " + valorFact +
                    "] " + $scope.sepEnter + $scope.sepEnter + " Este valor excede el Saldo Pendiente del Pedido que puede ser facturado: [$ " + $scope.pValSaldoPendFact.toFixed(2) + "]." +
                    $scope.sepEnter + $scope.sepEnter + " Revise los valores: " + $scope.sepEnter + " - Sub Total del Pedido (menos descuentos) [$ " + $scope.pValTotalPedido.toFixed(2) +
                    "] " + $scope.sepEnter + " - Sub Total Facturado  (menos descuentos)  [$ " + $scope.pValTotSumaFact.toFixed(2) + "]";
                $scope.tipoConfirma = "OE";
                mensjConf = mensjConf + $scope.sepEnter + $scope.sepEnter + $scope.sepEnter + " Estimado Proveedor:" +
                    $scope.sepEnter + $scope.sepEnter + " El valor de los productos que está registrando en su factura es superior al valor de la mercadería solicitada en nuestro Pedido." +
                    $scope.sepEnter + " Se permitirá el registro de este documento pero el mismo será objeto de revisión por nuestra área de control interno." +
                    $scope.sepEnter + $scope.sepEnter + " CORPORACION EL ROSADO";
                $scope.MenjConfirmacion = mensjConf;
                $('#idMensajeConfirmacion').modal('show');
                return Result;
            }
        }
        //fin validaciones
        Result = false;
        return Result;
    };

    $scope.cambioFecha = function () {
        
        var Fecha1 = $filter('date')($scope.txtFechaEmision, 'dd/MM/yyyy');
        if (Fecha1 != undefined) {
            if (validate_fechaMayorQue(Fecha1, "01/06/2016") && validate_fechaMayorQue("31/05/2017", Fecha1)) {
            $scope.porcentaje = $scope.porcentajeIva14;
        } else {
            $scope.porcentaje = $scope.porcentajeIva12;
        }
        $scope.funcCalculaTotn();
        }
    }

    $scope.grabar = function () {
        if ($scope.tipoConfirma == "OE") {//omitir error y grabar/enviar
            $scope.pOmitirDifTotFact = true;
            $scope.btnSubmitClick();
        }
        if ($scope.tipoConfirma == "AF") {//anular factura
            $scope.voidAnular();
        }
    };

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

    function validate_fechaMayorQue(fechaInicial, fechaFinal) {

        valuesStart = fechaInicial.split("/");

        valuesEnd = fechaFinal.split("/");



        // Verificamos que la fecha no sea posterior a la actual

        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);

        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);

        if (dateStart >= dateEnd) {

            return 1;

        }

        return 0;

    }

    // ini - recepcion de parametros
    if ($scope.pCodSAP == null || $scope.pCodSAP == '') {
        $scope.showMessage('E', 'Transacción o acción no permitida, la sesión actual no está activa.');
        $('#btnMsjError').click(function () {
            window.location = "/Home/IndexPrivado";
        });
    }
    else {
        if (localStorageService.get('facControlFact')) {
            var tipoFrmInvoca = localStorageService.get('facControlFact').tipoID;
            var NumID_FrmInvoca = localStorageService.get('facControlFact').numID;
            $scope.codigoIva12 = localStorageService.get('facControlFact').codigoIva12;
            $scope.porcentajeIva12 = localStorageService.get('facControlFact').porcentajeIva12;
            $scope.codigoIva14 = localStorageService.get('facControlFact').codigoIva14;
            $scope.porcentajeIva14 = localStorageService.get('facControlFact').porcentajeIva14;

            localStorageService.remove('facControlFact');
            if (tipoFrmInvoca == "P") {
                $scope.pUrlRegresar = "/Facturacion/frmSelPedidos";
                $scope.ConsultaPedido(NumID_FrmInvoca);
     
            }
            if (tipoFrmInvoca == "F") {
                $scope.pUrlRegresar = "/Facturacion/frmSelFacturas";
                $scope.ConsultaFactura(NumID_FrmInvoca);
                $scope.cambioFecha();
                $scope.funcCalculaTotn();
            }
            //FacturasService.cargarParametros("Iva").then(function (results) {
            //    $scope.codigoIva12 = results.data.split(",")[0];
            //    $scope.porcentajeIva12 = results.data.split(",")[1];
            //    $scope.codigoIva14 = results.data.split(",")[2];
            //    $scope.porcentajeIva14 = results.data.split(",")[3];

            //    if (validate_fechaMayorQue($scope.txtFechaEmision, "31/05/2016")) {
            //        $scope.porcentaje = $scope.porcentajeIva14;
            //    } else {
            //        $scope.porcentaje = $scope.porcentajeIva12;
            //    }

            //}, function (error) {

            //});
        }
        else {
            $scope.showMessage('E', 'Transacción o acción no permitida, debe ser invocada desde una transacción previa.');
            $('#btnMsjError').click(function () {
                window.location = "/Home/IndexPrivado";
            });
        }
    }
    // fin - recepcion de parametros

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

app.directive('ngFocus', ['$parse', function ($parse) {
    return function (scope, element, attr) {
        var fn = $parse(attr['ngFocus']);
        element.bind('focus', function (event) {
            scope.$apply(function () {
                fn(scope, { $event: event });
            });
        });
    }
}]);

app.directive('ngBlur', ['$parse', function ($parse) {
    return function (scope, element, attr) {
        var fn = $parse(attr['ngBlur']);
        element.bind('blur', function (event) {
            scope.$apply(function () {
                fn(scope, { $event: event });
            });
        });
    }
}]);

app.directive('loseFocus', function () {
    return {
        link: function (scope, element, attrs) {
            scope.$watch(attrs.loseFocus, function (value) {
                if (value === true) {
                    console.log('value=', value);
                    element[0].blur();
                }
            });
        }
    };
});