app.controller('tipoProductoController', ['$rootScope', '$scope', 'tipoProductoService', function ($rootScope, $scope, tipoProductoService) {

    $scope.model = {};
    $scope.elementos = { lista: [] };
    
    $scope.model.estado = 'ACTIVO';

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarTipoProducto();
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
    $scope.guardarTipoProducto = function () {
        var params = {
            tipoProductoServicioViewModels: $scope.model
        };
        tipoProductoService.guardarTipoProducto(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};
                $scope.model.estado = 'ACTIVO';
                $scope.listarTipoProducto();
            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarTipoProducto').modal('hide')
    }
    //Fin

    /* ---------------------------------------- */

    //Listar
    $scope.listarTipoProducto = function () {
        tipoProductoService.listarTipoProducto().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModalEliminar = function (idTipoProductoServicio) {
        $scope.id = idTipoProductoServicio;
    }

    $scope.eliminarTipoProducto = function () {
        tipoProductoService.eliminarTipoProducto({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarTipoProducto();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }

    $scope.editarTipoProducto = function (tipoProducto) {
        $scope.model = tipoProducto;
    }
    //Fin

    $scope.listarTipoProducto();

}]);