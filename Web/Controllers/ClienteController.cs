using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.PersonaMaster;
using Web.Models.Clientes;
using Web.Utilitario;
using Datos;
using Negocio;
using Web.Models;

namespace Web.Controllers
{
    public class ClienteController : Controller
    {
        ClienteNEG clienteNEG = new ClienteNEG();
        TipoPersonaNEG tipoPersonaNEG = new TipoPersonaNEG();
        TipoDocumentoNEG tipoDocumentoNEG = new TipoDocumentoNEG();
        PersonaMastNEG personaMastNEG = new PersonaMastNEG();

        // GET: Cliente
        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        #region Agregar Cliente
        public ActionResult AddCliente()
        {
            return View();
        }

        //public JsonResult guardarCliente(ClienteViewModel clienteViewModel, PersonaMastViewModel personaEmpresaViewModel, PersonaMastViewModel personaTitularViewModel, PersonaMastViewModel personaContactoViewModel)
        public JsonResult guardarCliente(ClienteViewModel clienteViewModel)
        {
            long IdEmpresa = 0;
            long IdTitular = 0;
            long IdContacto = 0;
            try
            {
                OperationResult resultado = null;
                if (clienteViewModel.idCliente != 0)
                {
                    resultado = personaMastNEG.actualizarPersonaMast(clienteViewModel.idEmpresa, clienteViewModel.empresaNombre, null, clienteViewModel.idTipoPersona, clienteViewModel.empresaIdTipoDocumento, clienteViewModel.empresaNumeroDocumento, clienteViewModel.empresaRubro, clienteViewModel.empresaCorreo, null, clienteViewModel.empresaDomicilio, null);
                    IdEmpresa = resultado.data;
                    resultado = personaMastNEG.actualizarPersonaMast(clienteViewModel.idTitular, clienteViewModel.titularNombre, clienteViewModel.titularApellidos, clienteViewModel.idTipoPersona, clienteViewModel.titularIdTipoDocumento, clienteViewModel.titularNumeroDocumento, null, clienteViewModel.titularCorreo, clienteViewModel.titularTelefono, clienteViewModel.titularDomicilio, null);
                    IdTitular = resultado.data;
                    resultado = personaMastNEG.actualizarPersonaMast(clienteViewModel.idContacto, clienteViewModel.contactoNombre, clienteViewModel.contactoApellidos, clienteViewModel.idTipoPersona, 2, null, clienteViewModel.contactoCargo, clienteViewModel.contactoCorreo, clienteViewModel.contactoTelefono, null, null);
                    IdContacto = resultado.data;
                    resultado = clienteNEG.actualizarCliente(clienteViewModel.idCliente, Convert.ToInt32(IdEmpresa), Convert.ToInt32(IdTitular), clienteViewModel.rubro, Convert.ToInt32(IdContacto));
                }
                else
                {
                    if (clienteViewModel.empresaNombre != null && clienteViewModel.empresaDomicilio != null)
                    {
                        resultado = personaMastNEG.guardarPersonaMast(clienteViewModel.empresaNombre, null, clienteViewModel.idTipoPersona, clienteViewModel.empresaIdTipoDocumento, clienteViewModel.empresaNumeroDocumento, clienteViewModel.empresaRubro, clienteViewModel.empresaCorreo, null, clienteViewModel.empresaDomicilio, null);
                        IdEmpresa = resultado.data;
                    }
                    if (clienteViewModel.titularNombre != null && clienteViewModel.titularApellidos != null)
                    {
                        resultado = personaMastNEG.guardarPersonaMast(clienteViewModel.titularNombre, clienteViewModel.titularApellidos, clienteViewModel.idTipoPersona, clienteViewModel.titularIdTipoDocumento, clienteViewModel.titularNumeroDocumento, null, clienteViewModel.titularCorreo, clienteViewModel.titularTelefono, clienteViewModel.titularDomicilio, null);
                        IdTitular = resultado.data;
                    }
                    if (clienteViewModel.contactoNombre != null && clienteViewModel.contactoApellidos != null)
                    {
                        resultado = personaMastNEG.guardarPersonaMast(clienteViewModel.contactoNombre, clienteViewModel.contactoApellidos, clienteViewModel.idTipoPersona, 2, null, clienteViewModel.contactoCargo, clienteViewModel.contactoCorreo, clienteViewModel.contactoTelefono, null, null);
                        IdContacto = resultado.data;
                    }                    
                    resultado = clienteNEG.guardarCliente(Convert.ToInt32(IdEmpresa == 0 ? IdTitular : IdEmpresa), Convert.ToInt32(IdTitular), clienteViewModel.titularRubro == null ? clienteViewModel.empresaRubro : clienteViewModel.titularRubro, Convert.ToInt32(IdContacto == 0 ? IdTitular : IdContacto));
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

        [Authorize(Roles = "ADMINISTRADOR, CONSULTOR, OPERADOR")]
        #region Listar Cliente
        public ActionResult ViewAllCliente()
        {
            return View();
        }

        [Authorize(Roles = "ADMINISTRADOR, CONSULTOR, OPERADOR")]
        public JsonResult listarCliente()
        {
            try
            {
                List<Cliente> resultado = clienteNEG.listarCliente();
                List<MostrarClienteViewModel> lista = MostrarClienteViewModel.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        #region Eliminar Tipo de Cliente
        public JsonResult eliminarCliente(int id)
        {
            try
            {
                OperationResult resultado;
                resultado = clienteNEG.eliminarCliente(id);

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
        public JsonResult comboTipoDocumento()
        {
            try
            {
                List<TipoDocumento> resultado = tipoDocumentoNEG.listarDocumento();

                List<ComboGenericoViewModel> lista = new List<ComboGenericoViewModel>();
                foreach (var item in resultado)
                {
                    ComboGenericoViewModel combo = new ComboGenericoViewModel(item.idTipoDocumento, item.tipoDocumento1);
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

        public JsonResult comboTipoPersona()
        {
            try
            {
                List<TipoPersona> resultado = tipoPersonaNEG.listarTipoPersona();

                List<ComboGenericoViewModel> lista = new List<ComboGenericoViewModel>();
                foreach (var item in resultado)
                {
                    ComboGenericoViewModel combo = new ComboGenericoViewModel(item.idTipoPersona, item.tipoPersona1);
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