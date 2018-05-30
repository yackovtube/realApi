(function () {
    'use strict';

    angular
        .module('cxDashboard')
        .controller('ReportController', ReportController);

    /** @ngInject */
    function ReportController(toastr, $state, $stateParams, currentUser, resourceUtil, $window) {
        var controllerPath = 'api/reports/:action/:id';
        var vm = this;
        vm.reportName = $stateParams.name;
        vm.categoryName = $stateParams.categoryName;
        vm.userData = currentUser.getProfile();
        vm.editChart = editChart;
        vm.deleteChart = deleteChart;
        vm.redirectToTfs = redirectToTfs;

        activate();

        function activate() {
            //toastr.info('Loading ' + $stateParams.name + ' report', 'server information');
            resourceUtil.get(controllerPath, { action: 'GetReportsList', reportName: $stateParams.name, categoryName: $stateParams.categoryName }).then(function (data) {
                if (data.responseObject.length != 0) {
                    vm.chartList = data.responseObject;
                } else {
                    toastr.info('No data', 'server information');
                }
            });
        }

        function editChart(chart) {
            resourceUtil.get(controllerPath, { action: 'GetChartData', categoryName: $stateParams.categoryName, reportName: $stateParams.name, chartName: chart.name }).then(function (data) {
                $state.chartToEdit = data;
                $state.go('management', { reportName: chart.name });
            });
        }

        function deleteChart(chart) {
            resourceUtil.get(controllerPath, { action: 'GetChartData', categoryName: $stateParams.categoryName, reportName: $stateParams.name, chartName: chart.name }).then(function (data) {
                resourceUtil.delete(controllerPath, { id: data.reportChartId, action: 'Delete' }).then(function () {
                    toastr.success('Delete successfully!', 'Delete information');
                });
            });

        }

        function redirectToTfs(chartLabel, chartName) {
            resourceUtil.get(controllerPath, { action: 'GetFilterQueryLink', categoryName: $stateParams.categoryName, reportName: $stateParams.name, chartName: chartName, filterBy: chartLabel.data.label })
                .then(function (data) {
                    $window.open(data.responseObject);
                });
        }

    }
})();