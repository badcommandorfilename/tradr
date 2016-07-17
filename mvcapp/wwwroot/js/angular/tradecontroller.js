
    'use strict';

    var app = angular
        .module('tradr', ['ngRoute', 'ngResource']);

    // add a controller
    app.controller('tradecontroller', ['$scope', '$resource', '$q', function($scope, $resource, $q) {
        $scope.confirm = {
            symbol: null,
            show: false
        };

        var Stocks = $resource('/api/v0/portfolio/stocks');
        var Symbols = $resource('/api/v0/stocks/prices');

        $scope.refresh = function () {

            $q.all([Stocks.query({}).$promise, Symbols.query({}).$promise])
                .then(function (values) {
                    var stocks = values[0];
                    var symbols = values[1];
                    $scope.stocks = stocks;
                    for (var i = 0; i < symbols.length; i++){
                        symbols[i].own = 0
                        for (var j = 0; j < $scope.stocks.length; j++)
                        {
                            if ($scope.stocks[j].symbol === symbols[i].symbol) {
                                symbols[i].own = $scope.stocks[j].quantity;
                                break;
                            }
                        }
                    }
                    $scope.symbols = symbols;
            })
        }
        $scope.refresh();
    }]);


    app.directive("tradeCalc", ['$http', function($http) {
        var t2 = '<p class="control has-addons"> \
  <input data-ng-model=amt ng-value=amt string-to-number type="number" class="input" size="2" min="0" style="width:50px"> \
  <a class="button is-danger" style="width:190px" data-ng-click=callaction()> @ ${{price}} for ${{(amt*price)}}</a>'
        return {
            scope: {
                amt: '@',
                price: '@',
                sym: '@'
            },
            template: t2,
            link: function (scope, elm, attrs) {
                var buy = function (symbol, quantity) {
                        var req = {
                            method: 'POST',
                            url: '/api/v0/stocks/buy',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            data: { symbol: symbol, quantity: quantity }
                        }
                        $http(req);
                    }

                scope.callaction = function() {
                    buy(scope.sym, scope.amt);
                }
            }
        }
    }]);

    app.directive('stringToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (value) {
                    return '' + value;
                });
                ngModel.$formatters.push(function (value) {
                    try {
                        return parseFloat(value, 10);
                    } catch (err) {
                        return 1;
                    }
                });
            }
        };
    });