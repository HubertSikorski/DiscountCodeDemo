using DiscountCodeDemo.Core.Services;

namespace DiscountCodeDemo.UnitTests;

public class DiscountCodeValidatorTests
{
    private readonly DiscountCodeValidator _validator;

    public DiscountCodeValidatorTests()
    {
        _validator = new DiscountCodeValidator();
    }

    [Theory]
    [InlineData(" CODE123\n", "CODE123")]
    [InlineData(" \tABC123\r\n", "ABC123")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void NormalizeCode_RemovesIncorrectCharacters(string? code, string expectedResult)
    {
        //Act
        var result = _validator.NormalizeCode(code);
        
        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("CODE123", true)]
    [InlineData("CODE1234", true)]
    [InlineData("CODE12", false)]
    [InlineData("CODE12345", false)]
    [InlineData("", false)]
    [InlineData("       ", false)]
    public void IsValidCode_ReturnsExpectedResult(string code, bool expectedResult)
    {
        //Act
        var result = _validator.IsValidCode(code);
        
        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1, 7, true)]
    [InlineData(2000, 8, true)]
    [InlineData(0, 7, true)]
    [InlineData(2001, 7, false)]
    [InlineData(10, 6, false)]
    [InlineData(10, 9, false)]
    public void IsValidRequest_ReturnsExpectedResult(ushort count, byte length, bool expectedResult)
    {
        //Act
        var result = _validator.IsValidRequest(count, length);
        
        //Assert
        Assert.Equal(expectedResult, result);
    }
}