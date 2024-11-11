
//Choferes proveedor controller administrativo
app.controller('CatagoloController', ['$scope', '$location', 'CatalogoService', '$cookies', 'ngAuthSettings', 'SeguridadService', '$filter', 'authService', function ($scope, $location, CatalogoService, $cookies, ngAuthSettings, SeguridadService, $filter, authService) {
    $scope.formData = {};

    $scope.etiTotRegistros = "";
    $scope.etiTotRegistrosLP = "";
    $scope.etiTotRegistrosHE = "";
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
    $scope.pageContentProveedor = [];

    $scope.habilitar = true;
    $scope.habilitares = true;
    //Variable de Busquedas
    $scope.PorProveedor = 1;
    $scope.txtcodigocomprador = "";
    $scope.txtnombrecomprador = "";
    $scope.cmbestado = [];
    $scope.cmbestadolistado = [];
    $scope.pagesNot = [];
    //Fin Variable

    //Varible de Mantenimientos

    $scope.traCo = {};
    $scope.traCo.Tipo = "1";
    $scope.traCo.idCabecera = "";
    $scope.traCo.idCatalogo = "";
    $scope.traCo.descripcion = "";
    $scope.traCo.descAlterno = "";
    $scope.traCo.estado = "A";
    $scope.traCo.CodProveedor = authService.authentication.CodSAP;
    $scope.traCo.usuarioCreacion = authService.authentication.userName;

    $scope.codigoDetalle = "";
    $scope.descripcinDetalle = "";
    $scope.descAlterno = "";

    $scope.selectempresa = "";
    $scope.listaEmpresa = [{ codigo: "", nombreproveedor: "" }];
    $scope.Setting = { displayProp: 'nombreproveedor', idProp: 'codigo', enableSearch: true, scrollableHeight: '300px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.sortType = 'nombres'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';
    //Fin Variable

    $scope.idDetalle = "";

    //Variable de Mensaje
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    //Fin Variable
    $scope.okGrabar = function () {
        $scope.nuevo("2");
        $scope.ConsultarDespues();
    }
    $scope.okGrabarCargar = function () {

    }
    $scope.myPromise = SeguridadService.getCatalogo('tbl_EstadoUsuarios').then(function (results) {
        $scope.cmbestado = results.data[0];
        $scope.cmbestadolistado = results.data;
    }, function (error) {
    });

    $scope.proveedorcatalogo = function () {
        $scope.myPromise = CatalogoService.catalogoEmpresa("4").then(function (results) {
            if (results.data.cCodError == "0") {
                var datos = results.data.root[0];
                datos.splice(0, 0, { codigo: "", nombreproveedor: "" });
                $scope.listaEmpresa = datos;
                $scope.selectempresa = datos[0];
            }

        }, function (error) {
        });
    }
    $scope.proveedorcatalogo();
    $scope.limpioCaja = function () {
        if ($scope.PorProveedor === "1") {
            $scope.txtcodigocomprador = "";
            $scope.txtnombrecomprador = "";
        }
        if ($scope.PorProveedor === "2") {
            $scope.txtnombrecomprador = "";
        }
        if ($scope.PorProveedor === "3") {
            $scope.txtcodigocomprador = "";
        }
    }
    function Limpiar(tipo) {
        if (tipo == 1) {
            $scope.habilitar = false;
            $scope.habilitares = false;
            $scope.traCo.Tipo = "1";
            $scope.traCo.idCatalogo = "";
            $scope.traCo.descripcion = "";
            $scope.traCo.idCabecera = "";
            $scope.traCo.codigoCabecera = "";
            $scope.traCo.descripcionCabecera = "";
            $scope.traCo.estado = "A";
            $scope.selectempresa = "";
            $scope.selectEstado = "";
            $scope.selectBloqueo = "";
            $scope.selectCategoria = "";
            $scope.selectTipoSangre = "";
            $scope.div1 = false;
            $scope.div2 = true;
            $scope.pageContentChoP = [];
            $scope.pageContentChoH = [];
            $scope.pageContentProveedor = [];
            $scope.codigoDetalle = "";
            $scope.codigoDetalle = "";
            $scope.descripcinDetalle = "";
            $scope.descAlterno = "";
            $scope.proveedorcatalogo();
            $scope.habilitarModi = false;
        }

        if (tipo == 2) {
            $scope.habilitar = false;
            $scope.habilitares = false;
            $scope.traCo.Tipo = "1";
            $scope.traCo.idCatalogo = "";
            $scope.traCo.descripcion = "";
            $scope.traCo.idCabecera = "";
            $scope.traCo.codigoCabecera = "";
            $scope.traCo.descripcionCabecera = "";
            $scope.traCo.estado = "A";
            $scope.selectempresa = "";
            $scope.selectEstado = "";
            $scope.selectBloqueo = "";
            $scope.selectCategoria = "";
            $scope.selectTipoSangre = "";
            $scope.div1 = false;
            $scope.div2 = true;
            $scope.pageContentChoP = [];
            $scope.pageContentChoH = [];
            $scope.pageContentProveedor = [];
            $scope.codigoDetalle = "";
            $scope.descripcinDetalle = "";
            $scope.descAlterno = "";
            $scope.proveedorcatalogo();
            $scope.habilitarModi = false;
        }
        if (tipo == 3) {
            $scope.habilitar = true;
            $scope.habilitares = true;
            $scope.traCo.Tipo = "1";
            $scope.traCo.idCatalogo = "";
            $scope.traCo.descripcion = "";
            $scope.traCo.idCabecera = "";
            $scope.traCo.codigoCabecera = "";
            $scope.traCo.descripcionCabecera = "";
            $scope.traCo.estado = "A";
            $scope.selectempresa = "";
            $scope.selectEstado = "";
            $scope.selectBloqueo = "";
            $scope.selectCategoria = "";
            $scope.selectTipoSangre = "";
            $scope.div1 = false;
            $scope.div2 = true;
            $scope.pageContentChoP = [];
            $scope.pageContentChoH = [];
            $scope.pageContentProveedor = [];
            $scope.codigoDetalle = "";
            $scope.descripcinDetalle = "";
            $scope.descAlterno = "";
            $scope.proveedorcatalogo();
            $scope.habilitarModi = false;
        }

    }
    $scope.nuevo = function (tipo) {

        $scope.idDetalle
        Limpiar(tipo);

    }

    $scope.selEliminarAdic = function (content)
    {
        for (var i = $scope.pageContentProveedor.length - 1; i >= 0; i--) {
            if (content.codigoDetalle === $scope.pageContentProveedor[i].codigoDetalle) {
                index = i;
                $scope.pageContentProveedor.splice(index, 1);
            }
        }
    }

    $scope.selRowUsrsAdic = function (content) {
        $scope.idDetalle = content.idDetalle;
        $scope.codigoDetalle = content.codigoDetalle;
        $scope.descripcinDetalle = content.descripcion;
        $scope.descAlterno = content.descAlterno;
        $scope.habilitarModi = true;
    }
    $scope.agregarDetalle = function () {
        var tmp = {}
        var ban = 0;
        if ($scope.idDetalle=="") {
            if ($scope.codigoDetalle != "" && $scope.descripcinDetalle != "") {
                for (var i = 0; i < $scope.pageContentProveedor.length; i++) {
                    if ($scope.pageContentProveedor[i].codigoDetalle == $scope.codigoDetalle) {
                        $scope.MenjError = "El Catalogo ya exite en la relacion."
                        $('#idMensajeError').modal('show');
                        $scope.codigoDetalle = "";
                        $scope.descripcinDetalle = "";
                        ban = 1;
                    }
                }
                tmp.idDetalle = "";
                tmp.codigoDetalle = $scope.codigoDetalle;
                tmp.descripcion = $scope.descripcinDetalle;
                tmp.descAlterno = $scope.descAlterno;
                tmp.estado = true;
                if (ban == 0) {
                    $scope.pageContentProveedor.push(tmp);
                }
            }
        } else
        {
            if ($scope.descripcinDetalle != "") {
                for (var i = 0; i < $scope.pageContentProveedor.length; i++) {
                    if ($scope.pageContentProveedor[i].idDetalle == $scope.idDetalle) {
                        $scope.pageContentProveedor[i].descripcion = $scope.descripcinDetalle;
                        $scope.pageContentProveedor[i].descAlterno = $scope.descAlterno;
                        $scope.habilitarModi = false;
                        $scope.idDetalle = "";
                        $scope.codigoDetalle = "";
                        $scope.descripcinDetalle = "";
                        $scope.descAlterno = "";
                        break;
                    }

                }
            }
        }
        $scope.habilitarModi = false;
        $scope.idDetalle = "";
        $scope.codigoDetalle = "";
        $scope.descripcinDetalle = "";
        $scope.descAlterno = "";
    }
    $scope.BuscarFiltro = function () {
        $scope.Consultar();
    }
    $scope.ConsultarDespues = function () {

        if ($scope.PorProveedor === "2") {
            if ($scope.txtcodigocomprador === "") {
                $scope.MenjError = "Se debe ingresar el Codigo Comprador que desee consultar."
                $('#idMensajeError').modal('show');
                return;
            } else {
                if (isNaN($scope.txtcodigocomprador)) {
                    $scope.MenjError = "Este campo debe tener sólo números."
                    $('#idMensajeError').modal('show');
                    return;
                }
            }


        }
        if ($scope.PorProveedor === "3") {
            if ($scope.txtnombrecomprador === "") {
                $scope.MenjError = "Se debe ingresar el Nombre de Comprador que desee consultar."
                $('#idMensajeError').modal('show');
                return;
            }
        }
        $scope.myPromise = null;
        $scope.etiTotRegistros = ""
        $scope.myPromise = CatalogoService.getConsulaGrid($scope.txtcodigocomprador, $scope.txtnombrecomprador, $scope.cmbestado.codigo).then(function (results) {
            if (results.data.lSuccess) {
                $scope.GridTransporte = results.data.root[0];
                $scope.etiTotRegistros = $scope.GridTransporte.length.toString();
                if ($scope.GridTransporte.length == 0) {
                    $scope.MenjError = "No hay datos para su consulta."
                    $('#idMensajeInformativo').modal('show');
                    return;
                }
            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTodosProveedores').focus(); }, 150);
        }, function (error) {
        });
    }

    $scope.Consultar = function () {
        if ($scope.PorProveedor === "2") {
            if ($scope.txtcodigocomprador === "") {
                $scope.MenjError = "Se debe ingresar el Codigo Comprador que desee consultar."
                $('#idMensajeError').modal('show');
                return; 
            } else {
                if (isNaN($scope.txtcodigocomprador)) {
                    $scope.MenjError = "Este campo debe tener sólo números."
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
        }
        if ($scope.PorProveedor === "3") {
            if ($scope.txtnombrecomprador === "") {
                $scope.MenjError = "Se debe ingresar el Nombre de Comprador que desee consultar."
                $('#idMensajeError').modal('show');
                return;
            }
        }
        $scope.myPromise = null;
        $scope.etiTotRegistros = ""
        $scope.myPromise = CatalogoService.getConsulaGrid($scope.txtcodigocomprador, $scope.txtnombrecomprador, $scope.cmbestado.codigo).then(function (results) {
            if (results.data.lSuccess) {
                $scope.GridTransporte = results.data.root[0];
                $scope.etiTotRegistros = $scope.GridTransporte.length.toString();
                if ($scope.GridTransporte.length == 0) {
                    $scope.MenjError = "No hay datos para su consulta."
                    $('#idMensajeInformativo').modal('show');
                    return;
                }
            }
            setTimeout(function () { $('#consultagrid').focus(); }, 100);
            setTimeout(function () { $('#rbtTodosProveedores').focus(); }, 150);
        }, function (error) {
        });
    }

    $scope.Confirmargrabar = function () {
        if ($scope.traCo.Tipo === "1") {
            $scope.MenjConfirmacion = "¿Está seguro de guardar la información?"
        }
        if ($scope.traCo.Tipo === "2") {
            $scope.MenjConfirmacion = "¿Está seguro de modificar la información?"
        }
        $('#idMensajeConfirmacion').modal('show');
    }




    $scope.grabar = function () {
        if ($scope.traCo.Tipo === "1") {
            if ($scope.pageContentProveedor.length == 0) {
                $scope.MenjError = "Se debe ingresar un detalle en el Catálogo."
                $('#idMensajeError').modal('show');
                return;
            }

            $scope.myPromise = null;
            $scope.myPromise = CatalogoService.getGrabaCatalogo($scope.traCo, $scope.pageContentProveedor).then(function (results) {
                if (results.data.lSuccess) {
                    if ($scope.traCo.Tipo === "1") {
                        $scope.MenjError = "Catálogo ingresado correctamente"
                        $('#idMensajeGrabarBuscar').modal('show');
                        $scope.nuevo("2");
                        //$scope.ConsultarDespues();
                        $('.nav-tabs a[href="#CatalogoRegistrados"]').tab('show');
                    }
                }
                else {
                    if (results.data.cMsgError === "EXISTE")
                        if ($scope.traCo.Tipo === "1") {
                            $scope.MenjError = "El Catálogo ya exite verifique"
                            $('#idMensajeError').modal('show');

                            //$scope.Consultar();
                        }

                }
                setTimeout(function () { $('#consultagrid').focus(); }, 100);
                setTimeout(function () { $('#rbtTodosProveedores').focus(); }, 150);
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
        if ($scope.traCo.Tipo === "2") {
            debugger;
            $scope.myPromise = null;
            $scope.myPromise = CatalogoService.getModiCatalogo($scope.traCo, $scope.pageContentProveedor).then(function (results) {
                if (results.data.lSuccess) {
                    if ($scope.traCo.Tipo === "2" && results.data.cMsgError === "ACTUALIZADA") {
                        $scope.MenjError = "Catálogo modificado correctamente"
                        $('#idMensajeGrabarBuscar').modal('show');
                        $scope.nuevo("3");
                        //$scope.ConsultarDespues();
                        $('.nav-tabs a[href="#RegistroCatalogoNuevos"]').tab('show');

                    }
                }
                else {
                    $scope.MenjError = "No se pudo actualizar el Catalogo"
                    $('#idMensajeError').modal('show');
                }
                setTimeout(function () { $('#consultagrid').focus(); }, 100);
                setTimeout(function () { $('#rbtTodosProveedores').focus(); }, 150);
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

    
    $scope.SelecionarGrid = function (valorrecibido) {
        Limpiar("2");
        $('.nav-tabs a[href="#RegistroCatalogoNuevos"]').tab('show');
        $scope.myPromise = null;
        $scope.myPromise = CatalogoService.getConsulaGridUno(valorrecibido).then(function (results) {
            if (results.data.lSuccess) {
                var retorno = {};
                $scope.habilitar = false;
                retorno = results.data.root[0];
                debugger;
                $scope.div1 = true;
                $scope.div2 = false;
                $scope.traCo.idCabecera = retorno[0].idCabecera;
                $scope.traCo.idCatalogo = retorno[0].codigoCatalogo;
                $scope.traCo.descripcion = retorno[0].descripcion;
                $scope.traCo.Tipo = "2";
                $scope.traCo.descAlterno = retorno[0].descAlterno;
                $scope.pageContentProveedor = results.data.root[1];



            }
        }, function (error) {
        });
    }



    //Fin Archivo
    // $scope.nuevo();

}
]);