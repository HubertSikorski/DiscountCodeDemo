using DiscountCodeDemo.Core.Services;

namespace DiscountCodeDemo.UnitTests;

public class DiscountCodeGeneratorTests
{
    private readonly DiscountCodeGenerator _codeGenerator;

    public DiscountCodeGeneratorTests()
    {
        _codeGenerator = new DiscountCodeGenerator();
    }

    [Fact]
    public void GenerateCodes_ReturnsRequestedAmountOfCodes()
    {
        //Arrange
        var existingCodes = new HashSet<string>();
        ushort count = 10;
        byte length = 7;
        
        //Act
        var result = _codeGenerator.GenerateCodes(existingCodes, count, length);
        
        //Assert
        Assert.Equal(count, result.Count());
    }

    [Fact]
    public void GenerateCodes_ReturnsUniqueCodes()
    {
        //Arrange
        var existingCodes = new HashSet<string>();
        ushort count = 100;
        byte length = 7;
        
        //Act
        var result = _codeGenerator.GenerateCodes(existingCodes, count, length);
        
        //Arrange
        Assert.Equal(result.Count(), result.Distinct().Count());
    }

    [Fact]
    public void GenerateCodes_ReturnsCorrectLengthCodes()
    {
        //Arrange
        var existingCodes = new HashSet<string>();
        ushort count = 100;
        byte length = 7;
        
        //Act
        var result = _codeGenerator.GenerateCodes(existingCodes, count, length);
        
        //Arrange
        Assert.All(result, code => Assert.Equal(length, code.Length));
    }
}