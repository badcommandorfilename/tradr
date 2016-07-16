
    'use strict';

    var app = angular
        .module('tradr', ['ngRoute', 'ngResource']);

    // add a controller
    app.controller('tradecontroller', ['$scope', '$resource', function($scope, $resource) {
        $scope.confirm = {
            symbol: null,
            show: false
        };
        $scope.buy = function (symbol) {
            return function () {
                $scope.confirm.symbol = symbol;
                $scope.confirm.show = true;
            }
        }
        var Symbols = $resource('/api/v0/prices');
        $scope.symbols = [
        { symbol: "goog", name: "Google", own: 10, buy: 4, sell: 2, amt: 4 },
        { symbol: "msft", name: "Microsoft", own: 10, buy: 5, sell: 4, amt: 1 }
        ];
        $scope.symbols = Symbols.query();
    }]);

    // add a filter
    app.filter("myUpperFilter", function () {
        return function (input) {
            return input.toUpperCase();
        }
    });

    app.directive("tradeCalc", function () {
        var t2 = '<p class="control has-addons"> \
  <input data-ng-model=amt ng-value=amt string-to-number type="number" class="input" size="2" min="0" style="width:50px"> \
  <a class="button is-danger" style="width:190px" ng-click=action> @ ${{price}} for ${{(amt*price)}} </a>'
        return {
            scope: {
                amt: '@',
                price: '@',
                action: '&'
            },
            template: t2
        }
    });

    app.directive('stringToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (value) {
                    return '' + value;
                });
                ngModel.$formatters.push(function (value) {
                    return parseFloat(value, 10);
                });
            }
        };
    });

    app.directive("symbolRow", function () {
        var tpl = '\
      <td>{{row.symbol}}</td>\
      <td>{{row.name}}</td>\
      <td>{{row.own}}</td>\
      <td><div trade-calc amt=1 price={{row.buy}} confirm="confirm"></td>\
      <td><div trade-calc amt=1 price={{row.sell}} confirm="confirm"></td>\
    ';
        return {
            scope: {
                row: '@',
                confirm: '='
            },
            template: tpl
        }
    });