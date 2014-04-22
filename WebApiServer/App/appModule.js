(function () {
    'use strict';

    var my = angular.module('my.app', [
                'ngRoute',
                'ui.router',
                //'ui.bootstrap',
                'ngAnimate',
                'ajoslin.promise-tracker',
                'cgBusy',
                'jason.client',
                'radical.typeahead',
                'radical.itemTemplate',
                'my.constants',
                'my.services',
                'my.controllers',
                'my.directives'
    ]);

    var config = function (presentationBaseUrl, $stateProvider, $locationProvider, $logProvider) {

        $logProvider.debugEnabled(true);

        var defaultHeaderView = {
            templateUrl: presentationBaseUrl + 'headerView.html',
            controller: 'headerController'
        }

        var dashboardViews = {
            '': {
                templateUrl: presentationBaseUrl + 'dashboardView.html',
                controller: 'dashboardController'
            },
            'header-view': defaultHeaderView
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
                    },
                    'header-view': defaultHeaderView
                }
            })
            .state('viewById', {
                url: '/{collection}/{id}',
                views: {
                    '': {
                        templateUrl: presentationBaseUrl + 'itemView.html',
                        controller: 'itemController'
                    },
                    'header-view': defaultHeaderView
                },
                resolve: {
                    'item': ['backendService', '$stateParams', function (backend, state) {
                        var promise = backend.getItem(state.id, state.collection);
                        
                        return promise;
                    }]
                },
            });

        $locationProvider.html5Mode(false);

        console.debug('configuration completed.');
    };
    my.config(['presentationBaseUrl', '$stateProvider', '$locationProvider', '$logProvider', config]);

    var run = function ($log, $rootScope, $state, $stateParams, tracking) {
        $rootScope.$state = $state;
        $rootScope.$log = $log;
        $rootScope.$stateParams = $stateParams;
        $rootScope.safeApply = function ($scope, fn) {
            var phase = $scope.$root.$$phase;
            if (phase == '$apply' || phase == '$digest') {
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

    my.run(['$log', '$rootScope', '$state', '$stateParams', 'commandsTracking', run]);

    my.filter('typeName', function () {
        return function (item) {

            var commaIdx = item.indexOf(',');
            var segment = item.substring(0, commaIdx);
            var lastDotIdx = segment.lastIndexOf('.');
            segment = segment.substring(lastDotIdx + 1);

            return segment;
        };
    });

}());