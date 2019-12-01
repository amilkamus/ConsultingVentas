app.controller('tipoPersonaController', ['$rootScope', '$scope', 'tipoPersonaService', function ($rootScope, $scope, tipoPersonaService) {

    $scope.model = {};
    $scope.elementos = { lista: [] };

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarTipoPersona();
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
    $scope.guardarTipoPersona = function () {
        var params = {
            tipoPersonaViewModel: $scope.model
        };
        tipoPersonaService.guardarTipoPersona(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};
                $scope.listarTipoPersona();
            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarTipoPersona').modal('hide')
    }
    //Fin

    /* ---------------------------------------- */
    
    //Listar
    $scope.listarTipoPersona = function () {
        tipoPersonaService.listarTipoPersona().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModalEliminar = function (idTipoPersona) {
        $scope.id = idTipoPersona;
    }

    $scope.eliminarTipoPersona = function () {
        tipoPersonaService.eliminarTipoPersona({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarTipoPersona();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }

    $scope.editarTipoPersona = function (tipoPersona) {
        $scope.model = tipoPersona;
    }
    //Fin
    
    $scope.listarTipoPersona();

}]);