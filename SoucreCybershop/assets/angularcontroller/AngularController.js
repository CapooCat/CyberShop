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
app.controller('MyController', function ($scope, $http, $window, $q) {
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

    $scope.Reload = function () {
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
                    while ((Length - 12) > 0) {
                        Length = Length - 12;
                        PageNum = PageNum + 1;
                        $scope.paginate.push(PageNum);
                    }
                    Length = Object.keys(res.data).length;
                    var BeginPageIndex = 0;
                    var EndPageIndex = 0;
                    for (i = 0; i <= Page; i++) {
                        if (EndPageIndex < Length) {
                            if (BeginPageIndex == EndPageIndex)
                                EndPageIndex = EndPageIndex + 12;
                            else {
                                BeginPageIndex = BeginPageIndex + 12;
                                EndPageIndex = EndPageIndex + 12;
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
                        while ((Length - 12) > 0) {
                            Length = Length - 12;
                            PageNum = PageNum + 1;
                            $scope.paginate.push(PageNum);
                        }
                        Length = Object.keys(response.data).length;
                        var BeginPageIndex = 0;
                        var EndPageIndex = 0;
                        for (i = 0; i <= Page; i++) {
                            if (EndPageIndex < Length) {
                                if (BeginPageIndex == EndPageIndex)
                                    EndPageIndex = EndPageIndex + 12;
                                else {
                                    BeginPageIndex = BeginPageIndex + 12;
                                    EndPageIndex = EndPageIndex + 12;
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
        console.log(BuyNow);
        var url = "/Pay";
        $http.get("/Cart/ReturnCartItem").then(function (response) {
            $scope.cartList = response.data;
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

