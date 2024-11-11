app.controller('groupCtrl', function ($scope) {
    console.log($scope.group);
    if ($scope.group.value != 'Rol Comercial')
        $scope.group.$hideRows = true;
    console.log($scope.group);
});

app.controller('frmBandejaUsrAdicController', ['$scope', 'SeguridadService', '$filter', 'authService', function ($scope, SeguridadService, $filter, authService) {
    //app.controller('frmBandejaUsrAdicController', ['$scope', 'SeguridadService', '$filter', 'ngTableParams', 'authService', function ($scope, SeguridadService,  $filter,ngTableParams, authService) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.etiTotRegistros = "";
    $scope.resDgUsrsAdic = [];
    $scope.resDgUsrsAdicNew = [];
    $scope.usrAsoc = {};
    $scope.usrAsoc.apellido = "";
    $scope.usrAsoc.celular = "";
    $scope.usrAsoc.clave = "";
    $scope.usrAsoc.correoE = "";
    $scope.usrAsoc.estado = "";
    $scope.usrAsoc.identificacion = "";
    $scope.usrAsoc.nombre = "";
    $scope.usrAsoc.usrAutorizador = "";
    $scope.usrAsoc.usuario = "";
    $scope.usrAsoc.rolAdmin = false;
    $scope.usrAsoc.rolComercial = false;
    $scope.usrAsoc.rolContable = false;
    $scope.usrAsoc.rolLogistico = false;
    $scope.allChecksSel = false;
    $scope.recActasSN = true;
    $scope.pRazonSocial = "";
    $scope.pApoderado = "";

    var _resDgUsrsAdic = [];
    $scope.pagesUsrsAdic = [];
    $scope.pgcDgUsrsAdic = [];

    //$scope.ListDgRolDisp = [];
    $scope.ListDgRoles = [];

    //$scope.ListDgZonaDisp = [];
    $scope.ListDgZonas = [];
    $scope.ListDgAlmacenes = [];
    $scope.ListDgAlmacenesT = [];
    $scope.ListDgAlmacenesR = [];

    $scope.esNuevo = true;

    $scope.usrAdic = {};

    //Inicio Filtro de busqueda
    $scope.txtusuario = "";
    $scope.txtnombre = "";
    $scope.txtapellido = "";
    $scope.cmbestado = [];
    $scope.cmbactas = [];
    $scope.cmbestadolistado = [];

    //Fin Filtro de Busqueda
    $scope.DepartaContacto = [];
    $scope.FuncionContacto = [];

    $scope.usrAdic.pRuc = "";
    $scope.usrAdic.pUsuario = "";
    $scope.usrAdic.pIdParticipante = "";
    $scope.usrAdic.pCodSap = "";
    $scope.usrAdic.pCorreoE = "";
    $scope.usrAdic.pTelefono = "";
    $scope.usrAdic.pDepartamento = "";
    $scope.usrAdic.pCelular = "";
    $scope.usrAdic.pEstado = "";
    $scope.usrAdic.pDepartamento = "";
    $scope.usrAdic.pFuncion = "";
    $scope.usrAdic.pTipoIdentificacion = "";
    $scope.desCiudad = "";
    $scope.usrAdic.pClave = "";
    $scope.usrAdic.pTipoIdent = "C";
    $scope.usrAdic.pIdentificacion = "";
    $scope.usrAdic.pApellido1 = "";
    $scope.usrAdic.pApellido2 = "";
    $scope.usrAdic.pNombre1 = "";
    $scope.usrAdic.pNombre2 = "";
    $scope.usrAdic.pEstadoCivil = "S";
    $scope.usrAdic.pGenero = "M";
    $scope.usrAdic.pPais = "EC";
    $scope.usrAdic.pProvincia = "";
    $scope.usrAdic.pCiudad = "";
    $scope.usrAdic.pDireccion = "N/A";
    $scope.usrAdic.pRecActas = "";
    $scope.usrAdic.pRazonSocial = "";
    $scope.usrAdic.pApoderado = "";

    $scope.usrAdic.pListRol = [];
    $scope.usrAdic.pListZona = [];
    $scope.usrAdic.pListAlm = [];



    $scope.ListDepartaContacto = [];
    $scope.ListFuncionContacto = [];
    $scope.ListFuncionContactoT = [];
    $scope.ctrls = {};
    $scope.cboAlmacenList1T = [];
    $scope.cboAlmacenList1 = [];
    $scope.cmbrecibeActas = [];

    $scope.myPromise = SeguridadService.getConsAlmacenes("7").then(function (results) {
        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            $scope.cboAlmacenList1T = listAlmacen;
            //ListDgAlmacenes = listAlmacen;
            for (var idx = 0; idx < $scope.cboAlmacenList1T.length; idx++) {
                //var update = $scope.cboAlmacenList1T[idx];
                $scope.cboAlmacenList1T[idx].pCodCiudad = $scope.cboAlmacenList1T[idx].pCodCiudad;
                $scope.ListDgAlmacenesT.push({
                    pCodAlmacen: $scope.cboAlmacenList1T[idx].pCodAlmacen,
                    pNomAlmacen: $scope.cboAlmacenList1T[idx].pNomAlmacen,
                    pCodCiudad: $scope.cboAlmacenList1T[idx].pCodCiudad,
                    pElegir: false
                });

                //$scope.ListDgAlmacenes[idx].pCodCiudad = parseInt($scope.ListDgAlmacenes[idx].pCodCiudad);
                //$scope.ListDgAlmacenes[idx].pIsCheck = false;
            }
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: getConsAlmacenes().");
    });




    $scope.ctrls.cboEstadoList = [];
    $scope.ctrls.cboEstadoSelItem = null;
    $scope.myPromise = SeguridadService.getCatalogo('tbl_EstadoUsuarios').then(function (results) {
        $scope.ctrls.cboEstadoList = results.data;
        $scope.ctrls.cboEstadoSelItem = results.data[0];
        $scope.cmbestado = results.data[0];
        $scope.cmbestadolistado = results.data;
    }, function (error) {
    });

    $scope.myPromise = SeguridadService.getCatalogo('tbl_DespachaProvincia').then(function (results) {
        $scope.cmbrecibeActas = results.data;
        
        $scope.cmbrecibeActas.splice(0, 0, { codigo: "T", detalle: "Todos", descalterno: "" });
        $scope.cmbactas = results.data[0];
    }, function (error) {
    });

    $scope.myPromise = SeguridadService.getCatalogo('tbl_DepartaContacto').then(function (results) {
        $scope.ListDepartaContacto = results.data;
    }, function (error) {
    });

    $scope.myPromise = SeguridadService.getCatalogo('tbl_FuncionContacto').then(function (results) {
        $scope.ListFuncionContactoT = results.data;
    }, function (error) {
    });

    //luis chacha 
    //-->Catalogo: Tipo de Identificación
    $scope.ctrls.cboTipoIdentificacionList = [];
    $scope.ctrls.cboTipoIdentificacionSelItem = null;
    $scope.myPromise = SeguridadService.getCatalogo('tbl_TipoIdentificacion').then(function (results) {
        $scope.ctrls.cboTipoIdentificacionList = results.data;
        $scope.ctrls.cboTipoIdentificacionSelItem = results.data[0];
    }, function (error) {
    });
    //--<Catalogo: Tipo de Identificación
    //luis chacha

    $scope.$watch('usrAdic.pNombre1', function (val) {
        $scope.usrAdic.pNombre1 = $filter('uppercase')(val);
    }, true);
    $scope.$watch('usrAdic.pApellido1', function (val) {
        $scope.usrAdic.pApellido1 = $filter('uppercase')(val);
    }, true);
    //Filtrar funciones por departamentos
    $scope.$watch('DepartaContacto', function () {
        if ($scope.DepartaContacto == null) { $scope.ListFuncionContacto = []; return; };
        if ($scope.DepartaContacto.length == 0) { $scope.ListFuncionContacto = []; return; };
        $scope.ListFuncionContacto = [];
        $scope.ListFuncionContacto = $filter('filter')($scope.ListFuncionContactoT, { descAlterno: $scope.DepartaContacto.codigo }, true);

        if ($scope.DepartaContacto.codigo == "0006")
            $scope.recActasSN = false;
        else {
            $scope.recActasSN = true;
            $scope.usrAdic.pRecActas = false;
        }


    });
    //$scope.ctrls.cboTipoIdentList = [];
    //$scope.ctrls.cboTipoIdentSelItem = null;
    //$scope.myPromise = SeguridadService.getCatalogo('tbl_tipoIdentificacionSys').then(function (results) {
    //    $scope.ctrls.cboTipoIdentList = results.data;
    //    $scope.ctrls.cboTipoIdentSelItem = results.data[0];
    //}, function (error) {
    //});

    //$scope.ctrls.cboEstCivilList = [];
    //$scope.ctrls.cboEstCivilSelItem = null;
    //$scope.myPromise = SeguridadService.getCatalogo('tbl_EstadoCivil').then(function (results) {
    //    $scope.ctrls.cboEstCivilList = results.data;
    //    $scope.ctrls.cboEstCivilSelItem = results.data[0];
    //}, function (error) {
    //});

    //$scope.ctrls.cboGeneroList = [];
    //$scope.ctrls.cboGeneroSelItem = null;
    //$scope.myPromise = SeguridadService.getCatalogo('tbl_Genero').then(function (results) {
    //    $scope.ctrls.cboGeneroList = results.data;
    //    $scope.ctrls.cboGeneroSelItem = results.data[0];
    //}, function (error) {
    //});

    //$scope.ctrls.cboPaisList = [];
    //$scope.ctrls.cboPaisSelItem = null;
    //$scope.myPromise = SeguridadService.getCatalogo('tbl_Pais').then(function (results) {
    //    $scope.ctrls.cboPaisList = results.data;
    //    //$scope.ctrls.cboPaisSelItem = results.data[0];
    //}, function (error) {
    //});

    //$scope.ctrls.cboRegionList = [];
    //$scope.ctrls.cboRegionListTmp = [];
    //$scope.ctrls.cboRegionSelItem = null;
    //$scope.myPromise = SeguridadService.getCatalogo('tbl_Region').then(function (results) {
    //    $scope.ctrls.cboRegionList = results.data;
    //    //$scope.ctrls.cboRegionSelItem = results.data[0];
    //}, function (error) {
    //});

    $scope.ctrls.cboRegionList = [];
    $scope.ctrls.cboRegionSelItem = null;
    $scope.myPromise = SeguridadService.getCatalogo('tbl_Provincias').then(function (results) {
        $scope.ctrls.cboRegionList = results.data;
        //$scope.ctrls.cboRegionList = $filter('filter')(results.data, { descAlterno: $scope.usrAdic.pPais });
        //$scope.ctrls.cboRegionSelItem = results.data[0];
    }, function (error) {
    });

    $scope.ctrls.cboCiudadList = [];
    $scope.ctrls.cboCiudadListTmp = [];
    $scope.ctrls.cboCiudadSelItem = null;
    $scope.myPromise = SeguridadService.getCatalogo('tbl_cantones').then(function (results) {
        $scope.ctrls.cboCiudadList = results.data;
        //$scope.ctrls.cboCiudadSelItem = results.data[0];
    }, function (error) {
    });

    //$scope.$watch('ctrls.cboPaisSelItem', function () {
    //    $scope.ctrls.cboRegionListTmp = [];
    //    if ($scope.ctrls.cboPaisSelItem != null && angular.isUndefined($scope.ctrls.cboPaisSelItem) != true) {
    //        $scope.ctrls.cboRegionListTmp = $filter('filter')($scope.ctrls.cboRegionList,
    //                                          { descAlterno: $scope.ctrls.cboPaisSelItem.codigo });
    //    }
    //});

    $scope.$watch('ctrls.cboRegionSelItem', function () {
        $scope.ctrls.cboCiudadListTmp = [];
        if ($scope.ctrls.cboRegionSelItem != null && angular.isUndefined($scope.ctrls.cboRegionSelItem) != true) {
            $scope.ctrls.cboCiudadListTmp = $filter('filter')($scope.ctrls.cboCiudadList,
                                              { descAlterno: $scope.ctrls.cboRegionSelItem.codigo });
        }
    });

    //$scope.myPromise = SeguridadService.getConsTodasZonas('').then(function (results) {
    //    $scope.ListDgZonaDisp = results.data.root[0];
    //    if (results.data.root[0] != null) {
    //        for (var i = 0; i < $scope.ListDgZonaDisp.length; i++) {
    //            $scope.ListDgZonaDisp[i].pElegir = false;
    //        }
    //    }
    //}, function (error) {
    //});
    $scope.myPromise = SeguridadService.getConsTodasZonas('').then(function (results) {
        $scope.ListDgZonas = results.data.root[0];
        if (results.data.root[0] != null) {
            for (var i = 0; i < $scope.ListDgZonas.length; i++) {
                $scope.ListDgZonas[i].pElegir = false;
            }
        }
    }, function (error) {
    });


    //$scope.myPromise = SeguridadService.getConsTodosRoles('').then(function (results) {
    //    $scope.ListDgRolDisp = results.data.root[0];
    //    if (results.data.root[0] != null) {
    //        for (var i = 0; i < $scope.ListDgRolDisp.length; i++) {
    //            $scope.ListDgRolDisp[i].pElegir = false;
    //        }
    //    }
    //}, function (error) {
    //});
    $scope.myPromise = SeguridadService.getConsTodosRoles('').then(function (results) {
        $scope.ListDgRoles = results.data.root[0];
        if (results.data.root[0] != null) {
            for (var i = 0; i < $scope.ListDgRoles.length; i++) {
                $scope.ListDgRoles[i].pElegir = false;
            }
        }
    }, function (error) {
    });

    function datos() {
        //$scope.tableParams = new ngTableParams({

        //    page: 1,            // show first page
        //    count: 10          // count per page
        //}, {
        //    groupBy: 'rol',
        //    total: $scope.resDgUsrsAdic.length,
        //    getData: function ($defer, params) {
        //        var orderedData = params.sorting() ?
        //                $filter('orderBy')($scope.resDgUsrsAdic, $scope.tableParams.orderBy()) :
        //                $scope.resDgUsrsAdic;

        //        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
        //    }
        //});
    }



    $scope.cancelaAlm = function () {
        $scope.ListDgAlmacenesT = angular.copy($scope.ListDgAlmacenesR);
        $scope.ListDgAlmacenesR = [];
        $("#selAlmc").prop("checked", "");
        $('#almacenesLista').modal('hide');
    }


    $scope.selAlmacen = function (codciudad, desciudad) {

        $scope.allChecksSel = false;
        $scope.ListDgAlmacenesR = [];
        $scope.ListDgAlmacenesR = angular.copy($scope.ListDgAlmacenesT);
        $scope.ListDgAlmacenes = $filter('filter')($scope.ListDgAlmacenesT, { pCodCiudad: codciudad }, true);
        $scope.desCiudad = desciudad;
        $("#selAlmc").prop("checked", "");
        $('#almacenesLista').modal('show');
    }

    $scope.muestraAlmacenes = function (valor, codciudad, desciudad) {

        $scope.ListDgAlmacenesR = [];
        $scope.ListDgAlmacenesR = angular.copy($scope.ListDgAlmacenesT);
        if (valor) {
            $scope.desCiudad = desciudad;
            //$scope.ListDgAlmacenes = [];

            $scope.ListDgAlmacenes = $filter('filter')($scope.ListDgAlmacenesT, { pCodCiudad: codciudad }, true);
            $("#selAlmc").prop("checked", "");
            $('#almacenesLista').modal('show');

        }
        else {
            //$scope.ListDgAlmacenes = $filter('filter')($scope.ListDgAlmacenesT, { pCodCiudad: parseInt(codciudad) }, true);
            for (var idx = 0; idx < $scope.ListDgAlmacenesT.length; idx++) {
                if ($scope.ListDgAlmacenesT[idx].pCodCiudad == codciudad)
                    $scope.ListDgAlmacenesT[idx].pElegir = false;
            }
        }
    }

    $scope.actualizaAlmacenes = function () {

        //debugger;
        //for (var i = 0 ; i < $scope.ListDgAlmacenesT.length ; i++)
        //{
        //    var update = $scope.ListDgAlmacenesT[i];
        //    var reg = $filter('filter')($scope.ListDgAlmacenes, { pCodCiudad: parseInt(update.pCodCiudad), pCodAlmacen: update.pCodAlmacen }, true);
        //    if(reg.length >= 1)
        //        $scope.ListDgAlmacenesT[i].pElegir = reg.pElegir;

        //}
        var reg = $filter('filter')($scope.ListDgAlmacenes, { pElegir: true }, true);
        if (reg.length < 1) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
        }
        else {
            $('#almacenesLista').modal('hide');
        }


    }
    $scope.btnConsultaClickdespues = function () {
        $scope.myPromise = SeguridadService.getConsBandjUsrsAdic($scope.usrAdic.pRuc, $scope.txtusuario, $scope.txtnombre, $scope.txtapellido, $scope.cmbestado.codigo).then(function (results) {
            if (results.data.success) {
                $scope.resDgUsrsAdic = results.data.root[0];
                var listUsr = $scope.resDgUsrsAdic.slice();
                var ingresaUsr = true;
                $scope.resDgUsrsAdicNew = [];
                for (var idx = 0 ; idx < listUsr.length; idx++) {
                    ingresaUsr = true;
                    for (var i = 0, len = $scope.resDgUsrsAdicNew.length; i < len; i++) {
                        var update = $scope.resDgUsrsAdicNew[i];

                        if (update.usuario == listUsr[idx].usuario) {
                            ingresaUsr = false;
                            if (listUsr[idx].idRol == "24") {
                                update.rolAdmin = true;

                            }
                            if (listUsr[idx].idRol == "25") {

                                update.rolContable = true;

                            }
                            if (listUsr[idx].idRol == "26") {

                                update.rolLogistico = true;
                            }
                            if (listUsr[idx].idRol == "27") {

                                update.rolComercial = true;
                            }
                        }
                    }

                    if (ingresaUsr) {
                        $scope.usrAsoc = {};
                        
                        $scope.usrAsoc.apellido = listUsr[idx].apellido;
                        $scope.usrAsoc.celular = listUsr[idx].celular;
                        $scope.usrAsoc.clave = listUsr[idx].clave;
                        $scope.usrAsoc.correoE = listUsr[idx].correoE;
                        $scope.usrAsoc.estado = listUsr[idx].estado;
                        $scope.usrAsoc.identificacion = listUsr[idx].identificacion;
                        $scope.usrAsoc.nombre = listUsr[idx].nombre;
                        $scope.usrAsoc.usrAutorizador = listUsr[idx].usrAutorizador;
                        $scope.usrAsoc.usuario = listUsr[idx].usuario;
                        $scope.usrAsoc.recActas = listUsr[idx].recActas;
                        $scope.usrAsoc.rolAdmin = false;
                        $scope.usrAsoc.rolComercial = false;
                        $scope.usrAsoc.rolContable = false;
                        $scope.usrAsoc.rolLogistico = false;
                        if (listUsr[idx].idRol == "24")
                            $scope.usrAsoc.rolAdmin = true;
                        if (listUsr[idx].idRol == "27")
                            $scope.usrAsoc.rolComercial = true;
                        if (listUsr[idx].idRol == "25")
                            $scope.usrAsoc.rolContable = true;
                        if (listUsr[idx].idRol == "26")
                            $scope.usrAsoc.rolLogistico = true;
                        $scope.resDgUsrsAdicNew.push($scope.usrAsoc);
                    }



                }
                $scope.resDgUsrsAdic = [];
               

                $scope.resDgUsrsAdic = $scope.resDgUsrsAdicNew.slice();

                $scope.etiTotRegistros = $scope.resDgUsrsAdic.length.toString();
                if ($scope.etiTotRegistros == 0) {
                }
                datos();
            }
            else {
                $scope.resDgUsrsAdic = [];
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            setTimeout(function () { $('#ctrFocus').focus(); }, 150);
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
    $scope.btnConsultaClick = function () {
        //Filtros recibe actas
        if ($scope.cmbactas.codigo == undefined) $scope.cmbactas.codigo = "T";
        else
        {
            if ($scope.cmbactas.codigo == "S") $scope.cmbactas.codigo = "1";
            if ($scope.cmbactas.codigo == "N") $scope.cmbactas.codigo = "0";
        }

        $scope.myPromise = SeguridadService.getConsBandjUsrsAdic($scope.usrAdic.pRuc, $scope.txtusuario, $scope.txtnombre, $scope.txtapellido, $scope.cmbestado.codigo, $scope.cmbactas.codigo).then(function (results) {
            if (results.data.success) {
                $scope.resDgUsrsAdic = results.data.root[0];
                $scope.pRazonSocial = results.data.root[2];
                $scope.pApoderado = results.data.root[1];
                var listUsr = $scope.resDgUsrsAdic.slice();
                var ingresaUsr = true;
                $scope.resDgUsrsAdicNew = [];
                for (var idx = 0 ; idx < listUsr.length; idx++) {
                    ingresaUsr = true;
                    for (var i = 0, len = $scope.resDgUsrsAdicNew.length; i < len; i++) {
                        var update = $scope.resDgUsrsAdicNew[i];

                        if (update.usuario == listUsr[idx].usuario) {
                            ingresaUsr = false;
                            if (listUsr[idx].idRol == "24") {
                                update.rolAdmin = true;

                            }
                            if (listUsr[idx].idRol == "25") {

                                update.rolContable = true;

                            }
                            if (listUsr[idx].idRol == "26") {

                                update.rolLogistico = true;
                            }
                            if (listUsr[idx].idRol == "27") {

                                update.rolComercial = true;
                            }
                        }
                    }

                    if (ingresaUsr) {
                        $scope.usrAsoc = {};
                        $scope.usrAsoc.apellido = listUsr[idx].apellido;
                        $scope.usrAsoc.celular = listUsr[idx].celular;
                        $scope.usrAsoc.clave = listUsr[idx].clave;
                        $scope.usrAsoc.correoE = listUsr[idx].correoE;
                        $scope.usrAsoc.estado = listUsr[idx].estado;
                        $scope.usrAsoc.identificacion = listUsr[idx].identificacion;
                        $scope.usrAsoc.nombre = listUsr[idx].nombre;
                        $scope.usrAsoc.usrAutorizador = listUsr[idx].usrAutorizador;
                        $scope.usrAsoc.usuario = listUsr[idx].usuario;
                        $scope.usrAsoc.recActas = listUsr[idx].recActas;
                        $scope.usrAsoc.rolAdmin = false;
                        $scope.usrAsoc.rolComercial = false;
                        $scope.usrAsoc.rolContable = false;
                        $scope.usrAsoc.rolLogistico = false;
                        if (listUsr[idx].idRol == "24")
                            $scope.usrAsoc.rolAdmin = true;
                        if (listUsr[idx].idRol == "27")
                            $scope.usrAsoc.rolComercial = true;
                        if (listUsr[idx].idRol == "25")
                            $scope.usrAsoc.rolContable = true;
                        if (listUsr[idx].idRol == "26")
                            $scope.usrAsoc.rolLogistico = true;
                        $scope.resDgUsrsAdicNew.push($scope.usrAsoc);
                    }



                }
                $scope.resDgUsrsAdic = [];
                $scope.resDgUsrsAdic = $scope.resDgUsrsAdicNew.slice();


                $scope.etiTotRegistros = $scope.resDgUsrsAdic.length.toString();
                if ($scope.etiTotRegistros == 0) {
                    $scope.showMessage('I', 'No exite datos para su consulta.');
                }
                datos();
            }
            else {
                $scope.resDgUsrsAdic = [];
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            setTimeout(function () { $('#ctrFocus').focus(); }, 150);
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

    //$scope.btnAddZonaClick = function (row) {
    //    for (var i = 0; i < $scope.ListDgZonaDisp.length; i++) {
    //        if ($scope.ListDgZonaDisp[i].pElegir) {
    //            var siIngZona = true;
    //            for (var x = 0; x < $scope.usrAdic.pListZona.length; x++) {
    //                if ($scope.usrAdic.pListZona[x].pCodZona == $scope.ListDgZonaDisp[i].pCodZona)
    //                {
    //                    siIngZona = false;
    //                    break;
    //                }
    //            };
    //            if (siIngZona) {
    //                $scope.usrAdic.pListZona.push({
    //                    pCodZona: $scope.ListDgZonaDisp[i].pCodZona,
    //                    pDescripcion: $scope.ListDgZonaDisp[i].pDescripcion
    //                });
    //            }
    //        }
    //    }
    //};

    //$scope.eliminarRowZona = function (row) {
    //    $scope.usrAdic.pListZona.splice($scope.usrAdic.pListZona.indexOf(row), 1);
    //};

    //$scope.btnAddRolClick = function (row) {
    //    for (var i = 0; i < $scope.ListDgRolDisp.length; i++) {
    //        if ($scope.ListDgRolDisp[i].pElegir) {
    //            var siIngRol = true;
    //            for (var x = 0; x < $scope.usrAdic.pListRol.length; x++) {
    //                if ($scope.usrAdic.pListRol[x].pIdRol == $scope.ListDgRolDisp[i].pIdRol) {
    //                    siIngRol = false;
    //                    break;
    //                }
    //            };
    //            if (siIngRol) {
    //                $scope.usrAdic.pListRol.push({
    //                    pIdRol: $scope.ListDgRolDisp[i].pIdRol,
    //                    pDescripcion: $scope.ListDgRolDisp[i].pDescripcion
    //                });
    //            }
    //        }
    //    }
    //};
    //$scope.eliminarRowRol = function (row) {
    //    $scope.usrAdic.pListRol.splice($scope.usrAdic.pListRol.indexOf(row), 1);
    //};

    $scope.cambio = function () {
        //$scope.resDgUsrsAdic = [];
        //$scope.pgcDgUsrsAdic = [];
    }
    $scope.eliminarClickUsuario = function (row, tipo) {
        $scope.myPromise = SeguridadService.getConsActivarusuario($scope.usrAdic.pRuc, row.usuario, tipo).then(function (results) {
            if (results.data.success) {

                if (tipo == 1) {
                    row.estado = 'I';
                    $scope.showMessage('M', 'Usuario Inactivo correctamente.');
                }
                if (tipo == 2) {
                    row.estado = 'A';
                    $scope.showMessage('M', 'Usuario Activado correctamente.');
                }
                //$scope.resDgUsrsAdic = [];
                //$scope.pgcDgUsrsAdic = [];
                //   $scope.btnConsultaClickdespues();
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
    }
    $scope.selRowUsrsAdic = function (row) {
        $scope.allChecksAlm = false;
        $scope.allChecksCiu = false;
        $scope.allChecksSel = false;
        if (row.estado == 'A' || row.estado == 'I') {
            $scope.myPromise = SeguridadService.getConsDatosUsrsAdic($scope.usrAdic.pRuc, row.usuario).then(function (results) {
                if (results.data.success) {
                    oUsAdic = results.data.root[0][0];
                    $scope.usrAdic.pUsuario = row.usuario;
                    $scope.usrAdic.pIdParticipante = oUsAdic.pIdParticipante;
                    $scope.usrAdic.pCorreoE = oUsAdic.pCorreoE;
                    $scope.usrAdic.pTelefono = oUsAdic.pTelefono;
                    $scope.usrAdic.pCelular = oUsAdic.pCelular;
                    if (authService.authentication.Usuario.substring(0, 4) == "CER0")
                        $scope.usrAdic.pClave = row.clave;
                    else
                        $scope.usrAdic.pClave = "";

                    $scope.usrAdic.pIdentificacion = oUsAdic.pIdentificacion;
                    $scope.usrAdic.pApellido1 = oUsAdic.pApellido1;
                    $scope.usrAdic.pApellido2 = oUsAdic.pApellido2;
                    $scope.usrAdic.pNombre1 = oUsAdic.pNombre1;
                    $scope.usrAdic.pNombre2 = oUsAdic.pNombre2;
                    $scope.usrAdic.pDireccion = oUsAdic.pDireccion;
                    $scope.usrAdic.pRecActas = oUsAdic.pRecibeActa;
                    //combos
                    $scope.usrAdic.pEstado = oUsAdic.pEstado;
                    $scope.usrAdic.pTipoIdentificacion
                    //luis chacha
                    $scope.usrAdic.pTipoIdentificacion = oUsAdic.pTipoIdentificacion;
                    //luis chacha

                    $scope.usrAdic.pTipoIdent = oUsAdic.pTipoIdent;
                    $scope.usrAdic.pEstadoCivil = oUsAdic.pEstadoCivil;
                    $scope.usrAdic.pGenero = oUsAdic.pGenero;
                    $scope.usrAdic.pPais = oUsAdic.pPais;
                    $scope.usrAdic.pProvincia = oUsAdic.pProvincia;
                    $scope.usrAdic.pCiudad = oUsAdic.pCiudad;
                    $scope.ctrls.cboEstadoSelItem = $scope.setValorCombo($scope.ctrls.cboEstadoList, $scope.ctrls.cboEstadoSelItem, oUsAdic.pEstado);

                    //luis chacha
                    $scope.ctrls.cboTipoIdentificacionSelItem = [];
                    $scope.ctrls.cboTipoIdentificacionSelItem = $scope.setValorCombo($scope.ctrls.cboTipoIdentificacionList, $scope.ctrls.cboTipoIdentificacionSelItem, oUsAdic.pTipoIdent);
                    //luis chacha

                    //$scope.ctrls.cboTipoIdentSelItem = $scope.setValorCombo($scope.ctrls.cboTipoIdentList, $scope.ctrls.cboTipoIdentSelItem, oUsAdic.pTipoIdent);
                    //$scope.ctrls.cboEstCivilSelItem = $scope.setValorCombo($scope.ctrls.cboEstCivilList, $scope.ctrls.cboEstCivilSelItem, oUsAdic.pEstadoCivil);
                    //$scope.ctrls.cboGeneroSelItem = $scope.setValorCombo($scope.ctrls.cboGeneroList, $scope.ctrls.cboGeneroSelItem, oUsAdic.pGenero);
                    //$scope.ctrls.cboPaisSelItem = $scope.setValorCombo($scope.ctrls.cboPaisList, $scope.ctrls.cboPaisSelItem, oUsAdic.pPais);
                    //$scope.ctrls.cboRegionListTmp = $filter('filter')($scope.ctrls.cboRegionList,
                    //                  { descAlterno: $scope.ctrls.cboPaisSelItem.codigo });
                    $scope.DepartaContacto = [];
                    $scope.DepartaContacto = $scope.setValorCombo($scope.ListDepartaContacto, $scope.DepartaContacto, oUsAdic.pDepartamento);

                    $scope.FuncionContacto = [];
                    $scope.FuncionContacto = $scope.setValorCombo($scope.ListFuncionContactoT, $scope.FuncionContacto, oUsAdic.pFuncion);

                    $scope.ctrls.cboRegionSelItem = $scope.setValorCombo($scope.ctrls.cboRegionList, $scope.ctrls.cboRegionSelItem, oUsAdic.pProvincia);
                    $scope.ctrls.cboCiudadListTmp = $filter('filter')($scope.ctrls.cboCiudadList,
                                      { descAlterno: $scope.ctrls.cboRegionSelItem.codigo });
                    $scope.ctrls.cboCiudadSelItem = $scope.setValorCombo($scope.ctrls.cboCiudadListTmp, $scope.ctrls.cboCiudadSelItem, oUsAdic.pCiudad);
                    //grids
                    $scope.usrAdic.pListZona = [];
                    //if (oUsAdic.pListZona != null) {
                    //    for (var i = 0; i < oUsAdic.pListZona.length; i++) {
                    //        $scope.usrAdic.pListZona.push({ pCodZona: oUsAdic.pListZona[i].pCodZona, pDescripcion: oUsAdic.pListZona[i].pDescripcion });
                    //    }
                    //}
                    for (var a = 0; a < $scope.ListDgZonas.length; a++) {
                        $scope.ListDgZonas[a].pElegir = false;
                    }
                    if (oUsAdic.pListZona != null) {
                        var noExisteZona;
                        for (var i = 0; i < oUsAdic.pListZona.length; i++) {
                            noExisteZona = true;
                            for (var j = 0; j < $scope.ListDgZonas.length; j++) {
                                if (oUsAdic.pListZona[i].pCodZona == $scope.ListDgZonas[j].pCodZona) {
                                    $scope.ListDgZonas[j].pElegir = true;
                                    noExisteZona = false;
                                }
                            }
                            if (noExisteZona) {
                                //$scope.usrAdic.ListDgZonas.push({ pCodZona: oUsAdic.pListZona[i].pCodZona, pDescripcion: oUsAdic.pListZona[i].pDescripcion, pElegir: true });
                            }
                        }
                    }

                    for (var a = 0; a < $scope.ListDgAlmacenesT.length; a++) {
                        $scope.ListDgAlmacenesT[a].pElegir = false;
                    }
                    if (oUsAdic.pListAlm != null) {
                        //var noExisteZona;
                        for (var i = 0; i < oUsAdic.pListAlm.length; i++) {
                            //noExisteZona = true;
                            for (var j = 0; j < $scope.ListDgAlmacenesT.length; j++) {
                                if (oUsAdic.pListAlm[i].pCodCiudad == $scope.ListDgAlmacenesT[j].pCodCiudad &&
                                    oUsAdic.pListAlm[i].pCodAlmacen == $scope.ListDgAlmacenesT[j].pCodAlmacen) {
                                    $scope.ListDgAlmacenesT[j].pElegir = true;
                                    //noExisteZona = false;
                                }
                            }
                            //if (noExisteZona) {
                            //    $scope.usrAdic.ListDgZonas.push({ pCodZona: oUsAdic.pListZona[i].pCodZona, pDescripcion: oUsAdic.pListZona[i].pDescripcion, pElegir: true });
                            //}
                        }
                    }


                    $scope.usrAdic.pListRol = [];
                    //if (oUsAdic.pListRol != null) {
                    //    for (var i = 0; i < oUsAdic.pListRol.length; i++) {
                    //        $scope.usrAdic.pListRol.push({ pIdRol: oUsAdic.pListRol[i].pIdRol, pDescripcion: oUsAdic.pListRol[i].pDescripcion });
                    //    }
                    //}
                    for (var a = 0; a < $scope.ListDgRoles.length; a++) {
                        $scope.ListDgRoles[a].pElegir = false;
                    }
                    if (oUsAdic.pListRol != null) {
                        var noExisteRol;
                        for (var i = 0; i < oUsAdic.pListRol.length; i++) {
                            noExisteRol = true;
                            for (var j = 0; j < $scope.ListDgRoles.length; j++) {
                                if (oUsAdic.pListRol[i].pIdRol == $scope.ListDgRoles[j].pIdRol) {
                                    $scope.ListDgRoles[j].pElegir = true;
                                    noExisteRol = false;
                                }
                            }
                            if (noExisteRol) {
                              //  $scope.usrAdic.ListDgRoles.push({ pIdRol: oUsAdic.pListRol[i].pIdRol, pDescripcion: oUsAdic.pListRol[i].pDescripcion, pElegir: true });
                            }
                        }
                    }
                    $scope.esNuevo = false;
                    $('#MantUsrAdicDialog').modal('show');

                    if ($('#accordion_default').hasClass('collapsed')) {
                        setTimeout(function () { angular.element('#accordion_default').trigger('click'); }, 150);
                    }
                }
                else {
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
        }
    };

    $scope.btnNuevoClick = function (row) {
        $scope.allChecksAlm = false;
        $scope.allChecksCiu = false;
        $scope.allChecksSel = false;
        $scope.recActasSN = true;
        $scope.usrAdic.pUsuario = "";
        $scope.usrAdic.pIdParticipante = 0;
        $scope.usrAdic.pCorreoE = "";
        $scope.usrAdic.pTelefono = "";
        $scope.usrAdic.pCelular = "";
        $scope.usrAdic.pEstado = "";
        $scope.usrAdic.pClave = "";
        //$scope.usrAdic.pTipoIdent = "C";
        $scope.usrAdic.pTipoIdent = $scope.ctrls.cboTipoIdentificacionSelItem.codigo;   //lui
        $scope.usrAdic.pIdentificacion = "";
        $scope.usrAdic.pApellido1 = "";
        $scope.usrAdic.pApellido2 = "";
        $scope.usrAdic.pNombre1 = "";
        $scope.usrAdic.pNombre2 = "";
        $scope.usrAdic.pEstadoCivil = "S";
        $scope.usrAdic.pGenero = "M";
        $scope.usrAdic.pPais = "EC";
        $scope.usrAdic.pProvincia = "";
        $scope.usrAdic.pCiudad = "";
        $scope.usrAdic.pDireccion = "N/A";
        $scope.usrAdic.pTipoIdentificacion = "";
        $scope.usrAdic.pRecActas = false;
        $scope.usrAdic.pListRol = [];
        $scope.DepartaContacto = [];
        $scope.FuncionContacto = [];
        for (var a = 0; a < $scope.ListDgRoles.length; a++) {
            $scope.ListDgRoles[a].pElegir = false;
        }
        $scope.usrAdic.pListZona = [];
        for (var a = 0; a < $scope.ListDgZonas.length; a++) {
            $scope.ListDgZonas[a].pElegir = false;
        }
        for (var a = 0; a < $scope.ListDgAlmacenesT.length; a++) {
            $scope.ListDgAlmacenesT[a].pElegir = false;
        }
        $scope.ctrls.cboEstadoSelItem = $scope.ctrls.cboEstadoList[0];
      
        $scope.ctrls.cboTipoIdentificacionSelItem = [];

        $scope.esNuevo = true;
        $('#MantUsrAdicDialog').modal('show');
    
        if ($('#accordion_default').hasClass('collapsed')) {
            setTimeout(function () { angular.element('#accordion_default').trigger('click'); }, 150);
        }
    };

    $scope.btnGrabarClick = function () {
        $scope.usrAdic.pListRol = [];
        $scope.usrAdic.pListAlm = [];
        if ($scope.resDgUsrsAdic.length.toString() == "100") {
            $scope.showMessage('I', "Solo se permite crear 100 usuario asociado.");
            return;
        }
        for (var a = 0; a < $scope.ListDgRoles.length; a++) {
            if ($scope.ListDgRoles[a].pElegir) {
                $scope.usrAdic.pListRol.push({ pIdRol: $scope.ListDgRoles[a].pIdRol, pDescripcion: $scope.ListDgRoles[a].pDescripcion, pDescripcionRol: $scope.ListDgRoles[a].pDescripcionRol });
            }
        }

        for (var a = 0; a < $scope.ListDgAlmacenesT.length; a++) {

            if ($scope.ListDgAlmacenesT[a].pElegir && $scope.usrAdic.pRecActas) {
                $scope.usrAdic.pListAlm.push({
                    pCodAlmacen: $scope.ListDgAlmacenesT[a].pCodAlmacen,
                    pCodCiudad: $scope.ListDgAlmacenesT[a].pCodCiudad
                });
            }
        }

        if ($scope.usrAdic.pListRol.length < 1) {
            $scope.showMessage('I', "Debe seleccionar al menos un Permiso.");
            return;
        }
        if ($scope.usrAdic.pRecActas) {

            var existeRolLogistico = $filter('filter')($scope.usrAdic.pListRol, { pIdRol: "26" }, true);
            if (existeRolLogistico.length < 1) {
                $scope.showMessage('I', "Debe asignar Rol Logístico.");
                return;
            }


        }



        $scope.usrAdic.pListZona = [];
        for (var a = 0; a < $scope.ListDgZonas.length; a++) {
            if ($scope.ListDgZonas[a].pElegir && $scope.usrAdic.pRecActas) {
                $scope.usrAdic.pListZona.push({ pCodZona: $scope.ListDgZonas[a].pCodZona, pDescripcion: $scope.ListDgZonas[a].pDescripcion });
            }
        }
        if ($scope.usrAdic.pRecActas) {
            if ($scope.usrAdic.pListZona.length < 1) {
                $scope.showMessage('I', "Debe seleccionar al menos una ciudad.");
                return;
            }
            for (var r = 0 ; r < $scope.usrAdic.pListZona.length ; r++) {

                var reg = $filter('filter')($scope.ListDgAlmacenesT, { pElegir: true, pCodCiudad: $scope.usrAdic.pListZona[r].pCodZona }, true);
                if (reg.length < 1) {
                    $scope.showMessage('I', 'Seleccione al menos un almacén para la ciudad de ' + $scope.usrAdic.pListZona[r].pDescripcion);
                    return;
                }
            }
        }



        $scope.usrAdic.pTipoIdent = $scope.ctrls.cboTipoIdentificacionSelItem.codigo;
        if ($scope.esNuevo) {
            if ($scope.usrAdic.pTipoIdent == "CD") {

                if ($scope.usrAdic.pUsuario.length != 10) {
                    $scope.showMessage('I', "La identificación (Usuario) debe tener 10 dígitos.");
                    return;
                }
                if (!$scope.validadorCedula($scope.usrAdic.pUsuario)) {
                    return;
                }
            }
            //luis chacha

            //if ($scope.usrAdic.pUsuario.length != 10) {
            //    $scope.showMessage('I', "La identificación (Usuario) debe tener 10 dígitos.");
            //    return;
            //}
            //if (!$scope.validadorCedula($scope.usrAdic.pUsuario)) {
            //    return;
            //}

            $scope.usrAdic.pClave = generarPassword(8, 0, '');
            $scope.usrAdic.pIdentificacion = $scope.usrAdic.pUsuario;
        }
        $scope.usrAdic.pApellido1 = $scope.usrAdic.pApellido1.toUpperCase();;
        $scope.usrAdic.pApellido2 = $scope.usrAdic.pApellido2.toUpperCase();;
        $scope.usrAdic.pNombre1 = $scope.usrAdic.pNombre1.toUpperCase();;
        $scope.usrAdic.pNombre2 = $scope.usrAdic.pNombre2.toUpperCase();;
        $scope.usrAdic.pDireccion = $scope.usrAdic.pDireccion.toUpperCase();;
        //lectura de combos
        $scope.usrAdic.pEstado = $scope.ctrls.cboEstadoSelItem.codigo;
        //$scope.usrAdic.pTipoIdent = $scope.ctrls.cboTipoIdentSelItem.codigo;
        //$scope.usrAdic.pEstadoCivil = $scope.ctrls.cboEstCivilSelItem.codigo;
        //$scope.usrAdic.pGenero = $scope.ctrls.cboGeneroSelItem.codigo;
        //$scope.usrAdic.pPais = $scope.ctrls.cboPaisSelItem.codigo;
        $scope.usrAdic.pProvincia = $scope.ctrls.cboRegionSelItem.codigo;
        $scope.usrAdic.pCiudad = $scope.ctrls.cboCiudadSelItem.codigo;
        $scope.usrAdic.pDepartamento = $scope.DepartaContacto.codigo;
        $scope.usrAdic.pFuncion = $scope.FuncionContacto.codigo;
        $scope.usrAdic.pRecibeActa = $scope.usrAdic.pRecActas;
        $scope.usrAdic.pRazonSocial = $scope.pRazonSocial;
        $scope.usrAdic.pApoderado = $scope.pApoderado;

        //llamada al servicio
        $scope.myPromise = SeguridadService.getGrabaUsrAdic($scope.usrAdic).then(function (results) {
            if (results.data.success) {
                //debugger;
                $scope.usrAdic.pClave = "";
                if ($scope.esNuevo) {
                    //$scope.usrAdic.pUsuario = results.data.root[0].pUsuario;
                    $scope.usrAdic.pIdParticipante = results.data.root[0].pIdParticipante;
                    $scope.esNuevo = false;
                    $scope.showMessage('M', 'Usuario ingresado correctamente.');
                }
                else {
                    $scope.showMessage('M', 'Usuario actualizado correctamente.');
                }
                $scope.btnConsultaClick();
            }
            else {
                //debugger;
                $scope.showMessage('E', 'Error al grabar: ' + results.data.msgError);
            }
        },
         function (error) {
             //debugger;
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

    $scope.marcaChecksS = function (valor) {

        for (var i = 0; i < $scope.ListDgAlmacenes.length; i++) {
            var updt = $scope.ListDgAlmacenes[i];
            updt.pElegir = valor;
        }

    }

    $scope.marcaChecksA = function (valor) {

        for (var j = 0; j < $scope.ListDgAlmacenesT.length; j++) {
            $scope.ListDgAlmacenesT[j].pElegir = valor;
        }

    }

    $scope.marcaChecksC = function (valor) {


        for (var j = 0; j < $scope.ListDgZonas.length; j++) {
            $scope.ListDgZonas[j].pElegir = valor;
        }

        if (!valor) {
            $scope.allChecksAlm = valor;
            for (var j = 0; j < $scope.ListDgAlmacenesT.length; j++) {
                $scope.ListDgAlmacenesT[j].pElegir = valor;
            }
        }

    }

    $scope.btnDesbloquearClick = function () {
        if ($scope.usrAdic.pCorreoE == "") {
            $scope.showMessage('I', "Falta el Correo Electrónico.");
            return;
        }
        if ($scope.usrAdic.pApellido1 == "") {
            $scope.showMessage('I', "Falta el 1er Apellido.");
            return;
        }
        if ($scope.usrAdic.pNombre1 == "") {
            $scope.showMessage('I', "Falta el 1er Nombre.");
            return;
        }
        $scope.tipoConfirma = "1";
        $scope.MenjConfirmacion = "¿ESTA SEGURO DE DESBLOQUEAR LA CONTRASEÑA DE ESTE USUARIO?";
        $('#idMensajeConfirmacion').modal('show');
    };

    $scope.voidDesbloquear = function () {
        var Correo = $scope.usrAdic.pCorreoE;
        var NomUsuario = $scope.usrAdic.pApellido1 + ' ' + $scope.usrAdic.pApellido2 + ' ' + $scope.usrAdic.pNombre1 + ' ' + $scope.usrAdic.pNombre2;
        $scope.myPromise = SeguridadService.getDesbloquearClaveUsrAdic($scope.usrAdic.pRuc, $scope.usrAdic.pUsuario, Correo, NomUsuario, authService.authentication.nomEmpresa).then(function (results) {
            if (results.data.success) {
                $scope.showMessage('M', 'Desbloqueada correctamente.');
            }
            else {
                $scope.showMessage('E', 'Error al Desbloquear: ' + results.data.msgError);
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

    $scope.btnNuevaClaveClick = function () {
        if ($scope.usrAdic.pCorreoE == "") {
            $scope.showMessage('I', "Falta el Correo Electrónico.");
            return;
        }
        if ($scope.usrAdic.pApellido1 == "") {
            $scope.showMessage('I', "Falta el 1er Apellido.");
            return;
        }
        if ($scope.usrAdic.pNombre1 == "") {
            $scope.showMessage('I', "Falta el 1er Nombre.");
            return;
        }
        $scope.tipoConfirma = "2";
        $scope.MenjConfirmacion = "¿ESTA SEGURO DE REINICIAR LA CONTRASEÑA DE ESTE USUARIO? " +
            "(Se generará una nueva contraseña temporal que será enviada por correo al Usuario y se forzará el cambio de la misma en el próximo inicio de sesión)";
        $('#idMensajeConfirmacion').modal('show');
    };

    $scope.voidNuevaClave = function () {
        var Clave = generarPassword(8, 0, '');
        var Correo = $scope.usrAdic.pCorreoE;
        var NomUsuario = $scope.usrAdic.pApellido1 + ' ' + $scope.usrAdic.pApellido2 + ' ' + $scope.usrAdic.pNombre1 + ' ' + $scope.usrAdic.pNombre2;
        $scope.myPromise = SeguridadService.getResetClaveUsrAdic($scope.usrAdic.pRuc, $scope.usrAdic.pUsuario, Clave, Correo, NomUsuario, authService.authentication.nomEmpresa).then(function (results) {
            if (results.data.success) {
                $scope.showMessage('M', 'Contraseña reiniciada correctamente.');
            }
            else {
                $scope.showMessage('E', 'Error al Reiniciar: ' + results.data.msgError);
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

    $scope.grabar = function () {
        if ($scope.tipoConfirma == "1") {
            $scope.voidDesbloquear();
        }
        else if ($scope.tipoConfirma == "2") {
            $scope.voidNuevaClave();
        }
    };


    //recuperar del login
    //$scope.usrAdic.pRuc = "1702576651001";
    //$scope.usrAdic.pCodSap = "115610";

    //Adicionado por djbm
    $scope.usrAdic.pRuc = authService.authentication.ruc;
    $scope.usrAdic.pCodSap = authService.authentication.CodSAP;
    //fin


    setTimeout(function () { $('#cargaConsulta').focus(); }, 100);
    setTimeout(function () { angular.element('#cargaConsulta').trigger('click'); }, 150);



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

    $("#btnMensajeOK").click(function () {
        $('#idMensajeOk').modal('hide');
        $scope.btnConsultaClick();
        setTimeout(function () { $('#cargaConsulta').focus(); }, 100);
        //setTimeout(function () { $('#txtnombre').focus(); }, 150);


    });
    $scope.setValorCombo = function (pLista, pItemSel, pCodigo) {
        for (idx = 0; idx < pLista.length; ++idx) {
            if (pLista[idx].codigo == pCodigo) {
                pItemSel = pLista[idx];
                break;
            }
        }
        return pItemSel;
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

    $scope.validadorCedula = function (valorCedula) {

        var txtIdentificacion = valorCedula;
        var numero = 0;
        var d1 = 0;
        var d2 = 0;
        var d3 = 0;
        var d4 = 0;
        var d5 = 0;
        var d6 = 0;
        var d7 = 0;
        var d8 = 0;
        var d9 = 0;
        var d10 = 0;

        var p1 = 0;
        var p2 = 0;
        var p3 = 0;
        var p4 = 0;
        var p5 = 0;
        var p6 = 0;
        var p7 = 0;
        var p8 = 0;
        var p9 = 0;
        var digitoVerificador = 0;




        var campos = txtIdentificacion;
        if (campos.length == 10 || campos.length == 13) {
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
                $scope.showMessage('I', 'El tercer dígito ingresado para la identificación es inválido');
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

            /* Si residuo=0, dig.ver.=0, caso contrario 10 - residuo*/
            digitoVerificador = residuo == 0 ? 0 : modulo - residuo;

            /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
            if (pub == true) {
                if (digitoVerificador != d9) {
                    $scope.showMessage('I', 'La identificación que corresponde al sector público es incorrecto.');
                    return false;
                }
                /* El ruc de las empresas del sector publico terminan con 0001*/
                if (numero.substr(9, 4) != '0001') {
                    $scope.showMessage('I', 'La identificación que corresponde al sector público debe terminar con 0001');
                    return false;
                }
            }
            else if (pri == true) {
                if (digitoVerificador != d10) {
                    $scope.showMessage('I', 'La identificación que corresponde al sector privado es incorrecto.');
                    return false;
                }
                if (numero.substr(10, 3) != '001') {
                    $scope.showMessage('I', 'La identificación que corresponde al sector privado debe terminar con 001');
                    return false;
                }
            }

            else if (nat == true) {
                if (digitoVerificador != d10) {
                    $scope.showMessage('I', 'La identificación de la persona natural es incorrecto.');
                    return false;
                }
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    $scope.showMessage('I', 'La identificación de la persona natural debe terminar con 001');
                    return false;
                }
            }
            return true;
        }
        else {
            $scope.showMessage('I', 'La identificación debe tener 10 dígitos para cédulas o 13 para ruc.');
            return false;
        }
    }

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
