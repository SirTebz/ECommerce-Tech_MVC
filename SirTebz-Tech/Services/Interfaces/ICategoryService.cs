using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Models.ViewModels;

namespace SirTebz_Tech.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<CategoryCreateEditViewModel?> GetForEditAsync(int? id = null);
    Task CreateAsync(CategoryCreateEditViewModel model);
    Task UpdateAsync(CategoryCreateEditViewModel model);
    Task DeleteAsync(int id);
}