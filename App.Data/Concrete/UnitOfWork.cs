using App.Data.Abstract;
using App.Data.Concrete.EntityFramework.Context;
using App.Data.Concrete.EntityFramework.Repositories;

namespace App.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private EfCategoryRepository _categoryRepository;
        private EfNewsRepository _newsRepository;
        private EfNewsImageRepository _newsImageRepository;
        private EfNewsCommentRepository _newsCommentRepository;
        private EfPageRepository _pageRepository;
        private EfSettingRepository _settingRepository;
        private EfContactRepository _contactRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public ICategoryRepository Categories => _categoryRepository ?? new EfCategoryRepository(_context);

        public INewsRepository News => _newsRepository ?? new EfNewsRepository(_context);

        public INewsImageRepository NewsImages => _newsImageRepository ?? new EfNewsImageRepository(_context);

        public INewsCommentRepository NewsComments => _newsCommentRepository ?? new EfNewsCommentRepository(_context);

        public IPageRepository Pages => _pageRepository ?? new EfPageRepository(_context);

        public ISettingRepository Settings => _settingRepository ?? new EfSettingRepository(_context);

        public IContactRepository Contacts => _contactRepository ?? new EfContactRepository(_context);

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync(); 
        }
    }
}
