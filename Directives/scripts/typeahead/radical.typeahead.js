/*
 * Radical typeahead Directive v 1.0.0.0
 */
(function () {

    function Debouncer(func, wait, $timeout) {
        var laterPromise = null;

        this.reset = function () {

            var context = this, args = arguments;
            var later = function () {
                laterPromise = null;

                return func.apply(context, args);
            };

            $timeout.cancel(laterPromise);
            laterPromise = $timeout(function () {
                laterPromise = null;

                return func.apply(context, args);
            }, wait);

            return this._laterPromise;
        };

        this.cancel = function () {
            $timeout.cancel(laterPromise);
            laterPromise = null;
        }
    };

    var typeaheadDirective = function ($parse, $compile, $log, $timeout, config) {

        $log.debug('typeahead directive injecting function:', config);

        return {
            restrict: 'EA',
            transclude: true,
            replace: true,
            templateUrl: config.templateUrl,
            scope: {
                search: '&',
                itemSelected: '&',
                items: '=',
                term: '=',
                placeholder: '=',
                tags: '=',
                tagDisplayValue: '&',
                tagClass: '&',
                showSearchButton: '='
            },
            controller: ['$scope', function ($scope) {

                $scope.items = [];
                $scope.hide = false;
                $scope.isBusy = false;
                $scope.focused = false;
                $scope.mousedOver = false;

                $log.debug('typehead -> controller $scope: ', $scope);

                this.activate = function (item) {
                    $scope.active = item;
                };

                this.activateNextItem = function () {
                    var index = $scope.items.indexOf($scope.active);

                    this.activate($scope.items[(index + 1) % $scope.items.length]);
                };

                this.activatePreviousItem = function () {
                    var index = $scope.items.indexOf($scope.active);

                    this.activate($scope.items[index === 0 ? $scope.items.length - 1 : index - 1]);
                };

                this.removeLastTag = function () {
                    if ($scope.tags.length > 0) {
                        var last = $scope.tags[$scope.tags.length - 1];
                        $scope.removeTag(last);
                    }
                };

                this.isActive = function (item) {
                    return $scope.active === item;
                };

                this.selectActive = function () {
                    this.select($scope.active);
                    $scope.active = null;
                };

                this.select = function (item) {

                    $scope.hide = true;
                    $scope.focused = true;

                    var obj = {
                        item: item,
                        refineSearch: function (term) {
                            $scope.term = term;
                            searchHandler()
                              .then(function () {
                                  $scope.focused = true;
                              });
                        }
                    };

                    $scope.itemSelected(obj);
                };

                this.selectNone = function () {
                    $scope.active = null;
                    $scope.hide = true;
                    $scope.focused = true;
                    $scope.itemSelected({ item: null });
                };

                $scope.isVisible = function () {
                    var result = !$scope.hide && ($scope.focused || $scope.mousedOver) && $scope.items.length > 0;
                    return result;
                };

                var searchHandler = function () {

                    var onSearchCompleted = function () {
                        $scope.isBusy = false;
                        $scope.hide = false;
                    };

                    $scope.isBusy = true;

                    $log.debug('debounce completed -> executing search: [term]', $scope.term);
                    var promise = $scope.search({ term: $scope.term });
                    if (promise) {
                        promise.then(onSearchCompleted);
                    } else {
                        onSearchCompleted();
                    }

                    return promise;
                };

                var explicitSearchHandler = function () {

                    var onExplicitSearchCompleted = function () {
                        $scope.focused = true;
                    };

                    var promise = searchHandler();
                    if (promise) {
                        promise.then(function () {
                            onExplicitSearchCompleted();
                        });
                    } else {
                        onExplicitSearchCompleted();
                    }
                };

                $scope.removeTag = function (tag) {

                    $log.debug('typehead -> removeTag(tag): ', tag);
                    var index = $.inArray(tag, $scope.tags);
                    if (index !== -1) {
                        $scope.tags.splice(index, 1);
                    }
                };

                var debouncer = config.createDebouncer(searchHandler, config.debouncerDelay);
                $scope.query = function () {

                    $log.debug('typehead -> query(): ', $scope.term);

                    if ($scope.term === '' || $scope.term === null || $scope.term === undefined) {
                        $scope.items = [];
                        return;
                    }

                    debouncer.reset();
                };

                $scope.explicitQuery = function () {

                    $log.debug('typehead -> explicitSearchHandler(): ', $scope.term);

                    explicitSearchHandler();
                };
            }],
            compile: function compiler($linkAttributes) {

                $log.debug('typeahead directive compile function.');

                var link = {
                    post: function (scope, element, attrs, controller) {
                        $log.debug('typeahead directive post link function, scope:', scope);

                        if (!attrs.tagClass) {
                            $log.debug('tagClass is not defined, setting default.');
                            scope.tagClass = function (obj) {
                                return config.tagClass(obj.tag);
                            };
                        }
                        $log.debug('tagClass:', scope.tagClass);

                        if (!attrs.tagDisplayValue) {
                            $log.debug('tagDisplayValue is not defined, setting default.');
                            scope.tagDisplayValue = function (obj) {
                                return config.tagDisplayValue(obj.tag);
                            };
                        }
                        $log.debug('tagDisplayValue:', scope.tagDisplayValue);

                        //if (!attrs.showSearchButton) {
                        //    $log.debug('showSearchButton is not defined, setting default.');
                        //    scope.showSearchButton = config.showSearchButton;
                        //}
                        //$log.debug('showSearchButton:', scope.showSearchButton);

                        var $input = element.find('.typeahead-input-box');
                        var $list = element.find('.typeahead-list-container');

                        $log.debug('$input:', $input);

                        $input.bind('focus', function () {
                            scope.$apply(function () { scope.focused = true; });
                        });

                        $input.bind('blur', function () {
                            scope.$apply(function () { scope.focused = false; });
                        });

                        $list.bind('mouseover', function () {
                            scope.$apply(function () { scope.mousedOver = true; });
                        });

                        $list.bind('mouseleave', function () {
                            scope.$apply(function () { scope.mousedOver = false; });
                        });

                        $input.bind('keyup', function (e) {
                            $log.debug('input -> keyup:', e.keyCode, e);

                            if (e.keyCode === 9 || e.keyCode === 13) {
                                scope.$apply(function () { controller.selectActive(); });
                            }

                            if (e.keyCode === 27) {
                                scope.$apply(function () {
                                    controller.selectNone();
                                });
                            }
                        });

                        $input.bind('keydown', function (e) {

                            $log.debug('input -> keydown:', e.keyCode, e);

                            if (e.keyCode === 8 && e.target.value === '') {
                                e.preventDefault();
                                scope.$apply(function () {
                                    scope.hide = true;
                                    controller.removeLastTag();
                                });
                            }

                            if (e.keyCode === 9 && scope.isVisible()) {
                                e.preventDefault();
                            }

                            if (e.keyCode === 13 || e.keyCode === 27) {
                                e.preventDefault();
                            }

                            if (e.keyCode === 40) {
                                e.preventDefault();
                                scope.$apply(function () { controller.activateNextItem(); });
                            }

                            if (e.keyCode === 38) {
                                e.preventDefault();
                                scope.$apply(function () { controller.activatePreviousItem(); });
                            }

                            if (e.keyCode === 13 && e.ctrlKey) {
                                e.preventDefault();
                                scope.explicitQuery();
                            }
                        });

                        scope.$watch('items', function (items) {
                            controller.activate(items.length ? items[0] : null);
                        });

                        scope.$watch('focused', function (focused) {

                            if (focused) {
                                $timeout(function () {
                                    $input.focus();
                                }, 0, false);
                            }
                        });

                        scope.$watch('isVisible()', function (visible) {
                            if (visible) {
                                var pos = $input.position();
                                var height = $input[0].offsetHeight;

                                $list.css({
                                    top: pos.top + height + 4,
                                    left: pos.left - 6,
                                    position: 'absolute',
                                    display: 'block',
                                    'z-index': '9999'
                                });
                            } else {
                                $list.css('display', 'none');
                            }
                        });
                    }
                };

                return link;
            }
        };
    };

    /*
     * typeahead-item Directive
     */
    var typeaheadItemDirective = function ($log) {

        return {
            require: '^radicalTypeahead',
            compile: function compiler($linkAttributes) {

                var link = {
                    post: function (scope, element, attrs, controller) {

                        var item = scope.$eval(attrs.radicalTypeaheadItem);

                        scope.$watch(function () {
                            return controller.isActive(item);
                        }, function (active) {
                            if (active) {
                                element.addClass('active');
                            } else {
                                element.removeClass('active');
                            }
                        });

                        element.bind('mouseenter', function (e) {
                            scope.$apply(function () { controller.activate(item); });
                        });

                        element.bind('click', function (e) {
                            scope.$apply(function () { controller.select(item); });
                        });
                    }
                };

                return link;
            }
        };
    };

    angular.module('radical.typeahead', [])
        .factory('typeaheadConfig', ['$log', '$timeout', function ($log, $timeout) {
            return {
                templateUrl: '/radical/directives/typeahead/template.html',
                debouncerDelay: 350,
                createDebouncer: function (func, wait) {
                    return new Debouncer(func, wait, $timeout);
                },
                tagDisplayValue: function (tag) {
                    return tag.name;
                },
                tagClass: function (tag) {
                    return 'label label-info';
                },
                showSearchButton: false
            };
        }])
        .directive('radicalTypeahead', ['$parse', '$compile', '$log', '$timeout', 'typeaheadConfig', typeaheadDirective])
        .directive('radicalTypeaheadItem', ['$log', typeaheadItemDirective]);

})()