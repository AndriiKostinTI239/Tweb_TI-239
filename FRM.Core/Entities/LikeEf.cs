// FRM.Core/Entities/LikeEf.cs
using System;

namespace FRM.Core.Entities
{
    public class LikeEf
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Кто поставил лайк
        public Guid UserId { get; set; }
        public virtual UserEf User { get; set; }

        // Какой комментарий лайкнули
        public Guid CommentId { get; set; }
        public virtual CommentEf Comment { get; set; }
    }
}