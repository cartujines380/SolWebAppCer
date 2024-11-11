
//'use strict';
app.controller('frmToleranciaPedidosController', ['$scope', 'lineaNegocioService','SeguridadService', '$filter', 'authService', function ($scope,lineaNegocioService, SeguridadService, $filter, authService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.resDgUsrsAdmin = [];

    var _resDgUsrsAdmin = [];
    $scope.pagesUsrsAdmin = [];

    $scope.pgcDgUsrsAdmin = [];

    $scope.EsBloqCambioClave = true;
    $scope.rbtOpciones = 'C';
    $scope.chkSinUsuario = true;
    $scope.chkConUsuario = true;
    $scope.txtFiltraRuc = "";
    $scope.txtFiltraNombre = "";
    $scope.txtFiltraCodSap = "";
    $scope.cboFiltraEstado = [];
    $scope.cboFiltraEstadoSelItem = "";
    $scope.usrAdm = {};
    $scope.usrAdm.pRuc = "";
    $scope.usrAdm.pNombre = "";
    $scope.usrAdm.pUsuario = "";
    $scope.usrAdm.pClave = "";
    $scope.usrAdm.pCorreoE = "";
    $scope.usrAdm.pTelefono = "";
    $scope.usrAdm.pCelular = "";
    $scope.usrAdm.pCodSap = "";
    $scope.usrAdm.pIdParticipante = parseInt("-1", 10);
    $scope.usrAdm.pEstado = "";
    $scope.usrAdm.pIdRepresentante = "";
    $scope.usrAdmCtr = {};

    $scope.usrLinea = {};
    $scope.usrsLinea = [];
    $scope.lineas = [];
    $scope.lineasUsuario = [];
    $scope.listaProveedores = [];
    $scope.nombresEmpresas = [];
    getLineas();
    $scope.chkTodos = true;
    $scope.bloquear = true;
    $scope.textoBoton = "Guardar";

    //Obtener proveedores con usuario CER
    var criterio = "<Root CRITERIO='T' NOMBRE='' CODPROVEEDOR='' />";
    var myPromise = lineaNegocioService.getBandejaproveedor(criterio).then(function (results) {
        $scope.listaProveedores = results.data.root[0];
        if ($scope.listaProveedores != undefined) {
            for (var j = 0; j < $scope.listaProveedores.length ; j++) {
                $scope.nombresEmpresas.push($scope.listaProveedores[j].razonSocial);
            }
        }
        
    }, function (error) {
    });

   
  

    function getLineas() {

        var promesa = SeguridadService.getCatalogo('tbl_LineaNegocio').then(function (results) {
    
            $scope.lineas = results.data;
            setLineas($scope.lineas);
        }, function (error) {
        });
    }

    function setTextBoton(row) {

        if(row.esIngresado==0)
            $scope.textoBoton = "Guardar";
        else
            $scope.textoBoton = "Actualizar";
    }

    function getBandeja(criterio) {
        var promesa = lineaNegocioService.getBandeja(criterio).then(function (results) {
            $scope.usrsLinea = results.data.root[0];
        }, function (error) {
        });
    }

    $scope.showEliminar = function (row) {
  
        $scope.usrLinea = angular.copy(row);
        $scope.MenjConfirmacion = "¿Estas seguro de eliminar el registro?";
        $('#idMensajeConfirmacionEliminar').modal('show');
    }


    $scope.Eliminar = function () {
  
        var criterio = "<Root CRITERIO='E' USUARIO='" + $scope.usrLinea.idUsuario + "'/>"
        var promesa = lineaNegocioService.getElimina(criterio).then(function (results) {
                
            if (results.data == 0) {
                $('#idMensajeConfirmacionEliminar').modal('hide');
                $scope.showMessage('S', "Registro eliminado satisfactoriamente.");
                //getBandeja("<Root CRITERIO='T' NOMBRE='' RUC='' />");
                $scope.btnConsultaClick();
            }
        }, function (error) {
        });
    }



    $scope.usrAdmCtr.cboEstado = [];
    $scope.usrAdmCtr.cboEstadoSelItem = "";
    //$scope.myPromise = SeguridadService.getCatalogo('tbl_EstadoUsuarios').then(function (results) {

    //    $scope.usrAdmCtr.cboEstado = results.data;
    //    $scope.usrAdmCtr.cboEstadoSelItem = results.data[0];
    //    $scope.cboFiltraEstado = results.data;
    //    $scope.cboFiltraEstadoSelItem = results.data[0];
    //}, function (error) {
    //});
    $scope.etiTotRegistros = "";

    $scope.exportar = function (tipoReporte) {
        $scope.CurrentDate = new Date();
        var fechaFormateada = $filter('date')($scope.CurrentDate, "yyyyMMddHHmmss");
        $scope.myPromise = lineaNegocioService.getReporteTolerancia(authService.authentication.IdentParticipante, tipoReporte).then(function (results) {

            if (results.data != "") {
                var file;
                if(tipoReporte == "1")                    
                {
                    
                    file = new Blob([results.data], { type: 'application/pdf' });
                    saveAs(file, 'ReporteToleranciaProveedor' + fechaFormateada + '.pdf');
                }
                if (tipoReporte == "2") {
                    file = new Blob([results.data], { type: 'application/xls' });
                    saveAs(file, 'ReporteToleranciaProveedor' + fechaFormateada + '.xls');
                }
                
            }

        }, function (error) {

            $scope.MenjError = "Error en comunicación: getReporteTolerancia().";
            $('#idMensajeError').modal('show');

        });
    }
    
    $scope.saveTolerancia = function () {
        var accion = "U";

        if (!$scope.bloquear)
        {
            //Validar que no este registrado el proveedor
            var registro = $filter('filter')($scope.pgcDgUsrsAdmin, { codProveedor: $scope.usrLinea.codProveedor }, true)[0];
            if (registro != undefined)
            {
                if (registro.codProveedor == $scope.usrLinea.codProveedor) {
                    $scope.showMessage('E', 'Proveedor ya registrado.');
                    return;
                }
            }
            
            accion = "I";
        }


        $scope.myPromise = lineaNegocioService.getMantenimientoTolerancia($scope.usrLinea.codProveedor, $scope.usrLinea.porcentaje, accion).then(function (results) {            
            if (results.data.success) {
                if ($scope.bloquear) $scope.showMessage('S', 'Porcentaje actualizado correctamente');
                if (!$scope.bloquear) $scope.showMessage('S', 'Porcentaje registrado correctamente');
                
            }
            else {
                $scope.showMessage('E', 'Error: ' + results.data.msgError);
            }

        }, function (error) {
            $scope.showMessage('E', "Error en comunicación: getMantenimientoTolerancia();");

        });


        
    }

    
    $("#btnMensajeOK").click(function () {
        window.location = 'frmToleranciaPedidos';
    });

    $scope.buscarDatos = function () {
        if ($scope.usrLinea.razonSocial == undefined) return;
        var tam = $scope.usrLinea.razonSocial.length
        if (tam > 9)
        {
            
            var registro = $filter('filter')($scope.listaProveedores, { razonSocial: $scope.usrLinea.razonSocial }, true)[0];
            if (registro == undefined || registro == "") {
                $scope.usrLinea.ruc = "";
                $scope.usrLinea.codProveedor = "";
            }
            else {
                $scope.usrLinea.ruc = registro.ruc;
                $scope.usrLinea.codProveedor = registro.codProveedor;
            }
            
        }
        
    }

    $scope.btnConsultaClick = function () {
        debugger;
        var sRuc = '';
        var sNombre = '';
        var criterio;
        if (!$scope.chkTodos) {

            if ($scope.rbtOpciones == 'C') {
                if (angular.isUndefined($scope.txtFiltraCedula) || $scope.txtFiltraCedula == '' || $scope.txtFiltraCedula.length == 0) {
                    $scope.showMessage('I', 'Ingrese código de proveedor.');
                    return;
                }
                sRuc = $scope.txtFiltraCedula;
                var criterio = "<Root CRITERIO='R' NOMBRE='" + sRuc + "'/>"
            }
            else
                if ($scope.rbtOpciones == 'N') {
                    if (angular.isUndefined($scope.txtFiltraNombre) || $scope.txtFiltraNombre == '' || $scope.txtFiltraNombre.length == 0) {
                        $scope.showMessage('I', 'Ingrese razón social del proveedor.');
                        return;
                    }
                    sNombre = $scope.txtFiltraNombre;
                    var criterio = "<Root CRITERIO='N' NOMBRE='" + sNombre + "'/>"
                }
                else {
                    if ($scope.rbtOpciones == 'R') {
                        if (angular.isUndefined($scope.txtFiltraRuc) || $scope.txtFiltraRuc == '' || $scope.txtFiltraRuc.length == 0) {
                            $scope.showMessage('I', 'Ingrese RUC del proveedor.');
                            return;
                        }
                        sNombre = $scope.txtFiltraRuc;
                        var criterio = "<Root CRITERIO='U' NOMBRE='" + sNombre + "'/>"
                    }
                }
        }
        else {
            criterio = "<Root CRITERIO='T' NOMBRE='' RUC='' />";
        }

        $scope.etiTotRegistros = "";

        $scope.myPromise = lineaNegocioService.getBandejaproveedorTolerancia(criterio).then(function (results) {
            $scope.usrsLinea = results.data.root[0];
            if (results.data.success) {
                //$scope.resDgUsrsAdmin = results.data.root[0]; 
                //$scope.usrsLinea = results.data.root[0];
                $scope.resDgUsrsAdmin = results.data.root[0];
                if ($scope.resDgUsrsAdmin.length < 1) {
                    $scope.showMessage('I', 'No existen datos para mostrar.');
                }
                else
                {
                    $scope.etiTotRegistros = $scope.resDgUsrsAdmin.length.toString();
                }
            }
            else {
                $scope.usrsLinea = [];
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }

            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#chkTodos').focus(); }, 150);

        }, function (error) {

            $scope.showMessage('E', "Error en comunicación: getBandejaproveedorTolerancia();" );

           
            
        });
    };


   

    $scope.selectedRowUsrsAdmin = {};

    $scope.selRowUsrsAdmin = function (row) {
        $scope.bloquear = true;
        $scope.usrLinea.codProveedor = row.codProveedor;
        $scope.usrLinea.razonSocial = row.razonSocial;
        $scope.usrLinea.porcentaje = parseInt(row.porcentaje);
        $scope.usrLinea.ruc = row.ruc;
        $('.nav-tabs a[href="#RegistroTolerancia"]').tab('show');

    };

    $scope.nuevo = function () {
        $scope.bloquear = false;
        $scope.usrLinea.codProveedor = "";
        $scope.usrLinea.razonSocial = "";
        $scope.usrLinea.porcentaje = 0;
        $scope.usrLinea.ruc = "";
    }


    function getToleranciaProveedor(empleado){
        
        var criterio = "<Root CRITERIO='C' USUARIO='" + empleado.idUsuario + "'/>";
        var promesa = lineaNegocioService.getToleranciaProveedor(criterio).then(function (results) {
       
            $scope.lineasUsuario = results.data.root[0];

            setEmpleLinea($scope.lineas, $scope.lineasUsuario);
        }, function (error) {
        });

    }

    $scope.marcaLinea = function (item) {

        if (!angular.isUndefined(item.marcado)) {
            item.marcado = !item.marcado;
        }
    };

    function setLineas(lineas) {
        for (var i = 0; i < lineas.length; i++)
            lineas[i].marcado=false;
    };

    function setEmpleLinea(lineas, empleLinea) {
       
        for (var j = 0; j < empleLinea.length; j++)
            for (var i = 0; i < lineas.length; i++)
                if (empleLinea[j].linea == lineas[i].codigo)
                    lineas[i].marcado = true;
    };

    $scope.saveEmplLinea = function () {
        if (!angular.isUndefined($scope.usrLinea)) {
         
            var lineas = getLineasSele();

            var criterio = "<Root CRITERIO='M' USUARIO='" + $scope.usrLinea.idUsuario + "' IDEMPRESA='" + $scope.usrLinea.idEmpresa + "' RUC='" + $scope.usrLinea.ruc + "' LINEAS='" + lineas + "'/>";
            var promesa = lineaNegocioService.getGraba(criterio).then(function (results) {
                
                if(results.data==0){
                    $('#modalLineaNeg').modal('hide');
                    $scope.showMessage('S', "Registro procesado satisfactoriamente.");
                    //getBandeja("<Root CRITERIO='T' NOMBRE='' RUC='' />");
                    $scope.btnConsultaClick();
                    setLineas($scope.lineas);
                }

            }, function (error) {
            });

        }
    }

    function getLineasSele() {
    
        var codigos = "";
        var lineas=[];

        for (var i = 0; i < $scope.lineas.length; i++)
            if ($scope.lineas[i].marcado == true)
                lineas.unshift($scope.lineas[i].codigo);

        if (lineas.length > 0) {
            for (var i = 0, len = lineas.length - 1; i < len; i++)
                codigos += lineas[i] + "|";

            codigos += lineas[lineas.length - 1];
        }
       
        return codigos;
    }
    
    $scope.btnGrabarClick = function () {
        if ($scope.usrAdm.pRuc == '') {
            $scope.showMessage('I', 'Falta el RUC del proveedor.');
            return;
        }
        if ($scope.usrAdm.pNombre == '') {
            $scope.showMessage('I', 'Falta la Razón Social del proveedor.');
            return;
        }
        if ($scope.usrAdm.pCorreoE == '') {
            $scope.showMessage('I', 'Ingrese correctamente el Correo Electrónico del proveedor.');
            return;
        }
        if ($scope.usrAdm.pCelular.length < 10) {
            $scope.showMessage('I', 'Ingrese mínimo 10 caracteres para el número de Celular del proveedor.');
            return;
        }
        if ($scope.usrAdm.pTelefono.length < 7) {
            $scope.showMessage('I', 'Ingrese mínimo 7 caracteres para el número de Teléfono del proveedor.');
            return;
        }
        if ($scope.usrAdm.pUsuario == '') {
            $scope.usrAdm.pClave = generarPassword(8, 0, '');
        }
        $scope.usrAdm.pEstado = $scope.usrAdmCtr.cboEstadoSelItem.codigo;
        $scope.myPromise = SeguridadService.getGrabaUsrAdmin($scope.usrAdm).then(function (results) {
            if (results.data.success) {
           
                $scope.usrAdm.pClave = "";
                if ($scope.usrAdm.pUsuario == '') {
                    $scope.usrAdm.pUsuario = results.data.root[0].pUsuario;//"CERXXXXX"; //----------->>> FALTA
                    $scope.usrAdm.pIdParticipante = results.data.root[0].pIdParticipante;
                    $scope.showMessage('M', 'Usuario generado correctamente.');
                }
                else {
                    $scope.showMessage('M', 'Usuario actualizado correctamente.');
                }
                $scope.EsBloqCambioClave = false;
                $scope.btnConsultaClick();
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

    $scope.tipoConfirma = "";

    $scope.btnCambiarClaveClick = function () {
        $scope.tipoConfirma = "1";
        $scope.MenjConfirmacion = "¿ESTA SEGURO DE CAMBIAR LA CONTRASEÑA DE ESTE PROVEEDOR? " +
            "(Cuando dé click en el botón <<ACTUALIZAR>> del recuadro principal,   " +
            "junto con la actualización de datos se generará una nueva contraseña temporal,   " +
            "que será enviada por correo al Proveedor y se forzará el cambio de la misma en el próximo inicio de sesión)";
        $('#idMensajeConfirmacion').modal('show');
    };

    $scope.grabar = function () {
        if ($scope.tipoConfirma == "1") {
            $scope.usrAdm.pClave = generarPassword(8, 0, '');
            $scope.EsBloqCambioClave = true;
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

    //int largo, int numEsp, string listaCarEsp
    //function generarPassword(largo, numEsp, listaCarEsp) {
    //    var Resultado = "";
    //    var CanNum = false;
    //    var CanLet = false;
    //    var idx = 0;
    //    //Cargamos la matriz con números y letras
    //    var Caracter = [
    //        "0",
    //        "1",
    //        "2",
    //        "3",
    //        "4",
    //        "5",
    //        "6",
    //        "7",
    //        "8",
    //        "9",
    //        "a",
    //        "b",
    //        "c",
    //        "d",
    //        "e",
    //        "f",
    //        "g",
    //        "h",
    //        "i",
    //        "j",
    //        "k",
    //        "l",
    //        "m",
    //        "n",
    //        "o",
    //        "p",
    //        "q",
    //        "r",
    //        "s",
    //        "t",
    //        "u",
    //        "v",
    //        "w",
    //        "x",
    //        "y",
    //        "z"
    //    ];
    //    //Math.random();
    //    while (Resultado.length < (largo - numEsp)) {
    //        idx = Math.floor(36 * Math.random());
    //        Resultado = Resultado + Caracter[idx];
    //        if ((idx < 10))
    //            CanNum = true;
    //        if ((idx > 9))
    //            CanLet = true;
    //    }
    //    if (CanNum == false) {
    //        Resultado = Resultado.substring(0, Resultado.length - 1);
    //        idx = Math.floor(10 * Math.random());
    //        Resultado = Resultado + Caracter[idx];
    //    }
    //    if (CanLet == false) {
    //        Resultado = Resultado.substring(0, Resultado.length - 1);
    //        idx = Math.floor(26 * Math.random()) + 10;
    //        Resultado = Resultado + Caracter[idx];
    //    }
    //    //Math.random();
    //    while (Resultado.length < largo) {
    //        idx = Math.floor(listaCarEsp.length * Math.random());
    //        Resultado = Resultado + listaCarEsp.substring(idx, idx + 1);
    //    }
    //    if (Resultado.length > 2) {
    //        Resultado = Resultado.substring(1, Resultado.length) + Resultado.substring(0, 1);
    //    }
    //    return Resultado;
    //};
    setTimeout(function () { $('#btnConsulta').focus(); }, 100);
    setTimeout(function () { angular.element('#btnConsulta').trigger('click'); }, 150);
}
]);


