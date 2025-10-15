using DiscountCodeDemo.Core.Interfaces;
using DiscountCodeDemo.Core.Models;

namespace DiscountCodeDemo.Infrastructure.Repositories;

public class JsonFileDiscountCodeRepository : IDiscountCodeRepository
{
    private readonly string _jsonFilePath;
    private readonly List<DiscountCodeEntity> _cache;

    public JsonFileDiscountCodeRepository(string filePath)
    {
        _jsonFilePath = filePath;
        _cache = LoadFromFile();
    }
    
    public Task<IEnumerable<DiscountCodeEntity>> GetALlAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(string code)
    {
        throw new NotImplementedException();
    }

    public Task AddManyAsync(IEnumerable<DiscountCodeEntity> codeList)
    {
        throw new NotImplementedException();
    }

    public Task<DiscountCodeEntity?> GetByCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    public Task<bool> MarkCodeAsUsedAsync(string code)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
    
    private List<DiscountCodeEntity> LoadFromFile()
    {
        throw new NotImplementedException();
    }
}