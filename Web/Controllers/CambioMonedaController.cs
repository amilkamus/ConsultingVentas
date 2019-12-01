using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.CambioMonedas;
using Web.Utilitario;
using Negocio;
using Datos;
using Web.Models;

namespace Web.Controllers
{
    public class CambioMonedaController : BaseController
    {
        CambioMonedaNEG cambioMonedaNEG = new CambioMonedaNEG();
        CO_MonedaNEG monedaNEG = new CO_MonedaNEG();

        // GET: UnidadMedida
        [Authorize(Roles = "ADMINISTRADOR")]
        #region Agregar Cambio de Moneda
        public ActionResult AddCambioMoneda()
        {
            return View();
        }

        public JsonResult guardarCambioMoneda(CambioMonedaViewModels cambioMonedaViewModels)
        {
            try
            {
                OperationResult resultado = null;
                if (cambioMonedaViewModels.idCambioMoneda != 0)
                {
                    resultado = cambioMonedaNEG.actualizarCambioMoneda(cambioMonedaViewModels.idCambioMoneda, cambioMonedaViewModels.idMoneda,cambioMonedaViewModels.compraMoneda,cambioMonedaViewModels.ventaMoneda, cambioMonedaViewModels.descripcion, cambioMonedaViewModels.estado,cambioMonedaViewModels.fechaRegistro,cambioMonedaViewModels.usuarioRegistro, IdUsuario());
                }
                else
                {
                    resultado = cambioMonedaNEG.guardarCambioMoneda(cambioMonedaViewModels.idMoneda, cambioMonedaViewModels.compraMoneda, cambioMonedaViewModels.ventaMoneda, cambioMonedaViewModels.descripcion, cambioMonedaViewModels.estado, IdUsuario());
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
        #region Listar Cambio de Moneda
        public ActionResult ViewAllCambioMoneda()
        {
            return View();
        }

        public JsonResult listarCambioMoneda()
        {
            try
            {
                List<CambioMoneda> resultado = cambioMonedaNEG.listarCambioMoneda();

                List<MostrarCambioMonedaViewModels> lista = MostrarCambioMonedaViewModels.convert(resultado);
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
        public JsonResult eliminarCambioMoneda(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = cambioMonedaNEG.eliminarCambioMoneda(id);

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

        #region Cargar Combos
        public JsonResult comboMoneda()
        {
            try
            {
                List<CO_Moneda> resultado = monedaNEG.listarMoneda();

                List<ComboGenericoViewModel> lista = new List<ComboGenericoViewModel>();
                foreach (var item in resultado)
                {
                    ComboGenericoViewModel combo = new ComboGenericoViewModel(item.idMoneda, item.descripcion);
                    lista.Add(combo);
                }

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