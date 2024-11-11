'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', 'SeguridadService', 'Idle', 'Keepalive', function ($scope, $location, authService, SeguridadService, Idle, Keepalive) {

    if (!authService.authentication.isAuth) {
        window.location = "../";
    }

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
    $scope.arsubmenu.nbp2620 = true;
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
    $scope.arsubmenu.nbp2365 = true;
    $scope.arsubmenu.nbp2380 = true;
    $scope.arsubmenu.nbp2381 = true;
    $scope.arsubmenu.nbp2220 = true;
    $scope.arsubmenu.nbp2225 = true;
    $scope.arsubmenu.nbp2230 = true;
    $scope.arsubmenu.nbp2150 = true;
    $scope.arsubmenu.nbp2120 = true;
    $scope.arsubmenu.nbp2125 = true;
    $scope.arsubmenu.nbp2130 = true;
    $scope.arsubmenu.nbp2250 = true;
    $scope.arsubmenu.nbp2215 = true;
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
    $scope.arsubmenu.nbp2910 = true;

    $scope.arsubmenu.nbp2425 = true;

    $scope.arsubmenu.nbp3200 = true;
    $scope.arsubmenu.nbp3201 = true;
    $scope.arsubmenu.nbp3202 = true;

    $scope.arsubmenu.nbp3100 = true;
    $scope.arsubmenu.nbp3105 = true;

    $scope.arsubmenu.nbp3500 = true;
    $scope.arsubmenu.nbp3505 = true;
    $scope.arsubmenu.nbp3510 = true;
    $scope.arsubmenu.nbp3515 = true;
    $scope.arsubmenu.nbp2375 = true;
    $scope.arsubmenu.nbp2370 = true;
    $scope.arsubmenu.nbp3516 = true;
    $scope.arsubmenu.nbp2915 = true;
    $scope.arsubmenu.nbp2255 = true;

    $scope.arsubmenu.nbp3517 = true;
    $scope.arsubmenu.nbp3518 = true;
    $scope.arsubmenu.nbp3519 = true;

    $scope.arsubmenu.nbp4100 = true;
    $scope.arsubmenu.nbp4101 = true;
    $scope.arsubmenu.nbp4102 = true;
    $scope.arsubmenu.nbp4200 = true;
    $scope.arsubmenu.nbp4201 = true;
    $scope.arsubmenu.nbp4202 = true;
    $scope.arsubmenu.nbp4203 = true;

    $scope.arsubmenu.nbp2260 = true;
    $scope.arsubmenu.nbp2265 = true;

    //MODULO SEGURIDAD
    $scope.arsubmenu.nbp4400 = true;
    $scope.arsubmenu.nbp4401 = true;
    $scope.arsubmenu.nbp4402 = true;
    $scope.arsubmenu.nbp4403 = true;

    function menuoc(index) {
        if (index == 3517) {
            $scope.arsubmenu.nbp3517 = false;
        }
        if (index == 3518) {
            $scope.arsubmenu.nbp3518 = false;
        }
        if (index == 3519) {
            $scope.arsubmenu.nbp3519 = false;
        }
        if (index == 2255) {
            $scope.arsubmenu.nbp2255 = false;
        }
        if (index == 2915) {
            $scope.arsubmenu.nbp2915 = false;
        }

        if (index == 3516) {
            $scope.arsubmenu.nbp3516 = false;
        }
        if (index == 2375) {
            $scope.arsubmenu.nbp2375 = false;
        }
        if (index == 2370) {
            $scope.arsubmenu.nbp2370 = false;
        }
        if (index == 2380) {
            $scope.arsubmenu.nbp2380 = false;
        }
        if (index == 2381) {
            $scope.arsubmenu.nbp2381 = false;
        }
        if (index == 3500) {
            $scope.arsubmenu.nbp3500 = false;
        }
        if (index == 3505) {
            $scope.arsubmenu.nbp3505 = false;
        }
        if (index == 3510) {
            $scope.arsubmenu.nbp3510 = false;
        }
        if (index == 3515) {
            $scope.arsubmenu.nbp3515 = false;
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
        if (index == 2910) {
            $scope.arsubmenu.nbp2910 = false;
        }
        if (index == 2405) {
            $scope.arsubmenu.nbp2405 = false;
        }
        if (index == 2250) {
            $scope.arsubmenu.nbp2250 = false;
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
        if (index == 2150) {
            $scope.arsubmenu.nbp2150 = false;
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
        if (index == 2620) {
            $scope.arsubmenu.nbp2620 = false;
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
        if (index == 2365) {
            $scope.arsubmenu.nbp2365 = false;
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
        if (index == 4100) {
            $scope.arsubmenu.nbp4100 = false;
        }
        if (index == 4101) {
            $scope.arsubmenu.nbp4101 = false;
        }
        if (index == 4102) {
            $scope.arsubmenu.nbp4102 = false;
        }
        if (index == 4200) {
            $scope.arsubmenu.nbp4200 = false;
        }
        if (index == 4201) {
            $scope.arsubmenu.nbp4201 = false;
        }
        if (index == 3100) {
            $scope.arsubmenu.nbp3100 = false;
        }
        if (index == 3105) {
            $scope.arsubmenu.nbp3105 = false;
        }
        if (index == 4202) {
            $scope.arsubmenu.nbp4202 = false;
        }
        if (index == 4203) {
            $scope.arsubmenu.nbp4203 = false;
        }
        if (index == 2260) {
            $scope.arsubmenu.nbp2260 = false;
        }
        if (index == 2265) {
            $scope.arsubmenu.nbp2265 = false;
        }
        //MODULO SEGURIDAD
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

    function menu() {
        if (roles != undefined)
            for (var i = 0; i < roles.length; ++i) {
                menuoc(roles[i]);
            }
    }
    menu();

    function displayNav(r) {
        var okayToGo = false;
        //The logic below is very similar to the logic in the config.route.js 
        //checkSecurity function. It should probably be combined into a 
        //common function
        var settings = r.config.settings;
        var loginRequired = settings.loginRequired || false;
        var roles = settings.roles || [];
        if (loginRequired) {
            if (!authService.authentication.isAuth) {
                //nothing to do
            } else {
                if (roles.length > 0) {
                    if (!common.checkRole(authService.authentication.roles, roles)) {
                        //nothing to do
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
        if ((URLactual.href.indexOf("frmConsultaProveedorSol") !== -1)) {
            $('#idMensajeCerrarSesion').modal('show');
        }
        else {
            authService.logOut();
            window.location = "../Home/Index";
        }
    }

    $scope.confirmaCerrar = function () {
        authService.logOut();
        window.location = "../Account/SignOut";
    }
   
    authService.authentication.NombreParticipante = authService.authentication.NombreParticipante.toUpperCase();
    $scope.authentication = authService.authentication;

    $scope.started = false;

    function closeModals() {
        authService.refreshToken;
    }

    $scope.$on('IdleStart', function () {
        closeModals();
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
        //edit to close modal login
        $scope.cancelLogin = function () {
            $modalInstance.dismiss('cancel');
        }
    }
    //#region RFD0 - 2022 - 155: Módulo de seguridad

    $scope.AuditarLogAcceso = function (Trx) {
        $scope.myPromise = SeguridadService.verificarTransaccion(Trx).then(function (results) { }, function (error) { });
    };
    //#endregion



}]);







