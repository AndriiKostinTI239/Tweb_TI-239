// FRM.Controllers/HomeController.cs
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using FRM.BuisnessLogic.Services;
using FRM.Core.Interfaces.Services;
using FRM.Domain;
using FRM.Domain.Repositories;

namespace FRM.Controllers
{
    public class HomeController : Controller
    {
        // --- ДОБАВЬТЕ ЭТИ ПОЛЯ И КОНСТРУКТОР ---
        private readonly IThreadService _threadService;

        public HomeController()
        {
            // Ручное создание зависимостей, как в других ваших контроллерах
            var context = new AppDbContext();
            var userRepo = new UserRepository(context);
            var threadRepo = new ThreadRepository(context);
            var commentRepo = new CommentRepository(context);

            _threadService = new ThreadService(threadRepo, commentRepo, userRepo);
        }
        // ------------------------------------------

        public async Task<ActionResult> Index()
        {
            // --- ЗАМЕНИТЕ СТАРОЕ СОДЕРЖИМОЕ МЕТОДА НА ЭТО ---

            // 1. Получаем ID текущего пользователя (если он залогинен)
            ViewBag.CurrentUserId = GetCurrentUserId();

            // 2. Получаем список всех тредов через сервис
            var threads = await _threadService.GetAllThreadsAsync();

            // 3. Передаем список тредов в представление
            return View(threads);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        // --- ДОБАВЬТЕ МЕТОД ДЛЯ ПОЛУЧЕНИЯ ID ПОЛЬЗОВАТЕЛЯ ---
        // (Скопируйте его из ThreadController или ProfileController)
        private Guid GetCurrentUserId()
        {
            var formsIdentity = User.Identity as FormsIdentity;
            if (formsIdentity == null || !formsIdentity.IsAuthenticated)
            {
                return Guid.Empty;
            }

            var ticket = formsIdentity.Ticket;
            if (ticket == null)
            {
                return Guid.Empty;
            }

            var userData = ticket.UserData;
            if (string.IsNullOrEmpty(userData))
            {
                return Guid.Empty;
            }

            var userDataParts = userData.Split('|');
            if (userDataParts.Length > 0 && Guid.TryParse(userDataParts[0], out Guid userId))
            {
                return userId;
            }

            return Guid.Empty;
        }
    }
}