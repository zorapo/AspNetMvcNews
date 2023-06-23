namespace App.Entities.Dtos
{
	public class AssignRoleToUserDto
	{
		public string RoleId { get; set; }
		public string RoleName { get; set; }

		public bool HasRole { get; set; }
	}
}
