using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CotizacionContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public CotizacionContext() : base("name=CotizacionContext")
        {
        }

        public DbSet<Cotizacion.Cotizacion> Cotizacions { get; set; }
        public DbSet<Cotizacion.CotizacionCertificado> CotizacionCertificado { get; set; }
        public DbSet<Cotizacion.CotizacionProducto> CotizacionProducto { get; set; }
        public DbSet<Cotizacion.TipoCotizacion> TipoCotizacion { get; set; }
        public DbSet<OrdenServicio.OrdenServicio> OrdenServicio { get; set; }
        public DbSet<Cotizacion.CotizacionInspeccion> CotizacionInspeccion { get; set; }
        public DbSet<Cotizacion.CotizacionResumen> CotizacionResumen { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ParametroContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
