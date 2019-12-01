using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Datos;

namespace Web.Models.Modelo
{
    [Serializable]
    [DataContract]
    public class ClienteContacto
    {
        [DataMember]
        public int idContacto { get; set; }
        public string contactoNombre { get; set; }
        public string contactoApellidos { get; set; }
        public string contactoCorreo { get; set; }
        public string contactoTelefono { get; set; }
        public string contactoCargo { get; set; }

        public ClienteContacto()
        {
        }

        public ClienteContacto(Cliente _cliente)
        {
            //Contacto
            this.idContacto = _cliente.PersonaMast2.idPersona;
            this.contactoNombre = _cliente.PersonaMast2.nombre;
            this.contactoApellidos = _cliente.PersonaMast2.apellidos;
            this.contactoCorreo = _cliente.PersonaMast2.correo;
            this.contactoTelefono = _cliente.PersonaMast2.telefono;
            this.contactoCargo = _cliente.PersonaMast2.cargo;
        }
    }
}