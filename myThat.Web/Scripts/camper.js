camperApp = angular.module('camperApp', []);

camperApp.controller('camperCtrl', ['$scope', function($scope) {
    $scope.camper = {};
    $scope.image = "";
}]).
directive('sameAs', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {
                if (viewValue === document.getElementsByName(attrs.sameAs)[0].value) {
                    ctrl.$setValidity('sameAs', true);
                    return viewValue;
                } else {
                    ctrl.$setValidity('sameAs', false);
                    return undefined;
                }
            });
        }
    };
}).
directive('myUpload', [function () {
    return {
        restrict: 'A',
        link: function (scope, elem, attrs) {
            var reader = new FileReader();
            reader.onload = function(e) {
                scope.image = e.target.result;
                document.getElementById("profilePic").src = e.target.result;
                scope.$apply();
            };

            elem.on('change', function () {
                reader.readAsDataURL(elem[0].files[0]);
            });
        }
    };
}]);
;