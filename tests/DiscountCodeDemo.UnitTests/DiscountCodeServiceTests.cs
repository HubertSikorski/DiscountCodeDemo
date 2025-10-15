using DiscountCodeDemo.Core.Services;

namespace DiscountCodeDemo.UnitTests;

public class DiscountCodeServiceTests
{
    private readonly DiscountCodeService _service;

    public DiscountCodeServiceTests()
    {
            _service = new DiscountCodeService();
    }

    [Fact]
    public async Task GenerateDiscountCodeAsync_ShouldGenerateUniqueDiscountCodes()
    {
        //Act
        var result = await _service.GenerateDiscountCodesAsync(10, 8);
        
        //Arrange
        Assert.True(result.Result);
        Assert.Equal(10, result.Codes.Count);
        Assert.Equal(result.Codes.Distinct().Count(), result.Codes.Count);
    }
}