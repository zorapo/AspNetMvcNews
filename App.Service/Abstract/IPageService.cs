using App.Entities.Concrete;
using App.Entities.Dtos;
using App.Shared.Utilities.Results.Abstract;

namespace App.Service.Abstract
{
	public interface IPageService
    {
        Task<IDataResult<Page>> GetPageAsync(int id);
        Task<IDataResult<PageListDto>> GetAllPageAsync();
        Task<IDataResult<Page>> UpdateAsync(Page page);
        Task<IDataResult<Page>> DeleteAsync(Page page);
        Task<IDataResult<Page>> AddAsync(Page page);
        Task<IDataResult<Page>> HardDeleteAsync(int id);
    }
}
