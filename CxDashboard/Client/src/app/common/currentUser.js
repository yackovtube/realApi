(function () {
    'use strict';

    angular
        .module('common.services')
        .service('currentUser', currentUser)

    function currentUser() {
        var profile = {
            isLoggedIn: false,
            userName: '',
            token:''
        }

        var setProfile = function (userName, token) {
            profile.userName = userName;
            profile.token = token;
            profile.isLoggedIn = true;
        };

        var getProfile = function () {
            return profile;
        }

        var clearProfile = function () {
            profile.isLoggedIn = false;
            profile.userName = '';
            profile.token = '';
        }

        return {
            setProfile: setProfile,
            getProfile: getProfile,
            clearProfile: clearProfile
        }
    }

})();