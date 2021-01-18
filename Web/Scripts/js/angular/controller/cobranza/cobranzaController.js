app.controller('cobranzaController', ['$rootScope', '$scope', 'cobranzaService', function ($rootScope, $scope, cobranzaService) {

    $scope.model = {};
    $scope.cobranza = {};
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
        $("#myModal").css("display", "block");
        $('.txtFecha').datepicker({
            todayHighlight: true,
            format: "dd/mm/yyyy",
            language: "es",
            autoclose: true
        });

        $scope.cobranza.Saldo = "0";
        $scope.cobranza.PagoDetraccion = "-1";

        var parametro = $("#idDetalle").attr("value");
        var parametro = { id: $("#idDetalle").attr("value") }
        cobranzaService.obtenerCotizacion(parametro).then(function (data) {
            if (data.data) {
                $scope.model = data.data.Cotizacion;
                $scope.cobranza = data.data.Cobranza;

                $scope.cobranza.Importe1 = $scope.formatearDecimales($scope.cobranza.Importe1);
                $scope.cobranza.Importe2 = $scope.formatearDecimales($scope.cobranza.Importe2);
                $scope.cobranza.Importe3 = $scope.formatearDecimales($scope.cobranza.Importe3);
                $scope.cobranza.Saldo = $scope.formatearDecimales($scope.cobranza.Saldo);

                $scope.model.Fecha = data.data.Cotizacion.Fecha;
                $scope.cliente.numeroDocumento = data.data.Cotizacion.RUC;
                $scope.cliente.contacto = data.data.Cotizacion.Contacto;
                $scope.cliente.contactoCorreo = data.data.Cotizacion.Email;
                $scope.cliente.cliente = data.data.Cotizacion.Solicitante;
            } else {
                $scope.mensajeAlerta('Ocurrió un error en el registro, contáctese con el administrador.');
            }
            $("#myModal").css("display", "none");
        });
    }

    $scope.formatearDecimales = function (valor) {
        return parseFloat(valor).toFixed(2);
    }

    $scope.registrarCobranza = function (idCotizacion) {
        $scope.cobranza.IdCotizacion = idCotizacion;

        if ($scope.cobranza.EjecutivoVenta == undefined || $scope.cobranza.EjecutivoVenta == "") {
            $scope.mensajeAlerta("Debe ingresar el ejecutivo de ventas");
            return;
        }

        if ($scope.cobranza.FechaIngreso == undefined || $scope.cobranza.FechaIngreso == "") {
            $scope.mensajeAlerta("Debe ingresar la fecha de ingreso");
            return;
        }

        if ($scope.cobranza.Importe1 > 0 && ($scope.cobranza.FechaPago1 == undefined || $scope.cobranza.FechaPago1 == "")) {
            $scope.mensajeAlerta("Debe ingresar la fecha de pago 1");
            return;
        }

        if ($scope.cobranza.Importe2 > 0 && ($scope.cobranza.FechaPago2 == undefined || $scope.cobranza.FechaPago2 == "")) {
            $scope.mensajeAlerta("Debe ingresar la fecha de pago 2");
            return;
        }

        if ($scope.cobranza.Importe3 > 0 && ($scope.cobranza.FechaPago3 == undefined || $scope.cobranza.FechaPago3 == "")) {
            $scope.mensajeAlerta("Debe ingresar la fecha de pago 3");
            return;
        }

        if ($scope.cobranza.PagoDetraccion == "-1") {
            $scope.mensajeAlerta("Debe seleccionar pago de detracción");
            return;
        }

        if (parseFloat($scope.cobranza.Saldo) < 0) {
            $scope.mensajeAlerta("El saldo no puede ser un valor negativo");
            return;
        }

        $("#myModal").css("display", "block");
        cobranzaService.registrarCobranza($scope.cobranza).then(function (data) {
            if (data.data) {
                if (data.data.Codigo == "0") {
                    $("#linkRegistrarComprobante").css("display", "none");
                    $scope.mensajeExito(data.data.Mensaje);
                } else {
                    $scope.mensajeAlerta(data.data.Mensaje);
                }
            } else {
                $scope.mensajeAlerta('Ocurrió un error en el registro, contáctese con el administrador.');
            }
            $("#myModal").css("display", "none");
        });
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

    $scope.calcularSaldo = function () {
        var total = parseFloat($scope.model.Total);
        var importe1 = parseFloat(($scope.cobranza.Importe1 == undefined || $scope.cobranza.Importe1 == '') ? 0 : $scope.cobranza.Importe1);
        var importe2 = parseFloat(($scope.cobranza.Importe2 == undefined || $scope.cobranza.Importe2 == '') ? 0 : $scope.cobranza.Importe2);
        var importe3 = parseFloat(($scope.cobranza.Importe3 == undefined || $scope.cobranza.Importe3 == '') ? 0 : $scope.cobranza.Importe3);        
        var detraccion = parseFloat($scope.cobranza.Detraccion);
        var preSaldo = total - importe1 - importe2 - importe3;
        var saldo = ($scope.cobranza.Autodetraccion == true) ? preSaldo : preSaldo - detraccion;
        $scope.cobranza.Saldo = $scope.formatearDecimales(saldo);
    }

    $scope.init();
}]);