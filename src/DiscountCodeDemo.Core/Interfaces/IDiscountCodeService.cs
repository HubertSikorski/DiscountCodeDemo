using DiscountCodeDemo.Core.Dto;

namespace DiscountCodeDemo.Core.Interfaces;

public interface IDiscountCodeService
{
    Task<GenerateResponse> GenerateDiscountCodesAsync(ushort count, byte length);
    Task<bool> UseCodesAsync(string code);
}