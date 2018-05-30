(function() {
  'use strict';

  angular
    .module('cxDashboard')
    .config(routerConfig);

  /** @ngInject */
  function routerConfig($stateProvider, $urlRouterProvider) {
    $stateProvider
      .state('home', {
        url: '/',
        templateUrl: 'app/main/main.html',
        controller: 'MainController',
        controllerAs: 'main'
      })
      .state('reports', {
            url: '/reports',
            controller: 'ReportsDashboardController',
            templateUrl: 'app/reportsDashboard/reportsDashboard.html',
            controllerAs: 'reportsDashboard'
        })
        .state('reports.report', {
            url: '/report/{categoryName}/{name}',
            controller: 'ReportController',
            templateUrl: 'app/report/report.html',
            controllerAs: 'report'
        })
        .state('management', {
            url: '/management/{reportName}',
            controller: 'ManagementController',
            templateUrl: 'app/management/management.html',
            controllerAs: 'management'
        })
        .state('login', {
            url: '/login',
            controller: 'LoginController',
            templateUrl: 'app/login/login.html',
            controllerAs: 'login'
        });

    $urlRouterProvider.otherwise('/')
  }

})();
