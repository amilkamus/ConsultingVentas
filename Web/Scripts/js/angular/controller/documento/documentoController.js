app.controller('documentoController', ['$rootScope', '$scope', 'documentoService', function ($rootScope, $scope, documentoService) {
    $scope.model = {};
    $scope.elementos = { lista: [] };

    $scope.model.estado = 'ACTIVO';

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarDocumento();
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
    $scope.guardarDocumento = function () {
        var params = {
            documentoViewModel: $scope.model
        };
        documentoService.guardarDocumento(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};
                $scope.model.estado = 'ACTIVO';
                $scope.listarDocumento();
            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarDocumento').modal('hide')
    }
    //Fin

    /* ---------------------------------------- */

    //Listar
    $scope.listarDocumento = function () {        
        documentoService.listarDocumento().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;                
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModalEliminar = function (idTipoDocumento) {
        $scope.id = idTipoDocumento;
    }

    $scope.eliminarDocumento = function () {
        documentoService.eliminarDocumento({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarDocumento();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }

    $scope.editarDocumento = function (documento) {
        $scope.model = documento;
    }
    //Fin

    $scope.listarDocumento();

}]);