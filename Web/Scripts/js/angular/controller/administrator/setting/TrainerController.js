app.controller('TrainerController', ['$rootScope', '$scope', 'blockUI', 'TrainerService', function($rootScope, $scope, blockUI, TrainerService) {
    $scope.clone = {};
    $scope.page = { trainer: [] };

    $scope.search = function(inputSearch) {
        var out = [];
        out = $rootScope.searchFilter($scope.page.trainer, inputSearch);
        $scope.message = '<p class="badge badge-success" style="font-size: 14px">Mostrando Registros: ' + out.length + ' de ' + $scope.page.trainer.length + '</p>';
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

    $scope.getPersonalData = function(document){
        var savedDocument = document;
        if(document!=undefined){
            TrainerService.getPersonalData({document:document}).then(function(data) {
                if(data.data.person!=null) $scope.objectSelected = data.data;
                else $scope.objectSelected.person = {};
                if(data.data.code==undefined){
                    $scope.objectSelected.code = 'Asignado Automáticamente';
                    $scope.objectSelected.id = 0;
                }
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

    $scope.getAllTrainer = function() {
        $scope.clone = {};
        $scope.page.trainer = [];
        TrainerService.getAllTrainer().then(function(data) {
            if (data.data.length) {
                $scope.page.trainer = data.data;
                $scope.clone = Object.assign({}, $scope.page);
                $rootScope.pager.objects = data.data;
                $rootScope.pager.pages = [];
                $rootScope.numberOfPages(data.data.length);
            } else
                $scope.message = '<p class="badge badge-danger" style="font-size: 14px;margin: 0;">No se encontro Registro alguno (^_^)!!..</p>';
        });
    };

    $scope.select = function(input, type) {
        $scope.objectSelected = undefined;
        $scope.objectSelected = {};
        $scope.type = type;
        if (input != undefined) {
            $scope.objectSelected = input;
            $('#sexo').val($scope.objectSelected.person.sexo!=null?$scope.objectSelected.person.sexo.toString():null).trigger('change.select2');
            if(input.person.fecnac!=null && input.person.fecnac!=undefined && typeof(input.person.fecnac)=='string'){
                $scope.objectSelected.person.fecnac = $rootScope.date(input.person.fecnac);
            }
            if($scope.type=='edit'){
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
        } else {
            $scope.objectSelected.id = 0;
            $('#sexo').val(undefined).trigger('change.select2');
            $('#department').val(undefined).trigger('change.select2');
            $('#province').val(undefined).trigger('change.select2');
            $('#district').val(undefined).trigger('change.select2');
        }
    };

    $scope.saveUpdate = function() {
        $scope.objectSelected.person.ubigeo = $scope.tmp.department+$scope.tmp.province+$scope.tmp.district;
        var params = {
            trainer: $scope.objectSelected
        };
        TrainerService.saveUpdate(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllTrainer();
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
            trainer: $scope.objectSelected
        };
        TrainerService.delete(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllTrainer();
            } else {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Alerta!',
                    message: data.data.message,
                }, { type: 'danger' });
            }
        });
    }

    $scope.action = function() {
        if ($scope.type == 'save' || $scope.type=='edit')
            $scope.saveUpdate();
        else if ($scope.type == 'delete')
            $scope.delete();
    };

    $scope.cancel = function() {
        $scope.seat = Object.assign({}, $scope.clone);
    };

    $scope.init = function () {
        $rootScope.listUbigeo();
        $scope.getAllTrainer();
    }

    $scope.init();
}]);