using App.Entities.Concrete;
using App.Entities.Dtos.NewsDtos;
using App.Shared.Utilities.Results.Abstract;
using X.PagedList;

namespace App.Service.Abstract
{
    public interface INewsService
    {
        Task<IDataResult<NewsDto>> GetAsync(int newsId);

		/// <summary>
		/// Parametre olarak gelen newsId ile NewsUpdateDto modelinin güncellenmesini sağlar
		/// </summary>
		/// <param name="newsId">0'dan büyük integer değer</param>
		/// <returns>Asenkron operasyon ile Task olarak DataResult tipinde döner</returns>
		Task<IDataResult<NewsUpdateDto>> GetUpdateDtoAsync(int newsId);
        Task<IDataResult<NewsListDto>> GetAllAsync();
        Task<IDataResult<NewsListDto>> GetAllByNonDeletedAsync();
        Task<IDataResult<NewsListDto>> GetAllByNonDeletedAndActiveAsync();
        Task<IPagedList<News>> GetAllNewsByCategoryPagingAsync(int? categoryId,int currentPage=1,int pageSize=6, bool isAscending = false);
        Task<IPagedList<News>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 6, bool isAscending = false);
        Task<IDataResult<NewsListDto>> GetAllByCategoryAsync(int categoyId);
        Task<IDataResult<NewsListDto>> GetNewsByCategoryIdAsync(int categoryId);
        Task<IResult> AddAsync(NewsAddDto newsAddDto, string createdByName);
        Task<IResult> UpdateAsync(NewsUpdateDto newsUpdateDto, string modifiedByName);
        Task<IResult> DeleteAsync(int newsId, string modifiedByName);
        Task<IResult> HardDeleteAsync(int newsId); //Tamamen vertabanından siler
        Task<IDataResult<int>> CountAsync();
        Task<IDataResult<int>> CountByNonDeletedAsync();

    }
}
