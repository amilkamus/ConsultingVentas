using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace Web.Models.Ventas
{
    [Serializable]
    [DataContract]
    public class VentaViewModels
    {
        [DataMember]
        public int idComprobante { get; set; }
        public int idCorrelativo { get; set; }
        public string serieCorrelativo { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
        public string textoTotal { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }
        public string idUsuario { get; set; }
        public int idCliente { get; set; }
        public int idMoneda { get; set; }
        public int idIGV { get; set; }

        public VentaViewModels()
        {
        }

        public VentaViewModels(CO_Comprobante venta)
        {
            this.idComprobante = venta.idComprobante;
            this.idCorrelativo = Convert.ToInt32(venta.idCorrelativo);
            this.serieCorrelativo = venta.serieCorrelativo;
            this.descripcion = venta.descripcion;
            this.estado = venta.estado;
            this.subtotal = Convert.ToDecimal(venta.subTotal);
            this.total = Convert.ToDecimal(venta.montoTotal);
            this.textoTotal = venta.textoTotal;
            this.fechaRegistro = Convert.ToDateTime(venta.fechaRegistro);
            this.fechaActualizacion = Convert.ToDateTime(venta.fechaActualizacion);
            this.usuarioRegistro = venta.usuarioRegistro;
            this.usuarioActualizacion = venta.usuarioActualizacion;
            this.idUsuario = venta.idUsuario;
            this.idCliente = Convert.ToInt32(venta.idCliente);
            this.idMoneda = Convert.ToInt32(venta.idMoneda);
            this.idIGV = Convert.ToInt32(venta.idIGV);

        }

        public static List<VentaViewModels> convert(List<CO_Comprobante> venta)
        {
            List<VentaViewModels> resultado = new List<VentaViewModels>();

            foreach (var item in venta)
            {
                resultado.Add(new VentaViewModels(item));
            }
            return resultado;
        }
    }
}