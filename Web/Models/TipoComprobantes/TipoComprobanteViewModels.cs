using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace Web.Models.TipoComprobantes
{
    [Serializable]
    [DataContract]
    public class TipoComprobanteViewModels
    {
        [DataMember]
        public int idTipoComprobante { get; set; }
        public string tipoComprobante { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }

        public TipoComprobanteViewModels()
        {
        }

        public TipoComprobanteViewModels(CO_TipoComprobante tipoComprobante)
        {
            this.idTipoComprobante = tipoComprobante.idTipoComprobante;
            this.tipoComprobante = tipoComprobante.tipoComprobante;
            this.descripcion = tipoComprobante.descripcion;
            this.estado = tipoComprobante.estado;
        }

        public static List<TipoComprobanteViewModels> convert(List<CO_TipoComprobante> tipoComprobante)
        {
            List<TipoComprobanteViewModels> resultado = new List<TipoComprobanteViewModels>();

            foreach (var item in tipoComprobante)
            {
                resultado.Add(new TipoComprobanteViewModels(item));
            }
            return resultado;
        }
    }
}