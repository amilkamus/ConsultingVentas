using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CO_ComprobanteDetalleNEG
    {
        CO_ComprobanteDetalleDAT comprobanteDetalleDAT;

        public CO_ComprobanteDetalleNEG()
        {
            comprobanteDetalleDAT = new CO_ComprobanteDetalleDAT();
        }

        public OperationResult guardarComprobanteDetalle(int idProducto, int idComprobante, int cantidad, decimal precio, decimal montoUnitario)
        {
            try
            {
                CO_ComprobanteDetalle _comprobanteDetalle = new CO_ComprobanteDetalle();

                _comprobanteDetalle.idProducto = idProducto;
                _comprobanteDetalle.idComprobante = idComprobante;
                _comprobanteDetalle.cantidad = cantidad;
                _comprobanteDetalle.precio = precio;
                _comprobanteDetalle.montoUnitario = montoUnitario;

                return comprobanteDetalleDAT.crear(_comprobanteDetalle);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarComprobanteDetalle(int idDetalleComprobante, int idProducto, int idComprobante, int cantidad, decimal precio, decimal montoUnitario)
        {
            try
            {
                CO_ComprobanteDetalle _comprobanteDetalle = new CO_ComprobanteDetalle();

                _comprobanteDetalle.idDetalleComprobante = idDetalleComprobante;
                _comprobanteDetalle.idProducto = idProducto;
                _comprobanteDetalle.idComprobante = idComprobante;
                _comprobanteDetalle.cantidad = cantidad;
                _comprobanteDetalle.precio = precio;
                _comprobanteDetalle.montoUnitario = montoUnitario;

                return comprobanteDetalleDAT.editar(_comprobanteDetalle);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CO_ComprobanteDetalle> listarComprobanteDetalle(int id)
        {
            try
            {
                return comprobanteDetalleDAT.listarDetalle(id).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
