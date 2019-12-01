using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;

namespace Negocio
{
    public class CompaniaNEG
    {
        CompaniaDAT companiaDAT;

        public CompaniaNEG()
        {
            companiaDAT = new CompaniaDAT();
        }

        public OperationResult guardarCompania(string razonSocial, string ruc, string domicilioFiscal, string correo, int idTitular, int idContacto,string usuarioRegistro)
        {
            try
            {
                DateTime fechaRegistro = DateTime.Today;

                //if (!companiaDAT.existeCompania(ruc))
                //{
                    Compania compania = new Compania();

                    compania.razonSocial = razonSocial;
                    compania.ruc = ruc;
                    compania.domicilioFiscal = domicilioFiscal;
                    compania.correo = correo;
                    compania.idTitular = idTitular;
                    compania.idContacto = idContacto;
                    compania.fechaRegistro = fechaRegistro;
                    compania.fechaActualizacion = fechaRegistro;
                    compania.usuarioRegistro = usuarioRegistro;
                    compania.usuarioActualizacion = usuarioRegistro;

                    return companiaDAT.crear(compania);
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

        public OperationResult actualizarCompania(int id, string razonSocial, string ruc, string domicilioFiscal, string correo, int idTitular, int idContacto, string usuarioActualizacion)
        {
            try
            {
                DateTime fechaActualizacion = DateTime.Today;

                Compania compania = new Compania();

                compania.idCompania = id;
                compania.razonSocial = razonSocial;
                compania.ruc = ruc;
                compania.domicilioFiscal = domicilioFiscal;
                compania.correo = correo;
                compania.idTitular = idTitular;
                compania.idContacto = idContacto;
                //compania.fechaRegistro = fechaRegistro;
                compania.fechaActualizacion = fechaActualizacion;
                //compania.usuarioRegistro = usuarioRegistro;
                compania.usuarioActualizacion = usuarioActualizacion;

                return companiaDAT.editar(compania);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Compania> listarCompania()
        {
            try
            {
                return companiaDAT.listar().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
