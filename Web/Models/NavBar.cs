using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class NavBar
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Area { get; set; }
        public string Icono { get; set; }
        public string Activeli { get; set; }
        public bool Estado { get; set; }
        public int IdPadre { get; set; }
        public bool EsPadre { get; set; }
        public int Rol { get; set; }
    }
}