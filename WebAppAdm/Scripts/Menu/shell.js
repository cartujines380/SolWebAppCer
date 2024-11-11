(function () {
    'use strict';

    var controllerId = 'shell';

    angular
        .module('AngularAuthApp')
        .controller(controllerId, shell);

    shell.$inject = ['$rootScope', 'authService', 'common', 'config', 'menus', 'routes', 'SeguridadService'];

    function shell($rootScope, authService, common, config, menus, routes, SeguridadService) {
        /* jshint validthis:true */
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
        vm.logOut = logOut;
        vm.displayNav = displayNav;
        vm.Permisos = [];

        activate();

        function activate() {
            getNavMenus();
            getNavRoutes();
            common.activateController([], controllerId).then(function () {
                common.logger.logSuccess('Aplicacion cargada', true);
                vm.showSplash = false;
            });
        }


        
        //Carga de Menus


        SeguridadService.getMenusS().then(function (results) {
            if (results.data.success) {
                vm.Permisos = results.data.root[0];
            }
        }, function (error) {
        });


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

        function getNavMenus() {

            vm.navMenus = menus.filter(function (r) {
               return r.config.settings && r.config.settings.nav;
            }).sort(function (r1, r2) {
                return r1.config.settings.nav - r2.config.settings.nav;
            });

        }

        function getNavRoutes() {
            vm.navRoutes = routes.filter(function (r) {
                return r.config.settings && r.config.settings.nav;
            }).sort(function (r1, r2) {
                return r1.config.settings.nav - r2.config.settings.nav;
            });
        }



        function logOut() {
            authService.logOut();
        }

        function toggleSpinner(on) { vm.isBusy = on; }

        $rootScope.$on('$routeChangeStart',
            function(event, next, current) {
                 toggleSpinner(true);
            }
        );

        $rootScope.$on(config.events.controllerActivateSuccess,
            function(data) {
                 toggleSpinner(false);
            }
        );

        $rootScope.$on(config.events.spinnerToggle,
            function(data) {
                 toggleSpinner(data.show);
            }
        );
    }
})();
