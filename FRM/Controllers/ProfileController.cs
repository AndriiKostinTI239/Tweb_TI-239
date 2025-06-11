// FRM.Controllers/ProfileController.cs
using FRM.BuisnessLogic.Helper;
using FRM.BuisnessLogic.Services;
using FRM.Core.DTOs;
using FRM.Core.Interfaces.Services;
using FRM.Domain;
using FRM.Domain.Repositories;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security; // Для FormsAuthentication

namespace FRM.Controllers
{
    [Authorize] // Все действия в этом контроллере требуют авторизации
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        // ВАЖНО: Тебе нужно как-то получить ID текущего пользователя.
        // В твоем ThreadController используется ClaimsIdentity. Это правильно, если у тебя настроена
        // аутентификация на основе Claims. Однако SignInAsync использует FormsAuthentication.
        // Чтобы Claims работали с FormsAuthentication, нужно в Global.asax.cs добавить обработчик:
        // Application_PostAuthenticateRequest.
        // Я буду использовать твой метод GetCurrentUserId, предполагая, что он работает.

        public ProfileController()
        {
            // Ручное создание зависимостей, как в твоем ThreadController
            var context = new AppDbContext();
            var userRepo = new UserRepository(context);
            var hasher = new Hasher();
            _profileService = new ProfileService(userRepo, hasher);
        }

        // Показывает профиль текущего пользователя
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var profile = await _profileService.GetUserProfileAsync(userId);

            if (profile == null) return HttpNotFound();

            return View(profile); // Нужна View: ~/Views/Profile/Index.cshtml
        }

        // Показывает форму для редактирования профиля
        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var userId = GetCurrentUserId();
            var model = await _profileService.GetUserProfileForEditAsync(userId);
            if (model == null) return HttpNotFound();

            return View(model); // Нужна View: ~/Views/Profile/Edit.cshtml
        }

        // Обрабатывает отправку формы редактирования
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProfileEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var userId = GetCurrentUserId();
            var success = await _profileService.UpdateUserProfileAsync(userId, dto);

            if (success)
            {
                TempData["SuccessMessage"] = "Профиль успешно обновлен!";
                return RedirectToAction("Index");
            }

            // Если email уже занят
            ModelState.AddModelError("", "Пользователь с таким Email уже существует.");
            return View(dto);
        }

        // Показывает форму смены пароля
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(); // Нужна View: ~/Views/Profile/ChangePassword.cshtml
        }

        // Обрабатывает смену пароля
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var userId = GetCurrentUserId();
            var success = await _profileService.ChangePasswordAsync(userId, dto);

            if (success)
            {
                TempData["SuccessMessage"] = "Пароль успешно изменен!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("OldPassword", "Неверный текущий пароль.");
            return View(dto);
        }

        // Метод для получения ID текущего пользователя (такой же, как в ThreadController)
        private Guid GetCurrentUserId()
        {
            // Это будет работать, если в Global.asax ты парсишь UserData из cookie
            // и создаешь GenericPrincipal с ClaimsIdentity
            var identity = (FormsIdentity)User.Identity;
            var ticket = identity.Ticket;
            var userData = ticket.UserData.Split('|');

            return Guid.Parse(userData[0]);
        }
    }
}