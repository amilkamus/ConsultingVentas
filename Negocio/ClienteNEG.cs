using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class ClienteNEG
    {
        ClienteDAT clienteDAT;

        public ClienteNEG()
        {
            clienteDAT = new ClienteDAT();
        }

        public OperationResult guardarCliente(int idEmpresaCliente, int idTitular, string rubro, int idContacto)
        {
            try
            {
                Cliente _cliente = new Cliente();

                _cliente.idEmpresaCliente = idEmpresaCliente;
                _cliente.idTitular = idTitular;
                _cliente.rubro = rubro;
                _cliente.idContacto = idContacto;

                return clienteDAT.crear(_cliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarCliente(int id, int idEmpresaCliente, int idTitular, string rubro, int idContacto)
        {
            try
            {
                Cliente _cliente = new Cliente();

                _cliente.idCliente = id;
                _cliente.idEmpresaCliente = idEmpresaCliente;
                _cliente.idTitular = idTitular;
                _cliente.rubro = rubro;
                _cliente.idContacto = idContacto;

                return clienteDAT.editar(_cliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarCliente(int id)
        {
            try
            {
                return clienteDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Cliente> listarCliente()
        {
            try
            {
                return clienteDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
