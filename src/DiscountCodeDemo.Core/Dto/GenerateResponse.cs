namespace DiscountCodeDemo.Core.Dto;

public class GenerateResponse
{
    public bool Result { get; set; }
    public List<string> Codes { get; set; } = new List<string>(); }