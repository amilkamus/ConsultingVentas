using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CO_TipoComprobanteNEG
    {
        CO_TipoComprobanteDAT tipoComprobanteDAT;

        public CO_TipoComprobanteNEG()
        {
            tipoComprobanteDAT = new CO_TipoComprobanteDAT();
        }

        public OperationResult guardarTipoComprobante(string tipoComprobante,string descripcion, string estado)
        {
            try
            {
                CO_TipoComprobante _tipoComprobante = new CO_TipoComprobante();

                _tipoComprobante.tipoComprobante = tipoComprobante;
                _tipoComprobante.descripcion = descripcion;
                _tipoComprobante.estado = estado;

                return tipoComprobanteDAT.crear(_tipoComprobante);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarTipoComprobante(int id, string tipoComprobante, string descripcion, string estado)
        {
            try
            {
                CO_TipoComprobante _tipoComprobante = new CO_TipoComprobante();

                _tipoComprobante.idTipoComprobante = id;
                _tipoComprobante.tipoComprobante = tipoComprobante;
                _tipoComprobante.descripcion = descripcion;
                _tipoComprobante.estado = estado;

                return tipoComprobanteDAT.editar(_tipoComprobante);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarTipoComprobante(int id)
        {
            try
            {
                return tipoComprobanteDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public List<CO_TipoComprobante> listarTipoComprobante()
        {
            try
            {
                return tipoComprobanteDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
