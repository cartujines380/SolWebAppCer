 
var app = angular.module('AngularAuthApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'angularjs-dropdown-multiselect', 'ngCookies', 'angularFileUpload', 'common', 'ngSanitize', 'cgBusy', 'ngIdle', 'ui.calendar', 'highcharts-ng']);

app.config(function (IdleProvider, KeepaliveProvider) {
    IdleProvider.idle(600);
    IdleProvider.timeout(120)
    KeepaliveProvider.interval(10);
});
app.config(['$routeProvider',
 function ($routeProvider) {
     $routeProvider.
       when('/FrmConsolidacion', {
           templateUrl: '../Transporte/frmConsolidacion',
           controller: 'Transporte/ConsolidacionPedidosController'
       }).

       otherwise({
           redirectTo: '/addOrder'
       });
 }]);
app.config(['$httpProvider', function ($httpProvider) {
    //initialize get if not there
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }

    // Answer edited to include suggestions from comments
    // because previous version of code introduced browser-related errors

    //disable IE ajax request caching
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
    // extra
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
}]);



app.config(function ($routeProvider) {
    $routeProvider.when("/Pedidos", {
        templateUrl: "Pedidos/frmConsPedidos.cshtml",
	    controller: "frmConsPedidosController"
	})
	.otherwise({ reditrectTo: "/" });
})
app.config(function ($routeProvider, $locationProvider, $compileProvider) {

    $locationProvider.html5Mode(false);

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/scripts/seguridad/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/scripts/seguridad/views/signup.html"
    });

    $routeProvider.when("/orders", {
        controller: "ordersController",
        templateUrl: "/app/views/orders.html"
    });

    $routeProvider.when("/refresh", {
        controller: "refreshController",
        templateUrl: "/app/views/refresh.html"
    });

    $routeProvider.when("/tokens", {
        controller: "tokensManagerController",
        templateUrl: "/app/views/tokens.html"
    });

    $routeProvider.when("/associate", {
        controller: "associateController",
        templateUrl: "/app/views/associate.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });

});

var serviceBase = 'http://localhost:26265/';
//var serviceBase = 'http://10.2.7.244/CerWebApi/';
//var serviceBase = 'http://192.168.0.154:8888/';


//var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: '98b0dc2582b703018e15020c1c3cbe52'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

app.directive('ncgRequestVerificationToken', ['$http', function ($http) {
    return function (scope, element, attrs) {
        $http.defaults.headers.common['RequestVerificationToken'] = attrs.ncgRequestVerificationToken || "no request verification token";
    };
}]);


//incluir directiva ecetre
app.directive('validFile', function () {
    return {
        require: 'ngModel',
        link: function (scope, el, attrs, ngModel) {
            //change event is fired when file is selected
            el.bind('change', function () {
                scope.$apply(function () {
                    ngModel.$setViewValue(el.val());
                    ngModel.$render();
                });
            });
        }
    }
});

app.factory("stateService", function () {
    
    var numberPerPage;
    var results;
    var currentPage;
    var pageMax;
    var pageContent;

    return {
        setNumberPerPage: function(_numberPerPage_){
            numberPerPage = parseInt(_numberPerPage_);
        },

        getNumberPerPage: function(){
            return numberPerPage;
        },

        setResults: function(_results_){
            results = _results_.slice(0);
        },

        getResults: function(){
            return results;
        },

        setCurrentPage: function(_currentPage_){
            currentPage = _currentPage_;
        },

        getCurrentPage: function(){
            return currentPage;
        },

        setPageMax: function(_pageMax_){
            pageMax = _pageMax_;
        },

        getPageMax: function(){
            return pageMax;
        },

        setPageContent: function(_pageContent_){
            pageContent = _pageContent_;
        },

        getPageContent: function(){
            return pageContent;
        }
    };
});

app.controller("PaginationController", ["$scope", "stateService", function($scope, stateService){
    
    $scope.setPageMax = function(pageMax){
        stateService.setPageMax(pageMax);
    };

    $scope.getPageMax = function(){
        return stateService.getPageMax();
    }

    $scope.isFirstPage = function(){
        return ($scope.getCurrentPage() === 1) ? true : false;
    };

    $scope.isLastPage = function(){
        return ($scope.getCurrentPage() === $scope.getPageMax()) ? true : false
    };

    $scope.getCurrentPage = function(){
        return stateService.getCurrentPage();
    }

    $scope.setCurrentPage = function(currentPage){
        stateService.setCurrentPage(currentPage);
    }

    $scope.setNumberPerPage = function(numberPerPage){
        stateService.setNumberPerPage(numberPerPage);
    }

    $scope.setResults = function(results){
        stateService.setResults(results);
    }

   

    $scope.setPageContent = function(pageContent){
        stateService.setPageContent(pageContent);
    }

    $scope.getPageContent = function(page){
        var numberPerPage = stateService.getNumberPerPage();
        var results = stateService.getResults();
        var startIndex = numberPerPage * page - numberPerPage;
        var pageContent = stateService.getPageContent();


     
        while(pageContent.length > 0) pageContent.pop();      // clear previous page content 
        
        if(results){
         
            // if there are less results than number per page, display all results in pageContent
            if ((startIndex + numberPerPage - 1) > results.length) {

               
                for(var i = startIndex; i < results.length; i++){
                    pageContent.push(results[i]);
                }
            } else {
                for(var i = startIndex; i < (numberPerPage + startIndex); i++){
                    pageContent.push(results[i]);
                };
            };
        };
    };
    
}])

app.directive("createPages", function(){
    return {
        restrict: "A",
        scope: {
            results: "=",       
            numberPerPage: "@",
            pages: "=",
            pageContent: "=",
            pageLimit: "@",                  // optional: default maximum page numbers = 15
            binding: "@"                     // optional: default pagination creation = on 'click'
        },
        controller: "PaginationController",
        link: function(scope, elem, attrs){
            
            // check if binding attribute present otherwise default to click
            if (!scope.binding) scope.binding = "click";

            
            elem.bind(scope.binding, function(){ 
                scope.pages = [];

           
                
                // gets last page number
                var pageMax = Math.ceil(scope.results.length / parseInt(scope.numberPerPage));                
               
                // sets maximum number of page numbers to 15 if option not provided
                if(!scope.pageLimit) scope.pageLimit = 15;
                pageMax = (pageMax > parseInt(scope.pageLimit)) ? parseInt(scope.pageLimit) : pageMax;


                
                // // array to store page numbers
                for(var i = 1; i <= pageMax; i++){
                    scope.pages.push(i);
                };
                
                // store pageMax in service to access when styling right-pagination
                scope.setPageMax(pageMax);

                // set number of results per page
                scope.setNumberPerPage(scope.numberPerPage);

                // set page content variable
                scope.setPageContent(scope.pageContent);

                // results = data set
                scope.setResults(scope.results);

                // intialise current page as 1
                scope.setCurrentPage(1);

                // update pageContent with page 1
                scope.getPageContent(1);

                scope.$apply();         // apply changes
            });
        }
    };
})

app.directive("pagination", function(){
    return {
        restrict: "A",
        scope: {
            page: "@"
        },
        controller: "PaginationController",
        link: function (scope, elem, attrs) {

           

            elem.bind("click", function(){

              
                // set current page (for styles)
                scope.setCurrentPage(parseInt(scope.page));

                // get content for selected page
                scope.getPageContent(parseInt(scope.page));

                scope.$apply();         // apply changes
            }); 
        }
    };
})

app.directive("paginationLeft", function(){
    return {
        restrict: "A",
        scope: {},
        controller: "PaginationController",
        link: function (scope, elem, attrs) {

           

            elem.bind("click", function(){
                var currentPage = scope.getCurrentPage();
                
                if(currentPage !== 1){
                    scope.setCurrentPage( --currentPage );          // move to previous page
                    scope.getPageContent(currentPage);              // reset content to previous page
                    scope.$apply();                                 // apply changes
                }
            });
        }
    };
})

app.directive("paginationRight", function(){
    return {
        restrict: "A",
        scope: {},
        controller: "PaginationController",
        link: function (scope, elem, attrs) {

            

            elem.bind("click", function(){
                var currentPage = scope.getCurrentPage();
                
                if(currentPage !== scope.getPageMax()){
                    scope.setCurrentPage(++currentPage);        // move to next page
                    scope.getPageContent(currentPage);          // reset contents to next page
                    scope.$apply();                             // apply changes
                }
            });
        }
    };
});