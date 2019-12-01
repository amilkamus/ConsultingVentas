using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Datos
{
    public class CompaniaDAT:ConexionBD
    {
        public OperationResult crear(Compania compania)
        {
            try
            {
                _db.Compania.Add(compania);
                var result = Save();
                result.data = compania.idCompania;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(Compania compania)
        {
            try
            {
                Compania dbCompania = _db.Compania.Single(m => m.idCompania == compania.idCompania);
                _db.Entry(dbCompania).CurrentValues.SetValues(compania);
                var result = Save();
                result.data = compania.idCompania;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public IEnumerable<Compania> listar()
        {
            try
            {
                return _db.Compania.Include(c => c.PersonaMast).Include(c => c.PersonaMast1);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public bool existeCompania(string ruc)
        {
            try
            {
                return _db.Compania.Any(c => c.ruc == ruc);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
