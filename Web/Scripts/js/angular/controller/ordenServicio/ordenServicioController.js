app.controller('ordenServicioController', ['$rootScope', '$scope', 'ordenServicioService', function ($rootScope, $scope, ordenServicioService) {
    
    $scope.model = {};
    $scope.model.Cotizacion = {};
    $scope.cliente = {};
    $scope.elementosCotizacion = { lista: [] };
    $scope.elementosDetalle = { lista: [] };
    $scope.elementosCertificado = { lista: [] };
    $scope.productosDetalle = [];
    $scope.elementosInspeccion = { lista: [] };

    $scope.init = function () {
        $('#FechaEnvioMateriales,#FechaInspeccion').datepicker({ format: "dd/mm/yyyy", language: "es" });

        var idOrdenServicio = $("#IdNumeroOrdenServicio").attr("value");
        var idCotizacion = $("#IdCotizacion").attr("value");

        if (idOrdenServicio != undefined) {
            obtenerDatosOrdenServicio(idOrdenServicio, idCotizacion);            
        }
    }

    $scope.listarCotizacion = function () {
        $scope.elementosCotizacion = { lista: [] };
        ordenServicioService.listarCotizacion().then(function (data) {
            if (data.data.result_description) {
                $scope.elementosCotizacion.lista = data.data;
            } else {
                $scope.elementosCotizacion.lista = data.data;
            }
        });
    }

    $scope.seleccionCotizacion = function (item) {
        $scope.elementosDetalle = { lista: [] };
        $scope.elementosCertificado = { lista: [] };
        obtenerDatosCotizacion(item.ID);
    }

    $scope.validacion = function (form) {
        if (form.$valid) {

            if ($scope.model.EmailInspeccion != undefined && $scope.model.EmailInspeccion != "" && !isValidEmail($scope.model.EmailInspeccion)) {
                $scope.mensajeAlerta('El valor de email de inspección no tiene el formato correcto.');
                return;
            }

            var parametro = {
                ordenServicio: $scope.model
            }

            ordenServicioService.registrarOrdenServicio(parametro).then(function (data) {
                if (data.data) {
                    $scope.model.NumeroOrdenServicio = data.data.NumeroOrdenServicio;
                    $scope.mensajeExito(data.data.Mensaje);
                    $("#btnRegistraCotizacion").css("display", "none");
                } else {
                    $scope.mensajeAlerta('Ocurrió un error en el registro, contáctese con el administrador.');
                }
            });
        } else {
            $scope.mensajeAlerta('Ingrese todo los datos.');
        }
    }

    $scope.mensajeAlerta = function (mensaje) {
        $.notify({
            icon: 'fa fa-exclamation-circle',
            title: 'Alerta !',
            message: mensaje
        }, { type: 'warning', z_index: 2000 });
    }

    $scope.mensajeExito = function (mensaje) {
        $.notify({
            icon: 'fa fa-success-circle',
            title: 'Éxito !',
            message: mensaje
        }, { type: 'success', z_index: 2000 });
    }

    isValidEmail = function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

    obtenerDatosOrdenServicio = function (idOrdenServicio, idCotizacion) {
        var parametro = { id: idOrdenServicio }
        ordenServicioService.obtenerOrdenServicio(parametro).then(function (data) {
            if (data.data) {
                $scope.model = data.data;
                obtenerDatosCotizacion(idCotizacion);
            }
        });
    }

    obtenerDatosCotizacion = function (id) {
        var parametro = { id: id }
        ordenServicioService.obtenerCotizacion(parametro).then(function (data) {
            if (data.data) {
                $scope.model.NumeroCotizacion = data.data.Cotizacion.NumeroCotizacion;
                $scope.cliente.cliente = data.data.Cotizacion.Solicitante;
                $scope.cliente.numeroDocumento = data.data.Cotizacion.RUC;
                $scope.cliente.contacto = data.data.Cotizacion.Contacto;
                $scope.cliente.titularCorreo = data.data.Cotizacion.Email;
                $scope.model.Telefono = data.data.Cotizacion.Telefono;
                $scope.model.DescripcionProducto = data.data.Cotizacion.DescripcionProducto;
                $scope.model.CantidadMuestra = data.data.Cotizacion.CantidadMuestra;
                $scope.model.Cotizacion.Observaciones = data.data.Cotizacion.Observaciones;

                for (var i = 0; i < data.data.Certificados.length; i++) {
                    var itemDetalle = data.data.Certificados[i];
                    itemDetalle.IdCotizacion = $scope.model.IdCotizacion;
                    $scope.elementosCertificado.lista.push(itemDetalle);
                }

                for (var i = 0; i < data.data.Inspeccion.length; i++) {
                    var itemDetalle = data.data.Inspeccion[i];
                    itemDetalle.IdCotizacion = $scope.model.IdCotizacion;
                    $scope.elementosInspeccion.lista.push(itemDetalle);
                }

                for (var i = 0; i < data.data.Detalles.length; i++) {
                    var itemDetalle = data.data.Detalles[i];
                    itemDetalle.IdCotizacion = $scope.model.IdCotizacion;
                    itemDetalle.producto.cantidad = itemDetalle.productoCotizacion.Cantidad;
                    $scope.elementosDetalle.lista.push(itemDetalle);

                    var existeProducto = false;
                    for (var p = 0; p < $scope.productosDetalle.length; p++) {
                        if (itemDetalle.producto.idProducto == $scope.productosDetalle[p].idProducto) {
                            existeProducto = true;
                        }
                    }

                    if (!existeProducto) {
                        $scope.productosDetalle.push(itemDetalle.producto);
                    }

                }
            }
        });
    }

    $scope.listarCotizacion();
    $scope.init();

}]);