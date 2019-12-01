using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class TipoDocumentoNEG
    {
        TipoDocumentoDAT tipoDocumentoDAT;

        public TipoDocumentoNEG()
        {
            tipoDocumentoDAT = new TipoDocumentoDAT();
        }

        public OperationResult guardarDocumento(string nombre, string estado)
        {
            try
            {
                TipoDocumento _documento = new TipoDocumento();

                _documento.tipoDocumento1 = nombre;
                _documento.estado = estado;

                return tipoDocumentoDAT.crear(_documento);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarDocumento(int id, string nombre, string estado)
        {
            try
            {
                TipoDocumento _documento = new TipoDocumento();

                _documento.idTipoDocumento = id;
                _documento.tipoDocumento1 = nombre;
                _documento.estado = estado;

                return tipoDocumentoDAT.editar(_documento);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarDocumento(int id)
        {
            try
            {
                return tipoDocumentoDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TipoDocumento> listarDocumento()
        {
            try
            {
                return tipoDocumentoDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
