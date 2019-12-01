using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.PersonaMaster;
using Web.Utilitario;
using Datos;
using Negocio;

namespace Web.Controllers
{
    public class PersonaMastController : Controller
    {
        PersonaMastNEG personaMastNEG = new PersonaMastNEG();

        // GET: PersonaMast
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult guardarPersonaMaster(PersonaMastViewModel personaMastViewModel)
        {
            try
            {
                OperationResult resultado = null;
                if (personaMastViewModel.idPersona != 0)
                {
                    resultado = personaMastNEG.guardarPersonaMast(personaMastViewModel.nombre,personaMastViewModel.apellidos,personaMastViewModel.idTipoPersona,personaMastViewModel.idTipoDocumento,personaMastViewModel.numeroDocumento,personaMastViewModel.cargo,personaMastViewModel.correo,personaMastViewModel.telefono,personaMastViewModel.domicilio,personaMastViewModel.estado);
                }
                else
                {
                    resultado = personaMastNEG.actualizarPersonaMast(personaMastViewModel.idPersona, personaMastViewModel.nombre, personaMastViewModel.apellidos, personaMastViewModel.idTipoPersona, personaMastViewModel.idTipoDocumento, personaMastViewModel.numeroDocumento, personaMastViewModel.cargo, personaMastViewModel.correo, personaMastViewModel.telefono, personaMastViewModel.domicilio, personaMastViewModel.estado);
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
    }
}