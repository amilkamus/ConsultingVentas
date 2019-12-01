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
    public class RegistroCompaniaViewModel
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
        public string titularCargo { get; set; }
        public string titularTelefono { get; set; }

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


        public RegistroCompaniaViewModel()
        {
        }

        public RegistroCompaniaViewModel(Datos.Compania _compania)
        {
            this.idCompania = _compania.idCompania;
            this.razonSocial = _compania.razonSocial;
            this.ruc = _compania.ruc;
            this.domicilioFiscal = _compania.domicilioFiscal;
            this.correo = _compania.correo;
            this.idTitular = Convert.ToInt32(_compania.idTitular);
            this.idContacto = Convert.ToInt32(_compania.idContacto);
            this.fechaRegistro = Convert.ToDateTime(_compania.fechaRegistro);
            this.fechaActualizacion = Convert.ToDateTime(_compania.fechaActualizacion);
            this.usuarioRegistro = _compania.usuarioRegistro;
            this.usuarioActualizacion = _compania.usuarioActualizacion;
        }

        public static List<RegistroCompaniaViewModel> convert(List<Datos.Compania> compania)
        {
            List<RegistroCompaniaViewModel> resultado = new List<RegistroCompaniaViewModel>();

            foreach (var item in compania)
            {
                resultado.Add(new RegistroCompaniaViewModel(item));
            }
            return resultado;
        }

    }
}