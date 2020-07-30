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
    $scope.txt_Add_Type = "";
    $scope.txt_Add_Brand = "";
    var InvoiceID = 0;
    var ProductID = 0;
    var n = 0;
    $scope.TempIMG = [];

    
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
    $scope.getCatList();
    $scope.getCatListLv2();
    $scope.getCatListLv3();
    $scope.DetailCateLv1 = function (id) {
        $scope.loading = true;
        $http.get("/Admin/CategoryManager/ReturnCategoryLv2/" + id).then(function (response) {
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
        $scope.loading = true;
        $http.get("/Admin/CategoryManager/ReturnCategoryLv3/" + id).then(function (response) {
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
        $scope.loading = true;
        if (CategoryName == null || productTypeKW == "") {
            $scope.loading = false;
            alert("Không được để trống");
        }
        else if ((CategoryName != null && productTypeKW != "" && lowPrice == null) || (CategoryName != null && productTypeKW != "" && highPrice == null))
        {
            $scope.loading = false;
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
                $scope.loading = false;
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã thêm thành công',
                })
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
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
        $scope.loading = true;
        if (CategoryName == null || CategoryIdLv1 == "" || productTypeKW == "") {
            $scope.loading = false;
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
                $scope.loading = false;
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã thêm thành công',
                })
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Thêm thất bại',
                })
                console.log(response);
            });
        }
    }
    
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
        $scope.loading = true;
        if (CategoryName == null || CategoryIdLv1 == "" || productTypeKW == "" || CategoryIdLv2 == "") {
            $scope.loading = false;
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
                $scope.loading = false;
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã thêm thành công',
                })
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
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
        $scope.loading = true;
        if (dateFrom > dateTo) {
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Thời gian từ không được lớn hơn Thời gian đến',
            })
        }
        else {
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
                $scope.loading = false;
                $scope.invoiceList = response.data;
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                console.log(response);
            });
        }
    }
    $scope.UpdateCategory = function (id) {
        $scope.loading = true;
        $http.get("/Admin/CategoryManager/ReturnCategoryUpdate/" + id).then(function (response) {
            $scope.DataUpdate = angular.fromJson(response.data);
            $scope.CateId = $scope.DataUpdate[0].Id;
            $scope.CateUpdate = $scope.DataUpdate[0].Name;
            $scope.MetatitleUpdate = $scope.DataUpdate[0].Metatitle;
            $scope.loading = false;
        });
    }
    $scope.UpdateCategoryPost = function () {
        var Id = document.getElementById("cate_id_lv1").value;
        var cateName = document.getElementById("inp_updateCatelv1").value;
        var Metatitle = document.getElementById("inp_updateMetatitlelv1").value;
        $scope.loading = true;
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
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã sửa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Sửa thất bại',
            })
            console.log(response);
        });
    }
    $scope.UpdateCategoryLv2 = function (id) {
        $scope.loading = true;
        $http.get("/Admin/CategoryManager/ReturnCategoryUpdateLv2/" + id).then(function (response) {
            $scope.DataUpdate = angular.fromJson(response.data);
            $scope.CateId = $scope.DataUpdate[0].Id;
            $scope.CateLv1 = $scope.DataUpdate[0].CateNameLv1;
            $scope.CateUpdate = $scope.DataUpdate[0].Name;
            $scope.MetatitleUpdate = $scope.DataUpdate[0].Metatitle;
            $scope.loading = false;
        });
    }
    $scope.UpdateCategoryPostLv2 = function () {
        var Id = document.getElementById("cate_id_lv2").value;
        var cateName = document.getElementById("inp_updateCatelv2").value;
        var Metatitle = document.getElementById("inp_updateMetatitlelv2").value;
        $scope.loading = true;
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
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã sửa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Sửa thất bại',
            })
            console.log(response);
        });
    }
    $scope.UpdateCategoryLv3 = function (id) {
        $scope.loading = true;
        $http.get("/Admin/CategoryManager/ReturnCategoryUpdateLv3/" + id).then(function (response) {
            $scope.DataUpdate = angular.fromJson(response.data);
            $scope.CateId = $scope.DataUpdate[0].Id;
            $scope.CateLv1 = $scope.DataUpdate[0].CateNameLv1;
            $scope.CateLv2 = $scope.DataUpdate[0].CateNameLv2;
            $scope.CateUpdate = $scope.DataUpdate[0].Name;
            $scope.MetatitleUpdate = $scope.DataUpdate[0].Metatitle;
            $scope.loading = false;
        });
    }
    $scope.UpdateCategoryPostLv3 = function () {
        var Id = document.getElementById("cate_id_lv3").value;
        var cateName = document.getElementById("inp_updateCatelv3").value;
        var Metatitle = document.getElementById("inp_updateMetatitlelv3").value;
        $scope.loading = true;
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
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã sửa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
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
        $scope.loading = true;
        $http.get("/Admin/CategoryManager/DeleteCategory/" + $scope.DeleteCateId).then(function (response) {
            $scope.loading = false;
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
    }
    $scope.bucket = { total_price: 0 };

    $scope.DeleteInvoiceChecked = function () {
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteInvoiceCheckedConfirm();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }
    $scope.DeleteInvoiceCheckedConfirm = function () {
        CheckAll = document.getElementById("check-all");
        table = document.getElementById("items-table");
        ItemCheckBox = table.getElementsByTagName("input");
        var lstId = [];
        if (CheckAll.checked == true) {
            for (i = 0; ItemCheckBox.length > i; i++) {
                if (ItemCheckBox[i].type == 'checkbox' && ItemCheckBox[i].checked == true)
                    lstId.push(ItemCheckBox[i].value);
            }
        }
        else
        {
            for (i = 0; ItemCheckBox.length > i; i++) {
                if (ItemCheckBox[i].type == 'checkbox' && ItemCheckBox[i].checked == true)
                   lstId.push(ItemCheckBox[i].value);
            }
        }
        $scope.loading = false;
        $http({
            url: '/Admin/InvoiceManager/DeleteInvoiceChecked',
            method: "POST",
            data: {
                id: lstId
            },
        }).then(function onSuccess(response) {
            $scope.invoiceList = response.data;
            // Handle success
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Xóa thất bại',
            })
            console.log(response);
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
        $scope.loading = true;
        if (priceFrom > priceTo) {
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Giá tiền từ không được lớn hơn Giá tiền đến',
            })
        }
        else {
            $http({
                url: '/Admin/ProductManager/FilterProduct',
                method: "POST",
                data: {
                    id: productId,
                    ProductName: productName,
                    ProductType_id: productTypeId,
                    BrandName: productBrand,
                    PriceFrom: priceFrom,
                    PriceTo: priceTo
                }
            }).then(function onSuccess(response) {
                // Handle success
                $scope.loading = false;
                $scope.productList = response.data;
                console.log(response);

            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                console.log(response);
            });
        }
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
        $scope.loading = true;
        if (Brand == "" || MetaTitle == "") {
            $scope.loading = false;
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
                $scope.loading = false;
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã thêm thành công',
                })
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
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
        $scope.loading = true;
        if (Type == "" || MetaTitle == "") {
            $scope.loading = false;
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
                $scope.loading = false;
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã thêm thành công',
                })
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Thêm thất bại',
                })
                console.log(response);
            });
        }
    }

    $scope.AddProduct = function () {
        var MaxProductId = 0;
        var ProductName = document.getElementById("txt_Add_ProductName").value;
        var Type = document.getElementById("txt_Add_Type").value;
        var Brand = document.getElementById("txt_Add_Brand").value;
        var SellPrice = document.getElementById("txt_Add_SellPrice").value;
        var Quantity = document.getElementById("txt_Add_Quantity").value;
        var Doc = document.getElementById("txt_Add_Doc").value;
        var MetaTitle = document.getElementById("txt_Add_MetaTitle").value;
        var Warranty = document.getElementById("txt_Add_Warranty").value;

        $scope.loading = true;
        if (ProductName == "" || Type == "" || Brand == "" || SellPrice == "" || Quantity == "" || Doc == "" || MetaTitle == "" || Warranty == "") {
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Không được bỏ trống',
            })
        } else if ($scope.TempIMG.length == 0) {
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Vui lòng thêm ít nhất 1 hình ảnh',
            })
        } else {
            var m = 0;
            for (i = 0; $scope.TempIMG.length > i; i++) {
                if (document.getElementById("IMG_DATA").getElementsByClassName("MainIMG")[i].checked == true)
                {
                    $http({
                        url: '/Admin/ProductManager/AddProduct',
                        method: "POST",
                        data: {
                            ProductName: ProductName,
                            Brand_id: Brand,
                            ProductType_id: Type,
                            Price: SellPrice,
                            Amount: Quantity,
                            Info: Doc,
                            MetaTitle: MetaTitle
                        }
                    }).then(function onSuccess(response) {
                        // Handle success
                        var j = 0;
                        $http.get("/Admin/ProductManager/ReturnProduct").then(function (response) {
                            $scope.NewId = angular.fromJson(response.data);
                            for (i = 0; response.data.length > i; i++) {
                                if (MaxProductId < $scope.NewId[i].id)
                                    MaxProductId = $scope.NewId[i].id;
                            }
                            for (i = 0; $scope.TempIMG.length > i; i++) {
                                
                                $http({
                                    url: '/Admin/ProductManager/AddImage',
                                    method: "POST",
                                    data: {
                                        Product_id: MaxProductId,
                                        Name: $scope.TempIMG[i].Name,
                                        Url: $scope.TempIMG[i].Url
                                    }
                                }).then(function onSuccess(response) {
                                    // Handle success
                                    var MainIMG = "";
                                    for (i = 0; $scope.TempIMG.length > i; i++) {
                                        if (document.getElementById("IMG_DATA").getElementsByClassName("MainIMG")[i].checked == true) {
                                            j = j + 1;
                                            $http({
                                                url: '/Admin/ProductManager/MainImage',
                                                method: "POST",
                                                data: {
                                                    Id: MaxProductId,
                                                    Image: document.getElementById("IMG_DATA").getElementsByClassName("MainIMG")[i].value
                                                }
                                            }).then(function onSuccess(response) {
                                                // Handle success
                                                if (j >= $scope.TempIMG.length) {
                                                    $scope.loading = false;
                                                    Swal.fire({
                                                        icon: 'success',
                                                        title: 'Thành công',
                                                        text: 'thêm thành công',
                                                    })
                                                    $scope.txt_Add_ProductName = "";
                                                    $scope.txt_Add_Type = "";
                                                    $scope.txt_Add_Brand = "";
                                                    $scope.txt_Add_SellPrice = "";
                                                    $scope.txt_Add_Quantity = "";
                                                    $scope.txt_Add_Doc = "";
                                                    $scope.txt_Add_MetaTitle = "";
                                                    $scope.txt_Add_Warranty = "";
                                                    $scope.TempIMG = [];
                                                    $http.get("/Admin/ProductManager/ReturnProduct").then(function (response) {
                                                        $scope.productList = response.data;
                                                    });
                                                }
                                                console.log(response);
                                            }).catch(function onError(response) {
                                                // Handle error
                                                if (j >= $scope.TempIMG.length) {
                                                    $scope.loading = false;
                                                    Swal.fire({
                                                        icon: 'error',
                                                        title: 'Thất bại',
                                                        text: 'Thêm thất bại',
                                                    })
                                                }
                                                console.log(response);
                                            });
                                        }
                                    }
                                    console.log(response);
                                }).catch(function onError(response) {
                                    // Handle error
                                    $scope.loading = false;
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Thất bại',
                                        text: 'thêm thất bại',
                                    })
                                    console.log(response);
                                });
                            }
                        });
                        console.log(response);
                    }).catch(function onError(response) {
                        // Handle error
                        $scope.loading = false;
                        Swal.fire({
                            icon: 'error',
                            title: 'Thất bại',
                            text: 'thêm thất bại',
                        })
                        console.log(response);
                    });
                }
                else {
                    m += 1;
                    if (m == $scope.TempIMG.length) {
                        $scope.loading = false;
                        Swal.fire({
                            icon: 'error',
                            title: 'Thất bại',
                            text: 'Vui lòng chọn 1 ảnh chính',
                        })
                    }
                }
            }
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
        $scope.loading = true;
        $http.get("/Admin/ProductManager/DeleteBrand/" + $scope.DeleteBrandId).then(function (response) {
            $scope.loading = false;
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
        $scope.loading = true;
        $http.get("/Admin/ProductManager/DeleteType/" + $scope.DeleteTypeId).then(function (response) {
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            $scope.ReturnProductTypeList();
        });
    }
    $scope.returnHistory = function () {
        $scope.loading = true;
        $http.get("/Admin/ProductManager/ReturnHistory").then(function (response) {
            $scope.loading = false;
            $scope.lstHistory = response.data;
        });
    }
    $scope.DeleteProductChecked = function () {
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteProductCheckedConfirm();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }
    $scope.DeleteProductCheckedConfirm = function () {
        CheckAll = document.getElementById("check-all");
        table = document.getElementById("items-table");
        ItemCheckBox = table.getElementsByTagName("input");
        var lstId = [];
        if (CheckAll.checked == true) {
            for (i = 0; ItemCheckBox.length > i; i++) {
                if (ItemCheckBox[i].type == 'checkbox' && ItemCheckBox[i].checked == true)
                    lstId.push(ItemCheckBox[i].value);
            }
        }
        else {
            for (i = 0; ItemCheckBox.length > i; i++) {
                if (ItemCheckBox[i].type == 'checkbox' && ItemCheckBox[i].checked == true)
                    lstId.push(ItemCheckBox[i].value);
            }
        }
        $scope.loading = true;
        $http({
            url: '/Admin/ProductManager/DeleteProductChecked',
            method: "POST",
            data: {
                id: lstId
            },
        }).then(function onSuccess(response) {
            $scope.productList = response.data;
            // Handle success
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            console.log(response.data);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Xóa thất bại',
            })
            console.log(response);
        });
    }
    $scope.returnProductId = function(id){
        $scope.product_id = id;
        $scope.loading = true;
        $http.get("/Admin/ProductManager/ReturnProductById/" + id).then(function (response) {
            $scope.dataProduct = angular.fromJson(response.data);
            $scope.Product_Name = $scope.dataProduct[0].ProductName;
            $scope.Product_Type = $scope.dataProduct[0].ProductType_id;
            $scope.Brand = $scope.dataProduct[0].Brand_id;
            $scope.Sell_Price = $scope.dataProduct[0].Price;
            $scope.Quantity = $scope.dataProduct[0].Amount;
            $scope.Doc = $scope.dataProduct[0].Info;
            $scope.MetaTitle = $scope.dataProduct[0].MetaTitle;
            $scope.Warranty = $scope.dataProduct[0].MonthWarranty;
            var Image = $scope.dataProduct[0].Image;
            $http.get("/Admin/ProductManager/ReturnImage/" + id).then(function (response) {
                $scope.loading = false;
                $scope.lstImage = response.data;
                $http.get("/Admin/ProductManager/ReturnImage/" + id).then(function (response) {
                    $scope.ImageData = angular.fromJson(response.data);
                    for (i = 0; i < $scope.ImageData.length; i++) {
                        if (Image == $scope.ImageData[i].Url) {
                            document.getElementById($scope.ImageData[i].id).checked = true;
                        }
                    }
                });
            });
        }); 
    }

    $scope.UpdateProduct = function () {
        $scope.loading = true;
        var Brand_id = document.getElementById("Edit_Brand").value;
        var ProductType_id = document.getElementById("Edit_Product_Type").value;
        var ProductName = document.getElementById("Edit_Product_Name").value;
        var MetaTitle = document.getElementById("Edit_MetaTitle").value;
        var Quantity = document.getElementById("Edit_Quantity").value;
        var Info = document.getElementById("Edit_Doc").value;
        var Price = document.getElementById("Edit_Sell_Price").value;
        var MonthWarranty = document.getElementById("Edit_Warranty").value;
        console.log(Brand_id, ProductType_id, ProductName, MetaTitle, Info, Price, MonthWarranty);
        $http({
            url: '/Admin/ProductManager/ProductUpdate',
            method: "POST",
            data: {
                id: $scope.product_id,
                Brand_id: Brand_id,
                ProductType_id: ProductType_id,
                ProductName: ProductName,
                MetaTitle: MetaTitle,
                Amount: Quantity,
                Info: Info,
                Price: Price,
                MonthWarranty: MonthWarranty
            }
        }).then(function onSuccess(response) {
            // Handle success
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã sửa thành công',
            })
            $scope.ReturnProductList();
            $("[data-dismiss=modal]").trigger({ type: "click" });
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Sửa thất bại',
            })
            console.log(response);
        });
    }

    $scope.MainImage = function (url) {
        $scope.loading = true;
            $http({
                url: '/Admin/ProductManager/MainImage',
                method: "POST",
                data: {
                    Id: $scope.product_id,
                    Image: url
                }
            }).then(function onSuccess(response) {
                // Handle success
                $scope.loading = false;
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'đổi thất bại',
                })
                console.log(response);
            });
    }

    $scope.AddFiles = function (files) {
        $scope.addfile = files[0];
        $scope.TempFiles();
    }

    $scope.TempFiles = function () {
        $scope.loading = true;
        var fdata = new FormData();
        fdata.append("files", $scope.addfile);
        $http.post("/Admin/ProductManager/AddTempImage", fdata, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).then(function onSuccess(response) {
            // Handle success
            $scope.ItemIMG = angular.fromJson(response.data);
            $scope.TempIMG.push($scope.ItemIMG[0]);
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Lưu thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Lưu thất bại',
            })
            console.log(response);
        });
    }

    $scope.DeleteTempIMG = function (Name) {
        var index = $scope.TempIMG.indexOf(Name);
        $scope.TempIMG.splice(index, 1);
    }

    $scope.SelectedFiles=function(files) {
        $scope.selectedfile = files[0];
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có muốn lưu hình ảnh này ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.UploadFiles();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                document.getElementById("myFile2").value = "";
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Lưu thất bại',
                })
            }
        })
    }

    $scope.UploadFiles = function () {
        $scope.loading = true;
            var fdata = new FormData();
            fdata.append("Product_Id", $scope.product_id);
            fdata.append("files", $scope.selectedfile);
            $http.post("/Admin/ProductManager/UploadImage", fdata, {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).then(function onSuccess(response) {
                // Handle success
                $http.get("/Admin/ProductManager/ReturnImage/" + $scope.product_id).then(function (response) {
                    $scope.lstImage = response.data;
                });
                $scope.loading = false;
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Lưu thành công',
                })
                $scope.lstImage = response.data;
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Lưu thất bại',
                })
                console.log(response);
            });
    }
    $scope.DeleteProduct = function (id) {
        $scope.product_id = id;
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteProductConfirm();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }



    $scope.DeleteImage = function (id) {
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteImageConfirm(id);
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }

    $scope.DeleteImageConfirm = function (id) {
        $scope.loading = true;
        $http({
            url: '/Admin/ProductManager/DeleteImage',
            method: "POST",
            data: {
                id: id
            },
        }).then(function onSuccess(response) {
            // Handle success
            $http.get("/Admin/ProductManager/ReturnImage/" + $scope.product_id).then(function (response) {
                $scope.lstImage = response.data;
            });
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Xóa thất bại',
            })
            console.log(response);
        });
    }

    $scope.DeleteProductConfirm = function () {
        $scope.loading = true;
        $http({
            url: '/Admin/ProductManager/DeleteProduct',
            method: "POST",
            data: {
                id: $scope.product_id
            },
        }).then(function onSuccess(response) {
            $scope.productList = response.data;
            // Handle success
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Xóa thất bại',
            })
            console.log(response);
        });
    }


    //--------------PRODUCT_END--------------------//



    //--------------USER_START--------------------//
    $scope.returnUser = function () {
        $scope.loading = true;
        $http.get("/Admin/CustomerManager/ReturnCustomer").then(function (response) {
            $scope.lstUser = response.data;
        });
        $scope.loading = false;
    }
    $scope.returnUser();
    $scope.sortByUser = function (propertyName) {
        $scope.reverse = (propertyName !== null && $scope.propertyName === propertyName)
        ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
        if ($scope.reverse == false) {
            $scope.lstUser = $filter('orderBy')($scope.lstUser, propertyName);
        }
        else {
            $scope.lstUser = $filter('orderBy')($scope.lstUser, '-' + propertyName);
        }
    }
    $scope.FilterUser = function () {
        var customer_name = document.getElementById("customer_name").value;
        var customer_phone = document.getElementById("customer_phone").value;
        var customer_email = document.getElementById("customer_email").value;
        var customer_createdate = document.getElementById("customer_createdate").value;
        $scope.loading = true;
        $http({
            url: '/Admin/CustomerManager/FilterUser',
            method: "POST",
            data: {
                Email: customer_email,
                Name: customer_name,
                PhoneNum: customer_phone,
                CreatedDate: customer_createdate
            }
        }).then(function onSuccess(response) {
            // Handle success
            $scope.loading = false;
            $scope.lstUser = response.data;
            console.log(response);

        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            console.log(response);
        });
    }
    $scope.DeleteUserChecked = function () {
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteUserCheckedConfirm();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }
    $scope.DeleteUserCheckedConfirm = function () {
        CheckAll = document.getElementById("check-all");
        table = document.getElementById("items-table");
        ItemCheckBox = table.getElementsByTagName("input");
        var lstId = [];
        if (CheckAll.checked == true) {
            for (i = 0; ItemCheckBox.length > i; i++) {
                if (ItemCheckBox[i].type == 'checkbox' && ItemCheckBox[i].checked == true)
                    lstId.push(ItemCheckBox[i].value);
            }
        }
        else {
            for (i = 0; ItemCheckBox.length > i; i++) {
                if (ItemCheckBox[i].type == 'checkbox' && ItemCheckBox[i].checked == true)
                    lstId.push(ItemCheckBox[i].value);
            }
        }
        $scope.loading = true;
        $http({
            url: '/Admin/CustomerManager/DeleteUserChecked',
            method: "POST",
            data: {
                id: lstId
            },
        }).then(function onSuccess(response) {
            $scope.lstUser = response.data;
            // Handle success
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Xóa thất bại',
            })
            console.log(response);
        });
    }
    $scope.DeleteUser = function (id) {
        $scope.user_id = id;
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteUserConfirm();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }
    $scope.DeleteUserConfirm = function () {
        $scope.loading = true;
        $http({
            url: '/Admin/CustomerManager/DeleteUser',
            method: "POST",
            data: {
                id: $scope.user_id
            },
        }).then(function onSuccess(response) {
            $scope.lstUser = response.data;
            // Handle success
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Xóa thất bại',
            })
            console.log(response);
        });
    }
    $scope.returnUserById = function (id) {
        $scope.loading = true;
        $scope.user_id = id;
        $http.get("/Admin/CustomerManager/ReturnCustomerById/" + id).then(function (response) {
            $scope.userData = angular.fromJson(response.data);
            $scope.CustomerName = $scope.userData[0].Name;
            $scope.CustomerAddress = $scope.userData[0].Address;
            $scope.CustomerPhone = $scope.userData[0].PhoneNum;
            $scope.CustomerEmail = $scope.userData[0].Email;
            $scope.CustomerUserName = $scope.userData[0].Username;
            $scope.loading = false;
        });
    }
    $scope.UpdateUser = function () {
        var CustomerName = document.getElementById("user_name").value;
        var CustomerAdress = document.getElementById("user_address").value;
        var CustomerPhone = document.getElementById("user_phone").value;
        var CustomerEmail = document.getElementById("user_email").value;
        var CustomerUserName = document.getElementById("user_username").value;
        var CustomerPass = document.getElementById("user_pass").value;
        $scope.loading = true;
        $http({
            url: '/Admin/CustomerManager/UpdateUser',
            method: "POST",
            data: {
                id: $scope.user_id,
                Name: CustomerName,
                Address: CustomerAdress,
                PhoneNum: CustomerPhone,
                Email: CustomerEmail,
                Username: CustomerUserName,
                Password: CustomerPass
            },
        }).then(function onSuccess(response) {
            // Handle success
            $scope.lstUser = response.data;
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Cập nhật thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Cập nhật thất bại',
            })
            console.log(response);
        });
    }
    $scope.returnUserInvoice = function (id) {
        $scope.bucket = { stt: 0 };
        $scope.loading = true;
        $http.get("/Admin/CustomerManager/ReturnDetailInvoiceByUserId/" + id).then(function (response) {
            $scope.lstInvoiceDetail = response.data;
            $scope.loading = false;
        });
    }
    //--------------USER_END--------------------//


    //--------------INVOICE_START--------------------//
    var GetInvoiceId = 0;
    var GetStatus = "";

    $scope.EditInvoice = function (id) {
        InvoiceID = id;
        $scope.loading = true;
        $http.get("/Admin/InvoiceManager/ReturnInvoiceById/" + id).then(function (response) {
            $scope.dataInvoice = angular.fromJson(response.data);
            $scope.clientName = $scope.dataInvoice[0].CustomerName;
            $scope.phoneNumber = $scope.dataInvoice[0].DeliveryPhoneNum;
            $scope.address = $scope.dataInvoice[0].DeliveryAddress;
            $http.get("/Admin/InvoiceManager/ReturnDetailInvoiceById/" + id).then(function (response) {
                $scope.loading = false;
                $scope.bucket.total_price = 0;
                $scope.listInvoiceDetail = response.data;
            });
        });
    };

    $scope.ReturnInvoice = function () {
        $scope.loading = true;
        $http.get("/Admin/InvoiceManager/ReturnInvoice").then(function (response) {
            $scope.invoiceList = response.data;
        });
        $scope.loading = false;
    }
    $scope.ReturnInvoice();

    $scope.ViewInvoice = function (id, Status) {
        GetInvoiceId = id;
        GetStatus = Status;
        if (Status == "Chưa hoàn thành") { document.getElementById("ConfirmInvoice").disabled = false; document.getElementById("ConfirmInvoice").style.display = "block"; }
        else { document.getElementById("ConfirmInvoice").disabled = true; document.getElementById("ConfirmInvoice").style.display = "none"; }

        $scope.loading = true;
        $http.get("/Admin/InvoiceManager/ReturnViewInvoiceById/" + id).then(function (response) {
            $scope.dataInvoice = angular.fromJson(response.data);
            $scope.Invoice_Id = $scope.dataInvoice[0].Id;
            $scope.Invoice_CustomerName = $scope.dataInvoice[0].CustomerName;
            $scope.Invoice_DeliveryPhoneNum = $scope.dataInvoice[0].DeliveryPhoneNum;
            $scope.Invoice_DeliveryAddress = $scope.dataInvoice[0].DeliveryAddress;
            $scope.Invoice_Status = $scope.dataInvoice[0].Status;
            $scope.Invoice_CreateDate = $scope.dataInvoice[0].CreateDate;
            $http.get("/Admin/InvoiceManager/ReturnViewDetailInvoiceById/" + id).then(function (response) {
                $scope.loading = false;
                $scope.WarrantyInvoiceDetail = response.data;
            });
        });
    };



    $scope.confirm = function () {
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xác nhận ? sau khi xác nhận bạn sẽ không thể sửa hoặc xóa hóa đơn này',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.Completed();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xác nhận thất bại',
                })
            }
        })
    }

    $scope.Completed = function () {
        $scope.loading = true;
        $http({
            url: '/Admin/InvoiceManager/Confirm',
            method: "POST",
            data: {
                Id: GetInvoiceId,
                Status: "Đã hoàn thành",
            },
        }).then(function onSuccess(response) {
            // Handle success
            $scope.loading = false;
            if (response.data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Cập nhật thành công',
                })
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Không đủ số lượng hàng để xác nhận',
                })
            }
            $scope.ViewInvoice(GetInvoiceId, "Đã Hoàn Thành");
            $scope.ReturnInvoice();
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Cập nhật thất bại',
            })
            console.log(response);
        });
    }

    $scope.UpdateInvoice = function () {
        $scope.loading = true;
        var Name = document.getElementById("client_name").value;
        var SDT = document.getElementById("phone_number").value;
        var Address = document.getElementById("address").value;
        $http({
            url: '/Admin/InvoiceManager/UpdateInvoice',
            method: "POST",
            data: {
                Id: InvoiceID,
                DeliveryAddress: Address,
                DeliveryPhoneNum: SDT,
                CustomerName: Name
            },
        }).then(function onSuccess(response) {
            // Handle success
            $scope.lstUser = response.data;
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Cập nhật thành công',
            })
            $("[data-dismiss=modal]").trigger({ type: "click" });
            $scope.ReturnInvoice();
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Cập nhật thất bại',
            })
            console.log(response);
        });
    }

        $scope.AddProductToInvoice = function (id, price, warranty) {
        $scope.loading = true;
            $http({
                url: '/Admin/InvoiceManager/AddProductToInvoice',
                method: "POST",
                data: {
                    Invoice_id: InvoiceID,
                    Product_id: id,
                    Price: price,
                    Product_Warranty: warranty
                }
            }).then(function onSuccess(response) {
                // Handle success
                $scope.loading = false;
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
                $scope.loading = false;
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
            $scope.loading = true;
            $http.get("/Admin/InvoiceManager/DeleteDetailInvoice/" + $scope.DeleteDetailInvoiceId).then(function (response) {
                $scope.loading = false;
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
                $scope.loading = false;
            });
        }

        $scope.DeleteInvoice = function (id) {
            $scope.invoice_id = id;
            Swal.fire({
                title: 'Cảnh báo',
                text: 'Bạn có chắc muốn xóa ?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Có!',
                cancelButtonText: 'Không'
            }).then((result) => {
                if (result.value) {
                    $scope.DeleteInvoiceConfirm();
                } else if (result.dismiss === Swal.DismissReason.cancel) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Thất bại',
                        text: 'Xóa thất bại',
                    })
                }
            })
        }
        $scope.DeleteInvoiceConfirm = function () {
            $scope.loading = true;
            $http({
                url: '/Admin/InvoiceManager/DeleteInvoice',
                method: "POST",
                data: {
                    id: $scope.invoice_id
                },
            }).then(function onSuccess(response) {
                $scope.invoiceList = response.data;
                // Handle success
                $scope.loading = false;
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã xóa thành công',
                })
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
                console.log(response);
            });
        }
    //--------------INVOICE_END--------------------//
    //--------------INVOICE_IN_START--------------------//
    var InvoiceInID = 0;
    $scope.ReturnInvoiceIn = function () {
        $scope.loading = true;
        $http.get("/Admin/InvoiceInManager/ReturnInvoiceIn").then(function (response) {
            $scope.invoiceInList = response.data;
        });
        $scope.loading = false;
    }
    $scope.ReturnInvoiceIn();

    $scope.sortByInvoiceIn = function (Name) {
        $scope.reverseName = ($scope.Name === Name) ? !$scope.reverseName : false;
        $scope.Name = Name;
    };

    $.fn.modal.Constructor.prototype._enforceFocus = function () { };

    $scope.EditInvoiceIn = function (id) {
        InvoiceInID = id;
        $scope.loading = true;
            $http.get("/Admin/InvoiceInManager/ReturnDetailInvoiceInById/" + id).then(function (response) {
                $scope.loading = false;
                $scope.bucket.total_price = 0;
                $scope.listInvoiceInDetail = response.data;
            });
    };

    $scope.EnterInformation = function (id) {
        var Amount;
        var Price;

        swal.mixin({
            input: 'text',
            confirmButtonText: 'Next &rarr;',
            showCancelButton: true,
            progressSteps: ['1', '2']
        }).queue([
          {
              title: 'Bước 1',
              text: 'Nhập số lượng sản phẩm cần nhập',
              preConfirm: function (value) {
                  Amount = value;
              }
          },

          {
              title: 'Bước 2',
              text: 'Nhập giá tiền mua sản phẩm',
              preConfirm: function (value) {
                  Price = value;
              }
          }
        ]).then((result) => {
            if (result.value) {
                if (Amount == "" || Price == "") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Thất bại',
                        text: 'Không được để trống',
                    })
                } else {
                    $scope.AddProductToInvoiceIn(id, Price, Amount);
                }
            }
        })
    }

    $scope.AddProductToInvoiceIn = function (id, price, amount) {
        $scope.loading = true;
        amount = parseInt(amount);
        $http({
            url: '/Admin/InvoiceInManager/AddProductToInvoiceIn',
            method: "POST",
            data: {
                InOrder_id: InvoiceInID,
                Product_id: id,
                Price: price,
                Amount: amount
            }
        }).then(function onSuccess(response) {
            // Handle success
            console.log(response);
            $http.get("/Admin/InvoiceInManager/ReturnDetailInvoiceInById/" + InvoiceInID).then(function (response) {
                $scope.loading = false;
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã thêm thành công',
                })
                $scope.bucket.total_price = 0;
                $scope.ReturnInvoiceIn();
                $scope.listInvoiceInDetail = response.data;
            });
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Thêm thất bại',
            })
            console.log(response);
        });
    }

    $scope.FilterInvoiceIn = function () {
        var OrderId = document.getElementById("invoiceIn_id").value;
        var dateFrom = document.getElementById("date_from").value;
        var dateTo = document.getElementById("date_to").value;
        var InProduct = document.getElementById("in_product_search").value;
        $scope.loading = true;
        if (dateFrom > dateTo) {
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Thời gian từ không được lớn hơn Thời gian đến',
            })
        }
        else {
            $http({
                url: '/Admin/InvoiceInManager/FilterInvoiceIn',
                method: "POST",
                data: {
                    Id: OrderId,
                    DateFrom: dateFrom,
                    DateTo: dateTo,
                    Product_id: InProduct
                }
            }).then(function onSuccess(response) {
                // Handle success
                $scope.loading = false;
                $scope.invoiceInList = response.data;
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                console.log(response);
            });
        }
    }

    $scope.DeleteDetailProductIn = function (id) {
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteDetailInvoiceInConfirm(id);
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }


    $scope.DeleteDetailInvoiceInConfirm = function (id) {
        $scope.loading = true;
        $http.get("/Admin/InvoiceInManager/DeleteDetailInvoiceIn/" + id).then(function (response) {
            $scope.loading = false;
            console.log(response);
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            $http.get("/Admin/InvoiceInManage/ReturnDetailInvoiceInById/" + InvoiceInID).then(function (response) {
                $scope.bucket.total_price = 0;
                $scope.listInvoiceInDetail = response.data;
            });
            $scope.loading = false;
        });
    }

    $scope.ViewInvoiceIn = function (id, Status) {
        InvoiceInID = id;
        GetStatus = Status;
        if (Status == "Chưa hoàn thành") { document.getElementById("ConfirmInvoice").disabled = false; document.getElementById("ConfirmInvoice").style.display = "block"; }
        else { document.getElementById("ConfirmInvoice").disabled = true; document.getElementById("ConfirmInvoice").style.display = "none"; }

        $scope.loading = true;
        $http.get("/Admin/InvoiceInManager/ReturnInvoiceInById/" + id).then(function (response) {
            $scope.dataInvoice = angular.fromJson(response.data);
            $scope.InvoiceIn_Id = $scope.dataInvoice[0].Id;
            $scope.InvoiceIn_Status = $scope.dataInvoice[0].Status;
            $scope.InvoiceIn_CreateDate = $scope.dataInvoice[0].CreateDate;
            $http.get("/Admin/InvoiceInManager/ReturnDetailInvoiceInById/" + id).then(function (response) {
                $scope.loading = false;
                $scope.listInvoiceInDetail = response.data;
            });
        });
    };

    $scope.confirmIn = function () {
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xác nhận ? sau khi xác nhận bạn sẽ không thể sửa hoặc xóa hóa đơn này',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.CompletedIn();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xác nhận thất bại',
                })
            }
        })
    }

    $scope.CompletedIn = function () {
        $scope.loading = true;
        $http({
            url: '/Admin/InvoiceInManager/Confirm',
            method: "POST",
            data: {
                Id: InvoiceInID,
                Status: "Đã hoàn thành",
            },
        }).then(function onSuccess(response) {
            // Handle success
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Cập nhật thành công',
            })
            $scope.ViewInvoiceIn(InvoiceInID, "Đã Hoàn Thành");
            $scope.ReturnInvoiceIn();
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Cập nhật thất bại',
            })
            console.log(response);
        });
    }

    $scope.DeleteInvoiceIn = function (id) {
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteInvoiceInConfirm(id);
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }
    $scope.DeleteInvoiceInConfirm = function (id) {
        $scope.loading = true;
        $http({
            url: '/Admin/InvoiceInManager/DeleteInvoiceIn',
            method: "POST",
            data: {
                id: id
            },
        }).then(function onSuccess(response) {
            $scope.invoiceInList = response.data;
            // Handle success
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Xóa thất bại',
            })
            console.log(response);
        });
    }

    //--------------INVOICE_IN_END--------------------//
    //--------------INVOICE OUT_START--------------------//
    $scope.AddProductToInvoiceDetail = function (id)
    {
        $scope.loading = true;
        $http.get("/Admin/InvoiceOutManager/AddProductToInvoiceOut/" + id).then(function (response) {
            $scope.ReturnInvoiceoOut();
            $scope.loading = false;
        });
    }
    $scope.bucket={total_price_invoice : 0};
    $scope.ReturnInvoiceoOut = function () {
        $scope.bucket.total_price_invoice = 0;
        $scope.loading = true;
        $http.get("/Admin/InvoiceOutManager/ReturnInvoiceDetailSession").then(function (response) {
            $scope.lstInvoiceDetailss = response.data;
            $scope.loading = false;
        });
    }
    $scope.ReturnInvoiceoOut();
    $scope.RemoveItem = function (id) {
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.loading = true;
                $http.get("/Admin/InvoiceOutManager/RemoveItem/" + id).then(function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Thành công',
                        text: 'Xóa thành công',
                    })
                    $scope.ReturnInvoiceoOut();
                    $scope.loading = false;
                });
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }

    $scope.DeleteInvoiceOutChecked = function () {
        Swal.fire({
            title: 'Cảnh báo',
            text: 'Bạn có chắc muốn xóa ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có!',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.value) {
                $scope.DeleteInvoiceOutCheckedConfirm();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Xóa thất bại',
                })
            }
        })
    }
    $scope.DeleteInvoiceOutCheckedConfirm = function () {
        CheckAll = document.getElementById("check-all");
        table = document.getElementById("items-table");
        ItemCheckBox = table.getElementsByTagName("input");
        var lstId = [];
        if (CheckAll.checked == true) {
            for (i = 0; ItemCheckBox.length > i; i++) {
                if (ItemCheckBox[i].type == 'checkbox' && ItemCheckBox[i].checked == true)
                    lstId.push(ItemCheckBox[i].value);
            }
        }
        else {
            for (i = 0; ItemCheckBox.length > i; i++) {
                if (ItemCheckBox[i].type == 'checkbox' && ItemCheckBox[i].checked == true)
                    lstId.push(ItemCheckBox[i].value);
            }
        }
        $scope.loading = true;
        $http({
            url: '/Admin/InvoiceOutManager/DeleteInvoiceOutChecked',
            method: "POST",
            data: {
                id: lstId
            },
        }).then(function onSuccess(response) {
            $scope.ReturnInvoiceoOut();
            // Handle success
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            console.log(response);
        }).catch(function onError(response) {
            // Handle error
            $scope.loading = false;
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Xóa thất bại',
            })
            console.log(response);
        });
    }
    $scope.SubmitInvoiceOut = function () {
        var customerName = document.getElementById("customer_name").value;
        var address = document.getElementById("customer_address").value;
        var phone = document.getElementById("customer_phone").value;
        if ($scope.lstInvoiceDetailss == "" || $scope.lstInvoiceDetailss == null || $scope.lstInvoiceDetailss=="{}") {
            Swal.fire({
                icon: 'error',
                title: 'Xuất hóa đơn thất bại',
                text: 'Bạn chưa thêm sản phẩm',
            });
        }
        else if (customerName == "" || address == "" || phone=="")
        {
            Swal.fire({
                icon: 'error',
                title: 'Xuất hóa đơn thất bại',
                text: 'Bạn chưa thêm thông tin khách hàng',
            });
        }
        else {
            $scope.loading = true;
            $http({
                url: '/Admin/InvoiceOutManager/SubmitInvoiceOut',
                method: "POST",
                data: {
                    CustomerName: customerName,
                    Address: address,
                    NumberPhone: phone
                },
            }).then(function onSuccess(response) {
                // Handle success
                $scope.ReturnInvoiceoOut();
                document.getElementById("PrintToPdf").click();
                $scope.loading = false;
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                console.log(response);
            });
        }
    }
    //--------------INVOICE OUT_END--------------------//

    //----------------SUMMARY_START--------------------//
    $scope.ReturnTurnover=function()
    {
        $scope.loading = true;
        $http.get("/Admin/SummaryManager/ReturnTurnover").then(function (response) {
            $scope.temp = angular.fromJson(response.data);
            $scope.turnOver = $scope.temp.totalTurnover;
            $scope.loading = false;
        });
    }
    $scope.ReturnCustomerAmount = function () {
        $scope.loading = true;
        $http.get("/Admin/SummaryManager/ReturnCustomerAmount").then(function (response) {
            $scope.temp = angular.fromJson(response.data);
            $scope.UserAmount = $scope.temp.totalUser;
            $scope.loading = false;
        });
    }
    $scope.ReturnTotalProductSell = function () {
        $scope.loading = true;
        $http.get("/Admin/SummaryManager/ReturnTotalProductSell").then(function (response) {
            $scope.temp = angular.fromJson(response.data);
            $scope.totalProductSell = $scope.temp.totalProductSell;
            $scope.loading = false;
        });
    }
    $scope.ReturnBestSeller = function () {
        $scope.loading = true;
        $http.get("/Admin/SummaryManager/ReturnBestSeller").then(function (response) {
            $scope.lstBestSeller = response.data;
            $scope.loading = false;
        });
    }
    $scope.ReturnNewDeal = function () {
        $scope.loading = true;
        $http.get("/Admin/SummaryManager/ReturnNewDeal").then(function (response) {
            $scope.lstNewDeal = response.data;
            $scope.loading = false;
        });
    }
    $scope.ReturnTurnoverChart = function () {
        $scope.loading = true;
        $http.get("/Admin/SummaryManager/ReturnTurnoverChart").then(function (response) {
            $scope.TurnoverChart = response.data;
            var chartData = [];
            chartData = angular.fromJson(response.data);
            var ctx = document.getElementById("myChart");
            ctx.style = "-webkit-tap-highlight-color: transparent;";
            Chart.defaults.global.defaultFontFamily = 'Oswald';
            Chart.defaults.global.defaultFontSize = 14;
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ["T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12"],
                    datasets: [{
                        label: 'Triệu VNĐ',
                        data: chartData,
                        backgroundColor: [
						'rgba(30, 144, 255, 0.5)',
						'rgba(30, 144, 255, 0.5)',
						'rgba(30, 144, 255, 0.5)',
						'rgba(30, 144, 255, 0.5)',
						'rgba(30, 144, 255, 0.5)',
						'rgba(30, 144, 255, 0.5)'
                        ],
                        borderColor: [
						'rgba(30, 144, 255,1)',
						'rgba(30, 144, 255, 1)',
						'rgba(30, 144, 255, 1)',
						'rgba(30, 144, 255, 1)',
						'rgba(30, 144, 255, 1)',
						'rgba(30, 144, 255, 1)'
                        ],
                        borderWidth: 2,
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,

                            }
                        }]
                    }
                }
            });
            document.getElementById("admin-table-best-sale").style.height = document.getElementById("myChart").style.height;
            $scope.loading = false;
        });
    }
    $scope.sortByBestSeller = function (propertyName) {
        $scope.reverse = (propertyName !== null && $scope.propertyName === propertyName)
        ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
        if ($scope.reverse == false) {
            $scope.lstBestSeller = $filter('orderBy')($scope.lstBestSeller, propertyName);
        }
        else {
            $scope.lstBestSeller = $filter('orderBy')($scope.lstBestSeller, '-' + propertyName);
        }
    }
    $scope.sortByNewDeal = function (propertyName) {
        $scope.reverse = (propertyName !== null && $scope.propertyName === propertyName)
        ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
        if ($scope.reverse == false) {
            $scope.lstNewDeal = $filter('orderBy')($scope.lstNewDeal, propertyName);
        }
        else {
            $scope.lstNewDeal = $filter('orderBy')($scope.lstNewDeal, '-' + propertyName);
        }
    }
    $scope.ReturnTurnoverChart();
    $scope.ReturnTurnover();
    $scope.ReturnCustomerAmount();
    $scope.ReturnTotalProductSell();
    $scope.ReturnBestSeller();
    $scope.ReturnNewDeal();
    //----------------SUMMARY_END--------------------//
});