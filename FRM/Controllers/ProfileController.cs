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
    public class ProfileController : BaseController
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
        public async Task<ActionResult> Index(Guid? id)
        {
            // Если ID не передан, показываем профиль текущего пользователя
            // Если ID передан, показываем профиль этого пользователя
            Guid userIdToShow = id ?? GetCurrentUserId();

            if (userIdToShow == Guid.Empty)
            {
                // Если ID не передан и пользователь не залогинен, отправляем на страницу входа
                return RedirectToAction("SignIn", "Account");
            }

            var profile = await _profileService.GetUserProfileAsync(userIdToShow);

            if (profile == null)
            {
                return HttpNotFound();
            }

            // Передаем в ViewBag, чтобы знать, чей профиль мы смотрим (свой или чужой)
            ViewBag.IsMyProfile = (userIdToShow == GetCurrentUserId());

            return View(profile);
        }
        // Показывает форму для редактирования профиля
        [HttpGet]
        public async Task<ActionResult> Edit() // Больше не принимает ID
        {
            // Всегда получаем ID ТОЛЬКО текущего залогиненного пользователя
            var currentUserId = GetCurrentUserId();
            if (currentUserId == Guid.Empty)
            {
                return new HttpUnauthorizedResult(); // Если как-то попал сюда без логина
            }

            var model = await _profileService.GetUserProfileForEditAsync(currentUserId);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }
        // Обрабатывает отправку формы редактирования
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Добавляем параметр HttpPostedFileBase profilePicture
        public async Task<ActionResult> Edit(ProfileEditDto dto, HttpPostedFileBase profilePicture)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == Guid.Empty)
            {
                return new HttpUnauthorizedResult();
            }

            // Передаем ID в сервис для обновления
            // Сервис тоже может содержать проверку, но лучше делать это на уровне контроллера
            var success = await _profileService.UpdateUserProfileAsync(currentUserId, dto);

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
        public ActionResult ChangePassword() // Убедись, что он не принимает ID
        {
            return View();
        }

        // --- POST-метод ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            // Получаем ID ТОЛЬКО текущего пользователя
            var currentUserId = GetCurrentUserId();
            if (currentUserId == Guid.Empty)
            {
                return new HttpUnauthorizedResult();
            }

            var success = await _profileService.ChangePasswordAsync(currentUserId, dto);

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