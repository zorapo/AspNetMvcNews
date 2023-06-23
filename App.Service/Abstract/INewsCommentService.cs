using App.Entities.Dtos.NewsCommentDtos;
using App.Shared.Utilities.Results.Abstract;

namespace App.Service.Abstract
{
    public interface INewsCommentService
    {
        Task<IDataResult<NewsCommentDto>> GetAsync(int commentId);

        Task<IDataResult<NewsCommentListDto>> GetAllAsync();
        Task<IDataResult<NewsCommentListDto>> GetNewsCommentsAsync(int newsId);
        Task<IDataResult<NewsCommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId);
        Task<IDataResult<NewsCommentListDto>> GetAllByNonDeletedAndActiveAsync();
        Task<IDataResult<NewsCommentDto>> AddAsync(NewsCommentAddDto commentAddDto);
        Task<IDataResult<NewsCommentDto>> ApproveAsync(int commetnId);
        Task<IDataResult<NewsCommentDto>> UpdateAsync(NewsCommentUpdateDto commentUpdateDto, string modifiedByName);
        Task<IDataResult<NewsCommentDto>> DeleteAsync(int commentId, string modifiedByName);
        Task<IResult> HardDeleteAsync(int commentId); //Tamamen vertabanından siler
        Task<IDataResult<int>> CountAsync();
        Task<IDataResult<int>> CountByNonDeletedAsync();


    }
}
 