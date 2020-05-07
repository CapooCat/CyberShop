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
    var Page = 0;
    var SortType = 1;
    $a = window.location.pathname;
    $scope.$watch('$viewContentLoaded', function () {
        $scope.Reload();
    });
    

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
            });
        }
        else if ($a.includes("/danh-muc/") == true || $a.includes("/chi-tiet-danh-muc/") == true || $a.includes("/san-pham/") == true) {
            $scope.loading = true;
            $http.get(window.location.pathname + "/JSON")
                .then(function (response) {
                    $scope.loading = false;
                    $scope.datalist = response.data;

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
                });
        }
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


});

