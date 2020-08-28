using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.Models.Compania;
using Web.Utilitario;
using Negocio;
using Datos;

namespace Web.Controllers
{
    public class CompaniaController : BaseController
    {
        CompaniaNEG companiaNEG = new CompaniaNEG();
        PersonaMastNEG personaMastNEG = new PersonaMastNEG();

        // GET: Compania
        [Authorize(Roles = "ADMINISTRADOR")]
        #region Agregar Compañia
        public ActionResult AddCompania()
        {
            return View();
        }

        public JsonResult guardarCompania(RegistroCompaniaViewModel companiaViewModel)
        {
            long IdTitular = 0;
            long IdContacto = 0;

            try
            {
                OperationResult resultado = null;
                if (companiaViewModel.idCompania != 0)
                {
                    resultado = personaMastNEG.actualizarPersonaMast(companiaViewModel.idTitular,companiaViewModel.titularNombre, companiaViewModel.titularApellidos, 1, 2, null, companiaViewModel.titularCargo, companiaViewModel.titularCorreo, companiaViewModel.titularTelefono, null, "ACTIVO");
                    IdTitular = resultado.data;
                    resultado = personaMastNEG.actualizarPersonaMast(companiaViewModel.idContacto,companiaViewModel.contactoNombre, companiaViewModel.contactoApellidos, 1, 2, null, companiaViewModel.contactoCargo, companiaViewModel.contactoCorreo, companiaViewModel.contactoTelefono, null, "ACTIVADO");
                    IdContacto = resultado.data;
                    resultado = companiaNEG.actualizarCompania(companiaViewModel.idCompania,companiaViewModel.razonSocial, companiaViewModel.ruc, companiaViewModel.domicilioFiscal, companiaViewModel.correo, Convert.ToInt32(IdTitular), Convert.ToInt32(IdContacto), IdUsuario());
                }
                else
                {
                    resultado = personaMastNEG.guardarPersonaMast(companiaViewModel.titularNombre, companiaViewModel.titularApellidos, 1, 2, null, companiaViewModel.titularCargo, companiaViewModel.titularCorreo, companiaViewModel.titularTelefono,null, "ACTIVO");
                    IdTitular = resultado.data;
                    resultado = personaMastNEG.guardarPersonaMast(companiaViewModel.contactoNombre, companiaViewModel.contactoApellidos, 1, 2, null, companiaViewModel.contactoCargo, companiaViewModel.contactoCorreo, companiaViewModel.contactoTelefono,null, "ACTIVADO");
                    IdContacto = resultado.data;
                    resultado = companiaNEG.guardarCompania(companiaViewModel.razonSocial, companiaViewModel.ruc, companiaViewModel.domicilioFiscal, companiaViewModel.correo, Convert.ToInt32(IdTitular), Convert.ToInt32(IdContacto), IdUsuario());
                }

                Util.verificarError(resultado);
                return Json(new { code_result = resultado.code_result, data = resultado.data, result_description = resultado.title });
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        [Authorize(Roles = "ADMINISTRADOR")]
        #region Listar Compañia
        public ActionResult ViewAllCompania()
        {
            return View();
        }

        public JsonResult listarCompania()
        {
            try
            {
                List<Compania> resultado = companiaNEG.listarCompania();

                List<MostrarCompaniaViewModel> lista = MostrarCompaniaViewModel.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion
               
    }
}