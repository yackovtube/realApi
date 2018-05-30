(function () {
    'use strict';

    angular
        .module('cxDashboard')
        .controller('ManagementController', ManagementController);

    /** @ngInject */
    function ManagementController(resourceUtil, toastr, $state, $stateParams) {
        var controllerPath = 'api/settings/:action/:id';
        var vm = this;
        vm.report = {};
        vm.files = [];
        vm.categorySelected = categorySelected;
        vm.querySelected = querySelected;
        vm.submit = submit;
        vm.editMode = false;
        vm.setRC1Grade = setRC1Grade;
        vm.setGAGrade = setGAGrade;
        vm.updateTimeline = updateTimeline;
        vm.processFiles = processFiles;
        vm.setEngineGrade = setEngineGrade;
        vm.setSelected = setSelected;

        activate();

        function activate() {
            toastr.info('Loading query data', 'server information');

            resourceUtil.query(controllerPath, { action: 'GetCategoryList' }).then(function (data) {
                vm.availableCategoryOptions = data;
                if ($state.chartToEdit && $stateParams.reportName === $state.chartToEdit.chartName) {
                    vm.editMode = true;
                    vm.report = $state.chartToEdit;
                    categorySelected();
                }
            });
        }

        function categorySelected() {
            resourceUtil.get(controllerPath, { action: 'GetReportsAndQueriesName', categoryId: vm.report.categoryId }).then(function (data) {
                vm.availableReportNameOptions = data.allReports;
                vm.availableQueriesOptions = data.allAvailableQueries;
                if (vm.editMode)
                    querySelected();
            });
        }

        function querySelected() {
            resourceUtil.get(controllerPath, { action: 'GetQueryData', queryId: vm.report.queryId }).then(function (data) {
                vm.availableColumnOptions = data.groupByFields;
                vm.availableAggregationFields = data.aggregationFields;
            });
        }

        function submit() {
            if (vm.editMode && vm.report.reportChartId) {
                resourceUtil.update(controllerPath, { action: 'UpdateChart', id: vm.report.reportChartId }, vm.report).then(function () {
                    toastr.success('Saved successfully!', 'Save information');
                });
            } else {
                resourceUtil.save(controllerPath, { action: 'SaveNewChart' }, vm.report).then(function () {
                    toastr.success('Saved successfully!', 'Save information');
                });
            }

        }

        function setRC1Grade() {
            resourceUtil.update(controllerPath, { action: 'SetRC1Grade', version: vm.versionNumber }).then(function () {
                toastr.success('RC1 set successfully!', 'Version Grade information');
            })
        }

        function setGAGrade() {
            resourceUtil.update(controllerPath, { action: 'SetGAGrade' }).then(function () {
                toastr.success('GA set successfully!', 'Version Grade information');
            })
        }

        function updateTimeline() {

            resourceUtil.save(controllerPath, { action: 'SaveTimeline' }, { 'fileBase64': vm.files[0] }).then(function () {
                toastr.success('Image uploaded successfully', 'Update Timeline');
            });
        }

        function processFiles(file) {
            angular.forEach(file, function (flowFile, i) {
                var fileReader = new FileReader();
                fileReader.onload = function (event) {
                    var uri = event.target.result;
                    vm.files[i] = uri;
                };
                fileReader.readAsDataURL(flowFile.file);
            });
        }

        function setEngineGrade() {
            resourceUtil.update(controllerPath, { action: 'SetEngineNewVersion', engineVersion: vm.engineVersionNumber }).then(function () {
                toastr.success('Engine version set successfully!', 'Version Grade information');
            });
        }

        function setSelected(chartType){
            vm.report.chartType = chartType;
        }

    }
})();