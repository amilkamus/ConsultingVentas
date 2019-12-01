using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Web.Models.Parametro
{
    public class Parametro
    {
        public long ID { get; set; }
        public long CodParametro { get; set; }
        public string ParametroDescripcion { get; set; }
        public string Metodologia { get; set; }        
        public decimal? Precio { get; set; }
        public string LimiteDeteccion { get; set; }
        public string Estado { get; set; }
        public string UsuarioCreacion { get; set; }
        public string FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }        
    }
}