using App.Data.Abstract;
using App.Entities.Concrete;
using App.Shared.Data.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Concrete.EntityFramework.Repositories
{
    public class EfNewsImageRepository : EfEntityRepositoryBase<NewsImage>, INewsImageRepository
    {
        public EfNewsImageRepository(DbContext context) : base(context)
        {
        }
    }
}
