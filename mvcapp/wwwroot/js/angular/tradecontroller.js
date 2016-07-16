
    'use strict';

    var app = angular
        .module('tradr', ['ngRoute', 'ngResource']);

    // add a controller
    app.controller('tradecontroller', ['$scope', '$resource', '$http', function($scope, $resource, $http) {
        $scope.confirm = {
            symbol: null,
            show: false
        };
        $scope.buy = function (symbol, quantity) {
            var req = {
                method: 'POST',
                url: '/api/v0/stocks/buy',
                headers: {
                    'Content-Type': 'application/json'
                },
                data: { symbol: symbol, quantity: quantity }
            }
            $http(req).then($scope.refresh);
        }

        var Symbols = $resource('/api/v0/stocks/prices');
        $scope.refresh = function(){
            $scope.symbols = Symbols.query();
        }
        $scope.symbols = Symbols.query();
    }]);


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
      <td><div trade-calc amt=1 price={{row.buy}} action=action></td>\
      <td><div trade-calc amt=1 price={{row.sell}} action=action></td>\
    ';
        return {
            scope: {
                row: '@',
                action: '='
            },
            template: tpl
        }
    });