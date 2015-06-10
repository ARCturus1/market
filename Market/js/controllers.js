﻿(function () {
    angular.module('market.controllers', [])

        .controller('HomeController', [
            '$scope', '$http', 'productsService', function ($scope, $http, productsService) {
                $scope.model = {};
                $scope.model.products = [];
                productsService.getProducts()
                    .success(function (data) { $scope.model.products = data; })
                    .error(function (message) { console.error(message); });
            }
        ])

        .controller('IndexController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {
            $scope.logOut = function () {
                authService.logOut();
                $location.path('/home');
            }
            $scope.authentication = authService.authentication;
        }])

        .controller('ProductCtrl', [
            '$scope', function ($scope) {
                $scope.addToPurchase = function (product) {
                    console.log(product);
                };
            }
        ])

        .controller('SliderCtrl', ['$scope', function($scope) {
            
        }])

        .controller('NewsController', [
            '$scope', '$http', 'newsService', function ($scope, $http, newsService) {
                var _updateNewsSuccess = function(data) { $scope.model.news = data; };
                var _newsError = function(message) { console.error(message); };

                $scope.model = {};
                $scope.model.news = [];

                $scope.model.NewName = '';
                $scope.model.NewDescription = '';
                $scope.addNew = function() {
                    if ($scope.model.NewName == '' || $scope.model.NewDescription == '')
                        return;

                    newsService.addNew($scope.model.NewName, $scope.model.NewDescription)
                        .success(function() {
                            $scope.model.NewName = '';
                            $scope.model.NewDescription = '';
                            $scope.getNews();
                        })
                        .error(_newsError);
                }

                $scope.getNews = function () {
                    newsService.getNews()
                        .success(_updateNewsSuccess)
                        .error(_newsError);
                }

                $scope.editNew = function (postedNew) {
                    console.log(postedNew);
                }
                $scope.deleteNew = function(postedNew) {
                    newsService.deleteNew(postedNew.NewId)
                        .success(function() { newsService.getNews().success(_updateNewsSuccess).error(_newsError) })
                        .error(_newsError);
                }

                $scope.getNews();
            }
        ])

        .controller('SignupController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

            $scope.savedSuccessfully = false;
            $scope.message = "";

            $scope.registration = {
                userName: "",
                password: "",
                confirmPassword: ""
            };

            $scope.signUp = function () {

                authService.saveRegistration($scope.registration).then(function (response) {

                    $scope.savedSuccessfully = true;
                    $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
                    startTimer();

                },
                 function (response) {
                     var errors = [];
                     for (var key in response.data.modelState) {
                         if (response.data.modelState.hasOwnProperty(key)) {
                             for (var i = 0; i < response.data.modelState[key].length; i++) {
                                 errors.push(response.data.modelState[key][i]);
                             }
                         }
                     }
                     $scope.message = "Failed to register user due to:" + errors.join(' ');
                 });
            };

            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $location.path('/login');
                }, 2000);
            }

        }])
        .controller('LoginController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

            $scope.loginData = {
                userName: "",
                password: ""
            };

            $scope.message = "";

            $scope.login = function () {

                authService.login($scope.loginData).then(function (response) {
                    $location.path('/home');
                },
                 function (err) {
                     $scope.message = err.error_description;
                 });
            };

        }]);
})();
