using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CajaNEG
    {
        Cre cajaDAT;

        public CajaNEG()
        {
            cajaDAT = new Cre();
        }

        public OperationResult guardarCaja(string idUsuario, decimal ingreso, decimal saldonInicial, decimal saldoFinal, string usuarioRegistro, string estado)
        {
            try
            {
                DateTime fechaRegistro = DateTime.Today;
                
                Caja _caja = new Caja();

                _caja.idUsuario = idUsuario;
                _caja.ingresos = ingreso;
                _caja.saldonIcial = saldonInicial;
                _caja.saldoFinal = saldoFinal;
                _caja.fechaRegistro = fechaRegistro;
                _caja.fechaActualizacion = fechaRegistro;
                _caja.usuarioRegistro = usuarioRegistro;
                _caja.usuarioActualizacion = usuarioRegistro;
                _caja.estado = estado;

                return cajaDAT.crear(_caja);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarCaja(int id, string idUsuario, decimal ingreso, decimal saldonInicial, decimal saldoFinal, string usuarioActualizacion, string estado)
        {
            try
            {
                DateTime fechaActualizacion = DateTime.Today;

                Caja _caja = new Caja();

                _caja.idCaja = id;
                _caja.idUsuario = idUsuario;
                _caja.ingresos = ingreso;
                _caja.saldonIcial = saldonInicial;
                _caja.saldoFinal = saldoFinal;
                //_caja.fechaRegistro = fechaRegistro;
                _caja.fechaActualizacion = fechaActualizacion;
                //_caja.usuarioRegistro = usuarioRegistro;
                _caja.usuarioActualizacion = usuarioActualizacion;
                _caja.estado = estado;

                return cajaDAT.editar(_caja);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarCaja(int id)
        {
            try
            {
                return cajaDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public List<Caja> listarCaja()
        {
            try
            {
                return cajaDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
