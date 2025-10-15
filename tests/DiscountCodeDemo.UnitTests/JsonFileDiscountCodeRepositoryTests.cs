using DiscountCodeDemo.Core.Models;
using DiscountCodeDemo.Infrastructure.Repositories;

namespace DiscountCodeDemo.UnitTests;

public class JsonFileDiscountCodeRepositoryTests
{
    [Fact]
    public async Task AddManyAsync_ShouldHandleConcurrentAccessWithoutDataLoss()
    {
        //Arange
        var jsonFilePath = Path.GetTempFileName();
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