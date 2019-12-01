using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.OrdenServicio
{
    public class OrdenServicioCotizacion
    {
        public long ID { get; set; }
        public int Correlativo { get; set; }
        public string NumeroOrdenServicio { get; set; }
        public string NumeroCotizacion { get; set; }
        public string Contacto { get; set; }
        public string Fecha { get; set; }
        public string RUC { get; set; }
        public string Solicitante { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }

    public class OrdenServicio
    {
        [Key]
        public long ID { get; set; }
        public int Correlativo { get; set; }
        public string NumeroOrdenServicio { get; set; }
        public string NumeroCotizacion { get; set; }
        // Informes
        public int NumeroCopiasInforme { get; set; }
        public string ObservacionesInforme { get; set; }
        // Materiales
        public string DireccionEnvioMateriales { get; set; }
        public string FechaEnvioMateriales { get; set; }
        public string ContactoMateriales { get; set; }
        // Inspección
        public string ContactoInspeccion { get; set; }
        public string EmailInspeccion { get; set; }
        public string TelefonoInspeccion { get; set; }
        public string CoordinadorInspeccion { get; set; }
        public string LugarInspeccion { get; set; }
        public string FechaInspeccion { get; set; }
        public string HoraInspeccion { get; set; }
        public string TipoServicioInspeccion { get; set; }
        public string NombreProductoInspeccion { get; set; }
        public string CantidadLoteInspeccion { get; set; }
        public string CodigosLoteInspeccion { get; set; }
        public string PresentacionInspeccion { get; set; }
        public string ObservacionesInspeccion { get; set; }
        // usuario
        public string IdUsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}