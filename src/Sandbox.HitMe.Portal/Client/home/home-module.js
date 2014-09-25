'use strict';

angular.module('home', [
        'ngCookies', 'ngResource'
])
    .constant('homeEvents', {
        send: 'home:send',
        sent: 'home:sent'
    })
    .controller(
        'HomeController',
        [
            '$scope',
            function (
                $scope) {


            }
        ])
    .directive('homeSendForm', [
        '$timeout',
        'homeEvents',
        function (
            $timeout,
            homeEvents) {

            return {
                restrict: 'AE',
                replace: true,
                templateUrl: 'Client/home/home-send-form.cshtml',
                link: function (scope) {

                    scope.status = "Send";

                    scope.send = function () {
                        scope.status = "Sending ...";
                        scope.sendDisabled = true;

                        $timeout(function() {
                            scope.$root.$broadcast(homeEvents.send, { email: scope.email });
                        }, 1000);
                    }

                    scope.$on(homeEvents.send, function (e, success) {
                        scope.status = success ? 'Sent!' : 'Error';
                        scope.sendDisabled = false;
                    });
                }
            };
        }
    ])
    .factory('HomeSendService', [
        '$resource',
        function ($resource) {

            return $resource(
                '/api/send/', {},
                { post: { method: 'POST' } });
        }
    ]);