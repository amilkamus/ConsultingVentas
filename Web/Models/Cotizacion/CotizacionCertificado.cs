namespace Web.Models.Cotizacion
{
    public class CotizacionCertificado
    {
        public long ID { get; set; }
        public long IdCotizacion { get; set; }
        public string Documento { get; set; }
        public string NormaReferencia { get; set; }
        public decimal Precio { get; set; }
        public string TipoServicio { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
    }
}