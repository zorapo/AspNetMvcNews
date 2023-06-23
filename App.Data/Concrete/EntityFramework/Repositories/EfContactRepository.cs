using App.Data.Abstract;
using App.Entities.Concrete;
using App.Shared.Data.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Concrete.EntityFramework.Repositories
{
    public class EfContactRepository : EfEntityRepositoryBase<Contact>, IContactRepository
    {
        public EfContactRepository(DbContext context) : base(context)
        {
        }
    }
}
