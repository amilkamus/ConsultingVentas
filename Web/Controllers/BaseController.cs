using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        #region Retornar el ID del empleado logeado
        public string IdUsuario()
        {
            string idUsuario = "";
            idUsuario = User.Identity.GetUserId();
            if (idUsuario == "")
            {
                throw new Exception("No existe un empleado para la cuenta de usuario, por lo tanto no se puede realizar la operación");
            }
            return idUsuario;
        }
        public string NombreUsuario(string id)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (userManager == null)
                throw new Exception("No existe un empleado para la cuenta de usuario, por lo tanto no se puede realizar la operación");

            var usuario = userManager.Users.Where(u => u.Id == id).FirstOrDefault();
            if (usuario == null)
                throw new Exception("No existe un empleado para la cuenta de usuario, por lo tanto no se puede realizar la operación");

            return string.Format("{0} {1}", usuario.FirstName, usuario.LastName); ;
        }
        #endregion
    }
}