using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Documento;
using Web.Utilitario;
using Datos;
using Negocio;

namespace Web.Controllers
{
    public class DocumentoController : Controller
    {
        TipoDocumentoNEG tipoDocumentoNEG = new TipoDocumentoNEG();

        // GET: Documento
        [Authorize(Roles = "ADMINISTRADOR")]
        #region Agregar Tipo de Documento
        public ActionResult AddDocumento()
        {
            return View();
        }

        public JsonResult guardarDocumento(DocumentoViewModels documentoViewModel)
        {
            try
            {
                OperationResult resultado = null;
                if (documentoViewModel.idTipoDocumento != 0)
                {
                    resultado = tipoDocumentoNEG.actualizarDocumento(documentoViewModel.idTipoDocumento,documentoViewModel.nombre, documentoViewModel.estado);
                }
                else
                {
                    resultado = tipoDocumentoNEG.guardarDocumento(documentoViewModel.nombre, documentoViewModel.estado);
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
        #region Listar Tipo de Documentos
        public ActionResult ViewAllDocumento()
        {
            return View();
        }
        public JsonResult listarDocumento()
        {
            try
            {
                List<TipoDocumento> resultado = tipoDocumentoNEG.listarDocumento();

                List<DocumentoViewModels> lista = DocumentoViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        #region Eliminar Tipo de Documentos
        public JsonResult eliminarDocumento(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = tipoDocumentoNEG.eliminarDocumento(id);

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