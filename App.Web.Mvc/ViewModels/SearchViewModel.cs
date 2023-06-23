using App.Entities.Concrete;
using App.Entities.Dtos.NewsDtos;
using X.PagedList;

namespace App.Web.Mvc.ViewModels
{
	public class SearchViewModel
	{
		public IPagedList<News> NewsList { get; set; }
		public NewsListDto News { get; set; }
        public string Keyword { get; set; }
    }
}
