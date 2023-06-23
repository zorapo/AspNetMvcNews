using App.Data.Abstract;
using App.Entities.Concrete;
using App.Entities.Dtos.NewsDtos;
using App.Service.Abstract;
using App.Shared.Utilities.Results.Abstract;
using App.Shared.Utilities.Results.ComplexTypes;
using App.Shared.Utilities.Results.Concrete;
using AutoMapper;
using System.Linq.Expressions;
using X.PagedList;

namespace App.Service.Concrete
{
    public class NewsManager:INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

		public NewsManager(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IResult> AddAsync(NewsAddDto newsAddDto, string createdByName)
        {
			var news = _mapper.Map<News>(newsAddDto);
            news.CreatedByName = createdByName;
            await _unitOfWork.News.AddAsync(news);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, $"{newsAddDto.Title} başlıklı haber başarıyla eklenmiştir.");
        }

        public async Task<IResult> DeleteAsync(int newsId, string modifiedByName)
        {
            var result = await _unitOfWork.News.AnyAsync(a => a.Id == newsId);
            if (result)
            {
                var news = await _unitOfWork.News.GetAsync(a => a.Id == newsId);
                news.IsDeleted = true;
                news.IsActive = false;
                news.ModifiedByName = modifiedByName;
                news.UpdatedAt = DateTime.Now;
                news.DeletedAt = DateTime.Now;
                await _unitOfWork.News.UpdateAsync(news);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{news.Title}başlıklı haber başarıyla silinmiştir.");
            }
            return new Result(ResultStatus.Error, "Böyle bir haber bulunamadı.");
        }

        public async Task<IDataResult<NewsDto>> GetAsync(int newsId)
        {
            var news = await _unitOfWork.News.GetAsync(a => a.Id == newsId, a => a.User, a => a.Category,news=>news.NewsComments);
            if (news != null)
            {
                news.NewsComments = await _unitOfWork.NewsComments.GetAllAsync(c => c.NewsId == newsId && !c.IsDeleted&&c.IsActive);
                return new DataResult<NewsDto>(ResultStatus.Success, new NewsDto
                {
                    News = news,
                });
            }
            return new DataResult<NewsDto>(ResultStatus.Error, "Böyle bir haber bulunamadı.", null);
        }

        public async Task<IDataResult<NewsListDto>> GetAllAsync()
        {
            var news = await _unitOfWork.News.GetAllAsync(null, a => a.User, a => a.Category);
            if (news.Count > -1) // İlk başta uygulamamıza hiç haber eklememiş olabiliriz. Admin panelinden eklemek isteyebiliriz. Oyüzden sıfır da olabilir.
            {
                return new DataResult<NewsListDto>(ResultStatus.Success, new NewsListDto
                {
                    News = news,      
                });
            }
            return new DataResult<NewsListDto>(ResultStatus.Error, "Haberler bulunamadı.", null);
        }

        public async Task<IDataResult<NewsListDto>> GetAllByCategoryAsync(int categoyId)
        {
            var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoyId);
            if (result)
            {

                var news = await _unitOfWork.News.GetAllAsync(a => a.CategoryId == categoyId && !a.IsDeleted && a.IsActive, ar => ar.User, news => news.Category);
                if (news.Count > -1) // İlk başta uygulamamıza hiç haber eklememiş olabiliriz. Admin panelinden eklemek isteyebiliriz. Oyüzden sıfır da olabilir.
                {
                    return new DataResult<NewsListDto>(ResultStatus.Success, new NewsListDto
                    {
                        News = news,          
                    });
                }
                return new DataResult<NewsListDto>(ResultStatus.Error, "Haberler bulunamadı.", null);
            }
            return new DataResult<NewsListDto>(ResultStatus.Error, "Böyle bir kategori bulunamadı", null);
        }

        public async Task<IDataResult<NewsListDto>> GetAllByNonDeletedAsync()
        {
            var news = await _unitOfWork.News.GetAllAsync(a => !a.IsDeleted, ar => ar.Category, ar => ar.User);
            if (news.Count > -1) // İlk başta uygulamamıza hiç haber eklememiş olabiliriz. Admin panelinden eklemek isteyebiliriz. Oyüzden sıfır da olabilir.
            {
                return new DataResult<NewsListDto>(ResultStatus.Success, new NewsListDto
                {
                    News = news,
                });
            }
            return new DataResult<NewsListDto>(ResultStatus.Error, "Haberler bulunamadı.", null);
        }

        public async Task<IDataResult<NewsListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var news = await _unitOfWork.News.GetAllAsync(a => !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
            if (news.Count > -1) // İlk başta uygulamamıza hiç makale eklememiş olabiliriz. Admin panelinden eklemek isteyebiliriz. Oyüzden sıfır da olabilir.
            {
                return new DataResult<NewsListDto>(ResultStatus.Success, new NewsListDto
                {
                    News = news,
                });
            }
            return new DataResult<NewsListDto>(ResultStatus.Error, "Haberler bulunamadı.", null);
        }

        public async Task<IDataResult<NewsListDto>> GetNewsByCategoryIdAsync(int categoryId)
        {
            var news = await _unitOfWork.News.GetAllAsync(c => c.CategoryId == categoryId,t=>t.Category,t =>t.User);
            if (news.Count > -1) 
            {
                return new DataResult<NewsListDto>(ResultStatus.Success, new NewsListDto
                {
                    News = news,
                });
            }
            return new DataResult<NewsListDto>(ResultStatus.Error, "Haberler bulunamadı.", null);
        
        }
 
		public async Task<IDataResult<NewsUpdateDto>> GetUpdateDtoAsync(int newsId)
        {
            var news = await _unitOfWork.News.GetAsync(a => a.Id == newsId, a => a.User, a => a.Category);
            if (news != null)
            {
                return new DataResult<NewsUpdateDto>(ResultStatus.Success, new NewsUpdateDto
                {
                    Category=news.Category,
                    CategoryId=news.CategoryId,
                    SubTitle=news.SubTitle,
                    Content=news.Content,
                    UpdatedAt=news.UpdatedAt,
                    Id=news.Id,
                    Title=news.Title,
                    IsActive=news.IsActive,
                    IsDeleted=news.IsDeleted,
                    ImagePath=news.ImagePath
                });
            }
            return new DataResult<NewsUpdateDto>(ResultStatus.Error, "Böyle bir haber bulunamadı.", null);
        }

        public async Task<IResult> HardDeleteAsync(int newsId)
        {
            var result = await _unitOfWork.News.AnyAsync(a => a.Id == newsId);
            if (result)
            {
                var news = await _unitOfWork.News.GetAsync(a => a.Id == newsId);
                await _unitOfWork.News.DeleteAsync(news);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{news.Title}başlıklı haber başarıyla veritabanından silinmiştir.");
            }
            return new Result(ResultStatus.Error, "Böyle bir haber bulunamadı.");
        }

        public async Task<IResult> UpdateAsync(NewsUpdateDto newsUpdateDto, string modifiedByName)
        {
            var news = _mapper.Map<News>(newsUpdateDto);
            news.ModifiedByName = modifiedByName;
            news.CreatedByName = "Admin";
            await _unitOfWork.News.UpdateAsync(news);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, $"{newsUpdateDto.Title} başlıklı haber başarıyla güncellenmiştir.");
        }

        public async Task<IPagedList<News>> GetAllNewsByCategoryPagingAsync(int? categoryId, int currentPage=1, int pageSize=6,bool isAscending=false)
        {
            var news = categoryId == null ? await _unitOfWork.News.GetAllAsync(n => n.IsActive && !n.IsDeleted, n => n.Category, n => n.User)
                : await _unitOfWork.News.GetAllAsync(n => n.CategoryId == categoryId && n.IsActive && !n.IsDeleted, n => n.Category, n => n.User);
            var sortedNews = isAscending ? await news.OrderBy(t => t.CreatedAt).ToPagedListAsync(currentPage, pageSize)
				:  await news.OrderByDescending(t => t.CreatedAt).ToPagedListAsync(currentPage, pageSize);
            return sortedNews;
        }
        public async Task<IDataResult<int>> CountAsync()
        {
            var newsCount = await _unitOfWork.News.CountAsync();
            if (newsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, newsCount);
            }
            return new DataResult<int>(ResultStatus.Error, "Beklenmeyen bir hata oluştu.", -1);
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var newsCount = await _unitOfWork.News.CountAsync(n=>!n.IsDeleted);
            if (newsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, newsCount);
            }
            return new DataResult<int>(ResultStatus.Error, "Beklenmeyen bir hata oluştu.", -1);
        }

		public async Task<IPagedList<News>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 6, bool isAscending = false)
		{
            if (string.IsNullOrWhiteSpace(keyword))
            {
                var news = await _unitOfWork.News.GetAllAsync(n => n.IsActive && !n.IsDeleted, n => n.Category, n => n.User);
			  
				var sortedNews = isAscending ? await news.OrderBy(t => t.CreatedAt).ToPagedListAsync(currentPage, pageSize)
					: await news.OrderByDescending(t => t.CreatedAt).ToPagedListAsync(currentPage, pageSize);
				return sortedNews;
			}
            keyword = keyword.Trim();
            var searchedNews = await _unitOfWork.News.SearchAsync(new List<Expression<Func<News, bool>>>
            {
                news=>news.Title.ToUpper().Contains(keyword.ToUpper()),
                news=>news.SubTitle.ToUpper().Contains(keyword.ToUpper()),
                news=>news.Content.ToUpper().Contains(keyword.ToUpper()),
                news=>news.Category.Name.ToUpper().Contains(keyword.ToUpper())
            },
              news=>news.Category);
            var searchedAndSortedNews= isAscending ? await searchedNews.OrderBy(t => t.CreatedAt).ToPagedListAsync(currentPage, pageSize)
					: await searchedNews.OrderByDescending(t => t.CreatedAt).ToPagedListAsync(currentPage, pageSize);
			return searchedAndSortedNews;
		
		}
	}
}
