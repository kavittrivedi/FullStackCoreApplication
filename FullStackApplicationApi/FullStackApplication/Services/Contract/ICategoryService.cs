using FullStackApplication.ViewModels;

namespace FullStackApplication.Services.Contract
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<Category> GetCategoryByIdAsync(int id);

          Task AddCategoryAsync(Category category);

          Task UpdateCategoryAsync(Category category);

        Task DeleteCategoryAsync(int id);
    }
}
