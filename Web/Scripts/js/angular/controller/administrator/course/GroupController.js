app.controller('GroupController', ['$rootScope', '$scope', 'GroupService', 'CommonService', function($rootScope, $scope, GroupService, CommonService) {
    $scope.clone = {};
    $scope.page = { groups: [] };
    $scope.objectSelected = {};
    $scope.maxdate = new Date();
    $scope.basics = { locations: [] };

    $scope.search = function(inputSearch,lista) {
        list == null ? list = null : list = lista;
        var out = [];
        out = $rootScope.searchFilter(list==null?$scope.page.courses:list, inputSearch);
        $scope.message = list==null?'<p class="badge badge-success" style="font-size: 14px">Mostrando Registros: ' + out.length + ' de ' + $scope.page.courses.length + '</p>':'';
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

    $scope.getAllBasics = function() {
        $scope.basics.locations = [];
        $scope.basics.trainers = [];
        CommonService.getLocation().then(function(data) {
            if (data.data.length) {
                $scope.basics.locations = data.data;
            }
        });
        GroupService.getAllTrainer().then(function(data) {
            if (data.data.length) {
                $scope.basics.trainers = data.data;
            }
        });
    };

    $scope.getAllGroup = function() {
        $scope.clone = {};
        $scope.page.courses = [];
        GroupService.getAllGroups().then(function(data) {
            if (data.data.length) {
                $scope.page.courses = data.data;
                $scope.clone = Object.assign({}, $scope.page);
                $rootScope.pager.objects = data.data;
                $rootScope.pager.pages = [];
                $rootScope.numberOfPages(data.data.length);
            } else
                $scope.message = '<p class="badge badge-danger" style="font-size: 14px;margin: 0;">No se encontro Registro alguno (^_^)!!..</p>';
        });
    };

    $scope.removeTrainer = function(element){
        for (let i = 0,n=$scope.objectSelected.trainers.length; i < n; i++) {
            if($scope.objectSelected.trainers[i].activity_id == element.activity_id && 
                $scope.objectSelected.trainers[i].trainer_id == element.trainer_id ){
                $scope.objectSelected.trainers[i].state = 0;
                break;
            }
        }
    };

    $scope.appendGroupActivityTrainer = function(){
        if($scope.tmp.toAppend.activity_id!=undefined && $scope.tmp.toAppend.activity_id!='' &&
            $scope.tmp.toAppend.trainer.id!=undefined && $scope.tmp.toAppend.trainer.id!=''
            ){
            $scope.tmp.toAppend.id = 0;
            $scope.tmp.toAppend.group_id = $scope.objectSelected.id;
            $scope.tmp.toAppend.state = 1;
            $scope.tmp.toAppend.trainer_id = $scope.tmp.toAppend.trainer.id;
            for (let i = 0,n=$scope.objectSelected.parent.child.length; i < n; i++) {
                if($scope.tmp.toAppend.activity_id.toString()==$scope.objectSelected.parent.child[i].id.toString()){
                    $scope.tmp.toAppend.activity = $scope.objectSelected.parent.child[i];
                    break;
                }
            }
            $scope.appendTrainers( Object.assign({}, $scope.tmp.toAppend));
        }else{
            $.notify({
                icon: 'fa fa-exclamation-triangle',
                title: '<strong>Alerta!</strong>',
                message: '<br>Selecciona un Instructor y Curso para agregar!!',
            }, { type: 'danger' , z_index: 2000});
        }        
    };

    $scope.appendTrainers = function(element){
        var exist = false;
        for (let i = 0,n=$scope.objectSelected.trainers.length; i < n; i++) {
            if($scope.objectSelected.trainers[i].activity_id == element.activity_id && $scope.objectSelected.trainers[i].state==1 ){
                exist = true;
                break;
            }
        }
        if(exist!=true){
            exist = false;
            for (let i = 0,n=$scope.objectSelected.trainers.length; i < n; i++) {
                if($scope.objectSelected.trainers[i].activity_id == element.activity_id && 
                    $scope.objectSelected.trainers[i].trainer_id == element.trainer_id && $scope.objectSelected.trainers[i].state==0 ){
                    exist = true;
                    $scope.objectSelected.trainers[i].state = 1;
                    break;
                }
            }
            if(exist!=true)
                $scope.objectSelected.trainers.push(element);
        }else{
            $.notify({
                icon: 'fa fa-exclamation-triangle',
                title: '<strong>Alerta!</strong>',
                message: '<br>El Curso ya tiene Docente, si desea cambiarlo, primero quítelo!!',
            }, { type: 'danger' , z_index: 2000});
        }
    };

    $scope.select = function(object, type, item) {
        $scope.objectSelected = undefined;
        $scope.objectSelected = {};
        $scope.type = type;
        if (object != undefined) {
            $scope.objectSelected = object;
            $scope.objectSelected.location_id = $scope.objectSelected.location_id!=null?$scope.objectSelected.location_id.toString():null;
            $('#location').val($scope.objectSelected.location_id!=null?$scope.objectSelected.location_id.toString():null).trigger('change.select2');
            if (object.start != null && object.start != undefined && typeof(object.start) == 'string') {
                var start = object.start.split('-');
                $scope.objectSelected.start = new Date(start[0], start[1], start[2]);
            }
            if (object.end != null && object.end != undefined && typeof(object.end) == 'string') {
                var end = object.end.split('-');
                $scope.objectSelected.end = new Date(end[0], end[1], end[2]);
            }
        } else {
            $scope.objectSelected.id = 0;
            var name = item.group[(item.group.length - 1)].name.split(' - ');
            $scope.objectSelected.name = name[0] + ' - Group' + $rootScope.zeroPad((parseInt(name[1].split('Group')[1]) + 1), 3);
            $scope.objectSelected.activity_id = item.id;
            $scope.objectSelected.trainers = [];
        }
        $scope.objectSelected.parent = Object.assign({}, item);
        $scope.objectSelected.parent.group = null;
    };

    $scope.saveUpdate = function() {
        var params = {
            group: $scope.objectSelected
        };
        GroupService.saveUpdate(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllGroup();
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
            group: $scope.objectSelected
        };
        GroupService.delete(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllGroup();
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
        $scope.getAllGroup();
    };

    $scope.init();
}]);