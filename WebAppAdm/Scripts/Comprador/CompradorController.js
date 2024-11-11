
//Choferes proveedor controller administrativo
app.controller('CompradorController', ['$scope', '$location', 'CompradorService', '$cookies', 'ngAuthSettings', 'SeguridadService', '$filter', 'authService', function ($scope, $location, CompradorService, $cookies, ngAuthSettings, SeguridadService, $filter, authService) {
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
    
    //Fin Variable
   
    //Varible de Mantenimientos

    $scope.traCo = {};
    $scope.traCo.Tipo = "1";
    $scope.traCo.idComprador = "";
    $scope.traCo.nombreComprador = "";
    $scope.traCo.correoComprador = "";
    $scope.traCo.correoAsistenteComprador = "";
    $scope.traCo.estado = "";
    $scope.traCo.CodProveedor = authService.authentication.CodSAP;
    $scope.traCo.usuarioCreacion = authService.authentication.userName;

    $scope.selectempresa = "";
    $scope.listaEmpresa = [{ codigo: "", nombreproveedor: "" }];
    $scope.Setting = { displayProp: 'nombreproveedor', idProp: 'codigo', enableSearch: true, scrollableHeight: '300px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.sortType = 'nombres'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';
    //Fin Variable

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
        $scope.myPromise = CompradorService.catalogoEmpresa("4").then(function (results) {
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
        if (tipo==1) {
            $scope.habilitar = false;
            $scope.habilitares = false;
            $scope.traCo.Tipo = "1";
            $scope.traCo.idComprador = "";
            $scope.traCo.nombreComprador = "";
            $scope.traCo.correoComprador = "";
            $scope.traCo.correoAsistenteComprador = "";
            $scope.traCo.estado = "A";
            $scope.traCo.detalle = [];
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
            $scope.proveedorcatalogo();
        }
        
        if (tipo == 2) {
            $scope.habilitar = false;
            $scope.habilitares = false;
            $scope.traCo.Tipo = "1";
            $scope.traCo.idComprador = "";
            $scope.traCo.nombreComprador = "";
            $scope.traCo.correoComprador = "";
            $scope.traCo.correoAsistenteComprador = "";
            $scope.traCo.estado = "";
            $scope.traCo.detalle = [];
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
            $scope.proveedorcatalogo();
        }
        if (tipo == 3) {
            $scope.habilitar = true;
            $scope.habilitares = true;
            $scope.traCo.Tipo = "1";
            $scope.traCo.idComprador = "";
            $scope.traCo.nombreComprador = "";
            $scope.traCo.correoComprador = "";
            $scope.traCo.correoAsistenteComprador = "";
            $scope.traCo.estado = "";
            $scope.traCo.detalle = [];
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
            $scope.proveedorcatalogo();
        }

    }
    $scope.nuevo = function (tipo) {


        Limpiar(tipo);

    }

    $scope.agregarProveedor= function()
    {
        var tmp = {}
        var ban = 0;
        if ($scope.selectempresa != "" && $scope.selectempresa != undefined && $scope.selectempresa.codigo!="") {
            for (var i = 0; i < $scope.pageContentProveedor.length; i++) {
                if ($scope.pageContentProveedor[i].codSap == $scope.selectempresa.codigo) {
                    $scope.MenjError = "El Proveedor ya exite en la relacion."
                    $('#idMensajeError').modal('show');
                    $scope.selectempresa = "";
                    ban = 1;
                }

            }
            tmp.codSap = $scope.selectempresa.codigo;
            tmp.nombreComercial = $scope.selectempresa.nombreproveedor;
            tmp.estado = true;
            if (ban == 0) {
                $scope.pageContentProveedor.push(tmp);
            }
        }
      
        $scope.selectempresa = "";
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
        $scope.myPromise = CompradorService.getConsulaGrid($scope.txtcodigocomprador, $scope.txtnombrecomprador, $scope.cmbestado.codigo).then(function (results) {
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
        $scope.myPromise = CompradorService.getConsulaGrid($scope.txtcodigocomprador, $scope.txtnombrecomprador, $scope.cmbestado.codigo).then(function (results) {
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
            if ($scope.pageContentProveedor.length==0) {
                $scope.MenjError = "Se debe ingresar un proveedor en la relacion."
                $('#idMensajeError').modal('show');
                return;
            }

            $scope.myPromise = null;
            $scope.myPromise = CompradorService.getGrabaComprador($scope.traCo, $scope.pageContentProveedor).then(function (results) {
                if (results.data.lSuccess) {
                    if ($scope.traCo.Tipo === "1") {
                        $scope.MenjError = "Comprador ingresado correctamente"
                        $('#idMensajeGrabarBuscar').modal('show');
                        $scope.nuevo("2");
                        //$scope.ConsultarDespues();
                        $('.nav-tabs a[href="#CompradoresRegistrados"]').tab('show');
                    }
                }
                else {
                    if (results.data.cMsgError === "EXISTE")
                        if ($scope.traCo.Tipo === "1") {
                            $scope.MenjError = "El Comprador ya exite verifique"
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

            $scope.myPromise = null;
            $scope.myPromise = CompradorService.getModiComprador($scope.traCo, $scope.pageContentProveedor).then(function (results) {
                if (results.data.lSuccess) {
                    debugger;
                    if ($scope.traCo.Tipo === "2" && results.data.cMsgError === "ACTUALIZADA") {
                        $scope.MenjError = "Comprador modificado correctamente"
                        $('#idMensajeGrabarBuscar').modal('show');
                        $scope.nuevo("3");
                        //$scope.ConsultarDespues();
                        $('.nav-tabs a[href="#CompradoresRegistrados"]').tab('show');

                    }else
                    {
                        $scope.MenjError = results.data.cMsgError;
                        $('#idMensajeError').modal('show');
                    }
                }
                else {
                    $scope.MenjError = "No se pudo actualizar el Comprador"
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
       $('.nav-tabs a[href="#RegistroCompradoresNuevos"]').tab('show');
        $scope.myPromise = null;
        $scope.myPromise = CompradorService.getConsulaGridUno(valorrecibido).then(function (results) {
            if (results.data.lSuccess) {
                var retorno = {};
                $scope.habilitar = false;
                retorno = results.data.root[0];

                $scope.div1 = true;
                $scope.div2 = false;

                $scope.traCo.idComprador = retorno[0].idComprador;
                $scope.traCo.nombreComprador = retorno[0].nombreComprador;
                $scope.traCo.correoComprador = retorno[0].correoComprador;
                $scope.traCo.correoAsistenteComprador = retorno[0].correoAsistenteComprador;
                $scope.traCo.estado = retorno[0].estado;
                $scope.traCo.Tipo = "2";
                $scope.pageContentProveedor = results.data.root[1];


             
            }
        }, function (error) {
        });
    }


    
    //Fin Archivo
    // $scope.nuevo();

}
]);