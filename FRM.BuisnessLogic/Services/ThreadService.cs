using FRM.Core.DTOs;
using FRM.Core.Entities;
using FRM.Core.Interfaces.Repositories;
using FRM.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRM.BuisnessLogic.Services
{
    public class ThreadService : IThreadService
    {
        private readonly IThreadRepository _threadRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IUserRepository _userRepo;

        public ThreadService(
            IThreadRepository threadRepo,
            ICommentRepository commentRepo,
            IUserRepository userRepo)
        {
            _threadRepo = threadRepo;
            _commentRepo = commentRepo;
            _userRepo = userRepo;
        }

        public async Task CreateThreadAsync(CreateThreadDto dto, Guid authorId)
        {
            var author = await _userRepo.GetByIdAsync(authorId);
            if (author == null) throw new Exception("User not found");

            var thread = new ThreadEf
            {
                Title = dto.Title,
                Content = dto.Content,
                AuthorId = authorId
            };

            await _threadRepo.CreateAsync(thread);
        }

        public async Task<IEnumerable<ThreadDto>> GetAllThreadsAsync()
        {
            var threads = await _threadRepo.GetAllAsync();
            return threads.Select(t => new ThreadDto
            {
                Id = t.Id,
                Title = t.Title,
                // Исправление: используем правильное свойство
                Content = t.Content,
                CreatedAt = t.CreatedAt,
                // Защита от null-значений
                AuthorName = t.Author?.Name ?? "Неизвестный автор",
                // Защита от null-коллекции
                CommentCount = t.Comments?.Count ?? 0
            });
        }

        public async Task<ThreadEf> GetThreadByIdAsync(Guid id)
        {
            return await _threadRepo.GetByIdAsync(id);
        }

        public async Task AddCommentAsync(AddCommentDto dto, Guid threadId, Guid authorId)
        {
            var comment = new CommentEf
            {
                Content = dto.Content,
                ThreadId = threadId,
                AuthorId = authorId
            };

            await _commentRepo.AddAsync(comment);
        }
    }
}