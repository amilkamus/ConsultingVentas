using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CO_ComprobanteDetalleDAT:ConexionBD
    {
        public OperationResult crear(CO_ComprobanteDetalle comprobanteDetalle)
        {
            try
            {
                _db.CO_ComprobanteDetalle.Add(comprobanteDetalle);
                var result = Save();
                result.data = comprobanteDetalle.idDetalleComprobante;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(CO_ComprobanteDetalle comprobanteDetalle)
        {
            try
            {
                CO_ComprobanteDetalle dbComprobanteDetalle = _db.CO_ComprobanteDetalle.Single(m => m.idDetalleComprobante == comprobanteDetalle.idDetalleComprobante);
                _db.Entry(dbComprobanteDetalle).CurrentValues.SetValues(comprobanteDetalle);
                var result = Save();
                result.data = comprobanteDetalle.idDetalleComprobante;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public IEnumerable<CO_ComprobanteDetalle> listar()
        {
            try
            {
                return _db.CO_ComprobanteDetalle;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<CO_ComprobanteDetalle> listarDetalle(int id)
        {
            try
            {
                return _db.CO_ComprobanteDetalle.Where(x=> x.idComprobante==id).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
