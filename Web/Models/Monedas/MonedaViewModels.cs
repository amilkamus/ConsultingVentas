using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace Web.Models.Monedas
{
    [Serializable]
    [DataContract]
    public class MonedaViewModels
    {
        [DataMember]
        public int idMoneda { get; set; }
        public string descripcion { get; set; }

        public MonedaViewModels()
        {
        }

        public MonedaViewModels(CO_Moneda moneda)
        {
            this.idMoneda = moneda.idMoneda;
            this.descripcion = moneda.descripcion;
        }

        public static List<MonedaViewModels> convert(List<CO_Moneda> moneda)
        {
            List<MonedaViewModels> resultado = new List<MonedaViewModels>();

            foreach (var item in moneda)
            {
                resultado.Add(new MonedaViewModels(item));
            }
            return resultado;
        }
    }
}