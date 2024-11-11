
'use strict';
app.controller('VerArchivoPDFController', ['$scope', '$location',  '$sce', '$cookies', '$filter', '$http', function ($scope, $location,  $sce, $cookies,  $filter, $http) {

  
    $scope.visualizar = function () {
  
        $('#visualizarpdfpregunta').modal('show');
      


    }

    $scope.visualizar2 = function () {

        $('#visualizarpdfpregunta2').modal('show');



    }
    
    $scope.visualizar3 = function () {

        $('#visualizarpdfpregunta3').modal('show');



    }

    $scope.visualizar4 = function () {

        $('#visualizarVideopregunta4').modal('show');
    }
    $scope.visualizar5 = function () {

        $('#visualizarVideopregunta5').modal('show');
    }
    $scope.visualizar6 = function () {

        $('#visualizarVideopregunta6').modal('show');
    }
    $scope.visualizar7 = function () {

        $('#visualizarVideopregunta7').modal('show');
    }
}
]);