using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Parametro
{
    public class ParametroViewModel
    {
        public bool Activo { get; set; }
        public long ID { get; set; }
        public long CodParametro { get; set; }
        public string ParametroDescripcion { get; set; }
        public string Metodologia { get; set; }
        public decimal? Precio { get; set; }
        public long IdTipoParametro { get; set; }
        public long IdProducto { get; set; }
    }
}