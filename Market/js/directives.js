(function() {
    angular.module('market.directives', [])
        .directive('productCtrl', function() {
            return {
                restrict: 'E',
                templateUrl: 'Views/product.html',
                controller: "ProductCtrl"
            };
        })
        .directive('sliderCtrl', function() {
            return {
                restrict: 'E',
                templateUrl: 'Views/slider.html',
                controller: 'SliderCtrl'
            }
        });
})();