app.controller('ManualesController', ['$scope', '$location', '$sce', '$cookies', 'ngAuthSettings', 'FileUploader', '$filter', '$http', function ($scope, $location,  $sce, $cookies, ngAuthSettings, FileUploader, $filter, $http) {
    $scope.cargaPdf = function (dato) {
        if(dato==1)
        {
            window.open("../PDF/Manual de UsuarioProveedoresv1.1.pdf", '_blank', "");
        }
        if (dato == 2) {
            window.open("../PDF/Manual de UsuarioProveedores - FacturaManual v1.1.pdf", '_blank', "");
        }
        if (dato == 3) {
            window.open("../PDF/Manual de UsuarioProveedores - Notificaciones.pdf", '_blank', "");
        }
        if (dato == 4) {
            window.open("../PDF/Manual de UsuarioProveedores-ÓrdenesCompra.pdf", '_blank', "");
        }
        if (dato == 5) {
            window.open("../PDF/Proximamente.pdf", '_blank', "");
        }
        if (dato == 6) {
            window.open("../PDF/Portal-Presentación-Proveedores-Marzo2016-VF.pdf", '_blank', "");
        }
        if (dato == 7) {
            window.open("../PDF/Manual del Proveedores_UsuarioMaster.pdf", '_blank', "");
        }
        if (dato == 8) {
            window.open("../PDF/Manual del Proveedores_UsuarioROL_Administrador.pdf", '_blank', "");
        }
        if (dato == 9) {
            window.open("../PDF/Manual del Proveedores_UsuarioROL_Comercial.pdf", '_blank', "");
        }
        if (dato == 10) {
            window.open("../PDF/USUARIOS DEL KIOSKO EN TIENDAS.pdf", '_blank', "");
        }
    }
}
]);