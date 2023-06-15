using App.Entities.Concrete;
using App.Entities.Dtos.CategoryDtos;
using App.Shared.Utilities.Results.Abstract;

namespace App.Service.Abstract
{
    public interface ICategoryService
    {
        Task<IDataResult<CategoryDto>> GetAsync(int categoryId);
        Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId);
        Task<IDataResult<CategoryListDto>> GetAllAsync();
        Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync();
        Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync();
        Task<IDataResult<CategoryListDto>> GetCategoryByIdAsync(int categoryId);
        Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName);
        Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName);
		Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName);
        Task<IResult> HardDeleteAsync(int categoryId); //Tamamen vertabanından siler
        Task<IDataResult<int>> CountAsync();
        Task<IDataResult<int>> CountByNonDeletedAsync();


    }
}
