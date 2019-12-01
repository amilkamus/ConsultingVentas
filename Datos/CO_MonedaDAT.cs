using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CO_MonedaDAT:ConexionBD
    {
        public OperationResult crear(CO_Moneda moneda)
        {
            try
            {
                _db.CO_Moneda.Add(moneda);
                var result = Save();
                result.data = moneda.idMoneda;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(CO_Moneda moneda)
        {
            try
            {
                CO_Moneda dbMoneda = _db.CO_Moneda.Single(m => m.idMoneda == moneda.idMoneda);
                _db.Entry(dbMoneda).CurrentValues.SetValues(moneda);
                var result = Save();
                result.data = moneda.idMoneda;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idMoneda)
        {

            try
            {
                var moneda = _db.CO_Moneda.Single(p => p.idMoneda == idMoneda);
                _db.CO_Moneda.Remove(moneda);
                var result = Save();
                result.data = moneda.idMoneda;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<CO_Moneda> listar()
        {
            try
            {
                return _db.CO_Moneda;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
