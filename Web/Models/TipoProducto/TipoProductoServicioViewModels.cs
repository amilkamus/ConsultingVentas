using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Datos;

namespace Web.Models.TipoProducto
{
    [Serializable]
    [DataContract]
    public class TipoProductoServicioViewModels
    {
        [DataMember]
        public int idTipoProductoServicio { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }

        public TipoProductoServicioViewModels()
        {
        }

        public TipoProductoServicioViewModels(WH_TipoProductoServicio tipoProductoServicio)
        {
            this.idTipoProductoServicio = tipoProductoServicio.idTipoProductoServicio;
            this.nombre = tipoProductoServicio.tipoProductoServicio;
            this.descripcion = tipoProductoServicio.descripcion;
            this.estado = tipoProductoServicio.estado;
            this.fechaRegistro = Convert.ToDateTime(tipoProductoServicio.fechaRegistro);
            this.fechaActualizacion = Convert.ToDateTime(tipoProductoServicio.fechaActualizacion);
            this.usuarioRegistro = tipoProductoServicio.usuarioRegistro;
            this.usuarioActualizacion = tipoProductoServicio.usuarioActualizaion;
        }

        public static List<TipoProductoServicioViewModels> convert(List<WH_TipoProductoServicio> tipoProductoServicio)
        {
            List<TipoProductoServicioViewModels> resultado = new List<TipoProductoServicioViewModels>();

            foreach (var item in tipoProductoServicio)
            {
                resultado.Add(new TipoProductoServicioViewModels(item));
            }
            return resultado;
        }
    }
}