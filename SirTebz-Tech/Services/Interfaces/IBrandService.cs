using SirTebzTech.Models.Entities;
using SirTebzTech.Models.ViewModels;

namespace SirTebzTech.Services.Interfaces;

public interface IBrandService
{
    Task<IEnumerable<Brand>> GetAllAsync();
    Task<BrandCreateEditViewModel?> GetForEditAsync(int? id = null);
    Task CreateAsync(BrandCreateEditViewModel model);
    Task UpdateAsync(BrandCreateEditViewModel model);
    Task DeleteAsync(int id);
}