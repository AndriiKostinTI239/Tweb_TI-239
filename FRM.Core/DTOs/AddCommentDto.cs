using System;
using System.ComponentModel.DataAnnotations;

namespace FRM.Core.DTOs
{
    public class AddCommentDto
    {
        [Required(ErrorMessage = "Комментарий не может быть пустым")]
        public string Content { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}