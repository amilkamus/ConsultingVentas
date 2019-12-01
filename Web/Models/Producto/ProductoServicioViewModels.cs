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
    public class ProductoServicioViewModels
    {
        [DataMember]
        public int idProducto { get; set; }
        public int idTipoProductoServicio { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int stock { get; set; }
        public string estado { get; set; }
        public decimal costo { get; set; }
        public decimal precio { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }
        
        public ProductoServicioViewModels()
        {
        }

        public ProductoServicioViewModels(WH_ProductoServicio productoServicio)
        {
            this.idProducto = productoServicio.idProducto;
            this.idTipoProductoServicio = Convert.ToInt32(productoServicio.idTipoProductoServicio);
            this.codigo = productoServicio.codigo;
            this.nombre = productoServicio.nombre;
            this.descripcion = productoServicio.descripcion;
            this.stock = Convert.ToInt32(productoServicio.stock);
            this.estado = productoServicio.estado;
            this.costo = Convert.ToDecimal(productoServicio.costo);
            this.precio = Convert.ToDecimal(productoServicio.precio);
            this.fechaRegistro = Convert.ToDateTime(productoServicio.fechaRegistro);
            this.fechaActualizacion = Convert.ToDateTime(productoServicio.fechaActualizacion);
            this.usuarioRegistro = productoServicio.usuarioRegistro;
            this.usuarioActualizacion = productoServicio.usuarioActualizacion;
        }

        public static List<ProductoServicioViewModels> convert(List<WH_ProductoServicio> productoServicio)
        {
            List<ProductoServicioViewModels> resultado = new List<ProductoServicioViewModels>();

            foreach (var item in productoServicio)
            {
                resultado.Add(new ProductoServicioViewModels(item));
            }
            return resultado;
        }
    }
}