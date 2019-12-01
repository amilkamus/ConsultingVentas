using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Cotizacion
{
    public class CotizacionProducto
    {
        public long ID { get; set; }
        public long IdCotizacion { get; set; }
        public long IdProducto { get; set; }
        public long IdTipoParametro { get; set; }
        public long IdParametro { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}