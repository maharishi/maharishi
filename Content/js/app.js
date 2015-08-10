var controllers = {};
var app = angular.module("maharishiApp", ['ngRoute']);
app.controller(controllers);
app.config(function ($routeProvider) {
    $routeProvider
        .when("/", { controller: 'homeCtrl', templateUrl: '/Content/partials/home.html' })
        .when("/onlinezip", { controller: 'homeCtrl', templateUrl: '/Content/partials/onlinezip.html' })
        .when("/todolistconv", { controller: 'homeCtrl', templateUrl: '/Content/partials/todolistconv.html' });
});

controllers.navCtrl = function ($scope, $location) {
    function init() {
        $scope.homeActiveTab = ($location.path() === "/") ? "active" : "";
        $scope.onlinezipActiveTab = ($location.path() === "/onlinezip") ? "active" : "";
        $scope.todolistconvActiveTab = ($location.path() === "/todolistconv") ? "active" : "";
    }
    init();
    
    $scope.setActiveTab = function (navigateTo) {
        $scope.homeActiveTab = (navigateTo == "/") ? "active" : "";
        $scope.onlinezipActiveTab = (navigateTo == "/onlinezip") ? "active" : "";
        $scope.todolistconvActiveTab = (navigateTo == "/todolistconv") ? "active" : "";
    }
}

controllers.homeCtrl = function ($scope, $location, zipFactory) {
    $scope.zipFile = function () {
        zipFactory.zipFile({ url: $scope.url, filename: $scope.filename }, function (result) {
            $scope.downloadlabel = "Downloaded File: ";
            $scope.filelink = result.filelink;
            $scope.downloadfilename = result.filename;
        });
    }
}

app.factory('zipFactory', function ($http) {
    var factory = {};
    factory.zipFile = function (zipRequestData, callback) {
        var svc = $http.post('/CompressFileHandler.ashx', { url: zipRequestData.url, filename: zipRequestData.filename }).
                      success(function(data, status, headers, config) {
                          // this callback will be called asynchronously
                          // when the response is available
                          //console.log(status);
                          //console.log(data);
                          callback(data);
                      }).
                      error(function (data, status, headers, config) {
                          // called asynchronously if an error occurs
                          // or server returns response with an error status.
                          console.log(status);
                      });
    }

    return factory;
});