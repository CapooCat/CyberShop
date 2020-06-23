var app = angular.module('MyAdmin', []);
app.controller('MyAdminController', function ($scope, $http) {
    $scope.getCatList = function () {
        $http.get("/Admin/CategoryManager/ReturnCategory").then(function (response) {
            $scope.catList = response.data;
        });
    }
    $scope.getCatList();
    $scope.getCatListLv2 = function () {
        $http.get("/Admin/CategoryManager/ReturnCategoryLv2/" + 1).then(function (response) {
            $scope.catListLv2 = response.data;
        });
    }
    $scope.getCatListLv2()
});