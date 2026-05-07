using SirTebzTech.Models.Entities;

namespace SirTebzTech.Repositories.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetAllAsync();
    Task<Brand?> GetByIdAsync(int id);
    Task<Brand> CreateAsync(Brand brand);
    Task UpdateAsync(Brand brand);
    Task DeleteAsync(int id);
}