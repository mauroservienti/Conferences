(function () {
    'use strict';

    angular.module('sample.controllers', []);
    angular.module('sample.services', []);

    var sampleApp = angular.module('sample.App', [
                'sample.controllers',
                'sample.services'
    ]);

    sampleApp.config(['$locationProvider', '$logProvider', 'sampleServiceProvider',
            function ( $locationProvider, $logProvider, sampleServiceProvider) {

                $logProvider.debugEnabled(true);
                $locationProvider.html5Mode(false);
                
                sampleServiceProvider.aSetting = 'this is my setting';

                console.debug('configuration completed.');
            }]);

}());