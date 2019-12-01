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

        public void ActualizarSerieCorrelativo(string serie)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("ActualizarSerieCorrelativo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@serie", System.Data.SqlDbType.VarChar, 20).Value = serie;
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

        public void ListarSerieCorrelativo(ref string serie, ref long numero)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("ListarSerieCorrelativo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
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

    }
}
