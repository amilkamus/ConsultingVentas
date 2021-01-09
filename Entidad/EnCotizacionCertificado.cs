using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EnCotizacionCertificado
    {
        public long ID { get; set; }
        public long IdCotizacion { get; set; }
        public string Documento { get; set; }
        public string NormaReferencia { get; set; }
        public decimal Precio { get; set; }
    }
}
