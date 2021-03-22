using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Web.Models;
using System.Collections.Generic;
using Web.Utilitario;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {        
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        #region Listar Usuarios
        [Authorize(Roles = "ADMINISTRADOR")]
        public ActionResult ViewUser()
        {
            return View();
        }

        public JsonResult ListarUsuarios()
        {
            try
            {
                List<UserToRolViewModel> lista = new List<UserToRolViewModel>();
                var result = UserManager.Users.ToList();
                var resultRol = RoleManager.Roles.ToList();
                

                foreach (var item in result)
                {
                    UserToRolViewModel objUserDTO = new UserToRolViewModel();

                    objUserDTO.UserId = item.Id;
                    objUserDTO.FirstName = item.FirstName;
                    objUserDTO.LastName = item.LastName;
                    objUserDTO.UserName = item.UserName;
                    objUserDTO.EmailConfirmed = item.EmailConfirmed;
                    objUserDTO.Email = item.Email;
                    //objUserDTO.LockoutEndDateUtc = item.LockoutEndDateUtc;
                    objUserDTO.EsUsuarioPrincipal = item.UserName.ToLower() != this.User.Identity.Name.ToLower() ? true : false;
                    objUserDTO.Role = UserManager.GetRoles(item.Id).Single();

                    var resultUser = UserManager.FindById(objUserDTO.UserId);

                    var roleName = UserManager.GetRoles(resultUser.Id).Single();
                    objUserDTO.ApplicationRoleId = RoleManager.Roles.Single(r => r.Name == roleName).Id;

                    lista.Add(objUserDTO);
                }

                return Json(lista);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<UserToRolViewModel> col_UserDTO = new List<UserToRolViewModel>();

                return Json(col_UserDTO);
            }
        }
        #endregion

        #region Login
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Requiere que el usuario tenga un correo electrónico confirmado antes de poder iniciar sesión.
            // var user = await UserManager.FindByNameAsync(model.Email);
            var user = UserManager.Find(model.Email, model.Password);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account-Resend");

                    // Descomentar para depurar localmente  
                    // ViewBag.Link = callbackUrl;

                    ViewBag.errorMessage = "Debe tener un correo electrónico confirmado para iniciar sesión.";
                    return View("Error");
                }
            }

            // Esto no cuenta los errores de inicio de sesión para el bloqueo de cuentas
            // Para habilitar las fallas de contraseña para activar el bloqueo de la cuenta, cambie a shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        Session["IdUser"] = user.Id;
                        Session["NameUser"] = string.Format("{0} {1}", user.FirstName, user.LastName);
                        return RedirectToLocal(returnUrl == null ? "~/Home/Index" : returnUrl);
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Intento de inicio de sesión inválido");
                    return View(model);
            }
        }
        #endregion

        #region Código de verificación
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        #endregion

        #region Crear Usuario
        // GET: /Account/Register
        //[Authorize(Roles = "ADMINISTRADOR")]
        public ActionResult Register()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var role in RoleManager.Roles.ToList())
            {
                list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
            }
            ViewBag.Roles = list;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { FirstName = model.FirstName, LastName = model.LastName, UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // La siguiente línea PERMITE el inicio de sesión del usuario aun no sea confirmado el correo
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    result = await UserManager.AddToRoleAsync(user.Id, model.RoleName);

                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirme su cuenta - ROUILLON CONSULTING");

                    // Uncomment to debug locally 
                    // TempData["ViewBagLink"] = callbackUrl;

                    //ViewBag.Message = "Verifique su correo electrónico y confirme su cuenta, debe estar confirmado " + "antes de que puedas iniciar sesión.";

                    //return View("Info");
                    return RedirectToAction("Register", "Account");
                }
                AddErrors(result);
            }

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var role in RoleManager.Roles.ToList())
            {
                list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
            }
            ViewBag.Roles = list;
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region Editar Usuario
        public ActionResult EditUser(string UserId)
        {
            if (UserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserToRolViewModel objExpandedUserDTO = GetUser(UserId);

            if (objExpandedUserDTO == null)
            {
                return HttpNotFound();
            }
            return View(objExpandedUserDTO);
            //return View();
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(UserToRolViewModel paramExpandedUserDTO)
        {
            try
            {
                if (paramExpandedUserDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserToRolViewModel objExpandedUserDTO = UpdateDTOUser(paramExpandedUserDTO);

                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }
                return RedirectToAction("ViewUser");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(paramExpandedUserDTO.UserName));
            }
        }
        #endregion

        #region Eliminar Usuario
        [Authorize(Roles = "ADMINISTRADOR")]
        public JsonResult DeleteUser(string UserId)
        {
            try
            {
                ApplicationUser user = UserManager.FindById(UserId);

                if (user.Email.ToLower() == this.User.Identity.Name.ToLower())
                {
                    throw new Exception("Error: No se puede eliminar el usuario actual");
                }

                UserToRolViewModel objExpandedUserDTO = GetUser(UserId);

                if (objExpandedUserDTO == null)
                {
                    throw new Exception("Usuario no encontrado");
                }
                else
                {
                    DeleteUser(objExpandedUserDTO);
                }

                return Json(Util.successJson("Se elimino el usuario"));
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(ex));
            }
        }
        #endregion

        #region Confirmar correo electrónico
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        #endregion

        #region Recuperación / restablecimiento de contraseña
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Envíe un correo electrónico con este enlace:
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Hola!, pediste resetear tu password. <br/><br/> Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "<b>Hola "+ user.FirstName +"!</b><br/><br/> Recibimos una solicitud para resetear el password de tu cuenta en QUY. <br/> Puedes resetear tu password haciendo clic <b><a href=\"" + callbackUrl + "\">aquí</a></b>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");

                //return RedirectToAction("ForgotPassword", "Account");
            }

            // Si llegamos tan lejos, algo falla, volver a mostrar la forma.
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region Restablecer la contraseña
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region Error de inicio de sesión externo / Confirmación de inicio de sesión externo  / Devolución de llamada externa
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        #endregion

        #region Desconectarse
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Enviar confirmación de correo electrónico Token asíncrono
        // Reenviar el enlace de confirmación de correo electrónico 
        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject, "Por favor confirme su cuenta haciendo clic <a href=\"" + callbackUrl + "\">here</a>");

            return callbackUrl;
        }
        #endregion

        #region GetAllRolesAsSelectList
        public JsonResult GetAllRolesAsSelectList()
        {
            List<SelectListItem> SelectRoleListItems = new List<SelectListItem>();

            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var colRoleSelectList = roleManager.Roles.OrderBy(x => x.Name).ToList();

            foreach (var item in colRoleSelectList)
            {
                SelectRoleListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Name.ToString(),
                        Value = item.Name.ToString(),
                    });
            }

            return Json(SelectRoleListItems);
        }
        #endregion

        #region GetUser
        private UserToRolViewModel GetUser(string paramUserId)
        {
            UserToRolViewModel objExpandedUserDTO = new  UserToRolViewModel();

            objExpandedUserDTO.ApplicationRoles = RoleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            ApplicationUser result = UserManager.FindById(paramUserId);

            // Si no pudimos encontrar al usuario, lanza una excepción
            if (result == null) throw new Exception("No se pudo encontrar el usuario " + result.UserName);

            objExpandedUserDTO.FirstName = result.FirstName;
            objExpandedUserDTO.LastName = result.LastName;
            objExpandedUserDTO.UserName = result.UserName;
            objExpandedUserDTO.Email = result.Email;
            objExpandedUserDTO.PhoneNumber = result.PhoneNumber;

            var roleName = UserManager.GetRoles(result.Id).Single();

            objExpandedUserDTO.ApplicationRoleId = RoleManager.Roles.Single(r => r.Name == roleName).Id;

            return objExpandedUserDTO;
        }
        #endregion

        #region UpdateDTOUser
        private UserToRolViewModel UpdateDTOUser(UserToRolViewModel paramExpandedUserDTO)
        {
            ApplicationUser result = UserManager.FindById(paramExpandedUserDTO.UserId);

            string existingRole = UserManager.GetRolesAsync(result.Id).Result.Single();
            string existingRoleId = RoleManager.Roles.Single(r => r.Name == existingRole).Id;

            // If we could not find the user, throw an exception
            if (result == null)
            {
                throw new Exception("No se pudo encontrar el usuario " + result.UserName);
            }

            result.FirstName = paramExpandedUserDTO.FirstName;
            result.LastName = paramExpandedUserDTO.LastName;
            result.Email = paramExpandedUserDTO.Email;
            result.UserName = paramExpandedUserDTO.Email;
            result.PhoneNumber = paramExpandedUserDTO.PhoneNumber;

            // Comprobemos si la cuenta debe ser desbloqueada
            if (UserManager.IsLockedOut(result.Id))
            {
                // Desbloquear usuario
                UserManager.ResetAccessFailedCountAsync(result.Id);
            }

            IdentityResult resultUser = UserManager.Update(result);
            if (resultUser.Succeeded)
            {
                // ¿Se envió una contraseña?
                if (!string.IsNullOrEmpty(paramExpandedUserDTO.Password))
                {
                    // Eliminar la contraseña actual
                    var removePassword = UserManager.RemovePassword(result.Id);
                    if (removePassword.Succeeded)
                    {
                        //Agregar nueva contraseña
                        var AddPassword =
                            UserManager.AddPassword(
                                result.Id,
                                paramExpandedUserDTO.Password
                                );

                        if (AddPassword.Errors.Count() > 0)
                        {
                            throw new Exception(AddPassword.Errors.FirstOrDefault());
                        }
                    }
                }
            }

            //codigo agregado
            if (existingRoleId != paramExpandedUserDTO.ApplicationRoleId)
            {
                IdentityResult roleResult = UserManager.RemoveFromRole(result.Id, existingRole);
                if (roleResult.Succeeded)
                {
                    ApplicationRole applicationRole = RoleManager.FindById(paramExpandedUserDTO.ApplicationRoleId);
                    if (applicationRole != null)
                    {
                        //Agregando Usuario a Rol
                        UserManager.AddToRole(result.Id, applicationRole.Name);
                    }
                }
            }

            paramExpandedUserDTO.UserId = result.Id;

            return paramExpandedUserDTO;
        }
        #endregion

        #region DeleteUser
        private void DeleteUser(UserToRolViewModel paramExpandedUserDTO)
        {
            ApplicationUser user = UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }

            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            UserManager.Update(user);
            UserManager.Delete(user);
        }
        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
        
    }
}