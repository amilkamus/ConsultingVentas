using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class WH_TipoProductoServicioDAT:ConexionBD
    {
        public OperationResult crear(WH_TipoProductoServicio tipoProductoServicio)
        {
            try
            {
                _db.WH_TipoProductoServicio.Add(tipoProductoServicio);
                var result = Save();
                result.data = tipoProductoServicio.idTipoProductoServicio;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(WH_TipoProductoServicio tipoProductoServicio)
        {
            try
            {
                WH_TipoProductoServicio dbTipoProductoServicio = _db.WH_TipoProductoServicio.Single(m => m.idTipoProductoServicio == tipoProductoServicio.idTipoProductoServicio);
                _db.Entry(dbTipoProductoServicio).CurrentValues.SetValues(tipoProductoServicio);
                var result = Save();
                result.data = tipoProductoServicio.idTipoProductoServicio;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idTipoProducto)
        {

            try
            {
                var producto = _db.WH_TipoProductoServicio.Single(p => p.idTipoProductoServicio == idTipoProducto);
                _db.WH_TipoProductoServicio.Remove(producto);
                var result = Save();
                result.data = producto.idTipoProductoServicio;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public IEnumerable<WH_TipoProductoServicio> listar()
        {
            try
            {
                return _db.WH_TipoProductoServicio;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
