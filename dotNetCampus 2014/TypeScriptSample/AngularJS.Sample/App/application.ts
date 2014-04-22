/// <reference path='_all.ts' />

module myApp {
    'use strict';

    angular.module('my.services', []);
    angular.module('my.controllers', []);
    angular.module('my.directives', []);

    var myConstants = angular.module('my.constants', []);
    myConstants.constant('frontendBaseUrl', '/app/');
    myConstants.constant('presentationBaseUrl', '/app/presentation/');

    var application = angular.module('my.app', [
        'ui.router', //angular-ui -> ui-router
        'ngAnimate',
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
                controller: 'dashboardCtrl'
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
                        controller: 'aboutCtrl'
                    }
                }
            });

        $locationProvider.html5Mode(false);

        console.debug('configuration completed.');
    };
    application.config(['presentationBaseUrl', '$stateProvider', '$locationProvider', '$logProvider', config]);

    var run = function ($log, $rootScope, $state, $stateParams) {
        $rootScope.$state = $state;
        $rootScope.$log = $log;
        $rootScope.$stateParams = $stateParams;
        $rootScope.safeApply = function ($scope, fn) {
            var phase = $scope.$root.$$phase;
            if (phase === '$apply' || phase === '$digest') {
                if (fn) {
                    $scope.$eval(fn);
                }
            } else {
                if (fn) {
                    $scope.$apply(fn);
                } else {
                    $scope.$apply();
                }
            }
        };
    };
    application.run(['$log', '$rootScope', '$state', '$stateParams', run]);
}