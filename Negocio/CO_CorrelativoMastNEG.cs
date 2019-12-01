using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CO_CorrelativoMastNEG
    {
        CO_CorrelativoMastDAT correlativoMasterDAT;

        public CO_CorrelativoMastNEG()
        {
            correlativoMasterDAT = new CO_CorrelativoMastDAT();
        }

        public OperationResult guardarCorrelativoMast(int idTipoComprobante, string serie, string correlativo)
        {
            try
            {
                CorrelativoMast _correlativoMast = new CorrelativoMast();

                _correlativoMast.idTipoComprobante = idTipoComprobante;
                _correlativoMast.serie = serie;
                _correlativoMast.correlativo = correlativo;

                return correlativoMasterDAT.crear(_correlativoMast);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarCorrelativoMast(int id, int idTipoComprobante, string serie, string correlativo)
        {
            try
            {
                CorrelativoMast _correlativoMast = new CorrelativoMast();

                _correlativoMast.idCorrelativo = id;
                _correlativoMast.idTipoComprobante = idTipoComprobante;
                _correlativoMast.serie = serie;
                _correlativoMast.correlativo = correlativo;

                return correlativoMasterDAT.editar(_correlativoMast);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult eliminarCorrelativoMast(int id)
        {
            try
            {
                return correlativoMasterDAT.eliminar(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CorrelativoMast> listarCorrelativoMast()
        {
            try
            {
                return correlativoMasterDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
