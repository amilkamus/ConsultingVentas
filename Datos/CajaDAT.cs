using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class Cre:ConexionBD
    {
        public OperationResult crear(Caja caja)
        {
            try
            {
                _db.Caja.Add(caja);
                var result = Save();
                result.data = caja.idCaja;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(Caja caja)
        {
            try
            {
                Caja dbCaja = _db.Caja.Single(c => c.idCaja == caja.idCaja);
                _db.Entry(dbCaja).CurrentValues.SetValues(caja);
                var result = Save();
                result.data = caja.idCaja;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idCaja)
        {

            try
            {
                var caja = _db.Caja.Single(p => p.idCaja == idCaja);
                _db.Caja.Remove(caja);
                var result = Save();
                result.data = caja.idCaja;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Caja> listar()
        {
            try
            {
                return _db.Caja;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
