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

namespace FRM.Controllers
{
    [Authorize]
    public class ThreadController : Controller
    {
        private readonly IThreadService _threadService;
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;

        public ThreadController()
        {
            _context = new AppDbContext();
            var threadRepo = new ThreadRepository(_context);
            var commentRepo = new CommentRepository(_context);
            var userRepo = new UserRepository(_context);
            var hasher = new Hasher();

            _threadService = new ThreadService(threadRepo, commentRepo, userRepo);
            _authService = new AuthService(userRepo, hasher);
        }
    
    // Остальной код...


        [HttpGet]
        public async Task<ActionResult> Index()
        {
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
            await _threadService.CreateThreadAsync(dto, userId);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> View(Guid id)
        {
            var thread = await _threadService.GetThreadByIdAsync(id);
            if (thread == null) return HttpNotFound();

            return View(thread);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(AddCommentDto dto, Guid threadId)
        {
            if (!ModelState.IsValid)
            {
                // Обработка ошибок
                return RedirectToAction("View", new { id = threadId });
            }

            var userId = GetCurrentUserId();
            await _threadService.AddCommentAsync(dto, threadId, userId);

            return RedirectToAction("View", new { id = threadId });
        }

        private Guid GetCurrentUserId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            return Guid.Parse(userIdClaim.Value);
        }
    }
}