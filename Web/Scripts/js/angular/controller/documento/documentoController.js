app.controller('documentoController', ['$rootScope', '$scope', 'documentoService', function ($rootScope, $scope, documentoService) {
    $scope.model = {};
    $scope.elementos = { lista: [] };
    $scope.elementosCotizacion = { lista: [] };
    $scope.elementosOrdenServicio = { lista: [] };
    $scope.elementosUsuario = { lista: [] };
    $scope.elementosCobranza = { lista: [] };
    $scope.model.estado = 'ACTIVO';

    $scope.init = function () {

        $('.txtFecha').datepicker({
            todayHighlight: true,
            format: "dd/mm/yyyy",
            language: "es",
            autoclose: true,
            endDate: "-1d"
        });

        var current_datetime = new Date()
        var fecha = appendLeadingZeroes(current_datetime.getDate()) + "/" + appendLeadingZeroes(current_datetime.getMonth() + 1) + "/" + current_datetime.getFullYear();
        $scope.model.FechaInicio = fecha;
        $scope.model.FechaFin = fecha;

        $scope.model.TipoCotizacion = "0";
        $scope.model.IdUsuarioRegistro = "0";
    }

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

    $scope.listarCotizaciones = function () {
        $("#myModal").css("display", "block");
        documentoService.listarCotizaciones($scope.model).then(function (data) {
            if (data.data) {
                $scope.elementosCotizacion.lista = data.data;
            } else {
                $scope.elementosCotizacion.lista = data.data;
            }
            $("#myModal").css("display", "none");
        });
    }
    
    $scope.listarOrdenesServicio = function () {
        $("#myModal").css("display", "block");
        documentoService.listarOrdenesServicio($scope.model).then(function (data) {
            if (data.data) {
                $scope.elementosOrdenServicio.lista = data.data;
            } else {
                $scope.elementosOrdenServicio.lista = data.data;
            }
            $("#myModal").css("display", "none");
        });
    }

    $scope.listarCobranzas = function () {
        $("#myModal").css("display", "block");
        documentoService.listarCobranzas($scope.model).then(function (data) {
            if (data.data) {
                $scope.elementosCobranza.lista = data.data;
            } else {
                $scope.elementosCobranza.lista = data.data;
            }
            $("#myModal").css("display", "none");
        });
    }

    $scope.listarUsuarios = function () {
        documentoService.listarUsuarios().then(function (data) {
            if (data.data) {
                $scope.elementosUsuario.lista = data.data;
            } else {
                $scope.elementosUsuario.lista = data.data;
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

    appendLeadingZeroes = function (n) {
        if (n <= 9) {
            return "0" + n;
        }
        return n
    }

    $scope.init();
    $scope.listarUsuarios();
    $scope.listarDocumento();
    $scope.listarCotizaciones();
    $scope.listarOrdenesServicio();
    $scope.listarCobranzas();

}]);