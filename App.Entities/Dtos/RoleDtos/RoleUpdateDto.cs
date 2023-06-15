using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Entities.Dtos.RoleDtos
{
    public class RoleUpdateDto
    {
        [Required]
        public string Id { get; set; }

        [DisplayName("Rol")]
        public string Name { get; set; }
    }
}
