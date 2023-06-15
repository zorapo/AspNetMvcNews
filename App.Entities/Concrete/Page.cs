using App.Shared.Entities.Abstract;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entities.Concrete
{
	public class Page : EntityBase, IEntity, IAuiditEntity
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[StringLength(20)]
		[Column(TypeName = "NVARCHAR")]
		public string Title { get; set; }

		[Column(TypeName = "text")]
		public string? Content { get; set; }
		public string? Content_One { get; set; }
		public string? Content_Two { get; set; }
		public bool IsActive { get; set; }
		public string? ImagePath { get; set; }

		[NotMapped]
        public IFormFile? ImageFile { get; set; }

        [DisplayName("Durum")]
        public string? IsActiveString => !IsActive? "Pasif" : "Aktif";
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
	}
}
