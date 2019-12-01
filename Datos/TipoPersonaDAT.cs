using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class TipoPersonaDAT:ConexionBD
    {
        public OperationResult crear(TipoPersona tipoPersona)
        {
            try
            {
                _db.TipoPersona.Add(tipoPersona);
                var result = Save();
                result.data = tipoPersona.idTipoPersona;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(TipoPersona tipoPersona)
        {
            try
            {
                TipoPersona dbTipoPersona = _db.TipoPersona.Single(m => m.idTipoPersona == tipoPersona.idTipoPersona);
                _db.Entry(dbTipoPersona).CurrentValues.SetValues(tipoPersona);
                var result = Save();
                result.data = tipoPersona.idTipoPersona;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idTipoPersona)
        {

            try
            {
                var persona = _db.TipoPersona.Single(p => p.idTipoPersona == idTipoPersona);
                _db.TipoPersona.Remove(persona);
                var result = Save();
                result.data = persona.idTipoPersona;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<TipoPersona> listar()
        {
            try
            {
                return _db.TipoPersona;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
