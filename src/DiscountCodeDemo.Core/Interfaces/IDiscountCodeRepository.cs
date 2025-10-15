using DiscountCodeDemo.Core.Models;

namespace DiscountCodeDemo.Core.Interfaces;

public interface IDiscountCodeRepository
{
    Task<IEnumerable<DiscountCodeEntity>> GetAllAsync();
    Task<bool> ExistsAsync(string code);
    Task AddManyAsync(IEnumerable<DiscountCodeEntity> codeList);
    Task<DiscountCodeEntity?> GetByCodeAsync(string code);
    Task<bool> MarkCodeAsUsedAsync(string code);
    Task SaveChangesAsync();
}