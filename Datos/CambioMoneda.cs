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
    
    public partial class CambioMoneda
    {
        public int idCambioMoneda { get; set; }
        public int idMoneda { get; set; }
        public Nullable<double> compraMoneda { get; set; }
        public Nullable<double> ventaMoneda { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public Nullable<System.DateTime> fechaRegistro { get; set; }
        public Nullable<System.DateTime> fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }
    
        public virtual CO_Moneda CO_Moneda { get; set; }
    }
}
