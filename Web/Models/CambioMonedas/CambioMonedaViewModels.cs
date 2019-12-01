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
    public class CambioMonedaViewModels
    {
        [DataMember]
        public int idCambioMoneda { get; set; }
        public int idMoneda { get; set; }
        public double compraMoneda { get; set; }
        public double ventaMoneda { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }

        public CambioMonedaViewModels()
        {
        }

        public CambioMonedaViewModels(CambioMoneda cambioMoneda)
        {
            this.idCambioMoneda = cambioMoneda.idCambioMoneda;
            this.idMoneda = cambioMoneda.idMoneda;
            this.compraMoneda = Convert.ToDouble(cambioMoneda.compraMoneda);
            this.ventaMoneda = Convert.ToDouble(cambioMoneda.ventaMoneda);
            this.descripcion = cambioMoneda.descripcion;
            this.estado = cambioMoneda.estado;
            this.fechaRegistro = Convert.ToDateTime(cambioMoneda.fechaRegistro);
            this.fechaActualizacion = Convert.ToDateTime(cambioMoneda.fechaActualizacion);
            this.usuarioRegistro = cambioMoneda.usuarioRegistro;
            this.usuarioActualizacion = cambioMoneda.usuarioActualizacion;
        }

        public static List<CambioMonedaViewModels> convert(List<CambioMoneda> cambioMoneda)
        {
            List<CambioMonedaViewModels> resultado = new List<CambioMonedaViewModels>();

            foreach (var item in cambioMoneda)
            {
                resultado.Add(new CambioMonedaViewModels(item));
            }
            return resultado;
        }
    }
}