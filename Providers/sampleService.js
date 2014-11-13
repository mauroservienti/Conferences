(function(){

	angular.module('sample.services')
		.provider('sampleService', [function sampleServiceProvider() {
        
        	this.aSetting = '';

        	function SampleService( setting ){
        		this.setting = setting;
        	}

	    	this.$get = function sampleServiceFactory() {
	    		return new SampleService( this.aSetting );
	    	};
    }]);

}())