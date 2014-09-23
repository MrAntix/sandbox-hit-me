'use strict';

angular.module('antix.map', [
])
    .constant('antixMapEvents', {
        ready: 'antix-map:ready',
        addMarker: 'antix-map:add-marker',
        removeMarker: 'antix-map:remove-marker',
        markerMessage: 'antix-map:marker-message'
    })
    .directive('antixMap', [
        '$log',
        function (
            $log) {
            return {
                restrict: 'AE',
                replace: true,
                templateUrl: 'Scripts/antix/map.cshtml',
                controller: 'AntixMapController',
                link: function(scope, element) {
                    $log.debug('antixMap.link');
                }
            };
        }
    ])
    .directive('antixMapMarker', [
        '$log',
        function ($log) {
            return {
                restrict: 'AE',
                replace: true,
                templateUrl: 'Scripts/antix/map-marker.cshtml',
                link: function (scope, element) {
                    $log.debug('antixMapMarker.link');

                    function getTop(latitude) {

                        if (latitude > 89.1) latitude = 89.1;
                        if (latitude < -89.1) latitude = -89.1;

                        var rad = latitude * (Math.PI / 180);

                        return 100 - 10
                            * (Math.log(Math.tan(Math.PI / 4 + rad / 2)) + 5);
                    }

                    function getLeft(longitude) {

                        return (longitude + 180) / 360 * 100;
                    }

                    var marker = scope.marker;

                    var left = getLeft(marker.location.longitude),
                            top = getTop(marker.location.latitude) * 2.406 - 50;

                    $log.debug('AntixMapController.addMarker @ ' + left + ', ' + top);

                    element.css({ left: left + "%", top: top + "%" });
                }
            };
        }
    ])
    .controller('AntixMapController', [
        '$log', '$scope',
        'AntixMapService',
        'antixMapEvents',
        function (
            $log, $scope,
            AntixMapService, antixMapEvents) {

            $scope.markers = AntixMapService.markers;

            $scope.$root.$broadcast(antixMapEvents.ready);
        }
    ])
    .service('AntixMapService', [
        '$log', '$rootScope',
        'antixMapEvents',
        function (
            $log, $rootScope,
            antixMapEvents) {

            var service = {
                markers: {},

                addMarker: function (marker) {
                    $log.debug('AntixMapService.addMarker ' + JSON.stringify(marker));

                    $rootScope.$apply(function () {
                        service.markers[marker.id] = marker;
                    });

                    $rootScope.$broadcast(antixMapEvents.addMarker, marker);
                },

                removeMarker: function (marker) {
                    $log.debug('AntixMapService.removeMarker ' + JSON.stringify(marker));

                    $rootScope.$apply(function () {
                        delete service.markers[marker.id];
                    });

                    $rootScope.$broadcast(antixMapEvents.removeMarker, marker);
                },

                markerMessage: function (message) {
                    $log.debug('AntixMapService.markerMessage ' + JSON.stringify(message));

                    $rootScope.$apply(function () {
                        var marker = service.markers[message.markerId];
                        if (!marker.messages) marker.messages = [];
                        marker.messages.push(message);
                    });

                    $rootScope.$broadcast(antixMapEvents.markerMessage, message);
                }
            }

            return service;
        }
    ]);