using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class WH_InventarioDetalleDAT:ConexionBD
    {
        public OperationResult crear(WH_InventarioDetalle inventarioDetalle)
        {
            try
            {
                _db.WH_InventarioDetalle.Add(inventarioDetalle);
                var result = Save();
                result.data = inventarioDetalle.idDetalleInventario;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(WH_InventarioDetalle inventarioDetalle)
        {
            try
            {
                WH_InventarioDetalle dbInventarioDetalle = _db.WH_InventarioDetalle.Single(m => m.idDetalleInventario == inventarioDetalle.idDetalleInventario);
                _db.Entry(dbInventarioDetalle).CurrentValues.SetValues(inventarioDetalle);
                var result = Save();
                result.data = inventarioDetalle.idDetalleInventario;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public IEnumerable<WH_InventarioDetalle> listar()
        {
            try
            {
                return _db.WH_InventarioDetalle;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
