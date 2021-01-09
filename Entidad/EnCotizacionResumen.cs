using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EnCotizacionResumen
    {
        public long ID { get; set; }
        public long IdCotizacion { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int NumeroDias { get; set; }
        public decimal Total { get; set; }
    }
}
