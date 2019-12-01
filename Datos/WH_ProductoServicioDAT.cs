using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class WH_ProductoServicioDAT:ConexionBD
    {
        public OperationResult crear(WH_ProductoServicio productoServicio)
        {
            try
            {
                _db.WH_ProductoServicio.Add(productoServicio);
                var result = Save();
                result.data = productoServicio.idProducto;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(WH_ProductoServicio productoServicio)
        {
            try
            {
                WH_ProductoServicio dbProductoServicio = _db.WH_ProductoServicio.Single(m => m.idProducto == productoServicio.idProducto);
                _db.Entry(dbProductoServicio).CurrentValues.SetValues(productoServicio);
                var result = Save();
                result.data = productoServicio.idProducto;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idProducto)
        {
            try
            {
                var producto = _db.WH_ProductoServicio.Single(p => p.idProducto == idProducto);
                _db.WH_ProductoServicio.Remove(producto);
                var result = Save();
                result.data = producto.idProducto;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public IEnumerable<WH_ProductoServicio> listar()
        {
            try
            {
                return _db.WH_ProductoServicio;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool existeProducto(string codigo)
        {
            try
            {
                return _db.WH_ProductoServicio.Any(c => c.codigo == codigo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public WH_ProductoServicio obtenerProducto(int id)
        {
            try
            {
                return _db.WH_ProductoServicio.SingleOrDefault(an => an.idProducto == id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
