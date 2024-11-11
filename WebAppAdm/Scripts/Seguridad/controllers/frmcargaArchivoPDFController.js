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
'use strict';
app.controller('frmcargaArchivoPDFController', ['$scope', '$location', 'archivoPDFService', '$sce', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', '$http', 'authService', function ($scope, $location, archivoPDFService, $sce, $cookies, ngAuthSettings, FileUploader, $filter, $http, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var Ruta = serviceBase + 'api/Upload/UploadFile/?path=ArchivoP12';
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    $scope.habilitar = true;
    $scope.txtclave = "";
    $scope.txtruc = "";
    $scope.nombreArchivo = "";
    var isVisualizacion = false;

    $scope.pUsuario = authService.authentication.userName;

    $scope.cargaInicial = function () {
        $('#idCargaMasiva').modal('show');
    }
    $scope.cargaInicial();
    var uploader = $scope.uploader = new FileUploader({
        url: Ruta
    });


    $scope.btnerror = function () {
        window.location = "/Seguridad/frmArchivoP12";
    }
    $scope.btnCancelarl = function () {
        $('#idCargaMasiva').modal('hide');
        $('#idMensajeGrabar').modal('hide');
        window.location = "/Home/Index";
        $scope.habilitar = false;
    };
    $scope.CargaMasiva = function () {
        $scope.MenjConfirmacion = "¿Está seguro de subir el archivo P12?"
        $('#idMensajeConfirmacion').modal('show');

    };
    $scope.visualizar = function () {
        isVisualizacion = true;
        if (uploader.progress == 100) {
            $('#visualizarpdf').modal('show');
        }
        else {
            //uploader2.uploadAll();
            uploader.queue[0].file.name = "PREVISUALIZACION.PDF";
            $scope.myPromise = $scope.uploader.uploadAll();
        }


    }

    $scope.grabar = function () {
     
        //var Rutanueva = serviceBase + 'api/FileP12/UploadFile/?clave=' + $scope.txtclave + '&ruc=' + authService.authentication.ruc;
        //    $scope.uploader.url = Rutanueva

        uploader.uploadAll();
        if (uploader.progress == 100) {
           
        }
       

          
            //Servicio para renombrar
      
    }
    $scope.okGrabar = function () {
        $scope.btnCancelarl();
    }
    $scope.btnerrortama = function () {
        window.location = "/Home/Index";
        $scope.habilitar = false;
    }
    $scope.nombreArchivo = "";

    //Archivo
    uploader.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "p12") {
                $scope.habilitar = false;
                return true;
            }
            else {
                $scope.MenjError = "El tipo del archivo debe ser p12."
                $('#idMensajeError').modal('show');
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
                $scope.MenjError = "El archivo sobrepasa los 5MB permitidos.";
                $('#idMensajeError').modal('show');
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
        $scope.nombreArchivo = fileItem.file.name;
    };

    uploader.onSuccessItem = function (fileItem, response, status, headers) {
        if ($scope.uploader.progress == 100 ) {

            //uploader.clearQueue();
            //$('#visualizarpdf').modal('show');
            archivoPDFService.getCargaArchivoP12($scope.nombreArchivo, $scope.txtclave, $scope.txtruc, $scope.pUsuario).then(function (response) {
                
                if (response.data == "01/01/0001,01/01/0001")
                {
                    //$scope.uploader.queue = [];
                    $scope.uploader.progress = 0;
                    $scope.MenjError = "El ruc o clave esta incorrecto";
                    $('#idMensajeError').modal('show');
                }
                else
                {
                    if(response.data =="-1")
                    {
                        $scope.uploader.progress = 0;
                        $scope.MenjError = "El archivo ya fue subido por favor verifique";
                        $('#idMensajeError').modal('show');
                    }else{
                        $scope.MenjError = "Se grabo el archivo sin problema";
                        $('#idMensajeGrabar').modal('show');
                        }
                }
                
                //$scope.MejError = "Se actualizó el archivo correctamente"
                //$('#idMensajeGrabar').modal('show');
            }, function (err) {

            });
            //$scope.MenjError = "Se actualizó el archivo correctamente"
            //$('#idMensajeGrabar').modal('show');
        }
        // if ($scope.uploader.progress==100)
        //   alert('Selected file has been uploaded successfully.');
        //$scope.uploader.queue = [];
        //$scope.uploader.progress = 0;
        //alert('Selected file has been uploaded successfully.');
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
}
]);