using DiscountCodeDemo.Core.Dto;
using DiscountCodeDemo.Core.Interfaces;
using DiscountCodeDemo.Core.Models;

namespace DiscountCodeDemo.Core.Services;

public class DiscountCodeService : IDiscountCodeService
{
    private const byte MinLength = 7;
    private const byte MaxLength = 8;
    private const int MaxNumberOfCodes = 2000;
    private const int MaxGenerationAttempts = 10000;
    
    private readonly IDiscountCodeRepository _discountCodeRepository;

    public DiscountCodeService(IDiscountCodeRepository discountCodeRepository)
    {
        _discountCodeRepository = discountCodeRepository;
    }
    
    public async Task<GenerateResponse> GenerateDiscountCodesAsync(ushort count, byte length)
    {
        var response = new GenerateResponse()
        {
            Result = true,
            Codes = new List<string>()
        };

        if (length is < MinLength or > MaxLength || count == 0 || count > MaxNumberOfCodes)
        {
            response.Result = false;
            return await Task.FromResult(response);
        }
        
        var existingCodes = 
            (await _discountCodeRepository.GetAllAsync()).Select(c => c.Code).ToHashSet();

        var newCodes = new HashSet<string>();
        int attempts = 0;

        while (newCodes.Count < count && attempts < MaxGenerationAttempts)
        {
            var code = GenerateRandomCode(length);
            attempts++;

            if (!existingCodes.Contains(code) && existingCodes.Add(code))
            {
                newCodes.Add(code);
                response.Codes.Add(code);
            }
        }

        if (newCodes.Count == 0)
        {
            response.Result = false;
            return response;
        }

        var entities = newCodes.Select(c => new DiscountCodeEntity
        {
            Code = c,
            IsUsed = false
        });
        
        await _discountCodeRepository.AddManyAsync(entities);
        await _discountCodeRepository.SaveChangesAsync();

        return response;
    }

    public async Task<bool> UseCodesAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length < MinLength || code.Length > MaxLength)
            return false;

        var success = await _discountCodeRepository.MarkCodeAsUsedAsync(code);
        
        if(success)
            await _discountCodeRepository.SaveChangesAsync();
        
        return success;
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