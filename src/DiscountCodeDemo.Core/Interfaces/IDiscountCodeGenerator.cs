namespace DiscountCodeDemo.Core.Interfaces;

public interface IDiscountCodeGenerator
{
    IEnumerable<string> GenerateCodes(HashSet<string> existingCodes, ushort count, byte length);
}