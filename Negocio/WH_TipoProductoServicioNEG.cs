using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class WH_TipoProductoServicioNEG
    {
        WH_TipoProductoServicioDAT tipoProductoServicioDAT;
        
        public WH_TipoProductoServicioNEG()
        {
            tipoProductoServicioDAT = new WH_TipoProductoServicioDAT();
        }

        public OperationResult guardarTipoProducto(string tipoProducto, string descripcion, string estado, string usuarioRegistro)
        {
            try
            {
                DateTime fechaRegistro = DateTime.Today;

                WH_TipoProductoServicio _tipoProducto = new WH_TipoProductoServicio();

                _tipoProducto.tipoProductoServicio = tipoProducto;
                _tipoProducto.descripcion = descripcion;
                _tipoProducto.estado = estado;
                _tipoProducto.fechaRegistro = fechaRegistro;
                _tipoProducto.fechaActualizacion = fechaRegistro;
                _tipoProducto.usuarioRegistro = usuarioRegistro;
                _tipoProducto.usuarioActualizaion = usuarioRegistro;

                return tipoProductoServicioDAT.crear(_tipoProducto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarTipoProducto(int id, string tipoProducto, string descripcion, string estado, string usuarioActualizacion)
        {
            try
            {
                DateTime fechaActualizacion = DateTime.Today;

                WH_TipoProductoServicio _tipoProducto = new WH_TipoProductoServicio();

                _tipoProducto.idTipoProductoServicio = id;
                _tipoProducto.tipoProductoServicio = tipoProducto;
                _tipoProducto.descripcion = descripcion;
                _tipoProducto.estado = estado;
                //_tipoProducto.fechaRegistro = null;
                _tipoProducto.fechaActualizacion = fechaActualizacion;
                //_tipoProducto.usuarioRegistro = usuarioRegistro;
                _tipoProducto.usuarioActualizaion = usuarioActualizacion;

                return tipoProductoServicioDAT.editar(_tipoProducto);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarTipoProducto(int id)
        {
            try
            {
                return tipoProductoServicioDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public List<WH_TipoProductoServicio> listarTipoProducto()
        {
            try
            {
                return tipoProductoServicioDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
