(function () {
    'use strict';
    angular
        .module("common.services")
        .service("resourceUtil", resourceUtil);

    function resourceUtil($resource, appSettings, $q, currentUser, toastr) {
        var authHeader = function (newHeaders) {
            var defaultHeader = {
                'Authorization': 'bearer ' + currentUser.getProfile().token
            };
            if (newHeaders) {
                for (var prop in defaultHeader) {
                    newHeaders[prop] = defaultHeader[prop];

                }
                return newHeaders;
            }
            return defaultHeader;
        };

        var serverResource = function (url) {
            return $resource(appSettings.serverPath + url, null, {
                query: {
                    method: 'GET',
                    headers: authHeader(),
                    isArray: true
                },
                get: {
                    method: 'GET',
                    headers: authHeader()
                },
                save: {
                    method: 'POST',
                    headers: authHeader()
                },
                update: {
                    method: 'PUT',
                    headers: authHeader()
                },
                delete: {
                    method: 'DELETE',
                    headers: authHeader()
                }
            });
        }

        var serverErrorMessage = function (errorMsg) {
            toastr.error(errorMsg, 'Server error');
        }

        return {
            query: function (url, listParams) {
                var defferd = $q.defer();
                var server = serverResource(url);

                server.query(listParams).$promise.then(function (data) {
                    defferd.resolve(data);
                }, function (error) {
                    var msg = error.statusText + '\r\n';
                    if (error.data) {
                        if (error.data.exceptionMessage) {
                            msg += error.data.exceptionMessage;
                        }
                    }
                    serverErrorMessage(msg);
                    defferd.reject(error);
                });

                return defferd.promise;
            },
            get: function (url, listParams) {
                var defferd = $q.defer();
                var server = serverResource(url);

                server.get(listParams).$promise.then(function (data) {
                    defferd.resolve(data);
                }, function (error) {
                    var msg = error.statusText + '\r\n';
                    if (error.data) {
                        if (error.data.exceptionMessage) {
                            msg += error.data.exceptionMessage;
                        }
                    }
                    serverErrorMessage(msg);
                    defferd.reject(error);
                });

                return defferd.promise;
            },
            save: function (url, listParams, object) {
                var defferd = $q.defer();
                var server = serverResource(url);

                server.save(listParams, object).$promise.then(function (data) {
                    defferd.resolve(data);
                }, function (error) {
                    var msg = error.statusText + '\r\n';
                    if (error.data) {
                        if (error.data.exceptionMessage) {
                            msg += error.data.exceptionMessage;
                        }
                    }
                    serverErrorMessage(msg);
                    defferd.reject(error);
                });

                return defferd.promise;
            },
            update: function (url, listParams, object) {
                var defferd = $q.defer();
                var server = serverResource(url);

                server.update(listParams, object).$promise.then(function (data) {
                    defferd.resolve(data);
                }, function (error) {
                    var msg = error.statusText + '\r\n';

                    if (error.data) {
                        if (error.data.modelState) {
                            for (var key in error.data.modelState) {
                                msg += error.data.modelState[key] + '\r\n';
                            }
                        }
                        if (error.data.exceptionMessage) {
                            msg += error.data.exceptionMessage[key];
                        }
                    }
                    serverErrorMessage(msg);
                    defferd.reject(error);
                });

                return defferd.promise;
            },
            delete: function (url, listParams) {
                var defferd = $q.defer();
                var server = serverResource(url);

                server.delete(listParams).$promise.then(function (data) {
                    defferd.resolve(data);
                }, function (error) {
                    var msg = error.statusText + '\r\n';
                    
                    if (error.data) {
                        if (error.data.exceptionMessage) {
                            msg += error.data.exceptionMessage;
                        }
                    }
                    serverErrorMessage(msg);
                    defferd.reject(error);
                });

                return defferd.promise;
            }
        }


    }
}());