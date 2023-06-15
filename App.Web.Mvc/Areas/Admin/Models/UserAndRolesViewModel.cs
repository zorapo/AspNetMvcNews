using App.Entities.Concrete;

namespace App.Web.Mvc.Areas.Admin.Models
{
	public class UserAndRolesViewModel
	{
        public User User { get; set; }
        public IList<string> Roles { get; set; }
	}
}
