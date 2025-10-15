namespace DiscountCodeDemo.Core.Models;

public class DiscountCodeEntity
{
    public string Code { get; set; } = string.Empty;
    public bool IsUsed { get; set; } = false;
}