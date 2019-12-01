app.controller('cambioMonedaController', ['$rootScope', '$scope', 'cambioMonedaService', function ($rootScope, $scope, cambioMonedaService) {

    $scope.model = {};
    $scope.cmbMoneda = [];
    $scope.elementos = { lista: [] };
    
    $scope.model.estado = "ACTIVO";

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarCambioMoneda();
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
    $scope.guardarCambioMoneda = function () {
        var params = {
            cambioMonedaViewModels: $scope.model
        };
        cambioMonedaService.guardarCambioMoneda(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};
                $scope.model.estado = "ACTIVO";
                $scope.listarCambioMoneda();
                $scope.listarComboMoneda();

            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarCambioMoneda').modal('hide')
    }
    //Fin

    //Listar Combo de Monedas
    $scope.listarComboMoneda = function () {
        cambioMonedaService.comboMoneda({}).then(function (data) {
            $scope.cmbMoneda = data.data;
            $scope.model.idMoneda = 1;
        });
    }
    //Fin

    /* ---------------------------------------- */

    //Listar
    $scope.listarCambioMoneda = function () {
        cambioMonedaService.listarCambioMoneda().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModalEliminar = function (idCambioMoneda) {
        $scope.id = idCambioMoneda;
    }

    $scope.eliminarCambioMoneda = function () {
        cambioMonedaService.eliminarCambioMoneda({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarCambioMoneda();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }

    $scope.editarCambioMoneda = function (cambioMoneda) {
        $scope.model = cambioMoneda;
    }
    //Fin

    $scope.listarComboMoneda();
    $scope.listarCambioMoneda();

}]);