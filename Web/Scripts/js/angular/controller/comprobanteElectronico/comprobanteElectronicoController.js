app.controller('comprobanteElectronicoController', ['$rootScope', '$scope', 'comprobanteElectronicoService', function ($rootScope, $scope, comprobanteElectronicoService) {

    $scope.model = {};
    $scope.model.Comprobante = {};
    $scope.elementosComprobante = { lista: [] };
    $scope.elementosCliente = { lista: [] };

    $scope.init = function () {

        $('.txtFecha').datepicker({
            todayHighlight: true,
            format: "dd/mm/yyyy",
            language: "es",            
            autoclose: true
        });

        $scope.model.Estado = "-1";

        var current_datetime = new Date()
        var fecha = appendLeadingZeroes(current_datetime.getDate()) + "/" + appendLeadingZeroes(current_datetime.getMonth() + 1) + "/" + current_datetime.getFullYear();
        $scope.model.FechaInicial = fecha;
        $scope.model.FechaFinal = fecha;
        $scope.model.NumeroDocumentoIdentidadReceptor = "";
        $scope.model.TipoComprobante = "0";
        $scope.model.SerieNumero = "";
        $scope.model.TipoEmision = "0";
        $scope.model.MotivoEmision = "";
    }

    $scope.registrarNota = function () {
        var idComprobante = $scope.model.Comprobante.IdComprobante;
        var tipoEmision = $scope.model.TipoEmision;
        var motivoEmision = $scope.model.MotivoEmision;

        if (tipoEmision == "0") {
            $scope.mensajeAlerta("Debe seleccionar un tipo de emisión para la nota de crédito.");
            return;
        }

        if (motivoEmision == undefined || motivoEmision.trim() == "") {
            $scope.mensajeAlerta("Debe ingresar el motivo de emisión de la nota de crédito.");
            return;
        }

        var parametro = {
            IdComprobante: idComprobante,
            TipoEmision: tipoEmision,
            MotivoEmision: motivoEmision
        };

        $.confirm({
            title: 'Confirmación',
            content: '¿Desea continuar con la generación de la nota de crédito?',
            buttons: {
                confirm: {
                    text: 'Continuar',
                    action: function () {
                        $('#modal-crearNotaCredito').modal('hide')
                        $("#myModal").css("display", "block");
                        comprobanteElectronicoService.GenerarNotaCredito(parametro).then(function (data) {
                            if (data.data) {
                                if (data.data.Codigo == "0") {
                                    $scope.mensajeExito(data.data.Descripcion);
                                    $scope.listarComprobante();
                                } else {
                                    $scope.mensajeAlerta(data.data.Descripcion);
                                }
                            }
                            $("#myModal").css("display", "none");
                        });
                    }
                },
                cancel: {
                    text: 'Cancelar'
                }
            }
        });
    }

    $scope.listarComprobante = function () {

        var nroDocumentoEmisor = "20602034675";
        var nroDocumentoReceptor = $scope.model.NumeroDocumentoIdentidadReceptor;
        var fechaInicial = $scope.model.FechaInicial;
        var fechaFinal = $scope.model.FechaFinal;
        var estado = $scope.model.Estado;
        var tipoComprobante = $scope.model.TipoComprobante;
        var serieNumero = $scope.model.SerieNumero;

        var parametro = {
            Estado: estado,
            NumeroDocumentoIdentidadEmisor: nroDocumentoEmisor,
            NumeroDocumentoIdentidadReceptor: nroDocumentoReceptor,
            FechaInicial: fechaInicial,
            FechaFinal: fechaFinal,
            TipoComprobante: tipoComprobante,
            SerieNumero: serieNumero
        };

        comprobanteElectronicoService.listarComprobante(parametro).then(function (data) {
            if (data.data) {
                if (data.data.Codigo == "0") {
                    $scope.elementosComprobante.lista = data.data.Comprobantes;
                } else {
                    console.log(data.data);
                }
            }
        });
    }

    $scope.ObtenerDocumentoComprobante = function (idComprobante) {
        var parametro = {
            id: idComprobante
        };

        comprobanteElectronicoService.ObtenerDocumentoComprobante(parametro).then(function (data) {
            if (data.data) {
                if (data.data.Codigo == "0") {
                    const linkSource = 'data:application/xml;base64,' + data.data.ContenidoArchivo;
                    const downloadLink = document.createElement('a');
                    document.body.appendChild(downloadLink);

                    downloadLink.href = linkSource;
                    downloadLink.target = '_self';
                    downloadLink.download = data.data.NombreArchivo;
                    downloadLink.click();
                } else {
                    $scope.mensajeAlerta(data.data.Descripcion);
                }
            } else {
                data.data("No se obtuvo respuesta desde el servicio de facturación.");
            }
        });
    }

    $scope.ObtenerRepresentacionImpresa = function (idComprobante) {
        var parametro = {
            id: idComprobante
        };

        comprobanteElectronicoService.ObtenerRepresentacionImpresa(parametro).then(function (data) {
            if (data.data) {
                if (data.data.Codigo == "0") {
                    const linkSource = 'data:application/pdf;base64,' + data.data.ContenidoArchivo;
                    const downloadLink = document.createElement('a');
                    document.body.appendChild(downloadLink);

                    downloadLink.href = linkSource;
                    downloadLink.target = '_self';
                    downloadLink.download = data.data.NombreArchivo;
                    downloadLink.click();
                } else {
                    $scope.mensajeAlerta(data.data.Descripcion);
                }
            } else {
                data.data("No se obtuvo respuesta desde el servicio de facturación.");
            }
        });
    }

    $scope.ObtenerRespuestaComprobante = function (item) {
        var parametro = {
            id: item.IdComprobante
        };

        comprobanteElectronicoService.ObtenerRespuestaComprobante(parametro).then(function (data) {
            if (data.data) {
                if (data.data.Codigo == "0") {
                    if (item.Estado == 'Excepcion' || item.Estado == 'Rechazado') {
                        const linkSource = 'data:application/xml;base64,' + data.data.ContenidoArchivo;
                        console.log("XML: " + linkSource);
                        const downloadLink = document.createElement('a');
                        document.body.appendChild(downloadLink);

                        downloadLink.href = linkSource;
                        downloadLink.target = '_self';
                        downloadLink.download = data.data.NombreArchivo.replace(".zip", ".xml");;
                        downloadLink.click();
                    } else {
                        const linkSource = 'data:application/zip;base64,' + data.data.ContenidoArchivo;
                        console.log("ZIP: " + linkSource);
                        const downloadLink = document.createElement('a');
                        document.body.appendChild(downloadLink);

                        downloadLink.href = linkSource;
                        downloadLink.target = '_self';
                        downloadLink.download = data.data.NombreArchivo;
                        downloadLink.click();
                    }
                } else {
                    $scope.mensajeAlerta(data.data.Descripcion);
                }
            } else {
                data.data("No se obtuvo respuesta desde el servicio de facturación.");
            }
        });
    }

    $scope.listarCliente = function () {
        comprobanteElectronicoService.listarCliente().then(function (data) {
            if (data.data.result_description) {
                $scope.elementosCliente.lista = data.data;
            } else {
                $scope.elementosCliente.lista = data.data;
            }
        });
    }

    $scope.seleccionCliente = function (itemCliente) {
        $scope.model.NumeroDocumentoIdentidadReceptor = itemCliente.numeroDocumento;
    }

    $scope.PantallaGeneracionNotas = function (comprobante) {
        $scope.model.Comprobante = comprobante;
        $scope.model.TipoEmision = "0";
        $scope.model.MotivoEmision = "";
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

    appendLeadingZeroes = function (n) {
        if (n <= 9) {
            return "0" + n;
        }
        return n
    }

    $scope.init();
    $scope.listarCliente();
    $scope.listarComprobante();
}]);