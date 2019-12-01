app.controller('egresoController', ['$rootScope', '$scope', 'egresoService', function ($rootScope, $scope, egresoService) {

    $scope.model = {};
    $scope.elementos = { lista: [] };

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarEgreso();
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta !',
                message: 'Ingrese todo los datos.'
            }, { type: 'warning', z_index: 2000 });
        }
    }
    //Fin

    // Guardar
    $scope.guardarEgreso = function () {
        var params = {
            egresoViewModels: $scope.model
        };
        egresoService.guardarEgreso(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};
                $scope.listarEgreso();
            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarEgreso').modal('hide')
    }
    //Fin

    /* ---------------------------------------- */

    //Listar
    $scope.listarEgreso = function () {
        egresoService.listarEgreso().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModalEliminar = function (idGasto) {
        $scope.id = idGasto;
    }

    $scope.eliminarEgreso = function () {
        egresoService.eliminarEgreso({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarEgreso();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }

    $scope.editarEgreso = function (egreso) {
        $scope.model = egreso;
    }
    //Fin

    $scope.listarEgreso();

}]);