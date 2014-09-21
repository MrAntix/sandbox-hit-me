'use strict';

var app = angular.module('app', [
    'ngTouch',
    'ngAnimate',
    'ui.bootstrap',
    'ui.router',
    'antix.status', 'antix.map',
    'home'
]);

app
    .controller(
        'AppController',
        [
            '$log','$scope',
            'AntixMapService', 'antixMapEvents',
            function (
                $log, $scope,
                AntixMapService, antixMapEvents) {

                var clients = $.connection.clientsHub;

                clients.client.add = function(client) {
                    $log.debug('AppController.client.add ' + JSON.stringify(location));

                    AntixMapService.addMarker({
                        id: client.id,
                        title: client.id == $.connection.hub.id ? 'You' : '',
                        location: client.location
                    });
                };
                clients.client.remove = function (client) {
                    $log.debug('AppController.client.remove ' + JSON.stringify(location));

                    AntixMapService.removeMarker({
                        id: client.id
                    });
                };

                clients.client.hit = function(hit) {
                    $log.debug('AppController.client.hit ' + JSON.stringify(hit));
                }

                $scope.$on(antixMapEvents.ready, function () {
                    $.connection.hub.start().done(function() {

                        //calibrate();

                    });
                });

                var calibrate=function() {
                    var add = function (latitude, longitude, title) {
                        AntixMapService.addMarker({
                            title: title,
                            location: { longitude: longitude, latitude: latitude }
                        });
                    }

                    add(0, 0, 'mid');

                    add(23.0506654, -82.3329682, 'havana');
                    add(51.5286416, -0.1015987, 'london');
                    add(64.132442, -21.8569031, 'reykjavik');
                    add(-33.7969235, 150.9224326, 'sydney');
                }
            }
        ])

    .config([
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise("/");

            $stateProvider
                .state('home', {
                    url: '/',
                    templateUrl: 'Client/home/home.cshtml',
                });
        }
    ])
    .config([
        '$httpProvider', function ($httpProvider) {

            $httpProvider.interceptors.push('antixStatusInterceptor');
        }
    ]);