﻿using Entidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CO_ComprobanteDAT : ConexionBD
    {
        public DataSet ObtenerCotizacionImpresion(long idCotizacion)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);
            try
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("ObtenerCotizacionImpresion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idCotizacion", System.Data.SqlDbType.BigInt).Value = idCotizacion;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                if (cn.State == System.Data.ConnectionState.Open)
                {
                    cn.Close();
                }
                throw e;
            }
        }

        public void ActualizarSerieCorrelativo(string serie, string tipoComprobante)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("ActualizarSerieCorrelativo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@serie", System.Data.SqlDbType.VarChar, 20).Value = serie;
                cmd.Parameters.Add("@tipoComprobante", SqlDbType.VarChar, 50).Value = tipoComprobante;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception e)
            {
                if (cn.State == System.Data.ConnectionState.Open)
                {
                    cn.Close();
                }
                throw e;
            }
        }

        public void RegistrarCobranza(EnCobranza cobranza)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("usp_RegistrarCobranza", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdCotizacion", SqlDbType.BigInt).Value = cobranza.IdCotizacion;
                cmd.Parameters.Add("@OS", SqlDbType.VarChar, 20).Value = cobranza.OS;
                cmd.Parameters.Add("@EjecutivoVenta", SqlDbType.VarChar, 400).Value = cobranza.EjecutivoVenta;
                cmd.Parameters.Add("@FechaIngreso", SqlDbType.VarChar, 20).Value = cobranza.FechaIngreso;
                cmd.Parameters.Add("@FechaPago", SqlDbType.VarChar, 20).Value = cobranza.FechaPago;
                cmd.Parameters.Add("@Detraccion", SqlDbType.Decimal).Value = cobranza.Detraccion;
                cmd.Parameters.Add("@TotalConDetraccion", SqlDbType.Decimal).Value = cobranza.TotalConDetraccion;
                cmd.Parameters.Add("@FechaPago1", SqlDbType.VarChar, 20).Value = cobranza.FechaPago1;
                cmd.Parameters.Add("@Importe1", SqlDbType.Decimal).Value = cobranza.Importe1;
                cmd.Parameters.Add("@FechaPago2", SqlDbType.VarChar, 20).Value = cobranza.FechaPago2;
                cmd.Parameters.Add("@Importe2", SqlDbType.Decimal).Value = cobranza.Importe2;
                cmd.Parameters.Add("@ImporteDebe2", SqlDbType.Decimal).Value = cobranza.ImporteDebe2;
                cmd.Parameters.Add("@PagoDetraccion", SqlDbType.Bit).Value = (cobranza.PagoDetraccion == "SI") ? true : false;
                cmd.Parameters.Add("@Observacion1", SqlDbType.VarChar, 5000).Value = cobranza.Observacion1;
                cmd.Parameters.Add("@Observacion2", SqlDbType.VarChar, 5000).Value = cobranza.Observacion2;
                cmd.Parameters.Add("@Autodetraccion", SqlDbType.VarChar, 20).Value = cobranza.Autodetraccion;
                cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 20).Value = cobranza.Tipo;
                cmd.Parameters.Add("@Saldo", SqlDbType.VarChar, 20).Value = cobranza.Saldo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.NVarChar, 128).Value = cobranza.IdUsuario;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception e)
            {
                if (cn.State == System.Data.ConnectionState.Open)
                {
                    cn.Close();
                }
                throw e;
            }
        }

        public EnCobranza ListarCobranzasPorCotizacion(long idCotizacion)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);

            try
            {
                EnCobranza cobranza = null;

                SqlCommand cmd = new SqlCommand("usp_ObtenerCobranza", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdCotizacion", SqlDbType.VarChar, 20).Value = idCotizacion;

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cobranza = new EnCobranza()
                    {
                        Autodetraccion = dr.GetString(dr.GetOrdinal("Autodetraccion")),
                        Detraccion = dr.GetDecimal(dr.GetOrdinal("Detraccion")),
                        EjecutivoVenta = dr.GetString(dr.GetOrdinal("EjecutivoVenta")),
                        FechaIngreso = dr.GetString(dr.GetOrdinal("FechaIngreso")),
                        FechaPago = dr.GetString(dr.GetOrdinal("FechaPago")),
                        FechaPago1 = dr.GetString(dr.GetOrdinal("FechaPago1")),
                        FechaPago2 = dr.GetString(dr.GetOrdinal("FechaPago2")),
                        IdCotizacion = dr.GetInt64(dr.GetOrdinal("IdCotizacion")),
                        Importe1 = dr.GetDecimal(dr.GetOrdinal("Importe1")),
                        Importe2 = dr.GetDecimal(dr.GetOrdinal("Importe2")),
                        ImporteDebe2 = dr.GetDecimal(dr.GetOrdinal("ImporteDebe2")),
                        Observacion1 = dr.GetString(dr.GetOrdinal("Observacion1")),
                        Observacion2 = dr.GetString(dr.GetOrdinal("Observacion2")),
                        OS = dr.GetString(dr.GetOrdinal("OS")),
                        PagoDetraccion = (dr.GetBoolean(dr.GetOrdinal("PagoDetraccion"))) ? "SI" : "NO",
                        Saldo = dr.GetString(dr.GetOrdinal("Saldo")),
                        Tipo = dr.GetString(dr.GetOrdinal("Tipo")),
                        TotalConDetraccion = dr.GetDecimal(dr.GetOrdinal("TotalConDetraccion"))
                    };
                }

                cn.Close();
                return cobranza;
            }
            catch (Exception e)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                throw e;
            }
        }

        public DataSet CargarVenta(long idComprobante)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);
            try
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("usp_CargarVenta", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idComprobante", SqlDbType.BigInt).Value = idComprobante;
                cn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                cn.Close();
                return ds;
            }
            catch (Exception e)
            {
                if (cn.State == System.Data.ConnectionState.Open)
                {
                    cn.Close();
                }
                throw e;
            }
        }

        public void ListarSerieCorrelativo(string tipoComprobante, ref string serie, ref long numero)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("ListarSerieCorrelativo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@TipoComprobante", SqlDbType.VarChar, 50).Value = tipoComprobante;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    serie = dr.GetString(dr.GetOrdinal("Serie"));
                    numero = dr.GetInt64(dr.GetOrdinal("Numero"));
                }

                cn.Close();
            }
            catch (Exception e)
            {
                if (cn.State == System.Data.ConnectionState.Open)
                {
                    cn.Close();
                }
                throw e;
            }
        }

        public List<EnCotizacionOut> ListarCotizaciones(EnContizacionIn cotizacionIn)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);

            try
            {
                List<EnCotizacionOut> cotizaciones = null;
                EnCotizacionOut cotizacion = null;

                SqlCommand cmd = new SqlCommand("usp_ListarCotizaciones", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TipoCotizacion", SqlDbType.VarChar, 20).Value = cotizacionIn.TipoCotizacion;
                cmd.Parameters.Add("@NumeroCotizacion", SqlDbType.VarChar, 200).Value = cotizacionIn.NumeroCotizacion;
                cmd.Parameters.Add("@RucSolicitante", SqlDbType.VarChar, 20).Value = cotizacionIn.RucSolicitante;
                cmd.Parameters.Add("@NombreSolicitante", SqlDbType.VarChar, 200).Value = cotizacionIn.NombreSolicitante;
                cmd.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = cotizacionIn.FechaInicio;
                cmd.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = cotizacionIn.FechaFin;
                cmd.Parameters.Add("@DescripcionProducto", SqlDbType.VarChar, 200).Value = cotizacionIn.DescripcionProducto;
                cmd.Parameters.Add("@ObservacionesProducto", SqlDbType.VarChar, 200).Value = cotizacionIn.ObservacionesProducto;
                cmd.Parameters.Add("@NombreContacto", SqlDbType.VarChar, 200).Value = cotizacionIn.NombreContacto;
                cmd.Parameters.Add("@SerieNumero", SqlDbType.VarChar, 12).Value = cotizacionIn.SerieNumero;
                cmd.Parameters.Add("@IdUsuarioRegistro", SqlDbType.NVarChar, 128).Value = cotizacionIn.IdUsuarioRegistro;

                cn.Open();

                cotizaciones = new List<EnCotizacionOut>();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cotizacion = new EnCotizacionOut()
                    {
                        ID = dr.GetInt64(dr.GetOrdinal("ID")),
                        TipoCotizacion = dr.GetString(dr.GetOrdinal("TipoCotizacion")),
                        NumeroCotizacion = dr.GetString(dr.GetOrdinal("NumeroCotizacion")),
                        RUC = dr.GetString(dr.GetOrdinal("RUC")),
                        Solicitante = dr.GetString(dr.GetOrdinal("Solicitante")),
                        Fecha = dr.GetString(dr.GetOrdinal("Fecha")),
                        DescripcionProducto = dr.GetString(dr.GetOrdinal("DescripcionProducto")),
                        Observaciones = dr.GetString(dr.GetOrdinal("Observaciones")),
                        Contacto = dr.GetString(dr.GetOrdinal("Contacto")),
                        SerieNumero = dr.GetString(dr.GetOrdinal("SerieNumero")),
                        UsuarioRegistro = dr.GetString(dr.GetOrdinal("UsuarioRegistro"))
                    };
                    cotizaciones.Add(cotizacion);
                }

                cn.Close();
                return cotizaciones;
            }
            catch (Exception e)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                throw e;
            }
        }

        public List<EnOrdenServicioOut> ListarOrdenServicio(EnOrdenServicioIn ordenServicioIn)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);

            try
            {
                List<EnOrdenServicioOut> ordenesServicio = null;
                EnOrdenServicioOut ordenServicio = null;

                SqlCommand cmd = new SqlCommand("usp_ListarOrdenServicios", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NumeroOrdenServicio", SqlDbType.VarChar, 20).Value = ordenServicioIn.NumeroOrdenServicio;
                cmd.Parameters.Add("@NumeroCotizacion", SqlDbType.VarChar, 200).Value = ordenServicioIn.NumeroCotizacion;
                cmd.Parameters.Add("@RucSolicitante", SqlDbType.VarChar, 20).Value = ordenServicioIn.RucSolicitante;
                cmd.Parameters.Add("@NombreSolicitante", SqlDbType.VarChar, 200).Value = ordenServicioIn.NombreSolicitante;
                cmd.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = ordenServicioIn.FechaInicio;
                cmd.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = ordenServicioIn.FechaFin;
                cmd.Parameters.Add("@DescripcionProducto", SqlDbType.VarChar, 200).Value = ordenServicioIn.DescripcionProducto;
                cmd.Parameters.Add("@ObservacionesProducto", SqlDbType.VarChar, 200).Value = ordenServicioIn.ObservacionesProducto;
                cmd.Parameters.Add("@NombreContacto", SqlDbType.VarChar, 200).Value = ordenServicioIn.NombreContacto;
                cmd.Parameters.Add("@SerieNumero", SqlDbType.VarChar, 12).Value = ordenServicioIn.SerieNumero;
                cmd.Parameters.Add("@IdUsuarioRegistro", SqlDbType.NVarChar, 128).Value = ordenServicioIn.IdUsuarioRegistro;

                cn.Open();

                ordenesServicio = new List<EnOrdenServicioOut>();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ordenServicio = new EnOrdenServicioOut()
                    {
                        ID = dr.GetInt64(dr.GetOrdinal("ID")),
                        DescripcionProducto = dr.GetString(dr.GetOrdinal("DescripcionProducto")),
                        Fecha = dr.GetString(dr.GetOrdinal("Fecha")),
                        NumeroCotizacion = dr.GetString(dr.GetOrdinal("NumeroCotizacion")),
                        NumeroOrdenServicio = dr.GetString(dr.GetOrdinal("NumeroOrdenServicio")),
                        Observaciones = dr.GetString(dr.GetOrdinal("Observaciones")),
                        ObservacionesInforme = dr.GetString(dr.GetOrdinal("ObservacionesInforme")),
                        RUC = dr.GetString(dr.GetOrdinal("RUC")),
                        Solicitante = dr.GetString(dr.GetOrdinal("Solicitante")),
                        UsuarioRegistro = dr.GetString(dr.GetOrdinal("UsuarioRegistro"))
                    };
                    ordenesServicio.Add(ordenServicio);
                }

                cn.Close();
                return ordenesServicio;
            }
            catch (Exception e)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                throw e;
            }
        }

        public List<EnCobranzaOut> ListarCobranzas(EnCobranzaIn cobranzaIn)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);

            try
            {
                List<EnCobranzaOut> cobranzas = null;
                EnCobranzaOut cobranza = null;

                SqlCommand cmd = new SqlCommand("usp_ListarCobranzas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NumeroOrdenServicio", SqlDbType.VarChar, 20).Value = cobranzaIn.NumeroOrdenServicio;
                cmd.Parameters.Add("@NumeroCotizacion", SqlDbType.VarChar, 200).Value = cobranzaIn.NumeroCotizacion;
                cmd.Parameters.Add("@RucSolicitante", SqlDbType.VarChar, 20).Value = cobranzaIn.RucSolicitante;
                cmd.Parameters.Add("@NombreSolicitante", SqlDbType.VarChar, 200).Value = cobranzaIn.NombreSolicitante;
                cmd.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = cobranzaIn.FechaInicio;
                cmd.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = cobranzaIn.FechaFin;
                cmd.Parameters.Add("@DescripcionProducto", SqlDbType.VarChar, 200).Value = cobranzaIn.DescripcionProducto;
                cmd.Parameters.Add("@ObservacionesProducto", SqlDbType.VarChar, 200).Value = cobranzaIn.ObservacionesProducto;
                cmd.Parameters.Add("@NombreContacto", SqlDbType.VarChar, 200).Value = cobranzaIn.NombreContacto;
                cmd.Parameters.Add("@SerieNumero", SqlDbType.VarChar, 12).Value = cobranzaIn.SerieNumero;
                cmd.Parameters.Add("@IdUsuarioRegistro", SqlDbType.NVarChar, 128).Value = cobranzaIn.IdUsuarioRegistro;

                cn.Open();

                cobranzas = new List<EnCobranzaOut>();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cobranza = new EnCobranzaOut()
                    {
                        IdCotizacion = dr.GetInt64(dr.GetOrdinal("IdCotizacion")),
                        Mes = dr.GetString(dr.GetOrdinal("Mes")),
                        TipoCotizacion = dr.GetString(dr.GetOrdinal("TipoCotizacion")),
                        NumeroOrdenServicio = dr.GetString(dr.GetOrdinal("NumeroOrdenServicio")),
                        NumeroCotizacion = dr.GetString(dr.GetOrdinal("NumeroCotizacion")),
                        EjecutivoVenta = dr.GetString(dr.GetOrdinal("EjecutivoVenta")),
                        Solicitante = dr.GetString(dr.GetOrdinal("Solicitante")),
                        DescripcionProducto = dr.GetString(dr.GetOrdinal("DescripcionProducto")),
                        Contacto = dr.GetString(dr.GetOrdinal("Contacto")),
                        Email = dr.GetString(dr.GetOrdinal("Email")),
                        Telefono = dr.GetString(dr.GetOrdinal("Telefono")),
                        OS = dr.GetString(dr.GetOrdinal("OS")),
                        SerieNumero = dr.GetString(dr.GetOrdinal("SerieNumero")),
                        CondicionPago_1 = dr.GetString(dr.GetOrdinal("CondicionPago_1")),
                        CondicionPago_2 = dr.GetString(dr.GetOrdinal("CondicionPago_2")),
                        FechaIngreso = dr.GetString(dr.GetOrdinal("FechaIngreso")),
                        FechaPago = dr.GetString(dr.GetOrdinal("FechaPago")),
                        Total = dr.GetDecimal(dr.GetOrdinal("Total")),
                        Detraccion = dr.GetDecimal(dr.GetOrdinal("Detraccion")),
                        TotalConDetraccion = dr.GetDecimal(dr.GetOrdinal("TotalConDetraccion")),
                        FechaPago1 = dr.GetString(dr.GetOrdinal("FechaPago1")),
                        Importe1 = dr.GetDecimal(dr.GetOrdinal("Importe1")),
                        FechaPago2 = dr.GetString(dr.GetOrdinal("FechaPago2")),
                        Importe2 = dr.GetDecimal(dr.GetOrdinal("Importe2")),
                        ImporteDebe2 = dr.GetDecimal(dr.GetOrdinal("ImporteDebe2")),
                        PagoDetraccion = dr.GetString(dr.GetOrdinal("PagoDetraccion")),
                        Saldo = dr.GetString(dr.GetOrdinal("Saldo")),
                        Observacion1 = dr.GetString(dr.GetOrdinal("Observacion1")),
                        Observacion2 = dr.GetString(dr.GetOrdinal("Observacion2")),
                        Autodetraccion = dr.GetString(dr.GetOrdinal("Autodetraccion")),
                        Tipo = dr.GetString(dr.GetOrdinal("Tipo"))
                    };
                    cobranzas.Add(cobranza);
                }

                cn.Close();
                return cobranzas;
            }
            catch (Exception e)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                throw e;
            }
        }

        public List<EnUsuario> ListarUsuarios()
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);

            try
            {
                List<EnUsuario> usuarios = null;
                EnUsuario usuario = null;

                SqlCommand cmd = new SqlCommand("usp_ListarUsuarios", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                usuarios = new List<EnUsuario>();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    usuario = new EnUsuario()
                    {
                        ID = dr.GetString(dr.GetOrdinal("ID")),
                        Nombre = dr.GetString(dr.GetOrdinal("Nombre"))
                    };
                    usuarios.Add(usuario);
                }

                cn.Close();
                return usuarios;
            }
            catch (Exception e)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                throw e;
            }
        }

        public OperationResult crear(CO_Comprobante comprobante)
        {
            try
            {
                _db.CO_Comprobante.Add(comprobante);
                var result = Save();
                result.data = comprobante.idComprobante;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(CO_Comprobante comprobante)
        {
            try
            {
                CO_Comprobante dbComprobante = _db.CO_Comprobante.Single(m => m.idComprobante == comprobante.idComprobante);
                _db.Entry(dbComprobante).CurrentValues.SetValues(comprobante);
                var result = Save();
                result.data = comprobante.idComprobante;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public IEnumerable<CO_Comprobante> listar()
        {
            try
            {
                return _db.CO_Comprobante;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int returnIdCorrelativo(int idComprobante)
        {
            int id = 0;

            var query = from mi in _db.CO_Comprobante
                        where mi.idComprobante == idComprobante
                        select mi;


            foreach (var resul in query)
            {
                id = Convert.ToInt32(resul.idCorrelativo);
            }

            return id;
        }

        public CO_Comprobante tmpComprobante(int idComprobante)
        {
            try
            {
                return _db.CO_Comprobante.Where(z => z.idComprobante == idComprobante).First();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string tmpClienteCorreo(int idCliente)
        {
            try
            {
                string email = "";

                var query = from cli in _db.Cliente
                            join pm in _db.PersonaMast on cli.idEmpresaCliente equals pm.idPersona
                            where cli.idCliente == idCliente
                            select pm;

                foreach (var result in query)
                {
                    email = result.correo;
                }

                return email;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public CorrelativoMast tmpCorrelativo(int idCorrelativo)
        {
            try
            {
                return _db.CorrelativoMast.Where(z => z.idCorrelativo == idCorrelativo).First();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CO_TipoComprobante tmpTipoComprobante(int idTipoComprobante)
        {
            try
            {
                return _db.CO_TipoComprobante.Where(z => z.idTipoComprobante == idTipoComprobante).First();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ContarComprobante(int idCorrelativo)
        {
            var cantidadComprobantes = (from miVentas in _db.CO_Comprobante
                                        where miVentas.idCorrelativo == idCorrelativo
                                        select miVentas.idCorrelativo).Count();
            return cantidadComprobantes;
        }

        public string DevolverCorrelativoComprobante(int idCorrelativo)
        {
            string miNumeroComprobante = "";

            var query = from miFactura in _db.CorrelativoMast
                        where miFactura.idCorrelativo == idCorrelativo
                        select miFactura;

            foreach (var configuracion in query)
            {
                miNumeroComprobante = Convert.ToString(configuracion.correlativo);
            }
            return miNumeroComprobante;
        }

        public string DevolverSerieComprobante(int idCorreñativo)
        {
            string miSerieComprobante = "";

            var query = from miFactura in _db.CorrelativoMast
                        where miFactura.idCorrelativo == idCorreñativo
                        select miFactura;

            foreach (var configuracion in query)
            {
                miSerieComprobante = Convert.ToString(configuracion.serie);
            }
            return miSerieComprobante;
        }

        public string DevolverTipoComprobante(int idCorrelativo)
        {
            int? idTipoComprobante = (from miFactura in _db.CorrelativoMast
                                      where miFactura.idCorrelativo == idCorrelativo
                                      select miFactura.idTipoComprobante).FirstOrDefault();

            CO_TipoComprobante tipoComprobante = _db.CO_TipoComprobante.Where(z => z.idTipoComprobante == idTipoComprobante.Value).First();
            return tipoComprobante.tipoComprobante;
        }

    }
}
