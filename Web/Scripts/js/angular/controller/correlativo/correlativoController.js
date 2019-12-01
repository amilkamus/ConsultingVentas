app.controller('correlativoController', ['$rootScope', '$scope', 'correlativoService', function ($rootScope, $scope, correlativoService) {
    
    $scope.model = {};
    $scope.cmbTipoComprobante = [];
    $scope.elementos = { lista: [] };

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarCorrelativoMast();
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
    $scope.guardarCorrelativoMast = function () {
        var params = {
            correlativoViewModels: $scope.model
        };
        correlativoService.guardarCorrelativoMast(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};

                $scope.listarCorrelativoMast();
                $scope.listarComboTipoComprobante();

            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarCorrelativo').modal('hide')
    }
    //Fin

    //Listar Combo de TipoComprobante
    $scope.listarComboTipoComprobante = function () {
        correlativoService.comboTipoComprobante({}).then(function (data) {
            $scope.cmbTipoComprobante = data.data;
            $scope.model.idTipoComprobante = 1;
        });
    }
    //Fin

    /* ---------------------------------------- */

    //Listar
    $scope.listarCorrelativoMast = function () {
        correlativoService.listarCorrelativoMast().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModalEliminar = function (idCorrelativo) {
        $scope.id = idCorrelativo;
    }

    $scope.eliminarCorrelativoMast = function () {
        correlativoService.eliminarCorrelativoMast({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarCorrelativoMast();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }

    $scope.editarCorrelativoMast = function (correlativo) {
        $scope.model = correlativo;
    }
    //Fin

    $scope.listarComboTipoComprobante();
    $scope.listarCorrelativoMast();

}]);