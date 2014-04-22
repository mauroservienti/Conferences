/// <reference path='../_all.ts' />

module myControllers {

    export class dashboardCtrl {

        public static $inject = [
            '$scope',
        ];

        constructor($scope: any) {
            $scope.name = 'Hello from the DashboardController!';
        }

    }

    angular.module('my.controllers')
        .controller('dashboardCtrl', myControllers.dashboardCtrl );
}