using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace Web.Models.CambioMonedas
{
    [Serializable]
    [DataContract]
    public class MostrarCambioMonedaViewModels
    {
        [DataMember]
        public int idCambioMoneda { get; set; }
        public int idMoneda { get; set; }
        public string moneda { get; set; }
        public double compraMoneda { get; set; }
        public double ventaMoneda { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public string fechaRegistro { get; set; }
        public string fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }

        public MostrarCambioMonedaViewModels()
        {
        }

        public MostrarCambioMonedaViewModels(CambioMoneda cambioMoneda)
        {
            this.idCambioMoneda = cambioMoneda.idCambioMoneda;
            this.idMoneda = cambioMoneda.idMoneda;
            this.moneda = cambioMoneda.CO_Moneda.descripcion;
            this.compraMoneda = Convert.ToDouble(cambioMoneda.compraMoneda);
            this.ventaMoneda = Convert.ToDouble(cambioMoneda.ventaMoneda);
            this.descripcion = cambioMoneda.descripcion;
            this.estado = cambioMoneda.estado;
            this.fechaRegistro = (Convert.ToDateTime(cambioMoneda.fechaRegistro)).ToString();
            this.fechaActualizacion = (Convert.ToDateTime(cambioMoneda.fechaActualizacion)).ToString();
            this.usuarioRegistro = cambioMoneda.usuarioRegistro;
            this.usuarioActualizacion = cambioMoneda.usuarioActualizacion;
        }

        public static List<MostrarCambioMonedaViewModels> convert(List<CambioMoneda> cambioMoneda)
        {
            List<MostrarCambioMonedaViewModels> resultado = new List<MostrarCambioMonedaViewModels>();

            foreach (var item in cambioMoneda)
            {
                resultado.Add(new MostrarCambioMonedaViewModels(item));
            }
            return resultado;
        }
    }
}