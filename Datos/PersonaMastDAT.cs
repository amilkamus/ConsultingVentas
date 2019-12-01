using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class PersonaMastDAT:ConexionBD
    {
        public OperationResult crear(PersonaMast persona)
        {
            try
            {
                _db.PersonaMast.Add(persona);
                var result = Save();
                result.data = persona.idPersona;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public OperationResult editar(PersonaMast persona)
        {
            try
            {
                PersonaMast dbPersona = _db.PersonaMast.Single(m => m.idPersona == persona.idPersona);
                _db.Entry(dbPersona).CurrentValues.SetValues(persona);
                var result = Save();
                result.data = persona.idPersona;
                return result;
            }
            catch (Exception e)
            {
                return new OperationResult(e);
            }
        }

        public IEnumerable<PersonaMast> listar()
        {
            try
            {
                return _db.PersonaMast;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public bool existePersonaMast(string numeroDocumento)
        {
            try
            {
                return _db.PersonaMast.Any(c => c.numeroDocumento == numeroDocumento);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int retornarId(string usuario)
        {
            int idPersona = 0;

            var query = from mi in _db.PersonaMast
                        where mi.nombre == usuario
                        select mi;

            foreach (var resul in query)
            {
                idPersona = Convert.ToInt32(resul.idPersona);
            }

            return idPersona;
        }
    }
}
