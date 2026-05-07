using Microsoft.EntityFrameworkCore;
using SirTebz_Tech.Data;
using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Repositories.Interfaces;

namespace SirTebz_Tech.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    public CategoryRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Category>> GetAllAsync()
        => await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();

    public async Task<Category?> GetByIdAsync(int id)
        => await _context.Categories.FindAsync(id);

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var cat = await _context.Categories.FindAsync(id);
        if (cat != null) { cat.IsActive = false; await _context.SaveChangesAsync(); }
    }
}