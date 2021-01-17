using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EnCobranza
    {
        public long IdCobranza { get; set; }
        public long IdCotizacion { get; set; }
        public string EjecutivoVenta { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaPago { get; set; }
        public decimal Detraccion { get; set; }
        public string FechaPago1 { get; set; }
        public decimal Importe1 { get; set; }
        public string FechaPago2 { get; set; }
        public decimal Importe2 { get; set; }
        public string FechaPago3 { get; set; }
        public decimal Importe3 { get; set; }
        public string PagoDetraccion { get; set; }
        public string Observacion1 { get; set; }
        public bool Autodetraccion { get; set; }
        public decimal Saldo { get; set; }
        public string IdUsuario { get; set; }
        public string NroOperacion { get; set; }
        public string CodigoInterno { get; set; }
    }

    public class EnCobranzaIn
    {
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

    public class EnCobranzaOut
    {
        public long IdCotizacion { get; set; }
        public string Mes { get; set; }
        public string TipoCotizacion { get; set; }
        public string NumeroCotizacion { get; set; }
        public string EjecutivoVenta { get; set; }
        public string Solicitante { get; set; }
        public string DescripcionProducto { get; set; }
        public string Contacto { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string SerieNumero { get; set; }
        public string CondicionPago_1 { get; set; }
        public string CondicionPago_2 { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaPago { get; set; }
        public decimal Total { get; set; }
        public decimal Detraccion { get; set; }
        public string FechaPago1 { get; set; }
        public decimal Importe1 { get; set; }
        public string FechaPago2 { get; set; }
        public decimal Importe2 { get; set; }
        public string FechaPago3 { get; set; }
        public decimal Importe3 { get; set; }
        public string PagoDetraccion { get; set; }
        public decimal Saldo { get; set; }
        public string Observacion1 { get; set; }
        public bool Autodetraccion { get; set; }
        public string NroOperacion { get; set; }
        public string CodigoInterno { get; set; }
    }
}
