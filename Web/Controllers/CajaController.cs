using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.CierreCaja;
using Web.Utilitario;
using Negocio;
using Datos;

namespace Web.Controllers
{
    public class CajaController : BaseController
    {
        CajaNEG cajaNEG = new CajaNEG();

        // GET: Caja
        [Authorize(Roles = "ADMINISTRADOR")]
        #region Agregar Cierre de Caja
        public ActionResult AddCaja()
        {
            return View();
        }

        public JsonResult guardarCaja(CajaViewModels cajaViewModels)
        {
            try
            {
                OperationResult resultado = null;
                if (cajaViewModels.idCaja != 0)
                {
                    resultado = cajaNEG.actualizarCaja(cajaViewModels.idCaja, IdUsuario(), cajaViewModels.ingresos, cajaViewModels.saldoInicial, cajaViewModels.saldoFinal, IdUsuario(), cajaViewModels.estado);
                }
                else
                {                    
                    resultado = cajaNEG.guardarCaja(IdUsuario(), cajaViewModels.ingresos, cajaViewModels.saldoInicial,cajaViewModels.saldoFinal, IdUsuario(), cajaViewModels.estado);
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
        #region Listar Caja
        public ActionResult ViewAllCaja()
        {
            return View();
        }

        public JsonResult listarCaja()
        {
            try
            {
                List<Caja> resultado = cajaNEG.listarCaja();

                List<CajaViewModels> lista = CajaViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        #region Eliminar Caja
        public JsonResult eliminarCaja(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = cajaNEG.eliminarCaja(id);

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