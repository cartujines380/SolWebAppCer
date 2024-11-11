Date.prototype.AddDays = function (noOfDays) {
    this.setTime(this.getTime() + (noOfDays * (1000 * 60 * 60 * 24)));
    return this;
}
app.directive('uploadermodel', ["$parse", function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, iElement, iAttrs) {
            iElement.on("change", function (e) {
                $parse(iAttrs.uploaderModel).assign(scope, iElement[0].files[0]);
            });
        }
    }
}]);
app.controller('ChoferesProveedorController', ['$scope', '$location', 'TransporteProveedorService', '$sce', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', '$http', 'authService', function ($scope, $location, TransporteProveedorService, $sce, $cookies, ngAuthSettings, FileUploader, $filter, $http, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var Ruta = serviceBase + 'api/FileTransporte/UploadFile/?direccion=prueba';
    var uploader = $scope.uploader = new FileUploader({
        url: Ruta
    });

    $scope.report = {}
    $scope.report.id = "";
    $scope.report.viewDetailPath = "";
    $scope.pagesPedidos = [];
    $scope.etiTotRegistros = "";
    $scope.etiTotRegistrosLP = "";
    $scope.etiTotRegistrosHE = "";
    $scope.delay = 0;
    $scope.minDuration = 0;
    $scope.message = 'Por Favor Espere...';
    $scope.backdrop = true;
    $scope.promise = null;
    $scope.templateUrl = '';

    $scope.myPromise = null;
    $scope.isSaving = undefined;
    $scope.div1 = false;
    $scope.div2 = true;

    $scope.myFile = "";
    $scope.formData = {};
    //Variable de Grid
    $scope.GridTransporte = [];
    var _GridTransporte = [];
    $scope.pagesCho = [];
    $scope.pageContentCho = [];

    $scope.file = null;
    $scope.GridTransporteP = [];
    var _GridTransporteP = [];
    $scope.pagesChoP = [];
    $scope.pageContentChoP = [];

    $scope.GridTransporteH = [];
    var _GridTransporteH = [];
    $scope.pagesChoH = [];
    $scope.pageContentChoH = [];

    $scope.gridapiTRP = {};
    $scope.gridapiTRPP = {};
    $scope.gridapiTRPH = {};

    //fin Grid

    //Variable de Busquedas
    $scope.PorNumeroCed = 1;
    $scope.txtNumero = "";
    $scope.PorDatosNombreApellido = 1;
    $scope.txtPorNombre = "";
    $scope.txtPorApellido = "";
    $scope.PorEstadosTipo = 1;
    $scope.EstadoSolicitudTipo = [];
    $scope.selectedItemTipo = "";
    //Fin Variable

    $scope.habilitares = true;
    $scope.habilitar = true;
    $scope.traCho = {};
    $scope.traCho.Tipo = "1";
    $scope.traCho.idChofer = "";
    $scope.traCho.txtNombresPrimero = "";
    $scope.traCho.txtNombresSegundo = "";
    $scope.traCho.txtApellidoPrimero = "";
    $scope.traCho.txtApellidoSegundo = "";
    $scope.traCho.txtFechaNacimiento = "";
    $scope.selectedIdentificacion = "";
    $scope.traCho.CodIdentificacion = "";
    $scope.IdentificacionDatos = [];
    $scope.traCho.txtIdentificacion = "";
    $scope.traCho.txtructranspo = "";
    $scope.traCho.txtNombreEmptran = "";
    $scope.traCho.txtDireccionDomicilio = "";
    $scope.traCho.txtTelefonoDomicilio = "";
    $scope.selectEstado = "";
    $scope.traCho.CodEstado = "";
    $scope.EstadoDatos = [];
    $scope.selectBloqueo = "";
    $scope.traCho.CodBloqueo = "";
    $scope.BloqueoDatos = [];
    $scope.traCho.txtFechaBloqueo = "";
    $scope.traCho.txtOrigenBloque = "";
    $scope.traCho.txtNumeroLicencia = "";
    $scope.selectCategoria = "";
    $scope.traCho.CodCategoria = "";
    $scope.CategoriaLDatos = [];
    $scope.selectTipoSangre = "";
    $scope.traCho.CodTipoSangre = "";
    $scope.TipoSangreLDatos = [];
    $scope.traCho.txtFechaEmision = "";
    $scope.traCho.txtFechaExpiracion = "";
    $scope.traCho.txtarchivo = "";
    $scope.traCho.transecuencial = "";
    $scope.traCho.CodProveedor = authService.authentication.CodSAP;
    $scope.traCho.usuarioCreacion = authService.authentication.userName;


    $scope.sortType = 'nombres'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';
    $scope.rutaDirectorio = "";
    //Fin Variable

    $scope.imagenurl = "";
    //Variable de Mensaje
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    //Fin Variable

    $scope.keyPress = function (eve) {
        if (eve.keyCode < 45 || eve.keyCode > 57) {

        }

    }

    $scope.printDivPage = function () {

        $('#idConfirmacionImprimir').modal('show');
    }
    $scope.printDiv = function (divName) {

        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;

        if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
            var popupWin = window.open('', '_blank', 'width=800,height=600,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no;orientation=horizontal;');
            popupWin.window.focus();
            popupWin.document.write('<!DOCTYPE html><html><head>' +
                '<link rel="stylesheet" type="text/css" href="style.css" />' +
                '</head><body onload="window.print()"><div class="reward-body">' + printContents + '</div></html>');
            popupWin.onbeforeunload = function (event) {
                popupWin.close();
                return '.\n';
            };
            popupWin.onabort = function (event) {
                popupWin.document.close();
                popupWin.close();
            }
        } else {
            var popupWin = window.open('', '_blank', 'width=800,height=600');
            popupWin.document.open();
            popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="style.css" /></head><body onload="window.print()">' + printContents + '</html>');
            popupWin.document.close();
        }
        popupWin.document.close();

        return true;
    }

    //Carga Tipo Identificacion
    $scope.okGrabar = function () {
        $scope.nuevo();
        $scope.ConsultarDespues();
    }

    TransporteProveedorService.getCatalogo('tbl_TipoDocumentos').then(function (results) {
        $scope.IdentificacionDatos = $filter('filter')(results.data, function (obj) {
            return obj.codigo == 'CD' || obj.codigo == 'PS' || obj.codigo == 'RC' || obj.codigo == 'RE';
        });


    }, function (error) {
    });

    //Carga Tipo Sangre
    TransporteProveedorService.getCatalogo('tbl_TipoSangre').then(function (results) {
        $scope.TipoSangreLDatos = results.data;
    }, function (error) {
    });

    //Carga Catalogo Categoria
    TransporteProveedorService.getCatalogo('tbl_TipoLicencia').then(function (results) {
        $scope.CategoriaLDatos = results.data;
    }, function (error) {
    });

    //Carga Catalogo Estado
    TransporteProveedorService.getCatalogo('tbl_EstadoGeneral').then(function (results) {
        $scope.EstadoDatos = results.data;
        var a = $scope.EstadoDatos;
        for (index = 0; index < a.length; ++index) {
            if (a[index].codigo === "A")
                $scope.selectEstado = a[index];
        }
    }, function (error) {
    });

    //Carga Catalogo Bloqueo
    TransporteProveedorService.getCatalogo('tbl_BloqueoProveedor').then(function (results) {
        $scope.BloqueoDatos = results.data;
    }, function (error) {
    });

    //Carga Catalogo Tipo
    TransporteProveedorService.getCatalogo('tbl_EstadoGeneral').then(function (results) {
        $scope.EstadoSolicitudTipo = results.data;
    }, function (error) {
    });

    $scope.getthefile = function (identificacion, archivo) {

        $scope.myPromise = TransporteProveedorService.getthefile(identificacion, archivo).then(function (results) {
            $scope.imagenurl = results.data;
        }, function (error) {
        });
    }

    $scope.limpioCaja = function () {
        if ($scope.PorNumeroCed === "1") {
            $scope.txtNumero = "";
        }
        if ($scope.PorDatosNombreApellido === "1") {
            $scope.txtPorApellido = "";
        }
        if ($scope.PorDatosNombreApellido === "2") {
            $scope.txtPorNombre = "";
        }
        if ($scope.PorEstadosTipo === "1") {
            $scope.selectedItemTipo = "";
        }

    }


    function validacedula(txtIdentificacion) {

        var campos = txtIdentificacion;
        if (campos.length >= 10) {
            var numero = campos;
            var suma = 0;
            var residuo = 0;
            var pri = false;
            var pub = false;
            var nat = false;
            var numeroProvincias = 24;
            var modulo = 11;

            /* Verifico que el campo no contenga letras */
            var ok = 1;

            /* Aqui almacenamos los digitos de la cedula en variables. */
            var d1 = numero.substr(0, 1);
            var d2 = numero.substr(1, 1);
            var d3 = numero.substr(2, 1);
            var d4 = numero.substr(3, 1);
            var d5 = numero.substr(4, 1);
            var d6 = numero.substr(5, 1);
            var d7 = numero.substr(6, 1);
            var d8 = numero.substr(7, 1);
            var d9 = numero.substr(8, 1);
            var d10 = numero.substr(9, 1);

            /* El tercer digito es: */
            /* 9 para sociedades privadas y extranjeros */
            /* 6 para sociedades publicas */
            /* menor que 6 (0,1,2,3,4,5) para personas naturales */

            if (d3 == 7 || d3 == 8) {
                $scope.MenjError = "El tercer dígito ingresado es inválido ";
                $('#idMensajeError').modal('show');
                return false;
            }

            /* Solo para personas naturales (modulo 10) */
            if (d3 < 6) {
                nat = true;
                var p1 = d1 * 2; if (p1 >= 10) p1 -= 9;
                var p2 = d2 * 1; if (p2 >= 10) p2 -= 9;
                var p3 = d3 * 2; if (p3 >= 10) p3 -= 9;
                var p4 = d4 * 1; if (p4 >= 10) p4 -= 9;
                var p5 = d5 * 2; if (p5 >= 10) p5 -= 9;
                var p6 = d6 * 1; if (p6 >= 10) p6 -= 9;
                var p7 = d7 * 2; if (p7 >= 10) p7 -= 9;
                var p8 = d8 * 1; if (p8 >= 10) p8 -= 9;
                var p9 = d9 * 2; if (p9 >= 10) p9 -= 9;
                modulo = 10;
            }

                /* Solo para sociedades publicas (modulo 11) */
                /* Aqui el digito verficador esta en la posicion 9, en las otras 2 en la pos. 10 */
            else if (d3 == 6) {
                pub = true;
                p1 = d1 * 3;
                p2 = d2 * 2;
                p3 = d3 * 7;
                p4 = d4 * 6;
                p5 = d5 * 5;
                p6 = d6 * 4;
                p7 = d7 * 3;
                p8 = d8 * 2;
                p9 = 0;
            }

                /* Solo para entidades privadas (modulo 11) */
            else if (d3 == 9) {
                pri = true;
                p1 = d1 * 4;
                p2 = d2 * 3;
                p3 = d3 * 2;
                p4 = d4 * 7;
                p5 = d5 * 6;
                p6 = d6 * 5;
                p7 = d7 * 4;
                p8 = d8 * 3;
                p9 = d9 * 2;
            }

            var suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
            var residuo = suma % modulo;


            /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
            if (pub == true) {
                
                /* El ruc de las empresas del sector publico terminan con 0001*/
                if (numero.substr(9, 4) != '0001') {
                    $scope.MenjError = "El ruc de la empresa del sector público debe terminar con 0001";
                    $('#idMensajeError').modal('show');


                    return false;
                }
            }
            else if (pri == true) {
                
                if (numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la empresa del sector privado debe terminar con 001";
                    $('#idMensajeError').modal('show');

                    return false;
                }
            }

            else if (nat == true) {
                
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la persona natural debe terminar con 001";
                    $('#idMensajeError').modal('show');

                    return false;
                }
            }
            return true;
        }

    }




    $scope.validorCedula = function (txtIdentificacion) {
        if (txtIdentificacion == undefined) {
            return;
        }
        validacedula(txtIdentificacion);

    }

    function Limpiar() {
        $scope.habilitar = true;
        $scope.habilitares = true;
        $scope.traCho.Tipo = "1";
        $scope.traCho.txtNombresPrimero = "";
        $scope.traCho.txtNombresSegundo = "";
        $scope.traCho.txtApellidoPrimero = "";
        $scope.traCho.txtApellidoSegundo = "";
        $scope.selectedIdentificacion = "";
        $scope.traCho.txtIdentificacion = "";
        $scope.traCho.txtructranspo = "";
        $scope.traCho.txtNombreEmptran = "";
        $scope.traCho.txtDireccionDomicilio = "";
        $scope.traCho.txtTelefonoDomicilio = "";
        $scope.selectEstado = "";
        $scope.selectBloqueo = "";
        $scope.traCho.txtFechaBloqueo = "";
        $scope.traCho.txtOrigenBloque = "";
        $scope.traCho.txtNumeroLicencia = "";
        $scope.selectCategoria = "";
        $scope.selectTipoSangre = "";
        $scope.traCho.txtFechaEmision = "";
        $scope.traCho.txtFechaExpiracion = "";
        $scope.traCho.txtFechaNacimiento = "";
        $scope.imagenurl = "";
        $scope.div1 = false;
        $scope.div2 = true;
        $scope.pageContentChoP = [];
        $scope.pageContentChoH = [];

        var a = $scope.EstadoDatos;
        for (index = 0; index < a.length; ++index) {
            if (a[index].codigo === "A")
                $scope.selectEstado = a[index];
        }
        uploader.clearQueue();
        TransporteProveedorService.getSecuenciaDirectorio("Notificacion").then(function (results) {
            if (results.data.success) {
                var secuencia = results.data.root[0];
                var direc = "TRA_" + secuencia;
                $scope.rutaDirectorio = direc;
                $scope.traCho.transecuencial = direc;
                var serviceBase = ngAuthSettings.apiServiceBaseUri;
                var rutanew = serviceBase + 'api/FileTransporte/UploadFile/?direccion=' + direc;

                
                $scope.uploader.url = rutanew;
            }
        }, function (error) {
        });
    }

    $scope.nuevo = function () {

        Limpiar();

    }

    $scope.BuscarFiltro = function () {
        $scope.Consultar();

    }
    $scope.Confirmargrabar = function () {
        if ($scope.traCho.Tipo === "1") {
            $scope.MenjConfirmacion = "¿Está seguro de guardar la información?"
        }
        if ($scope.traCho.Tipo === "2") {
            $scope.MenjConfirmacion = "¿Está seguro de modificar la información?"
        }
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.selecionaComboEstado = function () {
        if ($scope.selectEstado.codigo == "B") {
            $scope.habilitar = false;
            $scope.selectBloqueo = ""
            $scope.traCho.txtFechaBloqueo = "";
            $scope.traCho.txtOrigenBloque = "";
        } else {
            $scope.habilitar = true;
            $scope.selectBloqueo = ""
            $scope.traCho.txtFechaBloqueo = "";
            $scope.traCho.txtOrigenBloque = "";
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

    $scope.grabar = function () {
        $scope.isSaving = true;
        if ($scope.traCho.Tipo === "1") {

            if ($scope.selectEstado.codigo == "B") {
                if ($scope.selectBloqueo == "") {
                    $scope.MenjError = "Debe seleccionar motivo de bloqueo."
                    $('#idMensajeError').modal('show');
                    return;
                }

                if ($scope.traCho.txtFechaBloqueo == "") {
                    $scope.MenjError = "Debe ingresar la fecha de bloqueo."
                    $('#idMensajeError').modal('show');
                    return;
                }


                if ($scope.traCho.txtOrigenBloque == "") {
                    $scope.MenjError = "Se debe ingresar el origen de bloqueo"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            $scope.traCho.txtFechaEmision.substr(3, 2) + '/' + $scope.traCho.txtFechaEmision.substr(0, 2) + '/' + $scope.traCho.txtFechaEmision.substr(6, 4)
            var fechaex = $filter('date')($scope.traCho.txtFechaEmision.substr(3, 2) + '/' + $scope.traCho.txtFechaEmision.substr(0, 2) + '/' + $scope.traCho.txtFechaEmision.substr(6, 4), 'dd/MM/yyyy');
            var fechaex1 = $filter('date')($scope.traCho.txtFechaExpiracion.substr(3, 2) + '/' + $scope.traCho.txtFechaExpiracion.substr(0, 2) + '/' + $scope.traCho.txtFechaExpiracion.substr(6, 4), 'dd/MM/yyyy');
            if (validate_fechaMayorQue(fechaex, fechaex1)) {
                $scope.MenjError = "La fecha de emisión no debe ser mayor a la fecha de expiración"
                $('#idMensajeError').modal('show');
                return;
            }
            if ($scope.selectedIdentificacion === null)
                $scope.traCho.CodIdentificacion = "";
            else
                $scope.traCho.CodIdentificacion = $scope.selectedIdentificacion.codigo;

            if ($scope.selectEstado === null)
                $scope.traCho.CodEstado = "";
            else
                $scope.traCho.CodEstado = $scope.selectEstado.codigo;

            if ($scope.selectBloqueo === null)
                $scope.traCho.CodBloqueo = "";
            else
                $scope.traCho.CodBloqueo = $scope.selectBloqueo.codigo;

            if ($scope.selectCategoria === null)
                $scope.traCho.CodCategoria = "";
            else
                $scope.traCho.CodCategoria = $scope.selectCategoria.codigo;

            if ($scope.selectTipoSangre === null)
                $scope.traCho.CodTipoSangre = "";
            else
                $scope.traCho.CodTipoSangre = $scope.selectTipoSangre.codigo;

            if ($scope.selectedIdentificacion.codigo == 'CD' || $scope.selectedIdentificacion.codigo == 'RC') {
                if ($scope.selectedIdentificacion.codigo == 'CD') {
                    if ($scope.traCho.txtIdentificacion.length != 10) {
                        $scope.MenjError = "Identificación ingresada es incorrecta.";
                        $('#idMensajeError').modal('show');
                        return false;
                    }
                }

                if ($scope.selectedIdentificacion.codigo == 'RC') {
                    if ($scope.traCho.txtIdentificacion.length != 13) {
                        $scope.MenjError = "RUC ingresado es inválido.";
                        $('#idMensajeError').modal('show');
                        return false;
                    }
                }

                if (validacedula($scope.traCho.txtIdentificacion) == false) {
                    return;
                }
            }
            if (validacedula($scope.traCho.txtructranspo) == false) {
                return;
            }



            $scope.traCho.txtNumeroLicencia = $scope.traCho.txtIdentificacion;
            uploader.uploadAll();

            var Fecha1 = $filter('date')($scope.traCho.txtFechaNacimiento, 'dd/MM/yyyy');

            $scope.traCho.txtFechaNacimiento = Fecha1;
            $scope.myPromise = null;
            $scope.myPromise = TransporteProveedorService.getGrabaTraCho($scope.traCho).then(function (results) {
                if (results.data.success) {
                    if ($scope.traCho.Tipo === "1") {
                        $scope.MenjError = "Chofer ingresado correctamente"
                        $('#idMensajeGrabar').modal('show');
                        $scope.isSaving = false;
                    }
                }
                else {
                    if (results.data.msgError === "EXISTE")
                        if ($scope.traCho.Tipo === "1") {
                            $scope.MenjError = "El chofer ya exite verifique"
                            $('#idMensajeError').modal('show');
                            $scope.isSaving = false;
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
              alert("Error en comunicación: " + errors.join(' '));
          });
        }
        if ($scope.traCho.Tipo === "2") {

            if ($scope.selectEstado.codigo == "B") {
                if ($scope.selectBloqueo == "") {
                    $scope.MenjError = "Debe seleccionar motivo de bloqueo."
                    $('#idMensajeError').modal('show');
                    $('#ChoferesRegistrados').tab("")
                    return;
                }

                if ($scope.traCho.txtFechaBloqueo == "") {
                    $scope.MenjError = "Debe ingresar la fecha de bloqueo."
                    $('#idMensajeError').modal('show');
                    return;
                }


                if ($scope.traCho.txtOrigenBloque == "") {
                    $scope.MenjError = "Se debe ingresar el origen de bloqueo"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            $scope.traCho.txtFechaEmision.substr(3, 2) + '/' + $scope.traCho.txtFechaEmision.substr(0, 2) + '/' + $scope.traCho.txtFechaEmision.substr(6, 4)
            var fechaex = $filter('date')($scope.traCho.txtFechaEmision.substr(3, 2) + '/' + $scope.traCho.txtFechaEmision.substr(0, 2) + '/' + $scope.traCho.txtFechaEmision.substr(6, 4), 'dd/MM/yyyy');
            var fechaex1 = $filter('date')($scope.traCho.txtFechaExpiracion.substr(3, 2) + '/' + $scope.traCho.txtFechaExpiracion.substr(0, 2) + '/' + $scope.traCho.txtFechaExpiracion.substr(6, 4), 'dd/MM/yyyy');
            if (validate_fechaMayorQue(fechaex, fechaex1)) {
                $scope.MenjError = "La fecha de emisión no debe ser mayor a la fecha de expiración"
                $('#idMensajeError').modal('show');
                return;
            }
            if ($scope.selectedIdentificacion === null)
                $scope.traCho.CodIdentificacion = "";
            else
                $scope.traCho.CodIdentificacion = $scope.selectedIdentificacion.codigo;

            if ($scope.selectEstado === null)
                $scope.traCho.CodEstado = "";
            else
                $scope.traCho.CodEstado = $scope.selectEstado.codigo;

            if ($scope.selectBloqueo === null)
                $scope.traCho.CodBloqueo = "";
            else
                $scope.traCho.CodBloqueo = $scope.selectBloqueo.codigo;

            if ($scope.selectCategoria === null)
                $scope.traCho.CodCategoria = "";
            else
                $scope.traCho.CodCategoria = $scope.selectCategoria.codigo;

            if ($scope.selectTipoSangre === null)
                $scope.traCho.CodTipoSangre = "";
            else
                $scope.traCho.CodTipoSangre = $scope.selectTipoSangre.codigo;

            $scope.traCho.txtNumeroLicencia = $scope.traCho.txtIdentificacion;
            uploader.uploadAll();
            var Fecha1 = $filter('date')($scope.traCho.txtFechaNacimiento, 'dd/MM/yyyy');

            $scope.traCho.txtFechaNacimiento = Fecha1;
            $scope.myPromise = null;
            $scope.myPromise = TransporteProveedorService.getGrabaTraCho($scope.traCho).then(function (results) {
                if (results.data.success) {
                    if ($scope.traCho.Tipo === "2" && results.data.msgError === "ACTUALIZADA") {
                        $scope.MenjError = "Chofer modificado correctamente"
                        $('#idMensajeGrabar').modal('show');
                        $scope.nuevo();
                        $scope.ConsultarDespues();
                        $('.nav-tabs a[href="#ChoferesRegistrados"]').tab('show');
                        $scope.isSaving = false;
                    }
                }
                else {

                    $scope.MenjError = "No se pudo actualizar el chofer"
                    $('#idMensajeError').modal('show');
                    $scope.isSaving = false;
                }
            },
          function (error) {
              var errors = [];
              for (var key in error.data.modelState) {
                  for (var i = 0; i < error.data.modelState[key].length; i++) {
                      errors.push(error.data.modelState[key][i]);
                  }
              }
              alert("Error en comunicación: " + errors.join(' '));
          });
        }
    }



    $scope.Consultar = function () {
        if ($scope.PorNumeroCed === "2") {
            if ($scope.txtNumero === "") {
                $scope.MenjError = "Debe ingresar No. de identificación a consultar"
                $('#idMensajeError').modal('show');
                return;
            } else {
                if (isNaN($scope.txtNumero)) {
                    $scope.MenjError = "Este campo debe tener sólo números."
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
        }

        if ($scope.PorEstadosTipo === "2") {
            if ($scope.selectedItemTipo === "") {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.selectedItemTipo === null) {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }
        }
        $scope.etiTotRegistros = "";
        $scope.myPromise = null;
        $scope.myPromise = TransporteProveedorService.getConsulaGrid("1", $scope.txtNumero, $scope.txtPorNombre, $scope.txtPorApellido, $scope.selectedItemTipo.codigo, $scope.traCho.CodProveedor).then(function (results) {
            if (results.data.success) {
                $scope.GridTransporte = results.data.root[0];
                $scope.etiTotRegistros = $scope.GridTransporte.length.toString();
                $scope.showPaginate = true;

                if ($scope.GridTransporte.length == 0) {
                    $scope.MenjError = "No hay datos para su consulta."
                    $('#idMensajeInformativo').modal('show');
                    return;
                }

            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
        }, function (error) {
        });

    }




    $scope.ConsultarDespues = function () {

        if ($scope.PorNumeroCed === "2") {
            if ($scope.txtNumero === "") {
                $scope.MenjError = "Debe ingresar No. de identificación a consultar"
                $('#idMensajeError').modal('show');
                return;
            } else {
                if (isNaN($scope.txtNumero)) {
                    $scope.MenjError = "Este campo debe tener sólo números."
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
        }

        if ($scope.PorEstadosTipo === "2") {
            if ($scope.selectedItemTipo === "") {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.selectedItemTipo === null) {
                $scope.MenjError = "Debe selecionar Un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }
        }

        $scope.etiTotRegistros = "";
        $scope.myPromise = null;
        $scope.myPromise = TransporteProveedorService.getConsulaGrid("1", $scope.txtNumero, $scope.txtPorNombre, $scope.txtPorApellido, $scope.selectedItemTipo.codigo, $scope.traCho.CodProveedor).then(function (results) {
            if (results.data.success) {

                $scope.GridTransporte = results.data.root[0];
                $scope.etiTotRegistros = $scope.GridTransporte.length.toString();

            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
        }, function (error) {
        });
    }

    $scope.modi = function () {
        $scope.div1 = false;
        $scope.div2 = true;
    }
    $scope.exportar = function (tipo) {
        if ($scope.GridTransporte.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeInformativo').modal('show');
            return;
        }
        $scope.myPromise = null;
        $scope.Tablacabecera = {};
        $scope.Tablacabecera.tipo = tipo;
        $scope.Tablacabecera.usuario = $scope.traCho.usuarioCreacion;
        $scope.myPromise = TransporteProveedorService.getExportarDataChofer($scope.Tablacabecera, $scope.GridTransporte).then(function (results) {
            if (results.data != "") {
                window.open(results.data, '_blank', "");

            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
        }, function (error) {
        });

    }
    $scope.buscarProveedor = function (valor) {
        $scope.myPromise = null;
        $scope.myPromise = TransporteProveedorService.getConsulaGridUnorucced(valor, $scope.traCho.txtIdentificacion).then(function (results) {
            if (results.data.success) {
                var retorno = {};
                retorno = results.data.root[0];


                $scope.div1 = true;
                $scope.div2 = false;


                $scope.traCho.txtNombresPrimero = retorno[0].txtNombresPrimero;
                $scope.traCho.txtNombresSegundo = retorno[0].txtNombresSegundo;
                $scope.traCho.txtApellidoPrimero = retorno[0].txtApellidoPrimero;
                $scope.traCho.txtApellidoSegundo = retorno[0].txtApellidoSegundo
                var index;
                var a = $scope.IdentificacionDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codIdentificacion)
                        $scope.selectedIdentificacion = a[index];
                }
                $scope.traCho.txtIdentificacion = retorno[0].txtIdentificacion
                $scope.traCho.txtructranspo = retorno[0].txtructranspo
                $scope.traCho.txtNombreEmptran = retorno[0].txtNombreEmptran
                $scope.traCho.txtDireccionDomicilio = retorno[0].txtDireccionDomicilio
                $scope.traCho.txtTelefonoDomicilio = retorno[0].txtTelefonoDomicilio
                var a = $scope.EstadoDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codEstado)
                        $scope.selectEstado = a[index];
                }
                var a = $scope.BloqueoDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codBloqueo)
                        $scope.selectBloqueo = a[index];
                }
                if (retorno[0].txtFechaBloqueo == "01/01/1900") {
                    $scope.traCho.txtFechaBloqueo = "";
                } else {
                    $scope.traCho.txtFechaBloqueo = new Date(retorno[0].txtFechaBloqueo);
                }
                $scope.traCho.txtOrigenBloque = retorno[0].txtOrigenBloque
                $scope.traCho.txtNumeroLicencia = retorno[0].txtNumeroLicencia
                var a = $scope.CategoriaLDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codCategoria)
                        $scope.selectCategoria = a[index];
                }
                var a = $scope.TipoSangreLDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codTipoSangre)
                        $scope.selectTipoSangre = a[index];
                }
                var Fecha1 = $filter('date')(retorno[0].txtFechaEmision, 'dd/MM/yyyy');
                $scope.traCho.txtFechaEmision = Fecha1;
                Fecha1 = $filter('date')(retorno[0].txtFechaExpiracion, 'dd/MM/yyyy');
                $scope.traCho.txtFechaExpiracion = Fecha1;
                Fecha1 = $filter('date')(retorno[0].txtFechaNacimiento, 'dd/MM/yyyy');
                $scope.traCho.txtFechaNacimiento = Fecha1;
                $scope.traCho.txtarchivo = retorno[0].txtarchivo;
                $scope.getthefile(retorno[0].txtIdentificacion, retorno[0].txtarchivo);
            }
            else {
                $scope.nuevo();
                $scope.MenjError = "No hay informacion a buscar"
                $('#idMensajeError').modal('show');
            }
        }, function (error) {
        });

    }



    $scope.SelecionarGrid = function (valorrecibido, codsap) {

        Limpiar();
        $('.nav-tabs a[href="#RegistroChoferesNuevos"]').tab('show');
        $scope.myPromise = null;
        $scope.myPromise = TransporteProveedorService.getConsulaGridUno("1", valorrecibido, $scope.traCho.CodProveedor).then(function (results) {
            if (results.data.success) {

                var retorno = {};
                retorno = results.data.root[0];


                $scope.div1 = true;
                $scope.div2 = false;

                $scope.habilitares = false;

                $scope.traCho.txtNombresPrimero = retorno[0].txtNombresPrimero;
                $scope.traCho.txtNombresSegundo = retorno[0].txtNombresSegundo;
                $scope.traCho.txtApellidoPrimero = retorno[0].txtApellidoPrimero;
                $scope.traCho.txtApellidoSegundo = retorno[0].txtApellidoSegundo
                var index;
                var a = $scope.IdentificacionDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codIdentificacion)
                        $scope.selectedIdentificacion = a[index];
                }
                $scope.traCho.txtIdentificacion = retorno[0].txtIdentificacion
                $scope.traCho.txtructranspo = retorno[0].txtructranspo
                $scope.traCho.txtNombreEmptran = retorno[0].txtNombreEmptran
                $scope.traCho.txtDireccionDomicilio = retorno[0].txtDireccionDomicilio
                $scope.traCho.txtTelefonoDomicilio = retorno[0].txtTelefonoDomicilio
                var a = $scope.EstadoDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codEstado)
                        $scope.selectEstado = a[index];
                }
                var a = $scope.BloqueoDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codBloqueo)
                        $scope.selectBloqueo = a[index];
                }
                if (retorno[0].txtFechaBloqueo == "01/01/1900") {
                    $scope.traCho.txtFechaBloqueo = "";
                } else {
                    $scope.traCho.txtFechaBloqueo = new Date(retorno[0].txtFechaBloqueo);
                }
                $scope.traCho.txtOrigenBloque = retorno[0].txtOrigenBloque
                $scope.traCho.txtNumeroLicencia = retorno[0].txtNumeroLicencia
                var a = $scope.CategoriaLDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codCategoria)
                        $scope.selectCategoria = a[index];
                }
                var a = $scope.TipoSangreLDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codTipoSangre)
                        $scope.selectTipoSangre = a[index];
                }
                var Fecha1 = $filter('date')(retorno[0].txtFechaEmision, 'dd/MM/yyyy');
                $scope.traCho.txtFechaEmision = Fecha1;
                Fecha1 = $filter('date')(retorno[0].txtFechaExpiracion, 'dd/MM/yyyy');
                $scope.traCho.txtFechaExpiracion = Fecha1;
                Fecha1 = $filter('date')(retorno[0].txtFechaNacimiento, 'dd/MM/yyyy');
                $scope.traCho.txtFechaNacimiento = Fecha1;

                $scope.traCho.CodProveedor = retorno[0].codProveedor
                $scope.traCho.idChofer = valorrecibido;
                $scope.traCho.Tipo = "2";
                $scope.traCho.txtarchivo = retorno[0].txtarchivo;
                $scope.getthefile(retorno[0].txtIdentificacion, retorno[0].txtarchivo);
                $scope.pageContentChoP = [];
                $scope.pageContentChoH = [];
                $scope.pageContentChoP = results.data.root[1];
                $scope.pageContentChoH = results.data.root[2];

                $scope.etiTotRegistrosLP = "";
                $scope.etiTotRegistrosHE = "";
                $scope.etiTotRegistrosLP = $scope.pageContentChoP.length.toString();
                $scope.etiTotRegistrosHE = $scope.pageContentChoH.length.toString();

            }
        }, function (error) {
        });



    }

    $scope.loadrecordg = function () {
        $('#MantUsrAdminDialog').modal('show');

    }
    function selRowUsrsAdmin(rowId) {

        var ret = $scope.GridTransporte[rowId - 1];
        $scope.txtNombresPrimero1 = ret.tipoSolArticulo;
        $scope.txtNombresSegundo1 = ret.idSolicitud;
        $('#MantUsrAdminDialog').modal('show');
        return;
    }

    //Archivo
    uploader.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "pdf" || extension == "doc" || extension == "docx" || extension == "rtf" || extension == "jpg" || extension == "png" || extension == "xls")
                return true;
            else {
                alert('Invalid file format. Please select a file with pdf/doc/docs or rtf format  and try again.');
                return false;
            }
        }
    });

    uploader.filters.push({
        name: 'sizeFilter',
        fn: function (item, options) {
            var fileSize = item.size;
            fileSize = parseInt(fileSize) / (1024 * 1024);
            if (fileSize <= 5)
                return true;
            else {
                alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
                return false;
            }
        }
    });

    uploader.filters.push({
        name: 'itemResetFilter',
        fn: function (item, options) {
            if (this.queue.length < 5)
                return true;
            else {
                alert('You have exceeded the limit of uploading files.');
                return false;
            }
        }
    });

    // CALLBACKS

    uploader.onWhenAddingFileFailed = function (item, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };
    uploader.onAfterAddingFile = function (fileItem) {
        $scope.traCho.txtarchivo = fileItem.file.name;
    };

    uploader.onSuccessItem = function (fileItem, response, status, headers) {
    };
    uploader.onErrorItem = function (fileItem, response, status, headers) {
        alert('We were unable to upload your file. Please try again.');
    };
    uploader.onCancelItem = function (fileItem, response, status, headers) {
        alert('File uploading has been cancelled.');
    };

    uploader.onAfterAddingAll = function (addedFileItems) {
        console.info('onAfterAddingAll', addedFileItems);
    };
    uploader.onBeforeUploadItem = function (item) {
        console.info('onBeforeUploadItem', item);
    };
    uploader.onProgressItem = function (fileItem, progress) {
        console.info('onProgressItem', fileItem, progress);
    };
    uploader.onProgressAll = function (progress) {
        console.info('onProgressAll', progress);
    };

    uploader.onCompleteItem = function (fileItem, response, status, headers) {
        console.info('onCompleteItem', fileItem, response, status, headers);
    };
    uploader.onCompleteAll = function () {
        console.info('onCompleteAll');
    };


    //Fin Archivo
    $scope.nuevo();

    $scope.Consultar();

}
]);
//Vehiculos proveedor controller
app.controller('VehiculosProveedorController', ['$scope', '$location', 'VehiculosProveedorService', '$cookies', '$filter', 'ngAuthSettings', 'FileUploader', 'authService', function ($scope, $location, VehiculosProveedorService, $cookies, $filter, ngAuthSettings, FileUploader, authService) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var Ruta = serviceBase + 'api/FileTransporte/UploadFile/?direccion=prueba';
    var uploader = $scope.uploader = new FileUploader({
        url: Ruta
    });

    $scope.etiTotRegistros = "";
    $scope.etiTotRegistrosLP = "";
    $scope.etiTotRegistrosHE = "";
    $scope.formData = {};
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.div1 = false;
    $scope.div2 = true;
    $scope.imagenurl = "";
    //Variable de Grid
    $scope.GridTransporte = [];
    var _GridTransporte = [];
    $scope.pagesCho = [];
    $scope.pageContentCho = [];


    $scope.GridTransporteP = [];
    var _GridTransporteP = [];
    $scope.pagesChoP = [];
    $scope.pageContentChoP = [];

    $scope.GridTransporteH = [];
    var _GridTransporteH = [];
    $scope.pagesChoH = [];
    $scope.pageContentChoH = [];

    $scope.gridapiTRP = {};
    $scope.gridapiTRPP = {};
    $scope.gridapiTRPH = {};
    //fin Grid

    //Variable de Busquedas
    $scope.PorTipos = 1;
    $scope.PorPlaca = 1;
    $scope.PorEstadosTipo = 1;
    $scope.PorPropietario = 1;
    $scope.txtPropietario = "";
    $scope.txtPlaca = "";


    $scope.SolicitudTipo = [];
    $scope.selectedItemTipo = "";
    $scope.selectedItemEstado = "";
    $scope.EstadoSolicitud = [];
    //Fin Variable


    //Combos Formularios
    $scope.habilitares = true;
    $scope.habilitar = true;
    $scope.IdentificacionDatos = [];
    $scope.selectedIdentificacion = "";
    $scope.TipoVehiculosdatos = [];
    $scope.selecttipovehiculo = "";
    $scope.ColorPrincipalDatos = [];
    $scope.selectcolorprincipal = "";
    $scope.ColorSecundarioDatos = [];
    $scope.selectcolorsecundario = "";
    $scope.MarcaDatos = [];
    $scope.selectmarca = "";
    $scope.ModeloDatos = [];
    $scope.ModeloDatostmp = [];
    $scope.selectmodelo = "";
    $scope.PaisDatos = [];
    $scope.selectpais = "";
    $scope.EstadoDatos = [];
    $scope.selectEstado = "";
    $scope.BloqueoDatos = [];
    $scope.selectBloqueo = "";
    //Fin Combos

    $scope.traCho = {};
    $scope.traCho.Tipo = "1";
    $scope.traCho.idvehiculo = "";
    $scope.traCho.CodProveedor = authService.authentication.CodSAP;
    $scope.traCho.usuarioCreacion = authService.authentication.userName;
    $scope.traCho.codIdentificacion = "";
    $scope.traCho.txtIdentificacion = "";
    $scope.traCho.txtNombresPrimero = "";
    $scope.traCho.txtNombresSegundo = "";
    $scope.traCho.txtApellidoPrimero = "";
    $scope.traCho.txtApellidoSegundo = "";
    $scope.traCho.txtdirpropie = "";
    $scope.traCho.txttelfpro = "";
    $scope.traCho.codTipoVehiculo = "";
    $scope.traCho.codColorPrincipal = "";
    $scope.traCho.codColorSecundario = "";
    $scope.traCho.txtmotor = "";
    $scope.traCho.codMarca = "";
    $scope.traCho.codModelo = "";
    $scope.traCho.txtplaca = "";
    $scope.traCho.txtchasis = "";
    $scope.traCho.codEstado = "";
    $scope.traCho.codBloqueo = "";
    $scope.traCho.txtFechaBloqueo = "";
    $scope.traCho.txtOrigenBloque = "";
    $scope.traCho.txtmatricula = "";
    $scope.traCho.txtfechamatricula = "";
    $scope.traCho.txtexpiracionmatricula = "";
    $scope.traCho.txtaniomatricula = "";
    $scope.traCho.codPais = "";
    $scope.traCho.txtcilindraje = "";
    $scope.traCho.txttonelaje = "";
    $scope.traCho.PorSota = "";
    $scope.traCho.txtemisionsoat = "";
    $scope.traCho.txtexpiracionsoat = "";
    $scope.traCho.txtarchivo = "";
    $scope.traCho.transecuencial = "";


    $scope.sortType = 'numplaca'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';

    //Fin Variable

    //Variable de Mensaje
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    //Fin Variable

    $scope.okGrabar = function () {
        $scope.nuevo();
        $scope.ConsultarDespues();
    }

    //Carga Catalogo Estado
    VehiculosProveedorService.getCatalogo('tbl_EstadoGeneral').then(function (results) {
        $scope.EstadoSolicitud = results.data;
    }, function (error) {
    });

    //Carga Catalogo Tipo Vehiculo
    VehiculosProveedorService.getCatalogo('tbl_TipoVehiculo').then(function (results) {
        $scope.SolicitudTipo = results.data;
    }, function (error) {
    });



    //Carga Tipo Identificacion
    VehiculosProveedorService.getCatalogo('tbl_TipoDocumentos').then(function (results) {

        $scope.IdentificacionDatos = $filter('filter')(results.data, function (obj) {
            return obj.codigo == 'CD' || obj.codigo == 'PS' || obj.codigo == 'RC' || obj.codigo == 'RE';
        });

    }, function (error) {
    });

    //Carga Tipo Pais
    VehiculosProveedorService.getCatalogo('tbl_Pais').then(function (results) {
        $scope.PaisDatos = results.data;
    }, function (error) {
    });

    //Carga Catalogo Categoria
    VehiculosProveedorService.getCatalogo('tbl_TipoVehiculo').then(function (results) {
        $scope.TipoVehiculosdatos = results.data;
    }, function (error) {
    });

    //Carga Catalogo Estado
    VehiculosProveedorService.getCatalogo('tbl_EstadoGeneral').then(function (results) {
        $scope.EstadoDatos = results.data;
        var a = $scope.EstadoDatos;
        for (index = 0; index < a.length; ++index) {
            if (a[index].codigo === "A")
                $scope.selectEstado = a[index];
        }
    }, function (error) {
    });

    //Carga Catalogo Bloqueo
    VehiculosProveedorService.getCatalogo('tbl_BloqueoProveedor').then(function (results) {
        $scope.BloqueoDatos = results.data;
    }, function (error) {
    });

    //Carga Catalogo Color Primario
    VehiculosProveedorService.getCatalogo('tbl_Color').then(function (results) {
        $scope.ColorPrincipalDatos = results.data;
    }, function (error) {
    });

    //Carga Catalogo Color Secundario
    VehiculosProveedorService.getCatalogo('tbl_Color').then(function (results) {
        $scope.ColorSecundarioDatos = results.data;
    }, function (error) {
    });

    //Carga Catalogo Marca Vehiculo
    VehiculosProveedorService.getCatalogo('tbl_MarcaVehiculo').then(function (results) {
        $scope.MarcaDatos = results.data;
    }, function (error) {
    });

    VehiculosProveedorService.getCatalogo('tbl_ModeloVehiculo').then(function (results) {
        $scope.ModeloDatostmp = results.data;
    }, function (error) {
    });

    $scope.$watch('selectmarca', function () {
        $scope.ModeloDatos = [];
        if ($scope.selectmarca != '' && angular.isUndefined($scope.selectmarca) != true) {
            $scope.ModeloDatos = $filter('filter')($scope.ModeloDatostmp,
                                              { descAlterno: $scope.selectmarca.codigo });
        }
    });
    $scope.getthefile = function (identificacion, archivo) {
        $scope.myPromise = null;
        $scope.myPromise = VehiculosProveedorService.getthefile(identificacion, archivo).then(function (results) {
            $scope.imagenurl = results.data;
        }, function (error) {
        });
    }
    $scope.limpioCaja = function () {
        if ($scope.PorTipos === "1") {
            $scope.selectedItemTipo = "";
        }
        if ($scope.PorEstadosTipo === "1") {
            $scope.selectedItemEstado = "";
        }
        if ($scope.PorPropietario === "1") {
            $scope.txtPropietario = "";
        }
        if ($scope.PorPlaca === "1") {
            $scope.txtPlaca = "";
        }
    }

    function validacedula(txtIdentificacion) {
        var campos = txtIdentificacion;
        if (campos.length >= 10) {
            numero = campos;
            var suma = 0;
            var residuo = 0;
            var pri = false;
            var pub = false;
            var nat = false;
            var numeroProvincias = 24;
            var modulo = 11;

            /* Verifico que el campo no contenga letras */
            var ok = 1;

            /* Aqui almacenamos los digitos de la cedula en variables. */
            d1 = numero.substr(0, 1);
            d2 = numero.substr(1, 1);
            d3 = numero.substr(2, 1);
            d4 = numero.substr(3, 1);
            d5 = numero.substr(4, 1);
            d6 = numero.substr(5, 1);
            d7 = numero.substr(6, 1);
            d8 = numero.substr(7, 1);
            d9 = numero.substr(8, 1);
            d10 = numero.substr(9, 1);

            /* El tercer digito es: */
            /* 9 para sociedades privadas y extranjeros */
            /* 6 para sociedades publicas */
            /* menor que 6 (0,1,2,3,4,5) para personas naturales */

            if (d3 == 7 || d3 == 8) {
                $scope.MenjError = "El tercer dígito ingresado es inválido"
                $('#idMensajeError').modal('show');
                return false;
            }

            /* Solo para personas naturales (modulo 10) */
            if (d3 < 6) {
                nat = true;
                p1 = d1 * 2; if (p1 >= 10) p1 -= 9;
                p2 = d2 * 1; if (p2 >= 10) p2 -= 9;
                p3 = d3 * 2; if (p3 >= 10) p3 -= 9;
                p4 = d4 * 1; if (p4 >= 10) p4 -= 9;
                p5 = d5 * 2; if (p5 >= 10) p5 -= 9;
                p6 = d6 * 1; if (p6 >= 10) p6 -= 9;
                p7 = d7 * 2; if (p7 >= 10) p7 -= 9;
                p8 = d8 * 1; if (p8 >= 10) p8 -= 9;
                p9 = d9 * 2; if (p9 >= 10) p9 -= 9;
                modulo = 10;
            }

                /* Solo para sociedades publicas (modulo 11) */
                /* Aqui el digito verficador esta en la posicion 9, en las otras 2 en la pos. 10 */
            else if (d3 == 6) {
                pub = true;
                p1 = d1 * 3;
                p2 = d2 * 2;
                p3 = d3 * 7;
                p4 = d4 * 6;
                p5 = d5 * 5;
                p6 = d6 * 4;
                p7 = d7 * 3;
                p8 = d8 * 2;
                p9 = 0;
            }

                /* Solo para entidades privadas (modulo 11) */
            else if (d3 == 9) {
                pri = true;
                p1 = d1 * 4;
                p2 = d2 * 3;
                p3 = d3 * 2;
                p4 = d4 * 7;
                p5 = d5 * 6;
                p6 = d6 * 5;
                p7 = d7 * 4;
                p8 = d8 * 3;
                p9 = d9 * 2;
            }

            suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
            residuo = suma % modulo;

            
            /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
            if (pub == true) {
                
                /* El ruc de las empresas del sector publico terminan con 0001*/
                if (numero.substr(9, 4) != '0001') {
                    $scope.MenjError = "El ruc de la empresa del sector público debe terminar con 0001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }
            else if (pri == true) {
                
                if (numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la empresa del sector privado debe terminar con 001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }

            else if (nat == true) {
                
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la persona natural debe terminar con 001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }
            return true;
        }

    }
    $scope.validorCedula = function (txtIdentificacion) {
        if (txtIdentificacion == undefined) {
            return;
        }
        validacedula(txtIdentificacion);

    }



    function Limpiar() {
        $scope.habilitar = true;
        $scope.habilitares = true;
        $scope.traCho.Tipo = "1";
        $scope.traCho.idvehiculo = "";
        $scope.traCho.codIdentificacion = "";
        $scope.traCho.txtIdentificacion = "";
        $scope.traCho.txtNombresPrimero = "";
        $scope.traCho.txtNombresSegundo = "";
        $scope.traCho.txtApellidoPrimero = "";
        $scope.traCho.txtApellidoSegundo = "";
        $scope.traCho.txtdirpropie = "";
        $scope.traCho.txttelfpro = "";
        $scope.traCho.codTipoVehiculo = "";
        $scope.traCho.codColorPrincipal = "";
        $scope.traCho.codColorSecundario = "";
        $scope.traCho.txtmotor = "";
        $scope.traCho.codMarca = "";
        $scope.traCho.codModelo = "";
        $scope.traCho.txtplaca = "";
        $scope.traCho.txtchasis = "";
        $scope.traCho.codEstado = "";
        $scope.traCho.codBloqueo = "";
        $scope.traCho.txtFechaBloqueo = "";
        $scope.traCho.txtOrigenBloque = "";
        $scope.traCho.txtmatricula = "";
        $scope.traCho.txtfechamatricula = "";
        $scope.traCho.txtexpiracionmatricula = "";
        $scope.traCho.txtaniomatricula = "";
        $scope.traCho.codPais = "";
        $scope.traCho.txtcilindraje = "";
        $scope.traCho.txttonelaje = "";
        $scope.traCho.PorSota = "";
        $scope.traCho.txtemisionsoat = "";
        $scope.traCho.txtexpiracionsoat = "";


        $scope.selectedIdentificacion = "";
        $scope.selecttipovehiculo = "";
        $scope.selectcolorprincipal = "";
        $scope.selectcolorsecundario = "";
        $scope.selectmarca = "";
        $scope.selectmodelo = "";
        $scope.selectpais = "";
        $scope.selectEstado = "";
        $scope.selectBloqueo = "";
        $scope.div1 = false;
        $scope.div2 = true;
        $scope.pageContentChoP = [];
        $scope.pageContentChoH = [];
        $('#file-3').val('');
        uploader.clearQueue();
        var a = $scope.EstadoDatos;
        for (index = 0; index < a.length; ++index) {
            if (a[index].codigo === "A")
                $scope.selectEstado = a[index];
        }
        VehiculosProveedorService.getSecuenciaDirectorio("Notificacion").then(function (results) {
            if (results.data.success) {
                var secuencia = results.data.root[0];
                var direc = "VEH_" + secuencia;
                $scope.rutaDirectorio = direc;
                $scope.traCho.transecuencial = direc;
                var serviceBase = ngAuthSettings.apiServiceBaseUri;
                var rutanew = serviceBase + 'api/FileTransporte/UploadFile/?direccion=' + direc;
                $scope.uploader.url = rutanew;
            }
        }, function (error) {
        });
        $('#idselectIdentificacion').focus();
    }

    $scope.nuevo = function () {

        Limpiar();
    }

    $scope.BuscarFiltro = function () {
        $scope.Consultar();
    }

    $scope.Confirmargrabar = function () {
        if ($scope.traCho.Tipo === "1") {
            $scope.MenjConfirmacion = "¿Está seguro de guardar la información?"
        }
        if ($scope.traCho.Tipo === "2") {
            $scope.MenjConfirmacion = "¿Está seguro de modificar la información?"
        }
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.selecionaComboEstado = function () {
        if ($scope.selectEstado.codigo == "B") {
            $scope.habilitar = false;
            $scope.selectBloqueo = ""
            $scope.traCho.txtFechaBloqueo = "";
            $scope.traCho.txtOrigenBloque = "";
        } else {
            $scope.habilitar = true;
            $scope.selectBloqueo = ""
            $scope.traCho.txtFechaBloqueo = "";
            $scope.traCho.txtOrigenBloque = "";
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
    $scope.grabar = function () {
        if ($scope.traCho.Tipo === "1") {

            if ($scope.selectEstado.codigo == "B") {
                if ($scope.selectBloqueo == "") {
                    $scope.MenjError = "Debe seleccionar motivo de bloqueo."
                    $('#idMensajeError').modal('show');
                    return;
                }

                if ($scope.traCho.txtFechaBloqueo == "") {
                    $scope.MenjError = "Debe ingresar la fecha de bloqueo."
                    $('#idMensajeError').modal('show');
                    return;
                }


                if ($scope.traCho.txtOrigenBloque == "") {
                    $scope.MenjError = "Se debe ingresar el origen de bloqueo"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }

            if (validate_fechaMayorQue($scope.traCho.txtemisionsoat, $scope.traCho.txtexpiracionsoat)) {
                $scope.MenjError = "La fecha de emisión soat no debe ser mayor a la fecha de expiración soat"
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.selectedIdentificacion === null)
                $scope.traCho.codIdentificacion = "";
            else
                $scope.traCho.codIdentificacion = $scope.selectedIdentificacion.codigo;

            if ($scope.selecttipovehiculo === null)
                $scope.traCho.codTipoVehiculo = "";
            else
                $scope.traCho.codTipoVehiculo = $scope.selecttipovehiculo.codigo;

            if ($scope.selectcolorprincipal === null)
                $scope.traCho.codColorPrincipal = "";
            else
                $scope.traCho.codColorPrincipal = $scope.selectcolorprincipal.codigo;

            if ($scope.selectcolorsecundario === null)
                $scope.traCho.codColorSecundario = "";
            else
                $scope.traCho.codColorSecundario = $scope.selectcolorsecundario.codigo;

            if ($scope.selectmarca === null)
                $scope.traCho.codMarca = "";
            else
                $scope.traCho.codMarca = $scope.selectmarca.codigo;

            if ($scope.selectmodelo === null)
                $scope.traCho.codModelo = "";
            else
                $scope.traCho.codModelo = $scope.selectmodelo.codigo;

            if ($scope.selectpais === null)
                $scope.traCho.codPais = "";
            else
                $scope.traCho.codPais = $scope.selectpais.codigo;

            if ($scope.selectEstado === null)
                $scope.traCho.codEstado = "";
            else
                $scope.traCho.codEstado = $scope.selectEstado.codigo;

            if ($scope.selectBloqueo === null)
                $scope.traCho.codBloqueo = "";
            else
                $scope.traCho.codBloqueo = $scope.selectBloqueo.codigo;

            if ($scope.selectedIdentificacion.codigo == 'CD' || $scope.selectedIdentificacion.codigo == 'RC') {
                if ($scope.selectedIdentificacion.codigo == 'CD') {
                    if ($scope.traCho.txtIdentificacion.length != 10) {
                        $scope.MenjError = "Identificación ingresada es incorrecta.";
                        $('#idMensajeError').modal('show');
                        return false;
                    }
                }

                if ($scope.selectedIdentificacion.codigo == 'RC') {
                    if ($scope.traCho.txtIdentificacion.length != 13) {
                        $scope.MenjError = "RUC ingresado es inválido.";
                        $('#idMensajeError').modal('show');
                        return false;
                    }
                }
                if (validacedula($scope.traCho.txtIdentificacion) == false) {
                    return;
                }
            }
            if ($scope.traCho.txtfechamatricula == null || $scope.traChotxtfechamatricula == "") {
                $scope.showMessage('I', 'Seleccione la fecha de matricula.');
                return;
            }
            if ($scope.traCho.txtexpiracionmatricula == null || $scope.traCho.txtexpiracionmatricula == "") {
                $scope.showMessage('I', 'Seleccione la fecha de matricula.');
                return;
            }
            uploader.uploadAll();

            var Fecha1 = $filter('date')($scope.traCho.txtFechaBloqueo, 'dd/MM/yyyy');
            $scope.traCho.txtFechaBloqueo = Fecha1;
            Fecha1 = $filter('date')($scope.traCho.txtfechamatricula, 'dd/MM/yyyy');
            $scope.traCho.txtfechamatricula = Fecha1;
            Fecha1 = $filter('date')($scope.traCho.txtexpiracionmatricula, 'dd/MM/yyyy');
            $scope.traCho.txtexpiracionmatricula = Fecha1;
            Fecha1 = $filter('date')($scope.traCho.txtemisionsoat, 'dd/MM/yyyy');
            $scope.traCho.txtemisionsoat = Fecha1;
            Fecha1 = $filter('date')($scope.traCho.txtexpiracionsoat, 'dd/MM/yyyy');
            $scope.traCho.txtexpiracionsoat = Fecha1;

            $scope.myPromise = null;
            $scope.myPromise = VehiculosProveedorService.getGrabaTraCho($scope.traCho).then(function (results) {
                if (results.data.success) {
                    if ($scope.traCho.Tipo === "1") {
                        $scope.MenjError = "Se ingreso el vehículo correctamente"
                        $('#idMensajeGrabar').modal('show');
                    }
                }
                else {
                    if (results.data.msgError === "EXISTE")
                        if ($scope.traCho.Tipo === "1") {
                            $scope.MenjError = "El vehículo ya exite verifique"
                            $('#idMensajeError').modal('show');
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
              alert("Error en comunicación: " + errors.join(' '));
          });

        }

        if ($scope.traCho.Tipo === "2") {

            if ($scope.selectEstado.codigo == "B") {
                if ($scope.selectBloqueo == "") {
                    $scope.MenjError = "Debe seleccionar motivo de bloqueo."
                    $('#idMensajeError').modal('show');
                    return;
                }

                if ($scope.traCho.txtFechaBloqueo == "") {
                    $scope.MenjError = "Debe ingresar la fecha de bloqueo."
                    $('#idMensajeError').modal('show');
                    return;
                }


                if ($scope.traCho.txtOrigenBloque == "") {
                    $scope.MenjError = "Se debe ingresar el origen de bloqueo"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            if ($scope.selectedIdentificacion === null)
                $scope.traCho.codIdentificacion = "";
            else
                $scope.traCho.codIdentificacion = $scope.selectedIdentificacion.codigo;

            if ($scope.selecttipovehiculo === null)
                $scope.traCho.codTipoVehiculo = "";
            else
                $scope.traCho.codTipoVehiculo = $scope.selecttipovehiculo.codigo;

            if ($scope.selectcolorprincipal === null)
                $scope.traCho.codColorPrincipal = "";
            else
                $scope.traCho.codColorPrincipal = $scope.selectcolorprincipal.codigo;

            if ($scope.selectcolorsecundario === null)
                $scope.traCho.codColorSecundario = "";
            else
                $scope.traCho.codColorSecundario = $scope.selectcolorsecundario.codigo;

            if ($scope.selectmarca === null)
                $scope.traCho.codMarca = "";
            else
                $scope.traCho.codMarca = $scope.selectmarca.codigo;

            if ($scope.selectmodelo === null)
                $scope.traCho.codModelo = "";
            else
                $scope.traCho.codModelo = $scope.selectmodelo.codigo;

            if ($scope.selectpais === null)
                $scope.traCho.codPais = "";
            else
                $scope.traCho.codPais = $scope.selectpais.codigo;

            if ($scope.selectEstado === null)
                $scope.traCho.codEstado = "";
            else
                $scope.traCho.codEstado = $scope.selectEstado.codigo;

            if ($scope.selectBloqueo === null)
                $scope.traCho.codBloqueo = "";
            else
                $scope.traCho.codBloqueo = $scope.selectBloqueo.codigo;

            uploader.uploadAll();
            var Fecha1 = $filter('date')($scope.traCho.txtFechaBloqueo, 'dd/MM/yyyy');
            $scope.traCho.txtFechaBloqueo = Fecha1;
            Fecha1 = $filter('date')($scope.traCho.txtfechamatricula, 'dd/MM/yyyy');
            $scope.traCho.txtfechamatricula = Fecha1;
            Fecha1 = $filter('date')($scope.traCho.txtexpiracionmatricula, 'dd/MM/yyyy');
            $scope.traCho.txtexpiracionmatricula = Fecha1;
            Fecha1 = $filter('date')($scope.traCho.txtemisionsoat, 'dd/MM/yyyy');
            $scope.traCho.txtemisionsoat = Fecha1;
            Fecha1 = $filter('date')($scope.traCho.txtexpiracionsoat, 'dd/MM/yyyy');
            $scope.traCho.txtexpiracionsoat = Fecha1;
            $scope.myPromise = null;
            $scope.myPromise = VehiculosProveedorService.getGrabaTraCho($scope.traCho).then(function (results) {
                if (results.data.success) {
                    if ($scope.traCho.Tipo === "2" && results.data.msgError === "ACTUALIZADA") {
                        $scope.MenjError = "Se actualizo el vehículo correctamente"
                        $('#idMensajeGrabar').modal('show');
                        $scope.nuevo();
                        $scope.Consultar();
                        $('.nav-tabs a[href="#VehiculosRegistrados"]').tab('show');
                    }
                }
                else {
                    $scope.MenjError = "No se pudo actualizar el vehículo";
                    $('#idMensajeError').modal('show');

                }
            },
          function (error) {
              var errors = [];
              for (var key in error.data.modelState) {
                  for (var i = 0; i < error.data.modelState[key].length; i++) {
                      errors.push(error.data.modelState[key][i]);
                  }
              }
              alert("Error en comunicación: " + errors.join(' '));
          });
        }
    }




    $scope.Consultar = function () {

        if ($scope.PorTipos === "2") {
            if ($scope.selectedItemTipo === "") {
                $scope.MenjError = "Debe selecionar un tipo de vehículo a consultar"
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.selectedItemTipo === null) {
                $scope.MenjError = "Debe selecionar un tipo de vehículo a consultar"
                $('#idMensajeError').modal('show');
                return;
            }

        }
        if ($scope.PorPlaca === "2") {
            if ($scope.txtPlaca === "") {
                $scope.MenjError = "Debe ingresar una placa  a consultar"
                $('#idMensajeError').modal('show');
                return;
            }
        }

        if ($scope.PorPropietario === "2") {
            if ($scope.txtPropietario === "") {
                $scope.MenjError = "Debe ingresar Un propietario  a consultar"
                $('#idMensajeError').modal('show');
                return;
            }
        }

        if ($scope.PorEstadosTipo === "2") {
            if ($scope.selectedItemEstado === "") {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.selectedItemEstado === null) {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }

        }
        $scope.etiTotRegistros = "";
        $scope.myPromise = null;
        $scope.myPromise = VehiculosProveedorService.getConsulaGrid("1", $scope.selectedItemTipo.codigo, $scope.txtPropietario, $scope.txtPlaca, $scope.selectedItemEstado.codigo, $scope.traCho.CodProveedor).then(function (results) {
            if (results.data.success) {
                $scope.GridTransporte = results.data.root[0];
                $scope.etiTotRegistros = $scope.GridTransporte.length.toString();
                if ($scope.GridTransporte.length == 0) {
                    $scope.MenjError = "No hay datos para su consulta."
                    $('#idMensajeInformativo').modal('show');
                    return;
                }
            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTraTN').focus(); }, 150);


        }, function (error) {
        });
    }

    $scope.ConsultarDespues = function () {

        if ($scope.PorNumeroCed === "2") {
            if ($scope.txtNumero === "") {
                $scope.MenjError = "Debe ingresar cedula o ruc a consultar"
                $('#idMensajeError').modal('show');
                return;
            } else {
                if (isNaN($scope.txtNumero)) {
                    $scope.MenjError = "Este campo debe tener sólo números."
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
        }

        if ($scope.PorEstadosTipo === "2") {
            if ($scope.selectedItemTipo === "") {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.selectedItemTipo === null) {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }
        }

        $scope.etiTotRegistros = "";
        $scope.myPromise = null;
        $scope.myPromise = VehiculosProveedorService.getConsulaGrid("1", $scope.txtNumero, $scope.txtPorNombre, $scope.txtPorApellido, $scope.selectedItemTipo.codigo, $scope.traCho.CodProveedor).then(function (results) {
            if (results.data.success) {
                $scope.pageContentCho = results.data.root[0];
                $scope.etiTotRegistros = $scope.pageContentCho.length.toString();
            }


        }, function (error) {
        });
    }
    $scope.modi = function () {
        $scope.div1 = false;
        $scope.div2 = true;
    }
    $scope.buscarProveedor = function (valor) {
        $scope.myPromise = null;
        $scope.myPromise = VehiculosProveedorService.getConsulaGridUnorucced(valor, $scope.traCho.txtplaca).then(function (results) {
            if (results.data.success) {
                var retorno = {};
                retorno = results.data.root[0];
                $scope.div1 = true;
                $scope.div2 = false;
                $scope.traCho.txtNombresPrimero = retorno[0].txtNombresPrimero;
                $scope.traCho.txtNombresSegundo = retorno[0].txtNombresSegundo;
                $scope.traCho.txtApellidoPrimero = retorno[0].txtApellidoPrimero;
                $scope.traCho.txtApellidoSegundo = retorno[0].txtApellidoSegundo
                var index;
                var a = $scope.IdentificacionDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codIdentificacion)
                        $scope.selectedIdentificacion = a[index];
                }
                $scope.traCho.txtIdentificacion = retorno[0].txtIdentificacion;
                $scope.traCho.txtdirpropie = retorno[0].txtdirpropie;
                $scope.traCho.txttelfpro = retorno[0].txttelfpro;

                var a = $scope.TipoVehiculosdatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codTipoVehiculo)
                        $scope.selecttipovehiculo = a[index];
                }
                $scope.traCho.txtDireccionDomicilio = retorno[0].txtDireccionDomicilio;
                $scope.traCho.txtTelefonoDomicilio = retorno[0].txtTelefonoDomicilio;
                var a = $scope.ColorPrincipalDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codColorPrincipal)
                        $scope.selectcolorprincipal = a[index];
                }
                var a = $scope.ColorSecundarioDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codColorSecundario)
                        $scope.selectcolorsecundario = a[index];
                }
                $scope.traCho.txtmotor = retorno[0].txtmotor
                var a = $scope.MarcaDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codMarca)
                        $scope.selectmarca = a[index];
                }

                $scope.ModeloDatos = [];
                if ($scope.selectmarca != '' && angular.isUndefined($scope.selectmarca) != true) {
                    $scope.ModeloDatos = $filter('filter')($scope.ModeloDatostmp,
                                                      { descAlterno: $scope.selectmarca.codigo });
                }

                var a = $scope.ModeloDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codModelo)
                        $scope.selectmodelo = a[index];
                }
                $scope.traCho.txtplaca = retorno[0].txtplaca;
                $scope.traCho.txtchasis = retorno[0].txtchasis;
                var a = $scope.EstadoDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codEstado)
                        $scope.selectEstado = a[index];
                }
                var a = $scope.BloqueoDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codBloqueo)
                        $scope.selectBloqueo = a[index];
                }
                if (retorno[0].txtFechaBloqueo == "01/01/1900") {
                    $scope.traCho.txtFechaBloqueo = "";
                } else {
                    var Fecha1 = $filter('date')(retorno[0].txtFechaBloqueo, 'dd/MM/yyyy');
                    $scope.traCho.txtFechaBloqueo = Fecha1;
                }
                $scope.traCho.txtOrigenBloque = retorno[0].txtOrigenBloque;
                $scope.traCho.txtmatricula = retorno[0].txtmatricula;

                var Fecha2 = $filter('date')(retorno[0].txtfechamatricula, 'dd/MM/yyyy');
                $scope.traCho.txtfechamatricula = Fecha2;
                Fecha2 = $filter('date')(retorno[0].txtexpiracionmatricula, 'dd/MM/yyyy');
                $scope.traCho.txtexpiracionmatricula = Fecha2;

                $scope.traCho.txtaniomatricula = retorno[0].txtaniomatricula
                var a = $scope.PaisDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codPais)
                        $scope.selectpais = a[index];
                }
                $scope.traCho.txtcilindraje = retorno[0].txtcilindraje;
                $scope.traCho.txttonelaje = retorno[0].txttonelaje;

                Fecha2 = $filter('date')(retorno[0].txtemisionsoat, 'dd/MM/yyyy');
                $scope.traCho.txtemisionsoat = Fecha2;
                Fecha2 = $filter('date')(retorno[0].txtexpiracionsoat, 'dd/MM/yyyy');
                $scope.traCho.txtexpiracionsoat = Fecha2;



                $scope.traCho.PorSota = retorno[0].PorSota;
                $scope.traCho.Tipo = "1";
                $scope.traCho.txtarchivo = retorno[0].txtarchivo;
            }
            else {
                $scope.nuevo();
                $scope.MenjError = "No hay informacion a buscar"
                $('#idMensajeError').modal('show');
            }
        }, function (error) {
        });

    }
    $scope.exportar = function (tipo) {
        if ($scope.GridTransporte.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeInformativo').modal('show');
            return;
        }
        $scope.myPromise = null;
        $scope.Tablacabecera = {};
        $scope.Tablacabecera.tipo = tipo;
        $scope.Tablacabecera.usuario = $scope.traCho.usuarioCreacion;
        $scope.myPromise = VehiculosProveedorService.getExportarDataVehiculo($scope.Tablacabecera, $scope.GridTransporte).then(function (results) {
            if (results.data != "") {
                window.open(results.data, '_blank', "");
            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
        }, function (error) {
        });

    }

    $scope.SelecionarGrid = function (valorrecibido) {
        Limpiar();
        $('.nav-tabs a[href="#RegistroVehiculosNuevos"]').tab('show');
        $scope.myPromise = null;
        $scope.myPromise = VehiculosProveedorService.getConsulaGridUno("1", valorrecibido, $scope.traCho.CodProveedor).then(function (results) {
            if (results.data.success) {
                var retorno = {};
                retorno = results.data.root[0];
                $scope.div1 = true;
                $scope.div2 = false;

                $scope.habilitares = false;
                $scope.traCho.txtNombresPrimero = retorno[0].txtNombresPrimero;
                $scope.traCho.txtNombresSegundo = retorno[0].txtNombresSegundo;
                $scope.traCho.txtApellidoPrimero = retorno[0].txtApellidoPrimero;
                $scope.traCho.txtApellidoSegundo = retorno[0].txtApellidoSegundo
                var index;
                var a = $scope.IdentificacionDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codIdentificacion)
                        $scope.selectedIdentificacion = a[index];
                }
                $scope.traCho.txtIdentificacion = retorno[0].txtIdentificacion;
                $scope.traCho.txtdirpropie = retorno[0].txtdirpropie;
                $scope.traCho.txttelfpro = retorno[0].txttelfpro;

                var a = $scope.TipoVehiculosdatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codTipoVehiculo)
                        $scope.selecttipovehiculo = a[index];
                }
                $scope.traCho.txtDireccionDomicilio = retorno[0].txtDireccionDomicilio;
                $scope.traCho.txtTelefonoDomicilio = retorno[0].txtTelefonoDomicilio;
                var a = $scope.ColorPrincipalDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codColorPrincipal)
                        $scope.selectcolorprincipal = a[index];
                }
                var a = $scope.ColorSecundarioDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codColorSecundario)
                        $scope.selectcolorsecundario = a[index];
                }
                $scope.traCho.txtmotor = retorno[0].txtmotor
                var a = $scope.MarcaDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codMarca)
                        $scope.selectmarca = a[index];
                }

                $scope.ModeloDatos = [];
                if ($scope.selectmarca != '' && angular.isUndefined($scope.selectmarca) != true) {
                    $scope.ModeloDatos = $filter('filter')($scope.ModeloDatostmp,
                                                      { descAlterno: $scope.selectmarca.codigo });
                }

                var a = $scope.ModeloDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codModelo)
                        $scope.selectmodelo = a[index];
                }
                $scope.traCho.txtplaca = retorno[0].txtplaca;
                $scope.traCho.txtchasis = retorno[0].txtchasis;
                var a = $scope.EstadoDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codEstado)
                        $scope.selectEstado = a[index];
                }
                var a = $scope.BloqueoDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codBloqueo)
                        $scope.selectBloqueo = a[index];
                }
                if (retorno[0].txtFechaBloqueo == "01/01/1900") {
                    $scope.traCho.txtFechaBloqueo = "";
                } else {
                    var Fecha1 = $filter('date')(retorno[0].txtFechaBloqueo, 'dd/MM/yyyy');
                    $scope.traCho.txtFechaBloqueo = Fecha1;
                }




                $scope.traCho.txtOrigenBloque = retorno[0].txtOrigenBloque;
                $scope.traCho.txtmatricula = retorno[0].txtmatricula;

                var Fecha2 = $filter('date')(retorno[0].txtfechamatricula, 'dd/MM/yyyy');
                $scope.traCho.txtfechamatricula = Fecha2;
                Fecha2 = $filter('date')(retorno[0].txtexpiracionmatricula, 'dd/MM/yyyy');
                $scope.traCho.txtexpiracionmatricula = Fecha2;

                $scope.traCho.txtaniomatricula = retorno[0].txtaniomatricula
                var a = $scope.PaisDatos;
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codigo === retorno[0].codPais)
                        $scope.selectpais = a[index];
                }
                $scope.traCho.txtcilindraje = retorno[0].txtcilindraje;
                $scope.traCho.txttonelaje = retorno[0].txttonelaje;

                Fecha2 = $filter('date')(retorno[0].txtemisionsoat, 'dd/MM/yyyy');
                $scope.traCho.txtemisionsoat = Fecha2;
                Fecha2 = $filter('date')(retorno[0].txtexpiracionsoat, 'dd/MM/yyyy');
                $scope.traCho.txtexpiracionsoat = Fecha2;


                $scope.traCho.PorSota = retorno[0].PorSota;
                $scope.traCho.CodProveedor = retorno[0].codProveedor;
                $scope.traCho.idvehiculo = valorrecibido;
                $scope.traCho.Tipo = "2";
                $scope.traCho.txtarchivo = retorno[0].txtarchivo;
                $scope.pageContentChoP = [];
                $scope.pageContentChoH = [];
                $scope.pageContentChoP = results.data.root[1];
                $scope.pageContentChoH = results.data.root[2];
                $scope.getthefile(valorrecibido, retorno[0].txtarchivo);

                $scope.etiTotRegistrosLP = "";
                $scope.etiTotRegistrosHE = "";
                $scope.etiTotRegistrosLP = $scope.pageContentChoP.length.toString();
                $scope.etiTotRegistrosHE = $scope.pageContentChoH.length.toString();
            }
        }, function (error) {
        });



    }


    uploader.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "pdf" || extension == "doc" || extension == "docx" || extension == "rtf" || extension == "jpg" || extension == "png")
                return true;
            else {
                alert('Invalid file format. Please select a file with pdf/doc/docs or rtf format  and try again.');
                return false;
            }
        }
    });

    uploader.filters.push({
        name: 'sizeFilter',
        fn: function (item, options) {
            var fileSize = item.size;
            fileSize = parseInt(fileSize) / (1024 * 1024);
            if (fileSize <= 5)
                return true;
            else {
                alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
                return false;
            }
        }
    });

    uploader.filters.push({
        name: 'itemResetFilter',
        fn: function (item, options) {
            if (this.queue.length < 5)
                return true;
            else {
                alert('You have exceeded the limit of uploading files.');
                return false;
            }
        }
    });

    // CALLBACKS

    uploader.onWhenAddingFileFailed = function (item, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };
    uploader.onAfterAddingFile = function (fileItem) {
        $scope.traCho.txtarchivo = fileItem.file.name;
    };

    uploader.onSuccessItem = function (fileItem, response, status, headers) {
    };
    uploader.onErrorItem = function (fileItem, response, status, headers) {
        alert('We were unable to upload your file. Please try again.');
    };
    uploader.onCancelItem = function (fileItem, response, status, headers) {
        alert('File uploading has been cancelled.');
    };

    uploader.onAfterAddingAll = function (addedFileItems) {
        console.info('onAfterAddingAll', addedFileItems);
    };
    uploader.onBeforeUploadItem = function (item) {
        console.info('onBeforeUploadItem', item);
    };
    uploader.onProgressItem = function (fileItem, progress) {
        console.info('onProgressItem', fileItem, progress);
    };
    uploader.onProgressAll = function (progress) {
        console.info('onProgressAll', progress);
    };

    uploader.onCompleteItem = function (fileItem, response, status, headers) {
        console.info('onCompleteItem', fileItem, response, status, headers);
    };
    uploader.onCompleteAll = function () {
        console.info('onCompleteAll');
    };
    $scope.nuevo();

    setTimeout(function () { $('#consultagrid').focus(); }, 150);
    setTimeout(function () { angular.element('#consultagrid').trigger('click'); }, 250);
}
]);
//Consolidacion Pedidos controller
app.controller('ConsolidacionPedidosController', ['$scope', '$location', 'ConsolidacionPedidosService', '$cookies', '$filter', 'authService', function ($scope, $location, ConsolidacionPedidosService, $cookies, $filter, authService) {
    var dateFormat = function () {
        var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
            timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
            timezoneClip = /[^-+\dA-Z]/g,
            pad = function (val, len) {
                val = String(val);
                len = len || 2;
                while (val.length < len) val = "0" + val;
                return val;
            };

        return function (date, mask, utc) {
            var dF = dateFormat;


            if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }


            date = date ? new Date(date) : new Date;
            if (isNaN(date)) throw SyntaxError("invalid date");

            mask = String(dF.masks[mask] || mask || dF.masks["default"]);


            if (mask.slice(0, 4) == "UTC:") {
                mask = mask.slice(4);
                utc = true;
            }

            var _ = utc ? "getUTC" : "get",
                d = date[_ + "Date"](),
                D = date[_ + "Day"](),
                m = date[_ + "Month"](),
                y = date[_ + "FullYear"](),
                H = date[_ + "Hours"](),
                M = date[_ + "Minutes"](),
                s = date[_ + "Seconds"](),
                L = date[_ + "Milliseconds"](),
                o = utc ? 0 : date.getTimezoneOffset(),
                flags = {
                    d: d,
                    dd: pad(d),
                    ddd: dF.i18n.dayNames[D],
                    dddd: dF.i18n.dayNames[D + 7],
                    m: m + 1,
                    mm: pad(m + 1),
                    mmm: dF.i18n.monthNames[m],
                    mmmm: dF.i18n.monthNames[m + 12],
                    yy: String(y).slice(2),
                    yyyy: y,
                    h: H % 12 || 12,
                    hh: pad(H % 12 || 12),
                    H: H,
                    HH: pad(H),
                    M: M,
                    MM: pad(M),
                    s: s,
                    ss: pad(s),
                    l: pad(L, 3),
                    L: pad(L > 99 ? Math.round(L / 10) : L),
                    t: H < 12 ? "a" : "p",
                    tt: H < 12 ? "am" : "pm",
                    T: H < 12 ? "A" : "P",
                    TT: H < 12 ? "AM" : "PM",
                    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
                };

            return mask.replace(token, function ($0) {
                return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
            });
        };
    }();


    dateFormat.masks = {
        "default": "ddd mmm dd yyyy HH:MM:ss",
        shortDate: "m/d/yy",
        mediumDate: "mmm d, yyyy",
        longDate: "mmmm d, yyyy",
        fullDate: "dddd, mmmm d, yyyy",
        shortTime: "h:MM TT",
        mediumTime: "h:MM:ss TT",
        longTime: "h:MM:ss TT Z",
        isoDate: "yyyy-mm-dd",
        isoTime: "HH:MM:ss",
        isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
        isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
    };


    dateFormat.i18n = {
        dayNames: [
            "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        ],
        monthNames: [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
        ]
    };


    Date.prototype.format = function (mask, utc) {
        return dateFormat(this, mask, utc);
    };
    $scope.formData = {};
    $scope.etiTotRegistros = "";
    $scope.etiTotRegistrosLP = "";

    $scope.div1 = false;
    $scope.div2 = true;
    $scope.imagenurl = "";
    //Variable de Grid
    $scope.GridConsolidacion = [];
    var _GridConsolidacion = [];
    $scope.pagesCo = [];
    $scope.pageContentCo = [];

    $scope.sortTypeCon = 'idconsolidacion';
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.GridTransporteP = [];
    var _GridTransporteP = [];
    $scope.pagesChoP = [];

    var idgridp = 0;

    //fin Grid

    //Variable de Busquedas
    $scope.PorFechas = 1;
    $scope.fechahabilitar = true;
    $scope.txtfechadesde = "";
    $scope.txtfechahasta = "";
    $scope.PorNumero = 1;
    $scope.txtnumero = "";
    $scope.Portodosfacturas = true;

    $scope.EstadoSolicitud = [];
    $scope.selectedItemEstado = [];
    $scope.SettingEstadoSol = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };
    //Fin Variable


    //Combos Formularios
    $scope.Vehiculosdatos = [];
    $scope.selectvehiculo = "";
    $scope.Choferdatos = [];
    $scope.selectchofer = "";
    $scope.Ayudantesdatos = [];
    $scope.selectayudante = "";

    $scope.selectbodega = "";
    $scope.BodegaDatos = [];

    $scope.habilitar = true;
    //Fin Combos


    //Accion a realizar en modal de confirmacion
    $scope.accion = 0;
    $scope.estadoCross = false;
    //Cabecera de datos de pedidos Cross
    $scope.datosDistribucion = [];
    $scope.datosTDetalleCross = [];
    $scope.datosDetalleCross = [];
    $scope.datosDetalleCrossHis = [];
    $scope.CabCross = {};
    $scope.CabCross.codProveedor = "";
    $scope.CabCross.nomProveedor = "";
    $scope.CabCross.numPedido = "";

    //Detalle de datos de pedidos Cross
    $scope.DetCross = {};
    $scope.DetCross.codArticulo = "";
    $scope.DetCross.desArticulo = "";
    $scope.DetCross.cantPlanificada = "";
    $scope.DetCross.uniPlanificada = "";
    $scope.DetCross.cantReal = "";
    $scope.DetCross.uniReal = "";

    //Cabecera Distribucion Tiendas
    $scope.CabTiendas = {};
    $scope.CabTiendas.codArticulo = "";
    $scope.CabTiendas.desArticulo = "";
    $scope.CabTiendas.canPlanificada = "";
    $scope.CabTiendas.uniPlanificada = "";






    //Variable de pedido Factura
    $scope.traCho = {};
    $scope.traCho.idconsolidacion = "";
    $scope.traCho.Tipo = "1";
    $scope.traCho.codChofer = "";
    $scope.traCho.codAyudante = "";
    $scope.traCho.codVehiculo = "";
    $scope.traCho.codAlmacenDestino = "";
    $scope.traCho.codProveedor = authService.authentication.CodSAP;
    $scope.traCho.usuarioCreacion = authService.authentication.userName;
    $scope.traCho.cosRapido = false;
    $scope.pageContentChoP = [];
    $scope.pageContentChoPD = [];
    $scope.pageContentChoPGloabal = [];
    $scope.pageContentChoPQ = [];
    $scope.pageContentChoPGloabalQ = [];
    //fin Variable de pedido Factura

    $scope.catDistibui = "";
    $scope.catDistibuiped = "";



    $scope.traChop = {};
    $scope.traChop.txtcodigoconsolidacion = "";
    $scope.traChop.txtestado = "";
    $scope.traChop.txtfechaemision = "";
    $scope.traChop.txtalmacendestino = "";
    $scope.traChop.txtcaducidad = "";


    $scope.traChod = {};
    $scope.traChod.txtnumeropedido


    $scope.sortType = 'numplaca'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';

    //Fin Variable

    //Variable de Mensaje
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    //Fin Variable


    $scope.fecha = {};
    $scope.fecha.txtfechadesde = "";
    $scope.fecha.txtfechahasta = "";


    $scope.numeroTmp = "";
    $scope.fechaTmp = "";

    //Carga Catalogo Estado
    ConsolidacionPedidosService.getCatalogo('tbl_EstadoGeneral').then(function (results) {
        $scope.EstadoSolicitud = results.data;
    }, function (error) {
    });



    //Carga Vehiculo
    ConsolidacionPedidosService.getChoferVehiculos('2', $scope.traCho.codProveedor).then(function (results) {
        $scope.Vehiculosdatos = results.data.root[0];

    }, function (error) {
    });

    //Carga Chofer
    ConsolidacionPedidosService.getChoferVehiculos('1', $scope.traCho.codProveedor).then(function (results) {
        $scope.Choferdatos = results.data.root[0];
    }, function (error) {
    });

    //Carga Ayudate
    ConsolidacionPedidosService.getChoferVehiculos('1', $scope.traCho.codProveedor).then(function (results) {
        $scope.Ayudantesdatos = results.data.root[0];
    }, function (error) {
    });

    //Carga Bodega
    ConsolidacionPedidosService.getChoferVehiculos('6', $scope.traCho.codProveedor).then(function (results) {
        $scope.BodegaDatos = results.data.root[0];
    }, function (error) {
    });

    $scope.SelecionaComboNUEVO = function () {
        if ($scope.selectayudante != undefined && $scope.selectchofer != undefined) {
            if ($scope.selectayudante.codigo == $scope.selectchofer.codigo) {
                $scope.MenjError = "El chofer y el ayudante no pueden ser iguales"
                $('#idMensajeError').modal('show');
            }
        } else {
            $scope.MenjError = "Debe Selecionar un chofer y un ayudante"
            $('#idMensajeError').modal('show');
        }


    }



    $scope.getthefile = function () {
        $scope.myPromise = ConsolidacionPedidosService.getthefile().then(function (results) {

            $scope.imagenurl = results.data;
        }, function (error) {
        });
    }
    $scope.limpioCaja = function () {
        if ($scope.PorFechas === "1") {
            $scope.fecha.txtfechadesde = "";
            $scope.fecha.txtfechahasta = "";
            $scope.fechahabilitar = true;
        } else {
            var dateString = new Date();
            var d1 = dateString.format("dd/mm/yyyy");

            $scope.fecha.txtfechadesde = d1;
            $scope.fecha.txtfechahasta = d1;

            $scope.fechahabilitar = false;

        }
        if ($scope.PorEstadosTipo === "1") {
            $scope.selectedItemEstado = "";
        }

        if ($scope.PorNumero === "1") {
            $scope.txtnumero = "";
        }
    }

    $scope.cambiarBodega = function () {
        if ($scope.selectbodega != undefined) {
            $scope.cargarPedido();
        } else {
            $scope.pageContentChoP = [];
            $scope.pageContentChoPGloabal = [];
        }
    }
    function Limpiar() {
        $scope.selectvehiculo = "";
        $scope.selectchofer = "";
        $scope.selectayudante = "";
        $scope.selectbodega = "";
        $scope.traCho.Tipo = "1";
        $scope.pageContentChoP = [];
        $scope.pageContentChoPGloabal = [];
        $scope.pageContentChoPQ = [];
        $scope.pageContentChoPGloabalQ = [];
        $scope.traCho.cosRapido = false;
        $scope.habilitar = false;
    }
    function LimpiarGrabar() {
        $scope.selectvehiculo = "";
        $scope.selectchofer = "";
        $scope.selectayudante = "";
        $scope.selectbodega = "";
        $scope.pageContentChoP = [];
        $scope.pageContentChoPGloabal = [];
        $scope.pageContentChoPQ = [];
        $scope.pageContentChoPGloabalQ = [];
        $scope.traCho.cosRapido = false;
        $scope.habilitar = true;
        $scope.traChop.txtcodigoconsolidacion = "";
        $scope.traChop.txtestado = "";
        $scope.traChop.txtfechaemision = "";
        $scope.traChop.txtalmacendestino = "";
        $scope.traChop.txtcaducidad = "";
        $('.nav-tabs a[href="#ConsolidacionesDisponibles"]').tab('show');
        setTimeout(function () { $('#consultagrid').focus(); }, 150);
        setTimeout(function () { angular.element('#consultagrid').trigger('click'); }, 250);
    }
    $scope.cargarPedido = function () {
        $scope.myPromise = null;
        $scope.myPromise = ConsolidacionPedidosService.getBuscarPedidosBodega("8", $scope.traCho.codProveedor, $scope.selectbodega.codigo).then(function (results) {
            $scope.pageContentChoP = results.data.root[0];
        }, function (error) {
        });
    }
    $scope.nuevo = function () {

        Limpiar();
    }

    $scope.cancelar = function () {
        $scope.selectvehiculo = "";
        $scope.selectchofer = "";
        $scope.selectayudante = "";
        $scope.selectbodega = "";
        $scope.pageContentChoP = [];
        $scope.pageContentChoPGloabal = [];
        $scope.traChop.txtcodigoconsolidacion = "";
        $scope.traChop.txtestado = "";
        $scope.traCho.cosRapido = "";
        $scope.traChop.txtfechaemision = "";
        $scope.traChop.txtalmacendestino = "";
        $scope.traChop.txtcaducidad = "";
        $scope.habilitar = true;
    }
    $scope.cambio = function () {
        if ($scope.Portodosfacturas == true) {
            for (var i = 0; i < $scope.pageContentChoPD.length; i++) {
                if (i == 0) {
                    $scope.numeroTmp = $scope.pageContentChoPD[i].factura;
                    $scope.fechaTmp = $scope.pageContentChoPD[i].fechaFactura;
                } else {
                    $scope.pageContentChoPD[i].factura = $scope.numeroTmp;
                    $scope.pageContentChoPD[i].fechaFactura = $scope.fechaTmp;
                }
                $scope.pageContentChoPD[i].cantidadDespachada = $scope.pageContentChoPD[i].cantidadxDespachar;
                $scope.pageContentChoPD[i].cantidadPediente = $scope.pageContentChoPD[i].cantidadxDespachar - $scope.pageContentChoPD[i].cantidadDespachada;
            }
        } else {
            for (var i = 0; i < $scope.pageContentChoPD.length; i++) {
                if (i == 0) {
                    $scope.pageContentChoPD[i].factura = $scope.numeroTmp;
                    $scope.pageContentChoPD[i].fechaFactura = $scope.fechaTmp;
                } else {
                    $scope.pageContentChoPD[i].factura = "";
                    $scope.pageContentChoPD[i].fechaFactura = "";
                }
                $scope.pageContentChoPD[i].cantidadDespachada = 0;
                $scope.pageContentChoPD[i].cantidadPediente = $scope.pageContentChoPD[i].cantidadxDespachar;
            }
        }
    }
    $scope.BuscarFiltro = function () {
        $scope.Consultar();
    }

    $scope.Confirmargrabar = function () {
        if ($scope.traCho.Tipo === "1") {
            $scope.MenjConfirmacion = "¿Está seguro de guardar la información?"
        }
        if ($scope.traCho.Tipo === "2") {
            $scope.MenjConfirmacion = "¿Está seguro de modificar la información?"
        }
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.cancelarDetTienda = function () {
        for (var idx = 0; idx < $scope.datosDistribucion.length; idx++) {
            var update = $scope.datosDistribucion[idx];
            update.pCantidadProveedor = update.pCantidadRealDistribucion;
        }
        $('#DetalleTiendas').modal('hide');
    }


    $scope.confirmacionFinDistribucion = function () {
        $scope.accion = 999;
        $scope.MenjConfirmacion = "Si finaliza la distribución el sistema no le permitirá realizar más cambios."
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.verDetalleHistorico = function () {


        $scope.myPromise = ConsolidacionPedidosService.getBuscarDetPedidosCrossHis($scope.CabCross.numPedido, authService.authentication.CodSAP, "0").then(function (results) {
            if (results.data.success) {
                debugger;
                $scope.datosDetalleCrossHis = results.data.root[0];
                $('#DetallePedidoCrossHis').modal('show');
            }
            else {
                $scope.MenjError = results.data.msgError;
                $('#idMensajeError').modal('show');
            }

        }, function (error) {

            $scope.MenjError = "Error en comunicación: getBuscarDetPedidosCrossHis().";
            $('#idMensajeError').modal('show');

        });
    }

    $scope.finalizarDistribucion = function () {
        //Finalizar distribucion

        $scope.myPromise = ConsolidacionPedidosService.getActualizaPedidoCross($scope.datosTDetalleCross, "1", authService.authentication.userName).then(function (results) {
            if (results.data.success) {
                $scope.MenjError = "Distribución finalizada correctamente, puede generar reportes del detalle de la distribución. ";
                $('#idMensajeOk').modal('show');

            }
            else {
                $scope.MenjError = results.data.msgError;
                $('#idMensajeError').modal('show');
            }

        }, function (error) {

            $scope.MenjError = "Error en comunicación: getActualizaPedidoCross().";
            $('#idMensajeError').modal('show');

        });
    }

    $scope.grabarDetTienda = function () {
        var newCantidadReal = 0;
        for (var idx = 0; idx < $scope.datosDistribucion.length; idx++) {

            var update = $scope.datosDistribucion[idx];
            var montoMinimo = Math.round(update.pCantidadDistribucion * update.pTolerancia / 100);
            var porcentajeTolerancia = update.pTolerancia;
            if (String(update.pCantidadProveedor) == "") {
                $scope.MenjError = "Ingrese cantidad a distribuir en almacén " + update.pNomAlmacen;
                $('#idMensajeInformativo').modal('show');
                return;
            }
            var cantNumerico = parseInt(update.pCantidadProveedor)
            if (cantNumerico < 0) {
                $scope.MenjError = "Ingrese una cantidad mayor a 0 en almacén " + update.pNomAlmacen;
                $('#idMensajeInformativo').modal('show');
                return;
            }
            if (cantNumerico < montoMinimo) {
                $scope.MenjError = "Cantidad no puede ser menor a " + montoMinimo + " (correspondiente al " + porcentajeTolerancia + "% de la cantidad planificada) en almacén " + update.pNomAlmacen;
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if (cantNumerico > update.pCantidadDistribucion) {
                $scope.MenjError = "Cantidad no puede ser mayor a Cantidad Planificada en almacén " + update.pNomAlmacen;
                $('#idMensajeInformativo').modal('show');
                return;
            }


            update.pCantidadRealDistribucion = parseInt(update.pCantidadProveedor);
            update.pCantidadProveedor = update.pCantidadRealDistribucion;
            newCantidadReal = newCantidadReal + update.pCantidadRealDistribucion;

        }

        //Grabar datos

        $scope.myPromise = ConsolidacionPedidosService.getActualizaPedidoCross($scope.datosDistribucion, "0", authService.authentication.userName).then(function (results) {
            if (results.data.success) {
                //Actualizar totales por artículos
                var CodArtBusqueda = $scope.CabTiendas.codArticulo;
                var updateCons = $filter('filter')($scope.datosDetalleCross, { codArticulo: CodArtBusqueda }, true)[0];
                updateCons.cantReal = newCantidadReal;
                $('#DetalleTiendas').modal('hide');
            }
            else {
                $scope.MenjError = results.data.msgError;
                $('#idMensajeError').modal('show');
            }

        }, function (error) {

            $scope.MenjError = "Error en comunicación: getActualizaPedidoCross().";
            $('#idMensajeError').modal('show');

        });



    }

    $scope.verDetalleTiendas = function (codArticulo, desArticulo, canPlanificada, uniPlanificada,tipoPedidos) {

        $scope.datosDistribucion = [];
        $scope.datosDistribucion = $filter('filter')($scope.datosTDetalleCross, { pCodArticulo: codArticulo }, true);
        $scope.catDistibui = "";
        $scope.catDistibui = $scope.datosDistribucion.length

        $scope.CabTiendas = {};
        $scope.CabTiendas.codArticulo = codArticulo;
        $scope.CabTiendas.desArticulo = desArticulo;
        $scope.CabTiendas.canPlanificada = canPlanificada;
        $scope.CabTiendas.uniPlanificada = uniPlanificada;

        if (tipoPedidos=="2") {
            $('#DetalleTiendas').modal('show');
        }
        if (tipoPedidos == "3") {
            $('#DetalleTiendasFlow').modal('show');
        }

    }

    $scope.DistribucionCross = function (registro) {

        $scope.CabCross.codProveedor = authService.authentication.CodSAP;
        $scope.CabCross.nomProveedor = authService.authentication.nomEmpresa;
        $scope.CabCross.numPedido = registro.numPedido;

        //Consulta de detalle del pedido cross

        $scope.myPromise = ConsolidacionPedidosService.getBuscarDetPedidosCross(registro.numPedido, authService.authentication.CodSAP).then(function (results) {

            $scope.datosDetalleCross = [];
            $scope.datosTDetalleCross = [];
            if (results.data.success) {
                var listaRegistros = results.data.root[0];
                $scope.datosTDetalleCross = listaRegistros;
                var existe;
                for (var i = 0 ; i < listaRegistros.length; i++) {

                    if (listaRegistros[i].pEstado == "1")
                        $scope.estadoCross = true;
                    else
                        $scope.estadoCross = false;
                    var CodArticuloBus = listaRegistros[i].pCodArticulo;
                    if ($scope.datosDetalleCross.length > 0) {
                        existe = $filter('filter')($scope.datosDetalleCross, { codArticulo: CodArticuloBus }, true)[0];
                    }

                    if (existe != undefined) {
                        existe.cantPlanificada = existe.cantPlanificada + listaRegistros[i].pCantidadDistribucion;
                        existe.cantReal = existe.cantReal + listaRegistros[i].pCantidadRealDistribucion;
                    }
                    else {
                        $scope.DetCross = {};
                        $scope.DetCross.codArticulo = listaRegistros[i].pCodArticulo;
                        $scope.DetCross.desArticulo = listaRegistros[i].pDesArticulo;
                        $scope.DetCross.cantPlanificada = listaRegistros[i].pCantidadDistribucion;
                        $scope.DetCross.uniPlanificada = listaRegistros[i].pUnidadDistribucion;
                        if (listaRegistros[i].pCantidadRealDistribucion=="0") {
                            $scope.DetCross.cantReal = listaRegistros[i].pCantidadDistribucion;
                        } else
                        {
                            $scope.DetCross.cantReal = listaRegistros[i].pCantidadRealDistribucion;
                        }
                        $scope.DetCross.uniReal = listaRegistros[i].pUnidadRealDistribucion;
                        $scope.datosDetalleCross.push($scope.DetCross);

                    }
                }
                $scope.catDistibuiped = "";
                $scope.catDistibuiped = $scope.datosDetalleCross.length;

            }
            else {
                $scope.MenjError = results.data.msgError;
                $('#idMensajeError').modal('show');
            }

        }, function (error) {

            $scope.MenjError = "Error en comunicación: getBuscarDetPedidosCross().";
            $('#idMensajeError').modal('show');

        });



        $('#DetallePedidoCross').modal('show');
    }

    //Boton aceptar de modal OK
    $("#btnMensajeOK").click(function () {

        $('#DetallePedidoCross').modal('hide');
    });

    //Boton Cancelar
    $("#btnCancelar").click(function () {
        $scope.accion = 0;

    });




    $scope.grabar = function () {
        if ($scope.accion == 999) {


            $scope.accion = 0;
            $scope.finalizarDistribucion();
        }
        else {
            if ($scope.traCho.Tipo === "1") {
                if ($scope.traCho.codChofer == null) {
                    $scope.traCho.codChofer = "";
                } else {
                    $scope.traCho.codChofer = $scope.selectchofer.codigo;
                }

             
                if ($scope.selectayudante==null) {
                    $scope.traCho.codAyudante = "";
                } else
                {
                    $scope.traCho.codAyudante = $scope.selectayudante.codigo;
                }
                
                $scope.traCho.codVehiculo = $scope.selectvehiculo.codigo;
                $scope.traCho.codAlmacenDestino = $scope.selectbodega.codigo;
                var gripp = $filter('filter')($scope.pageContentChoP, { chkpedido: true });
                var grippt = [];

                if (gripp.length == 0) {
                    $scope.MenjError = "Se debe seleccionar al menos un pedido."
                    $('#idMensajeError').modal('show');
                    return;
                } else
                {
                    for (var i = 0; i < gripp.length; i++) {
                        if (gripp[i].numeroBulto == "0" || gripp[i].numeroPalet == "0") {
                            $scope.MenjError = "Debe ingresar numero de Bulto y numeros de Palet en las ordenes."
                            $('#idMensajeError').modal('show');
                            return;
                        }
                    }
                }

                if ($scope.traCho.cosRapido == 0) {
                    for (var i = 0; i < $scope.pageContentChoPGloabal.length; i++) {
                        if ($scope.pageContentChoPGloabal[i].cantidadxDespachar != "0")
                            grippt.push($scope.pageContentChoPGloabal[i]);
                    }
                } else {
                    for (var i = 0; i < $scope.pageContentChoPGloabalQ.length; i++) {
                        var grid = {};
                        grid.item = $scope.pageContentChoPGloabalQ[i].id;
                        grid.numPedido = $scope.pageContentChoPGloabalQ[i].pedido;
                        grid.codigoProducto = $scope.pageContentChoPGloabalQ[i].id;
                        grid.factura = $scope.pageContentChoPGloabalQ[i].factura;
                        grid.fechaFactura = "";
                        grid.descripcion = "";
                        grid.cantidadPedido = "0";
                        grid.precioUnitario = "0";
                        grid.unidadCaja = "0";
                        grid.descuento1 = "0";
                        grid.descuento2 = "0";
                        grid.iva = "0";
                        grid.subtotal = "0";
                        grid.total = "0";
                        grid.cantidadxDespachar = "0";
                        grid.cantidadDespachada = "0";
                        grid.cantidadPediente = "0";
                        grippt.push(grid);
                    }
                }
                if (grippt.length == 0) {
                    $scope.MenjError = "Se debe tener por lo menos un detalle de un pedido."
                    $('#idMensajeError').modal('show');
                    return;
                }

                $scope.myPromise = null;
                $scope.myPromise = ConsolidacionPedidosService.getGrabaConsolidacion($scope.traCho, gripp, grippt).then(function (results) {
                    if (results.data.split("|")[0] == "true") {
                        if ($scope.traCho.Tipo === "1") {
                            $scope.MenjError = "Se ingreso la consolidación correctamente N°" + results.data.split("|")[1];
                            $('#idMensajeGrabar').modal('show');
                        }
                    }
                    else {
                        if (results.data.msgError === "EXISTE") {
                            if ($scope.traCho.Tipo === "1") {
                                $scope.MenjError = "La consolidación ya exite verifique"
                                $('#idMensajeError').modal('show');
                            }
                        } else {
                            $scope.MenjError = "No se puedo grabar el registro"
                            $('#idMensajeError').modal('show');
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
                  $scope.MenjError = "No se puedo grabar el registro"
                  $('#idMensajeError').modal('show');
              });
            } else {
                $scope.traCho.idconsolidacion = $scope.traChop.txtcodigoconsolidacion;
                $scope.traCho.codChofer = $scope.selectchofer.codigo;
                $scope.traCho.codAyudante = $scope.selectayudante.codigo;
                $scope.traCho.codVehiculo = $scope.selectvehiculo.codigo;
                $scope.traCho.codAlmacenDestino = $scope.selectbodega.codigo;
                var gripp = $filter('filter')($scope.pageContentChoP, { chkpedido: true });
                var grippt = [];
                if ($scope.traCho.cosRapido == 0) {
                    for (var i = 0; i < $scope.pageContentChoPGloabal.length; i++) {
                        if ($scope.pageContentChoPGloabal[i].cantidadxDespachar != "0")
                            grippt.push($scope.pageContentChoPGloabal[i]);
                    }
                } else {
                    for (var i = 0; i < $scope.pageContentChoPGloabalQ.length; i++) {
                        var grid = {};
                        grid.item = $scope.pageContentChoPGloabalQ[i].id;
                        grid.numPedido = $scope.pageContentChoPGloabalQ[i].pedido;
                        grid.codigoProducto = $scope.pageContentChoPGloabalQ[i].id;
                        grid.factura = $scope.pageContentChoPGloabalQ[i].factura;
                        grid.fechaFactura = "";
                        grid.descripcion = "";
                        grid.cantidadPedido = "0";
                        grid.precioUnitario = "0";
                        grid.unidadCaja = "0";
                        grid.descuento1 = "0";
                        grid.descuento2 = "0";
                        grid.iva = "0";
                        grid.subtotal = "0";
                        grid.total = "0";
                        grid.cantidadxDespachar = "0";
                        grid.cantidadDespachada = "0";
                        grid.cantidadPediente = "0";
                        grippt.push(grid);
                    }
                }
                $scope.myPromise = null;
                $scope.myPromise = ConsolidacionPedidosService.getGrabaConsolidacion($scope.traCho, gripp, grippt).then(function (results) {
                    if (results.data) {
                        if ($scope.traCho.Tipo === "2") {
                            $scope.MenjError = "Se modificó la consolidación correctamente"
                            $('#idMensajeGrabar').modal('show');
                        }
                    }
                    else {
                        if (results.data.msgError === "EXISTE") {
                            if ($scope.traCho.Tipo === "2") {
                                $scope.MenjError = "La consolidación ya exite verifique"
                                $('#idMensajeError').modal('show');
                            }
                        } else {
                            $scope.MenjError = "No se puedo grabar el registro"
                            $('#idMensajeError').modal('show');
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
                  $scope.MenjError = "No se puedo grabar el registro"
                  $('#idMensajeError').modal('show');
              });


            }
        }
    }
    $scope.okGrabar = function () {
        LimpiarGrabar();

    }

    $scope.Consultar = function () {
        var index;
        var estados = new Array();
        var a = $scope.selectedItemEstado;
        for (index = 0; index < a.length; ++index) {
            estados[index] = a[index].id;
        }
        var a1 = "";
        var a2 = "";
        if ($scope.fecha.txtfechadesde != "") {
            var parts1 = $scope.fecha.txtfechadesde.split('/');

            a1 = new Date(parts1[2], parts1[1] - 1, parts1[0]);

        }

        if ($scope.fecha.txtfechahasta != "") {
            var parts2 = $scope.fecha.txtfechahasta.split('/');

            a2 = new Date(parts2[2], parts2[1] - 1, parts2[0]);

        }

        var fecha1 = $filter('date')(a1, 'yyyy-MM-dd');
        var fecha2 = $filter('date')(a2, 'yyyy-MM-dd');



        $scope.myPromise = null;
        $scope.etiTotRegistros = "";
        $scope.myPromise = ConsolidacionPedidosService.getBuscarConsolidacion($scope.txtnumero, $scope.traCho.codProveedor, estados, fecha1, fecha2).then(function (results) {
            $scope.GridConsolidacion = results.data.root[0];
            if (results.data.msgError == "NO") {
                $scope.etiTotRegistros = "0";
            }
            else {
                $scope.etiTotRegistros = $scope.GridConsolidacion.length.toString();
            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
        }, function (error) {
        });
    }


    $scope.ConsultarDespues = function () {

        if ($scope.PorNumeroCed === "2") {
            if ($scope.txtNumero === "") {
                $scope.MenjError = "Debe ingresar cedula o ruc a consultar"
                $('#idMensajeError').modal('show');
                return;
            } else {
                if (isNaN($scope.txtNumero)) {
                    $scope.MenjError = "Este campo debe tener sólo números."
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
        }

        if ($scope.PorEstadosTipo === "2") {
            if ($scope.selectedItemTipo === "") {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.selectedItemTipo === null) {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }
        }

        $scope.etiTotRegistros = "";
        $scope.myPromise = null;
        $scope.myPromise = ConsolidacionPedidosService.getConsulaGrid("1", $scope.txtNumero, $scope.txtPorNombre, $scope.txtPorApellido, $scope.selectedItemTipo.codigo, $scope.traCho.CodProveedor).then(function (results) {
            if (results.data.success) {
                $scope.pageContentCho = results.data.root[0];
                $scope.etiTotRegistros = $scope.pageContentCho.length.toString();
            }
        }, function (error) {
        });
        setTimeout(function () { $('#consultagrid').focus(); }, 10);
    }
    $scope.modi = function () {
        $scope.div1 = false;
        $scope.div2 = true;
    }

    $scope.calculo = function (content) {
        content.cantidadPediente = content.cantidadxDespachar - content.cantidadDespachada;
    }
    $scope.exportar = function (tipo) {
        if ($scope.GridConsolidacion.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeGrabar').modal('show');
            return;
        }
        $scope.myPromise = null;
        $scope.Tablacabecera = {};
        $scope.Tablacabecera.tipo = tipo;
        $scope.Tablacabecera.usuario = $scope.traCho.usuarioCreacion;
        $scope.myPromise = ConsolidacionPedidosService.getExportarDataConsolidacion($scope.Tablacabecera, $scope.GridConsolidacion).then(function (results) {
            if (results.data != "") {
                window.open(results.data, '_blank', "");
            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
        }, function (error) {
        });

    }
    $scope.SelecionarGrid = function (chkpedido, numPedido) {
        if (chkpedido == false) {
            $scope.MenjError = "Debe selecionar el registro que desea ingresar datos de pedidos"
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.traCho.cosRapido == false) {
            $scope.traChod.txtnumeropedido = numPedido;

            $scope.Portodosfacturas = false;
            $scope.pageContentChoPD = [];
            $scope.myPromise = null;
            $scope.myPromise = ConsolidacionPedidosService.getBuscarDetPedidos("4", numPedido).then(function (results) {
                var aux = $filter('filter')($scope.pageContentChoPGloabal, { numPedido: $scope.traChod.txtnumeropedido });
                if (aux.length != 0) {
                    for (var i = 0; i < aux.length; i++) {
                        $scope.p_Dp = {};
                        $scope.p_Dp.item = aux[i].item;
                        $scope.p_Dp.numPedido = aux[i].numPedido;
                        $scope.p_Dp.codigoProducto = aux[i].codigoProducto;
                        $scope.p_Dp.factura = aux[i].factura;
                        $scope.p_Dp.fechaFactura = new Date(aux[i].fechaFactura);
                        $scope.p_Dp.descripcion = aux[i].descripcion;
                        $scope.p_Dp.cantidadPedido = aux[i].cantidadPedido;
                        $scope.p_Dp.precioUnitario = aux[i].precioUnitario;
                        $scope.p_Dp.unidadCaja = aux[i].unidadCaja;
                        $scope.p_Dp.descuento1 = aux[i].descuento1;
                        $scope.p_Dp.descuento2 = aux[i].descuento2;
                        $scope.p_Dp.iva = aux[i].iva;
                        $scope.p_Dp.subtotal = aux[i].subtotal;
                        $scope.p_Dp.total = aux[i].total;
                        $scope.p_Dp.cantidadxDespachar = aux[i].cantidadxDespachar;
                        $scope.p_Dp.cantidadDespachada = aux[i].cantidadDespachada;
                        $scope.p_Dp.cantidadPediente = aux[i].cantidadPediente;
                        $scope.p_Dp.extfactura = 0;
                        $scope.pageContentChoPD.push($scope.p_Dp);
                    }
                } else {
                    $scope.pageContentChoPD = results.data.root[0];
                }
            }, function (error) {
            });

            $('#IngPedidoFactura').modal('show');
        } else {
            $scope.traChod.txtnumeropedido = numPedido;
            $scope.pageContentChoPQ = [];
            var aux1 = $filter('filter')($scope.pageContentChoPGloabalQ, { pedido: $scope.traChod.txtnumeropedido });
            if (aux1.length != 0) {
                for (var i = 0; i < aux1.length; i++) {
                    var auxgrp = {};
                    auxgrp.id = aux1[i].id;
                    auxgrp.pedido = aux1[i].pedido;
                    auxgrp.factura = aux1[i].factura;
                    auxgrp.extfactura = aux1[i].extfactura;
                    $scope.pageContentChoPQ.push(auxgrp);
                }
            } else {
                var auxgrp = {};
                auxgrp.id = 1;
                auxgrp.pedido = $scope.traChod.txtnumeropedido;
                auxgrp.factura = "";
                auxgrp.extfactura = 0;
                $scope.pageContentChoPQ.push(auxgrp);
            }
            $('#IngPedidoFactura2').modal('show');
        }
    }

    $scope.agregarItem = function (item, codigoProducto, factura, fechaFactura, descripcion, cantidadPedido, precioUnitario, unidadCaja, descuento1, descuento2, iva, subtotal, total, cantidadxDespachar, cantidadDespachada, CantidadPediente, content) {
        if (CantidadPediente > 0 && CantidadPediente != cantidadxDespachar) {
            var max = $scope.pageContentChoPD.length;
            $scope.p_Dp = {};
            $scope.p_Dp.item = $scope.pageContentChoPD[max - 1].item + 1;
            $scope.p_Dp.numPedido = $scope.traChod.txtnumeropedido;
            $scope.p_Dp.codigoProducto = codigoProducto;
            $scope.p_Dp.factura = "";
            $scope.p_Dp.fechaFactura = "";
            $scope.p_Dp.descripcion = descripcion;
            $scope.p_Dp.cantidadPedido = cantidadPedido;
            $scope.p_Dp.precioUnitario = precioUnitario;
            $scope.p_Dp.unidadCaja = unidadCaja;
            $scope.p_Dp.descuento1 = descuento1;
            $scope.p_Dp.descuento2 = descuento2;
            $scope.p_Dp.iva = iva;
            $scope.p_Dp.subtotal = subtotal;
            $scope.p_Dp.total = total;
            $scope.p_Dp.cantidadxDespachar = CantidadPediente;
            $scope.p_Dp.cantidadDespachada = 0;
            $scope.p_Dp.cantidadPediente = CantidadPediente;
            $scope.p_Dp.extfactura = 0;
            content.cantidadxDespachar = content.cantidadxDespachar - CantidadPediente;
            content.cantidadPediente = content.cantidadxDespachar - content.cantidadDespachada;
            $scope.pageContentChoPD.push($scope.p_Dp);
        }

    }
    $scope.grabargrip = function (tipo) {
        var bandera = 0;
        if (tipo == 1) {


            var baridofacturap = $scope.pageContentChoPD;
            for (var i = 0; i < baridofacturap.length; i++) {
                for (var j = 0; j < $scope.pageContentChoPGloabal.length; j++) {
                    if (baridofacturap[i].cantidadDespachada != "0") {
                        if (baridofacturap[i].numPedido != $scope.pageContentChoPGloabal[j].numPedido) {
                            if ($scope.pageContentChoPGloabal[j].factura == baridofacturap[i].factura) {
                                bandera = 2;
                                break;
                            }
                        }
                    }
                }
            }

            for (var j = 0; j < $scope.pageContentChoPD.length; j++) {
                if ($scope.pageContentChoPD[j].cantidadxDespachar - $scope.pageContentChoPD[j].cantidadDespachada < 0) {
                    bandera = 1;
                    break;
                }
            }
            if (bandera == 1) {
                $scope.MenjError = "Hay columna cantidad pendiente en negativo por favor verifique"
                $('#idMensajeError').modal('show');
                return;
            } else {
                bandera = 0;
                for (var j = 0; j < $scope.pageContentChoPD.length; j++) {
                    if ($scope.pageContentChoPD[j].cantidadDespachada != 0) {
                        if ($scope.pageContentChoPD[j].factura == "" || $scope.pageContentChoPD[j].fechaFactura == "") {
                            bandera = 1;
                            break;
                        }
                    }
                }

                for (var j = 0; j < $scope.pageContentChoPD.length; j++) {
                    if ($scope.pageContentChoPD[j].cantidadDespachada != 0) {
                        if ($scope.pageContentChoPD[j].factura.length < 15) {
                            bandera = 3;
                            break;
                        }
                    }
                }

                if (bandera == 1000000000) {
                }
                else {
                    var griptmp = $filter('filter')($scope.pageContentChoPGloabal, { numPedido: $scope.traChod.txtnumeropedido });
                    if (griptmp.length == 0) {
                        for (var i = 0; i < $scope.pageContentChoPD.length; i++) {
                            $scope.p_Dp = {};
                            $scope.p_Dp.item = $scope.pageContentChoPD[i].item;
                            $scope.p_Dp.numPedido = $scope.traChod.txtnumeropedido;
                            $scope.p_Dp.codigoProducto = $scope.pageContentChoPD[i].codigoProducto;
                            $scope.p_Dp.factura = $scope.pageContentChoPD[i].factura;
                            $scope.p_Dp.fechaFactura = $scope.pageContentChoPD[i].fechaFactura;
                            $scope.p_Dp.descripcion = $scope.pageContentChoPD[i].descripcion;
                            $scope.p_Dp.cantidadPedido = $scope.pageContentChoPD[i].cantidadPedido;
                            $scope.p_Dp.precioUnitario = $scope.pageContentChoPD[i].precioUnitario;
                            $scope.p_Dp.unidadCaja = $scope.pageContentChoPD[i].unidadCaja;
                            $scope.p_Dp.descuento1 = $scope.pageContentChoPD[i].descuento1;
                            $scope.p_Dp.descuento2 = $scope.pageContentChoPD[i].descuento2;
                            $scope.p_Dp.iva = $scope.pageContentChoPD[i].iva;
                            $scope.p_Dp.subtotal = $scope.pageContentChoPD[i].subtotal;
                            $scope.p_Dp.total = $scope.pageContentChoPD[i].total;
                            $scope.p_Dp.cantidadxDespachar = $scope.pageContentChoPD[i].cantidadxDespachar;
                            $scope.p_Dp.cantidadDespachada = $scope.pageContentChoPD[i].cantidadDespachada;
                            $scope.p_Dp.cantidadPediente = $scope.pageContentChoPD[i].cantidadxDespachar - $scope.pageContentChoPD[i].cantidadDespachada;
                            $scope.p_Dp.extfactura = $scope.pageContentChoPD[i].extfactura;
                            $scope.pageContentChoPGloabal.push($scope.p_Dp);
                        }
                        $('#IngPedidoFactura').modal('hide');
                    } else {
                        var index = -1;
                        var comArr = eval($scope.pageContentChoPGloabal);
                        for (var i = comArr.length - 1; i >= 0; i--) {
                            if (comArr[i].numPedido === $scope.traChod.txtnumeropedido) {
                                index = i;
                                $scope.pageContentChoPGloabal.splice(index, 1);
                            }
                        }
                        if (index === -1) {
                            alert("Something gone wrong");
                        }
                        for (var i = 0; i < $scope.pageContentChoPD.length; i++) {
                            $scope.p_Dp = {};
                            $scope.p_Dp.item = $scope.pageContentChoPD[i].item;
                            $scope.p_Dp.numPedido = $scope.traChod.txtnumeropedido;
                            $scope.p_Dp.codigoProducto = $scope.pageContentChoPD[i].codigoProducto;
                            $scope.p_Dp.factura = $scope.pageContentChoPD[i].factura;
                            $scope.p_Dp.fechaFactura = $scope.pageContentChoPD[i].fechaFactura;
                            $scope.p_Dp.descripcion = $scope.pageContentChoPD[i].descripcion;
                            $scope.p_Dp.cantidadPedido = $scope.pageContentChoPD[i].cantidadPedido;
                            $scope.p_Dp.precioUnitario = $scope.pageContentChoPD[i].precioUnitario;
                            $scope.p_Dp.unidadCaja = $scope.pageContentChoPD[i].unidadCaja;
                            $scope.p_Dp.descuento1 = $scope.pageContentChoPD[i].descuento1;
                            $scope.p_Dp.descuento2 = $scope.pageContentChoPD[i].descuento2;
                            $scope.p_Dp.iva = $scope.pageContentChoPD[i].iva;
                            $scope.p_Dp.subtotal = $scope.pageContentChoPD[i].subtotal;
                            $scope.p_Dp.total = $scope.pageContentChoPD[i].total;
                            $scope.p_Dp.cantidadxDespachar = $scope.pageContentChoPD[i].cantidadxDespachar;
                            $scope.p_Dp.cantidadDespachada = $scope.pageContentChoPD[i].cantidadDespachada;
                            $scope.p_Dp.cantidadPediente = $scope.pageContentChoPD[i].cantidadxDespachar - $scope.pageContentChoPD[i].cantidadDespachada;
                            $scope.p_Dp.extfactura = $scope.pageContentChoPD[i].extfactura;
                            $scope.pageContentChoPGloabal.push($scope.p_Dp);
                        }
                        $('#IngPedidoFactura').modal('hide');
                    }
                }
            }
        }
        else {

            var baridofactura = $scope.pageContentChoPQ;
            for (var i = 0; i < baridofactura.length; i++) {
                for (var j = 0; j < $scope.pageContentChoPGloabal.length; j++) {
                    if ($scope.pageContentChoPGloabal[j].id != baridofactura[i].id) {
                        if ($scope.pageContentChoPGloabal[j].factura == baridofactura[i].factura) {
                            bandera = 2;
                            break;
                        }
                    }
                }
            }

            for (var j = 0; j < $scope.pageContentChoPQ.length; j++) {
                if ($scope.pageContentChoPQ[j].factura == "") {
                    bandera = 1;
                    break;
                }
            }
            for (var j = 0; j < $scope.pageContentChoPQ.length; j++) {
                if ($scope.pageContentChoPQ[j].factura.length < 15) {
                    bandera = 3;
                    break;
                }
            }
            if (bandera == 1000000) {
                return;
            } else {
                var griptmp = $filter('filter')($scope.pageContentChoPGloabalQ, { pedido: $scope.traChod.txtnumeropedido });
                if (griptmp.length == 0) {
                    for (var i = 0; i < $scope.pageContentChoPQ.length; i++) {
                        var auxgrp = {};
                        auxgrp.id = $scope.pageContentChoPQ[i].id;
                        auxgrp.pedido = $scope.pageContentChoPQ[i].pedido;
                        auxgrp.factura = $scope.pageContentChoPQ[i].factura;
                        auxgrp.extfactura = $scope.pageContentChoPQ[i].extfactura;
                        $scope.pageContentChoPGloabalQ.push(auxgrp);
                    }
                    $('#IngPedidoFactura2').modal('hide');
                } else {
                    var index = -1;
                    var comArr = eval($scope.pageContentChoPGloabalQ);
                    for (var i = comArr.length - 1; i >= 0; i--) {
                        if (comArr[i].pedido === $scope.traChod.txtnumeropedido) {
                            index = i;
                            $scope.pageContentChoPGloabalQ.splice(index, 1);
                        }
                    }
                    for (var i = 0; i < $scope.pageContentChoPQ.length; i++) {
                        var auxgrp = {};
                        auxgrp.id = $scope.pageContentChoPQ[i].id;
                        auxgrp.pedido = $scope.pageContentChoPQ[i].pedido;
                        auxgrp.factura = $scope.pageContentChoPQ[i].factura;
                        auxgrp.extfactura = $scope.pageContentChoPQ[i].extfactura;
                        $scope.pageContentChoPGloabalQ.push(auxgrp);
                    }
                    $('#IngPedidoFactura2').modal('hide');
                }

            }

        }

    }
    $scope.agregarItem2 = function () {
        var bandera = 0;
        for (var i = 0; i < $scope.pageContentChoPQ.length; i++) {
            if ($scope.pageContentChoPQ[i].factura == "") {
                bandera = 1;
                break;
            }
        }
        if (bandera == 0) {
            var auxgrp = {};
            auxgrp.id = parseInt($scope.pageContentChoPQ[parseInt($scope.pageContentChoPQ.length) - 1].id) + 1;
            auxgrp.pedido = $scope.traChod.txtnumeropedido;
            auxgrp.factura = "";
            auxgrp.extfactura = "0";
            $scope.pageContentChoPQ.push(auxgrp);
        }

    }
    $scope.buscarfac = function (content) {
        var bandera = 0;
        for (var i = 0; i < $scope.pageContentChoPQ.length; i++) {
            if ($scope.pageContentChoPQ[i].id != content.id) {
                if ($scope.pageContentChoPQ[i].factura == content.factura) {
                    bandera = 1;
                    break;
                }
            }
        }
        if ($scope.pageContentChoPGloabalQ.length > 0) {
            for (var i = 0; i < $scope.pageContentChoPGloabalQ.length; i++) {
                if ($scope.pageContentChoPGloabalQ[i].factura == content.factura) {
                    bandera = 1;
                    break;
                }
            }
        }
        content.extfactura = bandera;
    }
    $scope.buscarfacGP = function (content) {
        var bandera = 0;
        debugger;
        if ($scope.pageContentChoPGloabal.length > 0) {
            for (var i = 0; i < $scope.pageContentChoPGloabal.length; i++) {
                if (content.numPedido != $scope.pageContentChoPGloabal[i].numPedido) {
                    if ($scope.pageContentChoPGloabal[i].factura == content.factura) {
                        bandera = 1;
                        break;
                    }
                }
            }
        }
        content.extfactura = bandera;
    }
    $scope.eliminarGrid2 = function (id) {
        var index = -1;
        var comArr = eval($scope.pageContentChoPQ);
        for (var i = comArr.length - 1; i >= 1; i--) {
            if (comArr[i].id === id) {
                index = i;
                $scope.pageContentChoPQ.splice(index, 1);
                break;
            }
        }

    }
    $scope.EliminarRegistro = function (id, estadoconsolidacion) {
        if (estadoconsolidacion == "ACTIVO") {
            idgridp = id;
            $scope.MenjConfirmacion = "¿Esta seguro de eliminar la consolidación " + id + "?"
            $('#idMensajeConfirmacionEliminar').modal('show');
        }
        else {
            $scope.MenjError = "Solo se puede eliminar una consolidación activa"
            $('#idMensajeError').modal('show');
        }
    }
    $scope.Eliminar = function () {
        $scope.myPromise = ConsolidacionPedidosService.getEliminarConsolidacion(idgridp, $scope.traCho.codProveedor).then(function (results) {
            if (results.data) {
                $scope.MenjError = "Se eliminó la consolidación sin novedad"
                $('#idMensajeGrabar').modal('show');
                $scope.BuscarFiltro();
            }
            else {
                $scope.MenjError = "No se puedo eliminar la consolidación"
                $('#idMensajeError').modal('show');
            }
        },
                function (error) {
                    var errors = [];
                    for (var key in error.data.modelState) {
                        for (var i = 0; i < error.data.modelState[key].length; i++) {
                            errors.push(error.data.modelState[key][i]);
                        }
                    }
                    $scope.MenjError = "No se puedo eliminar la consolidación"
                    $('#idMensajeError').modal('show');
                });
    }

    $scope.modificarRegistro = function (idconsolidacion, estadoconsolidacion) {
        if (estadoconsolidacion == "ACTIVO") {
            $scope.myPromise = null;
            $scope.myPromise = ConsolidacionPedidosService.getModificarConsolidacion(idconsolidacion, $scope.traCho.codProveedor).then(function (results) {
                var retorno = {};
                retorno = results.data.root[0];
                $scope.traCho.Tipo = "2";
                $scope.traChop.txtcodigoconsolidacion = retorno[0].idconsolidacion;
                $scope.traChop.txtestado = retorno[0].estado;
                $scope.traCho.cosRapido = retorno[0].cosrapido;
                $scope.traChop.txtfechaemision = retorno[0].fechaemision;
                $scope.traChop.txtalmacendestino = retorno[0].almacendestino;
                $scope.traChop.txtcaducidad = retorno[0].fechacaducidad;
                for (var i = 0; i < $scope.BodegaDatos.length; i++) {
                    if ($scope.BodegaDatos[i].codigo == retorno[0].idalmacendestino) {
                        $scope.selectbodega = $scope.BodegaDatos[i];
                        break;
                    }
                }

                for (var i = 0; i < $scope.Choferdatos.length; i++) {
                    if ($scope.Choferdatos[i].codigo == retorno[0].idchofer) {
                        $scope.selectchofer = $scope.Choferdatos[i];
                        break;
                    }
                }
                for (var i = 0; i < $scope.Ayudantesdatos.length; i++) {
                    if ($scope.Ayudantesdatos[i].codigo == retorno[0].idayudante) {
                        $scope.selectayudante = $scope.Ayudantesdatos[i];
                        break;
                    }
                }
                for (var i = 0; i < $scope.Vehiculosdatos.length; i++) {
                    if ($scope.Vehiculosdatos[i].codigo == retorno[0].idvehiculo) {
                        $scope.selectvehiculo = $scope.Vehiculosdatos[i];
                        break;
                    }
                }

                $scope.habilitar = false;
                if (retorno[0].cosrapido == "False")
                    $scope.traCho.cosRapido = false;
                else
                    $scope.traCho.cosRapido = true;

                $scope.pageContentChoP = results.data.root[1];
                if ($scope.traCho.cosRapido == false) {
                    var auxgrpd = results.data.root[2];
                    for (var i = 0; i < auxgrpd.length; i++) {
                        $scope.p_Dp = {};
                        $scope.p_Dp.item = auxgrpd[i].item;
                        $scope.p_Dp.numPedido = auxgrpd[i].numPedido;
                        $scope.p_Dp.codigoProducto = auxgrpd[i].codigoProducto;
                        $scope.p_Dp.factura = auxgrpd[i].factura;
                        $scope.p_Dp.fechaFactura = new Date(auxgrpd[i].fechaFactura);
                        $scope.p_Dp.descripcion = auxgrpd[i].descripcion;
                        $scope.p_Dp.cantidadPedido = auxgrpd[i].cantidadPedido;
                        $scope.p_Dp.precioUnitario = auxgrpd[i].precioUnitario;
                        $scope.p_Dp.unidadCaja = auxgrpd[i].unidadCaja;
                        $scope.p_Dp.descuento1 = auxgrpd[i].descuento1;
                        $scope.p_Dp.descuento2 = auxgrpd[i].descuento2;
                        $scope.p_Dp.iva = auxgrpd[i].iva;
                        $scope.p_Dp.subtotal = auxgrpd[i].subtotal;
                        $scope.p_Dp.total = auxgrpd[i].total;
                        $scope.p_Dp.cantidadxDespachar = auxgrpd[i].cantidadxDespachar;
                        $scope.p_Dp.cantidadDespachada = auxgrpd[i].cantidadDespachada;
                        $scope.p_Dp.cantidadPediente = auxgrpd[i].cantidadPediente;
                        $scope.pageContentChoPGloabal.push($scope.p_Dp);
                    }
                }
                else {
                    var auxgrpd = results.data.root[2];
                    for (var i = 0; i < auxgrpd.length; i++) {
                        var auxgrp = {};
                        auxgrp.id = auxgrpd[i].item;
                        auxgrp.pedido = auxgrpd[i].numPedido;
                        auxgrp.factura = auxgrpd[i].factura;
                        auxgrp.extfactura = "0";
                        $scope.pageContentChoPGloabalQ.push(auxgrp);
                    }
                }
                $('.nav-tabs a[href="#RegistroConsolidación"]').tab('show');
            }, function (error) {
            });
            $scope.Consultar();
        } else {
            $scope.MenjError = "Solo se puede modificar una consolidación activa"
            $('#idMensajeError').modal('show');
        }
    }
    // Limpiar();
    setTimeout(function () { $('#consultagrid').focus(); }, 150);
    setTimeout(function () { angular.element('#consultagrid').trigger('click'); }, 250);
}
]);
//Consolidacion Pedidos controller
app.controller('ConsolidacionPedidosCrossController', ['$scope', '$location', 'ConsolidacionPedidosService', '$cookies', '$filter', 'authService', function ($scope, $location, ConsolidacionPedidosService, $cookies, $filter, authService) {
    var dateFormat = function () {
        var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
            timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
            timezoneClip = /[^-+\dA-Z]/g,
            pad = function (val, len) {
                val = String(val);
                len = len || 2;
                while (val.length < len) val = "0" + val;
                return val;
            };

        return function (date, mask, utc) {
            var dF = dateFormat;


            if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }


            date = date ? new Date(date) : new Date;
            if (isNaN(date)) throw SyntaxError("invalid date");

            mask = String(dF.masks[mask] || mask || dF.masks["default"]);


            if (mask.slice(0, 4) == "UTC:") {
                mask = mask.slice(4);
                utc = true;
            }

            var _ = utc ? "getUTC" : "get",
                d = date[_ + "Date"](),
                D = date[_ + "Day"](),
                m = date[_ + "Month"](),
                y = date[_ + "FullYear"](),
                H = date[_ + "Hours"](),
                M = date[_ + "Minutes"](),
                s = date[_ + "Seconds"](),
                L = date[_ + "Milliseconds"](),
                o = utc ? 0 : date.getTimezoneOffset(),
                flags = {
                    d: d,
                    dd: pad(d),
                    ddd: dF.i18n.dayNames[D],
                    dddd: dF.i18n.dayNames[D + 7],
                    m: m + 1,
                    mm: pad(m + 1),
                    mmm: dF.i18n.monthNames[m],
                    mmmm: dF.i18n.monthNames[m + 12],
                    yy: String(y).slice(2),
                    yyyy: y,
                    h: H % 12 || 12,
                    hh: pad(H % 12 || 12),
                    H: H,
                    HH: pad(H),
                    M: M,
                    MM: pad(M),
                    s: s,
                    ss: pad(s),
                    l: pad(L, 3),
                    L: pad(L > 99 ? Math.round(L / 10) : L),
                    t: H < 12 ? "a" : "p",
                    tt: H < 12 ? "am" : "pm",
                    T: H < 12 ? "A" : "P",
                    TT: H < 12 ? "AM" : "PM",
                    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
                };

            return mask.replace(token, function ($0) {
                return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
            });
        };
    }();


    dateFormat.masks = {
        "default": "ddd mmm dd yyyy HH:MM:ss",
        shortDate: "m/d/yy",
        mediumDate: "mmm d, yyyy",
        longDate: "mmmm d, yyyy",
        fullDate: "dddd, mmmm d, yyyy",
        shortTime: "h:MM TT",
        mediumTime: "h:MM:ss TT",
        longTime: "h:MM:ss TT Z",
        isoDate: "yyyy-mm-dd",
        isoTime: "HH:MM:ss",
        isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
        isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
    };


    dateFormat.i18n = {
        dayNames: [
            "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        ],
        monthNames: [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
        ]
    };


    Date.prototype.format = function (mask, utc) {
        return dateFormat(this, mask, utc);
    };
    $scope.formData = {};
    $scope.etiTotRegistros = "";
    $scope.etiTotRegistrosLP = "";

    $scope.div1 = false;
    $scope.div2 = true;
    $scope.imagenurl = "";
    //Variable de Grid
    $scope.GridConsolidacion = [];
    var _GridConsolidacion = [];
    $scope.pagesCo = [];
    $scope.pageContentCo = [];

    $scope.sortTypeCon = 'idconsolidacion';
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.GridTransporteP = [];
    var _GridTransporteP = [];
    $scope.pagesChoP = [];

    var idgridp = 0;

    //fin Grid

    //Variable de Busquedas
    $scope.PorFechas = 1;
    $scope.fechahabilitar = true;
    $scope.txtfechadesde = "";
    $scope.txtfechahasta = "";
    $scope.PorNumero = 1;
    $scope.txtnumero = "";
    $scope.Portodosfacturas = true;

    $scope.EstadoSolicitud = [];
    $scope.selectedItemEstado = [];
    $scope.SettingEstadoSol = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };
    //Fin Variable


    //Combos Formularios
    $scope.Vehiculosdatos = [];
    $scope.selectvehiculo = "";
    $scope.Choferdatos = [];
    $scope.selectchofer = "";
    $scope.Ayudantesdatos = [];
    $scope.selectayudante = "";

    $scope.selectbodega = "";
    $scope.BodegaDatos = [];

    $scope.habilitar = true;
    //Fin Combos


    //Accion a realizar en modal de confirmacion
    $scope.accion = 0;
    $scope.estadoCross = false;
    $scope.estadoCrossG = false;
    //Cabecera de datos de pedidos Cross
    $scope.datosDistribucion = [];
    $scope.datosTDetalleCross = [];
    $scope.datosDetalleCross = [];
    $scope.datosDetalleCrossHis = [];
    $scope.CabCross = {};
    $scope.CabCross.codProveedor = "";
    $scope.CabCross.nomProveedor = "";
    $scope.CabCross.numPedido = "";

    //Detalle de datos de pedidos Cross
    $scope.DetCross = {};
    $scope.DetCross.codArticulo = "";
    $scope.DetCross.desArticulo = "";
    $scope.DetCross.cantPlanificada = "";
    $scope.DetCross.uniPlanificada = "";
    $scope.DetCross.cantReal = "";
    $scope.DetCross.uniReal = "";

    //Cabecera Distribucion Tiendas
    $scope.CabTiendas = {};
    $scope.CabTiendas.codArticulo = "";
    $scope.CabTiendas.desArticulo = "";
    $scope.CabTiendas.canPlanificada = "";
    $scope.CabTiendas.uniPlanificada = "";






    //Variable de pedido Factura
    $scope.traCho = {};
    $scope.traCho.idconsolidacion = "";
    $scope.traCho.Tipo = "1";
    $scope.traCho.codChofer = "";
    $scope.traCho.codAyudante = "";
    $scope.traCho.codVehiculo = "";
    $scope.traCho.codAlmacenDestino = "";
    $scope.traCho.codProveedor = authService.authentication.CodSAP;
    $scope.traCho.usuarioCreacion = authService.authentication.userName;
    $scope.traCho.cosRapido = false;
    $scope.pageContentChoP = [];
    $scope.pageContentChoPD = [];
    $scope.pageContentChoPGloabal = [];
    $scope.pageContentChoPQ = [];
    $scope.pageContentChoPGloabalQ = [];
    //fin Variable de pedido Factura

    $scope.catDistibui = "";
    $scope.catDistibuiped = "";



    $scope.traChop = {};
    $scope.traChop.txtcodigoconsolidacion = "";
    $scope.traChop.txtestado = "";
    $scope.traChop.txtfechaemision = "";
    $scope.traChop.txtalmacendestino = "";
    $scope.traChop.txtcaducidad = "";


    $scope.traChod = {};
    $scope.traChod.txtnumeropedido


    $scope.sortType = 'numplaca'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';

    //Fin Variable

    //Variable de Mensaje
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    //Fin Variable


    $scope.fecha = {};
    $scope.fecha.txtfechadesde = "";
    $scope.fecha.txtfechahasta = "";


    //Carga Catalogo Estado
    ConsolidacionPedidosService.getCatalogo('tbl_EstadoGeneral').then(function (results) {
        $scope.EstadoSolicitud = results.data;
    }, function (error) {
    });



    //Carga Vehiculo
    ConsolidacionPedidosService.getChoferVehiculos('2', $scope.traCho.codProveedor).then(function (results) {
        $scope.Vehiculosdatos = results.data.root[0];

    }, function (error) {
    });

    //Carga Chofer
    ConsolidacionPedidosService.getChoferVehiculos('1', $scope.traCho.codProveedor).then(function (results) {
        $scope.Choferdatos = results.data.root[0];
    }, function (error) {
    });

    //Carga Ayudate
    ConsolidacionPedidosService.getChoferVehiculos('1', $scope.traCho.codProveedor).then(function (results) {
        $scope.Ayudantesdatos = results.data.root[0];
    }, function (error) {
    });

    //Carga Bodega
    ConsolidacionPedidosService.getChoferVehiculos('6', $scope.traCho.codProveedor).then(function (results) {
        $scope.BodegaDatos = results.data.root[0];
    }, function (error) {
    });

    $scope.SelecionaComboNUEVO = function () {
        if ($scope.selectayudante != undefined && $scope.selectchofer != undefined) {
            if ($scope.selectayudante.codigo == $scope.selectchofer.codigo) {
                $scope.MenjError = "El chofer y el ayudante no pueden ser iguales"
                $('#idMensajeError').modal('show');
            }
        } else {
            $scope.MenjError = "Debe Selecionar un chofer y un ayudante"
            $('#idMensajeError').modal('show');
        }


    }



    $scope.getthefile = function () {
        $scope.myPromise = ConsolidacionPedidosService.getthefile().then(function (results) {

            $scope.imagenurl = results.data;
        }, function (error) {
        });
    }
    $scope.limpioCaja = function () {
        if ($scope.PorFechas === "1") {
            $scope.fecha.txtfechadesde = "";
            $scope.fecha.txtfechahasta = "";
            $scope.fechahabilitar = true;
        } else {
            var dateString = new Date();
            var d1 = dateString.format("dd/mm/yyyy");

            $scope.fecha.txtfechadesde = d1;
            $scope.fecha.txtfechahasta = d1;

            $scope.fechahabilitar = false;

        }
        if ($scope.PorEstadosTipo === "1") {
            $scope.selectedItemEstado = "";
        }

        if ($scope.PorNumero === "1") {
            $scope.txtnumero = "";
        }
    }

    function Limpiar() {
        $scope.selectvehiculo = "";
        $scope.selectchofer = "";
        $scope.selectayudante = "";
        $scope.selectbodega = "";
        $scope.cargarPedido();
        $scope.traCho.Tipo = "1";
        $scope.pageContentChoP = [];
        $scope.pageContentChoPGloabal = [];
        $scope.pageContentChoPQ = [];
        $scope.pageContentChoPGloabalQ = [];
        $scope.traCho.cosRapido = false;
        $scope.habilitar = false;

        $('#DetallePedidoCross').modal('hide');

        $('#DetallePedidoCrossFlow').modal('hide');

        $('#DetalleTiendas').modal('hide');
        $('#DetalleTiendasFlow').modal('hide');
    }
    function LimpiarGrabar() {
        $scope.selectvehiculo = "";
        $scope.selectchofer = "";
        $scope.selectayudante = "";
        $scope.selectbodega = "";
        $scope.pageContentChoP = [];
        $scope.pageContentChoPGloabal = [];
        $scope.pageContentChoPQ = [];
        $scope.pageContentChoPGloabalQ = [];
        $scope.traCho.cosRapido = false;
        $scope.habilitar = true;
        $scope.traChop.txtcodigoconsolidacion = "";
        $scope.traChop.txtestado = "";
        $scope.traChop.txtfechaemision = "";
        $scope.traChop.txtalmacendestino = "";
        $scope.traChop.txtcaducidad = "";
    }
    $scope.cargarPedido = function () {
        $scope.myPromise = null;
        $scope.myPromise = ConsolidacionPedidosService.getBuscarPedidos("3", $scope.traCho.codProveedor).then(function (results) {
            $scope.pageContentChoP = results.data.root[0];
        }, function (error) {
        });
    }
    $scope.nuevo = function () {

        Limpiar();
    }

    $scope.cancelar = function () {
        $scope.selectvehiculo = "";
        $scope.selectchofer = "";
        $scope.selectayudante = "";
        $scope.selectbodega = "";
        $scope.pageContentChoP = [];
        $scope.pageContentChoPGloabal = [];
        $scope.traChop.txtcodigoconsolidacion = "";
        $scope.traChop.txtestado = "";
        $scope.traCho.cosRapido = "";
        $scope.traChop.txtfechaemision = "";
        $scope.traChop.txtalmacendestino = "";
        $scope.traChop.txtcaducidad = "";
        $scope.habilitar = true;
    }
    $scope.cambio = function () {
        if ($scope.Portodosfacturas == true) {
            for (var i = 0; i < $scope.pageContentChoPD.length; i++) {
                $scope.pageContentChoPD[i].cantidadDespachada = $scope.pageContentChoPD[i].cantidadxDespachar;
                $scope.pageContentChoPD[i].cantidadPediente = $scope.pageContentChoPD[i].cantidadxDespachar - $scope.pageContentChoPD[i].cantidadDespachada;
            }
        } else {
            for (var i = 0; i < $scope.pageContentChoPD.length; i++) {
                $scope.pageContentChoPD[i].cantidadDespachada = 0;
                $scope.pageContentChoPD[i].cantidadPediente = $scope.pageContentChoPD[i].cantidadxDespachar;
            }
        }
    }
    $scope.BuscarFiltro = function () {
        $scope.Consultar();
    }

    $scope.Confirmargrabar = function () {
        if ($scope.traCho.Tipo === "1") {
            $scope.MenjConfirmacion = "¿Está seguro de guardar la información?"
        }
        if ($scope.traCho.Tipo === "2") {
            $scope.MenjConfirmacion = "¿Está seguro de modificar la información?"
        }
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.cancelarDetTienda = function () {
        for (var idx = 0; idx < $scope.datosDistribucion.length; idx++) {
            var update = $scope.datosDistribucion[idx];
            update.pCantidadProveedor = update.pCantidadRealDistribucion;
        }
        $('#DetalleTiendas').modal('hide');
        $('#DetalleTiendasFlow').modal('hide');
    }


    $scope.confirmacionFinDistribucion = function () {
        $scope.accion = 999;
        $scope.MenjConfirmacion = "Si finaliza la distribución el sistema no le permitirá realizar más cambios."
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.verDetalleHistorico = function () {


        $scope.myPromise = ConsolidacionPedidosService.getBuscarDetPedidosCrossHis($scope.CabCross.numPedido, authService.authentication.CodSAP, "0").then(function (results) {
            if (results.data.success) {
                $scope.datosDetalleCrossHis = results.data.root[0];
                $('#DetallePedidoCrossHis').modal('show');
            }
            else {
                $scope.MenjError = results.data.msgError;
                $('#idMensajeError').modal('show');
            }

        }, function (error) {

            $scope.MenjError = "Error en comunicación: getBuscarDetPedidosCrossHis().";
            $('#idMensajeError').modal('show');

        });
    }

    $scope.finalizarDistribucion = function () {
        //Finalizar distribucion
        if ($scope.datosDetalleCross[0].tipoPedidos == "2") {
            $scope.myPromise = ConsolidacionPedidosService.getActualizaPedidoCross($scope.datosTDetalleCross, "1", authService.authentication.userName).then(function (results) {
                if (results.data.success) {
                    $scope.MenjError = "Distribución finalizada correctamente, puede generar reportes del detalle de la distribución. ";
                    $('#idMensajeOk').modal('show');
                    Limpiar();
                }
                else {
                    $scope.MenjError = results.data.msgError;
                    $('#idMensajeError').modal('show');
                }

            }, function (error) {

                $scope.MenjError = "Error en comunicación: getActualizaPedidoCross().";
                $('#idMensajeError').modal('show');

            });
        }
        if ($scope.datosDetalleCross[0].tipoPedidos == "3") {
            for (var idx = 0; idx < $scope.datosDetalleCross.length; idx++) {

                var update = $scope.datosDetalleCross[idx];
                var montoMinimo = Math.round(update.cantPlanificada * update.pTolerancia / 100);
                var porcentajeTolerancia = update.pTolerancia;
                if (String(update.cantReal) == "") {
                    $scope.MenjError = "Ingrese cantidad a distribuir en artículo " + update.desArticulo;
                    $('#idMensajeInformativo').modal('show');
                    return;
                }
                var cantNumerico = parseInt(update.cantReal)
                if (cantNumerico < 0) {
                    $scope.MenjError = "Ingrese una cantidad mayor a 0 en artículo " + update.desArticulo;
                    $('#idMensajeInformativo').modal('show');
                    return;
                }
                if (cantNumerico < montoMinimo) {
                    $scope.MenjError = "Cantidad no puede ser menor a " + montoMinimo + " (correspondiente al " + porcentajeTolerancia + "% de la cantidad planificada) en artículo " + update.desArticulo;
                    $('#idMensajeInformativo').modal('show');
                    return;
                }

                if (cantNumerico > update.cantPlanificada) {
                    $scope.MenjError = "Cantidad no puede ser mayor a Cantidad Planificada en almacén " + update.desArticulo;
                    $('#idMensajeInformativo').modal('show');
                    return;
                }


            }
            $scope.myPromise = ConsolidacionPedidosService.getActualizaPedidoCrossFlow($scope.datosDetalleCross, "1", authService.authentication.userName).then(function (results) {
                if (results.data.success) {
                    $scope.MenjError = "Modificación de la Orden de Compra finalizada, en espera del cambio de la distribución por parte de la compradora.";
                    $('#idMensajeOk').modal('show');
                    Limpiar();
                }
                else {
                    $scope.MenjError = results.data.msgError;
                    $('#idMensajeInformativo').modal('show');
                    Limpiar();
                }

            }, function (error) {

                $scope.MenjError = "Error en comunicación: getActualizaPedidoCross().";
                $('#idMensajeError').modal('show');

            });
        }
    }

    $scope.grabarDetTienda = function () {
        var newCantidadReal = 0;
        for (var idx = 0; idx < $scope.datosDistribucion.length; idx++) {

            var update = $scope.datosDistribucion[idx];
            var montoMinimo = Math.round(update.pCantidadDistribucion * update.pTolerancia / 100);
            var porcentajeTolerancia = update.pTolerancia;
            if (String(update.pCantidadProveedor) == "") {
                $scope.MenjError = "Ingrese cantidad a distribuir en almacén " + update.pNomAlmacen;
                $('#idMensajeInformativo').modal('show');
                return;
            }
            var cantNumerico = parseInt(update.pCantidadProveedor)
            if (cantNumerico < 0) {
                $scope.MenjError = "Ingrese una cantidad mayor a 0 en almacén " + update.pNomAlmacen;
                $('#idMensajeInformativo').modal('show');
                return;
            }
            if (cantNumerico < montoMinimo) {
                $scope.MenjError = "Cantidad no puede ser menor a " + montoMinimo + " (correspondiente al " + porcentajeTolerancia + "% de la cantidad planificada) en almacén " + update.pNomAlmacen;
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if (cantNumerico > update.pCantidadDistribucion) {
                $scope.MenjError = "Cantidad no puede ser mayor a Cantidad Planificada en almacén " + update.pNomAlmacen;
                $('#idMensajeInformativo').modal('show');
                return;
            }


            update.pCantidadRealDistribucion = parseInt(update.pCantidadProveedor);
            update.pCantidadProveedor = update.pCantidadRealDistribucion;
            newCantidadReal = newCantidadReal + update.pCantidadRealDistribucion;

        }

        //Grabar datos

        $scope.myPromise = ConsolidacionPedidosService.getActualizaPedidoCross($scope.datosDistribucion, "0", authService.authentication.userName).then(function (results) {
            if (results.data.success) {
                //Actualizar totales por artículos
                var CodArtBusqueda = $scope.CabTiendas.codArticulo;
                var updateCons = $filter('filter')($scope.datosDetalleCross, { codArticulo: CodArtBusqueda }, true)[0];
                updateCons.cantReal = newCantidadReal;
                $('#DetalleTiendas').modal('hide');
            }
            else {
                $scope.MenjError = results.data.msgError;
                $('#idMensajeError').modal('show');
            }

        }, function (error) {

            $scope.MenjError = "Error en comunicación: getActualizaPedidoCross().";
            $('#idMensajeError').modal('show');

        });



    }

    $scope.grabarDetTiendaFlow = function () {
        debugger;
        var newCantidadReal = 0;
        for (var idx = 0; idx < $scope.datosDetalleCross.length; idx++) {

            var update = $scope.datosDetalleCross[idx];
            var montoMinimo = Math.round(update.cantPlanificada * update.pTolerancia / 100);
            var porcentajeTolerancia = update.pTolerancia;
            if (String(update.cantReal) == "") {
                $scope.MenjError = "Ingrese cantidad a distribuir en artículo " + update.desArticulo;
                $('#idMensajeInformativo').modal('show');
                return;
            }
            var cantNumerico = parseInt(update.cantReal)
            if (cantNumerico < 0) {
                $scope.MenjError = "Ingrese una cantidad mayor a 0 en artículo " + update.desArticulo;
                $('#idMensajeInformativo').modal('show');
                return;
            }
            if (cantNumerico < montoMinimo) {
                $scope.MenjError = "Cantidad no puede ser menor a " + montoMinimo + " (correspondiente al " + porcentajeTolerancia + "% de la cantidad planificada) en artículo " + update.desArticulo;
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if (cantNumerico > update.cantPlanificada) {
                $scope.MenjError = "Cantidad no puede ser mayor a Cantidad Planificada en almacén " + update.desArticulo;
                $('#idMensajeInformativo').modal('show');
                return;
            }


        }

        //Grabar datos
        $scope.myPromise = ConsolidacionPedidosService.getActualizaPedidoCrossFlow($scope.datosDetalleCross, "0", authService.authentication.userName).then(function (results) {
            debugger;
            if (results.data.success) {
                debugger;
                //Actualizar totales por artículos
                var CodArtBusqueda = $scope.CabTiendas.codArticulo;
                var updateCons = $filter('filter')($scope.datosDetalleCross, { codArticulo: CodArtBusqueda }, true)[0];
                $('#DetallePedidoCrossFlow').modal('hide');
            }
            else {
                $scope.MenjError = results.data.msgError;
                $('#idMensajeError').modal('show');
            }

        }, function (error) {

            $scope.MenjError = "Error en comunicación: getActualizaPedidoCross().";
            $('#idMensajeError').modal('show');

        });



    }

    $scope.verDetalleTiendas = function (codArticulo, desArticulo, canPlanificada, uniPlanificada, tipoPedidos) {

        $scope.datosDistribucion = [];
        $scope.datosDistribucion = $filter('filter')($scope.datosTDetalleCross, { pCodArticulo: codArticulo }, true);
        $scope.catDistibui = "";
        $scope.catDistibui = $scope.datosDistribucion.length

        $scope.CabTiendas = {};
        $scope.CabTiendas.codArticulo = codArticulo;
        $scope.CabTiendas.desArticulo = desArticulo;
        $scope.CabTiendas.canPlanificada = canPlanificada;
        $scope.CabTiendas.uniPlanificada = uniPlanificada;


        if (tipoPedidos=="2") {
            $('#DetalleTiendas').modal('show');
        }
        
        if (tipoPedidos == "3") {
            $('#DetalleTiendasFlow').modal('show');
        }
    }

    $scope.DistribucionCross = function (registro) {
        $scope.CabCross.codProveedor = authService.authentication.CodSAP;
        $scope.CabCross.nomProveedor = authService.authentication.nomEmpresa;
        $scope.CabCross.numPedido = registro.numPedido;

        //Consulta de detalle del pedido cross

        if (registro.tipoPedido!="") {

        
            $scope.myPromise = ConsolidacionPedidosService.getBuscarDetPedidosCrossFlow(registro.numPedido, authService.authentication.CodSAP, registro.tipoPedidos).then(function (results) {

                $scope.datosDetalleCross = [];
                $scope.datosTDetalleCross = [];
                if (results.data.success) {
                    var listaRegistros = results.data.root[0];
                    $scope.datosTDetalleCross = listaRegistros;
                    var existe;
                    for (var i = 0 ; i < listaRegistros.length; i++) {
                        if (listaRegistros[i].pEstado == "1" || listaRegistros[i].pEstado == "3" || listaRegistros[i].pEstado == "4") {
                            $scope.estadoCross = true;
                            $scope.estadoCrossG = true;
                        }
                        else
                        {
                            $scope.estadoCross = false;
                            $scope.estadoCrossG = false;
                        }
                        debugger;
                        if (listaRegistros[i].pTolerancia==100) {
                            $scope.estadoCrossG = true;
                        }
                        
                        var CodArticuloBus = listaRegistros[i].pCodArticulo;
                        if ($scope.datosDetalleCross.length > 0) {
                            existe = $filter('filter')($scope.datosDetalleCross, { codArticulo: CodArticuloBus }, true)[0];
                        }

                        if (existe != undefined) {
                            existe.cantPlanificada = existe.cantPlanificada + listaRegistros[i].pCantidadDistribucion;
                            if (registro.tipoPedidos == "2") {
                                existe.cantReal = existe.cantReal + listaRegistros[i].pCantidadRealDistribucion;
                            } 
                            if (registro.tipoPedidos == "3") {
                                if (listaRegistros[i].real == "0") {
                                    $scope.DetCross.cantReal = existe.cantPlanificada;
                                } else {
                                    existe.cantReal = listaRegistros[i].real;
                                }

                            }
                        }
                        else {
                            $scope.DetCross = {};
                            $scope.DetCross.codArticulo = listaRegistros[i].pCodArticulo;
                            $scope.DetCross.desArticulo = listaRegistros[i].pDesArticulo;
                            $scope.DetCross.cantPlanificada = listaRegistros[i].pCantidadDistribucion;
                            $scope.DetCross.uniPlanificada = listaRegistros[i].pUnidadDistribucion;
                            if (registro.tipoPedidos == "2") {
                                $scope.DetCross.cantReal = listaRegistros[i].pCantidadRealDistribucion;
                            }
                            if (registro.tipoPedidos == "3") {
                            

                                if (listaRegistros[i].real == "0") {
                                    $scope.DetCross.cantReal = $scope.DetCross.cantPlanificada;
                                } else {
                                    $scope.DetCross.cantReal = listaRegistros[i].real;
                                }

                            }
                            $scope.DetCross.uniReal = listaRegistros[i].pUnidadRealDistribucion;
                            $scope.DetCross.pTolerancia = listaRegistros[i].pTolerancia;
                            $scope.DetCross.tipoPedidos = registro.tipoPedidos;
                            $scope.DetCross.pIdPedido = listaRegistros[i].pIdPedido;
                        
                            $scope.datosDetalleCross.push($scope.DetCross);

                        }
                    }
                    $scope.catDistibuiped = "";
                    $scope.catDistibuiped = $scope.datosDetalleCross.length;

                }
                else {
                    $scope.MenjError = results.data.msgError;
                    $('#idMensajeError').modal('show');
                }

            }, function (error) {

                $scope.MenjError = "Error en comunicación: getBuscarDetPedidosCross().";
                $('#idMensajeError').modal('show');

            });


        
            if (registro.tipoPedidos=="2") {
                $('#DetallePedidoCross').modal('show');
            }
            if (registro.tipoPedidos == "3") {
                $('#DetallePedidoCrossFlow').modal('show');
            }
        }
    }

    //Boton aceptar de modal OK
    $("#btnMensajeOK").click(function () {

        $('#DetallePedidoCross').modal('hide');
    });

    //Boton Cancelar
    $("#btnCancelar").click(function () {
        $scope.accion = 0;

    });




    $scope.grabar = function () {
        if ($scope.accion == 999) {


            $scope.accion = 0;
            $scope.finalizarDistribucion();
        }
        else {
            if ($scope.traCho.Tipo === "1") {
                $scope.traCho.codChofer = $scope.selectchofer.codigo;
                $scope.traCho.codAyudante = $scope.selectayudante.codigo;
                $scope.traCho.codVehiculo = $scope.selectvehiculo.codigo;
                $scope.traCho.codAlmacenDestino = $scope.selectbodega.codigo;
                var gripp = $filter('filter')($scope.pageContentChoP, { chkpedido: true });
                var grippt = [];

                if (gripp.length == 0) {
                    $scope.MenjError = "Se debe seleccionar al menos un pedido."
                    $('#idMensajeError').modal('show');
                    return;
                }
                if ($scope.traCho.cosRapido == 0) {
                    for (var i = 0; i < $scope.pageContentChoPGloabal.length; i++) {
                        if ($scope.pageContentChoPGloabal[i].cantidadxDespachar != "0")
                            grippt.push($scope.pageContentChoPGloabal[i]);
                    }
                } else {
                    for (var i = 0; i < $scope.pageContentChoPGloabalQ.length; i++) {
                        var grid = {};
                        grid.item = $scope.pageContentChoPGloabalQ[i].id;
                        grid.numPedido = $scope.pageContentChoPGloabalQ[i].pedido;
                        grid.codigoProducto = $scope.pageContentChoPGloabalQ[i].id;
                        grid.factura = $scope.pageContentChoPGloabalQ[i].factura;
                        grid.fechaFactura = "";
                        grid.descripcion = "";
                        grid.cantidadPedido = "0";
                        grid.precioUnitario = "0";
                        grid.unidadCaja = "0";
                        grid.descuento1 = "0";
                        grid.descuento2 = "0";
                        grid.iva = "0";
                        grid.subtotal = "0";
                        grid.total = "0";
                        grid.cantidadxDespachar = "0";
                        grid.cantidadDespachada = "0";
                        grid.cantidadPediente = "0";
                        grippt.push(grid);
                    }
                }
                if (grippt.length == 0) {
                    $scope.MenjError = "Se debe tener por lo menos un detalle de un pedido."
                    $('#idMensajeError').modal('show');
                    return;
                }

                $scope.myPromise = null;
                $scope.myPromise = ConsolidacionPedidosService.getGrabaConsolidacion($scope.traCho, gripp, grippt).then(function (results) {
                    if (results.data) {
                        if ($scope.traCho.Tipo === "1") {
                            $scope.MenjError = "Se ingreso la consolidación correctamente"
                            $('#idMensajeGrabar').modal('show');
                        }
                    }
                    else {
                        if (results.data.msgError === "EXISTE") {
                            if ($scope.traCho.Tipo === "1") {
                                $scope.MenjError = "La consolidación ya exite verifique"
                                $('#idMensajeError').modal('show');
                            }
                        } else {
                            $scope.MenjError = "No se puedo grabar el registro"
                            $('#idMensajeError').modal('show');
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
                  $scope.MenjError = "No se puedo grabar el registro"
                  $('#idMensajeError').modal('show');
              });
            } else {
                $scope.traCho.idconsolidacion = $scope.traChop.txtcodigoconsolidacion;
                $scope.traCho.codChofer = $scope.selectchofer.codigo;
                $scope.traCho.codAyudante = $scope.selectayudante.codigo;
                $scope.traCho.codVehiculo = $scope.selectvehiculo.codigo;
                $scope.traCho.codAlmacenDestino = $scope.selectbodega.codigo;
                var gripp = $filter('filter')($scope.pageContentChoP, { chkpedido: true });
                var grippt = [];
                if ($scope.traCho.cosRapido == 0) {
                    for (var i = 0; i < $scope.pageContentChoPGloabal.length; i++) {
                        if ($scope.pageContentChoPGloabal[i].cantidadxDespachar != "0")
                            grippt.push($scope.pageContentChoPGloabal[i]);
                    }
                } else {
                    for (var i = 0; i < $scope.pageContentChoPGloabalQ.length; i++) {
                        var grid = {};
                        grid.item = $scope.pageContentChoPGloabalQ[i].id;
                        grid.numPedido = $scope.pageContentChoPGloabalQ[i].pedido;
                        grid.codigoProducto = $scope.pageContentChoPGloabalQ[i].id;
                        grid.factura = $scope.pageContentChoPGloabalQ[i].factura;
                        grid.fechaFactura = "";
                        grid.descripcion = "";
                        grid.cantidadPedido = "0";
                        grid.precioUnitario = "0";
                        grid.unidadCaja = "0";
                        grid.descuento1 = "0";
                        grid.descuento2 = "0";
                        grid.iva = "0";
                        grid.subtotal = "0";
                        grid.total = "0";
                        grid.cantidadxDespachar = "0";
                        grid.cantidadDespachada = "0";
                        grid.cantidadPediente = "0";
                        grippt.push(grid);
                    }
                }
                $scope.myPromise = null;
                $scope.myPromise = ConsolidacionPedidosService.getGrabaConsolidacion($scope.traCho, gripp, grippt).then(function (results) {
                    if (results.data) {
                        if ($scope.traCho.Tipo === "2") {
                            $scope.MenjError = "Se modificó la consolidación correctamente"
                            $('#idMensajeGrabar').modal('show');
                        }
                    }
                    else {
                        if (results.data.msgError === "EXISTE") {
                            if ($scope.traCho.Tipo === "2") {
                                $scope.MenjError = "La consolidación ya exite verifique"
                                $('#idMensajeError').modal('show');
                            }
                        } else {
                            $scope.MenjError = "No se puedo grabar el registro"
                            $('#idMensajeError').modal('show');
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
                  $scope.MenjError = "No se puedo grabar el registro"
                  $('#idMensajeError').modal('show');
              });


            }
        }
    }
    $scope.okGrabar = function () {
        LimpiarGrabar();

    }

    $scope.Consultar = function () {
        var index;
        var estados = new Array();
        var a = $scope.selectedItemEstado;
        for (index = 0; index < a.length; ++index) {
            estados[index] = a[index].id;
        }
        var a1 = "";
        var a2 = "";
        if ($scope.fecha.txtfechadesde != "") {
            var parts1 = $scope.fecha.txtfechadesde.split('/');

            a1 = new Date(parts1[2], parts1[1] - 1, parts1[0]);

        }

        if ($scope.fecha.txtfechahasta != "") {
            var parts2 = $scope.fecha.txtfechahasta.split('/');

            a2 = new Date(parts2[2], parts2[1] - 1, parts2[0]);

        }

        var fecha1 = $filter('date')(a1, 'yyyy-MM-dd');
        var fecha2 = $filter('date')(a2, 'yyyy-MM-dd');



        $scope.myPromise = null;
        $scope.etiTotRegistros = "";
        $scope.myPromise = ConsolidacionPedidosService.getBuscarConsolidacion($scope.txtnumero, $scope.traCho.codProveedor, estados, fecha1, fecha2).then(function (results) {
            $scope.GridConsolidacion = results.data.root[0];
            if (results.data.msgError == "NO") {
                $scope.etiTotRegistros = "0";
            }
            else {
                $scope.etiTotRegistros = $scope.GridConsolidacion.length.toString();
            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
        }, function (error) {
        });
    }


    $scope.ConsultarDespues = function () {

        if ($scope.PorNumeroCed === "2") {
            if ($scope.txtNumero === "") {
                $scope.MenjError = "Debe ingresar cedula o ruc a consultar"
                $('#idMensajeError').modal('show');
                return;
            } else {
                if (isNaN($scope.txtNumero)) {
                    $scope.MenjError = "Este campo debe tener sólo números."
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
        }

        if ($scope.PorEstadosTipo === "2") {
            if ($scope.selectedItemTipo === "") {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.selectedItemTipo === null) {
                $scope.MenjError = "Debe selecionar un estado a consultar"
                $('#idMensajeError').modal('show');
                return;
            }
        }

        $scope.etiTotRegistros = "";
        $scope.myPromise = null;
        $scope.myPromise = ConsolidacionPedidosService.getConsulaGrid("1", $scope.txtNumero, $scope.txtPorNombre, $scope.txtPorApellido, $scope.selectedItemTipo.codigo, $scope.traCho.CodProveedor).then(function (results) {
            if (results.data.success) {
                $scope.pageContentCho = results.data.root[0];
                $scope.etiTotRegistros = $scope.pageContentCho.length.toString();
            }
        }, function (error) {
        });
        setTimeout(function () { $('#consultagrid').focus(); }, 10);
    }
    $scope.modi = function () {
        $scope.div1 = false;
        $scope.div2 = true;
    }

    $scope.calculo = function (content) {
        content.cantidadPediente = content.cantidadxDespachar - content.cantidadDespachada;
    }
    $scope.exportar = function (tipo) {
        if ($scope.GridConsolidacion.length == 0) {
            $scope.MenjError = "No hay datos para generar reporte"
            $('#idMensajeGrabar').modal('show');
            return;
        }
        $scope.myPromise = null;
        $scope.Tablacabecera = {};
        $scope.Tablacabecera.tipo = tipo;
        $scope.Tablacabecera.usuario = $scope.traCho.usuarioCreacion;
        $scope.myPromise = ConsolidacionPedidosService.getExportarDataConsolidacion($scope.Tablacabecera, $scope.GridConsolidacion).then(function (results) {
            if (results.data != "") {
                window.open(results.data, '_blank', "");
            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
        }, function (error) {
        });

    }
    $scope.SelecionarGrid = function (chkpedido, numPedido) {
        if (chkpedido == false) {
            $scope.MenjError = "Debe selecionar el registro que desea ingresar datos de pedidos"
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.traCho.cosRapido == false) {
            $scope.traChod.txtnumeropedido = numPedido;

            $scope.Portodosfacturas = false;
            $scope.pageContentChoPD = [];
            $scope.myPromise = null;
            $scope.myPromise = ConsolidacionPedidosService.getBuscarDetPedidos("4", numPedido).then(function (results) {
                var aux = $filter('filter')($scope.pageContentChoPGloabal, { numPedido: $scope.traChod.txtnumeropedido });
                if (aux.length != 0) {
                    for (var i = 0; i < aux.length; i++) {
                        $scope.p_Dp = {};
                        $scope.p_Dp.item = aux[i].item;
                        $scope.p_Dp.numPedido = aux[i].numPedido;
                        $scope.p_Dp.codigoProducto = aux[i].codigoProducto;
                        $scope.p_Dp.factura = aux[i].factura;
                        $scope.p_Dp.fechaFactura = new Date(aux[i].fechaFactura);
                        $scope.p_Dp.descripcion = aux[i].descripcion;
                        $scope.p_Dp.cantidadPedido = aux[i].cantidadPedido;
                        $scope.p_Dp.precioUnitario = aux[i].precioUnitario;
                        $scope.p_Dp.unidadCaja = aux[i].unidadCaja;
                        $scope.p_Dp.descuento1 = aux[i].descuento1;
                        $scope.p_Dp.descuento2 = aux[i].descuento2;
                        $scope.p_Dp.iva = aux[i].iva;
                        $scope.p_Dp.subtotal = aux[i].subtotal;
                        $scope.p_Dp.total = aux[i].total;
                        $scope.p_Dp.cantidadxDespachar = aux[i].cantidadxDespachar;
                        $scope.p_Dp.cantidadDespachada = aux[i].cantidadDespachada;
                        $scope.p_Dp.cantidadPediente = aux[i].cantidadPediente;
                        $scope.p_Dp.extfactura = 0;
                        $scope.pageContentChoPD.push($scope.p_Dp);
                    }
                } else {
                    $scope.pageContentChoPD = results.data.root[0];
                }
            }, function (error) {
            });

            $('#IngPedidoFactura').modal('show');
        } else {
            $scope.traChod.txtnumeropedido = numPedido;
            $scope.pageContentChoPQ = [];
            var aux1 = $filter('filter')($scope.pageContentChoPGloabalQ, { pedido: $scope.traChod.txtnumeropedido });
            if (aux1.length != 0) {
                for (var i = 0; i < aux1.length; i++) {
                    var auxgrp = {};
                    auxgrp.id = aux1[i].id;
                    auxgrp.pedido = aux1[i].pedido;
                    auxgrp.factura = aux1[i].factura;
                    auxgrp.extfactura = aux1[i].extfactura;
                    $scope.pageContentChoPQ.push(auxgrp);
                }
            } else {
                var auxgrp = {};
                auxgrp.id = 1;
                auxgrp.pedido = $scope.traChod.txtnumeropedido;
                auxgrp.factura = "";
                auxgrp.extfactura = 0;
                $scope.pageContentChoPQ.push(auxgrp);
            }
            $('#IngPedidoFactura2').modal('show');
        }
    }

    $scope.agregarItem = function (item, codigoProducto, factura, fechaFactura, descripcion, cantidadPedido, precioUnitario, unidadCaja, descuento1, descuento2, iva, subtotal, total, cantidadxDespachar, cantidadDespachada, CantidadPediente, content) {
        if (CantidadPediente > 0 && CantidadPediente != cantidadxDespachar) {
            var max = $scope.pageContentChoPD.length;
            $scope.p_Dp = {};
            $scope.p_Dp.item = $scope.pageContentChoPD[max - 1].item + 1;
            $scope.p_Dp.numPedido = $scope.traChod.txtnumeropedido;
            $scope.p_Dp.codigoProducto = codigoProducto;
            $scope.p_Dp.factura = "";
            $scope.p_Dp.fechaFactura = "";
            $scope.p_Dp.descripcion = descripcion;
            $scope.p_Dp.cantidadPedido = cantidadPedido;
            $scope.p_Dp.precioUnitario = precioUnitario;
            $scope.p_Dp.unidadCaja = unidadCaja;
            $scope.p_Dp.descuento1 = descuento1;
            $scope.p_Dp.descuento2 = descuento2;
            $scope.p_Dp.iva = iva;
            $scope.p_Dp.subtotal = subtotal;
            $scope.p_Dp.total = total;
            $scope.p_Dp.cantidadxDespachar = CantidadPediente;
            $scope.p_Dp.cantidadDespachada = 0;
            $scope.p_Dp.cantidadPediente = CantidadPediente;
            $scope.p_Dp.extfactura = 0;
            content.cantidadxDespachar = content.cantidadxDespachar - CantidadPediente;
            content.cantidadPediente = content.cantidadxDespachar - content.cantidadDespachada;
            $scope.pageContentChoPD.push($scope.p_Dp);
        }

    }
    $scope.grabargrip = function (tipo) {
        var bandera = 0;
        if (tipo == 1) {


            var baridofacturap = $scope.pageContentChoPD;
            for (var i = 0; i < baridofacturap.length; i++) {
                for (var j = 0; j < $scope.pageContentChoPGloabal.length; j++) {
                    if (baridofacturap[i].cantidadDespachada != "0") {
                        if (baridofacturap[i].numPedido != $scope.pageContentChoPGloabal[j].numPedido) {
                            if ($scope.pageContentChoPGloabal[j].factura == baridofacturap[i].factura) {
                                bandera = 2;
                                break;
                            }
                        }
                    }
                }
            }
            if (bandera == 2) {
                $scope.MenjError = "Hay número de factura ya repetidas"
                $('#idMensajeError').modal('show');
                return;
            }

            for (var j = 0; j < $scope.pageContentChoPD.length; j++) {
                if ($scope.pageContentChoPD[j].cantidadxDespachar - $scope.pageContentChoPD[j].cantidadDespachada < 0) {
                    bandera = 1;
                    break;
                }
            }
            if (bandera == 1) {
                $scope.MenjError = "Hay columna cantidad pendiente en negativo por favor verifique"
                $('#idMensajeError').modal('show');
                return;
            } else {
                bandera = 0;
                for (var j = 0; j < $scope.pageContentChoPD.length; j++) {
                    if ($scope.pageContentChoPD[j].cantidadDespachada != 0) {
                        if ($scope.pageContentChoPD[j].factura == "" || $scope.pageContentChoPD[j].fechaFactura == "") {
                            bandera = 1;
                            break;
                        }
                    }
                }

                for (var j = 0; j < $scope.pageContentChoPD.length; j++) {
                    if ($scope.pageContentChoPD[j].cantidadDespachada != 0) {
                        if ($scope.pageContentChoPD[j].factura.length < 15) {
                            bandera = 3;
                            break;
                        }
                    }
                }

                if (bandera == 3) {
                    $scope.MenjError = "Los número de factura debe tener 15 dígitos"
                    $('#idMensajeError').modal('show');
                    return;
                }



                if (bandera == 1) {
                    $scope.MenjError = "Debe ingresar el número y fecha de factura"
                    $('#idMensajeError').modal('show');
                    return;
                }
                else {
                    var griptmp = $filter('filter')($scope.pageContentChoPGloabal, { numPedido: $scope.traChod.txtnumeropedido });
                    if (griptmp.length == 0) {
                        for (var i = 0; i < $scope.pageContentChoPD.length; i++) {
                            $scope.p_Dp = {};
                            $scope.p_Dp.item = $scope.pageContentChoPD[i].item;
                            $scope.p_Dp.numPedido = $scope.traChod.txtnumeropedido;
                            $scope.p_Dp.codigoProducto = $scope.pageContentChoPD[i].codigoProducto;
                            $scope.p_Dp.factura = $scope.pageContentChoPD[i].factura;
                            $scope.p_Dp.fechaFactura = $scope.pageContentChoPD[i].fechaFactura;
                            $scope.p_Dp.descripcion = $scope.pageContentChoPD[i].descripcion;
                            $scope.p_Dp.cantidadPedido = $scope.pageContentChoPD[i].cantidadPedido;
                            $scope.p_Dp.precioUnitario = $scope.pageContentChoPD[i].precioUnitario;
                            $scope.p_Dp.unidadCaja = $scope.pageContentChoPD[i].unidadCaja;
                            $scope.p_Dp.descuento1 = $scope.pageContentChoPD[i].descuento1;
                            $scope.p_Dp.descuento2 = $scope.pageContentChoPD[i].descuento2;
                            $scope.p_Dp.iva = $scope.pageContentChoPD[i].iva;
                            $scope.p_Dp.subtotal = $scope.pageContentChoPD[i].subtotal;
                            $scope.p_Dp.total = $scope.pageContentChoPD[i].total;
                            $scope.p_Dp.cantidadxDespachar = $scope.pageContentChoPD[i].cantidadxDespachar;
                            $scope.p_Dp.cantidadDespachada = $scope.pageContentChoPD[i].cantidadDespachada;
                            $scope.p_Dp.cantidadPediente = $scope.pageContentChoPD[i].cantidadxDespachar - $scope.pageContentChoPD[i].cantidadDespachada;
                            $scope.p_Dp.extfactura = $scope.pageContentChoPD[i].extfactura;
                            $scope.pageContentChoPGloabal.push($scope.p_Dp);
                        }
                        $('#IngPedidoFactura').modal('hide');
                    } else {
                        var index = -1;
                        var comArr = eval($scope.pageContentChoPGloabal);
                        for (var i = comArr.length - 1; i >= 0; i--) {
                            if (comArr[i].numPedido === $scope.traChod.txtnumeropedido) {
                                index = i;
                                $scope.pageContentChoPGloabal.splice(index, 1);
                            }
                        }
                        if (index === -1) {
                            alert("Something gone wrong");
                        }
                        for (var i = 0; i < $scope.pageContentChoPD.length; i++) {
                            $scope.p_Dp = {};
                            $scope.p_Dp.item = $scope.pageContentChoPD[i].item;
                            $scope.p_Dp.numPedido = $scope.traChod.txtnumeropedido;
                            $scope.p_Dp.codigoProducto = $scope.pageContentChoPD[i].codigoProducto;
                            $scope.p_Dp.factura = $scope.pageContentChoPD[i].factura;
                            $scope.p_Dp.fechaFactura = $scope.pageContentChoPD[i].fechaFactura;
                            $scope.p_Dp.descripcion = $scope.pageContentChoPD[i].descripcion;
                            $scope.p_Dp.cantidadPedido = $scope.pageContentChoPD[i].cantidadPedido;
                            $scope.p_Dp.precioUnitario = $scope.pageContentChoPD[i].precioUnitario;
                            $scope.p_Dp.unidadCaja = $scope.pageContentChoPD[i].unidadCaja;
                            $scope.p_Dp.descuento1 = $scope.pageContentChoPD[i].descuento1;
                            $scope.p_Dp.descuento2 = $scope.pageContentChoPD[i].descuento2;
                            $scope.p_Dp.iva = $scope.pageContentChoPD[i].iva;
                            $scope.p_Dp.subtotal = $scope.pageContentChoPD[i].subtotal;
                            $scope.p_Dp.total = $scope.pageContentChoPD[i].total;
                            $scope.p_Dp.cantidadxDespachar = $scope.pageContentChoPD[i].cantidadxDespachar;
                            $scope.p_Dp.cantidadDespachada = $scope.pageContentChoPD[i].cantidadDespachada;
                            $scope.p_Dp.cantidadPediente = $scope.pageContentChoPD[i].cantidadxDespachar - $scope.pageContentChoPD[i].cantidadDespachada;
                            $scope.p_Dp.extfactura = $scope.pageContentChoPD[i].extfactura;
                            $scope.pageContentChoPGloabal.push($scope.p_Dp);
                        }
                        $('#IngPedidoFactura').modal('hide');
                    }
                }
            }
        }
        else {

            var baridofactura = $scope.pageContentChoPQ;
            for (var i = 0; i < baridofactura.length; i++) {
                for (var j = 0; j < $scope.pageContentChoPGloabal.length; j++) {
                    if ($scope.pageContentChoPGloabal[j].id != baridofactura[i].id) {
                        if ($scope.pageContentChoPGloabal[j].factura == baridofactura[i].factura) {
                            bandera = 2;
                            break;
                        }
                    }
                }
            }
            if (bandera == 2) {
                $scope.MenjError = "Hay Número de factura ya repetidas"
                $('#idMensajeError').modal('show');
                return;
            }

            for (var j = 0; j < $scope.pageContentChoPQ.length; j++) {
                if ($scope.pageContentChoPQ[j].factura == "") {
                    bandera = 1;
                    break;
                }
            }
            for (var j = 0; j < $scope.pageContentChoPQ.length; j++) {
                if ($scope.pageContentChoPQ[j].factura.length < 15) {
                    bandera = 3;
                    break;
                }
            }

            if (bandera == 3) {
                $scope.MenjError = "El número de factura debe tener 15 dígitos"
                $('#idMensajeError').modal('show');
                return;
            }

            if (bandera == 1) {
                $scope.MenjError = "Hay columna de número de factura en blanco"
                $('#idMensajeError').modal('show');
                return;
            } else {
                var griptmp = $filter('filter')($scope.pageContentChoPGloabalQ, { pedido: $scope.traChod.txtnumeropedido });
                if (griptmp.length == 0) {
                    for (var i = 0; i < $scope.pageContentChoPQ.length; i++) {
                        var auxgrp = {};
                        auxgrp.id = $scope.pageContentChoPQ[i].id;
                        auxgrp.pedido = $scope.pageContentChoPQ[i].pedido;
                        auxgrp.factura = $scope.pageContentChoPQ[i].factura;
                        auxgrp.extfactura = $scope.pageContentChoPQ[i].extfactura;
                        $scope.pageContentChoPGloabalQ.push(auxgrp);
                    }
                    $('#IngPedidoFactura2').modal('hide');
                } else {
                    var index = -1;
                    var comArr = eval($scope.pageContentChoPGloabalQ);
                    for (var i = comArr.length - 1; i >= 0; i--) {
                        if (comArr[i].pedido === $scope.traChod.txtnumeropedido) {
                            index = i;
                            $scope.pageContentChoPGloabalQ.splice(index, 1);
                        }
                    }
                    for (var i = 0; i < $scope.pageContentChoPQ.length; i++) {
                        var auxgrp = {};
                        auxgrp.id = $scope.pageContentChoPQ[i].id;
                        auxgrp.pedido = $scope.pageContentChoPQ[i].pedido;
                        auxgrp.factura = $scope.pageContentChoPQ[i].factura;
                        auxgrp.extfactura = $scope.pageContentChoPQ[i].extfactura;
                        $scope.pageContentChoPGloabalQ.push(auxgrp);
                    }
                    $('#IngPedidoFactura2').modal('hide');
                }

            }

        }

    }
    $scope.agregarItem2 = function () {
        var bandera = 0;
        for (var i = 0; i < $scope.pageContentChoPQ.length; i++) {
            if ($scope.pageContentChoPQ[i].factura == "") {
                bandera = 1;
                break;
            }
        }
        if (bandera == 0) {
            var auxgrp = {};
            auxgrp.id = parseInt($scope.pageContentChoPQ[parseInt($scope.pageContentChoPQ.length) - 1].id) + 1;
            auxgrp.pedido = $scope.traChod.txtnumeropedido;
            auxgrp.factura = "";
            auxgrp.extfactura = "0";
            $scope.pageContentChoPQ.push(auxgrp);
        }

    }
    $scope.buscarfac = function (content) {
        var bandera = 0;
        for (var i = 0; i < $scope.pageContentChoPQ.length; i++) {
            if ($scope.pageContentChoPQ[i].id != content.id) {
                if ($scope.pageContentChoPQ[i].factura == content.factura) {
                    bandera = 1;
                    break;
                }
            }
        }
        if ($scope.pageContentChoPGloabalQ.length > 0) {
            for (var i = 0; i < $scope.pageContentChoPGloabalQ.length; i++) {
                if ($scope.pageContentChoPGloabalQ[i].factura == content.factura) {
                    bandera = 1;
                    break;
                }
            }
        }
        content.extfactura = bandera;
    }
    $scope.buscarfacGP = function (content) {
        var bandera = 0;
        debugger;
        if ($scope.pageContentChoPGloabal.length > 0) {
            for (var i = 0; i < $scope.pageContentChoPGloabal.length; i++) {
                if (content.numPedido != $scope.pageContentChoPGloabal[i].numPedido) {
                    if ($scope.pageContentChoPGloabal[i].factura == content.factura) {
                        bandera = 1;
                        break;
                    }
                }
            }
        }
        content.extfactura = bandera;
    }
    $scope.eliminarGrid2 = function (id) {
        var index = -1;
        var comArr = eval($scope.pageContentChoPQ);
        for (var i = comArr.length - 1; i >= 1; i--) {
            if (comArr[i].id === id) {
                index = i;
                $scope.pageContentChoPQ.splice(index, 1);
                break;
            }
        }

    }
    $scope.EliminarRegistro = function (id, estadoconsolidacion) {
        if (estadoconsolidacion == "ACTIVO") {
            idgridp = id;
            $scope.MenjConfirmacion = "¿Esta seguro de eliminar la consolidación " + id + "?"
            $('#idMensajeConfirmacionEliminar').modal('show');
        }
        else {
            $scope.MenjError = "Solo se puede eliminar una consolidación activa"
            $('#idMensajeError').modal('show');
        }
    }
    $scope.Eliminar = function () {
        $scope.myPromise = ConsolidacionPedidosService.getEliminarConsolidacion(idgridp, $scope.traCho.codProveedor).then(function (results) {
            if (results.data) {
                $scope.MenjError = "Se eliminó la consolidación sin novedad"
                $('#idMensajeGrabar').modal('show');
                $scope.BuscarFiltro();
            }
            else {
                $scope.MenjError = "No se puedo eliminar la consolidación"
                $('#idMensajeError').modal('show');
            }
        },
                function (error) {
                    var errors = [];
                    for (var key in error.data.modelState) {
                        for (var i = 0; i < error.data.modelState[key].length; i++) {
                            errors.push(error.data.modelState[key][i]);
                        }
                    }
                    $scope.MenjError = "No se puedo eliminar la consolidación"
                    $('#idMensajeError').modal('show');
                });
    }

    $scope.modificarRegistro = function (idconsolidacion, estadoconsolidacion) {
        if (estadoconsolidacion == "ACTIVO") {
            $scope.myPromise = null;
            $scope.myPromise = ConsolidacionPedidosService.getModificarConsolidacion(idconsolidacion, $scope.traCho.codProveedor).then(function (results) {
                var retorno = {};
                retorno = results.data.root[0];
                $scope.traCho.Tipo = "2";
                $scope.traChop.txtcodigoconsolidacion = retorno[0].idconsolidacion;
                $scope.traChop.txtestado = retorno[0].estado;
                $scope.traCho.cosRapido = retorno[0].cosrapido;
                $scope.traChop.txtfechaemision = retorno[0].fechaemision;
                $scope.traChop.txtalmacendestino = retorno[0].almacendestino;
                $scope.traChop.txtcaducidad = retorno[0].fechacaducidad;
                for (var i = 0; i < $scope.BodegaDatos.length; i++) {
                    if ($scope.BodegaDatos[i].codigo == retorno[0].idalmacendestino) {
                        $scope.selectbodega = $scope.BodegaDatos[i];
                        break;
                    }
                }

                for (var i = 0; i < $scope.Choferdatos.length; i++) {
                    if ($scope.Choferdatos[i].codigo == retorno[0].idchofer) {
                        $scope.selectchofer = $scope.Choferdatos[i];
                        break;
                    }
                }
                for (var i = 0; i < $scope.Ayudantesdatos.length; i++) {
                    if ($scope.Ayudantesdatos[i].codigo == retorno[0].idayudante) {
                        $scope.selectayudante = $scope.Ayudantesdatos[i];
                        break;
                    }
                }
                for (var i = 0; i < $scope.Vehiculosdatos.length; i++) {
                    if ($scope.Vehiculosdatos[i].codigo == retorno[0].idvehiculo) {
                        $scope.selectvehiculo = $scope.Vehiculosdatos[i];
                        break;
                    }
                }

                $scope.habilitar = false;
                if (retorno[0].cosrapido == "False")
                    $scope.traCho.cosRapido = false;
                else
                    $scope.traCho.cosRapido = true;

                $scope.pageContentChoP = results.data.root[1];
                if ($scope.traCho.cosRapido == false) {
                    var auxgrpd = results.data.root[2];
                    for (var i = 0; i < auxgrpd.length; i++) {
                        $scope.p_Dp = {};
                        $scope.p_Dp.item = auxgrpd[i].item;
                        $scope.p_Dp.numPedido = auxgrpd[i].numPedido;
                        $scope.p_Dp.codigoProducto = auxgrpd[i].codigoProducto;
                        $scope.p_Dp.factura = auxgrpd[i].factura;
                        $scope.p_Dp.fechaFactura = new Date(auxgrpd[i].fechaFactura);
                        $scope.p_Dp.descripcion = auxgrpd[i].descripcion;
                        $scope.p_Dp.cantidadPedido = auxgrpd[i].cantidadPedido;
                        $scope.p_Dp.precioUnitario = auxgrpd[i].precioUnitario;
                        $scope.p_Dp.unidadCaja = auxgrpd[i].unidadCaja;
                        $scope.p_Dp.descuento1 = auxgrpd[i].descuento1;
                        $scope.p_Dp.descuento2 = auxgrpd[i].descuento2;
                        $scope.p_Dp.iva = auxgrpd[i].iva;
                        $scope.p_Dp.subtotal = auxgrpd[i].subtotal;
                        $scope.p_Dp.total = auxgrpd[i].total;
                        $scope.p_Dp.cantidadxDespachar = auxgrpd[i].cantidadxDespachar;
                        $scope.p_Dp.cantidadDespachada = auxgrpd[i].cantidadDespachada;
                        $scope.p_Dp.cantidadPediente = auxgrpd[i].cantidadPediente;
                        $scope.pageContentChoPGloabal.push($scope.p_Dp);
                    }
                }
                else {
                    var auxgrpd = results.data.root[2];
                    for (var i = 0; i < auxgrpd.length; i++) {
                        var auxgrp = {};
                        auxgrp.id = auxgrpd[i].item;
                        auxgrp.pedido = auxgrpd[i].numPedido;
                        auxgrp.factura = auxgrpd[i].factura;
                        auxgrp.extfactura = "0";
                        $scope.pageContentChoPGloabalQ.push(auxgrp);
                    }
                }
                $('.nav-tabs a[href="#RegistroConsolidación"]').tab('show');
            }, function (error) {
            });
            $scope.Consultar();
        } else {
            $scope.MenjError = "Solo se puede modificar una consolidación activa"
            $('#idMensajeError').modal('show');
        }
    }
    Limpiar();
}
]);
//Solicitud de Cita controller
app.controller('SolicitudCitaController', ['$scope', '$location', 'SolicitudCitaService','ConsolidacionPedidosService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', 'uiCalendarConfig', function ($scope, $location, SolicitudCitaService,ConsolidacionPedidosService, $cookies, ngAuthSettings, FileUploader, $filter, authService, uiCalendarConfig) {
    var dateFormat = function () {
        var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
            timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
            timezoneClip = /[^-+\dA-Z]/g,
            pad = function (val, len) {
                val = String(val);
                len = len || 2;
                while (val.length < len) val = "0" + val;
                return val;
            };

        return function (date, mask, utc) {
            var dF = dateFormat;


            if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }


            date = date ? new Date(date) : new Date;
            if (isNaN(date)) throw SyntaxError("invalid date");

            mask = String(dF.masks[mask] || mask || dF.masks["default"]);


            if (mask.slice(0, 4) == "UTC:") {
                mask = mask.slice(4);
                utc = true;
            }

            var _ = utc ? "getUTC" : "get",
                d = date[_ + "Date"](),
                D = date[_ + "Day"](),
                m = date[_ + "Month"](),
                y = date[_ + "FullYear"](),
                H = date[_ + "Hours"](),
                M = date[_ + "Minutes"](),
                s = date[_ + "Seconds"](),
                L = date[_ + "Milliseconds"](),
                o = utc ? 0 : date.getTimezoneOffset(),
                flags = {
                    d: d,
                    dd: pad(d),
                    ddd: dF.i18n.dayNames[D],
                    dddd: dF.i18n.dayNames[D + 7],
                    m: m + 1,
                    mm: pad(m + 1),
                    mmm: dF.i18n.monthNames[m],
                    mmmm: dF.i18n.monthNames[m + 12],
                    yy: String(y).slice(2),
                    yyyy: y,
                    h: H % 12 || 12,
                    hh: pad(H % 12 || 12),
                    H: H,
                    HH: pad(H),
                    M: M,
                    MM: pad(M),
                    s: s,
                    ss: pad(s),
                    l: pad(L, 3),
                    L: pad(L > 99 ? Math.round(L / 10) : L),
                    t: H < 12 ? "a" : "p",
                    tt: H < 12 ? "am" : "pm",
                    T: H < 12 ? "A" : "P",
                    TT: H < 12 ? "AM" : "PM",
                    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
                };

            return mask.replace(token, function ($0) {
                return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
            });
        };
    }();


    dateFormat.masks = {
        "default": "ddd mmm dd yyyy HH:MM:ss",
        shortDate: "m/d/yy",
        mediumDate: "mmm d, yyyy",
        longDate: "mmmm d, yyyy",
        fullDate: "dddd, mmmm d, yyyy",
        shortTime: "h:MM TT",
        mediumTime: "h:MM:ss TT",
        longTime: "h:MM:ss TT Z",
        isoDate: "yyyy-mm-dd",
        isoTime: "HH:MM:ss",
        isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
        isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
    };


    dateFormat.i18n = {
        dayNames: [
            "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        ],
        monthNames: [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
        ]
    };


    Date.prototype.format = function (mask, utc) {
        return dateFormat(this, mask, utc);
    };

    $scope.div1 = true;
    $scope.etiTotRegistros = "";
    //Variable de Busquedas
    $scope.PorFechas = 1;
    $scope.fechahabilitar = true;
    $scope.numerohabilitar = true;
    $scope.txtfechadesde = "";
    $scope.txtfechahasta = "";
    $scope.PorNumero = 1;
    $scope.txtnumero = "";
    $scope.Portodosfacturas = true;
    //Fin Variable
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.GridConsolidacion = [];
    var _GridConsolidacion = [];
    $scope.pagesCo = [];
    $scope.pageContentCo = [];
    $scope.sortTypeCon = 'idconsolidacion';



    $scope.solicitad = "";
    $scope.codid = "";


    $scope.traCho = {};
    $scope.traCho.Tipo = "1";
    $scope.traCho.txtfechasolicitud = "";
    $scope.traCho.horainicial = "";
    $scope.traCho.horafinal = "";
    $scope.traCho.CodProveedor = authService.authentication.CodSAP;
    $scope.usuarioCreacion = authService.authentication.userName;

    $scope.datoscita = 0;

    $scope.sortType = 'nombres'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';
    $scope.rutaDirectorio = "";
    //Fin Variable

    $scope.imagenurl = "";
    //Variable de Mensaje
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    //Fin Variable

    $scope.consulta = true;

    var selectedDateini = "";
    var selectedMinutosini = "";
    var selectedDatefin = "";
    var selectedMinutosfin = "";
    $scope.selectedDateini = "";
    $scope.selectedMinutosini = "";
    $scope.selectedDatefin = "";
    $scope.selectedMinutosfin = "";



    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    $scope.selectbodega = "";
    $scope.BodegaDatos = [];
    ConsolidacionPedidosService.getChoferVehiculos('6', $scope.traCho.codProveedor).then(function (results) {
        $scope.BodegaDatos = results.data.root[0];
        var aux = $scope.BodegaDatos;
        for (var i = 0; i < aux.length; i++) {
            if(aux[i].codigo=='49')        
            {
                $scope.selectbodega = aux[i];
                break;
            }
        }
    }, function (error) {
    });


    $scope.calendarDate = [
       {
           color: 'blue',
           textColor: 'black',
           events: [
                    { id: 2, title: 'Repeating Event', start: new Date(2016, 12, 17, 10, 0), end: new Date(2016, 12, 17, 11, 0), allDay: false }

           ]
       }
     
    ];

    $scope.Eliminar=function()
    {
        for (var i = $scope.calendarDate.length; i > 0; i--) {
            $scope.calendarDate.splice(i, 1);
        }
    }
    

    $scope.Actualizar = function()
    {
        if ($scope.selectbodega == undefined || $scope.selectbodega.codigo == '' || $scope.selectbodega == '') {
            $scope.MenjError = "Seleccione una bodega para cargar el horario."
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.addEvent();
        }
    }
    $scope.addEvent = function () {
        $scope.Eliminar();
        var codigo = "";
        var bodega = "";
        if ($scope.traCho.CodProveedor.length==6) {
            var codigo = "0000" + $scope.traCho.CodProveedor;
        }
        else
        {
            var codigo = "00000" + $scope.traCho.CodProveedor;
        }
        if ($scope.selectbodega == undefined || $scope.selectbodega.codigo == '' || $scope.selectbodega=='') {
            bodega = '49';
        } else
        {
            bodega = $scope.selectbodega.codigo;
        }
        $scope.myPromise = SolicitudCitaService.getCargarHorario(bodega, codigo, $filter('date')(new Date(), 'yyyy/MM/dd')).then(function (results) {
            if (results.data.success) {
                var datos = results.data.root[0];
                for (var i = 0; i < datos.length; i++) {
                    $scope.calendarDate.push({
                        color: datos[i].color,
                        textColor: datos[i].colorTexto,
                        events: [
                                { id: datos[i].id, title: datos[i].titulo, start: new Date(datos[i].dias.split("/")[0], datos[i].dias.split("/")[1] - 1, datos[i].dias.split("/")[2], datos[i].hora_Inicial.split(":")[0], datos[i].hora_Inicial.split(":")[1]), end: new Date(datos[i].dias.split("/")[0], datos[i].dias.split("/")[1] - 1, datos[i].dias.split("/")[2], datos[i].hora_Final.split(":")[0], datos[i].hora_Final.split(":")[1]), allDay: false },

                        ]
                    });
                }

                if (datos.length == 0) {
                    $scope.calendarDate.push({
                        color: 'blue',
                        textColor: 'black',
                        events: [
                                { id: 89, title: 'Repeating Event', start: new Date(y, m, 8, 14, 0), end: new Date(y, m, 8, 15, 0), allDay: false },

                        ]
                    });

                    $scope.calendarDate.push({
                        color: 'blue',
                        textColor: 'black',
                        events: [
                                { id: 89, title: 'Repeating Event', start: new Date(y, m, 15, 14, 0), end: new Date(y, m, 15, 15, 0), allDay: false },

                        ]
                    });

                    $scope.calendarDate.push({
                        color: 'blue',
                        textColor: 'black',
                        events: [
                                { id: 89, title: 'Repeating Event', start: new Date(y, m, 22, 14, 0), end: new Date(y, m, 22, 15, 0), allDay: false },

                        ]
                    });

                }
            } 
            }, function (error) {
            });
        };
    

        $scope.eventClick = function (date, jsEvent, view) {
            var vario = date.source.color;
            if (vario == 'blue') {
                var selectedDateini = moment(date.start).format('YYYY-MM-DD');
                var selectedMinutosini = moment(date.start).format('HH:mm:ss');
                var selectedDatefin = moment(date.end).format('YYYY-MM-DD');
                var selectedMinutosfin = moment(date.end).format('HH:mm:ss');
                $scope.selectedDateini = $filter('date')(selectedDateini, 'yyyy-MM-dd');	// update $scope.dateFrom
                $scope.selectedMinutosini = $filter('date')(selectedMinutosini, 'HH:mm:ss');	// update $scope.dateFrom
                $scope.selectedDatefin = $filter('date')(selectedDatefin, 'yyyy-MM-dd');	// update $scope.dateFrom
                $scope.selectedMinutosfin = $filter('date')(selectedMinutosfin, 'HH:mm:ss');	// update $scope.dateFrom




                var fecha = $filter('date')(new Date(), 'yyyy-MM-dd');
                var parts2 = $scope.selectedDateini.split('-');

                var a2 = new Date(parts2[0], parts2[1] - 1, parts2[2]);
                var fechaSolicitud = $filter('date')(a2, 'yyyy-MM-dd');
                if (fechaSolicitud == fecha) {
                    $scope.MenjError = "No se puede selecionar una cita el mismo día"
                    $('#idMensajeError').modal('show');
                    return;
                }
                else {
                    if (fechaSolicitud < fecha) {
                        $scope.MenjError = "La fecha solicitada no puede ser menor a la fecha del día"
                        $('#idMensajeError').modal('show');
                        return;
                    } else {
                        var dateNew = new Date();
                        var noOfDays = 1;

                        dateNew.AddDays(noOfDays);
                        var fecha1 = $filter('date')(dateNew, 'yyyy-MM-dd');
                        if (fechaSolicitud < fecha1) {
                            $scope.MenjError = "La fecha selecionada debe ser mayor igual a " + fecha1;
                            $('#idMensajeError').modal('show');
                            return;
                        }
                        else {

                            var dateNewcom = new Date();
                            dateNewcom.AddDays(1);
                            var fechacom = $filter('date')(dateNewcom, 'yyyy-MM-dd');
                            var hora = $filter('date')(dateNewcom, 'HH:mm');

                            if (fechaSolicitud == fechacom && hora>="16:00") {
                                $scope.MenjError = "Las solicitudes de citas solo se puede generar hasta las 16:00.";
                                $('#idMensajeError').modal('show');
                                return;
                            }

                      

                        }

                    }
                }

                $scope.alertMessage = (date.title + ' Fecha Cita:' + $scope.selectedDateini + ' Hora Inicial:' + $scope.selectedMinutosini + ' Hora Final:'  + $scope.selectedMinutosfin);
                $('#IngConsolidacionPedido').modal('show');
                $scope.consulta = false;
                setTimeout(function () { $('#consultagrid').focus(); }, 150);
                setTimeout(function () { angular.element('#consultagrid').trigger('click'); }, 250);
            }
       
        };

        $scope.uiConfig = {
            calendar: {
                editable: false,
                lang: 'es',
                height: 450,
                aspectRatio: 2,
                navLinks: true,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay,listWeek'
                },
                eventClick: $scope.eventClick,
                background: '#f26522',
                loading: "Espero Por Favor"
            },
        };


        $scope.uiConfig.calendar.dayNames = ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"];
        $scope.uiConfig.calendar.dayNamesShort = ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"];
        $scope.uiConfig.calendar.monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"]


        $scope.limpioCaja = function () {
            if ($scope.PorFechas === "1") {
                $scope.txtfechadesde = "";
                $scope.txtfechahasta = "";
                $scope.fechahabilitar = true;
            }
            if ($scope.PorFechas === "2") {


                var dateString1 = new Date();
                var d1 = dateString1.format("dd/mm/yyyy");
                $scope.txtfechadesde = d1;
                $scope.txtfechahasta = d1;

                $scope.fechahabilitar = false;
            }
            if ($scope.PorNumero === "1") {
                $scope.txtnumero = "";
                $scope.numerohabilitar = true;
            }
            if ($scope.PorNumero === "2") {
                $scope.txtnumero = "";
                $scope.numerohabilitar = false;
            }
        }

        $scope.exportar = function (tipo) {
            if ($scope.GridConsolidacion.length == 0) {
                $scope.MenjError = "No hay datos para generar reporte"
                $('#idMensajeGrabar').modal('show');
                return;
            }
            $scope.myPromise = null;
            $scope.Tablacabecera = {};
            $scope.Tablacabecera.tipo = tipo;
            $scope.Tablacabecera.usuario = $scope.usuarioCreacion;
            $scope.myPromise = SolicitudCitaService.getExportarDataCita($scope.Tablacabecera, $scope.GridConsolidacion).then(function (results) {
                if (results.data != "") {
                    window.open(results.data, '_blank', "");
                }
                setTimeout(function () { $('#consultagrid').focus(); }, 100);
                setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
            }, function (error) {
            });

        }
        SolicitudCitaService.getdatos('2', "TiempoCitas").then(function (results) {
            var retorno = {};
            retorno = results.data.root[0];
            if (retorno.length > 0) {
                $scope.datoscita = retorno[0].dato;
            }

        }, function (error) {
        });

        $scope.BuscarFiltro = function () {
            $scope.Consultar();
        }


        $scope.Consultar = function () {
            $scope.solicitad = "";
            if ($scope.PorNumero == "2") {
                if ($scope.txtnumero === "") {
                    $scope.MenjError = "Debe ingresar numero consolidación"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            var a1 = "";
            var a2 = "";

            if ($scope.txtfechadesde != "") {
                var parts1 = $scope.txtfechadesde.split('/');

                a1 = new Date(parts1[2], parts1[1] - 1, parts1[0]);

            }

            if ($scope.txtfechahasta != "") {
                var parts2 = $scope.txtfechahasta.split('/');

                a2 = new Date(parts2[2], parts2[1] - 1, parts2[0]);

            }

            var fecha1 = $filter('date')(a1, 'yyyy-MM-dd');
            var fecha2 = $filter('date')(a2, 'yyyy-MM-dd');

            $scope.etiTotRegistros = "";
            $scope.myPromise = null;
            $scope.myPromise = SolicitudCitaService.getConsulaGrid("1", fecha1, fecha2, $scope.txtnumero, $scope.traCho.CodProveedor, $scope.selectbodega.codigo).then(function (results) {
                if (results.data.success) {
                    $scope.GridConsolidacion = results.data.root[0];
                    $scope.etiTotRegistros = $scope.GridConsolidacion.length.toString();
                }
                setTimeout(function () { $('#consultagrid').focus(); }, 100);
                setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
            }, function (error) {
            });
        }

        $scope.okGrabar = function () {
            $scope.GridConsolidacion = [];
            $scope.codid = "";
            $scope.solicitad = "";
            $scope.div1 = true;
            $('#IngConsolidacionPedido').modal('hide');
        }
        $scope.nuevo = function () {
            $scope.codid = "";
            $scope.solicitad = "";
            $scope.div1 = true;
            $scope.traCho.txtfechasolicitud = "";
            $scope.traCho.horainicial = "";
            $scope.traCho.horafinal = "";

            $scope.consulta = true;
        }
        $scope.SelecionarGrid = function (idconsolidacion, variacita, content) {
            if (variacita != "0") {
                $scope.MenjError = "La consolidación selecionada ya tiene una cita en proceso N°:" + variacita
                $('#idMensajeError').modal('show');
                $scope.codid = "";
                $scope.solicitad = "";
                $scope.div1 = true;
                return;
            }
            $scope.codid = idconsolidacion;
            $scope.solicitad = "La consolidación selecionada es:" + idconsolidacion;

        

            for (var i = 0; i < $scope.pageContentCo.length.toString(); i++) {
                $scope.pageContentCo[i].estado = "0";
            }
            content.estado = 'FT';
        }

        $scope.imprimir = function () {
            window.print();
        }

        $scope.generarCita = function () {
            $scope.MenjConfirmacion = "¿Esta seguro de enviar la solictud de cita?"
            $('#idMensajeConfirmacion').modal('show');
        }

        $scope.grabar = function () {
            var fecha = $filter('date')(new Date(), 'yyyy-MM-dd');
            var parts2 = $scope.selectedDateini.split('-');

            var a2 = new Date(parts2[0], parts2[1] - 1, parts2[2]);
            var fechaSolicitud = $filter('date')(a2, 'yyyy-MM-dd');
            if (fechaSolicitud == fecha) {
                $scope.MenjError = "No se puede selecionar una cita el mismo día"
                $('#idMensajeError').modal('show');
                return;
            }
            else {
                if (fechaSolicitud < fecha) {
                    $scope.MenjError = "La fecha solicitada no puede ser menor a la fecha del día"
                    $('#idMensajeError').modal('show');
                    return;
                } else {
                    var dateNew = new Date();
                    var noOfDays = $scope.datoscita;

                    dateNew.AddDays(noOfDays);
                    var fecha1 = $filter('date')(dateNew, 'yyyy-MM-dd');

                }
            }


            var horainicial = $filter('date')($scope.selectedMinutosini, 'hh:mm');
            var horafinal = $filter('date')($scope.selectedMinutosfin, 'hh:mm');

            if (horainicial >= horafinal) {
                $scope.MenjError = "La Hora Inicial no puede ser mayor a la hora Final.";
                $('#idMensajeError').modal('show');
                return;
            }
            var parts2 = $scope.selectedDateini.split('-');

            var a2 = new Date(parts2[0], parts2[1] - 1, parts2[2]);
            var fechaSolicitud = $filter('date')(a2, 'yyyy-MM-dd');

            var usuarioproveedor = 'VARIO';
            $scope.myPromise = null;
            $scope.myPromise = SolicitudCitaService.getGenerarCita($scope.codid, fechaSolicitud, horainicial, horafinal, $scope.traCho.CodProveedor, usuarioproveedor).then(function (results) {
                if (results.data.success) {
                    $scope.MenjError = "Solicitud de cita enviada. N°:" + results.data.root[0];
                    $('#idMensajeGrabar').modal('show');
                    $scope.GridConsolidacion = [];
                    $scope.pageContentCo = [];
                }else
                {
                    $scope.MenjError = "Error :" + results.data.msgError;
                    $('#idMensajeError').modal('show');
                }

            },
              function (error) {
                  var errors = [];
                  for (var key in error.data.modelState) {
                      for (var i = 0; i < error.data.modelState[key].length; i++) {
                          errors.push(error.data.modelState[key][i]);
                      }
                  }
                  alert("Error en comunicación: " + errors.join(' '));
              });


        }
    }
]);
    //Aprobacion Citas
    app.controller('AprobacionCitasController', ['$scope', '$location', 'AprobacionCitasService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, AprobacionCitasService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {



        //Variable de Busquedas
        $scope.PorFechas = 1;
        $scope.fechahabilitar = true;
        $scope.numerohabilitar = true;
        $scope.rucprohabilitar = true;
        $scope.codsapprohabilitar = true;
        $scope.txtfechadesde = "";
        $scope.txtfechahasta = "";
        $scope.PorNumero = 1;
        $scope.txtnumero = "";
        $scope.Portodosfacturas = true;
        $scope.PorProveedor = 1;
        $scope.txtRucProveedor = "";
        $scope.txtCodigoSapPr = "";
        $scope.message = 'Por Favor Espere...';
        $scope.myPromise = null;
        $scope.EstadoSolicitud = [];
        $scope.selectedItemEstado = [];
        $scope.SettingEstadoSol = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };
        //Fin Variable

        $scope.etiTotRegistros = "";
        $scope.etiTotRegistrosLP = ""
        //Combos
        $scope.EstadoSolicitudGrip = [];
        $scope.selectedItemGrip = "";
        //Fin Combos

        $scope.GridConsolidacion = [];
        var _GridConsolidacion = [];
        $scope.pagesCo = [];
        $scope.pageContentCo = [];
        $scope.sortTypeCon = 'idconsolidacion';

        $scope.pageContentCon = [];

        $scope.codid = "";


        $scope.traCho = {};
        $scope.traCho.Tipo = "1";
        $scope.traCho.txtfechasolicitud = "";
        $scope.traCho.horainicial = "";
        $scope.traCho.horafinal = "";


        $scope.sortType = 'nombres'; // set the default sort type
        $scope.sortReverse = false;  // set the default sort order
        $scope.searchFish = '';
        $scope.rutaDirectorio = "";
        //Fin Variable


        //Variable Chofer y Vehiculos
        $scope.nombreschoMostrar = "";
        $scope.cedulachoMostrar = "";
        $scope.telefonochoMostrar = "";
        $scope.nombresasiMostrar = "";
        $scope.cedulaasiMostrar = "";
        $scope.telefonoasiMostrar = "";
        $scope.imagenurlchofer = "";
        $scope.imagenurlasistente = "";
        $scope.placavehiculoMostrar = "";
        $scope.tipovehiculoMostrar = "";
        $scope.colorprincipalMostrar = "";
        $scope.imagenurlvehiculo = "";
        //Variable de Mensaje
        $scope.MenjError = "";
        $scope.MenjConfirmacion = "";
        //Fin Variable

        $scope.CodProveedor = authService.authentication.CodSAP;
        $scope.usuarioCreacion = authService.authentication.userName;
        $scope.variablelleva = "";
        $scope.id = "";
        $scope.idcita = "";
        //Carga Catalogo Estado
        AprobacionCitasService.getCatalogo('tbl_EstadoProceso').then(function (results) {
            $scope.EstadoSolicitud = results.data;
        }, function (error) {
        });

        AprobacionCitasService.getCatalogo('tbl_MotivoCancelarAdm').then(function (results) {
            $scope.EstadoSolicitudGrip = results.data;
        }, function (error) {
        });

        $scope.limpioCaja = function () {
            if ($scope.PorFechas === "1") {
                $scope.txtfechadesde = "";
                $scope.txtfechahasta = "";
                $scope.fechahabilitar = true;
            }
            if ($scope.PorFechas === "2") {
                $scope.txtfechadesde = new Date();
                $scope.txtfechahasta = new Date();
                $scope.fechahabilitar = false;
            }

            if ($scope.PorProveedor == "1") {
                $scope.txtRucProveedor = "";
                $scope.txtCodigoSapPr = "";
                $scope.rucprohabilitar = true;
                $scope.codsapprohabilitar = true;
            }
            if ($scope.PorProveedor == "2") {
                $scope.txtRucProveedor = "";
                $scope.txtCodigoSapPr = "";
                $scope.rucprohabilitar = false;
                $scope.codsapprohabilitar = true;
            }
            if ($scope.PorProveedor == "3") {
                $scope.txtRucProveedor = "";
                $scope.txtCodigoSapPr = "";
                $scope.rucprohabilitar = true;
                $scope.codsapprohabilitar = false;
            }
            if ($scope.PorNumero === "1") {
                $scope.txtnumero = "";
                $scope.numerohabilitar = true;
            }
            if ($scope.PorNumero === "2") {
                $scope.txtnumero = "";
                $scope.numerohabilitar = false;
            }
        }
        $scope.exportar = function (tipo) {
            if ($scope.pageContentCo.length == 0) {
                $scope.MenjError = "No hay datos para generar reporte"
                $('#idMensajeGrabar').modal('show');
                return;
            }
            $scope.myPromise = null;
            $scope.Tablacabecera = {};
            $scope.Tablacabecera.tipo = tipo;
            $scope.Tablacabecera.usuario = $scope.usuarioCreacion;
            $scope.myPromise = AprobacionCitasService.getExportarDataAprobarCita($scope.Tablacabecera, $scope.pageContentCo).then(function (results) {
                if (results.data != "") {
                    window.open(results.data, '_blank', "");
                }
            }, function (error) {
            });
            setTimeout(function () { $('#consultagrid').focus(); }, 10);

        }
        $scope.cancelarcita = function (idconsolidacion, selectedItemGrip, idcita, estado) {
            if (estado == "CANCELADA") {
                $scope.MenjError = "La cita ya esta cancelada"
                $('#idMensajeError').modal('show');
                return;
            }
            if (selectedItemGrip == null || selectedItemGrip == "") {
                $scope.MenjError = "Debe selecionar el motivo de cancelacion"
                $('#idMensajeError').modal('show');
                return;
            } else {
                $scope.id = idconsolidacion
                $scope.variablelleva = selectedItemGrip;
                $scope.idcita = idcita;
            }
            $scope.MenjConfirmacion = "¿Está seguro de cancelar la cita?"
            $('#idMensajeConfirmacion').modal('show');
        }

        $scope.grabar = function () {
            $scope.myPromise = null;
            $scope.myPromise = AprobacionCitasService.getGrabarCancelar($scope.id, $scope.variablelleva.codigo, $scope.usuarioCreacion, $scope.idcita).then(function (results) {
                if (results.data.success) {
                    $scope.MenjError = "Se canceló la cita sin novedad"
                    $('#idMensajeGrabar').modal('show');
                }
                $scope.Consultar();
            }, function (error) {
            });
        }

        $scope.BuscarFiltro = function () {
            $scope.Consultar();
        }

        $scope.Consultar = function () {

            if ($scope.PorNumero == "2") {
                if ($scope.txtnumero === "") {
                    $scope.MenjError = "Debe ingresar numero consolidación"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            if ($scope.PorProveedor === "2") {
                if ($scope.txtRucProveedor === "") {
                    $scope.MenjError = "Debe ingresar ruc del proveedor a consultar"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            if ($scope.PorProveedor === "3") {
                if ($scope.txtCodigoSapPr === "") {
                    $scope.MenjError = "Debe ingresar el codigo sap proveedor a consultar"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }

            var index;
            var estados = new Array();
            var a = $scope.selectedItemEstado;
            for (index = 0; index < a.length; ++index) {
                estados[index] = a[index].id;
            }
            var fecha1 = $filter('date')($scope.txtfechadesde, 'yyyy-MM-dd');
            var fecha2 = $filter('date')($scope.txtfechahasta, 'yyyy-MM-dd');
            $scope.myPromise = null;
            $scope.etiTotRegistros = "";
            $scope.myPromise = AprobacionCitasService.getConsulaGrid("1", fecha1, fecha2, $scope.txtnumero, estados, $scope.txtCodigoSapPr, $scope.txtRucProveedor).then(function (results) {
                if (results.data.success) {
                    $scope.GridConsolidacion = results.data.root[0];
                    $scope.etiTotRegistros = $scope.GridConsolidacion.length.toString();
                }
                setTimeout(function () { $('#consultagrid').focus(); }, 100);
                setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
            }, function (error) {
            });
        }

        $scope.SelecionarGrid = function (idconsolidacion) {
            $scope.codid = idconsolidacion;
            $scope.myPromise = null;
            $scope.myPromise = AprobacionCitasService.getConsulaGridDetallePedido("2", idconsolidacion).then(function (results) {
                if (results.data.success) {
                    $scope.pageContentCon = results.data.root[0];
                    $scope.etiTotRegistrosLP = $scope.pageContentCon.length.toString();
                    var retorno = {};
                    retorno = results.data.root[1];
                    $scope.nombreschoMostrar = retorno[0].nombreschoMostrar;
                    $scope.cedulachoMostrar = retorno[0].cedulachoMostrar;
                    $scope.telefonochoMostrar = retorno[0].telefonochoMostrar;
                    $scope.nombresasiMostrar = retorno[0].nombresasiMostrar;
                    $scope.cedulaasiMostrar = retorno[0].cedulaasiMostrar;
                    $scope.telefonoasiMostrar = retorno[0].telefonoasiMostrar;
                    $scope.imagenurlchofer = retorno[0].imagenurlchofer;
                    $scope.imagenurlasistente = retorno[0].imagenurlasistente;
                    $scope.placavehiculoMostrar = retorno[0].placavehiculoMostrar;
                    $scope.tipovehiculoMostrar = retorno[0].tipovehiculoMostrar;
                    $scope.colorprincipalMostrar = retorno[0].colorprincipalMostrar;
                    $scope.imagenurlvehiculo = retorno[0].imagenurlvehiculo;
                }
            }, function (error) {
            });
            $('.nav-tabs a[href="#DetalleConsolidacion"]').tab('show');
        }

        $scope.imprimir = function () {
            window.print();
        }

    }
    ]);
    //Aprobacion Citas
    app.controller('AprobacionCitasProveedorController', ['$scope', '$location', 'AprobacionCitasProveedorService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, AprobacionCitasProveedorService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {
        var dateFormat = function () {
            var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
                timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
                timezoneClip = /[^-+\dA-Z]/g,
                pad = function (val, len) {
                    val = String(val);
                    len = len || 2;
                    while (val.length < len) val = "0" + val;
                    return val;
                };

            return function (date, mask, utc) {
                var dF = dateFormat;


                if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                    mask = date;
                    date = undefined;
                }


                date = date ? new Date(date) : new Date;
                if (isNaN(date)) throw SyntaxError("invalid date");

                mask = String(dF.masks[mask] || mask || dF.masks["default"]);


                if (mask.slice(0, 4) == "UTC:") {
                    mask = mask.slice(4);
                    utc = true;
                }

                var _ = utc ? "getUTC" : "get",
                    d = date[_ + "Date"](),
                    D = date[_ + "Day"](),
                    m = date[_ + "Month"](),
                    y = date[_ + "FullYear"](),
                    H = date[_ + "Hours"](),
                    M = date[_ + "Minutes"](),
                    s = date[_ + "Seconds"](),
                    L = date[_ + "Milliseconds"](),
                    o = utc ? 0 : date.getTimezoneOffset(),
                    flags = {
                        d: d,
                        dd: pad(d),
                        ddd: dF.i18n.dayNames[D],
                        dddd: dF.i18n.dayNames[D + 7],
                        m: m + 1,
                        mm: pad(m + 1),
                        mmm: dF.i18n.monthNames[m],
                        mmmm: dF.i18n.monthNames[m + 12],
                        yy: String(y).slice(2),
                        yyyy: y,
                        h: H % 12 || 12,
                        hh: pad(H % 12 || 12),
                        H: H,
                        HH: pad(H),
                        M: M,
                        MM: pad(M),
                        s: s,
                        ss: pad(s),
                        l: pad(L, 3),
                        L: pad(L > 99 ? Math.round(L / 10) : L),
                        t: H < 12 ? "a" : "p",
                        tt: H < 12 ? "am" : "pm",
                        T: H < 12 ? "A" : "P",
                        TT: H < 12 ? "AM" : "PM",
                        Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                        o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                        S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
                    };

                return mask.replace(token, function ($0) {
                    return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
                });
            };
        }();


        dateFormat.masks = {
            "default": "ddd mmm dd yyyy HH:MM:ss",
            shortDate: "m/d/yy",
            mediumDate: "mmm d, yyyy",
            longDate: "mmmm d, yyyy",
            fullDate: "dddd, mmmm d, yyyy",
            shortTime: "h:MM TT",
            mediumTime: "h:MM:ss TT",
            longTime: "h:MM:ss TT Z",
            isoDate: "yyyy-mm-dd",
            isoTime: "HH:MM:ss",
            isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
            isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
        };


        dateFormat.i18n = {
            dayNames: [
                "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
                "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
            ],
            monthNames: [
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
                "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
            ]
        };


        Date.prototype.format = function (mask, utc) {
            return dateFormat(this, mask, utc);
        };


        //Variable de Busquedas
        $scope.PorFechas = 1;
        $scope.fechahabilitar = true;
        $scope.numerohabilitar = true;
        $scope.rucprohabilitar = true;
        $scope.codsapprohabilitar = true;
        $scope.txtfechadesde = "";
        $scope.txtfechahasta = "";
        $scope.PorNumero = 1;
        $scope.txtnumero = "";
        $scope.Portodosfacturas = true;
        $scope.PorProveedor = 1;
        $scope.txtRucProveedor = "";
        $scope.txtCodigoSapPr = "";
        $scope.message = 'Por Favor Espere...';
        $scope.myPromise = null;
        $scope.EstadoSolicitud = [];
        $scope.selectedItemEstado = [];
        $scope.SettingEstadoSol = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };
        //Fin Variable

        $scope.etiTotRegistros = "";
        $scope.etiTotRegistrosLP = ""
        //Combos
        $scope.EstadoSolicitudGrip = [];
        $scope.selectedItemGrip = "";
        //Fin Combos

        $scope.GridConsolidacion = [];
        var _GridConsolidacion = [];
        $scope.pagesCo = [];
        $scope.pageContentCo = [];
        $scope.sortTypeCon = 'idconsolidacion';

        $scope.pageContentCon = [];

        $scope.codid = "";


        $scope.traCho = {};
        $scope.traCho.Tipo = "1";
        $scope.traCho.txtfechasolicitud = "";
        $scope.traCho.horainicial = "";
        $scope.traCho.horafinal = "";


        $scope.sortType = 'nombres'; // set the default sort type
        $scope.sortReverse = false;  // set the default sort order
        $scope.searchFish = '';
        $scope.rutaDirectorio = "";
        //Fin Variable


        //Variable Chofer y Vehiculos
        $scope.nombreschoMostrar = "";
        $scope.cedulachoMostrar = "";
        $scope.telefonochoMostrar = "";
        $scope.nombresasiMostrar = "";
        $scope.cedulaasiMostrar = "";
        $scope.telefonoasiMostrar = "";
        $scope.imagenurlchofer = "";
        $scope.imagenurlasistente = "";
        $scope.placavehiculoMostrar = "";
        $scope.tipovehiculoMostrar = "";
        $scope.colorprincipalMostrar = "";
        $scope.imagenurlvehiculo = "";
        //Variable de Mensaje
        $scope.MenjError = "";
        $scope.MenjConfirmacion = "";
        //Fin Variable

        $scope.CodProveedor = authService.authentication.CodSAP;
        $scope.usuarioCreacion = authService.authentication.userName;
        $scope.variablelleva = "";
        $scope.id = "";
        $scope.idcita = "";
        //Carga Catalogo Estado
        AprobacionCitasProveedorService.getCatalogo('tbl_EstadoProceso').then(function (results) {
            $scope.EstadoSolicitud = results.data;
        }, function (error) {
        });

        AprobacionCitasProveedorService.getCatalogo('tbl_MotivoCancelarAdm').then(function (results) {
            $scope.EstadoSolicitudGrip = results.data;
        }, function (error) {
        });

        $scope.limpioCaja = function () {
            if ($scope.PorFechas === "1") {
                $scope.txtfechadesde = "";
                $scope.txtfechahasta = "";
                $scope.fechahabilitar = true;
            }
            if ($scope.PorFechas === "2") {

                var dateString = new Date();
                var d1 = dateString.format("dd/mm/yyyy");

                $scope.txtfechadesde = d1;
                $scope.txtfechahasta = d1;
                $scope.fechahabilitar = false;
            }

            if ($scope.PorProveedor == "1") {
                $scope.txtRucProveedor = "";
                $scope.txtCodigoSapPr = "";
                $scope.rucprohabilitar = true;
                $scope.codsapprohabilitar = true;
            }
            if ($scope.PorProveedor == "2") {
                $scope.txtRucProveedor = "";
                $scope.txtCodigoSapPr = "";
                $scope.rucprohabilitar = false;
                $scope.codsapprohabilitar = true;
            }
            if ($scope.PorProveedor == "3") {
                $scope.txtRucProveedor = "";
                $scope.txtCodigoSapPr = "";
                $scope.rucprohabilitar = true;
                $scope.codsapprohabilitar = false;
            }
            if ($scope.PorNumero === "1") {
                $scope.txtnumero = "";
                $scope.numerohabilitar = true;
            }
            if ($scope.PorNumero === "2") {
                $scope.txtnumero = "";
                $scope.numerohabilitar = false;
            }
        }
        $scope.exportar = function (tipo) {
            if ($scope.GridConsolidacion.length == 0) {
                $scope.MenjError = "No hay datos para generar reporte"
                $('#idMensajeGrabar').modal('show');
                return;
            }
            $scope.myPromise = null;
            $scope.Tablacabecera = {};
            $scope.Tablacabecera.tipo = tipo;
            $scope.Tablacabecera.usuario = $scope.usuarioCreacion;
            $scope.myPromise = AprobacionCitasProveedorService.getExportarDataAprobarCita($scope.Tablacabecera, $scope.GridConsolidacion).then(function (results) {
                if (results.data != "") {
                    window.open(results.data, '_blank', "");
                }
            }, function (error) {
            });
            setTimeout(function () { $('#consultagrid').focus(); }, 10);

        }
        $scope.cancelarcita = function (idconsolidacion, selectedItemGrip, idcita, estado) {
            if (estado == "CANCELADA") {
                $scope.MenjError = "La cita ya esta cancelada"
                $('#idMensajeError').modal('show');
                return;
            }
            if (selectedItemGrip == null || selectedItemGrip == "") {
                $scope.MenjError = "Debe selecionar el motivo de cancelacion"
                $('#idMensajeError').modal('show');
                return;
            } else {
                $scope.id = idconsolidacion
                $scope.variablelleva = selectedItemGrip;
                $scope.idcita = idcita;
            }
            $scope.MenjConfirmacion = "¿Está seguro de cancelar la cita?"
            $('#idMensajeConfirmacion').modal('show');
        }

        $scope.grabar = function () {
            $scope.myPromise = null;
            $scope.myPromise = AprobacionCitasProveedorService.getGrabarCancelar($scope.id, $scope.variablelleva.codigo, $scope.usuarioCreacion, $scope.idcita).then(function (results) {
                if (results.data.success) {
                    $scope.MenjError = "Se canceló la cita sin novedad"
                    $('#idMensajeGrabar').modal('show');
                }
                $scope.Consultar();
            }, function (error) {
            });
        }

        $scope.BuscarFiltro = function () {
            $scope.Consultar();
        }

        $scope.Consultar = function () {

            if ($scope.PorNumero == "2") {
                if ($scope.txtnumero === "") {
                    $scope.MenjError = "Debe ingresar numero consolidación"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            if ($scope.PorProveedor === "2") {
                if ($scope.txtRucProveedor === "") {
                    $scope.MenjError = "Debe ingresar ruc del proveedor a consultar"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            if ($scope.PorProveedor === "3") {
                if ($scope.txtCodigoSapPr === "") {
                    $scope.MenjError = "Debe ingresar el codigo sap proveedor a consultar"
                    $('#idMensajeError').modal('show');
                    return;
                }
            }

            var index;
            var estados = new Array();
            var a = $scope.selectedItemEstado;
            for (index = 0; index < a.length; ++index) {
                estados[index] = a[index].id;
            }
            var a1 = "";
            var a2 = "";
            if ($scope.txtfechadesde != "") {
                var parts1 = $scope.txtfechadesde.split('/');

                a1 = new Date(parts1[2], parts1[1] - 1, parts1[0]);

            }

            if ($scope.txtfechahasta != "") {
                var parts2 = $scope.txtfechahasta.split('/');

                a2 = new Date(parts2[2], parts2[1] - 1, parts2[0]);

            }

            var fecha1 = $filter('date')(a1, 'yyyy-MM-dd');
            var fecha2 = $filter('date')(a2, 'yyyy-MM-dd');


            $scope.myPromise = null;
            $scope.etiTotRegistros = "";
            $scope.myPromise = AprobacionCitasProveedorService.getConsulaGrid("1", fecha1, fecha2, $scope.txtnumero, estados, $scope.CodProveedor, $scope.txtRucProveedor).then(function (results) {
                if (results.data.success) {
                    $scope.GridConsolidacion = results.data.root[0];
                    $scope.etiTotRegistros = $scope.GridConsolidacion.length.toString();
                    if ($scope.GridConsolidacion.length == 0) {
                        $scope.MenjError = "No hay datos para generar reporte"
                        $('#idMensajeGrabar').modal('show');
                        return;
                    }
                } else {
                    $scope.MenjError = "No hay datos para generar reporte"
                    $('#idMensajeGrabar').modal('show');
                }
                setTimeout(function () { $('#consultagrid').focus(); }, 100);
                setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
            }, function (error) {
            });
        }

        $scope.SelecionarGrid = function (idconsolidacion) {
            $scope.codid = idconsolidacion;
            $scope.myPromise = null;
            $scope.myPromise = AprobacionCitasProveedorService.getConsulaGridDetallePedido("2", idconsolidacion).then(function (results) {
                if (results.data.success) {
                    $scope.pageContentCon = results.data.root[0];
                    $scope.etiTotRegistrosLP = $scope.pageContentCon.length.toString();
                    var retorno = {};
                    retorno = results.data.root[1];
                    $scope.nombreschoMostrar = retorno[0].nombreschoMostrar;
                    $scope.cedulachoMostrar = retorno[0].cedulachoMostrar;
                    $scope.telefonochoMostrar = retorno[0].telefonochoMostrar;
                    $scope.nombresasiMostrar = retorno[0].nombresasiMostrar;
                    $scope.cedulaasiMostrar = retorno[0].cedulaasiMostrar;
                    $scope.telefonoasiMostrar = retorno[0].telefonoasiMostrar;
                    $scope.imagenurlchofer = retorno[0].imagenurlchofer;
                    $scope.imagenurlasistente = retorno[0].imagenurlasistente;
                    $scope.placavehiculoMostrar = retorno[0].placavehiculoMostrar;
                    $scope.tipovehiculoMostrar = retorno[0].tipovehiculoMostrar;
                    $scope.colorprincipalMostrar = retorno[0].colorprincipalMostrar;
                    $scope.imagenurlvehiculo = retorno[0].imagenurlvehiculo;
                }
            }, function (error) {
            });
            $('.nav-tabs a[href="#DetalleConsolidacion"]').tab('show');
        }

        $scope.imprimir = function () {
            window.print();
        }
        setTimeout(function () { $('#consultagrid').focus(); }, 150);
        setTimeout(function () { angular.element('#consultagrid').trigger('click'); }, 250);
    }
    ]);
    //Solicitud de Ckita Rapida controller
    app.controller('SolicitudCitaRapidaController', ['$scope', '$location', 'SolicitudCitaRapidaService', '$cookies', '$filter', 'authService', function ($scope, $location, SolicitudCitaRapidaService, $cookies, $filter, authService) {
        $scope.formData = {};

        $scope.div1 = true;
        $scope.div2 = true;

        $scope.message = 'Por Favor Espere...';
        $scope.myPromise = null;

        var banderacarga = 0;

        $scope.pageContentChoPQ = [];
        $scope.pageContentChoP = [];


        $scope.traCho = {};
        $scope.traCho.txtrucproveedor = "";
        $scope.traCho.txtnombreempresa = "";
        $scope.traCho.idchofer = "";
        $scope.traCho.cedulachofer = "";
        $scope.traCho.idayudante = "";
        $scope.traCho.cedulaayudante = "";
        $scope.traCho.txtNombresPrimero = "";
        $scope.traCho.txtApellidoPrimero = "";
        $scope.traCho.txtnombreayudante = "";
        $scope.traCho.txtApellidoayudate = "";
        $scope.traCho.idvehiculo = "";
        $scope.traCho.txtplacavehiculo = "";
        $scope.traCho.codtipo = "";
        $scope.traCho.codAlmacenDestino = "";
        $scope.traCho.txtfechasolicitud = "";
        $scope.traCho.horainicial = "";
        $scope.traCho.horafinal = "";
        $scope.traCho.usuarioCreacion = authService.authentication.userName;



        $scope.datoscita = 0;

        $scope.selectbodega = "";
        $scope.BodegaDatos = [];
        $scope.selecttipovehiculo = "";
        $scope.TipoVehiculosdatos = [];

        $scope.habilitar = true;

        $scope.sortType = 'pedido'; // set the default sort type
        $scope.sortReverse = false;  // set the default sort order
        $scope.searchFish = '';

        //Carga Catalogo Categoria
        SolicitudCitaRapidaService.getCatalogo('tbl_TipoVehiculo').then(function (results) {
            $scope.TipoVehiculosdatos = results.data;
        }, function (error) {
        });

        SolicitudCitaRapidaService.getChoferVehiculos('6', $scope.traCho.codProveedor).then(function (results) {
            $scope.BodegaDatos = results.data.root[0];
        }, function (error) {
        });


        SolicitudCitaRapidaService.getdatos('2', "TiempoCitas").then(function (results) {
            var retorno = {};
            retorno = results.data.root[0];
            $scope.datoscita = retorno[0].dato;
        }, function (error) {
        });



        $scope.Confirmargrabar = function () {
            if ($scope.pageContentChoP.length == 0) {
                $scope.MenjError = "Se debe ingresar por lo menos una pedido factura"
                $('#idMensajeError').modal('show');
                return;
            }
            $scope.MenjConfirmacion = "¿Está seguro de guardar la información?"
            $('#idMensajeConfirmacion').modal('show');
        }

        $scope.grabar = function () {
            var fecha = $filter('date')(new Date(), 'yyyy-MM-dd');
            var fechaSolicitud = $filter('date')($scope.traCho.txtfechasolicitud, 'yyyy-MM-dd');

            if (fechaSolicitud < fecha) {
                $scope.MenjError = "La fecha solicitada no puede ser menor a la fecha del Día"
                $('#idMensajeError').modal('show');
                return;
            }

            $scope.traCho.codtipo = $scope.selecttipovehiculo.codigo;
            $scope.traCho.codAlmacenDestino = $scope.selectbodega.codigo;
            $scope.myPromise = null;
            $scope.myPromise = SolicitudCitaRapidaService.getGrabaCitaRapida($scope.traCho, $scope.pageContentChoP).then(function (results) {
                if (results.data) {
                    $scope.MenjError = "Se ingresó la cita rápida correctamente"
                    $('#idMensajeGrabar').modal('show');
                    limpiar();
                }
            },
          function (error) {
              var errors = [];
              for (var key in error.data.modelState) {
                  for (var i = 0; i < error.data.modelState[key].length; i++) {
                      errors.push(error.data.modelState[key][i]);
                  }
              }
              $scope.MenjError = "No se puedo grabar el registro"
              $('#idMensajeError').modal('show');
          });
        }
        $scope.buscarfac = function (content) {
            if (content.chkpedido == false) {
                content.factura = "";
                $scope.MenjError = "Se debe selecionar el pedido"
                $('#idMensajeError').modal('show');
                return;
            }

            var bandera = 0;
            for (var i = 0; i < $scope.pageContentChoPQ.length; i++) {
                if ($scope.pageContentChoPQ[i].id != content.id) {
                    if ($scope.pageContentChoPQ[i].factura == content.factura) {
                        bandera = 1;
                        break;
                    }
                }
            }
            content.extfactura = bandera;

        }

        $scope.validorCedula = function (txtrucproveedor) {
            if (txtrucproveedor == undefined) {
                return;
            }
            var campos = txtrucproveedor;
            if (campos.length >= 10) {
                numero = campos;
                var suma = 0;
                var residuo = 0;
                var pri = false;
                var pub = false;
                var nat = false;
                var numeroProvincias = 24;
                var modulo = 11;

                /* Verifico que el campo no contenga letras */
                var ok = 1;

                /* Aqui almacenamos los digitos de la cedula en variables. */
                d1 = numero.substr(0, 1);
                d2 = numero.substr(1, 1);
                d3 = numero.substr(2, 1);
                d4 = numero.substr(3, 1);
                d5 = numero.substr(4, 1);
                d6 = numero.substr(5, 1);
                d7 = numero.substr(6, 1);
                d8 = numero.substr(7, 1);
                d9 = numero.substr(8, 1);
                d10 = numero.substr(9, 1);

                /* El tercer digito es: */
                /* 9 para sociedades privadas y extranjeros */
                /* 6 para sociedades publicas */
                /* menor que 6 (0,1,2,3,4,5) para personas naturales */

                if (d3 == 7 || d3 == 8) {
                    $scope.MenjError = "El tercer dígito ingresado es inválido"
                    $('#idMensajeError').modal('show');
                    return false;
                }

                /* Solo para personas naturales (modulo 10) */
                if (d3 < 6) {
                    nat = true;
                    p1 = d1 * 2; if (p1 >= 10) p1 -= 9;
                    p2 = d2 * 1; if (p2 >= 10) p2 -= 9;
                    p3 = d3 * 2; if (p3 >= 10) p3 -= 9;
                    p4 = d4 * 1; if (p4 >= 10) p4 -= 9;
                    p5 = d5 * 2; if (p5 >= 10) p5 -= 9;
                    p6 = d6 * 1; if (p6 >= 10) p6 -= 9;
                    p7 = d7 * 2; if (p7 >= 10) p7 -= 9;
                    p8 = d8 * 1; if (p8 >= 10) p8 -= 9;
                    p9 = d9 * 2; if (p9 >= 10) p9 -= 9;
                    modulo = 10;
                }

                    /* Solo para sociedades publicas (modulo 11) */
                    /* Aqui el digito verficador esta en la posicion 9, en las otras 2 en la pos. 10 */
                else if (d3 == 6) {
                    pub = true;
                    p1 = d1 * 3;
                    p2 = d2 * 2;
                    p3 = d3 * 7;
                    p4 = d4 * 6;
                    p5 = d5 * 5;
                    p6 = d6 * 4;
                    p7 = d7 * 3;
                    p8 = d8 * 2;
                    p9 = 0;
                }

                    /* Solo para entidades privadas (modulo 11) */
                else if (d3 == 9) {
                    pri = true;
                    p1 = d1 * 4;
                    p2 = d2 * 3;
                    p3 = d3 * 2;
                    p4 = d4 * 7;
                    p5 = d5 * 6;
                    p6 = d6 * 5;
                    p7 = d7 * 4;
                    p8 = d8 * 3;
                    p9 = d9 * 2;
                }

                suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
                residuo = suma % modulo;


                /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
                if (pub == true) {
                    
                    /* El ruc de las empresas del sector publico terminan con 0001*/
                    if (numero.substr(9, 4) != '0001') {
                        $scope.MenjError = "El ruc de la empresa del sector público debe terminar con 0001"
                        $('#idMensajeError').modal('show');
                        return false;
                    }
                }
                else if (pri == true) {
                    
                    if (numero.substr(10, 3) != '001') {
                        $scope.MenjError = "El ruc de la empresa del sector privado debe terminar con 001"
                        $('#idMensajeError').modal('show');
                        return false;
                    }
                }

                else if (nat == true) {
                    
                    if (numero.length > 10 && numero.substr(10, 3) != '001') {
                        $scope.MenjError = "El ruc de la persona natural debe terminar con 001"
                        $('#idMensajeError').modal('show');
                        return false;
                    }
                }
                return true;
            }
        }
        $scope.buscarProveedor = function (tipo) {
            if (tipo == 1) {
                if ($scope.validorCedula($scope.traCho.txtrucproveedor) == false) {
                    return
                }
                if ($scope.traCho.txtrucproveedor == "") {
                    $scope.MenjError = "Debe ingresar el ruc del proveedor a consultar"
                    $('#idMensajeError').modal('show');
                    return
                }
                $scope.myPromise = null;
                $scope.myPromise = SolicitudCitaRapidaService.getBuscarProveedorDatos(tipo, $scope.traCho.txtrucproveedor).then(function (results) {
                    if (results.data.success) {
                        if (results.data.root[0].length > 0) {
                            var retorno = {};
                            retorno = results.data.root[0];
                            $scope.traCho.txtnombreempresa = retorno[0].dato;
                            $scope.div1 = false;
                        } else {
                            $scope.MenjError = "No existe el proveedor a consultar"
                            $('#idMensajeGrabar').modal('show');
                            $scope.traCho.txtrucproveedor = "";
                            $scope.traCho.txtnombreempresa = "";
                            $('#txtrucproveedor').focus();
                            $scope.div1 = true;
                        }
                    }
                }, function (error) {
                });
            }
            if (tipo == 3) {
                if ($scope.validorCedula($scope.traCho.cedulachofer) == false) {
                    return
                }
                if ($scope.traCho.cedulachofer == "") {
                    $scope.MenjError = "Debe ingresar la cédula del chofer a consultar"
                    $('#idMensajeError').modal('show');
                    return
                }
                $scope.myPromise = null;
                $scope.myPromise = SolicitudCitaRapidaService.getBuscarDatosVarios(3, $scope.traCho.cedulachofer, $scope.traCho.txtrucproveedor).then(function (results) {
                    if (results.data.success) {
                        if (results.data.root[0].length > 0) {
                            var retorno = {};
                            retorno = results.data.root[0];
                            $scope.traCho.idchofer = retorno[0].id;
                            $scope.traCho.txtNombresPrimero = retorno[0].nombre;
                            $scope.traCho.txtApellidoPrimero = retorno[0].apellido;
                        } else {
                            $scope.MenjError = "No existe el chofer a consultar"
                            $('#idMensajeGrabar').modal('show');
                            $('#txtrucproveedor').focus();
                        }
                    }
                }, function (error) {
                });
            }

            if (tipo == 5) {
                if ($scope.validorCedula($scope.traCho.cedulaayudante) == false) {
                    return
                }
                if ($scope.traCho.cedulaayudante == "") {
                    $scope.MenjError = "Debe ingresar la cédula de ayudante a consultar"
                    $('#idMensajeError').modal('show');
                    return
                }
                $scope.myPromise = null;
                $scope.myPromise = SolicitudCitaRapidaService.getBuscarDatosVarios(3, $scope.traCho.cedulaayudante, $scope.traCho.txtrucproveedor).then(function (results) {
                    if (results.data.success) {
                        if (results.data.root[0].length > 0) {
                            var retorno = {};
                            retorno = results.data.root[0];
                            $scope.traCho.idayudante = retorno[0].id;
                            $scope.traCho.txtnombreayudante = retorno[0].nombre;
                            $scope.traCho.txtApellidoayudate = retorno[0].apellido;
                        } else {
                            $scope.MenjError = "No existe el ayudante a consultar"
                            $('#idMensajeGrabar').modal('show');
                            $('#txtrucproveedor').focus();
                        }
                    }
                }, function (error) {
                });
            }

            if (tipo == 4) {
                if ($scope.validorCedula($scope.traCho.txtplacavehiculo) == false) {
                    return
                }
                if ($scope.traCho.txtplacavehiculo == "") {
                    $scope.MenjError = "Debe ingresar la placa a consultar"
                    $('#idMensajeError').modal('show');
                    return
                }
                $scope.myPromise = null;
                $scope.myPromise = SolicitudCitaRapidaService.getBuscarDatosVarios(4, $scope.traCho.txtplacavehiculo, $scope.traCho.txtrucproveedor).then(function (results) {
                    if (results.data.success) {
                        if (results.data.root[0].length > 0) {
                            var retorno = {};
                            retorno = results.data.root[0];
                            $scope.traCho.idvehiculo = retorno[0].id;
                            $scope.traCho.txtplacavehiculo = retorno[0].placa;
                            var index;
                            var a = $scope.TipoVehiculosdatos;
                            for (index = 0; index < a.length; ++index) {
                                if (a[index].codigo === retorno[0].codtipo)
                                    $scope.selecttipovehiculo = a[index];
                            }
                        } else {
                            $scope.MenjError = "No existe la placa a consultar"
                            $('#idMensajeGrabar').modal('show');
                            $('#txtrucproveedor').focus();
                        }
                    }
                }, function (error) {
                });
            }
        }

        $scope.nuevo = function () {
            limpiar();
        }

        $scope.cancelar = function () {
            limpiar();
        }
        function limpiar() {
            banderacarga = 0;
            $scope.traCho.txtrucproveedor = "";
            $scope.traCho.txtnombreempresa = "";
            $scope.traCho.idchofer = "";
            $scope.traCho.cedulachofer = "";
            $scope.traCho.idayudante = "";
            $scope.traCho.cedulaayudante = "";
            $scope.traCho.txtNombresPrimero = "";
            $scope.traCho.txtApellidoPrimero = "";
            $scope.traCho.txtnombreayudante = "";
            $scope.traCho.txtApellidoayudate = "";
            $scope.traCho.idvehiculo = "";
            $scope.traCho.txtplacavehiculo = "";
            $scope.traCho.codtipo = "";
            $scope.traCho.codAlmacenDestino = "";

            $scope.selectbodega = "";
            $scope.selecttipovehiculo = "";
            $scope.pageContentChoPQ = [];
            $scope.pageContentChoP = [];
            $scope.div1 = true;
        }

        $scope.cargarPedido = function () {
            if (banderacarga == 0) {
                $scope.pageContentChoPQ = [];
                $scope.myPromise = null;
                $scope.myPromise = SolicitudCitaRapidaService.getBuscarPedidosCitas("2", $scope.traCho.txtrucproveedor).then(function (results) {
                    if (results.data.success) {
                        $scope.pageContentChoPQ = results.data.root[0];
                        banderacarga = 1;
                    }
                }, function (error) {
                });
            }

            $('#IngPedidoFactura2').modal('show');
        }

        $scope.agregarItem2 = function (txtnumeropedido, factura) {
            var bandera = 0;

            if (factura == "") {
                bandera = 1;
                return;
            }
            if (bandera == 0) {
                var auxgrp = {};
                auxgrp.id = parseInt($scope.pageContentChoPQ[parseInt($scope.pageContentChoPQ.length) - 1].id) + 1;
                auxgrp.pedido = txtnumeropedido;
                auxgrp.factura = "";
                auxgrp.extfactura = "0";
                $scope.pageContentChoPQ.push(auxgrp);
            }

        }

        $scope.eliminarGrid2 = function (id) {
            var index = -1;
            var comArr = eval($scope.pageContentChoPQ);
            for (var i = comArr.length - 1; i >= 1; i--) {
                if (comArr[i].id === id) {
                    index = i;
                    $scope.pageContentChoPQ.splice(index, 1);
                    break;
                }
            }

        }

        $scope.grabargrip = function (tipo) {
            var bandera = 0;
            for (var j = 0; j < $scope.pageContentChoPQ.length; j++) {
                if ($scope.pageContentChoPQ[j].chkpedido == 1) {
                    if ($scope.pageContentChoPQ[j].factura.length < 15) {
                        bandera = 3;
                        break;
                    }
                }
            }

            if (bandera == 3) {
                $scope.MenjError = "Los número de factura debe tener 15 dígitos"
                $('#idMensajeError').modal('show');
                return;
            }

            var aux1 = $filter('filter')($scope.pageContentChoPQ, { chkpedido: true });
            $scope.pageContentChoP = [];
            $scope.pageContentChoP = aux1;
            $('#IngPedidoFactura2').modal('hide');
        }

    }
    ]);

    //Reoporte Tabular Citas
    app.controller('ReporteTabularCitasController', ['$scope', '$location', 'ReporteTabularCitasService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, ReporteTabularCitasService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {



        //Variable de Busquedas
        $scope.PorFechas = 1;
        $scope.fechahabilitar = true;
        $scope.txtfechadesde = "";
        $scope.txtfechahasta = "";
        $scope.message = 'Por Favor Espere...';
        $scope.myPromise = null;
        $scope.EstadoSolicitud = [];
        $scope.selectedItemEstado = [];
        $scope.SettingEstadoSol = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };
        //Fin Variable

        $scope.etiTotRegistros = "";
        //Combos
        $scope.EstadoSolicitudGrip = [];
        $scope.selectedItemGrip = "";
        //Fin Combos

        $scope.GridConsolidacion = [];
        var _GridConsolidacion = [];
        $scope.pagesCo = [];
        $scope.pageContentCo = [];
        $scope.sortTypeCon = 'idconsolidacion';

        $scope.pageContentCon = [];

        $scope.codid = "";


        $scope.traCho = {};
        $scope.traCho.Tipo = "1";
        $scope.traCho.txtfechasolicitud = "";
        $scope.traCho.horainicial = "";
        $scope.traCho.horafinal = "";


        $scope.sortType = 'nombres'; // set the default sort type
        $scope.sortReverse = false;  // set the default sort order
        $scope.searchFish = '';
        $scope.rutaDirectorio = "";
        //Fin Variable



        $scope.MenjError = "";
        $scope.MenjConfirmacion = "";
        //Fin Variable

        $scope.CodProveedor = authService.authentication.CodSAP;
        $scope.usuarioCreacion = authService.authentication.userName;
        $scope.variablelleva = "";
        $scope.id = "";
        $scope.idcita = "";


        $scope.limpioCaja = function () {
            if ($scope.PorFechas === "1") {
                $scope.txtfechadesde = "";
                $scope.txtfechahasta = "";
                $scope.fechahabilitar = true;
            }
            if ($scope.PorFechas === "2") {
                $scope.txtfechadesde = new Date();
                $scope.txtfechahasta = new Date();
                $scope.fechahabilitar = false;
            }


        }

        $scope.BuscarFiltro = function () {
            $scope.Consultar();
        }

        $scope.Consultar = function () {
            var index;
            var estados = new Array();
            var a = $scope.selectedItemEstado;
            for (index = 0; index < a.length; ++index) {
                estados[index] = a[index].id;
            }
            var fecha1 = $filter('date')($scope.txtfechadesde, 'yyyy-MM-dd');
            var fecha2 = $filter('date')($scope.txtfechahasta, 'yyyy-MM-dd');
            $scope.myPromise = null;
            $scope.etiTotRegistros = "";
            $scope.myPromise = ReporteTabularCitasService.getConsulaGrid("1", fecha1, fecha2).then(function (results) {
                if (results.data.success) {
                    $scope.pageContentCo = results.data.root[0];
                    $scope.etiTotRegistros = $scope.pageContentCo.length.toString();
                }
                setTimeout(function () { $('#consultagrid').focus(); }, 100);
                setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
            }, function (error) {
            });
        }

        $scope.exportar = function (tipo) {
            if ($scope.pageContentCo.length == 0) {
                $scope.MenjError = "No hay datos para generar reporte"
                $('#idMensajeGrabar').modal('show');
                return;
            }
            $scope.myPromise = null;
            $scope.Tablacabecera = {};
            $scope.Tablacabecera.tipo = tipo;
            $scope.Tablacabecera.usuario = $scope.usuarioCreacion;
            $scope.myPromise = ReporteTabularCitasService.getExportarData($scope.Tablacabecera, $scope.pageContentCo).then(function (results) {
                if (results.data != "") {
                    window.open(results.data, '_blank', "");
                }
            }, function (error) {
            });
            setTimeout(function () { $('#consultagrid').focus(); }, 10);

        }


    }
    ]);
    //Reoporte Mercaderia Recibir
    app.controller('ReporteMercaderiaRecibirController', ['$scope', '$location', 'ReporteMercaderiaRecibirService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, ReporteMercaderiaRecibirService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {

        //Variable de Busquedas
        $scope.PorFechas = 1;
        $scope.fechahabilitar = true;
        $scope.txtnunmeroCita = "";
        $scope.txtfechahasta = "";
        $scope.message = 'Por Favor Espere...';
        $scope.myPromise = null;
        $scope.EstadoSolicitud = [];
        $scope.selectedItemEstado = [];
        $scope.SettingEstadoSol = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };
        //Fin Variable

        $scope.etiTotRegistros = "";
        //Combos
        $scope.EstadoSolicitudGrip = [];
        $scope.selectedItemGrip = "";
        //Fin Combos

        $scope.GridConsolidacion = [];
        var _GridConsolidacion = [];
        $scope.pagesCo = [];
        $scope.pageContentCo = [];
        $scope.sortTypeCon = 'idconsolidacion';

        $scope.pageContentCon = [];

        $scope.codid = "";


        $scope.traCho = {};
        $scope.traCho.Tipo = "1";
        $scope.traCho.txtfechasolicitud = "";
        $scope.traCho.horainicial = "";
        $scope.traCho.horafinal = "";


        $scope.sortType = 'nombres'; // set the default sort type
        $scope.sortReverse = false;  // set the default sort order
        $scope.searchFish = '';
        $scope.rutaDirectorio = "";
        //Fin Variable



        $scope.MenjError = "";
        $scope.MenjConfirmacion = "";
        //Fin Variable

        $scope.CodProveedor = authService.authentication.CodSAP;
        $scope.usuarioCreacion = authService.authentication.userName;
        $scope.variablelleva = "";
        $scope.id = "";
        $scope.idcita = "";


        $scope.limpioCaja = function () {
            if ($scope.PorFechas === "1") {
                $scope.txtfechadesde = "";
                $scope.txtfechahasta = "";
                $scope.fechahabilitar = true;
            }
            if ($scope.PorFechas === "2") {
                $scope.txtfechadesde = new Date();
                $scope.txtfechahasta = new Date();
                $scope.fechahabilitar = false;
            }


        }

        $scope.BuscarFiltro = function () {
            $scope.Consultar();
        }

        $scope.Consultar = function () {
            var index;
            var estados = new Array();
            var a = $scope.selectedItemEstado;
            for (index = 0; index < a.length; ++index) {
                estados[index] = a[index].id;
            }


            var a1 = "";
            var a2 = "";

            if ($scope.txtfechahasta != "") {
                var parts2 = $scope.txtfechahasta.split('/');

                a2 = new Date(parts2[2], parts2[1] - 1, parts2[0]);

            }

            var fecha1 = $filter('date')(a2, 'yyyy-MM-dd');
            var fecha2 = $filter('date')(a2, 'yyyy-MM-dd');

            $scope.myPromise = null;
            $scope.etiTotRegistros = "";
            $scope.myPromise = ReporteMercaderiaRecibirService.getConsulaGrid("1", fecha1, fecha2, $scope.txtnunmeroCita, $scope.CodProveedor).then(function (results) {
                if (results.data.success) {
                    $scope.pageContentCo = results.data.root[0];
                    $scope.etiTotRegistros = $scope.pageContentCo.length.toString();
                    if ($scope.pageContentCo.length == 0) {
                        $scope.MenjError = "No hay datos para generar reporte"
                        $('#idMensajeGrabar').modal('show');
                        return;
                    }
                }
                else
                {
                    if ($scope.pageContentCo.length == 0) {
                        $scope.MenjError = "No hay datos para generar reporte"
                        $('#idMensajeGrabar').modal('show');
                        return;
                    }
                }
                setTimeout(function () { $('#consultagrid').focus(); }, 100);
                setTimeout(function () { $('#rbtTraTN').focus(); }, 150);
            }, function (error) {
            });
        }

        $scope.exportar = function (tipo) {
            if ($scope.pageContentCo.length == 0) {
                $scope.MenjError = "No hay Datos para Generar Reporte"
                $('#idMensajeGrabar').modal('show');
                return;
            }

            var a1 = "";
            var a2 = "";

            if ($scope.txtfechahasta != "") {
                var parts2 = $scope.txtfechahasta.split('/');

                a2 = new Date(parts2[2], parts2[1] - 1, parts2[0]);

            }

            var fecha1 = $filter('date')(a1, 'yyyy-MM-dd');
            var fecha2 = $filter('date')(a2, 'yyyy-MM-dd');


            $scope.myPromise = null;
            $scope.Tablacabecera = {};
            $scope.Tablacabecera.tipo = tipo;
            $scope.Tablacabecera.usuario = $scope.usuarioCreacion;
            $scope.myPromise = ReporteMercaderiaRecibirService.getExportarData(tipo, $scope.usuarioCreacion, "1", fecha2, fecha2, $scope.txtnunmeroCita, $scope.CodProveedor).then(function (results) {
                if (results.data != "") {
                    if (tipo == "1") {
                        var file = new Blob([results.data], { type: 'application/xls' });
                        saveAs(file, 'ReporteTabularCita_' + $scope.CodProveedor + '.xls');
                    }
                    if (tipo == "2") {
                        var file = new Blob([results.data], { type: 'application/pdf' });
                        saveAs(file, 'ReporteTabularCita_' + $scope.CodProveedor + '.pdf');
                    }
                }
            }, function (error) {
            });
            setTimeout(function () { $('#consultagrid').focus(); }, 10);

        }
    }
    ]);

    //Reporte Acta Reepcion de Pedido
    app.controller('ReporteActaRecepcionController', ['$scope', '$location', 'ReporteActaRecepcionService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', 'TransporteProveedorService', function ($scope, $location, ReporteActaRecepcionService, $cookies, ngAuthSettings, FileUploader, $filter, authService, TransporteProveedorService) {

        var dateFormat = function () {
            var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
                timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
                timezoneClip = /[^-+\dA-Z]/g,
                pad = function (val, len) {
                    val = String(val);
                    len = len || 2;
                    while (val.length < len) val = "0" + val;
                    return val;
                };

            return function (date, mask, utc) {
                var dF = dateFormat;


                if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                    mask = date;
                    date = undefined;
                }


                date = date ? new Date(date) : new Date;
                if (isNaN(date)) throw SyntaxError("invalid date");

                mask = String(dF.masks[mask] || mask || dF.masks["default"]);


                if (mask.slice(0, 4) == "UTC:") {
                    mask = mask.slice(4);
                    utc = true;
                }

                var _ = utc ? "getUTC" : "get",
                    d = date[_ + "Date"](),
                    D = date[_ + "Day"](),
                    m = date[_ + "Month"](),
                    y = date[_ + "FullYear"](),
                    H = date[_ + "Hours"](),
                    M = date[_ + "Minutes"](),
                    s = date[_ + "Seconds"](),
                    L = date[_ + "Milliseconds"](),
                    o = utc ? 0 : date.getTimezoneOffset(),
                    flags = {
                        d: d,
                        dd: pad(d),
                        ddd: dF.i18n.dayNames[D],
                        dddd: dF.i18n.dayNames[D + 7],
                        m: m + 1,
                        mm: pad(m + 1),
                        mmm: dF.i18n.monthNames[m],
                        mmmm: dF.i18n.monthNames[m + 12],
                        yy: String(y).slice(2),
                        yyyy: y,
                        h: H % 12 || 12,
                        hh: pad(H % 12 || 12),
                        H: H,
                        HH: pad(H),
                        M: M,
                        MM: pad(M),
                        s: s,
                        ss: pad(s),
                        l: pad(L, 3),
                        L: pad(L > 99 ? Math.round(L / 10) : L),
                        t: H < 12 ? "a" : "p",
                        tt: H < 12 ? "am" : "pm",
                        T: H < 12 ? "A" : "P",
                        TT: H < 12 ? "AM" : "PM",
                        Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                        o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                        S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
                    };

                return mask.replace(token, function ($0) {
                    return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
                });
            };
        }();


        dateFormat.masks = {
            "default": "ddd mmm dd yyyy HH:MM:ss",
            shortDate: "m/d/yy",
            mediumDate: "mmm d, yyyy",
            longDate: "mmmm d, yyyy",
            fullDate: "dddd, mmmm d, yyyy",
            shortTime: "h:MM TT",
            mediumTime: "h:MM:ss TT",
            longTime: "h:MM:ss TT Z",
            isoDate: "yyyy-mm-dd",
            isoTime: "HH:MM:ss",
            isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
            isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
        };


        dateFormat.i18n = {
            dayNames: [
                "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
                "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
            ],
            monthNames: [
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
                "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
            ]
        };


        Date.prototype.format = function (mask, utc) {
            return dateFormat(this, mask, utc);
        };
        //Variable de Busquedas
        $scope.txtorden = "";
        $scope.txtfactura = "";

        $scope.pOpcGrp1 = "P";
        var dateString1 = new Date();
        var dateString = new Date();

        var d1 = dateString1.format("dd/mm/yyyy");

        dateString.setDate(dateString.getDate() - 15);
        var d2 = dateString.format("dd/mm/yyyy");

        $scope.pFechaIni = d2;
        $scope.pFechaFin = d1;

        $scope.message = 'Por Favor Espere...';
        $scope.myPromise = null;
        //Fin Variable

        $scope.etiTotRegistros = "";
        //Combos
        $scope.EstadoSolicitudGrip = [];
        $scope.selectedItemGrip = "";
        //Fin Combos

        $scope.GridConsolidacion = [];
        var _GridConsolidacion = [];
        $scope.pagesCo = [];
        $scope.pageContentCo = [];

        $scope.resDgConsulta = [];
        $scope.sortTypeCon = 'idconsolidacion';

        $scope.pageContentCon = [];

        $scope.codid = "";
        $scope.datArchivo = {};

        $scope.pCodSAP = authService.authentication.CodSAP;



        $scope.traCho = {};
        $scope.traCho.Tipo = "1";
        $scope.traCho.txtfechasolicitud = "";
        $scope.traCho.horainicial = "";
        $scope.traCho.horafinal = "";


        $scope.sortType = 'nombres'; // set the default sort type
        $scope.sortReverse = false;  // set the default sort order
        $scope.searchFish = '';
        $scope.rutaDirectorio = "";
        //Fin Variable



        $scope.MenjError = "";
        $scope.MenjConfirmacion = "";
        //Fin Variable

        $scope.CodProveedor = authService.authentication.CodSAP;
        $scope.usuarioCreacion = authService.authentication.userName;
        $scope.variablelleva = "";
        $scope.id = "";
        $scope.idcita = "";
        //fca
        $scope.cmbestado = [];
        $scope.cmbestadoActas = [];

        $scope.myPromise = TransporteProveedorService.getCatalogo('tbl_estadoActas').then(function (results) {
            $scope.cmbestadoActas = results.data;
            $scope.cmbestadoActas.splice(0, 0, { codigo: "T", detalle: "Todos", descalterno: "" });
            $scope.cmbestado = results.data[0];
        }, function (error) {
        });

        $scope.limpioCaja = function () {
            $scope.txtsap = "";
            $scope.txtusuario = "";

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
        $scope.Consultar = function () {
            var Fecha1 = ""; var Fecha2 = "";
            if ($scope.pOpcGrp1 == "R") {
                if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
                    $scope.showMessage('I', 'Seleccione la fecha inicial del rango a consultar.');
                    return;
                }
                if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
                    $scope.showMessage('I', 'Seleccione la fecha final del rango a consultar.');
                    return;
                }


                if (validate_fechaMayorQue($scope.pFechaIni, $scope.pFechaFin)) {
                    $scope.showMessage('I', 'La fecha final debe ser mayor a la fecha inicial a consultar.');
                    return;
                }

                Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
                Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');

            }


            $scope.myPromise = null;
            $scope.etiTotRegistros = "";
            $scope.myPromise = ReporteActaRecepcionService.getConsulaGridActaRecepcion("2", $scope.txtorden, $scope.txtfactura, Fecha1, Fecha2, $scope.pCodSAP,"").then(function (results) {
                if (results.data.success) {
                    $scope.resDgConsulta = results.data.root[0];
                    //Filtro de estado fca
                    debugger;
                    if ($scope.cmbestado.codigo == 'A')
                        $scope.resDgConsulta = $filter('filter')($scope.resDgConsulta, { estado: "A" }, true);
                    if ($scope.cmbestado.codigo == 'X')
                        $scope.resDgConsulta = $filter('filter')($scope.resDgConsulta, { estado: "X" }, true);
                    $scope.etiTotRegistros = $scope.resDgConsulta.length.toString();


                    if ($scope.etiTotRegistros == 0) {
                        $scope.resDgConsulta = [];
                        $scope.showMessage('I', 'No exiten resultado para su consulta.');
                        return;
                    }
                }
                else {
                    $scope.resDgConsulta = [];
                    $scope.etiTotRegistros = "0";
                    $scope.showMessage('I', 'No exiten resultado para su consulta.');
                    return;
                }
                setTimeout(function () { $('#btnConsulta1').focus(); }, 100);
                setTimeout(function () { $('#txtsap').focus(); }, 150);

            }, function (error) {
            });
        }

        $scope.descargaArchivo = function (content) {
            $scope.myPromise = null;
            $scope.datArchivo = content
            if (content.estado == 'X')
            {
                $scope.MenjError = "El acta que desea descargar se encuentra en estado "
                $('#idMensajeInformativoActas').modal('show');
            }
            else {
                $scope.descargaArchivo2();

            }


        }

        $scope.descargaArchivo2 = function () {
            var nombreArchivo = $scope.datArchivo.archivo;
            var codSap = $scope.datArchivo.archivo.split('_')[1];
            var codAlmacen = $scope.datArchivo.archivo.split('_')[2].substring(1,4);
            $scope.myPromise = ReporteActaRecepcionService.getArchivoActaRecepcion($scope.datArchivo.archivo, $scope.datArchivo.anio, codSap, codAlmacen).then(function (results) {
                if (results.data != "") {
                    var file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, nombreArchivo);
                }
            }, function (error) {
            });
            setTimeout(function () { $('#consultagrid').focus(); }, 10);
            $scope.datArchivo = {};
        }


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

    }
    ]);

    //Reoporte Mercaderia Recibir
    app.controller('muertosessionController', ['$scope', '$location', 'ReporteMercaderiaRecibirService', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', 'authService', function ($scope, $location, ReporteMercaderiaRecibirService, $cookies, ngAuthSettings, FileUploader, $filter, authService) {

        $scope.aceptarmuerte = function () {
            authService.logOut();
            window.location = "../Home/Index";
        }
    }
    ]);


