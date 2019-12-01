app.controller('CourseController', ['$rootScope', '$scope', 'CourseService', 'CommonService', function($rootScope, $scope, CourseService, CommonService) {
    $scope.clone = {};
    $scope.page = { courses: [] };
    $scope.objectSelected = {};
    $scope.maxdate = new Date();
    $scope.basics = { locations: [] };

    $scope.search = function(inputSearch) {
        var out = [];
        out = $rootScope.searchFilter($scope.page.courses, inputSearch);
        $scope.message = '<p class="badge badge-success" style="font-size: 14px">Mostrando Registros: ' + out.length + ' de ' + $scope.page.courses.length + '</p>';
        return out;
    };

    $scope.validateForm = function(form) {
        if (form.$valid) {
            showModal('#showQuestion');
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta!',
                message: 'Error de Validación Asegurese de Ingresar todos los campos (*)',
            }, { type: 'danger', z_index: 2000 });
        }
    };

    $scope.exist = function(data) {
        if ($scope.page.courses.length > 0) {
            for (let i = 0, n = $scope.page.courses.length; i < n; i++) {
                if ($scope.page.courses[i].id == data.id) { return true };
            }
            return false;
        }
        return false;
    };

    $scope.changePageCustom = function(number) {
        if ((number / $rootScope.pager.pageDivide >= 1 && (number % $rootScope.pager.pageDivide == 0 || number % 6 < $rootScope.pager.pageDivide))) {
            var n = parseInt(number / $rootScope.pager.pageDivide);
            if (n > 1) {
                for (let i = 0; i < n; i++) {
                    $rootScope.pager.currentPage = $rootScope.pager.pageDivide;
                    $rootScope.pager.pageDivide += 5;
                    $scope.getAllCourse();
                }
                $rootScope.pager.currentPage = number;
                $rootScope.pager.pageDivide = number + 5;
            } else {
                $rootScope.pager.pageDivide += 5;
                $scope.getAllCourse();
            }
        }
    };
    //Cargar datos iniciales
    $scope.getAllBasics = function() {
        $scope.basics.locations = [];
        $scope.basics.mains = [];

        CommonService.getLocation().then(function(data) {
            if (data.data.length) {
                $scope.basics.locations = data.data;
            }
        });
        CourseService.getAllMain().then(function(data) {
            if (data.data.length) {
                $scope.basics.mains = data.data;
            }
        });
    };
    //Seleccionar el objeto
    $scope.select = function(input, type) {
        $scope.objectSelected = undefined;
        $scope.objectSelected = {};
        if (input != undefined) {
            $scope.type = type;
            $scope.objectSelected = input;
            $scope.objectSelected.mainactivity = input.parent != null ? false : true;
        } else {
            $scope.type = 'save';
            $scope.objectSelected.id = 0;
            $scope.objectSelected.mainactivity = true;
        }
        $scope.objectSelected.type = 'course';
    };
    //Read
    $scope.getAllCourse = function() {
        $scope.clone = {};
        $scope.page.courses = [];

        var params = { start: $rootScope.pager.currentPage };

        CourseService.getAllCourse(params).then(function (data) {
            if (data.data.data.length) {
                for (let i = 0, n = data.data.data.length; i < n; i++) {
                    if (!$scope.exist(data.data.data[i]))
                        $scope.page.courses.push(data.data.data[i]);
                }
                $rootScope.pager.total = data.data.count;
                $rootScope.pager.pages = [];
                $rootScope.numberOfPages(data.data.count);
                $scope.clone = Object.assign({}, $scope.page);
            } else
                $scope.message = '<p class="badge badge-danger" style="font-size: 14px;margin: 0;">No se encontro Registro alguno (^_^)!!..</p>';
        });
    };

    $scope.saveUpdate = function() {
        var params = {
            course: $scope.objectSelected
        };
        CourseService.saveUpdate(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllCourse();
                $scope.getAllBasics();
            } else {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Alerta!',
                    message: data.data.message,
                }, { type: 'danger' });
            }
        });
    };

    $scope.delete = function() {
        var params = {
            course: $scope.objectSelected
        };
        CourseService.delete(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllCourse();
                $scope.getAllBasics();
            } else {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Alerta! <br>',
                    message: data.data.message,
                }, { type: 'danger' });
            }
        });
    }

    $scope.action = function() {
        if ($scope.type == 'save' || $scope.type == 'edit')
            $scope.saveUpdate();
        else if ($scope.type == 'delete')
            $scope.delete();
    };

    $scope.cancel = function() {
        $scope.page = Object.assign({}, $scope.clone);
    };

    $scope.init = function() {
        $scope.getAllBasics();
        $scope.getAllCourse();
    };

    $scope.init();
}]);