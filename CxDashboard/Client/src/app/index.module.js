(function () {
  'use strict';

  angular
    .module('cxDashboard', ['ngAnimate', 'ngCookies', 'ngTouch', 'ngSanitize', 'ngMessages', 'ngAria', 'common.services', 'ui.router', 'ui.bootstrap', 'toastr', 'flow'])
    .run(function ($rootScope, $state, currentUser) {
      var redirectEvent = $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState) {
        $state.previous = fromState;
        
        if (toState.name !== 'login')
          $state.parameters = toParams;

        if (toState.name === 'management') {
          $state.previous = toState;
          if (!currentUser.getProfile().isLoggedIn) {
            event.preventDefault();
            $state.go('login');
          }
        }
      })
      $rootScope.$on('$destroy', redirectEvent);
    });

})();

