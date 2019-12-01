using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class TipoDocumentoDAT:ConexionBD
    {
        public OperationResult crear(TipoDocumento tipoDocuemento)
        {
            try
            {
                _db.TipoDocumento.Add(tipoDocuemento);
                var result = Save();
                result.data = tipoDocuemento.idTipoDocumento;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(TipoDocumento tipoDocuemento)
        {
            try
            {
                TipoDocumento dbTipoDocumento = _db.TipoDocumento.Single(m => m.idTipoDocumento == tipoDocuemento.idTipoDocumento);
                _db.Entry(dbTipoDocumento).CurrentValues.SetValues(tipoDocuemento);
                var result = Save();
                result.data = tipoDocuemento.idTipoDocumento;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idTipoDocumento)
        {

            try
            {
                var documento = _db.TipoDocumento.Single(p => p.idTipoDocumento == idTipoDocumento);
                _db.TipoDocumento.Remove(documento);
                var result = Save();
                result.data = documento.idTipoDocumento;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<TipoDocumento> listar()
        {
            try
            {
                return _db.TipoDocumento;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
