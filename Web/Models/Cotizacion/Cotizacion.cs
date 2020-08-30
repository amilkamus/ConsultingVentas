using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Web.Models.Cotizacion
{
    public class Cotizacion
    {
        public long ID { get; set; }
        public string IdCotizacion { get; set; }
        public string TipoCotizacion { get; set; }
        public string NumeroCotizacion { get; set; }
        public string Solicitante { get; set; }
        public string RUC { get; set; }
        public string Contacto { get; set; }
        public string Email { get; set; }
        public string Fecha { get; set; }
        public string TipoDocumentoSolicitado { get; set; }
        public string DescripcionProducto { get; set; }
        public string CantidadMuestra { get; set; }
        public string Telefono { get; set; }
        public decimal SubTotal { get; set; }
        public decimal IGV { get; set; }
        public decimal Total { get; set; }
        public int DiasEntrega { get; set; }
        public string CorreoConfirmacion { get; set; }
        public string CondicionPago_1 { get; set; }
        public string CondicionPago_2 { get; set; }
        public string Banco { get; set; }
        public string Moneda { get; set; }
        public string CuentaCorriente { get; set; }
        public string CuentaAhorro { get; set; }
        public string CCI { get; set; }
        public string Observaciones { get; set; }
        public string CondicionPago_De { get; set; }
        public string Detracciones { get; set; }
        public string Correlativo { get; set; }
        public bool CorrelativoInicial { get; set; }
        public string IdUsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal MontoDescuento { get; set; }
        public string SerieNumero { get; set; }
        public decimal SubTotalFinal { get; set; }
        public string Proyecto { get; set; }
        public bool EmisionDigital { get; set; }
        public enum TeaType
        {
            Tea, Coffee, GreenTea, BlackTea
        }
    }
}
