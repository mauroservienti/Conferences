(function (module) {

    var aboutController = function ($log, $scope, backend, $rootScope) {

        $scope.name = 'The following is the history (first 30 items) of what has happened:';
        $scope.self = '';
        $scope.next = '';
        $scope.prev = '';

        backend.history()
            .then(function (data) {
                $rootScope.safeApply($scope, function () {
                    $scope.history = data.results;
                    $scope.self = data.self;
                    $scope.next = data.next;
                    $scope.prev = data.prev;
                });
            });
    };

    aboutController.$inject = ['$log', '$scope', 'backendService', '$rootScope'];

    module.controller('aboutController', aboutController);

}(angular.module('my.controllers')));