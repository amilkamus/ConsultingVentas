using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidad;

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
        
        public void ActualizarSerieCorrelativo(string serie, string tipoComprobante)
        {
            comprobanteDAT.ActualizarSerieCorrelativo(serie, tipoComprobante);
        }

        public void RegistrarCobranza(EnCobranza cobranza)
        {
            comprobanteDAT = new CO_ComprobanteDAT();
            comprobanteDAT.RegistrarCobranza(cobranza);
        }

        public EnCobranza ListarCobranzasPorCotizacion(long idCotizacion)
        {
            comprobanteDAT = new CO_ComprobanteDAT();
            return comprobanteDAT.ListarCobranzasPorCotizacion(idCotizacion);
        }

        public DataSet CargarVenta(long idComprobante)
        {
            return comprobanteDAT.CargarVenta(idComprobante);
        }

        public void ListarSerieCorrelativo(string tipoComprobante, ref string serie, ref long numero)
        {
            comprobanteDAT.ListarSerieCorrelativo(tipoComprobante, ref serie, ref numero);
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

        public string DevolverTipoComprobante(int idCorrelativo)
        {
            return comprobanteDAT.DevolverTipoComprobante(idCorrelativo);
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

        public List<EnCotizacionOut> ListarCotizaciones(EnContizacionIn cotizacionIn)
        {
            comprobanteDAT = new CO_ComprobanteDAT();
            return comprobanteDAT.ListarCotizaciones(cotizacionIn);
        }

        public List<EnCotizacionCertificado> ObtenerCertificadosCotizacion(long idCotizacion)
        {
            comprobanteDAT = new CO_ComprobanteDAT();
            return comprobanteDAT.ObtenerCertificadosCotizacion(idCotizacion);
        }

        public List<EnCotizacionInspeccion> ObtenerInspeccionesCotizacion(long idCotizacion)
        {
            comprobanteDAT = new CO_ComprobanteDAT();
            return comprobanteDAT.ObtenerInspeccionesCotizacion(idCotizacion);
        }

        public List<EnCotizacionResumen> ObtenerResumenesCotizacion(long idCotizacion)
        {
            comprobanteDAT = new CO_ComprobanteDAT();
            return comprobanteDAT.ObtenerResumenesCotizacion(idCotizacion);
        }

        public List<EnOrdenServicioOut> ListarOrdenServicio(EnOrdenServicioIn ordenServicioIn)
        {
            comprobanteDAT = new CO_ComprobanteDAT();
            return comprobanteDAT.ListarOrdenServicio(ordenServicioIn);
        }

        public List<EnCobranzaOut> ListarCobranzas(EnCobranzaIn cobranzaIn)
        {
            comprobanteDAT = new CO_ComprobanteDAT();
            return comprobanteDAT.ListarCobranzas(cobranzaIn);
        }

        public List<EnUsuario> ListarUsuarios()
        {
            comprobanteDAT = new CO_ComprobanteDAT();
            return comprobanteDAT.ListarUsuarios();
        }

    }
}
