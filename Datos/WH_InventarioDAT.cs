using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class WH_InventarioDAT:ConexionBD
    {
        public OperationResult crear(WH_Inventario inventario)
        {
            try
            {
                _db.WH_Inventario.Add(inventario);
                var result = Save();
                result.data = inventario.idInventario;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(WH_Inventario inventario)
        {
            try
            {
                WH_Inventario dbInventario = _db.WH_Inventario.Single(m => m.idInventario == inventario.idInventario);
                _db.Entry(dbInventario).CurrentValues.SetValues(inventario);
                var result = Save();
                result.data = inventario.idInventario;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public IEnumerable<WH_Inventario> listar()
        {
            try
            {
                return _db.WH_Inventario;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
