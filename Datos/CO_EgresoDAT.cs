using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CO_EgresoDAT: ConexionBD
    {
        public OperationResult crear(CO_Egreso egreso)
        {
            try
            {
                _db.CO_Egreso.Add(egreso);
                var result = Save();
                result.data = egreso.idGasto;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(CO_Egreso egreso)
        {
            try
            {
                CO_Egreso dbEgreso = _db.CO_Egreso.Single(e => e.idGasto == egreso.idGasto);
                _db.Entry(dbEgreso).CurrentValues.SetValues(egreso);
                var result = Save();
                result.data = egreso.idGasto;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idEgreso)
        {

            try
            {
                var egreso = _db.CO_Egreso.Single(p => p.idGasto == idEgreso);
                _db.CO_Egreso.Remove(egreso);
                var result = Save();
                result.data = egreso.idGasto;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<CO_Egreso> listar()
        {
            try
            {
                return _db.CO_Egreso;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
