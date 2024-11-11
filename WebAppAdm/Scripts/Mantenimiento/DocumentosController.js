app.controller('DocumentosController', ['$scope', 'ngAuthSettings', 'DocumentosService', 'GeneralService', 'FileUploader', '$sce', 'authService', '$http', '$filter', '$timeout', '$q', function ($scope, ngAuthSettings, DocumentosService, GeneralService,FileUploader, $sce, authService, $http, $filter, $timeout,$q) {
    $scope.message = 'Por Favor Espere...';
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    $scope.GridDocumentos = [];
    $scope.allDocumentos = [];
    $scope.myPromise = null;

    $scope.nDoc = 0;
    $scope.nDescripcion = "";
    $scope.nCodTipoPersona = "";
    $scope.nEsObligatorio = false;
    $scope.tAccion = "";
        
    $scope.ListadoDocumentos = [];
    $scope.listaClasePersona = [];
    $scope.selectedTipoPersonaCon = null;
    
  
    //Carga lista de documentos registrados
    $scope.ConsultaDocumentos = function () {

        $scope.myPromise = DocumentosService.getConsultaDocumentos().then(function (results) {
            if (results.data.success) {
                $scope.allDocumentos = results.data.root[0];

                if ($scope.allDocumentos.length > 0) {
                    for (var c=0; c < $scope.allDocumentos.length; c++){
                        if ($scope.allDocumentos[c]['esObligatorio'] == "S") {
                            $scope.allDocumentos[c]['esObligatorio'] = true;
                        }
                        else {
                            $scope.allDocumentos[c]['esObligatorio'] = false;
                        }
                    }
                }

                $scope.GridDocumentos = $scope.allDocumentos;
                $scope.etiTotRegistros = $scope.GridDocumentos.length;
            }
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            setTimeout(function () { $('#cargaDocumentos').focus(); }, 150);
        }, function (error) {
            $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
            $('#idMensajeError').modal('show');
        });

    };

    $scope.$watch('$viewContentLoaded', function () {
        $timeout(function () {
            $scope.cargaCatalagos();
        }, 500);
    });

    $scope.GrabarDoc = function (accion) {
        var nAccion = accion.substring(0, 1);

        if (nAccion == "R" || nAccion == "M") {
            var tInfo = "";

            //Validaciones
            if ($scope.selectedTipoPersonaCon.codigo == undefined) {
                $scope.showMessage('E', "No ha seleccionado un Tipo de Persona");
                return;
            }

            if ($("#txtDescripcion").val() == "") {
                $scope.showMessage('E', "No ha escrito ninguna Descripción");
                return;
            }

            if ($("#selectEstadoCon").val() == "N") {
                $scope.showMessage('E', "No ha seleccionado ningún Estado");
                return;
            }
            //Validaciones
        }

        if (nAccion == "R") {
            tInfo = "Creado";
        }
        if (nAccion == "M") {
            tInfo = "Modificado";
        }
        if (nAccion == "E") {
            tInfo = "Eliminado";
        }

        let objDocumentos = {
            'IdDocumentos': $scope.nDoc,
            'CodTipoPersona': $scope.selectedTipoPersonaCon.codigo,
            'Descripcion': $("#txtDescripcion").val(),
            'EsObligatorio': $scope.nEsObligatorio,
            'UsuarioCreacion': authService.authentication.userName,
            'Estado': $("#selectEstadoCon").val()
        };
            
        $scope.myPromise = DocumentosService.ingresaDocumentos(objDocumentos,nAccion).then(function (results) {
            if (results.data.success) {
                if (nAccion == "R") {
                    $scope.Nuevo();
                }
                $scope.ConsultaDocumentos();
                $scope.showMessage('S', 'Registro ' + tInfo + ' Exitosamente.');


            } else {
                console.log(results.data.MsgError);
                $scope.showMessage('E', results.data.MsgError);
            }
        }, function (error) {
            console.log(error);
        });
        

    };

    $scope.Nuevo = function () {
        $scope.nDoc = 0;
        $scope.nDescripcion = "";
        $scope.nCodTipoPersona = "";
        $scope.nEsObligatorio = false;
        $scope.selectedEstadoCon = "N";
        $scope.selectedTipoPersonaCon = "";

        $("#txtDescripcion").val("");

        $scope.tAccion = "Registrar";
    };

    $scope.Editar = function (id,tipopersona,descripcion,esobliga,estado) {
        $scope.nDoc = id;
        $scope.nDescripcion = descripcion;
        $scope.nCodTipoPersona = tipopersona;
        $scope.nEsObligatorio = esobliga;

        $("#txtDescripcion").val(descripcion);

        $scope.selectedEstadoCon = estado
        //Cargar tipo de persona almacenado
        $scope.listaClasePersona.forEach(function (item) {
            if (item.codigo === $scope.nCodTipoPersona) {
                $scope.selectedTipoPersonaCon = item;
            }
        });

        $scope.tAccion = "Modificar";
    };

    $scope.Eliminar = function (id, tipopersona, descripcion, esobliga) {
        $scope.nDoc = id;
        $scope.nDescripcion = descripcion;
        $scope.nCodTipoPersona = tipopersona;
        $scope.nEsObligatorio = esobliga;
        $scope.tAccion = "Eliminar";

        $scope.listaClasePersona.forEach(function (item) {
            if (item.codigo === $scope.nCodTipoPersona) {
                $scope.selectedTipoPersonaCon = item;
            }
        });

        $scope.MenjConfirmacion = "¿ Está seguro de Eliminar el registro ?";
        $('#idMensajeConfirmacion').modal('show');

    };

    $scope.grabar = function () {
        $scope.GrabarDoc($scope.tAccion);
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

    $scope.cargaCatalagos = function () {

        var promise = asyncAllCatalogos();
        promise.then(function (results) {
            $scope.listaClasePersona = results[0].data;
        }, function (error) {

        });
    };

    function asyncAllCatalogos() {

        var defered = $q.defer();
        var promesas = [];
        promesas.push(GeneralService.getCatalogo('tbl_ClaseImpuesto'));

        $scope.myPromise = $q.all(promesas).then(function (promesasRes) {
            defered.resolve(promesasRes);
        }, function (error) {
            defered.reject(error);
        }
        ).finally(function () {
        });

        return defered.promise;
    };

}]);



