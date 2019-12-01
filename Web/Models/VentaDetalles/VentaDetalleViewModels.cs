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
    public class VentaDetalleViewModels
    {
        [DataMember]
        public int idDetalleComprobante { get; set; }
        public int idProducto { get; set; }
        public int idComprobante { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal montoUnitario { get; set; }

        public VentaDetalleViewModels()
        {
        }

        public VentaDetalleViewModels(CO_ComprobanteDetalle venta)
        {
            this.idDetalleComprobante = venta.idDetalleComprobante;
            this.idProducto = Convert.ToInt32(venta.idProducto);
            this.idComprobante = Convert.ToInt32(venta.idComprobante);
            this.cantidad = Convert.ToInt32(venta.cantidad);
            this.precio = Convert.ToDecimal(venta.precio);
            this.montoUnitario = Convert.ToDecimal(venta.montoUnitario);
        }

        public static List<VentaDetalleViewModels> convert(List<CO_ComprobanteDetalle> venta)
        {
            List<VentaDetalleViewModels> resultado = new List<VentaDetalleViewModels>();

            foreach (var item in venta)
            {
                resultado.Add(new VentaDetalleViewModels(item));
            }
            return resultado;
        }
    }
}