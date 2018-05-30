(function() {
  'use strict';

  angular
    .module('cxDashboard')
    .run(runBlock);

  /** @ngInject */
  function runBlock($log) {

    $log.debug('runBlock end');
  }

})();
