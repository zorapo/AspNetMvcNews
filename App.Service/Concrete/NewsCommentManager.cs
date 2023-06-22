using App.Data.Abstract;
using App.Entities.Concrete;
using App.Entities.Dtos.NewsCommentDtos;
using App.Service.Abstract;
using App.Shared.Utilities.Results.Abstract;
using App.Shared.Utilities.Results.ComplexTypes;
using App.Shared.Utilities.Results.Concrete;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace App.Service.Concrete
{
    public class NewsCommentManager : INewsCommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

		public NewsCommentManager(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userManager = userManager;
		}

		public async Task<IDataResult<NewsCommentDto>> AddAsync(NewsCommentAddDto commentAddDto)
        {
            var news = await _unitOfWork.News.GetAsync(n => n.Id == commentAddDto.NewsId);
			if (news == null)
			{
				return new DataResult<NewsCommentDto>(ResultStatus.Error, "Böyle bir haber bulunamadı.",null);
			}
			var comment = _mapper.Map<NewsComment>(commentAddDto);
            comment.CreatedByName = commentAddDto.Name;
            comment.User=await _userManager.FindByEmailAsync(commentAddDto.Email);
            comment.UserId = commentAddDto.UserId;
            comment.NewsId = commentAddDto.NewsId;
            comment.CreatedAt = DateTime.Now;
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
        public async Task<IDataResult<NewsCommentListDto>> GetNewsCommentsAsync(int newsId)
        {

            var comments = await _unitOfWork.NewsComments.GetAllAsync(c=>c.NewsId== newsId&&!c.IsDeleted && c.IsActive, nc => nc.News,nc=>nc.User);
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
                var comment = await _unitOfWork.NewsComments.GetAsync(c => c.Id == commentId,c=>c.News);
            if (comment!=null)
            {
                return new DataResult<NewsCommentUpdateDto>(ResultStatus.Success, new NewsCommentUpdateDto
                {
                    Id = comment.Id,
                    NewsId = comment.NewsId,
                    IsActive = comment.IsActive,
                    IsDeleted = comment.IsDeleted,
                    Comment=comment.Comment

                });
            }
            else
            {
                return new DataResult<NewsCommentUpdateDto>(ResultStatus.Error, "Böyle bir yorum bulunamadı.", null);
            }
        }

        public async Task<IDataResult<NewsCommentDto>> UpdateAsync(NewsCommentUpdateDto commentUpdateDto, string modifiedByName)
        {
            var oldComment = await _unitOfWork.NewsComments.GetAsync(c => c.Id == commentUpdateDto.Id,c=>c.News);
            var comment = _mapper.Map<NewsCommentUpdateDto, NewsComment>(commentUpdateDto, oldComment);
            comment.ModifiedByName = modifiedByName;
            comment.UpdatedAt = DateTime.Now;         
            var updatedComment = await _unitOfWork.NewsComments.UpdateAsync(comment);
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