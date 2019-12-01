using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.TipoProducto;
using Web.Utilitario;
using Negocio;
using Datos;

namespace Web.Controllers
{
    public class TipoProductoController : BaseController
    {
        WH_TipoProductoServicioNEG tipoProductoServicioNEG = new WH_TipoProductoServicioNEG();
        
        // GET: TipoProducto
        [Authorize(Roles = "ADMINISTRADOR")]
        #region Agregar Tipo de Producto
        public ActionResult AddTipoProducto()
        {
            return View();
        }

        public JsonResult guardarTipoProducto(TipoProductoServicioViewModels tipoProductoServicioViewModels)
        {
            try
            {
                OperationResult resultado = null;
                if (tipoProductoServicioViewModels.idTipoProductoServicio != 0)
                {
                    resultado = tipoProductoServicioNEG.actualizarTipoProducto(tipoProductoServicioViewModels.idTipoProductoServicio, tipoProductoServicioViewModels.nombre, tipoProductoServicioViewModels.descripcion, tipoProductoServicioViewModels.estado, IdUsuario());
                }
                else
                {
                    resultado = tipoProductoServicioNEG.guardarTipoProducto(tipoProductoServicioViewModels.nombre, tipoProductoServicioViewModels.descripcion, tipoProductoServicioViewModels.estado, IdUsuario());
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
        #region Listar Tipo de Producto
        public ActionResult ViewAllTipoProducto()
        {
            return View();
        }

        public JsonResult listarTipoProducto()
        {
            try
            {
                List<WH_TipoProductoServicio> resultado = tipoProductoServicioNEG.listarTipoProducto();

                List<TipoProductoServicioViewModels> lista = TipoProductoServicioViewModels.convert(resultado);
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
        public JsonResult eliminarTipoProducto(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = tipoProductoServicioNEG.eliminarTipoProducto(id);

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