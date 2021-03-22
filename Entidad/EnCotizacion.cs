using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EnRegistrarCotizacion
    {
        public long Id { get; set; }
        public string FacturacionRuc { get; set; }
        public string FacturacionRazonSocial { get; set; }
        public string FacturacionCorreo { get; set; }
    }
    public class EnContizacionIn
    {
        public string TipoCotizacion { get; set; }
        public string NumeroCotizacion { get; set; }
        public string RucSolicitante { get; set; }
        public string NombreSolicitante { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string DescripcionProducto { get; set; }
        public string ObservacionesProducto { get; set; }
        public string NombreContacto { get; set; }
        public string SerieNumero { get; set; }
        public string IdUsuarioRegistro { get; set; }
    }

    public class EnCotizacionOut
    {
        public long ID { get; set; }
        public string TipoCotizacion { get; set; }
        public string NumeroCotizacion { get; set; }
        public string RUC { get; set; }
        public string Solicitante { get; set; }
        public string Fecha { get; set; }
        public string DescripcionProducto { get; set; }
        public string Observaciones { get; set; }
        public string Contacto { get; set; }
        public string SerieNumero { get; set; }
        public string UsuarioRegistro { get; set; }
    }
}
