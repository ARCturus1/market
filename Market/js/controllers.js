(function () {
    "use strict";
    angular.module('market.controllers', [])
        .controller('CatalogController', [
            '$scope', '$http', 'productsService', function ($scope, $http, productsService) {
                $scope.getByCategory = function (category) {
                    productsService.getProducts(category)
                     .success(function (data) { $scope.model.products = data; })
                     .error(function (message) { console.error(message); });
                }
                $scope.getCategories = function () {
                    $scope.model.categories = [];
                    $scope.model.categories[0] = 'Все категории';
                    productsService.getCategories()
                        .success(function (data) {
                            angular.forEach(data, function (item) {
                                $scope.model.categories.push(item);
                            });
                        })
                        .error(function (message) {
                            console.error(message);
                        });
                }
                $scope.model = {};
                $scope.model.products = [];
                $scope.getByCategory();
                $scope.getCategories();
            }
        ])

        .controller('ProductController', ['$scope', '$routeParams', '$q', 'productsService', 'Upload',
            function ($scope, $routeParams, $q, productsService, upload) {
            $scope.product = {};
            var id = $routeParams.id;
            $scope.progreses = [];

            var _getProduct = function () {
                productsService.getProduct(id)
                    .success(function (data) {
                        $scope.product = data;
                    })
                    .error(function (message) {
                        console.error(message);
                    });
            }

            var _getImages = function () {
                productsService.getImagesForProduct(id)
                    .success(function (data) {
                        $scope.product.Images = data;
                    })
                    .error(function (message) {
                        console.error(message);
                    });
            }


            $scope.uploadImage = function (files) {
                if (files && files.length) {
                    var arrayPromises = [];
                    for (var i = 0; i < files.length; i++) {
                        $scope.progreses[i] = 0;
                        var file = files[i];
                        arrayPromises.push(upload.upload({
                            url: 'api/products/addImage/' + $scope.product.ProductID,
                            file: file
                        }).progress(function(evt) {
                            $scope.progreses[i] = parseInt(100.0 * evt.loaded / evt.total);
                        }));

                    }
                    $q.all(arrayPromises).then(
                        function (values) {
                            //$scope.progress = 0;
                            _getImages();
                        },
                        function (message) {
                            $scope.progreses[i] = 0;
                            console.error(message);
                        });
                }
            }

            $scope.deleteImage = function (productId, imageId) {
                productsService.deleteImage(productId, imageId).then(
                    function(data) {
                        $scope.product.Images = data.data;
                    }, function(message) {
                        console.error(message);
                    });
            }

            $scope.save = function () {
                productsService.updateProduct($scope.product)
                    .success(function () {
                        _getProduct();
                    }).error(function (message) {
                        console.error(message);
                    });
            }
            $scope.back = function () {
                window.history.back();
            }

            _getProduct();
        }])

        .controller('ProductCtrl', [
            '$scope', function ($scope) {
                $scope.addToPurchase = function (product) {
                    console.log(product);
                };
            }
        ])

        .controller('IndexController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {
            $scope.logOut = function () {
                authService.logOut();
                //$location.path('/home');
                window.history.back();
            }
            $scope.authentication = authService.authentication;

            //$scope.links = [{ name: 'Home', url: '#/home'}, { name: 'News', url: '#/news'}];
        }])

        .controller('SliderCtrl', ['$scope', 'Upload', 'sliderService', function ($scope, upload, sliderService) {
            $scope.slide = {};
            $scope.slide.textForSlide = '';
            $scope.myInterval = 5000;
            $scope.slides = [];
            $scope.files = undefined;

            $scope.upload = function (files) {
                if (files && files.length) {
                    for (var i = 0; i < files.length; i++) {
                        var file = files[i];
                        upload.upload({
                            url: 'api/slider/add',
                            data: $scope.slide.textForSlide,
                            file: file
                        }).progress(function (evt) {
                            $scope.progress = parseInt(100.0 * evt.loaded / evt.total);
                        }).success(function (data, status, headers, config) {
                            $scope.progress = 0;
                            sliderService.getSlides().success(function (data) {
                                $scope.slide.textForSlide = '';
                                $scope.slides = [];
                                data.forEach(function (item) {
                                    $scope.slides.push({ id: item.Id, image: item.FilePath, text: item.Name });
                                });
                            }).error(function (error) {
                                console.error(error);
                            });
                        }).error(function (message) {
                            $scope.progress = 0;
                            console.error(message);
                        });
                    }
                }
            };
            $scope.cleanSlide = function () {
                $scope.files = undefined;
                $scope.slide.textForSlide = '';
            }

            sliderService.getSlides()
                .success(function (data) {
                    data.forEach(function (item) {
                        $scope.slides.push({ id: item.Id, image: item.FilePath, text: item.Name });
                    });
                }).error(function (error) {
                    console.error(error);
                });

            $scope.isCollapsed = true;
        }])

        .controller('SliderItemCtrl', ['$scope', 'sliderService', function ($scope, sliderService) {
            $scope.deleteSlide = function (slide) {
                sliderService.deleteSlide(slide.id)
                    .success(function () {
                        sliderService.getSlides()
                            .success(function (data) {
                                $scope.$parent.$parent.slides = [];
                                data.forEach(function (item) {
                                    $scope.$parent.$parent.slides.push({ id: item.Id, image: item.FilePath, text: item.Name });
                                });
                            });
                    });
            };
        }])


        .controller('NewsController', [
            '$scope', '$http', 'newsService', function ($scope, $http, newsService) {
                var _updateNewsSuccess = function (data) { $scope.model.news = data; };
                var _newsError = function (message) { console.error(message); };

                $scope.model = {};
                $scope.model.news = [];

                $scope.model.NewName = '';
                $scope.model.NewDescription = '';
                $scope.model.NewShortDesk = '';
                $scope.addNew = function () {
                    if ($scope.model.NewName == '' || $scope.model.NewDescription == '')
                        return;

                    newsService.addNew($scope.model.NewName, $scope.model.NewShortDesk, $scope.model.NewDescription)
                        .success(function () {
                            $scope.model.NewName = '';
                            $scope.model.NewDescription = '';
                            $scope.model.NewShortDesk = '';
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
                $scope.deleteNew = function (postedNew) {
                    newsService.deleteNew(postedNew.NewId)
                        .success(function () { newsService.getNews().success(_updateNewsSuccess).error(_newsError) })
                        .error(_newsError);
                }

                $scope.getNews();
            }
        ])

        .controller('NewEditController', ['$scope', '$routeParams', 'newsService', function ($scope, $routeParams, newsService) {
            $scope.postedNew = {};
            var id = $routeParams.id;
            var _getNew = function () {
                newsService.getNew(id)
                    .success(function (data) {
                        $scope.postedNew = data;
                    })
                    .error(function (message) {
                        console.log(message);
                    });
            }
            $scope.save = function () {
                newsService.updateNew($scope.postedNew)
                    .success(function () {
                        _getNew();
                    }).error(function (message) {
                        console.error(message);
                    });
            }
            $scope.back = function () {
                window.history.back();
            }

            _getNew();
        }])

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
