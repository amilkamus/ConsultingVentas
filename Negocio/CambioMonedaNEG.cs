using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CambioMonedaNEG
    {
        CambioMonedaDAT cambioMonedaDAT;

        public CambioMonedaNEG()
        {
            cambioMonedaDAT = new CambioMonedaDAT();
        }

        public OperationResult guardarCambioMoneda(int idMoneda, double compra,double venta, string descripcion, string estado, string usuarioRegistro)
        {
            try
            {
                DateTime fechaRegistro = Convert.ToDateTime(DateTime.Now.ToString());

                CambioMoneda _cambioMoneda = new CambioMoneda();

                _cambioMoneda.idMoneda = idMoneda;
                _cambioMoneda.compraMoneda = compra;
                _cambioMoneda.ventaMoneda = venta;
                _cambioMoneda.descripcion = descripcion;
                _cambioMoneda.estado = estado;
                _cambioMoneda.fechaRegistro = fechaRegistro;
                _cambioMoneda.fechaActualizacion = fechaRegistro;
                _cambioMoneda.usuarioRegistro = usuarioRegistro;
                _cambioMoneda.usuarioActualizacion = usuarioRegistro;

                return cambioMonedaDAT.crear(_cambioMoneda);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarCambioMoneda(int idCambioMoneda, int idMoneda, double compra, double venta, string descripcion, string estado, DateTime fechaRegistro, string usuarioRegistro, string usuarioActualizacion)
        {
            try
            {
                DateTime fechaActualizacion = Convert.ToDateTime(DateTime.Now.ToString());

                CambioMoneda _cambioMoneda = new CambioMoneda();

                _cambioMoneda.idCambioMoneda = idCambioMoneda;
                _cambioMoneda.idMoneda = idMoneda;
                _cambioMoneda.compraMoneda = compra;
                _cambioMoneda.ventaMoneda = venta;
                _cambioMoneda.descripcion = descripcion;
                _cambioMoneda.estado = estado;
                _cambioMoneda.fechaRegistro = fechaRegistro;
                _cambioMoneda.fechaActualizacion = fechaActualizacion;
                _cambioMoneda.usuarioRegistro = usuarioRegistro;
                _cambioMoneda.usuarioActualizacion = usuarioActualizacion;

                return cambioMonedaDAT.editar(_cambioMoneda);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarCambioMoneda(int id)
        {
            try
            {
                return cambioMonedaDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public List<CambioMoneda> listarCambioMoneda()
        {
            try
            {
                return cambioMonedaDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
