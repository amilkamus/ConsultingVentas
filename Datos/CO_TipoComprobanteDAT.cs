using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CO_TipoComprobanteDAT:ConexionBD
    {
        public OperationResult crear(CO_TipoComprobante tipoComprobante)
        {
            try
            {
                _db.CO_TipoComprobante.Add(tipoComprobante);
                var result = Save();
                result.data = tipoComprobante.idTipoComprobante;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(CO_TipoComprobante tipoComprobante)
        {
            try
            {
                CO_TipoComprobante dbTipoComprobante = _db.CO_TipoComprobante.Single(m => m.idTipoComprobante == tipoComprobante.idTipoComprobante);
                _db.Entry(dbTipoComprobante).CurrentValues.SetValues(tipoComprobante);
                var result = Save();
                result.data = tipoComprobante.idTipoComprobante;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idTipoComprobante)
        {

            try
            {
                var comprobante = _db.CO_TipoComprobante.Single(p => p.idTipoComprobante == idTipoComprobante);
                _db.CO_TipoComprobante.Remove(comprobante);
                var result = Save();
                result.data = comprobante.idTipoComprobante;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public IEnumerable<CO_TipoComprobante> listar()
        {
            try
            {
                return _db.CO_TipoComprobante;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
