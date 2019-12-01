using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class IGVMastNEG
    {
        IGVMastDAT igvMastDAT = new IGVMastDAT();

        public int porcentaje(int id)
        {
            return igvMastDAT.porcentaje(id);
        }

        public List<IGVMast> listarIGV()
        {
            try
            {
                return igvMastDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
