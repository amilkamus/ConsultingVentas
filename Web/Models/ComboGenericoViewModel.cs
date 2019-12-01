using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ComboGenericoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ComboGenericoViewModel() { }

        public ComboGenericoViewModel(int id, string nombre)
        {
            this.Id = id;
            this.Nombre = nombre;
        }

    }
}