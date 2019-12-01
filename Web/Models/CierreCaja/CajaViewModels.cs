using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Datos;

namespace Web.Models.CierreCaja
{
    [Serializable]
    [DataContract]
    public class CajaViewModels
    {
        [DataMember]
        public int idCaja { get; set; }
        public string idUsuario { get; set; }
        public decimal ingresos { get; set; }
        public decimal saldoInicial { get; set; }
        public decimal saldoFinal { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }
        public string estado { get; set; }

        public CajaViewModels()
        {
        }

        public CajaViewModels(Caja caja)
        {
            this.idCaja = caja.idCaja;
            this.idUsuario = caja.idUsuario;
            this.ingresos = Convert.ToDecimal(caja.ingresos);
            this.saldoInicial = Convert.ToDecimal(caja.saldonIcial);
            this.saldoFinal = Convert.ToDecimal(caja.saldoFinal);
            this.fechaRegistro = Convert.ToDateTime(caja.fechaRegistro);
            this.fechaActualizacion = Convert.ToDateTime(caja.fechaActualizacion);
            this.usuarioRegistro = caja.usuarioRegistro;
            this.usuarioActualizacion = caja.usuarioActualizacion;
            this.estado = caja.estado;
        }

        public static List<CajaViewModels> convert(List<Caja> caja)
        {
            List<CajaViewModels> resultado = new List<CajaViewModels>();

            foreach (var item in caja)
            {
                resultado.Add(new CajaViewModels(item));
            }
            return resultado;
        }

    }
}