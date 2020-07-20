using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ComprobanteViewModel
    {
        public long IdComprobante { get; set; }
        public string TipoEmision { get; set; }
        public string MotivoEmision { get; set; }
    }
}