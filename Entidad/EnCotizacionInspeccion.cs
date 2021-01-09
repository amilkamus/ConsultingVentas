using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EnCotizacionInspeccion
    {
        public long ID { get; set; }
        public long IdCotizacion { get; set; }
        public string Actividad { get; set; }
        public string Procedimiento { get; set; }
        public string ReferenciaNormativa { get; set; }
        public string ReferenciaMuestreo { get; set; }
        public string PlanMuestreo { get; set; }
        public string LugarMuestreo { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
