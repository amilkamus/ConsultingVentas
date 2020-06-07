using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CO_ComprobanteNEG
    {
        CO_ComprobanteDAT comprobanteDAT;

        public CO_ComprobanteNEG()
        {
            comprobanteDAT = new CO_ComprobanteDAT();
        }

        public DataSet ObtenerCotizacionImpresion(long idCotizacion)
        {
            return comprobanteDAT.ObtenerCotizacionImpresion(idCotizacion);
        }
        public void ActualizarSerieCorrelativo(string serie)
        {
            comprobanteDAT.ActualizarSerieCorrelativo(serie);
        }

        public DataSet CargarVenta(long idComprobante)
        {
            return comprobanteDAT.CargarVenta(idComprobante);
        }

        public void ListarSerieCorrelativo(ref string serie, ref long numero)
        {
            comprobanteDAT.ListarSerieCorrelativo(ref serie, ref numero);
        }

        public OperationResult guardarComprobante(int idCorrelativo,string serieCorrlativo ,string descripcion, string estado, decimal subtotal, decimal total, string textoTotal, string usuarioRegistro, string idUsuario, int idCliente, int idMoneda, int idIGV)
        {
            try
            {
                DateTime fechaRegistro = Convert.ToDateTime(DateTime.Now.ToString());

                CO_Comprobante _comprobante = new CO_Comprobante();

                _comprobante.idCorrelativo = idCorrelativo;
                _comprobante.serieCorrelativo = serieCorrlativo;
                _comprobante.descripcion = descripcion;
                _comprobante.estado = estado;
                _comprobante.subTotal = subtotal;
                _comprobante.montoTotal = total;
                _comprobante.textoTotal = textoTotal;
                _comprobante.fechaRegistro = fechaRegistro;
                _comprobante.fechaActualizacion = fechaRegistro;
                _comprobante.usuarioRegistro = usuarioRegistro;
                _comprobante.usuarioActualizacion = usuarioRegistro;
                _comprobante.idUsuario = idUsuario;
                _comprobante.idCliente = idCliente;
                _comprobante.idMoneda = idMoneda;
                _comprobante.idIGV = idIGV;

                return comprobanteDAT.crear(_comprobante);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperationResult actualizarComprobante(int idComprobante, int idCorrelativo, string serieCorrlativo, string descripcion, string estado, decimal subtotal, decimal total, string textoTotal, DateTime fechaRegistro, string usuarioRegistro, string usuarioActualizacion, string idUsuario, int idCliente, int idMoneda, int idIGV)
        {
            try
            {
                DateTime fechaActualizacion = Convert.ToDateTime(DateTime.Now.ToString());

                CO_Comprobante _comprobante = new CO_Comprobante();

                _comprobante.idComprobante = idComprobante;
                _comprobante.idCorrelativo = idCorrelativo;
                _comprobante.serieCorrelativo = serieCorrlativo;
                _comprobante.descripcion = descripcion;
                _comprobante.estado = estado;
                _comprobante.subTotal = subtotal;
                _comprobante.montoTotal = total;
                _comprobante.textoTotal = textoTotal;
                _comprobante.fechaRegistro = fechaRegistro;
                _comprobante.fechaActualizacion = fechaActualizacion;
                _comprobante.usuarioRegistro = usuarioRegistro;
                _comprobante.usuarioActualizacion = usuarioActualizacion;
                _comprobante.idUsuario = idUsuario;
                _comprobante.idCliente = idCliente;
                _comprobante.idMoneda = idMoneda;
                _comprobante.idIGV = idIGV;

                return comprobanteDAT.editar(_comprobante);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public List<CO_Comprobante> listarComprobante()
        {
            try
            {
                return comprobanteDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int returnIdCorrelativo(int idComprobante)
        {
            return comprobanteDAT.returnIdCorrelativo(idComprobante);
        }

        public CO_Comprobante tmpComprobante(int idComprobante)
        {
            return comprobanteDAT.tmpComprobante(idComprobante);
        }
        
        public string tmpClienteCorreo(int idCliente)
        {
            return comprobanteDAT.tmpClienteCorreo(idCliente);
        }

        public CorrelativoMast tmpCorrelativo(int idCorrelativo)
        {
            return comprobanteDAT.tmpCorrelativo(idCorrelativo);
        }

        public CO_TipoComprobante tmpTipoComprobante(int idTipoComprobante)
        {
            try
            {
                return comprobanteDAT.tmpTipoComprobante(idTipoComprobante);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public string DevolverCorrelativoComprobante(int id)
        {
            return comprobanteDAT.DevolverCorrelativoComprobante(id);
        }

        public string DevolverSerieComprobante(int id)
        {            
            return comprobanteDAT.DevolverSerieComprobante(id);
        }


        /// <summary>
        /// Generando el numero de Bolestas/Facturas. ejmplo "BOL-0000001","FAC-0000001", ect
        /// </summary>
        /// <returns> Retorna la Serie y Correlativo de Boleta/Factura</returns>
        #region Generar Codigo        
        public int CodigoComprobante(int idCorreñativo)
        {
            int generadorCodigo = 0;
            int contarBoletas = comprobanteDAT.ContarComprobante(idCorreñativo);

            if (contarBoletas < 9)
            {
                generadorCodigo = Convert.ToInt32(comprobanteDAT.DevolverCorrelativoComprobante(idCorreñativo)) + (contarBoletas + 1);
            }
            else
            {
                if (contarBoletas >= 9)
                {
                    generadorCodigo = Convert.ToInt32(comprobanteDAT.DevolverCorrelativoComprobante(idCorreñativo)) + (contarBoletas + 1);
                }
                else
                {
                    if (contarBoletas >= 99)
                    {
                        generadorCodigo = Convert.ToInt32(comprobanteDAT.DevolverCorrelativoComprobante(idCorreñativo)) + (contarBoletas + 1);
                    }
                    else
                    {
                        if (contarBoletas >= 999)
                        {
                            generadorCodigo = Convert.ToInt32(comprobanteDAT.DevolverCorrelativoComprobante(idCorreñativo)) + (contarBoletas + 1);
                        }
                    }
                }
            }
            return generadorCodigo;
        }
        #endregion

    }
}
