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

});

