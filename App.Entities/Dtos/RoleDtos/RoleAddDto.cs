using System.ComponentModel;

namespace App.Entities.Dtos.RoleDtos
{
    public class RoleAddDto
    {
        [DisplayName("Rol")]
        public string Name { get; set; }
    }
}
