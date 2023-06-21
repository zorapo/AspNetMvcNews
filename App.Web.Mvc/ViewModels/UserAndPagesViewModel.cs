using App.Entities.Concrete;
using App.Entities.Dtos;

namespace App.Web.Mvc.ViewModels
{
	public class UserAndPagesViewModel
	{
		public PageListDto PageListDto { get; set; }
		public IList<User> User { get; set; }
	}
}
