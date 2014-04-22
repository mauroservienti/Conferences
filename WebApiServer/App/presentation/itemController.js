(function (module) {

    var itemController = function ($log, $scope, $state, item) {

        $log.debug('item controller', item);

        $scope.item = {
            collection: $state.params.collection,
            id: $state.params.id,
            data: item
        };
    };

    itemController.$inject = ['$log', '$scope', '$state', 'item'];

    module.controller('itemController', itemController);

}(angular.module('my.controllers')));