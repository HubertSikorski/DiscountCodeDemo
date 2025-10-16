using DiscountCodeDemo.Core.Models;
using DiscountCodeDemo.Infrastructure.Repositories;

namespace DiscountCodeDemo.UnitTests;

public class JsonFileDiscountCodeRepositoryTests
{
    private string CreateTempFilePath() => Path.GetTempFileName();

    private async Task<JsonFileDiscountCodeRepository> CreateRepository(string path)
    {
        var repository = new JsonFileDiscountCodeRepository(path);
        await Task.Delay(50);
        return repository;
    }

    [Fact]
    public async Task AssManyAsync_ShouldAddCodes()
    {
        //Arrange
        var jsonFilePath = CreateTempFilePath();
        var repository = await CreateRepository(jsonFilePath);

        var codes = new List<DiscountCodeEntity>
        {
            new() { Code = "ABC1234", IsUsed = false },
            new() { Code = "DEF1234", IsUsed = false },
        };
        
        //Act
        await repository.AddManyAsync(codes);
        await repository.SaveChangesAsync();
        
        var all = (await repository.GetAllAsync()).ToList();
        
        //Assert
        Assert.Equal(codes.Count, all.Count);
        Assert.Contains(all, c => c.Code == "ABC1234");
        Assert.Contains(all, c => c.Code == "DEF1234");
    }

    [Fact]
    public async Task MarkCodeAsUsed_ShouldMarkCodeAsUsed()
    {
        //Arrange
        var jsonFilePath = CreateTempFilePath();
        var repository = await CreateRepository(jsonFilePath);
        const string testCode = "ABC1234";
        
        var code = new DiscountCodeEntity { Code = testCode, IsUsed = false };
        
        await repository.AddManyAsync(new[] { code });
        await repository.SaveChangesAsync();
        
        //Act
        var result = await repository.MarkCodeAsUsedAsync(testCode);
        
        //Assert
        Assert.True(result);
        var updated = await repository.GetByCodeAsync(testCode);
        Assert.True(updated!.IsUsed);
        
        File.Delete(CreateTempFilePath());
    }
    
    [Fact]
    public async Task AddManyAsync_ShouldHandleConcurrentAccessWithoutDataLoss()
    {
        //Arange
        var jsonFilePath = CreateTempFilePath();
        var repository = new JsonFileDiscountCodeRepository(jsonFilePath);

        int totalTasks = 10;
        int codesPerTask = 100;
        
        var tasks = new List<Task>();

        for (int i = 0; i < totalTasks; i++)
        {
            int taskId = i;
            tasks.Add(Task.Run(async () =>
            {
                var codes = new List<DiscountCodeEntity>();

                for (int j = 0; j < codesPerTask; j++)
                {
                    codes.Add(new DiscountCodeEntity
                    {
                        Code = $"T{taskId}_CODE{j}",
                        IsUsed = false
                    });
                }
                
                await repository.AddManyAsync(codes);
            }));
        }
        
        //Act
        await Task.WhenAll(tasks);
        await repository.SaveChangesAsync();

        var allCodes = (await repository.GetAllAsync()).ToList();
        
        //Assert
        Assert.Equal(totalTasks * codesPerTask, allCodes.Count);
        var distinctCodes = allCodes.Select(c => c.Code).Distinct();
        Assert.Equal(allCodes.Count, distinctCodes.Count());
        
        File.Delete(jsonFilePath);
    }
}