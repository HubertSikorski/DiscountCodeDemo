using DiscountCodeDemo.Core.Services;

namespace DiscountCodeDemo.UnitTests;

public class DiscountCodeServiceTests
{
    // private readonly DiscountCodeService _service;
    //
    // public DiscountCodeServiceTests()
    // {
    //         _service = new DiscountCodeService();
    // }
    //
    // [Fact]
    // public async Task GenerateDiscountCodeAsync_ShouldGenerateUniqueDiscountCodes()
    // {
    //     //Act
    //     var result = await _service.GenerateDiscountCodesAsync(10, 8);
    //     
    //     //Arrange
    //     Assert.True(result.Result);
    //     Assert.Equal(10, result.Codes.Count);
    //     Assert.Equal(result.Codes.Distinct().Count(), result.Codes.Count);
    // }
    //
    // [Fact]
    // public async Task GenerateDiscountCodeAsync_ShouldHandleLargeNumberOfCodes()
    // {
    //     //Arrange
    //     ushort count = 2000;
    //     
    //     //Act
    //     var result = await _service.GenerateDiscountCodesAsync(count, 8);
    //     
    //     //Assert
    //     Assert.Equal(count, result.Codes.Count);
    // }
    //
    // [Fact]
    // public async Task GenerateDiscountCodeAsync_ShouldReturnEmptyList_WhenCountIsZero()
    // {
    //     //Act
    //     var result = await _service.GenerateDiscountCodesAsync(0, 8);
    //     
    //     //Assert
    //     Assert.True(result.Result);
    //     Assert.Empty(result.Codes);
    // }
    //
    // [Fact]
    // public async Task GenerateDiscountCodeAsync_ShouldGenerateCodesWithCorrectLength()
    // {
    //     //Arange
    //     byte codeLength = 7;
    //     
    //     //Act
    //     var result = await _service.GenerateDiscountCodesAsync(10, codeLength);
    //     
    //     //Assert
    //     Assert.All(result.Codes, code => Assert.Equal(codeLength, code.Length));
    // }
    //
    // [Theory]
    // [InlineData(6)]
    // [InlineData(9)]
    // public async Task GenerateDiscountCodeAsync_ShouldReturnFalse_WhenCodeLengthIsOutOfRange(byte lenght)
    // {
    //     //Act
    //     var result = await _service.GenerateDiscountCodesAsync(5, lenght);
    //     
    //     //Assert
    //     Assert.False(result.Result);
    //     Assert.Empty(result.Codes);
    // }
}