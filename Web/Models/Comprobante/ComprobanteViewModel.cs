using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Comprobante
{
    public class ComprobanteViewModel
    {
        public int Estado { get; set; }
        public string NumeroDocumentoIdentidadEmisor { get; set; }
        public string NumeroDocumentoIdentidadReceptor { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string TipoComprobante { get; set; }
        public string SerieNumero { get; set; }
        public decimal TotalImpuesto { get; set; }
        public decimal TotalValorVenta { get; set; }
        public decimal TotalDescuento { get; set; }
    }
}