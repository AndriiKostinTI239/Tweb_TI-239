using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FRM.BuisnessLogic.Helper;
using FRM.BuisnessLogic.Services;
using FRM.Core.DTOs;
using FRM.Core.Interfaces.Repositories;
using FRM.Core.Interfaces.Services;
using FRM.Domain;
using FRM.Domain.Repositories;

namespace FRM.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

      
            public AccountController()
            {
                // Создаем зависимости
                var context = new AppDbContext();
                var userRepo = new UserRepository(context);
                var hasher = new Hasher();

                _authService = new AuthService(userRepo, hasher);
            }

            [HttpGet]
        public ActionResult SignIn() => View();
        
        [HttpGet]
        public ActionResult SignUp() => View();
        
        
        
        [HttpPost]
        public async Task<ActionResult> SignIn(SignInDto signInDto)
        {
            if (!ModelState.IsValid)
                return View(signInDto);

            bool isAuthenticated = await _authService.SignInAsync(signInDto);
            
    
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неверный email или пароль");
            return View(signInDto);
        }

        

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _authService.SignUpAsync(dto);
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
            
            
        }
        
        public ActionResult Logout()
        {
            // Удаляем куки аутентификации
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(cookie);
            }
    
            // Очищаем сессию
            Session.Clear();
    
            // Перенаправляем на страницу входа
            return RedirectToAction("SignIn", "Account");
        }
    }
}