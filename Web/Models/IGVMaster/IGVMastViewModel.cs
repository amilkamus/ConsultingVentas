using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Datos;

namespace Web.Models.IGVMaster
{
    [Serializable]
    [DataContract]
    public class IGVMastViewModel
    {
        [DataMember]
        public int idIGV { get; set; }
        public int porcentaje { get; set; }

        public IGVMastViewModel() { }

        public IGVMastViewModel(IGVMast igvMast)
        {
            this.idIGV = igvMast.idIGV;
            this.porcentaje = Convert.ToInt32(igvMast.porcentaje);
        }

        public static List<IGVMastViewModel> convert(List<IGVMast> igvMast)
        {
            List<IGVMastViewModel> resultado = new List<IGVMastViewModel>();

            foreach (var item in igvMast)
            {
                resultado.Add(new IGVMastViewModel(item));
            }
            return resultado;
        }

    }
}