using DiscountCodeDemo.Infrastructure.Dto;
using DiscountCodeDemo.Infrastructure.Interfaces;

namespace DiscountCodeDemo.Core.Services;

public class DiscountCodeService : IDiscountCodeService
{
    public Task<GenerateResponse> GenerateDiscountCodesAsync(ushort count, byte length)
    {
        throw new NotImplementedException();
    }
}