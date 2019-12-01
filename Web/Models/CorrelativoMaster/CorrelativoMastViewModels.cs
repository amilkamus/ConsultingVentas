using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace Web.Models.CorrelativoMaster
{
    [Serializable]
    [DataContract]
    public class CorrelativoMastViewModels
    {
        [DataMember]
        public int idCorrelativo { get; set; }
        public int idTipoComprobante { get; set; }
        public String tipoComprobante { get; set; }
        public string serie { get; set; }
        public string correlativo { get; set; }

        public CorrelativoMastViewModels()
        {
        }

        public CorrelativoMastViewModels(CorrelativoMast correlativoMaster)
        {
            this.idCorrelativo = correlativoMaster.idCorrelativo;
            this.idTipoComprobante = Convert.ToInt32(correlativoMaster.idTipoComprobante);
            this.tipoComprobante = correlativoMaster.CO_TipoComprobante.tipoComprobante;
            this.serie = correlativoMaster.serie;
            this.correlativo = correlativoMaster.correlativo;
        }

        public static List<CorrelativoMastViewModels> convert(List<CorrelativoMast> correlativoMaster)
        {
            List<CorrelativoMastViewModels> resultado = new List<CorrelativoMastViewModels>();

            foreach (var item in correlativoMaster)
            {
                resultado.Add(new CorrelativoMastViewModels(item));
            }
            return resultado;
        }
    }
}