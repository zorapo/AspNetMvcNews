namespace App.Data.Abstract
{
	public interface IUnitOfWork:IAsyncDisposable
    {
        ICategoryRepository Categories { get; }

        INewsRepository News { get; }
        INewsImageRepository NewsImages { get; }

        INewsCommentRepository NewsComments { get; }

        IPageRepository Pages { get; }
        IContactRepository Contacts { get; }

        ISettingRepository Settings { get; }

        Task<int> SaveAsync();
    }
}
