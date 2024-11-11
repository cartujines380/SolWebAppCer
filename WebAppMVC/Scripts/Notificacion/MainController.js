app.controller('mainController', function ($scope, NotificacionService) {

    // function to submit the form after all validation has occurred			
    $scope.submitForm = function () {

        // Set the 'submitted' flag to true
        $scope.isSaving = false;
        $scope.submitted = true;

        if ($scope.userForm.$valid) {
            debugger;
            $scope.submitted = false;
            NotificacionService.getRegistraNuevoProv($scope.user.username, $scope.user.email, $scope.user.telefono, "", $scope.user.message).then(function (results) {
                $scope.submitted = true;
                if (results.data.success) {
                    $scope.user = [];
                    $scope.MenjError = 'Mensaje registrado.';
                    $('#idMensajeOk').modal('show');
                    $scope.submitted = false;
                    $scope.userForm.username.$dirty = false;
                    $scope.userForm.email.$dirty = false;
                    $scope.userForm.message.$dirty = false;
                    //location.href = "#inicio";
                }
                else {
                    $scope.MenjError = 'Error al registrar mensaje.';
                    $('#idMensajeError').modal('show');
                }
                
            }, function (error) {
                $scope.submitted = true;
            });
        }
        else {
            
        }
    };
});