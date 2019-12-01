using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Datos;
using Web.Models.Modelo;

namespace Web.Models.Clientes
{
    [Serializable]
    [DataContract]
    public class MostrarClienteViewModel
    {
        [DataMember]
        //public ClienteContacto clienteContacto;
        //public ClienteTitular clienteTitular;
        //public ClienteEmpresa clienteEmpresa;

        public int idCliente { get; set; }

        public string cliente { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string rubros { get; set; }
        public string contacto { get; set; }
        public string tipoPersona { get; set; }
        public int idTipoPersona { get; set; }

        #region null
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
        #endregion

        public MostrarClienteViewModel()
        {
        }

        public MostrarClienteViewModel(Cliente _cliente)
        {
            this.idCliente = _cliente.idCliente;

            this.rubros = _cliente.rubro;

            if (_cliente.PersonaMast != null)
            {
                this.cliente = _cliente.PersonaMast.nombre + " " + _cliente.PersonaMast.apellidos;
                this.tipoDocumento = _cliente.PersonaMast.TipoDocumento.tipoDocumento1;
                this.numeroDocumento = _cliente.PersonaMast.numeroDocumento;

                this.tipoPersona = _cliente.PersonaMast.TipoPersona.tipoPersona1;
                this.idTipoPersona = _cliente.PersonaMast.TipoPersona.idTipoPersona;

                ////Empresa
                //clienteEmpresa = new ClienteEmpresa(_cliente);
                ////Titular
                //clienteTitular = new ClienteTitular(_cliente);
                ////Contacto
                //clienteContacto = new ClienteContacto(_cliente);

                #region null
                //Empresa
                this.idEmpresa = _cliente.PersonaMast.idPersona;
                this.empresaNombre = _cliente.PersonaMast.nombre;
                this.empresaDomicilio = _cliente.PersonaMast.domicilio;
                this.empresaIdTipoDocumento = _cliente.PersonaMast.TipoDocumento.idTipoDocumento;
                this.empresaTipoDocumento = _cliente.PersonaMast.TipoDocumento.tipoDocumento1;
                this.empresaNumeroDocumento = _cliente.PersonaMast.numeroDocumento;
                this.empresaCorreo = _cliente.PersonaMast.correo;
                this.empresaRubro = _cliente.PersonaMast.cargo;

                #endregion
            }

            //titular
            this.titularRubro = _cliente.rubro;

            if (_cliente.PersonaMast1 != null)
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
                this.titularDomicilio = _cliente.PersonaMast1.domicilio;
            }

            if (_cliente.PersonaMast2 != null)
            {
                this.contacto = _cliente.PersonaMast2.nombre + " " + _cliente.PersonaMast2.apellidos;

                //Contacto
                this.idContacto = _cliente.PersonaMast2.idPersona;
                this.contactoNombre = _cliente.PersonaMast2.nombre;
                this.contactoApellidos = _cliente.PersonaMast2.apellidos;
                this.contactoCorreo = _cliente.PersonaMast2.correo;
                this.contactoTelefono = _cliente.PersonaMast2.telefono;
                this.contactoCargo = _cliente.PersonaMast2.cargo;
            }
            //if (string.IsNullOrEmpty(_cliente.PersonaMast.nombre))
            //{
            //    _cliente.PersonaMast.nombre = "";
            //}
            //if (string.IsNullOrEmpty(_cliente.PersonaMast.apellidos))
            //{
            //    _cliente.PersonaMast.apellidos = "";
            //}
            //if (string.IsNullOrEmpty(_cliente.PersonaMast.TipoDocumento.tipoDocumento1))
            //{
            //    _cliente.PersonaMast.TipoDocumento.tipoDocumento1 = "";
            //}
            //if (string.IsNullOrEmpty(_cliente.PersonaMast.numeroDocumento))
            //{
            //    _cliente.PersonaMast.numeroDocumento = "";
            //}
            //if (string.IsNullOrEmpty(_cliente.PersonaMast2.nombre))
            //{
            //    _cliente.PersonaMast2.nombre = "";
            //}
            //if (string.IsNullOrEmpty(_cliente.PersonaMast2.apellidos))
            //{
            //    _cliente.PersonaMast2.apellidos = "";
            //}
            //if (string.IsNullOrEmpty(_cliente.PersonaMast.TipoPersona.tipoPersona1))
            //{
            //    _cliente.PersonaMast.TipoPersona.tipoPersona1 = "";
            //}
            //if (_cliente.PersonaMast.TipoPersona.idTipoPersona== null)
            //{
            //    _cliente.PersonaMast.TipoPersona.tipoPersona1 = "";
            //}


        }

        public static List<MostrarClienteViewModel> convert(List<Cliente> cliente)
        {
            List<MostrarClienteViewModel> resultado = new List<MostrarClienteViewModel>();

            foreach (var item in cliente)
            {
                resultado.Add(new MostrarClienteViewModel(item));
            }
            return resultado;
        }
    }
}