using Datos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.Utilitario;

namespace Web.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationRoleManager _roleManager;
        
        public RoleController(){ }

        public RoleController(ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }
        
        // GET: Role
        #region Listar
        [Authorize(Roles = "ADMINISTRADOR")]
        public ActionResult Index()
        {
            //List<RoleViewModel> list = new List<RoleViewModel>();
            //foreach (var role in RoleManager.Roles)
            //{
            //    list.Add(new RoleViewModel(role));
            //}
            //return View(list);
            return View();
        }
        public JsonResult ListarRoles()
        {
            try
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                List<RoleViewModel> colRoleDTO = (from objRole in roleManager.Roles
                                                  select new RoleViewModel
                                                  {
                                                      Id = objRole.Id,
                                                      Name = objRole.Name
                                                  }).ToList();
                return Json(colRoleDTO);
            }
            catch (Exception)
            {
                List<RoleViewModel> colRoleDTO = new List<RoleViewModel>();

                return Json(colRoleDTO);
            }
        }
        #endregion

        #region Crear
        [Authorize(Roles = "ADMINISTRADOR")]
        public ActionResult Create()
        {
            return View();
        }
                
        public JsonResult Guardar(RoleViewModel modelRole)
        {
            try
            {
                if (modelRole.Id == null)
                {
                    // Create Role
                    var roleManager = new ApplicationRole() { Name = modelRole.Name };
                    RoleManager.Create(roleManager);
                }
                else
                {
                    var role = new ApplicationRole() { Id = modelRole.Id, Name = modelRole.Name };
                    RoleManager.Update(role);
                }
                return Json(Util.successJson("Se registro con exito el Rol: " + modelRole.Name));
            }

            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion
        
        #region Eliminar
        [Authorize(Roles = "ADMINISTRADOR")]
        public JsonResult Delete(string Id)
        {
            try
            {
                var UsersInRole = RoleManager.FindById(Id).Users.Count();
                if (UsersInRole > 0)
                {
                    return Json(Util.warningJson("No se puede borrar el ROL porque tiene usuario asignado."));
                }

                var objRoleToDelete = (from objRole in RoleManager.Roles
                                       where objRole.Id == Id
                                       select objRole).FirstOrDefault();

                if (objRoleToDelete != null)
                {
                    RoleManager.Delete(objRoleToDelete);
                }
                return Json(Util.successJson("Se Elimino correctamente el rol."));
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(ex));
            }
        }
        #endregion
    }
}