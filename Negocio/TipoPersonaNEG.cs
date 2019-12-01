using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class TipoPersonaNEG
    {
        TipoPersonaDAT tipoPersonaDAT;

        public TipoPersonaNEG()
        {
            tipoPersonaDAT = new TipoPersonaDAT();
        }

        public OperationResult guardarTipoPersona(string nombre, string descripcion)
        {
            try
            {
                TipoPersona _tipoPersona = new TipoPersona();

                _tipoPersona.tipoPersona1= nombre;
                _tipoPersona.descripcion = descripcion;

                return tipoPersonaDAT.crear(_tipoPersona);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarTipoPersona(int id, string nombre, string descripcion)
        {
            try
            {
                TipoPersona _tipoPersona = new TipoPersona();

                _tipoPersona.idTipoPersona = id;
                _tipoPersona.tipoPersona1 = nombre;
                _tipoPersona.descripcion = descripcion;

                return tipoPersonaDAT.editar(_tipoPersona);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarTipoPersona(int id)
        {
            try
            {
                return tipoPersonaDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TipoPersona> listarTipoPersona()
        {
            try
            {
                return tipoPersonaDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
