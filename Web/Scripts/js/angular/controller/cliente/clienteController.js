app.controller('clienteController', ['$rootScope', '$scope', 'clienteService', function ($rootScope, $scope, clienteService) {

    $scope.model = {};

    $scope.cmbTipoDocumento = [];
    $scope.cmbTipoPersona = [];
    $scope.elementos = { lista: [] };

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarCliente();
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta !',
                message: 'Ingrese todo los datos.'
            }, { type: 'warning', z_index: 2000 });
        }
    }
    //Fin

    $scope.desabilitarCamposDatosEmpresa = true;
    $scope.desabilitarCamposDatosTitular = true;
    $scope.requeridoEmpresa = true;
    $scope.requeridoTitular = true;

    //Guardar
    $scope.guardarCliente = function () {
        var params = {
            clienteViewModel: $scope.model
        };
        clienteService.guardarCliente(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};

                $scope.listarComboTipoDocumento();
                $scope.listarComboTipoPersona();
                $scope.listarCliente();

            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarCliente').modal('hide')
    }
    //Fin

    //Listar Combo de TipoDocumento/TipoPersona
    $scope.listarComboTipoDocumento = function () {
        clienteService.comboTipoDocumento({}).then(function (data) {
            $scope.cmbTipoDocumento = data.data;
            $scope.model.empresaIdTipoDocumento = 1;
            $scope.model.titularIdTipoDocumento = 1;
        });
    }

    $scope.listarComboTipoPersona = function () {
        clienteService.comboTipoPersona({}).then(function (data) {
            $scope.cmbTipoPersona = data.data;
            $scope.model.idTipoPersona = 1;

            $scope.desabilitarCampos();
        });
    }
    //Fin

    //Deshabilitar y Habilitar los cuadros de textos
    $scope.desabilitarCampos = function () {
        if ($scope.model.idTipoPersona == 1) {
            $scope.model = {};
            $scope.model.empresaNombre = "";

            $scope.listarComboTipoDocumento();

            $scope.desabilitarCamposDatosTitular = false; //abilitar
            $scope.desabilitarCamposDatosEmpresa = true; //desabilitar

            $scope.requeridoEmpresa = false;
            $scope.requeridoTitular = true;
            $scope.model.idTipoPersona = 1;
        }
        if ($scope.model.idTipoPersona == 2) {
            $scope.model = {};

            $scope.listarComboTipoDocumento();

            $scope.desabilitarCamposDatosTitular = false; //abilitar
            $scope.desabilitarCamposDatosEmpresa = false; //abilitar

            $scope.requeridoEmpresa = true;
            $scope.requeridoTitular = false;
            $scope.model.idTipoPersona = 2;
        }
    }
    $scope.habilitarCamposDatosEmpresa = function () {
        $scope.desabilitarCamposDatosEmpresa = false;
    }
    //Fin

    /* ---------------------------------------- */

    //Listar
    $scope.listarCliente = function () {
        clienteService.listarCliente({}).then(function (data) {
            if (data.data.result_description) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Modal de Eliminar/Editar/Ver
    $scope.cargarModalEliminar = function (idCliente) {
        $scope.id = idCliente;
    }
    $scope.eliminarCliente = function () {
        clienteService.eliminarCliente({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarCliente();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }
    $scope.editarCliente = function (cliente) {
        $scope.model = cliente;
        //$scope.modelContacto = cliente;
        //$scope.modelEmpresa = cliente;
        //$scope.modelTitular = cliente;
    }
    $scope.verCliente = function (cliente) {
        $scope.model = cliente;

        if (cliente.tipoPersona == 'NATURAL') {
            $('#empresa').hide()
            $('#contacto').show()
        }
        if (cliente.tipoPersona == 'JURIDICA') {
            $('#contacto').hide()
            $('#empresa').show()
        }
    }
    //Fin

    $scope.listarComboTipoDocumento();
    $scope.listarComboTipoPersona();
    $scope.listarCliente();

}]);