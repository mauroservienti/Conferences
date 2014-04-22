(function () {

    var ctrl = function ($log, $scope) {

        $scope.name = 'Hello from the DashboardController!';
    };

    angular.module('my.controllers')
        .controller('dashboardController', ['$log', '$scope', ctrl]);

})();