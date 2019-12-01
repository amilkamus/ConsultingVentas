using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace Web.Models.TipoPersonas
{
    [Serializable]
    [DataContract]
    public class TipoPersonaViewModel
    {
        [DataMember]
        public int idTipoPersona { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

        public TipoPersonaViewModel()
        {

        }

        public TipoPersonaViewModel(TipoPersona tipoPersona)
        {
            this.idTipoPersona = tipoPersona.idTipoPersona;
            this.nombre = tipoPersona.tipoPersona1;
            this.descripcion = tipoPersona.descripcion;
        }

        public static List<TipoPersonaViewModel> convert(List<TipoPersona> tipoPersona)
        {
            List<TipoPersonaViewModel> resultado = new List<TipoPersonaViewModel>();

            foreach (var item in tipoPersona)
            {
                resultado.Add(new TipoPersonaViewModel(item));
            }
            return resultado;
        }
    }
}