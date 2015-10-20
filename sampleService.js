(function(){

	angular.module('sample.services')
		.provider('sampleService', [function sampleServiceProvider() {
        
        	this.aSetting = '';

        	function SampleService( setting ){
        		this.setting = setting;
        	}

	    	this.$get = ['$log', function sampleServiceFactory($log) {
	    		$log.debug('sampleServiceFactory invoked');
	    		return new SampleService( this.aSetting );
	    	}];
    }]);

}())