using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ClienteDAT:ConexionBD
    {
        public OperationResult crear(Cliente cliente)
        {
            try
            {
                _db.Cliente.Add(cliente);
                var result = Save();
                result.data = cliente.idCliente;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(Cliente cliente)
        {
            try
            {
                Cliente dbCliente = _db.Cliente.Single(m => m.idCliente == cliente.idCliente);
                _db.Entry(dbCliente).CurrentValues.SetValues(cliente);
                var result = Save();
                result.data = cliente.idCliente;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idCliente)
        {

            try
            {
                var cliente = _db.Cliente.Single(p => p.idCliente == idCliente);
                _db.Cliente.Remove(cliente);
                var result = Save();
                result.data = cliente.idCliente;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Cliente> listar()
        {
            try
            {
                return _db.Cliente;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
                
        public bool existeCliente(string numeroDocumento)
        {
            try
            {
                return _db.PersonaMast.Any(c => c.numeroDocumento == numeroDocumento);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
