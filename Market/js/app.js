(function() {
    angular.module('market', [
        'ngRoute',
        'market.controllers',
        'market.directives',
        'market.factories',
        'angular-loading-bar',
        'ngAnimate',
        'LocalStorageModule',
        'ui.bootstrap',
        'ngFileUpload'])
        .config([
            '$routeProvider', '$httpProvider', function($routeProvider, $httpProvider) {
                $routeProvider
                    .when('/home', {
                        templateUrl: 'Views/home.html',
                        controller: 'CatalogController'
                    })
                    .when('/home/:id', {
                        templateUrl: 'Views/productDetails.html',
                        controller: 'ProductController'
                    })
                    //.when('/home/category/{category}', {
                    //    templateUrl: 'Views/home.html',
                    //    controller: 'CatalogControllerByCategory'
                    //})
                    .when('/news', {
                        templateUrl: 'Views/news.html',
                        controller: 'NewsController'
                    })
                    .when('/news/:id', {
                        templateUrl: 'Views/newEdit.html',
                        controller: 'NewEditController'
                    })
                    .when('/login', {
                        templateUrl: 'Views/login.html',
                        controller: 'LoginController'
                    })
                    .when('/signup', {
                        templateUrl: 'Views/signup.html',
                        controller: 'SignupController'
                    })
                    .otherwise({
                        redirectTo: '/home'
                    });
                $httpProvider.interceptors.push('authInterceptorService');
            }
        ])
        .run([
            'authService', function (authService) {
                authService.fillAuthData();
            }
        ]);
})();