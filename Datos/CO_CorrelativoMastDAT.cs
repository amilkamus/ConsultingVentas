using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CO_CorrelativoMastDAT:ConexionBD
    {
        public OperationResult crear(CorrelativoMast correlativo)
        {
            try
            {
                _db.CorrelativoMast.Add(correlativo);
                var result = Save();
                result.data = correlativo.idCorrelativo;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(CorrelativoMast correlativo)
        {
            try
            {
                CorrelativoMast dbCorrelativoMast = _db.CorrelativoMast.Single(m => m.idCorrelativo == correlativo.idCorrelativo);
                _db.Entry(dbCorrelativoMast).CurrentValues.SetValues(correlativo);
                var result = Save();
                result.data = correlativo.idCorrelativo;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idCorrelativo)
        {
            try
            {
                var correlativo = _db.CorrelativoMast.Single(p => p.idCorrelativo == idCorrelativo);
                _db.CorrelativoMast.Remove(correlativo);
                var result = Save();
                result.data = correlativo.idCorrelativo;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<CorrelativoMast> listar()
        {
            try
            {
                return _db.CorrelativoMast;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
