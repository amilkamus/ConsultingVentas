using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Cotizacion
{
    public class CotizacionInspeccion
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
        public string Producto { get; set; }
        public string Documento { get; set; }
        public string TipoServicio { get; set; }
    }

    public class CotizacionResumen
    {
        public long ID { get; set; }
        public long IdCotizacion { get; set; }
        public string Descripcion { get; set; }        
        public decimal Precio { get; set; }
        public int NumeroDias { get; set; }
        public decimal Total { get; set; }
    }

}