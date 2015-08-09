var controllers = {};
var app = angular.module("maharishiApp", ['ngRoute',]);
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

controllers.homeCtrl = function ($scope, $location) {

}