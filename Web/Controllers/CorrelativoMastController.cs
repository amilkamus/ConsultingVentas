using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.CorrelativoMaster;
using Web.Utilitario;
using Datos;
using Negocio;
using Web.Models;

namespace Web.Controllers
{
    public class CorrelativoMastController : Controller
    {
        CO_CorrelativoMastNEG correlativoMasterNEG = new CO_CorrelativoMastNEG();
        CO_TipoComprobanteNEG tipoComprobanteNEG = new CO_TipoComprobanteNEG();

        // GET: CorrelativoMast
        [Authorize(Roles = "ADMINISTRADOR")]
        #region Agregar Correlativo
        public ActionResult AddCorrelativo()
        {
            return View();
        }

        public JsonResult guardarCorrelativoMast(CorrelativoMastViewModels correlativoViewModels)
        {
            try
            {
                OperationResult resultado = null;
                if (correlativoViewModels.idCorrelativo != 0)
                {
                    resultado = correlativoMasterNEG.actualizarCorrelativoMast(correlativoViewModels.idCorrelativo, correlativoViewModels.idTipoComprobante, correlativoViewModels.serie, correlativoViewModels.correlativo);
                }
                else
                {
                    resultado = correlativoMasterNEG.guardarCorrelativoMast(correlativoViewModels.idTipoComprobante, correlativoViewModels.serie, correlativoViewModels.correlativo);
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
        #region Listar Correlativos
        public ActionResult ViewAllCorrelativoMast()
        {
            return View();
        }

        public JsonResult listarCorrelativoMast()
        {
            try
            {
                List<CorrelativoMast> resultado = correlativoMasterNEG.listarCorrelativoMast();

                List<CorrelativoMastViewModels> lista = CorrelativoMastViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        #region Eliminar Correlativos
        public JsonResult eliminarCorrelativoMast(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = correlativoMasterNEG.eliminarCorrelativoMast(id);

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
        public JsonResult comboTipoComprobante()
        {
            try
            {
                List<CO_TipoComprobante> resultado = tipoComprobanteNEG.listarTipoComprobante();

                List<ComboGenericoViewModel> lista = new List<ComboGenericoViewModel>();
                foreach (var item in resultado)
                {
                    ComboGenericoViewModel combo = new ComboGenericoViewModel(item.idTipoComprobante, item.tipoComprobante);
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