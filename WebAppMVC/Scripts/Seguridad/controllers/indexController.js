'use strict';




app.controller('indexController', ['$scope', '$location', 'authService', 'SeguridadService', 'Idle', 'Keepalive', function ($scope, $location, authService, SeguridadService, Idle, Keepalive) {
   var vm = this;
    vm.title = 'shell';
    vm.busyMessage = 'Please wait ...';
    vm.isBusy = true;
    vm.spinnerOptions = {
        radius: 40,
        lines: 7,
        length: 0,
        width: 30,
        speed: 1.7,
        corners: 1.0,
        trail: 100,
        color: '#F58A00'
    };
    vm.showSplash = true;
    vm.authentication = authService.authentication;
    vm.logOut = $scope.logOut;
    vm.displayNav = displayNav;
    vm.Permisos = [];

    var roles = authService.authentication.roles;
    
    

    $scope.arsubmenu = {};
    $scope.arsubmenu.nbp2405 = true;
    $scope.arsubmenu.nbp2410 = true;
    $scope.arsubmenu.nbp2415 = true;
    $scope.arsubmenu.nbp2420 = true;
    $scope.arsubmenu.nbp2710 = true;
    $scope.arsubmenu.nbp2715 = true;
    $scope.arsubmenu.nbp2605 = true;
    $scope.arsubmenu.nbp2610 = true;
    $scope.arsubmenu.nbp2615 = true;
    $scope.arsubmenu.nbp2505 = true;
    $scope.arsubmenu.nbp2510 = true;
    $scope.arsubmenu.nbp2515 = true;
    $scope.arsubmenu.nbp2520 = true;
    $scope.arsubmenu.nbp2805 = true;
    $scope.arsubmenu.nbp2810 = true;
    $scope.arsubmenu.nbp2205 = true;
    $scope.arsubmenu.nbp2115 = true;
    $scope.arsubmenu.nbp2110 = true;
    $scope.arsubmenu.nbp2105 = true;
    $scope.arsubmenu.nbp2302 = true;
    $scope.arsubmenu.nbp2305 = true;
    $scope.arsubmenu.nbp2310 = true;
    $scope.arsubmenu.nbp2315 = true;
    $scope.arsubmenu.nbp2320 = true;
    $scope.arsubmenu.nbp2325 = true;
    $scope.arsubmenu.nbp2330 = true;
    $scope.arsubmenu.nbp2335 = true;
    $scope.arsubmenu.nbp2340 = true;
    $scope.arsubmenu.nbp2345 = true;
    $scope.arsubmenu.nbp2350 = true;
    $scope.arsubmenu.nbp2355 = true;
    $scope.arsubmenu.nbp2360 = true;
    $scope.arsubmenu.nbp2220 = true;
    $scope.arsubmenu.nbp2225 = true;
    $scope.arsubmenu.nbp2230 = true;
    $scope.arsubmenu.nbp2120 = true;
    $scope.arsubmenu.nbp2125 = true;
    $scope.arsubmenu.nbp2130 = true;

    $scope.arsubmenu.nbp2400 = true;
    $scope.arsubmenu.nbp2700 = true;
    $scope.arsubmenu.nbp2600 = true;
    $scope.arsubmenu.nbp2500 = true;
    $scope.arsubmenu.nbp2800 = true;
    $scope.arsubmenu.nbp2200 = true;
    $scope.arsubmenu.nbp2100 = true;
    $scope.arsubmenu.nbp2300 = true;

    $scope.arsubmenu.nbp2900 = true;
    $scope.arsubmenu.nbp2905 = true;

    $scope.arsubmenu.nbp2425 = true;
    $scope.arsubmenu.nbp2525 = true;
    $scope.arsubmenu.nbp2506 = true;

    $scope.arsubmenu.nbp2303 = true;
    $scope.arsubmenu.nbp2304 = true;
    $scope.arsubmenu.etiqueta = true;

    $scope.arsubmenu.nbp3200 = true;
    $scope.arsubmenu.nbp3201 = true;
    $scope.arsubmenu.nbp3202 = true;

    $scope.arsubmenu.nbp3100 = true;
    $scope.arsubmenu.nbp3105 = true;

    $scope.arsubmenu.nbp4000 = true;
    $scope.arsubmenu.nbp4001 = true;
    $scope.arsubmenu.nbp4002 = true;
    $scope.arsubmenu.nbp4003 = true;
    $scope.arsubmenu.nbp4004 = true;
    $scope.arsubmenu.nbp4005 = true;
    $scope.arsubmenu.nbp4006 = true;
    $scope.arsubmenu.nbp4007 = true;
    $scope.arsubmenu.nbp4008 = true;

    $scope.arsubmenu.nbp4400 = true;
    $scope.arsubmenu.nbp4401 = true;
    $scope.arsubmenu.nbp4402 = true;
    $scope.arsubmenu.nbp4403 = true;


    if (authService.authentication.esEtiqueta=='true') {
        $scope.arsubmenu.etiqueta = true;
    }else
    {
        $scope.arsubmenu.etiqueta=false;
    }

    function menuoc(index) {
        if (index == 4000) {
            $scope.arsubmenu.nbp4000 = false;
        }
        if (index == 4001) {
            $scope.arsubmenu.nbp4001 = false;
        }
        if (index == 4002) {
            $scope.arsubmenu.nbp4002 = false;
        }
        if (index == 4003) {
            $scope.arsubmenu.nbp4003 = false;
        }
        if (index == 4004) {
            $scope.arsubmenu.nbp4004 = false;
        }
        if (index == 4005) {
            $scope.arsubmenu.nbp4005 = false;
        }
        if (index == 4006) {
            $scope.arsubmenu.nbp4006 = false;
        }
        if (index == 4007) {
            $scope.arsubmenu.nbp4007 = false;
        }
        if (index == 4008) {
            $scope.arsubmenu.nbp4008 = false;
        }
        if (index == 2303) {
            $scope.arsubmenu.nbp2303 = false;
        }
        if (index == 2304) {
            $scope.arsubmenu.nbp2304 = false;
        }
        if (index == 2506) {
            $scope.arsubmenu.nbp2506 = false;
        }
        if (index == 2525) {
            $scope.arsubmenu.nbp2525 = false;
        }
        if (index == 2425) {
            $scope.arsubmenu.nbp2425 = false;
        }
        if (index == 2900) {
            $scope.arsubmenu.nbp2900 = false;
        }
        if (index == 2905) {
            $scope.arsubmenu.nbp2905 = false;
        }
        if (index == 2405) {
            $scope.arsubmenu.nbp2405 = false;
        }
        if (index == 2215) {
            $scope.arsubmenu.nbp2215 = false;
        }
        if (index == 2220) {
            $scope.arsubmenu.nbp2220 = false;
        }
        if (index == 2230) {
            $scope.arsubmenu.nbp2230 = false;
        }
        if (index == 2120) {
            $scope.arsubmenu.nbp2120 = false;
        }
        if (index == 2125) {
            $scope.arsubmenu.nbp2125 = false;
        }
        if (index == 2130) {
            $scope.arsubmenu.nbp2130 = false;
        }
        if (index == 2225) {
            $scope.arsubmenu.nbp2225 = false;
        }
        if (index == 2410) {
            $scope.arsubmenu.nbp2410 = false;
        }
        if (index == 2415) {
            $scope.arsubmenu.nbp2415 = false;
        }
        if (index == 2420) {
            $scope.arsubmenu.nbp2420 = false;
        }
        if (index == 2710) {
            $scope.arsubmenu.nbp2710 = false;
        }
        if (index == 2715) {
            $scope.arsubmenu.nbp2715 = false;
        }
        if (index == 2605) {
            $scope.arsubmenu.nbp2605 = false;
        }
        if (index == 2610) {
            $scope.arsubmenu.nbp2610 = false;
        }
        if (index == 2615) {
            $scope.arsubmenu.nbp2615 = false;
        }
        if (index == 2505) {
            $scope.arsubmenu.nbp2505 = false;
        }
        if (index == 2510) {
            $scope.arsubmenu.nbp2510 = false;
        }
        if (index == 2515) {
            $scope.arsubmenu.nbp2515 = false;
        }
        if (index == 2520) {
            $scope.arsubmenu.nbp2520 = false;
        }
        if (index == 2805) {
            $scope.arsubmenu.nbp2805 = false;
        }
        if (index == 2810) {
            $scope.arsubmenu.nbp2810 = false;
        }
        if (index == 2205) {
            $scope.arsubmenu.nbp2205 = false;
        }
        if (index == 2110) {
            $scope.arsubmenu.nbp2110 = false;
        }
        if (index == 2105) {
            $scope.arsubmenu.nbp2105 = false;
        }
        if (index == 2115) {
            $scope.arsubmenu.nbp2115 = false;
        }
        if (index == 2302) {
            $scope.arsubmenu.nbp2302 = false;
        }
        if (index == 2305) {
            $scope.arsubmenu.nbp2305 = false;
        }
        if (index == 2310) {
            $scope.arsubmenu.nbp2310 = false;
        }
        if (index == 2315) {
            $scope.arsubmenu.nbp2315 = false;
        }
        if (index == 2320) {
            $scope.arsubmenu.nbp2320 = false;
        }
        if (index == 2325) {
            $scope.arsubmenu.nbp2325 = false;
        }
        if (index == 2330) {
            $scope.arsubmenu.nbp2330 = false;
        }
        if (index == 2335) {
            $scope.arsubmenu.nbp2335 = false;
        }
        if (index == 2340) {
            $scope.arsubmenu.nbp2340 = false;
        }
        if (index == 2345) {
            $scope.arsubmenu.nbp2345 = false;
        }
        if (index == 2350) {
            $scope.arsubmenu.nbp2350 = false;
        }
        if (index == 2355) {
            $scope.arsubmenu.nbp2355 = false;
        }
        if (index == 2360) {
            $scope.arsubmenu.nbp2360 = false;
        }
        if (index == 2400) {
            $scope.arsubmenu.nbp2400 = false;
        }
        if (index == 2700) {
            $scope.arsubmenu.nbp2700 = false;
        }
        if (index == 2600) {
            $scope.arsubmenu.nbp2600 = false;
        }
        if (index == 2500) {
            $scope.arsubmenu.nbp2500 = false;
        }
        if (index == 2800) {
            $scope.arsubmenu.nbp2800 = false;
        }
        if (index == 2200) {
            $scope.arsubmenu.nbp2200 = false;
        }
        if (index == 2100) {
            $scope.arsubmenu.nbp2100 = false;
        }
        if (index == 2300) {
            $scope.arsubmenu.nbp2300 = false;
        }
        if (index == 3200) {
            $scope.arsubmenu.nbp3200 = false;
        }
        if (index == 3201) {
            $scope.arsubmenu.nbp3201 = false;
        }
        if (index == 3202) {
            $scope.arsubmenu.nbp3202 = false;
        }
        if (index == 3100) {
            $scope.arsubmenu.nbp3100 = false;
        }
        if (index == 3105) {
            $scope.arsubmenu.nbp3105 = false;
        }
        if (index == 4400) {
            $scope.arsubmenu.nbp4400 = false;
        }
        if (index == 4401) {
            $scope.arsubmenu.nbp4401 = false;
        }
        if (index == 4402) {
            $scope.arsubmenu.nbp4402 = false;
        }
        if (index == 4403) {
            $scope.arsubmenu.nbp4403 = false;
        }
    }

    function menu()
    {
        if (roles != undefined)
        for (var i = 0; i < roles.length; ++i) {
            menuoc(roles[i]);
        }
    }
    menu();
    function displayNav(r) {
        var okayToGo = false;
        var settings = r.config.settings;
        var loginRequired = settings.loginRequired || false;
        var roles = settings.roles || [];
        if (loginRequired) {
            if (!authService.authentication.isAuth) {
            } else {
                if (roles.length > 0) {
                    if (!common.checkRole(authService.authentication.roles, roles)) {
                    } else {
                        okayToGo = true;
                    }
                } else {
                    okayToGo = true;
                }
            }
        } else {
            okayToGo = true;
        }
        return okayToGo;
    }

    $scope.logOut = function () {
        var URLactual = window.location;
        if ((URLactual.href.indexOf("frmIngPreCalifica") !== -1) || (URLactual.href.indexOf("frmSolictud") !== -1) || (URLactual.href.indexOf("frmConsultaProveedorSol") !== -1)) {
            $('#idMensajeCerrarSesion').modal('show');
        }
        else {
            authService.logOut();
            window.location = "../Home/Index";
        }
    }

    $scope.confirmaCerrar = function () {
        authService.logOut();
        window.location = "../Home/Index";
    }

    

    $scope.authentication = authService.authentication;


    $scope.started = false;

    function closeModals() {
        authService.refreshToken;
    }





    $scope.$on('IdleStart', function () {
        closeModals();
        authService.logOut();
        $('#muerteSessiontiempo').modal('show');

    });

    $scope.$on('IdleEnd', function () {
        closeModals();
        $('#muerteSessiontiempo').modal('hide');
    });

    $scope.$on('IdleTimeout', function () {
        $('#muerteSessiontiempo').modal('hide');
        $('#muerteSession').modal('show');
    });

    $scope.start = function () {
        if (authService.authentication.isAuth == true) {
            closeModals();
            Idle.watch();
            $scope.started = true;
        }
    };

    $scope.stop = function () {
        closeModals();
        Idle.unwatch();
        $scope.started = false;

    };

    $scope.start();

    $scope.modalClose = function ($scope, $modalInstance) {
        $scope.cancelLogin = function () {
            $modalInstance.dismiss('cancel');
        }
    }

}]);







