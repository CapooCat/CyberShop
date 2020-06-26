var app = angular.module('MyAdmin', [])
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
app.controller('MyAdminController', function ($scope, $http) {
    $scope.getCatList = function () {
        $http.get("/Admin/CategoryManager/ReturnCategory").then(function (response) {
            $scope.catList = response.data;
        });
    }
    $scope.getCatListLv2 = function () {
        $http.get("/Admin/CategoryManager/ReturnCategoryLv2/" + 1).then(function (response) {
            $scope.catListLv2 = response.data;
            $scope.CateNameLv1 = "Laptop";
            //$scope.CateJson = angular.fromJson(response.data);
            //$http.get("/Admin/CategoryManager/ReturnNameCategory/" + $scope.CateJson[0].category_lv1_master_id).then(function (response) {
            //    $scope.NameJson = angular.fromJson(response.data);
            //    $scope.CateNameLV1 = $scope.NameJson[0].Name;
            //});
        });
    }
    $scope.getCatListLv3 = function () {
        $http.get("/Admin/CategoryManager/ReturnCategoryLv3/" + 1).then(function (response) {
            $scope.catListLv3 = response.data;
            $scope.CateNameLv2 = "Hãng Laptop";
        });
    }
    //LoadCategory
    $scope.loading = true;
        $scope.getCatList();
        $scope.getCatListLv2();
        $scope.getCatListLv3();
    $scope.loading = false;
    $scope.DetailCateLv1 = function (id) {
        $http.get("/Admin/CategoryManager/ReturnCategoryLv2/" + id).then(function (response) {
            $scope.loading = true;
            $scope.catListLv2 = response.data;
            $scope.firstCatIdLv2 = angular.fromJson(response.data);
            $http.get("/Admin/CategoryManager/ReturnCategoryLv3/" + $scope.firstCatIdLv2[0].category_lv2_id).then(function (response) {
                $scope.catListLv3 = response.data;
            });
            $http.get("/Admin/CategoryManager/ReturnNameCategory/" + id).then(function (response) {
                $scope.NameJson = angular.fromJson(response.data);
                $scope.CateNameLv1 = $scope.NameJson[0].Name;
            });
            $http.get("/Admin/CategoryManager/ReturnNameCategoryLv2/" + $scope.firstCatIdLv2[0].category_lv2_id).then(function (response) {
                $scope.NameJson = angular.fromJson(response.data);
                $scope.CateNameLv2 = $scope.NameJson[0].Name;
            });
            $scope.loading = false;
        });
    }
    $scope.DetailCateLv2 = function (id) {
        $http.get("/Admin/CategoryManager/ReturnCategoryLv3/" + id).then(function (response) {
            $scope.loading = true;
            $scope.catListLv3 = response.data;
            $http.get("/Admin/CategoryManager/ReturnNameCategoryLv2/" + id).then(function (response) {
                $scope.NameJson = angular.fromJson(response.data);
                $scope.CateNameLv2 = $scope.NameJson[0].Name;
            });
            $scope.loading = false;
        });
    }
    $scope.AddCategory = function () {
        var CategoryName = document.getElementById("inp_CategoryName").value;
        var CateKeyName = document.getElementById("inp_CategoryKey").value;
        if (CategoryName == null || CateKeyName == null) {

        }
        else
        {
            $http({
                url: '/Admin/CategoryManager/AddCategory',
                method: "POST",
                data: {
                    Name: CategoryName,
                    Metatitle:CateKeyName
                }
            }).then(function onSuccess(response) {
                // Handle success
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                console.log(response);
            });
        }
    }
});