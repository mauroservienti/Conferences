(function () {
    'use strict';

    angular.module('my.services', []);
    angular.module('my.controllers', []);
    angular.module('my.directives', []);

    var module = angular.module('my.constants', []);
    module.constant('frontendBaseUrl', '/app/');
    module.constant('presentationBaseUrl', '/app/presentation/');

}());