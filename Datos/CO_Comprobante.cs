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
    using System.Collections.Generic;
    
    public partial class CO_Comprobante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CO_Comprobante()
        {
            this.CO_ComprobanteDetalle = new HashSet<CO_ComprobanteDetalle>();
        }
    
        public int idComprobante { get; set; }
        public Nullable<int> idCorrelativo { get; set; }
        public string serieCorrelativo { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public Nullable<decimal> subTotal { get; set; }
        public Nullable<decimal> montoTotal { get; set; }
        public string textoTotal { get; set; }
        public Nullable<System.DateTime> fechaRegistro { get; set; }
        public Nullable<System.DateTime> fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }
        public string idUsuario { get; set; }
        public Nullable<int> idMoneda { get; set; }
        public Nullable<int> idCliente { get; set; }
        public Nullable<int> idIGV { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual CO_Moneda CO_Moneda { get; set; }
        public virtual CorrelativoMast CorrelativoMast { get; set; }
        public virtual IGVMast IGVMast { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CO_ComprobanteDetalle> CO_ComprobanteDetalle { get; set; }
    }
}
