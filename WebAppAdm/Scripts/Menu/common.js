(function () {
    'use strict';

    // Define the common module 
    // Contains services:
    //  - common
    //  - logger
    //  - spinner
    var commonModule = angular.module('common', []);

    commonModule.factory('common', common);

    common.$inject = ['$q', '$rootScope', '$timeout', '$location', 'logger', 'config'];

    function common($q, $rootScope, $timeout, $location, logger, config) {
        var throttles = {};

        var service = {
            // common angular dependencies
            $broadcast: $broadcast,
            $q: $q,
            $timeout: $timeout,
            $location: $location,
            // generic
            activateController: activateController,
            createSearchThrottle: createSearchThrottle,
            debouncedThrottle: debouncedThrottle,
            isNumber: isNumber,
            logger: logger, // for accessibility
            textContains: textContains,
            serviceUrl: serviceUrl,
            jsonMessage: jsonMessage,
            checkRole: checkRole,            
        };

        return service;

        function activateController(promises, controllerId) {
            return $q.all(promises).then(function (eventArgs) {
                var data = { controllerId: controllerId };
                $broadcast(config.events.controllerActivateSuccess, data);
            });
        }

        function $broadcast() {
            return $rootScope.$broadcast.apply($rootScope, arguments);
        }

        function createSearchThrottle(viewmodel, list, filteredList, filter, delay) {
            // After a delay, search a viewmodel's list using 
            // a filter function, and return a filteredList.

            // custom delay or use default
            delay = +delay || 300;
            // if only vm and list parameters were passed, set others by naming convention 
            if (!filteredList) {
                // assuming list is named sessions, filteredList is filteredSessions
                filteredList = 'filtered' + list[0].toUpperCase() + list.substr(1).toLowerCase(); // string
                // filter function is named sessionFilter
                filter = list + 'Filter'; // function in string form
            }

            // create the filtering function we will call from here
            var filterFn = function () {
                // translates to ...
                // vm.filteredSessions 
                //      = vm.sessions.filter(function(item( { returns vm.sessionFilter (item) } );
                viewmodel[filteredList] = viewmodel[list].filter(function (item) {
                    return viewmodel[filter](item);
                });
            };

            return (function () {
                // Wrapped in outer IFFE so we can use closure 
                // over filterInputTimeout which references the timeout
                var filterInputTimeout;

                // return what becomes the 'applyFilter' function in the controller
                return function (searchNow) {
                    if (filterInputTimeout) {
                        $timeout.cancel(filterInputTimeout);
                        filterInputTimeout = null;
                    }
                    if (searchNow || !delay) {
                        filterFn();
                    } else {
                        filterInputTimeout = $timeout(filterFn, delay);
                    }
                };
            })();
        }

        function debouncedThrottle(key, callback, delay, immediate) {
            // Perform some action (callback) after a delay. 
            // Track the callback by key, so if the same callback 
            // is issued again, restart the delay.

            var defaultDelay = 1000;
            delay = delay || defaultDelay;
            if (throttles[key]) {
                $timeout.cancel(throttles[key]);
                throttles[key] = undefined;
            }
            if (immediate) {
                callback();
            } else {
                throttles[key] = $timeout(callback, delay);
            }
        }

        function isNumber(val) {
            // negative or positive
            return /^[-]?\d+$/.test(val);
        }

        function textContains(text, searchText) {
            return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
        }

        function serviceUrl(serviceName) {

            if (serviceName === config.apiServices.login) {
                return config.baseUrl + '/token';
            }
            return config.apiUrl + serviceName + '/';

        }

        function jsonMessage(error) {
            var message = "Unknown";

            if (error.data) {
                if (error.data.exceptionMessage) {
                    message = error.data.exceptionMessage;
                    if (error.data.stackTrace) {
                        message += "<br />" + crToBr(error.data.stackTrace);
                    }
                } else if (error.data.messageDetail) {
                    message = error.data.messageDetail;
                } else if (error.data.message) {
                    if (error.data.modelState) {
                        var items;
                        message = '';
                        for (var key in error.data.modelState) {
                            items = error.data.modelState[key];

                            if (items.length) {
                                for (var i = 0; i < items.length; i++) {
                                    message += '<br />' + items[i];
                                }
                            }
                        }
                    } else {
                        message = error.data.message;
                    }
                } else if (error.data.error_description) {
                    message = error.data.error_description;
                }
            } else if (error.statusText) {
                message = error.statusText;
            } else if (error.description) {
                message = error.description;
            } else if (error.error_description) {
                message = error.error_description;
            }

            return message;
        };

        function checkRole(userRoles, rolesToCheck) {
            if (rolesToCheck.length == 0) {
                return true;
            }
            if (userRoles.length == 0) {
                return false;
            }
            for (var i = 0; i < userRoles.length; i++) {                             
                if (rolesToCheck.indexOf(userRoles[i]) > -1) {
                    return true;
                }
            }
            return false;
        }
        
    }
})();