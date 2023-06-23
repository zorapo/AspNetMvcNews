using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Shared.Entities.Abstract;

namespace App.Entities.Concrete
{
    public class Contact: EntityBase, IEntity, IAuiditEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Name")]
        [StringLength(30)]
        [Column(TypeName = "NVARCHAR")]
        public string? Name { get; set; }

        [DisplayName("Email")]
		[MaxLength(100)]
		[MinLength(10)]
		[DataType(DataType.EmailAddress)]
		[Column(TypeName = "NVARCHAR(MAX)")]
        public string? Email { get; set; }

        [DisplayName("Message")]
		[MaxLength(1000)]
		[MinLength(10)]
		[Column(TypeName = "NVARCHAR(MAX)")]
        public string? Message { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
