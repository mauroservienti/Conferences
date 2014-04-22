(function (module) {
    var utilities = function () {
        return {
            generateIdentifier: function () {
                var d = new Date().getTime();
                var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = (d + Math.random() * 16) % 16 | 0;
                    d = Math.floor(d / 16);
                    return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
                });
                return uuid;
            },

            isSuccessStatus: function (status) {
                return status >= 200 && status <= 299;
            }
        };
    };

    module.factory("utilities", utilities);

}(angular.module("my.services")));