using App.Data.Abstract;
using App.Entities.Concrete;
using App.Shared.Data.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Concrete.EntityFramework.Repositories
{
    public class EfNewsCommentRepository : EfEntityRepositoryBase<NewsComment>, INewsCommentRepository
    {
        public EfNewsCommentRepository(DbContext context) : base(context)
        {
        }
    }
}
