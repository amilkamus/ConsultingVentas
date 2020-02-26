app.controller('cotizacionController', ['$rootScope', '$scope', 'cotizacionService', function ($rootScope, $scope, cotizacionService) {

    $scope.model = {};

    $scope.elementosCliente = { lista: [] };
    $scope.elementosProducto = { lista: [] };
    $scope.elementosParametro = { lista: [] };
    $scope.elementosTipoParametro = { lista: [] };
    $scope.elementosDetalle = { lista: [] };
    $scope.elementosCertificado = { lista: [] };
    $scope.elementosInspeccion = { lista: [] };
    $scope.elementosResumen = { lista: [] };
    $scope.elementosMoneda = [];
    $scope.elementosTipoCotizacion = [];
    $scope.cliente = {};
    $scope.producto = {};
    $scope.productosDetalle = [];
    $scope.parametro = {};
    $scope.tipoParametro = {};
    $scope.certificado = {};
    $scope.inspeccion = {};
    $scope.resumen = {};
    $scope.model.FlagControles = false;

    $scope.init = function () {

        $('#Fecha').datepicker({
            format: "dd/mm/yyyy",
            language: "es"
        });

        var parametro = $("#idDetalle").attr("value");

        if (parametro != undefined) {
            var parametro = { id: $("#idDetalle").attr("value") }
            cotizacionService.obtenerCotizacion(parametro).then(function (data) {
                if (data.data) {
                    $scope.model = data.data.Cotizacion;
                    $scope.model.Fecha = data.data.Cotizacion.Fecha;
                    $scope.cliente.numeroDocumento = data.data.Cotizacion.RUC;
                    $scope.cliente.contacto = data.data.Cotizacion.Contacto;
                    $scope.cliente.contactoCorreo = data.data.Cotizacion.Email;
                    $scope.cliente.cliente = data.data.Cotizacion.Solicitante;

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

                    for (var i = 0; i < data.data.Resumen.length; i++) {
                        var itemDetalle = data.data.Resumen[i];
                        itemDetalle.IdCotizacion = $scope.model.IdCotizacion;
                        $scope.elementosResumen.lista.push(itemDetalle);
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
                } else {
                    $scope.mensajeAlerta('Ocurrió un error en el registro, contáctese con el administrador.');
                }
            });
        } else {
            var current_datetime = new Date()
            var fecha = appendLeadingZeroes(current_datetime.getDate()) + "/" + appendLeadingZeroes(current_datetime.getMonth() + 1) + "/" + current_datetime.getFullYear();
            $scope.model.Fecha = fecha;
            $scope.model.SubTotal = parseFloat("0").toFixed(2);
            $scope.model.PorcentajeDescuento = parseFloat("0");
            $scope.model.MontoDescuento = parseFloat("0").toFixed(2);
            $scope.model.SubTotalFinal = parseFloat("0").toFixed(2);
            $scope.model.IGV = parseFloat("0").toFixed(2);
            $scope.model.Total = parseFloat("0").toFixed(2);

            $scope.model.Banco = "BANCO INTERBANK";
            $scope.model.CuentaCorriente = "------";
            $scope.model.CuentaAhorro = "091-310627109-5";
            $scope.model.CCI = "003-091-013106271095-62";
            $scope.model.Detracciones = "12% Banco de la Nación 00-004-130979 - TIPO BIEN / SERVICIO: 037";
        }
    }

    appendLeadingZeroes = function (n) {
        if (n <= 9) {
            return "0" + n;
        }
        return n
    }

    isValidEmail = function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

    $scope.formatearDecimales = function (valor) {
        return parseFloat(valor).toFixed(2);
    }

    $scope.habilitarNroDias = function () {
        $("#txtActivarNroDias").attr("disabled", !$scope.resumen.HabilitarNroDias);
    }

    $scope.mostrarSubtotalCreacion = function (idProducto) {
        var lista = $scope.elementosDetalle.lista;
        var subtotal = 0;
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].producto.idProducto == idProducto) {
                var cantidad = parseInt(lista[i].producto.cantidad);
                var precio = parseFloat(lista[i].parametro.Precio);
                subtotal += cantidad * precio;
            }
        }
        return subtotal;
    }

    $scope.mostrarSubtotalEdicion = function (idProducto) {
        var lista = $scope.elementosDetalle.lista;
        var subtotal = 0;
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].producto.idProducto == idProducto) {
                var cantidad = parseInt(lista[i].producto.cantidad);
                var precio = parseFloat(lista[i].Precio);
                subtotal += cantidad * precio;
            }
        }
        return subtotal;
    }

    $scope.listarComboMoneda = function () {
        cotizacionService.comboTipoMoneda().then(function (data) {
            $scope.elementosMoneda = data.data;
        });
    }

    $scope.listarTipoCotizacion = function () {
        cotizacionService.listarTipoCotizacion().then(function (data) {
            $scope.elementosTipoCotizacion = data.data;
        });
    }

    $scope.listarCliente = function () {
        cotizacionService.listarCliente().then(function (data) {
            if (data.data.result_description) {
                $scope.elementosCliente.lista = data.data;
            } else {
                $scope.elementosCliente.lista = data.data;
            }
        });
    }

    $scope.listarProducto = function () {
        cotizacionService.listarProducto().then(function (data) {
            if (data.data) {
                $scope.elementosProducto.lista = data.data;
            } else {
                $scope.elementosProducto.lista = data.data;
            }
        });
    }

    $scope.listarParametro = function () {
        cotizacionService.listarParametro().then(function (data) {
            if (data.data) {
                $scope.elementosParametro.lista = data.data;
            } else {
                $scope.elementosParametro.lista = data.data;
            }
        });
    }

    $scope.listarTipoParametro = function () {
        cotizacionService.listarTipoParametro().then(function (data) {
            if (data.data) {
                $scope.elementosTipoParametro.lista = data.data;
            } else {
                $scope.elementosTipoParametro.lista = data.data;
            }
        });
    }

    $scope.seleccionProducto = function (itemProducto) {
        $scope.producto = itemProducto;
    }

    $scope.seleccionCliente = function (itemCliente) {
        $scope.cliente = itemCliente;
    }

    $scope.seleccionParametro = function (itemParametro) {
        $scope.parametro = itemParametro;
    }

    $scope.seleccionTipoParametro = function (itemTipoParametro) {
        $scope.tipoParametro = itemTipoParametro;
    }

    $scope.validacion = function (form) {
        if (form.$valid) {
            if ($scope.elementosDetalle.lista.length == 0) {
                $scope.mensajeAlerta('No se han adicionado productos a analizar.');
                return;
            }

            if (!isValidEmail($scope.model.CorreoConfirmacion)) {
                $scope.mensajeAlerta('El valor de correo de confirmación no tiene el formato correcto.');
                return;
            }

            $scope.model.RUC = $scope.cliente.numeroDocumento;
            $scope.model.Contacto = $scope.cliente.contacto;
            $scope.model.Email = $scope.cliente.contactoCorreo;
            $scope.model.Solicitante = $scope.cliente.cliente;
            $scope.model.Certificados = $scope.elementosCertificado.lista;

            var lista = [];

            var parametro = $("#idDetalle").attr("value");

            if (parametro != undefined) {
                for (var i = 0; i < $scope.elementosDetalle.lista.length; i++) {
                    var item = $scope.elementosDetalle.lista[i];
                    var parametro = $("#idDetalle").attr("value");
                    var itemDetalle = {
                        IdCotizacion: item.IdCotizacion,
                        IdProducto: item.producto.idProducto,
                        IdTipoParametro: item.tipoParametro.ID,
                        IdParametro: item.parametro.ID,
                        Cantidad: item.producto.cantidad,
                        Precio: item.Precio
                    }
                    lista.push(itemDetalle);
                }
            } else {
                for (var i = 0; i < $scope.elementosDetalle.lista.length; i++) {
                    var item = $scope.elementosDetalle.lista[i];
                    var parametro = $("#idDetalle").attr("value");
                    var itemDetalle = {
                        IdCotizacion: item.IdCotizacion,
                        IdProducto: item.producto.idProducto,
                        IdTipoParametro: item.tipoParametro.ID,
                        IdParametro: item.parametro.ID,
                        Cantidad: item.producto.cantidad,
                        Precio: item.parametro.Precio
                    }
                    lista.push(itemDetalle);
                }
            }

            var parametro = {
                Cotizacion: $scope.model,
                Certificados: $scope.elementosCertificado.lista,
                Productos: lista,
                Inspeccion: $scope.elementosInspeccion.lista,
                Resumen: $scope.elementosResumen.lista
            }

            cotizacionService.registrarCotizacion(parametro).then(function (data) {
                if (data.data) {
                    $scope.model.NumeroCotizacion = data.data.NumeroCotizacion;
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

    $scope.registrarComprobante = function (idCotizacion) {

        var parametro = {
            id: idCotizacion
        };

        $("#myModal").css("display", "block");
        cotizacionService.generarComprobante(parametro).then(function (data) {
            if (data.data) {
                if (data.data.Codigo == "0") {
                    $("#linkRegistrarComprobante").css("display", "none");
                    $scope.mensajeExito(data.data.Descripcion);
                } else {
                    $scope.mensajeAlerta(data.data.Descripcion);
                }
            } else {
                $scope.mensajeAlerta('Ocurrió un error en el registro, contáctese con el administrador.');
            }
            $("#myModal").css("display", "none");
        });

    }

    $scope.agregarDetalle = function () {
        var lista = $scope.elementosDetalle.lista;
        var existe = false;
        var itemDetalle = null;
        var index = -1;

        if (!$scope.model.FlagControles) {
            if ($.isEmptyObject($scope.producto) || $.isEmptyObject($scope.parametro) || $.isEmptyObject($scope.tipoParametro)) {
                $scope.mensajeAlerta('Debe ingresar todos los datos del detalle a analizar.');
                return;
            }
            if ($scope.parametro.Precio == undefined || $scope.parametro.Precio == "") {
                $scope.mensajeAlerta('Debe ingresar el precio del detalle a analizar');
                return
            }

            if (parseFloat($scope.parametro.Precio) <= 0) {
                $scope.mensajeAlerta('Debe ingresar un valor mayor a cero para el precio del detalle.');
                return;
            }

            if ($scope.producto.cantidad == undefined || parseFloat($scope.producto.cantidad) == 0) {
                $scope.mensajeAlerta('Debe ingresar el número de muestras del detalle.');
                return;
            }

            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                if (item.producto.idProducto == $scope.producto.idProducto && item.parametro.ID == $scope.parametro.ID) {
                    itemDetalle = item;
                    existe = true;
                    index = i;
                    break;
                }
            }

            var existeProducto = false;
            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                if (item.producto.idProducto == $scope.producto.idProducto) {
                    existeProducto = true;
                    break;
                }
            }
            if (!existeProducto) {
                $scope.productosDetalle.push($scope.producto);
            }

            var objetoDetalle = {
                producto: $scope.producto,
                parametro: $scope.parametro,
                tipoParametro: $scope.tipoParametro
            }

            if (!existe) {
                objetoDetalle.Precio = $scope.parametro.Precio;
                objetoDetalle.IdCotizacion = $scope.model.IdCotizacion;
                $scope.elementosDetalle.lista.push(objetoDetalle);
            } else {
                var cantidad = parseInt(itemDetalle.producto.cantidad) + parseInt($scope.producto.cantidad);
                itemDetalle = objetoDetalle;
                itemDetalle.Precio = $scope.parametro.Precio;
                itemDetalle.producto.cantidad = cantidad;
                itemDetalle.IdCotizacion = $scope.model.IdCotizacion;
                $scope.elementosDetalle.lista.splice(index, 1, itemDetalle);
            }

            $scope.recalcularSubtotal();
            $scope.producto = {};
            $scope.parametro = {};
            $scope.tipoParametro = {};

        } else {
            if ($.isEmptyObject($scope.producto)) {
                $scope.mensajeAlerta('Debe ingresar todos los datos del detalle a analizar.');
                return;
            }
            
            // obtener lista de parámetros y tipo de parámetros
            // recorrer la lista de parámetros e ir agregándolos
            cotizacionService.listarParametrosProducto({ id: $scope.producto.idProducto }).then(function (data) {
                if (data.data && data.data.Detalles.length > 0) {

                    var existeProducto = false;
                    for (var i = 0; i < lista.length; i++) {
                        var item = lista[i];
                        if (item.producto.idProducto == $scope.producto.idProducto) {
                            existeProducto = true;
                            break;
                        }
                    }
                    if (!existeProducto) {
                        $scope.productosDetalle.push($scope.producto);
                    }

                    for (var i = 0; i < data.data.Detalles.length; i++) {
                        lista = $scope.elementosDetalle.lista;

                        var detalle = data.data.Detalles[i];
                        detalle.producto.cantidad = 1;

                        var objetoDetalle = {
                            producto: detalle.producto,
                            parametro: detalle.parametro,
                            tipoParametro: detalle.tipoParametro
                        }

                        for (var x = 0; x < lista.length; x++) {
                            var item = lista[x];
                            if (item.producto.idProducto == detalle.producto.idProducto && item.parametro.ID == detalle.parametro.ID) {
                                itemDetalle = item;
                                existe = true;
                                index = x;
                                break;
                            }
                        }

                        if (!existe) {
                            objetoDetalle.Precio = detalle.parametro.Precio;
                            objetoDetalle.IdCotizacion = $scope.model.IdCotizacion;
                            $scope.elementosDetalle.lista.push(objetoDetalle);
                        } else {
                            var cantidad = parseInt(itemDetalle.producto.cantidad) + parseInt(detalle.producto.cantidad);
                            itemDetalle = objetoDetalle;
                            itemDetalle.Precio = detalle.parametro.Precio;
                            itemDetalle.producto.cantidad = cantidad;
                            itemDetalle.IdCotizacion = $scope.model.IdCotizacion;
                            $scope.elementosDetalle.lista.splice(index, 1, itemDetalle);
                        }
                    }

                    $scope.recalcularSubtotal();
                    $scope.calculcarDescuento($scope.model.PorcentajeDescuento);
                    $scope.producto = {};
                    $scope.parametro = {};
                    $scope.tipoParametro = {};
                } else {
                    $scope.mensajeAlerta("El producto seleccionado no tiene parámetros asociados.");
                }
            });
        }
    }

    $scope.agregarCertificado = function () {
        var certificado = $scope.certificado;

        if (certificado.Documento == undefined || certificado.Precio == undefined || certificado.NormaReferencia == undefined ||
            $.trim(certificado.Documento) == "" || $.trim(certificado.NormaReferencia) == "") {
            $scope.mensajeAlerta('Debe ingresar todos los datos del certificado.');
            return;
        }

        if (parseFloat(certificado.Precio) <= 0) {
            $scope.mensajeAlerta('El precio del certificado no puede ser negativo.');
            return;
        }

        $scope.certificado.IdCotizacion = $scope.model.IdCotizacion;
        $scope.elementosCertificado.lista.push($scope.certificado);
        $scope.recalcularSubtotal();
        $scope.certificado = {};
    }

    $scope.agregarInspeccion = function () {
        $scope.inspeccion.Subtotal = parseInt($scope.inspeccion.Cantidad) * parseFloat($scope.inspeccion.Precio);
        $scope.elementosInspeccion.lista.push($scope.inspeccion);
        $scope.recalcularSubtotal();
        $scope.inspeccion = {};
    }

    $scope.agregarResumen = function () {

        if ($.isEmptyObject($scope.resumen)) {
            $scope.mensajeAlerta("Debe ingresar todos los datos necesarios para el resumen.");
            return;
        }

        if ($scope.resumen.Descripcion == undefined || $scope.resumen.Descripcion == "") {
            $scope.mensajeAlerta('Debe ingresar la descripción del resumen.');
            return
        }

        if ($scope.resumen.Precio == undefined || $scope.resumen.Precio == "") {
            $scope.mensajeAlerta('Debe ingresar el precio del resumen.');
            return
        }

        if (parseFloat($scope.resumen.Precio) <= 0) {
            $scope.mensajeAlerta('Debe ingresar un valor mayor a cero para el precio del resumen.');
            return;
        }

        if ($scope.resumen.HabilitarNroDias) {
            if ($scope.resumen.NumeroDias == undefined || $scope.resumen.NumeroDias == "") {
                $scope.mensajeAlerta('Debe ingresar el número de días del resumen.');
                return
            }

            if (parseInt($scope.resumen.NumeroDias) <= 0) {
                $scope.mensajeAlerta('Debe ingresar un valor mayor a cero para el número de días del resumen.');
                return;
            }

            $scope.resumen.Total = parseInt($scope.resumen.NumeroDias) * parseFloat($scope.resumen.Precio);
        } else {
            $scope.resumen.NumeroDias = "-";
            $scope.resumen.Total = parseFloat($scope.resumen.Precio);
        }

        $scope.elementosResumen.lista.push($scope.resumen);
        $scope.recalcularSubtotal();
        $scope.resumen = {};
    }

    $scope.quitarDetalle = function (index) {
        var idProducto = $scope.elementosDetalle.lista[index].producto.idProducto;
        $scope.elementosDetalle.lista.splice(index, 1);

        var indexProducto = -1;
        for (var i = 0; i < $scope.productosDetalle.length; i++) {
            if (idProducto == $scope.productosDetalle[i].idProducto) {
                indexProducto = i;
            }
        }

        var existeProducto = false;
        for (var i = 0; i < $scope.elementosDetalle.lista.length; i++) {
            if (idProducto == $scope.elementosDetalle.lista[i].producto.idProducto) {
                existeProducto = true;
            }
        }

        if (!existeProducto) {
            $scope.productosDetalle.splice(indexProducto, 1);
        }

        $scope.recalcularSubtotal();
        $scope.calculcarDescuento($scope.model.PorcentajeDescuento);
    }

    $scope.quitarCertificado = function (index) {
        $scope.elementosCertificado.lista.splice(index, 1);
        $scope.recalcularSubtotal();
    }

    $scope.quitarInspeccion = function (index) {
        $scope.elementosInspeccion.lista.splice(index, 1);
        $scope.recalcularSubtotal();
    }

    $scope.quitarResumen = function (index) {
        $scope.elementosResumen.lista.splice(index, 1);
        $scope.recalcularSubtotal();
    }

    $scope.calculcarDescuento = function (valor) {
        if (valor != "") {
            console.log(valor);
            var porcentaje = parseFloat(valor);
            var subtotal = parseFloat($scope.model.SubTotal);
            var montoDescuento = subtotal * (porcentaje / 100);
            $scope.model.MontoDescuento = montoDescuento.toFixed(2);
        } else {
            $scope.model.MontoDescuento = 0;
        }
        $scope.recalcularSubtotal();
    }

    $scope.recalcularSubtotal = function () {
        var subtotal = 0.00;
        var parametro = $("#idDetalle").attr("value");

        if (parametro != undefined) {
            for (var i = 0; i < $scope.elementosDetalle.lista.length; i++) {
                var item = $scope.elementosDetalle.lista[i]
                var precio = parseFloat(item.Precio) * parseInt(item.producto.cantidad);
                subtotal += parseFloat(precio);
            }
        } else {
            for (var i = 0; i < $scope.elementosDetalle.lista.length; i++) {
                var item = $scope.elementosDetalle.lista[i]
                var precio = parseFloat(item.parametro.Precio) * parseInt(item.producto.cantidad);
                subtotal += parseFloat(precio);
            }
        }

        for (var i = 0; i < $scope.elementosCertificado.lista.length; i++) {
            var item = $scope.elementosCertificado.lista[i]
            var precio = parseFloat(item.Precio) * 1;
            subtotal += parseFloat(precio);
        }

        for (var i = 0; i < $scope.elementosInspeccion.lista.length; i++) {
            var item = $scope.elementosInspeccion.lista[i]
            var precio = parseFloat(item.Subtotal);
            subtotal += parseFloat(precio);
        }

        for (var i = 0; i < $scope.elementosResumen.lista.length; i++) {
            var item = $scope.elementosResumen.lista[i]
            var precio = parseFloat(item.Total);
            subtotal += parseFloat(precio);
        }

        $scope.model.SubTotal = subtotal.toFixed(2);
        subtotal = subtotal - parseFloat($scope.model.MontoDescuento);

        var igv = parseFloat(subtotal) * 0.18;
        var total = parseFloat(subtotal) + parseFloat(igv);

        $scope.model.SubTotalFinal = subtotal.toFixed(2);
        $scope.model.IGV = igv.toFixed(2);
        $scope.model.Total = total.toFixed(2);
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

    $scope.listarCliente();
    $scope.listarProducto();
    $scope.listarParametro();
    $scope.listarTipoParametro();
    $scope.listarComboMoneda();
    $scope.listarTipoCotizacion();
    $scope.init();
}]);