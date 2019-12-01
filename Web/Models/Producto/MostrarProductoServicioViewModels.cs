using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Datos;

namespace Web.Models.Producto
{
    [Serializable]
    [DataContract]
    public class MostrarProductoServicioViewModels
    {
        [DataMember]
        public int idProducto { get; set; }
        public int idTipoProductoServicio { get; set; }
        public string tipoProducto { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int stock { get; set; }
        public string estado { get; set; }
        public decimal costo { get; set; }
        public decimal precio { get; set; }
        public string fechaRegistro { get; set; }
        public string fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }

        public MostrarProductoServicioViewModels()
        {
        }

        public MostrarProductoServicioViewModels(WH_ProductoServicio productoServicio)
        {
            this.idProducto = productoServicio.idProducto;
            this.idTipoProductoServicio = Convert.ToInt32(productoServicio.idTipoProductoServicio);
            this.tipoProducto = productoServicio.WH_TipoProductoServicio.tipoProductoServicio;
            this.codigo = productoServicio.codigo;
            this.nombre = productoServicio.nombre;
            this.descripcion = productoServicio.descripcion;
            this.stock = Convert.ToInt32(productoServicio.stock);
            this.estado = productoServicio.estado;
            this.costo = Convert.ToDecimal(productoServicio.costo);
            this.precio = Convert.ToDecimal(productoServicio.precio);
            this.fechaRegistro =   (Convert.ToDateTime(productoServicio.fechaRegistro)).ToString();
            this.fechaActualizacion = (Convert.ToDateTime(productoServicio.fechaActualizacion)).ToString();
            this.usuarioRegistro = productoServicio.usuarioRegistro;
            this.usuarioActualizacion = productoServicio.usuarioActualizacion;
        }

        public static List<MostrarProductoServicioViewModels> convert(List<WH_ProductoServicio> productoServicio)
        {
            List<MostrarProductoServicioViewModels> resultado = new List<MostrarProductoServicioViewModels>();

            foreach (var item in productoServicio)
            {
                resultado.Add(new MostrarProductoServicioViewModels(item));
            }
            return resultado;
        }
    }
}