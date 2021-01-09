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

        if ($scope.cobranza.FechaPago == undefined || $scope.cobranza.FechaPago == "") {
            $scope.mensajeAlerta("Debe ingresar la fecha de pago");
            return;
        }

        if ($scope.cobranza.PagoDetraccion == "-1") {
            $scope.mensajeAlerta("Debe seleccionar pago de detracción");
            return;
        }

        if ($scope.cobranza.Saldo == "0") {
            $scope.mensajeAlerta("Debe seleccionar el saldo");
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

    $scope.init();
}]);