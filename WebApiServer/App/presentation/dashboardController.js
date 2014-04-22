(function (module) {

    var ctrl = function ($log, $scope, $rootScope, backend, $state) {

        $scope.cueBanner = 'Search...';

        $scope.firstName = '';
        $scope.lastName = '';
        $scope.fullName = function () {
            return $scope.firstName + ' ' + $scope.lastName;
        };

        $scope.companyName = '';

        $scope.createNewPerson = function () {

            backend.executeCommand({
                command: {
                    '$type': 'WebApi.Commands.CreateNewPersonCommand, WebApi.Commands',
                    firstName: $scope.firstName,
                    lastName: $scope.lastName
                },
                commandTracking: {
                    enabled: true,
                    removeOnEvent: true,
                    removeOnFailure: false,
                    eventTypePattern: '*PersonCreated*',
                    onServerProcessingError: function (subscription, evt) {
                        $log.debug('onServerProcessingError: [subscription, evt]', subscription, evt);
                    },
                    onEventReceived: function (subscription, evt) {
                        $log.debug('CreateNewPersonCommand completed.', subscription, evt);
                    },
                    description: 'Adding new person: ' + $scope.firstName  + ' ' + $scope.lastName
                }
            });
        };

        $scope.createNewCompany = function () {

            backend.executeCommand({
                command: {
                    '$type': 'WebApi.Commands.CreateNewCompanyCommand, WebApi.Commands',
                    companyName: $scope.companyName
                },
                commandTracking: {
                    enabled: true,
                    removeOnEvent: true,
                    removeOnFailure: false,
                    eventTypePattern: '*CompanyCreated*',
                    onServerProcessingError: function (subscription, evt) {
                        $log.debug('onServerProcessingError: [subscription, evt]', subscription, evt);
                    },
                    onEventReceived: function (subscription, evt) {
                        $log.debug('CreateNewCompanyCommand completed.', subscription, evt);
                    },
                    description: 'Adding new company: ' + $scope.companyName
                }
            });
        };

        $scope.executeSearch = function (term) {

            if (term && term != '') {

                return backend.search(term)
                    .then(function (data) {
                        $log.debug('results from server: ', data);

                        var commands = [{
                            displayName: 'Create new contact named "' + term + '"',
                            executeCommand: function () { }
                        }, {
                            displayName: 'Search all contacts for "' + term + '"',
                            executeCommand: function () {
                                var params = {
                                    q: term
                                };
                                $log.log('going to search for all: [term, params]', term, params)
                            }
                        }];

                        $scope.results = data.suggestions
                                                    .concat(data.results
                                                        .concat(commands));

                        $log.debug($scope.results);

                        $scope.serverSuggestions = data.suggestions;
                        $scope.serverResults = data.results;
                        $scope.clientCommands = commands;
                        $scope.totalResults = data.totalResults;
                    });
            }
            else {
                $log.debug('tentative to execute a search without any term, skipping.');
            }
        };

        $scope.selectItem = function (item, refineSearch) {

            $log.debug('select invoked on: ', item, refineSearch);
            $log.debug('item is suggestion: ', typeof (item) == typeof (''));
            $log.debug('item is command: ', item.executeCommand != undefined);

            if (typeof (item) == typeof ('')) {
                $log.debug('select invoked on a suggestion.');
                refineSearch(item);
            }
            else if (item.executeCommand != undefined) {
                $log.debug('select invoked on a command.');
                item.executeCommand();
            }
            else if (item.clrType != undefined) {
                $log.debug('select invoked on contact item.');
                window.location.hash = '/' + item.id;
            }
        };
    };

    ctrl.$inject = ['$log', '$scope', '$rootScope', 'backendService', '$state'];

    module.controller('dashboardController', ctrl);

}(angular.module('my.controllers')));