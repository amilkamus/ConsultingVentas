app.controller('StudentController', ['$rootScope', '$scope', 'StudentService', 'CommonService', function($rootScope, $scope, StudentService, CommonService) {
    $scope.clone = {};
    $scope.page = { student: [] };
    $scope.objectSelected = {};
    $scope.basics = {};

    $scope.search = function(inputSearch) {
        var out = [];
        out = $rootScope.searchFilter($scope.page.student, inputSearch);
        $scope.message = '<p class="badge badge-success" style="font-size: 14px">Mostrando Registros: ' + out.length + ' de ' + $scope.page.student.length + '</p>';
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
        if ($scope.page.student.length > 0) {
            for (let i = 0, n = $scope.page.student.length; i < n; i++) {
                if ($scope.page.student[i].id == data.id) { return true };
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
    
    $scope.getAllBasic = function() {
        CommonService.getAllRoleApp().then(function(data) {
            $scope.basics.role = data.data;
        });
    };

    $scope.getPersonalData = function(document){
        var savedDocument = document;
        if(document!=undefined){
            StudentService.getPersonalData({document:document}).then(function(data) {
                if(data.data.person!=null) $scope.objectSelected = data.data;
                else $scope.objectSelected.person = {};
                if(data.data.code==undefined){
                    $scope.objectSelected.code = 'Asignado Automáticamente';
                    $scope.objectSelected.id = 0;
                }
                $('#role').val($scope.objectSelected.role_id!=undefined?$scope.objectSelected.role_id.toString():null).trigger('change.select2');
                if(data.data.person!=null){
                    $('#sexo').val($scope.objectSelected.person.sexo!=null?$scope.objectSelected.person.sexo.toString():null).trigger('change.select2');
                    if(data.data.person.fecnac!=null && data.data.person.fecnac!=undefined && typeof(data.data.person.fecnac)=='string'){
                        $scope.objectSelected.person.fecnac = $rootScope.date(data.data.person.fecnac);
                    }
                    if(data.data.person.ubigeo!=null){
                        if(data.data.person.ubigeo.length==6){
                            var ubigeo = data.data.person.ubigeo;
                            $scope.tmp.department = ubigeo.substring(0,2).toString();
                            $rootScope.listUbigeo($scope.tmp.department,null,null);
                            setTimeout(function() {
                                $('#department').val($scope.tmp.department.toString()).trigger('change.select2');
                                $scope.tmp.province = ubigeo.substring(2,4).toString();
                                $rootScope.listUbigeo($scope.tmp.department,$scope.tmp.province,null);
                            }, 1000);
                            setTimeout(function() {
                                $('#province').val($scope.tmp.province.toString()).trigger('change.select2');
                                $scope.tmp.district = ubigeo.substring(4,6).toString();
                                $('#district').val($scope.tmp.district.toString()).trigger('change.select2');
                            }, 2000);
                            setTimeout(function() {
                                $scope.tmp.district = ubigeo.substring(4,6).toString();
                                $('#district').val($scope.tmp.district.toString()).trigger('change.select2');
                            }, 1000);
                        }
                    }
                    $scope.objectSelected.person.id = 1;
                }
                else{
                    $scope.objectSelected.person.dni = document;
                    $scope.objectSelected.person.id = 0;
                    $scope.objectSelected.code = 'Asignado Automáticamente';
                    $('#department').val(null).trigger('change.select2');
                    $('#province').val(null).trigger('change.select2');
                    $('#district').val(null).trigger('change.select2');
                    $scope.basic.province = [];
                    $scope.basic.district = [];
                }                    
            });
        }
    };

    $scope.select = function(input, type) {
        $scope.objectSelected = undefined;
        $scope.objectSelected = {person:{}};
        $scope.type = type;
        if (input != undefined) {
            $scope.objectSelected = input;
            $('#sexo').val($scope.objectSelected.person.sexo!=null?$scope.objectSelected.person.sexo.toString():null).trigger('change.select2');
            $('#role').val($scope.objectSelected.role_id!=null?$scope.objectSelected.role_id.toString():null).trigger('change.select2');
            if(input.person.fecnac!=null && input.person.fecnac!=undefined && typeof(input.person.fecnac)=='string'){
                $scope.objectSelected.person.fecnac = $rootScope.date(input.person.fecnac);
            }
            if($scope.type=='edit' && input.person.ubigeo!=null){
                var ubigeo = input.person.ubigeo;
                $scope.tmp.department = ubigeo.substring(0,2).toString();
                $rootScope.listUbigeo($scope.tmp.department,null,null);
                setTimeout(function() {
                    $('#department').val($scope.tmp.department.toString()).trigger('change.select2');
                    $scope.tmp.province = ubigeo.substring(2,4).toString();
                    $rootScope.listUbigeo($scope.tmp.department,$scope.tmp.province,null);
                }, 1000);
                setTimeout(function() {
                    $('#province').val($scope.tmp.province.toString()).trigger('change.select2');
                    $scope.tmp.district = ubigeo.substring(4,6).toString();
                    $('#district').val($scope.tmp.district.toString()).trigger('change.select2');
                }, 2000);
                setTimeout(function() {
                    $scope.tmp.district = ubigeo.substring(4,6).toString();
                    $('#district').val($scope.tmp.district.toString()).trigger('change.select2');
                }, 1000);
            }
            $scope.objectSelected.person.id = 1;
        } else {
            $scope.objectSelected.id = 0;
            $scope.objectSelected.person.id = 0;
            $('#sexo').val(undefined).trigger('change.select2');
            $('#department').val(undefined).trigger('change.select2');
            $('#province').val(undefined).trigger('change.select2');
            $('#district').val(undefined).trigger('change.select2');
            $('#role').val(undefined).trigger('change.select2');
        }
    };

    $scope.getAllStudent = function() {
        $scope.clone = {};
        $scope.page.student = [];
        var params = { start: $rootScope.pager.currentPage };
        StudentService.getAllStudent(params).then(function(data) {
            if (data.data.data.length) {
                for (let i = 0, n = data.data.data.length; i < n; i++) {
                    if (!$scope.exist(data.data.data[i]))
                        $scope.page.student.push(data.data.data[i]);
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
        $scope.objectSelected.person.ubigeo = $scope.tmp.department+$scope.tmp.province+$scope.tmp.district;
        var params = {
            student: $scope.objectSelected
        };
        StudentService.saveUpdate(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllStudent();
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
            student: $scope.objectSelected
        };
        StudentService.delete(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllStudent();
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
        $rootScope.listUbigeo();
        $scope.getAllBasic();
        $scope.getAllStudent();
    };

    $scope.init();
}]);