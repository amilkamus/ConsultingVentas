using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace Web.Models.PersonaMaster
{
    [Serializable]
    [DataContract]
    public class PersonaMastViewModel
    {
        [DataMember]
        public int idPersona { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public int idTipoPersona { get; set; }
        public int idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string cargo { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string domicilio { get; set; }
        public string estado { get; set; }


        public PersonaMastViewModel()
        {

        }

        public PersonaMastViewModel(PersonaMast personaMast)
        {
            this.idPersona = personaMast.idPersona;
            this.nombre = personaMast.nombre;
            this.apellidos = personaMast.apellidos;
            this.idTipoPersona = Convert.ToInt32(personaMast.idTipoPersona);
            this.idTipoDocumento = Convert.ToInt32(personaMast.TipoDocumento.idTipoDocumento);
            this.numeroDocumento = personaMast.numeroDocumento;
            this.cargo = personaMast.cargo;
            this.correo = personaMast.correo;
            this.telefono = personaMast.telefono;
            this.domicilio = personaMast.domicilio;
            this.estado = personaMast.estado;
        }

        public static List<PersonaMastViewModel> convert(List<PersonaMast> personaMast)
        {
            List<PersonaMastViewModel> resultado = new List<PersonaMastViewModel>();

            foreach (var item in personaMast)
            {
                resultado.Add(new PersonaMastViewModel(item));
            }
            return resultado;
        }
    }
}