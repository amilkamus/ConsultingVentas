using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace Web.Models.VentaDetalles
{
    [Serializable]
    [DataContract]
    public class MostrarVentaDetalleViewModels
    {
        [DataMember]
        public int idDetalleComprobante { get; set; }
        public int idProducto { get; set; }
        public string producto { get; set; }
        public int stock { get; set; }

        public int idTipoProducto { get; set; }
        public string tipoProducto { get; set; }

        public int idComprobante { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal montoUnitario { get; set; }

        public MostrarVentaDetalleViewModels()
        {
        }

        public MostrarVentaDetalleViewModels(CO_ComprobanteDetalle venta)
        {
            this.idDetalleComprobante = venta.idDetalleComprobante;
            this.idProducto = Convert.ToInt32(venta.idProducto);
            this.producto = venta.WH_ProductoServicio.nombre;
            this.stock = Convert.ToInt32(venta.WH_ProductoServicio.stock);

            this.idTipoProducto = Convert.ToInt32(venta.WH_ProductoServicio.WH_TipoProductoServicio.idTipoProductoServicio);
            this.tipoProducto = venta.WH_ProductoServicio.WH_TipoProductoServicio.tipoProductoServicio;

            this.idComprobante = Convert.ToInt32(venta.idComprobante);
            this.cantidad = Convert.ToInt32(venta.cantidad);
            this.precio = Convert.ToDecimal(venta.precio);
            this.montoUnitario = Convert.ToDecimal(venta.montoUnitario);
        }

        public static List<MostrarVentaDetalleViewModels> convert(List<CO_ComprobanteDetalle> venta)
        {
            List<MostrarVentaDetalleViewModels> resultado = new List<MostrarVentaDetalleViewModels>();

            foreach (var item in venta)
            {
                resultado.Add(new MostrarVentaDetalleViewModels(item));
            }
            return resultado;
        }
    }
}