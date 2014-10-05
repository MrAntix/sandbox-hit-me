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
            '$log', '$scope', '$timeout',
            'AntixStatusEvents',
            'AntixMapService', 'antixMapEvents',
            'homeEvents', 'HomeSendService',
            function(
                $log, $scope, $timeout,
                AntixStatusEvents,
                AntixMapService, antixMapEvents,
                homeEvents, HomeSendService) {

                var clients = $.connection.clientsHub;

                clients.client.add = function(client) {
                    $log.debug('AppController.client.add ' + JSON.stringify(client));

                    var you = client.id == $.connection.hub.id;

                    AntixMapService.addMarker({
                        id: client.id,
                        active: you,
                        title: you ? 'You' : '',
                        'class': you ? 'you' : '',
                        location: client.location
                    });
                };

                clients.client.remove = function(client) {
                    $log.debug('AppController.client.remove ' + JSON.stringify(client));

                    AntixMapService.removeMarker({
                        id: client.id
                    });
                };

                clients.client.hit = function(hit) {
                    $log.debug('AppController.client.hit ' + JSON.stringify(hit));

                    AntixMapService.addMarker({
                        id: hit.toClientId + '_hit',
                        active: true,
                        title: hit.fromIP.address,
                        'class': 'sniper',
                        location: hit.fromIP.location
                    });

                    $timeout(function() {
                        AntixMapService.removeMarker({
                            id: hit.toClientId + '_hit'
                        });
                    }, 20000);
                }

                clients.client.status = function (status) {
                    $log.debug('AppController.client.status ' + JSON.stringify(status));

                    $scope.$root.$broadcast(AntixStatusEvents.Status, status);
                };

                $scope.$on(antixMapEvents.ready, function() {
                    $.connection.hub.start().done(function() {

                        // calibrate();

                    });
                });

                $scope.$on(homeEvents.send, function(e, message) {
                    $log.debug('AppController.server.send ' + JSON.stringify(message));

                    HomeSendService.post({
                            email: message.email,
                            clientConnectionId: $.connection.hub.id
                        })
                        .$promise
                        .then(function() {
                            $scope.$root.$broadcast(homeEvents.sent, true);
                        })
                        .catch(function() {
                            $scope.$root.$broadcast(homeEvents.sent, false);
                        });
                });

                var calibrate = function() {
                    var add = function(latitude, longitude, title) {
                        AntixMapService.addMarker({
                            id: title,
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
        function($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise("/");

            $stateProvider
                .state('home', {
                    url: '/',
                    templateUrl: 'Client/home/home.cshtml',
                });
        }
    ])
    .config([
        '$tooltipProvider', function($tooltipProvider) {
            $tooltipProvider.options({
                placement: 'top'
            });
        }
    ]);