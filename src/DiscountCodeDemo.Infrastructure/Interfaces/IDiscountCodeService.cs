using DiscountCodeDemo.Infrastructure.Dto;

namespace DiscountCodeDemo.Infrastructure.Interfaces;

public interface IDiscountCodeService
{
    Task<GenerateResponse> GenerateDiscountCodesAsync(ushort count, byte length);
}