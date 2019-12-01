using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ConexionBD
    {
        //protected BDFacturacionEntities contexto;
        //protected void Init(BDFacturacionEntities _contexto = null)
        //{
        //    contexto = _contexto ?? Helper.getDBContext;
        //}
        
        protected DBRouillonConsultinVentasEntities _db = null;

        public ConexionBD()
        {
            _db = new DBRouillonConsultinVentasEntities();
        }

        public OperationResult Save()
        {
            try
            {
                _db.SaveChanges();
                return new OperationResult(OperationResultEnum.Success);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new OperationResult(ex);
            }
            catch (EntityException e)
            {
                OperationResult result = new OperationResult(OperationResultEnum.Error, "Error al intentar guardar Datos, inentelo denuevo o comuníquese con un administrador.");
                result.exceptions.Add(e);
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }

        }

        public DateTime obtenerFechaActual()
        {
            return _db.Database.SqlQuery<DateTime>("Select GetDate() Fecha").SingleOrDefault();
        }
    }
}
