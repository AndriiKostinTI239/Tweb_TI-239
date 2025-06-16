// FRM/Controllers/AccountController.cs
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using FRM.Core.DTOs;
using FRM.Core.Interfaces.Repositories; // <-- ДОБАВЬТЕ ЭТОТ USING
using FRM.Core.Interfaces.Services;

namespace FRM.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepo; // <-- 1. ДОБАВЛЕНО ПОЛЕ

        // 2. КОНСТРУКТОР ОБНОВЛЕН
        public AccountController(IAuthService authService, IUserRepository userRepo)
        {
            _authService = authService;
            _userRepo = userRepo; // <-- 3. ДОБАВЛЕНО ПРИСВОЕНИЕ
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

            // Теперь этот код будет работать, т.к. _userRepo существует
            var user = await _userRepo.GetByEmailAsync(signInDto.Email);
            if (user != null && user.IsBanned)
            {
                ModelState.AddModelError("", "Ваш аккаунт заблокирован.");
            }
            else
            {
                ModelState.AddModelError("", "Неверный email или пароль.");
            }

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
                ModelState.AddModelError("", "Пользователь с таким email уже существует.");
                return View(dto);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("SignIn", "Account");
        }
    }
}