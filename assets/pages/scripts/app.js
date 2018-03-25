
var app = angular.module('FMS', []);

app.controller('CheckoutController', function ($scope, $http) {
    $scope.firstName = "John";
    $scope.lastName = "Doe";

    $scope.nextQuestion = function () {

        $http.get("/api/checkout/GetAllProducts").success(function (data) {
            console.log(data);
        }).error(function () {
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });
    };


});