using DiscountCodeDemo.Core.Interfaces;

namespace DiscountCodeDemo.Core.Services;

public class DiscountCodeValidator : IDiscountCodeValidator
{
    private const byte MinLength = 7;
    private const byte MaxLength = 8;
    private const int MaxNumberOfCodes = 2000;
    
    public string NormalizeCode(string? code) =>
        code?.Trim('\0', ' ', '\r', '\n') ?? string.Empty;

    public bool IsValidCode(string code)
    {
        return !string.IsNullOrWhiteSpace(code) &&
               code.Length >= MinLength && 
               code.Length <= MaxLength;
    }

    public bool IsValidRequest(ushort count, byte length)
    {
        return length >= MinLength &&
               length <= MaxLength &&
               count <= MaxNumberOfCodes;
    }
}