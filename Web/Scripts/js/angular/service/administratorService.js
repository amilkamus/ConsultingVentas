app.factory('adminService', function ($http) {
    return {
        listarUsuarios: function (params) {
            return $http.post(URL + "/Account/ListarUsuarios", params);
        },
        listarRoles: function (params) {
            return $http.post(URL + "/Role/ListarRoles", params);
        },
        eliminarUsuario: function (params) {
            return $http.post(URL + "/Account/DeleteUser", params);
        },
        eliminarRol: function (params) {
            return $http.post(URL + "/Role/Delete", params);
        },
        obtenerRoles: function (params) {
            return $http.post(URL + "/Account/GetAllRolesAsSelectList", params);
        },
        guardarUsuario: function (params) {
            return $http.post(URL + "/Account/GuardarUsuario", params);
        },
        guardarRol: function (params) {
            return $http.post(URL + "/Role/Guardar", params);
        }
    };
});

app.factory('companiaService', function ($http) {
    return {
        guardarCompania: function (params) {
            return $http.post(URL + "/Compania/guardarCompania", params);
        },
        listarCompania: function (params) {
            return $http.post(URL + "/Compania/listarCompania", params);
        }
    };
});

app.factory('documentoService', function ($http) {
    return {
        guardarDocumento: function (params) {
            return $http.post(URL + "/Documento/guardarDocumento", params);
        },
        listarDocumento: function (params) {
            return $http.post(URL + "/Documento/listarDocumento", params);
        },
        eliminarDocumento: function (params) {
            return $http.post(URL + "/Documento/eliminarDocumento", params);
        },
        listarCotizaciones: function (params) {
            return $http.post(URL + "/Cotizacion/ListarCotizaciones", params);
        },
        listarOrdenesServicio: function (params) {
            return $http.post(URL + "/OrdenServicio/ListarOrdenesServicio", params);
        },
        listarUsuarios: function (params) {
            return $http.post(URL + "/Cotizacion/ListarUsuarios", params);
        },
        listarCobranzas: function (params) {
            return $http.post(URL + "/Cobranza/ListarCobranzas", params);
        }
    };
});

app.factory('tipoPersonaService', function ($http) {
    return {
        guardarTipoPersona: function (params) {
            return $http.post(URL + "/TipoPersonas/guardarTipoPersona", params);
        },
        listarTipoPersona: function (params) {
            return $http.post(URL + "/TipoPersonas/listarTipoPersona", params);
        },
        eliminarTipoPersona: function (params) {
            return $http.post(URL + "/TipoPersonas/eliminarTipoPersona", params);
        }
    };
});

app.factory('tipoProductoService', function ($http) {
    return {
        guardarTipoProducto: function (params) {
            return $http.post(URL + "/TipoProducto/guardarTipoProducto", params);
        },
        listarTipoProducto: function (params) {
            return $http.post(URL + "/TipoProducto/listarTipoProducto", params);
        },
        eliminarTipoProducto: function (params) {
            return $http.post(URL + "/TipoProducto/eliminarTipoProducto", params);
        }
    };
});

app.factory('tipoComprobanteService', function ($http) {
    return {
        guardarTipoComprobante: function (params) {
            return $http.post(URL + "/TipoComprobante/guardarTipoComprobante", params);
        },
        listarTipoComprobante: function (params) {
            return $http.post(URL + "/TipoComprobante/listarTipoComprobante", params);
        },
        eliminarTipoComprobante: function (params) {
            return $http.post(URL + "/TipoComprobante/eliminarTipoComprobante", params);
        }
    };
});

app.factory('comprobanteService', function ($http) {
    return {
        guardarComprobante: function (params) {
            return $http.post(URL + "/Venta/guardarComprobante", params);
        },
        listarComprobante: function (params) {
            return $http.post(URL + "/Venta/listarComprobante", params);
        },
        listarComprobanteDetalle: function (params) {
            return $http.post(URL + "/Venta/listarComprobanteDetalle", params);
        },
        listarCorrelativoMast: function (params) {
            return $http.post(URL + "/Venta/listarCorrelativoMast", params);
        },
        listarCambioMoneda: function (params) {
            return $http.post(URL + "/Venta/listarCambioMoneda", params);
        },
        listarCliente: function (params) {
            return $http.post(URL + "/Venta/listarCliente", params);
        },
        listarProducto: function (params) {
            return $http.post(URL + "/Venta/listarProducto", params);
        },
        comboTipoComprobante: function (params) {
            return $http.post(URL + "/Venta/comboTipoComprobante", params);
        },
        comboTipoMoneda: function (params) {
            return $http.post(URL + "/Venta/comboTipoMoneda", params);
        },
        comboIGV: function (params) {
            return $http.post(URL + "/Venta/comboIGV", params);
        },
        listarparametro: function (params) {
            return $http.post(URL + "/Parametro/Index", params);
        },
        guardarComprobanteElectronico: function (params) {
            return $http.post(URL + "/Venta/GenerarComprobanteElectronico", params);
        }
    };
});

app.factory('correlativoService', function ($http) {
    return {
        guardarCorrelativoMast: function (params) {
            return $http.post(URL + "/CorrelativoMast/guardarCorrelativoMast", params);
        },
        listarCorrelativoMast: function (params) {
            return $http.post(URL + "/CorrelativoMast/listarCorrelativoMast", params);
        },
        eliminarCorrelativoMast: function (params) {
            return $http.post(URL + "/CorrelativoMast/eliminarCorrelativoMast", params);
        },
        comboTipoComprobante: function (params) {
            return $http.post(URL + "/CorrelativoMast/comboTipoComprobante", params);
        }
    };
});

app.factory('cajaService', function ($http) {
    return {
        guardarCaja: function (params) {
            return $http.post(URL + "/Caja/guardarCaja", params);
        },
        listarCaja: function (params) {
            return $http.post(URL + "/Caja/listarCaja", params);
        },
        eliminarCaja: function (params) {
            return $http.post(URL + "/Caja/eliminarCaja", params);
        }
    };
});

app.factory('egresoService', function ($http) {
    return {
        guardarEgreso: function (params) {
            return $http.post(URL + "/Egreso/guardarEgreso", params);
        },
        listarEgreso: function (params) {
            return $http.post(URL + "/Egreso/listarEgreso", params);
        },
        eliminarEgreso: function (params) {
            return $http.post(URL + "/Egreso/eliminarEgreso", params);
        }
    };
});

app.factory('clienteService', function ($http) {
    return {
        guardarCliente: function (params) {
            return $http.post(URL + "/Cliente/guardarCliente", params);
        },
        listarCliente: function (params) {
            return $http.post(URL + "/Cliente/listarCliente", params);
        },
        eliminarCliente: function (params) {
            return $http.post(URL + "/Cliente/eliminarCliente", params);
        },
        comboTipoDocumento: function (params) {
            return $http.post(URL + "/Cliente/comboTipoDocumento", params);
        },
        comboTipoPersona: function (params) {
            return $http.post(URL + "/Cliente/comboTipoPersona", params);
        }
    };
});

app.factory('productoService', function ($http) {
    return {
        guardarProducto: function (params) {
            return $http.post(URL + "/Producto/guardarProducto", params);
        },
        listarProducto: function (params) {
            return $http.post(URL + "/Producto/listarProducto", params);
        },
        eliminarProducto: function (params) {
            return $http.post(URL + "/Producto/eliminarProducto", params);
        },
        comboTipoProducto: function (params) {
            return $http.post(URL + "/Producto/comboTipoProducto", params);
        },
        listarParametro: function (params) {
            return $http.post(URL + "/Producto/listarParametroProducto", params);
        }
    };
});

app.factory('cambioMonedaService', function ($http) {
    return {
        guardarCambioMoneda: function (params) {
            return $http.post(URL + "/CambioMoneda/guardarCambioMoneda", params);
        },
        listarCambioMoneda: function (params) {
            return $http.post(URL + "/CambioMoneda/listarCambioMoneda", params);
        },
        eliminarCambioMoneda: function (params) {
            return $http.post(URL + "/CambioMoneda/eliminarCambioMoneda", params);
        },
        comboMoneda: function (params) {
            return $http.post(URL + "/CambioMoneda/comboMoneda", params);
        }
    };
});

app.factory('monedaService', function ($http) {
    return {
        guardarMoneda: function (params) {
            return $http.post(URL + "/Moneda/guardarMoneda", params);
        },
        listarMoneda: function (params) {
            return $http.post(URL + "/Moneda/listarMoneda", params);
        },
        eliminarMoneda: function (params) {
            return $http.post(URL + "/Moneda/eliminarMoneda", params);
        }
    };
});

app.factory('cotizacionService', function ($http) {
    return {
        listarCliente: function (params) {
            return $http.post(URL + "/Venta/listarCliente", params);
        },
        listarProducto: function (params) {
            return $http.post(URL + "/Venta/listarProducto", params);
        },
        listarParametro: function (params) {
            return $http.post(URL + "/Parametro/listarParametro", params);
        },
        listarTipoParametro: function (params) {
            return $http.post(URL + "/TipoParametro/listarTipoParametro", params);
        },
        comboTipoMoneda: function (params) {
            return $http.post(URL + "/Venta/comboTipoMoneda", params);
        },
        registrarCotizacion: function (params) {
            return $http.post(URL + "/Cotizacion/RegistrarCotizacion", params);
        },
        obtenerCotizacion: function (params) {
            return $http.post(URL + "/Cotizacion/ObtenerCotizacion", params);
        },
        listarTipoCotizacion: function (params) {
            return $http.post(URL + "/Cotizacion/listarTipoCotizacion", params);
        },
        generarComprobante: function (params) {
            return $http.post(URL + "/Cotizacion/GenerarComprobante", params);
        },
        listarParametrosProducto: function (params) {
            return $http.post(URL + "/Cotizacion/ObtenerParametrosProducto", params);
        },
        registrarCobranza: function (params) {
            return $http.post(URL + "/Cotizacion/RegistrarCobranza", params);
        }
    };
});

app.factory('ordenServicioService', function ($http) {
    return {
        listarCotizacion: function (params) {
            return $http.post(URL + "/OrdenServicio/listarCotizaciones", params);
        },
        obtenerCotizacion: function (params) {
            return $http.post(URL + "/Cotizacion/ObtenerCotizacion", params);
        },
        registrarOrdenServicio: function (params) {
            return $http.post(URL + "/OrdenServicio/RegistrarOrdenServicio", params);
        },
        obtenerOrdenServicio: function (params) {
            return $http.post(URL + "/OrdenServicio/ObtenerOrdenServicio", params);
        }
    };
});

app.factory('comprobanteElectronicoService', function ($http) {
    return {
        listarComprobante: function (params) {
            return $http.post(URL + "/Comprobante/ListarComprobanteElectronicos", params);
        },
        ObtenerDocumentoComprobante: function (params) {
            return $http.post(URL + "/Comprobante/ObtenerDocumentoComprobante", params);
        },
        ObtenerRepresentacionImpresa: function (params) {
            return $http.post(URL + "/Comprobante/ObtenerRepresentacionImpresa", params);
        },
        ObtenerRespuestaComprobante: function (params) {
            return $http.post(URL + "/Comprobante/ObtenerRespuestaComprobante", params);
        },
        GenerarNotaCredito: function (params) {
            return $http.post(URL + "/Comprobante/GenerarNotaCredito", params);
        },
        listarCliente: function (params) {
            return $http.post(URL + "/Venta/listarCliente", params);
        }
    };
});

app.factory('cobranzaService', function ($http) {
    return {
        obtenerCotizacion: function (params) {
            return $http.post(URL + "/Cobranza/ObtenerCotizacion", params);
        },
        listarTipoCotizacion: function (params) {
            return $http.post(URL + "/Cotizacion/listarTipoCotizacion", params);
        },
        registrarCobranza: function (params) {
            return $http.post(URL + "/Cotizacion/RegistrarCobranza", params);
        }
    };
});