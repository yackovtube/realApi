(function () {
  'use strict';

  angular
    .module('cxDashboard')
    .controller('MainController', MainController);

  /** @ngInject */
  function MainController($timeout, toastr, $uibModal, currentUser, resourceUtil) {
    var controllerPath = 'api/home/:action/:id';
    var vm = this;
    vm.showAppDetails = showAppPopupdetails;
    vm.showEngineDetails = showEngineDetails;
    vm.userData = currentUser.getProfile();
    vm.logOut = logout;
    vm.getVersionString = getVersionString;

    activate();

    function activate() {
      toastr.info('Loading main CxDashboard', 'server information');
      resourceUtil.get(controllerPath, { action: 'GetApplicationGrade' }).then(function (response) {
        vm.timeline = response.timeLine;
        vm.appGrade = response.currentApplicationKPIsGrade;
        vm.oldVersionList = response.oldVersionApplicationList;
        vm.engineGrade = response.currentEngineKPIsGrade;
        vm.oldVersionEngineList = response.oldVersionEngineList;
        vm.smokeGrades = response.smokeGrades;
        vm.engineCodeCoverage = response.engineCodeCoverage;
        vm.applicationCodeCoverage = response.applicationCodeCoverage;
      });
    }

    function getVersionString(data) {
      return data.replace(/\./g, '');
    }

    function showAppPopupdetails(item) {
      $uibModal.open(
        {
          animation: true,
          ariaLabelledBy: 'modal-title-bottom',
          ariaDescribedBy: 'modal-body-bottom',
          templateUrl: 'app/popup/applicationPopup.html',
          controller: 'PopupController',
          size: 'sm',
          controllerAs: 'popup',
          resolve: {
            items: function () {
              return item;
            }
          }
        }
      )
    }
    
    function showEngineDetails(item) {
      $uibModal.open(
        {
          animation: true,
          ariaLabelledBy: 'modal-title-bottom',
          ariaDescribedBy: 'modal-body-bottom',
          templateUrl: 'app/popup/enginePopup.html',
          controller: 'PopupController',
          size: 'sm',
          controllerAs: 'popup',
          resolve: {
            items: function () {
              return item;
            }
          }
        }
      )
    }

    function logout() {
      currentUser.clearProfile();
    }

  }
})();
