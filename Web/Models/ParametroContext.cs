using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ParametroContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ParametroContext() : base("name=CotizacionContext")
        {
            //Database.SetInitializer<ParametroContext>(new DropCreateDatabaseIfModelChanges<ParametroContext>());
        }

        public System.Data.Entity.DbSet<Web.Models.Parametro.Parametro> Parametroes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ParametroContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
