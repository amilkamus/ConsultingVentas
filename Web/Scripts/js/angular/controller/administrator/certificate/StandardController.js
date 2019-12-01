app.controller('StandardController', ['$rootScope', '$scope', 'StandardService', 'CommonService', function($rootScope, $scope, StandardService, CommonService) {
    $scope.clone = {};
    $scope.page = { incripcion: [] };
    $scope.objectSelected = {};
    $scope.basics = {};
    $scope.objectRow = [];
    $scope.random = Math.floor((Math.random() * 100000) + 1);

    $scope.search = function(inputSearch) {
        var out = [];
        out = $rootScope.searchFilter($scope.page.incripcion, inputSearch);
        $scope.message = '<p class="badge badge-success" style="font-size: 14px">Mostrando Registros: ' + out.length + ' de ' + $scope.page.incripcion.length + '</p>';
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

    $scope.getAllBasic = function() {
        StandardService.getActivityGroup().then(function(data) {            
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

    $scope.exist = function(data) {
        if ($scope.page.incripcion.length > 0) {
            for (let i = 0, n = $scope.page.incripcion.length; i < n; i++) {
                if ($scope.page.incripcion[i].id == data.id) { return true };
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

    $scope.getAllInscription = function() {
        $scope.clone = {};
        $scope.page.incripcion = [];
        if($scope.tmp.course!=undefined && $scope.tmp.course!='' && $scope.tmp.group!=undefined && $scope.tmp.group!=''){
            var params = { start: $rootScope.pager.currentPage,course:$scope.tmp };
            StandardService.getAllInscription(params).then(function(data) {
                if (data.data.data.length) {
                    for (let i = 0, n = data.data.data.length; i < n; i++) {
                        if (!$scope.exist(data.data.data[i]))
                            $scope.page.incripcion.push(data.data.data[i]);
                    }
                    $rootScope.pager.total = data.data.count;
                    $rootScope.pager.pages = [];
                    $rootScope.numberOfPages(data.data.count);
                    $scope.clone = Object.assign({}, $scope.page);
                } else
                    $scope.message = '<p class="badge badge-danger" style="font-size: 14px;margin: 0;">No se encontro Registro alguno (^_^)!!..</p>';
            });
        }
    };

    $scope.setImage = function(item){
        $scope.random = Math.floor((Math.random() * 100000) + 1);
        $scope.selectRow = undefined;
        $scope.selectRow = {};
        $scope.selectRow = item;
    };

    $scope.select = function(item,tipo){
        type == null ? type = 'all' : type = tipo;
        $scope.objectRow = undefined;
        $scope.objectRow = [];
        if(type=='all'){
            for (let i = 0, n = $scope.page.incripcion.length; i < n; i++) {
                var element = Object.assign({}, $scope.page.incripcion[i]);
                if(element.record.certificate==null || element.record.certificate.length==0){
                    element.record.certificate = null;
                    $scope.objectRow.push(element);
                }
            }
        }else if(type=='generate'){
            var element = Object.assign({}, item);;
            if(element.record.certificate==null || element.record.certificate.length==0){
                element.record.certificate = null;
                $scope.objectRow.push(element);
            }
        }
    }

    $scope.generate = function() {
        var params = {
            certificate: $scope.objectRow
        };
        StandardService.generate(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllInscription();
            } else {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Alerta!',
                    message: data.data.message,
                }, { type: 'danger' });
            }
        });
    };

    $scope.cancel = function() {
        $scope.page = Object.assign({}, $scope.clone);
    };

    $scope.init = function() {
        $scope.getAllBasic();
    };

    $scope.init();
}]);