camperApp = angular.module('camperApp', ['angularFileUpload']);

camperApp.controller('camperCtrl', ['$scope', '$upload', function ($scope, $upload) {
    $scope.camper = {};
    $scope.image = "";
    $scope.onFileSelect = function($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            $scope.image = $files[i];
        }
    };
    $scope.formSubmit = function () {
        $scope.upload = $upload.upload({
            url: '/api/camper', //upload.php script, node.js route, or servlet url
            method: 'POST', //or PUT,
           // headers: {'headerKey': 'headerValue'},
           // withCredentials: true,
           data: $scope.camper,
           file: $scope.image,
           // file: $files, //upload multiple files, this feature only works in HTML5 FromData browsers
           /* set file formData name for 'Content-Desposition' header. Default: 'file' */
           //fileFormDataName: myFile, //OR for HTML5 multiple upload only a list: ['name1', 'name2', ...]
           /* customize how data is added to formData. See #40#issuecomment-28612000 for example */
           //formDataAppender: function(formData, key, val){} //#40#issuecomment-28612000
        }).progress(function (evt) {
            console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
        }).success(function(data, status, headers, config) {
            // file is uploaded successfully
            console.log(data);
        });   
    };

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
            
            //var reader = new FileReader();
            //reader.onload = function(e) {
            //    scope.image = e.target.result;
            //    document.getElementById("profilePic").src = e.target.result;
            //    scope.$apply();
            //};

            elem.on('change', function () {
                //reader.readAsDataURL(elem[0].files[0]);
                scope.image = elem[0].files[0];
                scope.$apply();
            });
        }
    };
}]);