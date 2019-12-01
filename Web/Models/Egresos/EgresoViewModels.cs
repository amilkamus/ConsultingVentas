using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Datos;

namespace Web.Models.Egresos
{
    [Serializable]
    [DataContract]
    public class EgresoViewModels
    {
        [DataMember]
        public int idGasto { get; set; }
        public string idUsuario { get; set; }
        public string egreso { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public decimal monto { get; set; }
        public decimal total { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public string usuarioRegistro { get; set; }
        public string usuarioActualizacion { get; set; }

        public EgresoViewModels()
        {
        }

        public EgresoViewModels(CO_Egreso egresos)
        {
            this.idGasto = egresos.idGasto;
            this.idUsuario = egresos.idUsuario;
            this.egreso = egresos.egreso;
            this.descripcion = egresos.descripcion;
            this.cantidad = Convert.ToInt32(egresos.cantidad);
            this.monto = Convert.ToDecimal(egresos.monto);
            this.total = Convert.ToDecimal(egresos.total);
            this.fechaRegistro = Convert.ToDateTime(egresos.fechaRegistro);
            this.fechaActualizacion = Convert.ToDateTime(egresos.fechaActualizacion);
            this.usuarioRegistro = egresos.usuarioRegistro;
            this.usuarioActualizacion = egresos.usuarioActualizacion;
        }

        public static List<EgresoViewModels> convert(List<CO_Egreso> egreso)
        {
            List<EgresoViewModels> resultado = new List<EgresoViewModels>();

            foreach (var item in egreso)
            {
                resultado.Add(new EgresoViewModels(item));
            }
            return resultado;
        }
    }
}