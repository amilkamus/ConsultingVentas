//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    
    public partial class sp_ReporteComprobante_Result
    {
        public int idDetalleComprobante { get; set; }
        public int idComprobante { get; set; }
        public string serieCorrelativo { get; set; }
        public string fechaRegistro { get; set; }
        public string tipoComprobante { get; set; }
        public string serie { get; set; }
        public string correlativo { get; set; }
        public int idCliente { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string numeroDocumento { get; set; }
        public string domicilio { get; set; }
        public Nullable<int> idProducto { get; set; }
        public string producto { get; set; }
        public string tipoProductoServicio { get; set; }
        public Nullable<decimal> precio { get; set; }
        public Nullable<int> cantidad { get; set; }
        public Nullable<decimal> montoUnitario { get; set; }
        public Nullable<int> idMoneda { get; set; }
        public string descripcion { get; set; }
        public Nullable<double> ventaMoneda { get; set; }
        public Nullable<decimal> subTotal { get; set; }
        public Nullable<decimal> montoTotal { get; set; }
        public string textoTotal { get; set; }
        public Nullable<int> idIGV { get; set; }
        public Nullable<int> porcentaje { get; set; }
        public string estado { get; set; }
    }
}
