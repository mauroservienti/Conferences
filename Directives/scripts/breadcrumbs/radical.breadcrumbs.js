(function () {

	var module = angular.module('radical.breadcrumbs', ['ui.router']);

    module.provider('breadcrumbsConfig', ['$locationProvider',function breadcrumbsConfigProvider($locationProvider) {
        this.templateUrl = '/radical/directives/breadcrumbs/template.html',
        this.defaultItemNameResolver = function(state, stateParams, isCurrent) {
            if(state.data && state.data.settings && state.data.settings.displayName){
                return state.data.settings.displayName;
            }

            return state.name;
        };
        this.defaultUrlFomatter = function(state, stateParams){

            var url = state.url.format(stateParams);
            var mode = $locationProvider.html5Mode();
            if(mode.enabled === false){
                url = '#' + $locationProvider.hashPrefix() + url;
            }

            return url;
        }
     
      this.$get = function breadcrumbsConfigFactory() {
         return this;
      };
    }]);

	module.directive('breadcrumbs', ['$log', 'breadcrumbsConfig','$parse', function ($log, config, $parse) {
        return {
            restrict: 'EA',
            replace: false,
            scope: {
                displayNameResolver: '&'
            },
            templateUrl: function(tElement, tAttrs) {
                return config.templateUrl;
            },
            controller: ['$scope', '$state', '$stateParams', function ($scope, $state, $stateParams) {

                var isCurrent = function(state){
                    return $state.$current.name === state.name;
                };

                var setNavigationState = function () {
                    $scope.$navigationState = {
                        currentState: $state.$current,
                        params: $stateParams,
                        getDisplayName: function (state) {

                            if ($scope.hasCustomResolver) {
                                return $scope.displayNameResolver({
                                    defaultResolver: config.defaultItemNameResolver,
                                    state: state,
                                    stateParams: $stateParams,
                                    isCurrent: isCurrent(state)
                                });
                            }
                            else {
                                return config.defaultItemNameResolver(state, $stateParams, isCurrent(state));
                            }
                        },
                        isCurrent: function (state) {
                            return isCurrent(state);
                        },
                        formatUrl: function(state){

                            var url = config.defaultUrlFomatter(state, $stateParams);

                            return url;
                        }
                    }
                };

                $scope.$on('$stateChangeSuccess', function () {
                    setNavigationState();
                });

                setNavigationState();
            }],
            link: function (scope, element, attrs, controller) {
                scope.hasCustomResolver = angular.isDefined(attrs['displayNameResolver']);
            }
        };
    }]);

})();