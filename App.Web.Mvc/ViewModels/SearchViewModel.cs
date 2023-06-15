using App.Entities.Concrete;
using X.PagedList;

namespace App.Web.Mvc.ViewModels
{
	public class SearchViewModel
	{
		public IPagedList<News> NewsList { get; set; }
        public string Keyword { get; set; }
    }
}
