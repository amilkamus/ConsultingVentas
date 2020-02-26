using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class WH_ProductoServicioDAT : ConexionBD
    {
        public DataTable ListarParametrosProducto(long idProducto)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("usp_ListarParametrosProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idProducto", SqlDbType.BigInt).Value = idProducto;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                da.Fill(tbl);

                return tbl;
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
        public void InsertarParametrosProducto(long idProducto, DataTable parametros)
        {
            SqlConnection cn = new SqlConnection(_db.Database.Connection.ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("usp_InsertarParametrosProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idProducto", SqlDbType.BigInt).Value = idProducto;
                cmd.Parameters.Add("@parametroProducto", SqlDbType.Structured).Value = parametros;
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

        public OperationResult crear(WH_ProductoServicio productoServicio)
        {
            try
            {
                _db.WH_ProductoServicio.Add(productoServicio);
                var result = Save();
                result.data = productoServicio.idProducto;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(WH_ProductoServicio productoServicio)
        {
            try
            {
                WH_ProductoServicio dbProductoServicio = _db.WH_ProductoServicio.Single(m => m.idProducto == productoServicio.idProducto);
                _db.Entry(dbProductoServicio).CurrentValues.SetValues(productoServicio);
                var result = Save();
                result.data = productoServicio.idProducto;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public OperationResult eliminar(int idProducto)
        {
            try
            {
                var producto = _db.WH_ProductoServicio.Single(p => p.idProducto == idProducto);
                _db.WH_ProductoServicio.Remove(producto);
                var result = Save();
                result.data = producto.idProducto;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<WH_ProductoServicio> listar()
        {
            try
            {
                return _db.WH_ProductoServicio;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool existeProducto(string codigo)
        {
            try
            {
                return _db.WH_ProductoServicio.Any(c => c.codigo == codigo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public WH_ProductoServicio obtenerProducto(int id)
        {
            try
            {
                return _db.WH_ProductoServicio.SingleOrDefault(an => an.idProducto == id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
