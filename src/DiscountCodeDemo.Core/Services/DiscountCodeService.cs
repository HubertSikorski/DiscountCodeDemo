using DiscountCodeDemo.Infrastructure.Dto;
using DiscountCodeDemo.Infrastructure.Interfaces;

namespace DiscountCodeDemo.Core.Services;

public class DiscountCodeService : IDiscountCodeService
{
    private const byte MinLength = 7;
    private const byte MaxLength = 8;
    private readonly HashSet<string> _discountCodes = new HashSet<string>();
    public async Task<GenerateResponse> GenerateDiscountCodesAsync(ushort count, byte length)
    {
        var response = new GenerateResponse()
        {
            Result = true,
            Codes = new List<string>()
        };

        if (length is < MinLength or > MaxLength)
        {
            response.Result = false;
            return await Task.FromResult(response);
        }

        for (int i = 0; i < count; i++)
        {
            string code;

            do
            {
                code = GenerateRandomCode(length);
            } while (_discountCodes.Contains(code));
            
            _discountCodes.Add(code);
            response.Codes.Add(code);
        }

        return await Task.FromResult(response);
    }

    private string GenerateRandomCode(byte length)
    {
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        var random = new Random();
        var code = new string(Enumerable.Range(0, length)
            .Select(c => validChars[random.Next(validChars.Length)])
            .ToArray());

        return code;
    }
}