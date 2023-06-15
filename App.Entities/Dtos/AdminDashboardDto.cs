using App.Entities.Dtos.NewsDtos;

namespace App.Entities.Dtos
{
    public class AdminDashboardDto
    {
        public int CategoriesCount { get; set; }
        public int NewsCount { get; set; }
        public int NewsCommentsCount { get; set; }
        public int UsersCount { get; set; }
        public NewsListDto NewsList { get; set; }
    }
}
