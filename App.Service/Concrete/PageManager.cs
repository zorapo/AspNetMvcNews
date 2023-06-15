using App.Data.Abstract;
using App.Entities.Concrete;
using App.Entities.Dtos;
using App.Service.Abstract;
using App.Shared.Utilities.Results.Abstract;
using App.Shared.Utilities.Results.ComplexTypes;
using App.Shared.Utilities.Results.Concrete;

namespace App.Service.Concrete
{
	public class PageManager : IPageService
	{
		private readonly IUnitOfWork _unitOfWork;

		public PageManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IDataResult<Page>> AddAsync(Page page)
		{
			if (page is null)
				return new DataResult<Page>(ResultStatus.Error, "Bir hata oluştu", null);

			page.UpdatedAt = DateTime.Now;
			page.CreatedAt = DateTime.Now;
			page.ModifiedByName = "Admin";
			await _unitOfWork.Pages.AddAsync(page);
			await _unitOfWork.SaveAsync();
			return new DataResult<Page>(ResultStatus.Success, $"{page.Title} başlıklı sayfa başarıyla eklenmiştir", null);
		}

		public async Task<IDataResult<Page>> DeleteAsync(Page page)
		{

			if (page is null)
				return new DataResult<Page>(ResultStatus.Error, "Bir hata oluştu", null);
			if (page.IsDeleted)
				return new DataResult<Page>(ResultStatus.Warning, "Bu sayfa daha önceden silinmiş", null);
			if (!page.IsActive)
				return new DataResult<Page>(ResultStatus.Warning, "Bu sayfa aktif değil", null);
			page.IsActive = false;
			page.DeletedAt = DateTime.Now;
			page.ModifiedByName = "Admin";
			page.IsDeleted = true;
			await _unitOfWork.Pages.UpdateAsync(page);
			await _unitOfWork.SaveAsync();
			return new DataResult<Page>(ResultStatus.Success, $"{page.Title} başlıklı sayfa başarılı bir şekilde silinmiştir", null);
		}

		public async Task<IDataResult<Page>> GetPageAsync(int id)
		{
			var page = await _unitOfWork.Pages.GetAsync(a => a.Id == id);
			if (page is null)
				return new DataResult<Page>(ResultStatus.Error, "Bir hata oluştu", null);
			return new DataResult<Page>(ResultStatus.Success, page);
		}
		public async Task<IDataResult<PageListDto>> GetAllPageAsync()
		{
			var pages = await _unitOfWork.Pages.GetAllAsync();
			if (pages is null)
				return new DataResult<PageListDto>(ResultStatus.Error, "Bir hata oluştu", null);
			return new DataResult<PageListDto>(ResultStatus.Success, new PageListDto
			{
				Pages = pages,
			});
		}


		public async Task<IDataResult<Page>> UpdateAsync(Page page)
		{

			if (page is null)
				return new DataResult<Page>(ResultStatus.Error, "Bir hata oluştu", null);

			await _unitOfWork.Pages.UpdateAsync(page);
			await _unitOfWork.SaveAsync();
			return new DataResult<Page>(ResultStatus.Success, page);

		}

		public async Task<IDataResult<Page>> HardDeleteAsync(int id)
		{
			var page = await _unitOfWork.Pages.GetAsync(a => a.Id == id);
			if (page is null || page.IsDeleted || !page.IsActive)
				return new DataResult<Page>(ResultStatus.Error, "Bir hata oluştu", null);
			await _unitOfWork.Pages.DeleteAsync(page);
			await _unitOfWork.SaveAsync();
			return new DataResult<Page>(ResultStatus.Success, $"{page.Title} başlıklı sayfa başarılı bir şekilde kalıcı olarak silinmiştir.", null);

		}
	}
}
