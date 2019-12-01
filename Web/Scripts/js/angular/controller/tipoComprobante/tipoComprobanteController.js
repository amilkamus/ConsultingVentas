app.controller('tipoComprobanteController', ['$rootScope', '$scope', 'tipoComprobanteService', function ($rootScope, $scope, tipoComprobanteService) {

    $scope.model = {};
    $scope.elementos = { lista: [] };
    
    $scope.model.estado = "ACTIVO";

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarTipoComprobante();
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
    $scope.guardarTipoComprobante = function () {
        var params = {
            tipoComprobanteViewModel: $scope.model
        };
        tipoComprobanteService.guardarTipoComprobante(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};
                $scope.model.estado = "ACTIVO";
                $scope.listarTipoComprobante();
            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarTipoComprobante').modal('hide')
    }
    //Fin

    /* ---------------------------------------- */

    //Listar
    $scope.listarTipoComprobante = function () {
        tipoComprobanteService.listarTipoComprobante().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModalEliminar = function (idTipoComprobante) {
        $scope.id = idTipoComprobante;
    }

    $scope.eliminarTipoComprobante = function () {
        tipoComprobanteService.eliminarTipoComprobante({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarTipoComprobante();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }

    $scope.editarTipoComprobante = function (tipoComprobante) {
        $scope.model = tipoComprobante;
    }
    //Fin

    $scope.listarTipoComprobante();
    
}]);