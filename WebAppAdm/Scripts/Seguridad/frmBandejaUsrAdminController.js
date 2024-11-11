
//'use strict';
app.controller('frmBandejaUsrAdminController', ['$scope', 'SeguridadService', function ($scope, SeguridadService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    //$scope.getDefaultRow = function () {
    //    return [{
    //        "ruc": "",
    //        "codSAP": "",
    //        "razonSocial": "",
    //        "correoE": "",
    //        "telefono": "",
    //        "celular": "",
    //        "usuario": "",
    //        "estado": "",
    //        "idParticipante": -1
    //    }];
    //}
    $scope.resDgUsrsAdmin = [];
    //$scope.resDgUsrsAdmin = $scope.getDefaultRow();
    var _resDgUsrsAdmin = [];
    $scope.pagesUsrsAdmin = [];
    //var _pagesUsrsAdmin = [];
    $scope.pgcDgUsrsAdmin = [];
    //$scope.pgcDgUsrsAdmin = [].concat($scope.resDgUsrsAdmin);
    //$scope.gridUsrsAdmin = {};

    //for (idx = 0; idx < $scope.resDgUsrsAdmin.length; idx++) {
    //    $scope.resDgUsrsAdmin[idx].estado = $scope.resDgUsrsAdmin[idx].estado;
    //    break;
    //}
    $scope.EsBloqCambioClave = true;
    $scope.rbtOpciones = 'U';
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
    //$scope.usrAdmCtr.ifOpenModal = false;
    $scope.usrAdmCtr.cboEstado = [];
    $scope.usrAdmCtr.cboEstadoSelItem = "";
    $scope.myPromise = SeguridadService.getCatalogo('tbl_EstadoUsuarios').then(function (results) {
        //debugger;
        $scope.usrAdmCtr.cboEstado = results.data;
        $scope.usrAdmCtr.cboEstadoSelItem = results.data[0];
        $scope.cboFiltraEstado = results.data;
        $scope.cboFiltraEstadoSelItem = results.data[0];
    }, function (error) {
    });
    $scope.etiTotRegistros = "";
    ////var myMantUsrAdminFormatter = function (id, cellp, rowData) {
    ////    //var objCtr = "<button class=\"btn btn-xs btn-danger margin-left-5\" onClick=\"selRowUsrsAdmin('" + listCamposGet + "')\"><i class=\"fa fa-edit\"></i></button>";
    ////    var newCtr = "<button class=\"btn btn-xs btn-danger margin-left-5\" id=\"btnUsAd" + cellp.rowId +
    ////        "\" onClick=\"selRowUsrsAdmin();\"><i class=\"fa fa-edit\"></i></button>";
    ////    return newCtr;
    ////};
    //$scope.configUsrsAdmin = {
    //    datatype: "local",
    //    height: '100%',
    //    width: 'auto',
    //    id: 'ruc',
    //    colNames: [ 'RUC', 'COD.SAP', 'RAZÓN SOCIAL', 'E-MAIL', 'USUARIO', 'TELÉFONO', 'CELULAR', 'ESTADO'],
    //    colModel: [
    //                        { name: 'ruc', index: 'ruc', editable: false, width: 150 },
    //                        { name: 'codSAP', index: 'codSAP', editable: false, width: 140 },
    //                        { name: 'razonSocial', index: 'razonSocial', editable: false, width: 300 },
    //                        { name: 'correoE', index: 'correoE', editable: false, width: 160 },
    //                        { name: 'usuario', index: 'usuario', editable: false, width: 150 },
    //                        { name: 'telefono', index: 'telefono', editable: false, width: 150 },
    //                        { name: 'celular', index: 'celular', editable: false, width: 150 },
    //                        { name: 'estado', index: 'estado', editable: false, width: 140 }
    //                        //{ name: 'idParticipante', label: 'Actions', formatter: myMantUsrAdminFormatter, width: 30, fixed: true }
    //                        //{ name: 'idParticipante', index: 'idParticipante', editable: false, width: 1, hidden: true }
    //    ],
    //    onSelectRow: selRowUsrsAdmin,
    //    multiselect: false,
    //    caption: "Usuarios Proveedores Administrativos",
    //    viewrecords: true,
    //    rowNum: 10,
    //    rowList: [10, 20, 30, 40, 50],
    //    autowidth: true
    //};
    $scope.btnConsultaClick = function () {
        var sCodSap = '';
        var sRuc = '';
        var sNombre = '';
        var sConUsuario = '';
        var sEstado = '';
        if ($scope.rbtOpciones == 'U') {
            if ($scope.chkSinUsuario && $scope.chkConUsuario)
                sConUsuario = 'T';
            else if ($scope.chkSinUsuario && !$scope.chkConUsuario)
                sConUsuario = 'N';
            else if (!$scope.chkSinUsuario && $scope.chkConUsuario)
                sConUsuario = 'S';
            else {
                $scope.showMessage('I', 'Seleccione una opción válida.');
                return;
            }
        }
        else if ($scope.rbtOpciones == 'R') {
            if ($scope.txtFiltraRuc == '' || $scope.txtFiltraRuc.length < 13) {
                $scope.showMessage('I', 'Ingrese el los 13 dígitos del RUC.');
                return;
            }
            sRuc = $scope.txtFiltraRuc;
        }
        else if ($scope.rbtOpciones == 'N') {
            if ($scope.txtFiltraNombre == '') {
                $scope.showMessage('I', 'Ingrese la Razón Social.');
                return;
            }
            sNombre = $scope.txtFiltraNombre;
        }
        else if ($scope.rbtOpciones == 'S') {
            if ($scope.txtFiltraCodSap == '') {
                $scope.showMessage('I', 'Ingrese el  Código del Proveedor.');
                return;
            }
            sCodSap = $scope.txtFiltraCodSap;
        }
        else if ($scope.rbtOpciones == 'E') {
            if ($scope.cboFiltraEstadoSelItem == null || $scope.cboFiltraEstadoSelItem == '') {
                $scope.showMessage('I', 'Seleccione el estado.');
                return;
            }
            sEstado = $scope.cboFiltraEstadoSelItem.codigo;
        }
        $scope.etiTotRegistros = "";
        $scope.myPromise = SeguridadService.getConsBandjUsrsAdmin(sCodSap, sRuc, sNombre, sConUsuario, sEstado,"").then(function (results) {
            if (results.data.success) {
                $scope.resDgUsrsAdmin = results.data.root[0];
                if ($scope.resDgUsrsAdmin.length < 1) {
                    $scope.showMessage('I', 'No existen datos para mostrar.');
                }
                else
                {
                    $scope.etiTotRegistros = $scope.resDgUsrsAdmin.length.toString();
                }
                //$scope.pgcDgUsrsAdmin.reload();
                //for (idx = 0; idx < $scope.resDgUsrsAdmin.length; idx++) {
                //    $scope.resDgUsrsAdmin[idx].ruc = $scope.resDgUsrsAdmin[idx].ruc;
                //}
                //for (idx = 0; idx < $scope.pgcDgUsrsAdmin.length; idx++) {
                //    $scope.pgcDgUsrsAdmin[idx].ruc = $scope.pgcDgUsrsAdmin[idx].ruc;
                //}
                //for (idx = 0; idx < $scope.pagesUsrsAdmin.length; idx++) {
                //    $scope.pagesUsrsAdmin[idx][0] = $scope.pagesUsrsAdmin[idx][0];
                //}
                //if ($scope.resDgUsrsAdmin.length < 1) {
                //    $scope.resDgUsrsAdmin = $scope.getDefaultRow();
                //}
                //$scope.resDgUsrsAdmin.reload();
                //$scope.selectedItem
            }
            else {
                $scope.resDgUsrsAdmin = [];
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }

            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 150);


            //$scope.pgcDgUsrsAdmin = [].concat($scope.resDgUsrsAdmin);
            //$scope.gridUsrsAdmin.clear();
            //$scope.gridUsrsAdmin.insert($scope.resDgUsrsAdmin);
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

    $scope.selectedRowUsrsAdmin = {};

    //function selRowUsrsAdmin(rowId) {
    $scope.selRowUsrsAdmin = function (rowId) {
        var ret = rowId; //$scope.resDgUsrsAdmin[rowId - 1];
        if (ret.ruc == "") {
            return;
        }
        $scope.selectedRowUsrsAdmin = ret;
        //debugger;
        $('#MantUsrAdminDialog').modal('show');
        $scope.usrAdm.pRuc = ret.ruc;
        $scope.usrAdm.pNombre = ret.razonSocial;
        $scope.usrAdm.pUsuario = ret.usuario;
        $scope.usrAdm.pCorreoE = ret.correoE;
        $scope.usrAdm.pTelefono = ret.telefono;
        $scope.usrAdm.pCelular = ret.celular;
        $scope.usrAdm.pCodSap = ret.codSAP;
        $scope.usrAdm.pClave = "";
        if (ret.usuario == null || ret.usuario.length < 1)
            $scope.EsBloqCambioClave = true;
        else
            $scope.EsBloqCambioClave = false;
        $scope.usrAdm.pEstado = ret.estado;
        $scope.usrAdm.pIdParticipante = ret.idParticipante;
        $scope.usrAdm.pIdRepresentante = ret.idRepresentante;
        //$scope.usrAdmCtr.ifOpenModal = true;
        debugger;
        for (idx = 0; idx < $scope.usrAdmCtr.cboEstado.length; ++idx) {
            if ($scope.usrAdmCtr.cboEstado[idx].codigo == ret.estado) {
                $scope.usrAdmCtr.cboEstadoSelItem = $scope.usrAdmCtr.cboEstado[idx];
                break;
            }
        }
    };

    //$scope.refrescaModal = function () {
    //    if ($scope.usrAdmCtr.ifOpenModal) {
    //        $scope.usrAdm.pClave = "";
    //        $scope.usrAdmCtr.ifOpenModal = false;
    //    }
    //};
    
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
                //debugger;
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
                debugger;
                $scope.showMessage('E', 'Error al grabar: ' + results.data.msgError);
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
    setTimeout(function () { $('#btnConsulta').focus(); }, 100);
    setTimeout(function () { angular.element('#btnConsulta').trigger('click'); }, 150);
}
]);

app.controller('frmGestionUsuariosController', ['$scope', 'SeguridadService', function ($scope, SeguridadService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.resDgUsrsAdmin = [];
    var _resDgUsrsAdmin = [];
    $scope.pagesUsrsAdmin = [];
    $scope.pgcDgUsrsAdmin = [];

    $scope.rbtOpciones = 'R';
    $scope.txtFiltraRuc = "";
    $scope.txtFiltraNombre = "";

    $scope.User = [];
    $scope.p_User = {};
    $scope.p_User.pRuc = '';
    $scope.p_User.pUsuario = '';
    $scope.p_User.pCedula = '';
    $scope.p_User.pNombres = '';
    $scope.p_User.pApellidos = '';
    $scope.p_User.pCodLegacy = '';
    $scope.p_User.pUserLegacy = '';


    $scope.etiTotRegistros = "";

    $scope.GestionUsuario = function (content) {
        //Limpia
        $scope.limpiadatos();

        //Carga
        if (content != "") {
            $scope.p_User = {};
            $scope.p_User.pRuc = content.pRuc;
            $scope.p_User.pUsuario = content.pUsuario;
            $scope.p_User.pNombres = content.pNombres + ' ' + content.pApellidos;
            $scope.p_User.pCodLegacy = content.pCodLegacy;
            $scope.p_User.pUserLegacy = content.pUserLegacy;
        }

        $('#modal-form').modal('show');
        return false;
    }

    $scope.adicionausuario = function () {
        //Recorre el objeto
        var index;
        for (index = 0; index < $scope.resDgUsrsAdmin.length; index++) {
            //Filtrar de modo que sea unico
            if ($scope.resDgUsrsAdmin[index].pUsuario === $scope.p_User.pUsuario) {
                //Actualiza en la base: Ruc, Usuario, Cedula, CodLegacy, UserLegacy
                $scope.myPromise = SeguridadService.getActDatosLegAsociados($scope.p_User.pRuc, $scope.p_User.pUsuario, $scope.resDgUsrsAdmin[index].pCedula,
                                                                            $scope.p_User.pCodLegacy, $scope.p_User.pUserLegacy).then(function (results) {
                                                                                if (results.data.success) {
                                                                                    $scope.limpiadatos();
                                                                                    $scope.showMessage('M', 'Usuario actualizado correctamente.');
                                                                                    $('#modal-form').modal('hide');
                                                                                }
                                                                                else {
                                                                                    debugger;
                                                                                    $scope.showMessage('E', 'Error al grabar: ' + results.data.msgError);
                                                                                }
                                                                            }, function (error) {
                                                                                debugger;
                                                                                var errors = [];
                                                                                for (var key in error.data.modelState) {
                                                                                    for (var i = 0; i < error.data.modelState[key].length; i++) {
                                                                                        errors.push(error.data.modelState[key][i]);
                                                                                    }
                                                                                }
                                                                                $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
                                                                            });
                //Actualiza los datos del Grid
                $scope.resDgUsrsAdmin[index].pCodLegacy = $scope.p_User.pCodLegacy;
                $scope.resDgUsrsAdmin[index].pUserLegacy = $scope.p_User.pUserLegacy;
                break;
            }
        }
    };

    $scope.limpiadatos = function () {
        $scope.p_User = {
            pRuc: '',
            pUsuario: '',
            pCedula: '',
            pNombres: '',
            pApellidos: '',
            pCodLegacy: '',
            pUserLegacy: ''
        }
    }

    $scope.btnConsultaClick = function () {
        var sRuc = '';
        var sNombre = '';

        if ($scope.rbtOpciones == 'R') {
            if ($scope.txtFiltraRuc == '' || $scope.txtFiltraRuc.length < 13) {
                $scope.showMessage('I', 'Ingrese los 13 dígitos del RUC.');
                return;
            }
            sRuc = $scope.txtFiltraRuc;
        }
        else if ($scope.rbtOpciones == 'N') {
            if ($scope.txtFiltraNombre == '') {
                $scope.showMessage('I', 'Ingrese el Nombre.');
                return;
            }
            sNombre = $scope.txtFiltraNombre;
        }

        $scope.etiTotRegistros = "";

        $scope.myPromise = SeguridadService.getConsDatosLegAsociados(sRuc, sNombre).then(function (results) {
            if (results.data.success) {

                $scope.resDgUsrsAdmin = results.data.root[0];

                if ($scope.resDgUsrsAdmin.length < 1) {
                    $scope.showMessage('I', 'No existen datos para mostrar.');
                }
                else {
                    $scope.etiTotRegistros = $scope.resDgUsrsAdmin.length.toString();
                }
            }
            else {
                $scope.resDgUsrsAdmin = [];
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }

            setTimeout(function () { $('#btnConsultaUs').focus(); }, 100);
            setTimeout(function () { $('#rbtPorRuc').focus(); }, 150);
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

    //setTimeout(function () { $('#btnConsultaUs').focus(); }, 100);
    //setTimeout(function () { angular.element('#btnConsulta').trigger('click'); }, 150);
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
