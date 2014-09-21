'use strict';

angular.module('antix.status', [
        'ngCookies'
])

    .factory('antixStatusInterceptor', [
        '$log', '$rootScope', '$q',
        function (
            $log, $rootScope, $q) {

            return {
                request: function (config) {
                    $log.debug('antixStatusInterceptor.request ' + JSON.stringify(config.url));

                    return config;
                },
                response: function (response) {
                    $log.debug('antixStatusInterceptor.response ' + JSON.stringify(response.config.url));

                    return response;
                },
                responseError: function (rejection) {
                    $log.debug('antixStatusInterceptor.responseError ' + rejection.status);

                    if (rejection.status) {

                        switch (rejection.status) {
                            case 401:

                                break;
                            case 500:

                                break;
                        }
                    }

                    return $q.reject(rejection);
                }
            };
        }
    ]);