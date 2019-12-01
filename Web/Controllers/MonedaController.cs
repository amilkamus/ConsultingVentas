using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Monedas;
using Web.Utilitario;
using Negocio;
using Datos;

namespace Web.Controllers
{
    public class MonedaController : Controller
    {
        CO_MonedaNEG monedaNEG = new CO_MonedaNEG();
        
        // GET: Moneda
        [Authorize(Roles = "ADMINISTRADOR")]
        #region Agregar Moneda
        public ActionResult AddMoneda()
        {
            return View();
        }

        public JsonResult guardarMoneda(MonedaViewModels monedaViewModels)
        {
            try
            {
                OperationResult resultado = null;
                if (monedaViewModels.idMoneda != 0)
                {
                    resultado = monedaNEG.actualizarMoneda(monedaViewModels.idMoneda, monedaViewModels.descripcion);
                }
                else
                {
                    resultado = monedaNEG.guardarMoneda(monedaViewModels.descripcion);
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
        #region Listar Moneda
        public ActionResult ViewAllMoneda()
        {
            return View();
        }

        public JsonResult listarMoneda()
        {
            try
            {
                List<CO_Moneda> resultado = monedaNEG.listarMoneda();

                List<MonedaViewModels> lista = MonedaViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        #region Eliminar Cambio de Moneda
        public JsonResult eliminarMoneda(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = monedaNEG.eliminarMoneda(id);

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