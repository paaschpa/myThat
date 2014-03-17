camperApp = angular.module('camperApp', ['angularFileUpload']);

camperApp.controller('camperEditCtrl', ['$scope', '$upload','$http', function ($scope, $upload, $http) {
    $scope.camper = {};
    $scope.image = "";

    $scope.onFileSelect = function($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            $scope.image = $files[i];
        }
    };

    $scope.formSubmit = function () {
        var url = '/api/camper';
        var method = 'put';

        if ($scope.image) {
            //PUT with Photo Upload
            $scope.upload = $upload.upload({
                url: url, //upload.php script, node.js route, or servlet url
                method: method, //or put,
                // headers: {'headerkey': 'headervalue'},
                // withcredentials: true,
                data: $scope.camper,
                file: $scope.image,
                // file: $files, //upload multiple files, this feature only works in html5 fromdata browsers
                /* set file formdata name for 'content-desposition' header. default: 'file' */
                //fileformdataname: myfile, //or for html5 multiple upload only a list: ['name1', 'name2', ...]
                /* customize how data is added to formdata. see #40#issuecomment-28612000 for example */
                //formdataappender: function(formdata, key, val){} //#40#issuecomment-28612000
            }).success(function(data, status, headers, config) {
                // file is uploaded successfully
                console.log(data);
            });
        } else {
            //PUT with no photo
            var editCamper = $http.put(url, $scope.camper);
            editCamper.success(function (data) {
                alert('Update');
            });
            editCamper.error(function (data) {
                alert('Error');
            });
        }
    };

}]).
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

var camperRegister = angular.module('camperRegister', []);
camperRegister.controller('camperRegisterCtrl', ['$scope', '$http', function ($scope, $http) {
    $('#RegistrationSuccess').hide();
    $('#error').hide();

    $scope.camper = {};
    $scope.register = function() {
        var newRegistration = {
            'username': $scope.camper.email,
            'email': $scope.camper.email,
            'password': $scope.camper.password,
            'autologin': true
        };

        var registration = $http.post('/api/register', newRegistration);
        registration.success(function(data) {
            $('#RegistrationForm').hide();
            $('#RegistrationSuccess').show();
        });
        registration.error(function(data) {
            $('#error').show();
            $('#RegistrationSuccess').hide();
            $scope.errorMsg = data.responseStatus.message;
        });

    };
}]).directive('sameAs', function () {
    return {
        require: 'ngModel',
        link: function(scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function(viewValue) {
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
});