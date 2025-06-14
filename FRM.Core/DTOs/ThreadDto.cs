using System;

namespace FRM.Core.DTOs
{
    public class ThreadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; } // Добавляем это свойство
        public DateTime CreatedAt { get; set; }
        public string AuthorName { get; set; }
        public int CommentCount { get; set; }
        public Guid AuthorId { get; set; }
    }
}