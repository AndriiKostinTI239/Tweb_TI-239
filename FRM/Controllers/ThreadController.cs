// ThreadController.cs
using FRM.BuisnessLogic.Helper;
using FRM.BuisnessLogic.Services;
using FRM.Core.DTOs;
using FRM.Core.Interfaces.Services;
using FRM.Domain.Repositories;
using FRM.Domain;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace FRM.Controllers
{
    [Authorize]
    public class ThreadController : BaseController
    {
        private readonly IThreadService _threadService;
      

        public ThreadController() : base()
        {
            var context = new AppDbContext();

            var userRepo = new UserRepository(context);
            var threadRepo = new ThreadRepository(context);
            var commentRepo = new CommentRepository(context);

            _threadService = new ThreadService(threadRepo, commentRepo, userRepo);
        }
    
   


        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ViewBag.CurrentUserId = GetCurrentUserId();
            var threads = await _threadService.GetAllThreadsAsync();
            return View(threads);
        }

        [HttpGet]
        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateThreadDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                // Если по какой-то причине не удалось получить ID,
                // возвращаем пользователя на страницу входа
                return RedirectToAction("SignIn", "Account");
            }
            await _threadService.CreateThreadAsync(dto, userId);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> View(Guid id)
        {

            ViewBag.CurrentUserId = GetCurrentUserId();
            var thread = await _threadService.GetThreadByIdAsync(id);
            if (thread == null) return HttpNotFound();

            return View(thread);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Теперь метод принимает DTO, а не отдельные параметры
        public async Task<ActionResult> AddComment(AddCommentDto dto, Guid threadId)
        {
            if (!ModelState.IsValid)
            {
                // В случае ошибки лучше вернуть на ту же страницу с сообщением об ошибке
                // Это более сложная логика, пока просто редиректим
                return RedirectToAction("View", new { id = threadId });
            }

            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return new HttpUnauthorizedResult();

            await _threadService.AddCommentAsync(dto, threadId, userId);

            return RedirectToAction("View", new { id = threadId });
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteThread(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return new HttpUnauthorizedResult();

            var success = await _threadService.DeleteThreadAsync(id, userId);

            // Тут можно добавить сообщение в TempData, если `success` is false

            return RedirectToAction("Index"); // Перенаправляем на список тем
        }

        // --- ЭКШЕН ДЛЯ УДАЛЕНИЯ КОММЕНТАРИЯ ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> DeleteComment(Guid id, Guid threadId)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return new HttpUnauthorizedResult();

            await _threadService.DeleteCommentAsync(id, userId);

            return RedirectToAction("View", new { id = threadId }); // Возвращаемся на страницу треда
        }
    }
}