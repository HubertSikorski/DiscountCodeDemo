using DiscountCodeDemo.Core.Interfaces;
using DiscountCodeDemo.Core.Models;
using DiscountCodeDemo.Core.Services;
using Moq;

namespace DiscountCodeDemo.UnitTests;

public class DiscountCodeServiceTests
{
    private readonly Mock<IDiscountCodeRepository> _repositoryMock;
    private readonly Mock<IDiscountCodeGenerator> _codeGeneratorMock;
    private readonly Mock<IDiscountCodeValidator> _validatorMock;
    
    private readonly DiscountCodeService _service;
    
    public DiscountCodeServiceTests()
    {
        _repositoryMock = new Mock<IDiscountCodeRepository>();
        _codeGeneratorMock = new Mock<IDiscountCodeGenerator>();
        _validatorMock = new Mock<IDiscountCodeValidator>();

        _service = new DiscountCodeService(
            _repositoryMock.Object,
            _codeGeneratorMock.Object,
            _validatorMock.Object);
    }
    
    [Fact]
    public async Task GenerateDiscountCodeAsync_ShouldGenerateUniqueDiscountCodes()
    {
        //Arrange
        ushort count = 10;
        byte length = 7;
        
        _validatorMock.Setup(v => v.IsValidRequest(count, length)).Returns(true);
        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<DiscountCodeEntity>());
        _codeGeneratorMock.Setup(g => g.GenerateCodes(It.IsAny<HashSet<string>>(), count, length))
            .Returns(Enumerable.Range(1, count).Select(i => $"CODE{i:D4}"));
        
        //Act
        var result = await _service.GenerateDiscountCodesAsync(count, length);
        
        //Arrange
        Assert.True(result.Result);
        Assert.Equal(count, result.Codes.Count);
        Assert.Equal(result.Codes.Distinct().Count(), result.Codes.Count);

        _repositoryMock.Verify(r => r.AddManyAsync((It.IsAny<IEnumerable<DiscountCodeEntity>>())), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task GenerateDiscountCodeAsync_ShouldHandleLargeNumberOfCodes()
    {
        //Arrange
        ushort count = 2000;
        byte length = 7;
        
        _validatorMock.Setup(v => v.IsValidRequest(count, length)).Returns(true);
        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<DiscountCodeEntity>());
        _codeGeneratorMock.Setup(g => g.GenerateCodes(It.IsAny<HashSet<string>>(), count, length))
            .Returns(Enumerable.Range(1, count).Select(i => $"CODE{i:D4}"));
        
        //Act
        var result = await _service.GenerateDiscountCodesAsync(count, length);
        
        //Assert
        Assert.Equal(count, result.Codes.Count);
    }
    
    [Fact]
    public async Task GenerateDiscountCodeAsync_ShouldReturnFalse_WhenCountIsZero()
    {
        //Arrange
        ushort count = 0;
        byte length = 7;
        
        _validatorMock.Setup(v => v.IsValidRequest(count, length)).Returns(false);
        
        //Act
        var result = await _service.GenerateDiscountCodesAsync(count, length);
        
        //Assert
        Assert.False(result.Result);
        Assert.Empty(result.Codes);
    }
    
    [Fact]
    public async Task GenerateDiscountCodeAsync_ShouldGenerateCodesWithCorrectLength()
    {
        //Arange
        ushort count = 10;
        byte length = 7;
        
        _validatorMock.Setup(v => v.IsValidRequest(count, length)).Returns(true);
        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<DiscountCodeEntity>());
        _codeGeneratorMock.Setup(g => g.GenerateCodes(It.IsAny<HashSet<string>>(), count, length))
            .Returns(Enumerable.Range(1, count).Select(i => new string('A', length)));
        
        //Act
        var result = await _service.GenerateDiscountCodesAsync(count, length);
        
        //Assert
        Assert.All(result.Codes, code => Assert.Equal(length, code.Length));
    }
    
    [Theory]
    [InlineData(6)]
    [InlineData(9)]
    public async Task GenerateDiscountCodeAsync_ShouldReturnFalse_WhenCodeLengthIsOutOfRange(byte length)
    {
        //Arange
        ushort count = 10;
        
        _validatorMock.Setup(v => v.IsValidRequest(count, length)).Returns(false);
        
        //Act
        var result = await _service.GenerateDiscountCodesAsync(count, length);
        
        //Assert
        Assert.False(result.Result);
        Assert.Empty(result.Codes);
    }
}