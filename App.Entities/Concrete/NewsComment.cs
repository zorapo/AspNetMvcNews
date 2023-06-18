using App.Shared.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace App.Entities.Concrete
{
	public class NewsComment: EntityBase, IEntity, IAuiditEntity
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public int NewsId { get; set; }
        public News? News { get; set; }

		[Column(TypeName = "NVARCHAR(MAX)")]
		public string Comment { get; set; }
        public override bool IsActive { get; set; } = false;
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
