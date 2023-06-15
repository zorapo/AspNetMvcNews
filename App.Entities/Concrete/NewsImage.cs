using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using App.Shared.Entities.Abstract;

namespace App.Entities.Concrete
{
	public class NewsImage: EntityBase, IEntity
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        public int NewsId { get; set; }
        public News? News { get; set; }

		[StringLength(200)]
		[Column(TypeName = "NVARCHAR")]
		public string ImagePath { get; set; }
    }
}
