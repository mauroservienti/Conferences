(function () {
    'use strict';

    var my = angular.module('my.app', [
                'ui.utils',
                'ui.state', //angular-ui -> ui-router
                'ui.compat', //angular-ui -> ui-compat
                'my.constants',
                'my.services',
                'my.controllers',
                'my.directives'
    ]);

    var config = function (presentationBaseUrl, $stateProvider, $locationProvider, $logProvider) {

        $logProvider.debugEnabled(true);

        var dashboardViews = {
            '': {
                templateUrl: presentationBaseUrl + 'dashboardView.html',
                controller: 'dashboardController'
            }
        };

        $stateProvider
            .state('root', {
                url: '',
                views: dashboardViews
            })
            .state('dashboard', {
                url: '/',
                views: dashboardViews
            })
            .state('about', {
                url: '/about',
                views: {
                    '': {
                        templateUrl: presentationBaseUrl + 'aboutView.html',
                        controller: 'aboutController'
                    }
                }
            });

        $locationProvider.html5Mode(false);

        console.debug('configuration completed.');
    };
    my.config(['presentationBaseUrl', '$stateProvider', '$locationProvider', '$logProvider', config]);

})();