(function () {
    angular
        .module("common.services")
        .factory("userAccount", userAccount)

    function userAccount($resource, appSettings) {
        return $resource(appSettings.serverPath + "Token", null,
            {
                loginUser: {
                    method: 'POST',
                    headers: {
                        'content-Type': 'application/x-www-form-urlencoded'
                    },
                    transformRequest: function (data, headersGetter) {
                        var str = [];
                        for (var d in data) {
                            str.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]));
                        }
                        return str.join('&');
                    }
                }
            });
    }

}());