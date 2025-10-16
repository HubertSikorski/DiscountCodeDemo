using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeDemo.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const string host = "127.0.0.1";
            const int port = 5000;

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
                            await GenerateCodes(host, port);
                            break;
                        case "2":
                            await UseCode(host, port);
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
        }

        private static async Task GenerateCodes(string host, int port)
        {
            Console.Write("Enter the number of discount codes to generate (1-2000): ");
            if (!ushort.TryParse(Console.ReadLine(), out ushort count) || count == 0 || count > 2000)
            {
                Console.WriteLine("Invalid number of codes.");
                return;
            }
            

            Console.Write("Enter the length of discount codes (7 or 8): ");
            if (!byte.TryParse(Console.ReadLine(), out byte length) || (length != 7 && length != 8))
            {
                Console.WriteLine("Invalid code length.");
                return;
            }

            using var client = new TcpClient();
            await client.ConnectAsync(host, port);
            using NetworkStream stream = client.GetStream();

 
            byte[] buffer = new byte[4];
            buffer[0] = 0x01;
            buffer[1] = (byte)(count & 0xFF);
            buffer[2] = (byte)((count >> 8) & 0xFF);
            buffer[3] = length;

            await stream.WriteAsync(buffer, 0, buffer.Length);


            int response = await stream.ReadByteAsync();
            if (response == 0x01)
                Console.WriteLine("Discount codes generated successfully.");
            else
                Console.WriteLine("Failed to generate discount codes.");
        }

        private static async Task UseCode(string host, int port)
        {
            Console.Write("Enter discount code to use: ");
            string? code = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(code))
            {
                Console.WriteLine("Code cannot be empty.");
                return;
            }

            byte[] codeBytes = Encoding.ASCII.GetBytes(code);

            using var client = new TcpClient();
            await client.ConnectAsync(host, port);
            using NetworkStream stream = client.GetStream();


            byte[] buffer = new byte[1 + codeBytes.Length + 1];
            buffer[0] = 0x02;
            Array.Copy(codeBytes, 0, buffer, 1, codeBytes.Length);
            buffer[buffer.Length - 1] = 0x00; // terminator

            await stream.WriteAsync(buffer, 0, buffer.Length);


            int response = await stream.ReadByteAsync();
            if (response == 0x01)
                Console.WriteLine("Discount code used successfully.");
            else
                Console.WriteLine("Failed to use discount code.");
        }
    }

    static class NetworkStreamExtensions
    {
        public static async Task<int> ReadByteAsync(this NetworkStream stream)
        {
            var buffer = new byte[1];
            int read = await stream.ReadAsync(buffer, 0, 1);
            return read == 1 ? buffer[0] : -1;
        }
    }
}
