using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;
using Negocio;

namespace Web.Models.Ventas
{
    [Serializable]
    [DataContract]
    public class MostrarVentaViewModels
    {
        ClienteNEG clienteNEG = new ClienteNEG();

        [DataMember]
        public int idComprobante { get; set; }

        public int idCorrelativo { get; set; }
        public string correlativo { get; set; }    //serie+correlativo = FAC-000001
        public string serieCorrelativo { get; set; }

        public int idTipoComprobante { get; set; }
        public string tipoComprobante { get; set; }

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
        public int idPersonaMaster { get; set; }
        public string cliente { get; set; }

        public int idMoneda { get; set; }
        public string moneda { get; set; }

        public int idIGV { get; set; }
        public int porcentaje { get; set; }

        //public int idTipoCambio { get; set; }
        //public decimal precioVenta { get; set; }
        //public decimal precioCompra { get; set; }

        public MostrarVentaViewModels()
        {
        }

        public MostrarVentaViewModels(CO_Comprobante venta)
        {
            this.idComprobante = venta.idComprobante;

            this.idCorrelativo = Convert.ToInt32(venta.idCorrelativo);
            //this.correlativo = venta.CorrelativoMast.serie + "-" + venta.CorrelativoMast.correlativo;
            this.serieCorrelativo = venta.serieCorrelativo;

            this.idTipoComprobante = venta.CorrelativoMast.CO_TipoComprobante.idTipoComprobante;
            this.tipoComprobante = venta.CorrelativoMast.CO_TipoComprobante.tipoComprobante;

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

            this.idCliente = Convert.ToInt32((venta.idCliente == null) ? 0 : venta.idCliente);
            this.idPersonaMaster = Convert.ToInt32((venta.Cliente == null) ? 0 : (venta.Cliente.idEmpresaCliente == null ? 0 : venta.Cliente.idEmpresaCliente));
            this.cliente = "amilcar"; // venta.Cliente.PersonaMast.nombre + " " + venta.Cliente.PersonaMast.apellidos;

            this.idMoneda = Convert.ToInt32(venta.idMoneda);
            this.moneda = venta.CO_Moneda.descripcion;

            this.idIGV = Convert.ToInt32(venta.idIGV);
            this.porcentaje = Convert.ToInt32(venta.IGVMast.porcentaje);

        }

        public static List<MostrarVentaViewModels> convert(List<CO_Comprobante> venta)
        {
            List<MostrarVentaViewModels> resultado = new List<MostrarVentaViewModels>();

            foreach (var item in venta)
            {
                resultado.Add(new MostrarVentaViewModels(item));
            }
            return resultado;
        }
    }
}