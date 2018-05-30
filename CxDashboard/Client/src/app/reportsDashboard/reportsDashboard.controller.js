(function () {
  'use strict';

  angular
    .module('cxDashboard')
    .controller('ReportsDashboardController', ReportsDashboardController);

  /** @ngInject */
  function ReportsDashboardController(toastr, resourceUtil) {
    var controllerPath = 'api/reports/:action/:id';
    var vm = this;

    activate();

    function activate() {
      toastr.info('Loading reports dashboard', 'server information');
      resourceUtil.get(controllerPath, { action: 'GetCategoryReportsList' }).then(function (data) {
        vm.categoryList = data.responseObject;
      });
    }

  }
})();
