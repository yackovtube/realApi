(function () {
    'use strict';

    angular
        .module('cxDashboard')
        .controller('LoginController', LoginController);

    /** @ngInject */
    function LoginController(userAccount, currentUser, toastr, $state) {
        var vm = this;
        vm.userData = {
            userName: '',
            email: '',
            password: '',
            confirmPassword: ''
        };
        vm.signIn = login;

        function login() {
            vm.userData.grant_type = 'password';
            vm.userData.userName = vm.userData.email;

            userAccount.loginUser(vm.userData, function (data) {
                vm.userData.password = '';
                currentUser.setProfile(vm.userData.userName, data.access_token);
                var parms = $state.parameters;
                if ($state.previous.name !== 'login' && $state.previous.name !== '')
                    $state.go($state.previous, parms);
                else
                    $state.go('home');
            },
                function (response) {
                    vm.userData.password = '';
                    var errorMsg = '';
                    errorMsg = response.statusText + '\r\n';
                    if (response.data.exceptionMessage) {
                        errorMsg += response.data.exceptionMessage + '\r\n';
                    }
                    if (response.data.error) {
                        errorMsg += response.data.error;
                    }
                    toastr.error(errorMsg, 'Login error');
                });
        }


    }
})();
