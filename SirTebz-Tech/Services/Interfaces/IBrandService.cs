using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Models.ViewModels;

namespace SirTebz_Tech.Services.Interfaces;

public interface IBrandService
{
    Task<IEnumerable<Brand>> GetAllAsync();
    Task<BrandCreateEditViewModel?> GetForEditAsync(int? id = null);
    Task CreateAsync(BrandCreateEditViewModel model);
    Task UpdateAsync(BrandCreateEditViewModel model);
    Task DeleteAsync(int id);
}