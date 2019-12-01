using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Egresos;
using Web.Utilitario;
using Negocio;
using Datos;

namespace Web.Controllers
{
    public class EgresoController : BaseController
    {
        CO_EgresoNEG egresoNEG = new CO_EgresoNEG();

        // GET: Egreso
        [Authorize(Roles = "ADMINISTRADOR")]
        #region Agregar Gastos
        public ActionResult AddEgreso()
        {
            return View();
        }

        public JsonResult guardarEgreso(EgresoViewModels egresoViewModels)
        {
            try
            {
                OperationResult resultado = null;
                if (egresoViewModels.idGasto != 0)
                {
                    resultado = egresoNEG.actualizarEgreso(egresoViewModels.idGasto, IdUsuario(), egresoViewModels.egreso, egresoViewModels.descripcion, egresoViewModels.cantidad, egresoViewModels.monto, egresoViewModels.total,egresoViewModels.fechaRegistro,egresoViewModels.usuarioRegistro, IdUsuario());
                }
                else
                {
                    resultado = egresoNEG.guardarEgreso(IdUsuario(), egresoViewModels.egreso, egresoViewModels.descripcion, egresoViewModels.cantidad, egresoViewModels.monto, egresoViewModels.total, IdUsuario());
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
        #region Listar Gastos
        public ActionResult ViewAllEgreso()
        {
            return View();
        }

        public JsonResult listarEgreso()
        {
            try
            {
                List<CO_Egreso> resultado = egresoNEG.listarEgreso();

                List<MostrarEgresoViewModels> lista = MostrarEgresoViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        #region Eliminar Gastos
        public JsonResult eliminarEgreso(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = egresoNEG.eliminarEgreso(id);

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