using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Producto;
using Web.Utilitario;
using Negocio;
using Datos;
using Web.Models;

namespace Web.Controllers
{
    public class ProductoController : BaseController
    {
        WH_ProductoServicioNEG productoServicioNEG = new WH_ProductoServicioNEG();
        WH_TipoProductoServicioNEG tipoProductoNEG = new WH_TipoProductoServicioNEG();

        // GET: Ventas/Producto
        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        #region Agregar Producto
        public ActionResult AddProducto()
        {
            return View();
        }

        public JsonResult guardarProducto(ProductoServicioViewModels productoViewModels)
        {
            try
            {
                OperationResult resultado = null;
                if (productoViewModels.idProducto != 0)
                {
                    resultado = productoServicioNEG.actualizarProducto(productoViewModels.idProducto, productoViewModels.idTipoProductoServicio, productoViewModels.codigo, productoViewModels.nombre, productoViewModels.descripcion, productoViewModels.stock, productoViewModels.estado, productoViewModels.costo, productoViewModels.precio,productoViewModels.fechaRegistro, productoViewModels.usuarioRegistro, IdUsuario());
                }
                else
                {
                    resultado = productoServicioNEG.guardarProducto(productoViewModels.idTipoProductoServicio, productoViewModels.codigo, productoViewModels.nombre, productoViewModels.descripcion, productoViewModels.stock, productoViewModels.estado, productoViewModels.costo, productoViewModels.precio, IdUsuario());
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

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        #region Listar Producto
        public ActionResult ViewAllProducto()
        {
            return View();
        }

        public JsonResult listarProducto()
        {
            try
            {
                List<WH_ProductoServicio> resultado = productoServicioNEG.listarProducto();

                List<MostrarProductoServicioViewModels> lista = MostrarProductoServicioViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        #region Eliminar Producto
        public JsonResult eliminarProducto(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = productoServicioNEG.eliminarProducto(id);

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
        public JsonResult comboTipoProducto()
        {
            try
            {
                List<WH_TipoProductoServicio> resultado = tipoProductoNEG.listarTipoProducto();

                List<ComboGenericoViewModel> lista = new List<ComboGenericoViewModel>();
                foreach (var item in resultado)
                {
                    ComboGenericoViewModel combo = new ComboGenericoViewModel(item.idTipoProductoServicio, item.tipoProductoServicio);
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