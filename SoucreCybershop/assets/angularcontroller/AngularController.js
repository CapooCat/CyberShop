var app = angular.module('MyApp', [])
        .directive('loading', function () {
            return {
                restrict: 'E',
                replace: true,
                template: '<div class="loading"><div class="obj"></div><div class="obj"></div><div class="obj"></div><div class="obj"></div><div class="obj"></div><div class="obj"></div><div class="Loading-background"></div></div>',
                link: function (scope, element, attr) {
                    scope.$watch('loading', function (val) {
                        if (val)
                            $(element).show();
                        else
                            $(element).hide();
                    });
                }
            }
        })
app.controller('MyController', function ($scope, $http) {
    $a = window.location.pathname;
    
    if ($a.includes("/Product") != false) {
        $scope.loading = true;
        $http.get("/Product/SanPham").then(function (res) {
            $scope.loading = false;
            $scope.data = res.data;
        });
    }
    
    if ($a.includes("/danh-muc/") != false || $a.includes("/chi-tiet-danh-muc/") != false || $a.includes("/san-pham/") != false) {
        $scope.loading = true;
        $http.get(window.location.pathname + "/JSON")
            .then(function (response) {
                $scope.loading = false;
                $scope.datalist = response.data;
            });
    }
    
    $scope.FetchCart = function () {
        $http.get("/Cart/ReturnCartItem").then(function (response) {
            $scope.cartList = response.data;
        });
    }
    $scope.bucket = { total_amount: 0 };
    $scope.FetchCart();
    $scope.AddItem = function (id) {
        $scope.loading = true;
        $http.get("/Cart/AddItem/" + id).then(function (response) {
            $scope.bucket.total_amount = 0;
            $scope.loading = false;
            $scope.FetchCart();
        });
    }
    $scope.RemoveItem = function (id) {
        $http.get("/Cart/RemoveItem/" + id).then(function (response) {
            $scope.bucket.total_amount = 0;
            $scope.FetchCart();
        });
    }
    $scope.MiniusItem = function (id) {
        $http.get("/Cart/MiniusItem/" + id).then(function (response) {
            $scope.bucket.total_amount = 0;
            $scope.FetchCart();
        });
    }
    $scope.sortBy = '+Price';

    //Kiếm tra đã chọn thành phố hay chưa

});

