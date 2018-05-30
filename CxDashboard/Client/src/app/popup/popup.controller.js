(function () {
  'use strict';

  angular.module('cxDashboard').controller('PopupController', PopupController);


  function PopupController($uibModalInstance, items, resourceUtil, $window, $q) {
    var controllerPath = 'api/reports/:action/:id';
    var vm = this;
    vm.items = items;
    vm.close = close;
    vm.openLink = openLink;

    function close() {
      $uibModalInstance.close();
    }

    function openLink(item) {
      getTempQueryLink(item).then(function () {
        $window.open(item.link);
      })
    }

    function getTempQueryLink(item) {
      var defferd = $q.defer();
      if (item.wiql) {
        resourceUtil.save(controllerPath, { action: 'CreateTempQuery', queryName: 'Old version ' + item.dataName }, { 'RequestBody': item.wiql }).then(function (data) {
          item.link = data.responseObject
          defferd.resolve(data.responseObject);
        })
      }
      else
        defferd.resolve();
      return defferd.promise;
    }
  }
})();