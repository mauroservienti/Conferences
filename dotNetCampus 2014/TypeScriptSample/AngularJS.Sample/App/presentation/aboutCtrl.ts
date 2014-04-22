/// <reference path='../_all.ts' />

module myControllers {

    export class aboutCtrl {

        public static $inject = [
            '$scope',
        ];

        constructor($scope: myControllers.IAboutScope) {
            $scope.name = 'Hello from the AboutController!';
            $scope.data = [
                new myModels.Person('mauro', 'servienti'),
                new myModels.Person('andrea', 'saltarello')];
        }

    }

    angular.module('my.controllers')
        .controller('aboutCtrl', myControllers.aboutCtrl);
}