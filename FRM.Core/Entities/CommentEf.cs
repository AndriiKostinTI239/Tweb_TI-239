using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace FRM.Core.Entities
{
    public class CommentEf
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid ThreadId { get; set; }
        public Guid AuthorId { get; set; }

        // Навигационные свойства
        public virtual ThreadEf Thread { get; set; }
        public virtual UserEf Author { get; set; }
        public Guid? ParentCommentId { get; set; } // Nullable, т.к. у корневых комментариев нет родителя

        public virtual CommentEf ParentComment { get; set; }
        public virtual ICollection<CommentEf> Replies { get; set; } = new List<CommentEf>();
    }
}