(function () {
    'use strict';

    var app = angular.module('AngularAuthApp');

    // Collect the routes
    app.constant('menus', getMenus());
    app.constant('routes', getItems());

    // Configure the routes and route resolvers
    app.config(['$routeProvider', 'routes',  routeConfigurator]);

    function routeConfigurator($routeProvider, routes) {

        routes.forEach(function (r) {
            //$routeProvider.when(r.url, r.config);
            setRoute(r.url, r.config);
        });
        $routeProvider.otherwise({ redirectTo: '/' });

        function setRoute(url, config) {
            //Sets resolver for all the routes
            //by extending any existing resolvers (or create a new one.)
            config.resolve = angular.extend(config.resolve || {},
            {
                checkSecurity: checkSecurity
            });

            $routeProvider.when(url, config);
            return $routeProvider;
        }
    }

    checkSecurity.$inject = ['$route', 'authService', 'common'];
    function checkSecurity($route, authService, common) {
        var deferred = common.$q.defer();
        authService.fillData().then(function () {
            var settings = $route.current.settings;
            var loginRequired = settings.loginRequired || false;
            var roles = settings.roles || [];
            if (loginRequired) {
                if (!authService.authData.isAuth) {
                    common.$location.path('/login');
                } else {
                    if (roles.length > 0) {
                        if (!common.checkRole(authService.authData.roles, roles)) {
                            common.$location.path('/notauthorized').replace();
                        }
                    }
                }
            }
            deferred.resolve(true); //We want to return just true even if we have to re-route. 
                                    //If we returned an reject, the the global handler will re-route us to home
        }, function(error) {
            deferred.reject(error);
        });

        return deferred.promise;               
    }

    function getMenus() {
        return [
            {
                MenuPadre: '001',
                url: '/home/about',
                config: {
                    //templateUrl: 'app/home/about.html',
                    title: 'about',
                    settings: {
                        nav: 2,
                        loginRequired: false,
                        roles: [],
                        content: '<i class="fa fa-building" style="color:black !Important"></i> About'
                    }
                }
            },
            {
                MenuPadre: '001',
                url: '/home/Contact',
                config: {
                    title: 'users',
                    //templateUrl: 'app/users/users.html',
                    settings: {
                        nav: 3,
                        loginRequired: false,
                        roles: ['Admin', 'User'],
                        content: '<i class="fa fa-users"></i> People'
                    }
                }
            },
            {
                MenuPadre: '002',
                url: '/Notificacion/frmBandejaNotificaciones',
                config: {
                    title: 'user detail',
                    //templateUrl: 'app/users/userdetail.html',
                    settings: {
                        nav: 4,
                        loginRequired: false,
                        roles: ['User'],
                        content: '<i class="fa fa-users"></i>  Generar notificaciones'
                    }
                }
            }, {
                MenuPadre: '003',
                config: {
                    title: 'Dummy',
                    settings: {
                        nav: 10000,
                        loginRequired: false,
                        roles: ['User'],
                        content: ''
                    }
                }
            }, {
                MenuPadre: '002',
                url: '/login',
                config: {
                    title: 'login',
                    templateUrl: 'app/users/login.html',
                    settings: {
                    }
                }
            }, {
                MenuPadre: '002',
                url: '/notauthorized',
                config: {
                    title: 'not authorized',
                    templateUrl: 'app/home/notauthorized.html',
                    settings: {
                    }
                }
            }
        ];
    }
    // Define the routes 
    //function getMenus( ) {


    //    return [
    //          {
    //            MenuId: '001',
    //            nav: 1,
    //            title: 'Comunicaciones',
    //            loginRequired: false,
    //            content: '<i class="menu-icon fa fa-book !Important"></i> <span class="menu-text">Comunicaciones</span>',
    //            roles: []
    //            }
    //          ,{
    //              MenuId: '002',
    //              nav: 2,
    //              title: 'Notificaciones',
    //              loginRequired: false,
    //              content: '<i class="menu-icon fa fa-newspaper-o !Important"></i> <span class="menu-text">Notificaciones</span>',
    //              roles: []

    //            }
    //          , {
    //              MenuId: '003',
    //              nav: 3,
    //              title: 'Acceso de Usuarios',
    //              loginRequired: false,
    //              content: '<i class="menu-icon fa fa-key !Important"></i> <span class="menu-text">Acceso de Usuarios</span>',
    //              roles: []
               
    //          }, {
    //            MenuId: '004',
    //            url: '/login',
    //            config: {
    //                title: 'login',
    //                templateUrl: 'app/users/login.html',
    //                settings: {                        
    //                }
    //            }
    //        }, {
    //            MenuId: '005',
    //            url: '/notauthorized',
    //            config: {
    //                title: 'not authorized',
    //                templateUrl: 'app/home/notauthorized.html',
    //                settings: {
    //                }
    //            }
    //        }
    //    ];
    //}

    function getItems() {
        return [
            {
                MenuPadre: '001',
                url: '/home/about',
                config: {
                    //templateUrl: 'app/home/about.html',
                    title: 'about',
                    settings: {
                        nav: 2,
                        loginRequired: false,
                        roles: [],
                        content: '<i class="fa fa-building" style="color:black !Important"></i> About'
                    }
                }
            },
            {
                MenuPadre: '001',
                url: '/home/Contact',
                config: {
                    title: 'users',
                    //templateUrl: 'app/users/users.html',
                    settings: {
                        nav: 3,
                        loginRequired: false,
                        roles: ['Admin', 'User'],
                        content: '<i class="fa fa-users"></i> People'
                    }
                }
            },
            {
                MenuPadre: '002',
                url: '/Notificacion/frmBandejaNotificaciones',
                config: {
                    title: 'user detail',
                    //templateUrl: 'app/users/userdetail.html',
                    settings: {
                        nav: 4,
                        loginRequired: false,
                        roles: ['User'],
                        content: '<i class="fa fa-users"></i>  Generar notificaciones'
                    }
                }
            }, {
                MenuPadre: '003',
                config: {
                    title: 'Dummy',
                    settings: {
                        nav: 10000,
                        loginRequired: false,
                        roles: ['User'],
                        content: ''
                    }
                }
            }, {
                MenuPadre: '002',
                url: '/login',
                config: {
                    title: 'login',
                    templateUrl: 'app/users/login.html',
                    settings: {
                    }
                }
            }, {
                MenuPadre: '002',
                url: '/notauthorized',
                config: {
                    title: 'not authorized',
                    templateUrl: 'app/home/notauthorized.html',
                    settings: {
                    }
                }
            }
        ];
    }

})();