using Entidad;
using System.Collections.Generic;
using Web.Models.Clientes;
using Web.Models.Producto;

namespace Web.Models.Cotizacion
{
    public class CotizacionViewModel
    {
        public MostrarClienteViewModel Cliente { get; set; }
        public Cotizacion Cotizacion { get; set; }
        public List<CotizacionCertificado> Certificados { get; set; }
        public List<CotizacionProducto> Productos { get; set; }
        public List<DetalleCotizacionViewModel> Detalles { get; set; }
        public List<CotizacionInspeccion> Inspeccion { get; set; }
        public List<CotizacionResumen> Resumen { get; set; }
        public string NombreUsuario { get; set; }
        public EnCobranza Cobranza { get; set; }
    }

    public class CotizacionViewModelOut
    {
        public MostrarClienteViewModel Cliente { get; set; }
        public Cotizacion Cotizacion { get; set; }
        public List<EnCotizacionCertificado> Certificados { get; set; }
        public List<CotizacionProducto> Productos { get; set; }
        public List<DetalleCotizacionViewModel> Detalles { get; set; }
        public List<EnCotizacionInspeccion> Inspeccion { get; set; }
        public List<EnCotizacionResumen> Resumen { get; set; }
        public string NombreUsuario { get; set; }
        public EnCobranza Cobranza { get; set; }
        public ApplicationUser Usuario { get; set; }
    }

    public class DetalleCotizacionViewModel
    {
        public MostrarProductoServicioViewModels producto { get; set; }
        public Parametro.Parametro parametro { get; set; }
        public TipoParametro.TipoParametro tipoParametro { get; set; }
        public CotizacionProducto productoCotizacion { get; set; }
        public decimal Precio { get; set; }
    }
}