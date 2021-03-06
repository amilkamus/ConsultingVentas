﻿app.controller('productoController', ['$rootScope', '$scope', 'productoService', function ($rootScope, $scope, productoService) {

    $scope.model = {};
    $scope.cmbTipoProducto = [];
    $scope.elementos = { lista: [] };
    $scope.model.estado = "ACTIVO";
    $scope.elementosParametro = { lista: [] };

    //Validacion para el guardar
    $scope.validacion = function (form) {
        if (form.$valid) {
            $scope.guardarProducto();
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
    $scope.guardarProducto = function () {

        var listaID = [];
        listaID.push(0);

        for (i = 0; i < $scope.elementosParametro.lista.length; i++) {
            if ($scope.elementosParametro.lista[i].Activo) {
                listaID.push($scope.elementosParametro.lista[i].ID);
            }
        }

        var params = {
            productoViewModels: $scope.model,
            Parametros: listaID //$scope.elementosParametro.lista
        };
        productoService.guardarProducto(params).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.model = {};
                $scope.model.estado = "ACTIVO";
                $scope.listarProductos();
                $scope.listarComboTipoProducto();
                $scope.listarParametro(0);
            } else {
                $scope.messageError(data.data.error);
            }
        });
        $('#modal-editarProducto').modal('hide')
    }
    //Fin

    //Cargar Combo Tipo Ptoducto
    $scope.listarComboTipoProducto = function () {
        productoService.comboTipoProducto({}).then(function (data) {
            $scope.cmbTipoProducto = data.data;
            $scope.model.idTipoProductoServicio = 2;
        });
    }
    //Fin

    /* ---------------------------------------- */

    //Listar
    $scope.listarProductos = function () {
        productoService.listarProducto({}).then(function (data) {
            $scope.elementos.lista = data.data;
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModalEliminar = function (idProducto) {
        $scope.id = idProducto;
    }

    $scope.eliminarProducto = function () {
        productoService.eliminarProducto({ id: $scope.id }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarProductos();
            } else {
                $scope.messageError(data.data.error);
            }
        });
    }

    $scope.editarProducto = function (producto) {
        $scope.listarParametro(producto.idProducto);
        $scope.model = producto;
    }

    $scope.listarParametro = function (valor) {
        productoService.listarParametro({ id: valor }).then(function (data) {
            if (data.data) {
                $scope.elementosParametro.lista = data.data;
            } else {
                $scope.elementosParametro.lista = data.data;
            }
        });
    }

    //Fin

    $scope.listarComboTipoProducto();
    $scope.listarProductos();
    $scope.listarParametro(0);
}]);