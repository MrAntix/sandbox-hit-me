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
        'antixMapEvents',
        function(
            $log,
            antixMapEvents) {
            return {
                restrict: 'AE',
                replace: true,
                templateUrl: 'Scripts/antix/map.cshtml',
                controller: 'AntixMapController',
                link: function(scope, element) {

                    var canvas = angular
                        .element(element.find('div')[0]);

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

                    scope.$on(antixMapEvents.addMarker, function(e, marker) {
                        $log.debug('AntixMapController.addMarker ' + JSON.stringify(marker));

                        var left = getLeft(marker.location.longitude),
                            top = getTop(marker.location.latitude) * 2.406 - 50;

                        $log.debug('AntixMapController.addMarker @ ' + left + ', ' + top);

                        var markerElement = angular
                            .element('<div class=\'antix-map-marker\'></div>')
                            .html(marker.title)
                            .attr({ id: marker.id })
                            .css({ left: left + "%", top: top + "%" });

                        canvas.append(markerElement);
                    });

                    scope.$on(antixMapEvents.removeMarker, function(e, marker) {
                        $log.debug('AntixMapController.removeMarker ' + JSON.stringify(marker));

                        angular
                            .element("#" + marker.id)
                            .remove();

                    });

                    scope.$on(antixMapEvents.markerMessage, function(e, message) {
                        $log.debug('AntixMapController.markerMessage ' + JSON.stringify(message));

                        var markerElement = angular
                            .element("#" + message.markerId);

                        var html = markerElement.html();

                        markerElement.html(message.text);
                    });
                }
            };
        }
    ])
    .controller('AntixMapController', [
        '$scope',
        'antixMapEvents',
        function(
            $scope,
            antixMapEvents) {

            $scope.$root.$broadcast(antixMapEvents.ready);
        }
    ])
    .service('AntixMapService', [
        '$log', '$rootScope',
        'antixMapEvents',
        function(
            $log, $rootScope,
            antixMapEvents) {

            var service = {

                addMarker: function(marker) {
                    $log.debug('AntixMapService.addMarker ' + JSON.stringify(marker));

                    $rootScope.$broadcast(antixMapEvents.addMarker, marker);
                },

                removeMarker: function(marker) {
                    $log.debug('AntixMapService.removeMarker ' + JSON.stringify(marker));

                    $rootScope.$broadcast(antixMapEvents.removeMarker, marker);
                },

                markerMessage: function(message) {
                    $log.debug('AntixMapService.markerMessage ' + JSON.stringify(message));

                    $rootScope.$broadcast(antixMapEvents.markerMessage, message);
                }
            }

            return service;
        }
    ]);