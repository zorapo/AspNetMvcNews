using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Entities.Dtos.NewsCommentDtos
{
    public class NewsCommentUpdateDto
    {

        [Required]
        public int Id { get; set; }

        [DisplayName("Comment")]
        [Required]
        [MaxLength(1000)]
        [MinLength(2)]
        public string Comment { get; set; }

        [DisplayName("Is Active?")]
        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int NewsId { get; set; }
    }
}
