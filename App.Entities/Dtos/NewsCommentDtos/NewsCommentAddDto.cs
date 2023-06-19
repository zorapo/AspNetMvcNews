using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Entities.Dtos.NewsCommentDtos
{
    public class NewsCommentAddDto
    {
        [DisplayName("Full Name")]
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }

        [DisplayName("E-mail Address")]
        [Required]
        [MaxLength(100)]
        [MinLength(10)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Comment")]
        [Required]
        [MaxLength(1000)]
        [MinLength(10)]
        public string Comment { get; set; }

		[Required]
		public int NewsId { get; set; }
		public string UserId { get; set; }
	}
}
