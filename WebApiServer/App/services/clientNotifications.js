(function (module) {
    var clientNotifications = function ($http, $log, $rootScope, tracking) {

        $log.debug('clientNotifications factory ctor.');

        var hub = null;

        var svc = {

            notifyOnEventByPattern: function (correlationId, eventTypePattern) {

                var subscription = {
                    unsubscribeOnReceive: true,
                    correlationId: correlationId,
                    eventTypePattern: eventTypePattern || '*ViewCreated*',
                };

                $log.debug('notifyOnEventByPattern:', subscription);

                this.hub.server.notifyOnEventByPattern(subscription);

                $log.debug('notifyOnEventByPattern request done.');
            },

            unsubscribeByCorrelationId: function (correlationId) {
                $log.debug('unsubscribeByCorrelationId, correlationId:', correlationId);
                this.hub.server.unsubscribeByCorrelationId(correlationId);
                $log.debug('unsubscribed.');
            },

            initialize: function () {

                $log.debug('clientNotifications.initialize');

                this.hub = $.connection.clientNotificationsHub;
                this.hub.client.onSubscribedServerEvent = function (subscription, evt) {

                    $log.debug('onSubscribedServerEvent: subscription, evt:', subscription, evt);

                    $rootScope.$broadcast('clientNotifications:onSubscribedServerEvent/Received', { subscription: subscription, serverEvent: evt });

                    var trackingInfo = tracking.getTrackingInfo(subscription.correlationId);
                    if (trackingInfo != null) {
                        $log.debug('trackingInfo found.');

                        if (trackingInfo.removeOnEvent) {
                            $log.debug('removeOnEvent is true.');
                            tracking.stopTracking(trackingInfo);
                            if (!$rootScope.$$phase) {
                                $rootScope.$apply();
                            }
                        } else {
                            $log.debug('removeOnEvent is false, skipping.');
                        }

                        $log.debug('Invoking onEventReceived.');
                        trackingInfo.onEventReceived();
                        if (!$rootScope.$$phase) {
                            $rootScope.$apply();
                        }
                        $log.debug('onEventReceived invoked.');
                    } else {
                        $log.debug('trackingInfo not found.');
                    }

                    if (subscription.unsubscribeOnReceive) {
                        $log.debug('Automatic unsubscription is enabled.');
                        svc.unsubscribeByCorrelationId(subscription.correlationId);
                        $log.debug('Automatic unsubscription done.');
                    } else {
                        $log.debug('Automatic unsubscription is disabled.');
                    }
                };
                $log.debug('onSubscribedServerEvent function attached.');

                $log.debug('hub:', this.hub);
                $log.debug('$hub:', $.connection.clientNotificationsHub);

                this.hub.logging = true;
                this.hub.connection.start()
                    .done(function () {
                        $log.debug('clientNotificationsHub.start.done:', arguments);
                        $log.debug('clientNotifications initialization completed');
                    })
                    .fail(function () {
                        $log.debug('clientNotificationsHub.start.fail:', arguments);
                    });
            }
        };

        svc.initialize();

        return svc;
    };
    clientNotifications.$inject = ['$http', '$log', '$rootScope', 'commandsTracking'];

    module.factory('clientNotifications', clientNotifications);

}(angular.module('my.services')));