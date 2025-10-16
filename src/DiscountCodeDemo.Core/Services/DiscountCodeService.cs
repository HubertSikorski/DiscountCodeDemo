using DiscountCodeDemo.Core.Dto;
using DiscountCodeDemo.Core.Interfaces;
using DiscountCodeDemo.Core.Models;

namespace DiscountCodeDemo.Core.Services;

public class DiscountCodeService : IDiscountCodeService
{
    private readonly IDiscountCodeRepository _discountCodeRepository;
    private readonly IDiscountCodeGenerator _discountCodeGenerator;
    private readonly IDiscountCodeValidator _discountCodeValidator;

    public DiscountCodeService(
        IDiscountCodeRepository discountCodeRepository,
        IDiscountCodeGenerator discountCodeGenerator,
        IDiscountCodeValidator discountCodeValidator)
    {
        _discountCodeRepository = discountCodeRepository;
        _discountCodeGenerator = discountCodeGenerator;
        _discountCodeValidator = discountCodeValidator;
    }
    
    public async Task<GenerateResponse> GenerateDiscountCodesAsync(ushort count, byte length)
    {
        var response = new GenerateResponse()
        {
            Result = true,
            Codes = new List<string>()
        };

        if (!_discountCodeValidator.IsValidRequest(count, length))
        {
            response.Result = false;
            return await Task.FromResult(response);
        }
        
        var existingCodes = 
            (await _discountCodeRepository.GetAllAsync())
            .Select(c => c.Code)
            .ToHashSet();

        var newCodes = 
            _discountCodeGenerator.GenerateCodes(existingCodes, count, length).ToList();

        if (newCodes.Count == 0)
        {
            response.Result = false;
            return response;
        }

        var entities = newCodes.Select(code => new DiscountCodeEntity
        {
            Code = code,
            IsUsed = false
        });
        
        await _discountCodeRepository.AddManyAsync(entities);
        await _discountCodeRepository.SaveChangesAsync();
        
        response.Codes.AddRange(newCodes);
        return response;
    }

    public async Task<bool> UseCodesAsync(string code)
    {
        var normalizedCode  = _discountCodeValidator.NormalizeCode(code);
        
        if (!_discountCodeValidator.IsValidCode(normalizedCode))
            return false;

        var success = await _discountCodeRepository.MarkCodeAsUsedAsync(normalizedCode);
        
        if(success)
            await _discountCodeRepository.SaveChangesAsync();
        
        return success;
    }
}