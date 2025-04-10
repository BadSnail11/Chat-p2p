using System;
using System.Net;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: PeerChat.exe <ip-address> <username>");
            Console.WriteLine("Example for testing on single machine:");
            Console.WriteLine("  PeerChat.exe 127.0.0.1 Alice");
            Console.WriteLine("  PeerChat.exe 127.0.0.2 Bob");
            Console.WriteLine("  PeerChat.exe 127.0.0.3 Charlie");
            return;
        }

        if (!IPAddress.TryParse(args[0], out var ipAddress))
        {
            Console.WriteLine($"Invalid IP address: {args[0]}");
            return;
        }

        string username = args[1];
        var chatNode = new ChatNode(ipAddress, username);

        Console.WriteLine($"Chat node started as {username} on {ipAddress}. Type messages and press Enter to send.");
        Console.WriteLine("Type 'exit' to quit.");

        while (true)
        {
            string message = Console.ReadLine();
            if (message?.ToLower() == "exit")
            {
                chatNode.Stop();
                break;
            }

            if (!string.IsNullOrEmpty(message))
            {
                chatNode.BroadcastMessage(message);
            }
        }
    }
}