using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CambioMonedaDAT:ConexionBD
    {
        public OperationResult crear(CambioMoneda cambioMoneda)
        {
            try
            {
                _db.CambioMoneda.Add(cambioMoneda);
                var result = Save();
                result.data = cambioMoneda.idCambioMoneda;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(CambioMoneda cambioMoneda)
        {
            try
            {
                CambioMoneda dbCambioMoneda = _db.CambioMoneda.Single(m => m.idCambioMoneda == cambioMoneda.idCambioMoneda);
                _db.Entry(dbCambioMoneda).CurrentValues.SetValues(cambioMoneda);
                var result = Save();
                result.data = cambioMoneda.idCambioMoneda;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idCambioMoneda)
        {

            try
            {
                var cambioMoneda = _db.CambioMoneda.Single(p => p.idCambioMoneda == idCambioMoneda);
                _db.CambioMoneda.Remove(cambioMoneda);
                var result = Save();
                result.data = cambioMoneda.idCambioMoneda;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<CambioMoneda> listar()
        {
            try
            {
                return _db.CambioMoneda;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
