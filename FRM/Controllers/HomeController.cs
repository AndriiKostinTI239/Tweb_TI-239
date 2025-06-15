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
    public class HomeController : BaseController
    {
        // --- ДОБАВЬТЕ ЭТИ ПОЛЯ И КОНСТРУКТОР ---
        private readonly IThreadService _threadService;

        public HomeController() : base()
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
       
    }
}