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
    public class ClienteTitular
    {
        [DataMember]
        public int idTitular { get; set; }
        public string titularNombre { get; set; }
        public string titularApellidos { get; set; }
        public int titularIdTipoDocumento { get; set; }    //Id Tipo Documento
        public string titularTipoDocumento { get; set; }
        public string titularNumeroDocumento { get; set; }
        public string titularCorreo { get; set; }
        public string titularCargo { get; set; }
        public string titularTelefono { get; set; }
        public string titularRubro { get; set; }

        public ClienteTitular()
        {
        }

        public ClienteTitular(Cliente _cliente)
        {
            //Titular
            this.idTitular = _cliente.PersonaMast1.idPersona;
            this.titularNombre = _cliente.PersonaMast1.nombre;
            this.titularApellidos = _cliente.PersonaMast1.apellidos;
            this.titularIdTipoDocumento = _cliente.PersonaMast1.TipoDocumento.idTipoDocumento;
            this.titularTipoDocumento = _cliente.PersonaMast1.TipoDocumento.tipoDocumento1;
            this.titularNumeroDocumento = _cliente.PersonaMast1.numeroDocumento;
            this.titularCorreo = _cliente.PersonaMast1.correo;
            this.titularTelefono = _cliente.PersonaMast1.telefono;
            this.titularCargo = _cliente.PersonaMast1.cargo;
            this.titularRubro = _cliente.rubro;
        }
    }
}