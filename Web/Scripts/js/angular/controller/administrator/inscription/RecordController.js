app.controller('RecordController', ['$rootScope', '$scope', 'RecordService', 'CommonService', function($rootScope, $scope, RecordService, CommonService) {
    $scope.clone = {};
    $scope.page = { inscribed: [] };
    $scope.objectSelected = {};
    $scope.basics={};
    $scope.editall = false;
    $scope.objectRow = [];

    $scope.search = function(inputSearch) {
        if(inputSearch!=undefined && inputSearch!=''){
            var out = [];
            out = $rootScope.searchFilter($scope.page.inscribed, inputSearch);
            $scope.message = '<p class="badge badge-success" style="font-size: 14px">Mostrando Registros: ' + out.length + ' de ' + $scope.page.inscribed.length + '</p>';
            return out;
        }
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
    
    $scope.getAllBasic = function() {
        RecordService.getActivityGroup().then(function(data) {
            $scope.basics.course = data.data;
        });
    };

    $scope.loadGroup = function(course){
        if(course!=undefined && course!=''){
            for (let i = 0,n=$scope.basics.course.length; i < n; i++) {
                if($scope.basics.course[i].id == course){
                    $scope.course = $scope.basics.course[i];
                    $scope.basics.group = $scope.course.group;
                    $scope.objectSelected = $scope.course;
                    break;
                }
            }
        }
    };

    $scope.setCourse = function(group){
        if(group!=undefined && group!=''){
            for (let i = 0,n=$scope.basics.group.length; i < n; i++) {
                if($scope.basics.group[i].id.toString()==group.toString()){
                    $scope.objectSelected.group = $scope.basics.group[i];
                    break;
                }
            }
        }        
    };

    $scope.existrow = function(list,element){
        for (let i = 0, n = list.length; i < n; i++) {
            if (list[i].student_id == element.student_id) {
                return true;
            }
        }
        return {average:element.average,student_id:element.student_id,record:[]};
    };

    $scope.calculate = function(record,row) {
        var total = 0;
        for (let i = 0,n=row.record.length; i < n; i++) {
            total += parseFloat(row.record[i].average!=undefined?row.record[i].average:0);
        }
        row.average.average = Math.round((total/row.record.length) * 100) / 100;;
    };

    $scope.existrecord = function(list,record){
        for (let i = 0, n = list.length; i < n; i++) {
            if (list[i].activity_id == record.activity_id) {
                return i;
            }
        }
        return -1;
    };

    $scope.append=function(row, record){
        $scope.calculate(record,row);
        var result = $scope.existrow($scope.objectRow,row);
        if(result!=true){
            result.average.student_id=result.student_id;
            $scope.objectRow.push(result);
        }
        result = $scope.existrecord($scope.objectRow[$scope.objectRow.length-1].record,record);
        if(result>=0)
            $scope.objectRow[$scope.objectRow.length-1].record.splice(result,1);
        $scope.objectRow[$scope.objectRow.length-1].record.push(record);
    };

    $scope.getAllInscribed = function() {
        $scope.clone = {};
        $scope.page.inscribed = [];
        if($scope.tmp.course!=undefined && $scope.tmp.course!='' && $scope.tmp.group!=undefined && $scope.tmp.group!=''){
            var params = { 
                course:$scope.tmp
            };
            RecordService.getAllInscription(params).then(function(data) {
                if (data.data.length) {
                    for (let i = 0,n=data.data.length; i < n; i++) {
                        var row = data.data[i];
                        row.enabled = false;
                        $scope.page.inscribed.push(row);
                    }
                    $rootScope.pager.total = data.data.length;
                    $rootScope.pager.pages = [];
                    $rootScope.numberOfPages(data.data.length);
                    $scope.clone = Object.assign({}, $scope.page);
                    $scope.objectRow = undefined;
                    $scope.objectRow = [];
                    $scope.editall = false;
                } else
                    $scope.message = '<p class="badge badge-danger" style="font-size: 14px;margin: 0;">No se encontro Registro alguno (^_^)!!..</p>';
            });
        }
    };
    
    $scope.saveUpdate = function() {
        if($scope.password !='' && $scope.password!=undefined){
            var params = {
                password: $scope.password,
                record:$scope.objectRow
            };
            RecordService.saveUpdate(params).then(function (data) {
                if (data.data.status === "true") {
                    $.notify({
                        icon: 'fa fa-exclamation-triangle',
                        title: 'Satisfactorio!',
                        message: data.data.message,
                    }, { type: 'success' });
                    $scope.getAllInscribed();
                } else {
                    $.notify({
                        icon: 'fa fa-exclamation-triangle',
                        title: 'Alerta!',
                        message: data.data.message,
                    }, { type: 'danger' });
                }
            });
        }else{
            $.notify({
                icon: 'fa fa-exclamation-triangle',
                title: 'Alerta!',
                message: 'Por Favor Ingrese Contraseña',
            }, { type: 'danger' });
        }
        $scope.password='';
    };

    $scope.cancel = function() {
        $scope.page = Object.assign({}, $scope.clone);
    };

    $scope.init = function() {
        $scope.getAllBasic();
    };

    $scope.init();
}]);