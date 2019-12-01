using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace Web.Models.Clientes
{
    [Serializable]
    [DataContract]
    public class ClienteViewModel
    {
        [DataMember]
        public int idCliente { get; set; }        
        public int idTipoPersona { get; set; }

        public int idEmpresa { get; set; }
        public string empresaNombre { get; set; }           //Razon social
        public string empresaDomicilio { get; set; }        //Domicilio
        public int empresaIdTipoDocumento { get; set; }    //Id Tipo Documento
        public string empresaTipoDocumento { get; set; }    //Tipo Documento
        public string empresaNumeroDocumento { get; set; }  //N° Documento
        public string empresaCorreo { get; set; }           //Correo
        public string empresaRubro { get; set; }            //rubro

        public int idTitular { get; set; }
        public string titularNombre { get; set; }
        public string titularApellidos { get; set; }
        public int titularIdTipoDocumento { get; set; }    //Id Tipo Documento
        public string titularTipoDocumento { get; set; }
        public string titularNumeroDocumento { get; set; }
        public string titularCorreo { get; set; }
        public string titularDomicilio { get; set; }
        public string titularTelefono { get; set; }
        public string titularRubro { get; set; }

        public int idContacto { get; set; }
        public string contactoNombre { get; set; }
        public string contactoApellidos { get; set; }
        public string contactoCorreo { get; set; }
        public string contactoTelefono { get; set; }
        public string contactoCargo { get; set; }

        //[DataMember]
        //public int idCliente { get; set; }
        public int idEmpresaCliente { get; set; }
        //public int idTitular { get; set; }
        public string rubro { get; set; }
        //public int idContacto { get; set; }

        public ClienteViewModel()
        {
        }

        public ClienteViewModel(Cliente cliente)
        {
            this.idCliente = cliente.idCliente;
            this.idEmpresaCliente = Convert.ToInt32(cliente.idEmpresaCliente);
            this.idTitular = Convert.ToInt32(cliente.idTitular);
            this.rubro = cliente.rubro;
            this.idContacto = Convert.ToInt32(cliente.idContacto);
            
        }

        public static List<ClienteViewModel> convert(List<Cliente> cliente)
        {
            List<ClienteViewModel> resultado = new List<ClienteViewModel>();

            foreach (var item in cliente)
            {
                resultado.Add(new ClienteViewModel(item));
            }
            return resultado;
        }

    }
}