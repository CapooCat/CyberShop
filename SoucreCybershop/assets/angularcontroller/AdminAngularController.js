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
app.controller('MyAdminController', function ($scope, $http,$filter) {
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
        var CategoryName = document.getElementById("inp_cateNameLv1").value;
        var a = document.getElementById("sl_prdTypeLv1");
        var productTypeKW = a.options[a.selectedIndex].value;
        var b = document.getElementById("sl_brandNameLv1");
        var brandKW = b.options[b.selectedIndex].value;
        var lowPrice = document.getElementById("inp_lowPrice").value;
        var highPrice = document.getElementById("inp_highPrice").value;
        var productKW = document.getElementById("inp_productKw").value;
        if (CategoryName == null || productTypeKW == "") {
            alert("Không được để trống");
        }
        else if ((CategoryName != null && productTypeKW != "" && lowPrice == null) || (CategoryName != null && productTypeKW != "" && highPrice == null))
        {
            alert("Không được để trống ngày");
        }
        else
        {
            $http({
                url: '/Admin/CategoryManager/AddCategory',
                method: "POST",
                data: {
                    Name: CategoryName,
                    ProductTypeKW: productTypeKW,
                    BrandKW: brandKW,
                    LowPrice: lowPrice,
                    HighPrice: highPrice,
                    ProductKW: productKW
                }
            }).then(function onSuccess(response) {
                // Handle success
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã thêm thành công',
                })
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Thêm thất bại',
                })
                console.log(response);
            });
        }
    }
    $scope.AddCategoryLv2 = function () {
        var e = document.getElementById("slc_catelv2");
        var CategoryIdLv1 = e.options[e.selectedIndex].value;
        var CategoryName = document.getElementById("inp_cateNameLv2").value;
        var a = document.getElementById("sl_prdTypeLv2");
        var productTypeKW = a.options[a.selectedIndex].value;
        var b = document.getElementById("sl_brandNameLv2");
        var brandKW = b.options[b.selectedIndex].value;
        var lowPrice = document.getElementById("inp_lowPriceLv2").value;
        var highPrice = document.getElementById("inp_highPriceLv2").value;
        var productKW = document.getElementById("inp_productKwLv2").value;
        if (CategoryName == null || CategoryIdLv1 == "" || productTypeKW == "") {
            alert("Không được để trống");
        }
        else {
            $http({
                url: '/Admin/CategoryManager/AddCategoryLv2',
                method: "POST",
                data: {
                    category_lv1_master_id: CategoryIdLv1,
                    Name: CategoryName,
                    ProductTypeKW: productTypeKW,
                    BrandKW: brandKW,
                    LowPrice: lowPrice,
                    HighPrice: highPrice,
                    ProductKW: productKW
                }
            }).then(function onSuccess(response) {
                // Handle success
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã thêm thành công',
                })
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Thêm thất bại',
                })
                console.log(response);
            });
        }
    }
    $scope.ReturnInvoice = function () {
        $http.get("/Admin/InvoiceManager/ReturnInvoice").then(function (response) {
            $scope.invoiceList = response.data;
        });
    }
    $scope.ReturnInvoice();
    $scope.ReturnCateLv2 = function () {
        switch ($scope.selectionCat) {
            case "":
                $scope.cateListLv2 = null;
                break;
            case $scope.selectionCat:
                $http.get("/Admin/CategoryManager/ReturnCategoryLv2/" + $scope.selectionCat).then(function (response) {
                    $scope.cateListLv2 = response.data;
                });
                break;
        }
    }
    $scope.AddCategoryLv3 = function () {
        var e = document.getElementById("slc_catelv3_1");
        var CategoryIdLv1 = e.options[e.selectedIndex].value;
        var d = document.getElementById("slc_catelv3_2");
        var CategoryIdLv2 = d.options[d.selectedIndex].value;
        var CategoryName = document.getElementById("inp_cateNameLv3").value;
        var a = document.getElementById("sl_prdTypeLv3");
        var productTypeKW = a.options[a.selectedIndex].value;
        var b = document.getElementById("sl_brandNameLv3");
        var brandKW = b.options[b.selectedIndex].value;
        var lowPrice = document.getElementById("inp_lowPriceLv3").value;
        var highPrice = document.getElementById("inp_highPriceLv3").value;
        var productKW = document.getElementById("inp_productKwLv3").value;
        if (CategoryName == null || CategoryIdLv1 == "" || productTypeKW == "" || CategoryIdLv2=="") {
            alert("Không được để trống");
        }
        else {
            $http({
                url: '/Admin/CategoryManager/AddCategoryLv3',
                method: "POST",
                data: {
                    category_lv1_master_id: CategoryIdLv1,
                    category_lv2_master_id:CategoryIdLv2,
                    Name: CategoryName,
                    ProductTypeKW: productTypeKW,
                    BrandKW: brandKW,
                    LowPrice: lowPrice,
                    HighPrice: highPrice,
                    ProductKW: productKW
                }
            }).then(function onSuccess(response) {
                // Handle success
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã thêm thành công',
                })
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Thêm thất bại',
                })
                console.log(response);
            });
        }
    }
    $scope.FilterInvoice = function () {
        var invoiceId = document.getElementById("invoice_id").value;
        var dateFrom = document.getElementById("date_from").value;
        var dateTo = document.getElementById("date_to").value;
        var customerName = document.getElementById("customer_name").value;
        var customerPhone = document.getElementById("customer_phone").value;
            $http({
                url: '/Admin/InvoiceManager/FilterInvoice',
                method: "POST",
                data: {
                    Id: invoiceId,
                    DateFrom: dateFrom,
                    DateTo: dateTo,
                    CustomerName: customerName,
                    DeliveryPhoneNum: customerPhone
                }
            }).then(function onSuccess(response) {
                // Handle success
                $scope.invoiceList = response.data;
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                console.log(response);
            }); 
    }
    $scope.UpdateCategory = function (id) {
        $http.get("/Admin/CategoryManager/ReturnCategoryUpdate/" + id).then(function (response) {
            $scope.DataUpdate = angular.fromJson(response.data);
            $scope.CateId = $scope.DataUpdate[0].Id;
            $scope.CateUpdate = $scope.DataUpdate[0].Name;
            $scope.MetatitleUpdate = $scope.DataUpdate[0].Metatitle;
        });
    }
    $scope.UpdateCategoryPost = function () {
        var Id = document.getElementById("cate_id_lv1").value;
        var cateName = document.getElementById("inp_updateCatelv1").value;
        var Metatitle = document.getElementById("inp_updateMetatitlelv1").value;
        $http({
            url: '/Admin/CategoryManager/CategoryUpdatePost',
            method: "POST",
            data: {
                Id: Id,
                Name: cateName,
                Metatitle: Metatitle
            }
        }).then(function onSuccess(response) {
            // Handle success
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã sửa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Sửa thất bại',
            })
            console.log(response);
        });
    }
    $scope.UpdateCategoryLv2 = function (id) {
        $http.get("/Admin/CategoryManager/ReturnCategoryUpdateLv2/" + id).then(function (response) {
            $scope.DataUpdate = angular.fromJson(response.data);
            $scope.CateId = $scope.DataUpdate[0].Id;
            $scope.CateLv1 = $scope.DataUpdate[0].CateNameLv1;
            $scope.CateUpdate = $scope.DataUpdate[0].Name;
            $scope.MetatitleUpdate = $scope.DataUpdate[0].Metatitle;
        });
    }
    $scope.UpdateCategoryPostLv2 = function () {
        var Id = document.getElementById("cate_id_lv2").value;
        var cateName = document.getElementById("inp_updateCatelv2").value;
        var Metatitle = document.getElementById("inp_updateMetatitlelv2").value;
        $http({
            url: '/Admin/CategoryManager/CategoryUpdatePost',
            method: "POST",
            data: {
                Id: Id,
                Name: cateName,
                Metatitle: Metatitle
            }
        }).then(function onSuccess(response) {
            // Handle success
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã sửa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Sửa thất bại',
            })
            console.log(response);
        });
    }
    $scope.UpdateCategoryLv3 = function (id) {
        $http.get("/Admin/CategoryManager/ReturnCategoryUpdateLv3/" + id).then(function (response) {
            $scope.DataUpdate = angular.fromJson(response.data);
            $scope.CateId = $scope.DataUpdate[0].Id;
            $scope.CateLv1 = $scope.DataUpdate[0].CateNameLv1;
            $scope.CateLv2 = $scope.DataUpdate[0].CateNameLv2;
            $scope.CateUpdate = $scope.DataUpdate[0].Name;
            $scope.MetatitleUpdate = $scope.DataUpdate[0].Metatitle;
        });
    }
    $scope.UpdateCategoryPostLv3 = function () {
        var Id = document.getElementById("cate_id_lv3").value;
        var cateName = document.getElementById("inp_updateCatelv3").value;
        var Metatitle = document.getElementById("inp_updateMetatitlelv3").value;
        $http({
            url: '/Admin/CategoryManager/CategoryUpdatePost',
            method: "POST",
            data: {
                Id: Id,
                Name: cateName,
                Metatitle: Metatitle
            }
        }).then(function onSuccess(response) {
            // Handle success
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã sửa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Sửa thất bại',
            })
            console.log(response);
        });
    }
    $scope.DeleteCategory = function (id) {
        $scope.DeleteCateId = id;
    }
    $scope.DeleteConfirm = function ()
    {
        $http.get("/Admin/CategoryManager/DeleteCategory/" + $scope.DeleteCateId).then(function (response) {
            $scope.getCatList();
            $scope.getCatListLv2();
            $scope.getCatListLv3();
        });
    }
    $scope.propertyName = 'Id';
    $scope.sortBy = function (propertyName) {
        $scope.reverse = (propertyName !== null && $scope.propertyName === propertyName)
        ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
        if ($scope.reverse == false) {
            $scope.invoiceList = $filter('orderBy')($scope.invoiceList, propertyName);
        }
        else {
            $scope.invoiceList = $filter('orderBy')($scope.invoiceList, '-' + propertyName);
        }   
    };
});