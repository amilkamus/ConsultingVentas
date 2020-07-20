app.controller('comprobanteController', ['$rootScope', '$scope', 'comprobanteService', function ($rootScope, $scope, comprobanteService) {

    $scope.model = {};
    $scope.modelDetalle = {};
    $scope.cmbTipoComprobante = [];
    $scope.cmbTipoMoneda = [];
    $scope.cmbIGV = [];
    $scope.elementos = { lista: [] };
    $scope.elementosDetalle = { lista: [] };
    $scope.elementosCliente = { lista: [] };
    $scope.elementosProducto = { lista: [] };
    $scope.elementosCambioMoneda = { lista: [] };
    $scope.elementosCorrelativo = { lista: [] };
    $scope.carrito = { lista: [] };

    $scope.elementosParametro = { lista: [] };///////////

    $scope.validar = true;

    $scope.soles = 0.00;
    $scope.solesTotal = 0.00;
    $scope.igv = 0.00;

    $scope.cliente = {};
    $scope.producto = {};

    $scope.parametro = {};

    $scope.init = function () {
        $scope.cliente.cliente = "";
        $scope.cliente.numeroDocumento = "";
        $scope.cliente.contacto = "";
        $scope.producto.codigo = "";
        $scope.producto.nombre = "";
        $scope.producto.precio = "";
        $scope.producto.tipoProducto = "";
        $scope.producto.stock = "";
        $scope.producto.cantidad = "";

        $scope.parametro.CodParametro = ""; /////////////////////////
        $scope.parametro.ParametroDescripcion = ""; /////////////////
        $scope.parametro.Metodologia = "";  /////////////////////////

        $scope.model.subtotal = 0.00;
        $scope.model.total = 0.00;
        $('#dolaresSubtotal').hide()
        $('#solesSubtotal').show()
        $('#dolaresTotal').hide()
        $('#solesTotal').show()
    }

    // Guardar
    $scope.guardarComprobante = function () {
        $scope.model.idCliente = $scope.cliente.idCliente;
        $scope.buscarCorrelativo($scope.model.idTipoComprobante);

        var params = {
            ventaViewModels: $scope.model,
            ventaDetalleViewModels: $scope.carrito.lista,
            productoViewModels: $scope.elementosProducto.lista
        };
        if ($scope.carrito.lista.length != 0) {
            if ($scope.cliente.cliente != "") {
                comprobanteService.guardarComprobante(params).then(function (data) {
                    if (data.data.result_description) {
                        $scope.messageSuccess(data.data.result_description);
                        $scope.model = {};
                        $scope.cliente = {};
                        $scope.carrito = { lista: [] };
                        $scope.listarComboTipoComprobante();
                        $scope.listarComboTipoMoneda();
                        $scope.listarComboIGV();
                        $scope.soles = 0.00;
                    } else {
                        $scope.messageError(data.data.error);
                    }
                });
            } else {
                //$scope.carrito = { lista: [] };
                $.notify({
                    icon: 'fa fa-exclamation-circle',
                    title: 'Alerta !',
                    message: 'No se puede guardar porque no hay cliente seleccionado'
                }, { type: 'warning', z_index: 2000 });
                $scope.producto = {};
            }
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta !',
                message: 'No se puede guardar porque no hay productos agregados al carrito'
            }, { type: 'warning', z_index: 2000 });
            $scope.producto = {};
        }
    }
    //Fin

    //Listar Combo de TipoComprobante
    $scope.listarComboTipoComprobante = function () {
        comprobanteService.comboTipoComprobante({}).then(function (data) {
            $scope.cmbTipoComprobante = data.data;
            $scope.model.idTipoComprobante = 1;
        });
    }
    //Fin

    //Listar Combo de TipoComprobante
    $scope.listarComboTipoMoneda = function () {
        comprobanteService.comboTipoMoneda({}).then(function (data) {
            $scope.cmbTipoMoneda = data.data;
            $scope.model.idMoneda = 1;
        });
    }
    //Fin

    //Listar IGV
    $scope.listarComboIGV = function () {
        comprobanteService.comboIGV({}).then(function (data) {
            $scope.cmbIGV = data.data;
            $scope.model.idIGV = 1;
            $scope.campoIgv();
        });
    }
    //Fin

    /* ---------------------------------------- */

    //Listar Ventas
    $scope.listarComprobante = function () {
        comprobanteService.listarComprobante().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    //Listar Detalle de Venta
    $scope.verVentaDetalle = function (venta) {
        $scope.modelDetalle = venta;
        comprobanteService.listarComprobanteDetalle({ idComprobante: venta.idComprobante }).then(function (data) {
            if (data.data) {
                $scope.elementosDetalle.lista = data.data;
            } else {
                $scope.elementosDetalle.lista = data.data;
            }
        });
    }
    //Fin

    //Listar Correlativo
    $scope.listarCorrelativoMast = function () {
        comprobanteService.listarCorrelativoMast().then(function (data) {
            if (data.data) {
                $scope.elementosCorrelativo.lista = data.data;
            } else {
                $scope.elementosCorrelativo.lista = data.data;
            }
        });
    }
    $scope.buscarCorrelativo = function (id) {
        for (var i = 0; i < $scope.elementosCorrelativo.lista.length; i++) {
            if ($scope.elementosCorrelativo.lista[i].idTipoComprobante == id) {
                $scope.model.idCorrelativo = $scope.elementosCorrelativo.lista[i].idCorrelativo;
            }
        }
    }
    //Fin

    //Listar Cambio Moneda
    $scope.listarCambioMoneda = function () {
        comprobanteService.listarCambioMoneda().then(function (data) {
            if (data.data) {
                $scope.elementosCambioMoneda.lista = data.data;
            } else {
                $scope.elementosCambioMoneda.lista = data.data;
            }
        });
    }
    //Fin

    //Listar Cliente
    $scope.listarCliente = function () {
        comprobanteService.listarCliente({}).then(function (data) {
            if (data.data.result_description) {
                $scope.elementosCliente.lista = data.data;
            } else {
                $scope.elementosCliente.lista = data.data;
            }
        });
    }
    //Fin

    //Listar Producto
    $scope.listarProducto = function () {
        comprobanteService.listarProducto().then(function (data) {
            if (data.data) {
                $scope.elementosProducto.lista = data.data;
            } else {
                $scope.elementosProducto.lista = data.data;
            }
        });
    }
    //Fin

    //Listar Parámetro
    $scope.listarParametro = function () {
        comprobanteService.listarparametro().then(function (data) {
            if (data.data) {
                $scope.elementosParametro.lista = data.data;
            } else {
                $scope.elementosParametro.lista = data.data;
            }
        });
    }
    //Fin

    //Obtener cliente para la venta
    $scope.selccionCliente = function (itemCliente) {
        $scope.cliente = itemCliente;
    }
    //Fin

    //Obtener producto para la venta
    $scope.selccionProducto = function (itemProducto) {
        $scope.producto = itemProducto;
    }
    //Fin

    //Obtener parametro para la venta
    $scope.selccionParametro = function (itemProducto) {
        $scope.parametro = itemProducto;
    }
    //Fin

    //Agregar producto al carro de venta
    $scope.agregar = function (producto) {
        if (producto.estado == "ACTIVO") {
            if (producto.cantidad < producto.stock) {
                if ($scope.model.idMoneda == 1) {
                    $scope.carrito.lista.push(producto);
                    $scope.soles = (producto.cantidad * producto.precio) + $scope.soles;
                    $scope.model.subtotal = $scope.soles.toFixed(2);
                    $scope.solesTotal = (($scope.soles * ($scope.igv / 100)) + $scope.soles);
                    //$scope.model.subtotal = ($scope.soles - ($scope.soles * ($scope.igv / 100)));
                    //$scope.solesTotal = $scope.soles;
                    $scope.model.total = $scope.solesTotal.toFixed(2);
                    $scope.producto = {};
                }
                if ($scope.model.idMoneda == 2) {
                    $scope.carrito.lista.push(producto);
                    $scope.soles = (producto.cantidad * producto.precio) + $scope.soles;
                    $scope.solesTotal = (($scope.soles * ($scope.igv / 100)) + $scope.soles);
                    //$scope.soles = ($scope.soles - ($scope.soles * ($scope.igv / 100)));
                    //$scope.solesTotal = (producto.cantidad * producto.precio) + $scope.soles;
                    $scope.cambioMoneda();
                    $scope.producto = {};
                }
            } else {
                $.notify({
                    icon: 'fa fa-exclamation-circle',
                    title: 'Alerta !',
                    message: 'La cantidad debe de ser menor que el Stock'
                }, { type: 'warning', z_index: 2000 });
                $scope.producto.cantidad = "";
            }
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta !',
                message: 'No se puede agregar el producto INACTIVO'
            }, { type: 'warning', z_index: 2000 });
            $scope.producto = {};
        }
    }
    $scope.agregarCarrito = function (producto) {
        //console.log($scope.carrito.lista);
        var contador = 0;
        if (producto.codigo != "" && producto.producto != "" && producto.tipoProducto != "") {
            if ($scope.carrito.lista.length != 0) {
                for (var i = 0; i < $scope.carrito.lista.length; i++) {
                    if ($scope.carrito.lista[i].idProducto == producto.idProducto) {
                        contador = contador + 1;
                    }
                }
                if (contador == 0) {
                    $scope.agregar(producto);
                } else {
                    $.notify({
                        icon: 'fa fa-exclamation-circle',
                        title: 'Alerta !',
                        message: 'No se puede agregar el producto 2 veces'
                    }, { type: 'warning', z_index: 2000 });
                    $scope.producto = {};
                }
            } else {
                $scope.agregar(producto);
            }
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta !',
                message: 'No hay producto seleccionado'
            }, { type: 'warning', z_index: 2000 });
            $scope.producto = {};
        }
    }
    //Fin

    //Quitar producto del carro de venta
    $scope.quitarCarrito = function (Producto) {
        for (var i = 0; i < $scope.carrito.lista.length; i++) {
            if ($scope.carrito.lista[i].idProducto == Producto.idProducto) {
                //$scope.carrito.lista.splice(i, 1);
                //console.log($scope.carrito.lista);
                if ($scope.model.idMoneda == 1) {
                    //$scope.carrito.lista.push(producto);
                    $scope.carrito.lista.splice(i, 1);
                    $scope.soles = $scope.soles - (Producto.cantidad * Producto.precio);
                    $scope.model.subtotal = $scope.soles.toFixed(2);
                    $scope.solesTotal = ($scope.soles * ($scope.igv / 100)) + $scope.soles;
                    $scope.model.total = $scope.solesTotal.toFixed(2);
                    $scope.Producto = {};
                }
                if ($scope.model.idMoneda == 2) {
                    //$scope.carrito.lista.push(producto);
                    $scope.carrito.lista.splice(i, 1);
                    $scope.soles = $scope.soles - (Producto.cantidad * Producto.precio);
                    $scope.solesTotal = ($scope.soles * ($scope.igv / 100)) + $scope.soles;
                    $scope.cambioMoneda();
                    $scope.Producto = {};
                }
            }
        }
    }
    //Fin


    //Inicio - Cotizacion

    //Agregar producto al carro de venta
    $scope.agregarCotizacion = function (producto) {
        if (producto.estado == "ACTIVO") {
            if ($scope.model.idMoneda == 1) {
                $scope.carrito.lista.push(producto);
                $scope.soles = (producto.cantidad * producto.precio) + $scope.soles;
                $scope.model.subtotal = $scope.soles.toFixed(2);
                $scope.solesTotal = (($scope.soles * ($scope.igv / 100)) + $scope.soles);
                $scope.model.total = $scope.solesTotal.toFixed(2);
                $scope.producto = {};
            }
            if ($scope.model.idMoneda == 2) {
                $scope.carrito.lista.push(producto);
                $scope.soles = (producto.cantidad * producto.precio) + $scope.soles;
                $scope.solesTotal = (($scope.soles * ($scope.igv / 100)) + $scope.soles);
                $scope.cambioMoneda();
                $scope.producto = {};
            }
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta !',
                message: 'No se puede agregar el producto INACTIVO'
            }, { type: 'warning', z_index: 2000 });
            $scope.producto = {};
        }
    }
    $scope.agregarCarritoCotizacion = function (producto) {
        //console.log($scope.carrito.lista);
        var contador = 0;
        if (producto.codigo != "" && producto.producto != "" && producto.tipoProducto != "") {
            if ($scope.carrito.lista.length != 0) {
                for (var i = 0; i < $scope.carrito.lista.length; i++) {
                    if ($scope.carrito.lista[i].idProducto == producto.idProducto) {
                        contador = contador + 1;
                    }
                }
                $scope.agregarCotizacion(producto);
            } else {
                $scope.agregarCotizacion(producto);
            }
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta !',
                message: 'No hay producto seleccionado'
            }, { type: 'warning', z_index: 2000 });
            $scope.producto = {};
        }
    }
    //Fin

    //Fin - Cotizacion


    //Ontencion de Igv
    $scope.campoIgv = function () {
        console.log("sssss" + $scope.cmbIGV.length);
        for (var i = 0; i < $scope.cmbIGV.length; i++) {
            if ($scope.cmbIGV[i].Id == $scope.model.idIGV) {
                $scope.igv = parseInt($scope.cmbIGV[i].Nombre);
            }
        }
    }
    //Fin

    //Cambio de moneda
    $scope.cambioMoneda = function () {
        if ($scope.model.idMoneda == 1) {
            $('#dolaresSubtotal').hide()
            $('#solesSubtotal').show()
            $('#dolaresTotal').hide()
            $('#solesTotal').show()
            console.log("sssss" + $scope.cmbTipoMoneda.length);
        }
        if ($scope.model.idMoneda == 2) {
            $('#solesSubtotal').hide()
            $('#dolaresSubtotal').show()
            $('#solesTotal').hide()
            $('#dolaresTotal').show()
        }

        for (var i = 0; i < $scope.elementosCambioMoneda.lista.length; i++) {
            if ($scope.elementosCambioMoneda.lista[i].idMoneda == $scope.model.idMoneda) {
                $scope.model.subtotal = $scope.soles / $scope.elementosCambioMoneda.lista[i].ventaMoneda;
                $scope.model.subtotal = $scope.model.subtotal.toFixed(2);

                $scope.model.total = $scope.solesTotal / $scope.elementosCambioMoneda.lista[i].ventaMoneda;
                $scope.model.total = $scope.model.total.toFixed(2);
            } if ($scope.model.idMoneda == 1) {
                $scope.model.subtotal = $scope.soles.toFixed(2);
                $scope.model.total = $scope.solesTotal.toFixed(2);
            }
        }
    }
    //Fin

    // Registrar comprobante electrónico - Inicio
    $scope.guardarComprobanteElectronico = function (idComprobante) {

        var params = {
            id: idComprobante
        };

        if (confirm("¿Seguro de crear el comprobante electrónico?")) {
            comprobanteService.guardarComprobanteElectronico(params).then(function (data) {
                if (data.data.code_result == 0) {
                    $scope.messageSuccess(data.data.result_description);
                } else {
                    $scope.messageError(data.data.result_description);
                }
            });
        }
    }
    // Fin

    $scope.listarComboTipoComprobante();
    $scope.listarComboTipoMoneda();
    $scope.listarCambioMoneda();
    $scope.listarCorrelativoMast();
    $scope.listarComprobante();
    $scope.listarProducto();
    $scope.listarCliente();
    $scope.listarComboIGV();
    $scope.listarParametro();
    $scope.init();

}]);