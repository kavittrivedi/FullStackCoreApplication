using FullStackApplication.Services.Contract;
using FullStackApplication.ViewModels;

namespace FullStackApplication.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ApiService _apiService;
        private const string ApiBaseUri = "api/category/";

        public CategoryService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _apiService.GetAllAsync<Category>(ApiBaseUri);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _apiService.GetAsync<Category>($"{ApiBaseUri}{id}");
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _apiService.PostAsync(ApiBaseUri, category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _apiService.PutAsync($"{ApiBaseUri}{category.Id}", category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _apiService.DeleteAsync($"{ApiBaseUri}{id}");
        }
    }
}
