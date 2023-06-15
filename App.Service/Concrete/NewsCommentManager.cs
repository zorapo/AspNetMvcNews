using App.Data.Abstract;
using App.Service.Abstract;
using App.Shared.Utilities.Results.Abstract;
using App.Shared.Utilities.Results.ComplexTypes;
using App.Shared.Utilities.Results.Concrete;
using AutoMapper;

namespace App.Service.Concrete
{
    public class NewsCommentManager : INewsCommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewsCommentManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IDataResult<int>> CountAsync()
        {
            var newsCommentCount = await _unitOfWork.NewsComments.CountAsync();
            if (newsCommentCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, newsCommentCount);
            }
            return new DataResult<int>(ResultStatus.Error, "Beklenmeyen bir hata oluştu.", -1);
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var newsCommentCount = await _unitOfWork.NewsComments.CountAsync(c=>!c.IsDeleted);
            if (newsCommentCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, newsCommentCount);
            }
            return new DataResult<int>(ResultStatus.Error, "Beklenmeyen bir hata oluştu.", -1);
        }
    }
}
