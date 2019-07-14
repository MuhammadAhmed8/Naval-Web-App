using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApplication6.Infrastructure;
using WebApplication6.Models;
using WebApplication6.ViewModels;
using System.Text;

namespace WebApplication6.Controllers
{
    public class AccountController : Controller
    {
        private IUserManager UserManager;
        public AccountController(IUserManager UserManager)
        {
            this.UserManager = UserManager;
            
        }

       
        public IActionResult Login(string returnUrl = "/")
        {
            if (HttpContext.Session.GetString("uid_a") != null && HttpContext.Session.GetString("mhk") == "admin")
            {
                return Redirect("/Admin/Index");
            }
            string sel = Request.Cookies["ils"];
            string val = Request.Cookies["ajv"];
            if ( sel != null &&  val != null)
            {
               int? userId =  UserManager.VerifyAuthCredentials(sel, val);
               if(userId != null)
                {
                    HttpContext.Session.SetString("uid_a", userId.ToString());
                    HttpContext.Session.SetString("mhk","admin");
                    string token = UserManager.GenerateToken();
                    UserManager.SetAuthCredentials(sel, token, (int)userId,DateTime.Now.AddMinutes(14400));

                    CookieExtensions.SetCookie("ajv", token, 14400, Response, true, true);
                    return Redirect("/Admin/Index");

                }
            }
          
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel LoginModel)
        {
            
            if(HttpContext.Session.GetString("uid_a") != null && HttpContext.Session.GetString("mhk") =="admin")
            { 
                return Redirect("/Admin/Index");
            }
            if (ModelState.IsValid)
            {

                User user = UserManager.FindUser(LoginModel.Username, LoginModel.Password);
                if ( user != null)
                {
                    foreach(var i in user.Roles)
                    {
                        if(i != null && i.Name == "Admin")
                        {
                            HttpContext.Session.SetString("mhk", "admin");
                            break;

                        }
                    }
                    HttpContext.Session.SetString("uid_a", user.UserId.ToString());
                    user.isLoggedIn = true;
                    if (LoginModel.Rememberme)
                    {
                        string selector = UserManager.GenerateSelector();
                        string token = UserManager.GenerateToken();
                        UserManager.SetAuthCredentials(selector, token, (int)user.UserId,DateTime.Now.AddMinutes(14400));

                        CookieExtensions.SetCookie("ils",selector, 14400, Response,true,true);
                        CookieExtensions.SetCookie("ajv", token, 14400, Response, true, true);

                    }
                    return Redirect(LoginModel?.ReturnUrl ?? "/Admin/Index");
                }
               
                TempData["message"] = "Oops! Looks like you typed it wrong.";
           
            }
            return View(LoginModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            try
            {
                if(HttpContext.Session.Get("uid_a") != null)
                    HttpContext.Session.Remove("uid_a");
                    HttpContext.Session.Remove("mhk");
                CookieExtensions.Remove("ils", Response);
                CookieExtensions.Remove("ajv", Response);

            }
            catch (Exception)
            {
                
            }
            return RedirectToAction("Login");


        }



        public ContentResult Hash_PBKDF2(string PlainPassword)
        {
            
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(PlainPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return Content(savedPasswordHash);
        }






    }
}