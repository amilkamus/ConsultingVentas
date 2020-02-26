using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class WH_ProductoServicioNEG
    {
        WH_ProductoServicioDAT productoServicioDAT;

        public WH_ProductoServicioNEG()
        {
            productoServicioDAT = new WH_ProductoServicioDAT();
        }

        public DataTable ListarParametrosProducto(long idProducto)
        {
            WH_ProductoServicioDAT wH_ProductoServicioDAT = new WH_ProductoServicioDAT();
            return wH_ProductoServicioDAT.ListarParametrosProducto(idProducto);
        }

        public void InsertarParametrosProducto(long idProducto, DataTable parametros)
        {
            WH_ProductoServicioDAT wH_ProductoServicioDAT = new WH_ProductoServicioDAT();
            wH_ProductoServicioDAT.InsertarParametrosProducto(idProducto, parametros);
        }

        public OperationResult guardarProducto(int idTipoProducto, string codigo, string nombre, string descripcion, int stock, string estado, decimal costo, decimal precio, string usuarioRegistro)
        {
            try
            {
                DateTime fechaRegistro = Convert.ToDateTime(DateTime.Now.ToString());

                WH_ProductoServicio _producto = new WH_ProductoServicio();

                _producto.idTipoProductoServicio = idTipoProducto;
                _producto.codigo = codigo;
                _producto.nombre = nombre;
                _producto.descripcion = descripcion;
                _producto.stock = stock;
                _producto.estado = estado;
                _producto.costo = costo;
                _producto.precio = precio;
                _producto.fechaRegistro = fechaRegistro;
                _producto.fechaActualizacion = fechaRegistro;
                _producto.usuarioRegistro = usuarioRegistro;
                _producto.usuarioActualizacion = usuarioRegistro;

                return productoServicioDAT.crear(_producto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarProducto(int id, int idTipoProducto, string codigo, string nombre, string descripcion, int stock, string estado, decimal costo, decimal precio, DateTime fechaRegistro, string usuarioRegistro, string usuarioActualizacion)
        {
            try
            {
                DateTime fechaActualizacion = Convert.ToDateTime(DateTime.Now.ToString());

                WH_ProductoServicio _producto = new WH_ProductoServicio();

                _producto.idProducto = id;
                _producto.idTipoProductoServicio = idTipoProducto;
                _producto.codigo = codigo;
                _producto.nombre = nombre;
                _producto.descripcion = descripcion;
                _producto.stock = stock;
                _producto.estado = estado;
                _producto.costo = costo;
                _producto.precio = precio;
                _producto.fechaRegistro = fechaRegistro;
                _producto.fechaActualizacion = fechaActualizacion;
                _producto.usuarioRegistro = usuarioRegistro;
                _producto.usuarioActualizacion = usuarioActualizacion;

                return productoServicioDAT.editar(_producto);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarProducto(int id)
        {
            try
            {
                return productoServicioDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<WH_ProductoServicio> listarProducto()
        {
            try
            {
                return productoServicioDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public WH_ProductoServicio obtenerProducto(int id)
        {
            return productoServicioDAT.obtenerProducto(id);
        }
    }
}
