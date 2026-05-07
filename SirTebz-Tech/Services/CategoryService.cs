using SirTebzTech.Models.Entities;
using SirTebzTech.Models.ViewModels;
using SirTebzTech.Repositories.Interfaces;
using SirTebzTech.Services.Interfaces;

namespace SirTebzTech.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;
    public CategoryService(ICategoryRepository repo) => _repo = repo;

    public async Task<IEnumerable<Category>> GetAllAsync() => await _repo.GetAllAsync();

    public async Task<CategoryCreateEditViewModel?> GetForEditAsync(int? id = null)
    {
        if (!id.HasValue) return new CategoryCreateEditViewModel { IsActive = true };
        var cat = await _repo.GetByIdAsync(id.Value);
        if (cat == null) return null;
        return new CategoryCreateEditViewModel
        {
            Id = cat.Id,
            Name = cat.Name,
            Description = cat.Description,
            IconClass = cat.IconClass,
            IsActive = cat.IsActive
        };
    }

    public async Task CreateAsync(CategoryCreateEditViewModel model)
        => await _repo.CreateAsync(new Category
        {
            Name = model.Name,
            Description = model.Description,
            IconClass = model.IconClass,
            IsActive = model.IsActive
        });

    public async Task UpdateAsync(CategoryCreateEditViewModel model)
    {
        var cat = await _repo.GetByIdAsync(model.Id);
        if (cat == null) return;
        cat.Name = model.Name; cat.Description = model.Description;
        cat.IconClass = model.IconClass; cat.IsActive = model.IsActive;
        await _repo.UpdateAsync(cat);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
}