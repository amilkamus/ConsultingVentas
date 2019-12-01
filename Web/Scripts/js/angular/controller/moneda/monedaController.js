app.controller('monedaController', ['$rootScope', '$scope', 'monedaService', function ($rootScope, $scope, monedaService) {

    $scope.model = {};
    $scope.elementos = { lista: [] };

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarMoneda();
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
    $scope.guardarMoneda = function () {
        var params = {
            monedaViewModels: $scope.model
        };
        monedaService.guardarMoneda(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};

                $scope.listarMoneda();
            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarMoneda').modal('hide')
    }
    //Fin

    /* ---------------------------------------- */
    
    //Listar
    $scope.listarMoneda = function () {
        monedaService.listarMoneda().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModalEliminar = function (idMoneda) {
        $scope.id = idMoneda;
    }

    $scope.eliminarMoneda = function () {
        monedaService.eliminarMoneda({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarMoneda();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }

    $scope.editarMoneda = function (moneda) {
        $scope.model = moneda;
    }
    //Fin

    $scope.listarMoneda();
}]);