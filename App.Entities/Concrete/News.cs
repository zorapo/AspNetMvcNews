using App.Shared.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entities.Concrete
{
	public class News : EntityBase, IEntity, IAuiditEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }     

		[StringLength(200)]
		[Column(TypeName = "NVARCHAR")]
		public string Title { get; set; }

		[MaxLength]
		[Column(TypeName = "NVARCHAR(MAX)")]
		public string SubTitle { get; set; }

		[Column(TypeName = "NVARCHAR(MAX)")]
		[MaxLength]
		public string Content { get; set; }
        public  User? User { get; set; }
        public  Category Category { get; set; }

        //public NewsImage? NewsImage { get; set; }
        public string? ImagePath { get; set; }
		public  IEnumerable<NewsComment>? NewsComments { get; set; }
        public DateTime? CreatedAt { get; set; }=DateTime.Now;
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
