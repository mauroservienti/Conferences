(function () {
    'use strict';

    angular.module('sample.controllers', []);

    var sampleApp = angular.module('sample.App', [
                'ngRoute',
                'ngAnimate',
                'ui.router',
                'ui.bootstrap',
                'cgBusy',
                'sample.controllers',
                'radical.breadcrumbs',
                'radical.itemTemplate',
                'radical.typeahead'
    ]);

    sampleApp.config(['$stateProvider', '$locationProvider', '$logProvider', 'breadcrumbsConfigProvider',
            function ( $stateProvider, $locationProvider, $logProvider, breadcrumbsConfigProvider) {

                $locationProvider.html5Mode(false);
                breadcrumbsConfigProvider.templateUrl =  '/templates/breadcrumbs/template.html';

                var original = breadcrumbsConfigProvider.defaultItemNameResolver;
                breadcrumbsConfigProvider.defaultItemNameResolver = function(state, stateParams, isCurrent) {
                    
                    if(state.name === 'products.byId'){
                        return '"' + stateParams.id + '" ' + state.data.settings.displayName;   
                    }

                    return original(state, stateParams, isCurrent);
                }; 

                $logProvider.debugEnabled(true);
                
                var rootViews = {
                    '': {
                        templateUrl: '/presentation/dashboardView.html',
                        controller: 'dashboardController as dashboard'
                    }
                };

                $stateProvider
                    .state('root', {
                        url: '',
                        views: rootViews,
                        data:{
                            settings:{
                                displayName: 'Dashboard'
                            }
                        }
                    })
                    .state('dashboard', {
                        url: '/',
                        views: rootViews,
                        data:{
                            settings:{
                                displayName: 'Dashboard'
                            }
                        }
                    })
                    .state('about', {
                        url: '/about',
                        views: {
                            '': {
                                templateUrl: '/presentation/aboutView.html',
                                controller: 'aboutController as about'
                            }
                        },
                        data:{
                            settings:{
                                displayName: 'About screen'
                            }
                        }
                    })
                    .state('products', {
                        url: '/products',
                        views: {
                            '': {
                                templateUrl: '/presentation/productsView.html',
                                controller: 'productsController as products'
                            }
                        },
                        data:{
                            settings:{
                                displayName: 'Products'
                            }
                        }
                    })
                    .state('products.byId', {
                        url: '/{id}',
                        views: {
                            '': {
                                templateUrl: '/presentation/productView.html',
                                controller: 'productController as product'
                            }
                        },
                        data:{
                            settings:{
                                displayName: 'Product'
                            }
                        }
                    });

                console.debug('configuration completed.');
            }]);

    sampleApp.run(['$log', '$state', '$rootScope', '$stateParams','typeaheadConfig',
        function ( $log, $state, $rootScope, $stateParams, typeaheadConfig) {

            typeaheadConfig.templateUrl = '/templates/typeahead/template.html';

            $rootScope.$state = $state;
            $rootScope.$log = $log;
            $rootScope.$stateParams = $stateParams;

        }]);

}());