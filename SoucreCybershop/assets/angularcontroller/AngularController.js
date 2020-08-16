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
app.controller('MyController', function ($scope, $http, $window, $q, $sce) {
    var Page = 0;
    var SortType = 1;
    var BuyNow = false;

    $scope.Domain = window.location.hostname;

    $a = window.location.pathname;
    $scope.$watch('$viewContentLoaded', function () {
        $scope.Reload();
    });

    var typingTimer;
    var doneTypingInterval = 1000;
    var myInput = document.getElementById('Search');

    myInput.addEventListener('keyup', () => {
        clearTimeout(typingTimer);
        if (myInput.value) {
              $scope.SResult = null;
              typingTimer = setTimeout(doneTyping, doneTypingInterval);
              document.getElementsByClassName("dropdown-loading")[0].style.visibility = "visible";
        } else {
              $scope.SResult = null;
              document.getElementsByClassName("dropdown-loading")[0].style.visibility = "hidden";
              task();
        }
    });

    function doneTyping() {
        $scope.SResult = null;
        if ($scope.myValue == "") {
            document.getElementsByClassName("dropdown-search")[0].style.visibility = "hidden";
            document.getElementsByClassName("dropdown-loading")[0].style.visibility = "hidden";
        } else {
            $http.get("/TimKiem/" + $scope.myValue + "/JSON").then(function (res) {
                var Input = document.getElementById("Search");
                document.getElementsByClassName("dropdown-loading")[0].style.visibility = "hidden";
                if ($(Input).is(':focus')) {
                    document.getElementsByClassName("dropdown-search")[0].style.visibility = "visible";
                }
                $scope.SResult = res.data;
            });
        }
    }
    

    $scope.Back = function() {
        var Number = document.getElementsByClassName("NumberPage");
        for (i = 0; i < Number.length; i++)
        {
            Number[i].className = "NumberPage";
        }
        if(Page <= 0)
            Number[Page].className += " active";
        else {
            Page = Page - 1;
            Number[Page].className += " active";
        }
    }

    $scope.Next = function () {
        var Number = document.getElementsByClassName("NumberPage");
        for (i = 0; i < Number.length; i++) {
            Number[i].className = "NumberPage";
        }
        if (Page >= Number.length-1)
            Number[Page].className += " active";
        else {
            Page = Page + 1;
            Number[Page].className += " active";
        }
    }

    $scope.Change = function (n) {
        var Number = document.getElementsByClassName("NumberPage");
        for (i = 0; i < Number.length; i++) {
            Number[i].className = "NumberPage";
        }
        Page = n-1;
        Number[n-1].className += " active";
    }

    $scope.LoadItems = function (Tid, Pid) {
        $scope.loading = true;
        $http.get("/Home/LoadItems/" + Tid + "/" + Pid).then(function (response) {
            $scope.ProductSame = response.data;
            $scope.loading = false;
        });
    }
    $scope.ItemSelectedId = 0;
    $scope.OpenItemDoc = function (id) {
        $scope.loading = true;
        $http.get("/Home/OpenItemDoc/" + id).then(function (response) {
            $scope.temp = angular.fromJson(response.data);
            $scope.ItemSelectedImage = $scope.temp[0].Image;
            $scope.ItemSelectedPrice = $scope.temp[0].Price;
            $scope.ItemSelectedName = $scope.temp[0].ProductName;
            $scope.ItemSelectedId = $scope.temp[0].id;
            $scope.currentProjectUrl = $sce.trustAsResourceUrl($scope.temp[0].Info + "?embedded=true");
            $scope.loading = false;
            document.getElementById("modal-side-by-side").style.visibility = "visible";
        });
    }

    $scope.Xem = function () {
        $window.location.href = '/Home/DetailProduct/' + $scope.ItemSelectedId;
    }


    $scope.Reload = function () {
        var Amount = 0;
        if(Number(document.getElementById("item_amount") != null))
            Amount = Number(document.getElementById("item_amount").value);
        if ($a.includes("/Product") != false) {
            $scope.loading = true;
            $http.get("/Product/SanPham").then(function (res) {
                $scope.loading = false;
                if ((res.data).length == 0) {
                    document.getElementsByClassName("empty-alert")[0].style.display = "block";
                }
                else {

                    if (SortType == 1)
                        (res.data).sort((a, b) => (a.Price - b.Price));
                    if (SortType == 2)
                        (res.data).sort((a, b) => (b.Price - a.Price));
                    if (SortType == 3)
                        (res.data).sort((a, b) => a.ProductName !== b.ProductName ? a.ProductName < b.ProductName ? -1 : 1 : 0);
                    if (SortType == 4)
                        (res.data).sort((a, b) => a.ProductName !== b.ProductName ? a.ProductName > b.ProductName ? -1 : 1 : 0);
                    if (SortType == 5)
                        (res.data).sort(NewToOld);
                    if (SortType == 6)
                        (res.data).sort(OldToNew);

                    var Length = Object.keys(res.data).length;
                    $scope.paginate = [];
                    $scope.data = [];
                    var PageNum = 1;
                    while ((Length - Amount) > 0) {
                        Length = Length - Amount;
                        PageNum = PageNum + 1;
                        $scope.paginate.push(PageNum);
                    }
                    Length = Object.keys(res.data).length;
                    var BeginPageIndex = 0;
                    var EndPageIndex = 0;
                    for (i = 0; i <= Page; i++) {
                        if (EndPageIndex < Length) {
                            if (BeginPageIndex == EndPageIndex)
                                EndPageIndex = EndPageIndex + Amount;
                            else {
                                BeginPageIndex = BeginPageIndex + Amount;
                                EndPageIndex = EndPageIndex + Amount;
                            }
                        }
                        if (EndPageIndex >= Length) {
                            EndPageIndex = Length;
                        }
                    }
                    while (BeginPageIndex < EndPageIndex) {
                        $scope.data.push(res.data[BeginPageIndex])
                        BeginPageIndex = BeginPageIndex + 1;
                    }
                    window.scroll({
                        top: 0,
                        behavior: 'auto'
                    })
                }
            });
        }
        else if ($a.includes("/danh-muc/") == true || $a.includes("/chi-tiet-danh-muc/") == true || $a.includes("/san-pham/") == true || $a.includes("/TimKiem/") == true) {
            $scope.loading = true;
            $http.get(window.location.pathname + "/JSON")
                .then(function (response) {
                    $scope.loading = false;
                    if ((response.data).length == 0) {
                        document.getElementsByClassName("empty-alert")[0].style.display = "block";
                    }

                    else {
                        if (SortType == 1)
                            (response.data).sort((a, b) => (a.Price - b.Price));
                        if (SortType == 2)
                            (response.data).sort((a, b) => (b.Price - a.Price));
                        if (SortType == 3)
                            (response.data).sort((a, b) => a.ProductName !== b.ProductName ? a.ProductName < b.ProductName ? -1 : 1 : 0);
                        if (SortType == 4)
                            (response.data).sort((a, b) => a.ProductName !== b.ProductName ? a.ProductName > b.ProductName ? -1 : 1 : 0);
                        if (SortType == 5)
                            (response.data).sort(NewToOld);
                        if (SortType == 6)
                            (response.data).sort(OldToNew);

                        var Length = Object.keys(response.data).length;
                        $scope.datalist = [];
                        $scope.paginate = [];
                        var PageNum = 1;
                        while ((Length - Amount) > 0) {
                            Length = Length - Amount;
                            PageNum = PageNum + 1;
                            $scope.paginate.push(PageNum);
                        }
                        Length = Object.keys(response.data).length;
                        var BeginPageIndex = 0;
                        var EndPageIndex = 0;
                        for (i = 0; i <= Page; i++) {
                            if (EndPageIndex < Length) {
                                if (BeginPageIndex == EndPageIndex)
                                    EndPageIndex = EndPageIndex + Amount;
                                else {
                                    BeginPageIndex = BeginPageIndex + Amount;
                                    EndPageIndex = EndPageIndex + Amount;
                                }
                            }
                            if (EndPageIndex >= Length) {
                                EndPageIndex = Length;
                            }
                        }
                        while (BeginPageIndex < EndPageIndex) {
                            $scope.datalist.push(response.data[BeginPageIndex])
                            BeginPageIndex = BeginPageIndex + 1;
                        }
                        window.scroll({
                            top: 0,
                            behavior: 'auto'
                        })
                    }
                });
        }
    }

    $scope.GoToPay = function() {
        BuyNow = true;
    }

    function NewToOld(a, b) {
        var dateA = new Date(a.CreateDatee).getTime();
        var dateB = new Date(b.CreateDate).getTime();
        return dateA > dateB ? 1 : -1;
    };

    function OldToNew(a, b) {
        var dateA = new Date(a.CreateDatee).getTime();
        var dateB = new Date(b.CreateDate).getTime();
        return dateA < dateB ? -1 : 1;
    };

    $scope.SortBy = function (n) {
            SortType = n;           
    }
    
    $scope.FetchCart = function () {
        var Items = document.getElementById("cart-layout");
        var Element = document.getElementById("cart-content");
        var url = "/Pay";
        $http.get("/Cart/ReturnCartItem").then(function (response) {
            $scope.cartList = response.data;
            $http.get("/Cart/ReturnCartItem").then(function (response) {
                if (Items.offsetHeight > 500)
                {
                    Element.style.height = "500px";
                    Element.style.overflowY = "auto";
                }
                else {
                    Element.style.height = "auto";
                }
            });
            if (BuyNow == true) {
                $window.location.href = url;
            } else {
                $scope.loading = false;
            }
        });
    }
    $scope.bucket = { total_amount: 0 };
    $scope.FetchCart();
    $scope.AddItem = function (id) {
        
        $scope.loading = true;
        $http.get("/Cart/AddItem/" + id).then(function (response) {
            $scope.bucket.total_amount = 0;
            $scope.FetchCart();
        });
    }

    $scope.RemoveItem = function (id) {
        $scope.loading = true;
        $http.get("/Cart/RemoveItem/" + id).then(function (response) {
            $scope.bucket.total_amount = 0;
            $scope.FetchCart();
        });
    }
    $scope.MiniusItem = function (id) {
        $scope.loading = true;
        $http.get("/Cart/MiniusItem/" + id).then(function (response) {
            $scope.bucket.total_amount = 0;
            $scope.FetchCart();
        });
    }

    $scope.redirect = function () {
        if ($scope.myValue.replace(/\s/g, '').length != 0)
        {
            var url = "/TimKiem/" + $scope.myValue
            $window.location.href = url;
        }
    }
    //Lấy list Catgory
    $scope.getCatList = function () {
        $http.get("/Admin/CategoryManager/ReturnCategory").then(function (response) {
            $scope.catList = response.data;
        });
    }

    $scope.SelectPcPart= function (id)
    {
        $scope.loading = true;
        $http.get("/Build/ReturnPCPart/" + id).then(function (response) {
            $scope.temp = angular.fromJson(response.data);
            $scope.PcPartLst = response.data;
            $scope.TypeName = $scope.temp[0].TypeName;
            $scope.loading = false;
        });
    }
    $scope.TotalPrice=0;
    var lstIdPcPart = [];
    $scope.AddToBuildPC = function (id) {
        $scope.loading = true;
        var mainSelect = document.getElementById("mainSelect");
        var mainItem = document.getElementById("mainItem");

        var cpuSelect = document.getElementById("cpuSelect");
        var cpuItem = document.getElementById("cpuItem");

        var ramSelect = document.getElementById("ramSelect");
        var ramItem = document.getElementById("ramItem");

        var ssdSelect = document.getElementById("ssdSelect");
        var ssdItem = document.getElementById("ssdItem");

        var hddSelect = document.getElementById("hddSelect");
        var hddItem = document.getElementById("hddItem");

        var psuSelect = document.getElementById("psuSelect");
        var psuItem = document.getElementById("psuItem");

        var vgaSelect = document.getElementById("vgaSelect");
        var vgaItem = document.getElementById("vgaItem");

        var caseSelect = document.getElementById("caseSelect");
        var caseItem = document.getElementById("caseItem");

        var monitorSelect = document.getElementById("monitorSelect");
        var monitorItem = document.getElementById("monitorItem");

        var coolSelect = document.getElementById("coolSelect");
        var coolItem = document.getElementById("coolItem");
        $http.get("/Build/ReturnPcItem/" + id).then(function (response) {
            $scope.temp = angular.fromJson(response.data);
            var ProductId = $scope.temp[0].id;
            lstIdPcPart.push(ProductId);
            $scope.TotalPrice = $scope.TotalPrice + $scope.temp[0].Price;
            $scope.ProductTypeName = $scope.temp[0].TypeName;
            if ($scope.ProductTypeName == "Main")
            {
                $scope.MainId = $scope.temp[0].id;
                $scope.MainName=$scope.temp[0].ProductName;
                $scope.MainPrice = $scope.temp[0].Price;
                $scope.MainImage = $scope.temp[0].Image;
                mainSelect.hidden = true;
                mainItem.hidden = false;
            }
            else if ($scope.ProductTypeName == "CPU")
            {
                $scope.CPUId = $scope.temp[0].id;
                $scope.CPUName = $scope.temp[0].ProductName;
                $scope.CPUPrice = $scope.temp[0].Price;
                $scope.CPUImage = $scope.temp[0].Image;
                cpuSelect.hidden = true;
                cpuItem.hidden = false;
            }
            else if ($scope.ProductTypeName == "RAM") {
                $scope.RamId = $scope.temp[0].id;
                $scope.RamName = $scope.temp[0].ProductName;
                $scope.RamPrice = $scope.temp[0].Price;
                $scope.RamImage = $scope.temp[0].Image;
                ramSelect.hidden = true;
                ramItem.hidden = false;
            }
            else if ($scope.ProductTypeName == "SSD") {
                $scope.SSDId = $scope.temp[0].id;
                $scope.SSDName = $scope.temp[0].ProductName;
                $scope.SSDPrice = $scope.temp[0].Price;
                $scope.SSDImage = $scope.temp[0].Image;
                ssdSelect.hidden = true;
                ssdItem.hidden = false;
            }
            else if ($scope.ProductTypeName == "HDD") {
                $scope.HDDId = $scope.temp[0].id;
                $scope.HDDName = $scope.temp[0].ProductName;
                $scope.HDDPrice = $scope.temp[0].Price;
                $scope.HDDImage = $scope.temp[0].Image;
                hddSelect.hidden = true;
                hddItem.hidden = false;
            }
            else if ($scope.ProductTypeName == "PSU") {
                $scope.PSUId = $scope.temp[0].id;
                $scope.PSUName = $scope.temp[0].ProductName;
                $scope.PSUPrice = $scope.temp[0].Price;
                $scope.PSUImage = $scope.temp[0].Image;
                psuSelect.hidden = true;
                psuItem.hidden = false;
            }
            else if ($scope.ProductTypeName == "Card màn hình") {
                $scope.VGAId = $scope.temp[0].id;
                $scope.VGAName = $scope.temp[0].ProductName;
                $scope.VGAPrice = $scope.temp[0].Price;
                $scope.VGAImage = $scope.temp[0].Image;
                vgaSelect.hidden = true;
                vgaItem.hidden = false;
            }
            else if ($scope.ProductTypeName == "Case") {
                $scope.CaseId = $scope.temp[0].id;
                $scope.CaseName = $scope.temp[0].ProductName;
                $scope.CasePrice = $scope.temp[0].Price;
                $scope.CaseImage = $scope.temp[0].Image;
                caseSelect.hidden = true;
                caseItem.hidden = false;
            }
            else if ($scope.ProductTypeName == "Màn hình") {
                $scope.MonitorId = $scope.temp[0].id;
                $scope.MonitorName = $scope.temp[0].ProductName;
                $scope.MonitorPrice = $scope.temp[0].Price;
                $scope.MonitorImage = $scope.temp[0].Image;
                monitorSelect.hidden = true;
                monitorItem.hidden = false;
            }
            else if ($scope.ProductTypeName == "Tản nhiệt") {
                $scope.CoolerId = $scope.temp[0].id;
                $scope.CoolerName = $scope.temp[0].ProductName;
                $scope.CoolerPrice = $scope.temp[0].Price;
                $scope.CoolerImage = $scope.temp[0].Image;
                coolSelect.hidden = true;
                coolItem.hidden = false;
            }
            $scope.loading = false;
        });
    }
    $scope.DeletePcPart = function (id) {
        $scope.loading = true;
        var mainSelect = document.getElementById("mainSelect");
        var mainItem = document.getElementById("mainItem");

        var cpuSelect = document.getElementById("cpuSelect");
        var cpuItem = document.getElementById("cpuItem");

        var ramSelect = document.getElementById("ramSelect");
        var ramItem = document.getElementById("ramItem");

        var ssdSelect = document.getElementById("ssdSelect");
        var ssdItem = document.getElementById("ssdItem");

        var hddSelect = document.getElementById("hddSelect");
        var hddItem = document.getElementById("hddItem");

        var psuSelect = document.getElementById("psuSelect");
        var psuItem = document.getElementById("psuItem");

        var vgaSelect = document.getElementById("vgaSelect");
        var vgaItem = document.getElementById("vgaItem");

        var caseSelect = document.getElementById("caseSelect");
        var caseItem = document.getElementById("caseItem");

        var monitorSelect = document.getElementById("monitorSelect");
        var monitorItem = document.getElementById("monitorItem");

        var coolSelect = document.getElementById("coolSelect");
        var coolItem = document.getElementById("coolItem");
        $http.get("/Build/ReturnPcItem/" + id).then(function (response) {
            $scope.temp = angular.fromJson(response.data);
            $scope.TotalPrice= $scope.TotalPrice-$scope.temp[0].Price;
            for(var i=0;i<lstIdPcPart.length;i++)
            {
                if(lstIdPcPart[i]==id)
                {
                    lstIdPcPart.splice(i,1);
                }
            }
            $scope.ProductTypeName = $scope.temp[0].TypeName;
            if ($scope.ProductTypeName == "Main")
            {
                mainSelect.hidden = false;
                mainItem.hidden = true;
            }
            else if ($scope.ProductTypeName == "CPU")
            {
                cpuSelect.hidden = false;
                cpuItem.hidden = true;
            }
            else if ($scope.ProductTypeName == "RAM") {
                ramSelect.hidden = false;
                ramItem.hidden = true;
            }
            else if ($scope.ProductTypeName == "SSD") {
                ssdSelect.hidden = false;
                ssdItem.hidden = true;
            }
            else if ($scope.ProductTypeName == "HDD") {
                hddSelect.hidden = false;
                hddItem.hidden = true;
            }
            else if ($scope.ProductTypeName == "PSU") {
                psuSelect.hidden = false;
                psuItem.hidden = true;
            }
            else if ($scope.ProductTypeName == "Card màn hình") {
                vgaSelect.hidden = false;
                vgaItem.hidden = true;
            }
            else if ($scope.ProductTypeName == "Case") {
                caseSelect.hidden = false;
                caseItem.hidden = true;
            }
            else if ($scope.ProductTypeName == "Màn hình") {
                monitorSelect.hidden = false;
                monitorItem.hidden = true;
            }
            else if ($scope.ProductTypeName == "Tản nhiệt") {
                coolSelect.hidden = false;
                coolItem.hidden = true;
            }
            $scope.loading = false;
        });
    }
    $scope.ResetPcPart = function(){
        document.getElementById("rsPage").click();
    }
    $scope.AddAllToCart = function () {
        $scope.loading = true;
            $http({
                url: '/Build/AddAllToCart',
                method: "POST",
                data: {
                    lstId: lstIdPcPart
                }
            }).then(function onSuccess(response) {
                // Handle success
                $scope.FetchCart();
                $scope.loading = false;
                console.log(response);
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                console.log(response);
            });
    }
    $scope.FindPassword = function () {
        var email = document.getElementById("forgot_password").value;
        $http.get("/Home/CheckEmail?email=" + email).then(function (response) {
            $scope.temp = angular.fromJson(response.data);
            var success = $scope.temp.success;
            if(success =="true")
            {
                Swal.fire({
                    icon: 'success',
                    title: 'Gửi mail thành công',
                    text: 'Vui lòng kiểm tra email của mình',
                })
                $http.get("/Home/SendEmail?email=" + email).then(function (response) {
                });
                console.log(response);
            }
            else
            {
                Swal.fire({
                    icon: 'error',
                    title: 'Email không đúng',
                    text: 'Vui lòng nhập lại email',
                })
                console.log(response);
            }
        });
    }
    //$scope.discount_price = 0;
    //var btn_confirm = document.getElementById("ConfirmDiscount");
    //if (btn_confirm != null) {
    //    btn_confirm.addEventListener('click', (e) => {
    //        e.preventDefault();
    //        $scope.temp = "";
    //        var coupon_value = document.getElementById("input_cpn").value;
    //        $http.get("/Pay/ConfirmCoupon/" + coupon_value).then(function (response) {
    //            $scope.temp = angular.fromJson(response.data);
    //            if ($scope.temp.success == "true") {
    //                document.getElementById("tickmark").hidden = false;
    //                btn_confirm.hidden = true;
    //                document.getElementById("error_cpn").hidden = true;
    //                document.getElementById("input_cpn").readOnly = true;
    //                $scope.discount_price = -($scope.temp.price_discount);
    //                $scope.bucket.total_amount = $scope.bucket.total_amount + $scope.discount_price;
    //            }
    //            else {
    //                document.getElementById("error_cpn").hidden = false;
    //            }
    //        });

    //    });
    //}
    //else { }

    //-------------------USER------------------------//
    $scope.RequestChangePass = function (username) {
        if (document.getElementById("old_pass").value == "") {
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Không được để trống',
            })
        } else {
            var Password = document.getElementById("old_pass").value;
            $scope.loading = true;
            $http({
                url: '/Home/CheckPassword',
                method: "POST",
                data: {
                    Password: Password,
                    Username: username
                },
            }).then(function onSuccess(response) {
                // Handle success
                if (response.data.success == false) {
                    $scope.loading = false;
                    Swal.fire({
                        icon: 'error',
                        title: 'Thất bại',
                        text: 'Mật khẩu không đúng',
                    })
                } else {
                    $scope.loading = false;
                    document.getElementById("new_pass").disabled = false;
                    document.getElementById("re_enter_pass").disabled = false;
                    document.getElementById("Confirm_change").className = "";
                    document.getElementById("Confirm_change").disabled = false;
                    console.log(response);
                }
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Đã xảy ra lỗi',
                })
                console.log(response);
            });
        }
    }

    $scope.ChangePass = function (username) {
        if (document.getElementById("new_pass").value == "" && document.getElementById("re_enter_pass").value == "") {
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Không được để trống',
            })
        } else if (document.getElementById("new_pass").value != document.getElementById("re_enter_pass").value) {
            Swal.fire({
                icon: 'error',
                title: 'Thất bại',
                text: 'Nhập lại mật khẩu không chính xác',
            })
        } else {
            var newPassword = document.getElementById("new_pass").value;
            $scope.loading = true;
            $http({
                url: '/Home/ChangePassword',
                method: "POST",
                data: {
                    Password: newPassword,
                    Username: username
                },
            }).then(function onSuccess(response) {
                // Handle success
                $scope.loading = false;
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Thay đổi mật khẩu thành công',
                })
            }).catch(function onError(response) {
                // Handle error
                $scope.loading = false;
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Đã xảy ra lỗi',
                })
                console.log(response);
            });
        }
    }
    $scope.bucket = { total_price: 0 };
    $scope.EditInvoice = function (id) {
        InvoiceID = id;
        $scope.bucket.total_price = 0;
        $scope.loading = true;
        $http.get("/User/ReturnInvoiceById/" + id).then(function (response) {
            $scope.dataInvoice = angular.fromJson(response.data);
            $scope.clientName = $scope.dataInvoice[0].CustomerName;
            $scope.phoneNumber = $scope.dataInvoice[0].DeliveryPhoneNum;
            $scope.address = $scope.dataInvoice[0].DeliveryAddress;
            $http.get("/User/ReturnInvoiceDetailById/" + id).then(function (response) {
                $scope.listInvoiceDetail = response.data;
                $scope.loading = false;
            });
        });
    }
    $scope.ReturnProduct = function () {
        $http.get("/Admin/ProductManager/ReturnProduct").then(function (response) {
            $scope.productList = response.data;
        });
    }
    $scope.sortProduct = function (ProductN) {
        $scope.reverseProduct = ($scope.ProductN === ProductN) ? !$scope.reverseProduct : false;
        $scope.ProductN = ProductN;
    };
    $scope.sortInvoiceProduct = function (InProduct) {
        $scope.reverseInProduct = ($scope.InProduct === InProduct) ? !$scope.reverseInProduct : false;
        $scope.InProduct = InProduct;
    };
    $scope.ReturnOrderHistory = function () {
        $http.get("/User/ReturnOrderHistory").then(function (response) {
            $scope.lstOrderHistory = response.data;
        });
    }
    $scope.UpdateInvoice = function () {
        $scope.loading = true;
        var Name = document.getElementById("client_name").value;
        var SDT = document.getElementById("phone_number").value;
        var Address = document.getElementById("address").value;
        table = document.getElementById("items-table");
        ItemInput = table.getElementsByTagName("input");
        if (document.getElementById("btn_close").hasAttribute("data-dismiss") && document.getElementById("btn_out").hasAttribute("data-dismiss")) {
            for (var i = 0; i < ItemInput.length; i++)
            {
                var id = ItemInput[i].getAttribute("id");
                id = id.replace("AmountInput_", "");
                id = parseInt(id);
                var amount = ItemInput[i].value;
                $http({
                    url: '/User/UpdateAmountInvoiceDetail',
                    method: "POST",
                    data: {
                        id: id,
                        Amount: amount
                    },
                }).then(function onSuccess(response) {
                    // Handle success
                    
                }).catch(function onError(response) {
                    // Handle error
                    console.log(response);
                });
            }
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
                $scope.loading = false;
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Cập nhật thành công',
                })
                $("[data-dismiss=modal]").trigger({ type: "click" });
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
        else {
            Swal.fire({
                icon: 'error',
                title: 'Không được để trống',
                text: 'Bạn chưa nhập số lượng sản phẩm',
            })
        }
        
    }
    $scope.ViewInvoice = function (id) {
        $scope.bucket.total_price = 0;
        $scope.invoice_id = id;
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
                $scope.listInvoiceDetail2 = response.data;
            });
        });
    };
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
    $scope.ReturnOrderHistory();
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
            url: '/User/DeleteInvoice',
            method: "POST",
            data: {
                id: $scope.invoice_id
            },
        }).then(function onSuccess(response) {
            // Handle success
            $scope.loading = false;
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: 'Đã xóa thành công',
            })
            $scope.ReturnOrderHistory();
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
    $scope.ChangeAmount = function (id) {
        table = document.getElementById("items-table");
        ItemInput = table.getElementsByTagName("input");
        $scope.loading = true;
        var inputNull = 0;
        for (var i = 0; ItemInput.length > i; i++) {
            if (ItemInput[i].value == null || ItemInput[i].value == "") {
                inputNull += 1;
                if(document.getElementById("btn_close").hasAttribute("data-dismiss") && document.getElementById("btn_out").hasAttribute("data-dismiss"))
                {
                    document.getElementById("btn_close").removeAttribute("data-dismiss");
                    document.getElementById("btn_out").removeAttribute("data-dismiss");
                    break;
                }
                else {
                    break;
                }
            }
        }
        if (inputNull == 0)
        {
            if(document.getElementById("btn_close").hasAttribute("data-dismiss") && document.getElementById("btn_out").hasAttribute("data-dismiss"))
            {
            }
            else {
                document.getElementById("btn_close").setAttribute("data-dismiss","modal");
                document.getElementById("btn_out").setAttribute("data-dismiss","modal");
            }
        }
        $scope.loading = false;
    }
    
    $scope.CheckAmountNull = function () {
        table = document.getElementById("items-table");
        ItemInput = table.getElementsByTagName("input");
        if (document.getElementById("btn_close").hasAttribute("data-dismiss") && document.getElementById("btn_out").hasAttribute("data-dismiss")) {
            document.getElementById("btn_close").click();
        }
        else {
            Swal.fire({
                icon: 'error',
                title: 'Không được để trống',
                text: 'Bạn chưa nhập số lượng sản phẩm',
            })
        }
    }
});

