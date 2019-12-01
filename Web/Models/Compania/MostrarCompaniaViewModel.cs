using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Datos;

namespace Web.Models.Compania
{
    [Serializable]
    [DataContract]
    public class MostrarCompaniaViewModel
    {
        [DataMember]
        public int idCompania { get; set; }
        public string razonSocial { get; set; }
        public string ruc { get; set; }
        public string domicilioFiscal { get; set; }
        public string correo { get; set; }

        public int idTitular { get; set; }
        public string titularNombre { get; set; }
        public string titularApellidos { get; set; }
        public string titularCorreo { get; set; }
        public string titularTelefono { get; set; }
        public string titularCargo { get; set; }

        public int idContacto { get; set; }
        public string contactoNombre { get; set; }
        public string contactoApellidos { get; set; }
        public string contactoCorreo { get; set; }
        public string contactoTelefono { get; set; }
        public string contactoCargo { get; set; }

        public DateTime fechaRegistro { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }

        public MostrarCompaniaViewModel()
        {
        }

        public MostrarCompaniaViewModel(Datos.Compania _compania)
        {
            this.idCompania = _compania.idCompania;
            this.razonSocial = _compania.razonSocial;
            this.ruc = _compania.ruc;
            this.domicilioFiscal = _compania.domicilioFiscal;
            this.correo = _compania.correo;

            this.idTitular = _compania.PersonaMast.idPersona;
            this.titularNombre = _compania.PersonaMast.nombre;
            this.titularApellidos = _compania.PersonaMast.apellidos;
            this.titularCorreo = _compania.PersonaMast.correo;
            this.titularTelefono = _compania.PersonaMast.telefono;
            this.titularCargo = _compania.PersonaMast.cargo;

            this.idContacto = _compania.PersonaMast1.idPersona;
            this.contactoNombre = _compania.PersonaMast1.nombre;
            this.contactoApellidos = _compania.PersonaMast1.apellidos;
            this.contactoCorreo = _compania.PersonaMast1.correo;
            this.contactoTelefono = _compania.PersonaMast1.telefono;
            this.contactoCargo = _compania.PersonaMast1.cargo;

            this.fechaRegistro = Convert.ToDateTime(_compania.fechaRegistro);
            this.fechaActualizacion = Convert.ToDateTime(_compania.fechaActualizacion);
            this.usuarioRegistro = _compania.usuarioRegistro;
            this.usuarioActualizacion = _compania.usuarioActualizacion;
        }

        public static List<MostrarCompaniaViewModel> convert(List<Datos.Compania> compania)
        {
            List<MostrarCompaniaViewModel> resultado = new List<MostrarCompaniaViewModel>();

            foreach (var item in compania)
            {
                resultado.Add(new MostrarCompaniaViewModel(item));
            }
            return resultado;
        }
    }
}