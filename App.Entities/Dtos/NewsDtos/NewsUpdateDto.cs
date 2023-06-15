using App.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entities.Dtos.NewsDtos
{
    public class NewsUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Başlık")]
        [StringLength(200)]
        [Column(TypeName = "NVARCHAR")]
        public string Title { get; set; }

        [DisplayName("İçerik Giriş")]
        [MaxLength]
        [Column(TypeName = "ntext")]
        public string SubTitle { get; set; }

        [DisplayName("İçerik")]
        [Column(TypeName = "ntext")]
        [MaxLength]
        public string Content { get; set; }

        [DisplayName("Kategori")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string UserId { get; set; }
        public User? User { get; set; }

        [DisplayName("Resim")]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }

        [DisplayName("Resim")]
        public string? ImagePath { get; set; }

        [DisplayName("Güncellenme Tarihi")]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        [DisplayName("Aktif Mi?")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        public bool IsActive { get; set; }

        [DisplayName("Silinsin Mi?")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        public bool IsDeleted { get; set; }
    }
}
