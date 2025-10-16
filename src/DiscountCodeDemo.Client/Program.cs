using DiscountCodeDemo.Client.Tcp;

const string host = "127.0.0.1";
const int port = 5000;

var client = new DiscountCodeClient(host, port);
bool exit = false;

while (!exit)
{
    Console.WriteLine("\nChoose an action:");
    Console.WriteLine("1. Generate discount codes");
    Console.WriteLine("2. Use discount code");
    Console.WriteLine("q. Quit");
    Console.Write("Your choice: ");
    string? choice = Console.ReadLine()?.Trim().ToLower();

    try
    {
        switch (choice)
        {
            case "1":
                await GenerateCodes(client);
                break;
            case "2":
                await UseCode(client);
                break;
            case "q":
                exit = true;
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static async Task GenerateCodes(DiscountCodeClient client)
{
    Console.Write("Enter number of codes (1–2000): ");
    if (!ushort.TryParse(Console.ReadLine(), out ushort count) || count == 0 || count > 2000)
    {
        Console.WriteLine("Invalid count.");
        return;
    }

    Console.Write("Enter code length (7 or 8): ");
    if (!byte.TryParse(Console.ReadLine(), out byte length) || (length != 7 && length != 8))
    {
        Console.WriteLine("Invalid length.");
        return;
    }

    bool success = await client.GenerateAsync(count, length);
    Console.WriteLine(success ? "Codes generated." : "Failed to generate codes.");
}

static async Task UseCode(DiscountCodeClient client)
{
    Console.Write("Enter code to use: ");
    string? code = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(code))
    {
        Console.WriteLine("Code cannot be empty.");
        return;
    }

    bool success = await client.UseCodeAsync(code);
    Console.WriteLine(success ? "Code used successfully." : "Code use failed.");
}