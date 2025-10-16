using System.Text.Json;
using DiscountCodeDemo.Core.Interfaces;
using DiscountCodeDemo.Core.Models;

namespace DiscountCodeDemo.Infrastructure.Repositories;

public class JsonFileDiscountCodeRepository : IDiscountCodeRepository
{
    private readonly string _jsonFilePath;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    private List<DiscountCodeEntity>? _cache;
    private Task _initTask;
    
    public JsonFileDiscountCodeRepository(string filePath)
    {
        _jsonFilePath = filePath;
        _initTask = InitializeAsync();
    }
    
    public async Task<IEnumerable<DiscountCodeEntity>> GetAllAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            return _cache.Select(code => new DiscountCodeEntity
            {
                Code = code.Code,
                IsUsed = code.IsUsed
            }).ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> ExistsAsync(string code)
    {
        await _semaphore.WaitAsync();
        try
        {
            return _cache.Any(c => c.Code == code);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task AddManyAsync(IEnumerable<DiscountCodeEntity> codeList)
    {
        await _semaphore.WaitAsync();
        try
        {
            _cache.AddRange(codeList);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<DiscountCodeEntity?> GetByCodeAsync(string code)
    {
        await _semaphore.WaitAsync();
        try
        {
            var found = _cache.FirstOrDefault(c => c.Code == code);
            if(found is null)
                return null;

            return new DiscountCodeEntity
            {
                Code = found.Code,
                IsUsed = found.IsUsed
            };
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    public async Task<bool> MarkCodeAsUsedAsync(string code)
    {
        await _semaphore.WaitAsync();
        try
        {
            var found = _cache.FirstOrDefault(c => c.Code == code);
            if (found is null || found.IsUsed)
                return false;
            
            found.IsUsed = true;
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(_cache, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            
            await File.WriteAllTextAsync(_jsonFilePath, json);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task InitializeAsync()
    {
        _cache = await LoadFromFileAsync();
    }
    
    private async Task<List<DiscountCodeEntity>> LoadFromFileAsync()
    {
        if (!File.Exists(_jsonFilePath))
            return new List<DiscountCodeEntity>();
        
        var json = await File.ReadAllTextAsync(_jsonFilePath);
        
        if(string.IsNullOrWhiteSpace(json))
            return new List<DiscountCodeEntity>();
        
        return JsonSerializer.Deserialize<List<DiscountCodeEntity>>(json)
            ?? new List<DiscountCodeEntity>();
    }
}