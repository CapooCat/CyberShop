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
            alert("Không được để trống");
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
                alert("Thêm thành công");
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                alert("Thêm thất bại");
                console.log(response);
            });
        }
    }
    $scope.AddCategoryLv2 = function () {
        var e = document.getElementById("slc_lv1");
        var CategoryIdLv1 = e.options[e.selectedIndex].value;
        var CategoryNameLv2 = document.getElementById("inp_CategoryNameLv2").value;
        var MetatitleLv2 = document.getElementById("inp_CategoryKeyLv2").value;
        if (CategoryNameLv2 == null || MetatitleLv2 == null) {
            alert("Không được để trống");
        }
        else {
            $http({
                url: '/Admin/CategoryManager/AddCategoryLv2',
                method: "POST",
                data: {
                    category_lv1_master_id: CategoryIdLv1,
                    Name: CategoryNameLv2,
                    Metatitle: MetatitleLv2
                }
            }).then(function onSuccess(response) {
                // Handle success
                alert("Thêm thành công");
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                alert("Thêm thất bại");
                console.log(response);
            });
        }
    }
});