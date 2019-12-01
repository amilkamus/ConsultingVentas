using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.TipoParametro
{
    public class TipoParametro
    {
        public long ID { get; set; }

        public long CodTipoParametro { get; set; }
        public string TipoParametroDescripcion { get; set; }
        public string Estado { get; set; }
        public string UsuarioCreacion { get; set; }
        public string FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }

    }
}