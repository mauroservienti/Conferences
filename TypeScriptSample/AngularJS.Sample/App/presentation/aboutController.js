(function () {

    var ctrl = function ($log, $scope) {

        $scope.name = 'Hello from the AboutController!';
        $scope.data = [{
            firstName: 'mauro',
            lastName: 'servienti'
        }, {
            firstName: 'andrea',
            lastName: 'saltarello'
        }];
    };

    angular.module('my.controllers')
        .controller('aboutController', ['$log', '$scope', ctrl]);

})();