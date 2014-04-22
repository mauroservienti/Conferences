(function (module) {
    var backend = function ($log, $http, jason, clientNotifications, tracking) {

        var svc = {

            history: function () {
                $log.debug('history');

                var uri = '/api/history';
                $log.debug('history: [uri]', uri);

                var promise = $http.get(uri)
                    .success(function (data, status, headers, config) {
                        $log.debug('history executed:', data, status, headers, config);
                    })
                    .then(function (result) {
                        return result.data;
                    });

                $log.debug('server called, waiting for the promise.');

                return promise;
            },

            search: function (query, pageIndex, pageSize) {

                $log.debug('search: [query, pageIndex, pageSize]', query, pageIndex, pageSize);

                var uri = '/api/search?q=' + query;
                if (pageIndex) {
                    uri += '&p=' + pageIndex.toString();
                }
                if (pageSize) {
                    uri += '&s=' + pageSize.toString();
                }

                $log.debug('search: [uri]', uri);

                var promise = $http.get(uri)
                    .success(function (data, status, headers, config) {
                        $log.debug('search executed:', data, status, headers, config);
                    })
                    .then(function (result) {
                        return result.data;
                    });

                $log.debug('server called, waiting for the promise.');

                return promise;
            },

            getItem: function (id, collection) {
                $log.debug('getItem: [id, collection]', id, collection);

                var uri = '/api/' + collection + '/' + id;
                $log.debug('getItem: [uri]', uri);

                var promise = $http.get(uri)
                    .success(function (data, status, headers, config) {
                        $log.debug('getItem executed:', data, status, headers, config);
                    })
                    .then(function (result) {
                        return result.data;
                    });

                $log.debug('server called, waiting for the promise.');

                return promise;
            },

            executeCommand: function (context) {

                var before = context.beforeExecute || function (command, correlationId) { };

                context.beforeExecute = function (command, correlationId) {
                    if (context.commandTracking.enabled) {
                        context.commandTracking.removeOnFailure = context.commandTracking.removeOnFailure || true;
                        context.commandTracking.removeOnEvent = context.commandTracking.removeOnEvent || true;

                        $log.debug('adding to commands client notifications via SignalR for:', correlationId);
                        clientNotifications.notifyOnEventByPattern(correlationId, context.commandTracking.eventTypePattern);

                        $log.debug('adding to commands tracking for:', correlationId);
                        tracking.track({
                            correlationId: correlationId,
                            eventTypePattern: context.commandTracking.eventTypePattern || '',
                            onEventReceived: context.commandTracking.onEventReceived || function () { },
                            removeOnFailure: context.commandTracking.removeOnFailure,
                            removeOnEvent: context.commandTracking.removeOnEvent,
                            description: context.commandTracking.description || 'Executing commdand: [' + correlationId + '].'
                        });
                    }

                    before(command, correlationId);
                };
                context.afterExecute = context.afterExecute || function () { };
                context.executionFailed = context.executionFailed || function () { };
                context.commandTracking = context.commandTracking || { enabled: false };
                
                var promise = jason.execute(
                    context.command,
                    context.beforeExecute,
                    context.afterExecute);

                promise.then(function (data) {
                    //success
                }, function (error) {
                    $log.error('backendServices -> processing error:', error);
                });

                return promise;
            }
        };

        return svc;
    };
    backend.$inject = ['$log', '$http', 'jasonClient', 'clientNotifications', 'commandsTracking'];

    module.factory('backendService', backend);

}(angular.module('my.services')));