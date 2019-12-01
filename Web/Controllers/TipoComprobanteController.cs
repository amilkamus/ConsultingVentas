using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.TipoComprobantes;
using Web.Utilitario;
using Datos;
using Negocio;

namespace Web.Controllers
{
    public class TipoComprobanteController : Controller
    {
        CO_TipoComprobanteNEG tipoComprobanteNEG = new CO_TipoComprobanteNEG();

        // GET: TipoComprobante
        [Authorize(Roles = "ADMINISTRADOR")]
        #region Agregar Tipo de Comprobante
        public ActionResult AddTipoComprobante()
        {
            return View();
        }

        public JsonResult guardarTipoComprobante(TipoComprobanteViewModels tipoComprobanteViewModel)
        {
            try
            {
                OperationResult resultado = null;
                if (tipoComprobanteViewModel.idTipoComprobante != 0)
                {
                    resultado = tipoComprobanteNEG.actualizarTipoComprobante(tipoComprobanteViewModel.idTipoComprobante, tipoComprobanteViewModel.tipoComprobante, tipoComprobanteViewModel.descripcion, tipoComprobanteViewModel.estado);
                }
                else
                {
                    resultado = tipoComprobanteNEG.guardarTipoComprobante(tipoComprobanteViewModel.tipoComprobante,tipoComprobanteViewModel.descripcion, tipoComprobanteViewModel.estado);
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
        #region Listar Tipo de Comprobante
        public ActionResult ViewAllTipoComprobante()
        {
            return View();
        }
        public JsonResult listarTipoComprobante()
        {
            try
            {
                List<CO_TipoComprobante> resultado = tipoComprobanteNEG.listarTipoComprobante();

                List<TipoComprobanteViewModels> lista = TipoComprobanteViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        #region Eliminar Tipo de Comprobante
        public JsonResult eliminarTipoComprobante(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = tipoComprobanteNEG.eliminarTipoComprobante(id);

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