(function(){

	angular.module('sample.controllers')
		.controller('productController', ['$log', function($log){
			$log.debug('productController.ctor');
	}]);

}())