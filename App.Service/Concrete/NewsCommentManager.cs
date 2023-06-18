using App.Data.Abstract;
using App.Entities.Concrete;
using App.Entities.Dtos.NewsCommentDtos;
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

        public async Task<IDataResult<NewsCommentDto>> AddAsync(NewsCommentAddDto commentAddDto)
        {         
            var comment = _mapper.Map<NewsComment>(commentAddDto);
            comment.CreatedByName = commentAddDto.Name;
            var addedComment = await _unitOfWork.NewsComments.AddAsync(comment);
            await _unitOfWork.SaveAsync();
            return new DataResult<NewsCommentDto>(ResultStatus.Success, "Yorumunuz başarıyla gönderilmiştir. Onaylandıktan sonra sisteme eklenecektir.", new NewsCommentDto
            {
                NewsComment = addedComment,
            });
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
            var newsCommentCount = await _unitOfWork.NewsComments.CountAsync(c => !c.IsDeleted);
            if (newsCommentCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, newsCommentCount);
            }
            return new DataResult<int>(ResultStatus.Error, "Beklenmeyen bir hata oluştu.", -1);
        }

        public async Task<IDataResult<NewsCommentDto>> DeleteAsync(int commentId, string modifiedByName)
        {
            var comment = await _unitOfWork.NewsComments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                comment.IsDeleted = true;
                comment.ModifiedByName = modifiedByName;
                var deletedComment = await _unitOfWork.NewsComments.UpdateAsync(comment);
                await _unitOfWork.SaveAsync();
                return new DataResult<NewsCommentDto>(ResultStatus.Success, "Yorum başarıyla silinmiştir", new NewsCommentDto
                {
                    NewsComment = deletedComment,
                });
            }
            return new DataResult<NewsCommentDto>(ResultStatus.Error, "Böyle bir yorum bulunamadı.", new NewsCommentDto
            {
                NewsComment = null,
            });
        }

        public async Task<IDataResult<NewsCommentListDto>> GetAllAsync()
        {
            
            var comments = await _unitOfWork.NewsComments.GetAllAsync(null,nc=>nc.News);
            if (comments.Count > -1)
            {
                return new DataResult<NewsCommentListDto>(ResultStatus.Success, new NewsCommentListDto
                {
                    NewsComments = comments,
                });
            }
            return new DataResult<NewsCommentListDto>(ResultStatus.Error, "Yorumlar bulunamadı.", new NewsCommentListDto
            {
                NewsComments = null,
            });
        }

        public async Task<IDataResult<NewsCommentListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var comments = await _unitOfWork.NewsComments.GetAllAsync(c => !c.IsDeleted && c.IsActive, nc => nc.News);
            if (comments.Count > -1)
            {
                return new DataResult<NewsCommentListDto>(ResultStatus.Success, new NewsCommentListDto
                {
                    NewsComments = comments,
                });
            }
            return new DataResult<NewsCommentListDto>(ResultStatus.Error, "Yorumlar bulunamadı.", new NewsCommentListDto
            {
                NewsComments = null,
            });
        }

        public async Task<IDataResult<NewsCommentDto>> GetAsync(int commentId)
        {
            var comment = await _unitOfWork.NewsComments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                return new DataResult<NewsCommentDto>(ResultStatus.Success, new NewsCommentDto
                {
                    NewsComment = comment,
                });
            }
            return new DataResult<NewsCommentDto>(ResultStatus.Error, "Yorum bulunamadı.", new NewsCommentDto
            {
                NewsComment = null,
            });
        }

        public async Task<IResult> HardDeleteAsync(int commentId)
        {
            var comment = await _unitOfWork.NewsComments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                await _unitOfWork.NewsComments.DeleteAsync(comment);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, "Yorum başarıyla veritabanından silinmiştir.");
            }
            return new Result(ResultStatus.Error, "Yorum bulunamadı.");
        }
        public async Task<IDataResult<NewsCommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId)
        {
            var result = await _unitOfWork.NewsComments.AnyAsync(c => c.Id == commentId);
            if (result)
            {
                var comment = await _unitOfWork.NewsComments.GetAsync(c => c.Id == commentId);
                var commentUpdateDto = _mapper.Map<NewsCommentUpdateDto>(comment);
                return new DataResult<NewsCommentUpdateDto>(ResultStatus.Success, commentUpdateDto);
            }
            else
            {
                return new DataResult<NewsCommentUpdateDto>(ResultStatus.Error, "Böyle bir yorum bulunamadı.", null);
            }
        }

        public async Task<IDataResult<NewsCommentDto>> UpdateAsync(NewsCommentUpdateDto commentUpdateDto, string modifiedByName)
        {
            var oldComment = await _unitOfWork.NewsComments.GetAsync(c => c.Id == commentUpdateDto.Id);
            var comment = _mapper.Map<NewsCommentUpdateDto, NewsComment>(commentUpdateDto, oldComment);
            comment.ModifiedByName = modifiedByName;
            var updatedComment = await _unitOfWork.NewsComments.UpdateAsync(comment);
            updatedComment.News = await _unitOfWork.News.GetAsync(n => n.Id == updatedComment.NewsId);
            await _unitOfWork.SaveAsync();
            return new DataResult<NewsCommentDto>(ResultStatus.Success, "Yorum başarıyla güncellenmiştir.", new NewsCommentDto
            {
                NewsComment = updatedComment,
            });
        }

		public async Task<IDataResult<NewsCommentDto>> ApproveAsync(int commetnId)
		{
            var comment = await _unitOfWork.NewsComments.GetAsync(c => c.Id==commetnId, c => c.News);
            if (comment != null)
            {
                comment.IsActive = true;
                comment.UpdatedAt = DateTime.Now;
                var updatedComment=await _unitOfWork.NewsComments.UpdateAsync(comment);
                await _unitOfWork.SaveAsync();
                return new DataResult<NewsCommentDto>(ResultStatus.Success, "Yorum başarıyla onaylanmıştır", new NewsCommentDto
                {
                    NewsComment = updatedComment
                });
                
            }
           return new DataResult<NewsCommentDto>(ResultStatus.Error, "Yorum bulunamadı", null);
		}
	}
}