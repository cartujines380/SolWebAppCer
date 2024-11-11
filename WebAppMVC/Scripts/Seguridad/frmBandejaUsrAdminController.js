app.controller('frmCambioCorreoController', ['$scope', 'SeguridadAdmin', 'authService', function ($scope, SeguridadAdmin, authService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;


    $scope.resDgUsrsAdmin = [];
    //$scope.resDgUsrsAdmin = $scope.getDefaultRow();
    var _resDgUsrsAdmin = [];
    $scope.pagesUsrsAdmin = [];
    //var _pagesUsrsAdmin = [];
    $scope.pgcDgUsrsAdmin = [];

    $scope.EsBloqCambioClave = true;
    $scope.rbtOpciones = 'U';
    $scope.chkSinUsuario = true;
    $scope.chkConUsuario = true;
    $scope.txtFiltraRuc = "";
    $scope.txtFiltraNombre = "";
    $scope.txtFiltraCodSap = "";
    $scope.cboFiltraEstado = [];
    $scope.cboFiltraEstadoSelItem = "";
    $scope.usrAdmCorreo = {};
    $scope.usrAdmCorreo.pRuc = "";
    $scope.usrAdmCorreo.pNombre = "";
    $scope.usrAdmCorreo.pUsuario = "";
    $scope.usrAdmCorreo.pClave = "";
    $scope.usrAdmCorreo.pCorreoE = "";
    $scope.usrAdmCorreo.pTelefono = "";
    $scope.usrAdmCorreo.pCelular = "";
    $scope.usrAdmCorreo.pCodSap = "";
    $scope.usrAdmCorreo.pIdParticipante = parseInt("-1", 10);
    $scope.usrAdmCorreo.pEstado = "";
    $scope.usrAdmCorreo.pIdRepresentante = "";
    $scope.usrAdmCtr = {};
    //$scope.usrAdmCtr.ifOpenModal = false;
    $scope.usrAdmCtr.cboEstado = [];
    $scope.usrAdmCtr.cboEstadoSelItem = "";
    $scope.myPromise = SeguridadAdmin.getCatalogo('tbl_EstadoUsuarios').then(function (results) {
        //debugger;
        $scope.usrAdmCtr.cboEstado = results.data;
        $scope.usrAdmCtr.cboEstadoSelItem = results.data[0];
        $scope.cboFiltraEstado = results.data;
        $scope.cboFiltraEstadoSelItem = results.data[0];
    }, function (error) {
    });
    $scope.etiTotRegistros = "";

    $scope.selectedRowUsrsAdmin = {};
    $scope.btnCancelar = function () {
        window.location = "../Notificacion/frmVisualizaNotificaciones"; //"/Account/Frmloginprov";
    };
    //function selRowUsrsAdmin(rowId) {
    $scope.carga = function () {

        $('#MantUsrAdminDialog').modal('show');
        $scope.usrAdmCorreo.pRuc = authService.authentication.ruc;
        $scope.usrAdmCorreo.pNombre = authService.authentication.NombreParticipante;
        $scope.usrAdmCorreo.pUsuario = authService.authentication.Usuario;
        //$scope.usrAdmCorreo.pCorreoE = authService.authentication.CorreoE;
        //$scope.usrAdmCorreo.pTelefono = authService.authentication.Celular;
        //$scope.usrAdmCorreo.pCelular =  authService.authentication.Celular;
        $scope.usrAdmCorreo.pCodSap = authService.authentication.CodSAP;
        $scope.usrAdmCorreo.pClave = "";
        if (authService.authentication.Usuario == null || authService.authentication.Usuario.length < 1)
            $scope.EsBloqCambioClave = true;
        else
            $scope.EsBloqCambioClave = false;
        $scope.usrAdmCorreo.pEstado = "A";
        $scope.usrAdmCorreo.pIdParticipante = authService.authentication.IdParticipante;
        //$scope.usrAdm.pIdRepresentante = ret.idRepresentante;
        ////$scope.usrAdmCtr.ifOpenModal = true;
        for (idx = 0; idx < $scope.usrAdmCtr.cboEstado.length; ++idx) {
            if ($scope.usrAdmCtr.cboEstado[idx].codigo == "A") {
                $scope.usrAdmCtr.cboEstadoSelItem = $scope.usrAdmCtr.cboEstado[idx];
                break;
            }
        }

        $scope.myPromise = SeguridadAdmin.getConsBandjUsrsAdmin("", "", "", "", "", authService.authentication.Usuario).then(function (results) {
            if (results.data.success) {
                //debugger;
                $scope.resDgUsrsAdmin = results.data.root[0];
                $scope.usrAdmCorreo.pCorreoE = $scope.resDgUsrsAdmin[0].correoE;
                $scope.usrAdmCorreo.pTelefono = $scope.resDgUsrsAdmin[0].telefono;
                $scope.usrAdmCorreo.pCelular = $scope.resDgUsrsAdmin[0].celular;
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
    }




    $scope.btnGrabarClick = function () {
        if ($scope.usrAdmCorreo.pRuc == '') {
            $scope.showMessage('I', 'Falta el RUC del proveedor.');
            return;
        }
        if ($scope.usrAdmCorreo.pNombre == '') {
            $scope.showMessage('I', 'Falta la Razón Social del proveedor.');
            return;
        }
        if ($scope.usrAdmCorreo.pCorreoE == '') {
            $scope.showMessage('I', 'Ingrese correctamente el Correo Electrónico del proveedor.');
            return;
        }
        if ($scope.usrAdmCorreo.pCelular.length < 10) {
            $scope.showMessage('I', 'Ingrese mínimo 10 caracteres para el número de Celular del proveedor.');
            return;
        }
        if ($scope.usrAdmCorreo.pTelefono.length < 7) {
            $scope.showMessage('I', 'Ingrese mínimo 7 caracteres para el número de Teléfono del proveedor.');
            return;
        }
        if ($scope.usrAdmCorreo.pUsuario == '') {
            $scope.usrAdmCorreo.pClave = generarPassword(8, 0, '');
        }
        $scope.usrAdmCorreo.pEstado = $scope.usrAdmCtr.cboEstadoSelItem.codigo;
        $scope.myPromise = SeguridadAdmin.getGrabaUsrAdmin($scope.usrAdmCorreo).then(function (results) {
            if (results.data.success) {
                //debugger;
                $scope.usrAdmCorreo.pClave = "";
                if ($scope.usrAdmCorreo.pUsuario == '') {
                    $scope.usrAdmCorreo.pUsuario = results.data.root[0].pUsuario;//"CERXXXXX"; //----------->>> FALTA
                    $scope.usrAdmCorreo.pIdParticipante = results.data.root[0].pIdParticipante;
                    $scope.showMessage('M', 'Usuario generado correctamente.');
                }
                else {
                    $scope.showMessage('M', 'Usuario actualizado correctamente.');
                    authService.correo("1", $scope.usrAdmCorreo.pCorreoE, $scope.usrAdmCorreo.pTelefono);
                    //authService.authentication.Celular = $scope.usrAdmCorreo.pTelefono;
                    //authService.authentication.Celular = $scope.usrAdmCorreo.pCelular;
                }
                $scope.EsBloqCambioClave = false;
            }
            else {
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
    $scope.carga();
}
]);