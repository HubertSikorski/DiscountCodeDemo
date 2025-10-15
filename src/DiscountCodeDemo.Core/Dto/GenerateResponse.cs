namespace DiscountCodeDemo.Core.Dto;

public class GenerateResponse
{
    public bool Result { get; set; }
    public List<String> Codes { get; set; } = new List<string>(); }