using App.Shared.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entities.Concrete
{
    public class Setting: EntityBase, IEntity
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }

		[StringLength(200)]
		[Column(TypeName = "NVARCHAR")]
		public string Name { get; set; }

		[StringLength(400)]
		[Column(TypeName = "NVARCHAR")]
		public string Value { get; set; }
    }
}
