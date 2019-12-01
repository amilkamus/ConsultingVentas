using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.TipoPersonas;
using Web.Utilitario;
using Datos;
using Negocio;

namespace Web.Controllers
{
    public class TipoPersonasController : Controller
    {
        TipoPersonaNEG tipoPersonaNEG = new TipoPersonaNEG();

        // GET: TipoPersonas
        [Authorize(Roles = "ADMINISTRADOR")]
        #region Agregar Tipo de Persona
        public ActionResult AddTipoPersona()
        {
            return View();
        }
        public JsonResult guardarTipoPersona(TipoPersonaViewModel tipoPersonaViewModel)
        {
            try
            {
                OperationResult resultado = null;
                if (tipoPersonaViewModel.idTipoPersona != 0)
                {
                    resultado = tipoPersonaNEG.actualizarTipoPersona(tipoPersonaViewModel.idTipoPersona, tipoPersonaViewModel.nombre, tipoPersonaViewModel.descripcion);
                }
                else
                {
                    resultado = tipoPersonaNEG.guardarTipoPersona(tipoPersonaViewModel.nombre, tipoPersonaViewModel.descripcion);
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
        #region Listar Tipo de Persona
        public ActionResult ViewAllTipoPersona()
        {
            return View();
        }
        public JsonResult listarTipoPersona()
        {
            try
            {
                List<TipoPersona> resultado = tipoPersonaNEG.listarTipoPersona();

                List<TipoPersonaViewModel> lista = TipoPersonaViewModel.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        #region Eliminar Tipo de Persona
        public JsonResult eliminarTipoPersona(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = tipoPersonaNEG.eliminarTipoPersona(id);

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
    }
}