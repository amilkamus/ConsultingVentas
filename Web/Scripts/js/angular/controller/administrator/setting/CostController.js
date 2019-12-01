app.controller('CostController', ['$rootScope', '$scope', 'blockUI', 'CostService', function($rootScope, $scope, blockUI, CostService) {
    $scope.clone = {};
    $scope.page = { cost: [] };

    $scope.search = function(inputSearch) {
        var out = [];
        out = $rootScope.searchFilter($scope.page.cost, inputSearch);
        $scope.message = '<p class="badge badge-success" style="font-size: 14px">Mostrando Registros: ' + out.length + ' de ' + $scope.page.cost.length + '</p>';
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
    }

    $scope.getAllCost = function() {
        $scope.clone = {};
        $scope.page.cost = [];
        CostService.getAllCost().then(function(data) {
            if (data.data.length) {
                $scope.page.cost = data.data;
                $scope.clone = Object.assign({}, $scope.page);
                $rootScope.pager.objects = data.data;
                $rootScope.pager.pages = [];
                $rootScope.numberOfPages(data.data.length);
            } else
                $scope.message = '<p class="badge badge-danger" style="font-size: 14px;margin: 0;">No se encontro Registro alguno (^_^)!!..</p>';
        });
    };

    $scope.getAllBasic = function() {
        CostService.getAllRole().then(function(data) {
            $scope.role = data.data;
        });
    };

    $scope.setPrice = function(selected = null, role=null){
        if(selected!=null){
            for (let i = 0,n = selected.length; i < n; i++) {
                if(selected[i].role_id.toString() == role.toString()){
                    $scope.objectSelected.price =  selected[i].cost;
                    break;
                }
                $scope.objectSelected.price =  0;
            }
        }
    };

    $scope.selectCombo = function(id=null){
        if(id!=null && id!=''){
            for (let i = 0,n = $scope.page.cost.length; i < n; i++) {
                if($scope.page.cost[i].id.toString() == id.toString()){
                    $scope.objectSelected = $scope.page.cost[i];
                    break;
                }
            }
        }
    };

    $scope.select = function(input, type) {
        $scope.objectSelected = undefined;
        $scope.objectSelected = {};
        $scope.type = type;
        if (input != undefined) {
            $scope.objectSelected = input;
            $scope.tmp.course = input.id.toString();   
            $('#course').val($scope.tmp.course.toString()).trigger('change.select2');
        } else {
            $scope.objectSelected.id = 0;
            $('#course').val(null).trigger('change.select2');     
        }
        $('#role').val(null).trigger('change.select2');
        $scope.objectSelected.price =  0;
    };

    $scope.saveUpdate = function() {
        $scope.objectSelected.role = $scope.tmp.role;
        $scope.objectSelected.activity = $scope.tmp.course;
        var params = {
            cost: $scope.objectSelected
        };
        CostService.saveUpdate(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllCost();
            } else {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Alerta!',
                    message: data.data.message,
                }, { type: 'danger' });
            }
        });
    };

    $scope.action = function() {
        if ($scope.type == 'save' || $scope.type=='edit')
            $scope.saveUpdate();
    };

    $scope.cancel = function() {
        $scope.seat = Object.assign({}, $scope.clone);
    };

    $scope.init = function () {
        $scope.getAllBasic();
        $scope.getAllCost();
    }

    $scope.init();
}]);