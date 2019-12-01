app.controller('ClassroomController', ['$rootScope', '$scope', 'blockUI', 'ClassroomService', function($rootScope, $scope, blockUI, ClassroomService) {
    $scope.clone = {};
    $scope.page = { classroom: [] };

    $scope.search = function(inputSearch) {
        var out = [];
        out = $rootScope.searchFilter($scope.page.classroom, inputSearch);
        $scope.message = '<p class="badge badge-success" style="font-size: 14px">Mostrando Registros: ' + out.length + ' de ' + $scope.page.classroom.length + '</p>';
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

    $scope.getAllClassroom = function() {
        $scope.clone = {};
        $scope.page.classroom = [];
        ClassroomService.getAllClassroom().then(function(data) {
            if (data.data.length) {
                $scope.page.classroom = data.data;
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
        } else {
            $scope.objectSelected.id = 0;
        }
    };

    $scope.saveUpdate = function() {
        var params = {
            classroom: $scope.objectSelected
        };
        ClassroomService.saveUpdate(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllClassroom();
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
            classroom: $scope.objectSelected
        };
        ClassroomService.delete(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllClassroom();
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
        $scope.getAllClassroom();
    }

    $scope.init();
}]);