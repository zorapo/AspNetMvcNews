using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Dtos
{
	public class UserWithRolesDto
	{
        public UserWithRolesDto()
        {
            AssignRoleToUserDtos = new List<AssignRoleToUserDto>();

		}
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<AssignRoleToUserDto> AssignRoleToUserDtos { get; set; }
    }
}
