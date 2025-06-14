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
using System.Web;
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
            var threadRepo = new ThreadRepository(context);
            _profileService = new ProfileService(userRepo, threadRepo, hasher);
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
        // Добавляем параметр HttpPostedFileBase profilePicture
        public async Task<ActionResult> Edit(ProfileEditDto dto, HttpPostedFileBase profilePicture)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var userId = GetCurrentUserId();

            // Обновляем основную информацию
            var profileUpdateSuccess = await _profileService.UpdateUserProfileAsync(userId, dto);
            if (!profileUpdateSuccess)
            {
                ModelState.AddModelError("", "Пользователь с таким Email уже существует.");
                return View(dto);
            }

            // Если был загружен файл, обновляем аватар
            if (profilePicture != null && profilePicture.ContentLength > 0)
            {
                await _profileService.UpdateProfilePictureAsync(userId, profilePicture);
            }

            TempData["SuccessMessage"] = "Профиль успешно обновлен!";
            return RedirectToAction("Index");
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
            // Пытаемся получить Identity как FormsIdentity
            var formsIdentity = User.Identity as FormsIdentity;
            if (formsIdentity == null)
            {
                // Если не получилось, значит пользователь не аутентифицирован через Forms
                return Guid.Empty;
            }

            // Получаем билет аутентификации
            var ticket = formsIdentity.Ticket;
            if (ticket == null)
            {
                return Guid.Empty;
            }

            // Извлекаем UserData, которую мы записали при входе
            var userData = ticket.UserData;
            if (string.IsNullOrEmpty(userData))
            {
                return Guid.Empty;
            }

            // Разделяем строку, чтобы получить ID
            var userDataParts = userData.Split('|');
            if (userDataParts.Length > 0 && Guid.TryParse(userDataParts[0], out Guid userId))
            {
                return userId;
            }

            // Если не удалось распарсить Guid, возвращаем пустой Guid
            return Guid.Empty;
        }
    }
}