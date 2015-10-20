(function(){

	angular.module('sample.controllers')
		.controller('dashboardController', ['$log', 'sampleService', function($log, sampleService){
			var vm = this;
			vm.setting = sampleService.setting;
	}]);

}())