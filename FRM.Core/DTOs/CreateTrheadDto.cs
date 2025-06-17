using System.ComponentModel.DataAnnotations;
using System.Web;
namespace FRM.Core.DTOs
{
    public class CreateThreadDto
    {
        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(100, ErrorMessage = "Максимум 100 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержание обязательно")]
        public string Content { get; set; }
        public HttpPostedFileBase AttachedImage { get; set; }
    }
}

