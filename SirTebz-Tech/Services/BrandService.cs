using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Models.ViewModels;
using SirTebz_Tech.Repositories.Interfaces;
using SirTebz_Tech.Services.Interfaces;

namespace SirTebz_Tech.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _repo;
    public BrandService(IBrandRepository repo) => _repo = repo;

    public async Task<IEnumerable<Brand>> GetAllAsync() => await _repo.GetAllAsync();

    public async Task<BrandCreateEditViewModel?> GetForEditAsync(int? id = null)
    {
        if (!id.HasValue) return new BrandCreateEditViewModel { IsActive = true };
        var brand = await _repo.GetByIdAsync(id.Value);
        if (brand == null) return null;
        return new BrandCreateEditViewModel { Id = brand.Id, Name = brand.Name, LogoUrl = brand.LogoUrl, IsActive = brand.IsActive };
    }

    public async Task CreateAsync(BrandCreateEditViewModel model)
        => await _repo.CreateAsync(new Brand { Name = model.Name, LogoUrl = model.LogoUrl, IsActive = model.IsActive });

    public async Task UpdateAsync(BrandCreateEditViewModel model)
    {
        var brand = await _repo.GetByIdAsync(model.Id);
        if (brand == null) return;
        brand.Name = model.Name; brand.LogoUrl = model.LogoUrl; brand.IsActive = model.IsActive;
        await _repo.UpdateAsync(brand);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
}