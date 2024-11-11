app.controller('ProductosNuevosController', ['$scope', 'ngAuthSettings', 'ProductosNuevosService', 'FileUploader', '$sce', 'authService', '$http', '$filter', function ($scope, ngAuthSettings, ProductosNuevosService, FileUploader, $sce, authService, $http, $filter) {
    $scope.MenjError = "";

    $scope.visible = true;


    $scope.btnRightVisible = true;

    $scope.OnClickBtnArrowRight = function (mostrar) {
        $scope.visible = false;
        $scope.btnRightVisible = false;

    }

    $scope.OnClickBtnArrowLeft = function (mostrar) {
        $scope.visible = true;
        $scope.btnRightVisible = true;

    }

    $scope.getVisblePanel = function (visible) {
        if (!visible)
            return { 'display': 'none' };
        else
            return { 'display': 'block' };
    }

    
}]);