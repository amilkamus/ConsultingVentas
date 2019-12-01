using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CO_EgresoNEG
    {
        CO_EgresoDAT egresoDAT;

        public CO_EgresoNEG()
        {
            egresoDAT = new CO_EgresoDAT();
        }

        public OperationResult guardarEgreso(string idUsuario, string egreso, string descripcion, int cantidad, decimal monto, decimal total, string usuarioRegistro)
        {
            try
            {
                DateTime fechaRegistro = Convert.ToDateTime(DateTime.Now.ToString());

                CO_Egreso _egreso = new CO_Egreso();

                _egreso.idUsuario = idUsuario;
                _egreso.egreso = egreso;
                _egreso.descripcion = descripcion;
                _egreso.cantidad = cantidad;
                _egreso.monto = monto;
                _egreso.total = total;
                _egreso.fechaRegistro = fechaRegistro;
                _egreso.fechaActualizacion = fechaRegistro;
                _egreso.usuarioRegistro = usuarioRegistro;
                _egreso.usuarioActualizacion = usuarioRegistro;

                return egresoDAT.crear(_egreso);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarEgreso(int id, string idUsuario, string egreso, string descripcion, int cantidad, decimal monto, decimal total,DateTime fechaRegistro, string usuarioRegistro, string usuarioActualizacion)
        {
            try
            {
                DateTime fechaActualizacion = Convert.ToDateTime(DateTime.Now.ToString());

                CO_Egreso _egreso = new CO_Egreso();

                _egreso.idGasto = id;
                _egreso.idUsuario = idUsuario;
                _egreso.egreso = egreso;
                _egreso.descripcion = descripcion;
                _egreso.cantidad = cantidad;
                _egreso.monto = monto;
                _egreso.total = total;
                _egreso.fechaRegistro = fechaRegistro;
                _egreso.fechaActualizacion = fechaActualizacion;
                _egreso.usuarioRegistro = usuarioRegistro;
                _egreso.usuarioActualizacion = usuarioActualizacion;

                return egresoDAT.editar(_egreso);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarEgreso(int id)
        {
            try
            {
                return egresoDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CO_Egreso> listarEgreso()
        {
            try
            {
                return egresoDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
