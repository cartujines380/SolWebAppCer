app.controller('mainController', function ($scope, NotificacionService) {

    // function to submit the form after all validation has occurred			
    $scope.submitForm = function () {

        // Set the 'submitted' flag to true
        $scope.isSaving = false;
        $scope.submitted = true;

        if ($scope.userForm.$valid) {
            debugger;
            $scope.submitted = false;
            NotificacionService.getRegistraMensaje($scope.user.username, $scope.user.email, $scope.user.message).then(function (results) {
                $scope.submitted = true;
                if (results.data.success) {
                    $scope.user = [];
                    alert("Mensaje Registrado.");
                    $scope.submitted  = false;
                    $scope.userForm.username.$dirty = false;
                    $scope.userForm.email.$dirty = false;
                    $scope.userForm.message.$dirty = false;
                    location.href = "#inicio";
                }
                
            }, function (error) {
                $scope.submitted = true;
            });
        }
        else {
            
        }
    };
});