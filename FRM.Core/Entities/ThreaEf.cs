using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ThreadEf.cs
namespace FRM.Core.Entities
{
    public class ThreadEf
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid AuthorId { get; set; }

        // Навигационные свойства
        public virtual UserEf Author { get; set; }
        public virtual ICollection<CommentEf> Comments { get; set; } = new List<CommentEf>();
    }
}