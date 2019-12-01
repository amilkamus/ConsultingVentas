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
    public class ClienteEmpresa
    {
        [DataMember]
        public int idEmpresa { get; set; }
        public string empresaNombre { get; set; }           //Razon social
        public string empresaDomicilio { get; set; }        //Domicilio
        public int empresaIdTipoDocumento { get; set; }    //Id Tipo Documento
        public string empresaTipoDocumento { get; set; }    //Tipo Documento
        public string empresaNumeroDocumento { get; set; }  //N° Documento
        public string empresaCorreo { get; set; }           //Correo
        public string empresaRubro { get; set; }            //rubro

        public ClienteEmpresa()
        {
        }

        public ClienteEmpresa(Cliente _cliente)
        {
            //Empresa
            this.idEmpresa = _cliente.PersonaMast.idPersona;
            this.empresaNombre = _cliente.PersonaMast.nombre;
            this.empresaDomicilio = _cliente.PersonaMast.domicilio;
            this.empresaIdTipoDocumento = _cliente.PersonaMast.TipoDocumento.idTipoDocumento;
            this.empresaTipoDocumento = _cliente.PersonaMast.TipoDocumento.tipoDocumento1;
            this.empresaNumeroDocumento = _cliente.PersonaMast.numeroDocumento;
            this.empresaCorreo = _cliente.PersonaMast.correo;
            this.empresaRubro = _cliente.PersonaMast.cargo;
        }
    }
}