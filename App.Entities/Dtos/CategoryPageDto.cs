using App.Entities.Concrete;
using App.Entities.Dtos.CategoryDtos;
using App.Entities.Dtos.NewsDtos;
using X.PagedList;

namespace App.Entities.Dtos
{
	public class CategoryPageDto
	{
		public NewsListDto NewsList { get; set; }
		public Category Category { get; set; }
		public CategoryListDto? Categories { get; set; }
		public IPagedList<News> PagedNews { get; set;}
	}
}
