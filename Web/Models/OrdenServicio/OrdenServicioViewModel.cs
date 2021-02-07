using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Clientes;
using Web.Models.Cotizacion;

namespace Web.Models.OrdenServicio
{
    public class OrdenServicioViewModel
    {
        public OrdenServicio OrdenServicio { get; set; }
        public Web.Models.Cotizacion.Cotizacion Cotizacion { get; set; }
        public List<CotizacionCertificado> Certificados { get; set; }
        public List<DetalleCotizacionViewModel> Detalles { get; set; }
        public List<CotizacionInspeccion> Inspeccion { get; set; }
        public List<CotizacionResumen> Resumen { get; set; }
        public string NombreUsuario { get; set; }
        public MostrarClienteViewModel Cliente { get; set; }
    }
}