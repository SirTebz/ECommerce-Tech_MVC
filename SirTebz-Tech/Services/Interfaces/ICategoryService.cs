using SirTebzTech.Models.Entities;
using SirTebzTech.Models.ViewModels;

namespace SirTebzTech.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<CategoryCreateEditViewModel?> GetForEditAsync(int? id = null);
    Task CreateAsync(CategoryCreateEditViewModel model);
    Task UpdateAsync(CategoryCreateEditViewModel model);
    Task DeleteAsync(int id);
}