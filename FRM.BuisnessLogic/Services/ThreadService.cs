// FRM.BuisnessLogic.Services/ThreadService.cs
using FRM.Core.DTOs;
using FRM.Core.Entities;
using FRM.Core.Interfaces.Repositories;
using FRM.Core.Interfaces.Services;
using FRM.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FRM.BuisnessLogic.Services
{
    public class ThreadService : IThreadService
    {
        private readonly IThreadRepository _threadRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IUserRepository _userRepo;

        public ThreadService(IThreadRepository threadRepo, ICommentRepository commentRepo, IUserRepository userRepo)
        {
            _threadRepo = threadRepo;
            _commentRepo = commentRepo;
            _userRepo = userRepo;
        }

        public async Task CreateThreadAsync(CreateThreadDto dto, Guid authorId)
        {
            var author = await _userRepo.GetByIdAsync(authorId);
            if (author == null) throw new Exception("User not found");

            string imageUrl = null;
            if (dto.AttachedImage != null && dto.AttachedImage.ContentLength > 0)
            {
                var uploadsFolder = "~/Uploads/ThreadImages";
                var physicalFolder = HttpContext.Current.Server.MapPath(uploadsFolder);
                if (!Directory.Exists(physicalFolder))
                {
                    Directory.CreateDirectory(physicalFolder);
                }
                var fileExtension = Path.GetExtension(dto.AttachedImage.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(physicalFolder, uniqueFileName);
                dto.AttachedImage.SaveAs(filePath);
                imageUrl = $"{uploadsFolder}/{uniqueFileName}";
            }

            var thread = new ThreadEf
            {
                Title = dto.Title,
                Content = dto.Content,
                AuthorId = authorId,
                ImageUrl = imageUrl
            };
            await _threadRepo.CreateAsync(thread);
        }

        // --- Метод получения всех тредов (он тоже был правильный) ---
       

        // --- ВОЗВРАЩАЕМ ЭТОТ МЕТОД К ПРОСТОЙ И НАДЕЖНОЙ ВЕРСИИ ---
        public async Task<ThreadEf> GetThreadByIdAsync(Guid id)
        {
            var thread = await _threadRepo.GetByIdAsync(id);

            if (thread != null)
            {
                // Используем сессию для защиты от накрутки просмотров
                string sessionKey = $"viewed_thread_{id}";
                if (HttpContext.Current.Session[sessionKey] == null)
                {
                    thread.ViewsCount++;
                    await _threadRepo.UpdateAsync(thread);
                    HttpContext.Current.Session[sessionKey] = true;
                }
            }

            return thread;
        }


        public async Task<IEnumerable<ThreadDto>> GetAllThreadsAsync()
        {
            var threads = await _threadRepo.GetAllAsync();
            return threads.Select(t => new ThreadDto
            {
                Id = t.Id,
                Title = t.Title,
                Content = t.Content,
                CreatedAt = t.CreatedAt,
                AuthorName = t.Author?.Name ?? "Неизвестный автор",
                CommentCount = t.Comments?.Count ?? 0,
                AuthorId = t.AuthorId,
                ViewsCount = t.ViewsCount
            });
        }

       

        public async Task AddCommentAsync(AddCommentDto dto, Guid threadId, Guid authorId)
        {
            string imageUrl = null;
            if (dto.AttachedImage != null && dto.AttachedImage.ContentLength > 0)
            {
                var uploadsFolder = "~/Uploads/CommentImages";
                var physicalFolder = HttpContext.Current.Server.MapPath(uploadsFolder);
                if (!Directory.Exists(physicalFolder))
                {
                    Directory.CreateDirectory(physicalFolder);
                }

                var fileExtension = Path.GetExtension(dto.AttachedImage.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(physicalFolder, uniqueFileName);

                dto.AttachedImage.SaveAs(filePath);
                imageUrl = $"{uploadsFolder}/{uniqueFileName}";
            }
            var comment = new CommentEf
            {
                Content = dto.Content,
                ThreadId = threadId,
                AuthorId = authorId,
                ImageUrl = imageUrl,
                ParentCommentId = dto.ParentCommentId
            };

            await _commentRepo.AddAsync(comment);
        }

        public async Task<bool> DeleteThreadAsync(Guid threadId, Guid currentUserId)
        {
            var thread = await _threadRepo.GetByIdAsync(threadId);
            if (thread == null) return false;

            if (thread.AuthorId != currentUserId) return false;

            await _threadRepo.DeleteAsync(threadId);
            return true;
        }

        public async Task<bool> DeleteCommentAsync(Guid commentId, Guid currentUserId)
        {
            var comment = await _commentRepo.GetByIdAsync(commentId);
            if (comment == null) return false;

            if (comment.AuthorId != currentUserId) return false;

            await _commentRepo.DeleteAsync(commentId);
            return true;
        }
    }
}