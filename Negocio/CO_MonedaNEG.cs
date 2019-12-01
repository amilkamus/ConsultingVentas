using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CO_MonedaNEG
    {
        CO_MonedaDAT monedaDAT;

        public CO_MonedaNEG()
        {
            monedaDAT = new CO_MonedaDAT();
        }

        public OperationResult guardarMoneda( string descripcion)
        {
            try
            {
                CO_Moneda _moneda = new CO_Moneda();

                _moneda.descripcion = descripcion;

                return monedaDAT.crear(_moneda);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarMoneda(int idMoneda, string descripcion)
        {
            try
            {
                CO_Moneda _moneda = new CO_Moneda();

                _moneda.idMoneda = idMoneda;
                _moneda.descripcion = descripcion;

                return monedaDAT.editar(_moneda);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarMoneda(int id)
        {
            try
            {
                return monedaDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CO_Moneda> listarMoneda()
        {
            try
            {
                return monedaDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
