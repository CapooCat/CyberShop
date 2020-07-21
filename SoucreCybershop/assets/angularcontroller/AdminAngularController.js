﻿var app = angular.module('MyAdmin', [])
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
app.controller('MyAdminController', function ($scope, $http, $filter) {
    var InvoiceID = 0;
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
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            $scope.getCatList();
            $scope.getCatListLv2();
            $scope.getCatListLv3();
        });
    }
    $scope.propertyName = 'Id';
    $scope.sortByInvoice = function (propertyName) {
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
    $scope.bucket = { total_price: 0 };
    $scope.EditInvoice = function (id) {
        InvoiceID = id;
        $http.get("/Admin/InvoiceManager/ReturnInvoiceById/" + id).then(function (response) {
            $scope.dataInvoice = angular.fromJson(response.data);
            $scope.clientName = $scope.dataInvoice[0].CustomerName;
            $scope.phoneNumber = $scope.dataInvoice[0].DeliveryPhoneNum;
            $scope.address = $scope.dataInvoice[0].DeliveryAddress;
            $http.get("/Admin/InvoiceManager/ReturnDetailInvoiceById/" + id).then(function (response) {
                $scope.bucket.total_price = 0;
                $scope.listInvoiceDetail = response.data;
            });
        });
    };

    $scope.AddProductToInvoice = function (id, price) {
            $http({
                url: '/Admin/InvoiceManager/AddProductToInvoice',
                method: "POST",
                data: {
                    Invoice_id: InvoiceID,
                    Product_id: id,
                    Price: price
                }
            }).then(function onSuccess(response) {
                // Handle success
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã thêm thành công',
                })
                console.log(response);
                $http.get("/Admin/InvoiceManager/ReturnDetailInvoiceById/" + InvoiceID).then(function (response) {
                    $scope.bucket.total_price = 0;
                    $scope.listInvoiceDetail = response.data;
                });
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

    $scope.DeleteDetailProduct = function (id) {
        $scope.DeleteDetailInvoiceId = id;
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteDetailInvoiceConfirm();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }

    $scope.DeleteDetailInvoiceConfirm = function () {
        $http.get("/Admin/InvoiceManager/DeleteDetailInvoice/" + $scope.DeleteDetailInvoiceId).then(function (response) {
            console.log(response);
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            $http.get("/Admin/InvoiceManager/ReturnDetailInvoiceById/" + InvoiceID).then(function (response) {
                $scope.bucket.total_price = 0;
                $scope.listInvoiceDetail = response.data;
            });
        });
    }

    //--------------PRODUCT_START--------------------//
    $scope.ReturnProductList = function () {
        $http.get("/Admin/ProductManager/ReturnProduct").then(function (response) {
            $scope.productList = response.data;
        });
    }
    $scope.ReturnBrandList = function () {
        $http.get("/Admin/ProductManager/ReturnBrand").then(function (response) {
            $scope.brandList = response.data;
        });
    }
    $scope.ReturnProductTypeList = function () {
        $http.get("/Admin/ProductManager/ReturnProductType").then(function (response) {
            $scope.productTypeList = response.data;
        });
    }

    $scope.ReturnProductTypeList();
    $scope.ReturnBrandList();
    $scope.ReturnProductList();

    $scope.sortByProduct = function (propertyName) {
        $scope.reverse = (propertyName !== null && $scope.propertyName === propertyName)
        ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
        if ($scope.reverse == false) {
            $scope.productList = $filter('orderBy')($scope.productList, propertyName);
        }
        else {
            $scope.productList = $filter('orderBy')($scope.productList, '-' + propertyName);
        }
    };
    $scope.FilterProduct = function () {
        var productId = document.getElementById("product_id").value;
        var productName = document.getElementById("product_name").value;
        var e = document.getElementById("product_type");
        var productTypeId = e.options[e.selectedIndex].value;
        var productBrand = document.getElementById("product_brand").value;
        var priceFrom = document.getElementById("product_priceFrom").value;
        var priceTo = document.getElementById("product_priceTo").value;
        $http({
            url: '/Admin/ProductManager/FilterProduct',
            method: "POST",
            data: {
                id: productId,
                ProductName:productName,
                ProductType_id: productTypeId,
                BrandName: productBrand,
                PriceFrom: priceFrom,
                PriceTo: priceTo
            }
        }).then(function onSuccess(response) {
            // Handle success
            $scope.productList = response.data;
            console.log(response);
            
        }).catch(function onError(response) {
            // Handle error
            console.log(response);
        });
    }
    $scope.sortBrandBy = function (BrandName) {
        $scope.reverseBrand = ($scope.BrandName === BrandName) ? !$scope.reverseBrand : false;
        $scope.BrandName = BrandName;
    };
    $scope.sortTypeBy = function (PTypeName) {
        $scope.reverseType = ($scope.PTypeName === PTypeName) ? !$scope.reverseType : false;
        $scope.PTypeName = PTypeName;
    };

    $scope.AddBrand = function () {
        var Brand = document.getElementById("txt_Brand").value;
        var MetaTitle = document.getElementById("txt_Brand_MetaTitle").value;

        if (Brand == "" || MetaTitle == "") {
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Không được bỏ trống',
            })
        }
        else {
            $http({
                url: '/Admin/ProductManager/AddBrand',
                method: "POST",
                data: {
                    BrandName: Brand,
                    MetaTitle: MetaTitle
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

    $scope.AddType = function () {
        var Type = document.getElementById("txt_Type").value;
        var MetaTitle = document.getElementById("txt_Type_MetaTitle").value;

        if (Type == "" || MetaTitle == "") {
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Không được bỏ trống',
            })
        }
        else {
            $http({
                url: '/Admin/ProductManager/AddType',
                method: "POST",
                data: {
                    TypeName: Type,
                    MetaTitle: MetaTitle
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
    
    $scope.DeleteBrand = function (id) {
        $scope.DeleteBrandId = id;
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteBrandConfirm();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }
    $scope.DeleteBrandConfirm = function () {
        $http.get("/Admin/ProductManager/DeleteBrand/" + $scope.DeleteBrandId).then(function (response) {
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            $scope.ReturnBrandList();
        });
    }

    $scope.DeleteType = function (id) {
        $scope.DeleteTypeId = id;
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteTypeConfirm();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }
    $scope.DeleteTypeConfirm = function () {
        $http.get("/Admin/ProductManager/DeleteType/" + $scope.DeleteTypeId).then(function (response) {
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            $scope.ReturnProductTypeList();
        });
    }
    $scope.returnHistory=function() {
        http.get("/Admin/ProductManager/ReturnHistory").then(function (response) {
            $scope.lstHistory = response.data;
        });
    }

    //--------------PRODUCT_END--------------------//
});