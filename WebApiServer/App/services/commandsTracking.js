(function (module) {
    var commandsTracking = function ($rootScope, $log) {

        $rootScope.activeCommands = [];

        return {
            track: function (trackingInfo) {
                $rootScope.activeCommands.push(trackingInfo);
                $rootScope.$broadcast('commandsTracking:trackingStarted', trackingInfo);
            },

            stopTracking: function (trackingInfo) {
                $log.debug('stopTracking:', trackingInfo, $rootScope.activeCommands);

                var index = this.indexOfTrackingInfo(trackingInfo.correlationId);
                if (index != -1) {
                    var current = $rootScope.activeCommands[index];
                    $rootScope.activeCommands.splice(index, 1);
                    $log.debug('element removed from activeCommands:', $rootScope.activeCommands);
                    $rootScope.$broadcast('commandsTracking:trackingStopped', current);
                } else {
                    $log.debug('element not found in activeCommands:', $rootScope.activeCommands);
                }
            },

            indexOfTrackingInfo: function (correlationId) {
                $log.debug('indexOfTrackingInfo:', correlationId);

                for (var i = 0; i < $rootScope.activeCommands.length; i++) {
                    var current = $rootScope.activeCommands[i];
                    if (current.correlationId == correlationId) {
                        $log.debug('element found, index:', i);
                        return i;
                    }
                }

                $log.debug('element not found.');

                return -1;
            },

            getTrackingInfo: function (correlationId) {
                $log.debug('getTrackingInfo:', correlationId);

                var index = this.indexOfTrackingInfo(correlationId);
                if (index != -1) {
                    $log.debug('element found in activeCommands, index:', index);
                    return $rootScope.activeCommands[index];
                } else {
                    $log.debug('element not found in activeCommands');
                    return null;
                }
            }
        };
    };
    commandsTracking.$inject = ['$rootScope', '$log'];

    module.factory("commandsTracking", commandsTracking);

}(angular.module("my.services")));