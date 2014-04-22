(function () {

    var m = angular.module('jason.client', []);

    m.provider('jasonClient', function jasonClientProvider() {
        var baseUrl = '/';
        var jasonBaseUrl = 'api/jason/';
        var postUrlSegment = 'post/';
        var getNextIdsUrlSegment = 'getNextIdentifiersChunk/';

        var correlationIdHeaderName = 'x-jason-correlation-id';
        var defaultContentType = 'application/json';

        this.setBaseUrl = function (value) {
            baseUrl = value;
            if (baseUrl) {
                var lastChar = baseUrl.substring(baseUrl.length - 1);
                if (lastChar !== '/') {
                    baseUrl = baseUrl + '/'
                }
            } else {
                baseUrl = '/';
            }
        };

        this.setCorrelationIdHeaderName = function (value) {
            correlationIdHeaderName = value;
        };

        this.setDefaultContentType = function (value) {
            defaultContentType = value;
        };

        this.$get = ['$log', '$http', function jasonClientFactory($log, $http) {

            $log.debug('[jason.client] factory.');


            var client = {
                execute: function (command, beforeExecute, afterExecute) {

                    $log.debug('[jason.client] ready to post command: ', command);

                    var correlationId = this.getIdentifier();

                    $log.debug('[jason.client] correlationId: ', correlationId);


                    if (beforeExecute) {
                        $log.debug('[jason.client] calling beforeExecute: ', command, correlationId);
                        beforeExecute(command, correlationId);
                        $log.debug('[jason.client] beforeExecute called: ', command, correlationId);
                    }

                    var headers = {};
                    headers[correlationIdHeaderName] = correlationId;
                    headers['Content-Type'] = defaultContentType;

                    var uri = baseUrl + jasonBaseUrl + postUrlSegment;
                    $log.debug('post: [uri]', uri);
                    var promise = $http({
                        method: 'POST',
                        url: uri,
                        data: JSON.stringify(command),
                        headers: headers
                    })
                    .success(function (data, status, headers, config) {
                        $log.debug('[jason.client] jason post completed, result:', data, status, headers, config);
                    })
                    .error(function (error, status, headers, config) {
                        $log.debug('[jason.client] jason post failed, result:', error, status, headers, config);
                    });

                    $log.debug('[jason.client] jason controller invoked.');
                    if (afterExecute) {
                        $log.debug('[jason.client] calling afterExecute', command, correlationId);
                        afterExecute(command, correlationId);
                        $log.debug('[jason.client] afterExecute called', command, correlationId);
                    }

                    return promise;
                },

                _ids: [],
                _checkIds: function () {
                    var self = this;
                    if (self._ids.length < 25) {
                        $log.debug('ids are less than 25.');
                        self._getNextIdsChunk()
                            .then(function (data) {
                                $log.debug('ids returned from the server.');
                                self._ids = self._ids.concat(data);
                            });
                    }
                },
                _getNextIdsChunk: function (qty) {

                    $log.debug('[jason.client] _getNextIdsChunk(qty)', qty);

                    if (!qty) {
                        qty = 50;
                    }

                    var uri = baseUrl + jasonBaseUrl + getNextIdsUrlSegment + '?qty=' + qty;
                    $log.debug('_getNextIdsChunk: [uri]', uri);

                    var promise = $http.get(uri)
                        .then(function (result) {
                            $log.debug('_getNextIdsChunk executed:', result);
                            return result.data;
                        });

                    $log.debug('server called, waiting for the promise.');

                    return promise;
                },
                getIdentifier: function () {

                    var self = this;

                    if (self._ids.length == 0) {
                        throw "Not supported exception, still getting ids from the server";
                    }

                    var uuid = self._ids.pop();
                    self._checkIds();

                    return uuid;
                },
            };

            return client;
        }];
    });

    m.run(['$log', 'jasonClient', function ($log, jason) {
        $log.debug('[jason.client] - run.');
        jason._checkIds();
        $log.debug('[jason.client] - run completed.');
    }]);

}());