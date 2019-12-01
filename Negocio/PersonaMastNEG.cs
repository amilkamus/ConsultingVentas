using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class PersonaMastNEG
    {
        PersonaMastDAT personaMastDAT;

        public PersonaMastNEG()
        {
            personaMastDAT = new PersonaMastDAT();
        }

        public OperationResult guardarPersonaMast(string nombre, string apellidos, int idTipoPersona, int idTipoDocumento, string numeroDocumento, string cargo, string corre, string telefono, string domicilio, string estado)
        {
            try
            {
                //if (!personaMastDAT.existePersonaMast(numeroDocumento))
                //{
                PersonaMast personaMast = new PersonaMast();

                personaMast.nombre = nombre;
                personaMast.apellidos = apellidos;
                personaMast.idTipoPersona = idTipoPersona;
                personaMast.idTipoDocumento = idTipoDocumento;
                personaMast.numeroDocumento = numeroDocumento;
                personaMast.cargo = cargo;
                personaMast.correo = corre;
                personaMast.telefono = telefono;
                personaMast.domicilio = domicilio;
                personaMast.estado = estado;

                return personaMastDAT.crear(personaMast);
                //}
                //else
                //{
                //    throw (new Exception("El producto ya existe"));
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }

        public OperationResult actualizarPersonaMast(int id, string nombre, string apellidos, int idTipoPersona, int idTipoDocumento, string numeroDocumento, string cargo, string corre, string telefono, string domicilio, string estado)
        {
            try
            {
                PersonaMast personaMast = new PersonaMast();

                personaMast.idPersona = id;
                personaMast.nombre = nombre;
                personaMast.apellidos = apellidos;
                personaMast.idTipoPersona = idTipoPersona;
                personaMast.idTipoDocumento = idTipoDocumento;
                personaMast.numeroDocumento = numeroDocumento;
                personaMast.cargo = cargo;
                personaMast.correo = corre;
                personaMast.telefono = telefono;
                personaMast.domicilio = domicilio;
                personaMast.estado = estado;

                return personaMastDAT.editar(personaMast);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
