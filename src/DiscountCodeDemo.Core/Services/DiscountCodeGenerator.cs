using DiscountCodeDemo.Core.Interfaces;

namespace DiscountCodeDemo.Core.Services;

public class DiscountCodeGenerator : IDiscountCodeGenerator
{
    private const int MaxGenerationAttempts = 10000;
    private const string ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    public IEnumerable<string> GenerateCodes(HashSet<string> existingCodes, ushort count, byte length)
    {
        var attempts = 0;
        var random = new Random();
        var newCodes = new HashSet<string>();

        while (newCodes.Count < count && attempts < MaxGenerationAttempts)
        {
            var code = new string(Enumerable.Range(0, length)
                .Select(c => ValidChars[random.Next(ValidChars.Length)])
                .ToArray());

            if (!existingCodes.Contains(code) && existingCodes.Add(code))
            {
                newCodes.Add(code);
            }
            
            attempts++;
        }
        
        return newCodes;
    }
}