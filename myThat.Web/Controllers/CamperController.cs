using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ServiceStack;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;

namespace myThat.Controllers
{
    public class CamperController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Edit()
        {
            var key = SessionFeature.GetSessionKey() ?? "";
            var sess = MvcApplication.myThatAppHost.Resolve<ICacheClient>().Get<AuthUserSession>(key);
            dynamic model = new ExpandoObject();
            model.Email = sess.Email;
            return View(model);
        }

        public ActionResult SigningIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string userName, string password)
        {
            try
            {
                var apiAuthService = AppHostBase.Resolve<AuthService>();
                apiAuthService.RequestContext = System.Web.HttpContext.Current.ToRequestContext();
                var apiResponse = apiAuthService.Authenticate(new Auth
                {
                    UserName = userName,
                    Password = password,
                    RememberMe = false
                });

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ModelState.AddModelError("AuthenticationError", "The user name or password provided is incorrect.");
                return View();
            }
        }

        public ActionResult LogOut()
        {
            //api logout
            var apiAuthService = AppHostBase.Resolve<AuthService>();
            apiAuthService.RequestContext = System.Web.HttpContext.Current.ToRequestContext();
            apiAuthService.Post(new Auth() { provider = "logout" });
            //forms logout
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
