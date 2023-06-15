using App.Shared.Entities.Abstract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entities.Concrete
{
	public class Category: EntityBase,IEntity
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
		[DisplayName("Kategori Adı")]
		[StringLength(100)]
		[Column(TypeName = "NVARCHAR")]
		public string Name { get; set; }

		[StringLength(200)]
		[Column(TypeName = "NVARCHAR")]
		public string? Description { get; set; }
        public IEnumerable<News>? News { get; set; }
    }
}
