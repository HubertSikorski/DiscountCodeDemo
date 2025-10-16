namespace DiscountCodeDemo.Core.Interfaces;

public interface IDiscountCodeValidator
{
    string NormalizeCode(string? code);
    bool IsValidCode(string code);
    bool IsValidRequest(ushort count, byte length);
}