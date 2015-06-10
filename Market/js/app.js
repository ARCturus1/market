(function() {
    angular.module('market', [
        'ngRoute',
        'market.controllers',
        'market.directives',
        'market.factories',
        'angular-loading-bar',
        'ngAnimate',
        'LocalStorageModule'])
        .config([
            '$routeProvider', '$httpProvider', function($routeProvider, $httpProvider) {
                $routeProvider
                    .when('/home', {
                        templateUrl: 'Views/home.html',
                        controller: 'HomeController'
                    })
                    .when('/news', {
                        templateUrl: 'Views/news.html',
                        controller: 'NewsController'
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