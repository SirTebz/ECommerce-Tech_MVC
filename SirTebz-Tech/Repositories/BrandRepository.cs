using Microsoft.EntityFrameworkCore;
using SirTebz_Tech.Data;
using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Repositories.Interfaces;

namespace SirTebz_Tech.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly ApplicationDbContext _context;
    public BrandRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Brand>> GetAllAsync()
        => await _context.Brands.Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync();

    public async Task<Brand?> GetByIdAsync(int id)
        => await _context.Brands.FindAsync(id);

    public async Task<Brand> CreateAsync(Brand brand)
    {
        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();
        return brand;
    }

    public async Task UpdateAsync(Brand brand)
    {
        _context.Brands.Update(brand);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var brand = await _context.Brands.FindAsync(id);
        if (brand != null) { brand.IsActive = false; await _context.SaveChangesAsync(); }
    }
}