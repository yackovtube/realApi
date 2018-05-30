(function() {
  'use strict';

  angular
    .module('cxDashboard')
    .config(config);

  /** @ngInject */
  function config($logProvider, toastrConfig) {
    // Enable log
    $logProvider.debugEnabled(true);

    // Set options third-party lib
    //toastrConfig.allowHtml = true;
    toastrConfig.timeOut = 5000;
    toastrConfig.extendedTimeOut = 1000;
    toastrConfig.positionClass = 'toast-bottom-right';
    toastrConfig.preventDuplicates = false;
    toastrConfig.progressBar = true;
  }

})();
