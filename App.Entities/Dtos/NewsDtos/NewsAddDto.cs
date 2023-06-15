using App.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entities.Dtos.NewsDtos
{
    public class NewsAddDto
    {
        [DisplayName("Başlık")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        [StringLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string Title { get; set; }

        [DisplayName("İçerik Giriş")]
        [MaxLength]
        [Column(TypeName = "ntext")]
        public string SubTitle { get; set; }

        [DisplayName("İçerik")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        [Column(TypeName = "ntext")]
        [MaxLength]
        public string Content { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }

        [DisplayName("Resim")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }

        [DisplayName("Resim")]
        public string? ImagePath { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [DisplayName("Aktif Mi?")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        public bool IsActive { get; set; }
    }
}
