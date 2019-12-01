app.controller('cajaController', ['$rootScope', '$scope', 'cajaService', function ($rootScope, $scope, cajaService) {

    $scope.model = {};
    $scope.elementos = { lista: [] };

    $scope.model.estado = "ACTIVO";

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarCaja();
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
    $scope.guardarCaja = function () {
        var params = {
            cajaViewModels: $scope.model
        };
        cajaService.guardarCaja(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};
                $scope.model.estado = "ACTIVO";
                $scope.listarCaja();
            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarCaja').modal('hide')
    }
    //Fin

    /* ---------------------------------------- */
    
    //Listar
    $scope.listarCaja = function () {
        cajaService.listarCaja().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModalEliminar = function (idCaja) {
        $scope.id = idCaja;
    }

    $scope.eliminarCaja = function () {
        cajaService.eliminarCaja({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarCaja();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }

    $scope.editarCaja = function (caja) {
        $scope.model = caja;
    }
    //Fin

    $scope.listarCaja();

}]);