using App.Entities.Concrete;

namespace App.Entities.Dtos.CategoryDtos
{
    public class CategoryListDto
    {
        public IList<Category> Categories { get; set; }
    }
}
