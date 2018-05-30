angular.module('cxDashboard')
    .directive('reportChart', ReportDirective);

function ReportDirective() {
    return {
        controller: 'ReportController',
        controllerAs: 'reportCtrl',
        bindToController: true,
        link: function (scope, element, attrs) {
            attrs.$observe('reportChart', function (param) {
                var value = angular.fromJson(param);
                switch (value.type) {
                    case 'line':
                        Morris.Line({
                            element: value.element,
                            data: [
                                { y: '2006', a: 150, b: null },
                                { y: '2007', a: 125,  b: null },
                                { y: '2008', a: 100,  b: 115 },
                                { y: '2009', a: 75,  b: null },
                                { y: '2010', a: 50,  b: null },
                                { y: '2011', a: 25,  b: null },
                                { y: '2012', a: 0, b: null }
                              ],
                            xkey: 'y',
                            ykeys: ['a', 'b'],
                           labels: ['Series A', 'Series B']
                        });
                        break;
                    case 'pie':
                        var pie = new d3pie(value.element, {
                            "size": {
                                "canvasHeight": 400,
                                "canvasWidth": 590,
                                "pieOuterRadius": "100%"
                            },
                            "data": {
                                "content": value.data
                            },
                            "labels": {
                                "outer": {
                                    "pieDistance": 32
                                },
                                "inner": {
                                    "format": value.dataShowType
                                },
                                "mainLabel": {
                                    "font": "verdana",
                                    "fontSize": 12
                                },
                                "percentage": {
                                    "color": "#e1e1e1",
                                    "font": "verdana",
                                    "decimalPlaces": 0,
                                    "fontSize": 14
                                },
                                "value": {
                                    "color": "#e1e1e1",
                                    "font": "verdana",
                                    "fontSize": 14
                                },
                                "lines": {
                                    "enabled": true,
                                    "color": "#cccccc"
                                },
                                "truncation": {
                                    "enabled": true
                                }
                            },
                            "effects": {
                                "pullOutSegmentOnClick": {
                                    "effect": "linear",
                                    "speed": 400,
                                    "size": 8
                                }
                            },
                            "callbacks": {
                                onClickSegment: function (a) {
                                    scope.reportCtrl.redirectToTfs(a, value.name);
                                }
                            }
                        });
                        break;
                    case 'Bar':
                        Morris.Bar({
                            element: value.element,
                            data: value.data,
                            xkey: value.xkey,
                            ykeys: value.ykeys,
                            labels: value.labels,
                            hideHover: value.hideHover,
                            resize: value.resize
                        });
                        break;
                }
            });
        }
    }
}