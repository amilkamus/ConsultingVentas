using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class IGVMastDAT:ConexionBD
    {
        public int porcentaje(int idIGV)
        {
            int id = 0;

            var query = from mi in _db.IGVMast
                        where mi.idIGV == idIGV
                        select mi;


            foreach (var resul in query)
            {
                id = Convert.ToInt32(resul.idIGV);
            }

            return id;
        }

        public IEnumerable<IGVMast> listar()
        {
            try
            {
                return _db.IGVMast;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
