'use strict';

angular.module('home', [
        'ngCookies'
    ])
    .constant('homeEvents', {
        send: 'home:send',
        sent: 'home:sent'
    })
    .controller(
        'HomeController',
        [
            '$scope',
            function(
                $scope) {


            }
        ])
    .directive('homeSendForm', [
        'homeEvents',
        function (
            homeEvents) {

            return {
                restrict: 'AE',
                replace: true,
                templateUrl: 'Client/home/home-send-form.cshtml',
                link: function(scope) {

                    scope.status = "Send";

                    scope.send = function() {
                        scope.status = "Sending ...";

                        scope.$root.$broadcast(homeEvents.send, { email: scope.email });
                    }

                    scope.$on(homeEvents.send, function(e, success) {
                        scope.status = success ? 'Success' : 'Error';
                    });
                }
            };
        }
    ]);