(function(){

	angular.module('sample.controllers')
		.controller('dashboardController', ['$log', function($log){
			var vm = this;

			var _sampleValue = '';
			var _sampleValue_original = _sampleValue;
			Object.defineProperty( vm, 'sampleValue', {
				get: function(){
					return _sampleValue;
				},
				set: function(value){
					if(value !== _sampleValue){
						_sampleValue = value;
					}
				}
			} );

			Object.defineProperty( vm, 'isChanged', {
				get: function(){
					return _sampleValue !== _sampleValue_original;
				}
			} );
	}]);

}())