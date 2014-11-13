(function(){

	angular.module('sample.controllers')
		.controller('productsController', ['$log', '$timeout', function($log, $timeout){
			$log.debug('productsController.ctor');

			var products = [{
					name: 'Apples',
					dataType: 'a'
				},{
      				name: 'Oranges',
					dataType: 'b'
		      	},{
		        	name: 'Grape',
					dataType: 'b'
		      	},{
		        	name: 'Peaches',
					dataType: 'a'
		      	}];


			var vm = this;
		    vm.searchTerm = '';
      		vm.searchResults = [];
		    vm.selected = null;

		    vm.search = function (term) {
		        if (term && term !== '') {
		          //simulate a long http call to the backend
		          return $timeout( function(){
		            var lcTerm = term.toLowerCase();
		            var results =[];
		            angular.forEach( products, function(obj, index){

		            	if(obj.name.toLowerCase().indexOf(lcTerm) !== -1){
		            		results.push(obj);
		            	}

		            } );
		            
		            return results;
		                  
		          }, 500).then(function(results){
		            $log.debug('results:', results);
		            vm.searchResults = results;
		          });
		                    
		        } else {
		          return $timeout(function () {
		            $log.debug('tentative to execute a search without any term, skipping.');
		          }, 10);
		        }
		      };

		      vm.select = function (t) {
		        $log.debug('select invoked on: ', t);
		        vm.selected = t;
		        vm.searchTerm = '';
		      };

	}]);

}())