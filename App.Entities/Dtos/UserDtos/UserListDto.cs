using App.Entities.Concrete;

namespace App.Entities.Dtos.UserDtos
{
    public class UserListDto
    {
        public IList<User> Users { get; set; }
        public Role Roles { get; set; }
    }
}
