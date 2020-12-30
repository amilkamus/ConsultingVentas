using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EnOrdenServicioIn
    {
        public string NumeroOrdenServicio { get; set; }
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

    public class EnOrdenServicioOut
    {
        public long ID { get; set; }
        public string NumeroOrdenServicio { get; set; }
        public string NumeroCotizacion { get; set; }
        public string RUC { get; set; }
        public string Solicitante { get; set; }
        public string Fecha { get; set; }
        public string DescripcionProducto { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesInforme { get; set; }
        public string UsuarioRegistro { get; set; }
    }
}
